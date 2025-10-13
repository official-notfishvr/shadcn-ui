using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.Tabs;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    /// <summary>
    /// Main GUI Helper class providing a comprehensive set of UI components
    /// </summary>
    public class GUIHelper
    {
        #region Internal Settings
        internal bool animationsEnabled = false;
        internal float animationSpeed = 12f;
        internal bool glowEffectsEnabled = true;
        internal bool particleEffectsEnabled = false;
        internal float backgroundAlpha = 0.9f;
        internal int fontSize = 12;
        internal int cornerRadius = 14;
        internal bool hoverEffectsEnabled = true;
        internal bool fadeTransitionsEnabled = true;
        internal float glowIntensity = 16.5f;
        internal bool smoothAnimationsEnabled = true;
        public float uiScale = 1f;
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
        #region Component Instances - Core System
        private StyleManager styleManager;
        private AnimationManager animationManager;
        #endregion

        #region Component Instances - Layout & Structure
        private shadcnui.GUIComponents.Layout.Layout layoutComponents;
        private Card cardComponents;
        private Separator separatorComponents;
        #endregion

        #region Component Instances - Input & Form
        private shadcnui.GUIComponents.Controls.Input inputComponents;
        private TextArea textAreaComponents;
        private Checkbox checkboxComponents;
        private Switch switchComponents;
        private Slider sliderComponents;
        #endregion

        #region Component Instances - Buttons & Toggles
        private Button buttonComponents;
        private Toggle toggleComponents;
        #endregion

        #region Component Instances - Text & Labels
        private Label labelComponents;
        #endregion

        #region Component Instances - Navigation
        private Tabs tabsComponents;
        private MenuBar menuBarComponents;
        #endregion

        #region Component Instances - Feedback & Status
        private Progress progressComponents;
        private Badge badgeComponents;
        #endregion

        #region Component Instances - Data Display
        private Table tableComponents;
        private shadcnui.GUIComponents.Display.Avatar avatarComponents;
        private Chart chartComponents;
        private DataTable dataTableComponents;
        #endregion

        #region Component Instances - Interactive Controls
        private Calendar calendarComponents;
        private DatePicker datePickerComponents;
        private Select selectComponents;
        #endregion

        #region Component Instances - Overlays & Modals
        private Dialog dialogComponents;
        private DropdownMenu dropdownMenuComponents;
        private Popover popoverComponents;
        #endregion
        #endregion

        #region Public Style Access
        /// <summary>
        /// Get the style manager for advanced styling operations
        /// </summary>
        public StyleManager GetStyleManager() => styleManager;

        public Calendar GetCalendarComponents() => calendarComponents;

        public Chart GetChartComponents() => chartComponents;
        #endregion

        #region Static Compatibility
        public static GUIStyle buttonStyle;
        public static GUIStyle labelStyle;
        public static GUIStyle textFieldStyle;
        #endregion

        #region Initialization
        private bool initialized = false;

        /// <summary>
        /// Initialize a new instance of GUIHelper with all components
        /// </summary>
        public GUIHelper()
        {
            InitializeComponents();
        }

        /// <summary>
        /// Initialize all GUI component instances with proper error handling
        /// </summary>
        private void InitializeComponents()
        {
            try
            {
                GUILogger.LogInfo("Initializing GUIHelper components", "GUIHelper.InitializeComponents");

                // Core System
                styleManager = new StyleManager(this);
                animationManager = new AnimationManager(this);

                // Input & Form
                inputComponents = new shadcnui.GUIComponents.Controls.Input(this);
                textAreaComponents = new TextArea(this);
                checkboxComponents = new Checkbox(this);
                switchComponents = new Switch(this);
                sliderComponents = new Slider(this);

                // Buttons & Toggles
                buttonComponents = new Button(this);
                toggleComponents = new Toggle(this);

                // Layout & Structure
                layoutComponents = new shadcnui.GUIComponents.Layout.Layout(this);
                cardComponents = new Card(this);
                separatorComponents = new Separator(this);

                // Text & Labels
                labelComponents = new Label(this);

                // Navigation
                tabsComponents = new Tabs(this);
                menuBarComponents = new MenuBar(this);

                // Feedback & Status
                progressComponents = new Progress(this);
                badgeComponents = new Badge(this);

                // Data Display
                tableComponents = new Table(this);
                avatarComponents = new shadcnui.GUIComponents.Display.Avatar(this);
                chartComponents = new Chart(this);
                dataTableComponents = new DataTable(this);

                // Interactive Controls
                calendarComponents = new Calendar(this);
                selectComponents = new Select(this);
                datePickerComponents = new DatePicker(this);

                // Overlays & Modals
                dialogComponents = new Dialog(this);
                dropdownMenuComponents = new DropdownMenu(this);
                popoverComponents = new Popover(this);

                GUILogger.LogInfo("GUIHelper components initialized successfully", "GUIHelper.InitializeComponents");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "InitializeComponents", "GUIHelper");
                styleManager = null;
                animationManager = null;
            }
        }
        #endregion

        #region Core System Methods
        /// <summary>
        /// Create a new Settings instance
        /// </summary>
        /// <returns>New Settings instance</returns>
        public Settings CreateSetting()
        {
            try
            {
                return new Settings(this);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "CreateSetting", "GUIHelper");
                return null;
            }
        }

        /// <summary>
        /// Update animation states for GUI components
        /// </summary>
        /// <param name="isOpen">Whether the GUI is currently open</param>
        public void UpdateAnimations(bool isOpen)
        {
            try
            {
                if (!animationsEnabled)
                {
                    menuAlpha = 1f;
                    menuScale = uiScale;
                    titleGlow = 1f;
                    backgroundPulse = backgroundAlpha;
                    hoveredButton = -1;
                    focusedField = -1;
                    particleTime = 0f;
                    return;
                }
                animationManager?.UpdateAnimations(isOpen, ref menuAlpha, ref menuScale, ref titleGlow, ref backgroundPulse, ref hoveredButton, buttonGlowEffects, inputFieldGlow, ref focusedField, ref particleTime, ref mousePos);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "UpdateAnimations", "GUIHelper");
            }
        }

        /// <summary>
        /// Begin animated GUI rendering with proper initialization and background setup
        /// </summary>
        /// <returns>True if GUI should continue rendering</returns>
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

                float currentMenuAlpha = menuAlpha;
                float currentBackgroundPulse = backgroundPulse;
                Vector2 currentMousePos = mousePos;

                float screenWidth = Screen.width * 2f;
                float screenHeight = Screen.height * 2f;
                float offsetX = (screenWidth - Screen.width) / 2f;
                float offsetY = (screenHeight - Screen.height) / 2f;

                Rect scaledBackgroundRect = new Rect(-offsetX, -offsetY, screenWidth, screenHeight);

                float targetPulse = animationsEnabled ? currentBackgroundPulse : 1f;
                float smoothPulse = Mathf.Lerp(targetPulse, currentBackgroundPulse, Time.deltaTime * animationSpeed);
                float bgAlpha = Mathf.Clamp(currentMenuAlpha * backgroundAlpha * smoothPulse, 0.05f, 1f);

                Color baseCol = ThemeManager.Instance.CurrentTheme.PrimaryColor;
                Color backgroundColor = new Color(baseCol.r, baseCol.g, baseCol.b, bgAlpha);

                GUI.color = backgroundColor;
                GUI.DrawTexture(scaledBackgroundRect, Texture2D.whiteTexture);
                GUI.color = Color.white;

                return animationManager?.BeginAnimatedGUI(currentMenuAlpha, currentBackgroundPulse, currentMousePos) ?? true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginAnimatedGUI", "GUIHelper");
                return true;
            }
        }

        /// <summary>
        /// End animated GUI rendering and cleanup
        /// </summary>
        public void EndAnimatedGUI()
        {
            try
            {
                animationManager?.EndAnimatedGUI();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "EndAnimatedGUI", "GUIHelper");
            }
        }

        /// <summary>
        /// Cleanup all GUI components and resources
        /// </summary>
        public void Cleanup()
        {
            try
            {
                GUILogger.LogInfo("Starting GUIHelper cleanup", "GUIHelper.Cleanup");
                styleManager?.Cleanup();
                animationManager?.Cleanup();
                GUILogger.LogInfo("GUIHelper cleanup completed", "GUIHelper.Cleanup");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Cleanup", "GUIHelper");
            }
        }

        internal float GetMenuAlpha() => menuAlpha;

        internal Vector2 GetMousePos() => mousePos;

        internal float GetParticleTime() => particleTime;
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
                GUILogger.LogException(ex, "DrawSectionHeader", "GUIHelper");
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
                GUILogger.LogException(ex, "RenderLabel", "GUIHelper");
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
                GUILogger.LogException(ex, "DrawPasswordField", "GUIHelper");
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
                GUILogger.LogException(ex, "DrawTextArea", "GUIHelper");
            }
        }
        #endregion

        #region Button Components
        public bool DrawButton(float windowWidth, string text, Action onClick, float opacity = 1f)
        {
            try
            {
                return buttonComponents?.DrawButton(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(windowWidth) }) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawButton", "GUIHelper");
                return false;
            }
        }

        public bool DrawFixedButton(string text, float width, float height, Action onClick = null, float opacity = 1f)
        {
            try
            {
                return buttonComponents?.DrawButton(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) }) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawFixedButton", "GUIHelper");
                return false;
            }
        }

        public bool Button(string text, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            try
            {
                return buttonComponents?.DrawButton(text, variant, size, onClick, disabled, opacity, options ?? Array.Empty<GUILayoutOption>()) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Button", "GUIHelper");
                return false;
            }
        }

        public bool DestructiveButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Destructive, size, onClick, false, opacity, options);
        }

        public bool OutlineButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Outline, size, onClick, false, opacity, options);
        }

        public bool SecondaryButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Secondary, size, onClick, false, opacity, options);
        }

        public bool GhostButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Ghost, size, onClick, false, opacity, options);
        }

        public bool LinkButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, ButtonVariant.Link, size, onClick, false, opacity, options);
        }

        public bool SmallButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Small, onClick, false, opacity, options);
        }

        public bool LargeButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Large, onClick, false, opacity, options);
        }

        public bool IconButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Button(text, variant, ButtonSize.Icon, onClick, false, opacity, options);
        }

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            try
            {
                buttonComponents?.ButtonGroup(drawButtons, horizontal, spacing);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ButtonGroup", "GUIHelper");
            }
        }

        public bool DrawButtonVariant(string text, ButtonVariant variant, ButtonSize size)
        {
            try
            {
                GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

                GUILayoutOption[] options;
                switch (size)
                {
                    case ButtonSize.Small:
#if IL2CPP_MELONLOADER
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Height(24 * uiScale) };
#else
                        options = new GUILayoutOption[] { GUILayout.Height(24 * uiScale) };
#endif
                        break;
                    case ButtonSize.Large:
#if IL2CPP_MELONLOADER
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Height(40 * uiScale) };
#else
                        options = new GUILayoutOption[] { GUILayout.Height(40 * uiScale) };
#endif
                        break;
                    case ButtonSize.Icon:
#if IL2CPP_MELONLOADER
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(36 * uiScale), GUILayout.Height(36 * uiScale) };
#else
                        options = new GUILayoutOption[] { GUILayout.Width(36 * uiScale), GUILayout.Height(36 * uiScale) };
#endif
                        break;
                    default:
#if IL2CPP_MELONLOADER
                        options = (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Height(30 * uiScale) };
#else
                        options = new GUILayoutOption[] { GUILayout.Height(30 * uiScale) };
#endif
                        break;
                }

                return GUILayout.Button(text ?? "Button", buttonStyle ?? GUI.skin.button, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing button variant", "GUIHelper");
                return false;
            }
        }
        #endregion

        #region Toggle Components
        public bool Toggle(string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            try
            {
                bool newValue = toggleComponents?.DrawToggle(text, value, variant, size, onToggle, disabled, options) ?? value;
                if (newValue != value)
                {
                    onToggle?.Invoke(newValue);
                }
                return newValue;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing toggle variant", "GUIHelper");
                return value;
            }
        }

        public bool Toggle(Rect rect, string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return toggleComponents?.DrawToggle(rect, text, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing toggle variant in rect", "GUIHelper");
                return false;
            }
        }

        public bool OutlineToggle(string text, bool value, Action<bool> onToggle = null, ToggleSize size = ToggleSize.Default, params GUILayoutOption[] options)
        {
            return Toggle(text, value, ToggleVariant.Outline, size, onToggle, false, options);
        }

        public bool SmallToggle(string text, bool value, Action<bool> onToggle = null, ToggleVariant variant = ToggleVariant.Default, params GUILayoutOption[] options)
        {
            return Toggle(text, value, variant, ToggleSize.Small, onToggle, false, options);
        }

        public bool LargeToggle(string text, bool value, Action<bool> onToggle = null, ToggleVariant variant = ToggleVariant.Default, params GUILayoutOption[] options)
        {
            return Toggle(text, value, variant, ToggleSize.Large, onToggle, false, options);
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
                GUILogger.LogException(ex, "beginning card", "GUIHelper");
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
                GUILogger.LogException(ex, "ending card", "GUIHelper");
            }
        }

        public void DrawCard(string title, string description, string content, System.Action footerContent = null, float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.DrawCard(title, description, content, footerContent, width, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing card", "GUIHelper");
            }
        }

        public void DrawCardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.DrawCardWithImage(image, title, description, content, footerContent, width, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing card with image", "GUIHelper");
            }
        }

        public void DrawCardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.DrawCardWithAvatar(avatar, title, subtitle, content, footerContent, width, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing card with avatar", "GUIHelper");
            }
        }

        public void DrawCardWithHeader(string title, string description, Action header, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            try
            {
                cardComponents?.DrawCardWithHeader(title, description, header, content, footerContent, width, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing card with header ", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing simple card", "GUIHelper");
            }
        }

        public void CardHeader(Action content) => cardComponents?.CardHeader(content);

        public void CardTitle(string title) => cardComponents?.CardTitle(title);

        public void CardDescription(string description) => cardComponents?.CardDescription(description);

        public void CardContent(Action content) => cardComponents?.CardContent(content);

        public void CardFooter(Action content) => cardComponents?.CardFooter(content);
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
                GUILogger.LogException(ex, "drawing slider", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing int slider", "GUIHelper");
            }
        }
        #endregion

        #region Layout Components
        public Vector2 DrawScrollView(Vector2 scrollPosition, System.Action drawContent, params GUILayoutOption[] options)
        {
            try
            {
                return layoutComponents?.DrawScrollView(scrollPosition, drawContent, options) ?? scrollPosition;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing scroll view", "GUIHelper");
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
                GUILogger.LogException(ex, "beginning horizontal group", "GUIHelper");
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
                GUILogger.LogException(ex, "ending horizontal group", "GUIHelper");
            }
        }

        public void BeginVerticalGroup(params GUILayoutOption[] options)
        {
            try
            {
                layoutComponents?.BeginVerticalGroup(options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "beginning vertical group", "GUIHelper");
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
                GUILogger.LogException(ex, "ending vertical group", "GUIHelper");
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
                GUILogger.LogException(ex, "adding space", "GUIHelper");
            }
        }
        #endregion

        #region Label Components
        public void Label(string text, LabelVariant variant = LabelVariant.Default, bool disabled = false, params GUILayoutOption[] options)
        {
            try
            {
                labelComponents?.DrawLabel(text, variant, disabled, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing label", "GUIHelper");
            }
        }

        public void Label(Rect rect, string text, LabelVariant variant = LabelVariant.Default, bool disabled = false)
        {
            try
            {
                labelComponents?.DrawLabel(rect, text, variant, disabled);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing label in rect", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing secondary label", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing muted label", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing destructive label", "GUIHelper");
            }
        }
        #endregion

        #region Progress Components
        public void Progress(float value, float width = -1, float height = -1, params GUILayoutOption[] options)
        {
            try
            {
                progressComponents?.DrawProgress(value, width, height, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing progress", "GUIHelper");
            }
        }

        public void Progress(Rect rect, float value)
        {
            try
            {
                progressComponents?.DrawProgress(rect, value);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing progress in rect", "GUIHelper");
            }
        }

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options)
        {
            try
            {
                progressComponents?.LabeledProgress(label, value, width, height, showPercentage, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing labeled progress", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing circular progress", "GUIHelper");
            }
        }
        #endregion

        #region Avatar Components
        public void Avatar(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.DrawAvatar(image, fallbackText, size, shape, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing avatar", "GUIHelper");
            }
        }

        public void Avatar(Rect rect, Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle)
        {
            try
            {
                avatarComponents?.DrawAvatar(rect, image, fallbackText, size, shape);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing avatar in rect", "GUIHelper");
            }
        }

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing avatar with status", "GUIHelper");
            }
        }

        public void AvatarWithName(Texture2D image, string fallbackText, string name, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing avatar with name", "GUIHelper");
            }
        }

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            try
            {
                avatarComponents?.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing avatar with border", "GUIHelper");
            }
        }

        #endregion

        #region Table Components
        public void Table(string[] headers, string[,] data, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.DrawTable(headers, data, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing table", "GUIHelper");
            }
        }

        public void Table(Rect rect, string[] headers, string[,] data, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default)
        {
            try
            {
                tableComponents?.DrawTable(rect, headers, data, variant, size);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing table in rect", "GUIHelper");
            }
        }

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.SortableTable(headers, data, ref sortColumns, ref sortAscending, variant, size, onSort, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing sortable table", "GUIHelper");
            }
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.SelectableTable(headers, data, ref selectedRows, variant, size, onSelectionChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing selectable table", "GUIHelper");
            }
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.CustomTable(headers, data, cellRenderer, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing custom table", "GUIHelper");
            }
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.PaginatedTable(headers, data, ref currentPage, pageSize, variant, size, onPageChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing paginated table", "GUIHelper");
            }
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery, ref string[,] filteredData, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.SearchableTable(headers, data, ref searchQuery, ref filteredData, variant, size, onSearch, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing searchable table", "GUIHelper");
            }
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                tableComponents?.ResizableTable(headers, data, ref columnWidths, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing resizable table", "GUIHelper");
            }
        }
        #endregion

        #region Calendar Components
        public void Calendar()
        {
            try
            {
                calendarComponents?.DrawCalendar();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing calendar", "GUIHelper");
            }
        }
        #endregion

        #region DropdownMenu Components
        public void DropdownMenu(DropdownMenuConfig config)
        {
            try
            {
                dropdownMenuComponents?.Draw(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing dropdown menu", "GUIHelper");
            }
        }
        #endregion

        #region Popover Components
        public void Popover(Action content)
        {
            try
            {
                popoverComponents?.DrawPopover(content);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing popover", "GUIHelper");
            }
        }

        public void OpenPopover()
        {
            popoverComponents?.Open();
        }

        public void ClosePopover()
        {
            popoverComponents?.Close();
        }

        public bool IsPopoverOpen()
        {
            return popoverComponents?.IsOpen ?? false;
        }
        #endregion

        #region Select Components
        public int Select(string[] items, int selectedIndex)
        {
            try
            {
                return selectComponents?.DrawSelect(items, selectedIndex) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing select", "GUIHelper");
                return selectedIndex;
            }
        }

        public void OpenSelect()
        {
            selectComponents?.Open();
        }

        public void CloseSelect()
        {
            selectComponents?.Close();
        }

        public bool IsSelectOpen()
        {
            return selectComponents?.IsOpen ?? false;
        }
        #endregion

        #region Dialog Components
        public void DrawDialog(string dialogId, Action content, float width = 400, float height = 300)
        {
            dialogComponents?.DrawDialog(dialogId, content, width, height);
        }

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300)
        {
            dialogComponents?.DrawDialog(dialogId, title, description, content, footer, width, height);
        }

        public void OpenDialog(string dialogId)
        {
            dialogComponents?.Open(dialogId);
        }

        public void CloseDialog()
        {
            dialogComponents?.Close();
        }

        public bool IsDialogOpen()
        {
            return dialogComponents?.IsOpen ?? false;
        }

        public bool DrawDialogTrigger(string label, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default)
        {
            try
            {
                return dialogComponents?.DrawDialogTrigger(label, variant, size) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing dialog trigger", "GUIHelper");
                return false;
            }
        }

        public void DrawDialogHeader(string title, string description = null)
        {
            try
            {
                dialogComponents?.DrawDialogHeader(title, description);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing dialog header", "GUIHelper");
            }
        }

        public void DrawDialogContent(Action content)
        {
            try
            {
                dialogComponents?.DrawDialogContent(content);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing dialog content", "GUIHelper");
            }
        }

        public void DrawDialogFooter(Action footer)
        {
            try
            {
                dialogComponents?.DrawDialogFooter(footer);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing dialog footer", "GUIHelper");
            }
        }
        #endregion

        #region DatePicker Components
        public DateTime? DatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            try
            {
                return datePickerComponents?.DrawDatePicker(placeholder, selectedDate, id, options) ?? selectedDate;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing date picker", "GUIHelper");
                return selectedDate;
            }
        }

        public DateTime? DatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            try
            {
                return datePickerComponents?.DrawDatePickerWithLabel(label, placeholder, selectedDate, id, options) ?? selectedDate;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing date picker with label", "GUIHelper");
                return selectedDate;
            }
        }

        public DateTime? DateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options)
        {
            try
            {
                return datePickerComponents?.DrawDateRangePicker(placeholder, startDate, endDate, id, options) ?? startDate;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing date range picker", "GUIHelper");
                return startDate;
            }
        }

        public void CloseDatePicker(string id)
        {
            try
            {
                datePickerComponents?.CloseDatePicker(id);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "closing date picker", "GUIHelper");
            }
        }

        public bool IsDatePickerOpen(string id)
        {
            try
            {
                return datePickerComponents?.IsDatePickerOpen(id) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "checking date picker state", "GUIHelper");
                return false;
            }
        }
        #endregion

        #region DataTable Components
        public void DrawDataTable(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, params GUILayoutOption[] options)
        {
            try
            {
                dataTableComponents?.DrawDataTable(id, columns, data, showPagination, showSearch, showSelection, showColumnToggle, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing data table", "GUIHelper");
            }
        }

        public DataTableColumn CreateDataTableColumn(string id, string header, string accessorKey = null)
        {
            return new DataTableColumn(id, header, accessorKey);
        }

        public DataTableRow CreateDataTableRow(string id)
        {
            return new DataTableRow(id);
        }

        public List<string> GetSelectedRows(string tableId)
        {
            try
            {
                return dataTableComponents?.GetSelectedRows(tableId) ?? new List<string>();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "getting selected rows", "GUIHelper");
                return new List<string>();
            }
        }

        public void ClearSelection(string tableId)
        {
            try
            {
                dataTableComponents?.ClearSelection(tableId);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "clearing selection", "GUIHelper");
            }
        }
        #endregion

        #region Chart Components
        public void Chart(ChartConfig config)
        {
            try
            {
                chartComponents?.DrawChart(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing chart", "GUIHelper");
            }
        }

        public void Chart(List<ChartSeries> chartSeries, ChartType chartType, Vector2 size, params GUILayoutOption[] options)
        {
            try
            {
                var config = new ChartConfig(chartSeries, chartType) { Size = size, Options = options };
                chartComponents?.DrawChart(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing chart", "GUIHelper");
            }
        }
        #endregion

        #region Separator Components
        public void Separator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.DrawSeparator(orientation, decorative, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing separator", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing horizontal separator", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing vertical separator", "GUIHelper");
            }
        }

        public void Separator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal)
        {
            try
            {
                separatorComponents?.DrawSeparator(rect, orientation);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing separator in rect", "GUIHelper");
            }
        }

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, float spacingBefore = 8f, float spacingAfter = 8f, params GUILayoutOption[] options)
        {
            try
            {
                separatorComponents?.SeparatorWithSpacing(orientation, spacingBefore, spacingAfter, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing separator with spacing", "GUIHelper");
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
                GUILogger.LogException(ex, "drawing labeled separator", "GUIHelper");
            }
        }
        #endregion

        #region Tabs Components
        public int DrawTabs(string[] tabNames, int selectedIndex, Action<int> onTabChange, int maxLines = 1, params GUILayoutOption[] options)
        {
            try
            {
                var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                {
                    OnTabChange = onTabChange,
                    MaxLines = maxLines,
                    Options = options,
                };
                var result = tabsComponents?.Draw(config);
                if (result.HasValue)
                {
                    return result.Value;
                }
                return selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing tabs", "GUIHelper");
                return -1;
            }
        }

        public int DrawTabs(string[] tabNames, int selectedIndex, Action content, int maxLines = 1, TabPosition position = TabPosition.Top, params GUILayoutOption[] options)
        {
            try
            {
                var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                {
                    Content = content,
                    MaxLines = maxLines,
                    Position = position,
                    Options = options,
                };
                var result = tabsComponents?.Draw(config);
                if (result.HasValue)
                {
                    return result.Value;
                }
                return selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing tabs with content", "GUIHelper");
                return selectedIndex;
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
                GUILogger.LogException(ex, "beginning tab content", "GUIHelper");
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
                GUILogger.LogException(ex, "ending tab content", "GUIHelper");
            }
        }

        public int TabsWithContent(Tabs.TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null, params GUILayoutOption[] options)
        {
            try
            {
                if (tabConfigs == null || tabConfigs.Length == 0)
                    return selectedIndex;

                var tabNames = new string[tabConfigs.Length];
                for (var i = 0; i < tabConfigs.Length; i++)
                {
                    tabNames[i] = tabConfigs[i].Name;
                }

                var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                {
                    OnTabChange = onTabChange,
                    MaxLines = 1,
                    Position = Tabs.TabPosition.Top,
                    Options = options,
                };

                var newSelectedIndex = tabsComponents?.Draw(config) ?? selectedIndex;

                if (newSelectedIndex >= 0 && newSelectedIndex < tabConfigs.Length)
                {
                    BeginTabContent();
                    tabConfigs[newSelectedIndex].Content?.Invoke();
                    EndTabContent();
                }

                return newSelectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing tabs with content", "GUIHelper");
                return -1;
            }
        }

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action content, float tabWidth = 120f, int maxLines = 1, TabSide side = TabSide.Left, params GUILayoutOption[] options)
        {
            try
            {
                var position = side == Tabs.TabSide.Left ? Tabs.TabPosition.Left : Tabs.TabPosition.Right;

                var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                {
                    Content = content,
                    TabWidth = tabWidth,
                    MaxLines = maxLines,
                    Side = side,
                    Position = position,
                    Options = options,
                };
                return tabsComponents?.Draw(config) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing vertical tabs", "GUIHelper");
                return -1;
            }
        }
        #endregion

        #region TextArea Components
        public string TextArea(string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing text area", "GUIHelper");
                return "Error: " + ex.Message;
            }
        }

        public string TextArea(Rect rect, string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1)
        {
            try
            {
                return textAreaComponents?.DrawTextArea(rect, text, variant, placeholder, disabled, maxLength) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing text area in rect", "GUIHelper");
                return "Error: " + ex.Message;
            }
        }

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing outline text area", "GUIHelper");
                return "Error: " + ex.Message;
            }
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing ghost text area", "GUIHelper");
                return "Error: " + ex.Message;
            }
        }

        public string LabeledTextArea(string label, string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight, maxLength, showCharCount, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing labeled text area", "GUIHelper");
                return "Error: " + ex.Message;
            }
        }

        public string ResizableTextArea(string text, ref float height, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return textAreaComponents?.ResizableTextArea(text, ref height, variant, placeholder, disabled, minHeight, maxHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing resizable text area", "GUIHelper");
                return "Error: " + ex.Message;
            }
        }
        #endregion

        #region Checkbox Components
        public bool Checkbox(string text, bool value, CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            try
            {
                return checkboxComponents?.DrawCheckbox(text, value, variant, size, onToggle, disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing checkbox", "GUIHelper");
                return false;
            }
        }

        public bool Checkbox(Rect rect, string text, bool value, CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return checkboxComponents?.DrawCheckbox(rect, text, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing checkbox in rect", "GUIHelper");
                return false;
            }
        }
        #endregion

        #region Switch Components
        public bool Switch(string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            try
            {
                return switchComponents?.DrawSwitch(text, value, variant, size, onToggle, disabled, options) ?? value;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing switch", "GUIHelper");
                return false;
            }
        }

        public bool Switch(Rect rect, string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            try
            {
                return switchComponents?.DrawSwitch(rect, text, value, variant, size, onToggle, disabled) ?? value;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing switch in rect", "GUIHelper");
                return false;
            }
        }

        #endregion

        #region Badge Components
        public void Badge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.DrawBadge(text, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing badge", "GUIHelper");
            }
        }

        public void Badge(Rect rect, string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default)
        {
            try
            {
                badgeComponents?.DrawBadge(rect, text, variant, size);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing badge in rect", "GUIHelper");
            }
        }

        public void BadgeWithIcon(string text, Texture2D icon, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.BadgeWithIcon(text, icon, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing badge with icon", "GUIHelper");
            }
        }

        public void CountBadge(int count, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, int maxCount = 99, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.CountBadge(count, variant, size, maxCount, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing count badge", "GUIHelper");
            }
        }

        public void StatusBadge(string text, bool isActive, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.StatusBadge(text, isActive, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing status badge", "GUIHelper");
            }
        }

        public void ProgressBadge(string text, float progress, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.ProgressBadge(text, progress, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing progress badge", "GUIHelper");
            }
        }

        public void AnimatedBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.AnimatedBadge(text, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing animated badge", "GUIHelper");
            }
        }

        public void RoundedBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, float cornerRadius = 12f, params GUILayoutOption[] options)
        {
            try
            {
                badgeComponents?.RoundedBadge(text, variant, size, cornerRadius, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing rounded badge", "GUIHelper");
            }
        }
        #endregion

        #region MenuBar Components
        public void MenuBar(MenuBar.MenuBarConfig config)
        {
            try
            {
                menuBarComponents?.Draw(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing menu bar", "GUIHelper");
            }
        }

        public void MenuBar(List<MenuBar.MenuItem> items, params GUILayoutOption[] options)
        {
            MenuBar.MenuBarConfig config = new MenuBar.MenuBarConfig(items) { Options = options };
            try
            {
                menuBarComponents?.Draw(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing menu bar", "GUIHelper");
            }
        }
        #endregion
    }
}
