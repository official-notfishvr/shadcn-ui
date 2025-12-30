using System;
using System.Collections.Generic;
using System.IO;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core.Styling
{
    #region Enums
    public enum ControlVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline,
        Ghost,
        Link,
        Muted,
    }

    public enum ControlSize
    {
        Default,
        Small,
        Large,
        Icon,
        Mini,
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical,
    }

    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded,
    }

    public enum StyleComponentType
    {
        Button,
        Toggle,
        Input,
        Label,
        PasswordField,
        TextArea,
        ProgressBar,
        Separator,
        TabsList,
        TabsTrigger,
        TabsContent,
        Checkbox,
        Switch,
        Badge,
        Avatar,
        Table,
        TableRow,
        TableHeader,
        TableCell,
        Calendar,
        CalendarWeekday,
        CalendarDay,
        CalendarDaySelected,
        CalendarDayOutsideMonth,
        CalendarDayToday,
        CalendarDayInRange,
        DatePicker,
        DatePickerWeekday,
        DatePickerDay,
        DatePickerDaySelected,
        DatePickerDayOutsideMonth,
        DatePickerDayToday,
        Dialog,
        Chart,
        ChartAxis,
        MenuBar,
        MenuBarItem,
        MenuDropdown,
        SelectContent,
        SelectItem,
        DropdownMenu,
        DropdownMenuItem,
        Popover,
        AnimatedBox,
        SectionHeader,
        Card,
        CardHeader,
        CardTitle,
        CardDescription,
        CardContent,
        CardFooter,
        Toast,
        ToastTitle,
        ToastDescription,
        SliderTrack,
        SliderThumb,
        SliderFill,
    }

    public struct StyleKey : IEquatable<StyleKey>
    {
        public StyleComponentType Type;
        public ControlVariant Variant;
        public ControlSize Size;
        public int State;

        public StyleKey(StyleComponentType type, ControlVariant variant, ControlSize size, int state = 0)
        {
            Type = type;
            Variant = variant;
            Size = size;
            State = state;
        }

        public bool Equals(StyleKey other)
        {
            return Type == other.Type && Variant == other.Variant && Size == other.Size && State == other.State;
        }

        public override bool Equals(object obj)
        {
            return obj is StyleKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)Type;
                hashCode = (hashCode * 397) ^ (int)Variant;
                hashCode = (hashCode * 397) ^ (int)Size;
                hashCode = (hashCode * 397) ^ State;
                return hashCode;
            }
        }
    }
    #endregion

    public partial class StyleManager
    {
        #region Fields
        private GUIHelper guiHelper;
        private bool isInitialized = false;
        public Font customFont;
        public StyleRegistry Registry { get; private set; }
        #endregion

        #region Texture Fields
        private Texture2D gradientTexture;
        private Texture2D glowTexture;
        private Texture2D particleTexture;
        private Texture2D cardBackgroundTexture;
        private Texture2D outlineTexture;
        private Texture2D transparentTexture;
        private Texture2D inputBackgroundTexture;
        private Texture2D inputFocusedTexture;
        private Texture2D progressBarBackgroundTexture;
        private Texture2D progressBarFillTexture;
        private Texture2D separatorTexture;
        private Texture2D tabsBackgroundTexture;
        private Texture2D tabsActiveTexture;
        private Texture2D switchTexture;
        private Texture2D switchOnTexture;
        private Texture2D switchOffTexture;
        private Texture2D badgeTexture;
        private Texture2D avatarTexture;
        private Texture2D tableTexture;
        private Texture2D tableHeaderTexture;
        private Texture2D tableCellTexture;
        private Texture2D calendarBackgroundTexture;
        private Texture2D calendarHeaderTexture;
        private Texture2D calendarDayTexture;
        private Texture2D calendarDaySelectedTexture;
        private Texture2D dropdownMenuContentTexture;
        private Texture2D popoverContentTexture;
        private Texture2D scrollAreaThumbTexture;
        private Texture2D scrollAreaTrackTexture;
        private Texture2D selectTriggerTexture;
        private Texture2D selectContentTexture;
        private Texture2D chartContainerTexture;
        private Texture2D chartGridTexture;
        private Texture2D chartAxisTexture;
        private Texture2D sliderTrackTexture;
        #endregion

        #region Base GUIStyles
        private GUIStyle baseButtonStyle;
        private GUIStyle baseToggleStyle;
        private GUIStyle baseCheckboxStyle;
        private GUIStyle baseSwitchStyle;
        private GUIStyle baseInputStyle;
        private GUIStyle baseLabelStyle;
        private GUIStyle baseBadgeStyle;
        private GUIStyle baseTableStyle;
        private GUIStyle baseCalendarStyle;
        private GUIStyle calendarDayStyle;
        private GUIStyle tableCellStyle;
        private GUIStyle cardStyle;
        private GUIStyle tabsListStyle;
        private GUIStyle tabsTriggerStyle;
        private GUIStyle chartContainerStyle;
        private GUIStyle dialogContentStyle;
        private GUIStyle dropdownContentStyle;
        private GUIStyle dropdownItemStyle;
        private GUIStyle menuBarStyle;
        private GUIStyle progressBarStyle;
        private GUIStyle separatorStyle;
        private GUIStyle avatarStyle;
        internal GUIStyle animatedBoxStyle;
        private GUIStyle baseSliderStyle;
        #endregion

        #region Style Caches
        private Dictionary<StyleKey, GUIStyle> styleCache = new();
        private const int MaxCacheSize = 500;
        private LinkedList<StyleKey> cacheOrder = new LinkedList<StyleKey>();
        private Dictionary<StyleKey, LinkedListNode<StyleKey>> cacheNodes = new Dictionary<StyleKey, LinkedListNode<StyleKey>>();
        #endregion

        #region Texture Caches
        private Dictionary<Color, Texture2D> solidColorTextureCache = new();
        private Dictionary<(Color, Color), Texture2D> outlineButtonTextureCache = new();
        private Dictionary<Color, Texture2D> outlineTextureCache = new();
        private List<Texture2D> trackedTextures = new();
        #endregion

        #region LRU Cache Methods
        private void EvictOldestCacheEntry()
        {
            if (cacheOrder.Count >= MaxCacheSize && cacheOrder.Last != null)
            {
                var oldest = cacheOrder.Last.Value;
                styleCache.Remove(oldest);
                cacheNodes.Remove(oldest);
                cacheOrder.RemoveLast();
            }
        }

        private void TouchCacheEntry(StyleKey key)
        {
            if (cacheNodes.TryGetValue(key, out var node))
            {
                cacheOrder.Remove(node);
                cacheOrder.AddFirst(node);
            }
        }

        private void AddToCacheWithLRU(StyleKey key, GUIStyle style)
        {
            EvictOldestCacheEntry();
            styleCache[key] = style;
            var node = cacheOrder.AddFirst(key);
            cacheNodes[key] = node;
        }
        #endregion

        #region Constructor
        public StyleManager(GUIHelper helper)
        {
            try
            {
                guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
                Registry = new StyleRegistry();

                ThemeManager.Instance.OnThemeChanged += OnThemeChanged;

                GUILogger.LogInfo("StyleManager initialized successfully", "StyleManager.Constructor");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Constructor", "StyleManager");
                throw;
            }
        }
        #endregion

        #region Core Methods
        public Theme GetTheme() => ThemeManager.Instance.CurrentTheme;

        private void OnThemeChanged()
        {
            try
            {
                GUILogger.LogInfo("Theme changed, clearing style cache and recreating textures", "StyleManager.OnThemeChanged");
                styleCache.Clear();
                DestroyTrackedTextures();
                solidColorTextureCache.Clear();
                outlineButtonTextureCache.Clear();
                outlineTextureCache.Clear();

                if (isInitialized)
                {
                    CreateCustomTextures();
                    SetupAllStyles();
                }
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "OnThemeChanged", "StyleManager");
            }
        }

        private void DestroyTrackedTextures()
        {
            try
            {
                int count = trackedTextures.Count;
                foreach (var texture in trackedTextures)
                {
                    if (texture != null)
                    {
                        Object.DestroyImmediate(texture);
                    }
                }
                trackedTextures.Clear();
                GUILogger.LogInfo($"Destroyed {count} tracked textures", "StyleManager.DestroyTrackedTextures");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DestroyTrackedTextures", "StyleManager");
            }
        }

        public void SetCustomFont(byte[] fontData, string fontName = "CustomFont.ttf")
        {
            try
            {
                if (fontData == null || fontData.Length == 0)
                {
                    GUILogger.LogWarning("Font data is null or empty", "StyleManager.SetCustomFont");
                    return;
                }

                string tempPath = Path.Combine(Application.temporaryCachePath, fontName);
                File.WriteAllBytes(tempPath, fontData);

#if IL2CPP_MELONLOADER_PRE57
                GUILogger.LogWarning("IL2CPP: Cannot load dynamic fonts from file easily", "StyleManager.SetCustomFont");
#else
                UnityHelpers.Font loadedFont = new UnityHelpers.Font(tempPath);
                if (loadedFont != null)
                {
                    customFont = loadedFont;
                    GUILogger.LogInfo($"Custom font '{fontName}' loaded successfully", "StyleManager.SetCustomFont");
                }
                else
                {
                    GUILogger.LogWarning($"Failed to create font from bytes for '{fontName}'", "StyleManager.SetCustomFont");
                }

                try
                {
                    File.Delete(tempPath);
                }
                catch (Exception deleteEx)
                {
                    GUILogger.LogWarning($"Failed to delete temporary font file: {deleteEx.Message}", "StyleManager.SetCustomFont");
                }
#endif
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetCustomFont", "StyleManager");
            }
        }

        public void InitializeGUI()
        {
            try
            {
                if (isInitialized)
                {
                    GUILogger.LogWarning("StyleManager already initialized", "StyleManager.InitializeGUI");
                    return;
                }

                GUILogger.LogInfo("Starting StyleManager initialization", "StyleManager.InitializeGUI");

                solidColorTextureCache.Clear();
                outlineButtonTextureCache.Clear();
                outlineTextureCache.Clear();
                styleCache.Clear();
                Registry.Clear();

                CreateCustomTextures();
                SetupAllStyles();
                RegisterDefaultStyles();
                isInitialized = true;
                GUILogger.LogInfo("StyleManager initialization completed successfully", "StyleManager.InitializeGUI");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "InitializeGUI", "StyleManager");
                isInitialized = false;
            }
        }

        internal void SetupAllStyles()
        {
            try
            {
                SetupCardStyles();
                SetupButtonStyle();
                SetupToggleStyle();
                SetupCheckboxStyles();
                SetupSwitchStyles();
                SetupInputStyle();
                SetupLabelStyle();
                SetupProgressBarStyles();
                SetupSeparatorStyles();
                SetupTabsStyles();
                SetupBadgeStyle();
                SetupAvatarStyles();
                SetupTableStyles();
                SetupCalendarStyle();
                SetupDropdownMenuStyles();
                SetupDialogStyles();
                SetupChartStyles();
                SetupMenuBarStyles();
                SetupSliderStyles();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAllStyles", "StyleManager");
            }
        }
        #endregion

        #region Texture Creation
        public Texture2D CreateBorderTexture(Color borderColor, int thickness)
        {
            int size = thickness * 2 + 2;
            var texture = new Texture2D(size, size);
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                bool isBorder = x < thickness || y < thickness || x >= size - thickness || y >= size - thickness;
                texture.SetPixel(x, y, isBorder ? borderColor : Color.clear);
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        private Texture2D CreateGlowTexture(Color glowColor, int size)
        {
            var texture = new Texture2D(size, size);
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float maxDist = size / 2f;
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float t = Mathf.Clamp01(distance / maxDist);
                float alpha = 1f - (t * t * (3f - 2f * t));
                texture.SetPixel(x, y, new Color(glowColor.r, glowColor.g, glowColor.b, alpha * 0.6f));
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        private Texture2D CreateBottomBorderTexture(int width, int height, int borderThickness, Color borderColor, Color fillColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                bool isBorder = y >= height - borderThickness;
                texture.SetPixel(x, y, isBorder ? borderColor : fillColor);
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        public Texture2D CreateSolidTexture(Color color)
        {
            if (solidColorTextureCache.TryGetValue(color, out var cachedTexture))
                return cachedTexture;
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            solidColorTextureCache[color] = texture;
            trackedTextures.Add(texture);
            return texture;
        }

        public Texture2D CreateRoundedRectTexture(int width, int height, int radius, Color color) => CreateGradientRoundedRectTexture(width, height, radius, color, color);

        public Texture2D CreateGradientRoundedRectTexture(int width, int height, int radius, Color topColor, Color bottomColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isCornerArea = (x < radius && y < radius) || (x > width - radius && y < radius) || (x < radius && y > height - radius) || (x > width - radius && y > height - radius);
                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(x, y, width, height, radius);
                        if (Vector2.Distance(new Vector2(x, y), cornerCenter) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }
                    float t = (float)y / (Mathf.Max(1, height - 1));
                    texture.SetPixel(x, y, Color.Lerp(bottomColor, topColor, t));
                }
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        public Texture2D CreateFocusRingTexture(int width, int height, int radius, Color ringColor, float thickness = 2f)
        {
            Texture2D texture = new Texture2D(width, height);
            Color transparent = new Color(ringColor.r, ringColor.g, ringColor.b, 0f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float dist = GetSoftDistanceToRoundedRect(x, y, width, height, radius);

                    if (dist < 0)
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                    else if (dist < thickness)
                    {
                        float alpha = 1f - (dist / thickness);
                        texture.SetPixel(x, y, new Color(ringColor.r, ringColor.g, ringColor.b, ringColor.a * alpha));
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                }
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        private float GetSoftDistanceToRoundedRect(int x, int y, int width, int height, int radius)
        {
            if (x >= radius && x <= width - radius)
                return Mathf.Max(0 - y, y - (height - 1), -y, y - height);
            if (y >= radius && y <= height - radius)
                return Mathf.Max(0 - x, x - (width - 1), -x, x - width);

            Vector2 innerPos = new Vector2(x < radius ? radius : width - radius, y < radius ? radius : height - radius);
            return Vector2.Distance(new Vector2(x, y), innerPos) - radius;
        }

        public Texture2D CreateGradientRoundedRectWithShadowTexture(int width, int height, int radius, Color topColor, Color bottomColor, float shadowIntensity = 0.12f, int shadowBlur = 10)
        {
            Texture2D texture = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                texture.SetPixel(x, y, Color.clear);

            int margin = shadowBlur / 2;
            int innerW = width - shadowBlur;
            int innerH = height - shadowBlur;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int ix = x - margin;
                    int iy = y - margin;

                    if (ix < 0 || iy < 0 || ix >= innerW || iy >= innerH)
                        continue;

                    bool isCornerArea = (ix < radius && iy < radius) || (ix > innerW - radius && iy < radius) || (ix < radius && iy > innerH - radius) || (ix > innerW - radius && iy > innerH - radius);
                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(ix, iy, innerW, innerH, radius);
                        if (Vector2.Distance(new Vector2(ix, iy), cornerCenter) > radius)
                        {
                            continue;
                        }
                    }
                    float t = (float)iy / (Mathf.Max(1, innerH - 1));
                    texture.SetPixel(x, y, Color.Lerp(bottomColor, topColor, t));
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color px = texture.GetPixel(x, y);
                    if (px.a > 0.1f)
                        continue;

                    float minDist = float.MaxValue;
                    int dx = Mathf.Min(x - margin, (width - margin) - x);
                    int dy = Mathf.Min(y - margin, (height - margin) - y);
                    int d = Mathf.Min(dx, dy);

                    if (d < 0)
                    {
                        float dist = Mathf.Abs(d);
                        if (dist < shadowBlur)
                        {
                            float alpha = (1f - (dist / shadowBlur)) * shadowIntensity;
                            texture.SetPixel(x, y, new Color(0, 0, 0, alpha));
                        }
                    }
                }
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        public Texture2D CreateInnerShadowTexture(int width, int height, int radius, Color baseColor, float shadowIntensity = 0.1f, int shadowSize = 4)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isCornerArea = (x < radius && y < radius) || (x > width - radius && y < radius) || (x < radius && y > height - radius) || (x > width - radius && y > height - radius);
                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(x, y, width, height, radius);
                        if (Vector2.Distance(new Vector2(x, y), cornerCenter) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }

                    int distToEdge = Mathf.Min(x, width - 1 - x, y, height - 1 - y);
                    float shadowFactor = 0f;
                    if (distToEdge < shadowSize)
                    {
                        shadowFactor = (1f - (float)distToEdge / shadowSize) * shadowIntensity;
                    }

                    texture.SetPixel(x, y, Color.Lerp(baseColor, Color.black, shadowFactor));
                }
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        private Vector2 GetCornerCenter(int x, int y, int width, int height, int radius)
        {
            if (x < radius && y < radius)
                return new Vector2(radius, radius);
            if (x > width - radius && y < radius)
                return new Vector2(width - radius, radius);
            if (x < radius && y > height - radius)
                return new Vector2(radius, height - radius);
            return new Vector2(width - radius, height - radius);
        }

        public Texture2D CreateRoundedOutlineTexture(int width, int height, int radius, Color borderColor, float thickness = 1f)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isCornerArea = (x < radius && y < radius) || (x > width - radius && y < radius) || (x < radius && y > height - radius) || (x > width - radius && y > height - radius);

                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(x, y, width, height, radius);
                        float dist = Vector2.Distance(new Vector2(x, y), cornerCenter);
                        if (dist > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                        }
                        else if (dist > radius - thickness)
                        {
                            texture.SetPixel(x, y, borderColor);
                        }
                        else
                        {
                            texture.SetPixel(x, y, Color.clear);
                        }
                    }
                    else
                    {
                        if (x < thickness || x >= width - thickness || y < thickness || y >= height - thickness)
                        {
                            texture.SetPixel(x, y, borderColor);
                        }
                        else
                        {
                            texture.SetPixel(x, y, Color.clear);
                        }
                    }
                }
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        public Texture2D CreateBorderedRoundedRectTexture(int width, int height, int radius, Color fillColor, Color borderColor, float borderThickness = 1f)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isCornerArea = (x < radius && y < radius) || (x > width - radius && y < radius) || (x < radius && y > height - radius) || (x > width - radius && y > height - radius);

                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(x, y, width, height, radius);
                        float dist = Vector2.Distance(new Vector2(x, y), cornerCenter);
                        if (dist > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }

                        if (dist > radius - borderThickness)
                            texture.SetPixel(x, y, borderColor);
                        else
                            texture.SetPixel(x, y, fillColor);
                    }
                    else
                    {
                        if (x < borderThickness || x >= width - borderThickness || y < borderThickness || y >= height - borderThickness)
                            texture.SetPixel(x, y, borderColor);
                        else
                            texture.SetPixel(x, y, fillColor);
                    }
                }
            }
            texture.Apply();
            trackedTextures.Add(texture);
            return texture;
        }

        private void CreateCustomTextures()
        {
            try
            {
                GUILogger.LogInfo("Creating custom textures", "StyleManager.CreateCustomTextures");
                var theme = ThemeManager.Instance.CurrentTheme;

                gradientTexture = new Texture2D(1, 100);
                for (int i = 0; i < 100; i++)
                {
                    float t = i / 99f;
                    gradientTexture.SetPixel(0, i, Color.Lerp(theme.Base, theme.Secondary, t * 0.2f));
                }
                gradientTexture.Apply();

                cardBackgroundTexture = CreateGradientRoundedRectWithShadowTexture(
                    DesignTokens.TextureSize.Default,
                    DesignTokens.TextureSize.Default,
                    (int)DesignTokens.Radius.LG,
                    Color.Lerp(theme.Elevated, Color.white, 0.02f),
                    theme.Elevated,
                    DesignTokens.Effects.ShadowLight,
                    DesignTokens.Effects.ShadowBlurSM
                );

                inputBackgroundTexture = CreateInnerShadowTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, (int)DesignTokens.Radius.MD, theme.Base, DesignTokens.Effects.InnerShadowIntensity, DesignTokens.Effects.InnerShadowSize);
                inputFocusedTexture = CreateBorderedRoundedRectTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, (int)DesignTokens.Radius.MD, theme.Base, theme.Accent, DesignTokens.Effects.FocusRingThickness);

                outlineTexture = CreateRoundedOutlineTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, (int)DesignTokens.Radius.MD, theme.Border, 1);
                transparentTexture = CreateSolidTexture(Color.clear);
                glowTexture = CreateGlowTexture(theme.Accent, DesignTokens.TextureSize.Small);
                particleTexture = CreateSolidTexture(Color.Lerp(theme.Accent, theme.Text, 0.5f));

                progressBarBackgroundTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.ProgressBar.TextureHeight, (int)DesignTokens.Radius.SM, theme.Secondary);
                progressBarFillTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.ProgressBar.TextureHeight, (int)DesignTokens.Radius.SM, theme.Accent);

                separatorTexture = CreateSolidTexture(theme.Border);

                tabsBackgroundTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, (int)DesignTokens.Radius.MD, theme.TabsBg);
                tabsActiveTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, (int)DesignTokens.Radius.SM, theme.TabsTriggerActiveBg);

                switchTexture = CreateRoundedRectTexture(DesignTokens.Switch.Width + 2, DesignTokens.Switch.Height + 2, DesignTokens.Switch.Radius, theme.Secondary);
                switchOnTexture = CreateRoundedRectTexture(DesignTokens.Switch.Width + 2, DesignTokens.Switch.Height + 2, DesignTokens.Switch.Radius, theme.Accent);
                switchOffTexture = CreateRoundedRectTexture(DesignTokens.Switch.Width + 2, DesignTokens.Switch.Height + 2, DesignTokens.Switch.Radius, Color.Lerp(theme.Secondary, theme.Muted, 0.2f));

                badgeTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Medium, DesignTokens.Badge.Height, (int)DesignTokens.Radius.XL, theme.Accent);

                avatarTexture = CreateRoundedRectTexture((int)DesignTokens.Height.Default, (int)DesignTokens.Height.Default, (int)DesignTokens.Radius.LG, theme.Muted);

                tableTexture = CreateSolidTexture(theme.Base);
                tableHeaderTexture = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Base, 0.5f));
                tableCellTexture = CreateSolidTexture(theme.Base);

                calendarBackgroundTexture = CreateBorderedRoundedRectTexture(DesignTokens.TextureSize.Large, DesignTokens.TextureSize.Large, (int)DesignTokens.Radius.LG, theme.Elevated, theme.Border, 1);
                calendarHeaderTexture = CreateSolidTexture(theme.Elevated);
                calendarDayTexture = CreateSolidTexture(theme.Elevated);
                calendarDaySelectedTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Small, DesignTokens.TextureSize.Small, (int)DesignTokens.Radius.SM, theme.Accent);

                dropdownMenuContentTexture = CreateGradientRoundedRectWithShadowTexture(
                    DesignTokens.TextureSize.Default,
                    DesignTokens.TextureSize.Default,
                    (int)DesignTokens.Radius.MD,
                    Color.Lerp(theme.Elevated, Color.white, 0.02f),
                    theme.Elevated,
                    DesignTokens.Effects.ShadowMedium,
                    DesignTokens.Effects.ShadowBlurMD
                );
                popoverContentTexture = CreateGradientRoundedRectWithShadowTexture(
                    DesignTokens.TextureSize.Default,
                    DesignTokens.TextureSize.Default,
                    (int)DesignTokens.Radius.LG,
                    Color.Lerp(theme.Elevated, Color.white, 0.02f),
                    theme.Elevated,
                    DesignTokens.Effects.ShadowMedium,
                    DesignTokens.Effects.ShadowBlurMD
                );

                scrollAreaThumbTexture = CreateRoundedRectTexture(8, 8, 4, Color.Lerp(theme.Border, theme.Muted, 0.3f));
                scrollAreaTrackTexture = CreateSolidTexture(Color.clear);

                selectTriggerTexture = CreateBorderedRoundedRectTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, (int)DesignTokens.Radius.MD, theme.Base, theme.Border, 1);
                selectContentTexture = CreateGradientRoundedRectWithShadowTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, (int)DesignTokens.Radius.MD, theme.Elevated, theme.Elevated, DesignTokens.Effects.ShadowMedium, DesignTokens.Effects.ShadowBlurMD);

                chartContainerTexture = CreateBorderedRoundedRectTexture(DesignTokens.TextureSize.Large, DesignTokens.TextureSize.Large, (int)DesignTokens.Radius.LG, theme.Elevated, theme.Border, 1);
                chartGridTexture = CreateSolidTexture(Color.Lerp(theme.Border, Color.clear, 0.5f));
                chartAxisTexture = CreateSolidTexture(theme.Muted);

                sliderTrackTexture = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, 8, (int)DesignTokens.Radius.SM, theme.Secondary);
                GUILogger.LogInfo("Custom textures created successfully", "StyleManager.CreateCustomTextures");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "CreateCustomTextures", "StyleManager");
            }
        }
        #endregion

        #region Design Helpers
        public int GetScaledBorderRadius(float radius) => Mathf.RoundToInt(radius * guiHelper.uiScale);

        public int GetScaledSpacing(float spacing) => Mathf.RoundToInt(spacing * guiHelper.uiScale);

        public int GetScaledHeight(float height) => Mathf.RoundToInt(height * guiHelper.uiScale);

        public int GetScaledFontSize(float scale = 1.0f) => Mathf.RoundToInt(guiHelper.fontSize * scale * guiHelper.uiScale);

        public RectOffset GetSpacingOffset(float horizontal = 8f, float vertical = 8f)
        {
            int h = GetScaledSpacing(horizontal);
            int v = GetScaledSpacing(vertical);
            return new UnityHelpers.RectOffset(h, h, v, v);
        }

        public Color GetHoverColor(Color baseColor, bool isDark = true) => isDark ? Color.Lerp(baseColor, Color.white, DesignTokens.Effects.HoverLighten) : Color.Lerp(baseColor, Color.black, DesignTokens.Effects.HoverDarken);
        #endregion

        #region Helpers
        private GUIStyle CreateStyleWithFont(GUIStyle baseStyle, int fontSize, FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle);
            if (customFont != null)
                style.font = customFont;
            style.fontSize = fontSize;
            style.fontStyle = fontStyle;
            return style;
        }
        #endregion

        #region Cleanup
        public void Cleanup()
        {
            DestroyTexture(gradientTexture);
            DestroyTexture(glowTexture);
            DestroyTexture(particleTexture);
            DestroyTexture(cardBackgroundTexture);
            DestroyTexture(outlineTexture);
            DestroyTexture(transparentTexture);
            DestroyTexture(inputBackgroundTexture);
            DestroyTexture(inputFocusedTexture);
            DestroyTexture(progressBarBackgroundTexture);
            DestroyTexture(progressBarFillTexture);
            DestroyTexture(separatorTexture);
            DestroyTexture(tabsBackgroundTexture);
            DestroyTexture(tabsActiveTexture);
            DestroyTexture(switchTexture);
            DestroyTexture(switchOnTexture);
            DestroyTexture(switchOffTexture);
            DestroyTexture(badgeTexture);
            DestroyTexture(avatarTexture);
            DestroyTexture(tableTexture);
            DestroyTexture(tableHeaderTexture);
            DestroyTexture(tableCellTexture);
            DestroyTexture(calendarBackgroundTexture);
            DestroyTexture(calendarHeaderTexture);
            DestroyTexture(calendarDayTexture);
            DestroyTexture(calendarDaySelectedTexture);
            DestroyTexture(dropdownMenuContentTexture);
            DestroyTexture(popoverContentTexture);
            DestroyTexture(scrollAreaThumbTexture);
            DestroyTexture(scrollAreaTrackTexture);
            DestroyTexture(selectTriggerTexture);
            DestroyTexture(selectContentTexture);
            DestroyTexture(chartContainerTexture);
            DestroyTexture(chartGridTexture);
            DestroyTexture(chartAxisTexture);
            ClearTextureCache(solidColorTextureCache);
            ClearTextureCache(outlineButtonTextureCache);
            ClearTextureCache(outlineTextureCache);
            DestroyTrackedTextures();
        }

        private void DestroyTexture(Texture2D texture)
        {
            if (texture)
                Object.Destroy(texture);
        }

        private void ClearTextureCache<K>(Dictionary<K, Texture2D> cache)
        {
            foreach (var texture in cache.Values)
                if (texture)
                    Object.Destroy(texture);
            cache.Clear();
        }
        #endregion

        #region Fix GUI Styles
        private bool _stylesCorruption = false;
        private float _lastScanTime = 0f;
        private float _lastRefreshTime = 0f;
        private float _scanInterval = 5f;
        private Dictionary<string, int> _styleHealthChecks = new Dictionary<string, int>();
        private List<System.Reflection.FieldInfo> _monitoredStyleFields = new List<System.Reflection.FieldInfo>();

        private int GetStyleHealth(GUIStyle style)
        {
            if (style == null)
                return 0;
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (style.normal.background?.GetInstanceID() ?? 0);
                hash = hash * 31 + (style.hover.background?.GetInstanceID() ?? 0);
                hash = hash * 31 + (style.active.background?.GetInstanceID() ?? 0);
                hash = hash * 31 + style.fontSize;
                hash = hash * 31 + style.padding.left + style.padding.right + style.padding.top + style.padding.bottom;
                hash = hash * 31 + (style.normal.textColor.GetHashCode());
                return hash;
            }
        }

        public void MarkStylesCorruption()
        {
            _stylesCorruption = true;
        }

        public void RefreshStylesIfCorruption()
        {
            if (!_stylesCorruption)
                return;

            if (Time.realtimeSinceStartup - _lastRefreshTime < 1.0f)
            {
                _stylesCorruption = false;
                return;
            }

            _lastRefreshTime = Time.realtimeSinceStartup;

            try
            {
                GUILogger.LogWarning("Corruption detected or flagged. Refreshing styles...", "StyleManager.RefreshStylesIfCorruption");
                isInitialized = false;
                InitializeGUI();

                if (_monitoredStyleFields.Count == 0)
                {
                    var fields = this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    foreach (var field in fields)
                    {
                        if (field.FieldType == typeof(GUIStyle))
                        {
                            _monitoredStyleFields.Add(field);
                        }
                    }
                }

                _styleHealthChecks.Clear();
                foreach (var field in _monitoredStyleFields)
                {
                    var style = field.GetValue(this) as GUIStyle;
                    if (style != null)
                    {
                        _styleHealthChecks[field.Name] = GetStyleHealth(style);
                    }
                }

                _stylesCorruption = false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "RefreshStylesIfCorruption", "StyleManager");
            }
        }

        public bool ScanForCorruption()
        {
            if (Time.realtimeSinceStartup - _lastScanTime < _scanInterval)
                return false;

            _lastScanTime = Time.realtimeSinceStartup;

            if (_monitoredStyleFields.Count == 0)
            {
                var fields = this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(GUIStyle))
                    {
                        _monitoredStyleFields.Add(field);
                    }
                }

                foreach (var field in _monitoredStyleFields)
                {
                    var style = field.GetValue(this) as GUIStyle;
                    if (style != null)
                    {
                        _styleHealthChecks[field.Name] = GetStyleHealth(style);
                    }
                }
                return false;
            }

            foreach (var field in _monitoredStyleFields)
            {
                var style = field.GetValue(this) as GUIStyle;
                if (style == null)
                    continue;

                int currentHealth = GetStyleHealth(style);
                string styleName = field.Name;

                if (_styleHealthChecks.ContainsKey(styleName) && _styleHealthChecks[styleName] != currentHealth)
                {
                    GUILogger.LogWarning($"Style corruption detected in '{styleName}'", "StyleManager.ScanForCorruption");
                    _stylesCorruption = true;
                    RefreshStylesIfCorruption();
                    return true;
                }

                _styleHealthChecks[styleName] = currentHealth;
            }

            return false;
        }
        #endregion
    }
}
