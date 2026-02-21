using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Progress variant configuration.
/// </summary>
public readonly record struct ProgressVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Progress style helper.
/// </summary>
public static class ProgressStyles
{
    public static GUIStyle GetTrackStyle(ProgressVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Progress, variants.Variant);
    }

    public static GUIStyle GetIndicatorStyle(ProgressVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Progress, variants.Variant, ControlSize.Default, ComponentState.Checked);
    }
}

#endregion

#region Progress Component

/// <summary>
/// Progress component for showing completion status.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a linear progress bar.
    /// </summary>
    /// <param name="value">Current value (0-100)</param>
    /// <param name="max">Maximum value</param>
    /// <param name="label">Optional label</param>
    /// <param name="showPercentage">Show percentage text</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="options">GUILayout options</param>
    public static void Progress(
        float value,
        float max = 100f,
        string? label = null,
        bool showPercentage = true,
        ProgressVariants? variants = null,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new ProgressVariants();
        var theme = CurrentTheme;
        
        // Calculate percentage
        var percentage = max > 0 ? Mathf.Clamp01(value / max) : 0f;
        var percentageText = Mathf.RoundToInt(percentage * 100) + "%";
        
        // Optional label row
        if (!string.IsNullOrEmpty(label) || showPercentage)
        {
            BeginHorizontal();
            
            if (!string.IsNullOrEmpty(label))
            {
                Label(label);
            }
            
            if (showPercentage)
            {
                FlexibleSpace();
                Label(percentageText, ControlVariant.Muted);
            }
            
            EndHorizontal();
            Space(DesignTokens.Spacing.XS);
        }
        
        // Progress bar
        var trackHeight = 8f * UIScale;
        var trackRect = GUILayoutUtility.GetRect(100, trackHeight, options);
        
        // Track background
        RenderHelpers.DrawRoundedRect(trackRect, theme.Muted, Mathf.RoundToInt(trackHeight / 2));
        
        // Progress fill
        if (percentage > 0)
        {
            var fillWidth = trackRect.width * percentage;
            var fillRect = new Rect(trackRect.x, trackRect.y, fillWidth, trackRect.height);
            var fillColor = v.Variant switch
            {
                ControlVariant.Primary => theme.Primary,
                ControlVariant.Secondary => theme.Secondary,
                ControlVariant.Destructive => theme.Destructive,
                ControlVariant.Success => theme.Success,
                ControlVariant.Ghost => theme.MutedForeground,
                _ => theme.Primary
            };
            
            RenderHelpers.DrawRoundedRect(fillRect, fillColor, Mathf.RoundToInt(trackHeight / 2));
        }
        
        Space(DesignTokens.Spacing.SM);
    }

    /// <summary>
    /// Renders an indeterminate progress bar.
    /// </summary>
    public static void ProgressIndeterminate(
        string? label = null,
        ProgressVariants? variants = null,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new ProgressVariants();
        var theme = CurrentTheme;
        
        if (!string.IsNullOrEmpty(label))
        {
            Label(label);
            Space(DesignTokens.Spacing.XS);
        }
        
        var trackHeight = 8f * UIScale;
        var trackRect = GUILayoutUtility.GetRect(100, trackHeight, options);
        
        // Track background
        RenderHelpers.DrawRoundedRect(trackRect, theme.Muted, Mathf.RoundToInt(trackHeight / 2));
        
        // Animated indeterminate segment
        var segmentWidth = trackRect.width * 0.3f;
        var animOffset = Mathf.PingPong(Time.time * 0.5f, 1f - 0.3f);
        var fillRect = new Rect(
            trackRect.x + (trackRect.width - segmentWidth) * animOffset,
            trackRect.y,
            segmentWidth,
            trackRect.height
        );
        
        var fillColor = v.Variant switch
        {
            ControlVariant.Primary => theme.Primary,
            ControlVariant.Secondary => theme.Secondary,
            ControlVariant.Destructive => theme.Destructive,
            _ => theme.Primary
        };
        
        RenderHelpers.DrawRoundedRect(fillRect, fillColor, Mathf.RoundToInt(trackHeight / 2));
        
        Space(DesignTokens.Spacing.SM);
    }

    #region Convenience Methods

    /// <summary>
    /// Renders a primary progress bar.
    /// </summary>
    public static void PrimaryProgress(float value, float max = 100f, string? label = null, bool showPercentage = true)
    {
        Progress(value, max, label, showPercentage, new ProgressVariants(ControlVariant.Primary));
    }

    /// <summary>
    /// Renders a secondary progress bar.
    /// </summary>
    public static void SecondaryProgress(float value, float max = 100f, string? label = null, bool showPercentage = true)
    {
        Progress(value, max, label, showPercentage, new ProgressVariants(ControlVariant.Secondary));
    }

    /// <summary>
    /// Renders a destructive/error progress bar.
    /// </summary>
    public static void DestructiveProgress(float value, float max = 100f, string? label = null, bool showPercentage = true)
    {
        Progress(value, max, label, showPercentage, new ProgressVariants(ControlVariant.Destructive));
    }

    /// <summary>
    /// Renders a success progress bar.
    /// </summary>
    public static void SuccessProgress(float value, float max = 100f, string? label = null, bool showPercentage = true)
    {
        Progress(value, max, label, showPercentage, new ProgressVariants(ControlVariant.Success));
    }

    #endregion

    #region Helper Methods

    #endregion
}

#endregion
