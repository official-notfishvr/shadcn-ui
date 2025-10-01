using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using shadcnui;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    #region Enums
    /// <summary>
    /// Variants for the Button component.
    /// </summary>
    public enum ButtonVariant
    {
        Default,
        Destructive,
        Outline,
        Secondary,
        Ghost,
        Link,
    }

    /// <summary>
    /// Sizes for the Button component.
    /// </summary>
    public enum ButtonSize
    {
        Default,
        Small,
        Large,
        Icon,
    }

    /// <summary>
    /// Variants for the Toggle component.
    /// </summary>
    public enum ToggleVariant
    {
        Default,
        Outline,
    }

    /// <summary>
    /// Sizes for the Toggle component.
    /// </summary>
    public enum ToggleSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the Input component.
    /// </summary>
    public enum InputVariant
    {
        Default,
        Outline,
        Ghost,
    }

    /// <summary>
    /// Variants for the Label component.
    /// </summary>
    public enum LabelVariant
    {
        Default,
        Secondary,
        Muted,
        Destructive,
    }

    /// <summary>
    /// Orientations for the Separator component.
    /// </summary>
    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// Variants for the TextArea component.
    /// </summary>
    public enum TextAreaVariant
    {
        Default,
        Outline,
        Ghost,
    }

    /// <summary>
    /// Variants for the Progress component.
    /// </summary>
    public enum ProgressVariant
    {
        Default,
    }

    /// <summary>
    /// Variants for the Tabs component.
    /// </summary>
    public enum TabsVariant
    {
        Default,
    }

    /// <summary>
    /// Variants for the Checkbox component.
    /// </summary>
    public enum CheckboxVariant
    {
        Default,
        Outline,
        Ghost,
    }

    /// <summary>
    /// Sizes for the Checkbox component.
    /// </summary>
    public enum CheckboxSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the Switch component.
    /// </summary>
    public enum SwitchVariant
    {
        Default,
        Outline,
        Ghost,
    }

    /// <summary>
    /// Sizes for the Switch component.
    /// </summary>
    public enum SwitchSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the Badge component.
    /// </summary>
    public enum BadgeVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline,
    }

    /// <summary>
    /// Sizes for the Badge component.
    /// </summary>
    public enum BadgeSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the Alert component.
    /// </summary>
    public enum AlertVariant
    {
        Default,
        Destructive,
    }

    /// <summary>
    /// Types for the Alert component.
    /// </summary>
    public enum AlertType
    {
        Info,
        Warning,
        Error,
        Success,
    }

    /// <summary>
    /// Sizes for the Avatar component.
    /// </summary>
    public enum AvatarSize
    {
        Small,
        Default,
        Large,
    }

    /// <summary>
    /// Shapes for the Avatar component.
    /// </summary>
    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded,
    }

    /// <summary>
    /// Variants for the Skeleton component.
    /// </summary>
    public enum SkeletonVariant
    {
        Default,
        Rounded,
        Circular,
    }

    /// <summary>
    /// Sizes for the Skeleton component.
    /// </summary>
    public enum SkeletonSize
    {
        Small,
        Default,
        Large,
    }

    /// <summary>
    /// Variants for the Table component.
    /// </summary>
    public enum TableVariant
    {
        Default,
        Striped,
        Bordered,
        Hover,
    }

    /// <summary>
    /// Sizes for the Table component.
    /// </summary>
    public enum TableSize
    {
        Small,
        Default,
        Large,
    }

    /// <summary>
    /// Variants for the Calendar component.
    /// </summary>
    public enum CalendarVariant
    {
        Default,
    }

    /// <summary>
    /// Sizes for the Calendar component.
    /// </summary>
    public enum CalendarSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the DropdownMenu component.
    /// </summary>
    public enum DropdownMenuVariant
    {
        Default,
    }

    /// <summary>
    /// Sizes for the DropdownMenu component.
    /// </summary>
    public enum DropdownMenuSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the ScrollArea component.
    /// </summary>
    public enum ScrollAreaVariant
    {
        Default,
    }

    /// <summary>
    /// Sizes for the ScrollArea component.
    /// </summary>
    public enum ScrollAreaSize
    {
        Default,
        Small,
        Large,
    }

    /// <summary>
    /// Variants for the Select component.
    /// </summary>
    public enum SelectVariant
    {
        Default,
    }

    /// <summary>
    /// Sizes for the Select component.
    /// </summary>
    public enum SelectSize
    {
        Default,
        Small,
        Large,
    }
    #endregion

    /// <summary>
    /// Manages the GUI styles for the shadcnui components.
    /// </summary>
    public class StyleManager
    {
        private GUIHelper guiHelper;

        public Font customFont;

        #region Design Helper Methods
        public int GetScaledBorderRadius(float radius)
        {
            return Mathf.RoundToInt(radius * guiHelper.uiScale);
        }

        public int GetScaledSpacing(float spacing)
        {
            return Mathf.RoundToInt(spacing * guiHelper.uiScale);
        }

        public int GetScaledHeight(float height)
        {
            return Mathf.RoundToInt(height * guiHelper.uiScale);
        }

        public int GetScaledFontSize(float scale = 1.0f)
        {
            return Mathf.RoundToInt(guiHelper.fontSize * scale * guiHelper.uiScale);
        }

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

        private Texture2D CreateBorderTexture(Color borderColor, int thickness)
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
            var theme = ThemeManager.Instance.CurrentTheme;
            Color shadowColor = new Color(0f, 0f, 0f, intensity);

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

        public Color GetHoverColor(Color baseColor, bool isDark = true)
        {
            if (isDark)
            {
                return Color.Lerp(baseColor, Color.white, 0.08f);
            }
            else
            {
                return Color.Lerp(baseColor, Color.black, 0.05f);
            }
        }

        public Color GetFocusColor(Color baseColor)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return Color.Lerp(baseColor, theme.AccentColor, 0.15f);
        }
        #endregion

        #region GUIStyle Fields
        // Animated Styles
        public GUIStyle animatedBoxStyle;
        public GUIStyle animatedButtonStyle;
        public GUIStyle animatedInputStyle;
        public GUIStyle glowLabelStyle;
        public GUIStyle titleStyle;
        public GUIStyle colorPresetStyle;
        public GUIStyle sectionHeaderStyle;

        // Card Styles
        public GUIStyle cardStyle;
        public GUIStyle cardHeaderStyle;
        public GUIStyle cardTitleStyle;
        public GUIStyle cardDescriptionStyle;
        public GUIStyle cardContentStyle;
        public GUIStyle cardFooterStyle;

        // Button Styles
        public GUIStyle buttonDefaultStyle;
        public GUIStyle buttonDestructiveStyle;
        public GUIStyle buttonOutlineStyle;
        public GUIStyle buttonSecondaryStyle;
        public GUIStyle buttonGhostStyle;
        public GUIStyle buttonLinkStyle;

        public GUIStyle buttonIconStyle;

        // Toggle Styles
        public GUIStyle toggleDefaultStyle;
        public GUIStyle toggleOutlineStyle;

        // Input Styles
        public GUIStyle inputDefaultStyle;
        public GUIStyle inputOutlineStyle;
        public GUIStyle inputGhostStyle;
        public GUIStyle inputFocusedStyle;
        public GUIStyle inputDisabledStyle;

        // Label Styles
        public GUIStyle labelDefaultStyle;
        public GUIStyle labelSecondaryStyle;
        public GUIStyle labelMutedStyle;
        public GUIStyle labelDestructiveStyle;

        // PasswordField Styles
        public GUIStyle passwordFieldStyle;

        // TextArea Styles
        public GUIStyle textAreaStyle;
        public GUIStyle textAreaFocusedStyle;
        public GUIStyle textAreaOutlineStyle;
        public GUIStyle textAreaGhostStyle;

        // ProgressBar Styles
        public GUIStyle progressBarStyle;
        public GUIStyle progressBarBackgroundStyle;
        public GUIStyle progressBarFillStyle;

        // Separator Styles
        public GUIStyle separatorHorizontalStyle;
        public GUIStyle separatorVerticalStyle;

        // Tabs Styles
        public GUIStyle tabsListStyle;
        public GUIStyle tabsTriggerStyle;
        public GUIStyle tabsTriggerActiveStyle;
        public GUIStyle tabsContentStyle;

        // Checkbox Styles
        public GUIStyle checkboxDefaultStyle;
        public GUIStyle checkboxOutlineStyle;
        public GUIStyle checkboxGhostStyle;

        // Switch Styles
        public GUIStyle switchDefaultStyle;
        public GUIStyle switchOutlineStyle;
        public GUIStyle switchGhostStyle;

        // Badge Styles
        public GUIStyle badgeDefaultStyle;
        public GUIStyle badgeSecondaryStyle;
        public GUIStyle badgeDestructiveStyle;
        public GUIStyle badgeOutlineStyle;

        // Alert Styles
        public GUIStyle alertDefaultStyle;
        public GUIStyle alertDestructiveStyle;
        public GUIStyle alertTitleStyle;
        public GUIStyle alertDescriptionStyle;

        // Avatar Styles
        public GUIStyle avatarStyle;

        // Skeleton Styles
        public GUIStyle skeletonStyle;
        public GUIStyle skeletonRoundedStyle;
        public GUIStyle skeletonCircularStyle;

        // Table Styles
        public GUIStyle tableStyle;
        public GUIStyle tableHeaderStyle;
        public GUIStyle tableCellStyle;
        public GUIStyle tableStripedStyle;
        public GUIStyle tableBorderedStyle;
        public GUIStyle tableHoverStyle;

        // Slider Styles
        public GUIStyle sliderDefaultStyle;
        public GUIStyle sliderRangeStyle;
        public GUIStyle sliderVerticalStyle;
        public GUIStyle sliderDisabledStyle;

        public GUIStyle sliderLabelStyle;
        public GUIStyle sliderValueStyle;

        // Calendar Styles
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

        // DatePicker Styles
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

        // Dialog Styles
        public GUIStyle dialogOverlayStyle;
        public GUIStyle dialogContentStyle;
        public GUIStyle dialogTitleStyle;
        public GUIStyle dialogDescriptionStyle;

        // DropdownMenu Styles
        public GUIStyle dropdownMenuContentStyle;
        public GUIStyle dropdownMenuItemStyle;
        public GUIStyle dropdownMenuSeparatorStyle;
        public GUIStyle dropdownMenuHeaderStyle;

        // Popover Styles
        public GUIStyle popoverContentStyle;

        // ScrollArea Styles
        public GUIStyle scrollAreaStyle;
        public GUIStyle scrollAreaThumbStyle;
        public GUIStyle scrollAreaTrackStyle;

        // Select Styles
        public GUIStyle selectTriggerStyle;
        public GUIStyle selectContentStyle;
        public GUIStyle selectItemStyle;

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
        private Texture2D alertTexture;
        private Texture2D avatarTexture;
        private Texture2D skeletonTexture;
        private Texture2D tableTexture;
        private Texture2D tableHeaderTexture;
        private Texture2D tableCellTexture;

        // Calendar Textures
        private Texture2D calendarBackgroundTexture;
        private Texture2D calendarHeaderTexture;
        private Texture2D calendarDayTexture;
        private Texture2D calendarDaySelectedTexture;

        // DropdownMenu Textures
        private Texture2D dropdownMenuContentTexture;

        // Popover Textures
        private Texture2D popoverContentTexture;

        // ScrollArea Textures
        private Texture2D scrollAreaThumbTexture;
        private Texture2D scrollAreaTrackTexture;

        // Select Textures
        private Texture2D selectTriggerTexture;
        private Texture2D selectContentTexture;
        #endregion


        public StyleManager(GUIHelper helper)
        {
            guiHelper = helper;
        }

        /// <summary>
        /// Gets the current theme with shadcn/ui design tokens.
        /// </summary>
        public Theme GetTheme()
        {
            return ThemeManager.Instance.CurrentTheme;
        }

        public void SetCustomFont(byte[] fontData, string fontName = "CustomFont.ttf")
        {
            if (fontData == null || fontData.Length == 0)
            {
                //Debug.LogWarning("Font data is null or empty.");
                return;
            }

            string tempPath = Path.Combine(Application.temporaryCachePath, fontName);
            File.WriteAllBytes(tempPath, fontData);

#if IL2CPP_MELONLOADER
    // IL2CPP: Cannot load dynamic fonts from file easily
#else
            UnityHelpers.Font loadedFont = new UnityHelpers.Font(tempPath);

            if (loadedFont != null)
            {
                customFont = loadedFont;
            }
            else
            {
                //Debug.LogWarning($"Failed to create font from bytes for '{fontName}'.");
            }

            try
            {
                File.Delete(tempPath);
            }
            catch { }
#endif
        }

        #region Initialization
        /// <summary>
        /// Initializes the GUI styles.
        /// </summary>
        public void InitializeGUI()
        {
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
            SetupAlertStyles();
            SetupAvatarStyles();
            SetupSkeletonStyles();
            SetupTableStyles();
            SetupCalendarStyles();
            SetupDropdownMenuStyles();
            SetupPopoverStyles();
            SetupScrollAreaStyles();
            SetupSelectStyles();
            SetupDatePickerStyles();
            SetupDialogStyles();
        }
        #endregion

        #region Texture Creation
        private void CreateCustomTextures()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            gradientTexture = new Texture2D(1, 100);
            for (int i = 0; i < 100; i++)
            {
                float t = i / 99f;
                Color gradColor = Color.Lerp(theme.PrimaryColor, theme.SecondaryColor, t);
                gradientTexture.SetPixel(0, i, gradColor);
            }
            gradientTexture.Apply();

            cardBackgroundTexture = CreateSolidTexture(theme.CardBg);
            inputBackgroundTexture = CreateSolidTexture(theme.InputBg);
            inputFocusedTexture = CreateSolidTexture(theme.InputBg);
            outlineTexture = CreateBorderTexture(theme.ButtonOutlineBorder, 4);
            transparentTexture = CreateSolidTexture(Color.clear);
            glowTexture = CreateGlowTexture(theme.AccentColor, 32);
            particleTexture = CreateSolidTexture(Color.Lerp(theme.AccentColor, theme.TextColor, 0.5f));
            progressBarBackgroundTexture = CreateSolidTexture(theme.SecondaryColor);
            progressBarFillTexture = CreateSolidTexture(theme.PrimaryColor);
            separatorTexture = CreateSolidTexture(theme.SeparatorColor);
            tabsBackgroundTexture = CreateSolidTexture(theme.TabsBg);
            tabsActiveTexture = CreateSolidTexture(theme.BackgroundColor);
            checkboxTexture = CreateSolidTexture(theme.CheckboxBg);
            checkboxCheckedTexture = CreateSolidTexture(theme.CheckboxCheckedBg);
            switchTexture = CreateSolidTexture(theme.SwitchBg);
            switchOnTexture = CreateSolidTexture(theme.SwitchOnBg);
            switchOffTexture = CreateSolidTexture(theme.SwitchOffBg);
            badgeTexture = CreateSolidTexture(theme.BadgeBg);
            alertTexture = CreateSolidTexture(theme.AlertDefaultBg);
            avatarTexture = CreateSolidTexture(theme.AvatarBg);
            skeletonTexture = CreateSolidTexture(theme.SkeletonBg);
            tableTexture = CreateSolidTexture(theme.TableBg);
            tableHeaderTexture = CreateSolidTexture(theme.TableHeaderBg);
            tableCellTexture = CreateSolidTexture(theme.TableCellBg);
            calendarBackgroundTexture = CreateSolidTexture(theme.CardBg);
            calendarHeaderTexture = CreateSolidTexture(theme.CardBg);
            calendarDayTexture = CreateSolidTexture(theme.CardBg);
            calendarDaySelectedTexture = CreateSolidTexture(theme.PrimaryColor);
            dropdownMenuContentTexture = CreateSolidTexture(theme.CardBg);
            popoverContentTexture = CreateSolidTexture(theme.CardBg);
            scrollAreaThumbTexture = CreateSolidTexture(theme.ButtonOutlineBorder);
            scrollAreaTrackTexture = CreateSolidTexture(theme.SecondaryColor);
            selectTriggerTexture = CreateSolidTexture(theme.InputBg);
            selectContentTexture = CreateSolidTexture(theme.CardBg);

            cardShadowTexture = CreateShadowTexture(32, 32, 0.08f, 6f);
            buttonShadowTexture = CreateDropShadowTexture(16, new Vector2(0, 1), 2f, 0.05f);
            popoverShadowTexture = CreateShadowTexture(24, 24, 0.12f, 8f);
            dialogShadowTexture = CreateShadowTexture(40, 40, 0.15f, 12f);
        }
        #endregion

        #region Style Setup Methods
        private void SetupAnimatedStyles()
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

        private void SetupCardStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            cardStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            cardStyle.normal.background = CreateSolidTexture(theme.CardBg);
            cardStyle.border = GetBorderOffset(12f);
            cardStyle.padding = GetSpacingOffset(0f, 0f);
            cardStyle.margin = GetSpacingOffset(0f, 0f);

            cardHeaderStyle = new UnityHelpers.GUIStyle();
            cardHeaderStyle.padding = GetSpacingOffset(24f, 24f);
            cardHeaderStyle.margin = GetSpacingOffset(0f, 0f);

            cardTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                cardTitleStyle.font = customFont;
            cardTitleStyle.fontSize = GetScaledFontSize(1.25f);
            cardTitleStyle.fontStyle = FontStyle.Bold;
            cardTitleStyle.normal.textColor = theme.CardTitle;
            cardTitleStyle.wordWrap = true;
            cardTitleStyle.margin = GetSpacingOffset(0f, 0f);

            cardDescriptionStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                cardDescriptionStyle.font = customFont;
            cardDescriptionStyle.fontSize = GetScaledFontSize(0.875f);
            cardDescriptionStyle.normal.textColor = theme.CardDescription;
            cardDescriptionStyle.wordWrap = true;
            cardDescriptionStyle.margin = GetSpacingOffset(0f, 2f);

            cardContentStyle = new UnityHelpers.GUIStyle();
            cardContentStyle.padding = GetSpacingOffset(24f, 0f);

            cardFooterStyle = new UnityHelpers.GUIStyle();
            cardFooterStyle.padding = GetSpacingOffset(24f, 24f);
        }

        private void SetupButtonVariantStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            buttonDefaultStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonDefaultStyle.font = customFont;
            buttonDefaultStyle.fontSize = GetScaledFontSize(0.875f);
            buttonDefaultStyle.fontStyle = FontStyle.Normal;
            buttonDefaultStyle.alignment = TextAnchor.MiddleCenter;
            buttonDefaultStyle.padding = GetSpacingOffset(16f, 8f);
            buttonDefaultStyle.border = GetBorderOffset(6f);
            buttonDefaultStyle.fixedHeight = GetScaledHeight(40f);
            buttonDefaultStyle.normal.background = CreateSolidTexture(theme.ButtonPrimaryBg);
            buttonDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.ButtonPrimaryBg, Color.white, 0.08f));
            buttonDefaultStyle.normal.textColor = theme.ButtonPrimaryFg;
            buttonDefaultStyle.hover.textColor = theme.ButtonPrimaryFg;
            buttonDefaultStyle.active.background = CreateSolidTexture(theme.ButtonPrimaryActiveBg);
            buttonDefaultStyle.active.textColor = theme.ButtonPrimaryActiveFg;
            buttonDefaultStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.ButtonPrimaryBg, theme.AccentColor, 0.1f));
            buttonDefaultStyle.focused.textColor = theme.ButtonPrimaryFg;

            buttonDestructiveStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonDestructiveStyle.font = customFont;
            buttonDestructiveStyle.fontSize = GetScaledFontSize(0.875f);
            buttonDestructiveStyle.fontStyle = FontStyle.Normal;
            buttonDestructiveStyle.alignment = TextAnchor.MiddleCenter;
            buttonDestructiveStyle.padding = GetSpacingOffset(16f, 8f);
            buttonDestructiveStyle.border = GetBorderOffset(6f);
            buttonDestructiveStyle.fixedHeight = GetScaledHeight(40f);
            buttonDestructiveStyle.normal.background = CreateSolidTexture(theme.ButtonDestructiveBg);
            buttonDestructiveStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.1f));
            buttonDestructiveStyle.normal.textColor = theme.ButtonDestructiveFg;
            buttonDestructiveStyle.hover.textColor = theme.ButtonDestructiveFg;
            buttonDestructiveStyle.active.background = CreateSolidTexture(Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.2f));
            buttonDestructiveStyle.active.textColor = theme.ButtonDestructiveFg;

            buttonOutlineStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonOutlineStyle.font = customFont;
            buttonOutlineStyle.fontSize = GetScaledFontSize(0.875f);
            buttonOutlineStyle.fontStyle = FontStyle.Normal;
            buttonOutlineStyle.alignment = TextAnchor.MiddleCenter;
            buttonOutlineStyle.padding = GetSpacingOffset(16f, 8f);
            buttonOutlineStyle.border = GetBorderOffset(6f);
            buttonOutlineStyle.fixedHeight = GetScaledHeight(40f);
            buttonOutlineStyle.normal.background = CreateBorderTexture(theme.ButtonOutlineBorder, 1);
            buttonOutlineStyle.hover.background = CreateSolidTexture(theme.ButtonOutlineHoverBg);
            buttonOutlineStyle.normal.textColor = theme.ButtonOutlineFg;
            buttonOutlineStyle.hover.textColor = theme.ButtonOutlineHoverFg;
            buttonOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(theme.ButtonOutlineHoverBg, Color.black, 0.05f));
            buttonOutlineStyle.active.textColor = theme.ButtonOutlineHoverFg;
            buttonOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.ButtonOutlineHoverBg, theme.AccentColor, 0.15f));
            buttonOutlineStyle.focused.textColor = theme.ButtonOutlineHoverFg;

            buttonSecondaryStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonSecondaryStyle.font = customFont;
            buttonSecondaryStyle.fontSize = GetScaledFontSize(0.875f);
            buttonSecondaryStyle.fontStyle = FontStyle.Normal;
            buttonSecondaryStyle.alignment = TextAnchor.MiddleCenter;
            buttonSecondaryStyle.padding = GetSpacingOffset(16f, 8f);
            buttonSecondaryStyle.border = GetBorderOffset(6f);
            buttonSecondaryStyle.fixedHeight = GetScaledHeight(40f);
            buttonSecondaryStyle.normal.background = CreateSolidTexture(theme.ButtonSecondaryBg);
            buttonSecondaryStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.ButtonSecondaryBg, Color.white, 0.05f));
            buttonSecondaryStyle.normal.textColor = theme.ButtonSecondaryFg;
            buttonSecondaryStyle.hover.textColor = theme.ButtonSecondaryFg;
            buttonSecondaryStyle.active.background = CreateSolidTexture(Color.Lerp(theme.ButtonSecondaryBg, Color.black, 0.05f));
            buttonSecondaryStyle.active.textColor = theme.ButtonSecondaryFg;

            buttonGhostStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonGhostStyle.font = customFont;
            buttonGhostStyle.fontSize = GetScaledFontSize(0.875f);
            buttonGhostStyle.fontStyle = FontStyle.Normal;
            buttonGhostStyle.alignment = TextAnchor.MiddleCenter;
            buttonGhostStyle.padding = GetSpacingOffset(16f, 8f);
            buttonGhostStyle.border = GetBorderOffset(6f);
            buttonGhostStyle.fixedHeight = GetScaledHeight(40f);
            buttonGhostStyle.normal.background = transparentTexture;
            buttonGhostStyle.hover.background = CreateSolidTexture(theme.ButtonGhostHoverBg);
            buttonGhostStyle.normal.textColor = theme.ButtonGhostFg;
            buttonGhostStyle.hover.textColor = theme.ButtonGhostHoverFg;
            buttonGhostStyle.active.background = CreateSolidTexture(GetHoverColor(theme.ButtonGhostHoverBg, true));
            buttonGhostStyle.active.textColor = theme.ButtonGhostHoverFg;
            buttonGhostStyle.focused.background = CreateSolidTexture(GetFocusColor(theme.ButtonGhostHoverBg));
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

        private GUIStyle CreateBaseButtonStyle()
        {
            GUIStyle style = new UnityHelpers.GUIStyle(GUI.skin.button);
            style.wordWrap = false;
            style.clipping = TextClipping.Clip;
            return style;
        }

        private Dictionary<(Color, Color), Texture2D> outlineButtonTextureCache = new Dictionary<(Color, Color), Texture2D>();

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor, Color borderColor)
        {
            if (outlineButtonTextureCache.TryGetValue((backgroundColor, borderColor), out var cachedTexture))
            {
                return cachedTexture;
            }

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

        /// <summary>
        /// Sets up the toggle styles.
        /// </summary>
        private void SetupToggleVariantStyles()
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

        /// <summary>
        /// Sets up the input styles.
        /// </summary>
        private void SetupInputVariantStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            inputDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.textField);
            if (customFont != null)
                inputDefaultStyle.font = customFont;
            inputDefaultStyle.fontSize = GetScaledFontSize(0.875f);
            inputDefaultStyle.padding = GetSpacingOffset(12f, 8f);
            inputDefaultStyle.margin = GetSpacingOffset(0f, 4f);
            inputDefaultStyle.border = GetBorderOffset(6f);
            inputDefaultStyle.fixedHeight = GetScaledHeight(40f);
            inputDefaultStyle.normal.background = inputBackgroundTexture;
            inputDefaultStyle.normal.textColor = theme.InputFg;
            inputDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.InputBg, theme.AccentColor, 0.05f));
            inputDefaultStyle.hover.textColor = theme.InputFg;
            inputDefaultStyle.focused.background = inputFocusedTexture;
            inputDefaultStyle.focused.textColor = theme.InputFocusedFg;
            inputDefaultStyle.onFocused.background = CreateBorderTexture(theme.AccentColor, 2);

            inputOutlineStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputOutlineStyle.font = customFont;
            inputOutlineStyle.normal.background = CreateOutlineTexture();
            inputOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

            inputGhostStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputGhostStyle.font = customFont;
            inputGhostStyle.normal.background = transparentTexture;
            inputGhostStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f));

            inputFocusedStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputFocusedStyle.font = customFont;
            inputFocusedStyle.normal.background = inputFocusedTexture;
            inputFocusedStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);

            inputDisabledStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputDisabledStyle.font = customFont;
            inputDisabledStyle.normal.textColor = theme.InputDisabledFg;
            inputDisabledStyle.normal.background = CreateSolidTexture(theme.InputDisabledBg);

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

        /// <summary>
        /// Sets up the label styles.
        /// </summary>
        private void SetupLabelVariantStyles()
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

        private Dictionary<Color, Texture2D> solidColorTextureCache = new Dictionary<Color, Texture2D>();

        public Texture2D CreateSolidTexture(Color color)
        {
            if (solidColorTextureCache.TryGetValue(color, out var cachedTexture))
            {
                return cachedTexture;
            }

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            solidColorTextureCache[color] = texture;
            return texture;
        }

        private Dictionary<Color, Texture2D> outlineTextureCache = new Dictionary<Color, Texture2D>();

        private Texture2D CreateOutlineTexture()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            if (outlineTextureCache.TryGetValue(theme.AccentColor, out var cachedTexture))
            {
                return cachedTexture;
            }

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

        /// <summary>
        /// Sets up the progress bar styles.
        /// </summary>
        private void SetupProgressBarStyles()
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

        /// <summary>
        /// Sets up the separator styles.
        /// </summary>
        private void SetupSeparatorStyles()
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

        /// <summary>
        /// Sets up the tabs styles.
        /// </summary>
        private void SetupTabsStyles()
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

        /// <summary>
        /// Sets up the text area styles.
        /// </summary>
        private void SetupTextAreaVariantStyles()
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

        /// <summary>
        /// Sets up the checkbox styles.
        /// </summary>
        private void SetupCheckboxStyles()
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

        /// <summary>
        /// Sets up the switch styles.
        /// </summary>
        private void SetupSwitchStyles()
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

        /// <summary>
        /// Sets up the badge styles.
        /// </summary>
        private void SetupBadgeStyles()
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

        /// <summary>
        /// Sets up the alert styles.
        /// </summary>
        private void SetupAlertStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            alertDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            Color defaultBg = theme.AlertDefaultBg;
            Color defaultFg = theme.AlertDefaultFg;
            alertDefaultStyle.normal.background = CreateSolidTexture(defaultBg);
            alertDefaultStyle.normal.textColor = defaultFg;
            alertDefaultStyle.hover.background = CreateSolidTexture(defaultBg);
            alertDefaultStyle.border = GetBorderOffset(6f);
            alertDefaultStyle.padding = GetSpacingOffset(16f, 12f);

            alertDestructiveStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            Color destructiveColor = theme.AlertDestructiveFg;
            Color destructiveBorder = theme.AlertDestructiveBg;
            alertDestructiveStyle.normal.background = CreateSolidTexture(destructiveBorder);
            alertDestructiveStyle.normal.textColor = destructiveColor;
            alertDestructiveStyle.hover.background = CreateSolidTexture(destructiveBorder);
            alertDestructiveStyle.border = GetBorderOffset(6f);
            alertDestructiveStyle.padding = GetSpacingOffset(16f, 12f);

            alertTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                alertTitleStyle.font = customFont;
            alertTitleStyle.fontSize = GetScaledFontSize(1.125f);
            alertTitleStyle.fontStyle = FontStyle.Normal;
            alertTitleStyle.normal.textColor = defaultFg;
            alertTitleStyle.margin = GetSpacingOffset(0f, 4f);

            alertDescriptionStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                alertDescriptionStyle.font = customFont;
            alertDescriptionStyle.fontSize = GetScaledFontSize(1.0f);
            alertDescriptionStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.PrimaryColor, 0.3f);
            alertDescriptionStyle.wordWrap = true;
        }

        /// <summary>
        /// Sets up the avatar styles.
        /// </summary>
        private void SetupAvatarStyles()
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

        private int GetAvatarFontSize(AvatarSize size)
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

        private RectOffset GetAvatarBorder(AvatarShape shape, AvatarSize size)
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

        /// <summary>
        /// Sets up the skeleton styles.
        /// </summary>
        private void SetupSkeletonStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            skeletonStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            skeletonStyle.normal.background = CreateSolidTexture(theme.SkeletonBg);
            skeletonStyle.border = GetBorderOffset(4f);
            skeletonStyle.padding = GetSpacingOffset(0f, 0f);

            skeletonRoundedStyle = new UnityHelpers.GUIStyle(skeletonStyle);
            skeletonRoundedStyle.border = GetBorderOffset(8f);

            skeletonCircularStyle = new UnityHelpers.GUIStyle(skeletonStyle);
            skeletonCircularStyle.border = GetBorderOffset(9999f);
        }

        /// <summary>
        /// Sets up the table styles
        /// </summary>
        private void SetupTableStyles()
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

        private void SetupCalendarStyles()
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

        private void SetupDropdownMenuStyles()
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

        private void SetupPopoverStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            popoverContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            popoverContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
            popoverContentStyle.border = GetBorderOffset(6f);
            popoverContentStyle.padding = GetSpacingOffset(12f, 12f);
        }

        private void SetupScrollAreaStyles()
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

        private void SetupSelectStyles()
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

        private void SetupDatePickerStyles()
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

        private void SetupDialogStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            dialogOverlayStyle = new UnityHelpers.GUIStyle();
            dialogOverlayStyle.normal.background = CreateSolidTexture(new Color(0, 0, 0, 0.5f));

            dialogContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            dialogContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
            dialogContentStyle.border = new UnityHelpers.RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius);
            dialogContentStyle.padding = new UnityHelpers.RectOffset(20, 20, 20, 20);

            dialogTitleStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                dialogTitleStyle.font = customFont;
            dialogTitleStyle.fontSize = guiHelper.fontSize + 4;
            dialogTitleStyle.fontStyle = FontStyle.Bold;
            dialogTitleStyle.normal.textColor = theme.TextColor;
            dialogTitleStyle.wordWrap = true;

            dialogDescriptionStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                dialogDescriptionStyle.font = customFont;
            dialogDescriptionStyle.fontSize = guiHelper.fontSize;
            dialogDescriptionStyle.normal.textColor = theme.MutedColor;
            dialogDescriptionStyle.wordWrap = true;
        }
        #endregion

        #region Style Getters
        /// <summary>
        /// Gets the button style for the given variant and size.
        /// </summary>
        private Dictionary<(ButtonVariant, ButtonSize), GUIStyle> buttonStyleCache = new Dictionary<(ButtonVariant, ButtonSize), GUIStyle>();

        public GUIStyle GetButtonStyle(ButtonVariant variant, ButtonSize size)
        {
            if (buttonStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle = null;
            switch (variant)
            {
                case ButtonVariant.Default:
                    baseStyle = buttonDefaultStyle;
                    break;
                case ButtonVariant.Destructive:
                    baseStyle = buttonDestructiveStyle;
                    break;
                case ButtonVariant.Outline:
                    baseStyle = buttonOutlineStyle;
                    break;
                case ButtonVariant.Secondary:
                    baseStyle = buttonSecondaryStyle;
                    break;
                case ButtonVariant.Ghost:
                    baseStyle = buttonGhostStyle;
                    break;
                case ButtonVariant.Link:
                    baseStyle = buttonLinkStyle;
                    break;
                default:
                    baseStyle = buttonDefaultStyle;
                    break;
            }

            if (baseStyle == null)
                return GUI.skin.button;

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

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
                default:

                    break;
            }

            buttonStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the toggle style for the given variant and size.
        /// </summary>
        private Dictionary<(ToggleVariant, ToggleSize), GUIStyle> toggleStyleCache = new Dictionary<(ToggleVariant, ToggleSize), GUIStyle>();

        public GUIStyle GetToggleStyle(ToggleVariant variant, ToggleSize size)
        {
            if (toggleStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case ToggleVariant.Outline:
                    baseStyle = toggleOutlineStyle;
                    break;
                default:
                    baseStyle = toggleDefaultStyle;
                    break;
            }

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

        /// <summary>
        /// Gets the input style for the given variant, focus, and disabled state.
        /// </summary>
        private Dictionary<(InputVariant, bool, bool), GUIStyle> inputStyleCache = new Dictionary<(InputVariant, bool, bool), GUIStyle>();

        public GUIStyle GetInputStyle(InputVariant variant, bool focused = false, bool disabled = false)
        {
            if (inputStyleCache.TryGetValue((variant, focused, disabled), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

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

            GUIStyle style;
            switch (variant)
            {
                case InputVariant.Outline:
                    style = inputOutlineStyle ?? GUI.skin.textField;
                    break;
                case InputVariant.Ghost:
                    style = inputGhostStyle ?? GUI.skin.textField;
                    break;
                default:
                    style = inputDefaultStyle ?? GUI.skin.textField;
                    break;
            }

            inputStyleCache[(variant, focused, disabled)] = style;
            return style;
        }

        /// <summary>
        /// Gets the label style for the given variant.
        /// </summary>
        private Dictionary<LabelVariant, GUIStyle> labelStyleCache = new Dictionary<LabelVariant, GUIStyle>();

        public GUIStyle GetLabelStyle(LabelVariant variant)
        {
            if (labelStyleCache.TryGetValue(variant, out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle style;
            switch (variant)
            {
                case LabelVariant.Secondary:
                    style = labelSecondaryStyle ?? GUI.skin.label;
                    break;
                case LabelVariant.Muted:
                    style = labelMutedStyle ?? GUI.skin.label;
                    break;
                case LabelVariant.Destructive:
                    style = labelDestructiveStyle ?? GUI.skin.label;
                    break;
                default:
                    style = labelDefaultStyle ?? GUI.skin.label;
                    break;
            }

            labelStyleCache[variant] = style;
            return style;
        }

        /// <summary>
        /// Gets the password field style.
        /// </summary>
        public GUIStyle GetPasswordFieldStyle()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return passwordFieldStyle ?? GUI.skin.textField;
        }

        /// <summary>
        /// Gets the text area style for the given variant and focus state.
        /// </summary>
        private Dictionary<(TextAreaVariant, bool), GUIStyle> textAreaStyleCache = new Dictionary<(TextAreaVariant, bool), GUIStyle>();

        public GUIStyle GetTextAreaStyle(TextAreaVariant variant = TextAreaVariant.Default, bool focused = false)
        {
            if (textAreaStyleCache.TryGetValue((variant, focused), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case TextAreaVariant.Outline:
                    baseStyle = textAreaOutlineStyle ?? textAreaStyle ?? GUI.skin.textArea;
                    break;
                case TextAreaVariant.Ghost:
                    baseStyle = textAreaGhostStyle ?? textAreaStyle ?? GUI.skin.textArea;
                    break;
                default:
                    baseStyle = textAreaStyle ?? GUI.skin.textArea;
                    break;
            }

            var style = focused ? (textAreaFocusedStyle ?? baseStyle) : baseStyle;
            textAreaStyleCache[(variant, focused)] = style;
            return style;
        }

        /// <summary>
        /// Gets the progress bar style.
        /// </summary>
        public GUIStyle GetProgressBarStyle() => progressBarStyle ?? GUI.skin.box;

        /// <summary>
        /// Gets the progress bar background style.
        /// </summary>
        public GUIStyle GetProgressBarBackgroundStyle() => progressBarBackgroundStyle ?? GUI.skin.box;

        /// <summary>
        /// Gets the progress bar fill style.
        /// </summary>
        public GUIStyle GetProgressBarFillStyle() => progressBarFillStyle ?? GUI.skin.box;

        /// <summary>
        /// Gets the separator style for the given orientation.
        /// </summary>
        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return orientation == SeparatorOrientation.Horizontal ? (separatorHorizontalStyle ?? GUI.skin.box) : (separatorVerticalStyle ?? GUI.skin.box);
        }

        /// <summary>
        /// Gets the tabs list style.
        /// </summary>
        public GUIStyle GetTabsListStyle() => tabsListStyle ?? GUI.skin.box;

        /// <summary>
        /// Gets the tabs trigger style for the given active state.
        /// </summary>
        public GUIStyle GetTabsTriggerStyle(bool active = false)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) : (tabsTriggerStyle ?? GUI.skin.button);
        }

        /// <summary>
        /// Gets the tabs content style.
        /// </summary>
        public GUIStyle GetTabsContentStyle() => tabsContentStyle ?? GUIStyle.none;

        /// <summary>
        /// Gets the glow texture.
        /// </summary>
        public Texture2D GetGlowTexture() => glowTexture;

        /// <summary>
        /// Gets the particle texture.
        /// </summary>
        public Texture2D GetParticleTexture() => particleTexture;

        /// <summary>
        /// Gets the input background texture.
        /// </summary>
        public Texture2D GetInputBackgroundTexture() => inputBackgroundTexture;

        /// <summary>
        /// Gets the input focused texture.
        /// </summary>
        public Texture2D GetInputFocusedTexture() => inputFocusedTexture;

        /// <summary>
        /// Gets the transparent texture.
        /// </summary>
        public Texture2D GetTransparentTexture() => transparentTexture;

        /// <summary>
        /// Gets the progress bar background texture.
        /// </summary>
        public Texture2D GetProgressBarBackgroundTexture() => progressBarBackgroundTexture;

        /// <summary>
        /// Gets the progress bar fill texture.
        /// </summary>
        public Texture2D GetProgressBarFillTexture() => progressBarFillTexture;

        /// <summary>
        /// Gets the checkbox style for the given variant and size.
        /// </summary>
        private Dictionary<(CheckboxVariant, CheckboxSize), GUIStyle> checkboxStyleCache = new Dictionary<(CheckboxVariant, CheckboxSize), GUIStyle>();

        public GUIStyle GetCheckboxStyle(CheckboxVariant variant, CheckboxSize size)
        {
            if (checkboxStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case CheckboxVariant.Outline:
                    baseStyle = checkboxOutlineStyle;
                    break;
                case CheckboxVariant.Ghost:
                    baseStyle = checkboxGhostStyle;
                    break;
                default:
                    baseStyle = checkboxDefaultStyle;
                    break;
            }

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);

            switch (size)
            {
                case CheckboxSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case CheckboxSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
                default:

                    break;
            }

            checkboxStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the switch style for the given variant and size.
        /// </summary>
        private Dictionary<(SwitchVariant, SwitchSize), GUIStyle> switchStyleCache = new Dictionary<(SwitchVariant, SwitchSize), GUIStyle>();

        public GUIStyle GetSwitchStyle(SwitchVariant variant, SwitchSize size)
        {
            if (switchStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case SwitchVariant.Outline:
                    baseStyle = switchOutlineStyle;
                    break;
                case SwitchVariant.Ghost:
                    baseStyle = switchGhostStyle;
                    break;
                default:
                    baseStyle = switchDefaultStyle;
                    break;
            }

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);

            switch (size)
            {
                case SwitchSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case SwitchSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
                default:

                    break;
            }

            switchStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the badge style for the given variant and size.
        /// </summary>
        private Dictionary<(BadgeVariant, BadgeSize), GUIStyle> badgeStyleCache = new Dictionary<(BadgeVariant, BadgeSize), GUIStyle>();

        public GUIStyle GetBadgeStyle(BadgeVariant variant, BadgeSize size)
        {
            if (badgeStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case BadgeVariant.Secondary:
                    baseStyle = badgeSecondaryStyle;
                    break;
                case BadgeVariant.Destructive:
                    baseStyle = badgeDestructiveStyle;
                    break;
                case BadgeVariant.Outline:
                    baseStyle = badgeOutlineStyle;
                    break;
                default:
                    baseStyle = badgeDefaultStyle;
                    break;
            }

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);

            switch (size)
            {
                case BadgeSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case BadgeSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
                default:

                    break;
            }

            badgeStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the alert style for the given variant and type.
        /// </summary>
        private Dictionary<(AlertVariant, AlertType), GUIStyle> alertStyleCache = new Dictionary<(AlertVariant, AlertType), GUIStyle>();

        public GUIStyle GetAlertStyle(AlertVariant variant, AlertType type)
        {
            if (alertStyleCache.TryGetValue((variant, type), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle baseStyle = variant == AlertVariant.Destructive ? alertDestructiveStyle : alertDefaultStyle;
            var style = baseStyle ?? GUI.skin.box;
            alertStyleCache[(variant, type)] = style;
            return style;
        }

        /// <summary>
        /// Gets the alert title style for the given type.
        /// </summary>
        public GUIStyle GetAlertTitleStyle(AlertType type)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return alertTitleStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the alert description style for the given type.
        /// </summary>
        public GUIStyle GetAlertDescriptionStyle(AlertType type)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return alertDescriptionStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the avatar style for the given size and shape.
        /// </summary>
        private Dictionary<(AvatarSize, AvatarShape), GUIStyle> avatarStyleCache = new Dictionary<(AvatarSize, AvatarShape), GUIStyle>();

        public GUIStyle GetAvatarStyle(AvatarSize size, AvatarShape shape)
        {
            if (avatarStyleCache.TryGetValue((size, shape), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle = avatarStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);

            int avatarSizeValue = 0;
            switch (size)
            {
                case AvatarSize.Small:
                    avatarSizeValue = Mathf.RoundToInt(32 * guiHelper.uiScale);
                    break;
                case AvatarSize.Large:
                    avatarSizeValue = Mathf.RoundToInt(48 * guiHelper.uiScale);
                    break;
                default:
                    avatarSizeValue = Mathf.RoundToInt(40 * guiHelper.uiScale);
                    break;
            }
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

        /// <summary>
        /// Gets the avatar fallback style for the given size and shape.
        /// </summary>
        /// <summary>
        /// Gets the skeleton style for the given variant and size.
        /// </summary>
        private Dictionary<(SkeletonVariant, SkeletonSize), GUIStyle> skeletonStyleCache = new Dictionary<(SkeletonVariant, SkeletonSize), GUIStyle>();

        public GUIStyle GetSkeletonStyle(SkeletonVariant variant, SkeletonSize size)
        {
            if (skeletonStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case SkeletonVariant.Rounded:
                    baseStyle = skeletonRoundedStyle;
                    break;
                case SkeletonVariant.Circular:
                    baseStyle = skeletonCircularStyle;
                    break;
                default:
                    baseStyle = skeletonStyle;
                    break;
            }

            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle);

            switch (size)
            {
                case SkeletonSize.Small:
                    sizedStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);
                    break;
                case SkeletonSize.Large:

                    break;
                default:

                    break;
            }

            skeletonStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the table style for the given variant and size.
        /// </summary>
        private Dictionary<(TableVariant, TableSize), GUIStyle> tableStyleCache = new Dictionary<(TableVariant, TableSize), GUIStyle>();

        public GUIStyle GetTableStyle(TableVariant variant, TableSize size)
        {
            if (tableStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle;
            switch (variant)
            {
                case TableVariant.Striped:
                    baseStyle = tableStripedStyle;
                    break;
                case TableVariant.Bordered:
                    baseStyle = tableBorderedStyle;
                    break;
                case TableVariant.Hover:
                    baseStyle = tableHoverStyle;
                    break;
                default:
                    baseStyle = tableStyle;
                    break;
            }

            var style = baseStyle ?? GUI.skin.box;
            tableStyleCache[(variant, size)] = style;
            return style;
        }

        /// <summary>
        /// Gets the table header style for the given variant and size.
        /// </summary>
        public GUIStyle GetTableHeaderStyle(TableVariant variant, TableSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return tableHeaderStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the table cell style for the given variant and size.
        /// </summary>
        public GUIStyle GetTableCellStyle(TableVariant variant, TableSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return tableCellStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the calendar style for the given variant and size.
        /// </summary>
        private Dictionary<(CalendarVariant, CalendarSize), GUIStyle> calendarStyleCache = new Dictionary<(CalendarVariant, CalendarSize), GUIStyle>();

        public GUIStyle GetCalendarStyle(CalendarVariant variant, CalendarSize size)
        {
            if (calendarStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(calendarStyle);

            switch (size)
            {
                case CalendarSize.Small:
                    sizedStyle.fontSize = guiHelper.fontSize - 2;
                    break;
                case CalendarSize.Large:
                    sizedStyle.fontSize = guiHelper.fontSize + 2;
                    break;
                default:

                    break;
            }

            calendarStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the dropdown menu style for the given variant and size.
        /// </summary>
        private Dictionary<(DropdownMenuVariant, DropdownMenuSize), GUIStyle> dropdownMenuStyleCache = new Dictionary<(DropdownMenuVariant, DropdownMenuSize), GUIStyle>();

        public GUIStyle GetDropdownMenuStyle(DropdownMenuVariant variant, DropdownMenuSize size)
        {
            if (dropdownMenuStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;
            var style = dropdownMenuContentStyle;
            dropdownMenuStyleCache[(variant, size)] = style;
            return style;
        }

        /// <summary>
        /// Gets the scroll area style for the given variant and size.
        /// </summary>
        private Dictionary<(ScrollAreaVariant, ScrollAreaSize), GUIStyle> scrollAreaStyleCache = new Dictionary<(ScrollAreaVariant, ScrollAreaSize), GUIStyle>();

        public GUIStyle GetScrollAreaStyle(ScrollAreaVariant variant, ScrollAreaSize size)
        {
            if (scrollAreaStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(scrollAreaStyle);

            switch (size)
            {
                case ScrollAreaSize.Small:
                    sizedStyle.fixedWidth = 10;
                    break;
                case ScrollAreaSize.Large:
                    sizedStyle.fixedWidth = 20;
                    break;
                default:

                    break;
            }

            scrollAreaStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        /// <summary>
        /// Gets the select style for the given variant and size.
        /// </summary>
        private Dictionary<(SelectVariant, SelectSize), GUIStyle> selectStyleCache = new Dictionary<(SelectVariant, SelectSize), GUIStyle>();

        public GUIStyle GetSelectStyle(SelectVariant variant, SelectSize size)
        {
            if (selectStyleCache.TryGetValue((variant, size), out var cachedStyle))
            {
                return cachedStyle;
            }

            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(selectContentStyle);

            switch (size)
            {
                case SelectSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                    break;
                case SelectSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                    break;
                default:

                    break;
            }

            selectStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        public GUIStyle GetSelectTriggerStyle()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return selectTriggerStyle;
        }

        public GUIStyle GetSelectItemStyle()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return selectItemStyle;
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Cleans up the textures created by the style manager.
        /// </summary>
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
            if (alertTexture)
                Object.Destroy(alertTexture);
            if (avatarTexture)
                Object.Destroy(avatarTexture);
            if (skeletonTexture)
                Object.Destroy(skeletonTexture);
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
            {
                if (texture)
                    Object.Destroy(texture);
            }
            solidColorTextureCache.Clear();

            foreach (var texture in outlineButtonTextureCache.Values)
            {
                if (texture)
                    Object.Destroy(texture);
            }
            outlineButtonTextureCache.Clear();

            foreach (var texture in outlineTextureCache.Values)
            {
                if (texture)
                    Object.Destroy(texture);
            }
            outlineTextureCache.Clear();
        }
        #endregion
    }
}
