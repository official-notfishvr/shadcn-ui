namespace ShadcnUI.Core;

/// <summary>
/// Design tokens for spacing, sizing, animation, and z-index.
/// All values are in pixels and will be scaled by the UI scale factor at runtime.
/// </summary>
public static class DesignTokens
{
    #region Spacing (in pixels)
    public static class Spacing
    {
        public const float XXS = 2f;
        public const float XS = 4f;
        public const float SM = 8f;
        public const float MD = 12f;
        public const float LG = 16f;
        public const float XL = 24f;
        public const float XXL = 32f;
        public const float XXXL = 48f;
    }
    #endregion

    #region Border Radius (in pixels)
    public static class Radius
    {
        public const float None = 0f;
        public const float SM = 4f;
        public const float MD = 8f;
        public const float LG = 12f;
        public const float XL = 16f;
        public const float Full = 9999f;
    }
    #endregion

    #region Component Heights (in pixels)
    public static class Height
    {
        public const float Mini = 24f;
        public const float Small = 32f;
        public const float Default = 40f;
        public const float Large = 48f;
    }
    #endregion

    #region Font Sizes (in points)
    public static class FontSize
    {
        public const float XS = 10f;
        public const float SM = 12f;
        public const float Default = 14f;
        public const float LG = 16f;
        public const float XL = 18f;
        public const float XXL = 24f;
    }
    #endregion

    #region Z-Index Layers
    public static class ZIndex
    {
        public const int Base = 0;
        public const int Dropdown = 100;
        public const int Sticky = 200;
        public const int Popover = 300;
        public const int Modal = 400;
        public const int Toast = 500;
    }
    #endregion

    #region Animation
    public static class Animation
    {
        public const float DurationFast = 0.1f;
        public const float DurationNormal = 0.2f;
        public const float DurationSlow = 0.3f;
        public const string EasingDefault = "ease-out";
    }
    #endregion

    #region Effects
    public static class Effects
    {
        public const float SurfaceHighlightTop = 0.06f;
        public const float SurfaceDepthBottom = 0.08f;
        public const float ShadowLight = 0.12f;
        public const float ShadowMedium = 0.16f;
        public const float ShadowElevation = 0.18f;
        public const float FocusRingThickness = 2f;
    }
    #endregion

    #region Icon Sizes
    public static class IconSize
    {
        public const float Small = 14f;
        public const float Default = 16f;
        public const float Large = 20f;
    }
    #endregion
}
