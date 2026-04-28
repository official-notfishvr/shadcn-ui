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
    public class Toggle : BaseComponent
    {
        public Toggle(GUIHelper helper)
            : base(helper) { }

        public bool Draw(ToggleConfig config)
        {
            if (config == null)
                return false;

            GUIStyle toggleStyle = styleManager?.GetToggleStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.button;

            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            bool newValue = config.Rect.HasValue ? DrawRect(config, toggleStyle) : DrawLayout(config, toggleStyle);

            GUI.enabled = prevEnabled;

            if (newValue != config.Value && !config.IsDisabled)
                config.OnValueChanged?.Invoke(newValue);

            return config.IsDisabled ? config.Value : newValue;
        }

        private bool DrawLayout(BoolControlConfigBase config, GUIStyle style)
        {
            var options = BuildLayoutOptions(config);
            var content = new UnityHelpers.GUIContent(config.Label ?? string.Empty);
            var rect = GUILayoutUtility.GetRect(content, style, options.ToArray());
            return DrawToggleButton(rect, config, style);
        }

        private bool DrawRect(BoolControlConfigBase config, GUIStyle style)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return DrawToggleButton(scaledRect, config, style);
        }

        private List<GUILayoutOption> BuildLayoutOptions(BoolControlConfigBase config)
        {
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
            if (config.FullRowClick)
                options.Add(GUILayout.ExpandWidth(true));
            return options;
        }

        private bool DrawToggleButton(Rect rect, BoolControlConfigBase config, GUIStyle style)
        {
            bool next = GUI.Toggle(rect, config.Value, GUIContent.none, style);
            DrawToggleContent(rect, config, style);
            return next;
        }

        private void DrawToggleContent(Rect rect, BoolControlConfigBase config, GUIStyle style)
        {
            float spacing = DesignTokens.Spacing.XS * guiHelper.uiScale;
            float iconSize = config.Icon?.Image != null ? config.Icon.Size * guiHelper.uiScale : 0f;
            var labelStyle = new UnityHelpers.GUIStyle(style) { normal = { background = null }, alignment = TextAnchor.MiddleCenter };

            var labelContent = new UnityHelpers.GUIContent(config.Label ?? string.Empty);
            Vector2 labelSize = labelStyle.CalcSize(labelContent);
            float totalWidth = labelSize.x + (iconSize > 0f ? iconSize + spacing : 0f);
            float startX = rect.x + (rect.width - totalWidth) * 0.5f;

            if (iconSize > 0f && config.Icon?.Image != null)
            {
                var iconRect = new Rect(startX, rect.y + (rect.height - iconSize) * 0.5f, iconSize, iconSize);
                GUI.DrawTexture(iconRect, config.Icon.Image, ScaleMode.ScaleToFit);
                startX += iconSize + spacing;
            }

            var textRect = new Rect(startX, rect.y, labelSize.x, rect.height);
            GUI.Label(textRect, labelContent, labelStyle);
        }
    }
}
