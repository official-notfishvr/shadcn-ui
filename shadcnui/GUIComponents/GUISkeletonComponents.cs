using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUISkeletonComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUISkeletonComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public void Skeleton(float width, float height, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle skeletonStyle = styleManager.GetSkeletonStyle(variant, size);

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

#if IL2CPP
            GUILayout.Box("", skeletonStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight) });
#else
            GUILayout.Box("", skeletonStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
        }

        public void AnimatedSkeleton(float width, float height, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            float time = Time.time * 2f;
            float alpha = (Mathf.Sin(time) + 1f) * 0.5f * 0.3f + 0.7f;

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            Skeleton(width, height, variant, size, options);

            GUI.color = originalColor;
        }

        public void ShimmerSkeleton(float width, float height, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            float time = Time.time * 3f;
            float shimmerPos = (Mathf.Sin(time) + 1f) * 0.5f;

            GUIStyle skeletonStyle = styleManager.GetSkeletonStyle(variant, size);

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);

#if IL2CPP
            GUILayout.Box("", skeletonStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight) });
#else
            GUILayout.Box("", skeletonStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
            GUI.color = originalColor;
        }

        public void CustomSkeleton(float width, float height, Color backgroundColor, Color shimmerColor, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.border = GetSkeletonBorder(variant, size);

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

#if IL2CPP
            GUILayout.Box("", customStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight) });
#else
            GUILayout.Box("", customStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
        }

        public void SkeletonText(float width, int lines, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();
            for (int i = 0; i < lines; i++)
            {
                float currentWidth = width * guiHelper.uiScale;
                if (i == lines - 1 && lines > 1)
                    currentWidth *= 0.7f;

                Skeleton(currentWidth, guiHelper.fontSize * 1.2f, SkeletonVariant.Default, SkeletonSize.Default, options);
                if (i < lines - 1)
                {
                    layoutComponents.AddSpace(guiHelper.fontSize * 0.3f);
                }
            }
            layoutComponents.EndVerticalGroup();
        }

        public void SkeletonAvatar(float size, SkeletonVariant variant, params GUILayoutOption[] options)
        {
            Skeleton((float)size, (float)size, variant, SkeletonSize.Default, options ?? new GUILayoutOption[0]);
        }

        public void SkeletonButton(float width, float height, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            Skeleton((float)width, (float)height, variant, size, options ?? new GUILayoutOption[0]);
        }

        public void SkeletonCard(float width, float height, bool includeHeader, bool includeFooter, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();

            if (includeHeader)
            {
                layoutComponents.BeginVerticalGroup();
                SkeletonText(width * 0.8f, 1);
                layoutComponents.AddSpace(guiHelper.fontSize * 0.3f);
                SkeletonText(width * 0.6f, 1);
                layoutComponents.EndVerticalGroup();
                layoutComponents.AddSpace(guiHelper.controlSpacing);
            }

            SkeletonText(width, 3);

            if (includeFooter)
            {
                layoutComponents.AddSpace(guiHelper.controlSpacing);
                layoutComponents.BeginHorizontalGroup();
                Skeleton(width * 0.3f, guiHelper.fontSize * 1.5f, SkeletonVariant.Default, SkeletonSize.Default, options);
                layoutComponents.AddSpace(guiHelper.controlSpacing);
                Skeleton(width * 0.3f, guiHelper.fontSize * 1.5f, SkeletonVariant.Default, SkeletonSize.Default, options);
                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void SkeletonTable(float width, int rows, int columns, float cellHeight, float cellSpacing, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();
            for (int r = 0; r < rows; r++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int c = 0; c < columns; c++)
                {
                    Skeleton(width / columns - cellSpacing, cellHeight, SkeletonVariant.Default, SkeletonSize.Default, options);
                    if (c < columns - 1)
                    {
                        layoutComponents.AddSpace(cellSpacing);
                    }
                }
                layoutComponents.EndHorizontalGroup();
                if (r < rows - 1)
                {
                    layoutComponents.AddSpace(cellSpacing);
                }
            }
            layoutComponents.EndVerticalGroup();
        }

        public void SkeletonList(float itemWidth, float itemHeight, int itemCount, float spacing, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();
            for (int i = 0; i < itemCount; i++)
            {
                Skeleton(itemWidth, itemHeight, SkeletonVariant.Default, SkeletonSize.Default, options);
                if (i < itemCount - 1)
                {
                    layoutComponents.AddSpace(spacing);
                }
            }
            layoutComponents.EndVerticalGroup();
        }

        public void CustomShapeSkeleton(float width, float height, float cornerRadius, Color backgroundColor, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.border = new RectOffset(Mathf.RoundToInt(cornerRadius * guiHelper.uiScale), Mathf.RoundToInt(cornerRadius * guiHelper.uiScale), Mathf.RoundToInt(cornerRadius * guiHelper.uiScale), Mathf.RoundToInt(cornerRadius * guiHelper.uiScale));

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

#if IL2CPP
            GUILayout.Box("", customStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight) });
#else
            GUILayout.Box("", customStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
        }

        public void SkeletonWithProgress(float width, float height, float progress, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();

            Skeleton(width, height, variant, size, options);

            GUILayout.Space(4 * guiHelper.uiScale);
            Rect progressRect = GUILayoutUtility.GetRect(width * guiHelper.uiScale, 4 * guiHelper.uiScale);

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle bgStyle = new GUIStyle(GUI.skin.box);
                bgStyle.normal.background = styleManager.CreateSolidTexture(Color.gray);
                GUI.Box(progressRect, "", bgStyle);

                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * Mathf.Clamp01(progress), progressRect.height);
                GUIStyle fillStyle = new GUIStyle(GUI.skin.box);
                fillStyle.normal.background = styleManager.CreateSolidTexture(Color.blue);
                GUI.Box(fillRect, "", fillStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        public void FadeSkeleton(float width, float height, float fadeTime, SkeletonVariant variant, SkeletonSize size, params GUILayoutOption[] options)
        {
            float time = Time.time;
            float alpha = Mathf.Clamp01(1f - (time / fadeTime));

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            Skeleton(width, height, variant, size, options);

            GUI.color = originalColor;
        }

        private RectOffset GetSkeletonBorder(SkeletonVariant variant, SkeletonSize size)
        {
            float scale = guiHelper.uiScale;
            int borderRadius = 0;

            switch (variant)
            {
                case SkeletonVariant.Circular:
                    borderRadius = Mathf.RoundToInt(50 * scale);
                    break;
                case SkeletonVariant.Rounded:
                    borderRadius = Mathf.RoundToInt(8 * scale);
                    break;
                case SkeletonVariant.Default:
                    borderRadius = Mathf.RoundToInt(4 * scale);
                    break;
            }

            return new RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }
    }
}
