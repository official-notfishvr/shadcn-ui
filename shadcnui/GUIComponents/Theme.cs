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

        // Button
        public Color ButtonPrimaryBg { get; set; }
        public Color ButtonPrimaryFg { get; set; }
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
            PrimaryColor = new Color(0.09f, 0.09f, 0.11f),
            SecondaryColor = new Color(0.16f, 0.16f, 0.18f),
            AccentColor = new Color(0.5f, 0.8f, 1f),
            BackgroundColor = new Color(0.02f, 0.02f, 0.04f),
            TextColor = new Color(0.98f, 0.98f, 0.98f),

            ButtonPrimaryBg = new Color(0.09f, 0.09f, 0.11f),
            ButtonPrimaryFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonDestructiveBg = new Color(0.86f, 0.24f, 0.24f),
            ButtonDestructiveFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonOutlineBorder = new Color(0.23f, 0.23f, 0.27f),
            ButtonOutlineBg = new Color(0.02f, 0.02f, 0.04f),
            ButtonOutlineFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonOutlineHoverBg = new Color(0.16f, 0.16f, 0.18f),
            ButtonOutlineHoverFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonSecondaryBg = new Color(0.16f, 0.16f, 0.18f),
            ButtonSecondaryFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonGhostFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonGhostHoverBg = new Color(0.16f, 0.16f, 0.18f),
            ButtonGhostHoverFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonLinkColor = new Color(0.09f, 0.09f, 0.11f),
            ButtonLinkHoverColor = Color.Lerp(new Color(0.09f, 0.09f, 0.11f), Color.white, 0.2f),

            InputBg = new Color(0.1f, 0.1f, 0.15f, 0.8f),
            InputFg = Color.white,
            InputFocusedBg = new Color(0.2f, 0.3f, 0.5f, 0.9f),
            InputFocusedFg = new Color(0.9f, 0.9f, 1f),
            InputDisabledBg = new Color(0.2f, 0.2f, 0.2f, 0.5f),
            InputDisabledFg = new Color(0.5f, 0.5f, 0.5f),

            CardBg = new Color(0.15f, 0.15f, 0.25f),
            CardFg = new Color(0.9f, 0.9f, 1f),
            CardTitle = new Color(0.9f, 0.9f, 1f),
            CardDescription = new Color(0.7f, 0.7f, 0.8f),

            ToggleBg = new Color(0.16f, 0.16f, 0.18f),
            ToggleFg = new Color(0.8f, 0.8f, 0.9f),
            ToggleHoverBg = new Color(0.16f, 0.16f, 0.18f, 0.5f),
            ToggleHoverFg = Color.white,
            ToggleOnBg = new Color(0.3f, 0.6f, 1.0f),
            ToggleOnFg = Color.white,

            SeparatorColor = new Color(0.23f, 0.23f, 0.27f),

            TabsBg = new Color(0.16f, 0.16f, 0.18f),
            TabsTriggerFg = new Color(0.64f, 0.64f, 0.71f),
            TabsTriggerActiveBg = new Color(0.02f, 0.02f, 0.04f),
            TabsTriggerActiveFg = new Color(0.98f, 0.98f, 0.98f),

            CheckboxBg = new Color(0.1f, 0.1f, 0.15f),
            CheckboxCheckedBg = new Color(0.09f, 0.09f, 0.11f),

            SwitchBg = new Color(0.16f, 0.16f, 0.18f),
            SwitchOnBg = new Color(0.09f, 0.09f, 0.11f),
            SwitchOffBg = new Color(0.16f, 0.16f, 0.18f),

            BadgeBg = new Color(0.09f, 0.09f, 0.11f),
            BadgeSecondaryBg = new Color(0.16f, 0.16f, 0.18f),
            BadgeDestructiveBg = new Color(0.86f, 0.24f, 0.24f),

            AlertDefaultBg = new Color(0.02f, 0.02f, 0.04f),
            AlertDefaultFg = new Color(0.98f, 0.98f, 0.98f),
            AlertDestructiveBg = new Color(0.86f, 0.24f, 0.24f),
            AlertDestructiveFg = new Color(0.86f, 0.24f, 0.24f),

            AvatarBg = new Color(0.16f, 0.16f, 0.18f),
            AvatarFallbackBg = new Color(0.16f, 0.16f, 0.18f),
            AvatarFallbackFg = new Color(0.98f, 0.98f, 0.98f),

            SkeletonBg = new Color(0.09f, 0.09f, 0.11f, 0.1f),

            TableBg = new Color(0.02f, 0.02f, 0.04f),
            TableHeaderBg = new Color(0.16f, 0.16f, 0.18f),
            TableCellBg = new Color(0.02f, 0.02f, 0.04f),
        };

        public static Theme Light = new Theme
        {
            Name = "Light",
            PrimaryColor = new Color(0.9f, 0.9f, 0.9f),
            SecondaryColor = new Color(0.8f, 0.8f, 0.8f),
            AccentColor = new Color(0.2f, 0.5f, 1f),
            BackgroundColor = new Color(1f, 1f, 1f),
            TextColor = new Color(0.02f, 0.02f, 0.02f),

            ButtonPrimaryBg = new Color(0.9f, 0.9f, 0.9f),
            ButtonPrimaryFg = new Color(0.02f, 0.02f, 0.02f),
            ButtonDestructiveBg = new Color(0.86f, 0.24f, 0.24f),
            ButtonDestructiveFg = new Color(0.98f, 0.98f, 0.98f),
            ButtonOutlineBorder = new Color(0.8f, 0.8f, 0.8f),
            ButtonOutlineBg = new Color(1f, 1f, 1f),
            ButtonOutlineFg = new Color(0.02f, 0.02f, 0.02f),
            ButtonOutlineHoverBg = new Color(0.9f, 0.9f, 0.9f),
            ButtonOutlineHoverFg = new Color(0.02f, 0.02f, 0.02f),
            ButtonSecondaryBg = new Color(0.9f, 0.9f, 0.9f),
            ButtonSecondaryFg = new Color(0.02f, 0.02f, 0.02f),
            ButtonGhostFg = new Color(0.02f, 0.02f, 0.02f),
            ButtonGhostHoverBg = new Color(0.9f, 0.9f, 0.9f),
            ButtonGhostHoverFg = new Color(0.02f, 0.02f, 0.02f),
            ButtonLinkColor = new Color(0.9f, 0.9f, 0.9f),
            ButtonLinkHoverColor = Color.Lerp(new Color(0.9f, 0.9f, 0.9f), Color.black, 0.2f),

            InputBg = new Color(0.9f, 0.9f, 0.9f, 0.8f),
            InputFg = Color.black,
            InputFocusedBg = new Color(0.8f, 0.9f, 1f, 0.9f),
            InputFocusedFg = new Color(0.1f, 0.1f, 0.2f),
            InputDisabledBg = new Color(0.8f, 0.8f, 0.8f, 0.5f),
            InputDisabledFg = new Color(0.5f, 0.5f, 0.5f),

            CardBg = new Color(0.95f, 0.95f, 1f),
            CardFg = new Color(0.1f, 0.1f, 0.2f),
            CardTitle = new Color(0.1f, 0.1f, 0.2f),
            CardDescription = new Color(0.3f, 0.3f, 0.4f),

            ToggleBg = new Color(0.9f, 0.9f, 0.9f),
            ToggleFg = new Color(0.2f, 0.2f, 0.3f),
            ToggleHoverBg = new Color(0.9f, 0.9f, 0.9f, 0.5f),
            ToggleHoverFg = Color.black,
            ToggleOnBg = new Color(0.7f, 0.4f, 0.0f),
            ToggleOnFg = Color.black,

            SeparatorColor = new Color(0.8f, 0.8f, 0.8f),

            TabsBg = new Color(0.9f, 0.9f, 0.9f),
            TabsTriggerFg = new Color(0.36f, 0.36f, 0.29f),
            TabsTriggerActiveBg = new Color(1f, 1f, 1f),
            TabsTriggerActiveFg = new Color(0.02f, 0.02f, 0.02f),

            CheckboxBg = new Color(0.9f, 0.9f, 0.9f),
            CheckboxCheckedBg = new Color(1f, 1f, 1f),

            SwitchBg = new Color(0.9f, 0.9f, 0.9f),
            SwitchOnBg = new Color(1f, 1f, 1f),
            SwitchOffBg = new Color(0.9f, 0.9f, 0.9f),

            BadgeBg = new Color(1f, 1f, 1f),
            BadgeSecondaryBg = new Color(0.9f, 0.9f, 0.9f),
            BadgeDestructiveBg = new Color(0.86f, 0.24f, 0.24f),

            AlertDefaultBg = new Color(1f, 1f, 1f),
            AlertDefaultFg = new Color(0.02f, 0.02f, 0.02f),
            AlertDestructiveBg = new Color(0.86f, 0.24f, 0.24f),
            AlertDestructiveFg = new Color(0.86f, 0.24f, 0.24f),

            AvatarBg = new Color(0.9f, 0.9f, 0.9f),
            AvatarFallbackBg = new Color(0.9f, 0.9f, 0.9f),
            AvatarFallbackFg = new Color(0.02f, 0.02f, 0.02f),

            SkeletonBg = new Color(1f, 1f, 1f, 0.1f),

            TableBg = new Color(1f, 1f, 1f),
            TableHeaderBg = new Color(0.9f, 0.9f, 0.9f),
            TableCellBg = new Color(1f, 1f, 1f),
        };
    }
}
