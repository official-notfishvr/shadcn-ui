using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    public partial class StyleManager
    {
        #region Variant Application

        private void ApplyContainerVariant(GUIStyle style, ControlVariant variant, Theme theme)
        {
            switch (variant)
            {
                case ControlVariant.Secondary:
                    style.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                    break;
                case ControlVariant.Destructive:
                    style.normal.background = CreateSolidTexture(theme.Destructive);
                    style.normal.textColor = theme.ButtonDestructiveFg;
                    break;
                case ControlVariant.Outline:
                    style.normal.background = transparentTexture;
                    style.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = transparentTexture;
                    break;
                case ControlVariant.Muted:
                    style.normal.background = CreateSolidTexture(theme.Muted);
                    break;
            }
        }

        private void ApplyVariantToStyle(GUIStyle style, StyleComponentType type, ControlVariant variant, Theme theme)
        {
            var modifier = Registry.GetVariantModifier(type, variant);
            if (modifier != null)
            {
                modifier(style, theme, guiHelper);
            }
        }

        #endregion

        #region Style Sizing

        private void ApplySizing(GUIStyle style, StyleComponentType type, ControlSize size)
        {
            var modifier = Registry.GetSizeModifier(type, size);
            if (modifier != null)
            {
                modifier(style, ThemeManager.Instance.CurrentTheme, guiHelper);
            }
        }

        #endregion

        #region Helper Methods

        private GUIStyle GetCachedStyle(StyleComponentType componentType, ControlVariant variant, ControlSize size, GUIStyle baseStyle, int state = 0, bool applyVariant = true, bool applySizing = true, Action<GUIStyle> customLogic = null)
        {
            var key = new StyleKey(componentType, variant, size, state);
            if (styleCache.TryGetValue(key, out var cachedStyle))
                return cachedStyle;

            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle);

            if (applyVariant)
                ApplyVariantToStyle(style, componentType, variant, ThemeManager.Instance.CurrentTheme);

            if (applySizing)
                ApplySizing(style, componentType, size);

            customLogic?.Invoke(style);

            styleCache[key] = style;
            return style;
        }

        #endregion

        #region Style Getters

        public GUIStyle GetButtonStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Button, variant, size, baseButtonStyle ?? GUI.skin.button);

        public GUIStyle GetToggleStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Toggle, variant, size, baseToggleStyle ?? GUI.skin.button);

        public GUIStyle GetInputStyle(ControlVariant variant, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            int state = (focused ? 1 : 0) | (disabled ? 2 : 0);
            return GetCachedStyle(
                StyleComponentType.Input,
                variant,
                size,
                baseInputStyle ?? GUI.skin.textField,
                state,
                customLogic: style =>
                {
                    if (disabled)
                        style.normal.textColor = ThemeManager.Instance.CurrentTheme.Muted;
                    if (focused)
                        style.focused.background = inputFocusedTexture;
                }
            );
        }

        public GUIStyle GetLabelStyle(ControlVariant variant, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Label, variant, size, baseLabelStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetPasswordFieldStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            int state = (focused ? 1 : 0) | (disabled ? 2 : 0);
            return GetCachedStyle(
                StyleComponentType.PasswordField,
                variant,
                size,
                passwordFieldStyle ?? GUI.skin.textField,
                state,
                customLogic: style =>
                {
                    if (disabled)
                        style.normal.textColor = ThemeManager.Instance.CurrentTheme.Muted;
                    if (focused)
                        style.focused.background = inputFocusedTexture;
                }
            );
        }

        public GUIStyle GetTextAreaStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false) => GetCachedStyle(StyleComponentType.TextArea, variant, size, textAreaStyle ?? GUI.skin.textArea, focused ? 1 : 0);

        public GUIStyle GetProgressBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.ProgressBar, variant, size, progressBarStyle ?? GUI.skin.box);

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            GUIStyle baseStyle = orientation == SeparatorOrientation.Vertical ? (separatorVerticalStyle ?? GUI.skin.box) : (separatorHorizontalStyle ?? GUI.skin.box);
            return GetCachedStyle(
                StyleComponentType.Separator,
                variant,
                size,
                baseStyle,
                (int)orientation,
                applySizing: false,
                customLogic: style =>
                {
                    float thickness = size switch
                    {
                        ControlSize.Small => 1f,
                        ControlSize.Large => 4f,
                        ControlSize.Mini => 1f,
                        ControlSize.Default => 2f,
                        _ => 1f,
                    };
                    if (orientation == SeparatorOrientation.Horizontal)
                        style.fixedHeight = GetScaledHeight(thickness);
                    else
                        style.fixedWidth = GetScaledHeight(thickness);
                }
            );
        }

        public GUIStyle GetTabsListStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.TabsList, variant, size, tabsListStyle ?? GUI.skin.box);

        public GUIStyle GetTabsTriggerStyle(bool active = false, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            GUIStyle baseStyle = active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) : (tabsTriggerStyle ?? GUI.skin.button);
            return GetCachedStyle(
                StyleComponentType.TabsTrigger,
                variant,
                size,
                baseStyle,
                active ? 1 : 0,
                applyVariant: variant != ControlVariant.Default,
                customLogic: variant != ControlVariant.Default && active
                    ? style =>
                    {
                        style.normal = style.onNormal;
                        style.hover = style.onHover;
                        style.active = style.onActive;
                    }
                    : null
            );
        }

        public GUIStyle GetTabsContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.TabsContent, variant, size, tabsContentStyle ?? GUIStyle.none);

        public GUIStyle GetCheckboxStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Checkbox, variant, size, baseCheckboxStyle ?? GUI.skin.toggle);

        public GUIStyle GetSwitchStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Switch, variant, size, baseSwitchStyle ?? GUI.skin.toggle);

        public GUIStyle GetBadgeStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Badge, variant, size, baseBadgeStyle ?? GUI.skin.box);

        public GUIStyle GetAvatarStyle(ControlSize size, AvatarShape shape, ControlVariant variant = ControlVariant.Default)
        {
            return GetCachedStyle(
                StyleComponentType.Avatar,
                variant,
                size,
                avatarStyle ?? GUI.skin.box,
                (int)shape,
                applyVariant: false,
                applySizing: false,
                customLogic: style =>
                {
                    int avatarSizeValue = size switch
                    {
                        ControlSize.Small => GetScaledHeight(32f),
                        ControlSize.Large => GetScaledHeight(48f),
                        ControlSize.Mini => GetScaledHeight(24f),
                        ControlSize.Default => GetScaledHeight(40f),
                        _ => GetScaledHeight(40f),
                    };
                    style.fixedWidth = avatarSizeValue;
                    style.fixedHeight = avatarSizeValue;
                    int borderRadius = shape switch
                    {
                        AvatarShape.Circle => Mathf.RoundToInt(50 * guiHelper.uiScale),
                        AvatarShape.Rounded => GetScaledBorderRadius(8f),
                        AvatarShape.Square => 0,
                        _ => 0,
                    };
                    style.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                    ApplyContainerVariant(style, variant, ThemeManager.Instance.CurrentTheme);
                }
            );
        }

        public float GetStatusIndicatorSize(ControlSize size) =>
            size switch
            {
                ControlSize.Small => 6f,
                ControlSize.Large => 12f,
                ControlSize.Mini => 4f,
                ControlSize.Default => 8f,
                _ => 8f,
            };

        public GUIStyle GetTableStyle(ControlVariant variant, ControlSize size)
        {
            GUIStyle baseStyle = variant switch
            {
                ControlVariant.Secondary => tableStripedStyle,
                ControlVariant.Outline => tableBorderedStyle,
                ControlVariant.Ghost => tableHoverStyle,
                _ => baseTableStyle,
            };
            return GetCachedStyle(StyleComponentType.Table, variant, size, baseStyle ?? GUI.skin.box);
        }

        public GUIStyle GetTableHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.TableHeader, variant, size, tableHeaderStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetTableCellStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, TextAnchor alignment = TextAnchor.MiddleLeft) =>
            GetCachedStyle(StyleComponentType.TableCell, variant, size, tableCellStyle ?? GUI.skin.label, (int)alignment, applyVariant: false, customLogic: style => style.alignment = alignment);

        public GUIStyle GetCalendarStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Calendar, variant, size, baseCalendarStyle ?? GUI.skin.box);

        public GUIStyle GetCalendarHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CalendarHeader, variant, size, calendarHeaderStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetCalendarWeekdayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CalendarWeekday, variant, size, calendarWeekdayStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetCalendarDayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CalendarDay, variant, size, calendarDayStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetCalendarDaySelectedStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.CalendarDaySelected,
                variant,
                size,
                calendarDaySelectedStyle ?? GUI.skin.button,
                applyVariant: variant != ControlVariant.Default,
                customLogic: variant != ControlVariant.Default
                    ? style =>
                    {
                        style.normal = style.onNormal;
                        style.hover = style.onHover;
                        style.active = style.onActive;
                    }
                    : null
            );

        public GUIStyle GetCalendarDayOutsideMonthStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.CalendarDayOutsideMonth, variant, size, calendarDayOutsideMonthStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetCalendarDayTodayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CalendarDayToday, variant, size, calendarDayTodayStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetCalendarDayInRangeStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            return GetCachedStyle(
                StyleComponentType.CalendarDayInRange,
                variant,
                size,
                calendarDayInRangeStyle ?? GUI.skin.button,
                applyVariant: variant != ControlVariant.Default,
                customLogic: variant != ControlVariant.Default
                    ? style =>
                    {
                        var theme = ThemeManager.Instance.CurrentTheme;
                        style.normal.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.55f));
                    }
                    : null
            );
        }

        public GUIStyle GetDatePickerStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.DatePicker, variant, size, baseDatePickerStyle ?? GUI.skin.box);

        public GUIStyle GetDatePickerWeekdayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.DatePickerWeekday, variant, size, datePickerWeekdayStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetDatePickerDayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.DatePickerDay, variant, size, datePickerDayStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetDatePickerDaySelectedStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            return GetCachedStyle(
                StyleComponentType.DatePickerDaySelected,
                variant,
                size,
                datePickerDaySelectedStyle ?? GUI.skin.button,
                applyVariant: variant != ControlVariant.Default,
                customLogic: variant != ControlVariant.Default
                    ? style =>
                    {
                        style.normal = style.onNormal;
                        style.hover = style.onHover;
                        style.active = style.onActive;
                    }
                    : null
            );
        }

        public GUIStyle GetDatePickerDayOutsideMonthStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.DatePickerDayOutsideMonth, variant, size, datePickerDayOutsideMonthStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetDatePickerDayTodayStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.DatePickerDayToday, variant, size, datePickerDayTodayStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetDialogContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Dialog, variant, size, dialogContentStyle ?? GUI.skin.box);

        public GUIStyle GetDropdownMenuItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.DropdownMenuItem, variant, size, dropdownMenuItemStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetDropdownMenuStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.MenuDropdown, variant, size, dropdownMenuContentStyle ?? GUI.skin.box);

        public GUIStyle GetPopoverContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Popover, variant, size, popoverContentStyle ?? GUI.skin.box);

        public GUIStyle GetSelectTriggerStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.SelectTrigger, variant, size, selectTriggerStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetSelectItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.SelectItem, variant, size, selectItemStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetSelectStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.SelectContent, variant, size, selectContentStyle ?? GUI.skin.box);

        public GUIStyle GetChartStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Chart, variant, size, chartContainerStyle ?? GUI.skin.box);

        public GUIStyle GetChartAxisStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.ChartAxis, variant, size, chartAxisStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetMenuBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.MenuBar, variant, size, menuBarStyle ?? GUIStyle.none);

        public GUIStyle GetMenuBarItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool isShortcut = false) =>
            GetCachedStyle(StyleComponentType.MenuBarItem, variant, size, menuBarItemStyle ?? GUI.skin.button, isShortcut ? 1 : 0, applyVariant: variant != ControlVariant.Default, customLogic: isShortcut ? style => style.alignment = TextAnchor.MiddleRight : null);

        public GUIStyle GetMenuDropdownStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.MenuDropdown, variant, size, menuDropdownStyle ?? GUI.skin.box);

        public GUIStyle GetAnimatedBoxStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.AnimatedBox, variant, size, animatedBoxStyle ?? GUI.skin.box);

        public GUIStyle GetSectionHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.SectionHeader, variant, size, sectionHeaderStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetCardStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Card, variant, size, cardStyle ?? GUI.skin.box);

        public GUIStyle GetCardHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CardHeader, variant, size, cardHeaderStyle ?? GUIStyle.none);

        public GUIStyle GetCardTitleStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CardTitle, variant, size, cardTitleStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetCardDescriptionStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CardDescription, variant, size, cardDescriptionStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetCardContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CardContent, variant, size, cardContentStyle ?? GUIStyle.none);

        public GUIStyle GetCardFooterStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.CardFooter, variant, size, cardFooterStyle ?? GUIStyle.none);

        public Texture2D GetParticleTexture() => particleTexture;

        #endregion
    }
}
