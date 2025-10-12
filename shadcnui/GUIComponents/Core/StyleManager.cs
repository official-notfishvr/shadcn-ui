using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER
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

    /// <summary>
    /// Manages the GUI styles for the shadcnui components with proper error handling and caching.
    /// Provides a comprehensive styling system based on shadcn/ui design.
    /// </summary>
    public class StyleManager
    {
        #region Private Fields
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
        private Texture2D cardShadowTexture;
        private Texture2D buttonShadowTexture;
        private Texture2D popoverShadowTexture;
        private Texture2D dialogShadowTexture;
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

        #region GUIStyle Fields

        #region GUIStyle Fields - Animated
        public GUIStyle animatedBoxStyle;
        public GUIStyle animatedButtonStyle;
        public GUIStyle animatedInputStyle;
        public GUIStyle glowLabelStyle;
        public GUIStyle titleStyle;
        public GUIStyle colorPresetStyle;
        public GUIStyle sectionHeaderStyle;
        #endregion

        #region GUIStyle Fields - Card
        public GUIStyle cardStyle;
        public GUIStyle cardHeaderStyle;
        public GUIStyle cardTitleStyle;
        public GUIStyle cardDescriptionStyle;
        public GUIStyle cardContentStyle;
        public GUIStyle cardFooterStyle;
        #endregion

        #region GUIStyle Fields - Button
        public GUIStyle buttonDefaultStyle;
        public GUIStyle buttonDestructiveStyle;
        public GUIStyle buttonOutlineStyle;
        public GUIStyle buttonSecondaryStyle;
        public GUIStyle buttonGhostStyle;
        public GUIStyle buttonLinkStyle;
        public GUIStyle buttonIconStyle;
        #endregion

        #region GUIStyle Fields - Toggle
        public GUIStyle toggleDefaultStyle;
        public GUIStyle toggleOutlineStyle;
        #endregion

        #region GUIStyle Fields - Input
        public GUIStyle inputDefaultStyle;
        public GUIStyle inputOutlineStyle;
        public GUIStyle inputGhostStyle;
        public GUIStyle inputFocusedStyle;
        public GUIStyle inputDisabledStyle;
        public GUIStyle passwordFieldStyle;
        #endregion

        #region GUIStyle Fields - Label
        public GUIStyle labelDefaultStyle;
        public GUIStyle labelSecondaryStyle;
        public GUIStyle labelMutedStyle;
        public GUIStyle labelDestructiveStyle;
        #endregion

        #region GUIStyle Fields - TextArea
        public GUIStyle textAreaStyle;
        public GUIStyle textAreaFocusedStyle;
        public GUIStyle textAreaOutlineStyle;
        public GUIStyle textAreaGhostStyle;
        #endregion

        #region GUIStyle Fields - ProgressBar
        public GUIStyle progressBarStyle;
        public GUIStyle progressBarBackgroundStyle;
        public GUIStyle progressBarFillStyle;
        #endregion

        #region GUIStyle Fields - Separator
        public GUIStyle separatorHorizontalStyle;
        public GUIStyle separatorVerticalStyle;
        #endregion

        #region GUIStyle Fields - Tabs
        public GUIStyle tabsListStyle;
        public GUIStyle tabsTriggerStyle;
        public GUIStyle tabsTriggerActiveStyle;
        public GUIStyle tabsContentStyle;
        #endregion

        #region GUIStyle Fields - Checkbox
        public GUIStyle checkboxDefaultStyle;
        public GUIStyle checkboxOutlineStyle;
        public GUIStyle checkboxGhostStyle;
        #endregion

        #region GUIStyle Fields - Switch
        public GUIStyle switchDefaultStyle;
        public GUIStyle switchOutlineStyle;
        public GUIStyle switchGhostStyle;
        #endregion

        #region GUIStyle Fields - Badge
        public GUIStyle badgeDefaultStyle;
        public GUIStyle badgeSecondaryStyle;
        public GUIStyle badgeDestructiveStyle;
        public GUIStyle badgeOutlineStyle;
        #endregion

        #region GUIStyle Fields - Alert
        public GUIStyle alertDefaultStyle;
        public GUIStyle alertDestructiveStyle;
        public GUIStyle alertTitleStyle;
        public GUIStyle alertDescriptionStyle;
        #endregion

        #region GUIStyle Fields - Avatar
        public GUIStyle avatarStyle;
        #endregion

        #region GUIStyle Fields - Table
        public GUIStyle tableStyle;
        public GUIStyle tableHeaderStyle;
        public GUIStyle tableCellStyle;
        public GUIStyle tableStripedStyle;
        public GUIStyle tableBorderedStyle;
        public GUIStyle tableHoverStyle;
        #endregion

        #region GUIStyle Fields - Slider
        public GUIStyle sliderDefaultStyle;
        public GUIStyle sliderRangeStyle;
        public GUIStyle sliderVerticalStyle;
        public GUIStyle sliderDisabledStyle;
        public GUIStyle sliderLabelStyle;
        public GUIStyle sliderValueStyle;
        #endregion

        #region GUIStyle Fields - Calendar
        public GUIStyle calendarStyle;
        public GUIStyle calendarHeaderStyle;
        public GUIStyle calendarTitleStyle;
        public GUIStyle calendarWeekdayStyle;
        public GUIStyle calendarDayStyle;
        public GUIStyle calendarDaySelectedStyle;
        public GUIStyle calendarDayOutsideMonthStyle;
        public GUIStyle calendarDayTodayStyle;
        public GUIStyle calendarDayDisabledStyle;
        public GUIStyle calendarDayInRangeStyle;
        #endregion

        #region GUIStyle Fields - DatePicker
        public GUIStyle datePickerStyle;
        public GUIStyle datePickerHeaderStyle;
        public GUIStyle datePickerTitleStyle;
        public GUIStyle datePickerWeekdayStyle;
        public GUIStyle datePickerDayStyle;
        public GUIStyle datePickerDaySelectedStyle;
        public GUIStyle datePickerDayOutsideMonthStyle;
        public GUIStyle datePickerDayTodayStyle;
        public GUIStyle datePickerDayDisabledStyle;
        public GUIStyle datePickerDayInRangeStyle;
        #endregion

        #region GUIStyle Fields - Dialog
        public GUIStyle dialogOverlayStyle;
        public GUIStyle dialogContentStyle;
        public GUIStyle dialogTitleStyle;
        public GUIStyle dialogDescriptionStyle;
        #endregion

        #region GUIStyle Fields - DropdownMenu
        public GUIStyle dropdownMenuContentStyle;
        public GUIStyle dropdownMenuItemStyle;
        public GUIStyle dropdownMenuSeparatorStyle;
        public GUIStyle dropdownMenuHeaderStyle;
        #endregion

        #region GUIStyle Fields - Popover
        public GUIStyle popoverContentStyle;
        #endregion

        #region GUIStyle Fields - ScrollArea
        public GUIStyle scrollAreaStyle;
        public GUIStyle scrollAreaThumbStyle;
        public GUIStyle scrollAreaTrackStyle;
        #endregion

        #region GUIStyle Fields - Select
        public GUIStyle selectTriggerStyle;
        public GUIStyle selectContentStyle;
        public GUIStyle selectItemStyle;
        #endregion

        #region GUIStyle Fields - Chart
        public GUIStyle chartContainerStyle;
        public GUIStyle chartContentStyle;
        public GUIStyle chartAxisStyle;
        public GUIStyle chartGridStyle;
        #endregion

        #region GUIStyle Fields - MenuBar
        public GUIStyle menuBarStyle;
        public GUIStyle menuBarItemStyle;
        public GUIStyle menuDropdownStyle;
        #endregion


        #endregion

        #region Style Cache Dictionaries
        private Dictionary<(ButtonVariant, ButtonSize), GUIStyle> buttonStyleCache = new Dictionary<(ButtonVariant, ButtonSize), GUIStyle>();
        private Dictionary<(ToggleVariant, ToggleSize), GUIStyle> toggleStyleCache = new Dictionary<(ToggleVariant, ToggleSize), GUIStyle>();
        private Dictionary<(InputVariant, bool, bool), GUIStyle> inputStyleCache = new Dictionary<(InputVariant, bool, bool), GUIStyle>();
        private Dictionary<LabelVariant, GUIStyle> labelStyleCache = new Dictionary<LabelVariant, GUIStyle>();
        private Dictionary<(TextAreaVariant, bool), GUIStyle> textAreaStyleCache = new Dictionary<(TextAreaVariant, bool), GUIStyle>();
        private Dictionary<(CheckboxVariant, CheckboxSize), GUIStyle> checkboxStyleCache = new Dictionary<(CheckboxVariant, CheckboxSize), GUIStyle>();
        private Dictionary<(SwitchVariant, SwitchSize), GUIStyle> switchStyleCache = new Dictionary<(SwitchVariant, SwitchSize), GUIStyle>();
        private Dictionary<(BadgeVariant, BadgeSize), GUIStyle> badgeStyleCache = new Dictionary<(BadgeVariant, BadgeSize), GUIStyle>();
        private Dictionary<(AvatarSize, AvatarShape), GUIStyle> avatarStyleCache = new Dictionary<(AvatarSize, AvatarShape), GUIStyle>();
        private Dictionary<(TableVariant, TableSize), GUIStyle> tableStyleCache = new Dictionary<(TableVariant, TableSize), GUIStyle>();
        private Dictionary<(CalendarVariant, CalendarSize), GUIStyle> calendarStyleCache = new Dictionary<(CalendarVariant, CalendarSize), GUIStyle>();
        private Dictionary<(DropdownMenuVariant, DropdownMenuSize), GUIStyle> dropdownMenuStyleCache = new Dictionary<(DropdownMenuVariant, DropdownMenuSize), GUIStyle>();
        private Dictionary<(ScrollAreaVariant, ScrollAreaSize), GUIStyle> scrollAreaStyleCache = new Dictionary<(ScrollAreaVariant, ScrollAreaSize), GUIStyle>();
        private Dictionary<(SelectVariant, SelectSize), GUIStyle> selectStyleCache = new Dictionary<(SelectVariant, SelectSize), GUIStyle>();
        private Dictionary<(ChartVariant, ChartSize), GUIStyle> chartStyleCache = new Dictionary<(ChartVariant, ChartSize), GUIStyle>();
        #endregion

        #region Texture Cache Dictionaries
        private Dictionary<Color, Texture2D> solidColorTextureCache = new Dictionary<Color, Texture2D>();
        private Dictionary<(Color, Color), Texture2D> outlineButtonTextureCache = new Dictionary<(Color, Color), Texture2D>();
        private Dictionary<Color, Texture2D> outlineTextureCache = new Dictionary<Color, Texture2D>();
        #endregion

        #region Core
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

        public Theme GetTheme()
        {
            try
            {
                return ThemeManager.Instance.CurrentTheme;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "GetTheme", "StyleManager");
                return null;
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

#if IL2CPP_MELONLOADER
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

                isInitialized = true;
                GUILogger.LogInfo("StyleManager initialization completed successfully", "StyleManager.InitializeGUI");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "InitializeGUI", "StyleManager");
                isInitialized = false;
            }
        }
        #endregion

        #region Design Helper Methods
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

        public Color GetHoverColor(Color baseColor, bool isDark = true)
        {
            return isDark ? Color.Lerp(baseColor, Color.white, 0.08f) : Color.Lerp(baseColor, Color.black, 0.05f);
        }

        public Color GetFocusColor(Color baseColor)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return Color.Lerp(baseColor, theme.AccentColor, 0.15f);
        }
        #endregion

        #region Texture Creation Methods
        public Texture2D CreateBorderTexture(Color borderColor, int thickness)
        {
            int size = thickness * 2 + 2;
            var texture = new Texture2D(size, size);
            Color fillColor = Color.clear;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (x < thickness || y < thickness || x >= size - thickness || y >= size - thickness)
                        texture.SetPixel(x, y, borderColor);
                    else
                        texture.SetPixel(x, y, fillColor);
                }
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateGlowTexture(Color glowColor, int size)
        {
            var texture = new Texture2D(size, size);
            Vector2 center = new Vector2(size / 2f, size / 2f);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    float alpha = Mathf.Clamp01(1f - (distance / (size / 2f)));
                    alpha = Mathf.Pow(alpha, 2f);
                    texture.SetPixel(x, y, new Color(glowColor.r, glowColor.g, glowColor.b, alpha * 0.3f));
                }
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateShadowTexture(int width, int height, float intensity = 0.15f, float blur = 2f)
        {
            var texture = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float edgeDistanceX = Mathf.Min(x, width - x - 1);
                    float edgeDistanceY = Mathf.Min(y, height - y - 1);
                    float edgeDistance = Mathf.Min(edgeDistanceX, edgeDistanceY);

                    float alpha = Mathf.Clamp01(edgeDistance / blur);
                    alpha = 1f - alpha;
                    alpha *= intensity;

                    texture.SetPixel(x, y, new Color(0f, 0f, 0f, alpha));
                }
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateDropShadowTexture(int size, Vector2 offset, float blur = 3f, float intensity = 0.1f)
        {
            var texture = new Texture2D(size + (int)Mathf.Abs(offset.x) + (int)blur * 2, size + (int)Mathf.Abs(offset.y) + (int)blur * 2);
            Vector2 shadowCenter = new Vector2(texture.width / 2f + offset.x, texture.height / 2f + offset.y);

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Vector2 pos = new Vector2(x, y);
                    float distance = Vector2.Distance(pos, shadowCenter);
                    float alpha = Mathf.Clamp01(1f - (distance / (size / 2f + blur)));
                    alpha = Mathf.Pow(alpha, 1.5f) * intensity;
                    texture.SetPixel(x, y, new Color(0f, 0f, 0f, alpha));
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
                    if (x < radius && y < radius)
                    {
                        if (Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius)) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }
                    else if (x > width - radius && y < radius)
                    {
                        if (Vector2.Distance(new Vector2(x, y), new Vector2(width - radius, radius)) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }
                    else if (x < radius && y > height - radius)
                    {
                        if (Vector2.Distance(new Vector2(x, y), new Vector2(radius, height - radius)) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }
                    else if (x > width - radius && y > height - radius)
                    {
                        if (Vector2.Distance(new Vector2(x, y), new Vector2(width - radius, height - radius)) > radius)
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

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor, Color borderColor)
        {
            if (outlineButtonTextureCache.TryGetValue((backgroundColor, borderColor), out var cachedTexture))
                return cachedTexture;

            Texture2D texture = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (x == 0 || y == 0 || x == 3 || y == 3)
                        texture.SetPixel(x, y, borderColor);
                    else
                        texture.SetPixel(x, y, backgroundColor);
                }
            }
            texture.Apply();
            outlineButtonTextureCache[(backgroundColor, borderColor)] = texture;
            return texture;
        }

        private Texture2D CreateOutlineTexture()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            if (outlineTextureCache.TryGetValue(theme.AccentColor, out var cachedTexture))
                return cachedTexture;

            Texture2D texture = new Texture2D(4, 4);
            Color borderColor = theme.AccentColor;
            Color fillColor = new Color(0f, 0f, 0f, 0f);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (x == 0 || y == 0 || x == 3 || y == 3)
                        texture.SetPixel(x, y, borderColor);
                    else
                        texture.SetPixel(x, y, fillColor);
                }
            }
            texture.Apply();
            outlineTextureCache[theme.AccentColor] = texture;
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
                    Color gradColor = Color.Lerp(theme.PrimaryColor, theme.SecondaryColor, t);
                    gradientTexture.SetPixel(0, i, gradColor);
                }
                gradientTexture.Apply();

                cardBackgroundTexture = CreateRoundedRectTexture(128, 128, 12, theme.CardBg);
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
                dropdownMenuContentTexture = CreateRoundedRectTexture(128, 128, 8, theme.CardBg);
                popoverContentTexture = CreateRoundedRectTexture(128, 128, 8, theme.CardBg);
                scrollAreaThumbTexture = CreateRoundedRectTexture(8, 8, 4, theme.ButtonOutlineBorder);
                scrollAreaTrackTexture = CreateRoundedRectTexture(8, 8, 4, theme.SecondaryColor);
                selectTriggerTexture = CreateRoundedRectTexture(128, 40, 8, theme.InputBg);
                selectContentTexture = CreateRoundedRectTexture(128, 128, 8, theme.CardBg);
                chartContainerTexture = CreateRoundedRectTexture(256, 256, 8, theme.ChartBg);
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

        #region Style Setup Methods

        #region Style Setup Methods - Animated
        private void SetupAnimatedStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                animatedBoxStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                animatedBoxStyle.normal.background = gradientTexture;
                animatedBoxStyle.border = new UnityHelpers.RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, 5);
                animatedBoxStyle.padding = new UnityHelpers.RectOffset(15, 15, 15, 15);

                animatedButtonStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    animatedButtonStyle.font = customFont;
                animatedButtonStyle.fontSize = guiHelper.fontSize;
                animatedButtonStyle.fontStyle = FontStyle.Bold;
                animatedButtonStyle.alignment = TextAnchor.MiddleCenter;
                animatedButtonStyle.normal.textColor = Color.Lerp(Color.white, theme.AccentColor, 0.3f);
                animatedButtonStyle.hover.textColor = theme.AccentColor;

                colorPresetStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    colorPresetStyle.font = customFont;
                colorPresetStyle.fontSize = Mathf.RoundToInt(guiHelper.fontSize * 0.9f);
                colorPresetStyle.fontStyle = FontStyle.Bold;
                colorPresetStyle.alignment = TextAnchor.MiddleCenter;

                animatedInputStyle = new UnityHelpers.GUIStyle(GUI.skin.textField);
                if (customFont != null)
                    animatedInputStyle.font = customFont;
                animatedInputStyle.fontSize = guiHelper.fontSize + 1;
                animatedInputStyle.padding = new UnityHelpers.RectOffset(8, 8, 4, 4);
                animatedInputStyle.normal.textColor = theme.TextColor;
                animatedInputStyle.focused.textColor = theme.AccentColor;

                glowLabelStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    glowLabelStyle.font = customFont;
                glowLabelStyle.fontSize = guiHelper.fontSize;
                glowLabelStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.AccentColor, 0.2f);

                titleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    titleStyle.font = customFont;
                titleStyle.fontSize = guiHelper.fontSize + 4;
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.alignment = TextAnchor.MiddleCenter;
                titleStyle.normal.textColor = theme.AccentColor;

                sectionHeaderStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    sectionHeaderStyle.font = customFont;
                sectionHeaderStyle.fontSize = guiHelper.fontSize + 2;
                sectionHeaderStyle.fontStyle = FontStyle.Bold;
                sectionHeaderStyle.normal.textColor = Color.Lerp(theme.AccentColor, theme.TextColor, 0.3f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAnimatedStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - Card
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

                cardTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    cardTitleStyle.font = customFont;
                cardTitleStyle.fontSize = GetScaledFontSize(1.5f);
                cardTitleStyle.fontStyle = FontStyle.Bold;
                cardTitleStyle.normal.textColor = theme.CardTitle;
                cardTitleStyle.wordWrap = true;

                cardDescriptionStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    cardDescriptionStyle.font = customFont;
                cardDescriptionStyle.fontSize = GetScaledFontSize(1.0f);
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

        #region Style Setup Methods - Button
        private GUIStyle CreateBaseButtonStyle()
        {
            GUIStyle style = new UnityHelpers.GUIStyle(GUI.skin.button);
            style.wordWrap = false;
            style.clipping = TextClipping.Clip;
            return style;
        }

        private void SetupButtonVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                buttonDefaultStyle = CreateBaseButtonStyle();
                if (customFont != null)
                    buttonDefaultStyle.font = customFont;
                buttonDefaultStyle.fontSize = GetScaledFontSize(0.875f);
                buttonDefaultStyle.fontStyle = FontStyle.Normal;
                buttonDefaultStyle.alignment = TextAnchor.MiddleCenter;
                buttonDefaultStyle.padding = GetSpacingOffset(16f, 8f);
                buttonDefaultStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                buttonDefaultStyle.fixedHeight = GetScaledHeight(40f);
                buttonDefaultStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonPrimaryBg);
                buttonDefaultStyle.hover.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonPrimaryBg, Color.white, 0.08f));
                buttonDefaultStyle.normal.textColor = theme.ButtonPrimaryFg;
                buttonDefaultStyle.hover.textColor = theme.ButtonPrimaryFg;
                buttonDefaultStyle.active.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonPrimaryActiveBg);
                buttonDefaultStyle.active.textColor = theme.ButtonPrimaryActiveFg;
                buttonDefaultStyle.focused.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonPrimaryBg, theme.AccentColor, 0.1f));
                buttonDefaultStyle.focused.textColor = theme.ButtonPrimaryFg;

                buttonDestructiveStyle = CreateBaseButtonStyle();
                if (customFont != null)
                    buttonDestructiveStyle.font = customFont;
                buttonDestructiveStyle.fontSize = GetScaledFontSize(0.875f);
                buttonDestructiveStyle.fontStyle = FontStyle.Normal;
                buttonDestructiveStyle.alignment = TextAnchor.MiddleCenter;
                buttonDestructiveStyle.padding = GetSpacingOffset(16f, 8f);
                buttonDestructiveStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                buttonDestructiveStyle.fixedHeight = GetScaledHeight(40f);
                buttonDestructiveStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonDestructiveBg);
                buttonDestructiveStyle.hover.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.1f));
                buttonDestructiveStyle.normal.textColor = theme.ButtonDestructiveFg;
                buttonDestructiveStyle.hover.textColor = theme.ButtonDestructiveFg;
                buttonDestructiveStyle.active.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.2f));
                buttonDestructiveStyle.active.textColor = theme.ButtonDestructiveFg;

                buttonOutlineStyle = CreateBaseButtonStyle();
                if (customFont != null)
                    buttonOutlineStyle.font = customFont;
                buttonOutlineStyle.fontSize = GetScaledFontSize(0.875f);
                buttonOutlineStyle.fontStyle = FontStyle.Normal;
                buttonOutlineStyle.alignment = TextAnchor.MiddleCenter;
                buttonOutlineStyle.padding = GetSpacingOffset(16f, 8f);
                buttonOutlineStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                buttonOutlineStyle.fixedHeight = GetScaledHeight(40f);
                buttonOutlineStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonOutlineBorder);
                buttonOutlineStyle.hover.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonOutlineHoverBg);
                buttonOutlineStyle.normal.textColor = theme.ButtonOutlineFg;
                buttonOutlineStyle.hover.textColor = theme.ButtonOutlineHoverFg;
                buttonOutlineStyle.active.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonOutlineHoverBg, Color.black, 0.05f));
                buttonOutlineStyle.active.textColor = theme.ButtonOutlineHoverFg;
                buttonOutlineStyle.focused.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonOutlineHoverBg, theme.AccentColor, 0.15f));
                buttonOutlineStyle.focused.textColor = theme.ButtonOutlineHoverFg;

                buttonSecondaryStyle = CreateBaseButtonStyle();
                if (customFont != null)
                    buttonSecondaryStyle.font = customFont;
                buttonSecondaryStyle.fontSize = GetScaledFontSize(0.875f);
                buttonSecondaryStyle.fontStyle = FontStyle.Normal;
                buttonSecondaryStyle.alignment = TextAnchor.MiddleCenter;
                buttonSecondaryStyle.padding = GetSpacingOffset(16f, 8f);
                buttonSecondaryStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                buttonSecondaryStyle.fixedHeight = GetScaledHeight(40f);
                buttonSecondaryStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonSecondaryBg);
                buttonSecondaryStyle.hover.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonSecondaryBg, Color.white, 0.05f));
                buttonSecondaryStyle.normal.textColor = theme.ButtonSecondaryFg;
                buttonSecondaryStyle.hover.textColor = theme.ButtonSecondaryFg;
                buttonSecondaryStyle.active.background = CreateRoundedRectTexture(128, 40, 8, Color.Lerp(theme.ButtonSecondaryBg, Color.black, 0.05f));
                buttonSecondaryStyle.active.textColor = theme.ButtonSecondaryFg;

                buttonGhostStyle = CreateBaseButtonStyle();
                if (customFont != null)
                    buttonGhostStyle.font = customFont;
                buttonGhostStyle.fontSize = GetScaledFontSize(0.875f);
                buttonGhostStyle.fontStyle = FontStyle.Normal;
                buttonGhostStyle.alignment = TextAnchor.MiddleCenter;
                buttonGhostStyle.padding = GetSpacingOffset(16f, 8f);
                buttonGhostStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                buttonGhostStyle.fixedHeight = GetScaledHeight(40f);
                buttonGhostStyle.normal.background = transparentTexture;
                buttonGhostStyle.hover.background = CreateRoundedRectTexture(128, 40, 8, theme.ButtonGhostHoverBg);
                buttonGhostStyle.normal.textColor = theme.ButtonGhostFg;
                buttonGhostStyle.hover.textColor = theme.ButtonGhostHoverFg;
                buttonGhostStyle.active.background = CreateRoundedRectTexture(128, 40, 8, GetHoverColor(theme.ButtonGhostHoverBg, true));
                buttonGhostStyle.active.textColor = theme.ButtonGhostHoverFg;
                buttonGhostStyle.focused.background = CreateRoundedRectTexture(128, 40, 8, GetFocusColor(theme.ButtonGhostHoverBg));
                buttonGhostStyle.focused.textColor = theme.ButtonGhostHoverFg;

                buttonLinkStyle = CreateBaseButtonStyle();
                if (customFont != null)
                    buttonLinkStyle.font = customFont;
                buttonLinkStyle.fontSize = GetScaledFontSize(0.875f);
                buttonLinkStyle.fontStyle = FontStyle.Normal;
                buttonLinkStyle.alignment = TextAnchor.MiddleCenter;
                buttonLinkStyle.padding = GetSpacingOffset(0f, 2f);
                buttonLinkStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                buttonLinkStyle.normal.background = transparentTexture;
                buttonLinkStyle.hover.background = transparentTexture;
                buttonLinkStyle.normal.textColor = theme.ButtonLinkColor;
                buttonLinkStyle.hover.textColor = theme.ButtonLinkHoverColor;
                buttonLinkStyle.active.background = transparentTexture;
                buttonLinkStyle.active.textColor = Color.Lerp(theme.ButtonLinkHoverColor, Color.black, 0.2f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupButtonVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - Toggle
        private void SetupToggleVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                toggleDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    toggleDefaultStyle.font = customFont;
                toggleDefaultStyle.fontSize = guiHelper.fontSize;
                toggleDefaultStyle.fontStyle = FontStyle.Normal;
                toggleDefaultStyle.alignment = TextAnchor.MiddleCenter;
                toggleDefaultStyle.normal.textColor = theme.ToggleFg;
                toggleDefaultStyle.normal.background = CreateSolidTexture(theme.ToggleBg);
                toggleDefaultStyle.hover.background = CreateSolidTexture(theme.ToggleHoverBg);
                toggleDefaultStyle.hover.textColor = theme.ToggleHoverFg;
                toggleDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.ToggleBg, Color.black, 0.1f));
                toggleDefaultStyle.active.textColor = Color.Lerp(theme.ToggleFg, Color.white, 0.5f);

                Color onBgColor = theme.ToggleOnBg;
                Color onHoverBgColor = Color.Lerp(onBgColor, Color.white, 0.1f);
                Color onActiveBgColor = Color.Lerp(onBgColor, Color.black, 0.1f);

                toggleDefaultStyle.onNormal.background = CreateSolidTexture(onBgColor);
                toggleDefaultStyle.onNormal.textColor = theme.ToggleOnFg;
                toggleDefaultStyle.onHover.background = CreateSolidTexture(onHoverBgColor);
                toggleDefaultStyle.onHover.textColor = theme.ToggleOnFg;
                toggleDefaultStyle.onActive.background = CreateSolidTexture(onActiveBgColor);
                toggleDefaultStyle.onActive.textColor = theme.ToggleOnFg;

                toggleOutlineStyle = new UnityHelpers.GUIStyle(toggleDefaultStyle);
                if (customFont != null)
                    toggleOutlineStyle.font = customFont;
                toggleOutlineStyle.normal.background = CreateOutlineButtonTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f), theme.AccentColor);
                toggleOutlineStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);
                toggleOutlineStyle.hover.background = CreateSolidTexture(theme.ToggleHoverBg);
                toggleOutlineStyle.hover.textColor = theme.ToggleHoverFg;
                toggleOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(theme.ToggleBg, Color.black, 0.1f));
                toggleOutlineStyle.active.textColor = Color.Lerp(theme.ToggleFg, Color.white, 0.5f);

                Color onOutlineBgColor = theme.ToggleOnBg;
                Color onOutlineHoverBgColor = Color.Lerp(onOutlineBgColor, Color.white, 0.1f);
                Color onOutlineActiveBgColor = Color.Lerp(onOutlineBgColor, Color.black, 0.1f);

                toggleOutlineStyle.onNormal.background = CreateOutlineButtonTexture(onOutlineBgColor, onOutlineBgColor);
                toggleOutlineStyle.onNormal.textColor = theme.ToggleOnFg;
                toggleOutlineStyle.onHover.background = CreateSolidTexture(onOutlineHoverBgColor);
                toggleOutlineStyle.onHover.textColor = theme.ToggleOnFg;
                toggleOutlineStyle.onActive.background = CreateSolidTexture(onOutlineActiveBgColor);
                toggleOutlineStyle.onActive.textColor = theme.ToggleOnFg;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupToggleVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - Input
        private void SetupInputVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                inputDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.textField);
                if (customFont != null)
                    inputDefaultStyle.font = customFont;
                inputDefaultStyle.fontSize = GetScaledFontSize(0.875f);
                inputDefaultStyle.padding = GetSpacingOffset(12f, 8f);
                inputDefaultStyle.margin = GetSpacingOffset(0f, 4f);
                inputDefaultStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                inputDefaultStyle.fixedHeight = GetScaledHeight(40f);
                inputDefaultStyle.normal.background = inputBackgroundTexture;
                inputDefaultStyle.normal.textColor = theme.InputFg;
                inputDefaultStyle.hover.background = inputBackgroundTexture;
                inputDefaultStyle.hover.textColor = theme.InputFg;
                inputDefaultStyle.focused.background = inputFocusedTexture;
                inputDefaultStyle.focused.textColor = theme.InputFocusedFg;
                inputDefaultStyle.onFocused.background = CreateRoundedRectTexture(128, 40, 8, theme.AccentColor);

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

                passwordFieldStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    passwordFieldStyle.font = customFont;
                passwordFieldStyle.fontSize = guiHelper.fontSize + 2;

                textAreaStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    textAreaStyle.font = customFont;
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
        #endregion

        #region Style Setup Methods - Label
        private void SetupLabelVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                labelDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    labelDefaultStyle.font = customFont;
                labelDefaultStyle.fontSize = GetScaledFontSize(1.0f);
                labelDefaultStyle.fontStyle = FontStyle.Normal;
                labelDefaultStyle.normal.textColor = theme.TextColor;
                labelDefaultStyle.padding = GetSpacingOffset(0f, 2f);
                labelDefaultStyle.wordWrap = true;

                labelSecondaryStyle = new UnityHelpers.GUIStyle(labelDefaultStyle);
                if (customFont != null)
                    labelSecondaryStyle.font = customFont;
                labelSecondaryStyle.fontSize = GetScaledFontSize(0.875f);
                labelSecondaryStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.MutedColor, 0.3f);

                labelMutedStyle = new UnityHelpers.GUIStyle(labelDefaultStyle);
                if (customFont != null)
                    labelMutedStyle.font = customFont;
                labelMutedStyle.fontSize = GetScaledFontSize(0.875f);
                labelMutedStyle.normal.textColor = theme.MutedColor;

                labelDestructiveStyle = new UnityHelpers.GUIStyle(labelDefaultStyle);
                if (customFont != null)
                    labelDestructiveStyle.font = customFont;
                labelDestructiveStyle.fontSize = GetScaledFontSize(1.0f);
                labelDestructiveStyle.normal.textColor = theme.ButtonDestructiveBg;
                labelDestructiveStyle.fontStyle = FontStyle.Bold;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupLabelVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - ProgressBar, Separator, Tabs, TextArea
        private void SetupProgressBarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

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

        private void SetupSeparatorStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                separatorHorizontalStyle = new UnityHelpers.GUIStyle();
                separatorHorizontalStyle.normal.background = CreateSolidTexture(theme.SeparatorColor);
                separatorHorizontalStyle.fixedHeight = Mathf.RoundToInt(1 * guiHelper.uiScale);
                separatorHorizontalStyle.stretchWidth = true;

                separatorVerticalStyle = new UnityHelpers.GUIStyle();
                separatorVerticalStyle.normal.background = CreateSolidTexture(theme.SeparatorColor);
                separatorVerticalStyle.fixedWidth = Mathf.RoundToInt(1 * guiHelper.uiScale);
                separatorVerticalStyle.stretchHeight = true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSeparatorStyles", "StyleManager");
            }
        }

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

                tabsTriggerStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    tabsTriggerStyle.font = customFont;
                tabsTriggerStyle.fontSize = GetScaledFontSize(0.875f);
                tabsTriggerStyle.fontStyle = FontStyle.Bold;
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

                tabsTriggerActiveStyle.normal.background = CreateSolidTexture(theme.TabsTriggerActiveBg);
                tabsTriggerActiveStyle.normal.textColor = theme.TabsTriggerActiveFg;
                tabsTriggerActiveStyle.hover.background = CreateSolidTexture(theme.TabsTriggerActiveBg);
                tabsTriggerActiveStyle.active.background = CreateSolidTexture(theme.TabsTriggerActiveBg);

                tabsContentStyle = new UnityHelpers.GUIStyle();
                tabsContentStyle.padding = GetSpacingOffset(15f, 15f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTabsStyles", "StyleManager");
            }
        }

        private void SetupTextAreaVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                textAreaStyle = new UnityHelpers.GUIStyle(GUI.skin.textArea);
                textAreaStyle.fontSize = GetScaledFontSize(0.875f);
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
                textAreaOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

                textAreaGhostStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaGhostStyle.normal.background = transparentTexture;
                textAreaGhostStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTextAreaVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - Checkbox, Switch, Badge, Alert
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
                checkboxDefaultStyle.onNormal.background = checkboxCheckedTexture;
                checkboxDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxBg, Color.white, 0.1f));
                checkboxDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.CheckboxBg, Color.black, 0.1f));
                checkboxDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.white, 0.1f));
                checkboxDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.black, 0.1f));
                checkboxDefaultStyle.border = GetBorderOffset(6f);

                Color onBgColor = theme.ToggleOnBg;
                Color onHoverBgColor = Color.Lerp(onBgColor, Color.white, 0.1f);
                Color onActiveBgColor = Color.Lerp(onBgColor, Color.black, 0.1f);

                checkboxDefaultStyle.onNormal.background = CreateSolidTexture(onBgColor);
                checkboxDefaultStyle.onNormal.textColor = Color.white;
                checkboxDefaultStyle.onHover.background = CreateSolidTexture(onHoverBgColor);
                checkboxDefaultStyle.onHover.textColor = Color.white;
                checkboxDefaultStyle.onActive.background = CreateSolidTexture(onActiveBgColor);
                checkboxDefaultStyle.onActive.textColor = Color.white;

                checkboxOutlineStyle = new UnityHelpers.GUIStyle(checkboxDefaultStyle);
                if (customFont != null)
                    checkboxOutlineStyle.font = customFont;
                checkboxOutlineStyle.normal.background = CreateOutlineTexture();
                checkboxOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.white, 0.1f));
                checkboxOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.black, 0.1f));
                checkboxOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.AccentColor, theme.AccentColor);
                checkboxOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.white, 0.1f));
                checkboxOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.black, 0.1f));

                checkboxGhostStyle = new UnityHelpers.GUIStyle(checkboxDefaultStyle);
                if (customFont != null)
                    checkboxGhostStyle.font = customFont;
                checkboxGhostStyle.normal.background = transparentTexture;
                checkboxGhostStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f));
                checkboxGhostStyle.active.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.2f));
                checkboxGhostStyle.onNormal.background = CreateSolidTexture(theme.AccentColor);
                checkboxGhostStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.1f));
                checkboxGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCheckboxStyles", "StyleManager");
            }
        }

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
                switchDefaultStyle.onNormal.background = switchOnTexture;
                switchDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.SwitchOffBg, Color.white, 0.1f));
                switchDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.SwitchOffBg, Color.black, 0.1f));
                switchDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.SwitchOnBg, Color.white, 0.1f));
                switchDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.SwitchOnBg, Color.black, 0.1f));
                switchDefaultStyle.border = GetBorderOffset(6f);

                switchOutlineStyle = new UnityHelpers.GUIStyle(switchDefaultStyle);
                if (customFont != null)
                    switchOutlineStyle.font = customFont;
                switchOutlineStyle.normal.background = CreateOutlineTexture();
                switchOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.white, 0.1f));
                switchOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.black, 0.1f));
                switchOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.AccentColor, theme.AccentColor);
                switchOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.white, 0.1f));
                switchOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.black, 0.1f));

                switchGhostStyle = new UnityHelpers.GUIStyle(switchDefaultStyle);
                if (customFont != null)
                    switchGhostStyle.font = customFont;
                switchGhostStyle.normal.background = transparentTexture;
                switchGhostStyle.hover.background = CreateSolidTexture(Color.Lerp(transparentTexture.GetPixel(0, 0), Color.white, 0.1f));
                switchGhostStyle.active.background = CreateSolidTexture(Color.Lerp(transparentTexture.GetPixel(0, 0), Color.black, 0.1f));
                switchGhostStyle.onNormal.background = CreateSolidTexture(theme.AccentColor);
                switchGhostStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.1f));
                switchGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSwitchStyles", "StyleManager");
            }
        }

        private void SetupBadgeStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                badgeDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                if (customFont != null)
                    badgeDefaultStyle.font = customFont;
                badgeDefaultStyle.fontSize = GetScaledFontSize(0.75f);
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

        #region Style Setup Methods - Avatar, Skeleton, Table
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

        public int GetAvatarFontSize(AvatarSize size)
        {
            switch (size)
            {
                case AvatarSize.Small:
                    return GetScaledFontSize(0.75f);
                case AvatarSize.Large:
                    return GetScaledFontSize(1.25f);
                default:
                    return GetScaledFontSize(1.0f);
            }
        }

        public RectOffset GetAvatarBorder(AvatarShape shape, AvatarSize size)
        {
            float scale = guiHelper.uiScale;
            int borderRadius = 0;

            switch (shape)
            {
                case AvatarShape.Circle:
                    borderRadius = Mathf.RoundToInt(50 * scale);
                    break;
                case AvatarShape.Rounded:
                    borderRadius = Mathf.RoundToInt(8 * scale);
                    break;
                case AvatarShape.Square:
                    borderRadius = 0;
                    break;
            }

            return new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }

        public float GetStatusIndicatorSize(AvatarSize size)
        {
            switch (size)
            {
                case AvatarSize.Small:
                    return 6f;
                case AvatarSize.Large:
                    return 12f;
                default:
                    return 8f;
            }
        }

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

                tableHeaderStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    tableHeaderStyle.font = customFont;
                tableHeaderStyle.normal.background = CreateSolidTexture(theme.SecondaryColor);
                tableHeaderStyle.hover.background = CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.8f));
                tableHeaderStyle.active.background = CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.9f));
                tableHeaderStyle.normal.textColor = theme.TextColor;
                tableHeaderStyle.hover.textColor = theme.TextColor;
                tableHeaderStyle.active.textColor = theme.TextColor;
                tableHeaderStyle.padding = GetSpacingOffset(12f, 12f);
                tableHeaderStyle.margin = GetSpacingOffset(0f, 0f);
                tableHeaderStyle.fontSize = GetScaledFontSize(0.875f);
                tableHeaderStyle.fontStyle = FontStyle.Bold;
                tableHeaderStyle.alignment = TextAnchor.MiddleLeft;
                tableHeaderStyle.border = GetBorderOffset(0f);

                tableCellStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    tableCellStyle.font = customFont;
                tableCellStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);
                tableCellStyle.normal.textColor = theme.TextColor;
                tableCellStyle.padding = GetSpacingOffset(12f, 12f);
                tableCellStyle.margin = GetSpacingOffset(0f, 0f);
                tableCellStyle.fontSize = GetScaledFontSize(1.0f);
                tableCellStyle.alignment = TextAnchor.MiddleLeft;
                tableCellStyle.wordWrap = false;
                tableCellStyle.clipping = TextClipping.Clip;

                tableStripedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableStripedStyle.font = customFont;
                tableStripedStyle.normal.background = CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.3f));

                tableBorderedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableBorderedStyle.font = customFont;
                tableBorderedStyle.normal.background = CreateBorderTexture(theme.ButtonOutlineBorder, 1);

                tableHoverStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableHoverStyle.font = customFont;
                tableHoverStyle.hover.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.1f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTableStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - Calendar, Dropdown, MenuBar, Popover, ScrollArea, Select
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

                calendarTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    calendarTitleStyle.font = customFont;
                calendarTitleStyle.fontSize = GetScaledFontSize(1.125f);
                calendarTitleStyle.fontStyle = FontStyle.Bold;
                calendarTitleStyle.normal.textColor = theme.TextColor;
                calendarTitleStyle.alignment = TextAnchor.MiddleCenter;

                calendarWeekdayStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    calendarWeekdayStyle.font = customFont;
                calendarWeekdayStyle.fontSize = GetScaledFontSize(0.875f);
                calendarWeekdayStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.3f);
                calendarWeekdayStyle.alignment = TextAnchor.MiddleCenter;

                calendarDayStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    calendarDayStyle.font = customFont;
                calendarDayStyle.fontSize = GetScaledFontSize(1.0f);
                calendarDayStyle.normal.textColor = theme.TextColor;
                calendarDayStyle.normal.background = CreateSolidTexture(theme.CardBg);
                calendarDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.white, 0.1f));
                calendarDayStyle.active.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.black, 0.1f));

                calendarDaySelectedStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDaySelectedStyle.font = customFont;
                calendarDaySelectedStyle.normal.background = CreateSolidTexture(theme.AccentColor);
                calendarDaySelectedStyle.normal.textColor = Color.white;
                calendarDaySelectedStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.1f));
                calendarDaySelectedStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

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
                calendarDayInRangeStyle.normal.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.5f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCalendarStyles", "StyleManager");
            }
        }

        private void SetupDropdownMenuStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                dropdownMenuContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                dropdownMenuContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
                dropdownMenuContentStyle.border = GetBorderOffset(6f);
                dropdownMenuContentStyle.padding = GetSpacingOffset(4f, 4f);

                dropdownMenuItemStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    dropdownMenuItemStyle.font = customFont;
                dropdownMenuItemStyle.fontSize = GetScaledFontSize(0.875f);
                dropdownMenuItemStyle.fontStyle = FontStyle.Normal;
                dropdownMenuItemStyle.alignment = TextAnchor.MiddleLeft;
                dropdownMenuItemStyle.normal.background = transparentTexture;
                dropdownMenuItemStyle.normal.textColor = theme.TextColor;
                dropdownMenuItemStyle.hover.background = CreateSolidTexture(theme.AccentColor);
                dropdownMenuItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));
                dropdownMenuItemStyle.padding = GetSpacingOffset(12f, 4f);

                dropdownMenuSeparatorStyle = new UnityHelpers.GUIStyle();
                dropdownMenuSeparatorStyle.normal.background = CreateSolidTexture(theme.SeparatorColor);
                dropdownMenuSeparatorStyle.fixedHeight = 1;
                dropdownMenuSeparatorStyle.margin = GetSpacingOffset(4f, 4f);

                dropdownMenuHeaderStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    dropdownMenuHeaderStyle.font = customFont;
                dropdownMenuHeaderStyle.fontSize = GetScaledFontSize(0.75f);
                dropdownMenuHeaderStyle.fontStyle = FontStyle.Bold;
                dropdownMenuHeaderStyle.normal.textColor = theme.MutedColor;
                dropdownMenuHeaderStyle.padding = GetSpacingOffset(12f, 4f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDropdownMenuStyles", "StyleManager");
            }
        }

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

                menuBarItemStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    menuBarItemStyle.font = customFont;
                menuBarItemStyle.fontSize = GetScaledFontSize(0.875f);
                menuBarItemStyle.alignment = TextAnchor.MiddleLeft;
                menuBarItemStyle.padding = GetSpacingOffset(12f, 8f);
                menuBarItemStyle.margin = GetSpacingOffset(0f, 0f);
                menuBarItemStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                menuBarItemStyle.normal.background = transparentTexture;
                menuBarItemStyle.normal.textColor = theme.TextColor;
                menuBarItemStyle.hover.background = CreateSolidTexture(theme.ButtonGhostHoverBg);
                menuBarItemStyle.hover.textColor = theme.TextColor;
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

        private void SetupScrollAreaStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                scrollAreaStyle = new UnityHelpers.GUIStyle();
                scrollAreaStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);

                scrollAreaThumbStyle = new UnityHelpers.GUIStyle();
                scrollAreaThumbStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.white, 0.2f));
                scrollAreaThumbStyle.border = GetBorderOffset(4f);

                scrollAreaTrackStyle = new UnityHelpers.GUIStyle();
                scrollAreaTrackStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.black, 0.1f));
                scrollAreaTrackStyle.border = GetBorderOffset(4f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupScrollAreaStyles", "StyleManager");
            }
        }

        private void SetupSelectStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
                int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

                selectTriggerStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    selectTriggerStyle.font = customFont;
                selectTriggerStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
                selectTriggerStyle.fontStyle = FontStyle.Normal;
                selectTriggerStyle.alignment = TextAnchor.MiddleLeft;
                selectTriggerStyle.normal.background = CreateSolidTexture(theme.InputBg);
                selectTriggerStyle.normal.textColor = theme.TextColor;
                selectTriggerStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.InputBg, theme.AccentColor, 0.1f));
                selectTriggerStyle.active.background = CreateSolidTexture(Color.Lerp(theme.InputBg, theme.AccentColor, 0.2f));
                selectTriggerStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
                selectTriggerStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

                selectContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                selectContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
                selectContentStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle.padding = new UnityHelpers.RectOffset(5, 5, 5, 5);

                selectItemStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    selectItemStyle.font = customFont;
                selectItemStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
                selectItemStyle.fontStyle = FontStyle.Normal;
                selectItemStyle.alignment = TextAnchor.MiddleLeft;
                selectItemStyle.normal.background = transparentTexture;
                selectItemStyle.normal.textColor = theme.TextColor;
                selectItemStyle.hover.background = CreateSolidTexture(theme.AccentColor);
                selectItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));
                selectItemStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSelectStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup Methods - DatePicker, Dialog, Chart
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

                datePickerTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    datePickerTitleStyle.font = customFont;
                datePickerTitleStyle.fontSize = guiHelper.fontSize + 2;
                datePickerTitleStyle.fontStyle = FontStyle.Bold;
                datePickerTitleStyle.normal.textColor = theme.TextColor;
                datePickerTitleStyle.alignment = TextAnchor.MiddleCenter;

                datePickerWeekdayStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    datePickerWeekdayStyle.font = customFont;
                datePickerWeekdayStyle.fontSize = guiHelper.fontSize - 1;
                datePickerWeekdayStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.3f);
                datePickerWeekdayStyle.alignment = TextAnchor.MiddleCenter;

                datePickerDayStyle = new UnityHelpers.GUIStyle(GUI.skin.button);
                if (customFont != null)
                    datePickerDayStyle.font = customFont;
                datePickerDayStyle.fontSize = guiHelper.fontSize;
                datePickerDayStyle.normal.textColor = theme.TextColor;
                datePickerDayStyle.normal.background = CreateSolidTexture(theme.CardBg);
                datePickerDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.white, 0.1f));
                datePickerDayStyle.active.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.black, 0.1f));

                datePickerDaySelectedStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDaySelectedStyle.font = customFont;
                datePickerDaySelectedStyle.normal.background = CreateSolidTexture(theme.AccentColor);
                datePickerDaySelectedStyle.normal.textColor = Color.white;
                datePickerDaySelectedStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.1f));
                datePickerDaySelectedStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

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
                datePickerDayInRangeStyle.normal.background = CreateSolidTexture(new Color(theme.AccentColor.r, theme.AccentColor.g, theme.AccentColor.b, 0.5f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDatePickerStyles", "StyleManager");
            }
        }

        private void SetupDialogStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                dialogOverlayStyle = new UnityHelpers.GUIStyle();
                dialogOverlayStyle.normal.background = CreateSolidTexture(new Color(0, 0, 0, 0.8f));

                dialogContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                dialogContentStyle.normal.background = CreateRoundedRectTexture(512, 512, 12, theme.CardBg);
                dialogContentStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                dialogContentStyle.padding = new UnityHelpers.RectOffset(24, 24, 24, 24);

                dialogTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    dialogTitleStyle.font = customFont;
                dialogTitleStyle.fontSize = GetScaledFontSize(1.25f);
                dialogTitleStyle.fontStyle = FontStyle.Bold;
                dialogTitleStyle.normal.textColor = theme.TextColor;
                dialogTitleStyle.wordWrap = true;

                dialogDescriptionStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    dialogDescriptionStyle.font = customFont;
                dialogDescriptionStyle.fontSize = GetScaledFontSize(1.0f);
                dialogDescriptionStyle.normal.textColor = theme.MutedColor;
                dialogDescriptionStyle.wordWrap = true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDialogStyles", "StyleManager");
            }
        }

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

                chartAxisStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                if (customFont != null)
                    chartAxisStyle.font = customFont;
                chartAxisStyle.fontSize = GetScaledFontSize(0.75f);
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

        #endregion

        #region Style Getters
        #region Style Getters - Button, Toggle, Input, Label
        public GUIStyle GetButtonStyle(ButtonVariant variant, ButtonSize size)
        {
            if (buttonStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle baseStyle = variant switch
            {
                ButtonVariant.Destructive => buttonDestructiveStyle,
                ButtonVariant.Outline => buttonOutlineStyle,
                ButtonVariant.Secondary => buttonSecondaryStyle,
                ButtonVariant.Ghost => buttonGhostStyle,
                ButtonVariant.Link => buttonLinkStyle,
                _ => buttonDefaultStyle,
            };

            if (baseStyle == null)
                return GUI.skin.button;

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);

            switch (size)
            {
                case ButtonSize.Small:
                    sizedStyle.fontSize = GetScaledFontSize(0.75f);
                    sizedStyle.padding = GetSpacingOffset(12f, 2f);
                    sizedStyle.fixedHeight = GetScaledHeight(32f);
                    sizedStyle.border = GetBorderOffset(6f);
                    break;
                case ButtonSize.Large:
                    sizedStyle.fontSize = GetScaledFontSize(1.0f);
                    sizedStyle.padding = GetSpacingOffset(24f, 12f);
                    sizedStyle.fixedHeight = GetScaledHeight(44f);
                    sizedStyle.border = GetBorderOffset(6f);
                    break;
                case ButtonSize.Icon:
                    sizedStyle.fontSize = GetScaledFontSize(1.0f);
                    sizedStyle.padding = GetSpacingOffset(0f, 0f);
                    int iconSize = GetScaledHeight(40f - 4f);
                    sizedStyle.fixedWidth = iconSize;
                    sizedStyle.fixedHeight = iconSize;
                    sizedStyle.border = GetBorderOffset(6f);
                    break;
            }

            buttonStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetToggleStyle(ToggleVariant variant, ToggleSize size)
        {
            if (toggleStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;

            GUIStyle baseStyle = variant == ToggleVariant.Outline ? toggleOutlineStyle : toggleDefaultStyle;
            if (baseStyle == null)
                return GUI.skin.button;

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);
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

            if (disabled)
            {
                inputStyleCache[(variant, focused, disabled)] = inputDisabledStyle ?? GUI.skin.textField;
                return inputDisabledStyle ?? GUI.skin.textField;
            }
            if (focused)
            {
                inputStyleCache[(variant, focused, disabled)] = inputFocusedStyle ?? GUI.skin.textField;
                return inputFocusedStyle ?? GUI.skin.textField;
            }

            GUIStyle style = variant switch
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

            var style = focused ? (textAreaFocusedStyle ?? baseStyle) : baseStyle;
            textAreaStyleCache[(variant, focused)] = style;
            return style;
        }
        #endregion

        #region Style Getters - ProgressBar, Separator, Tabs
        public GUIStyle GetProgressBarStyle() => progressBarStyle ?? GUI.skin.box;

        public GUIStyle GetProgressBarBackgroundStyle() => progressBarBackgroundStyle ?? GUI.skin.box;

        public GUIStyle GetProgressBarFillStyle() => progressBarFillStyle ?? GUI.skin.box;

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation) => orientation == SeparatorOrientation.Horizontal ? (separatorHorizontalStyle ?? GUI.skin.box) : (separatorVerticalStyle ?? GUI.skin.box);

        public GUIStyle GetTabsListStyle() => tabsListStyle ?? GUI.skin.box;

        public GUIStyle GetTabsTriggerStyle(bool active = false) => active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) : (tabsTriggerStyle ?? GUI.skin.button);

        public GUIStyle GetTabsContentStyle() => tabsContentStyle ?? GUIStyle.none;

        public Texture2D GetGlowTexture() => glowTexture;

        public Texture2D GetParticleTexture() => particleTexture;

        public Texture2D GetInputBackgroundTexture() => inputBackgroundTexture;

        public Texture2D GetInputFocusedTexture() => inputFocusedTexture;

        public Texture2D GetTransparentTexture() => transparentTexture;

        public Texture2D GetProgressBarBackgroundTexture() => progressBarBackgroundTexture;

        public Texture2D GetProgressBarFillTexture() => progressBarFillTexture;
        #endregion

        #region Style Getters - Checkbox, Switch, Badge
        public GUIStyle GetCheckboxStyle(CheckboxVariant variant, CheckboxSize size)
        {
            if (checkboxStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;

            var theme = ThemeManager.Instance.CurrentTheme;
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
        #endregion

        #region Style Getters - Skeleton, Table
        public GUIStyle GetAvatarStyle(AvatarSize size, AvatarShape shape)
        {
            if (avatarStyleCache.TryGetValue((size, shape), out var cachedStyle))
                return cachedStyle;

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(avatarStyle);

            int avatarSizeValue = size switch
            {
                AvatarSize.Small => Mathf.RoundToInt(32 * guiHelper.uiScale),
                AvatarSize.Large => Mathf.RoundToInt(48 * guiHelper.uiScale),
                _ => Mathf.RoundToInt(40 * guiHelper.uiScale),
            };
            sizedStyle.fixedWidth = avatarSizeValue;
            sizedStyle.fixedHeight = avatarSizeValue;

            switch (shape)
            {
                case AvatarShape.Circle:
                    sizedStyle.border = new UnityHelpers.RectOffset(50, 50, 50, 50);
                    break;
                case AvatarShape.Rounded:
                    sizedStyle.border = new UnityHelpers.RectOffset(8, 8, 8, 8);
                    break;
                case AvatarShape.Square:
                    sizedStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    break;
            }

            avatarStyleCache[(size, shape)] = sizedStyle;
            return sizedStyle;
        }

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

            var style = baseStyle ?? GUI.skin.box;
            tableStyleCache[(variant, size)] = style;
            return style;
        }

        public GUIStyle GetTableHeaderStyle(TableVariant variant, TableSize size) => tableHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetTableCellStyle(TableVariant variant, TableSize size) => tableCellStyle ?? GUI.skin.label;
        #endregion

        #region Style Getters - Calendar, Dropdown, ScrollArea, Select, Chart

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

        public GUIStyle GetDropdownMenuStyle(DropdownMenuVariant variant, DropdownMenuSize size)
        {
            if (dropdownMenuStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;

            var style = dropdownMenuContentStyle;
            dropdownMenuStyleCache[(variant, size)] = style;
            return style;
        }

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

        public GUIStyle GetSelectTriggerStyle() => selectTriggerStyle;

        public GUIStyle GetSelectItemStyle() => selectItemStyle;

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

        public GUIStyle GetChartAxisStyle() => chartAxisStyle ?? GUI.skin.label;
        #endregion
        #endregion

        #region Cleanup
        public void Cleanup()
        {
            if (gradientTexture)
                Object.Destroy(gradientTexture);
            if (glowTexture)
                Object.Destroy(glowTexture);
            if (particleTexture)
                Object.Destroy(particleTexture);
            if (cardBackgroundTexture)
                Object.Destroy(cardBackgroundTexture);
            if (outlineTexture)
                Object.Destroy(outlineTexture);
            if (transparentTexture)
                Object.Destroy(transparentTexture);
            if (inputBackgroundTexture)
                Object.Destroy(inputBackgroundTexture);
            if (inputFocusedTexture)
                Object.Destroy(inputFocusedTexture);
            if (progressBarBackgroundTexture)
                Object.Destroy(progressBarBackgroundTexture);
            if (progressBarFillTexture)
                Object.Destroy(progressBarFillTexture);
            if (separatorTexture)
                Object.Destroy(separatorTexture);
            if (tabsBackgroundTexture)
                Object.Destroy(tabsBackgroundTexture);
            if (tabsActiveTexture)
                Object.Destroy(tabsActiveTexture);
            if (checkboxTexture)
                Object.Destroy(checkboxTexture);
            if (checkboxCheckedTexture)
                Object.Destroy(checkboxCheckedTexture);
            if (switchTexture)
                Object.Destroy(switchTexture);
            if (switchOnTexture)
                Object.Destroy(switchOnTexture);
            if (switchOffTexture)
                Object.Destroy(switchOffTexture);
            if (badgeTexture)
                Object.Destroy(badgeTexture);
            if (avatarTexture)
                Object.Destroy(avatarTexture);
            if (tableTexture)
                Object.Destroy(tableTexture);
            if (tableHeaderTexture)
                Object.Destroy(tableHeaderTexture);
            if (tableCellTexture)
                Object.Destroy(tableCellTexture);
            if (calendarBackgroundTexture)
                Object.Destroy(calendarBackgroundTexture);
            if (calendarHeaderTexture)
                Object.Destroy(calendarHeaderTexture);
            if (calendarDayTexture)
                Object.Destroy(calendarDayTexture);
            if (calendarDaySelectedTexture)
                Object.Destroy(calendarDaySelectedTexture);
            if (dropdownMenuContentTexture)
                Object.Destroy(dropdownMenuContentTexture);
            if (popoverContentTexture)
                Object.Destroy(popoverContentTexture);
            if (scrollAreaThumbTexture)
                Object.Destroy(scrollAreaThumbTexture);
            if (scrollAreaTrackTexture)
                Object.Destroy(scrollAreaTrackTexture);
            if (selectTriggerTexture)
                Object.Destroy(selectTriggerTexture);
            if (selectContentTexture)
                Object.Destroy(selectContentTexture);

            foreach (var texture in solidColorTextureCache.Values)
                if (texture)
                    Object.Destroy(texture);
            solidColorTextureCache.Clear();

            foreach (var texture in outlineButtonTextureCache.Values)
                if (texture)
                    Object.Destroy(texture);
            outlineButtonTextureCache.Clear();

            foreach (var texture in outlineTextureCache.Values)
                if (texture)
                    Object.Destroy(texture);
            outlineTextureCache.Clear();
        }
        #endregion
    }
}
