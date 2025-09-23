using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shadcnui;
using UnityEngine;

namespace shadcnui
{
    public class Settings
    {
        private GUIHelper helper;

        public Settings(GUIHelper helper)
        {
            helper = this.helper;
        }

        public bool SetAnimation
        {
            get => helper?.animationsEnabled ?? false;
            set
            {
                if (helper != null)
                    helper.animationsEnabled = value;
            }
        }

        public float SetAnimationSpeed
        {
            get => helper?.animationSpeed ?? 12f;
            set
            {
                if (helper != null)
                    helper.animationSpeed = Mathf.Clamp(value, 1f, 50f);
            }
        }

        public bool SetGlowEffects
        {
            get => helper?.glowEffectsEnabled ?? false;
            set
            {
                if (helper != null)
                    helper.glowEffectsEnabled = value;
            }
        }

        public bool SetParticleEffects
        {
            get => helper?.particleEffectsEnabled ?? false;
            set
            {
                if (helper != null)
                    helper.particleEffectsEnabled = value;
            }
        }

        public float SetBackgroundAlpha
        {
            get => helper?.backgroundAlpha ?? 0.9f;
            set
            {
                if (helper != null)
                    helper.backgroundAlpha = Mathf.Clamp01(value);
            }
        }

        public int SetFontSize
        {
            get => helper?.fontSize ?? 12;
            set
            {
                if (helper != null)
                    helper.fontSize = Mathf.Clamp(value, 8, 24);
            }
        }

        public int SetCornerRadius
        {
            get => helper?.cornerRadius ?? 8;
            set
            {
                if (helper != null)
                    helper.cornerRadius = Mathf.Clamp(value, 0, 20);
            }
        }

        public bool SetHoverEffects
        {
            get => helper?.hoverEffectsEnabled ?? false;
            set
            {
                if (helper != null)
                    helper.hoverEffectsEnabled = value;
            }
        }

        public bool SetFadeTransitions
        {
            get => helper?.fadeTransitionsEnabled ?? false;
            set
            {
                if (helper != null)
                    helper.fadeTransitionsEnabled = value;
            }
        }

        public float SetGlowIntensity
        {
            get => helper?.glowIntensity ?? 1f;
            set
            {
                if (helper != null)
                    helper.glowIntensity = Mathf.Clamp01(value);
            }
        }

        public bool SetSmoothAnimations
        {
            get => helper?.smoothAnimationsEnabled ?? false;
            set
            {
                if (helper != null)
                    helper.smoothAnimationsEnabled = value;
            }
        }

        public float SetUIScale
        {
            get => helper?.uiScale ?? 1f;
            set
            {
                if (helper != null)
                    helper.uiScale = Mathf.Clamp(value, 0.5f, 2f);
            }
        }
    }
}
