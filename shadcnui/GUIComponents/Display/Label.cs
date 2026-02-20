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

        #region Config-based API
        public void DrawLabel(LabelConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = ApplyLabelStyling(styleManager.GetLabelStyle(config.Variant), config);

            if (config.Icon?.Image != null)
                DrawLabelWithIcon(config, labelStyle);
            else
                DrawBasicLabel(config, labelStyle);
        }
        #endregion

        #region API
        public void DrawLabel(string text, ControlVariant variant = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] options)
        {
            DrawLabel(
                new LabelConfig
                {
                    Text = text,
                    Variant = variant,
                    Disabled = disabled,
                    Options = options,
                }
            );
        }

        public void DrawLabel(Rect rect, string text, ControlVariant variant = ControlVariant.Default, bool disabled = false)
        {
            DrawLabel(
                new LabelConfig
                {
                    Rect = rect,
                    Text = text,
                    Variant = variant,
                    Disabled = disabled,
                }
            );
        }

        public void SecondaryLabel(string text, params GUILayoutOption[] options)
        {
            DrawLabel(text, ControlVariant.Secondary, false, options);
        }

        public void MutedLabel(string text, params GUILayoutOption[] options)
        {
            DrawLabel(text, ControlVariant.Muted, false, options);
        }

        public void DestructiveLabel(string text, params GUILayoutOption[] options)
        {
            DrawLabel(text, ControlVariant.Destructive, false, options);
        }
        #endregion

        #region Private Methods
        private GUIStyle ApplyLabelStyling(GUIStyle baseStyle, LabelConfig config)
        {
            GUIStyle scaledStyle = new UnityHelpers.GUIStyle(baseStyle);
            scaledStyle.fontSize = Mathf.RoundToInt(baseStyle.fontSize * guiHelper.uiScale);

            if (config.Disabled)
            {
                Color disabledColor = new Color(baseStyle.normal.textColor.r, baseStyle.normal.textColor.g, baseStyle.normal.textColor.b, 0.7f);
                scaledStyle.normal.textColor = disabledColor;
            }

            return scaledStyle;
        }

        private void DrawLabelWithIcon(LabelConfig config, GUIStyle labelStyle)
        {
            bool isHorizontal = config.Icon.Position == IconPosition.Left || config.Icon.Position == IconPosition.Right;

            if (isHorizontal)
                GUILayout.BeginHorizontal();
            else
                GUILayout.BeginVertical();

            if (config.Icon.Position == IconPosition.Above)
            {
                RenderIcon(config.Icon);
                GUILayout.Space(config.Icon.Spacing * guiHelper.uiScale);
            }

            if (config.Icon.Position == IconPosition.Left)
            {
                RenderIcon(config.Icon);
                GUILayout.Space(config.Icon.Spacing * guiHelper.uiScale);
            }

            DrawLabelContent(config, labelStyle);

            if (config.Icon.Position == IconPosition.Right)
            {
                GUILayout.Space(config.Icon.Spacing * guiHelper.uiScale);
                RenderIcon(config.Icon);
            }

            if (config.Icon.Position == IconPosition.Below)
            {
                GUILayout.Space(config.Icon.Spacing * guiHelper.uiScale);
                RenderIcon(config.Icon);
            }

            if (isHorizontal)
                GUILayout.EndHorizontal();
            else
                GUILayout.EndVertical();
        }

        private void DrawBasicLabel(LabelConfig config, GUIStyle labelStyle)
        {
            DrawLabelContent(config, labelStyle);
        }

        private void DrawLabelContent(LabelConfig config, GUIStyle labelStyle)
        {
            if (config.Rect.HasValue)
            {
                Rect scaledRect = new Rect(config.Rect.Value.x * guiHelper.uiScale, config.Rect.Value.y * guiHelper.uiScale, config.Rect.Value.width * guiHelper.uiScale, config.Rect.Value.height * guiHelper.uiScale);
                GUI.Label(scaledRect, config.Text ?? "", labelStyle);
            }
            else
            {
                GUILayoutOption[] options = config.Options ?? System.Array.Empty<GUILayoutOption>();
                UnityHelpers.Label(config.Text ?? "", labelStyle, options);
            }
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }
        #endregion
    }
}
