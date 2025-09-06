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

       
        private Texture2D gradientTexture;
        private Texture2D glowTexture;
        private Texture2D particleTexture;
        private Texture2D cardBackgroundTexture;
        private Texture2D outlineTexture;
        private Texture2D transparentTexture;
        private Texture2D inputBackgroundTexture;
        private Texture2D inputFocusedTexture;

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
           
            buttonDefaultStyle = new GUIStyle(GUI.skin.button);
            buttonDefaultStyle.fontSize = guiHelper.fontSize;
            buttonDefaultStyle.fontStyle = FontStyle.Normal;
            buttonDefaultStyle.alignment = TextAnchor.MiddleCenter;
            buttonDefaultStyle.normal.textColor = Color.white;
            buttonDefaultStyle.hover.textColor = new Color(0.9f, 0.9f, 0.9f);
            if (guiHelper.customColorsEnabled)
            {
                buttonDefaultStyle.normal.background = CreateSolidTexture(guiHelper.primaryColor);
                buttonDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(guiHelper.primaryColor, Color.white, 0.1f));
            }

           
            buttonDestructiveStyle = new GUIStyle(buttonDefaultStyle);
            Color destructiveColor = new Color(0.8f, 0.2f, 0.2f);
            buttonDestructiveStyle.normal.background = CreateSolidTexture(destructiveColor);
            buttonDestructiveStyle.hover.background = CreateSolidTexture(Color.Lerp(destructiveColor, Color.white, 0.1f));

           
            buttonOutlineStyle = new GUIStyle(GUI.skin.button);
            buttonOutlineStyle.fontSize = guiHelper.fontSize;
            buttonOutlineStyle.fontStyle = FontStyle.Normal;
            buttonOutlineStyle.alignment = TextAnchor.MiddleCenter;
            buttonOutlineStyle.normal.textColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.8f, 0.8f, 0.9f);
            buttonOutlineStyle.hover.textColor = Color.white;
            buttonOutlineStyle.normal.background = CreateOutlineTexture();
            buttonOutlineStyle.hover.background = CreateSolidTexture(guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.3f, 0.3f, 0.5f));

           
            buttonSecondaryStyle = new GUIStyle(buttonDefaultStyle);
            Color secondaryColor = guiHelper.customColorsEnabled ? guiHelper.secondaryColor : new Color(0.4f, 0.4f, 0.5f);
            buttonSecondaryStyle.normal.background = CreateSolidTexture(secondaryColor);
            buttonSecondaryStyle.hover.background = CreateSolidTexture(Color.Lerp(secondaryColor, Color.white, 0.1f));

           
            buttonGhostStyle = new GUIStyle(GUI.skin.button);
            buttonGhostStyle.fontSize = guiHelper.fontSize;
            buttonGhostStyle.fontStyle = FontStyle.Normal;
            buttonGhostStyle.alignment = TextAnchor.MiddleCenter;
            buttonGhostStyle.normal.textColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.8f, 0.8f, 0.9f);
            buttonGhostStyle.hover.textColor = Color.white;
            buttonGhostStyle.normal.background = transparentTexture;
            buttonGhostStyle.hover.background = CreateSolidTexture(new Color(0.2f, 0.2f, 0.3f, 0.5f));

           
            buttonLinkStyle = new GUIStyle(GUI.skin.label);
            buttonLinkStyle.fontSize = guiHelper.fontSize;
            buttonLinkStyle.fontStyle = FontStyle.Normal;
            buttonLinkStyle.alignment = TextAnchor.MiddleCenter;
            buttonLinkStyle.normal.textColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.5f, 0.7f, 1f);
            buttonLinkStyle.hover.textColor = Color.Lerp(buttonLinkStyle.normal.textColor, Color.white, 0.3f);

           
            buttonSmallStyle = new GUIStyle(buttonDefaultStyle);
            buttonSmallStyle.fontSize = guiHelper.fontSize - 2;
            buttonSmallStyle.padding = new RectOffset(12, 12, 4, 4);

            buttonLargeStyle = new GUIStyle(buttonDefaultStyle);
            buttonLargeStyle.fontSize = guiHelper.fontSize + 2;
            buttonLargeStyle.padding = new RectOffset(32, 32, 8, 8);

            buttonIconStyle = new GUIStyle(buttonDefaultStyle);
            buttonIconStyle.padding = new RectOffset(8, 8, 8, 8);
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

        private Texture2D CreateSolidTexture(Color color)
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

            GUIStyle sizedStyle = new GUIStyle(baseStyle);
            switch (size)
            {
                case ButtonSize.Small:
                    sizedStyle.fontSize = guiHelper.fontSize - 2;
                    sizedStyle.padding = new RectOffset(12, 12, 4, 4);
                    break;
                case ButtonSize.Large:
                    sizedStyle.fontSize = guiHelper.fontSize + 2;
                    sizedStyle.padding = new RectOffset(32, 32, 8, 8);
                    break;
                case ButtonSize.Icon:
                    sizedStyle.padding = new RectOffset(8, 8, 8, 8);
                    break;
            }

            return sizedStyle;
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

        public GUIStyle GetTextAreaStyle(bool focused = false)
        {
            return focused ? (textAreaFocusedStyle ?? textAreaStyle ?? GUI.skin.textArea) :
                            (textAreaStyle ?? GUI.skin.textArea);
        }

        public Texture2D GetGlowTexture() => glowTexture;
        public Texture2D GetParticleTexture() => particleTexture;
        public Texture2D GetInputBackgroundTexture() => inputBackgroundTexture;
        public Texture2D GetInputFocusedTexture() => inputFocusedTexture;

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
        }
    }
}
