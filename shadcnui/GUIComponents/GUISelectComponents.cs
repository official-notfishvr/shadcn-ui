using System;
using System.Collections.Generic;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUISelectComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;
        private bool isOpen;
        private int selectedIndex;
        private Vector2 scrollPosition;

        public GUISelectComponents(GUIHelper helper)
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

        public int Select(string[] items, int selectedIndex)
        {
            if (!isOpen) return selectedIndex;

#if IL2CPP
            GUILayout.BeginVertical(guiHelper.GetStyleManager().GetSelectStyle(SelectVariant.Default, SelectSize.Default), new UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.GUILayoutOption>(new GUILayoutOption[] { GUILayout.MaxWidth(300), GUILayout.MaxHeight(200) }));
#else
            GUILayout.BeginVertical(guiHelper.GetStyleManager().GetSelectStyle(SelectVariant.Default, SelectSize.Default), GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
#endif

            scrollPosition = layoutComponents.DrawScrollView(scrollPosition, () =>
            {
                for (int i = 0; i < items.Length; i++)
                {
                    GUIStyle itemStyle = guiHelper.GetStyleManager().GetSelectItemStyle();
                    if (i == selectedIndex)
                    {
                        itemStyle = new GUIStyle(itemStyle);
                        itemStyle.normal.textColor = Color.blue;
                    }

#if IL2CPP
                    if (GUILayout.Button(new GUIContent(items[i]), itemStyle, new UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.GUILayoutOption>(0)))
#else
                    if (GUILayout.Button(items[i], itemStyle))
#endif
                    {
                        selectedIndex = i;
                        Close();
                    }
                }
            }, GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200));

            GUILayout.EndVertical();

            return selectedIndex;
        }
    }
}
