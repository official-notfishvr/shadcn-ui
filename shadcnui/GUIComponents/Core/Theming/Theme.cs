using UnityEngine;

namespace shadcnui.GUIComponents.Core.Theming
{
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

        public static Theme Dark =>
            new Theme
            {
                Name = "Dark",
                Base = new Color(0.055f, 0.059f, 0.075f),
                Secondary = new Color(0.098f, 0.106f, 0.137f),
                Elevated = new Color(0.078f, 0.086f, 0.114f),
                Text = new Color(0.965f, 0.969f, 0.980f),
                Muted = new Color(0.525f, 0.545f, 0.608f),
                Border = new Color(0.157f, 0.169f, 0.212f),
                Accent = new Color(0.380f, 0.565f, 0.980f),
                Destructive = new Color(0.910f, 0.298f, 0.286f),
                Success = new Color(0.255f, 0.725f, 0.435f),
                Warning = new Color(0.965f, 0.690f, 0.275f),
                Info = new Color(0.380f, 0.565f, 0.980f),
                Overlay = new Color(0f, 0f, 0f, 0.50f),
                Shadow = new Color(0f, 0f, 0f, 0.40f),
                ButtonPrimaryBg = new Color(0.16f, 0.17f, 0.21f),
                ButtonPrimaryFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonDestructiveBg = new Color(0.910f, 0.298f, 0.286f),
                ButtonDestructiveFg = new Color(0.98f, 0.98f, 1.0f),
                ButtonOutlineFg = new Color(0.965f, 0.969f, 0.980f),
                ButtonSecondaryBg = new Color(0.137f, 0.149f, 0.188f),
                ButtonSecondaryFg = new Color(0.965f, 0.969f, 0.980f),
                ButtonGhostFg = new Color(0.965f, 0.969f, 0.980f),
                ButtonLinkColor = new Color(0.380f, 0.565f, 0.980f),
                TabsBg = new Color(0.078f, 0.086f, 0.114f),
                TabsTriggerFg = new Color(0.525f, 0.545f, 0.608f),
                TabsTriggerActiveBg = new Color(0.137f, 0.149f, 0.188f),
                TabsTriggerActiveFg = new Color(0.965f, 0.969f, 0.980f),
                BackgroundColor = new Color(0.055f, 0.059f, 0.075f),
            };

        public static Theme Light =>
            new Theme
            {
                Name = "Light",
                Base = new Color(1.0f, 1.0f, 1.0f),
                Secondary = new Color(0.965f, 0.969f, 0.976f),
                Elevated = new Color(1.0f, 1.0f, 1.0f),
                Text = new Color(0.078f, 0.086f, 0.102f),
                Muted = new Color(0.412f, 0.443f, 0.502f),
                Border = new Color(0.894f, 0.902f, 0.918f),
                Accent = new Color(0.212f, 0.455f, 0.878f),
                Destructive = new Color(0.878f, 0.227f, 0.227f),
                Success = new Color(0.133f, 0.584f, 0.341f),
                Warning = new Color(0.902f, 0.580f, 0.133f),
                Info = new Color(0.212f, 0.455f, 0.878f),
                Overlay = new Color(0f, 0f, 0f, 0.25f),
                Shadow = new Color(0f, 0f, 0f, 0.10f),
                ButtonPrimaryBg = new Color(0.078f, 0.086f, 0.102f),
                ButtonPrimaryFg = new Color(1.0f, 1.0f, 1.0f),
                ButtonDestructiveBg = new Color(0.878f, 0.227f, 0.227f),
                ButtonDestructiveFg = new Color(1.0f, 1.0f, 1.0f),
                ButtonOutlineFg = new Color(0.078f, 0.086f, 0.102f),
                ButtonSecondaryBg = new Color(0.945f, 0.949f, 0.957f),
                ButtonSecondaryFg = new Color(0.078f, 0.086f, 0.102f),
                ButtonGhostFg = new Color(0.078f, 0.086f, 0.102f),
                ButtonLinkColor = new Color(0.212f, 0.455f, 0.878f),
                TabsBg = new Color(0.965f, 0.969f, 0.976f),
                TabsTriggerFg = new Color(0.412f, 0.443f, 0.502f),
                TabsTriggerActiveBg = new Color(1.0f, 1.0f, 1.0f),
                TabsTriggerActiveFg = new Color(0.078f, 0.086f, 0.102f),
                BackgroundColor = new Color(1.0f, 1.0f, 1.0f),
            };

        public static Theme Slate =>
            new Theme
            {
                Name = "Slate",
                Base = new Color(0.008f, 0.012f, 0.020f),
                Secondary = new Color(0.094f, 0.125f, 0.161f),
                Elevated = new Color(0.008f, 0.012f, 0.020f),
                Text = new Color(0.973f, 0.976f, 0.980f),
                Muted = new Color(0.576f, 0.608f, 0.659f),
                Border = new Color(0.094f, 0.125f, 0.161f),
                Accent = new Color(0.973f, 0.976f, 0.980f),
                Destructive = new Color(0.494f, 0.114f, 0.114f),
                Success = new Color(0.22f, 0.706f, 0.42f),
                Warning = new Color(0.98f, 0.78f, 0.38f),
                Info = new Color(0.2f, 0.49f, 0.98f),
                Overlay = new Color(0f, 0f, 0f, 0.5f),
                Shadow = new Color(0f, 0f, 0f, 0.3f),
                ButtonPrimaryBg = new Color(0.973f, 0.976f, 0.980f),
                ButtonPrimaryFg = new Color(0.008f, 0.012f, 0.020f),
                ButtonDestructiveBg = new Color(0.494f, 0.114f, 0.114f),
                ButtonDestructiveFg = new Color(0.973f, 0.976f, 0.980f),
                ButtonOutlineFg = new Color(0.973f, 0.976f, 0.980f),
                ButtonSecondaryBg = new Color(0.094f, 0.125f, 0.161f),
                ButtonSecondaryFg = new Color(0.973f, 0.976f, 0.980f),
                ButtonGhostFg = new Color(0.973f, 0.976f, 0.980f),
                ButtonLinkColor = new Color(0.973f, 0.976f, 0.980f),
                TabsBg = new Color(0.008f, 0.012f, 0.020f),
                TabsTriggerFg = new Color(0.576f, 0.608f, 0.659f),
                TabsTriggerActiveBg = new Color(0.094f, 0.125f, 0.161f),
                TabsTriggerActiveFg = new Color(0.973f, 0.976f, 0.980f),
                BackgroundColor = new Color(0.008f, 0.012f, 0.020f),
            };

        public static Theme Gray =>
            new Theme
            {
                Name = "Gray",
                Base = new Color(0.035f, 0.035f, 0.039f),
                Secondary = new Color(0.149f, 0.149f, 0.161f),
                Elevated = new Color(0.035f, 0.035f, 0.039f),
                Text = new Color(0.980f, 0.980f, 0.980f),
                Muted = new Color(0.639f, 0.639f, 0.639f),
                Border = new Color(0.149f, 0.149f, 0.161f),
                Accent = new Color(0.980f, 0.980f, 0.980f),
                Destructive = new Color(0.494f, 0.114f, 0.114f),
                Success = new Color(0.22f, 0.706f, 0.42f),
                Warning = new Color(0.98f, 0.78f, 0.38f),
                Info = new Color(0.2f, 0.49f, 0.98f),
                Overlay = new Color(0f, 0f, 0f, 0.5f),
                Shadow = new Color(0f, 0f, 0f, 0.3f),
                ButtonPrimaryBg = new Color(0.980f, 0.980f, 0.980f),
                ButtonPrimaryFg = new Color(0.035f, 0.035f, 0.039f),
                ButtonDestructiveBg = new Color(0.494f, 0.114f, 0.114f),
                ButtonDestructiveFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonOutlineFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonSecondaryBg = new Color(0.149f, 0.149f, 0.161f),
                ButtonSecondaryFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonGhostFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonLinkColor = new Color(0.980f, 0.980f, 0.980f),
                TabsBg = new Color(0.035f, 0.035f, 0.039f),
                TabsTriggerFg = new Color(0.639f, 0.639f, 0.639f),
                TabsTriggerActiveBg = new Color(0.149f, 0.149f, 0.161f),
                TabsTriggerActiveFg = new Color(0.980f, 0.980f, 0.980f),
                BackgroundColor = new Color(0.035f, 0.035f, 0.039f),
            };

        public static Theme Zinc =>
            new Theme
            {
                Name = "Zinc",
                Base = new Color(0.035f, 0.035f, 0.039f),
                Secondary = new Color(0.149f, 0.149f, 0.157f),
                Elevated = new Color(0.035f, 0.035f, 0.039f),
                Text = new Color(0.980f, 0.980f, 0.980f),
                Muted = new Color(0.639f, 0.639f, 0.639f),
                Border = new Color(0.149f, 0.149f, 0.157f),
                Accent = new Color(0.980f, 0.980f, 0.980f),
                Destructive = new Color(0.494f, 0.114f, 0.114f),
                Success = new Color(0.22f, 0.706f, 0.42f),
                Warning = new Color(0.98f, 0.78f, 0.38f),
                Info = new Color(0.2f, 0.49f, 0.98f),
                Overlay = new Color(0f, 0f, 0f, 0.5f),
                Shadow = new Color(0f, 0f, 0f, 0.3f),
                ButtonPrimaryBg = new Color(0.980f, 0.980f, 0.980f),
                ButtonPrimaryFg = new Color(0.035f, 0.035f, 0.039f),
                ButtonDestructiveBg = new Color(0.494f, 0.114f, 0.114f),
                ButtonDestructiveFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonOutlineFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonSecondaryBg = new Color(0.149f, 0.149f, 0.157f),
                ButtonSecondaryFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonGhostFg = new Color(0.980f, 0.980f, 0.980f),
                ButtonLinkColor = new Color(0.980f, 0.980f, 0.980f),
                TabsBg = new Color(0.035f, 0.035f, 0.039f),
                TabsTriggerFg = new Color(0.639f, 0.639f, 0.639f),
                TabsTriggerActiveBg = new Color(0.149f, 0.149f, 0.157f),
                TabsTriggerActiveFg = new Color(0.980f, 0.980f, 0.980f),
                BackgroundColor = new Color(0.035f, 0.035f, 0.039f),
            };

        public static Theme Stone =>
            new Theme
            {
                Name = "Stone",
                Base = new Color(0.047f, 0.043f, 0.039f),
                Secondary = new Color(0.169f, 0.161f, 0.153f),
                Elevated = new Color(0.047f, 0.043f, 0.039f),
                Text = new Color(0.980f, 0.980f, 0.976f),
                Muted = new Color(0.659f, 0.643f, 0.612f),
                Border = new Color(0.169f, 0.161f, 0.153f),
                Accent = new Color(0.980f, 0.980f, 0.976f),
                Destructive = new Color(0.494f, 0.114f, 0.114f),
                Success = new Color(0.22f, 0.706f, 0.42f),
                Warning = new Color(0.98f, 0.78f, 0.38f),
                Info = new Color(0.2f, 0.49f, 0.98f),
                Overlay = new Color(0f, 0f, 0f, 0.5f),
                Shadow = new Color(0f, 0f, 0f, 0.3f),
                ButtonPrimaryBg = new Color(0.980f, 0.980f, 0.976f),
                ButtonPrimaryFg = new Color(0.047f, 0.043f, 0.039f),
                ButtonDestructiveBg = new Color(0.494f, 0.114f, 0.114f),
                ButtonDestructiveFg = new Color(0.980f, 0.980f, 0.976f),
                ButtonOutlineFg = new Color(0.980f, 0.980f, 0.976f),
                ButtonSecondaryBg = new Color(0.169f, 0.161f, 0.153f),
                ButtonSecondaryFg = new Color(0.980f, 0.980f, 0.976f),
                ButtonGhostFg = new Color(0.980f, 0.980f, 0.976f),
                ButtonLinkColor = new Color(0.980f, 0.980f, 0.976f),
                TabsBg = new Color(0.047f, 0.043f, 0.039f),
                TabsTriggerFg = new Color(0.659f, 0.643f, 0.612f),
                TabsTriggerActiveBg = new Color(0.169f, 0.161f, 0.153f),
                TabsTriggerActiveFg = new Color(0.980f, 0.980f, 0.976f),
                BackgroundColor = new Color(0.047f, 0.043f, 0.039f),
            };

        public static Theme Olive =>
            new Theme
            {
                Name = "Olive",
                Base = new Color(0.035f, 0.051f, 0.035f),
                Secondary = new Color(0.094f, 0.122f, 0.094f),
                Elevated = new Color(0.02f, 0.027f, 0.02f),
                Text = new Color(0.98f, 0.98f, 0.98f),
                Muted = new Color(0.678f, 0.694f, 0.678f),
                Border = new Color(0.18f, 0.2f, 0.18f),
                Accent = new Color(0.38f, 0.69f, 0.38f),
                Destructive = new Color(0.98f, 0.341f, 0.341f),
                Success = new Color(0.22f, 0.706f, 0.42f),
                Warning = new Color(0.98f, 0.78f, 0.38f),
                Info = new Color(0.38f, 0.69f, 0.98f),
                Overlay = new Color(0f, 0f, 0f, 0.5f),
                Shadow = new Color(0f, 0f, 0f, 0.3f),
                ButtonPrimaryBg = new Color(0.18f, 0.2f, 0.18f),
                ButtonPrimaryFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonDestructiveBg = new Color(0.98f, 0.341f, 0.341f),
                ButtonDestructiveFg = new Color(0.02f, 0.027f, 0.02f),
                ButtonOutlineFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonSecondaryBg = new Color(0.094f, 0.122f, 0.094f),
                ButtonSecondaryFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonGhostFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonLinkColor = new Color(0.38f, 0.69f, 0.38f),
                TabsBg = new Color(0.18f, 0.2f, 0.18f, 0.6f),
                TabsTriggerFg = new Color(0.678f, 0.694f, 0.678f),
                TabsTriggerActiveBg = new Color(0.035f, 0.051f, 0.035f),
                TabsTriggerActiveFg = new Color(0.98f, 0.98f, 0.98f),
                BackgroundColor = new Color(0.02f, 0.027f, 0.02f),
            };

        public static Theme Cyan =>
            new Theme
            {
                Name = "Cyan",
                Base = new Color(0.02f, 0.051f, 0.063f),
                Secondary = new Color(0.08f, 0.145f, 0.165f),
                Elevated = new Color(0.01f, 0.027f, 0.035f),
                Text = new Color(0.98f, 0.98f, 0.98f),
                Muted = new Color(0.69f, 0.78f, 0.82f),
                Border = new Color(0.16f, 0.22f, 0.243f),
                Accent = new Color(0.2f, 0.78f, 0.98f),
                Destructive = new Color(0.98f, 0.341f, 0.341f),
                Success = new Color(0.22f, 0.706f, 0.42f),
                Warning = new Color(0.98f, 0.78f, 0.38f),
                Info = new Color(0.38f, 0.69f, 0.98f),
                Overlay = new Color(0f, 0f, 0f, 0.5f),
                Shadow = new Color(0f, 0f, 0f, 0.3f),
                ButtonPrimaryBg = new Color(0.16f, 0.22f, 0.243f),
                ButtonPrimaryFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonDestructiveBg = new Color(0.98f, 0.341f, 0.341f),
                ButtonDestructiveFg = new Color(0.01f, 0.027f, 0.035f),
                ButtonOutlineFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonSecondaryBg = new Color(0.08f, 0.145f, 0.165f),
                ButtonSecondaryFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonGhostFg = new Color(0.98f, 0.98f, 0.98f),
                ButtonLinkColor = new Color(0.2f, 0.78f, 0.98f),
                TabsBg = new Color(0.16f, 0.22f, 0.243f, 0.6f),
                TabsTriggerFg = new Color(0.69f, 0.78f, 0.82f),
                TabsTriggerActiveBg = new Color(0.02f, 0.051f, 0.063f),
                TabsTriggerActiveFg = new Color(0.98f, 0.98f, 0.98f),
                BackgroundColor = new Color(0.01f, 0.027f, 0.035f),
            };

        public static Theme BlueDark =>
            new Theme
            {
                Name = "BlueDark",
                Base = new Color(0.06f, 0.09f, 0.16f),
                Secondary = new Color(0.14f, 0.18f, 0.28f),
                Elevated = new Color(0.09f, 0.12f, 0.20f),
                Text = new Color(0.96f, 0.98f, 1.0f),
                Muted = new Color(0.55f, 0.60f, 0.70f),
                Border = new Color(0.20f, 0.25f, 0.35f),
                Accent = new Color(0.25f, 0.55f, 0.95f),
                Destructive = new Color(0.95f, 0.3f, 0.3f),
                Success = new Color(0.2f, 0.75f, 0.45f),
                Warning = new Color(0.95f, 0.75f, 0.25f),
                Info = new Color(0.25f, 0.55f, 0.95f),
                Overlay = new Color(0.02f, 0.04f, 0.1f, 0.6f),
                Shadow = new Color(0f, 0f, 0f, 0.4f),
                ButtonPrimaryBg = new Color(0.25f, 0.55f, 0.95f),
                ButtonPrimaryFg = new Color(1.0f, 1.0f, 1.0f),
                ButtonDestructiveBg = new Color(0.95f, 0.3f, 0.3f),
                ButtonDestructiveFg = new Color(1.0f, 1.0f, 1.0f),
                ButtonOutlineFg = new Color(0.96f, 0.98f, 1.0f),
                ButtonSecondaryBg = new Color(0.14f, 0.18f, 0.28f),
                ButtonSecondaryFg = new Color(0.96f, 0.98f, 1.0f),
                ButtonGhostFg = new Color(0.96f, 0.98f, 1.0f),
                ButtonLinkColor = new Color(0.25f, 0.55f, 0.95f),
                TabsBg = new Color(0.09f, 0.12f, 0.20f, 0.8f),
                TabsTriggerFg = new Color(0.55f, 0.60f, 0.70f),
                TabsTriggerActiveBg = new Color(0.14f, 0.18f, 0.28f),
                TabsTriggerActiveFg = new Color(0.96f, 0.98f, 1.0f),
                BackgroundColor = new Color(0.06f, 0.09f, 0.16f),
            };
    }
}
