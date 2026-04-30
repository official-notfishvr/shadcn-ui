using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Label : BaseComponent
    {
        public Label(GUIHelper helper)
            : base(helper) { }

        public void DrawLabel(LabelConfig config)
        {
            if (config == null)
                return;

            GUIStyle style = styleManager?.GetLabelStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.label;
            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            if (config.Rect.HasValue)
            {
                Rect rect = ControlLayoutUtility.ScaleRect(config.Rect.Value, guiHelper.uiScale);
                DrawLabelContent(rect, config, style);
            }
            else
            {
                DrawLabelLayout(config, style);
            }

            GUI.enabled = prevEnabled;
        }

        public void DrawLabel(string text, ControlVariant variant = ControlVariant.Default, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            DrawLabel(
                new LabelConfig
                {
                    Text = text,
                    Variant = variant,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void DrawLabel(Rect rect, string text, ControlVariant variant = ControlVariant.Default, bool disabled = false)
        {
            DrawLabel(
                new LabelConfig
                {
                    Text = text,
                    Variant = variant,
                    IsDisabled = disabled,
                    Rect = rect,
                }
            );
        }

        private void DrawLabelLayout(LabelConfig config, GUIStyle style)
        {
            if (config.Icon?.Image == null)
            {
                UnityHelpers.Label(config.Text ?? string.Empty, style, config.LayoutOptions);
                return;
            }

            DrawWithIcon(config, style);
        }

        private void DrawLabelContent(Rect rect, LabelConfig config, GUIStyle style)
        {
            if (config.Icon?.Image == null)
            {
                GUI.Label(rect, config.Text ?? string.Empty, style);
                return;
            }

            GUI.BeginGroup(rect);
            DrawWithIcon(
                new LabelConfig
                {
                    Text = config.Text,
                    Icon = config.Icon,
                    Variant = config.Variant,
                    Size = config.Size,
                },
                style
            );
            GUI.EndGroup();
        }

        private void DrawWithIcon(LabelConfig config, GUIStyle style)
        {
            var icon = config.Icon;
            bool horizontal = icon.Position == IconPosition.Left || icon.Position == IconPosition.Right;

            if (horizontal)
                layoutComponents.BeginHorizontalGroup();
            else
                layoutComponents.BeginVerticalGroup();

            if (icon.Position == IconPosition.Above)
            {
                DrawIcon(icon);
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
            }

            if (icon.Position == IconPosition.Left)
            {
                DrawIcon(icon);
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
            }

            UnityHelpers.Label(config.Text ?? string.Empty, style, config.LayoutOptions);

            if (icon.Position == IconPosition.Right)
            {
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
                DrawIcon(icon);
            }

            if (icon.Position == IconPosition.Below)
            {
                layoutComponents.AddSpace(icon.Spacing * guiHelper.uiScale);
                DrawIcon(icon);
            }

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();
        }

        private void DrawIcon(IconConfig icon)
        {
            if (icon?.Image == null)
                return;

            float size = icon.Size * guiHelper.uiScale;
            UnityHelpers.Label(icon.Image, GUILayout.Width(size), GUILayout.Height(size));
        }
    }
}
