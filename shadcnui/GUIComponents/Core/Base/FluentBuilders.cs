using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Base
{
    public abstract class FluentBuilder<TBuilder, TConfig>
        where TBuilder : FluentBuilder<TBuilder, TConfig>
    {
        protected readonly GUIHelper _helper;
        protected TConfig _config;

        protected FluentBuilder(GUIHelper helper, TConfig config)
        {
            _helper = helper;
            _config = config;
        }

        protected TBuilder Self => (TBuilder)this;

        public TConfig Build() => _config;

        public TBuilder Configure(Action<TConfig> configure)
        {
            configure?.Invoke(_config);
            return Self;
        }

        public TBuilder When(bool condition, Func<TBuilder, TBuilder> action)
        {
            return condition ? action(Self) : Self;
        }
    }

    public abstract class ComponentBuilder<TBuilder, TConfig> : FluentBuilder<TBuilder, TConfig>
        where TBuilder : ComponentBuilder<TBuilder, TConfig>
        where TConfig : class
    {
        protected ControlVariant _variant = ControlVariant.Default;
        protected ControlSize _size = ControlSize.Default;
        protected bool _disabled;
        protected readonly List<GUILayoutOption> _options = new List<GUILayoutOption>();

        protected ComponentBuilder(GUIHelper helper, TConfig config)
            : base(helper, config) { }

        public TBuilder Variant(ControlVariant variant)
        {
            _variant = variant;
            return Self;
        }

        public TBuilder Size(ControlSize size)
        {
            _size = size;
            return Self;
        }

        public TBuilder Disabled(bool disabled = true)
        {
            _disabled = disabled;
            return Self;
        }

        public TBuilder Enabled(bool enabled = true)
        {
            _disabled = !enabled;
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

        public TBuilder MinWidth(float width)
        {
            _options.Add(GUILayout.MinWidth(width));
            return Self;
        }

        public TBuilder MaxWidth(float width)
        {
            _options.Add(GUILayout.MaxWidth(width));
            return Self;
        }

        public TBuilder MinHeight(float height)
        {
            _options.Add(GUILayout.MinHeight(height));
            return Self;
        }

        public TBuilder MaxHeight(float height)
        {
            _options.Add(GUILayout.MaxHeight(height));
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

        public TBuilder FixedSize(float width, float height)
        {
            _options.Add(GUILayout.Width(width));
            _options.Add(GUILayout.Height(height));
            return Self;
        }

        public TBuilder Options(params GUILayoutOption[] options)
        {
            if (options != null)
                _options.AddRange(options);
            return Self;
        }

        protected GUILayoutOption[] GetOptions() => _options.Count > 0 ? _options.ToArray() : Array.Empty<GUILayoutOption>();

        public TBuilder Secondary() => Variant(ControlVariant.Secondary);

        public TBuilder Destructive() => Variant(ControlVariant.Destructive);

        public TBuilder Outline() => Variant(ControlVariant.Outline);

        public TBuilder Ghost() => Variant(ControlVariant.Ghost);

        public TBuilder Small() => Size(ControlSize.Small);

        public TBuilder Large() => Size(ControlSize.Large);

        public TBuilder Mini() => Size(ControlSize.Mini);
    }

    public abstract class IconSupportBuilder<TBuilder, TConfig> : ComponentBuilder<TBuilder, TConfig>
        where TBuilder : IconSupportBuilder<TBuilder, TConfig>
        where TConfig : class
    {
        protected IconSupportBuilder(GUIHelper helper, TConfig config)
            : base(helper, config) { }

        protected abstract void SetIcon(IconConfig icon);

        public TBuilder Icon(Texture2D icon)
        {
            SetIcon(new IconConfig(icon));
            return Self;
        }

        public TBuilder Icon(Texture2D icon, IconPosition position)
        {
            SetIcon(new IconConfig(icon, position));
            return Self;
        }

        public TBuilder Icon(Texture2D icon, IconPosition position, float size, float spacing)
        {
            SetIcon(new IconConfig(icon, position) { Size = size, Spacing = spacing });
            return Self;
        }

        public TBuilder Icon(IconConfig icon)
        {
            SetIcon(icon);
            return Self;
        }
    }

    public class ButtonBuilder : IconSupportBuilder<ButtonBuilder, ButtonConfig>
    {
        public ButtonBuilder(GUIHelper helper)
            : base(helper, new ButtonConfig("")) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public ButtonBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public ButtonBuilder OnClick(Action onClick)
        {
            _config.OnClick = onClick;
            return this;
        }

        public ButtonBuilder Opacity(float opacity)
        {
            _config.Opacity = opacity;
            return this;
        }

        public ButtonBuilder Primary() => Variant(ControlVariant.Default);

        public ButtonBuilder Link() => Variant(ControlVariant.Link);

        public ButtonBuilder IconSize() => Size(ControlSize.Icon);

        public bool Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            return _helper.Button(_config.Text, _config.Variant, _config.Size, _config.OnClick, _config.Disabled, _config.Opacity, _config.Options);
        }

        public bool Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            return GUI.Button(rect, _config.Text);
        }
    }

    public class LabelBuilder : IconSupportBuilder<LabelBuilder, LabelConfig>
    {
        public LabelBuilder(GUIHelper helper)
            : base(helper, new LabelConfig()) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public LabelBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public LabelBuilder Muted() => Variant(ControlVariant.Muted);

        public void Draw()
        {
            _config.Variant = _variant;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            _helper.Label(_config);
        }

        public void Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Disabled = _disabled;
            _config.Rect = rect;
            _helper.Label(_config);
        }
    }

    public class InputBuilder : IconSupportBuilder<InputBuilder, InputConfig>
    {
        public InputBuilder(GUIHelper helper)
            : base(helper, new InputConfig()) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public InputBuilder Value(string value)
        {
            _config.Value = value;
            return this;
        }

        public InputBuilder Placeholder(string placeholder)
        {
            _config.Placeholder = placeholder;
            return this;
        }

        public InputBuilder Focused(bool focused = true)
        {
            _config.Focused = focused;
            return this;
        }

        public InputBuilder OnChange(Action<string> onChange)
        {
            _config.OnChange = onChange;
            return this;
        }

        public InputBuilder InputWidth(int width)
        {
            _config.Width = width;
            return this;
        }

        public InputBuilder InputHeight(float height)
        {
            _config.Height = height;
            return this;
        }

        public InputBuilder InputLabel(string label)
        {
            _config.Label = label;
            return this;
        }

        public InputBuilder LabelVariant(ControlVariant variant)
        {
            _config.LabelVariant = variant;
            return this;
        }

        public InputBuilder MaxLength(int length)
        {
            _config.MaxLength = length;
            return this;
        }

        public InputBuilder Password(char maskChar = '*')
        {
            _config.MaskChar = maskChar;
            return this;
        }

        public string Draw()
        {
            _config.Variant = _variant;
            _config.Disabled = _disabled;
            return _helper.Input(_config);
        }

        public string DrawLabeled()
        {
            _config.Variant = _variant;
            _config.Disabled = _disabled;
            return _helper.LabeledInput(_config);
        }
    }

    public class ToggleBuilder : IconSupportBuilder<ToggleBuilder, ToggleConfig>
    {
        public ToggleBuilder(GUIHelper helper)
            : base(helper, new ToggleConfig()) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public ToggleBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public ToggleBuilder Value(bool value)
        {
            _config.Value = value;
            return this;
        }

        public ToggleBuilder OnToggle(Action<bool> onToggle)
        {
            _config.OnToggle = onToggle;
            return this;
        }

        public new ToggleBuilder Outline() => (ToggleBuilder)base.Outline();

        public new ToggleBuilder Small() => (ToggleBuilder)base.Small();

        public new ToggleBuilder Large() => (ToggleBuilder)base.Large();

        public bool Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            return _helper.Toggle(_config.Text, _config.Icon, _config.Value, _config.Variant, _config.Size, _config.OnToggle, _config.Disabled, _config.Options);
        }

        public bool Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            _config.Rect = rect;
            return _helper.Toggle(_config.Text, _config.Icon, _config.Value, _config.Variant, _config.Size, _config.OnToggle, _config.Disabled, _config.Options);
        }
    }

    public class CheckboxBuilder : IconSupportBuilder<CheckboxBuilder, CheckboxConfig>
    {
        public CheckboxBuilder(GUIHelper helper)
            : base(helper, new CheckboxConfig()) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public CheckboxBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public CheckboxBuilder Value(bool value)
        {
            _config.Value = value;
            return this;
        }

        public CheckboxBuilder OnToggle(Action<bool> onToggle)
        {
            _config.OnToggle = onToggle;
            return this;
        }

        public new CheckboxBuilder Small() => (CheckboxBuilder)base.Small();

        public new CheckboxBuilder Large() => (CheckboxBuilder)base.Large();

        public bool Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            return _helper.Checkbox(_config.Text, _config.Icon, _config.Value, _config.Variant, _config.Size, _config.OnToggle, _config.Disabled, _config.Options);
        }

        public bool Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            return _helper.Checkbox(rect, _config.Text, _config.Value, _config.Variant, _config.Size, _config.OnToggle, _config.Disabled);
        }
    }

    public class SwitchBuilder : IconSupportBuilder<SwitchBuilder, SwitchConfig>
    {
        public SwitchBuilder(GUIHelper helper)
            : base(helper, new SwitchConfig()) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public SwitchBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public SwitchBuilder Value(bool value)
        {
            _config.Value = value;
            return this;
        }

        public SwitchBuilder OnToggle(Action<bool> onToggle)
        {
            _config.OnToggle = onToggle;
            return this;
        }

        public new SwitchBuilder Small() => (SwitchBuilder)base.Small();

        public new SwitchBuilder Large() => (SwitchBuilder)base.Large();

        public bool Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            return _helper.Switch(_config.Text, _config.Icon, _config.Value, _config.Variant, _config.Size, _config.OnToggle, _config.Disabled, _config.Options);
        }

        public bool Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            return _helper.Switch(rect, _config.Text, _config.Value, _config.Variant, _config.Size, _config.OnToggle, _config.Disabled);
        }
    }

    public class BadgeBuilder : IconSupportBuilder<BadgeBuilder, BadgeConfig>
    {
        public BadgeBuilder(GUIHelper helper)
            : base(helper, new BadgeConfig()) { }

        protected override void SetIcon(IconConfig icon) => _config.Icon = icon;

        public BadgeBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public BadgeBuilder Count(int count)
        {
            _config.Count = count;
            return this;
        }

        public BadgeBuilder MaxCount(int max)
        {
            _config.MaxCount = max;
            return this;
        }

        public BadgeBuilder Active(bool active = true)
        {
            _config.IsActive = active;
            return this;
        }

        public BadgeBuilder StatusDot(bool show = true)
        {
            _config.ShowStatusDot = show;
            return this;
        }

        public BadgeBuilder Progress(float progress)
        {
            _config.Progress = progress;
            return this;
        }

        public BadgeBuilder CornerRadius(float radius)
        {
            _config.CornerRadius = radius;
            return this;
        }

        public new BadgeBuilder Secondary() => (BadgeBuilder)base.Secondary();

        public new BadgeBuilder Destructive() => (BadgeBuilder)base.Destructive();

        public new BadgeBuilder Outline() => (BadgeBuilder)base.Outline();

        public new BadgeBuilder Small() => (BadgeBuilder)base.Small();

        public new BadgeBuilder Large() => (BadgeBuilder)base.Large();

        public void Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Options = GetOptions();
            _helper.Badge(_config);
        }

        public void Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Rect = rect;
            _helper.Badge(_config);
        }
    }

    public class ProgressBuilder : ComponentBuilder<ProgressBuilder, ProgressConfig>
    {
        public ProgressBuilder(GUIHelper helper)
            : base(helper, new ProgressConfig()) { }

        public ProgressBuilder Value(float value)
        {
            _config.Value = value;
            return this;
        }

        public ProgressBuilder ProgressWidth(float width)
        {
            _config.Width = width;
            return this;
        }

        public ProgressBuilder ProgressHeight(float height)
        {
            _config.Height = height;
            return this;
        }

        public ProgressBuilder ProgressSize(float size)
        {
            _config.Size = size;
            return this;
        }

        public ProgressBuilder ProgressLabel(string label)
        {
            _config.Label = label;
            return this;
        }

        public ProgressBuilder ShowPercentage(bool show = true)
        {
            _config.ShowPercentage = show;
            return this;
        }

        public ProgressBuilder HidePercentage()
        {
            _config.ShowPercentage = false;
            return this;
        }

        public void Draw()
        {
            _config.Options = GetOptions();
            _helper.Progress(_config);
        }

        public void Draw(Rect rect)
        {
            _config.Rect = rect;
            _helper.Progress(_config);
        }
    }

    public class AvatarBuilder : ComponentBuilder<AvatarBuilder, AvatarConfig>
    {
        public AvatarBuilder(GUIHelper helper)
            : base(helper, new AvatarConfig()) { }

        public AvatarBuilder Image(Texture2D image)
        {
            _config.Image = image;
            return this;
        }

        public AvatarBuilder Fallback(string text)
        {
            _config.FallbackText = text;
            return this;
        }

        public AvatarBuilder Shape(AvatarShape shape)
        {
            _config.Shape = shape;
            return this;
        }

        public AvatarBuilder Online(bool isOnline = true)
        {
            _config.IsOnline = isOnline;
            return this;
        }

        public AvatarBuilder Offline()
        {
            _config.IsOnline = false;
            return this;
        }

        public AvatarBuilder Name(string name)
        {
            _config.Name = name;
            return this;
        }

        public AvatarBuilder ShowNameBelow(bool show = true)
        {
            _config.ShowNameBelow = show;
            return this;
        }

        public AvatarBuilder NameBelow(string name)
        {
            _config.Name = name;
            _config.ShowNameBelow = true;
            return this;
        }

        public AvatarBuilder Border(Color color)
        {
            _config.BorderColor = color;
            return this;
        }

        public AvatarBuilder Circle() => Shape(AvatarShape.Circle);

        public AvatarBuilder Square() => Shape(AvatarShape.Square);

        public AvatarBuilder Rounded() => Shape(AvatarShape.Rounded);

        public new AvatarBuilder Small() => (AvatarBuilder)base.Small();

        public new AvatarBuilder Large() => (AvatarBuilder)base.Large();

        public new AvatarBuilder Mini() => (AvatarBuilder)base.Mini();

        public void Draw()
        {
            _config.Size = _size;
            _config.Options = GetOptions();
            _helper.Avatar(_config);
        }

        public void Draw(Rect rect)
        {
            _config.Size = _size;
            _config.Rect = rect;
            _helper.Avatar(_config);
        }
    }

    public class CardBuilder : ComponentBuilder<CardBuilder, CardConfig>
    {
        public CardBuilder(GUIHelper helper)
            : base(helper, new CardConfig()) { }

        public CardBuilder Title(string title)
        {
            _config.Title = title;
            return this;
        }

        public CardBuilder Description(string description)
        {
            _config.Description = description;
            return this;
        }

        public CardBuilder Subtitle(string subtitle)
        {
            _config.Subtitle = subtitle;
            return this;
        }

        public CardBuilder Content(string content)
        {
            _config.Content = content;
            return this;
        }

        public CardBuilder Header(Action header)
        {
            _config.HeaderContent = header;
            return this;
        }

        public CardBuilder Footer(Action footer)
        {
            _config.FooterContent = footer;
            return this;
        }

        public CardBuilder Image(Texture2D image)
        {
            _config.Image = image;
            return this;
        }

        public CardBuilder Avatar(Texture2D avatar)
        {
            _config.Avatar = avatar;
            return this;
        }

        public CardBuilder CardWidth(float width)
        {
            _config.Width = width;
            return this;
        }

        public CardBuilder CardHeight(float height)
        {
            _config.Height = height;
            return this;
        }

        public CardBuilder CardSize(float width, float height)
        {
            _config.Width = width;
            _config.Height = height;
            return this;
        }

        public void Draw()
        {
            if (_config.Image != null)
                _helper.CardWithImage(_config.Image, _config.Title, _config.Description, _config.Content, _config.FooterContent, _config.Width, _config.Height);
            else if (_config.Avatar != null)
                _helper.CardWithAvatar(_config.Avatar, _config.Title, _config.Subtitle ?? _config.Description, _config.Content, _config.FooterContent, _config.Width, _config.Height);
            else if (_config.HeaderContent != null)
                _helper.CardWithHeader(_config.Title, _config.Description, _config.HeaderContent, _config.Content, _config.FooterContent, _config.Width, _config.Height);
            else
                _helper.Card(_config);
        }
    }

    public class DialogBuilder : ComponentBuilder<DialogBuilder, DialogConfig>
    {
        public DialogBuilder(GUIHelper helper)
            : base(helper, new DialogConfig()) { }

        public DialogBuilder Id(string id)
        {
            _config.Id = id;
            return this;
        }

        public DialogBuilder Title(string title)
        {
            _config.Title = title;
            return this;
        }

        public DialogBuilder Description(string description)
        {
            _config.Description = description;
            return this;
        }

        public DialogBuilder Content(Action content)
        {
            _config.Content = content;
            return this;
        }

        public DialogBuilder Footer(Action footer)
        {
            _config.Footer = footer;
            return this;
        }

        public DialogBuilder DialogWidth(float width)
        {
            _config.Width = width;
            return this;
        }

        public DialogBuilder DialogHeight(float height)
        {
            _config.Height = height;
            return this;
        }

        public DialogBuilder DialogSize(float width, float height)
        {
            _config.Width = width;
            _config.Height = height;
            return this;
        }

        public DialogBuilder CloseOnOverlay(bool close = true)
        {
            _config.CloseOnOverlayClick = close;
            return this;
        }

        public DialogBuilder ZIndex(int zIndex)
        {
            _config.ZIndex = zIndex;
            return this;
        }

        public void Draw() => _helper.Dialog(_config);

        public void Open() => _helper.OpenDialog(_config.Id);

        public void Close() => _helper.CloseDialog();
    }

    public class SeparatorBuilder : ComponentBuilder<SeparatorBuilder, SeparatorConfig>
    {
        public SeparatorBuilder(GUIHelper helper)
            : base(helper, new SeparatorConfig()) { }

        public SeparatorBuilder Orientation(SeparatorOrientation orientation)
        {
            _config.Orientation = orientation;
            return this;
        }

        public SeparatorBuilder Decorative(bool decorative = true)
        {
            _config.Decorative = decorative;
            return this;
        }

        public SeparatorBuilder WithLabel(string label)
        {
            _config.Text = label;
            return this;
        }

        public SeparatorBuilder SpacingBefore(float spacing)
        {
            _config.SpacingBefore = spacing;
            return this;
        }

        public SeparatorBuilder SpacingAfter(float spacing)
        {
            _config.SpacingAfter = spacing;
            return this;
        }

        public SeparatorBuilder Spacing(float before, float after)
        {
            _config.SpacingBefore = before;
            _config.SpacingAfter = after;
            return this;
        }

        public SeparatorBuilder Horizontal() => Orientation(SeparatorOrientation.Horizontal);

        public SeparatorBuilder Vertical() => Orientation(SeparatorOrientation.Vertical);

        public void Draw()
        {
            _config.Options = GetOptions();
            _helper.Separator(_config);
        }

        public void Draw(Rect rect)
        {
            _config.Rect = rect;
            _helper.Separator(_config);
        }
    }

    public class SliderBuilder : ComponentBuilder<SliderBuilder, SliderConfig>
    {
        public SliderBuilder(GUIHelper helper)
            : base(helper, new SliderConfig()) { }

        public SliderBuilder Value(float value)
        {
            _config.Value = value;
            return this;
        }

        public SliderBuilder Min(float min)
        {
            _config.MinValue = min;
            return this;
        }

        public SliderBuilder Max(float max)
        {
            _config.MaxValue = max;
            return this;
        }

        public SliderBuilder Step(float step)
        {
            _config.Step = step;
            return this;
        }

        public SliderBuilder Range(float min, float max)
        {
            _config.MinValue = min;
            _config.MaxValue = max;
            return this;
        }

        public SliderBuilder SliderLabel(string label)
        {
            _config.Label = label;
            return this;
        }

        public SliderBuilder ShowValue(bool show = true)
        {
            _config.ShowValue = show;
            return this;
        }

        public SliderBuilder HideValue()
        {
            _config.ShowValue = false;
            return this;
        }

        public SliderBuilder ValueFormat(string format)
        {
            _config.ValueFormat = format;
            return this;
        }

        public SliderBuilder OnChange(Action<float> onChange)
        {
            _config.OnChange = onChange;
            return this;
        }

        public float Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            return _helper.Slider(_config);
        }
    }

    public class TextAreaBuilder : ComponentBuilder<TextAreaBuilder, TextAreaConfig>
    {
        public TextAreaBuilder(GUIHelper helper)
            : base(helper, new TextAreaConfig()) { }

        public TextAreaBuilder Id(string id)
        {
            _config.Id = id;
            return this;
        }

        public TextAreaBuilder Text(string text)
        {
            _config.Text = text;
            return this;
        }

        public TextAreaBuilder Placeholder(string placeholder)
        {
            _config.Placeholder = placeholder;
            return this;
        }

        public TextAreaBuilder TextAreaLabel(string label)
        {
            _config.Label = label;
            return this;
        }

        public TextAreaBuilder TextAreaMinHeight(float height)
        {
            _config.MinHeight = height;
            return this;
        }

        public TextAreaBuilder TextAreaMaxHeight(float height)
        {
            _config.MaxHeight = height;
            return this;
        }

        public TextAreaBuilder MaxLength(int length)
        {
            _config.MaxLength = length;
            return this;
        }

        public TextAreaBuilder ShowCharCount(bool show = true)
        {
            _config.ShowCharCount = show;
            return this;
        }

        public TextAreaBuilder HideCharCount()
        {
            _config.ShowCharCount = false;
            return this;
        }

        public new TextAreaBuilder Outline() => (TextAreaBuilder)base.Outline();

        public new TextAreaBuilder Ghost() => (TextAreaBuilder)base.Ghost();

        public string Draw()
        {
            _config.Variant = _variant;
            _config.Disabled = _disabled;
            _config.Options = GetOptions();
            return _helper.TextArea(_config);
        }

        public string Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Disabled = _disabled;
            _config.Rect = rect;
            return _helper.TextArea(_config);
        }
    }

    public class TabsBuilder : ComponentBuilder<TabsBuilder, TabsConfig>
    {
        public TabsBuilder(GUIHelper helper)
            : base(helper, new TabsConfig(Array.Empty<string>(), 0)) { }

        public TabsBuilder Items(params string[] items)
        {
            _config.TabNames = items;
            _config.DisabledTabs = new bool[items?.Length ?? 0];
            return this;
        }

        public TabsBuilder SelectedIndex(int index)
        {
            _config.SelectedIndex = index;
            return this;
        }

        public TabsBuilder OnTabChange(Action<int> onChange)
        {
            _config.OnTabChange = onChange;
            return this;
        }

        public TabsBuilder Content(Action content)
        {
            _config.Content = content;
            return this;
        }

        public TabsBuilder MaxLines(int lines)
        {
            _config.MaxLines = lines;
            return this;
        }

        public TabsBuilder Position(TabPosition position)
        {
            _config.Position = position;
            return this;
        }

        public TabsBuilder Side(TabSide side)
        {
            _config.Side = side;
            return this;
        }

        public TabsBuilder TabWidth(float width)
        {
            _config.TabWidth = width;
            return this;
        }

        public TabsBuilder DisabledTabs(params int[] indices)
        {
            if (_config.DisabledTabs != null)
                foreach (var i in indices)
                    if (i >= 0 && i < _config.DisabledTabs.Length)
                        _config.DisabledTabs[i] = true;
            return this;
        }

        public TabsBuilder DisabledTabs(bool[] disabled)
        {
            _config.DisabledTabs = disabled;
            return this;
        }

        public TabsBuilder Indicator(IndicatorStyle style)
        {
            _config.IndicatorStyle = style;
            return this;
        }

        public TabsBuilder ShowIndicator(bool show = true)
        {
            _config.ShowIndicator = show;
            return this;
        }

        public TabsBuilder HideIndicator()
        {
            _config.ShowIndicator = false;
            return this;
        }

        public TabsBuilder Closable(params int[] indices)
        {
            _config.ClosableTabs = new bool[_config.TabNames?.Length ?? 0];
            foreach (var i in indices)
                if (i >= 0 && i < _config.ClosableTabs.Length)
                    _config.ClosableTabs[i] = true;
            return this;
        }

        public TabsBuilder ClosableTabs(bool[] closable)
        {
            _config.ClosableTabs = closable;
            return this;
        }

        public TabsBuilder OnTabClose(Action<int> onClose)
        {
            _config.OnTabClose = onClose;
            return this;
        }

        public TabsBuilder OverflowScroll(bool enable = true)
        {
            _config.EnableOverflowScroll = enable;
            return this;
        }

        public TabsBuilder Icons(params Texture2D[] icons)
        {
            _config.TabIcons = icons;
            return this;
        }

        public TabsBuilder Top() => Position(TabPosition.Top);

        public TabsBuilder Bottom() => Position(TabPosition.Bottom);

        public TabsBuilder Left()
        {
            Position(TabPosition.Left);
            return Side(TabSide.Left);
        }

        public TabsBuilder Right()
        {
            Position(TabPosition.Right);
            return Side(TabSide.Right);
        }

        public int Draw()
        {
            _config.Options = GetOptions();
            return _helper.Tabs(_config);
        }
    }

    public class TableBuilder : ComponentBuilder<TableBuilder, TableConfig>
    {
        public TableBuilder(GUIHelper helper)
            : base(helper, new TableConfig()) { }

        public TableBuilder Headers(params string[] headers)
        {
            _config.Headers = headers;
            return this;
        }

        public TableBuilder Data(string[,] data)
        {
            _config.Data = data;
            return this;
        }

        public TableBuilder ObjectData(object[,] data)
        {
            _config.ObjectData = data;
            return this;
        }

        public TableBuilder SortColumns(params int[] columns)
        {
            _config.SortColumns = columns;
            return this;
        }

        public TableBuilder SortAscending(params bool[] ascending)
        {
            _config.SortAscending = ascending;
            return this;
        }

        public TableBuilder OnSort(Action<int, bool> onSort)
        {
            _config.OnSort = onSort;
            return this;
        }

        public TableBuilder SelectedRows(bool[] selected)
        {
            _config.SelectedRows = selected;
            return this;
        }

        public TableBuilder OnSelectionChange(Action<int, bool> onChange)
        {
            _config.OnSelectionChange = onChange;
            return this;
        }

        public TableBuilder CurrentPage(int page)
        {
            _config.CurrentPage = page;
            return this;
        }

        public TableBuilder PageSize(int size)
        {
            _config.PageSize = size;
            return this;
        }

        public TableBuilder OnPageChange(Action<int> onChange)
        {
            _config.OnPageChange = onChange;
            return this;
        }

        public TableBuilder SearchQuery(string query)
        {
            _config.SearchQuery = query;
            return this;
        }

        public TableBuilder FilteredData(string[,] data)
        {
            _config.FilteredData = data;
            return this;
        }

        public TableBuilder OnSearch(Action<string> onSearch)
        {
            _config.OnSearch = onSearch;
            return this;
        }

        public TableBuilder ColumnWidths(params float[] widths)
        {
            _config.ColumnWidths = widths;
            return this;
        }

        public TableBuilder CellRenderer(Action<object, int, int> renderer)
        {
            _config.CellRenderer = renderer;
            return this;
        }

        public void Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Options = GetOptions();
            _helper.Table(_config);
        }

        public void Draw(Rect rect)
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _config.Rect = rect;
            _helper.Table(_config);
        }
    }

    public class CalendarBuilder : ComponentBuilder<CalendarBuilder, CalendarConfig>
    {
        public CalendarBuilder(GUIHelper helper)
            : base(helper, new CalendarConfig()) { }

        public void Draw()
        {
            _config.Variant = _variant;
            _config.Size = _size;
            _helper.Calendar(_config);
        }
    }

    public class DatePickerBuilder : ComponentBuilder<DatePickerBuilder, DatePickerConfig>
    {
        public DatePickerBuilder(GUIHelper helper)
            : base(helper, new DatePickerConfig()) { }

        public DatePickerBuilder Id(string id)
        {
            _config.Id = id;
            return this;
        }

        public DatePickerBuilder Placeholder(string placeholder)
        {
            _config.Placeholder = placeholder;
            return this;
        }

        public DatePickerBuilder SelectedDate(DateTime? date)
        {
            _config.SelectedDate = date;
            return this;
        }

        public DatePickerBuilder DatePickerLabel(string label)
        {
            _config.Label = label;
            return this;
        }

        public DatePickerBuilder MinDate(DateTime? date)
        {
            _config.MinDate = date;
            return this;
        }

        public DatePickerBuilder MaxDate(DateTime? date)
        {
            _config.MaxDate = date;
            return this;
        }

        public DatePickerBuilder DateRange(DateTime? min, DateTime? max)
        {
            _config.MinDate = min;
            _config.MaxDate = max;
            return this;
        }

        public DatePickerBuilder StartDate(DateTime? date)
        {
            _config.StartDate = date;
            return this;
        }

        public DatePickerBuilder EndDate(DateTime? date)
        {
            _config.EndDate = date;
            return this;
        }

        public DateTime? Draw()
        {
            _config.Options = GetOptions();
            return _helper.DatePicker(_config);
        }
    }

    public class ChartBuilder : ComponentBuilder<ChartBuilder, ChartConfig>
    {
        private readonly List<ChartSeries> _series = new List<ChartSeries>();

        public ChartBuilder(GUIHelper helper)
            : base(helper, new ChartConfig(new List<ChartSeries>(), ChartType.Line)) { }

        public ChartBuilder Type(ChartType type)
        {
            _config.ChartType = type;
            return this;
        }

        public ChartBuilder ChartSize(Vector2 size)
        {
            _config.Size = size;
            return this;
        }

        public ChartBuilder ChartSize(float width, float height)
        {
            _config.Size = new Vector2(width, height);
            return this;
        }

        public ChartBuilder Series(ChartSeries series)
        {
            _series.Add(series);
            return this;
        }

        public ChartBuilder Series(string key, string label, Color color, params ChartDataPoint[] dataPoints)
        {
            var series = new ChartSeries(key, label, color);
            series.Data.AddRange(dataPoints);
            _series.Add(series);
            return this;
        }

        public ChartBuilder Series(string key, string label, params ChartDataPoint[] dataPoints)
        {
            var series = new ChartSeries(key, label);
            series.Data.AddRange(dataPoints);
            _series.Add(series);
            return this;
        }

        public ChartBuilder AddDataPoint(string seriesKey, string name, float value, Color color = default)
        {
            var series = _series.Find(s => s.Key == seriesKey);
            if (series != null)
                series.Data.Add(new ChartDataPoint(name, value, color == default ? series.Color : color));
            return this;
        }

        public ChartBuilder Line() => Type(ChartType.Line);

        public ChartBuilder Bar() => Type(ChartType.Bar);

        public ChartBuilder Pie() => Type(ChartType.Pie);

        public ChartBuilder Area() => Type(ChartType.Area);

        public ChartBuilder Scatter() => Type(ChartType.Scatter);

        public void Draw()
        {
            _config.Series = _series;
            _config.Options = GetOptions();
            _helper.Chart(_config);
        }
    }

    public class PopoverBuilder : ComponentBuilder<PopoverBuilder, PopoverConfig>
    {
        public PopoverBuilder(GUIHelper helper)
            : base(helper, new PopoverConfig()) { }

        public PopoverBuilder Content(Action content)
        {
            _config.Content = content;
            return this;
        }

        public void Draw()
        {
            _config.Options = GetOptions();
            _helper.Popover(_config);
        }

        public void Open() => _helper.OpenPopover();

        public void Close() => _helper.ClosePopover();
    }

    public class DropdownMenuBuilder : ComponentBuilder<DropdownMenuBuilder, DropdownMenuConfig>
    {
        private readonly List<DropdownMenuItem> _items = new List<DropdownMenuItem>();

        public DropdownMenuBuilder(GUIHelper helper)
            : base(helper, null) { }

        public DropdownMenuBuilder Item(string content, Action onClick = null, bool isSelected = false, Texture2D icon = null)
        {
            _items.Add(new DropdownMenuItem(DropdownMenuItemType.Item, content, onClick, isSelected, icon));
            return this;
        }

        public DropdownMenuBuilder Header(string content)
        {
            _items.Add(new DropdownMenuItem(DropdownMenuItemType.Header, content));
            return this;
        }

        public DropdownMenuBuilder AddSeparator()
        {
            _items.Add(new DropdownMenuItem(DropdownMenuItemType.Separator));
            return this;
        }

        public DropdownMenuBuilder SubMenu(string content, Action<DropdownMenuBuilder> buildSubMenu)
        {
            var subBuilder = new DropdownMenuBuilder(_helper);
            buildSubMenu(subBuilder);
            var item = new DropdownMenuItem(DropdownMenuItemType.Item, content);
            item.SubItems = subBuilder._items;
            _items.Add(item);
            return this;
        }

        public DropdownMenuBuilder Items(params (string text, Action onClick)[] items)
        {
            foreach (var (text, onClick) in items)
                _items.Add(new DropdownMenuItem(DropdownMenuItemType.Item, text, onClick));
            return this;
        }

        public void Draw()
        {
            _config = new DropdownMenuConfig(_items) { Options = GetOptions() };
            _helper.DropdownMenu(_config);
        }

        public void Close() => _helper.CloseDropdownMenu();
    }

    public class ToastBuilder : ComponentBuilder<ToastBuilder, ToastConfig>
    {
        public ToastBuilder(GUIHelper helper)
            : base(helper, new ToastConfig()) { }

        public ToastBuilder Id(string id)
        {
            _config.Id = id;
            return this;
        }

        public ToastBuilder Title(string title)
        {
            _config.Title = title;
            return this;
        }

        public ToastBuilder Description(string description)
        {
            _config.Description = description;
            return this;
        }

        public ToastBuilder Variant(ToastVariant variant)
        {
            _config.Variant = variant;
            return this;
        }

        public ToastBuilder Duration(float ms)
        {
            _config.DurationMs = ms;
            return this;
        }

        public ToastBuilder Dismissible(bool dismissible = true)
        {
            _config.Dismissible = dismissible;
            return this;
        }

        public ToastBuilder Position(ToastPosition position)
        {
            _config.Position = position;
            return this;
        }

        public ToastBuilder StackDirection(ToastStackDirection direction)
        {
            _config.StackDirection = direction;
            return this;
        }

        public ToastBuilder Action(string label, Action onAction)
        {
            _config.ActionLabel = label;
            _config.OnAction = onAction;
            return this;
        }

        public ToastBuilder PauseOnHover(bool enable = true)
        {
            _config.EnablePauseOnHover = enable;
            return this;
        }

        public ToastBuilder HoverDelay(float delay)
        {
            _config.HoverPauseDelay = delay;
            return this;
        }

        public ToastBuilder ClickToDismiss(bool enable = true)
        {
            _config.EnableClickToDismiss = enable;
            return this;
        }

        public ToastBuilder ProgressBar(bool show = true)
        {
            _config.ShowProgressBar = show;
            return this;
        }

        public ToastBuilder HideProgressBar()
        {
            _config.ShowProgressBar = false;
            return this;
        }

        public ToastBuilder AccentBar(bool show = true)
        {
            _config.ShowAccentBar = show;
            return this;
        }

        public ToastBuilder HideAccentBar()
        {
            _config.ShowAccentBar = false;
            return this;
        }

        public ToastBuilder ToastWidth(float width)
        {
            _config.Width = width;
            return this;
        }

        public ToastBuilder ToastMinWidth(float width)
        {
            _config.MinWidth = width;
            return this;
        }

        public ToastBuilder ToastMaxWidth(float width)
        {
            _config.MaxWidth = width;
            return this;
        }

        public ToastBuilder ToastMinHeight(float height)
        {
            _config.MinHeight = height;
            return this;
        }

        public ToastBuilder Padding(float padding)
        {
            _config.Padding = padding;
            return this;
        }

        public ToastBuilder BorderRadius(float radius)
        {
            _config.BorderRadius = radius;
            return this;
        }

        public ToastBuilder Margin(float margin)
        {
            _config.Margin = margin;
            return this;
        }

        public ToastBuilder ToastSpacing(float spacing)
        {
            _config.Spacing = spacing;
            return this;
        }

        public ToastBuilder SystemStyle(bool use = true)
        {
            _config.UseSystemNotificationStyle = use;
            return this;
        }

        public ToastBuilder Success() => Variant(ToastVariant.Success);

        public ToastBuilder Error() => Variant(ToastVariant.Error);

        public ToastBuilder Warning() => Variant(ToastVariant.Warning);

        public ToastBuilder Info() => Variant(ToastVariant.Info);

        public ToastBuilder TopLeft() => Position(ToastPosition.TopLeft);

        public ToastBuilder TopCenter() => Position(ToastPosition.TopCenter);

        public ToastBuilder TopRight() => Position(ToastPosition.TopRight);

        public ToastBuilder BottomLeft() => Position(ToastPosition.BottomLeft);

        public ToastBuilder BottomCenter() => Position(ToastPosition.BottomCenter);

        public ToastBuilder BottomRight() => Position(ToastPosition.BottomRight);

        public ToastBuilder CenterLeft() => Position(ToastPosition.CenterLeft);

        public ToastBuilder Center() => Position(ToastPosition.Center);

        public ToastBuilder CenterRight() => Position(ToastPosition.CenterRight);

        public ToastBuilder StackUp() => StackDirection(ToastStackDirection.Up);

        public ToastBuilder StackDown() => StackDirection(ToastStackDirection.Down);

        public ToastBuilder StackLeft() => StackDirection(ToastStackDirection.Left);

        public ToastBuilder StackRight() => StackDirection(ToastStackDirection.Right);

        public string Show()
        {
            _helper.ShowToast(_config);
            return _config.Id;
        }
    }

    public class SelectBuilder : ComponentBuilder<SelectBuilder, SelectConfig>
    {
        public SelectBuilder(GUIHelper helper)
            : base(helper, new SelectConfig()) { }

        public SelectBuilder Items(params string[] items)
        {
            _config.Items = items;
            return this;
        }

        public SelectBuilder SelectedIndex(int index)
        {
            _config.SelectedIndex = index;
            return this;
        }

        public SelectBuilder OnChange(Action<int> onChange)
        {
            _config.OnChange = onChange;
            return this;
        }

        public int Draw()
        {
            _config.Options = GetOptions();
            return _helper.Select(_config.Items, _config.SelectedIndex);
        }

        public void Open() => _helper.OpenSelect();

        public void Close() => _helper.CloseSelect();

        public bool IsOpen() => _helper.IsSelectOpen();
    }

    public class MenuBarBuilder : ComponentBuilder<MenuBarBuilder, MenuBar.MenuBarConfig>
    {
        private readonly List<MenuBar.MenuItem> _items = new List<MenuBar.MenuItem>();

        public MenuBarBuilder(GUIHelper helper)
            : base(helper, null) { }

        public MenuBarBuilder Item(string label, Action onClick = null)
        {
            _items.Add(new MenuBar.MenuItem(label, onClick));
            return this;
        }

        public MenuBarBuilder Item(string label, Action<MenuBarBuilder> buildSubMenu)
        {
            var subBuilder = new MenuBarBuilder(_helper);
            buildSubMenu(subBuilder);
            _items.Add(new MenuBar.MenuItem(label, null, false, subBuilder._items));
            return this;
        }

        public MenuBarBuilder Items(params MenuBar.MenuItem[] items)
        {
            _items.AddRange(items);
            return this;
        }

        public MenuBarBuilder Separator()
        {
            _items.Add(MenuBar.MenuItem.Separator());
            return this;
        }

        public MenuBarBuilder Header(string text)
        {
            _items.Add(MenuBar.MenuItem.Header(text));
            return this;
        }

        public void Draw()
        {
            _config = new MenuBar.MenuBarConfig(_items) { Options = GetOptions() };
            _helper.MenuBar(_config);
        }
    }

    public partial class GUIHelper
    {
        public ButtonBuilder CreateButton() => new ButtonBuilder(this);

        public ButtonBuilder CreateButton(string text) => new ButtonBuilder(this).Text(text);

        public ButtonBuilder CreateButton(string text, Action onClick) => new ButtonBuilder(this).Text(text).OnClick(onClick);

        public LabelBuilder CreateLabel() => new LabelBuilder(this);

        public LabelBuilder CreateLabel(string text) => new LabelBuilder(this).Text(text);

        public InputBuilder CreateInput() => new InputBuilder(this);

        public InputBuilder CreateInput(string value) => new InputBuilder(this).Value(value);

        public InputBuilder CreateInput(string value, string placeholder) => new InputBuilder(this).Value(value).Placeholder(placeholder);

        public ToggleBuilder CreateToggle() => new ToggleBuilder(this);

        public ToggleBuilder CreateToggle(string text, bool value) => new ToggleBuilder(this).Text(text).Value(value);

        public ToggleBuilder CreateToggle(string text, bool value, Action<bool> onToggle) => new ToggleBuilder(this).Text(text).Value(value).OnToggle(onToggle);

        public CheckboxBuilder CreateCheckbox() => new CheckboxBuilder(this);

        public CheckboxBuilder CreateCheckbox(string text, bool value) => new CheckboxBuilder(this).Text(text).Value(value);

        public CheckboxBuilder CreateCheckbox(string text, bool value, Action<bool> onToggle) => new CheckboxBuilder(this).Text(text).Value(value).OnToggle(onToggle);

        public SwitchBuilder CreateSwitch() => new SwitchBuilder(this);

        public SwitchBuilder CreateSwitch(string text, bool value) => new SwitchBuilder(this).Text(text).Value(value);

        public SwitchBuilder CreateSwitch(string text, bool value, Action<bool> onToggle) => new SwitchBuilder(this).Text(text).Value(value).OnToggle(onToggle);

        public BadgeBuilder CreateBadge() => new BadgeBuilder(this);

        public BadgeBuilder CreateBadge(string text) => new BadgeBuilder(this).Text(text);

        public BadgeBuilder CreateBadge(int count) => new BadgeBuilder(this).Count(count);

        public ProgressBuilder CreateProgress() => new ProgressBuilder(this);

        public ProgressBuilder CreateProgress(float value) => new ProgressBuilder(this).Value(value);

        public ProgressBuilder CreateProgress(float value, string label) => new ProgressBuilder(this).Value(value).ProgressLabel(label);

        public AvatarBuilder CreateAvatar() => new AvatarBuilder(this);

        public AvatarBuilder CreateAvatar(Texture2D image) => new AvatarBuilder(this).Image(image);

        public AvatarBuilder CreateAvatar(string fallback) => new AvatarBuilder(this).Fallback(fallback);

        public CardBuilder CreateCard() => new CardBuilder(this);

        public CardBuilder CreateCard(string title) => new CardBuilder(this).Title(title);

        public CardBuilder CreateCard(string title, string description) => new CardBuilder(this).Title(title).Description(description);

        public DialogBuilder CreateDialog() => new DialogBuilder(this);

        public DialogBuilder CreateDialog(string id) => new DialogBuilder(this).Id(id);

        public DialogBuilder CreateDialog(string id, string title) => new DialogBuilder(this).Id(id).Title(title);

        public SeparatorBuilder CreateSeparator() => new SeparatorBuilder(this);

        public SliderBuilder CreateSlider() => new SliderBuilder(this);

        public SliderBuilder CreateSlider(float value) => new SliderBuilder(this).Value(value);

        public SliderBuilder CreateSlider(float value, float min, float max) => new SliderBuilder(this).Value(value).Range(min, max);

        public TextAreaBuilder CreateTextArea() => new TextAreaBuilder(this);

        public TextAreaBuilder CreateTextArea(string text) => new TextAreaBuilder(this).Text(text);

        public TextAreaBuilder CreateTextArea(string text, string placeholder) => new TextAreaBuilder(this).Text(text).Placeholder(placeholder);

        public TabsBuilder CreateTabs() => new TabsBuilder(this);

        public TabsBuilder CreateTabs(params string[] items) => new TabsBuilder(this).Items(items);

        public TabsBuilder CreateTabs(int selectedIndex, params string[] items) => new TabsBuilder(this).Items(items).SelectedIndex(selectedIndex);

        public TableBuilder CreateTable() => new TableBuilder(this);

        public TableBuilder CreateTable(params string[] headers) => new TableBuilder(this).Headers(headers);

        public CalendarBuilder CreateCalendar() => new CalendarBuilder(this);

        public DatePickerBuilder CreateDatePicker() => new DatePickerBuilder(this);

        public DatePickerBuilder CreateDatePicker(string id) => new DatePickerBuilder(this).Id(id);

        public DatePickerBuilder CreateDatePicker(string id, string placeholder) => new DatePickerBuilder(this).Id(id).Placeholder(placeholder);

        public ChartBuilder CreateChart() => new ChartBuilder(this);

        public ChartBuilder CreateChart(ChartType type) => new ChartBuilder(this).Type(type);

        public PopoverBuilder CreatePopover() => new PopoverBuilder(this);

        public PopoverBuilder CreatePopover(Action content) => new PopoverBuilder(this).Content(content);

        public DropdownMenuBuilder CreateDropdownMenu() => new DropdownMenuBuilder(this);

        public ToastBuilder CreateToast() => new ToastBuilder(this);

        public ToastBuilder CreateToast(string title) => new ToastBuilder(this).Title(title);

        public ToastBuilder CreateToast(string title, string description) => new ToastBuilder(this).Title(title).Description(description);

        public SelectBuilder CreateSelect() => new SelectBuilder(this);

        public SelectBuilder CreateSelect(params string[] items) => new SelectBuilder(this).Items(items);

        public SelectBuilder CreateSelect(int selectedIndex, params string[] items) => new SelectBuilder(this).Items(items).SelectedIndex(selectedIndex);

        public MenuBarBuilder CreateMenuBar() => new MenuBarBuilder(this);
    }
}
