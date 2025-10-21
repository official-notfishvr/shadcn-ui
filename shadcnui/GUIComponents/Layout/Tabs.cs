using System;
using System.Collections.Generic;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Tabs : BaseComponent
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
            Left,
            Right,
        }

        public Tabs(GUIHelper helper)
            : base(helper) { }

        public class TabsConfig
        {
            public string[] TabNames { get; set; }
            public int SelectedIndex { get; set; }
            public Action<int> OnTabChange { get; set; }
            public Action Content { get; set; }
            public int MaxLines { get; set; }
            public TabPosition Position { get; set; }
            public TabSide Side { get; set; }
            public float TabWidth { get; set; }
            public GUILayoutOption[] Options { get; set; }

            public TabsConfig(string[] tabNames, int selectedIndex)
            {
                TabNames = tabNames;
                SelectedIndex = selectedIndex;
                OnTabChange = null;
                Content = null;
                MaxLines = 1;
                Position = TabPosition.Top;
                Side = TabSide.Left;
                TabWidth = 120f;
                Options = Array.Empty<GUILayoutOption>();
            }
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

        public int Draw(TabsConfig config)
        {
            return DrawTabsInternal(config);
        }

        public int Draw(string[] tabNames, int selectedIndex, Action content, Action<int> onTabChange = null, int maxLines = 1, TabPosition position = TabPosition.Top, TabSide side = TabSide.Left, float tabWidth = 120f, params GUILayoutOption[] options)
        {
            var config = new TabsConfig(tabNames, selectedIndex)
            {
                OnTabChange = onTabChange,
                Content = content,
                MaxLines = maxLines,
                Position = position,
                Side = side,
                TabWidth = tabWidth,
                Options = options,
            };
            return DrawTabsInternal(config);
        }

        private int DrawTabsInternal(TabsConfig config)
        {
            if (config.TabNames == null || config.TabNames.Length == 0)
            {
                config.Content?.Invoke();
                return config.SelectedIndex;
            }

            var styleManager = guiHelper.GetStyleManager();
            var selectedIndex = Mathf.Clamp(config.SelectedIndex, 0, config.TabNames.Length - 1);
            var newSelectedIndex = selectedIndex;

            Action drawTabButtons = () =>
            {
                var tabsPerLine = (int)Mathf.Ceil((float)config.TabNames.Length / config.MaxLines);
                for (var line = 0; line < config.MaxLines; line++)
                {
                    var horizontalStarted = false;
                    try
                    {
                        layoutComponents.BeginHorizontalGroup(styleManager.GetTabsListStyle());
                        horizontalStarted = true;

                        for (var i = line * tabsPerLine; i < (line + 1) * tabsPerLine && i < config.TabNames.Length; i++)
                        {
                            var isActive = i == selectedIndex;
                            var triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

                            var tabOptions = new List<GUILayoutOption>();
                            tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));

                            if (config.Options != null && config.Options.Length > 0)
                                tabOptions.AddRange(config.Options);

                            var clicked = UnityHelpers.Button(config.TabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
                            if (clicked && i != selectedIndex)
                            {
                                newSelectedIndex = i;
                                config.OnTabChange?.Invoke(i);
                            }

                            if (i < (line + 1) * tabsPerLine - 1 && i < config.TabNames.Length - 1)
                            {
                                layoutComponents.AddSpace(2);
                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (horizontalStarted)
                        {
                            try
                            {
                                layoutComponents.EndHorizontalGroup();
                            }
                            catch { }
                        }
                    }
                }
            };

            Action drawVerticalTabButtons = () =>
            {
                var tabsPerColumn = (int)Mathf.Ceil((float)config.TabNames.Length / config.MaxLines);
                for (var col = 0; col < config.MaxLines; col++)
                {
                    var verticalStarted = false;
                    try
                    {
                        layoutComponents.BeginVerticalGroup(GUILayout.Width(config.TabWidth * guiHelper.uiScale));
                        verticalStarted = true;

                        for (var i = col * tabsPerColumn; i < (col + 1) * tabsPerColumn && i < config.TabNames.Length; i++)
                        {
                            var isActive = i == selectedIndex;
                            var triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

                            var tabOptions = new List<GUILayoutOption>();

                            tabOptions.Add(GUILayout.Width(config.TabWidth * guiHelper.uiScale));
                            tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));
                            tabOptions.Add(GUILayout.ExpandWidth(false));
                            tabOptions.Add(GUILayout.ExpandHeight(false));

                            if (config.Options != null && config.Options.Length > 0)
                                tabOptions.AddRange(config.Options);

                            var clicked = UnityHelpers.Button(config.TabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());

                            if (clicked && i != selectedIndex)
                            {
                                newSelectedIndex = i;
                                config.OnTabChange?.Invoke(i);
                            }

                            if (i < (col + 1) * tabsPerColumn - 1 && i < config.TabNames.Length - 1)
                            {
                                layoutComponents.AddSpace(2);
                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (verticalStarted)
                        {
                            try
                            {
                                layoutComponents.EndVerticalGroup();
                            }
                            catch { }
                        }
                    }
                }
            };

            if (config.Position == TabPosition.Top)
            {
                drawTabButtons();
                config.Content?.Invoke();
            }
            else if (config.Position == TabPosition.Bottom)
            {
                config.Content?.Invoke();
                drawTabButtons();
            }
            else if (config.Position == TabPosition.Left)
            {
                var mainHorizontalStarted = false;
                try
                {
                    layoutComponents.BeginHorizontalGroup();
                    mainHorizontalStarted = true;

                    drawVerticalTabButtons();
                    config.Content?.Invoke();
                }
                catch (Exception) { }
                finally
                {
                    if (mainHorizontalStarted)
                    {
                        try
                        {
                            layoutComponents.EndHorizontalGroup();
                        }
                        catch { }
                    }
                }
            }
            else if (config.Position == TabPosition.Right)
            {
                var mainHorizontalStarted = false;
                try
                {
                    layoutComponents.BeginHorizontalGroup();
                    mainHorizontalStarted = true;

                    config.Content?.Invoke();
                    drawVerticalTabButtons();
                }
                catch (Exception) { }
                finally
                {
                    if (mainHorizontalStarted)
                    {
                        try
                        {
                            layoutComponents.EndHorizontalGroup();
                        }
                        catch { }
                    }
                }
            }

            return newSelectedIndex;
        }

        public void BeginTabContent(params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetTabsContentStyle(), options);
        }

        public void EndTabContent()
        {
            layoutComponents.EndVerticalGroup();
        }

        public int TabsWithContent(TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null)
        {
            if (tabConfigs == null || tabConfigs.Length == 0)
                return selectedIndex;

            var tabNames = new string[tabConfigs.Length];
            for (var i = 0; i < tabConfigs.Length; i++)
            {
                tabNames[i] = tabConfigs[i].Name;
            }

            var newSelectedIndex = Draw(tabNames, selectedIndex, null, onTabChange, 1, TabPosition.Top);

            if (newSelectedIndex >= 0 && newSelectedIndex < tabConfigs.Length)
            {
                BeginTabContent(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                tabConfigs[newSelectedIndex].Content?.Invoke();
                EndTabContent();
            }

            return newSelectedIndex;
        }
    }
}
