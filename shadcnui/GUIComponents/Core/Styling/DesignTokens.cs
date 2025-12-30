namespace shadcnui.GUIComponents.Core.Styling
{
    public static class DesignTokens
    {
        #region Spacing (4px base unit)
        public static class Spacing
        {
            public const float None = 0f;
            public const float XXS = 2f; // 0.5x
            public const float XS = 4f; // 1x
            public const float SM = 8f; // 2x
            public const float MD = 12f; // 3x
            public const float LG = 16f; // 4x
            public const float XL = 24f; // 6x
            public const float XXL = 32f; // 8x
        }
        #endregion

        #region Border Radius
        public static class Radius
        {
            public const float None = 0f;
            public const float SM = 4f; // Small: checkboxes, small buttons
            public const float MD = 6f; // Medium: buttons, inputs
            public const float LG = 8f; // Large: cards, dialogs
            public const float XL = 12f; // Extra large: badges, pills
            public const float Full = 9999f;
        }
        #endregion

        #region Font Scale
        public static class FontScale
        {
            public const float XS = 0.75f; // 12px at 16px base - Chart labels, hints
            public const float SM = 0.875f; // 14px at 16px base - Body small, buttons
            public const float MD = 1.0f; // 16px at 16px base - Body text
            public const float LG = 1.125f; // 18px at 16px base - Subtitles
            public const float XL = 1.25f; // 20px at 16px base - Card titles
            public const float XXL = 1.5f; // 24px at 16px base - Headers
        }
        #endregion

        #region Component Heights (consistent scale)
        public static class Height
        {
            public const float Mini = 24f; // Mini buttons, compact elements
            public const float Small = 32f; // Small buttons, inputs
            public const float Default = 40f; // Default buttons, inputs
            public const float Large = 48f; // Large buttons
            public const float XL = 56f; // Extra large buttons
        }
        #endregion

        #region Component Padding
        public static class Padding
        {
            // Button padding (horizontal, vertical)
            public const float ButtonMiniH = 8f;
            public const float ButtonMiniV = 4f;
            public const float ButtonSmallH = 12f;
            public const float ButtonSmallV = 6f;
            public const float ButtonDefaultH = 16f;
            public const float ButtonDefaultV = 10f;
            public const float ButtonLargeH = 24f;
            public const float ButtonLargeV = 14f;

            // Input padding
            public const float InputH = 12f;
            public const float InputV = 10f;

            // Card padding
            public const float CardH = 24f;
            public const float CardV = 24f;

            // Badge padding
            public const float BadgeH = 10f;
            public const float BadgeV = 4f;

            // Tab padding
            public const float TabH = 16f;
            public const float TabV = 8f;

            // Table cell padding
            public const float TableCellH = 16f;
            public const float TableCellV = 12f;

            // Dropdown item padding
            public const float DropdownItemH = 12f;
            public const float DropdownItemV = 8f;
        }
        #endregion

        #region Shadows and Effects
        public static class Effects
        {
            // Shadow intensities (0-1)
            public const float ShadowLight = 0.08f;
            public const float ShadowMedium = 0.12f;
            public const float ShadowHeavy = 0.20f;

            // Shadow blur sizes
            public const int ShadowBlurSM = 4;
            public const int ShadowBlurMD = 8;
            public const int ShadowBlurLG = 12;

            // Inner shadow for inputs
            public const float InnerShadowIntensity = 0.08f;
            public const int InnerShadowSize = 3;

            // Focus ring thickness
            public const float FocusRingThickness = 2f;

            // Hover state adjustments
            public const float HoverLighten = 0.12f; // For dark themes
            public const float HoverDarken = 0.08f; // For light themes
            public const float ActiveDarken = 0.15f; // Pressed state
        }
        #endregion

        #region Texture Sizes
        public static class TextureSize
        {
            public const int Small = 32;
            public const int Medium = 64;
            public const int Default = 128;
            public const int Large = 256;
            public const int XL = 512;
        }
        #endregion

        #region Animation
        public static class Animation
        {
            public const float DurationFast = 0.1f;
            public const float DurationNormal = 0.2f;
            public const float DurationSlow = 0.3f;
        }
        #endregion

        #region Slider
        public static class Slider
        {
            // Track heights
            public const float TrackMini = 4f;
            public const float TrackSmall = 6f;
            public const float TrackDefault = 8f;
            public const float TrackLarge = 10f;

            // Thumb sizes
            public const float ThumbMini = 12f;
            public const float ThumbSmall = 16f;
            public const float ThumbDefault = 20f;
            public const float ThumbLarge = 24f;
        }
        #endregion

        #region Status Indicator
        public static class StatusIndicator
        {
            public const float Mini = 4f;
            public const float Small = 6f;
            public const float Default = 8f;
            public const float Large = 12f;
        }
        #endregion

        #region Icons
        public static class Icon
        {
            public const float Small = 16f;
            public const float Default = 20f;
            public const float Large = 24f;
        }
        #endregion

        #region Close Button
        public static class CloseButton
        {
            public const float HitArea = 20f;
            public const float IconSize = 16f;
            public const float FontSize = 14f;
        }
        #endregion

        #region Tab
        public static class Tab
        {
            public const float Height = 36f;
            public const float IndicatorHeight = 3f;
            public const float BorderWidth = 2f;
        }
        #endregion

        #region Z-Index / Layer ordering
        public static class ZIndex
        {
            public const int Base = 0;
            public const int Dropdown = 100;
            public const int Popover = 200;
            public const int Modal = 300;
            public const int Toast = 400;
            public const int Tooltip = 500;
        }
        #endregion

        #region Checkbox
        public static class Checkbox
        {
            public const int Size = 20;
        }
        #endregion

        #region Switch
        public static class Switch
        {
            public const int Width = 40;
            public const int Height = 22;
            public const int Radius = 11;
            public const int ThumbSize = 18;
            public const int ThumbOffset = 2;
        }
        #endregion

        #region Toggle
        public static class Toggle
        {
            public const int TextureSize = 32;
        }
        #endregion

        #region Avatar
        public static class Avatar
        {
            public const float CircleRadiusScale = 50f;
        }
        #endregion

        #region ProgressBar
        public static class ProgressBar
        {
            public const float Height = 10f;
            public const int TextureHeight = 16;
        }
        #endregion

        #region Separator
        public static class Separator
        {
            public const float DefaultThickness = 1f;
            public const float LargeThickness = 2f;
        }
        #endregion

        #region Badge
        public static class Badge
        {
            public const int Height = 28;
        }
        #endregion

        #region Calendar Day
        public static class CalendarDay
        {
            public const float Mini = 20f;
            public const float Small = 28f;
            public const float Default = 32f; // Same as Height.Small
            public const float Large = 40f; // Same as Height.Default
        }
        #endregion

        #region Toast Colors
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
        #endregion
    }
}
