using shadcnui.GUIComponents.Core.Theming;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public partial class StyleManager
    {
        private void CreateBaseStyles()
        {
            var theme = GetTheme();
            var metrics = theme.Metrics;

            _baseButtonStyle = MakeControlStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Radius.MD, theme.Border, theme.Typography.ButtonWeight);
            _baseToggleStyle = MakeControlStyle(theme.Secondary, theme.Text, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Radius.MD, theme.Border, FontStyle.Bold);
            _baseInputStyle = MakeInputStyle(theme.Base, theme.Text, theme.Border);
            _baseLabelStyle = MakeLabelStyle(theme.Text);
            _baseBadgeStyle = MakeChipStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg);
            _baseTableStyle = MakePanelStyle(theme.Base, theme.Border, DesignTokens.Radius.MD, DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
            _checkboxStyle = MakeControlStyle(theme.Base, theme.Text, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.SM, theme.Border, FontStyle.Normal);
            _checkboxSolidStyle = MakeControlStyle(theme.Accent, theme.Base, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.SM, theme.Accent, FontStyle.Bold);
            _baseSwitchStyle = MakeControlStyle(theme.Base, theme.Text, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.Full, theme.Border, FontStyle.Normal);
            ApplyCheckedState(_baseToggleStyle, theme.Accent, theme.Base, theme.Accent);
            ApplyCheckedState(_checkboxStyle, theme.Accent, theme.Base, theme.Accent);
            ApplyCheckedState(_checkboxSolidStyle, theme.Accent, theme.Base, theme.Accent);
            ApplyCheckedState(_baseSwitchStyle, theme.Accent, theme.Base, theme.Accent);
            _progressBarStyle = MakePanelStyle(theme.Secondary, Color.clear, DesignTokens.Radius.Full, 0f, 0f);
            _separatorStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = Textures.Separator },
                margin = new RectOffset(),
                padding = new RectOffset(),
                border = new RectOffset(),
            };
            _tabsListStyle = MakePanelStyle(theme.TabsBg, theme.Border, DesignTokens.Radius.MD, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS);
            _tabsTriggerStyle = MakeControlStyle(Color.clear, theme.TabsTriggerFg, DesignTokens.Padding.Tab.Horizontal, DesignTokens.Padding.Tab.Vertical, DesignTokens.Radius.SM, Color.clear, FontStyle.Bold);
            _tableHeaderStyle = MakeLabelStyle(theme.Text, FontStyle.Bold);
            _tableHeaderStyle.normal.background = Textures.TableHeader;
            _tableHeaderStyle.padding = GetSpacingOffset(DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
            _tableRowStyle = MakePanelStyle(theme.Base, theme.Border, 0f, 0f, 0f);
            _tableCellStyle = MakeLabelStyle(theme.Text);
            _tableCellStyle.padding = GetSpacingOffset(DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
            _dialogContentStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.LG, DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical);
            _cardStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical);
            _dropdownContentStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS);
            _dropdownItemStyle = MakeControlStyle(Color.clear, theme.Text, DesignTokens.Padding.Dropdown.ItemH, DesignTokens.Padding.Dropdown.ItemV, DesignTokens.Radius.SM, Color.clear, FontStyle.Normal);
            _menuBarStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.SM, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS);
            _chartContainerStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Chart.Radius, DesignTokens.Chart.ContainerPaddingH, DesignTokens.Chart.ContainerPaddingV);
            _avatarStyle = MakeAvatarStyle(theme);
            _navigationStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.SM, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM);
            _calendarStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, metrics.ContentPadding, metrics.ContentPadding);
            _calendarWeekdayStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _calendarDayStyle = MakeCalendarDayStyle(theme.Base, theme.Text, theme.Border);
            _calendarDaySelectedStyle = MakeCalendarDayStyle(theme.Accent, theme.Base, theme.Accent);
            _calendarDayInRangeStyle = MakeCalendarDayStyle(Tint(theme.Accent, 0.18f), theme.Text, theme.Accent);
            _calendarDayTodayStyle = MakeCalendarDayStyle(theme.Secondary, theme.Text, theme.Accent);
            _calendarDayOutsideMonthStyle = MakeCalendarDayStyle(Color.clear, theme.Muted, Color.clear);
            _datePickerStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, metrics.ContentPadding, metrics.ContentPadding);
            _datePickerWeekdayStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _datePickerDayStyle = MakeCalendarDayStyle(theme.Base, theme.Text, theme.Border);
            _datePickerDaySelectedStyle = MakeCalendarDayStyle(theme.Accent, theme.Base, theme.Accent);
            _datePickerDayTodayStyle = MakeCalendarDayStyle(theme.Secondary, theme.Text, theme.Accent);
            _datePickerDayOutsideMonthStyle = MakeCalendarDayStyle(Color.clear, theme.Muted, Color.clear);
            AnimatedBoxStyle = CloneStyle(GUIStyle.none);
            AnimatedBoxStyle.padding = GetSpacingOffset(metrics.PanelPadding, metrics.PanelPadding);
            AnimatedBoxStyle.margin = new RectOffset(0, 0, 0, 0);
            AnimatedBoxStyle.normal.textColor = theme.Text;
        }

        private GUIStyle MakeControlStyle(Color background, Color text, float paddingH, float paddingV, float radius, Color border, FontStyle fontStyle)
        {
            var style = CloneStyle(GUI.skin.button);
            style.normal.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), GetScaledBorderRadius(radius), background, border == Color.clear ? background : border, border == Color.clear ? 0f : 1f);
            style.hover.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), GetScaledBorderRadius(radius), Lift(background, 0.06f), border == Color.clear ? Lift(background, 0.06f) : border, border == Color.clear ? 0f : 1f);
            style.active.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), GetScaledBorderRadius(radius), Lower(background, 0.08f), border == Color.clear ? Lower(background, 0.08f) : border, border == Color.clear ? 0f : 1f);
            style.focused.background = style.hover.background;
            style.normal.textColor = style.hover.textColor = style.active.textColor = style.focused.textColor = text;
            style.onNormal.background = style.normal.background;
            style.onHover.background = style.hover.background;
            style.onActive.background = style.active.background;
            style.onFocused.background = style.focused.background;
            style.onNormal.textColor = style.onHover.textColor = style.onActive.textColor = style.onFocused.textColor = text;
            style.padding = GetSpacingOffset(paddingH, paddingV);
            style.margin = new RectOffset(0, 0, 0, 0);
            style.border = new RectOffset(0, 0, 0, 0);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
            style.fontStyle = fontStyle;
            style.stretchWidth = false;
            style.stretchHeight = false;
            return style;
        }

        private GUIStyle MakeInputStyle(Color background, Color text, Color border)
        {
            var style = CloneStyle(GUI.skin.textField);
            style.normal.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), GetScaledBorderRadius(DesignTokens.Radius.MD), background, border, 1f);
            style.focused.background = CreateBorderTexture(128, GetScaledHeight(DesignTokens.Height.Default), GetScaledBorderRadius(DesignTokens.Radius.MD), background, GetTheme().Accent, 1f);
            style.hover.background = style.normal.background;
            style.active.background = style.focused.background;
            style.normal.textColor = style.hover.textColor = style.focused.textColor = style.active.textColor = text;
            style.padding = GetSpacingOffset(DesignTokens.Padding.Input.Horizontal, DesignTokens.Padding.Input.Vertical);
            style.margin = new RectOffset(0, 0, 0, 0);
            style.border = new RectOffset(0, 0, 0, 0);
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
            return style;
        }

        private GUIStyle MakeLabelStyle(Color color, FontStyle fontStyle = FontStyle.Normal)
        {
            var style = CloneStyle(GUI.skin.label);
            style.normal.background = null;
            style.normal.textColor = color;
            style.hover.textColor = color;
            style.active.textColor = color;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
            style.fontStyle = fontStyle;
            style.wordWrap = true;
            style.margin = new RectOffset(0, 0, 0, 0);
            style.padding = new RectOffset(0, 0, 0, 0);
            return style;
        }

        private GUIStyle MakePanelStyle(Color fill, Color border, float radius, float paddingH, float paddingV)
        {
            var style = CloneStyle(GUI.skin.box);
            style.normal.background = CreateBorderTexture(256, 256, GetScaledBorderRadius(radius), fill, border, border == Color.clear ? 0f : 1f);
            style.padding = GetSpacingOffset(paddingH, paddingV);
            style.margin = new RectOffset(0, 0, 0, 0);
            style.border = new RectOffset(0, 0, 0, 0);
            style.normal.textColor = GetTheme().Text;
            return style;
        }

        private void ApplyCheckedState(GUIStyle style, Color fill, Color text, Color border)
        {
            var height = Mathf.Max(8, Mathf.RoundToInt(style.fixedHeight > 0 ? style.fixedHeight : GetScaledHeight(DesignTokens.Height.Default)));
            var radius = GetScaledBorderRadius(DesignTokens.Radius.MD);
            var hoverFill = Lift(fill, 0.04f);
            var activeFill = Lower(fill, 0.06f);

            style.onNormal.background = CreateBorderTexture(128, height, radius, fill, border, 1f);
            style.onHover.background = CreateBorderTexture(128, height, radius, hoverFill, border, 1f);
            style.onActive.background = CreateBorderTexture(128, height, radius, activeFill, border, 1f);
            style.onFocused.background = style.onHover.background;
            style.onNormal.textColor = style.onHover.textColor = style.onActive.textColor = style.onFocused.textColor = text;
        }

        private GUIStyle MakeChipStyle(Color fill, Color text)
        {
            var style = MakeControlStyle(fill, text, DesignTokens.Padding.Badge.Horizontal, DesignTokens.Padding.Badge.Vertical, DesignTokens.Radius.Full, Color.clear, FontStyle.Bold);
            style.fixedHeight = 0f;
            return style;
        }

        private GUIStyle MakeAvatarStyle(Theme theme)
        {
            var size = GetScaledHeight(DesignTokens.Height.Default);
            var style = CloneStyle(GUI.skin.box);
            style.normal.background = CreateAvatarTexture(size, size / 2, theme.Secondary, theme.Border, DesignTokens.Avatar.BorderThickness, false);
            style.normal.textColor = theme.Text;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = GetScaledFontSize(DesignTokens.Avatar.FallbackFontScale);
            style.padding = new RectOffset();
            style.margin = new RectOffset();
            style.border = new RectOffset(size / 2, size / 2, size / 2, size / 2);
            style.fixedWidth = size;
            style.fixedHeight = size;
            return style;
        }

        private GUIStyle MakeCalendarDayStyle(Color fill, Color text, Color border)
        {
            var style = CloneStyle(GUI.skin.button);
            style.normal.background = border == Color.clear ? CreateSolidTexture(fill == Color.clear ? Color.clear : fill) : CreateBorderTexture(36, 32, GetScaledBorderRadius(DesignTokens.Radius.SM), fill == Color.clear ? Color.clear : fill, border, 1f);
            style.hover.background = style.normal.background;
            style.active.background = style.normal.background;
            style.normal.textColor = style.hover.textColor = style.active.textColor = text;
            style.padding = new RectOffset(0, 0, 0, 0);
            style.margin = new RectOffset(0, 0, 0, 0);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
            return style;
        }

        private Color Lift(Color color, float amount) => Color.Lerp(color, Color.white, amount);

        private Color Lower(Color color, float amount) => Color.Lerp(color, Color.black, amount);

        private Color Tint(Color color, float alpha) => new(color.r, color.g, color.b, alpha);
    }
}
