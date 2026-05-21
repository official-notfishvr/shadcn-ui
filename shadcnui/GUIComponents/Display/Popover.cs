using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Popover : BaseComponent
    {
        private bool _isOpen;
        private string _currentId;
        private int _currentZIndex;
        private const float AnimationDuration = DesignTokens.Animation.DurationFast;

        public Popover(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => _isOpen;

        public void Render(PopoverConfig config)
        {
            if (!_isOpen || config == null)
                return;

            string id = _currentId ?? "popover";
            var anim = guiHelper.GetAnimationManager();
            float alpha = anim.GetFloat($"popover_alpha_{id}", 1f);
            float scale = anim.GetFloat($"popover_scale_{id}", 1f);
            Vector2 slide = anim.GetVector2($"popover_slide_{id}", Vector2.zero);

            var prevMatrix = GUI.matrix;
            var prevColor = GUI.color;

            if (alpha < 1f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * alpha);

            if (scale < 1f || slide != Vector2.zero)
            {
                GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
                GUI.matrix = Matrix4x4.Translate(new Vector3(slide.x, slide.y, 0f)) * GUI.matrix;
            }

            layoutComponents.BeginVerticalGroup(styleManager?.GetPopoverContentStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box, GUILayout.MaxWidth(320), GUILayout.MaxHeight(220));
            config.Content?.Invoke();
            layoutComponents.EndVerticalGroup();

            GUI.matrix = prevMatrix;
            GUI.color = prevColor;
        }

        internal void DrawPopover(Action content)
        {
            Render(new PopoverConfig { Content = content });
        }

        public void Open(string id = "popover", int zIndex = -1)
        {
            _currentId = id;
            _currentZIndex = zIndex >= 0 ? zIndex : DesignTokens.ZIndex.Popover;
            _isOpen = true;

            var anim = guiHelper.GetAnimationManager();
            anim.FadeIn($"popover_alpha_{id}", AnimationDuration, EasingFunctions.EaseOutCubic);
            anim.ScaleIn($"popover_scale_{id}", AnimationDuration, 0.92f, EasingFunctions.EaseOutCubic);
            anim.SlideIn($"popover_slide_{id}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.LG), AnimationDuration, EasingFunctions.EaseOutCubic);
        }

        public void Close()
        {
            if (_currentId != null)
            {
                var anim = guiHelper.GetAnimationManager();
                anim.FadeOut($"popover_alpha_{_currentId}", AnimationDuration * 0.8f, EasingFunctions.EaseInCubic);
                anim.ScaleOut($"popover_scale_{_currentId}", AnimationDuration * 0.8f, 0.92f, EasingFunctions.EaseInCubic);
            }

            _isOpen = false;
            _currentId = null;
        }

        public int GetZIndex() => _currentZIndex;
    }
}
