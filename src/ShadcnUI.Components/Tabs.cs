using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Tabs variant configuration.
/// </summary>
public readonly record struct TabsVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Tabs style helper.
/// </summary>
public static class TabsStyles
{
    public static GUIStyle GetListStyle(TabsVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.TabsList, variants.Variant);
    }

    public static GUIStyle GetTriggerStyle(TabsVariants variants, bool isSelected)
    {
        var state = isSelected ? ComponentState.Selected : ComponentState.Default;
        return Shadcn.GetStyle(StyleComponentType.TabsTrigger, variants.Variant, ControlSize.Default, state);
    }

    public static GUIStyle GetContentStyle(TabsVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.TabsContent, variants.Variant);
    }
}

#endregion

#region Tabs Component

/// <summary>
/// Tabs component for organizing content into tabbed sections.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a tabbed interface.
    /// </summary>
    /// <param name="tabNames">Array of tab names</param>
    /// <param name="selectedIndex">Currently selected tab index</param>
    /// <param name="content">Content renderer for each tab</param>
    /// <param name="variants">Visual variants</param>
    public static void Tabs(
        string[] tabNames,
        int selectedIndex,
        Action<int> content,
        TabsVariants? variants = null)
    {
        var v = variants ?? new TabsVariants();
        
        // Tab list
        TabsList(() =>
        {
            for (int i = 0; i < tabNames.Length; i++)
            {
                var isSelected = i == selectedIndex;
                var tabName = tabNames[i];
                
                if (TabTrigger(tabName, isSelected, v))
                {
                    // Tab clicked - caller should update selectedIndex
                }
                
                if (i < tabNames.Length - 1)
                {
                    Space(DesignTokens.Spacing.XS);
                }
            }
        }, v);
        
        Space(DesignTokens.Spacing.MD);
        
        // Tab content
        TabsContent(() =>
        {
            content?.Invoke(selectedIndex);
        }, v);
    }

    /// <summary>
    /// Renders tabs with automatic state management.
    /// </summary>
    public static void Tabs(
        string[] tabNames,
        ref int selectedIndex,
        Action<int> content,
        TabsVariants? variants = null,
        Action<int>? onSelectionChanged = null)
    {
        var v = variants ?? new TabsVariants();
        var currentIndex = selectedIndex;
        
        TabsList(() =>
        {
            for (int i = 0; i < tabNames.Length; i++)
            {
                var isSelected = i == currentIndex;
                
                if (TabTrigger(tabNames[i], isSelected, v))
                {
                    if (i != currentIndex)
                    {
                        currentIndex = i;
                        onSelectionChanged?.Invoke(i);
                    }
                }
                
                if (i < tabNames.Length - 1)
                {
                    Space(DesignTokens.Spacing.XS);
                }
            }
        }, v);
        
        selectedIndex = currentIndex;
        
        Space(DesignTokens.Spacing.MD);
        
        TabsContent(() =>
        {
            content?.Invoke(currentIndex);
        }, v);
    }

    #region Tab Parts

    /// <summary>
    /// Renders the tabs list container.
    /// </summary>
    public static void TabsList(Action triggers, TabsVariants? variants = null)
    {
        var v = variants ?? new TabsVariants();
        var theme = CurrentTheme;
        
        // Background container for tabs
        var padding = Mathf.RoundToInt(DesignTokens.Spacing.SM * UIScale);
        var radius = Mathf.RoundToInt(DesignTokens.Radius.MD * UIScale);
        
        BeginHorizontal(GUILayout.ExpandWidth(true));
        
        // Draw tabs background after something is in the group
        FlexibleSpace();
        var bgRect = GUILayoutUtility.GetLastRect();
        bgRect.height = DesignTokens.Height.Default * UIScale + padding * 2;
        
        GUILayout.BeginHorizontal(GUI.skin.box);
        triggers?.Invoke();
        GUILayout.EndHorizontal();
        
        EndHorizontal();
    }

    /// <summary>
    /// Renders a single tab trigger button.
    /// </summary>
    /// <param name="label">Tab label</param>
    /// <param name="isSelected">Whether this tab is selected</param>
    /// <param name="variants">Visual variants</param>
    /// <returns>True if clicked</returns>
    public static bool TabTrigger(string label, bool isSelected, TabsVariants? variants = null)
    {
        var v = variants ?? new TabsVariants();
        var theme = CurrentTheme;
        
        var style = new GUIStyle(GUI.skin.button);
        var padding = Mathf.RoundToInt(DesignTokens.Spacing.MD * UIScale);
        var radius = Mathf.RoundToInt(DesignTokens.Radius.SM * UIScale);
        
        if (isSelected)
        {
            style.normal.background = RenderHelpers.CreateSolidTexture(theme.Background);
            style.normal.textColor = theme.Foreground;
            style.fontStyle = FontStyle.Bold;
        }
        else
        {
            style.normal.background = RenderHelpers.CreateSolidTexture(Color.clear);
            style.normal.textColor = theme.MutedForeground;
            style.fontStyle = FontStyle.Normal;
        }
        
        style.hover.background = RenderHelpers.CreateSolidTexture(isSelected ? theme.Background : theme.Accent);
        style.hover.textColor = isSelected ? theme.Foreground : theme.AccentForeground;
        style.padding = new RectOffset(padding, padding, padding / 2, padding / 2);
        style.fontSize = Mathf.RoundToInt(DesignTokens.FontSize.SM * UIScale);
        style.border = new RectOffset(radius, radius, radius, radius);
        
        return GUILayout.Button(label, style, GUILayout.ExpandWidth(false));
    }

    /// <summary>
    /// Renders the content area for the active tab.
    /// </summary>
    public static void TabsContent(Action content, TabsVariants? variants = null)
    {
        var v = variants ?? new TabsVariants();
        
        GUILayout.BeginVertical();
        content?.Invoke();
        GUILayout.EndVertical();
    }

    #endregion

    #region Helper Methods

    #endregion
}

#endregion
