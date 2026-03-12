// NOT BEING WORKED ON
using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Base
{
    public abstract class FluentBuilder<TBuilder, TConfig>
        where TBuilder : FluentBuilder<TBuilder, TConfig>
        where TConfig : class
    {
        protected readonly GUIHelper Helper;
        protected readonly TConfig Config;

        protected FluentBuilder(GUIHelper helper, TConfig config)
        {
            Helper = helper;
            Config = config;
        }

        protected TBuilder Self => (TBuilder)this;

        public TConfig Build() => Config;

        public TBuilder Configure(Action<TConfig> configure)
        {
            configure?.Invoke(Config);
            return Self;
        }
    }

    public abstract class SizedBuilder<TBuilder, TConfig> : FluentBuilder<TBuilder, TConfig>
        where TBuilder : SizedBuilder<TBuilder, TConfig>
        where TConfig : class
    {
        protected SizedBuilder(GUIHelper helper, TConfig config)
            : base(helper, config) { }

        protected readonly List<GUILayoutOption> Options = new();
        protected ControlVariant VariantValue = ControlVariant.Default;
        protected ControlSize SizeValue = ControlSize.Default;
        protected bool DisabledValue;

        public TBuilder Variant(ControlVariant variant)
        {
            VariantValue = variant;
            return Self;
        }

        public TBuilder Size(ControlSize size)
        {
            SizeValue = size;
            return Self;
        }

        public TBuilder Disabled(bool disabled = true)
        {
            DisabledValue = disabled;
            return Self;
        }

        public TBuilder Width(float width)
        {
            Options.Add(GUILayout.Width(width));
            return Self;
        }

        public TBuilder Height(float height)
        {
            Options.Add(GUILayout.Height(height));
            return Self;
        }

        public TBuilder ExpandWidth(bool expand = true)
        {
            Options.Add(GUILayout.ExpandWidth(expand));
            return Self;
        }

        public TBuilder ExpandHeight(bool expand = true)
        {
            Options.Add(GUILayout.ExpandHeight(expand));
            return Self;
        }

        public TBuilder Secondary() => Variant(ControlVariant.Secondary);

        public TBuilder Outline() => Variant(ControlVariant.Outline);

        public TBuilder Ghost() => Variant(ControlVariant.Ghost);

        public TBuilder Destructive() => Variant(ControlVariant.Destructive);

        public TBuilder Small() => Size(ControlSize.Small);

        public TBuilder Large() => Size(ControlSize.Large);

        public TBuilder Mini() => Size(ControlSize.Mini);

        protected GUILayoutOption[] GetOptions() => Options.Count == 0 ? Array.Empty<GUILayoutOption>() : Options.ToArray();
    }

    public sealed class ButtonBuilder : SizedBuilder<ButtonBuilder, ButtonConfig>
    {
        public ButtonBuilder(GUIHelper helper, string text = "")
            : base(helper, new ButtonConfig(text)) { }

        public ButtonBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public ButtonBuilder OnClick(Action onClick)
        {
            Config.OnClick = onClick;
            return this;
        }

        public ButtonBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public ButtonBuilder Opacity(float opacity)
        {
            Config.Opacity = opacity;
            return this;
        }

        public bool Draw()
        {
            Config.Variant = VariantValue;
            Config.Size = SizeValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            return Helper.Button(Config);
        }
    }

    public sealed class InputBuilder : SizedBuilder<InputBuilder, InputConfig>
    {
        private bool _labeled;

        public InputBuilder(GUIHelper helper)
            : base(helper, new InputConfig()) { }

        public InputBuilder Value(string value)
        {
            Config.Value = value;
            return this;
        }

        public InputBuilder Placeholder(string placeholder)
        {
            Config.Placeholder = placeholder;
            return this;
        }

        public InputBuilder Label(string label)
        {
            Config.Label = label;
            _labeled = true;
            return this;
        }

        public InputBuilder Password(char mask = '*')
        {
            Config.MaskChar = mask;
            Config.Label ??= string.Empty;
            return this;
        }

        public InputBuilder Focused(bool focused = true)
        {
            Config.Focused = focused;
            return this;
        }

        public InputBuilder InputWidth(int width)
        {
            Config.Width = width;
            return this;
        }

        public InputBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public InputBuilder OnChange(Action<string> onChange)
        {
            Config.OnChange = onChange;
            return this;
        }

        public string Draw()
        {
            Config.Variant = VariantValue;
            Config.Disabled = DisabledValue;
            return _labeled ? Helper.LabeledInput(Config) : Helper.Input(Config);
        }
    }

    public sealed class ToggleBuilder : SizedBuilder<ToggleBuilder, ToggleConfig>
    {
        public ToggleBuilder(GUIHelper helper, string text, bool value)
            : base(helper, new ToggleConfig { Text = text, Value = value }) { }

        public ToggleBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public ToggleBuilder Value(bool value)
        {
            Config.Value = value;
            return this;
        }

        public ToggleBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public ToggleBuilder OnToggle(Action<bool> onToggle)
        {
            Config.OnToggle = onToggle;
            return this;
        }

        public bool Draw()
        {
            Config.Variant = VariantValue;
            Config.Size = SizeValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            return Helper.Toggle(Config);
        }
    }

    public sealed class CheckboxBuilder : SizedBuilder<CheckboxBuilder, CheckboxConfig>
    {
        public CheckboxBuilder(GUIHelper helper, string text, bool value)
            : base(helper, new CheckboxConfig { Text = text, Value = value }) { }

        public CheckboxBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public CheckboxBuilder OnToggle(Action<bool> onToggle)
        {
            Config.OnToggle = onToggle;
            return this;
        }

        public bool Draw()
        {
            Config.Variant = VariantValue;
            Config.Size = SizeValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            return Helper.Checkbox(Config);
        }
    }

    public sealed class SwitchBuilder : SizedBuilder<SwitchBuilder, SwitchConfig>
    {
        public SwitchBuilder(GUIHelper helper, string text, bool value)
            : base(helper, new SwitchConfig { Text = text, Value = value }) { }

        public SwitchBuilder OnToggle(Action<bool> onToggle)
        {
            Config.OnToggle = onToggle;
            return this;
        }

        public bool Draw()
        {
            Config.Variant = VariantValue;
            Config.Size = SizeValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            return Helper.Switch(Config);
        }
    }

    public sealed class LabelBuilder : SizedBuilder<LabelBuilder, LabelConfig>
    {
        public LabelBuilder(GUIHelper helper, string text = "")
            : base(helper, new LabelConfig { Text = text }) { }

        public LabelBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public LabelBuilder Muted() => Variant(ControlVariant.Muted);

        public LabelBuilder Icon(Texture2D image, IconPosition position = IconPosition.Left, float size = DesignTokens.Icon.Default, float spacing = DesignTokens.Spacing.XS)
        {
            Config.Icon = new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public void Draw()
        {
            Config.Variant = VariantValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            Helper.Label(Config);
        }
    }

    public sealed class BadgeBuilder : SizedBuilder<BadgeBuilder, BadgeConfig>
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
            Config.Icon = new IconConfig(image, position) { Size = size, Spacing = spacing };
            return this;
        }

        public BadgeBuilder Count(int count, int maxCount = 99)
        {
            Config.Count = count;
            Config.MaxCount = maxCount;
            Config.Text = count > maxCount ? $"{maxCount}+" : count.ToString();
            return this;
        }

        public BadgeBuilder StatusDot(bool active = true)
        {
            Config.ShowStatusDot = true;
            Config.IsActive = active;
            return this;
        }

        public BadgeBuilder Progress(float progress)
        {
            Config.Progress = progress;
            return this;
        }

        public BadgeBuilder CornerRadius(float radius)
        {
            Config.CornerRadius = radius;
            return this;
        }

        public void Draw()
        {
            Config.Variant = VariantValue;
            Config.Size = SizeValue;
            Config.Options = GetOptions();
            Helper.Badge(Config);
        }
    }

    public sealed class AvatarBuilder : SizedBuilder<AvatarBuilder, AvatarConfig>
    {
        public AvatarBuilder(GUIHelper helper, Texture2D image = null)
            : base(helper, new AvatarConfig { Image = image }) { }

        public AvatarBuilder Image(Texture2D image)
        {
            Config.Image = image;
            return this;
        }

        public AvatarBuilder Fallback(string text)
        {
            Config.FallbackText = text;
            return this;
        }

        public AvatarBuilder Circle()
        {
            Config.Shape = AvatarShape.Circle;
            return this;
        }

        public AvatarBuilder Square()
        {
            Config.Shape = AvatarShape.Square;
            return this;
        }

        public AvatarBuilder Rounded()
        {
            Config.Shape = AvatarShape.Rounded;
            return this;
        }

        public AvatarBuilder Online(bool online = true)
        {
            Config.IsOnline = online;
            return this;
        }

        public AvatarBuilder Border(Color color)
        {
            Config.BorderColor = color;
            return this;
        }

        public AvatarBuilder NameBelow(string name)
        {
            Config.Name = name;
            Config.ShowNameBelow = true;
            return this;
        }

        public void Draw()
        {
            Config.Size = SizeValue;
            Config.Options = GetOptions();
            Helper.Avatar(Config);
        }
    }

    public sealed class CardBuilder : FluentBuilder<CardBuilder, CardConfig>
    {
        public CardBuilder(GUIHelper helper, string title = null)
            : base(helper, new CardConfig { Title = title }) { }

        public CardBuilder Title(string title)
        {
            Config.Title = title;
            return this;
        }

        public CardBuilder Description(string description)
        {
            Config.Description = description;
            return this;
        }

        public CardBuilder Subtitle(string subtitle)
        {
            Config.Subtitle = subtitle;
            return this;
        }

        public CardBuilder Content(string content)
        {
            Config.Content = content;
            return this;
        }

        public CardBuilder Footer(Action footer)
        {
            Config.FooterContent = footer;
            return this;
        }

        public CardBuilder Header(Action header)
        {
            Config.HeaderContent = header;
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

        public CardBuilder CardSize(float width, float height)
        {
            Config.Width = width;
            Config.Height = height;
            return this;
        }

        public void Draw()
        {
            Helper.Card(Config);
        }
    }

    public sealed class ProgressBuilder : FluentBuilder<ProgressBuilder, ProgressConfig>
    {
        public ProgressBuilder(GUIHelper helper, float value = 0f)
            : base(helper, new ProgressConfig { Value = value }) { }

        public ProgressBuilder Value(float value)
        {
            Config.Value = value;
            return this;
        }

        public ProgressBuilder ProgressLabel(string label)
        {
            Config.Label = label;
            return this;
        }

        public ProgressBuilder ProgressWidth(float width)
        {
            Config.Width = width;
            return this;
        }

        public ProgressBuilder ProgressHeight(float height)
        {
            Config.Height = height;
            return this;
        }

        public ProgressBuilder Circular(float size = DesignTokens.Height.Small)
        {
            Config.Size = size;
            return this;
        }

        public ProgressBuilder ShowPercentage(bool show = true)
        {
            Config.ShowPercentage = show;
            return this;
        }

        public void Draw()
        {
            Helper.Progress(Config);
        }
    }

    public sealed class SeparatorBuilder : FluentBuilder<SeparatorBuilder, SeparatorConfig>
    {
        public SeparatorBuilder(GUIHelper helper)
            : base(helper, new SeparatorConfig()) { }

        public SeparatorBuilder Horizontal()
        {
            Config.Orientation = SeparatorOrientation.Horizontal;
            return this;
        }

        public SeparatorBuilder Vertical()
        {
            Config.Orientation = SeparatorOrientation.Vertical;
            return this;
        }

        public SeparatorBuilder WithLabel(string text)
        {
            Config.Text = text;
            return this;
        }

        public SeparatorBuilder Spacing(float before, float after)
        {
            Config.SpacingBefore = before;
            Config.SpacingAfter = after;
            return this;
        }

        public void Draw()
        {
            Helper.Separator(Config);
        }
    }

    public sealed class SliderBuilder : SizedBuilder<SliderBuilder, SliderConfig>
    {
        public SliderBuilder(GUIHelper helper, float value = 0f)
            : base(helper, new SliderConfig { Value = value }) { }

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

        public SliderBuilder SliderLabel(string label)
        {
            Config.Label = label;
            return this;
        }

        public SliderBuilder ShowValue(bool show = true)
        {
            Config.ShowValue = show;
            return this;
        }

        public SliderBuilder Format(string format)
        {
            Config.ValueFormat = format;
            return this;
        }

        public SliderBuilder OnChange(Action<float> onChange)
        {
            Config.OnChange = onChange;
            return this;
        }

        public float Draw()
        {
            Config.Variant = VariantValue;
            Config.Size = SizeValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            return Helper.Slider(Config);
        }
    }

    public sealed class TextAreaBuilder : SizedBuilder<TextAreaBuilder, TextAreaConfig>
    {
        public TextAreaBuilder(GUIHelper helper, string text = "")
            : base(helper, new TextAreaConfig { Text = text }) { }

        public TextAreaBuilder Text(string text)
        {
            Config.Text = text;
            return this;
        }

        public TextAreaBuilder Placeholder(string placeholder)
        {
            Config.Placeholder = placeholder;
            return this;
        }

        public TextAreaBuilder TextAreaLabel(string label)
        {
            Config.Label = label;
            return this;
        }

        public TextAreaBuilder TextAreaMinHeight(float height)
        {
            Config.MinHeight = height;
            return this;
        }

        public TextAreaBuilder TextAreaMaxHeight(float height)
        {
            Config.MaxHeight = height;
            return this;
        }

        public TextAreaBuilder MaxLength(int maxLength)
        {
            Config.MaxLength = maxLength;
            return this;
        }

        public TextAreaBuilder ShowCharCount(bool show = true)
        {
            Config.ShowCharCount = show;
            return this;
        }

        public string Draw()
        {
            Config.Variant = VariantValue;
            Config.Disabled = DisabledValue;
            Config.Options = GetOptions();
            return Helper.TextArea(Config);
        }
    }

    public sealed class TableBuilder : FluentBuilder<TableBuilder, TableConfig>
    {
        public TableBuilder(GUIHelper helper, params string[] headers)
            : base(helper, new TableConfig { Headers = headers ?? Array.Empty<string>() }) { }

        public TableBuilder Headers(params string[] headers)
        {
            Config.Headers = headers;
            return this;
        }

        public TableBuilder Data(string[,] data)
        {
            Config.Data = data;
            return this;
        }

        public TableBuilder Variant(ControlVariant variant)
        {
            Config.Variant = variant;
            return this;
        }

        public TableBuilder Size(ControlSize size)
        {
            Config.Size = size;
            return this;
        }

        public void Draw()
        {
            Helper.Table(Config);
        }
    }

    public sealed class ChartBuilder : FluentBuilder<ChartBuilder, ChartConfig>
    {
        public ChartBuilder(GUIHelper helper, ChartType type = ChartType.Bar)
            : base(helper, new ChartConfig { ChartType = type, Series = new List<ChartSeries>() }) { }

        public ChartBuilder Type(ChartType type)
        {
            Config.ChartType = type;
            return this;
        }

        public ChartBuilder Series(params ChartSeries[] series)
        {
            Config.Series.AddRange(series);
            return this;
        }

        public ChartBuilder ChartSize(float width, float height)
        {
            Config.Size = new Vector2(width, height);
            return this;
        }

        public void Draw()
        {
            Helper.Chart(Config);
        }
    }

    public sealed class DialogBuilder : FluentBuilder<DialogBuilder, DialogConfig>
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

        public DialogBuilder CloseOnOverlayClick(bool close = true)
        {
            Config.CloseOnOverlayClick = close;
            return this;
        }

        public DialogBuilder ZIndex(int zIndex)
        {
            Config.ZIndex = zIndex;
            return this;
        }

        public void Open()
        {
            Helper.OpenDialog(Config.Id);
        }

        public void Close()
        {
            Helper.CloseDialog();
        }

        public void Draw()
        {
            Helper.Dialog(Config);
        }
    }

    public sealed class PopoverBuilder : FluentBuilder<PopoverBuilder, PopoverConfig>
    {
        private string _id = "popover";
        private int _zIndex = -1;

        public PopoverBuilder(GUIHelper helper, Action content = null)
            : base(helper, new PopoverConfig { Content = content }) { }

        public PopoverBuilder Content(Action content)
        {
            Config.Content = content;
            return this;
        }

        public PopoverBuilder Id(string id)
        {
            _id = id;
            return this;
        }

        public PopoverBuilder ZIndex(int zIndex)
        {
            _zIndex = zIndex;
            return this;
        }

        public void Open()
        {
            Helper.OpenPopover(_id, _zIndex);
        }

        public void Close()
        {
            Helper.ClosePopover();
        }

        public bool IsOpen() => Helper.IsPopoverOpen();

        public void Draw()
        {
            Helper.Popover(Config);
        }
    }

    public sealed class SelectBuilder : FluentBuilder<SelectBuilder, SelectConfig>
    {
        public SelectBuilder(GUIHelper helper, int selectedIndex = 0, params string[] items)
            : base(helper, new SelectConfig { SelectedIndex = selectedIndex, Items = items ?? Array.Empty<string>() }) { }

        public SelectBuilder Items(params string[] items)
        {
            Config.Items = items;
            return this;
        }

        public SelectBuilder SelectedIndex(int index)
        {
            Config.SelectedIndex = index;
            return this;
        }

        public SelectBuilder OnChange(Action<int> onChange)
        {
            Config.OnChange = onChange;
            return this;
        }

        public void Open(string id = "select")
        {
            Helper.OpenSelect(id);
        }

        public bool IsOpen() => Helper.IsSelectOpen();

        public int Draw()
        {
            return Helper.Select(Config);
        }
    }

    public sealed class DropdownMenuBuilder : FluentBuilder<DropdownMenuBuilder, DropdownMenuConfig>
    {
        public DropdownMenuBuilder(GUIHelper helper)
            : base(helper, new DropdownMenuConfig(new List<DropdownMenuItem>())) { }

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

        public DropdownMenuBuilder Item(string text, Action onClick = null, Texture2D icon = null)
        {
            Config.Items.Add(new DropdownMenuItem(DropdownMenuItemType.Item, text, onClick, false, icon));
            return this;
        }

        public void Draw()
        {
            Helper.DropdownMenu(Config);
        }
    }

    public sealed class ToastBuilder : FluentBuilder<ToastBuilder, ToastConfig>
    {
        public ToastBuilder(GUIHelper helper, string title = null, string description = null)
            : base(helper, new ToastConfig { Title = title, Description = description }) { }

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

        public ToastBuilder Success() => Variant(ToastVariant.Success);

        public ToastBuilder Error() => Variant(ToastVariant.Error);

        public ToastBuilder Warning() => Variant(ToastVariant.Warning);

        public ToastBuilder Info() => Variant(ToastVariant.Info);

        public ToastBuilder Duration(float milliseconds)
        {
            Config.DurationMs = milliseconds;
            return this;
        }

        public ToastBuilder Action(string label, Action onAction)
        {
            Config.ActionLabel = label;
            Config.OnAction = onAction;
            return this;
        }

        public ToastBuilder Position(ToastPosition position)
        {
            Config.Position = position;
            return this;
        }

        public ToastBuilder Stack(ToastStackDirection direction)
        {
            Config.StackDirection = direction;
            return this;
        }

        public void Show()
        {
            Helper.ShowToast(Config);
        }
    }

    public sealed class MenuItemGroupBuilder
    {
        internal readonly List<MenuBar.MenuItem> Items = new();

        public MenuItemGroupBuilder Item(string text, Action onClick = null, bool disabled = false, string shortcut = "")
        {
            Items.Add(new MenuBar.MenuItem(text, onClick, disabled, null, shortcut));
            return this;
        }

        public MenuItemGroupBuilder Separator()
        {
            Items.Add(MenuBar.MenuItem.Separator());
            return this;
        }

        public MenuItemGroupBuilder Header(string text)
        {
            Items.Add(MenuBar.MenuItem.Header(text));
            return this;
        }
    }

    public sealed class MenuBarBuilder : FluentBuilder<MenuBarBuilder, MenuBar.MenuBarConfig>
    {
        public MenuBarBuilder(GUIHelper helper)
            : base(helper, new MenuBar.MenuBarConfig(new List<MenuBar.MenuItem>())) { }

        public MenuBarBuilder Item(string text, Action<MenuItemGroupBuilder> children = null, Action onClick = null, bool disabled = false, string shortcut = "")
        {
            var childBuilder = new MenuItemGroupBuilder();
            children?.Invoke(childBuilder);
            Config.Items.Add(new MenuBar.MenuItem(text, onClick, disabled, childBuilder.Items, shortcut));
            return this;
        }

        public void Draw()
        {
            Helper.MenuBar(Config);
        }
    }

    public partial class GUIHelper
    {
        public ButtonBuilder CreateButton(string text = "") => new(this, text);

        public InputBuilder CreateInput() => new(this);

        public ToggleBuilder CreateToggle(string text, bool value) => new(this, text, value);

        public CheckboxBuilder CreateCheckbox(string text, bool value) => new(this, text, value);

        public SwitchBuilder CreateSwitch(string text, bool value) => new(this, text, value);

        public LabelBuilder CreateLabel(string text = "") => new(this, text);

        public BadgeBuilder CreateBadge(string text = "Badge") => new(this, text);

        public AvatarBuilder CreateAvatar(Texture2D image = null) => new(this, image);

        public CardBuilder CreateCard(string title = null) => new(this, title);

        public ProgressBuilder CreateProgress(float value = 0f) => new(this, value);

        public SeparatorBuilder CreateSeparator() => new(this);

        public SliderBuilder CreateSlider(float value = 0f) => new(this, value);

        public TextAreaBuilder CreateTextArea(string text = "") => new(this, text);

        public TableBuilder CreateTable(params string[] headers) => new(this, headers);

        public ChartBuilder CreateChart(ChartType type = ChartType.Bar) => new(this, type);

        public DialogBuilder CreateDialog(string id) => new(this, id);

        public PopoverBuilder CreatePopover(Action content = null) => new(this, content);

        public SelectBuilder CreateSelect(int selectedIndex = 0, params string[] items) => new(this, selectedIndex, items);

        public DropdownMenuBuilder CreateDropdownMenu() => new(this);

        public ToastBuilder CreateToast(string title = null, string description = null) => new(this, title, description);

        public MenuBarBuilder CreateMenuBar() => new(this);
    }
}
