using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Select : BaseComponent
    {
        private bool isOpen;
        private int selectedIndex;
        private Vector2 scrollPosition;

        public Select(GUIHelper helper) : base(helper) { }

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

            layoutComponents.BeginVerticalGroup(guiHelper.GetStyleManager().GetSelectStyle(SelectVariant.Default, SelectSize.Default), GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));

            scrollPosition = layoutComponents.DrawScrollView(
                scrollPosition,
                () =>
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        GUIStyle itemStyle = guiHelper.GetStyleManager().GetSelectItemStyle();

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
