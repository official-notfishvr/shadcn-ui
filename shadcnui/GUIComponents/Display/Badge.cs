using System;
using System.Collections.Generic;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Badge : BaseComponent
    {
        public Badge(GUIHelper helper) : base(helper) { }
        public void DrawBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle badgeStyle = styleManager.GetBadgeStyle(variant, size);

            UnityHelpers.Label(text ?? "Badge", badgeStyle, options);
        }

        public void DrawBadge(Rect rect, string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle badgeStyle = styleManager.GetBadgeStyle(variant, size);

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            UnityHelpers.Label(scaledRect, text ?? "Badge", badgeStyle);
        }

        public void BadgeWithIcon(string text, Texture2D icon, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            layoutComponents.BeginHorizontalGroup();

            if (icon != null)
            {
                UnityHelpers.Label(icon, GUILayout.Width(16 * guiHelper.uiScale), GUILayout.Height(16 * guiHelper.uiScale));
                layoutComponents.AddSpace(4);
            }

            DrawBadge(text, variant, size, options);

            layoutComponents.EndHorizontalGroup();
        }

        public void CountBadge(int count, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, int maxCount = 99, params GUILayoutOption[] options)
        {
            string displayText = count > maxCount ? $"{maxCount}+" : count.ToString();
            DrawBadge(displayText, variant, size, options);
        }

        public void StatusBadge(string text, bool isActive, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            layoutComponents.BeginHorizontalGroup();

            Color dotColor = isActive ? Color.green : Color.gray;
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle dotStyle = new UnityHelpers.GUIStyle(GUI.skin.box)
                {
                    normal = { background = styleManager.CreateSolidTexture(dotColor) },
                    fixedWidth = 8 * guiHelper.uiScale,
                    fixedHeight = 8 * guiHelper.uiScale,
                    border = new UnityHelpers.RectOffset(0, 0, 0, 0),
                    padding = new UnityHelpers.RectOffset(0, 0, 0, 0),
                    margin = new UnityHelpers.RectOffset(0, 0, 0, 0),
                };

                UnityHelpers.Label("", dotStyle);

                layoutComponents.AddSpace(4);
            }

            DrawBadge(text, variant, size, options);

            layoutComponents.EndHorizontalGroup();
        }

        public void ProgressBadge(string text, float progress, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();

            DrawBadge(text, variant, size, options);

            layoutComponents.AddSpace(2);
            Rect progressRect = GUILayoutUtility.GetRect(60 * guiHelper.uiScale, 4 * guiHelper.uiScale);

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle bgStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                bgStyle.normal.background = styleManager.CreateSolidTexture(Color.gray);
                GUI.Box(progressRect, "", bgStyle);

                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * Mathf.Clamp01(progress), progressRect.height);
                GUIStyle fillStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                fillStyle.normal.background = styleManager.CreateSolidTexture(Color.green);
                GUI.Box(fillRect, "", fillStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        public void AnimatedBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options)
        {
            float time = Time.time * 2f;
            float alpha = (Mathf.Sin(time) + 1f) * 0.5f * 0.3f + 0.7f;

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            DrawBadge(text, variant, size, options);

            GUI.color = originalColor;
        }

        public void RoundedBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, float cornerRadius = 12f, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle badgeStyle = styleManager.GetBadgeStyle(variant, size);

            GUIStyle roundedStyle = new UnityHelpers.GUIStyle(badgeStyle);
            roundedStyle.border = new UnityHelpers.RectOffset(Mathf.RoundToInt(cornerRadius * guiHelper.uiScale), Mathf.RoundToInt(cornerRadius * guiHelper.uiScale), Mathf.RoundToInt(cornerRadius * guiHelper.uiScale), Mathf.RoundToInt(cornerRadius * guiHelper.uiScale));

            UnityHelpers.Label(text ?? "Badge", roundedStyle, options);
        }
    }
}
