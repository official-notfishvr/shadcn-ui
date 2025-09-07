using shadcnui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIAlertComponents
    {
        private GUIHelper guiHelper;

        public GUIAlertComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void Alert(string title, string description, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(title ?? "Alert", GUI.skin.label);
                if (!string.IsNullOrEmpty(description))
                    GUILayout.Label(description, GUI.skin.label);
                GUILayout.EndVertical();
                return;
            }

            GUIStyle alertStyle = styleManager.GetAlertStyle(variant, type);

            GUILayout.BeginVertical(alertStyle, options);
            
            if (!string.IsNullOrEmpty(title))
            {
                GUIStyle titleStyle = styleManager.GetAlertTitleStyle(type);
#if IL2CPP
                GUILayout.Label(title, titleStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(title, titleStyle);
#endif
            }
            
            if (!string.IsNullOrEmpty(description))
            {
                GUIStyle descStyle = styleManager.GetAlertDescriptionStyle(type);
#if IL2CPP
                GUILayout.Label(description, descStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(description, descStyle);
#endif
            }
            
            GUILayout.EndVertical();
        }

        public void Alert(Rect rect, string title, string description, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUI.Box(rect, title ?? "Alert", GUI.skin.box);
                return;
            }

            GUIStyle alertStyle = styleManager.GetAlertStyle(variant, type);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            GUI.Box(scaledRect, "", alertStyle);
            
            Rect contentRect = new Rect(scaledRect.x + 16, scaledRect.y + 8, scaledRect.width - 32, scaledRect.height - 16);
            GUILayout.BeginArea(contentRect);
            
            if (!string.IsNullOrEmpty(title))
            {
                GUIStyle titleStyle = styleManager.GetAlertTitleStyle(type);
                GUILayout.Label(title, titleStyle);
            }
            
            if (!string.IsNullOrEmpty(description))
            {
                GUIStyle descStyle = styleManager.GetAlertDescriptionStyle(type);
                GUILayout.Label(description, descStyle);
            }
            
            GUILayout.EndArea();
        }

        public void AlertWithIcon(string title, string description, Texture2D icon, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            
            if (icon != null)
            {
                GUILayout.Label(icon, GUILayout.Width(24 * guiHelper.uiScale), GUILayout.Height(24 * guiHelper.uiScale));
                GUILayout.Space(8 * guiHelper.uiScale);
            }
            
            GUILayout.BeginVertical();
            Alert(title, description, variant, type, options);
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }

        public bool DismissibleAlert(string title, string description, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info, Action onDismiss = null, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            Alert(title, description, variant, type, options);
            
            GUILayout.Space(4 * guiHelper.uiScale);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool closeClicked = GUILayout.Button("×", GUILayout.Width(20 * guiHelper.uiScale), GUILayout.Height(20 * guiHelper.uiScale));
            GUILayout.EndHorizontal();
            
            if (closeClicked && onDismiss != null)
            {
                onDismiss.Invoke();
            }
            
            GUILayout.EndVertical();
            
            return closeClicked;
        }

        public void AlertWithActions(string title, string description, string[] buttonTexts, Action<int> onButtonClick,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            Alert(title, description, variant, type, options);
            
            if (buttonTexts != null && buttonTexts.Length > 0)
            {
                GUILayout.Space(8 * guiHelper.uiScale);
                GUILayout.BeginHorizontal();
                
                for (int i = 0; i < buttonTexts.Length; i++)
                {
                    int index = i;
                    if (GUILayout.Button(buttonTexts[i]))
                    {
                        onButtonClick?.Invoke(index);
                    }
                    
                    if (i < buttonTexts.Length - 1)
                    {
                        GUILayout.Space(4 * guiHelper.uiScale);
                    }
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        public void CustomAlert(string title, string description, Color backgroundColor, Color textColor, 
            Color borderColor, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(title ?? "Alert", GUI.skin.label);
                if (!string.IsNullOrEmpty(description))
                    GUILayout.Label(description, GUI.skin.label);
                GUILayout.EndVertical();
                return;
            }

            GUIStyle customStyle = new GUIStyle(GUI.skin.box);
            customStyle.normal.background = styleManager.CreateSolidTexture(backgroundColor);
            customStyle.normal.textColor = textColor;
            customStyle.border = new RectOffset(2, 2, 2, 2);
            customStyle.padding = new RectOffset(16, 16, 12, 12);

            GUILayout.BeginVertical(customStyle, options);
            
            if (!string.IsNullOrEmpty(title))
            {
                GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
                titleStyle.fontSize = guiHelper.fontSize + 2;
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.normal.textColor = textColor;
                GUILayout.Label(title, titleStyle);
            }
            
            if (!string.IsNullOrEmpty(description))
            {
                GUIStyle descStyle = new GUIStyle(GUI.skin.label);
                descStyle.fontSize = guiHelper.fontSize;
                descStyle.normal.textColor = textColor;
                GUILayout.Label(description, descStyle);
            }
            
            GUILayout.EndVertical();
        }

        public void AlertWithProgress(string title, string description, float progress, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            Alert(title, description, variant, type, options);
            
            GUILayout.Space(8 * guiHelper.uiScale);
            Rect progressRect = GUILayoutUtility.GetRect(200 * guiHelper.uiScale, 6 * guiHelper.uiScale);
            
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager != null)
            {
                GUIStyle bgStyle = new GUIStyle(GUI.skin.box);
                bgStyle.normal.background = styleManager.CreateSolidTexture(Color.gray);
                GUI.Box(progressRect, "", bgStyle);
                
                // Progress fill
                Rect fillRect = new Rect(progressRect.x, progressRect.y, progressRect.width * Mathf.Clamp01(progress), progressRect.height);
                GUIStyle fillStyle = new GUIStyle(GUI.skin.box);
                fillStyle.normal.background = styleManager.CreateSolidTexture(GetProgressColor(type));
                GUI.Box(fillRect, "", fillStyle);
            }
            
            GUILayout.EndVertical();
        }

        public void AnimatedAlert(string title, string description, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            float time = Time.time * 2f;
            float alpha = Mathf.Clamp01(time);
            
            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
            Alert(title, description, variant, type, options);
            
            GUI.color = originalColor;
        }


        public void AlertWithCountdown(string title, string description, float countdownTime, Action onTimeout,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            Alert(title, description, variant, type, options);
            
            GUILayout.Space(4 * guiHelper.uiScale);
            int remainingSeconds = Mathf.CeilToInt(countdownTime);
            string countdownText = $"Auto-dismiss in {remainingSeconds}s";
            
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle countdownStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
            GUILayout.Label(countdownText, countdownStyle);
            
            if (countdownTime <= 0 && onTimeout != null)
            {
                onTimeout.Invoke();
            }
            
            GUILayout.EndVertical();
        }


        public bool ExpandableAlert(string title, string description, string expandedContent, ref bool isExpanded,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical();
            
            Alert(title, description, variant, type, options);
            
            GUILayout.Space(4 * guiHelper.uiScale);
            string buttonText = isExpanded ? "Show Less" : "Show More";
            bool buttonClicked = GUILayout.Button(buttonText, GUILayout.Width(80 * guiHelper.uiScale));
            
            if (buttonClicked)
            {
                isExpanded = !isExpanded;
            }
            
            if (isExpanded && !string.IsNullOrEmpty(expandedContent))
            {
                GUILayout.Space(4 * guiHelper.uiScale);
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle expandedStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
                GUILayout.Label(expandedContent, expandedStyle);
            }
            
            GUILayout.EndVertical();
            
            return buttonClicked;
        }

        public void AlertWithStatus(string title, string description, bool isActive, AlertVariant variant = AlertVariant.Default, 
            AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            
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
                
                GUILayout.Label("", statusStyle);
                GUILayout.Space(8 * guiHelper.uiScale);
            }
            
            GUILayout.BeginVertical();
            Alert(title, description, variant, type, options);
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }

        public void AlertWithCustomIcon(string title, string description, Texture2D icon, Color iconColor,
            AlertVariant variant = AlertVariant.Default, AlertType type = AlertType.Info, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            
            if (icon != null)
            {
                Color originalColor = GUI.color;
                GUI.color = iconColor;
                GUILayout.Label(icon, GUILayout.Width(24 * guiHelper.uiScale), GUILayout.Height(24 * guiHelper.uiScale));
                GUI.color = originalColor;
                GUILayout.Space(8 * guiHelper.uiScale);
            }
            
            GUILayout.BeginVertical();
            Alert(title, description, variant, type, options);
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
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
