using UnityEngine;

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
            public const float SM = 1f;
            public const float MD = 2f;
            public const float LG = 4f;
            public const float XL = 6f;
            public const float Full = 9999f;
        }

        public static class FontScale
        {
            public const float XS = 0.75f;
            public const float SM = 0.875f;
            public const float MD = 1f;
            public const float LG = 1.125f;
            public const float XL = 1.25f;
            public const float XXL = 1.5f;
        }

        public static class Height
        {
            public const float Mini = 22f;
            public const float Small = 28f;
            public const float Default = 34f;
            public const float Large = 42f;
            public const float XL = 52f;
        }

        public static class Padding
        {
            public static class Button
            {
                public const float MiniH = 8f;
                public const float MiniV = 4f;
                public const float SmallH = 10f;
                public const float SmallV = 6f;
                public const float DefaultH = 12f;
                public const float DefaultV = 8f;
                public const float LargeH = 16f;
                public const float LargeV = 10f;
            }

            public static class Input
            {
                public const float Horizontal = 10f;
                public const float Vertical = 8f;
            }

            public static class Card
            {
                public const float Horizontal = 16f;
                public const float Vertical = 16f;
            }

            public static class Badge
            {
                public const float Horizontal = 8f;
                public const float Vertical = 3f;
            }

            public static class Tab
            {
                public const float Horizontal = 12f;
                public const float Vertical = 8f;
            }

            public static class Table
            {
                public const float CellH = 12f;
                public const float CellV = 8f;
            }

            public static class Dropdown
            {
                public const float ItemH = 10f;
                public const float ItemV = 8f;
            }
        }

        public static class Effects
        {
            public const float ShadowLight = 0.04f;
            public const float ShadowMedium = 0.08f;
            public const float ShadowHeavy = 0.12f;
            public const float ShadowElevation = 0.14f;
            public const float ShadowBlurSM = 4f;
            public const float ShadowBlurMD = 8f;
            public const float FocusRingThickness = 1f;
            public const float FocusRingAlpha = 0.55f;
            public const float SurfaceHighlightTop = 0.01f;
            public const float SurfaceDepthBottom = 0.02f;
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
            public const float DurationFast = 0.10f;
            public const float DurationNormal = 0.18f;
            public const float DurationSlow = 0.28f;
        }

        public static class Slider
        {
            public const float TrackMini = 4f;
            public const float TrackSmall = 5f;
            public const float TrackDefault = 6f;
            public const float TrackLarge = 8f;
            public const float ThumbMini = 10f;
            public const float ThumbSmall = 12f;
            public const float ThumbDefault = 14f;
            public const float ThumbLarge = 18f;
        }

        public static class StatusIndicator
        {
            public const float Mini = 4f;
            public const float Small = 5f;
            public const float Default = 6f;
            public const float Large = 8f;
        }

        public static class Icon
        {
            public const float Small = 14f;
            public const float Default = 18f;
            public const float Large = 22f;
        }

        public static class ZIndex
        {
            public const int Base = 0;
            public const int Dropdown = 100;
            public const int Popover = 200;
            public const int Modal = 300;
            public const int Toast = 400;
        }

        public static class Checkbox
        {
            public const float Size = 18f;
        }

        public static class Switch
        {
            public const float Width = 38f;
            public const float Height = 20f;
            public const float Radius = 10f;
        }

        public static class Layout
        {
            public const float ControlTextSlack = 4f;
        }

        public static class Avatar
        {
            public const float BorderThickness = 1f;
            public const float FallbackFontScale = 1f;
        }

        public static class ProgressBar
        {
            public const int TextureHeight = 8;
        }

        public static class Chart
        {
            public const float ContainerPaddingH = 12f;
            public const float ContainerPaddingV = 12f;
            public const float AxisFontScale = 0.8f;
            public const float Radius = 2f;
        }

        public static class Separator
        {
            public const float DefaultThickness = 1f;
            public const float LargeThickness = 2f;
        }

        public static class Badge
        {
            public const float Height = 24f;
        }

        public static class ToastColors
        {
            public static readonly Color SuccessBg = new(0.09f, 0.18f, 0.12f, 0.98f);
            public static readonly Color SuccessAccent = new(0.32f, 0.82f, 0.47f, 1f);
            public static readonly Color ErrorBg = new(0.20f, 0.11f, 0.11f, 0.98f);
            public static readonly Color ErrorAccent = new(0.94f, 0.34f, 0.34f, 1f);
            public static readonly Color WarningBg = new(0.23f, 0.18f, 0.10f, 0.98f);
            public static readonly Color WarningAccent = new(0.95f, 0.76f, 0.24f, 1f);
            public static readonly Color InfoBg = new(0.10f, 0.16f, 0.22f, 0.98f);
            public static readonly Color InfoAccent = new(0.39f, 0.67f, 0.98f, 1f);
            public static readonly Color DefaultBg = new(0.12f, 0.12f, 0.14f, 0.98f);
            public static readonly Color DefaultAccent = new(0.72f, 0.72f, 0.76f, 1f);
            public static readonly Color Text = Color.white;
        }
    }
}
