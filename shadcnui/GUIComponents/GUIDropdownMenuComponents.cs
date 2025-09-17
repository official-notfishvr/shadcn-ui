
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIDropdownMenuComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;
        private bool isOpen;
        private Vector2 scrollPosition;

        public GUIDropdownMenuComponents(GUIHelper helper)
        {
            this.guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public bool IsOpen => isOpen;

        public void Open()
        {
            isOpen = true;
        }

        public void Close()
        {
            isOpen = false;
        }

        public void DropdownMenu(string[] items, Action<int> onItemSelected)
        {
            if (!isOpen) return;

            layoutComponents.BeginVerticalGroup(guiHelper.GetStyleManager().dropdownMenuContentStyle, GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200));
            scrollPosition = layoutComponents.DrawScrollView(scrollPosition, () =>
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (GUILayout.Button(items[i], guiHelper.GetStyleManager().dropdownMenuItemStyle))
                    {
                        onItemSelected?.Invoke(i);
                        Close();
                    }
                }
            }, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            layoutComponents.EndVerticalGroup();
        }
    }
}
