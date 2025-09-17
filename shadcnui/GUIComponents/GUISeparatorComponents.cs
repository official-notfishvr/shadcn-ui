using System;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUISeparatorComponents
    {
        private GUIHelper guiHelper;

        public GUISeparatorComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void Separator(
            SeparatorOrientation orientation = SeparatorOrientation.Horizontal,
            bool decorative = true,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(orientation);

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();

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

#if IL2CPP
            GUILayout.Box(
                GUIContent.none,
                separatorStyle,
                (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray()
            );
#else
            GUILayout.Box(GUIContent.none, separatorStyle, layoutOptions.ToArray());
#endif
        }

        public void HorizontalSeparator(params GUILayoutOption[] options)
        {
            Separator(SeparatorOrientation.Horizontal, true, options);
        }

        public void VerticalSeparator(params GUILayoutOption[] options)
        {
            Separator(SeparatorOrientation.Vertical, true, options);
        }

        public void Separator(
            Rect rect,
            SeparatorOrientation orientation = SeparatorOrientation.Horizontal
        )
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(orientation);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            GUI.Box(scaledRect, GUIContent.none, separatorStyle);
        }

        public void SeparatorWithSpacing(
            SeparatorOrientation orientation = SeparatorOrientation.Horizontal,
            float spacingBefore = 8f,
            float spacingAfter = 8f,
            params GUILayoutOption[] options
        )
        {
            if (spacingBefore > 0)
            {
                if (orientation == SeparatorOrientation.Horizontal)
                    GUILayout.Space(spacingBefore * guiHelper.uiScale);
                else
                    GUILayout.Space(spacingBefore * guiHelper.uiScale);
            }

            Separator(orientation, true, options);

            if (spacingAfter > 0)
            {
                if (orientation == SeparatorOrientation.Horizontal)
                    GUILayout.Space(spacingAfter * guiHelper.uiScale);
                else
                    GUILayout.Space(spacingAfter * guiHelper.uiScale);
            }
        }

        public void LabeledSeparator(string text, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

#if IL2CPP
            GUILayout.BeginHorizontal(
                new UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.GUILayoutOption>(0)
            );
#else
            GUILayout.BeginHorizontal();
#endif

            Separator(SeparatorOrientation.Horizontal, true, GUILayout.ExpandWidth(true));

            if (!string.IsNullOrEmpty(text))
            {
                GUILayout.Space(8 * guiHelper.uiScale);
#if IL2CPP
                GUILayout.Label(
                    new GUIContent(text),
                    styleManager.GetLabelStyle(LabelVariant.Muted),
                    new UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.GUILayoutOption>(0)
                );
#else
                GUILayout.Label(text, styleManager.GetLabelStyle(LabelVariant.Muted));
#endif
                GUILayout.Space(8 * guiHelper.uiScale);
            }

            Separator(SeparatorOrientation.Horizontal, true, GUILayout.ExpandWidth(true));

            GUILayout.EndHorizontal();
        }
    }
}
