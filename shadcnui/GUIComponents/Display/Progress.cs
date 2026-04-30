using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Progress : BaseComponent
    {
        public Progress(GUIHelper helper)
            : base(helper) { }

        public void DrawProgress(ProgressConfig config)
        {
            if (config == null)
                return;

            if (!string.IsNullOrEmpty(config.Label))
            {
                layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default, ControlSize.Default, config.Appearance));
                if (config.ShowPercentage)
                {
                    GUILayout.FlexibleSpace();
                    UnityHelpers.Label($"{Mathf.Clamp01(config.Value) * 100f:0}%", styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Default, config.Appearance));
                }
                layoutComponents.EndHorizontalGroup();
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            Rect rect = config.Rect.HasValue ? ControlLayoutUtility.ScaleRect(config.Rect.Value, guiHelper.uiScale) : GetRect(config.Width, config.Height, config.LayoutOptions);
            DrawProgressBar(rect, config.Value, config.Variant, config.Appearance);
        }

        public void DrawProgress(float value, float width = -1f, float height = -1f, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Value = value,
                    Width = width,
                    Height = height,
                    Appearance = appearance,
                    LayoutOptions = options,
                }
            );
        }

        public void DrawProgress(Rect rect, float value, ComponentAppearance appearance = null)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Value = value,
                    Rect = rect,
                    Appearance = appearance,
                }
            );
        }

        public void LabeledProgress(string label, float value, float width = -1f, float height = -1f, bool showPercentage = true, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Label = label,
                    Value = value,
                    Width = width,
                    Height = height,
                    ShowPercentage = showPercentage,
                    Appearance = appearance,
                    LayoutOptions = options,
                }
            );
        }

        public void CircularProgress(float value, float size = 32f, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            float scaled = size * guiHelper.uiScale;
            Rect rect = SurfaceDrawUtility.ReserveSquare(scaled);
            DrawProgressBar(new Rect(rect.x, rect.y + rect.height * 0.45f, rect.width, Mathf.Max(4f, rect.height * 0.12f)), value, ControlVariant.Default, appearance);
            GUI.Label(rect, $"{Mathf.Clamp01(value) * 100f:0}%", styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small, appearance));
        }

        public void AnimatedProgress(string id, float value, float width = -1f, float height = -1f, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            var animation = guiHelper.GetAnimationManager();
            float current = animation.GetFloat(id, value);
            if (Mathf.Abs(current - value) > 0.001f)
                animation.StartFloat(id, current, value, DesignTokens.Animation.DurationNormal, EasingFunctions.EaseOutCubic);

            DrawProgress(
                new ProgressConfig
                {
                    Value = animation.GetFloat(id, value),
                    Width = width,
                    Height = height,
                    Appearance = appearance,
                    LayoutOptions = options,
                }
            );
        }

        private Rect GetRect(float width, float height, GUILayoutOption[] options)
        {
            float resolvedHeight = height > 0 ? height * guiHelper.uiScale : styleManager.GetProgressBarStyle().fixedHeight;
            if (options != null && options.Length > 0)
                return GUILayoutUtility.GetRect(0f, resolvedHeight, options);

            if (width > 0)
                return GUILayoutUtility.GetRect(width * guiHelper.uiScale, resolvedHeight, GUILayout.Width(width * guiHelper.uiScale), GUILayout.Height(resolvedHeight));

            return ControlLayoutUtility.ReserveRect(UnityHelpers.GUIContent.none, GUIStyle.none, ControlLayoutUtility.BuildLayoutOptions(null, fixedHeight: resolvedHeight, expandWidth: true));
        }

        private void DrawProgressBar(Rect rect, float value, ControlVariant variant, ComponentAppearance appearance)
        {
            value = Mathf.Clamp01(value);
            GUI.Box(rect, GUIContent.none, styleManager.GetProgressBarStyle(variant, ControlSize.Default, appearance));

            if (rect.width <= 0f || value <= 0f)
                return;

            float fillWidth = Mathf.Max(rect.height, rect.width * value);
            Rect fillRect = new Rect(rect.x, rect.y, Mathf.Min(rect.width, fillWidth), rect.height);
            var fillColor = styleManager.GetSliderFillColor(variant, false, appearance);
            SurfaceDrawUtility.DrawRoundedFill(styleManager, fillRect, fillColor, Mathf.RoundToInt(fillRect.height / 2f));
        }
    }
}
