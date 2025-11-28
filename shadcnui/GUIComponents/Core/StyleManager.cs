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
        CardFooter
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

    public class StyleManager
    {
        #region Fields
        private GUIHelper guiHelper;
        private bool isInitialized = false;
        public Font customFont;
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
        private Texture2D checkboxTexture;
        private Texture2D checkboxCheckedTexture;
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
        #endregion

        #region Constructor
        public StyleManager(GUIHelper helper)
        {
            try
            {
                guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
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

                CreateCustomTextures();
                SetupAllStyles();
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
                    gradientTexture.SetPixel(0, i, Color.Lerp(theme.Base, theme.Secondary, t));
                }
                gradientTexture.Apply();

                cardBackgroundTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 12, Color.Lerp(theme.Secondary, theme.Base, 0.05f), theme.Secondary, 0.1f, 8);
                inputBackgroundTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                inputFocusedTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                outlineTexture = CreateRoundedRectTexture(128, 128, 6, theme.Border);
                transparentTexture = CreateSolidTexture(Color.clear);
                glowTexture = CreateGlowTexture(theme.Accent, 32);
                particleTexture = CreateSolidTexture(Color.Lerp(theme.Accent, theme.Text, 0.5f));
                progressBarBackgroundTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, Color.black, 0.1f));
                progressBarFillTexture = CreateGradientRoundedRectTexture(128, 128, 6, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                separatorTexture = CreateGradientRoundedRectTexture(128, 2, 0, theme.Border, Color.Lerp(theme.Border, Color.black, 0.1f));
                tabsBackgroundTexture = CreateRoundedRectTexture(128, 128, 6, theme.Secondary);
                tabsActiveTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                checkboxTexture = CreateGradientRoundedRectTexture(16, 16, 4, Color.Lerp(theme.Secondary, theme.Text, 0.1f), Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                checkboxCheckedTexture = CreateGradientRoundedRectTexture(16, 16, 4, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                switchTexture = CreateGradientRoundedRectTexture(32, 16, 8, Color.Lerp(theme.Secondary, theme.Text, 0.1f), Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                switchOnTexture = CreateGradientRoundedRectTexture(32, 16, 8, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                switchOffTexture = CreateGradientRoundedRectTexture(32, 16, 8, Color.Lerp(theme.Secondary, theme.Text, 0.1f), Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                badgeTexture = CreateGradientRoundedRectTexture(64, 24, 12, Color.Lerp(theme.Elevated, Color.white, 0.05f), theme.Elevated);
                avatarTexture = CreateRoundedRectTexture(40, 40, 20, theme.Muted);
                tableTexture = CreateGradientRoundedRectTexture(128, 128, 0, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.02f));
                tableHeaderTexture = CreateSolidTexture(theme.Secondary);
                tableCellTexture = CreateSolidTexture(theme.Base);
                calendarBackgroundTexture = CreateGradientRoundedRectTexture(256, 256, 6, Color.Lerp(theme.Elevated, theme.Base, 0.05f), theme.Elevated);
                calendarHeaderTexture = CreateSolidTexture(theme.Elevated);
                calendarDayTexture = CreateRoundedRectTexture(32, 32, 4, theme.Elevated);
                calendarDaySelectedTexture = CreateGradientRoundedRectTexture(32, 32, 4, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                dropdownMenuContentTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 6, Color.Lerp(theme.Elevated, theme.Base, 0.05f), theme.Elevated, 0.1f, 6);
                popoverContentTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 6, Color.Lerp(theme.Base, theme.Elevated, 0.05f), theme.Base, 0.1f, 6);
                scrollAreaThumbTexture = CreateRoundedRectTexture(8, 8, 4, theme.Border);
                scrollAreaTrackTexture = CreateRoundedRectTexture(8, 8, 4, theme.Secondary);
                selectTriggerTexture = CreateGradientRoundedRectTexture(128, 40, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                selectContentTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 6, Color.Lerp(theme.Elevated, theme.Base, 0.05f), theme.Elevated, 0.1f, 6);
                chartContainerTexture = CreateGradientRoundedRectWithShadowTexture(256, 256, 8, Color.Lerp(theme.Elevated, theme.Base, 0.05f), theme.Elevated, 0.08f, 6);
                chartGridTexture = CreateGradientRoundedRectTexture(128, 2, 0, theme.Border, Color.Lerp(theme.Border, Color.clear, 0.5f));
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

        #region Style Setup - Animated
        private void SetupAnimatedStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                animatedBoxStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                animatedBoxStyle.normal.background = gradientTexture;
                animatedBoxStyle.border = new UnityHelpers.RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, 5);
                animatedBoxStyle.padding = new UnityHelpers.RectOffset(15, 15, 15, 15);
                sectionHeaderStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 2, FontStyle.Bold);
                sectionHeaderStyle.normal.textColor = Color.Lerp(theme.Accent, theme.Text, 0.4f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAnimatedStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Card
        private void SetupCardStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                cardStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                int cardRadius = GetScaledBorderRadius(12f);
                cardStyle.normal.background = CreateRoundedRectWithShadowTexture(256, 256, cardRadius, theme.Secondary, 0.2f, 12);
                cardStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                cardStyle.padding = GetSpacingOffset(28f, 28f);
                cardStyle.margin = GetSpacingOffset(8f, 8f);
                cardHeaderStyle = new UnityHelpers.GUIStyle();
                cardHeaderStyle.padding = GetSpacingOffset(0f, 20f);
                cardTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.75f), FontStyle.Bold);
                cardTitleStyle.normal.textColor = theme.Text;
                cardTitleStyle.wordWrap = true;
                cardDescriptionStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.95f));
                cardDescriptionStyle.normal.textColor = theme.Muted;
                cardDescriptionStyle.wordWrap = true;
                cardDescriptionStyle.margin = GetSpacingOffset(0f, 6f);
                cardContentStyle = new UnityHelpers.GUIStyle();
                cardContentStyle.padding = GetSpacingOffset(0f, 12f);
                cardFooterStyle = new UnityHelpers.GUIStyle();
                cardFooterStyle.padding = GetSpacingOffset(0f, 20f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCardStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Button Base
        private void SetupButtonStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseButtonStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f), FontStyle.Bold);
                baseButtonStyle.alignment = TextAnchor.MiddleCenter;
                baseButtonStyle.padding = GetSpacingOffset(16f, 8f);
                baseButtonStyle.border = GetBorderOffset(6f);
                baseButtonStyle.fixedHeight = GetScaledHeight(36f);
                int btnTexH = GetScaledHeight(36f);
                int btnRadius = GetScaledBorderRadius(6f);
                baseButtonStyle.normal.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, theme.ButtonPrimaryBg, Color.Lerp(theme.ButtonPrimaryBg, Color.black, 0.05f));
                baseButtonStyle.normal.textColor = theme.ButtonPrimaryFg;
                baseButtonStyle.hover.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, GetHoverColor(theme.ButtonPrimaryBg, true), Color.Lerp(GetHoverColor(theme.ButtonPrimaryBg, true), Color.black, 0.05f));
                baseButtonStyle.hover.textColor = theme.ButtonPrimaryFg;
                baseButtonStyle.active.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, Color.Lerp(theme.ButtonPrimaryBg, Color.black, 0.15f), Color.Lerp(theme.ButtonPrimaryBg, Color.black, 0.1f));
                baseButtonStyle.active.textColor = theme.ButtonPrimaryFg;
                baseButtonStyle.focused.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, Color.Lerp(theme.ButtonPrimaryBg, theme.Accent, 0.1f), Color.Lerp(theme.ButtonPrimaryBg, theme.Accent, 0.15f));
                baseButtonStyle.focused.textColor = theme.ButtonPrimaryFg;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupButtonStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Toggle Base
        private void SetupToggleStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseToggleStyle = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize);
                baseToggleStyle.alignment = TextAnchor.MiddleCenter;
                baseToggleStyle.normal.textColor = theme.Text;
                int radius = GetScaledBorderRadius(4f);
                baseToggleStyle.normal.background = CreateGradientRoundedRectTexture(32, 32, radius, theme.ButtonPrimaryBg, Color.Lerp(theme.ButtonPrimaryBg, Color.black, 0.05f));
                baseToggleStyle.hover.background = CreateGradientRoundedRectTexture(32, 32, radius, theme.ButtonPrimaryBg, Color.Lerp(theme.ButtonPrimaryBg, Color.white, 0.05f));
                baseToggleStyle.hover.textColor = theme.Text;
                baseToggleStyle.active.background = CreateGradientRoundedRectTexture(32, 32, radius, Color.Lerp(theme.ButtonPrimaryBg, Color.black, 0.15f), Color.Lerp(theme.ButtonPrimaryBg, Color.black, 0.2f));
                baseToggleStyle.active.textColor = Color.Lerp(theme.Text, Color.white, 0.5f);
                baseToggleStyle.onNormal.background = CreateGradientRoundedRectTexture(32, 32, radius, theme.Accent, Color.Lerp(theme.Accent, Color.black, 0.05f));
                baseToggleStyle.onNormal.textColor = theme.Text;
                baseToggleStyle.onHover.background = CreateGradientRoundedRectTexture(32, 32, radius, Color.Lerp(theme.Accent, Color.white, 0.12f), Color.Lerp(theme.Accent, Color.white, 0.05f));
                baseToggleStyle.onHover.textColor = theme.Text;
                baseToggleStyle.onActive.background = CreateGradientRoundedRectTexture(32, 32, radius, Color.Lerp(theme.Accent, Color.black, 0.15f), Color.Lerp(theme.Accent, Color.black, 0.2f));
                baseToggleStyle.onActive.textColor = theme.Text;
                baseToggleStyle.border = GetBorderOffset(4f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupToggleStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Input Base
        private void SetupInputStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseInputStyle = CreateStyleWithFont(GUI.skin.textField, GetScaledFontSize(0.875f));
                baseInputStyle.padding = GetSpacingOffset(12f, 8f);
                baseInputStyle.margin = GetSpacingOffset(0f, 4f);
                baseInputStyle.border = GetBorderOffset(6f);
                baseInputStyle.fixedHeight = GetScaledHeight(36f);
                baseInputStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                baseInputStyle.normal.textColor = theme.Text;
                baseInputStyle.hover.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.Secondary, theme.Text, 0.1f));
                baseInputStyle.hover.textColor = theme.Text;
                baseInputStyle.focused.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                baseInputStyle.focused.textColor = theme.Accent;
                int inpRadius = GetScaledBorderRadius(6f);
                Color focusTint = Color.Lerp(theme.Accent, theme.Overlay, 0.1f);
                baseInputStyle.onFocused.background = CreateGradientRoundedRectTexture(128, GetScaledHeight(36f), inpRadius, focusTint, Color.Lerp(focusTint, Color.black, 0.05f));
                passwordFieldStyle = CreateStyleWithFont(baseInputStyle, guiHelper.fontSize + 2);
                textAreaStyle = CreateStyleWithFont(baseInputStyle, guiHelper.fontSize);
                textAreaStyle.wordWrap = true;
                textAreaStyle.stretchHeight = true;
                textAreaStyle.padding = new UnityHelpers.RectOffset(12, 12, 8, 8);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupInputStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Label Base
        private void SetupLabelStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseLabelStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f), FontStyle.Normal);
                baseLabelStyle.normal.textColor = theme.Text;
                baseLabelStyle.padding = GetSpacingOffset(0f, 3f);
                baseLabelStyle.wordWrap = true;
                baseLabelStyle.alignment = TextAnchor.UpperLeft;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupLabelStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Progress
        private void SetupProgressBarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                progressBarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                int progRadius = GetScaledBorderRadius(8f);
                progressBarStyle.normal.background = CreateRoundedRectTexture(256, 12, progRadius, theme.Base);
                progressBarStyle.border = GetBorderOffset(8f);
                progressBarStyle.padding = GetSpacingOffset(0f, 0f);
                progressBarStyle.margin = GetSpacingOffset(0f, 4f);
                progressBarStyle.fixedHeight = GetScaledHeight(12f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupProgressBarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Separator
        private void SetupSeparatorStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                separatorHorizontalStyle = new UnityHelpers.GUIStyle();
                separatorHorizontalStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Border, Color.black, 0.1f));
                separatorHorizontalStyle.fixedHeight = Mathf.RoundToInt(1.5f * guiHelper.uiScale);
                separatorHorizontalStyle.stretchWidth = true;
                separatorHorizontalStyle.margin = GetSpacingOffset(0f, 6f);
                separatorVerticalStyle = new UnityHelpers.GUIStyle();
                separatorVerticalStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Border, Color.black, 0.1f));
                separatorVerticalStyle.fixedWidth = Mathf.RoundToInt(1.5f * guiHelper.uiScale);
                separatorVerticalStyle.stretchHeight = true;
                separatorVerticalStyle.margin = GetSpacingOffset(6f, 0f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSeparatorStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Tabs
        private void SetupTabsStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                tabsListStyle = new UnityHelpers.GUIStyle();
                tabsListStyle.normal.background = CreateGradientRoundedRectTexture(128, 128, 6, theme.TabsBg, Color.Lerp(theme.TabsBg, Color.black, 0.05f));
                tabsListStyle.border = GetBorderOffset(0f);
                tabsListStyle.padding = GetSpacingOffset(6f, 6f);
                tabsListStyle.margin = GetSpacingOffset(0f, 0f);
                tabsTriggerStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.9f), FontStyle.Bold);
                tabsTriggerStyle.alignment = TextAnchor.MiddleCenter;
                tabsTriggerStyle.padding = GetSpacingOffset(14f, 6f);
                tabsTriggerStyle.border = GetBorderOffset(0f);
                tabsTriggerStyle.normal.background = transparentTexture;
                tabsTriggerStyle.normal.textColor = Color.Lerp(theme.Muted, Color.black, 0.2f);
                tabsTriggerStyle.hover.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.08f));
                tabsTriggerStyle.active.background = transparentTexture;
                tabsTriggerActiveStyle = new UnityHelpers.GUIStyle(tabsTriggerStyle);
                if (customFont != null)
                    tabsTriggerActiveStyle.font = customFont;
                tabsTriggerActiveStyle.normal.background = CreateBottomBorderTexture(128, GetScaledHeight(40f), Mathf.Max(3, Mathf.RoundToInt(3f * guiHelper.uiScale)), theme.Accent, theme.Base);
                tabsTriggerActiveStyle.normal.textColor = theme.TabsTriggerActiveFg;
                tabsTriggerActiveStyle.hover.background = tabsTriggerActiveStyle.normal.background;
                tabsTriggerActiveStyle.active.background = tabsTriggerActiveStyle.normal.background;
                tabsContentStyle = new UnityHelpers.GUIStyle();
                tabsContentStyle.padding = GetSpacingOffset(18f, 18f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTabsStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Checkbox Base
        private void SetupCheckboxStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseCheckboxStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                if (customFont != null)
                    baseCheckboxStyle.font = customFont;
                baseCheckboxStyle.fontSize = GetScaledFontSize(0.9f);
                int cbRadius = GetScaledBorderRadius(6f);
                baseCheckboxStyle.normal.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Secondary, theme.Text, 0.1f));
                baseCheckboxStyle.normal.textColor = theme.Text;
                baseCheckboxStyle.hover.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Secondary, theme.Text, 0.2f));
                baseCheckboxStyle.hover.textColor = theme.Text;
                baseCheckboxStyle.active.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                baseCheckboxStyle.active.textColor = theme.Text;
                baseCheckboxStyle.onNormal.background = CreateRoundedRectTexture(20, 20, cbRadius, theme.Accent);
                baseCheckboxStyle.onNormal.textColor = Color.white;
                baseCheckboxStyle.onHover.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Accent, Color.white, 0.12f));
                baseCheckboxStyle.onHover.textColor = Color.white;
                baseCheckboxStyle.onActive.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Accent, Color.black, 0.1f));
                baseCheckboxStyle.onActive.textColor = Color.white;
                baseCheckboxStyle.border = GetBorderOffset(6f);
                baseCheckboxStyle.padding = GetSpacingOffset(8f, 0f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCheckboxStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Switch Base
        private void SetupSwitchStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseSwitchStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                if (customFont != null)
                    baseSwitchStyle.font = customFont;
                baseSwitchStyle.fontSize = GetScaledFontSize(0.875f);
                baseSwitchStyle.normal.background = switchOffTexture;
                baseSwitchStyle.normal.textColor = theme.Text;
                baseSwitchStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.2f));
                baseSwitchStyle.hover.textColor = theme.Text;
                baseSwitchStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                baseSwitchStyle.active.textColor = theme.Text;
                baseSwitchStyle.onNormal.background = switchOnTexture;
                baseSwitchStyle.onNormal.textColor = theme.Text;
                baseSwitchStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                baseSwitchStyle.onHover.textColor = theme.Text;
                baseSwitchStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                baseSwitchStyle.onActive.textColor = theme.Text;
                baseSwitchStyle.border = GetBorderOffset(6f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSwitchStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Badge Base
        private void SetupBadgeStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseBadgeStyle = CreateStyleWithFont(GUI.skin.box, GetScaledFontSize(0.8f), FontStyle.Bold);
                baseBadgeStyle.normal.background = CreateRoundedRectTexture(128, 24, 12, theme.Elevated);
                baseBadgeStyle.normal.textColor = theme.Text;
                baseBadgeStyle.border = GetBorderOffset(12f);
                baseBadgeStyle.padding = GetSpacingOffset(10f, 4f);
                baseBadgeStyle.alignment = TextAnchor.MiddleCenter;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupBadgeStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Avatar
        private void SetupAvatarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                int defaultAvatarSize = GetScaledHeight(40f);
                avatarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                avatarStyle.normal.background = CreateSolidTexture(theme.Muted);
                avatarStyle.alignment = TextAnchor.MiddleCenter;
                avatarStyle.fixedWidth = defaultAvatarSize;
                avatarStyle.fixedHeight = defaultAvatarSize;
                avatarStyle.border = GetAvatarBorder(AvatarShape.Circle, ControlSize.Default);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAvatarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Table Base
        private void SetupTableStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseTableStyle = new UnityHelpers.GUIStyle();
                baseTableStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                baseTableStyle.border = GetBorderOffset(1f);
                baseTableStyle.padding = GetSpacingOffset(0f, 0f);
                baseTableStyle.margin = GetSpacingOffset(0f, 0f);
                tableHeaderStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.9f), FontStyle.Bold);
                tableHeaderStyle.normal.background = CreateGradientRoundedRectTexture(128, 40, 0, Color.Lerp(theme.Secondary, theme.Accent, 0.15f), Color.Lerp(theme.Secondary, theme.Accent, 0.1f));
                tableHeaderStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.2f));
                tableHeaderStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.25f));
                tableHeaderStyle.normal.textColor = theme.Text;
                tableHeaderStyle.hover.textColor = theme.Text;
                tableHeaderStyle.active.textColor = theme.Text;
                tableHeaderStyle.padding = GetSpacingOffset(14f, 14f);
                tableHeaderStyle.alignment = TextAnchor.MiddleLeft;
                tableHeaderStyle.border = GetBorderOffset(0f);
                tableCellStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.95f));
                tableCellStyle.normal.background = CreateSolidTexture(theme.Secondary);
                tableCellStyle.normal.textColor = theme.Text;
                tableCellStyle.padding = GetSpacingOffset(14f, 14f);
                tableCellStyle.alignment = TextAnchor.MiddleLeft;
                tableCellStyle.wordWrap = false;
                tableCellStyle.clipping = TextClipping.Clip;
                tableStripedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableStripedStyle.font = customFont;
                tableStripedStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.08f));
                tableBorderedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableBorderedStyle.font = customFont;
                tableBorderedStyle.normal.background = CreateBorderTexture(theme.Border, 1);
                tableHoverStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableHoverStyle.font = customFont;
                tableHoverStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                tableHoverStyle.normal.textColor = theme.Text;
                tableHoverStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.12f));
                tableHoverStyle.hover.textColor = theme.Text;
                tableHoverStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.12f));
                tableHoverStyle.active.textColor = theme.Text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTableStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Calendar Base
        private void SetupCalendarStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseCalendarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                baseCalendarStyle.normal.background = CreateSolidTexture(theme.Elevated);
                baseCalendarStyle.border = GetBorderOffset(4f);
                baseCalendarStyle.padding = GetSpacingOffset(12f, 12f);
                calendarHeaderStyle = new UnityHelpers.GUIStyle();
                calendarHeaderStyle.padding = GetSpacingOffset(0f, 4f);

                calendarWeekdayStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.875f));
                calendarWeekdayStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.35f);
                calendarWeekdayStyle.alignment = TextAnchor.MiddleCenter;
                calendarDayStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(1.0f));
                calendarDayStyle.normal.textColor = theme.Text;
                calendarDayStyle.normal.background = CreateSolidTexture(theme.Elevated);
                calendarDayStyle.hover.background = CreateGradientRoundedRectTexture(32, 32, 4, Color.Lerp(theme.Secondary, theme.Accent, 0.12f), Color.Lerp(theme.Secondary, theme.Accent, 0.08f));
                calendarDaySelectedStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDaySelectedStyle.font = customFont;
                calendarDaySelectedStyle.normal.background = CreateSolidTexture(theme.Accent);
                calendarDaySelectedStyle.normal.textColor = theme.Text;
                calendarDayOutsideMonthStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayOutsideMonthStyle.font = customFont;
                calendarDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.5f);
                calendarDayTodayStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayTodayStyle.font = customFont;
                calendarDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.Secondary, theme.Accent);
                calendarDayInRangeStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayInRangeStyle.font = customFont;
                calendarDayInRangeStyle.normal.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.55f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCalendarStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - DropdownMenu
        private void SetupDropdownMenuStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dropdownMenuContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                int dropdownRadius = GetScaledBorderRadius(8f);
                dropdownMenuContentStyle.normal.background = CreateRoundedRectWithShadowTexture(256, 256, dropdownRadius, theme.Elevated, 0.18f, 10);
                dropdownMenuContentStyle.border = GetBorderOffset(8f);
                dropdownMenuContentStyle.padding = GetSpacingOffset(6f, 6f);
                dropdownMenuItemStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.9f));
                dropdownMenuItemStyle.alignment = TextAnchor.MiddleLeft;
                dropdownMenuItemStyle.normal.background = transparentTexture;
                dropdownMenuItemStyle.normal.textColor = theme.Text;
                dropdownMenuItemStyle.hover.background = CreateGradientRoundedRectTexture(128, 32, 6, Color.Lerp(theme.Accent, Color.white, 0.15f), Color.Lerp(theme.Accent, Color.white, 0.1f));
                dropdownMenuItemStyle.hover.textColor = theme.Accent;
                dropdownMenuItemStyle.active.background = CreateGradientRoundedRectTexture(128, 32, 6, Color.Lerp(theme.Accent, Color.black, 0.1f), Color.Lerp(theme.Accent, Color.black, 0.15f));
                dropdownMenuItemStyle.active.textColor = theme.Accent;
                dropdownMenuItemStyle.padding = GetSpacingOffset(14f, 6f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDropdownMenuStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Popover
        private void SetupPopoverStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                popoverContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                popoverContentStyle.normal.background = CreateSolidTexture(theme.Elevated);
                popoverContentStyle.border = GetBorderOffset(6f);
                popoverContentStyle.padding = GetSpacingOffset(12f, 12f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupPopoverStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Select
        private void SetupSelectStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
                int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);
                selectTriggerStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(scaledFontSize));
                selectTriggerStyle.alignment = TextAnchor.MiddleLeft;
                selectTriggerStyle.normal.background = selectTriggerTexture;
                selectTriggerStyle.normal.textColor = theme.Text;
                selectTriggerStyle.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Base, theme.Accent, 0.12f), Color.Lerp(theme.Base, theme.Accent, 0.08f));
                selectTriggerStyle.active.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Base, theme.Accent, 0.25f), Color.Lerp(theme.Base, theme.Accent, 0.2f));
                selectTriggerStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
                selectTriggerStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                selectContentStyle.normal.background = CreateSolidTexture(theme.Elevated);
                selectContentStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle.padding = new UnityHelpers.RectOffset(5, 5, 5, 5);
                selectItemStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(scaledFontSize));
                selectItemStyle.alignment = TextAnchor.MiddleLeft;
                selectItemStyle.normal.background = transparentTexture;
                selectItemStyle.normal.textColor = theme.Text;
                selectItemStyle.hover.background = CreateSolidTexture(GetHoverColor(theme.Base));
                selectItemStyle.hover.textColor = theme.Text;
                selectItemStyle.active.background = CreateSolidTexture(GetHoverColor(theme.Base, true));
                selectItemStyle.active.textColor = theme.Text;
                selectItemStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSelectStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - DatePicker Base
        private void SetupDatePickerStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseDatePickerStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                baseDatePickerStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                baseDatePickerStyle.border = new UnityHelpers.RectOffset(4, 4, 4, 4);
                baseDatePickerStyle.padding = new UnityHelpers.RectOffset(10, 10, 10, 10);
                datePickerHeaderStyle = new UnityHelpers.GUIStyle();
                datePickerHeaderStyle.padding = new UnityHelpers.RectOffset(0, 0, 5, 5);

                datePickerWeekdayStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize - 1);
                datePickerWeekdayStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.35f);
                datePickerWeekdayStyle.alignment = TextAnchor.MiddleCenter;
                datePickerDayStyle = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize);
                datePickerDayStyle.normal.textColor = theme.Text;
                datePickerDayStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                datePickerDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.12f));
                datePickerDaySelectedStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDaySelectedStyle.font = customFont;
                datePickerDaySelectedStyle.normal.background = CreateSolidTexture(theme.Accent);
                datePickerDaySelectedStyle.normal.textColor = theme.Text;
                datePickerDayOutsideMonthStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayOutsideMonthStyle.font = customFont;
                datePickerDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.5f);
                datePickerDayTodayStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayTodayStyle.font = customFont;
                datePickerDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.Secondary, theme.Accent);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDatePickerStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Dialog
        private void SetupDialogStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dialogContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                dialogContentStyle.normal.background = CreateRoundedRectWithShadowTexture(512, 512, 12, theme.Elevated, 0.18f, 12);
                dialogContentStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                dialogContentStyle.padding = new UnityHelpers.RectOffset(24, 24, 24, 24);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDialogStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Chart
        private void SetupChartStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                chartContainerStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                chartContainerStyle.normal.background = chartContainerTexture;
                chartContainerStyle.border = GetBorderOffset(12f);
                chartContainerStyle.padding = GetSpacingOffset(16f, 16f);
                chartContainerStyle.margin = GetSpacingOffset(0f, 0f);

                chartAxisStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.75f));
                chartAxisStyle.normal.textColor = theme.Muted;
                chartAxisStyle.alignment = TextAnchor.MiddleCenter;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupChartStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - MenuBar
        private void SetupMenuBarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                menuBarStyle = new UnityHelpers.GUIStyle();
                menuBarStyle.padding = GetSpacingOffset(4f, 0f);
                menuBarStyle.margin = GetSpacingOffset(0f, 0f);
                menuBarStyle.fixedHeight = GetScaledHeight(40f);
                menuBarStyle.stretchWidth = false;
                menuBarItemStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f));
                menuBarItemStyle.alignment = TextAnchor.MiddleLeft;
                menuBarItemStyle.padding = GetSpacingOffset(12f, 8f);
                menuBarItemStyle.margin = GetSpacingOffset(0f, 0f);
                menuBarItemStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                menuBarItemStyle.normal.background = transparentTexture;
                menuBarItemStyle.normal.textColor = theme.Text;
                menuBarItemStyle.hover.background = CreateSolidTexture(GetHoverColor(theme.Base));
                menuBarItemStyle.hover.textColor = theme.Text;
                menuBarItemStyle.active.background = CreateSolidTexture(GetHoverColor(theme.Base, true));
                menuBarItemStyle.active.textColor = theme.Text;
                menuBarItemStyle.stretchWidth = false;
                menuDropdownStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                Color menuBg = Color.Lerp(theme.Secondary, theme.Text, 0.05f);
                menuDropdownStyle.normal.background = CreateGradientRoundedRectWithShadowTexture(200, 200, 4, menuBg, menuBg, 0.1f, 6);
                menuDropdownStyle.border = GetBorderOffset(4f);
                menuDropdownStyle.padding = GetSpacingOffset(4f, 4f);
                menuDropdownStyle.margin = GetSpacingOffset(0f, 0f);
                menuDropdownStyle.fixedWidth = 200;
                menuDropdownStyle.stretchWidth = false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupMenuBarStyles", "StyleManager");
            }
        }
        #endregion

        #region Variant Application

        private void ApplyContainerVariant(GUIStyle style, ControlVariant variant, Theme theme)
        {
            switch (variant)
            {
                case ControlVariant.Secondary:
                    style.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                    break;
                case ControlVariant.Destructive:
                    style.normal.background = CreateSolidTexture(theme.Destructive);
                    style.normal.textColor = theme.ButtonDestructiveFg;
                    break;
                case ControlVariant.Outline:
                    style.normal.background = transparentTexture;
                    style.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = transparentTexture;
                    break;
                case ControlVariant.Muted:
                    style.normal.background = CreateSolidTexture(theme.Muted);
                    break;
            }
        }

        private void ApplyVariantToStyle(GUIStyle style, ControlVariant variant, Theme theme)
        {
            switch (variant)
            {
                case ControlVariant.Secondary:
                    style.normal.textColor = theme.ButtonSecondaryFg;
                    Color secBg = Color.Lerp(theme.Secondary, theme.Text, 0.1f);
                    style.normal.background = CreateGradientRoundedRectTexture(128, 40, 6, secBg, Color.Lerp(secBg, Color.black, 0.05f));
                    style.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(secBg, Color.white, 0.05f), Color.Lerp(secBg, Color.black, 0.05f));
                    style.onNormal.textColor = theme.ButtonSecondaryFg;
                    style.onNormal.background = CreateGradientRoundedRectTexture(128, 40, 6, theme.Accent, Color.Lerp(theme.Accent, Color.black, 0.05f));
                    style.onHover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Accent, Color.white, 0.1f), Color.Lerp(theme.Accent, Color.white, 0.05f));
                    break;
                case ControlVariant.Destructive:
                    style.normal.textColor = theme.ButtonDestructiveFg;
                    style.normal.background = CreateGradientRoundedRectTexture(128, 40, 6, theme.ButtonDestructiveBg, Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.1f));
                    style.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.ButtonDestructiveBg, Color.white, 0.08f), Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.05f));
                    style.onNormal.textColor = theme.ButtonDestructiveFg;
                    style.onNormal.background = CreateGradientRoundedRectTexture(128, 40, 6, theme.Destructive, Color.Lerp(theme.Destructive, Color.black, 0.2f));
                    break;
                case ControlVariant.Outline:
                    style.normal.textColor = theme.ButtonOutlineFg;
                    style.normal.background = CreateRoundedOutlineTexture(128, 40, 6, theme.Border);
                    style.hover.background = CreateRoundedOutlineTexture(128, 40, 6, theme.Accent);
                    style.border = new UnityHelpers.RectOffset(6, 6, 6, 6);
                    style.onNormal.textColor = theme.Accent;
                    style.onNormal.background = CreateRoundedOutlineTexture(128, 40, 6, theme.Accent);
                    style.onHover.background = CreateRoundedOutlineTexture(128, 40, 6, theme.Accent);
                    break;
                case ControlVariant.Ghost:
                    style.normal.textColor = theme.ButtonGhostFg;
                    style.normal.background = transparentTexture;
                    Color ghostHover = new Color(theme.Secondary.r, theme.Secondary.g, theme.Secondary.b, 0.5f);
                    style.hover.background = CreateSolidTexture(ghostHover);
                    style.active.background = CreateSolidTexture(Color.Lerp(ghostHover, Color.black, 0.2f));
                    style.onNormal.textColor = theme.Accent;
                    style.onNormal.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.5f));
                    style.onHover.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.65f));
                    break;
                case ControlVariant.Link:
                    style.normal.textColor = theme.ButtonLinkColor;
                    style.normal.background = transparentTexture;
                    style.hover.background = transparentTexture;
                    style.hover.textColor = Color.Lerp(theme.ButtonLinkColor, Color.white, 0.2f);
                    style.active.background = transparentTexture;
                    style.active.textColor = Color.Lerp(theme.ButtonLinkColor, Color.black, 0.25f);
                    style.padding = GetSpacingOffset(0f, 2f);
                    style.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.onNormal.textColor = theme.Accent;
                    style.onNormal.background = transparentTexture;
                    break;
                case ControlVariant.Muted:
                    style.normal.textColor = theme.Muted;
                    style.normal.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Secondary, Color.black, 0.08f), Color.Lerp(theme.Secondary, Color.black, 0.12f));
                    style.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Secondary, Color.black, 0.1f), Color.Lerp(theme.Secondary, Color.black, 0.15f));
                    style.onNormal.textColor = theme.Muted;
                    style.onNormal.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Secondary, theme.Accent, 0.1f), Color.Lerp(theme.Secondary, theme.Accent, 0.15f));
                    break;
            }
        }

        private void ApplyVariantToInputStyle(GUIStyle style, ControlVariant variant)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            switch (variant)
            {
                case ControlVariant.Outline:
                    style.normal.background = CreateRoundedRectTexture(128, 40, 6, theme.Base);
                    style.focused.background = CreateRoundedRectTexture(128, 40, 6, theme.Base);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = transparentTexture;
                    style.focused.background = transparentTexture;
                    break;
                default:
                    style.normal.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                    style.focused.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                    break;
            }
        }

        #endregion

        #region Style Sizing

        private void ApplySizing(GUIStyle style, ControlSize size, string styleType, bool isHeader = false, SeparatorOrientation orientation = SeparatorOrientation.Horizontal)
        {
            switch (styleType)
            {
                case "Button":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(12f, 2f);
                            style.fixedHeight = GetScaledHeight(32f);
                            style.border = GetBorderOffset(6f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(24f, 12f);
                            style.fixedHeight = GetScaledHeight(44f);
                            style.border = GetBorderOffset(6f);
                            break;
                        case ControlSize.Icon:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(0f, 0f);
                            int iconSize = GetScaledHeight(36f);
                            style.fixedWidth = iconSize;
                            style.fixedHeight = iconSize;
                            style.border = GetBorderOffset(6f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(8f, 2f);
                            style.fixedHeight = GetScaledHeight(24f);
                            style.border = GetBorderOffset(4f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(16f, 8f);
                            style.fixedHeight = GetScaledHeight(36f);
                            style.border = GetBorderOffset(6f);
                            break;
                    }
                    break;

                case "Toggle":
                case "Checkbox":
                case "Switch":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(6f, styleType == "Checkbox" || styleType == "Switch" ? 0f : 2f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(styleType == "Toggle" ? 1.25f : 1.25f);
                            style.padding = GetSpacingOffset(10f, styleType == "Checkbox" || styleType == "Switch" ? 0f : 6f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(styleType == "Toggle" ? 0.65f : 0.65f);
                            style.padding = GetSpacingOffset(4f, styleType == "Checkbox" || styleType == "Switch" ? 0f : 1f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(styleType == "Checkbox" ? 0.9f : 1.0f);
                            style.padding = GetSpacingOffset(8f, styleType == "Checkbox" || styleType == "Switch" ? 0f : 4f);
                            break;
                    }
                    break;

                case "Input":
                case "PasswordField":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(8f, 4f);
                            style.fixedHeight = GetScaledHeight(28f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(16f, 10f);
                            style.fixedHeight = GetScaledHeight(44f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(6f, 2f);
                            style.fixedHeight = GetScaledHeight(24f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(12f, 8f);
                            style.fixedHeight = GetScaledHeight(36f);
                            break;
                    }
                    break;

                case "Label":
                case "ChartAxis":
                case "SectionHeader":
                case "CardTitle":
                case "CardDescription":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.25f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.65f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            break;
                    }
                    break;

                case "TextArea":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(6f, 4f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(12f, 8f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(4f, 2f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(8f, 6f);
                            break;
                    }
                    break;

                case "ProgressBar":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fixedHeight = GetScaledHeight(4f);
                            break;
                        case ControlSize.Large:
                            style.fixedHeight = GetScaledHeight(12f);
                            break;
                        case ControlSize.Mini:
                            style.fixedHeight = GetScaledHeight(2f);
                            break;
                        case ControlSize.Default:
                            style.fixedHeight = GetScaledHeight(8f);
                            break;
                    }
                    break;

                case "Separator":
                    float thickness = size switch
                    {
                        ControlSize.Small => 1f,
                        ControlSize.Large => 4f,
                        ControlSize.Mini => 1f,
                        ControlSize.Default => 2f,
                        _ => 1f,
                    };
                    if (orientation == SeparatorOrientation.Horizontal)
                        style.fixedHeight = GetScaledHeight(thickness);
                    else
                        style.fixedWidth = GetScaledHeight(thickness);
                    break;

                case "TabsList":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(2f, 2f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(6f, 6f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(1f, 1f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(4f, 4f);
                            break;
                    }
                    break;

                case "TabsTrigger":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(8f, 4f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(16f, 8f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(6f, 2f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(12f, 6f);
                            break;
                    }
                    break;

                case "TabsContent":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(8f, 8f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(24f, 24f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(4f, 4f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(16f, 16f);
                            break;
                    }
                    break;

                case "Badge":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.65f);
                            style.padding = GetSpacingOffset(8f, 2f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(12f, 6f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.6f);
                            style.padding = GetSpacingOffset(6f, 1f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.8f);
                            style.padding = GetSpacingOffset(10f, 4f);
                            break;
                    }
                    break;

                case "CalendarChild":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.8f);
                            style.fixedWidth = GetScaledHeight(28f);
                            style.fixedHeight = GetScaledHeight(28f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.fixedWidth = GetScaledHeight(40f);
                            style.fixedHeight = GetScaledHeight(40f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.fixedWidth = GetScaledHeight(20f);
                            style.fixedHeight = GetScaledHeight(20f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.9f);
                            style.fixedWidth = GetScaledHeight(32f);
                            style.fixedHeight = GetScaledHeight(32f);
                            break;
                    }
                    break;

                case "Table":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.9f);
                            style.padding = GetSpacingOffset(8f, 8f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.1f);
                            style.padding = GetSpacingOffset(20f, 20f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(6f, 4f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.95f);
                            style.padding = GetSpacingOffset(14f, 14f);
                            break;
                    }
                    break;

                case "TableChild":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(isHeader ? 0.85f : 0.8f);
                            style.padding = GetSpacingOffset(8f, 6f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(isHeader ? 1.1f : 1.0f);
                            style.padding = GetSpacingOffset(16f, 12f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(isHeader ? 0.75f : 0.7f);
                            style.padding = GetSpacingOffset(4f, 2f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(isHeader ? 0.95f : 0.9f);
                            style.padding = GetSpacingOffset(12f, 8f);
                            break;
                    }
                    break;

                case "Dialog":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(16f, 16f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(32f, 32f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(8f, 8f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(24f, 24f);
                            break;
                    }
                    break;

                case "Calendar":
                case "DatePicker":
                case "Popover":
                case "DropdownMenu":
                case "AnimatedBox":
                case "Chart":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(8f, 8f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(24f, 24f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(4f, 4f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(16f, 16f);
                            break;
                    }
                    break;

                case "Card":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(12f, 12f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(32f, 32f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(8f, 8f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(24f, 24f);
                            break;
                    }
                    break;

                case "CardChild":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(4f, 4f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(8f, 8f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(2f, 2f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(6f, 6f);
                            break;
                    }
                    break;

                case "DropdownItem":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(6f, 4f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(12f, 8f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(4f, 2f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(8f, 6f);
                            break;
                    }
                    break;

                case "MenuBar":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.padding = GetSpacingOffset(2f, 2f);
                            break;
                        case ControlSize.Large:
                            style.padding = GetSpacingOffset(6f, 6f);
                            break;
                        case ControlSize.Mini:
                            style.padding = GetSpacingOffset(1f, 1f);
                            break;
                        case ControlSize.Default:
                            style.padding = GetSpacingOffset(4f, 4f);
                            break;
                    }
                    break;

                case "MenuBarItem":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(6f, 4f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(12f, 8f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(4f, 2f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(8f, 6f);
                            break;
                    }
                    break;

                case "SelectTrigger":
                case "SelectItem":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(8f, 4f);
                            style.fixedHeight = GetScaledHeight(28f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.0f);
                            style.padding = GetSpacingOffset(16f, 8f);
                            style.fixedHeight = GetScaledHeight(44f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.7f);
                            style.padding = GetSpacingOffset(6f, 2f);
                            style.fixedHeight = GetScaledHeight(24f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.875f);
                            style.padding = GetSpacingOffset(12f, 6f);
                            style.fixedHeight = GetScaledHeight(36f);
                            break;
                    }
                    break;

                case "SelectContent":
                    switch (size)
                    {
                        case ControlSize.Small:
                            style.fontSize = GetScaledFontSize(0.8f);
                            style.padding = GetSpacingOffset(8f, 6f);
                            break;
                        case ControlSize.Large:
                            style.fontSize = GetScaledFontSize(1.1f);
                            style.padding = GetSpacingOffset(12f, 10f);
                            break;
                        case ControlSize.Mini:
                            style.fontSize = GetScaledFontSize(0.75f);
                            style.padding = GetSpacingOffset(6f, 4f);
                            break;
                        case ControlSize.Default:
                            style.fontSize = GetScaledFontSize(0.95f);
                            style.padding = GetSpacingOffset(10f, 8f);
                            break;
                    }
                    break;
            }
        }

        #endregion

        #region Style Getters

        public GUIStyle GetButtonStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Button, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseButtonStyle ?? GUI.skin.button);
            ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Button");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetToggleStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Toggle, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseToggleStyle ?? GUI.skin.button);
            ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Toggle");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetInputStyle(ControlVariant variant, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            int state = (focused ? 1 : 0) | (disabled ? 2 : 0);
            var key = new StyleKey(StyleComponentType.Input, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseInputStyle ?? GUI.skin.textField);

            if (disabled)
                style.normal.textColor = ThemeManager.Instance.CurrentTheme.Muted;

            if (focused)
                style.focused.background = inputFocusedTexture;

            ApplyVariantToInputStyle(style, variant);
            ApplySizing(style, size, "Input");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetLabelStyle(ControlVariant variant, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.Label, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseLabelStyle ?? GUI.skin.label);
            ApplySizing(style, size, "Label");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetPasswordFieldStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            int state = (focused ? 1 : 0) | (disabled ? 2 : 0);
            var key = new StyleKey(StyleComponentType.PasswordField, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(passwordFieldStyle ?? GUI.skin.textField);

            if (disabled)
                style.normal.textColor = ThemeManager.Instance.CurrentTheme.Muted;

            if (focused)
                style.focused.background = inputFocusedTexture;

            ApplyVariantToInputStyle(style, variant);
            ApplySizing(style, size, "PasswordField");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetTextAreaStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false)
        {
            int state = focused ? 1 : 0;
            var key = new StyleKey(StyleComponentType.TextArea, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle baseStyle = textAreaStyle ?? GUI.skin.textArea;
            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle);

            switch (variant)
            {
                case ControlVariant.Outline:
                    style.normal.background = CreateRoundedOutlineTexture(128, 40, 6, theme.Border);
                    style.focused.background = CreateRoundedOutlineTexture(128, 40, 6, theme.Accent);
                    style.border = new UnityHelpers.RectOffset(6, 6, 6, 6);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = transparentTexture;
                    style.focused.background = CreateSolidTexture(Color.Lerp(theme.Secondary, Color.black, 0.08f));
                    break;
            }

            ApplySizing(style, size, "TextArea");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetProgressBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.ProgressBar, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(progressBarStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "ProgressBar");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            int state = (int)orientation;
            var key = new StyleKey(StyleComponentType.Separator, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle baseStyle = orientation switch
            {
                SeparatorOrientation.Horizontal => separatorHorizontalStyle ?? GUI.skin.box,
                SeparatorOrientation.Vertical => separatorVerticalStyle ?? GUI.skin.box,
                _ => separatorHorizontalStyle ?? GUI.skin.box,
            };
            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Separator", false, orientation);

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetTabsListStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.TabsList, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(tabsListStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "TabsList");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetTabsTriggerStyle(bool active = false, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            int state = active ? 1 : 0;
            var key = new StyleKey(StyleComponentType.TabsTrigger, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle baseStyle = active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) : (tabsTriggerStyle ?? GUI.skin.button);
            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle);

            if (variant != ControlVariant.Default)
            {
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
                if (active)
                {
                    style.normal = style.onNormal;
                    style.hover = style.onHover;
                    style.active = style.onActive;
                }
            }

            ApplySizing(style, size, "TabsTrigger");
            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetTabsContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.TabsContent, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(tabsContentStyle ?? GUIStyle.none);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "TabsContent");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCheckboxStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Checkbox, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseCheckboxStyle ?? GUI.skin.toggle);

            switch (variant)
            {
                case ControlVariant.Outline:
                    style.normal.background = CreateRoundedOutlineTexture(20, 20, 6, ThemeManager.Instance.CurrentTheme.Border);
                    style.onNormal.background = CreateOutlineButtonTexture(ThemeManager.Instance.CurrentTheme.Accent, ThemeManager.Instance.CurrentTheme.Accent);
                    style.border = new UnityHelpers.RectOffset(6, 6, 6, 6);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = transparentTexture;
                    style.onNormal.background = CreateSolidTexture(ThemeManager.Instance.CurrentTheme.Accent);
                    break;
            }

            ApplySizing(style, size, "Checkbox");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetSwitchStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Switch, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseSwitchStyle ?? GUI.skin.toggle);

            switch (variant)
            {
                case ControlVariant.Outline:
                    style.normal.background = CreateRoundedOutlineTexture(40, 24, 12, ThemeManager.Instance.CurrentTheme.Border);
                    style.onNormal.background = CreateOutlineButtonTexture(ThemeManager.Instance.CurrentTheme.Accent, ThemeManager.Instance.CurrentTheme.Accent);
                    style.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = transparentTexture;
                    style.onNormal.background = CreateSolidTexture(ThemeManager.Instance.CurrentTheme.Accent);
                    break;
            }

            ApplySizing(style, size, "Switch");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetBadgeStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Badge, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle style = new UnityHelpers.GUIStyle(baseBadgeStyle ?? GUI.skin.box);

            switch (variant)
            {
                case ControlVariant.Secondary:
                    style.normal.background = CreateGradientRoundedRectTexture(128, 24, 12, theme.Secondary, Color.Lerp(theme.Secondary, Color.black, 0.05f));
                    break;
                case ControlVariant.Destructive:
                    style.normal.background = CreateGradientRoundedRectTexture(128, 24, 12, theme.Destructive, Color.Lerp(theme.Destructive, Color.black, 0.1f));
                    break;
                case ControlVariant.Outline:
                    style.normal.background = CreateRoundedOutlineTexture(128, 24, 12, theme.Border);
                    style.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                    break;
            }

            ApplySizing(style, size, "Badge");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetAvatarStyle(ControlSize size, AvatarShape shape, ControlVariant variant = ControlVariant.Default)
        {
            int state = (int)shape;
            var key = new StyleKey(StyleComponentType.Avatar, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(avatarStyle ?? GUI.skin.box);
            int avatarSizeValue = size switch
            {
                ControlSize.Small => GetScaledHeight(32f),
                ControlSize.Large => GetScaledHeight(48f),
                ControlSize.Mini => GetScaledHeight(24f),
                ControlSize.Default => GetScaledHeight(40f),
                _ => GetScaledHeight(40f),
            };
            style.fixedWidth = avatarSizeValue;
            style.fixedHeight = avatarSizeValue;
            style.border = GetAvatarBorder(shape, size);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);

            styleCache[key] = style;
            return style;
        }

        public RectOffset GetAvatarBorder(AvatarShape shape, ControlSize size)
        {
            int borderRadius = shape switch
            {
                AvatarShape.Circle => Mathf.RoundToInt(50 * guiHelper.uiScale),
                AvatarShape.Rounded => GetScaledBorderRadius(8f),
                AvatarShape.Square => 0,
                _ => 0,
            };
            return new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }

        public float GetStatusIndicatorSize(ControlSize size) =>
            size switch
            {
                ControlSize.Small => 6f,
                ControlSize.Large => 12f,
                ControlSize.Mini => 4f,
                ControlSize.Default => 8f,
                _ => 8f,
            };

        public GUIStyle GetTableStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Table, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle baseStyle = variant switch
            {
                ControlVariant.Secondary => tableStripedStyle,
                ControlVariant.Outline => tableBorderedStyle,
                ControlVariant.Ghost => tableHoverStyle,
                _ => baseTableStyle,
            };
            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, theme);
            ApplySizing(style, size, "Table");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetTableHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.TableHeader, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(tableHeaderStyle ?? GUI.skin.label);
            ApplySizing(style, size, "TableChild", true);

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetTableCellStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            var key = new StyleKey(StyleComponentType.TableCell, variant, size, (int)alignment);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(tableCellStyle ?? GUI.skin.label);
            ApplySizing(style, size, "TableChild", false);
            style.alignment = alignment;

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Calendar, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseCalendarStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Calendar");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarHeader, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarHeaderStyle ?? GUI.skin.label);
            ApplySizing(style, size, "CalendarHeader");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarWeekdayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarWeekday, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarWeekdayStyle ?? GUI.skin.label);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarDayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarDay, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarDayStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarDaySelectedStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarDaySelected, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarDaySelectedStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
            {
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
                style.normal = style.onNormal;
                style.hover = style.onHover;
                style.active = style.onActive;
            }
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarDayOutsideMonthStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarDayOutsideMonth, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarDayOutsideMonthStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarDayTodayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarDayToday, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarDayTodayStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCalendarDayInRangeStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CalendarDayInRange, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(calendarDayInRangeStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
            {
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
                style.normal.background = CreateSolidTexture(new Color(ThemeManager.Instance.CurrentTheme.Accent.r, ThemeManager.Instance.CurrentTheme.Accent.g, ThemeManager.Instance.CurrentTheme.Accent.b, 0.55f));
            }
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDatePickerStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.DatePicker, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseDatePickerStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "DatePicker");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDatePickerWeekdayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.DatePickerWeekday, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(datePickerWeekdayStyle ?? GUI.skin.label);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDatePickerDayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.DatePickerDay, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(datePickerDayStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDatePickerDaySelectedStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.DatePickerDaySelected, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(datePickerDaySelectedStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
            {
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
                style.normal = style.onNormal;
                style.hover = style.onHover;
                style.active = style.onActive;
            }
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDatePickerDayOutsideMonthStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.DatePickerDayOutsideMonth, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(datePickerDayOutsideMonthStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDatePickerDayTodayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.DatePickerDayToday, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(datePickerDayTodayStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CalendarChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDialogContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.Dialog, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(dialogContentStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Dialog");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDropdownMenuItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.DropdownMenuItem, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(dropdownMenuItemStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "DropdownItem");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetDropdownMenuStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.DropdownMenu, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(dropdownMenuContentStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "DropdownMenu");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetPopoverContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.Popover, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(popoverContentStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Popover");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetSelectTriggerStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.SelectTrigger, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(selectTriggerStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "SelectTrigger");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetSelectItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.SelectItem, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(selectItemStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "SelectItem");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetSelectStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.SelectContent, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(selectContentStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "SelectContent");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetChartStyle(ControlVariant variant, ControlSize size)
        {
            var key = new StyleKey(StyleComponentType.Chart, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(chartContainerStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Chart");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetChartAxisStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.ChartAxis, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(chartAxisStyle ?? GUI.skin.label);
            ApplySizing(style, size, "ChartAxis");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetMenuBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.MenuBar, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(menuBarStyle ?? GUIStyle.none);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "MenuBar");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetMenuBarItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool isShortcut = false)
        {
            var key = new StyleKey(StyleComponentType.MenuBarItem, variant, size, isShortcut ? 1 : 0);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(menuBarItemStyle ?? GUI.skin.button);
            if (variant != ControlVariant.Default)
                ApplyVariantToStyle(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "MenuBarItem");

            if (isShortcut)
            {
                style.alignment = TextAnchor.MiddleRight;
            }

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetMenuDropdownStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.MenuDropdown, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(menuDropdownStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "DropdownMenu");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetAnimatedBoxStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.AnimatedBox, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(animatedBoxStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "AnimatedBox");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetSectionHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.SectionHeader, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(sectionHeaderStyle ?? GUI.skin.label);
            ApplySizing(style, size, "SectionHeader");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCardStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.Card, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(cardStyle ?? GUI.skin.box);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "Card");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCardHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CardHeader, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(cardHeaderStyle ?? GUIStyle.none);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CardChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCardTitleStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CardTitle, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(cardTitleStyle ?? GUI.skin.label);
            ApplySizing(style, size, "CardTitle");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCardDescriptionStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CardDescription, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(cardDescriptionStyle ?? GUI.skin.label);
            ApplySizing(style, size, "CardDescription");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCardContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CardContent, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(cardContentStyle ?? GUIStyle.none);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CardChild");

            styleCache[key] = style;
            return style;
        }

        public GUIStyle GetCardFooterStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            var key = new StyleKey(StyleComponentType.CardFooter, variant, size);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(cardFooterStyle ?? GUIStyle.none);
            ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
            ApplySizing(style, size, "CardChild");

            styleCache[key] = style;
            return style;
        }

        public Texture2D GetParticleTexture() => particleTexture;

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
            DestroyTexture(checkboxTexture);
            DestroyTexture(checkboxCheckedTexture);
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
