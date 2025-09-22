using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using shadcnui;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP
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
    /// Variants for the Slider component.
    /// </summary>
    public enum SliderVariant
    {
        Default,
        Range,
        Vertical,
        Disabled,
    }

    /// <summary>
    /// Sizes for the Slider component.
    /// </summary>
    public enum SliderSize
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
    /// Variants for the Dialog component.
    /// </summary>
    public enum DialogVariant
    {
        Default,
    }

    /// <summary>
    /// Sizes for the Dialog component.
    /// </summary>
    public enum DialogSize
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
    /// Variants for the Popover component.
    /// </summary>
    public enum PopoverVariant
    {
        Default,
    }

    /// <summary>
    /// Sizes for the Popover component.
    /// </summary>
    public enum PopoverSize
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
    public class GUIStyleManager
    {
        private GUIHelper guiHelper;

        public Font customFont;

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

        // DropdownMenu Styles
        public GUIStyle dropdownMenuContentStyle;
        public GUIStyle dropdownMenuItemStyle;
        public GUIStyle dropdownMenuSeparatorStyle;

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


        public GUIStyleManager(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void SetCustomFont(byte[] fontData, string fontName = "CustomFont.ttf")
        {
            if (fontData == null || fontData.Length == 0)
            {
                Debug.LogWarning("Font data is null or empty.");
                return;
            }

            string tempPath = Path.Combine(Application.temporaryCachePath, fontName);
            File.WriteAllBytes(tempPath, fontData);

#if IL2CPP
            // it does not work for il2cpp idk why :sob
#else
            Font loadedFont = new Font(tempPath);

            if (loadedFont != null)
            {
                customFont = loadedFont;
                Debug.Log($"Custom font '{fontName}' loaded successfully from bytes.");
            }
            else
            {
                Debug.LogWarning($"Failed to create font from bytes for '{fontName}'.");
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
        }
        #endregion

        #region Texture Creation
        /// <summary>
        /// Creates the custom textures used by the GUI styles.
        /// </summary>
        private void CreateCustomTextures()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            gradientTexture = new Texture2D(1, 100);
            for (int i = 0; i < 100; i++)
            {
                float t = i / 99f;
                Color startColor,
                    endColor;
                if (theme.Name == "Custom")
                {
                    startColor = Color.Lerp(guiHelper.primaryColor, Color.black, 0.8f);
                    endColor = Color.Lerp(guiHelper.secondaryColor, Color.black, 0.6f);
                }
                else
                {
                    startColor = theme.PrimaryColor;
                    endColor = theme.SecondaryColor;
                }
                Color gradColor = Color.Lerp(startColor, endColor, t);
                gradientTexture.SetPixel(0, i, gradColor);
            }
            gradientTexture.Apply();

            cardBackgroundTexture = new Texture2D(1, 1);
            cardBackgroundTexture.SetPixel(0, 0, theme.Name == "Custom" ? new Color(guiHelper.primaryColor.r, guiHelper.primaryColor.g, guiHelper.primaryColor.b, guiHelper.backgroundAlpha) : theme.CardBg);
            cardBackgroundTexture.Apply();

            inputBackgroundTexture = new Texture2D(1, 1);
            inputBackgroundTexture.SetPixel(0, 0, theme.Name == "Custom" ? new Color(guiHelper.primaryColor.r * 0.3f, guiHelper.primaryColor.g * 0.3f, guiHelper.primaryColor.b * 0.3f, 0.8f) : theme.InputBg);
            inputBackgroundTexture.Apply();

            inputFocusedTexture = new Texture2D(1, 1);
            inputFocusedTexture.SetPixel(0, 0, theme.Name == "Custom" ? new Color(guiHelper.accentColor.r * 0.3f, guiHelper.accentColor.g * 0.3f, guiHelper.accentColor.b * 0.3f, 0.9f) : theme.InputFocusedBg);
            inputFocusedTexture.Apply();

            outlineTexture = new Texture2D(4, 4);
            Color borderColor = theme.Name == "Custom" ? guiHelper.accentColor : theme.ButtonOutlineBorder;
            Color fillColor = new Color(0f, 0f, 0f, 0f);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (x == 0 || y == 0 || x == 3 || y == 3)
                        outlineTexture.SetPixel(x, y, borderColor);
                    else
                        outlineTexture.SetPixel(x, y, fillColor);
                }
            }
            outlineTexture.Apply();

            transparentTexture = new Texture2D(1, 1);
            transparentTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
            transparentTexture.Apply();

            glowTexture = new Texture2D(32, 32);
            Vector2 center = new Vector2(16, 16);
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    float alpha = Mathf.Clamp01(1f - (distance / 16f));
                    alpha = Mathf.Pow(alpha, 2f);
                    Color glowColor = theme.AccentColor;
                    glowTexture.SetPixel(x, y, new Color(glowColor.r, glowColor.g, glowColor.b, alpha * guiHelper.glowIntensity));
                }
            }
            glowTexture.Apply();

            particleTexture = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Color particleColor = Color.Lerp(theme.AccentColor, Color.white, 0.5f);
                    particleTexture.SetPixel(x, y, particleColor);
                }
            }
            particleTexture.Apply();

            progressBarBackgroundTexture = new Texture2D(1, 1);
            progressBarBackgroundTexture.SetPixel(0, 0, new Color(theme.PrimaryColor.r * 0.2f, theme.PrimaryColor.g * 0.2f, theme.PrimaryColor.b * 0.2f, 0.2f));
            progressBarBackgroundTexture.Apply();

            progressBarFillTexture = new Texture2D(1, 1);
            progressBarFillTexture.SetPixel(0, 0, theme.PrimaryColor);
            progressBarFillTexture.Apply();

            separatorTexture = new Texture2D(1, 1);
            separatorTexture.SetPixel(0, 0, theme.SeparatorColor);
            separatorTexture.Apply();

            tabsBackgroundTexture = new Texture2D(1, 1);
            tabsBackgroundTexture.SetPixel(0, 0, theme.TabsBg);
            tabsBackgroundTexture.Apply();

            tabsActiveTexture = new Texture2D(1, 1);
            tabsActiveTexture.SetPixel(0, 0, theme.TabsTriggerActiveBg);
            tabsActiveTexture.Apply();

            checkboxTexture = new Texture2D(1, 1);
            checkboxTexture.SetPixel(0, 0, theme.CheckboxBg);
            checkboxTexture.Apply();

            checkboxCheckedTexture = new Texture2D(1, 1);
            checkboxCheckedTexture.SetPixel(0, 0, theme.CheckboxCheckedBg);
            checkboxCheckedTexture.Apply();

            switchTexture = new Texture2D(1, 1);
            switchTexture.SetPixel(0, 0, theme.SwitchBg);
            switchTexture.Apply();

            switchOnTexture = new Texture2D(1, 1);
            switchOnTexture.SetPixel(0, 0, theme.SwitchOnBg);
            switchOnTexture.Apply();

            switchOffTexture = new Texture2D(1, 1);
            switchOffTexture.SetPixel(0, 0, theme.SwitchOffBg);
            switchOffTexture.Apply();

            badgeTexture = new Texture2D(1, 1);
            badgeTexture.SetPixel(0, 0, theme.BadgeBg);
            badgeTexture.Apply();

            alertTexture = new Texture2D(1, 1);
            alertTexture.SetPixel(0, 0, theme.AlertDefaultBg);
            alertTexture.Apply();

            avatarTexture = new Texture2D(1, 1);
            avatarTexture.SetPixel(0, 0, theme.AvatarBg);
            avatarTexture.Apply();

            skeletonTexture = new Texture2D(1, 1);
            skeletonTexture.SetPixel(0, 0, theme.SkeletonBg);
            skeletonTexture.Apply();

            tableTexture = new Texture2D(1, 1);
            tableTexture.SetPixel(0, 0, theme.TableBg);
            tableTexture.Apply();

            tableHeaderTexture = new Texture2D(1, 1);
            tableHeaderTexture.SetPixel(0, 0, theme.TableHeaderBg);
            tableHeaderTexture.Apply();

            tableCellTexture = new Texture2D(1, 1);
            tableCellTexture.SetPixel(0, 0, theme.TableCellBg);
            tableCellTexture.Apply();

            calendarBackgroundTexture = CreateSolidTexture(theme.CardBg);
            calendarHeaderTexture = CreateSolidTexture(theme.CardBg);
            calendarDayTexture = CreateSolidTexture(theme.CardBg);
            calendarDaySelectedTexture = CreateSolidTexture(theme.AccentColor);

            dropdownMenuContentTexture = CreateSolidTexture(theme.CardBg);

            popoverContentTexture = CreateSolidTexture(theme.CardBg);

            scrollAreaThumbTexture = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.white, 0.2f));
            scrollAreaTrackTexture = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.black, 0.1f));

            selectTriggerTexture = CreateSolidTexture(theme.InputBg);
            selectContentTexture = CreateSolidTexture(theme.CardBg);
        }
        #endregion

        #region Style Setup Methods
        /// <summary>
        /// Sets up the animated styles.
        /// </summary>
        private void SetupAnimatedStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            animatedBoxStyle = new GUIStyle(GUI.skin.box);
            animatedBoxStyle.normal.background = gradientTexture;
            animatedBoxStyle.border = new RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, 5);
            animatedBoxStyle.padding = new RectOffset(15, 15, 15, 15);

            animatedButtonStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                animatedButtonStyle.font = customFont;
            animatedButtonStyle.fontSize = guiHelper.fontSize;
            animatedButtonStyle.fontStyle = FontStyle.Bold;
            animatedButtonStyle.alignment = TextAnchor.MiddleCenter;
            animatedButtonStyle.normal.textColor = Color.Lerp(Color.white, theme.AccentColor, 0.3f);
            animatedButtonStyle.hover.textColor = theme.AccentColor;

            colorPresetStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                colorPresetStyle.font = customFont;
            colorPresetStyle.fontSize = Mathf.RoundToInt(guiHelper.fontSize * 0.9f);
            colorPresetStyle.fontStyle = FontStyle.Bold;
            colorPresetStyle.alignment = TextAnchor.MiddleCenter;

            animatedInputStyle = new GUIStyle(GUI.skin.textField);
            if (customFont != null)
                animatedInputStyle.font = customFont;
            animatedInputStyle.fontSize = guiHelper.fontSize + 1;
            animatedInputStyle.padding = new RectOffset(8, 8, 4, 4);
            animatedInputStyle.normal.textColor = Color.white;
            animatedInputStyle.focused.textColor = theme.AccentColor;

            glowLabelStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                glowLabelStyle.font = customFont;
            glowLabelStyle.fontSize = guiHelper.fontSize;
            Color labelColor = Color.Lerp(Color.white, theme.AccentColor, 0.2f);
            glowLabelStyle.normal.textColor = labelColor;

            titleStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                titleStyle.font = customFont;
            titleStyle.fontSize = guiHelper.fontSize + 4;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            Color titleColor = theme.AccentColor;
            titleStyle.normal.textColor = titleColor;

            sectionHeaderStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                sectionHeaderStyle.font = customFont;
            sectionHeaderStyle.fontSize = guiHelper.fontSize + 2;
            sectionHeaderStyle.fontStyle = FontStyle.Bold;
            Color headerColor = Color.Lerp(theme.AccentColor, Color.white, 0.3f);
            sectionHeaderStyle.normal.textColor = headerColor;
        }

        /// <summary>
        /// Sets up the card styles.
        /// </summary>
        private void SetupCardStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            cardStyle = new GUIStyle(GUI.skin.box);
            cardStyle.normal.background = cardBackgroundTexture;
            cardStyle.border = new RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius);
            cardStyle.padding = new RectOffset(0, 0, 0, 0);
            cardStyle.margin = new RectOffset(0, 0, 5, 5);

            cardHeaderStyle = new GUIStyle();
            cardHeaderStyle.padding = new RectOffset(16, 16, 16, 8);
            cardHeaderStyle.margin = new RectOffset(0, 0, 0, 0);

            cardTitleStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                cardTitleStyle.font = customFont;
            cardTitleStyle.fontSize = guiHelper.fontSize + 4;
            cardTitleStyle.fontStyle = FontStyle.Bold;
            cardTitleStyle.normal.textColor = theme.CardTitle;
            cardTitleStyle.wordWrap = true;
            cardTitleStyle.margin = new RectOffset(0, 0, 0, 4);

            cardDescriptionStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                cardDescriptionStyle.font = customFont;
            cardDescriptionStyle.fontSize = guiHelper.fontSize - 1;
            cardDescriptionStyle.normal.textColor = theme.CardDescription;
            cardDescriptionStyle.wordWrap = true;

            cardContentStyle = new GUIStyle();
            cardContentStyle.padding = new RectOffset(16, 16, 8, 16);

            cardFooterStyle = new GUIStyle();
            cardFooterStyle.padding = new RectOffset(16, 16, 8, 16);
        }

        /// <summary>
        /// Sets up the button styles.
        /// </summary>
        private void SetupButtonVariantStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            buttonDefaultStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonDefaultStyle.font = customFont;
            buttonDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonDefaultStyle.fontStyle = FontStyle.Normal;
            buttonDefaultStyle.alignment = TextAnchor.MiddleCenter;
            buttonDefaultStyle.padding = GetScaledPadding(16, 8);
            buttonDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            buttonDefaultStyle.fixedHeight = Mathf.RoundToInt(36 * guiHelper.uiScale);

            Color primaryBg = theme.ButtonPrimaryBg;
            Color primaryFg = theme.ButtonPrimaryFg;
            buttonDefaultStyle.normal.background = CreateSolidTexture(primaryBg);
            buttonDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.black, 0.1f));
            buttonDefaultStyle.normal.textColor = primaryFg;
            buttonDefaultStyle.hover.textColor = primaryFg;
            buttonDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.black, 0.2f));
            buttonDefaultStyle.active.textColor = primaryFg;

            buttonDestructiveStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonDestructiveStyle.font = customFont;
            buttonDestructiveStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonDestructiveStyle.fontStyle = FontStyle.Normal;
            buttonDestructiveStyle.alignment = TextAnchor.MiddleCenter;
            buttonDestructiveStyle.padding = GetScaledPadding(16, 8);
            buttonDestructiveStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color destructiveBg = theme.ButtonDestructiveBg;
            Color destructiveFg = theme.ButtonDestructiveFg;
            buttonDestructiveStyle.normal.background = CreateSolidTexture(destructiveBg);
            buttonDestructiveStyle.hover.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.1f));
            buttonDestructiveStyle.normal.textColor = destructiveFg;
            buttonDestructiveStyle.hover.textColor = destructiveFg;
            buttonDestructiveStyle.active.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.2f));
            buttonDestructiveStyle.active.textColor = destructiveFg;

            buttonOutlineStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonOutlineStyle.font = customFont;
            buttonOutlineStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonOutlineStyle.fontStyle = FontStyle.Normal;
            buttonOutlineStyle.alignment = TextAnchor.MiddleCenter;
            buttonOutlineStyle.padding = GetScaledPadding(16, 8);
            buttonOutlineStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color outlineBorder = theme.ButtonOutlineBorder;
            Color outlineBg = theme.ButtonOutlineBg;
            Color outlineFg = theme.ButtonOutlineFg;
            Color outlineHoverBg = theme.ButtonOutlineHoverBg;
            Color outlineHoverFg = theme.ButtonOutlineHoverFg;

            buttonOutlineStyle.normal.background = CreateOutlineButtonTexture(outlineBg, outlineBorder);
            buttonOutlineStyle.hover.background = CreateSolidTexture(outlineHoverBg);
            buttonOutlineStyle.normal.textColor = outlineFg;
            buttonOutlineStyle.hover.textColor = outlineHoverFg;
            buttonOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(outlineHoverBg, Color.black, 0.2f));
            buttonOutlineStyle.active.textColor = outlineHoverFg;

            buttonSecondaryStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonSecondaryStyle.font = customFont;
            buttonSecondaryStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonSecondaryStyle.fontStyle = FontStyle.Normal;
            buttonSecondaryStyle.alignment = TextAnchor.MiddleCenter;
            buttonSecondaryStyle.padding = GetScaledPadding(16, 8);
            buttonSecondaryStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color secondaryBg = theme.ButtonSecondaryBg;
            Color secondaryFg = theme.ButtonSecondaryFg;
            buttonSecondaryStyle.normal.background = CreateSolidTexture(secondaryBg);
            buttonSecondaryStyle.hover.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.white, 0.2f));
            buttonSecondaryStyle.normal.textColor = secondaryFg;
            buttonSecondaryStyle.hover.textColor = secondaryFg;
            buttonSecondaryStyle.active.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.black, 0.2f));
            buttonSecondaryStyle.active.textColor = secondaryFg;

            buttonGhostStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonGhostStyle.font = customFont;
            buttonGhostStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonGhostStyle.fontStyle = FontStyle.Normal;
            buttonGhostStyle.alignment = TextAnchor.MiddleCenter;
            buttonGhostStyle.padding = GetScaledPadding(16, 8);
            buttonGhostStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color ghostFg = theme.ButtonGhostFg;
            Color ghostHoverBg = theme.ButtonGhostHoverBg;
            Color ghostHoverFg = theme.ButtonGhostHoverFg;

            buttonGhostStyle.normal.background = transparentTexture;
            buttonGhostStyle.hover.background = CreateSolidTexture(ghostHoverBg);
            buttonGhostStyle.normal.textColor = ghostFg;
            buttonGhostStyle.hover.textColor = ghostHoverFg;
            buttonGhostStyle.active.background = CreateSolidTexture(Color.Lerp(ghostHoverBg, Color.black, 0.2f));
            buttonGhostStyle.active.textColor = ghostHoverFg;

            buttonLinkStyle = CreateBaseButtonStyle();
            if (customFont != null)
                buttonLinkStyle.font = customFont;
            buttonLinkStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonLinkStyle.fontStyle = FontStyle.Normal;
            buttonLinkStyle.alignment = TextAnchor.MiddleCenter;
            buttonLinkStyle.padding = GetScaledPadding(0, 0);
            buttonLinkStyle.border = new RectOffset(0, 0, 0, 0);

            Color linkColor = theme.ButtonLinkColor;
            Color linkHoverColor = theme.ButtonLinkHoverColor;

            buttonLinkStyle.normal.background = transparentTexture;
            buttonLinkStyle.hover.background = transparentTexture;
            buttonLinkStyle.normal.textColor = linkColor;
            buttonLinkStyle.hover.textColor = linkHoverColor;
            buttonLinkStyle.active.background = transparentTexture;
            buttonLinkStyle.active.textColor = Color.Lerp(linkHoverColor, Color.black, 0.2f);
        }

        private GUIStyle CreateBaseButtonStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.wordWrap = false;
            style.clipping = TextClipping.Clip;
            return style;
        }

        private RectOffset GetScaledPadding(int horizontal, int vertical)
        {
            return new RectOffset(Mathf.RoundToInt(horizontal * guiHelper.uiScale), Mathf.RoundToInt(horizontal * guiHelper.uiScale), Mathf.RoundToInt(vertical * guiHelper.uiScale), Mathf.RoundToInt(vertical * guiHelper.uiScale));
        }

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor, Color borderColor)
        {
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
            return texture;
        }

        /// <summary>
        /// Sets up the toggle styles.
        /// </summary>
        private void SetupToggleVariantStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            toggleDefaultStyle = new GUIStyle(GUI.skin.button);
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

            toggleOutlineStyle = new GUIStyle(toggleDefaultStyle);
            if (customFont != null)
                toggleOutlineStyle.font = customFont;
            toggleOutlineStyle.normal.background = CreateOutlineButtonTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f), theme.AccentColor);
            toggleOutlineStyle.border = new RectOffset(2, 2, 2, 2);
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

            inputDefaultStyle = new GUIStyle(GUI.skin.textField);
            if (customFont != null)
                inputDefaultStyle.font = customFont;
            inputDefaultStyle.fontSize = guiHelper.fontSize;
            inputDefaultStyle.padding = new RectOffset(12, 12, 4, 4);
            inputDefaultStyle.margin = new RectOffset(0, 0, 4, 4);
            inputDefaultStyle.border = new RectOffset(1, 1, 1, 1);
            inputDefaultStyle.normal.background = inputBackgroundTexture;
            inputDefaultStyle.normal.textColor = theme.InputFg;
            inputDefaultStyle.hover.background = inputBackgroundTexture;
            inputDefaultStyle.focused.background = inputFocusedTexture;
            inputDefaultStyle.focused.textColor = theme.InputFocusedFg;

            inputOutlineStyle = new GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputOutlineStyle.font = customFont;
            inputOutlineStyle.normal.background = CreateOutlineTexture();
            inputOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

            inputGhostStyle = new GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputGhostStyle.font = customFont;
            inputGhostStyle.normal.background = transparentTexture;
            inputGhostStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f));

            inputFocusedStyle = new GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputFocusedStyle.font = customFont;
            inputFocusedStyle.normal.background = inputFocusedTexture;
            inputFocusedStyle.border = new RectOffset(2, 2, 2, 2);

            inputDisabledStyle = new GUIStyle(inputDefaultStyle);
            if (customFont != null)
                inputDisabledStyle.font = customFont;
            inputDisabledStyle.normal.textColor = theme.InputDisabledFg;
            inputDisabledStyle.normal.background = CreateSolidTexture(theme.InputDisabledBg);

            passwordFieldStyle = new GUIStyle(inputDefaultStyle);
            if (customFont != null)
                passwordFieldStyle.font = customFont;
            passwordFieldStyle.fontSize = guiHelper.fontSize + 2;

            textAreaStyle = new GUIStyle(inputDefaultStyle);
            if (customFont != null)
                textAreaStyle.font = customFont;
            textAreaStyle.wordWrap = true;
            textAreaStyle.stretchHeight = true;
            textAreaStyle.padding = new RectOffset(12, 12, 8, 8);

            textAreaFocusedStyle = new GUIStyle(inputDefaultStyle);
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

            labelDefaultStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                labelDefaultStyle.font = customFont;
            labelDefaultStyle.fontSize = guiHelper.fontSize;
            labelDefaultStyle.fontStyle = FontStyle.Normal;
            labelDefaultStyle.normal.textColor = theme.TextColor;
            labelDefaultStyle.padding = new RectOffset(0, 0, 2, 2);

            labelSecondaryStyle = new GUIStyle(labelDefaultStyle);
            if (customFont != null)
                labelSecondaryStyle.font = customFont;
            labelSecondaryStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.SecondaryColor, 0.4f);

            labelMutedStyle = new GUIStyle(labelDefaultStyle);
            if (customFont != null)
                labelMutedStyle.font = customFont;
            labelMutedStyle.normal.textColor = new Color(0.6f, 0.6f, 0.7f);
            labelMutedStyle.fontSize = guiHelper.fontSize - 1;

            labelDestructiveStyle = new GUIStyle(labelDefaultStyle);
            if (customFont != null)
                labelDestructiveStyle.font = customFont;
            labelDestructiveStyle.normal.textColor = new Color(0.9f, 0.3f, 0.3f);
            labelDestructiveStyle.fontStyle = FontStyle.Bold;
        }

        public Texture2D CreateSolidTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private Texture2D CreateOutlineTexture()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
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
            return texture;
        }

        /// <summary>
        /// Sets up the progress bar styles.
        /// </summary>
        private void SetupProgressBarStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            progressBarStyle = new GUIStyle(GUI.skin.box);
            progressBarStyle.normal.background = progressBarBackgroundTexture;
            progressBarStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            progressBarStyle.padding = new RectOffset(0, 0, 0, 0);
            progressBarStyle.margin = new RectOffset(0, 0, 2, 2);

            progressBarBackgroundStyle = new GUIStyle();
            progressBarBackgroundStyle.normal.background = progressBarBackgroundTexture;

            progressBarFillStyle = new GUIStyle();
            progressBarFillStyle.normal.background = progressBarFillTexture;
        }

        /// <summary>
        /// Sets up the separator styles.
        /// </summary>
        private void SetupSeparatorStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            separatorHorizontalStyle = new GUIStyle();
            separatorHorizontalStyle.normal.background = CreateSolidTexture(theme.SeparatorColor);
            separatorHorizontalStyle.fixedHeight = Mathf.RoundToInt(1 * guiHelper.uiScale);
            separatorHorizontalStyle.stretchWidth = true;

            separatorVerticalStyle = new GUIStyle();
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

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);
            int padding = Mathf.RoundToInt(4 * guiHelper.uiScale);

            tabsListStyle = new GUIStyle();
            tabsListStyle.normal.background = CreateSolidTexture(theme.TabsBg);
            tabsListStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            tabsListStyle.padding = new RectOffset(padding, padding, padding, padding);
            tabsListStyle.margin = new RectOffset(0, 0, 2, 2);

            tabsTriggerStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                tabsTriggerStyle.font = customFont;
            tabsTriggerStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            tabsTriggerStyle.fontStyle = FontStyle.Bold;
            tabsTriggerStyle.alignment = TextAnchor.MiddleCenter;
            tabsTriggerStyle.padding = new RectOffset(Mathf.RoundToInt(12 * guiHelper.uiScale), Mathf.RoundToInt(12 * guiHelper.uiScale), Mathf.RoundToInt(4 * guiHelper.uiScale), Mathf.RoundToInt(4 * guiHelper.uiScale));
            tabsTriggerStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            tabsTriggerStyle.normal.background = transparentTexture;
            tabsTriggerStyle.normal.textColor = theme.TabsTriggerFg;
            tabsTriggerStyle.hover.background = transparentTexture;
            tabsTriggerStyle.active.background = transparentTexture;

            tabsTriggerActiveStyle = new GUIStyle(tabsTriggerStyle);
            if (customFont != null)
                tabsTriggerActiveStyle.font = customFont;
            tabsTriggerActiveStyle.normal.background = CreateSolidTexture(theme.TabsTriggerActiveBg);
            tabsTriggerActiveStyle.normal.textColor = theme.TabsTriggerActiveFg;
            tabsTriggerActiveStyle.hover.background = CreateSolidTexture(theme.TabsTriggerActiveBg);
            tabsTriggerActiveStyle.active.background = CreateSolidTexture(theme.TabsTriggerActiveBg);

            tabsContentStyle = new GUIStyle();
            tabsContentStyle.padding = new RectOffset(0, 0, Mathf.RoundToInt(8 * guiHelper.uiScale), 0);
        }

        /// <summary>
        /// Sets up the text area styles.
        /// </summary>
        private void SetupTextAreaVariantStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int padding = Mathf.RoundToInt(12 * guiHelper.uiScale);
            int verticalPadding = Mathf.RoundToInt(8 * guiHelper.uiScale);
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            textAreaStyle = new GUIStyle(GUI.skin.textArea);
            textAreaStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            textAreaStyle.padding = new RectOffset(padding, padding, verticalPadding, verticalPadding);
            textAreaStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            textAreaStyle.normal.background = inputBackgroundTexture;
            textAreaStyle.normal.textColor = theme.InputFg;
            textAreaStyle.focused.background = inputFocusedTexture;
            textAreaStyle.focused.textColor = theme.InputFocusedFg;
            textAreaStyle.hover.background = inputBackgroundTexture;
            textAreaStyle.wordWrap = true;
            textAreaStyle.stretchHeight = true;

            textAreaFocusedStyle = new GUIStyle(textAreaStyle);
            textAreaFocusedStyle.normal.background = inputFocusedTexture;
            textAreaFocusedStyle.border = new RectOffset(0, 0, 0, 0);

            textAreaOutlineStyle = new GUIStyle(textAreaStyle);
            textAreaOutlineStyle.normal.background = CreateOutlineTexture();
            textAreaOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

            textAreaGhostStyle = new GUIStyle(textAreaStyle);
            textAreaGhostStyle.normal.background = transparentTexture;
            textAreaGhostStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.PrimaryColor, Color.black, 0.1f));
        }

        /// <summary>
        /// Sets up the checkbox styles.
        /// </summary>
        private void SetupCheckboxStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            checkboxDefaultStyle = new GUIStyle(GUI.skin.toggle);
            if (customFont != null)
                checkboxDefaultStyle.font = customFont;
            checkboxDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            checkboxDefaultStyle.normal.background = checkboxTexture;
            checkboxDefaultStyle.onNormal.background = checkboxCheckedTexture;
            checkboxDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxBg, Color.white, 0.1f));
            checkboxDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.CheckboxBg, Color.black, 0.1f));
            checkboxDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.white, 0.1f));
            checkboxDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.black, 0.1f));
            checkboxDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color onBgColor = theme.ToggleOnBg;
            Color onHoverBgColor = Color.Lerp(onBgColor, Color.white, 0.1f);
            Color onActiveBgColor = Color.Lerp(onBgColor, Color.black, 0.1f);

            checkboxDefaultStyle.onNormal.background = CreateSolidTexture(onBgColor);
            checkboxDefaultStyle.onNormal.textColor = Color.white;
            checkboxDefaultStyle.onHover.background = CreateSolidTexture(onHoverBgColor);
            checkboxDefaultStyle.onHover.textColor = Color.white;
            checkboxDefaultStyle.onActive.background = CreateSolidTexture(onActiveBgColor);
            checkboxDefaultStyle.onActive.textColor = Color.white;

            checkboxOutlineStyle = new GUIStyle(checkboxDefaultStyle);
            if (customFont != null)
                checkboxOutlineStyle.font = customFont;
            checkboxOutlineStyle.normal.background = CreateOutlineTexture();
            checkboxOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.white, 0.1f));
            checkboxOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.black, 0.1f));
            checkboxOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.AccentColor, theme.AccentColor);
            checkboxOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.white, 0.1f));
            checkboxOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.CheckboxCheckedBg, Color.black, 0.1f));

            checkboxGhostStyle = new GUIStyle(checkboxDefaultStyle);
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

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            switchDefaultStyle = new GUIStyle(GUI.skin.toggle);
            if (customFont != null)
                switchDefaultStyle.font = customFont;
            switchDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            switchDefaultStyle.normal.background = switchOffTexture;
            switchDefaultStyle.onNormal.background = switchOnTexture;
            switchDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.SwitchOffBg, Color.white, 0.1f));
            switchDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.SwitchOffBg, Color.black, 0.1f));
            switchDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.SwitchOnBg, Color.white, 0.1f));
            switchDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.SwitchOnBg, Color.black, 0.1f));
            switchDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            switchOutlineStyle = new GUIStyle(switchDefaultStyle);
            if (customFont != null)
                switchOutlineStyle.font = customFont;
            switchOutlineStyle.normal.background = CreateOutlineTexture();
            switchOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.white, 0.1f));
            switchOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.black, 0.1f));
            switchOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.AccentColor, theme.AccentColor);
            switchOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.white, 0.1f));
            switchOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0, 0), Color.black, 0.1f));

            switchGhostStyle = new GUIStyle(switchDefaultStyle);
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

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            badgeDefaultStyle = new GUIStyle(GUI.skin.box);
            if (customFont != null)
                badgeDefaultStyle.font = customFont;
            badgeDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            badgeDefaultStyle.normal.background = CreateSolidTexture(theme.BadgeBg);
            badgeDefaultStyle.normal.textColor = theme.TextColor;
            badgeDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            badgeDefaultStyle.padding = new RectOffset(8, 8, 3, 3);
            badgeDefaultStyle.alignment = TextAnchor.MiddleCenter;

            badgeSecondaryStyle = new GUIStyle(badgeDefaultStyle);
            if (customFont != null)
                badgeSecondaryStyle.font = customFont;
            badgeSecondaryStyle.normal.background = CreateSolidTexture(theme.BadgeSecondaryBg);

            badgeDestructiveStyle = new GUIStyle(badgeDefaultStyle);
            if (customFont != null)
                badgeDestructiveStyle.font = customFont;
            badgeDestructiveStyle.normal.background = CreateSolidTexture(theme.BadgeDestructiveBg);

            badgeOutlineStyle = new GUIStyle(badgeDefaultStyle);
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

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);
            int horizontalPadding = Mathf.RoundToInt(16 * guiHelper.uiScale);
            int verticalPadding = Mathf.RoundToInt(12 * guiHelper.uiScale);

            alertDefaultStyle = new GUIStyle(GUI.skin.box);
            Color defaultBg = theme.AlertDefaultBg;
            Color defaultFg = theme.AlertDefaultFg;
            Color defaultBorder = theme.SeparatorColor;
            alertDefaultStyle.normal.background = CreateSolidTexture(defaultBg);
            alertDefaultStyle.normal.textColor = defaultFg;
            alertDefaultStyle.hover.background = CreateSolidTexture(defaultBg);
            alertDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            alertDefaultStyle.padding = new RectOffset(horizontalPadding, horizontalPadding, verticalPadding, verticalPadding);

            alertDestructiveStyle = new GUIStyle(GUI.skin.box);
            Color destructiveColor = theme.AlertDestructiveFg;
            Color destructiveBorder = theme.AlertDestructiveBg;
            alertDestructiveStyle.normal.background = CreateSolidTexture(destructiveBorder);
            alertDestructiveStyle.normal.textColor = destructiveColor;
            alertDestructiveStyle.hover.background = CreateSolidTexture(destructiveBorder);
            alertDestructiveStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            alertDestructiveStyle.padding = new RectOffset(horizontalPadding, horizontalPadding, verticalPadding, verticalPadding);

            alertTitleStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                alertTitleStyle.font = customFont;
            alertTitleStyle.fontSize = Mathf.RoundToInt(scaledFontSize + 2);
            alertTitleStyle.fontStyle = FontStyle.Normal;
            alertTitleStyle.normal.textColor = defaultFg;
            alertTitleStyle.margin = new RectOffset(0, 0, 0, Mathf.RoundToInt(4 * guiHelper.uiScale));

            alertDescriptionStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                alertDescriptionStyle.font = customFont;
            alertDescriptionStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            alertDescriptionStyle.normal.textColor = Color.Lerp(theme.TextColor, theme.PrimaryColor, 0.3f);
            alertDescriptionStyle.wordWrap = true;
        }

        /// <summary>
        /// Sets up the avatar styles.
        /// </summary>
        private void SetupAvatarStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int defaultAvatarSize = Mathf.RoundToInt(40 * guiHelper.uiScale);

            avatarStyle = new GUIStyle(GUI.skin.box);
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
                    return Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                case AvatarSize.Large:
                    return Mathf.RoundToInt((guiHelper.fontSize + 4) * guiHelper.uiScale);
                default:
                    return Mathf.RoundToInt(guiHelper.fontSize * guiHelper.uiScale);
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

            return new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
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

            skeletonStyle = new GUIStyle(GUI.skin.box);
            skeletonStyle.normal.background = CreateSolidTexture(theme.SkeletonBg);
            skeletonStyle.border = new RectOffset(4, 4, 4, 4);
            skeletonStyle.padding = new RectOffset(0, 0, 0, 0);

            skeletonRoundedStyle = new GUIStyle(skeletonStyle);
            skeletonRoundedStyle.border = new RectOffset(8, 8, 8, 8);

            skeletonCircularStyle = new GUIStyle(skeletonStyle);
            skeletonCircularStyle.border = new RectOffset(50, 50, 50, 50);
        }

        /// <summary>
        /// Sets up the table styles.
        /// </summary>
        private void SetupTableStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            tableStyle = new GUIStyle(GUI.skin.box);
            tableStyle.normal.background = CreateSolidTexture(theme.TableBg);
            tableStyle.border = new RectOffset(1, 1, 1, 1);
            tableStyle.padding = new RectOffset(0, 0, 0, 0);

            tableHeaderStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                tableHeaderStyle.font = customFont;
            tableHeaderStyle.normal.background = CreateSolidTexture(theme.TableHeaderBg);
            tableHeaderStyle.normal.textColor = theme.TextColor;
            tableHeaderStyle.padding = new RectOffset(8, 8, 8, 8);
            tableHeaderStyle.fontStyle = FontStyle.Bold;

            tableCellStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                tableCellStyle.font = customFont;
            tableCellStyle.normal.background = CreateSolidTexture(theme.TableCellBg);
            tableCellStyle.normal.textColor = theme.TextColor;
            tableCellStyle.padding = new RectOffset(8, 8, 8, 8);

            tableStripedStyle = new GUIStyle(tableCellStyle);
            if (customFont != null)
                tableStripedStyle.font = customFont;
            tableStripedStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.TableCellBg, Color.black, 0.1f));

            tableBorderedStyle = new GUIStyle(tableCellStyle);
            if (customFont != null)
                tableBorderedStyle.font = customFont;
            tableBorderedStyle.border = new RectOffset(1, 1, 1, 1);
            tableBorderedStyle.normal.background = CreateSolidTexture(theme.TableCellBg);

            tableHoverStyle = new GUIStyle(tableCellStyle);
            if (customFont != null)
                tableHoverStyle.font = customFont;
            tableHoverStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.TableCellBg, Color.white, 0.1f));
        }

        private void SetupCalendarStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            calendarStyle = new GUIStyle(GUI.skin.box);
            calendarStyle.normal.background = CreateSolidTexture(theme.CardBg);
            calendarStyle.border = new RectOffset(4, 4, 4, 4);
            calendarStyle.padding = new RectOffset(10, 10, 10, 10);

            calendarHeaderStyle = new GUIStyle();
            calendarHeaderStyle.padding = new RectOffset(0, 0, 5, 5);

            calendarTitleStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                calendarTitleStyle.font = customFont;
            calendarTitleStyle.fontSize = guiHelper.fontSize + 2;
            calendarTitleStyle.fontStyle = FontStyle.Bold;
            calendarTitleStyle.normal.textColor = theme.TextColor;
            calendarTitleStyle.alignment = TextAnchor.MiddleCenter;

            calendarWeekdayStyle = new GUIStyle(GUI.skin.label);
            if (customFont != null)
                calendarWeekdayStyle.font = customFont;
            calendarWeekdayStyle.fontSize = guiHelper.fontSize - 1;
            calendarWeekdayStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.3f);
            calendarWeekdayStyle.alignment = TextAnchor.MiddleCenter;

            calendarDayStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                calendarDayStyle.font = customFont;
            calendarDayStyle.fontSize = guiHelper.fontSize;
            calendarDayStyle.normal.textColor = theme.TextColor;
            calendarDayStyle.normal.background = CreateSolidTexture(theme.CardBg);
            calendarDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.white, 0.1f));
            calendarDayStyle.active.background = CreateSolidTexture(Color.Lerp(theme.CardBg, Color.black, 0.1f));

            calendarDaySelectedStyle = new GUIStyle(calendarDayStyle);
            if (customFont != null)
                calendarDaySelectedStyle.font = customFont;
            calendarDaySelectedStyle.normal.background = CreateSolidTexture(theme.AccentColor);
            calendarDaySelectedStyle.normal.textColor = Color.white;
            calendarDaySelectedStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.white, 0.1f));
            calendarDaySelectedStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));

            calendarDayOutsideMonthStyle = new GUIStyle(calendarDayStyle);
            if (customFont != null)
                calendarDayOutsideMonthStyle.font = customFont;
            calendarDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.TextColor, Color.black, 0.5f);

            calendarDayTodayStyle = new GUIStyle(calendarDayStyle);
            if (customFont != null)
                calendarDayTodayStyle.font = customFont;
            calendarDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.CardBg, theme.AccentColor);
        }

        private void SetupDropdownMenuStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            dropdownMenuContentStyle = new GUIStyle(GUI.skin.box);
            dropdownMenuContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
            dropdownMenuContentStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            dropdownMenuContentStyle.padding = new RectOffset(5, 5, 5, 5);

            dropdownMenuItemStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                dropdownMenuItemStyle.font = customFont;
            dropdownMenuItemStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            dropdownMenuItemStyle.fontStyle = FontStyle.Normal;
            dropdownMenuItemStyle.alignment = TextAnchor.MiddleLeft;
            dropdownMenuItemStyle.normal.background = transparentTexture;
            dropdownMenuItemStyle.normal.textColor = theme.TextColor;
            dropdownMenuItemStyle.hover.background = CreateSolidTexture(theme.AccentColor);
            dropdownMenuItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));
            dropdownMenuItemStyle.padding = new RectOffset(10, 10, 5, 5);

            dropdownMenuSeparatorStyle = new GUIStyle();
            dropdownMenuSeparatorStyle.normal.background = CreateSolidTexture(theme.SeparatorColor);
            dropdownMenuSeparatorStyle.fixedHeight = 1;
            dropdownMenuSeparatorStyle.margin = new RectOffset(5, 5, 5, 5);
        }

        private void SetupPopoverStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            popoverContentStyle = new GUIStyle(GUI.skin.box);
            popoverContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
            popoverContentStyle.border = new RectOffset(4, 4, 4, 4);
            popoverContentStyle.padding = new RectOffset(10, 10, 10, 10);
        }

        private void SetupScrollAreaStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            scrollAreaStyle = new GUIStyle();
            scrollAreaStyle.normal.background = CreateSolidTexture(theme.BackgroundColor);

            scrollAreaThumbStyle = new GUIStyle();
            scrollAreaThumbStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.white, 0.2f));
            scrollAreaThumbStyle.border = new RectOffset(4, 4, 4, 4);

            scrollAreaTrackStyle = new GUIStyle();
            scrollAreaTrackStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.BackgroundColor, Color.black, 0.1f));
            scrollAreaTrackStyle.border = new RectOffset(4, 4, 4, 4);
        }

        private void SetupSelectStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            selectTriggerStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                selectTriggerStyle.font = customFont;
            selectTriggerStyle.normal.background = CreateSolidTexture(theme.InputBg);
            selectTriggerStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            selectTriggerStyle.padding = new RectOffset(10, 10, 5, 5);
            selectTriggerStyle.alignment = TextAnchor.MiddleLeft;
            selectTriggerStyle.normal.textColor = theme.TextColor;
            selectTriggerStyle.fontSize = Mathf.RoundToInt(scaledFontSize);

            selectContentStyle = new GUIStyle(GUI.skin.box);
            selectContentStyle.normal.background = CreateSolidTexture(theme.CardBg);
            selectContentStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            selectContentStyle.padding = new RectOffset(5, 5, 5, 5);

            selectItemStyle = new GUIStyle(GUI.skin.button);
            if (customFont != null)
                selectItemStyle.font = customFont;
            selectItemStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            selectItemStyle.fontStyle = FontStyle.Normal;
            selectItemStyle.alignment = TextAnchor.MiddleLeft;
            selectItemStyle.normal.background = transparentTexture;
            selectItemStyle.normal.textColor = theme.TextColor;
            selectItemStyle.hover.background = CreateSolidTexture(theme.AccentColor);
            selectItemStyle.active.background = CreateSolidTexture(Color.Lerp(theme.AccentColor, Color.black, 0.1f));
            selectItemStyle.padding = new RectOffset(10, 10, 5, 5);
        }
        #endregion

        #region Style Getters
        /// <summary>
        /// Gets the button style for the given variant and size.
        /// </summary>
        public GUIStyle GetButtonStyle(ButtonVariant variant, ButtonSize size)
        {
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            switch (size)
            {
                case ButtonSize.Small:
                    sizedStyle.fontSize = Mathf.RoundToInt((scaledFontSize - 2) * guiHelper.uiScale);
                    sizedStyle.padding = GetScaledPadding(12, 4);
                    sizedStyle.fixedHeight = Mathf.RoundToInt(32 * guiHelper.uiScale);
                    sizedStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                    break;
                case ButtonSize.Large:
                    sizedStyle.fontSize = Mathf.RoundToInt(scaledFontSize * guiHelper.uiScale);
                    sizedStyle.padding = GetScaledPadding(32, 10);
                    sizedStyle.fixedHeight = Mathf.RoundToInt(40 * guiHelper.uiScale);
                    sizedStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                    break;
                case ButtonSize.Icon:
                    sizedStyle.fontSize = Mathf.RoundToInt(scaledFontSize * guiHelper.uiScale);
                    sizedStyle.padding = GetScaledPadding(0, 0);
                    sizedStyle.fixedWidth = Mathf.RoundToInt(36 * guiHelper.uiScale);
                    sizedStyle.fixedHeight = Mathf.RoundToInt(36 * guiHelper.uiScale);
                    sizedStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                    break;
                default:

                    break;
            }

            return sizedStyle;
        }

        /// <summary>
        /// Gets the toggle style for the given variant and size.
        /// </summary>
        public GUIStyle GetToggleStyle(ToggleVariant variant, ToggleSize size)
        {
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);
            switch (size)
            {
                case ToggleSize.Small:
                    sizedStyle.fontSize = guiHelper.fontSize - 2;
                    sizedStyle.padding = new RectOffset(6, 6, 4, 4);
                    break;
                case ToggleSize.Large:
                    sizedStyle.fontSize = guiHelper.fontSize + 2;
                    sizedStyle.padding = new RectOffset(10, 10, 8, 8);
                    break;
            }

            return sizedStyle;
        }

        /// <summary>
        /// Gets the input style for the given variant, focus, and disabled state.
        /// </summary>
        public GUIStyle GetInputStyle(InputVariant variant, bool focused = false, bool disabled = false)
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            if (disabled)
                return inputDisabledStyle ?? GUI.skin.textField;
            if (focused)
                return inputFocusedStyle ?? GUI.skin.textField;

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

            return style;
        }

        /// <summary>
        /// Gets the label style for the given variant.
        /// </summary>
        public GUIStyle GetLabelStyle(LabelVariant variant)
        {
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
        public GUIStyle GetTextAreaStyle(TextAreaVariant variant = TextAreaVariant.Default, bool focused = false)
        {
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

            return focused ? (textAreaFocusedStyle ?? baseStyle) : baseStyle;
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
        public GUIStyle GetCheckboxStyle(CheckboxVariant variant, CheckboxSize size)
        {
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);

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

            return sizedStyle;
        }

        /// <summary>
        /// Gets the switch style for the given variant and size.
        /// </summary>
        public GUIStyle GetSwitchStyle(SwitchVariant variant, SwitchSize size)
        {
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);

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

            return sizedStyle;
        }

        /// <summary>
        /// Gets the badge style for the given variant and size.
        /// </summary>
        public GUIStyle GetBadgeStyle(BadgeVariant variant, BadgeSize size)
        {
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);

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

            return sizedStyle;
        }

        /// <summary>
        /// Gets the alert style for the given variant and type.
        /// </summary>
        public GUIStyle GetAlertStyle(AlertVariant variant, AlertType type)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle baseStyle = variant == AlertVariant.Destructive ? alertDestructiveStyle : alertDefaultStyle;
            return baseStyle ?? GUI.skin.box;
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
        public GUIStyle GetAvatarStyle(AvatarSize size, AvatarShape shape)
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle = avatarStyle;
            GUIStyle sizedStyle = new GUIStyle(baseStyle);

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
                    sizedStyle.border = new RectOffset(50, 50, 50, 50);
                    break;
                case AvatarShape.Rounded:
                    sizedStyle.border = new RectOffset(8, 8, 8, 8);
                    break;
                case AvatarShape.Square:
                    sizedStyle.border = new RectOffset(0, 0, 0, 0);
                    break;
            }

            return sizedStyle;
        }

        /// <summary>
        /// Gets the avatar fallback style for the given size and shape.
        /// </summary>
        public GUIStyle GetAvatarFallbackStyle(AvatarSize size, AvatarShape shape)
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            GUIStyle baseStyle = avatarStyle;

            GUIStyle sizedStyle = new GUIStyle(baseStyle);

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
            sizedStyle.fontSize = GetAvatarFontSize(size);

            switch (shape)
            {
                case AvatarShape.Circle:
                    sizedStyle.border = new RectOffset(50, 50, 50, 50);
                    break;
                case AvatarShape.Rounded:
                    sizedStyle.border = new RectOffset(8, 8, 8, 8);
                    break;
                case AvatarShape.Square:
                    sizedStyle.border = new RectOffset(0, 0, 0, 0);
                    break;
            }

            return sizedStyle;
        }

        /// <summary>
        /// Gets the skeleton style for the given variant and size.
        /// </summary>
        public GUIStyle GetSkeletonStyle(SkeletonVariant variant, SkeletonSize size)
        {
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);

            switch (size)
            {
                case SkeletonSize.Small:
                    sizedStyle.border = new RectOffset(2, 2, 2, 2);
                    break;
                case SkeletonSize.Large:

                    break;
                default:

                    break;
            }

            return sizedStyle;
        }

        /// <summary>
        /// Gets the table style for the given variant and size.
        /// </summary>
        public GUIStyle GetTableStyle(TableVariant variant, TableSize size)
        {
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

            return baseStyle ?? GUI.skin.box;
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
        public GUIStyle GetCalendarStyle(CalendarVariant variant, CalendarSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new GUIStyle(calendarStyle);

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

            return sizedStyle;
        }

        /// <summary>
        /// Gets the dropdown menu style for the given variant and size.
        /// </summary>
        public GUIStyle GetDropdownMenuStyle(DropdownMenuVariant variant, DropdownMenuSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            return dropdownMenuContentStyle;
        }

        /// <summary>
        /// Gets the popover style for the given variant and size.
        /// </summary>
        public GUIStyle GetPopoverStyle(PopoverVariant variant, PopoverSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new GUIStyle(popoverContentStyle);

            switch (size)
            {
                case PopoverSize.Small:
                    sizedStyle.padding = new RectOffset(5, 5, 5, 5);
                    break;
                case PopoverSize.Large:
                    sizedStyle.padding = new RectOffset(15, 15, 15, 15);
                    break;
                default:

                    break;
            }

            return sizedStyle;
        }

        /// <summary>
        /// Gets the scroll area style for the given variant and size.
        /// </summary>
        public GUIStyle GetScrollAreaStyle(ScrollAreaVariant variant, ScrollAreaSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new GUIStyle(scrollAreaStyle);

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

            return sizedStyle;
        }

        /// <summary>
        /// Gets the select style for the given variant and size.
        /// </summary>
        public GUIStyle GetSelectStyle(SelectVariant variant, SelectSize size)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            GUIStyle sizedStyle = new GUIStyle(selectContentStyle);

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
        }
        #endregion
    }
}
