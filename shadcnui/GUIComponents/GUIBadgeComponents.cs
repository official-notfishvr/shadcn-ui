using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUIBadgeComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUIBadgeComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public void Badge(
            string text,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle badgeStyle = styleManager.GetBadgeStyle(variant, size);

#if IL2CPP
            GUILayout.Label(
                text ?? "Badge",
                badgeStyle,
                (Il2CppReferenceArray<GUILayoutOption>)options
            );
#else
            GUILayout.Label(text ?? "Badge", badgeStyle, options);
#endif
        }

        public void Badge(
            Rect rect,
            string text,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle badgeStyle = styleManager.GetBadgeStyle(variant, size);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            GUI.Label(scaledRect, text ?? "Badge", badgeStyle);
        }

        public void BadgeWithIcon(
            string text,
            Texture2D icon,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            layoutComponents.BeginHorizontalGroup();

            if (icon != null)
            {
#if IL2CPP
                GUILayout.Label(
                    icon,
                    GUI.skin.label,
                    (Il2CppReferenceArray<GUILayoutOption>)
                        new GUILayoutOption[]
                        {
                            GUILayout.Width(16 * guiHelper.uiScale),
                            GUILayout.Height(16 * guiHelper.uiScale),
                        }
                );
#else
                GUILayout.Label(
                    icon,
                    GUILayout.Width(16 * guiHelper.uiScale),
                    GUILayout.Height(16 * guiHelper.uiScale)
                );
#endif
                layoutComponents.AddSpace(4);
            }

            Badge(text, variant, size, options);

            layoutComponents.EndHorizontalGroup();
        }

        public void CustomBadge(
            string text,
            Color backgroundColor,
            Color textColor,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.normal.textColor = textColor;
            customStyle.fontSize = GetBadgeFontSize(size);
            customStyle.padding = GetBadgePadding(size);
            customStyle.border = new RectOffset(4, 4, 2, 2);
            customStyle.alignment = TextAnchor.MiddleCenter;

#if IL2CPP
            GUILayout.Label(
                text ?? "Badge",
                customStyle,
                (Il2CppReferenceArray<GUILayoutOption>)options
            );
#else
            GUILayout.Label(text ?? "Badge", customStyle, options);
#endif
        }

        public void CountBadge(
            int count,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            int maxCount = 99,
            params GUILayoutOption[] options
        )
        {
            string displayText = count > maxCount ? $"{maxCount}+" : count.ToString();
            Badge(displayText, variant, size, options);
        }

        public void StatusBadge(
            string text,
            bool isActive,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            layoutComponents.BeginHorizontalGroup();

            Color dotColor = isActive ? Color.green : Color.gray;
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle dotStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = styleManager.CreateSolidTexture(dotColor) },
                    fixedWidth = 8 * guiHelper.uiScale,
                    fixedHeight = 8 * guiHelper.uiScale,
                    border = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                };

#if IL2CPP
                GUILayout.Label(
                    GUIContent.none,
                    dotStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label("", dotStyle);
#endif
                layoutComponents.AddSpace(4);
            }

            Badge(text, variant, size, options);

            layoutComponents.EndHorizontalGroup();
        }

        public bool DismissibleBadge(
            string text,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            Action onDismiss = null,
            params GUILayoutOption[] options
        )
        {
            layoutComponents.BeginHorizontalGroup();

            Badge(text, variant, size, options);

            layoutComponents.AddSpace(4);
#if IL2CPP
            bool closeClicked = GUILayout.Button(
                "×",
                GUI.skin.button,
                new Il2CppReferenceArray<GUILayoutOption>(
                    new GUILayoutOption[]
                    {
                        GUILayout.Width(16 * guiHelper.uiScale),
                        GUILayout.Height(16 * guiHelper.uiScale),
                    }
                )
            );
#else
            bool closeClicked = GUILayout.Button(
                "×",
                GUI.skin.button,
                new GUILayoutOption[]
                {
                    GUILayout.Width(16 * guiHelper.uiScale),
                    GUILayout.Height(16 * guiHelper.uiScale),
                }
            );
#endif
            if (closeClicked && onDismiss != null)
            {
                onDismiss.Invoke();
            }

            layoutComponents.EndHorizontalGroup();

            return closeClicked;
        }

        public void ProgressBadge(
            string text,
            float progress,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            layoutComponents.BeginVerticalGroup();

            Badge(text, variant, size, options);

            layoutComponents.AddSpace(2);
            Rect progressRect = GUILayoutUtility.GetRect(
                60 * guiHelper.uiScale,
                4 * guiHelper.uiScale
            );

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle bgStyle = new GUIStyle(GUI.skin.box);
                bgStyle.normal.background = styleManager.CreateSolidTexture(Color.gray);
                GUI.Box(progressRect, "", bgStyle);

                Rect fillRect = new Rect(
                    progressRect.x,
                    progressRect.y,
                    progressRect.width * Mathf.Clamp01(progress),
                    progressRect.height
                );
                GUIStyle fillStyle = new GUIStyle(GUI.skin.box);
                fillStyle.normal.background = styleManager.CreateSolidTexture(Color.green);
                GUI.Box(fillRect, "", fillStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        public void AnimatedBadge(
            string text,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            float time = Time.time * 2f;
            float alpha = (Mathf.Sin(time) + 1f) * 0.5f * 0.3f + 0.7f;

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            Badge(text, variant, size, options);

            GUI.color = originalColor;
        }

        public void BadgeWithTooltip(
            string text,
            string tooltip,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            params GUILayoutOption[] options
        )
        {
            layoutComponents.BeginHorizontalGroup();

            Badge(text, variant, size, options);

            if (!string.IsNullOrEmpty(tooltip))
            {
                layoutComponents.AddSpace(4);

                var styleManager = guiHelper.GetStyleManager();
                GUIStyle tooltipStyle =
                    styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);

#if IL2CPP
                GUILayout.Label("?", tooltipStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label("?", tooltipStyle);
#endif
                GUI.color = originalColor;
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void BadgeGroup(
            string[] texts,
            BadgeVariant[] variants,
            BadgeSize size = BadgeSize.Default,
            bool horizontal = true,
            float spacing = 5f
        )
        {
            if (texts == null || texts.Length == 0)
                return;

            if (variants == null || variants.Length != texts.Length)
            {
                variants = new BadgeVariant[texts.Length];
                for (int i = 0; i < variants.Length; i++)
                {
                    variants[i] = BadgeVariant.Default;
                }
            }

            if (horizontal)
            {
                layoutComponents.BeginHorizontalGroup();
            }
            else
            {
                layoutComponents.BeginVerticalGroup();
            }

            for (int i = 0; i < texts.Length; i++)
            {
                Badge(texts[i], variants[i], size);

                if (i < texts.Length - 1)
                {
                    layoutComponents.AddSpace(spacing);
                }
            }

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();
        }

        public void RoundedBadge(
            string text,
            BadgeVariant variant = BadgeVariant.Default,
            BadgeSize size = BadgeSize.Default,
            float cornerRadius = 12f,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle badgeStyle = styleManager.GetBadgeStyle(variant, size);

            GUIStyle roundedStyle = new GUIStyle(badgeStyle);
            roundedStyle.border = new RectOffset(
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale),
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale),
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale),
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale)
            );

#if IL2CPP
            GUILayout.Label(
                text ?? "Badge",
                roundedStyle,
                (Il2CppReferenceArray<GUILayoutOption>)options
            );
#else
            GUILayout.Label(text ?? "Badge", roundedStyle, options);
#endif
        }

        private int GetBadgeFontSize(BadgeSize size)
        {
            switch (size)
            {
                case BadgeSize.Small:
                    return Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                case BadgeSize.Large:
                    return Mathf.RoundToInt((guiHelper.fontSize + 2) * guiHelper.uiScale);
                default:
                    return Mathf.RoundToInt(guiHelper.fontSize * guiHelper.uiScale);
            }
        }

        private RectOffset GetBadgePadding(BadgeSize size)
        {
            float scale = guiHelper.uiScale;
            switch (size)
            {
                case BadgeSize.Small:
                    return new RectOffset(
                        Mathf.RoundToInt(6 * scale),
                        Mathf.RoundToInt(6 * scale),
                        Mathf.RoundToInt(2 * scale),
                        Mathf.RoundToInt(2 * scale)
                    );
                case BadgeSize.Large:
                    return new RectOffset(
                        Mathf.RoundToInt(12 * scale),
                        Mathf.RoundToInt(12 * scale),
                        Mathf.RoundToInt(4 * scale),
                        Mathf.RoundToInt(4 * scale)
                    );
                default:
                    return new RectOffset(
                        Mathf.RoundToInt(8 * scale),
                        Mathf.RoundToInt(8 * scale),
                        Mathf.RoundToInt(3 * scale),
                        Mathf.RoundToInt(3 * scale)
                    );
            }
        }
    }
}
