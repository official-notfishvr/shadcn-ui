using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace shadcnui.GUIComponents.Core.Styling
{
    public class TextureManager
    {
        private readonly GUIHelper _guiHelper;
        private readonly Dictionary<int, Texture2D> _textureCache = new();
        private readonly List<Texture2D> _activeTextures = new();
        private Theme _cachedTheme;
        private float _cachedScale;

        public Texture2D Gradient { get; private set; }
        public Texture2D Glow { get; private set; }
        public Texture2D Particle { get; private set; }
        public Texture2D CardBackground { get; private set; }
        public Texture2D Transparent { get; private set; }
        public Texture2D InputFocused { get; private set; }
        public Texture2D ProgressBarBackground { get; private set; }
        public Texture2D Separator { get; private set; }
        public Texture2D TabsBackground { get; private set; }
        public Texture2D TabsActive { get; private set; }
        public Texture2D Badge { get; private set; }
        public Texture2D TableCell { get; private set; }
        public Texture2D CalendarBackground { get; private set; }
        public Texture2D CalendarDay { get; private set; }
        public Texture2D CalendarDaySelected { get; private set; }
        public Texture2D DropdownMenuContent { get; private set; }
        public Texture2D ChartContainer { get; private set; }

        private struct TextureKey : IEquatable<TextureKey>
        {
            public readonly int Width;
            public readonly int Height;
            public readonly int Radius;
            public readonly Color PrimaryColor;
            public readonly Color SecondaryColor;
            public readonly Color BorderColor;
            public readonly float BorderThickness;
            public readonly float ShadowStrength;
            public readonly int ShadowBlur;

            public TextureKey(int w, int h, int r, Color c1, Color c2, Color bc, float bt, float ss = 0, int sb = 0)
            {
                Width = w;
                Height = h;
                Radius = r;
                PrimaryColor = c1;
                SecondaryColor = c2;
                BorderColor = bc;
                BorderThickness = bt;
                ShadowStrength = ss;
                ShadowBlur = sb;
            }

            public bool Equals(TextureKey other)
            {
                return Width == other.Width
                    && Height == other.Height
                    && Radius == other.Radius
                    && PrimaryColor.Equals(other.PrimaryColor)
                    && SecondaryColor.Equals(other.SecondaryColor)
                    && BorderColor.Equals(other.BorderColor)
                    && BorderThickness.Equals(other.BorderThickness)
                    && ShadowStrength.Equals(other.ShadowStrength)
                    && ShadowBlur == other.ShadowBlur;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + Width;
                    hash = hash * 31 + Height;
                    hash = hash * 31 + Radius;
                    hash = hash * 31 + PrimaryColor.GetHashCode();
                    hash = hash * 31 + SecondaryColor.GetHashCode();
                    hash = hash * 31 + BorderColor.GetHashCode();
                    hash = hash * 31 + BorderThickness.GetHashCode();
                    hash = hash * 31 + ShadowStrength.GetHashCode();
                    hash = hash * 31 + ShadowBlur;
                    return hash;
                }
            }
        }

        public TextureManager(GUIHelper helper)
        {
            _guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
            _cachedScale = helper.uiScale;
        }

        public void CreateAllTextures()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            _cachedTheme = theme;
            _cachedScale = _guiHelper.uiScale;

            int sizeDef = DesignTokens.TextureSize.Default;
            int sizeSm = DesignTokens.TextureSize.Small;
            int sizeLg = DesignTokens.TextureSize.Large;
            int sizeMd = DesignTokens.TextureSize.Medium;

            int rMD = Mathf.RoundToInt(DesignTokens.Radius.MD * _cachedScale);
            int rSM = Mathf.RoundToInt(DesignTokens.Radius.SM * _cachedScale);
            int rLG = Mathf.RoundToInt(DesignTokens.Radius.LG * _cachedScale);
            int rXL = Mathf.RoundToInt(DesignTokens.Radius.XL * _cachedScale);

            Gradient = GenerateVerticalGradient(1, 64, theme.Base, theme.Secondary);

            CardBackground = GenerateShape(sizeDef, sizeDef, rLG, theme.Elevated, theme.Elevated, Color.clear, 0, 0.15f, 8);

            InputFocused = GenerateShape(sizeDef, (int)DesignTokens.Height.Default, rMD, theme.Base, theme.Base, theme.Accent, 2f);

            Transparent = GenerateSolid(Color.clear);

            Glow = GenerateGlow(sizeSm, theme.Accent);

            Particle = GenerateSolid(Color.Lerp(theme.Accent, theme.Text, 0.3f));

            ProgressBarBackground = GenerateShape(sizeDef, DesignTokens.ProgressBar.TextureHeight, rSM, theme.Secondary, theme.Secondary, Color.clear, 0);

            Separator = GenerateSolid(theme.Border);

            TabsBackground = GenerateShape(sizeDef, sizeDef, rMD, theme.TabsBg, theme.TabsBg, Color.clear, 0);

            TabsActive = GenerateShape(sizeDef, sizeDef, rSM, theme.TabsTriggerActiveBg, theme.TabsTriggerActiveBg, Color.clear, 0);

            Badge = GenerateShape(sizeMd, (int)DesignTokens.Badge.Height, rXL, theme.Accent, theme.Accent, Color.clear, 0);

            TableCell = GenerateSolid(theme.Base);

            CalendarBackground = GenerateShape(sizeLg, sizeLg, rLG, theme.Elevated, theme.Elevated, theme.Border, 1f);

            CalendarDay = GenerateSolid(theme.Elevated);

            CalendarDaySelected = GenerateShape(sizeSm, sizeSm, rSM, theme.Accent, theme.Accent, Color.clear, 0);

            DropdownMenuContent = GenerateShape(sizeDef, sizeDef, rMD, theme.Elevated, theme.Elevated, Color.clear, 0, 0.12f, 6);

            ChartContainer = GenerateShape(sizeLg, sizeLg, rLG, theme.Elevated, theme.Elevated, theme.Border, 1f);
        }

        public Texture2D GenerateShape(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderPx, float shadowAlpha = 0, int shadowBlur = 0)
        {
            var key = new TextureKey(width, height, radius, topColor, bottomColor, borderColor, borderPx, shadowAlpha, shadowBlur);
            int hash = key.GetHashCode();

            if (_textureCache.TryGetValue(hash, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color32[width * height];
            int contentW = width - shadowBlur;
            int contentH = height - shadowBlur;
            int offset = shadowBlur / 2;

            bool hasBorder = borderPx > 0;
            bool hasShadow = shadowAlpha > 0 && shadowBlur > 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    int cx = x - offset;
                    int cy = y - offset;

                    float dist = GetSDF(cx, cy, contentW, contentH, radius);

                    if (dist <= 0)
                    {
                        float t = (float)cy / (contentH - 1);
                        Color fill = Color.Lerp(bottomColor, topColor, t);

                        if (hasBorder && dist > -borderPx)
                        {
                            float borderT = Mathf.Clamp01(1f - (Mathf.Abs(dist) / borderPx));
                            pixels[idx] = Color32.Lerp(fill, borderColor, borderT);
                        }
                        else
                        {
                            pixels[idx] = fill;
                        }

                        if (dist > -1.0f)
                        {
                            float aa = Mathf.Clamp01(-dist);
                            var col = (Color)pixels[idx];
                            col.a *= aa;
                            pixels[idx] = col;
                        }
                    }
                    else
                    {
                        if (hasShadow && dist < shadowBlur)
                        {
                            float shadowT = 1f - (dist / shadowBlur);
                            shadowT = shadowT * shadowT;
                            pixels[idx] = new Color(0, 0, 0, shadowT * shadowAlpha);
                        }
                        else
                        {
                            pixels[idx] = new Color32(0, 0, 0, 0);
                        }
                    }
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();
            _textureCache[hash] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateStyledShape(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderPx, float shadowAlpha = 0, int shadowBlur = 0, float highlightIntensity = 0.1f)
        {
            var key = new TextureKey(width, height, radius, topColor, bottomColor, borderColor, borderPx, shadowAlpha + 1.234f, shadowBlur + 567);
            int hash = key.GetHashCode();

            if (_textureCache.TryGetValue(hash, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color32[width * height];
            int contentW = width - shadowBlur;
            int contentH = height - shadowBlur;
            int offset = shadowBlur / 2;

            bool hasBorder = borderPx > 0;
            bool hasShadow = shadowAlpha > 0 && shadowBlur > 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    int cx = x - offset;
                    int cy = y - offset;

                    float dist = GetSDF(cx, cy, contentW, contentH, radius);

                    if (dist <= 0)
                    {
                        float t = (float)cy / (contentH - 1);
                        Color fill = Color.Lerp(bottomColor, topColor, t);

                        if (cy > contentH * 0.5f && dist < -borderPx)
                        {
                            float highlightT = Mathf.Clamp01((float)(cy - contentH * 0.5f) / (contentH * 0.5f));
                            float edgeT = Mathf.Clamp01(1f - (Mathf.Abs(dist + radius) / radius));
                            fill = Color.Lerp(fill, Color.white, highlightT * highlightIntensity * edgeT);
                        }

                        if (hasBorder && dist > -borderPx)
                        {
                            float borderT = Mathf.Clamp01(1f - (Mathf.Abs(dist) / borderPx));
                            pixels[idx] = Color32.Lerp(fill, borderColor, borderT);
                        }
                        else
                        {
                            pixels[idx] = fill;
                        }

                        if (dist > -1.0f)
                        {
                            float aa = Mathf.Clamp01(-dist);
                            var col = (Color)pixels[idx];
                            col.a *= aa;
                            pixels[idx] = col;
                        }
                    }
                    else
                    {
                        if (hasShadow && dist < shadowBlur)
                        {
                            float shadowT = 1f - (dist / shadowBlur);
                            shadowT = shadowT * shadowT;
                            pixels[idx] = new Color(0, 0, 0, shadowT * shadowAlpha);
                        }
                        else
                        {
                            pixels[idx] = new Color32(0, 0, 0, 0);
                        }
                    }
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();
            _textureCache[hash] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateSolid(Color color)
        {
            var key = new TextureKey(1, 1, 0, color, color, Color.clear, 0);
            int hash = key.GetHashCode();

            if (_textureCache.TryGetValue(hash, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, color);
            tex.Apply();

            _textureCache[hash] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateVerticalGradient(int width, int height, Color top, Color bottom)
        {
            var key = new TextureKey(width, height, 0, top, bottom, Color.clear, 0);
            int hash = key.GetHashCode();

            if (_textureCache.TryGetValue(hash, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color32[width * height];

            for (int y = 0; y < height; y++)
            {
                float t = (float)y / (height - 1);
                Color32 c = Color.Lerp(bottom, top, t);
                for (int x = 0; x < width; x++)
                {
                    pixels[y * width + x] = c;
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();

            _textureCache[hash] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateGlow(int size, Color color)
        {
            var key = new TextureKey(size, size, 0, color, color, Color.clear, -1);
            int hash = key.GetHashCode();

            if (_textureCache.TryGetValue(hash, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color32[size * size];
            float center = size * 0.5f;
            float maxR = size * 0.45f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist <= maxR)
                    {
                        float t = dist / maxR;
                        float falloff = 1f - (t * t);
                        falloff = falloff * falloff * (3f - 2f * falloff);
                        pixels[y * size + x] = new Color(color.r, color.g, color.b, color.a * falloff * 0.7f);
                    }
                    else
                    {
                        pixels[y * size + x] = new Color32(0, 0, 0, 0);
                    }
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();

            _textureCache[hash] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public float GetSDF(float x, float y, float width, float height, float radius)
        {
            if (radius <= 0)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                    return -Mathf.Min(x, width - x, y, height - y);
                return 1f;
            }

            float centerW = width - 2 * radius;
            float centerH = height - 2 * radius;

            if (centerW < 0)
                centerW = 0;
            if (centerH < 0)
                centerH = 0;

            float qx = Mathf.Abs(x - width / 2f) - centerW / 2f;
            float qy = Mathf.Abs(y - height / 2f) - centerH / 2f;

            float offsetX = Mathf.Max(qx, 0);
            float offsetY = Mathf.Max(qy, 0);

            float dist = Mathf.Sqrt(offsetX * offsetX + offsetY * offsetY);

            float insideDist = Mathf.Min(Mathf.Max(qx, qy), 0);

            return (dist + insideDist) - radius;
        }

        public void DestroyAllTextures()
        {
            for (int i = 0; i < _activeTextures.Count; i++)
            {
                if (_activeTextures[i] != null)
                {
                    Object.DestroyImmediate(_activeTextures[i]);
                }
            }
            _activeTextures.Clear();
            _textureCache.Clear();
        }

        public void Cleanup() => DestroyAllTextures();
    }
}
