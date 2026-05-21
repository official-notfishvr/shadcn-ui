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
    public class Checkbox : BooleanControlBase
    {
        public Checkbox(GUIHelper helper)
            : base(helper) { }

        public bool Render(CheckboxConfig config)
        {
            if (config == null)
                return false;

            float rowHeight = DesignTokens.Checkbox.Size * guiHelper.uiScale + DesignTokens.Spacing.XS * guiHelper.uiScale;
            return RenderBoolControl(config, GUIStyle.none, rowHeight, DrawCheckboxRow);
        }

        private bool DrawCheckboxRow(Rect rowRect, BoolControlConfigBase config)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            float boxSize = DesignTokens.Checkbox.Size * guiHelper.uiScale;
            float gap = DesignTokens.Spacing.MD * guiHelper.uiScale;
            float x = DrawLeadingIcon(rowRect, config.Icon);

            var boxRect = new Rect(x, rowRect.y + (rowRect.height - boxSize) * 0.5f, boxSize, boxSize);
            x += boxSize + gap;

            var labelStyle = GetBooleanLabelStyle(config);
            var labelRect = new Rect(x, rowRect.y, Mathf.Max(0f, rowRect.xMax - x), rowRect.height);
            GUI.Label(labelRect, config.Label ?? string.Empty, labelStyle);

            bool value = HandleCheckboxInput(rowRect, config.Value, config.IsDisabled);
            DrawCheckboxVisual(boxRect, value, config.IsDisabled, theme);
            return value;
        }

        private bool HandleCheckboxInput(Rect rect, bool currentValue, bool disabled)
        {
            return base.HandleToggleInput(rect, currentValue, disabled);
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
            SurfaceDrawUtility.DrawRoundedBorder(styleManager, boxRect, radius, fill, border, 1f);

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
