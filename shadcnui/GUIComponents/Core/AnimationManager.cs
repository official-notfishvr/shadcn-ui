using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    public class AnimationManager
    {
        private GUIHelper guiHelper;
        private shadcnui.GUIComponents.Layout.Layout layoutComponents;
        private bool _layoutGroupStarted = false;

        public AnimationManager(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new shadcnui.GUIComponents.Layout.Layout(helper);
        }

        public void UpdateAnimations(bool isOpen, ref float menuAlpha, ref float menuScale, ref float backgroundPulse, ref float particleTime, ref Vector2 mousePos)
        {
            if (!guiHelper.animationsEnabled)
                return;

            float speed = guiHelper.smoothAnimationsEnabled ? guiHelper.animationSpeed : guiHelper.animationSpeed * 0.5f;

            float targetAlpha = isOpen ? 1f : 0f;
            menuAlpha = Mathf.Lerp(menuAlpha, targetAlpha, Time.deltaTime * speed * 0.67f);

            float targetScale = isOpen ? guiHelper.uiScale : (guiHelper.uiScale * 0.8f);
            menuScale = Mathf.Lerp(menuScale, targetScale, Time.deltaTime * speed * 0.83f);

            backgroundPulse = guiHelper.backgroundAlpha;

            if (guiHelper.particleEffectsEnabled)
                particleTime += Time.deltaTime;

            mousePos = Event.current.mousePosition;
        }

        public bool BeginAnimatedGUI(float menuAlpha, float backgroundPulse, Vector2 mousePos)
        {
            try
            {
                float currentAlpha = guiHelper.fadeTransitionsEnabled ? menuAlpha : 1f;

                float pulseAlpha = guiHelper.animationsEnabled ? backgroundPulse : guiHelper.backgroundAlpha;
                Color bgColor = ThemeManager.Instance.CurrentTheme.BackgroundColor;
                Color backgroundColor = new Color(bgColor.r, bgColor.g, bgColor.b, currentAlpha * pulseAlpha);

                GUI.color = backgroundColor;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);

                if (guiHelper.particleEffectsEnabled)
                    RenderBackgroundParticles(currentAlpha);

                GUI.color = new Color(1f, 1f, 1f, currentAlpha);

                var styleManager = guiHelper.GetStyleManager();
#if IL2CPP_MELONLOADER_PRE57
                layoutComponents.BeginVerticalGroup(styleManager.animatedBoxStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginVerticalGroup(styleManager.GetAnimatedBoxStyle());
#endif
                GUI.color = new Color(1f, 1f, 1f, currentAlpha);
                _layoutGroupStarted = true;
                return true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "AnimationManager", "AnimationManager");
                return false;
            }
        }

        public void EndAnimatedGUI()
        {
            if (_layoutGroupStarted)
            {
                layoutComponents.EndVerticalGroup();
                _layoutGroupStarted = false;
            }
            GUI.color = Color.white;
        }

        private void RenderBackgroundParticles(float menuAlpha)
        {
            if (!guiHelper.particleEffectsEnabled)
                return;

            var styleManager = guiHelper.GetStyleManager();
            var particleTexture = styleManager.GetParticleTexture();
            var particleTime = guiHelper.GetParticleTime();

            int particleCount = 12;
            float scale = guiHelper.uiScale;

            for (int i = 0; i < particleCount; i++)
            {
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;

                float x = (screenWidth / particleCount) * i + Mathf.Sin(particleTime * 0.5f + i * 0.8f) * 25;
                float y = screenHeight / 2 + Mathf.Cos(particleTime * 0.3f + i * 1.2f) * (screenHeight / 4);

                float alpha = (Mathf.Sin(particleTime * 2f + i) * 0.3f + 0.4f) * menuAlpha * guiHelper.glowIntensity;
                alpha = Mathf.Clamp01(alpha);

                Color particleColor = new Color(ThemeManager.Instance.CurrentTheme.Accent.r, ThemeManager.Instance.CurrentTheme.Accent.g, ThemeManager.Instance.CurrentTheme.Accent.b, alpha);

                GUI.color = particleColor;
                GUI.DrawTexture(new Rect(x, y, 4 * scale, 4 * scale), particleTexture);
            }

            GUI.color = Color.white;
        }

        public void Cleanup() { }
    }
}
