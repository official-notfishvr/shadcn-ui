using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class RangeSlider : BaseComponent
    {
        private enum ActiveThumb
        {
            None,
            Lower,
            Upper,
        }

        private int _activeControlId = -1;
        private ActiveThumb _activeThumb;

        public RangeSlider(GUIHelper helper)
            : base(helper) { }

        public Vector2 Render(RangeSliderConfig config)
        {
            if (config == null)
                return Vector2.zero;

            try
            {
                bool prevEnabled = GUI.enabled;
                if (config.IsDisabled)
                    GUI.enabled = false;

                DrawLabel(config);
                Vector2 result = DrawTrack(config);

                GUI.enabled = prevEnabled;

                if (!config.IsDisabled && (!Mathf.Approximately(result.x, config.LowerValue) || !Mathf.Approximately(result.y, config.UpperValue)))
                    config.OnValueChanged?.Invoke(result.x, result.y);

                return config.IsDisabled ? new Vector2(config.LowerValue, config.UpperValue) : result;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(Render), nameof(RangeSlider));
                return new Vector2(config.LowerValue, config.UpperValue);
            }
        }

        private void DrawLabel(RangeSliderConfig config)
        {
            if (string.IsNullOrEmpty(config.Label))
                return;

            GUILayout.BeginHorizontal();
            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.label;
            UnityHelpers.Label(config.Label, labelStyle);

            if (config.ShowValue)
            {
                GUILayout.FlexibleSpace();
                string valueText = $"{config.LowerValue.ToString(config.ValueFormat)} - {config.UpperValue.ToString(config.ValueFormat)}";
                var mutedStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size, config.Appearance) ?? GUI.skin.label;
                UnityHelpers.Label(valueText, mutedStyle);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(DesignTokens.Spacing.XS * guiHelper.uiScale);
        }

        private Vector2 DrawTrack(RangeSliderConfig config)
        {
            float trackHeight = styleManager.GetSliderTrackHeight(config.Size);
            float thumbSize = styleManager.GetSliderThumbSize(config.Size);
            float totalHeight = Mathf.Max(trackHeight, thumbSize);
            Rect sliderRect = GetSliderRect(config, totalHeight);

            Rect trackRect = new Rect(sliderRect.x + thumbSize / 2f, sliderRect.y + (totalHeight - trackHeight) / 2f, sliderRect.width - thumbSize, trackHeight);

            if (Event.current.type != EventType.Repaint)
                return HandleInput(sliderRect, trackRect, config, thumbSize, totalHeight);

            DrawRepaint(trackRect, config, thumbSize, totalHeight, sliderRect.y);
            return new Vector2(config.LowerValue, config.UpperValue);
        }

        private Rect GetSliderRect(RangeSliderConfig config, float totalHeight)
        {
            if (config.LayoutOptions != null && config.LayoutOptions.Length > 0)
                return GUILayoutUtility.GetRect(100f * guiHelper.uiScale, totalHeight, config.LayoutOptions);

            return ControlLayoutUtility.ReserveRect(UnityHelpers.GUIContent.none, GUI.skin.horizontalSlider, ControlLayoutUtility.BuildLayoutOptions(null, fixedHeight: totalHeight, expandWidth: true), totalHeight);
        }

        private void DrawRepaint(Rect trackRect, RangeSliderConfig config, float thumbSize, float totalHeight, float sliderY)
        {
            Color trackColor = styleManager.GetSliderTrackColor(config.Variant, config.IsDisabled, config.Appearance);
            Color fillColor = styleManager.GetSliderFillColor(config.Variant, config.IsDisabled, config.Appearance);
            Color thumbColor = styleManager.GetSliderThumbColor(config.Variant, config.IsDisabled, config.Appearance);

            float lowerNormalized = Slider.ValueToNormalized(config.LowerValue, config.MinValue, config.MaxValue);
            float upperNormalized = Slider.ValueToNormalized(config.UpperValue, config.MinValue, config.MaxValue);
            float lowerX = trackRect.x + trackRect.width * lowerNormalized;
            float upperX = trackRect.x + trackRect.width * upperNormalized;
            int trackRadius = Mathf.RoundToInt(trackRect.height / 2f);

            SurfaceDrawUtility.DrawRoundedFill(styleManager, trackRect, trackColor, trackRadius);

            Rect fillRect = new Rect(lowerX, trackRect.y, Mathf.Max(1f, upperX - lowerX), trackRect.height);
            SurfaceDrawUtility.DrawRoundedFill(styleManager, fillRect, fillColor, trackRadius);

            Rect lowerThumbRect = new Rect(lowerX - thumbSize / 2f, sliderY + (totalHeight - thumbSize) / 2f, thumbSize, thumbSize);
            Rect upperThumbRect = new Rect(upperX - thumbSize / 2f, sliderY + (totalHeight - thumbSize) / 2f, thumbSize, thumbSize);

            DrawThumb(lowerThumbRect, thumbColor, config.IsDisabled);
            DrawThumb(upperThumbRect, thumbColor, config.IsDisabled);
        }

        private Vector2 HandleInput(Rect sliderRect, Rect trackRect, RangeSliderConfig config, float thumbSize, float totalHeight)
        {
            if (config.IsDisabled)
                return new Vector2(config.LowerValue, config.UpperValue);

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Event evt = Event.current;

            float lowerNormalized = Slider.ValueToNormalized(config.LowerValue, config.MinValue, config.MaxValue);
            float upperNormalized = Slider.ValueToNormalized(config.UpperValue, config.MinValue, config.MaxValue);
            float lowerX = trackRect.x + trackRect.width * lowerNormalized;
            float upperX = trackRect.x + trackRect.width * upperNormalized;

            Rect lowerThumbRect = new Rect(lowerX - thumbSize / 2f, sliderRect.y + (totalHeight - thumbSize) / 2f, thumbSize, thumbSize);
            Rect upperThumbRect = new Rect(upperX - thumbSize / 2f, sliderRect.y + (totalHeight - thumbSize) / 2f, thumbSize, thumbSize);

            float lowerValue = config.LowerValue;
            float upperValue = config.UpperValue;

            switch (evt.type)
            {
                case EventType.MouseDown:
                    if (evt.button == 0 && sliderRect.Contains(evt.mousePosition))
                    {
                        _activeControlId = controlId;
                        GUIUtility.hotControl = controlId;
                        _activeThumb = ResolveActiveThumb(evt.mousePosition.x, lowerThumbRect, upperThumbRect);
                        UpdateValuesFromMouse(evt.mousePosition.x, trackRect, config, ref lowerValue, ref upperValue);
                        evt.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlId && _activeControlId == controlId)
                    {
                        UpdateValuesFromMouse(evt.mousePosition.x, trackRect, config, ref lowerValue, ref upperValue);
                        evt.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId && _activeControlId == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        _activeControlId = -1;
                        _activeThumb = ActiveThumb.None;
                        evt.Use();
                    }
                    break;
            }

            return new Vector2(lowerValue, upperValue);
        }

        private ActiveThumb ResolveActiveThumb(float mouseX, Rect lowerThumbRect, Rect upperThumbRect)
        {
            bool inLower = lowerThumbRect.Contains(new Vector2(mouseX, lowerThumbRect.center.y));
            bool inUpper = upperThumbRect.Contains(new Vector2(mouseX, upperThumbRect.center.y));

            if (inLower && !inUpper)
                return ActiveThumb.Lower;
            if (inUpper && !inLower)
                return ActiveThumb.Upper;

            float lowerDistance = Mathf.Abs(mouseX - lowerThumbRect.center.x);
            float upperDistance = Mathf.Abs(mouseX - upperThumbRect.center.x);
            return lowerDistance <= upperDistance ? ActiveThumb.Lower : ActiveThumb.Upper;
        }

        private void UpdateValuesFromMouse(float mouseX, Rect trackRect, RangeSliderConfig config, ref float lowerValue, ref float upperValue)
        {
            float normalized = Mathf.Clamp01((mouseX - trackRect.x) / trackRect.width);
            float raw = Slider.CalculateValue(normalized, config.MinValue, config.MaxValue);
            float snapped = Slider.SnapToStep(raw, config.Step, config.MinValue, config.MaxValue);

            if (_activeThumb == ActiveThumb.Upper)
            {
                upperValue = Mathf.Max(snapped, lowerValue);
                upperValue = Mathf.Clamp(upperValue, config.MinValue, config.MaxValue);
            }
            else
            {
                lowerValue = Mathf.Min(snapped, upperValue);
                lowerValue = Mathf.Clamp(lowerValue, config.MinValue, config.MaxValue);
            }
        }

        private void DrawThumb(Rect rect, Color color, bool disabled)
        {
            var theme = styleManager.GetTheme();
            int radius = Mathf.RoundToInt(rect.width / 2f);

            if (!disabled)
            {
                Rect shadowRect = new Rect(rect.x + 1f, rect.y + 2f, rect.width, rect.height);
                SurfaceDrawUtility.DrawRoundedFill(styleManager, shadowRect, new Color(0f, 0f, 0f, 0.2f), radius);
            }

            SurfaceDrawUtility.DrawRoundedBorder(styleManager, rect, radius, color, theme.Border, 1f);
        }
    }
}
