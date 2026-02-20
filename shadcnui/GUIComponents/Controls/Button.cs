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

        #region Config-based API
        public bool DrawButton(ButtonConfig config)
        {
            GUIStyle buttonStyle = styleManager?.GetButtonStyle(config.Variant, config.Size) ?? GUI.skin.button;
            var layoutOptions = BuildLayoutOptions(config, buttonStyle);

            ApplyDisabledState(config.Disabled);
            var originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * config.Opacity);

            bool clicked = config.Icon?.Image != null ? DrawButtonWithIcon(config, buttonStyle, layoutOptions) : DrawBasicButton(config, buttonStyle, layoutOptions);

            GUI.color = originalColor;
            GUI.enabled = true;

            if (clicked && !config.Disabled)
                config.OnClick?.Invoke();

            return clicked && !config.Disabled;
        }
        #endregion

        #region API
        public bool DrawButton(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            return DrawButton(
                new ButtonConfig(text)
                {
                    Variant = variant,
                    Size = size,
                    OnClick = onClick,
                    Disabled = disabled,
                    Opacity = opacity,
                    Options = options,
                }
            );
        }

        public bool DrawButton(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            return DrawButton(
                new ButtonConfig(text)
                {
                    Icon = icon != null ? new IconConfig(icon) : null,
                    Variant = variant,
                    Size = size,
                    OnClick = onClick,
                    Disabled = disabled,
                    Opacity = opacity,
                    Options = options,
                }
            );
        }

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            if (horizontal)
                layoutComponents.BeginHorizontalGroup();
            else
                layoutComponents.BeginVerticalGroup();

            drawButtons?.Invoke();

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            layoutComponents.AddSpace(spacing);
        }
        #endregion

        #region Private Methods
        private List<GUILayoutOption> BuildLayoutOptions(ButtonConfig config, GUIStyle buttonStyle)
        {
            var layoutOptions = new List<GUILayoutOption>(config.Options ?? Array.Empty<GUILayoutOption>());

            if (config.Size != ControlSize.Icon && config.Icon?.Image == null)
                layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (buttonStyle.fixedWidth > 0)
                layoutOptions.Add(GUILayout.Width(buttonStyle.fixedWidth));
            if (buttonStyle.fixedHeight > 0)
                layoutOptions.Add(GUILayout.Height(buttonStyle.fixedHeight));

            return layoutOptions;
        }

        private bool DrawButtonWithIcon(ButtonConfig config, GUIStyle buttonStyle, List<GUILayoutOption> layoutOptions)
        {
            var isHorizontal = config.Icon.Position == IconPosition.Left || config.Icon.Position == IconPosition.Right;

            if (isHorizontal)
                layoutComponents.BeginHorizontalGroup();
            else
                layoutComponents.BeginVerticalGroup();

            if (config.Icon.Position == IconPosition.Above)
            {
                RenderIcon(config.Icon, false);
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
            }

            if (config.Icon.Position == IconPosition.Left)
            {
                RenderIcon(config.Icon, false);
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
            }

            var content = new UnityHelpers.GUIContent(config.Text ?? "");
            var rect = GUILayoutUtility.GetRect(content, buttonStyle, layoutOptions.ToArray());
            bool isHovered = rect.Contains(Event.current.mousePosition);

            bool clicked = DrawButtonRect(rect, content, buttonStyle, isHovered && !config.Disabled);

            if (config.Icon.Position == IconPosition.Right)
            {
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                RenderIcon(config.Icon, isHovered && !config.Disabled);
            }

            if (config.Icon.Position == IconPosition.Below)
            {
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                RenderIcon(config.Icon, isHovered && !config.Disabled);
            }

            if (isHorizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            return clicked;
        }

        private bool DrawBasicButton(ButtonConfig config, GUIStyle buttonStyle, List<GUILayoutOption> layoutOptions)
        {
            var content = new UnityHelpers.GUIContent(config.Text ?? "");
            var rect = GUILayoutUtility.GetRect(content, buttonStyle, layoutOptions.ToArray());
            bool isHovered = rect.Contains(Event.current.mousePosition);
            return DrawButtonRect(rect, content, buttonStyle, isHovered && !config.Disabled);
        }

        private bool DrawButtonRect(Rect rect, UnityHelpers.GUIContent content, GUIStyle buttonStyle, bool isHovered)
        {
            if (isHovered)
            {
                var offsetRect = new Rect(rect.x, rect.y - (DesignTokens.Spacing.XXS * guiHelper.uiScale), rect.width, rect.height);
                return UnityHelpers.Button(offsetRect, content, buttonStyle);
            }
            return UnityHelpers.Button(rect, content, buttonStyle);
        }

        private void RenderIcon(IconConfig iconConfig, bool isHovered)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            float offset = isHovered ? (DesignTokens.Spacing.XXS * guiHelper.uiScale) : 0f;

            var rect = GUILayoutUtility.GetRect(scaledSize, scaledSize, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
            var offsetRect = new Rect(rect.x, rect.y - offset, rect.width, rect.height);

            GUI.DrawTexture(offsetRect, iconConfig.Image, ScaleMode.ScaleToFit);
        }

        private void ApplyDisabledState(bool disabled)
        {
            if (disabled)
                GUI.enabled = false;
        }
        #endregion
    }
}
