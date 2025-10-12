using System;
using System.Collections.Generic;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#elif IL2CPP_BEPINEX
using Il2CppInterop.Runtime.InteropTypes.Arrays;
#endif

namespace shadcnui.GUIComponents.Layout
{
    public class Separator
    {
        private GUIHelper guiHelper;

        public Separator(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void DrawSeparator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options)
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

        public void HorizontalSeparator(params GUILayoutOption[] options)
        {
            DrawSeparator(SeparatorOrientation.Horizontal, true, options);
        }

        public void VerticalSeparator(params GUILayoutOption[] options)
        {
            DrawSeparator(SeparatorOrientation.Vertical, true, options);
        }

        public void DrawSeparator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(orientation);

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            GUI.Box(scaledRect, UnityHelpers.GUIContent.none, separatorStyle);
        }

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, float spacingBefore = 8f, float spacingAfter = 8f, params GUILayoutOption[] options)
        {
            if (spacingBefore > 0)
            {
                GUILayout.Space(spacingBefore * guiHelper.uiScale);
            }

            DrawSeparator(orientation, true, options);

            if (spacingAfter > 0)
            {
                GUILayout.Space(spacingAfter * guiHelper.uiScale);
            }
        }

        public void LabeledSeparator(string text, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

#if IL2CPP_BEPINEX || IL2CPP_MELONLOADER
            GUILayout.BeginHorizontal(new Il2CppReferenceArray<GUILayoutOption>(0));
#else
            GUILayout.BeginHorizontal();
#endif

            DrawSeparator(SeparatorOrientation.Horizontal, true, GUILayout.ExpandWidth(true));

            if (!string.IsNullOrEmpty(text))
            {
                GUILayout.Space(8 * guiHelper.uiScale);

                UnityHelpers.Label(text, styleManager.GetLabelStyle(LabelVariant.Muted));
                GUILayout.Space(8 * guiHelper.uiScale);
            }

            DrawSeparator(SeparatorOrientation.Horizontal, true, GUILayout.ExpandWidth(true));

            GUILayout.EndHorizontal();
        }
    }
}
