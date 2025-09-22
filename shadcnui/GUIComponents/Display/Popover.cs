using System;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Popover
    {
        private GUIHelper guiHelper;
        private bool isOpen;

        public Popover(GUIHelper helper)
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

        public void DrawPopover(Action content)
        {
            if (!isOpen)
                return;

#if IL2CPP
            GUILayout.BeginVertical(guiHelper.GetStyleManager().popoverContentStyle, new UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.GUILayoutOption>(new GUILayoutOption[] { GUILayout.MaxWidth(300), GUILayout.MaxHeight(200) }));
#else
            GUILayout.BeginVertical(guiHelper.GetStyleManager().popoverContentStyle, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
#endif
            content?.Invoke();
            GUILayout.EndVertical();
        }
    }
}
