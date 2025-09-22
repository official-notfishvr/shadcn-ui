using System.Collections.Generic;

namespace shadcnui.GUIComponents
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

        private ThemeManager()
        {
            Themes = new Dictionary<string, Theme>();
            AddTheme(Theme.Dark);
            AddTheme(Theme.Light);
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
            }
        }
    }
}
