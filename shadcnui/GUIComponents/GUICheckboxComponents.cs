using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUICheckboxComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUICheckboxComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public bool Checkbox(
            string text,
            bool value,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
#if IL2CPP
                return GUILayout.Toggle(
                    value,
                    text ?? "Checkbox",
                    GUI.skin.toggle,
                    new Il2CppReferenceArray<GUILayoutOption>(0)
                );
#else
                return GUILayout.Toggle(
                    value,
                    text ?? "Checkbox",
                    GUI.skin.toggle,
                    new GUILayoutOption[0]
                );
#endif
            }

            GUIStyle checkboxStyle = styleManager.GetCheckboxStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;
#if IL2CPP
            newValue = GUILayout.Toggle(
                value,
                text ?? "Checkbox",
                checkboxStyle,
                options != null && options.Length > 0
                    ? new Il2CppReferenceArray<GUILayoutOption>(options)
                    : new Il2CppReferenceArray<GUILayoutOption>(0)
            );
#else
            newValue = GUILayout.Toggle(value, text ?? "Checkbox", checkboxStyle, options);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool Checkbox(
            Rect rect,
            string text,
            bool value,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false
        )
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return GUI.Toggle(rect, value, text ?? "Checkbox");
            }

            GUIStyle checkboxStyle = styleManager.GetCheckboxStyle(variant, size);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue = GUI.Toggle(scaledRect, value, text ?? "Checkbox", checkboxStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool CheckboxWithLabel(
            string label,
            ref bool value,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false
        )
        {
            layoutComponents.BeginHorizontalGroup();

            bool newValue = Checkbox(
                "",
                value,
                variant,
                size,
                onToggle,
                disabled,
                GUILayout.Width(20 * guiHelper.uiScale)
            );

            if (newValue != value)
            {
                value = newValue;
                onToggle?.Invoke(newValue);
            }

            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle =
                styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;

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
            layoutComponents.EndHorizontalGroup();

            return newValue;
        }

        public bool[] CheckboxGroup(
            string[] labels,
            bool[] values,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<int, bool> onToggleChange = null,
            bool disabled = false,
            bool horizontal = false,
            float spacing = 5f
        )
        {
            if (labels == null || values == null || labels.Length != values.Length)
            {
                Debug.LogError(
                    "CheckboxGroup: labels and values arrays must be non-null and same length"
                );
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
                bool currentValue = newValues[i];
                CheckboxWithLabel(
                    labels[i],
                    ref currentValue,
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

        public bool CustomCheckbox(
            string text,
            bool value,
            Color checkColor,
            Color backgroundColor,
            Action<bool> onToggle = null,
            bool disabled = false,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new GUIStyle(GUI.skin.toggle);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.active.background = styleManager.CreateSolidTexture(
                Color.Lerp(backgroundColor, checkColor, 0.3f)
            );
            customStyle.hover.background = styleManager.CreateSolidTexture(
                Color.Lerp(backgroundColor, checkColor, 0.1f)
            );
            customStyle.onNormal.background = styleManager.CreateSolidTexture(checkColor);
            customStyle.onActive.background = styleManager.CreateSolidTexture(
                Color.Lerp(checkColor, backgroundColor, 0.3f)
            );
            customStyle.onHover.background = styleManager.CreateSolidTexture(
                Color.Lerp(checkColor, backgroundColor, 0.1f)
            );
            customStyle.normal.textColor = checkColor;
            customStyle.active.textColor = checkColor;
            customStyle.hover.textColor = checkColor;
            customStyle.onNormal.textColor = backgroundColor;
            customStyle.onActive.textColor = backgroundColor;
            customStyle.onHover.textColor = backgroundColor;

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;
#if IL2CPP
            newValue = GUILayout.Toggle(
                value,
                text ?? "Checkbox",
                customStyle,
                (Il2CppReferenceArray<GUILayoutOption>)options
            );
#else
            newValue = GUILayout.Toggle(value, text ?? "Checkbox", customStyle, options);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);

            return newValue && !disabled;
        }

        public bool CheckboxWithIcon(
            string text,
            ref bool value,
            Texture2D icon,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false
        )
        {
            layoutComponents.BeginHorizontalGroup();

            bool newValue = Checkbox(
                "",
                value,
                variant,
                size,
                onToggle,
                disabled,
                GUILayout.Width(20 * guiHelper.uiScale)
            );
            if (newValue != value)
            {
                value = newValue;
                onToggle?.Invoke(newValue);
            }

            if (icon != null)
            {
#if IL2CPP
                GUILayout.Label(
                    icon,
                    GUIStyle.none,
                    new Il2CppReferenceArray<GUILayoutOption>(
                        new GUILayoutOption[]
                        {
                            GUILayout.Width(16 * guiHelper.uiScale),
                            GUILayout.Height(16 * guiHelper.uiScale),
                        }
                    )
                );
#else
                GUILayout.Label(
                    icon,
                    GUILayout.Width(16 * guiHelper.uiScale),
                    GUILayout.Height(16 * guiHelper.uiScale)
                );
#endif
                layoutComponents.AddSpace(4);
            }

            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle =
                styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;

            Color originalColor = GUI.color;
            if (disabled)
            {
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
            }

#if IL2CPP
            GUILayout.Label(text ?? "", labelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(text ?? "", labelStyle);
#endif

            GUI.color = originalColor;
            layoutComponents.EndHorizontalGroup();

            return newValue;
        }

        public bool CheckboxWithDescription(
            string label,
            string description,
            ref bool value,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false
        )
        {
            layoutComponents.BeginVerticalGroup();

            bool newValue = CheckboxWithLabel(label, ref value, variant, size, onToggle, disabled);

            if (!string.IsNullOrEmpty(description))
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle descStyle =
                    styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

                Color originalColor = GUI.color;
                if (disabled)
                {
                    GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
                }

                layoutComponents.AddSpace(2);
#if IL2CPP
                GUILayout.Label(
                    description,
                    descStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label(description, descStyle);
#endif

                GUI.color = originalColor;
            }

            layoutComponents.EndVerticalGroup();

            return newValue;
        }

        public bool ValidatedCheckbox(
            string text,
            ref bool value,
            bool isValid,
            string validationMessage,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false
        )
        {
            layoutComponents.BeginVerticalGroup();

            bool newValue = CheckboxWithLabel(text, ref value, variant, size, onToggle, disabled);

            if (!isValid && !string.IsNullOrEmpty(validationMessage))
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle errorStyle =
                    styleManager?.GetLabelStyle(LabelVariant.Destructive) ?? GUI.skin.label;

                layoutComponents.AddSpace(2);
#if IL2CPP
                GUILayout.Label(
                    validationMessage,
                    errorStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label(validationMessage, errorStyle);
#endif
            }

            layoutComponents.EndVerticalGroup();

            return newValue;
        }

        public bool CheckboxWithTooltip(
            string text,
            ref bool value,
            string tooltip,
            CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null,
            bool disabled = false
        )
        {
            layoutComponents.BeginHorizontalGroup();

            bool newValue = CheckboxWithLabel(text, ref value, variant, size, onToggle, disabled);

            if (!string.IsNullOrEmpty(tooltip))
            {
                layoutComponents.AddSpace(4);

                var styleManager = guiHelper.GetStyleManager();
                GUIStyle tooltipStyle =
                    styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);

#if IL2CPP
                GUILayout.Label("?", tooltipStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label("?", tooltipStyle);
#endif

                GUI.color = originalColor;
            }

            layoutComponents.EndHorizontalGroup();

            return newValue;
        }
    }
}
