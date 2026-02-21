using UnityEngine;
using ShadcnUI.Core.Theming;

namespace ShadcnUI.Core.Styling;

/// <summary>
/// Provides GUI styles with caching for performance.
/// </summary>
public sealed class StyleProvider : IStyleProvider
{
    private readonly IUIService _services;
    private readonly Dictionary<StyleKey, CachedStyle> _styleCache = new();
    private readonly Dictionary<float, int> _scaledRadiusCache = new();
    private readonly Dictionary<float, int> _scaledSpacingCache = new();
    private readonly Dictionary<float, int> _scaledHeightCache = new();
    private float _lastUIScale = -1f;

    private const int MAX_CACHE_SIZE = 500;
    private const float CACHE_PRUNE_THRESHOLD = 5f;

    private struct CachedStyle
    {
        public GUIStyle Style;
        public float LastAccessTime;
    }

    public StyleProvider(IUIService services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _services.ThemeManager.OnThemeChanged += InvalidateCache;
    }

    public GUIStyle GetStyle(StyleKey key)
    {
        if (_styleCache.TryGetValue(key, out var cached))
        {
            cached.LastAccessTime = Time.realtimeSinceStartup;
            _styleCache[key] = cached;
            return cached.Style;
        }

        var style = CreateStyle(key);
        AddToCache(key, style);
        return style;
    }

    public GUIStyle GetStyle(StyleComponentType type, ControlVariant variant = ControlVariant.Default, 
        ControlSize size = ControlSize.Default, ComponentState state = ComponentState.Default)
    {
        return GetStyle(new StyleKey(type, variant, size, state));
    }

    public void InvalidateCache()
    {
        _styleCache.Clear();
        _scaledRadiusCache.Clear();
        _scaledSpacingCache.Clear();
        _scaledHeightCache.Clear();
    }

    private GUIStyle CreateStyle(StyleKey key)
    {
        var theme = _services.ThemeManager.CurrentTheme;
        
        return key.Type switch
        {
            StyleComponentType.Button => CreateButtonStyle(key, theme),
            StyleComponentType.Input => CreateInputStyle(key, theme),
            StyleComponentType.Label => CreateLabelStyle(key, theme),
            StyleComponentType.Card => CreateCardStyle(key, theme),
            StyleComponentType.CardHeader => CreateCardHeaderStyle(key, theme),
            StyleComponentType.CardTitle => CreateCardTitleStyle(key, theme),
            StyleComponentType.CardDescription => CreateCardDescriptionStyle(key, theme),
            StyleComponentType.CardContent => CreateCardContentStyle(key, theme),
            StyleComponentType.CardFooter => CreateCardFooterStyle(key, theme),
            StyleComponentType.Alert => CreateAlertStyle(key, theme),
            StyleComponentType.Dialog => CreateDialogStyle(key, theme),
            StyleComponentType.Separator => CreateSeparatorStyle(key, theme),
            StyleComponentType.Badge => CreateBadgeStyle(key, theme),
            StyleComponentType.Checkbox => CreateCheckboxStyle(key, theme),
            StyleComponentType.Switch => CreateSwitchStyle(key, theme),
            StyleComponentType.Tabs => CreateTabsStyle(key, theme),
            StyleComponentType.TabsTrigger => CreateTabsTriggerStyle(key, theme),
            StyleComponentType.TabsContent => CreateTabsContentStyle(key, theme),
            StyleComponentType.Progress => CreateProgressBarStyle(key, theme),
            StyleComponentType.Slider => CreateSliderStyle(key, theme),
            StyleComponentType.Select => CreateSelectStyle(key, theme),
            StyleComponentType.Avatar => CreateAvatarStyle(key, theme),
            StyleComponentType.Tooltip => CreateTooltipStyle(key, theme),
            StyleComponentType.Navigation => CreateNavigationStyle(key, theme),
            _ => new GUIStyle(GUI.skin.button)
        };
    }

    private GUIStyle CreateButtonStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.button);
        var (bg, fg, border) = GetButtonColors(key.Variant, theme);
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        // Use larger textures for better quality when scaled
        System.Func<Color, Texture2D> btnTex = c => CreateGradientTexture(128, 64, radius, c);
        
        style.normal.background = btnTex(bg);
        style.normal.textColor = fg;
        
        // Hover: lighter
        style.hover.background = btnTex(Color.Lerp(bg, Color.white, 0.1f));
        style.hover.textColor = fg;
        
        // Active: darker
        style.active.background = btnTex(Color.Lerp(bg, Color.black, 0.15f));
        style.active.textColor = fg;
        
        // Focused
        style.focused.background = btnTex(bg);
        style.focused.textColor = fg;
        
        style.border = new RectOffset(1, 1, 1, 1);
        style.padding = GetButtonPadding(key.Size);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = GetScaledFontSize(GetFontSizeForControlSize(key.Size));
        style.wordWrap = false;
        style.clipping = TextClipping.Clip;
        
        return style;
    }

    #region Texture Factories (OldCode Pattern)
    
    private Texture2D CreateGradientTexture(int width, int height, int radius, Color baseColor)
    {
        var topColor = Color.Lerp(baseColor, Color.white, 0.06f);
        var bottomColor = Color.Lerp(baseColor, Color.black, 0.08f);
        return CreateTexture(width, height, radius, topColor, bottomColor);
    }

    private Texture2D CreateSolidTexture(int width, int height, int radius, Color color)
    {
        return CreateTexture(width, height, radius, color, color);
    }

    private Texture2D CreateTexture(int width, int height, int radius, Color topColor, Color bottomColor, Color borderColor = default, float borderThickness = 0)
    {
        int w = Mathf.Max(4, width);
        int h = Mathf.Max(4, height);
        var texture = new Texture2D(w, h, TextureFormat.RGBA32, false);
        
        var pixels = new Color[w * h];
        int r = Mathf.Min(radius, w / 2, h / 2);
        
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                // Gradient from top to bottom
                float t = y / (float)(h - 1);
                var baseColor = Color.Lerp(topColor, bottomColor, t);
                
                // Rounded corners with smooth anti-aliasing
                float alpha = 1f;
                if (r > 0)
                {
                    float dx = 0, dy = 0;
                    
                    if (x < r && y < r)
                    {
                        dx = r - x - 0.5f;
                        dy = r - y - 0.5f;
                    }
                    else if (x >= w - r && y < r)
                    {
                        dx = x - (w - r) + 0.5f;
                        dy = r - y - 0.5f;
                    }
                    else if (x < r && y >= h - r)
                    {
                        dx = r - x - 0.5f;
                        dy = y - (h - r) + 0.5f;
                    }
                    else if (x >= w - r && y >= h - r)
                    {
                        dx = x - (w - r) + 0.5f;
                        dy = y - (h - r) + 0.5f;
                    }
                    
                    if (dx > 0 && dy > 0)
                    {
                        float dist = Mathf.Sqrt(dx * dx + dy * dy);
                        if (dist > r)
                        {
                            // Smooth anti-aliased edge
                            alpha = Mathf.Clamp01(1f - (dist - r) / 1.5f);
                        }
                    }
                }
                
                // Border
                var finalColor = baseColor;
                if (borderThickness > 0 && borderColor.a > 0)
                {
                    bool isBorder = x < borderThickness || x >= w - borderThickness || 
                                    y < borderThickness || y >= h - borderThickness;
                    if (isBorder && alpha > 0)
                    {
                        finalColor = Color.Lerp(baseColor, borderColor, borderColor.a);
                    }
                }
                
                finalColor.a *= alpha;
                pixels[y * w + x] = finalColor;
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return texture;
    }

    private void ApplyRoundedCorners(Color[] pixels, int width, int height, int radius)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int dx = 0, dy = 0;
                
                if (x < radius && y < radius)
                {
                    dx = radius - x;
                    dy = radius - y;
                }
                else if (x >= width - radius && y < radius)
                {
                    dx = x - (width - 1 - radius);
                    dy = radius - y;
                }
                else if (x < radius && y >= height - radius)
                {
                    dx = radius - x;
                    dy = y - (height - 1 - radius);
                }
                else if (x >= width - radius && y >= height - radius)
                {
                    dx = x - (width - 1 - radius);
                    dy = y - (height - 1 - radius);
                }
                
                if (dx > 0 && dy > 0)
                {
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    if (dist > radius)
                    {
                        pixels[y * width + x] = Color.clear;
                    }
                }
            }
        }
    }

    #endregion

    private GUIStyle CreateInputStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.textField);
        var bg = theme.Input;
        var fg = theme.Foreground;
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        // Use flat texture for input (no gradient needed)
        style.normal.background = CreateSolidTexture(64, 32, radius, bg);
        style.normal.textColor = fg;
        style.focused.background = CreateSolidTexture(64, 32, radius, bg);
        style.focused.textColor = fg;
        style.hover.background = CreateSolidTexture(64, 32, radius, bg);
        style.hover.textColor = fg;
        
        style.border = new RectOffset(1, 1, 1, 1);
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD),
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM));
        style.alignment = TextAnchor.MiddleLeft;
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.Default);
        
        return style;
    }

    private GUIStyle CreateLabelStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.label);
        
        var color = key.Variant switch
        {
            ControlVariant.Muted => theme.MutedForeground,
            ControlVariant.Primary => theme.PrimaryForeground,
            ControlVariant.Secondary => theme.SecondaryForeground,
            ControlVariant.Destructive => theme.DestructiveForeground,
            _ => theme.Foreground
        };
        
        style.normal.textColor = color;
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.Default);
        style.alignment = TextAnchor.MiddleLeft;
        style.wordWrap = true;
        
        return style;
    }

    private GUIStyle CreateCardStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.LG);
        
        // Card has subtle gradient
        style.normal.background = CreateGradientTexture(128, 64, radius, theme.Card);
        style.normal.textColor = theme.CardForeground;
        style.border = new RectOffset(1, 1, 1, 1);
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG),
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG));
        
        return style;
    }

    private GUIStyle CreateCardHeaderStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle();
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG),
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD));
        return style;
    }

    private GUIStyle CreateCardTitleStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.LG);
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = theme.Foreground;
        return style;
    }

    private GUIStyle CreateCardDescriptionStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.SM);
        style.normal.textColor = theme.MutedForeground;
        return style;
    }

    private GUIStyle CreateCardContentStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle();
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG),
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM));
        return style;
    }

    private GUIStyle CreateCardFooterStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle();
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG),
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD));
        style.alignment = TextAnchor.MiddleLeft;
        return style;
    }

    private GUIStyle CreateAlertStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        var bgColor = key.Variant switch
        {
            ControlVariant.Destructive => Color.Lerp(theme.Destructive, theme.Background, 0.9f),
            ControlVariant.Secondary => Color.Lerp(theme.Secondary, theme.Background, 0.9f),
            _ => Color.Lerp(theme.Primary, theme.Background, 0.9f)
        };
        
        style.normal.background = CreateSolidTexture(128, 64, radius, bgColor);
        style.border = new RectOffset(1, 1, 1, 1);
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD),
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD));
        
        return style;
    }

    private GUIStyle CreateDialogStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.XL);
        
        // Dialog uses gradient for elevation effect
        style.normal.background = CreateGradientTexture(128, 64, radius, theme.Card);
        style.normal.textColor = theme.Foreground;
        style.border = new RectOffset(1, 1, 1, 1);
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.XL), 
            GetScaledSpacing(DesignTokens.Spacing.XL),
            GetScaledSpacing(DesignTokens.Spacing.XL), 
            GetScaledSpacing(DesignTokens.Spacing.XL));
        
        return style;
    }

    private GUIStyle CreateTabsContentStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle();
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG),
            GetScaledSpacing(DesignTokens.Spacing.LG), 
            GetScaledSpacing(DesignTokens.Spacing.LG));
        return style;
    }

    private GUIStyle CreateProgressBarStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.LG);
        
        style.normal.background = CreateGradientTexture(64, 32, radius, theme.Secondary);
        style.border = new RectOffset(1, 1, 1, 1);
        return style;
    }

    private GUIStyle CreateSliderStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.horizontalSlider);
        int radius = GetScaledRadius(DesignTokens.Radius.Full);
        
        style.normal.background = CreateSolidTexture(128, 8, radius, theme.Secondary);
        return style;
    }

    private GUIStyle CreateSelectStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.button);
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        // Select uses input background color with gradient
        style.normal.background = CreateGradientTexture(128, 64, radius, theme.Input);
        style.normal.textColor = theme.Foreground;
        style.hover.background = CreateGradientTexture(128, 64, radius, Color.Lerp(theme.Input, Color.white, 0.05f));
        style.hover.textColor = theme.Foreground;
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD),
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM));
        return style;
    }

    private GUIStyle CreateAvatarStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.Full);
        
        style.normal.background = CreateGradientTexture(64, 64, radius, theme.Secondary);
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }

    private GUIStyle CreateTooltipStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        style.normal.background = CreateGradientTexture(128, 64, radius, theme.Card);
        style.normal.textColor = theme.Foreground;
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM),
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM));
        return style;
    }

    private GUIStyle CreateNavigationStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        style.normal.background = CreateGradientTexture(64, 32, radius, theme.Secondary);
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM),
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM));
        return style;
    }

    private GUIStyle CreateSeparatorStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle();
        int radius = 0;
        style.normal.background = CreateSolidTexture(64, 2, radius, theme.Border);
        return style;
    }

    private GUIStyle CreateBadgeStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.label);
        var (bg, fg, _) = GetButtonColors(key.Variant, theme);
        int radius = GetScaledRadius(DesignTokens.Radius.SM);
        
        style.normal.background = CreateGradientTexture(32, 16, radius, bg);
        style.normal.textColor = fg;
        style.border = new RectOffset(1, 1, 1, 1);
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM),
            GetScaledSpacing(DesignTokens.Spacing.XS), 
            GetScaledSpacing(DesignTokens.Spacing.XS));
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.XS);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        
        return style;
    }

    private GUIStyle CreateCheckboxStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.toggle);
        
        style.normal.textColor = theme.Foreground;
        style.onNormal.textColor = theme.Foreground;
        style.hover.textColor = theme.Foreground;
        style.onHover.textColor = theme.Foreground;
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.Default);
        
        return style;
    }

    private GUIStyle CreateSwitchStyle(StyleKey key, Theme theme)
    {
        return CreateCheckboxStyle(key, theme);
    }

    private GUIStyle CreateTabsStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.box);
        int radius = GetScaledRadius(DesignTokens.Radius.MD);
        
        style.normal.background = CreateGradientTexture(64, 32, radius, theme.Muted);
        return style;
    }

    private GUIStyle CreateTabsTriggerStyle(StyleKey key, Theme theme)
    {
        var style = new GUIStyle(GUI.skin.button);
        int radius = GetScaledRadius(DesignTokens.Radius.SM);
        
        // Transparent normal state
        style.normal.background = CreateSolidTexture(64, 32, radius, Color.clear);
        style.normal.textColor = theme.MutedForeground;
        
        // Hover shows accent
        style.hover.background = CreateSolidTexture(64, 32, radius, theme.Accent);
        style.hover.textColor = theme.AccentForeground;
        
        // Active/selected shows background
        style.onNormal.background = CreateSolidTexture(64, 32, radius, theme.Background);
        style.onNormal.textColor = theme.Foreground;
        
        style.padding = new RectOffset(
            GetScaledSpacing(DesignTokens.Spacing.MD), 
            GetScaledSpacing(DesignTokens.Spacing.MD),
            GetScaledSpacing(DesignTokens.Spacing.SM), 
            GetScaledSpacing(DesignTokens.Spacing.SM));
        style.fontSize = GetScaledFontSize(DesignTokens.FontSize.SM);
        
        return style;
    }

    private (Color bg, Color fg, Color border) GetButtonColors(ControlVariant variant, Theme theme)
    {
        return variant switch
        {
            ControlVariant.Primary => (theme.Primary, theme.PrimaryForeground, theme.Border),
            ControlVariant.Secondary => (theme.Secondary, theme.SecondaryForeground, theme.Border),
            ControlVariant.Destructive => (theme.Destructive, theme.DestructiveForeground, theme.Destructive),
            ControlVariant.Outline => (Color.clear, theme.Foreground, theme.Border),
            ControlVariant.Ghost => (Color.clear, theme.Foreground, Color.clear),
            ControlVariant.Link => (Color.clear, theme.Accent, Color.clear),
            _ => (theme.Primary, theme.PrimaryForeground, theme.Border)
        };
    }

    private RectOffset GetButtonPadding(ControlSize size)
    {
        var (h, v) = size switch
        {
            ControlSize.Mini => (DesignTokens.Spacing.SM, DesignTokens.Spacing.XS),
            ControlSize.Small => (DesignTokens.Spacing.MD, DesignTokens.Spacing.SM),
            ControlSize.Large => (DesignTokens.Spacing.LG, DesignTokens.Spacing.MD),
            _ => (DesignTokens.Spacing.MD, DesignTokens.Spacing.SM)
        };
        
        return new RectOffset(GetScaledSpacing(h), GetScaledSpacing(h), GetScaledSpacing(v), GetScaledSpacing(v));
    }

    private float GetFontSizeForControlSize(ControlSize size)
    {
        return size switch
        {
            ControlSize.Mini => DesignTokens.FontSize.XS,
            ControlSize.Small => DesignTokens.FontSize.SM,
            ControlSize.Large => DesignTokens.FontSize.LG,
            _ => DesignTokens.FontSize.Default
        };
    }

    private void AddToCache(StyleKey key, GUIStyle style)
    {
        PruneOldCacheEntries();
        _styleCache[key] = new CachedStyle { Style = style, LastAccessTime = Time.realtimeSinceStartup };
    }

    private void PruneOldCacheEntries()
    {
        if (_styleCache.Count < MAX_CACHE_SIZE * 0.9f) return;
        
        var currentTime = Time.realtimeSinceStartup;
        var toRemove = _styleCache
            .Where(kvp => currentTime - kvp.Value.LastAccessTime > CACHE_PRUNE_THRESHOLD)
            .Select(kvp => kvp.Key)
            .ToList();
        
        foreach (var key in toRemove)
        {
            _styleCache.Remove(key);
        }
    }

    private void InvalidateScaleCaches()
    {
        if (Mathf.Abs(_services.UIScale - _lastUIScale) > 0.001f)
        {
            _scaledRadiusCache.Clear();
            _scaledSpacingCache.Clear();
            _scaledHeightCache.Clear();
            _lastUIScale = _services.UIScale;
        }
    }

    public int GetScaledRadius(float radius)
    {
        InvalidateScaleCaches();
        
        if (_scaledRadiusCache.TryGetValue(radius, out var cached))
            return cached;
        
        int scaled = Mathf.RoundToInt(radius * _services.UIScale);
        _scaledRadiusCache[radius] = scaled;
        return scaled;
    }

    public int GetScaledSpacing(float spacing)
    {
        InvalidateScaleCaches();
        
        if (_scaledSpacingCache.TryGetValue(spacing, out var cached))
            return cached;
        
        int scaled = Mathf.RoundToInt(spacing * _services.UIScale);
        _scaledSpacingCache[spacing] = scaled;
        return scaled;
    }

    public int GetScaledHeight(float height)
    {
        InvalidateScaleCaches();
        
        if (_scaledHeightCache.TryGetValue(height, out var cached))
            return cached;
        
        int scaled = Mathf.RoundToInt(height * _services.UIScale);
        _scaledHeightCache[height] = scaled;
        return scaled;
    }

    public int GetScaledFontSize(float baseSize)
    {
        return Mathf.RoundToInt(baseSize * _services.UIScale);
    }

    private Texture2D CreateTexture(int width, int height, int radius, Color color)
    {
        int w = Mathf.Max(4, width);
        int h = Mathf.Max(4, height);
        var texture = new Texture2D(w, h, TextureFormat.RGBA32, false);
        
        var pixels = new Color[w * h];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        
        if (radius > 0)
        {
            ApplyRoundedCorners(pixels, w, h, radius, color);
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return texture;
    }

    private void ApplyRoundedCorners(Color[] pixels, int width, int height, int radius, Color color)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int dx = 0, dy = 0;
                
                if (x < radius && y < radius)
                {
                    dx = radius - x;
                    dy = radius - y;
                }
                else if (x >= width - radius && y < radius)
                {
                    dx = x - (width - 1 - radius);
                    dy = radius - y;
                }
                else if (x < radius && y >= height - radius)
                {
                    dx = radius - x;
                    dy = y - (height - 1 - radius);
                }
                else if (x >= width - radius && y >= height - radius)
                {
                    dx = x - (width - 1 - radius);
                    dy = y - (height - 1 - radius);
                }
                
                if (dx > 0 && dy > 0)
                {
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    if (dist > radius)
                    {
                        pixels[y * width + x] = Color.clear;
                    }
                }
            }
        }
    }
}
