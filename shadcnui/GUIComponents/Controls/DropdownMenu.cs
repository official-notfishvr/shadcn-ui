using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public enum DropdownMenuItemType
    {
        Item,
        Separator,
        Header,
    }

    public class DropdownMenuItem
    {
        public DropdownMenuItemType Type { get; set; }
        public GUIContent Content { get; set; }
        public Action OnClick { get; set; }
        public bool IsSelected { get; set; }
        public List<DropdownMenuItem> SubItems { get; set; }

        public DropdownMenuItem(DropdownMenuItemType type, string text = null, Action onClick = null, bool isSelected = false, Texture2D icon = null)
        {
            Type = type;
            Content = new UnityHelpers.GUIContent(text, icon);
            OnClick = onClick;
            IsSelected = isSelected;
            SubItems = new List<DropdownMenuItem>();
        }
    }

    public class DropdownMenuConfig
    {
        public List<DropdownMenuItem> Items { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public DropdownMenuConfig(List<DropdownMenuItem> items)
        {
            Items = items;
        }
    }

    public class DropdownMenu
    {
        private readonly GUIHelper _guiHelper;
        private readonly shadcnui.GUIComponents.Layout.Layout _layoutComponents;
        private bool _isOpen;
        private Vector2 _scrollPosition;
        private readonly Stack<List<DropdownMenuItem>> _menuStack = new Stack<List<DropdownMenuItem>>();

        public DropdownMenu(GUIHelper helper)
        {
            _guiHelper = helper;
            _layoutComponents = new shadcnui.GUIComponents.Layout.Layout(helper);
        }

        public bool IsOpen => _isOpen;

        public void Open(List<DropdownMenuItem> rootItems)
        {
            _menuStack.Clear();
            _menuStack.Push(rootItems);
            _isOpen = true;
        }

        public void Close()
        {
            _menuStack.Clear();
            _isOpen = false;
        }

        public void Draw(DropdownMenuConfig config)
        {
            if (config?.Items == null || config.Items.Count == 0)
            {
                Close();
                return;
            }

            if (!_isOpen)
            {
                Open(config.Items);
            }

            DrawDropdownMenu(config.Options);
        }

        private void DrawDropdownMenu(GUILayoutOption[] options)
        {
            if (!_isOpen || _menuStack.Count == 0)
                return;

            var styleManager = _guiHelper.GetStyleManager();

            var layoutOptions = new List<GUILayoutOption> { GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200) };
            if (options != null)
            {
                layoutOptions.AddRange(options);
            }

            _layoutComponents.BeginVerticalGroup(styleManager.dropdownMenuContentStyle, layoutOptions.ToArray());
            _scrollPosition = _layoutComponents.DrawScrollView(
                _scrollPosition,
                () =>
                {
                    if (_menuStack.Count > 1)
                    {
                        if (UnityHelpers.Button("<- Back", styleManager.dropdownMenuItemStyle))
                        {
                            _menuStack.Pop();
                            return;
                        }

                        UnityHelpers.Box("", styleManager.dropdownMenuSeparatorStyle);
                    }

                    var currentItems = _menuStack.Peek();
                    foreach (var item in currentItems)
                    {
                        DrawMenuItem(item);
                    }
                },
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );
            _layoutComponents.EndVerticalGroup();
        }

        private void DrawMenuItem(DropdownMenuItem item)
        {
            var styleManager = _guiHelper.GetStyleManager();

            switch (item.Type)
            {
                case DropdownMenuItemType.Header:
#if IL2CPP_MELONLOADER
                    GUILayout.Label(item.Content, styleManager.dropdownMenuHeaderStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions);
#else
                    GUILayout.Label(item.Content, styleManager.dropdownMenuHeaderStyle);
#endif
                    break;
                case DropdownMenuItemType.Separator:
                    UnityHelpers.Box("", styleManager.dropdownMenuSeparatorStyle);
                    break;
                case DropdownMenuItemType.Item:
                    if (item.SubItems != null && item.SubItems.Count > 0)
                    {
                        if (
#if IL2CPP_MELONLOADER
                            GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions)
#else
                            GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle)
#endif
                        )
                        {
                            _menuStack.Push(item.SubItems);
                        }
                    }
                    else
                    {
                        if (
#if IL2CPP_MELONLOADER
                            GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions)
#else
                            GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle)
#endif
                        )
                        {
                            item.OnClick?.Invoke();
                            Close();
                        }
                    }
                    break;
            }
        }
    }
}
