using shadcnui;
using System;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUILabelComponents
    {
        private GUIHelper guiHelper;

        public GUILabelComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }
        public void Label(string text, LabelVariant variant = LabelVariant.Default, bool disabled = false,
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager.GetLabelStyle(variant);

           
            GUIStyle scaledStyle = new GUIStyle(labelStyle);
            scaledStyle.fontSize = Mathf.RoundToInt(labelStyle.fontSize * guiHelper.uiScale);

           
            if (disabled)
            {
                Color disabledColor = new Color(labelStyle.normal.textColor.r,
                    labelStyle.normal.textColor.g, labelStyle.normal.textColor.b, 0.7f);
                scaledStyle.normal.textColor = disabledColor;
            }

           
            GUILayoutOption[] autoScaledOptions = GetAutoScaledOptions(options);

#if IL2CPP
            GUILayout.Label(text ?? "", scaledStyle, (Il2CppReferenceArray<GUILayoutOption>)autoScaledOptions);
#else
            GUILayout.Label(text ?? "", scaledStyle, autoScaledOptions);
#endif
        }
        public void Label(Rect rect, string text, LabelVariant variant = LabelVariant.Default, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle labelStyle = styleManager.GetLabelStyle(variant);

           
            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            GUIStyle scaledStyle = new GUIStyle(labelStyle);
            scaledStyle.fontSize = Mathf.RoundToInt(labelStyle.fontSize * guiHelper.uiScale);

           
            if (disabled)
            {
                Color disabledColor = new Color(labelStyle.normal.textColor.r,
                    labelStyle.normal.textColor.g, labelStyle.normal.textColor.b, 0.7f);
                scaledStyle.normal.textColor = disabledColor;
            }

            GUI.Label(scaledRect, text ?? "", scaledStyle);
        }
        public void SecondaryLabel(string text, params GUILayoutOption[] options)
        {
            Label(text, LabelVariant.Secondary, false, options);
        }
        public void MutedLabel(string text, params GUILayoutOption[] options)
        {
            Label(text, LabelVariant.Muted, false, options);
        }
        public void DestructiveLabel(string text, params GUILayoutOption[] options)
        {
            Label(text, LabelVariant.Destructive, false, options);
        }
        private GUILayoutOption[] GetAutoScaledOptions(GUILayoutOption[] userOptions)
        {
            if (userOptions == null || userOptions.Length == 0)
                return new GUILayoutOption[0];

           
           
            return userOptions;
        }
    }
}
