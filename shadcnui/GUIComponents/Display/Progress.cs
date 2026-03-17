using System;
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
                UnityHelpers.Label(config.Label, styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label);

            if (config.Rect.HasValue)
            {
                DrawProgressRect(config.Rect.Value, config.Value, config);
                return;
            }

            float width = config.Width > 0 ? config.Width * guiHelper.uiScale : 240f * guiHelper.uiScale;
            float height = config.Height > 0 ? config.Height * guiHelper.uiScale : 10f * guiHelper.uiScale;
            Rect rect = GUILayoutUtility.GetRect(width, height, (config.LayoutOptions != null && config.LayoutOptions.Length > 0) ? config.LayoutOptions : new[] { GUILayout.Width(width), GUILayout.Height(height) });
            DrawProgressRect(rect, config.Value, config);

            if (config.ShowPercentage)
            {
                layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                UnityHelpers.Label($"{Mathf.Clamp01(config.Value) * 100f:0}%", styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Default) ?? GUI.skin.label);
                layoutComponents.EndHorizontalGroup();
            }
        }

        public void DrawProgress(float value, float width = -1f, float height = -1f, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Value = value,
                    Width = width,
                    Height = height,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void DrawProgress(Rect rect, float value)
        {
            DrawProgressRect(rect, value, new ProgressConfig());
        }

        public void LabeledProgress(string label, float value, float width = -1f, float height = -1f, bool showPercentage = true, params GUILayoutOption[] options)
        {
            DrawProgress(
                new ProgressConfig
                {
                    Label = label,
                    Value = value,
                    Width = width,
                    Height = height,
                    ShowPercentage = showPercentage,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void CircularProgress(float value, float size = DesignTokens.Height.Small, params GUILayoutOption[] options)
        {
            float px = size * guiHelper.uiScale;
            Rect rect = GUILayoutUtility.GetRect(px, px, GUILayout.Width(px), GUILayout.Height(px));
            GUIStyle style = styleManager?.GetProgressBarStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.box;
            GUI.Box(rect, GUIContent.none, style);

            string text = $"{Mathf.Clamp01(value) * 100f:0}%";
            var labelStyle = new UnityHelpers.GUIStyle(styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            GUI.Label(rect, text, labelStyle);
        }

        public void AnimatedProgress(string id, float value, float width = -1f, float height = -1f, params GUILayoutOption[] options)
        {
            var anim = guiHelper.GetAnimationManager();
            if (!anim.Exists(id))
                anim.StartFloat(id, value, value, 0f);

            float current = anim.GetFloat(id, value);
            if (Mathf.Abs(current - value) > 0.001f)
                anim.StartFloat(id, current, value, DesignTokens.Animation.DurationFast, EasingFunctions.EaseOutCubic);

            DrawProgress(current, width, height, options);
        }

        private void DrawProgressRect(Rect rect, float value, ProgressConfig config)
        {
            Rect scaled = config.Rect.HasValue ? ScaleRect(rect) : rect;
            float clamped = Mathf.Clamp01(value);

            GUIStyle trackStyle = styleManager?.GetProgressBarStyle(config.Variant, ControlSize.Default) ?? GUI.skin.box;
            GUI.Box(scaled, GUIContent.none, trackStyle);

            var fillColor = styleManager?.GetTheme()?.Accent ?? new Color(0.2f, 0.6f, 1f);
            Rect fill = new Rect(scaled.x, scaled.y, scaled.width * clamped, scaled.height);

            Color prev = GUI.color;
            GUI.color = fillColor;
            GUI.DrawTexture(fill, Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private Rect ScaleRect(Rect rect)
        {
            return new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);
        }
    }
}
