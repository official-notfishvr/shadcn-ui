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

        public void DrawToasts()
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

            var titleStyle = styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label;
            var descStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Default) ?? GUI.skin.label;

            float padding = cfg.Padding * guiHelper.uiScale;
            float contentWidth = width - padding * 2;

            float titleHeight = string.IsNullOrEmpty(cfg.Title) ? 0f : titleStyle.CalcHeight(new GUIContent(cfg.Title), contentWidth);
            float descHeight = string.IsNullOrEmpty(cfg.Description) ? 0f : descStyle.CalcHeight(new GUIContent(cfg.Description), contentWidth);

            float total = padding * 2 + titleHeight + descHeight;
            height = Mathf.Max(height, total + (string.IsNullOrEmpty(cfg.Description) ? 0f : 6f * guiHelper.uiScale));

            return new Vector2(width, height);
        }

        private void DrawToast(Rect rect, ToastItem toast, float now)
        {
            var cfg = toast.Config;
            var theme = styleManager?.GetTheme();
            Color bg = styleManager?.GetToastBackgroundColor(cfg.Variant) ?? (theme?.Elevated ?? Color.black);
            Color accent = styleManager?.GetToastAccentColor(cfg.Variant) ?? (theme?.Accent ?? Color.white);
            Color textColor = styleManager?.GetToastTextColor(cfg.Variant) ?? Color.white;

            bool hovered = rect.Contains(Event.current.mousePosition);
            HandleHover(toast, hovered, now);

            Color prev = GUI.color;
            GUI.color = bg;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = prev;

            if (cfg.ShowAccentBar)
            {
                Rect accentRect = new Rect(rect.x, rect.y, 4f * guiHelper.uiScale, rect.height);
                DrawSolid(accentRect, accent);
            }

            float padding = cfg.Padding * guiHelper.uiScale;
            Rect content = new Rect(rect.x + padding, rect.y + padding, rect.width - padding * 2, rect.height - padding * 2);

            var titleStyle = new GUIStyle(styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label) { normal = { textColor = textColor } };
            var descStyle = new GUIStyle(styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Default) ?? GUI.skin.label) { normal = { textColor = textColor }, wordWrap = true };

            float y = content.y;
            if (!string.IsNullOrEmpty(cfg.Title))
            {
                float h = titleStyle.CalcHeight(new GUIContent(cfg.Title), content.width);
                GUI.Label(new Rect(content.x, y, content.width, h), cfg.Title, titleStyle);
                y += h + 4f * guiHelper.uiScale;
            }

            if (!string.IsNullOrEmpty(cfg.Description))
            {
                float h = descStyle.CalcHeight(new GUIContent(cfg.Description), content.width);
                GUI.Label(new Rect(content.x, y, content.width, h), cfg.Description, descStyle);
                y += h + 6f * guiHelper.uiScale;
            }

            if (!string.IsNullOrEmpty(cfg.ActionLabel) && cfg.OnAction != null)
            {
                if (GUI.Button(new Rect(content.x, y, 90f * guiHelper.uiScale, 26f * guiHelper.uiScale), cfg.ActionLabel))
                    cfg.OnAction?.Invoke();
            }

            if (cfg.IsDismissible)
            {
                Rect closeRect = new Rect(rect.xMax - 22f * guiHelper.uiScale, rect.y + 6f * guiHelper.uiScale, 16f * guiHelper.uiScale, 16f * guiHelper.uiScale);
                if (GUI.Button(closeRect, "×"))
                    Dismiss(cfg.Id);
            }

            if (cfg.ShowProgressBar && cfg.DurationMs > 0)
            {
                float elapsed = GetElapsedSeconds(toast, now);
                float t = Mathf.Clamp01(elapsed / (cfg.DurationMs / 1000f));
                Rect bar = new Rect(rect.x, rect.yMax - 3f * guiHelper.uiScale, rect.width * (1f - t), 3f * guiHelper.uiScale);
                DrawSolid(bar, accent);
            }

            if (cfg.EnableClickToDismiss && Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                Dismiss(cfg.Id);
                Event.current.Use();
            }
        }

        private void DrawSolid(Rect rect, Color color)
        {
            Color prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = prev;
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
