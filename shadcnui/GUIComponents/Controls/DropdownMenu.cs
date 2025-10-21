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

    public class DropdownMenu : BaseComponent
    {
        private bool _isOpen;
        private Vector2 _scrollPosition;
        private readonly Stack<List<DropdownMenuItem>> _menuStack = new Stack<List<DropdownMenuItem>>();

        public DropdownMenu(GUIHelper helper)
            : base(helper) { }

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

            var styleManager = guiHelper.GetStyleManager();

            var layoutOptions = new List<GUILayoutOption> { GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200) };
            if (options != null)
            {
                layoutOptions.AddRange(options);
            }

            GUIStyle contentStyle = styleManager?.dropdownMenuContentStyle ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.dropdownMenuItemStyle ?? GUI.skin.button;
            GUIStyle separatorStyle = styleManager?.dropdownMenuSeparatorStyle ?? GUI.skin.box;

            layoutComponents.BeginVerticalGroup(contentStyle, layoutOptions.ToArray());
            _scrollPosition = layoutComponents.DrawScrollView(
                _scrollPosition,
                () =>
                {
                    if (_menuStack.Count > 1)
                    {
                        if (UnityHelpers.Button("<- Back", itemStyle))
                        {
                            _menuStack.Pop();
                            return;
                        }

                        UnityHelpers.Box("", separatorStyle);
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
            layoutComponents.EndVerticalGroup();
        }

        private void DrawMenuItem(DropdownMenuItem item)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle headerStyle = styleManager?.dropdownMenuHeaderStyle ?? GUI.skin.label;
            GUIStyle separatorStyle = styleManager?.dropdownMenuSeparatorStyle ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.dropdownMenuItemStyle ?? GUI.skin.button;

            switch (item.Type)
            {
                case DropdownMenuItemType.Header:
#if IL2CPP_MELONLOADER
                    GUILayout.Label(item.Content, headerStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions);
#else
                    GUILayout.Label(item.Content, headerStyle);
#endif
                    break;
                case DropdownMenuItemType.Separator:
                    UnityHelpers.Box("", separatorStyle);
                    break;
                case DropdownMenuItemType.Item:
                    if (item.SubItems != null && item.SubItems.Count > 0)
                    {
                        if (
#if IL2CPP_MELONLOADER
                            GUILayout.Button(item.Content, itemStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions)
#else
                            GUILayout.Button(item.Content, itemStyle)
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
                            GUILayout.Button(item.Content, itemStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions)
#else
                            GUILayout.Button(item.Content, itemStyle)
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
