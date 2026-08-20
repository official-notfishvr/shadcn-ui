using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Toast : BaseComponent
    {
        private sealed class ToastItem
        {
            public ToastConfig Config;
            public float CreatedAt;
            public float PausedAt;
            public float PausedTotal;
            public bool Hovered;
            public Rect LastRect;
        }

        private readonly List<ToastItem> _toasts = new();

        public Toast(GUIHelper helper)
            : base(helper) { }

        public void Show(ToastConfig config)
        {
            if (config == null)
                return;

            if (string.IsNullOrEmpty(config.Id))
                config.Id = Guid.NewGuid().ToString();

            _toasts.Add(new ToastItem { Config = config, CreatedAt = Time.realtimeSinceStartup });
        }

        public void Dismiss(string id, bool animate = true)
        {
            _toasts.RemoveAll(t => t.Config?.Id == id);
        }

        public void DismissAll(bool animate = true)
        {
            _toasts.Clear();
        }

        public int GetActiveToastCount() => _toasts.Count;

        public void Cleanup() => _toasts.Clear();

        internal void DrawToasts()
        {
            if (_toasts.Count == 0)
                return;

            float now = Time.realtimeSinceStartup;
            var expired = new List<ToastItem>();

            foreach (var toast in _toasts)
            {
                if (toast.Config == null)
                {
                    expired.Add(toast);
                    continue;
                }

                if (toast.Config.DurationMs <= 0f)
                    continue;

                float elapsed = GetElapsedSeconds(toast, now);
                if (elapsed >= toast.Config.DurationMs / 1000f)
                    expired.Add(toast);
            }

            foreach (var toast in expired)
                _toasts.Remove(toast);

            if (_toasts.Count == 0)
                return;

            DrawStacks(now);
        }

        private void DrawStacks(float now)
        {
            var grouped = _toasts.GroupBy(t => t.Config.Position).ToDictionary(g => g.Key, g => g.ToList());
            foreach (var pair in grouped)
            {
                var list = pair.Value.OrderBy(t => t.CreatedAt).ToList();
                DrawStack(pair.Key, list, now);
            }
        }

        private void DrawStack(ToastPosition position, List<ToastItem> list, float now)
        {
            if (list.Count == 0)
                return;

            float margin = list[0].Config.Margin * guiHelper.uiScale;
            float spacing = list[0].Config.Spacing * guiHelper.uiScale;

            Vector2 cursor = GetStartPosition(position, list[0].Config, margin);

            foreach (var toast in list)
            {
                Vector2 size = MeasureToast(toast.Config);
                Rect rect = new Rect(cursor.x, cursor.y, size.x, size.y);

                toast.LastRect = rect;
                DrawToast(rect, toast, now);

                AdvanceCursor(ref cursor, position, toast.Config.StackDirection, size, spacing);
            }
        }

        private Vector2 GetStartPosition(ToastPosition position, ToastConfig cfg, float margin)
        {
            float width = MeasureToast(cfg).x;
            float height = MeasureToast(cfg).y;

            return position switch
            {
                ToastPosition.TopLeft => new Vector2(margin, margin),
                ToastPosition.TopCenter => new Vector2((Screen.width - width) / 2f, margin),
                ToastPosition.TopRight => new Vector2(Screen.width - width - margin, margin),
                ToastPosition.BottomLeft => new Vector2(margin, Screen.height - height - margin),
                ToastPosition.BottomCenter => new Vector2((Screen.width - width) / 2f, Screen.height - height - margin),
                ToastPosition.BottomRight => new Vector2(Screen.width - width - margin, Screen.height - height - margin),
                ToastPosition.CenterLeft => new Vector2(margin, (Screen.height - height) / 2f),
                ToastPosition.Center => new Vector2((Screen.width - width) / 2f, (Screen.height - height) / 2f),
                ToastPosition.CenterRight => new Vector2(Screen.width - width - margin, (Screen.height - height) / 2f),
                _ => new Vector2(Screen.width - width - margin, Screen.height - height - margin),
            };
        }

        private void AdvanceCursor(ref Vector2 cursor, ToastPosition position, ToastStackDirection direction, Vector2 size, float spacing)
        {
            if (direction == ToastStackDirection.Left)
                cursor.x -= size.x + spacing;
            else if (direction == ToastStackDirection.Right)
                cursor.x += size.x + spacing;
            else if (direction == ToastStackDirection.Up)
                cursor.y -= size.y + spacing;
            else
                cursor.y += size.y + spacing;
        }

        private Vector2 MeasureToast(ToastConfig cfg)
        {
            float width = Mathf.Clamp(cfg.Width * guiHelper.uiScale, cfg.MinWidth * guiHelper.uiScale, cfg.MaxWidth * guiHelper.uiScale);
            float height = cfg.MinHeight * guiHelper.uiScale;

            var titleStyle = styleManager?.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, cfg.Appearance) ?? GUI.skin.label;
            var descStyle = styleManager?.GetCardDescriptionStyle(ControlVariant.Default, ControlSize.Default, cfg.Appearance) ?? GUI.skin.label;

            float padding = cfg.Padding * guiHelper.uiScale;
            float accentWidth = cfg.ShowAccentBar ? 5f * guiHelper.uiScale : 0f;
            float closeAllowance = cfg.IsDismissible ? 30f * guiHelper.uiScale : 0f;
            float contentWidth = width - padding * 2 - accentWidth - closeAllowance;

            float titleHeight = string.IsNullOrEmpty(cfg.Title) ? 0f : titleStyle.CalcHeight(new GUIContent(cfg.Title), contentWidth);
            float descHeight = string.IsNullOrEmpty(cfg.Description) ? 0f : descStyle.CalcHeight(new GUIContent(cfg.Description), contentWidth);
            float actionHeight = !string.IsNullOrEmpty(cfg.ActionLabel) && cfg.OnAction != null ? 32f * guiHelper.uiScale : 0f;
            float eyebrowHeight = 16f * guiHelper.uiScale;
            float gapAfterEyebrow = (titleHeight > 0f || descHeight > 0f) ? 8f * guiHelper.uiScale : 0f;
            float titleGap = titleHeight > 0f && descHeight > 0f ? 6f * guiHelper.uiScale : 0f;
            float actionGap = actionHeight > 0f && (titleHeight > 0f || descHeight > 0f) ? 10f * guiHelper.uiScale : 0f;

            float total = padding * 2 + eyebrowHeight + gapAfterEyebrow + titleHeight + titleGap + descHeight + actionGap + actionHeight;
            height = Mathf.Max(height, total);

            return new Vector2(width, height);
        }

        private void DrawToast(Rect rect, ToastItem toast, float now)
        {
            var cfg = toast.Config;
            var theme = styleManager?.GetTheme();
            Color bg = styleManager?.GetToastBackgroundColor(cfg.Variant) ?? (theme?.Elevated ?? Color.black);
            Color accent = styleManager?.GetToastAccentColor(cfg.Variant) ?? (theme?.Accent ?? Color.white);
            Color textColor = styleManager?.GetToastTextColor(cfg.Variant) ?? Color.white;
            Color border = theme != null ? Color.Lerp(theme.Border, accent, 0.22f) : accent;
            Color muted = theme != null ? Color.Lerp(theme.Muted, textColor, 0.18f) : textColor;

            bool hovered = rect.Contains(Event.current.mousePosition);
            HandleHover(toast, hovered, now);

            SurfaceDrawUtility.DrawRoundedBorder(
                styleManager,
                rect,
                styleManager.GetScaledBorderRadius(cfg.BorderRadius),
                bg,
                border,
                1f,
                hovered ? DesignTokens.Effects.ShadowMedium : DesignTokens.Effects.ShadowLight,
                hovered ? styleManager.GetScaledSpacing(DesignTokens.Effects.ShadowBlurLG) : styleManager.GetScaledSpacing(DesignTokens.Effects.ShadowBlurMD),
                theme?.Shadow ?? new Color(0f, 0f, 0f, 0.28f)
            );

            if (cfg.ShowAccentBar)
            {
                Rect accentRect = new Rect(rect.x + 1f, rect.y + 1f, 5f * guiHelper.uiScale, rect.height - 2f);
                SurfaceDrawUtility.DrawRoundedFill(styleManager, accentRect, accent, styleManager.GetScaledBorderRadius(cfg.BorderRadius));
            }

            float padding = cfg.Padding * guiHelper.uiScale;
            float accentInset = cfg.ShowAccentBar ? 8f * guiHelper.uiScale : 0f;
            float closeAllowance = cfg.IsDismissible ? 30f * guiHelper.uiScale : 0f;
            Rect content = new Rect(rect.x + padding + accentInset, rect.y + padding, rect.width - padding * 2 - accentInset - closeAllowance, rect.height - padding * 2);

            var eyebrowStyle = new UnityHelpers.GUIStyle(styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small, cfg.Appearance) ?? GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Clip,
            };
            eyebrowStyle.normal.textColor = accent;

            var titleStyle = new UnityHelpers.GUIStyle(styleManager?.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, cfg.Appearance) ?? GUI.skin.label) { wordWrap = true, clipping = TextClipping.Clip };
            titleStyle.normal.textColor = textColor;

            var descStyle = new UnityHelpers.GUIStyle(styleManager?.GetCardDescriptionStyle(ControlVariant.Default, ControlSize.Default, cfg.Appearance) ?? GUI.skin.label) { wordWrap = true, clipping = TextClipping.Clip };
            descStyle.normal.textColor = muted;

            float y = content.y;
            string eyebrow = GetToastEyebrow(cfg.Variant);
            GUI.Label(new Rect(content.x, y, content.width, 16f * guiHelper.uiScale), eyebrow, eyebrowStyle);
            y += 16f * guiHelper.uiScale;

            if (!string.IsNullOrEmpty(cfg.Title) || !string.IsNullOrEmpty(cfg.Description))
                y += 8f * guiHelper.uiScale;

            if (!string.IsNullOrEmpty(cfg.Title))
            {
                float h = titleStyle.CalcHeight(new UnityHelpers.GUIContent(cfg.Title), content.width);
                GUI.Label(new Rect(content.x, y, content.width, h), cfg.Title, titleStyle);
                y += h + (!string.IsNullOrEmpty(cfg.Description) ? 6f * guiHelper.uiScale : 0f);
            }

            if (!string.IsNullOrEmpty(cfg.Description))
            {
                float h = descStyle.CalcHeight(new UnityHelpers.GUIContent(cfg.Description), content.width);
                GUI.Label(new Rect(content.x, y, content.width, h), cfg.Description, descStyle);
                y += h;
            }

            if (!string.IsNullOrEmpty(cfg.ActionLabel) && cfg.OnAction != null)
            {
                y += 10f * guiHelper.uiScale;
                var actionWidth = Mathf.Min(content.width, Mathf.Max(96f * guiHelper.uiScale, (styleManager?.GetButtonStyle(ControlVariant.Secondary, ControlSize.Small, cfg.Appearance) ?? GUI.skin.button).CalcSize(new GUIContent(cfg.ActionLabel)).x + 28f * guiHelper.uiScale));
                var actionButton = new Controls.Button(guiHelper);
                actionButton.Render(
                    new ButtonConfig
                    {
                        Rect = new Rect(content.x, y, actionWidth / guiHelper.uiScale, 32f),
                        Text = cfg.ActionLabel,
                        Variant = ControlVariant.Secondary,
                        Size = ControlSize.Small,
                        Appearance = cfg.Appearance,
                        OnClick = cfg.OnAction,
                    }
                );
            }

            if (cfg.IsDismissible)
            {
                var closeButton = new Controls.Button(guiHelper);
                closeButton.Render(
                    new ButtonConfig
                    {
                        Rect = new Rect((rect.xMax - padding - 22f * guiHelper.uiScale) / guiHelper.uiScale, (rect.y + padding - 2f * guiHelper.uiScale) / guiHelper.uiScale, 22f, 22f),
                        Text = "×",
                        Variant = ControlVariant.Ghost,
                        Size = ControlSize.Icon,
                        Appearance = cfg.Appearance,
                        OnClick = () => Dismiss(cfg.Id),
                    }
                );
            }

            if (cfg.ShowProgressBar && cfg.DurationMs > 0)
            {
                float elapsed = GetElapsedSeconds(toast, now);
                float t = Mathf.Clamp01(elapsed / (cfg.DurationMs / 1000f));
                Rect bar = ControlLayoutUtility.BottomAligned(new Rect(rect.x, rect.y, rect.width * (1f - t), rect.height), 3f * guiHelper.uiScale);
                SurfaceDrawUtility.DrawSolid(bar, accent);
            }

            if (cfg.EnableClickToDismiss && Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                Dismiss(cfg.Id);
                Event.current.Use();
            }
        }

        private string GetToastEyebrow(ToastVariant variant)
        {
            return variant switch
            {
                ToastVariant.Success => "SUCCESS",
                ToastVariant.Error => "ERROR",
                ToastVariant.Warning => "WARNING",
                ToastVariant.Info => "INFO",
                _ => "NOTICE",
            };
        }

        private void HandleHover(ToastItem toast, bool hovered, float now)
        {
            if (!toast.Config.EnablePauseOnHover)
                return;

            if (hovered)
            {
                if (!toast.Hovered)
                {
                    toast.Hovered = true;
                    toast.PausedAt = now;
                }
            }
            else if (toast.Hovered)
            {
                toast.Hovered = false;
                toast.PausedTotal += now - toast.PausedAt;
            }
        }

        private float GetElapsedSeconds(ToastItem toast, float now)
        {
            float paused = toast.PausedTotal;
            if (toast.Hovered)
                paused += now - toast.PausedAt;
            return now - toast.CreatedAt - paused;
        }
    }
}
