using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
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
            float boxSize = DesignTokens.Checkbox.Size * guiHelper.uiScale;
            float gap = DesignTokens.Spacing.MD * guiHelper.uiScale;
            float x = DrawLeadingIcon(rowRect, config.Icon);

            var boxRect = new Rect(x, rowRect.y + (rowRect.height - boxSize) * 0.5f, boxSize, boxSize);
            x += boxSize + gap;

            var labelStyle = GetBooleanLabelStyle(config);
            var labelRect = new Rect(x, rowRect.y, Mathf.Max(0f, rowRect.xMax - x), rowRect.height);
            GUI.Label(labelRect, config.Label ?? string.Empty, labelStyle);

            bool value = HandleCheckboxInput(rowRect, config.Value, config.IsDisabled);
            DrawCheckboxVisual(boxRect, value, config);
            return value;
        }

        private bool HandleCheckboxInput(Rect rect, bool currentValue, bool disabled)
        {
            return base.HandleToggleInput(rect, currentValue, disabled);
        }

        private void DrawCheckboxVisual(Rect boxRect, bool value, BoolControlConfigBase config)
        {
            var style = value ? styleManager.GetCheckboxSolidStyle(config.Variant, config.Size, config.Appearance) : styleManager.GetCheckboxStyle(config.Variant, config.Size, config.Appearance);

            if (Event.current.type == EventType.Repaint && style?.normal?.background != null)
                GUI.DrawTexture(boxRect, style.normal.background, ScaleMode.StretchToFill);

            if (!value)
                return;

            var checkStyle = new UnityHelpers.GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = Mathf.RoundToInt(11f * guiHelper.uiScale),
                normal = { textColor = style?.normal?.textColor ?? styleManager.GetTheme().ButtonPrimaryFg },
            };
            GUI.Label(boxRect, "✓", checkStyle);
        }
    }
}
