using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Switch
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Switch(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public bool DrawSwitch(string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
#if IL2CPP_MELONLOADER
                return GUILayout.Toggle(value, text ?? "Switch", GUI.skin.toggle, new Il2CppReferenceArray<GUILayoutOption>(0));
#else
                return GUILayout.Toggle(value, text ?? "Switch", GUI.skin.toggle, new GUILayoutOption[0]);
#endif
            }

            GUIStyle switchStyle = styleManager.GetSwitchStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;
#if IL2CPP_MELONLOADER
            newValue = GUILayout.Toggle(value, text ?? "Switch", switchStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            newValue = GUILayout.Toggle(value, text ?? "Switch", switchStyle, options);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool DrawSwitch(Rect rect, string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return GUI.Toggle(rect, value, text ?? "Switch");
            }

            GUIStyle switchStyle = styleManager.GetSwitchStyle(variant, size);

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue = GUI.Toggle(scaledRect, value, text ?? "Switch", switchStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool SwitchWithLabel(string label, ref bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            layoutComponents.BeginHorizontalGroup();

            bool newValue = DrawSwitch("", value, variant, size, onToggle, disabled, GUILayout.Width(40 * guiHelper.uiScale));

            if (newValue != value)
            {
                value = newValue;
                onToggle?.Invoke(newValue);
            }

            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;

            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
            }

#if IL2CPP_MELONLOADER
            GUILayout.Label(label ?? "", labelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label ?? "", labelStyle);
#endif

            GUI.color = originalColor;
            layoutComponents.EndHorizontalGroup();

            return newValue;
        }

        public bool SwitchWithDescription(string label, string description, ref bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            layoutComponents.BeginVerticalGroup();

            bool newValue = SwitchWithLabel(label, ref value, variant, size, onToggle, disabled);

            if (!string.IsNullOrEmpty(description))
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle descStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

                Color originalColor = GUI.color;
                if (disabled)
                {
                    GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
                }

                layoutComponents.AddSpace(2);
#if IL2CPP_MELONLOADER
                GUILayout.Label(description, descStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(description, descStyle);
#endif

                GUI.color = originalColor;
            }

            layoutComponents.EndVerticalGroup();

            return newValue;
        }

        public bool CustomSwitch(string text, bool value, Color onColor, Color offColor, Color thumbColor, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
            customStyle.normal.background = styleManager.CreateSolidTexture(offColor);
            customStyle.onNormal.background = styleManager.CreateSolidTexture(onColor);
            customStyle.normal.textColor = thumbColor;
            customStyle.onNormal.textColor = thumbColor;

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;
#if IL2CPP_MELONLOADER
            newValue = GUILayout.Toggle(value, text ?? "Switch", customStyle, options != null && options.Length > 0 ? new Il2CppReferenceArray<GUILayoutOption>(options) : new Il2CppReferenceArray<GUILayoutOption>(0));
#else
            newValue = GUILayout.Toggle(value, text ?? "Switch", customStyle, options);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool SwitchWithIcon(string text, bool value, Texture2D onIcon, Texture2D offIcon, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
#if IL2CPP_MELONLOADER
            GUILayout.BeginHorizontal(new Il2CppReferenceArray<GUILayoutOption>(0));
#else
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
#endif
            bool newValue = DrawSwitch("", value, variant, size, onToggle, disabled, GUILayout.Width(50 * guiHelper.uiScale));

            Texture2D iconToShow = value ? onIcon : offIcon;
            if (iconToShow != null)
            {
                GUILayout.Space(4 * guiHelper.uiScale);
#if IL2CPP_MELONLOADER
                GUILayout.Label(iconToShow, GUI.skin.label, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(16 * guiHelper.uiScale), GUILayout.Height(16 * guiHelper.uiScale) }));
#else
                GUILayout.Label(iconToShow, GUILayout.Width(16 * guiHelper.uiScale), GUILayout.Height(16 * guiHelper.uiScale));
#endif
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

#if IL2CPP_MELONLOADER
                GUILayout.Label(text, labelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(text, labelStyle);
#endif

                GUI.color = originalColor;
            }

            GUILayout.EndHorizontal();

            return newValue;
        }

        public bool ValidatedSwitch(string text, ref bool value, bool isValid, string validationMessage, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            layoutComponents.BeginVerticalGroup();

            bool newValue = SwitchWithLabel(text, ref value, variant, size, onToggle, disabled);

            if (!isValid && !string.IsNullOrEmpty(validationMessage))
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle errorStyle = styleManager?.GetLabelStyle(LabelVariant.Destructive) ?? GUI.skin.label;

                layoutComponents.AddSpace(2);
#if IL2CPP_MELONLOADER
                GUILayout.Label(validationMessage, errorStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(validationMessage, errorStyle);
#endif
            }

            layoutComponents.EndVerticalGroup();

            return newValue;
        }

        public bool SwitchWithTooltip(string text, ref bool value, string tooltip, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            layoutComponents.BeginHorizontalGroup();

            bool newValue = SwitchWithLabel(text, ref value, variant, size, onToggle, disabled);

            if (!string.IsNullOrEmpty(tooltip))
            {
                layoutComponents.AddSpace(4);

                var styleManager = guiHelper.GetStyleManager();
                GUIStyle tooltipStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);

#if IL2CPP_MELONLOADER
                GUILayout.Label("?", tooltipStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label("?", tooltipStyle);
#endif
                GUI.color = originalColor;
            }

            layoutComponents.EndHorizontalGroup();

            return newValue;
        }

        public bool[] SwitchGroup(string[] labels, bool[] values, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<int, bool> onToggleChange = null, bool disabled = false, bool horizontal = false, float spacing = 5f)
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
                layoutComponents.BeginHorizontalGroup();
            }
            else
            {
                layoutComponents.BeginVerticalGroup();
            }

            for (int i = 0; i < labels.Length; i++)
            {
                int index = i;
                bool newValue = SwitchWithLabel(
                    labels[i],
                    ref newValues[i],
                    variant,
                    size,
                    (val) =>
                    {
                        newValues[index] = val;
                        onToggleChange?.Invoke(index, val);
                    },
                    disabled
                );

                if (i < labels.Length - 1)
                {
                    layoutComponents.AddSpace(spacing);
                }
            }

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            return newValues;
        }

        public bool SwitchWithLoading(string text, bool value, bool isLoading, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            layoutComponents.BeginHorizontalGroup();

            bool newValue = SwitchWithLabel(text, ref value, variant, size, onToggle, disabled || isLoading);

            if (isLoading)
            {
                layoutComponents.AddSpace(4);

                float time = Time.time * 2f;
                float alpha = (Mathf.Sin(time) + 1f) * 0.5f;

                var styleManager = guiHelper.GetStyleManager();
                GUIStyle loadingStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

#if IL2CPP_MELONLOADER
                GUILayout.Label("...", loadingStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label("...", loadingStyle);
#endif
                GUI.color = originalColor;
            }

            layoutComponents.EndHorizontalGroup();

            return newValue;
        }
    }
}
