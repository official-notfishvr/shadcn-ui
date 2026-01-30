using System;
using System.Collections.Generic;

namespace shadcnui.GUIComponents.Core.Theming
{
    public class ThemeManager
    {
        private static readonly Lazy<ThemeManager> _lazy = new Lazy<ThemeManager>(() => new ThemeManager());
        public static ThemeManager Instance => _lazy.Value;

        public Dictionary<string, Theme> Themes { get; private set; }
        public Theme CurrentTheme { get; private set; }

        public event Action OnThemeChanged;

        private readonly object _themeLock = new object();

        private ThemeManager()
        {
            Themes = new Dictionary<string, Theme>();
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

            if (Themes.TryGetValue("Dark", out var darkTheme))
            {
                CurrentTheme = darkTheme;
            }
            else if (Themes.Count > 0)
            {
                foreach (var theme in Themes.Values)
                {
                    CurrentTheme = theme;
                    break;
                }
            }
            else
            {
                throw new InvalidOperationException("ThemeManager initialization failed: no valid themes available.");
            }
        }

        public void AddTheme(Theme theme)
        {
            if (theme == null || string.IsNullOrEmpty(theme.Name))
                return;
            lock (_themeLock)
            {
                if (!Themes.ContainsKey(theme.Name))
                    Themes[theme.Name] = theme;
            }
        }

        public bool RemoveTheme(string themeName)
        {
            if (string.IsNullOrEmpty(themeName))
                return false;
            lock (_themeLock)
            {
                if (themeName == CurrentTheme?.Name)
                    return false;
                return Themes.Remove(themeName);
            }
        }

        public void SetTheme(string themeName)
        {
            Action handler = null;
            lock (_themeLock)
            {
                if (Themes.TryGetValue(themeName, out var theme))
                {
                    CurrentTheme = theme;
                    handler = OnThemeChanged;
                }
            }
            handler?.Invoke();
        }
    }
}
