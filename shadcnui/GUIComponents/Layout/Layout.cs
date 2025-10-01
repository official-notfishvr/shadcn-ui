using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Layout
    {
#if IL2CPP_MELONLOADER
        public static readonly Il2CppReferenceArray<GUILayoutOption> EmptyOptions = new Il2CppReferenceArray<GUILayoutOption>(0);
#endif
        private GUIHelper guiHelper;

        public Layout(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public Vector2 DrawScrollView(Vector2 scrollPosition, Action drawContent, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, options);
#endif
            drawContent?.Invoke();
            GUILayout.EndScrollView();
            return scrollPosition;
        }

        public void BeginHorizontalGroup()
        {
#if IL2CPP_MELONLOADER
            GUILayout.BeginHorizontal(GUIStyle.none, EmptyOptions);
#else
            GUILayout.BeginHorizontal();
#endif
        }

        public void BeginHorizontalGroup(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER
            GUILayout.BeginHorizontal(style, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            GUILayout.BeginHorizontal(style, options);
#endif
        }

        public void EndHorizontalGroup()
        {
            GUILayout.EndHorizontal();
        }

        public void BeginVerticalGroup()
        {
            GUIStyle boxStyle = GUIStyle.none;
#if IL2CPP_MELONLOADER
            GUILayout.BeginVertical(boxStyle, EmptyOptions);
#else
            GUILayout.BeginVertical(boxStyle);
#endif
        }

        public void BeginVerticalGroup(params GUILayoutOption[] options)
        {
            GUIStyle boxStyle = GUIStyle.none;
#if IL2CPP_MELONLOADER
            GUILayout.BeginVertical(boxStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            GUILayout.BeginVertical(boxStyle, options);
#endif
        }

        public void BeginVerticalGroup(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER
            GUILayout.BeginVertical(style, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            GUILayout.BeginVertical(style, options);
#endif
        }

        public void EndVerticalGroup()
        {
            GUILayout.EndVertical();
        }

        public void AddSpace(float pixels)
        {
            GUILayout.Space(pixels * guiHelper.uiScale);
        }
    }
}
