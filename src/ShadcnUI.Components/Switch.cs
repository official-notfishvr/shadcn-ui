using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Switch variant configuration.
/// </summary>
public readonly record struct SwitchVariants(
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default
);

/// <summary>
/// Switch style helper.
/// </summary>
public static class SwitchStyles
{
    public static GUIStyle GetStyle(SwitchVariants variants, bool isOn)
    {
        var state = isOn ? ComponentState.Checked : ComponentState.Default;
        return Shadcn.GetStyle(StyleComponentType.Switch, variants.Variant, variants.Size, state);
    }
}

#endregion

#region Switch Component

/// <summary>
/// Switch component (toggle) for on/off states.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a switch with label.
    /// </summary>
    /// <param name="label">Label text</param>
    /// <param name="isOn">Current state</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="disabled">Whether disabled</param>
    /// <param name="options">GUILayout options</param>
    /// <returns>New state</returns>
    public static bool Switch(
        string label,
        bool isOn,
        SwitchVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new SwitchVariants();
        
        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        BeginHorizontal();
        
        // Draw switch track and thumb
        var (trackWidth, trackHeight, thumbSize) = GetSwitchDimensions(v.Size);
        var switchRect = GUILayoutUtility.GetRect(trackWidth, Mathf.Max(trackHeight, thumbSize), 
            GUILayout.Width(trackWidth), GUILayout.Height(Mathf.Max(trackHeight, thumbSize)));
        
        DrawSwitch(switchRect, isOn, v, disabled, trackWidth, trackHeight, thumbSize);
        
        // Label
        if (!string.IsNullOrEmpty(label))
        {
            Space(DesignTokens.Spacing.SM);
            var labelStyle = new GUIStyle(LabelStyles.GetStyle(new LabelVariants(v.Variant)))
            {
                fontSize = Mathf.RoundToInt(GetFontSizeForSwitchSize(v.Size) * UIScale)
            };
            
            if (disabled)
            {
                labelStyle.normal.textColor = CurrentTheme.MutedForeground;
            }
            
            GUILayout.Label(label, labelStyle);
        }
        
        EndHorizontal();

        GUI.enabled = wasEnabled;

        // Check for click
        if (Event.current.type == EventType.MouseDown && switchRect.Contains(Event.current.mousePosition) && !disabled)
        {
            Event.current.Use();
            return !isOn;
        }

        return isOn;
    }

    /// <summary>
    /// Renders a switch with change callback.
    /// </summary>
    public static void Switch(
        string label,
        ref bool isOn,
        Action<bool>? onChange = null,
        SwitchVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var newValue = Switch(label, isOn, variants, disabled, options);
        if (newValue != isOn)
        {
            isOn = newValue;
            onChange?.Invoke(newValue);
        }
    }

    /// <summary>
    /// Renders a switch without label.
    /// </summary>
    public static bool Switch(
        bool isOn,
        SwitchVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        return Switch("", isOn, variants, disabled, options);
    }

    #region Helper Methods

    private static void DrawSwitch(Rect rect, bool isOn, SwitchVariants variants, bool disabled,
        float trackWidth, float trackHeight, float thumbSize)
    {
        var theme = CurrentTheme;
        var trackRadius = trackHeight / 2;
        
        // Track position (centered vertically)
        var trackY = rect.y + (rect.height - trackHeight) / 2;
        var trackRect = new Rect(rect.x, trackY, trackWidth, trackHeight);
        
        // Track color
        var trackColor = disabled ? theme.Muted : 
            isOn ? theme.Primary : theme.Border;
        
        RenderHelpers.DrawRoundedRect(trackRect, trackColor, Mathf.RoundToInt(trackRadius));
        
        // Thumb
        var thumbPadding = 2f * UIScale;
        var thumbX = isOn ? 
            rect.x + trackWidth - thumbSize - thumbPadding : 
            rect.x + thumbPadding;
        var thumbY = trackY + (trackHeight - thumbSize) / 2;
        var thumbRect = new Rect(thumbX, thumbY, thumbSize, thumbSize);
        
        var thumbColor = disabled ? theme.MutedForeground : theme.Background;
        var thumbRadius = thumbSize / 2;
        
        RenderHelpers.DrawRoundedRect(thumbRect, thumbColor, Mathf.RoundToInt(thumbRadius));
        
        // Thumb shadow/border
        RenderHelpers.DrawBorder(thumbRect, theme.Border, 1, Mathf.RoundToInt(thumbRadius));
    }

    private static (float width, float height, float thumb) GetSwitchDimensions(ControlSize size)
    {
        return size switch
        {
            ControlSize.Mini => (32f, 16f, 12f),
            ControlSize.Small => (36f, 18f, 14f),
            ControlSize.Large => (52f, 28f, 24f),
            _ => (44f, 22f, 18f)
        };
    }

    private static float GetFontSizeForSwitchSize(ControlSize size)
    {
        return size switch
        {
            ControlSize.Mini => DesignTokens.FontSize.XS,
            ControlSize.Small => DesignTokens.FontSize.SM,
            ControlSize.Large => DesignTokens.FontSize.LG,
            _ => DesignTokens.FontSize.Default
        };
    }

    #endregion
}

#endregion
