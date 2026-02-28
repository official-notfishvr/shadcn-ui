using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class LayoutExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 600);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        void Start()
        {
            gui = new GUIHelper();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(2, windowRect, DrawLayoutWindow, "Layout & Cards Example");
            }
            gui.DrawOverlay();
        }

        void DrawLayoutWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI()) return;

            scroll = gui.ScrollView(scroll, () =>
            {
                gui.BeginVerticalGroup();

                gui.Label("Simple Card Example", ControlVariant.Default);
                gui.Card("Welcome", "Getting Started", "This is a basic card with title, description, and content.");

                gui.AddSpace(20);

                gui.Label("Card with Footer", ControlVariant.Default);
                gui.Card("Notification", "Settings", "Configure your notification preferences.",
                    () =>
                    {
                        gui.BeginHorizontalGroup();
                        GUILayout.FlexibleSpace();
                        if (gui.Button("Save", ControlVariant.Default, ControlSize.Small))
                            gui.ShowSuccessToast("Saved!");
                        if (gui.Button("Cancel", ControlVariant.Ghost, ControlSize.Small))
                            gui.ShowToast("Cancelled");
                        gui.EndHorizontalGroup();
                    });

                gui.AddSpace(20);

                gui.Label("Horizontal Layout with Cards", ControlVariant.Default);
                gui.BeginHorizontalGroup();
                gui.SimpleCard("Card 1\nContent here", 180, 100);
                gui.AddSpace(10);
                gui.SimpleCard("Card 2\nMore content", 180, 100);
                gui.AddSpace(10);
                gui.SimpleCard("Card 3\nEven more", 180, 100);
                gui.EndHorizontalGroup();

                gui.AddSpace(20);

                gui.Label("Card with Custom Header", ControlVariant.Default);
                gui.BeginCard(400, 200);
                gui.CardHeader(() =>
                {
                    gui.BeginHorizontalGroup();
                    gui.Badge("New", ControlVariant.Default);
                    GUILayout.FlexibleSpace();
                    gui.MutedLabel("Just now");
                    gui.EndHorizontalGroup();
                });
                gui.CardTitle("Custom Header Card");
                gui.CardDescription("This card uses CardHeader, CardTitle, CardDescription, CardContent, and CardFooter.");
                gui.CardContent(() =>
                {
                    gui.Progress(0.75f, 350, 8);
                    gui.MutedLabel("75% complete");
                });
                gui.CardFooter(() =>
                {
                    gui.BeginHorizontalGroup();
                    if (gui.Button("View Details", ControlVariant.Outline, ControlSize.Small))
                        gui.ShowToast("Viewing details...");
                    GUILayout.FlexibleSpace();
                    gui.EndHorizontalGroup();
                });
                gui.EndCard();

                gui.AddSpace(20);

                gui.Label("Separators", ControlVariant.Default);
                gui.HorizontalSeparator();
                gui.MutedLabel("Above: Horizontal Separator");
                gui.AddSpace(10);
                gui.LabeledSeparator("Section Divider");
                gui.AddSpace(10);
                gui.SeparatorWithSpacing(SeparatorOrientation.Horizontal, 10, 10);

                gui.EndVerticalGroup();
            }, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
