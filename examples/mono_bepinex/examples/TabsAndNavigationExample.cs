using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class TabsAndNavigationExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 700, 600);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        private int selectedTab = 0;
        private int selectedVerticalTab = 0;
        private int selectedSidebarItem = 0;
        private float cacheSize = 50f;
        private bool autoSave = true;
        private bool showWelcome = false;
        private bool enableAnalytics = true;
        private bool enableExperimental = false;

        private string[] tabNames = new[] { "Account", "Settings", "Security", "Notifications" };
        private string[] sidebarItems = new[] { "Dashboard", "Analytics", "Projects", "Team", "Settings" };

        void Start()
        {
            gui = new GUIHelper();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(4, windowRect, DrawTabsWindow, "Tabs & Navigation Example");
            }
            gui.DrawOverlay();
        }

        void DrawTabsWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI()) return;

            scroll = gui.ScrollView(scroll, () =>
            {
                gui.BeginVerticalGroup();

                gui.Label("Standard Tabs", ControlVariant.Default);
                selectedTab = gui.Tabs(tabNames, selectedTab, index =>
                {
                    selectedTab = index;
                    gui.ShowToast($"Switched to {tabNames[index]} tab");
                });

                gui.BeginTabContent();
                switch (selectedTab)
                {
                    case 0:
                        gui.Card("Account", "Manage your account", "Here you can update your profile information, change your avatar, and manage connected accounts.");
                        break;
                    case 1:
                        gui.Card("Settings", "Application settings", "Configure application preferences, display options, and default behaviors.");
                        break;
                    case 2:
                        gui.Card("Security", "Security settings", "Manage passwords, two-factor authentication, and security keys.");
                        break;
                    case 3:
                        gui.Card("Notifications", "Notification preferences", "Choose which notifications you want to receive and how.");
                        break;
                }
                gui.EndTabContent();

                gui.AddSpace(20);

                gui.Label("Vertical Tabs (Left Side)", ControlVariant.Default);
                gui.BeginHorizontalGroup();

                gui.BeginVerticalGroup(GUILayout.Width(120));
                selectedVerticalTab = gui.VerticalTabs(new[] { "General", "Appearance", "Advanced" }, selectedVerticalTab, null, 100, 1, TabSide.Left);
                gui.EndVerticalGroup();

                gui.BeginVerticalGroup();
                switch (selectedVerticalTab)
                {
                    case 0:
                        gui.Label("General Settings", ControlVariant.Default);
                        autoSave = gui.Checkbox("Enable auto-save", autoSave);
                        showWelcome = gui.Checkbox("Show welcome screen", showWelcome);
                        enableAnalytics = gui.Checkbox("Enable analytics", enableAnalytics);
                        break;
                    case 1:
                        gui.Label("Appearance Settings", ControlVariant.Default);
                        gui.ThemeChanger();
                        break;
                    case 2:
                        gui.Label("Advanced Settings", ControlVariant.Default);
                        cacheSize = gui.LabeledSlider("Cache size", cacheSize, 10, 100);
                        enableExperimental = gui.Checkbox("Enable experimental features", enableExperimental);
                        break;
                }
                gui.EndVerticalGroup();

                gui.EndHorizontalGroup();

                gui.AddSpace(20);

                gui.Label("Sidebar Navigation", ControlVariant.Default);
                selectedSidebarItem = gui.Sidebar(sidebarItems, selectedSidebarItem, null, "APP", index =>
                {
                    selectedSidebarItem = index;
                    gui.ShowSuccessToast($"Navigated to {sidebarItems[index]}");
                }, 100);

                gui.EndVerticalGroup();
            }, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
