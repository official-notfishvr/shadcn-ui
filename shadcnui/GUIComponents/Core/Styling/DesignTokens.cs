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
            public const float ShadowElevation = 0.20f;
            public const float ShadowBlurSM = 8f;
            public const float ShadowBlurMD = 12f;
            public const float FocusRingThickness = 2.5f;
            public const float FocusRingAlpha = 0.45f;
            public const float SurfaceHighlightTop = 0.04f;
            public const float SurfaceDepthBottom = 0.06f;
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
            public const float Size = 20f;
        }

        public static class Switch
        {
            public const float Width = 40f;
            public const float Height = 22f;
            public const float Radius = 11f;
        }

        public static class Avatar
        {
            public const float BorderThickness = 2f;
            public const float FallbackFontScale = 1.2f;
        }

        public static class ProgressBar
        {
            public const int TextureHeight = 12;
        }

        public static class Chart
        {
            public const float ContainerPaddingH = 16f;
            public const float ContainerPaddingV = 16f;
            public const float AxisFontScale = 0.8f;
            public const float Radius = 8f;
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

        public static class ToastColors
        {
            public static readonly Color SuccessBg = new Color(0.08f, 0.28f, 0.14f, 0.96f);
            public static readonly Color SuccessAccent = new Color(0.35f, 0.85f, 0.45f, 1f);
            public static readonly Color ErrorBg = new Color(0.32f, 0.10f, 0.12f, 0.96f);
            public static readonly Color ErrorAccent = new Color(0.98f, 0.42f, 0.46f, 1f);
            public static readonly Color WarningBg = new Color(0.32f, 0.24f, 0.06f, 0.96f);
            public static readonly Color WarningAccent = new Color(0.98f, 0.78f, 0.32f, 1f);
            public static readonly Color InfoBg = new Color(0.08f, 0.22f, 0.34f, 0.96f);
            public static readonly Color InfoAccent = new Color(0.38f, 0.72f, 0.98f, 1f);
            public static readonly Color DefaultBg = new Color(0.14f, 0.14f, 0.16f, 0.96f);
            public static readonly Color DefaultAccent = new Color(0.72f, 0.72f, 0.76f, 1f);
            public static readonly Color Text = Color.white;
        }
    }
}
