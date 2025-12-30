using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.Tabs;
using Input = shadcnui.GUIComponents.Controls.Input;
using Toast = shadcnui.GUIComponents.Display.Toast;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core.Base
{
    public class GUIHelper
    {
        #region Fields

        private readonly Dictionary<Type, BaseComponent> _componentInstances;
        private readonly IComponentResolver _componentResolver;

        #endregion

        #region Internal Settings

        internal float backgroundAlpha = 0.85f;
        internal int fontSize = 14;
        internal int cornerRadius = 16;
        internal bool fadeTransitionsEnabled = true;
        internal float glowIntensity = 12.0f;
        public float uiScale = 1f;

        #endregion

        #region Animation State

        private float menuAlpha = 0f;
        private float menuScale = 0.8f;
        private float backgroundPulse = 0f;
        private Vector2 mousePos;
        private float particleTime = 0f;

        #endregion

        #region Component Instances

        private StyleManager styleManager;
        private AnimationManager animationManager;
        private shadcnui.GUIComponents.Layout.Layout layoutComponents;
        private bool initialized = false;

        #endregion

        #region Constructor

        public GUIHelper()
        {
            _componentInstances = new Dictionary<Type, BaseComponent>();
            _componentResolver = new ComponentResolver(_componentInstances);
            InitializeComponents();
        }

        #endregion

        #region Initialization

        private void InitializeComponents()
        {
            try
            {
                GUILogger.LogInfo("Initializing GUIHelper components", "GUIHelper.InitializeComponents");

                styleManager = new StyleManager(this);
                animationManager = new AnimationManager(this);

                // Controls
                _componentInstances[typeof(Input)] = new Input(this);
                _componentInstances[typeof(TextArea)] = new TextArea(this);
                _componentInstances[typeof(Checkbox)] = new Checkbox(this);
                _componentInstances[typeof(Switch)] = new Switch(this);
                _componentInstances[typeof(Button)] = new Button(this);
                _componentInstances[typeof(Toggle)] = new Toggle(this);
                _componentInstances[typeof(Select)] = new Select(this);
                _componentInstances[typeof(DropdownMenu)] = new DropdownMenu(this);
                _componentInstances[typeof(Slider)] = new Slider(this);

                // Layout
                layoutComponents = new shadcnui.GUIComponents.Layout.Layout(this);
                _componentInstances[typeof(Card)] = new Card(this);
                _componentInstances[typeof(Separator)] = new Separator(this);
                _componentInstances[typeof(Tabs)] = new Tabs(this);
                _componentInstances[typeof(MenuBar)] = new MenuBar(this);
                _componentInstances[typeof(Table)] = new Table(this);

                // Display
                _componentInstances[typeof(Label)] = new Label(this);
                _componentInstances[typeof(Progress)] = new Progress(this);
                _componentInstances[typeof(Badge)] = new Badge(this);
                _componentInstances[typeof(shadcnui.GUIComponents.Display.Avatar)] = new shadcnui.GUIComponents.Display.Avatar(this);
                _componentInstances[typeof(Chart)] = new Chart(this);
                _componentInstances[typeof(Dialog)] = new Dialog(this);
                _componentInstances[typeof(Popover)] = new Popover(this);
                _componentInstances[typeof(Toast)] = new Toast(this);

                // Data
                _componentInstances[typeof(DataTable)] = new DataTable(this);
                _componentInstances[typeof(Calendar)] = new Calendar(this);
                _componentInstances[typeof(DatePicker)] = new DatePicker(this);

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

        #region Component Resolution

        private T GetComponent<T>()
            where T : BaseComponent
        {
            try
            {
                return _componentResolver.GetComponent<T>();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, $"GetComponent<{typeof(T).Name}>", "GUIHelper");
                return null;
            }
        }

        public StyleManager GetStyleManager() => styleManager;

        public AnimationManager GetAnimationManager() => animationManager;

        public Calendar GetCalendarComponents() => GetComponent<Calendar>();

        public Chart GetChartComponents() => GetComponent<Chart>();

        #endregion

        #region Animation Management
        public void UpdateGUI(bool isOpen)
        {
            // not used yet, but reserved for future use
        }

        [Obsolete("UpdateAnimations is deprecated. Use UpdateGUI(bool) instead.", error: false)]
        public void UpdateAnimations(bool isOpen)
        {
            UpdateGUI(isOpen);
        }

        public bool BeginGUI()
        {
            try
            {
                if (!initialized && styleManager != null)
                {
                    styleManager.InitializeGUI();
                    initialized = true;
                }
                if (styleManager.ScanForCorruption())
                    styleManager.MarkStylesCorruption();
                styleManager.RefreshStylesIfCorruption();

                return animationManager?.BeginGUI() ?? true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginAnimatedGUI", "GUIHelper");
                return true;
            }
        }

        [Obsolete("BeginAnimatedGUI is deprecated. Use BeginGUI() instead.", error: false)]
        public bool BeginAnimatedGUI()
        {
            return BeginGUI();
        }

        public void EndGUI()
        {
            try
            {
                animationManager?.EndGUI();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "EndAnimatedGUI", "GUIHelper");
            }
        }

        [Obsolete("EndAnimatedGUI is deprecated. Use EndGUI() instead.", error: false)]
        public void EndAnimatedGUI()
        {
            EndGUI();
        }

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

        #region Config-based API

        public string DrawInput(InputConfig config) => TryExecute(() => GetComponent<Input>()?.DrawInput(config) ?? config.Value, "DrawInput");

        public string DrawLabeledInput(InputConfig config) => TryExecute(() => GetComponent<Input>()?.DrawLabeledInput(config) ?? config.Value, "DrawLabeledInput");

        #endregion

        #region API

        public void DrawSectionHeader(string title) => TryExecute(() => GetComponent<Input>()?.DrawSectionHeader(title), "DrawSectionHeader");

        public void RenderLabel(string text, int width = -1) => TryExecute(() => GetComponent<Input>()?.RenderLabel(text, width), "RenderLabel");

        public string DrawPasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            try
            {
                return GetComponent<Input>()?.DrawPasswordField(windowWidth, label, ref password, maskChar) ?? password;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawPasswordField", "GUIHelper");
                return password;
            }
        }

        public void DrawPasswordField(float windowWidth, string label, ref string password)
        {
            DrawPasswordField(windowWidth, label, ref password, '*');
        }

        public void DrawTextArea(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            try
            {
                GetComponent<Input>()?.DrawTextArea(windowWidth, label, ref text, maxLength, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawTextArea", "GUIHelper");
            }
        }

        public string DrawInput(string value, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            TryExecute(() => GetComponent<Input>()?.DrawInput(value, placeholder, variant, disabled, focused, width, onChange) ?? value, "DrawInput");

        public string DrawInput(string value, Texture2D icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            TryExecute(() => GetComponent<Input>()?.DrawInput(value, icon, placeholder, variant, disabled, focused, width, onChange) ?? value, "DrawInput");

        public string DrawInput(string value, IconConfig icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            TryExecute(
                () =>
                    GetComponent<Input>()
                        ?.DrawInput(
                            new InputConfig
                            {
                                Value = value,
                                Icon = icon,
                                Placeholder = placeholder,
                                Variant = variant,
                                Disabled = disabled,
                                Focused = focused,
                                Width = width,
                                OnChange = onChange,
                            }
                        ) ?? value,
                "DrawInput"
            );

        public string DrawLabeledInput(string label, string value, string placeholder = "", ControlVariant inputVariant = ControlVariant.Default, ControlVariant labelVariant = ControlVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null) =>
            TryExecute(() => GetComponent<Input>()?.DrawLabeledInput(label, value, placeholder, inputVariant, labelVariant, disabled, inputWidth, onChange) ?? value, "DrawLabeledInput");

        #endregion

        #endregion

        #region Button Components

        #region Config-based API

        public bool Button(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, variant, size, onClick, disabled, opacity, options ?? Array.Empty<GUILayoutOption>()) ?? false, "Button");

        #endregion

        #region API

        public bool DrawButtonVariant(string text, ControlVariant variant, ControlSize size) =>
            TryExecute(
                () =>
                {
                    GUIStyle btnStyle = styleManager.GetButtonStyle(variant, size);
                    GUILayoutOption[] options = GetButtonSizeOptions(size);
                    return GUILayout.Button(text ?? "Button", btnStyle ?? GUI.skin.button, options);
                },
                "DrawButtonVariant"
            );

        public bool DrawButton(float windowWidth, string text, Action onClick, float opacity = 1f) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, ControlVariant.Default, ControlSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(windowWidth) }) ?? false, "DrawButton");

        public bool DrawFixedButton(string text, float width, float height, Action onClick = null, float opacity = 1f) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, ControlVariant.Default, ControlSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) }) ?? false, "DrawFixedButton");

        public bool DestructiveButton(string text, Action onClick, ControlSize size = ControlSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ControlVariant.Destructive, size, onClick, false, opacity, options);

        public bool OutlineButton(string text, Action onClick, ControlSize size = ControlSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ControlVariant.Outline, size, onClick, false, opacity, options);

        public bool SecondaryButton(string text, Action onClick, ControlSize size = ControlSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ControlVariant.Secondary, size, onClick, false, opacity, options);

        public bool GhostButton(string text, Action onClick, ControlSize size = ControlSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ControlVariant.Ghost, size, onClick, false, opacity, options);

        public bool LinkButton(string text, Action onClick, ControlSize size = ControlSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ControlVariant.Link, size, onClick, false, opacity, options);

        public bool SmallButton(string text, Action onClick, ControlVariant variant = ControlVariant.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, variant, ControlSize.Small, onClick, false, opacity, options);

        public bool LargeButton(string text, Action onClick, ControlVariant variant = ControlVariant.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, variant, ControlSize.Large, onClick, false, opacity, options);

        public bool IconButton(string text, Action onClick, ControlVariant variant = ControlVariant.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, variant, ControlSize.Icon, onClick, false, opacity, options);

        public bool Button(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, icon, variant, size, onClick, disabled, opacity, options ?? Array.Empty<GUILayoutOption>()) ?? false, "Button");

        public bool Button(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                    GetComponent<Button>()
                        ?.DrawButton(
                            new ButtonConfig(text)
                            {
                                Icon = icon,
                                Variant = variant,
                                Size = size,
                                OnClick = onClick,
                                Disabled = disabled,
                                Opacity = opacity,
                                Options = options ?? Array.Empty<GUILayoutOption>(),
                            }
                        ) ?? false,
                "Button"
            );

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f) => TryExecute(() => GetComponent<Button>()?.ButtonGroup(drawButtons, horizontal, spacing), "ButtonGroup");

        private GUILayoutOption[] GetButtonSizeOptions(ControlSize size) =>
            size switch
            {
                ControlSize.Small => new GUILayoutOption[] { GUILayout.Height(24 * uiScale) },
                ControlSize.Large => new GUILayoutOption[] { GUILayout.Height(40 * uiScale) },
                ControlSize.Icon => new GUILayoutOption[] { GUILayout.Width(36 * uiScale), GUILayout.Height(36 * uiScale) },
                _ => new GUILayoutOption[] { GUILayout.Height(30 * uiScale) },
            };

        #endregion

        #endregion

        #region Toggle Components

        #region Config-based API

        public bool Toggle(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    bool newValue = GetComponent<Toggle>()?.DrawToggle(text, value, variant, size, onToggle, disabled, options) ?? value;
                    if (newValue != value)
                        onToggle?.Invoke(newValue);
                    return newValue;
                },
                "Toggle"
            );

        public bool Toggle(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    bool newValue =
                        GetComponent<Toggle>()
                            ?.DrawToggle(
                                new ToggleConfig
                                {
                                    Text = text,
                                    Icon = icon,
                                    Value = value,
                                    Variant = variant,
                                    Size = size,
                                    OnToggle = onToggle,
                                    Disabled = disabled,
                                    Options = options,
                                }
                            ) ?? value;
                    if (newValue != value)
                        onToggle?.Invoke(newValue);
                    return newValue;
                },
                "Toggle"
            );

        #endregion

        #region API

        public bool OutlineToggle(string text, bool value, Action<bool> onToggle = null, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Toggle(text, value, ControlVariant.Outline, size, onToggle, false, options);

        public bool SmallToggle(string text, bool value, Action<bool> onToggle = null, ControlVariant variant = ControlVariant.Default, params GUILayoutOption[] options) => Toggle(text, value, variant, ControlSize.Small, onToggle, false, options);

        public bool LargeToggle(string text, bool value, Action<bool> onToggle = null, ControlVariant variant = ControlVariant.Default, params GUILayoutOption[] options) => Toggle(text, value, variant, ControlSize.Large, onToggle, false, options);

        #endregion

        #endregion

        #region Checkbox Components

        #region Config-based API

        public bool Checkbox(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Checkbox>()?.DrawCheckbox(text, value, variant, size, onToggle, disabled, options) ?? value, "Checkbox");

        public bool Checkbox(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            TryExecute(() => GetComponent<Checkbox>()?.DrawCheckbox(rect, text, value, variant, size, onToggle, disabled) ?? value, "Checkbox");

        public bool Checkbox(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                    GetComponent<Checkbox>()
                        ?.DrawCheckbox(
                            new CheckboxConfig
                            {
                                Text = text,
                                Icon = icon,
                                Value = value,
                                Variant = variant,
                                Size = size,
                                OnToggle = onToggle,
                                Disabled = disabled,
                                Options = options ?? Array.Empty<GUILayoutOption>(),
                            }
                        ) ?? value,
                "Checkbox"
            );

        #endregion

        #endregion

        #region Switch Components

        #region Config-based API

        public bool Switch(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Switch>()?.DrawSwitch(text, value, variant, size, onToggle, disabled, options) ?? value, "Switch");

        public bool Switch(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            TryExecute(() => GetComponent<Switch>()?.DrawSwitch(rect, text, value, variant, size, onToggle, disabled) ?? false, "Switch");

        public bool Switch(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                    GetComponent<Switch>()
                        ?.DrawSwitch(
                            new SwitchConfig
                            {
                                Text = text,
                                Icon = icon,
                                Value = value,
                                Variant = variant,
                                Size = size,
                                OnToggle = onToggle,
                                Disabled = disabled,
                                Options = options ?? Array.Empty<GUILayoutOption>(),
                            }
                        ) ?? value,
                "Switch"
            );

        #endregion

        #endregion

        #region Slider Components

        #region Config-based API

        public float Slider(SliderConfig config) => TryExecute(() => GetComponent<Slider>()?.Draw(config) ?? config?.Value ?? 0f, "Slider");

        #endregion

        #region API

        public float Slider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Slider>()?.Draw(value, min, max, options) ?? value, "Slider");

        public float Slider(float value, float min, float max, float step, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Slider>()?.Draw(value, min, max, step, options) ?? value, "Slider");

        public float LabeledSlider(string label, float value, float min, float max, bool showValue = true, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Slider>()?.LabeledSlider(label, value, min, max, showValue, options) ?? value, "Slider");

        public float LabeledSlider(string label, float value, float min, float max, float step, bool showValue = true, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Slider>()?.LabeledSlider(label, value, min, max, step, showValue, options) ?? value, "Slider");

        public float DisabledSlider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Slider>()?.DisabledSlider(value, min, max, options) ?? value, "Slider");

        #endregion

        #endregion

        #region Select Components

        #region API

        public int Select(string[] items, int selectedIndex) => TryExecute(() => GetComponent<Select>()?.DrawSelect(items, selectedIndex) ?? selectedIndex, "Select");

        public void OpenSelect() => GetComponent<Select>()?.Open();

        public void CloseSelect() => GetComponent<Select>()?.Close();

        public bool IsSelectOpen() => GetComponent<Select>()?.IsOpen ?? false;

        #endregion

        #endregion

        #region DropdownMenu Components

        #region Config-based API

        public void DropdownMenu(DropdownMenuConfig config) => TryExecute(() => GetComponent<DropdownMenu>()?.Draw(config), "DropdownMenu");

        #endregion

        #region API

        public bool IsDropdownMenuOpen() => GetComponent<DropdownMenu>()?.IsOpen ?? false;

        public void CloseDropdownMenu() => GetComponent<DropdownMenu>()?.Close();

        #endregion

        #endregion

        #region Card Components

        #region Config-based API

        public void DrawCard(CardConfig config) => TryExecute(() => GetComponent<Card>()?.DrawCard(config), "DrawCard");

        #endregion

        #region API

        public void BeginCard(float width = -1, float height = -1) => TryExecute(() => GetComponent<Card>()?.BeginCard(width, height), "BeginCard");

        public void EndCard() => TryExecute(() => GetComponent<Card>()?.EndCard(), "EndCard");

        public void DrawCard(string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) => TryExecute(() => GetComponent<Card>()?.DrawCard(title, description, content, footerContent, width, height), "DrawCard");

        public void DrawCardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) =>
            TryExecute(() => GetComponent<Card>()?.DrawCardWithImage(image, title, description, content, footerContent, width, height), "DrawCardWithImage");

        public void DrawCardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1) =>
            TryExecute(() => GetComponent<Card>()?.DrawCardWithAvatar(avatar, title, subtitle, content, footerContent, width, height), "DrawCardWithAvatar");

        public void DrawCardWithHeader(string title, string description, Action header, string content, Action footerContent = null, float width = -1, float height = -1) =>
            TryExecute(() => GetComponent<Card>()?.DrawCardWithHeader(title, description, header, content, footerContent, width, height), "DrawCardWithHeader");

        public void DrawSimpleCard(string content, float width = -1, float height = -1) => TryExecute(() => GetComponent<Card>()?.DrawSimpleCard(content, width, height), "DrawSimpleCard");

        public void CardHeader(Action content) => GetComponent<Card>()?.CardHeader(content);

        public void CardTitle(string title) => GetComponent<Card>()?.CardTitle(title);

        public void CardDescription(string description) => GetComponent<Card>()?.CardDescription(description);

        public void CardContent(Action content) => GetComponent<Card>()?.CardContent(content);

        public void CardFooter(Action content) => GetComponent<Card>()?.CardFooter(content);

        #endregion

        #endregion

        #region Layout Components

        #region API

        public Vector2 DrawScrollView(Vector2 scrollPosition, Action drawContent, params GUILayoutOption[] options) => TryExecute(() => layoutComponents?.DrawScrollView(scrollPosition, drawContent, options) ?? scrollPosition, "DrawScrollView");

        public void BeginHorizontalGroup() => TryExecute(() => layoutComponents?.BeginHorizontalGroup(), "BeginHorizontalGroup");

        public void EndHorizontalGroup() => TryExecute(() => layoutComponents?.EndHorizontalGroup(), "EndHorizontalGroup");

        public void BeginVerticalGroup(params GUILayoutOption[] options) => TryExecute(() => layoutComponents?.BeginVerticalGroup(options), "BeginVerticalGroup");

        public void EndVerticalGroup() => TryExecute(() => layoutComponents?.EndVerticalGroup(), "EndVerticalGroup");

        public void AddSpace(float pixels) => TryExecute(() => layoutComponents?.AddSpace(pixels), "AddSpace");

        #endregion

        #endregion

        #region Label Components

        #region Config-based API

        public void Label(LabelConfig config) => TryExecute(() => GetComponent<Label>()?.DrawLabel(config), "Label");

        #endregion

        #region API

        public void Label(string text, ControlVariant variant = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.DrawLabel(text, variant, disabled, options), "Label");

        public void Label(Rect rect, string text, ControlVariant variant = ControlVariant.Default, bool disabled = false) => TryExecute(() => GetComponent<Label>()?.DrawLabel(rect, text, variant, disabled), "Label");

        public void SecondaryLabel(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.SecondaryLabel(text, options), "SecondaryLabel");

        public void MutedLabel(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.MutedLabel(text, options), "MutedLabel");

        public void DestructiveLabel(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.DestructiveLabel(text, options), "DestructiveLabel");

        public void Label(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                    GetComponent<Label>()
                        ?.DrawLabel(
                            new LabelConfig
                            {
                                Text = text,
                                Icon = icon,
                                Variant = variant,
                                Disabled = disabled,
                                Options = options ?? Array.Empty<GUILayoutOption>(),
                            }
                        ),
                "Label"
            );

        #endregion

        #endregion

        #region Progress Components

        #region Config-based API

        public void Progress(ProgressConfig config) => TryExecute(() => GetComponent<Progress>()?.DrawProgress(config), "Progress");

        #endregion

        #region API

        public void Progress(float value, float width = -1, float height = -1, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.DrawProgress(value, width, height, options), "Progress");

        public void Progress(Rect rect, float value) => TryExecute(() => GetComponent<Progress>()?.DrawProgress(rect, value), "Progress");

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.LabeledProgress(label, value, width, height, showPercentage, options), "LabeledProgress");

        public void AnimatedProgress(string id, float value, float width = -1, float height = -1, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.AnimatedProgress(id, value, width, height, options), "AnimatedProgress");

        public void CircularProgress(float value, float size = 32f, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.CircularProgress(value, size, options), "CircularProgress");

        #endregion

        #endregion

        #region Badge Components

        #region Config-based API

        public void Badge(BadgeConfig config) => TryExecute(() => GetComponent<Badge>()?.DrawBadge(config), "Badge");

        #endregion

        #region API

        public void Badge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.DrawBadge(text, variant, size, options), "Badge");

        public void Badge(Rect rect, string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => TryExecute(() => GetComponent<Badge>()?.DrawBadge(rect, text, variant, size), "Badge");

        public void BadgeWithIcon(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.BadgeWithIcon(text, icon, variant, size, options), "BadgeWithIcon");

        public void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.CountBadge(count, variant, size, maxCount, options), "CountBadge");

        public void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.StatusBadge(text, isActive, variant, size, options), "StatusBadge");

        public void ProgressBadge(string text, float progress, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Badge>()?.ProgressBadge(text, progress, variant, size, options), "ProgressBadge");

        public void AnimatedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.AnimatedBadge(text, variant, size, options), "AnimatedBadge");

        public void AnimatedBadge(string text, string animationId, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Badge>()?.AnimatedBadge(text, animationId, variant, size, options), "AnimatedBadge");

        public void RoundedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, float cornerRadius = 12f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Badge>()?.RoundedBadge(text, variant, size, cornerRadius, options), "RoundedBadge");

        public void PillBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.PillBadge(text, variant, size, options), "PillBadge");

        public void Badge(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                    GetComponent<Badge>()
                        ?.DrawBadge(
                            new BadgeConfig
                            {
                                Text = text,
                                Icon = icon,
                                Variant = variant,
                                Size = size,
                                Options = options ?? Array.Empty<GUILayoutOption>(),
                            }
                        ),
                "Badge"
            );

        #endregion

        #endregion

        #region Avatar Components

        #region Config-based API

        public void Avatar(AvatarConfig config) => TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(config), "Avatar");

        #endregion

        #region API

        public void Avatar(Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(image, fallbackText, size, shape, options), "Avatar");

        public void Avatar(Rect rect, Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle) => TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(rect, image, fallbackText, size, shape), "Avatar");

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options), "AvatarWithStatus");

        public void AvatarWithName(Texture2D image, string fallbackText, string name, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options), "AvatarWithName");

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options), "AvatarWithBorder");

        #endregion

        #endregion

        #region Dialog Components

        #region Config-based API

        public void Dialog(DialogConfig config) => TryExecute(() => GetComponent<Dialog>()?.DrawDialog(config), "Dialog");

        #endregion

        #region API

        public void DrawDialog(string dialogId, Action content, float width = 400, float height = 300) => TryExecute(() => GetComponent<Dialog>()?.DrawDialog(dialogId, content, width, height), "DrawDialog");

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300) => TryExecute(() => GetComponent<Dialog>()?.DrawDialog(dialogId, title, description, content, footer, width, height), "DrawDialog");

        public void DrawDialog(DialogConfig config) => TryExecute(() => GetComponent<Dialog>()?.DrawDialog(config), "DrawDialog");

        public void OpenDialog(string dialogId) => GetComponent<Dialog>()?.Open(dialogId);

        public void CloseDialog() => GetComponent<Dialog>()?.Close();

        public bool IsDialogOpen() => GetComponent<Dialog>()?.IsOpen ?? false;

        public bool DrawDialogTrigger(string label, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            try
            {
                return GetComponent<Dialog>()?.DrawDialogTrigger(label, variant, size) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawDialogTrigger", "GUIHelper");
                return false;
            }
        }

        public void DrawDialogHeader(string title, string description = null) => TryExecute(() => GetComponent<Dialog>()?.DrawDialogHeader(title, description), "DrawDialogHeader");

        public void DrawDialogContent(Action content) => TryExecute(() => GetComponent<Dialog>()?.DrawDialogContent(content), "DrawDialogContent");

        public void DrawDialogFooter(Action footer) => TryExecute(() => GetComponent<Dialog>()?.DrawDialogFooter(footer), "DrawDialogFooter");

        #endregion

        #endregion

        #region Popover Components

        #region Config-based API

        public void Popover(PopoverConfig config) => TryExecute(() => GetComponent<Popover>()?.DrawPopover(config), "Popover");

        #endregion

        #region API

        public void Popover(Action content) => TryExecute(() => GetComponent<Popover>()?.DrawPopover(content), "Popover");

        public void OpenPopover() => GetComponent<Popover>()?.Open();

        public void ClosePopover() => GetComponent<Popover>()?.Close();

        public bool IsPopoverOpen() => GetComponent<Popover>()?.IsOpen ?? false;

        #endregion

        #endregion

        #region Toast Components
        #region Config-based API
        public void ShowToast(ToastConfig config) => TryExecute(() => GetComponent<Toast>()?.Show(config), "ShowToast");
        #endregion

        #region Fluent API
        public ToastConfigBuilder Toast() => new ToastConfigBuilder(this);

        public class ToastConfigBuilder
        {
            private ToastConfig _config;
            private GUIHelper _helper;

            public ToastConfigBuilder(GUIHelper helper)
            {
                _helper = helper;
                _config = new ToastConfig();
            }

            public ToastConfigBuilder Title(string title)
            {
                _config.Title = title;
                return this;
            }

            public ToastConfigBuilder Description(string description)
            {
                _config.Description = description;
                return this;
            }

            public ToastConfigBuilder Variant(ToastVariant variant)
            {
                _config.Variant = variant;
                return this;
            }

            public ToastConfigBuilder Duration(float ms)
            {
                _config.DurationMs = ms;
                return this;
            }

            public ToastConfigBuilder Dismissible(bool dismissible)
            {
                _config.Dismissible = dismissible;
                return this;
            }

            public ToastConfigBuilder Position(ToastPosition position)
            {
                _config.Position = position;
                return this;
            }

            public ToastConfigBuilder StackDirection(ToastStackDirection direction)
            {
                _config.StackDirection = direction;
                return this;
            }

            public ToastConfigBuilder Action(string label, Action onAction)
            {
                _config.ActionLabel = label;
                _config.OnAction = onAction;
                return this;
            }

            public ToastConfigBuilder PauseOnHover(bool enable)
            {
                _config.EnablePauseOnHover = enable;
                return this;
            }

            public ToastConfigBuilder HoverDelay(float delay)
            {
                _config.HoverPauseDelay = delay;
                return this;
            }

            public ToastConfigBuilder ClickToDismiss(bool enable)
            {
                _config.EnableClickToDismiss = enable;
                return this;
            }

            public ToastConfigBuilder ProgressBar(bool show)
            {
                _config.ShowProgressBar = show;
                return this;
            }

            public ToastConfigBuilder AccentBar(bool show)
            {
                _config.ShowAccentBar = show;
                return this;
            }

            public ToastConfigBuilder Width(float width)
            {
                _config.Width = width;
                return this;
            }

            public ToastConfigBuilder Padding(float padding)
            {
                _config.Padding = padding;
                return this;
            }

            public ToastConfigBuilder BorderRadius(float radius)
            {
                _config.BorderRadius = radius;
                return this;
            }

            public string Show()
            {
                _helper.TryExecute(() => _helper.GetComponent<Toast>()?.Show(_config), "ShowToast");
                return _config.Id;
            }

            public ToastConfig Build() => _config;
        }
        #endregion

        #region Quick API
        public string ShowToast(string title, string description = "", ToastVariant variant = ToastVariant.Default) =>
            TryExecute(
                () =>
                {
                    var config = new ToastConfig
                    {
                        Title = title,
                        Description = description,
                        Variant = variant,
                    };
                    var toast = GetComponent<Toast>();
                    toast?.Show(config);
                    return config.Id;
                },
                "ShowToast"
            );

        public string ShowSuccessToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Success);

        public string ShowErrorToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Error);

        public string ShowWarningToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Warning);

        public string ShowInfoToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Info);
        #endregion

        #region Toast Management
        public void DismissToast(string id) => TryExecute(() => GetComponent<Toast>()?.Dismiss(id), "DismissToast");

        public void DismissAllToasts() => TryExecute(() => GetComponent<Toast>()?.DismissAll(), "DismissAllToasts");

        public void PauseToast(string id) => TryExecute(() => GetComponent<Toast>()?.PauseToast(id), "PauseToast");

        public void ResumeToast(string id) => TryExecute(() => GetComponent<Toast>()?.ResumeToast(id), "ResumeToast");

        public void DrawToasts() => TryExecute(() => GetComponent<Toast>()?.DrawToasts(), "DrawToasts");
        #endregion

        #region Toast Info
        public int GetActiveToastCount() => TryExecute(() => GetComponent<Toast>()?.GetActiveToastCount() ?? 0, "GetActiveToastCount");

        public bool HasToast(string id) => TryExecute(() => GetComponent<Toast>()?.HasToast(id) ?? false, "HasToast");

        public List<string> GetActiveToastIds() => TryExecute(() => GetComponent<Toast>()?.GetActiveToastIds() ?? new List<string>(), "GetActiveToastIds");
        #endregion

        #region Toast Configuration
        public void SetMaxConcurrentToasts(int max) =>
            TryExecute(
                () =>
                {
                    var t = GetComponent<Toast>();
                    if (t != null)
                        t.MaxConcurrentToasts = max;
                },
                "SetMaxConcurrentToasts"
            );

        public void SetDismissAnimationDuration(float duration) =>
            TryExecute(
                () =>
                {
                    var t = GetComponent<Toast>();
                    if (t != null)
                        t.GlobalDismissAnimationDuration = duration;
                },
                "SetDismissAnimationDuration"
            );

        public void EnablePauseOnHover(bool enable) =>
            TryExecute(
                () =>
                {
                    var t = GetComponent<Toast>();
                    if (t != null)
                        t.GlobalEnablePauseOnHover = enable;
                },
                "EnablePauseOnHover"
            );

        public void EnableGrouping(bool enable, int timeWindowMs = 500) =>
            TryExecute(
                () =>
                {
                    var t = GetComponent<Toast>();
                    if (t != null)
                    {
                        t.EnableGlobalGrouping = enable;
                        t.GroupingTimeWindowMs = timeWindowMs;
                    }
                },
                "EnableGrouping"
            );
        #endregion
        #endregion

        #region Separator Components

        #region Config-based API

        public void Separator(SeparatorConfig config) => TryExecute(() => GetComponent<Separator>()?.DrawSeparator(config), "Separator");

        #endregion

        #region API

        public void Separator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.DrawSeparator(orientation, decorative, options), "Separator");

        public void HorizontalSeparator(params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.HorizontalSeparator(options), "HorizontalSeparator");

        public void VerticalSeparator(params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.VerticalSeparator(options), "VerticalSeparator");

        public void Separator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal) => TryExecute(() => GetComponent<Separator>()?.DrawSeparator(rect, orientation), "Separator");

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, float spacingBefore = 8f, float spacingAfter = 8f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Separator>()?.SeparatorWithSpacing(orientation, spacingBefore, spacingAfter, options), "SeparatorWithSpacing");

        public void LabeledSeparator(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.LabeledSeparator(text, options), "LabeledSeparator");

        #endregion

        #endregion

        #region Table Components

        #region Config-based API

        public void Table(TableConfig config) => TryExecute(() => GetComponent<Table>()?.DrawTable(config), "Table");

        #endregion

        #region API

        public void Table(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Table>()?.DrawTable(headers, data, variant, size, options), "Table");

        public void Table(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => TryExecute(() => GetComponent<Table>()?.DrawTable(rect, headers, data, variant, size), "Table");

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.SortableTable(headers, data, ref sortColumns, ref sortAscending, variant, size, onSort, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SortableTable", "GUIHelper");
            }
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.SelectableTable(headers, data, ref selectedRows, variant, size, onSelectionChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SelectableTable", "GUIHelper");
            }
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Table>()?.CustomTable(headers, data, cellRenderer, variant, size, options), "CustomTable");

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.PaginatedTable(headers, data, ref currentPage, pageSize, variant, size, onPageChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "PaginatedTable", "GUIHelper");
            }
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery, ref string[,] filteredData, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.SearchableTable(headers, data, ref searchQuery, ref filteredData, variant, size, onSearch, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SearchableTable", "GUIHelper");
            }
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.ResizableTable(headers, data, ref columnWidths, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResizableTable", "GUIHelper");
            }
        }

        #endregion

        #endregion

        #region Tabs Components

        #region Config-based API

        public int Tabs(TabsConfig config) => TryExecute(() => GetComponent<Tabs>()?.Draw(config) ?? config.SelectedIndex, "Tabs");

        #endregion

        #region API

        public int DrawTabs(string[] tabNames, int selectedIndex, Action<int> onTabChange, int maxLines = 1, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var config = new TabsConfig(tabNames, selectedIndex)
                    {
                        OnTabChange = onTabChange,
                        MaxLines = maxLines,
                        Options = options,
                    };

                    var result = GetComponent<Tabs>()?.Draw(config);
                    return result ?? selectedIndex;
                },
                "DrawTabs"
            );

        public int DrawTabs(string[] tabNames, int selectedIndex, Action content, int maxLines = 1, TabPosition position = TabPosition.Top, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var config = new TabsConfig(tabNames, selectedIndex)
                    {
                        Content = content,
                        MaxLines = maxLines,
                        Position = position,
                        Options = options,
                    };

                    var result = GetComponent<Tabs>()?.Draw(config);
                    return result ?? selectedIndex;
                },
                "DrawTabs"
            );

        public void BeginTabContent() => TryExecute(() => GetComponent<Tabs>()?.BeginTabContent(), "BeginTabContent");

        public void EndTabContent() => TryExecute(() => GetComponent<Tabs>()?.EndTabContent(), "EndTabContent");

        public int TabsWithContent(Tabs.TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    if (tabConfigs == null || tabConfigs.Length == 0)
                        return selectedIndex;

                    var tabNames = new string[tabConfigs.Length];
                    for (var i = 0; i < tabConfigs.Length; i++)
                        tabNames[i] = tabConfigs[i].Name;

                    var config = new TabsConfig(tabNames, selectedIndex)
                    {
                        OnTabChange = onTabChange,
                        MaxLines = 1,
                        Position = TabPosition.Top,
                        Options = options,
                    };

                    var newSelectedIndex = GetComponent<Tabs>()?.Draw(config) ?? selectedIndex;

                    if (newSelectedIndex >= 0 && newSelectedIndex < tabConfigs.Length)
                    {
                        BeginTabContent();
                        tabConfigs[newSelectedIndex].Content?.Invoke();
                        EndTabContent();
                    }

                    return newSelectedIndex;
                },
                "TabsWithContent"
            );

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action content, float tabWidth = 120f, int maxLines = 1, TabSide side = TabSide.Left, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var position = side == TabSide.Left ? TabPosition.Left : TabPosition.Right;
                    var config = new TabsConfig(tabNames, selectedIndex)
                    {
                        Content = content,
                        TabWidth = tabWidth,
                        MaxLines = maxLines,
                        Side = side,
                        Position = position,
                        Options = options,
                    };
                    return GetComponent<Tabs>()?.Draw(config) ?? selectedIndex;
                },
                "VerticalTabs"
            );

        public int DrawClosableTabs(ref string[] tabNames, ref bool[] closableTabs, int selectedIndex, Action content = null, Action<int> onTabChange = null)
        {
            try
            {
                return GetComponent<Tabs>()?.DrawWithAutoClose(ref tabNames, ref closableTabs, selectedIndex, content, onTabChange) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawClosableTabs", "GUIHelper");
                return selectedIndex;
            }
        }

        #endregion

        #endregion

        #region MenuBar Components

        #region Config-based API

        public void MenuBar(MenuBar.MenuBarConfig config) => TryExecute(() => GetComponent<MenuBar>()?.Draw(config), "MenuBar");

        #endregion

        #region API

        public void MenuBar(List<MenuBar.MenuItem> items, params GUILayoutOption[] options)
        {
            try
            {
                var config = new MenuBar.MenuBarConfig(items) { Options = options };
                GetComponent<MenuBar>()?.Draw(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "MenuBar", "GUIHelper");
            }
        }

        #endregion

        #endregion

        #region Calendar Components

        #region Config-based API

        public void Calendar(CalendarConfig config) => TryExecute(() => GetComponent<Calendar>()?.DrawCalendar(config), "Calendar");

        #endregion

        #region API

        public void Calendar() => TryExecute(() => GetComponent<Calendar>()?.DrawCalendar(), "Calendar");

        #endregion

        #endregion

        #region DatePicker Components

        #region Config-based API

        public DateTime? DatePicker(DatePickerConfig config) => TryExecute(() => GetComponent<DatePicker>()?.DrawDatePicker(config), "DatePicker");

        #endregion

        #region API

        public DateTime? DatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options) => TryExecute(() => GetComponent<DatePicker>()?.DrawDatePicker(placeholder, selectedDate, id, options) ?? selectedDate, "DatePicker");

        public DateTime? DatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<DatePicker>()?.DrawDatePickerWithLabel(label, placeholder, selectedDate, id, options) ?? selectedDate, "DatePickerWithLabel");

        public DateTime? DateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<DatePicker>()?.DrawDateRangePicker(placeholder, startDate, endDate, id, options) ?? startDate, "DateRangePicker");

        public void CloseDatePicker(string id) => TryExecute(() => GetComponent<DatePicker>()?.CloseDatePicker(id), "CloseDatePicker");

        public bool IsDatePickerOpen(string id) => TryExecute(() => GetComponent<DatePicker>()?.IsDatePickerOpen(id) ?? false, "IsDatePickerOpen");

        #endregion

        #endregion

        #region DataTable Components

        #region API

        public void DrawDataTable(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<DataTable>()?.DrawDataTable(id, columns, data, showPagination, showSearch, showSelection, showColumnToggle, options), "DrawDataTable");

        public DataTableColumn CreateDataTableColumn(string id, string header, string accessorKey = null) => new DataTableColumn(id, header, accessorKey);

        public DataTableRow CreateDataTableRow(string id) => new DataTableRow(id);

        public List<string> GetSelectedRows(string tableId) => TryExecute(() => GetComponent<DataTable>()?.GetSelectedRows(tableId) ?? new List<string>(), "GetSelectedRows");

        public void ClearSelection(string tableId) => TryExecute(() => GetComponent<DataTable>()?.ClearSelection(tableId), "ClearSelection");

        #endregion

        #endregion

        #region Chart Components

        #region Config-based API

        public void Chart(ChartConfig config) => TryExecute(() => GetComponent<Chart>()?.DrawChart(config), "Chart");

        #endregion

        #region API

        public void Chart(List<ChartSeries> chartSeries, ChartType chartType, Vector2 size, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var config = new ChartConfig(chartSeries, chartType) { Size = size, Options = options };
                    GetComponent<Chart>()?.DrawChart(config);
                },
                "Chart"
            );

        #endregion

        #endregion

        #region TextArea Components

        #region Config-based API

        public string TextArea(TextAreaConfig config) => TryExecute(() => GetComponent<TextArea>()?.DrawTextArea(config) ?? config.Text, "TextArea");

        #endregion

        #region API

        public string TextArea(string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options) ?? text, "TextArea");

        public string TextArea(Rect rect, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1) =>
            TryExecute(() => GetComponent<TextArea>()?.DrawTextArea(rect, text, variant, placeholder, disabled, maxLength) ?? text, "TextArea");

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text, "OutlineTextArea");

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text, "GhostTextArea");

        public string LabeledTextArea(string label, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight, maxLength, showCharCount, options) ?? text, "LabeledTextArea");

        public string ResizableTextArea(string text, ref float height, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return GetComponent<TextArea>()?.ResizableTextArea(text, ref height, variant, placeholder, disabled, minHeight, maxHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResizableTextArea", "GUIHelper");
                return text;
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        private void TryExecute(Action action, string methodName)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, methodName, "GUIHelper");
            }
        }

        private T TryExecute<T>(Func<T> func, string methodName)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, methodName, "GUIHelper");
                return default;
            }
        }

        #endregion
    }

    #region Component Resolution Infrastructure

    internal interface IComponentResolver
    {
        T GetComponent<T>()
            where T : BaseComponent;
    }

    internal class ComponentResolver : IComponentResolver
    {
        private readonly Dictionary<Type, BaseComponent> _componentInstances;

        public ComponentResolver(Dictionary<Type, BaseComponent> componentInstances)
        {
            _componentInstances = componentInstances;
        }

        public T GetComponent<T>()
            where T : BaseComponent
        {
            if (_componentInstances.TryGetValue(typeof(T), out BaseComponent component))
            {
                return (T)component;
            }
            GUILogger.LogWarning($"Component of type {typeof(T).Name} not found.", "ComponentResolver.GetComponent<T>");
            return null;
        }
    }

    #endregion
}
