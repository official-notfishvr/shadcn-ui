using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Button : BaseComponent
    {
        public Button(GUIHelper helper)
            : base(helper) { }

        public bool Draw(ButtonConfig config)
        {
            if (config == null)
                return false;

            GUIStyle buttonStyle = styleManager?.GetButtonStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.button;
            var options = BuildLayoutOptions(config, buttonStyle);

            bool prevEnabled = GUI.enabled;
            Color prevColor = GUI.color;

            if (config.IsDisabled)
                GUI.enabled = false;

            float opacity = Mathf.Clamp01(config.Opacity);
            if (opacity < 0.999f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * opacity);

            bool clicked = config.Icon?.Image != null ? DrawWithIcon(config, buttonStyle, options) : DrawBasic(config, buttonStyle, options);

            GUI.enabled = prevEnabled;
            GUI.color = prevColor;

            if (clicked && !config.IsDisabled)
                config.OnClick?.Invoke();

            return clicked && !config.IsDisabled;
        }

        private List<GUILayoutOption> BuildLayoutOptions(ButtonConfig config, GUIStyle style)
        {
            return ControlLayoutUtility.BuildLayoutOptions(config.LayoutOptions, style.fixedWidth, style.fixedHeight, expandWidth: style.fixedWidth <= 0f);
        }

        private bool DrawBasic(ButtonConfig config, GUIStyle style, List<GUILayoutOption> options)
        {
            var content = new UnityHelpers.GUIContent(config.Text ?? string.Empty);
            var rect = config.Rect.HasValue ? ControlLayoutUtility.ScaleRect(config.Rect.Value, guiHelper.uiScale) : ControlLayoutUtility.ReserveRect(content, style, options);
            bool hovered = rect.Contains(Event.current.mousePosition);
            return DrawButtonRect(rect, content, style, hovered && !config.IsDisabled);
        }

        private bool DrawWithIcon(ButtonConfig config, GUIStyle style, List<GUILayoutOption> options)
        {
            var icon = config.Icon;
            var content = new UnityHelpers.GUIContent(config.Text ?? string.Empty);
            var rect = config.Rect.HasValue ? ControlLayoutUtility.ScaleRect(config.Rect.Value, guiHelper.uiScale) : ControlLayoutUtility.ReserveRect(content, style, options);
            bool hovered = rect.Contains(Event.current.mousePosition) && !config.IsDisabled;
            bool clicked = DrawButtonRect(rect, UnityHelpers.GUIContent.none, style, hovered);
            DrawButtonIconContent(rect, style, config.Text ?? string.Empty, icon);
            return clicked;
        }

        private bool DrawButtonRect(Rect rect, UnityHelpers.GUIContent content, GUIStyle style, bool hovered)
        {
            return UnityHelpers.Button(rect, content, style);
        }

        private void DrawButtonIconContent(Rect rect, GUIStyle style, string text, IconConfig iconConfig)
        {
            if (iconConfig?.Image == null)
                return;
            ContentRenderUtility.DrawCenteredContent(rect, style, text, iconConfig.Image, iconConfig.Position, iconConfig.Size * guiHelper.uiScale, iconConfig.Spacing * guiHelper.uiScale);
        }
    }
}
