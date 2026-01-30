using System;
using System.Collections.Generic;
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
        #region Style Builder
        internal class StyleBuilder
        {
            private readonly StyleManager _manager;
            private readonly StyleComponentType? _type;
            private readonly GUIStyle _style;
            private readonly Theme _theme;
            private readonly List<(ControlVariant variant, StyleModifier modifier)> _variants = new();
            private readonly List<(ControlSize size, StyleModifier modifier)> _sizes = new();
            private SizeConfig? _sizeConfig;

            public StyleBuilder(StyleManager mgr, GUIStyle baseStyle = null, StyleComponentType? type = null)
            {
                _manager = mgr;
                _type = type;
                _theme = mgr.GetTheme();
                _style = baseStyle != null ? new UnityHelpers.GUIStyle(baseStyle) : new UnityHelpers.GUIStyle(GUI.skin.box);
                if (mgr.CustomFont != null)
                    _style.font = mgr.CustomFont;
            }

            public StyleBuilder Font(int size, FontStyle fontStyle = FontStyle.Normal)
            {
                _style.fontSize = _manager.GetScaledFontSize(size / (float)_manager._guiHelper.fontSize);
                _style.fontStyle = fontStyle;
                return this;
            }

            public StyleBuilder FontScale(float scale, FontStyle fontStyle = FontStyle.Normal)
            {
                _style.fontSize = _manager.GetScaledFontSize(scale);
                _style.fontStyle = fontStyle;
                return this;
            }

            public StyleBuilder Align(TextAnchor anchor)
            {
                _style.alignment = anchor;
                return this;
            }

            public StyleBuilder Padding(int h, int v)
            {
                _style.padding = new UnityHelpers.RectOffset(h, h, v, v);
                return this;
            }

            public StyleBuilder Padding(float h, float v)
            {
                int hScaled = _manager.GetScaledSpacing(h);
                int vScaled = _manager.GetScaledSpacing(v);
                _style.padding = new UnityHelpers.RectOffset(hScaled, hScaled, vScaled, vScaled);
                return this;
            }

            public StyleBuilder Margin(int h, int v)
            {
                _style.margin = new UnityHelpers.RectOffset(h, h, v, v);
                return this;
            }

            public StyleBuilder Margin(float h, float v)
            {
                int hScaled = _manager.GetScaledSpacing(h);
                int vScaled = _manager.GetScaledSpacing(v);
                _style.margin = new UnityHelpers.RectOffset(hScaled, hScaled, vScaled, vScaled);
                return this;
            }

            public StyleBuilder TextColor(Color c)
            {
                _style.normal.textColor = c;
                return this;
            }

            public StyleBuilder Background(Texture2D tex)
            {
                _style.normal.background = tex;
                return this;
            }

            public StyleBuilder FixedHeight(float h)
            {
                _style.fixedHeight = _manager.GetScaledHeight(h);
                return this;
            }

            public StyleBuilder FixedWidth(float w)
            {
                _style.fixedWidth = _manager.GetScaledHeight(w);
                return this;
            }

            public StyleBuilder FixedSize(float size)
            {
                int scaled = _manager.GetScaledHeight(size);
                _style.fixedWidth = scaled;
                _style.fixedHeight = scaled;
                return this;
            }

            public StyleBuilder Border(int r)
            {
                _style.border = new UnityHelpers.RectOffset(r, r, r, r);
                return this;
            }

            public StyleBuilder Border(float r)
            {
                int scaled = _manager.GetScaledBorderRadius(r);
                _style.border = new UnityHelpers.RectOffset(scaled, scaled, scaled, scaled);
                return this;
            }

            public StyleBuilder WordWrap(bool wrap)
            {
                _style.wordWrap = wrap;
                return this;
            }

            public StyleBuilder RichText(bool rich)
            {
                _style.richText = rich;
                return this;
            }

            public StyleBuilder Clipping(TextClipping clip)
            {
                _style.clipping = clip;
                return this;
            }

            public StyleBuilder Stretch(bool width, bool height)
            {
                _style.stretchWidth = width;
                _style.stretchHeight = height;
                return this;
            }

            public StyleBuilder Apply(Action<GUIStyle, Theme> customize)
            {
                customize?.Invoke(_style, _theme);
                return this;
            }

            public StyleBuilder Variant(ControlVariant variant, StyleModifier modifier)
            {
                _variants.Add((variant, modifier));
                return this;
            }

            public StyleBuilder Variant(ControlVariant variant, Action<GUIStyle, Theme> modifier)
            {
                _variants.Add((variant, (s, t, _) => modifier(s, t)));
                return this;
            }

            public StyleBuilder Sizes(SizeConfig config)
            {
                _sizeConfig = config;
                return this;
            }

            public StyleBuilder Size(ControlSize size, StyleModifier modifier)
            {
                _sizes.Add((size, modifier));
                return this;
            }

            public StyleBuilder Size(ControlSize size, Action<GUIStyle, Theme> modifier)
            {
                _sizes.Add((size, (s, t, _) => modifier(s, t)));
                return this;
            }

            public GUIStyle Build()
            {
                if (_type.HasValue)
                {
                    foreach (var (variant, modifier) in _variants)
                        _manager.Registry.RegisterVariant(_type.Value, variant, modifier);

                    if (_sizeConfig.HasValue)
                        _manager.RegisterSizeSet(_type.Value, _sizeConfig.Value);

                    foreach (var (size, modifier) in _sizes)
                        _manager.Registry.RegisterSize(_type.Value, size, modifier);
                }

                return _style;
            }
        }

        internal StyleBuilder Style(StyleComponentType type) => new StyleBuilder(this, null, type);

        internal StyleBuilder Style(GUIStyle baseStyle = null) => new StyleBuilder(this, baseStyle, null);
        #endregion

        #region Texture Factories
        internal Func<Color, Texture2D> SolidTex(int w, int h, float r) => c => CreateGradientRoundedRectTexture(w, h, GetScaledBorderRadius(r), c);

        internal Func<Color, Texture2D> GradientTex(int w, int h, float r) => c => CreateGradientRoundedRectWithShadowTexture(w, h, GetScaledBorderRadius(r), Color.Lerp(c, Color.white, 0.05f), c, DesignTokens.Effects.ShadowLight, (int)DesignTokens.Effects.ShadowBlurSM);

        internal Func<Color, Texture2D> OutlineTex(int w, int h, float r, float t = 1.5f) => c => CreateRoundedOutlineTexture(w, h, GetScaledBorderRadius(r), c, t);

        internal Func<Color, Color, Texture2D> BorderedTex(int w, int h, float r, float t = 1.5f) => (f, b) => CreateBorderedRoundedRectTexture(w, h, GetScaledBorderRadius(r), f, b, t);

        internal Func<Color, Texture2D> FocusTex(int w, int h, float r) => c => CreateFocusRingTexture(w, h, GetScaledBorderRadius(r), c, DesignTokens.Effects.FocusRingThickness);

        internal static Color Alpha(Color c, float a) => new Color(c.r, c.g, c.b, a);

        internal static bool IsDark(Color c) => (0.299f * c.r + 0.587f * c.g + 0.114f * c.b) < 0.3f;
        #endregion

        #region Size Configuration
        public readonly struct SizeValues
        {
            public readonly float FontScale;
            public readonly float PadH;
            public readonly float PadV;
            public readonly float Height;
            public readonly float Width;

            public SizeValues(float fontScale, float padH, float padV, float height, float width)
            {
                FontScale = fontScale;
                PadH = padH;
                PadV = padV;
                Height = height;
                Width = width;
            }

            public static SizeValues Font(float scale) => new SizeValues(scale, -1, -1, 0, 0);

            public static SizeValues Pad(float h, float v) => new SizeValues(0, h, v, 0, 0);

            public static SizeValues Full(float font, float padH, float padV, float height) => new SizeValues(font, padH, padV, height, 0);

            public static SizeValues Square(float font, float size) => new SizeValues(font, -1, -1, size, size);

            public static SizeValues H(float height) => new SizeValues(0, -1, -1, height, 0);
        }

        public readonly struct SizeConfig
        {
            public readonly SizeValues Mini;
            public readonly SizeValues Small;
            public readonly SizeValues Default;
            public readonly SizeValues Large;

            public SizeConfig(SizeValues mini, SizeValues small, SizeValues @default, SizeValues large)
            {
                Mini = mini;
                Small = small;
                Default = @default;
                Large = large;
            }
        }

        private void RegisterSizeSet(StyleComponentType type, SizeConfig config)
        {
            Registry.RegisterSize(type, ControlSize.Mini, (s, t, g) => ApplySize(s, config.Mini));
            Registry.RegisterSize(type, ControlSize.Small, (s, t, g) => ApplySize(s, config.Small));
            Registry.RegisterSize(type, ControlSize.Default, (s, t, g) => ApplySize(s, config.Default));
            Registry.RegisterSize(type, ControlSize.Large, (s, t, g) => ApplySize(s, config.Large));
        }

        private void ApplySize(GUIStyle s, SizeValues v)
        {
            if (v.FontScale > 0)
                s.fontSize = GetScaledFontSize(v.FontScale);
            if (v.PadH >= 0 && v.PadV >= 0)
                s.padding = GetSpacingOffset(v.PadH, v.PadV);
            if (v.Height > 0)
                s.fixedHeight = GetScaledHeight(v.Height);
            if (v.Width > 0)
                s.fixedWidth = GetScaledHeight(v.Width);
        }
        #endregion

        #region Pred Size Configs
        private static class Sizes
        {
            public static readonly SizeConfig Button = new SizeConfig(
                SizeValues.Full(DesignTokens.FontScale.XS, DesignTokens.Padding.Button.MiniH, DesignTokens.Padding.Button.MiniV, DesignTokens.Height.Mini),
                SizeValues.Full(DesignTokens.FontScale.XS, DesignTokens.Padding.Button.SmallH, DesignTokens.Padding.Button.SmallV, DesignTokens.Height.Small),
                SizeValues.Full(DesignTokens.FontScale.SM, DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV, DesignTokens.Height.Default),
                SizeValues.Full(DesignTokens.FontScale.MD, DesignTokens.Padding.Button.LargeH, DesignTokens.Padding.Button.LargeV, DesignTokens.Height.Large)
            );

            public static readonly SizeConfig Toggle = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS * 0.85f, DesignTokens.Spacing.XS, DesignTokens.Spacing.None, 0, 0),
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.XXS, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, 0, 0),
                new SizeValues(DesignTokens.FontScale.LG, DesignTokens.Spacing.MD, DesignTokens.Spacing.SM, 0, 0)
            );

            public static readonly SizeConfig Input = new SizeConfig(
                SizeValues.Full(DesignTokens.FontScale.XS * 0.9f, DesignTokens.Spacing.SM, DesignTokens.Spacing.XXS, DesignTokens.Height.Mini),
                SizeValues.Full(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Height.Small),
                SizeValues.Full(DesignTokens.FontScale.SM, DesignTokens.Padding.Input.Horizontal, DesignTokens.Padding.Input.Vertical, DesignTokens.Height.Default),
                SizeValues.Full(DesignTokens.FontScale.MD, DesignTokens.Spacing.LG, DesignTokens.Spacing.MD, DesignTokens.Height.Large)
            );

            public static readonly SizeConfig TextArea = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS * 0.9f, DesignTokens.Spacing.XS, DesignTokens.Spacing.XXS, 0, 0),
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.MD, DesignTokens.Spacing.SM, 0, 0)
            );

            public static readonly SizeConfig FontOnly = new SizeConfig(SizeValues.Font(DesignTokens.FontScale.XS), SizeValues.Font(DesignTokens.FontScale.XS), SizeValues.Font(DesignTokens.FontScale.SM), SizeValues.Font(DesignTokens.FontScale.MD));

            public static readonly SizeConfig Item = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.XS, DesignTokens.Spacing.XXS, 0, 0),
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Spacing.MD, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.LG, DesignTokens.Spacing.SM, 0, 0)
            );

            public static readonly SizeConfig Container = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.XS, DesignTokens.Spacing.XS, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Spacing.LG, DesignTokens.Spacing.LG, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.XL, DesignTokens.Spacing.XL, 0, 0)
            );

            public static readonly SizeConfig CalendarDay = new SizeConfig(
                SizeValues.Square(DesignTokens.FontScale.XS, DesignTokens.CalendarDay.Mini),
                SizeValues.Square(DesignTokens.FontScale.XS, DesignTokens.CalendarDay.Small),
                SizeValues.Square(DesignTokens.FontScale.SM, DesignTokens.CalendarDay.Default),
                SizeValues.Square(DesignTokens.FontScale.MD, DesignTokens.CalendarDay.Large)
            );

            public static readonly SizeConfig ProgressBar = new SizeConfig(SizeValues.H(DesignTokens.Spacing.XXS), SizeValues.H(DesignTokens.Spacing.XS), SizeValues.H(DesignTokens.Spacing.SM), SizeValues.H(DesignTokens.Spacing.MD));

            public static readonly SizeConfig Separator = new SizeConfig(SizeValues.H(DesignTokens.Separator.DefaultThickness), SizeValues.H(DesignTokens.Separator.DefaultThickness), SizeValues.H(DesignTokens.Separator.LargeThickness), SizeValues.H(DesignTokens.Separator.LargeThickness * 2));

            public static readonly SizeConfig TabsList = new SizeConfig(
                SizeValues.Pad(DesignTokens.Spacing.None, DesignTokens.Spacing.None),
                SizeValues.Pad(DesignTokens.Spacing.XXS, DesignTokens.Spacing.XXS),
                SizeValues.Pad(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS),
                SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM)
            );

            public static readonly SizeConfig TabsContent = new SizeConfig(
                SizeValues.Pad(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS),
                SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
                SizeValues.Pad(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG),
                SizeValues.Pad(DesignTokens.Spacing.XL, DesignTokens.Spacing.XL)
            );

            public static readonly SizeConfig Badge = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS * 0.8f, DesignTokens.Spacing.SM, DesignTokens.Spacing.None, 0, 0),
                new SizeValues(DesignTokens.FontScale.XS * 0.85f, DesignTokens.Spacing.SM, DesignTokens.Spacing.XXS, 0, 0),
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Padding.Badge.Horizontal, DesignTokens.Padding.Badge.Vertical, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.MD, DesignTokens.Spacing.SM, 0, 0)
            );

            public static readonly SizeConfig Table = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.XL, DesignTokens.Spacing.LG, 0, 0)
            );

            public static readonly SizeConfig TableHeader = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM * 0.97f, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Padding.Table.CellH, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.LG, DesignTokens.Spacing.MD, 0, 0)
            );

            public static readonly SizeConfig TableCell = new SizeConfig(
                new SizeValues(DesignTokens.FontScale.XS * 0.9f, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.SM, DesignTokens.Padding.Table.CellH, DesignTokens.Spacing.SM, 0, 0),
                new SizeValues(DesignTokens.FontScale.MD, DesignTokens.Spacing.LG, DesignTokens.Spacing.MD, 0, 0)
            );

            public static readonly SizeConfig Dialog = new SizeConfig(
                SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
                SizeValues.Pad(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG),
                SizeValues.Pad(DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical),
                SizeValues.Pad(DesignTokens.Spacing.XXL, DesignTokens.Spacing.XXL)
            );

            public static readonly SizeConfig Card = new SizeConfig(
                SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
                SizeValues.Pad(DesignTokens.Spacing.MD, DesignTokens.Spacing.MD),
                SizeValues.Pad(DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical),
                SizeValues.Pad(DesignTokens.Spacing.XXL, DesignTokens.Spacing.XXL)
            );

            public static readonly SizeConfig MenuBar = new SizeConfig(
                SizeValues.Pad(DesignTokens.Spacing.None, DesignTokens.Spacing.None),
                SizeValues.Pad(DesignTokens.Spacing.XXS, DesignTokens.Spacing.None),
                SizeValues.Pad(DesignTokens.Spacing.XS, DesignTokens.Spacing.None),
                SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.None)
            );

            public static readonly SizeConfig SliderTrack = new SizeConfig(SizeValues.H(DesignTokens.Slider.TrackMini), SizeValues.H(DesignTokens.Slider.TrackSmall), SizeValues.H(DesignTokens.Slider.TrackDefault), SizeValues.H(DesignTokens.Slider.TrackLarge));

            public static readonly SizeConfig SliderThumb = new SizeConfig(SizeValues.Square(0, DesignTokens.Slider.ThumbMini), SizeValues.Square(0, DesignTokens.Slider.ThumbSmall), SizeValues.Square(0, DesignTokens.Slider.ThumbDefault), SizeValues.Square(0, DesignTokens.Slider.ThumbLarge));
        }
        #endregion

        #region Styles
        private void AllStyles()
        {
            ButtonStyles();
            ToggleStyles();
            InputStyles();
            TextAreaStyles();
            LabelStyles();
            ProgressBarStyles();
            SeparatorStyles();
            TabStyles();
            CheckboxStyles();
            SwitchStyles();
            BadgeStyles();
            TableStyles();
            CalendarStyles();
            DialogStyles();
            CardStyles();
            MenuStyles();
            SelectStyles();
            PopoverStyles();
            ChartStyles();
            AnimatedBoxStyles();
            ToastStyles();
            SliderStyles();
            AvatarStyles();
            DropdownStyles();
        }
        #endregion

        #region Button
        private void ButtonStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;
            int radius = GetScaledBorderRadius(r);

            var outline = OutlineTex(w, h, r);
            var bordered = BorderedTex(w, h, r);
            var focus = FocusTex(w, h, r);

            Func<Color, Texture2D> btnTex = c => 
                CreateGradientRoundedRectTexture(w, h, radius, c);

            _baseButtonStyle = Style(StyleComponentType.Button)
                .FontScale(DesignTokens.FontScale.SM, FontStyle.Bold)
                .Padding(DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV)
                .Border(DesignTokens.Radius.MD)
                .Align(TextAnchor.MiddleCenter)
                .FixedHeight(DesignTokens.Height.Default)
                .Stretch(false, false)
                .Apply((s, t) => 
                {
                    s.normal.background = btnTex(t.ButtonPrimaryBg);
                    s.normal.textColor = t.ButtonPrimaryFg;
                    
                    s.hover.background = btnTex(Alpha(t.ButtonPrimaryBg, 0.9f));
                    s.hover.textColor = t.ButtonPrimaryFg;
                    
                    s.active.background = btnTex(Color.Lerp(t.ButtonPrimaryBg, Color.black, 0.1f));
                    s.active.textColor = t.ButtonPrimaryFg;
                    
                    s.focused.background = focus(t.ButtonPrimaryFg);
                    s.focused.textColor = t.ButtonPrimaryFg;
                })
                .Variant(
                    ControlVariant.Destructive,
                    (s, t) =>
                    {
                        s.normal.background = btnTex(t.ButtonDestructiveBg);
                        s.normal.textColor = t.ButtonDestructiveFg;
                        s.hover.background = btnTex(Alpha(t.ButtonDestructiveBg, 0.9f));
                        s.hover.textColor = t.ButtonDestructiveFg;
                        s.active.background = btnTex(Color.Lerp(t.ButtonDestructiveBg, Color.black, 0.1f));
                        s.active.textColor = t.ButtonDestructiveFg;
                    }
                )
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        s.normal.background = outline(t.Border);
                        s.normal.textColor = t.Text;
                        
                        s.hover.background = bordered(Alpha(t.Accent, 0.1f), t.Accent);
                        s.hover.textColor = t.Text;
                        
                        s.active.background = bordered(Alpha(t.Accent, 0.2f), t.Accent);
                        s.active.textColor = t.Text;
                    }
                )
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        s.normal.background = btnTex(t.ButtonSecondaryBg);
                        s.normal.textColor = t.ButtonSecondaryFg;
                        
                        s.hover.background = btnTex(Color.Lerp(t.ButtonSecondaryBg, Color.black, 0.05f));
                        s.hover.textColor = t.ButtonSecondaryFg;
                        
                        s.active.background = btnTex(Color.Lerp(t.ButtonSecondaryBg, Color.black, 0.1f));
                        s.active.textColor = t.ButtonSecondaryFg;
                    }
                )
                .Variant(
                    ControlVariant.Ghost,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.ButtonGhostFg;
                        
                        s.hover.background = btnTex(Alpha(t.Accent, 0.1f));
                        s.hover.textColor = t.ButtonGhostFg;
                        
                        s.active.background = btnTex(Alpha(t.Accent, 0.2f));
                        s.active.textColor = t.ButtonGhostFg;
                    }
                )
                .Variant(
                    ControlVariant.Link,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.ButtonLinkColor;
                        s.hover.background = TransparentTexture;
                        s.hover.textColor = Color.Lerp(t.ButtonLinkColor, Color.white, 0.2f);
                        s.active.background = TransparentTexture;
                        s.active.textColor = Color.Lerp(t.ButtonLinkColor, Color.black, 0.2f);
                    }
                )
                .Sizes(Sizes.Button)
                .Size(
                    ControlSize.Icon,
                    (s, t) =>
                    {
                        int sz = GetScaledHeight(DesignTokens.Height.Default);
                        s.fixedWidth = sz;
                        s.fixedHeight = sz;
                        s.padding = GetSpacingOffset(0, 0);
                        s.alignment = TextAnchor.MiddleCenter;
                    }
                )
                .Build();
        }
        #endregion

        #region Toggle
        private void ToggleStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;
            int radius = GetScaledBorderRadius(r);

            var outline = OutlineTex(w, h, r);
            var bordered = BorderedTex(w, h, r);
            var focus = FocusTex(w, h, r);

            Func<Color, Texture2D> toggleTex = c => CreateGradientRoundedRectTexture(w, h, radius, c);

            _baseToggleStyle = Style(StyleComponentType.Toggle)
                .FontScale(DesignTokens.FontScale.SM, FontStyle.Bold)
                .Padding(DesignTokens.Padding.Button.DefaultH, DesignTokens.Padding.Button.DefaultV)
                .Border(DesignTokens.Radius.MD)
                .Align(TextAnchor.MiddleCenter)
                .FixedHeight(DesignTokens.Height.Default)
                .Stretch(false, false)
                .Apply((s, t) =>
                {
                    Color offBg = t.ButtonPrimaryBg;
                    Color onBg = Color.Lerp(t.ButtonPrimaryBg, t.Accent, 0.08f);

                    s.normal.background = toggleTex(offBg);
                    s.normal.textColor = t.ButtonPrimaryFg;

                    s.hover.background = toggleTex(Alpha(offBg, 0.9f));
                    s.hover.textColor = t.ButtonPrimaryFg;

                    s.active.background = toggleTex(Color.Lerp(offBg, Color.black, 0.1f));
                    s.active.textColor = t.ButtonPrimaryFg;

                    s.focused.background = focus(t.ButtonPrimaryFg);
                    s.focused.textColor = t.ButtonPrimaryFg;

                    s.onNormal.background = toggleTex(onBg);
                    s.onNormal.textColor = t.ButtonPrimaryFg;

                    s.onHover.background = toggleTex(Alpha(onBg, 0.9f));
                    s.onHover.textColor = t.ButtonPrimaryFg;

                    s.onActive.background = toggleTex(Color.Lerp(onBg, Color.black, 0.1f));
                    s.onActive.textColor = t.ButtonPrimaryFg;

                    s.onFocused.background = focus(t.ButtonPrimaryFg);
                    s.onFocused.textColor = t.ButtonPrimaryFg;
                })
                .Variant(
                    ControlVariant.Destructive,
                    (s, t) =>
                    {
                        Color offBg = t.ButtonDestructiveBg;
                        Color onBg = Color.Lerp(t.ButtonDestructiveBg, t.Destructive, 0.08f);

                        s.normal.background = toggleTex(offBg);
                        s.normal.textColor = t.ButtonDestructiveFg;
                        s.hover.background = toggleTex(Alpha(offBg, 0.9f));
                        s.hover.textColor = t.ButtonDestructiveFg;
                        s.active.background = toggleTex(Color.Lerp(offBg, Color.black, 0.1f));
                        s.active.textColor = t.ButtonDestructiveFg;

                        s.onNormal.background = toggleTex(onBg);
                        s.onNormal.textColor = t.ButtonDestructiveFg;
                        s.onHover.background = toggleTex(Alpha(onBg, 0.9f));
                        s.onHover.textColor = t.ButtonDestructiveFg;
                        s.onActive.background = toggleTex(Color.Lerp(onBg, Color.black, 0.1f));
                        s.onActive.textColor = t.ButtonDestructiveFg;
                    }
                )
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        s.normal.background = outline(t.Border);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Alpha(t.Accent, 0.1f), t.Accent);
                        s.hover.textColor = t.Text;
                        s.active.background = bordered(Alpha(t.Accent, 0.2f), t.Accent);
                        s.active.textColor = t.Text;

                        s.onNormal.background = bordered(Alpha(t.Accent, 0.08f), t.Accent);
                        s.onNormal.textColor = t.Text;
                        s.onHover.background = bordered(Alpha(t.Accent, 0.15f), t.Accent);
                        s.onHover.textColor = t.Text;
                        s.onActive.background = bordered(Alpha(t.Accent, 0.2f), t.Accent);
                        s.onActive.textColor = t.Text;
                    }
                )
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        Color offBg = t.ButtonSecondaryBg;
                        Color onBg = Color.Lerp(t.ButtonSecondaryBg, t.Accent, 0.08f);

                        s.normal.background = toggleTex(offBg);
                        s.normal.textColor = t.ButtonSecondaryFg;
                        s.hover.background = toggleTex(Color.Lerp(offBg, Color.black, 0.05f));
                        s.hover.textColor = t.ButtonSecondaryFg;
                        s.active.background = toggleTex(Color.Lerp(offBg, Color.black, 0.1f));
                        s.active.textColor = t.ButtonSecondaryFg;

                        s.onNormal.background = toggleTex(onBg);
                        s.onNormal.textColor = t.ButtonSecondaryFg;
                        s.onHover.background = toggleTex(Color.Lerp(onBg, Color.black, 0.05f));
                        s.onHover.textColor = t.ButtonSecondaryFg;
                        s.onActive.background = toggleTex(Color.Lerp(onBg, Color.black, 0.1f));
                        s.onActive.textColor = t.ButtonSecondaryFg;
                    }
                )
                .Variant(
                    ControlVariant.Ghost,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.ButtonGhostFg;
                        s.hover.background = toggleTex(Alpha(t.Accent, 0.1f));
                        s.hover.textColor = t.ButtonGhostFg;
                        s.active.background = toggleTex(Alpha(t.Accent, 0.2f));
                        s.active.textColor = t.ButtonGhostFg;

                        s.onNormal.background = toggleTex(Alpha(t.Accent, 0.05f));
                        s.onNormal.textColor = t.ButtonGhostFg;
                        s.onHover.background = toggleTex(Alpha(t.Accent, 0.12f));
                        s.onHover.textColor = t.ButtonGhostFg;
                        s.onActive.background = toggleTex(Alpha(t.Accent, 0.2f));
                        s.onActive.textColor = t.ButtonGhostFg;
                    }
                )
                .Variant(
                    ControlVariant.Link,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.ButtonLinkColor;
                        s.hover.background = TransparentTexture;
                        s.hover.textColor = Color.Lerp(t.ButtonLinkColor, Color.white, 0.2f);
                        s.active.background = TransparentTexture;
                        s.active.textColor = Color.Lerp(t.ButtonLinkColor, Color.black, 0.2f);

                        s.onNormal.background = TransparentTexture;
                        s.onNormal.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.1f);
                        s.onHover.background = TransparentTexture;
                        s.onHover.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.2f);
                        s.onActive.background = TransparentTexture;
                        s.onActive.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.15f);
                    }
                )
                .Sizes(Sizes.Button)
                .Size(
                    ControlSize.Icon,
                    (s, t) =>
                    {
                        int sz = GetScaledHeight(DesignTokens.Height.Default);
                        s.fixedWidth = sz;
                        s.fixedHeight = sz;
                        s.padding = GetSpacingOffset(0, 0);
                        s.alignment = TextAnchor.MiddleCenter;
                    }
                )
                .Build();
        }

        #endregion

        #region Input
        private void InputStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;
            int radius = GetScaledBorderRadius(r);

            _baseInputStyle = Style(StyleComponentType.Input)
                .FontScale(DesignTokens.FontScale.SM)
                .Padding(DesignTokens.Padding.Input.Horizontal, DesignTokens.Padding.Input.Vertical)
                .Border(DesignTokens.Radius.MD)
                .FixedHeight(DesignTokens.Height.Default)
                .Margin(DesignTokens.Spacing.None, DesignTokens.Spacing.SM)
                .Apply(
                    (s, t) =>
                    {
                        s.clipping = TextClipping.Clip;
                        s.contentOffset = new Vector2(2f, 0f);
                        s.wordWrap = false;
                        s.normal.textColor = s.hover.textColor = s.focused.textColor = s.active.textColor = t.Text;
                    }
                )
                .Sizes(Sizes.Input)
                .Build();

            Style(StyleComponentType.PasswordField)
                .Apply((s, t) => s.contentOffset = new Vector2(2f, 2f))
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        s.normal.background = CreateBorderedRoundedRectTexture(w, h, radius, t.Base, t.Border, 1.5f);
                        s.focused.background = CreateFocusRingTexture(w, h, radius, t.Accent, 2f);
                    }
                )
                .Variant(
                    ControlVariant.Ghost,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.focused.background = CreateFocusRingTexture(w, h, radius, t.Accent, 1.5f);
                    }
                )
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        var bg = IsDark(t.Base) ? Color.Lerp(t.Secondary, t.Elevated, 0.4f) : Color.Lerp(t.Secondary, t.Base, 0.3f);
                        var solidFactory = SolidTex(w, h, radius);
                        s.normal.background = solidFactory(bg);
                        s.focused.background = CreateFocusRingTexture(w, h, radius, t.Accent, 2f);
                    }
                )
                .Sizes(Sizes.Input)
                .Build();
        }
        #endregion

        #region TextArea
        private void TextAreaStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int br = GetScaledBorderRadius(DesignTokens.Radius.MD);

            Style(StyleComponentType.TextArea)
                .Apply((s, t) =>
                {
                    s.normal.textColor = t.Text;
                    s.wordWrap = true;
                    s.padding = GetSpacingOffset(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM);
                })
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        s.normal.background = CreateBorderedRoundedRectTexture(w, w, br, t.Base, t.Border, 1.5f);
                        s.focused.background = CreateFocusRingTexture(w, w, br, t.Accent, 2f);
                        s.border = new UnityHelpers.RectOffset(br, br, br, br);
                    }
                )
                .Variant(
                    ControlVariant.Ghost,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.focused.background = CreateFocusRingTexture(w, w, br, t.Accent, 1.5f);
                    }
                )
                .Sizes(Sizes.TextArea)
                .Build();
        }
        #endregion

        #region Label
        private void LabelStyles()
        {
            _baseLabelStyle = Style(StyleComponentType.Label)
                .FontScale(DesignTokens.FontScale.SM)
                .Padding(DesignTokens.Spacing.None, DesignTokens.Spacing.XS)
                .Align(TextAnchor.UpperLeft)
                .WordWrap(true)
                .RichText(true)
                .Apply(
                    (s, t) =>
                    {
                        s.normal.background = null;
                        s.normal.textColor = t.Text;
                    }
                )
                .Sizes(Sizes.FontOnly)
                .Build();

            Style(StyleComponentType.ChartAxis).Sizes(Sizes.FontOnly).Build();
            Style(StyleComponentType.SectionHeader).Sizes(Sizes.FontOnly).Build();
            Style(StyleComponentType.CardTitle).Sizes(Sizes.FontOnly).Build();
            Style(StyleComponentType.CardDescription).Sizes(Sizes.FontOnly).Build();
        }
        #endregion

        #region ProgressBar
        private void ProgressBarStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.ProgressBar.TextureHeight;
            int r = GetScaledBorderRadius(DesignTokens.Radius.LG);

            _progressBarStyle = Style(StyleComponentType.ProgressBar)
                .Border(DesignTokens.Radius.LG)
                .Margin(DesignTokens.Spacing.None, DesignTokens.Spacing.SM)
                .FixedHeight(DesignTokens.ProgressBar.Height)
                .Stretch(true, false)
                .Apply((s, t) => 
                {
                    s.normal.background = CreateSolidTexture(t.Secondary);
                    s.hover = s.active = s.normal;
                })
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        s.normal.background = CreateSolidTexture(t.Accent);
                    }
                )
                .Variant(
                    ControlVariant.Destructive,
                    (s, t) =>
                    {
                         s.normal.background = CreateSolidTexture(t.Destructive);
                         s.normal.textColor = t.ButtonDestructiveFg;
                    }
                )
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        s.normal.background = CreateRoundedOutlineTexture(w, h, r, t.Border, 1.5f);
                        s.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
                    }
                )
                .Variant(ControlVariant.Ghost, (s, t) => s.normal.background = TransparentTexture)
                .Variant(
                    ControlVariant.Muted,
                    (s, t) =>
                    {
                        s.normal.background = CreateSolidTexture(t.Muted);
                    }
                )
                .Sizes(Sizes.ProgressBar)
                .Build();
        }
        #endregion

        #region Separator
        private void SeparatorStyles()
        {
            _separatorStyle = Style(StyleComponentType.Separator)
                .Background(SeparatorTexture)
                .Padding(DesignTokens.Spacing.None, DesignTokens.Spacing.None)
                .Border(0)
                .FixedHeight(Mathf.Max(1, Mathf.RoundToInt(DesignTokens.Separator.DefaultThickness * _guiHelper.uiScale)))
                .Margin(DesignTokens.Spacing.None, DesignTokens.Spacing.None)
                .Stretch(true, false)
                .Sizes(Sizes.Separator)
                .Build();
        }
        #endregion

        #region Tab
        private void TabStyles()
        {
            _tabsListStyle = Style(StyleComponentType.TabsList)
                .Apply((s, t) => s.normal.background = CreateSolidTexture(t.Secondary))
                .Padding(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS)
                .Border(DesignTokens.Radius.MD)
                .Sizes(Sizes.TabsList)
                .Build();

            _tabsTriggerStyle = Style(StyleComponentType.TabsTrigger)
                .Background(TransparentTexture)
                .FontScale(DesignTokens.FontScale.SM, FontStyle.Bold)
                .Padding(DesignTokens.Padding.Tab.Horizontal, DesignTokens.Padding.Tab.Vertical)
                .Border(DesignTokens.Radius.SM)
                .Align(TextAnchor.MiddleCenter)
                .Apply(
                    (s, t) =>
                    {
                        s.normal.textColor = t.Muted;
                        s.normal.background = TransparentTexture;
                        
                        var activeBg = CreateGradientRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, GetScaledBorderRadius(DesignTokens.Radius.SM), t.Base);
                        
                        s.onNormal.textColor = t.Text;
                        s.onNormal.background = activeBg;
                        s.active = s.onNormal;
                        
                        s.hover.textColor = t.Text;
                        s.hover.background = SolidTex(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, DesignTokens.Radius.SM)(Alpha(t.Accent, 0.1f));
                    }
                )
                .Sizes(Sizes.Item)
                .Build();

            Style(StyleComponentType.TabsContent)
                .Apply((s, t) => s.normal.background = TransparentTexture)
                .Sizes(Sizes.TabsContent)
                .Build();
        }
        #endregion

        #region Checkbox
        private void CheckboxStyles()
        {
            int sz = (int)DesignTokens.Checkbox.Size;
            float r = DesignTokens.Radius.SM;
            int radius = GetScaledBorderRadius(r);

            var outline = OutlineTex(sz, sz, r, 1.5f);
            var bordered = BorderedTex(sz, sz, r, 1.5f);

            Func<Color, Texture2D> checkboxTex = c => CreateGradientRoundedRectTexture(sz, sz, radius, c);

            _baseCheckboxStyle = Style(StyleComponentType.Checkbox)
                .FontScale(DesignTokens.FontScale.SM)
                .Padding(DesignTokens.Spacing.SM, DesignTokens.Spacing.XS)
                .Border(DesignTokens.Radius.SM)
                .Stretch(false, false)
                .Apply((s, t) =>
                {
                    Color offBg = Color.Lerp(t.Accent, t.Base, 0.85f);
                    Color offBorder = Color.Lerp(t.Accent, t.Border, 0.5f);
                    
                    s.normal.background = bordered(offBg, offBorder);
                    s.normal.textColor = t.Text;

                    s.hover.background = bordered(Color.Lerp(offBg, t.Accent, 0.15f), t.Accent);
                    s.hover.textColor = t.Text;

                    s.active.background = bordered(Color.Lerp(offBg, t.Accent, 0.25f), t.Accent);
                    s.active.textColor = t.Text;

                    s.focused.background = bordered(offBg, t.Accent);
                    s.focused.textColor = t.Text;

                    s.onNormal.background = checkboxTex(t.Accent);
                    s.onNormal.textColor = t.ButtonPrimaryFg;

                    s.onHover.background = checkboxTex(Color.Lerp(t.Accent, Color.white, 0.15f));
                    s.onHover.textColor = t.ButtonPrimaryFg;

                    s.onActive.background = checkboxTex(Color.Lerp(t.Accent, Color.black, 0.12f));
                    s.onActive.textColor = t.ButtonPrimaryFg;

                    s.onFocused.background = checkboxTex(t.Accent);
                    s.onFocused.textColor = t.ButtonPrimaryFg;
                })
                .Variant(
                    ControlVariant.Destructive,
                    (s, t) =>
                    {
                        Color offBg = Color.Lerp(t.Destructive, t.Base, 0.88f);
                        Color offBorder = Color.Lerp(t.Destructive, t.Border, 0.4f);
                        
                        s.normal.background = bordered(offBg, offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Color.Lerp(offBg, t.Destructive, 0.18f), t.Destructive);
                        s.hover.textColor = t.Text;

                        s.onNormal.background = checkboxTex(t.Destructive);
                        s.onNormal.textColor = t.ButtonDestructiveFg;
                        s.onHover.background = checkboxTex(Color.Lerp(t.Destructive, Color.white, 0.15f));
                        s.onHover.textColor = t.ButtonDestructiveFg;
                        s.onActive.background = checkboxTex(Color.Lerp(t.Destructive, Color.black, 0.12f));
                        s.onActive.textColor = t.ButtonDestructiveFg;
                    }
                )
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        Color offBorder = Color.Lerp(t.Border, t.Text, 0.2f);
                        
                        s.normal.background = outline(offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Alpha(t.Accent, 0.1f), t.Accent);
                        s.hover.textColor = t.Text;

                        s.onNormal.background = bordered(t.Accent, t.Accent);
                        s.onNormal.textColor = t.ButtonPrimaryFg;
                        s.onHover.background = bordered(Color.Lerp(t.Accent, Color.white, 0.12f), t.Accent);
                        s.onHover.textColor = t.ButtonPrimaryFg;
                    }
                )
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        Color offBg = IsDark(t.Base) ? Color.Lerp(t.Base, t.Secondary, 0.6f) : Color.Lerp(t.Secondary, t.Base, 0.3f);
                        Color offBorder = Color.Lerp(t.Secondary, t.Text, 0.25f);
                        
                        s.normal.background = bordered(offBg, offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Color.Lerp(offBg, t.Text, 0.12f), Color.Lerp(offBorder, t.Text, 0.15f));
                        s.hover.textColor = t.Text;

                        s.onNormal.background = checkboxTex(Color.Lerp(t.Secondary, t.Text, 0.4f));
                        s.onNormal.textColor = IsDark(t.Base) ? t.Base : t.Text;
                        s.onHover.background = checkboxTex(Color.Lerp(t.Secondary, t.Text, 0.5f));
                        s.onHover.textColor = IsDark(t.Base) ? t.Base : t.Text;
                    }
                )
                .Variant(
                    ControlVariant.Ghost,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.Muted;
                        s.hover.background = checkboxTex(Alpha(t.Text, 0.1f));
                        s.hover.textColor = t.Text;

                        s.onNormal.background = checkboxTex(Alpha(t.Accent, 0.2f));
                        s.onNormal.textColor = t.Accent;
                        s.onHover.background = checkboxTex(Alpha(t.Accent, 0.3f));
                        s.onHover.textColor = t.Accent;
                    }
                )
                .Variant(
                    ControlVariant.Link,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.ButtonLinkColor;
                        s.hover.background = TransparentTexture;
                        s.hover.textColor = Color.Lerp(t.ButtonLinkColor, Color.white, 0.2f);

                        s.onNormal.background = TransparentTexture;
                        s.onNormal.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.3f);
                        s.onHover.background = TransparentTexture;
                        s.onHover.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.5f);
                    }
                )
                .Variant(
                    ControlVariant.Muted,
                    (s, t) =>
                    {
                        Color offBg = Color.Lerp(t.Muted, t.Base, 0.85f);
                        Color offBorder = Color.Lerp(t.Muted, t.Border, 0.5f);
                        
                        s.normal.background = bordered(offBg, offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Color.Lerp(offBg, t.Muted, 0.15f), t.Muted);
                        s.hover.textColor = t.Text;

                        s.onNormal.background = checkboxTex(t.Muted);
                        s.onNormal.textColor = t.Base;
                        s.onHover.background = checkboxTex(Color.Lerp(t.Muted, Color.white, 0.15f));
                        s.onHover.textColor = t.Base;
                        s.onActive.background = checkboxTex(Color.Lerp(t.Muted, Color.black, 0.12f));
                        s.onActive.textColor = t.Base;
                    }
                )
                .Sizes(Sizes.FontOnly)
                .Size(
                    ControlSize.Icon,
                    (s, t) =>
                    {
                        int iconSz = GetScaledHeight(DesignTokens.Checkbox.Size);
                        s.fixedWidth = iconSz;
                        s.fixedHeight = iconSz;
                        s.padding = GetSpacingOffset(0, 0);
                    }
                )
                .Build();
        }
        #endregion

        #region Switch
        private void SwitchStyles()
        {
            int w = (int)DesignTokens.Switch.Width;
            int h = (int)DesignTokens.Switch.Height;
            float r = DesignTokens.Switch.Radius;
            int radius = GetScaledBorderRadius(r);

            var outline = OutlineTex(w, h, r, 1.5f);
            var bordered = BorderedTex(w, h, r, 1.5f);

            Func<Color, Texture2D> switchTex = c => CreateGradientRoundedRectTexture(w, h, radius, c);

            _baseSwitchStyle = Style(StyleComponentType.Switch)
                .FontScale(DesignTokens.FontScale.SM)
                .Padding(DesignTokens.Spacing.SM, DesignTokens.Spacing.None)
                .Border(DesignTokens.Switch.Radius)
                .Stretch(false, false)
                .Apply((s, t) =>
                {
                    Color offBg = Color.Lerp(t.Accent, t.Base, 0.85f);
                    Color offBorder = Color.Lerp(t.Accent, t.Border, 0.5f);
                    
                    s.normal.background = bordered(offBg, offBorder);
                    s.normal.textColor = t.Text;

                    s.hover.background = bordered(Color.Lerp(offBg, t.Accent, 0.15f), t.Accent);
                    s.hover.textColor = t.Text;

                    s.active.background = bordered(Color.Lerp(offBg, t.Accent, 0.25f), t.Accent);
                    s.active.textColor = t.Text;

                    s.focused.background = bordered(offBg, t.Accent);
                    s.focused.textColor = t.Text;

                    s.onNormal.background = switchTex(t.Accent);
                    s.onNormal.textColor = t.ButtonPrimaryFg;

                    s.onHover.background = switchTex(Color.Lerp(t.Accent, Color.white, 0.15f));
                    s.onHover.textColor = t.ButtonPrimaryFg;

                    s.onActive.background = switchTex(Color.Lerp(t.Accent, Color.black, 0.12f));
                    s.onActive.textColor = t.ButtonPrimaryFg;

                    s.onFocused.background = switchTex(t.Accent);
                    s.onFocused.textColor = t.ButtonPrimaryFg;
                })
                .Variant(
                    ControlVariant.Destructive,
                    (s, t) =>
                    {
                        Color offBg = Color.Lerp(t.Destructive, t.Base, 0.88f);
                        Color offBorder = Color.Lerp(t.Destructive, t.Border, 0.4f);
                        
                        s.normal.background = bordered(offBg, offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Color.Lerp(offBg, t.Destructive, 0.18f), t.Destructive);
                        s.hover.textColor = t.Text;

                        s.onNormal.background = switchTex(t.Destructive);
                        s.onNormal.textColor = t.ButtonDestructiveFg;
                        s.onHover.background = switchTex(Color.Lerp(t.Destructive, Color.white, 0.15f));
                        s.onHover.textColor = t.ButtonDestructiveFg;
                        s.onActive.background = switchTex(Color.Lerp(t.Destructive, Color.black, 0.12f));
                        s.onActive.textColor = t.ButtonDestructiveFg;
                    }
                )
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        Color offBorder = Color.Lerp(t.Border, t.Text, 0.2f);
                        
                        s.normal.background = outline(offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Alpha(t.Accent, 0.1f), t.Accent);
                        s.hover.textColor = t.Text;

                        s.onNormal.background = bordered(t.Accent, t.Accent);
                        s.onNormal.textColor = t.ButtonPrimaryFg;
                        s.onHover.background = bordered(Color.Lerp(t.Accent, Color.white, 0.12f), t.Accent);
                        s.onHover.textColor = t.ButtonPrimaryFg;
                    }
                )
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        Color offBg = IsDark(t.Base) ? Color.Lerp(t.Base, t.Secondary, 0.6f) : Color.Lerp(t.Secondary, t.Base, 0.3f);
                        Color offBorder = Color.Lerp(t.Secondary, t.Text, 0.25f);
                        
                        s.normal.background = bordered(offBg, offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Color.Lerp(offBg, t.Text, 0.12f), Color.Lerp(offBorder, t.Text, 0.15f));
                        s.hover.textColor = t.Text;

                        s.onNormal.background = switchTex(Color.Lerp(t.Secondary, t.Text, 0.4f));
                        s.onNormal.textColor = IsDark(t.Base) ? t.Base : t.Text;
                        s.onHover.background = switchTex(Color.Lerp(t.Secondary, t.Text, 0.5f));
                        s.onHover.textColor = IsDark(t.Base) ? t.Base : t.Text;
                    }
                )
                .Variant(
                    ControlVariant.Ghost,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.Muted;
                        s.hover.background = switchTex(Alpha(t.Text, 0.1f));
                        s.hover.textColor = t.Text;

                        s.onNormal.background = switchTex(Alpha(t.Accent, 0.2f));
                        s.onNormal.textColor = t.Accent;
                        s.onHover.background = switchTex(Alpha(t.Accent, 0.3f));
                        s.onHover.textColor = t.Accent;
                    }
                )
                .Variant(
                    ControlVariant.Link,
                    (s, t) =>
                    {
                        s.normal.background = TransparentTexture;
                        s.normal.textColor = t.ButtonLinkColor;
                        s.hover.background = TransparentTexture;
                        s.hover.textColor = Color.Lerp(t.ButtonLinkColor, Color.white, 0.2f);

                        s.onNormal.background = TransparentTexture;
                        s.onNormal.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.3f);
                        s.onHover.background = TransparentTexture;
                        s.onHover.textColor = Color.Lerp(t.ButtonLinkColor, t.Accent, 0.5f);
                    }
                )
                .Variant(
                    ControlVariant.Muted,
                    (s, t) =>
                    {
                        Color offBg = Color.Lerp(t.Muted, t.Base, 0.85f);
                        Color offBorder = Color.Lerp(t.Muted, t.Border, 0.5f);
                        
                        s.normal.background = bordered(offBg, offBorder);
                        s.normal.textColor = t.Text;
                        s.hover.background = bordered(Color.Lerp(offBg, t.Muted, 0.15f), t.Muted);
                        s.hover.textColor = t.Text;

                        s.onNormal.background = switchTex(t.Muted);
                        s.onNormal.textColor = t.Base;
                        s.onHover.background = switchTex(Color.Lerp(t.Muted, Color.white, 0.15f));
                        s.onHover.textColor = t.Base;
                        s.onActive.background = switchTex(Color.Lerp(t.Muted, Color.black, 0.12f));
                        s.onActive.textColor = t.Base;
                    }
                )
                .Sizes(Sizes.FontOnly)
                .Size(
                    ControlSize.Icon,
                    (s, t) =>
                    {
                        int iconSz = GetScaledHeight(DesignTokens.Switch.Height);
                        s.fixedWidth = GetScaledHeight(DesignTokens.Switch.Width);
                        s.fixedHeight = iconSz;
                        s.padding = GetSpacingOffset(0, 0);
                    }
                )
                .Build();
        }
        #endregion

        #region Badge
        private void BadgeStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.XL;

            var solid = SolidTex(w, h, r);
            var outline = OutlineTex(w, h, r);

            _baseBadgeStyle = Style(StyleComponentType.Badge)
                .FontScale(DesignTokens.FontScale.XS, FontStyle.Bold)
                .Padding(DesignTokens.Padding.Badge.Horizontal, DesignTokens.Padding.Badge.Vertical)
                .Border(DesignTokens.Radius.XL)
                .Align(TextAnchor.MiddleCenter)
                .Apply((s, t) =>
                {
                    s.normal.textColor = t.ButtonPrimaryFg;
                    s.normal.background = solid(t.ButtonPrimaryBg);
                })
                .Variant(
                    ControlVariant.Destructive,
                    (s, t) =>
                    {
                        s.normal.background = solid(t.Destructive);
                        s.normal.textColor = t.ButtonDestructiveFg;
                    }
                )
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) =>
                    {
                        s.normal.background = solid(t.Secondary);
                        s.normal.textColor = t.ButtonSecondaryFg;
                    }
                )
                .Variant(
                    ControlVariant.Outline,
                    (s, t) =>
                    {
                        s.normal.background = outline(t.Border);
                        s.normal.textColor = t.Text;
                    }
                )
                .Variant(
                    ControlVariant.Muted,
                    (s, t) =>
                    {
                        s.normal.background = solid(t.Muted);
                        s.normal.textColor = t.Text;
                    }
                )
                .Sizes(Sizes.Badge)
                .Build();
        }
        #endregion

        #region Table
        private void TableStyles()
        {
            _baseTableStyle = Style(StyleComponentType.Table)
                .Border(DesignTokens.Radius.LG)
                .Apply((s, t) => s.normal.background = CreateBorderedRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, GetScaledBorderRadius(DesignTokens.Radius.LG), t.Base, t.Border, 1f))
                .Sizes(Sizes.Table)
                .Build();

            _tableCellStyle = Style(StyleComponentType.TableCell)
                .Background(TableCellTexture)
                .FontScale(DesignTokens.FontScale.SM)
                .Padding(DesignTokens.Padding.Table.CellH, DesignTokens.Padding.Table.CellV)
                .Align(TextAnchor.MiddleLeft)
                .Clipping(TextClipping.Clip)
                .Apply((s, t) => s.normal.textColor = t.Text)
                .Sizes(Sizes.TableCell)
                .Build();

            Style(StyleComponentType.TableHeader).Sizes(Sizes.TableHeader).Build();
        }
        #endregion

        #region Calendar
        private void CalendarStyles()
        {
            _baseCalendarStyle = Style(StyleComponentType.Calendar).Background(CalendarBackgroundTexture).Padding(DesignTokens.Spacing.MD, DesignTokens.Spacing.MD).Border(DesignTokens.Radius.LG).Sizes(Sizes.Container).Build();

            _calendarDayStyle = Style(StyleComponentType.CalendarDay).Background(CalendarDayTexture).FontScale(DesignTokens.FontScale.SM).Align(TextAnchor.MiddleCenter).Apply((s, t) => s.normal.textColor = s.hover.textColor = t.Text).Sizes(Sizes.CalendarDay).Build();

            Style(StyleComponentType.DatePicker).Sizes(Sizes.Container).Build();
            Style(StyleComponentType.CalendarDaySelected).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.CalendarDayOutsideMonth).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.CalendarDayToday).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.CalendarDayInRange).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.DatePickerDay).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.DatePickerDaySelected).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.DatePickerDayOutsideMonth).Sizes(Sizes.CalendarDay).Build();
            Style(StyleComponentType.DatePickerDayToday).Sizes(Sizes.CalendarDay).Build();
        }
        #endregion

        #region Dialog
        private void DialogStyles()
        {
            _dialogContentStyle = Style(StyleComponentType.Dialog)
                .Padding(DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical)
                .Border(DesignTokens.Radius.XL)
                .Apply(
                    (s, t) =>
                        s.normal.background = CreateBorderedRoundedRectTexture(
                            DesignTokens.TextureSize.XL,
                            DesignTokens.TextureSize.XL,
                            GetScaledBorderRadius(DesignTokens.Radius.XL),
                            t.Base,
                            t.Border,
                            1f
                        )
                )
                .Sizes(Sizes.Dialog)
                .Build();
        }
        #endregion

        #region Card
        private void CardStyles()
        {
            _cardStyle = Style(StyleComponentType.Card)
                .Apply((s, t) => 
                {
                     s.normal.background = CreateBorderedRoundedRectTexture(
                        DesignTokens.TextureSize.Default, 
                        DesignTokens.TextureSize.Default, 
                        GetScaledBorderRadius(DesignTokens.Radius.LG), 
                        t.Base, 
                        t.Border, 
                        1f
                     );
                })
                .Padding(DesignTokens.Padding.Card.Horizontal, DesignTokens.Padding.Card.Vertical)
                .Border(DesignTokens.Radius.LG)
                .Stretch(false, false)
                .Apply((s, t) => s.normal.textColor = t.Text)
                .Sizes(Sizes.Card)
                .Build();

            Style(StyleComponentType.CardHeader).Sizes(Sizes.Container).Build();
            Style(StyleComponentType.CardContent).Sizes(Sizes.Container).Build();
            Style(StyleComponentType.CardFooter).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region Menu
        private void MenuStyles()
        {
            _menuBarStyle = Style(StyleComponentType.MenuBar).Background(TransparentTexture).Padding(DesignTokens.Spacing.XS, DesignTokens.Spacing.None).FixedHeight(DesignTokens.Height.Default).Align(TextAnchor.MiddleLeft).Stretch(false, false).Sizes(Sizes.MenuBar).Build();

            Style(StyleComponentType.MenuBarItem).Sizes(Sizes.Item).Build();
            Style(StyleComponentType.DropdownMenuItem).Sizes(Sizes.Item).Build();
            Style(StyleComponentType.DropdownMenu).Sizes(Sizes.Container).Build();
            Style(StyleComponentType.MenuDropdown).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region Select
        private void SelectStyles()
        {
            Style(StyleComponentType.SelectItem).Sizes(Sizes.Item).Build();
            Style(StyleComponentType.SelectContent).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region Popover
        private void PopoverStyles()
        {
            Style(StyleComponentType.Popover).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region Chart
        private void ChartStyles()
        {
            _chartContainerStyle = Style(StyleComponentType.Chart).Background(ChartContainerTexture).Padding(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG).Border(DesignTokens.Radius.LG).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region AnimatedBox
        private void AnimatedBoxStyles()
        {
            Style(StyleComponentType.AnimatedBox).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region Toast
        private void ToastStyles()
        {
            Style(StyleComponentType.Toast).Sizes(Sizes.Container).Build();
            Style(StyleComponentType.ToastTitle).Sizes(Sizes.Container).Build();
            Style(StyleComponentType.ToastDescription).Sizes(Sizes.Container).Build();
        }
        #endregion

        #region Slider
        private void SliderStyles()
        {
            _baseSliderStyle = Style(StyleComponentType.SliderTrack)
                .Apply((s, t) => s.normal.background = CreateSolidTexture(t.Secondary))
                .Border(DesignTokens.Radius.Full)
                .FixedHeight(DesignTokens.Slider.TrackDefault)
                .Stretch(true, false)
                .Apply((s, t) => s.normal.textColor = t.Text)
                .Sizes(Sizes.SliderTrack)
                .Build();

            Style(StyleComponentType.SliderThumb).Sizes(Sizes.SliderThumb).Build();

            Style(StyleComponentType.SliderFill)
                .Variant(ControlVariant.Default, (s, t) => s.normal.background = CreateSolidTexture(t.Accent))
                .Variant(ControlVariant.Destructive, (s, t) => s.normal.background = CreateSolidTexture(t.Destructive))
                .Variant(
                    ControlVariant.Secondary,
                    (s, t) => s.normal.background = CreateSolidTexture(t.Secondary)
                )
                .Variant(ControlVariant.Muted, (s, t) => s.normal.background = CreateSolidTexture(t.Muted))
                .Build();
        }
        #endregion

        #region Avatar
        private void AvatarStyles()
        {
            int size = GetScaledHeight(DesignTokens.Height.Default);
            int radius = size / 2;

            _avatarStyle = Style(StyleComponentType.Avatar)
                .Border(radius)
                .Align(TextAnchor.MiddleCenter)
                .FixedSize(DesignTokens.Height.Default)
                .Apply(
                    (s, t) =>
                    {
                        s.normal.background = CreateSolidTexture(t.Muted);
                        s.normal.textColor = t.Text;
                        s.fontStyle = FontStyle.Bold;
                    }
                )
                .Build();
        }
        #endregion

        #region Dropdown
        private void DropdownStyles()
        {
            _dropdownContentStyle = Style(StyleComponentType.DropdownMenu)
                .Apply((s, t) => 
                {
                    s.normal.background = CreateBorderedRoundedRectTexture(
                        DesignTokens.TextureSize.Default, 64, 
                        GetScaledBorderRadius(DesignTokens.Radius.MD), 
                        t.Base, 
                        t.Border, 
                        1f);
                })
                .Padding(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS)
                .Border(DesignTokens.Radius.MD)
                .Build();

            _dropdownItemStyle = Style(StyleComponentType.DropdownMenuItem)
                .Background(TransparentTexture)
                .FontScale(DesignTokens.FontScale.SM)
                .Padding(DesignTokens.Padding.Dropdown.ItemH, DesignTokens.Padding.Dropdown.ItemV)
                .Align(TextAnchor.MiddleLeft)
                .Apply(
                    (s, t) =>
                    {
                        s.normal.textColor = t.Text;
                        s.hover.textColor = s.active.textColor = t.ButtonPrimaryFg;
                        s.hover.background = s.active.background = CreateSolidTexture(t.Accent);
                    }
                )
                .Sizes(Sizes.Item)
                .Build();
        }
        #endregion
    }
}
