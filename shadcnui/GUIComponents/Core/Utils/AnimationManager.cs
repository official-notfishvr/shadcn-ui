using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core.Utils
{
    public enum AnimationType
    {
        Float,
        Color,
        Vector2,
    }

    internal class AnimationState
    {
        public string Id { get; set; }
        public AnimationType Type { get; set; }
        public float StartValue { get; set; }
        public float TargetValue { get; set; }
        public float CurrentValue { get; set; }
        public Color StartColor { get; set; }
        public Color TargetColor { get; set; }
        public Color CurrentColor { get; set; }
        public Vector2 StartVector { get; set; }
        public Vector2 TargetVector { get; set; }
        public Vector2 CurrentVector { get; set; }
        public float Duration { get; set; }
        public float ElapsedTime { get; set; }
        public bool IsPaused { get; set; }
        public bool IsComplete { get; set; }
        public float CompletedTime { get; set; }
        public Func<float, float> Easing { get; set; }
        public Action OnComplete { get; set; }
        public bool CallbackInvoked { get; set; }

        public float Progress => Duration > 0 ? Mathf.Clamp01(ElapsedTime / Duration) : 1f;

        public void Reset()
        {
            Id = null;
            Type = AnimationType.Float;
            StartValue = 0f;
            TargetValue = 0f;
            CurrentValue = 0f;
            StartColor = default;
            TargetColor = default;
            CurrentColor = default;
            StartVector = default;
            TargetVector = default;
            CurrentVector = default;
            Duration = 0f;
            ElapsedTime = 0f;
            IsPaused = false;
            IsComplete = false;
            CompletedTime = 0f;
            Easing = null;
            OnComplete = null;
            CallbackInvoked = false;
        }
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

    public class AnimationManager
    {
        private GUIHelper guiHelper;
        private shadcnui.GUIComponents.Layout.Layout layoutComponents;
        private bool _layoutGroupStarted = false;
        private Dictionary<string, AnimationState> _animations;
        private Queue<AnimationState> _statePool;
        private List<string> _toRemove;
        private float _currentTime;

        public float RetentionPeriod { get; set; } = 1f;
        public int PoolSize { get; set; } = 50;

        public AnimationManager(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new shadcnui.GUIComponents.Layout.Layout(helper);
            _animations = new Dictionary<string, AnimationState>();
            _statePool = new Queue<AnimationState>();
            _toRemove = new List<string>();
            for (int i = 0; i < PoolSize; i++)
                _statePool.Enqueue(new AnimationState());
        }

        private AnimationState GetPooledState()
        {
            if (_statePool.Count > 0)
            {
                var state = _statePool.Dequeue();
                state.Reset();
                return state;
            }
            return new AnimationState();
        }

        private void ReturnToPool(AnimationState state)
        {
            state.Reset();
            _statePool.Enqueue(state);
        }

        public void StartFloat(string id, float from, float to, float duration, Func<float, float> easing = null, Action onComplete = null)
        {
            if (string.IsNullOrEmpty(id))
                return;
            AnimationState state;
            if (_animations.TryGetValue(id, out state))
            {
                state.StartValue = state.CurrentValue;
                state.TargetValue = to;
                state.Duration = duration;
                state.ElapsedTime = 0f;
                state.IsPaused = false;
                state.IsComplete = false;
                state.Easing = easing ?? EasingFunctions.Linear;
                state.OnComplete = onComplete;
                state.CallbackInvoked = false;
            }
            else
            {
                state = GetPooledState();
                state.Id = id;
                state.Type = AnimationType.Float;
                state.StartValue = from;
                state.TargetValue = to;
                state.CurrentValue = from;
                state.Duration = duration;
                state.Easing = easing ?? EasingFunctions.Linear;
                state.OnComplete = onComplete;
                _animations[id] = state;
            }
            if (duration <= 0)
            {
                state.CurrentValue = to;
                state.IsComplete = true;
                state.CompletedTime = _currentTime;
            }
        }

        public float GetFloat(string id, float defaultValue = 0f)
        {
            if (_animations.TryGetValue(id, out var state) && state.Type == AnimationType.Float)
                return state.CurrentValue;
            return defaultValue;
        }

        public void StartColor(string id, Color from, Color to, float duration, Func<float, float> easing = null, Action onComplete = null)
        {
            if (string.IsNullOrEmpty(id))
                return;
            AnimationState state;
            if (_animations.TryGetValue(id, out state))
            {
                state.StartColor = state.CurrentColor;
                state.TargetColor = to;
                state.Duration = duration;
                state.ElapsedTime = 0f;
                state.IsPaused = false;
                state.IsComplete = false;
                state.Easing = easing ?? EasingFunctions.Linear;
                state.OnComplete = onComplete;
                state.CallbackInvoked = false;
            }
            else
            {
                state = GetPooledState();
                state.Id = id;
                state.Type = AnimationType.Color;
                state.StartColor = from;
                state.TargetColor = to;
                state.CurrentColor = from;
                state.Duration = duration;
                state.Easing = easing ?? EasingFunctions.Linear;
                state.OnComplete = onComplete;
                _animations[id] = state;
            }
            if (duration <= 0)
            {
                state.CurrentColor = to;
                state.IsComplete = true;
                state.CompletedTime = _currentTime;
            }
        }

        public Color GetColor(string id, Color defaultValue = default)
        {
            if (_animations.TryGetValue(id, out var state) && state.Type == AnimationType.Color)
                return state.CurrentColor;
            return defaultValue;
        }

        public void StartVector2(string id, Vector2 from, Vector2 to, float duration, Func<float, float> easing = null, Action onComplete = null)
        {
            if (string.IsNullOrEmpty(id))
                return;
            AnimationState state;
            if (_animations.TryGetValue(id, out state))
            {
                state.StartVector = state.CurrentVector;
                state.TargetVector = to;
                state.Duration = duration;
                state.ElapsedTime = 0f;
                state.IsPaused = false;
                state.IsComplete = false;
                state.Easing = easing ?? EasingFunctions.Linear;
                state.OnComplete = onComplete;
                state.CallbackInvoked = false;
            }
            else
            {
                state = GetPooledState();
                state.Id = id;
                state.Type = AnimationType.Vector2;
                state.StartVector = from;
                state.TargetVector = to;
                state.CurrentVector = from;
                state.Duration = duration;
                state.Easing = easing ?? EasingFunctions.Linear;
                state.OnComplete = onComplete;
                _animations[id] = state;
            }
            if (duration <= 0)
            {
                state.CurrentVector = to;
                state.IsComplete = true;
                state.CompletedTime = _currentTime;
            }
        }

        public Vector2 GetVector2(string id, Vector2 defaultValue = default)
        {
            if (_animations.TryGetValue(id, out var state) && state.Type == AnimationType.Vector2)
                return state.CurrentVector;
            return defaultValue;
        }

        public void Pause(string id)
        {
            if (_animations.TryGetValue(id, out var state))
                state.IsPaused = true;
        }

        public void Resume(string id)
        {
            if (_animations.TryGetValue(id, out var state))
                state.IsPaused = false;
        }

        public void Cancel(string id, bool snapToTarget = false)
        {
            if (_animations.TryGetValue(id, out var state))
            {
                if (snapToTarget)
                {
                    switch (state.Type)
                    {
                        case AnimationType.Float:
                            state.CurrentValue = state.TargetValue;
                            break;
                        case AnimationType.Color:
                            state.CurrentColor = state.TargetColor;
                            break;
                        case AnimationType.Vector2:
                            state.CurrentVector = state.TargetVector;
                            break;
                    }
                }
                state.IsComplete = true;
                state.CompletedTime = _currentTime;
                state.CallbackInvoked = true;
            }
        }

        public void Remove(string id)
        {
            if (_animations.TryGetValue(id, out var state))
            {
                _animations.Remove(id);
                ReturnToPool(state);
            }
        }

        public bool Exists(string id) => _animations.ContainsKey(id);

        public bool IsActive(string id) => _animations.TryGetValue(id, out var s) && !s.IsPaused && !s.IsComplete;

        public bool IsComplete(string id) => _animations.TryGetValue(id, out var s) && s.IsComplete;

        public float GetProgress(string id) => _animations.TryGetValue(id, out var s) ? s.Progress : 0f;

        public void Update(float deltaTime)
        {
            _currentTime += deltaTime;
            _toRemove.Clear();
            foreach (var kvp in _animations)
            {
                var state = kvp.Value;
                if (state.IsPaused || state.IsComplete)
                    continue;
                state.ElapsedTime += deltaTime;
                float t = state.Duration > 0 ? Mathf.Clamp01(state.ElapsedTime / state.Duration) : 1f;
                float easedT = state.Easing != null ? state.Easing(t) : t;
                switch (state.Type)
                {
                    case AnimationType.Float:
                        state.CurrentValue = state.StartValue + (state.TargetValue - state.StartValue) * easedT;
                        break;
                    case AnimationType.Color:
                        state.CurrentColor = new Color(
                            Mathf.Clamp01(state.StartColor.r + (state.TargetColor.r - state.StartColor.r) * easedT),
                            Mathf.Clamp01(state.StartColor.g + (state.TargetColor.g - state.StartColor.g) * easedT),
                            Mathf.Clamp01(state.StartColor.b + (state.TargetColor.b - state.StartColor.b) * easedT),
                            Mathf.Clamp01(state.StartColor.a + (state.TargetColor.a - state.StartColor.a) * easedT)
                        );
                        break;
                    case AnimationType.Vector2:
                        state.CurrentVector = new Vector2(state.StartVector.x + (state.TargetVector.x - state.StartVector.x) * easedT, state.StartVector.y + (state.TargetVector.y - state.StartVector.y) * easedT);
                        break;
                }
                if (t >= 1f)
                {
                    state.IsComplete = true;
                    state.CompletedTime = _currentTime;
                    if (!state.CallbackInvoked && state.OnComplete != null)
                    {
                        state.CallbackInvoked = true;
                        try
                        {
                            state.OnComplete();
                        }
                        catch (Exception ex)
                        {
                            GUILogger.LogException(ex, "OnComplete", "AnimationManager");
                        }
                    }
                }
            }
            foreach (var kvp in _animations)
            {
                var state = kvp.Value;
                if (state.IsComplete && (_currentTime - state.CompletedTime) > RetentionPeriod)
                    _toRemove.Add(kvp.Key);
            }
            foreach (var id in _toRemove)
                Remove(id);
        }

        public string Serialize(string id)
        {
            if (!_animations.TryGetValue(id, out var state))
                return null;
            return $"{state.Id}|{(int)state.Type}|{state.StartValue}|{state.TargetValue}|{state.CurrentValue}|{state.Duration}|{state.ElapsedTime}|{(state.IsPaused ? 1 : 0)}|{(state.IsComplete ? 1 : 0)}";
        }

        public void Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            try
            {
                var parts = data.Split('|');
                if (parts.Length < 9)
                {
                    GUILogger.LogWarning("Invalid animation data format", "AnimationManager.Deserialize");
                    return;
                }
                var state = GetPooledState();
                state.Id = parts[0];
                state.Type = (AnimationType)int.Parse(parts[1]);
                state.StartValue = float.Parse(parts[2]);
                state.TargetValue = float.Parse(parts[3]);
                state.CurrentValue = float.Parse(parts[4]);
                state.Duration = float.Parse(parts[5]);
                state.ElapsedTime = float.Parse(parts[6]);
                state.IsPaused = parts[7] == "1";
                state.IsComplete = parts[8] == "1";
                state.Easing = EasingFunctions.Linear;
                _animations[state.Id] = state;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Deserialize", "AnimationManager");
            }
        }

        public bool BeginGUI()
        {
            try
            {
                Update(Time.deltaTime);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginGUI.Update", "AnimationManager");
            }

            try
            {
                Color bg = ThemeManager.Instance?.CurrentTheme?.BackgroundColor ?? new Color(0.1f, 0.1f, 0.1f, 0.95f);
                GUI.color = bg;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginGUI.Background", "AnimationManager");
                GUI.color = Color.white;
            }

            try
            {
                var styleManager = guiHelper.GetStyleManager();
                var boxStyle = styleManager?.GetAnimatedBoxStyle() ?? GUI.skin.box;
#if IL2CPP_MELONLOADER_PRE57
                layoutComponents.BeginVerticalGroup(boxStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginVerticalGroup(boxStyle);
#endif
                _layoutGroupStarted = true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginGUI.Layout", "AnimationManager");
                GUILayout.BeginVertical();
                _layoutGroupStarted = true;
            }

            return true;
        }

        public void EndGUI()
        {
            if (_layoutGroupStarted)
            {
                layoutComponents.EndVerticalGroup();
                _layoutGroupStarted = false;
            }
            GUI.color = Color.white;
        }

        public void Cleanup()
        {
            foreach (var kvp in _animations)
                ReturnToPool(kvp.Value);
            _animations.Clear();
            _toRemove.Clear();
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

        public static void ScaleIn(this AnimationManager manager, string id, float duration = 0.2f, float fromScale = 0.95f, Func<float, float> easing = null)
        {
            manager.StartFloat(id, fromScale, 1f, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void ScaleOut(this AnimationManager manager, string id, float duration = 0.2f, float toScale = 0.95f, Func<float, float> easing = null)
        {
            manager.StartFloat(id, 1f, toScale, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void SlideIn(this AnimationManager manager, string id, Vector2 target, Vector2 offset, float duration = 0.3f, Func<float, float> easing = null)
        {
            manager.StartVector2(id, target + offset, target, duration, easing ?? EasingFunctions.EaseOutCubic);
        }

        public static void SlideOut(this AnimationManager manager, string id, Vector2 current, Vector2 offset, float duration = 0.3f, Func<float, float> easing = null)
        {
            manager.StartVector2(id, current, current + offset, duration, easing ?? EasingFunctions.EaseOutCubic);
        }
    }
}
