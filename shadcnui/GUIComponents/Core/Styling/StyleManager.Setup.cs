using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public partial class StyleManager
    {
        private void CreateBaseStyles()
        {
            var theme = GetTheme();

            _baseButtonStyle = MakeControlStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Radius.MD, Color.clear, FontStyle.Bold);
            _baseToggleStyle = MakeControlStyle(theme.Secondary, theme.Text, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Radius.MD, Color.clear, FontStyle.Bold);
            _baseInputStyle = MakeInputStyle(theme.Base, theme.Text, Color.clear);
            _baseLabelStyle = MakeLabelStyle(theme.Text);
            _baseBadgeStyle = MakeChipStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg);
            _baseTableStyle = MakePanelStyle(theme.Base, Color.clear, DesignTokens.Radius.MD, DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
            _checkboxStyle = MakeControlStyle(theme.Base, theme.Text, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.SM, Color.clear, FontStyle.Normal);
            _checkboxSolidStyle = MakeControlStyle(theme.Accent, theme.Base, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.SM, Color.clear, FontStyle.Bold);
            _baseSwitchStyle = MakeControlStyle(theme.Base, theme.Text, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.Full, Color.clear, FontStyle.Normal);
            ApplyCheckedState(_baseToggleStyle, theme.Accent, theme.Base, theme.Accent);
            ApplyCheckedState(_checkboxStyle, theme.Accent, theme.Base, theme.Accent);
            ApplyCheckedState(_checkboxSolidStyle, theme.Accent, theme.Base, theme.Accent);
            ApplyCheckedState(_baseSwitchStyle, theme.Accent, theme.Base, theme.Accent);
            _progressBarStyle = MakePanelStyle(theme.Secondary, Color.clear, DesignTokens.Radius.Full, 0f, 0f);
            _separatorStyle = new UnityHelpers.GUIStyle(GUI.skin.box)
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
            _tableRowStyle = new UnityHelpers.GUIStyle(GUIStyle.none) { normal = { background = null } };
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
            _calendarStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, 12f, 12f);
            _calendarWeekdayStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _calendarDayStyle = MakeCalendarDayStyle(theme.Base, theme.Text, theme.Border);
            _calendarDaySelectedStyle = MakeCalendarDayStyle(theme.Accent, theme.Base, theme.Accent);
            _calendarDayInRangeStyle = MakeCalendarDayStyle(Tint(theme.Accent, 0.18f), theme.Text, theme.Accent);
            _calendarDayTodayStyle = MakeCalendarDayStyle(theme.Secondary, theme.Text, theme.Accent);
            _calendarDayOutsideMonthStyle = MakeCalendarDayStyle(Color.clear, theme.Muted, Color.clear);
            _datePickerStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, 12f, 12f);
            _datePickerWeekdayStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _datePickerDayStyle = MakeCalendarDayStyle(theme.Base, theme.Text, theme.Border);
            _datePickerDaySelectedStyle = MakeCalendarDayStyle(theme.Accent, theme.Base, theme.Accent);
            _datePickerDayTodayStyle = MakeCalendarDayStyle(theme.Secondary, theme.Text, theme.Accent);
            _datePickerDayOutsideMonthStyle = MakeCalendarDayStyle(Color.clear, theme.Muted, Color.clear);
            AnimatedBoxStyle = CloneStyle(GUIStyle.none);
            AnimatedBoxStyle.padding = GetSpacingOffset(16f, 16f);
            AnimatedBoxStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            AnimatedBoxStyle.normal.textColor = theme.Text;
        }

        private GUIStyle MakeControlStyle(Color background, Color text, float paddingH, float paddingV, float radius, Color border, FontStyle fontStyle)
        {
            var style = CloneStyle(GUI.skin.button);
            int r = GetScaledBorderRadius(radius);
            int height = GetScaledHeight(DesignTokens.Height.Default);
            bool hasBorder = border != Color.clear;
            style.normal.background = CreateBorderTexture(128, height, r, background, hasBorder ? border : background, hasBorder ? 1f : 0f);
            style.hover.background = CreateBorderTexture(128, height, r, Lift(background, 0.06f), hasBorder ? Lift(border, 0.06f) : Lift(background, 0.06f), hasBorder ? 1f : 0f);
            style.active.background = CreateBorderTexture(128, height, r, Lower(background, 0.08f), hasBorder ? Lower(border, 0.08f) : Lower(background, 0.08f), hasBorder ? 1f : 0f);
            style.focused.background = style.hover.background;
            style.normal.textColor = style.hover.textColor = style.active.textColor = style.focused.textColor = text;
            style.onNormal.background = style.normal.background;
            style.onHover.background = style.hover.background;
            style.onActive.background = style.active.background;
            style.onFocused.background = style.focused.background;
            style.onNormal.textColor = style.onHover.textColor = style.onActive.textColor = style.onFocused.textColor = text;
            style.padding = GetSpacingOffset(paddingH, paddingV);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
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
            int radius = GetScaledBorderRadius(DesignTokens.Radius.MD);
            int height = GetScaledHeight(DesignTokens.Height.Default);
            style.normal.background = CreateBorderTexture(128, height, radius, background, border, 1f);
            style.focused.background = CreateBorderTexture(128, height, radius, background, GetTheme().Accent, 1f);
            style.hover.background = style.normal.background;
            style.active.background = style.focused.background;
            style.normal.textColor = style.hover.textColor = style.focused.textColor = style.active.textColor = text;
            style.padding = GetSpacingOffset(DesignTokens.Padding.Input.Horizontal, DesignTokens.Padding.Input.Vertical);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
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
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            return style;
        }

        private GUIStyle MakePanelStyle(Color fill, Color border, float radius, float paddingH, float paddingV)
        {
            var style = CloneStyle(GUI.skin.box);
            int r = GetScaledBorderRadius(radius);
            bool hasBorder = border != Color.clear;
            style.normal.background = CreateBorderTexture(256, 256, r, fill, border, hasBorder ? 1f : 0f);
            style.padding = GetSpacingOffset(paddingH, paddingV);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
            style.normal.textColor = GetTheme().Text;
            return style;
        }

        private void ApplyCheckedState(GUIStyle style, Color fill, Color text, Color border)
        {
            var height = Mathf.Max(8, Mathf.RoundToInt(style.fixedHeight > 0 ? style.fixedHeight : GetScaledHeight(DesignTokens.Height.Default)));
            var radius = GetScaledBorderRadius(DesignTokens.Radius.MD);
            var hoverFill = Lift(fill, 0.04f);
            var activeFill = Lower(fill, 0.06f);
            bool hasBorder = border != Color.clear;

            style.onNormal.background = CreateBorderTexture(128, height, radius, fill, hasBorder ? border : fill, hasBorder ? 1f : 0f);
            style.onHover.background = CreateBorderTexture(128, height, radius, hoverFill, hasBorder ? Lift(border, 0.04f) : hoverFill, hasBorder ? 1f : 0f);
            style.onActive.background = CreateBorderTexture(128, height, radius, activeFill, hasBorder ? Lower(border, 0.06f) : activeFill, hasBorder ? 1f : 0f);
            style.onFocused.background = style.onHover.background;
            style.onNormal.textColor = style.onHover.textColor = style.onActive.textColor = style.onFocused.textColor = text;
            style.border = new UnityHelpers.RectOffset(radius, radius, radius, radius);
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
            style.padding = new UnityHelpers.RectOffset();
            style.margin = new UnityHelpers.RectOffset();
            style.border = new UnityHelpers.RectOffset(size / 2, size / 2, size / 2, size / 2);
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
            style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
            return style;
        }

        private Color Lift(Color color, float amount) => Color.Lerp(color, Color.white, amount);

        private Color Lower(Color color, float amount) => Color.Lerp(color, Color.black, amount);

        private Color Tint(Color color, float alpha) => new(color.r, color.g, color.b, alpha);
    }
}
