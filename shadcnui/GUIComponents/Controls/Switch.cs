using System;
using System.Collections.Generic;
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

        public bool Draw(SwitchConfig config)
        {
            if (config == null)
                return false;

            GUIStyle switchStyle = styleManager?.GetSwitchStyle(config.Variant, config.Size) ?? GUI.skin.toggle;

            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            bool newValue = config.Rect.HasValue ? DrawRect(config, switchStyle) : DrawLayout(config, switchStyle);

            GUI.enabled = prevEnabled;

            if (newValue != config.Value && !config.IsDisabled)
                config.OnValueChanged?.Invoke(newValue);

            return config.IsDisabled ? config.Value : newValue;
        }

        private bool DrawLayout(BoolControlConfigBase config, GUIStyle style)
        {
            var options = BuildLayoutOptions(config);
            string label = config.Label ?? string.Empty;

            if (config.Icon?.Image != null)
            {
                layoutComponents.BeginHorizontalGroup();
                RenderIcon(config.Icon);
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                bool value = UnityHelpers.Toggle(config.Value, label, style, options.ToArray());
                layoutComponents.EndHorizontalGroup();
                return value;
            }

            return UnityHelpers.Toggle(config.Value, label, style, options.ToArray());
        }

        private bool DrawRect(BoolControlConfigBase config, GUIStyle style)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return UnityHelpers.Toggle(scaledRect, config.Value, config.Label ?? string.Empty, style);
        }

        private List<GUILayoutOption> BuildLayoutOptions(BoolControlConfigBase config)
        {
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
            if (config.FullRowClick)
                options.Add(GUILayout.ExpandWidth(true));
            return options;
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            if (iconConfig?.Image == null)
                return;

            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }
    }
}
