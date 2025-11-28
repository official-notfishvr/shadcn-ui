using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.Layout;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Toggle : BaseComponent
    {
        public Toggle(GUIHelper helper)
            : base(helper) { }

        public bool DrawToggle(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager?.GetToggleStyle(variant, size) ?? GUI.skin.toggle;

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

        public bool DrawToggle(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager?.GetToggleStyle(variant, size) ?? GUI.skin.toggle;

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            bool newValue = UnityHelpers.Toggle(scaledRect, value, text ?? "", toggleStyle);

            GUI.enabled = wasEnabled;

            if (newValue != value && !disabled)
            {
                onToggle?.Invoke(newValue);
            }

            return disabled ? value : newValue;
        }
    }
}
