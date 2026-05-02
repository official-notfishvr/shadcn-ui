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

            _baseButtonStyle = MakeControlStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Radius.MD, Color.clear, FontStyle.Normal);
            _baseToggleStyle = MakeControlStyle(theme.Secondary, theme.Text, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Radius.MD, Color.clear, FontStyle.Normal);
            _baseInputStyle = MakeInputStyle(theme.Elevated, theme.Text, theme.Border);
            _baseLabelStyle = MakeLabelStyle(theme.Text);
            _baseBadgeStyle = MakeChipStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg);
            _baseTableStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.LG, 0f, 0f);
            _checkboxStyle = MakeControlStyle(theme.Base, theme.Text, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Radius.SM, theme.Border, FontStyle.Normal);
            _checkboxSolidStyle = MakeControlStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, DesignTokens.Spacing.MD, DesignTokens.Spacing.XS, DesignTokens.Radius.SM, Color.clear, FontStyle.Normal);
            _baseSwitchStyle = MakeControlStyle(theme.Secondary, theme.Text, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Switch.Radius, Color.clear, FontStyle.Normal);
            ApplyCheckedState(_baseToggleStyle, theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, Color.clear);
            ApplyCheckedState(_checkboxStyle, theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, Color.clear);
            ApplyCheckedState(_checkboxSolidStyle, theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, Color.clear);
            ApplyCheckedState(_baseSwitchStyle, theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, Color.clear);
            _progressBarStyle = CloneStyle(GUI.skin.box);
            var progressHeight = Mathf.Max(1, DesignTokens.ProgressBar.TextureHeight);
            var progressRadius = Mathf.Max(1, progressHeight / 2);
            var progressTexture = CreateSurfaceTexture(DesignTokens.TextureSize.Default, progressHeight, DesignTokens.Radius.Full, theme.Secondary, Color.clear, 0f);
            SetBackgroundStates(_progressBarStyle, progressTexture, progressTexture, progressTexture, progressTexture);
            _progressBarStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            _progressBarStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            _progressBarStyle.border = CreateBorderSlice(progressRadius, DesignTokens.TextureSize.Default, progressHeight);
            _progressBarStyle.fixedHeight = progressHeight;
            _progressBarStyle.stretchHeight = false;
            _progressBarStyle.stretchWidth = true;
            _separatorStyle = new UnityHelpers.GUIStyle(GUI.skin.box)
            {
                normal = { background = Textures.Separator },
                margin = new RectOffset(),
                padding = new RectOffset(),
                border = new RectOffset(),
            };
            _tabsListStyle = MakePanelStyle(theme.TabsBg, Color.clear, DesignTokens.Radius.MD, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS);
            _tabsTriggerStyle = MakeControlStyle(Color.clear, theme.TabsTriggerFg, DesignTokens.Padding.Tab.Horizontal, DesignTokens.Padding.Tab.Vertical, DesignTokens.Radius.MD, Color.clear, FontStyle.Normal);
            _tableHeaderStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _tableHeaderStyle.normal.background = Textures.TableHeader;
            _tableHeaderStyle.padding = GetSpacingOffset(DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
            _tableHeaderStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 1);
            _tableRowStyle = CloneStyle(GUIStyle.none);
            SetBackgroundStates(_tableRowStyle, Textures.TableRow, Textures.TableRowAlternate, Textures.TableRowAlternate, Textures.TableRow);
            _tableRowStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            _tableRowStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            _tableRowStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
            _tableRowStyle.stretchWidth = true;
            _tableCellStyle = MakeLabelStyle(theme.Text);
            _tableCellStyle.padding = GetSpacingOffset(DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
            _dialogContentStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.LG, DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical, DesignTokens.Effects.ShadowMedium, GetScaledBlur(DesignTokens.Effects.ShadowBlurLG));
            _cardStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.LG, 0f, 0f, DesignTokens.Effects.ShadowLight, GetScaledBlur(DesignTokens.Effects.ShadowBlurMD));
            _dropdownContentStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS, DesignTokens.Effects.ShadowMedium, GetScaledBlur(DesignTokens.Effects.ShadowBlurMD));
            _dropdownItemStyle = MakeControlStyle(Color.clear, theme.Text, DesignTokens.Padding.Dropdown.ItemH, DesignTokens.Padding.Dropdown.ItemV, DesignTokens.Radius.SM, Color.clear, FontStyle.Normal);
            _menuBarStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.MD, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS);
            _chartContainerStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Chart.Radius, DesignTokens.Chart.ContainerPaddingH, DesignTokens.Chart.ContainerPaddingV);
            _avatarStyle = MakeAvatarStyle(theme);
            _navigationStyle = MakePanelStyle(theme.Secondary, theme.Border, DesignTokens.Radius.LG, DesignTokens.Spacing.MD, DesignTokens.Spacing.MD);
            _calendarStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.LG, DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical, DesignTokens.Effects.ShadowMedium, GetScaledBlur(DesignTokens.Effects.ShadowBlurMD));
            _calendarWeekdayStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _calendarWeekdayStyle.alignment = TextAnchor.MiddleCenter;
            _calendarWeekdayStyle.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
            _calendarDayStyle = MakeCalendarDayStyle(Color.clear, theme.Text, Color.clear);
            _calendarDaySelectedStyle = MakeCalendarDayStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, Color.clear);
            _calendarDayInRangeStyle = MakeCalendarDayStyle(Tint(theme.ButtonPrimaryBg, 0.18f), theme.Text, Color.clear);
            _calendarDayTodayStyle = MakeCalendarDayStyle(theme.Secondary, theme.Text, theme.Border);
            _calendarDayOutsideMonthStyle = MakeCalendarDayStyle(Color.clear, theme.Muted, Color.clear);
            _datePickerStyle = MakePanelStyle(theme.Elevated, theme.Border, DesignTokens.Radius.LG, DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical, DesignTokens.Effects.ShadowMedium, GetScaledBlur(DesignTokens.Effects.ShadowBlurMD));
            _datePickerWeekdayStyle = MakeLabelStyle(theme.Muted, FontStyle.Bold);
            _datePickerWeekdayStyle.alignment = TextAnchor.MiddleCenter;
            _datePickerWeekdayStyle.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
            _datePickerDayStyle = MakeCalendarDayStyle(Color.clear, theme.Text, Color.clear);
            _datePickerDaySelectedStyle = MakeCalendarDayStyle(theme.ButtonPrimaryBg, theme.ButtonPrimaryFg, Color.clear);
            _datePickerDayTodayStyle = MakeCalendarDayStyle(theme.Secondary, theme.Text, theme.Border);
            _datePickerDayOutsideMonthStyle = MakeCalendarDayStyle(Color.clear, theme.Muted, Color.clear);
            AnimatedBoxStyle = CloneStyle(_cardStyle);
            AnimatedBoxStyle.padding = GetSpacingOffset(DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical);
            AnimatedBoxStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            AnimatedBoxStyle.normal.textColor = theme.Text;
        }

        private GUIStyle MakeControlStyle(Color background, Color text, float paddingH, float paddingV, float radius, Color border, FontStyle fontStyle)
        {
            var style = CloneStyle(GUI.skin.button);
            var radiusPx = GetScaledBorderRadius(radius);
            var height = GetScaledHeight(DesignTokens.Height.Default);
            var width = DesignTokens.TextureSize.Default;
            var borderThickness = border.a > 0f ? 1f : 0f;
            var normalFill = background.a > 0f ? background : Color.clear;
            var hoverFill = background.a > 0f ? HoverSurface(background) : GetGhostFill(0.06f);
            var activeFill = background.a > 0f ? ActiveSurface(background) : GetGhostFill(0.1f);
            var hoverBorder = border.a > 0f ? HoverSurface(border, 0.025f) : Color.clear;
            var activeBorder = border.a > 0f ? ActiveSurface(border, 0.04f) : Color.clear;

            SetBackgroundStates(
                style,
                CreateSurfaceTexture(width, height, radius, normalFill, border, borderThickness),
                CreateSurfaceTexture(width, height, radius, hoverFill, hoverBorder, borderThickness),
                CreateSurfaceTexture(width, height, radius, activeFill, activeBorder, borderThickness),
                CreateSurfaceTexture(width, height, radius, hoverFill, hoverBorder, borderThickness)
            );

            SetTextStates(style, text);
            style.padding = GetSpacingOffset(paddingH, paddingV);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = CreateBorderSlice(radiusPx, width, height);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
            style.fontStyle = fontStyle;
            style.wordWrap = false;
            style.richText = false;
            style.clipping = TextClipping.Clip;
            style.stretchWidth = false;
            style.stretchHeight = false;
            return style;
        }

        private GUIStyle MakeInputStyle(Color background, Color text, Color border)
        {
            var style = CloneStyle(GUI.skin.textField);
            var radiusPx = GetScaledBorderRadius(DesignTokens.Radius.MD);
            var height = GetScaledHeight(DesignTokens.Height.Default);
            var width = DesignTokens.TextureSize.Default;

            SetBackgroundStates(
                style,
                CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, background, border, 1f),
                CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, background, HoverSurface(border, 0.04f), 1f),
                CreateFocusTexture(width, height, DesignTokens.Radius.MD, background),
                CreateFocusTexture(width, height, DesignTokens.Radius.MD, background)
            );

            SetTextStates(style, text);
            style.padding = GetSpacingOffset(DesignTokens.Padding.Input.Horizontal, DesignTokens.Padding.Input.Vertical);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = CreateBorderSlice(radiusPx, width, height);
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
            style.alignment = TextAnchor.MiddleLeft;
            style.wordWrap = false;
            style.stretchWidth = true;
            style.stretchHeight = false;
            return style;
        }

        private GUIStyle MakeLabelStyle(Color color, FontStyle fontStyle = FontStyle.Normal)
        {
            var style = CloneStyle(GUI.skin.label);
            style.normal.background = null;
            style.normal.textColor = color;
            style.hover.textColor = color;
            style.active.textColor = color;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
            style.fontStyle = fontStyle;
            style.wordWrap = true;
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            return style;
        }

        private GUIStyle MakePanelStyle(Color fill, Color border, float radius, float paddingH, float paddingV, float shadowAlpha = 0f, int shadowBlur = 0)
        {
            var style = CloneStyle(GUI.skin.box);
            var radiusPx = GetScaledBorderRadius(radius);
            var texture = CreateSurfaceTexture(DesignTokens.TextureSize.Large, DesignTokens.TextureSize.Large, radius, fill, border, border.a > 0f ? 1f : 0f, shadowAlpha, shadowBlur, GetTheme().Shadow);

            SetBackgroundStates(style, texture, texture, texture, texture);
            SetTextStates(style, GetTheme().Text);
            style.padding = GetSpacingOffset(paddingH, paddingV);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = CreateBorderSlice(radiusPx, DesignTokens.TextureSize.Large, DesignTokens.TextureSize.Large);
            style.alignment = TextAnchor.UpperLeft;
            style.stretchWidth = true;
            style.stretchHeight = true;
            return style;
        }

        private void ApplyCheckedState(GUIStyle style, Color fill, Color text, Color border)
        {
            var height = GetTextureHeight(style, DesignTokens.Height.Default);
            var width = GetTextureWidth(style, DesignTokens.TextureSize.Default);
            var radius = GetRadiusFromStyle(style, DesignTokens.Radius.MD);
            var borderThickness = border.a > 0f ? 1f : 0f;

            style.onNormal.background = CreateSurfaceTexture(width, height, radius, fill, border, borderThickness);
            style.onHover.background = CreateSurfaceTexture(width, height, radius, HoverSurface(fill), border.a > 0f ? HoverSurface(border, 0.025f) : border, borderThickness);
            style.onActive.background = CreateSurfaceTexture(width, height, radius, ActiveSurface(fill), border.a > 0f ? ActiveSurface(border, 0.04f) : border, borderThickness);
            style.onFocused.background = CreateSurfaceTexture(width, height, radius, HoverSurface(fill), border.a > 0f ? HoverSurface(border, 0.025f) : border, borderThickness);
            style.onNormal.textColor = style.onHover.textColor = style.onActive.textColor = style.onFocused.textColor = text;
        }

        private GUIStyle MakeChipStyle(Color fill, Color text)
        {
            var style = MakeControlStyle(fill, text, DesignTokens.Padding.Badge.Horizontal, DesignTokens.Padding.Badge.Vertical, DesignTokens.Radius.MD, Color.clear, FontStyle.Normal);
            style.fixedHeight = GetScaledHeight(DesignTokens.Badge.Height);
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
            return style;
        }

        private GUIStyle MakeAvatarStyle(Theme theme)
        {
            var size = GetScaledHeight(DesignTokens.Height.Default);
            var style = CloneStyle(GUI.skin.box);
            style.normal.background = CreateAvatarTexture(size, size / 2, theme.Secondary, theme.Border, DesignTokens.Avatar.BorderThickness, false);
            style.hover.background = style.normal.background;
            style.active.background = style.normal.background;
            style.focused.background = style.normal.background;
            SetTextStates(style, theme.Text);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = GetScaledFontSize(DesignTokens.Avatar.FallbackFontScale);
            style.padding = new UnityHelpers.RectOffset();
            style.margin = new UnityHelpers.RectOffset();
            style.border = CreateBorderSlice(size / 2, size, size);
            style.fixedWidth = size;
            style.fixedHeight = size;
            return style;
        }

        private GUIStyle MakeCalendarDayStyle(Color fill, Color text, Color border)
        {
            var style = CloneStyle(GUI.skin.button);
            var radius = DesignTokens.Radius.MD;
            var width = 36;
            var height = GetScaledHeight(DesignTokens.Height.Small);
            var borderThickness = border.a > 0f ? 1f : 0f;

            SetBackgroundStates(
                style,
                CreateSurfaceTexture(width, height, radius, fill, border, borderThickness),
                CreateSurfaceTexture(width, height, radius, fill.a > 0f ? HoverSurface(fill) : GetGhostFill(0.075f), border.a > 0f ? HoverSurface(border, 0.025f) : border, borderThickness),
                CreateSurfaceTexture(width, height, radius, fill.a > 0f ? ActiveSurface(fill) : GetGhostFill(0.11f), border.a > 0f ? ActiveSurface(border, 0.04f) : border, borderThickness),
                CreateSurfaceTexture(width, height, radius, fill.a > 0f ? HoverSurface(fill) : GetGhostFill(0.075f), border.a > 0f ? HoverSurface(border, 0.025f) : border, borderThickness)
            );

            SetTextStates(style, text);
            style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.border = CreateBorderSlice(GetScaledBorderRadius(radius), width, height);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
            return style;
        }

        private Texture2D CreateSurfaceTexture(int width, int height, float radius, Color fill, Color border, float borderThickness = 1f, float shadowAlpha = 0f, int shadowBlur = 0, Color shadowColor = default)
        {
            var radiusPx = GetScaledBorderRadius(radius);
            var effectiveBorder = border.a > 0f ? border : Color.clear;
            var effectiveBorderThickness = effectiveBorder.a > 0f ? borderThickness : 0f;
            return effectiveBorderThickness > 0f ? CreateBorderTexture(width, height, radiusPx, fill, effectiveBorder, effectiveBorderThickness, shadowAlpha, shadowBlur, shadowColor) : CreateTexture(width, height, radiusPx, fill, shadowAlpha, shadowBlur, shadowColor);
        }

        private Texture2D CreateFocusTexture(int width, int height, float radius, Color fill)
        {
            var theme = GetTheme();
            var focusShadow = new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.9f);
            return CreateSurfaceTexture(width, height, radius, fill, theme.Accent, DesignTokens.Effects.FocusRingThickness, DesignTokens.Effects.FocusRingAlpha, GetScaledBlur(DesignTokens.Effects.FocusRingBlur), focusShadow);
        }

        private int GetTextureWidth(GUIStyle style, int fallback)
        {
            return Mathf.Max(1, Mathf.RoundToInt(style?.fixedWidth > 0f ? style.fixedWidth : fallback));
        }

        private int GetTextureHeight(GUIStyle style, float fallback)
        {
            var sourceHeight = style != null && style.fixedHeight > 0f ? style.fixedHeight : GetScaledHeight(fallback);
            return Mathf.Max(1, Mathf.RoundToInt(sourceHeight));
        }

        private float GetRadiusFromStyle(GUIStyle style, float fallback)
        {
            if (style?.border != null && style.border.left > 0)
                return style.border.left / Mathf.Max(0.0001f, _guiHelper.uiScale);
            return fallback;
        }

        private RectOffset CreateBorderSlice(int radiusPx, int textureWidth = 0, int textureHeight = 0)
        {
            var maxSlice = textureWidth > 0 && textureHeight > 0 ? Mathf.Max(1, Mathf.Min(textureWidth, textureHeight) / 2) : Mathf.Max(1, GetScaledHeight(DesignTokens.Height.Small) / 2);
            var slice = Mathf.Clamp(radiusPx, 1, maxSlice);
            return new UnityHelpers.RectOffset(slice, slice, slice, slice);
        }

        private int GetScaledBlur(float blur) => Mathf.Max(0, Mathf.RoundToInt(blur * _guiHelper.uiScale));

        private void SetBackgroundStates(GUIStyle style, Texture2D normal, Texture2D hover, Texture2D active, Texture2D focused)
        {
            style.normal.background = normal;
            style.hover.background = hover ?? normal;
            style.active.background = active ?? hover ?? normal;
            style.focused.background = focused ?? hover ?? normal;
            style.onNormal.background = normal;
            style.onHover.background = hover ?? normal;
            style.onActive.background = active ?? hover ?? normal;
            style.onFocused.background = focused ?? hover ?? normal;
        }

        private void SetOffBackgroundStates(GUIStyle style, Texture2D normal, Texture2D hover, Texture2D active, Texture2D focused)
        {
            style.normal.background = normal;
            style.hover.background = hover ?? normal;
            style.active.background = active ?? hover ?? normal;
            style.focused.background = focused ?? hover ?? normal;
        }

        private void SetTextStates(GUIStyle style, Color normal, Color hover = default, Color active = default, Color focused = default)
        {
            var hoverText = hover == default ? normal : hover;
            var activeText = active == default ? normal : active;
            var focusedText = focused == default ? hoverText : focused;

            style.normal.textColor = normal;
            style.hover.textColor = hoverText;
            style.active.textColor = activeText;
            style.focused.textColor = focusedText;
            style.onNormal.textColor = normal;
            style.onHover.textColor = hoverText;
            style.onActive.textColor = activeText;
            style.onFocused.textColor = focusedText;
        }

        private void SetOffTextStates(GUIStyle style, Color normal, Color hover = default, Color active = default, Color focused = default)
        {
            var hoverText = hover == default ? normal : hover;
            var activeText = active == default ? normal : active;
            var focusedText = focused == default ? hoverText : focused;

            style.normal.textColor = normal;
            style.hover.textColor = hoverText;
            style.active.textColor = activeText;
            style.focused.textColor = focusedText;
        }

        private Color HoverSurface(Color color, float amount = 0.08f)
        {
            return Color.Lerp(color, GetTheme().Base, Mathf.Clamp01(amount));
        }

        private Color ActiveSurface(Color color, float amount = 0.16f)
        {
            return Color.Lerp(color, GetTheme().Base, Mathf.Clamp01(amount));
        }

        private Color GetGhostFill(float alpha)
        {
            var theme = GetTheme();
            var mix = Mathf.Clamp01(alpha / 0.1f);
            return Color.Lerp(theme.Base, theme.Secondary, mix);
        }

        private Color Lift(Color color, float amount) => Color.Lerp(color, Color.white, amount);

        private Color Lower(Color color, float amount) => Color.Lerp(color, Color.black, amount);

        private Color Tint(Color color, float alpha) => new(color.r, color.g, color.b, alpha);
    }
}
