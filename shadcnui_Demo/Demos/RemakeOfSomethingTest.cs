using System;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_Demo.Demos
{
    public class RemakeOfSomethingTest : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new Rect(50, 50, 900, 600);
        private bool showWindow = true;
        private Vector2 scrollPosition;

        private string searchQuery = "";
        private int currentSidebarTab = 0;
        private int currentTopTab = 1;

        private readonly string[] sidebarLabels = new[] { "MODS", "ROOM", "USER" };
        private readonly string[] sidebarIcons = new[] { "≡", "≡", "≡" };

        private readonly string[] topTabNames = new[] { "Visuals", "OP", "Master", "Global", "Mode", "Beta", "MenuSe" };

        private readonly string[] listItems = new[] { "Back", "Instant Freeze Server", "Deafen Server", "Deafen Gun", "Kick Master", "Kick Master Gun" };

        void Start()
        {
            guiHelper = new GUIHelper();

            var theme = new Theme
            {
                Name = "RemakeTheme",
                Base = Theme.Hex("#050505"),
                Secondary = Theme.Hex("#111111"),
                Elevated = Theme.Hex("#141414"),
                Text = Theme.Hex("#ffffff"),
                Muted = Theme.Hex("#888888"),
                Border = Theme.Hex("#2a2a2a"),
                Accent = Theme.Hex("#00ffff"),
                Destructive = Theme.Hex("#ef4444"),
                Success = Theme.Hex("#22c55e"),
                Warning = Theme.Hex("#f59e0b"),
                Info = Theme.Hex("#3b82f6"),
                Overlay = new Color(0, 0, 0, 0.6f),
                Shadow = new Color(0, 0, 0, 0.5f),

                ButtonPrimaryBg = Theme.Hex("#111111"),
                ButtonPrimaryFg = Theme.Hex("#ffffff"),

                TabsBg = Theme.Hex("#050505"),
                TabsTriggerFg = Theme.Hex("#888888"),
                TabsTriggerActiveBg = Theme.Hex("#00ffff"),
                TabsTriggerActiveFg = Theme.Hex("#000000"),

                BackgroundColor = Theme.Hex("#050505"),
            };

            ThemeManager.Instance.AddTheme(theme);
            ThemeManager.Instance.SetTheme("RemakeTheme");
        }

        void OnGUI()
        {
            GUI.skin.horizontalScrollbar = GUIStyle.none;
            GUI.skin.verticalScrollbar = GUIStyle.none;

            if (showWindow)
            {
                windowRect = GUI.Window(201, windowRect, (GUI.WindowFunction)DrawWindow, "");
            }

            guiHelper.DrawOverlay();
        }

        void DrawWindow(int windowID)
        {
            if (guiHelper == null)
                return;

            guiHelper.UpdateGUI(showWindow);
            if (!guiHelper.BeginGUI())
                return;

            guiHelper.BeginHorizontalGroup(GUILayout.ExpandHeight(true));

            DrawSidebar();

            DrawMainContent();

            guiHelper.EndHorizontalGroup();

            guiHelper.EndGUI();
            GUI.DragWindow();
        }

        void DrawSidebar()
        {
            GUILayout.BeginVertical(GUILayout.Width(80), GUILayout.ExpandHeight(true));

            currentSidebarTab = guiHelper.Sidebar(sidebarLabels, currentSidebarTab, sidebarIcons, "U", (index) => currentSidebarTab = index, 80f);

            GUILayout.EndVertical();
        }

        void DrawMainContent()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.AddSpace(20);

            GUILayout.Label(
                "Untitled",
                new UnityHelpers.GUIStyle(GUI.skin.label)
                {
                    fontSize = 24,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white },
                }
            );

            guiHelper.AddSpace(20);

            searchQuery = guiHelper.Input(searchQuery, "Search mods...", ControlVariant.Secondary, false, false, -1, (val) => searchQuery = val);

            guiHelper.AddSpace(20);

            DrawTopTabs();

            guiHelper.AddSpace(20);

            DrawContentList();

            guiHelper.EndVerticalGroup();
        }

        void DrawTopTabs()
        {
            var tabsConfig = new TabsConfig
            {
                TabNames = topTabNames,
                SelectedIndex = currentTopTab,
                OnTabChange = (index) => currentTopTab = index,
                IndicatorStyle = IndicatorStyle.Background,
                Position = TabPosition.Top,
            };

            guiHelper.Tabs(tabsConfig);
        }

        void DrawContentList()
        {
            scrollPosition = guiHelper.ScrollView(
                scrollPosition,
                () =>
                {
                    foreach (var item in listItems)
                    {
                        var btnStyle = new UnityHelpers.GUIStyle(GUI.skin.button);

                        bool clicked = guiHelper.Button(item, ControlVariant.Secondary, ControlSize.Large, null, false, 1f, GUILayout.Height(50), GUILayout.ExpandWidth(true));

                        if (clicked) { }

                        guiHelper.AddSpace(10);
                    }
                },
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );
        }
    }
}
