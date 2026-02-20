using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#elif IL2CPP_BEPINEX
using Il2CppInterop.Runtime.InteropTypes.Arrays;
#endif

namespace shadcnui.GUIComponents.Layout
{
    public class Separator : BaseComponent
    {
        public Separator(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public void DrawSeparator(SeparatorConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (config.SpacingBefore > 0)
                GUILayout.Space(config.SpacingBefore * guiHelper.uiScale);

            if (!string.IsNullOrEmpty(config.Text))
                DrawLabeledSeparator(config, styleManager);
            else if (config.Rect.HasValue)
                DrawSeparatorRect(config.Rect.Value, config.Orientation);
            else
                DrawSeparatorInternal(config.Orientation, config.Options);

            if (config.SpacingAfter > 0)
                GUILayout.Space(config.SpacingAfter * guiHelper.uiScale);
        }
        #endregion

        #region API
        public void DrawSeparator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Orientation = orientation,
                    Decorative = decorative,
                    SpacingBefore = 0,
                    SpacingAfter = 0,
                    Options = options,
                }
            );
        }

        public void HorizontalSeparator(params GUILayoutOption[] options)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Orientation = SeparatorOrientation.Horizontal,
                    SpacingBefore = 0,
                    SpacingAfter = 0,
                    Options = options,
                }
            );
        }

        public void VerticalSeparator(params GUILayoutOption[] options)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Orientation = SeparatorOrientation.Vertical,
                    SpacingBefore = 0,
                    SpacingAfter = 0,
                    Options = options,
                }
            );
        }

        public void DrawSeparator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Rect = rect,
                    Orientation = orientation,
                    SpacingBefore = 0,
                    SpacingAfter = 0,
                }
            );
        }

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, float spacingBefore = DesignTokens.Spacing.SM, float spacingAfter = DesignTokens.Spacing.SM, params GUILayoutOption[] options)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Orientation = orientation,
                    SpacingBefore = spacingBefore,
                    SpacingAfter = spacingAfter,
                    Options = options,
                }
            );
        }

        public void LabeledSeparator(string text, params GUILayoutOption[] options)
        {
            DrawSeparator(
                new SeparatorConfig
                {
                    Text = text,
                    SpacingBefore = 0,
                    SpacingAfter = 0,
                    Options = options,
                }
            );
        }
        #endregion

        #region Private Methods
        private void DrawSeparatorInternal(SeparatorOrientation orientation, GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(orientation);

            var layoutOptions = new List<GUILayoutOption>();

            if (orientation == SeparatorOrientation.Horizontal)
            {
                layoutOptions.Add(GUILayout.Height(Mathf.RoundToInt(1 * guiHelper.uiScale)));
                layoutOptions.Add(GUILayout.ExpandWidth(true));
            }
            else
            {
                layoutOptions.Add(GUILayout.Width(Mathf.RoundToInt(1 * guiHelper.uiScale)));
                layoutOptions.Add(GUILayout.ExpandHeight(true));
            }

            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

            UnityHelpers.Box(UnityHelpers.GUIContent.none, separatorStyle, layoutOptions.ToArray());
        }

        private void DrawSeparatorRect(Rect rect, SeparatorOrientation orientation)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(orientation);
            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);
            GUI.Box(scaledRect, UnityHelpers.GUIContent.none, separatorStyle);
        }

        private void DrawLabeledSeparator(SeparatorConfig config, StyleManager styleManager)
        {
#if IL2CPP_BEPINEX || IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginHorizontal(new Il2CppReferenceArray<GUILayoutOption>(0));
#else
            GUILayout.BeginHorizontal();
#endif
            DrawSeparatorInternal(SeparatorOrientation.Horizontal, config.Options);
            GUILayout.Space(DesignTokens.Spacing.SM * guiHelper.uiScale);
            UnityHelpers.Label(config.Text, styleManager.GetLabelStyle(ControlVariant.Muted));
            GUILayout.Space(DesignTokens.Spacing.SM * guiHelper.uiScale);
            DrawSeparatorInternal(SeparatorOrientation.Horizontal, config.Options);
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
