using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Input variant configuration.
/// </summary>
public readonly record struct InputVariants(
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default
);

/// <summary>
/// Input type options.
/// </summary>
public enum InputType
{
    Text,
    Password,
    Multiline
}

/// <summary>
/// Input style helper.
/// </summary>
public static class InputStyles
{
    public static GUIStyle GetStyle(InputVariants variants, ComponentState state = ComponentState.Default)
    {
        return Shadcn.GetStyle(StyleComponentType.Input, variants.Variant, variants.Size, state);
    }
}

#endregion

#region Input Component

/// <summary>
/// Input component for text entry with support for password and multiline modes.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a single-line text input.
    /// </summary>
    /// <param name="label">Optional label above the input</param>
    /// <param name="value">Current value (will be updated)</param>
    /// <param name="placeholder">Placeholder text when empty</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="disabled">Whether the input is disabled</param>
    /// <param name="options">GUILayout options</param>
    /// <returns>The new value</returns>
    public static string Input(
        string label,
        string value,
        string? placeholder = null,
        InputVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        return InputInternal(label, value, placeholder, InputType.Text, '*', variants, disabled, options);
    }

    /// <summary>
    /// Renders a text input with change callback.
    /// </summary>
    public static string Input(
        string value,
        Action<string>? onChange = null,
        string? placeholder = null,
        InputVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var newValue = InputInternal(null, value, placeholder, InputType.Text, '*', variants, disabled, options);
        if (newValue != value)
        {
            onChange?.Invoke(newValue);
        }
        return newValue;
    }

    /// <summary>
    /// Renders a password input.
    /// </summary>
    public static string Password(
        string label,
        string value,
        char maskChar = '*',
        InputVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        return InputInternal(label, value, null, InputType.Password, maskChar, variants, disabled, options);
    }

    /// <summary>
    /// Renders a multiline text area.
    /// </summary>
    public static string TextArea(
        string label,
        string value,
        string? placeholder = null,
        float height = 60f,
        InputVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var allOptions = new List<GUILayoutOption>(options);
        allOptions.Add(GUILayout.Height(height * UIScale));

        return InputInternal(label, value, placeholder, InputType.Multiline, '*', variants, disabled, allOptions.ToArray());
    }

    #region Private Implementation

    private static string InputInternal(
        string? label,
        string value,
        string? placeholder,
        InputType type,
        char maskChar,
        InputVariants? variants,
        bool disabled,
        GUILayoutOption[] options)
    {
        var v = variants ?? new InputVariants();

        // Draw label if provided
        if (!string.IsNullOrEmpty(label))
        {
            Label(label, ControlVariant.Default);
            Space(DesignTokens.Spacing.XS);
        }

        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        string newValue;

        switch (type)
        {
            case InputType.Password:
                newValue = GUILayout.PasswordField(value, maskChar, InputStyles.GetStyle(v), options);
                break;
            case InputType.Multiline:
                newValue = GUILayout.TextArea(value, InputStyles.GetStyle(v), options);
                break;
            default:
                newValue = GUILayout.TextField(value, InputStyles.GetStyle(v), options);
                break;
        }

        GUI.enabled = wasEnabled;

        // Show placeholder if empty and not focused
        if (string.IsNullOrEmpty(newValue) && !string.IsNullOrEmpty(placeholder))
        {
            var lastRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && !lastRect.Contains(Event.current.mousePosition))
            {
                var placeholderStyle = new GUIStyle(InputStyles.GetStyle(v))
                {
                    normal = { textColor = CurrentTheme.MutedForeground }
                };
                GUI.Label(lastRect, placeholder, placeholderStyle);
            }
        }

        return newValue;
    }

    #endregion
}

#endregion
