using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Utils
{
    public enum IconPosition
    {
        Left,
        Right,
        Above,
        Below,
    }

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

    public class ButtonConfig
    {
        public string Text { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Action OnClick { get; set; }
        public bool Disabled { get; set; }
        public float Opacity { get; set; } = 1f;
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();

        public ButtonConfig() { }

        public ButtonConfig(string text)
        {
            Text = text;
        }
    }

    public class InputConfig
    {
        public string Value { get; set; }
        public string Placeholder { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public bool Disabled { get; set; }
        public bool Focused { get; set; }
        public int Width { get; set; } = -1;
        public Action<string> OnChange { get; set; }
        public string Label { get; set; }
        public ControlVariant LabelVariant { get; set; } = ControlVariant.Default;
        public char MaskChar { get; set; } = '*';
        public int MaxLength { get; set; } = 1000;
        public float Height { get; set; } = 60f;
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class CheckboxConfig
    {
        public string Text { get; set; }
        public bool Value { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Action<bool> OnToggle { get; set; }
        public bool Disabled { get; set; }
        public IconConfig Icon { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
        public bool ShowCheckmark { get; set; }
    }

    public class SwitchConfig
    {
        public string Text { get; set; } = "Switch";
        public bool Value { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Action<bool> OnToggle { get; set; }
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class LabelConfig
    {
        public string Text { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class TableConfig
    {
        public string[] Headers { get; set; }
        public string[,] Data { get; set; }
        public object[,] ObjectData { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Rect? Rect { get; set; }
        public int[] SortColumns { get; set; }
        public bool[] SortAscending { get; set; }
        public Action<int, bool> OnSort { get; set; }
        public bool[] SelectedRows { get; set; }
        public Action<int, bool> OnSelectionChange { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public Action<int> OnPageChange { get; set; }
        public string SearchQuery { get; set; }
        public string[,] FilteredData { get; set; }
        public Action<string> OnSearch { get; set; }
        public float[] ColumnWidths { get; set; }
        public Action<object, int, int> CellRenderer { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class DialogConfig
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Action Content { get; set; }
        public Action Footer { get; set; }
        public float Width { get; set; } = 400f;
        public float Height { get; set; } = 300f;
        public bool CloseOnOverlayClick { get; set; }
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Modal;
    }

    public class AvatarConfig
    {
        public Texture2D Image { get; set; }
        public string FallbackText { get; set; }
        public ControlSize Size { get; set; } = ControlSize.Default;
        public AvatarShape Shape { get; set; } = AvatarShape.Circle;
        public bool IsOnline { get; set; }
        public string Name { get; set; }
        public bool ShowNameBelow { get; set; }
        public Color BorderColor { get; set; } = Color.clear;
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class BadgeConfig
    {
        public string Text { get; set; } = "Badge";
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public IconConfig Icon { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; } = 99;
        public bool IsActive { get; set; }
        public bool ShowStatusDot { get; set; }
        public float Progress { get; set; }
        public float CornerRadius { get; set; } = 8f;
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class ChartConfig
    {
        public List<ChartSeries> Series { get; set; }
        public ChartType ChartType { get; set; }
        public Vector2 Size { get; set; } = new(400f, 300f);
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();

        public ChartConfig() { }

        public ChartConfig(List<ChartSeries> series, ChartType chartType)
        {
            Series = series;
            ChartType = chartType;
        }
    }

    public class TabsConfig
    {
        public string[] TabNames { get; set; } = Array.Empty<string>();
        public int SelectedIndex { get; set; }
        public Action<int> OnTabChange { get; set; }
        public Action Content { get; set; }
        public int MaxLines { get; set; } = 1;
        public TabPosition Position { get; set; } = TabPosition.Top;
        public TabSide Side { get; set; } = TabSide.Left;
        public float TabWidth { get; set; } = 120f;
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
        public bool[] DisabledTabs { get; set; } = Array.Empty<bool>();
        public IndicatorStyle IndicatorStyle { get; set; } = IndicatorStyle.Underline;
        public bool ShowIndicator { get; set; } = true;
        public bool[] ClosableTabs { get; set; }
        public Action<int> OnTabClose { get; set; }
        public bool EnableOverflowScroll { get; set; }
        public Texture2D[] TabIcons { get; set; }

        public TabsConfig() { }

        public TabsConfig(string[] tabNames, int selectedIndex)
        {
            TabNames = tabNames ?? Array.Empty<string>();
            SelectedIndex = selectedIndex;
            DisabledTabs = new bool[TabNames.Length];
        }
    }

    public class ProgressConfig
    {
        public float Value { get; set; }
        public float Width { get; set; } = -1f;
        public float Height { get; set; } = -1f;
        public float Size { get; set; } = 32f;
        public string Label { get; set; }
        public bool ShowPercentage { get; set; } = true;
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class CardConfig
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Action FooterContent { get; set; }
        public Action HeaderContent { get; set; }
        public Texture2D Image { get; set; }
        public Texture2D Avatar { get; set; }
        public string Subtitle { get; set; }
        public float Width { get; set; } = -1f;
        public float Height { get; set; } = -1f;
    }

    public class SeparatorConfig
    {
        public SeparatorOrientation Orientation { get; set; } = SeparatorOrientation.Horizontal;
        public bool Decorative { get; set; } = true;
        public float SpacingBefore { get; set; } = DesignTokens.Spacing.SM;
        public float SpacingAfter { get; set; } = DesignTokens.Spacing.SM;
        public string Text { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class ToggleConfig
    {
        public string Text { get; set; }
        public bool Value { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Action<bool> OnToggle { get; set; }
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class TextAreaConfig
    {
        private static int _counter;

        public string Id { get; set; } = "textarea_" + System.Threading.Interlocked.Increment(ref _counter);
        public string Text { get; set; }
        public string Placeholder { get; set; }
        public string Label { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public bool Disabled { get; set; }
        public float MinHeight { get; set; } = 60f;
        public float MaxHeight { get; set; } = 300f;
        public int MaxLength { get; set; } = -1;
        public bool ShowCharCount { get; set; } = true;
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class SelectConfig
    {
        public string[] Items { get; set; } = Array.Empty<string>();
        public int SelectedIndex { get; set; }
        public Action<int> OnChange { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class PopoverConfig
    {
        public Action Content { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Popover;
    }

    public enum DropdownMenuItemType
    {
        Item,
        Separator,
        Header,
    }

    public class DropdownMenuItem
    {
        public DropdownMenuItemType Type { get; set; }
        public GUIContent Content { get; set; }
        public Action OnClick { get; set; }
        public bool IsSelected { get; set; }
        public List<DropdownMenuItem> SubItems { get; set; } = new();

        public DropdownMenuItem(DropdownMenuItemType type, string text = null, Action onClick = null, bool isSelected = false, Texture2D icon = null)
        {
            Type = type;
            Content = new UnityHelpers.GUIContent(text, icon);
            OnClick = onClick;
            IsSelected = isSelected;
        }
    }

    public class DropdownMenuConfig
    {
        public List<DropdownMenuItem> Items { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Dropdown;

        public DropdownMenuConfig() { }

        public DropdownMenuConfig(List<DropdownMenuItem> items)
        {
            Items = items;
        }
    }

    public enum ChartType
    {
        Line,
        Bar,
        Area,
        Pie,
        Scatter,
    }

    [Serializable]
    public class ChartDataPoint
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public Color Color { get; set; }
        public Dictionary<string, object> Payload { get; set; } = new();

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

    public class ToastConfig
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Description { get; set; }
        public ToastVariant Variant { get; set; } = ToastVariant.Default;
        public float DurationMs { get; set; } = 5000f;
        public Action OnAction { get; set; }
        public string ActionLabel { get; set; }
        public bool Dismissible { get; set; } = true;
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
        public ToastPosition Position { get; set; } = ToastPosition.BottomRight;
        public ToastStackDirection StackDirection { get; set; } = ToastStackDirection.Up;
        public float Margin { get; set; } = DesignTokens.Spacing.LG;
        public float Spacing { get; set; } = DesignTokens.Spacing.MD;
        public float Width { get; set; } = 360f;
        public float MinHeight { get; set; } = 90f;
        public float BorderRadius { get; set; } = DesignTokens.Radius.LG;
        public float Padding { get; set; } = DesignTokens.Spacing.LG;
        public bool ShowProgressBar { get; set; } = true;
        public bool ShowAccentBar { get; set; } = true;
        public float MaxWidth { get; set; } = 500f;
        public float MinWidth { get; set; } = 280f;
        public bool EnablePauseOnHover { get; set; } = true;
        public float HoverPauseDelay { get; set; } = DesignTokens.Animation.DurationNormal;
        public bool EnableClickToDismiss { get; set; }
        public bool UseSystemNotificationStyle { get; set; }
        public int ZIndex { get; set; } = DesignTokens.ZIndex.Toast;
    }

    public class SliderConfig
    {
        public float Value { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; } = 1f;
        public float Step { get; set; }
        public string Label { get; set; }
        public bool ShowValue { get; set; } = true;
        public string ValueFormat { get; set; } = "F2";
        public bool Disabled { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Action<float> OnChange { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();

        public SliderConfig() { }

        public SliderConfig(float value, float min = 0f, float max = 1f)
        {
            Value = value;
            MinValue = min;
            MaxValue = max;
        }
    }

    public class ThemeChangerConfig
    {
        public string Id { get; set; } = "theme_changer";
        public float Width { get; set; } = 200f;
        public float DropdownHeight { get; set; } = 250f;
        public bool ShowPreview { get; set; } = true;
        public Action<Theme> OnThemeChanged { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class TooltipConfig
    {
        public float HoverDelaySeconds { get; set; } = 0.4f;
        public float FadeDurationSeconds { get; set; } = 0.15f;
        public float MaxWidth { get; set; } = 280f;
        public float ShadowOffset { get; set; } = 4f;
        public float MouseOffset { get; set; } = 12f;
    }

    public class LayerConfig
    {
        public string Id { get; set; }
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

    public class NavigationItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public Action OnClick { get; set; }

        public NavigationItem() { }

        public NavigationItem(string id, string label, string icon = null)
        {
            Id = id;
            Label = label;
            Icon = icon;
        }
    }

    public class NavigationConfig
    {
        public NavigationItem[] Items { get; set; } = Array.Empty<NavigationItem>();
        public int SelectedIndex { get; set; }
        public float Width { get; set; } = 70f;
        public bool ShowIndicator { get; set; } = true;
        public IndicatorStyle IndicatorStyle { get; set; } = IndicatorStyle.Border;
        public Color IndicatorColor { get; set; } = new(0.2f, 0.7f, 1f, 1f);
        public string LogoText { get; set; } = "U";
        public Action<int> OnSelectionChanged { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class CalendarConfig
    {
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public DateTime? SelectedDate { get; set; }
        public Action<DateTime> OnDateSelected { get; set; }
        public List<DateTime> DisabledDates { get; set; } = new();
        public List<(DateTime Start, DateTime End)> Ranges { get; set; } = new();
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class DatePickerConfig
    {
        private static int _counter;

        public string Id { get; set; } = "datepicker_" + System.Threading.Interlocked.Increment(ref _counter);
        public string Label { get; set; }
        public string Placeholder { get; set; } = "Select date";
        public DateTime? SelectedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public GUILayoutOption[] Options { get; set; } = Array.Empty<GUILayoutOption>();
    }

    public class DataTableColumn
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string AccessorKey { get; set; }
        public float Width { get; set; } = 120f;
        public bool Sortable { get; set; } = true;
        public bool Filterable { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public TextAnchor Alignment { get; set; } = TextAnchor.MiddleLeft;
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
        public bool SelectAll { get; set; }
    }
}
