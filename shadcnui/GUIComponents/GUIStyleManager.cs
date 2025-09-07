using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace shadcnui.GUIComponents
{
    public enum ButtonVariant
    {
        Default,
        Destructive,
        Outline,
        Secondary,
        Ghost,
        Link
    }

    public enum ButtonSize
    {
        Default,
        Small,
        Large,
        Icon
    }

    public enum ToggleVariant
    {
        Default,
        Outline
    }

    public enum ToggleSize
    {
        Default,
        Small,
        Large
    }

    public enum InputVariant
    {
        Default,
        Outline,
        Ghost
    }

    public enum LabelVariant
    {
        Default,
        Secondary,
        Muted,
        Destructive
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical
    }

    public enum TextAreaVariant
    {
        Default,
        Outline,
        Ghost
    }

    public enum ProgressVariant
    {
        Default
    }

    public enum TabsVariant
    {
        Default
    }

    public class GUIStyleManager
    {
        private GUIHelper guiHelper;


        public GUIStyle animatedBoxStyle;
        public GUIStyle animatedButtonStyle;
        public GUIStyle animatedInputStyle;
        public GUIStyle glowLabelStyle;
        public GUIStyle titleStyle;
        public GUIStyle colorPresetStyle;
        public GUIStyle sectionHeaderStyle;


        public GUIStyle cardStyle;
        public GUIStyle cardHeaderStyle;
        public GUIStyle cardTitleStyle;
        public GUIStyle cardDescriptionStyle;
        public GUIStyle cardContentStyle;
        public GUIStyle cardFooterStyle;


        public GUIStyle buttonDefaultStyle;
        public GUIStyle buttonDestructiveStyle;
        public GUIStyle buttonOutlineStyle;
        public GUIStyle buttonSecondaryStyle;
        public GUIStyle buttonGhostStyle;
        public GUIStyle buttonLinkStyle;


        public GUIStyle buttonSmallStyle;
        public GUIStyle buttonLargeStyle;
        public GUIStyle buttonIconStyle;


        public GUIStyle toggleDefaultStyle;
        public GUIStyle toggleOutlineStyle;
        public GUIStyle toggleSmallStyle;
        public GUIStyle toggleLargeStyle;


        public GUIStyle inputDefaultStyle;
        public GUIStyle inputOutlineStyle;
        public GUIStyle inputGhostStyle;
        public GUIStyle inputFocusedStyle;
        public GUIStyle inputDisabledStyle;


        public GUIStyle labelDefaultStyle;
        public GUIStyle labelSecondaryStyle;
        public GUIStyle labelMutedStyle;
        public GUIStyle labelDestructiveStyle;
        public GUIStyle labelSmallStyle;
        public GUIStyle labelLargeStyle;


        public GUIStyle passwordFieldStyle;


        public GUIStyle textAreaStyle;
        public GUIStyle textAreaFocusedStyle;
        public GUIStyle textAreaOutlineStyle;
        public GUIStyle textAreaGhostStyle;

       
        public GUIStyle progressBarStyle;
        public GUIStyle progressBarBackgroundStyle;
        public GUIStyle progressBarFillStyle;

       
        public GUIStyle separatorHorizontalStyle;
        public GUIStyle separatorVerticalStyle;

       
        public GUIStyle tabsListStyle;
        public GUIStyle tabsTriggerStyle;
        public GUIStyle tabsTriggerActiveStyle;
        public GUIStyle tabsContentStyle;

       
        public GUIStyle labelSmallScaledStyle;
        public GUIStyle labelMediumScaledStyle;
        public GUIStyle labelLargeScaledStyle;


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

        public GUIStyleManager(GUIHelper helper)
        {
            guiHelper = helper;
        }

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
        }

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
        }

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

        private void SetupButtonVariantStyles()
        {
           
            float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
            int basePadding = Mathf.RoundToInt(16 * guiHelper.uiScale);
            int baseVerticalPadding = Mathf.RoundToInt(8 * guiHelper.uiScale);
            int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);

           
            buttonDefaultStyle = CreateBaseButtonStyle();
            buttonDefaultStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonDefaultStyle.fontStyle = FontStyle.Bold;
            buttonDefaultStyle.alignment = TextAnchor.MiddleCenter;
            buttonDefaultStyle.padding = new RectOffset(basePadding, basePadding, baseVerticalPadding, baseVerticalPadding);
            buttonDefaultStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color primaryBg = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            Color primaryFg = new Color(0.98f, 0.98f, 0.98f);
            buttonDefaultStyle.normal.background = CreateSolidTexture(primaryBg);
            buttonDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.white, 0.1f));
            buttonDefaultStyle.normal.textColor = primaryFg;
            buttonDefaultStyle.hover.textColor = primaryFg;

           
            buttonDestructiveStyle = CreateBaseButtonStyle();
            buttonDestructiveStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonDestructiveStyle.fontStyle = FontStyle.Bold;
            buttonDestructiveStyle.alignment = TextAnchor.MiddleCenter;
            buttonDestructiveStyle.padding = new RectOffset(basePadding, basePadding, baseVerticalPadding, baseVerticalPadding);
            buttonDestructiveStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color destructiveBg = new Color(0.86f, 0.24f, 0.24f);
            Color destructiveFg = new Color(0.98f, 0.98f, 0.98f);
            buttonDestructiveStyle.normal.background = CreateSolidTexture(destructiveBg);
            buttonDestructiveStyle.hover.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.1f));
            buttonDestructiveStyle.normal.textColor = destructiveFg;
            buttonDestructiveStyle.hover.textColor = destructiveFg;

           
            buttonOutlineStyle = CreateBaseButtonStyle();
            buttonOutlineStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonOutlineStyle.fontStyle = FontStyle.Bold;
            buttonOutlineStyle.alignment = TextAnchor.MiddleCenter;
            buttonOutlineStyle.padding = new RectOffset(basePadding, basePadding, baseVerticalPadding, baseVerticalPadding);
            buttonOutlineStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color outlineBg = new Color(0.02f, 0.02f, 0.04f);
            Color outlineFg = new Color(0.98f, 0.98f, 0.98f);
            Color outlineHoverBg = new Color(0.16f, 0.16f, 0.18f);
            buttonOutlineStyle.normal.background = CreateOutlineButtonTexture(outlineBg);
            buttonOutlineStyle.hover.background = CreateSolidTexture(outlineHoverBg);
            buttonOutlineStyle.normal.textColor = outlineFg;
            buttonOutlineStyle.hover.textColor = new Color(0.16f, 0.16f, 0.18f);

           
            buttonSecondaryStyle = CreateBaseButtonStyle();
            buttonSecondaryStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonSecondaryStyle.fontStyle = FontStyle.Bold;
            buttonSecondaryStyle.alignment = TextAnchor.MiddleCenter;
            buttonSecondaryStyle.padding = new RectOffset(basePadding, basePadding, baseVerticalPadding, baseVerticalPadding);
            buttonSecondaryStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color secondaryBg = new Color(0.16f, 0.16f, 0.18f);
            Color secondaryFg = new Color(0.98f, 0.98f, 0.98f);
            buttonSecondaryStyle.normal.background = CreateSolidTexture(secondaryBg);
            buttonSecondaryStyle.hover.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.white, 0.2f));
            buttonSecondaryStyle.normal.textColor = secondaryFg;
            buttonSecondaryStyle.hover.textColor = secondaryFg;

           
            buttonGhostStyle = CreateBaseButtonStyle();
            buttonGhostStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonGhostStyle.fontStyle = FontStyle.Bold;
            buttonGhostStyle.alignment = TextAnchor.MiddleCenter;
            buttonGhostStyle.padding = new RectOffset(basePadding, basePadding, baseVerticalPadding, baseVerticalPadding);
            buttonGhostStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

            Color ghostFg = new Color(0.98f, 0.98f, 0.98f);
            Color ghostHoverBg = new Color(0.16f, 0.16f, 0.18f);
            buttonGhostStyle.normal.background = transparentTexture;
            buttonGhostStyle.hover.background = CreateSolidTexture(ghostHoverBg);
            buttonGhostStyle.normal.textColor = ghostFg;
            buttonGhostStyle.hover.textColor = new Color(0.16f, 0.16f, 0.18f);

           
            buttonLinkStyle = CreateBaseButtonStyle();
            buttonLinkStyle.fontSize = Mathf.RoundToInt(scaledFontSize);
            buttonLinkStyle.fontStyle = FontStyle.Normal;
            buttonLinkStyle.alignment = TextAnchor.MiddleCenter;
            buttonLinkStyle.padding = new RectOffset(0, 0, baseVerticalPadding, baseVerticalPadding);
            buttonLinkStyle.border = new RectOffset(0, 0, 0, 0);

            Color linkColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
            buttonLinkStyle.normal.background = transparentTexture;
            buttonLinkStyle.hover.background = transparentTexture;
            buttonLinkStyle.normal.textColor = linkColor;
            buttonLinkStyle.hover.textColor = Color.Lerp(linkColor, Color.white, 0.2f);

           
            SetupButtonSizeVariants(scaledFontSize, guiHelper.uiScale, borderRadius);
        }

        private GUIStyle CreateBaseButtonStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.wordWrap = false;
            style.clipping = TextClipping.Clip;
            return style;
        }

        private void SetupButtonSizeVariants(float baseFontSize, float scale, int borderRadius)
        {
           
            buttonSmallStyle = CreateBaseButtonStyle();
            buttonSmallStyle.fontSize = Mathf.RoundToInt((baseFontSize - 2) * scale);
            buttonSmallStyle.fontStyle = FontStyle.Bold;
            buttonSmallStyle.alignment = TextAnchor.MiddleCenter;
            buttonSmallStyle.padding = new RectOffset(
                Mathf.RoundToInt(12 * scale),
                Mathf.RoundToInt(12 * scale),
                Mathf.RoundToInt(4 * scale),
                Mathf.RoundToInt(4 * scale)
            );
            buttonSmallStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

           
            buttonLargeStyle = CreateBaseButtonStyle();
            buttonLargeStyle.fontSize = Mathf.RoundToInt(baseFontSize * scale);
            buttonLargeStyle.fontStyle = FontStyle.Bold;
            buttonLargeStyle.alignment = TextAnchor.MiddleCenter;
            buttonLargeStyle.padding = new RectOffset(
                Mathf.RoundToInt(32 * scale),
                Mathf.RoundToInt(32 * scale),
                Mathf.RoundToInt(10 * scale),
                Mathf.RoundToInt(10 * scale)
            );
            buttonLargeStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);

           
            buttonIconStyle = CreateBaseButtonStyle();
            buttonIconStyle.fontSize = Mathf.RoundToInt(baseFontSize * scale);
            buttonIconStyle.fontStyle = FontStyle.Bold;
            buttonIconStyle.alignment = TextAnchor.MiddleCenter;
            buttonIconStyle.padding = new RectOffset(
                Mathf.RoundToInt(8 * scale),
                Mathf.RoundToInt(8 * scale),
                Mathf.RoundToInt(8 * scale),
                Mathf.RoundToInt(8 * scale)
            );
            buttonIconStyle.border = new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor)
        {
            Texture2D texture = new Texture2D(4, 4);
            Color borderColor = new Color(0.23f, 0.23f, 0.27f);

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

        private void SetupInputVariantStyles()
        {

            inputDefaultStyle = new GUIStyle(GUI.skin.textField);
            inputDefaultStyle.fontSize = guiHelper.fontSize;
            inputDefaultStyle.padding = new RectOffset(12, 12, 4, 4);
            inputDefaultStyle.margin = new RectOffset(0, 0, 4, 4);
            inputDefaultStyle.border = new RectOffset(1, 1, 1, 1);
            inputDefaultStyle.normal.background = inputBackgroundTexture;
            inputDefaultStyle.normal.textColor = Color.white;
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
                    style.hover.background = CreateSolidTexture(Color.Lerp(primaryBg, Color.white, 0.1f));
                    style.normal.textColor = primaryFg;
                    style.hover.textColor = primaryFg;
                    break;
                case ButtonVariant.Destructive:
                    Color destructiveBg = new Color(0.86f, 0.24f, 0.24f);
                    Color destructiveFg = new Color(0.98f, 0.98f, 0.98f);
                    style.normal.background = CreateSolidTexture(destructiveBg);
                    style.hover.background = CreateSolidTexture(Color.Lerp(destructiveBg, Color.black, 0.1f));
                    style.normal.textColor = destructiveFg;
                    style.hover.textColor = destructiveFg;
                    break;
                case ButtonVariant.Outline:
                    Color outlineBg = new Color(0.02f, 0.02f, 0.04f);
                    Color outlineFg = new Color(0.98f, 0.98f, 0.98f);
                    Color outlineHoverBg = new Color(0.16f, 0.16f, 0.18f);
                    style.normal.background = CreateOutlineButtonTexture(outlineBg);
                    style.hover.background = CreateSolidTexture(outlineHoverBg);
                    style.normal.textColor = outlineFg;
                    style.hover.textColor = new Color(0.16f, 0.16f, 0.18f);
                    break;
                case ButtonVariant.Secondary:
                    Color secondaryBg = new Color(0.16f, 0.16f, 0.18f);
                    Color secondaryFg = new Color(0.98f, 0.98f, 0.98f);
                    style.normal.background = CreateSolidTexture(secondaryBg);
                    style.hover.background = CreateSolidTexture(Color.Lerp(secondaryBg, Color.white, 0.2f));
                    style.normal.textColor = secondaryFg;
                    style.hover.textColor = secondaryFg;
                    break;
                case ButtonVariant.Ghost:
                    Color ghostFg = new Color(0.98f, 0.98f, 0.98f);
                    Color ghostHoverBg = new Color(0.16f, 0.16f, 0.18f);
                    style.normal.background = transparentTexture;
                    style.hover.background = CreateSolidTexture(ghostHoverBg);
                    style.normal.textColor = ghostFg;
                    style.hover.textColor = new Color(0.16f, 0.16f, 0.18f);
                    break;
                case ButtonVariant.Link:
                    Color linkColor = guiHelper.customColorsEnabled ? guiHelper.primaryColor : new Color(0.09f, 0.09f, 0.11f);
                    style.normal.background = transparentTexture;
                    style.hover.background = transparentTexture;
                    style.normal.textColor = linkColor;
                    style.hover.textColor = Color.Lerp(linkColor, Color.white, 0.2f);
                    style.fontStyle = FontStyle.Normal;
                    break;
            }

            return style;
        }

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
            tabsTriggerStyle.hover.background = CreateSolidTexture(new Color(0.16f, 0.16f, 0.18f, 0.5f));

           
            tabsTriggerActiveStyle = new GUIStyle(tabsTriggerStyle);
            tabsTriggerActiveStyle.normal.background = tabsActiveTexture;
            tabsTriggerActiveStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f);
            tabsTriggerActiveStyle.hover.background = tabsActiveTexture;

           
            tabsContentStyle = new GUIStyle();
            tabsContentStyle.padding = new RectOffset(0, 0, Mathf.RoundToInt(8 * guiHelper.uiScale), 0);
        }

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
            textAreaStyle.wordWrap = true;
            textAreaStyle.stretchHeight = true;

           
            textAreaFocusedStyle = new GUIStyle(textAreaStyle);
            textAreaFocusedStyle.normal.background = inputFocusedTexture;
            textAreaFocusedStyle.border = new RectOffset(2, 2, 2, 2);

           
            textAreaOutlineStyle = new GUIStyle(textAreaStyle);
            textAreaOutlineStyle.normal.background = CreateOutlineTexture();
            textAreaOutlineStyle.focused.background = CreateSolidTexture(guiHelper.customColorsEnabled ?
                new Color(guiHelper.accentColor.r, guiHelper.accentColor.g, guiHelper.accentColor.b, 0.1f) :
                new Color(0.3f, 0.4f, 0.6f, 0.1f));

           
            textAreaGhostStyle = new GUIStyle(textAreaStyle);
            textAreaGhostStyle.normal.background = transparentTexture;
            textAreaGhostStyle.focused.background = CreateSolidTexture(new Color(0.1f, 0.1f, 0.2f, 0.3f));
        }

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

        public GUIStyle GetPasswordFieldStyle()
        {
            return passwordFieldStyle ?? GUI.skin.textField;
        }

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

        public GUIStyle GetProgressBarStyle() => progressBarStyle ?? GUI.skin.box;
        public GUIStyle GetProgressBarBackgroundStyle() => progressBarBackgroundStyle ?? GUI.skin.box;
        public GUIStyle GetProgressBarFillStyle() => progressBarFillStyle ?? GUI.skin.box;

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation)
        {
            return orientation == SeparatorOrientation.Horizontal ?
                (separatorHorizontalStyle ?? GUI.skin.box) :
                (separatorVerticalStyle ?? GUI.skin.box);
        }

        public GUIStyle GetTabsListStyle() => tabsListStyle ?? GUI.skin.box;
        public GUIStyle GetTabsTriggerStyle(bool active = false)
        {
            return active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) :
                           (tabsTriggerStyle ?? GUI.skin.button);
        }
        public GUIStyle GetTabsContentStyle() => tabsContentStyle ?? GUIStyle.none;

        public Texture2D GetGlowTexture() => glowTexture;
        public Texture2D GetParticleTexture() => particleTexture;
        public Texture2D GetInputBackgroundTexture() => inputBackgroundTexture;
        public Texture2D GetInputFocusedTexture() => inputFocusedTexture;
        public Texture2D GetTransparentTexture() => transparentTexture;
        public Texture2D GetProgressBarBackgroundTexture() => progressBarBackgroundTexture;
        public Texture2D GetProgressBarFillTexture() => progressBarFillTexture;

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
        }
    }
}
