using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
using AvatarComponent = shadcnui.GUIComponents.Display.Avatar;
using InputComponent = shadcnui.GUIComponents.Controls.Input;

namespace shadcnui.GUIComponents.Core.Base
{
    public partial class GUIHelper : IDisposable
    {
        private readonly StyleManager _styleManager;
        private readonly AnimationManager _animationManager;
        private readonly Layout.Layout _layout;
        private readonly List<BaseComponent> _components;

        private readonly Button _button;
        private readonly InputComponent _input;
        private readonly Checkbox _checkbox;
        private readonly Switch _switch;
        private readonly Toggle _toggle;
        private readonly Slider _slider;
        private readonly Select _select;
        private readonly DropdownMenu _dropdownMenu;
        private readonly ThemeChanger _themeChanger;
        private readonly TextArea _textArea;
        private readonly Calendar _calendar;
        private readonly DatePicker _datePicker;
        private readonly DataTable _dataTable;

        private readonly Label _label;
        private readonly Badge _badge;
        private readonly AvatarComponent _avatar;
        private readonly Progress _progress;
        private readonly Chart _chart;
        private readonly Dialog _dialog;
        private readonly Popover _popover;
        private readonly Toast _toast;
        private readonly Tooltip _tooltip;

        private readonly Card _card;
        private readonly Separator _separator;
        private readonly Tabs _tabs;
        private readonly MenuBar _menuBar;
        private readonly Table _table;
        private readonly Navigation _navigation;

        internal int fontSize = 14;
        public float uiScale = 1f;

        public GUIHelper()
        {
            _styleManager = new StyleManager(this);
            _animationManager = new AnimationManager(this);
            _layout = new Layout.Layout(this);

            _button = new Button(this);
            _input = new InputComponent(this);
            _checkbox = new Checkbox(this);
            _switch = new Switch(this);
            _toggle = new Toggle(this);
            _slider = new Slider(this);
            _select = new Select(this);
            _dropdownMenu = new DropdownMenu(this);
            _themeChanger = new ThemeChanger(this);
            _textArea = new TextArea(this);
            _calendar = new Calendar(this);
            _datePicker = new DatePicker(this);
            _dataTable = new DataTable(this);

            _label = new Label(this);
            _badge = new Badge(this);
            _avatar = new AvatarComponent(this);
            _progress = new Progress(this);
            _chart = new Chart(this);
            _dialog = new Dialog(this);
            _popover = new Popover(this);
            _toast = new Toast(this);
            _tooltip = new Tooltip(this);

            _card = new Card(this);
            _separator = new Separator(this);
            _tabs = new Tabs(this);
            _menuBar = new MenuBar(this);
            _table = new Table(this);
            _navigation = new Navigation(this);

            _components = new List<BaseComponent>
            {
                _button,
                _input,
                _checkbox,
                _switch,
                _toggle,
                _slider,
                _select,
                _dropdownMenu,
                _themeChanger,
                _textArea,
                _calendar,
                _datePicker,
                _dataTable,
                _label,
                _badge,
                _avatar,
                _progress,
                _chart,
                _dialog,
                _popover,
                _toast,
                _tooltip,
                _card,
                _separator,
                _tabs,
                _menuBar,
                _table,
                _navigation,
            };
        }

        public StyleManager GetStyleManager() => _styleManager;

        public AnimationManager GetAnimationManager() => _animationManager;

        public ThemeManager GetThemeManager() => ThemeManager.Instance;

        public Theme CurrentTheme => ThemeManager.Instance.CurrentTheme;

        public Chart GetChartComponent() => _chart;

        public Chart GetChartComponents() => _chart;

        public void SetTheme(string themeName)
        {
            if (ThemeManager.Instance.SetTheme(themeName))
                _styleManager.MarkStylesCorruption();
        }

        public void RegisterTheme(Theme theme)
        {
            ThemeManager.Instance.AddTheme(theme);
            _styleManager.MarkStylesCorruption();
        }

        public void SetUiScale(float scale)
        {
            var next = Mathf.Max(0.5f, scale);
            if (Mathf.Abs(uiScale - next) < 0.001f)
                return;

            uiScale = next;
            _styleManager.MarkStylesCorruption();
        }

        public void SetFontSize(int size)
        {
            var next = Mathf.Max(8, size);
            if (fontSize == next)
                return;

            fontSize = next;
            _styleManager.MarkStylesCorruption();
        }

        public void SetCustomFont(Font font)
        {
            if (_styleManager.CustomFont == font)
                return;

            _styleManager.CustomFont = font;
            _styleManager.MarkStylesCorruption();
        }

        public void UpdateGUI(bool isOpen) { }

        private bool _scrollbarsInitialized;
        private int _lastCheckFrame = -10;

        public bool BeginGUI()
        {
            return Execute(
                () =>
                {
                    _styleManager.InitializeGUI();
                    
                    if (Time.frameCount - _lastCheckFrame >= 60)
                    {
                        _lastCheckFrame = Time.frameCount;
                        if (_styleManager.ScanForCorruption())
                            _styleManager.MarkStylesCorruption();
                        _styleManager.RefreshStylesIfCorruption();
                    }
                    
                    if (!_scrollbarsInitialized)
                    {
                        GUI.skin.horizontalScrollbar = GUIStyle.none;
                        GUI.skin.verticalScrollbar = GUIStyle.none;
                        _scrollbarsInitialized = true;
                    }
                    
                    return _animationManager.BeginGUI();
                },
                true,
                nameof(BeginGUI)
            );
        }

        public void EndGUI() => Execute(_animationManager.EndGUI, nameof(EndGUI));

        public void DrawOverlays()
        {
            Execute(
                () =>
                {
                    LayerManager.Instance.DrawLayers();
                    _toast.DrawToasts();
                    _tooltip.FlushAndDraw(new Rect(0f, 0f, Screen.width, Screen.height));
                },
                nameof(DrawOverlays)
            );
        }

        public void DrawOverlay() => DrawOverlays();

        public void Cleanup()
        {
            foreach (var component in _components)
                component.Dispose();

            _toast.Cleanup();
            LayerManager.Instance.CloseAll();
            _styleManager.Cleanup();
            _animationManager.Cleanup();
        }

        public void Dispose() => Cleanup();

        public Vector2 ScrollView(Vector2 scrollPosition, Action drawContent, params GUILayoutOption[] options)
        {
            return Execute(() => _layout.DrawScrollView(scrollPosition, drawContent, options), scrollPosition, nameof(ScrollView));
        }

        public void BeginHorizontalGroup() => Execute(_layout.BeginHorizontalGroup, nameof(BeginHorizontalGroup));

        public void BeginHorizontalGroup(params GUILayoutOption[] options) => Execute(() => _layout.BeginHorizontalGroup(options), nameof(BeginHorizontalGroup));

        public void BeginHorizontalGroup(GUIStyle style, params GUILayoutOption[] options) => Execute(() => _layout.BeginHorizontalGroup(style, options), nameof(BeginHorizontalGroup));

        public void EndHorizontalGroup() => Execute(_layout.EndHorizontalGroup, nameof(EndHorizontalGroup));

        public void BeginVerticalGroup() => Execute(_layout.BeginVerticalGroup, nameof(BeginVerticalGroup));

        public void BeginVerticalGroup(params GUILayoutOption[] options) => Execute(() => _layout.BeginVerticalGroup(options), nameof(BeginVerticalGroup));

        public void BeginVerticalGroup(GUIStyle style, params GUILayoutOption[] options) => Execute(() => _layout.BeginVerticalGroup(style, options), nameof(BeginVerticalGroup));

        public void EndVerticalGroup() => Execute(_layout.EndVerticalGroup, nameof(EndVerticalGroup));

        public void AddSpace(float pixels) => Execute(() => _layout.AddSpace(pixels), nameof(AddSpace));

        public bool Button(ButtonConfig config) => Execute(() => _button.DrawButton(config), false, nameof(Button));

        public bool Button(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            Execute(() => _button.DrawButton(text, variant, size, onClick, disabled, opacity, options), false, nameof(Button));

        public bool Button(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            Execute(() => _button.DrawButton(text, icon, variant, size, onClick, disabled, opacity, options), false, nameof(Button));

        public bool Button(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            Button(
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
            );

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f) => Execute(() => _button.ButtonGroup(drawButtons, horizontal, spacing), nameof(ButtonGroup));

        public string Input(InputConfig config) => Execute(() => _input.DrawInput(config), config?.Value ?? string.Empty, nameof(Input));

        public string Input(string value, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            Execute(() => _input.DrawInput(value, placeholder, variant, disabled, focused, width, onChange), value ?? string.Empty, nameof(Input));

        public string Input(string value, Texture2D icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            Execute(() => _input.DrawInput(value, icon, placeholder, variant, disabled, focused, width, onChange), value ?? string.Empty, nameof(Input));

        public string Input(string value, IconConfig icon, string placeholder = "", ControlVariant variant = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            Input(
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
            );

        public string LabeledInput(InputConfig config) => Execute(() => _input.DrawLabeledInput(config), config?.Value ?? string.Empty, nameof(LabeledInput));

        public string LabeledInput(string label, string value, string placeholder = "", ControlVariant inputVariant = ControlVariant.Default, ControlVariant labelVariant = ControlVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null) =>
            Execute(() => _input.DrawLabeledInput(label, value, placeholder, inputVariant, labelVariant, disabled, inputWidth, onChange), value ?? string.Empty, nameof(LabeledInput));

        public string PasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            try
            {
                return _input.DrawPasswordField(windowWidth, label, ref password, maskChar);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(PasswordField), nameof(GUIHelper));
                return password ?? string.Empty;
            }
        }

        public void MultilineInput(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            try
            {
                _input.DrawTextArea(windowWidth, label, ref text, maxLength, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(MultilineInput), nameof(GUIHelper));
            }
        }

        public void SectionHeader(string title) => Execute(() => _input.DrawSectionHeader(title), nameof(SectionHeader));

        public void InputLabel(string text, int width = -1) => Execute(() => _input.DrawLabel(text, ControlVariant.Default, width), nameof(InputLabel));

        public bool Toggle(ToggleConfig config) => Execute(() => _toggle.DrawToggle(config), config?.Value ?? false, nameof(Toggle));

        public bool Toggle(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            Execute(() => _toggle.DrawToggle(text, value, variant, size, onToggle, disabled, options), value, nameof(Toggle));

        public bool Toggle(string text, IconConfig icon, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            Toggle(
                new ToggleConfig
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
            );

        public bool Checkbox(CheckboxConfig config) => Execute(() => _checkbox.DrawCheckbox(config), config?.Value ?? false, nameof(Checkbox));

        public bool Checkbox(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            Execute(() => _checkbox.DrawCheckbox(text, value, variant, size, onToggle, disabled, options), value, nameof(Checkbox));

        public bool Checkbox(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            Execute(() => _checkbox.DrawCheckbox(rect, text, value, variant, size, onToggle, disabled), value, nameof(Checkbox));

        public bool Switch(SwitchConfig config) => Execute(() => _switch.DrawSwitch(config), config?.Value ?? false, nameof(Switch));

        public bool Switch(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            Execute(() => _switch.DrawSwitch(text, value, variant, size, onToggle, disabled, options), value, nameof(Switch));

        public float Slider(SliderConfig config) => Execute(() => _slider.Draw(config), config?.Value ?? 0f, nameof(Slider));

        public float Slider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options) => Execute(() => _slider.Draw(value, min, max, options), value, nameof(Slider));

        public float Slider(float value, float min, float max, float step, params GUILayoutOption[] options) => Execute(() => _slider.Draw(value, min, max, step, options), value, nameof(Slider));

        public float LabeledSlider(string label, float value, float min, float max, bool showValue = true, params GUILayoutOption[] options) => Execute(() => _slider.LabeledSlider(label, value, min, max, showValue, options), value, nameof(LabeledSlider));

        public float LabeledSlider(string label, float value, float min, float max, float step, bool showValue = true, params GUILayoutOption[] options) => Execute(() => _slider.LabeledSlider(label, value, min, max, step, showValue, options), value, nameof(LabeledSlider));

        public float DisabledSlider(float value, float min = 0f, float max = 1f, params GUILayoutOption[] options) => Execute(() => _slider.DisabledSlider(value, min, max, options), value, nameof(DisabledSlider));

        public int Select(SelectConfig config) => Execute(() => _select.DrawSelect(config), config?.SelectedIndex ?? 0, nameof(Select));

        public int Select(string[] items, int selectedIndex) => Execute(() => _select.DrawSelect(items, selectedIndex), selectedIndex, nameof(Select));

        public void OpenSelect(string id = "select") => Execute(() => _select.Open(id), nameof(OpenSelect));

        public void CloseSelect() => Execute(_select.Close, nameof(CloseSelect));

        public bool IsSelectOpen() => _select.IsOpen;

        public void DropdownMenu(DropdownMenuConfig config) => Execute(() => _dropdownMenu.Draw(config), nameof(DropdownMenu));

        public void OpenDropdownMenu(List<DropdownMenuItem> items, string id = "dropdown", int zIndex = -1) => Execute(() => _dropdownMenu.Open(items, id, zIndex), nameof(OpenDropdownMenu));

        public void CloseDropdownMenu() => Execute(_dropdownMenu.Close, nameof(CloseDropdownMenu));

        public void ThemeChanger(ThemeChangerConfig config = null) => Execute(() => _themeChanger.Draw(config), nameof(ThemeChanger));

        public void ThemeChangerCompact(string id = "theme_compact") => Execute(() => _themeChanger.DrawCompact(id), nameof(ThemeChangerCompact));

        public void ThemeChangerWithPreview(string id = "theme_preview", float width = 220f) => Execute(() => _themeChanger.DrawWithPreview(id, width), nameof(ThemeChangerWithPreview));

        public string TextArea(TextAreaConfig config) => Execute(() => _textArea.DrawTextArea(config), config?.Text ?? string.Empty, nameof(TextArea));

        public string TextArea(string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            Execute(() => _textArea.DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options), text ?? string.Empty, nameof(TextArea));

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            Execute(() => _textArea.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options), text ?? string.Empty, nameof(OutlineTextArea));

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            Execute(() => _textArea.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options), text ?? string.Empty, nameof(GhostTextArea));

        public string LabeledTextArea(string label, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options) =>
            Execute(() => _textArea.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight, maxLength, showCharCount, options), text ?? string.Empty, nameof(LabeledTextArea));

        public string ResizableTextArea(string text, ref float height, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return _textArea.ResizableTextArea(text, ref height, variant, placeholder, disabled, minHeight, maxHeight, maxLength, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(ResizableTextArea), nameof(GUIHelper));
                return text ?? string.Empty;
            }
        }

        public void Label(LabelConfig config) => Execute(() => _label.DrawLabel(config), nameof(Label));

        public void Label(string text, ControlVariant variant = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] options) => Execute(() => _label.DrawLabel(text, variant, disabled, options), nameof(Label));

        public void MutedLabel(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Muted, false, options);

        public void SecondaryLabel(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Secondary, false, options);

        public void DestructiveLabel(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Destructive, false, options);

        public void Badge(BadgeConfig config) => Execute(() => _badge.DrawBadge(config), nameof(Badge));

        public void Badge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => _badge.DrawBadge(text, variant, size, options), nameof(Badge));

        public void Badge(string text, IconConfig icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            Badge(
                new BadgeConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = variant,
                    Size = size,
                    Options = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void BadgeWithIcon(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => _badge.BadgeWithIcon(text, icon, variant, size, options), nameof(BadgeWithIcon));

        public void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options) => Execute(() => _badge.CountBadge(count, variant, size, maxCount, options), nameof(CountBadge));

        public void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => _badge.StatusBadge(text, isActive, variant, size, options), nameof(StatusBadge));

        public void ProgressBadge(string text, float progress, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => _badge.ProgressBadge(text, progress, variant, size, options), nameof(ProgressBadge));

        public void RoundedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, float cornerRadius = DesignTokens.Radius.XL, params GUILayoutOption[] options) =>
            Execute(() => _badge.RoundedBadge(text, variant, size, cornerRadius, options), nameof(RoundedBadge));

        public void AnimatedBadge(string text, string animationId = null, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    if (string.IsNullOrEmpty(animationId))
                        _badge.AnimatedBadge(text, variant, size, options);
                    else
                        _badge.AnimatedBadge(text, animationId, variant, size, options);
                },
                nameof(AnimatedBadge)
            );

        public void Avatar(AvatarConfig config) => Execute(() => _avatar.DrawAvatar(config), nameof(Avatar));

        public void Avatar(Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) => Execute(() => _avatar.DrawAvatar(image, fallbackText, size, shape, options), nameof(Avatar));

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            Execute(() => _avatar.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options), nameof(AvatarWithStatus));

        public void AvatarWithName(Texture2D image, string fallbackText, string name, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options) =>
            Execute(() => _avatar.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options), nameof(AvatarWithName));

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            Execute(() => _avatar.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options), nameof(AvatarWithBorder));

        public void Progress(ProgressConfig config) => Execute(() => _progress.DrawProgress(config), nameof(Progress));

        public void Progress(float value, float width = -1, float height = -1, params GUILayoutOption[] options) => Execute(() => _progress.DrawProgress(value, width, height, options), nameof(Progress));

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options) => Execute(() => _progress.LabeledProgress(label, value, width, height, showPercentage, options), nameof(LabeledProgress));

        public void CircularProgress(float value, float size = DesignTokens.Height.Small, params GUILayoutOption[] options) => Execute(() => _progress.CircularProgress(value, size, options), nameof(CircularProgress));

        public void AnimatedProgress(string id, float value, float width = -1, float height = -1, params GUILayoutOption[] options) => Execute(() => _progress.AnimatedProgress(id, value, width, height, options), nameof(AnimatedProgress));

        public void Chart(ChartConfig config) => Execute(() => _chart.DrawChart(config), nameof(Chart));

        public void OpenDialog(string dialogId) => Execute(() => _dialog.Open(dialogId), nameof(OpenDialog));

        public void CloseDialog() => Execute(_dialog.Close, nameof(CloseDialog));

        public void Dialog(DialogConfig config) => Execute(() => _dialog.DrawDialog(config), nameof(Dialog));

        public void Dialog(string dialogId, Action content, float width = 400f, float height = 300f) => Execute(() => _dialog.DrawDialog(dialogId, content, width, height), nameof(Dialog));

        public void Dialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400f, float height = 300f) => Execute(() => _dialog.DrawDialog(dialogId, title, description, content, footer, width, height), nameof(Dialog));

        public void OpenPopover(string id = "popover", int zIndex = -1) => Execute(() => _popover.Open(id, zIndex), nameof(OpenPopover));

        public void ClosePopover() => Execute(_popover.Close, nameof(ClosePopover));

        public bool IsPopoverOpen() => _popover.IsOpen;

        public void Popover(PopoverConfig config) => Execute(() => _popover.DrawPopover(config), nameof(Popover));

        public void Popover(Action content) => Execute(() => _popover.DrawPopover(content), nameof(Popover));

        public void WithTooltip(string text, Action draw) => Execute(() => _tooltip.WithTooltip(text, draw), nameof(WithTooltip));

        public void WithTooltip(string text, TooltipConfig config, Action draw) => Execute(() => _tooltip.WithTooltip(text, config, draw), nameof(WithTooltip));

        public T WithTooltip<T>(string text, Func<T> draw) => Execute(() => _tooltip.WithTooltip(text, draw), default(T), nameof(WithTooltip));

        public T WithTooltip<T>(string text, TooltipConfig config, Func<T> draw) => Execute(() => _tooltip.WithTooltip(text, config, draw), default(T), nameof(WithTooltip));

        public void ShowToast(ToastConfig config) => Execute(() => _toast.Show(config), nameof(ShowToast));

        public void ShowToast(string title, string description = null, ToastVariant variant = ToastVariant.Default, float durationMs = 4000f) =>
            ShowToast(
                new ToastConfig
                {
                    Title = title,
                    Description = description,
                    Variant = variant,
                    DurationMs = durationMs,
                }
            );

        public void ShowSuccessToast(string title, string description = null, float durationMs = 4000f) => ShowToast(title, description, ToastVariant.Success, durationMs);

        public void ShowErrorToast(string title, string description = null, float durationMs = 4000f) => ShowToast(title, description, ToastVariant.Error, durationMs);

        public void ShowWarningToast(string title, string description = null, float durationMs = 4000f) => ShowToast(title, description, ToastVariant.Warning, durationMs);

        public void ShowInfoToast(string title, string description = null, float durationMs = 4000f) => ShowToast(title, description, ToastVariant.Info, durationMs);

        public void DismissToast(string id, bool animate = true) => Execute(() => _toast.Dismiss(id, animate), nameof(DismissToast));

        public void DismissAllToasts(bool animate = true) => Execute(() => _toast.DismissAll(animate), nameof(DismissAllToasts));

        public int GetActiveToastCount() => _toast.GetActiveToastCount();

        public void Card(CardConfig config) => Execute(() => _card.DrawCard(config), nameof(Card));

        public void Card(string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) => Execute(() => _card.DrawCard(title, description, content, footerContent, width, height), nameof(Card));

        public void CardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) => Execute(() => _card.DrawCardWithImage(image, title, description, content, footerContent, width, height), nameof(CardWithImage));

        public void CardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1) => Execute(() => _card.DrawCardWithAvatar(avatar, title, subtitle, content, footerContent, width, height), nameof(CardWithAvatar));

        public void SimpleCard(string content, float width = -1, float height = -1) => Execute(() => _card.DrawSimpleCard(content, width, height), nameof(SimpleCard));

        public void BeginCard(float width = -1, float height = -1) => Execute(() => _card.BeginCard(width, height), nameof(BeginCard));

        public void EndCard() => Execute(_card.EndCard, nameof(EndCard));

        public void CardHeader(Action content) => Execute(() => _card.CardHeader(content), nameof(CardHeader));

        public void CardTitle(string title) => Execute(() => _card.CardTitle(title), nameof(CardTitle));

        public void CardDescription(string description) => Execute(() => _card.CardDescription(description), nameof(CardDescription));

        public void CardContent(Action content) => Execute(() => _card.CardContent(content), nameof(CardContent));

        public void CardFooter(Action content) => Execute(() => _card.CardFooter(content), nameof(CardFooter));

        public void Separator(SeparatorConfig config) => Execute(() => _separator.DrawSeparator(config), nameof(Separator));

        public void Separator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options) => Execute(() => _separator.DrawSeparator(orientation, decorative, options), nameof(Separator));

        public void HorizontalSeparator(params GUILayoutOption[] options) => Separator(SeparatorOrientation.Horizontal, true, options);

        public void VerticalSeparator(params GUILayoutOption[] options) => Separator(SeparatorOrientation.Vertical, true, options);

        public void LabeledSeparator(string text, params GUILayoutOption[] options) => Separator(new SeparatorConfig { Text = text, Options = options ?? Array.Empty<GUILayoutOption>() });

        public void SeparatorWithSpacing(SeparatorOrientation orientation, float spacingBefore, float spacingAfter, params GUILayoutOption[] options) =>
            Separator(
                new SeparatorConfig
                {
                    Orientation = orientation,
                    SpacingBefore = spacingBefore,
                    SpacingAfter = spacingAfter,
                    Options = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public int Tabs(TabsConfig config) => Execute(() => _tabs.Draw(config), config?.SelectedIndex ?? 0, nameof(Tabs));

        public int Tabs(string[] tabNames, int selectedIndex, Action content = null, Action<int> onTabChange = null, int maxLines = 1, TabPosition position = TabPosition.Top, IndicatorStyle indicatorStyle = IndicatorStyle.Underline, bool enableOverflowScroll = false) =>
            Tabs(
                new TabsConfig(tabNames, selectedIndex)
                {
                    Content = content,
                    OnTabChange = onTabChange,
                    MaxLines = maxLines,
                    Position = position,
                    IndicatorStyle = indicatorStyle,
                    EnableOverflowScroll = enableOverflowScroll,
                }
            );

        public int TabsWithContent(string[] tabNames, int selectedIndex, Action content, Action<int> onTabChange = null, int maxLines = 1) => Tabs(tabNames, selectedIndex, content, onTabChange, maxLines);

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action content = null, Action<int> onTabChange = null, float tabWidth = 140f, int maxLines = 1, TabSide side = TabSide.Left, IndicatorStyle indicatorStyle = IndicatorStyle.Background) =>
            Tabs(
                new TabsConfig(tabNames, selectedIndex)
                {
                    Content = content,
                    OnTabChange = onTabChange,
                    MaxLines = maxLines,
                    Position = side == TabSide.Right ? TabPosition.Right : TabPosition.Left,
                    TabWidth = tabWidth,
                    IndicatorStyle = indicatorStyle,
                }
            );

        public int ClosableTabs(ref string[] tabNames, ref bool[] closableTabs, int selectedIndex, Action content = null, Action<int> onTabChange = null)
        {
            try
            {
                return _tabs.DrawWithAutoClose(ref tabNames, ref closableTabs, selectedIndex, content, onTabChange);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(ClosableTabs), nameof(GUIHelper));
                return selectedIndex;
            }
        }

        public void BeginTabContent(params GUILayoutOption[] options) => Execute(() => _tabs.BeginTabContent(options), nameof(BeginTabContent));

        public void EndTabContent() => Execute(_tabs.EndTabContent, nameof(EndTabContent));

        public void MenuBar(MenuBar.MenuBarConfig config) => Execute(() => _menuBar.Draw(config), nameof(MenuBar));

        public void MenuBar(List<MenuBar.MenuItem> items, params GUILayoutOption[] options) => Execute(() => _menuBar.Draw(items, options), nameof(MenuBar));

        public void CloseMenuBarDropdown() => Execute(_menuBar.CloseDropdown, nameof(CloseMenuBarDropdown));

        public void Table(TableConfig config) => Execute(() => _table.DrawTable(config), nameof(Table));

        public void Table(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => Execute(() => _table.DrawTable(headers, data, variant, size, options), nameof(Table));

        public void CustomTable(TableConfig config) => Execute(() => _table.CustomTable(config), nameof(CustomTable));

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            Execute(() => _table.CustomTable(headers, data, cellRenderer, variant, size, options), nameof(CustomTable));

        public int Sidebar(string[] labels, int selectedIndex, string[] icons = null, string logoText = "U", Action<int> onSelectionChanged = null, float width = 70f) =>
            Execute(() => _navigation.DrawSidebar(labels, selectedIndex, icons, logoText, onSelectionChanged, width), selectedIndex, nameof(Sidebar));

        public int Navigation(NavigationConfig config) => Execute(() => _navigation.Draw(config), config?.SelectedIndex ?? 0, nameof(Navigation));

        public void Calendar(CalendarConfig config = null) => Execute(() => _calendar.DrawCalendar(config), nameof(Calendar));

        public DateTime? DatePicker(DatePickerConfig config) => Execute(() => _datePicker.DrawDatePicker(config), config?.SelectedDate, nameof(DatePicker));

        public DateTime? DatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options) => Execute(() => _datePicker.DrawDatePicker(placeholder, selectedDate, id, options), selectedDate, nameof(DatePicker));

        public DateTime? LabeledDatePicker(DatePickerConfig config) => Execute(() => _datePicker.DrawDatePickerWithLabel(config), config?.SelectedDate, nameof(LabeledDatePicker));

        public DateTime? LabeledDatePicker(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options) =>
            Execute(() => _datePicker.DrawDatePickerWithLabel(label, placeholder, selectedDate, id, options), selectedDate, nameof(LabeledDatePicker));

        public DateTime? DateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options) => Execute(() => _datePicker.DrawDateRangePicker(placeholder, startDate, endDate, id, options), startDate, nameof(DateRangePicker));

        public void CloseDatePicker(string id) => Execute(() => _datePicker.CloseDatePicker(id), nameof(CloseDatePicker));

        public void DataTable(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, params GUILayoutOption[] options) =>
            Execute(() => _dataTable.DrawDataTable(id, columns, data, showPagination, showSearch, showSelection, showColumnToggle, options), nameof(DataTable));

        private void Execute(Action action, string operation)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, operation, nameof(GUIHelper));
            }
        }

        private T Execute<T>(Func<T> action, T fallback, string operation)
        {
            try
            {
                return action != null ? action() : fallback;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, operation, nameof(GUIHelper));
                return fallback;
            }
        }
    }
}
