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
        // Core
        private readonly StyleManager _styleManager;
        private readonly AnimationManager _animationManager;
        private readonly Layout.Layout _layout;
        private readonly List<BaseComponent> _components;

        // Controls
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

        // Display
        private readonly Label _label;
        private readonly Badge _badge;
        private readonly AvatarComponent _avatar;
        private readonly Progress _progress;
        private readonly Chart _chart;
        private readonly Dialog _dialog;
        private readonly Popover _popover;
        private readonly Toast _toast;
        private readonly Tooltip _tooltip;

        // Layout
        private readonly Card _card;
        private readonly Separator _separator;
        private readonly Tabs _tabs;
        private readonly MenuBar _menuBar;
        private readonly Table _table;
        private readonly Navigation _navigation;

        // State dictionaries
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
        private readonly Dictionary<string, bool> _legacySelectOpen = new();
        private readonly Stack<string> _stateScopes = new();
        private const string LegacySelectId = "legacy_select";

        internal int fontSize = 14;
        public float uiScale = 1f;

        private bool _scrollbarsInitialized;
        private int _lastCheckFrame = -10;
        internal Rect _rootGuiScreenRect;
        internal bool _rootGuiScreenRectValid;

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

        // Theme & style
        public StyleManager GetStyleManager() => _styleManager;

        public AnimationManager GetAnimationManager() => _animationManager;

        public ThemeManager GetThemeManager() => ThemeManager.Instance;

        public Theme CurrentTheme => ThemeManager.Instance.CurrentTheme;

        public Chart GetChartComponent() => _chart;

        public Chart GetChartComponents() => _chart;

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

        public void RegisterStyle(StyleComponentType type, string styleId, ComponentAppearance profile)
        {
            _styleManager.RegisterStyle(type, styleId, profile);
        }

        public void RegisterStyle(StyleComponentType type, string styleId, StatefulStyleModifier modifier)
        {
            _styleManager.RegisterStyle(type, styleId, modifier);
        }

        public bool UnregisterStyle(StyleComponentType type, string styleId)
        {
            return _styleManager.UnregisterStyle(type, styleId);
        }

        public void SetUiScale(float scale)
        {
            var n = Mathf.Max(0.5f, scale);
            if (Mathf.Abs(uiScale - n) > 0.001f)
            {
                uiScale = n;
                _styleManager.MarkStylesCorruption();
            }
        }

        public void SetFontSize(int size)
        {
            var n = Mathf.Max(8, size);
            if (fontSize != n)
            {
                fontSize = n;
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

        public void EndGUI() => Execute(_animationManager.EndGUI, nameof(EndGUI));

        public void DrawOverlay() => DrawOverlays();

        public void DrawOverlays() =>
            Execute(
                () =>
                {
                    var prevMatrix = GUI.matrix;
                    var prevColor = GUI.color;
                    var prevEnabled = GUI.enabled;

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
                        GUI.matrix = prevMatrix;
                        GUI.color = prevColor;
                        GUI.enabled = prevEnabled;
                    }
                },
                nameof(DrawOverlays)
            );

        public void Cleanup()
        {
            foreach (var c in _components)
                c.Dispose();
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
            _legacySelectOpen.Clear();
            _rootGuiScreenRectValid = false;
        }

        public void Dispose() => Cleanup();

        // Layout helpers
        public Vector2 ScrollView(Vector2 pos, Action draw, params GUILayoutOption[] opts)
        {
            string scopedKey = GetScopedStateKey(nameof(ScrollView));
            if (!string.IsNullOrEmpty(scopedKey))
            {
                int id = GetStateId(nameof(ScrollView), scopedKey);
                Vector2 seed = _v2State.TryGetValue(id, out var existing) ? existing : Vector2.zero;
                Vector2 result = Execute(() => _layout.DrawScrollView(seed, draw, opts), seed, nameof(ScrollView));
                SetV2State(id, result);
                _v2Input[id] = result;
                return result;
            }

            return ExecStatefulV2(nameof(ScrollView), s => _layout.DrawScrollView(s, draw, opts), pos);
        }

        public Vector2 ScrollView(ref Vector2 pos, Action draw, params GUILayoutOption[] opts)
        {
            pos = ScrollView(pos, draw, opts);
            return pos;
        }

        public void BeginHorizontalGroup(params GUILayoutOption[] opts) => Execute(() => _layout.BeginHorizontalGroup(opts), nameof(BeginHorizontalGroup));

        public void BeginHorizontalGroup(GUIStyle s, params GUILayoutOption[] opts) => Execute(() => _layout.BeginHorizontalGroup(s, opts), nameof(BeginHorizontalGroup));

        public void EndHorizontalGroup() => Execute(_layout.EndHorizontalGroup, nameof(EndHorizontalGroup));

        public void BeginVerticalGroup(params GUILayoutOption[] opts) => Execute(() => _layout.BeginVerticalGroup(opts), nameof(BeginVerticalGroup));

        public void BeginVerticalGroup(GUIStyle s, params GUILayoutOption[] opts) => Execute(() => _layout.BeginVerticalGroup(s, opts), nameof(BeginVerticalGroup));

        public void EndVerticalGroup() => Execute(_layout.EndVerticalGroup, nameof(EndVerticalGroup));

        public void AddSpace(float px) => Execute(() => _layout.AddSpace(px), nameof(AddSpace));

        public void FlexSpace() => GUILayout.FlexibleSpace();

        public void ButtonGroup(Action draw, bool horizontal = true, float spacing = 6f)
        {
            Execute(
                () =>
                {
                    if (horizontal)
                        _layout.BeginHorizontalGroup();
                    else
                        _layout.BeginVerticalGroup();

                    draw?.Invoke();

                    if (horizontal)
                        _layout.EndHorizontalGroup();
                    else
                        _layout.EndVerticalGroup();
                },
                nameof(ButtonGroup)
            );
        }

        // Button
        public bool Button(ButtonConfig cfg) => Execute(() => _button.Draw(cfg), false, nameof(Button));

        public bool Button(string text, ControlVariant v, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Button(
                new ButtonConfig
                {
                    Text = text,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Opacity = opacity,
                    Appearance = appearance,
                    OnClick = onClick,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Button(string text, Texture2D icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] opts) =>
            Button(
                new ButtonConfig
                {
                    Text = text,
                    Icon = icon != null ? new IconConfig(icon) : null,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Opacity = opacity,
                    OnClick = onClick,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Button(string text, IconConfig icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] opts) =>
            Button(
                new ButtonConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Opacity = opacity,
                    OnClick = onClick,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Button(string text, Action onClick = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, float opacity = 1f, IconConfig icon = null, params GUILayoutOption[] opts) =>
            Button(
                new ButtonConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Opacity = opacity,
                    OnClick = onClick,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        // Input
        public string Input(InputConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _input.Draw(cfg), string.Empty, nameof(Input));
            string key = cfg.Id ?? cfg.Label ?? cfg.Placeholder;
            return ExecStatefulStr(
                nameof(Input),
                s =>
                {
                    cfg.Value = s;
                    return _input.Draw(cfg);
                },
                cfg.Value,
                key
            );
        }

        public string Input(string value, string placeholder = "", string label = null, Action<string> onChange = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Input(
                new InputConfig
                {
                    Value = value,
                    Placeholder = placeholder,
                    Label = label,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnValueChanged = onChange,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public string Input(string placeholder, Texture2D icon, string value = "", ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Input(
                new InputConfig
                {
                    Value = value,
                    Placeholder = placeholder,
                    Icon = icon != null ? new IconConfig(icon) : null,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public string Input(string placeholder, IconConfig icon, string value = "", ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Input(
                new InputConfig
                {
                    Value = value,
                    Placeholder = placeholder,
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public string Password(InputConfig cfg)
        {
            if (cfg != null)
                cfg.InputKind = InputKind.Password;
            return Input(cfg);
        }

        public string Password(string value, string label = null, char mask = '*', Action<string> onChange = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Password(
                new InputConfig
                {
                    Value = value,
                    Label = label,
                    MaskCharacter = mask,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnValueChanged = onChange,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public string PasswordField(float width, string placeholder, ref string value, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false)
        {
            var options = width > 0 ? new[] { GUILayout.Width(width * uiScale) } : Array.Empty<GUILayoutOption>();
            value = Password(
                new InputConfig
                {
                    Value = value,
                    Placeholder = placeholder,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    InputKind = InputKind.Password,
                    LayoutOptions = options,
                }
            );
            return value;
        }

        // Toggle
        public bool Toggle(ToggleConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _toggle.Draw(cfg), false, nameof(Toggle));
            string key = cfg.Id ?? cfg.Label;
            return ExecStatefulBool(
                nameof(Toggle),
                s =>
                {
                    cfg.Value = s;
                    return _toggle.Draw(cfg);
                },
                cfg.Value,
                key
            );
        }

        public bool Toggle(string label, bool value, Action<bool> onToggle = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Toggle(
                new ToggleConfig
                {
                    Label = label,
                    Value = value,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Toggle(string label, bool value, ControlVariant v, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Toggle(
                new ToggleConfig
                {
                    Label = label,
                    Value = value,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Toggle(string label, IconConfig icon, bool value, Action<bool> onToggle = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Toggle(
                new ToggleConfig
                {
                    Label = label,
                    Icon = icon,
                    Value = value,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    Appearance = appearance,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Toggle(string label, Texture2D icon, bool value, Action<bool> onToggle = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Toggle(label, icon != null ? new IconConfig(icon) : null, value, onToggle, v, sz, disabled, appearance, opts);

        // Checkbox
        public bool Checkbox(CheckboxConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _checkbox.Draw(cfg), false, nameof(Checkbox));
            string key = cfg.Id ?? cfg.Label;
            return ExecStatefulBool(
                nameof(Checkbox),
                s =>
                {
                    cfg.Value = s;
                    return _checkbox.Draw(cfg);
                },
                cfg.Value,
                key
            );
        }

        public bool Checkbox(string label, bool value, bool showCheckmark = true, Action<bool> onToggle = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Checkbox(
                new CheckboxConfig
                {
                    Label = label,
                    Value = value,
                    ShowCheckmark = showCheckmark,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Checkbox(string label, bool value, ControlVariant v, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Checkbox(
                new CheckboxConfig
                {
                    Label = label,
                    Value = value,
                    ShowCheckmark = true,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        // Switch
        public bool Switch(SwitchConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _switch.Draw(cfg), false, nameof(Switch));
            string key = cfg.Id ?? cfg.Label;
            return ExecStatefulBool(
                nameof(Switch),
                s =>
                {
                    cfg.Value = s;
                    return _switch.Draw(cfg);
                },
                cfg.Value,
                key
            );
        }

        public bool Switch(string label, bool value, Action<bool> onToggle = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Switch(
                new SwitchConfig
                {
                    Label = label,
                    Value = value,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Switch(string label, bool value, ControlVariant v, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Switch(
                new SwitchConfig
                {
                    Label = label,
                    Value = value,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnValueChanged = onToggle,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        // Slider
        public float Slider(SliderConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _slider.Draw(cfg), 0f, nameof(Slider));
            string key = cfg.Id ?? cfg.Label;
            return ExecStatefulFloat(
                nameof(Slider),
                s =>
                {
                    cfg.Value = s;
                    return _slider.Draw(cfg);
                },
                cfg.Value,
                key
            );
        }

        public float Slider(
            float value,
            float min,
            float max,
            float step = 0f,
            string label = null,
            Action<float> onChange = null,
            ControlVariant v = ControlVariant.Default,
            ControlSize sz = ControlSize.Default,
            bool disabled = false,
            bool showValue = true,
            string format = "F2",
            params GUILayoutOption[] opts
        ) =>
            Slider(
                new SliderConfig
                {
                    Label = label,
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Step = step,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    ShowValue = showValue,
                    ValueFormat = format,
                    OnValueChanged = onChange,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public float LabeledSlider(string label, float value, float min, float max, bool showValue = true, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, string format = "F2", params GUILayoutOption[] opts) =>
            Slider(
                new SliderConfig
                {
                    Label = label,
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Step = 0f,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    ShowValue = showValue,
                    ValueFormat = format,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public float LabeledSlider(string label, float value, float min, float max, float step, bool showValue = true, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, string format = "F2", params GUILayoutOption[] opts) =>
            Slider(
                new SliderConfig
                {
                    Label = label,
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Step = step,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    ShowValue = showValue,
                    ValueFormat = format,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public float DisabledSlider(float value, float min, float max, float step = 0f, bool showValue = true, string format = "F2", ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) =>
            Slider(
                new SliderConfig
                {
                    Value = value,
                    MinValue = min,
                    MaxValue = max,
                    Step = step,
                    Variant = v,
                    Size = sz,
                    IsDisabled = true,
                    ShowValue = showValue,
                    ValueFormat = format,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        // Select
        public int Select(SelectConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _select.Draw(cfg), 0, nameof(Select));
            string key = cfg.Id ?? cfg.Label ?? "select";
            return ExecStatefulInt(
                nameof(Select),
                s =>
                {
                    cfg.SelectedIndex = s;
                    return _select.Draw(cfg);
                },
                cfg.SelectedIndex,
                key
            );
        }

        public int Select(string label, string[] items, int selectedIndex, Action<int> onChange = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts)
        {
            var options = items == null ? Array.Empty<SelectOption>() : Array.ConvertAll(items, t => new SelectOption(t, t));
            return Select(
                new SelectConfig
                {
                    Label = label,
                    Options = options,
                    SelectedIndex = selectedIndex,
                    Variant = v,
                    Size = sz,
                    IsDisabled = disabled,
                    OnSelectionChanged = onChange,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public int Select(string[] items, int selectedIndex, Action<int> onChange = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts)
        {
            var options = items == null ? Array.Empty<SelectOption>() : Array.ConvertAll(items, t => new SelectOption(t, t));
            int result = selectedIndex;
            var cfg = new SelectConfig
            {
                Id = LegacySelectId,
                Options = options,
                SelectedIndex = selectedIndex,
                Variant = v,
                Size = sz,
                IsDisabled = disabled,
                OnSelectionChanged = i =>
                {
                    result = i;
                    onChange?.Invoke(i);
                },
                LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
            };

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;
            Execute(() => _select.DrawMenu(cfg), nameof(SelectMenu));
            GUI.enabled = wasEnabled;

            if (result != selectedIndex)
                _legacySelectOpen[LegacySelectId] = false;

            return result;
        }

        public int SelectMenu(SelectConfig cfg) => Execute(() => _select.DrawMenu(cfg), 0, nameof(SelectMenu));

        public void OpenSelect() => OpenSelect(LegacySelectId);

        public void OpenSelect(string id)
        {
            if (string.IsNullOrEmpty(id))
                id = LegacySelectId;
            _legacySelectOpen[id] = true;
        }

        public void OpenSelect(SelectConfig cfg, Rect anchorRect) => Execute(() => _select.Open(cfg, anchorRect), nameof(OpenSelect));

        public void CloseSelect() => CloseSelect(LegacySelectId);

        public void CloseSelect(string id) =>
            Execute(
                () =>
                {
                    _select.Close(id);
                    if (_legacySelectOpen.ContainsKey(id))
                        _legacySelectOpen[id] = false;
                },
                nameof(CloseSelect)
            );

        public bool IsSelectOpen() => IsSelectOpen(LegacySelectId);

        public bool IsSelectOpen(string id) =>
            Execute(
                () =>
                {
                    if (_select.IsOpen(id))
                        return true;
                    return _legacySelectOpen.TryGetValue(id, out bool open) && open;
                },
                false,
                nameof(IsSelectOpen)
            );

        public void DropdownMenu(DropdownMenuConfig cfg) => Execute(() => _dropdownMenu.Draw(cfg), nameof(DropdownMenu));

        public void OpenDropdownMenu(DropdownMenuConfig cfg, Rect anchorRect) => Execute(() => _dropdownMenu.Open(cfg, anchorRect), nameof(OpenDropdownMenu));

        public void CloseDropdownMenu(string id) => Execute(() => _dropdownMenu.Close(id), nameof(CloseDropdownMenu));

        public bool IsDropdownMenuOpen(string id) => Execute(() => _dropdownMenu.IsOpen(id), false, nameof(IsDropdownMenuOpen));

        public void ThemeChanger(ThemeChangerConfig cfg) => Execute(() => _themeChanger.Draw(cfg), nameof(ThemeChanger));

        public void ThemeChangerWithPreview(string id = "theme_changer", float width = 200f) =>
            ThemeChanger(
                new ThemeChangerConfig
                {
                    Id = id,
                    Width = width,
                    ShowPreview = true,
                }
            );

        // TextArea
        public string TextArea(TextAreaConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _textArea.DrawTextArea(cfg), string.Empty, nameof(TextArea));
            string key = cfg.Id ?? cfg.Label ?? cfg.Placeholder;
            return ExecStatefulStr(
                nameof(TextArea),
                s =>
                {
                    cfg.Value = s;
                    return _textArea.DrawTextArea(cfg);
                },
                cfg.Value,
                key
            );
        }

        public string TextArea(string text, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, params GUILayoutOption[] opts) =>
            ExecStatefulStr(nameof(TextArea), s => _textArea.DrawTextArea(s, v, placeholder, disabled, minHeight, maxLen, opts), text, placeholder);

        public string TextArea(ref string text, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, params GUILayoutOption[] opts)
        {
            text = TextArea(text, v, placeholder, disabled, minHeight, maxLen, opts);
            return text;
        }

        public string TextArea(Rect rect, string text, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, int maxLen = -1) => ExecStatefulStr(nameof(TextArea) + "Rect", s => _textArea.DrawTextArea(rect, s, v, placeholder, disabled, maxLen), text, placeholder);

        public string TextArea(Rect rect, ref string text, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, int maxLen = -1)
        {
            text = TextArea(rect, text, v, placeholder, disabled, maxLen);
            return text;
        }

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, params GUILayoutOption[] opts) =>
            ExecStatefulStr(nameof(OutlineTextArea), s => _textArea.OutlineTextArea(s, placeholder, disabled, minHeight, maxLen, opts), text, placeholder);

        public string OutlineTextArea(ref string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, params GUILayoutOption[] opts)
        {
            text = OutlineTextArea(text, placeholder, disabled, minHeight, maxLen, opts);
            return text;
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, params GUILayoutOption[] opts) =>
            ExecStatefulStr(nameof(GhostTextArea), s => _textArea.GhostTextArea(s, placeholder, disabled, minHeight, maxLen, opts), text, placeholder);

        public string GhostTextArea(ref string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, params GUILayoutOption[] opts)
        {
            text = GhostTextArea(text, placeholder, disabled, minHeight, maxLen, opts);
            return text;
        }

        public string LabeledTextArea(string label, string text, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, bool showCharCount = true, params GUILayoutOption[] opts) =>
            ExecStatefulStr(nameof(LabeledTextArea), s => _textArea.LabeledTextArea(label, s, v, placeholder, disabled, minHeight, maxLen, showCharCount, opts), text, label);

        public string LabeledTextArea(string label, ref string text, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLen = -1, bool showCharCount = true, params GUILayoutOption[] opts)
        {
            text = LabeledTextArea(label, text, v, placeholder, disabled, minHeight, maxLen, showCharCount, opts);
            return text;
        }

        public string ResizableTextArea(string text, ref float height, ControlVariant v = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxH = 300f, int maxLen = -1, params GUILayoutOption[] opts)
        {
            try
            {
                return _textArea.ResizableTextArea(text, ref height, v, placeholder, disabled, minHeight, maxH, maxLen, opts);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(ResizableTextArea), nameof(GUIHelper));
                return text ?? string.Empty;
            }
        }

        public void SectionHeader(string text)
        {
            Execute(
                () =>
                {
                    var style = _styleManager?.GetSectionHeaderStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label;
                    UnityHelpers.Label(text, style);
                },
                nameof(SectionHeader)
            );
        }

        public void InputLabel(string text, float width = -1f)
        {
            if (width > 0f)
                Label(text, ControlVariant.Default, false, null, GUILayout.Width(width * uiScale));
            else
                Label(text, ControlVariant.Default);
        }

        // Label
        public void Label(LabelConfig cfg) => Execute(() => _label.DrawLabel(cfg), nameof(Label));

        public void Label(string text, ControlVariant v = ControlVariant.Default, bool disabled = false, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => Execute(() => _label.DrawLabel(text, v, disabled, appearance, opts), nameof(Label));

        public void Label(Rect rect, string text, ControlVariant v = ControlVariant.Default, bool disabled = false) => Execute(() => _label.DrawLabel(rect, text, v, disabled), nameof(Label));

        public void Label(string text, IconConfig icon, ControlVariant v = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Label(
                new LabelConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = v,
                    IsDisabled = disabled,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void MutedLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Muted, false, null, opts);

        public void SecondaryLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Secondary, false, null, opts);

        public void DestructiveLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Destructive, false, null, opts);

        public void Heading(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Default, false, null, opts);

        public void Caption(string text, params GUILayoutOption[] opts) => MutedLabel(text, opts);

        public void CodeLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Secondary, false, null, opts);

        // Badge
        public void Badge(BadgeConfig cfg) => Execute(() => _badge.DrawBadge(cfg), nameof(Badge));

        public void Badge(string text, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => Execute(() => _badge.DrawBadge(text, v, sz, appearance, opts), nameof(Badge));

        public void Badge(string text, IconConfig icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) =>
            Badge(
                new BadgeConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void BadgeWithIcon(string text, Texture2D icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.BadgeWithIcon(text, icon, v, sz, opts), nameof(BadgeWithIcon));

        public void CountBadge(int count, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] opts) => Execute(() => _badge.CountBadge(count, v, sz, maxCount, opts), nameof(CountBadge));

        public void StatusBadge(string text, bool isActive, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.StatusBadge(text, isActive, v, sz, opts), nameof(StatusBadge));

        public void ProgressBadge(string text, float progress, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.ProgressBadge(text, progress, v, sz, opts), nameof(ProgressBadge));

        public void RoundedBadge(string text, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, float cornerRadius = DesignTokens.Radius.XL, params GUILayoutOption[] opts) => Execute(() => _badge.RoundedBadge(text, v, sz, cornerRadius, opts), nameof(RoundedBadge));

        public void AnimatedBadge(string text, string animId = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Execute(
                () =>
                {
                    if (string.IsNullOrEmpty(animId))
                        _badge.AnimatedBadge(text, v, sz, appearance, opts);
                    else
                        _badge.AnimatedBadge(text, animId, v, sz, appearance, opts);
                },
                nameof(AnimatedBadge)
            );

        // Avatar
        public void Avatar(AvatarConfig cfg) => Execute(() => _avatar.DrawAvatar(cfg), nameof(Avatar));

        public void Avatar(Texture2D img, string fallback, ControlSize sz = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] opts) => Execute(() => _avatar.DrawAvatar(img, fallback, sz, shape, opts), nameof(Avatar));

        public void AvatarWithStatus(Texture2D img, string fallback, bool online, ControlSize sz = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] opts) => Execute(() => _avatar.AvatarWithStatus(img, fallback, online, sz, shape, opts), nameof(AvatarWithStatus));

        public void AvatarWithName(Texture2D img, string fallback, string name, ControlSize sz = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] opts) =>
            Execute(() => _avatar.AvatarWithName(img, fallback, name, sz, shape, showNameBelow, opts), nameof(AvatarWithName));

        public void AvatarWithBorder(Texture2D img, string fallback, Color borderColor, ControlSize sz = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] opts) =>
            Execute(() => _avatar.AvatarWithBorder(img, fallback, borderColor, sz, shape, opts), nameof(AvatarWithBorder));

        public void AvatarGroup(IList<(Texture2D img, string fallback)> avatars, ControlSize sz = ControlSize.Default, int maxVisible = 4)
        {
            int count = Mathf.Min(avatars?.Count ?? 0, maxVisible);
            BeginHorizontalGroup();
            for (int i = 0; i < count; i++)
                Avatar(avatars[i].img, avatars[i].fallback, sz);
            if (avatars != null && avatars.Count > maxVisible)
                CountBadge(avatars.Count - maxVisible, ControlVariant.Secondary, sz);
            EndHorizontalGroup();
        }

        // Progress
        public void Progress(ProgressConfig cfg) => Execute(() => _progress.DrawProgress(cfg), nameof(Progress));

        public void Progress(float val, float width = -1, float height = -1, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => Execute(() => _progress.DrawProgress(val, width, height, appearance, opts), nameof(Progress));

        public void Progress(Rect rect, float val, ComponentAppearance appearance = null) => Execute(() => _progress.DrawProgress(rect, val, appearance), nameof(Progress));

        public void LabeledProgress(string label, float val, float width = -1, float height = -1, bool showPercentage = true, ComponentAppearance appearance = null, params GUILayoutOption[] opts) =>
            Execute(() => _progress.LabeledProgress(label, val, width, height, showPercentage, appearance, opts), nameof(LabeledProgress));

        public void CircularProgress(float val, float size = DesignTokens.Height.Small, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => Execute(() => _progress.CircularProgress(val, size, appearance, opts), nameof(CircularProgress));

        public void AnimatedProgress(string id, float val, float width = -1, float height = -1, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => Execute(() => _progress.AnimatedProgress(id, val, width, height, appearance, opts), nameof(AnimatedProgress));

        public void IndeterminateProgress(string id, float width = -1, float height = -1, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => AnimatedProgress(id, Mathf.PingPong(Time.realtimeSinceStartup * 0.5f, 1f), width, height, appearance, opts);

        // Chart
        public void Chart(ChartConfig cfg) => Execute(() => _chart.DrawChart(cfg), nameof(Chart));

        // Dialog
        public void OpenDialog(string id) => Execute(() => _dialog.Open(id), nameof(OpenDialog));

        public void CloseDialog() => Execute(_dialog.Close, nameof(CloseDialog));

        public void Dialog(DialogConfig cfg) => Execute(() => _dialog.DrawDialog(cfg), nameof(Dialog));

        public void Dialog(string id, Action content, float width = 400f, float height = 300f) => Execute(() => _dialog.DrawDialog(id, content, width, height), nameof(Dialog));

        public void Dialog(string id, string title, string desc, Action content, Action footer = null, float width = 400f, float height = 300f) => Execute(() => _dialog.DrawDialog(id, title, desc, content, footer, width, height), nameof(Dialog));

        // Popover
        public void OpenPopover(string id = "popover", int zIndex = -1) => Execute(() => _popover.Open(id, zIndex), nameof(OpenPopover));

        public void ClosePopover() => Execute(_popover.Close, nameof(ClosePopover));

        public bool IsPopoverOpen() => _popover.IsOpen;

        public int GetPopoverZIndex() => Execute(() => _popover.GetZIndex(), DesignTokens.ZIndex.Popover, nameof(GetPopoverZIndex));

        public void Popover(PopoverConfig cfg) => Execute(() => _popover.DrawPopover(cfg), nameof(Popover));

        public void Popover(Action content) => Execute(() => _popover.DrawPopover(content), nameof(Popover));

        // Tooltip
        public void WithTooltip(string text, Action draw) => Execute(() => _tooltip.WithTooltip(text, draw), nameof(WithTooltip));

        public void WithTooltip(string text, TooltipConfig cfg, Action draw) => Execute(() => _tooltip.WithTooltip(text, cfg, draw), nameof(WithTooltip));

        public T WithTooltip<T>(string text, Func<T> draw) => Execute(() => _tooltip.WithTooltip(text, draw), default(T), nameof(WithTooltip));

        public T WithTooltip<T>(string text, TooltipConfig cfg, Func<T> draw) => Execute(() => _tooltip.WithTooltip(text, cfg, draw), default(T), nameof(WithTooltip));

        // Toast
        public void ShowToast(ToastConfig cfg) => Execute(() => _toast.Show(cfg), nameof(ShowToast));

        public void ShowToast(string title, string desc = null, ToastVariant v = ToastVariant.Default, float durationMs = 4000f) =>
            ShowToast(
                new ToastConfig
                {
                    Title = title,
                    Description = desc,
                    Variant = v,
                    DurationMs = durationMs,
                }
            );

        public void ShowSuccessToast(string title, string desc = null, float ms = 4000f) => ShowToast(title, desc, ToastVariant.Success, ms);

        public void ShowErrorToast(string title, string desc = null, float ms = 4000f) => ShowToast(title, desc, ToastVariant.Error, ms);

        public void ShowWarningToast(string title, string desc = null, float ms = 4000f) => ShowToast(title, desc, ToastVariant.Warning, ms);

        public void ShowInfoToast(string title, string desc = null, float ms = 4000f) => ShowToast(title, desc, ToastVariant.Info, ms);

        public void DismissToast(string id, bool animate = true) => Execute(() => _toast.Dismiss(id, animate), nameof(DismissToast));

        public void DismissAllToasts(bool animate = true) => Execute(() => _toast.DismissAll(animate), nameof(DismissAllToasts));

        public int GetActiveToastCount() => _toast.GetActiveToastCount();

        // Card
        public void Card(CardConfig cfg) => Execute(() => _card.DrawCard(cfg), nameof(Card));

        public void Card(string title, string desc, string content, Action footer = null, float width = -1, float height = -1) => Execute(() => _card.DrawCard(title, desc, content, footer, width, height), nameof(Card));

        public void CardWithImage(Texture2D img, string title, string desc, string content, Action footer = null, float width = -1, float height = -1) => Execute(() => _card.DrawCardWithImage(img, title, desc, content, footer, width, height), nameof(CardWithImage));

        public void CardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footer = null, float width = -1, float height = -1) => Execute(() => _card.DrawCardWithAvatar(avatar, title, subtitle, content, footer, width, height), nameof(CardWithAvatar));

        public void SimpleCard(string content, float width = -1, float height = -1) => Execute(() => _card.DrawSimpleCard(content, width, height), nameof(SimpleCard));

        public void BeginCard(float width = -1, float height = -1) => Execute(() => _card.BeginCard(width, height), nameof(BeginCard));

        public void BeginCard(float width, float height, ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => Execute(() => _card.BeginCard(width, height, variant, size, appearance), nameof(BeginCard));

        public void EndCard() => Execute(_card.EndCard, nameof(EndCard));

        public void CardHeader(Action content) => Execute(() => _card.CardHeader(content), nameof(CardHeader));

        public void CardTitle(string title, ComponentAppearance appearance = null) => Execute(() => _card.CardTitle(title, appearance), nameof(CardTitle));

        public void CardDescription(string d) => Execute(() => _card.CardDescription(d), nameof(CardDescription));

        public void CardContent(Action content) => Execute(() => _card.CardContent(content), nameof(CardContent));

        public void CardFooter(Action content) => Execute(() => _card.CardFooter(content), nameof(CardFooter));

        public void StatCard(string label, string value, string delta = null, float width = -1)
        {
            BeginCard(width);
            CardContent(() =>
            {
                MutedLabel(label);
                Label(value);
                if (delta != null)
                    Caption(delta);
            });
            EndCard();
        }

        // Separator
        public void Separator(SeparatorConfig cfg) => Execute(() => _separator.DrawSeparator(cfg), nameof(Separator));

        public void Separator(SeparatorOrientation o = SeparatorOrientation.Horizontal, bool decorative = true, ComponentAppearance appearance = null, params GUILayoutOption[] opts) => Execute(() => _separator.DrawSeparator(o, decorative, appearance, opts), nameof(Separator));

        public void HorizontalSeparator(params GUILayoutOption[] opts) => Separator(SeparatorOrientation.Horizontal, true, null, opts);

        public void VerticalSeparator(params GUILayoutOption[] opts) => Separator(SeparatorOrientation.Vertical, true, null, opts);

        public void LabeledSeparator(string text, params GUILayoutOption[] opts) => Separator(new SeparatorConfig { Text = text, LayoutOptions = opts ?? Array.Empty<GUILayoutOption>() });

        public void SeparatorWithSpacing(SeparatorOrientation o, float before, float after, params GUILayoutOption[] opts) =>
            Separator(
                new SeparatorConfig
                {
                    Orientation = o,
                    SpacingBefore = before,
                    SpacingAfter = after,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        // Tabs
        public int Tabs(TabsConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _tabs.Draw(cfg), 0, nameof(Tabs));
            string key = cfg.TabLabels?.Length > 0 ? string.Join("|", cfg.TabLabels) : null;
            return ExecStatefulInt(
                nameof(Tabs),
                s =>
                {
                    cfg.SelectedIndex = s;
                    return _tabs.Draw(cfg);
                },
                cfg.SelectedIndex,
                key
            );
        }

        public int Tabs(TabsConfig cfg, ref int idx)
        {
            if (cfg != null)
                cfg.SelectedIndex = idx;
            idx = Tabs(cfg);
            return idx;
        }

        public int Tabs(string[] tabNames, int idx, Action content = null, Action<int> onTabChange = null, int maxLines = 1, TabPosition position = TabPosition.Top, IndicatorStyle indicatorStyle = IndicatorStyle.Underline, bool overflowScroll = false, ComponentAppearance appearance = null) =>
            Tabs(
                new TabsConfig(tabNames, idx)
                {
                    Content = content,
                    OnSelectionChanged = onTabChange,
                    MaxLines = maxLines,
                    Position = position,
                    IndicatorStyle = indicatorStyle,
                    EnableOverflowScroll = overflowScroll,
                    Appearance = appearance,
                }
            );

        public int Tabs(string[] tabNames, ref int idx, Action content = null, Action<int> onTabChange = null, int maxLines = 1, TabPosition position = TabPosition.Top, IndicatorStyle indicatorStyle = IndicatorStyle.Underline, bool overflowScroll = false, ComponentAppearance appearance = null)
        {
            idx = Tabs(tabNames, idx, content, onTabChange, maxLines, position, indicatorStyle, overflowScroll, appearance);
            return idx;
        }

        public int TabsWithContent(string[] tabNames, int idx, Action content, Action<int> onTabChange = null, int maxLines = 1) => Tabs(tabNames, idx, content, onTabChange, maxLines);

        public int TabsWithContent(string[] tabNames, ref int idx, Action content, Action<int> onTabChange = null, int maxLines = 1)
        {
            idx = TabsWithContent(tabNames, idx, content, onTabChange, maxLines);
            return idx;
        }

        public int VerticalTabs(string[] tabNames, int idx, Action content = null, Action<int> onTabChange = null, float tabWidth = 140f, int maxLines = 1, TabSide side = TabSide.Left, IndicatorStyle style = IndicatorStyle.Background) =>
            Tabs(
                new TabsConfig(tabNames, idx)
                {
                    Content = content,
                    OnSelectionChanged = onTabChange,
                    MaxLines = maxLines,
                    Position = side == TabSide.Right ? TabPosition.Right : TabPosition.Left,
                    TabWidth = tabWidth,
                    IndicatorStyle = style,
                }
            );

        public int VerticalTabs(string[] tabNames, ref int idx, Action content = null, Action<int> onTabChange = null, float tabWidth = 140f, int maxLines = 1, TabSide side = TabSide.Left, IndicatorStyle style = IndicatorStyle.Background)
        {
            idx = VerticalTabs(tabNames, idx, content, onTabChange, tabWidth, maxLines, side, style);
            return idx;
        }

        public int ClosableTabs(ref string[] tabNames, ref bool[] closable, int idx, Action content = null, Action<int> onTabChange = null)
        {
            try
            {
                return _tabs.DrawWithAutoClose(ref tabNames, ref closable, idx, content, onTabChange);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(ClosableTabs), nameof(GUIHelper));
                return idx;
            }
        }

        public void BeginTabContent(params GUILayoutOption[] opts) => Execute(() => _tabs.BeginTabContent(opts), nameof(BeginTabContent));

        public void EndTabContent() => Execute(_tabs.EndTabContent, nameof(EndTabContent));

        // MenuBar
        public void MenuBar(MenuBar.MenuBarConfig cfg) => Execute(() => _menuBar.Draw(cfg), nameof(MenuBar));

        public void MenuBar(List<MenuBar.MenuItem> items, params GUILayoutOption[] opts) => Execute(() => _menuBar.Draw(items, opts), nameof(MenuBar));

        public void CloseMenuBarDropdown() => Execute(_menuBar.CloseDropdown, nameof(CloseMenuBarDropdown));

        // Table
        public void Table(TableConfig cfg) => Execute(() => _table.DrawTable(cfg), nameof(Table));

        public void Table(string[] headers, string[,] data, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _table.DrawTable(headers, data, v, sz, opts), nameof(Table));

        public void Table(Rect rect, string[] headers, string[,] data, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default) => Execute(() => _table.DrawTable(rect, headers, data, v, sz), nameof(Table));

        public void SortableTable(TableConfig cfg) => Execute(() => _table.SortableTable(cfg), nameof(SortableTable));

        public void SortableTable(string[] headers, string[,] data, ref int[] sortCols, ref bool[] sortAsc, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] opts) =>
            _table.SortableTable(headers, data, ref sortCols, ref sortAsc, v, sz, onSort, opts);

        public void SelectableTable(TableConfig cfg) => Execute(() => _table.SelectableTable(cfg), nameof(SelectableTable));

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selected, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<int, bool> onSelChange = null, params GUILayoutOption[] opts) =>
            _table.SelectableTable(headers, data, ref selected, v, sz, onSelChange, opts);

        public void PaginatedTable(TableConfig cfg) => Execute(() => _table.PaginatedTable(cfg), nameof(PaginatedTable));

        public void PaginatedTable(string[] headers, string[,] data, ref int page, int pageSize, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] opts) =>
            _table.PaginatedTable(headers, data, ref page, pageSize, v, sz, onPageChange, opts);

        public void SearchableTable(TableConfig cfg) => Execute(() => _table.SearchableTable(cfg), nameof(SearchableTable));

        public void SearchableTable(string[] headers, string[,] data, ref string query, ref string[,] filtered, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] opts) =>
            _table.SearchableTable(headers, data, ref query, ref filtered, v, sz, onSearch, opts);

        public void ResizableTable(TableConfig cfg) => Execute(() => _table.ResizableTable(cfg), nameof(ResizableTable));

        public void ResizableTable(string[] headers, string[,] data, ref float[] colWidths, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => _table.ResizableTable(headers, data, ref colWidths, v, sz, opts);

        public void CustomTable(TableConfig cfg) => Execute(() => _table.CustomTable(cfg), nameof(CustomTable));

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) =>
            Execute(() => _table.CustomTable(headers, data, cellRenderer, v, sz, opts), nameof(CustomTable));

        // Navigation
        public int Sidebar(string[] labels, int idx, string[] icons = null, string logo = "U", Action<int> onChanged = null, float width = 70f) =>
            ExecStatefulInt(nameof(Sidebar), s => _navigation.DrawSidebar(labels, s, icons, logo, onChanged, width), idx, labels?.Length > 0 ? string.Join("|", labels) : null);

        public int Sidebar(string[] labels, ref int idx, string[] icons = null, string logo = "U", Action<int> onChanged = null, float width = 70f)
        {
            idx = Sidebar(labels, idx, icons, logo, onChanged, width);
            return idx;
        }

        public int Navigation(NavigationConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _navigation.Draw(cfg), 0, nameof(Navigation));
            string key = cfg.Items?.Length > 0 ? string.Join("|", Array.ConvertAll(cfg.Items, i => i?.Id ?? i?.Label ?? "")) : null;
            return ExecStatefulInt(
                nameof(Navigation),
                s =>
                {
                    cfg.SelectedIndex = s;
                    return _navigation.Draw(cfg);
                },
                cfg.SelectedIndex,
                key
            );
        }

        public int Navigation(NavigationConfig cfg, ref int idx)
        {
            if (cfg != null)
                cfg.SelectedIndex = idx;
            idx = Navigation(cfg);
            return idx;
        }

        // Calendar
        public void Calendar(CalendarConfig cfg = null) =>
            Execute(
                () =>
                {
                    _calendar.DrawCalendar(cfg);
                },
                nameof(Calendar)
            );

        // DatePicker
        public DateTime? DatePicker(DatePickerConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _datePicker.DrawDatePicker(cfg), null, nameof(DatePicker));
            return ExecStatefulDate(
                nameof(DatePicker),
                s =>
                {
                    cfg.SelectedDate = s;
                    return _datePicker.DrawDatePicker(cfg);
                },
                cfg.SelectedDate,
                cfg.Id
            );
        }

        public DateTime? DatePicker(DatePickerConfig cfg, ref DateTime? date)
        {
            if (cfg != null)
                cfg.SelectedDate = date;
            date = DatePicker(cfg);
            return date;
        }

        public DateTime? DatePicker(string placeholder, DateTime? date, string id = "datepicker", params GUILayoutOption[] opts) =>
            DatePicker(
                new DatePickerConfig
                {
                    Placeholder = placeholder,
                    SelectedDate = date,
                    Id = id,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public DateTime? DatePicker(string placeholder, ref DateTime? date, string id = "datepicker", params GUILayoutOption[] opts)
        {
            date = DatePicker(placeholder, date, id, opts);
            return date;
        }

        public DateTime? DatePicker(string placeholder, DateTime? date, DateTime? min, DateTime? max, string id = "datepicker", params GUILayoutOption[] opts) =>
            DatePicker(
                new DatePickerConfig
                {
                    Placeholder = placeholder,
                    SelectedDate = date,
                    MinDate = min,
                    MaxDate = max,
                    Id = id,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public DateTime? DatePicker(string placeholder, ref DateTime? date, DateTime? min, DateTime? max, string id = "datepicker", params GUILayoutOption[] opts)
        {
            date = DatePicker(placeholder, date, min, max, id, opts);
            return date;
        }

        public DateTime? LabeledDatePicker(DatePickerConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _datePicker.DrawDatePickerWithLabel(cfg), null, nameof(LabeledDatePicker));
            return ExecStatefulDate(
                nameof(LabeledDatePicker),
                s =>
                {
                    cfg.SelectedDate = s;
                    return _datePicker.DrawDatePickerWithLabel(cfg);
                },
                cfg.SelectedDate,
                cfg.Id
            );
        }

        public DateTime? LabeledDatePicker(DatePickerConfig cfg, ref DateTime? date)
        {
            if (cfg != null)
                cfg.SelectedDate = date;
            date = LabeledDatePicker(cfg);
            return date;
        }

        public DateTime? LabeledDatePicker(string label, string placeholder, DateTime? date, string id = "datepicker", params GUILayoutOption[] opts) =>
            LabeledDatePicker(
                new DatePickerConfig
                {
                    Label = label,
                    Placeholder = placeholder,
                    SelectedDate = date,
                    Id = id,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public DateTime? LabeledDatePicker(string label, string placeholder, ref DateTime? date, string id = "datepicker", params GUILayoutOption[] opts)
        {
            date = LabeledDatePicker(label, placeholder, date, id, opts);
            return date;
        }

        public DateTime? LabeledDatePicker(string label, string placeholder, DateTime? date, DateTime? min, DateTime? max, string id = "datepicker", params GUILayoutOption[] opts) =>
            LabeledDatePicker(
                new DatePickerConfig
                {
                    Label = label,
                    Placeholder = placeholder,
                    SelectedDate = date,
                    MinDate = min,
                    MaxDate = max,
                    Id = id,
                    LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public DateTime? LabeledDatePicker(string label, string placeholder, ref DateTime? date, DateTime? min, DateTime? max, string id = "datepicker", params GUILayoutOption[] opts)
        {
            date = LabeledDatePicker(label, placeholder, date, min, max, id, opts);
            return date;
        }

        public DateTime? DateRangePicker(string placeholder, DateTime? start, DateTime? end, string id = "daterange", params GUILayoutOption[] opts) => Execute(() => _datePicker.DrawDateRangePicker(placeholder, start, end, id, opts), start, nameof(DateRangePicker));

        public DateTime? DateRangePicker(string placeholder, ref DateTime? start, DateTime? end, string id = "daterange", params GUILayoutOption[] opts)
        {
            start = DateRangePicker(placeholder, start, end, id, opts);
            return start;
        }

        public DateTime? DateRangePicker(string placeholder, DateTime? start, DateTime? end, DateTime? min, DateTime? max, string id = "daterange", params GUILayoutOption[] opts) =>
            Execute(
                () =>
                    _datePicker.DrawDateRangePicker(
                        new DatePickerConfig
                        {
                            Placeholder = placeholder,
                            StartDate = start,
                            EndDate = end,
                            MinDate = min,
                            MaxDate = max,
                            Id = id,
                            LayoutOptions = opts ?? Array.Empty<GUILayoutOption>(),
                        }
                    ),
                start,
                nameof(DateRangePicker)
            );

        public DateTime? DateRangePicker(string placeholder, ref DateTime? start, DateTime? end, DateTime? min, DateTime? max, string id = "daterange", params GUILayoutOption[] opts)
        {
            start = DateRangePicker(placeholder, start, end, min, max, id, opts);
            return start;
        }

        public void CloseDatePicker(string id) => Execute(() => _datePicker.CloseDatePicker(id), nameof(CloseDatePicker));

        public bool IsDatePickerOpen(string id) => Execute(() => _datePicker.IsDatePickerOpen(id), false, nameof(IsDatePickerOpen));

        // DataTable
        public void DataTable(string id, List<DataTableColumn> cols, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColToggle = false, params GUILayoutOption[] opts) =>
            Execute(() => _dataTable.DrawDataTable(id, cols, data, showPagination, showSearch, showSelection, showColToggle, null, opts), nameof(DataTable));

        public void DataTable(string id, List<DataTableColumn> cols, List<DataTableRow> data, ComponentAppearance appearance, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColToggle = false, params GUILayoutOption[] opts) =>
            Execute(() => _dataTable.DrawDataTable(id, cols, data, showPagination, showSearch, showSelection, showColToggle, appearance, opts), nameof(DataTable));

        public DataTableState GetDataTableState(string id) => Execute(() => _dataTable.GetTableState(id), null, nameof(GetDataTableState));

        public void SetDataTablePageSize(string id, int size) => Execute(() => _dataTable.SetPageSize(id, size), nameof(SetDataTablePageSize));

        public void ClearDataTableSelection(string id) => Execute(() => _dataTable.ClearSelection(id), nameof(ClearDataTableSelection));

        public List<string> GetSelectedDataTableRows(string id) => Execute(() => _dataTable.GetSelectedRows(id), new List<string>(), nameof(GetSelectedDataTableRows));

        // Compound helpers
        public void KeyValueRow(string key, string value, float spacing = 8f)
        {
            BeginHorizontalGroup();
            MutedLabel(key);
            AddSpace(spacing);
            Label(value);
            EndHorizontalGroup();
        }

        public void ErrorAlert(string message, params GUILayoutOption[] opts)
        {
            BeginCard(-1f, -1f);
            CardContent(() => DestructiveLabel(message, opts));
            EndCard();
        }

        public void Disabled(bool isDisabled, Action draw)
        {
            var prev = GUI.enabled;
            GUI.enabled = !isDisabled;
            try
            {
                draw?.Invoke();
            }
            finally
            {
                GUI.enabled = prev;
            }
        }

        // State internals
        private int GetStateId(string prefix, string key = null)
        {
            string k = string.IsNullOrEmpty(key) ? prefix : $"{prefix}:{key}";
            return GUIUtility.GetControlID(new GUIContent(k), FocusType.Passive);
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

        private string GetScopedStateKey(string leaf = null)
        {
            if (_stateScopes.Count == 0)
                return leaf;

            var scopes = _stateScopes.ToArray();
            Array.Reverse(scopes);
            string prefix = string.Join("/", scopes);
            return string.IsNullOrEmpty(leaf) ? prefix : $"{prefix}:{leaf}";
        }

        private float GetFloatState(int id, float v)
        {
            if (!_floatState.TryGetValue(id, out _))
            {
                _floatState[id] = v;
                _floatInput[id] = v;
                return v;
            }
            if (_floatInput.TryGetValue(id, out float li) && Mathf.Abs(v - li) > 0.0001f)
                _floatState[id] = v;
            _floatInput[id] = v;
            return _floatState[id];
        }

        private void SetFloatState(int id, float v) => _floatState[id] = v;

        private int GetIntState(int id, int v)
        {
            if (!_intState.TryGetValue(id, out _))
            {
                _intState[id] = v;
                _intInput[id] = v;
                return v;
            }
            if (_intInput.TryGetValue(id, out int li) && v != li)
                _intState[id] = v;
            _intInput[id] = v;
            return _intState[id];
        }

        private void SetIntState(int id, int v) => _intState[id] = v;

        private bool GetBoolState(int id, bool v)
        {
            if (!_boolState.TryGetValue(id, out _))
            {
                _boolState[id] = v;
                _boolInput[id] = v;
                return v;
            }
            if (_boolInput.TryGetValue(id, out bool li) && v != li)
                _boolState[id] = v;
            _boolInput[id] = v;
            return _boolState[id];
        }

        private void SetBoolState(int id, bool v) => _boolState[id] = v;

        private string GetStringState(int id, string v)
        {
            string n = v ?? string.Empty;
            if (!_stringState.TryGetValue(id, out _))
            {
                _stringState[id] = n;
                _stringInput[id] = n;
                return n;
            }
            if (_stringInput.TryGetValue(id, out string li) && !string.Equals(n, li, StringComparison.Ordinal))
                _stringState[id] = n;
            _stringInput[id] = n;
            return _stringState[id];
        }

        private void SetStringState(int id, string v) => _stringState[id] = v ?? string.Empty;

        private Vector2 GetV2State(int id, Vector2 v)
        {
            if (!_v2State.TryGetValue(id, out _))
            {
                _v2State[id] = v;
                _v2Input[id] = v;
                return v;
            }
            if (_v2Input.TryGetValue(id, out Vector2 li) && (v - li).sqrMagnitude > 1e-6f)
                _v2State[id] = v;
            _v2Input[id] = v;
            return _v2State[id];
        }

        private void SetV2State(int id, Vector2 v) => _v2State[id] = v;

        private DateTime? GetDateState(int id, DateTime? v)
        {
            if (!_dateState.TryGetValue(id, out _))
            {
                _dateState[id] = v;
                _dateInput[id] = v;
                return v;
            }
            if (_dateInput.TryGetValue(id, out DateTime? li) && v != li)
                _dateState[id] = v;
            _dateInput[id] = v;
            return _dateState[id];
        }

        private void SetDateState(int id, DateTime? v) => _dateState[id] = v;

        // Stateful execute helpers
        private float ExecStatefulFloat(string p, Func<float, float> d, float v, string k = null)
        {
            int id = GetStateId(p, k);
            float s = GetFloatState(id, v);
            float r = Execute(() => d(s), s, p);
            SetFloatState(id, r);
            return r;
        }

        private int ExecStatefulInt(string p, Func<int, int> d, int v, string k = null)
        {
            int id = GetStateId(p, k);
            int s = GetIntState(id, v);
            int r = Execute(() => d(s), s, p);
            SetIntState(id, r);
            return r;
        }

        private bool ExecStatefulBool(string p, Func<bool, bool> d, bool v, string k = null)
        {
            int id = GetStateId(p, k);
            bool s = GetBoolState(id, v);
            bool r = Execute(() => d(s), s, p);
            SetBoolState(id, r);
            return r;
        }

        private string ExecStatefulStr(string p, Func<string, string> d, string v, string k = null)
        {
            int id = GetStateId(p, k);
            string s = GetStringState(id, v);
            string r = Execute(() => d(s), s, p);
            SetStringState(id, r);
            return r;
        }

        private Vector2 ExecStatefulV2(string p, Func<Vector2, Vector2> d, Vector2 v, string k = null)
        {
            int id = GetStateId(p, k);
            Vector2 s = GetV2State(id, v);
            Vector2 r = Execute(() => d(s), s, p);
            SetV2State(id, r);
            return r;
        }

        private DateTime? ExecStatefulDate(string p, Func<DateTime?, DateTime?> d, DateTime? v, string k = null)
        {
            int id = GetStateId(p, k);
            DateTime? s = GetDateState(id, v);
            DateTime? r = Execute(() => d(s), s, p);
            SetDateState(id, r);
            return r;
        }

        // Execute guards
        private void Execute(Action action, string op)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, op, nameof(GUIHelper));
            }
        }

        private T Execute<T>(Func<T> action, T fallback, string op)
        {
            try
            {
                return action != null ? action() : fallback;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, op, nameof(GUIHelper));
                return fallback;
            }
        }

        internal Rect GetRootGuiScreenRect()
        {
            if (!_rootGuiScreenRectValid)
            {
                _rootGuiScreenRect = CaptureGuiScreenRect();
                _rootGuiScreenRectValid = true;
            }
            return _rootGuiScreenRect;
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
                if (topRectMethod != null)
                {
                    object val = topRectMethod.Invoke(null, null);
                    if (val is Rect topRect && topRect.width > 1f && topRect.height > 1f)
                        return topRect;
                }

                var prop = guiClip?.GetProperty("visibleRect", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (prop != null)
                {
                    object value = prop.GetValue(null, null);
                    if (value is Rect rect && rect.width > 1f && rect.height > 1f)
                        return rect;
                }
            }
            catch
            {
                // ignored
            }

            return new Rect(0f, 0f, Screen.width, Screen.height);
        }
    }
}
