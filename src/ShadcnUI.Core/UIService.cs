using ShadcnUI.Core.Theming;
using ShadcnUI.Core.Styling;
using ShadcnUI.Core.Animation;

namespace ShadcnUI.Core;

/// <summary>
/// Service interface providing access to core UI services.
/// </summary>
public interface IUIService
{
    ThemeManager ThemeManager { get; }
    IStyleProvider StyleProvider { get; }
    IAnimationEngine AnimationEngine { get; }
    ComponentContext Context { get; }
    float UIScale { get; set; }
    int FontSize { get; set; }
}

/// <summary>
/// Style provider interface for resolving GUI styles.
/// </summary>
public interface IStyleProvider
{
    GUIStyle GetStyle(StyleKey key);
    GUIStyle GetStyle(StyleComponentType type, ControlVariant variant = ControlVariant.Default, 
        ControlSize size = ControlSize.Default, ComponentState state = ComponentState.Default);
    void InvalidateCache();
}

/// <summary>
/// Animation engine interface.
/// </summary>
public interface IAnimationEngine
{
    float DeltaTime { get; }
    void Animate(ref float value, float target, float speed);
    float SmoothStep(float t);
    float Lerp(float a, float b, float t);
}

/// <summary>
/// Default implementation of UI services.
/// </summary>
public sealed class UIService : IUIService
{
    public ThemeManager ThemeManager { get; }
    public IStyleProvider StyleProvider { get; }
    public IAnimationEngine AnimationEngine { get; }
    public ComponentContext Context { get; }
    
    public float UIScale 
    { 
        get => Context.UIScale; 
        set => Context.UIScale = value; 
    }
    
    public int FontSize 
    { 
        get => Context.FontSize; 
        set => Context.FontSize = value; 
    }

    public UIService()
    {
        ThemeManager = ThemeManager.Instance;
        StyleProvider = new StyleProvider(this);
        AnimationEngine = new AnimationEngine();
        Context = new ComponentContext(this);
    }
}
