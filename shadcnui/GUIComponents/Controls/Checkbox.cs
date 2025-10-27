using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Checkbox : BaseComponent
    {
        public Checkbox(GUIHelper helper)
            : base(helper) { }

        public bool DrawCheckbox(string text, bool value, CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            if (styleManager == null)
            {
                return UnityHelpers.Toggle(value, text ?? "Checkbox", GUI.skin.toggle, new GUILayoutOption[0]);
            }
            GUIStyle checkboxStyle = styleManager.GetCheckboxStyle(variant, size);
            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;
            bool newValue = UnityHelpers.Toggle(value, text ?? "Checkbox", checkboxStyle, options);
            GUI.enabled = wasEnabled;
            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);
            return newValue;
        }

        public bool DrawCheckbox(Rect rect, string text, bool value, CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            if (styleManager == null)
            {
                return GUI.Toggle(rect, value, text ?? "Checkbox");
            }
            GUIStyle checkboxStyle = styleManager.GetCheckboxStyle(variant, size);
            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);
            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;
            bool newValue = UnityHelpers.Toggle(scaledRect, value, text ?? "Checkbox", checkboxStyle);
            GUI.enabled = wasEnabled;
            if (newValue != value && !disabled && onToggle != null)
                onToggle.Invoke(newValue);
            return newValue;
        }
    }
}
