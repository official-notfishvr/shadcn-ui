using shadcnui.GUIComponents;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
        internal int cornerRadius = 8;
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
        internal float uiScale = 1f;
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
        #endregion

        #region Public Style Access
        public GUIStyle labelStyle2 => styleManager?.glowLabelStyle ?? GUI.skin.label;
        public GUIStyle buttonStylePublic => styleManager?.animatedButtonStyle ?? GUI.skin.button;
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
                GUILayout.Label(title, GUI.skin.label);
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
                if (width > 0)
                    GUILayout.Label(text, GUILayout.Width(width));
                else
                    GUILayout.Label(text);
            }
        }

        public string RenderGlowInputField(string text, int fieldIndex, string placeholder, int width)
        {
            try
            {
                if (fieldIndex < 0 || fieldIndex >= inputFieldGlow.Length)
                {
                    Debug.LogWarning($"Input field index {fieldIndex} out of bounds, using fallback");
                    return GUILayout.TextField(text ?? placeholder ?? "", GUILayout.Width(width));
                }

                return inputComponents?.RenderGlowInputField(text, fieldIndex, placeholder, width, inputFieldGlow, focusedField, menuAlpha) ??
                       GUILayout.TextField(text ?? placeholder ?? "", GUILayout.Width(width));
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering glow input field: " + ex.Message);
                return GUILayout.TextField(text ?? placeholder ?? "", GUILayout.Width(width));
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
                GUILayout.BeginHorizontal();
                GUILayout.Label(label, GUILayout.Width(120));
                password = GUILayout.PasswordField(password, maskChar, GUILayout.Width(windowWidth - 130));
                GUILayout.EndHorizontal();
                return password;
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
                GUILayout.BeginVertical();
                GUILayout.Label(label);
                text = GUILayout.TextArea(text ?? "", GUILayout.Width(windowWidth), GUILayout.Height(height));
                if (text.Length > maxLength)
                    text = text.Substring(0, maxLength);
                GUILayout.EndVertical();
            }
        }
        #endregion

        #region Button Components - Original Methods
        public bool RenderGlowButton(string text, int buttonIndex)
        {
            try
            {
                if (buttonIndex < 0 || buttonIndex >= buttonGlowEffects.Length)
                {
                    Debug.LogWarning($"Button index {buttonIndex} out of bounds, using fallback");
                    return GUILayout.Button(text ?? "Button");
                }

                return buttonComponents?.RenderGlowButton(text, buttonIndex, hoveredButton, buttonGlowEffects, mousePos, menuAlpha) ??
                       GUILayout.Button(text ?? "Button");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering glow button: " + ex.Message);
                return GUILayout.Button(text ?? "Button");
            }
        }

        public bool RenderColorPresetButton(string colorName, Color presetColor)
        {
            try
            {
                return buttonComponents?.RenderColorPresetButton(colorName, presetColor) ??
                       GUILayout.Button(colorName ?? "Color");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error rendering color preset button: " + ex.Message);
                return GUILayout.Button(colorName ?? "Color");
            }
        }

        public bool DrawButton(float windowWidth, string text, Action onClick)
        {
            try
            {
                return buttonComponents?.DrawButton(windowWidth, text, onClick) ?? false;
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
                return buttonComponents?.DrawColoredButton(windowWidth, text, color, onClick) ?? false;
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
                return buttonComponents?.DrawFixedButton(text, width, height, onClick) ?? false;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing fixed button: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Button Components - New Variant Methods
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
                return GUILayout.Button(text ?? "Button");
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
                return GUI.Button(rect, text ?? "Button");
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
                drawButtons?.Invoke();
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
                return GUILayout.Button(text ?? "Button");
            }
        }
        #endregion

        #region Toggle Components - Original Methods
        public void DrawToggle(float windowWidth, string label, ref bool value, Action<bool> onToggle)
        {
            try
            {
                toggleComponents?.DrawToggle(windowWidth, label, ref value, onToggle);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error drawing toggle: " + ex.Message);
                GUILayout.BeginHorizontal();
                bool newValue = GUILayout.Toggle(value, label ?? "Toggle");
                if (newValue != value)
                {
                    value = newValue;
                    onToggle?.Invoke(value);
                }
                GUILayout.EndHorizontal();
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
                GUILayout.BeginHorizontal();
                value = GUILayout.Toggle(value, label ?? "Checkbox");
                GUILayout.EndHorizontal();
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
                GUILayout.BeginVertical();
                GUILayout.Label(label ?? "Selection");
                if (texts != null && texts.Length > 0)
                {
                    int newSelected = GUILayout.SelectionGrid(selected, texts, xCount);
                    if (newSelected >= 0 && newSelected < texts.Length)
                        return newSelected;
                }
                GUILayout.EndVertical();
                return selected;
            }
        }
        #endregion

        #region Toggle Components - New Variant Methods
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
                return GUILayout.Toggle(value, text ?? "Toggle");
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
                return GUI.Toggle(rect, value, text ?? "Toggle");
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
                GUILayout.BeginVertical(GUI.skin.box);
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
                GUILayout.EndVertical();
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
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(title ?? "Card Title");
                GUILayout.Label(description ?? "");
                GUILayout.Label(content ?? "");
                footerContent?.Invoke();
                GUILayout.EndVertical();
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
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(content ?? "Card Content");
                GUILayout.EndVertical();
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
                GUILayout.BeginHorizontal();
                GUILayout.Label(label ?? "Slider", GUILayout.Width(120));
                value = GUILayout.HorizontalSlider(value, minValue, maxValue, GUILayout.Width(windowWidth - 180));
                GUILayout.Label(value.ToString("F1"), GUILayout.Width(50));
                GUILayout.EndHorizontal();
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
                GUILayout.BeginHorizontal();
                GUILayout.Label(label ?? "Int Slider", GUILayout.Width(120));
                value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue, GUILayout.Width(windowWidth - 180));
                GUILayout.Label(value.ToString(), GUILayout.Width(50));
                GUILayout.EndHorizontal();
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
                GUILayout.BeginHorizontal();
                GUILayout.Label(label ?? "Progress", GUILayout.Width(80));
                Rect rect = GUILayoutUtility.GetRect(windowWidth - 100, 20);
                GUI.Box(rect, "");
                Rect fillRect = new Rect(rect.x, rect.y, rect.width * Mathf.Clamp01(progress), rect.height);
                GUI.color = barColor;
                GUI.Box(fillRect, "");
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
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
                GUILayout.Box(content ?? "Box Content", GUILayout.Width(windowWidth), GUILayout.Height(height));
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
                GUILayout.Box("", GUILayout.Width(windowWidth), GUILayout.Height(height));
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
                GUILayout.Label(text ?? "Instructions", GUI.skin.label);
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
                Vector2 newScrollPos = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));
                try
                {
                    drawContent?.Invoke();
                }
                catch (Exception contentEx)
                {
                    Debug.LogError("Error in scroll view content: " + contentEx.Message);
                }
                GUILayout.EndScrollView();
                return newScrollPos;
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
                GUILayout.BeginHorizontal();
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
                GUILayout.EndHorizontal();
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
                GUILayout.BeginVertical();
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
                GUILayout.EndVertical();
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
                GUILayout.Space(pixels);
            }
        }
        #endregion

        #region Utility Components
        public Color GetCurrentColor()
        {
            try
            {
                return utilityComponents?.GetCurrentColor(r, g, b) ?? Color.white;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error getting current color: " + ex.Message);
                return Color.white;
            }
        }

        public string GetColorHex()
        {
            try
            {
                return utilityComponents?.GetColorHex(r, g, b) ?? "#FFFFFF";
            }
            catch (Exception ex)
            {
                Debug.LogError("Error getting color hex: " + ex.Message);
                return "#FFFFFF";
            }
        }

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

        public void SetRandomColor()
        {
            try
            {
                utilityComponents?.SetRandomColor(ref r, ref g, ref b, ref rSlider, ref gSlider, ref bSlider);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error setting random color: " + ex.Message);
                System.Random rand = new System.Random();
                SetRGBValues(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
            }
        }

        public void SetInputText(string text)
        {
            try
            {
                utilityComponents?.SetInputText(text, ref inputText);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error setting input text: " + ex.Message);
                inputText = text ?? "default";
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

        internal GUIStyleManager GetStyleManager() => styleManager;
        internal float GetMenuAlpha() => menuAlpha;
        internal Vector2 GetMousePos() => mousePos;
        internal float GetParticleTime() => particleTime;

        #endregion

    }

}
