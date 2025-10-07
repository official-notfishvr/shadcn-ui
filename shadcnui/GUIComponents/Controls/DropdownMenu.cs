using System;
using System.Collections.Generic;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
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

    public class DropdownMenu
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;
        private bool isOpen;
        private Vector2 scrollPosition;
        private Stack<List<DropdownMenuItem>> menuStack = new Stack<List<DropdownMenuItem>>();

        public DropdownMenu(GUIHelper helper)
        {
            this.guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public bool IsOpen => isOpen;

        public void Open(List<DropdownMenuItem> rootItems)
        {
            menuStack.Clear();
            menuStack.Push(rootItems);
            isOpen = true;
        }

        public void Close()
        {
            menuStack.Clear();
            isOpen = false;
        }

        public void DrawDropdownMenu()
        {
            if (!isOpen || menuStack.Count == 0)
                return;

            var styleManager = guiHelper.GetStyleManager();

            layoutComponents.BeginVerticalGroup(styleManager.dropdownMenuContentStyle, GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200));
            scrollPosition = layoutComponents.DrawScrollView(
                scrollPosition,
                () =>
                {
                    if (menuStack.Count > 1)
                    {
                        if (UnityHelpers.Button("<- Back", styleManager.dropdownMenuItemStyle))
                        {
                            menuStack.Pop();
                            return;
                        }

                        UnityHelpers.Box("", styleManager.dropdownMenuSeparatorStyle);
                    }

                    var currentItems = menuStack.Peek();
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

            switch (item.Type)
            {
                case DropdownMenuItemType.Header:
#if IL2CPP_MELONLOADER
                    GUILayout.Label(item.Content, styleManager.dropdownMenuHeaderStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
                    GUILayout.Label(item.Content, styleManager.dropdownMenuHeaderStyle);
#endif
                    break;
                case DropdownMenuItemType.Separator:
#if IL2CPP_MELONLOADER
                    GUILayout.Box("", styleManager.dropdownMenuSeparatorStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
                    GUILayout.Box("", styleManager.dropdownMenuSeparatorStyle);
#endif
                    break;
                case DropdownMenuItemType.Item:
                    if (item.SubItems != null && item.SubItems.Count > 0)
                    {
#if IL2CPP_MELONLOADER
                        if (GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
                        if (GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle))
#endif
                        {
                            menuStack.Push(item.SubItems);
                        }
                    }
                    else
                    {
#if IL2CPP_MELONLOADER
                        if (GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
                        if (GUILayout.Button(item.Content, styleManager.dropdownMenuItemStyle))
#endif
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
