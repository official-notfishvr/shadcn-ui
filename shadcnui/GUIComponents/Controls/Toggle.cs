using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.Layout;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Toggle : BaseComponent
    {
        public Toggle(GUIHelper helper) : base(helper) { }

        public bool DrawToggle(string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager.GetToggleStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;
            newValue = options != null && options.Length > 0 ? UnityHelpers.Toggle(value, text, toggleStyle, options) : UnityHelpers.Toggle(value, text, toggleStyle);

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

            bool newValue = UnityHelpers.Toggle(rect, value, text, toggleStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled)
            {
                onToggle?.Invoke(newValue);
            }

            return newValue;
        }
    }
}
