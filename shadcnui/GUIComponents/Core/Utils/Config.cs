using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Utils
{
    #region Icon Configuration

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
        public IconPosition Position { get; set; }
        public float Size { get; set; }
        public float Spacing { get; set; }

        public IconConfig()
        {
            Position = IconPosition.Left;
            Size = 20f;
            Spacing = 6f;
        }

        public IconConfig(Texture2D image)
            : this()
        {
            Image = image;
        }

        public IconConfig(Texture2D image, IconPosition position)
            : this(image)
        {
            Position = position;
        }

        public bool HasIcon => Image != null;
    }

    #endregion

    public class ButtonConfig
    {
        public string Text { get; set; }
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }
        public Action OnClick { get; set; }
        public bool Disabled { get; set; }
        public float Opacity { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public ButtonConfig(string text)
        {
            Text = text;
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
            OnClick = null;
            Disabled = false;
            Opacity = 1f;
            Icon = null;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class InputConfig
    {
        public string Value { get; set; }
        public string Placeholder { get; set; }
        public ControlVariant Variant { get; set; }
        public bool Disabled { get; set; }
        public bool Focused { get; set; }
        public int Width { get; set; }
        public Action<string> OnChange { get; set; }
        public string Label { get; set; }
        public ControlVariant LabelVariant { get; set; }
        public char MaskChar { get; set; }
        public int MaxLength { get; set; }
        public float Height { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public InputConfig()
        {
            Variant = ControlVariant.Default;
            LabelVariant = ControlVariant.Default;
            Disabled = false;
            Focused = false;
            Width = -1;
            MaskChar = '*';
            MaxLength = 1000;
            Height = 60f;
            Icon = null;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class CheckboxConfig
    {
        public string Text { get; set; }
        public bool Value { get; set; }
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }
        public Action<bool> OnToggle { get; set; }
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public CheckboxConfig()
        {
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
            Disabled = false;
            Rect = null;
            Icon = null;
            Options = Array.Empty<GUILayoutOption>();
            Text = "Checkbox";
        }
    }

    public class SwitchConfig
    {
        public string Text { get; set; }
        public bool Value { get; set; }
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }
        public Action<bool> OnToggle { get; set; }
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public SwitchConfig()
        {
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
            Disabled = false;
            Rect = null;
            Icon = null;
            Options = Array.Empty<GUILayoutOption>();
            Text = "Switch";
        }
    }

    public class LabelConfig
    {
        public string Text { get; set; }
        public ControlVariant Variant { get; set; }
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }
        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public LabelConfig()
        {
            Variant = ControlVariant.Default;
            Disabled = false;
            Rect = null;
            Icon = null;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class TableConfig
    {
        public string[] Headers { get; set; }
        public string[,] Data { get; set; }
        public object[,] ObjectData { get; set; }
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }
        public Rect? Rect { get; set; }

        public int[] SortColumns { get; set; }
        public bool[] SortAscending { get; set; }
        public Action<int, bool> OnSort { get; set; }

        public bool[] SelectedRows { get; set; }
        public Action<int, bool> OnSelectionChange { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public Action<int> OnPageChange { get; set; }

        public string SearchQuery { get; set; }
        public string[,] FilteredData { get; set; }
        public Action<string> OnSearch { get; set; }

        public float[] ColumnWidths { get; set; }

        public Action<object, int, int> CellRenderer { get; set; }

        public GUILayoutOption[] Options { get; set; }

        public TableConfig()
        {
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
            Rect = null;
            Options = Array.Empty<GUILayoutOption>();
            PageSize = 10;
        }
    }

    public class DialogConfig
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Action Content { get; set; }
        public Action Footer { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public bool CloseOnOverlayClick { get; set; }
        public int ZIndex { get; set; }

        public DialogConfig()
        {
            Width = 400;
            Height = 300;
            CloseOnOverlayClick = false;
            ZIndex = DesignTokens.ZIndex.Modal;
        }
    }

    public class CalendarConfig
    {
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }

        public CalendarConfig()
        {
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
        }
    }

    public class DatePickerConfig
    {
        public string Placeholder { get; set; }
        public DateTime? SelectedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public DatePickerConfig()
        {
            Id = "datepicker";
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class AvatarConfig
    {
        public Texture2D Image { get; set; }
        public string FallbackText { get; set; }
        public ControlSize Size { get; set; }
        public AvatarShape Shape { get; set; }
        public bool IsOnline { get; set; }
        public string Name { get; set; }
        public bool ShowNameBelow { get; set; }
        public Color BorderColor { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public AvatarConfig()
        {
            Size = ControlSize.Default;
            Shape = AvatarShape.Circle;
            IsOnline = false;
            ShowNameBelow = false;
            BorderColor = Color.clear;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class BadgeConfig
    {
        public string Text { get; set; }
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }
        public IconConfig Icon { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public bool IsActive { get; set; }
        public bool ShowStatusDot { get; set; }
        public float Progress { get; set; }
        public float CornerRadius { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public BadgeConfig()
        {
            Text = "Badge";
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
            Icon = null;
            Count = 0;
            MaxCount = 99;
            CornerRadius = 12f;
            ShowStatusDot = false;
            IsActive = false;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class ChartConfig
    {
        public List<ChartSeries> Series { get; set; }
        public ChartType ChartType { get; set; }
        public Vector2 Size { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public ChartConfig(List<ChartSeries> series, ChartType chartType)
        {
            Series = series;
            ChartType = chartType;
            Size = new Vector2(400, 300);
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class TabsConfig
    {
        public string[] TabNames { get; set; }
        public int SelectedIndex { get; set; }
        public Action<int> OnTabChange { get; set; }
        public Action Content { get; set; }
        public int MaxLines { get; set; }
        public Tabs.TabPosition Position { get; set; }
        public Tabs.TabSide Side { get; set; }
        public float TabWidth { get; set; }
        public GUILayoutOption[] Options { get; set; }
        public bool[] DisabledTabs { get; set; }
        public Tabs.IndicatorStyle IndicatorStyle { get; set; }
        public bool ShowIndicator { get; set; }
        public bool[] ClosableTabs { get; set; }
        public Action<int> OnTabClose { get; set; }
        public bool EnableOverflowScroll { get; set; }
        public Texture2D[] TabIcons { get; set; }

        public TabsConfig(string[] tabNames, int selectedIndex)
        {
            TabNames = tabNames;
            SelectedIndex = selectedIndex;
            OnTabChange = null;
            Content = null;
            MaxLines = 1;
            Position = Tabs.TabPosition.Top;
            Side = Tabs.TabSide.Left;
            TabWidth = 120f;
            Options = Array.Empty<GUILayoutOption>();
            DisabledTabs = new bool[tabNames?.Length ?? 0];
            IndicatorStyle = Tabs.IndicatorStyle.Underline;
            ShowIndicator = true;
            ClosableTabs = null;
            OnTabClose = null;
            EnableOverflowScroll = true;
            TabIcons = null;
        }
    }

    public class ProgressConfig
    {
        public float Value { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Size { get; set; }
        public string Label { get; set; }
        public bool ShowPercentage { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public ProgressConfig()
        {
            Width = -1;
            Height = -1;
            Size = 32f;
            ShowPercentage = true;
            Options = Array.Empty<GUILayoutOption>();
        }
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
        public float Width { get; set; }
        public float Height { get; set; }

        public CardConfig()
        {
            Width = -1;
            Height = -1;
        }
    }

    public class SeparatorConfig
    {
        public SeparatorOrientation Orientation { get; set; }
        public bool Decorative { get; set; }
        public float SpacingBefore { get; set; }
        public float SpacingAfter { get; set; }
        public string Text { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public SeparatorConfig()
        {
            Orientation = SeparatorOrientation.Horizontal;
            Decorative = true;
            SpacingBefore = 8f;
            SpacingAfter = 8f;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class ToggleConfig
    {
        public string Text { get; set; }
        public bool Value { get; set; }
        public ControlVariant Variant { get; set; }
        public ControlSize Size { get; set; }
        public Action<bool> OnToggle { get; set; }
        public bool Disabled { get; set; }
        public Rect? Rect { get; set; }

        public IconConfig Icon { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public ToggleConfig()
        {
            Variant = ControlVariant.Default;
            Size = ControlSize.Default;
            Disabled = false;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class TextAreaConfig
    {
        public string Text { get; set; }
        public string Placeholder { get; set; }
        public string Label { get; set; }
        public ControlVariant Variant { get; set; }
        public bool Disabled { get; set; }
        public float MinHeight { get; set; }
        public float MaxHeight { get; set; }
        public int MaxLength { get; set; }
        public bool ShowCharCount { get; set; }
        public Rect? Rect { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public TextAreaConfig()
        {
            Variant = ControlVariant.Default;
            MinHeight = 60f;
            MaxHeight = 300f;
            MaxLength = -1;
            ShowCharCount = true;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class SelectConfig
    {
        public string[] Items { get; set; }
        public int SelectedIndex { get; set; }
        public Action<int> OnChange { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public SelectConfig()
        {
            Items = Array.Empty<string>();
            SelectedIndex = 0;
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class PopoverConfig
    {
        public Action Content { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public PopoverConfig()
        {
            Options = Array.Empty<GUILayoutOption>();
        }
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
        public List<DropdownMenuItem> SubItems { get; set; }

        public DropdownMenuItem(DropdownMenuItemType type, string text = null, Action onClick = null, bool isSelected = false, Texture2D icon = null)
        {
            Type = type;
            Content = new UnityHelpers.GUIContent(text, icon);
            OnClick = onClick;
            IsSelected = isSelected;
            SubItems = new List<DropdownMenuItem>();
        }
    }

    public class DropdownMenuConfig
    {
        public List<DropdownMenuItem> Items { get; set; }
        public GUILayoutOption[] Options { get; set; }

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
        public Dictionary<string, object> Payload { get; set; }

        public ChartDataPoint(string name, float value, Color color = default)
        {
            Name = name;
            Value = value;
            Color = color == default ? Color.blue : color;
            Payload = new Dictionary<string, object>();
        }
    }

    [Serializable]
    public class ChartSeries
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public Color Color { get; set; }
        public List<ChartDataPoint> Data { get; set; }
        public bool Visible { get; set; }

        public ChartSeries(string key, string label, Color color = default)
        {
            Key = key;
            Label = label;
            Color = color == default ? Color.blue : color;
            Data = new List<ChartDataPoint>();
            Visible = true;
        }
    }

    public class DataTableColumn
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string AccessorKey { get; set; }
        public bool Sortable { get; set; } = true;
        public bool Filterable { get; set; } = true;
        public float Width { get; set; } = 120f;
        public float MinWidth { get; set; } = 80f;
        public bool CanHide { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public Func<object, string> CellRenderer { get; set; }
        public TextAnchor Alignment { get; set; } = TextAnchor.MiddleLeft;

        public DataTableColumn(string id, string header, string accessorKey = null)
        {
            Id = id;
            Header = header;
            AccessorKey = accessorKey ?? id;
        }
    }

    public class DataTableRow
    {
        public string Id { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public bool Selected { get; set; } = false;

        public DataTableRow(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            if (Data.TryGetValue(key, out object value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public void SetValue(string key, object value)
        {
            Data[key] = value;
        }

        public DataTableRow SetData(string key, object value)
        {
            Data[key] = value;
            return this;
        }
    }

    public class DataTableState
    {
        public string SortColumn { get; set; }
        public bool SortAscending { get; set; } = true;
        public string FilterText { get; set; } = "";
        public int CurrentPage { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public List<string> SelectedRows { get; set; } = new List<string>();
        public bool SelectAll { get; set; } = false;
        public Dictionary<string, bool> ColumnVisibility { get; set; } = new Dictionary<string, bool>();
        public bool ShowColumnToggle { get; set; } = false;
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
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ToastVariant Variant { get; set; } = ToastVariant.Default;
        public float DurationMs { get; set; } = 5000f;
        public Action OnAction { get; set; }
        public string ActionLabel { get; set; }
        public bool Dismissible { get; set; } = true;
        public GUILayoutOption[] Options { get; set; }
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
        public bool EnableClickToDismiss { get; set; } = false;
        public bool UseSystemNotificationStyle { get; set; } = false;

        public ToastConfig()
        {
            Id = Guid.NewGuid().ToString();
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class SliderConfig
    {
        public float Value { get; set; }
        public float MinValue { get; set; } = 0f;
        public float MaxValue { get; set; } = 1f;
        public float Step { get; set; } = 0f;
        public string Label { get; set; }
        public bool ShowValue { get; set; } = true;
        public string ValueFormat { get; set; } = "F2";
        public bool Disabled { get; set; }
        public ControlVariant Variant { get; set; } = ControlVariant.Default;
        public ControlSize Size { get; set; } = ControlSize.Default;
        public Action<float> OnChange { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public SliderConfig()
        {
            Options = Array.Empty<GUILayoutOption>();
        }

        public SliderConfig(float value, float min = 0f, float max = 1f)
        {
            Value = value;
            MinValue = min;
            MaxValue = max;
            Options = Array.Empty<GUILayoutOption>();
        }
    }
}
