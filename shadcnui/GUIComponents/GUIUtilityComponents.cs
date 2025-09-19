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
    public class GUIUtilityComponents
    {
        private GUIHelper guiHelper;

        public GUIUtilityComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void SetRGBValues(
            int red,
            int green,
            int blue,
            ref string r,
            ref string g,
            ref string b,
            ref float rSlider,
            ref float gSlider,
            ref float bSlider
        )
        {
            r = Mathf.Clamp(red, 0, 255).ToString();
            g = Mathf.Clamp(green, 0, 255).ToString();
            b = Mathf.Clamp(blue, 0, 255).ToString();

            rSlider = red;
            gSlider = green;
            bSlider = blue;
        }

        public void ApplyTheme(string themeName, GUIHelper helper)
        {
            string r = "",
                g = "",
                b = "";
            float rSlider = 0,
                gSlider = 0,
                bSlider = 0;

            switch (themeName.ToLower())
            {
                case "dark":
                    SetRGBValues(
                        20,
                        20,
                        30,
                        ref r,
                        ref g,
                        ref b,
                        ref rSlider,
                        ref gSlider,
                        ref bSlider
                    );
                    helper.primaryColor = new Color(0.1f, 0.1f, 0.2f);
                    helper.secondaryColor = new Color(0.2f, 0.2f, 0.3f);
                    helper.accentColor = new Color(0.4f, 0.6f, 1f);
                    break;
                case "light":
                    SetRGBValues(
                        240,
                        240,
                        250,
                        ref r,
                        ref g,
                        ref b,
                        ref rSlider,
                        ref gSlider,
                        ref bSlider
                    );
                    helper.primaryColor = new Color(0.9f, 0.9f, 0.95f);
                    helper.secondaryColor = new Color(0.8f, 0.8f, 0.9f);
                    helper.accentColor = new Color(0.2f, 0.4f, 0.8f);
                    break;
                case "neon":
                    SetRGBValues(
                        255,
                        0,
                        255,
                        ref r,
                        ref g,
                        ref b,
                        ref rSlider,
                        ref gSlider,
                        ref bSlider
                    );
                    helper.primaryColor = new Color(0.2f, 0f, 0.4f);
                    helper.secondaryColor = new Color(0.4f, 0f, 0.6f);
                    helper.accentColor = new Color(0f, 1f, 1f);
                    break;
                case "forest":
                    SetRGBValues(
                        34,
                        139,
                        34,
                        ref r,
                        ref g,
                        ref b,
                        ref rSlider,
                        ref gSlider,
                        ref bSlider
                    );
                    helper.primaryColor = new Color(0.1f, 0.3f, 0.1f);
                    helper.secondaryColor = new Color(0.2f, 0.4f, 0.2f);
                    helper.accentColor = new Color(0.6f, 1f, 0.6f);
                    break;
                default:
                    SetRGBValues(
                        255,
                        128,
                        0,
                        ref r,
                        ref g,
                        ref b,
                        ref rSlider,
                        ref gSlider,
                        ref bSlider
                    );
                    helper.primaryColor = new Color(0.2f, 0.3f, 0.6f);
                    helper.secondaryColor = new Color(0.3f, 0.2f, 0.4f);
                    helper.accentColor = new Color(0.5f, 0.8f, 1f);
                    break;
            }

            helper.r = r;
            helper.g = g;
            helper.b = b;
            helper.rSlider = rSlider;
            helper.gSlider = gSlider;
            helper.bSlider = bSlider;

            var styleManager = helper.GetStyleManager();
            if (styleManager != null)
            {
                styleManager.InitializeGUI();
            }
        }

        public void ResetAllSettings(GUIHelper helper)
        {
            helper.animationsEnabled = true;
            helper.animationSpeed = 12f;
            helper.glowEffectsEnabled = true;
            helper.particleEffectsEnabled = true;
            helper.backgroundAlpha = 0.9f;
            helper.fontSize = 12;
            helper.cornerRadius = 8;
            helper.shadowEffectsEnabled = false;
            helper.hoverEffectsEnabled = true;
            helper.fadeTransitionsEnabled = true;
            helper.primaryColor = new Color(0.2f, 0.3f, 0.6f);
            helper.secondaryColor = new Color(0.3f, 0.2f, 0.4f);
            helper.accentColor = new Color(0.5f, 0.8f, 1f);
            helper.controlSpacing = 10f;
            helper.buttonHeight = 30f;
            helper.borderEffectsEnabled = false;
            helper.glowIntensity = 0.5f;
            helper.smoothAnimationsEnabled = true;
            helper.uiScale = 1f;
            helper.debugModeEnabled = false;

            var styleManager = helper.GetStyleManager();
            if (styleManager != null)
            {
                styleManager.InitializeGUI();
            }
        }
    }
}
