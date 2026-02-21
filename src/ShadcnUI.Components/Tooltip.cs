using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Tooltip variant configuration.
/// </summary>
public readonly record struct TooltipVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Tooltip style helper.
/// </summary>
public static class TooltipStyles
{
    public static GUIStyle GetContainerStyle(TooltipVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Tooltip, variants.Variant);
    }
}

#endregion

#region Tooltip Component

/// <summary>
/// Tooltip component for displaying information on hover.
/// </summary>
public static partial class Shadcn
{
    private static string? _activeTooltip;
    private static float _tooltipTimer;
    private static readonly float TOOLTIP_DELAY = 0.4f;

    /// <summary>
    /// Renders content with a tooltip that appears on hover.
    /// </summary>
    /// <param name="tooltip">Tooltip text</param>
    /// <param name="content">Content to render</param>
    /// <param name="variants">Visual variants</param>
    public static void WithTooltip(
        string tooltip,
        Action content,
        TooltipVariants? variants = null)
    {
        // Store current GUI state
        var lastRect = GUILayoutUtility.GetLastRect();
        
        // Render content
        content?.Invoke();
        
        // Get the rect of rendered content
        var contentRect = GUILayoutUtility.GetLastRect();
        
        // Check for hover
        HandleTooltipHover(contentRect, tooltip);
    }

    /// <summary>
    /// Renders a button with tooltip.
    /// </summary>
    public static bool ButtonWithTooltip(
        string text,
        string tooltip,
        Action? onClick = null,
        ButtonVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var clicked = Button(text, onClick, variants, disabled, options);
        
        if (!string.IsNullOrEmpty(tooltip))
        {
            var lastRect = GUILayoutUtility.GetLastRect();
            HandleTooltipHover(lastRect, tooltip);
        }
        
        return clicked;
    }

    /// <summary>
    /// Must be called at the end of OnGUI to render active tooltip.
    /// </summary>
    public static void RenderTooltip()
    {
        if (string.IsNullOrEmpty(_activeTooltip)) return;
        
        var theme = CurrentTheme;
        var mousePos = Event.current.mousePosition;
        
        // Calculate tooltip size
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.SM * UIScale),
            normal = { textColor = theme.Foreground },
            wordWrap = true
        };
        
        var content = new GUIContent(_activeTooltip);
        var size = style.CalcSize(content);
        size.x = Mathf.Min(size.x, 280f * UIScale);
        size.y = style.CalcHeight(content, size.x);
        
        // Position tooltip near mouse but keep on screen
        var padding = 10f * UIScale;
        var x = mousePos.x + padding;
        var y = mousePos.y + padding;
        
        // Keep within screen bounds
        if (x + size.x > Screen.width)
            x = mousePos.x - size.x - padding;
        if (y + size.y > Screen.height)
            y = mousePos.y - size.y - padding;
        
        var tooltipRect = new Rect(x, y, size.x + padding * 2, size.y + padding);
        
        // Background
        RenderHelpers.DrawRoundedRect(tooltipRect, theme.Popover, Mathf.RoundToInt(DesignTokens.Radius.MD * UIScale));
        RenderHelpers.DrawBorder(tooltipRect, theme.Border, 1, Mathf.RoundToInt(DesignTokens.Radius.MD * UIScale));
        
        // Shadow effect
        var shadowRect = new Rect(tooltipRect.x + 4, tooltipRect.y + 4, tooltipRect.width, tooltipRect.height);
        RenderHelpers.DrawRoundedRect(shadowRect, new Color(0, 0, 0, 0.2f), Mathf.RoundToInt(DesignTokens.Radius.MD * UIScale));
        
        // Text
        GUI.Label(new Rect(tooltipRect.x + padding, tooltipRect.y + padding/2, size.x, size.y), _activeTooltip, style);
    }

    #region Private Implementation

    private static void HandleTooltipHover(Rect rect, string tooltip)
    {
        var mousePos = Event.current.mousePosition;
        var isHovering = rect.Contains(mousePos);
        
        if (isHovering)
        {
            _tooltipTimer += Time.deltaTime;
            if (_tooltipTimer >= TOOLTIP_DELAY)
            {
                _activeTooltip = tooltip;
            }
        }
        else if (_activeTooltip == tooltip)
        {
            _activeTooltip = null;
            _tooltipTimer = 0f;
        }
    }

    /// <summary>
    /// Call this when mouse leaves an area to clear tooltip.
    /// </summary>
    public static void ClearTooltip()
    {
        _activeTooltip = null;
        _tooltipTimer = 0f;
    }

    #endregion
}

#endregion
