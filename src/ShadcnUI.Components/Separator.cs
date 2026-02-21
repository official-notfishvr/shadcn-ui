using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Separator orientation options.
/// </summary>
public enum SeparatorOrientation
{
    Horizontal,
    Vertical
}

/// <summary>
/// Separator variant configuration.
/// </summary>
public readonly record struct SeparatorVariants(
    SeparatorOrientation Orientation = SeparatorOrientation.Horizontal,
    bool Decorative = true
);

/// <summary>
/// Separator style helper.
/// </summary>
public static class SeparatorStyles
{
    public static GUIStyle GetStyle(SeparatorVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Separator);
    }
}

#endregion

#region Separator Component

/// <summary>
/// Separator component for visual separation of content.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a horizontal separator line.
    /// </summary>
    /// <param name="spacing">Spacing before and after the separator</param>
    /// <param name="options">GUILayout options</param>
    public static void Separator(
        float spacing = DesignTokens.Spacing.MD,
        params GUILayoutOption[] options)
    {
        SeparatorInternal(SeparatorOrientation.Horizontal, spacing, null, options);
    }

    /// <summary>
    /// Renders a horizontal separator with custom variants.
    /// </summary>
    public static void Separator(
        SeparatorVariants variants,
        float spacing = DesignTokens.Spacing.MD,
        params GUILayoutOption[] options)
    {
        SeparatorInternal(variants.Orientation, spacing, null, options);
    }

    /// <summary>
    /// Renders a horizontal separator with text label.
    /// </summary>
    /// <param name="text">Text to display on the separator</param>
    /// <param name="spacing">Spacing before and after</param>
    /// <param name="options">GUILayout options</param>
    public static void Separator(
        string text,
        float spacing = DesignTokens.Spacing.MD,
        params GUILayoutOption[] options)
    {
        SeparatorInternal(SeparatorOrientation.Horizontal, spacing, text, options);
    }

    /// <summary>
    /// Renders a horizontal separator with convenient API.
    /// </summary>
    public static void HorizontalSeparator(
        float spacing = DesignTokens.Spacing.MD,
        params GUILayoutOption[] options)
    {
        SeparatorInternal(SeparatorOrientation.Horizontal, spacing, null, options);
    }

    /// <summary>
    /// Renders a vertical separator.
    /// </summary>
    /// <param name="height">Height of the separator</param>
    /// <param name="options">GUILayout options</param>
    public static void VerticalSeparator(
        float height = DesignTokens.Height.Default,
        params GUILayoutOption[] options)
    {
        var allOptions = new List<GUILayoutOption>(options);
        allOptions.Add(GUILayout.Height(height * UIScale));
        allOptions.Add(GUILayout.Width(1));

        SeparatorInternal(SeparatorOrientation.Vertical, 0, null, allOptions.ToArray());
    }

    #region Private Implementation

    private static void SeparatorInternal(
        SeparatorOrientation orientation,
        float spacing,
        string? text,
        GUILayoutOption[] options)
    {
        var theme = CurrentTheme;
        var style = SeparatorStyles.GetStyle(new SeparatorVariants(orientation));

        if (orientation == SeparatorOrientation.Horizontal)
        {
            Space(spacing * 0.5f);

            if (!string.IsNullOrEmpty(text))
            {
                // Text with separator lines on both sides
                BeginHorizontal();
                
                GUILayout.BeginVertical(GUILayout.Height(1));
                FlexibleSpace();
                GUILayout.Box(GUIContent.none, style, GUILayout.ExpandWidth(true), GUILayout.Height(1));
                FlexibleSpace();
                GUILayout.EndVertical();
                
                Space(DesignTokens.Spacing.SM);
                MutedLabel(text);
                Space(DesignTokens.Spacing.SM);
                
                GUILayout.BeginVertical(GUILayout.Height(1));
                FlexibleSpace();
                GUILayout.Box(GUIContent.none, style, GUILayout.ExpandWidth(true), GUILayout.Height(1));
                FlexibleSpace();
                GUILayout.EndVertical();
                
                EndHorizontal();
            }
            else
            {
                GUILayout.Box(GUIContent.none, style, GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }

            Space(spacing * 0.5f);
        }
        else
        {
            // Vertical separator
            GUILayout.Box(GUIContent.none, style, options);
        }
    }

    #endregion
}

#endregion
