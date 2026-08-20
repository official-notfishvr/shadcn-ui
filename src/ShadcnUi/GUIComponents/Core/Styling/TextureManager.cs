using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public sealed class TextureManager
    {
        private readonly GUIHelper _helper;
        private readonly Dictionary<TextureKey, Texture2D> _cache = new();
        private readonly List<Texture2D> _ownedTextures = new();

        public Texture2D Gradient { get; private set; }
        public Texture2D Glow { get; private set; }
        public Texture2D Particle { get; private set; }
        public Texture2D CardBackground { get; private set; }
        public Texture2D Transparent { get; private set; }
        public Texture2D InputFocused { get; private set; }
        public Texture2D ProgressBarBackground { get; private set; }
        public Texture2D Separator { get; private set; }
        public Texture2D TabsActive { get; private set; }
        public Texture2D Badge { get; private set; }
        public Texture2D TableCell { get; private set; }
        public Texture2D TableRow { get; private set; }
        public Texture2D TableRowAlternate { get; private set; }
        public Texture2D DropdownMenuContent { get; private set; }
        public Texture2D ChartContainer { get; private set; }

        private readonly struct TextureKey : IEquatable<TextureKey>
        {
            public readonly int Width;
            public readonly int Height;
            public readonly int Radius;
            public readonly Color FillA;
            public readonly Color FillB;
            public readonly Color Border;
            public readonly float BorderThickness;
            public readonly float ShadowAlpha;
            public readonly int ShadowBlur;
            public readonly Color ShadowColor;

            public TextureKey(int width, int height, int radius, Color fillA, Color fillB, Color border, float borderThickness, float shadowAlpha, int shadowBlur, Color shadowColor)
            {
                Width = width;
                Height = height;
                Radius = radius;
                FillA = fillA;
                FillB = fillB;
                Border = border;
                BorderThickness = borderThickness;
                ShadowAlpha = shadowAlpha;
                ShadowBlur = shadowBlur;
                ShadowColor = shadowColor;
            }

            public bool Equals(TextureKey other)
            {
                return Width == other.Width
                    && Height == other.Height
                    && Radius == other.Radius
                    && FillA.Equals(other.FillA)
                    && FillB.Equals(other.FillB)
                    && Border.Equals(other.Border)
                    && BorderThickness.Equals(other.BorderThickness)
                    && ShadowAlpha.Equals(other.ShadowAlpha)
                    && ShadowBlur == other.ShadowBlur
                    && ShadowColor.Equals(other.ShadowColor);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = Width;
                    hash = (hash * 397) ^ Height;
                    hash = (hash * 397) ^ Radius;
                    hash = (hash * 397) ^ FillA.GetHashCode();
                    hash = (hash * 397) ^ FillB.GetHashCode();
                    hash = (hash * 397) ^ Border.GetHashCode();
                    hash = (hash * 397) ^ BorderThickness.GetHashCode();
                    hash = (hash * 397) ^ ShadowAlpha.GetHashCode();
                    hash = (hash * 397) ^ ShadowBlur;
                    hash = (hash * 397) ^ ShadowColor.GetHashCode();
                    return hash;
                }
            }
        }

        public TextureManager(GUIHelper helper)
        {
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public void CreateAllTextures()
        {
            DestroyAllTextures();

            var theme = ThemeManager.Instance.CurrentTheme;
            var mdRadius = Mathf.Max(0, Mathf.RoundToInt(DesignTokens.Radius.MD * _helper.uiScale));
            var lgRadius = Mathf.Max(0, Mathf.RoundToInt(DesignTokens.Radius.LG * _helper.uiScale));
            var xlRadius = Mathf.Max(0, Mathf.RoundToInt(DesignTokens.Radius.XL * _helper.uiScale));
            var focusHeight = Mathf.Max(1, Mathf.RoundToInt(DesignTokens.Height.Default * _helper.uiScale));
            var tabsHeight = Mathf.Max(1, Mathf.RoundToInt(DesignTokens.Height.Small * _helper.uiScale));
            var badgeHeight = Mathf.Max(1, Mathf.RoundToInt(DesignTokens.Badge.Height * _helper.uiScale));
            var shadowBlurMd = Mathf.Max(0, Mathf.RoundToInt(DesignTokens.Effects.ShadowBlurMD * _helper.uiScale));
            var shadowBlurLg = Mathf.Max(0, Mathf.RoundToInt(DesignTokens.Effects.ShadowBlurLG * _helper.uiScale));
            var focusBlur = Mathf.Max(0, Mathf.RoundToInt(DesignTokens.Effects.FocusRingBlur * _helper.uiScale));
            var focusShadow = new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.9f);

            Gradient = GenerateVerticalGradient(1, 32, theme.Base, theme.Secondary);
            Glow = GenerateGlow(64, new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.18f));
            Particle = GenerateSolid(theme.Accent);
            Transparent = GenerateSolid(Color.clear);
            Separator = GenerateSolid(theme.Border);
            InputFocused = GenerateShape(128, focusHeight, mdRadius, theme.Base, theme.Base, theme.Accent, DesignTokens.Effects.FocusRingThickness, DesignTokens.Effects.FocusRingAlpha, focusBlur, focusShadow);
            ProgressBarBackground = GenerateShape(128, DesignTokens.ProgressBar.TextureHeight, 999, theme.Secondary, theme.Secondary, Color.clear, 0f, 0f, 0);
            TabsActive = GenerateShape(128, tabsHeight, mdRadius, theme.Base, theme.Base, theme.Border, 1f, DesignTokens.Effects.ShadowLight, shadowBlurMd, theme.Shadow);
            Badge = GenerateShape(96, badgeHeight, mdRadius, theme.ButtonPrimaryBg, theme.ButtonPrimaryBg, Color.clear, 0f, 0f, 0);
            TableCell = GenerateSolid(theme.Base);
            TableRow = GenerateSolid(theme.Base);
            TableRowAlternate = GenerateSolid(Color.Lerp(theme.Base, theme.Secondary, 0.6f));
            DropdownMenuContent = GenerateShape(192, 192, lgRadius, theme.Elevated, theme.Elevated, theme.Border, 1f, DesignTokens.Effects.ShadowMedium, shadowBlurMd, theme.Shadow);
            ChartContainer = GenerateShape(256, 256, xlRadius, theme.Elevated, theme.Elevated, theme.Border, 1f, 0f, 0);
            CardBackground = GenerateShape(256, 256, xlRadius, theme.Elevated, theme.Elevated, theme.Border, 1f, DesignTokens.Effects.ShadowLight, shadowBlurLg, theme.Shadow);
        }

        public Texture2D GenerateShape(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderPx, float shadowAlpha = 0f, int shadowBlur = 0, Color shadowColor = default)
        {
            width = Mathf.Max(1, width);
            height = Mathf.Max(1, height);
            radius = Mathf.Clamp(radius, 0, Mathf.Min(width, height) / 2);
            shadowBlur = Mathf.Max(0, shadowBlur);

            var effectiveShadowColor = shadowColor.a > 0f ? shadowColor : ThemeManager.Instance.CurrentTheme?.Shadow ?? new Color(0f, 0f, 0f, 0.18f);

            var key = new TextureKey(width, height, radius, topColor, bottomColor, borderColor, borderPx, shadowAlpha, shadowBlur, effectiveShadowColor);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var tex = CreateTexture2D(width, height);
            var pixels = new Color[width * height];
            var borderThickness = Mathf.Max(0f, borderPx);

            for (int y = 0; y < height; y++)
            {
                var t = height <= 1 ? 0f : y / (float)(height - 1);
                var fillColor = Color.Lerp(bottomColor, topColor, t);

                for (int x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    var distance = DistanceToRoundedRectEdge(x + 0.5f, y + 0.5f, width, height, radius);
                    var coverage = Coverage(distance);

                    if (coverage <= 0f)
                    {
                        if (shadowBlur > 0 && shadowAlpha > 0f && distance <= shadowBlur)
                        {
                            var shadowT = 1f - distance / shadowBlur;
                            var alpha = effectiveShadowColor.a * shadowAlpha * shadowT * shadowT;
                            pixels[index] = new Color(effectiveShadowColor.r, effectiveShadowColor.g, effectiveShadowColor.b, alpha);
                        }
                        else
                        {
                            pixels[index] = Color.clear;
                        }

                        continue;
                    }

                    var color = fillColor;
                    if (borderThickness > 0f && borderColor.a > 0f)
                    {
                        var innerCoverage = Coverage(distance + borderThickness);
                        color = Color.Lerp(borderColor, fillColor, innerCoverage);
                    }

                    color.a *= coverage;
                    pixels[index] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            Track(key, tex);
            return tex;
        }

        public Texture2D GenerateSolid(Color color)
        {
            var key = new TextureKey(1, 1, 0, color, color, Color.clear, 0f, 0f, 0, Color.clear);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var texture = CreateTexture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            Track(key, texture);
            return texture;
        }

        public Texture2D GenerateVerticalGradient(int width, int height, Color top, Color bottom)
        {
            width = Mathf.Max(1, width);
            height = Mathf.Max(1, height);

            var key = new TextureKey(width, height, 0, top, bottom, Color.clear, 0f, 0f, 0, Color.clear);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var texture = CreateTexture2D(width, height);

            for (int y = 0; y < height; y++)
            {
                var t = height <= 1 ? 0f : y / (float)(height - 1);
                var color = Color.Lerp(bottom, top, t);
                for (int x = 0; x < width; x++)
                    texture.SetPixel(x, y, color);
            }

            texture.Apply();
            Track(key, texture);
            return texture;
        }

        public Texture2D GenerateGlow(int size, Color color)
        {
            size = Mathf.Max(1, size);

            var key = new TextureKey(size, size, 0, color, color, Color.clear, -1f, 0f, 0, Color.clear);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var texture = CreateTexture2D(size, size);
            var center = size * 0.5f;
            var maxDistance = center;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    var alpha = Mathf.Clamp01(1f - distance / maxDistance);
                    texture.SetPixel(x, y, new Color(color.r, color.g, color.b, alpha * alpha * color.a));
                }
            }

            texture.Apply();
            Track(key, texture);
            return texture;
        }

        public Texture2D GenerateAvatarTexture(int size, int radius, Color backgroundColor, Color borderColor, float borderThickness, bool withShadow = true)
        {
            return GenerateShape(size, size, radius, backgroundColor, backgroundColor, borderColor, borderThickness, withShadow ? DesignTokens.Effects.ShadowLight : 0f, withShadow ? Mathf.RoundToInt(DesignTokens.Effects.ShadowBlurSM * _helper.uiScale) : 0);
        }

        public void DestroyAllTextures()
        {
            foreach (var texture in _ownedTextures)
            {
                if (texture != null)
                    UnityEngine.Object.DestroyImmediate(texture);
            }

            _ownedTextures.Clear();
            _cache.Clear();
            Gradient = null;
            Glow = null;
            Particle = null;
            CardBackground = null;
            Transparent = null;
            InputFocused = null;
            ProgressBarBackground = null;
            Separator = null;
            TabsActive = null;
            Badge = null;
            TableCell = null;
            TableRow = null;
            TableRowAlternate = null;
            DropdownMenuContent = null;
            ChartContainer = null;
        }

        public void Cleanup() => DestroyAllTextures();

        public void DrawTabUnderlineIndicator(Rect tabRect, Color color, bool isVertical, bool isLeft, float indicatorHeight, float uiScale)
        {
            var thickness = Mathf.Max(1f, indicatorHeight * uiScale);
            Rect rect;

            if (isVertical)
            {
                rect = isLeft ? new Rect(tabRect.xMax - thickness, tabRect.y, thickness, tabRect.height) : new Rect(tabRect.x, tabRect.y, thickness, tabRect.height);
            }
            else
            {
                rect = new Rect(tabRect.x, tabRect.yMax - thickness, tabRect.width, thickness);
            }

            var previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = previous;
        }

        public void DrawTabBackgroundIndicator(Rect tabRect, Color color)
        {
            var previous = GUI.color;
            GUI.color = new Color(color.r, color.g, color.b, 0.12f);
            GUI.DrawTexture(tabRect, Texture2D.whiteTexture);
            GUI.color = previous;
        }

        public void DrawTabBorderIndicator(Rect tabRect, Color color, bool isVertical, bool isLeft, float borderWidth, float uiScale)
        {
            var thickness = Mathf.Max(1f, borderWidth * uiScale);
            var previous = GUI.color;
            GUI.color = color;

            if (isVertical)
            {
                if (isLeft)
                    GUI.DrawTexture(new Rect(tabRect.xMax - thickness, tabRect.y, thickness, tabRect.height), Texture2D.whiteTexture);
                else
                    GUI.DrawTexture(new Rect(tabRect.x, tabRect.y, thickness, tabRect.height), Texture2D.whiteTexture);
            }
            else
            {
                GUI.DrawTexture(new Rect(tabRect.x, tabRect.yMax - thickness, tabRect.width, thickness), Texture2D.whiteTexture);
            }

            GUI.color = previous;
        }

        private static Texture2D CreateTexture2D(int width, int height)
        {
            return new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                anisoLevel = 1,
            };
        }

        private void Track(TextureKey key, Texture2D texture)
        {
            _cache[key] = texture;
            _ownedTextures.Add(texture);
        }

        private static float Coverage(float signedDistance) => Mathf.Clamp01(0.5f - signedDistance);

        private static float DistanceToRoundedRectEdge(float x, float y, float width, float height, float radius)
        {
            if (radius <= 0f)
            {
                if (x >= 0f && x <= width && y >= 0f && y <= height)
                    return -Mathf.Min(x, width - x, y, height - y);

                var dx = x < 0f ? -x : (x > width ? x - width : 0f);
                var dy = y < 0f ? -y : (y > height ? y - height : 0f);
                return Mathf.Sqrt(dx * dx + dy * dy);
            }

            var qx = Mathf.Abs(x - width * 0.5f) - (width * 0.5f - radius);
            var qy = Mathf.Abs(y - height * 0.5f) - (height * 0.5f - radius);
            var outside = new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f));
            var inside = Mathf.Min(Mathf.Max(qx, qy), 0f);
            return outside.magnitude + inside - radius;
        }
    }
}
