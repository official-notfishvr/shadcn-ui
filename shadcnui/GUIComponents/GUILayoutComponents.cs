using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUILayoutComponents
    {
        private GUIHelper guiHelper;

        public GUILayoutComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public Vector2 DrawScrollView(Vector2 scrollPosition, Action drawContent, params GUILayoutOption[] options)
        {
#if IL2CPP
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
#if IL2CPP
            GUILayout.BeginHorizontal(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.BeginHorizontal();
#endif
        }

        public void BeginHorizontalGroup(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP
            GUILayout.BeginHorizontal(style, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            GUILayout.BeginHorizontal(style, options);
#endif
        }

        public void EndHorizontalGroup()
        {
            GUILayout.EndHorizontal();
        }

        public void BeginVerticalGroup(params GUILayoutOption[] options)
        {
            GUIStyle boxStyle = guiHelper.borderEffectsEnabled ? GUI.skin.box : GUIStyle.none;
#if IL2CPP
            GUILayout.BeginVertical(boxStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
            GUILayout.BeginVertical(boxStyle, options);
#endif
        }

        public void BeginVerticalGroup(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP
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
