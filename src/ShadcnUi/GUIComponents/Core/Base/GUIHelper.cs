using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class GUIHelper : IDisposable
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
        private readonly RangeSlider _rangeSlider;
        private readonly Select _select;
        private readonly DropdownMenu _dropdownMenu;
        private readonly ThemeChanger _themeChanger;
        private readonly FontChanger _fontChanger;
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

        private readonly Dictionary<int, float> _floatState = new(),
            _floatInput = new();
        private readonly Dictionary<int, int> _intState = new(),
            _intInput = new();
        private readonly Dictionary<int, bool> _boolState = new(),
            _boolInput = new();
        private readonly Dictionary<int, string> _stringState = new(),
            _stringInput = new();
        private readonly Dictionary<int, Vector2> _v2State = new(),
            _v2Input = new();
        private readonly Dictionary<int, DateTime?> _dateState = new(),
            _dateInput = new();
        private readonly Stack<string> _stateScopes = new();

        private HashSet<string> _availableFonts;
        private bool _scrollbarsInitialized;
        private int _lastCheckFrame = -10;
        internal Rect _rootGuiScreenRect;
        internal bool _rootGuiScreenRectValid;
        internal int fontSize = 14;
        private IAutoRenderBuilder _pendingAutoRenderBuilder;
        private bool _flushingAutoRenderBuilder;

        public const string DefaultFontName = "Segoe UI";
        public float uiScale = 1f;
        private string _currentFontName = DefaultFontName;
        private Font _ownedDynamicFont;

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
            _rangeSlider = new RangeSlider(this);
            _select = new Select(this);
            _dropdownMenu = new DropdownMenu(this);
            _themeChanger = new ThemeChanger(this);
            _fontChanger = new FontChanger(this);
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
                _rangeSlider,
                _select,
                _dropdownMenu,
                _themeChanger,
                _fontChanger,
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

            ApplyDefaultFont();
        }

        public StyleManager GetStyleManager() => _styleManager;

        public AnimationManager GetAnimationManager() => _animationManager;

        public ThemeManager GetThemeManager() => ThemeManager.Instance;

        public Theme CurrentTheme => ThemeManager.Instance.CurrentTheme;

        public string CurrentFontName => _currentFontName;

        public void SetTheme(string name)
        {
            if (ThemeManager.Instance.SetTheme(name))
                _styleManager.MarkStylesCorruption();
        }

        public void RegisterTheme(Theme theme)
        {
            ThemeManager.Instance.AddTheme(theme);
            _styleManager.MarkStylesCorruption();
        }

        public void RegisterStyle(StyleComponentType type, string styleId, ComponentAppearance profile) => _styleManager.RegisterStyle(type, styleId, profile);

        public void RegisterStyle(StyleComponentType type, string styleId, StatefulStyleModifier modifier) => _styleManager.RegisterStyle(type, styleId, modifier);

        public bool UnregisterStyle(StyleComponentType type, string styleId) => _styleManager.UnregisterStyle(type, styleId);

        public void SetUiScale(float scale)
        {
            var normalized = Mathf.Max(0.5f, scale);
            if (Mathf.Abs(uiScale - normalized) > 0.001f)
            {
                uiScale = normalized;
                _styleManager.MarkStylesCorruption();
            }
        }

        public void SetFontSize(int size)
        {
            var normalized = Mathf.Max(8, size);
            if (fontSize != normalized)
            {
                fontSize = normalized;
                _styleManager.MarkStylesCorruption();
            }
        }

        public void SetCustomFont(Font font)
        {
            if (_styleManager.CustomFont != font)
            {
                _styleManager.CustomFont = font;
                _styleManager.MarkStylesCorruption();
            }
        }

        public void SetFont(string fontName)
        {
            if (string.IsNullOrWhiteSpace(fontName))
                return;

            fontName = NormalizeFontName(fontName);
            if (!TryCreateOsFont(fontName, out var font))
                return;

            _currentFontName = fontName;
            ReleaseOwnedDynamicFont();
            _ownedDynamicFont = font;
            SetCustomFont(font);
        }

        public void SetFont(Font font, string displayName = null)
        {
            if (font == null)
                return;

            if (font == _ownedDynamicFont)
                return;

            _currentFontName = string.IsNullOrWhiteSpace(displayName) ? font.name : displayName;
            ReleaseOwnedDynamicFont();
            SetCustomFont(font);
        }

        public string[] GetAvailableFonts()
        {
            EnsureFontsLoaded();
            return _availableFonts?.Select(NormalizeFontName).Where(static name => !string.IsNullOrWhiteSpace(name)).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(static name => name, StringComparer.OrdinalIgnoreCase).ToArray() ?? Array.Empty<string>();
        }

        public void UpdateGUI(bool isOpen) { }

        public bool BeginGUI() =>
            Execute(
                () =>
                {
                    _styleManager.InitializeGUI();
                    _rootGuiScreenRect = CaptureGuiScreenRect();
                    _rootGuiScreenRectValid = true;

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

        public void EndGUI() =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _animationManager.EndGUI();
                },
                nameof(EndGUI)
            );

        public void DrawOverlay() => DrawOverlays();

        public void WithTooltip(string text, Action draw) => Execute(() => _tooltip.WithTooltip(text, draw), nameof(WithTooltip));

        public void WithTooltip(string text, TooltipConfig config, Action draw) => Execute(() => _tooltip.WithTooltip(text, config, draw), nameof(WithTooltip));

        public T WithTooltip<T>(string text, Func<T> draw) => Execute(() => _tooltip.WithTooltip(text, draw), default, nameof(WithTooltip));

        public T WithTooltip<T>(string text, TooltipConfig config, Func<T> draw) => Execute(() => _tooltip.WithTooltip(text, config, draw), default, nameof(WithTooltip));

        public void DrawOverlays() =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();

                    var previousMatrix = GUI.matrix;
                    var previousColor = GUI.color;
                    var previousEnabled = GUI.enabled;

                    GUI.matrix = Matrix4x4.identity;
                    GUI.color = Color.white;
                    GUI.enabled = true;

                    try
                    {
                        LayerManager.Instance.DrawLayers();
                        _toast.DrawToasts();
                        _tooltip.FlushAndDraw(new Rect(0f, 0f, Screen.width, Screen.height));
                    }
                    finally
                    {
                        GUI.matrix = previousMatrix;
                        GUI.color = previousColor;
                        GUI.enabled = previousEnabled;
                    }
                },
                nameof(DrawOverlays)
            );

        public void Cleanup()
        {
            foreach (var component in _components)
                component.Dispose();

            _toast.Cleanup();
            LayerManager.Instance.CloseAll();
            _styleManager.Cleanup();
            _animationManager.Cleanup();
            _floatState.Clear();
            _floatInput.Clear();
            _intState.Clear();
            _intInput.Clear();
            _boolState.Clear();
            _boolInput.Clear();
            _stringState.Clear();
            _stringInput.Clear();
            _v2State.Clear();
            _v2Input.Clear();
            _dateState.Clear();
            _dateInput.Clear();
            _rootGuiScreenRectValid = false;
            _pendingAutoRenderBuilder = null;
            ReleaseOwnedDynamicFont();
        }

        public void Dispose() => Cleanup();

        public Vector2 ScrollView(Vector2 position, Action draw, params GUILayoutOption[] options)
        {
            FlushAutoRenderBuilder();

            string scopedKey = GetScopedStateKey(nameof(ScrollView));
            if (!string.IsNullOrEmpty(scopedKey))
            {
                int id = GetStateId(nameof(ScrollView), scopedKey);
                Vector2 seed = _v2State.TryGetValue(id, out var existing) ? existing : Vector2.zero;
                Vector2 result = Execute(() => _layout.DrawScrollView(seed, draw, options), seed, nameof(ScrollView));
                SetV2State(id, result);
                _v2Input[id] = result;
                return result;
            }

            return ExecStatefulV2(nameof(ScrollView), state => _layout.DrawScrollView(state, draw, options), position);
        }

        public Vector2 ScrollView(ref Vector2 position, Action draw, params GUILayoutOption[] options)
        {
            position = ScrollView(position, draw, options);
            return position;
        }

        public void Row(Action draw, params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    WrapHorizontal(draw, options);
                },
                nameof(Row)
            );

        public void Column(Action draw, params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    WrapVertical(draw, options);
                },
                nameof(Column)
            );

        public void BeginRow(params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.BeginHorizontalGroup(options);
                },
                nameof(BeginRow)
            );

        public void EndRow() =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.EndHorizontalGroup();
                },
                nameof(EndRow)
            );

        public void BeginColumn(params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.BeginVerticalGroup(options);
                },
                nameof(BeginColumn)
            );

        public void EndColumn() =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.EndVerticalGroup();
                },
                nameof(EndColumn)
            );

        public void BeginHorizontalGroup(params GUILayoutOption[] options) => BeginRow(options);

        public void BeginHorizontalGroup(GUIStyle style, params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.BeginHorizontalGroup(style, options);
                },
                nameof(BeginHorizontalGroup)
            );

        public void EndHorizontalGroup() => EndRow();

        public void BeginVerticalGroup(params GUILayoutOption[] options) => BeginColumn(options);

        public void BeginVerticalGroup(GUIStyle style, params GUILayoutOption[] options) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.BeginVerticalGroup(style, options);
                },
                nameof(BeginVerticalGroup)
            );

        public void EndVerticalGroup() => EndColumn();

        public void Space(float pixels) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _layout.AddSpace(pixels);
                },
                nameof(Space)
            );

        public void AddSpace(float pixels) => Space(pixels);

        public void Flex()
        {
            FlushAutoRenderBuilder();
            GUILayout.FlexibleSpace();
        }

        public ButtonBuilder Button(string text = "") => new(this, text);

        public bool Button(string text, Action onClick = null, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool disabled = false, float opacity = 1f, IconConfig icon = null, params GUILayoutOption[] options) =>
            DrawButton(text, onClick, variant, size, disabled, opacity, icon, options);

        public bool Button(string text, ControlVariant variant, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            DrawButton(text, variant, size, onClick, disabled, opacity, appearance, options);

        public InputBuilder Input(string value = "") => new(this, value);

        public string Input(string value, string placeholder = null, bool disabled = false, GUILayoutOption[] opts = null) =>
            Render(
                new InputConfig
                {
                    Value = value,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public CheckboxBuilder Checkbox(string label = "", bool value = false) => new(this, label, value);

        public bool Checkbox(string label, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onChange = null, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new CheckboxConfig
                {
                    Label = label,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnValueChanged = onChange,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public SwitchBuilder Switch(string label = "", bool value = false) => new(this, label, value);

        public bool Switch(string label, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onChange = null, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new SwitchConfig
                {
                    Label = label,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnValueChanged = onChange,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public ToggleBuilder Toggle(string label = "", bool value = false) => new(this, label, value);

        public bool Toggle(string label, bool value, ControlVariant variant, ControlSize size = ControlSize.Default, Action<bool> onChange = null, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            DrawToggle(label, value, variant, size, onChange, disabled, appearance, options);

        public SliderBuilder Slider(float value = 0f) => new(this, value);

        public float Slider(SliderConfig config) => Render(config);

        public RangeSliderBuilder RangeSlider(float lowerValue = 0f, float upperValue = 1f) => new(this, lowerValue, upperValue);

        public Vector2 RangeSlider(RangeSliderConfig config) => Render(config);

        public SelectBuilder Select() => new(this);

        public int Select(SelectConfig config) => Render(config);

        public DropdownMenuBuilder DropdownMenu() => new(this);

        public void DropdownMenu(DropdownMenuConfig config) => Render(config);

        public ThemeChangerBuilder ThemeChanger() => new(this);

        public void ThemeChanger(ThemeChangerConfig config) => Render(config);

        public FontChangerBuilder FontChanger() => new(this);

        public void FontChanger(FontChangerConfig config) => Render(config);

        public TextAreaBuilder TextArea(string value = "") => new(this, value);

        public string TextArea(TextAreaConfig config) => Render(config);

        public CalendarBuilder Calendar() => new(this);

        public DateTime? Calendar(CalendarConfig config) => Render(config);

        public DatePickerBuilder DatePicker() => new(this);

        public DateTime? DatePicker(DatePickerConfig config) => Render(config);

        public DataTableBuilder DataTable(string id) => new(this, id);

        public void DataTable(DataTableConfig config) => Render(config);

        public LabelBuilder Label(string text = "") => new(this, text);

        public void Label(string text, ControlVariant variant, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new LabelConfig
                {
                    Text = text,
                    Variant = variant,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public BadgeBuilder Badge(string text = "Badge") => new(this, text);

        public void Badge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options) =>
            Render(
                new BadgeConfig
                {
                    Count = count,
                    MaxCount = maxCount,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void CountBadge(int count, ControlVariant variant, ControlSize size, int maxCount, ComponentAppearance appearance, params GUILayoutOption[] options) =>
            Render(
                new BadgeConfig
                {
                    Count = count,
                    MaxCount = maxCount,
                    Variant = variant,
                    Size = size,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) =>
            Render(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    ShowStatusDot = true,
                    IsActive = isActive,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void StatusBadge(string text, bool isActive, ControlVariant variant, ControlSize size, ComponentAppearance appearance, params GUILayoutOption[] options) =>
            Render(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Appearance = appearance,
                    ShowStatusDot = true,
                    IsActive = isActive,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public AvatarBuilder Avatar() => new(this);

        public ProgressBuilder Progress(float value = 0f) => new(this, value);

        public void Progress(float value, float width) => Render(new ProgressConfig { Value = value, Width = width });

        public void Progress(float value, float width, ControlVariant variant, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new ProgressConfig
                {
                    Value = value,
                    Width = width,
                    Variant = variant,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public ChartBuilder Chart() => new(this);

        public DialogBuilder Dialog(string id) => new(this, id);

        public PopoverBuilder Popover(string id = "popover") => new(this, id);

        public ToastBuilder Toast() => new(this);

        public CardBuilder Card() => new(this);

        public void BeginCard(float width = -1f, float height = -1f, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _card.BeginCard(width, height, variant, size, appearance);
                },
                nameof(BeginCard)
            );

        public void CardHeader(Action content) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _card.CardHeader(content);
                },
                nameof(CardHeader)
            );

        public void CardContent(Action content) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _card.CardContent(content);
                },
                nameof(CardContent)
            );

        public void CardFooter(Action content) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _card.CardFooter(content);
                },
                nameof(CardFooter)
            );

        public void EndCard() =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _card.EndCard();
                },
                nameof(EndCard)
            );

        public SeparatorBuilder Separator() => new(this);

        public void Heading(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Default, options: options);

        public void Caption(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Muted, options: options);

        public void MutedLabel(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Muted, options: options);

        public void ErrorAlert(string text, params GUILayoutOption[] options) => Label(text, ControlVariant.Destructive, options: options);

        public void HorizontalSeparator(ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new SeparatorConfig
                {
                    Orientation = SeparatorOrientation.Horizontal,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void VerticalSeparator(ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new SeparatorConfig
                {
                    Orientation = SeparatorOrientation.Vertical,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public TabsBuilder Tabs() => new(this);

        public int Tabs(TabsConfig config) => Render(config);

        public TableBuilder Table() => new(this);

        public void Table(TableConfig config) => Render(config);

        public NavigationBuilder Navigation() => new(this);

        public int Navigation(NavigationConfig config) => Render(config);

        public MenuBarBuilder MenuBar() => new(this);

        public void MenuBar(MenuBar.MenuBarConfig config) => Render(config);

        public DataTableState GetDataTableState(string id) => Execute(() => _dataTable.GetTableState(id), null, nameof(GetDataTableState));

        public void SetDataTablePageSize(string id, int size) => Execute(() => _dataTable.SetPageSize(id, size), nameof(SetDataTablePageSize));

        public void ClearDataTableSelection(string id) => Execute(() => _dataTable.ClearSelection(id), nameof(ClearDataTableSelection));

        public List<string> GetSelectedDataTableRows(string id) => Execute(() => _dataTable.GetSelectedRows(id), new List<string>(), nameof(GetSelectedDataTableRows));

        internal void RegisterAutoRenderBuilder(IAutoRenderBuilder builder)
        {
            if (_flushingAutoRenderBuilder)
            {
                _pendingAutoRenderBuilder = builder;
                return;
            }

            FlushAutoRenderBuilder();
            _pendingAutoRenderBuilder = builder;
        }

        internal void ClearAutoRenderBuilder(IAutoRenderBuilder builder)
        {
            if (ReferenceEquals(_pendingAutoRenderBuilder, builder))
                _pendingAutoRenderBuilder = null;
        }

        internal void FlushAutoRenderBuilder()
        {
            if (_flushingAutoRenderBuilder)
                return;

            var builder = _pendingAutoRenderBuilder;
            if (builder == null)
                return;

            _pendingAutoRenderBuilder = null;
            _flushingAutoRenderBuilder = true;
            try
            {
                builder.RenderIfPending();
            }
            finally
            {
                _flushingAutoRenderBuilder = false;
            }
        }

        internal bool Render(ButtonConfig config)
        {
            FlushAutoRenderBuilder();
            return Execute(() => _button.Render(config), false, nameof(Button));
        }

        internal bool DrawButton(string text, Action onClick = null, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool disabled = false, float opacity = 1f, IconConfig icon = null, params GUILayoutOption[] options) =>
            Render(
                new ButtonConfig
                {
                    Text = text,
                    OnClick = onClick,
                    Variant = variant,
                    Size = size,
                    IsDisabled = disabled,
                    Opacity = opacity,
                    Icon = icon,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        internal bool DrawButton(string text, ControlVariant variant, ControlSize size = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new ButtonConfig
                {
                    Text = text,
                    OnClick = onClick,
                    Variant = variant,
                    Size = size,
                    IsDisabled = disabled,
                    Opacity = opacity,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        internal string Render(InputConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return Execute(() => _input.Render(config), string.Empty, nameof(Input));

            string key = config.Id ?? config.Label ?? config.Placeholder;
            return ExecStatefulStr(
                nameof(Input),
                state =>
                {
                    config.Value = state;
                    return _input.Render(config);
                },
                config.Value,
                key
            );
        }

        internal bool Render(CheckboxConfig config)
        {
            FlushAutoRenderBuilder();
            return RenderBoolean(nameof(Checkbox), config, _checkbox.Render);
        }

        internal bool Render(SwitchConfig config)
        {
            FlushAutoRenderBuilder();
            return RenderBoolean(nameof(Switch), config, _switch.Render);
        }

        internal bool Render(ToggleConfig config)
        {
            FlushAutoRenderBuilder();
            return RenderBoolean(nameof(Toggle), config, _toggle.Render);
        }

        internal bool DrawToggle(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onChange = null, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new ToggleConfig
                {
                    Label = text,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnValueChanged = onChange,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        internal float Render(SliderConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return Execute(() => _slider.Render(config), 0f, nameof(Slider));

            return ExecStatefulFloat(
                nameof(Slider),
                state =>
                {
                    config.Value = state;
                    return _slider.Render(config);
                },
                config.Value,
                config.Id ?? config.Label
            );
        }

        internal Vector2 Render(RangeSliderConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return Execute(() => _rangeSlider.Render(config), Vector2.zero, nameof(RangeSlider));

            return ExecStatefulV2(
                nameof(RangeSlider),
                state =>
                {
                    config.LowerValue = state.x;
                    config.UpperValue = state.y;
                    return _rangeSlider.Render(config);
                },
                new Vector2(config.LowerValue, config.UpperValue),
                config.Id ?? config.Label
            );
        }

        internal int Render(SelectConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return Execute(() => _select.Render(config), 0, nameof(Select));

            string key = config.Id ?? config.Label ?? config.Placeholder;
            return ExecStatefulInt(
                nameof(Select),
                state =>
                {
                    config.SelectedIndex = state;
                    return _select.Render(config);
                },
                config.SelectedIndex,
                key
            );
        }

        internal void Render(DropdownMenuConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _dropdownMenu.Render(config), nameof(DropdownMenu));
        }

        internal void Render(ThemeChangerConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _themeChanger.Render(config), nameof(ThemeChanger));
        }

        internal void Render(FontChangerConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _fontChanger.Render(config), nameof(FontChanger));
        }

        internal string Render(TextAreaConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return Execute(() => _textArea.Render(config), string.Empty, nameof(TextArea));

            return ExecStatefulStr(
                nameof(TextArea),
                state =>
                {
                    config.Value = state;
                    return _textArea.Render(config);
                },
                config.Value,
                config.Id ?? config.Label ?? config.Placeholder
            );
        }

        internal DateTime? Render(CalendarConfig config)
        {
            FlushAutoRenderBuilder();

            config ??= new CalendarConfig();
            string key = config.Id ?? nameof(Calendar);
            return ExecStatefulDate(
                nameof(Calendar),
                state =>
                {
                    DateTime? next = state;
                    config.SelectedDate = state;
                    config.OnDateSelected = date => next = date;
                    _calendar.Render(config);
                    return next;
                },
                config.SelectedDate,
                key
            );
        }

        internal DateTime? Render(DatePickerConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return Execute(() => _datePicker.Render(config), null, nameof(DatePicker));

            return ExecStatefulDate(
                nameof(DatePicker),
                state =>
                {
                    config.SelectedDate = state;
                    return _datePicker.Render(config);
                },
                config.SelectedDate ?? config.StartDate,
                config.Id
            );
        }

        internal void Render(DataTableConfig config)
        {
            FlushAutoRenderBuilder();

            if (config == null)
                return;

            Execute(() => _dataTable.Render(config), nameof(DataTable));
        }

        internal void Render(LabelConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _label.Render(config), nameof(Label));
        }

        internal void Render(BadgeConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _badge.Render(config), nameof(Badge));
        }

        internal void Render(AvatarConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _avatar.Render(config), nameof(Avatar));
        }

        internal void Render(ProgressConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _progress.Render(config), nameof(Progress));
        }

        internal void Render(ChartConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _chart.Render(config), nameof(Chart));
        }

        internal void Render(DialogConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _dialog.Render(config), nameof(Dialog));
        }

        internal void Render(PopoverConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _popover.Render(config), nameof(Popover));
        }

        internal void Render(CardConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _card.Render(config), nameof(Card));
        }

        internal void Render(SeparatorConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _separator.Render(config), nameof(Separator));
        }

        internal int Render(TabsConfig config)
        {
            FlushAutoRenderBuilder();
            return Execute(() => _tabs.Render(config), config?.SelectedIndex ?? 0, nameof(Tabs));
        }

        internal void Render(TableConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _table.Render(config), nameof(Table));
        }

        internal int Render(NavigationConfig config)
        {
            FlushAutoRenderBuilder();
            return Execute(() => _navigation.Render(config), config?.SelectedIndex ?? 0, nameof(Navigation));
        }

        internal void Render(MenuBar.MenuBarConfig config)
        {
            FlushAutoRenderBuilder();
            Execute(() => _menuBar.Render(config), nameof(MenuBar));
        }

        public void ShowToast(ToastConfig config) =>
            Execute(
                () =>
                {
                    FlushAutoRenderBuilder();
                    _toast.Show(config);
                },
                nameof(Toast)
            );

        public void DismissToast(string id, bool animate = true) => Execute(() => _toast.Dismiss(id, animate), nameof(DismissToast));

        public void DismissAllToasts(bool animate = true) => Execute(() => _toast.DismissAll(animate), nameof(DismissAllToasts));

        public int GetActiveToastCount() => Execute(() => _toast.GetActiveToastCount(), 0, nameof(GetActiveToastCount));

        public void OpenDialog(string id) => Execute(() => _dialog.Open(id), nameof(OpenDialog));

        public void CloseDialog() => Execute(_dialog.Close, nameof(CloseDialog));

        public bool IsDialogOpen() => Execute(() => _dialog.IsOpen, false, nameof(IsDialogOpen));

        public void OpenPopover(string id, int zIndex = -1) => Execute(() => _popover.Open(id, zIndex), nameof(OpenPopover));

        public void ClosePopover() => Execute(_popover.Close, nameof(ClosePopover));

        public bool IsPopoverOpen() => Execute(() => _popover.IsOpen, false, nameof(IsPopoverOpen));

        public void OpenSelect(SelectConfig config, Rect anchorRect) => Execute(() => _select.Open(config, anchorRect), nameof(OpenSelect));

        public void CloseSelect(string id = "select") => Execute(() => _select.Close(id), nameof(CloseSelect));

        public bool IsSelectOpen(string id) => Execute(() => _select.IsOpen(id), false, nameof(IsSelectOpen));

        public void CloseDropdownMenu(string id) => Execute(() => _dropdownMenu.Close(id), nameof(CloseDropdownMenu));

        public void CloseDatePicker(string id) => Execute(() => _datePicker.CloseDatePicker(id), nameof(CloseDatePicker));

        public bool IsDatePickerOpen(string id) => Execute(() => _datePicker.IsDatePickerOpen(id), false, nameof(IsDatePickerOpen));

        internal Rect GetRootGuiScreenRect()
        {
            if (!_rootGuiScreenRectValid)
            {
                _rootGuiScreenRect = CaptureGuiScreenRect();
                _rootGuiScreenRectValid = true;
            }

            return _rootGuiScreenRect;
        }

        internal void PushStateScope(string scope)
        {
            if (!string.IsNullOrWhiteSpace(scope))
                _stateScopes.Push(scope);
        }

        internal void PopStateScope()
        {
            if (_stateScopes.Count > 0)
                _stateScopes.Pop();
        }

        private bool RenderBoolean<TConfig>(string operation, TConfig config, Func<TConfig, bool> renderer)
            where TConfig : BoolControlConfigBase
        {
            if (config == null)
                return Execute(() => renderer(config), false, operation);

            return ExecStatefulBool(
                operation,
                state =>
                {
                    config.Value = state;
                    return renderer(config);
                },
                config.Value,
                config.Id ?? config.Label
            );
        }

        private void WrapHorizontal(Action draw, GUILayoutOption[] options)
        {
            _layout.BeginHorizontalGroup(options);
            try
            {
                draw?.Invoke();
                FlushAutoRenderBuilder();
            }
            finally
            {
                _layout.EndHorizontalGroup();
            }
        }

        private void WrapVertical(Action draw, GUILayoutOption[] options)
        {
            _layout.BeginVerticalGroup(options);
            try
            {
                draw?.Invoke();
                FlushAutoRenderBuilder();
            }
            finally
            {
                _layout.EndVerticalGroup();
            }
        }

        private int GetStateId(string prefix, string key = null)
        {
            string fullKey = string.IsNullOrEmpty(key) ? prefix : $"{prefix}:{key}";
            return GUIUtility.GetControlID(new GUIContent(fullKey), FocusType.Passive);
        }

        private string GetScopedStateKey(string leaf = null)
        {
            if (_stateScopes.Count == 0)
                return leaf;

            var scopes = _stateScopes.ToArray();
            Array.Reverse(scopes);
            string prefix = string.Join("/", scopes);
            return string.IsNullOrEmpty(leaf) ? prefix : $"{prefix}:{leaf}";
        }

        private float GetFloatState(int id, float value)
        {
            if (!_floatState.TryGetValue(id, out _))
            {
                _floatState[id] = value;
                _floatInput[id] = value;
                return value;
            }

            if (_floatInput.TryGetValue(id, out float lastInput) && Mathf.Abs(value - lastInput) > 0.0001f)
                _floatState[id] = value;

            _floatInput[id] = value;
            return _floatState[id];
        }

        private void SetFloatState(int id, float value) => _floatState[id] = value;

        private int GetIntState(int id, int value)
        {
            if (!_intState.TryGetValue(id, out _))
            {
                _intState[id] = value;
                _intInput[id] = value;
                return value;
            }

            if (_intInput.TryGetValue(id, out int lastInput) && value != lastInput)
                _intState[id] = value;

            _intInput[id] = value;
            return _intState[id];
        }

        private void SetIntState(int id, int value) => _intState[id] = value;

        private bool GetBoolState(int id, bool value)
        {
            if (!_boolState.TryGetValue(id, out _))
            {
                _boolState[id] = value;
                _boolInput[id] = value;
                return value;
            }

            if (_boolInput.TryGetValue(id, out bool lastInput) && value != lastInput)
                _boolState[id] = value;

            _boolInput[id] = value;
            return _boolState[id];
        }

        private void SetBoolState(int id, bool value) => _boolState[id] = value;

        private string GetStringState(int id, string value)
        {
            string normalized = value ?? string.Empty;
            if (!_stringState.TryGetValue(id, out _))
            {
                _stringState[id] = normalized;
                _stringInput[id] = normalized;
                return normalized;
            }

            if (_stringInput.TryGetValue(id, out string lastInput) && !string.Equals(normalized, lastInput, StringComparison.Ordinal))
                _stringState[id] = normalized;

            _stringInput[id] = normalized;
            return _stringState[id];
        }

        private void SetStringState(int id, string value) => _stringState[id] = value ?? string.Empty;

        private Vector2 GetV2State(int id, Vector2 value)
        {
            if (!_v2State.TryGetValue(id, out _))
            {
                _v2State[id] = value;
                _v2Input[id] = value;
                return value;
            }

            if (_v2Input.TryGetValue(id, out Vector2 lastInput) && (value - lastInput).sqrMagnitude > 1e-6f)
                _v2State[id] = value;

            _v2Input[id] = value;
            return _v2State[id];
        }

        private void SetV2State(int id, Vector2 value) => _v2State[id] = value;

        private DateTime? GetDateState(int id, DateTime? value)
        {
            if (!_dateState.TryGetValue(id, out _))
            {
                _dateState[id] = value;
                _dateInput[id] = value;
                return value;
            }

            if (_dateInput.TryGetValue(id, out DateTime? lastInput) && value != lastInput)
                _dateState[id] = value;

            _dateInput[id] = value;
            return _dateState[id];
        }

        private void SetDateState(int id, DateTime? value) => _dateState[id] = value;

        private float ExecStatefulFloat(string prefix, Func<float, float> draw, float value, string key = null)
        {
            int id = GetStateId(prefix, key);
            float state = GetFloatState(id, value);
            float result = Execute(() => draw(state), state, prefix);
            SetFloatState(id, result);
            return result;
        }

        private int ExecStatefulInt(string prefix, Func<int, int> draw, int value, string key = null)
        {
            int id = GetStateId(prefix, key);
            int state = GetIntState(id, value);
            int result = Execute(() => draw(state), state, prefix);
            SetIntState(id, result);
            return result;
        }

        private bool ExecStatefulBool(string prefix, Func<bool, bool> draw, bool value, string key = null)
        {
            int id = GetStateId(prefix, key);
            bool state = GetBoolState(id, value);
            bool result = Execute(() => draw(state), state, prefix);
            SetBoolState(id, result);
            return result;
        }

        private string ExecStatefulStr(string prefix, Func<string, string> draw, string value, string key = null)
        {
            int id = GetStateId(prefix, key);
            string state = GetStringState(id, value);
            string result = Execute(() => draw(state), state, prefix);
            SetStringState(id, result);
            return result;
        }

        private Vector2 ExecStatefulV2(string prefix, Func<Vector2, Vector2> draw, Vector2 value, string key = null)
        {
            int id = GetStateId(prefix, key);
            Vector2 state = GetV2State(id, value);
            Vector2 result = Execute(() => draw(state), state, prefix);
            SetV2State(id, result);
            return result;
        }

        private DateTime? ExecStatefulDate(string prefix, Func<DateTime?, DateTime?> draw, DateTime? value, string key = null)
        {
            int id = GetStateId(prefix, key);
            DateTime? state = GetDateState(id, value);
            DateTime? result = Execute(() => draw(state), state, prefix);
            SetDateState(id, result);
            return result;
        }

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

        private Rect CaptureGuiScreenRect()
        {
            Rect clip = GetClipRect();
            Vector2 origin = GUIUtility.GUIToScreenPoint(Vector2.zero);
            return new Rect(origin.x + clip.x, origin.y + clip.y, clip.width, clip.height);
        }

        private Rect GetClipRect()
        {
            try
            {
                var guiClip = typeof(GUI).Assembly.GetType("UnityEngine.GUIClip");
                var topRectMethod = guiClip?.GetMethod("GetTopRect", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (topRectMethod != null && topRectMethod.Invoke(null, null) is Rect topRect && topRect.width > 1f && topRect.height > 1f)
                    return topRect;

                var prop = guiClip?.GetProperty("visibleRect", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (prop != null && prop.GetValue(null, null) is Rect rect && rect.width > 1f && rect.height > 1f)
                    return rect;
            }
            catch { }

            return new Rect(0f, 0f, Screen.width, Screen.height);
        }

        private void ApplyDefaultFont()
        {
            string[] preferredFonts = { DefaultFontName, "Arial" };
            foreach (var fontName in preferredFonts)
            {
                if (!TryCreateOsFont(fontName, out var font))
                    continue;

                _currentFontName = fontName;
                _ownedDynamicFont = font;
                _styleManager.CustomFont = font;
                _styleManager.MarkStylesCorruption();
                return;
            }
        }

        private bool TryCreateOsFont(string fontName, out Font font)
        {
            font = null;
            fontName = NormalizeFontName(fontName);
            if (string.IsNullOrWhiteSpace(fontName) || !IsFontAvailable(fontName))
                return false;

            try
            {
                font = Font.CreateDynamicFontFromOSFont(fontName, Mathf.Max(8, fontSize));
                return font != null;
            }
            catch
            {
                font = null;
                return false;
            }
        }

        private void ReleaseOwnedDynamicFont()
        {
            if (_ownedDynamicFont == null)
                return;

            if (_styleManager.CustomFont == _ownedDynamicFont)
                _styleManager.CustomFont = null;

            if (Application.isPlaying)
                UnityEngine.Object.Destroy(_ownedDynamicFont);
            else
                UnityEngine.Object.DestroyImmediate(_ownedDynamicFont);

            _ownedDynamicFont = null;
        }

        private bool IsFontAvailable(string fontName)
        {
            EnsureFontsLoaded();
            fontName = NormalizeFontName(fontName);
            return _availableFonts != null && _availableFonts.Contains(fontName);
        }

        private void EnsureFontsLoaded()
        {
            if (_availableFonts != null)
                return;

            try
            {
                _availableFonts = new HashSet<string>(Font.GetOSInstalledFontNames() ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                _availableFonts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private static string NormalizeFontName(string fontName)
        {
            if (string.IsNullOrWhiteSpace(fontName))
                return string.Empty;

            string normalized = fontName.Trim();
            string[] suffixes = { " Bold Italic", " Bold Oblique", " SemiBold Italic", " Semibold Italic", " ExtraBold Italic", " Light Italic", " Italic", " Oblique", " SemiBold", " Semibold", " ExtraBold", " Bold", " Regular" };

            bool changed;
            do
            {
                changed = false;
                foreach (var suffix in suffixes)
                {
                    if (!normalized.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                        continue;

                    normalized = normalized[..^suffix.Length].TrimEnd();
                    changed = true;
                    break;
                }
            } while (changed && !string.IsNullOrWhiteSpace(normalized));

            return normalized;
        }
    }
}
