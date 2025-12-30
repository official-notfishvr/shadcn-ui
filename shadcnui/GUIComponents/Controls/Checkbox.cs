using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
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

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        #region Config-based API
        public bool DrawCheckbox(CheckboxConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle checkboxStyle = styleManager?.GetCheckboxStyle(config.Variant, config.Size) ?? GUI.skin.toggle;

            bool wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            bool newValue;

            if (config.Icon?.Image != null)
            {
                GUILayout.BeginHorizontal();
                RenderIcon(config.Icon);
                layoutComponents.AddSpace(config.Icon.Spacing);
                newValue = UnityHelpers.Toggle(config.Value, config.Text ?? "Checkbox", checkboxStyle);
                GUILayout.EndHorizontal();
            }
            else
            {
                if (config.Rect.HasValue)
                {
                    Rect r = config.Rect.Value;
                    Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
                    newValue = UnityHelpers.Toggle(scaledRect, config.Value, config.Text ?? "Checkbox", checkboxStyle);
                }
                else
                {
                    newValue = UnityHelpers.Toggle(config.Value, config.Text ?? "Checkbox", checkboxStyle, config.Options);
                }
            }

            GUI.enabled = wasEnabled;

            if (newValue != config.Value && !config.Disabled)
                config.OnToggle?.Invoke(newValue);

            return config.Disabled ? config.Value : newValue;
        }
        #endregion

        #region API
        public bool DrawCheckbox(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return DrawCheckbox(
                new CheckboxConfig
                {
                    Text = text,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnToggle = onToggle,
                    Disabled = disabled,
                    Options = options,
                }
            );
        }

        public bool DrawCheckbox(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            return DrawCheckbox(
                new CheckboxConfig
                {
                    Rect = rect,
                    Text = text,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnToggle = onToggle,
                    Disabled = disabled,
                }
            );
        }
        #endregion
    }
}
