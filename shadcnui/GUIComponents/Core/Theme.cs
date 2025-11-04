using UnityEngine;

namespace shadcnui.GUIComponents.Core
{
    public class Theme
    {
        public string Name { get; set; }

        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public Color AccentColor { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Color MutedColor { get; set; }

        public Color BorderColor { get; set; }
        public Color OverlayColor { get; set; }
        public Color ShadowColor { get; set; }

        public Color SuccessBg { get; set; }
        public Color SuccessFg { get; set; }
        public Color WarningBg { get; set; }
        public Color WarningFg { get; set; }
        public Color InfoBg { get; set; }
        public Color InfoFg { get; set; }

        public Color ButtonPrimaryBg { get; set; }
        public Color ButtonPrimaryFg { get; set; }
        public Color ButtonPrimaryActiveBg { get; set; }
        public Color ButtonPrimaryActiveFg { get; set; }
        public Color ButtonDestructiveBg { get; set; }
        public Color ButtonDestructiveFg { get; set; }
        public Color ButtonOutlineBorder { get; set; }
        public Color ButtonOutlineBg { get; set; }
        public Color ButtonOutlineFg { get; set; }
        public Color ButtonOutlineHoverBg { get; set; }
        public Color ButtonOutlineHoverFg { get; set; }
        public Color ButtonSecondaryBg { get; set; }
        public Color ButtonSecondaryFg { get; set; }
        public Color ButtonGhostFg { get; set; }
        public Color ButtonGhostHoverBg { get; set; }
        public Color ButtonGhostHoverFg { get; set; }
        public Color ButtonLinkColor { get; set; }
        public Color ButtonLinkHoverColor { get; set; }

        public Color InputBg { get; set; }
        public Color InputFg { get; set; }
        public Color InputFocusedBg { get; set; }
        public Color InputFocusedFg { get; set; }
        public Color InputDisabledBg { get; set; }
        public Color InputDisabledFg { get; set; }

        public Color CardBg { get; set; }
        public Color CardFg { get; set; }
        public Color CardTitle { get; set; }
        public Color CardDescription { get; set; }

        public Color ToggleBg { get; set; }
        public Color ToggleFg { get; set; }
        public Color ToggleHoverBg { get; set; }
        public Color ToggleHoverFg { get; set; }
        public Color ToggleOnBg { get; set; }
        public Color ToggleOnFg { get; set; }

        public Color SeparatorColor { get; set; }

        public Color TabsBg { get; set; }
        public Color TabsTriggerFg { get; set; }
        public Color TabsTriggerActiveBg { get; set; }
        public Color TabsTriggerActiveFg { get; set; }

        public Color CheckboxBg { get; set; }
        public Color CheckboxCheckedBg { get; set; }

        public Color SwitchBg { get; set; }
        public Color SwitchOnBg { get; set; }
        public Color SwitchOffBg { get; set; }

        public Color BadgeBg { get; set; }
        public Color BadgeSecondaryBg { get; set; }
        public Color BadgeDestructiveBg { get; set; }

        public Color AvatarBg { get; set; }
        public Color AvatarFallbackBg { get; set; }
        public Color AvatarFallbackFg { get; set; }

        public Color TableBg { get; set; }
        public Color TableHeaderBg { get; set; }
        public Color TableCellBg { get; set; }

        public Color ChartBg { get; set; }
        public Color ChartGridColor { get; set; }
        public Color ChartAxisColor { get; set; }

        public static Theme Dark = new Theme
        {
            Name = "Dark",
            PrimaryColor = new Color(0.012f, 0.027f, 0.071f),
            SecondaryColor = new Color(0.118f, 0.161f, 0.231f),
            AccentColor = new Color(0.378f, 0.631f, 0.969f),
            BackgroundColor = new Color(0.012f, 0.027f, 0.071f),
            TextColor = new Color(0.980f, 0.988f, 0.996f),
            MutedColor = new Color(0.639f, 0.651f, 0.667f),
            BorderColor = new Color(0.118f, 0.161f, 0.231f),
            OverlayColor = new Color(0f, 0f, 0f, 0.35f),
            ShadowColor = new Color(0f, 0f, 0f, 0.5f),

            SuccessBg = new Color(0.188f, 0.569f, 0.306f),
            SuccessFg = new Color(0.980f, 0.988f, 0.996f),
            WarningBg = new Color(0.871f, 0.702f, 0.251f),
            WarningFg = new Color(0.071f, 0.078f, 0.090f),
            InfoBg = new Color(0.173f, 0.388f, 0.969f),
            InfoFg = new Color(0.980f, 0.988f, 0.996f),

            ButtonPrimaryBg = new Color(0.059f, 0.071f, 0.165f),
            ButtonPrimaryFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonPrimaryActiveBg = new Color(0.071f, 0.086f, 0.196f),
            ButtonPrimaryActiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonDestructiveBg = new Color(0.937f, 0.266f, 0.266f),
            ButtonDestructiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineBorder = new Color(0.118f, 0.161f, 0.231f),
            ButtonOutlineBg = new Color(0.012f, 0.027f, 0.071f, 0.0f),
            ButtonOutlineFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineHoverBg = new Color(0.059f, 0.071f, 0.165f, 0.1f),
            ButtonOutlineHoverFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonSecondaryBg = new Color(0.118f, 0.161f, 0.231f),
            ButtonSecondaryFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonGhostFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonGhostHoverBg = new Color(0.059f, 0.071f, 0.165f, 0.1f),
            ButtonGhostHoverFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonLinkColor = new Color(0.378f, 0.631f, 0.969f),
            ButtonLinkHoverColor = new Color(0.478f, 0.731f, 1.0f),

            InputBg = new Color(0.059f, 0.071f, 0.165f, 0.3f),
            InputFg = new Color(0.980f, 0.988f, 0.996f),
            InputFocusedBg = new Color(0.059f, 0.071f, 0.165f, 0.5f),
            InputFocusedFg = new Color(0.980f, 0.988f, 0.996f),
            InputDisabledBg = new Color(0.118f, 0.161f, 0.231f, 0.3f),
            InputDisabledFg = new Color(0.639f, 0.651f, 0.667f),

            CardBg = new Color(0.059f, 0.071f, 0.165f),
            CardFg = new Color(0.980f, 0.988f, 0.996f),
            CardTitle = new Color(0.980f, 0.988f, 0.996f),
            CardDescription = new Color(0.639f, 0.651f, 0.667f),

            ToggleBg = new Color(0.118f, 0.161f, 0.231f),
            ToggleFg = new Color(0.980f, 0.988f, 0.996f),
            ToggleHoverBg = new Color(0.157f, 0.196f, 0.267f),
            ToggleHoverFg = new Color(0.980f, 0.988f, 0.996f),
            ToggleOnBg = new Color(0.378f, 0.631f, 0.969f),
            ToggleOnFg = new Color(0.980f, 0.988f, 0.996f),

            SeparatorColor = new Color(0.118f, 0.161f, 0.231f),

            TabsBg = new Color(0.059f, 0.071f, 0.165f, 0.5f),
            TabsTriggerFg = new Color(0.639f, 0.651f, 0.667f),
            TabsTriggerActiveBg = new Color(0.012f, 0.027f, 0.071f),
            TabsTriggerActiveFg = new Color(0.980f, 0.988f, 0.996f),

            CheckboxBg = new Color(0.059f, 0.071f, 0.165f),
            CheckboxCheckedBg = new Color(0.378f, 0.631f, 0.969f),

            SwitchBg = new Color(0.118f, 0.161f, 0.231f),
            SwitchOnBg = new Color(0.378f, 0.631f, 0.969f),
            SwitchOffBg = new Color(0.118f, 0.161f, 0.231f),

            BadgeBg = new Color(0.059f, 0.071f, 0.165f),
            BadgeSecondaryBg = new Color(0.118f, 0.161f, 0.231f),
            BadgeDestructiveBg = new Color(0.937f, 0.266f, 0.266f),

            AvatarBg = new Color(0.118f, 0.161f, 0.231f),
            AvatarFallbackBg = new Color(0.118f, 0.161f, 0.231f),
            AvatarFallbackFg = new Color(0.980f, 0.988f, 0.996f),

            TableBg = new Color(0.012f, 0.027f, 0.071f),
            TableHeaderBg = new Color(0.118f, 0.161f, 0.231f),
            TableCellBg = new Color(0.012f, 0.027f, 0.071f),

            ChartBg = new Color(0.059f, 0.071f, 0.165f),
            ChartGridColor = new Color(0.118f, 0.161f, 0.231f, 0.2f),
            ChartAxisColor = new Color(0.639f, 0.651f, 0.667f, 0.6f),
        };

        public static Theme Light = new Theme
        {
            Name = "Light",
            PrimaryColor = new Color(1.0f, 1.0f, 1.0f),
            SecondaryColor = new Color(0.945f, 0.961f, 0.976f),
            AccentColor = new Color(0.231f, 0.510f, 0.965f),
            BackgroundColor = new Color(1.0f, 1.0f, 1.0f),
            TextColor = new Color(0.020f, 0.024f, 0.031f),
            MutedColor = new Color(0.396f, 0.447f, 0.525f),
            BorderColor = new Color(0.886f, 0.898f, 0.918f),
            OverlayColor = new Color(0f, 0f, 0f, 0.20f),
            ShadowColor = new Color(0f, 0f, 0f, 0.2f),

            SuccessBg = new Color(0.827f, 0.973f, 0.875f),
            SuccessFg = new Color(0.110f, 0.388f, 0.208f),
            WarningBg = new Color(0.996f, 0.949f, 0.847f),
            WarningFg = new Color(0.486f, 0.337f, 0.059f),
            InfoBg = new Color(0.882f, 0.937f, 0.996f),
            InfoFg = new Color(0.129f, 0.310f, 0.733f),

            ButtonPrimaryBg = new Color(0.020f, 0.024f, 0.031f),
            ButtonPrimaryFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonPrimaryActiveBg = new Color(0.031f, 0.043f, 0.067f),
            ButtonPrimaryActiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonDestructiveBg = new Color(0.937f, 0.266f, 0.266f),
            ButtonDestructiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineBorder = new Color(0.886f, 0.898f, 0.918f),
            ButtonOutlineBg = new Color(1.0f, 1.0f, 1.0f, 0.0f),
            ButtonOutlineFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonOutlineHoverBg = new Color(0.945f, 0.961f, 0.976f, 0.8f),
            ButtonOutlineHoverFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonSecondaryBg = new Color(0.945f, 0.961f, 0.976f),
            ButtonSecondaryFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonGhostFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonGhostHoverBg = new Color(0.945f, 0.961f, 0.976f, 0.8f),
            ButtonGhostHoverFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonLinkColor = new Color(0.231f, 0.510f, 0.965f),
            ButtonLinkHoverColor = new Color(0.331f, 0.610f, 1.0f),

            InputBg = new Color(1.0f, 1.0f, 1.0f),
            InputFg = new Color(0.020f, 0.024f, 0.031f),
            InputFocusedBg = new Color(1.0f, 1.0f, 1.0f),
            InputFocusedFg = new Color(0.020f, 0.024f, 0.031f),
            InputDisabledBg = new Color(0.945f, 0.961f, 0.976f, 0.5f),
            InputDisabledFg = new Color(0.396f, 0.447f, 0.525f),

            CardBg = new Color(1.0f, 1.0f, 1.0f),
            CardFg = new Color(0.020f, 0.024f, 0.031f),
            CardTitle = new Color(0.020f, 0.024f, 0.031f),
            CardDescription = new Color(0.396f, 0.447f, 0.525f),

            ToggleBg = new Color(0.886f, 0.898f, 0.918f),
            ToggleFg = new Color(0.020f, 0.024f, 0.031f),
            ToggleHoverBg = new Color(0.827f, 0.847f, 0.878f),
            ToggleHoverFg = new Color(0.020f, 0.024f, 0.031f),
            ToggleOnBg = new Color(0.231f, 0.510f, 0.965f),
            ToggleOnFg = new Color(0.980f, 0.988f, 0.996f),

            SeparatorColor = new Color(0.886f, 0.898f, 0.918f),

            TabsBg = new Color(0.945f, 0.961f, 0.976f),
            TabsTriggerFg = new Color(0.396f, 0.447f, 0.525f),
            TabsTriggerActiveBg = new Color(1.0f, 1.0f, 1.0f),
            TabsTriggerActiveFg = new Color(0.020f, 0.024f, 0.031f),

            CheckboxBg = new Color(1.0f, 1.0f, 1.0f),
            CheckboxCheckedBg = new Color(0.231f, 0.510f, 0.965f),

            SwitchBg = new Color(0.886f, 0.898f, 0.918f),
            SwitchOnBg = new Color(0.231f, 0.510f, 0.965f),
            SwitchOffBg = new Color(0.886f, 0.898f, 0.918f),

            BadgeBg = new Color(0.020f, 0.024f, 0.031f),
            BadgeSecondaryBg = new Color(0.945f, 0.961f, 0.976f),
            BadgeDestructiveBg = new Color(0.937f, 0.266f, 0.266f),

            AvatarBg = new Color(0.945f, 0.961f, 0.976f),
            AvatarFallbackBg = new Color(0.945f, 0.961f, 0.976f),
            AvatarFallbackFg = new Color(0.020f, 0.024f, 0.031f),

            TableBg = new Color(1.0f, 1.0f, 1.0f),
            TableHeaderBg = new Color(0.945f, 0.961f, 0.976f),
            TableCellBg = new Color(1.0f, 1.0f, 1.0f),

            ChartBg = new Color(1.0f, 1.0f, 1.0f),
            ChartGridColor = new Color(0.886f, 0.898f, 0.918f, 0.4f),
            ChartAxisColor = new Color(0.396f, 0.447f, 0.525f, 0.7f),
        };

        public static Theme Slate = new Theme
        {
            Name = "Slate",
            PrimaryColor = new Color(0.071f, 0.082f, 0.094f),
            SecondaryColor = new Color(0.114f, 0.129f, 0.149f),
            AccentColor = new Color(0.345f, 0.514f, 0.961f),
            BackgroundColor = new Color(0.055f, 0.063f, 0.075f),
            TextColor = new Color(0.957f, 0.965f, 0.973f),
            MutedColor = new Color(0.608f, 0.627f, 0.651f),
            BorderColor = new Color(0.169f, 0.192f, 0.224f),
            OverlayColor = new Color(0f, 0f, 0f, 0.40f),
            ShadowColor = new Color(0f, 0f, 0f, 0.55f),

            ButtonPrimaryBg = new Color(0.114f, 0.129f, 0.149f),
            ButtonPrimaryFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonPrimaryActiveBg = new Color(0.145f, 0.165f, 0.188f),
            ButtonPrimaryActiveFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonDestructiveBg = new Color(0.902f, 0.275f, 0.298f),
            ButtonDestructiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineBorder = new Color(0.169f, 0.192f, 0.224f),
            ButtonOutlineBg = new Color(0f, 0f, 0f, 0f),
            ButtonOutlineFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonOutlineHoverBg = new Color(0.114f, 0.129f, 0.149f, 0.12f),
            ButtonOutlineHoverFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonSecondaryBg = new Color(0.114f, 0.129f, 0.149f),
            ButtonSecondaryFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonGhostFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonGhostHoverBg = new Color(0.114f, 0.129f, 0.149f, 0.12f),
            ButtonGhostHoverFg = new Color(0.957f, 0.965f, 0.973f),
            ButtonLinkColor = new Color(0.345f, 0.514f, 0.961f),
            ButtonLinkHoverColor = new Color(0.454f, 0.631f, 1.0f),

            InputBg = new Color(0.090f, 0.102f, 0.118f),
            InputFg = new Color(0.957f, 0.965f, 0.973f),
            InputFocusedBg = new Color(0.110f, 0.125f, 0.141f),
            InputFocusedFg = new Color(0.957f, 0.965f, 0.973f),
            InputDisabledBg = new Color(0.114f, 0.129f, 0.149f, 0.35f),
            InputDisabledFg = new Color(0.608f, 0.627f, 0.651f),

            CardBg = new Color(0.090f, 0.102f, 0.118f),
            CardFg = new Color(0.957f, 0.965f, 0.973f),
            CardTitle = new Color(0.957f, 0.965f, 0.973f),
            CardDescription = new Color(0.608f, 0.627f, 0.651f),

            ToggleBg = new Color(0.114f, 0.129f, 0.149f),
            ToggleFg = new Color(0.957f, 0.965f, 0.973f),
            ToggleHoverBg = new Color(0.141f, 0.161f, 0.184f),
            ToggleHoverFg = new Color(0.957f, 0.965f, 0.973f),
            ToggleOnBg = new Color(0.345f, 0.514f, 0.961f),
            ToggleOnFg = new Color(0.980f, 0.988f, 0.996f),

            SeparatorColor = new Color(0.169f, 0.192f, 0.224f),

            TabsBg = new Color(0.090f, 0.102f, 0.118f, 0.5f),
            TabsTriggerFg = new Color(0.608f, 0.627f, 0.651f),
            TabsTriggerActiveBg = new Color(0.071f, 0.082f, 0.094f),
            TabsTriggerActiveFg = new Color(0.957f, 0.965f, 0.973f),

            CheckboxBg = new Color(0.090f, 0.102f, 0.118f),
            CheckboxCheckedBg = new Color(0.345f, 0.514f, 0.961f),

            SwitchBg = new Color(0.114f, 0.129f, 0.149f),
            SwitchOnBg = new Color(0.345f, 0.514f, 0.961f),
            SwitchOffBg = new Color(0.114f, 0.129f, 0.149f),

            BadgeBg = new Color(0.090f, 0.102f, 0.118f),
            BadgeSecondaryBg = new Color(0.114f, 0.129f, 0.149f),
            BadgeDestructiveBg = new Color(0.902f, 0.275f, 0.298f),

            AvatarBg = new Color(0.114f, 0.129f, 0.149f),
            AvatarFallbackBg = new Color(0.114f, 0.129f, 0.149f),
            AvatarFallbackFg = new Color(0.957f, 0.965f, 0.973f),

            TableBg = new Color(0.055f, 0.063f, 0.075f),
            TableHeaderBg = new Color(0.114f, 0.129f, 0.149f),
            TableCellBg = new Color(0.055f, 0.063f, 0.075f),

            ChartBg = new Color(0.090f, 0.102f, 0.118f),
            ChartGridColor = new Color(0.169f, 0.192f, 0.224f, 0.25f),
            ChartAxisColor = new Color(0.608f, 0.627f, 0.651f, 0.65f),
        };

        public static Theme Ocean = new Theme
        {
            Name = "Ocean",
            PrimaryColor = new Color(0.008f, 0.043f, 0.086f),
            SecondaryColor = new Color(0.035f, 0.137f, 0.224f),
            AccentColor = new Color(0.267f, 0.647f, 0.961f),
            BackgroundColor = new Color(0.008f, 0.043f, 0.086f),
            TextColor = new Color(0.922f, 0.961f, 1.0f),
            MutedColor = new Color(0.569f, 0.651f, 0.725f),
            BorderColor = new Color(0.035f, 0.137f, 0.224f),
            OverlayColor = new Color(0f, 0.2f, 0.4f, 0.35f),
            ShadowColor = new Color(0f, 0.1f, 0.2f, 0.5f),

            SuccessBg = new Color(0.149f, 0.620f, 0.420f),
            SuccessFg = new Color(0.922f, 0.961f, 1.0f),
            WarningBg = new Color(0.957f, 0.749f, 0.227f),
            WarningFg = new Color(0.035f, 0.043f, 0.055f),
            InfoBg = new Color(0.267f, 0.647f, 0.961f),
            InfoFg = new Color(0.922f, 0.961f, 1.0f),

            ButtonPrimaryBg = new Color(0.035f, 0.137f, 0.224f),
            ButtonPrimaryFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonPrimaryActiveBg = new Color(0.055f, 0.173f, 0.275f),
            ButtonPrimaryActiveFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonDestructiveBg = new Color(0.937f, 0.329f, 0.318f),
            ButtonDestructiveFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonOutlineBorder = new Color(0.035f, 0.137f, 0.224f),
            ButtonOutlineBg = new Color(0.008f, 0.043f, 0.086f, 0.0f),
            ButtonOutlineFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonOutlineHoverBg = new Color(0.035f, 0.137f, 0.224f, 0.15f),
            ButtonOutlineHoverFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonSecondaryBg = new Color(0.055f, 0.173f, 0.275f),
            ButtonSecondaryFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonGhostFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonGhostHoverBg = new Color(0.035f, 0.137f, 0.224f, 0.15f),
            ButtonGhostHoverFg = new Color(0.922f, 0.961f, 1.0f),
            ButtonLinkColor = new Color(0.267f, 0.647f, 0.961f),
            ButtonLinkHoverColor = new Color(0.380f, 0.725f, 1.0f),

            InputBg = new Color(0.035f, 0.137f, 0.224f, 0.3f),
            InputFg = new Color(0.922f, 0.961f, 1.0f),
            InputFocusedBg = new Color(0.035f, 0.137f, 0.224f, 0.5f),
            InputFocusedFg = new Color(0.922f, 0.961f, 1.0f),
            InputDisabledBg = new Color(0.055f, 0.173f, 0.275f, 0.3f),
            InputDisabledFg = new Color(0.569f, 0.651f, 0.725f),

            CardBg = new Color(0.035f, 0.137f, 0.224f),
            CardFg = new Color(0.922f, 0.961f, 1.0f),
            CardTitle = new Color(0.922f, 0.961f, 1.0f),
            CardDescription = new Color(0.569f, 0.651f, 0.725f),

            ToggleBg = new Color(0.055f, 0.173f, 0.275f),
            ToggleFg = new Color(0.922f, 0.961f, 1.0f),
            ToggleHoverBg = new Color(0.075f, 0.208f, 0.318f),
            ToggleHoverFg = new Color(0.922f, 0.961f, 1.0f),
            ToggleOnBg = new Color(0.267f, 0.647f, 0.961f),
            ToggleOnFg = new Color(0.922f, 0.961f, 1.0f),

            SeparatorColor = new Color(0.035f, 0.137f, 0.224f),

            TabsBg = new Color(0.035f, 0.137f, 0.224f, 0.5f),
            TabsTriggerFg = new Color(0.569f, 0.651f, 0.725f),
            TabsTriggerActiveBg = new Color(0.008f, 0.043f, 0.086f),
            TabsTriggerActiveFg = new Color(0.922f, 0.961f, 1.0f),

            CheckboxBg = new Color(0.035f, 0.137f, 0.224f),
            CheckboxCheckedBg = new Color(0.267f, 0.647f, 0.961f),

            SwitchBg = new Color(0.055f, 0.173f, 0.275f),
            SwitchOnBg = new Color(0.267f, 0.647f, 0.961f),
            SwitchOffBg = new Color(0.055f, 0.173f, 0.275f),

            BadgeBg = new Color(0.035f, 0.137f, 0.224f),
            BadgeSecondaryBg = new Color(0.055f, 0.173f, 0.275f),
            BadgeDestructiveBg = new Color(0.937f, 0.329f, 0.318f),

            AvatarBg = new Color(0.055f, 0.173f, 0.275f),
            AvatarFallbackBg = new Color(0.055f, 0.173f, 0.275f),
            AvatarFallbackFg = new Color(0.922f, 0.961f, 1.0f),

            TableBg = new Color(0.008f, 0.043f, 0.086f),
            TableHeaderBg = new Color(0.035f, 0.137f, 0.224f),
            TableCellBg = new Color(0.008f, 0.043f, 0.086f),

            ChartBg = new Color(0.035f, 0.137f, 0.224f),
            ChartGridColor = new Color(0.055f, 0.173f, 0.275f, 0.2f),
            ChartAxisColor = new Color(0.569f, 0.651f, 0.725f, 0.6f),
        };

        public static Theme Forest = new Theme
        {
            Name = "Forest",
            PrimaryColor = new Color(0.051f, 0.071f, 0.043f),
            SecondaryColor = new Color(0.118f, 0.180f, 0.098f),
            AccentColor = new Color(0.408f, 0.796f, 0.482f),
            BackgroundColor = new Color(0.051f, 0.071f, 0.043f),
            TextColor = new Color(0.941f, 0.965f, 0.922f),
            MutedColor = new Color(0.584f, 0.647f, 0.573f),
            BorderColor = new Color(0.118f, 0.180f, 0.098f),
            OverlayColor = new Color(0f, 0.2f, 0f, 0.35f),
            ShadowColor = new Color(0f, 0.1f, 0f, 0.5f),

            SuccessBg = new Color(0.310f, 0.682f, 0.371f),
            SuccessFg = new Color(0.941f, 0.965f, 0.922f),
            WarningBg = new Color(0.945f, 0.686f, 0.196f),
            WarningFg = new Color(0.051f, 0.043f, 0.020f),
            InfoBg = new Color(0.249f, 0.565f, 0.859f),
            InfoFg = new Color(0.941f, 0.965f, 0.922f),

            ButtonPrimaryBg = new Color(0.118f, 0.180f, 0.098f),
            ButtonPrimaryFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonPrimaryActiveBg = new Color(0.157f, 0.224f, 0.125f),
            ButtonPrimaryActiveFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonDestructiveBg = new Color(0.902f, 0.310f, 0.290f),
            ButtonDestructiveFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonOutlineBorder = new Color(0.118f, 0.180f, 0.098f),
            ButtonOutlineBg = new Color(0.051f, 0.071f, 0.043f, 0.0f),
            ButtonOutlineFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonOutlineHoverBg = new Color(0.118f, 0.180f, 0.098f, 0.15f),
            ButtonOutlineHoverFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonSecondaryBg = new Color(0.157f, 0.224f, 0.125f),
            ButtonSecondaryFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonGhostFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonGhostHoverBg = new Color(0.118f, 0.180f, 0.098f, 0.15f),
            ButtonGhostHoverFg = new Color(0.941f, 0.965f, 0.922f),
            ButtonLinkColor = new Color(0.408f, 0.796f, 0.482f),
            ButtonLinkHoverColor = new Color(0.525f, 0.859f, 0.596f),

            InputBg = new Color(0.118f, 0.180f, 0.098f, 0.3f),
            InputFg = new Color(0.941f, 0.965f, 0.922f),
            InputFocusedBg = new Color(0.118f, 0.180f, 0.098f, 0.5f),
            InputFocusedFg = new Color(0.941f, 0.965f, 0.922f),
            InputDisabledBg = new Color(0.157f, 0.224f, 0.125f, 0.3f),
            InputDisabledFg = new Color(0.584f, 0.647f, 0.573f),

            CardBg = new Color(0.118f, 0.180f, 0.098f),
            CardFg = new Color(0.941f, 0.965f, 0.922f),
            CardTitle = new Color(0.941f, 0.965f, 0.922f),
            CardDescription = new Color(0.584f, 0.647f, 0.573f),

            ToggleBg = new Color(0.157f, 0.224f, 0.125f),
            ToggleFg = new Color(0.941f, 0.965f, 0.922f),
            ToggleHoverBg = new Color(0.180f, 0.251f, 0.149f),
            ToggleHoverFg = new Color(0.941f, 0.965f, 0.922f),
            ToggleOnBg = new Color(0.408f, 0.796f, 0.482f),
            ToggleOnFg = new Color(0.941f, 0.965f, 0.922f),

            SeparatorColor = new Color(0.118f, 0.180f, 0.098f),

            TabsBg = new Color(0.118f, 0.180f, 0.098f, 0.5f),
            TabsTriggerFg = new Color(0.584f, 0.647f, 0.573f),
            TabsTriggerActiveBg = new Color(0.051f, 0.071f, 0.043f),
            TabsTriggerActiveFg = new Color(0.941f, 0.965f, 0.922f),

            CheckboxBg = new Color(0.118f, 0.180f, 0.098f),
            CheckboxCheckedBg = new Color(0.408f, 0.796f, 0.482f),

            SwitchBg = new Color(0.157f, 0.224f, 0.125f),
            SwitchOnBg = new Color(0.408f, 0.796f, 0.482f),
            SwitchOffBg = new Color(0.157f, 0.224f, 0.125f),

            BadgeBg = new Color(0.118f, 0.180f, 0.098f),
            BadgeSecondaryBg = new Color(0.157f, 0.224f, 0.125f),
            BadgeDestructiveBg = new Color(0.902f, 0.310f, 0.290f),

            AvatarBg = new Color(0.157f, 0.224f, 0.125f),
            AvatarFallbackBg = new Color(0.157f, 0.224f, 0.125f),
            AvatarFallbackFg = new Color(0.941f, 0.965f, 0.922f),

            TableBg = new Color(0.051f, 0.071f, 0.043f),
            TableHeaderBg = new Color(0.118f, 0.180f, 0.098f),
            TableCellBg = new Color(0.051f, 0.071f, 0.043f),

            ChartBg = new Color(0.118f, 0.180f, 0.098f),
            ChartGridColor = new Color(0.157f, 0.224f, 0.125f, 0.2f),
            ChartAxisColor = new Color(0.584f, 0.647f, 0.573f, 0.6f),
        };

        public static Theme Sunset = new Theme
        {
            Name = "Sunset",
            PrimaryColor = new Color(0.102f, 0.063f, 0.047f),
            SecondaryColor = new Color(0.243f, 0.125f, 0.086f),
            AccentColor = new Color(0.973f, 0.631f, 0.380f),
            BackgroundColor = new Color(0.102f, 0.063f, 0.047f),
            TextColor = new Color(0.996f, 0.922f, 0.878f),
            MutedColor = new Color(0.729f, 0.596f, 0.522f),
            BorderColor = new Color(0.243f, 0.125f, 0.086f),
            OverlayColor = new Color(0.3f, 0.1f, 0f, 0.35f),
            ShadowColor = new Color(0.2f, 0.05f, 0f, 0.5f),

            SuccessBg = new Color(0.337f, 0.698f, 0.380f),
            SuccessFg = new Color(0.996f, 0.922f, 0.878f),
            WarningBg = new Color(0.996f, 0.714f, 0.251f),
            WarningFg = new Color(0.063f, 0.043f, 0.020f),
            InfoBg = new Color(0.294f, 0.596f, 0.945f),
            InfoFg = new Color(0.996f, 0.922f, 0.878f),

            ButtonPrimaryBg = new Color(0.243f, 0.125f, 0.086f),
            ButtonPrimaryFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonPrimaryActiveBg = new Color(0.302f, 0.157f, 0.110f),
            ButtonPrimaryActiveFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonDestructiveBg = new Color(0.933f, 0.255f, 0.255f),
            ButtonDestructiveFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonOutlineBorder = new Color(0.243f, 0.125f, 0.086f),
            ButtonOutlineBg = new Color(0.102f, 0.063f, 0.047f, 0.0f),
            ButtonOutlineFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonOutlineHoverBg = new Color(0.243f, 0.125f, 0.086f, 0.15f),
            ButtonOutlineHoverFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonSecondaryBg = new Color(0.302f, 0.157f, 0.110f),
            ButtonSecondaryFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonGhostFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonGhostHoverBg = new Color(0.243f, 0.125f, 0.086f, 0.15f),
            ButtonGhostHoverFg = new Color(0.996f, 0.922f, 0.878f),
            ButtonLinkColor = new Color(0.973f, 0.631f, 0.380f),
            ButtonLinkHoverColor = new Color(1.0f, 0.729f, 0.498f),

            InputBg = new Color(0.243f, 0.125f, 0.086f, 0.3f),
            InputFg = new Color(0.996f, 0.922f, 0.878f),
            InputFocusedBg = new Color(0.243f, 0.125f, 0.086f, 0.5f),
            InputFocusedFg = new Color(0.996f, 0.922f, 0.878f),
            InputDisabledBg = new Color(0.302f, 0.157f, 0.110f, 0.3f),
            InputDisabledFg = new Color(0.729f, 0.596f, 0.522f),

            CardBg = new Color(0.243f, 0.125f, 0.086f),
            CardFg = new Color(0.996f, 0.922f, 0.878f),
            CardTitle = new Color(0.996f, 0.922f, 0.878f),
            CardDescription = new Color(0.729f, 0.596f, 0.522f),

            ToggleBg = new Color(0.302f, 0.157f, 0.110f),
            ToggleFg = new Color(0.996f, 0.922f, 0.878f),
            ToggleHoverBg = new Color(0.341f, 0.180f, 0.133f),
            ToggleHoverFg = new Color(0.996f, 0.922f, 0.878f),
            ToggleOnBg = new Color(0.973f, 0.631f, 0.380f),
            ToggleOnFg = new Color(0.996f, 0.922f, 0.878f),

            SeparatorColor = new Color(0.243f, 0.125f, 0.086f),

            TabsBg = new Color(0.243f, 0.125f, 0.086f, 0.5f),
            TabsTriggerFg = new Color(0.729f, 0.596f, 0.522f),
            TabsTriggerActiveBg = new Color(0.102f, 0.063f, 0.047f),
            TabsTriggerActiveFg = new Color(0.996f, 0.922f, 0.878f),

            CheckboxBg = new Color(0.243f, 0.125f, 0.086f),
            CheckboxCheckedBg = new Color(0.973f, 0.631f, 0.380f),

            SwitchBg = new Color(0.302f, 0.157f, 0.110f),
            SwitchOnBg = new Color(0.973f, 0.631f, 0.380f),
            SwitchOffBg = new Color(0.302f, 0.157f, 0.110f),

            BadgeBg = new Color(0.243f, 0.125f, 0.086f),
            BadgeSecondaryBg = new Color(0.302f, 0.157f, 0.110f),
            BadgeDestructiveBg = new Color(0.933f, 0.255f, 0.255f),

            AvatarBg = new Color(0.302f, 0.157f, 0.110f),
            AvatarFallbackBg = new Color(0.302f, 0.157f, 0.110f),
            AvatarFallbackFg = new Color(0.996f, 0.922f, 0.878f),

            TableBg = new Color(0.102f, 0.063f, 0.047f),
            TableHeaderBg = new Color(0.243f, 0.125f, 0.086f),
            TableCellBg = new Color(0.102f, 0.063f, 0.047f),

            ChartBg = new Color(0.243f, 0.125f, 0.086f),
            ChartGridColor = new Color(0.302f, 0.157f, 0.110f, 0.2f),
            ChartAxisColor = new Color(0.729f, 0.596f, 0.522f, 0.6f),
        };

        public static Theme Cyberpunk = new Theme
        {
            Name = "Cyberpunk",
            PrimaryColor = new Color(0.016f, 0.016f, 0.035f),
            SecondaryColor = new Color(0.035f, 0.043f, 0.082f),
            AccentColor = new Color(0.980f, 0.149f, 0.839f),
            BackgroundColor = new Color(0.016f, 0.016f, 0.035f),
            TextColor = new Color(0.996f, 1.0f, 0.980f),
            MutedColor = new Color(0.729f, 0.749f, 0.792f),
            BorderColor = new Color(0.080f, 0.839f, 0.980f),
            OverlayColor = new Color(0.980f, 0.149f, 0.839f, 0.25f),
            ShadowColor = new Color(0f, 0.2f, 0.3f, 0.5f),

            SuccessBg = new Color(0.149f, 0.980f, 0.467f),
            SuccessFg = new Color(0.016f, 0.016f, 0.035f),
            WarningBg = new Color(0.980f, 0.757f, 0.149f),
            WarningFg = new Color(0.016f, 0.016f, 0.035f),
            InfoBg = new Color(0.080f, 0.839f, 0.980f),
            InfoFg = new Color(0.016f, 0.016f, 0.035f),

            ButtonPrimaryBg = new Color(0.980f, 0.149f, 0.839f),
            ButtonPrimaryFg = new Color(0.016f, 0.016f, 0.035f),
            ButtonPrimaryActiveBg = new Color(1.0f, 0.275f, 0.914f),
            ButtonPrimaryActiveFg = new Color(0.016f, 0.016f, 0.035f),
            ButtonDestructiveBg = new Color(0.980f, 0.149f, 0.149f),
            ButtonDestructiveFg = new Color(0.996f, 1.0f, 0.980f),
            ButtonOutlineBorder = new Color(0.080f, 0.839f, 0.980f),
            ButtonOutlineBg = new Color(0.016f, 0.016f, 0.035f, 0.0f),
            ButtonOutlineFg = new Color(0.080f, 0.839f, 0.980f),
            ButtonOutlineHoverBg = new Color(0.080f, 0.839f, 0.980f, 0.2f),
            ButtonOutlineHoverFg = new Color(0.996f, 1.0f, 0.980f),
            ButtonSecondaryBg = new Color(0.035f, 0.043f, 0.082f),
            ButtonSecondaryFg = new Color(0.080f, 0.839f, 0.980f),
            ButtonGhostFg = new Color(0.080f, 0.839f, 0.980f),
            ButtonGhostHoverBg = new Color(0.080f, 0.839f, 0.980f, 0.15f),
            ButtonGhostHoverFg = new Color(0.996f, 1.0f, 0.980f),
            ButtonLinkColor = new Color(0.080f, 0.839f, 0.980f),
            ButtonLinkHoverColor = new Color(0.149f, 0.937f, 1.0f),

            InputBg = new Color(0.035f, 0.043f, 0.082f, 0.5f),
            InputFg = new Color(0.080f, 0.839f, 0.980f),
            InputFocusedBg = new Color(0.035f, 0.043f, 0.082f, 0.8f),
            InputFocusedFg = new Color(0.980f, 0.149f, 0.839f),
            InputDisabledBg = new Color(0.035f, 0.043f, 0.082f, 0.3f),
            InputDisabledFg = new Color(0.729f, 0.749f, 0.792f),

            CardBg = new Color(0.035f, 0.043f, 0.082f),
            CardFg = new Color(0.080f, 0.839f, 0.980f),
            CardTitle = new Color(0.980f, 0.149f, 0.839f),
            CardDescription = new Color(0.729f, 0.749f, 0.792f),

            ToggleBg = new Color(0.035f, 0.043f, 0.082f),
            ToggleFg = new Color(0.080f, 0.839f, 0.980f),
            ToggleHoverBg = new Color(0.055f, 0.063f, 0.106f),
            ToggleHoverFg = new Color(0.149f, 0.937f, 1.0f),
            ToggleOnBg = new Color(0.980f, 0.149f, 0.839f),
            ToggleOnFg = new Color(0.016f, 0.016f, 0.035f),

            SeparatorColor = new Color(0.080f, 0.839f, 0.980f, 0.3f),

            TabsBg = new Color(0.035f, 0.043f, 0.082f),
            TabsTriggerFg = new Color(0.729f, 0.749f, 0.792f),
            TabsTriggerActiveBg = new Color(0.016f, 0.016f, 0.035f),
            TabsTriggerActiveFg = new Color(0.980f, 0.149f, 0.839f),

            CheckboxBg = new Color(0.035f, 0.043f, 0.082f),
            CheckboxCheckedBg = new Color(0.980f, 0.149f, 0.839f),

            SwitchBg = new Color(0.035f, 0.043f, 0.082f),
            SwitchOnBg = new Color(0.080f, 0.839f, 0.980f),
            SwitchOffBg = new Color(0.035f, 0.043f, 0.082f),

            BadgeBg = new Color(0.980f, 0.149f, 0.839f),
            BadgeSecondaryBg = new Color(0.080f, 0.839f, 0.980f),
            BadgeDestructiveBg = new Color(0.980f, 0.149f, 0.149f),

            AvatarBg = new Color(0.035f, 0.043f, 0.082f),
            AvatarFallbackBg = new Color(0.035f, 0.043f, 0.082f),
            AvatarFallbackFg = new Color(0.980f, 0.149f, 0.839f),

            TableBg = new Color(0.016f, 0.016f, 0.035f),
            TableHeaderBg = new Color(0.035f, 0.043f, 0.082f),
            TableCellBg = new Color(0.016f, 0.016f, 0.035f),

            ChartBg = new Color(0.035f, 0.043f, 0.082f),
            ChartGridColor = new Color(0.080f, 0.839f, 0.980f, 0.15f),
            ChartAxisColor = new Color(0.980f, 0.149f, 0.839f, 0.5f),
        };
    }
}
