using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
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

        #region Config-based API
        public void DrawProgress(ProgressConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            float targetValue = Mathf.Clamp01(config.Value);

            if (config.Size > 0)
            {
                DrawCircularProgressInternal(targetValue, config.Size, config.Options);
                return;
            }

            DrawLabelIfPresent(config, styleManager, targetValue);
            DrawProgressBar(config, styleManager, targetValue);
        }
        #endregion

        #region API
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
        #endregion

        #region Private Methods
        private void DrawCircularProgressInternal(float value, float size, GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            float scaledSize = size * guiHelper.uiScale;
            var layoutOptions = new List<GUILayoutOption> { GUILayout.Width(scaledSize), GUILayout.Height(scaledSize) };
            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

#if IL2CPP_MELONLOADER_PRE57
            Rect circleRect = GUILayoutUtility.GetRect(scaledSize, scaledSize, (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray());
#else
            Rect circleRect = GUILayoutUtility.GetRect(scaledSize, scaledSize, layoutOptions.ToArray());
#endif

            DrawCircularProgressContent(circleRect, value, scaledSize, styleManager);
        }

        private void DrawCircularProgressContent(Rect circleRect, float value, float scaledSize, StyleManager styleManager)
        {
            float thickness = Mathf.Max(2f, scaledSize * 0.1f);
            float radius = (scaledSize - thickness) * 0.5f;
            Vector2 center = new Vector2(circleRect.x + scaledSize * 0.5f, circleRect.y + scaledSize * 0.5f);

            Color bgColor = ThemeManager.Instance.CurrentTheme.Secondary;
            Color fgColor = ThemeManager.Instance.CurrentTheme.Accent;

            int segments = 36;
            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float startAngle = i * angleStep - 90f;
                float endAngle = startAngle + angleStep;
                float progress = (float)i / segments;
                Color segmentColor = progress < value ? fgColor : bgColor;
                DrawArcSegment(center, radius, startAngle, endAngle, thickness, segmentColor);
            }

            string percentText = (value * 100f).ToString("F0") + "%";
            GUIStyle centeredStyle = new UnityHelpers.GUIStyle(styleManager.GetLabelStyle(ControlVariant.Default));
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            centeredStyle.fontSize = Mathf.RoundToInt(scaledSize * 0.2f);
            GUI.Label(circleRect, percentText, centeredStyle);
        }

        private void DrawLabelIfPresent(ProgressConfig config, StyleManager styleManager, float displayValue)
        {
            if (string.IsNullOrEmpty(config.Label))
                return;

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

        private void DrawProgressBar(ProgressConfig config, StyleManager styleManager, float displayValue)
        {
            if (config.Rect.HasValue)
                DrawProgressRect(config.Rect.Value, displayValue, styleManager);
            else
                DrawProgressLayout(displayValue, config.Width, config.Height, config.Options, styleManager);
        }

        private void DrawProgressLayout(float value, float width, float height, GUILayoutOption[] options, StyleManager styleManager)
        {
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

            DrawProgressBars(progressRect, value, styleManager);
        }

        private void DrawProgressRect(Rect rect, float value, StyleManager styleManager)
        {
            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);
            DrawProgressBars(scaledRect, value, styleManager);
        }

        private void DrawProgressBars(Rect rect, float value, StyleManager styleManager)
        {
            GUI.Box(rect, GUIContent.none, styleManager.GetProgressBarStyle(ControlVariant.Default, ControlSize.Default));

            if (value > 0)
            {
                Rect fillRect = new Rect(rect.x, rect.y, rect.width * value, rect.height);
                GUI.Box(fillRect, GUIContent.none, styleManager.GetProgressBarStyle(ControlVariant.Secondary, ControlSize.Default));
            }
        }

        private void DrawProgressInternal(ProgressConfig config, string animId)
        {
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

            DrawProgress(
                new ProgressConfig
                {
                    Value = displayValue,
                    Label = config.Label,
                    Width = config.Width,
                    Height = config.Height,
                    ShowPercentage = config.ShowPercentage,
                    Options = config.Options,
                }
            );
        }

        private void DrawArcSegment(Vector2 center, float radius, float startAngle, float endAngle, float thickness, Color color)
        {
            float startRad = startAngle * Mathf.Deg2Rad;
            float endRad = endAngle * Mathf.Deg2Rad;

            Vector2 start = center + new Vector2(Mathf.Cos(startRad), Mathf.Sin(startRad)) * radius;
            Vector2 end = center + new Vector2(Mathf.Cos(endRad), Mathf.Sin(endRad)) * radius;

            DrawLine(start, end, thickness, color);
        }

        private void DrawLine(Vector2 start, Vector2 end, float thickness, Color color)
        {
            var styleManager = guiHelper.GetStyleManager();
            Texture2D tex = styleManager.CreateSolidTexture(color);

            Vector2 delta = end - start;
            float length = delta.magnitude;
            if (length < 0.001f)
                return;

            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

            Matrix4x4 prevMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y - thickness * 0.5f, length, thickness), tex);
            GUI.matrix = prevMatrix;
        }
        #endregion
    }
}
