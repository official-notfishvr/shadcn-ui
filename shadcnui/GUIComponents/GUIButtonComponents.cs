using shadcnui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIButtonComponents
    {
        private GUIHelper guiHelper;

        public GUIButtonComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }


        public bool RenderGlowButton(string text, int buttonIndex, int hoveredButton, float[] buttonGlowEffects,
            Vector2 mousePos, float menuAlpha)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(text), styleManager.animatedButtonStyle,
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Height(guiHelper.buttonHeight * guiHelper.uiScale), 
                    GUILayout.ExpandWidth(true) 
                });
#else
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(text), styleManager.animatedButtonStyle,
                GUILayout.Height(guiHelper.buttonHeight * guiHelper.uiScale), GUILayout.ExpandWidth(true));
#endif

            if (guiHelper.glowEffectsEnabled && buttonGlowEffects[buttonIndex] > 0.1f)
            {
                Color glowColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.8f, 0.9f, 1f);
                GUI.color = new Color(glowColor.r, glowColor.g, glowColor.b, buttonGlowEffects[buttonIndex] * 0.5f * menuAlpha);
                GUI.DrawTexture(new Rect(buttonRect.x - 5, buttonRect.y - 3, buttonRect.width + 10, buttonRect.height + 6),
                    styleManager.GetGlowTexture());
            }

            Color buttonColor = Color.white;
            if (guiHelper.hoverEffectsEnabled && hoveredButton == buttonIndex)
            {
                Color hoverColor = guiHelper.customColorsEnabled ? guiHelper.accentColor : new Color(0.8f, 0.9f, 1f);
                buttonColor = Color.Lerp(Color.white, hoverColor, buttonGlowEffects[buttonIndex]);
            }

            GUI.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, menuAlpha);

            return GUI.Button(buttonRect, text, styleManager.animatedButtonStyle);
        }

        public bool RenderColorPresetButton(string colorName, Color presetColor)
        {
            var styleManager = guiHelper.GetStyleManager();
            Color originalBg = GUI.backgroundColor;
            GUI.backgroundColor = guiHelper.customColorsEnabled ?
                Color.Lerp(presetColor, guiHelper.primaryColor, 0.3f) : presetColor;

#if IL2CPP
            bool clicked = GUILayout.Button(colorName, styleManager.colorPresetStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Height(25 * guiHelper.uiScale), 
                    GUILayout.ExpandWidth(true) 
                });
#else
            bool clicked = GUILayout.Button(colorName, styleManager.colorPresetStyle,
                GUILayout.Height(25 * guiHelper.uiScale), GUILayout.ExpandWidth(true));
#endif

            GUI.backgroundColor = originalBg;
            return clicked;
        }

        public bool DrawButton(float windowWidth, string text, Action onClick)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            bool clicked = GUILayout.Button(text, styleManager.animatedButtonStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            bool clicked = GUILayout.Button(text, styleManager.animatedButtonStyle);
#endif
            if (clicked && onClick != null)
                onClick();
            GUILayout.Space(guiHelper.controlSpacing);
            return clicked;
        }

        public bool DrawColoredButton(float windowWidth, string text, Color color, Action onClick)
        {
            var styleManager = guiHelper.GetStyleManager();
            Color originalColor = GUI.backgroundColor;
            Color buttonColor = guiHelper.customColorsEnabled ?
                Color.Lerp(color, guiHelper.primaryColor, 0.2f) : color;
            GUI.backgroundColor = buttonColor;
#if IL2CPP
            bool clicked = GUILayout.Button(text, styleManager.animatedButtonStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            bool clicked = GUILayout.Button(text, styleManager.animatedButtonStyle);
#endif
            GUI.backgroundColor = originalColor;
            if (clicked && onClick != null)
                onClick();
            GUILayout.Space(guiHelper.controlSpacing);
            return clicked;
        }

        public bool DrawFixedButton(string text, float width, float height, Action onClick = null)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            bool clicked = GUILayout.Button(text, styleManager.animatedButtonStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(width * guiHelper.uiScale), 
                    GUILayout.Height(height * guiHelper.uiScale) 
                });
#else
            bool clicked = GUILayout.Button(text, styleManager.animatedButtonStyle,
                GUILayout.Width(width * guiHelper.uiScale), GUILayout.Height(height * guiHelper.uiScale));
#endif
            if (clicked && onClick != null)
                onClick();
            return clicked;
        }


        public bool Button(string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

           
            GUILayoutOption[] autoScaledOptions = GetAutoScaledOptions(size, options);

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool clicked;
#if IL2CPP
            clicked = GUILayout.Button(text ?? "Button", buttonStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)autoScaledOptions);
#else
            clicked = GUILayout.Button(text ?? "Button", buttonStyle, autoScaledOptions);
#endif

            GUI.enabled = wasEnabled;

            if (clicked && !disabled && onClick != null)
                onClick();

            return clicked && !disabled;
        }


        private GUILayoutOption[] GetAutoScaledOptions(ButtonSize size, GUILayoutOption[] userOptions)
        {
            var autoOptions = new List<GUILayoutOption>();

           
            switch (size)
            {
                case ButtonSize.Small:
                   
                    autoOptions.Add(GUILayout.Height(Mathf.RoundToInt(32 * guiHelper.uiScale)));
                    break;
                case ButtonSize.Large:
                   
                    autoOptions.Add(GUILayout.Height(Mathf.RoundToInt(40 * guiHelper.uiScale)));
                    break;
                case ButtonSize.Icon:
                   
                    int iconSize = Mathf.RoundToInt(36 * guiHelper.uiScale);
                    autoOptions.Add(GUILayout.Width(iconSize));
                    autoOptions.Add(GUILayout.Height(iconSize));
                    break;
                default:
                   
                    autoOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));
                    break;
            }

           
            if (userOptions != null && userOptions.Length > 0)
            {
                autoOptions.AddRange(userOptions);
            }

            return autoOptions.ToArray();
        }


        public bool Button(Rect rect, string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

           
            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool clicked = GUI.Button(scaledRect, text ?? "Button", buttonStyle);

            GUI.enabled = wasEnabled;

            if (clicked && !disabled && onClick != null)
                onClick();

            return clicked && !disabled;
        }

        public bool DestructiveButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Destructive, size, onClick, false, options);
        }

        public bool OutlineButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Outline, size, onClick, false, options);
        }

        public bool SecondaryButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Secondary, size, onClick, false, options);
        }

        public bool GhostButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Ghost, size, onClick, false, options);
        }

        public bool LinkButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Link, size, onClick, false, options);
        }

        public bool SmallButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Small, onClick, false, options);
        }

        public bool LargeButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Large, onClick, false, options);
        }
        public bool IconButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Icon, onClick, false, options);
        }


        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            float scaledSpacing = spacing * guiHelper.uiScale;

            if (horizontal)
            {
#if IL2CPP
                GUILayout.BeginHorizontal(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.BeginHorizontal();
#endif
            }
            else
            {
#if IL2CPP
                GUILayout.BeginVertical(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.BeginVertical();
#endif
            }

            drawButtons?.Invoke();

            if (horizontal)
                GUILayout.EndHorizontal();
            else
                GUILayout.EndVertical();

            GUILayout.Space(scaledSpacing);
        }


        public bool CreateButton(string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
            params GUILayoutOption[] className)
        {
            return Button(text, variant, size, onClick, disabled, className);
        }


        public void RenderButtonSet(ButtonConfig[] buttons, bool horizontal = true, float spacing = 8f)
        {
            if (buttons == null || buttons.Length == 0) return;

            ButtonGroup(() =>
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    var config = buttons[i];
                    Button(config.Text, config.Variant, config.Size, config.OnClick, config.Disabled, config.Options);

                   
                    if (i < buttons.Length - 1)
                    {
                        if (horizontal)
                            GUILayout.Space(spacing * guiHelper.uiScale);
                        else
                            GUILayout.Space(spacing * guiHelper.uiScale);
                    }
                }
            }, horizontal, 0f);
        }

        public struct ButtonConfig
        {
            public string Text;
            public ButtonVariant Variant;
            public ButtonSize Size;
            public Action OnClick;
            public bool Disabled;
            public GUILayoutOption[] Options;

            public ButtonConfig(string text, ButtonVariant variant = ButtonVariant.Default,
                ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
                params GUILayoutOption[] options)
            {
                Text = text;
                Variant = variant;
                Size = size;
                OnClick = onClick;
                Disabled = disabled;
                Options = options;
            }
        }
    }
}
