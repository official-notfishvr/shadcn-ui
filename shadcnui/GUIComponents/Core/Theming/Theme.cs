using System;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Theming
{
    public sealed class ThemeMetrics
    {
        public float BorderWidth { get; set; } = 1f;
        public float CornerRadius { get; set; } = 2f;
        public float CompactCornerRadius { get; set; } = 1f;
        public float SectionSpacing { get; set; } = 16f;
        public float ControlSpacing { get; set; } = 8f;
        public float PanelPadding { get; set; } = 16f;
        public float ContentPadding { get; set; } = 12f;
        public float ControlHeight { get; set; } = 34f;
        public float CompactControlHeight { get; set; } = 28f;
        public float LargeControlHeight { get; set; } = 42f;
        public float TooltipMaxWidth { get; set; } = 280f;
        public float DropdownWidth { get; set; } = 280f;

        public ThemeMetrics Clone() => (ThemeMetrics)MemberwiseClone();
    }

    public sealed class ThemeTypography
    {
        public string[] FontFamilies { get; set; } = Array.Empty<string>();
        public int BaseFontSize { get; set; } = 14;
        public FontStyle HeadingWeight { get; set; } = FontStyle.Bold;
        public FontStyle LabelWeight { get; set; } = FontStyle.Normal;
        public FontStyle ButtonWeight { get; set; } = FontStyle.Bold;

        public ThemeTypography Clone()
        {
            return new ThemeTypography
            {
                FontFamilies = (string[])FontFamilies.Clone(),
                BaseFontSize = BaseFontSize,
                HeadingWeight = HeadingWeight,
                LabelWeight = LabelWeight,
                ButtonWeight = ButtonWeight,
            };
        }
    }

    public class Theme
    {
        public string Name { get; set; }
        public Color Base { get; set; }
        public Color Secondary { get; set; }
        public Color Elevated { get; set; }
        public Color Text { get; set; }
        public Color Muted { get; set; }
        public Color Border { get; set; }
        public Color Accent { get; set; }
        public Color Destructive { get; set; }
        public Color Success { get; set; }
        public Color Warning { get; set; }
        public Color Info { get; set; }
        public Color Overlay { get; set; }
        public Color Shadow { get; set; }
        public Color ButtonPrimaryBg { get; set; }
        public Color ButtonPrimaryFg { get; set; }
        public Color ButtonDestructiveBg { get; set; }
        public Color ButtonDestructiveFg { get; set; }
        public Color ButtonOutlineFg { get; set; }
        public Color ButtonSecondaryBg { get; set; }
        public Color ButtonSecondaryFg { get; set; }
        public Color ButtonGhostFg { get; set; }
        public Color ButtonLinkColor { get; set; }
        public Color TabsBg { get; set; }
        public Color TabsTriggerFg { get; set; }
        public Color TabsTriggerActiveBg { get; set; }
        public Color TabsTriggerActiveFg { get; set; }
        public Color BackgroundColor { get; set; }
        public ThemeMetrics Metrics { get; set; } = new();
        public ThemeTypography Typography { get; set; } = new();

        public Theme Clone()
        {
            return new Theme
            {
                Name = Name,
                Base = Base,
                Secondary = Secondary,
                Elevated = Elevated,
                Text = Text,
                Muted = Muted,
                Border = Border,
                Accent = Accent,
                Destructive = Destructive,
                Success = Success,
                Warning = Warning,
                Info = Info,
                Overlay = Overlay,
                Shadow = Shadow,
                ButtonPrimaryBg = ButtonPrimaryBg,
                ButtonPrimaryFg = ButtonPrimaryFg,
                ButtonDestructiveBg = ButtonDestructiveBg,
                ButtonDestructiveFg = ButtonDestructiveFg,
                ButtonOutlineFg = ButtonOutlineFg,
                ButtonSecondaryBg = ButtonSecondaryBg,
                ButtonSecondaryFg = ButtonSecondaryFg,
                ButtonGhostFg = ButtonGhostFg,
                ButtonLinkColor = ButtonLinkColor,
                TabsBg = TabsBg,
                TabsTriggerFg = TabsTriggerFg,
                TabsTriggerActiveBg = TabsTriggerActiveBg,
                TabsTriggerActiveFg = TabsTriggerActiveFg,
                BackgroundColor = BackgroundColor,
                Metrics = Metrics.Clone(),
                Typography = Typography.Clone(),
            };
        }

        public static Color Hex(string hex)
        {
            return ColorUtility.TryParseHtmlString(hex, out var color) ? color : Color.white;
        }

        private static Theme Create(
            string name,
            string @base,
            string secondary,
            string elevated,
            string text,
            string muted,
            string border,
            string accent,
            string destructive,
            string success,
            string warning,
            string info,
            string overlay,
            string shadow,
            string buttonPrimaryBg,
            string buttonPrimaryFg,
            string buttonSecondaryBg,
            string buttonSecondaryFg,
            string buttonGhostFg,
            string link,
            string tabsBg,
            string tabsActiveBg,
            string tabsActiveFg
        )
        {
            return new Theme
            {
                Name = name,
                Base = Hex(@base),
                Secondary = Hex(secondary),
                Elevated = Hex(elevated),
                Text = Hex(text),
                Muted = Hex(muted),
                Border = Hex(border),
                Accent = Hex(accent),
                Destructive = Hex(destructive),
                Success = Hex(success),
                Warning = Hex(warning),
                Info = Hex(info),
                Overlay = Hex(overlay),
                Shadow = Hex(shadow),
                ButtonPrimaryBg = Hex(buttonPrimaryBg),
                ButtonPrimaryFg = Hex(buttonPrimaryFg),
                ButtonDestructiveBg = Hex(destructive),
                ButtonDestructiveFg = Color.white,
                ButtonOutlineFg = Hex(text),
                ButtonSecondaryBg = Hex(buttonSecondaryBg),
                ButtonSecondaryFg = Hex(buttonSecondaryFg),
                ButtonGhostFg = Hex(buttonGhostFg),
                ButtonLinkColor = Hex(link),
                TabsBg = Hex(tabsBg),
                TabsTriggerFg = Hex(muted),
                TabsTriggerActiveBg = Hex(tabsActiveBg),
                TabsTriggerActiveFg = Hex(tabsActiveFg),
                BackgroundColor = Hex(@base),
            };
        }

        public static Theme Dark => Create("Dark", "#101214", "#171a1f", "#14181c", "#f3f5f7", "#9099a4", "#242a31", "#5aa2ff", "#d94b4b", "#2fbf71", "#e0a43b", "#5aa2ff", "#99000000", "#55000000", "#1b2026", "#f3f5f7", "#171c22", "#f3f5f7", "#e8edf3", "#7bb6ff", "#14181c", "#20262d", "#f3f5f7");

        public static Theme Light => Create("Light", "#f7f7f8", "#eceef0", "#ffffff", "#15181c", "#66707c", "#d9dde2", "#0f6bff", "#d44242", "#1ea85d", "#d48b19", "#2e7df6", "#66000000", "#15000000", "#15181c", "#ffffff", "#eceef0", "#15181c", "#15181c", "#0f6bff", "#eceef0", "#ffffff", "#15181c");

        public static Theme Slate => Create("Slate", "#111827", "#162033", "#131c2b", "#eff4ff", "#94a3b8", "#25324a", "#4da3ff", "#ef4444", "#22c55e", "#f59e0b", "#38bdf8", "#a0000000", "#66000000", "#1b283d", "#eff4ff", "#1a2435", "#eff4ff", "#eff4ff", "#4da3ff", "#131c2b", "#1b283d", "#eff4ff");

        public static Theme Gray => Create("Gray", "#111111", "#191919", "#141414", "#f5f5f5", "#9b9b9b", "#282828", "#7f8ea3", "#e05252", "#3fbf73", "#e3a546", "#6ea8ff", "#99000000", "#66000000", "#1d1d1d", "#f5f5f5", "#191919", "#f5f5f5", "#f5f5f5", "#88a4c4", "#141414", "#1d1d1d", "#f5f5f5");

        public static Theme Zinc => Create("Zinc", "#15161a", "#1c1f24", "#181b20", "#f3f4f6", "#979da6", "#2a2f37", "#4f8ee8", "#de4c4c", "#34c56f", "#e2a33b", "#63a6ff", "#9a000000", "#66000000", "#21252c", "#f3f4f6", "#1c1f24", "#f3f4f6", "#f3f4f6", "#79a8ff", "#181b20", "#20242b", "#f3f4f6");

        public static Theme Stone => Create("Stone", "#171311", "#211c18", "#1c1715", "#faf7f3", "#9a8f86", "#2e2621", "#c98244", "#d94b4b", "#35b76b", "#d9a045", "#6aa7ff", "#9a000000", "#66000000", "#231d19", "#faf7f3", "#211c18", "#faf7f3", "#faf7f3", "#c98244", "#1c1715", "#241f1a", "#faf7f3");

        public static Theme Olive => Create("Olive", "#12150f", "#1a1f17", "#161b13", "#f4f6ef", "#8e9886", "#2a3224", "#80b35c", "#d94b4b", "#43bd71", "#d2a43c", "#63a6ff", "#9a000000", "#66000000", "#1d2319", "#f4f6ef", "#1a1f17", "#f4f6ef", "#f4f6ef", "#8ec96a", "#161b13", "#1e241a", "#f4f6ef");

        public static Theme Cyan => Create("Cyan", "#0f171a", "#152025", "#121c20", "#eff9fb", "#90a8ad", "#26353a", "#3fb8d5", "#d94b4b", "#33bd78", "#dfa63d", "#51bbf4", "#99000000", "#66000000", "#1a2529", "#eff9fb", "#152025", "#eff9fb", "#eff9fb", "#3fb8d5", "#121c20", "#1b282d", "#eff9fb");

        public static Theme BlueDark =>
            Create("BlueDark", "#0f1623", "#162033", "#121b2b", "#eff4ff", "#92a0ba", "#25324a", "#5e91ff", "#df5050", "#37bc73", "#dfa43e", "#5fa9ff", "#9d000000", "#66000000", "#1a263a", "#eff4ff", "#162033", "#eff4ff", "#eff4ff", "#7ba7ff", "#121b2b", "#1c2940", "#eff4ff");

        public static Theme Rose => Create("Rose", "#1a1216", "#25171d", "#1f1419", "#fff3f8", "#b296a1", "#38242c", "#e97ea9", "#df4d67", "#3abc74", "#e1aa46", "#6ea8ff", "#9a000000", "#66000000", "#2a1a21", "#fff3f8", "#25171d", "#fff3f8", "#fff3f8", "#e97ea9", "#1f1419", "#2a1b22", "#fff3f8");

        public static Theme Violet =>
            Create("Violet", "#161222", "#21192f", "#1b1528", "#faf4ff", "#a793bd", "#322643", "#a07bff", "#df4b63", "#39bc74", "#e0a644", "#6ea8ff", "#99000000", "#66000000", "#261d37", "#faf4ff", "#21192f", "#faf4ff", "#faf4ff", "#a07bff", "#1b1528", "#261d37", "#faf4ff");
    }
}
