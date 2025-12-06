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
        #region Style Registration
        private void RegisterVariant(StyleComponentType type, ControlVariant variant, StyleModifier mod) => Registry.RegisterVariant(type, variant, mod);

        private void RegisterSize(StyleComponentType type, ControlSize size, StyleModifier mod) => Registry.RegisterSize(type, size, mod);

        private void RegisterDefaultStyles()
        {
            RegisterButtonStyles();
            RegisterToggleStyles();
            RegisterInputStyles();
            RegisterLabelStyles();
            RegisterTextAreaStyles();
            RegisterProgressBarStyles();
            RegisterSeparatorStyles();
            RegisterTabStyles();
            RegisterCheckboxStyles();
            RegisterSwitchStyles();
            RegisterBadgeStyles();
            RegisterTableStyles();
            RegisterCalendarStyles();
            RegisterDialogStyles();
            RegisterCardStyles();
            RegisterMenuStyles();
            RegisterSelectStyles();
            RegisterPopoverStyles();
            RegisterDatePickerStyles();
            RegisterChartStyles();
            RegisterAnimatedBoxStyles();
        }

        private void RegisterButtonStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            int r = GetScaledBorderRadius(6f);
            var tex = (Color c) => CreateRoundedRectTexture(128, 40, r, c);

            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Destructive,
                (s, t, h) =>
                {
                    s.normal.background = tex(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                    s.hover.background = tex(GetHoverColor(t.Destructive, true));
                    s.hover.textColor = t.ButtonDestructiveFg;
                    s.active.background = tex(Color.Lerp(t.Destructive, Color.black, 0.2f));
                    s.active.textColor = t.ButtonDestructiveFg;
                }
            );
            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Outline,
                (s, t, h) =>
                {
                    s.normal.background = CreateRoundedOutlineTexture(128, 40, 6, t.Border, 2f);
                    s.normal.textColor = t.Text;
                    s.hover.background = CreateBorderedRoundedRectTexture(128, 40, 6, t.Secondary, t.Border, 2f);
                    s.hover.textColor = t.Text;
                    s.active.background = CreateBorderedRoundedRectTexture(128, 40, 6, Color.Lerp(t.Secondary, Color.black, 0.1f), t.Border, 2f);
                    s.active.textColor = t.Text;
                }
            );
            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Secondary,
                (s, t, h) =>
                {
                    Color sec = Color.Lerp(t.Secondary, t.Text, 0.1f);
                    s.normal.background = CreateGradientRoundedRectTexture(128, 40, 6, sec, Color.Lerp(sec, Color.black, 0.05f));
                    s.normal.textColor = t.ButtonSecondaryFg;
                    s.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(sec, Color.white, 0.05f), Color.Lerp(sec, Color.black, 0.05f));
                }
            );
            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Ghost,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.normal.textColor = t.ButtonGhostFg;
                    Color gh = new Color(t.Secondary.r, t.Secondary.g, t.Secondary.b, 0.5f);
                    s.hover.background = CreateRoundedRectTexture(128, 40, 6, gh);
                    s.active.background = CreateRoundedRectTexture(128, 40, 6, Color.Lerp(gh, Color.black, 0.2f));
                }
            );
            RegisterVariant(
                StyleComponentType.Button,
                ControlVariant.Link,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.normal.textColor = t.ButtonLinkColor;
                    s.hover.background = transparentTexture;
                    s.hover.textColor = Color.Lerp(t.ButtonLinkColor, Color.white, 0.2f);
                    s.active.background = transparentTexture;
                    s.active.textColor = Color.Lerp(t.ButtonLinkColor, Color.black, 0.25f);
                    s.padding = GetSpacingOffset(0f, 2f);
                    s.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                }
            );

            RegisterSize(
                StyleComponentType.Button,
                ControlSize.Small,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.75f);
                    s.padding = GetSpacingOffset(12f, 2f);
                    s.fixedHeight = GetScaledHeight(32f);
                    s.border = GetBorderOffset(6f);
                }
            );
            RegisterSize(
                StyleComponentType.Button,
                ControlSize.Large,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.0f);
                    s.padding = GetSpacingOffset(24f, 12f);
                    s.fixedHeight = GetScaledHeight(44f);
                    s.border = GetBorderOffset(6f);
                }
            );
            RegisterSize(
                StyleComponentType.Button,
                ControlSize.Icon,
                (s, t, h) =>
                {
                    int sz = GetScaledHeight(36f);
                    s.fixedWidth = sz;
                    s.fixedHeight = sz;
                    s.padding = GetSpacingOffset(0f, 0f);
                    s.border = GetBorderOffset(6f);
                }
            );
            RegisterSize(
                StyleComponentType.Button,
                ControlSize.Mini,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.7f);
                    s.padding = GetSpacingOffset(8f, 2f);
                    s.fixedHeight = GetScaledHeight(24f);
                    s.border = GetBorderOffset(4f);
                }
            );
            RegisterSize(
                StyleComponentType.Button,
                ControlSize.Default,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.875f);
                    s.padding = GetSpacingOffset(16f, 8f);
                    s.fixedHeight = GetScaledHeight(36f);
                    s.border = GetBorderOffset(6f);
                }
            );
        }

        private void RegisterToggleStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            int r = GetScaledBorderRadius(6f);
            var tex = (Color c) => CreateRoundedRectTexture(128, 40, r, c);

            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Destructive,
                (s, t, h) =>
                {
                    s.normal.background = tex(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                    s.hover.background = tex(GetHoverColor(t.Destructive, true));
                    s.hover.textColor = t.ButtonDestructiveFg;
                    s.active.background = tex(Color.Lerp(t.Destructive, Color.black, 0.2f));
                    s.active.textColor = t.ButtonDestructiveFg;
                    s.onNormal.background = tex(Color.Lerp(t.Destructive, Color.black, 0.15f));
                    s.onNormal.textColor = t.ButtonDestructiveFg;
                    s.onHover.background = tex(Color.Lerp(t.Destructive, Color.black, 0.25f));
                    s.onHover.textColor = t.ButtonDestructiveFg;
                    s.onActive.background = tex(Color.Lerp(t.Destructive, Color.black, 0.3f));
                    s.onActive.textColor = t.ButtonDestructiveFg;
                }
            );
            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Outline,
                (s, t, h) =>
                {
                    s.normal.background = CreateRoundedOutlineTexture(128, 40, 6, t.Border, 2f);
                    s.normal.textColor = t.Text;
                    s.hover.background = CreateBorderedRoundedRectTexture(128, 40, 6, t.Secondary, t.Border, 2f);
                    s.hover.textColor = t.Text;
                    s.active.background = CreateBorderedRoundedRectTexture(128, 40, 6, Color.Lerp(t.Secondary, Color.black, 0.1f), t.Border, 2f);
                    s.active.textColor = t.Text;
                    s.onNormal.background = CreateRoundedOutlineTexture(128, 40, 6, t.Accent, 2f);
                    s.onNormal.textColor = t.Accent;
                    s.onHover.background = CreateBorderedRoundedRectTexture(128, 40, 6, t.Secondary, t.Accent, 2f);
                    s.onHover.textColor = t.Accent;
                }
            );
            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Secondary,
                (s, t, h) =>
                {
                    Color sec = Color.Lerp(t.Secondary, t.Text, 0.1f);
                    s.normal.background = CreateGradientRoundedRectTexture(128, 40, 6, sec, Color.Lerp(sec, Color.black, 0.05f));
                    s.normal.textColor = t.ButtonSecondaryFg;
                    s.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(sec, Color.white, 0.05f), Color.Lerp(sec, Color.black, 0.05f));
                    s.onNormal.background = CreateRoundedOutlineTexture(128, 40, 6, t.Accent);
                    s.onNormal.textColor = t.Accent;
                    s.onHover.background = CreateRoundedOutlineTexture(128, 40, 6, t.Accent);
                }
            );
            RegisterVariant(
                StyleComponentType.Toggle,
                ControlVariant.Ghost,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.normal.textColor = t.ButtonGhostFg;
                    Color gh = new Color(t.Secondary.r, t.Secondary.g, t.Secondary.b, 0.5f);
                    s.hover.background = CreateRoundedRectTexture(128, 40, 6, gh);
                    s.active.background = CreateRoundedRectTexture(128, 40, 6, Color.Lerp(gh, Color.black, 0.2f));
                    s.onNormal.background = CreateRoundedRectTexture(128, 40, 6, new Color(t.Accent.r, t.Accent.g, t.Accent.b, 0.5f));
                    s.onNormal.textColor = t.Accent;
                    s.onHover.background = CreateRoundedRectTexture(128, 40, 6, new Color(t.Accent.r, t.Accent.g, t.Accent.b, 0.65f));
                }
            );

            RegisterSize(
                StyleComponentType.Toggle,
                ControlSize.Small,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.75f);
                    s.padding = GetSpacingOffset(6f, 2f);
                }
            );
            RegisterSize(
                StyleComponentType.Toggle,
                ControlSize.Large,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.25f);
                    s.padding = GetSpacingOffset(10f, 6f);
                }
            );
            RegisterSize(
                StyleComponentType.Toggle,
                ControlSize.Mini,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.65f);
                    s.padding = GetSpacingOffset(4f, 1f);
                }
            );
            RegisterSize(
                StyleComponentType.Toggle,
                ControlSize.Default,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.0f);
                    s.padding = GetSpacingOffset(8f, 4f);
                }
            );
        }

        private void RegisterInputStyles()
        {
            RegisterVariant(
                StyleComponentType.Input,
                ControlVariant.Outline,
                (s, t, h) =>
                {
                    s.normal.background = CreateRoundedRectTexture(128, 40, 6, t.Base);
                    s.focused.background = CreateRoundedRectTexture(128, 40, 6, t.Base);
                }
            );
            RegisterVariant(
                StyleComponentType.PasswordField,
                ControlVariant.Outline,
                (s, t, h) =>
                {
                    s.normal.background = CreateRoundedRectTexture(128, 40, 6, t.Base);
                    s.focused.background = CreateRoundedRectTexture(128, 40, 6, t.Base);
                }
            );
            RegisterVariant(
                StyleComponentType.Input,
                ControlVariant.Ghost,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.focused.background = transparentTexture;
                }
            );
            RegisterVariant(
                StyleComponentType.PasswordField,
                ControlVariant.Ghost,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.focused.background = transparentTexture;
                }
            );

            RegisterSize(
                StyleComponentType.Input,
                ControlSize.Small,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.75f);
                    s.padding = GetSpacingOffset(8f, 4f);
                    s.fixedHeight = GetScaledHeight(28f);
                }
            );
            RegisterSize(
                StyleComponentType.PasswordField,
                ControlSize.Small,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.75f);
                    s.padding = GetSpacingOffset(8f, 4f);
                    s.fixedHeight = GetScaledHeight(28f);
                }
            );
            RegisterSize(
                StyleComponentType.Input,
                ControlSize.Large,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.0f);
                    s.padding = GetSpacingOffset(16f, 10f);
                    s.fixedHeight = GetScaledHeight(44f);
                }
            );
            RegisterSize(
                StyleComponentType.PasswordField,
                ControlSize.Large,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.0f);
                    s.padding = GetSpacingOffset(16f, 10f);
                    s.fixedHeight = GetScaledHeight(44f);
                }
            );
            RegisterSize(
                StyleComponentType.Input,
                ControlSize.Mini,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.7f);
                    s.padding = GetSpacingOffset(6f, 2f);
                    s.fixedHeight = GetScaledHeight(24f);
                }
            );
            RegisterSize(
                StyleComponentType.PasswordField,
                ControlSize.Mini,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.7f);
                    s.padding = GetSpacingOffset(6f, 2f);
                    s.fixedHeight = GetScaledHeight(24f);
                }
            );
            RegisterSize(
                StyleComponentType.Input,
                ControlSize.Default,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.875f);
                    s.padding = GetSpacingOffset(12f, 8f);
                    s.fixedHeight = GetScaledHeight(36f);
                }
            );
            RegisterSize(
                StyleComponentType.PasswordField,
                ControlSize.Default,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.875f);
                    s.padding = GetSpacingOffset(12f, 8f);
                    s.fixedHeight = GetScaledHeight(36f);
                }
            );
        }

        private void RegisterLabelStyles()
        {
            var labelTypes = new[] { StyleComponentType.Label, StyleComponentType.ChartAxis, StyleComponentType.SectionHeader, StyleComponentType.CardTitle, StyleComponentType.CardDescription };
            foreach (var type in labelTypes)
            {
                RegisterSize(type, ControlSize.Small, (s, t, h) => s.fontSize = GetScaledFontSize(0.75f));
                RegisterSize(type, ControlSize.Large, (s, t, h) => s.fontSize = GetScaledFontSize(1.25f));
                RegisterSize(type, ControlSize.Mini, (s, t, h) => s.fontSize = GetScaledFontSize(0.65f));
                RegisterSize(type, ControlSize.Default, (s, t, h) => s.fontSize = GetScaledFontSize(0.875f));
            }
        }

        private void RegisterTextAreaStyles()
        {
            RegisterVariant(
                StyleComponentType.TextArea,
                ControlVariant.Outline,
                (s, t, h) =>
                {
                    s.normal.background = CreateRoundedOutlineTexture(128, 40, 6, t.Border);
                    s.focused.background = CreateRoundedOutlineTexture(128, 40, 6, t.Accent);
                    s.border = new UnityHelpers.RectOffset(6, 6, 6, 6);
                }
            );
            RegisterVariant(
                StyleComponentType.TextArea,
                ControlVariant.Ghost,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.focused.background = CreateSolidTexture(Color.Lerp(t.Secondary, Color.black, 0.08f));
                }
            );

            RegisterSize(
                StyleComponentType.TextArea,
                ControlSize.Small,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.75f);
                    s.padding = GetSpacingOffset(6f, 4f);
                }
            );
            RegisterSize(
                StyleComponentType.TextArea,
                ControlSize.Large,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.0f);
                    s.padding = GetSpacingOffset(12f, 8f);
                }
            );
            RegisterSize(
                StyleComponentType.TextArea,
                ControlSize.Mini,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.7f);
                    s.padding = GetSpacingOffset(4f, 2f);
                }
            );
            RegisterSize(
                StyleComponentType.TextArea,
                ControlSize.Default,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.875f);
                    s.padding = GetSpacingOffset(8f, 6f);
                }
            );
        }

        private void RegisterProgressBarStyles()
        {
            RegisterVariant(StyleComponentType.ProgressBar, ControlVariant.Secondary, (s, t, h) => s.normal.background = CreateSolidTexture(Color.Lerp(t.Secondary, t.Text, 0.05f)));
            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Destructive,
                (s, t, h) =>
                {
                    s.normal.background = CreateSolidTexture(t.Destructive);
                    s.normal.textColor = t.ButtonDestructiveFg;
                }
            );
            RegisterVariant(
                StyleComponentType.ProgressBar,
                ControlVariant.Outline,
                (s, t, h) =>
                {
                    s.normal.background = transparentTexture;
                    s.border = new UnityHelpers.RectOffset(1, 1, 1, 1);
                }
            );
            RegisterVariant(StyleComponentType.ProgressBar, ControlVariant.Ghost, (s, t, h) => s.normal.background = transparentTexture);
            RegisterVariant(StyleComponentType.ProgressBar, ControlVariant.Muted, (s, t, h) => s.normal.background = CreateSolidTexture(t.Muted));

            RegisterSize(StyleComponentType.ProgressBar, ControlSize.Small, (s, t, h) => s.fixedHeight = GetScaledHeight(4f));
            RegisterSize(StyleComponentType.ProgressBar, ControlSize.Large, (s, t, h) => s.fixedHeight = GetScaledHeight(12f));
            RegisterSize(StyleComponentType.ProgressBar, ControlSize.Mini, (s, t, h) => s.fixedHeight = GetScaledHeight(2f));
            RegisterSize(StyleComponentType.ProgressBar, ControlSize.Default, (s, t, h) => s.fixedHeight = GetScaledHeight(8f));
        }

        private void RegisterSeparatorStyles()
        {
            RegisterSize(StyleComponentType.Separator, ControlSize.Small, (s, t, h) => s.fixedHeight = GetScaledHeight(1f));
            RegisterSize(StyleComponentType.Separator, ControlSize.Large, (s, t, h) => s.fixedHeight = GetScaledHeight(4f));
            RegisterSize(StyleComponentType.Separator, ControlSize.Mini, (s, t, h) => s.fixedHeight = GetScaledHeight(1f));
            RegisterSize(StyleComponentType.Separator, ControlSize.Default, (s, t, h) => s.fixedHeight = GetScaledHeight(2f));
        }

        private void RegisterTabStyles()
        {
            RegisterSize(StyleComponentType.TabsList, ControlSize.Small, (s, t, h) => s.padding = GetSpacingOffset(2f, 2f));
            RegisterSize(StyleComponentType.TabsList, ControlSize.Large, (s, t, h) => s.padding = GetSpacingOffset(6f, 6f));
            RegisterSize(StyleComponentType.TabsList, ControlSize.Mini, (s, t, h) => s.padding = GetSpacingOffset(1f, 1f));
            RegisterSize(StyleComponentType.TabsList, ControlSize.Default, (s, t, h) => s.padding = GetSpacingOffset(4f, 4f));

            RegisterSize(
                StyleComponentType.TabsTrigger,
                ControlSize.Small,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.75f);
                    s.padding = GetSpacingOffset(8f, 4f);
                }
            );
            RegisterSize(
                StyleComponentType.TabsTrigger,
                ControlSize.Large,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(1.0f);
                    s.padding = GetSpacingOffset(16f, 8f);
                }
            );
            RegisterSize(
                StyleComponentType.TabsTrigger,
                ControlSize.Mini,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.7f);
                    s.padding = GetSpacingOffset(6f, 2f);
                }
            );
            RegisterSize(
                StyleComponentType.TabsTrigger,
                ControlSize.Default,
                (s, t, h) =>
                {
                    s.fontSize = GetScaledFontSize(0.875f);
                    s.padding = GetSpacingOffset(12f, 6f);
                }
            );

            RegisterSize(StyleComponentType.TabsContent, ControlSize.Small, (s, t, h) => s.padding = GetSpacingOffset(8f, 8f));
            RegisterSize(StyleComponentType.TabsContent, ControlSize.Large, (s, t, h) => s.padding = GetSpacingOffset(24f, 24f));
            RegisterSize(StyleComponentType.TabsContent, ControlSize.Mini, (s, t, h) => s.padding = GetSpacingOffset(4f, 4f));
            RegisterSize(StyleComponentType.TabsContent, ControlSize.Default, (s, t, h) => s.padding = GetSpacingOffset(16f, 16f));
        }

        private void RegisterBadgeStyles() => RegisterContainerSizes(StyleComponentType.Badge, new[] { (ControlSize.Small, 0.65f, 8f, 2f), (ControlSize.Large, 1.0f, 12f, 6f), (ControlSize.Mini, 0.6f, 6f, 1f), (ControlSize.Default, 0.8f, 10f, 4f) });

        private void RegisterTableStyles()
        {
            RegisterContainerSizes(StyleComponentType.Table, new[] { (ControlSize.Small, 0.9f, 8f, 8f), (ControlSize.Large, 1.1f, 20f, 20f), (ControlSize.Mini, 0.75f, 6f, 4f), (ControlSize.Default, 0.95f, 14f, 14f) });
            RegisterContainerSizes(StyleComponentType.TableHeader, new[] { (ControlSize.Small, 0.85f, 8f, 6f), (ControlSize.Large, 1.1f, 16f, 12f), (ControlSize.Mini, 0.75f, 4f, 2f), (ControlSize.Default, 0.95f, 12f, 8f) });
            RegisterContainerSizes(StyleComponentType.TableCell, new[] { (ControlSize.Small, 0.8f, 8f, 6f), (ControlSize.Large, 1.0f, 16f, 12f), (ControlSize.Mini, 0.7f, 4f, 2f), (ControlSize.Default, 0.9f, 12f, 8f) });
        }

        private void RegisterCalendarStyles()
        {
            var dayTypes = new[] { StyleComponentType.CalendarDay, StyleComponentType.CalendarDaySelected, StyleComponentType.CalendarDayOutsideMonth, StyleComponentType.CalendarDayToday, StyleComponentType.CalendarDayInRange };
            foreach (var type in dayTypes)
            {
                RegisterSize(
                    type,
                    ControlSize.Small,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.8f);
                        s.fixedWidth = GetScaledHeight(28f);
                        s.fixedHeight = GetScaledHeight(28f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Large,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(1.0f);
                        s.fixedWidth = GetScaledHeight(40f);
                        s.fixedHeight = GetScaledHeight(40f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Mini,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.7f);
                        s.fixedWidth = GetScaledHeight(20f);
                        s.fixedHeight = GetScaledHeight(20f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Default,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.9f);
                        s.fixedWidth = GetScaledHeight(32f);
                        s.fixedHeight = GetScaledHeight(32f);
                    }
                );
            }
            RegisterContainerSizes(StyleComponentType.Calendar);
        }

        private void RegisterDialogStyles()
        {
            RegisterSize(StyleComponentType.Dialog, ControlSize.Small, (s, t, h) => s.padding = GetSpacingOffset(16f, 16f));
            RegisterSize(StyleComponentType.Dialog, ControlSize.Large, (s, t, h) => s.padding = GetSpacingOffset(32f, 32f));
            RegisterSize(StyleComponentType.Dialog, ControlSize.Mini, (s, t, h) => s.padding = GetSpacingOffset(8f, 8f));
            RegisterSize(StyleComponentType.Dialog, ControlSize.Default, (s, t, h) => s.padding = GetSpacingOffset(24f, 24f));
        }

        private void RegisterCardStyles()
        {
            RegisterSize(StyleComponentType.Card, ControlSize.Small, (s, t, h) => s.padding = GetSpacingOffset(12f, 12f));
            RegisterSize(StyleComponentType.Card, ControlSize.Large, (s, t, h) => s.padding = GetSpacingOffset(32f, 32f));
            RegisterSize(StyleComponentType.Card, ControlSize.Mini, (s, t, h) => s.padding = GetSpacingOffset(8f, 8f));
            RegisterSize(StyleComponentType.Card, ControlSize.Default, (s, t, h) => s.padding = GetSpacingOffset(24f, 24f));

            var childTypes = new[] { StyleComponentType.CardHeader, StyleComponentType.CardContent, StyleComponentType.CardFooter };
            foreach (var type in childTypes)
                RegisterContainerSizes(type);
        }

        private void RegisterMenuStyles()
        {
            RegisterSize(StyleComponentType.MenuBar, ControlSize.Small, (s, t, h) => s.padding = GetSpacingOffset(2f, 2f));
            RegisterSize(StyleComponentType.MenuBar, ControlSize.Large, (s, t, h) => s.padding = GetSpacingOffset(6f, 6f));
            RegisterSize(StyleComponentType.MenuBar, ControlSize.Mini, (s, t, h) => s.padding = GetSpacingOffset(1f, 1f));
            RegisterSize(StyleComponentType.MenuBar, ControlSize.Default, (s, t, h) => s.padding = GetSpacingOffset(4f, 4f));

            var itemTypes = new[] { StyleComponentType.MenuBarItem, StyleComponentType.DropdownMenuItem };
            foreach (var type in itemTypes)
            {
                RegisterSize(
                    type,
                    ControlSize.Small,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.75f);
                        s.padding = GetSpacingOffset(6f, 4f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Large,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(1.0f);
                        s.padding = GetSpacingOffset(12f, 8f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Mini,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.7f);
                        s.padding = GetSpacingOffset(4f, 2f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Default,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.875f);
                        s.padding = GetSpacingOffset(8f, 6f);
                    }
                );
            }
            RegisterContainerSizes(StyleComponentType.DropdownMenu);
            RegisterContainerSizes(StyleComponentType.MenuDropdown);
        }

        private void RegisterSelectStyles()
        {
            var types = new[] { StyleComponentType.SelectTrigger, StyleComponentType.SelectItem };
            foreach (var type in types)
            {
                RegisterSize(
                    type,
                    ControlSize.Small,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.75f);
                        s.padding = GetSpacingOffset(8f, 4f);
                        s.fixedHeight = GetScaledHeight(28f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Large,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(1.0f);
                        s.padding = GetSpacingOffset(16f, 8f);
                        s.fixedHeight = GetScaledHeight(44f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Mini,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.7f);
                        s.padding = GetSpacingOffset(6f, 2f);
                        s.fixedHeight = GetScaledHeight(24f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Default,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.875f);
                        s.padding = GetSpacingOffset(12f, 6f);
                        s.fixedHeight = GetScaledHeight(36f);
                    }
                );
            }
            RegisterContainerSizes(StyleComponentType.SelectContent);
        }

        private void RegisterCheckboxStyles()
        {
            var theme = ThemeManager.Instance.CurrentTheme;

            int w = 16;
            int h = 16;
            int r = GetScaledBorderRadius(4f);

            Func<Color, Texture2D> fill = (c) => CreateRoundedRectTexture(w, h, r, c);
            Func<Color, Texture2D> outline = (c) => CreateRoundedOutlineTexture(w, h, r, c, 1);

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Destructive,
                (s, t, h2) =>
                {
                    s.normal.background = fill(t.Secondary);
                    s.onNormal.background = fill(t.Destructive);

                    s.hover.background = s.normal.background;
                    s.active.background = s.normal.background;
                    s.onHover.background = s.onNormal.background;
                    s.onActive.background = s.onNormal.background;

                    s.normal.textColor = t.Text;
                    s.onNormal.textColor = t.ButtonDestructiveFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Secondary,
                (s, t, h2) =>
                {
                    s.normal.background = fill(t.Secondary);
                    s.onNormal.background = fill(t.Secondary);

                    s.hover.background = s.normal.background;
                    s.active.background = s.normal.background;
                    s.onHover.background = s.onNormal.background;
                    s.onActive.background = s.onNormal.background;

                    s.normal.textColor = t.Text;
                    s.onNormal.textColor = t.ButtonPrimaryFg;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Outline,
                (s, t, h2) =>
                {
                    s.normal.background = outline(t.Border);
                    s.onNormal.background = outline(t.Accent);

                    s.hover.background = s.normal.background;
                    s.active.background = s.normal.background;

                    s.onHover.background = s.onNormal.background;
                    s.onActive.background = s.onNormal.background;

                    s.normal.textColor = t.Text;
                    s.onNormal.textColor = t.Accent;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Ghost,
                (s, t, h2) =>
                {
                    s.normal.background = transparentTexture;
                    s.onNormal.background = transparentTexture;

                    s.hover.background = transparentTexture;
                    s.active.background = transparentTexture;
                    s.onHover.background = transparentTexture;
                    s.onActive.background = transparentTexture;

                    s.normal.textColor = t.Text;
                    s.onNormal.textColor = t.Accent;
                }
            );

            RegisterVariant(
                StyleComponentType.Checkbox,
                ControlVariant.Muted,
                (s, t, h2) =>
                {
                    s.normal.background = fill(t.Muted);
                    s.onNormal.background = fill(Color.Lerp(t.Muted, t.Text, 0.3f));

                    s.hover.background = s.normal.background;
                    s.active.background = s.normal.background;
                    s.onHover.background = s.onNormal.background;
                    s.onActive.background = s.onNormal.background;

                    s.normal.textColor = t.Text;
                    s.onNormal.textColor = t.ButtonPrimaryFg;
                }
            );

            RegisterSize(StyleComponentType.Checkbox, ControlSize.Small, (s, t, h2) => s.fontSize = GetScaledFontSize(0.75f));
            RegisterSize(StyleComponentType.Checkbox, ControlSize.Large, (s, t, h2) => s.fontSize = GetScaledFontSize(1.0f));
            RegisterSize(StyleComponentType.Checkbox, ControlSize.Mini, (s, t, h2) => s.fontSize = GetScaledFontSize(0.7f));
            RegisterSize(StyleComponentType.Checkbox, ControlSize.Default, (s, t, h2) => s.fontSize = GetScaledFontSize(0.875f));
        }

        private void RegisterSwitchStyles()
        {
            var t = ThemeManager.Instance.CurrentTheme;

            int w = 32,
                h = 16;
            int r = GetScaledBorderRadius(6f);

            Func<Color, Texture2D> outline = c => CreateRoundedOutlineTexture(w, h, r, c, 1);
            Func<Color, Texture2D> fill = c => CreateRoundedRectTexture(w, h, r, c);

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Destructive,
                (s, x, hvr) =>
                {
                    s.normal.background = fill(x.Secondary);
                    s.onNormal.background = fill(x.Destructive);

                    s.hover.background = fill(Color.Lerp(x.Secondary, x.Text, 0.1f));
                    s.active.background = fill(x.Secondary);

                    s.onHover.background = fill(GetHoverColor(x.Destructive, true));
                    s.onActive.background = fill(Color.Lerp(x.Destructive, Color.black, 0.1f));
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Secondary,
                (s, x, hvr) =>
                {
                    s.normal.background = fill(x.Secondary);

                    s.hover.background = fill(Color.Lerp(x.Secondary, x.Text, 0.1f));
                    s.active.background = fill(x.Secondary);

                    s.onNormal.background = fill(x.Secondary);
                    s.onHover.background = fill(GetHoverColor(x.Secondary, true));
                    s.onActive.background = fill(Color.Lerp(x.Secondary, Color.black, 0.1f));
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Outline,
                (s, x, hvr) =>
                {
                    s.normal.background = outline(x.Border);
                    s.hover.background = outline(Color.Lerp(x.Border, x.Text, 0.2f));
                    s.active.background = outline(x.Border);

                    s.onNormal.background = outline(x.Accent);
                    s.onHover.background = outline(Color.Lerp(x.Accent, Color.white, 0.1f));
                    s.onActive.background = outline(x.Accent);
                }
            );

            RegisterVariant(
                StyleComponentType.Switch,
                ControlVariant.Ghost,
                (s, x, hvr) =>
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
                (s, x, hvr) =>
                {
                    s.normal.background = fill(x.Muted);
                    s.hover.background = fill(Color.Lerp(x.Muted, x.Text, 0.1f));
                    s.active.background = fill(x.Muted);

                    s.onNormal.background = fill(Color.Lerp(x.Muted, x.Text, 0.3f));
                    s.onHover.background = fill(Color.Lerp(x.Muted, x.Text, 0.5f));
                    s.onActive.background = fill(Color.Lerp(x.Muted, Color.black, 0.15f));
                }
            );

            RegisterSize(StyleComponentType.Switch, ControlSize.Small, (s, x, hvr) => s.fontSize = GetScaledFontSize(0.75f));
            RegisterSize(StyleComponentType.Switch, ControlSize.Large, (s, x, hvr) => s.fontSize = GetScaledFontSize(1.0f));
            RegisterSize(StyleComponentType.Switch, ControlSize.Mini, (s, x, hvr) => s.fontSize = GetScaledFontSize(0.7f));
            RegisterSize(StyleComponentType.Switch, ControlSize.Default, (s, x, hvr) => s.fontSize = GetScaledFontSize(0.875f));
        }

        private void RegisterPopoverStyles() => RegisterContainerSizes(StyleComponentType.Popover);

        private void RegisterDatePickerStyles()
        {
            RegisterContainerSizes(StyleComponentType.DatePicker);
            var dayTypes = new[] { StyleComponentType.DatePickerDay, StyleComponentType.DatePickerDaySelected, StyleComponentType.DatePickerDayOutsideMonth, StyleComponentType.DatePickerDayToday };
            foreach (var type in dayTypes)
            {
                RegisterSize(
                    type,
                    ControlSize.Small,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.8f);
                        s.fixedWidth = GetScaledHeight(28f);
                        s.fixedHeight = GetScaledHeight(28f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Large,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(1.0f);
                        s.fixedWidth = GetScaledHeight(40f);
                        s.fixedHeight = GetScaledHeight(40f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Mini,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.7f);
                        s.fixedWidth = GetScaledHeight(20f);
                        s.fixedHeight = GetScaledHeight(20f);
                    }
                );
                RegisterSize(
                    type,
                    ControlSize.Default,
                    (s, t, h) =>
                    {
                        s.fontSize = GetScaledFontSize(0.9f);
                        s.fixedWidth = GetScaledHeight(32f);
                        s.fixedHeight = GetScaledHeight(32f);
                    }
                );
            }
        }

        private void RegisterChartStyles() => RegisterContainerSizes(StyleComponentType.Chart);

        private void RegisterAnimatedBoxStyles() => RegisterContainerSizes(StyleComponentType.AnimatedBox);

        private void RegisterContainerSizes(StyleComponentType type, (ControlSize size, float fontSize, float hPad, float vPad)[] configs = null)
        {
            if (configs != null)
            {
                foreach (var (size, fs, hp, vp) in configs)
                    RegisterSize(
                        type,
                        size,
                        (s, t, h) =>
                        {
                            s.fontSize = GetScaledFontSize(fs);
                            s.padding = GetSpacingOffset(hp, vp);
                        }
                    );
            }
            else
            {
                RegisterSize(type, ControlSize.Small, (s, t, h) => s.padding = GetSpacingOffset(8f, 8f));
                RegisterSize(type, ControlSize.Large, (s, t, h) => s.padding = GetSpacingOffset(24f, 24f));
                RegisterSize(type, ControlSize.Mini, (s, t, h) => s.padding = GetSpacingOffset(4f, 4f));
                RegisterSize(type, ControlSize.Default, (s, t, h) => s.padding = GetSpacingOffset(16f, 16f));
            }
        }
        #endregion
    }
}
