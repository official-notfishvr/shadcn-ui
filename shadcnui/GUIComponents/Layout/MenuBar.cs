using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class MenuBar : BaseComponent
    {
        private int _activeMenuIndex = -1;
        private bool _isDropdownOpen = false;
        private readonly Stack<MenuData> _menuStack = new Stack<MenuData>();
        private Rect _menuBarRect;
        private string _menuId;
        private const float AnimationDuration = DesignTokens.Animation.DurationFast;

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
                Options = System.Array.Empty<GUILayoutOption>();
            }
        }

        #region Config-based API
        public void Draw(MenuBarConfig config)
        {
            Draw(config.Items, config.Options);
        }
        #endregion

        #region API
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

                DrawMenuBarItem(item, i, styleManager);
            }

            layoutComponents.EndHorizontalGroup();
            _menuBarRect = GUILayoutUtility.GetLastRect();

            if (_isDropdownOpen && _menuStack.Count > 0)
                DrawDropdownMenu();

            HandleClickOutside();
        }

        public void CloseDropdown()
        {
            if (_menuId != null)
            {
                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeOut($"menubar_alpha_{_menuId}", AnimationDuration * 0.8f, EasingFunctions.EaseInCubic);
                animManager.ScaleOut($"menubar_scale_{_menuId}", AnimationDuration * 0.8f, 0.92f, EasingFunctions.EaseInCubic);
            }
            _activeMenuIndex = -1;
            _isDropdownOpen = false;
            _menuStack.Clear();
            _menuId = null;
        }
        #endregion

        #region Private Methods
        private void DrawMenuBarItem(MenuItem item, int index, StyleManager styleManager)
        {
            var itemStyle = styleManager.GetMenuBarItemStyle(ControlVariant.Default, ControlSize.Default, active: _isDropdownOpen && _activeMenuIndex == index);

            var wasEnabled = GUI.enabled;
            if (item.Disabled)
                GUI.enabled = false;

            var clicked = UnityHelpers.Button(item.Text, itemStyle, GUILayout.ExpandWidth(false));

            GUI.enabled = wasEnabled;

            if (clicked)
                HandleMenuItemClick(item, index);
        }

        private void HandleMenuItemClick(MenuItem item, int index)
        {
            if (item.SubItems.Count > 0)
            {
                _activeMenuIndex = index;
                _menuId = $"menu_{index}";
                _menuStack.Clear();
                _menuStack.Push(new MenuData(item.SubItems, index));
                _isDropdownOpen = true;

                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeIn($"menubar_alpha_{_menuId}", AnimationDuration, EasingFunctions.EaseOutCubic);
                animManager.ScaleIn($"menubar_scale_{_menuId}", AnimationDuration, 0.92f, EasingFunctions.EaseOutCubic);
                animManager.SlideIn($"menubar_slide_{_menuId}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.MD), AnimationDuration, EasingFunctions.EaseOutCubic);
            }
            else
            {
                item.OnClick?.Invoke();
                CloseDropdown();
            }
        }

        private void DrawDropdownMenu()
        {
            var styleManager = guiHelper.GetStyleManager();
            var animManager = guiHelper.GetAnimationManager();
            var currentMenu = _menuStack.Peek();

            float alpha = animManager.GetFloat($"menubar_alpha_{_menuId}", 1f);
            float scale = animManager.GetFloat($"menubar_scale_{_menuId}", 1f);
            Vector2 slideOffset = animManager.GetVector2($"menubar_slide_{_menuId}", Vector2.zero);

            ApplyMenuAnimation(alpha, scale, slideOffset);
            DrawMenuContent(currentMenu, styleManager);
            RestoreGraphicsState();
        }

        private void ApplyMenuAnimation(float alpha, float scale, Vector2 slideOffset)
        {
            Color prevColor = GUI.color;
            if (alpha < 1f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * alpha);

            if (scale < 1f || slideOffset != Vector2.zero)
            {
                GUIUtility.ScaleAroundPivot(new Vector3(scale, scale, 1f), Vector2.zero);
                GUI.matrix = Matrix4x4.Translate(new Vector3(slideOffset.x, slideOffset.y, 0f)) * GUI.matrix;
            }
        }

        private void DrawMenuContent(MenuData currentMenu, StyleManager styleManager)
        {
            layoutComponents.BeginVerticalGroup(styleManager.GetMenuDropdownStyle(), GUILayout.Width(220 * guiHelper.uiScale));

            if (_menuStack.Count > 1)
            {
                if (UnityHelpers.Button("<- Back", styleManager.GetMenuBarItemStyle(ControlVariant.Default, ControlSize.Default)))
                {
                    _menuStack.Pop();
                    if (_menuStack.Count == 1)
                        _activeMenuIndex = _menuStack.Peek().ParentIndex;
                    layoutComponents.EndVerticalGroup();
                    GUI.matrix = Matrix4x4.identity;
                    GUI.color = Color.white;
                    return;
                }

                var separatorStyle = styleManager.GetSeparatorStyle(SeparatorOrientation.Horizontal);
                GUILayout.Box("", separatorStyle, GUILayout.Height(1 * guiHelper.uiScale), GUILayout.ExpandWidth(true));
            }

            foreach (var item in currentMenu.Items)
                DrawMenuItem(item, styleManager);

            layoutComponents.EndVerticalGroup();
        }

        private void DrawMenuItem(MenuItem item, StyleManager styleManager)
        {
            if (item.IsHeader)
            {
                UnityHelpers.Label(item.Text, styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default));
                return;
            }

            if (item.IsSeparator)
            {
                var separatorStyle = styleManager.GetSeparatorStyle(SeparatorOrientation.Horizontal);
                GUILayout.Box("", separatorStyle, GUILayout.Height(1 * guiHelper.uiScale), GUILayout.ExpandWidth(true));
                return;
            }

            var wasEnabled = GUI.enabled;
            if (item.Disabled)
                GUI.enabled = false;

            if (item.SubItems.Count > 0)
            {
                if (GUILayout.Button(item.Text, styleManager.GetMenuBarItemStyle(ControlVariant.Default, ControlSize.Default), GUILayout.ExpandWidth(true)))
                    _menuStack.Push(new MenuData(item.SubItems, _activeMenuIndex));
            }
            else
            {
                DrawMenuItemButton(item, styleManager);
            }

            GUI.enabled = wasEnabled;
        }

        private void DrawMenuItemButton(MenuItem item, StyleManager styleManager)
        {
            var buttonStyle = styleManager.GetMenuBarItemStyle(ControlVariant.Default, ControlSize.Default);
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
                var shortcutStyle = styleManager.GetMenuBarItemStyle(ControlVariant.Default, ControlSize.Default, true);
                GUI.Label(rect, item.Shortcut, shortcutStyle);
            }
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

        private void RestoreGraphicsState()
        {
            GUI.matrix = Matrix4x4.identity;
            GUI.color = Color.white;
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
        #endregion
    }
}
