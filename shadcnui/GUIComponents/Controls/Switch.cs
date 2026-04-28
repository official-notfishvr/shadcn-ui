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
    public class Switch : BaseComponent
    {
        public Switch(GUIHelper helper)
            : base(helper) { }

        public bool Draw(SwitchConfig config)
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
            float rowHeight = Mathf.Max(GetTrackHeight(config.Size), GetThumbSize(config.Size)) + DesignTokens.Spacing.XS * guiHelper.uiScale;
            var rowRect = GUILayoutUtility.GetRect(new UnityHelpers.GUIContent(config.Label ?? string.Empty), GUIStyle.none, options.ToArray());
            rowRect.height = Mathf.Max(rowRect.height, rowHeight);
            return DrawSwitchRow(rowRect, config);
        }

        private bool DrawRect(BoolControlConfigBase config)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return DrawSwitchRow(scaledRect, config);
        }

        private List<GUILayoutOption> BuildLayoutOptions(BoolControlConfigBase config)
        {
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
            if (config.FullRowClick)
                options.Add(GUILayout.ExpandWidth(true));
            return options;
        }

        private bool DrawSwitchRow(Rect rowRect, BoolControlConfigBase config)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            float trackWidth = GetTrackWidth(config.Size);
            float trackHeight = GetTrackHeight(config.Size);
            float thumbSize = GetThumbSize(config.Size);
            float gap = DesignTokens.Spacing.MD * guiHelper.uiScale;
            float iconSize = config.Icon?.Image != null ? config.Icon.Size * guiHelper.uiScale : 0f;
            float iconGap = config.Icon?.Image != null ? config.Icon.Spacing * guiHelper.uiScale : 0f;

            var labelStyle = styleManager?.GetLabelStyle(config.IsDisabled ? ControlVariant.Muted : config.LabelVariant, config.Size, config.Appearance) ?? GUI.skin.label;
            var text = config.Label ?? string.Empty;
            var labelContent = new UnityHelpers.GUIContent(text);
            var labelSize = labelStyle.CalcSize(labelContent);

            float trackX = rowRect.xMax - trackWidth;
            float trackY = rowRect.y + (rowRect.height - trackHeight) * 0.5f;
            var trackRect = new Rect(trackX, trackY, trackWidth, trackHeight);

            float contentX = rowRect.x;
            if (config.Icon?.Image != null)
            {
                var iconRect = new Rect(contentX, rowRect.y + (rowRect.height - iconSize) * 0.5f, iconSize, iconSize);
                GUI.DrawTexture(iconRect, config.Icon.Image, ScaleMode.ScaleToFit);
                contentX += iconSize + iconGap;
            }

            float labelWidth = Mathf.Max(0f, trackRect.x - gap - contentX);
            var labelRect = new Rect(contentX, rowRect.y, labelWidth, rowRect.height);
            GUI.Label(labelRect, text, labelStyle);

            bool toggled = HandleToggleInput(rowRect, config.Value, config.IsDisabled);
            DrawSwitchVisual(trackRect, toggled, config.IsDisabled, theme);
            return toggled;
        }

        private bool HandleToggleInput(Rect rect, bool currentValue, bool disabled)
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

        private void DrawSwitchVisual(Rect trackRect, bool value, bool disabled, Theme theme)
        {
            float thumbSize = Mathf.Max(1f, trackRect.height - 4f * guiHelper.uiScale);
            float thumbX = value ? trackRect.xMax - thumbSize - 2f * guiHelper.uiScale : trackRect.x + 2f * guiHelper.uiScale;
            var thumbRect = new Rect(thumbX, trackRect.y + (trackRect.height - thumbSize) * 0.5f, thumbSize, thumbSize);

            Color trackColor = value ? theme.ButtonPrimaryBg : theme.Border;
            if (!value)
                trackColor = theme.Secondary;
            if (disabled)
                trackColor = Color.Lerp(trackColor, theme.Base, 0.35f);

            Color thumbColor = theme.Base;
            if (disabled)
                thumbColor = Color.Lerp(thumbColor, theme.Muted, 0.3f);

            int trackRadius = Mathf.RoundToInt(trackRect.height * 0.5f);
            int thumbRadius = Mathf.RoundToInt(thumbRect.height * 0.5f);
            GUI.DrawTexture(trackRect, styleManager.CreateTexture(Mathf.RoundToInt(trackRect.width), Mathf.RoundToInt(trackRect.height), trackRadius, trackColor), ScaleMode.StretchToFill);
            GUI.DrawTexture(
                thumbRect,
                styleManager.CreateBorderTexture(Mathf.RoundToInt(thumbRect.width), Mathf.RoundToInt(thumbRect.height), thumbRadius, thumbColor, Color.clear, 0f, disabled ? 0f : 0.04f, Mathf.RoundToInt(DesignTokens.Effects.ShadowBlurSM * guiHelper.uiScale), theme.Shadow),
                ScaleMode.StretchToFill
            );
        }

        private float GetTrackWidth(ControlSize size)
        {
            return size switch
            {
                ControlSize.Small => 32f * guiHelper.uiScale,
                ControlSize.Large => 40f * guiHelper.uiScale,
                _ => DesignTokens.Switch.Width * guiHelper.uiScale,
            };
        }

        private float GetTrackHeight(ControlSize size)
        {
            return size switch
            {
                ControlSize.Small => 18f * guiHelper.uiScale,
                ControlSize.Large => 22f * guiHelper.uiScale,
                _ => DesignTokens.Switch.Height * guiHelper.uiScale,
            };
        }

        private float GetThumbSize(ControlSize size)
        {
            return size switch
            {
                ControlSize.Small => 14f * guiHelper.uiScale,
                ControlSize.Large => 18f * guiHelper.uiScale,
                _ => 16f * guiHelper.uiScale,
            };
        }
    }
}
