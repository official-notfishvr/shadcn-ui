using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Navigation : BaseComponent
    {
        public Navigation(GUIHelper helper)
            : base(helper) { }

        internal int DrawSidebar(string[] labels, int selectedIndex, string[] icons = null, string logo = "U", Action<int> onChanged = null, float width = 70f)
        {
            var source = labels ?? Array.Empty<string>();
            var items = new NavigationItem[source.Length];
            for (int i = 0; i < source.Length; i++)
                items[i] = new NavigationItem(i.ToString(), source[i], icons != null && i < icons.Length ? icons[i] : null);
            return Render(
                new NavigationConfig
                {
                    Items = items,
                    SelectedIndex = selectedIndex,
                    Width = width,
                    LogoText = logo,
                    OnSelectionChanged = onChanged,
                }
            );
        }

        public int Render(NavigationConfig config)
        {
            if (config == null || config.Items == null)
                return 0;

            int selected = Mathf.Clamp(config.SelectedIndex, 0, Mathf.Max(0, config.Items.Length - 1));
            var containerStyle = styleManager.GetNavigationStyle(config.Variant, config.Size, config.Appearance);

            layoutComponents.BeginVerticalGroup(containerStyle, GUILayout.Width(config.Width * guiHelper.uiScale));
            if (!string.IsNullOrEmpty(config.LogoText))
            {
                GUILayout.Label(config.LogoText, styleManager.GetCardTitleStyle(config.Appearance), GUILayout.Height(DesignTokens.Height.Large * guiHelper.uiScale));
                layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            }

            for (int i = 0; i < config.Items.Length; i++)
            {
                var item = config.Items[i];
                bool active = i == selected;
                var style = styleManager.GetButtonStyle(active ? ControlVariant.Secondary : ControlVariant.Ghost, config.Size, config.Appearance);
                bool prev = GUI.enabled;
                GUI.enabled = !item.IsDisabled;
                if (GUILayout.Button(item.Label ?? item.Id ?? $"Item {i + 1}", style, GUILayout.ExpandWidth(true)))
                {
                    selected = i;
                    config.OnSelectionChanged?.Invoke(i);
                }
                GUI.enabled = prev;
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            layoutComponents.EndVerticalGroup();
            return selected;
        }
    }
}
