using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public partial class StyleManager
    {
        private GUIStyle GetCachedStyle(StyleComponentType type, ControlVariant variant, ControlSize size, GUIStyle baseStyle, int state = 0, string styleId = null, System.Action<GUIStyle> customize = null)
        {
            var key = new StyleKey(type, variant, size, state, styleId);
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

            ApplyNamedAppearance(style, type, styleId, state);

            _styleCache[key] = style;
            return style;
        }

        private GUIStyle ResolveStyle(StyleComponentType type, ControlVariant variant, ControlSize size, GUIStyle baseStyle, ComponentAppearance appearance = null, int state = 0, System.Action<GUIStyle> customize = null)
        {
            var resolvedAppearance = ResolveAppearance(type, appearance);
            var cacheableAppearance = resolvedAppearance != null && !resolvedAppearance.IsInlineOverride ? resolvedAppearance : null;
            var style = GetCachedStyle(type, variant, size, baseStyle, state, cacheableAppearance?.StyleId, customize);

            if (resolvedAppearance?.IsInlineOverride == true)
            {
                style = CloneStyle(style);
                ApplyAppearance(style, resolvedAppearance, state);
            }

            return style;
        }

        private ComponentAppearance ResolveAppearance(StyleComponentType type, ComponentAppearance appearance)
        {
            if (appearance == null)
                return null;

            if (string.IsNullOrWhiteSpace(appearance.StyleId))
                return appearance;

            if (Registry.GetStyle(type, appearance.StyleId) is not { } profile)
                return appearance;

            return new ComponentAppearance
            {
                StyleId = appearance.StyleId,
                TemplateStyle = profile.TemplateStyle ?? appearance.TemplateStyle,
                ReplaceBaseStyle = profile.ReplaceBaseStyle || appearance.ReplaceBaseStyle,
                BackgroundColor = appearance.BackgroundColor ?? profile.BackgroundColor,
                HoverBackgroundColor = appearance.HoverBackgroundColor ?? profile.HoverBackgroundColor,
                ActiveBackgroundColor = appearance.ActiveBackgroundColor ?? profile.ActiveBackgroundColor,
                FocusedBackgroundColor = appearance.FocusedBackgroundColor ?? profile.FocusedBackgroundColor,
                ForegroundColor = appearance.ForegroundColor ?? profile.ForegroundColor,
                HoverForegroundColor = appearance.HoverForegroundColor ?? profile.HoverForegroundColor,
                ActiveForegroundColor = appearance.ActiveForegroundColor ?? profile.ActiveForegroundColor,
                FocusedForegroundColor = appearance.FocusedForegroundColor ?? profile.FocusedForegroundColor,
                BorderColor = appearance.BorderColor ?? profile.BorderColor,
                HoverBorderColor = appearance.HoverBorderColor ?? profile.HoverBorderColor,
                ActiveBorderColor = appearance.ActiveBorderColor ?? profile.ActiveBorderColor,
                AccentColor = appearance.AccentColor ?? profile.AccentColor,
                BorderRadius = appearance.BorderRadius ?? profile.BorderRadius,
                BorderThickness = appearance.BorderThickness ?? profile.BorderThickness,
                Modifier = MergeModifiers(profile.Modifier, appearance.Modifier),
            };
        }

        private StatefulStyleModifier MergeModifiers(StatefulStyleModifier first, StatefulStyleModifier second)
        {
            if (first == null)
                return second;
            if (second == null)
                return first;

            return (style, theme, helper, state) =>
            {
                first(style, theme, helper, state);
                second(style, theme, helper, state);
            };
        }

        private void ApplyNamedAppearance(GUIStyle style, StyleComponentType type, string styleId, int state)
        {
            if (string.IsNullOrWhiteSpace(styleId))
                return;

            if (Registry.GetStyle(type, styleId) is not { } appearance)
                return;

            ApplyAppearance(style, appearance, state);
        }

        private void ApplyAppearance(GUIStyle style, ComponentAppearance appearance, int state)
        {
            if (style == null || appearance == null)
                return;

            if (appearance.TemplateStyle != null)
            {
                if (appearance.ReplaceBaseStyle)
                    CopyStyle(style, appearance.TemplateStyle);
                else
                    MergeStyle(style, appearance.TemplateStyle);
            }

            ApplyAppearanceColors(style, appearance);
            appearance.Modifier?.Invoke(style, GetTheme(), _guiHelper, state);
        }

        private void ApplyAppearanceColors(GUIStyle style, ComponentAppearance appearance)
        {
            bool hasFill = appearance.BackgroundColor.HasValue || appearance.HoverBackgroundColor.HasValue || appearance.ActiveBackgroundColor.HasValue || appearance.FocusedBackgroundColor.HasValue;
            bool hasText = appearance.ForegroundColor.HasValue || appearance.HoverForegroundColor.HasValue || appearance.ActiveForegroundColor.HasValue || appearance.FocusedForegroundColor.HasValue;
            bool hasBorder = appearance.BorderColor.HasValue || appearance.HoverBorderColor.HasValue || appearance.ActiveBorderColor.HasValue;
            bool hasRadius = appearance.BorderRadius.HasValue;
            float borderThickness = appearance.BorderThickness ?? 1f;

            if (!hasFill && !hasBorder && !hasText)
                return;

            if (hasFill || hasBorder)
            {
                var fill = appearance.BackgroundColor ?? GetTheme().Base;
                var hoverFill = appearance.HoverBackgroundColor ?? HoverSurface(fill);
                var activeFill = appearance.ActiveBackgroundColor ?? ActiveSurface(fill);
                var focusedFill = appearance.FocusedBackgroundColor ?? hoverFill;
                var borderColor = appearance.BorderColor ?? Color.clear;
                var hoverBorder = appearance.HoverBorderColor ?? borderColor;
                var activeBorder = appearance.ActiveBorderColor ?? hoverBorder;
                var effectiveThickness = hasBorder ? borderThickness : 0f;
                var radius = hasRadius ? appearance.BorderRadius.Value : DesignTokens.Radius.MD;
                var textureWidth = GetTextureWidth(style, DesignTokens.TextureSize.Large);
                var textureHeight = GetTextureHeight(style, style.fixedHeight > 0f ? style.fixedHeight / Mathf.Max(0.0001f, _guiHelper.uiScale) : DesignTokens.TextureSize.Large);

                var normalBg = CreateSurfaceTexture(textureWidth, textureHeight, radius, fill, borderColor, effectiveThickness);
                var hoverBg = CreateSurfaceTexture(textureWidth, textureHeight, radius, hoverFill, hoverBorder, effectiveThickness);
                var activeBg = CreateSurfaceTexture(textureWidth, textureHeight, radius, activeFill, activeBorder, effectiveThickness);
                var focusedBg = CreateSurfaceTexture(textureWidth, textureHeight, radius, focusedFill, hoverBorder, effectiveThickness);

                SetBackgroundStates(style, normalBg, hoverBg, activeBg, focusedBg);
                style.border = CreateBorderSlice(GetScaledBorderRadius(radius), textureWidth, textureHeight);
            }

            if (hasText)
            {
                SetTextStates(
                    style,
                    appearance.ForegroundColor ?? style.normal.textColor,
                    appearance.HoverForegroundColor ?? appearance.ForegroundColor ?? style.hover.textColor,
                    appearance.ActiveForegroundColor ?? appearance.ForegroundColor ?? style.active.textColor,
                    appearance.FocusedForegroundColor ?? appearance.HoverForegroundColor ?? appearance.ForegroundColor ?? style.focused.textColor
                );
            }
        }

        private void CopyStyle(GUIStyle target, GUIStyle source)
        {
            if (target == null || source == null)
                return;

            var replacement = CloneStyle(source);
            target.name = replacement.name;
            target.font = replacement.font;
            target.fontStyle = replacement.fontStyle;
            target.fontSize = replacement.fontSize;
            target.alignment = replacement.alignment;
            target.wordWrap = replacement.wordWrap;
            target.richText = replacement.richText;
            target.clipping = replacement.clipping;
            target.imagePosition = replacement.imagePosition;
            target.contentOffset = replacement.contentOffset;
            target.fixedWidth = replacement.fixedWidth;
            target.fixedHeight = replacement.fixedHeight;
            target.stretchWidth = replacement.stretchWidth;
            target.stretchHeight = replacement.stretchHeight;
            target.margin = replacement.margin;
            target.padding = replacement.padding;
            target.border = replacement.border;
            target.overflow = replacement.overflow;
            target.normal = replacement.normal;
            target.hover = replacement.hover;
            target.active = replacement.active;
            target.focused = replacement.focused;
            target.onNormal = replacement.onNormal;
            target.onHover = replacement.onHover;
            target.onActive = replacement.onActive;
            target.onFocused = replacement.onFocused;
        }

        private void MergeStyle(GUIStyle target, GUIStyle source)
        {
            if (target == null || source == null)
                return;

            if (source.font != null)
                target.font = source.font;
            if (source.fontSize > 0)
                target.fontSize = source.fontSize;

            target.fontStyle = source.fontStyle;
            target.alignment = source.alignment;
            target.wordWrap = source.wordWrap;
            target.richText = source.richText;
            target.clipping = source.clipping;
            target.imagePosition = source.imagePosition;
            target.contentOffset = source.contentOffset;

            if (source.fixedWidth > 0f)
                target.fixedWidth = source.fixedWidth;
            if (source.fixedHeight > 0f)
                target.fixedHeight = source.fixedHeight;

            target.stretchWidth = source.stretchWidth;
            target.stretchHeight = source.stretchHeight;

            if (source.margin != null)
                target.margin = source.margin;
            if (source.padding != null)
                target.padding = source.padding;
            if (source.border != null)
                target.border = source.border;
            if (source.overflow != null)
                target.overflow = source.overflow;

            MergeState(target.normal, source.normal);
            MergeState(target.hover, source.hover);
            MergeState(target.active, source.active);
            MergeState(target.focused, source.focused);
            MergeState(target.onNormal, source.onNormal);
            MergeState(target.onHover, source.onHover);
            MergeState(target.onActive, source.onActive);
            MergeState(target.onFocused, source.onFocused);
        }

        private void MergeState(GUIStyleState target, GUIStyleState source)
        {
            if (target == null || source == null)
                return;

            if (source.background != null)
                target.background = source.background;
            if (source.textColor != default)
                target.textColor = source.textColor;
        }

        private Texture2D CreateFocusedBorderTexture(GUIStyle style, float fallbackHeight = -1f)
        {
            var height = Mathf.Max(28, Mathf.RoundToInt(style.fixedHeight > 0 ? style.fixedHeight : (fallbackHeight > 0 ? fallbackHeight : GetScaledHeight(DesignTokens.Height.Default))));
            return CreateFocusTexture(128, height, DesignTokens.Radius.MD, GetTheme().Base);
        }

        private void ApplySize(GUIStyle style, StyleComponentType type, ControlSize size)
        {
            switch (type)
            {
                case StyleComponentType.Button:
                case StyleComponentType.Toggle:
                case StyleComponentType.Switch:
                case StyleComponentType.Checkbox:
                case StyleComponentType.CheckboxSolid:
                case StyleComponentType.TabsTrigger:
                case StyleComponentType.DropdownMenuItem:
                case StyleComponentType.SelectItem:
                case StyleComponentType.MenuBarItem:
                    ApplyControlSize(style, size);
                    return;
                case StyleComponentType.Input:
                case StyleComponentType.PasswordField:
                case StyleComponentType.TextArea:
                    ApplyInputSize(style, size);
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
            style.fixedWidth = 0f;

            switch (size)
            {
                case ControlSize.ExtraSmall:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Button.ExtraSmallH, DesignTokens.Padding.Button.ExtraSmallV);
                    style.fixedHeight = GetMinimumControlHeight(DesignTokens.Height.ExtraSmall, style.fontSize, DesignTokens.Padding.Button.ExtraSmallV);
                    break;
                case ControlSize.Mini:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Button.MiniH, DesignTokens.Padding.Button.MiniV);
                    style.fixedHeight = GetMinimumControlHeight(DesignTokens.Height.Mini, style.fontSize, DesignTokens.Padding.Button.MiniV);
                    break;
                case ControlSize.Small:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
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
                case ControlSize.IconExtraSmall:
                    style.fixedWidth = GetScaledHeight(DesignTokens.Height.ExtraSmall);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Height.ExtraSmall);
                    style.padding = new RectOffset();
                    break;
                case ControlSize.IconSmall:
                    style.fixedWidth = GetScaledHeight(DesignTokens.Height.Small);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Height.Small);
                    style.padding = new RectOffset();
                    break;
                case ControlSize.IconLarge:
                    style.fixedWidth = GetScaledHeight(DesignTokens.Height.Large);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Height.Large);
                    style.padding = new RectOffset();
                    break;
                default:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
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

        private void ApplyInputSize(GUIStyle style, ControlSize size)
        {
            style.fixedWidth = 0f;
            style.wordWrap = false;

            switch (size)
            {
                case ControlSize.ExtraSmall:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(10f, 4f);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Height.ExtraSmall);
                    break;
                case ControlSize.Mini:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(10f, 4f);
                    style.fixedHeight = GetScaledHeight(28f);
                    break;
                case ControlSize.Small:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    style.padding = GetSpacingOffset(11f, 5f);
                    style.fixedHeight = GetScaledHeight(32f);
                    break;
                case ControlSize.Large:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Input.Horizontal + 2f, DesignTokens.Padding.Input.Vertical + 1f);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Height.Large);
                    break;
                default:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Input.Horizontal, DesignTokens.Padding.Input.Vertical);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Height.Default);
                    break;
            }
        }

        private void ApplyChipSize(GUIStyle style, ControlSize size)
        {
            switch (size)
            {
                case ControlSize.ExtraSmall:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.SM, DesignTokens.Spacing.XXS);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Badge.Height - 4f);
                    break;
                case ControlSize.Mini:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.MD, DesignTokens.Spacing.XXS);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Badge.Height - 2f);
                    break;
                case ControlSize.Small:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.MD, DesignTokens.Spacing.XS);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Badge.Height);
                    break;
                case ControlSize.Large:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.SM);
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.LG, DesignTokens.Spacing.XS);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Badge.Height + 4f);
                    break;
                default:
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XS);
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Badge.Horizontal, DesignTokens.Padding.Badge.Vertical);
                    style.fixedHeight = GetScaledHeight(DesignTokens.Badge.Height);
                    break;
            }
        }

        private void ApplyFontOnlySize(GUIStyle style, ControlSize size)
        {
            style.fontSize = size switch
            {
                ControlSize.Mini => GetScaledFontSize(DesignTokens.FontScale.XS),
                ControlSize.ExtraSmall => GetScaledFontSize(DesignTokens.FontScale.XS),
                ControlSize.Small => GetScaledFontSize(DesignTokens.FontScale.SM),
                ControlSize.Large => GetScaledFontSize(DesignTokens.FontScale.LG),
                _ => GetScaledFontSize(DesignTokens.FontScale.MD),
            };
        }

        private void ApplyVariant(GUIStyle style, StyleComponentType type, ControlVariant variant)
        {
            if (variant == ControlVariant.Default)
                return;

            var theme = GetTheme();

            if (type == StyleComponentType.Label || type == StyleComponentType.SectionHeader || type == StyleComponentType.ChartAxis || type == StyleComponentType.CardTitle || type == StyleComponentType.CardDescription)
            {
                var labelColor = variant switch
                {
                    ControlVariant.Destructive => theme.Destructive,
                    ControlVariant.Link => theme.ButtonLinkColor,
                    ControlVariant.Muted => theme.Muted,
                    _ => style.normal.textColor,
                };
                SetTextStates(style, labelColor);
                return;
            }

            if (type == StyleComponentType.Input || type == StyleComponentType.PasswordField || type == StyleComponentType.TextArea)
            {
                var height = GetTextureHeight(style, DesignTokens.Height.Default);
                var width = GetTextureWidth(style, DesignTokens.TextureSize.Default);
                switch (variant)
                {
                    case ControlVariant.Outline:
                        break;
                    case ControlVariant.Ghost:
                        SetBackgroundStates(
                            style,
                            CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, Color.clear, Color.clear, 0f),
                            CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, GetGhostFill(0.035f), Color.clear, 0f),
                            CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, GetGhostFill(0.055f), Color.clear, 0f),
                            CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, GetGhostFill(0.035f), Color.clear, 0f)
                        );
                        break;
                    case ControlVariant.Secondary:
                        SetBackgroundStates(
                            style,
                            CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, theme.Secondary, Color.clear, 0f),
                            CreateSurfaceTexture(width, height, DesignTokens.Radius.MD, HoverSurface(theme.Secondary), Color.clear, 0f),
                            CreateFocusTexture(width, height, DesignTokens.Radius.MD, theme.Secondary),
                            CreateFocusTexture(width, height, DesignTokens.Radius.MD, theme.Secondary)
                        );
                        break;
                    case ControlVariant.Muted:
                        SetTextStates(style, theme.Muted);
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
            var hoverFill = HoverSurface(fill);
            var activeFill = ActiveSurface(fill);
            var text = theme.ButtonPrimaryFg;
            var hoverText = text;
            var activeText = text;
            var border = Color.clear;
            var hoverBorder = Color.clear;
            var activeBorder = Color.clear;
            var borderThickness = 0f;

            switch (variant)
            {
                case ControlVariant.Secondary:
                    fill = theme.ButtonSecondaryBg;
                    hoverFill = HoverSurface(fill);
                    activeFill = ActiveSurface(fill);
                    text = theme.ButtonSecondaryFg;
                    break;
                case ControlVariant.Destructive:
                    fill = theme.ButtonDestructiveBg;
                    hoverFill = HoverSurface(fill);
                    activeFill = ActiveSurface(fill);
                    text = theme.ButtonDestructiveFg;
                    break;
                case ControlVariant.Outline:
                    fill = theme.Base;
                    hoverFill = theme.Secondary;
                    activeFill = ActiveSurface(theme.Secondary);
                    text = theme.ButtonOutlineFg;
                    border = theme.Border;
                    hoverBorder = theme.Border;
                    activeBorder = theme.Border;
                    borderThickness = 1f;
                    break;
                case ControlVariant.Ghost:
                    fill = Color.clear;
                    hoverFill = theme.Secondary;
                    activeFill = ActiveSurface(theme.Secondary);
                    text = theme.ButtonGhostFg;
                    break;
                case ControlVariant.Link:
                    fill = Color.clear;
                    hoverFill = Color.clear;
                    activeFill = Color.clear;
                    text = theme.ButtonLinkColor;
                    break;
                case ControlVariant.Muted:
                    fill = theme.Secondary;
                    hoverFill = HoverSurface(theme.Secondary);
                    activeFill = ActiveSurface(theme.Secondary);
                    text = theme.Muted;
                    break;
            }

            var radius = GetRadiusFromStyle(style, DesignTokens.Radius.MD);
            var height = GetTextureHeight(style, DesignTokens.Height.Default);
            var width = GetTextureWidth(style, DesignTokens.TextureSize.Default);
            var normal = CreateSurfaceTexture(width, height, radius, fill, border, borderThickness);
            var hover = CreateSurfaceTexture(width, height, radius, hoverFill, hoverBorder, borderThickness);
            var active = CreateSurfaceTexture(width, height, radius, activeFill, activeBorder, borderThickness);

            SetOffBackgroundStates(style, normal, hover, active, hover);
            SetOffTextStates(style, text, hoverText, activeText, hoverText);
        }

        public GUIStyle GetButtonStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Button, variant, size, _baseButtonStyle, appearance);

        public GUIStyle GetToggleStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Toggle, variant, size, _baseToggleStyle, appearance);

        public GUIStyle GetLabelStyle(ControlVariant variant, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Label, variant, size, _baseLabelStyle, appearance);

        public GUIStyle GetProgressBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.ProgressBar,
                variant,
                size,
                _progressBarStyle,
                appearance,
                0,
                style =>
                {
                    var height = Mathf.Max(1, Mathf.RoundToInt(DesignTokens.ProgressBar.TextureHeight * _guiHelper.uiScale));
                    style.fixedHeight = height;
                    style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.border = CreateBorderSlice(height / 2, DesignTokens.TextureSize.Default, height);
                    style.stretchHeight = false;
                    style.stretchWidth = true;
                }
            );

        public GUIStyle GetBadgeStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Badge, variant, size, _baseBadgeStyle, appearance);

        public GUIStyle GetCardStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Card, variant, size, _cardStyle, appearance);

        public GUIStyle GetDialogContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Dialog, variant, size, _dialogContentStyle, appearance);

        public GUIStyle GetChartStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Chart, variant, size, _chartContainerStyle, appearance);

        public GUIStyle GetAnimatedBoxStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Large, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.AnimatedBox, variant, size, AnimatedBoxStyle, appearance);

        public GUIStyle GetMenuBarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.MenuBar, variant, size, _menuBarStyle, appearance);

        public GUIStyle GetTabsListStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.TabsList,
                variant,
                size,
                _tabsListStyle,
                appearance,
                0,
                style =>
                {
                    style.stretchWidth = true;
                    style.stretchHeight = false;
                    style.fixedHeight = 0f;
                }
            );

        public GUIStyle GetSelectStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.SelectContent, variant, size, _dropdownContentStyle, appearance);

        public GUIStyle GetDropdownMenuStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DropdownMenu, variant, size, _dropdownContentStyle, appearance);

        public GUIStyle GetInputStyle(ControlVariant variant, ControlSize size = ControlSize.Default, bool focused = false, bool disabled = false, ComponentAppearance appearance = null)
        {
            return ResolveStyle(
                StyleComponentType.Input,
                variant,
                size,
                _baseInputStyle,
                appearance,
                (focused ? 1 : 0) | (disabled ? 2 : 0),
                style =>
                {
                    if (focused)
                        style.focused.background = CreateFocusedBorderTexture(style);

                    if (disabled)
                    {
                        SetTextStates(style, GetTheme().Muted);
                    }
                }
            );
        }

        public GUIStyle GetTextAreaStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool focused = false, ComponentAppearance appearance = null)
        {
            return ResolveStyle(
                StyleComponentType.TextArea,
                variant,
                size,
                _baseInputStyle,
                appearance,
                focused ? 1 : 0,
                style =>
                {
                    style.wordWrap = true;
                    style.stretchHeight = true;
                    style.fixedHeight = 0f;
                    if (focused)
                        style.focused.background = CreateFocusedBorderTexture(style, 96);
                }
            );
        }

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null)
        {
            return ResolveStyle(
                StyleComponentType.Separator,
                variant,
                size,
                _separatorStyle,
                appearance,
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

        public GUIStyle GetTabsTriggerStyle(bool active = false, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null)
        {
            return ResolveStyle(
                StyleComponentType.TabsTrigger,
                variant,
                size,
                _tabsTriggerStyle,
                appearance,
                active ? 1 : 0,
                style =>
                {
                    style.alignment = TextAnchor.MiddleCenter;
                    if (active)
                    {
                        SetOffBackgroundStates(style, Textures.TabsActive, Textures.TabsActive, Textures.TabsActive, Textures.TabsActive);
                        SetOffTextStates(style, GetTheme().TabsTriggerActiveFg);
                    }
                    else
                    {
                        SetOffTextStates(style, GetTheme().TabsTriggerFg);
                    }
                }
            );
        }

        public GUIStyle GetTabsContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(StyleComponentType.TabsContent, variant, size, GUIStyle.none, appearance, 0, style => style.padding = GetSpacingOffset(DesignTokens.Spacing.XL, DesignTokens.Spacing.LG));

        public GUIStyle GetCheckboxStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Checkbox, variant, size, _checkboxStyle, appearance);

        public GUIStyle GetCheckboxSolidStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CheckboxSolid, variant, size, _checkboxSolidStyle, appearance);

        public GUIStyle GetSwitchStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Switch, variant, size, _baseSwitchStyle, appearance);

        public GUIStyle GetAvatarStyle(ControlSize size, AvatarShape shape, ControlVariant variant = ControlVariant.Default, ComponentAppearance appearance = null)
        {
            return ResolveStyle(
                StyleComponentType.Avatar,
                variant,
                size,
                _avatarStyle,
                appearance,
                (int)shape,
                style =>
                {
                    var px = size switch
                    {
                        ControlSize.ExtraSmall => GetScaledHeight(DesignTokens.Height.ExtraSmall),
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
                ControlSize.ExtraSmall => DesignTokens.StatusIndicator.Mini * _guiHelper.uiScale,
                ControlSize.Mini => DesignTokens.StatusIndicator.Mini * _guiHelper.uiScale,
                ControlSize.Small => DesignTokens.StatusIndicator.Small * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.StatusIndicator.Large * _guiHelper.uiScale,
                _ => DesignTokens.StatusIndicator.Default * _guiHelper.uiScale,
            };

        public GUIStyle GetTableStyle(ControlVariant variant, ControlSize size, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.Table,
                variant,
                size,
                _baseTableStyle,
                appearance,
                0,
                style =>
                {
                    style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.stretchWidth = true;
                    style.stretchHeight = false;
                }
            );

        public GUIStyle GetTableRowStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.TableRow,
                variant,
                size,
                _tableRowStyle,
                appearance,
                0,
                style =>
                {
                    style.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                    style.stretchWidth = true;
                    style.stretchHeight = false;
                }
            );

        public GUIStyle GetTableHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.TableHeader,
                variant,
                size,
                _tableHeaderStyle,
                appearance,
                0,
                style =>
                {
                    style.alignment = TextAnchor.MiddleLeft;
                    style.padding = GetSpacingOffset(DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV);
                    style.wordWrap = false;
                    style.clipping = TextClipping.Clip;
                    style.stretchHeight = false;
                }
            );

        public UnityHelpers.GUIStyle GetTableCellStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, TextAnchor alignment = TextAnchor.MiddleLeft) =>
            ResolveStyle(
                StyleComponentType.TableCell,
                variant,
                size,
                _tableCellStyle,
                null,
                (int)alignment,
                style =>
                {
                    style.alignment = alignment;
                    style.wordWrap = false;
                    style.clipping = TextClipping.Clip;
                    style.stretchHeight = false;
                }
            );

        public GUIStyle GetDropdownMenuItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DropdownMenuItem, variant, size, _dropdownItemStyle, appearance);

        public GUIStyle GetSelectItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.SelectItem, variant, size, _dropdownItemStyle, appearance);

        public GUIStyle GetMenuBarItemStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, bool isShortcut = false, bool active = false, ComponentAppearance appearance = null)
        {
            return ResolveStyle(
                StyleComponentType.MenuBarItem,
                variant,
                size,
                _dropdownItemStyle,
                appearance,
                (isShortcut ? 1 : 0) | (active ? 2 : 0),
                style =>
                {
                    style.alignment = isShortcut ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
                    SetOffTextStates(style, isShortcut ? Lift(GetTheme().Muted, 0.2f) : GetTheme().Text);
                    if (active)
                    {
                        var background = CreateSurfaceTexture(128, GetScaledHeight(DesignTokens.Height.Small), DesignTokens.Radius.SM, GetTheme().Secondary, Color.clear, 0f);
                        SetOffBackgroundStates(style, background, background, background, background);
                    }
                }
            );
        }

        public GUIStyle GetMenuDropdownStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.MenuDropdown, variant, size, _dropdownContentStyle, appearance);

        public GUIStyle GetNavigationStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Navigation, variant, size, _navigationStyle, appearance);

        public GUIStyle GetPopoverContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Popover, variant, size, _dropdownContentStyle, appearance);

        public GUIStyle GetTooltipStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.Tooltip,
                variant,
                size,
                _dropdownContentStyle,
                appearance,
                0,
                style =>
                {
                    style.padding = GetSpacingOffset(DesignTokens.Spacing.MD, DesignTokens.Spacing.SM);
                    style.wordWrap = true;
                }
            );

        public GUIStyle GetSectionHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.SectionHeader,
                variant,
                size,
                _baseLabelStyle,
                appearance,
                0,
                style =>
                {
                    style.fontStyle = FontStyle.Bold;
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.LG);
                    style.normal.textColor = GetTheme().Text;
                }
            );

        public GUIStyle GetCardHeaderStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.CardHeader,
                variant,
                size,
                GUIStyle.none,
                appearance,
                0,
                style => style.padding = new UnityHelpers.RectOffset(GetScaledSpacing(DesignTokens.Padding.Card.Horizontal), GetScaledSpacing(DesignTokens.Padding.Card.Horizontal), GetScaledSpacing(DesignTokens.Padding.Card.Vertical), GetScaledSpacing(DesignTokens.Spacing.SM))
            );

        public GUIStyle GetCardContentStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.CardContent,
                variant,
                size,
                GUIStyle.none,
                appearance,
                0,
                style => style.padding = new UnityHelpers.RectOffset(GetScaledSpacing(DesignTokens.Padding.Card.Horizontal), GetScaledSpacing(DesignTokens.Padding.Card.Horizontal), 0, GetScaledSpacing(DesignTokens.Padding.Card.Vertical))
            );

        public GUIStyle GetCardFooterStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.CardFooter,
                variant,
                size,
                GUIStyle.none,
                appearance,
                0,
                style => style.padding = new UnityHelpers.RectOffset(GetScaledSpacing(DesignTokens.Padding.Card.Horizontal), GetScaledSpacing(DesignTokens.Padding.Card.Horizontal), GetScaledSpacing(DesignTokens.Spacing.SM), GetScaledSpacing(DesignTokens.Padding.Card.Vertical))
            );

        public GUIStyle GetCardTitleStyle(ComponentAppearance appearance = null) => GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, appearance);

        public GUIStyle GetCardTitleStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.CardTitle,
                variant,
                size,
                _baseLabelStyle,
                appearance,
                0,
                style =>
                {
                    style.fontStyle = FontStyle.Bold;
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.XL);
                    style.normal.textColor = GetTheme().Text;
                    style.wordWrap = true;
                }
            );

        public GUIStyle GetCardDescriptionStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) =>
            ResolveStyle(
                StyleComponentType.CardDescription,
                variant,
                size,
                _baseLabelStyle,
                appearance,
                0,
                style =>
                {
                    style.fontSize = GetScaledFontSize(DesignTokens.FontScale.MD);
                    style.normal.textColor = GetTheme().Muted;
                    style.wordWrap = true;
                }
            );

        public float GetSliderTrackHeight(ControlSize size) =>
            size switch
            {
                ControlSize.ExtraSmall => DesignTokens.Slider.TrackMini * _guiHelper.uiScale,
                ControlSize.Mini => DesignTokens.Slider.TrackMini * _guiHelper.uiScale,
                ControlSize.Small => DesignTokens.Slider.TrackSmall * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.Slider.TrackLarge * _guiHelper.uiScale,
                _ => DesignTokens.Slider.TrackDefault * _guiHelper.uiScale,
            };

        public float GetSliderThumbSize(ControlSize size) =>
            size switch
            {
                ControlSize.ExtraSmall => DesignTokens.Slider.ThumbMini * _guiHelper.uiScale,
                ControlSize.Mini => DesignTokens.Slider.ThumbMini * _guiHelper.uiScale,
                ControlSize.Small => DesignTokens.Slider.ThumbSmall * _guiHelper.uiScale,
                ControlSize.Large => DesignTokens.Slider.ThumbLarge * _guiHelper.uiScale,
                _ => DesignTokens.Slider.ThumbDefault * _guiHelper.uiScale,
            };

        public Color GetSliderTrackColor(ControlVariant variant, bool disabled, ComponentAppearance appearance = null)
        {
            var color = appearance?.BackgroundColor ?? GetTheme().Secondary;
            return disabled ? Color.Lerp(color, GetTheme().Muted, 0.5f) : color;
        }

        public Color GetSliderFillColor(ControlVariant variant, bool disabled, ComponentAppearance appearance = null)
        {
            var color =
                appearance?.AccentColor
                ?? variant switch
                {
                    ControlVariant.Destructive => GetTheme().ButtonDestructiveBg,
                    ControlVariant.Secondary => GetTheme().ButtonSecondaryBg,
                    ControlVariant.Muted => GetTheme().Muted,
                    _ => GetTheme().ButtonPrimaryBg,
                };

            return disabled ? Color.Lerp(color, GetTheme().Muted, 0.5f) : color;
        }

        public Color GetSliderThumbColor(ControlVariant variant, bool disabled, ComponentAppearance appearance = null)
        {
            var color = appearance?.ForegroundColor ?? GetTheme().Text;
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

        public GUIStyle GetCalendarStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.Calendar, variant, size, _calendarStyle, appearance);

        public GUIStyle GetCalendarWeekdayStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CalendarWeekday, ControlVariant.Default, ControlSize.Default, _calendarWeekdayStyle, appearance);

        public GUIStyle GetCalendarDayStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CalendarDay, ControlVariant.Default, ControlSize.Default, _calendarDayStyle, appearance);

        public GUIStyle GetCalendarDaySelectedStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CalendarDaySelected, ControlVariant.Default, ControlSize.Default, _calendarDaySelectedStyle, appearance);

        public GUIStyle GetCalendarDayInRangeStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CalendarDayInRange, ControlVariant.Default, ControlSize.Default, _calendarDayInRangeStyle, appearance);

        public GUIStyle GetCalendarDayTodayStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CalendarDayToday, ControlVariant.Default, ControlSize.Default, _calendarDayTodayStyle, appearance);

        public GUIStyle GetCalendarDayOutsideMonthStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.CalendarDayOutsideMonth, ControlVariant.Default, ControlSize.Default, _calendarDayOutsideMonthStyle, appearance);

        public GUIStyle GetDatePickerStyle(ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DatePicker, variant, size, _datePickerStyle, appearance);

        public GUIStyle GetDatePickerWeekdayStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DatePickerWeekday, ControlVariant.Default, ControlSize.Default, _datePickerWeekdayStyle, appearance);

        public GUIStyle GetDatePickerDayStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DatePickerDay, ControlVariant.Default, ControlSize.Default, _datePickerDayStyle, appearance);

        public GUIStyle GetDatePickerDaySelectedStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DatePickerDaySelected, ControlVariant.Default, ControlSize.Default, _datePickerDaySelectedStyle, appearance);

        public GUIStyle GetDatePickerDayTodayStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DatePickerDayToday, ControlVariant.Default, ControlSize.Default, _datePickerDayTodayStyle, appearance);

        public GUIStyle GetDatePickerDayOutsideMonthStyle(ComponentAppearance appearance = null) => ResolveStyle(StyleComponentType.DatePickerDayOutsideMonth, ControlVariant.Default, ControlSize.Default, _datePickerDayOutsideMonthStyle, appearance);
    }
}
