namespace shadcnui.GUIComponents.Core.Styling
{
    public static class DesignTokens
    {
        public static class Spacing
        {
            public const float None = 0f;
            public const float XXS = 2f;
            public const float XS = 4f;
            public const float SM = 8f;
            public const float MD = 12f;
            public const float LG = 16f;
            public const float XL = 24f;
            public const float XXL = 32f;
        }

        public static class Radius
        {
            public const float None = 0f;
            public const float SM = 4f;
            public const float MD = 8f;
            public const float LG = 12f;
            public const float XL = 16f;
            public const float Full = 9999f;
        }

        public static class FontScale
        {
            public const float XS = 0.75f;
            public const float SM = 0.875f;
            public const float MD = 1.0f;
            public const float LG = 1.125f;
            public const float XL = 1.25f;
            public const float XXL = 1.5f;
        }

        public static class Height
        {
            public const float Mini = 24f;
            public const float Small = 32f;
            public const float Default = 40f;
            public const float Large = 48f;
            public const float XL = 56f;
        }

        public static class Padding
        {
            public static class Button
            {
                public const float MiniH = 8f;
                public const float MiniV = 4f;
                public const float SmallH = 12f;
                public const float SmallV = 6f;
                public const float DefaultH = 16f;
                public const float DefaultV = 10f;
                public const float LargeH = 24f;
                public const float LargeV = 14f;
            }

            public static class Input
            {
                public const float Horizontal = 12f;
                public const float Vertical = 10f;
            }

            public static class Card
            {
                public const float Horizontal = 24f;
                public const float Vertical = 24f;
            }

            public static class Badge
            {
                public const float Horizontal = 10f;
                public const float Vertical = 4f;
            }

            public static class Tab
            {
                public const float Horizontal = 16f;
                public const float Vertical = 8f;
            }

            public static class Table
            {
                public const float CellH = 16f;
                public const float CellV = 12f;
            }

            public static class Dropdown
            {
                public const float ItemH = 12f;
                public const float ItemV = 8f;
            }
        }

        public static class Effects
        {
            public const float ShadowLight = 0.10f;
            public const float ShadowMedium = 0.16f;
            public const float ShadowHeavy = 0.24f;

            public const float ShadowBlurSM = 6f;
            public const float ShadowBlurMD = 12f;
            public const float ShadowBlurLG = 20f;

            public const float InnerShadowIntensity = 0.06f;
            public const float InnerShadowSize = 2f;

            public const float FocusRingThickness = 2f;

            public const float HoverLighten = 0.12f;
            public const float HoverDarken = 0.08f;
            public const float ActiveDarken = 0.15f;

            public const float GradientIntensity = 0.10f;
            public const float RimHighlight = 0.15f;
        }

        public static class TextureSize
        {
            public const int Small = 32;
            public const int Medium = 64;
            public const int Default = 128;
            public const int Large = 256;
            public const int XL = 512;
        }

        public static class Animation
        {
            public const float DurationFast = 0.1f;
            public const float DurationNormal = 0.2f;
            public const float DurationSlow = 0.3f;
        }

        public static class Slider
        {
            public const float TrackMini = 4f;
            public const float TrackSmall = 6f;
            public const float TrackDefault = 8f;
            public const float TrackLarge = 10f;

            public const float ThumbMini = 12f;
            public const float ThumbSmall = 16f;
            public const float ThumbDefault = 20f;
            public const float ThumbLarge = 24f;
        }

        public static class StatusIndicator
        {
            public const float Mini = 4f;
            public const float Small = 6f;
            public const float Default = 8f;
            public const float Large = 12f;
        }

        public static class Icon
        {
            public const float Small = 16f;
            public const float Default = 20f;
            public const float Large = 24f;
        }

        public static class CloseButton
        {
            public const float HitArea = 20f;
            public const float IconSize = 16f;
            public const float FontSize = 14f;
        }

        public static class Tab
        {
            public const float Height = 36f;
            public const float IndicatorHeight = 3f;
            public const float BorderWidth = 2f;
        }

        public static class ZIndex
        {
            public const int Base = 0;
            public const int Dropdown = 100;
            public const int Popover = 200;
            public const int Modal = 300;
            public const int Toast = 400;
            public const int Tooltip = 500;
        }

        public static class Checkbox
        {
            public const float Size = 20f;
        }

        public static class Switch
        {
            public const float Width = 40f;
            public const float Height = 22f;
            public const float Radius = 11f;
            public const float ThumbSize = 18f;
            public const float ThumbOffset = 2f;
        }

        public static class Toggle
        {
            public const int TextureSize = 32;
        }

        public static class Avatar
        {
            public const float CircleRadiusScale = 50f;
        }

        public static class ProgressBar
        {
            public const float Height = 10f;
            public const int TextureHeight = 16;
        }

        public static class Separator
        {
            public const float DefaultThickness = 1f;
            public const float LargeThickness = 2f;
        }

        public static class Badge
        {
            public const float Height = 28f;
        }

        public static class CalendarDay
        {
            public const float Mini = 20f;
            public const float Small = 28f;
            public const float Default = 32f;
            public const float Large = 40f;
        }

        public static class ToastColors
        {
            public static readonly UnityEngine.Color SuccessBg = new UnityEngine.Color(0.15f, 0.35f, 0.15f, 0.95f);
            public static readonly UnityEngine.Color SuccessBorder = new UnityEngine.Color(0.4f, 0.8f, 0.4f, 0.5f);
            public static readonly UnityEngine.Color SuccessAccent = new UnityEngine.Color(0.4f, 0.8f, 0.4f, 1f);

            public static readonly UnityEngine.Color ErrorBg = new UnityEngine.Color(0.45f, 0.15f, 0.15f, 0.95f);
            public static readonly UnityEngine.Color ErrorBorder = new UnityEngine.Color(1f, 0.4f, 0.4f, 0.5f);
            public static readonly UnityEngine.Color ErrorAccent = new UnityEngine.Color(1f, 0.4f, 0.4f, 1f);

            public static readonly UnityEngine.Color WarningBg = new UnityEngine.Color(0.45f, 0.35f, 0.1f, 0.95f);
            public static readonly UnityEngine.Color WarningBorder = new UnityEngine.Color(1f, 0.8f, 0.4f, 0.5f);
            public static readonly UnityEngine.Color WarningAccent = new UnityEngine.Color(1f, 0.8f, 0.4f, 1f);

            public static readonly UnityEngine.Color InfoBg = new UnityEngine.Color(0.15f, 0.35f, 0.45f, 0.95f);
            public static readonly UnityEngine.Color InfoBorder = new UnityEngine.Color(0.4f, 0.8f, 1f, 0.5f);
            public static readonly UnityEngine.Color InfoAccent = new UnityEngine.Color(0.4f, 0.8f, 1f, 1f);

            public static readonly UnityEngine.Color DefaultBg = new UnityEngine.Color(0.2f, 0.2f, 0.2f, 0.95f);
            public static readonly UnityEngine.Color DefaultBorder = new UnityEngine.Color(0.5f, 0.5f, 0.5f, 0.5f);
            public static readonly UnityEngine.Color DefaultAccent = new UnityEngine.Color(0.6f, 0.6f, 0.6f, 1f);

            public static readonly UnityEngine.Color Text = UnityEngine.Color.white;
        }
    }
}
