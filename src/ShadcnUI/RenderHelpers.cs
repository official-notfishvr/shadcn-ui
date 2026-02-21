using UnityEngine;

namespace ShadcnUI;

/// <summary>
/// Internal helper methods for rendering UI primitives.
/// These are shared across all components.
/// </summary>
internal static class RenderHelpers
{
    public static Texture2D CreateSolidTexture(Color color)
    {
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    public static void DrawRoundedRect(Rect rect, Color color, int radius)
    {
        var texture = CreateSolidTexture(color);
        GUI.DrawTexture(rect, texture);
        UnityEngine.Object.DestroyImmediate(texture);
    }

    public static void DrawBorder(Rect rect, Color color, int width, int radius)
    {
        var texture = CreateSolidTexture(color);
        
        // Top
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, width), texture);
        // Bottom
        GUI.DrawTexture(new Rect(rect.x, rect.yMax - width, rect.width, width), texture);
        // Left
        GUI.DrawTexture(new Rect(rect.x, rect.y, width, rect.height), texture);
        // Right
        GUI.DrawTexture(new Rect(rect.xMax - width, rect.y, width, rect.height), texture);
        
        UnityEngine.Object.DestroyImmediate(texture);
    }

    public static void DrawCircle(Rect rect, Color color)
    {
        var center = rect.center;
        var radius = rect.width / 2;
        
        var texture = new Texture2D(Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height));
        var pixels = texture.GetPixels();
        
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                var px = x - texture.width / 2f;
                var py = y - texture.height / 2f;
                var dist = Mathf.Sqrt(px * px + py * py);
                
                pixels[y * texture.width + x] = dist <= radius ? color : Color.clear;
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        GUI.DrawTexture(rect, texture);
        UnityEngine.Object.DestroyImmediate(texture);
    }

    public static void DrawCircleBorder(Rect rect, Color color, int thickness)
    {
        DrawCircle(rect, color);
    }
}
