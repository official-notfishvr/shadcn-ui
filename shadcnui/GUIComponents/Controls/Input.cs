using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Input : BaseComponent
    {
        public Input(GUIHelper helper)
            : base(helper) { }

        #region Config-based API

        public string DrawInput(InputConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle inputStyle = styleManager?.GetInputStyle(config.Variant, ControlSize.Default, config.Focused, config.Disabled) ?? GUI.skin.textField;

            var originalColor = GUI.color;
            ApplyDisabledColor(originalColor, config.Disabled);

            string newValue = config.Icon?.Image != null ? DrawInputWithIcon(config, inputStyle) : DrawBasicInput(config, inputStyle);

            GUI.color = originalColor;

            if (!config.Disabled && newValue != config.Value && config.OnChange != null)
                config.OnChange.Invoke(newValue);

            return config.Disabled ? config.Value : newValue;
        }

        public string DrawLabeledInput(InputConfig config)
        {
            DrawLabel(config.Label, config.LabelVariant, -1, config.Disabled);
            layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            return DrawInput(config);
        }

        public string DrawPasswordField(InputConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (!string.IsNullOrEmpty(config.Label))
            {
                DrawLabel(config.Label, ControlVariant.Default, -1, config.Disabled);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            return DrawPasswordFieldStyled(config, styleManager);
        }

        public string DrawTextArea(InputConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (!string.IsNullOrEmpty(config.Label))
            {
                DrawLabel(config.Label, ControlVariant.Default, -1, config.Disabled);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            return DrawTextAreaStyled(config, styleManager);
        }

        #endregion

        #region API

        public string DrawInput(string value, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            return DrawInput(
                new InputConfig
                {
                    Value = value,
                    Placeholder = placeholder,
                    Variant = variant,
                    Disabled = disabled,
                    Focused = focused,
                    Width = width,
                    OnChange = onChange,
                }
            );
        }

        public string DrawInput(string value, Texture2D icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            return DrawInput(
                new InputConfig
                {
                    Value = value,
                    Icon = icon != null ? new IconConfig(icon) : null,
                    Placeholder = placeholder,
                    Variant = variant,
                    Disabled = disabled,
                    Focused = focused,
                    Width = width,
                    OnChange = onChange,
                }
            );
        }

        public string DrawLabeledInput(string label, string value, string placeholder = "", ControlVariant inputVariant = ControlVariant.Default, ControlVariant labelVariant = ControlVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null)
        {
            return DrawLabeledInput(
                new InputConfig
                {
                    Label = label,
                    Value = value,
                    Placeholder = placeholder,
                    Variant = inputVariant,
                    LabelVariant = labelVariant,
                    Disabled = disabled,
                    Width = inputWidth,
                    OnChange = onChange,
                }
            );
        }

        public string DrawPasswordField(string value, string label = "", char maskChar = '*', ControlVariant variant = ControlVariant.Default, bool disabled = false, Action<string> onChange = null)
        {
            return DrawPasswordField(
                new InputConfig
                {
                    Value = value,
                    Label = label,
                    MaskChar = maskChar,
                    Variant = variant,
                    Disabled = disabled,
                    OnChange = onChange,
                }
            );
        }

        public string DrawPasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            password = DrawPasswordField(password, label, maskChar, ControlVariant.Default, false, null);
            return password;
        }

        public string DrawTextArea(string value, string label = "", string placeholder = "", int maxLength = 1000, float height = 60f, bool disabled = false, bool focused = false, Action<string> onChange = null)
        {
            return DrawTextArea(
                new InputConfig
                {
                    Value = value,
                    Label = label,
                    Placeholder = placeholder,
                    MaxLength = maxLength,
                    Height = height,
                    Disabled = disabled,
                    Focused = focused,
                    OnChange = onChange,
                }
            );
        }

        public void DrawTextArea(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            text = DrawTextArea(text, label, "", maxLength, height, false, false, null);
        }

        public void DrawLabel(string text, ControlVariant variant = ControlVariant.Default, int width = -1, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager?.GetLabelStyle(variant) ?? GUI.skin.label;

            var originalColor = GUI.color;
            ApplyDisabledColor(originalColor, disabled);

            if (width > 0)
                UnityHelpers.Label(text ?? "", labelStyle, GUILayout.Width(width));
            else
                UnityHelpers.Label(text ?? "", labelStyle);

            GUI.color = originalColor;
        }

        public void DrawSectionHeader(string title)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM * 0.5f);
            UnityHelpers.Label(title, styleManager?.GetSectionHeaderStyle() ?? GUI.skin.label);
            layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
        }

        public void RenderLabel(string text, int width = -1)
        {
            DrawLabel(text, ControlVariant.Default, width, false);
        }

        #endregion

        #region Private Methods

        private string DrawInputWithIcon(InputConfig config, GUIStyle inputStyle)
        {
            if (config.Icon.Position == IconPosition.Above || config.Icon.Position == IconPosition.Below)
            {
                layoutComponents.BeginVerticalGroup();

                if (config.Icon.Position == IconPosition.Above)
                {
                    RenderIcon(config.Icon);
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                }

                string value = DrawBasicInput(config, inputStyle);

                if (config.Icon.Position == IconPosition.Below)
                {
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                    RenderIcon(config.Icon);
                }

                layoutComponents.EndVerticalGroup();
                return value;
            }

            GUIStyle styledInput = new UnityHelpers.GUIStyle(inputStyle);
            float iconSize = config.Icon.Size * guiHelper.uiScale;
            float spacing = config.Icon.Spacing * guiHelper.uiScale;
            int paddingAddition = (int)(iconSize + spacing + DesignTokens.Spacing.XS * guiHelper.uiScale);

            if (config.Icon.Position == IconPosition.Left)
            {
                styledInput.padding.left += paddingAddition;
            }
            else if (config.Icon.Position == IconPosition.Right)
            {
                styledInput.padding.right += paddingAddition;
            }

            string newValue = DrawBasicInput(config, styledInput);
            Rect lastRect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint)
            {
                if (config.Icon.Position == IconPosition.Left)
                {
                    Rect iconRect = new Rect(lastRect.x + DesignTokens.Spacing.XS * guiHelper.uiScale, lastRect.y + (lastRect.height - iconSize) / 2, iconSize, iconSize);
                    RenderIcon(config.Icon, iconRect);
                }
                else if (config.Icon.Position == IconPosition.Right)
                {
                    Rect iconRect = new Rect(lastRect.x + lastRect.width - iconSize - DesignTokens.Spacing.XS * guiHelper.uiScale, lastRect.y + (lastRect.height - iconSize) / 2, iconSize, iconSize);
                    RenderIcon(config.Icon, iconRect);
                }
            }

            return newValue;
        }

        private string DrawBasicInput(InputConfig config, GUIStyle inputStyle)
        {
            if (config.Width > 0)
                return UnityHelpers.TextField(config.Value ?? "", inputStyle, GUILayout.Width(config.Width * guiHelper.uiScale), GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));
            else
                return UnityHelpers.TextField(config.Value ?? "", inputStyle, GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));
        }

        private string DrawPasswordFieldStyled(InputConfig config, StyleManager styleManager)
        {
            GUIStyle passwordStyle = styleManager.GetPasswordFieldStyle();
            var originalColor = GUI.color;
            ApplyDisabledColor(originalColor, config.Disabled);

            string newValue = UnityHelpers.PasswordField(config.Value ?? "", config.MaskChar, passwordStyle, GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));

            GUI.color = originalColor;
            layoutComponents.AddSpace(DesignTokens.Spacing.MD);

            if (newValue != config.Value && !config.Disabled && config.OnChange != null)
                config.OnChange.Invoke(newValue);

            return config.Disabled ? config.Value : newValue;
        }

        private string DrawTextAreaStyled(InputConfig config, StyleManager styleManager)
        {
            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(ControlVariant.Default, ControlSize.Default, config.Focused);
            var originalColor = GUI.color;
            ApplyDisabledColor(originalColor, config.Disabled);

            string newValue = UnityHelpers.TextArea(config.Value ?? "", config.MaxLength, textAreaStyle, GUILayout.Height(config.Height * guiHelper.uiScale));

            GUI.color = originalColor;
            layoutComponents.AddSpace(DesignTokens.Spacing.MD);

            if (newValue != config.Value && !config.Disabled && config.OnChange != null)
                config.OnChange.Invoke(newValue);

            return config.Disabled ? config.Value : newValue;
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        private void RenderIcon(IconConfig iconConfig, Rect rect)
        {
            if (iconConfig.Image != null)
            {
                GUI.DrawTexture(rect, iconConfig.Image, ScaleMode.ScaleToFit);
            }
        }

        private void ApplyDisabledColor(Color originalColor, bool disabled)
        {
            if (disabled)
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }

        #endregion
    }
}
