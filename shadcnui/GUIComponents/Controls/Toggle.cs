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
    public class Toggle : BaseComponent
    {
        public Toggle(GUIHelper helper)
            : base(helper) { }

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        #region Config-based API
        public bool DrawToggle(ToggleConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager?.GetToggleStyle(config.Variant, config.Size) ?? GUI.skin.toggle;

            bool wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            bool newValue;

            if (config.Icon?.Image != null)
            {
                GUILayout.BeginHorizontal();
                RenderIcon(config.Icon);
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                newValue = UnityHelpers.Toggle(config.Value, config.Text ?? "Toggle", toggleStyle);
                GUILayout.EndHorizontal();
            }
            else
            {
                if (config.Rect.HasValue)
                {
                    Rect scaledRect = new Rect(config.Rect.Value.x * guiHelper.uiScale, config.Rect.Value.y * guiHelper.uiScale, config.Rect.Value.width * guiHelper.uiScale, config.Rect.Value.height * guiHelper.uiScale);
                    newValue = UnityHelpers.Toggle(scaledRect, config.Value, config.Text ?? "", toggleStyle);
                }
                else
                {
                    newValue = config.Options != null && config.Options.Length > 0 ? UnityHelpers.Toggle(config.Value, config.Text, toggleStyle, config.Options) : UnityHelpers.Toggle(config.Value, config.Text, toggleStyle);
                }
            }

            GUI.enabled = wasEnabled;

            if (newValue != config.Value && !config.Disabled)
            {
                config.OnToggle?.Invoke(newValue);
            }

            return config.Disabled ? config.Value : newValue;
        }
        #endregion

        #region API
        public bool DrawToggle(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return DrawToggle(
                new ToggleConfig
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

        public bool DrawToggle(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            return DrawToggle(
                new ToggleConfig
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
