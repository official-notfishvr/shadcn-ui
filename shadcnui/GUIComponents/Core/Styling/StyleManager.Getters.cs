using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core.Styling
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
                    style.normal.background = TransparentTexture;
                    style.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
                    break;
                case ControlVariant.Ghost:
                    style.normal.background = TransparentTexture;
                    break;
                case ControlVariant.Muted:
                    style.normal.background = CreateSolidTexture(theme.Muted);
                    break;
            }
        }

        private void ApplyVariantToStyle(GUIStyle style, StyleComponentType type, ControlVariant variant)
        {
            var modifier = Registry.GetVariantModifier(type, variant);
            modifier?.Invoke(style, ThemeManager.Instance.CurrentTheme, _guiHelper);
        }

        private void ApplySizing(GUIStyle style, StyleComponentType type, ControlSize size)
        {
            var modifier = Registry.GetSizeModifier(type, size);
            modifier?.Invoke(style, ThemeManager.Instance.CurrentTheme, _guiHelper);
        }
        #endregion

        #region Helper Methods
        private GUIStyle GetCachedStyle(StyleComponentType componentType, ControlVariant variant, ControlSize size, GUIStyle baseStyle, int state = 0, bool applyVariant = true, bool applySizing = true, Action<GUIStyle> customLogic = null)
        {
            var key = new StyleKey(componentType, variant, size, state);

            if (_styleCache.TryGetValue(key, out var cached))
            {
                TouchCacheEntry(key);
                return cached.Style;
            }

            var style = Style(baseStyle).Build();
            customLogic?.Invoke(style);

            if (applyVariant)
                ApplyVariantToStyle(style, componentType, variant);

            if (applySizing)
                ApplySizing(style, componentType, size);

            AddToCache(key, style);
            return style;
        }

        private GUIStyle GetSimpleStyle(StyleComponentType type, GUIStyle baseStyle, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(type, variant, size, baseStyle);

        private GUIStyle GetLabelBasedStyle(StyleComponentType type, float fontScale, Action<GUIStyle, Theme> extraSetup = null, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                type,
                variant,
                size,
                _baseLabelStyle ?? GUI.skin.label,
                applyVariant: false,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    s.fontSize = GetScaledFontSize(fontScale);
                    extraSetup?.Invoke(s, t);
                }
            );

        #endregion

        #region Style Getters - Basic
        public GUIStyle GetButtonStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Button, variant, size, _baseButtonStyle ?? GUI.skin.button);

        public GUIStyle GetToggleStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Toggle, variant, size, _baseToggleStyle ?? GUI.skin.button);

        public GUIStyle GetLabelStyle(ControlVariant variant, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Label, variant, size, _baseLabelStyle ?? GUI.skin.label, applyVariant: false);

        public GUIStyle GetProgressBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetSimpleStyle(StyleComponentType.ProgressBar, _progressBarStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetBadgeStyle(ControlVariant variant, ControlSize size) => GetSimpleStyle(StyleComponentType.Badge, _baseBadgeStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetCardStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetSimpleStyle(StyleComponentType.Card, _cardStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetDialogContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetSimpleStyle(StyleComponentType.Dialog, _dialogContentStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetChartStyle(ControlVariant variant, ControlSize size) => GetSimpleStyle(StyleComponentType.Chart, _chartContainerStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetAnimatedBoxStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Large) => GetSimpleStyle(StyleComponentType.AnimatedBox, AnimatedBoxStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetMenuBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetSimpleStyle(StyleComponentType.MenuBar, _menuBarStyle ?? GUIStyle.none, variant, size);

        public GUIStyle GetTabsListStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetSimpleStyle(StyleComponentType.TabsList, _tabsListStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetSelectStyle(ControlVariant variant, ControlSize size) => GetSimpleStyle(StyleComponentType.SelectContent, _dropdownContentStyle ?? GUI.skin.box, variant, size);

        public GUIStyle GetDropdownMenuStyle(ControlVariant variant, ControlSize size) => GetSimpleStyle(StyleComponentType.DropdownMenu, _dropdownContentStyle ?? GUI.skin.box, variant, size);
        #endregion

        #region Style Getters - Input
        public GUIStyle GetInputStyle(ControlVariant variant, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            int state = (focused ? 1 : 0) | (disabled ? 2 : 0);
            return GetCachedStyle(
                StyleComponentType.Input,
                variant,
                size,
                _baseInputStyle ?? GUI.skin.textField,
                state,
                customLogic: s =>
                {
                    if (disabled)
                        s.normal.textColor = ThemeManager.Instance.CurrentTheme.Muted;
                    if (focused)
                        s.focused.background = InputFocusedTexture;
                }
            );
        }

        public GUIStyle GetPasswordFieldStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false)
        {
            int state = (focused ? 1 : 0) | (disabled ? 2 : 0);
            return GetCachedStyle(
                StyleComponentType.PasswordField,
                variant,
                size,
                _baseInputStyle ?? GUI.skin.textField,
                state,
                customLogic: s =>
                {
                    s.fontSize = _guiHelper.fontSize + 2;
                    if (disabled)
                        s.normal.textColor = ThemeManager.Instance.CurrentTheme.Muted;
                    if (focused)
                        s.focused.background = InputFocusedTexture;
                }
            );
        }

        public GUIStyle GetTextAreaStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false) =>
            GetCachedStyle(
                StyleComponentType.TextArea,
                variant,
                size,
                _baseInputStyle ?? GUI.skin.textArea,
                focused ? 1 : 0,
                customLogic: s =>
                {
                    s.wordWrap = true;
                    s.stretchHeight = true;
                    s.fixedHeight = 0;
                    s.normal.textColor = ThemeManager.Instance.CurrentTheme.Text;
                    if (focused)
                        s.focused.background = InputFocusedTexture;
                }
            );
        #endregion

        #region Style Getters - Separator
        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            float thickness = size switch
            {
                ControlSize.Small => DesignTokens.Separator.DefaultThickness,
                ControlSize.Large => DesignTokens.Separator.LargeThickness,
                _ => DesignTokens.Separator.DefaultThickness,
            };

            return GetCachedStyle(
                StyleComponentType.Separator,
                variant,
                size,
                _separatorStyle ?? GUI.skin.box,
                (int)orientation,
                applySizing: false,
                customLogic: s =>
                {
                    if (orientation == SeparatorOrientation.Horizontal)
                    {
                        s.fixedHeight = Mathf.Max(1, Mathf.RoundToInt(thickness * _guiHelper.uiScale));
                        s.fixedWidth = 0;
                        s.stretchWidth = true;
                        s.stretchHeight = false;
                    }
                    else
                    {
                        s.fixedWidth = Mathf.Max(1, Mathf.RoundToInt(thickness * _guiHelper.uiScale));
                        s.fixedHeight = 0;
                        s.stretchHeight = true;
                        s.stretchWidth = false;
                        s.margin = GetSpacingOffset(DesignTokens.Spacing.None, DesignTokens.Spacing.None);
                    }
                }
            );
        }
        #endregion

        #region Style Getters - Tabs
        public GUIStyle GetTabsTriggerStyle(bool active = false, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.TabsTrigger,
                variant,
                size,
                _tabsTriggerStyle ?? GUI.skin.button,
                active ? 1 : 0,
                applyVariant: variant != ControlVariant.Default,
                customLogic: s =>
                {
                    s.alignment = TextAnchor.MiddleCenter;
                    if (active)
                    {
                        var t = ThemeManager.Instance.CurrentTheme;
                        s.normal.background = TabsActiveTexture;
                        s.normal.textColor = t.TabsTriggerActiveFg;
                        s.hover = s.active = s.focused = s.normal;
                    }
                }
            );

        public GUIStyle GetTabsContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.TabsContent, variant, size, GUIStyle.none, customLogic: s => s.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG));
        #endregion

        #region Style Getters - Checkbox & Switch
        public GUIStyle GetCheckboxStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Checkbox, variant, size, _checkboxStyle ?? GUI.skin.toggle);

        public GUIStyle GetCheckboxSolidStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.CheckboxSolid, variant, size, _checkboxSolidStyle ?? GUI.skin.toggle);

        public GUIStyle GetSwitchStyle(ControlVariant variant, ControlSize size) => GetCachedStyle(StyleComponentType.Switch, variant, size, _baseSwitchStyle ?? GUI.skin.toggle);
        #endregion

        #region Style Getters - Avatar
        public GUIStyle GetAvatarStyle(ControlSize size, AvatarShape shape, ControlVariant variant = ControlVariant.Default)
        {
            int sz = size switch
            {
                ControlSize.Small => GetScaledHeight(DesignTokens.Height.Small),
                ControlSize.Large => GetScaledHeight(DesignTokens.Height.Large),
                ControlSize.Mini => GetScaledHeight(DesignTokens.Height.Mini),
                _ => GetScaledHeight(DesignTokens.Height.Default),
            };

            int br = shape switch
            {
                AvatarShape.Circle => sz / 2,
                AvatarShape.Rounded => GetScaledBorderRadius(DesignTokens.Radius.LG),
                _ => 0,
            };

            var theme = ThemeManager.Instance.CurrentTheme;

            return GetCachedStyle(
                StyleComponentType.Avatar,
                variant,
                size,
                _avatarStyle ?? GUI.skin.box,
                (int)shape,
                applyVariant: false,
                applySizing: false,
                customLogic: s =>
                {
                    s.fixedWidth = s.fixedHeight = sz;
                    s.border = new UnityHelpers.RectOffset(br, br, br, br);
                    s.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    s.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    s.alignment = TextAnchor.MiddleCenter;

                    Color bgTop = Color.Lerp(theme.Elevated, Color.white, 0.03f);

                    s.normal.background = CreateAvatarTexture(sz, br, bgTop, theme.Border, 1.5f, true);
                    s.normal.textColor = theme.Text;
                    s.fontSize = GetScaledFontSize(DesignTokens.Avatar.FallbackFontScale);
                    s.fontStyle = FontStyle.Bold;

                    s.hover = s.active = s.focused = s.normal;
                }
            );
        }

        public float GetStatusIndicatorSize(ControlSize size) =>
            size switch
            {
                ControlSize.Small => DesignTokens.StatusIndicator.Small * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.StatusIndicator.Large * _guiHelper.uiScale,
                ControlSize.Mini => DesignTokens.StatusIndicator.Mini * _guiHelper.uiScale,
                _ => DesignTokens.StatusIndicator.Default * _guiHelper.uiScale,
            };
        #endregion

        #region Style Getters - Table
        public GUIStyle GetTableStyle(ControlVariant variant, ControlSize size) =>
            GetCachedStyle(
                StyleComponentType.Table,
                variant,
                size,
                _baseTableStyle ?? GUI.skin.box,
                customLogic: variant switch
                {
                    ControlVariant.Secondary => s =>
                    {
                        var t = ThemeManager.Instance.CurrentTheme;
                        float lum = 0.299f * t.Base.r + 0.587f * t.Base.g + 0.114f * t.Base.b;
                        Color tableBg = lum < 0.3f ? Color.Lerp(t.Base, t.Secondary, 0.2f) : Color.Lerp(t.Base, t.Secondary, 0.15f);
                        s.normal.background = CreateSolidTexture(tableBg);
                    },
                    ControlVariant.Outline => s => s.normal.background = CreateBorderTexture(ThemeManager.Instance.CurrentTheme.Border, 1),
                    ControlVariant.Ghost => s =>
                    {
                        var t = ThemeManager.Instance.CurrentTheme;
                        float lum = 0.299f * t.Base.r + 0.587f * t.Base.g + 0.114f * t.Base.b;
                        Color hoverBg = lum < 0.3f ? Color.Lerp(t.Base, t.Accent, 0.12f) : Color.Lerp(t.Base, t.Accent, 0.08f);
                        var h = CreateSolidTexture(hoverBg);
                        s.hover.background = s.active.background = h;
                    },
                    _ => null,
                }
            );

        public GUIStyle GetTableRowStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.TableRow,
                variant,
                size,
                _tableRowStyle ?? GUIStyle.none,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    float lum = 0.299f * t.Base.r + 0.587f * t.Base.g + 0.114f * t.Base.b;
                    Color borderColor = lum < 0.3f ? Color.Lerp(t.Border, Color.white, 0.1f) : Color.Lerp(t.Border, Color.clear, 0.3f);
                    Color hoverBg = lum < 0.3f ? Color.Lerp(t.Base, t.Accent, 0.08f) : Color.Lerp(t.Base, t.Accent, 0.05f);

                    s.normal.background = CreateBorderTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, 1, borderColor, Color.clear);
                    var hover = CreateBorderTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, 1, t.Border, hoverBg);
                    s.hover.background = s.active.background = s.onNormal.background = hover;
                    s.fixedHeight = 0;
                    s.stretchHeight = false;
                }
            );

        public GUIStyle GetTableHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.TableHeader,
                variant,
                size,
                _tableHeaderStyle ?? GUI.skin.label,
                applyVariant: false,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    float lum = 0.299f * t.Base.r + 0.587f * t.Base.g + 0.114f * t.Base.b;
                    Color headerBg = lum < 0.3f ? Color.Lerp(t.Secondary, t.Text, 0.08f) : t.Secondary;
                    Color hoverBg = lum < 0.3f ? Color.Lerp(t.Secondary, t.Text, 0.12f) : Color.Lerp(t.Secondary, t.Text, 0.08f);

                    s.fontStyle = FontStyle.Bold;
                    s.normal.background = CreateBorderTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, 2, t.Border, headerBg);
                    s.hover.background = CreateBorderTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, 2, t.Accent, hoverBg);
                    s.normal.textColor = s.hover.textColor = t.Text;
                }
            );

        public GUIStyle GetTableCellStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, TextAnchor alignment = TextAnchor.MiddleLeft) =>
            GetCachedStyle(StyleComponentType.TableCell, variant, size, _tableCellStyle ?? GUI.skin.label, (int)alignment, applyVariant: false, customLogic: s => s.alignment = alignment);
        #endregion

        #region Style Getters - Menu & Select
        public GUIStyle GetDropdownMenuItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.DropdownMenuItem, variant, size, _dropdownItemStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetSelectItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.SelectItem, variant, size, _dropdownItemStyle ?? GUI.skin.button, applyVariant: variant != ControlVariant.Default);

        public GUIStyle GetMenuBarItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool isShortcut = false, bool active = false) =>
            GetCachedStyle(
                StyleComponentType.MenuBarItem,
                variant,
                size,
                _dropdownItemStyle ?? GUI.skin.button,
                (isShortcut ? 1 : 0) | (active ? 2 : 0),
                applyVariant: variant != ControlVariant.Default,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;

                    if (isShortcut)
                    {
                        s.alignment = TextAnchor.MiddleRight;
                        s.normal.textColor = Color.Lerp(t.Muted, t.Text, 0.5f);
                    }
                    else
                    {
                        s.alignment = TextAnchor.MiddleLeft;
                        s.normal.textColor = t.Text;
                        s.normal.background = TransparentTexture;
                        s.focused.background = TransparentTexture;

                        var hoverBg = CreateTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Small, (int)DesignTokens.Radius.SM, Color.Lerp(t.Accent, Color.white, 0.08f));

                        s.hover.background = hoverBg;
                        s.hover.textColor = t.Text;
                        s.active.background = CreateTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Small, (int)DesignTokens.Radius.SM, Color.Lerp(t.Accent, Color.black, 0.1f));
                        s.active.textColor = t.Text;
                        s.onNormal.background = hoverBg;

                        if (active)
                            s.normal.background = hoverBg;
                    }
                }
            );

        public GUIStyle GetMenuDropdownStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.MenuDropdown,
                variant,
                size,
                _dropdownContentStyle ?? GUI.skin.box,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    Color dropdownTop = Color.Lerp(t.Elevated, Color.white, 0.015f);
                    Color dropdownBottom = Color.Lerp(t.Elevated, Color.black, 0.025f);

                    s.normal.background = CreateTexture(DesignTokens.TextureSize.Large, DesignTokens.TextureSize.Large, (int)DesignTokens.Radius.MD, dropdownTop, dropdownBottom, 0.16f, 8);
                    s.padding = GetSpacingOffset(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS);
                    s.fixedWidth = DesignTokens.TextureSize.Large;
                    s.stretchWidth = false;
                }
            );

        public GUIStyle GetNavigationStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) => GetCachedStyle(StyleComponentType.Navigation, variant, size, _navigationStyle ?? GUI.skin.box);

        public GUIStyle GetPopoverContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.Popover, variant, size, _dropdownContentStyle ?? GUI.skin.box, customLogic: s => s.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG));
        #endregion

        #region Style Getters - Labels & Card
        public GUIStyle GetChartAxisStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetLabelBasedStyle(
                StyleComponentType.ChartAxis,
                DesignTokens.Chart.AxisFontScale,
                (s, t) =>
                {
                    s.alignment = TextAnchor.MiddleCenter;
                    s.normal.textColor = t.Muted;
                    s.wordWrap = true;
                },
                variant,
                size
            );

        public GUIStyle GetTooltipStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.Tooltip,
                variant,
                size,
                _chartContainerStyle ?? GUI.skin.box,
                1,
                applyVariant: false,
                applySizing: false,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    float lum = 0.299f * t.Base.r + 0.587f * t.Base.g + 0.114f * t.Base.b;
                    Color tooltipBg = lum < 0.3f ? Color.Lerp(t.Elevated, Color.white, 0.08f) : Color.Lerp(t.Elevated, Color.black, 0.04f);
                    s.normal.background = CreateTexture(DesignTokens.TextureSize.Small, DesignTokens.TextureSize.Small, (int)DesignTokens.Radius.MD, tooltipBg);
                    s.normal.textColor = t.Text;
                    s.padding = GetSpacingOffset(DesignTokens.Spacing.MD, DesignTokens.Spacing.SM);
                    s.border = new UnityHelpers.RectOffset((int)DesignTokens.Radius.MD, (int)DesignTokens.Radius.MD, (int)DesignTokens.Radius.MD, (int)DesignTokens.Radius.MD);
                }
            );

        public GUIStyle GetSectionHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetLabelBasedStyle(
                StyleComponentType.SectionHeader,
                DesignTokens.FontScale.LG,
                (s, t) =>
                {
                    s.fontStyle = FontStyle.Bold;
                    s.normal.textColor = t.Text;
                },
                variant,
                size
            );

        public GUIStyle GetCardHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.CardHeader, variant, size, GUIStyle.none, customLogic: s => s.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.MD));

        public GUIStyle GetCardContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(StyleComponentType.CardContent, variant, size, GUIStyle.none, customLogic: s => s.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.SM));

        public GUIStyle GetCardFooterStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.CardFooter,
                variant,
                size,
                GUIStyle.none,
                customLogic: s =>
                {
                    s.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.MD);
                    s.alignment = TextAnchor.MiddleLeft;
                }
            );

        public GUIStyle GetCardTitleStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.CardTitle,
                variant,
                size,
                _baseLabelStyle ?? GUI.skin.label,
                applyVariant: false,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    s.fontSize = GetScaledFontSize(DesignTokens.FontScale.XL);
                    s.fontStyle = FontStyle.Bold;
                    s.alignment = TextAnchor.UpperLeft;
                    s.normal.textColor = t.Text;
                    s.wordWrap = true;
                    s.margin = new UnityHelpers.RectOffset(0, 0, 0, (int)DesignTokens.Spacing.XS);
                }
            );

        public GUIStyle GetCardDescriptionStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default) =>
            GetCachedStyle(
                StyleComponentType.CardDescription,
                variant,
                size,
                _baseLabelStyle ?? GUI.skin.label,
                applyVariant: false,
                customLogic: s =>
                {
                    var t = ThemeManager.Instance.CurrentTheme;
                    s.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    s.alignment = TextAnchor.UpperLeft;
                    s.normal.textColor = t.Muted;
                    s.wordWrap = true;
                }
            );
        #endregion

        #region Size Helpers - Slider
        public float GetSliderTrackHeight(ControlSize size)
        {
            float baseHeight = size switch
            {
                ControlSize.Mini => DesignTokens.Slider.TrackMini,
                ControlSize.Small => DesignTokens.Slider.TrackSmall,
                ControlSize.Large => DesignTokens.Slider.TrackLarge,
                _ => DesignTokens.Slider.TrackDefault,
            };
            return baseHeight * _guiHelper.uiScale;
        }

        public float GetSliderThumbSize(ControlSize size)
        {
            float baseSize = size switch
            {
                ControlSize.Mini => DesignTokens.Slider.ThumbMini,
                ControlSize.Small => DesignTokens.Slider.ThumbSmall,
                ControlSize.Large => DesignTokens.Slider.ThumbLarge,
                _ => DesignTokens.Slider.ThumbDefault,
            };
            return baseSize * _guiHelper.uiScale;
        }
        #endregion

        #region Color Helpers - Slider
        public Color GetSliderTrackColor(ControlVariant variant, bool disabled)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            Color baseColor = theme.Secondary;

            if (disabled)
                return Color.Lerp(baseColor, theme.Muted, 0.5f);

            return baseColor;
        }

        public Color GetSliderFillColor(ControlVariant variant, bool disabled)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            Color baseColor = variant switch
            {
                ControlVariant.Destructive => theme.Destructive,
                ControlVariant.Secondary => Color.Lerp(theme.Secondary, theme.Text, 0.3f),
                ControlVariant.Muted => theme.Muted,
                _ => theme.Accent,
            };

            if (disabled)
                return Color.Lerp(baseColor, theme.Muted, 0.6f);

            return baseColor;
        }

        public Color GetSliderThumbColor(ControlVariant variant, bool disabled)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            float baseLuminance = 0.299f * theme.Base.r + 0.587f * theme.Base.g + 0.114f * theme.Base.b;

            Color baseColor;
            if (baseLuminance < 0.3f)
                baseColor = Color.Lerp(theme.Elevated, Color.white, 0.1f);
            else
                baseColor = theme.Elevated;

            if (disabled)
                return Color.Lerp(baseColor, theme.Muted, 0.4f);

            return baseColor;
        }
        #endregion

        #region Color Helpers - Toast
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
        #endregion
    }
}
