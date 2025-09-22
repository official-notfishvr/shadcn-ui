using System;
using System.Collections.Generic;
using System.Text;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Card
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Card(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public void BeginCard(float width = -1, float height = -1)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (width > 0 && height > 0)
            {
#if IL2CPP
                layoutComponents.BeginVerticalGroup(styleManager.cardStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(width * guiHelper.uiScale), GUILayout.Height(height * guiHelper.uiScale) });
#else
                layoutComponents.BeginVerticalGroup(styleManager.cardStyle, GUILayout.Width(width * guiHelper.uiScale), GUILayout.Height(height * guiHelper.uiScale));
#endif
            }
            else if (width > 0)
            {
#if IL2CPP
                layoutComponents.BeginVerticalGroup(styleManager.cardStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(width * guiHelper.uiScale) });
#else
                layoutComponents.BeginVerticalGroup(styleManager.cardStyle, GUILayout.Width(width * guiHelper.uiScale));
#endif
            }
            else
            {
#if IL2CPP
                layoutComponents.BeginVerticalGroup(styleManager.cardStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginVerticalGroup(styleManager.cardStyle);
#endif
            }
        }

        public void EndCard()
        {
            layoutComponents.EndVerticalGroup();
        }

        public void BeginCardHeader()
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            layoutComponents.BeginVerticalGroup(styleManager.cardHeaderStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            layoutComponents.BeginVerticalGroup(styleManager.cardHeaderStyle);
#endif
        }

        public void EndCardHeader()
        {
            layoutComponents.EndVerticalGroup();
        }

        public void DrawCardTitle(string title)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.Label(title, styleManager.cardTitleStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
            GUILayout.Label(title, styleManager.cardTitleStyle);
#endif
        }

        public void DrawCardDescription(string description)
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.Label(description, styleManager.cardDescriptionStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
            GUILayout.Label(description, styleManager.cardDescriptionStyle);
#endif
        }

        public void BeginCardContent()
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            layoutComponents.BeginVerticalGroup(styleManager.cardContentStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            layoutComponents.BeginVerticalGroup(styleManager.cardContentStyle);
#endif
        }

        public void EndCardContent()
        {
            layoutComponents.EndVerticalGroup();
        }

        public void BeginCardFooter()
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            layoutComponents.BeginHorizontalGroup(styleManager.cardFooterStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            layoutComponents.BeginHorizontalGroup(styleManager.cardFooterStyle);
#endif
        }

        public void EndCardFooter()
        {
            layoutComponents.EndHorizontalGroup();
        }

        public void DrawCard(string title, string description, string content, System.Action footerContent = null, float width = -1, float height = -1)
        {
            BeginCard(width, height);

            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(description))
            {
                BeginCardHeader();
                if (!string.IsNullOrEmpty(title))
                    DrawCardTitle(title);
                if (!string.IsNullOrEmpty(description))
                    DrawCardDescription(description);
                EndCardHeader();
            }

            if (!string.IsNullOrEmpty(content))
            {
                BeginCardContent();
                var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
                GUILayout.Label(content, styleManager.labelDefaultStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
                GUILayout.Label(content, styleManager.labelDefaultStyle);
#endif
                EndCardContent();
            }

            if (footerContent != null)
            {
                BeginCardFooter();
                footerContent();
                EndCardFooter();
            }

            EndCard();
        }

        public void DrawSimpleCard(string content, float width = -1, float height = -1)
        {
            BeginCard(width, height);
            BeginCardContent();
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.Label(content, styleManager.labelDefaultStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
            GUILayout.Label(content, styleManager.labelDefaultStyle);
#endif
            EndCardContent();
            EndCard();
        }
    }
}
