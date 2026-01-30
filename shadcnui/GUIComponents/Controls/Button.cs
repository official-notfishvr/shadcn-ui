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

            var layoutOptions = new List<GUILayoutOption>(config.Options ?? Array.Empty<GUILayoutOption>());

            if (config.Size != ControlSize.Icon && config.Icon?.Image == null)
                layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (buttonStyle.fixedWidth > 0)
                layoutOptions.Add(GUILayout.Width(buttonStyle.fixedWidth));
            if (buttonStyle.fixedHeight > 0)
                layoutOptions.Add(GUILayout.Height(buttonStyle.fixedHeight));

            var wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            var originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * config.Opacity);

            bool isHovered = false;
            bool clicked = false;

            if (config.Icon?.Image != null)
            {
                if (config.Icon.Position == IconPosition.Left || config.Icon.Position == IconPosition.Right)
                {
                    layoutComponents.BeginHorizontalGroup();
                }
                else
                {
                    layoutComponents.BeginVerticalGroup();
                }

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
                isHovered = rect.Contains(Event.current.mousePosition);

                if (isHovered && !config.Disabled)
                {
                    var offsetRect = new Rect(rect.x, rect.y - (DesignTokens.Spacing.XXS * guiHelper.uiScale), rect.width, rect.height);
                    clicked = UnityHelpers.Button(offsetRect, content, buttonStyle);
                }
                else
                {
                    clicked = UnityHelpers.Button(rect, content, buttonStyle);
                }

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

                if (config.Icon.Position == IconPosition.Left || config.Icon.Position == IconPosition.Right)
                {
                    layoutComponents.EndHorizontalGroup();
                }
                else
                {
                    layoutComponents.EndVerticalGroup();
                }
            }
            else
            {
                var content = new UnityHelpers.GUIContent(config.Text ?? "");
                var rect = GUILayoutUtility.GetRect(content, buttonStyle, layoutOptions.ToArray());
                isHovered = rect.Contains(Event.current.mousePosition);

                if (isHovered && !config.Disabled)
                {
                    var offsetRect = new Rect(rect.x, rect.y - (DesignTokens.Spacing.XXS * guiHelper.uiScale), rect.width, rect.height);
                    clicked = UnityHelpers.Button(offsetRect, content, buttonStyle);
                }
                else
                {
                    clicked = UnityHelpers.Button(rect, content, buttonStyle);
                }
            }

            GUI.color = originalColor;
            GUI.enabled = wasEnabled;

            if (clicked && !config.Disabled)
                config.OnClick?.Invoke();

            return clicked && !config.Disabled;
        }

        private void RenderIcon(IconConfig iconConfig, bool isHovered)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            float offset = isHovered ? (DesignTokens.Spacing.XXS * guiHelper.uiScale) : 0f;

            var rect = GUILayoutUtility.GetRect(scaledSize, scaledSize, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
            var offsetRect = new Rect(rect.x, rect.y - offset, rect.width, rect.height);

            GUI.DrawTexture(offsetRect, iconConfig.Image, ScaleMode.ScaleToFit);
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
        #endregion

        #region Button Groups
        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            var scaledSpacing = spacing * guiHelper.uiScale;

            if (horizontal)
                layoutComponents.BeginHorizontalGroup();
            else
                layoutComponents.BeginVerticalGroup();

            drawButtons?.Invoke();

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            layoutComponents.AddSpace(scaledSpacing);
        }
        #endregion
    }
}
