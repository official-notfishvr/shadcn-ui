using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Separator : BaseComponent
    {
        public Separator(GUIHelper helper)
            : base(helper) { }

        public void DrawSeparator(SeparatorConfig config)
        {
            config ??= new SeparatorConfig();

            if (config.SpacingBefore > 0f)
                layoutComponents.AddSpace(config.SpacingBefore);

            if (string.IsNullOrEmpty(config.Text) || config.Orientation == SeparatorOrientation.Vertical)
            {
                DrawLine(config);
            }
            else
            {
                DrawLabeledLine(config);
            }

            if (config.SpacingAfter > 0f)
                layoutComponents.AddSpace(config.SpacingAfter);
        }

        public void DrawSeparator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] opts)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Orientation = orientation,
                    IsDecorative = decorative,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        private void DrawLine(SeparatorConfig config)
        {
            var style = styleManager?.GetSeparatorStyle(config.Orientation, config.Variant, config.Size) ?? GUIStyle.none;

            if (config.Orientation == SeparatorOrientation.Horizontal)
            {
                UnityHelpers.Box(string.Empty, style, config.LayoutOptions);
            }
            else
            {
                UnityHelpers.Box(string.Empty, style, config.LayoutOptions);
            }
        }

        private void DrawLabeledLine(SeparatorConfig config)
        {
            var lineStyle = styleManager?.GetSeparatorStyle(SeparatorOrientation.Horizontal, config.Variant, config.Size) ?? GUIStyle.none;
            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small) ?? GUI.skin.label;
            var spacing = DesignTokens.Spacing.SM * guiHelper.uiScale;

            layoutComponents.BeginHorizontalGroup();

            UnityHelpers.Box(string.Empty, lineStyle, GUILayout.ExpandWidth(true));
            layoutComponents.AddSpace(spacing);
            UnityHelpers.Label(config.Text, labelStyle, GUILayout.ExpandWidth(false));
            layoutComponents.AddSpace(spacing);
            UnityHelpers.Box(string.Empty, lineStyle, GUILayout.ExpandWidth(true));

            layoutComponents.EndHorizontalGroup();
        }
    }
}
