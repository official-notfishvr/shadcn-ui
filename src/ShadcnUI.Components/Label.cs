using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Label variant configuration.
/// </summary>
public readonly record struct LabelVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Label style helper.
/// </summary>
public static class LabelStyles
{
    public static GUIStyle GetStyle(LabelVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Label, variants.Variant);
    }
}

#endregion

#region Label Component

/// <summary>
/// Label component for displaying text and form labels.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a text label.
    /// </summary>
    /// <param name="text">Label text</param>
    /// <param name="variant">Visual variant</param>
    /// <param name="options">GUILayout options</param>
    public static void Label(
        string text,
        ControlVariant variant = ControlVariant.Default,
        params GUILayoutOption[] options)
    {
        var style = LabelStyles.GetStyle(new LabelVariants(variant));
        GUILayout.Label(text, style, options);
    }

    /// <summary>
    /// Renders a label with specific variants.
    /// </summary>
    public static void Label(
        string text,
        LabelVariants variants,
        params GUILayoutOption[] options)
    {
        var style = LabelStyles.GetStyle(variants);
        GUILayout.Label(text, style, options);
    }

    /// <summary>
    /// Renders a muted/disabled label.
    /// </summary>
    public static void MutedLabel(
        string text,
        params GUILayoutOption[] options)
    {
        Label(text, ControlVariant.Muted, options);
    }

    /// <summary>
    /// Renders a section header label.
    /// </summary>
    public static void SectionHeader(
        string text,
        params GUILayoutOption[] options)
    {
        Space(DesignTokens.Spacing.SM * 0.5f);
        
        var style = new GUIStyle(LabelStyles.GetStyle(new LabelVariants(ControlVariant.Default)))
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.LG * UIScale),
            fontStyle = FontStyle.Bold
        };
        
        GUILayout.Label(text, style, options);
        Space(DesignTokens.Spacing.XXS);
    }

    /// <summary>
    /// Renders a title label.
    /// </summary>
    public static void Title(
        string text,
        params GUILayoutOption[] options)
    {
        var style = new GUIStyle(LabelStyles.GetStyle(new LabelVariants(ControlVariant.Default)))
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.XXL * UIScale),
            fontStyle = FontStyle.Bold
        };
        
        GUILayout.Label(text, style, options);
    }

    /// <summary>
    /// Renders a subtitle label.
    /// </summary>
    public static void Subtitle(
        string text,
        params GUILayoutOption[] options)
    {
        Label(text, ControlVariant.Muted, options);
    }
}

#endregion
