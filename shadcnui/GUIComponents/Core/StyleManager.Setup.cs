using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    public partial class StyleManager
    {
        #region Style Setup Helpers

        private void SetupStyle(ref GUIStyle style, Texture2D bg, int size, RectOffset pad, RectOffset border, TextAnchor anchor = TextAnchor.MiddleLeft, FontStyle font = FontStyle.Normal)
        {
            style = new UnityHelpers.GUIStyle(GUI.skin.box);
            if (customFont != null)
                style.font = customFont;
            if (bg != null)
                style.normal.background = bg;
            if (size > 0)
                style.fontSize = size;
            style.fontStyle = font;
            if (pad != null)
                style.padding = pad;
            if (border != null)
                style.border = border;
            style.alignment = anchor;
        }

        private GUIStyle CreateStyle(Texture2D bg = null, int fontSize = 0, RectOffset padding = null, RectOffset border = null, TextAnchor anchor = TextAnchor.MiddleLeft, FontStyle fontStyle = FontStyle.Normal, Action<GUIStyle> customSetup = null)
        {
            var style = new UnityHelpers.GUIStyle(GUI.skin.box);
            if (customFont != null)
                style.font = customFont;

            style.normal.background = bg;
            style.normal.textColor = ThemeManager.Instance.CurrentTheme.Text;

            if (fontSize > 0)
                style.fontSize = fontSize;
            style.fontStyle = fontStyle;
            if (padding != null)
                style.padding = padding;
            if (border != null)
                style.border = border;
            style.alignment = anchor;

            customSetup?.Invoke(style);
            return style;
        }

        private GUIStyle CreateLabelStyle(int fontSize = 0, RectOffset padding = null, TextAnchor anchor = TextAnchor.MiddleLeft, FontStyle fontStyle = FontStyle.Normal, Action<GUIStyle> customSetup = null)
        {
            var style = new UnityHelpers.GUIStyle(GUI.skin.label);
            if (customFont != null)
                style.font = customFont;
            style.normal.background = null;
            if (fontSize > 0)
                style.fontSize = fontSize;
            style.fontStyle = fontStyle;
            if (padding != null)
                style.padding = padding;
            style.alignment = anchor;
            style.richText = true;
            customSetup?.Invoke(style);
            return style;
        }

        private void ApplyInteractiveStates(GUIStyle style, Color baseBg, Color fgColor, Func<Color, Texture2D> texCreator, bool isToggle = false, Color? activeBg = null, Color? activeFg = null)
        {
            style.normal.background = texCreator(baseBg);
            style.normal.textColor = fgColor;

            Color hoverColor = GetHoverColor(baseBg, true);
            style.hover.background = texCreator(hoverColor);
            style.hover.textColor = fgColor;

            Color pressedColor = Color.Lerp(baseBg, Color.black, 0.15f);
            style.active.background = texCreator(pressedColor);
            style.active.textColor = fgColor;

            style.focused.background = texCreator(hoverColor);
            style.focused.textColor = fgColor;

            if (isToggle && activeBg.HasValue && activeFg.HasValue)
            {
                Color onBg = activeBg.Value;
                Color onFg = activeFg.Value;
                Color onHover = GetHoverColor(onBg, true);
                Color onPressed = Color.Lerp(onBg, Color.black, 0.15f);

                style.onNormal.background = texCreator(onBg);
                style.onNormal.textColor = onFg;

                style.onHover.background = texCreator(onHover);
                style.onHover.textColor = onFg;

                style.onActive.background = texCreator(onPressed);
                style.onActive.textColor = onFg;

                style.onFocused.background = texCreator(onHover);
                style.onFocused.textColor = onFg;
            }
        }

        #endregion

        #region Style Setup - Animated
        private void SetupAnimatedStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                animatedBoxStyle = CreateStyle(gradientTexture, 0, new UnityHelpers.RectOffset(16, 16, 16, 16), GetBorderOffset(guiHelper.cornerRadius));

                sectionHeaderStyle = CreateLabelStyle(
                    guiHelper.fontSize + 2,
                    fontStyle: FontStyle.Bold,
                    customSetup: s =>
                    {
                        s.normal.textColor = Color.Lerp(theme.Accent, theme.Text, 0.6f);
                        s.margin = GetSpacingOffset(0, 8);
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAnimatedStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Card
        private void SetupCardStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                cardStyle = CreateStyle(cardBackgroundTexture, 0, GetSpacingOffset(24f, 24f), new UnityHelpers.RectOffset(12, 12, 12, 12));
                cardHeaderStyle = new UnityHelpers.GUIStyle { padding = GetSpacingOffset(0f, 8f) };

                cardTitleStyle = CreateLabelStyle(
                    GetScaledFontSize(1.25f),
                    fontStyle: FontStyle.Bold,
                    anchor: TextAnchor.UpperLeft,
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Text;
                        s.wordWrap = true;
                        s.margin = new UnityHelpers.RectOffset(0, 0, 0, 4);
                    }
                );

                cardDescriptionStyle = CreateLabelStyle(
                    GetScaledFontSize(0.875f),
                    anchor: TextAnchor.UpperLeft,
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Muted;
                        s.wordWrap = true;
                    }
                );

                cardContentStyle = new UnityHelpers.GUIStyle { padding = GetSpacingOffset(0f, 16f) };
                cardFooterStyle = new UnityHelpers.GUIStyle { padding = GetSpacingOffset(0f, 0f), alignment = TextAnchor.MiddleLeft };
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCardStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Button Base
        private void SetupButtonStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                int radius = GetScaledBorderRadius(6f);
                Func<Color, Texture2D> btnTex = c => CreateRoundedRectTexture(128, 36, radius, c);

                baseButtonStyle = CreateStyle(
                    null,
                    GetScaledFontSize(0.875f),
                    GetSpacingOffset(16f, 8f),
                    GetBorderOffset(6f),
                    TextAnchor.MiddleCenter,
                    FontStyle.Bold,
                    s =>
                    {
                        s.fixedHeight = GetScaledHeight(36f);
                        ApplyInteractiveStates(s, theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, btnTex);
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupButtonStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Toggle Base
        private void SetupToggleStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                int radius = GetScaledBorderRadius(4f);
                Func<Color, Texture2D> toggleTex = c => CreateRoundedRectTexture(32, 32, radius, c);

                Color offBgColor = Color.Lerp(theme.Secondary, theme.Base, 0.5f);

                baseToggleStyle = CreateStyle(
                    null,
                    guiHelper.fontSize,
                    GetSpacingOffset(12f, 6f),
                    GetBorderOffset(4f),
                    TextAnchor.MiddleCenter,
                    customSetup: s =>
                    {
                        ApplyInteractiveStates(s, offBgColor, theme.Text, toggleTex, true, theme.Accent, theme.ButtonPrimaryFg);
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupToggleStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Input Base
        private void SetupInputStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                baseInputStyle = CreateStyle(
                    inputBackgroundTexture,
                    GetScaledFontSize(0.875f),
                    GetSpacingOffset(12f, 8f),
                    GetBorderOffset(6f),
                    customSetup: s =>
                    {
                        s.fixedHeight = GetScaledHeight(36f);
                        s.margin = GetSpacingOffset(0f, 8f);
                        s.clipping = TextClipping.Clip;
                        s.contentOffset = new Vector2(2f, 0f);

                        s.normal.textColor = theme.Text;
                        s.hover.textColor = theme.Text;

                        s.focused.background = inputFocusedTexture;
                        s.focused.textColor = theme.Text;

                        s.active.background = inputFocusedTexture;
                        s.active.textColor = theme.Text;
                    }
                );

                passwordFieldStyle = new UnityHelpers.GUIStyle(baseInputStyle);
                passwordFieldStyle.fontSize = guiHelper.fontSize + 2;

                textAreaStyle = CreateStyle(
                    inputBackgroundTexture,
                    guiHelper.fontSize,
                    GetSpacingOffset(12f, 8f),
                    GetBorderOffset(6f),
                    customSetup: s =>
                    {
                        s.wordWrap = true;
                        s.stretchHeight = true;
                        s.normal.textColor = theme.Text;
                        s.focused.background = inputFocusedTexture;
                        s.focused.textColor = theme.Text;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupInputStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Label Base
        private void SetupLabelStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseLabelStyle = CreateLabelStyle(
                    GetScaledFontSize(1.0f),
                    GetSpacingOffset(0f, 4f),
                    TextAnchor.UpperLeft,
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Text;
                        s.wordWrap = true;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupLabelStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Progress
        private void SetupProgressBarStyles()
        {
            try
            {
                progressBarStyle = CreateStyle(
                    progressBarBackgroundTexture,
                    0,
                    GetSpacingOffset(0f, 0f),
                    GetBorderOffset(8f),
                    customSetup: s =>
                    {
                        s.margin = GetSpacingOffset(0f, 6f);
                        s.fixedHeight = GetScaledHeight(12f);
                        s.hover = s.normal;
                        s.active = s.normal;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupProgressBarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Separator
        private void SetupSeparatorStyles()
        {
            try
            {
                separatorHorizontalStyle = new UnityHelpers.GUIStyle
                {
                    normal = { background = separatorTexture },
                    fixedHeight = Mathf.Max(1, Mathf.RoundToInt(1.0f * guiHelper.uiScale)),
                    stretchWidth = true,
                    margin = GetSpacingOffset(0f, 12f),
                };
                separatorVerticalStyle = new UnityHelpers.GUIStyle
                {
                    normal = { background = separatorTexture },
                    fixedWidth = Mathf.Max(1, Mathf.RoundToInt(1.0f * guiHelper.uiScale)),
                    stretchHeight = true,
                    margin = GetSpacingOffset(12f, 0f),
                };
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSeparatorStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Tabs
        private void SetupTabsStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                tabsListStyle = CreateStyle(tabsBackgroundTexture, 0, GetSpacingOffset(4f, 4f), GetBorderOffset(6f));

                tabsTriggerStyle = CreateStyle(
                    null,
                    GetScaledFontSize(0.9f),
                    GetSpacingOffset(16f, 6f),
                    GetBorderOffset(4f),
                    TextAnchor.MiddleCenter,
                    FontStyle.Bold,
                    s =>
                    {
                        s.normal.background = transparentTexture;
                        s.normal.textColor = theme.Muted;

                        Texture2D hoverTex = CreateRoundedRectTexture(128, 128, 4, Color.Lerp(theme.TabsBg, theme.Text, 0.05f));
                        s.hover.background = hoverTex;
                        s.hover.textColor = theme.Text;

                        s.active.background = hoverTex;
                        s.active.textColor = theme.Text;
                    }
                );

                tabsTriggerActiveStyle = new UnityHelpers.GUIStyle(tabsTriggerStyle);
                if (customFont != null)
                    tabsTriggerActiveStyle.font = customFont;

                tabsTriggerActiveStyle.normal.background = tabsActiveTexture;
                tabsTriggerActiveStyle.normal.textColor = theme.TabsTriggerActiveFg;
                tabsTriggerActiveStyle.hover = tabsTriggerActiveStyle.normal;
                tabsTriggerActiveStyle.active = tabsTriggerActiveStyle.normal;
                tabsTriggerActiveStyle.focused = tabsTriggerActiveStyle.normal;

                tabsContentStyle = new UnityHelpers.GUIStyle { padding = GetSpacingOffset(18f, 18f) };
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTabsStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Checkbox & Switch
        private void SetupCheckboxStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                int width = 18;
                int height = 18;
                int radius = GetScaledBorderRadius(4f);

                Func<Color, Texture2D> fill = c => CreateRoundedRectTexture(width, height, radius, c);
                Func<Color, Texture2D> outline = c => CreateRoundedOutlineTexture(width, height, radius, c, 1);

                baseCheckboxStyle = CreateStyle(
                    null,
                    GetScaledFontSize(0.9f),
                    GetSpacingOffset(8f, 0f),
                    GetBorderOffset(4f),
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Text;
                        s.normal.background = outline(theme.Border);
                        s.hover.background = outline(Color.Lerp(theme.Border, theme.Text, 0.4f));
                        s.active.background = outline(theme.Text);

                        s.onNormal.background = fill(theme.Accent);
                        s.onNormal.textColor = theme.ButtonPrimaryFg;

                        Color accentHover = GetHoverColor(theme.Accent, true);
                        s.onHover.background = fill(accentHover);
                        s.onHover.textColor = theme.ButtonPrimaryFg;

                        s.onActive.background = fill(Color.Lerp(theme.Accent, Color.black, 0.2f));
                        s.onActive.textColor = theme.ButtonPrimaryFg;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCheckboxStyle", "StyleManager");
            }
        }

        private void SetupSwitchStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;

                int width = 36;
                int height = 20;
                int radius = GetScaledBorderRadius(10f);

                Func<Color, Texture2D> fill = c => CreateRoundedRectTexture(width, height, radius, c);

                baseSwitchStyle = CreateStyle(
                    null,
                    GetScaledFontSize(0.9f),
                    GetSpacingOffset(8f, 0f),
                    GetBorderOffset(10f),
                    customSetup: s =>
                    {
                        Color offColor = Color.Lerp(theme.Secondary, theme.Text, 0.2f);

                        s.normal.textColor = theme.Text;
                        s.normal.background = fill(offColor);
                        s.hover.background = fill(GetHoverColor(offColor, true));
                        s.active.background = fill(offColor);

                        s.onNormal.background = fill(theme.Accent);
                        s.onNormal.textColor = theme.Text;
                        s.onHover.background = fill(GetHoverColor(theme.Accent, true));
                        s.onActive.background = fill(Color.Lerp(theme.Accent, Color.black, 0.1f));
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSwitchStyle", "StyleManager");
            }
        }

        #endregion

        #region Style Setup - Badge & Avatar
        private void SetupBadgeStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseBadgeStyle = CreateStyle(badgeTexture, GetScaledFontSize(0.70f), GetSpacingOffset(8f, 1f), GetBorderOffset(12f), TextAnchor.MiddleCenter, FontStyle.Bold, s => s.normal.textColor = theme.ButtonPrimaryFg);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupBadgeStyle", "StyleManager");
            }
        }

        private void SetupAvatarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                int size = GetScaledHeight(40f);
                int radius = Mathf.RoundToInt(50 * guiHelper.uiScale);
                avatarStyle = CreateStyle(
                    CreateSolidTexture(theme.Muted),
                    0,
                    null,
                    new UnityHelpers.RectOffset(radius, radius, radius, radius),
                    TextAnchor.MiddleCenter,
                    customSetup: s =>
                    {
                        s.fixedWidth = size;
                        s.fixedHeight = size;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAvatarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Table
        private void SetupTableStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseTableStyle = CreateStyle(tableTexture, 0, GetSpacingOffset(0f, 0f), GetBorderOffset(1f));

                tableHeaderStyle = CreateStyle(
                    tableHeaderTexture,
                    GetScaledFontSize(0.875f),
                    GetSpacingOffset(12f, 10f),
                    GetBorderOffset(0f),
                    TextAnchor.MiddleLeft,
                    FontStyle.Bold,
                    s =>
                    {
                        s.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Text, 0.05f));
                        s.normal.textColor = theme.Muted;
                        s.hover.textColor = theme.Text;
                    }
                );

                tableCellStyle = CreateStyle(
                    tableCellTexture,
                    GetScaledFontSize(0.875f),
                    GetSpacingOffset(12f, 10f),
                    anchor: TextAnchor.MiddleLeft,
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Text;
                        s.wordWrap = false;
                        s.clipping = TextClipping.Clip;
                    }
                );

                tableStripedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableStripedStyle.font = customFont;
                tableStripedStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Base, theme.Secondary, 0.3f));

                tableBorderedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableBorderedStyle.font = customFont;
                tableBorderedStyle.normal.background = CreateBorderTexture(theme.Border, 1);

                tableHoverStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableHoverStyle.font = customFont;

                Texture2D hoverTex = CreateSolidTexture(Color.Lerp(theme.Base, theme.Secondary, 0.8f));
                tableHoverStyle.hover.background = hoverTex;
                tableHoverStyle.active.background = hoverTex;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTableStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Calendar
        private void SetupCalendarStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseCalendarStyle = CreateStyle(calendarBackgroundTexture, 0, GetSpacingOffset(12f, 12f), GetBorderOffset(8f));

                calendarHeaderStyle = new UnityHelpers.GUIStyle { padding = GetSpacingOffset(0f, 8f) };

                calendarWeekdayStyle = CreateLabelStyle(GetScaledFontSize(0.8f), anchor: TextAnchor.MiddleCenter, fontStyle: FontStyle.Bold, customSetup: s => s.normal.textColor = theme.Muted);

                calendarDayStyle = CreateStyle(
                    calendarDayTexture,
                    GetScaledFontSize(0.9f),
                    anchor: TextAnchor.MiddleCenter,
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Text;
                        s.hover.background = CreateRoundedRectTexture(32, 32, 4, theme.Secondary);
                        s.hover.textColor = theme.Text;
                    }
                );

                calendarDaySelectedStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDaySelectedStyle.font = customFont;
                calendarDaySelectedStyle.normal.background = calendarDaySelectedTexture;
                calendarDaySelectedStyle.normal.textColor = theme.ButtonPrimaryFg;
                calendarDaySelectedStyle.hover = calendarDaySelectedStyle.normal;

                calendarDayOutsideMonthStyle = new UnityHelpers.GUIStyle(calendarDayStyle) { normal = { textColor = Color.Lerp(theme.Muted, theme.Base, 0.5f) } };
                if (customFont != null)
                    calendarDayOutsideMonthStyle.font = customFont;

                calendarDayTodayStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayTodayStyle.font = customFont;
                calendarDayTodayStyle.normal.background = CreateRoundedOutlineTexture(32, 32, 4, theme.Text, 1);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCalendarStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Dropdowns & Popovers
        private void SetupDropdownMenuStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dropdownMenuContentStyle = CreateStyle(dropdownMenuContentTexture, 0, GetSpacingOffset(4f, 4f), GetBorderOffset(6f));

                dropdownMenuItemStyle = CreateStyle(
                    null,
                    GetScaledFontSize(0.9f),
                    GetSpacingOffset(12f, 8f),
                    anchor: TextAnchor.MiddleLeft,
                    customSetup: s =>
                    {
                        s.normal.background = transparentTexture;
                        s.normal.textColor = theme.Text;

                        Texture2D hoverTex = CreateRoundedRectTexture(128, 32, 4, theme.Secondary);
                        s.hover.background = hoverTex;
                        s.hover.textColor = theme.Text;

                        s.active.background = hoverTex;
                        s.active.textColor = theme.Text;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDropdownMenuStyles", "StyleManager");
            }
        }

        private void SetupPopoverStyles()
        {
            try
            {
                popoverContentStyle = CreateStyle(popoverContentTexture, 0, GetSpacingOffset(16f, 16f), GetBorderOffset(8f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupPopoverStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Select
        private void SetupSelectStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                float fontSize = Mathf.RoundToInt(guiHelper.fontSize * guiHelper.uiScale);

                selectTriggerStyle = CreateStyle(
                    selectTriggerTexture,
                    (int)fontSize,
                    new UnityHelpers.RectOffset(12, 12, 8, 8),
                    new UnityHelpers.RectOffset(6, 6, 6, 6),
                    TextAnchor.MiddleLeft,
                    customSetup: s =>
                    {
                        s.normal.textColor = theme.Text;
                        s.hover.textColor = theme.Text;
                        s.hover.background = selectTriggerTexture;
                    }
                );

                selectContentStyle = CreateStyle(selectContentTexture, 0, new UnityHelpers.RectOffset(4, 4, 4, 4), new UnityHelpers.RectOffset(6, 6, 6, 6));

                selectItemStyle = CreateStyle(
                    null,
                    (int)fontSize,
                    new UnityHelpers.RectOffset(12, 12, 8, 8),
                    anchor: TextAnchor.MiddleLeft,
                    customSetup: s =>
                    {
                        s.normal.background = transparentTexture;
                        s.normal.textColor = theme.Text;

                        Texture2D hoverTex = CreateRoundedRectTexture(128, 32, 4, theme.Secondary);
                        s.hover.background = hoverTex;
                        s.hover.textColor = theme.Text;

                        s.active.background = hoverTex;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSelectStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - DatePicker
        private void SetupDatePickerStyle()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                baseDatePickerStyle = CreateStyle(CreateBorderedRoundedRectTexture(256, 256, 8, theme.Elevated, theme.Border, 1), 0, new UnityHelpers.RectOffset(16, 16, 16, 16), new UnityHelpers.RectOffset(8, 8, 8, 8));

                datePickerHeaderStyle = new UnityHelpers.GUIStyle { padding = new UnityHelpers.RectOffset(0, 0, 5, 10) };
                datePickerWeekdayStyle = CreateLabelStyle(guiHelper.fontSize - 1, anchor: TextAnchor.MiddleCenter, customSetup: s => s.normal.textColor = theme.Muted);

                datePickerDayStyle = CreateStyle(
                    null,
                    guiHelper.fontSize,
                    anchor: TextAnchor.MiddleCenter,
                    customSetup: s =>
                    {
                        s.normal.background = transparentTexture;
                        s.normal.textColor = theme.Text;
                        s.hover.background = CreateRoundedRectTexture(32, 32, 4, theme.Secondary);
                    }
                );

                datePickerDaySelectedStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDaySelectedStyle.font = customFont;
                datePickerDaySelectedStyle.normal.background = CreateRoundedRectTexture(32, 32, 4, theme.Accent);
                datePickerDaySelectedStyle.normal.textColor = theme.ButtonPrimaryFg;

                datePickerDayTodayStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayTodayStyle.font = customFont;
                datePickerDayTodayStyle.normal.background = CreateRoundedOutlineTexture(32, 32, 4, theme.Text, 1);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDatePickerStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Dialog & Chart
        private void SetupDialogStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dialogContentStyle = CreateStyle(CreateBorderedRoundedRectTexture(512, 512, 12, theme.Elevated, theme.Border, 1), 0, new UnityHelpers.RectOffset(24, 24, 24, 24), new UnityHelpers.RectOffset(12, 12, 12, 12));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDialogStyles", "StyleManager");
            }
        }

        private void SetupChartStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                chartContainerStyle = CreateStyle(chartContainerTexture, 0, GetSpacingOffset(16f, 16f), GetBorderOffset(8f));
                chartAxisStyle = CreateLabelStyle(GetScaledFontSize(0.75f), anchor: TextAnchor.MiddleCenter, customSetup: s => s.normal.textColor = theme.Muted);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupChartStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - MenuBar
        private void SetupMenuBarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                menuBarStyle = new UnityHelpers.GUIStyle
                {
                    padding = GetSpacingOffset(4f, 0f),
                    margin = GetSpacingOffset(0f, 0f),
                    fixedHeight = GetScaledHeight(40f),
                    stretchWidth = false,
                    alignment = TextAnchor.MiddleLeft,
                };

                menuBarItemStyle = CreateStyle(
                    null,
                    GetScaledFontSize(0.875f),
                    GetSpacingOffset(12f, 8f),
                    anchor: TextAnchor.MiddleLeft,
                    customSetup: s =>
                    {
                        s.margin = GetSpacingOffset(0f, 0f);
                        s.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                        s.normal.background = transparentTexture;
                        s.normal.textColor = theme.Text;

                        Texture2D hoverTex = CreateRoundedRectTexture(128, 32, 4, theme.Secondary);
                        s.hover.background = hoverTex;
                        s.hover.textColor = theme.Text;

                        s.active = s.hover;
                        s.onNormal = s.hover;
                        s.stretchWidth = false;
                    }
                );

                menuDropdownStyle = CreateStyle(
                    CreateBorderedRoundedRectTexture(200, 200, 6, theme.Elevated, theme.Border, 1),
                    0,
                    GetSpacingOffset(4f, 4f),
                    GetBorderOffset(6f),
                    customSetup: s =>
                    {
                        s.margin = GetSpacingOffset(0f, 0f);
                        s.fixedWidth = 200;
                        s.stretchWidth = false;
                    }
                );
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupMenuBarStyles", "StyleManager");
            }
        }
        #endregion
    }
}
