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

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        #region Config-based API
        public void DrawLabel(LabelConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager.GetLabelStyle(config.Variant);

            GUIStyle scaledStyle = new UnityHelpers.GUIStyle(labelStyle);
            scaledStyle.fontSize = Mathf.RoundToInt(labelStyle.fontSize * guiHelper.uiScale);

            if (config.Disabled)
            {
                Color disabledColor = new Color(labelStyle.normal.textColor.r, labelStyle.normal.textColor.g, labelStyle.normal.textColor.b, 0.7f);
                scaledStyle.normal.textColor = disabledColor;
            }

            if (config.Icon?.Image != null)
            {
                if (config.Icon.Position == IconPosition.Left || config.Icon.Position == IconPosition.Right)
                {
                    GUILayout.BeginHorizontal();
                }
                else
                {
                    GUILayout.BeginVertical();
                }

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

                if (config.Rect.HasValue)
                {
                    Rect r = config.Rect.Value;
                    Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
                    GUI.Label(scaledRect, config.Text ?? "", scaledStyle);
                }
                else
                {
                    UnityHelpers.Label(config.Text ?? "", scaledStyle);
                }

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

                if (config.Icon.Position == IconPosition.Left || config.Icon.Position == IconPosition.Right)
                {
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.EndVertical();
                }
            }
            else
            {
                if (config.Rect.HasValue)
                {
                    Rect r = config.Rect.Value;
                    Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
                    GUI.Label(scaledRect, config.Text ?? "", scaledStyle);
                }
                else
                {
                    GUILayoutOption[] autoScaledOptions = GetAutoScaledOptions(config.Options);
                    UnityHelpers.Label(config.Text ?? "", scaledStyle, autoScaledOptions);
                }
            }
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
        #endregion

        #region Variant Shortcuts
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

        #region Internal Helpers
        private GUILayoutOption[] GetAutoScaledOptions(GUILayoutOption[] userOptions)
        {
            if (userOptions == null || userOptions.Length == 0)
                return new GUILayoutOption[0];

            return userOptions;
        }
        #endregion
    }
}
