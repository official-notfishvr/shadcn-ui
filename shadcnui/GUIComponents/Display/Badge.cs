using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Badge : BaseComponent
    {
        public Badge(GUIHelper helper)
            : base(helper) { }

        public void DrawBadge(BadgeConfig config)
        {
            if (config == null)
                return;

            string text = ResolveText(config);
            GUIStyle badgeStyle = styleManager?.GetBadgeStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.label;

            layoutComponents.BeginHorizontalGroup(badgeStyle, config.LayoutOptions ?? Array.Empty<GUILayoutOption>());

            if (config.ShowStatusDot)
                DrawStatusDot(config.IsActive);

            if (config.Icon?.Image != null)
            {
                DrawIcon(config.Icon);
                layoutComponents.AddSpace(Mathf.Max(2f, config.Icon.Spacing) * guiHelper.uiScale);
            }

            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.label;
            UnityHelpers.Label(text, labelStyle);

            layoutComponents.EndHorizontalGroup();
        }

        public void DrawBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void BadgeWithIcon(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    Icon = icon != null ? new IconConfig(icon) : null,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options)
        {
            string text = count > maxCount ? $"{maxCount}+" : count.ToString();
            DrawBadge(text, variant, size, null, options);
        }

        public void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    ShowStatusDot = true,
                    IsActive = isActive,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void ProgressBadge(string text, float progress, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            string value = $"{text} {(progress * 100f):0}%";
            DrawBadge(value, variant, size, null, options);
        }

        public void RoundedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, float cornerRadius = DesignTokens.Radius.XL, params GUILayoutOption[] options)
        {
            DrawBadge(text, variant, size, null, options);
        }

        public void AnimatedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options) => AnimatedBadge(text, $"badge_{text}", variant, size, appearance, options);

        public void AnimatedBadge(string text, string animId, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            var animManager = guiHelper.GetAnimationManager();
            if (!animManager.Exists(animId))
                animManager.StartFloat(animId, 0.7f, 1f, DesignTokens.Animation.DurationNormal, EasingFunctions.EaseInOutCubic);

            float alpha = animManager.GetFloat(animId, 1f);
            Color prev = GUI.color;
            GUI.color = new Color(prev.r, prev.g, prev.b, prev.a * alpha);
            DrawBadge(text, variant, size, appearance, options);
            GUI.color = prev;
        }

        private string ResolveText(BadgeConfig config)
        {
            if (config.Count > 0)
            {
                int max = config.MaxCount > 0 ? config.MaxCount : 99;
                return config.Count > max ? $"{max}+" : config.Count.ToString();
            }

            if (config.Progress > 0f)
                return $"{config.Text} {(config.Progress * 100f):0}%";

            return config.Text ?? "Badge";
        }

        private void DrawIcon(IconConfig icon)
        {
            float size = icon.Size * guiHelper.uiScale;
            UnityHelpers.Label(icon.Image, GUILayout.Width(size), GUILayout.Height(size));
        }

        private void DrawStatusDot(bool isActive)
        {
            float size = 6f * guiHelper.uiScale;
            var theme = styleManager?.GetTheme();
            Color dot = isActive ? (theme?.Accent ?? Color.green) : (theme?.Muted ?? Color.gray);

            Rect rect = GUILayoutUtility.GetRect(size, size, GUILayout.Width(size), GUILayout.Height(size));
            Color prev = GUI.color;
            GUI.color = dot;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = prev;

            layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
        }
    }
}
