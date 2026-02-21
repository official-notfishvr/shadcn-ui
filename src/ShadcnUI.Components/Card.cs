using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Card variant configuration.
/// </summary>
public readonly record struct CardVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Card style helper.
/// </summary>
public static class CardStyles
{
    public static GUIStyle GetCardStyle(CardVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Card);
    }

    public static GUIStyle GetHeaderStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.CardHeader);
    }

    public static GUIStyle GetTitleStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.CardTitle);
    }

    public static GUIStyle GetDescriptionStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.CardDescription);
    }

    public static GUIStyle GetContentStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.CardContent);
    }

    public static GUIStyle GetFooterStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.CardFooter);
    }
}

#endregion

#region Card Component

/// <summary>
/// Card component for containing content with header, title, description, content, and footer sections.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a card container.
    /// </summary>
    /// <param name="content">Content to render inside the card</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="options">GUILayout options</param>
    public static void Card(
        Action content,
        CardVariants? variants = null,
        params GUILayoutOption[] options)
    {
        var style = CardStyles.GetCardStyle(variants ?? new CardVariants());
        
        GUILayout.BeginVertical(style, options);
        content?.Invoke();
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Renders a card header section.
    /// </summary>
    /// <param name="content">Header content</param>
    public static void CardHeader(Action content)
    {
        var style = CardStyles.GetHeaderStyle();
        
        GUILayout.BeginVertical(style);
        content?.Invoke();
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Renders a card header with title and optional description.
    /// </summary>
    /// <param name="title">Card title</param>
    /// <param name="description">Optional description</param>
    public static void CardHeader(string title, string? description = null)
    {
        CardHeader(() =>
        {
            CardTitle(title);
            if (!string.IsNullOrEmpty(description))
            {
                CardDescription(description);
            }
        });
    }

    /// <summary>
    /// Renders a card title.
    /// </summary>
    /// <param name="text">Title text</param>
    public static void CardTitle(string text)
    {
        var style = new GUIStyle(CardStyles.GetTitleStyle())
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.LG * UIScale),
            fontStyle = FontStyle.Bold
        };
        
        GUILayout.Label(text, style);
    }

    /// <summary>
    /// Renders card description text.
    /// </summary>
    /// <param name="text">Description text</param>
    public static void CardDescription(string text)
    {
        var style = new GUIStyle(CardStyles.GetDescriptionStyle())
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.SM * UIScale)
        };
        
        GUILayout.Label(text, style);
    }

    /// <summary>
    /// Renders card content section.
    /// </summary>
    /// <param name="content">Content to render</param>
    public static void CardContent(Action content)
    {
        var style = CardStyles.GetContentStyle();
        
        GUILayout.BeginVertical(style);
        content?.Invoke();
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Renders card footer section.
    /// </summary>
    /// <param name="content">Footer content</param>
    public static void CardFooter(Action content)
    {
        var style = CardStyles.GetFooterStyle();
        
        GUILayout.BeginHorizontal(style);
        content?.Invoke();
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Renders a complete card with all sections.
    /// </summary>
    public static void Card(
        string title,
        string? description,
        Action content,
        Action? footer = null,
        CardVariants? variants = null,
        params GUILayoutOption[] options)
    {
        Card(() =>
        {
            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(description))
            {
                CardHeader(title, description);
            }
            
            CardContent(content);
            
            if (footer != null)
            {
                CardFooter(footer);
            }
        }, variants, options);
    }
}

#endregion
