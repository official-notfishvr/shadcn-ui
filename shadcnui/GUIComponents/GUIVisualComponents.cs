using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIVisualComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;
        private static float horizontalPadding = 10f;

        public GUIVisualComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public void DrawProgressBar(float windowWidth, string label, float progress, Color barColor)
        {
            var styleManager = guiHelper.GetStyleManager();
            progress = Mathf.Clamp01(progress);

#if IL2CPP
            GUILayout.Label(label + ": " + (progress * 100f).ToString("F0") + "%", styleManager.glowLabelStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(label + ": " + (progress * 100f).ToString("F0") + "%", styleManager.glowLabelStyle);
#endif

            Rect progressRect = GUILayoutUtility.GetRect((windowWidth - 2 * horizontalPadding) * guiHelper.uiScale, 20 * guiHelper.uiScale);

           
            GUI.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            GUI.DrawTexture(progressRect, Texture2D.whiteTexture);

           
            Color finalBarColor = guiHelper.customColorsEnabled ?
                Color.Lerp(barColor, guiHelper.accentColor, 0.5f) : barColor;
            GUI.color = finalBarColor;
            Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * progress, progressRect.height);
            GUI.DrawTexture(fillRect, Texture2D.whiteTexture);

           
            GUI.color = Color.white;
            if (guiHelper.borderEffectsEnabled)
            {
                GUI.color = guiHelper.customColorsEnabled ? guiHelper.primaryColor : Color.white;
            }
            GUI.Box(progressRect, "", GUI.skin.box);

            layoutComponents.AddSpace(guiHelper.controlSpacing);
        }

        public void DrawBox(float windowWidth, string content, float height = 30f)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.Box(content, styleManager.animatedInputStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Height(height * guiHelper.uiScale) });
#else
            GUILayout.Box(content, styleManager.animatedInputStyle, GUILayout.Height(height * guiHelper.uiScale));
#endif
            layoutComponents.AddSpace(guiHelper.controlSpacing);
        }

        public void DrawSeparator(float windowWidth, float height = 2f)
        {
#if IL2CPP
            Rect rect = GUILayoutUtility.GetRect(
                (windowWidth - 2 * horizontalPadding) * guiHelper.uiScale,
                height * guiHelper.uiScale,
                (Il2CppReferenceArray<GUILayoutOption>)null
            );
#else
            Rect rect = GUILayoutUtility.GetRect(
                (windowWidth - 2 * horizontalPadding) * guiHelper.uiScale,
                height * guiHelper.uiScale
            );
#endif
            Color originalColor = GUI.backgroundColor;
            Color separatorColor = guiHelper.customColorsEnabled ?
                Color.Lerp(Color.gray, guiHelper.primaryColor, 0.5f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            GUI.backgroundColor = separatorColor;
            GUI.Box(rect, "");
            GUI.backgroundColor = originalColor;
            layoutComponents.AddSpace(guiHelper.controlSpacing);
        }

        public void RenderInstructions(string text)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle instructionStyle = new GUIStyle(styleManager.glowLabelStyle);
            instructionStyle.fontSize = Mathf.RoundToInt(10 * guiHelper.uiScale);
            Color instructionColor = guiHelper.customColorsEnabled ?
                Color.Lerp(new Color(0.8f, 0.8f, 0.8f), guiHelper.accentColor, 0.3f) : new Color(0.8f, 0.8f, 0.8f);
            instructionStyle.normal.textColor = instructionColor;

#if IL2CPP
            layoutComponents.BeginHorizontalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
            GUILayout.FlexibleSpace();
            GUILayout.Label(text, instructionStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
            GUILayout.FlexibleSpace();
            layoutComponents.EndHorizontalGroup();
#else
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            GUILayout.Label(text, instructionStyle);
            GUILayout.FlexibleSpace();
            layoutComponents.EndHorizontalGroup();
#endif
        }
    }
}

