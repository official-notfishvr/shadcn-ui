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

            GUIStyle buttonStyle = styleManager?.GetButtonStyle(config.Variant, config.Size) ?? GUI.skin.button;
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
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());

            if (config.Size != ControlSize.Icon && config.Icon?.Image == null)
                options.Add(GUILayout.ExpandWidth(true));

            if (style.fixedWidth > 0)
                options.Add(GUILayout.Width(style.fixedWidth));
            if (style.fixedHeight > 0)
                options.Add(GUILayout.Height(style.fixedHeight));

            return options;
        }

        private bool DrawBasic(ButtonConfig config, GUIStyle style, List<GUILayoutOption> options)
        {
            var content = new UnityHelpers.GUIContent(config.Text ?? string.Empty);
            return UnityHelpers.Button(content, style, options.ToArray());
        }

        private bool DrawWithIcon(ButtonConfig config, GUIStyle style, List<GUILayoutOption> options)
        {
            var icon = config.Icon;
            bool horizontal = icon.Position == IconPosition.Left || icon.Position == IconPosition.Right;

            if (horizontal)
                layoutComponents.BeginHorizontalGroup();
            else
                layoutComponents.BeginVerticalGroup();

            if (icon.Position == IconPosition.Above)
            {
                RenderIcon(icon);
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
            }

            if (icon.Position == IconPosition.Left)
            {
                RenderIcon(icon);
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
            }

            bool clicked = DrawBasic(config, style, options);

            if (icon.Position == IconPosition.Right)
            {
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
                RenderIcon(icon);
            }

            if (icon.Position == IconPosition.Below)
            {
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
                RenderIcon(icon);
            }

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            return clicked;
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            if (iconConfig?.Image == null)
                return;

            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }
    }
}
