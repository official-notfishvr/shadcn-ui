using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Tooltip : BaseComponent
    {
        private const string TooltipLayerId = "shadcnui_tooltip";

        private Rect? _hoverRect;
        private string _hoverText;
        private TooltipConfig _hoverConfig;
        private float _hoverStartTime;
        private bool _mouseLeftFrame;

        public Tooltip(GUIHelper helper)
            : base(helper) { }

        public void RegisterHover(Rect rect, string text, TooltipConfig config = null)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
                return;

            Vector2 mouse = Event.current.mousePosition;
            if (!rect.Contains(mouse))
                return;

            Rect screenRect = PopupLayoutUtility.ToScreenRect(rect);

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

        public void WithTooltip(string text, Action draw)
        {
            draw?.Invoke();
            guiHelper.FlushAutoRenderBuilder();
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
                RegisterHover(GUILayoutUtility.GetLastRect(), text);
        }

        public void WithTooltip(string text, TooltipConfig config, Action draw)
        {
            draw?.Invoke();
            guiHelper.FlushAutoRenderBuilder();
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
                RegisterHover(GUILayoutUtility.GetLastRect(), text, config);
        }

        public T WithTooltip<T>(string text, Func<T> draw)
        {
            T result = draw != null ? draw() : default;
            guiHelper.FlushAutoRenderBuilder();
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
                RegisterHover(GUILayoutUtility.GetLastRect(), text);
            return result;
        }

        public T WithTooltip<T>(string text, TooltipConfig config, Func<T> draw)
        {
            T result = draw != null ? draw() : default;
            guiHelper.FlushAutoRenderBuilder();
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
                RegisterHover(GUILayoutUtility.GetLastRect(), text, config);
            return result;
        }

        internal void FlushAndDraw(Rect clipBounds)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (styleManager?.GetTheme() == null)
            {
                Clear();
                return;
            }

            DrawTooltipLayer(clipBounds);
        }

        private void Clear()
        {
            _hoverRect = null;
            _hoverText = null;
            _hoverConfig = null;
            _mouseLeftFrame = false;
            LayerManager.Instance.Close(TooltipLayerId);
        }

        private void DrawTooltipLayer(Rect clipBounds)
        {
            if (!_hoverRect.HasValue || string.IsNullOrEmpty(_hoverText))
                return;

            Vector2 mouseScreen = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            bool contains = _hoverRect.Value.Contains(mouseScreen);

            if (!contains)
            {
                if (_mouseLeftFrame)
                    Clear();
                else
                    _mouseLeftFrame = true;
                return;
            }

            _mouseLeftFrame = false;

            var cfg = _hoverConfig ?? new TooltipConfig();
            if (Time.realtimeSinceStartup - _hoverStartTime < cfg.HoverDelaySeconds)
                return;

            Vector2 size = MeasureTooltip(_hoverText, cfg);
            Vector2 pos = ComputePosition(clipBounds, size, cfg);

            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = TooltipLayerId,
                    OpenPosition = pos,
                    Width = size.x,
                    Height = size.y,
                    CloseOnClickOutside = false,
                    DrawChrome = false,
                    ShowOverlay = false,
                    ZIndex = 10000,
                    Content = () => DrawTooltipBox(new Rect(0, 0, size.x, size.y), _hoverText, cfg),
                }
            );
        }

        private Vector2 MeasureTooltip(string text, TooltipConfig config)
        {
            GUIStyle style = styleManager?.GetTooltipStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box;
            GUIContent content = new GUIContent(text);
            float padH = DesignTokens.Spacing.MD * guiHelper.uiScale;
            float padV = DesignTokens.Spacing.SM * guiHelper.uiScale;

            Vector2 size = style.CalcSize(content);
            float maxW = config.MaxWidth * guiHelper.uiScale;

            if (size.x > maxW - padH * 2)
            {
                GUIStyle wrap = new UnityHelpers.GUIStyle(style) { wordWrap = true };
                float height = wrap.CalcHeight(content, maxW - padH * 2);
                return new Vector2(maxW, height + padV * 2);
            }

            return new Vector2(size.x + padH * 2, size.y + padV * 2);
        }

        private Vector2 ComputePosition(Rect clipBounds, Vector2 size, TooltipConfig config)
        {
            Vector2 mouse = Event.current.mousePosition;
            float offset = config.MouseOffset * guiHelper.uiScale;

            float tx = mouse.x + offset;
            float ty = mouse.y - size.y - 8f;

            if (tx + size.x > clipBounds.xMax)
                tx = mouse.x - size.x - offset;
            if (ty < clipBounds.yMin)
                ty = mouse.y + offset;
            if (ty + size.y > clipBounds.yMax)
                ty = clipBounds.yMax - size.y - 8f;

            return new Vector2(tx, ty);
        }

        private void DrawTooltipBox(Rect rect, string text, TooltipConfig config)
        {
            GUIStyle style = styleManager?.GetTooltipStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box;
            GUIContent content = new GUIContent(text);
            GUI.Box(rect, content, style);
        }
    }
}
