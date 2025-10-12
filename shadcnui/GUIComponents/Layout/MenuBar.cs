using shadcnui.GUIComponents.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class MenuBar
    {
        private readonly GUIHelper _guiHelper;
        private readonly Layout _layoutComponents;
        private int _activeMenuIndex = -1;
        private bool _isDropdownOpen = false;
        private readonly Stack<MenuData> _menuStack = new Stack<MenuData>();
        private Rect _menuBarRect;

        public MenuBar(GUIHelper helper)
        {
            _guiHelper = helper;
            _layoutComponents = new Layout(helper);
        }

        public bool IsDropdownOpen => _isDropdownOpen;

        public class MenuItem
        {
            public string Text;
            public Action OnClick;
            public bool Disabled;
            public List<MenuItem> SubItems;
            public string Shortcut;
            public bool IsSeparator;
            public bool IsHeader;

            public MenuItem() { }

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

        public class MenuBarConfig
        {
            public List<MenuItem> Items { get; set; }
            public GUILayoutOption[] Options { get; set; }

            public MenuBarConfig(List<MenuItem> items)
            {
                Items = items;
                Options = Array.Empty<GUILayoutOption>();
            }
        }

        public void Draw(MenuBarConfig config)
        {
            Draw(config.Items, config.Options);
        }

        public void Draw(List<MenuItem> items, params GUILayoutOption[] options)
        {
            if (items == null || items.Count == 0)
                return;

            var styleManager = _guiHelper.GetStyleManager();

            _layoutComponents.BeginHorizontalGroup(styleManager.menuBarStyle, options);
            _menuBarRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, options);

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.IsSeparator || item.IsHeader)
                    continue;

                var isActive = (i == _activeMenuIndex) && _isDropdownOpen;
                var itemStyle = isActive ? styleManager.menuBarItemStyle : styleManager.menuBarItemStyle;

                var wasEnabled = GUI.enabled;
                if (item.Disabled)
                    GUI.enabled = false;

                var clicked = UnityHelpers.Button(item.Text, itemStyle, GUILayout.ExpandWidth(false));

                GUI.enabled = wasEnabled;

                if (clicked)
                {
                    if (item.SubItems.Count > 0)
                    {
                        _activeMenuIndex = i;
                        _menuStack.Clear();
                        _menuStack.Push(new MenuData(item.SubItems, i));
                        _isDropdownOpen = true;
                    }
                    else
                    {
                        item.OnClick?.Invoke();
                        CloseDropdown();
                    }
                }
            }

            _layoutComponents.EndHorizontalGroup();

            if (_isDropdownOpen && _menuStack.Count > 0)
            {
                DrawDropdownMenu();
            }

            HandleClickOutside();
        }

        private void DrawDropdownMenu()
        {
            var styleManager = _guiHelper.GetStyleManager();
            var currentMenu = _menuStack.Peek();

            _layoutComponents.BeginVerticalGroup(styleManager.menuDropdownStyle, GUILayout.Width(200 * _guiHelper.uiScale));

            if (_menuStack.Count > 1)
            {
                if (UnityHelpers.Button("<- Back", styleManager.menuBarItemStyle))
                {
                    _menuStack.Pop();
                    if (_menuStack.Count == 1)
                    {
                        _activeMenuIndex = _menuStack.Peek().ParentIndex;
                    }
                    return;
                }

                UnityHelpers.Box("", GUI.skin.horizontalSlider);
            }

            foreach (var item in currentMenu.Items)
            {
                DrawMenuItem(item);
            }

            _layoutComponents.EndVerticalGroup();
        }

        private void DrawMenuItem(MenuItem item)
        {
            var styleManager = _guiHelper.GetStyleManager();

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

            var wasEnabled = GUI.enabled;
            if (item.Disabled)
                GUI.enabled = false;

            if (item.SubItems.Count > 0)
            {
                _layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(item.Text, styleManager.menuBarItemStyle);
                GUILayout.FlexibleSpace();
                UnityHelpers.Label("›", styleManager.menuBarItemStyle);
                _layoutComponents.EndHorizontalGroup();

                var itemRect = GUILayoutUtility.GetLastRect();
                if (GUI.Button(itemRect, "", GUIStyle.none))
                {
                    _menuStack.Push(new MenuData(item.SubItems, _activeMenuIndex));
                }
            }
            else
            {
                _layoutComponents.BeginHorizontalGroup();
                UnityHelpers.Label(item.Text, styleManager.menuBarItemStyle);

                if (!string.IsNullOrEmpty(item.Shortcut))
                {
                    GUILayout.FlexibleSpace();
                    UnityHelpers.Label(item.Shortcut, styleManager.menuBarItemStyle);
                }

                _layoutComponents.EndHorizontalGroup();

                var itemRect = GUILayoutUtility.GetLastRect();
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
            if (Event.current.type == EventType.MouseDown && _isDropdownOpen)
            {
                var mousePos = Event.current.mousePosition;
                if (!_menuBarRect.Contains(mousePos))
                {
                    CloseDropdown();
                    Event.current.Use();
                }
            }
        }

        public void CloseDropdown()
        {
            _activeMenuIndex = -1;
            _isDropdownOpen = false;
            _menuStack.Clear();
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
    }
}
