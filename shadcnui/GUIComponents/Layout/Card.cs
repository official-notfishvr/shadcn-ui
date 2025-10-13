using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Layout
{
    public class Card
    {
        private readonly GUIHelper _guiHelper;
        private readonly Layout _layoutComponents;

        public Card(GUIHelper helper)
        {
            _guiHelper = helper;
            _layoutComponents = new Layout(helper);
        }

        public void BeginCard(float width = -1, float height = -1)
        {
            var styleManager = _guiHelper.GetStyleManager();
            var options = new List<GUILayoutOption>();
            if (width > 0)
            {
                options.Add(GUILayout.Width(width * _guiHelper.uiScale));
            }
            if (height > 0)
            {
                options.Add(GUILayout.Height(height * _guiHelper.uiScale));
            }

            _layoutComponents.BeginVerticalGroup(styleManager.cardStyle, options.ToArray());
        }

        public void EndCard()
        {
            _layoutComponents.EndVerticalGroup();
        }

        public void CardHeader(Action content)
        {
            var styleManager = _guiHelper.GetStyleManager();
            _layoutComponents.BeginVerticalGroup(styleManager.cardHeaderStyle);
            content();
            _layoutComponents.EndVerticalGroup();
        }

        public void CardTitle(string title)
        {
            var styleManager = _guiHelper.GetStyleManager();

            UnityHelpers.Label(title, styleManager.cardTitleStyle);
        }

        public void CardDescription(string description)
        {
            var styleManager = _guiHelper.GetStyleManager();

            UnityHelpers.Label(description, styleManager.cardDescriptionStyle);
        }

        public void CardContent(Action content)
        {
            var styleManager = _guiHelper.GetStyleManager();
            _layoutComponents.BeginVerticalGroup(styleManager.cardContentStyle);
            content();
            _layoutComponents.EndVerticalGroup();
        }

        public void CardFooter(Action content)
        {
            var styleManager = _guiHelper.GetStyleManager();
            _layoutComponents.BeginHorizontalGroup(styleManager.cardFooterStyle);
            content();
            _layoutComponents.EndHorizontalGroup();
        }

        public void DrawCard(string title, string description, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            BeginCard(width, height);

            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(description))
            {
                CardHeader(() =>
                {
                    if (!string.IsNullOrEmpty(title))
                        CardTitle(title);
                    if (!string.IsNullOrEmpty(description))
                        CardDescription(description);
                });
            }

            if (!string.IsNullOrEmpty(content))
            {
                CardContent(() =>
                {
                    var styleManager = _guiHelper.GetStyleManager();
                    UnityHelpers.Label(content, styleManager.labelDefaultStyle);
                });
            }

            if (footerContent != null)
            {
                CardFooter(footerContent);
            }

            EndCard();
        }

        public void CardImage(Texture2D image, float height = 150)
        {
            var styleManager = _guiHelper.GetStyleManager();
#if IL2CPP_MELONLOADER
            var rect = GUILayoutUtility.GetRect(0, height, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.ExpandWidth(true) }));
#else
            var rect = GUILayoutUtility.GetRect(0, height, GUILayout.ExpandWidth(true));
#endif
            GUI.DrawTexture(rect, image, ScaleMode.ScaleAndCrop);
        }

        public void DrawCardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            BeginCard(width, height);

            CardImage(image);

            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(description))
            {
                CardHeader(() =>
                {
                    if (!string.IsNullOrEmpty(title))
                        CardTitle(title);
                    if (!string.IsNullOrEmpty(description))
                        CardDescription(description);
                });
            }

            if (!string.IsNullOrEmpty(content))
            {
                CardContent(() =>
                {
                    var styleManager = _guiHelper.GetStyleManager();

                    UnityHelpers.Label(content, styleManager.labelDefaultStyle);
                });
            }

            if (footerContent != null)
            {
                CardFooter(footerContent);
            }

            EndCard();
        }

        public void CardWithAvatar(Texture2D avatar, string title, string subtitle)
        {
            var styleManager = _guiHelper.GetStyleManager();
            _layoutComponents.BeginHorizontalGroup(styleManager.cardHeaderStyle);
            _guiHelper.Avatar(avatar, "");
            _layoutComponents.BeginVerticalGroup();
            CardTitle(title);
            CardDescription(subtitle);
            _layoutComponents.EndVerticalGroup();
            _layoutComponents.EndHorizontalGroup();
        }

        public void DrawCardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            BeginCard(width, height);

            CardWithAvatar(avatar, title, subtitle);

            if (!string.IsNullOrEmpty(content))
            {
                CardContent(() =>
                {
                    var styleManager = _guiHelper.GetStyleManager();

                    UnityHelpers.Label(content, styleManager.labelDefaultStyle);
                });
            }

            if (footerContent != null)
            {
                CardFooter(footerContent);
            }

            EndCard();
        }

        public void CardHeader(string title, string description, Action Actions)
        {
            var styleManager = _guiHelper.GetStyleManager();
            _layoutComponents.BeginHorizontalGroup(styleManager.cardHeaderStyle);
            _layoutComponents.BeginVerticalGroup();
            CardTitle(title);
            CardDescription(description);
            _layoutComponents.EndVerticalGroup();
            GUILayout.FlexibleSpace();
            Actions();
            _layoutComponents.EndHorizontalGroup();
        }

        public void DrawCardWithHeader(string title, string description, Action header, string content, Action footerContent = null, float width = -1, float height = -1)
        {
            BeginCard(width, height);

            CardHeader(title, description, header);

            if (!string.IsNullOrEmpty(content))
            {
                CardContent(() =>
                {
                    var styleManager = _guiHelper.GetStyleManager();

                    UnityHelpers.Label(content, styleManager.labelDefaultStyle);
                });
            }

            if (footerContent != null)
            {
                CardFooter(footerContent);
            }

            EndCard();
        }

        public void DrawSimpleCard(string content, float width = -1, float height = -1)
        {
            BeginCard(width, height);
            CardContent(() =>
            {
                var styleManager = _guiHelper.GetStyleManager();

                UnityHelpers.Label(content, styleManager.labelDefaultStyle);
            });
            EndCard();
        }
    }
}
