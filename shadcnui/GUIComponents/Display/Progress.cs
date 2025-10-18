using System;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Progress : BaseComponent
    {
        public Progress(GUIHelper helper) : base(helper) { }
        public void DrawProgress(float value, float width = -1, float height = -1, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            value = Mathf.Clamp01(value);

            float scaledHeight = height > 0 ? height * guiHelper.uiScale : 8 * guiHelper.uiScale;
            float scaledWidth = width > 0 ? width * guiHelper.uiScale : -1;

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.Height(scaledHeight));
            if (scaledWidth > 0)
                layoutOptions.Add(GUILayout.Width(scaledWidth));
            else
                layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

#if IL2CPP_MELONLOADER
            Rect progressRect = GUILayoutUtility.GetRect(GUIContent.none, styleManager.GetProgressBarStyle(), (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray());
#else
            Rect progressRect = GUILayoutUtility.GetRect(GUIContent.none, styleManager.GetProgressBarStyle(), layoutOptions.ToArray());
#endif

            GUI.Box(progressRect, GUIContent.none, styleManager.GetProgressBarBackgroundStyle());

            if (value > 0)
            {
                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * value, progressRect.height);

                GUI.Box(fillRect, GUIContent.none, styleManager.GetProgressBarFillStyle());
            }
        }

        public void DrawProgress(Rect rect, float value)
        {
            var styleManager = guiHelper.GetStyleManager();

            value = Mathf.Clamp01(value);

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            GUI.Box(scaledRect, GUIContent.none, styleManager.GetProgressBarBackgroundStyle());

            if (value > 0)
            {
                Rect fillRect = new Rect(scaledRect.x, scaledRect.y, scaledRect.width * value, scaledRect.height);

                GUI.Box(fillRect, GUIContent.none, styleManager.GetProgressBarFillStyle());
            }
        }

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (!string.IsNullOrEmpty(label))
            {
                layoutComponents.BeginHorizontalGroup();

                UnityHelpers.Label(label, styleManager.GetLabelStyle(LabelVariant.Default));

                if (showPercentage)
                {
                    GUILayout.FlexibleSpace();
                    string percentText = (value * 100f).ToString("F0") + "%";

                    UnityHelpers.Label(percentText, styleManager.GetLabelStyle(LabelVariant.Muted));
                }

                layoutComponents.EndHorizontalGroup();
                layoutComponents.AddSpace(4);
            }

            DrawProgress(value, width, height, options);
        }

        public void CircularProgress(float value, float size = 32f, params GUILayoutOption[] options)
        {
            value = Mathf.Clamp01(value);

            float scaledSize = size * guiHelper.uiScale;

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.Width(scaledSize));
            layoutOptions.Add(GUILayout.Height(scaledSize));

            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

#if IL2CPP_MELONLOADER
            Rect circleRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray());
#else
            Rect circleRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, layoutOptions.ToArray());
#endif

            var styleManager = guiHelper.GetStyleManager();

            GUI.Box(circleRect, GUIContent.none, styleManager.GetProgressBarBackgroundStyle());

            if (value > 0)
            {
                int segments = Mathf.RoundToInt(value * 8);
                for (int i = 0; i < segments; i++)
                {
                    float angle = (i / 8f) * 360f;

                    Rect segmentRect = new Rect(circleRect.x + (circleRect.width * 0.1f), circleRect.y + (circleRect.height * 0.1f), circleRect.width * 0.8f, circleRect.height * 0.8f);

                    if (i < segments)
                    {
                        GUI.Box(segmentRect, GUIContent.none, styleManager.GetProgressBarFillStyle());
                    }
                }
            }
        }
    }
}
