using System;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Popover
    {
        private GUIHelper guiHelper;
        private bool isOpen;
        private Layout _layoutComponents;

        public Popover(GUIHelper helper)
        {
            this.guiHelper = helper;
            _layoutComponents = new Layout(helper);
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

        public void DrawPopover(Action content)
        {
            if (!isOpen)
                return;

            _layoutComponents.BeginVerticalGroup(guiHelper.GetStyleManager().popoverContentStyle, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));

            content?.Invoke();
            GUILayout.EndVertical();
        }
    }
}
