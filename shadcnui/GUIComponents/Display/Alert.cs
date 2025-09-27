using System;
using System.Collections.Generic;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Alert
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Alert(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public void DrawAlert(string title, string description, AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, Texture2D icon = null, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle alertStyle = styleManager.GetAlertStyle(variant, type);

            layoutComponents.BeginVerticalGroup(alertStyle, options);
            layoutComponents.BeginHorizontalGroup();

            if (icon != null)
            {
#if IL2CPP
                GUILayout.Label(icon, (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(24 * guiHelper.uiScale), GUILayout.Height(24 * guiHelper.uiScale) });
#else
                GUILayout.Label(icon, GUILayout.Width(24 * guiHelper.uiScale), GUILayout.Height(24 * guiHelper.uiScale));
#endif
                layoutComponents.AddSpace(8);
            }

            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
            {
                GUIStyle titleStyle = styleManager.GetAlertTitleStyle(type);
#if IL2CPP
                GUILayout.Label(new GUIContent(title), titleStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(title, titleStyle);
#endif
            }

            if (!string.IsNullOrEmpty(description))
            {
                GUIStyle descStyle = styleManager.GetAlertDescriptionStyle(type);
#if IL2CPP
                GUILayout.Label(new GUIContent(description), descStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(description, descStyle);
#endif
            }
            layoutComponents.EndVerticalGroup();
            layoutComponents.EndHorizontalGroup();
            layoutComponents.EndVerticalGroup();
        }

        public void CustomAlert(string title, string description, Color backgroundColor, Color textColor, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.normal.textColor = textColor;
            customStyle.border = new RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius);
            customStyle.padding = new RectOffset(16, 16, 12, 12);

            layoutComponents.BeginVerticalGroup(customStyle, options);

            if (!string.IsNullOrEmpty(title))
            {
                GUIStyle titleStyle = styleManager.GetAlertTitleStyle(AlertType.Info);
                titleStyle.normal.textColor = textColor;
#if IL2CPP
                GUILayout.Label(new GUIContent(title), titleStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(title, titleStyle);
#endif
            }

            if (!string.IsNullOrEmpty(description))
            {
                GUIStyle descStyle = styleManager.GetAlertDescriptionStyle(AlertType.Info);
                descStyle.normal.textColor = textColor;
#if IL2CPP
                GUILayout.Label(new GUIContent(description), descStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(description, descStyle);
#endif
            }

            layoutComponents.EndVerticalGroup();
        }

        public void AlertWithProgress(string title, string description, float progress, AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();

            DrawAlert(title, description, variant, type, null, options);

            layoutComponents.AddSpace(8);
            Rect progressRect = GUILayoutUtility.GetRect(200 * guiHelper.uiScale, 6 * guiHelper.uiScale);

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle bgStyle = new GUIStyle(GUI.skin.box);
                bgStyle.normal.background = styleManager.CreateSolidTexture(Color.gray);
                GUI.Box(progressRect, GUIContent.none, bgStyle);

                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * Mathf.Clamp01(progress), progressRect.height);
                GUIStyle fillStyle = new GUIStyle(GUI.skin.box);
                fillStyle.normal.background = styleManager.CreateSolidTexture(GetProgressColor(type));
                GUI.Box(fillRect, GUIContent.none, fillStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        public void AnimatedAlert(string title, string description, AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            float time = Time.time * 2f;
            float alpha = Mathf.Clamp01(time);

            Color originalColor = GUI.color;
            DrawAlert(title, description, variant, type, null, options);

            GUI.color = originalColor;
        }

        public void AlertWithCountdown(string title, string description, float countdownTime, Action onTimeout, AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();

            DrawAlert(title, description, variant, type, null, options);

            layoutComponents.AddSpace(4);
            int remainingSeconds = Mathf.CeilToInt(countdownTime);
            string countdownText = $"Auto-dismiss in {remainingSeconds}s";

            var styleManager = guiHelper.GetStyleManager();
            GUIStyle countdownStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
#if IL2CPP
            GUILayout.Label(new GUIContent(countdownText), countdownStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
            GUILayout.Label(countdownText, countdownStyle);
#endif

            if (countdownTime <= 0 && onTimeout != null)
            {
                onTimeout.Invoke();
            }

            layoutComponents.EndVerticalGroup();
        }

        public bool ExpandableAlert(string title, string description, string expandedContent, ref bool isExpanded, AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            layoutComponents.BeginVerticalGroup();

            DrawAlert(title, description, variant, type, null, options);

            layoutComponents.AddSpace(4);
            string buttonText = isExpanded ? "Show Less" : "Show More";
            bool buttonClicked = guiHelper.Button(buttonText, ButtonVariant.Outline, ButtonSize.Small);

            if (buttonClicked)
            {
                isExpanded = !isExpanded;
            }

            if (isExpanded && !string.IsNullOrEmpty(expandedContent))
            {
                layoutComponents.AddSpace(4);
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle expandedStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
#if IL2CPP
                GUILayout.Label(new GUIContent(expandedContent), expandedStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(expandedContent, expandedStyle);
#endif
            }

            layoutComponents.EndVerticalGroup();
            return buttonClicked;
        }

        public void AlertWithStatus(string title, string description, bool isActive, AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            layoutComponents.BeginHorizontalGroup();

            Color statusColor = isActive ? Color.green : Color.gray;
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle statusStyle = new GUIStyle(GUI.skin.box);
                statusStyle.normal.background = styleManager.CreateSolidTexture(statusColor);
                statusStyle.fixedWidth = 8 * guiHelper.uiScale;
                statusStyle.fixedHeight = 8 * guiHelper.uiScale;
                statusStyle.border = new RectOffset(0, 0, 0, 0);
                statusStyle.padding = new RectOffset(0, 0, 0, 0);
                statusStyle.margin = new RectOffset(0, 0, 0, 0);
#if IL2CPP
                GUILayout.Label(GUIContent.none, statusStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(GUIContent.none, statusStyle);
#endif
                layoutComponents.AddSpace(8);
            }

            layoutComponents.BeginVerticalGroup();
            DrawAlert(title, description, variant, type, null, options);

            layoutComponents.EndHorizontalGroup();
        }

        private Color GetProgressColor(AlertType type)
        {
            switch (type)
            {
                case AlertType.Success:
                    return Color.green;
                case AlertType.Warning:
                    return Color.yellow;
                case AlertType.Error:
                    return Color.red;
                default:
                    return Color.blue;
            }
        }
    }
}
