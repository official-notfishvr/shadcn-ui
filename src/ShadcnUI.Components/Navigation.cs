using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Navigation variant configuration.
/// </summary>
public readonly record struct NavigationVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Navigation style helper.
/// </summary>
public static class NavigationStyles
{
    public static GUIStyle GetContainerStyle(NavigationVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Navigation, variants.Variant);
    }

    public static GUIStyle GetItemStyle(NavigationVariants variants, bool isSelected)
    {
        var state = isSelected ? ComponentState.Selected : ComponentState.Default;
        return Shadcn.GetStyle(StyleComponentType.Navigation, variants.Variant, ControlSize.Default, state);
    }
}

#endregion

#region Navigation Component

/// <summary>
/// Navigation component for sidebar or top navigation.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a vertical navigation menu.
    /// </summary>
    /// <param name="items">Navigation items</param>
    /// <param name="selectedIndex">Currently selected index</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="width">Navigation width</param>
    /// <param name="showLogo">Show logo area at top</param>
    /// <returns>New selected index</returns>
    public static int Navigation(
        NavItem[] items,
        int selectedIndex,
        NavigationVariants? variants = null,
        float width = 70f,
        bool showLogo = true)
    {
        var v = variants ?? new NavigationVariants();
        var theme = CurrentTheme;
        var navWidth = width * UIScale;
        
        // Navigation container
        BeginVertical(GUILayout.Width(navWidth), GUILayout.ExpandHeight(true));
        
        if (showLogo)
        {
            NavigationLogo();
        }
        
        // Navigation items
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var isSelected = i == selectedIndex;
            
            if (NavItem(item, isSelected, v))
            {
                if (i != selectedIndex)
                {
                    selectedIndex = i;
                    item.OnClick?.Invoke();
                }
            }
        }
        
        EndVertical();
        
        return selectedIndex;
    }

    /// <summary>
    /// Renders navigation with callback.
    /// </summary>
    public static void Navigation(
        NavItem[] items,
        ref int selectedIndex,
        Action<int>? onSelectionChanged = null,
        NavigationVariants? variants = null,
        float width = 70f,
        bool showLogo = true)
    {
        var newIndex = Navigation(items, selectedIndex, variants, width, showLogo);
        if (newIndex != selectedIndex)
        {
            selectedIndex = newIndex;
            onSelectionChanged?.Invoke(newIndex);
        }
    }

    /// <summary>
    /// Renders a horizontal navigation bar.
    /// </summary>
    public static void HorizontalNav(
        NavItem[] items,
        ref int selectedIndex,
        Action<int>? onSelectionChanged = null,
        NavigationVariants? variants = null)
    {
        var v = variants ?? new NavigationVariants();
        var theme = CurrentTheme;
        
        BeginHorizontal(GUILayout.ExpandWidth(true));
        
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var isSelected = i == selectedIndex;
            
            if (HorizontalNavItem(item, isSelected, v))
            {
                if (i != selectedIndex)
                {
                    selectedIndex = i;
                    onSelectionChanged?.Invoke(i);
                    item.OnClick?.Invoke();
                }
            }
            
            if (i < items.Length - 1)
            {
                Space(DesignTokens.Spacing.XS);
            }
        }
        
        EndHorizontal();
    }

    #region Navigation Parts

    /// <summary>
    /// Renders the navigation logo/brand area.
    /// </summary>
    public static void NavigationLogo(string? text = null, Texture2D? icon = null)
    {
        var theme = CurrentTheme;
        var size = 40f * UIScale;
        
        BeginHorizontal(GUILayout.Height(size));
        FlexibleSpace();
        
        if (icon != null)
        {
            GUILayout.Label(icon, GUILayout.Width(size), GUILayout.Height(size));
        }
        else if (!string.IsNullOrEmpty(text))
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(DesignTokens.FontSize.XL * UIScale),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = theme.Foreground }
            };
            
            GUILayout.Label(text, style, GUILayout.Width(size), GUILayout.Height(size));
        }
        else
        {
            // Default "U" logo
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(DesignTokens.FontSize.XL * UIScale),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = theme.Primary }
            };
            
            GUILayout.Label("U", style, GUILayout.Width(size), GUILayout.Height(size));
        }
        
        FlexibleSpace();
        EndHorizontal();
        
        Space(DesignTokens.Spacing.MD);
    }

    /// <summary>
    /// Renders a single vertical navigation item.
    /// </summary>
    public static bool NavItem(NavItem item, bool isSelected, NavigationVariants? variants = null)
    {
        var v = variants ?? new NavigationVariants();
        var theme = CurrentTheme;
        
        var height = 50f * UIScale;
        var style = new GUIStyle(GUI.skin.button);
        
        // Colors
        if (isSelected)
        {
            style.normal.background = RenderHelpers.CreateSolidTexture(theme.Primary);
            style.normal.textColor = theme.PrimaryForeground;
            style.fontStyle = FontStyle.Bold;
        }
        else
        {
            style.normal.background = RenderHelpers.CreateSolidTexture(Color.clear);
            style.normal.textColor = theme.MutedForeground;
            style.fontStyle = FontStyle.Normal;
        }
        
        style.hover.background = RenderHelpers.CreateSolidTexture(isSelected ? theme.Primary : theme.Muted);
        style.hover.textColor = isSelected ? theme.PrimaryForeground : theme.Foreground;
        style.fontSize = Mathf.RoundToInt(DesignTokens.FontSize.XS * UIScale);
        style.alignment = TextAnchor.MiddleCenter;
        style.padding = new RectOffset(0, 0, 4, 4);
        
        var content = !string.IsNullOrEmpty(item.Label) ? $"{item.Icon}\n{item.Label}" : item.Icon;
        
        return GUILayout.Button(content, style, GUILayout.ExpandWidth(true), GUILayout.Height(height));
    }

    /// <summary>
    /// Renders a single horizontal navigation item.
    /// </summary>
    public static bool HorizontalNavItem(NavItem item, bool isSelected, NavigationVariants? variants = null)
    {
        var v = variants ?? new NavigationVariants();
        var theme = CurrentTheme;
        
        var height = 40f * UIScale;
        var style = new GUIStyle(GUI.skin.button);
        
        if (isSelected)
        {
            style.normal.background = RenderHelpers.CreateSolidTexture(theme.Primary);
            style.normal.textColor = theme.PrimaryForeground;
            style.fontStyle = FontStyle.Bold;
        }
        else
        {
            style.normal.background = RenderHelpers.CreateSolidTexture(Color.clear);
            style.normal.textColor = theme.MutedForeground;
            style.fontStyle = FontStyle.Normal;
        }
        
        style.hover.background = RenderHelpers.CreateSolidTexture(isSelected ? theme.Primary : theme.Muted);
        style.hover.textColor = isSelected ? theme.PrimaryForeground : theme.Foreground;
        style.fontSize = Mathf.RoundToInt(DesignTokens.FontSize.SM * UIScale);
        style.padding = new RectOffset(
            Mathf.RoundToInt(DesignTokens.Spacing.MD * UIScale), 
            Mathf.RoundToInt(DesignTokens.Spacing.MD * UIScale), 
            0, 0);
        
        var content = !string.IsNullOrEmpty(item.Icon) ? $"{item.Icon} {item.Label}" : item.Label;
        
        return GUILayout.Button(content, style, GUILayout.Height(height));
    }

    #endregion

    #region Helper Methods

    #endregion
}

/// <summary>
/// Navigation item definition.
/// </summary>
public sealed class NavItem
{
    public string Id { get; }
    public string Label { get; }
    public string Icon { get; }
    public Action? OnClick { get; set; }
    public bool IsDisabled { get; set; }

    public NavItem(string id, string label, string icon = "")
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Icon = icon ?? "";
    }

    public NavItem(string id, string label, string icon, Action onClick) : this(id, label, icon)
    {
        OnClick = onClick;
    }
}

#endregion
