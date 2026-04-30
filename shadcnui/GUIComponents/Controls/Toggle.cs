using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Toggle : BooleanControlBase
    {
        public Toggle(GUIHelper helper)
            : base(helper) { }

        public bool Draw(ToggleConfig config)
        {
            if (config == null)
                return false;

            GUIStyle toggleStyle = styleManager?.GetToggleStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.button;
            float minHeight = toggleStyle.fixedHeight > 0f ? toggleStyle.fixedHeight : 0f;
            return RenderBoolControl(config, toggleStyle, minHeight, (rect, cfg) => DrawToggleButton(rect, cfg, toggleStyle));
        }

        private bool DrawToggleButton(Rect rect, BoolControlConfigBase config, GUIStyle style)
        {
            bool next = GUI.Toggle(rect, config.Value, GUIContent.none, style);
            DrawToggleContent(rect, config, style);
            return next;
        }

        private void DrawToggleContent(Rect rect, BoolControlConfigBase config, GUIStyle style)
        {
            ContentRenderUtility.DrawCenteredContent(rect, style, config.Label ?? string.Empty, config.Icon?.Image, IconPosition.Left, config.Icon?.Size * guiHelper.uiScale ?? 0f, config.Icon?.Spacing * guiHelper.uiScale ?? DesignTokens.Spacing.XS * guiHelper.uiScale);
        }
    }
}
