using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class AdvancedControlsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 680);
        private Vector2 scroll;
        private int selectedTab;
        private int selectedNavigation;
        private int selectedDropdown;
        private string text = "Resizable content is represented by the current TextArea builder.";
        private readonly bool[] closable = { true, true, true };
        private readonly string[] tabs = { "Document 1", "Document 2", "Document 3" };

        private void Start() => gui = new GUIHelper();

        private void OnGUI()
        {
            windowRect = GUI.Window(8, windowRect, DrawWindow, "Advanced Controls");
            gui.DrawOverlays();
        }

        private void DrawWindow(int windowID)
        {
            gui.UpdateGUI(true);
            if (!gui.BeginGUI())
                return;

            using (gui.Scope("advanced-controls"))
            {
                scroll = gui.ScrollView(
                    scroll,
                    () =>
                    {
                        gui.BeginColumn();
                        gui.Heading("Menu bar");
                        gui.MenuBar().Item("File", menu => menu.Item("New").Item("Open").Separator().Item("Exit")).Item("Edit", menu => menu.Item("Undo").Item("Redo")).Render();

                        gui.HorizontalSeparator();
                        gui.Heading("Dropdown menu");
                        selectedDropdown = gui.Select().Label("Action").Items("New file", "Open", "Save", "Save as", "Exit").SelectedIndex(selectedDropdown);

                        gui.HorizontalSeparator();
                        gui.Heading("Closable tabs");
                        selectedTab = gui.Tabs()
                            .Id("advanced_documents")
                            .Items(tabs)
                            .Closable(closable)
                            .SelectedIndex(selectedTab)
                            .Content(() =>
                            {
                                if (tabs.Length > 0)
                                    gui.Card().Title(tabs[Mathf.Clamp(selectedTab, 0, tabs.Length - 1)]).Content("Tab content is rendered by the Tabs builder.").Render();
                            });

                        gui.HorizontalSeparator();
                        gui.Heading("Text area");
                        text = gui.TextArea(text).Label("Notes").Placeholder("Type here...").MinHeight(100f).MaxHeight(180f);
                        gui.Caption("Text length: " + text.Length);

                        gui.HorizontalSeparator();
                        gui.Heading("Navigation");
                        selectedNavigation = gui.Navigation()
                            .Logo("APP")
                            .Items(new NavigationItem("dashboard", "Dashboard"), new NavigationItem("analytics", "Analytics"), new NavigationItem("projects", "Projects"), new NavigationItem("settings", "Settings"))
                            .SelectedIndex(selectedNavigation)
                            .Width(180f);

                        gui.HorizontalSeparator();
                        gui.Heading("Theme & font");
                        gui.ThemeChanger().Width(220f).ShowPreview().Render();
                        gui.FontChanger().Width(220f).ShowPreview().Render();
                        gui.EndColumn();
                    },
                    GUILayout.Width(windowRect.width - 20f),
                    GUILayout.Height(windowRect.height - 60f)
                );
            }

            gui.EndGUI();
            GUI.DragWindow();
        }

        private void OnDestroy() => gui?.Cleanup();
    }
}
