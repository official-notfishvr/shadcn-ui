using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUISliderComponents
    {
        private GUIHelper guiHelper;

        public GUISliderComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void DrawSlider(float windowWidth, string label, ref float value, float minValue, float maxValue)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.Label(label + ": " + value.ToString("F2"), styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            value = GUILayout.HorizontalSlider(value, minValue, maxValue, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label + ": " + value.ToString("F2"), styleManager.glowLabelStyle);
            value = GUILayout.HorizontalSlider(value, minValue, maxValue);
#endif
            GUILayout.Space(guiHelper.controlSpacing);
        }

        public void DrawIntSlider(float windowWidth, string label, ref int value, int minValue, int maxValue)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.Label(label + ": " + value.ToString(), styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label + ": " + value.ToString(), styleManager.glowLabelStyle);
            value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue);
#endif
            GUILayout.Space(guiHelper.controlSpacing);
        }

    }
}

