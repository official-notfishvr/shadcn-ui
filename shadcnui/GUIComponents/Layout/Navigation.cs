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
        private const float ItemHeight = 50f;
        private const float IndicatorWidth = 3f;

        public Navigation(GUIHelper helper)
            : base(helper) { }

        public int Draw(NavigationConfig config)
        {
            if (config?.Items == null || config.Items.Length == 0)
                return config?.SelectedIndex ?? 0;

            var selectedIndex = Mathf.Clamp(config.SelectedIndex, 0, config.Items.Length - 1);
            var newSelectedIndex = selectedIndex;

            var navStyle = styleManager?.GetNavigationStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box;
            GUILayout.BeginVertical(navStyle, GUILayout.Width(config.Width * guiHelper.uiScale), GUILayout.ExpandHeight(true));

            DrawLogo(config);
            layoutComponents.AddSpace(DesignTokens.Spacing.MD);

            for (int i = 0; i < config.Items.Length; i++)
            {
                var item = config.Items[i];
                var clicked = DrawNavItem(config, item, i, selectedIndex);
                if (clicked && !item.IsDisabled)
                {
                    newSelectedIndex = i;
                    config.OnSelectionChanged?.Invoke(i);
                }
                if (i < config.Items.Length - 1)
                    layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            return newSelectedIndex;
        }

        public int DrawSidebar(string[] labels, int selectedIndex, string[] icons = null, string logoText = "U", Action<int> onSelectionChanged = null, float width = 70f)
        {
            if (labels == null || labels.Length == 0)
                return selectedIndex;

            var items = new NavigationItem[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                var icon = icons != null && i < icons.Length ? icons[i] : null;
                items[i] = new NavigationItem($"nav_{i}", labels[i], icon);
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
            if (string.IsNullOrEmpty(config.LogoText))
                return;

            layoutComponents.AddSpace(DesignTokens.Spacing.MD);
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            var logoStyle = new UnityHelpers.GUIStyle(GUIStyle.none)
            {
                fontSize = Mathf.RoundToInt(24f * guiHelper.uiScale),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
            };
            logoStyle.normal.textColor = ThemeManager.Instance.CurrentTheme.Text;

            UnityHelpers.Label(config.LogoText, logoStyle, GUILayout.Width(40f * guiHelper.uiScale), GUILayout.Height(40f * guiHelper.uiScale));
            GUILayout.FlexibleSpace();
            layoutComponents.EndHorizontalGroup();
        }

        private bool DrawNavItem(NavigationConfig config, NavigationItem item, int index, int selectedIndex)
        {
            var rect = GUILayoutUtility.GetRect(config.Width * guiHelper.uiScale, ItemHeight * guiHelper.uiScale);
            var isSelected = index == selectedIndex;

            if (config.ShowIndicator && isSelected)
                DrawIndicator(rect, config);

            var textColor = item.IsDisabled ? ThemeManager.Instance.CurrentTheme.Muted : (isSelected ? ThemeManager.Instance.CurrentTheme.Text : ThemeManager.Instance.CurrentTheme.Muted);
            DrawIcon(rect, item.Icon, textColor);
            DrawLabel(rect, item.Label, textColor);

            if (item.IsDisabled)
                return false;

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                return true;
            }

            return false;
        }

        private void DrawIndicator(Rect rect, NavigationConfig config)
        {
            var color = config.IndicatorColor;
            var indicatorRect = config.IndicatorStyle switch
            {
                IndicatorStyle.Background => new Rect(rect.x + 6f * guiHelper.uiScale, rect.y + 6f * guiHelper.uiScale, rect.width - 12f * guiHelper.uiScale, rect.height - 12f * guiHelper.uiScale),
                _ => new Rect(rect.x, rect.y + (rect.height - 30f * guiHelper.uiScale) / 2f, IndicatorWidth * guiHelper.uiScale, 30f * guiHelper.uiScale),
            };

            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(indicatorRect, Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private void DrawIcon(Rect rect, string icon, Color color)
        {
            if (string.IsNullOrEmpty(icon))
                return;

            var iconStyle = new UnityHelpers.GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(20f * guiHelper.uiScale), alignment = TextAnchor.MiddleCenter };
            iconStyle.normal.textColor = color;

            var iconRect = new Rect(rect.x, rect.y + 6f * guiHelper.uiScale, rect.width, 24f * guiHelper.uiScale);
            GUI.Label(iconRect, icon, iconStyle);
        }

        private void DrawLabel(Rect rect, string label, Color color)
        {
            var labelStyle = new UnityHelpers.GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(9f * guiHelper.uiScale),
                alignment = TextAnchor.UpperCenter,
                fontStyle = FontStyle.Normal,
            };
            labelStyle.normal.textColor = color;

            var labelRect = new Rect(rect.x, rect.y + 30f * guiHelper.uiScale, rect.width, 20f * guiHelper.uiScale);
            GUI.Label(labelRect, label?.ToUpper() ?? string.Empty, labelStyle);
        }
    }
}
