
using System;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIPopoverComponents
    {
        private GUIHelper guiHelper;
        private bool isOpen;

        public GUIPopoverComponents(GUIHelper helper)
        {
            this.guiHelper = helper;
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

        public void Popover(Action content)
        {
            if (!isOpen) return;

            GUILayout.BeginVertical(guiHelper.GetStyleManager().popoverContentStyle, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
            content?.Invoke();
            GUILayout.EndVertical();
        }
    }
}
