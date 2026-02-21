using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Badge variant configuration.
/// </summary>
public readonly record struct BadgeVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Badge style helper.
/// </summary>
public static class BadgeStyles
{
    public static GUIStyle GetStyle(BadgeVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Badge, variants.Variant);
    }
}

#endregion

#region Badge Component

/// <summary>
/// Badge component for displaying status, labels, or counts.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a badge with text.
    /// </summary>
    /// <param name="text">Badge text</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="options">GUILayout options</param>
    public static void Badge(
        string text,
        BadgeVariants? variants = null,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new BadgeVariants();
        var style = BadgeStyles.GetStyle(v);
        
        GUILayout.Label(text, style, options);
    }

    /// <summary>
    /// Renders a badge with a count.
    /// </summary>
    /// <param name="count">Number to display</param>
    /// <param name="max">Maximum value before showing "+"</param>
    /// <param name="variants">Visual variants</param>
    public static void Badge(
        int count,
        int max = 99,
        BadgeVariants? variants = null)
    {
        var text = count > max ? $"{max}+" : count.ToString();
        Badge(text, variants, GUILayout.ExpandWidth(false));
    }

    /// <summary>
    /// Renders a status badge with dot indicator.
    /// </summary>
    /// <param name="text">Badge text</param>
    /// <param name="isActive">Whether the status is active</param>
    /// <param name="variants">Visual variants</param>
    public static void StatusBadge(
        string text,
        bool isActive = true,
        BadgeVariants? variants = null)
    {
        var v = variants ?? new BadgeVariants();
        var theme = CurrentTheme;
        
        BeginHorizontal(GUILayout.ExpandWidth(false));
        
        // Status dot
        var dotSize = 8f * UIScale;
        var dotColor = isActive ? theme.Success : theme.MutedForeground;
        var dotRect = GUILayoutUtility.GetRect(dotSize, dotSize, GUILayout.Width(dotSize), GUILayout.Height(dotSize));
        
        RenderHelpers.DrawCircle(dotRect, dotColor);
        
        Space(DesignTokens.Spacing.XS);
        Badge(text, v);
        
        EndHorizontal();
    }

    #region Convenience Methods for Variants

    /// <summary>
    /// Renders a primary badge.
    /// </summary>
    public static void PrimaryBadge(string text)
    {
        Badge(text, new BadgeVariants(ControlVariant.Primary));
    }

    /// <summary>
    /// Renders a secondary badge.
    /// </summary>
    public static void SecondaryBadge(string text)
    {
        Badge(text, new BadgeVariants(ControlVariant.Secondary));
    }

    /// <summary>
    /// Renders a destructive badge.
    /// </summary>
    public static void DestructiveBadge(string text)
    {
        Badge(text, new BadgeVariants(ControlVariant.Destructive));
    }

    /// <summary>
    /// Renders an outline badge.
    /// </summary>
    public static void OutlineBadge(string text)
    {
        Badge(text, new BadgeVariants(ControlVariant.Outline));
    }

    /// <summary>
    /// Renders a ghost badge.
    /// </summary>
    public static void GhostBadge(string text)
    {
        Badge(text, new BadgeVariants(ControlVariant.Ghost));
    }

    #endregion
}

#endregion
