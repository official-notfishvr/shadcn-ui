using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.MenuBar;

namespace shadcnui.GUIComponents.Core.Base
{
    public readonly struct RenderResult
    {
        public static readonly RenderResult Value = new();
    }

    internal interface IAutoRenderBuilder
    {
        void RenderIfPending();
    }

    public abstract class ComponentBuilder<TBuilder, TConfig, TResult>
        : IAutoRenderBuilder
        where TBuilder : ComponentBuilder<TBuilder, TConfig, TResult>
        where TConfig : ComponentConfigBase, new()
    {
        protected readonly GUIHelper Helper;
        protected readonly TConfig Config;
        private readonly List<GUILayoutOption> _options = new();
        private bool _rendered;

        protected ComponentBuilder(GUIHelper helper, TConfig config = null)
        {
            Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            Config = config ?? new TConfig();
            Helper.RegisterAutoRenderBuilder(this);
        }

        protected TBuilder Self => (TBuilder)this;

        public TConfig Props() => Config;

        public TBuilder Configure(Action<TConfig> configure)
        {
            configure?.Invoke(Config);
            return Self;
        }

        public TBuilder Id(string id)
        {
            Config.Id = id;
            return Self;
        }

        public TBuilder Variant(ControlVariant variant)
        {
            Config.Variant = variant;
            return Self;
        }

        public TBuilder Size(ControlSize size)
        {
            Config.Size = size;
            return Self;
        }

        public TBuilder Disabled(bool disabled = true)
        {
            Config.IsDisabled = disabled;
            return Self;
        }

        public TBuilder Appearance(ComponentAppearance appearance)
        {
            Config.Appearance = appearance;
            return Self;
        }

        public TBuilder Style(string styleId)
        {
            Config.Appearance ??= new ComponentAppearance();
            Config.Appearance.StyleId = styleId;
            return Self;
        }

        public TBuilder Options(params GUILayoutOption[] options)
        {
            if (options != null)
                _options.AddRange(options);
            return Self;
        }

        public TBuilder Width(float width)
        {
            _options.Add(GUILayout.Width(width));
            return Self;
        }

        public TBuilder Height(float height)
        {
            _options.Add(GUILayout.Height(height));
            return Self;
        }

        public TBuilder ExpandWidth(bool expand = true)
        {
            _options.Add(GUILayout.ExpandWidth(expand));
            return Self;
        }

        public TBuilder ExpandHeight(bool expand = true)
        {
            _options.Add(GUILayout.ExpandHeight(expand));
            return Self;
        }

        public TBuilder Default() => Variant(ControlVariant.Default);

        public TBuilder Secondary() => Variant(ControlVariant.Secondary);

        public TBuilder Outline() => Variant(ControlVariant.Outline);

        public TBuilder Ghost() => Variant(ControlVariant.Ghost);

        public TBuilder Link() => Variant(ControlVariant.Link);

        public TBuilder Destructive() => Variant(ControlVariant.Destructive);

        public TBuilder Muted() => Variant(ControlVariant.Muted);

        public TBuilder Small() => Size(ControlSize.Small);

        public TBuilder Large() => Size(ControlSize.Large);

        public TBuilder Mini() => Size(ControlSize.Mini);

        public TBuilder Icon() => Size(ControlSize.Icon);

        protected void ApplyOptions() => Config.LayoutOptions = _options.Count == 0 ? Array.Empty<GUILayoutOption>() : _options.ToArray();

        protected TResult RenderOnce(Func<TResult> render, TResult alreadyRenderedValue = default)
        {
            if (_rendered)
                return alreadyRenderedValue;

            MarkRendered();
            return render();
        }

        protected void MarkRendered()
        {
            _rendered = true;
            Helper.ClearAutoRenderBuilder(this);
        }

        public virtual void RenderIfPending() => Render();

        public abstract TResult Render();
    }

    public abstract class RectComponentBuilder<TBuilder, TConfig, TResult> : ComponentBuilder<TBuilder, TConfig, TResult>
        where TBuilder : RectComponentBuilder<TBuilder, TConfig, TResult>
        where TConfig : RectConfigBase, new()
    {
        protected RectComponentBuilder(GUIHelper helper, TConfig config = null)
            : base(helper, config) { }

        public TBuilder Rect(Rect rect)
        {
            Config.Rect = rect;
            return Self;
        }
    }

    public sealed class ButtonBuilder : RectComponentBuilder<ButtonBuilder, ButtonConfig, bool>
    {
        public ButtonBuilder(GUIHelper helper, string text = "")
            : base(helper, new ButtonConfig { Text = text }) { }

        public ButtonBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public ButtonBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = image == null ? null : new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public ButtonBuilder OnClick(Action onClick)
        {
            Config.OnClick = onClick;
            return this;
        }

        public ButtonBuilder Opacity(float opacity)
        {
            Config.Opacity = opacity;
            return this;
        }

        public override bool Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                }
            );

        public static implicit operator bool(ButtonBuilder builder) => builder?.Render() ?? false;
    }

    public sealed class InputBuilder : RectComponentBuilder<InputBuilder, InputConfig, string>
    {
        public InputBuilder(GUIHelper helper, string value = "")
            : base(helper, new InputConfig { Value = value }) { }

        public InputBuilder Value(string value)
        {
            Config.Value = value;
            return this;
        }

        public InputBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public InputBuilder Placeholder(string placeholder)
        {
            Config.Placeholder = placeholder;
            return this;
        }

        public InputBuilder HelperText(string text)
        {
            Config.HelperText = text;
            return this;
        }

        public InputBuilder Error(string text)
        {
            Config.ErrorText = text;
            return this;
        }

        public InputBuilder LabelVariant(ControlVariant variant)
        {
            Config.LabelVariant = variant;
            return this;
        }

        public InputBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = image == null ? null : new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public InputBuilder Password(char maskCharacter = '*')
        {
            Config.InputKind = InputKind.Password;
            Config.MaskCharacter = maskCharacter;
            return this;
        }

        public InputBuilder MaxLength(int maxLength)
        {
            Config.MaxLength = maxLength;
            return this;
        }

        public InputBuilder InputWidth(int width)
        {
            Config.Width = width;
            return this;
        }

        public InputBuilder InputHeight(float height)
        {
            Config.Height = height;
            return this;
        }

        public InputBuilder AutoFocus(bool autoFocus = true)
        {
            Config.AutoFocus = autoFocus;
            return this;
        }

        public InputBuilder OnChange(Action<string> onValueChanged)
        {
            Config.OnValueChanged = onValueChanged;
            return this;
        }

        public override string Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.Value
            );

        public static implicit operator string(InputBuilder builder) => builder?.Render() ?? string.Empty;
    }

    public abstract class BooleanControlBuilder<TBuilder, TConfig> : RectComponentBuilder<TBuilder, TConfig, bool>
        where TBuilder : BooleanControlBuilder<TBuilder, TConfig>
        where TConfig : BoolControlConfigBase, new()
    {
        protected BooleanControlBuilder(GUIHelper helper, string label, bool value)
            : base(helper, new TConfig { Label = label, Value = value }) { }

        public TBuilder Label(string label)
        {
            Config.Label = label;
            return Self;
        }

        public TBuilder Value(bool value)
        {
            Config.Value = value;
            return Self;
        }

        public TBuilder HelperText(string text)
        {
            Config.HelperText = text;
            return Self;
        }

        public TBuilder Error(string text)
        {
            Config.ErrorText = text;
            return Self;
        }

        public TBuilder LabelVariant(ControlVariant variant)
        {
            Config.LabelVariant = variant;
            return Self;
        }

        public TBuilder FullRowClick(bool enabled = true)
        {
            Config.FullRowClick = enabled;
            return Self;
        }

        public TBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = image == null ? null : new IconConfig(image, position) { Size = size, Spacing = spacing };
            return Self;
        }

        public TBuilder OnChange(Action<bool> onValueChanged)
        {
            Config.OnValueChanged = onValueChanged;
            return Self;
        }
    }

    public sealed class CheckboxBuilder : BooleanControlBuilder<CheckboxBuilder, CheckboxConfig>
    {
        public CheckboxBuilder(GUIHelper helper, string label, bool value)
            : base(helper, label, value) { }

        public CheckboxBuilder ShowCheckmark(bool show = true)
        {
            Config.ShowCheckmark = show;
            return this;
        }

        public override bool Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                }
            );

        public static implicit operator bool(CheckboxBuilder builder) => builder?.Render() ?? false;
    }

    public sealed class SwitchBuilder : BooleanControlBuilder<SwitchBuilder, SwitchConfig>
    {
        public SwitchBuilder(GUIHelper helper, string label, bool value)
            : base(helper, label, value) { }

        public override bool Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                }
            );

        public static implicit operator bool(SwitchBuilder builder) => builder?.Render() ?? false;
    }

    public sealed class ToggleBuilder : BooleanControlBuilder<ToggleBuilder, ToggleConfig>
    {
        public ToggleBuilder(GUIHelper helper, string label, bool value)
            : base(helper, label, value) { }

        public override bool Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                }
            );

        public static implicit operator bool(ToggleBuilder builder) => builder?.Render() ?? false;
    }

    public sealed class SliderBuilder : ComponentBuilder<SliderBuilder, SliderConfig, float>
    {
        public SliderBuilder(GUIHelper helper, float value = 0f)
            : base(helper, new SliderConfig { Value = value }) { }

        public SliderBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public SliderBuilder Value(float value)
        {
            Config.Value = value;
            return this;
        }

        public SliderBuilder Range(float min, float max)
        {
            Config.MinValue = min;
            Config.MaxValue = max;
            return this;
        }

        public SliderBuilder Step(float step)
        {
            Config.Step = step;
            return this;
        }

        public SliderBuilder Format(string format)
        {
            Config.ValueFormat = format;
            return this;
        }

        public SliderBuilder ShowValue(bool show = true)
        {
            Config.ShowValue = show;
            return this;
        }

        public SliderBuilder OnChange(Action<float> onValueChanged)
        {
            Config.OnValueChanged = onValueChanged;
            return this;
        }

        public override float Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.Value
            );

        public static implicit operator float(SliderBuilder builder) => builder?.Render() ?? 0f;
    }

    public sealed class RangeSliderBuilder : ComponentBuilder<RangeSliderBuilder, RangeSliderConfig, Vector2>
    {
        public RangeSliderBuilder(GUIHelper helper, float lowerValue = 0f, float upperValue = 1f)
            : base(helper, new RangeSliderConfig { LowerValue = lowerValue, UpperValue = upperValue }) { }

        public RangeSliderBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public RangeSliderBuilder Values(float lowerValue, float upperValue)
        {
            Config.LowerValue = lowerValue;
            Config.UpperValue = upperValue;
            return this;
        }

        public RangeSliderBuilder Range(float min, float max)
        {
            Config.MinValue = min;
            Config.MaxValue = max;
            return this;
        }

        public RangeSliderBuilder Step(float step)
        {
            Config.Step = step;
            return this;
        }

        public RangeSliderBuilder Format(string format)
        {
            Config.ValueFormat = format;
            return this;
        }

        public RangeSliderBuilder ShowValue(bool show = true)
        {
            Config.ShowValue = show;
            return this;
        }

        public RangeSliderBuilder OnChange(Action<float, float> onValueChanged)
        {
            Config.OnValueChanged = onValueChanged;
            return this;
        }

        public override Vector2 Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                new Vector2(Config.LowerValue, Config.UpperValue)
            );

        public static implicit operator Vector2(RangeSliderBuilder builder) => builder?.Render() ?? Vector2.zero;
    }

    public sealed class SelectBuilder : ComponentBuilder<SelectBuilder, SelectConfig, int>
    {
        public SelectBuilder(GUIHelper helper)
            : base(helper, new SelectConfig()) { }

        public SelectBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public SelectBuilder Placeholder(string placeholder)
        {
            Config.Placeholder = placeholder;
            return this;
        }

        public new SelectBuilder Width(float width)
        {
            Config.Width = width;
            return this;
        }

        public SelectBuilder MaxHeight(float maxHeight)
        {
            Config.MaxHeight = maxHeight;
            return this;
        }

        public SelectBuilder Options(params SelectOption[] options)
        {
            Config.Options = options ?? Array.Empty<SelectOption>();
            return this;
        }

        public SelectBuilder Items(params string[] items)
        {
            Config.Options = items == null ? Array.Empty<SelectOption>() : Array.ConvertAll(items, item => new SelectOption(item, item));
            return this;
        }

        public SelectBuilder SelectedIndex(int selectedIndex)
        {
            Config.SelectedIndex = selectedIndex;
            return this;
        }

        public SelectBuilder CloseOnSelect(bool closeOnSelect = true)
        {
            Config.CloseOnSelect = closeOnSelect;
            return this;
        }

        public SelectBuilder OnChange(Action<int> onSelectionChanged)
        {
            Config.OnSelectionChanged = onSelectionChanged;
            return this;
        }

        public override int Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.SelectedIndex
            );

        public static implicit operator int(SelectBuilder builder) => builder?.Render() ?? 0;
    }

    public sealed class DropdownMenuBuilder : ComponentBuilder<DropdownMenuBuilder, DropdownMenuConfig, RenderResult>
    {
        public DropdownMenuBuilder(GUIHelper helper)
            : base(helper, new DropdownMenuConfig()) { }

        public new DropdownMenuBuilder Width(float width)
        {
            Config.Width = width;
            return this;
        }

        public DropdownMenuBuilder MaxHeight(float maxHeight)
        {
            Config.MaxHeight = maxHeight;
            return this;
        }

        public DropdownMenuBuilder Trigger(Func<bool> trigger)
        {
            Config.Trigger = trigger;
            return this;
        }

        public DropdownMenuBuilder Anchor(Rect anchorRect)
        {
            Config.AnchorRect = anchorRect;
            return this;
        }

        public DropdownMenuBuilder CloseOnSelect(bool closeOnSelect = true)
        {
            Config.CloseOnSelect = closeOnSelect;
            return this;
        }

        public DropdownMenuBuilder CloseOnClickOutside(bool closeOnClickOutside = true)
        {
            Config.CloseOnClickOutside = closeOnClickOutside;
            return this;
        }

        public DropdownMenuBuilder Item(string text, Action onClick = null, Texture2D icon = null, bool disabled = false)
        {
            Config.Items.Add(new DropdownMenuItem(DropdownMenuItemType.Item, text, onClick, icon, disabled));
            return this;
        }

        public DropdownMenuBuilder Header(string text)
        {
            Config.Items.Add(new DropdownMenuItem(DropdownMenuItemType.Header, text));
            return this;
        }

        public DropdownMenuBuilder Separator()
        {
            Config.Items.Add(new DropdownMenuItem(DropdownMenuItemType.Separator));
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class ThemeChangerBuilder : ComponentBuilder<ThemeChangerBuilder, ThemeChangerConfig, RenderResult>
    {
        public ThemeChangerBuilder(GUIHelper helper)
            : base(helper, new ThemeChangerConfig()) { }

        public new ThemeChangerBuilder Width(float width)
        {
            Config.Width = width;
            return this;
        }

        public ThemeChangerBuilder DropdownHeight(float height)
        {
            Config.DropdownHeight = height;
            return this;
        }

        public ThemeChangerBuilder ShowPreview(bool showPreview = true)
        {
            Config.ShowPreview = showPreview;
            return this;
        }

        public ThemeChangerBuilder OnChange(Action<Theme> onThemeChanged)
        {
            Config.OnThemeChanged = onThemeChanged;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class FontChangerBuilder : ComponentBuilder<FontChangerBuilder, FontChangerConfig, RenderResult>
    {
        public FontChangerBuilder(GUIHelper helper)
            : base(helper, new FontChangerConfig()) { }

        public new FontChangerBuilder Width(float width)
        {
            Config.Width = width;
            return this;
        }

        public FontChangerBuilder DropdownHeight(float height)
        {
            Config.DropdownHeight = height;
            return this;
        }

        public FontChangerBuilder ShowPreview(bool showPreview = true)
        {
            Config.ShowPreview = showPreview;
            return this;
        }

        public FontChangerBuilder PreviewText(string previewText)
        {
            Config.PreviewText = previewText;
            return this;
        }

        public FontChangerBuilder OnChange(Action<string> onFontChanged)
        {
            Config.OnFontChanged = onFontChanged;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class TextAreaBuilder : RectComponentBuilder<TextAreaBuilder, TextAreaConfig, string>
    {
        public TextAreaBuilder(GUIHelper helper, string value = "")
            : base(helper, new TextAreaConfig { Value = value }) { }

        public TextAreaBuilder Value(string value)
        {
            Config.Value = value;
            return this;
        }

        public TextAreaBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public TextAreaBuilder Placeholder(string placeholder)
        {
            Config.Placeholder = placeholder;
            return this;
        }

        public TextAreaBuilder MinHeight(float minHeight)
        {
            Config.MinHeight = minHeight;
            return this;
        }

        public TextAreaBuilder MaxHeight(float maxHeight)
        {
            Config.MaxHeight = maxHeight;
            return this;
        }

        public TextAreaBuilder MaxLength(int maxLength)
        {
            Config.MaxLength = maxLength;
            return this;
        }

        public TextAreaBuilder ShowCharacterCount(bool showCharacterCount = true)
        {
            Config.ShowCharCount = showCharacterCount;
            return this;
        }

        public override string Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.Value
            );

        public static implicit operator string(TextAreaBuilder builder) => builder?.Render() ?? string.Empty;
    }

    public sealed class CalendarBuilder : ComponentBuilder<CalendarBuilder, CalendarConfig, DateTime?>
    {
        public CalendarBuilder(GUIHelper helper)
            : base(helper, new CalendarConfig()) { }

        public CalendarBuilder Value(DateTime? selectedDate)
        {
            Config.SelectedDate = selectedDate;
            return this;
        }

        public CalendarBuilder DisabledDates(params DateTime[] disabledDates)
        {
            Config.DisabledDates = disabledDates == null ? new List<DateTime>() : new List<DateTime>(disabledDates);
            return this;
        }

        public CalendarBuilder Ranges(params (DateTime Start, DateTime End)[] ranges)
        {
            Config.Ranges = ranges == null ? new List<(DateTime Start, DateTime End)>() : new List<(DateTime Start, DateTime End)>(ranges);
            return this;
        }

        public CalendarBuilder OnChange(Action<DateTime> onDateSelected)
        {
            Config.OnDateSelected = onDateSelected;
            return this;
        }

        public override DateTime? Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.SelectedDate
            );

        public static implicit operator DateTime?(CalendarBuilder builder) => builder?.Render();
    }

    public sealed class DatePickerBuilder : ComponentBuilder<DatePickerBuilder, DatePickerConfig, DateTime?>
    {
        public DatePickerBuilder(GUIHelper helper)
            : base(helper, new DatePickerConfig()) { }

        public DatePickerBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public DatePickerBuilder Placeholder(string placeholder)
        {
            Config.Placeholder = placeholder;
            return this;
        }

        public DatePickerBuilder DisplayFormat(string format)
        {
            Config.DisplayFormat = format;
            return this;
        }

        public DatePickerBuilder Value(DateTime? selectedDate)
        {
            Config.SelectedDate = selectedDate;
            return this;
        }

        public DatePickerBuilder Range(DateTime? minDate, DateTime? maxDate)
        {
            Config.MinDate = minDate;
            Config.MaxDate = maxDate;
            return this;
        }

        public DatePickerBuilder Start(DateTime? startDate)
        {
            Config.StartDate = startDate;
            return this;
        }

        public DatePickerBuilder End(DateTime? endDate)
        {
            Config.EndDate = endDate;
            return this;
        }

        public override DateTime? Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.SelectedDate
            );

        public static implicit operator DateTime?(DatePickerBuilder builder) => builder?.Render();
    }

    public sealed class DataTableBuilder : ComponentBuilder<DataTableBuilder, DataTableConfig, RenderResult>
    {
        public DataTableBuilder(GUIHelper helper, string id)
            : base(helper, new DataTableConfig { Id = id }) { }

        public DataTableBuilder Columns(List<DataTableColumn> columns)
        {
            Config.Columns = columns ?? new List<DataTableColumn>();
            return this;
        }

        public DataTableBuilder Rows(List<DataTableRow> rows)
        {
            Config.Rows = rows ?? new List<DataTableRow>();
            return this;
        }

        public DataTableBuilder ShowPagination(bool show = true)
        {
            Config.ShowPagination = show;
            return this;
        }

        public DataTableBuilder ShowSearch(bool show = true)
        {
            Config.ShowSearch = show;
            return this;
        }

        public DataTableBuilder ShowSelection(bool show = true)
        {
            Config.ShowSelection = show;
            return this;
        }

        public DataTableBuilder ShowColumnToggle(bool show = true)
        {
            Config.ShowColumnToggle = show;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class LabelBuilder : RectComponentBuilder<LabelBuilder, LabelConfig, RenderResult>
    {
        public LabelBuilder(GUIHelper helper, string text = "")
            : base(helper, new LabelConfig { Text = text }) { }

        public LabelBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public LabelBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = image == null ? null : new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class BadgeBuilder : RectComponentBuilder<BadgeBuilder, BadgeConfig, RenderResult>
    {
        public BadgeBuilder(GUIHelper helper, string text = "Badge")
            : base(helper, new BadgeConfig { Text = text }) { }

        public BadgeBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public BadgeBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Small, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = image == null ? null : new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public BadgeBuilder Count(int count, int maxCount = 99)
        {
            Config.Count = count;
            Config.MaxCount = maxCount;
            return this;
        }

        public BadgeBuilder Progress(float progress)
        {
            Config.Progress = progress;
            return this;
        }

        public BadgeBuilder StatusDot(bool active = true)
        {
            Config.ShowStatusDot = true;
            Config.IsActive = active;
            return this;
        }

        public BadgeBuilder CornerRadius(float cornerRadius)
        {
            Config.CornerRadius = cornerRadius;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class AvatarBuilder : RectComponentBuilder<AvatarBuilder, AvatarConfig, RenderResult>
    {
        public AvatarBuilder(GUIHelper helper)
            : base(helper, new AvatarConfig()) { }

        public AvatarBuilder Image(Texture2D image)
        {
            Config.Image = image;
            return this;
        }

        public AvatarBuilder Fallback(string fallbackText)
        {
            Config.FallbackText = fallbackText;
            return this;
        }

        public AvatarBuilder Name(string name, bool showBelow = true)
        {
            Config.Name = name;
            Config.ShowNameBelow = showBelow;
            return this;
        }

        public AvatarBuilder Shape(AvatarShape shape)
        {
            Config.Shape = shape;
            return this;
        }

        public AvatarBuilder Border(Color borderColor)
        {
            Config.BorderColor = borderColor;
            return this;
        }

        public AvatarBuilder Online(bool online = true)
        {
            Config.IsOnline = online;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class ProgressBuilder : RectComponentBuilder<ProgressBuilder, ProgressConfig, RenderResult>
    {
        public ProgressBuilder(GUIHelper helper, float value = 0f)
            : base(helper, new ProgressConfig { Value = value }) { }

        public ProgressBuilder Value(float value)
        {
            Config.Value = value;
            return this;
        }

        public ProgressBuilder Label(string label)
        {
            Config.Label = label;
            return this;
        }

        public ProgressBuilder WidthValue(float width)
        {
            Config.Width = width;
            return this;
        }

        public ProgressBuilder HeightValue(float height)
        {
            Config.Height = height;
            return this;
        }

        public ProgressBuilder IndicatorSize(float size)
        {
            Config.Size = size;
            return this;
        }

        public ProgressBuilder ShowPercentage(bool show = true)
        {
            Config.ShowPercentage = show;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class ChartBuilder : ComponentBuilder<ChartBuilder, ChartConfig, RenderResult>
    {
        public ChartBuilder(GUIHelper helper)
            : base(helper, new ChartConfig { Series = new List<ChartSeries>() }) { }

        public ChartBuilder Type(ChartType chartType)
        {
            Config.ChartType = chartType;
            return this;
        }

        public ChartBuilder Series(params ChartSeries[] series)
        {
            if (series != null)
                Config.Series.AddRange(series);
            return this;
        }

        public ChartBuilder Size(float width, float height)
        {
            Config.Size = new Vector2(width, height);
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class DialogBuilder : ComponentBuilder<DialogBuilder, DialogConfig, RenderResult>
    {
        public DialogBuilder(GUIHelper helper, string id)
            : base(helper, new DialogConfig { Id = id }) { }

        public DialogBuilder Title(string title)
        {
            Config.Title = title;
            return this;
        }

        public DialogBuilder Description(string description)
        {
            Config.Description = description;
            return this;
        }

        public DialogBuilder Content(Action content)
        {
            Config.Content = content;
            return this;
        }

        public DialogBuilder Footer(Action footer)
        {
            Config.Footer = footer;
            return this;
        }

        public DialogBuilder Size(float width, float height)
        {
            Config.Width = width;
            Config.Height = height;
            return this;
        }

        public DialogBuilder CloseOnOverlayClick(bool closeOnOverlayClick = true)
        {
            Config.CloseOnOverlayClick = closeOnOverlayClick;
            return this;
        }

        public DialogBuilder ZIndex(int zIndex)
        {
            Config.ZIndex = zIndex;
            return this;
        }

        public DialogBuilder ParentWindow(Rect parentWindowRect)
        {
            Config.ParentWindowRect = parentWindowRect;
            return this;
        }

        public DialogBuilder Open()
        {
            Helper.OpenDialog(Config.Id);
            MarkRendered();
            return this;
        }

        public DialogBuilder Close()
        {
            Helper.CloseDialog();
            MarkRendered();
            return this;
        }

        public override void RenderIfPending()
        {
            if (Config.Content == null && Config.Footer == null && string.IsNullOrEmpty(Config.Title) && string.IsNullOrEmpty(Config.Description))
            {
                MarkRendered();
                return;
            }

            base.RenderIfPending();
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class PopoverBuilder : ComponentBuilder<PopoverBuilder, PopoverConfig, RenderResult>
    {
        private int _zIndex = -1;

        public PopoverBuilder(GUIHelper helper, string id = "popover")
            : base(helper, new PopoverConfig { Id = id }) { }

        public PopoverBuilder Content(Action content)
        {
            Config.Content = content;
            return this;
        }

        public PopoverBuilder ZIndex(int zIndex)
        {
            _zIndex = zIndex;
            return this;
        }

        public PopoverBuilder Open()
        {
            Helper.OpenPopover(Config.Id, _zIndex);
            MarkRendered();
            return this;
        }

        public PopoverBuilder Close()
        {
            Helper.ClosePopover();
            MarkRendered();
            return this;
        }

        public bool IsOpen() => Helper.IsPopoverOpen();

        public override void RenderIfPending()
        {
            if (Config.Content == null)
            {
                MarkRendered();
                return;
            }

            base.RenderIfPending();
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class ToastBuilder : ComponentBuilder<ToastBuilder, ToastConfig, RenderResult>
    {
        public ToastBuilder(GUIHelper helper)
            : base(helper, new ToastConfig()) { }

        public ToastBuilder Title(string title)
        {
            Config.Title = title;
            return this;
        }

        public ToastBuilder Description(string description)
        {
            Config.Description = description;
            return this;
        }

        public ToastBuilder Variant(ToastVariant variant)
        {
            Config.Variant = variant;
            return this;
        }

        public ToastBuilder Position(ToastPosition position)
        {
            Config.Position = position;
            return this;
        }

        public ToastBuilder Stack(ToastStackDirection stackDirection)
        {
            Config.StackDirection = stackDirection;
            return this;
        }

        public ToastBuilder Duration(float durationMilliseconds)
        {
            Config.DurationMs = durationMilliseconds;
            return this;
        }

        public ToastBuilder Action(string label, Action onAction)
        {
            Config.ActionLabel = label;
            Config.OnAction = onAction;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.ShowToast(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class CardBuilder : ComponentBuilder<CardBuilder, CardConfig, RenderResult>
    {
        public CardBuilder(GUIHelper helper)
            : base(helper, new CardConfig()) { }

        public CardBuilder Title(string title)
        {
            Config.Title = title;
            return this;
        }

        public CardBuilder Subtitle(string subtitle)
        {
            Config.Subtitle = subtitle;
            return this;
        }

        public CardBuilder Description(string description)
        {
            Config.Description = description;
            return this;
        }

        public CardBuilder Content(string content)
        {
            Config.Content = content;
            return this;
        }

        public CardBuilder Header(Action headerContent)
        {
            Config.HeaderContent = headerContent;
            return this;
        }

        public CardBuilder Footer(Action footerContent)
        {
            Config.FooterContent = footerContent;
            return this;
        }

        public CardBuilder Image(Texture2D image)
        {
            Config.Image = image;
            return this;
        }

        public CardBuilder Avatar(Texture2D avatar)
        {
            Config.Avatar = avatar;
            return this;
        }

        public CardBuilder Size(float width, float height = -1f)
        {
            Config.Width = width;
            Config.Height = height;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class SeparatorBuilder : RectComponentBuilder<SeparatorBuilder, SeparatorConfig, RenderResult>
    {
        public SeparatorBuilder(GUIHelper helper)
            : base(helper, new SeparatorConfig()) { }

        public SeparatorBuilder Orientation(SeparatorOrientation orientation)
        {
            Config.Orientation = orientation;
            return this;
        }

        public SeparatorBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public SeparatorBuilder Decorative(bool decorative = true)
        {
            Config.IsDecorative = decorative;
            return this;
        }

        public SeparatorBuilder Spacing(float before, float after)
        {
            Config.SpacingBefore = before;
            Config.SpacingAfter = after;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class TabsBuilder : ComponentBuilder<TabsBuilder, TabsConfig, int>
    {
        public TabsBuilder(GUIHelper helper)
            : base(helper, new TabsConfig()) { }

        public TabsBuilder Items(params string[] tabLabels)
        {
            Config.TabLabels = tabLabels ?? Array.Empty<string>();
            if (Config.DisabledTabs == null || Config.DisabledTabs.Length != Config.TabLabels.Length)
                Config.DisabledTabs = new bool[Config.TabLabels.Length];
            return this;
        }

        public TabsBuilder SelectedIndex(int selectedIndex)
        {
            Config.SelectedIndex = selectedIndex;
            return this;
        }

        public TabsBuilder Content(Action content)
        {
            Config.Content = content;
            return this;
        }

        public TabsBuilder MaxLines(int maxLines)
        {
            Config.MaxLines = maxLines;
            return this;
        }

        public TabsBuilder TabWidth(float tabWidth)
        {
            Config.TabWidth = tabWidth;
            return this;
        }

        public TabsBuilder Position(TabPosition position)
        {
            Config.Position = position;
            if (position == TabPosition.Left)
                Config.Side = TabSide.Left;
            else if (position == TabPosition.Right)
                Config.Side = TabSide.Right;
            return this;
        }

        public TabsBuilder Side(TabSide side)
        {
            Config.Side = side;
            Config.Position = side == TabSide.Right ? TabPosition.Right : TabPosition.Left;
            return this;
        }

        public TabsBuilder Indicator(IndicatorStyle indicatorStyle, bool show = true)
        {
            Config.IndicatorStyle = indicatorStyle;
            Config.ShowIndicator = show;
            return this;
        }

        public TabsBuilder Closable(params bool[] closableTabs)
        {
            Config.ClosableTabs = closableTabs;
            return this;
        }

        public TabsBuilder DisabledTabs(params bool[] disabledTabs)
        {
            Config.DisabledTabs = disabledTabs ?? Array.Empty<bool>();
            return this;
        }

        public TabsBuilder Icons(params Texture2D[] icons)
        {
            Config.TabIcons = icons;
            return this;
        }

        public TabsBuilder OnChange(Action<int> onSelectionChanged)
        {
            Config.OnSelectionChanged = onSelectionChanged;
            return this;
        }

        public TabsBuilder OnClose(Action<int> onTabClosed)
        {
            Config.OnTabClosed = onTabClosed;
            return this;
        }

        public override int Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.SelectedIndex
            );

        public static implicit operator int(TabsBuilder builder) => builder?.Render() ?? 0;
    }

    public sealed class TableBuilder : RectComponentBuilder<TableBuilder, TableConfig, RenderResult>
    {
        public TableBuilder(GUIHelper helper)
            : base(helper, new TableConfig()) { }

        public TableBuilder Headers(params string[] headers)
        {
            Config.ColumnHeaders = headers;
            return this;
        }

        public TableBuilder Rows(string[,] rows)
        {
            Config.Rows = rows;
            return this;
        }

        public TableBuilder ObjectRows(object[,] rows)
        {
            Config.ObjectRows = rows;
            return this;
        }

        public TableBuilder Search(string searchText)
        {
            Config.SearchText = searchText;
            return this;
        }

        public TableBuilder Page(int currentPage, int pageSize)
        {
            Config.CurrentPage = currentPage;
            Config.PageSize = pageSize;
            return this;
        }

        public TableBuilder OnSort(Action<int, bool> onSortChanged)
        {
            Config.OnSortChanged = onSortChanged;
            return this;
        }

        public TableBuilder OnSelection(Action<int, bool> onSelectionChanged)
        {
            Config.OnSelectionChanged = onSelectionChanged;
            return this;
        }

        public TableBuilder OnPage(Action<int> onPageChanged)
        {
            Config.OnPageChanged = onPageChanged;
            return this;
        }

        public TableBuilder OnSearch(Action<string> onSearchChanged)
        {
            Config.OnSearchChanged = onSearchChanged;
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }

    public sealed class NavigationBuilder : ComponentBuilder<NavigationBuilder, NavigationConfig, int>
    {
        public NavigationBuilder(GUIHelper helper)
            : base(helper, new NavigationConfig()) { }

        public NavigationBuilder Items(params NavigationItem[] items)
        {
            Config.Items = items ?? Array.Empty<NavigationItem>();
            return this;
        }

        public NavigationBuilder SelectedIndex(int selectedIndex)
        {
            Config.SelectedIndex = selectedIndex;
            return this;
        }

        public new NavigationBuilder Width(float width)
        {
            Config.Width = width;
            return this;
        }

        public NavigationBuilder Logo(string logoText)
        {
            Config.LogoText = logoText;
            return this;
        }

        public NavigationBuilder Indicator(IndicatorStyle indicatorStyle, bool show = true)
        {
            Config.IndicatorStyle = indicatorStyle;
            Config.ShowIndicator = show;
            return this;
        }

        public NavigationBuilder IndicatorColor(Color indicatorColor)
        {
            Config.IndicatorColor = indicatorColor;
            return this;
        }

        public NavigationBuilder OnChange(Action<int> onSelectionChanged)
        {
            Config.OnSelectionChanged = onSelectionChanged;
            return this;
        }

        public override int Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    return Helper.Render(Config);
                },
                Config.SelectedIndex
            );

        public static implicit operator int(NavigationBuilder builder) => builder?.Render() ?? 0;
    }

    public sealed class MenuItemGroupBuilder
    {
        internal readonly List<MenuItem> Items = new();

        public MenuItemGroupBuilder Item(string text, Action onClick = null, bool disabled = false, string shortcut = "")
        {
            Items.Add(new MenuItem(text, onClick, disabled, null, shortcut));
            return this;
        }

        public MenuItemGroupBuilder Separator()
        {
            Items.Add(MenuItem.Separator());
            return this;
        }

        public MenuItemGroupBuilder Header(string text)
        {
            Items.Add(MenuItem.Header(text));
            return this;
        }
    }

    public sealed class MenuBarBuilder : ComponentBuilder<MenuBarBuilder, MenuBarConfig, RenderResult>
    {
        public MenuBarBuilder(GUIHelper helper)
            : base(helper, new MenuBarConfig(new List<MenuItem>())) { }

        public MenuBarBuilder Item(string text, Action<MenuItemGroupBuilder> children = null, Action onClick = null, bool disabled = false, string shortcut = "")
        {
            var group = new MenuItemGroupBuilder();
            children?.Invoke(group);
            Config.Items.Add(new MenuItem(text, onClick, disabled, group.Items, shortcut));
            return this;
        }

        public override RenderResult Render() =>
            RenderOnce(
                () =>
                {
                    ApplyOptions();
                    Helper.Render(Config);
                    return RenderResult.Value;
                },
                RenderResult.Value
            );
    }
}
