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
    public class Avatar
    {
        private GUIHelper guiHelper;
        private shadcnui.GUIComponents.Layout.Layout layoutComponents;

        public Avatar(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new shadcnui.GUIComponents.Layout.Layout(helper);
        }

        public void DrawAvatar(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle avatarStyle = styleManager.GetAvatarStyle(size, shape);
            GUIStyle fallbackStyle = styleManager.GetAvatarStyle(size, shape);

            GUILayoutOption[] layoutOptions = new List<GUILayoutOption>(options).ToArray();

            if (image != null)
            {
                UnityHelpers.Label(image, avatarStyle, layoutOptions);
            }
            else
            {
                UnityHelpers.Label(fallbackText ?? "A", fallbackStyle, layoutOptions);
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

            UnityHelpers.Label(GUIContent.none, statusStyle);
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

                    UnityHelpers.Label(name, nameStyle);
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

                    UnityHelpers.Label(name, nameStyle);
                }

                layoutComponents.EndHorizontalGroup();
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
