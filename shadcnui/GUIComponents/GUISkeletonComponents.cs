using shadcnui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUISkeletonComponents
    {
        private GUIHelper guiHelper;

        public GUISkeletonComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void Skeleton(float width, float height, SkeletonVariant variant = SkeletonVariant.Default, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));
                return;
            }

            GUIStyle skeletonStyle = styleManager.GetSkeletonStyle(variant, size);

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

#if IL2CPP
            GUILayout.Box("", skeletonStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(scaledWidth), 
                    GUILayout.Height(scaledHeight) 
                });
#else
            GUILayout.Box("", skeletonStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
        }

        public void Skeleton(Rect rect, SkeletonVariant variant = SkeletonVariant.Default, SkeletonSize size = SkeletonSize.Default)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUI.Box(rect, "");
                return;
            }

            GUIStyle skeletonStyle = styleManager.GetSkeletonStyle(variant, size);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            GUI.Box(scaledRect, "", skeletonStyle);
        }

        public void AnimatedSkeleton(float width, float height, SkeletonVariant variant = SkeletonVariant.Default, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            float time = Time.time * 2f;
            float alpha = (Mathf.Sin(time) + 1f) * 0.5f * 0.3f + 0.7f;
            
            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
            Skeleton(width, height, variant, size, options);
            
            GUI.color = originalColor;
        }

        public void ShimmerSkeleton(float width, float height, SkeletonVariant variant = SkeletonVariant.Default, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));
                return;
            }

            float time = Time.time * 3f;
            float shimmerPos = (Mathf.Sin(time) + 1f) * 0.5f;
            
            GUIStyle skeletonStyle = styleManager.GetSkeletonStyle(variant, size);
            
            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
            
#if IL2CPP
            GUILayout.Box("", skeletonStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(scaledWidth), 
                    GUILayout.Height(scaledHeight) 
                });
#else
            GUILayout.Box("", skeletonStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
            
            GUI.color = originalColor;
        }

        public void CustomSkeleton(float width, float height, Color backgroundColor, Color shimmerColor, 
            SkeletonVariant variant = SkeletonVariant.Default, SkeletonSize size = SkeletonSize.Default, 
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));
                return;
            }

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.border = GetSkeletonBorder(variant, size);

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

#if IL2CPP
            GUILayout.Box("", customStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(scaledWidth), 
                    GUILayout.Height(scaledHeight) 
                });
#else
            GUILayout.Box("", customStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
        }

        public void SkeletonText(int lineCount, float lineHeight = 20f, SkeletonVariant variant = SkeletonVariant.Default, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            for (int i = 0; i < lineCount; i++)
            {
                float lineWidth = 200f;
                if (i == lineCount - 1 && lineCount > 1)
                {
                    lineWidth = 120f;
                }
                else if (i % 3 == 0)
                {
                    lineWidth = 180f;
                }
                
                Skeleton(lineWidth, lineHeight, variant, size, options);
                
                if (i < lineCount - 1)
                {
                    GUILayout.Space(4 * guiHelper.uiScale);
                }
            }
            
            GUILayout.EndVertical();
        }

        public void SkeletonAvatar(float size, SkeletonVariant variant = SkeletonVariant.Circular, 
            params GUILayoutOption[] options)
        {
            Skeleton(size, size, variant, SkeletonSize.Default, options);
        }

        public void SkeletonButton(float width = 120f, float height = 36f, SkeletonVariant variant = SkeletonVariant.Rounded, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            Skeleton(width, height, variant, size, options);
        }

        public void SkeletonCard(float width = 300f, float height = 200f, SkeletonVariant variant = SkeletonVariant.Rounded, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            Skeleton(width, 40f, variant, size, options);
            GUILayout.Space(8 * guiHelper.uiScale);
            
            SkeletonText(3, 16f, variant, size, options);
            GUILayout.Space(8 * guiHelper.uiScale);
            
            Skeleton(width * 0.6f, 30f, variant, size, options);
            
            GUILayout.EndVertical();
        }

        public void SkeletonTable(int rowCount, int columnCount, float cellWidth = 100f, float cellHeight = 30f, 
            SkeletonVariant variant = SkeletonVariant.Default, SkeletonSize size = SkeletonSize.Default, 
            params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                for (int col = 0; col < columnCount; col++)
                {
                    Skeleton(cellWidth, cellHeight, variant, size, options);
                    
                    if (col < columnCount - 1)
                    {
                        GUILayout.Space(4 * guiHelper.uiScale);
                    }
                }
                
                GUILayout.EndHorizontal();
                
                if (row < rowCount - 1)
                {
                    GUILayout.Space(4 * guiHelper.uiScale);
                }
            }
            
            GUILayout.EndVertical();
        }

        public void SkeletonList(int itemCount, float itemHeight = 60f, SkeletonVariant variant = SkeletonVariant.Rounded, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            for (int i = 0; i < itemCount; i++)
            {
                GUILayout.BeginHorizontal();
                
                SkeletonAvatar(40f, variant, options);
                GUILayout.Space(8 * guiHelper.uiScale);
                
                GUILayout.BeginVertical();
                Skeleton(150f, 16f, variant, size, options);
                GUILayout.Space(4 * guiHelper.uiScale);
                Skeleton(100f, 12f, variant, size, options);
                GUILayout.EndVertical();
                
                GUILayout.EndHorizontal();
                
                if (i < itemCount - 1)
                {
                    GUILayout.Space(8 * guiHelper.uiScale);
                }
            }
            
            GUILayout.EndVertical();
        }

        public void CustomShapeSkeleton(float width, float height, float cornerRadius, Color backgroundColor, 
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));
                return;
            }

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.border = new RectOffset(
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale),
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale),
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale),
                Mathf.RoundToInt(cornerRadius * guiHelper.uiScale)
            );

            float scaledWidth = width * guiHelper.uiScale;
            float scaledHeight = height * guiHelper.uiScale;

#if IL2CPP
            GUILayout.Box("", customStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(scaledWidth), 
                    GUILayout.Height(scaledHeight) 
                });
#else
            GUILayout.Box("", customStyle, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
#endif
        }

        public void SkeletonWithProgress(float width, float height, float progress, SkeletonVariant variant = SkeletonVariant.Default, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
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
            
            GUILayout.EndVertical();
        }

        public void FadeSkeleton(float width, float height, float fadeTime, SkeletonVariant variant = SkeletonVariant.Default, 
            SkeletonSize size = SkeletonSize.Default, params GUILayoutOption[] options)
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
