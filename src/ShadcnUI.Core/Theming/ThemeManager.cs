namespace ShadcnUI.Core.Theming;

/// <summary>
/// Manages theme switching and provides access to the current theme.
/// </summary>
public sealed class ThemeManager
{
    private static readonly Lazy<ThemeManager> _instance = new(() => new ThemeManager());
    public static ThemeManager Instance => _instance.Value;

    private Theme _currentTheme = Theme.Dark;
    private readonly List<Theme> _availableThemes;

    public Theme CurrentTheme => _currentTheme;
    public IReadOnlyList<Theme> AvailableThemes => _availableThemes;

    public event Action? OnThemeChanged;

    private ThemeManager()
    {
        _availableThemes = new List<Theme>
        {
            Theme.Dark,
            Theme.Light,
            Theme.Zinc,
            Theme.Slate,
            Theme.Gray,
            Theme.Stone,
            Theme.Olive,
            Theme.Cyan,
            Theme.BlueDark,
            Theme.Rose,
            Theme.Violet
        };
    }

    public void SetTheme(Theme theme)
    {
        if (theme.Name == _currentTheme.Name) return;
        
        _currentTheme = theme;
        OnThemeChanged?.Invoke();
    }

    public void SetTheme(string themeName)
    {
        var theme = _availableThemes.FirstOrDefault(t => 
            t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
        
        if (theme != null)
        {
            SetTheme(theme);
        }
    }

    public void ToggleLightDark()
    {
        if (_currentTheme.Name == "light")
            SetTheme(Theme.Dark);
        else
            SetTheme(Theme.Light);
    }

    public void CycleThemes()
    {
        var currentIndex = _availableThemes.FindIndex(t => t.Name == _currentTheme.Name);
        var nextIndex = (currentIndex + 1) % _availableThemes.Count;
        SetTheme(_availableThemes[nextIndex]);
    }
}
