using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Select : BaseComponent
    {
        private bool isOpen;
        private int selectedIndex;
        private Vector2 scrollPosition;

        public Select(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => isOpen;

        public void Open()
        {
            isOpen = true;
        }

        public void Close()
        {
            isOpen = false;
        }

        public int DrawSelect(string[] items, int selectedIndex)
        {
            if (!isOpen)
                return selectedIndex;

            GUIStyle selectStyle = styleManager?.GetSelectStyle(SelectVariant.Default, SelectSize.Default) ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.GetSelectItemStyle() ?? GUI.skin.button;

            layoutComponents.BeginVerticalGroup(selectStyle, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));

            scrollPosition = layoutComponents.DrawScrollView(
                scrollPosition,
                () =>
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (UnityHelpers.Button(items[i], itemStyle))
                        {
                            selectedIndex = i;
                            Close();
                        }
                    }
                },
                GUILayout.ExpandWidth(true),
                GUILayout.MinHeight(0),
                GUILayout.MaxHeight(200)
            );

            GUILayout.EndVertical();

            return selectedIndex;
        }
    }
}
