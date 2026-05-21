using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Utils
{
    public abstract class GuiConfigBase
    {
        private GUILayoutOption[] _layoutOptions = Array.Empty<GUILayoutOption>();

        public GUILayoutOption[] LayoutOptions
        {
            get => _layoutOptions;
            set => _layoutOptions = value ?? Array.Empty<GUILayoutOption>();
        }
    }

    public abstract class ComponentConfigBase : GuiConfigBase
    {
        public string Id { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public bool IsDisabled { get; set; }
        public ComponentAppearance Appearance { get; set; }
    }

    public abstract class RectConfigBase : ComponentConfigBase
    {
        public Rect? Rect { get; set; }
    }

    #region Enums
    public enum IconPosition
    {
        Left,
        Right,
        Above,
        Below,
    }

    public enum DropdownMenuItemType
    {
        Item,
        Separator,
        Header,
    }

    public enum ChartType
    {
        Line,
        Bar,
        Area,
        Pie,
        Scatter,
    }

    public enum ToastVariant
    {
        Default,
        Success,
        Error,
        Warning,
        Info,
    }

    public enum ToastPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        CenterLeft,
        Center,
        CenterRight,
    }

    public enum ToastStackDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    public enum InputKind
    {
        Text,
        Password,
    }
    #endregion

    #region Shared Configs
    public class IconConfig
    {
        public Texture2D Image { get; set; }
        public IconPosition Position { get; set; } = IconPosition.Left;
        public float Size { get; set; } = DesignTokens.Icon.Default;
        public float Spacing { get; set; } = DesignTokens.Spacing.XS;

        public IconConfig() { }

        public IconConfig(Texture2D image)
        {
            Image = image;
        }

        public IconConfig(Texture2D image, IconPosition position)
        {
            Image = image;
            Position = position;
        }

        public bool HasIcon => Image != null;
    }
    #endregion

    #region Control Configs
    public abstract class LabeledControlConfigBase : RectConfigBase
    {
        public string Label { get; set; }
        public ControlVariant LabelVariant { get; set; } = ControlVariant.Default;
        public string HelperText { get; set; }
        public string ErrorText { get; set; }
    }

    public abstract class BoolControlConfigBase : LabeledControlConfigBase
    {
        public bool Value { get; set; }
        public IconConfig Icon { get; set; }
        public bool FullRowClick { get; set; } = true;
        public Action<bool> OnValueChanged { get; set; }
    }

    public class ButtonConfig : RectConfigBase
    {
        public string Text { get; set; }
        public IconConfig Icon { get; set; }
        public float Opacity { get; set; } = 1f;
        public Action OnClick { get; set; }

        public ButtonConfig() { }

        public ButtonConfig(string text)
        {
            Text = text;
        }
    }

    public class InputConfig : LabeledControlConfigBase
    {
        public string Value { get; set; }
        public string Placeholder { get; set; }
        public IconConfig Icon { get; set; }
        public int Width { get; set; } = -1;
        public float Height { get; set; } = DesignTokens.Height.Default;
        public char MaskCharacter { get; set; } = '*';
        public int MaxLength { get; set; } = 1000;
        public bool AutoFocus { get; set; }
        public InputKind InputKind { get; set; } = InputKind.Text;
        public Action<string> OnValueChanged { get; set; }
    }

    public class CheckboxConfig : BoolControlConfigBase
    {
        public bool ShowCheckmark { get; set; }
    }

    public class SwitchConfig : BoolControlConfigBase { }

    public class ToggleConfig : BoolControlConfigBase { }

    public class SliderConfig : ComponentConfigBase
    {
        public string Label { get; set; }
        public float Value { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; } = 1f;
        public float Step { get; set; }
        public string ValueFormat { get; set; } = "F2";
        public bool ShowValue { get; set; } = true;
        public Action<float> OnValueChanged { get; set; }

        public SliderConfig() { }

        public SliderConfig(float value, float min = 0f, float max = 1f)
        {
            Value = value;
            MinValue = min;
            MaxValue = max;
        }
    }

    public class RangeSliderConfig : ComponentConfigBase
    {
        public string Label { get; set; }
        public float LowerValue { get; set; }
        public float UpperValue { get; set; } = 1f;
        public float MinValue { get; set; }
        public float MaxValue { get; set; } = 1f;
        public float Step { get; set; }
        public string ValueFormat { get; set; } = "F0";
        public bool ShowValue { get; set; } = true;
        public Action<float, float> OnValueChanged { get; set; }

        public RangeSliderConfig() { }

        public RangeSliderConfig(float lowerValue, float upperValue, float min = 0f, float max = 1f)
        {
            LowerValue = lowerValue;
            UpperValue = upperValue;
            MinValue = min;
            MaxValue = max;
        }
    }

    public class TextAreaConfig : RectConfigBase
    {
        private static int _counter;

        public string Value { get; set; }
        public string Placeholder { get; set; }
        public string Label { get; set; }
        public float MinHeight { get; set; } = 80f;
        public float MaxHeight { get; set; } = 300f;
        public int MaxLength { get; set; } = -1;
        public bool ShowCharCount { get; set; } = true;

        public TextAreaConfig()
        {
            Id = "textarea_" + System.Threading.Interlocked.Increment(ref _counter);
        }
    }

    public class SelectOption
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public Texture2D Icon { get; set; }
        public bool IsDisabled { get; set; }

        public SelectOption() { }

        public SelectOption(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }

    public class SelectConfig : LabeledControlConfigBase
    {
        public SelectOption[] Options { get; set; } = Array.Empty<SelectOption>();
        public int SelectedIndex { get; set; }
        public string Placeholder { get; set; } = "Select...";
        public float Width { get; set; } = -1f;
        public float MaxHeight { get; set; } = 220f;
        public bool CloseOnSelect { get; set; } = true;
        public Action<int> OnSelectionChanged { get; set; }
    }
    #endregion

    #region Layout Configs
    public class TableConfig : RectConfigBase
    {
        public string[] ColumnHeaders { get; set; }
        public string[,] Rows { get; set; }
        public object[,] ObjectRows { get; set; }
        public int[] SortColumnIndices { get; set; }
        public bool[] SortAscending { get; set; }
        public bool[] SelectedRowFlags { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public string SearchText { get; set; }
        public string[,] FilteredRows { get; set; }
        public float[] ColumnWidths { get; set; }
        public Action<int, bool> OnSortChanged { get; set; }
        public Action<int, bool> OnSelectionChanged { get; set; }
        public Action<int> OnPageChanged { get; set; }
        public Action<string> OnSearchChanged { get; set; }
        public Action<object, int, int> CellRenderer { get; set; }
    }

    public class TabsConfig : ComponentConfigBase
    {
        public string[] TabLabels { get; set; } = Array.Empty<string>();
        public int SelectedIndex { get; set; }
        public Action Content { get; set; }
        public Action<int> OnSelectionChanged { get; set; }
        public Action<int> OnTabClosed { get; set; }
        public int MaxLines { get; set; } = 1;
        public float TabWidth { get; set; } = 120f;
        public TabPosition Position { get; set; } = TabPosition.Top;
        public TabSide Side { get; set; } = TabSide.Left;
        public IndicatorStyle IndicatorStyle { get; set; } = IndicatorStyle.Underline;
        public bool ShowIndicator { get; set; }
        public bool EnableOverflowScroll { get; set; }
        public bool[] DisabledTabs { get; set; } = Array.Empty<bool>();
        public bool[] ClosableTabs { get; set; }
        public Texture2D[] TabIcons { get; set; }

        public TabsConfig() { }

        public TabsConfig(string[] tabLabels, int selectedIndex)
        {
            TabLabels = tabLabels ?? Array.Empty<string>();
            SelectedIndex = selectedIndex;
            DisabledTabs = new bool[TabLabels.Length];
        }
    }

    public class CardConfig : ComponentConfigBase
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Texture2D Image { get; set; }
        public Texture2D Avatar { get; set; }
        public Action HeaderContent { get; set; }
        public Action FooterContent { get; set; }
        public float Width { get; set; } = -1f;
        public float Height { get; set; } = -1f;
    }

    public class SeparatorConfig : RectConfigBase
    {
        public SeparatorOrientation Orientation { get; set; } = SeparatorOrientation.Horizontal;
        public bool IsDecorative { get; set; } = true;
        public string Text { get; set; }
        public float SpacingBefore { get; set; } = DesignTokens.Spacing.SM;
        public float SpacingAfter { get; set; } = DesignTokens.Spacing.SM;
    }

    public class NavigationItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool IsDisabled { get; set; }

        public NavigationItem() { }

        public NavigationItem(string id, string label, string icon = null)
        {
            Id = id;
            Label = label;
            Icon = icon;
        }
    }

    public class NavigationConfig : ComponentConfigBase
    {
        public NavigationItem[] Items { get; set; } = Array.Empty<NavigationItem>();
        public int SelectedIndex { get; set; }
        public float Width { get; set; } = 70f;
        public string LogoText { get; set; } = "U";
        public IndicatorStyle IndicatorStyle { get; set; } = IndicatorStyle.Border;
        public Color IndicatorColor { get; set; } = new(0.2f, 0.7f, 1f, 1f);
        public bool ShowIndicator { get; set; } = true;
        public Action<int> OnSelectionChanged { get; set; }
    }

    public class LayerConfig : ComponentConfigBase
    {
        public Vector2 OpenPosition { get; set; }
        public float Width { get; set; } = 200f;
        public float Height { get; set; } = 150f;
        public int ZIndex { get; set; } = 100;
        public bool CloseOnClickOutside { get; set; } = true;
        public bool ShowOverlay { get; set; }
        public bool DrawChrome { get; set; } = true;
        public Action Content { get; set; }
        public Action OnClose { get; set; }
    }
    #endregion

    #region Display Configs
    public class LabelConfig : RectConfigBase
    {
        public string Text { get; set; }
        public IconConfig Icon { get; set; }
    }

    public class AvatarConfig : RectConfigBase
    {
        public Texture2D Image { get; set; }
        public string FallbackText { get; set; }
        public string Name { get; set; }
        public AvatarShape Shape { get; set; } = AvatarShape.Circle;
        public Color BorderColor { get; set; } = Color.clear;
        public bool IsOnline { get; set; }
        public bool ShowNameBelow { get; set; }
    }

    public class BadgeConfig : RectConfigBase
    {
        public string Text { get; set; } = "Badge";
        public IconConfig Icon { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; } = 99;
        public float Progress { get; set; }
        public float CornerRadius { get; set; } = 8f;
        public bool IsActive { get; set; }
        public bool ShowStatusDot { get; set; }
    }

    public class ProgressConfig : RectConfigBase
    {
        public float Value { get; set; }
        public string Label { get; set; }
        public float Width { get; set; } = -1f;
        public float Height { get; set; } = -1f;
        public new float Size { get; set; } = 32f;
        public bool ShowPercentage { get; set; } = true;
    }

    public class ChartConfig : ComponentConfigBase
    {
        public List<ChartSeries> Series { get; set; }
        public ChartType ChartType { get; set; }
        public new Vector2 Size { get; set; } = new(400f, 300f);

        public ChartConfig() { }

        public ChartConfig(List<ChartSeries> series, ChartType chartType)
        {
            Series = series;
            ChartType = chartType;
        }
    }

    public class DialogConfig : ComponentConfigBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Action Content { get; set; }
        public Action Footer { get; set; }
        public float Width { get; set; } = 400f;
        public float Height { get; set; } = 300f;
        public bool CloseOnOverlayClick { get; set; }
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Modal;
        public Rect? ParentWindowRect { get; set; }

        public DialogConfig()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class TooltipConfig : ComponentConfigBase
    {
        public float HoverDelaySeconds { get; set; } = 0.4f;
        public float MaxWidth { get; set; } = 280f;
        public float ShadowOffset { get; set; } = 4f;
        public float MouseOffset { get; set; } = 12f;
    }

    public class ToastConfig : ComponentConfigBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ActionLabel { get; set; }
        public new ToastVariant Variant { get; set; } = ToastVariant.Default;
        public ToastPosition Position { get; set; } = ToastPosition.BottomRight;
        public ToastStackDirection StackDirection { get; set; } = ToastStackDirection.Up;
        public float DurationMs { get; set; } = 5000f;
        public float Margin { get; set; } = DesignTokens.Spacing.LG;
        public float Spacing { get; set; } = DesignTokens.Spacing.MD;
        public float Width { get; set; } = 360f;
        public float MinHeight { get; set; } = 90f;
        public float MinWidth { get; set; } = 280f;
        public float MaxWidth { get; set; } = 500f;
        public float BorderRadius { get; set; } = DesignTokens.Radius.LG;
        public float Padding { get; set; } = DesignTokens.Spacing.LG;
        public bool IsDismissible { get; set; } = true;
        public bool ShowProgressBar { get; set; } = true;
        public bool ShowAccentBar { get; set; } = true;
        public bool EnablePauseOnHover { get; set; } = true;
        public float HoverPauseDelay { get; set; } = DesignTokens.Animation.DurationNormal;
        public bool EnableClickToDismiss { get; set; }
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Toast;
        public Action OnAction { get; set; }

        public ToastConfig()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    #endregion

    #region Data Configs
    public class CalendarConfig : ComponentConfigBase
    {
        public DateTime? SelectedDate { get; set; }
        public List<DateTime> DisabledDates { get; set; } = new();
        public List<(DateTime Start, DateTime End)> Ranges { get; set; } = new();
        public Action<DateTime> OnDateSelected { get; set; }
    }

    public class DatePickerConfig : ComponentConfigBase
    {
        private static int _counter;

        public string Label { get; set; }
        public string Placeholder { get; set; } = "Select date";
        public string DisplayFormat { get; set; } = "MMM d, yyyy";
        public DateTime? SelectedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public DatePickerConfig()
        {
            Id = "datepicker_" + System.Threading.Interlocked.Increment(ref _counter);
        }
    }

    public class DataTableConfig : ComponentConfigBase
    {
        public List<DataTableColumn> Columns { get; set; } = new();
        public List<DataTableRow> Rows { get; set; } = new();
        public bool ShowPagination { get; set; } = true;
        public bool ShowSearch { get; set; } = true;
        public bool ShowSelection { get; set; } = true;
        public bool ShowColumnToggle { get; set; }
    }

    public class DataTableColumn
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string AccessorKey { get; set; }
        public float Width { get; set; } = 120f;
        public TextAnchor Alignment { get; set; } = TextAnchor.MiddleLeft;
        public bool IsSortable { get; set; } = true;
        public bool IsFilterable { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public Func<object, string> CellRenderer { get; set; }

        public DataTableColumn() { }

        public DataTableColumn(string id, string header, string accessorKey, float width = 120f)
        {
            Id = id;
            Header = header;
            AccessorKey = accessorKey;
            Width = width;
        }
    }

    public class DataTableRow
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Dictionary<string, object> Data { get; set; } = new();

        public DataTableRow() { }

        public DataTableRow(string id, Dictionary<string, object> data)
        {
            Id = id;
            Data = data ?? new Dictionary<string, object>();
        }

        public T GetValue<T>(string key, T defaultValue = default)
        {
            if (!Data.TryGetValue(key, out var value) || value == null)
                return defaultValue;

            if (value is T typed)
                return typed;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }

    public class DataTableState
    {
        public Dictionary<string, bool> ColumnVisibility { get; } = new();
        public List<string> SelectedRows { get; } = new();
        public string FilterText { get; set; }
        public string SortColumn { get; set; }
        public bool SortAscending { get; set; } = true;
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public bool ShowColumnToggle { get; set; }
    }
    #endregion

    #region Dropdown & Popover Configs
    public class DropdownMenuItem
    {
        public DropdownMenuItemType Type { get; set; }
        public string Text { get; set; }
        public Texture2D Icon { get; set; }
        public bool IsDisabled { get; set; }
        public Action OnClick { get; set; }
        public List<DropdownMenuItem> SubItems { get; set; } = new();

        public DropdownMenuItem(DropdownMenuItemType type, string text = null, Action onClick = null, Texture2D icon = null, bool isDisabled = false)
        {
            Type = type;
            Text = text;
            Icon = icon;
            OnClick = onClick;
            IsDisabled = isDisabled;
        }
    }

    public class DropdownMenuConfig : ComponentConfigBase
    {
        public List<DropdownMenuItem> Items { get; set; } = new();
        public float Width { get; set; } = 240f;
        public float MaxHeight { get; set; } = 220f;
        public bool CloseOnSelect { get; set; } = true;
        public bool CloseOnClickOutside { get; set; } = true;
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Dropdown;
        public Func<bool> Trigger { get; set; }
        public Rect? AnchorRect { get; set; }

        public DropdownMenuConfig() { }

        public DropdownMenuConfig(List<DropdownMenuItem> items)
        {
            Items = items ?? new List<DropdownMenuItem>();
        }
    }

    public class PopoverConfig : ComponentConfigBase
    {
        public Action Content { get; set; }
    }
    #endregion

    #region Chart Data
    [Serializable]
    public class ChartDataPoint
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public Color Color { get; set; }

        public ChartDataPoint() { }

        public ChartDataPoint(string name, float value, Color color = default)
        {
            Name = name;
            Value = value;
            Color = color == default ? Color.white : color;
        }
    }

    [Serializable]
    public class ChartSeries
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public Color Color { get; set; }
        public List<ChartDataPoint> Data { get; set; } = new();
        public bool Visible { get; set; } = true;

        public ChartSeries() { }

        public ChartSeries(string key, string label, Color color = default)
        {
            Key = key;
            Label = label;
            Color = color == default ? Color.white : color;
        }
    }
    #endregion

    #region Theme Configs
    public class ThemeChangerConfig : ComponentConfigBase
    {
        public float Width { get; set; } = 200f;
        public float DropdownHeight { get; set; } = 250f;
        public bool ShowPreview { get; set; } = true;
        public Action<Theme> OnThemeChanged { get; set; }

        public ThemeChangerConfig()
        {
            Id = "theme_changer";
        }
    }

    public class FontChangerConfig : ComponentConfigBase
    {
        public float Width { get; set; } = 240f;
        public float DropdownHeight { get; set; } = 320f;
        public bool ShowPreview { get; set; } = true;
        public string PreviewText { get; set; } = "Aa";
        public Action<string> OnFontChanged { get; set; }

        public FontChangerConfig()
        {
            Id = "font_changer";
        }
    }
    #endregion
}
