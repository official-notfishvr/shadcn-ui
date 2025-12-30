using System;
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
    public class Avatar : BaseComponent
    {
        public Avatar(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public void DrawAvatar(AvatarConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (config.BorderColor != Color.clear)
            {
                GUIStyle borderedStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                borderedStyle.normal.background = styleManager.CreateSolidTexture(config.BorderColor);
                borderedStyle.alignment = TextAnchor.MiddleCenter;
                borderedStyle.padding = new UnityHelpers.RectOffset((int)DesignTokens.Spacing.XXS, (int)DesignTokens.Spacing.XXS, (int)DesignTokens.Spacing.XXS, (int)DesignTokens.Spacing.XXS);
                layoutComponents.BeginVerticalGroup(borderedStyle, config.Options);
            }

            if (!string.IsNullOrEmpty(config.Name))
            {
                if (config.ShowNameBelow)
                    layoutComponents.BeginVerticalGroup();
                else
                    layoutComponents.BeginHorizontalGroup();
            }
            else if (config.IsOnline)
            {
                layoutComponents.BeginVerticalGroup();
            }

            if (config.Rect.HasValue)
            {
                DrawAvatarRect(config.Rect.Value, config.Image, config.FallbackText, config.Size, config.Shape);
            }
            else
            {
                DrawAvatarInternal(config.Image, config.FallbackText, config.Size, config.Shape, config.Options);
            }

            if (config.IsOnline && styleManager != null && !config.Rect.HasValue)
            {
                var avatarRect = GUILayoutUtility.GetLastRect();
                float indicatorSize = styleManager.GetStatusIndicatorSize(config.Size) * guiHelper.uiScale;
                float x = avatarRect.x + avatarRect.width - indicatorSize * 0.9f;
                float y = avatarRect.y + avatarRect.height - indicatorSize * 0.9f;
                Rect dotRect = new Rect(x, y, indicatorSize, indicatorSize);
                DrawStatusDot(dotRect, true);
            }

            if (!string.IsNullOrEmpty(config.Name))
            {
                if (config.ShowNameBelow)
                {
                    layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                    GUIStyle baseStyle = styleManager?.GetLabelStyle(ControlVariant.Default) ?? GUI.skin.label;
                    GUIStyle nameStyle = new UnityHelpers.GUIStyle(baseStyle);
                    UnityHelpers.Label(config.Name, nameStyle);
                    layoutComponents.EndVerticalGroup();
                }
                else
                {
                    layoutComponents.AddSpace(DesignTokens.Spacing.SM);
                    GUIStyle baseStyle = styleManager?.GetLabelStyle(ControlVariant.Default) ?? GUI.skin.label;
                    GUIStyle nameStyle = new UnityHelpers.GUIStyle(baseStyle);
                    nameStyle.alignment = TextAnchor.MiddleLeft;
                    UnityHelpers.Label(config.Name, nameStyle);
                    layoutComponents.EndHorizontalGroup();
                }
            }
            else if (config.IsOnline)
            {
                layoutComponents.EndVerticalGroup();
            }

            if (config.BorderColor != Color.clear)
            {
                layoutComponents.EndVerticalGroup();
            }
        }
        #endregion

        #region Internal Drawing
        private void DrawAvatarInternal(Texture2D image, string fallbackText, ControlSize size, AvatarShape shape, GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle avatarStyle = styleManager.GetAvatarStyle(size, shape);
            GUIStyle fallbackStyle = styleManager.GetAvatarStyle(size, shape);

            GUILayoutOption[] layoutOptions = options != null ? new List<GUILayoutOption>(options).ToArray() : Array.Empty<GUILayoutOption>();

            if (image != null)
            {
                UnityHelpers.Label(image, avatarStyle, layoutOptions);
            }
            else
            {
                UnityHelpers.Label(fallbackText ?? "A", fallbackStyle, layoutOptions);
            }
        }

        private void DrawAvatarRect(Rect rect, Texture2D image, string fallbackText, ControlSize size, AvatarShape shape)
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

        private void DrawStatusDot(Rect rect, bool isOnline)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
                return;

            Color statusColor = isOnline ? Color.green : Color.gray;

            GUIStyle statusStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            statusStyle.normal.background = styleManager.CreateSolidTexture(statusColor);
            GUI.Box(rect, GUIContent.none, statusStyle);
        }
        #endregion

        #region API
        public void DrawAvatar(Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = image,
                    FallbackText = fallbackText,
                    Size = size,
                    Shape = shape,
                    Options = options,
                }
            );
        }

        public void DrawAvatar(Rect rect, Texture2D image, string fallbackText, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Rect = rect,
                    Image = image,
                    FallbackText = fallbackText,
                    Size = size,
                    Shape = shape,
                }
            );
        }

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = image,
                    FallbackText = fallbackText,
                    Size = size,
                    Shape = shape,
                    IsOnline = isOnline,
                    Options = options,
                }
            );
        }

        public void AvatarWithName(Texture2D image, string fallbackText, string name, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = image,
                    FallbackText = fallbackText,
                    Name = name,
                    Size = size,
                    Shape = shape,
                    ShowNameBelow = showNameBelow,
                    Options = options,
                }
            );
        }

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = image,
                    FallbackText = fallbackText,
                    BorderColor = borderColor,
                    Size = size,
                    Shape = shape,
                    Options = options,
                }
            );
        }
        #endregion

        #region Data Types
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
        #endregion
    }
}
