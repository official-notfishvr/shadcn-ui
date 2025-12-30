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
        #region Registration Entry Point
        private void RegisterDefaultStyles()
        {
            RegisterButtonStyles();
            RegisterToggleStyles();
            RegisterInputStyles();
            RegisterTextAreaStyles();
            RegisterLabelStyles();
            RegisterProgressBarStyles();
            RegisterSeparatorStyles();
            RegisterTabStyles();
            RegisterCheckboxStyles();
            RegisterSwitchStyles();
            RegisterBadgeStyles();
            RegisterTableStyles();
            RegisterCalendarAndDatePickerStyles();
            RegisterDialogStyles();
            RegisterCardStyles();
            RegisterMenuStyles();
            RegisterSelectStyles();
            RegisterPopoverStyles();
            RegisterChartStyles();
            RegisterAnimatedBoxStyles();
            RegisterToastStyles();
            RegisterSliderStyles();
        }
        #endregion

        #region Core Registration Methods
        private void RegisterVariant(StyleComponentType type, ControlVariant variant, StyleModifier mod) => Registry.RegisterVariant(type, variant, mod);

        private void RegisterSize(StyleComponentType type, ControlSize size, StyleModifier mod) => Registry.RegisterSize(type, size, mod);
        #endregion

        #region Size Configuration
        private struct SizeValues
        {
            public float FontScale;
            public float PadH;
            public float PadV;
            public float Height;
            public float Width;

            public static SizeValues Font(float scale) =>
                new SizeValues
                {
                    FontScale = scale,
                    PadH = -1,
                    PadV = -1,
                };

            public static SizeValues Pad(float h, float v) => new SizeValues { PadH = h, PadV = v };

            public static SizeValues Full(float font, float padH, float padV, float height) =>
                new SizeValues
                {
                    FontScale = font,
                    PadH = padH,
                    PadV = padV,
                    Height = height,
                };

            public static SizeValues Square(float font, float size) =>
                new SizeValues
                {
                    FontScale = font,
                    Height = size,
                    Width = size,
                    PadH = -1,
                    PadV = -1,
                };

            public static SizeValues H(float height) =>
                new SizeValues
                {
                    Height = height,
                    PadH = -1,
                    PadV = -1,
                };
        }

        private struct SizeConfig
        {
            public SizeValues Mini;
            public SizeValues Small;
            public SizeValues Default;
            public SizeValues Large;
        }

        private void RegisterSizeSet(StyleComponentType type, SizeConfig config)
        {
            RegisterSize(type, ControlSize.Mini, (s, t, g) => ApplySize(s, config.Mini));
            RegisterSize(type, ControlSize.Small, (s, t, g) => ApplySize(s, config.Small));
            RegisterSize(type, ControlSize.Default, (s, t, g) => ApplySize(s, config.Default));
            RegisterSize(type, ControlSize.Large, (s, t, g) => ApplySize(s, config.Large));
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

        #region Predefined Size Configs
        private static class Sizes
        {
            public static readonly SizeConfig Button = new SizeConfig
            {
                Mini = SizeValues.Full(DesignTokens.FontScale.XS, DesignTokens.Padding.ButtonMiniH, DesignTokens.Padding.ButtonMiniV, DesignTokens.Height.Mini),
                Small = SizeValues.Full(DesignTokens.FontScale.XS, DesignTokens.Padding.ButtonSmallH, DesignTokens.Padding.ButtonSmallV, DesignTokens.Height.Small),
                Default = SizeValues.Full(DesignTokens.FontScale.SM, DesignTokens.Padding.ButtonDefaultH, DesignTokens.Padding.ButtonDefaultV, DesignTokens.Height.Default),
                Large = SizeValues.Full(DesignTokens.FontScale.MD, DesignTokens.Padding.ButtonLargeH, DesignTokens.Padding.ButtonLargeV, DesignTokens.Height.Large),
            };

            public static readonly SizeConfig Toggle = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS * 0.85f,
                    PadH = DesignTokens.Spacing.XS,
                    PadV = DesignTokens.Spacing.None,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.XXS,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.XS,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.LG,
                    PadH = DesignTokens.Spacing.MD,
                    PadV = DesignTokens.Spacing.SM,
                },
            };

            public static readonly SizeConfig Input = new SizeConfig
            {
                Mini = SizeValues.Full(DesignTokens.FontScale.XS * 0.9f, DesignTokens.Spacing.SM, DesignTokens.Spacing.XXS, DesignTokens.Height.Mini),
                Small = SizeValues.Full(DesignTokens.FontScale.XS, DesignTokens.Spacing.SM, DesignTokens.Spacing.XS, DesignTokens.Height.Small),
                Default = SizeValues.Full(DesignTokens.FontScale.SM, DesignTokens.Padding.InputH, DesignTokens.Padding.InputV, DesignTokens.Height.Default),
                Large = SizeValues.Full(DesignTokens.FontScale.MD, DesignTokens.Spacing.LG, DesignTokens.Spacing.MD, DesignTokens.Height.Large),
            };

            public static readonly SizeConfig TextArea = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS * 0.9f,
                    PadH = DesignTokens.Spacing.XS,
                    PadV = DesignTokens.Spacing.XXS,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.XS,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.MD,
                    PadV = DesignTokens.Spacing.SM,
                },
            };

            public static readonly SizeConfig FontOnly = new SizeConfig
            {
                Mini = SizeValues.Font(DesignTokens.FontScale.XS),
                Small = SizeValues.Font(DesignTokens.FontScale.XS),
                Default = SizeValues.Font(DesignTokens.FontScale.SM),
                Large = SizeValues.Font(DesignTokens.FontScale.MD),
            };

            public static readonly SizeConfig Item = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.XS,
                    PadV = DesignTokens.Spacing.XXS,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.XS,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Spacing.MD,
                    PadV = DesignTokens.Spacing.SM,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.LG,
                    PadV = DesignTokens.Spacing.SM,
                },
            };

            public static readonly SizeConfig Container = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.XS,
                    PadV = DesignTokens.Spacing.XS,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Spacing.LG,
                    PadV = DesignTokens.Spacing.LG,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.XL,
                    PadV = DesignTokens.Spacing.XL,
                },
            };

            public static readonly SizeConfig CalendarDay = new SizeConfig
            {
                Mini = SizeValues.Square(DesignTokens.FontScale.XS, DesignTokens.CalendarDay.Mini),
                Small = SizeValues.Square(DesignTokens.FontScale.XS, DesignTokens.CalendarDay.Small),
                Default = SizeValues.Square(DesignTokens.FontScale.SM, DesignTokens.CalendarDay.Default),
                Large = SizeValues.Square(DesignTokens.FontScale.MD, DesignTokens.CalendarDay.Large),
            };

            public static readonly SizeConfig ProgressBar = new SizeConfig
            {
                Mini = SizeValues.H(DesignTokens.Spacing.XXS),
                Small = SizeValues.H(DesignTokens.Spacing.XS),
                Default = SizeValues.H(DesignTokens.Spacing.SM),
                Large = SizeValues.H(DesignTokens.Spacing.MD),
            };

            public static readonly SizeConfig Separator = new SizeConfig
            {
                Mini = SizeValues.H(DesignTokens.Separator.DefaultThickness),
                Small = SizeValues.H(DesignTokens.Separator.DefaultThickness),
                Default = SizeValues.H(DesignTokens.Separator.LargeThickness),
                Large = SizeValues.H(DesignTokens.Separator.LargeThickness * 2),
            };

            public static readonly SizeConfig TabsList = new SizeConfig
            {
                Mini = SizeValues.Pad(DesignTokens.Spacing.None, DesignTokens.Spacing.None),
                Small = SizeValues.Pad(DesignTokens.Spacing.XXS, DesignTokens.Spacing.XXS),
                Default = SizeValues.Pad(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS),
                Large = SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
            };

            public static readonly SizeConfig TabsContent = new SizeConfig
            {
                Mini = SizeValues.Pad(DesignTokens.Spacing.XS, DesignTokens.Spacing.XS),
                Small = SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
                Default = SizeValues.Pad(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG),
                Large = SizeValues.Pad(DesignTokens.Spacing.XL, DesignTokens.Spacing.XL),
            };

            public static readonly SizeConfig Badge = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS * 0.8f,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.None,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS * 0.85f,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.XXS,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Padding.BadgeH,
                    PadV = DesignTokens.Padding.BadgeV,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.MD,
                    PadV = DesignTokens.Spacing.SM,
                },
            };

            public static readonly SizeConfig Table = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.XS,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Padding.TableCellH,
                    PadV = DesignTokens.Padding.TableCellV,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.XL,
                    PadV = DesignTokens.Spacing.LG,
                },
            };

            public static readonly SizeConfig TableHeader = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM * 0.97f,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Padding.TableCellH,
                    PadV = DesignTokens.Spacing.SM,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.LG,
                    PadV = DesignTokens.Spacing.MD,
                },
            };

            public static readonly SizeConfig TableCell = new SizeConfig
            {
                Mini = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS * 0.9f,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Small = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.XS,
                    PadH = DesignTokens.Spacing.SM,
                    PadV = DesignTokens.Spacing.SM,
                },
                Default = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.SM,
                    PadH = DesignTokens.Padding.TableCellH,
                    PadV = DesignTokens.Spacing.SM,
                },
                Large = new SizeValues
                {
                    FontScale = DesignTokens.FontScale.MD,
                    PadH = DesignTokens.Spacing.LG,
                    PadV = DesignTokens.Spacing.MD,
                },
            };

            public static readonly SizeConfig Dialog = new SizeConfig
            {
                Mini = SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
                Small = SizeValues.Pad(DesignTokens.Spacing.LG, DesignTokens.Spacing.LG),
                Default = SizeValues.Pad(DesignTokens.Padding.CardH, DesignTokens.Padding.CardV),
                Large = SizeValues.Pad(DesignTokens.Spacing.XXL, DesignTokens.Spacing.XXL),
            };

            public static readonly SizeConfig Card = new SizeConfig
            {
                Mini = SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.SM),
                Small = SizeValues.Pad(DesignTokens.Spacing.MD, DesignTokens.Spacing.MD),
                Default = SizeValues.Pad(DesignTokens.Padding.CardH, DesignTokens.Padding.CardV),
                Large = SizeValues.Pad(DesignTokens.Spacing.XXL, DesignTokens.Spacing.XXL),
            };

            public static readonly SizeConfig MenuBar = new SizeConfig
            {
                Mini = SizeValues.Pad(DesignTokens.Spacing.None, DesignTokens.Spacing.None),
                Small = SizeValues.Pad(DesignTokens.Spacing.XXS, DesignTokens.Spacing.None),
                Default = SizeValues.Pad(DesignTokens.Spacing.XS, DesignTokens.Spacing.None),
                Large = SizeValues.Pad(DesignTokens.Spacing.SM, DesignTokens.Spacing.None),
            };

            public static readonly SizeConfig SliderTrack = new SizeConfig
            {
                Mini = SizeValues.H(DesignTokens.Slider.TrackMini),
                Small = SizeValues.H(DesignTokens.Slider.TrackSmall),
                Default = SizeValues.H(DesignTokens.Slider.TrackDefault),
                Large = SizeValues.H(DesignTokens.Slider.TrackLarge),
            };

            public static readonly SizeConfig SliderThumb = new SizeConfig
            {
                Mini = SizeValues.Square(0, DesignTokens.Slider.ThumbMini),
                Small = SizeValues.Square(0, DesignTokens.Slider.ThumbSmall),
                Default = SizeValues.Square(0, DesignTokens.Slider.ThumbDefault),
                Large = SizeValues.Square(0, DesignTokens.Slider.ThumbLarge),
            };
        }
        #endregion

        #region Texture Helpers
        private Func<Color, Texture2D> Solid(int w, int h, float r) => c => CreateRoundedRectTexture(w, h, GetScaledBorderRadius(r), c);

        private Func<Color, Texture2D> Gradient(int w, int h, float r) => c => CreateGradientRoundedRectWithShadowTexture(w, h, GetScaledBorderRadius(r), Color.Lerp(c, Color.white, 0.05f), c, DesignTokens.Effects.ShadowLight, DesignTokens.Effects.ShadowBlurSM);

        private Func<Color, Texture2D> Outline(int w, int h, float r, float t = 1.5f) => c => CreateRoundedOutlineTexture(w, h, GetScaledBorderRadius(r), c, t);

        private Func<Color, Color, Texture2D> Bordered(int w, int h, float r, float t = 1.5f) => (f, b) => CreateBorderedRoundedRectTexture(w, h, GetScaledBorderRadius(r), f, b, t);

        private Func<Color, Texture2D> Focus(int w, int h, float r) => c => CreateFocusRingTexture(w, h, GetScaledBorderRadius(r), c, DesignTokens.Effects.FocusRingThickness);

        private static Color Alpha(Color c, float a) => new Color(c.r, c.g, c.b, a);
        #endregion


        #region Button Styles
        private void RegisterButtonStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;

            var solid = Solid(w, h, r);
            var gradient = Gradient(w, h, r);
            var outline = Outline(w, h, r);
            var bordered = Bordered(w, h, r);
            var focus = Focus(w, h, r);

            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = gradient(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                    s.hover.background = gradient(GetHoverColor(t.Destructive, true));
                    s.hover.textColor = t.ButtonDestructiveFg;
                    s.active.background = solid(Color.Lerp(t.Destructive, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.active.textColor = t.ButtonDestructiveFg;
                    s.focused.background = focus(t.Destructive);
                    s.focused.textColor = t.ButtonDestructiveFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = outline(t.Border);
                    s.normal.textColor = t.Text;
                    s.hover.background = bordered(Alpha(t.Secondary, 0.4f), t.Border);
                    s.hover.textColor = t.Text;
                    s.active.background = bordered(Alpha(t.Secondary, 0.6f), t.Border);
                    s.active.textColor = t.Text;
                    s.focused.background = focus(t.Accent);
                    s.focused.textColor = t.Text;
                }
            );

            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    Color sec = Color.Lerp(t.Secondary, t.Text, 0.08f);
                    s.normal.background = CreateGradientRoundedRectWithShadowTexture(w, h, GetScaledBorderRadius(r), Color.Lerp(sec, Color.white, 0.03f), sec, DesignTokens.Effects.ShadowLight, DesignTokens.Effects.ShadowBlurSM);
                    s.normal.textColor = t.ButtonSecondaryFg;
                    s.hover.background = CreateGradientRoundedRectTexture(w, h, GetScaledBorderRadius(r), Color.Lerp(sec, Color.white, 0.1f), sec);
                    s.hover.textColor = t.ButtonSecondaryFg;
                    s.active.background = solid(Color.Lerp(sec, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.active.textColor = t.ButtonSecondaryFg;
                    s.focused.background = focus(t.Accent);
                    s.focused.textColor = t.ButtonSecondaryFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    Color ghost = Alpha(t.Secondary, 0.35f);
                    s.normal.background = transparentTexture;
                    s.normal.textColor = t.ButtonGhostFg;
                    s.hover.background = solid(ghost);
                    s.hover.textColor = t.ButtonGhostFg;
                    s.active.background = solid(Color.Lerp(ghost, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.active.textColor = t.ButtonGhostFg;
                    s.focused.background = focus(t.Accent);
                    s.focused.textColor = t.ButtonGhostFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Link,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.normal.textColor = t.ButtonLinkColor;
                    s.hover.background = transparentTexture;
                    s.hover.textColor = Color.Lerp(t.ButtonLinkColor, Color.white, 0.2f);
                    s.active.background = transparentTexture;
                    s.active.textColor = Color.Lerp(t.ButtonLinkColor, Color.black, 0.25f);
                    s.focused.background = transparentTexture;
                    s.focused.textColor = t.ButtonLinkColor;
                    s.padding = GetSpacingOffset(DesignTokens.Spacing.None, DesignTokens.Spacing.XXS);
                    s.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                }
            );

            RegisterSizeSet(StyleComponentType.Button, Sizes.Button);

            RegisterSize(
                StyleComponentType.Button,
                ControlSize.Icon,
                (s, t, _) =>
                {
                    int sz = GetScaledHeight(DesignTokens.Height.Default);
                    s.fixedWidth = sz;
                    s.fixedHeight = sz;
                    s.padding = GetSpacingOffset(DesignTokens.Spacing.None, DesignTokens.Spacing.None);
                }
            );
        }
        #endregion

        #region Toggle Styles
        private void RegisterToggleStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;

            var solid = Solid(w, h, r);
            var outline = Outline(w, h, r, 2f);
            var bordered = Bordered(w, h, r, 2f);

            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = solid(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                    s.hover.background = solid(GetHoverColor(t.Destructive, true));
                    s.hover.textColor = t.ButtonDestructiveFg;
                    s.active.background = solid(Color.Lerp(t.Destructive, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.active.textColor = t.ButtonDestructiveFg;
                    s.onNormal.background = solid(Color.Lerp(t.Destructive, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.onNormal.textColor = t.ButtonDestructiveFg;
                    s.onHover.background = solid(GetHoverColor(Color.Lerp(t.Destructive, Color.black, DesignTokens.Effects.ActiveDarken), true));
                    s.onHover.textColor = t.ButtonDestructiveFg;
                    s.onActive.background = solid(Color.Lerp(t.Destructive, Color.black, DesignTokens.Effects.ActiveDarken * 2));
                    s.onActive.textColor = t.ButtonDestructiveFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = outline(t.Border);
                    s.normal.textColor = t.Text;
                    s.hover.background = bordered(t.Secondary, t.Border);
                    s.hover.textColor = t.Text;
                    s.active.background = bordered(Color.Lerp(t.Secondary, Color.black, 0.1f), t.Border);
                    s.active.textColor = t.Text;
                    s.onNormal.background = outline(t.Accent);
                    s.onNormal.textColor = t.Accent;
                    s.onHover.background = bordered(t.Secondary, t.Accent);
                    s.onHover.textColor = t.Accent;
                    s.onActive.background = bordered(Color.Lerp(t.Secondary, Color.black, 0.1f), t.Accent);
                    s.onActive.textColor = t.Accent;
                }
            );

            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    Color sec = Color.Lerp(t.Secondary, t.Text, 0.1f);
                    s.normal.background = CreateGradientRoundedRectTexture(w, h, GetScaledBorderRadius(r), sec, Color.Lerp(sec, Color.black, 0.05f));
                    s.normal.textColor = t.ButtonSecondaryFg;
                    s.hover.background = CreateGradientRoundedRectTexture(w, h, GetScaledBorderRadius(r), Color.Lerp(sec, Color.white, 0.05f), Color.Lerp(sec, Color.black, 0.05f));
                    s.hover.textColor = t.ButtonSecondaryFg;
                    s.active.background = solid(Color.Lerp(sec, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.active.textColor = t.ButtonSecondaryFg;
                    s.onNormal.background = outline(t.Accent);
                    s.onNormal.textColor = t.Accent;
                    s.onHover.background = outline(t.Accent);
                    s.onHover.textColor = t.Accent;
                    s.onActive.background = outline(Color.Lerp(t.Accent, Color.black, 0.1f));
                    s.onActive.textColor = t.Accent;
                }
            );

            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    Color ghost = Alpha(t.Secondary, 0.5f);
                    s.normal.background = transparentTexture;
                    s.normal.textColor = t.ButtonGhostFg;
                    s.hover.background = solid(ghost);
                    s.hover.textColor = t.ButtonGhostFg;
                    s.active.background = solid(Color.Lerp(ghost, Color.black, DesignTokens.Effects.ActiveDarken));
                    s.active.textColor = t.ButtonGhostFg;
                    s.onNormal.background = solid(Alpha(t.Accent, 0.5f));
                    s.onNormal.textColor = t.Accent;
                    s.onHover.background = solid(Alpha(t.Accent, 0.65f));
                    s.onHover.textColor = t.Accent;
                    s.onActive.background = solid(Alpha(t.Accent, 0.8f));
                    s.onActive.textColor = t.Accent;
                }
            );

            RegisterSizeSet(StyleComponentType.Toggle, Sizes.Toggle);
        }
        #endregion

        #region Input Styles
        private void RegisterInputStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;

            var solid = Solid(w, h, r);

            RegisterVariant(
                StyleComponentType.Input,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = solid(t.Base);
                    s.focused.background = solid(t.Base);
                }
            );

            RegisterVariant(
                StyleComponentType.Input,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.focused.background = transparentTexture;
                }
            );

            RegisterVariant(
                StyleComponentType.PasswordField,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = solid(t.Base);
                    s.focused.background = solid(t.Base);
                }
            );

            RegisterVariant(
                StyleComponentType.PasswordField,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.focused.background = transparentTexture;
                }
            );

            RegisterSizeSet(StyleComponentType.Input, Sizes.Input);
            RegisterSizeSet(StyleComponentType.PasswordField, Sizes.Input);
        }
        #endregion

        #region TextArea Styles
        private void RegisterTextAreaStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;
            int br = (int)DesignTokens.Radius.MD;

            var outline = Outline(w, h, r);

            RegisterVariant(
                StyleComponentType.TextArea,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = outline(t.Border);
                    s.focused.background = outline(t.Accent);
                    s.border = new UnityHelpers.RectOffset(br, br, br, br);
                }
            );

            RegisterVariant(
                StyleComponentType.TextArea,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.focused.background = CreateSolidTexture(Color.Lerp(t.Secondary, Color.black, DesignTokens.Effects.HoverDarken));
                }
            );

            RegisterSizeSet(StyleComponentType.TextArea, Sizes.TextArea);
        }
        #endregion

        #region Label Styles
        private void RegisterLabelStyles()
        {
            RegisterSizeSet(StyleComponentType.Label, Sizes.FontOnly);
            RegisterSizeSet(StyleComponentType.ChartAxis, Sizes.FontOnly);
            RegisterSizeSet(StyleComponentType.SectionHeader, Sizes.FontOnly);
            RegisterSizeSet(StyleComponentType.CardTitle, Sizes.FontOnly);
            RegisterSizeSet(StyleComponentType.CardDescription, Sizes.FontOnly);
        }
        #endregion

        #region ProgressBar Styles
        private void RegisterProgressBarStyles()
        {
            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    s.normal.background = CreateSolidTexture(Color.Lerp(t.Secondary, t.Text, 0.05f));
                }
            );

            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = CreateSolidTexture(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                }
            );

            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
                }
            );

            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                }
            );

            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Muted,
                (s, t, _) =>
                {
                    s.normal.background = CreateSolidTexture(t.Muted);
                }
            );

            RegisterSizeSet(StyleComponentType.ProgressBar, Sizes.ProgressBar);
        }
        #endregion

        #region Separator Styles
        private void RegisterSeparatorStyles()
        {
            RegisterSizeSet(StyleComponentType.Separator, Sizes.Separator);
        }
        #endregion

        #region Tab Styles
        private void RegisterTabStyles()
        {
            RegisterSizeSet(StyleComponentType.TabsList, Sizes.TabsList);
            RegisterSizeSet(StyleComponentType.TabsTrigger, Sizes.Item);
            RegisterSizeSet(StyleComponentType.TabsContent, Sizes.TabsContent);
        }
        #endregion


        #region Checkbox Styles
        private void RegisterCheckboxStyles()
        {
            int size = DesignTokens.Checkbox.Size;
            float r = DesignTokens.Radius.SM;

            var fill = Solid(size, size, r);
            var outline = Outline(size, size, r, 2f);

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = fill(t.Secondary);
                    s.hover.background = fill(t.Secondary);
                    s.active.background = fill(t.Secondary);
                    s.onNormal.background = fill(t.Destructive);
                    s.onHover.background = fill(t.Destructive);
                    s.onActive.background = fill(t.Destructive);
                    s.normal.textColor = t.Text;
                    s.hover.textColor = t.Text;
                    s.active.textColor = t.Text;
                    s.onNormal.textColor = t.ButtonDestructiveFg;
                    s.onHover.textColor = t.ButtonDestructiveFg;
                    s.onActive.textColor = t.ButtonDestructiveFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    s.normal.background = fill(t.Secondary);
                    s.hover.background = fill(t.Secondary);
                    s.active.background = fill(t.Secondary);
                    s.onNormal.background = fill(t.Text);
                    s.onHover.background = fill(t.Text);
                    s.onActive.background = fill(t.Text);
                    s.normal.textColor = t.Text;
                    s.hover.textColor = t.Text;
                    s.active.textColor = t.Text;
                    s.onNormal.textColor = t.Base;
                    s.onHover.textColor = t.Base;
                    s.onActive.textColor = t.Base;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = outline(t.Border);
                    s.hover.background = outline(t.Border);
                    s.active.background = outline(t.Border);
                    s.onNormal.background = outline(t.Accent);
                    s.onHover.background = outline(t.Accent);
                    s.onActive.background = outline(t.Accent);
                    s.normal.textColor = t.Text;
                    s.hover.textColor = t.Text;
                    s.active.textColor = t.Text;
                    s.onNormal.textColor = t.Accent;
                    s.onHover.textColor = t.Accent;
                    s.onActive.textColor = t.Accent;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.hover.background = transparentTexture;
                    s.active.background = transparentTexture;
                    s.onNormal.background = transparentTexture;
                    s.onHover.background = transparentTexture;
                    s.onActive.background = transparentTexture;
                    s.normal.textColor = t.Text;
                    s.hover.textColor = t.Text;
                    s.active.textColor = t.Text;
                    s.onNormal.textColor = t.Accent;
                    s.onHover.textColor = t.Accent;
                    s.onActive.textColor = t.Accent;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Muted,
                (s, t, _) =>
                {
                    s.normal.background = fill(t.Muted);
                    s.hover.background = fill(t.Muted);
                    s.active.background = fill(t.Muted);
                    s.onNormal.background = fill(Color.Lerp(t.Muted, t.Text, 0.3f));
                    s.onHover.background = fill(Color.Lerp(t.Muted, t.Text, 0.3f));
                    s.onActive.background = fill(Color.Lerp(t.Muted, t.Text, 0.3f));
                    s.normal.textColor = t.Text;
                    s.hover.textColor = t.Text;
                    s.active.textColor = t.Text;
                    s.onNormal.textColor = t.ButtonPrimaryFg;
                    s.onHover.textColor = t.ButtonPrimaryFg;
                    s.onActive.textColor = t.ButtonPrimaryFg;
                }
            );

            RegisterSizeSet(StyleComponentType.Checkbox, Sizes.FontOnly);
        }
        #endregion

        #region Switch Styles
        private void RegisterSwitchStyles()
        {
            int w = DesignTokens.Switch.Width;
            int h = DesignTokens.Switch.Height;
            float r = DesignTokens.Switch.Radius;

            var fill = Solid(w, h, r);
            var outline = Outline(w, h, r, 2f);

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = fill(t.Secondary);
                    s.hover.background = fill(Color.Lerp(t.Secondary, t.Text, 0.1f));
                    s.active.background = fill(t.Secondary);
                    s.onNormal.background = fill(t.Destructive);
                    s.onHover.background = fill(GetHoverColor(t.Destructive, true));
                    s.onActive.background = fill(Color.Lerp(t.Destructive, Color.black, 0.1f));
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    s.normal.background = fill(t.Secondary);
                    s.hover.background = fill(Color.Lerp(t.Secondary, t.Text, 0.1f));
                    s.active.background = fill(t.Secondary);
                    s.onNormal.background = fill(t.Text);
                    s.onHover.background = fill(Color.Lerp(t.Text, t.Secondary, 0.2f));
                    s.onActive.background = fill(Color.Lerp(t.Text, Color.black, 0.1f));
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = outline(t.Border);
                    s.hover.background = outline(Color.Lerp(t.Border, t.Text, 0.2f));
                    s.active.background = outline(t.Border);
                    s.onNormal.background = outline(t.Accent);
                    s.onHover.background = outline(Color.Lerp(t.Accent, Color.white, 0.1f));
                    s.onActive.background = outline(t.Accent);
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Ghost,
                (s, t, _) =>
                {
                    s.normal.background = transparentTexture;
                    s.hover.background = transparentTexture;
                    s.active.background = transparentTexture;
                    s.onNormal.background = transparentTexture;
                    s.onHover.background = transparentTexture;
                    s.onActive.background = transparentTexture;
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Muted,
                (s, t, _) =>
                {
                    s.normal.background = fill(t.Muted);
                    s.hover.background = fill(Color.Lerp(t.Muted, t.Text, 0.1f));
                    s.active.background = fill(t.Muted);
                    s.onNormal.background = fill(Color.Lerp(t.Muted, t.Text, 0.3f));
                    s.onHover.background = fill(Color.Lerp(t.Muted, t.Text, 0.5f));
                    s.onActive.background = fill(Color.Lerp(t.Muted, Color.black, 0.15f));
                }
            );

            RegisterSizeSet(StyleComponentType.Switch, Sizes.FontOnly);
        }
        #endregion

        #region Badge Styles
        private void RegisterBadgeStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Height.Default;
            float r = DesignTokens.Radius.MD;

            var solid = Solid(w, h, r);
            var outline = Outline(w, h, DesignTokens.Radius.XL);

            RegisterVariant(
                StyleComponentType.Badge,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = solid(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Badge,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    s.normal.background = solid(t.Secondary);
                    s.normal.textColor = t.ButtonSecondaryFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Badge,
                ControlVariant.Outline,
                (s, t, _) =>
                {
                    s.normal.background = outline(t.Border);
                    s.normal.textColor = t.Text;
                }
            );

            RegisterSizeSet(StyleComponentType.Badge, Sizes.Badge);
        }
        #endregion

        #region Table Styles
        private void RegisterTableStyles()
        {
            RegisterSizeSet(StyleComponentType.Table, Sizes.Table);
            RegisterSizeSet(StyleComponentType.TableHeader, Sizes.TableHeader);
            RegisterSizeSet(StyleComponentType.TableCell, Sizes.TableCell);
        }
        #endregion

        #region Calendar & DatePicker Styles
        private void RegisterCalendarAndDatePickerStyles()
        {
            RegisterSizeSet(StyleComponentType.Calendar, Sizes.Container);
            RegisterSizeSet(StyleComponentType.DatePicker, Sizes.Container);
            RegisterSizeSet(StyleComponentType.CalendarDay, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.CalendarDaySelected, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.CalendarDayOutsideMonth, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.CalendarDayToday, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.CalendarDayInRange, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.DatePickerDay, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.DatePickerDaySelected, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.DatePickerDayOutsideMonth, Sizes.CalendarDay);
            RegisterSizeSet(StyleComponentType.DatePickerDayToday, Sizes.CalendarDay);
        }
        #endregion

        #region Dialog Styles
        private void RegisterDialogStyles()
        {
            RegisterSizeSet(StyleComponentType.Dialog, Sizes.Dialog);
        }
        #endregion

        #region Card Styles
        private void RegisterCardStyles()
        {
            RegisterSizeSet(StyleComponentType.Card, Sizes.Card);
            RegisterSizeSet(StyleComponentType.CardHeader, Sizes.Container);
            RegisterSizeSet(StyleComponentType.CardContent, Sizes.Container);
            RegisterSizeSet(StyleComponentType.CardFooter, Sizes.Container);
        }
        #endregion

        #region Menu Styles
        private void RegisterMenuStyles()
        {
            RegisterSizeSet(StyleComponentType.MenuBar, Sizes.MenuBar);
            RegisterSizeSet(StyleComponentType.MenuBarItem, Sizes.Item);
            RegisterSizeSet(StyleComponentType.DropdownMenuItem, Sizes.Item);
            RegisterSizeSet(StyleComponentType.DropdownMenu, Sizes.Container);
            RegisterSizeSet(StyleComponentType.MenuDropdown, Sizes.Container);
        }
        #endregion

        #region Select Styles
        private void RegisterSelectStyles()
        {
            RegisterSizeSet(StyleComponentType.SelectItem, Sizes.Item);
            RegisterSizeSet(StyleComponentType.SelectContent, Sizes.Container);
        }
        #endregion

        #region Popover Styles
        private void RegisterPopoverStyles()
        {
            RegisterSizeSet(StyleComponentType.Popover, Sizes.Container);
        }
        #endregion

        #region Chart Styles
        private void RegisterChartStyles()
        {
            RegisterSizeSet(StyleComponentType.Chart, Sizes.Container);
        }
        #endregion

        #region AnimatedBox Styles
        private void RegisterAnimatedBoxStyles()
        {
            RegisterSizeSet(StyleComponentType.AnimatedBox, Sizes.Container);
        }
        #endregion

        #region Toast Styles
        private void RegisterToastStyles()
        {
            RegisterSizeSet(StyleComponentType.Toast, Sizes.Container);
            RegisterSizeSet(StyleComponentType.ToastTitle, Sizes.Container);
            RegisterSizeSet(StyleComponentType.ToastDescription, Sizes.Container);
        }
        #endregion

        #region Slider Styles
        private void RegisterSliderStyles()
        {
            int w = DesignTokens.TextureSize.Default;
            int h = (int)DesignTokens.Slider.TrackDefault;
            float r = DesignTokens.Radius.SM;

            RegisterSizeSet(StyleComponentType.SliderTrack, Sizes.SliderTrack);
            RegisterSizeSet(StyleComponentType.SliderThumb, Sizes.SliderThumb);

            RegisterVariant(
                StyleComponentType.SliderFill,
                ControlVariant.Default,
                (s, t, _) =>
                {
                    s.normal.background = CreateRoundedRectTexture(w, h, GetScaledBorderRadius(r), t.Accent);
                }
            );

            RegisterVariant(
                StyleComponentType.SliderFill,
                ControlVariant.Destructive,
                (s, t, _) =>
                {
                    s.normal.background = CreateRoundedRectTexture(w, h, GetScaledBorderRadius(r), t.Destructive);
                }
            );

            RegisterVariant(
                StyleComponentType.SliderFill,
                ControlVariant.Secondary,
                (s, t, _) =>
                {
                    s.normal.background = CreateRoundedRectTexture(w, h, GetScaledBorderRadius(r), Color.Lerp(t.Secondary, t.Text, 0.3f));
                }
            );

            RegisterVariant(
                StyleComponentType.SliderFill,
                ControlVariant.Muted,
                (s, t, _) =>
                {
                    s.normal.background = CreateRoundedRectTexture(w, h, GetScaledBorderRadius(r), t.Muted);
                }
            );
        }
        #endregion
    }
}
