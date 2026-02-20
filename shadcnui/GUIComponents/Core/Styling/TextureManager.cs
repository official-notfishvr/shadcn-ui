using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public class TextureManager
    {
        private readonly GUIHelper _guiHelper;
        private readonly Dictionary<TextureKey, Texture2D> _textureCache = new();
        private readonly List<Texture2D> _activeTextures = new();
        private float _cachedScale;

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
            public readonly float HighlightIntensity;

            public TextureKey(int w, int h, int r, Color c1, Color c2, Color bc, float bt, float ss = 0, int sb = 0, float hi = 0)
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
                HighlightIntensity = hi;
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
                    && ShadowBlur == other.ShadowBlur
                    && HighlightIntensity.Equals(other.HighlightIntensity);
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
                    hash = hash * 31 + HighlightIntensity.GetHashCode();
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
            _cachedScale = _guiHelper.uiScale;

            int sizeDef = DesignTokens.TextureSize.Default;
            int sizeSm = DesignTokens.TextureSize.Small;
            int sizeLg = DesignTokens.TextureSize.Large;
            int sizeMd = DesignTokens.TextureSize.Medium;

            int rMd = Mathf.RoundToInt(DesignTokens.Radius.MD * _cachedScale);
            int rSm = Mathf.RoundToInt(DesignTokens.Radius.SM * _cachedScale);
            int rLg = Mathf.RoundToInt(DesignTokens.Radius.LG * _cachedScale);
            int rXl = Mathf.RoundToInt(DesignTokens.Radius.XL * _cachedScale);

            Gradient = GenerateVerticalGradient(1, 64, theme.Base, theme.Secondary);

            Color cardTop = Color.Lerp(theme.Elevated, Color.white, 0.04f);
            Color cardBottom = Color.Lerp(theme.Elevated, Color.black, 0.05f);
            CardBackground = GenerateStyledShape(sizeDef, sizeDef, rLg, cardTop, cardBottom, theme.Border, 0.5f, 0.16f, 10, 0.07f);

            Color focusFill = new Color(theme.Base.r, theme.Base.g, theme.Base.b, 0.98f);
            InputFocused = GenerateStyledShape(sizeDef, (int)DesignTokens.Height.Default, rMd, focusFill, focusFill, theme.Accent, 2.5f, 0.06f, 6, 0.04f);

            Transparent = GenerateSolid(Color.clear);

            Glow = GenerateGlow(sizeSm, theme.Accent);

            Particle = GenerateSolid(Color.Lerp(theme.Accent, theme.Text, 0.3f));

            Color progressBg = Color.Lerp(theme.Secondary, theme.Base, 0.25f);
            Color progressBottom = Color.Lerp(theme.Secondary, Color.black, 0.12f);
            ProgressBarBackground = GenerateStyledShape(sizeDef, DesignTokens.ProgressBar.TextureHeight, rSm, progressBg, progressBottom, Color.clear, 0, 0.05f, 4, 0.035f);

            Separator = GenerateSolid(new Color(theme.Border.r, theme.Border.g, theme.Border.b, 0.85f));

            Color tabsActiveTop = Color.Lerp(theme.TabsTriggerActiveBg, Color.white, 0.04f);
            Color tabsActiveBottom = Color.Lerp(theme.TabsTriggerActiveBg, Color.black, 0.05f);
            TabsActive = GenerateShape(sizeDef, sizeDef, rSm, tabsActiveTop, tabsActiveBottom, Color.clear, 0);

            Color badgeTop = Color.Lerp(theme.Accent, Color.white, 0.07f);
            Color badgeBottom = Color.Lerp(theme.Accent, Color.black, 0.07f);
            Badge = GenerateStyledShape(sizeMd, (int)DesignTokens.Badge.Height, rXl, badgeTop, badgeBottom, Color.clear, 0, 0.10f, 8, 0.06f);

            TableCell = GenerateSolid(theme.Base);

            Color tableHeaderTop = Color.Lerp(theme.Accent, Color.white, 0.08f);
            Color tableHeaderBottom = Color.Lerp(theme.Accent, Color.black, 0.05f);
            TableHeader = GenerateStyledShape(sizeDef, (int)DesignTokens.Height.Default, rSm, tableHeaderTop, tableHeaderBottom, theme.Border, 0.5f, 0.08f, 4, 0.06f);

            Color tableRowTop = Color.Lerp(theme.Base, Color.white, 0.01f);
            Color tableRowBottom = Color.Lerp(theme.Base, Color.black, 0.01f);
            TableRow = GenerateStyledShape(sizeDef, (int)DesignTokens.Height.Default, 0, tableRowTop, tableRowBottom, theme.Border, 0.3f, 0.04f, 2, 0.02f);

            Color tableRowAltTop = Color.Lerp(theme.Secondary, Color.white, 0.02f);
            Color tableRowAltBottom = Color.Lerp(theme.Secondary, Color.black, 0.02f);
            TableRowAlternate = GenerateStyledShape(sizeDef, (int)DesignTokens.Height.Default, 0, tableRowAltTop, tableRowAltBottom, theme.Border, 0.3f, 0.04f, 2, 0.02f);

            Color dropdownTop = Color.Lerp(theme.Elevated, Color.white, 0.025f);
            Color dropdownBottom = Color.Lerp(theme.Elevated, Color.black, 0.035f);
            DropdownMenuContent = GenerateStyledShape(sizeDef, sizeDef, rMd, dropdownTop, dropdownBottom, theme.Border, 0.6f, 0.14f, 8, 0.06f);

            Color chartBg = theme.Elevated;
            int chartRadius = Mathf.RoundToInt(DesignTokens.Chart.Radius * _cachedScale);
            chartRadius = Mathf.Clamp(chartRadius, 4, sizeLg / 4);
            ChartContainer = GenerateShape(sizeLg, sizeLg, chartRadius, chartBg, chartBg, theme.Border, 1f, 0f, 0);
        }

        public Texture2D GenerateShape(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderPx, float shadowAlpha = 0, int shadowBlur = 0)
        {
            var key = new TextureKey(width, height, radius, topColor, bottomColor, borderColor, borderPx, shadowAlpha, shadowBlur);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
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

                    float dist = GetSdf(cx, cy, contentW, contentH, radius);

                    if (dist <= 0)
                    {
                        float t = contentH > 1 ? (float)cy / (contentH - 1) : 0.5f;
                        float easeT = t * t * (3f - 2f * t);
                        Color fill = Color.Lerp(bottomColor, topColor, easeT);

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
                            Color col = pixels[idx];
                            col.a *= aa;
                            pixels[idx] = col;
                        }
                    }
                    else
                    {
                        if (hasShadow && dist < shadowBlur)
                        {
                            float shadowT = 1f - (dist / shadowBlur);
                            shadowT = shadowT * shadowT * shadowT;
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
            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateStyledShape(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderPx, float shadowAlpha = 0, int shadowBlur = 0, float highlightIntensity = 0.1f)
        {
            var key = new TextureKey(width, height, radius, topColor, bottomColor, borderColor, borderPx, shadowAlpha, shadowBlur, highlightIntensity);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
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

                    float dist = GetSdf(cx, cy, contentW, contentH, radius);

                    if (dist <= 0)
                    {
                        float t = contentH > 1 ? (float)cy / (contentH - 1) : 0.5f;
                        float easeT = t * t * (3f - 2f * t);
                        Color fill = Color.Lerp(bottomColor, topColor, easeT);

                        if (cy < contentH * 0.3f && dist < -borderPx * 0.5f)
                        {
                            float rimT = Mathf.Clamp01((contentH * 0.3f - cy) / (contentH * 0.3f));
                            float edgeT = Mathf.Clamp01(1f - (Mathf.Abs(dist) / (radius + 1)));
                            fill = Color.Lerp(fill, Color.white, rimT * highlightIntensity * 0.6f * edgeT);
                        }

                        if (cy > contentH * 0.7f && dist < -borderPx * 0.5f)
                        {
                            float depthT = Mathf.Clamp01((cy - contentH * 0.7f) / (contentH * 0.3f));
                            float edgeT = Mathf.Clamp01(1f - (Mathf.Abs(dist) / (radius + 1)));
                            fill = Color.Lerp(fill, Color.black, depthT * highlightIntensity * 0.4f * edgeT);
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
                            Color col = pixels[idx];
                            col.a *= aa;
                            pixels[idx] = col;
                        }
                    }
                    else
                    {
                        if (hasShadow && dist < shadowBlur)
                        {
                            float shadowT = 1f - (dist / shadowBlur);
                            shadowT = shadowT * shadowT * shadowT;
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
            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateSolid(Color color)
        {
            var key = new TextureKey(1, 1, 0, color, color, Color.clear, 0);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, color);
            tex.Apply();

            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateVerticalGradient(int width, int height, Color top, Color bottom)
        {
            var key = new TextureKey(width, height, 0, top, bottom, Color.clear, 0);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color32[width * height];

            for (int y = 0; y < height; y++)
            {
                float t = height > 1 ? (float)y / (height - 1) : 0.5f;
                float easeT = t * t * (3f - 2f * t);
                Color32 c = Color.Lerp(bottom, top, easeT);

                for (int x = 0; x < width; x++)
                {
                    pixels[y * width + x] = c;
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();

            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateGlow(int size, Color color)
        {
            var key = new TextureKey(size, size, 0, color, color, Color.clear, -1);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
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
                        falloff = falloff * falloff * falloff * (4f - 3f * falloff);
                        float intensity = falloff * 0.85f;
                        pixels[y * size + x] = new Color(color.r, color.g, color.b, color.a * intensity);
                    }
                    else
                    {
                        pixels[y * size + x] = new Color32(0, 0, 0, 0);
                    }
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();

            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public float GetSdf(float x, float y, float width, float height, float radius)
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
            foreach (var tex in _activeTextures)
            {
                if (tex != null)
                    UnityEngine.Object.DestroyImmediate(tex);
            }

            _activeTextures.Clear();
            _textureCache.Clear();
        }

        public void Cleanup() => DestroyAllTextures();

        public Texture2D GenerateAvatarTexture(int size, int radius, Color backgroundColor, Color borderColor, float borderThickness, bool withShadow = true)
        {
            var key = new TextureKey(size, size, radius, backgroundColor, borderColor, borderColor, borderThickness, withShadow ? 0.2f : 0, withShadow ? 10 : 0);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color32[size * size];
            int shadowBlur = withShadow ? 10 : 0;
            int contentSize = size - shadowBlur;
            int offset = shadowBlur / 2;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int idx = y * size + x;
                    int cx = x - offset;
                    int cy = y - offset;

                    float dist = GetSdf(cx, cy, contentSize, contentSize, radius);

                    if (dist <= 0)
                    {
                        float t = contentSize > 1 ? (float)cy / (contentSize - 1) : 0.5f;
                        float easeT = t * t * (3f - 2f * t);
                        Color fill = Color.Lerp(backgroundColor, backgroundColor, easeT);

                        if (cy < contentSize * 0.2f && dist < -borderThickness * 0.5f)
                        {
                            float rimT = Mathf.Clamp01((contentSize * 0.2f - cy) / (contentSize * 0.2f));
                            float edgeT = Mathf.Clamp01(1f - (Mathf.Abs(dist) / (radius + 1)));
                            fill = Color.Lerp(fill, Color.white, rimT * 0.15f * edgeT);
                        }

                        if (dist > -borderThickness)
                        {
                            float borderT = Mathf.Clamp01(1f - (Mathf.Abs(dist) / borderThickness));
                            pixels[idx] = Color32.Lerp(fill, borderColor, borderT * 0.8f);
                        }
                        else
                        {
                            pixels[idx] = fill;
                        }

                        if (dist > -1.5f)
                        {
                            float aa = Mathf.Clamp01(-dist / 1.5f);
                            Color col = pixels[idx];
                            col.a *= aa;
                            pixels[idx] = col;
                        }
                    }
                    else
                    {
                        if (withShadow && dist < shadowBlur)
                        {
                            float shadowT = 1f - (dist / shadowBlur);
                            shadowT = shadowT * shadowT * shadowT * shadowT;
                            pixels[idx] = new Color(0, 0, 0, shadowT * 0.2f);
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
            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateStatusIndicator(int size, bool isOnline)
        {
            Color statusColor = isOnline ? new Color(0.2f, 0.8f, 0.3f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f);
            Color borderColor = Color.white;

            var key = new TextureKey(size, size, size / 2, statusColor, statusColor, borderColor, 1.5f);

            if (_textureCache.TryGetValue(key, out var cached) && cached != null)
                return cached;

            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color32[size * size];
            float center = size * 0.5f;
            float radius = size * 0.4f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int idx = y * size + x;
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist <= radius)
                    {
                        if (dist > radius - 1.5f)
                        {
                            float borderT = Mathf.Clamp01((radius - dist) / 1.5f);
                            pixels[idx] = Color32.Lerp(statusColor, borderColor, 1f - borderT);
                        }
                        else
                        {
                            pixels[idx] = statusColor;
                        }

                        if (dist > radius - 2.0f)
                        {
                            float aa = Mathf.Clamp01((radius - dist) / 2.0f);
                            Color col = pixels[idx];
                            col.a *= aa;
                            pixels[idx] = col;
                        }
                    }
                    else
                    {
                        pixels[idx] = new Color32(0, 0, 0, 0);
                    }
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();
            _textureCache[key] = tex;
            _activeTextures.Add(tex);
            return tex;
        }

        public Texture2D GenerateTableHeaderTexture(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor, float borderThickness = 0.5f)
        {
            return GenerateStyledShape(width, height, radius, topColor, bottomColor, borderColor, borderThickness, 0.08f, 4, 0.06f);
        }

        public Texture2D GenerateTableRowTexture(int width, int height, Color topColor, Color bottomColor, Color borderColor, float borderThickness = 0.3f)
        {
            return GenerateStyledShape(width, height, 0, topColor, bottomColor, borderColor, borderThickness, 0.04f, 2, 0.02f);
        }

        public Texture2D GenerateTableCellTexture(Color cellColor)
        {
            return GenerateSolid(cellColor);
        }

        public void DrawTabUnderlineIndicator(Rect tabRect, Color color, bool isVertical, bool isLeft, float indicatorHeight, float uiScale)
        {
            Rect indicatorRect;

            if (isVertical)
            {
                float scaledHeight = indicatorHeight * uiScale;
                if (isLeft)
                {
                    indicatorRect = new Rect(tabRect.x + tabRect.width - scaledHeight, tabRect.y, scaledHeight, tabRect.height);
                }
                else
                {
                    indicatorRect = new Rect(tabRect.x, tabRect.y, scaledHeight, tabRect.height);
                }
            }
            else
            {
                float scaledHeight = indicatorHeight * uiScale;
                indicatorRect = new Rect(tabRect.x, tabRect.y + tabRect.height - scaledHeight, tabRect.width, scaledHeight);
            }

            GUI.color = color;
            GUI.DrawTexture(indicatorRect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        public void DrawTabBackgroundIndicator(Rect tabRect, Color color)
        {
            var bgColor = new Color(color.r, color.g, color.b, 0.1f);
            GUI.color = bgColor;
            GUI.DrawTexture(tabRect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        public void DrawTabBorderIndicator(Rect tabRect, Color color, bool isVertical, bool isLeft, float borderWidth, float uiScale)
        {
            float scaledBorderWidth = borderWidth * uiScale;
            GUI.color = color;

            if (isVertical)
            {
                if (isLeft)
                {
                    GUI.DrawTexture(new Rect(tabRect.x + tabRect.width - scaledBorderWidth, tabRect.y, scaledBorderWidth, tabRect.height), Texture2D.whiteTexture);
                }
                else
                {
                    GUI.DrawTexture(new Rect(tabRect.x, tabRect.y, scaledBorderWidth, tabRect.height), Texture2D.whiteTexture);
                }
            }
            else
            {
                GUI.DrawTexture(new Rect(tabRect.x, tabRect.y + tabRect.height - scaledBorderWidth, tabRect.width, scaledBorderWidth), Texture2D.whiteTexture);
            }

            GUI.color = Color.white;
        }
    }
}
