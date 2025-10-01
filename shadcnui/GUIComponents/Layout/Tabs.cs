using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Tabs
    {
        public enum TabSide
        {
            Left,
            Right,
        }
        public enum TabPosition
        {
            Top,
            Bottom,
        }

        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Tabs(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public int DrawTabButtons(string[] tabNames, int selectedIndex, Action content, Action<int> onTabChange, int maxLines, TabPosition position, params GUILayoutOption[] options)
        {
            if (tabNames == null || tabNames.Length == 0)
            {
                content?.Invoke();
                return selectedIndex;
            }

            var styleManager = guiHelper.GetStyleManager();
            selectedIndex = Mathf.Clamp(selectedIndex, 0, tabNames.Length - 1);
            int newSelectedIndex = selectedIndex;

            Action drawTabs = () =>
            {
                int tabsPerLine = (int)Mathf.Ceil((float)tabNames.Length / maxLines);
                for (int line = 0; line < maxLines; line++)
                {
                    layoutComponents.BeginHorizontalGroup(styleManager.GetTabsListStyle());
                    for (int i = line * tabsPerLine; i < (line + 1) * tabsPerLine && i < tabNames.Length; i++)
                    {
                        bool isActive = i == selectedIndex;
                        GUIStyle triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

                        var tabOptions = new List<GUILayoutOption>();
                        tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));

                        if (options != null && options.Length > 0)
                            tabOptions.AddRange(options);

#if IL2CPP
                        bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, (Il2CppReferenceArray<GUILayoutOption>)tabOptions.ToArray());
#else
                        bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
#endif
                        if (clicked && i != selectedIndex)
                        {
                            newSelectedIndex = i;
                            onTabChange?.Invoke(i);
                        }

                        if (i < (line + 1) * tabsPerLine - 1 && i < tabNames.Length - 1)
                        {
                            layoutComponents.AddSpace(2);
                        }
                    }
                    layoutComponents.EndHorizontalGroup();
                }
            };

            if (position == TabPosition.Top)
            {
                drawTabs();
                content?.Invoke();
            }
            else
            {
                content?.Invoke();
                drawTabs();
            }

            return newSelectedIndex;
        }

        public int DrawTabs(string[] tabNames, int selectedIndex, Action<int> onTabChange, int maxLines, params GUILayoutOption[] options)
        {
            return DrawTabButtons(tabNames, selectedIndex, null, onTabChange, maxLines, TabPosition.Top, options);
        }

        public void BeginTabContent()
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetTabsContentStyle());
        }

        public void EndTabContent()
        {
            layoutComponents.EndVerticalGroup();
        }

        public int TabsWithContent(TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null)
        {
            if (tabConfigs == null || tabConfigs.Length == 0)
                return selectedIndex;

            string[] tabNames = new string[tabConfigs.Length];
            for (int i = 0; i < tabConfigs.Length; i++)
            {
                tabNames[i] = tabConfigs[i].Name;
            }

            int newSelectedIndex = DrawTabButtons(tabNames, selectedIndex, null, onTabChange, 1, TabPosition.Top);

            if (newSelectedIndex >= 0 && newSelectedIndex < tabConfigs.Length)
            {
                BeginTabContent();
                tabConfigs[newSelectedIndex].Content?.Invoke();
                EndTabContent();
            }

            return newSelectedIndex;
        }

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action content, Action<int> onTabChange = null, float tabWidth = 120f, int maxLines = 1, TabSide side = TabSide.Left, params GUILayoutOption[] options)
        {
            if (tabNames == null || tabNames.Length == 0)
            {
                content?.Invoke();
                return selectedIndex;
            }

            var styleManager = guiHelper.GetStyleManager();
            selectedIndex = Mathf.Clamp(selectedIndex, 0, tabNames.Length - 1);
            int newSelectedIndex = selectedIndex;

            Action drawTabs = () =>
            {
                layoutComponents.BeginHorizontalGroup();
                int tabsPerColumn = (int)Mathf.Ceil((float)tabNames.Length / maxLines);
                for (int col = 0; col < maxLines; col++)
                {
                    layoutComponents.BeginVerticalGroup();
                    for (int i = col * tabsPerColumn; i < (col + 1) * tabsPerColumn && i < tabNames.Length; i++)
                    {
                        bool isActive = i == selectedIndex;
                        GUIStyle triggerStyle = styleManager.GetTabsTriggerStyle(isActive);
                        var tabOptions = new List<GUILayoutOption>();
                        tabOptions.Add(GUILayout.Width(tabWidth * guiHelper.uiScale));
                        tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));
                        if (options != null && options.Length > 0)
                            tabOptions.AddRange(options);
#if IL2CPP
                        bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, (Il2CppReferenceArray<GUILayoutOption>)tabOptions.ToArray());
#else
                        bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
#endif
                        if (clicked && i != selectedIndex)
                        {
                            newSelectedIndex = i;
                            onTabChange?.Invoke(i);
                        }
                        if (i < (col + 1) * tabsPerColumn - 1 && i < tabNames.Length - 1)
                        {
                            layoutComponents.AddSpace(2);
                        }
                    }
                    layoutComponents.EndVerticalGroup();
                }
                layoutComponents.EndHorizontalGroup();
            };

            layoutComponents.BeginHorizontalGroup();
            if (side == TabSide.Left)
            {
                drawTabs();
                content?.Invoke();
            }
            else
            {
                content?.Invoke();
                drawTabs();
            }
            layoutComponents.EndHorizontalGroup();

            return newSelectedIndex;
        }

        public struct TabConfig
        {
            public string Name;
            public Action Content;
            public bool Disabled;

            public TabConfig(string name, Action content, bool disabled = false)
            {
                Name = name;
                Content = content;
                Disabled = disabled;
            }
        }
    }
}
