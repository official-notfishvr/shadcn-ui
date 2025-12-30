using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Progress : BaseComponent
    {
        private Dictionary<string, float> _lastValues = new Dictionary<string, float>();

        public Progress(GUIHelper helper)
            : base(helper) { }

        public void DrawProgress(ProgressConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            float targetValue = Mathf.Clamp01(config.Value);
            float displayValue = targetValue;

            if (!string.IsNullOrEmpty(config.Label))
            {
                layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default));
                if (config.ShowPercentage)
                {
                    GUILayout.FlexibleSpace();
                    string percentText = (displayValue * 100f).ToString("F0") + "%";
                    UnityHelpers.Label(percentText, styleManager.GetLabelStyle(ControlVariant.Muted));
                }
                layoutComponents.EndHorizontalGroup();
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }
            if (config.Rect.HasValue)
                DrawProgressRect(config.Rect.Value, displayValue);
            else
                DrawProgressLayout(displayValue, config.Width, config.Height, config.Options);
        }

        private void DrawProgressInternal(ProgressConfig config, string animId)
        {
            var styleManager = guiHelper.GetStyleManager();
            float targetValue = Mathf.Clamp01(config.Value);
            float displayValue = targetValue;

            if (!string.IsNullOrEmpty(animId))
            {
                var animManager = guiHelper.GetAnimationManager();
                if (!_lastValues.TryGetValue(animId, out float lastValue))
                    lastValue = 0f;
                if (Mathf.Abs(lastValue - targetValue) > 0.001f)
                {
                    animManager.StartFloat($"progress_{animId}", lastValue, targetValue, 0.3f, EasingFunctions.EaseOutCubic);
                    _lastValues[animId] = targetValue;
                }
                displayValue = animManager.GetFloat($"progress_{animId}", targetValue);
            }

            if (!string.IsNullOrEmpty(config.Label))
            {
                layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default));
                if (config.ShowPercentage)
                {
                    GUILayout.FlexibleSpace();
                    string percentText = (displayValue * 100f).ToString("F0") + "%";
                    UnityHelpers.Label(percentText, styleManager.GetLabelStyle(ControlVariant.Muted));
                }
                layoutComponents.EndHorizontalGroup();
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }
            if (config.Rect.HasValue)
                DrawProgressRect(config.Rect.Value, displayValue);
            else
                DrawProgressLayout(displayValue, config.Width, config.Height, config.Options);
        }

        private void DrawProgressLayout(float value, float width, float height, GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            float scaledHeight = height > 0 ? height * guiHelper.uiScale : DesignTokens.Spacing.SM * guiHelper.uiScale;
            float scaledWidth = width > 0 ? width * guiHelper.uiScale : -1;
            var layoutOptions = new List<GUILayoutOption> { GUILayout.Height(scaledHeight) };
            if (scaledWidth > 0)
                layoutOptions.Add(GUILayout.Width(scaledWidth));
            else
                layoutOptions.Add(GUILayout.ExpandWidth(true));
            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);
#if IL2CPP_MELONLOADER_PRE57
            Rect progressRect = GUILayoutUtility.GetRect(GUIContent.none, styleManager.GetProgressBarStyle(), (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray());
#else
            Rect progressRect = GUILayoutUtility.GetRect(GUIContent.none, styleManager.GetProgressBarStyle(), layoutOptions.ToArray());
#endif
            GUI.Box(progressRect, GUIContent.none, styleManager.GetProgressBarStyle(ControlVariant.Default, ControlSize.Default));
            if (value > 0)
            {
                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * value, progressRect.height);
                GUI.Box(fillRect, GUIContent.none, styleManager.GetProgressBarStyle(ControlVariant.Secondary, ControlSize.Default));
            }
        }

        private void DrawProgressRect(Rect rect, float value)
        {
            var styleManager = guiHelper.GetStyleManager();
            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);
            GUI.Box(scaledRect, GUIContent.none, styleManager.GetProgressBarStyle(ControlVariant.Default, ControlSize.Default));
            if (value > 0)
            {
                Rect fillRect = new Rect(scaledRect.x, scaledRect.y, scaledRect.width * value, scaledRect.height);
                GUI.Box(fillRect, GUIContent.none, styleManager.GetProgressBarStyle(ControlVariant.Secondary, ControlSize.Default));
            }
        }

        public void DrawProgress(float value, float width = -1, float height = -1, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Value = value,
                    Width = width,
                    Height = height,
                    Options = options,
                }
            );
        }

        public void DrawProgress(Rect rect, float value)
        {
            DrawProgress(new ProgressConfig { Value = value, Rect = rect });
        }

        public void AnimatedProgress(string id, float value, float width = -1, float height = -1, params GUILayoutOption[] options)
        {
            DrawProgressInternal(
                new ProgressConfig
                {
                    Value = value,
                    Width = width,
                    Height = height,
                    Options = options,
                },
                id
            );
        }

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Value = value,
                    Label = label,
                    Width = width,
                    Height = height,
                    ShowPercentage = showPercentage,
                    Options = options,
                }
            );
        }

        public void CircularProgress(float value, float size = DesignTokens.Height.Small, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Value = value,
                    Size = size,
                    Options = options,
                }
            );
        }
    }
}
