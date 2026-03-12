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

        internal int fontSize = 14;
        public float uiScale = 1f;

        private bool _scrollbarsInitialized;
        private int _lastCheckFrame = -10;

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
                    LayerManager.Instance.DrawLayers();
                    _toast.DrawToasts();
                    _tooltip.FlushAndDraw(new Rect(0f, 0f, Screen.width, Screen.height));
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
        }

        public void Dispose() => Cleanup();

        // Layout helpers
        public Vector2 ScrollView(Vector2 pos, Action draw, params GUILayoutOption[] opts) => ExecStatefulV2(nameof(ScrollView), s => _layout.DrawScrollView(s, draw, opts), pos);

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

        // Button
        public bool Button(ButtonConfig cfg) => Execute(() => _button.DrawButton(cfg), false, nameof(Button));

        public bool Button(string text, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] opts) =>
            Execute(() => _button.DrawButton(text, v, sz, onClick, disabled, opacity, opts), false, nameof(Button));

        public bool Button(string text, Texture2D icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] opts) =>
            Execute(() => _button.DrawButton(text, icon, v, sz, onClick, disabled, opacity, opts), false, nameof(Button));

        public bool Button(string text, IconConfig icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] opts) =>
            Button(
                new ButtonConfig(text)
                {
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    OnClick = onClick,
                    Disabled = disabled,
                    Opacity = opacity,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool IconButton(IconConfig icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action onClick = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Button(
                new ButtonConfig(string.Empty)
                {
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    OnClick = onClick,
                    Disabled = disabled,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool LinkButton(string text, Action onClick = null, params GUILayoutOption[] opts) => Button(text, ControlVariant.Link, ControlSize.Default, onClick, false, 1f, opts);

        public void ButtonGroup(Action draw, bool horizontal = true, float spacing = 5f) => Execute(() => _button.ButtonGroup(draw, horizontal, spacing), nameof(ButtonGroup));

        // Input
        public string Input(InputConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _input.DrawInput(cfg), string.Empty, nameof(Input));
            int id = GetStateId(nameof(Input), cfg.Label ?? cfg.Placeholder);
            cfg.Value = GetStringState(id, cfg.Value);
            var r = Execute(() => _input.DrawInput(cfg), cfg.Value, nameof(Input));
            SetStringState(id, r);
            return r;
        }

        public string Input(string val, string placeholder = "", ControlVariant v = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            ExecStatefulStr(nameof(Input), s => _input.DrawInput(s, placeholder, v, disabled, focused, width, onChange), val, placeholder);

        public string Input(ref string val, string placeholder = "", ControlVariant v = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            val = Input(val, placeholder, v, disabled, focused, width, onChange);
            return val;
        }

        public string Input(string val, Texture2D icon, string placeholder = "", ControlVariant v = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            ExecStatefulStr(nameof(Input), s => _input.DrawInput(s, icon, placeholder, v, disabled, focused, width, onChange), val, placeholder);

        public string Input(ref string val, Texture2D icon, string placeholder = "", ControlVariant v = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            val = Input(val, icon, placeholder, v, disabled, focused, width, onChange);
            return val;
        }

        public string Input(string val, IconConfig icon, string placeholder = "", ControlVariant v = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null) =>
            Input(
                new InputConfig
                {
                    Value = val,
                    Icon = icon,
                    Placeholder = placeholder,
                    Variant = v,
                    Disabled = disabled,
                    Focused = focused,
                    Width = width,
                    OnChange = onChange,
                }
            );

        public string Input(ref string val, IconConfig icon, string placeholder = "", ControlVariant v = ControlVariant.Default, bool disabled = false, bool focused = false, int width = -1, Action<string> onChange = null)
        {
            val = Input(val, icon, placeholder, v, disabled, focused, width, onChange);
            return val;
        }

        public float NumericInput(float val, float min = float.MinValue, float max = float.MaxValue, string placeholder = "0", int width = -1, Action<float> onChange = null)
        {
            string str = ExecStatefulStr(nameof(NumericInput), s => _input.DrawInput(s, placeholder, ControlVariant.Default, false, false, width, null), val.ToString("G"), placeholder);
            if (float.TryParse(str, out float parsed))
            {
                parsed = Mathf.Clamp(parsed, min, max);
                onChange?.Invoke(parsed);
                return parsed;
            }
            return val;
        }

        public string SearchInput(string val, string placeholder = "Search...", int width = -1, Action<string> onChange = null) => Input(val, new IconConfig { }, placeholder, ControlVariant.Default, false, false, width, onChange);

        public string SearchInput(ref string val, string placeholder = "Search...", int width = -1, Action<string> onChange = null)
        {
            val = SearchInput(val, placeholder, width, onChange);
            return val;
        }

        public string LabeledInput(InputConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _input.DrawLabeledInput(cfg), string.Empty, nameof(LabeledInput));
            int id = GetStateId(nameof(LabeledInput), cfg.Label ?? cfg.Placeholder);
            cfg.Value = GetStringState(id, cfg.Value);
            var r = Execute(() => _input.DrawLabeledInput(cfg), cfg.Value, nameof(LabeledInput));
            SetStringState(id, r);
            return r;
        }

        public string LabeledInput(string label, string val, string placeholder = "", ControlVariant iv = ControlVariant.Default, ControlVariant lv = ControlVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null) =>
            ExecStatefulStr(nameof(LabeledInput), s => _input.DrawLabeledInput(label, s, placeholder, iv, lv, disabled, inputWidth, onChange), val, label);

        public string LabeledInput(ref string val, string label, string placeholder = "", ControlVariant iv = ControlVariant.Default, ControlVariant lv = ControlVariant.Default, bool disabled = false, int inputWidth = -1, Action<string> onChange = null)
        {
            val = LabeledInput(label, val, placeholder, iv, lv, disabled, inputWidth, onChange);
            return val;
        }

        public string LabeledInput(string label, string val, IconConfig icon, string placeholder = "", ControlVariant iv = ControlVariant.Default, ControlVariant lv = ControlVariant.Default, bool disabled = false, bool focused = false, int inputWidth = -1, Action<string> onChange = null) =>
            LabeledInput(
                new InputConfig
                {
                    Label = label,
                    Value = val,
                    Icon = icon,
                    Placeholder = placeholder,
                    Variant = iv,
                    LabelVariant = lv,
                    Disabled = disabled,
                    Focused = focused,
                    Width = inputWidth,
                    OnChange = onChange,
                }
            );

        public string LabeledInput(ref string val, string label, IconConfig icon, string placeholder = "", ControlVariant iv = ControlVariant.Default, ControlVariant lv = ControlVariant.Default, bool disabled = false, bool focused = false, int inputWidth = -1, Action<string> onChange = null)
        {
            val = LabeledInput(label, val, icon, placeholder, iv, lv, disabled, focused, inputWidth, onChange);
            return val;
        }

        public string PasswordField(InputConfig cfg) => Execute(() => _input.DrawPasswordField(cfg), cfg?.Value ?? string.Empty, nameof(PasswordField));

        public string PasswordField(string val, string label = "", char mask = '*', ControlVariant v = ControlVariant.Default, bool disabled = false, Action<string> onChange = null) =>
            ExecStatefulStr(
                nameof(PasswordField),
                s =>
                    PasswordField(
                        new InputConfig
                        {
                            Value = s,
                            Label = label,
                            MaskChar = mask,
                            Variant = v,
                            Disabled = disabled,
                            OnChange = onChange,
                        }
                    ),
                val,
                label
            );

        public string PasswordField(ref string val, string label = "", char mask = '*', ControlVariant v = ControlVariant.Default, bool disabled = false, Action<string> onChange = null)
        {
            val = PasswordField(val, label, mask, v, disabled, onChange);
            return val;
        }

        public string PasswordField(float windowWidth, string label, ref string password, char mask = '*')
        {
            try
            {
                return _input.DrawPasswordField(windowWidth, label, ref password, mask);
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

        // Toggle
        public bool Toggle(ToggleConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _toggle.DrawToggle(cfg), false, nameof(Toggle));
            return ExecStatefulBool(
                nameof(Toggle),
                s =>
                {
                    cfg.Value = s;
                    return _toggle.DrawToggle(cfg);
                },
                cfg.Value,
                string.IsNullOrEmpty(cfg.Text) ? null : cfg.Text
            );
        }

        public bool Toggle(string text, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            ExecStatefulBool(nameof(Toggle), s => _toggle.DrawToggle(text, s, v, sz, onToggle, disabled, opts), val, text);

        public bool Toggle(string text, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Toggle(text, val, v, sz, onToggle, disabled, opts);
            return val;
        }

        public bool Toggle(Rect rect, string text, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            ExecStatefulBool(nameof(Toggle) + "Rect", s => _toggle.DrawToggle(rect, text, s, v, sz, onToggle, disabled), val, text);

        public bool Toggle(Rect rect, string text, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            val = Toggle(rect, text, val, v, sz, onToggle, disabled);
            return val;
        }

        public bool Toggle(string text, IconConfig icon, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Toggle(
                new ToggleConfig
                {
                    Text = text,
                    Icon = icon,
                    Value = val,
                    Variant = v,
                    Size = sz,
                    OnToggle = onToggle,
                    Disabled = disabled,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Toggle(string text, IconConfig icon, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Toggle(text, icon, val, v, sz, onToggle, disabled, opts);
            return val;
        }

        // Checkbox
        public bool Checkbox(CheckboxConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _checkbox.DrawCheckbox(cfg), false, nameof(Checkbox));
            return ExecStatefulBool(
                nameof(Checkbox),
                s =>
                {
                    cfg.Value = s;
                    return _checkbox.DrawCheckbox(cfg);
                },
                cfg.Value,
                string.IsNullOrEmpty(cfg.Text) ? null : cfg.Text
            );
        }

        public bool Checkbox(string text, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            ExecStatefulBool(nameof(Checkbox), s => _checkbox.DrawCheckbox(text, s, v, sz, onToggle, disabled, opts), val, text);

        public bool Checkbox(string text, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Checkbox(text, val, v, sz, onToggle, disabled, opts);
            return val;
        }

        public bool Checkbox(string text, bool val, bool showCheckmark, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Checkbox(
                new CheckboxConfig
                {
                    Text = text,
                    Value = val,
                    Variant = v,
                    Size = sz,
                    OnToggle = onToggle,
                    Disabled = disabled,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                    ShowCheckmark = showCheckmark,
                }
            );

        public bool Checkbox(string text, ref bool val, bool showCheckmark, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Checkbox(text, val, showCheckmark, v, sz, onToggle, disabled, opts);
            return val;
        }

        public bool Checkbox(Rect rect, string text, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            ExecStatefulBool(nameof(Checkbox) + "Rect", s => _checkbox.DrawCheckbox(rect, text, s, v, sz, onToggle, disabled), val, text);

        public bool Checkbox(Rect rect, string text, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            val = Checkbox(rect, text, val, v, sz, onToggle, disabled);
            return val;
        }

        public bool Checkbox(string text, IconConfig icon, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Checkbox(
                new CheckboxConfig
                {
                    Text = text,
                    Icon = icon,
                    Value = val,
                    Variant = v,
                    Size = sz,
                    OnToggle = onToggle,
                    Disabled = disabled,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Checkbox(string text, IconConfig icon, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Checkbox(text, icon, val, v, sz, onToggle, disabled, opts);
            return val;
        }

        // Switch
        public bool Switch(SwitchConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _switch.DrawSwitch(cfg), false, nameof(Switch));
            return ExecStatefulBool(
                nameof(Switch),
                s =>
                {
                    cfg.Value = s;
                    return _switch.DrawSwitch(cfg);
                },
                cfg.Value,
                string.IsNullOrEmpty(cfg.Text) ? null : cfg.Text
            );
        }

        public bool Switch(string text, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            ExecStatefulBool(nameof(Switch), s => _switch.DrawSwitch(text, s, v, sz, onToggle, disabled, opts), val, text);

        public bool Switch(string text, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Switch(text, val, v, sz, onToggle, disabled, opts);
            return val;
        }

        public bool Switch(Rect rect, string text, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            ExecStatefulBool(nameof(Switch) + "Rect", s => _switch.DrawSwitch(rect, text, s, v, sz, onToggle, disabled), val, text);

        public bool Switch(Rect rect, string text, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            val = Switch(rect, text, val, v, sz, onToggle, disabled);
            return val;
        }

        public bool Switch(string text, IconConfig icon, bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts) =>
            Switch(
                new SwitchConfig
                {
                    Text = text,
                    Icon = icon,
                    Value = val,
                    Variant = v,
                    Size = sz,
                    OnToggle = onToggle,
                    Disabled = disabled,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public bool Switch(string text, IconConfig icon, ref bool val, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Switch(text, icon, val, v, sz, onToggle, disabled, opts);
            return val;
        }

        // Slider
        public float Slider(SliderConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _slider.Draw(cfg), 0f, nameof(Slider));
            return ExecStatefulFloat(
                nameof(Slider),
                s =>
                {
                    cfg.Value = s;
                    return _slider.Draw(cfg);
                },
                cfg.Value,
                string.IsNullOrEmpty(cfg.Label) ? null : cfg.Label
            );
        }

        public float Slider(float val, float min = 0f, float max = 1f, params GUILayoutOption[] opts) => ExecStatefulFloat(nameof(Slider), s => _slider.Draw(s, min, max, opts), val, null);

        public float Slider(float val, float min, float max, float step, params GUILayoutOption[] opts) => ExecStatefulFloat(nameof(Slider), s => _slider.Draw(s, min, max, step, opts), val, null);

        public float Slider(ref float val, float min = 0f, float max = 1f, params GUILayoutOption[] opts)
        {
            val = Slider(val, min, max, opts);
            return val;
        }

        public float Slider(ref float val, float min, float max, float step, params GUILayoutOption[] opts)
        {
            val = Slider(val, min, max, step, opts);
            return val;
        }

        public float Slider(float val, float min, float max, float step, Action<float> onChange, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            ExecStatefulFloat(
                nameof(Slider),
                s =>
                    _slider.Draw(
                        new SliderConfig
                        {
                            Value = s,
                            MinValue = min,
                            MaxValue = max,
                            Step = step,
                            OnChange = onChange,
                            Variant = v,
                            Size = sz,
                            Disabled = disabled,
                            Options = opts ?? Array.Empty<GUILayoutOption>(),
                        }
                    ),
                val,
                null
            );

        public float Slider(ref float val, float min, float max, float step, Action<float> onChange, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, params GUILayoutOption[] opts)
        {
            val = Slider(val, min, max, step, onChange, v, sz, disabled, opts);
            return val;
        }

        public float LabeledSlider(string label, float val, float min, float max, bool showValue = true, params GUILayoutOption[] opts) => ExecStatefulFloat(nameof(LabeledSlider), s => _slider.LabeledSlider(label, s, min, max, showValue, opts), val, label);

        public float LabeledSlider(string label, float val, float min, float max, float step, bool showValue = true, params GUILayoutOption[] opts) => ExecStatefulFloat(nameof(LabeledSlider), s => _slider.LabeledSlider(label, s, min, max, step, showValue, opts), val, label);

        public float LabeledSlider(string label, ref float val, float min, float max, bool showValue = true, params GUILayoutOption[] opts)
        {
            val = LabeledSlider(label, val, min, max, showValue, opts);
            return val;
        }

        public float LabeledSlider(string label, ref float val, float min, float max, float step, bool showValue = true, params GUILayoutOption[] opts)
        {
            val = LabeledSlider(label, val, min, max, step, showValue, opts);
            return val;
        }

        public float LabeledSlider(string label, float val, float min, float max, float step, Action<float> onChange, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, bool showValue = true, string fmt = "F2", params GUILayoutOption[] opts) =>
            ExecStatefulFloat(
                nameof(LabeledSlider),
                s =>
                    _slider.Draw(
                        new SliderConfig
                        {
                            Label = label,
                            Value = s,
                            MinValue = min,
                            MaxValue = max,
                            Step = step,
                            OnChange = onChange,
                            Variant = v,
                            Size = sz,
                            Disabled = disabled,
                            ShowValue = showValue,
                            ValueFormat = fmt,
                            Options = opts ?? Array.Empty<GUILayoutOption>(),
                        }
                    ),
                val,
                label
            );

        public float LabeledSlider(string label, ref float val, float min, float max, float step, Action<float> onChange, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, bool disabled = false, bool showValue = true, string fmt = "F2", params GUILayoutOption[] opts)
        {
            val = LabeledSlider(label, val, min, max, step, onChange, v, sz, disabled, showValue, fmt, opts);
            return val;
        }

        public float DisabledSlider(float val, float min = 0f, float max = 1f, params GUILayoutOption[] opts) => Execute(() => _slider.DisabledSlider(val, min, max, opts), val, nameof(DisabledSlider));

        public (float min, float max) RangeSlider(float minVal, float maxVal, float absMin, float absMax, string label = null, Action<float, float> onChange = null, params GUILayoutOption[] opts)
        {
            float lo = ExecStatefulFloat(
                nameof(RangeSlider) + "Min",
                s =>
                    _slider.Draw(
                        new SliderConfig
                        {
                            Value = s,
                            MinValue = absMin,
                            MaxValue = maxVal,
                            Step = 0f,
                            Options = opts ?? Array.Empty<GUILayoutOption>(),
                        }
                    ),
                minVal,
                label + "lo"
            );
            float hi = ExecStatefulFloat(
                nameof(RangeSlider) + "Max",
                s =>
                    _slider.Draw(
                        new SliderConfig
                        {
                            Value = s,
                            MinValue = lo,
                            MaxValue = absMax,
                            Step = 0f,
                            Options = opts ?? Array.Empty<GUILayoutOption>(),
                        }
                    ),
                maxVal,
                label + "hi"
            );
            if (lo > hi)
                lo = hi;
            if (!string.IsNullOrEmpty(label))
                Label(label);
            onChange?.Invoke(lo, hi);
            return (lo, hi);
        }

        // Select
        public int Select(SelectConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _select.DrawSelect(cfg), 0, nameof(Select));
            string key = cfg.Items?.Length > 0 ? string.Join("|", cfg.Items) : null;
            return ExecStatefulInt(
                nameof(Select),
                s =>
                {
                    cfg.SelectedIndex = s;
                    return _select.DrawSelect(cfg);
                },
                cfg.SelectedIndex,
                key
            );
        }

        public int Select(string[] items, int idx) => ExecStatefulInt(nameof(Select), s => _select.DrawSelect(items, s), idx, null);

        public int Select(string[] items, ref int idx)
        {
            idx = Select(items, idx);
            return idx;
        }

        public int Select(string[] items, int idx, Action<int> onChange) =>
            Select(
                new SelectConfig
                {
                    Items = items ?? Array.Empty<string>(),
                    SelectedIndex = idx,
                    OnChange = onChange,
                }
            );

        public int Select(string[] items, ref int idx, Action<int> onChange)
        {
            idx = Select(items, idx, onChange);
            return idx;
        }

        public void OpenSelect(string id = "select") => Execute(() => _select.Open(id), nameof(OpenSelect));

        public void CloseSelect() => Execute(_select.Close, nameof(CloseSelect));

        public bool IsSelectOpen() => _select.IsOpen;

        // Dropdown menu
        public void DropdownMenu(DropdownMenuConfig cfg) => Execute(() => _dropdownMenu.Draw(cfg), nameof(DropdownMenu));

        public void DropdownMenu(List<DropdownMenuItem> items, int zIndex = -1, params GUILayoutOption[] opts) => DropdownMenu(new DropdownMenuConfig(items) { ZIndex = zIndex >= 0 ? zIndex : DesignTokens.ZIndex.Dropdown, Options = opts ?? Array.Empty<GUILayoutOption>() });

        public void OpenDropdownMenu(List<DropdownMenuItem> items, string id = "dropdown", int zIndex = -1) => Execute(() => _dropdownMenu.Open(items, id, zIndex), nameof(OpenDropdownMenu));

        public void CloseDropdownMenu() => Execute(_dropdownMenu.Close, nameof(CloseDropdownMenu));

        public bool IsDropdownMenuOpen() => Execute(() => _dropdownMenu.IsOpen, false, nameof(IsDropdownMenuOpen));

        public int GetDropdownMenuZIndex() => Execute(() => _dropdownMenu.GetZIndex(), DesignTokens.ZIndex.Dropdown, nameof(GetDropdownMenuZIndex));

        // Theme changer
        public void ThemeChanger(ThemeChangerConfig cfg = null) => Execute(() => _themeChanger.Draw(cfg), nameof(ThemeChanger));

        public void ThemeChangerCompact(string id = "theme_compact") => Execute(() => _themeChanger.DrawCompact(id), nameof(ThemeChangerCompact));

        public void ThemeChangerWithPreview(string id = "theme_preview", float width = 220f) => Execute(() => _themeChanger.DrawWithPreview(id, width), nameof(ThemeChangerWithPreview));

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
                    cfg.Text = s;
                    return _textArea.DrawTextArea(cfg);
                },
                cfg.Text,
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

        // Label
        public void Label(LabelConfig cfg) => Execute(() => _label.DrawLabel(cfg), nameof(Label));

        public void Label(string text, ControlVariant v = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] opts) => Execute(() => _label.DrawLabel(text, v, disabled, opts), nameof(Label));

        public void Label(Rect rect, string text, ControlVariant v = ControlVariant.Default, bool disabled = false) => Execute(() => _label.DrawLabel(rect, text, v, disabled), nameof(Label));

        public void Label(string text, IconConfig icon, ControlVariant v = ControlVariant.Default, bool disabled = false, params GUILayoutOption[] opts) =>
            Label(
                new LabelConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = v,
                    Disabled = disabled,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void MutedLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Muted, false, opts);

        public void SecondaryLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Secondary, false, opts);

        public void DestructiveLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Destructive, false, opts);

        public void Heading(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Default, false, opts);

        public void Caption(string text, params GUILayoutOption[] opts) => MutedLabel(text, opts);

        public void CodeLabel(string text, params GUILayoutOption[] opts) => Label(text, ControlVariant.Secondary, false, opts);

        // Badge
        public void Badge(BadgeConfig cfg) => Execute(() => _badge.DrawBadge(cfg), nameof(Badge));

        public void Badge(string text, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.DrawBadge(text, v, sz, opts), nameof(Badge));

        public void Badge(string text, IconConfig icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) =>
            Badge(
                new BadgeConfig
                {
                    Text = text,
                    Icon = icon,
                    Variant = v,
                    Size = sz,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void BadgeWithIcon(string text, Texture2D icon, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.BadgeWithIcon(text, icon, v, sz, opts), nameof(BadgeWithIcon));

        public void CountBadge(int count, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] opts) => Execute(() => _badge.CountBadge(count, v, sz, maxCount, opts), nameof(CountBadge));

        public void StatusBadge(string text, bool isActive, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.StatusBadge(text, isActive, v, sz, opts), nameof(StatusBadge));

        public void ProgressBadge(string text, float progress, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) => Execute(() => _badge.ProgressBadge(text, progress, v, sz, opts), nameof(ProgressBadge));

        public void RoundedBadge(string text, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, float cornerRadius = DesignTokens.Radius.XL, params GUILayoutOption[] opts) => Execute(() => _badge.RoundedBadge(text, v, sz, cornerRadius, opts), nameof(RoundedBadge));

        public void AnimatedBadge(string text, string animId = null, ControlVariant v = ControlVariant.Default, ControlSize sz = ControlSize.Default, params GUILayoutOption[] opts) =>
            Execute(
                () =>
                {
                    if (string.IsNullOrEmpty(animId))
                        _badge.AnimatedBadge(text, v, sz, opts);
                    else
                        _badge.AnimatedBadge(text, animId, v, sz, opts);
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

        public void Progress(float val, float width = -1, float height = -1, params GUILayoutOption[] opts) => Execute(() => _progress.DrawProgress(val, width, height, opts), nameof(Progress));

        public void Progress(Rect rect, float val) => Execute(() => _progress.DrawProgress(rect, val), nameof(Progress));

        public void LabeledProgress(string label, float val, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] opts) => Execute(() => _progress.LabeledProgress(label, val, width, height, showPercentage, opts), nameof(LabeledProgress));

        public void CircularProgress(float val, float size = DesignTokens.Height.Small, params GUILayoutOption[] opts) => Execute(() => _progress.CircularProgress(val, size, opts), nameof(CircularProgress));

        public void AnimatedProgress(string id, float val, float width = -1, float height = -1, params GUILayoutOption[] opts) => Execute(() => _progress.AnimatedProgress(id, val, width, height, opts), nameof(AnimatedProgress));

        public void IndeterminateProgress(string id, float width = -1, float height = -1, params GUILayoutOption[] opts) => AnimatedProgress(id, Mathf.PingPong(Time.realtimeSinceStartup * 0.5f, 1f), width, height, opts);

        // Chart
        public void Chart(ChartConfig cfg) => Execute(() => _chart.DrawChart(cfg), nameof(Chart));

        // Dialog
        public void OpenDialog(string id) => Execute(() => _dialog.Open(id), nameof(OpenDialog));

        public void CloseDialog() => Execute(_dialog.Close, nameof(CloseDialog));

        public void Dialog(DialogConfig cfg) => Execute(() => _dialog.DrawDialog(cfg), nameof(Dialog));

        public void Dialog(string id, Action content, float width = 400f, float height = 300f) => Execute(() => _dialog.DrawDialog(id, content, width, height), nameof(Dialog));

        public void Dialog(string id, string title, string desc, Action content, Action footer = null, float width = 400f, float height = 300f) => Execute(() => _dialog.DrawDialog(id, title, desc, content, footer, width, height), nameof(Dialog));

        public bool ConfirmDialog(string id, string title, string message, string confirmLabel = "OK", string cancelLabel = "Cancel", Action onConfirm = null, Action onCancel = null, float width = 360f, float height = 180f)
        {
            bool confirmed = false;
            Dialog(
                id,
                title,
                message,
                content: null,
                footer: () =>
                {
                    if (Button(confirmLabel, ControlVariant.Default))
                    {
                        confirmed = true;
                        onConfirm?.Invoke();
                        CloseDialog();
                    }
                    AddSpace(8f);
                    if (Button(cancelLabel, ControlVariant.Secondary))
                    {
                        onCancel?.Invoke();
                        CloseDialog();
                    }
                },
                width: width,
                height: height
            );
            return confirmed;
        }

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

        public void EndCard() => Execute(_card.EndCard, nameof(EndCard));

        public void CardHeader(Action content) => Execute(() => _card.CardHeader(content), nameof(CardHeader));

        public void CardTitle(string title) => Execute(() => _card.CardTitle(title), nameof(CardTitle));

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

        public void Separator(SeparatorOrientation o = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] opts) => Execute(() => _separator.DrawSeparator(o, decorative, opts), nameof(Separator));

        public void HorizontalSeparator(params GUILayoutOption[] opts) => Separator(SeparatorOrientation.Horizontal, true, opts);

        public void VerticalSeparator(params GUILayoutOption[] opts) => Separator(SeparatorOrientation.Vertical, true, opts);

        public void LabeledSeparator(string text, params GUILayoutOption[] opts) => Separator(new SeparatorConfig { Text = text, Options = opts ?? Array.Empty<GUILayoutOption>() });

        public void SeparatorWithSpacing(SeparatorOrientation o, float before, float after, params GUILayoutOption[] opts) =>
            Separator(
                new SeparatorConfig
                {
                    Orientation = o,
                    SpacingBefore = before,
                    SpacingAfter = after,
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
                }
            );

        // Tabs
        public int Tabs(TabsConfig cfg)
        {
            if (cfg == null)
                return Execute(() => _tabs.Draw(cfg), 0, nameof(Tabs));
            string key = cfg.TabNames?.Length > 0 ? string.Join("|", cfg.TabNames) : null;
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

        public int Tabs(string[] tabNames, int idx, Action content = null, Action<int> onTabChange = null, int maxLines = 1, TabPosition position = TabPosition.Top, IndicatorStyle indicatorStyle = IndicatorStyle.Underline, bool overflowScroll = false) =>
            Tabs(
                new TabsConfig(tabNames, idx)
                {
                    Content = content,
                    OnTabChange = onTabChange,
                    MaxLines = maxLines,
                    Position = position,
                    IndicatorStyle = indicatorStyle,
                    EnableOverflowScroll = overflowScroll,
                }
            );

        public int Tabs(string[] tabNames, ref int idx, Action content = null, Action<int> onTabChange = null, int maxLines = 1, TabPosition position = TabPosition.Top, IndicatorStyle indicatorStyle = IndicatorStyle.Underline, bool overflowScroll = false)
        {
            idx = Tabs(tabNames, idx, content, onTabChange, maxLines, position, indicatorStyle, overflowScroll);
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
                    OnTabChange = onTabChange,
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
                    if (cfg != null)
                    {
                        _calendar.SelectedDate = cfg.SelectedDate;
                        _calendar.Ranges = cfg.Ranges ?? new List<(DateTime, DateTime)>();
                        _calendar.DisabledDates = cfg.DisabledDates ?? new List<DateTime>();
                        _calendar.OnDateSelected = cfg.OnDateSelected;
                    }
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
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
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
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
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
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
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
                    Options = opts ?? Array.Empty<GUILayoutOption>(),
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
                            Options = opts ?? Array.Empty<GUILayoutOption>(),
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
            Execute(() => _dataTable.DrawDataTable(id, cols, data, showPagination, showSearch, showSelection, showColToggle, opts), nameof(DataTable));

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
    }
}
