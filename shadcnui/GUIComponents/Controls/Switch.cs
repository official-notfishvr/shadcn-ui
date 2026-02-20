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
    public class Switch : BaseComponent
    {
        public Switch(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public bool DrawSwitch(SwitchConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle switchStyle = styleManager?.GetSwitchStyle(config.Variant, config.Size) ?? GUI.skin.toggle;

            bool wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            bool newValue = GetSwitchValue(config, switchStyle);

            GUI.enabled = wasEnabled;

            if (newValue != config.Value && !config.Disabled)
                config.OnToggle?.Invoke(newValue);

            return config.Disabled ? config.Value : newValue;
        }
        #endregion

        #region API
        public bool DrawSwitch(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return DrawSwitch(
                new SwitchConfig
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

        public bool DrawSwitch(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            return DrawSwitch(
                new SwitchConfig
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

        #region Private Methods
        private bool GetSwitchValue(SwitchConfig config, GUIStyle switchStyle)
        {
            if (config.Icon?.Image != null)
                return DrawSwitchWithIcon(config, switchStyle);

            if (config.Rect.HasValue)
                return DrawSwitchAtRect(config, switchStyle);

            return DrawSwitchLayout(config, switchStyle);
        }

        private bool DrawSwitchWithIcon(SwitchConfig config, GUIStyle switchStyle)
        {
            GUILayout.BeginHorizontal();
            RenderIcon(config.Icon);
            layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
            bool newValue = UnityHelpers.Toggle(config.Value, config.Text ?? "Switch", switchStyle);
            GUILayout.EndHorizontal();
            return newValue;
        }

        private bool DrawSwitchAtRect(SwitchConfig config, GUIStyle switchStyle)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return GUI.Toggle(scaledRect, config.Value, config.Text ?? "Switch", switchStyle);
        }

        private bool DrawSwitchLayout(SwitchConfig config, GUIStyle switchStyle)
        {
            bool useExpandWidth = config.Size != ControlSize.Icon;
            GUILayoutOption[] options = BuildToggleLayoutOptions(config.Options, useExpandWidth);
            return UnityHelpers.Toggle(config.Value, config.Text ?? "Switch", switchStyle, options);
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        private static GUILayoutOption[] BuildToggleLayoutOptions(GUILayoutOption[] configOptions, bool expandWidth)
        {
            int extra = expandWidth ? 1 : 0;
            if (configOptions == null || configOptions.Length == 0)
                return expandWidth ? new[] { GUILayout.ExpandWidth(true) } : Array.Empty<GUILayoutOption>();
            var options = new GUILayoutOption[configOptions.Length + extra];
            configOptions.CopyTo(options, 0);
            if (expandWidth)
                options[configOptions.Length] = GUILayout.ExpandWidth(true);
            return options;
        }
        #endregion
    }
}
