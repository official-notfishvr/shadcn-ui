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
            CurrentTheme = Themes["Dark"];
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
            if (string.IsNullOrEmpty(themeName) || themeName == CurrentTheme?.Name)
                return false;
            lock (_themeLock)
            {
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
