using System;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Popover : BaseComponent
    {
        private bool isOpen;

        public Popover(GUIHelper helper)
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

        public void DrawPopover(Action content)
        {
            if (!isOpen)
                return;

            layoutComponents.BeginVerticalGroup(guiHelper.GetStyleManager().GetPopoverContentStyle(), GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));

            content?.Invoke();
            GUILayout.EndVertical();
        }
    }
}
