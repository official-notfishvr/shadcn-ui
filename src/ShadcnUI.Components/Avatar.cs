using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Avatar shape options.
/// </summary>
public enum AvatarShape
{
    Circle,
    Square,
    Rounded
}

/// <summary>
/// Avatar variant configuration.
/// </summary>
public readonly record struct AvatarVariants(
    ControlSize Size = ControlSize.Default,
    AvatarShape Shape = AvatarShape.Circle
);

/// <summary>
/// Avatar style helper.
/// </summary>
public static class AvatarStyles
{
    public static GUIStyle GetStyle(AvatarVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Avatar, ControlVariant.Default, variants.Size);
    }
}

#endregion

#region Avatar Component

/// <summary>
/// Avatar component for displaying user images with fallback to initials.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders an avatar with an image.
    /// </summary>
    /// <param name="image">Avatar image texture</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="options">GUILayout options</param>
    public static void Avatar(
        Texture2D image,
        AvatarVariants? variants = null,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new AvatarVariants();
        var size = GetAvatarSize(v.Size);
        
        var avatarRect = GUILayoutUtility.GetRect(size, size, options);
        
        DrawAvatarBackground(avatarRect, v);
        
        if (image != null)
        {
            GUI.DrawTexture(avatarRect, image, ScaleMode.ScaleAndCrop, true, 1f);
        }
        
        DrawAvatarBorder(avatarRect, v);
    }

    /// <summary>
    /// Renders an avatar with fallback text (initials).
    /// </summary>
    /// <param name="fallbackText">Text to display when no image (e.g., initials)</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="options">GUILayout options</param>
    public static void Avatar(
        string fallbackText,
        AvatarVariants? variants = null,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new AvatarVariants();
        var size = GetAvatarSize(v.Size);
        var theme = CurrentTheme;
        
        var avatarRect = GUILayoutUtility.GetRect(size, size, options);
        
        DrawAvatarBackground(avatarRect, v);
        
        // Draw initials
        var initials = GetInitials(fallbackText);
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(GetAvatarFontSize(v.Size) * UIScale),
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = theme.MutedForeground }
        };
        
        GUI.Label(avatarRect, initials, style);
        
        DrawAvatarBorder(avatarRect, v);
    }

    /// <summary>
    /// Renders an avatar with both image and fallback.
    /// </summary>
    public static void Avatar(
        Texture2D? image,
        string fallbackText,
        AvatarVariants? variants = null,
        params GUILayoutOption[] options)
    {
        if (image != null)
        {
            Avatar(image, variants, options);
        }
        else
        {
            Avatar(fallbackText, variants, options);
        }
    }

    /// <summary>
    /// Renders an avatar with online status indicator.
    /// </summary>
    public static void AvatarWithStatus(
        Texture2D? image,
        string fallbackText,
        bool isOnline,
        AvatarVariants? variants = null)
    {
        var v = variants ?? new AvatarVariants();
        var theme = CurrentTheme;
        
        BeginVertical(GUILayout.ExpandWidth(false));
        
        // Avatar
        Avatar(image, fallbackText, v);
        
        // Status indicator
        var size = GetAvatarSize(v.Size);
        var statusSize = size * 0.25f;
        var statusRect = GUILayoutUtility.GetLastRect();
        statusRect.x = statusRect.xMax - statusSize;
        statusRect.y = statusRect.yMax - statusSize;
        statusRect.width = statusSize;
        statusRect.height = statusSize;
        
        var statusColor = isOnline ? theme.Success : theme.MutedForeground;
        RenderHelpers.DrawCircle(statusRect, statusColor);
        RenderHelpers.DrawCircleBorder(statusRect, theme.Card, Mathf.RoundToInt(statusSize * 0.15f));
        
        EndVertical();
    }

    /// <summary>
    /// Renders an avatar group (overlapping avatars).
    /// </summary>
    public static void AvatarGroup(
        (Texture2D? image, string fallback)[] avatars,
        int maxDisplay = 3,
        AvatarVariants? variants = null)
    {
        var v = variants ?? new AvatarVariants();
        var size = GetAvatarSize(v.Size);
        var overlap = size * 0.3f;
        
        BeginHorizontal();
        
        var displayCount = Math.Min(avatars.Length, maxDisplay);
        for (int i = 0; i < displayCount; i++)
        {
            var (image, fallback) = avatars[i];
            
            // Use negative margin for overlap effect
            if (i > 0)
            {
                GUILayout.Space(-overlap);
            }
            
            Avatar(image, fallback, v);
        }
        
        // Show +N for remaining
        if (avatars.Length > maxDisplay)
        {
            GUILayout.Space(-overlap);
            
            var remaining = avatars.Length - maxDisplay;
            var text = $"+{remaining}";
            
            // Render a "+N" badge-style avatar
            var groupSize = size;
            var theme = CurrentTheme;
            
            var rect = GUILayoutUtility.GetRect(groupSize, groupSize);
            
            RenderHelpers.DrawCircle(rect, theme.Muted);
            
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(GetAvatarFontSize(v.Size) * 0.8f * UIScale),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = theme.MutedForeground }
            };
            
            GUI.Label(rect, text, style);
        }
        
        EndHorizontal();
    }

    #region Helper Methods

    private static float GetAvatarSize(ControlSize size)
    {
        return size switch
        {
            ControlSize.Small => 32f,
            ControlSize.Large => 48f,
            ControlSize.Mini => 24f,
            _ => 40f
        } * UIScale;
    }

    private static float GetAvatarFontSize(ControlSize size)
    {
        return size switch
        {
            ControlSize.Small => 12f,
            ControlSize.Large => 18f,
            ControlSize.Mini => 10f,
            _ => 16f
        };
    }

    private static string GetInitials(string text)
    {
        if (string.IsNullOrEmpty(text)) return "?";
        
        var parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return "?";
        
        if (parts.Length == 1)
        {
            return parts[0][..Math.Min(2, parts[0].Length)].ToUpper();
        }
        
        return (parts[0][0].ToString() + parts[^1][0]).ToUpper();
    }

    private static void DrawAvatarBackground(Rect rect, AvatarVariants variants)
    {
        var theme = CurrentTheme;
        var radius = GetAvatarRadius(variants.Shape, rect.width);
        
        RenderHelpers.DrawRoundedRect(rect, theme.Muted, radius);
    }

    private static void DrawAvatarBorder(Rect rect, AvatarVariants variants)
    {
        var theme = CurrentTheme;
        var radius = GetAvatarRadius(variants.Shape, rect.width);
        
        RenderHelpers.DrawBorder(rect, theme.Border, 2, radius);
    }

    private static int GetAvatarRadius(AvatarShape shape, float size)
    {
        return shape switch
        {
            AvatarShape.Circle => Mathf.RoundToInt(size / 2),
            AvatarShape.Rounded => Mathf.RoundToInt(size * 0.2f),
            _ => 0
        };
    }

    #endregion
}

#endregion
