using UnityEngine;
using ShadcnUI.Core;
using ShadcnUI.Core.Styling;
using ShadcnUI.Core.Theming;

namespace ShadcnUI;

/// <summary>
/// Main entry point for all ShadcnUI components.
/// Provides a static API for rendering UI components.
/// </summary>
public static partial class Shadcn
{
    private static IUIService? _services;
    private static bool _isGUIActive = false;

    /// <summary>
    /// Gets or initializes the UI service.
    /// </summary>
    public static IUIService Services => _services ??= new UIService();

    /// <summary>
    /// Current theme from the theme manager.
    /// </summary>
    public static Theme CurrentTheme => Services.ThemeManager.CurrentTheme;

    /// <summary>
    /// Global UI scale factor.
    /// </summary>
    public static float UIScale
    {
        get => Services.UIScale;
        set => Services.UIScale = value;
    }

    /// <summary>
    /// Global font size.
    /// </summary>
    public static int FontSize
    {
        get => Services.FontSize;
        set => Services.FontSize = value;
    }

    /// <summary>
    /// Must be called at the beginning of OnGUI.
    /// </summary>
    public static void BeginGUI()
    {
        _isGUIActive = true;
        GUI.skin.font ??= Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    /// <summary>
    /// Must be called at the end of OnGUI.
    /// </summary>
    public static void EndGUI()
    {
        _isGUIActive = false;
    }

    /// <summary>
    /// Checks if GUI is currently active (between BeginGUI and EndGUI).
    /// </summary>
    internal static bool IsGUIActive => _isGUIActive;

    /// <summary>
    /// Gets a style from the style provider.
    /// </summary>
    public static GUIStyle GetStyle(StyleComponentType type, ControlVariant variant = ControlVariant.Default,
        ControlSize size = ControlSize.Default, ComponentState state = ComponentState.Default)
    {
        return Services.StyleProvider.GetStyle(type, variant, size, state);
    }

    #region Layout Helpers

    /// <summary>
    /// Begins a horizontal layout group.
    /// </summary>
    public static void BeginHorizontal(params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(options);
    }

    /// <summary>
    /// Ends a horizontal layout group.
    /// </summary>
    public static void EndHorizontal()
    {
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Begins a vertical layout group.
    /// </summary>
    public static void BeginVertical(params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical(options);
    }

    /// <summary>
    /// Ends a vertical layout group.
    /// </summary>
    public static void EndVertical()
    {
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Adds space between elements.
    /// </summary>
    public static void Space(float pixels)
    {
        GUILayout.Space(pixels * UIScale);
    }

    /// <summary>
    /// Flexible space that fills available area.
    /// </summary>
    public static void FlexibleSpace()
    {
        GUILayout.FlexibleSpace();
    }

    #endregion
}
