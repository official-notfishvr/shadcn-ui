using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIToggleComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUIToggleComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public void DrawToggle(float windowWidth, string label, ref bool value, Action<bool> onToggle)
        {
            var styleManager = guiHelper.GetStyleManager();
            string toggleText = label + (value ? " [ON]" : " [OFF]");
            Color originalColor = GUI.backgroundColor;
            if (guiHelper.customColorsEnabled)
            {
                GUI.backgroundColor = value ?
                    Color.Lerp(Color.green, guiHelper.primaryColor, 0.3f) :
                    Color.Lerp(Color.gray, guiHelper.secondaryColor, 0.3f);
            }
#if IL2CPP
            bool clicked = GUILayout.Button(toggleText, styleManager.animatedButtonStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            bool clicked = GUILayout.Button(toggleText, styleManager.animatedButtonStyle);
#endif
            GUI.backgroundColor = originalColor;
            if (clicked)
            {
                value = !value;
                onToggle?.Invoke(value);
            }
            layoutComponents.AddSpace(guiHelper.controlSpacing);
        }

        public bool DrawCheckbox(float windowWidth, string label, ref bool value)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            layoutComponents.BeginHorizontalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
            value = GUILayout.Toggle(value, "", (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(20) });
            GUILayout.Label(label, styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            layoutComponents.EndHorizontalGroup();
#else
            layoutComponents.BeginHorizontalGroup();
            value = GUILayout.Toggle(value, "", GUILayout.Width(20));
            GUILayout.Label(label, styleManager.glowLabelStyle);
            layoutComponents.EndHorizontalGroup();
#endif
            layoutComponents.AddSpace(guiHelper.controlSpacing);
            return value;
        }

        public int DrawSelectionGrid(float windowWidth, string label, int selected, string[] texts, int xCount)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            if (!string.IsNullOrEmpty(label))
                GUILayout.Label(label, styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            selected = GUILayout.SelectionGrid(selected, texts, xCount, styleManager.animatedButtonStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            if (!string.IsNullOrEmpty(label))
                GUILayout.Label(label, styleManager.glowLabelStyle);
            selected = GUILayout.SelectionGrid(selected, texts, xCount, styleManager.animatedButtonStyle);
#endif
            layoutComponents.AddSpace(guiHelper.controlSpacing);
            return selected;
        }
        public bool Toggle(string text, bool value, ToggleVariant variant = ToggleVariant.Default,
            ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager.GetToggleStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool newValue;
#if IL2CPP
            if (options != null && options.Length > 0)
                newValue = GUILayout.Toggle(value, text, toggleStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
            else
                newValue = GUILayout.Toggle(value, text, toggleStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            newValue = options != null && options.Length > 0 ?
                GUILayout.Toggle(value, text, toggleStyle, options) :
                GUILayout.Toggle(value, text, toggleStyle);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled)
            {
                onToggle?.Invoke(newValue);
            }

            return newValue;
        }
        public bool Toggle(Rect rect, string text, bool value, ToggleVariant variant = ToggleVariant.Default,
            ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager.GetToggleStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool newValue = GUI.Toggle(rect, value, text, toggleStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled)
            {
                onToggle?.Invoke(newValue);
            }

            return newValue;
        }
        public int ToggleGroup(string[] texts, int selectedIndex, Action<int> onSelectionChange = null,
            ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default,
            bool horizontal = true, float spacing = 5f)
        {
            int newSelectedIndex = selectedIndex;

            if (horizontal)
            {
#if IL2CPP
                layoutComponents.BeginHorizontalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginHorizontalGroup();
#endif
            }
            else
            {
#if IL2CPP
                layoutComponents.BeginVerticalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginVerticalGroup();
#endif
            }

            for (int i = 0; i < texts.Length; i++)
            {
                bool isSelected = (i == selectedIndex);
                if (Toggle(texts[i], isSelected, variant, size) && !isSelected)
                {
                    newSelectedIndex = i;
                    onSelectionChange?.Invoke(i);
                }

                if (horizontal && i < texts.Length - 1)
                    layoutComponents.AddSpace(spacing);
            }

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            layoutComponents.AddSpace(spacing);

            return newSelectedIndex;
        }
        public bool[] MultiToggleGroup(string[] texts, bool[] selectedStates, Action<int, bool> onToggleChange = null,
            ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default,
            bool horizontal = true, float spacing = 5f)
        {
            bool[] newStates = new bool[selectedStates.Length];
            Array.Copy(selectedStates, newStates, selectedStates.Length);

            if (horizontal)
            {
#if IL2CPP
                layoutComponents.BeginHorizontalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginHorizontalGroup();
#endif
            }
            else
            {
#if IL2CPP
                layoutComponents.BeginVerticalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginVerticalGroup();
#endif
            }

            for (int i = 0; i < texts.Length; i++)
            {
                newStates[i] = Toggle(texts[i], selectedStates[i], variant, size,
                    (value) => onToggleChange?.Invoke(i, value));

                if (horizontal && i < texts.Length - 1)
                    layoutComponents.AddSpace(spacing);
            }

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            layoutComponents.AddSpace(spacing);

            return newStates;
        }

        
    }
}
