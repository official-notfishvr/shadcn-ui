using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Alert variant configuration.
/// </summary>
public readonly record struct AlertVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Alert style helper.
/// </summary>
public static class AlertStyles
{
    public static GUIStyle GetContainerStyle(AlertVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Alert, variants.Variant);
    }
}

#endregion

#region Alert Component

/// <summary>
/// Alert component for displaying important messages to users.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders an alert box.
    /// </summary>
    /// <param name="title">Alert title</param>
    /// <param name="description">Alert description</param>
    /// <param name="variant">Visual variant</param>
    /// <param name="options">GUILayout options</param>
    public static void Alert(
        string title,
        string? description = null,
        ControlVariant variant = ControlVariant.Default,
        params GUILayoutOption[] options)
    {
        var theme = CurrentTheme;
        
        // Get colors based on variant
        var (bgColor, borderColor, iconColor, textColor) = GetAlertColors(variant);
        
        // Container
        var padding = Mathf.RoundToInt(DesignTokens.Spacing.MD * UIScale);
        var radius = Mathf.RoundToInt(DesignTokens.Radius.MD * UIScale);
        
        var containerRect = GUILayoutUtility.GetRect(100, 60, options);
        
        // Draw background
        RenderHelpers.DrawRoundedRect(containerRect, bgColor, radius);
        RenderHelpers.DrawBorder(containerRect, borderColor, 1, radius);
        
        // Content area
        var contentRect = new Rect(
            containerRect.x + padding,
            containerRect.y + padding,
            containerRect.width - padding * 2,
            containerRect.height - padding * 2
        );
        
        // Icon
        var iconSize = 20f * UIScale;
        var iconRect = new Rect(contentRect.x, contentRect.y, iconSize, iconSize);
        DrawAlertIcon(iconRect, variant, iconColor);
        
        // Title and description
        var textX = contentRect.x + iconSize + DesignTokens.Spacing.SM * UIScale;
        var textWidth = contentRect.width - iconSize - DesignTokens.Spacing.SM * UIScale;
        
        var titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.SM * UIScale),
            fontStyle = FontStyle.Bold,
            normal = { textColor = textColor }
        };
        
        GUI.Label(new Rect(textX, contentRect.y, textWidth, iconSize), title, titleStyle);
        
        if (!string.IsNullOrEmpty(description))
        {
            var descY = contentRect.y + iconSize + DesignTokens.Spacing.XS * UIScale;
            var descStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(DesignTokens.FontSize.SM * UIScale),
                wordWrap = true,
                normal = { textColor = textColor }
            };
            descStyle.normal.textColor = new Color(textColor.r, textColor.g, textColor.b, 0.8f);
            
            GUI.Label(new Rect(textX, descY, textWidth, iconSize), description, descStyle);
        }
    }

    #region Convenience Methods

    /// <summary>
    /// Renders a default alert.
    /// </summary>
    public static void DefaultAlert(string title, string? description = null)
    {
        Alert(title, description, ControlVariant.Default);
    }

    /// <summary>
    /// Renders a destructive/error alert.
    /// </summary>
    public static void DestructiveAlert(string title, string? description = null)
    {
        Alert(title, description, ControlVariant.Destructive);
    }

    /// <summary>
    /// Renders a success alert.
    /// </summary>
    public static void SuccessAlert(string title, string? description = null)
    {
        Alert(title, description, ControlVariant.Success);
    }

    /// <summary>
    /// Renders a warning alert.
    /// </summary>
    public static void WarningAlert(string title, string? description = null)
    {
        Alert(title, description, ControlVariant.Primary);
    }

    #endregion

    #region Helper Methods

    private static (Color bg, Color border, Color icon, Color text) GetAlertColors(ControlVariant variant)
    {
        var theme = CurrentTheme;
        
        return variant switch
        {
            ControlVariant.Destructive => (
                new Color(theme.Destructive.r, theme.Destructive.g, theme.Destructive.b, 0.1f),
                theme.Destructive,
                theme.Destructive,
                theme.Destructive
            ),
            ControlVariant.Success => (
                new Color(theme.Success.r, theme.Success.g, theme.Success.b, 0.1f),
                theme.Success,
                theme.Success,
                theme.Success
            ),
            ControlVariant.Primary => (
                new Color(theme.Primary.r, theme.Primary.g, theme.Primary.b, 0.1f),
                theme.Primary,
                theme.Primary,
                theme.Primary
            ),
            _ => (
                theme.Muted,
                theme.Border,
                theme.MutedForeground,
                theme.Foreground
            )
        };
    }

    private static void DrawAlertIcon(Rect rect, ControlVariant variant, Color color)
    {
        // Simplified icon - just a circle with symbol
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        
        var iconStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.RoundToInt(14 * UIScale),
            fontStyle = FontStyle.Bold,
            normal = { textColor = color }
        };
        
        var symbol = variant switch
        {
            ControlVariant.Destructive => "!",
            ControlVariant.Success => "✓",
            ControlVariant.Primary => "!",
            _ => "i"
        };
        
        GUI.Label(rect, symbol, iconStyle);
        
        UnityEngine.Object.DestroyImmediate(texture);
    }

    #endregion
}

#endregion
