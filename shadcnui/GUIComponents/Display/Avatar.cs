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
                BeginBorderedAvatar(styleManager, config);

            if (!string.IsNullOrEmpty(config.Name))
                BeginNameLayout(config);
            else if (config.IsOnline)
                layoutComponents.BeginVerticalGroup();

            DrawAvatarImage(config);
            DrawStatusIndicator(config, styleManager);

            if (!string.IsNullOrEmpty(config.Name))
                EndNameLayout(config, styleManager);
            else if (config.IsOnline)
                layoutComponents.EndVerticalGroup();

            if (config.BorderColor != Color.clear)
                layoutComponents.EndVerticalGroup();
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

        #region Private Methods
        private void BeginBorderedAvatar(StyleManager styleManager, AvatarConfig config)
        {
            int borderSize = Mathf.RoundToInt(DesignTokens.Avatar.BorderThickness * guiHelper.uiScale);
            Texture2D borderTexture = styleManager.CreateSolidTexture(config.BorderColor);

            GUIStyle borderedStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            borderedStyle.normal.background = borderTexture;
            borderedStyle.alignment = TextAnchor.MiddleCenter;
            borderedStyle.padding = new UnityHelpers.RectOffset(borderSize, borderSize, borderSize, borderSize);
            layoutComponents.BeginVerticalGroup(borderedStyle, config.Options);
        }

        private void BeginNameLayout(AvatarConfig config)
        {
            if (config.ShowNameBelow)
                layoutComponents.BeginVerticalGroup();
            else
                layoutComponents.BeginHorizontalGroup();
        }

        private void EndNameLayout(AvatarConfig config, StyleManager styleManager)
        {
            if (config.ShowNameBelow)
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                DrawNameLabel(styleManager, config.Name);
                layoutComponents.EndVerticalGroup();
            }
            else
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.SM);
                DrawNameLabel(styleManager, config.Name);
                layoutComponents.EndHorizontalGroup();
            }
        }

        private void DrawNameLabel(StyleManager styleManager, string name)
        {
            GUIStyle baseStyle = styleManager?.GetLabelStyle(ControlVariant.Default) ?? GUI.skin.label;
            GUIStyle nameStyle = new UnityHelpers.GUIStyle(baseStyle);
            nameStyle.fontStyle = FontStyle.Bold;
            if (!baseStyle.name.Contains("center"))
                nameStyle.alignment = TextAnchor.MiddleLeft;
            UnityHelpers.Label(name, nameStyle);
        }

        private void DrawAvatarImage(AvatarConfig config)
        {
            if (config.Rect.HasValue)
                DrawAvatarRect(config);
            else
                DrawAvatarLayout(config);
        }

        private void DrawAvatarLayout(AvatarConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle avatarStyle = styleManager.GetAvatarStyle(config.Size, config.Shape);
            GUILayoutOption[] layoutOptions = config.Options ?? System.Array.Empty<GUILayoutOption>();

            if (config.Image != null)
                UnityHelpers.Label(config.Image, avatarStyle, layoutOptions);
            else
                UnityHelpers.Label(config.FallbackText ?? "A", avatarStyle, layoutOptions);
        }

        private void DrawAvatarRect(AvatarConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle avatarStyle = styleManager.GetAvatarStyle(config.Size, config.Shape);

            Rect scaledRect = new Rect(config.Rect.Value.x * guiHelper.uiScale, config.Rect.Value.y * guiHelper.uiScale, config.Rect.Value.width * guiHelper.uiScale, config.Rect.Value.height * guiHelper.uiScale);

            if (config.Image != null)
                GUI.Label(scaledRect, config.Image, avatarStyle);
            else
                GUI.Label(scaledRect, new GUIContent(config.FallbackText ?? "A"), avatarStyle);
        }

        private void DrawStatusIndicator(AvatarConfig config, StyleManager styleManager)
        {
            if (!config.IsOnline || styleManager == null || config.Rect.HasValue)
                return;

            var avatarRect = GUILayoutUtility.GetLastRect();
            float indicatorSize = styleManager.GetStatusIndicatorSize(config.Size) * guiHelper.uiScale;
            float x = avatarRect.x + avatarRect.width - indicatorSize * 0.9f;
            float y = avatarRect.y + avatarRect.height - indicatorSize * 0.9f;

            Rect dotRect = new Rect(x, y, indicatorSize, indicatorSize);
            DrawStatusDot(dotRect, styleManager);
        }

        private void DrawStatusDot(Rect rect, StyleManager styleManager)
        {
            Texture2D statusTexture = styleManager.CreateStatusIndicator((int)rect.width, true);

            GUIStyle statusStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
            statusStyle.normal.background = statusTexture;
            statusStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
            statusStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);
            statusStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);

            GUI.Box(rect, GUIContent.none, statusStyle);
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
        #endregion
    }
}
