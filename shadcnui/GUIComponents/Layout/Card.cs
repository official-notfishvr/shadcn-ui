using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Card : BaseComponent
    {
        public Card(GUIHelper helper)
            : base(helper) { }

        public void DrawCard(CardConfig config)
        {
            if (config == null)
                return;

            BeginCard(config.Width, config.Height, config.Variant, config.Size, config.Appearance);

            if (config.Image != null)
            {
                DrawImage(config.Image);
                layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            }

            if (config.HeaderContent != null || !string.IsNullOrEmpty(config.Title) || !string.IsNullOrEmpty(config.Subtitle) || !string.IsNullOrEmpty(config.Description) || config.Avatar != null)
            {
                CardHeader(() => DrawHeaderContent(config));
            }

            if (!string.IsNullOrEmpty(config.Content))
            {
                CardContent(() =>
                {
                    var textStyle = styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label;
                    UnityHelpers.Label(config.Content, textStyle);
                });
            }

            if (config.FooterContent != null)
                CardFooter(config.FooterContent);

            EndCard();
        }

        public void DrawCard(string title, string desc, string content, Action footer = null, float width = -1f, float height = -1f)
        {
            DrawCard(
                new CardConfig
                {
                    Title = title,
                    Description = desc,
                    Content = content,
                    FooterContent = footer,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawCardWithImage(Texture2D img, string title, string desc, string content, Action footer = null, float width = -1f, float height = -1f)
        {
            DrawCard(
                new CardConfig
                {
                    Image = img,
                    Title = title,
                    Description = desc,
                    Content = content,
                    FooterContent = footer,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawCardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footer = null, float width = -1f, float height = -1f)
        {
            DrawCard(
                new CardConfig
                {
                    Avatar = avatar,
                    Title = title,
                    Subtitle = subtitle,
                    Content = content,
                    FooterContent = footer,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawSimpleCard(string content, float width = -1f, float height = -1f)
        {
            DrawCard(
                new CardConfig
                {
                    Content = content,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void BeginCard(float width = -1f, float height = -1f) => BeginCard(width, height, ControlVariant.Default, ControlSize.Default, null);

        public void BeginCard(float width, float height, ControlVariant variant, ControlSize size, ComponentAppearance appearance = null)
        {
            var style = styleManager?.GetCardStyle(variant, size, appearance) ?? GUI.skin.box;
            var options = BuildSizeOptions(width, height);
            layoutComponents.BeginVerticalGroup(style, options);
        }

        public void EndCard() => layoutComponents.EndVerticalGroup();

        public void CardHeader(Action content)
        {
            var headerStyle = styleManager?.GetCardHeaderStyle() ?? GUIStyle.none;
            layoutComponents.BeginVerticalGroup(headerStyle);
            content?.Invoke();
            layoutComponents.EndVerticalGroup();
        }

        public void CardTitle(string title, ComponentAppearance appearance = null)
        {
            if (string.IsNullOrEmpty(title))
                return;
            var titleStyle = styleManager?.GetCardTitleStyle(appearance) ?? GUI.skin.label;
            UnityHelpers.Label(title, titleStyle);
        }

        public void CardDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return;
            var descStyle = styleManager?.GetCardDescriptionStyle() ?? GUI.skin.label;
            UnityHelpers.Label(description, descStyle);
        }

        public void CardContent(Action content)
        {
            var contentStyle = styleManager?.GetCardContentStyle() ?? GUIStyle.none;
            layoutComponents.BeginVerticalGroup(contentStyle);
            content?.Invoke();
            layoutComponents.EndVerticalGroup();
        }

        public void CardFooter(Action content)
        {
            var footerStyle = styleManager?.GetCardFooterStyle() ?? GUIStyle.none;
            layoutComponents.BeginHorizontalGroup(footerStyle);
            content?.Invoke();
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawHeaderContent(CardConfig config)
        {
            if (config.Avatar != null)
            {
                layoutComponents.BeginHorizontalGroup();
                DrawAvatar(config.Avatar);
                layoutComponents.AddSpace(DesignTokens.Spacing.SM);
                layoutComponents.BeginVerticalGroup();
            }

            if (!string.IsNullOrEmpty(config.Title))
                CardTitle(config.Title);

            if (!string.IsNullOrEmpty(config.Subtitle))
                CardDescription(config.Subtitle);
            else if (!string.IsNullOrEmpty(config.Description))
                CardDescription(config.Description);

            if (config.Avatar != null)
            {
                layoutComponents.EndVerticalGroup();
                layoutComponents.EndHorizontalGroup();
                return;
            }

            config.HeaderContent?.Invoke();
        }

        private void DrawImage(Texture2D image)
        {
            if (image == null)
                return;

            var height = 140f * guiHelper.uiScale;
            var rect = ControlLayoutUtility.ReserveRect(UnityHelpers.GUIContent.none, GUIStyle.none, ControlLayoutUtility.BuildLayoutOptions(null, fixedHeight: height, expandWidth: true));
            GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit);
        }

        private void DrawAvatar(Texture2D image)
        {
            if (image == null)
                return;

            var size = DesignTokens.Height.Small * guiHelper.uiScale;
            var rect = SurfaceDrawUtility.ReserveSquare(size);
            GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit);
        }

        private GUILayoutOption[] BuildSizeOptions(float width, float height)
        {
            return ControlLayoutUtility.BuildLayoutOptions(null, width > 0f ? width * guiHelper.uiScale : 0f, height > 0f ? height * guiHelper.uiScale : 0f).ToArray();
        }
    }
}
