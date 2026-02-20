using System;
using System.Collections.Generic;
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
        private bool _isDragging = false;

        private struct TextureCacheKey : IEquatable<TextureCacheKey>
        {
            public int Width;
            public int Height;
            public int Radius;
            public Color Color;
            public Color BorderColor;
            public bool HasBorder;

            public bool Equals(TextureCacheKey other) => Width == other.Width && Height == other.Height && Radius == other.Radius && Color.Equals(other.Color) && BorderColor.Equals(other.BorderColor) && HasBorder == other.HasBorder;

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = Width * 397 ^ Height;
                    hash = hash * 397 ^ Radius;
                    hash = hash * 397 ^ Color.GetHashCode();
                    hash = hash * 397 ^ BorderColor.GetHashCode();
                    hash = hash * 397 ^ (HasBorder ? 1 : 0);
                    return hash;
                }
            }
        }

        private static Dictionary<TextureCacheKey, Texture2D> _textureCache = new Dictionary<TextureCacheKey, Texture2D>();

        public Slider(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public float Draw(SliderConfig config)
        {
            if (config == null)
                return 0f;

            try
            {
                float result = config.Value;
                bool wasEnabled = GUI.enabled;

                if (config.Disabled)
                    GUI.enabled = false;

                DrawSliderLabel(config);
                result = DrawSliderTrack(config);

                GUI.enabled = wasEnabled;

                if (!Mathf.Approximately(result, config.Value) && !config.Disabled)
                    config.OnChange?.Invoke(result);

                return config.Disabled ? config.Value : result;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Draw", "Slider");
                return config.Value;
            }
        }
        #endregion

        #region API
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

        public float Draw(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options)
        {
            return Draw(
                new SliderConfig
                {
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Options = options,
                }
            );
        }

        public float Draw(float value, float min, float max, float step, params GUILayoutOption[] options)
        {
            return Draw(
                new SliderConfig
                {
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Step = step,
                    Options = options,
                }
            );
        }

        public float LabeledSlider(string label, float value, float min, float max, bool showValue = true, params GUILayoutOption[] options)
        {
            return Draw(
                new SliderConfig
                {
                    Label = label,
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    ShowValue = showValue,
                    Options = options,
                }
            );
        }

        public float LabeledSlider(string label, float value, float min, float max, float step, bool showValue = true, params GUILayoutOption[] options)
        {
            return Draw(
                new SliderConfig
                {
                    Label = label,
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Step = step,
                    ShowValue = showValue,
                    Options = options,
                }
            );
        }

        public float DisabledSlider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options)
        {
            return Draw(
                new SliderConfig
                {
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Disabled = true,
                    Options = options,
                }
            );
        }
        #endregion

        #region Private Methods
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
                return HandleSliderInput(sliderRect, trackRect, config, Rect.zero);
            }

            return DrawSliderRepaint(sliderRect, config, trackHeight, thumbSize, totalHeight);
        }

        private Rect GetSliderRect(SliderConfig config, float totalHeight)
        {
            if (config.Options != null && config.Options.Length > 0)
                return GUILayoutUtility.GetRect(100f * guiHelper.uiScale, totalHeight, config.Options);
            return GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.horizontalSlider, GUILayout.Height(totalHeight), GUILayout.ExpandWidth(true));
        }

        private float DrawSliderRepaint(Rect sliderRect, SliderConfig config, float trackHeight, float thumbSize, float totalHeight)
        {
            float trackY = sliderRect.y + (totalHeight - trackHeight) / 2f;
            Rect trackRect = new Rect(sliderRect.x + thumbSize / 2f, trackY, sliderRect.width - thumbSize, trackHeight);

            Color trackColor = styleManager.GetSliderTrackColor(config.Variant, config.Disabled);
            Color fillColor = styleManager.GetSliderFillColor(config.Variant, config.Disabled);
            Color thumbColor = styleManager.GetSliderThumbColor(config.Variant, config.Disabled);

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

            DrawCachedThumb(thumbRect, thumbColor, config.Disabled);

            return config.Value;
        }

        private float HandleSliderInput(Rect sliderRect, Rect trackRect, SliderConfig config, Rect thumbRect)
        {
            if (config.Disabled)
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

            var key = new TextureCacheKey
            {
                Width = width,
                Height = height,
                Radius = radius,
                Color = color,
                HasBorder = false,
            };

            if (!_textureCache.TryGetValue(key, out var texture) || texture == null)
            {
                texture = CreateSimpleRoundedTexture(width, height, radius, color);
                _textureCache[key] = texture;
            }

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
            var shadowKey = new TextureCacheKey
            {
                Width = size,
                Height = size,
                Radius = size / 2,
                Color = new Color(0, 0, 0, 0.2f),
                HasBorder = false,
            };
            if (!_textureCache.TryGetValue(shadowKey, out var shadowTex) || shadowTex == null)
            {
                shadowTex = CreateSimpleRoundedTexture(size, size, size / 2, new Color(0, 0, 0, 0.2f));
                _textureCache[shadowKey] = shadowTex;
            }
            Rect shadowRect = new Rect(rect.x + 1, rect.y + 2, rect.width, rect.height);
            GUI.DrawTexture(shadowRect, shadowTex, ScaleMode.StretchToFill);
        }

        private void DrawThumbTexture(Rect rect, int size, Color color, Color borderColor)
        {
            int radius = size / 2;
            var thumbKey = new TextureCacheKey
            {
                Width = size,
                Height = size,
                Radius = radius,
                Color = color,
                BorderColor = borderColor,
                HasBorder = true,
            };
            if (!_textureCache.TryGetValue(thumbKey, out var thumbTex) || thumbTex == null)
            {
                thumbTex = CreateBorderedCircleTexture(size, color, borderColor);
                _textureCache[thumbKey] = thumbTex;
            }

            GUI.DrawTexture(rect, thumbTex, ScaleMode.StretchToFill);
        }

        private static Texture2D CreateCircleTexture(int size, Color color)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            var pixels = new Color[size * size];

            float center = (size - 1) / 2f;
            float radius = size / 2f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    pixels[y * size + x] = dist <= radius ? color : Color.clear;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateSimpleRoundedTexture(int width, int height, int radius, Color color)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            var pixels = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool inside = IsInsideRoundedRect(x, y, width, height, radius);
                    pixels[y * width + x] = inside ? color : Color.clear;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private static bool IsInsideRoundedRect(int x, int y, int width, int height, int radius)
        {
            if (x < radius && y < radius)
                return (x - radius) * (x - radius) + (y - radius) * (y - radius) <= radius * radius;
            else if (x >= width - radius && y < radius)
                return (x - (width - radius - 1)) * (x - (width - radius - 1)) + (y - radius) * (y - radius) <= radius * radius;
            else if (x < radius && y >= height - radius)
                return (x - radius) * (x - radius) + (y - (height - radius - 1)) * (y - (height - radius - 1)) <= radius * radius;
            else if (x >= width - radius && y >= height - radius)
                return (x - (width - radius - 1)) * (x - (width - radius - 1)) + (y - (height - radius - 1)) * (y - (height - radius - 1)) <= radius * radius;
            return true;
        }

        private static Texture2D CreateBorderedCircleTexture(int size, Color fillColor, Color borderColor)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            var pixels = new Color[size * size];

            float center = (size - 1) / 2f;
            float outerRadius = size / 2f;
            float innerRadius = outerRadius - 1f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist > outerRadius)
                        pixels[y * size + x] = Color.clear;
                    else if (dist > innerRadius)
                        pixels[y * size + x] = borderColor;
                    else
                        pixels[y * size + x] = fillColor;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        #endregion
    }
}
