using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Avatar : BaseComponent
    {
        public Avatar(GUIHelper helper)
            : base(helper) { }

        public void Render(AvatarConfig config)
        {
            if (config == null)
                return;

            DrawAvatarInternal(config.Image, config.FallbackText, config.Size, config.Shape, config.Variant, config.Appearance, config.BorderColor, config.IsOnline, config.Name, config.ShowNameBelow, config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
        }

        internal void DrawAvatar(Texture2D image, string fallback, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatarInternal(image, fallback, size, shape, ControlVariant.Default, null, Color.clear, false, null, false, options ?? Array.Empty<GUILayoutOption>());
        }

        internal void AvatarWithStatus(Texture2D image, string fallback, bool online, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatarInternal(image, fallback, size, shape, ControlVariant.Default, null, Color.clear, online, null, false, options ?? Array.Empty<GUILayoutOption>());
        }

        internal void AvatarWithName(Texture2D image, string fallback, string name, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options)
        {
            DrawAvatarInternal(image, fallback, size, shape, ControlVariant.Default, null, Color.clear, false, name, showNameBelow, options ?? Array.Empty<GUILayoutOption>());
        }

        internal void AvatarWithBorder(Texture2D image, string fallback, Color borderColor, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatarInternal(image, fallback, size, shape, ControlVariant.Default, null, borderColor, false, null, false, options ?? Array.Empty<GUILayoutOption>());
        }

        private void DrawAvatarInternal(Texture2D image, string fallback, ControlSize size, AvatarShape shape, ControlVariant variant, ComponentAppearance appearance, Color borderColor, bool online, string name, bool showNameBelow, params GUILayoutOption[] options)
        {
            var avatarStyle = styleManager.GetAvatarStyle(size, shape, variant, appearance);
            float dimension = avatarStyle.fixedWidth > 0 ? avatarStyle.fixedWidth : DesignTokens.Height.Default * guiHelper.uiScale;

            if (!string.IsNullOrEmpty(name) && showNameBelow)
            {
                layoutComponents.BeginVerticalGroup();
                DrawAvatarCore(image, fallback, dimension, avatarStyle, borderColor, online, size);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                UnityHelpers.Label(name, styleManager.GetLabelStyle(variant, size, appearance));
                layoutComponents.EndVerticalGroup();
                return;
            }

            if (!string.IsNullOrEmpty(name))
            {
                layoutComponents.BeginHorizontalGroup();
                DrawAvatarCore(image, fallback, dimension, avatarStyle, borderColor, online, size);
                layoutComponents.AddSpace(DesignTokens.Spacing.SM);
                UnityHelpers.Label(name, styleManager.GetLabelStyle(variant, size, appearance));
                layoutComponents.EndHorizontalGroup();
                return;
            }

            DrawAvatarCore(image, fallback, dimension, avatarStyle, borderColor, online, size);
        }

        private void DrawAvatarCore(Texture2D image, string fallback, float dimension, GUIStyle avatarStyle, Color borderColor, bool online, ControlSize size)
        {
            Rect rect = SurfaceDrawUtility.ReserveSquare(dimension);
            GUI.Box(rect, GUIContent.none, avatarStyle);

            if (image != null)
            {
                GUI.DrawTexture(rect, image, ScaleMode.ScaleAndCrop);
            }
            else
            {
                string initials = string.IsNullOrWhiteSpace(fallback) ? "?" : fallback.Trim().Substring(0, 1).ToUpperInvariant();
                GUI.Label(rect, initials, avatarStyle);
            }

            if (borderColor.a > 0f)
            {
                SurfaceDrawUtility.DrawRoundedBorder(styleManager, rect, Mathf.RoundToInt(rect.height / 2f), Color.clear, borderColor, 1f);
            }

            if (!online)
                return;

            float indicatorSize = styleManager.GetStatusIndicatorSize(size);
            Rect indicatorRect = new Rect(rect.xMax - indicatorSize, rect.yMax - indicatorSize, indicatorSize, indicatorSize);
            SurfaceDrawUtility.DrawRoundedBorder(styleManager, indicatorRect, Mathf.RoundToInt(indicatorRect.width / 2f), styleManager.GetTheme().Success, styleManager.GetTheme().Base, 2f);
        }
    }
}
