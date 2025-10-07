using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Input
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;
        private static float horizontalPadding = 10f;

        public Input(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public void DrawLabel(string text, LabelVariant variant = LabelVariant.Default, int width = -1, bool disabled = false)
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

        public string DrawInput(string value, string placeholder = "", InputVariant variant = InputVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle inputStyle = styleManager?.GetInputStyle(variant, focused, disabled) ?? GUI.skin.textField;

            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue;

            if (width > 0)
                newValue = UnityHelpers.TextField(value ?? "", inputStyle, GUILayout.Width(width * guiHelper.uiScale), GUILayout.Height(36 * guiHelper.uiScale));
            else
                newValue = UnityHelpers.TextField(value ?? "", inputStyle, GUILayout.Height(36 * guiHelper.uiScale));

            GUI.color = originalColor;

            if (!disabled && newValue != value && onChange != null)
                onChange.Invoke(newValue);

            return disabled ? value : newValue;
        }

        public string DrawLabeledInput(string label, string value, string placeholder = "", InputVariant inputVariant = InputVariant.Default, LabelVariant labelVariant = LabelVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null)
        {
            DrawLabel(label, labelVariant, -1, disabled);
            layoutComponents.AddSpace(4);
            return DrawInput(value, placeholder, inputVariant, disabled, false, inputWidth, onChange);
        }

        public string DrawPasswordField(string value, string label = "", char maskChar = '*', InputVariant variant = InputVariant.Default, bool disabled = false, Action<string> onChange = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (!string.IsNullOrEmpty(label))
                    UnityHelpers.Label(label, GUI.skin.label, new GUILayoutOption[0]);

                return UnityHelpers.PasswordField(value ?? "", maskChar, GUI.skin.textField, new GUILayoutOption[0]);
            }

            if (!string.IsNullOrEmpty(label))
            {
                DrawLabel(label, LabelVariant.Default, -1, disabled);
                layoutComponents.AddSpace(4);
            }

            GUIStyle passwordStyle = styleManager.GetPasswordFieldStyle();

            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue = UnityHelpers.PasswordField(value ?? "", maskChar, passwordStyle, GUILayout.Height(36 * guiHelper.uiScale));

            GUI.color = originalColor;
            layoutComponents.AddSpace(10f);

            if (newValue != value && !disabled && onChange != null)
            {
                onChange.Invoke(newValue);
            }

            return disabled ? value : newValue;
        }

        public string DrawTextArea(string value, string label = "", string placeholder = "", int maxLength = 1000, float height = 60f, bool disabled = false, bool focused = false, Action<string> onChange = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (!string.IsNullOrEmpty(label))
                    UnityHelpers.Label(label, GUI.skin.label, new GUILayoutOption[0]);

                string result = UnityHelpers.TextArea(value ?? "", GUILayout.Height(height));
                if (result.Length > maxLength)
                    result = result.Substring(0, maxLength);
                return result;
            }

            if (!string.IsNullOrEmpty(label))
            {
                DrawLabel(label, LabelVariant.Default, -1, disabled);
                layoutComponents.AddSpace(4);
            }

            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(TextAreaVariant.Default, focused);

            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue = UnityHelpers.TextArea(value ?? "", maxLength, textAreaStyle, GUILayout.Height(height * guiHelper.uiScale));

            GUI.color = originalColor;
            layoutComponents.AddSpace(10f);

            if (newValue != value && !disabled && onChange != null)
            {
                onChange.Invoke(newValue);
            }

            return disabled ? value : newValue;
        }

        public void DrawSectionHeader(string title)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.AddSpace(10f * 0.5f);

            UnityHelpers.Label(title, styleManager?.sectionHeaderStyle ?? GUI.skin.label);
            layoutComponents.AddSpace(2f);
        }

        public void RenderLabel(string text, int width = -1)
        {
            DrawLabel(text, LabelVariant.Default, width, false);
        }

        public string DrawPasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            password = DrawPasswordField(password, label, maskChar, InputVariant.Default, false, null);
            return password;
        }

        public void DrawTextArea(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            text = DrawTextArea(text, label, "", maxLength, height, false, false, null);
        }

        public int DrawNumberInput(int value, string label = "", int minValue = int.MinValue, int maxValue = int.MaxValue, bool disabled = false, Action<int> onChange = null)
        {
            string stringValue = value.ToString();
            string newStringValue = DrawLabeledInput(label, stringValue, "0", InputVariant.Default, LabelVariant.Default, disabled, -1, null);

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
            string newStringValue = DrawLabeledInput(label, stringValue, "0.0", InputVariant.Default, LabelVariant.Default, disabled, -1, null);

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
    }
}
