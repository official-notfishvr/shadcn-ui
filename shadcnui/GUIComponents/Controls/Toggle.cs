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

        #region Config-based API
        public bool DrawToggle(ToggleConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle toggleStyle = styleManager?.GetToggleStyle(config.Variant, config.Size) ?? GUI.skin.toggle;

            bool wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            bool newValue = GetToggleValue(config, toggleStyle);

            GUI.enabled = wasEnabled;

            if (newValue != config.Value && !config.Disabled)
                config.OnToggle?.Invoke(newValue);

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

        #region Private Methods
        private bool GetToggleValue(ToggleConfig config, GUIStyle toggleStyle)
        {
            if (config.Icon?.Image != null)
                return DrawToggleWithIcon(config, toggleStyle);

            if (config.Rect.HasValue)
                return DrawToggleAtRect(config, toggleStyle);

            return DrawToggleLayout(config, toggleStyle);
        }

        private bool DrawToggleWithIcon(ToggleConfig config, GUIStyle toggleStyle)
        {
            GUILayout.BeginHorizontal();
            RenderIcon(config.Icon);
            layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
            bool newValue = UnityHelpers.Toggle(config.Value, config.Text ?? "Toggle", toggleStyle);
            GUILayout.EndHorizontal();
            return newValue;
        }

        private bool DrawToggleAtRect(ToggleConfig config, GUIStyle toggleStyle)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return UnityHelpers.Toggle(scaledRect, config.Value, config.Text ?? "", toggleStyle);
        }

        private bool DrawToggleLayout(ToggleConfig config, GUIStyle toggleStyle)
        {
            bool useExpandWidth = config.Size != ControlSize.Icon;
            GUILayoutOption[] options = BuildToggleLayoutOptions(config.Options, useExpandWidth);
            return UnityHelpers.Toggle(config.Value, config.Text, toggleStyle, options);
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
