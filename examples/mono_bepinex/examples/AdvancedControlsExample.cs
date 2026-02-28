using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.MenuBar;

namespace shadcnui_examples.Examples
{
    public class AdvancedControlsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 550, 650);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        private string[] dropdownItems = new[] { "New File", "Open...", "Save", "Save As...", "Exit" };
        private string[] menuItems = new[] { "File", "Edit", "View", "Help" };
        private int selectedMenuItem = 0;

        private string resizableText = "This is a resizable text area. Drag the handle at the bottom to resize.";
        private float textAreaHeight = 80f;

        private string[] closableTabs = new[] { "Document 1", "Document 2", "Document 3" };
        private bool[] closableTabStates = new[] { true, true, true };
        private int selectedClosableTab = 0;

        void Start()
        {
            gui = new GUIHelper();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(8, windowRect, DrawAdvancedWindow, "Advanced Controls Example");
            }
            gui.DrawOverlay();
        }

        void DrawAdvancedWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI()) return;

            scroll = gui.ScrollView(scroll, () =>
            {
                gui.BeginVerticalGroup();

                gui.Label("Menu Bar", ControlVariant.Default);
                var menuBarItems = new System.Collections.Generic.List<MenuItem>
                {
                    new MenuItem("File", () => gui.ShowToast("File clicked")),
                    new MenuItem("Edit", () => gui.ShowToast("Edit clicked")),
                    new MenuItem("View", () => gui.ShowToast("View clicked")),
                    new MenuItem("Help", () => gui.ShowToast("Help clicked"))
                };
                gui.MenuBar(menuBarItems);

                gui.HorizontalSeparator();

                gui.Label("Dropdown Menu", ControlVariant.Default);
                if (gui.Button("Open Dropdown", ControlVariant.Outline))
                    gui.OpenSelect();

                gui.Select(dropdownItems, 0);

                gui.HorizontalSeparator();

                gui.Label("Closable Tabs", ControlVariant.Default);
                gui.ClosableTabs(ref closableTabs, ref closableTabStates, selectedClosableTab, null, index =>
                {
                    selectedClosableTab = index;
                    gui.ShowToast($"Selected tab: {closableTabs[index]}");
                });

                gui.BeginTabContent();
                if (selectedClosableTab >= 0 && selectedClosableTab < closableTabs.Length)
                {
                    gui.MutedLabel($"Content for {closableTabs[selectedClosableTab]}");
                    gui.Label("This content is shown within the selected tab.", ControlVariant.Default);
                }
                gui.EndTabContent();

                gui.HorizontalSeparator();

                gui.Label("Resizable Text Area", ControlVariant.Default);
                resizableText = gui.ResizableTextArea(resizableText, ref textAreaHeight, ControlVariant.Default, "Type here...", false, 60f, 200f);
                gui.MutedLabel($"Current height: {textAreaHeight:F0}px");

                gui.HorizontalSeparator();

                gui.Label("Navigation", ControlVariant.Default);
                selectedMenuItem = gui.Sidebar(menuItems, selectedMenuItem, null, "APP", index =>
                {
                    gui.ShowToast($"Navigated to {menuItems[index]}");
                }, 80);

                gui.HorizontalSeparator();

                gui.Label("Theme Variants", ControlVariant.Default);
                gui.BeginHorizontalGroup();
                gui.ThemeChangerCompact();
                gui.AddSpace(20);
                gui.ThemeChangerWithPreview("theme_preview", 200);
                gui.EndHorizontalGroup();

                gui.EndVerticalGroup();
            }, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
