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

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        #region Config-based API
        public string DrawInput(InputConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle inputStyle = styleManager?.GetInputStyle(config.Variant, ControlSize.Default, config.Focused, config.Disabled) ?? GUI.skin.textField;

            Color originalColor = GUI.color;
            if (config.Disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue;

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
                    RenderIcon(config.Icon);
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                }

                if (config.Icon.Position == IconPosition.Left)
                {
                    RenderIcon(config.Icon);
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                }
            }

            if (config.Width > 0)
                newValue = UnityHelpers.TextField(config.Value ?? "", inputStyle, GUILayout.Width(config.Width * guiHelper.uiScale), GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));
            else
                newValue = UnityHelpers.TextField(config.Value ?? "", inputStyle, GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));

            if (config.Icon?.Image != null)
            {
                if (config.Icon.Position == IconPosition.Right)
                {
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                    RenderIcon(config.Icon);
                }

                if (config.Icon.Position == IconPosition.Below)
                {
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                    RenderIcon(config.Icon);
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
            if (styleManager == null)
            {
                if (!string.IsNullOrEmpty(config.Label))
                    UnityHelpers.Label(config.Label, GUI.skin.label, new GUILayoutOption[0]);

                return UnityHelpers.PasswordField(config.Value ?? "", config.MaskChar, GUI.skin.textField, new GUILayoutOption[0]);
            }

            if (!string.IsNullOrEmpty(config.Label))
            {
                DrawLabel(config.Label, ControlVariant.Default, -1, config.Disabled);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            GUIStyle passwordStyle = styleManager.GetPasswordFieldStyle();

            Color originalColor = GUI.color;
            if (config.Disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue = UnityHelpers.PasswordField(config.Value ?? "", config.MaskChar, passwordStyle, GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));

            GUI.color = originalColor;
            layoutComponents.AddSpace(DesignTokens.Spacing.MD);

            if (newValue != config.Value && !config.Disabled && config.OnChange != null)
            {
                config.OnChange.Invoke(newValue);
            }

            return config.Disabled ? config.Value : newValue;
        }

        public string DrawTextArea(InputConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (!string.IsNullOrEmpty(config.Label))
                    UnityHelpers.Label(config.Label, GUI.skin.label, new GUILayoutOption[0]);

                string result = UnityHelpers.TextArea(config.Value ?? "", GUILayout.Height(config.Height));
                if (result.Length > config.MaxLength)
                    result = result.Substring(0, config.MaxLength);
                return result;
            }

            if (!string.IsNullOrEmpty(config.Label))
            {
                DrawLabel(config.Label, ControlVariant.Default, -1, config.Disabled);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(ControlVariant.Default, ControlSize.Default, config.Focused);

            Color originalColor = GUI.color;
            if (config.Disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue = UnityHelpers.TextArea(config.Value ?? "", config.MaxLength, textAreaStyle, GUILayout.Height(config.Height * guiHelper.uiScale));

            GUI.color = originalColor;
            layoutComponents.AddSpace(DesignTokens.Spacing.MD);

            if (newValue != config.Value && !config.Disabled && config.OnChange != null)
            {
                config.OnChange.Invoke(newValue);
            }

            return config.Disabled ? config.Value : newValue;
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
        #endregion

        #region Specialized Inputs
        public void DrawLabel(string text, ControlVariant variant = ControlVariant.Default, int width = -1, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager?.GetLabelStyle(variant) ?? GUI.skin.label;

            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
            }

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

        public int DrawNumberInput(int value, string label = "", int minValue = int.MinValue, int maxValue = int.MaxValue, bool disabled = false, Action<int> onChange = null)
        {
            string stringValue = value.ToString();
            string newStringValue = DrawLabeledInput(label, stringValue, "0", ControlVariant.Default, ControlVariant.Default, disabled, -1, null);

            if (int.TryParse(newStringValue, out int newValue))
            {
                newValue = Mathf.Clamp(newValue, minValue, maxValue);
                if (newValue != value && onChange != null)
                {
                    onChange.Invoke(newValue);
                }
                return newValue;
            }

            return value;
        }

        public float DrawFloatInput(float value, string label = "", float minValue = float.MinValue, float maxValue = float.MaxValue, bool disabled = false, Action<float> onChange = null)
        {
            string stringValue = value.ToString("F2");
            string newStringValue = DrawLabeledInput(label, stringValue, "0.0", ControlVariant.Default, ControlVariant.Default, disabled, -1, null);

            if (float.TryParse(newStringValue, out float newValue))
            {
                newValue = Mathf.Clamp(newValue, minValue, maxValue);
                if (Math.Abs(newValue - value) > 0.001f && onChange != null)
                {
                    onChange.Invoke(newValue);
                }
                return newValue;
            }

            return value;
        }
        #endregion
    }
}
