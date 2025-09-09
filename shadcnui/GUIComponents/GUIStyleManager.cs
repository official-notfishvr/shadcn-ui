using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

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
        Link
    }

    /// <summary>
    /// Sizes for the Button component.
    /// </summary>
    public enum ButtonSize
    {
        Default,
        Small,
        Large,
        Icon
    }

    /// <summary>
    /// Variants for the Toggle component.
    /// </summary>
    public enum ToggleVariant
    {
        Default,
        Outline
    }

    /// <summary>
    /// Sizes for the Toggle component.
    /// </summary>
    public enum ToggleSize
    {
        Default,
        Small,
        Large
    }

    /// <summary>
    /// Variants for the Input component.
    /// </summary>
    public enum InputVariant
    {
        Default,
        Outline,
        Ghost
    }

    /// <summary>
    /// Variants for the Label component.
    /// </summary>
    public enum LabelVariant
    {
        Default,
        Secondary,
        Muted,
        Destructive
    }

    /// <summary>
    /// Orientations for the Separator component.
    /// </summary>
    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// Variants for the TextArea component.
    /// </summary>
    public enum TextAreaVariant
    {
        Default,
        Outline,
        Ghost
    }

    /// <summary>
    /// Variants for the Progress component.
    /// </summary>
    public enum ProgressVariant
    {
        Default
    }

    /// <summary>
    /// Variants for the Tabs component.
    /// </summary>
    public enum TabsVariant
    {
        Default
    }

    /// <summary>
    /// Variants for the Checkbox component.
    /// </summary>
    public enum CheckboxVariant
    {
        Default,
        Outline,
        Ghost
    }

    /// <summary>
    /// Sizes for the Checkbox component.
    /// </summary>
    public enum CheckboxSize
    {
        Default,
        Small,
        Large
    }

    /// <summary>
    /// Variants for the Switch component.
    /// </summary>
    public enum SwitchVariant
    {
        Default,
        Outline,
        Ghost
    }

    /// <summary>
    /// Sizes for the Switch component.
    /// </summary>
    public enum SwitchSize
    {
        Default,
        Small,
        Large
    }

    /// <summary>
    /// Variants for the Badge component.
    /// </summary>
    public enum BadgeVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline
    }

    /// <summary>
    /// Sizes for the Badge component.
    /// </summary>
    public enum BadgeSize
    {
        Default,
        Small,
        Large
    }

    /// <summary>
    /// Variants for the Alert component.
    /// </summary>
    public enum AlertVariant
    {
        Default,
        Destructive
    }

    /// <summary>
    /// Types for the Alert component.
    /// </summary>
    public enum AlertType
    {
        Info,
        Warning,
        Error,
        Success
    }

    /// <summary>
    /// Sizes for the Avatar component.
    /// </summary>
    public enum AvatarSize
    {
        Small,
        Default,
        Large
    }

    /// <summary>
    /// Shapes for the Avatar component.
    /// </summary>
    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded
    }

    /// <summary>
    /// Variants for the Skeleton component.
    /// </summary>
    public enum SkeletonVariant
    {
        Default,
        Rounded,
        Circular
    }

    /// <summary>
    /// Sizes for the Skeleton component.
    /// </summary>
    public enum SkeletonSize
    {
        Small,
        Default,
        Large
    }

    /// <summary>
    /// Variants for the Table component.
    /// </summary>
    public enum TableVariant
    {
        Default,
        Striped,
        Bordered,
        Hover
    }

    /// <summary>
    /// Sizes for the Table component.
    /// </summary>
    public enum TableSize
    {
        Small,
        Default,
        Large
    }

    /// <summary>
    /// Variants for the Slider component.
    /// </summary>
    public enum SliderVariant
    {
        Default,
        Range,
        Vertical,
        Disabled
    }

    /// <summary>
    /// Sizes for the Slider component.
    /// </summary>
    public enum SliderSize
    {
        Small,
        Default,
        Large
    }
    #endregion

    /// <summary>
    /// Manages the GUI styles for the shadcnui components.
    /// </summary>
    public class GUIStyleManager
    {
        private GUIHelper guiHelper;

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
        public GUIStyle buttonSmallStyle;
        public GUIStyle buttonLargeStyle;
        public GUIStyle buttonIconStyle;

        // Toggle Styles
        public GUIStyle toggleDefaultStyle;
        public GUIStyle toggleOutlineStyle;
        public GUIStyle toggleSmallStyle;
        public GUIStyle toggleLargeStyle;

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
        public GUIStyle labelSmallStyle;
        public GUIStyle labelLargeStyle;

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
        public GUIStyle checkboxSmallStyle;
        public GUIStyle checkboxLargeStyle;

        // Switch Styles
        public GUIStyle switchDefaultStyle;
        public GUIStyle switchOutlineStyle;
        public GUIStyle switchGhostStyle;
        public GUIStyle switchSmallStyle;
        public GUIStyle switchLargeStyle;

        // Badge Styles
        public GUIStyle badgeDefaultStyle;
        public GUIStyle badgeSecondaryStyle;
        public GUIStyle badgeDestructiveStyle;
        public GUIStyle badgeOutlineStyle;
        public GUIStyle badgeSmallStyle;
        public GUIStyle badgeLargeStyle;

        // Alert Styles
        public GUIStyle alertDefaultStyle;
        public GUIStyle alertDestructiveStyle;
        public GUIStyle alertTitleStyle;
        public GUIStyle alertDescriptionStyle;

        // Avatar Styles
        public GUIStyle avatarStyle;
        public GUIStyle avatarFallbackStyle;
        public GUIStyle avatarSmallStyle;
        public GUIStyle avatarLargeStyle;

        // Skeleton Styles
        public GUIStyle skeletonStyle;
        public GUIStyle skeletonRoundedStyle;
        public GUIStyle skeletonCircularStyle;
        public GUIStyle skeletonSmallStyle;
        public GUIStyle skeletonLargeStyle;

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
        public GUIStyle sliderSmallStyle;
        public GUIStyle sliderLargeStyle;
        public GUIStyle sliderLabelStyle;
        public GUIStyle sliderValueStyle;

        // Scaled Label Styles
        public GUIStyle labelSmallScaledStyle;
        public GUIStyle labelMediumScaledStyle;
        public GUIStyle labelLargeScaledStyle;
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
        #endregion

        public GUIStyleManager(GUIHelper helper)
        {
            guiHelper = helper;
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
        }
        #endregion

        #region Texture Creation
        /// <summary>
        /// Creates the custom textures used by the GUI styles.
        /// </summary>
        private void CreateCustomTextures()
        {

            gradientTexture = new Texture2D(1, 100);
            for (int i = 0; i < 100; i++)
            {
                float t = i / 99f;
                Color startColor = guiHelper.customColorsEnabled ?
                    Color.Lerp(guiHelper.primaryColor, Color.black, 0.8f) :
                    new Color(0.1f, 0.1f, 0.2f, 0.9f);
                Color endColor = guiHelper.customColorsEnabled ?
                    Color.Lerp(guiHelper.secondaryColor, Color.black, 0.6f) :
                    new Color(0.3f, 0.2f, 0.4f, 0.9f);
                Color gradColor = Color.Lerp(startColor, endColor, t);
                gradientTexture.SetPixel(0, i, gradColor);
            }
            gradientTexture.Apply();


            cardBackgroundTexture = new Texture2D(1, 1);
            Color cardBgColor = guiHelper.customColorsEnabled ?
                new Color(guiHelper.primaryColor.r, guiHelper.primaryColor.g, guiHelper.primaryColor.b, guiHelper.backgroundAlpha) :
                new Color(0.15f, 0.15f, 0.25f, guiHelper.backgroundAlpha);
            cardBackgroundTexture.SetPixel(0, 0, cardBgColor);
            cardBackgroundTexture.Apply();


            inputBackgroundTexture = new Texture2D(1, 1);
            Color inputBgColor = guiHelper.customColorsEnabled ?
                new Color(guiHelper.primaryColor.r * 0.3f, guiHelper.primaryColor.g * 0.3f, guiHelper.primaryColor.b * 0.3f, 0.8f) :
                new Color(0.1f, 0.1f, 0.15f, 0.8f);
            inputBackgroundTexture.SetPixel(0, 0, inputBgColor);
            inputBackgroundTexture.Apply();


            inputFocusedTexture = new Texture2D(1, 1);
            Color focusColor = guiHelper.customColorsEnabled ?
                new Color(guiHelper.accentColor.r * 0.3f, guiHelper.accentColor.g * 0.3f, guiHelper.accentColor.b * 0.3f, 0.9f) :
                new Color(0.2f, 0.3f, 0.5f, 0.9f);
            inputFocusedTexture.SetPixel(0, 0, focusColor);
            inputFocusedTexture.Apply();


            outlineTexture = new Texture2D(4, 4);
            Color borderColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.5f, 0.5f, 0.7f);
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
                    Color glowColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.5f, 0.8f, 1f);
                    glowTexture.SetPixel(x, y, new Color(glowColor.r, glowColor.g, glowColor.b, alpha * guiHelper.glowIntensity));
                }
            }
            glowTexture.Apply();


            particleTexture = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Color particleColor = guiHelper.customColorsEnabled ?
                        Color.Lerp(guiHelper.accentColor, Color.white, 0.5f) :
                        new Color(1f, 1f, 1f, 0.8f);
                    particleTexture.SetPixel(x, y, particleColor);
                }
            }
            particleTexture.Apply();


            progressBarBackgroundTexture = new Texture2D(1, 1);
            Color progressBgColor = guiHelper.customColorsEnabled ?
                new Color(guiHelper.primaryColor.r * 0.2f, guiHelper.primaryColor.g * 0.2f, guiHelper.primaryColor.b * 0.2f, 0.2f) :
                new Color(0.09f, 0.09f, 0.11f, 0.2f);
            progressBarBackgroundTexture.SetPixel(0, 0, progressBgColor);
            progressBarBackgroundTexture.Apply();


            progressBarFillTexture = new Texture2D(1, 1);
            Color progressFillColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            progressBarFillTexture.SetPixel(0, 0, progressFillColor);
            progressBarFillTexture.Apply();


            separatorTexture = new Texture2D(1, 1);
            Color separatorColor = new Color(0.23f, 0.23f, 0.27f);
            separatorTexture.SetPixel(0, 0, separatorColor);
            separatorTexture.Apply();


            tabsBackgroundTexture = new Texture2D(1, 1);
            Color tabsBgColor = new Color(0.16f, 0.16f, 0.18f);
            tabsBackgroundTexture.SetPixel(0, 0, tabsBgColor);
            tabsBackgroundTexture.Apply();


            tabsActiveTexture = new Texture2D(1, 1);
            Color tabsActiveColor = new Color(0.02f, 0.02f, 0.04f);
            tabsActiveTexture.SetPixel(0, 0, tabsActiveColor);
            tabsActiveTexture.Apply();


            checkboxTexture = new Texture2D(1, 1);
            Color checkboxColor = new Color(0.1f, 0.1f, 0.15f);
            checkboxTexture.SetPixel(0, 0, checkboxColor);
            checkboxTexture.Apply();

            checkboxCheckedTexture = new Texture2D(1, 1);
            Color checkboxCheckedColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            checkboxCheckedTexture.SetPixel(0, 0, checkboxCheckedColor);
            checkboxCheckedTexture.Apply();

            switchTexture = new Texture2D(1, 1);
            Color switchColor = new Color(0.16f, 0.16f, 0.18f);
            switchTexture.SetPixel(0, 0, switchColor);
            switchTexture.Apply();

            switchOnTexture = new Texture2D(1, 1);
            Color switchOnColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            switchOnTexture.SetPixel(0, 0, switchOnColor);
            switchOnTexture.Apply();

            switchOffTexture = new Texture2D(1, 1);
            Color switchOffColor = new Color(0.16f, 0.16f, 0.18f);
            switchOffTexture.SetPixel(0, 0, switchOffColor);
            switchOffTexture.Apply();

            badgeTexture = new Texture2D(1, 1);
            Color badgeColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            badgeTexture.SetPixel(0, 0, badgeColor);
            badgeTexture.Apply();

            alertTexture = new Texture2D(1, 1);
            Color alertColor = new Color(0.15f, 0.15f, 0.25f);
            alertTexture.SetPixel(0, 0, alertColor);
            alertTexture.Apply();

            avatarTexture = new Texture2D(1, 1);
            Color avatarColor = new Color(0.16f, 0.16f, 0.18f);
            avatarTexture.SetPixel(0, 0, avatarColor);
            avatarTexture.Apply();

            skeletonTexture = new Texture2D(1, 1);
            Color skeletonColor = new Color(0.09f, 0.09f, 0.11f, 0.1f);
            skeletonTexture.SetPixel(0, 0, skeletonColor);
            skeletonTexture.Apply();

            tableTexture = new Texture2D(1, 1);
            Color tableColor = new Color(0.02f, 0.02f, 0.04f);
            tableTexture.SetPixel(0, 0, tableColor);
            tableTexture.Apply();

            tableHeaderTexture = new Texture2D(1, 1);
            Color tableHeaderColor = new Color(0.16f, 0.16f, 0.18f);
            tableHeaderTexture.SetPixel(0, 0, tableHeaderColor);
            tableHeaderTexture.Apply();

            tableCellTexture = new Texture2D(1, 1);
            Color tableCellColor = new Color(0.02f, 0.02f, 0.04f);
            tableCellTexture.SetPixel(0, 0, tableCellColor);
            tableCellTexture.Apply();
        }
        #endregion

        #region Style Setup Methods
        /// <summary>
        /// Sets up the animated styles.
        /// </summary>
        private void SetupAnimatedStyles()
        {

            animatedBoxStyle = new GUIStyle(GUI.skin.box);
            animatedBoxStyle.normal.background = gradientTexture;
            animatedBoxStyle.border = new RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius,
                guiHelper.cornerRadius, guiHelper.cornerRadius);
            animatedBoxStyle.padding = new RectOffset(15, 15, 15, 15);


            animatedButtonStyle = new GUIStyle(GUI.skin.button);
            animatedButtonStyle.fontSize = guiHelper.fontSize;
            animatedButtonStyle.fontStyle = FontStyle.Bold;
            animatedButtonStyle.alignment = TextAnchor.MiddleCenter;
            if (guiHelper.customColorsEnabled)
            {
                animatedButtonStyle.normal.textColor = Color.Lerp(Color.white, guiHelper.accentColor, 0.3f);
                animatedButtonStyle.hover.textColor = guiHelper.accentColor;
            }


            colorPresetStyle = new GUIStyle(GUI.skin.button);
            colorPresetStyle.fontSize = Mathf.RoundToInt(guiHelper.fontSize * 0.9f);
            colorPresetStyle.fontStyle = FontStyle.Bold;
            colorPresetStyle.alignment = TextAnchor.MiddleCenter;


            animatedInputStyle = new GUIStyle(GUI.skin.textField);
            animatedInputStyle.fontSize = guiHelper.fontSize + 1;
            animatedInputStyle.padding = new RectOffset(8, 8, 4, 4);
            if (guiHelper.customColorsEnabled)
            {
                animatedInputStyle.normal.textColor = Color.white;
                animatedInputStyle.focused.textColor = guiHelper.accentColor;
            }


            glowLabelStyle = new GUIStyle(GUI.skin.label);
            glowLabelStyle.fontSize = guiHelper.fontSize;
            Color labelColor = guiHelper.customColorsEnabled ?
                Color.Lerp(Color.white, guiHelper.accentColor, 0.2f) : Color.white;
            glowLabelStyle.normal.textColor = labelColor;


            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = guiHelper.fontSize + 4;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            Color titleColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : Color.white;
            titleStyle.normal.textColor = titleColor;


            sectionHeaderStyle = new GUIStyle(GUI.skin.label);
            sectionHeaderStyle.fontSize = guiHelper.fontSize + 2;
            sectionHeaderStyle.fontStyle = FontStyle.Bold;
            Color headerColor = guiHelper.customColorsEnabled ?
                Color.Lerp(guiHelper.accentColor, Color.white, 0.3f) : new Color(0.8f, 0.9f, 1f);
            sectionHeaderStyle.normal.textColor = headerColor;
        }

        /// <summary>
        /// Sets up the card styles.
        /// </summary>
        private void SetupCardStyles()
        {

            cardStyle = new GUIStyle(GUI.skin.box);
            cardStyle.normal.background = cardBackgroundTexture;
            cardStyle.border = new RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius);
            cardStyle.padding = new RectOffset(0, 0, 0, 0);
            cardStyle.margin = new RectOffset(0, 0, 5, 5);


            cardHeaderStyle = new GUIStyle();
            cardHeaderStyle.padding = new RectOffset(16, 16, 16, 8);
            cardHeaderStyle.margin = new RectOffset(0, 0, 0, 0);


            cardTitleStyle = new GUIStyle(GUI.skin.label);
            cardTitleStyle.fontSize = guiHelper.fontSize + 4;
            cardTitleStyle.fontStyle = FontStyle.Bold;
            cardTitleStyle.normal.textColor = guiHelper.customColorsEnabled ?
                Color.Lerp(guiHelper.accentColor, Color.white, 0.2f) : new Color(0.9f, 0.9f, 1f);
            cardTitleStyle.wordWrap = true;
            cardTitleStyle.margin = new RectOffset(0, 0, 0, 4);


            cardDescriptionStyle = new GUIStyle(GUI.skin.label);
            cardDescriptionStyle.fontSize = guiHelper.fontSize - 1;
            cardDescriptionStyle.normal.textColor = guiHelper.customColorsEnabled ?
                Color.Lerp(Color.white, guiHelper.primaryColor, 0.3f) : new Color(0.7f, 0.7f, 0.8f);
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
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);


            buttonDefaultStyle = CreateBaseButtonStyle();
            buttonDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonDefaultStyle.fontStyle = FontStyle.Normal;
            buttonDefaultStyle.alignment = TextAnchor.MiddleCenter;
            buttonDefaultStyle.padding = GetScaledPadding(16, 8);
            buttonDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            buttonDefaultStyle.fixedHeight = Mathf.RoundToInt(36 * guiHelper.uiScale);

            Color primaryBg = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            Color primaryFg = new Color(0.98f, 0.98f, 0.98f);
            buttonDefaultStyle.normal.background = CreateSolidTexture(primaryBg);
            buttonDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.black, 0.1f));
            buttonDefaultStyle.normal.textColor = primaryFg;
            buttonDefaultStyle.hover.textColor = primaryFg;
            buttonDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.black, 0.2f));
            buttonDefaultStyle.active.textColor = primaryFg;


            buttonDestructiveStyle = CreateBaseButtonStyle();
            buttonDestructiveStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonDestructiveStyle.fontStyle = FontStyle.Normal;
            buttonDestructiveStyle.alignment = TextAnchor.MiddleCenter;
            buttonDestructiveStyle.padding = GetScaledPadding(16, 8);
            buttonDestructiveStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color destructiveBg = new Color(0.86f, 0.24f, 0.24f);
            Color destructiveFg = new Color(0.98f, 0.98f, 0.98f);
            buttonDestructiveStyle.normal.background = CreateSolidTexture(destructiveBg);
            buttonDestructiveStyle.hover.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.1f));
            buttonDestructiveStyle.normal.textColor = destructiveFg;
            buttonDestructiveStyle.hover.textColor = destructiveFg;
            buttonDestructiveStyle.active.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.2f));
            buttonDestructiveStyle.active.textColor = destructiveFg;


            buttonOutlineStyle = CreateBaseButtonStyle();
            buttonOutlineStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonOutlineStyle.fontStyle = FontStyle.Normal;
            buttonOutlineStyle.alignment = TextAnchor.MiddleCenter;
            buttonOutlineStyle.padding = GetScaledPadding(16, 8);
            buttonOutlineStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color outlineBorder = new Color(0.23f, 0.23f, 0.27f);
            Color outlineBg = new Color(0.02f, 0.02f, 0.04f);
            Color outlineFg = new Color(0.98f, 0.98f, 0.98f);
            Color outlineHoverBg = new Color(0.16f, 0.16f, 0.18f);
            Color outlineHoverFg = new Color(0.98f, 0.98f, 0.98f);

            buttonOutlineStyle.normal.background = CreateOutlineButtonTexture(outlineBg, outlineBorder);
            buttonOutlineStyle.hover.background = CreateSolidTexture(outlineHoverBg);
            buttonOutlineStyle.normal.textColor = outlineFg;
            buttonOutlineStyle.hover.textColor = outlineHoverFg;
            buttonOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(outlineHoverBg, Color.black, 0.2f));
            buttonOutlineStyle.active.textColor = outlineHoverFg;


            buttonSecondaryStyle = CreateBaseButtonStyle();
            buttonSecondaryStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonSecondaryStyle.fontStyle = FontStyle.Normal;
            buttonSecondaryStyle.alignment = TextAnchor.MiddleCenter;
            buttonSecondaryStyle.padding = GetScaledPadding(16, 8);
            buttonSecondaryStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color secondaryBg = new Color(0.16f, 0.16f, 0.18f);
            Color secondaryFg = new Color(0.98f, 0.98f, 0.98f);
            buttonSecondaryStyle.normal.background = CreateSolidTexture(secondaryBg);
            buttonSecondaryStyle.hover.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.white, 0.2f));
            buttonSecondaryStyle.normal.textColor = secondaryFg;
            buttonSecondaryStyle.hover.textColor = secondaryFg;
            buttonSecondaryStyle.active.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.black, 0.2f));
            buttonSecondaryStyle.active.textColor = secondaryFg;


            buttonGhostStyle = CreateBaseButtonStyle();
            buttonGhostStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonGhostStyle.fontStyle = FontStyle.Normal;
            buttonGhostStyle.alignment = TextAnchor.MiddleCenter;
            buttonGhostStyle.padding = GetScaledPadding(16, 8);
            buttonGhostStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color ghostFg = new Color(0.98f, 0.98f, 0.98f);
            Color ghostHoverBg = new Color(0.16f, 0.16f, 0.18f);
            Color ghostHoverFg = new Color(0.98f, 0.98f, 0.98f);

            buttonGhostStyle.normal.background = transparentTexture;
            buttonGhostStyle.hover.background = CreateSolidTexture(ghostHoverBg);
            buttonGhostStyle.normal.textColor = ghostFg;
            buttonGhostStyle.hover.textColor = ghostHoverFg;
            buttonGhostStyle.active.background = CreateSolidTexture(Color.Lerp(ghostHoverBg, Color.black, 0.2f));
            buttonGhostStyle.active.textColor = ghostHoverFg;


            buttonLinkStyle = CreateBaseButtonStyle();
            buttonLinkStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonLinkStyle.fontStyle = FontStyle.Normal;
            buttonLinkStyle.alignment = TextAnchor.MiddleCenter;
            buttonLinkStyle.padding = GetScaledPadding(0, 0);
            buttonLinkStyle.border = new RectOffset(0, 0, 0, 0);

            Color linkColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            Color linkHoverColor = Color.Lerp(linkColor, Color.white, 0.2f);

            buttonLinkStyle.normal.background = transparentTexture;
            buttonLinkStyle.hover.background = transparentTexture;
            buttonLinkStyle.normal.textColor = linkColor;
            buttonLinkStyle.hover.textColor = linkHoverColor;
            buttonLinkStyle.active.background = transparentTexture;
            buttonLinkStyle.active.textColor = Color.Lerp(linkHoverColor, Color.black, 0.2f);

            SetupButtonSizeVariants(scaledFontSize, guiHelper.uiScale, borderRadius);
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
            return new RectOffset(
                Mathf.RoundToInt(horizontal * guiHelper.uiScale),
                Mathf.RoundToInt(horizontal * guiHelper.uiScale),
                Mathf.RoundToInt(vertical * guiHelper.uiScale),
                Mathf.RoundToInt(vertical * guiHelper.uiScale)
            );
        }

        private void SetupButtonSizeVariants(float baseFontSize, float scale, int borderRadius)
        {

            buttonDefaultStyle.fixedHeight = Mathf.RoundToInt(36 * scale);


            buttonSmallStyle = CreateBaseButtonStyle();
            buttonSmallStyle.fontSize = Mathf.RoundToInt((baseFontSize - 2) * scale);
            buttonSmallStyle.fontStyle = FontStyle.Normal;
            buttonSmallStyle.alignment = TextAnchor.MiddleCenter;
            buttonSmallStyle.padding = GetScaledPadding(12, 4);
            buttonSmallStyle.fixedHeight = Mathf.RoundToInt(32 * scale);
            buttonSmallStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);


            buttonLargeStyle = CreateBaseButtonStyle();
            buttonLargeStyle.fontSize = Mathf.RoundToInt(baseFontSize * scale);
            buttonLargeStyle.fontStyle = FontStyle.Normal;
            buttonLargeStyle.alignment = TextAnchor.MiddleCenter;
            buttonLargeStyle.padding = GetScaledPadding(32, 10);
            buttonLargeStyle.fixedHeight = Mathf.RoundToInt(40 * scale);
            buttonLargeStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);


            buttonIconStyle = CreateBaseButtonStyle();
            buttonIconStyle.fontSize = Mathf.RoundToInt(baseFontSize * scale);
            buttonIconStyle.fontStyle = FontStyle.Normal;
            buttonIconStyle.alignment = TextAnchor.MiddleCenter;
            buttonIconStyle.padding = GetScaledPadding(0, 0);
            buttonIconStyle.fixedWidth = Mathf.RoundToInt(36 * scale);
            buttonIconStyle.fixedHeight = Mathf.RoundToInt(36 * scale);
            buttonIconStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
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

            toggleDefaultStyle = new GUIStyle(GUI.skin.button);
            toggleDefaultStyle.fontSize = guiHelper.fontSize;
            toggleDefaultStyle.fontStyle = FontStyle.Normal;
            toggleDefaultStyle.alignment = TextAnchor.MiddleCenter;
            toggleDefaultStyle.normal.textColor = guiHelper.customColorsEnabled ?
                Color.Lerp(Color.white, guiHelper.accentColor, 0.3f) : new Color(0.8f, 0.8f, 0.9f);
            toggleDefaultStyle.normal.background = transparentTexture;


            toggleOutlineStyle = new GUIStyle(toggleDefaultStyle);
            toggleOutlineStyle.normal.background = CreateOutlineTexture();
            toggleOutlineStyle.border = new RectOffset(2, 2, 2, 2);


            toggleSmallStyle = new GUIStyle(toggleDefaultStyle);
            toggleSmallStyle.fontSize = guiHelper.fontSize - 2;
            toggleSmallStyle.padding = new RectOffset(6, 6, 4, 4);

            toggleLargeStyle = new GUIStyle(toggleDefaultStyle);
            toggleLargeStyle.fontSize = guiHelper.fontSize + 2;
            toggleLargeStyle.padding = new RectOffset(10, 10, 8, 8);
        }

        /// <summary>
        /// Sets up the input styles.
        /// </summary>
        private void SetupInputVariantStyles()
        {

            inputDefaultStyle = new GUIStyle(GUI.skin.textField);
            inputDefaultStyle.fontSize = guiHelper.fontSize;
            inputDefaultStyle.padding = new RectOffset(12, 12, 4, 4);
            inputDefaultStyle.margin = new RectOffset(0, 0, 4, 4);
            inputDefaultStyle.border = new RectOffset(1, 1, 1, 1);
            inputDefaultStyle.normal.background = inputBackgroundTexture;
            inputDefaultStyle.normal.textColor = Color.white;
            inputDefaultStyle.hover.background = inputBackgroundTexture;
            inputDefaultStyle.focused.background = inputFocusedTexture;
            inputDefaultStyle.focused.textColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.9f, 0.9f, 1f);


            inputOutlineStyle = new GUIStyle(inputDefaultStyle);
            inputOutlineStyle.normal.background = CreateOutlineTexture();
            inputOutlineStyle.focused.background = CreateSolidTexture(guiHelper.customColorsEnabled ?
                new Color(guiHelper.accentColor.r, guiHelper.accentColor.g, guiHelper.accentColor.b, 0.1f) :
                new Color(0.3f, 0.4f, 0.6f, 0.1f));


            inputGhostStyle = new GUIStyle(inputDefaultStyle);
            inputGhostStyle.normal.background = transparentTexture;
            inputGhostStyle.focused.background = CreateSolidTexture(new Color(0.1f, 0.1f, 0.2f, 0.3f));


            inputFocusedStyle = new GUIStyle(inputDefaultStyle);
            inputFocusedStyle.normal.background = inputFocusedTexture;
            inputFocusedStyle.border = new RectOffset(2, 2, 2, 2);


            inputDisabledStyle = new GUIStyle(inputDefaultStyle);
            inputDisabledStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
            inputDisabledStyle.normal.background = CreateSolidTexture(new Color(0.2f, 0.2f, 0.2f, 0.5f));


            passwordFieldStyle = new GUIStyle(inputDefaultStyle);
            passwordFieldStyle.fontSize = guiHelper.fontSize + 2;


            textAreaStyle = new GUIStyle(inputDefaultStyle);
            textAreaStyle.wordWrap = true;
            textAreaStyle.stretchHeight = true;
            textAreaStyle.padding = new RectOffset(12, 12, 8, 8);


            textAreaFocusedStyle = new GUIStyle(textAreaStyle);
            textAreaFocusedStyle.normal.background = inputFocusedTexture;
        }

        /// <summary>
        /// Sets up the label styles.
        /// </summary>
        private void SetupLabelVariantStyles()
        {

            labelDefaultStyle = new GUIStyle(GUI.skin.label);
            labelDefaultStyle.fontSize = guiHelper.fontSize;
            labelDefaultStyle.fontStyle = FontStyle.Normal;
            labelDefaultStyle.normal.textColor = Color.white;
            labelDefaultStyle.padding = new RectOffset(0, 0, 2, 2);


            labelSecondaryStyle = new GUIStyle(labelDefaultStyle);
            labelSecondaryStyle.normal.textColor = guiHelper.customColorsEnabled ?
                Color.Lerp(Color.white, guiHelper.secondaryColor, 0.4f) : new Color(0.8f, 0.8f, 0.9f);


            labelMutedStyle = new GUIStyle(labelDefaultStyle);
            labelMutedStyle.normal.textColor = new Color(0.6f, 0.6f, 0.7f);
            labelMutedStyle.fontSize = guiHelper.fontSize - 1;


            labelDestructiveStyle = new GUIStyle(labelDefaultStyle);
            labelDestructiveStyle.normal.textColor = new Color(0.9f, 0.3f, 0.3f);
            labelDestructiveStyle.fontStyle = FontStyle.Bold;


            labelSmallStyle = new GUIStyle(labelDefaultStyle);
            labelSmallStyle.fontSize = guiHelper.fontSize - 2;


            labelLargeStyle = new GUIStyle(labelDefaultStyle);
            labelLargeStyle.fontSize = guiHelper.fontSize + 2;
            labelLargeStyle.fontStyle = FontStyle.Bold;
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
            Texture2D texture = new Texture2D(4, 4);
            Color borderColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.5f, 0.5f, 0.7f);
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

            separatorHorizontalStyle = new GUIStyle();
            separatorHorizontalStyle.normal.background = separatorTexture;
            separatorHorizontalStyle.fixedHeight = Mathf.RoundToInt(1 * guiHelper.uiScale);
            separatorHorizontalStyle.stretchWidth = true;


            separatorVerticalStyle = new GUIStyle();
            separatorVerticalStyle.normal.background = separatorTexture;
            separatorVerticalStyle.fixedWidth = Mathf.RoundToInt(1 * guiHelper.uiScale);
            separatorVerticalStyle.stretchHeight = true;
        }

        /// <summary>
        /// Sets up the tabs styles.
        /// </summary>
        private void SetupTabsStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);
            int padding = Mathf.RoundToInt(4 * guiHelper.uiScale);


            tabsListStyle = new GUIStyle();
            tabsListStyle.normal.background = tabsBackgroundTexture;
            tabsListStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            tabsListStyle.padding = new RectOffset(padding, padding, padding, padding);
            tabsListStyle.margin = new RectOffset(0, 0, 2, 2);


            tabsTriggerStyle = new GUIStyle(GUI.skin.button);
            tabsTriggerStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            tabsTriggerStyle.fontStyle = FontStyle.Bold;
            tabsTriggerStyle.alignment = TextAnchor.MiddleCenter;
            tabsTriggerStyle.padding = new RectOffset(
                Mathf.RoundToInt(12 * guiHelper.uiScale),
                Mathf.RoundToInt(12 * guiHelper.uiScale),
                Mathf.RoundToInt(4 * guiHelper.uiScale),
                Mathf.RoundToInt(4 * guiHelper.uiScale)
            );
            tabsTriggerStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            tabsTriggerStyle.normal.background = transparentTexture;
            tabsTriggerStyle.normal.textColor = new Color(0.64f, 0.64f, 0.71f);
            tabsTriggerStyle.hover.background = transparentTexture;
            tabsTriggerStyle.active.background = transparentTexture;


            tabsTriggerActiveStyle = new GUIStyle(tabsTriggerStyle);
            tabsTriggerActiveStyle.normal.background = tabsActiveTexture;
            tabsTriggerActiveStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f);
            tabsTriggerActiveStyle.hover.background = tabsActiveTexture;
            tabsTriggerActiveStyle.active.background = tabsActiveTexture;


            tabsContentStyle = new GUIStyle();
            tabsContentStyle.padding = new RectOffset(0, 0, Mathf.RoundToInt(8 * guiHelper.uiScale), 0);
        }

        /// <summary>
        /// Sets up the text area styles.
        /// </summary>
        private void SetupTextAreaVariantStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int padding = Mathf.RoundToInt(12 * guiHelper.uiScale);
            int verticalPadding = Mathf.RoundToInt(8 * guiHelper.uiScale);
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);


            textAreaStyle = new GUIStyle(GUI.skin.textArea);
            textAreaStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            textAreaStyle.padding = new RectOffset(padding, padding, verticalPadding, verticalPadding);
            textAreaStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            textAreaStyle.normal.background = inputBackgroundTexture;
            textAreaStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f);
            textAreaStyle.focused.background = inputFocusedTexture;
            textAreaStyle.focused.textColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.9f, 0.9f, 1f);
            textAreaStyle.hover.background = inputBackgroundTexture;
            textAreaStyle.wordWrap = true;
            textAreaStyle.stretchHeight = true;


            textAreaFocusedStyle = new GUIStyle(textAreaStyle);
            textAreaFocusedStyle.normal.background = inputFocusedTexture;
            textAreaFocusedStyle.border = new RectOffset(0, 0, 0, 0);


            textAreaOutlineStyle = new GUIStyle(textAreaStyle);
            textAreaOutlineStyle.normal.background = CreateOutlineTexture();
            textAreaOutlineStyle.focused.background = CreateSolidTexture(guiHelper.customColorsEnabled ?
                new Color(guiHelper.accentColor.r, guiHelper.accentColor.g, guiHelper.accentColor.b, 0.1f) :
                new Color(0.3f, 0.4f, 0.6f, 0.1f));


            textAreaGhostStyle = new GUIStyle(textAreaStyle);
            textAreaGhostStyle.normal.background = transparentTexture;
            textAreaGhostStyle.focused.background = CreateSolidTexture(new Color(0.1f, 0.1f, 0.2f, 0.3f));
        }

        /// <summary>
        /// Sets up the checkbox styles.
        /// </summary>
        private void SetupCheckboxStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            checkboxDefaultStyle = new GUIStyle(GUI.skin.toggle);
            checkboxDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            checkboxDefaultStyle.normal.background = checkboxTexture;
            checkboxDefaultStyle.onNormal.background = checkboxCheckedTexture;
            checkboxDefaultStyle.active.background = checkboxTexture;
            checkboxDefaultStyle.hover.background = checkboxTexture;
            checkboxDefaultStyle.onHover.background = checkboxCheckedTexture;
            checkboxDefaultStyle.onActive.background = checkboxCheckedTexture;
            checkboxDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            checkboxOutlineStyle = new GUIStyle(checkboxDefaultStyle);
            checkboxOutlineStyle.normal.background = CreateOutlineTexture();
            checkboxOutlineStyle.active.background = CreateOutlineTexture();
            checkboxOutlineStyle.onActive.background = CreateOutlineTexture();

            checkboxGhostStyle = new GUIStyle(checkboxDefaultStyle);
            checkboxGhostStyle.normal.background = transparentTexture;
            checkboxGhostStyle.active.background = transparentTexture;

            checkboxSmallStyle = new GUIStyle(checkboxDefaultStyle);
            checkboxSmallStyle.fontSize = Mathf.RoundToInt((scaledFontSize - 2));

            checkboxLargeStyle = new GUIStyle(checkboxDefaultStyle);
            checkboxLargeStyle.fontSize = Mathf.RoundToInt((scaledFontSize + 2));
        }

        /// <summary>
        /// Sets up the switch styles.
        /// </summary>
        private void SetupSwitchStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            switchDefaultStyle = new GUIStyle(GUI.skin.toggle);
            switchDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            switchDefaultStyle.normal.background = switchOffTexture;
            switchDefaultStyle.onNormal.background = switchOnTexture;
            switchDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(switchOffTexture.GetPixel(0,0), Color.black, 0.1f));
            switchDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(switchOnTexture.GetPixel(0,0), Color.black, 0.1f));
            switchDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            switchOutlineStyle = new GUIStyle(switchDefaultStyle);
            switchOutlineStyle.normal.background = CreateOutlineTexture();
            switchOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0,0), Color.black, 0.1f));
            switchOutlineStyle.onNormal.background = CreateOutlineTexture();
            switchOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(CreateOutlineTexture().GetPixel(0,0), Color.black, 0.1f));

            switchGhostStyle = new GUIStyle(switchDefaultStyle);
            switchGhostStyle.normal.background = transparentTexture;
            switchGhostStyle.active.background = CreateSolidTexture(Color.Lerp(transparentTexture.GetPixel(0,0), Color.black, 0.1f));
            switchGhostStyle.onNormal.background = transparentTexture;
            switchGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(transparentTexture.GetPixel(0,0), Color.black, 0.1f));

            switchSmallStyle = new GUIStyle(switchDefaultStyle);
            switchSmallStyle.fontSize = Mathf.RoundToInt((scaledFontSize - 2));

            switchLargeStyle = new GUIStyle(switchDefaultStyle);
            switchLargeStyle.fontSize = Mathf.RoundToInt((scaledFontSize + 2));
        }

        /// <summary>
        /// Sets up the badge styles.
        /// </summary>
        private void SetupBadgeStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            badgeDefaultStyle = new GUIStyle(GUI.skin.box);
            badgeDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            badgeDefaultStyle.normal.background = badgeTexture;
            badgeDefaultStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f);
            badgeDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            badgeDefaultStyle.padding = new RectOffset(8, 8, 3, 3);
            badgeDefaultStyle.alignment = TextAnchor.MiddleCenter;

            badgeSecondaryStyle = new GUIStyle(badgeDefaultStyle);
            badgeSecondaryStyle.normal.background = CreateSolidTexture(new Color(0.16f, 0.16f, 0.18f));

            badgeDestructiveStyle = new GUIStyle(badgeDefaultStyle);
            badgeDestructiveStyle.normal.background = CreateSolidTexture(new Color(0.86f, 0.24f, 0.24f));

            badgeOutlineStyle = new GUIStyle(badgeDefaultStyle);
            badgeOutlineStyle.normal.background = CreateOutlineTexture();

            badgeSmallStyle = new GUIStyle(badgeDefaultStyle);
            badgeSmallStyle.fontSize = Mathf.RoundToInt((scaledFontSize - 2));

            badgeLargeStyle = new GUIStyle(badgeDefaultStyle);
            badgeLargeStyle.fontSize = Mathf.RoundToInt((scaledFontSize + 2));
        }

        /// <summary>
        /// Sets up the alert styles.
        /// </summary>
        private void SetupAlertStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);
            int horizontalPadding = Mathf.RoundToInt(16 * guiHelper.uiScale);
            int verticalPadding = Mathf.RoundToInt(12 * guiHelper.uiScale);


            alertDefaultStyle = new GUIStyle(GUI.skin.box);
            Color defaultBg = new Color(0.02f, 0.02f, 0.04f);
            Color defaultFg = new Color(0.98f, 0.98f, 0.98f);
            Color defaultBorder = new Color(0.23f, 0.23f, 0.27f);
            alertDefaultStyle.normal.background = CreateSolidTexture(defaultBg);
            alertDefaultStyle.normal.textColor = defaultFg;
            alertDefaultStyle.hover.background = CreateSolidTexture(defaultBg);
            alertDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            alertDefaultStyle.padding = new RectOffset(horizontalPadding, horizontalPadding, verticalPadding, verticalPadding);


            alertDestructiveStyle = new GUIStyle(GUI.skin.box);
            Color destructiveColor = new Color(0.86f, 0.24f, 0.24f);
            Color destructiveBorder = new Color(destructiveColor.r, destructiveColor.g, destructiveColor.b, 0.5f);
            alertDestructiveStyle.normal.background = CreateSolidTexture(destructiveBorder);
            alertDestructiveStyle.normal.textColor = destructiveColor;
            alertDestructiveStyle.hover.background = CreateSolidTexture(destructiveBorder);
            alertDestructiveStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            alertDestructiveStyle.padding = new RectOffset(horizontalPadding, horizontalPadding, verticalPadding, verticalPadding);


            alertTitleStyle = new GUIStyle(GUI.skin.label);
            alertTitleStyle.fontSize = Mathf.RoundToInt(scaledFontSize + 2);
            alertTitleStyle.fontStyle = FontStyle.Normal;
            alertTitleStyle.normal.textColor = defaultFg;
            alertTitleStyle.margin = new RectOffset(0, 0, 0, Mathf.RoundToInt(4 * guiHelper.uiScale));


            alertDescriptionStyle = new GUIStyle(GUI.skin.label);
            alertDescriptionStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            alertDescriptionStyle.normal.textColor = new Color(0.7f, 0.7f, 0.8f);
            alertDescriptionStyle.wordWrap = true;
        }

        

        /// <summary>
        /// Sets up the avatar styles.
        /// </summary>
        private void SetupAvatarStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int defaultAvatarSize = Mathf.RoundToInt(40 * guiHelper.uiScale);


            avatarStyle = new GUIStyle(GUI.skin.box);
            avatarStyle.normal.background = avatarTexture;
            avatarStyle.alignment = TextAnchor.MiddleCenter;
            avatarStyle.fixedWidth = defaultAvatarSize;
            avatarStyle.fixedHeight = defaultAvatarSize;
            avatarStyle.border = GetAvatarBorder(AvatarShape.Circle, AvatarSize.Default);


            avatarFallbackStyle = new GUIStyle(GUI.skin.box);
            Color mutedBg = new Color(0.16f, 0.16f, 0.18f);
            avatarFallbackStyle.normal.background = CreateSolidTexture(mutedBg);
            avatarFallbackStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f);
            avatarFallbackStyle.alignment = TextAnchor.MiddleCenter;
            avatarFallbackStyle.fixedWidth = defaultAvatarSize;
            avatarFallbackStyle.fixedHeight = defaultAvatarSize;
            avatarFallbackStyle.border = GetAvatarBorder(AvatarShape.Circle, AvatarSize.Default);
            avatarFallbackStyle.fontSize = GetAvatarFontSize(AvatarSize.Default);


            avatarSmallStyle = new GUIStyle(avatarFallbackStyle);
            int smallAvatarSize = Mathf.RoundToInt(32 * guiHelper.uiScale);
            avatarSmallStyle.fixedWidth = smallAvatarSize;
            avatarSmallStyle.fixedHeight = smallAvatarSize;
            avatarSmallStyle.fontSize = GetAvatarFontSize(AvatarSize.Small);

            avatarLargeStyle = new GUIStyle(avatarFallbackStyle);
            int largeAvatarSize = Mathf.RoundToInt(48 * guiHelper.uiScale);
            avatarLargeStyle.fixedWidth = largeAvatarSize;
            avatarLargeStyle.fixedHeight = largeAvatarSize;
            avatarLargeStyle.fontSize = GetAvatarFontSize(AvatarSize.Large);
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
            skeletonStyle = new GUIStyle(GUI.skin.box);
            skeletonStyle.normal.background = skeletonTexture;
            skeletonStyle.border = new RectOffset(4, 4, 4, 4);
            skeletonStyle.padding = new RectOffset(0, 0, 0, 0);

            skeletonRoundedStyle = new GUIStyle(skeletonStyle);
            skeletonRoundedStyle.border = new RectOffset(8, 8, 8, 8);

            skeletonCircularStyle = new GUIStyle(skeletonStyle);
            skeletonCircularStyle.border = new RectOffset(50, 50, 50, 50);

            skeletonSmallStyle = new GUIStyle(skeletonStyle);
            skeletonSmallStyle.border = new RectOffset(2, 2, 2, 2);

            skeletonLargeStyle = new GUIStyle(skeletonStyle);
            skeletonLargeStyle.border = new RectOffset(6, 6, 6, 6);
        }

        /// <summary>
        /// Sets up the table styles.
        /// </summary>
        private void SetupTableStyles()
        {
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

            tableStyle = new GUIStyle(GUI.skin.box);
            tableStyle.normal.background = tableTexture;
            tableStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
            tableStyle.padding = new RectOffset(8, 8, 8, 8);

            tableHeaderStyle = new GUIStyle(GUI.skin.label);
            tableHeaderStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            tableHeaderStyle.fontStyle = FontStyle.Normal;
            tableHeaderStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f);
            tableHeaderStyle.normal.background = tableHeaderTexture;
            tableHeaderStyle.padding = new RectOffset(8, 8, 4, 4);
            tableHeaderStyle.alignment = TextAnchor.MiddleLeft;

            tableCellStyle = new GUIStyle(GUI.skin.label);
            tableCellStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            tableCellStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
            tableCellStyle.normal.background = tableCellTexture;
            tableCellStyle.padding = new RectOffset(8, 8, 4, 4);
            tableCellStyle.alignment = TextAnchor.MiddleLeft;

            tableStripedStyle = new GUIStyle(tableStyle);
            tableStripedStyle.normal.background = CreateSolidTexture(new Color(0.05f, 0.05f, 0.08f));

            tableBorderedStyle = new GUIStyle(tableStyle);
            tableBorderedStyle.normal.background = CreateOutlineTexture();

            tableHoverStyle = new GUIStyle(tableStyle);
            tableHoverStyle.hover.background = CreateSolidTexture(new Color(0.1f, 0.1f, 0.15f));
        }
        #endregion

        #region Style Getters
        /// <summary>
        /// Gets the button style for the given variant and size.
        /// </summary>
        public GUIStyle GetButtonStyle(ButtonVariant variant, ButtonSize size)
        {
            GUIStyle baseStyle;
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

            if (baseStyle == null) return GUI.skin.button;


            switch (size)
            {
                case ButtonSize.Small:
                    return ApplyVariantToSizeStyle(buttonSmallStyle, variant);
                case ButtonSize.Large:
                    return ApplyVariantToSizeStyle(buttonLargeStyle, variant);
                case ButtonSize.Icon:
                    return ApplyVariantToSizeStyle(buttonIconStyle, variant);
                default:
                    return baseStyle;
            }
        }

        private GUIStyle ApplyVariantToSizeStyle(GUIStyle sizeStyle, ButtonVariant variant)
        {
            if (sizeStyle == null) return GUI.skin.button;

            GUIStyle style = new GUIStyle(sizeStyle);


            switch (variant)
            {
                case ButtonVariant.Default:
                    Color primaryBg = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
                    Color primaryFg = new Color(0.98f, 0.98f, 0.98f);
                    style.normal.background = CreateSolidTexture(primaryBg);
                    style.hover.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.black, 0.1f));
                    style.normal.textColor = primaryFg;
                    style.hover.textColor = primaryFg;
                    style.active.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.black, 0.2f));
                    style.active.textColor = primaryFg;
                    break;
                case ButtonVariant.Destructive:
                    Color destructiveBg = new Color(0.86f, 0.24f, 0.24f);
                    Color destructiveFg = new Color(0.98f, 0.98f, 0.98f);
                    style.normal.background = CreateSolidTexture(destructiveBg);
                    style.hover.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.1f));
                    style.normal.textColor = destructiveFg;
                    style.hover.textColor = destructiveFg;
                    style.active.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.2f));
                    style.active.textColor = destructiveFg;
                    break;
                case ButtonVariant.Outline:
                    Color outlineBorder = new Color(0.23f, 0.23f, 0.27f);
                    Color outlineBg = new Color(0.02f, 0.02f, 0.04f);
                    Color outlineFg = new Color(0.98f, 0.98f, 0.98f);
                    Color outlineHoverBg = new Color(0.16f, 0.16f, 0.18f);
                    Color outlineHoverFg = new Color(0.98f, 0.98f, 0.98f);

                    style.normal.background = CreateOutlineButtonTexture(outlineBg, outlineBorder);
                    style.hover.background = CreateSolidTexture(outlineHoverBg);
                    style.normal.textColor = outlineFg;
                    style.hover.textColor = outlineHoverFg;
                    style.active.background = CreateSolidTexture(Color.Lerp(outlineHoverBg, Color.black, 0.2f));
                    style.active.textColor = outlineHoverFg;
                    break;
                case ButtonVariant.Secondary:
                    Color secondaryBg = new Color(0.16f, 0.16f, 0.18f);
                    Color secondaryFg = new Color(0.98f, 0.98f, 0.98f);
                    style.normal.background = CreateSolidTexture(secondaryBg);
                    style.hover.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.white, 0.2f));
                    style.normal.textColor = secondaryFg;
                    style.hover.textColor = secondaryFg;
                    style.active.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.black, 0.2f));
                    style.active.textColor = secondaryFg;
                    break;
                case ButtonVariant.Ghost:
                    Color ghostFg = new Color(0.98f, 0.98f, 0.98f);
                    Color ghostHoverBg = new Color(0.16f, 0.16f, 0.18f);
                    Color ghostHoverFg = new Color(0.98f, 0.98f, 0.98f);

                    style.normal.background = transparentTexture;
                    style.hover.background = CreateSolidTexture(ghostHoverBg);
                    style.normal.textColor = ghostFg;
                    style.hover.textColor = ghostHoverFg;
                    style.active.background = CreateSolidTexture(Color.Lerp(ghostHoverBg, Color.black, 0.2f));
                    style.active.textColor = ghostHoverFg;
                    break;
                case ButtonVariant.Link:
                    Color linkColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
                    Color linkHoverColor = Color.Lerp(linkColor, Color.white, 0.2f);

                    style.normal.background = transparentTexture;
                    style.hover.background = transparentTexture;
                    style.normal.textColor = linkColor;
                    style.hover.textColor = linkHoverColor;
                    style.active.background = transparentTexture;
                    style.active.textColor = Color.Lerp(linkHoverColor, Color.black, 0.2f);
                    style.fontStyle = FontStyle.Normal;
                    break;
            }

            return style;
        }

        /// <summary>
        /// Gets the toggle style for the given variant and size.
        /// </summary>
        public GUIStyle GetToggleStyle(ToggleVariant variant, ToggleSize size)
        {
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

            if (baseStyle == null) return GUI.skin.button;

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
            if (disabled) return inputDisabledStyle ?? GUI.skin.textField;
            if (focused) return inputFocusedStyle ?? GUI.skin.textField;

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
            return passwordFieldStyle ?? GUI.skin.textField;
        }

        /// <summary>
        /// Gets the text area style for the given variant and focus state.
        /// </summary>
        public GUIStyle GetTextAreaStyle(TextAreaVariant variant = TextAreaVariant.Default, bool focused = false)
        {
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
            return orientation == SeparatorOrientation.Horizontal ?
                (separatorHorizontalStyle ?? GUI.skin.box) :
                (separatorVerticalStyle ?? GUI.skin.box);
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
            return active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) :
                           (tabsTriggerStyle ?? GUI.skin.button);
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

            if (baseStyle == null) return GUI.skin.toggle;

            switch (size)
            {
                case CheckboxSize.Small:
                    return checkboxSmallStyle ?? baseStyle;
                case CheckboxSize.Large:
                    return checkboxLargeStyle ?? baseStyle;
                default:
                    return baseStyle;
            }
        }

        /// <summary>
        /// Gets the switch style for the given variant and size.
        /// </summary>
        public GUIStyle GetSwitchStyle(SwitchVariant variant, SwitchSize size)
        {
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

            if (baseStyle == null) return GUI.skin.toggle;

            switch (size)
            {
                case SwitchSize.Small:
                    return switchSmallStyle ?? baseStyle;
                case SwitchSize.Large:
                    return switchLargeStyle ?? baseStyle;
                default:
                    return baseStyle;
            }
        }

        /// <summary>
        /// Gets the badge style for the given variant and size.
        /// </summary>
        public GUIStyle GetBadgeStyle(BadgeVariant variant, BadgeSize size)
        {
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

            if (baseStyle == null) return GUI.skin.box;

            switch (size)
            {
                case BadgeSize.Small:
                    return badgeSmallStyle ?? baseStyle;
                case BadgeSize.Large:
                    return badgeLargeStyle ?? baseStyle;
                default:
                    return baseStyle;
            }
        }

        /// <summary>
        /// Gets the alert style for the given variant and type.
        /// </summary>
        public GUIStyle GetAlertStyle(AlertVariant variant, AlertType type)
        {
            GUIStyle baseStyle = variant == AlertVariant.Destructive ? alertDestructiveStyle : alertDefaultStyle;
            return baseStyle ?? GUI.skin.box;
        }

        /// <summary>
        /// Gets the alert title style for the given type.
        /// </summary>
        public GUIStyle GetAlertTitleStyle(AlertType type)
        {
            return alertTitleStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the alert description style for the given type.
        /// </summary>
        public GUIStyle GetAlertDescriptionStyle(AlertType type)
        {
            return alertDescriptionStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the avatar style for the given size and shape.
        /// </summary>
        public GUIStyle GetAvatarStyle(AvatarSize size, AvatarShape shape)
        {
            GUIStyle baseStyle = avatarStyle;
            if (baseStyle == null) return GUI.skin.box;

            GUIStyle sizedStyle = new GUIStyle(baseStyle);
            switch (size)
            {
                case AvatarSize.Small:
                    sizedStyle = avatarSmallStyle ?? baseStyle;
                    break;
                case AvatarSize.Large:
                    sizedStyle = avatarLargeStyle ?? baseStyle;
                    break;
            }


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
            GUIStyle baseStyle = avatarFallbackStyle;
            if (baseStyle == null) return GUI.skin.box;

            GUIStyle sizedStyle = new GUIStyle(baseStyle);
            switch (size)
            {
                case AvatarSize.Small:
                    sizedStyle = avatarSmallStyle ?? baseStyle;
                    break;
                case AvatarSize.Large:
                    sizedStyle = avatarLargeStyle ?? baseStyle;
                    break;
            }


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

            if (baseStyle == null) return GUI.skin.box;

            switch (size)
            {
                case SkeletonSize.Small:
                    return skeletonSmallStyle ?? baseStyle;
                case SkeletonSize.Large:
                    return skeletonLargeStyle ?? baseStyle;
                default:
                    return baseStyle;
            }
        }

        /// <summary>
        /// Gets the table style for the given variant and size.
        /// </summary>
        public GUIStyle GetTableStyle(TableVariant variant, TableSize size)
        {
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
            return tableHeaderStyle ?? GUI.skin.label;
        }

        /// <summary>
        /// Gets the table cell style for the given variant and size.
        /// </summary>
        public GUIStyle GetTableCellStyle(TableVariant variant, TableSize size)
        {
            return tableCellStyle ?? GUI.skin.label;
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Cleans up the textures created by the style manager.
        /// </summary>
        public void Cleanup()
        {
            if (gradientTexture) Object.Destroy(gradientTexture);
            if (glowTexture) Object.Destroy(glowTexture);
            if (particleTexture) Object.Destroy(particleTexture);
            if (cardBackgroundTexture) Object.Destroy(cardBackgroundTexture);
            if (outlineTexture) Object.Destroy(outlineTexture);
            if (transparentTexture) Object.Destroy(transparentTexture);
            if (inputBackgroundTexture) Object.Destroy(inputBackgroundTexture);
            if (inputFocusedTexture) Object.Destroy(inputFocusedTexture);
            if (progressBarBackgroundTexture) Object.Destroy(progressBarBackgroundTexture);
            if (progressBarFillTexture) Object.Destroy(progressBarFillTexture);
            if (separatorTexture) Object.Destroy(separatorTexture);
            if (tabsBackgroundTexture) Object.Destroy(tabsBackgroundTexture);
            if (tabsActiveTexture) Object.Destroy(tabsActiveTexture);


            if (checkboxTexture) Object.Destroy(checkboxTexture);
            if (checkboxCheckedTexture) Object.Destroy(checkboxCheckedTexture);
            if (switchTexture) Object.Destroy(switchTexture);
            if (switchOnTexture) Object.Destroy(switchOnTexture);
            if (switchOffTexture) Object.Destroy(switchOffTexture);
            if (badgeTexture) Object.Destroy(badgeTexture);
            if (alertTexture) Object.Destroy(alertTexture);
            if (avatarTexture) Object.Destroy(avatarTexture);
            if (skeletonTexture) Object.Destroy(skeletonTexture);
            if (tableTexture) Object.Destroy(tableTexture);
            if (tableHeaderTexture) Object.Destroy(tableHeaderTexture);
            if (tableCellTexture) Object.Destroy(tableCellTexture);
        }
        #endregion
    }
}
