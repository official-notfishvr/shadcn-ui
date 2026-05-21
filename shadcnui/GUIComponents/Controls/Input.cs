using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Input : BaseComponent
    {
        private readonly HashSet<string> _autoFocused = new();

        public Input(GUIHelper helper)
            : base(helper) { }

        public string Render(InputConfig config)
        {
            if (config == null)
                return string.Empty;

            string id = ResolveId(config.Id, config.Label, config.Placeholder);
            string controlName = "input_" + id;

            if (!string.IsNullOrEmpty(config.Label))
            {
                DrawLabel(config);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            bool focused = GUI.GetNameOfFocusedControl() == controlName;
            if (config.AutoFocus && !_autoFocused.Contains(id))
            {
                GUI.FocusControl(controlName);
                _autoFocused.Add(id);
                focused = true;
            }

            GUIStyle inputStyle = styleManager?.GetInputStyle(config.Variant, config.Size, focused, config.IsDisabled, config.Appearance) ?? GUI.skin.textField;

            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            GUI.SetNextControlName(controlName);
            string value = DrawInputField(config, inputStyle, out GUIStyle renderedStyle);
            GUI.enabled = prevEnabled;

            DrawPlaceholderIfNeeded(config, renderedStyle, focused, value);
            DrawHelperOrError(config);

            if (!config.IsDisabled && value != config.Value)
                config.OnValueChanged?.Invoke(value);

            return config.IsDisabled ? (config.Value ?? string.Empty) : value;
        }

        private string DrawInputField(InputConfig config, GUIStyle inputStyle, out GUIStyle renderedStyle)
        {
            bool hasIcon = config.Icon?.Image != null;
            renderedStyle = hasIcon ? BuildStyleWithIconPadding(inputStyle, config.Icon) : new UnityHelpers.GUIStyle(inputStyle);
            renderedStyle.fixedHeight = ResolveHeight(config) * guiHelper.uiScale;
            renderedStyle.stretchHeight = false;

            string value = DrawField(config, renderedStyle);
            if (hasIcon && Event.current.type == EventType.Repaint)
                DrawIconOverlay(config.Icon, GUILayoutUtility.GetLastRect());

            return value;
        }

        private string DrawField(InputConfig config, GUIStyle style)
        {
            var options = BuildLayoutOptions(config);
            string current = config.Value ?? string.Empty;

            if (config.InputKind == InputKind.Password)
                return UnityHelpers.PasswordField(current, config.MaskCharacter, style, options.ToArray());

            if (config.MaxLength > 0)
                return UnityHelpers.TextField(current, config.MaxLength, style, options.ToArray());

            return UnityHelpers.TextField(current, style, options.ToArray());
        }

        private void DrawPlaceholderIfNeeded(InputConfig config, GUIStyle inputStyle, bool focused, string value)
        {
            if (focused || !string.IsNullOrEmpty(value) || string.IsNullOrEmpty(config.Placeholder) || Event.current.type != EventType.Repaint)
                return;

            Rect fieldRect = GUILayoutUtility.GetLastRect();
            var placeholderStyle = new UnityHelpers.GUIStyle(GUI.skin.label)
            {
                font = inputStyle.font,
                fontSize = inputStyle.fontSize,
                fontStyle = inputStyle.fontStyle,
                alignment = inputStyle.alignment,
            };
            placeholderStyle.normal.background = null;
            placeholderStyle.normal.textColor = styleManager?.GetTheme().Muted ?? new Color(0.55f, 0.55f, 0.60f, 1f);

            Rect textRect = new Rect(fieldRect.x + inputStyle.padding.left, fieldRect.y + inputStyle.padding.top, fieldRect.width - inputStyle.padding.horizontal, fieldRect.height - inputStyle.padding.vertical);
            GUI.Label(textRect, config.Placeholder, placeholderStyle);
        }

        private GUIStyle BuildStyleWithIconPadding(GUIStyle source, IconConfig icon)
        {
            var style = new UnityHelpers.GUIStyle(source);
            int padding = Mathf.RoundToInt(icon.Size * guiHelper.uiScale + icon.Spacing * guiHelper.uiScale);

            if (icon.Position == IconPosition.Left)
                style.padding.left += padding;
            else if (icon.Position == IconPosition.Right)
                style.padding.right += padding;

            return style;
        }

        private void DrawIconOverlay(IconConfig icon, Rect fieldRect)
        {
            float size = icon.Size * guiHelper.uiScale;
            float y = fieldRect.y + (fieldRect.height - size) * 0.5f;
            float xPad = DesignTokens.Spacing.SM * guiHelper.uiScale;
            Rect iconRect = icon.Position == IconPosition.Left ? new Rect(fieldRect.x + xPad, y, size, size) : new Rect(fieldRect.xMax - size - xPad, y, size, size);
            GUI.DrawTexture(iconRect, icon.Image, ScaleMode.ScaleToFit);
        }

        private void DrawLabel(InputConfig config)
        {
            GUIStyle labelStyle = styleManager?.GetLabelStyle(config.LabelVariant, config.Size, GetTextOnlyAppearance(config.Appearance)) ?? GUI.skin.label;
            UnityHelpers.Label(config.Label ?? string.Empty, labelStyle);
        }

        private void DrawHelperOrError(InputConfig config)
        {
            if (!string.IsNullOrEmpty(config.ErrorText))
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
                UnityHelpers.Label(config.ErrorText, styleManager?.GetLabelStyle(ControlVariant.Destructive, config.Size, GetTextOnlyAppearance(config.Appearance)) ?? GUI.skin.label);
                return;
            }

            if (!string.IsNullOrEmpty(config.HelperText))
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
                UnityHelpers.Label(config.HelperText, styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size, GetTextOnlyAppearance(config.Appearance)) ?? GUI.skin.label);
            }
        }

        private List<GUILayoutOption> BuildLayoutOptions(InputConfig config)
        {
            float width = config.Width > 0 ? config.Width * guiHelper.uiScale : 0f;
            float height = ResolveHeight(config) * guiHelper.uiScale;
            return ControlLayoutUtility.BuildLayoutOptions(config.LayoutOptions, width, height, expandWidth: width <= 0f);
        }

        private float ResolveHeight(InputConfig config)
        {
            if (config.Height > 0f && (config.Size == ControlSize.Default || !Mathf.Approximately(config.Height, DesignTokens.Height.Default)))
                return config.Height;

            return config.Size switch
            {
                ControlSize.Mini => DesignTokens.Height.Mini,
                ControlSize.Small => DesignTokens.Height.Small,
                ControlSize.Large => DesignTokens.Height.Large,
                _ => DesignTokens.Height.Default,
            };
        }

        private static ComponentAppearance GetTextOnlyAppearance(ComponentAppearance appearance)
        {
            if (appearance?.ForegroundColor == null)
                return null;

            return new ComponentAppearance { ForegroundColor = appearance.ForegroundColor };
        }

        private static string ResolveId(string id, string label, string placeholder)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            if (!string.IsNullOrEmpty(label))
                return label;
            if (!string.IsNullOrEmpty(placeholder))
                return placeholder;
            return Guid.NewGuid().ToString("N");
        }
    }
}
