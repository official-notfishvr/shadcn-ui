using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Slider : BaseComponent
    {
        public Slider(GUIHelper helper) : base(helper) { }
        public void DrawSlider(float windowWidth, string label, ref float value, float minValue, float maxValue)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP_MELONLOADER
            GUILayout.Label(label + ": " + value.ToString("F2"), styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            value = GUILayout.HorizontalSlider(value, minValue, maxValue, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label + ": " + value.ToString("F2"), styleManager.glowLabelStyle);
            value = GUILayout.HorizontalSlider(value, minValue, maxValue);
#endif
            layoutComponents.AddSpace(10f);
        }

        public void DrawIntSlider(float windowWidth, string label, ref int value, int minValue, int maxValue)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP_MELONLOADER
            GUILayout.Label(label + ": " + value.ToString(), styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label + ": " + value.ToString(), styleManager.glowLabelStyle);
            value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue);
#endif
            layoutComponents.AddSpace(10f);
        }
    }
}
