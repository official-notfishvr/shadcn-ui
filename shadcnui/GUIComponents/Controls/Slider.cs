using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Slider : BaseComponent
    {
        private int _activeControlId = -1;
        private bool _isDragging;

        public Slider(GUIHelper helper)
            : base(helper) { }

        public float Draw(SliderConfig config)
        {
            if (config == null)
                return 0f;

            try
            {
                float result = config.Value;
                bool prevEnabled = GUI.enabled;

                if (config.IsDisabled)
                    GUI.enabled = false;

                DrawSliderLabel(config);
                result = DrawSliderTrack(config);

                GUI.enabled = prevEnabled;

                if (!Mathf.Approximately(result, config.Value) && !config.IsDisabled)
                    config.OnValueChanged?.Invoke(result);

                return config.IsDisabled ? config.Value : result;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(Draw), nameof(Slider));
                return config.Value;
            }
        }

        public static float CalculateValue(float normalizedPosition, float min, float max)
        {
            normalizedPosition = Mathf.Clamp01(normalizedPosition);
            float rawValue = min + normalizedPosition * (max - min);
            return Mathf.Clamp(rawValue, min, max);
        }

        public static float SnapToStep(float value, float step, float min, float max)
        {
            if (step <= 0f)
                return Mathf.Clamp(value, min, max);
            float snapped = Mathf.Round((value - min) / step) * step + min;
            return Mathf.Clamp(snapped, min, max);
        }

        public static float ValueToNormalized(float value, float min, float max)
        {
            if (Mathf.Approximately(max, min))
                return 0f;
            return Mathf.Clamp01((value - min) / (max - min));
        }

        private void DrawSliderLabel(SliderConfig config)
        {
            if (string.IsNullOrEmpty(config.Label))
                return;

            GUILayout.BeginHorizontal();
            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default, config.Size) ?? GUI.skin.label;
            UnityHelpers.Label(config.Label, labelStyle);

            if (config.ShowValue)
            {
                GUILayout.FlexibleSpace();
                string valueText = config.Value.ToString(config.ValueFormat);
                var mutedStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size) ?? GUI.skin.label;
                UnityHelpers.Label(valueText, mutedStyle);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(DesignTokens.Spacing.XS * guiHelper.uiScale);
        }

        private float DrawSliderTrack(SliderConfig config)
        {
            float trackHeight = styleManager.GetSliderTrackHeight(config.Size);
            float thumbSize = styleManager.GetSliderThumbSize(config.Size);
            float totalHeight = Mathf.Max(trackHeight, thumbSize);

            Rect sliderRect = GetSliderRect(config, totalHeight);

            if (Event.current.type != EventType.Repaint)
            {
                Rect trackRect = new Rect(sliderRect.x + thumbSize / 2f, sliderRect.y, sliderRect.width - thumbSize, trackHeight);
                return HandleSliderInput(sliderRect, trackRect, config);
            }

            return DrawSliderRepaint(sliderRect, config, trackHeight, thumbSize, totalHeight);
        }

        private Rect GetSliderRect(SliderConfig config, float totalHeight)
        {
            if (config.LayoutOptions != null && config.LayoutOptions.Length > 0)
                return GUILayoutUtility.GetRect(100f * guiHelper.uiScale, totalHeight, config.LayoutOptions);
            return GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.horizontalSlider, GUILayout.Height(totalHeight), GUILayout.ExpandWidth(true));
        }

        private float DrawSliderRepaint(Rect sliderRect, SliderConfig config, float trackHeight, float thumbSize, float totalHeight)
        {
            float trackY = sliderRect.y + (totalHeight - trackHeight) / 2f;
            Rect trackRect = new Rect(sliderRect.x + thumbSize / 2f, trackY, sliderRect.width - thumbSize, trackHeight);

            Color trackColor = styleManager.GetSliderTrackColor(config.Variant, config.IsDisabled, config.Appearance);
            Color fillColor = styleManager.GetSliderFillColor(config.Variant, config.IsDisabled, config.Appearance);
            Color thumbColor = styleManager.GetSliderThumbColor(config.Variant, config.IsDisabled, config.Appearance);

            float normalizedValue = ValueToNormalized(config.Value, config.MinValue, config.MaxValue);
            float fillWidth = trackRect.width * normalizedValue;
            int trackRadius = Mathf.RoundToInt(trackHeight / 2f);

            DrawCachedRoundedRect(trackRect, trackColor, trackRadius);

            if (fillWidth > 1)
            {
                Rect fillRect = new Rect(trackRect.x, trackRect.y, fillWidth, trackHeight);
                DrawCachedRoundedRect(fillRect, fillColor, trackRadius);
            }

            float thumbX = trackRect.x + fillWidth - thumbSize / 2f;
            float thumbY = sliderRect.y + (totalHeight - thumbSize) / 2f;
            Rect thumbRect = new Rect(thumbX, thumbY, thumbSize, thumbSize);

            DrawCachedThumb(thumbRect, thumbColor, config.IsDisabled);

            return config.Value;
        }

        private float HandleSliderInput(Rect sliderRect, Rect trackRect, SliderConfig config)
        {
            if (config.IsDisabled)
                return config.Value;

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Event evt = Event.current;
            float newValue = config.Value;

            switch (evt.type)
            {
                case EventType.MouseDown:
                    HandleMouseDown(evt, sliderRect, controlId, trackRect, config, ref newValue);
                    break;
                case EventType.MouseDrag:
                    HandleMouseDrag(evt, controlId, trackRect, config, ref newValue);
                    break;
                case EventType.MouseUp:
                    HandleMouseUp(evt, controlId);
                    break;
            }

            return newValue;
        }

        private void HandleMouseDown(Event evt, Rect sliderRect, int controlId, Rect trackRect, SliderConfig config, ref float newValue)
        {
            if (sliderRect.Contains(evt.mousePosition) && evt.button == 0)
            {
                _activeControlId = controlId;
                _isDragging = true;
                GUIUtility.hotControl = controlId;
                newValue = CalculateValueFromMousePosition(evt.mousePosition.x, trackRect, config);
                evt.Use();
            }
        }

        private void HandleMouseDrag(Event evt, int controlId, Rect trackRect, SliderConfig config, ref float newValue)
        {
            if (_isDragging && _activeControlId == controlId && GUIUtility.hotControl == controlId)
            {
                newValue = CalculateValueFromMousePosition(evt.mousePosition.x, trackRect, config);
                evt.Use();
            }
        }

        private void HandleMouseUp(Event evt, int controlId)
        {
            if (_isDragging && _activeControlId == controlId)
            {
                _isDragging = false;
                _activeControlId = -1;
                GUIUtility.hotControl = 0;
                evt.Use();
            }
        }

        private float CalculateValueFromMousePosition(float mouseX, Rect trackRect, SliderConfig config)
        {
            float normalizedPos = Mathf.Clamp01((mouseX - trackRect.x) / trackRect.width);
            float value = CalculateValue(normalizedPos, config.MinValue, config.MaxValue);
            return SnapToStep(value, config.Step, config.MinValue, config.MaxValue);
        }

        private void DrawCachedRoundedRect(Rect rect, Color color, int radius)
        {
            int width = Mathf.Max(8, Mathf.RoundToInt(rect.width));
            int height = Mathf.Max(8, Mathf.RoundToInt(rect.height));

            var texture = styleManager.CreateTexture(width, height, radius, color);
            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);
        }

        private void DrawCachedThumb(Rect rect, Color color, bool disabled)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            int size = Mathf.Max(8, Mathf.RoundToInt(rect.width));
            int radius = size / 2;

            if (!disabled)
                DrawShadow(rect, size);

            DrawThumbTexture(rect, size, color, theme.Border);
        }

        private void DrawShadow(Rect rect, int size)
        {
            int radius = size / 2;
            var shadowColor = new Color(0, 0, 0, 0.2f);
            var shadowTex = styleManager.CreateTexture(size, size, radius, shadowColor);
            Rect shadowRect = new Rect(rect.x + 1, rect.y + 2, rect.width, rect.height);
            GUI.DrawTexture(shadowRect, shadowTex, ScaleMode.StretchToFill);
        }

        private void DrawThumbTexture(Rect rect, int size, Color color, Color borderColor)
        {
            int radius = size / 2;
            var thumbTex = styleManager.CreateBorderTexture(size, size, radius, color, borderColor, 1f);
            GUI.DrawTexture(rect, thumbTex, ScaleMode.StretchToFill);
        }
    }
}
