using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public partial class StyleManager
    {
        private GUIStyle GetCachedStyle(StyleComponentType type, ControlVariant variant, ControlSize size, GUIStyle baseStyle, int state = 0, System.Action<GUIStyle> customize = null)
        {
            var key = new StyleKey(type, variant, size, state);
            if (_styleCache.TryGetValue(key, out var cached))
                return cached;

            InitializeGUI();

            var style = CloneStyle(baseStyle);
            ApplySize(style, type, size);
            ApplyVariant(style, type, variant);
            customize?.Invoke(style);

            if (Registry.GetVariantModifier(type, variant) is { } variantModifier)
                variantModifier(style, GetTheme(), _guiHelper);

            if (Registry.GetSizeModifier(type, size) is { } sizeModifier)
                sizeModifier(style, GetTheme(), _guiHelper);

            _styleCache[key] = style;
            return style;
        }

        private void ApplySize(GUIStyle style, StyleComponentType type, ControlSize size)
        {
            switch (type)
            {
                case StyleComponentType.Button:
                case StyleComponentType.Toggle:
                case StyleComponentType.Input:
                case StyleComponentType.PasswordField:
                case StyleComponentType.Switch:
                    ApplyControlSize(style, size);
                    return;
                case StyleComponentType.Badge:
                    ApplyChipSize(style, size);
                    return;
                case StyleComponentType.Label:
                case StyleComponentType.SectionHeader:
                case StyleComponentType.ChartAxis:
                case StyleComponentType.CardTitle:
                case StyleComponentType.CardDescription:
                    ApplyFontOnlySize(style, size);
                    return;
            }

            if (size == ControlSize.Large)
                style.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG);
            else if (size == ControlSize.Small || size == ControlSize.Mini)
                style.padding = GetSpacingOffset(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM);
        }

        private void ApplyControlSize(GUIStyle style, ControlSize size)
        {
            switch (size)
            {
                case ControlSize.Mini:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Button.MiniH, DesignTokens.Padding.Button.MiniV);
                    style.fixedHeight = GetMinimumControlHeight(DesignTokens.Height.Mini, style.fontSize, DesignTokens.Padding.Button.MiniV);
                    break;
                case ControlSize.Small:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Button.SmallH, DesignTokens.Padding.Button.SmallV);
                    style.fixedHeight = GetMinimumControlHeight(DesignTokens.Height.Small, style.fontSize, DesignTokens.Padding.Button.SmallV);
                    break;
                case ControlSize.Large:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Button.LargeH, DesignTokens.Padding.Button.LargeV);
                    style.fixedHeight = GetMinimumControlHeight(DesignTokens.Height.Large, style.fontSize, DesignTokens.Padding.Button.LargeV);
                    break;
                case ControlSize.Icon:
                    var iconSize = GetScaledHeight(DesignTokens.Height.Default);
                    style.fixedWidth = iconSize;
                    style.fixedHeight = iconSize;
                    style.padding = new RectOffset();
                    break;
                default:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV);
                    style.fixedHeight = GetMinimumControlHeight(DesignTokens.Height.Default, style.fontSize, DesignTokens.Padding.Button.DefaultV);
                    break;
            }
        }

        private int GetMinimumControlHeight(float baseHeight, int fontSize, float verticalPadding)
        {
            int minHeight = GetScaledHeight(baseHeight);
            int contentHeight = fontSize + GetScaledSpacing(verticalPadding * 2f) + GetScaledSpacing(DesignTokens.Layout.ControlTextSlack);
            return Mathf.Max(minHeight, contentHeight);
        }

        private void ApplyChipSize(GUIStyle style, ControlSize size)
        {
            switch (size)
            {
                case ControlSize.Mini:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.SM, DesignTokens.Spacing.XXS);
                    break;
                case ControlSize.Small:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.SM, DesignTokens.Spacing.XS);
                    break;
                case ControlSize.Large:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.MD, DesignTokens.Spacing.SM);
                    break;
                default:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Badge.Horizontal, DesignTokens.Padding.Badge.Vertical);
                    break;
            }
        }

        private void ApplyFontOnlySize(GUIStyle style, ControlSize size)
        {
            style.fontSize = size switch
            {
                ControlSize.Mini => GetScaledFontSize(DesignTokens.FontScale.XS),
                ControlSize.Small => GetScaledFontSize(DesignTokens.FontScale.XS),
                ControlSize.Large => GetScaledFontSize(DesignTokens.FontScale.LG),
                _ => GetScaledFontSize(DesignTokens.FontScale.SM),
            };
        }

        private void ApplyVariant(GUIStyle style, StyleComponentType type, ControlVariant variant)
        {
            if (variant == ControlVariant.Default)
                return;

            var theme = GetTheme();

            if (type == StyleComponentType.Label || type == StyleComponentType.SectionHeader || type == StyleComponentType.ChartAxis || type == StyleComponentType.CardTitle || type == StyleComponentType.CardDescription)
            {
                style.normal.textColor = variant switch
                {
                    ControlVariant.Destructive => theme.Destructive,
                    ControlVariant.Link => theme.ButtonLinkColor,
                    ControlVariant.Muted => theme.Muted,
                    _ => style.normal.textColor,
                };
                return;
            }

            if (type == StyleComponentType.Input || type == StyleComponentType.PasswordField || type == StyleComponentType.TextArea)
            {
                var radius = GetScaledBorderRadius(DesignTokens.Radius.MD);
                switch (variant)
                {
                    case ControlVariant.Outline:
                        style.normal.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), radius, theme.Base, Color.clear, 0f);
                        break;
                    case ControlVariant.Ghost:
                        style.normal.background = CreateTexture(128, GetScaledHeight(DesignTokens.Height.Default), radius, Color.clear);
                        break;
                    case ControlVariant.Secondary:
                        style.normal.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), radius, theme.Secondary, Color.clear, 0f);
                        break;
                    case ControlVariant.Muted:
                        style.normal.textColor = theme.Muted;
                        break;
                }
                return;
            }

            ApplySurfaceVariant(style, variant);
        }

        private void ApplySurfaceVariant(GUIStyle style, ControlVariant variant)
        {
            var theme = GetTheme();
            var fill = theme.ButtonPrimaryBg;
            var text = theme.ButtonPrimaryFg;
            var border = Color.clear;

            switch (variant)
            {
                case ControlVariant.Secondary:
                    fill = theme.ButtonSecondaryBg;
                    text = theme.ButtonSecondaryFg;
                    break;
                case ControlVariant.Destructive:
                    fill = theme.ButtonDestructiveBg;
                    text = theme.ButtonDestructiveFg;
                    break;
                case ControlVariant.Outline:
                    fill = Color.clear;
                    text = theme.ButtonOutlineFg;
                    border = theme.Border;
                    break;
                case ControlVariant.Ghost:
                    fill = Color.clear;
                    text = theme.ButtonGhostFg;
                    border = Color.clear;
                    break;
                case ControlVariant.Link:
                    fill = Color.clear;
                    text = theme.ButtonLinkColor;
                    border = Color.clear;
                    break;
                case ControlVariant.Muted:
                    fill = theme.Muted;
                    text = theme.Base;
                    break;
            }

            var radius = style.border.left > 0 ? style.border.left : GetScaledBorderRadius(DesignTokens.Radius.SM);
            var height = Mathf.Max(8, Mathf.RoundToInt(style.fixedHeight > 0 ? style.fixedHeight : GetScaledHeight(DesignTokens.Height.Default)));
            style.normal.background = border == Color.clear ? CreateTexture(128, height, radius, fill) : CreateBorderTexture(128, height, radius, fill == Color.clear ? Color.clear : fill, border, 1f);
            style.hover.background = style.normal.background;
            style.active.background = style.normal.background;
            style.focused.background = style.normal.background;
            style.normal.textColor = style.hover.textColor = style.active.textColor = style.focused.textColor = text;
        }

        public GUIStyle GetButtonStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Button, variant, size, _baseButtonStyle);

        public GUIStyle GetToggleStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Toggle, variant, size, _baseToggleStyle);

        public GUIStyle GetLabelStyle(ControlVariant variant, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Label, variant, size, _baseLabelStyle);

        public GUIStyle GetProgressBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.ProgressBar, variant, size, _progressBarStyle);

        public GUIStyle GetBadgeStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Badge, variant, size, _baseBadgeStyle);

        public GUIStyle GetCardStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Card, variant, size, _cardStyle);

        public GUIStyle GetDialogContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Dialog, variant, size, _dialogContentStyle);

        public GUIStyle GetChartStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Chart, variant, size, _chartContainerStyle);

        public GUIStyle GetAnimatedBoxStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Large) => GetCachedStyle(StyleComponentType.AnimatedBox, variant, size, AnimatedBoxStyle);

        public GUIStyle GetMenuBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.MenuBar, variant, size, _menuBarStyle);

        public GUIStyle GetTabsListStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.TabsList, variant, size, _tabsListStyle);

        public GUIStyle GetSelectStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.SelectContent, variant, size, _dropdownContentStyle);

        public GUIStyle GetDropdownMenuStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.DropdownMenu, variant, size, _dropdownContentStyle);

        public GUIStyle GetInputStyle(ControlVariant variant, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            return GetCachedStyle(
                StyleComponentType.Input,
                variant,
                size,
                _baseInputStyle,
                (focused ? 1 : 0) | (disabled ? 2 : 0),
                style =>
                {
                    if (focused)
                        style.focused.background = CreateBorderTexture(128, Mathf.Max(28, Mathf.RoundToInt(style.fixedHeight > 0 ? style.fixedHeight : GetScaledHeight(DesignTokens.Height.Default))), GetScaledBorderRadius(DesignTokens.Radius.MD), GetTheme().Base, Color.clear, 0f);

                    if (disabled)
                    {
                        style.normal.textColor = GetTheme().Muted;
                        style.hover.textColor = GetTheme().Muted;
                        style.active.textColor = GetTheme().Muted;
                    }
                }
            );
        }

        public GUIStyle GetPasswordFieldStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            return GetCachedStyle(
                StyleComponentType.PasswordField,
                variant,
                size,
                _baseInputStyle,
                (focused ? 1 : 0) | (disabled ? 2 : 0),
                style =>
                {
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    if (focused)
                        style.focused.background = CreateBorderTexture(128, Mathf.Max(28, Mathf.RoundToInt(style.fixedHeight > 0 ? style.fixedHeight : GetScaledHeight(DesignTokens.Height.Default))), GetScaledBorderRadius(DesignTokens.Radius.MD), GetTheme().Base, Color.clear, 0f);
                    if (disabled)
                        style.normal.textColor = GetTheme().Muted;
                }
            );
        }

        public GUIStyle GetTextAreaStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false)
        {
            return GetCachedStyle(
                StyleComponentType.TextArea,
                variant,
                size,
                _baseInputStyle,
                focused ? 1 : 0,
                style =>
                {
                    style.wordWrap = true;
                    style.stretchHeight = true;
                    style.fixedHeight = 0f;
                    if (focused)
                        style.focused.background = CreateBorderTexture(128, 96, GetScaledBorderRadius(DesignTokens.Radius.MD), GetTheme().Base, Color.clear, 0f);
                }
            );
        }

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            return GetCachedStyle(
                StyleComponentType.Separator,
                variant,
                size,
                _separatorStyle,
                (int)orientation,
                style =>
                {
                    if (orientation == SeparatorOrientation.Horizontal)
                    {
                        style.fixedHeight = size == ControlSize.Large ? GetScaledHeight(DesignTokens.Separator.LargeThickness) : GetScaledHeight(DesignTokens.Separator.DefaultThickness);
                        style.fixedWidth = 0f;
                        style.stretchWidth = true;
                        style.stretchHeight = false;
                    }
                    else
                    {
                        style.fixedWidth = size == ControlSize.Large ? GetScaledHeight(DesignTokens.Separator.LargeThickness) : GetScaledHeight(DesignTokens.Separator.DefaultThickness);
                        style.fixedHeight = 0f;
                        style.stretchHeight = true;
                        style.stretchWidth = false;
                    }
                }
            );
        }

        public GUIStyle GetTabsTriggerStyle(bool active = false, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            return GetCachedStyle(
                StyleComponentType.TabsTrigger,
                variant,
                size,
                _tabsTriggerStyle,
                active ? 1 : 0,
                style =>
                {
                    style.alignment = TextAnchor.MiddleCenter;
                    if (active)
                    {
                        style.normal.background = Textures.TabsActive;
                        style.normal.textColor = GetTheme().TabsTriggerActiveFg;
                    }
                }
            );
        }

        public GUIStyle GetTabsContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.TabsContent, variant, size, GUIStyle.none, 0, style => style.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG));

        public GUIStyle GetCheckboxStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Checkbox, variant, size, _checkboxStyle);

        public GUIStyle GetCheckboxSolidStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.CheckboxSolid, variant, size, _checkboxSolidStyle);

        public GUIStyle GetSwitchStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Switch, variant, size, _baseSwitchStyle);

        public GUIStyle GetAvatarStyle(ControlSize size, AvatarShape shape, ControlVariant variant = ControlVariant.Default)
        {
            return GetCachedStyle(
                StyleComponentType.Avatar,
                variant,
                size,
                _avatarStyle,
                (int)shape,
                style =>
                {
                    var px = size switch
                    {
                        ControlSize.Mini => GetScaledHeight(DesignTokens.Height.Mini),
                        ControlSize.Small => GetScaledHeight(DesignTokens.Height.Small),
                        ControlSize.Large => GetScaledHeight(DesignTokens.Height.Large),
                        _ => GetScaledHeight(DesignTokens.Height.Default),
                    };

                    var radius = shape switch
                    {
                        AvatarShape.Circle => px / 2,
                        AvatarShape.Rounded => GetScaledBorderRadius(DesignTokens.Radius.MD),
                        _ => 0,
                    };

                    style.fixedWidth = px;
                    style.fixedHeight = px;
                    style.border = new UnityHelpers.RectOffset(radius, radius, radius, radius);
                    style.normal.background = CreateAvatarTexture(px, radius, GetTheme().Secondary, GetTheme().Border, DesignTokens.Avatar.BorderThickness, false);
                }
            );
        }

        public float GetStatusIndicatorSize(ControlSize size) =>
            size switch
            {
                ControlSize.Mini => DesignTokens.StatusIndicator.Mini * _guiHelper.uiScale,
                ControlSize.Small => DesignTokens.StatusIndicator.Small * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.StatusIndicator.Large * _guiHelper.uiScale,
                _ => DesignTokens.StatusIndicator.Default * _guiHelper.uiScale,
            };

        public GUIStyle GetTableStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Table, variant, size, _baseTableStyle);

        public GUIStyle GetTableRowStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.TableRow, variant, size, _tableRowStyle);

        public GUIStyle GetTableHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.TableHeader, variant, size, _tableHeaderStyle);

        public UnityHelpers.GUIStyle GetTableCellStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, TextAnchor alignment = TextAnchor.MiddleLeft) =>
            GetCachedStyle(StyleComponentType.TableCell, variant, size, _tableCellStyle, (int)alignment, style => style.alignment = alignment);

        public GUIStyle GetDropdownMenuItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.DropdownMenuItem, variant, size, _dropdownItemStyle);

        public GUIStyle GetSelectItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.SelectItem, variant, size, _dropdownItemStyle);

        public GUIStyle GetMenuBarItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool isShortcut = false, bool active = false)
        {
            return GetCachedStyle(
                StyleComponentType.MenuBarItem,
                variant,
                size,
                _dropdownItemStyle,
                (isShortcut ? 1 : 0) | (active ? 2 : 0),
                style =>
                {
                    style.alignment = isShortcut ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
                    style.normal.textColor = isShortcut ? Lift(GetTheme().Muted, 0.2f) : GetTheme().Text;
                    if (active)
                        style.normal.background = CreateTexture(128, 32, GetScaledBorderRadius(DesignTokens.Radius.SM), Tint(GetTheme().Accent, 0.14f));
                }
            );
        }

        public GUIStyle GetMenuDropdownStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.MenuDropdown, variant, size, _dropdownContentStyle);

        public GUIStyle GetNavigationStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Navigation, variant, size, _navigationStyle);

        public GUIStyle GetPopoverContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Popover, variant, size, _dropdownContentStyle);

        public GUIStyle GetChartAxisStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.ChartAxis,
                variant,
                size,
                _baseLabelStyle,
                0,
                style =>
                {
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = GetTheme().Muted;
                    style.fontSize = GetScaledFontSize(DesignTokens.Chart.AxisFontScale);
                }
            );

        public GUIStyle GetTooltipStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.Tooltip,
                variant,
                size,
                _dropdownContentStyle,
                0,
                style =>
                {
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.MD, DesignTokens.Spacing.SM);
                    style.wordWrap = true;
                }
            );

        public GUIStyle GetSectionHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.SectionHeader,
                variant,
                size,
                _baseLabelStyle,
                0,
                style =>
                {
                    style.fontStyle = FontStyle.Bold;
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.LG);
                    style.normal.textColor = GetTheme().Text;
                }
            );

        public GUIStyle GetCardHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.CardHeader, variant, size, GUIStyle.none, 0, style => style.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.MD));

        public GUIStyle GetCardContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.CardContent, variant, size, GUIStyle.none, 0, style => style.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.SM));

        public GUIStyle GetCardFooterStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.CardFooter, variant, size, GUIStyle.none, 0, style => style.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.MD));

        public GUIStyle GetCardTitleStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.CardTitle,
                variant,
                size,
                _baseLabelStyle,
                0,
                style =>
                {
                    style.fontStyle = FontStyle.Bold;
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XL);
                    style.normal.textColor = GetTheme().Text;
                }
            );

        public GUIStyle GetCardDescriptionStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.CardDescription,
                variant,
                size,
                _baseLabelStyle,
                0,
                style =>
                {
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    style.normal.textColor = GetTheme().Muted;
                }
            );

        public float GetSliderTrackHeight(ControlSize size) =>
            size switch
            {
                ControlSize.Mini => DesignTokens.Slider.TrackMini * _guiHelper.uiScale,
                ControlSize.Small => DesignTokens.Slider.TrackSmall * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.Slider.TrackLarge * _guiHelper.uiScale,
                _ => DesignTokens.Slider.TrackDefault * _guiHelper.uiScale,
            };

        public float GetSliderThumbSize(ControlSize size) =>
            size switch
            {
                ControlSize.Mini => DesignTokens.Slider.ThumbMini * _guiHelper.uiScale,
                ControlSize.Small => DesignTokens.Slider.ThumbSmall * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.Slider.ThumbLarge * _guiHelper.uiScale,
                _ => DesignTokens.Slider.ThumbDefault * _guiHelper.uiScale,
            };

        public Color GetSliderTrackColor(ControlVariant variant, bool disabled)
        {
            var color = GetTheme().Secondary;
            return disabled ? Color.Lerp(color, GetTheme().Muted, 0.5f) : color;
        }

        public Color GetSliderFillColor(ControlVariant variant, bool disabled)
        {
            var color = variant switch
            {
                ControlVariant.Destructive => GetTheme().Destructive,
                ControlVariant.Secondary => GetTheme().ButtonSecondaryBg,
                ControlVariant.Muted => GetTheme().Muted,
                _ => GetTheme().Accent,
            };

            return disabled ? Color.Lerp(color, GetTheme().Muted, 0.5f) : color;
        }

        public Color GetSliderThumbColor(ControlVariant variant, bool disabled)
        {
            var color = GetTheme().Text;
            return disabled ? Color.Lerp(color, GetTheme().Muted, 0.4f) : color;
        }

        public Color GetToastBackgroundColor(ToastVariant variant) =>
            variant switch
            {
                ToastVariant.Success => DesignTokens.ToastColors.SuccessBg,
                ToastVariant.Error => DesignTokens.ToastColors.ErrorBg,
                ToastVariant.Warning => DesignTokens.ToastColors.WarningBg,
                ToastVariant.Info => DesignTokens.ToastColors.InfoBg,
                _ => DesignTokens.ToastColors.DefaultBg,
            };

        public Color GetToastAccentColor(ToastVariant variant) =>
            variant switch
            {
                ToastVariant.Success => DesignTokens.ToastColors.SuccessAccent,
                ToastVariant.Error => DesignTokens.ToastColors.ErrorAccent,
                ToastVariant.Warning => DesignTokens.ToastColors.WarningAccent,
                ToastVariant.Info => DesignTokens.ToastColors.InfoAccent,
                _ => DesignTokens.ToastColors.DefaultAccent,
            };

        public Color GetToastTextColor(ToastVariant variant) => DesignTokens.ToastColors.Text;

        public GUIStyle GetCalendarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Calendar, variant, size, _calendarStyle);

        public GUIStyle GetCalendarWeekdayStyle() => GetCachedStyle(StyleComponentType.CalendarWeekday, ControlVariant.Default, ControlSize.Default, _calendarWeekdayStyle);

        public GUIStyle GetCalendarDayStyle() => GetCachedStyle(StyleComponentType.CalendarDay, ControlVariant.Default, ControlSize.Default, _calendarDayStyle);

        public GUIStyle GetCalendarDaySelectedStyle() => GetCachedStyle(StyleComponentType.CalendarDaySelected, ControlVariant.Default, ControlSize.Default, _calendarDaySelectedStyle);

        public GUIStyle GetCalendarDayInRangeStyle() => GetCachedStyle(StyleComponentType.CalendarDayInRange, ControlVariant.Default, ControlSize.Default, _calendarDayInRangeStyle);

        public GUIStyle GetCalendarDayTodayStyle() => GetCachedStyle(StyleComponentType.CalendarDayToday, ControlVariant.Default, ControlSize.Default, _calendarDayTodayStyle);

        public GUIStyle GetCalendarDayOutsideMonthStyle() => GetCachedStyle(StyleComponentType.CalendarDayOutsideMonth, ControlVariant.Default, ControlSize.Default, _calendarDayOutsideMonthStyle);

        public GUIStyle GetDatePickerStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.DatePicker, variant, size, _datePickerStyle);

        public GUIStyle GetDatePickerWeekdayStyle() => GetCachedStyle(StyleComponentType.DatePickerWeekday, ControlVariant.Default, ControlSize.Default, _datePickerWeekdayStyle);

        public GUIStyle GetDatePickerDayStyle() => GetCachedStyle(StyleComponentType.DatePickerDay, ControlVariant.Default, ControlSize.Default, _datePickerDayStyle);

        public GUIStyle GetDatePickerDaySelectedStyle() => GetCachedStyle(StyleComponentType.DatePickerDaySelected, ControlVariant.Default, ControlSize.Default, _datePickerDaySelectedStyle);

        public GUIStyle GetDatePickerDayTodayStyle() => GetCachedStyle(StyleComponentType.DatePickerDayToday, ControlVariant.Default, ControlSize.Default, _datePickerDayTodayStyle);

        public GUIStyle GetDatePickerDayOutsideMonthStyle() => GetCachedStyle(StyleComponentType.DatePickerDayOutsideMonth, ControlVariant.Default, ControlSize.Default, _datePickerDayOutsideMonthStyle);
    }
}
