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
        public Texture2D TableHeader { get; private set; }
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

            public TextureKey(int width, int height, int radius, Color fillA, Color fillB, Color border, float borderThickness, float shadowAlpha, int shadowBlur)
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
                    && ShadowBlur == other.ShadowBlur;
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
            Gradient = GenerateVerticalGradient(1, 32, theme.Base, theme.Secondary);
            Glow = GenerateGlow(32, theme.Accent);
            Particle = GenerateSolid(theme.Accent);
            Transparent = GenerateSolid(Color.clear);
            Separator = GenerateSolid(theme.Border);
            InputFocused = GenerateShape(128, Mathf.RoundToInt(DesignTokens.Height.Default), 2, Color.clear, Color.clear, theme.Accent, 1f, 0f, 0);
            ProgressBarBackground = GenerateShape(128, DesignTokens.ProgressBar.TextureHeight, 999, theme.Secondary, theme.Secondary, Color.clear, 0f, 0f, 0);
            TabsActive = GenerateShape(128, 36, 2, theme.TabsTriggerActiveBg, theme.TabsTriggerActiveBg, theme.Border, 1f, 0f, 0);
            Badge = GenerateShape(96, 24, 999, theme.ButtonPrimaryBg, theme.ButtonPrimaryBg, Color.clear, 0f, 0f, 0);
            TableCell = GenerateSolid(theme.Base);
            TableHeader = GenerateShape(128, 36, 0, theme.Secondary, theme.Secondary, theme.Border, 1f, 0f, 0);
            TableRow = GenerateShape(128, 36, 0, theme.Base, theme.Base, theme.Border, 1f, 0f, 0);
            TableRowAlternate = GenerateShape(128, 36, 0, theme.Elevated, theme.Elevated, theme.Border, 1f, 0f, 0);
            DropdownMenuContent = GenerateShape(192, 192, 2, theme.Elevated, theme.Elevated, theme.Border, 1f, 0.08f, 4);
            ChartContainer = GenerateShape(256, 256, 2, theme.Elevated, theme.Elevated, theme.Border, 1f, 0f, 0);
            CardBackground = GenerateShape(256, 256, 2, theme.Elevated, theme.Elevated, theme.Border, 1f, 0.04f, 4);
        }

        public Texture2D GenerateShape(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderPx, float shadowAlpha = 0f, int shadowBlur = 0)
        {
            width = Mathf.Max(1, width);
            height = Mathf.Max(1, height);
            radius = Mathf.Max(0, radius);
            shadowBlur = Mathf.Max(0, shadowBlur);

            var key = new TextureKey(width, height, radius, topColor, bottomColor, borderColor, borderPx, shadowAlpha, shadowBlur);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };

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

                    if (distance > 0f)
                    {
                        if (shadowBlur > 0 && distance <= shadowBlur)
                        {
                            var shadowT = 1f - distance / shadowBlur;
                            pixels[index] = new Color(0f, 0f, 0f, shadowAlpha * shadowT * shadowT);
                        }
                        else
                        {
                            pixels[index] = Color.clear;
                        }

                        continue;
                    }

                    var color = fillColor;
                    if (borderThickness > 0f && distance >= -borderThickness)
                        color = borderColor;

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
            var key = new TextureKey(1, 1, 0, color, color, Color.clear, 0f, 0f, 0);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };
            texture.SetPixel(0, 0, color);
            texture.Apply();
            Track(key, texture);
            return texture;
        }

        public Texture2D GenerateVerticalGradient(int width, int height, Color top, Color bottom)
        {
            var key = new TextureKey(width, height, 0, top, bottom, Color.clear, 0f, 0f, 0);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };

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
            var key = new TextureKey(size, size, 0, color, color, Color.clear, -1f, 0f, 0);
            if (_cache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };

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
            return GenerateShape(size, size, radius, backgroundColor, backgroundColor, borderColor, borderThickness, withShadow ? 0.08f : 0f, withShadow ? 4 : 0);
        }

        public Texture2D GenerateStatusIndicator(int size, bool isOnline)
        {
            var fill = isOnline ? new Color(0.22f, 0.78f, 0.40f, 1f) : new Color(0.53f, 0.57f, 0.63f, 1f);
            return GenerateShape(size, size, size / 2, fill, fill, Color.white, 1f, 0f, 0);
        }

        public Texture2D GenerateTableHeaderTexture(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderThickness = 1f)
        {
            return GenerateShape(width, height, radius, topColor, bottomColor, borderColor, borderThickness, 0f, 0);
        }

        public Texture2D GenerateTableRowTexture(int width, int height, Color topColor, Color bottomColor, Color borderColor, float borderThickness = 1f)
        {
            return GenerateShape(width, height, 0, topColor, bottomColor, borderColor, borderThickness, 0f, 0);
        }

        public Texture2D GenerateTableCellTexture(Color cellColor)
        {
            return GenerateSolid(cellColor);
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
            TableHeader = null;
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

        private void Track(TextureKey key, Texture2D texture)
        {
            _cache[key] = texture;
            _ownedTextures.Add(texture);
        }

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
