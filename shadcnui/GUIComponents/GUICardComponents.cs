using shadcnui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUICardComponents
    {
        private GUIHelper guiHelper;

        public GUICardComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void BeginCard(float width = -1, float height = -1)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (width > 0 && height > 0)
            {
#if IL2CPP
                GUILayout.BeginVertical(styleManager.cardStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(width * guiHelper.uiScale), 
                    GUILayout.Height(height * guiHelper.uiScale) 
                });
#else
                GUILayout.BeginVertical(styleManager.cardStyle,
                    GUILayout.Width(width * guiHelper.uiScale),
                    GUILayout.Height(height * guiHelper.uiScale));
#endif
            }
            else if (width > 0)
            {
#if IL2CPP
                GUILayout.BeginVertical(styleManager.cardStyle, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { 
                    GUILayout.Width(width * guiHelper.uiScale) 
                });
#else
                GUILayout.BeginVertical(styleManager.cardStyle, GUILayout.Width(width * guiHelper.uiScale));
#endif
            }
            else
            {
#if IL2CPP
                GUILayout.BeginVertical(styleManager.cardStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.BeginVertical(styleManager.cardStyle);
#endif
            }
        }

        public void EndCard()
        {
            GUILayout.EndVertical();
        }

        public void BeginCardHeader()
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.BeginVertical(styleManager.cardHeaderStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.BeginVertical(styleManager.cardHeaderStyle);
#endif
        }

        public void EndCardHeader()
        {
            GUILayout.EndVertical();
        }

        public void DrawCardTitle(string title)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUILayout.Label(title, styleManager.cardTitleStyle);
        }

        public void DrawCardDescription(string description)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUILayout.Label(description, styleManager.cardDescriptionStyle);
        }

        public void BeginCardContent()
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.BeginVertical(styleManager.cardContentStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.BeginVertical(styleManager.cardContentStyle);
#endif
        }

        public void EndCardContent()
        {
            GUILayout.EndVertical();
        }

        public void BeginCardFooter()
        {
            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            GUILayout.BeginHorizontal(styleManager.cardFooterStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.BeginHorizontal(styleManager.cardFooterStyle);
#endif
        }

        public void EndCardFooter()
        {
            GUILayout.EndHorizontal();
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
                GUILayout.Label(content, guiHelper.labelStyle2);
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
            GUILayout.Label(content, guiHelper.labelStyle2);
            EndCardContent();
            EndCard();
        }
    }
}
