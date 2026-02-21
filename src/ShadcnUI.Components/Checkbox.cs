using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Checkbox variant configuration.
/// </summary>
public readonly record struct CheckboxVariants(
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default
);

/// <summary>
/// Checkbox style helper.
/// </summary>
public static class CheckboxStyles
{
    public static GUIStyle GetStyle(CheckboxVariants variants, bool isChecked, ComponentState state = ComponentState.Default)
    {
        var componentState = isChecked ? ComponentState.Checked : state;
        return Shadcn.GetStyle(StyleComponentType.Checkbox, variants.Variant, variants.Size, componentState);
    }
}

#endregion

#region Checkbox Component

/// <summary>
/// Checkbox component for boolean selection.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a checkbox with label.
    /// </summary>
    /// <param name="label">Label text displayed next to checkbox</param>
    /// <param name="isChecked">Current checked state</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="disabled">Whether the checkbox is disabled</param>
    /// <param name="options">GUILayout options</param>
    /// <returns>New checked state</returns>
    public static bool Checkbox(
        string label,
        bool isChecked,
        CheckboxVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new CheckboxVariants();
        
        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        BeginHorizontal();
        
        // Draw custom checkbox
        var checkboxSize = GetCheckboxSize(v.Size);
        var checkboxRect = GUILayoutUtility.GetRect(checkboxSize, checkboxSize, GUILayout.Width(checkboxSize), GUILayout.Height(checkboxSize));
        
        DrawCheckboxBox(checkboxRect, isChecked, v, disabled);
        
        // Label
        if (!string.IsNullOrEmpty(label))
        {
            Space(DesignTokens.Spacing.SM);
            var labelStyle = new GUIStyle(LabelStyles.GetStyle(new LabelVariants(v.Variant)))
            {
                fontSize = Mathf.RoundToInt(GetFontSizeForSize(v.Size) * UIScale)
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
        if (Event.current.type == EventType.MouseDown && checkboxRect.Contains(Event.current.mousePosition) && !disabled)
        {
            Event.current.Use();
            return !isChecked;
        }

        return isChecked;
    }

    /// <summary>
    /// Renders a checkbox with change callback.
    /// </summary>
    public static void Checkbox(
        string label,
        ref bool isChecked,
        Action<bool>? onChange = null,
        CheckboxVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var newValue = Checkbox(label, isChecked, variants, disabled, options);
        if (newValue != isChecked)
        {
            isChecked = newValue;
            onChange?.Invoke(newValue);
        }
    }

    /// <summary>
    /// Renders a checkbox without label.
    /// </summary>
    public static bool Checkbox(
        bool isChecked,
        CheckboxVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        return Checkbox("", isChecked, variants, disabled, options);
    }

    #region Helper Methods

    private static void DrawCheckboxBox(Rect rect, bool isChecked, CheckboxVariants variants, bool disabled)
    {
        var theme = CurrentTheme;
        var size = variants.Size;
        var radius = size == ControlSize.Small ? 2 : 4;
        
        // Background
        var bgColor = disabled ? theme.Muted : 
            isChecked ? theme.Primary : theme.Input;
        
        RenderHelpers.DrawRoundedRect(rect, bgColor, radius);
        
        // Border
        var borderColor = disabled ? theme.Border : 
            isChecked ? theme.Primary : theme.Border;
        RenderHelpers.DrawBorder(rect, borderColor, 1, radius);
        
        // Checkmark
        if (isChecked)
        {
            var checkColor = theme.PrimaryForeground;
            DrawCheckmark(rect, checkColor);
        }
    }

    private static void DrawCheckmark(Rect rect, Color color)
    {
        var checkSize = rect.width * 0.6f;
        var center = rect.center;
        
        // Simple checkmark using lines
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        
        // Draw small checkmark texture centered
        var checkRect = new Rect(center.x - checkSize/2, center.y - checkSize/2, checkSize, checkSize);
        GUI.DrawTexture(checkRect, texture);
        
        UnityEngine.Object.DestroyImmediate(texture);
    }

    private static float GetCheckboxSize(ControlSize size)
    {
        return size switch
        {
            ControlSize.Mini => 14f,
            ControlSize.Small => 16f,
            ControlSize.Large => 24f,
            _ => 20f
        } * UIScale;
    }

    private static float GetFontSizeForSize(ControlSize size)
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
