using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class Theme
    {
        public string Name { get; set; }

        // General
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public Color AccentColor { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Color MutedColor { get; set; }

        // Button
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

        // Input
        public Color InputBg { get; set; }
        public Color InputFg { get; set; }
        public Color InputFocusedBg { get; set; }
        public Color InputFocusedFg { get; set; }
        public Color InputDisabledBg { get; set; }
        public Color InputDisabledFg { get; set; }

        // Card
        public Color CardBg { get; set; }
        public Color CardFg { get; set; }
        public Color CardTitle { get; set; }
        public Color CardDescription { get; set; }

        // Toggle
        public Color ToggleBg { get; set; }
        public Color ToggleFg { get; set; }
        public Color ToggleHoverBg { get; set; }
        public Color ToggleHoverFg { get; set; }
        public Color ToggleOnBg { get; set; }
        public Color ToggleOnFg { get; set; }

        // Separator
        public Color SeparatorColor { get; set; }

        // Tabs
        public Color TabsBg { get; set; }
        public Color TabsTriggerFg { get; set; }
        public Color TabsTriggerActiveBg { get; set; }
        public Color TabsTriggerActiveFg { get; set; }

        // Checkbox
        public Color CheckboxBg { get; set; }
        public Color CheckboxCheckedBg { get; set; }

        // Switch
        public Color SwitchBg { get; set; }
        public Color SwitchOnBg { get; set; }
        public Color SwitchOffBg { get; set; }

        // Badge
        public Color BadgeBg { get; set; }
        public Color BadgeSecondaryBg { get; set; }
        public Color BadgeDestructiveBg { get; set; }

        // Alert
        public Color AlertDefaultBg { get; set; }
        public Color AlertDefaultFg { get; set; }
        public Color AlertDestructiveBg { get; set; }
        public Color AlertDestructiveFg { get; set; }

        // Avatar
        public Color AvatarBg { get; set; }
        public Color AvatarFallbackBg { get; set; }
        public Color AvatarFallbackFg { get; set; }

        // Skeleton
        public Color SkeletonBg { get; set; }

        // Table
        public Color TableBg { get; set; }
        public Color TableHeaderBg { get; set; }
        public Color TableCellBg { get; set; }

        public static Theme Dark = new Theme
        {
            Name = "Dark",

            PrimaryColor = new Color(0.012f, 0.027f, 0.071f), // hsl(222.2 84% 4.9%)
            SecondaryColor = new Color(0.118f, 0.161f, 0.231f), // hsl(217.2 32.6% 17.5%)
            AccentColor = new Color(0.378f, 0.631f, 0.969f), // hsl(221.2 83.2% 53.3%)
            BackgroundColor = new Color(0.012f, 0.027f, 0.071f), // hsl(222.2 84% 4.9%)
            TextColor = new Color(0.980f, 0.988f, 0.996f), // hsl(210 40% 98%)
            MutedColor = new Color(0.639f, 0.651f, 0.667f), // hsl(215.4 16.3% 46.9%)

            ButtonPrimaryBg = new Color(0.059f, 0.071f, 0.165f), // hsl(222.2 47.4% 11.2%)
            ButtonPrimaryFg = new Color(0.980f, 0.988f, 0.996f), // hsl(210 40% 98%)
            ButtonPrimaryActiveBg = new Color(0.071f, 0.086f, 0.196f),
            ButtonPrimaryActiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonDestructiveBg = new Color(0.937f, 0.266f, 0.266f), // hsl(0 84.2% 60.2%)
            ButtonDestructiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineBorder = new Color(0.118f, 0.161f, 0.231f), // hsl(217.2 32.6% 17.5%)
            ButtonOutlineBg = new Color(0.012f, 0.027f, 0.071f, 0.0f),
            ButtonOutlineFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineHoverBg = new Color(0.059f, 0.071f, 0.165f, 0.1f), // hsl(222.2 47.4% 11.2%)
            ButtonOutlineHoverFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonSecondaryBg = new Color(0.118f, 0.161f, 0.231f), // hsl(217.2 32.6% 17.5%)
            ButtonSecondaryFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonGhostFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonGhostHoverBg = new Color(0.059f, 0.071f, 0.165f, 0.1f), // hsl(222.2 47.4% 11.2%)
            ButtonGhostHoverFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonLinkColor = new Color(0.378f, 0.631f, 0.969f), // hsl(221.2 83.2% 53.3%)
            ButtonLinkHoverColor = new Color(0.478f, 0.731f, 1.0f),

            // Input colors
            InputBg = new Color(0.059f, 0.071f, 0.165f, 0.3f), // hsl(222.2 47.4% 11.2%)
            InputFg = new Color(0.980f, 0.988f, 0.996f),
            InputFocusedBg = new Color(0.059f, 0.071f, 0.165f, 0.5f),
            InputFocusedFg = new Color(0.980f, 0.988f, 0.996f),
            InputDisabledBg = new Color(0.118f, 0.161f, 0.231f, 0.3f),
            InputDisabledFg = new Color(0.639f, 0.651f, 0.667f),

            // Card colors
            CardBg = new Color(0.059f, 0.071f, 0.165f), // hsl(222.2 47.4% 11.2%)
            CardFg = new Color(0.980f, 0.988f, 0.996f),
            CardTitle = new Color(0.980f, 0.988f, 0.996f),
            CardDescription = new Color(0.639f, 0.651f, 0.667f), // hsl(215.4 16.3% 46.9%)

            // Toggle colors
            ToggleBg = new Color(0.118f, 0.161f, 0.231f),
            ToggleFg = new Color(0.980f, 0.988f, 0.996f),
            ToggleHoverBg = new Color(0.157f, 0.196f, 0.267f),
            ToggleHoverFg = new Color(0.980f, 0.988f, 0.996f),
            ToggleOnBg = new Color(0.378f, 0.631f, 0.969f), // hsl(221.2 83.2% 53.3%)
            ToggleOnFg = new Color(0.980f, 0.988f, 0.996f),

            SeparatorColor = new Color(0.118f, 0.161f, 0.231f), // hsl(217.2 32.6% 17.5%)

            // Tabs colors
            TabsBg = new Color(0.059f, 0.071f, 0.165f, 0.5f),
            TabsTriggerFg = new Color(0.639f, 0.651f, 0.667f),
            TabsTriggerActiveBg = new Color(0.012f, 0.027f, 0.071f),
            TabsTriggerActiveFg = new Color(0.980f, 0.988f, 0.996f),

            // Checkbox colors
            CheckboxBg = new Color(0.059f, 0.071f, 0.165f),
            CheckboxCheckedBg = new Color(0.378f, 0.631f, 0.969f),

            // Switch colors
            SwitchBg = new Color(0.118f, 0.161f, 0.231f),
            SwitchOnBg = new Color(0.378f, 0.631f, 0.969f),
            SwitchOffBg = new Color(0.118f, 0.161f, 0.231f),

            // Badge colors
            BadgeBg = new Color(0.059f, 0.071f, 0.165f),
            BadgeSecondaryBg = new Color(0.118f, 0.161f, 0.231f),
            BadgeDestructiveBg = new Color(0.937f, 0.266f, 0.266f),

            // Alert colors
            AlertDefaultBg = new Color(0.012f, 0.027f, 0.071f, 0.8f),
            AlertDefaultFg = new Color(0.980f, 0.988f, 0.996f),
            AlertDestructiveBg = new Color(0.937f, 0.266f, 0.266f, 0.1f),
            AlertDestructiveFg = new Color(0.937f, 0.266f, 0.266f),

            // Avatar colors
            AvatarBg = new Color(0.118f, 0.161f, 0.231f),
            AvatarFallbackBg = new Color(0.118f, 0.161f, 0.231f),
            AvatarFallbackFg = new Color(0.980f, 0.988f, 0.996f),

            // Skeleton colors
            SkeletonBg = new Color(0.059f, 0.071f, 0.165f, 0.3f),

            // Table colors
            TableBg = new Color(0.012f, 0.027f, 0.071f),
            TableHeaderBg = new Color(0.118f, 0.161f, 0.231f),
            TableCellBg = new Color(0.012f, 0.027f, 0.071f),
        };

        public static Theme Light = new Theme
        {
            Name = "Light",
            PrimaryColor = new Color(1.0f, 1.0f, 1.0f), // hsl(0 0% 100%)
            SecondaryColor = new Color(0.945f, 0.961f, 0.976f), // hsl(210 40% 96%)
            AccentColor = new Color(0.231f, 0.510f, 0.965f), // hsl(221.2 83.2% 53.3%)
            BackgroundColor = new Color(1.0f, 1.0f, 1.0f), // hsl(0 0% 100%)
            TextColor = new Color(0.020f, 0.024f, 0.031f), // hsl(222.2 47.4% 11.2%)
            MutedColor = new Color(0.396f, 0.447f, 0.525f), // hsl(215.4 16.3% 46.9%)

            ButtonPrimaryBg = new Color(0.020f, 0.024f, 0.031f), // hsl(222.2 47.4% 11.2%)
            ButtonPrimaryFg = new Color(0.980f, 0.988f, 0.996f), // hsl(210 40% 98%)
            ButtonPrimaryActiveBg = new Color(0.031f, 0.043f, 0.067f),
            ButtonPrimaryActiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonDestructiveBg = new Color(0.937f, 0.266f, 0.266f), // hsl(0 84.2% 60.2%)
            ButtonDestructiveFg = new Color(0.980f, 0.988f, 0.996f),
            ButtonOutlineBorder = new Color(0.886f, 0.898f, 0.918f), // hsl(214.3 31.8% 91.4%)
            ButtonOutlineBg = new Color(1.0f, 1.0f, 1.0f, 0.0f),
            ButtonOutlineFg = new Color(0.020f, 0.024f, 0.031f), // hsl(222.2 47.4% 11.2%)
            ButtonOutlineHoverBg = new Color(0.945f, 0.961f, 0.976f, 0.8f), // hsl(210 40% 96%)
            ButtonOutlineHoverFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonSecondaryBg = new Color(0.945f, 0.961f, 0.976f), // hsl(210 40% 96%)
            ButtonSecondaryFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonGhostFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonGhostHoverBg = new Color(0.945f, 0.961f, 0.976f, 0.8f), // hsl(210 40% 96%)
            ButtonGhostHoverFg = new Color(0.020f, 0.024f, 0.031f),
            ButtonLinkColor = new Color(0.231f, 0.510f, 0.965f), // hsl(221.2 83.2% 53.3%)
            ButtonLinkHoverColor = new Color(0.331f, 0.610f, 1.0f),

            // Input colors
            InputBg = new Color(1.0f, 1.0f, 1.0f), // hsl(0 0% 100%)
            InputFg = new Color(0.020f, 0.024f, 0.031f), // hsl(222.2 47.4% 11.2%)
            InputFocusedBg = new Color(1.0f, 1.0f, 1.0f),
            InputFocusedFg = new Color(0.020f, 0.024f, 0.031f),
            InputDisabledBg = new Color(0.945f, 0.961f, 0.976f, 0.5f), // hsl(210 40% 96%)
            InputDisabledFg = new Color(0.396f, 0.447f, 0.525f), // hsl(215.4 16.3% 46.9%)

            // Card colors
            CardBg = new Color(1.0f, 1.0f, 1.0f), // hsl(0 0% 100%)
            CardFg = new Color(0.020f, 0.024f, 0.031f),
            CardTitle = new Color(0.020f, 0.024f, 0.031f),
            CardDescription = new Color(0.396f, 0.447f, 0.525f), // hsl(215.4 16.3% 46.9%)

            // Toggle colors
            ToggleBg = new Color(0.886f, 0.898f, 0.918f), // hsl(214.3 31.8% 91.4%)
            ToggleFg = new Color(0.020f, 0.024f, 0.031f),
            ToggleHoverBg = new Color(0.827f, 0.847f, 0.878f),
            ToggleHoverFg = new Color(0.020f, 0.024f, 0.031f),
            ToggleOnBg = new Color(0.231f, 0.510f, 0.965f), // hsl(221.2 83.2% 53.3%)
            ToggleOnFg = new Color(0.980f, 0.988f, 0.996f),

            SeparatorColor = new Color(0.886f, 0.898f, 0.918f), // hsl(214.3 31.8% 91.4%)

            // Tabs colors
            TabsBg = new Color(0.945f, 0.961f, 0.976f), // hsl(210 40% 96%)
            TabsTriggerFg = new Color(0.396f, 0.447f, 0.525f), // hsl(215.4 16.3% 46.9%)
            TabsTriggerActiveBg = new Color(1.0f, 1.0f, 1.0f),
            TabsTriggerActiveFg = new Color(0.020f, 0.024f, 0.031f),

            // Checkbox colors
            CheckboxBg = new Color(1.0f, 1.0f, 1.0f),
            CheckboxCheckedBg = new Color(0.231f, 0.510f, 0.965f),

            // Switch colors
            SwitchBg = new Color(0.886f, 0.898f, 0.918f),
            SwitchOnBg = new Color(0.231f, 0.510f, 0.965f),
            SwitchOffBg = new Color(0.886f, 0.898f, 0.918f),

            // Badge colors
            BadgeBg = new Color(0.020f, 0.024f, 0.031f),
            BadgeSecondaryBg = new Color(0.945f, 0.961f, 0.976f),
            BadgeDestructiveBg = new Color(0.937f, 0.266f, 0.266f),

            // Alert colors
            AlertDefaultBg = new Color(1.0f, 1.0f, 1.0f),
            AlertDefaultFg = new Color(0.020f, 0.024f, 0.031f),
            AlertDestructiveBg = new Color(0.937f, 0.266f, 0.266f, 0.1f),
            AlertDestructiveFg = new Color(0.937f, 0.266f, 0.266f),

            // Avatar colors
            AvatarBg = new Color(0.945f, 0.961f, 0.976f),
            AvatarFallbackBg = new Color(0.945f, 0.961f, 0.976f),
            AvatarFallbackFg = new Color(0.020f, 0.024f, 0.031f),

            // Skeleton colors
            SkeletonBg = new Color(0.886f, 0.898f, 0.918f, 0.5f),

            // Table colors
            TableBg = new Color(1.0f, 1.0f, 1.0f),
            TableHeaderBg = new Color(0.945f, 0.961f, 0.976f),
            TableCellBg = new Color(1.0f, 1.0f, 1.0f),
        };
    }
}
