using System;
using System.Collections.Generic;

namespace shadcnui.GUIComponents.Core.Theming
{
    public sealed class ThemeManager
    {
        private static readonly Lazy<ThemeManager> _instance = new(() => new ThemeManager());

        public static ThemeManager Instance => _instance.Value;

        private readonly object _lock = new();

        public Dictionary<string, Theme> Themes { get; } = new(StringComparer.OrdinalIgnoreCase);

        public Theme CurrentTheme { get; private set; }

        public event Action OnThemeChanged;

        private ThemeManager()
        {
            AddTheme(Theme.Dark);
            AddTheme(Theme.Light);
            AddTheme(Theme.Slate);
            AddTheme(Theme.Gray);
            AddTheme(Theme.Zinc);
            AddTheme(Theme.Stone);
            AddTheme(Theme.Olive);
            AddTheme(Theme.Cyan);
            AddTheme(Theme.BlueDark);
            AddTheme(Theme.Rose);
            AddTheme(Theme.Violet);
            CurrentTheme = Themes["Dark"];
        }

        public void AddTheme(Theme theme)
        {
            if (theme == null || string.IsNullOrWhiteSpace(theme.Name))
                return;

            bool currentThemeChanged = false;
            lock (_lock)
            {
                var registeredTheme = theme.Clone();
                Themes[theme.Name] = registeredTheme;

                if (CurrentTheme != null && string.Equals(CurrentTheme.Name, theme.Name, StringComparison.OrdinalIgnoreCase))
                {
                    CurrentTheme = registeredTheme.Clone();
                    currentThemeChanged = true;
                }
            }

            if (currentThemeChanged)
                OnThemeChanged?.Invoke();
        }

        public bool RemoveTheme(string themeName)
        {
            if (string.IsNullOrWhiteSpace(themeName))
                return false;

            lock (_lock)
            {
                if (CurrentTheme != null && string.Equals(CurrentTheme.Name, themeName, StringComparison.OrdinalIgnoreCase))
                    return false;

                return Themes.Remove(themeName);
            }
        }

        public bool SetTheme(string themeName)
        {
            if (string.IsNullOrWhiteSpace(themeName))
                return false;

            Theme nextTheme;
            lock (_lock)
            {
                if (!Themes.TryGetValue(themeName, out nextTheme))
                    return false;

                CurrentTheme = nextTheme.Clone();
            }

            OnThemeChanged?.Invoke();
            return true;
        }

        public Theme GetTheme(string themeName)
        {
            lock (_lock)
                return Themes.TryGetValue(themeName, out var theme) ? theme.Clone() : null;
        }
    }
}
