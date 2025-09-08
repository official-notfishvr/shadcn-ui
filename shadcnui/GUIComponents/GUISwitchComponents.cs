using shadcnui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUISwitchComponents
    {
        private GUIHelper guiHelper;

        public GUISwitchComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public bool Switch(string text, bool value, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return GUILayout.Toggle(value, text ?? "Switch");
            }

            GUIStyle switchStyle = styleManager.GetSwitchStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool newValue;
#if IL2CPP
            newValue = GUILayout.Toggle(value, text ?? "Switch", switchStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            newValue = GUILayout.Toggle(value, text ?? "Switch", switchStyle, options);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool Switch(Rect rect, string text, bool value, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return GUI.Toggle(rect, value, text ?? "Switch");
            }

            GUIStyle switchStyle = styleManager.GetSwitchStyle(variant, size);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool newValue = GUI.Toggle(scaledRect, value, text ?? "Switch", switchStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool SwitchWithLabel(string label, bool value, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            GUILayout.BeginHorizontal();
            
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;
            
            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
            }

#if IL2CPP
            GUILayout.Label(label ?? "", labelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label ?? "", labelStyle);
#endif

            GUI.color = originalColor;
            
            GUILayout.FlexibleSpace();
            
            bool newValue = Switch("", value, variant, size, onToggle, disabled, GUILayout.Width(50 * guiHelper.uiScale));
            
            GUILayout.EndHorizontal();

            return newValue;
        }

        public bool SwitchWithDescription(string label, string description, bool value, 
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            GUILayout.BeginVertical();
            
            bool newValue = SwitchWithLabel(label, value, variant, size, onToggle, disabled);
            
            if (!string.IsNullOrEmpty(description))
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle descStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
                
                Color originalColor = GUI.color;
                if (disabled)
                {
                    GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
                }

                GUILayout.Space(2 * guiHelper.uiScale);
#if IL2CPP
                GUILayout.Label(description, descStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(description, descStyle);
#endif

                GUI.color = originalColor;
            }
            
            GUILayout.EndVertical();

            return newValue;
        }

        public bool CustomSwitch(string text, bool value, Color onColor, Color offColor, Color thumbColor,
            Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return GUILayout.Toggle(value, text ?? "Switch");
            }

            GUIStyle customStyle = new GUIStyle(GUI.skin.toggle);
            
            if (value)
            {
                customStyle.normal.background = styleManager.CreateSolidTexture(onColor);
                customStyle.active.background = styleManager.CreateSolidTexture(Color.Lerp(onColor, Color.white, 0.1f));
                customStyle.hover.background = styleManager.CreateSolidTexture(Color.Lerp(onColor, Color.white, 0.05f));
            }
            else
            {
                customStyle.normal.background = styleManager.CreateSolidTexture(offColor);
                customStyle.active.background = styleManager.CreateSolidTexture(Color.Lerp(offColor, Color.black, 0.1f));
                customStyle.hover.background = styleManager.CreateSolidTexture(Color.Lerp(offColor, Color.black, 0.05f));
            }
            
            customStyle.normal.textColor = thumbColor;
            customStyle.active.textColor = thumbColor;
            customStyle.hover.textColor = thumbColor;

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool newValue;
#if IL2CPP
            newValue = GUILayout.Toggle(value, text ?? "Switch", customStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            newValue = GUILayout.Toggle(value, text ?? "Switch", customStyle, options);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool SwitchWithIcon(string text, bool value, Texture2D onIcon, Texture2D offIcon, 
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            GUILayout.BeginHorizontal();
            
            bool newValue = Switch("", value, variant, size, onToggle, disabled, GUILayout.Width(50 * guiHelper.uiScale));
            
            Texture2D iconToShow = value ? onIcon : offIcon;
            if (iconToShow != null)
            {
                GUILayout.Space(4 * guiHelper.uiScale);
                GUILayout.Label(iconToShow, GUILayout.Width(16 * guiHelper.uiScale), GUILayout.Height(16 * guiHelper.uiScale));
            }
            
            if (!string.IsNullOrEmpty(text))
            {
                GUILayout.Space(4 * guiHelper.uiScale);
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle labelStyle = styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;
                
                Color originalColor = GUI.color;
                if (disabled)
                {
                    GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
                }

#if IL2CPP
                GUILayout.Label(text, labelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(text, labelStyle);
#endif

                GUI.color = originalColor;
            }
            
            GUILayout.EndHorizontal();

            return newValue;
        }

        public bool ValidatedSwitch(string text, bool value, bool isValid, string validationMessage,
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            GUILayout.BeginVertical();
            
            bool newValue = SwitchWithLabel(text, value, variant, size, onToggle, disabled);
            
            if (!isValid && !string.IsNullOrEmpty(validationMessage))
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle errorStyle = styleManager?.GetLabelStyle(LabelVariant.Destructive) ?? GUI.skin.label;
                
                GUILayout.Space(2 * guiHelper.uiScale);
#if IL2CPP
                GUILayout.Label(validationMessage, errorStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(validationMessage, errorStyle);
#endif
            }
            
            GUILayout.EndVertical();

            return newValue;
        }

        public bool SwitchWithTooltip(string text, bool value, string tooltip, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            GUILayout.BeginHorizontal();
            
            bool newValue = SwitchWithLabel(text, value, variant, size, onToggle, disabled);
            
            if (!string.IsNullOrEmpty(tooltip))
            {
                GUILayout.Space(4 * guiHelper.uiScale);
                
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle tooltipStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
                
                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);
                
#if IL2CPP
                GUILayout.Label("?", tooltipStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label("?", tooltipStyle);
#endif
                
                GUI.color = originalColor;
            }
            
            GUILayout.EndHorizontal();

            return newValue;
        }

        public bool[] SwitchGroup(string[] labels, bool[] values, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<int, bool> onToggleChange = null, 
            bool disabled = false, bool horizontal = false, float spacing = 5f)
        {
            if (labels == null || values == null || labels.Length != values.Length)
            {
                Debug.LogError("SwitchGroup: labels and values arrays must be non-null and same length");
                return values ?? new bool[0];
            }

            bool[] newValues = new bool[values.Length];
            Array.Copy(values, newValues, values.Length);

            if (horizontal)
            {
                GUILayout.BeginHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();
            }

            for (int i = 0; i < labels.Length; i++)
            {
                int index = i;
                bool newValue = SwitchWithLabel(labels[i], values[i], variant, size, 
                    (val) => {
                        newValues[index] = val;
                        onToggleChange?.Invoke(index, val);
                    }, disabled);

                if (i < labels.Length - 1)
                {
                    GUILayout.Space(spacing * guiHelper.uiScale);
                }
            }

            if (horizontal)
                GUILayout.EndHorizontal();
            else
                GUILayout.EndVertical();

            return newValues;
        }

        public bool SwitchWithLoading(string text, bool value, bool isLoading, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            GUILayout.BeginHorizontal();
            
            bool newValue = SwitchWithLabel(text, value, variant, size, onToggle, disabled || isLoading);
            
            if (isLoading)
            {
                GUILayout.Space(4 * guiHelper.uiScale);
                
                float time = Time.time * 2f;
                float alpha = (Mathf.Sin(time) + 1f) * 0.5f;
                
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle loadingStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
                
                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                
#if IL2CPP
                GUILayout.Label("...", loadingStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label("...", loadingStyle);
#endif
                
                GUI.color = originalColor;
            }
            
            GUILayout.EndHorizontal();

            return newValue;
        }
    }
}
