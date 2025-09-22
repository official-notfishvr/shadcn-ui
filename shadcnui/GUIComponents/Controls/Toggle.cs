using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Toggle
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Toggle(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public bool DrawToggle(string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager.GetToggleStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;
#if IL2CPP
            if (options != null && options.Length > 0)
                newValue = GUILayout.Toggle(value, text, toggleStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
            else
                newValue = GUILayout.Toggle(value, text, toggleStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            newValue = options != null && options.Length > 0 ? GUILayout.Toggle(value, text, toggleStyle, options) : GUILayout.Toggle(value, text, toggleStyle);
#endif

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled)
            {
                onToggle?.Invoke(newValue);
            }

            return newValue;
        }

        public bool DrawToggle(Rect rect, string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager.GetToggleStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue = GUI.Toggle(rect, value, text, toggleStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled)
            {
                onToggle?.Invoke(newValue);
            }

            return newValue;
        }

        public int ToggleGroup(string[] texts, int selectedIndex, Action<int> onSelectionChange = null, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, bool horizontal = true, float spacing = 5f)
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
                if (DrawToggle(texts[i], isSelected, variant, size) && !isSelected)
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

        public bool[] MultiToggleGroup(string[] texts, bool[] selectedStates, Action<int, bool> onToggleChange = null, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, bool horizontal = true, float spacing = 5f)
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
                newStates[i] = DrawToggle(texts[i], selectedStates[i], variant, size, (value) => onToggleChange?.Invoke(i, value));

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
