using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class MenuBar : BaseComponent
    {
        private int _activeMenuIndex = -1;
        private bool _isDropdownOpen = false;
        private readonly Stack<MenuData> _menuStack = new Stack<MenuData>();
        private Rect _menuBarRect;

        public MenuBar(GUIHelper helper)
            : base(helper) { }

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

            var styleManager = guiHelper.GetStyleManager();

            layoutComponents.BeginHorizontalGroup(styleManager.GetMenuBarStyle(), options);

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.IsSeparator || item.IsHeader)
                    continue;

                var itemStyle = styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default);

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

            layoutComponents.EndHorizontalGroup();
            _menuBarRect = GUILayoutUtility.GetLastRect();

            if (_isDropdownOpen && _menuStack.Count > 0)
            {
                DrawDropdownMenu();
            }

            HandleClickOutside();
        }

        private void DrawDropdownMenu()
        {
            var styleManager = guiHelper.GetStyleManager();
            var currentMenu = _menuStack.Peek();

            layoutComponents.BeginVerticalGroup(styleManager.GetMenuDropdownStyle(), GUILayout.Width(220 * guiHelper.uiScale));

            if (_menuStack.Count > 1)
            {
                if (UnityHelpers.Button("<- Back", styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default)))
                {
                    _menuStack.Pop();
                    if (_menuStack.Count == 1)
                    {
                        _activeMenuIndex = _menuStack.Peek().ParentIndex;
                    }
                    layoutComponents.EndVerticalGroup();
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
                UnityHelpers.Label(item.Text, styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default));
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
                if (GUILayout.Button(item.Text, styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default), GUILayout.ExpandWidth(true)))
                {
                    _menuStack.Push(new MenuData(item.SubItems, _activeMenuIndex));
                }
            }
            else
            {
                var buttonStyle = styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default);
                var textStyle = styleManager.GetMenuBarItemStyle();

                Rect rect = GUILayoutUtility.GetRect(GUIContent.none, buttonStyle, GUILayout.ExpandWidth(true));

                if (GUI.Button(rect, "", buttonStyle))
                {
                    item.OnClick?.Invoke();
                    CloseDropdown();
                }

                GUI.Label(rect, item.Text, textStyle);

                if (!string.IsNullOrEmpty(item.Shortcut))
                {
                    var shortcutStyle = new UnityHelpers.GUIStyle(textStyle) { alignment = TextAnchor.MiddleRight };
                    GUI.Label(rect, item.Shortcut, shortcutStyle);
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
