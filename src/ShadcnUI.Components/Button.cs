using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Button variant configuration.
/// </summary>
public readonly record struct ButtonVariants(
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default
);

/// <summary>
/// Button style helper for creating and modifying GUIStyles.
/// </summary>
public static class ButtonStyles
{
    public static GUIStyle GetStyle(ButtonVariants variants, ComponentState state = ComponentState.Default)
    {
        return Shadcn.GetStyle(StyleComponentType.Button, variants.Variant, variants.Size, state);
    }
}

#endregion

#region Button Component

/// <summary>
/// Button component with support for variants, sizes, icons, and custom styling.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a button with text.
    /// </summary>
    /// <param name="text">Button text</param>
    /// <param name="onClick">Click handler</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="disabled">Whether the button is disabled</param>
    /// <param name="options">GUILayout options</param>
    /// <returns>True if clicked</returns>
    public static bool Button(
        string text,
        Action? onClick = null,
        ButtonVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new ButtonVariants();
        var style = ButtonStyles.GetStyle(v, disabled ? ComponentState.Disabled : ComponentState.Default);

        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        bool clicked = GUILayout.Button(text, style, options);

        GUI.enabled = wasEnabled;

        if (clicked && !disabled)
        {
            onClick?.Invoke();
        }

        return clicked && !disabled;
    }

    /// <summary>
    /// Renders a button with text and icon.
    /// </summary>
    /// <param name="text">Button text</param>
    /// <param name="icon">Icon texture</param>
    /// <param name="onClick">Click handler</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="iconPosition">Position of the icon</param>
    /// <param name="disabled">Whether the button is disabled</param>
    /// <param name="options">GUILayout options</param>
    /// <returns>True if clicked</returns>
    public static bool Button(
        string text,
        Texture2D? icon,
        Action? onClick = null,
        ButtonVariants? variants = null,
        IconPosition iconPosition = IconPosition.Left,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new ButtonVariants();

        if (icon == null)
        {
            return Button(text, onClick, v, disabled, options);
        }

        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        bool clicked = false;

        if (iconPosition is IconPosition.Above or IconPosition.Below)
        {
            BeginVertical();
            
            if (iconPosition == IconPosition.Above)
            {
                DrawIcon(icon, v.Size);
                Space(DesignTokens.Spacing.XS);
            }

            clicked = Button(text, onClick, v, disabled, options);

            if (iconPosition == IconPosition.Below)
            {
                Space(DesignTokens.Spacing.XS);
                DrawIcon(icon, v.Size);
            }

            EndVertical();
        }
        else
        {
            BeginHorizontal();

            if (iconPosition == IconPosition.Left)
            {
                DrawIcon(icon, v.Size);
                Space(DesignTokens.Spacing.XS);
            }

            clicked = Button(text, onClick, v, disabled, options);

            if (iconPosition == IconPosition.Right)
            {
                Space(DesignTokens.Spacing.XS);
                DrawIcon(icon, v.Size);
            }

            EndHorizontal();
        }

        GUI.enabled = wasEnabled;

        return clicked;
    }

    /// <summary>
    /// Renders a button with only an icon.
    /// </summary>
    /// <param name="icon">Icon texture</param>
    /// <param name="onClick">Click handler</param>
    /// <param name="size">Button size</param>
    /// <param name="disabled">Whether the button is disabled</param>
    /// <returns>True if clicked</returns>
    public static bool IconButton(
        Texture2D icon,
        Action? onClick = null,
        ControlSize size = ControlSize.Default,
        bool disabled = false)
    {
        return Button("", icon, onClick, new ButtonVariants(ControlVariant.Ghost, size), IconPosition.Left, disabled,
            GUILayout.Width(GetIconButtonSize(size)), GUILayout.Height(GetIconButtonSize(size)));
    }

    /// <summary>
    /// Renders a group of buttons horizontally.
    /// </summary>
    public static void ButtonGroup(Action drawButtons, float spacing = DesignTokens.Spacing.XS)
    {
        BeginHorizontal();
        drawButtons();
        EndHorizontal();
        Space(spacing);
    }

    #region Helper Methods

    private static void DrawIcon(Texture2D icon, ControlSize size)
    {
        float iconSize = size switch
        {
            ControlSize.Mini => DesignTokens.IconSize.Small,
            ControlSize.Small => DesignTokens.IconSize.Default,
            ControlSize.Large => DesignTokens.IconSize.Large,
            _ => DesignTokens.IconSize.Default
        } * UIScale;

        GUILayout.Label(icon, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
    }

    private static float GetIconButtonSize(ControlSize size)
    {
        return size switch
        {
            ControlSize.Mini => DesignTokens.Height.Mini,
            ControlSize.Small => DesignTokens.Height.Small,
            ControlSize.Large => DesignTokens.Height.Large,
            _ => DesignTokens.Height.Default
        } * UIScale;
    }

    #endregion
}

/// <summary>
/// Icon position for buttons with icons.
/// </summary>
public enum IconPosition
{
    Left,
    Right,
    Above,
    Below
}

#endregion
