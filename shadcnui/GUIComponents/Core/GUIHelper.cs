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
        private readonly Dictionary<Type, BaseComponent> _componentInstances;

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
        #endregion
        #endregion

        #region Public Style Access
        /// <summary>
        /// Get the style manager for advanced styling operations
        /// </summary>
        public StyleManager GetStyleManager() => styleManager;

        public Calendar GetCalendarComponents() => Get<Calendar>();

        public Chart GetChartComponents() => Get<Chart>();

        public T Get<T>()
            where T : BaseComponent
        {
            if (_componentInstances.TryGetValue(typeof(T), out BaseComponent component))
            {
                return (T)component;
            }
            GUILogger.LogWarning($"Component of type {typeof(T).Name} not found.", "GUIHelper.Get<T>");
            return null;
        }
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
            _componentInstances = new Dictionary<Type, BaseComponent>();
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
                _componentInstances.Add(typeof(shadcnui.GUIComponents.Controls.Input), new shadcnui.GUIComponents.Controls.Input(this));
                _componentInstances.Add(typeof(TextArea), new TextArea(this));
                _componentInstances.Add(typeof(Checkbox), new Checkbox(this));
                _componentInstances.Add(typeof(Switch), new Switch(this));
                _componentInstances.Add(typeof(Slider), new Slider(this));

                // Buttons & Toggles
                _componentInstances.Add(typeof(Button), new Button(this));
                _componentInstances.Add(typeof(Toggle), new Toggle(this));

                // Layout & Structure
                layoutComponents = new shadcnui.GUIComponents.Layout.Layout(this);
                _componentInstances.Add(typeof(Card), new Card(this));
                _componentInstances.Add(typeof(Separator), new Separator(this));

                // Text & Labels
                _componentInstances.Add(typeof(Label), new Label(this));

                // Navigation
                _componentInstances.Add(typeof(Tabs), new Tabs(this));
                _componentInstances.Add(typeof(MenuBar), new MenuBar(this));

                // Feedback & Status
                _componentInstances.Add(typeof(Progress), new Progress(this));
                _componentInstances.Add(typeof(Badge), new Badge(this));

                // Data Display
                _componentInstances.Add(typeof(Table), new Table(this));
                _componentInstances.Add(typeof(shadcnui.GUIComponents.Display.Avatar), new shadcnui.GUIComponents.Display.Avatar(this));
                _componentInstances.Add(typeof(Chart), new Chart(this));
                _componentInstances.Add(typeof(DataTable), new DataTable(this));

                // Interactive Controls
                _componentInstances.Add(typeof(Calendar), new Calendar(this));
                _componentInstances.Add(typeof(Select), new Select(this));
                _componentInstances.Add(typeof(DatePicker), new DatePicker(this));

                // Overlays & Modals
                _componentInstances.Add(typeof(Dialog), new Dialog(this));
                _componentInstances.Add(typeof(DropdownMenu), new DropdownMenu(this));
                _componentInstances.Add(typeof(Popover), new Popover(this));

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
                Get<shadcnui.GUIComponents.Controls.Input>()?.DrawSectionHeader(title);
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
                Get<shadcnui.GUIComponents.Controls.Input>()?.RenderLabel(text, width);
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
                return Get<shadcnui.GUIComponents.Controls.Input>()?.DrawPasswordField(windowWidth, label, ref password, maskChar) ?? password;
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
                Get<shadcnui.GUIComponents.Controls.Input>()?.DrawTextArea(windowWidth, label, ref text, maxLength, height);
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
                return Get<Button>()?.DrawButton(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(windowWidth) }) ?? false;
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
                return Get<Button>()?.DrawButton(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) }) ?? false;
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
                return Get<Button>()?.DrawButton(text, variant, size, onClick, disabled, opacity, options ?? Array.Empty<GUILayoutOption>()) ?? false;
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
                Get<Button>()?.ButtonGroup(drawButtons, horizontal, spacing);
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
                bool newValue = Get<Toggle>()?.DrawToggle(text, value, variant, size, onToggle, disabled, options) ?? value;
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
                return Get<Toggle>()?.DrawToggle(rect, text, value, variant, size, onToggle, disabled) ?? value;
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
                Get<Card>()?.BeginCard(width, height);
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
                Get<Card>()?.EndCard();
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
                Get<Card>()?.DrawCard(title, description, content, footerContent, width, height);
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
                Get<Card>()?.DrawCardWithImage(image, title, description, content, footerContent, width, height);
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
                Get<Card>()?.DrawCardWithAvatar(avatar, title, subtitle, content, footerContent, width, height);
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
                Get<Card>()?.DrawCardWithHeader(title, description, header, content, footerContent, width, height);
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
                Get<Card>()?.DrawSimpleCard(content, width, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing simple card", "GUIHelper");
            }
        }

        public void CardHeader(Action content) => Get<Card>()?.CardHeader(content);

        public void CardTitle(string title) => Get<Card>()?.CardTitle(title);

        public void CardDescription(string description) => Get<Card>()?.CardDescription(description);

        public void CardContent(Action content) => Get<Card>()?.CardContent(content);

        public void CardFooter(Action content) => Get<Card>()?.CardFooter(content);
        #endregion

        #region Slider Components
        public void DrawSlider(float windowWidth, string label, ref float value, float minValue, float maxValue)
        {
            try
            {
                Get<Slider>()?.DrawSlider(windowWidth, label, ref value, minValue, maxValue);
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
                Get<Slider>()?.DrawIntSlider(windowWidth, label, ref value, minValue, maxValue);
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
                Get<Label>()?.DrawLabel(text, variant, disabled, options);
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
                Get<Label>()?.DrawLabel(rect, text, variant, disabled);
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
                Get<Label>()?.SecondaryLabel(text, options);
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
                Get<Label>()?.MutedLabel(text, options);
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
                Get<Label>()?.DestructiveLabel(text, options);
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
                Get<Progress>()?.DrawProgress(value, width, height, options);
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
                Get<Progress>()?.DrawProgress(rect, value);
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
                Get<Progress>()?.LabeledProgress(label, value, width, height, showPercentage, options);
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
                Get<Progress>()?.CircularProgress(value, size, options);
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
                Get<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(image, fallbackText, size, shape, options);
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
                Get<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(rect, image, fallbackText, size, shape);
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
                Get<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options);
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
                Get<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options);
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
                Get<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options);
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
                Get<Table>()?.DrawTable(headers, data, variant, size, options);
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
                Get<Table>()?.DrawTable(rect, headers, data, variant, size);
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
                Get<Table>()?.SortableTable(headers, data, ref sortColumns, ref sortAscending, variant, size, onSort, options);
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
                Get<Table>()?.SelectableTable(headers, data, ref selectedRows, variant, size, onSelectionChange, options);
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
                Get<Table>()?.CustomTable(headers, data, cellRenderer, variant, size, options);
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
                Get<Table>()?.PaginatedTable(headers, data, ref currentPage, pageSize, variant, size, onPageChange, options);
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
                Get<Table>()?.SearchableTable(headers, data, ref searchQuery, ref filteredData, variant, size, onSearch, options);
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
                Get<Table>()?.ResizableTable(headers, data, ref columnWidths, variant, size, options);
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
                Get<Calendar>()?.DrawCalendar();
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
                Get<DropdownMenu>()?.Draw(config);
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
                Get<Popover>()?.DrawPopover(content);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing popover", "GUIHelper");
            }
        }

        public void OpenPopover()
        {
            Get<Popover>()?.Open();
        }

        public void ClosePopover()
        {
            Get<Popover>()?.Close();
        }

        public bool IsPopoverOpen()
        {
            return Get<Popover>()?.IsOpen ?? false;
        }
        #endregion

        #region Select Components
        public int Select(string[] items, int selectedIndex)
        {
            try
            {
                return Get<Select>()?.DrawSelect(items, selectedIndex) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing select", "GUIHelper");
                return selectedIndex;
            }
        }

        public void OpenSelect()
        {
            Get<Select>()?.Open();
        }

        public void CloseSelect()
        {
            Get<Select>()?.Close();
        }

        public bool IsSelectOpen()
        {
            return Get<Select>()?.IsOpen ?? false;
        }
        #endregion

        #region Dialog Components
        public void DrawDialog(string dialogId, Action content, float width = 400, float height = 300)
        {
            Get<Dialog>()?.DrawDialog(dialogId, content, width, height);
        }

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300)
        {
            Get<Dialog>()?.DrawDialog(dialogId, title, description, content, footer, width, height);
        }

        public void OpenDialog(string dialogId)
        {
            Get<Dialog>()?.Open(dialogId);
        }

        public void CloseDialog()
        {
            Get<Dialog>()?.Close();
        }

        public bool IsDialogOpen()
        {
            return Get<Dialog>()?.IsOpen ?? false;
        }

        public bool DrawDialogTrigger(string label, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default)
        {
            try
            {
                return Get<Dialog>()?.DrawDialogTrigger(label, variant, size) ?? false;
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
                Get<Dialog>()?.DrawDialogHeader(title, description);
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
                Get<Dialog>()?.DrawDialogContent(content);
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
                Get<Dialog>()?.DrawDialogFooter(footer);
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
                return Get<DatePicker>()?.DrawDatePicker(placeholder, selectedDate, id, options) ?? selectedDate;
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
                return Get<DatePicker>()?.DrawDatePickerWithLabel(label, placeholder, selectedDate, id, options) ?? selectedDate;
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
                return Get<DatePicker>()?.DrawDateRangePicker(placeholder, startDate, endDate, id, options) ?? startDate;
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
                Get<DatePicker>()?.CloseDatePicker(id);
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
                return Get<DatePicker>()?.IsDatePickerOpen(id) ?? false;
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
                Get<DataTable>()?.DrawDataTable(id, columns, data, showPagination, showSearch, showSelection, showColumnToggle, options);
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
                return Get<DataTable>()?.GetSelectedRows(tableId) ?? new List<string>();
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
                Get<DataTable>()?.ClearSelection(tableId);
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
                Get<Chart>()?.DrawChart(config);
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
                Get<Chart>()?.DrawChart(config);
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
                Get<Separator>()?.DrawSeparator(orientation, decorative, options);
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
                Get<Separator>()?.HorizontalSeparator(options);
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
                Get<Separator>()?.VerticalSeparator(options);
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
                Get<Separator>()?.DrawSeparator(rect, orientation);
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
                Get<Separator>()?.SeparatorWithSpacing(orientation, spacingBefore, spacingAfter, options);
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
                Get<Separator>()?.LabeledSeparator(text, options);
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
                var result = Get<Tabs>()?.Draw(config);
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
                var result = Get<Tabs>()?.Draw(config);
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
                Get<Tabs>()?.BeginTabContent();
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
                Get<Tabs>()?.EndTabContent();
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

                var newSelectedIndex = Get<Tabs>()?.Draw(config) ?? selectedIndex;

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
                return Get<Tabs>()?.Draw(config) ?? selectedIndex;
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
                return Get<TextArea>()?.DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options) ?? text;
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
                return Get<TextArea>()?.DrawTextArea(rect, text, variant, placeholder, disabled, maxLength) ?? text;
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
                return Get<TextArea>()?.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text;
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
                return Get<TextArea>()?.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text;
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
                return Get<TextArea>()?.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight, maxLength, showCharCount, options) ?? text;
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
                return Get<TextArea>()?.ResizableTextArea(text, ref height, variant, placeholder, disabled, minHeight, maxHeight, maxLength, options) ?? text;
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
                return Get<Checkbox>()?.DrawCheckbox(text, value, variant, size, onToggle, disabled, options) ?? value;
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
                return Get<Checkbox>()?.DrawCheckbox(rect, text, value, variant, size, onToggle, disabled) ?? value;
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
                return Get<Switch>()?.DrawSwitch(text, value, variant, size, onToggle, disabled, options) ?? value;
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
                return Get<Switch>()?.DrawSwitch(rect, text, value, variant, size, onToggle, disabled) ?? value;
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
                Get<Badge>()?.DrawBadge(text, variant, size, options);
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
                Get<Badge>()?.DrawBadge(rect, text, variant, size);
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
                Get<Badge>()?.BadgeWithIcon(text, icon, variant, size, options);
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
                Get<Badge>()?.CountBadge(count, variant, size, maxCount, options);
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
                Get<Badge>()?.StatusBadge(text, isActive, variant, size, options);
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
                Get<Badge>()?.ProgressBadge(text, progress, variant, size, options);
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
                Get<Badge>()?.AnimatedBadge(text, variant, size, options);
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
                Get<Badge>()?.RoundedBadge(text, variant, size, cornerRadius, options);
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
                Get<MenuBar>()?.Draw(config);
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
                Get<MenuBar>()?.Draw(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "drawing menu bar", "GUIHelper");
            }
        }
        #endregion
    }
}
