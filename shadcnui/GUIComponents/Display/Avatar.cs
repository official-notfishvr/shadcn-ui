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

        public void DrawAvatar(AvatarConfig config)
        {
            if (config == null)
                return;

            GUIStyle style = styleManager?.GetAvatarStyle(config.Size, config.Shape, config.Variant) ?? GUI.skin.box;

            if (config.Rect.HasValue)
            {
                Rect rect = ScaleRect(config.Rect.Value);
                DrawAvatarRect(rect, config, style);
                return;
            }

            layoutComponents.BeginVerticalGroup();
            Rect rectLayout = GUILayoutUtility.GetRect(style.fixedWidth, style.fixedHeight, (config.LayoutOptions != null && config.LayoutOptions.Length > 0) ? config.LayoutOptions : new[] { GUILayout.Width(style.fixedWidth), GUILayout.Height(style.fixedHeight) });
            DrawAvatarRect(rectLayout, config, style);

            if (config.ShowNameBelow && !string.IsNullOrEmpty(config.Name))
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                UnityHelpers.Label(config.Name, styleManager?.GetLabelStyle(ControlVariant.Default, config.Size) ?? GUI.skin.label);
            }

            layoutComponents.EndVerticalGroup();
        }

        public void DrawAvatar(Texture2D img, string fallback, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = img,
                    FallbackText = fallback,
                    Size = size,
                    Shape = shape,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void AvatarWithStatus(Texture2D img, string fallback, bool online, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = img,
                    FallbackText = fallback,
                    Size = size,
                    Shape = shape,
                    IsOnline = online,
                    ShowNameBelow = false,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void AvatarWithName(Texture2D img, string fallback, string name, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = img,
                    FallbackText = fallback,
                    Name = name,
                    Size = size,
                    Shape = shape,
                    ShowNameBelow = showNameBelow,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void AvatarWithBorder(Texture2D img, string fallback, Color borderColor, ControlSize size = ControlSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options)
        {
            DrawAvatar(
                new AvatarConfig
                {
                    Image = img,
                    FallbackText = fallback,
                    Size = size,
                    Shape = shape,
                    BorderColor = borderColor,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        private void DrawAvatarRect(Rect rect, AvatarConfig config, GUIStyle style)
        {
            GUI.Box(rect, GUIContent.none, style);

            if (config.Image != null)
                GUI.DrawTexture(rect, config.Image, ScaleMode.ScaleToFit);
            else
                DrawFallback(rect, config);

            if (config.BorderColor != Color.clear)
                DrawBorder(rect, config.BorderColor);

            if (config.IsOnline)
                DrawStatusIndicator(rect, true);
        }

        private void DrawFallback(Rect rect, AvatarConfig config)
        {
            string text = !string.IsNullOrEmpty(config.FallbackText) ? config.FallbackText : "?";
            var labelStyle = new GUIStyle(styleManager?.GetLabelStyle(ControlVariant.Default, config.Size) ?? GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            GUI.Label(rect, text, labelStyle);
        }

        private void DrawBorder(Rect rect, Color color)
        {
            float border = Mathf.Max(1f, DesignTokens.Avatar.BorderThickness * guiHelper.uiScale);
            Color prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, border), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - border, rect.width, border), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, border, rect.height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - border, rect.y, border, rect.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private void DrawStatusIndicator(Rect rect, bool online)
        {
            float size = styleManager?.GetStatusIndicatorSize(ControlSize.Small) ?? (10f * guiHelper.uiScale);
            Rect dotRect = new Rect(rect.xMax - size * 0.7f, rect.yMax - size * 0.7f, size, size);
            Color prev = GUI.color;
            var theme = styleManager?.GetTheme();
            GUI.color = online ? (theme?.Accent ?? Color.green) : (theme?.Destructive ?? Color.red);
            GUI.DrawTexture(dotRect, Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private Rect ScaleRect(Rect rect)
        {
            return new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);
        }
    }
}
