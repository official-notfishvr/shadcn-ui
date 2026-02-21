using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Select variant configuration.
/// </summary>
public readonly record struct SelectVariants(
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default
);

/// <summary>
/// Select style helper.
/// </summary>
public static class SelectStyles
{
    public static GUIStyle GetTriggerStyle(SelectVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Select, variants.Variant, variants.Size);
    }

    public static GUIStyle GetItemStyle(SelectVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.SelectItem, variants.Variant, variants.Size);
    }
}

#endregion

#region Select Component

/// <summary>
/// Select component (dropdown) for selecting from a list of options.
/// </summary>
public static partial class Shadcn
{
    // Static state for dropdowns
    private static string? _openDropdownId;
    private static Vector2 _dropdownScrollPosition;

    /// <summary>
    /// Renders a select dropdown with label.
    /// </summary>
    /// <param name="label">Label text</param>
    /// <param name="options">Array of options</param>
    /// <param name="selectedIndex">Currently selected index</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="disabled">Whether disabled</param>
    /// <param name="layoutOptions">GUILayout options</param>
    /// <returns>New selected index</returns>
    public static int Select(
        string label,
        string[] options,
        int selectedIndex,
        SelectVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] layoutOptions)
    {
        var v = variants ?? new SelectVariants();
        var id = GUIUtility.GetControlID(FocusType.Passive);
        var uniqueId = $"select_{id}";

        // Label
        if (!string.IsNullOrEmpty(label))
        {
            Label(label, ControlVariant.Default);
            Space(DesignTokens.Spacing.XS);
        }

        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        // Get display text
        var displayText = selectedIndex >= 0 && selectedIndex < options.Length 
            ? options[selectedIndex] 
            : "Select...";

        // Trigger button
        var triggerStyle = SelectStyles.GetTriggerStyle(v);
        var clicked = GUILayout.Button(displayText + " ▼", triggerStyle, layoutOptions);

        if (clicked && !disabled)
        {
            _openDropdownId = _openDropdownId == uniqueId ? null : uniqueId;
        }

        // Dropdown content
        if (_openDropdownId == uniqueId && !disabled)
        {
            // Get rect from the button that was just drawn
            var lastRect = GUILayoutUtility.GetLastRect();
            DrawDropdown(uniqueId, options, selectedIndex, (newIndex) =>
            {
                selectedIndex = newIndex;
                _openDropdownId = null;
            }, v, lastRect);
        }

        GUI.enabled = wasEnabled;

        return selectedIndex;
    }

    /// <summary>
    /// Renders a select with change callback.
    /// </summary>
    public static void Select(
        string label,
        string[] options,
        ref int selectedIndex,
        Action<int>? onChange = null,
        SelectVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] layoutOptions)
    {
        var newIndex = Select(label, options, selectedIndex, variants, disabled, layoutOptions);
        if (newIndex != selectedIndex)
        {
            selectedIndex = newIndex;
            onChange?.Invoke(newIndex);
        }
    }

    /// <summary>
    /// Renders a select without label.
    /// </summary>
    public static int Select(
        string[] options,
        int selectedIndex,
        SelectVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] layoutOptions)
    {
        return Select("", options, selectedIndex, variants, disabled, layoutOptions);
    }

    #region Private Implementation

    private static void DrawDropdown(string id, string[] options, int selectedIndex, 
        Action<int> onSelect, SelectVariants variants, Rect lastRect)
    {
        var dropdownWidth = lastRect.width;
        var itemHeight = GetSelectItemHeight(variants.Size);
        var maxVisibleItems = Mathf.Min(options.Length, 8);
        var dropdownHeight = maxVisibleItems * itemHeight;

        var dropdownRect = new Rect(
            lastRect.x, 
            lastRect.yMax + 2, 
            dropdownWidth, 
            dropdownHeight
        );

        // Close on click outside
        if (Event.current.type == EventType.MouseDown && !dropdownRect.Contains(Event.current.mousePosition))
        {
            _openDropdownId = null;
            Event.current.Use();
            return;
        }

        var theme = CurrentTheme;
        var bgStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = RenderHelpers.CreateSolidTexture(theme.Popover) }
        };

        GUI.Box(dropdownRect, GUIContent.none, bgStyle);

        // Scrollable area
        var contentHeight = options.Length * itemHeight;
        var viewRect = new Rect(0, 0, dropdownWidth - 20, contentHeight);
        
        _dropdownScrollPosition = GUI.BeginScrollView(
            dropdownRect, 
            _dropdownScrollPosition, 
            viewRect
        );

        var itemStyle = SelectStyles.GetItemStyle(variants);

        for (int i = 0; i < options.Length; i++)
        {
            var itemRect = new Rect(0, i * itemHeight, dropdownWidth - 20, itemHeight);
            var isSelected = i == selectedIndex;

            // Highlight on hover
            if (itemRect.Contains(Event.current.mousePosition))
            {
                GUI.DrawTexture(itemRect, RenderHelpers.CreateSolidTexture(theme.Accent));
            }
            else if (isSelected)
            {
                GUI.DrawTexture(itemRect, RenderHelpers.CreateSolidTexture(theme.Muted));
            }

            GUI.Label(itemRect, options[i], itemStyle);

            if (Event.current.type == EventType.MouseDown && itemRect.Contains(Event.current.mousePosition))
            {
                onSelect(i);
                Event.current.Use();
            }
        }

        GUI.EndScrollView();
    }

    private static float GetSelectItemHeight(ControlSize size)
    {
        return size switch
        {
            ControlSize.Small => 28f,
            ControlSize.Large => 40f,
            _ => 32f
        } * UIScale;
    }

    #endregion
}

#endregion
