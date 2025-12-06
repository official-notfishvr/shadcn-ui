using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
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
        TableHeader,
        TableCell,
        Calendar,
        CalendarHeader,
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
        SelectTrigger,
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
        #endregion

        #region Base GUIStyles
        private GUIStyle baseButtonStyle;
        private GUIStyle baseToggleStyle;
        private GUIStyle baseInputStyle;
        private GUIStyle baseLabelStyle;
        private GUIStyle baseCheckboxStyle;
        private GUIStyle baseSwitchStyle;
        private GUIStyle baseBadgeStyle;
        private GUIStyle baseTableStyle;
        private GUIStyle baseCalendarStyle;
        private GUIStyle baseDatePickerStyle;
        internal GUIStyle animatedBoxStyle;
        private GUIStyle sectionHeaderStyle;
        private GUIStyle cardStyle;
        private GUIStyle cardHeaderStyle;
        private GUIStyle cardTitleStyle;
        private GUIStyle cardDescriptionStyle;
        private GUIStyle cardContentStyle;
        private GUIStyle cardFooterStyle;
        private GUIStyle passwordFieldStyle;
        private GUIStyle textAreaStyle;
        private GUIStyle progressBarStyle;
        private GUIStyle separatorHorizontalStyle;
        private GUIStyle separatorVerticalStyle;
        private GUIStyle tabsListStyle;
        private GUIStyle tabsTriggerStyle;
        private GUIStyle tabsTriggerActiveStyle;
        private GUIStyle tabsContentStyle;
        private GUIStyle avatarStyle;
        private GUIStyle tableHeaderStyle;
        private GUIStyle tableCellStyle;
        private GUIStyle tableStripedStyle;
        private GUIStyle tableBorderedStyle;
        private GUIStyle tableHoverStyle;
        private GUIStyle calendarHeaderStyle;
        private GUIStyle calendarWeekdayStyle;
        private GUIStyle calendarDayStyle;
        private GUIStyle calendarDaySelectedStyle;
        private GUIStyle calendarDayOutsideMonthStyle;
        private GUIStyle calendarDayTodayStyle;
        private GUIStyle calendarDayInRangeStyle;
        private GUIStyle datePickerHeaderStyle;
        private GUIStyle datePickerWeekdayStyle;
        private GUIStyle datePickerDayStyle;
        private GUIStyle datePickerDaySelectedStyle;
        private GUIStyle datePickerDayOutsideMonthStyle;
        private GUIStyle datePickerDayTodayStyle;
        private GUIStyle dialogContentStyle;
        private GUIStyle dropdownMenuContentStyle;
        private GUIStyle dropdownMenuItemStyle;
        private GUIStyle popoverContentStyle;
        private GUIStyle selectTriggerStyle;
        private GUIStyle selectContentStyle;
        private GUIStyle selectItemStyle;
        private GUIStyle chartContainerStyle;
        private GUIStyle chartAxisStyle;
        private GUIStyle menuBarStyle;
        private GUIStyle menuBarItemStyle;
        private GUIStyle menuDropdownStyle;
        #endregion

        #region Style Caches
        private Dictionary<StyleKey, GUIStyle> styleCache = new();
        #endregion

        #region Texture Caches
        private Dictionary<Color, Texture2D> solidColorTextureCache = new();
        private Dictionary<(Color, Color), Texture2D> outlineButtonTextureCache = new();
        private Dictionary<Color, Texture2D> outlineTextureCache = new();
        private List<Texture2D> trackedTextures = new();
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
                SetupAnimatedStyles();
                SetupCardStyles();
                SetupButtonStyle();
                SetupToggleStyle();
                SetupInputStyle();
                SetupLabelStyle();
                SetupProgressBarStyles();
                SetupSeparatorStyles();
                SetupTabsStyles();
                SetupCheckboxStyle();
                SetupSwitchStyle();
                SetupBadgeStyle();
                SetupAvatarStyles();
                SetupTableStyles();
                SetupCalendarStyle();
                SetupDropdownMenuStyles();
                SetupPopoverStyles();
                SetupSelectStyles();
                SetupDatePickerStyle();
                SetupDialogStyles();
                SetupChartStyles();
                SetupMenuBarStyles();
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

        public Texture2D CreateRoundedRectWithShadowTexture(int width, int height, int radius, Color fillColor, float shadowIntensity = 0.08f, int shadowBlur = 6) => CreateGradientRoundedRectWithShadowTexture(width, height, radius, fillColor, fillColor, shadowIntensity, shadowBlur);

        public Texture2D CreateGradientRoundedRectWithShadowTexture(int width, int height, int radius, Color topColor, Color bottomColor, float shadowIntensity = 0.08f, int shadowBlur = 6)
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

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int dx = Mathf.Min(x, width - 1 - x);
                    int dy = Mathf.Min(y, height - 1 - y);
                    int d = Mathf.Min(dx, dy);
                    if (d < shadowBlur)
                    {
                        Color basePx = texture.GetPixel(x, y);
                        float t = 1f - (d / (float)shadowBlur);
                        float a = t * shadowIntensity;
                        Color shadow = new Color(0f, 0f, 0f, a);
                        float outA = shadow.a + basePx.a * (1f - shadow.a);
                        if (outA <= 0f)
                        {
                            texture.SetPixel(x, y, basePx);
                        }
                        else
                        {
                            float outR = (shadow.r * shadow.a + basePx.r * basePx.a * (1f - shadow.a)) / outA;
                            float outG = (shadow.g * shadow.a + basePx.g * basePx.a * (1f - shadow.a)) / outA;
                            float outB = (shadow.b * shadow.a + basePx.b * basePx.a * (1f - shadow.a)) / outA;
                            texture.SetPixel(x, y, new Color(outR, outG, outB, outA));
                        }
                    }
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

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor, Color borderColor)
        {
            if (outlineButtonTextureCache.TryGetValue((backgroundColor, borderColor), out var cachedTexture))
                return cachedTexture;
            int size = 32;
            Texture2D texture = new Texture2D(size, size);
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                bool isBorder = x == 0 || y == 0 || x == size - 1 || y == size - 1;
                texture.SetPixel(x, y, isBorder ? borderColor : backgroundColor);
            }
            texture.Apply();
            outlineButtonTextureCache[(backgroundColor, borderColor)] = texture;
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

                cardBackgroundTexture = CreateBorderedRoundedRectTexture(128, 128, 12, theme.Elevated, theme.Border, 1);
                inputBackgroundTexture = CreateBorderedRoundedRectTexture(128, 128, 6, theme.Base, theme.Border, 1);
                inputFocusedTexture = CreateBorderedRoundedRectTexture(128, 128, 6, theme.Base, theme.Accent, 1);
                outlineTexture = CreateRoundedOutlineTexture(128, 128, 6, theme.Border, 1);
                transparentTexture = CreateSolidTexture(Color.clear);
                glowTexture = CreateGlowTexture(theme.Accent, 32);
                particleTexture = CreateSolidTexture(Color.Lerp(theme.Accent, theme.Text, 0.5f));
                progressBarBackgroundTexture = CreateRoundedRectTexture(128, 128, 6, theme.Secondary);
                progressBarFillTexture = CreateRoundedRectTexture(128, 128, 6, theme.Accent);
                separatorTexture = CreateSolidTexture(theme.Border);
                tabsBackgroundTexture = CreateRoundedRectTexture(128, 128, 6, theme.TabsBg);
                tabsActiveTexture = CreateRoundedRectTexture(128, 128, 6, theme.Elevated);
                switchTexture = CreateRoundedRectTexture(32, 16, 8, theme.Secondary);
                switchOnTexture = CreateRoundedRectTexture(32, 16, 8, theme.Accent);
                switchOffTexture = CreateRoundedRectTexture(32, 16, 8, Color.Lerp(theme.Secondary, theme.Text, 0.15f));
                badgeTexture = CreateRoundedRectTexture(64, 24, 12, theme.Accent);
                avatarTexture = CreateRoundedRectTexture(40, 40, 20, theme.Muted);
                tableTexture = CreateSolidTexture(theme.Base);
                tableHeaderTexture = CreateSolidTexture(theme.Secondary);
                tableCellTexture = CreateSolidTexture(theme.Base);
                calendarBackgroundTexture = CreateBorderedRoundedRectTexture(256, 256, 8, theme.Elevated, theme.Border, 1);
                calendarHeaderTexture = CreateSolidTexture(theme.Elevated);
                calendarDayTexture = CreateSolidTexture(theme.Elevated);
                calendarDaySelectedTexture = CreateRoundedRectTexture(32, 32, 4, theme.Accent);
                dropdownMenuContentTexture = CreateBorderedRoundedRectTexture(128, 128, 6, theme.Elevated, theme.Border, 1);
                popoverContentTexture = CreateBorderedRoundedRectTexture(128, 128, 6, theme.Elevated, theme.Border, 1);
                scrollAreaThumbTexture = CreateRoundedRectTexture(8, 8, 4, theme.Border);
                scrollAreaTrackTexture = CreateSolidTexture(Color.clear);
                selectTriggerTexture = CreateBorderedRoundedRectTexture(128, 40, 6, theme.Base, theme.Border, 1);
                selectContentTexture = CreateBorderedRoundedRectTexture(128, 128, 6, theme.Elevated, theme.Border, 1);
                chartContainerTexture = CreateBorderedRoundedRectTexture(256, 256, 8, theme.Elevated, theme.Border, 1);
                chartGridTexture = CreateSolidTexture(theme.Border);
                chartAxisTexture = CreateSolidTexture(theme.Muted);

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

        public RectOffset GetBorderOffset(float radius = 6f)
        {
            int r = GetScaledBorderRadius(radius);
            return new UnityHelpers.RectOffset(r, r, r, r);
        }

        public Color GetHoverColor(Color baseColor, bool isDark = true) => isDark ? Color.Lerp(baseColor, Color.white, 0.15f) : Color.Lerp(baseColor, Color.black, 0.08f);

        public Color GetFocusColor(Color baseColor) => Color.Lerp(baseColor, ThemeManager.Instance.CurrentTheme.Accent, 0.25f);

        public Color GetOverlayColor() => ThemeManager.Instance.CurrentTheme.Overlay;

        public Color GetShadowColor() => ThemeManager.Instance.CurrentTheme.Shadow;

        public Color GetBorderColor() => ThemeManager.Instance.CurrentTheme.Border;
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
