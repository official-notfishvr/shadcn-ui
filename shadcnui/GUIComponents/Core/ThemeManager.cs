using System;
using System.Collections.Generic;

namespace shadcnui.GUIComponents.Core
{
    public class ThemeManager
    {
        private static ThemeManager _instance;
        public static ThemeManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ThemeManager();
                return _instance;
            }
        }

        public Dictionary<string, Theme> Themes { get; private set; }
        public Theme CurrentTheme { get; private set; }

        public event Action OnThemeChanged;

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
            if (!Themes.ContainsKey(theme.Name))
            {
                Themes.Add(theme.Name, theme);
            }
        }

        public void SetTheme(string themeName)
        {
            if (Themes.TryGetValue(themeName, out var theme))
            {
                CurrentTheme = theme;
                OnThemeChanged?.Invoke();
            }
        }
    }
}
