using UnityEngine;

namespace shadcnui.GUIComponents.Core.Theming
{
    public class Theme
    {
        #region Properties

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

        #endregion

        public static Color Hex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                return color;
            return Color.white;
        }

        #region Theme Definitions

        public static Theme Dark =>
            new Theme
            {
                Name = "Dark",
                Base = Hex("#0c0c0e"),
                Secondary = Hex("#16161a"),
                Elevated = Hex("#111114"),
                Text = Hex("#f8f8f8"),
                Muted = Hex("#808496"),
                Border = Hex("#252832"),
                Accent = Hex("#5b8cff"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#3cc171"),
                Warning = Hex("#fbba42"),
                Info = Hex("#5b8cff"),
                Overlay = new Color(0, 0, 0, 0.6f),
                Shadow = new Color(0, 0, 0, 0.5f),
                ButtonPrimaryBg = Hex("#252832"),
                ButtonPrimaryFg = Hex("#f8f8f8"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f8f8f8"),
                ButtonSecondaryBg = Hex("#1f222b"),
                ButtonSecondaryFg = Hex("#f8f8f8"),
                ButtonGhostFg = Hex("#f8f8f8"),
                ButtonLinkColor = Hex("#5b8cff"),
                TabsBg = Hex("#111114"),
                TabsTriggerFg = Hex("#808496"),
                TabsTriggerActiveBg = Hex("#1f222b"),
                TabsTriggerActiveFg = Hex("#f8f8f8"),
                BackgroundColor = Hex("#0c0c0e"),
            };

        public static Theme Light =>
            new Theme
            {
                Name = "Light",
                Base = Hex("#ffffff"),
                Secondary = Hex("#f4f5f7"),
                Elevated = Hex("#ffffff"),
                Text = Hex("#101217"),
                Muted = Hex("#656d7d"),
                Border = Hex("#e1e3e7"),
                Accent = Hex("#2268de"),
                Destructive = Hex("#dc3232"),
                Success = Hex("#1a8e4f"),
                Warning = Hex("#de8c18"),
                Info = Hex("#2268de"),
                Overlay = new Color(0, 0, 0, 0.3f),
                Shadow = new Color(0, 0, 0, 0.08f),
                ButtonPrimaryBg = Hex("#101217"),
                ButtonPrimaryFg = Hex("#fafafa"),
                ButtonDestructiveBg = Hex("#dc3232"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#101217"),
                ButtonSecondaryBg = Hex("#efeff2"),
                ButtonSecondaryFg = Hex("#101217"),
                ButtonGhostFg = Hex("#101217"),
                ButtonLinkColor = Hex("#2268de"),
                TabsBg = Hex("#f4f5f7"),
                TabsTriggerFg = Hex("#656d7d"),
                TabsTriggerActiveBg = Hex("#ffffff"),
                TabsTriggerActiveFg = Hex("#101217"),
                BackgroundColor = Hex("#ffffff"),
            };

        public static Theme Zinc =>
            new Theme
            {
                Name = "Zinc",
                Base = Hex("#030406"),
                Secondary = Hex("#101318"),
                Elevated = Hex("#090c10"),
                Text = Hex("#f0f3f7"),
                Muted = Hex("#7b848f"),
                Border = Hex("#191f27"),
                Accent = Hex("#5c89ba"),
                Destructive = Hex("#dc3f4a"),
                Success = Hex("#1a8e4f"),
                Warning = Hex("#fbba42"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.8f),
                Shadow = new Color(0, 0, 0, 0.5f),
                ButtonPrimaryBg = Hex("#5c89ba"),
                ButtonPrimaryFg = Hex("#fcfdfe"),
                ButtonDestructiveBg = Hex("#dc3f4a"),
                ButtonDestructiveFg = Hex("#f0f3f7"),
                ButtonOutlineFg = Hex("#f0f3f7"),
                ButtonSecondaryBg = Hex("#101318"),
                ButtonSecondaryFg = Hex("#f0f3f7"),
                ButtonGhostFg = Hex("#f0f3f7"),
                ButtonLinkColor = Hex("#5c89ba"),
                TabsBg = Hex("#090c10"),
                TabsTriggerFg = Hex("#7b848f"),
                TabsTriggerActiveBg = Hex("#101318"),
                TabsTriggerActiveFg = Hex("#f0f3f7"),
                BackgroundColor = Hex("#030406"),
            };

        public static Theme Slate =>
            new Theme
            {
                Name = "Slate",
                Base = Hex("#020617"),
                Secondary = Hex("#0f172a"),
                Elevated = Hex("#0b1120"),
                Text = Hex("#f1f5f9"),
                Muted = Hex("#64748b"),
                Border = Hex("#1e293b"),
                Accent = Hex("#6d90ad"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#10b981"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#6d90ad"),
                ButtonPrimaryFg = Hex("#f8fafc"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f1f5f9"),
                ButtonSecondaryBg = Hex("#1e293b"),
                ButtonSecondaryFg = Hex("#f1f5f9"),
                ButtonGhostFg = Hex("#f1f5f9"),
                ButtonLinkColor = Hex("#6d90ad"),
                BackgroundColor = Hex("#020617"),
                TabsBg = Hex("#0b1120"),
                TabsTriggerFg = Hex("#64748b"),
                TabsTriggerActiveBg = Hex("#0f172a"),
                TabsTriggerActiveFg = Hex("#f1f5f9"),
            };

        public static Theme Gray =>
            new Theme
            {
                Name = "Gray",
                Base = Hex("#030303"),
                Secondary = Hex("#111111"),
                Elevated = Hex("#0a0a0a"),
                Text = Hex("#f2f2f2"),
                Muted = Hex("#71717a"),
                Border = Hex("#27272a"),
                Accent = Hex("#3b82f6"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#27272a"),
                ButtonPrimaryFg = Hex("#f2f2f2"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f2f2f2"),
                ButtonSecondaryBg = Hex("#27272a"),
                ButtonSecondaryFg = Hex("#f2f2f2"),
                ButtonGhostFg = Hex("#f2f2f2"),
                ButtonLinkColor = Hex("#3b82f6"),
                BackgroundColor = Hex("#030303"),
                TabsBg = Hex("#0a0a0a"),
                TabsTriggerFg = Hex("#71717a"),
                TabsTriggerActiveBg = Hex("#111111"),
                TabsTriggerActiveFg = Hex("#f2f2f2"),
            };

        public static Theme Stone =>
            new Theme
            {
                Name = "Stone",
                Base = Hex("#0c0a09"),
                Secondary = Hex("#1c1917"),
                Elevated = Hex("#141312"),
                Text = Hex("#fafaf9"),
                Muted = Hex("#78716c"),
                Border = Hex("#292524"),
                Accent = Hex("#b45309"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#292524"),
                ButtonPrimaryFg = Hex("#fafaf9"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#fafaf9"),
                ButtonSecondaryBg = Hex("#292524"),
                ButtonSecondaryFg = Hex("#fafaf9"),
                ButtonGhostFg = Hex("#fafaf9"),
                ButtonLinkColor = Hex("#b45309"),
                BackgroundColor = Hex("#0c0a09"),
                TabsBg = Hex("#141312"),
                TabsTriggerFg = Hex("#78716c"),
                TabsTriggerActiveBg = Hex("#1c1917"),
                TabsTriggerActiveFg = Hex("#fafaf9"),
            };

        public static Theme Olive =>
            new Theme
            {
                Name = "Olive",
                Base = Hex("#040604"),
                Secondary = Hex("#0e130e"),
                Elevated = Hex("#090c09"),
                Text = Hex("#f5f7f5"),
                Muted = Hex("#848f84"),
                Border = Hex("#192119"),
                Accent = Hex("#6dba6d"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#192119"),
                ButtonPrimaryFg = Hex("#f5f7f5"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f5f7f5"),
                ButtonSecondaryBg = Hex("#192119"),
                ButtonSecondaryFg = Hex("#f5f7f5"),
                ButtonGhostFg = Hex("#f5f7f5"),
                ButtonLinkColor = Hex("#6dba6d"),
                BackgroundColor = Hex("#040604"),
                TabsBg = Hex("#090c09"),
                TabsTriggerFg = Hex("#848f84"),
                TabsTriggerActiveBg = Hex("#0e130e"),
                TabsTriggerActiveFg = Hex("#f5f7f5"),
            };

        public static Theme Cyan =>
            new Theme
            {
                Name = "Cyan",
                Base = Hex("#020507"),
                Secondary = Hex("#0b1317"),
                Elevated = Hex("#060c0f"),
                Text = Hex("#f3f7f9"),
                Muted = Hex("#7f97a3"),
                Border = Hex("#162329"),
                Accent = Hex("#39baba"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#162329"),
                ButtonPrimaryFg = Hex("#f3f7f9"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f3f7f9"),
                ButtonSecondaryBg = Hex("#162329"),
                ButtonSecondaryFg = Hex("#f3f7f9"),
                ButtonGhostFg = Hex("#f3f7f9"),
                ButtonLinkColor = Hex("#39baba"),
                BackgroundColor = Hex("#020507"),
                TabsBg = Hex("#060c0f"),
                TabsTriggerFg = Hex("#7f97a3"),
                TabsTriggerActiveBg = Hex("#0b1317"),
                TabsTriggerActiveFg = Hex("#f3f7f9"),
            };

        public static Theme BlueDark =>
            new Theme
            {
                Name = "BlueDark",
                Base = Hex("#050912"),
                Secondary = Hex("#101726"),
                Elevated = Hex("#0b101d"),
                Text = Hex("#f5f8ff"),
                Muted = Hex("#7f8ba3"),
                Border = Hex("#192335"),
                Accent = Hex("#5182f4"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#192335"),
                ButtonPrimaryFg = Hex("#f5f8ff"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f5f8ff"),
                ButtonSecondaryBg = Hex("#192335"),
                ButtonSecondaryFg = Hex("#f5f8ff"),
                ButtonGhostFg = Hex("#f5f8ff"),
                ButtonLinkColor = Hex("#5182f4"),
                BackgroundColor = Hex("#050912"),
                TabsBg = Hex("#0b101d"),
                TabsTriggerFg = Hex("#7f8ba3"),
                TabsTriggerActiveBg = Hex("#101726"),
                TabsTriggerActiveFg = Hex("#f5f8ff"),
            };

        public static Theme Rose =>
            new Theme
            {
                Name = "Rose",
                Base = Hex("#070406"),
                Secondary = Hex("#130b10"),
                Elevated = Hex("#0e090c"),
                Text = Hex("#faf5f8"),
                Muted = Hex("#977784"),
                Border = Hex("#211319"),
                Accent = Hex("#f45c89"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#211319"),
                ButtonPrimaryFg = Hex("#faf5f8"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#faf5f8"),
                ButtonSecondaryBg = Hex("#211319"),
                ButtonSecondaryFg = Hex("#faf5f8"),
                ButtonGhostFg = Hex("#faf5f8"),
                ButtonLinkColor = Hex("#f45c89"),
                BackgroundColor = Hex("#070406"),
                TabsBg = Hex("#0e090c"),
                TabsTriggerFg = Hex("#977784"),
                TabsTriggerActiveBg = Hex("#130b10"),
                TabsTriggerActiveFg = Hex("#faf5f8"),
            };

        public static Theme Violet =>
            new Theme
            {
                Name = "Violet",
                Base = Hex("#050409"),
                Secondary = Hex("#0f0b14"),
                Elevated = Hex("#0b0810"),
                Text = Hex("#f8f5ff"),
                Muted = Hex("#8b779f"),
                Border = Hex("#1d1326"),
                Accent = Hex("#9b5cf4"),
                Destructive = Hex("#ef4444"),
                Success = Hex("#22c55e"),
                Warning = Hex("#f59e0b"),
                Info = Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.5f),
                Shadow = new Color(0, 0, 0, 0.3f),
                ButtonPrimaryBg = Hex("#1d1326"),
                ButtonPrimaryFg = Hex("#f8f5ff"),
                ButtonDestructiveBg = Hex("#ef4444"),
                ButtonDestructiveFg = Hex("#ffffff"),
                ButtonOutlineFg = Hex("#f8f5ff"),
                ButtonSecondaryBg = Hex("#1d1326"),
                ButtonSecondaryFg = Hex("#f8f5ff"),
                ButtonGhostFg = Hex("#f8f5ff"),
                ButtonLinkColor = Hex("#9b5cf4"),
                BackgroundColor = Hex("#050409"),
                TabsBg = Hex("#0b0810"),
                TabsTriggerFg = Hex("#8b779f"),
                TabsTriggerActiveBg = Hex("#0f0b14"),
                TabsTriggerActiveFg = Hex("#f8f5ff"),
            };

        #endregion
    }
}
