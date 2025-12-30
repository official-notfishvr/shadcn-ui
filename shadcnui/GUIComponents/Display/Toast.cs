using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    internal class ActiveToast
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ToastVariant Variant { get; set; }
        public float DurationMs { get; set; }
        public float ElapsedMs { get; set; }
        public Action OnAction { get; set; }
        public string ActionLabel { get; set; }
        public bool Dismissible { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool ShowProgressBar { get; set; }
        public bool ShowAccentBar { get; set; }
        public ToastPosition Position { get; set; }
        public ToastStackDirection StackDirection { get; set; }
        public float Margin { get; set; }
        public float Spacing { get; set; }
        public float BorderRadius { get; set; }
        public float Padding { get; set; }
        public float MaxWidth { get; set; }
        public float MinWidth { get; set; }
        public bool IsDismissing { get; set; }
        public bool IsPaused { get; set; }
        public bool EnablePauseOnHover { get; set; }
        public float HoverPauseDelay { get; set; }
        public bool EnableClickToDismiss { get; set; }

        public ActiveToast(ToastConfig config)
        {
            Id = config.Id;
            Title = config.Title;
            Description = config.Description;
            Variant = config.Variant;
            DurationMs = config.DurationMs;
            OnAction = config.OnAction;
            ActionLabel = config.ActionLabel;
            Dismissible = config.Dismissible;
            ElapsedMs = 0f;
            Height = 0f;
            Width = config.Width;
            X = 0f;
            Y = 0f;
            ShowProgressBar = config.ShowProgressBar;
            ShowAccentBar = config.ShowAccentBar;
            Position = config.Position;
            StackDirection = config.StackDirection;
            Margin = config.Margin;
            Spacing = config.Spacing;
            BorderRadius = config.BorderRadius;
            Padding = config.Padding;
            MaxWidth = config.MaxWidth;
            MinWidth = config.MinWidth;
            IsDismissing = false;
            IsPaused = false;
            EnablePauseOnHover = config.EnablePauseOnHover;
            HoverPauseDelay = config.HoverPauseDelay;
            EnableClickToDismiss = config.EnableClickToDismiss;
        }

        public bool IsExpired => !IsDismissing && DurationMs > 0 && ElapsedMs >= DurationMs;
        public float ProgressPercent => DurationMs > 0 ? Mathf.Clamp01(ElapsedMs / DurationMs) : 0f;
    }

    public class Toast : BaseComponent
    {
        private List<ActiveToast> _activeToasts = new List<ActiveToast>();
        private Dictionary<string, Texture2D> _cachedTextures = new Dictionary<string, Texture2D>();
        private float _lastDrawTime = 0f;
        private const float DRAW_WARNING_THRESHOLD = 0.1f;
        private string _hoveredToastId = null;
        private float _hoverStartTime = 0f;
        private Queue<ToastConfig> _toastQueue = new Queue<ToastConfig>();
        private Dictionary<string, float> _toastCreationTimes = new Dictionary<string, float>();

        public float GlobalDismissAnimationDuration { get; set; } = DesignTokens.Animation.DurationSlow;
        public bool GlobalEnablePauseOnHover { get; set; } = true;
        public int MaxConcurrentToasts { get; set; } = 5;
        public int GroupingTimeWindowMs { get; set; } = 500;
        public bool EnableGlobalGrouping { get; set; } = false;

        public Toast(GUIHelper helper)
            : base(helper) { }

        public void Show(ToastConfig config, bool skipQueue = false)
        {
            try
            {
                if (config == null)
                    return;
                if (string.IsNullOrEmpty(config.Id))
                    config.Id = Guid.NewGuid().ToString();
                if (!skipQueue && _activeToasts.Count >= MaxConcurrentToasts)
                {
                    _toastQueue.Enqueue(config);
                    return;
                }
                if (EnableGlobalGrouping && HasRecentToast(config.Title, config.Description))
                {
                    GUILogger.LogInfo($"Toast grouped: '{config.Title}' already shown recently", "Toast.Show");
                    return;
                }
                var activeToast = new ActiveToast(config);
                _activeToasts.Add(activeToast);
                _toastCreationTimes[config.Id] = Time.time;
                if (Time.time - _lastDrawTime > DRAW_WARNING_THRESHOLD)
                    GUILogger.LogWarning("Toast added but DrawToasts() hasn't been called recently. Make sure to call guiHelper.DrawToasts() in your OnGUI method to render toasts.", "Toast.Show");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Show", "Toast");
            }
        }

        private bool HasRecentToast(string title, string description)
        {
            foreach (var toast in _activeToasts)
            {
                if (toast.Title == title && toast.Description == description)
                {
                    if (_toastCreationTimes.TryGetValue(toast.Id, out float creationTime))
                    {
                        float elapsed = (Time.time - creationTime) * 1000f;
                        if (elapsed < GroupingTimeWindowMs)
                            return true;
                    }
                }
            }
            return false;
        }

        public void Dismiss(string id, bool animate = true)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return;
                var toast = _activeToasts.Find(t => t.Id == id);
                if (toast != null)
                {
                    if (animate)
                    {
                        toast.IsDismissing = true;
                        var animManager = guiHelper.GetAnimationManager();
                        animManager.FadeOut($"toast_dismiss_{id}", GlobalDismissAnimationDuration, EasingFunctions.EaseOutCubic);
                    }
                    else
                    {
                        _activeToasts.Remove(toast);
                        _toastCreationTimes.Remove(id);
                        ProcessQueuedToasts();
                    }
                }
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Dismiss", "Toast");
            }
        }

        private void ProcessQueuedToasts()
        {
            while (_toastQueue.Count > 0 && _activeToasts.Count < MaxConcurrentToasts)
            {
                var queuedConfig = _toastQueue.Dequeue();
                Show(queuedConfig, skipQueue: true);
            }
        }

        public void DismissAll(bool animate = true)
        {
            try
            {
                if (animate)
                {
                    foreach (var toast in _activeToasts)
                        Dismiss(toast.Id, true);
                }
                else
                {
                    _activeToasts.Clear();
                    _toastCreationTimes.Clear();
                }
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DismissAll", "Toast");
            }
        }

        public void PauseToast(string id)
        {
            try
            {
                var toast = _activeToasts.Find(t => t.Id == id);
                if (toast != null)
                    toast.IsPaused = true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "PauseToast", "Toast");
            }
        }

        public void ResumeToast(string id)
        {
            try
            {
                var toast = _activeToasts.Find(t => t.Id == id);
                if (toast != null)
                    toast.IsPaused = false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResumeToast", "Toast");
            }
        }

        public void Cleanup()
        {
            try
            {
                _activeToasts.Clear();
                _toastCreationTimes.Clear();
                _toastQueue.Clear();
                _cachedTextures.Clear();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Cleanup", "Toast");
            }
        }

        public void DrawToasts()
        {
            try
            {
                _lastDrawTime = Time.time;
                UpdateHoverState();
                UpdateClickState();
                var animManager = guiHelper.GetAnimationManager();
                for (int i = _activeToasts.Count - 1; i >= 0; i--)
                {
                    var t = _activeToasts[i];
                    if (!t.IsPaused)
                        t.ElapsedMs += Time.deltaTime * 1000f;
                    if (t.IsDismissing)
                    {
                        float dismissAlpha = animManager.GetFloat($"toast_dismiss_{t.Id}", 1f);
                        if (dismissAlpha <= 0f)
                        {
                            animManager.Remove($"toast_dismiss_{t.Id}");
                            _activeToasts.RemoveAt(i);
                            _toastCreationTimes.Remove(t.Id);
                            ProcessQueuedToasts();
                            continue;
                        }
                    }
                    else if (t.IsExpired)
                        Dismiss(t.Id, animate: true);
                }
                if (_activeToasts.Count == 0)
                    return;
                CalculateToastPositions();
                for (int i = 0; i < _activeToasts.Count; i++)
                    DrawToastItem(_activeToasts[i], i);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawToasts", "Toast");
            }
        }

        private void UpdateHoverState()
        {
            try
            {
                if (!GlobalEnablePauseOnHover)
                    return;
                Vector2 mousePos = Event.current != null ? Event.current.mousePosition : Input.mousePosition;
                string newHoveredId = GetToastAtPosition(mousePos);
                if (newHoveredId != _hoveredToastId)
                {
                    if (_hoveredToastId != null)
                        ResumeToast(_hoveredToastId);
                    _hoveredToastId = newHoveredId;
                    _hoverStartTime = Time.time;
                }
                if (_hoveredToastId != null)
                {
                    var toast = _activeToasts.Find(t => t.Id == _hoveredToastId);
                    if (toast != null && Time.time - _hoverStartTime > toast.HoverPauseDelay && !toast.IsPaused)
                        PauseToast(_hoveredToastId);
                }
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "UpdateHoverState", "Toast");
            }
        }

        private void UpdateClickState()
        {
            try
            {
                if (Event.current != null && Event.current.type == EventType.MouseDown)
                {
                    Vector2 mousePos = Event.current.mousePosition;
                    string clickedToastId = GetToastAtPosition(mousePos);
                    if (!string.IsNullOrEmpty(clickedToastId))
                    {
                        var toast = _activeToasts.Find(t => t.Id == clickedToastId);
                        if (toast != null && toast.EnableClickToDismiss)
                        {
                            Dismiss(clickedToastId);
                            Event.current.Use();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "UpdateClickState", "Toast");
            }
        }

        private string GetToastAtPosition(Vector2 position)
        {
            for (int i = _activeToasts.Count - 1; i >= 0; i--)
            {
                var toast = _activeToasts[i];
                Rect toastRect = new Rect(toast.X, toast.Y, toast.Width, toast.Height);
                if (toastRect.Contains(position))
                    return toast.Id;
            }
            return null;
        }

        private void CalculateToastPositions()
        {
            if (_activeToasts.Count == 0)
                return;
            var firstToast = _activeToasts[0];
            float scaledWidth = firstToast.Width * guiHelper.uiScale;
            float scaledMargin = firstToast.Margin * guiHelper.uiScale;
            float scaledSpacing = firstToast.Spacing * guiHelper.uiScale;
            float toastHeight = 90f * guiHelper.uiScale;
            for (int i = 0; i < _activeToasts.Count; i++)
            {
                _activeToasts[i].Width = scaledWidth;
                _activeToasts[i].Height = toastHeight;
            }
            float baseX = 0,
                baseY = 0;
            GetBasePosition(out baseX, out baseY, scaledWidth, scaledMargin, toastHeight, firstToast.Position, firstToast.StackDirection);
            for (int i = 0; i < _activeToasts.Count; i++)
            {
                switch (firstToast.StackDirection)
                {
                    case ToastStackDirection.Up:
                        _activeToasts[i].X = baseX;
                        _activeToasts[i].Y = baseY - (i * (toastHeight + scaledSpacing));
                        break;
                    case ToastStackDirection.Down:
                        _activeToasts[i].X = baseX;
                        _activeToasts[i].Y = baseY + (i * (toastHeight + scaledSpacing));
                        break;
                    case ToastStackDirection.Left:
                        _activeToasts[i].X = baseX - (i * (scaledWidth + scaledSpacing));
                        _activeToasts[i].Y = baseY;
                        break;
                    case ToastStackDirection.Right:
                        _activeToasts[i].X = baseX + (i * (scaledWidth + scaledSpacing));
                        _activeToasts[i].Y = baseY;
                        break;
                }
            }
        }

        private void GetBasePosition(out float x, out float y, float scaledWidth, float scaledMargin, float toastHeight, ToastPosition position, ToastStackDirection stackDirection)
        {
            x = 0;
            y = 0;
            switch (position)
            {
                case ToastPosition.TopLeft:
                    x = scaledMargin;
                    y = scaledMargin;
                    break;
                case ToastPosition.TopCenter:
                    x = (Screen.width - scaledWidth) / 2f;
                    y = scaledMargin;
                    break;
                case ToastPosition.TopRight:
                    x = Screen.width - scaledWidth - scaledMargin;
                    y = scaledMargin;
                    break;
                case ToastPosition.BottomLeft:
                    x = scaledMargin;
                    y = stackDirection == ToastStackDirection.Up ? Screen.height - scaledMargin - toastHeight : Screen.height - scaledMargin;
                    break;
                case ToastPosition.BottomCenter:
                    x = (Screen.width - scaledWidth) / 2f;
                    y = stackDirection == ToastStackDirection.Up ? Screen.height - scaledMargin - toastHeight : Screen.height - scaledMargin;
                    break;
                case ToastPosition.BottomRight:
                    x = Screen.width - scaledWidth - scaledMargin;
                    y = stackDirection == ToastStackDirection.Up ? Screen.height - scaledMargin - toastHeight : Screen.height - scaledMargin;
                    break;
                case ToastPosition.CenterLeft:
                    x = scaledMargin;
                    y = (Screen.height - toastHeight) / 2f;
                    break;
                case ToastPosition.Center:
                    x = (Screen.width - scaledWidth) / 2f;
                    y = (Screen.height - toastHeight) / 2f;
                    break;
                case ToastPosition.CenterRight:
                    x = Screen.width - scaledWidth - scaledMargin;
                    y = (Screen.height - toastHeight) / 2f;
                    break;
            }
        }

        private void DrawToastItem(ActiveToast toast, int index)
        {
            try
            {
                var styleManager = guiHelper.GetStyleManager();
                if (styleManager == null)
                    return;
                var animManager = guiHelper.GetAnimationManager();
                float dismissAlpha = toast.IsDismissing ? animManager.GetFloat($"toast_dismiss_{toast.Id}", 1f) : 1f;
                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * dismissAlpha);
                Rect toastRect = new Rect(toast.X, toast.Y, toast.Width, toast.Height);
                Color bgColor = styleManager.GetToastBackgroundColor(toast.Variant);
                Color textColor = styleManager.GetToastTextColor(toast.Variant);
                Color accentColor = styleManager.GetToastAccentColor(toast.Variant);
                int borderRadius = Mathf.RoundToInt(toast.BorderRadius * guiHelper.uiScale);
                string bgKey = $"toast_bg_{toast.Variant}_{Mathf.RoundToInt(toastRect.width)}_{Mathf.RoundToInt(toastRect.height)}_{borderRadius}";
                if (!_cachedTextures.ContainsKey(bgKey))
                    _cachedTextures[bgKey] = styleManager.CreateRoundedRectTexture(Mathf.RoundToInt(toastRect.width), Mathf.RoundToInt(toastRect.height), borderRadius, bgColor);
                GUIStyle bgStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                bgStyle.normal.background = _cachedTextures[bgKey];
                bgStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                bgStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                GUI.Box(toastRect, "", bgStyle);
                float accentWidth = toast.ShowAccentBar ? DesignTokens.Spacing.XS * guiHelper.uiScale : 0f;
                if (toast.ShowAccentBar)
                {
                    Rect accentRect = new Rect(toastRect.x, toastRect.y, accentWidth, toastRect.height);
                    string accentKey = $"toast_accent_{toast.Variant}";
                    if (!_cachedTextures.ContainsKey(accentKey))
                        _cachedTextures[accentKey] = styleManager.CreateSolidTexture(accentColor);
                    GUIStyle accentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                    accentStyle.normal.background = _cachedTextures[accentKey];
                    accentStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    accentStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    GUI.Box(accentRect, "", accentStyle);
                }
                GUI.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a * dismissAlpha);
                float contentPadding = toast.Padding * guiHelper.uiScale;
                Rect contentRect = new Rect(toastRect.x + contentPadding + accentWidth, toastRect.y + contentPadding, toastRect.width - contentPadding * 2 - accentWidth - (40 * guiHelper.uiScale), toastRect.height - contentPadding * 2);
                GUILayout.BeginArea(contentRect);
                if (!string.IsNullOrEmpty(toast.Title))
                {
                    GUIStyle titleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                    titleStyle.fontStyle = FontStyle.Bold;
                    titleStyle.wordWrap = true;
                    titleStyle.fontSize = Mathf.RoundToInt(14 * guiHelper.uiScale);
                    GUILayout.Label(toast.Title, titleStyle, GUILayout.ExpandHeight(false));
                }
                if (!string.IsNullOrEmpty(toast.Description))
                {
                    GUIStyle descStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                    descStyle.wordWrap = true;
                    descStyle.fontSize = Mathf.RoundToInt(12 * guiHelper.uiScale);
                    GUILayout.Label(toast.Description, descStyle, GUILayout.ExpandHeight(false));
                }
                if (!string.IsNullOrEmpty(toast.ActionLabel) && toast.OnAction != null)
                {
                    GUIStyle actionButtonStyle = styleManager.GetButtonStyle(ControlVariant.Secondary, ControlSize.Small);
                    if (GUILayout.Button(toast.ActionLabel, actionButtonStyle, GUILayout.Height(24 * guiHelper.uiScale)))
                    {
                        toast.OnAction();
                        Dismiss(toast.Id);
                    }
                }
                GUILayout.EndArea();
                if (toast.Dismissible)
                {
                    Rect closeButtonRect = new Rect(toastRect.x + toastRect.width - (32 * guiHelper.uiScale), toastRect.y + (8 * guiHelper.uiScale), 28 * guiHelper.uiScale, 28 * guiHelper.uiScale);
                    GUIStyle closeButtonStyle = styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Icon);
                    if (GUI.Button(closeButtonRect, "×", closeButtonStyle))
                        Dismiss(toast.Id);
                }
                if (toast.ShowProgressBar && toast.DurationMs > 0)
                {
                    float progressHeight = DesignTokens.Spacing.XXS * guiHelper.uiScale;
                    Rect progressBgRect = new Rect(toastRect.x, toastRect.y + toastRect.height - progressHeight, toastRect.width, progressHeight);
                    string progressBgKey = "toast_progress_bg";
                    if (!_cachedTextures.ContainsKey(progressBgKey))
                        _cachedTextures[progressBgKey] = styleManager.CreateSolidTexture(new Color(0, 0, 0, 0.2f));
                    GUIStyle progressBgStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                    progressBgStyle.normal.background = _cachedTextures[progressBgKey];
                    progressBgStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    progressBgStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    GUI.Box(progressBgRect, "", progressBgStyle);
                    float progressWidth = progressBgRect.width * (1f - toast.ProgressPercent);
                    Rect progressRect = new Rect(toastRect.x, toastRect.y + toastRect.height - progressHeight, progressWidth, progressHeight);
                    string progressKey = $"toast_progress_{toast.Variant}";
                    if (!_cachedTextures.ContainsKey(progressKey))
                        _cachedTextures[progressKey] = styleManager.CreateSolidTexture(accentColor);
                    GUIStyle progressStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                    progressStyle.normal.background = _cachedTextures[progressKey];
                    progressStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    progressStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    GUI.Box(progressRect, "", progressStyle);
                }
                GUI.color = originalColor;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawToastItem", "Toast");
            }
        }

        public int GetActiveToastCount() => _activeToasts.Count;

        public bool HasToast(string id) => _activeToasts.Exists(t => t.Id == id);

        public List<string> GetActiveToastIds()
        {
            var ids = new List<string>();
            foreach (var toast in _activeToasts)
                ids.Add(toast.Id);
            return ids;
        }
    }
}
