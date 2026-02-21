using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Slider variant configuration.
/// </summary>
public readonly record struct SliderVariants(
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default
);

/// <summary>
/// Slider style helper.
/// </summary>
public static class SliderStyles
{
    public static GUIStyle GetTrackStyle(SliderVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.SliderTrack, variants.Variant, variants.Size);
    }

    public static GUIStyle GetThumbStyle(SliderVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.SliderThumb, variants.Variant, variants.Size);
    }

    public static GUIStyle GetFillStyle(SliderVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.SliderFill, variants.Variant, variants.Size);
    }
}

#endregion

#region Slider Component

/// <summary>
/// Slider component for selecting a numeric value from a range.
/// </summary>
public static partial class Shadcn
{
    /// <summary>
    /// Renders a horizontal slider with label.
    /// </summary>
    /// <param name="label">Label text</param>
    /// <param name="value">Current value</param>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="disabled">Whether disabled</param>
    /// <param name="showValue">Show the current value</param>
    /// <param name="options">GUILayout options</param>
    /// <returns>New value</returns>
    public static float Slider(
        string label,
        float value,
        float min,
        float max,
        SliderVariants? variants = null,
        bool disabled = false,
        bool showValue = true,
        params GUILayoutOption[] options)
    {
        var v = variants ?? new SliderVariants();

        // Label row
        if (!string.IsNullOrEmpty(label) || showValue)
        {
            BeginHorizontal();
            
            if (!string.IsNullOrEmpty(label))
            {
                Label(label);
            }
            
            if (showValue)
            {
                FlexibleSpace();
                Label(value.ToString("F2"), ControlVariant.Muted);
            }
            
            EndHorizontal();
            Space(DesignTokens.Spacing.XS);
        }

        return SliderInternal(value, min, max, v, disabled, options);
    }

    /// <summary>
    /// Renders a slider with change callback.
    /// </summary>
    public static void Slider(
        string label,
        ref float value,
        float min,
        float max,
        Action<float>? onChange = null,
        SliderVariants? variants = null,
        bool disabled = false,
        bool showValue = true,
        params GUILayoutOption[] options)
    {
        var newValue = Slider(label, value, min, max, variants, disabled, showValue, options);
        if (Mathf.Abs(newValue - value) > 0.0001f)
        {
            value = newValue;
            onChange?.Invoke(newValue);
        }
    }

    /// <summary>
    /// Renders an integer slider.
    /// </summary>
    public static int Slider(
        string label,
        int value,
        int min,
        int max,
        SliderVariants? variants = null,
        bool disabled = false,
        bool showValue = true,
        params GUILayoutOption[] options)
    {
        var floatValue = Slider(label, (float)value, min, max, variants, disabled, showValue, options);
        return Mathf.RoundToInt(floatValue);
    }

    /// <summary>
    /// Renders a slider without label.
    /// </summary>
    public static float Slider(
        float value,
        float min,
        float max,
        SliderVariants? variants = null,
        bool disabled = false,
        params GUILayoutOption[] options)
    {
        return SliderInternal(value, min, max, variants ?? new SliderVariants(), disabled, options);
    }

    #region Private Implementation

    private static float SliderInternal(float value, float min, float max, SliderVariants variants, 
        bool disabled, GUILayoutOption[] options)
    {
        var theme = CurrentTheme;
        var wasEnabled = GUI.enabled;
        GUI.enabled = !disabled;

        // Get dimensions
        var (trackHeight, thumbSize) = GetSliderDimensions(variants.Size);
        var sliderHeight = Mathf.Max(trackHeight, thumbSize);
        
        // Calculate rect
        var sliderRect = GUILayoutUtility.GetRect(100, sliderHeight, 
            GUILayout.ExpandWidth(true), GUILayout.Height(sliderHeight));

        // Track rect (centered vertically)
        var trackY = sliderRect.y + (sliderRect.height - trackHeight) / 2;
        var trackRect = new Rect(sliderRect.x, trackY, sliderRect.width, trackHeight);

        // Draw track background
        RenderHelpers.DrawRoundedRect(trackRect, theme.Input, Mathf.RoundToInt(trackHeight / 2));

        // Calculate value ratio
        var ratio = Mathf.InverseLerp(min, max, value);
        
        // Draw filled portion
        var fillWidth = trackRect.width * ratio;
        var fillRect = new Rect(trackRect.x, trackRect.y, fillWidth, trackRect.height);
        RenderHelpers.DrawRoundedRect(fillRect, theme.Primary, Mathf.RoundToInt(trackHeight / 2));

        // Draw thumb
        var thumbX = sliderRect.x + (sliderRect.width - thumbSize) * ratio;
        var thumbY = sliderRect.y + (sliderRect.height - thumbSize) / 2;
        var thumbRect = new Rect(thumbX, thumbY, thumbSize, thumbSize);
        
        var thumbColor = disabled ? theme.Muted : theme.Background;
        RenderHelpers.DrawCircle(thumbRect, thumbColor);
        RenderHelpers.DrawCircleBorder(thumbRect, theme.Border, 2);

        // Handle interaction
        if (!disabled)
        {
            HandleSliderInteraction(sliderRect, thumbRect, min, max, ref value, ref ratio);
        }

        GUI.enabled = wasEnabled;

        return value;
    }

    private static void HandleSliderInteraction(Rect trackRect, Rect thumbRect, float min, float max, 
        ref float value, ref float ratio)
    {
        var eventType = Event.current.type;
        var mousePos = Event.current.mousePosition;
        var controlId = GUIUtility.GetControlID(FocusType.Passive);

        switch (eventType)
        {
            case EventType.MouseDown:
                if (trackRect.Contains(mousePos) || thumbRect.Contains(mousePos))
                {
                    GUIUtility.hotControl = controlId;
                    UpdateSliderValue(trackRect, mousePos, min, max, ref value, ref ratio);
                    Event.current.Use();
                }
                break;

            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlId)
                {
                    UpdateSliderValue(trackRect, mousePos, min, max, ref value, ref ratio);
                    Event.current.Use();
                }
                break;

            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlId)
                {
                    GUIUtility.hotControl = 0;
                    Event.current.Use();
                }
                break;

            case EventType.Repaint:
                if (GUIUtility.hotControl == controlId)
                {
                    // Draw focus ring
                    var focusRect = new Rect(thumbRect.x - 4, thumbRect.y - 4, thumbRect.width + 8, thumbRect.height + 8);
                    RenderHelpers.DrawCircle(focusRect, new Color(CurrentTheme.Ring.r, CurrentTheme.Ring.g, CurrentTheme.Ring.b, 0.3f));
                }
                break;
        }
    }

    private static void UpdateSliderValue(Rect trackRect, Vector2 mousePos, float min, float max, 
        ref float value, ref float ratio)
    {
        ratio = Mathf.Clamp01((mousePos.x - trackRect.x) / trackRect.width);
        value = Mathf.Lerp(min, max, ratio);
    }

    private static (float trackHeight, float thumbSize) GetSliderDimensions(ControlSize size)
    {
        return size switch
        {
            ControlSize.Small => (4f, 14f),
            ControlSize.Large => (8f, 22f),
            _ => (6f, 18f)
        };
    }
    #endregion
}

#endregion
