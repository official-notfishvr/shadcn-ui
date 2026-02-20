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
    public partial class GUIHelper
    {
        #region Fields

        private readonly Dictionary<Type, BaseComponent> _components;
        private readonly IComponentResolver _resolver;

        #endregion

        #region Internal Settings

        internal int fontSize = 14;
        public float uiScale = 1f;

        #endregion

        #region Component Instances

        private StyleManager _styleManager;
        private AnimationManager _animationManager;
        private Layout.Layout _layout;
        private bool _initialized = false;

        #endregion

        #region Constructor

        public GUIHelper()
        {
            _components = new Dictionary<Type, BaseComponent>();
            _resolver = new ComponentResolver(_components);
            Initialize();
        }

        #endregion

        #region Initialization

        private void Initialize()
        {
            try
            {
                GUILogger.LogInfo("Initializing GUIHelper components", "GUIHelper.Initialize");

                _styleManager = new StyleManager(this);
                _animationManager = new AnimationManager(this);

                _components[typeof(Input)] = new Input(this);
                _components[typeof(TextArea)] = new TextArea(this);
                _components[typeof(Checkbox)] = new Checkbox(this);
                _components[typeof(Switch)] = new Switch(this);
                _components[typeof(Button)] = new Button(this);
                _components[typeof(Toggle)] = new Toggle(this);
                _components[typeof(Select)] = new Select(this);
                _components[typeof(DropdownMenu)] = new DropdownMenu(this);
                _components[typeof(Slider)] = new Slider(this);

                _layout = new Layout.Layout(this);
                _components[typeof(Card)] = new Card(this);
                _components[typeof(Separator)] = new Separator(this);
                _components[typeof(Tabs)] = new Tabs(this);
                _components[typeof(MenuBar)] = new MenuBar(this);
                _components[typeof(Table)] = new Table(this);
                _components[typeof(Navigation)] = new Navigation(this);

                _components[typeof(Label)] = new Label(this);
                _components[typeof(Progress)] = new Progress(this);
                _components[typeof(Badge)] = new Badge(this);
                _components[typeof(Display.Avatar)] = new Display.Avatar(this);
                _components[typeof(Chart)] = new Chart(this);
                _components[typeof(Dialog)] = new Dialog(this);
                _components[typeof(Popover)] = new Popover(this);
                _components[typeof(Toast)] = new Toast(this);
                _components[typeof(Tooltip)] = new Tooltip(this);

                _components[typeof(ThemeChanger)] = new ThemeChanger(this);

                GUILogger.LogInfo("GUIHelper components initialized successfully", "GUIHelper.Initialize");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Initialize", "GUIHelper");
                _styleManager = null;
                _animationManager = null;
            }
        }

        #endregion

        #region Component Resolution

        private T Get<T>()
            where T : BaseComponent
        {
            try
            {
                return _resolver.GetComponent<T>();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, $"Get<{typeof(T).Name}>", "GUIHelper");
                return null;
            }
        }

        public StyleManager GetStyleManager() => _styleManager;

        public AnimationManager GetAnimationManager() => _animationManager;

        public Chart GetChartComponent() => Get<Chart>();

        public Chart GetChartComponents() => Get<Chart>();

        #endregion

        #region Animation Management

        public void UpdateGUI(bool isOpen) { }

        public bool BeginGUI()
        {
            try
            {
                if (_styleManager == null)
                {
                    GUILogger.LogError("StyleManager is null in BeginGUI", "GUIHelper");
                    return false;
                }

                if (!_initialized)
                {
                    _styleManager.InitializeGUI();
                    _initialized = true;
                    GUILogger.LogInfo("GUIHelper initialized in BeginGUI", "GUIHelper");
                }

                if (_styleManager.ScanForCorruption())
                    _styleManager.MarkStylesCorruption();
                _styleManager.RefreshStylesIfCorruption();

                return _animationManager?.BeginGUI() ?? true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginGUI", "GUIHelper");
                return true;
            }
        }

        public void EndGUI()
        {
            try
            {
                _animationManager?.EndGUI();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "EndGUI", "GUIHelper");
            }
        }

        public void Cleanup()
        {
            try
            {
                GUILogger.LogInfo("Starting GUIHelper cleanup", "GUIHelper.Cleanup");
                _styleManager?.Cleanup();
                _animationManager?.Cleanup();
                GUILogger.LogInfo("GUIHelper cleanup completed", "GUIHelper.Cleanup");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Cleanup", "GUIHelper");
            }
        }

        #endregion

        #region Input Components

        public string Input(InputConfig config) => Execute(() => Get<Input>()?.DrawInput(config) ?? config.Value, "Input");

        public string LabeledInput(InputConfig config) => Execute(() => Get<Input>()?.DrawLabeledInput(config) ?? config.Value, "LabeledInput");

        public void SectionHeader(string title) => Execute(() => Get<Input>()?.DrawSectionHeader(title), "SectionHeader");

        public void InputLabel(string text, int width = -1) => Execute(() => Get<Input>()?.RenderLabel(text, width), "InputLabel");

        public string PasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            try
            {
                return Get<Input>()?.DrawPasswordField(windowWidth, label, ref password, maskChar) ?? password;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "PasswordField", "GUIHelper");
                return password;
            }
        }

        public void MultilineInput(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            try
            {
                Get<Input>()?.DrawTextArea(windowWidth, label, ref text, maxLength, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "MultilineInput", "GUIHelper");
            }
        }

        public string Input(string value, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            return Execute(() => Get<Input>()?.DrawInput(value, placeholder, variant, disabled, focused, width, onChange) ?? value, "Input");
        }

        public string Input(string value, Texture2D icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            return Execute(() => Get<Input>()?.DrawInput(value, icon, placeholder, variant, disabled, focused, width, onChange) ?? value, "Input");
        }

        public string Input(string value, IconConfig icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            return Execute(
                () =>
                    Get<Input>()
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
                "Input"
            );
        }

        public string LabeledInput(string label, string value, string placeholder = "", ControlVariant inputVariant = ControlVariant.Default, ControlVariant labelVariant = ControlVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null)
        {
            return Execute(() => Get<Input>()?.DrawLabeledInput(label, value, placeholder, inputVariant, labelVariant, disabled, inputWidth, onChange) ?? value, "LabeledInput");
        }

        #endregion

        #region Button Components

        public bool Button(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Execute(() => Get<Button>()?.DrawButton(text, variant, size, onClick, disabled, opacity, options) ?? false, "Button");
        }

        public bool Button(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Execute(() => Get<Button>()?.DrawButton(text, icon, variant, size, onClick, disabled, opacity, options) ?? false, "Button");
        }

        public bool Button(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                    Get<Button>()
                        ?.DrawButton(
                            new ButtonConfig(text)
                            {
                                Icon = icon,
                                Variant = variant,
                                Size = size,
                                OnClick = onClick,
                                Disabled = disabled,
                                Opacity = opacity,
                                Options = options,
                            }
                        ) ?? false,
                "Button"
            );
        }

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            Execute(() => Get<Button>()?.ButtonGroup(drawButtons, horizontal, spacing), "ButtonGroup");
        }

        #endregion

        #region Toggle Components

        public bool Toggle(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                {
                    bool newValue = Get<Toggle>()?.DrawToggle(text, value, variant, size, onToggle, disabled, options) ?? value;
                    if (newValue != value)
                        onToggle?.Invoke(newValue);
                    return newValue;
                },
                "Toggle"
            );
        }

        public bool Toggle(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                {
                    bool newValue =
                        Get<Toggle>()
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
        }

        #endregion

        #region Checkbox Components

        public bool Checkbox(CheckboxConfig config, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            config.OnToggle = onToggle;
            config.Disabled = disabled;
            if (options != null && options.Length > 0)
                config.Options = options;
            return Execute(() => Get<Checkbox>()?.DrawCheckbox(config) ?? config.Value, "Checkbox");
        }

        public bool Checkbox(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return Execute(() => Get<Checkbox>()?.DrawCheckbox(text, value, variant, size, onToggle, disabled, options) ?? value, "Checkbox");
        }

        public bool Checkbox(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            return Execute(() => Get<Checkbox>()?.DrawCheckbox(rect, text, value, variant, size, onToggle, disabled) ?? value, "Checkbox");
        }

        public bool Checkbox(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                    Get<Checkbox>()
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
                                Options = options,
                            }
                        ) ?? value,
                "Checkbox"
            );
        }

        #endregion

        #region Switch Components

        public bool Switch(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return Execute(() => Get<Switch>()?.DrawSwitch(text, value, variant, size, onToggle, disabled, options) ?? value, "Switch");
        }

        public bool Switch(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            return Execute(() => Get<Switch>()?.DrawSwitch(rect, text, value, variant, size, onToggle, disabled) ?? value, "Switch");
        }

        public bool Switch(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                    Get<Switch>()
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
                                Options = options,
                            }
                        ) ?? value,
                "Switch"
            );
        }

        #endregion

        #region Slider Components

        public float Slider(SliderConfig config) => Execute(() => Get<Slider>()?.Draw(config) ?? config?.Value ?? 0f, "Slider");

        public float Slider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options) => Execute(() => Get<Slider>()?.Draw(value, min, max, options) ?? value, "Slider");

        public float Slider(float value, float min, float max, float step, params GUILayoutOption[] options) => Execute(() => Get<Slider>()?.Draw(value, min, max, step, options) ?? value, "Slider");

        public float LabeledSlider(string label, float value, float min, float max, bool showValue = true, params GUILayoutOption[] options) => Execute(() => Get<Slider>()?.LabeledSlider(label, value, min, max, showValue, options) ?? value, "LabeledSlider");

        public float LabeledSlider(string label, float value, float min, float max, float step, bool showValue = true, params GUILayoutOption[] options) => Execute(() => Get<Slider>()?.LabeledSlider(label, value, min, max, step, showValue, options) ?? value, "LabeledSlider");

        public float DisabledSlider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options) => Execute(() => Get<Slider>()?.DisabledSlider(value, min, max, options) ?? value, "DisabledSlider");

        #endregion

        #region Select Components

        public int Select(string[] items, int selectedIndex) => Execute(() => Get<Select>()?.DrawSelect(items, selectedIndex) ?? selectedIndex, "Select");

        public int Select(SelectConfig config) => Execute(() => Get<Select>()?.DrawSelect(config) ?? config.SelectedIndex, "Select");

        public void OpenSelect() => Get<Select>()?.Open();

        public void CloseSelect() => Get<Select>()?.Close();

        public bool IsSelectOpen() => Get<Select>()?.IsOpen ?? false;

        #endregion

        #region DropdownMenu Components

        public void DropdownMenu(DropdownMenuConfig config) => Execute(() => Get<DropdownMenu>()?.Draw(config), "DropdownMenu");

        public bool IsDropdownMenuOpen() => Get<DropdownMenu>()?.IsOpen ?? false;

        public void CloseDropdownMenu() => Get<DropdownMenu>()?.Close();

        #endregion

        #region Card Components

        public void Card(CardConfig config) => Execute(() => Get<Card>()?.DrawCard(config), "Card");

        public void BeginCard(float width = -1, float height = -1) => Execute(() => Get<Card>()?.BeginCard(width, height), "BeginCard");

        public void EndCard() => Execute(() => Get<Card>()?.EndCard(), "EndCard");

        public void Card(string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) => Execute(() => Get<Card>()?.DrawCard(title, description, content, footerContent, width, height), "Card");

        public void CardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) => Execute(() => Get<Card>()?.DrawCardWithImage(image, title, description, content, footerContent, width, height), "CardWithImage");

        public void CardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1) => Execute(() => Get<Card>()?.DrawCardWithAvatar(avatar, title, subtitle, content, footerContent, width, height), "CardWithAvatar");

        public void CardWithHeader(string title, string description, Action header, string content, Action footerContent = null, float width = -1, float height = -1) =>
            Execute(() => Get<Card>()?.DrawCardWithHeader(title, description, header, content, footerContent, width, height), "CardWithHeader");

        public void SimpleCard(string content, float width = -1, float height = -1) => Execute(() => Get<Card>()?.DrawSimpleCard(content, width, height), "SimpleCard");

        public void CardHeader(Action content) => Get<Card>()?.CardHeader(content);

        public void CardTitle(string title) => Get<Card>()?.CardTitle(title);

        public void CardDescription(string description) => Get<Card>()?.CardDescription(description);

        public void CardContent(Action content) => Get<Card>()?.CardContent(content);

        public void CardFooter(Action content) => Get<Card>()?.CardFooter(content);

        #endregion

        #region Layout Components

        public Vector2 ScrollView(Vector2 scrollPosition, Action drawContent, params GUILayoutOption[] options) => Execute(() => _layout?.DrawScrollView(scrollPosition, drawContent, options) ?? scrollPosition, "ScrollView");

        public void BeginHorizontalGroup() => Execute(() => _layout?.BeginHorizontalGroup(), "BeginHorizontalGroup");

        public void BeginHorizontalGroup(params GUILayoutOption[] options) => Execute(() => _layout?.BeginHorizontalGroup(options), "BeginHorizontalGroup");

        public void EndHorizontalGroup() => Execute(() => _layout?.EndHorizontalGroup(), "EndHorizontalGroup");

        public void BeginVerticalGroup(params GUILayoutOption[] options) => Execute(() => _layout?.BeginVerticalGroup(options), "BeginVerticalGroup");

        public void EndVerticalGroup() => Execute(() => _layout?.EndVerticalGroup(), "EndVerticalGroup");

        public void AddSpace(float pixels) => Execute(() => _layout?.AddSpace(pixels), "AddSpace");

        #endregion

        #region Label Components

        public void Label(LabelConfig config) => Execute(() => Get<Label>()?.DrawLabel(config), "Label");

        public void Label(string text, ControlVariant variant = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] options) => Execute(() => Get<Label>()?.DrawLabel(text, variant, disabled, options), "Label");

        public void Label(Rect rect, string text, ControlVariant variant = ControlVariant.Default, bool disabled = false) => Execute(() => Get<Label>()?.DrawLabel(rect, text, variant, disabled), "Label");

        public void SecondaryLabel(string text, params GUILayoutOption[] options) => Execute(() => Get<Label>()?.SecondaryLabel(text, options), "SecondaryLabel");

        public void MutedLabel(string text, params GUILayoutOption[] options) => Execute(() => Get<Label>()?.MutedLabel(text, options), "MutedLabel");

        public void DestructiveLabel(string text, params GUILayoutOption[] options) => Execute(() => Get<Label>()?.DestructiveLabel(text, options), "DestructiveLabel");

        public void Label(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] options)
        {
            Execute(
                () =>
                    Get<Label>()
                        ?.DrawLabel(
                            new LabelConfig
                            {
                                Text = text,
                                Icon = icon,
                                Variant = variant,
                                Disabled = disabled,
                                Options = options,
                            }
                        ),
                "Label"
            );
        }

        #endregion

        #region Progress Components

        public void Progress(ProgressConfig config) => Execute(() => Get<Progress>()?.DrawProgress(config), "Progress");

        public void Progress(float value, float width = -1, float height = -1, params GUILayoutOption[] options) => Execute(() => Get<Progress>()?.DrawProgress(value, width, height, options), "Progress");

        public void Progress(Rect rect, float value) => Execute(() => Get<Progress>()?.DrawProgress(rect, value), "Progress");

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options) => Execute(() => Get<Progress>()?.LabeledProgress(label, value, width, height, showPercentage, options), "LabeledProgress");

        public void AnimatedProgress(string id, float value, float width = -1, float height = -1, params GUILayoutOption[] options) => Execute(() => Get<Progress>()?.AnimatedProgress(id, value, width, height, options), "AnimatedProgress");

        public void CircularProgress(float value, float size = 32f, params GUILayoutOption[] options) => Execute(() => Get<Progress>()?.CircularProgress(value, size, options), "CircularProgress");

        #endregion

        #region Badge Components

        public void Badge(BadgeConfig config) => Execute(() => Get<Badge>()?.DrawBadge(config), "Badge");

        public void Badge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.DrawBadge(text, variant, size, options), "Badge");

        public void Badge(Rect rect, string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => Execute(() => Get<Badge>()?.DrawBadge(rect, text, variant, size), "Badge");

        public void BadgeWithIcon(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.BadgeWithIcon(text, icon, variant, size, options), "BadgeWithIcon");

        public void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.CountBadge(count, variant, size, maxCount, options), "CountBadge");

        public void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.StatusBadge(text, isActive, variant, size, options), "StatusBadge");

        public void ProgressBadge(string text, float progress, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.ProgressBadge(text, progress, variant, size, options), "ProgressBadge");

        public void AnimatedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.AnimatedBadge(text, variant, size, options), "AnimatedBadge");

        public void AnimatedBadge(string text, string animationId, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.AnimatedBadge(text, animationId, variant, size, options), "AnimatedBadge");

        public void RoundedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, float cornerRadius = 12f, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.RoundedBadge(text, variant, size, cornerRadius, options), "RoundedBadge");

        public void PillBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Badge>()?.PillBadge(text, variant, size, options), "PillBadge");

        public void Badge(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            Execute(
                () =>
                    Get<Badge>()
                        ?.DrawBadge(
                            new BadgeConfig
                            {
                                Text = text,
                                Icon = icon,
                                Variant = variant,
                                Size = size,
                                Options = options,
                            }
                        ),
                "Badge"
            );
        }

        #endregion

        #region Avatar Components

        public void Avatar(AvatarConfig config) => Execute(() => Get<Display.Avatar>()?.DrawAvatar(config), "Avatar");

        public void Avatar(Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) => Execute(() => Get<Display.Avatar>()?.DrawAvatar(image, fallbackText, size, shape, options), "Avatar");

        public void Avatar(Rect rect, Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle) => Execute(() => Get<Display.Avatar>()?.DrawAvatar(rect, image, fallbackText, size, shape), "Avatar");

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            Execute(() => Get<Display.Avatar>()?.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options), "AvatarWithStatus");

        public void AvatarWithName(Texture2D image, string fallbackText, string name, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options) =>
            Execute(() => Get<Display.Avatar>()?.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options), "AvatarWithName");

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            Execute(() => Get<Display.Avatar>()?.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options), "AvatarWithBorder");

        #endregion

        #region Dialog Components

        public void Dialog(DialogConfig config) => Execute(() => Get<Dialog>()?.DrawDialog(config), "Dialog");

        public void Dialog(string dialogId, Action content, float width = 400, float height = 300) => Execute(() => Get<Dialog>()?.DrawDialog(dialogId, content, width, height), "Dialog");

        public void Dialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300) => Execute(() => Get<Dialog>()?.DrawDialog(dialogId, title, description, content, footer, width, height), "Dialog");

        public void OpenDialog(string dialogId) => Get<Dialog>()?.Open(dialogId);

        public void CloseDialog() => Get<Dialog>()?.Close();

        public bool IsDialogOpen() => Get<Dialog>()?.IsOpen ?? false;

        public bool DialogTrigger(string label, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            try
            {
                return Get<Dialog>()?.DrawDialogTrigger(label, variant, size) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DialogTrigger", "GUIHelper");
                return false;
            }
        }

        public void DialogHeader(string title, string description = null) => Execute(() => Get<Dialog>()?.DrawDialogHeader(title, description), "DialogHeader");

        public void DialogContent(Action content) => Execute(() => Get<Dialog>()?.DrawDialogContent(content), "DialogContent");

        public void DialogFooter(Action footer) => Execute(() => Get<Dialog>()?.DrawDialogFooter(footer), "DialogFooter");

        #endregion

        #region Popover Components

        public void Popover(PopoverConfig config) => Execute(() => Get<Popover>()?.DrawPopover(config), "Popover");

        public void Popover(Action content) => Execute(() => Get<Popover>()?.DrawPopover(content), "Popover");

        public void OpenPopover() => Get<Popover>()?.Open();

        public void ClosePopover() => Get<Popover>()?.Close();

        public bool IsPopoverOpen() => Get<Popover>()?.IsOpen ?? false;

        #endregion

        #region Toast Components

        public void ShowToast(ToastConfig config) => Execute(() => Get<Toast>()?.Show(config), "ShowToast");

        public string ShowToast(string title, string description = "", ToastVariant variant = ToastVariant.Default)
        {
            return Execute(
                () =>
                {
                    var config = new ToastConfig
                    {
                        Title = title,
                        Description = description,
                        Variant = variant,
                    };
                    Get<Toast>()?.Show(config);
                    return config.Id;
                },
                "ShowToast"
            );
        }

        public string ShowSuccessToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Success);

        public string ShowErrorToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Error);

        public string ShowWarningToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Warning);

        public string ShowInfoToast(string title, string description = "") => ShowToast(title, description, ToastVariant.Info);

        public void DismissToast(string id) => Execute(() => Get<Toast>()?.Dismiss(id), "DismissToast");

        public void DismissAllToasts() => Execute(() => Get<Toast>()?.DismissAll(), "DismissAllToasts");

        public void PauseToast(string id) => Execute(() => Get<Toast>()?.PauseToast(id), "PauseToast");

        public void ResumeToast(string id) => Execute(() => Get<Toast>()?.ResumeToast(id), "ResumeToast");

        public void DrawOverlay()
        {
            Execute(() => Get<Toast>()?.DrawToasts(), "Toasts");
            Execute(
                () =>
                {
                    Rect clipBounds = new Rect(0, 0, Screen.width, Screen.height);
                    Get<Tooltip>()?.FlushAndDraw(clipBounds);
                },
                "Tooltip"
            );
            Execute(() => LayerManager.Instance.DrawLayers(), "DrawLayers");
        }

        public int GetActiveToastCount() => Execute(() => Get<Toast>()?.GetActiveToastCount() ?? 0, "GetActiveToastCount");

        public bool HasToast(string id) => Execute(() => Get<Toast>()?.HasToast(id) ?? false, "HasToast");

        public List<string> GetActiveToastIds() => Execute(() => Get<Toast>()?.GetActiveToastIds() ?? new List<string>(), "GetActiveToastIds");

        public void SetMaxConcurrentToasts(int max)
        {
            Execute(
                () =>
                {
                    var t = Get<Toast>();
                    if (t != null)
                        t.MaxConcurrentToasts = max;
                },
                "SetMaxConcurrentToasts"
            );
        }

        public void SetDismissAnimationDuration(float duration)
        {
            Execute(
                () =>
                {
                    var t = Get<Toast>();
                    if (t != null)
                        t.GlobalDismissAnimationDuration = duration;
                },
                "SetDismissAnimationDuration"
            );
        }

        public void EnablePauseOnHover(bool enable)
        {
            Execute(
                () =>
                {
                    var t = Get<Toast>();
                    if (t != null)
                        t.GlobalEnablePauseOnHover = enable;
                },
                "EnablePauseOnHover"
            );
        }

        public void EnableGrouping(bool enable, int timeWindowMs = 500)
        {
            Execute(
                () =>
                {
                    var t = Get<Toast>();
                    if (t != null)
                    {
                        t.EnableGlobalGrouping = enable;
                        t.GroupingTimeWindowMs = timeWindowMs;
                    }
                },
                "EnableGrouping"
            );
        }

        #endregion

        #region Tooltip Components

        public void WithTooltip(string text, Action draw) => Execute(() => Get<Tooltip>()?.WithTooltip(text, draw), "WithTooltip");

        public void WithTooltip(string text, TooltipConfig config, Action draw) => Execute(() => Get<Tooltip>()?.WithTooltip(text, config, draw), "WithTooltip");

        public T WithTooltip<T>(string text, Func<T> draw) => Execute(() => Get<Tooltip>() != null ? Get<Tooltip>().WithTooltip(text, draw) : (draw != null ? draw() : default), "WithTooltip");

        public T WithTooltip<T>(string text, TooltipConfig config, Func<T> draw) => Execute(() => Get<Tooltip>() != null ? Get<Tooltip>().WithTooltip(text, config, draw) : (draw != null ? draw() : default), "WithTooltip");

        #endregion

        #region Separator Components

        public void Separator(SeparatorConfig config) => Execute(() => Get<Separator>()?.DrawSeparator(config), "Separator");

        public void Separator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options) => Execute(() => Get<Separator>()?.DrawSeparator(orientation, decorative, options), "Separator");

        public void HorizontalSeparator(params GUILayoutOption[] options) => Execute(() => Get<Separator>()?.HorizontalSeparator(options), "HorizontalSeparator");

        public void VerticalSeparator(params GUILayoutOption[] options) => Execute(() => Get<Separator>()?.VerticalSeparator(options), "VerticalSeparator");

        public void Separator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal) => Execute(() => Get<Separator>()?.DrawSeparator(rect, orientation), "Separator");

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, float spacingBefore = 8f, float spacingAfter = 8f, params GUILayoutOption[] options) =>
            Execute(() => Get<Separator>()?.SeparatorWithSpacing(orientation, spacingBefore, spacingAfter, options), "SeparatorWithSpacing");

        public void LabeledSeparator(string text, params GUILayoutOption[] options) => Execute(() => Get<Separator>()?.LabeledSeparator(text, options), "LabeledSeparator");

        #endregion

        #region Table Components

        public void Table(TableConfig config) => Execute(() => Get<Table>()?.DrawTable(config), "Table");

        public void Table(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => Get<Table>()?.DrawTable(headers, data, variant, size, options), "Table");

        public void Table(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => Execute(() => Get<Table>()?.DrawTable(rect, headers, data, variant, size), "Table");

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            try
            {
                Get<Table>()?.SortableTable(headers, data, ref sortColumns, ref sortAscending, variant, size, onSort, options);
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
                Get<Table>()?.SelectableTable(headers, data, ref selectedRows, variant, size, onSelectionChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SelectableTable", "GUIHelper");
            }
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            Execute(() => Get<Table>()?.CustomTable(headers, data, cellRenderer, variant, size, options), "CustomTable");
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            try
            {
                Get<Table>()?.PaginatedTable(headers, data, ref currentPage, pageSize, variant, size, onPageChange, options);
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
                Get<Table>()?.SearchableTable(headers, data, ref searchQuery, ref filteredData, variant, size, onSearch, options);
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
                Get<Table>()?.ResizableTable(headers, data, ref columnWidths, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResizableTable", "GUIHelper");
            }
        }

        #endregion

        #region Tabs Components

        public int Tabs(TabsConfig config) => Execute(() => Get<Tabs>()?.Draw(config) ?? config.SelectedIndex, "Tabs");

        public int Tabs(string[] tabNames, int selectedIndex, Action<int> onTabChange, int maxLines = 1, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                {
                    var config = new TabsConfig(tabNames, selectedIndex)
                    {
                        OnTabChange = onTabChange,
                        MaxLines = maxLines,
                        Options = options,
                    };
                    return Get<Tabs>()?.Draw(config) ?? selectedIndex;
                },
                "Tabs"
            );
        }

        public int Tabs(string[] tabNames, int selectedIndex, Action content, int maxLines = 1, TabPosition position = TabPosition.Top, params GUILayoutOption[] options)
        {
            return Execute(
                () =>
                {
                    var config = new TabsConfig(tabNames, selectedIndex)
                    {
                        Content = content,
                        MaxLines = maxLines,
                        Position = position,
                        Options = options,
                    };
                    return Get<Tabs>()?.Draw(config) ?? selectedIndex;
                },
                "Tabs"
            );
        }

        public void BeginTabContent() => Execute(() => Get<Tabs>()?.BeginTabContent(), "BeginTabContent");

        public void EndTabContent() => Execute(() => Get<Tabs>()?.EndTabContent(), "EndTabContent");

        public int TabsWithContent(TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null, params GUILayoutOption[] options)
        {
            return Execute(
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

                    var newIndex = Get<Tabs>()?.Draw(config) ?? selectedIndex;
                    if (newIndex >= 0 && newIndex < tabConfigs.Length)
                    {
                        BeginTabContent();
                        tabConfigs[newIndex].Content?.Invoke();
                        EndTabContent();
                    }

                    return newIndex;
                },
                "TabsWithContent"
            );
        }

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action content, float tabWidth = 120f, int maxLines = 1, TabSide side = TabSide.Left, params GUILayoutOption[] options)
        {
            return Execute(
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
                    return Get<Tabs>()?.Draw(config) ?? selectedIndex;
                },
                "VerticalTabs"
            );
        }

        public int ClosableTabs(ref string[] tabNames, ref bool[] closableTabs, int selectedIndex, Action content = null, Action<int> onTabChange = null)
        {
            try
            {
                return Get<Tabs>()?.DrawWithAutoClose(ref tabNames, ref closableTabs, selectedIndex, content, onTabChange) ?? selectedIndex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ClosableTabs", "GUIHelper");
                return selectedIndex;
            }
        }

        #endregion

        #region MenuBar Components

        public void MenuBar(MenuBar.MenuBarConfig config) => Execute(() => Get<MenuBar>()?.Draw(config), "MenuBar");

        public void MenuBar(List<MenuBar.MenuItem> items, params GUILayoutOption[] options)
        {
            try
            {
                Get<MenuBar>()?.Draw(new MenuBar.MenuBarConfig(items) { Options = options });
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "MenuBar", "GUIHelper");
            }
        }

        #endregion

        #region Navigation Components

        public int Navigation(NavigationConfig config) => Execute(() => Get<Navigation>()?.Draw(config) ?? config?.SelectedIndex ?? 0, "Navigation");

        public int Sidebar(string[] labels, int selectedIndex, string[] icons = null, string logoText = "U", Action<int> onSelectionChanged = null, float width = 70f)
        {
            return Execute(() => Get<Navigation>()?.DrawSidebar(labels, selectedIndex, icons, logoText, onSelectionChanged, width) ?? selectedIndex, "Sidebar");
        }

        #endregion

        #region ThemeChanger Components

        public void ThemeChanger(ThemeChangerConfig config = null) => Execute(() => Get<ThemeChanger>()?.Draw(config), "ThemeChanger");

        public void ThemeChangerCompact(string id = "theme_compact") => Execute(() => Get<ThemeChanger>()?.DrawCompact(id), "ThemeChangerCompact");

        public void ThemeChangerWithPreview(string id = "theme_preview", float width = 220f) => Execute(() => Get<ThemeChanger>()?.DrawWithPreview(id, width), "ThemeChangerWithPreview");

        #endregion

        #region Chart Components

        public void Chart(ChartConfig config) => Execute(() => Get<Chart>()?.DrawChart(config), "Chart");

        public void Chart(List<ChartSeries> chartSeries, ChartType chartType, Vector2 size, params GUILayoutOption[] options) => Execute(() => Get<Chart>()?.DrawChart(new ChartConfig(chartSeries, chartType) { Size = size, Options = options }), "Chart");

        #endregion

        #region TextArea Components

        public string TextArea(TextAreaConfig config) => Execute(() => Get<TextArea>()?.DrawTextArea(config) ?? config.Text, "TextArea");

        public string TextArea(string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return Execute(() => Get<TextArea>()?.DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options) ?? text, "TextArea");
        }

        public string TextArea(Rect rect, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1)
        {
            return Execute(() => Get<TextArea>()?.DrawTextArea(rect, text, variant, placeholder, disabled, maxLength) ?? text, "TextArea");
        }

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return Execute(() => Get<TextArea>()?.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text, "OutlineTextArea");
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return Execute(() => Get<TextArea>()?.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text, "GhostTextArea");
        }

        public string LabeledTextArea(string label, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options)
        {
            return Execute(() => Get<TextArea>()?.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight, maxLength, showCharCount, options) ?? text, "LabeledTextArea");
        }

        public string ResizableTextArea(string text, ref float height, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return Get<TextArea>()?.ResizableTextArea(text, ref height, variant, placeholder, disabled, minHeight, maxHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResizableTextArea", "GUIHelper");
                return text;
            }
        }

        #endregion

        #region Helper Methods

        private void Execute(Action action, string methodName)
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

        private T Execute<T>(Func<T> func, string methodName)
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
        private readonly Dictionary<Type, BaseComponent> _components;

        public ComponentResolver(Dictionary<Type, BaseComponent> components)
        {
            _components = components;
        }

        public T GetComponent<T>()
            where T : BaseComponent
        {
            if (_components.TryGetValue(typeof(T), out BaseComponent component))
                return (T)component;
            GUILogger.LogWarning($"Component of type {typeof(T).Name} not found.", "ComponentResolver.GetComponent<T>");
            return null;
        }
    }

    #endregion
}
