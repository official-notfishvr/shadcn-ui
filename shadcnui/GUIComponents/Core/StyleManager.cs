using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    #region Enums
    public enum ButtonVariant
    {
        Default,
        Destructive,
        Outline,
        Secondary,
        Ghost,
        Link,
    }

    public enum ButtonSize
    {
        Default,
        Small,
        Large,
        Icon,
    }

    public enum ToggleVariant
    {
        Default,
        Outline,
    }

    public enum ToggleSize
    {
        Default,
        Small,
        Large,
    }

    public enum InputVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum LabelVariant
    {
        Default,
        Secondary,
        Muted,
        Destructive,
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical,
    }

    public enum TextAreaVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum ProgressVariant
    {
        Default,
    }

    public enum TabsVariant
    {
        Default,
    }

    public enum CheckboxVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum CheckboxSize
    {
        Default,
        Small,
        Large,
    }

    public enum SwitchVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum SwitchSize
    {
        Default,
        Small,
        Large,
    }

    public enum BadgeVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline,
    }

    public enum BadgeSize
    {
        Default,
        Small,
        Large,
    }

    public enum AvatarSize
    {
        Small,
        Default,
        Large,
    }

    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded,
    }

    public enum TableVariant
    {
        Default,
        Striped,
        Bordered,
        Hover,
    }

    public enum TableSize
    {
        Small,
        Default,
        Large,
    }

    public enum CalendarVariant
    {
        Default,
    }

    public enum CalendarSize
    {
        Default,
        Small,
        Large,
    }

    public enum DropdownMenuVariant
    {
        Default,
    }

    public enum DropdownMenuSize
    {
        Default,
        Small,
        Large,
    }

    public enum ScrollAreaVariant
    {
        Default,
    }

    public enum ScrollAreaSize
    {
        Default,
        Small,
        Large,
    }

    public enum SelectVariant
    {
        Default,
    }

    public enum SelectSize
    {
        Default,
        Small,
        Large,
    }

    public enum ChartVariant
    {
        Default,
    }

    public enum ChartSize
    {
        Default,
        Small,
        Large,
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

        #region GUIStyle - Animated
        private GUIStyle animatedBoxStyle;
        private GUIStyle animatedButtonStyle;
        private GUIStyle animatedInputStyle;
        private GUIStyle glowLabelStyle;
        private GUIStyle titleStyle;
        private GUIStyle colorPresetStyle;
        private GUIStyle sectionHeaderStyle;
        #endregion

        #region GUIStyle - Card
        private GUIStyle cardStyle;
        private GUIStyle cardHeaderStyle;
        private GUIStyle cardTitleStyle;
        private GUIStyle cardDescriptionStyle;
        private GUIStyle cardContentStyle;
        private GUIStyle cardFooterStyle;
        #endregion

        #region GUIStyle - Button
        private GUIStyle buttonDefaultStyle;
        private GUIStyle buttonDestructiveStyle;
        private GUIStyle buttonOutlineStyle;
        private GUIStyle buttonSecondaryStyle;
        private GUIStyle buttonGhostStyle;
        private GUIStyle buttonLinkStyle;
        private GUIStyle buttonIconStyle;
        #endregion

        #region GUIStyle - Toggle
        private GUIStyle toggleDefaultStyle;
        private GUIStyle toggleOutlineStyle;
        #endregion

        #region GUIStyle - Input
        private GUIStyle inputDefaultStyle;
        private GUIStyle inputOutlineStyle;
        private GUIStyle inputGhostStyle;
        private GUIStyle inputFocusedStyle;
        private GUIStyle inputDisabledStyle;
        private GUIStyle passwordFieldStyle;
        #endregion

        #region GUIStyle - Label
        private GUIStyle labelDefaultStyle;
        private GUIStyle labelSecondaryStyle;
        private GUIStyle labelMutedStyle;
        private GUIStyle labelDestructiveStyle;
        #endregion

        #region GUIStyle - TextArea
        private GUIStyle textAreaStyle;
        private GUIStyle textAreaFocusedStyle;
        private GUIStyle textAreaOutlineStyle;
        private GUIStyle textAreaGhostStyle;
        #endregion

        #region GUIStyle - Progress
        private GUIStyle progressBarStyle;
        private GUIStyle progressBarBackgroundStyle;
        private GUIStyle progressBarFillStyle;
        #endregion

        #region GUIStyle - Separator
        private GUIStyle separatorHorizontalStyle;
        private GUIStyle separatorVerticalStyle;
        #endregion

        #region GUIStyle - Tabs
        private GUIStyle tabsListStyle;
        private GUIStyle tabsTriggerStyle;
        private GUIStyle tabsTriggerActiveStyle;
        private GUIStyle tabsContentStyle;
        #endregion

        #region GUIStyle - Checkbox
        private GUIStyle checkboxDefaultStyle;
        private GUIStyle checkboxOutlineStyle;
        private GUIStyle checkboxGhostStyle;
        #endregion

        #region GUIStyle - Switch
        private GUIStyle switchDefaultStyle;
        private GUIStyle switchOutlineStyle;
        private GUIStyle switchGhostStyle;
        #endregion

        #region GUIStyle - Badge
        private GUIStyle badgeDefaultStyle;
        private GUIStyle badgeSecondaryStyle;
        private GUIStyle badgeDestructiveStyle;
        private GUIStyle badgeOutlineStyle;
        #endregion

        #region GUIStyle - Alert
        private GUIStyle alertDefaultStyle;
        private GUIStyle alertDestructiveStyle;
        private GUIStyle alertTitleStyle;
        private GUIStyle alertDescriptionStyle;
        #endregion

        #region GUIStyle - Avatar
        private GUIStyle avatarStyle;
        #endregion

        #region GUIStyle - Table
        private GUIStyle tableStyle;
        private GUIStyle tableHeaderStyle;
        private GUIStyle tableCellStyle;
        private GUIStyle tableStripedStyle;
        private GUIStyle tableBorderedStyle;
        private GUIStyle tableHoverStyle;
        #endregion

        #region GUIStyle - Slider
        private GUIStyle sliderDefaultStyle;
        private GUIStyle sliderRangeStyle;
        private GUIStyle sliderVerticalStyle;
        private GUIStyle sliderDisabledStyle;
        private GUIStyle sliderLabelStyle;
        private GUIStyle sliderValueStyle;
        #endregion

        #region GUIStyle - Calendar
        private GUIStyle calendarStyle;
        private GUIStyle calendarHeaderStyle;
        private GUIStyle calendarTitleStyle;
        private GUIStyle calendarWeekdayStyle;
        private GUIStyle calendarDayStyle;
        private GUIStyle calendarDaySelectedStyle;
        private GUIStyle calendarDayOutsideMonthStyle;
        private GUIStyle calendarDayTodayStyle;
        private GUIStyle calendarDayDisabledStyle;
        private GUIStyle calendarDayInRangeStyle;
        #endregion

        #region GUIStyle - DatePicker
        private GUIStyle datePickerStyle;
        private GUIStyle datePickerHeaderStyle;
        private GUIStyle datePickerTitleStyle;
        private GUIStyle datePickerWeekdayStyle;
        private GUIStyle datePickerDayStyle;
        private GUIStyle datePickerDaySelectedStyle;
        private GUIStyle datePickerDayOutsideMonthStyle;
        private GUIStyle datePickerDayTodayStyle;
        private GUIStyle datePickerDayDisabledStyle;
        private GUIStyle datePickerDayInRangeStyle;
        #endregion

        #region GUIStyle - Dialog
        private GUIStyle dialogOverlayStyle;
        private GUIStyle dialogContentStyle;
        private GUIStyle dialogTitleStyle;
        private GUIStyle dialogDescriptionStyle;
        #endregion

        #region GUIStyle - DropdownMenu
        private GUIStyle dropdownMenuContentStyle;
        private GUIStyle dropdownMenuItemStyle;
        private GUIStyle dropdownMenuSeparatorStyle;
        private GUIStyle dropdownMenuHeaderStyle;
        #endregion

        #region GUIStyle - Popover
        private GUIStyle popoverContentStyle;
        #endregion

        #region GUIStyle - ScrollArea
        private GUIStyle scrollAreaStyle;
        private GUIStyle scrollAreaThumbStyle;
        private GUIStyle scrollAreaTrackStyle;
        #endregion

        #region GUIStyle - Select
        private GUIStyle selectTriggerStyle;
        private GUIStyle selectContentStyle;
        private GUIStyle selectItemStyle;
        #endregion

        #region GUIStyle - Chart
        private GUIStyle chartContainerStyle;
        private GUIStyle chartContentStyle;
        private GUIStyle chartAxisStyle;
        private GUIStyle chartGridStyle;
        #endregion

        #region GUIStyle - MenuBar
        private GUIStyle menuBarStyle;
        private GUIStyle menuBarItemStyle;
        private GUIStyle menuDropdownStyle;
        #endregion

        #region Style Caches
        private Dictionary<(ButtonVariant, ButtonSize), GUIStyle> buttonStyleCache = new();
        private Dictionary<(ToggleVariant, ToggleSize), GUIStyle> toggleStyleCache = new();
        private Dictionary<(InputVariant, bool, bool), GUIStyle> inputStyleCache = new();
        private Dictionary<LabelVariant, GUIStyle> labelStyleCache = new();
        private Dictionary<(TextAreaVariant, bool), GUIStyle> textAreaStyleCache = new();
        private Dictionary<(CheckboxVariant, CheckboxSize), GUIStyle> checkboxStyleCache = new();
        private Dictionary<(SwitchVariant, SwitchSize), GUIStyle> switchStyleCache = new();
        private Dictionary<(BadgeVariant, BadgeSize), GUIStyle> badgeStyleCache = new();
        private Dictionary<(AvatarSize, AvatarShape), GUIStyle> avatarStyleCache = new();
        private Dictionary<(TableVariant, TableSize), GUIStyle> tableStyleCache = new();
        private Dictionary<(CalendarVariant, CalendarSize), GUIStyle> calendarStyleCache = new();
        private Dictionary<(DropdownMenuVariant, DropdownMenuSize), GUIStyle> dropdownMenuStyleCache = new();
        private Dictionary<(ScrollAreaVariant, ScrollAreaSize), GUIStyle> scrollAreaStyleCache = new();
        private Dictionary<(SelectVariant, SelectSize), GUIStyle> selectStyleCache = new();
        private Dictionary<(ChartVariant, ChartSize), GUIStyle> chartStyleCache = new();
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

        private void SetupAllStyles()
        {
            try
            {
                SetupAnimatedStyles();
                SetupCardStyles();
                SetupButtonVariantStyles();
                SetupToggleVariantStyles();
                SetupInputVariantStyles();
                SetupLabelVariantStyles();
                SetupProgressBarStyles();
                SetupSeparatorStyles();
                SetupTabsStyles();
                SetupTextAreaVariantStyles();
                SetupCheckboxStyles();
                SetupSwitchStyles();
                SetupBadgeStyles();
                SetupAvatarStyles();
                SetupTableStyles();
                SetupCalendarStyles();
                SetupDropdownMenuStyles();
                SetupPopoverStyles();
                SetupScrollAreaStyles();
                SetupSelectStyles();
                SetupDatePickerStyles();
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

        public Color GetFocusColor(Color baseColor) => Color.Lerp(baseColor, ThemeManager.Instance.CurrentTheme.AccentColor, 0.25f);

        public Color GetOverlayColor() => ThemeManager.Instance.CurrentTheme.OverlayColor;

        public Color GetShadowColor() => ThemeManager.Instance.CurrentTheme.ShadowColor;

        public Color GetBorderColor() => ThemeManager.Instance.CurrentTheme.BorderColor;
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
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float alpha = Mathf.Clamp01(1f - (distance / (size / 2f)));
                alpha = Mathf.Pow(alpha, 2f);
                texture.SetPixel(x, y, new Color(glowColor.r, glowColor.g, glowColor.b, alpha * 0.45f));
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateShadowTexture(int width, int height, float intensity = 0.22f, float blur = 3.5f)
        {
            var texture = new Texture2D(width, height);
            Color themeShadow = ThemeManager.Instance.CurrentTheme.ShadowColor;
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                float edgeDistanceX = Mathf.Min(x, width - x - 1);
                float edgeDistanceY = Mathf.Min(y, height - y - 1);
                float edgeDistance = Mathf.Min(edgeDistanceX, edgeDistanceY);
                float alpha = Mathf.Clamp01(edgeDistance / blur);
                alpha = (1f - alpha) * intensity;
                texture.SetPixel(x, y, new Color(themeShadow.r, themeShadow.g, themeShadow.b, alpha));
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
                bool isBorder = y < borderThickness ? false : (y >= height - borderThickness);
                if (isBorder)
                {
                    texture.SetPixel(x, y, borderColor);
                }
                else
                {
                    texture.SetPixel(x, y, fillColor);
                }
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

        public Texture2D CreateRoundedRectTexture(int width, int height, int radius, Color color)
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
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }

        public Texture2D CreateRoundedRectWithShadowTexture(int width, int height, int radius, Color fillColor, float shadowIntensity = 0.18f, int shadowBlur = 9)
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
                    texture.SetPixel(x, y, fillColor);
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

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor, Color borderColor)
        {
            if (outlineButtonTextureCache.TryGetValue((backgroundColor, borderColor), out var cachedTexture))
                return cachedTexture;
            Texture2D texture = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                bool isBorder = x == 0 || y == 0 || x == 3 || y == 3;
                texture.SetPixel(x, y, isBorder ? borderColor : backgroundColor);
            }
            texture.Apply();
            outlineButtonTextureCache[(backgroundColor, borderColor)] = texture;
            return texture;
        }

        private Texture2D CreateOutlineTexture()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            var keyColor = theme.BorderColor;
            if (outlineTextureCache.TryGetValue(keyColor, out var cachedTexture))
                return cachedTexture;
            Texture2D texture = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                bool isBorder = x == 0 || y == 0 || x == 3 || y == 3;
                texture.SetPixel(x, y, isBorder ? keyColor : Color.clear);
            }
            texture.Apply();
            outlineTextureCache[keyColor] = texture;
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
                    gradientTexture.SetPixel(0, i, Color.Lerp(theme.PrimaryColor, theme.SecondaryColor, t));
                }
                gradientTexture.Apply();

                cardBackgroundTexture = CreateRoundedRectWithShadowTexture(128, 128, 12, theme.CardBg, 0.16f, 11);
                inputBackgroundTexture = CreateRoundedRectTexture(128, 128, 8, theme.InputBg);
                inputFocusedTexture = CreateRoundedRectTexture(128, 128, 8, theme.InputBg);
                outlineTexture = CreateRoundedRectTexture(128, 128, 8, theme.ButtonOutlineBorder);
                transparentTexture = CreateSolidTexture(Color.clear);
                glowTexture = CreateGlowTexture(theme.AccentColor, 32);
                particleTexture = CreateSolidTexture(Color.Lerp(theme.AccentColor, theme.TextColor, 0.5f));
                progressBarBackgroundTexture = CreateRoundedRectTexture(128, 128, 6, theme.SecondaryColor);
                progressBarFillTexture = CreateRoundedRectTexture(128, 128, 6, theme.PrimaryColor);
                separatorTexture = CreateSolidTexture(theme.SeparatorColor);
                tabsBackgroundTexture = CreateRoundedRectTexture(128, 128, 6, theme.TabsBg);
                tabsActiveTexture = CreateRoundedRectTexture(128, 128, 6, theme.BackgroundColor);
                checkboxTexture = CreateRoundedRectTexture(16, 16, 4, theme.CheckboxBg);
                checkboxCheckedTexture = CreateRoundedRectTexture(16, 16, 4, theme.CheckboxCheckedBg);
                switchTexture = CreateRoundedRectTexture(32, 16, 8, theme.SwitchBg);
                switchOnTexture = CreateRoundedRectTexture(32, 16, 8, theme.SwitchOnBg);
                switchOffTexture = CreateRoundedRectTexture(32, 16, 8, theme.SwitchOffBg);
                badgeTexture = CreateRoundedRectTexture(64, 24, 12, theme.BadgeBg);
                avatarTexture = CreateRoundedRectTexture(40, 40, 20, theme.AvatarBg);
                tableTexture = CreateSolidTexture(theme.TableBg);
                tableHeaderTexture = CreateSolidTexture(theme.TableHeaderBg);
                tableCellTexture = CreateSolidTexture(theme.TableCellBg);
                calendarBackgroundTexture = CreateRoundedRectTexture(256, 256, 8, theme.CardBg);
                calendarHeaderTexture = CreateSolidTexture(theme.CardBg);
                calendarDayTexture = CreateRoundedRectTexture(32, 32, 4, theme.CardBg);
                calendarDaySelectedTexture = CreateRoundedRectTexture(32, 32, 4, theme.PrimaryColor);
                dropdownMenuContentTexture = CreateRoundedRectWithShadowTexture(128, 128, 8, theme.CardBg, 0.14f, 10);
                popoverContentTexture = CreateRoundedRectWithShadowTexture(128, 128, 8, theme.CardBg, 0.14f, 10);
                scrollAreaThumbTexture = CreateRoundedRectTexture(8, 8, 4, theme.ButtonOutlineBorder);
                scrollAreaTrackTexture = CreateRoundedRectTexture(8, 8, 4, theme.SecondaryColor);
                selectTriggerTexture = CreateRoundedRectTexture(128, 40, 8, theme.InputBg);
                selectContentTexture = CreateRoundedRectWithShadowTexture(128, 128, 8, theme.CardBg, 0.14f, 10);
                chartContainerTexture = CreateRoundedRectWithShadowTexture(256, 256, 8, theme.ChartBg, 0.12f, 8);
                chartGridTexture = CreateSolidTexture(theme.ChartGridColor);
                chartAxisTexture = CreateSolidTexture(theme.ChartAxisColor);

                GUILogger.LogInfo("Custom textures created successfully", "StyleManager.CreateCustomTextures");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "CreateCustomTextures", "StyleManager");
            }
        }
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
                animatedButtonStyle = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize, FontStyle.Bold);
                animatedButtonStyle.alignment = TextAnchor.MiddleCenter;
                animatedButtonStyle.normal.textColor = Color.Lerp(Color.white, theme.AccentColor, 0.4f);
                animatedButtonStyle.hover.textColor = theme.AccentColor;
                colorPresetStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(guiHelper.fontSize * 0.9f), FontStyle.Bold);
                colorPresetStyle.alignment = TextAnchor.MiddleCenter;
                animatedInputStyle = CreateStyleWithFont(GUI.skin.textField, guiHelper.fontSize + 1);
                animatedInputStyle.padding = new UnityHelpers.RectOffset(8, 8, 4, 4);
                animatedInputStyle.normal.textColor = theme.TextColor;
                animatedInputStyle.focused.textColor = theme.AccentColor;
                glowLabelStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize);
                glowLabelStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.AccentColor, 0.25f);
                titleStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 4, FontStyle.Bold);
                titleStyle.alignment = TextAnchor.MiddleCenter;
                titleStyle.normal.textColor = theme.AccentColor;
                sectionHeaderStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 2, FontStyle.Bold);
                sectionHeaderStyle.normal.textColor = Color.Lerp(theme.AccentColor, theme.TextColor, 0.4f);
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
                cardStyle.normal.background = cardBackgroundTexture;
                cardStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                cardStyle.padding = GetSpacingOffset(24f, 24f);
                cardStyle.margin = GetSpacingOffset(0f, 0f);
                cardHeaderStyle = new UnityHelpers.GUIStyle();
                cardHeaderStyle.padding = GetSpacingOffset(0f, 16f);
                cardTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.5f), FontStyle.Bold);
                cardTitleStyle.normal.textColor = theme.CardTitle;
                cardTitleStyle.wordWrap = true;
                cardDescriptionStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f));
                cardDescriptionStyle.normal.textColor = theme.CardDescription;
                cardDescriptionStyle.wordWrap = true;
                cardDescriptionStyle.margin = GetSpacingOffset(0f, 4f);
                cardContentStyle = new UnityHelpers.GUIStyle();
                cardContentStyle.padding = GetSpacingOffset(0f, 0f);
                cardFooterStyle = new UnityHelpers.GUIStyle();
                cardFooterStyle.padding = GetSpacingOffset(0f, 16f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCardStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Button
        private void SetupButtonVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                buttonDefaultStyle = CreateButtonStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, theme.ButtonPrimaryActiveBg, theme.ButtonPrimaryActiveFg);
                buttonDestructiveStyle = CreateButtonStyle(theme.ButtonDestructiveBg, theme.ButtonDestructiveFg, Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.25f), theme.ButtonDestructiveFg);
                buttonOutlineStyle = CreateButtonStyle(theme.ButtonOutlineBorder, theme.ButtonOutlineFg, Color.Lerp(theme.ButtonOutlineHoverBg, Color.black, 0.08f), theme.ButtonOutlineHoverFg);
                buttonSecondaryStyle = CreateButtonStyle(theme.ButtonSecondaryBg, theme.ButtonSecondaryFg, Color.Lerp(theme.ButtonSecondaryBg, Color.black, 0.1f), theme.ButtonSecondaryFg);
                buttonGhostStyle = CreateButtonStyle(Color.clear, theme.ButtonGhostFg, Color.Lerp(theme.ButtonGhostHoverBg, Color.black, 0.12f), theme.ButtonGhostHoverFg);
                buttonLinkStyle = CreateLinkButtonStyle(theme);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupButtonVariantStyles", "StyleManager");
            }
        }

        private GUIStyle CreateButtonStyle(Color normalBg, Color normalFg, Color activeBg, Color activeFg)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f));
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = GetSpacingOffset(16f, 8f);
            style.border = GetBorderOffset(8f);
            style.fixedHeight = GetScaledHeight(40f);
            int btnTexH = GetScaledHeight(40f);
            int btnRadius = GetScaledBorderRadius(8f);
            style.normal.background = CreateRoundedRectWithShadowTexture(128, btnTexH, btnRadius, normalBg, 0.1f, 7);
            style.hover.background = CreateRoundedRectWithShadowTexture(128, btnTexH, btnRadius, GetHoverColor(normalBg), 0.12f, 8);
            style.normal.textColor = normalFg;
            style.hover.textColor = normalFg;
            style.active.background = CreateRoundedRectWithShadowTexture(128, btnTexH, btnRadius, activeBg, 0.14f, 7);
            style.active.textColor = activeFg;
            style.focused.background = CreateRoundedRectWithShadowTexture(128, btnTexH, btnRadius, Color.Lerp(normalBg, ThemeManager.Instance.CurrentTheme.AccentColor, 0.15f), 0.12f, 8);
            style.focused.textColor = normalFg;
            return style;
        }

        private GUIStyle CreateLinkButtonStyle(Theme theme)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f));
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = GetSpacingOffset(0f, 2f);
            style.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.normal.background = transparentTexture;
            style.hover.background = transparentTexture;
            style.normal.textColor = theme.ButtonLinkColor;
            style.hover.textColor = theme.ButtonLinkHoverColor;
            style.active.background = transparentTexture;
            style.active.textColor = Color.Lerp(theme.ButtonLinkHoverColor, Color.black, 0.25f);
            return style;
        }
        #endregion

        #region Style Setup - Toggle
        private void SetupToggleVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                toggleDefaultStyle = CreateToggleStyle(theme.ToggleBg, theme.ToggleFg, theme.ToggleOnBg, theme.ToggleOnFg);
                toggleOutlineStyle = new UnityHelpers.GUIStyle(toggleDefaultStyle);
                if (customFont != null)
                    toggleOutlineStyle.font = customFont;
                toggleOutlineStyle.normal.background = CreateOutlineButtonTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.15f), theme.AccentColor);
                toggleOutlineStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupToggleVariantStyles", "StyleManager");
            }
        }

        private GUIStyle CreateToggleStyle(Color normalBg, Color normalFg, Color onBg, Color onFg)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = normalFg;
            style.normal.background = CreateSolidTexture(normalBg);
            style.hover.background = CreateSolidTexture(ThemeManager.Instance.CurrentTheme.ToggleHoverBg);
            style.hover.textColor = ThemeManager.Instance.CurrentTheme.ToggleHoverFg;
            style.active.background = CreateSolidTexture(Color.Lerp(normalBg, Color.black, 0.15f));
            style.active.textColor = Color.Lerp(normalFg, Color.white, 0.5f);
            style.onNormal.background = CreateSolidTexture(onBg);
            style.onNormal.textColor = onFg;
            style.onHover.background = CreateSolidTexture(Color.Lerp(onBg, Color.white, 0.12f));
            style.onHover.textColor = onFg;
            style.onActive.background = CreateSolidTexture(Color.Lerp(onBg, Color.black, 0.15f));
            style.onActive.textColor = onFg;
            return style;
        }
        #endregion

        #region Style Setup - Input
        private void SetupInputVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                inputDefaultStyle = CreateInputStyle(inputBackgroundTexture, theme.InputFg, theme.InputFocusedFg);
                inputOutlineStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputOutlineStyle.font = customFont;
                inputOutlineStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.InputBg);
                inputOutlineStyle.focused.background = CreateRoundedRectTexture(128, 40, 8, theme.InputBg);
                inputGhostStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputGhostStyle.font = customFont;
                inputGhostStyle.normal.background = transparentTexture;
                inputGhostStyle.focused.background = transparentTexture;
                inputFocusedStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputFocusedStyle.font = customFont;
                inputFocusedStyle.normal.background = inputFocusedTexture;
                inputFocusedStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);
                inputDisabledStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputDisabledStyle.font = customFont;
                inputDisabledStyle.normal.textColor = theme.InputDisabledFg;
                inputDisabledStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.InputDisabledBg);
                passwordFieldStyle = CreateStyleWithFont(inputDefaultStyle, guiHelper.fontSize + 2);
                textAreaStyle = CreateStyleWithFont(inputDefaultStyle, guiHelper.fontSize);
                textAreaStyle.wordWrap = true;
                textAreaStyle.stretchHeight = true;
                textAreaStyle.padding = new UnityHelpers.RectOffset(12, 12, 8, 8);
                textAreaFocusedStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    textAreaFocusedStyle.font = customFont;
                textAreaFocusedStyle.normal.background = inputFocusedTexture;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupInputVariantStyles", "StyleManager");
            }
        }

        private GUIStyle CreateInputStyle(Texture2D background, Color textColor, Color focusedColor)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.textField, GetScaledFontSize(0.875f));
            style.padding = GetSpacingOffset(12f, 8f);
            style.margin = GetSpacingOffset(0f, 4f);
            style.border = GetBorderOffset(8f);
            style.fixedHeight = GetScaledHeight(40f);
            style.normal.background = background;
            style.normal.textColor = textColor;
            style.hover.background = background;
            style.hover.textColor = textColor;
            style.focused.background = inputFocusedTexture;
            style.focused.textColor = focusedColor;
            int inpRadius = GetScaledBorderRadius(8f);
            var theme = ThemeManager.Instance.CurrentTheme;
            Color focusTint = Color.Lerp(theme.AccentColor, theme.OverlayColor, 0.3f);
            style.onFocused.background = CreateRoundedRectTexture(128, GetScaledHeight(40f), inpRadius, focusTint);
            return style;
        }
        #endregion

        #region Style Setup - Label
        private void SetupLabelVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                labelDefaultStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f));
                labelDefaultStyle.normal.textColor = theme.TextColor;
                labelDefaultStyle.padding = GetSpacingOffset(0f, 2f);
                labelDefaultStyle.wordWrap = true;
                labelSecondaryStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.875f));
                labelSecondaryStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.MutedColor, 0.35f);
                labelMutedStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.875f));
                labelMutedStyle.normal.textColor = theme.MutedColor;
                labelDestructiveStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f), FontStyle.Bold);
                labelDestructiveStyle.normal.textColor = theme.ButtonDestructiveBg;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupLabelVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Progress
        private void SetupProgressBarStyles()
        {
            try
            {
                progressBarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                progressBarStyle.normal.background = progressBarBackgroundTexture;
                progressBarStyle.border = GetBorderOffset(6f);
                progressBarStyle.padding = GetSpacingOffset(0f, 0f);
                progressBarStyle.margin = GetSpacingOffset(0f, 2f);
                progressBarBackgroundStyle = new UnityHelpers.GUIStyle();
                progressBarBackgroundStyle.normal.background = progressBarBackgroundTexture;
                progressBarFillStyle = new UnityHelpers.GUIStyle();
                progressBarFillStyle.normal.background = progressBarFillTexture;
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
                separatorHorizontalStyle.normal.background = CreateSolidTexture(theme.BorderColor);
                separatorHorizontalStyle.fixedHeight = Mathf.RoundToInt(1 * guiHelper.uiScale);
                separatorHorizontalStyle.stretchWidth = true;
                separatorVerticalStyle = new UnityHelpers.GUIStyle();
                separatorVerticalStyle.normal.background = CreateSolidTexture(theme.BorderColor);
                separatorVerticalStyle.fixedWidth = Mathf.RoundToInt(1 * guiHelper.uiScale);
                separatorVerticalStyle.stretchHeight = true;
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
                tabsListStyle.normal.background = CreateSolidTexture(theme.TabsBg);
                tabsListStyle.border = GetBorderOffset(6f);
                tabsListStyle.padding = GetSpacingOffset(4f, 4f);
                tabsListStyle.margin = GetSpacingOffset(0f, 2f);
                tabsTriggerStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f), FontStyle.Bold);
                tabsTriggerStyle.alignment = TextAnchor.MiddleCenter;
                tabsTriggerStyle.padding = GetSpacingOffset(12f, 4f);
                tabsTriggerStyle.border = GetBorderOffset(6f);
                tabsTriggerStyle.normal.background = transparentTexture;
                tabsTriggerStyle.normal.textColor = theme.TabsTriggerFg;
                tabsTriggerStyle.hover.background = transparentTexture;
                tabsTriggerStyle.active.background = transparentTexture;
                tabsTriggerActiveStyle = new UnityHelpers.GUIStyle(tabsTriggerStyle);
                if (customFont != null)
                    tabsTriggerActiveStyle.font = customFont;

                tabsTriggerActiveStyle.normal.background = CreateBottomBorderTexture(128, GetScaledHeight(36f), Mathf.Max(2, Mathf.RoundToInt(2.5f * guiHelper.uiScale)), theme.AccentColor, theme.TabsTriggerActiveBg);
                tabsTriggerActiveStyle.normal.textColor = theme.TabsTriggerActiveFg;
                tabsTriggerActiveStyle.hover.background = tabsTriggerActiveStyle.normal.background;
                tabsTriggerActiveStyle.active.background = tabsTriggerActiveStyle.normal.background;
                tabsContentStyle = new UnityHelpers.GUIStyle();
                tabsContentStyle.padding = GetSpacingOffset(15f, 15f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTabsStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - TextArea
        private void SetupTextAreaVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                textAreaStyle = CreateStyleWithFont(GUI.skin.textArea, GetScaledFontSize(0.875f));
                textAreaStyle.padding = GetSpacingOffset(12f, 8f);
                textAreaStyle.border = GetBorderOffset(6f);
                textAreaStyle.normal.background = inputBackgroundTexture;
                textAreaStyle.normal.textColor = theme.InputFg;
                textAreaStyle.focused.background = inputFocusedTexture;
                textAreaStyle.focused.textColor = theme.InputFocusedFg;
                textAreaStyle.hover.background = inputBackgroundTexture;
                textAreaStyle.wordWrap = true;
                textAreaStyle.stretchHeight = true;
                textAreaFocusedStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaFocusedStyle.normal.background = inputFocusedTexture;
                textAreaFocusedStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                textAreaOutlineStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaOutlineStyle.normal.background = CreateOutlineTexture();
                textAreaOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                textAreaGhostStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaGhostStyle.normal.background = transparentTexture;
                textAreaGhostStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.15f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTextAreaVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Checkbox
        private void SetupCheckboxStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                checkboxDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                if (customFont != null)
                    checkboxDefaultStyle.font = customFont;
                checkboxDefaultStyle.fontSize = GetScaledFontSize(0.875f);
                checkboxDefaultStyle.normal.background = checkboxTexture;
                checkboxDefaultStyle.normal.textColor = Color.white;
                checkboxDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxBg, Color.white, 0.12f));
                checkboxDefaultStyle.hover.textColor = Color.white;
                checkboxDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.CheckboxBg, Color.black, 0.15f));
                checkboxDefaultStyle.active.textColor = Color.white;
                checkboxDefaultStyle.onNormal.background = CreateSolidTexture(theme.ToggleOnBg);
                checkboxDefaultStyle.onNormal.textColor = Color.white;
                checkboxDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.ToggleOnBg, Color.white, 0.12f));
                checkboxDefaultStyle.onHover.textColor = Color.white;
                checkboxDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.ToggleOnBg, Color.black, 0.15f));
                checkboxDefaultStyle.onActive.textColor = Color.white;
                checkboxDefaultStyle.border = GetBorderOffset(6f);
                checkboxOutlineStyle = new UnityHelpers.GUIStyle(checkboxDefaultStyle);
                if (customFont != null)
                    checkboxOutlineStyle.font = customFont;
                checkboxOutlineStyle.normal.background = CreateOutlineTexture();
                checkboxOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.12f));
                checkboxOutlineStyle.hover.textColor = Color.white;
                checkboxOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                checkboxOutlineStyle.active.textColor = Color.white;
                checkboxOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.AccentColor, theme.AccentColor);
                checkboxOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.12f));
                checkboxOutlineStyle.onHover.textColor = Color.white;
                checkboxOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                checkboxOutlineStyle.onActive.textColor = Color.white;
                checkboxGhostStyle = new UnityHelpers.GUIStyle(checkboxDefaultStyle);
                if (customFont != null)
                    checkboxGhostStyle.font = customFont;
                checkboxGhostStyle.normal.background = transparentTexture;
                checkboxGhostStyle.normal.textColor = theme.TextColor;
                checkboxGhostStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.12f));
                checkboxGhostStyle.hover.textColor = theme.TextColor;
                checkboxGhostStyle.active.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.2f));
                checkboxGhostStyle.active.textColor = theme.TextColor;
                checkboxGhostStyle.onNormal.background = CreateSolidTexture(theme.AccentColor);
                checkboxGhostStyle.onNormal.textColor = Color.white;
                checkboxGhostStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.12f));
                checkboxGhostStyle.onHover.textColor = Color.white;
                checkboxGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                checkboxGhostStyle.onActive.textColor = Color.white;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCheckboxStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Switch
        private void SetupSwitchStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                switchDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                if (customFont != null)
                    switchDefaultStyle.font = customFont;
                switchDefaultStyle.fontSize = GetScaledFontSize(0.875f);
                switchDefaultStyle.normal.background = switchOffTexture;
                switchDefaultStyle.normal.textColor = Color.white;
                switchDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.SwitchOffBg, Color.white, 0.12f));
                switchDefaultStyle.hover.textColor = Color.white;
                switchDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.SwitchOffBg, Color.black, 0.15f));
                switchDefaultStyle.active.textColor = Color.white;
                switchDefaultStyle.onNormal.background = switchOnTexture;
                switchDefaultStyle.onNormal.textColor = Color.white;
                switchDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.SwitchOnBg, Color.white, 0.12f));
                switchDefaultStyle.onHover.textColor = Color.white;
                switchDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.SwitchOnBg, Color.black, 0.15f));
                switchDefaultStyle.onActive.textColor = Color.white;
                switchDefaultStyle.border = GetBorderOffset(6f);
                switchOutlineStyle = new UnityHelpers.GUIStyle(switchDefaultStyle);
                if (customFont != null)
                    switchOutlineStyle.font = customFont;
                switchOutlineStyle.normal.background = CreateOutlineTexture();
                switchOutlineStyle.normal.textColor = Color.white;
                switchOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.12f));
                switchOutlineStyle.hover.textColor = Color.white;
                switchOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                switchOutlineStyle.active.textColor = Color.white;
                switchOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.AccentColor, theme.AccentColor);
                switchOutlineStyle.onNormal.textColor = Color.white;
                switchOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.12f));
                switchOutlineStyle.onHover.textColor = Color.white;
                switchOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                switchOutlineStyle.onActive.textColor = Color.white;
                switchGhostStyle = new UnityHelpers.GUIStyle(switchDefaultStyle);
                if (customFont != null)
                    switchGhostStyle.font = customFont;
                switchGhostStyle.normal.background = transparentTexture;
                switchGhostStyle.normal.textColor = theme.TextColor;
                switchGhostStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.12f));
                switchGhostStyle.hover.textColor = theme.TextColor;
                switchGhostStyle.active.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.2f));
                switchGhostStyle.active.textColor = theme.TextColor;
                switchGhostStyle.onNormal.background = CreateSolidTexture(theme.AccentColor);
                switchGhostStyle.onNormal.textColor = Color.white;
                switchGhostStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.12f));
                switchGhostStyle.onHover.textColor = Color.white;
                switchGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                switchGhostStyle.onActive.textColor = Color.white;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSwitchStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Badge
        private void SetupBadgeStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                badgeDefaultStyle = CreateStyleWithFont(GUI.skin.box, GetScaledFontSize(0.75f));
                badgeDefaultStyle.normal.background = CreateSolidTexture(theme.BadgeBg);
                badgeDefaultStyle.normal.textColor = theme.TextColor;
                badgeDefaultStyle.border = GetBorderOffset(12f);
                badgeDefaultStyle.padding = GetSpacingOffset(8f, 3f);
                badgeDefaultStyle.alignment = TextAnchor.MiddleCenter;
                badgeSecondaryStyle = new UnityHelpers.GUIStyle(badgeDefaultStyle);
                if (customFont != null)
                    badgeSecondaryStyle.font = customFont;
                badgeSecondaryStyle.normal.background = CreateSolidTexture(theme.BadgeSecondaryBg);
                badgeDestructiveStyle = new UnityHelpers.GUIStyle(badgeDefaultStyle);
                if (customFont != null)
                    badgeDestructiveStyle.font = customFont;
                badgeDestructiveStyle.normal.background = CreateSolidTexture(theme.BadgeDestructiveBg);
                badgeOutlineStyle = new UnityHelpers.GUIStyle(badgeDefaultStyle);
                if (customFont != null)
                    badgeOutlineStyle.font = customFont;
                badgeOutlineStyle.normal.background = CreateOutlineTexture();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupBadgeStyles", "StyleManager");
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
                avatarStyle.normal.background = CreateSolidTexture(theme.AvatarBg);
                avatarStyle.alignment = TextAnchor.MiddleCenter;
                avatarStyle.fixedWidth = defaultAvatarSize;
                avatarStyle.fixedHeight = defaultAvatarSize;
                avatarStyle.border = GetAvatarBorder(AvatarShape.Circle, AvatarSize.Default);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAvatarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Table
        private void SetupTableStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                tableStyle = new UnityHelpers.GUIStyle();
                tableStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);
                tableStyle.border = GetBorderOffset(1f);
                tableStyle.padding = GetSpacingOffset(0f, 0f);
                tableStyle.margin = GetSpacingOffset(0f, 0f);
                tableHeaderStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f), FontStyle.Bold);
                tableHeaderStyle.normal.background = CreateSolidTexture(theme.SecondaryColor);
                tableHeaderStyle.hover.background = CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.85f));
                tableHeaderStyle.active.background = CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.95f));
                tableHeaderStyle.normal.textColor = theme.TextColor;
                tableHeaderStyle.hover.textColor = theme.TextColor;
                tableHeaderStyle.active.textColor = theme.TextColor;
                tableHeaderStyle.padding = GetSpacingOffset(12f, 12f);
                tableHeaderStyle.alignment = TextAnchor.MiddleLeft;
                tableHeaderStyle.border = GetBorderOffset(0f);
                tableCellStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f));
                tableCellStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);
                tableCellStyle.normal.textColor = theme.TextColor;
                tableCellStyle.padding = GetSpacingOffset(12f, 12f);
                tableCellStyle.alignment = TextAnchor.MiddleLeft;
                tableCellStyle.wordWrap = false;
                tableCellStyle.clipping = TextClipping.Clip;
                tableStripedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableStripedStyle.font = customFont;
                tableStripedStyle.normal.background = CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.35f));
                tableBorderedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableBorderedStyle.font = customFont;
                tableBorderedStyle.normal.background = CreateBorderTexture(theme.ButtonOutlineBorder, 1);
                tableHoverStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableHoverStyle.font = customFont;
                tableHoverStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);
                tableHoverStyle.normal.textColor = theme.TextColor;
                tableHoverStyle.hover.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.12f));
                tableHoverStyle.hover.textColor = theme.TextColor;
                tableHoverStyle.active.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.25f));
                tableHoverStyle.active.textColor = theme.TextColor;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTableStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Calendar
        private void SetupCalendarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                calendarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                calendarStyle.normal.background = CreateSolidTexture(theme.CardBg);
                calendarStyle.border = GetBorderOffset(4f);
                calendarStyle.padding = GetSpacingOffset(12f, 12f);
                calendarHeaderStyle = new UnityHelpers.GUIStyle();
                calendarHeaderStyle.padding = GetSpacingOffset(0f, 4f);
                calendarTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.125f), FontStyle.Bold);
                calendarTitleStyle.normal.textColor = theme.TextColor;
                calendarTitleStyle.alignment = TextAnchor.MiddleCenter;
                calendarWeekdayStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.875f));
                calendarWeekdayStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.35f);
                calendarWeekdayStyle.alignment = TextAnchor.MiddleCenter;
                calendarDayStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(1.0f));
                calendarDayStyle.normal.textColor = theme.TextColor;
                calendarDayStyle.normal.background = CreateSolidTexture(theme.CardBg);
                calendarDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.white, 0.12f));
                calendarDaySelectedStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDaySelectedStyle.font = customFont;
                calendarDaySelectedStyle.normal.background = CreateSolidTexture(theme.AccentColor);
                calendarDaySelectedStyle.normal.textColor = Color.white;
                calendarDayOutsideMonthStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayOutsideMonthStyle.font = customFont;
                calendarDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.5f);
                calendarDayTodayStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayTodayStyle.font = customFont;
                calendarDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.CardBg, theme.AccentColor);
                calendarDayDisabledStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayDisabledStyle.font = customFont;
                calendarDayDisabledStyle.normal.textColor = theme.MutedColor;
                calendarDayInRangeStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayInRangeStyle.font = customFont;
                calendarDayInRangeStyle.normal.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.55f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCalendarStyles", "StyleManager");
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
                dropdownMenuContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
                dropdownMenuContentStyle.border = GetBorderOffset(6f);
                dropdownMenuContentStyle.padding = GetSpacingOffset(4f, 4f);
                dropdownMenuItemStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f));
                dropdownMenuItemStyle.alignment = TextAnchor.MiddleLeft;
                dropdownMenuItemStyle.normal.background = transparentTexture;
                dropdownMenuItemStyle.normal.textColor = theme.TextColor;
                dropdownMenuItemStyle.hover.background = CreateSolidTexture(theme.AccentColor);
                dropdownMenuItemStyle.hover.textColor = Color.white;
                dropdownMenuItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                dropdownMenuItemStyle.active.textColor = Color.white;
                dropdownMenuItemStyle.padding = GetSpacingOffset(12f, 4f);
                dropdownMenuSeparatorStyle = new UnityHelpers.GUIStyle();
                dropdownMenuSeparatorStyle.normal.background = CreateSolidTexture(theme.SeparatorColor);
                dropdownMenuSeparatorStyle.fixedHeight = 1;
                dropdownMenuSeparatorStyle.margin = GetSpacingOffset(4f, 4f);
                dropdownMenuHeaderStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.75f), FontStyle.Bold);
                dropdownMenuHeaderStyle.normal.textColor = theme.MutedColor;
                dropdownMenuHeaderStyle.padding = GetSpacingOffset(12f, 4f);
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
                popoverContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
                popoverContentStyle.border = GetBorderOffset(6f);
                popoverContentStyle.padding = GetSpacingOffset(12f, 12f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupPopoverStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - ScrollArea
        private void SetupScrollAreaStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                scrollAreaStyle = new UnityHelpers.GUIStyle();
                scrollAreaStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);
                scrollAreaThumbStyle = new UnityHelpers.GUIStyle();
                scrollAreaThumbStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.white, 0.25f));
                scrollAreaThumbStyle.border = GetBorderOffset(4f);
                scrollAreaTrackStyle = new UnityHelpers.GUIStyle();
                scrollAreaTrackStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.black, 0.12f));
                scrollAreaTrackStyle.border = GetBorderOffset(4f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupScrollAreaStyles", "StyleManager");
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
                selectTriggerStyle.normal.background = CreateSolidTexture(theme.InputBg);
                selectTriggerStyle.normal.textColor = theme.TextColor;
                selectTriggerStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.InputBg, theme.AccentColor, 0.12f));
                selectTriggerStyle.active.background = CreateSolidTexture(Color.Lerp(theme.InputBg, theme.AccentColor, 0.25f));
                selectTriggerStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
                selectTriggerStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                selectContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
                selectContentStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle.padding = new UnityHelpers.RectOffset(5, 5, 5, 5);
                selectItemStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(scaledFontSize));
                selectItemStyle.alignment = TextAnchor.MiddleLeft;
                selectItemStyle.normal.background = transparentTexture;
                selectItemStyle.normal.textColor = theme.TextColor;
                selectItemStyle.hover.background = CreateSolidTexture(theme.AccentColor);
                selectItemStyle.hover.textColor = Color.white;
                selectItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.15f));
                selectItemStyle.active.textColor = Color.white;
                selectItemStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSelectStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - DatePicker
        private void SetupDatePickerStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                datePickerStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                datePickerStyle.normal.background = CreateSolidTexture(theme.CardBg);
                datePickerStyle.border = new UnityHelpers.RectOffset(4, 4, 4, 4);
                datePickerStyle.padding = new UnityHelpers.RectOffset(10, 10, 10, 10);
                datePickerHeaderStyle = new UnityHelpers.GUIStyle();
                datePickerHeaderStyle.padding = new UnityHelpers.RectOffset(0, 0, 5, 5);
                datePickerTitleStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 2, FontStyle.Bold);
                datePickerTitleStyle.normal.textColor = theme.TextColor;
                datePickerTitleStyle.alignment = TextAnchor.MiddleCenter;
                datePickerWeekdayStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize - 1);
                datePickerWeekdayStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.35f);
                datePickerWeekdayStyle.alignment = TextAnchor.MiddleCenter;
                datePickerDayStyle = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize);
                datePickerDayStyle.normal.textColor = theme.TextColor;
                datePickerDayStyle.normal.background = CreateSolidTexture(theme.CardBg);
                datePickerDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.white, 0.12f));
                datePickerDaySelectedStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDaySelectedStyle.font = customFont;
                datePickerDaySelectedStyle.normal.background = CreateSolidTexture(theme.AccentColor);
                datePickerDaySelectedStyle.normal.textColor = Color.white;
                datePickerDayOutsideMonthStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayOutsideMonthStyle.font = customFont;
                datePickerDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.5f);
                datePickerDayTodayStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayTodayStyle.font = customFont;
                datePickerDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.CardBg, theme.AccentColor);
                datePickerDayDisabledStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayDisabledStyle.font = customFont;
                datePickerDayDisabledStyle.normal.textColor = theme.MutedColor;
                datePickerDayInRangeStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayInRangeStyle.font = customFont;
                datePickerDayInRangeStyle.normal.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.55f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDatePickerStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Dialog
        private void SetupDialogStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dialogOverlayStyle = new UnityHelpers.GUIStyle();
                dialogOverlayStyle.normal.background = CreateSolidTexture(new Color(0, 0, 0, 0.85f));
                dialogContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                dialogContentStyle.normal.background = CreateRoundedRectWithShadowTexture(512, 512, 12, theme.CardBg, 0.18f, 12);
                dialogContentStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                dialogContentStyle.padding = new UnityHelpers.RectOffset(24, 24, 24, 24);
                dialogTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.25f), FontStyle.Bold);
                dialogTitleStyle.normal.textColor = theme.TextColor;
                dialogTitleStyle.wordWrap = true;
                dialogDescriptionStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f));
                dialogDescriptionStyle.normal.textColor = theme.MutedColor;
                dialogDescriptionStyle.wordWrap = true;
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
                chartContentStyle = new UnityHelpers.GUIStyle();
                chartContentStyle.normal.background = transparentTexture;
                chartContentStyle.padding = GetSpacingOffset(0f, 0f);
                chartContentStyle.margin = GetSpacingOffset(0f, 0f);
                chartAxisStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.75f));
                chartAxisStyle.normal.textColor = theme.MutedColor;
                chartAxisStyle.alignment = TextAnchor.MiddleCenter;
                chartGridStyle = new UnityHelpers.GUIStyle();
                chartGridStyle.normal.background = chartGridTexture;
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
                menuBarItemStyle.normal.textColor = theme.TextColor;
                menuBarItemStyle.hover.background = CreateSolidTexture(theme.ButtonGhostHoverBg);
                menuBarItemStyle.hover.textColor = theme.TextColor;
                menuBarItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.ButtonGhostHoverBg, Color.black, 0.12f));
                menuBarItemStyle.active.textColor = theme.TextColor;
                menuBarItemStyle.stretchWidth = false;
                menuDropdownStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                menuDropdownStyle.normal.background = CreateSolidTexture(theme.CardBg);
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

        #region Style Getters - Public Only
        public GUIStyle GetAnimatedBoxStyle() => animatedBoxStyle ?? GUI.skin.box;

        public GUIStyle GetAnimatedButtonStyle() => animatedButtonStyle ?? GUI.skin.button;

        public GUIStyle GetAnimatedInputStyle() => animatedInputStyle ?? GUI.skin.textField;

        public GUIStyle GetGlowLabelStyle() => glowLabelStyle ?? GUI.skin.label;

        public GUIStyle GetTitleStyle() => titleStyle ?? GUI.skin.label;

        public GUIStyle GetColorPresetStyle() => colorPresetStyle ?? GUI.skin.button;

        public GUIStyle GetSectionHeaderStyle() => sectionHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetCardStyle() => cardStyle ?? GUI.skin.box;

        public GUIStyle GetCardHeaderStyle() => cardHeaderStyle ?? GUIStyle.none;

        public GUIStyle GetCardTitleStyle() => cardTitleStyle ?? GUI.skin.label;

        public GUIStyle GetCardDescriptionStyle() => cardDescriptionStyle ?? GUI.skin.label;

        public GUIStyle GetCardContentStyle() => cardContentStyle ?? GUIStyle.none;

        public GUIStyle GetCardFooterStyle() => cardFooterStyle ?? GUIStyle.none;

        public GUIStyle GetButtonStyle(ButtonVariant variant, ButtonSize size)
        {
            if (buttonStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            var baseStyle = variant switch
            {
                ButtonVariant.Destructive => buttonDestructiveStyle,
                ButtonVariant.Outline => buttonOutlineStyle,
                ButtonVariant.Secondary => buttonSecondaryStyle,
                ButtonVariant.Ghost => buttonGhostStyle,
                ButtonVariant.Link => buttonLinkStyle,
                _ => buttonDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.button);
            ApplyButtonSizing(sizedStyle, size);
            buttonStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyButtonSizing(GUIStyle style, ButtonSize size)
        {
            switch (size)
            {
                case ButtonSize.Small:
                    style.fontSize = GetScaledFontSize(0.75f);
                    style.padding = GetSpacingOffset(12f, 2f);
                    style.fixedHeight = GetScaledHeight(32f);
                    style.border = GetBorderOffset(6f);
                    break;

                case ButtonSize.Large:
                    style.fontSize = GetScaledFontSize(1.0f);
                    style.padding = GetSpacingOffset(24f, 12f);
                    style.fixedHeight = GetScaledHeight(44f);
                    style.border = GetBorderOffset(6f);
                    break;

                case ButtonSize.Icon:
                    style.fontSize = GetScaledFontSize(1.0f);
                    style.padding = GetSpacingOffset(0f, 0f);
                    int iconSize = GetScaledHeight(36f);
                    style.fixedWidth = iconSize;
                    style.fixedHeight = iconSize;
                    style.border = GetBorderOffset(6f);
                    break;
            }
        }

        public GUIStyle GetToggleStyle(ToggleVariant variant, ToggleSize size)
        {
            if (toggleStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant == ToggleVariant.Outline ? toggleOutlineStyle : toggleDefaultStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.button);
            switch (size)
            {
                case ToggleSize.Small:
                    sizedStyle.fontSize = guiHelper.fontSize - 2;
                    sizedStyle.padding = new UnityHelpers.RectOffset(6, 6, 4, 4);
                    break;
                case ToggleSize.Large:
                    sizedStyle.fontSize = guiHelper.fontSize + 2;
                    sizedStyle.padding = new UnityHelpers.RectOffset(10, 10, 8, 8);
                    break;
            }
            toggleStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetInputStyle(InputVariant variant, bool focused = false, bool disabled = false)
        {
            if (inputStyleCache.TryGetValue((variant, focused, disabled), out var cachedStyle))
                return cachedStyle;
            GUIStyle style =
                disabled ? (inputDisabledStyle ?? GUI.skin.textField)
                : focused ? (inputFocusedStyle ?? GUI.skin.textField)
                : variant switch
                {
                    InputVariant.Outline => inputOutlineStyle ?? GUI.skin.textField,
                    InputVariant.Ghost => inputGhostStyle ?? GUI.skin.textField,
                    _ => inputDefaultStyle ?? GUI.skin.textField,
                };
            inputStyleCache[(variant, focused, disabled)] = style;
            return style;
        }

        public GUIStyle GetLabelStyle(LabelVariant variant)
        {
            if (labelStyleCache.TryGetValue(variant, out var cachedStyle))
                return cachedStyle;
            GUIStyle style = variant switch
            {
                LabelVariant.Secondary => labelSecondaryStyle ?? GUI.skin.label,
                LabelVariant.Muted => labelMutedStyle ?? GUI.skin.label,
                LabelVariant.Destructive => labelDestructiveStyle ?? GUI.skin.label,
                _ => labelDefaultStyle ?? GUI.skin.label,
            };
            labelStyleCache[variant] = style;
            return style;
        }

        public GUIStyle GetPasswordFieldStyle() => passwordFieldStyle ?? GUI.skin.textField;

        public GUIStyle GetTextAreaStyle(TextAreaVariant variant = TextAreaVariant.Default, bool focused = false)
        {
            if (textAreaStyleCache.TryGetValue((variant, focused), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                TextAreaVariant.Outline => textAreaOutlineStyle ?? textAreaStyle ?? GUI.skin.textArea,
                TextAreaVariant.Ghost => textAreaGhostStyle ?? textAreaStyle ?? GUI.skin.textArea,
                _ => textAreaStyle ?? GUI.skin.textArea,
            };
            GUIStyle style = focused ? (textAreaFocusedStyle ?? baseStyle) : baseStyle;
            textAreaStyleCache[(variant, focused)] = style;
            return style;
        }

        public GUIStyle GetProgressBarStyle() => progressBarStyle ?? GUI.skin.box;

        public GUIStyle GetProgressBarBackgroundStyle() => progressBarBackgroundStyle ?? GUI.skin.box;

        public GUIStyle GetProgressBarFillStyle() => progressBarFillStyle ?? GUI.skin.box;

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation) => orientation == SeparatorOrientation.Horizontal ? (separatorHorizontalStyle ?? GUI.skin.box) : (separatorVerticalStyle ?? GUI.skin.box);

        public GUIStyle GetTabsListStyle() => tabsListStyle ?? GUI.skin.box;

        public GUIStyle GetTabsTriggerStyle(bool active = false) => active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) : (tabsTriggerStyle ?? GUI.skin.button);

        public GUIStyle GetTabsContentStyle() => tabsContentStyle ?? GUIStyle.none;

        public GUIStyle GetCheckboxStyle(CheckboxVariant variant, CheckboxSize size)
        {
            if (checkboxStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                CheckboxVariant.Outline => checkboxOutlineStyle,
                CheckboxVariant.Ghost => checkboxGhostStyle,
                _ => checkboxDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);
            switch (size)
            {
                case CheckboxSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case CheckboxSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
            }
            checkboxStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetSwitchStyle(SwitchVariant variant, SwitchSize size)
        {
            if (switchStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                SwitchVariant.Outline => switchOutlineStyle,
                SwitchVariant.Ghost => switchGhostStyle,
                _ => switchDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);
            switch (size)
            {
                case SwitchSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case SwitchSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
            }
            switchStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetBadgeStyle(BadgeVariant variant, BadgeSize size)
        {
            if (badgeStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                BadgeVariant.Secondary => badgeSecondaryStyle,
                BadgeVariant.Destructive => badgeDestructiveStyle,
                BadgeVariant.Outline => badgeOutlineStyle,
                _ => badgeDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);
            switch (size)
            {
                case BadgeSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case BadgeSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
            }
            badgeStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetAlertStyle(bool isDestructive = false) => isDestructive ? (alertDestructiveStyle ?? GUI.skin.box) : (alertDefaultStyle ?? GUI.skin.box);

        public GUIStyle GetAlertTitleStyle() => alertTitleStyle ?? GUI.skin.label;

        public GUIStyle GetAlertDescriptionStyle() => alertDescriptionStyle ?? GUI.skin.label;

        public GUIStyle GetAvatarStyle(AvatarSize size, AvatarShape shape)
        {
            if (avatarStyleCache.TryGetValue((size, shape), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(avatarStyle);
            int avatarSizeValue = size switch
            {
                AvatarSize.Small => Mathf.RoundToInt(32 * guiHelper.uiScale),
                AvatarSize.Large => Mathf.RoundToInt(48 * guiHelper.uiScale),
                _ => Mathf.RoundToInt(40 * guiHelper.uiScale),
            };
            sizedStyle.fixedWidth = avatarSizeValue;
            sizedStyle.fixedHeight = avatarSizeValue;
            sizedStyle.border = GetAvatarBorder(shape, size);
            avatarStyleCache[(size, shape)] = sizedStyle;
            return sizedStyle;
        }

        public int GetAvatarFontSize(AvatarSize size) =>
            size switch
            {
                AvatarSize.Small => GetScaledFontSize(0.75f),
                AvatarSize.Large => GetScaledFontSize(1.25f),
                _ => GetScaledFontSize(1.0f),
            };

        public RectOffset GetAvatarBorder(AvatarShape shape, AvatarSize size)
        {
            int borderRadius = shape switch
            {
                AvatarShape.Circle => Mathf.RoundToInt(50 * guiHelper.uiScale),
                AvatarShape.Rounded => Mathf.RoundToInt(8 * guiHelper.uiScale),
                _ => 0,
            };
            return new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }

        public float GetStatusIndicatorSize(AvatarSize size) =>
            size switch
            {
                AvatarSize.Small => 6f,
                AvatarSize.Large => 12f,
                _ => 8f,
            };

        public GUIStyle GetTableStyle(TableVariant variant, TableSize size)
        {
            if (tableStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                TableVariant.Striped => tableStripedStyle,
                TableVariant.Bordered => tableBorderedStyle,
                TableVariant.Hover => tableHoverStyle,
                _ => tableStyle,
            };
            tableStyleCache[(variant, size)] = baseStyle ?? GUI.skin.box;
            return baseStyle ?? GUI.skin.box;
        }

        public GUIStyle GetTableHeaderStyle(TableVariant variant, TableSize size) => tableHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetTableCellStyle(TableVariant variant, TableSize size) => tableCellStyle ?? GUI.skin.label;

        public GUIStyle GetSliderStyle(bool isRange = false, bool isVertical = false, bool isDisabled = false)
        {
            if (isDisabled)
                return sliderDisabledStyle ?? GUI.skin.horizontalSlider;
            if (isRange)
                return sliderRangeStyle ?? GUI.skin.horizontalSlider;
            if (isVertical)
                return sliderVerticalStyle ?? GUI.skin.verticalSlider;
            return sliderDefaultStyle ?? GUI.skin.horizontalSlider;
        }

        public GUIStyle GetSliderLabelStyle() => sliderLabelStyle ?? GUI.skin.label;

        public GUIStyle GetSliderValueStyle() => sliderValueStyle ?? GUI.skin.label;

        public GUIStyle GetCalendarStyle(CalendarVariant variant, CalendarSize size)
        {
            if (calendarStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(calendarStyle);
            switch (size)
            {
                case CalendarSize.Small:
                    sizedStyle.fontSize = guiHelper.fontSize - 2;
                    break;
                case CalendarSize.Large:
                    sizedStyle.fontSize = guiHelper.fontSize + 2;
                    break;
            }
            calendarStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetCalendarHeaderStyle() => calendarHeaderStyle ?? GUIStyle.none;

        public GUIStyle GetCalendarTitleStyle() => calendarTitleStyle ?? GUI.skin.label;

        public GUIStyle GetCalendarWeekdayStyle() => calendarWeekdayStyle ?? GUI.skin.label;

        public GUIStyle GetCalendarDayStyle() => calendarDayStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDaySelectedStyle() => calendarDaySelectedStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayOutsideMonthStyle() => calendarDayOutsideMonthStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayTodayStyle() => calendarDayTodayStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayDisabledStyle() => calendarDayDisabledStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayInRangeStyle() => calendarDayInRangeStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerStyle() => datePickerStyle ?? GUI.skin.box;

        public GUIStyle GetDatePickerHeaderStyle() => datePickerHeaderStyle ?? GUIStyle.none;

        public GUIStyle GetDatePickerTitleStyle() => datePickerTitleStyle ?? GUI.skin.label;

        public GUIStyle GetDatePickerWeekdayStyle() => datePickerWeekdayStyle ?? GUI.skin.label;

        public GUIStyle GetDatePickerDayStyle() => datePickerDayStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDaySelectedStyle() => datePickerDaySelectedStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDayOutsideMonthStyle() => datePickerDayOutsideMonthStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDayTodayStyle() => datePickerDayTodayStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDayDisabledStyle() => datePickerDayDisabledStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDayInRangeStyle() => datePickerDayInRangeStyle ?? GUI.skin.button;

        public GUIStyle GetDialogOverlayStyle() => dialogOverlayStyle ?? GUIStyle.none;

        public GUIStyle GetDialogContentStyle() => dialogContentStyle ?? GUI.skin.box;

        public GUIStyle GetDialogTitleStyle() => dialogTitleStyle ?? GUI.skin.label;

        public GUIStyle GetDialogDescriptionStyle() => dialogDescriptionStyle ?? GUI.skin.label;

        public GUIStyle GetDropdownMenuContentStyle() => dropdownMenuContentStyle ?? GUI.skin.box;

        public GUIStyle GetDropdownMenuItemStyle() => dropdownMenuItemStyle ?? GUI.skin.button;

        public GUIStyle GetDropdownMenuSeparatorStyle() => dropdownMenuSeparatorStyle ?? GUIStyle.none;

        public GUIStyle GetDropdownMenuHeaderStyle() => dropdownMenuHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetDropdownMenuStyle(DropdownMenuVariant variant, DropdownMenuSize size)
        {
            if (dropdownMenuStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            dropdownMenuStyleCache[(variant, size)] = dropdownMenuContentStyle;
            return dropdownMenuContentStyle;
        }

        public GUIStyle GetPopoverContentStyle() => popoverContentStyle ?? GUI.skin.box;

        public GUIStyle GetScrollAreaStyle(ScrollAreaVariant variant, ScrollAreaSize size)
        {
            if (scrollAreaStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(scrollAreaStyle);
            switch (size)
            {
                case ScrollAreaSize.Small:
                    sizedStyle.fixedWidth = 10;
                    break;
                case ScrollAreaSize.Large:
                    sizedStyle.fixedWidth = 20;
                    break;
            }
            scrollAreaStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetScrollAreaThumbStyle() => scrollAreaThumbStyle ?? GUIStyle.none;

        public GUIStyle GetScrollAreaTrackStyle() => scrollAreaTrackStyle ?? GUIStyle.none;

        public GUIStyle GetSelectTriggerStyle() => selectTriggerStyle ?? GUI.skin.button;

        public GUIStyle GetSelectContentStyle() => selectContentStyle ?? GUI.skin.box;

        public GUIStyle GetSelectItemStyle() => selectItemStyle ?? GUI.skin.button;

        public GUIStyle GetSelectStyle(SelectVariant variant, SelectSize size)
        {
            if (selectStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(selectContentStyle);
            switch (size)
            {
                case SelectSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case SelectSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
            }
            selectStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetChartContainerStyle() => chartContainerStyle ?? GUI.skin.box;

        public GUIStyle GetChartContentStyle() => chartContentStyle ?? GUIStyle.none;

        public GUIStyle GetChartAxisStyle() => chartAxisStyle ?? GUI.skin.label;

        public GUIStyle GetChartGridStyle() => chartGridStyle ?? GUIStyle.none;

        public GUIStyle GetChartStyle(ChartVariant variant, ChartSize size)
        {
            if (chartStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(chartContainerStyle);
            switch (size)
            {
                case ChartSize.Small:
                    sizedStyle.padding = GetSpacingOffset(12f, 12f);
                    break;
                case ChartSize.Large:
                    sizedStyle.padding = GetSpacingOffset(24f, 24f);
                    break;
            }
            chartStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetMenuBarStyle() => menuBarStyle ?? GUIStyle.none;

        public GUIStyle GetMenuBarItemStyle() => menuBarItemStyle ?? GUI.skin.button;

        public GUIStyle GetMenuDropdownStyle() => menuDropdownStyle ?? GUI.skin.box;

        public Texture2D GetGlowTexture() => glowTexture;

        public Texture2D GetParticleTexture() => particleTexture;

        public Texture2D GetInputBackgroundTexture() => inputBackgroundTexture;

        public Texture2D GetInputFocusedTexture() => inputFocusedTexture;

        public Texture2D GetTransparentTexture() => transparentTexture;

        public Texture2D GetProgressBarBackgroundTexture() => progressBarBackgroundTexture;

        public Texture2D GetProgressBarFillTexture() => progressBarFillTexture;
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
    }
}
