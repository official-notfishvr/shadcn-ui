using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Avatar
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Avatar(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public void DrawAvatar(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle avatarStyle = styleManager.GetAvatarStyle(size, shape);
            GUIStyle fallbackStyle = styleManager.GetAvatarStyle(size, shape);

            GUILayoutOption[] layoutOptions = new List<GUILayoutOption>(options).ToArray();

            if (image != null)
            {
#if IL2CPP_MELONLOADER
                GUILayout.Label(image, avatarStyle, (Il2CppReferenceArray<GUILayoutOption>)layoutOptions);
#else
                GUILayout.Label(image, avatarStyle, layoutOptions);
#endif
            }
            else
            {
#if IL2CPP_MELONLOADER
                GUILayout.Label(new GUIContent(fallbackText ?? "A"), fallbackStyle, (Il2CppReferenceArray<GUILayoutOption>)layoutOptions);
#else
                GUILayout.Label(fallbackText ?? "A", fallbackStyle, layoutOptions);
#endif
            }
        }

        public void DrawAvatar(Rect rect, Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle avatarStyle = styleManager.GetAvatarStyle(size, shape);
            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            if (image != null)
            {
                GUI.Label(scaledRect, image, avatarStyle);
            }
            else
            {
                GUIStyle fallbackStyle = styleManager.GetAvatarStyle(size, shape);
                GUI.Label(scaledRect, new GUIContent(fallbackText ?? "A"), fallbackStyle);
            }
        }

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            layoutComponents.BeginHorizontalGroup();
            DrawAvatar(image, fallbackText, size, shape, options);

            if (isOnline)
            {
                layoutComponents.AddSpace(-8);
                DrawStatusIndicator(isOnline, size);
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawStatusIndicator(bool isOnline, AvatarSize size)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
                return;

            Color statusColor = isOnline ? Color.green : Color.gray;
            float indicatorSize = styleManager.GetStatusIndicatorSize(size);

            GUIStyle statusStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            statusStyle.normal.background = styleManager.CreateSolidTexture(statusColor);
            statusStyle.fixedWidth = indicatorSize * guiHelper.uiScale;
            statusStyle.fixedHeight = indicatorSize * guiHelper.uiScale;
            statusStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
            statusStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            statusStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);

#if IL2CPP_MELONLOADER
            GUILayout.Label(GUIContent.none, statusStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
            GUILayout.Label(GUIContent.none, statusStyle);
#endif
        }

        public void AvatarWithName(Texture2D image, string fallbackText, string name, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options)
        {
            if (showNameBelow)
            {
                layoutComponents.BeginVerticalGroup();
                DrawAvatar(image, fallbackText, size, shape, options);

                if (!string.IsNullOrEmpty(name))
                {
                    layoutComponents.AddSpace(4);
                    var styleManager = guiHelper.GetStyleManager();
                    GUIStyle nameStyle = styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;
#if IL2CPP_MELONLOADER
                    GUILayout.Label(new GUIContent(name), nameStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                    GUILayout.Label(name, nameStyle);
#endif
                }

                layoutComponents.EndVerticalGroup();
            }
            else
            {
                layoutComponents.BeginHorizontalGroup();
                DrawAvatar(image, fallbackText, size, shape, options);

                if (!string.IsNullOrEmpty(name))
                {
                    layoutComponents.AddSpace(8);
                    var styleManager = guiHelper.GetStyleManager();
                    GUIStyle nameStyle = styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;
                    nameStyle.alignment = TextAnchor.MiddleLeft;
#if IL2CPP_MELONLOADER
                    GUILayout.Label(new GUIContent(name), nameStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                    GUILayout.Label(name, nameStyle);
#endif
                }

                layoutComponents.EndHorizontalGroup();
            }
        }

        public void CustomAvatar(Texture2D image, string fallbackText, Color backgroundColor, Color textColor, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.normal.textColor = textColor;
            customStyle.alignment = TextAnchor.MiddleCenter;
            customStyle.fontSize = GetAvatarFontSize(size);
            customStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            customStyle.border = GetAvatarBorder(shape, size);

            if (image != null)
            {
#if IL2CPP_MELONLOADER
                GUILayout.Label(image, customStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
                GUILayout.Label(image, customStyle, options);
#endif
            }
            else
            {
#if IL2CPP_MELONLOADER
                GUILayout.Label(new GUIContent(fallbackText ?? "A"), customStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
                GUILayout.Label(fallbackText ?? "A", customStyle, options);
#endif
            }
        }

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle borderedStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            borderedStyle.normal.background = styleManager.CreateSolidTexture(borderColor);
            borderedStyle.alignment = TextAnchor.MiddleCenter;
            borderedStyle.padding = new UnityHelpers.RectOffset(2, 2, 2, 2);

            layoutComponents.BeginVerticalGroup(borderedStyle, options);
            DrawAvatar(image, fallbackText, size, shape, options);
            layoutComponents.EndVerticalGroup();
        }

        public void AvatarWithHover(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, Action onClick = null, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            bool clicked = guiHelper.Button("", ButtonVariant.Ghost, ButtonSize.Icon, onClick: onClick, options: options);

            Rect lastRect = GUILayoutUtility.GetLastRect();
            if (image != null)
            {
                GUI.Label(lastRect, image, styleManager.GetAvatarStyle(size, shape));
            }
            else
            {
                GUI.Label(lastRect, new GUIContent(fallbackText ?? "A"), styleManager.GetAvatarStyle(size, shape));
            }
        }

        public void AvatarWithLoading(Texture2D image, string fallbackText, bool isLoading, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            if (isLoading)
            {
                float time = Time.time * 2f;
                float alpha = (Mathf.Sin(time) + 1f) * 0.5f * 0.3f + 0.7f;

                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

                DrawAvatar(image, fallbackText, size, shape, options);

                GUI.color = originalColor;
            }
            else
            {
                DrawAvatar(image, fallbackText, size, shape, options);
            }
        }

        public void AvatarWithTooltip(Texture2D image, string fallbackText, string tooltip, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            layoutComponents.BeginHorizontalGroup();

            DrawAvatar(image, fallbackText, size, shape, options);

            if (!string.IsNullOrEmpty(tooltip))
            {
                layoutComponents.AddSpace(4);

                var styleManager = guiHelper.GetStyleManager();
                GUIStyle tooltipStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

#if IL2CPP_MELONLOADER
                GUILayout.Label(new GUIContent("?"), tooltipStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
                GUILayout.Label("?", tooltipStyle, options);
#endif
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void AvatarGroup(AvatarData[] avatars, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, int maxVisible = 3, float overlap = -8f, params GUILayoutOption[] options)
        {
            if (avatars == null || avatars.Length == 0)
                return;

            layoutComponents.BeginHorizontalGroup();

            int visibleCount = Mathf.Min(avatars.Length, maxVisible);

            for (int i = 0; i < visibleCount; i++)
            {
                var avatar = avatars[i];

                if (i > 0)
                    layoutComponents.AddSpace(overlap);

                DrawAvatar(avatar.Image, avatar.FallbackText, size, shape, options);
            }

            if (avatars.Length > maxVisible)
            {
                layoutComponents.AddSpace(overlap);
                int remainingCount = avatars.Length - maxVisible;
                string countText = $"+{remainingCount}";

                var styleManager = guiHelper.GetStyleManager();
                GUIStyle countStyle = styleManager?.GetAvatarStyle(size, shape) ?? GUI.skin.box;
                countStyle.normal.textColor = Color.white;
                countStyle.normal.background = styleManager?.CreateSolidTexture(Color.gray) ?? GUI.skin.box.normal.background;

#if IL2CPP_MELONLOADER
                GUILayout.Label(new GUIContent(countText), countStyle, (Il2CppReferenceArray<GUILayoutOption>)options);
#else
                GUILayout.Label(countText, countStyle, options);
#endif
            }

            layoutComponents.EndHorizontalGroup();
        }

        private int GetAvatarFontSize(AvatarSize size)
        {
            switch (size)
            {
                case AvatarSize.Small:
                    return Mathf.RoundToInt((guiHelper.fontSize - 2) * guiHelper.uiScale);
                case AvatarSize.Large:
                    return Mathf.RoundToInt((guiHelper.fontSize + 4) * guiHelper.uiScale);
                default:
                    return Mathf.RoundToInt(guiHelper.fontSize * guiHelper.uiScale);
            }
        }

        private RectOffset GetAvatarBorder(AvatarShape shape, AvatarSize size)
        {
            float scale = guiHelper.uiScale;
            int borderRadius = 0;

            switch (shape)
            {
                case AvatarShape.Circle:
                    borderRadius = Mathf.RoundToInt(50 * scale);
                    break;
                case AvatarShape.Rounded:
                    borderRadius = Mathf.RoundToInt(8 * scale);
                    break;
                case AvatarShape.Square:
                    borderRadius = 0;
                    break;
            }

            return new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }

        public struct AvatarData
        {
            public Texture2D Image;
            public string FallbackText;
            public bool IsOnline;
            public string Name;

            public AvatarData(Texture2D image, string fallbackText, bool isOnline = false, string name = "")
            {
                Image = image;
                FallbackText = fallbackText;
                IsOnline = isOnline;
                Name = name;
            }
        }
    }
}
