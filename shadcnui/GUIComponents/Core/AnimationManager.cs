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
    public class AnimationManager
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;
        private bool _layoutGroupStarted = false;

        public AnimationManager(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public void UpdateAnimations(bool isOpen, ref float menuAlpha, ref float menuScale, ref float titleGlow, ref float backgroundPulse, ref int hoveredButton, float[] buttonGlowEffects, float[] inputFieldGlow, ref int focusedField, ref float particleTime, ref Vector2 mousePos)
        {
            if (!guiHelper.animationsEnabled)
                return;

            float speed = guiHelper.smoothAnimationsEnabled ? guiHelper.animationSpeed : guiHelper.animationSpeed * 0.5f;

            float targetAlpha = isOpen ? 1f : 0f;
            menuAlpha = Mathf.Lerp(menuAlpha, targetAlpha, Time.deltaTime * speed * 0.67f);

            float targetScale = isOpen ? guiHelper.uiScale : (guiHelper.uiScale * 0.8f);
            menuScale = Mathf.Lerp(menuScale, targetScale, Time.deltaTime * speed * 0.83f);

            titleGlow = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f;

            backgroundPulse = Mathf.Sin(Time.time * 1.5f) * 0.1f + guiHelper.backgroundAlpha;

            if (guiHelper.hoverEffectsEnabled)
            {
                for (int i = 0; i < buttonGlowEffects.Length; i++)
                {
                    float target = (hoveredButton == i) ? guiHelper.glowIntensity : 0f;
                    buttonGlowEffects[i] = Mathf.Lerp(buttonGlowEffects[i], target, Time.deltaTime * speed);
                }
            }

            if (guiHelper.glowEffectsEnabled)
            {
                for (int i = 0; i < inputFieldGlow.Length; i++)
                {
                    float target = (focusedField == i) ? guiHelper.glowIntensity : 0f;
                    inputFieldGlow[i] = Mathf.Lerp(inputFieldGlow[i], target, Time.deltaTime * speed * 0.67f);
                }
            }

            if (guiHelper.particleEffectsEnabled)
                particleTime += Time.deltaTime;

            mousePos = Event.current.mousePosition;
        }

        public bool BeginAnimatedGUI(float menuAlpha, float backgroundPulse, Vector2 mousePos)
        {
            float currentAlpha = guiHelper.fadeTransitionsEnabled ? menuAlpha : 1f;
            GUI.color = new Color(1f, 1f, 1f, currentAlpha);

            if (guiHelper.particleEffectsEnabled)
                RenderBackgroundParticles(currentAlpha);

            float pulseAlpha = guiHelper.animationsEnabled ? backgroundPulse : guiHelper.backgroundAlpha;
            GUI.color = new Color(1f, 1f, 1f, currentAlpha * pulseAlpha);

            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP_MELONLOADER
            layoutComponents.BeginVerticalGroup(styleManager.animatedBoxStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            layoutComponents.BeginVerticalGroup(styleManager.animatedBoxStyle);
#endif
            GUI.color = new Color(1f, 1f, 1f, currentAlpha);
            _layoutGroupStarted = true;
            return true;
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
            for (int i = 0; i < particleCount; i++)
            {
                float x = 30 + Mathf.Sin(particleTime * 0.5f + i * 0.8f) * 25 + i * 35;
                float y = 40 + Mathf.Cos(particleTime * 0.3f + i * 1.2f) * 20 + i * 20;
                float alpha = (Mathf.Sin(particleTime * 2f + i) * 0.3f + 0.4f) * menuAlpha * guiHelper.glowIntensity;

                Color particleColor = new Color(ThemeManager.Instance.CurrentTheme.AccentColor.r, ThemeManager.Instance.CurrentTheme.AccentColor.g, ThemeManager.Instance.CurrentTheme.AccentColor.b, alpha);

                GUI.color = particleColor;
                GUI.DrawTexture(new Rect(x * guiHelper.uiScale, y * guiHelper.uiScale, 4 * guiHelper.uiScale, 4 * guiHelper.uiScale), particleTexture);
            }
        }

        public void Cleanup() { }
    }
}
