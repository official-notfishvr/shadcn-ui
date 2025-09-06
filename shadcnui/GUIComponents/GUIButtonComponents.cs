using shadcnui;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Draw a button with specified variant and size using GUIStyleManager
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="variant">Button style variant</param>
        /// <param name="size">Button size</param>
        /// <param name="onClick">Click callback</param>
        /// <param name="disabled">Whether button is disabled</param>
        /// <param name="options">Additional GUILayout options</param>
        /// <returns>True if button was clicked</returns>
        public bool Button(string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool clicked;
#if IL2CPP
            if (options != null && options.Length > 0)
                clicked = GUILayout.Button(text, buttonStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
            else
                clicked = GUILayout.Button(text, buttonStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            clicked = options != null && options.Length > 0 ?
                GUILayout.Button(text, buttonStyle, options) :
                GUILayout.Button(text, buttonStyle);
#endif

            GUI.enabled = wasEnabled;

            if (clicked && !disabled && onClick != null)
                onClick();

            return clicked && !disabled;
        }

        /// <summary>
        /// Draw a button in a specific rect with variant and size
        /// </summary>
        public bool Button(Rect rect, string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool clicked = GUI.Button(rect, text, buttonStyle);

            GUI.enabled = wasEnabled;

            if (clicked && !disabled && onClick != null)
                onClick();

            return clicked && !disabled;
        }

        /// <summary>
        /// Draw a destructive action button (e.g., Delete, Remove)
        /// </summary>
        public bool DestructiveButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Destructive, size, onClick, false, options);
        }

        /// <summary>
        /// Draw an outline button
        /// </summary>
        public bool OutlineButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Outline, size, onClick, false, options);
        }

        /// <summary>
        /// Draw a secondary button
        /// </summary>
        public bool SecondaryButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Secondary, size, onClick, false, options);
        }

        /// <summary>
        /// Draw a ghost button (transparent background)
        /// </summary>
        public bool GhostButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Ghost, size, onClick, false, options);
        }

        /// <summary>
        /// Draw a link-style button
        /// </summary>
        public bool LinkButton(string text, Action onClick, ButtonSize size = ButtonSize.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Link, size, onClick, false, options);
        }

        /// <summary>
        /// Draw a small button
        /// </summary>
        public bool SmallButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Small, onClick, false, options);
        }

        /// <summary>
        /// Draw a large button
        /// </summary>
        public bool LargeButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Large, onClick, false, options);
        }

        /// <summary>
        /// Draw an icon-sized button
        /// </summary>
        public bool IconButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default,
            params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Icon, onClick, false, options);
        }

        /// <summary>
        /// Draw a button group with consistent spacing
        /// </summary>
        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
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

            GUILayout.Space(spacing);
        }

    }
}
