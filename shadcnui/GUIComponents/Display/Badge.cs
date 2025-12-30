using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Badge : BaseComponent
    {
        private Dictionary<string, bool> _pulseStarted = new Dictionary<string, bool>();
        private int _animatedBadgeCounter = 0;

        public Badge(GUIHelper helper)
            : base(helper) { }

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        public void DrawBadge(BadgeConfig config)
        {
            DrawBadgeInternal(config, false, null);
        }

        private void DrawBadgeInternal(BadgeConfig config, bool isAnimated, string animationId)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle badgeStyle = styleManager.GetBadgeStyle(config.Variant, config.Size);
            Color originalColor = GUI.color;
            if (isAnimated)
            {
                string animId = animationId ?? $"badge_pulse_{_animatedBadgeCounter++}";
                var animManager = guiHelper.GetAnimationManager();
                if (!_pulseStarted.ContainsKey(animId) || !_pulseStarted[animId])
                {
                    animManager.StartFloat(
                        animId,
                        0.7f,
                        1f,
                        0.5f,
                        EasingFunctions.EaseInOut,
                        () =>
                        {
                            animManager.StartFloat(animId, 1f, 0.7f, 0.5f, EasingFunctions.EaseInOut);
                        }
                    );
                    _pulseStarted[animId] = true;
                }
                float alpha = animManager.GetFloat(animId, 1f);
                if (animManager.IsComplete(animId))
                {
                    float current = animManager.GetFloat(animId, 1f);
                    if (current <= 0.71f)
                        animManager.StartFloat(animId, 0.7f, 1f, 0.5f, EasingFunctions.EaseInOut);
                    else
                        animManager.StartFloat(animId, 1f, 0.7f, 0.5f, EasingFunctions.EaseInOut);
                }
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            if (config.CornerRadius > 0 && config.CornerRadius != DesignTokens.Radius.XL)
            {
                var roundedStyle = new UnityHelpers.GUIStyle(badgeStyle);
                int r = Mathf.RoundToInt(config.CornerRadius * guiHelper.uiScale);
                roundedStyle.border = new UnityHelpers.RectOffset(r, r, r, r);
                roundedStyle.padding = new UnityHelpers.RectOffset(badgeStyle.padding.left + (int)DesignTokens.Spacing.XS, badgeStyle.padding.right + (int)DesignTokens.Spacing.XS, badgeStyle.padding.top + (int)DesignTokens.Spacing.XXS, badgeStyle.padding.bottom + (int)DesignTokens.Spacing.XXS);
                badgeStyle = roundedStyle;
            }
            bool verticalGroupStarted = false;
            if (config.Progress > 0)
            {
                layoutComponents.BeginVerticalGroup();
                verticalGroupStarted = true;
            }
            bool horizontalGroupStarted = false;
            if (config.Icon?.Image != null || config.ShowStatusDot)
            {
                layoutComponents.BeginHorizontalGroup();
                horizontalGroupStarted = true;
            }
            if (config.ShowStatusDot)
            {
                Color dotColor = config.IsActive ? Color.green : Color.gray;
                GUIStyle dotStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                dotStyle.normal.background = styleManager.CreateSolidTexture(dotColor);
                dotStyle.fixedWidth = DesignTokens.StatusIndicator.Default * guiHelper.uiScale;
                dotStyle.fixedHeight = DesignTokens.StatusIndicator.Default * guiHelper.uiScale;
                dotStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                dotStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
                dotStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);
                UnityHelpers.Label("", dotStyle);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }
            if (config.Icon?.Image != null)
            {
                RenderIcon(config.Icon);
                layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
            }
            if (config.Rect.HasValue)
            {
                Rect scaledRect = new Rect(config.Rect.Value.x * guiHelper.uiScale, config.Rect.Value.y * guiHelper.uiScale, config.Rect.Value.width * guiHelper.uiScale, config.Rect.Value.height * guiHelper.uiScale);
                UnityHelpers.Label(scaledRect, config.Text ?? "Badge", badgeStyle);
            }
            else
                UnityHelpers.Label(config.Text ?? "Badge", badgeStyle, config.Options);
            if (horizontalGroupStarted)
                layoutComponents.EndHorizontalGroup();
            if (config.Progress > 0)
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
                Rect progressRect = GUILayoutUtility.GetRect(60 * guiHelper.uiScale, DesignTokens.Spacing.XS * guiHelper.uiScale);
                GUIStyle bgStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                bgStyle.normal.background = styleManager.CreateSolidTexture(Color.gray);
                GUI.Box(progressRect, "", bgStyle);
                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * Mathf.Clamp01(config.Progress), progressRect.height);
                GUIStyle fillStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                fillStyle.normal.background = styleManager.CreateSolidTexture(Color.green);
                GUI.Box(fillRect, "", fillStyle);
            }
            if (verticalGroupStarted)
                layoutComponents.EndVerticalGroup();
            if (isAnimated)
                GUI.color = originalColor;
        }

        public void DrawBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Options = options,
                }
            );
        }

        public void DrawBadge(Rect rect, string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Rect = rect,
                    Text = text,
                    Variant = variant,
                    Size = size,
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
                    Options = options,
                }
            );
        }

        public void CountBadge(int count, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, int maxCount = 99, params GUILayoutOption[] options)
        {
            string displayText = count > maxCount ? $"{maxCount}+" : count.ToString();
            DrawBadge(
                new BadgeConfig
                {
                    Text = displayText,
                    Variant = variant,
                    Size = size,
                    Count = count,
                    MaxCount = maxCount,
                    Options = options,
                }
            );
        }

        public void StatusBadge(string text, bool isActive, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    IsActive = isActive,
                    ShowStatusDot = true,
                    Variant = variant,
                    Size = size,
                    Options = options,
                }
            );
        }

        public void ProgressBadge(string text, float progress, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    Progress = progress,
                    Variant = variant,
                    Size = size,
                    Options = options,
                }
            );
        }

        public void AnimatedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadgeInternal(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Options = options,
                },
                true,
                null
            );
        }

        public void AnimatedBadge(string text, string animationId, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawBadgeInternal(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    Options = options,
                },
                true,
                animationId
            );
        }

        public void RoundedBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, float cornerRadius = DesignTokens.Radius.XL, params GUILayoutOption[] options)
        {
            DrawBadge(
                new BadgeConfig
                {
                    Text = text,
                    Variant = variant,
                    Size = size,
                    CornerRadius = cornerRadius,
                    Options = options,
                }
            );
        }

        public void PillBadge(string text, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            RoundedBadge(text, variant, size, DesignTokens.Radius.Full, options);
        }
    }
}
