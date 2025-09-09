using shadcnui.GUIComponents;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static shadcnui.GUIComponents.GUIAvatarComponents;

namespace shadcnui
{
    public class GUIHelper
    {
        #region Public Fields for Compatibility
        public string inputText = "default";
        public string r = "255";
        public string g = "128";
        public string b = "0";
        public float rSlider = 255f;
        public float gSlider = 128f;
        public float bSlider = 0f;
        #endregion

        #region Internal Settings
        internal bool animationsEnabled = true;
        internal float animationSpeed = 12f;
        internal bool glowEffectsEnabled = true;
        internal bool particleEffectsEnabled = true;
        internal bool customColorsEnabled = true;
        internal float backgroundAlpha = 0.9f;
        internal int fontSize = 12;
        internal int cornerRadius =14;
        internal bool shadowEffectsEnabled = false;
        internal bool hoverEffectsEnabled = true;
        internal bool fadeTransitionsEnabled = true;
        internal Color primaryColor = new Color(0.2f, 0.3f, 0.6f);
        internal Color secondaryColor = new Color(0.3f, 0.2f, 0.4f);
        internal Color accentColor = new Color(0.5f, 0.8f, 1f);
        internal float controlSpacing = 10f;
        internal float buttonHeight = 30f;
        internal bool borderEffectsEnabled = false;
        internal float glowIntensity = 16.5f;
        internal bool smoothAnimationsEnabled = true;
        public float uiScale = 1f;
        internal bool debugModeEnabled = false;
        #endregion

        #region Animation State
        private float menuAlpha = 0f;
        private float menuScale = 0.8f;
        private float titleGlow = 0f;
        private int hoveredButton = -1;
        private float[] buttonGlowEffects = new float[20];
        private float backgroundPulse = 0f;
        private Vector2 mousePos;
        private float particleTime = 0f;
        private float[] inputFieldGlow = new float[10];
        private int focusedField = -1;
        #endregion

        #region Component Instances
        private GUIInputComponents inputComponents;
        private GUIButtonComponents buttonComponents;
        private GUISliderComponents sliderComponents;
        private GUIToggleComponents toggleComponents;
        private GUIVisualComponents visualComponents;
        private GUILayoutComponents layoutComponents;
        private GUIUtilityComponents utilityComponents;
        private GUICardComponents cardComponents;
        private GUIStyleManager styleManager;
        private GUIAnimationManager animationManager;
        private GUILabelComponents labelComponents;
        private GUIProgressComponents progressComponents;
        private GUISeparatorComponents separatorComponents;
        private GUITabsComponents tabsComponents;
        private GUITextAreaComponents textAreaComponents;
        private GUICheckboxComponents checkboxComponents;
        private GUISwitchComponents switchComponents;
        private GUIBadgeComponents badgeComponents;
        private GUIAlertComponents alertComponents;
        private GUIAvatarComponents avatarComponents;
        private GUISkeletonComponents skeletonComponents;
        private GUITableComponents tableComponents;
        #endregion

        #region Public Style Access
        public GUIStyle labelStyle2 => styleManager?.glowLabelStyle ?? GUI.skin.label;
        public GUIStyle buttonStylePublic => styleManager?.animatedButtonStyle ?? GUI.skin.button;

        /// <summary>
        /// Get the style manager for advanced styling operations
        /// </summary>
        public GUIStyleManager GetStyleManager() => styleManager;

        #endregion

        #region Static Compatibility
        public static GUIStyle buttonStyle;
        public static GUIStyle labelStyle;
        public static GUIStyle textFieldStyle;
        #endregion

        #region Initialization
        private bool initialized = false;

        public GUIHelper()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            try
            {
                styleManager = new GUIStyleManager(this);
                animationManager = new GUIAnimationManager(this);
                inputComponents = new GUIInputComponents(this);
                buttonComponents = new GUIButtonComponents(this);
                sliderComponents = new GUISliderComponents(this);
                toggleComponents = new GUIToggleComponents(this);
                visualComponents = new GUIVisualComponents(this);
                layoutComponents = new GUILayoutComponents(this);
                utilityComponents = new GUIUtilityComponents(this);
                cardComponents = new GUICardComponents(this);
                labelComponents = new GUILabelComponents(this);
                progressComponents = new GUIProgressComponents(this);
                separatorComponents = new GUISeparatorComponents(this);
                tabsComponents = new GUITabsComponents(this);
                textAreaComponents = new GUITextAreaComponents(this);
                checkboxComponents = new GUICheckboxComponents(this);
                switchComponents = new GUISwitchComponents(this);
                badgeComponents = new GUIBadgeComponents(this);
                alertComponents = new GUIAlertComponents(this);
                avatarComponents = new GUIAvatarComponents(this);
                skeletonComponents = new GUISkeletonComponents(this);
                tableComponents = new GUITableComponents(this);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to initialize GUIHelper components: " + ex.Message);
                styleManager = null;
                animationManager = null;
            }
        }
        #endregion

        #region Core Methods
        public GUISettings CreateSetting()
        {
            return new GUISettings(this);
        }

        public void UpdateAnimations(bool isOpen)
        {
            try
            {
                animationManager?.UpdateAnimations(isOpen, ref menuAlpha, ref menuScale, ref titleGlow,
                    ref backgroundPulse, ref hoveredButton, buttonGlowEffects, inputFieldGlow,
                    ref focusedField, ref particleTime, ref mousePos);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error updating animations: " + ex.Message);
            }
        }

        public bool BeginAnimatedGUI()
        {
            try
            {
                if (!initialized && styleManager != null)
                {
                    styleManager.InitializeGUI();
                    initialized = true;

                    buttonStyle = styleManager.animatedButtonStyle ?? GUI.skin.button;
                    labelStyle = styleManager.glowLabelStyle ?? GUI.skin.label;
                    textFieldStyle = styleManager.animatedInputStyle ?? GUI.skin.textField;
                }

                return animationManager?.BeginAnimatedGUI(menuAlpha, backgroundPulse, mousePos) ?? true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in BeginAnimatedGUI: " + ex.Message);
                return true;
            }
        }

        public void EndAnimatedGUI()
        {
            try
            {
                animationManager?.EndAnimatedGUI();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in EndAnimatedGUI: " + ex.Message);
            }
        }

        public void Cleanup()
        {
            try
            {
                styleManager?.Cleanup();
                animationManager?.Cleanup();
            }
            catch (Exception ex)
            {
                Debug.LogError("Cleanup error: " + ex.Message);
            }
        }
        #endregion

        #region Input Components
        public void DrawSectionHeader(string title)
        {
            try
            {
                inputComponents?.DrawSectionHeader(title);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing section header: " + ex.Message);
            }
        }

        public void RenderLabel(string text, int width = -1)
        {
            try
            {
                inputComponents?.RenderLabel(text, width);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering label: " + ex.Message);
            }
        }

        public string RenderGlowInputField(string text, int fieldIndex, string placeholder, int width)
        {
            try
            {
                if (fieldIndex < 0 || fieldIndex >= inputFieldGlow.Length)
                {
                    Debug.LogWarning($"Input field index {fieldIndex} out of bounds");
                    return "Input field index {fieldIndex} out of bounds";
                }

                return inputComponents?.RenderGlowInputField(text, fieldIndex, placeholder, width, inputFieldGlow, focusedField, menuAlpha) ??
                       GUILayout.TextField(text ?? placeholder ?? "", GUILayout.Width(width));
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering glow input field: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string DrawPasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            try
            {
                return inputComponents?.DrawPasswordField(windowWidth, label, ref password, maskChar) ?? password;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing password field: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public void DrawTextArea(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            try
            {
                inputComponents?.DrawTextArea(windowWidth, label, ref text, maxLength, height);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing text area: " + ex.Message);
            }
        }
        #endregion

        #region Button Components
        public bool RenderGlowButton(string text, int buttonIndex)
        {
            try
            {
                if (buttonIndex < 0 || buttonIndex >= buttonGlowEffects.Length)
                {
                    Debug.LogWarning($"Button index {buttonIndex} out of bounds");
                    return false;
                }

                GUIStyle glowButtonStyle = styleManager?.animatedButtonStyle ?? GUI.skin.button;

                bool clicked = GUILayout.Button(text ?? "Button", glowButtonStyle);

                Rect lastRect = GUILayoutUtility.GetLastRect();
                if (lastRect.Contains(Event.current.mousePosition))
                {
                    hoveredButton = buttonIndex;
                }
                else if (hoveredButton == buttonIndex && !lastRect.Contains(Event.current.mousePosition))
                {
                    hoveredButton = -1;
                }

                return clicked;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering glow button: " + ex.Message);
                return false;
            }
        }

        public bool RenderColorPresetButton(string colorName, Color presetColor)
        {
            try
            {
                var styleManager = GetStyleManager();
                if (styleManager == null)
                {
                    return GUILayout.Button(colorName ?? "Color");
                }

                GUIStyle customButtonStyle = new GUIStyle(GUI.skin.button);
                customButtonStyle.normal.background = styleManager.CreateSolidTexture(presetColor);
                customButtonStyle.hover.background = styleManager.CreateSolidTexture(presetColor * 1.2f);
                customButtonStyle.active.background = styleManager.CreateSolidTexture(presetColor * 0.8f);
                customButtonStyle.normal.textColor = Color.white;
                customButtonStyle.alignment = TextAnchor.MiddleCenter;
                customButtonStyle.fontSize = fontSize;
                customButtonStyle.padding = new RectOffset(10, 10, 5, 5);

                return GUILayout.Button(colorName ?? "Color", customButtonStyle);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering color preset button: " + ex.Message);
                return false;
            }
        }

        public bool DrawButton(float windowWidth, string text, Action onClick)
        {
            try
            {
                return buttonComponents?.Button(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, GUILayout.Width(windowWidth)) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing button: " + ex.Message);
                return false;
            }
        }

        public bool DrawColoredButton(float windowWidth, string text, Color color, Action onClick)
        {
            try
            {
                var styleManager = GetStyleManager();
                if (styleManager == null)
                {
                    return GUILayout.Button(text ?? "Button", GUILayout.Width(windowWidth));
                }

                GUIStyle customButtonStyle = new GUIStyle(GUI.skin.button);
                customButtonStyle.normal.background = styleManager.CreateSolidTexture(color);
                customButtonStyle.hover.background = styleManager.CreateSolidTexture(color * 1.2f);
                customButtonStyle.active.background = styleManager.CreateSolidTexture(color * 0.8f);
                customButtonStyle.normal.textColor = Color.white;
                customButtonStyle.alignment = TextAnchor.MiddleCenter;
                customButtonStyle.fontSize = fontSize;
                customButtonStyle.padding = new RectOffset(10, 10, 5, 5);

                bool clicked = GUILayout.Button(text ?? "Button", customButtonStyle, GUILayout.Width(windowWidth));
                if (clicked)
                {
                    onClick?.Invoke();
                }
                return clicked;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing colored button: " + ex.Message);
                return false;
            }
        }

        public bool DrawFixedButton(string text, float width, float height, Action onClick = null)
        {
            try
            {
                return buttonComponents?.Button(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, GUILayout.Width(width), GUILayout.Height(height)) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing fixed button: " + ex.Message);
                return false;
            }
        }

        public bool Button(string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            try
            {
                return buttonComponents?.Button(text, variant, size, onClick, disabled, options) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing button variant: " + ex.Message);
                return false;
            }
        }

        public bool Button(Rect rect, string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false)
        {
            try
            {
                return buttonComponents?.Button(rect, text, variant, size, onClick, disabled) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing button variant in rect: " + ex.Message);
                return false;
            }
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
            try
            {
                buttonComponents?.ButtonGroup(drawButtons, horizontal, spacing);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing button group: " + ex.Message);
            }
        }

        public bool DrawButtonVariant(string text, ButtonVariant variant, ButtonSize size)
        {
            try
            {
                if (styleManager == null)
                {
                    return GUILayout.Button(text ?? "Button");
                }

                GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

                GUILayoutOption[] options;
                switch (size)
                {
                    case ButtonSize.Small:
#if IL2CPP
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                            GUILayout.Height(24 * uiScale)
                        };
#else
                        options = new GUILayoutOption[] { GUILayout.Height(24 * uiScale) };
#endif
                        break;
                    case ButtonSize.Large:
#if IL2CPP
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                            GUILayout.Height(40 * uiScale)
                        };
#else
                        options = new GUILayoutOption[] { GUILayout.Height(40 * uiScale) };
#endif
                        break;
                    case ButtonSize.Icon:
#if IL2CPP
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                            GUILayout.Width(36 * uiScale), 
                            GUILayout.Height(36 * uiScale) 
                        };
#else
                        options = new GUILayoutOption[] { GUILayout.Width(36 * uiScale), GUILayout.Height(36 * uiScale) };
#endif
                        break;
                    default:
#if IL2CPP
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                            GUILayout.Height(buttonHeight * uiScale) 
                        };
#else
                        options = new GUILayoutOption[] { GUILayout.Height(buttonHeight * uiScale) };
#endif
                        break;
                }

                return GUILayout.Button(text ?? "Button", buttonStyle ?? GUI.skin.button, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing button variant: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Toggle Components
        public void DrawToggle(float windowWidth, string label, ref bool value, Action<bool> onToggle)
        {
            try
            {
                toggleComponents?.DrawToggle(windowWidth, label, ref value, onToggle);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing toggle: " + ex.Message);
            }
        }

        public bool DrawCheckbox(float windowWidth, string label, ref bool value)
        {
            try
            {
                return toggleComponents?.DrawCheckbox(windowWidth, label, ref value) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox: " + ex.Message);
                return false;
            }
        }

        public int DrawSelectionGrid(float windowWidth, string label, int selected, string[] texts, int xCount)
        {
            try
            {
                if (texts == null || texts.Length == 0)
                {
                    Debug.LogWarning("Selection grid texts array is null or empty");
                    return 0;
                }

                if (selected < 0 || selected >= texts.Length)
                {
                    Debug.LogWarning($"Selection index {selected} out of bounds for array of length {texts.Length}");
                    selected = 0;
                }

                return toggleComponents?.DrawSelectionGrid(windowWidth, label, selected, texts, xCount) ?? selected;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing selection grid: " + ex.Message);
                return -1;
            }
        }

        public bool Toggle(string text, bool value, ToggleVariant variant = ToggleVariant.Default,
            ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            try
            {
                return toggleComponents?.Toggle(text, value, variant, size, onToggle, disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing toggle variant: " + ex.Message);
                return false;
            }
        }

        public bool Toggle(Rect rect, string text, bool value, ToggleVariant variant = ToggleVariant.Default,
            ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return toggleComponents?.Toggle(rect, text, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing toggle variant in rect: " + ex.Message);
                return false;
            }
        }

        public bool OutlineToggle(string text, bool value, Action<bool> onToggle = null,
            ToggleSize size = ToggleSize.Default, params GUILayoutOption[] options)
        {
            return Toggle(text, value, ToggleVariant.Outline, size, onToggle, false, options);
        }

        public bool SmallToggle(string text, bool value, Action<bool> onToggle = null,
            ToggleVariant variant = ToggleVariant.Default, params GUILayoutOption[] options)
        {
            return Toggle(text, value, variant, ToggleSize.Small, onToggle, false, options);
        }

        public bool LargeToggle(string text, bool value, Action<bool> onToggle = null,
            ToggleVariant variant = ToggleVariant.Default, params GUILayoutOption[] options)
        {
            return Toggle(text, value, variant, ToggleSize.Large, onToggle, false, options);
        }

        public int ToggleGroup(string[] texts, int selectedIndex, Action<int> onSelectionChange = null,
            ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default,
            bool horizontal = true, float spacing = 5f)
        {
            try
            {
                return toggleComponents?.ToggleGroup(texts, selectedIndex, onSelectionChange, variant, size, horizontal, spacing) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing toggle group: " + ex.Message);
                return selectedIndex;
            }
        }

        public bool[] MultiToggleGroup(string[] texts, bool[] selectedStates, Action<int, bool> onToggleChange = null,
            ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default,
            bool horizontal = true, float spacing = 5f)
        {
            try
            {
                return toggleComponents?.MultiToggleGroup(texts, selectedStates, onToggleChange, variant, size, horizontal, spacing) ?? selectedStates;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing multi toggle group: " + ex.Message);
                return selectedStates;
            }
        }
        #endregion

        #region Card Components
        public void BeginCard(float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.BeginCard(width, height);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error beginning card: " + ex.Message);
            }
        }

        public void EndCard()
        {
            try
            {
                cardComponents?.EndCard();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error ending card: " + ex.Message);
            }
        }

        public void BeginCardHeader() => cardComponents?.BeginCardHeader();
        public void EndCardHeader() => cardComponents?.EndCardHeader();
        public void DrawCardTitle(string title) => cardComponents?.DrawCardTitle(title);
        public void DrawCardDescription(string description) => cardComponents?.DrawCardDescription(description);
        public void BeginCardContent() => cardComponents?.BeginCardContent();
        public void EndCardContent() => cardComponents?.EndCardContent();
        public void BeginCardFooter() => cardComponents?.BeginCardFooter();
        public void EndCardFooter() => cardComponents?.EndCardFooter();

        public void DrawCard(string title, string description, string content, System.Action footerContent = null, float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.DrawCard(title, description, content, footerContent, width, height);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing card: " + ex.Message);
            }
        }

        public void DrawSimpleCard(string content, float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.DrawSimpleCard(content, width, height);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing simple card: " + ex.Message);
            }
        }
        #endregion

        #region Slider Components
        public void DrawSlider(float windowWidth, string label, ref float value, float minValue, float maxValue)
        {
            try
            {
                sliderComponents?.DrawSlider(windowWidth, label, ref value, minValue, maxValue);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing slider: " + ex.Message);
            }
        }

        public void DrawIntSlider(float windowWidth, string label, ref int value, int minValue, int maxValue)
        {
            try
            {
                sliderComponents?.DrawIntSlider(windowWidth, label, ref value, minValue, maxValue);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing int slider: " + ex.Message);
            }
        }
        #endregion

        #region Visual Components
        public void DrawProgressBar(float windowWidth, string label, float progress, Color barColor)
        {
            try
            {
                visualComponents?.DrawProgressBar(windowWidth, label, progress, barColor);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing progress bar: " + ex.Message);
            }
        }

        public void DrawBox(float windowWidth, string content, float height = 30f)
        {
            try
            {
                visualComponents?.DrawBox(windowWidth, content, height);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing box: " + ex.Message);
            }
        }

        public void DrawSeparator(float windowWidth, float height = 2f)
        {
            try
            {
                visualComponents?.DrawSeparator(windowWidth, height);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing separator: " + ex.Message);
            }
        }

        public void RenderInstructions(string text)
        {
            try
            {
                visualComponents?.RenderInstructions(text);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering instructions: " + ex.Message);
            }
        }
        #endregion

        #region Layout Components
        public Vector2 DrawScrollView(Vector2 scrollPosition, float width, float height, System.Action drawContent)
        {
            try
            {
                return layoutComponents?.DrawScrollView(scrollPosition, width, height, drawContent) ?? scrollPosition;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing scroll view: " + ex.Message);
                return new Vector3(0, 0, 0);
            }
        }

        public void BeginHorizontalGroup()
        {
            try
            {
                layoutComponents?.BeginHorizontalGroup();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error beginning horizontal group: " + ex.Message);
            }
        }

        public void EndHorizontalGroup()
        {
            try
            {
                layoutComponents?.EndHorizontalGroup();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error ending horizontal group: " + ex.Message);
            }
        }

        public void BeginVerticalGroup()
        {
            try
            {
                layoutComponents?.BeginVerticalGroup();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error beginning vertical group: " + ex.Message);
            }
        }

        public void EndVerticalGroup()
        {
            try
            {
                layoutComponents?.EndVerticalGroup();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error ending vertical group: " + ex.Message);
            }
        }

        public void AddSpace(float pixels)
        {
            try
            {
                layoutComponents?.AddSpace(pixels);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error adding space: " + ex.Message);
            }
        }
        #endregion

        #region Utility Components
        public void SetRGBValues(int red, int green, int blue)
        {
            try
            {
                utilityComponents?.SetRGBValues(red, green, blue, ref r, ref g, ref b, ref rSlider, ref gSlider, ref bSlider);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error setting RGB values: " + ex.Message);
                r = Mathf.Clamp(red, 0, 255).ToString();
                g = Mathf.Clamp(green, 0, 255).ToString();
                b = Mathf.Clamp(blue, 0, 255).ToString();
            }
        }

        public void ApplyTheme(string themeName)
        {
            try
            {
                utilityComponents?.ApplyTheme(themeName, this);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error applying theme: " + ex.Message);
            }
        }

        public void ResetAllSettings()
        {
            try
            {
                utilityComponents?.ResetAllSettings(this);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error resetting all settings: " + ex.Message);

                animationsEnabled = true;
                glowEffectsEnabled = true;
                customColorsEnabled = true;
            }
        }

        internal float GetMenuAlpha() => menuAlpha;
        internal Vector2 GetMousePos() => mousePos;
        internal float GetParticleTime() => particleTime;
        #endregion

        #region Label Components
        public void Label(string text, LabelVariant variant = LabelVariant.Default, bool disabled = false,
            params GUILayoutOption[] options)
        {
            try
            {
                labelComponents?.Label(text, variant, disabled, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing label: " + ex.Message);
            }
        }

        public void Label(Rect rect, string text, LabelVariant variant = LabelVariant.Default, bool disabled = false)
        {
            try
            {
                labelComponents?.Label(rect, text, variant, disabled);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing label in rect: " + ex.Message);
            }
        }

        public void SecondaryLabel(string text, params GUILayoutOption[] options)
        {
            try
            {
                labelComponents?.SecondaryLabel(text, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing secondary label: " + ex.Message);
            }
        }

        public void MutedLabel(string text, params GUILayoutOption[] options)
        {
            try
            {
                labelComponents?.MutedLabel(text, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing muted label: " + ex.Message);
            }
        }

        public void DestructiveLabel(string text, params GUILayoutOption[] options)
        {
            try
            {
                labelComponents?.DestructiveLabel(text, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing destructive label: " + ex.Message);
            }
        }
        #endregion

        #region Progress Components
        public void Progress(float value, float width = -1, float height = -1, params GUILayoutOption[] options)
        {
            try
            {
                progressComponents?.Progress(value, width, height, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing progress: " + ex.Message);
            }
        }

        public void Progress(Rect rect, float value)
        {
            try
            {
                progressComponents?.Progress(rect, value);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing progress in rect: " + ex.Message);
            }
        }

        public void LabeledProgress(string label, float value, float width = -1, float height = -1,
            bool showPercentage = true, params GUILayoutOption[] options)
        {
            try
            {
                progressComponents?.LabeledProgress(label, value, width, height, showPercentage, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing labeled progress: " + ex.Message);
            }
        }

        public void CircularProgress(float value, float size = 32f, params GUILayoutOption[] options)
        {
            try
            {
                progressComponents?.CircularProgress(value, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing circular progress: " + ex.Message);
            }
        }
        #endregion

        #region Separator Components
        public void Separator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal,
            bool decorative = true, params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.Separator(orientation, decorative, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing separator: " + ex.Message);
            }
        }

        public void HorizontalSeparator(params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.HorizontalSeparator(options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing horizontal separator: " + ex.Message);
            }
        }

        public void VerticalSeparator(params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.VerticalSeparator(options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing vertical separator: " + ex.Message);
            }
        }

        public void Separator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal)
        {
            try
            {
                separatorComponents?.Separator(rect, orientation);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing separator in rect: " + ex.Message);
            }
        }

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal,
            float spacingBefore = 8f, float spacingAfter = 8f, params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.SeparatorWithSpacing(orientation, spacingBefore, spacingAfter, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing separator with spacing: " + ex.Message);
            }
        }

        public void LabeledSeparator(string text, params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.LabeledSeparator(text, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing labeled separator: " + ex.Message);
            }
        }
        #endregion

        #region Tabs Components
        public int Tabs(string[] tabNames, int selectedIndex, Action<int> onTabChange = null,
            params GUILayoutOption[] options)
        {
            try
            {
                return tabsComponents?.Tabs(tabNames, selectedIndex, onTabChange, options) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing tabs: " + ex.Message);
                return -1;
            }
        }

        public void BeginTabContent()
        {
            try
            {
                tabsComponents?.BeginTabContent();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error beginning tab content: " + ex.Message);
            }
        }

        public void EndTabContent()
        {
            try
            {
                tabsComponents?.EndTabContent();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error ending tab content: " + ex.Message);
            }
        }

        public int TabsWithContent(GUITabsComponents.TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null)
        {
            try
            {
                return tabsComponents?.TabsWithContent(tabConfigs, selectedIndex, onTabChange) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing tabs with content: " + ex.Message);
                return -1;
            }
        }

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action<int> onTabChange = null,
            float tabWidth = 120f, params GUILayoutOption[] options)
        {
            try
            {
                return tabsComponents?.VerticalTabs(tabNames, selectedIndex, onTabChange, tabWidth, options) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing vertical tabs: " + ex.Message);
                return -1;
            }
        }
        #endregion

        #region TextArea Components
        public string TextArea(string text, TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1,
            params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.TextArea(text, variant, placeholder, disabled, minHeight, maxLength, options)
                    ?? text;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing text area: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string TextArea(Rect rect, string text, TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "", bool disabled = false, int maxLength = -1)
        {
            try
            {
                return textAreaComponents?.TextArea(rect, text, variant, placeholder, disabled, maxLength) ?? text;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing text area in rect: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false,
            float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options)
                    ?? text;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing outline text area: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false,
            float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options)
                    ?? text;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing ghost text area: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string LabeledTextArea(string label, string text, TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1,
            bool showCharCount = true, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight,
                    maxLength, showCharCount, options) ?? text;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing labeled text area: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string ResizableTextArea(string text, ref float height, TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f,
            int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.ResizableTextArea(text, ref height, variant, placeholder, disabled,
                    minHeight, maxHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing resizable text area: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }
        #endregion

        #region Checkbox Components
        public bool Checkbox(string text, bool value, CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            try
            {
                return checkboxComponents?.Checkbox(text, value, variant, size, onToggle, disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox: " + ex.Message);
                return false;
            }
        }

        public bool Checkbox(Rect rect, string text, bool value, CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.Checkbox(rect, text, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox in rect: " + ex.Message);
                return false;
            }
        }

        public bool CheckboxWithLabel(string label, bool value, CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.CheckboxWithLabel(label, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox with label: " + ex.Message);
                return false;
            }
        }

        public bool[] CheckboxGroup(string[] labels, bool[] values, CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default, Action<int, bool> onToggleChange = null,
            bool disabled = false, bool horizontal = false, float spacing = 5f)
        {
            try
            {
                return checkboxComponents?.CheckboxGroup(labels, values, variant, size, onToggleChange,
                    disabled, horizontal, spacing) ?? values;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox group: " + ex.Message);
                return values ?? new bool[0];
            }
        }

        public bool CustomCheckbox(string text, bool value, Color checkColor, Color backgroundColor,
            Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            try
            {
                return checkboxComponents?.CustomCheckbox(text, value, checkColor, backgroundColor, onToggle,
                    disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom checkbox: " + ex.Message);
                return false;
            }
        }

        public bool CheckboxWithIcon(string text, bool value, Texture2D icon, CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.CheckboxWithIcon(text, value, icon, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox with icon: " + ex.Message);
                return false;
            }
        }

        public bool CheckboxWithDescription(string label, string description, bool value,
            CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.CheckboxWithDescription(label, description, value, variant, size,
                    onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox with description: " + ex.Message);
                return false;
            }
        }

        public bool ValidatedCheckbox(string text, bool value, bool isValid, string validationMessage,
            CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.ValidatedCheckbox(text, value, isValid, validationMessage, variant,
                    size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing validated checkbox: " + ex.Message);
                return false;
            }
        }

        public bool CheckboxWithTooltip(string text, bool value, string tooltip, CheckboxVariant variant = CheckboxVariant.Default,
            CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.CheckboxWithTooltip(text, value, tooltip, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing checkbox with tooltip: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Switch Components
        public bool Switch(string text, bool value, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            try
            {
                return switchComponents?.Switch(text, value, variant, size, onToggle, disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch: " + ex.Message);
                return false;
            }
        }

        public bool Switch(Rect rect, string text, bool value, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.Switch(rect, text, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch in rect: " + ex.Message);
                return false;
            }
        }

        public bool SwitchWithLabel(string label, bool value, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.SwitchWithLabel(label, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch with label: " + ex.Message);
                return false;
            }
        }

        public bool SwitchWithDescription(string label, string description, bool value,
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.SwitchWithDescription(label, description, value, variant, size,
                    onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch with description: " + ex.Message);
                return false;
            }
        }

        public bool CustomSwitch(string text, bool value, Color onColor, Color offColor, Color thumbColor,
            Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            try
            {
                return switchComponents?.CustomSwitch(text, value, onColor, offColor, thumbColor, onToggle,
                    disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom switch: " + ex.Message);
                return false;
            }
        }

        public bool SwitchWithIcon(string text, bool value, Texture2D onIcon, Texture2D offIcon,
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.SwitchWithIcon(text, value, onIcon, offIcon, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch with icon: " + ex.Message);
                return false;
            }
        }

        public bool ValidatedSwitch(string text, bool value, bool isValid, string validationMessage,
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.ValidatedSwitch(text, value, isValid, validationMessage, variant,
                    size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing validated switch: " + ex.Message);
                return false;
            }
        }

        public bool SwitchWithTooltip(string text, bool value, string tooltip,
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.SwitchWithTooltip(text, value, tooltip, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch with tooltip: " + ex.Message);
                return false;
            }
        }

        public bool[] SwitchGroup(string[] labels, bool[] values, SwitchVariant variant = SwitchVariant.Default,
            SwitchSize size = SwitchSize.Default, Action<int, bool> onToggleChange = null,
            bool disabled = false, bool horizontal = false, float spacing = 5f)
        {
            try
            {
                return switchComponents?.SwitchGroup(labels, values, variant, size, onToggleChange,
                    disabled, horizontal, spacing) ?? values;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch group: " + ex.Message);
                return values ?? new bool[0];
            }
        }

        public bool SwitchWithLoading(string text, bool value, bool isLoading,
            SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default,
            Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.SwitchWithLoading(text, value, isLoading, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing switch with loading: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Badge Components
        public void Badge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.Badge(text, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing badge: " + ex.Message);
            }
        }

        public void Badge(Rect rect, string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default)
        {
            try
            {
                badgeComponents?.Badge(rect, text, variant, size);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing badge in rect: " + ex.Message);
            }
        }

        public void BadgeWithIcon(string text, Texture2D icon, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.BadgeWithIcon(text, icon, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing badge with icon: " + ex.Message);
            }
        }

        public void CustomBadge(string text, Color backgroundColor, Color textColor, BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.CustomBadge(text, backgroundColor, textColor, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom badge: " + ex.Message);
            }
        }

        public void CountBadge(int count, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default,
            int maxCount = 99, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.CountBadge(count, variant, size, maxCount, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing count badge: " + ex.Message);
            }
        }

        public void StatusBadge(string text, bool isActive, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.StatusBadge(text, isActive, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing status badge: " + ex.Message);
            }
        }

        public bool DismissibleBadge(string text, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, Action onDismiss = null, params GUILayoutOption[] options)
        {
            try
            {
                return badgeComponents?.DismissibleBadge(text, variant, size, onDismiss, options) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing dismissible badge: " + ex.Message);
                return false;
            }
        }

        public void ProgressBadge(string text, float progress, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.ProgressBadge(text, progress, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing progress badge: " + ex.Message);
            }
        }

        public void AnimatedBadge(string text, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.AnimatedBadge(text, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing animated badge: " + ex.Message);
            }
        }

        public void BadgeWithTooltip(string text, string tooltip, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.BadgeWithTooltip(text, tooltip, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing badge with tooltip: " + ex.Message);
            }
        }

        public void BadgeGroup(string[] texts, BadgeVariant[] variants, BadgeSize size = BadgeSize.Default,
            bool horizontal = true, float spacing = 5f)
        {
            try
            {
                badgeComponents?.BadgeGroup(texts, variants, size, horizontal, spacing);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing badge group: " + ex.Message);
            }
        }

        public void RoundedBadge(string text, BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default, float cornerRadius = 12f, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.RoundedBadge(text, variant, size, cornerRadius, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing rounded badge: " + ex.Message);
            }
        }
        #endregion

        #region Alert Components
        public void Alert(string title, string description, AlertVariant variant = AlertVariant.Default,
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.Alert(title, description, variant, type, null, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert: " + ex.Message);
            }
        }

        

        public void AlertWithIcon(string title, string description, Texture2D icon, AlertVariant variant = AlertVariant.Default,
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.Alert(title, description, variant, type, icon, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert with icon: " + ex.Message);
            }
        }

        public bool DismissibleAlert(string title, string description, AlertVariant variant = AlertVariant.Default,
            AlertType type = AlertType.Info, Action onDismiss = null, params GUILayoutOption[] options)
        {
            try
            {
                return alertComponents?.DismissibleAlert(title, description, variant, type, onDismiss, options) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing dismissible alert: " + ex.Message);
                return false;
            }
        }

        public void AlertWithActions(string title, string description, string[] buttonTexts, Action<int> onButtonClick,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.AlertWithActions(title, description, buttonTexts, onButtonClick, variant, type, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert with actions: " + ex.Message);
            }
        }

        public void CustomAlert(string title, string description, Color backgroundColor, Color textColor, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.CustomAlert(title, description, backgroundColor, textColor, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom alert: " + ex.Message);
            }
        }

        public void AlertWithProgress(string title, string description, float progress, AlertVariant variant = AlertVariant.Default,
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.AlertWithProgress(title, description, progress, variant, type, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert with progress: " + ex.Message);
            }
        }

        public void AnimatedAlert(string title, string description, AlertVariant variant = AlertVariant.Default,
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.AnimatedAlert(title, description, variant, type, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing animated alert: " + ex.Message);
            }
        }

        public void AlertWithCountdown(string title, string description, float countdownTime, Action onTimeout,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.AlertWithCountdown(title, description, countdownTime, onTimeout, variant, type, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert with countdown: " + ex.Message);
            }
        }

        public bool ExpandableAlert(string title, string description, string expandedContent, ref bool isExpanded,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                return alertComponents?.ExpandableAlert(title, description, expandedContent, ref isExpanded, variant, type, options) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing expandable alert: " + ex.Message);
                return false;
            }
        }

        public void AlertWithStatus(string title, string description, bool isActive, AlertVariant variant = AlertVariant.Default,
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.AlertWithStatus(title, description, isActive, variant, type, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert with status: " + ex.Message);
            }
        }

        public void AlertWithCustomIcon(string title, string description, Texture2D icon, Color iconColor,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            try
            {
                alertComponents?.Alert(title, description, variant, type, icon, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing alert with custom icon: " + ex.Message);
            }
        }
        #endregion

        #region Avatar Components
        public void Avatar(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.Avatar(image, fallbackText, size, shape, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar: " + ex.Message);
            }
        }

        public void Avatar(Rect rect, Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle)
        {
            try
            {
                avatarComponents?.Avatar(rect, image, fallbackText, size, shape);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar in rect: " + ex.Message);
            }
        }

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar with status: " + ex.Message);
            }
        }

        public void AvatarWithName(Texture2D image, string fallbackText, string name, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar with name: " + ex.Message);
            }
        }

        public void CustomAvatar(Texture2D image, string fallbackText, Color backgroundColor, Color textColor,
            AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.CustomAvatar(image, fallbackText, backgroundColor, textColor, size, shape, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom avatar: " + ex.Message);
            }
        }

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor,
            AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar with border: " + ex.Message);
            }
        }

        public void AvatarWithHover(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle, Action onClick = null, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithHover(image, fallbackText, size, shape, onClick, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar with hover: " + ex.Message);
            }
        }

        public void AvatarWithLoading(Texture2D image, string fallbackText, bool isLoading, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithLoading(image, fallbackText, isLoading, size, shape, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar with loading: " + ex.Message);
            }
        }

        public void AvatarWithTooltip(Texture2D image, string fallbackText, string tooltip, AvatarSize size = AvatarSize.Default,
            AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithTooltip(image, fallbackText, tooltip, size, shape, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar with tooltip: " + ex.Message);
            }
        }

        public void AvatarGroup(AvatarData[] avatars, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle,
            int maxVisible = 3, float overlap = -8f, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarGroup(avatars, size, shape, maxVisible, overlap, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing avatar group: " + ex.Message);
            }
        }
        #endregion

        #region Skeleton Components
        public void Skeleton(float width, float height, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.Skeleton(width, height, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton: " + ex.Message);
            }
        }

        public void Skeleton(Rect rect, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default)
        {
            try
            {
                skeletonComponents?.Skeleton(rect, variant, size);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton in rect: " + ex.Message);
            }
        }

        public void AnimatedSkeleton(float width, float height, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.AnimatedSkeleton(width, height, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing animated skeleton: " + ex.Message);
            }
        }

        public void ShimmerSkeleton(float width, float height, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.ShimmerSkeleton(width, height, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing shimmer skeleton: " + ex.Message);
            }
        }

        public void CustomSkeleton(float width, float height, Color backgroundColor, Color shimmerColor,
            SkeletonVariant variant = SkeletonVariant.Default, SkeletonSize size = SkeletonSize.Default,
            params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.CustomSkeleton(width, height, backgroundColor, shimmerColor, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom skeleton: " + ex.Message);
            }
        }

        public void SkeletonText(int lineCount, float lineHeight = 20f, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonText(lineCount, lineHeight, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton text: " + ex.Message);
            }
        }

        public void SkeletonAvatar(float size, SkeletonVariant variant = SkeletonVariant.Circular,
            params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonAvatar(size, variant, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton avatar: " + ex.Message);
            }
        }

        public void SkeletonButton(float width = 120f, float height = 36f, SkeletonVariant variant = SkeletonVariant.Rounded,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonButton(width, height, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton button: " + ex.Message);
            }
        }

        public void SkeletonCard(float width = 300f, float height = 200f, SkeletonVariant variant = SkeletonVariant.Rounded,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonCard(width, height, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton card: " + ex.Message);
            }
        }

        public void SkeletonTable(int rowCount, int columnCount, float cellWidth = 100f, float cellHeight = 30f,
            SkeletonVariant variant = SkeletonVariant.Default, SkeletonSize size = SkeletonSize.Default,
            params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonTable(rowCount, columnCount, cellWidth, cellHeight, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton table: " + ex.Message);
            }
        }

        public void SkeletonList(int itemCount, float itemHeight = 60f, SkeletonVariant variant = SkeletonVariant.Rounded,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonList(itemCount, itemHeight, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton list: " + ex.Message);
            }
        }

        public void CustomShapeSkeleton(float width, float height, float cornerRadius, Color backgroundColor,
            params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.CustomShapeSkeleton(width, height, cornerRadius, backgroundColor, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom shape skeleton: " + ex.Message);
            }
        }

        public void SkeletonWithProgress(float width, float height, float progress, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.SkeletonWithProgress(width, height, progress, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing skeleton with progress: " + ex.Message);
            }
        }

        public void FadeSkeleton(float width, float height, float fadeTime, SkeletonVariant variant = SkeletonVariant.Default,
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                skeletonComponents?.FadeSkeleton(width, height, fadeTime, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing fade skeleton: " + ex.Message);
            }
        }
        #endregion

        #region Table Components
        public void Table(string[] headers, string[,] data,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default,
            params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.Table(headers, data, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing table: " + ex.Message);
            }
        }

        public void Table(Rect rect, string[] headers, string[,] data,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default)
        {
            try
            {
                tableComponents?.Table(rect, headers, data, variant, size);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing table in rect: " + ex.Message);
            }
        }

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns,
            ref bool[] sortAscending, TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default, Action<int, bool> onSort = null,
            params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.SortableTable(headers, data, ref sortColumns, ref sortAscending, variant, size, onSort, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing sortable table: " + ex.Message);
            }
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default,
            Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.SelectableTable(headers, data, ref selectedRows, variant, size, onSelectionChange, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing selectable table: " + ex.Message);
            }
        }

        public void CustomTable(string[] headers, object[,] data,
            Action<object, int, int> cellRenderer, TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.CustomTable(headers, data, cellRenderer, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing custom table: " + ex.Message);
            }
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default,
            Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.PaginatedTable(headers, data, ref currentPage, pageSize, variant, size, onPageChange, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing paginated table: " + ex.Message);
            }
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery,
            ref string[,] filteredData, TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default, Action<string> onSearch = null,
            params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.SearchableTable(headers, data, ref searchQuery, ref filteredData, variant, size, onSearch, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing searchable table: " + ex.Message);
            }
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default,
            params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.ResizableTable(headers, data, ref columnWidths, variant, size, options);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing resizable table: " + ex.Message);
            }
        }
        #endregion
    }
}