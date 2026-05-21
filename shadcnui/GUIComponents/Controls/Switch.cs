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
    public class Switch : BooleanControlBase
    {
        public Switch(GUIHelper helper)
            : base(helper) { }

        public bool Render(SwitchConfig config)
        {
            if (config == null)
                return false;

            float rowHeight = Mathf.Max(GetTrackHeight(config.Size), GetThumbSize(config.Size)) + DesignTokens.Spacing.XS * guiHelper.uiScale;
            return RenderBoolControl(config, GUIStyle.none, rowHeight, DrawSwitchRow);
        }

        private bool DrawSwitchRow(Rect rowRect, BoolControlConfigBase config)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            float trackWidth = GetTrackWidth(config.Size);
            float trackHeight = GetTrackHeight(config.Size);
            float gap = DesignTokens.Spacing.MD * guiHelper.uiScale;
            var labelStyle = GetBooleanLabelStyle(config);
            var text = config.Label ?? string.Empty;

            float trackX = rowRect.xMax - trackWidth;
            float trackY = rowRect.y + (rowRect.height - trackHeight) * 0.5f;
            var trackRect = new Rect(trackX, trackY, trackWidth, trackHeight);

            float contentX = DrawLeadingIcon(rowRect, config.Icon);

            float labelWidth = Mathf.Max(0f, trackRect.x - gap - contentX);
            var labelRect = new Rect(contentX, rowRect.y, labelWidth, rowRect.height);
            GUI.Label(labelRect, text, labelStyle);

            bool toggled = base.HandleToggleInput(rowRect, config.Value, config.IsDisabled);
            DrawSwitchVisual(trackRect, toggled, config.IsDisabled, theme);
            return toggled;
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
            SurfaceDrawUtility.DrawRoundedFill(styleManager, trackRect, trackColor, trackRadius);
            SurfaceDrawUtility.DrawRoundedBorder(styleManager, thumbRect, thumbRadius, thumbColor, Color.clear, 0f, disabled ? 0f : 0.04f, Mathf.RoundToInt(DesignTokens.Effects.ShadowBlurSM * guiHelper.uiScale), theme.Shadow);
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
