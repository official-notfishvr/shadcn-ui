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
        #region Style Setup Helpers

        private void ApplyInteractiveStates(GUIStyle style, Color baseBg, Color fgColor, Func<Color, Texture2D> texCreator, bool isToggle = false, Color? activeBg = null, Color? activeFg = null)
        {
            style.normal.background = texCreator(baseBg);
            style.normal.textColor = fgColor;

            Color hoverColor = GetHoverColor(baseBg, true);
            style.hover.background = texCreator(hoverColor);
            style.hover.textColor = fgColor;

            Color pressedColor = Color.Lerp(baseBg, Color.black, DesignTokens.Effects.ActiveDarken);
            style.active.background = texCreator(pressedColor);
            style.active.textColor = fgColor;

            style.focused.background = texCreator(hoverColor);
            style.focused.textColor = fgColor;

            if (isToggle && activeBg.HasValue && activeFg.HasValue)
            {
                Color onBg = activeBg.Value;
                Color onFg = activeFg.Value;
                Color onHover = GetHoverColor(onBg, true);
                Color onPressed = Color.Lerp(onBg, Color.black, DesignTokens.Effects.ActiveDarken);

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

        #region StyleBuilder
        internal class StyleBuilder
        {
            private GUIStyle style;
            private Theme theme;
            private StyleManager manager;

            public StyleBuilder(StyleManager mgr, GUIStyle baseStyle = null)
            {
                manager = mgr;
                theme = mgr.GetTheme();
                style = baseStyle != null ? new UnityHelpers.GUIStyle(baseStyle) : new UnityHelpers.GUIStyle(GUI.skin.box);
                if (mgr.customFont != null)
                    style.font = mgr.customFont;
            }

            public StyleBuilder Font(int size, FontStyle fontStyle = FontStyle.Normal)
            {
                style.fontSize = manager.GetScaledFontSize(size / (float)manager.guiHelper.fontSize);
                style.fontStyle = fontStyle;
                return this;
            }

            public StyleBuilder FontScale(float scale, FontStyle fontStyle = FontStyle.Normal)
            {
                style.fontSize = manager.GetScaledFontSize(scale);
                style.fontStyle = fontStyle;
                return this;
            }

            public StyleBuilder Align(TextAnchor anchor)
            {
                style.alignment = anchor;
                return this;
            }

            public StyleBuilder Padding(int h, int v)
            {
                style.padding = new UnityHelpers.RectOffset(h, h, v, v);
                return this;
            }

            public StyleBuilder Margin(int h, int v)
            {
                style.margin = new UnityHelpers.RectOffset(h, h, v, v);
                return this;
            }

            public StyleBuilder TextColor(Color c)
            {
                style.normal.textColor = c;
                return this;
            }

            public StyleBuilder Background(Texture2D tex)
            {
                style.normal.background = tex;
                return this;
            }

            public StyleBuilder FixedHeight(float h)
            {
                style.fixedHeight = manager.GetScaledHeight(h);
                return this;
            }

            public StyleBuilder FixedWidth(float w)
            {
                style.fixedWidth = manager.GetScaledHeight(w);
                return this;
            }

            public StyleBuilder Border(int r)
            {
                style.border = new UnityHelpers.RectOffset(r, r, r, r);
                return this;
            }

            public StyleBuilder WordWrap(bool wrap)
            {
                style.wordWrap = wrap;
                return this;
            }

            public StyleBuilder RichText(bool rich)
            {
                style.richText = rich;
                return this;
            }

            public StyleBuilder Clipping(TextClipping clip)
            {
                style.clipping = clip;
                return this;
            }

            public StyleBuilder StretchWidth(bool stretch)
            {
                style.stretchWidth = stretch;
                return this;
            }

            public StyleBuilder StretchHeight(bool stretch)
            {
                style.stretchHeight = stretch;
                return this;
            }

            public StyleBuilder Apply(Action<GUIStyle, Theme> customize)
            {
                customize?.Invoke(style, theme);
                return this;
            }

            public GUIStyle Build() => style;
        }

        internal StyleBuilder NewStyle(GUIStyle baseStyle = null) => new StyleBuilder(this, baseStyle);
        #endregion

        #region Style Setup - Card
        private void SetupCardStyles()
        {
            try
            {
                cardStyle = NewStyle().Background(cardBackgroundTexture).Padding((int)DesignTokens.Padding.CardH, (int)DesignTokens.Padding.CardV).Border((int)DesignTokens.Radius.LG).Build();
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
                int radius = GetScaledBorderRadius(DesignTokens.Radius.MD);
                Func<Color, Texture2D> btnTex = c => CreateGradientRoundedRectWithShadowTexture(DesignTokens.TextureSize.Default, (int)DesignTokens.Height.Default, radius, Color.Lerp(c, Color.white, 0.08f), c, DesignTokens.Effects.ShadowLight, DesignTokens.Effects.ShadowBlurSM);

                baseButtonStyle = NewStyle()
                    .FontScale(DesignTokens.FontScale.SM, FontStyle.Bold)
                    .Padding((int)DesignTokens.Padding.ButtonDefaultH, (int)DesignTokens.Padding.ButtonDefaultV)
                    .Border((int)DesignTokens.Radius.MD)
                    .Align(TextAnchor.MiddleCenter)
                    .FixedHeight(DesignTokens.Height.Default)
                    .Apply((s, t) => ApplyInteractiveStates(s, t.ButtonPrimaryBg, t.ButtonPrimaryFg, btnTex))
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupButtonStyle", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Toggle Base (Checkbox & Switch)
        private void SetupToggleStyle()
        {
            try
            {
                int radius = GetScaledBorderRadius(DesignTokens.Radius.MD);
                Func<Color, Texture2D> toggleTex = c => CreateRoundedRectTexture(DesignTokens.Toggle.TextureSize, DesignTokens.Toggle.TextureSize, radius, c);

                baseToggleStyle = NewStyle()
                    .Font(guiHelper.fontSize)
                    .Padding((int)DesignTokens.Spacing.MD, (int)DesignTokens.Spacing.SM)
                    .Border((int)DesignTokens.Radius.SM)
                    .Align(TextAnchor.MiddleCenter)
                    .Apply(
                        (s, t) =>
                        {
                            Color offBgColor = Color.Lerp(t.Secondary, t.Base, 0.5f);
                            ApplyInteractiveStates(s, offBgColor, t.Text, toggleTex, true, t.Accent, t.ButtonPrimaryFg);
                        }
                    )
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupToggleStyle", "StyleManager");
            }
        }

        private void SetupCheckboxStyles()
        {
            try
            {
                int w = DesignTokens.Checkbox.Size,
                    h = DesignTokens.Checkbox.Size;
                int r = GetScaledBorderRadius(DesignTokens.Radius.SM);
                Func<Color, Texture2D> fill = c => CreateRoundedRectTexture(w, h, r, c);
                Func<Color, Texture2D> outline = c => CreateRoundedOutlineTexture(w, h, r, c, 2);

                baseCheckboxStyle = NewStyle()
                    .FontScale(DesignTokens.FontScale.SM)
                    .Padding((int)DesignTokens.Spacing.SM, (int)DesignTokens.Spacing.XS)
                    .Border((int)DesignTokens.Radius.SM)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.textColor = t.Text;
                            s.onNormal.textColor = t.ButtonPrimaryFg;
                            s.normal.background = outline(t.Border);
                            s.hover.background = outline(Color.Lerp(t.Border, t.Accent, 0.4f));
                            s.active.background = fill(Color.Lerp(t.Base, t.Accent, 0.15f));
                            s.onNormal.background = fill(t.Accent);
                            s.onHover.background = fill(GetHoverColor(t.Accent, true));
                            s.onActive.background = fill(Color.Lerp(t.Accent, Color.black, DesignTokens.Effects.ActiveDarken));
                        }
                    )
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCheckboxStyles", "StyleManager");
            }
        }

        private void SetupSwitchStyles()
        {
            try
            {
                int w = DesignTokens.Switch.Width,
                    h = DesignTokens.Switch.Height;
                int r = GetScaledBorderRadius(DesignTokens.Switch.Radius);
                Func<Color, Texture2D> fill = c => CreateRoundedRectTexture(w, h, r, c);

                baseSwitchStyle = NewStyle()
                    .FontScale(DesignTokens.FontScale.SM)
                    .Padding((int)DesignTokens.Spacing.SM, (int)DesignTokens.Spacing.None)
                    .Border(DesignTokens.Switch.Radius)
                    .Apply(
                        (s, t) =>
                        {
                            Color offColor = Color.Lerp(t.Secondary, t.Muted, 0.3f);
                            s.normal.textColor = t.Text;
                            s.onNormal.textColor = t.Text;
                            s.normal.background = fill(offColor);
                            s.hover.background = fill(GetHoverColor(offColor, true));
                            s.active.background = fill(offColor);
                            s.onNormal.background = fill(t.Accent);
                            s.onHover.background = fill(GetHoverColor(t.Accent, true));
                            s.onActive.background = fill(Color.Lerp(t.Accent, Color.black, 0.1f));
                        }
                    )
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSwitchStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Input Base
        private void SetupInputStyle()
        {
            try
            {
                int w = DesignTokens.TextureSize.Default;
                int h = (int)DesignTokens.Height.Default;
                int r = GetScaledBorderRadius(DesignTokens.Radius.MD);

                baseInputStyle = NewStyle()
                    .FontScale(DesignTokens.FontScale.SM)
                    .Padding((int)DesignTokens.Padding.InputH, (int)DesignTokens.Padding.InputV)
                    .Border((int)DesignTokens.Radius.MD)
                    .FixedHeight(DesignTokens.Height.Default)
                    .Margin((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.SM)
                    .Apply(
                        (s, t) =>
                        {
                            s.clipping = TextClipping.Clip;
                            s.contentOffset = new Vector2(2f, 0f);
                            s.normal.textColor = t.Text;
                            s.hover.textColor = t.Text;
                            Texture2D normalTex = CreateInnerShadowTexture(w, h, r, t.Base, DesignTokens.Effects.InnerShadowIntensity, DesignTokens.Effects.InnerShadowSize);
                            s.normal.background = normalTex;
                            s.hover.background = CreateBorderedRoundedRectTexture(w, h, r, t.Base, Color.Lerp(t.Border, t.Accent, 0.3f), 1f);
                            Texture2D focusTex = CreateFocusRingTexture(w, h, r, t.Accent, DesignTokens.Effects.FocusRingThickness);
                            s.focused.background = focusTex;
                            s.focused.textColor = t.Text;
                            s.active.background = focusTex;
                            s.active.textColor = t.Text;
                        }
                    )
                    .Build();
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
                baseLabelStyle = NewStyle()
                    .Font(guiHelper.fontSize)
                    .Padding((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.XS)
                    .Align(TextAnchor.UpperLeft)
                    .WordWrap(true)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.textColor = t.Text;
                            s.normal.background = null;
                        }
                    )
                    .Build();
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
                progressBarStyle = NewStyle()
                    .Background(progressBarBackgroundTexture)
                    .Padding((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.None)
                    .Border((int)DesignTokens.Radius.LG)
                    .Margin((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.SM)
                    .FixedHeight(DesignTokens.ProgressBar.Height)
                    .Apply(
                        (s, t) =>
                        {
                            s.hover = s.normal;
                            s.active = s.normal;
                        }
                    )
                    .Build();
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
                separatorStyle = NewStyle()
                    .Background(separatorTexture)
                    .Padding((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.None)
                    .Border(0)
                    .FixedHeight(Mathf.Max(1, Mathf.RoundToInt(DesignTokens.Separator.DefaultThickness * guiHelper.uiScale)))
                    .Margin((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.None)
                    .Apply((s, t) => s.stretchWidth = true)
                    .Build();
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
                tabsListStyle = NewStyle().Background(tabsBackgroundTexture).Padding((int)DesignTokens.Spacing.XS, (int)DesignTokens.Spacing.XS).Border((int)DesignTokens.Radius.MD).Build();

                tabsTriggerStyle = NewStyle()
                    .Background(transparentTexture)
                    .FontScale(DesignTokens.FontScale.SM, FontStyle.Bold)
                    .Padding((int)DesignTokens.Padding.TabH, (int)DesignTokens.Padding.TabV)
                    .Border((int)DesignTokens.Radius.SM)
                    .Align(TextAnchor.MiddleCenter)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.textColor = t.Muted;
                            Texture2D hoverTex = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, (int)DesignTokens.Radius.SM, Color.Lerp(t.TabsBg, t.Text, 0.08f));
                            s.hover.background = hoverTex;
                            s.hover.textColor = t.Text;
                            s.active.background = hoverTex;
                            s.active.textColor = t.Text;
                        }
                    )
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTabsStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Badge & Avatar
        private void SetupBadgeStyle()
        {
            try
            {
                baseBadgeStyle = NewStyle()
                    .Background(badgeTexture)
                    .FontScale(DesignTokens.FontScale.XS)
                    .Padding((int)DesignTokens.Padding.BadgeH, (int)DesignTokens.Padding.BadgeV)
                    .Border((int)DesignTokens.Radius.XL)
                    .Align(TextAnchor.MiddleCenter)
                    .Apply(
                        (s, t) =>
                        {
                            s.fontStyle = FontStyle.Bold;
                            s.normal.textColor = t.ButtonPrimaryFg;
                        }
                    )
                    .Build();
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
                int size = GetScaledHeight(DesignTokens.Height.Default);
                int radius = Mathf.RoundToInt(DesignTokens.Avatar.CircleRadiusScale * guiHelper.uiScale);
                avatarStyle = NewStyle().Border(radius).Align(TextAnchor.MiddleCenter).FixedWidth((float)size).FixedHeight((float)size).Apply((s, t) => s.normal.background = CreateSolidTexture(t.Muted)).Build();
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
                baseTableStyle = NewStyle()
                    .Padding((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.None)
                    .Border((int)DesignTokens.Radius.LG)
                    .Apply((s, t) => s.normal.background = CreateBorderedRoundedRectTexture(DesignTokens.TextureSize.Default, DesignTokens.TextureSize.Default, (int)DesignTokens.Radius.LG, t.Base, t.Border, 1))
                    .Build();

                tableCellStyle = NewStyle()
                    .Background(tableCellTexture)
                    .FontScale(DesignTokens.FontScale.SM)
                    .Padding((int)DesignTokens.Padding.TableCellH, (int)DesignTokens.Padding.TableCellV)
                    .Align(TextAnchor.MiddleLeft)
                    .WordWrap(false)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.textColor = t.Text;
                            s.clipping = TextClipping.Clip;
                        }
                    )
                    .Build();
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
                baseCalendarStyle = NewStyle().Background(calendarBackgroundTexture).Padding((int)DesignTokens.Spacing.MD, (int)DesignTokens.Spacing.MD).Border((int)DesignTokens.Radius.LG).Build();

                calendarDayStyle = NewStyle()
                    .Background(calendarDayTexture)
                    .FontScale(DesignTokens.FontScale.SM)
                    .Align(TextAnchor.MiddleCenter)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.textColor = t.Text;
                            s.hover.background = CreateRoundedRectTexture(32, 32, (int)DesignTokens.Radius.SM, t.Secondary);
                            s.hover.textColor = t.Text;
                        }
                    )
                    .Build();
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
                dropdownContentStyle = NewStyle().Background(dropdownMenuContentTexture).Padding((int)DesignTokens.Spacing.XS, (int)DesignTokens.Spacing.XS).Border((int)DesignTokens.Radius.MD).Build();

                dropdownItemStyle = NewStyle()
                    .Background(transparentTexture)
                    .FontScale(DesignTokens.FontScale.SM)
                    .Padding((int)DesignTokens.Padding.DropdownItemH, (int)DesignTokens.Padding.DropdownItemV)
                    .Align(TextAnchor.MiddleLeft)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.textColor = t.Text;
                            Color hoverColor = t.Accent;
                            Texture2D hoverTex = CreateRoundedRectTexture(DesignTokens.TextureSize.Default, 32, (int)DesignTokens.Radius.SM, hoverColor);
                            s.hover.background = hoverTex;
                            s.hover.textColor = t.ButtonPrimaryFg;
                            s.active.background = hoverTex;
                            s.active.textColor = t.ButtonPrimaryFg;
                        }
                    )
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDropdownMenuStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Dialog & Chart
        private void SetupDialogStyles()
        {
            try
            {
                dialogContentStyle = NewStyle()
                    .Padding((int)DesignTokens.Padding.CardH, (int)DesignTokens.Padding.CardV)
                    .Border((int)DesignTokens.Radius.XL)
                    .Apply(
                        (s, t) =>
                            s.normal.background = CreateGradientRoundedRectWithShadowTexture(
                                DesignTokens.TextureSize.XL,
                                DesignTokens.TextureSize.XL,
                                (int)DesignTokens.Radius.XL,
                                Color.Lerp(t.Elevated, Color.white, 0.02f),
                                t.Elevated,
                                DesignTokens.Effects.ShadowMedium,
                                DesignTokens.Effects.ShadowBlurLG
                            )
                    )
                    .Build();
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
                chartContainerStyle = NewStyle().Background(chartContainerTexture).Padding((int)DesignTokens.Spacing.LG, (int)DesignTokens.Spacing.LG).Border((int)DesignTokens.Radius.LG).Build();
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
                menuBarStyle = NewStyle()
                    .Background(transparentTexture)
                    .Padding((int)DesignTokens.Spacing.XS, (int)DesignTokens.Spacing.None)
                    .Margin((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.None)
                    .FixedHeight(DesignTokens.Height.Default)
                    .Align(TextAnchor.MiddleLeft)
                    .Apply((s, t) => s.stretchWidth = false)
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupMenuBarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Slider
        private void SetupSliderStyles()
        {
            try
            {
                baseSliderStyle = NewStyle()
                    .Background(sliderTrackTexture)
                    .Padding((int)DesignTokens.Spacing.None, (int)DesignTokens.Spacing.None)
                    .Border((int)DesignTokens.Radius.SM)
                    .FixedHeight(6f)
                    .Apply(
                        (s, t) =>
                        {
                            s.normal.background = sliderTrackTexture;
                            s.stretchWidth = true;
                        }
                    )
                    .Build();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSliderStyles", "StyleManager");
            }
        }
        #endregion
    }
}
