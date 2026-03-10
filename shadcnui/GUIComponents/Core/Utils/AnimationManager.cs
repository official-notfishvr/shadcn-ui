using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Utils
{
    public enum AnimationType
    {
        Float,
        Color,
        Vector2,
    }

    internal sealed class AnimationState
    {
        public string Id;
        public AnimationType Type;
        public float StartFloat;
        public float CurrentFloat;
        public float TargetFloat;
        public Color StartColor;
        public Color CurrentColor;
        public Color TargetColor;
        public Vector2 StartVector;
        public Vector2 CurrentVector;
        public Vector2 TargetVector;
        public float Duration;
        public float Elapsed;
        public bool Paused;
        public bool Completed;
        public float CompletedAt;
        public Func<float, float> Easing;
        public Action OnComplete;
        public bool CompletionHandled;

        public float Progress => Duration <= 0f ? 1f : Mathf.Clamp01(Elapsed / Duration);
    }

    public static class EasingFunctions
    {
        public static float Linear(float t) => t;

        public static float EaseIn(float t) => t * t;

        public static float EaseOut(float t) => 1f - (1f - t) * (1f - t);

        public static float EaseInOut(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

        public static float EaseInQuad(float t) => t * t;

        public static float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);

        public static float EaseInOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

        public static float EaseInCubic(float t) => t * t * t;

        public static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);

        public static float EaseInOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }

    public sealed class AnimationManager
    {
        private readonly GUIHelper _guiHelper;
        private readonly shadcnui.GUIComponents.Layout.Layout _layout;
        private readonly Dictionary<string, AnimationState> _animations = new();
        private readonly List<string> _toRemove = new();
        private float _clock;
        private bool _rootGroupStarted;

        public float RetentionPeriod { get; set; } = 0.5f;
        public int PoolSize { get; set; } = 64;

        public AnimationManager(GUIHelper helper)
        {
            _guiHelper = helper;
            _layout = new shadcnui.GUIComponents.Layout.Layout(helper);
        }

        public void StartFloat(string id, float from, float to, float duration, Func<float, float> easing = null, Action onComplete = null)
        {
            var state = GetOrCreate(id, AnimationType.Float);
            state.StartFloat = state.Completed ? from : state.CurrentFloat;
            state.CurrentFloat = from;
            state.TargetFloat = to;
            Prepare(state, duration, easing, onComplete);

            if (duration <= 0f)
            {
                state.CurrentFloat = to;
                Complete(state);
            }
        }

        public float GetFloat(string id, float defaultValue = 0f)
        {
            return _animations.TryGetValue(id, out var state) && state.Type == AnimationType.Float ? state.CurrentFloat : defaultValue;
        }

        public void StartColor(string id, Color from, Color to, float duration, Func<float, float> easing = null, Action onComplete = null)
        {
            var state = GetOrCreate(id, AnimationType.Color);
            state.StartColor = from;
            state.CurrentColor = from;
            state.TargetColor = to;
            Prepare(state, duration, easing, onComplete);

            if (duration <= 0f)
            {
                state.CurrentColor = to;
                Complete(state);
            }
        }

        public Color GetColor(string id, Color defaultValue = default)
        {
            return _animations.TryGetValue(id, out var state) && state.Type == AnimationType.Color ? state.CurrentColor : defaultValue;
        }

        public void StartVector2(string id, Vector2 from, Vector2 to, float duration, Func<float, float> easing = null, Action onComplete = null)
        {
            var state = GetOrCreate(id, AnimationType.Vector2);
            state.StartVector = from;
            state.CurrentVector = from;
            state.TargetVector = to;
            Prepare(state, duration, easing, onComplete);

            if (duration <= 0f)
            {
                state.CurrentVector = to;
                Complete(state);
            }
        }

        public Vector2 GetVector2(string id, Vector2 defaultValue = default)
        {
            return _animations.TryGetValue(id, out var state) && state.Type == AnimationType.Vector2 ? state.CurrentVector : defaultValue;
        }

        public void Pause(string id)
        {
            if (_animations.TryGetValue(id, out var state))
                state.Paused = true;
        }

        public void Resume(string id)
        {
            if (_animations.TryGetValue(id, out var state))
                state.Paused = false;
        }

        public void Cancel(string id, bool snapToTarget = false)
        {
            if (!_animations.TryGetValue(id, out var state))
                return;

            if (snapToTarget)
            {
                switch (state.Type)
                {
                    case AnimationType.Float:
                        state.CurrentFloat = state.TargetFloat;
                        break;
                    case AnimationType.Color:
                        state.CurrentColor = state.TargetColor;
                        break;
                    case AnimationType.Vector2:
                        state.CurrentVector = state.TargetVector;
                        break;
                }
            }

            Complete(state, invokeCallback: false);
        }

        public void Remove(string id)
        {
            _animations.Remove(id);
        }

        public bool Exists(string id) => _animations.ContainsKey(id);

        public bool IsActive(string id) => _animations.TryGetValue(id, out var state) && !state.Completed && !state.Paused;

        public bool IsComplete(string id) => _animations.TryGetValue(id, out var state) && state.Completed;

        public float GetProgress(string id) => _animations.TryGetValue(id, out var state) ? state.Progress : 0f;

        public bool BeginGUI()
        {
            if (Event.current == null || Event.current.type == EventType.Repaint)
            {
                var delta = Time.unscaledDeltaTime > 0f ? Time.unscaledDeltaTime : Time.deltaTime;
                Update(delta);
            }

            if (Event.current == null || Event.current.type == EventType.Repaint)
                DrawBackground();

            BeginRootGroup();
            return true;
        }

        public void EndGUI()
        {
            if (!_rootGroupStarted)
                return;

            _layout.EndVerticalGroup();
            _rootGroupStarted = false;
        }

        public void Cleanup()
        {
            _animations.Clear();
            _toRemove.Clear();
        }

        public string Serialize(string id)
        {
            if (!_animations.TryGetValue(id, out var state))
                return null;

            return $"{state.Id}|{(int)state.Type}|{state.CurrentFloat}|{state.CurrentColor.r},{state.CurrentColor.g},{state.CurrentColor.b},{state.CurrentColor.a}|{state.CurrentVector.x},{state.CurrentVector.y}|{state.Duration}|{state.Elapsed}";
        }

        public void Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;
        }

        public void Update(float deltaTime)
        {
            if (deltaTime <= 0f)
                return;

            _clock += deltaTime;
            _toRemove.Clear();

            foreach (var pair in _animations)
            {
                var state = pair.Value;
                if (state.Paused || state.Completed)
                    continue;

                state.Elapsed += deltaTime;
                var t = state.Easing != null ? state.Easing(state.Progress) : state.Progress;

                switch (state.Type)
                {
                    case AnimationType.Float:
                        state.CurrentFloat = Mathf.LerpUnclamped(state.StartFloat, state.TargetFloat, t);
                        break;
                    case AnimationType.Color:
                        state.CurrentColor = Color.LerpUnclamped(state.StartColor, state.TargetColor, t);
                        break;
                    case AnimationType.Vector2:
                        state.CurrentVector = Vector2.LerpUnclamped(state.StartVector, state.TargetVector, t);
                        break;
                }

                if (state.Progress >= 1f)
                    Complete(state);
            }

            foreach (var pair in _animations)
            {
                if (pair.Value.Completed && _clock - pair.Value.CompletedAt > RetentionPeriod)
                    _toRemove.Add(pair.Key);
            }

            foreach (var id in _toRemove)
                _animations.Remove(id);
        }

        private AnimationState GetOrCreate(string id, AnimationType type)
        {
            if (!_animations.TryGetValue(id, out var state))
            {
                state = new AnimationState { Id = id, Type = type };
                _animations[id] = state;
            }

            state.Type = type;
            return state;
        }

        private void Prepare(AnimationState state, float duration, Func<float, float> easing, Action onComplete)
        {
            state.Duration = Mathf.Max(0f, duration);
            state.Elapsed = 0f;
            state.Paused = false;
            state.Completed = false;
            state.CompletedAt = 0f;
            state.Easing = easing ?? EasingFunctions.Linear;
            state.OnComplete = onComplete;
            state.CompletionHandled = false;
        }

        private void Complete(AnimationState state, bool invokeCallback = true)
        {
            state.Completed = true;
            state.Paused = false;
            state.CompletedAt = _clock;

            if (!invokeCallback || state.CompletionHandled || state.OnComplete == null)
                return;

            state.CompletionHandled = true;

            try
            {
                state.OnComplete();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(Complete), nameof(AnimationManager));
            }
        }

        private void DrawBackground()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                var previous = GUI.color;
                var background = theme?.BackgroundColor ?? theme?.Base ?? new Color(0.1f, 0.1f, 0.1f, 0.95f);
                GUI.color = background;
                GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = previous;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(DrawBackground), nameof(AnimationManager));
                GUI.color = Color.white;
            }
        }

        private void BeginRootGroup()
        {
            if (_rootGroupStarted)
                return;

            try
            {
                var style = _guiHelper.GetStyleManager()?.GetAnimatedBoxStyle() ?? GUI.skin.box;
                _layout.BeginVerticalGroup(style);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(BeginRootGroup), nameof(AnimationManager));
                GUILayout.BeginVertical();
            }

            _rootGroupStarted = true;
        }
    }

    public static class AnimationHelpers
    {
        public static void FadeIn(this AnimationManager manager, string id, float duration = 0.3f, Func<float, float> easing = null)
        {
            manager.StartFloat(id, 0f, 1f, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void FadeOut(this AnimationManager manager, string id, float duration = 0.3f, Func<float, float> easing = null)
        {
            manager.StartFloat(id, 1f, 0f, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void ScaleIn(this AnimationManager manager, string id, float duration = 0.2f, float fromScale = 0.96f, Func<float, float> easing = null)
        {
            manager.StartFloat(id, fromScale, 1f, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void ScaleOut(this AnimationManager manager, string id, float duration = 0.2f, float toScale = 0.96f, Func<float, float> easing = null)
        {
            manager.StartFloat(id, 1f, toScale, duration, easing ?? EasingFunctions.EaseInCubic);
        }

        public static void SlideIn(this AnimationManager manager, string id, Vector2 target, Vector2 offset, float duration = 0.3f, Func<float, float> easing = null)
        {
            manager.StartVector2(id, target + offset, target, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void SlideOut(this AnimationManager manager, string id, Vector2 current, Vector2 offset, float duration = 0.3f, Func<float, float> easing = null)
        {
            manager.StartVector2(id, current, current + offset, duration, easing ?? EasingFunctions.EaseInCubic);
        }
    }
}
