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

        public void Render(BadgeConfig config)
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

            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default, config.Size) ?? GUI.skin.label;
            if (config.Appearance?.ForegroundColor is { } foreground)
                labelStyle = new UnityHelpers.GUIStyle(labelStyle) { normal = { textColor = foreground } };
            var centeredLabelStyle = new UnityHelpers.GUIStyle(labelStyle) { alignment = TextAnchor.MiddleCenter };
            UnityHelpers.Label(text, centeredLabelStyle, GUILayout.ExpandHeight(true));

            layoutComponents.EndHorizontalGroup();
        }

        internal void DrawBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            Render(
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

        internal void BadgeWithIcon(string text, Texture2D icon, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            Render(
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

        internal void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options)
        {
            string text = count > maxCount ? $"{maxCount}+" : count.ToString();
            Render(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        internal void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            Render(
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

        internal void ProgressBadge(string text, float progress, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            string value = $"{text} {(progress * 100f):0}%";
            Render(
                new BadgeConfig
                {
                    Text = value,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        internal void RoundedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, float cornerRadius = DesignTokens.Radius.XL, params GUILayoutOption[] options)
        {
            Render(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        internal void AnimatedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options) => AnimatedBadge(text, $"badge_{text}", variant, size, appearance, options);

        internal void AnimatedBadge(string text, string animId, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            var animManager = guiHelper.GetAnimationManager();
            if (!animManager.Exists(animId))
                animManager.StartFloat(animId, 0.7f, 1f, DesignTokens.Animation.DurationNormal, EasingFunctions.EaseInOutCubic);

            float alpha = animManager.GetFloat(animId, 1f);
            Color prev = GUI.color;
            GUI.color = new Color(prev.r, prev.g, prev.b, prev.a * alpha);
            Render(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
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

            Rect rect = SurfaceDrawUtility.ReserveSquare(size);
            SurfaceDrawUtility.DrawSolid(rect, dot);

            layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
        }
    }
}
