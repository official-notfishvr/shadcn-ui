using ShadcnUI.Core.Theming;

namespace ShadcnUI.Core;

/// <summary>
/// Theme interface.
/// </summary>
public interface ITheme
{
    string Name { get; }
    Color Background { get; }
    Color Foreground { get; }
    Color Primary { get; }
    Color Secondary { get; }
}

/// <summary>
/// Base interface for all UI components.
/// </summary>
public interface IComponent : IDisposable
{
    void Initialize();
    void EnsureInitialized();
}

/// <summary>
/// Lightweight base class for components.
/// Uses composition over inheritance pattern.
/// </summary>
public abstract class ComponentBase : IComponent
{
    protected bool IsDisposed { get; private set; }
    private bool _initialized;

    protected ComponentBase()
    {
    }

    public void EnsureInitialized()
    {
        if (!_initialized)
        {
            _initialized = true;
            Initialize();
        }
    }

    public virtual void Initialize() { }

    protected virtual void OnDispose() { }

    public void Dispose()
    {
        if (IsDisposed) return;
        
        try
        {
            OnDispose();
        }
        finally
        {
            IsDisposed = true;
        }
    }
}

/// <summary>
/// Context for component rendering. Provides access to services and state.
/// </summary>
public sealed class ComponentContext
{
    public IUIService Services { get; }
    public Theme? CurrentTheme => Services.ThemeManager.CurrentTheme;
    public float UIScale { get; set; } = 1f;
    public int FontSize { get; set; } = 14;

    public ComponentContext(IUIService services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}
