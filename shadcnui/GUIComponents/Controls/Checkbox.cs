using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
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

            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            bool newValue = config.Rect.HasValue ? DrawRect(config) : DrawLayout(config);

            GUI.enabled = prevEnabled;

            if (newValue != config.Value && !config.IsDisabled)
                config.OnValueChanged?.Invoke(newValue);

            return config.IsDisabled ? config.Value : newValue;
        }

        private bool DrawLayout(BoolControlConfigBase config)
        {
            var options = BuildLayoutOptions(config);
            float rowHeight = DesignTokens.Checkbox.Size * guiHelper.uiScale + DesignTokens.Spacing.XS * guiHelper.uiScale;
            var rowRect = GUILayoutUtility.GetRect(new UnityHelpers.GUIContent(config.Label ?? string.Empty), GUIStyle.none, options.ToArray());
            rowRect.height = Mathf.Max(rowRect.height, rowHeight);
            return DrawCheckboxRow(rowRect, config);
        }

        private bool DrawRect(BoolControlConfigBase config)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return DrawCheckboxRow(scaledRect, config);
        }

        private List<GUILayoutOption> BuildLayoutOptions(BoolControlConfigBase config)
        {
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
            if (config.FullRowClick)
                options.Add(GUILayout.ExpandWidth(true));
            return options;
        }

        private bool DrawCheckboxRow(Rect rowRect, BoolControlConfigBase config)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            float boxSize = DesignTokens.Checkbox.Size * guiHelper.uiScale;
            float gap = DesignTokens.Spacing.MD * guiHelper.uiScale;
            float iconSize = config.Icon?.Image != null ? config.Icon.Size * guiHelper.uiScale : 0f;
            float iconGap = config.Icon?.Image != null ? config.Icon.Spacing * guiHelper.uiScale : 0f;

            float x = rowRect.x;
            if (config.Icon?.Image != null)
            {
                var iconRect = new Rect(x, rowRect.y + (rowRect.height - iconSize) * 0.5f, iconSize, iconSize);
                GUI.DrawTexture(iconRect, config.Icon.Image, ScaleMode.ScaleToFit);
                x += iconSize + iconGap;
            }

            var boxRect = new Rect(x, rowRect.y + (rowRect.height - boxSize) * 0.5f, boxSize, boxSize);
            x += boxSize + gap;

            var labelStyle = styleManager?.GetLabelStyle(config.IsDisabled ? ControlVariant.Muted : config.LabelVariant, config.Size, config.Appearance) ?? GUI.skin.label;
            var labelRect = new Rect(x, rowRect.y, Mathf.Max(0f, rowRect.xMax - x), rowRect.height);
            GUI.Label(labelRect, config.Label ?? string.Empty, labelStyle);

            bool value = HandleCheckboxInput(rowRect, config.Value, config.IsDisabled);
            DrawCheckboxVisual(boxRect, value, config.IsDisabled, theme);
            return value;
        }

        private bool HandleCheckboxInput(Rect rect, bool currentValue, bool disabled)
        {
            if (disabled)
                return currentValue;

            var evt = Event.current;
            if (evt.type == EventType.MouseDown && evt.button == 0 && rect.Contains(evt.mousePosition))
            {
                evt.Use();
                return !currentValue;
            }

            return currentValue;
        }

        private void DrawCheckboxVisual(Rect boxRect, bool value, bool disabled, Theme theme)
        {
            Color fill = value ? theme.ButtonPrimaryBg : theme.Base;
            Color border = value ? theme.ButtonPrimaryBg : theme.Border;
            if (disabled)
            {
                fill = Color.Lerp(fill, theme.Base, 0.35f);
                border = Color.Lerp(border, theme.Muted, 0.25f);
            }

            int radius = styleManager.GetScaledBorderRadius(DesignTokens.Radius.SM);
            GUI.DrawTexture(boxRect, styleManager.CreateBorderTexture(Mathf.RoundToInt(boxRect.width), Mathf.RoundToInt(boxRect.height), radius, fill, border, 1f), ScaleMode.StretchToFill);

            if (!value)
                return;

            var checkStyle = new UnityHelpers.GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = Mathf.RoundToInt(11f * guiHelper.uiScale),
                normal = { textColor = theme.ButtonPrimaryFg },
            };
            GUI.Label(boxRect, "✓", checkStyle);
        }
    }
}
