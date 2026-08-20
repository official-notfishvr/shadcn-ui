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
        private const float DropdownWidth = 220f;
        private const float MenuItemHeight = 32f;
        private const float HeaderHeight = 24f;
        private const float MenuPadding = 8f;
        private readonly string _layerId = $"menubar_layer_{Guid.NewGuid():N}";
        private int _activeMenuIndex = -1;
        private bool _isDropdownOpen = false;
        private readonly Stack<MenuData> _menuStack = new Stack<MenuData>();
        private readonly Dictionary<int, Rect> _menuItemRects = new Dictionary<int, Rect>();
        private ControlVariant _currentVariant = ControlVariant.Default;
        private ControlSize _currentSize = ControlSize.Default;
        private ComponentAppearance _currentAppearance;

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

        public class MenuBarConfig : Core.Utils.ComponentConfigBase
        {
            public List<MenuItem> Items { get; set; }

            public MenuBarConfig()
            {
                Items = new List<MenuItem>();
                LayoutOptions = System.Array.Empty<GUILayoutOption>();
            }

            public MenuBarConfig(List<MenuItem> items)
            {
                Items = items;
                LayoutOptions = System.Array.Empty<GUILayoutOption>();
            }
        }

        #region Config-based API
        public void Render(MenuBarConfig config)
        {
            Draw(config.Items, config.Variant, config.Size, config.Appearance, config.LayoutOptions);
        }
        #endregion

        #region API
        internal void Draw(List<MenuItem> items, params GUILayoutOption[] options)
        {
            Draw(items, ControlVariant.Default, ControlSize.Default, null, options);
        }

        internal void Draw(List<MenuItem> items, ControlVariant variant, ControlSize size, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            if (items == null || items.Count == 0)
                return;

            var styleManager = guiHelper.GetStyleManager();
            _currentVariant = variant;
            _currentSize = size;
            _currentAppearance = appearance;
            if (Event.current.type == EventType.Repaint)
                _menuItemRects.Clear();

            layoutComponents.BeginHorizontalGroup(styleManager.GetMenuBarStyle(variant, size, appearance), options);

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.IsSeparator || item.IsHeader)
                    continue;

                DrawMenuBarItem(item, i, styleManager, variant, size, appearance);
            }

            layoutComponents.EndHorizontalGroup();
            if (_isDropdownOpen && _menuStack.Count > 0)
                RefreshDropdownLayer();
            else
                CloseDropdownLayer();
        }

        public void CloseDropdown()
        {
            ResetDropdownState();
            CloseDropdownLayer();
        }

        private void ResetDropdownState()
        {
            _activeMenuIndex = -1;
            _isDropdownOpen = false;
            _menuStack.Clear();
        }
        #endregion

        #region Private Methods
        private void DrawMenuBarItem(MenuItem item, int index, StyleManager styleManager, ControlVariant variant, ControlSize size, ComponentAppearance appearance)
        {
            var itemStyle = styleManager.GetMenuBarItemStyle(variant, size, active: _isDropdownOpen && _activeMenuIndex == index, appearance: appearance);

            var wasEnabled = GUI.enabled;
            if (item.Disabled)
                GUI.enabled = false;

            var clicked = UnityHelpers.Button(item.Text, itemStyle, GUILayout.ExpandWidth(false));
            if (Event.current.type == EventType.Repaint)
                _menuItemRects[index] = PopupLayoutUtility.ToScreenRect(GUILayoutUtility.GetLastRect());

            GUI.enabled = wasEnabled;

            if (clicked)
                HandleMenuItemClick(item, index);
        }

        private void HandleMenuItemClick(MenuItem item, int index)
        {
            if (item.SubItems.Count > 0)
            {
                if (_isDropdownOpen && _activeMenuIndex == index)
                {
                    CloseDropdown();
                    return;
                }

                _activeMenuIndex = index;
                _menuStack.Clear();
                _menuStack.Push(new MenuData(item.SubItems, index));
                _isDropdownOpen = true;
                RefreshDropdownLayer();
            }
            else
            {
                item.OnClick?.Invoke();
                CloseDropdown();
            }
        }

        private void DrawDropdownMenu(float width, float height)
        {
            if (!_isDropdownOpen || _menuStack.Count == 0)
            {
                CloseDropdownLayer();
                return;
            }

            var styleManager = guiHelper.GetStyleManager();
            var currentMenu = _menuStack.Peek();
            layoutComponents.BeginVerticalGroup(styleManager.GetMenuDropdownStyle(_currentVariant, _currentSize, _currentAppearance), GUILayout.Width(width), GUILayout.Height(height));

            if (_menuStack.Count > 1)
            {
                if (UnityHelpers.Button("<- Back", styleManager.GetMenuBarItemStyle(_currentVariant, _currentSize, appearance: _currentAppearance), GUILayout.Height(MenuItemHeight * guiHelper.uiScale)))
                {
                    _menuStack.Pop();
                    if (_menuStack.Count == 1)
                        _activeMenuIndex = _menuStack.Peek().ParentIndex;
                    layoutComponents.EndVerticalGroup();
                    RefreshDropdownLayer();
                    return;
                }

                var separatorStyle = styleManager.GetSeparatorStyle(SeparatorOrientation.Horizontal, _currentVariant, _currentSize, _currentAppearance);
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
                UnityHelpers.Label(item.Text, styleManager.GetLabelStyle(ControlVariant.Muted, _currentSize, _currentAppearance));
                return;
            }

            if (item.IsSeparator)
            {
                var separatorStyle = styleManager.GetSeparatorStyle(SeparatorOrientation.Horizontal, _currentVariant, _currentSize, _currentAppearance);
                GUILayout.Box("", separatorStyle, GUILayout.Height(1 * guiHelper.uiScale), GUILayout.ExpandWidth(true));
                return;
            }

            var wasEnabled = GUI.enabled;
            if (item.Disabled)
                GUI.enabled = false;

            if (item.SubItems.Count > 0)
            {
                if (GUILayout.Button(item.Text, styleManager.GetMenuBarItemStyle(_currentVariant, _currentSize, appearance: _currentAppearance), GUILayout.ExpandWidth(true), GUILayout.Height(MenuItemHeight * guiHelper.uiScale)))
                {
                    _menuStack.Push(new MenuData(item.SubItems, _activeMenuIndex));
                    RefreshDropdownLayer();
                }
            }
            else
            {
                DrawMenuItemButton(item, styleManager);
            }

            GUI.enabled = wasEnabled;
        }

        private void DrawMenuItemButton(MenuItem item, StyleManager styleManager)
        {
            var buttonStyle = styleManager.GetMenuBarItemStyle(_currentVariant, _currentSize, appearance: _currentAppearance);
            var textStyle = styleManager.GetMenuBarItemStyle(_currentVariant, _currentSize, appearance: _currentAppearance);

            Rect rect = ControlLayoutUtility.ReserveRect(UnityHelpers.GUIContent.none, buttonStyle, ControlLayoutUtility.BuildLayoutOptions(null, fixedHeight: MenuItemHeight * guiHelper.uiScale, expandWidth: true));

            if (GUI.Button(rect, "", buttonStyle))
            {
                item.OnClick?.Invoke();
                CloseDropdown();
            }

            GUI.Label(rect, item.Text, textStyle);

            if (!string.IsNullOrEmpty(item.Shortcut))
            {
                var shortcutStyle = styleManager.GetMenuBarItemStyle(_currentVariant, _currentSize, isShortcut: true, appearance: _currentAppearance);
                GUI.Label(rect, item.Shortcut, shortcutStyle);
            }
        }

        private void RefreshDropdownLayer()
        {
            if (!_isDropdownOpen || _menuStack.Count == 0 || !_menuItemRects.TryGetValue(_activeMenuIndex, out var anchorRect))
            {
                CloseDropdownLayer();
                return;
            }

            var rootRect = guiHelper.GetRootGuiScreenRect();
            var anchorScreen = new Vector2(anchorRect.x, anchorRect.yMax + DesignTokens.Spacing.XS * guiHelper.uiScale);
            var width = GetDropdownWidth();
            var height = GetDropdownHeight(_menuStack.Peek());
            var position = new Vector2(anchorScreen.x, anchorScreen.y);

            if (position.x + width > rootRect.xMax)
                position.x = Mathf.Max(rootRect.xMin, rootRect.xMax - width);
            if (position.y + height > rootRect.yMax)
                position.y = Mathf.Max(rootRect.yMin, anchorScreen.y - height - anchorRect.height);

            LayerManager.Instance.Open(
                new Core.Utils.LayerConfig
                {
                    Id = GetLayerId(),
                    OpenPosition = position,
                    Width = width,
                    Height = height,
                    ZIndex = DesignTokens.ZIndex.Dropdown,
                    CloseOnClickOutside = true,
                    DrawChrome = false,
                    Content = () => DrawDropdownMenu(width, height),
                    OnClose = ResetDropdownState,
                }
            );
        }

        private void CloseDropdownLayer()
        {
            if (LayerManager.Instance.IsOpen(_layerId))
                LayerManager.Instance.Close(_layerId);
        }

        private string GetLayerId() => _layerId;

        private float GetDropdownWidth() => DropdownWidth * guiHelper.uiScale;

        private float GetDropdownHeight(MenuData menu)
        {
            float height = MenuPadding * 2f * guiHelper.uiScale;

            if (_menuStack.Count > 1)
                height += MenuItemHeight * guiHelper.uiScale + 1f * guiHelper.uiScale + DesignTokens.Spacing.XS * guiHelper.uiScale;

            foreach (var item in menu.Items)
            {
                if (item.IsSeparator)
                    height += 1f * guiHelper.uiScale + DesignTokens.Spacing.XS * guiHelper.uiScale;
                else if (item.IsHeader)
                    height += HeaderHeight * guiHelper.uiScale;
                else
                    height += MenuItemHeight * guiHelper.uiScale;
            }

            return Mathf.Max(MenuItemHeight * guiHelper.uiScale, height);
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
