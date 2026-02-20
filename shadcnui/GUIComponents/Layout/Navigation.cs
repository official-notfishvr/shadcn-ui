using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Navigation : BaseComponent
    {
        private const float INDICATOR_WIDTH = 3f;
        private const float ITEM_HEIGHT = 50f;

        public Navigation(GUIHelper helper)
            : base(helper) { }

        public int Draw(NavigationConfig config)
        {
            if (config.Items == null || config.Items.Length == 0)
                return config.SelectedIndex;

            var selectedIndex = Mathf.Clamp(config.SelectedIndex, 0, config.Items.Length - 1);
            var newSelectedIndex = selectedIndex;

            var navStyle = styleManager?.GetNavigationStyle() ?? GUI.skin.box;

            GUILayout.BeginVertical(navStyle, GUILayout.Width(config.Width * guiHelper.uiScale), GUILayout.ExpandHeight(true));

            DrawLogo(config);

            layoutComponents.AddSpace(DesignTokens.Spacing.MD * guiHelper.uiScale);

            for (int i = 0; i < config.Items.Length; i++)
            {
                var item = config.Items[i];
                var isSelected = i == selectedIndex;
                var clicked = DrawNavItem(item, isSelected, config);

                if (clicked && !item.IsDisabled)
                {
                    newSelectedIndex = i;
                    config.OnSelectionChanged?.Invoke(i);
                }

                if (i < config.Items.Length - 1)
                {
                    layoutComponents.AddSpace(DesignTokens.Spacing.XS * guiHelper.uiScale);
                }
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();

            return newSelectedIndex;
        }

        public int DrawSidebar(string[] labels, int selectedIndex, string[] icons = null, string logoText = "U", Action<int> onSelectionChanged = null, float width = 70f)
        {
            var items = new NavigationItem[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                items[i] = new NavigationItem($"nav_{i}", labels[i], icons != null && i < icons.Length ? icons[i] : null);
            }

            var config = new NavigationConfig
            {
                Items = items,
                SelectedIndex = selectedIndex,
                Width = width,
                LogoText = logoText,
                OnSelectionChanged = onSelectionChanged,
            };

            return Draw(config);
        }

        private void DrawLogo(NavigationConfig config)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.MD * guiHelper.uiScale);

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            var logoStyle = new UnityHelpers.GUIStyle(GUIStyle.none);
            logoStyle.fontSize = Mathf.RoundToInt(24 * guiHelper.uiScale);
            logoStyle.fontStyle = FontStyle.Bold;
            logoStyle.normal.textColor = Color.white;
            logoStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.Label(config.LogoText, logoStyle, GUILayout.Width(40 * guiHelper.uiScale), GUILayout.Height(40 * guiHelper.uiScale));

            GUILayout.FlexibleSpace();
            layoutComponents.EndHorizontalGroup();
        }

        private bool DrawNavItem(NavigationItem item, bool isSelected, NavigationConfig config)
        {
            var rect = GUILayoutUtility.GetRect(config.Width * guiHelper.uiScale, ITEM_HEIGHT * guiHelper.uiScale);

            if (config.ShowIndicator && isSelected)
            {
                DrawIndicator(rect, config);
            }

            var textColor = isSelected ? Color.white : new Color(0.4f, 0.4f, 0.4f);
            if (item.IsDisabled)
                textColor = new Color(0.3f, 0.3f, 0.3f);

            DrawIcon(rect, item.Icon, textColor);
            DrawLabel(rect, item.Label, textColor, isSelected);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                return true;
            }

            return false;
        }

        private void DrawIndicator(Rect rect, NavigationConfig config)
        {
            var indicatorRect = new Rect(rect.x, rect.y + (rect.height - 30 * guiHelper.uiScale) / 2, INDICATOR_WIDTH * guiHelper.uiScale, 30 * guiHelper.uiScale);

            var color = config.IndicatorColor;
            if (config.IndicatorStyle == IndicatorStyle.Background)
            {
                indicatorRect = new Rect(rect.x + 5 * guiHelper.uiScale, rect.y + 5 * guiHelper.uiScale, rect.width - 10 * guiHelper.uiScale, rect.height - 10 * guiHelper.uiScale);
            }

            GUI.color = color;
            GUI.DrawTexture(indicatorRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }

        private void DrawIcon(Rect rect, string icon, Color color)
        {
            if (string.IsNullOrEmpty(icon))
                return;

            var iconStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            iconStyle.fontSize = Mathf.RoundToInt(20 * guiHelper.uiScale);
            iconStyle.alignment = TextAnchor.MiddleCenter;
            iconStyle.normal.textColor = color;

            var iconRect = new Rect(rect.x, rect.y + 5 * guiHelper.uiScale, rect.width, 25 * guiHelper.uiScale);
            GUI.Label(iconRect, icon, iconStyle);
        }

        private void DrawLabel(Rect rect, string label, Color color, bool isSelected)
        {
            var labelStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            labelStyle.fontSize = Mathf.RoundToInt(9 * guiHelper.uiScale);
            labelStyle.alignment = TextAnchor.UpperCenter;
            labelStyle.normal.textColor = color;
            labelStyle.fontStyle = FontStyle.Normal;

            var labelRect = new Rect(rect.x, rect.y + 30 * guiHelper.uiScale, rect.width, 20 * guiHelper.uiScale);
            GUI.Label(labelRect, label.ToUpper(), labelStyle);
        }
    }
}
