using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class TabsAndNavigationExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 650);
        private Vector2 scroll;
        private int selectedTab;
        private int selectedSideTab;
        private int selectedNavigation;
        private bool autoSave = true;
        private bool analytics = true;
        private bool experimental;

        private readonly string[] tabs = { "Account", "Settings", "Security", "Notifications" };
        private readonly string[] sideTabs = { "General", "Appearance", "Advanced" };

        private void Start() => gui = new GUIHelper();

        private void OnGUI()
        {
            windowRect = GUI.Window(4, windowRect, DrawWindow, "Tabs & Navigation");
            gui.DrawOverlays();
        }

        private void DrawWindow(int windowID)
        {
            gui.UpdateGUI(true);
            if (!gui.BeginGUI())
                return;

            scroll = gui.ScrollView(
                scroll,
                () =>
                {
                    gui.BeginColumn();
                    gui.Heading("Standard tabs");
                    selectedTab = gui.Tabs()
                        .Items(tabs)
                        .SelectedIndex(selectedTab)
                        .Indicator(IndicatorStyle.Underline)
                        .Content(() => gui.Card().Title(tabs[Mathf.Clamp(selectedTab, 0, tabs.Length - 1)]).Description("Content is owned by the Tabs builder.").Content("This is a current API example for a tab panel.").Render());

                    gui.Space(18f);
                    gui.Heading("Vertical tabs");
                    gui.BeginRow();
                    selectedSideTab = gui.Tabs().Items(sideTabs).SelectedIndex(selectedSideTab).Side(TabSide.Left).TabWidth(130f).Content(() => DrawSideTab(selectedSideTab));
                    gui.EndRow();

                    gui.Space(18f);
                    gui.Heading("Navigation");
                    selectedNavigation = gui.Navigation()
                        .Logo("APP")
                        .Items(new NavigationItem("dashboard", "Dashboard"), new NavigationItem("analytics", "Analytics"), new NavigationItem("projects", "Projects"), new NavigationItem("team", "Team"), new NavigationItem("settings", "Settings"))
                        .SelectedIndex(selectedNavigation)
                        .Width(190f);

                    gui.EndColumn();
                },
                GUILayout.Width(windowRect.width - 20f),
                GUILayout.Height(windowRect.height - 60f)
            );

            gui.EndGUI();
            GUI.DragWindow();
        }

        private void DrawSideTab(int index)
        {
            gui.BeginColumn();
            if (index == 0)
            {
                autoSave = gui.Checkbox("Enable auto-save", autoSave);
                analytics = gui.Checkbox("Enable analytics", analytics);
            }
            else if (index == 1)
            {
                gui.ThemeChanger().Width(220f).ShowPreview().Render();
                gui.FontChanger().Width(220f).ShowPreview(false).Render();
            }
            else
            {
                experimental = gui.Switch("Experimental features", experimental);
                gui.Caption("Advanced settings are intentionally kept behind an explicit switch.");
            }
            gui.EndColumn();
        }

        private void OnDestroy() => gui?.Cleanup();
    }
}
