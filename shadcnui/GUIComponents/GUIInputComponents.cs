using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIInputComponents
    {
        private GUIHelper guiHelper;
        private static float horizontalPadding = 10f;

        public GUIInputComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

       
        public void DrawLabel(string text, LabelVariant variant = LabelVariant.Default, int width = -1, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (width > 0)
                    GUILayout.Label(text, GUILayout.Width(width));
                else
                    GUILayout.Label(text);
                return;
            }

            GUIStyle labelStyle = styleManager.GetLabelStyle(variant);

           
            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
            }

            if (width > 0)
            {
#if IL2CPP
                GUILayout.Label(text ?? "", labelStyle, 
                    (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(width) });
#else
                GUILayout.Label(text ?? "", labelStyle, GUILayout.Width(width));
#endif
            }
            else
            {
#if IL2CPP
                GUILayout.Label(text ?? "", labelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(text ?? "", labelStyle);
#endif
            }

            GUI.color = originalColor;
        }

       
        public string DrawInput(string value, string placeholder = "", InputVariant variant = InputVariant.Default,
            bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (width > 0)
                    return GUILayout.TextField(value ?? "", GUILayout.Width(width));
                else
                    return GUILayout.TextField(value ?? "");
            }

            GUIStyle inputStyle = styleManager.GetInputStyle(variant, focused, disabled);

           
            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

            string newValue;
            if (width > 0)
            {
#if IL2CPP
                newValue = GUILayout.TextField(value ?? "", inputStyle, 
                    (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                        GUILayout.Width(width * guiHelper.uiScale),
                        GUILayout.Height(36 * guiHelper.uiScale) 
                    });
#else
                newValue = GUILayout.TextField(value ?? "", inputStyle,
                    GUILayout.Width(width * guiHelper.uiScale),
                    GUILayout.Height(36 * guiHelper.uiScale));
#endif
            }
            else
            {
#if IL2CPP
                newValue = GUILayout.TextField(value ?? "", inputStyle, 
                    (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                        GUILayout.Height(36 * guiHelper.uiScale) 
                    });
#else
                newValue = GUILayout.TextField(value ?? "", inputStyle,
                    GUILayout.Height(36 * guiHelper.uiScale));
#endif
            }

            GUI.color = originalColor;

           
            if (newValue != value && !disabled && onChange != null)
            {
                onChange.Invoke(newValue);
            }

            return disabled ? value : newValue;
        }

       
        public string DrawLabeledInput(string label, string value, string placeholder = "",
            InputVariant inputVariant = InputVariant.Default, LabelVariant labelVariant = LabelVariant.Default,
            bool disabled = false, int inputWidth = -1, Action<string> onChange = null)
        {
            DrawLabel(label, labelVariant, -1, disabled);
            GUILayout.Space(4);
            return DrawInput(value, placeholder, inputVariant, disabled, false, inputWidth, onChange);
        }

       
        public string DrawPasswordField(string value, string label = "", char maskChar = '*',
            InputVariant variant = InputVariant.Default, bool disabled = false, Action<string> onChange = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (!string.IsNullOrEmpty(label))
                    GUILayout.Label(label);
                return GUILayout.PasswordField(value ?? "", maskChar);
            }

            if (!string.IsNullOrEmpty(label))
            {
                DrawLabel(label, LabelVariant.Default, -1, disabled);
                GUILayout.Space(4);
            }

            GUIStyle passwordStyle = styleManager.GetPasswordFieldStyle();

           
            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

#if IL2CPP
            string newValue = GUILayout.PasswordField(value ?? "", maskChar, passwordStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Height(36 * guiHelper.uiScale) 
                });
#else
            string newValue = GUILayout.PasswordField(value ?? "", maskChar, passwordStyle,
                GUILayout.Height(36 * guiHelper.uiScale));
#endif

            GUI.color = originalColor;
            GUILayout.Space(guiHelper.controlSpacing);

           
            if (newValue != value && !disabled && onChange != null)
            {
                onChange.Invoke(newValue);
            }

            return disabled ? value : newValue;
        }

       
        public string DrawTextArea(string value, string label = "", string placeholder = "", int maxLength = 1000,
            float height = 60f, bool disabled = false, bool focused = false, Action<string> onChange = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                if (!string.IsNullOrEmpty(label))
                    GUILayout.Label(label);
                string result = GUILayout.TextArea(value ?? "", GUILayout.Height(height));
                if (result.Length > maxLength)
                    result = result.Substring(0, maxLength);
                return result;
            }

            if (!string.IsNullOrEmpty(label))
            {
                DrawLabel(label, LabelVariant.Default, -1, disabled);
                GUILayout.Space(4);
            }

            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(focused);

           
            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            }

#if IL2CPP
            string newValue = GUILayout.TextArea(value ?? "", maxLength, textAreaStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Height(height * guiHelper.uiScale) 
                });
#else
            string newValue = GUILayout.TextArea(value ?? "", maxLength, textAreaStyle,
                GUILayout.Height(height * guiHelper.uiScale));
#endif

            GUI.color = originalColor;
            GUILayout.Space(guiHelper.controlSpacing);

           
            if (newValue != value && !disabled && onChange != null)
            {
                onChange.Invoke(newValue);
            }

            return disabled ? value : newValue;
        }

       
        public void DrawSectionHeader(string title)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUILayout.Space(guiHelper.controlSpacing * 0.5f);
#if IL2CPP
            GUILayout.Label(title, styleManager?.sectionHeaderStyle ?? GUI.skin.label, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(title, styleManager?.sectionHeaderStyle ?? GUI.skin.label);
#endif
            guiHelper.DrawSeparator(300, guiHelper.borderEffectsEnabled ? 2f : 1f);
        }

        public void RenderLabel(string text, int width = -1)
        {
            DrawLabel(text, LabelVariant.Default, width, false);
        }

        public string RenderGlowInputField(string text, int fieldIndex, string placeholder, int width,
            float[] inputFieldGlow, int focusedField, float menuAlpha)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return GUILayout.TextField(text ?? placeholder ?? "", GUILayout.Width(width));
            }

#if IL2CPP
            Rect fieldRect = GUILayoutUtility.GetRect(width * guiHelper.uiScale, 25 * guiHelper.uiScale,
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(width * guiHelper.uiScale) });
#else
            Rect fieldRect = GUILayoutUtility.GetRect(width * guiHelper.uiScale, 25 * guiHelper.uiScale,
                GUILayout.Width(width * guiHelper.uiScale));
#endif

            if (guiHelper.glowEffectsEnabled && inputFieldGlow[fieldIndex] > 0.1f)
            {
                Color glowColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.5f, 0.8f, 1f);
                GUI.color = new Color(glowColor.r, glowColor.g, glowColor.b, inputFieldGlow[fieldIndex] * 0.4f * menuAlpha);
                GUI.DrawTexture(new Rect(fieldRect.x - 3, fieldRect.y - 2, fieldRect.width + 6, fieldRect.height + 4),
                    styleManager.GetGlowTexture());
            }

            GUI.color = new Color(1f, 1f, 1f, menuAlpha);

            GUI.SetNextControlName("input" + fieldIndex);
            string newText = GUI.TextField(fieldRect, text, styleManager.animatedInputStyle);

           
            if (fieldIndex > 0)
            {
                if (int.TryParse(newText, out int value))
                {
                    if (value >= 0 && value <= 255)
                        return newText;
                }
                else if (string.IsNullOrEmpty(newText))
                {
                    return "";
                }
                return text;
            }

            return newText;
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

       
        public string DrawEmailInput(string value, string label = "Email", bool disabled = false, Action<string> onChange = null)
        {
            return DrawLabeledInput(label, value, "Enter email address...", InputVariant.Default, LabelVariant.Default, disabled, -1, onChange);
        }

        public string DrawSearchInput(string value, string placeholder = "Search...", bool disabled = false, Action<string> onChange = null)
        {
            return DrawInput(value, placeholder, InputVariant.Outline, disabled, false, -1, onChange);
        }

        public string DrawUrlInput(string value, string label = "URL", bool disabled = false, Action<string> onChange = null)
        {
            return DrawLabeledInput(label, value, "https://example.com", InputVariant.Default, LabelVariant.Default, disabled, -1, onChange);
        }

       
        public int DrawNumberInput(int value, string label = "", int minValue = int.MinValue, int maxValue = int.MaxValue,
            bool disabled = false, Action<int> onChange = null)
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

       
        public float DrawFloatInput(float value, string label = "", float minValue = float.MinValue, float maxValue = float.MaxValue,
            bool disabled = false, Action<float> onChange = null)
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
