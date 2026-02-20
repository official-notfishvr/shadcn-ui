using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Tooltip : BaseComponent
    {
        private const string TooltipLayerId = "shadcnui_tooltip";

        private Rect? _hoverRect;
        private string _hoverText;
        private float _hoverStartTime;
        private TooltipConfig _hoverConfig;
        private bool _mouseLeftFrame;

        public Tooltip(GUIHelper helper)
            : base(helper) { }

        #region API

        public void RegisterHover(Rect rect, string text, TooltipConfig config = null)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
                return;
            Vector2 mouse = Event.current.mousePosition;
            if (rect.Contains(mouse))
            {
                Vector2 screenMin = GUIUtility.GUIToScreenPoint(rect.position);
                Rect screenRect = new Rect(screenMin.x, screenMin.y, rect.width, rect.height);
                bool isNew = !_hoverRect.HasValue || _hoverText != text;
                if (isNew)
                {
                    _hoverRect = screenRect;
                    _hoverText = text;
                    _hoverConfig = config;
                    _hoverStartTime = Time.realtimeSinceStartup;
                    _mouseLeftFrame = false;
                }
            }
        }

        public void WithTooltip(string text, Action draw)
        {
            draw?.Invoke();
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (rect.width > 0 && rect.height > 0)
                    RegisterHover(rect, text, null);
            }
        }

        public void WithTooltip(string text, TooltipConfig config, Action draw)
        {
            draw?.Invoke();
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (rect.width > 0 && rect.height > 0)
                    RegisterHover(rect, text, config);
            }
        }

        public T WithTooltip<T>(string text, Func<T> draw)
        {
            T result = draw != null ? draw() : default;
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (rect.width > 0 && rect.height > 0)
                    RegisterHover(rect, text, null);
            }
            return result;
        }

        public T WithTooltip<T>(string text, TooltipConfig config, Func<T> draw)
        {
            T result = draw != null ? draw() : default;
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (rect.width > 0 && rect.height > 0)
                    RegisterHover(rect, text, config);
            }
            return result;
        }

        public void FlushAndDraw(Rect clipBounds)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            if (styleManager?.GetTheme() == null)
            {
                ClearAndClose();
                return;
            }
            DrawTooltipLayer(clipBounds);
        }

        #endregion

        #region Private Methods

        private void ClearAndClose()
        {
            _hoverRect = null;
            _hoverText = null;
            _hoverConfig = null;
            _mouseLeftFrame = false;
            LayerManager.Instance.Close(TooltipLayerId);
        }

        private void DrawTooltipLayer(Rect clipBounds)
        {
            var cfg = _hoverConfig ?? new TooltipConfig();
            Vector2 mouseScreen = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            bool contains = _hoverRect.HasValue && _hoverRect.Value.Contains(mouseScreen);

            if (!contains)
            {
                if (_mouseLeftFrame)
                {
                    ClearAndClose();
                }
                else
                {
                    _mouseLeftFrame = true;
                }
                return;
            }

            _mouseLeftFrame = false;
            float elapsed = Time.realtimeSinceStartup - _hoverStartTime;
            bool shouldShow = !string.IsNullOrEmpty(_hoverText) && elapsed >= cfg.HoverDelaySeconds;

            if (!shouldShow)
                return;

            Vector2 pos = ComputeTooltipPosition(clipBounds, _hoverText, cfg);
            Vector2 size = ComputeTooltipSize(_hoverText, cfg);
            float scale = guiHelper?.uiScale ?? 1f;
            float shadowPad = cfg.ShadowOffset * scale;

            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = TooltipLayerId,
                    OpenPosition = pos,
                    Width = size.x + shadowPad,
                    Height = size.y + shadowPad,
                    ZIndex = 10000,
                    CloseOnClickOutside = false,
                    ShowOverlay = false,
                    DrawChrome = false,
                    Content = () => DrawTooltipContent(size, _hoverText, cfg),
                }
            );
        }

        private void DrawTooltipContent(Vector2 size, string text, TooltipConfig config)
        {
            DrawTooltipBox(new Rect(0, 0, size.x, size.y), new GUIContent(text), config);
        }

        private Vector2 ComputeTooltipSize(string text, TooltipConfig config)
        {
            GUIStyle tooltipStyle = styleManager.GetTooltipStyle();
            float scale = guiHelper?.uiScale ?? 1f;
            float padH = DesignTokens.Spacing.MD * scale;
            float padV = DesignTokens.Spacing.SM * scale;
            GUIContent content = new GUIContent(text);
            Vector2 size = tooltipStyle.CalcSize(content);
            float maxW = config.MaxWidth * scale;

            if (size.x > maxW - padH * 2)
            {
                GUIStyle wrapStyle = new UnityHelpers.GUIStyle(tooltipStyle) { wordWrap = true };
                float h = wrapStyle.CalcHeight(content, maxW - padH * 2);
                return new Vector2(maxW, h + padV * 2);
            }

            return new Vector2(size.x + padH * 2, size.y + padV * 2);
        }

        private Vector2 ComputeTooltipPosition(Rect clipBounds, string text, TooltipConfig config)
        {
            Vector2 size = ComputeTooltipSize(text, config);
            Vector2 mouse = Event.current.mousePosition;
            float offset = config.MouseOffset * (guiHelper?.uiScale ?? 1f);
            float verticalOffset = 16f;

            float tx = mouse.x + offset;
            float ty = mouse.y - size.y - 8;

            if (tx + size.x > clipBounds.xMax)
                tx = mouse.x - size.x - offset;

            if (ty < clipBounds.y)
                ty = mouse.y + verticalOffset;

            if (ty + size.y > clipBounds.yMax)
                ty = clipBounds.yMax - size.y - 8;

            return new Vector2(tx, ty);
        }

        private void DrawTooltipBox(Rect rect, GUIContent content, TooltipConfig config)
        {
            var theme = styleManager?.GetTheme();
            if (theme == null)
                return;

            float scale = guiHelper?.uiScale ?? 1f;
            float shadowOffset = 3f * scale;
            float borderWidth = 1f;

            Color prev = GUI.color;

            DrawDropShadow(rect, shadowOffset);

            GUIStyle tooltipStyle = styleManager.GetTooltipStyle();
            if (rect.width >= (config.MaxWidth - 20f) * scale)
                tooltipStyle = new UnityHelpers.GUIStyle(tooltipStyle) { wordWrap = true };

            GUI.Box(rect, content, tooltipStyle);

            DrawTooltipBorder(rect, theme, borderWidth);

            GUI.color = prev;
        }

        private void DrawDropShadow(Rect rect, float offset)
        {
            Color prevColor = GUI.color;

            for (int i = 0; i < 3; i++)
            {
                float shadowAlpha = Mathf.Lerp(0.15f, 0.05f, i / 3f);
                float shadowDistance = offset * (1f + i * 0.5f);

                Rect shadowRect = new Rect(rect.x + shadowDistance, rect.y + shadowDistance, rect.width, rect.height);

                GUI.color = new Color(0f, 0f, 0f, shadowAlpha);
                GUIStyle shadowStyle = new UnityHelpers.GUIStyle(GUI.skin.box) { normal = { background = Texture2D.whiteTexture }, border = new UnityHelpers.RectOffset(4, 4, 4, 4) };
                GUI.Box(shadowRect, GUIContent.none, shadowStyle);
            }

            GUI.color = prevColor;
        }

        private void DrawTooltipBorder(Rect rect, Theme theme, float borderWidth)
        {
            Color prev = GUI.color;
            GUI.color = theme.Border;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, borderWidth), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - borderWidth, rect.width, borderWidth), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, borderWidth, rect.height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - borderWidth, rect.y, borderWidth, rect.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }

        #endregion
    }
}
