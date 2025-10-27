using System;
using System.Collections.Generic;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Switch : BaseComponent
    {
        public Switch(GUIHelper helper)
            : base(helper) { }

        public bool DrawSwitch(string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                return UnityHelpers.Toggle(value, text ?? "Switch", GUI.skin.toggle, new GUILayoutOption[0]);
            }

            GUIStyle switchStyle = styleManager.GetSwitchStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue;

            newValue = UnityHelpers.Toggle(value, text ?? "Switch", switchStyle, options);

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
    }
}
