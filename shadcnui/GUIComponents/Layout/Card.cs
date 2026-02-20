using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Layout
{
    public class Card : BaseComponent
    {
        public Card(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public void DrawCard(CardConfig config)
        {
            BeginCard(config.Width, config.Height);
            DrawCardHeader(config);
            DrawCardImage(config);
            DrawCardContent(config);
            DrawCardFooter(config);
            EndCard();
        }
        #endregion

        #region API
        public void BeginCard(float width = -1, float height = -1)
        {
            var styleManager = guiHelper.GetStyleManager();
            var options = new List<GUILayoutOption>();
            if (width > 0)
                options.Add(GUILayout.Width(width * guiHelper.uiScale));
            if (height > 0)
                options.Add(GUILayout.Height(height * guiHelper.uiScale));

            layoutComponents.BeginVerticalGroup(styleManager.GetCardStyle(), options.ToArray());
        }

        public void EndCard()
        {
            layoutComponents.EndVerticalGroup();
        }

        public void CardHeader(Action content)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetCardHeaderStyle());
            content();
            layoutComponents.EndVerticalGroup();
        }

        public void CardTitle(string title)
        {
            var styleManager = guiHelper.GetStyleManager();
            UnityHelpers.Label(title, styleManager.GetCardTitleStyle());
        }

        public void CardDescription(string description)
        {
            var styleManager = guiHelper.GetStyleManager();
            UnityHelpers.Label(description, styleManager.GetCardDescriptionStyle());
        }

        public void CardContent(Action content)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetCardContentStyle());
            content();
            layoutComponents.EndVerticalGroup();
        }

        public void CardFooter(Action content)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginHorizontalGroup(styleManager.GetCardFooterStyle());
            content();
            layoutComponents.EndHorizontalGroup();
        }

        public void CardImage(Texture2D image, float height = 150)
        {
#if IL2CPP_MELONLOADER_PRE57
            var rect = GUILayoutUtility.GetRect(0, height, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.ExpandWidth(true) }));
#else
            var rect = GUILayoutUtility.GetRect(0, height, GUILayout.ExpandWidth(true));
#endif
            GUI.DrawTexture(rect, image, ScaleMode.ScaleAndCrop);
        }

        public void CardWithAvatar(Texture2D avatar, string title, string subtitle)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginHorizontalGroup(styleManager.GetCardHeaderStyle());
            guiHelper.Avatar(avatar, "");
            layoutComponents.BeginVerticalGroup();
            CardTitle(title);
            CardDescription(subtitle);
            layoutComponents.EndVerticalGroup();
            layoutComponents.EndHorizontalGroup();
        }

        public void CardHeader(string title, string description, Action Actions)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginHorizontalGroup(styleManager.GetCardHeaderStyle());
            layoutComponents.BeginVerticalGroup();
            CardTitle(title);
            CardDescription(description);
            layoutComponents.EndVerticalGroup();
            GUILayout.FlexibleSpace();
            Actions();
            layoutComponents.EndHorizontalGroup();
        }

        public void DrawCard(string title, string description, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            DrawCard(
                new CardConfig
                {
                    Title = title,
                    Description = description,
                    Content = content,
                    FooterContent = footerContent,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawCardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            DrawCard(
                new CardConfig
                {
                    Image = image,
                    Title = title,
                    Description = description,
                    Content = content,
                    FooterContent = footerContent,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawCardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            DrawCard(
                new CardConfig
                {
                    Avatar = avatar,
                    Title = title,
                    Subtitle = subtitle,
                    Content = content,
                    FooterContent = footerContent,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawCardWithHeader(string title, string description, Action header, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            DrawCard(
                new CardConfig
                {
                    Title = title,
                    Description = description,
                    HeaderContent = header,
                    Content = content,
                    FooterContent = footerContent,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawSimpleCard(string content, float width = -1, float height = -1)
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
        #endregion

        #region Private Methods
        private void DrawCardHeader(CardConfig config)
        {
            if (config.Avatar != null)
            {
                CardWithAvatar(config.Avatar, config.Title, config.Subtitle);
            }
            else if (config.HeaderContent != null)
            {
                CardHeader(config.Title, config.Description, config.HeaderContent);
            }
            else if (!string.IsNullOrEmpty(config.Title) || !string.IsNullOrEmpty(config.Description))
            {
                CardHeader(() =>
                {
                    if (!string.IsNullOrEmpty(config.Title))
                        CardTitle(config.Title);
                    if (!string.IsNullOrEmpty(config.Description))
                        CardDescription(config.Description);
                });
            }
        }

        private void DrawCardImage(CardConfig config)
        {
            if (config.Image != null)
                CardImage(config.Image);
        }

        private void DrawCardContent(CardConfig config)
        {
            if (!string.IsNullOrEmpty(config.Content))
            {
                CardContent(() =>
                {
                    var styleManager = guiHelper.GetStyleManager();
                    UnityHelpers.Label(config.Content, styleManager.GetLabelStyle(ControlVariant.Default));
                });
            }
        }

        private void DrawCardFooter(CardConfig config)
        {
            if (config.FooterContent != null)
                CardFooter(config.FooterContent);
        }
        #endregion
    }
}
