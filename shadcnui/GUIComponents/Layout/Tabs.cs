using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#elif IL2CPP_BEPINEX
using Il2CppInterop.Runtime.InteropTypes.Arrays;
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
                    bool horizontalStarted = false;
                    try
                    {
                        layoutComponents.BeginHorizontalGroup(styleManager.GetTabsListStyle());
                        horizontalStarted = true;

                        for (int i = line * tabsPerLine; i < (line + 1) * tabsPerLine && i < tabNames.Length; i++)
                        {
                            bool isActive = i == selectedIndex;
                            GUIStyle triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

                            var tabOptions = new List<GUILayoutOption>();
                            tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));

                            if (options != null && options.Length > 0)
                                tabOptions.AddRange(options);

                            bool clicked = false;
#if IL2CPP_MELONLOADER
                            clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
#elif IL2CPP_BEPINEX
                            var optionsArray = new Il2CppReferenceArray<GUILayoutOption>(tabOptions.Count);
                            for (int j = 0; j < tabOptions.Count; j++)
                            {
                                optionsArray[j] = tabOptions[j];
                            }
                            clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, optionsArray);
#else
                            clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
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
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (horizontalStarted)
                        {
                            try { layoutComponents.EndHorizontalGroup(); } catch { }
                        }
                    }
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
                bool outerHorizontalStarted = false;
                try
                {
                    layoutComponents.BeginHorizontalGroup();
                    outerHorizontalStarted = true;

                    int tabsPerColumn = (int)Mathf.Ceil((float)tabNames.Length / maxLines);
                    for (int col = 0; col < maxLines; col++)
                    {
                        bool verticalStarted = false;
                        try
                        {
                            layoutComponents.BeginVerticalGroup();
                            verticalStarted = true;

                            for (int i = col * tabsPerColumn; i < (col + 1) * tabsPerColumn && i < tabNames.Length; i++)
                            {
                                bool isActive = i == selectedIndex;
                                GUIStyle triggerStyle = styleManager.GetTabsTriggerStyle(isActive);
                                var tabOptions = new List<GUILayoutOption>();
                                tabOptions.Add(GUILayout.Width(tabWidth * guiHelper.uiScale));
                                tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));
                                if (options != null && options.Length > 0)
                                    tabOptions.AddRange(options);

                                bool clicked = false;
#if IL2CPP_MELONLOADER
                                clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
#elif IL2CPP_BEPINEX
                                var optionsArray = new Il2CppReferenceArray<GUILayoutOption>(tabOptions.Count);
                                for (int j = 0; j < tabOptions.Count; j++)
                                {
                                    optionsArray[j] = tabOptions[j];
                                }
                                clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, optionsArray);
#else
                                clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle, tabOptions.ToArray());
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
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            if (verticalStarted)
                            {
                                try { layoutComponents.EndVerticalGroup(); } catch { }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (outerHorizontalStarted)
                    {
                        try { layoutComponents.EndHorizontalGroup(); } catch { }
                    }
                }
            };

            bool mainHorizontalStarted = false;
            try
            {
                layoutComponents.BeginHorizontalGroup();
                mainHorizontalStarted = true;

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
            }
            catch (Exception)
            {

            }
            finally
            {
                if (mainHorizontalStarted)
                {
                    try { layoutComponents.EndHorizontalGroup(); } catch { }
                }
            }

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