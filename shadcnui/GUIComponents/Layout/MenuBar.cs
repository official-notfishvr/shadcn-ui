using System;
using System.Collections.Generic;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class MenuBar
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;
        private int activeMenuIndex = -1;
        private bool isDropdownOpen = false;
        private Stack<MenuData> menuStack = new Stack<MenuData>();
        private Rect menuBarRect;

        public MenuBar(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public bool IsDropdownOpen => isDropdownOpen;

        public struct MenuItem
        {
            public string Text;
            public Action OnClick;
            public bool Disabled;
            public List<MenuItem> SubItems;
            public string Shortcut;
            public bool IsSeparator;
            public bool IsHeader;

            public MenuItem(string text, Action onClick = null, bool disabled = false, List<MenuItem> subItems = null, string shortcut = "")
            {
                Text = text;
                OnClick = onClick;
                Disabled = disabled;
                SubItems = subItems ?? new List<MenuItem>();
                Shortcut = shortcut;
                IsSeparator = false;
                IsHeader = false;
            }

            public static MenuItem Separator()
            {
                return new MenuItem { IsSeparator = true };
            }

            public static MenuItem Header(string text)
            {
                return new MenuItem { Text = text, IsHeader = true };
            }
        }

        private struct MenuData
        {
            public List<MenuItem> Items;
            public int ParentIndex;

            public MenuData(List<MenuItem> items, int parentIndex)
            {
                Items = items;
                ParentIndex = parentIndex;
            }
        }

        public void DrawMenuBar(List<MenuItem> items, params GUILayoutOption[] options)
        {
            if (items == null || items.Count == 0)
                return;

            var styleManager = guiHelper.GetStyleManager();

            layoutComponents.BeginHorizontalGroup(styleManager.menuBarStyle, options);
            menuBarRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, options);

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.IsSeparator || item.IsHeader)
                    continue;

                bool isActive = (i == activeMenuIndex) && isDropdownOpen;
                GUIStyle itemStyle = isActive ? styleManager.menuBarItemStyle : styleManager.menuBarItemStyle;

                bool wasEnabled = GUI.enabled;
                if (item.Disabled)
                    GUI.enabled = false;

                bool clicked = UnityHelpers.Button(item.Text, itemStyle, GUILayout.ExpandWidth(false));

                GUI.enabled = wasEnabled;

                if (clicked)
                {
                    if (item.SubItems.Count > 0)
                    {
                        activeMenuIndex = i;
                        menuStack.Clear();
                        menuStack.Push(new MenuData(item.SubItems, i));
                        isDropdownOpen = true;
                    }
                    else
                    {
                        item.OnClick?.Invoke();
                        CloseDropdown();
                    }
                }
            }

            layoutComponents.EndHorizontalGroup();

            if (isDropdownOpen && menuStack.Count > 0)
            {
                DrawDropdownMenu();
            }

            HandleClickOutside();
        }

        private void DrawDropdownMenu()
        {
            var styleManager = guiHelper.GetStyleManager();
            var currentMenu = menuStack.Peek();

            layoutComponents.BeginVerticalGroup(styleManager.menuDropdownStyle, GUILayout.Width(200 * guiHelper.uiScale));

            if (menuStack.Count > 1)
            {
                if (UnityHelpers.Button("← Back", styleManager.menuBarItemStyle))
                {
                    menuStack.Pop();
                    if (menuStack.Count == 1)
                    {
                        activeMenuIndex = menuStack.Peek().ParentIndex;
                    }
                    return;
                }

                UnityHelpers.Box("", GUI.skin.horizontalSlider);
            }

            foreach (var item in currentMenu.Items)
            {
                DrawMenuItem(item);
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawMenuItem(MenuItem item)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (item.IsHeader)
            {
                UnityHelpers.Label(item.Text, styleManager.menuBarItemStyle);
                return;
            }

            if (item.IsSeparator)
            {
                UnityHelpers.Box("", GUI.skin.horizontalSlider);
                return;
            }

            bool wasEnabled = GUI.enabled;
            if (item.Disabled)
                GUI.enabled = false;

            if (item.SubItems.Count > 0)
            {
                layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(item.Text, styleManager.menuBarItemStyle);
                GUILayout.FlexibleSpace();
                UnityHelpers.Label("›", styleManager.menuBarItemStyle);
                layoutComponents.EndHorizontalGroup();

                Rect itemRect = GUILayoutUtility.GetLastRect();
                if (GUI.Button(itemRect, "", GUIStyle.none))
                {
                    menuStack.Push(new MenuData(item.SubItems, activeMenuIndex));
                }
            }
            else
            {
                layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(item.Text, styleManager.menuBarItemStyle);

                if (!string.IsNullOrEmpty(item.Shortcut))
                {
                    GUILayout.FlexibleSpace();
                    UnityHelpers.Label(item.Shortcut, styleManager.menuBarItemStyle);
                }

                layoutComponents.EndHorizontalGroup();

                Rect itemRect = GUILayoutUtility.GetLastRect();
                if (GUI.Button(itemRect, "", GUIStyle.none))
                {
                    item.OnClick?.Invoke();
                    CloseDropdown();
                }
            }

            GUI.enabled = wasEnabled;
        }

        private void HandleClickOutside()
        {
            if (Event.current.type == EventType.MouseDown && isDropdownOpen)
            {
                Vector2 mousePos = Event.current.mousePosition;
                if (!menuBarRect.Contains(mousePos))
                {
                    CloseDropdown();
                    Event.current.Use();
                }
            }
        }

        public void CloseDropdown()
        {
            activeMenuIndex = -1;
            isDropdownOpen = false;
            menuStack.Clear();
        }
    }
}
