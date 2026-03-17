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
    public class Checkbox : BaseComponent
    {
        public Checkbox(GUIHelper helper)
            : base(helper) { }

        public bool Draw(CheckboxConfig config)
        {
            if (config == null)
                return false;

            GUIStyle checkboxStyle = config.ShowCheckmark ? (styleManager?.GetCheckboxStyle(config.Variant, config.Size) ?? GUI.skin.toggle) : (styleManager?.GetCheckboxSolidStyle(config.Variant, config.Size) ?? GUI.skin.toggle);

            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            bool newValue = config.Rect.HasValue ? DrawRect(config, checkboxStyle) : DrawLayout(config, checkboxStyle);

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
                bool value = DrawToggleRect(config.Value, label, style, options.ToArray(), config.IsDisabled);
                layoutComponents.EndHorizontalGroup();
                return value;
            }

            return DrawToggleRect(config.Value, label, style, options.ToArray(), config.IsDisabled);
        }

        private bool DrawRect(BoolControlConfigBase config, GUIStyle style)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            bool hovered = scaledRect.Contains(Event.current.mousePosition) && !config.IsDisabled;
            float offset = hovered ? DesignTokens.Spacing.XXS * guiHelper.uiScale : 0f;
            var rect = offset > 0f ? new Rect(scaledRect.x, scaledRect.y - offset, scaledRect.width, scaledRect.height) : scaledRect;
            return UnityHelpers.Toggle(rect, config.Value, config.Label ?? string.Empty, style);
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

        private bool DrawToggleRect(bool value, string label, GUIStyle style, GUILayoutOption[] options, bool disabled)
        {
            var content = new UnityHelpers.GUIContent(label ?? string.Empty);
            var rect = GUILayoutUtility.GetRect(content, style, options);
            bool hovered = rect.Contains(Event.current.mousePosition) && !disabled;
            float offset = hovered ? DesignTokens.Spacing.XXS * guiHelper.uiScale : 0f;
            var drawRect = offset > 0f ? new Rect(rect.x, rect.y - offset, rect.width, rect.height) : rect;
            return UnityHelpers.Toggle(drawRect, value, content, style);
        }
    }
}
