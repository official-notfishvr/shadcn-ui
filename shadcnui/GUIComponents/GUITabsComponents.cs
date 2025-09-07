using shadcnui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUITabsComponents
    {
        private GUIHelper guiHelper;

        public GUITabsComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }
        public int Tabs(string[] tabNames, int selectedIndex, Action<int> onTabChange = null,
            params GUILayoutOption[] options)
        {
            if (tabNames == null || tabNames.Length == 0)
                return selectedIndex;

            var styleManager = guiHelper.GetStyleManager();

           
            selectedIndex = Mathf.Clamp(selectedIndex, 0, tabNames.Length - 1);

           
            GUILayout.BeginHorizontal(styleManager.GetTabsListStyle());

            int newSelectedIndex = selectedIndex;

            for (int i = 0; i < tabNames.Length; i++)
            {
                bool isActive = i == selectedIndex;
                GUIStyle triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

               
                var tabOptions = new List<GUILayoutOption>();
                tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));

                if (options != null && options.Length > 0)
                    tabOptions.AddRange(options);

#if IL2CPP
                bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)tabOptions.ToArray());
#else
                bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle,
                    tabOptions.ToArray());
#endif

                if (clicked && i != selectedIndex)
                {
                    newSelectedIndex = i;
                    onTabChange?.Invoke(i);
                }

               
                if (i < tabNames.Length - 1)
                {
                    GUILayout.Space(2 * guiHelper.uiScale);
                }
            }

            GUILayout.EndHorizontal();

            return newSelectedIndex;
        }
        public void BeginTabContent()
        {
            var styleManager = guiHelper.GetStyleManager();
            GUILayout.BeginVertical(styleManager.GetTabsContentStyle());
        }
        public void EndTabContent()
        {
            GUILayout.EndVertical();
        }
        public int TabsWithContent(TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null)
        {
            if (tabConfigs == null || tabConfigs.Length == 0)
                return selectedIndex;

           
            string[] tabNames = new string[tabConfigs.Length];
            for (int i = 0; i < tabConfigs.Length; i++)
            {
                tabNames[i] = tabConfigs[i].Name;
            }

           
            int newSelectedIndex = Tabs(tabNames, selectedIndex, onTabChange);

           
            if (newSelectedIndex >= 0 && newSelectedIndex < tabConfigs.Length)
            {
                BeginTabContent();
                tabConfigs[newSelectedIndex].Content?.Invoke();
                EndTabContent();
            }

            return newSelectedIndex;
        }
        public int VerticalTabs(string[] tabNames, int selectedIndex, Action<int> onTabChange = null,
            float tabWidth = 120f, params GUILayoutOption[] options)
        {
            if (tabNames == null || tabNames.Length == 0)
                return selectedIndex;

            var styleManager = guiHelper.GetStyleManager();

           
            selectedIndex = Mathf.Clamp(selectedIndex, 0, tabNames.Length - 1);

            GUILayout.BeginVertical();

            int newSelectedIndex = selectedIndex;

            for (int i = 0; i < tabNames.Length; i++)
            {
                bool isActive = i == selectedIndex;
                GUIStyle triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

               
                var tabOptions = new List<GUILayoutOption>();
                tabOptions.Add(GUILayout.Width(tabWidth * guiHelper.uiScale));
                tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(36 * guiHelper.uiScale)));

                if (options != null && options.Length > 0)
                    tabOptions.AddRange(options);

#if IL2CPP
                bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)tabOptions.ToArray());
#else
                bool clicked = GUILayout.Button(tabNames[i] ?? $"Tab {i + 1}", triggerStyle,
                    tabOptions.ToArray());
#endif

                if (clicked && i != selectedIndex)
                {
                    newSelectedIndex = i;
                    onTabChange?.Invoke(i);
                }

               
                if (i < tabNames.Length - 1)
                {
                    GUILayout.Space(2 * guiHelper.uiScale);
                }
            }

            GUILayout.EndVertical();

            return newSelectedIndex;
        }
        public struct TabConfig
        {
            public string Name;
            public Action Content;
            public bool Disabled;

            public TabConfig(string name, Action content, bool disabled = false)
            {
                Name = name;
                Content = content;
                Disabled = disabled;
            }
        }
    }
}
