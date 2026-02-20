using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Popover : BaseComponent
    {
        private bool isOpen;
        private string _currentId;
        private int _currentZIndex;
        private const float AnimationDuration = DesignTokens.Animation.DurationFast;

        public Popover(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => isOpen;

        #region Config-based API
        public void DrawPopover(PopoverConfig config)
        {
            if (!isOpen)
                return;

            var animManager = guiHelper.GetAnimationManager();
            string id = _currentId ?? "popover";
            float alpha = animManager.GetFloat($"popover_alpha_{id}", 1f);
            float scale = animManager.GetFloat($"popover_scale_{id}", 1f);
            Vector2 slideOffset = animManager.GetVector2($"popover_slide_{id}", Vector2.zero);

            ApplyPopoverAnimation(alpha, scale, slideOffset);
            DrawPopoverContent(config);
            RestoreGraphicsState();
        }
        #endregion

        #region API
        public void Open(string id = "popover", int zIndex = -1)
        {
            _currentId = id;
            _currentZIndex = zIndex >= 0 ? zIndex : DesignTokens.ZIndex.Popover;
            isOpen = true;

            var animManager = guiHelper.GetAnimationManager();
            animManager.FadeIn($"popover_alpha_{id}", AnimationDuration, EasingFunctions.EaseOutCubic);
            animManager.ScaleIn($"popover_scale_{id}", AnimationDuration, 0.9f, EasingFunctions.EaseOutCubic);
            animManager.SlideIn($"popover_slide_{id}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.XL), AnimationDuration, EasingFunctions.EaseOutCubic);
        }

        public void Close()
        {
            if (_currentId != null)
            {
                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeOut($"popover_alpha_{_currentId}", AnimationDuration * 0.8f, EasingFunctions.EaseInCubic);
                animManager.ScaleOut($"popover_scale_{_currentId}", AnimationDuration * 0.8f, 0.9f, EasingFunctions.EaseInCubic);
            }
            isOpen = false;
            _currentId = null;
        }

        public void DrawPopover(Action content)
        {
            DrawPopover(new PopoverConfig { Content = content });
        }

        public int GetZIndex() => _currentZIndex;
        #endregion

        #region Private Methods
        private void ApplyPopoverAnimation(float alpha, float scale, Vector2 slideOffset)
        {
            Color prevColor = GUI.color;
            if (alpha < 1f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * alpha);

            if (scale < 1f || slideOffset != Vector2.zero)
            {
                GUIUtility.ScaleAroundPivot(new Vector3(scale, scale, 1f), Vector2.zero);
                GUI.matrix = Matrix4x4.Translate(new Vector3(slideOffset.x, slideOffset.y, 0f)) * GUI.matrix;
            }
        }

        private void DrawPopoverContent(PopoverConfig config)
        {
            layoutComponents.BeginVerticalGroup(guiHelper.GetStyleManager().GetPopoverContentStyle(), GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
            config.Content?.Invoke();
            GUILayout.EndVertical();
        }

        private void RestoreGraphicsState()
        {
            GUI.matrix = Matrix4x4.identity;
            GUI.color = Color.white;
        }
        #endregion
    }
}
