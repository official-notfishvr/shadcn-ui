using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public enum ControlVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline,
        Ghost,
        Link,
        Muted,
    }

    public enum ControlSize
    {
        Default,
        Small,
        Large,
        Icon,
        Mini,
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical,
    }

    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded,
    }

    public enum StyleComponentType
    {
        Button,
        Toggle,
        Input,
        PasswordField,
        TextArea,
        Label,
        ProgressBar,
        Separator,
        TabsList,
        TabsTrigger,
        TabsContent,
        Checkbox,
        CheckboxSolid,
        Switch,
        Badge,
        Avatar,
        Table,
        TableRow,
        TableHeader,
        TableCell,
        Dialog,
        Chart,
        ChartAxis,
        MenuBar,
        MenuBarItem,
        MenuDropdown,
        SelectContent,
        SelectItem,
        DropdownMenu,
        DropdownMenuItem,
        Popover,
        AnimatedBox,
        SectionHeader,
        Card,
        CardHeader,
        CardTitle,
        CardDescription,
        CardContent,
        CardFooter,
        Tooltip,
        Navigation,
        Calendar,
        CalendarWeekday,
        CalendarDay,
        CalendarDaySelected,
        CalendarDayInRange,
        CalendarDayToday,
        CalendarDayOutsideMonth,
        DatePicker,
        DatePickerWeekday,
        DatePickerDay,
        DatePickerDaySelected,
        DatePickerDayToday,
        DatePickerDayOutsideMonth,
    }

    public readonly struct StyleKey : IEquatable<StyleKey>
    {
        public readonly StyleComponentType Type;
        public readonly ControlVariant Variant;
        public readonly ControlSize Size;
        public readonly int State;
        public readonly string StyleId;

        public StyleKey(StyleComponentType type, ControlVariant variant, ControlSize size, int state, string styleId = null)
        {
            Type = type;
            Variant = variant;
            Size = size;
            State = state;
            StyleId = styleId;
        }

        public bool Equals(StyleKey other)
        {
            return Type == other.Type && Variant == other.Variant && Size == other.Size && State == other.State && string.Equals(StyleId, other.StyleId, StringComparison.Ordinal);
        }

        public override bool Equals(object obj) => obj is StyleKey other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)Type;
                hash = (hash * 397) ^ (int)Variant;
                hash = (hash * 397) ^ (int)Size;
                hash = (hash * 397) ^ State;
                hash = (hash * 397) ^ (StyleId != null ? StringComparer.Ordinal.GetHashCode(StyleId) : 0);
                return hash;
            }
        }
    }

    public partial class StyleManager
    {
        private readonly GUIHelper _guiHelper;
        private readonly Dictionary<StyleKey, GUIStyle> _styleCache = new();
        private Theme _theme;
        private bool _initialized;
        private bool _dirty = true;
        private float _lastScale = -1f;
        private string _lastThemeName;

        public Font CustomFont { get; set; }
        public StyleRegistry Registry { get; }
        public TextureManager Textures { get; }

        private GUIStyle _baseButtonStyle;
        private GUIStyle _baseToggleStyle;
        private GUIStyle _baseInputStyle;
        private GUIStyle _baseLabelStyle;
        private GUIStyle _baseBadgeStyle;
        private GUIStyle _baseTableStyle;
        private GUIStyle _checkboxStyle;
        private GUIStyle _checkboxSolidStyle;
        private GUIStyle _baseSwitchStyle;
        private GUIStyle _progressBarStyle;
        private GUIStyle _separatorStyle;
        private GUIStyle _tabsListStyle;
        private GUIStyle _tabsTriggerStyle;
        private GUIStyle _tableHeaderStyle;
        private GUIStyle _tableRowStyle;
        private GUIStyle _tableCellStyle;
        private GUIStyle _dialogContentStyle;
        private GUIStyle _cardStyle;
        private GUIStyle _dropdownContentStyle;
        private GUIStyle _dropdownItemStyle;
        private GUIStyle _menuBarStyle;
        private GUIStyle _chartContainerStyle;
        private GUIStyle _avatarStyle;
        private GUIStyle _navigationStyle;
        private GUIStyle _calendarStyle;
        private GUIStyle _calendarWeekdayStyle;
        private GUIStyle _calendarDayStyle;
        private GUIStyle _calendarDaySelectedStyle;
        private GUIStyle _calendarDayInRangeStyle;
        private GUIStyle _calendarDayTodayStyle;
        private GUIStyle _calendarDayOutsideMonthStyle;
        private GUIStyle _datePickerStyle;
        private GUIStyle _datePickerWeekdayStyle;
        private GUIStyle _datePickerDayStyle;
        private GUIStyle _datePickerDaySelectedStyle;
        private GUIStyle _datePickerDayTodayStyle;
        private GUIStyle _datePickerDayOutsideMonthStyle;
        internal GUIStyle AnimatedBoxStyle;

        public StyleManager(GUIHelper helper)
        {
            _guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
            Registry = new StyleRegistry();
            Textures = new TextureManager(helper);
            _theme = ThemeManager.Instance.CurrentTheme;
            _lastThemeName = _theme?.Name;
            ThemeManager.Instance.OnThemeChanged += OnThemeChanged;
        }

        public Theme GetTheme() => _theme ??= ThemeManager.Instance.CurrentTheme;

        public void InitializeGUI()
        {
            if (_initialized && !_dirty)
                return;

            _theme = ThemeManager.Instance.CurrentTheme;
            _lastThemeName = _theme?.Name;
            _lastScale = _guiHelper.uiScale;
            _styleCache.Clear();
            Textures.CreateAllTextures();
            CreateBaseStyles();
            _initialized = true;
            _dirty = false;
        }

        public int GetScaledBorderRadius(float radius) => Mathf.Max(0, Mathf.RoundToInt(radius * _guiHelper.uiScale));

        public int GetScaledSpacing(float spacing) => Mathf.Max(0, Mathf.RoundToInt(spacing * _guiHelper.uiScale));

        public int GetScaledHeight(float height) => Mathf.Max(0, Mathf.RoundToInt(height * _guiHelper.uiScale));

        public int GetScaledFontSize(float scale = 1f)
        {
            var baseSize = _guiHelper.fontSize > 0 ? _guiHelper.fontSize : 14;
            return Mathf.Max(8, Mathf.RoundToInt(baseSize * scale * _guiHelper.uiScale));
        }

        public RectOffset GetSpacingOffset(float horizontal = DesignTokens.Spacing.SM, float vertical = DesignTokens.Spacing.SM)
        {
            var h = GetScaledSpacing(horizontal);
            var v = GetScaledSpacing(vertical);
            return new UnityHelpers.RectOffset(h, h, v, v);
        }

        public Texture2D CreateTexture(int width, int height, int radius, Color color) => Textures.GenerateShape(width, height, radius, color, color, Color.clear, 0f, 0f, 0);

        public Texture2D CreateTexture(int width, int height, int radius, Color color, float shadowIntensity, int shadowBlur, Color shadowColor = default) => Textures.GenerateShape(width, height, radius, color, color, Color.clear, 0f, shadowIntensity, shadowBlur, shadowColor);

        public Texture2D CreateBorderTexture(int width, int height, int radius, Color fillColor, Color borderColor, float borderThickness = 1f, float shadowIntensity = 0f, int shadowBlur = 0, Color shadowColor = default) =>
            Textures.GenerateShape(width, height, radius, fillColor, fillColor, borderColor, borderThickness, shadowIntensity, shadowBlur, shadowColor);

        public Texture2D CreateAvatarTexture(int size, int radius, Color backgroundColor, Color borderColor, float borderThickness, bool withShadow = true) => Textures.GenerateAvatarTexture(size, radius, backgroundColor, borderColor, borderThickness, withShadow);

        public void Cleanup()
        {
            ThemeManager.Instance.OnThemeChanged -= OnThemeChanged;
            _styleCache.Clear();
            Textures.Cleanup();
        }

        public void MarkStylesCorruption() => _dirty = true;

        public void RegisterStyle(StyleComponentType type, string styleId, ComponentAppearance profile)
        {
            Registry.RegisterStyle(type, styleId, profile);
            MarkStylesCorruption();
        }

        public void RegisterStyle(StyleComponentType type, string styleId, StatefulStyleModifier modifier)
        {
            Registry.RegisterStyle(type, styleId, modifier);
            MarkStylesCorruption();
        }

        public bool UnregisterStyle(StyleComponentType type, string styleId)
        {
            var removed = Registry.UnregisterStyle(type, styleId);
            if (removed)
                MarkStylesCorruption();
            return removed;
        }

        public void RefreshStylesIfCorruption()
        {
            if (_dirty)
                InitializeGUI();
        }

        public bool ScanForCorruption()
        {
            var currentThemeName = ThemeManager.Instance.CurrentTheme?.Name;
            if (_lastThemeName != currentThemeName || Mathf.Abs(_lastScale - _guiHelper.uiScale) > 0.001f)
            {
                _dirty = true;
                return true;
            }

            return false;
        }

        private void OnThemeChanged()
        {
            _dirty = true;
        }

        private GUIStyle CloneStyle(GUIStyle source)
        {
            var clone = source != null ? new UnityHelpers.GUIStyle(source) : new UnityHelpers.GUIStyle();
            if (CustomFont != null)
                clone.font = CustomFont;
            return clone;
        }
    }
}
