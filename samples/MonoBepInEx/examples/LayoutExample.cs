using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class LayoutExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 620);
        private Vector2 scroll;
        private float completion = 0.75f;

        private void Start() => gui = new GUIHelper();

        private void OnGUI()
        {
            windowRect = GUI.Window(2, windowRect, DrawWindow, "Layout & Cards");
            gui.DrawOverlays();
        }

        private void DrawWindow(int windowID)
        {
            gui.UpdateGUI(true);
            if (!gui.BeginGUI())
                return;

            using (gui.Scope("layout-cards"))
            {
                scroll = gui.ScrollView(
                    scroll,
                    () =>
                    {
                        gui.BeginColumn();
                        gui.Heading("Cards");
                        gui.MutedLabel("Cards compose headers, descriptions, content, and footers.");

                        gui.Card().Title("Welcome").Description("A simple card built with the current fluent API.").Content("Cards keep related information together and work in any layout.").Render();
                        gui.Space(12f);

                        gui.Card()
                            .Title("Project status")
                            .Subtitle("This week")
                            .Description("A card with custom content and actions.")
                            .Header(() => gui.Badge("On track").Secondary().Small().Render())
                            .Content("The release checklist is nearly complete.")
                            .Footer(() =>
                            {
                                gui.BeginRow();
                                gui.Flex();
                                if (gui.Button("View details").Outline().Small())
                                    gui.Toast().Title("Opening project details").Render();
                                gui.EndRow();
                            })
                            .Render();

                        gui.Space(12f);
                        gui.BeginRow();
                        gui.Card().Title("One").Content("Compact card").Size(190f).Render();
                        gui.Space(8f);
                        gui.Card().Title("Two").Content("Another card").Size(190f).Render();
                        gui.Space(8f);
                        gui.Card().Title("Three").Content("A third card").Size(190f).Render();
                        gui.EndRow();

                        gui.Space(12f);
                        gui.Heading("Progress & separators");
                        completion = gui.Slider(completion * 100f).Label("Completion").Range(0f, 100f).Step(5f).ShowValue() / 100f;
                        gui.Progress(completion).Label("Release progress").WidthValue(420f).ShowPercentage().Render();
                        gui.Separator().Text("Section divider").Spacing(10f, 10f).Render();
                        gui.Caption("The same primitives can be combined with Row, Column, and ScrollView.");
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
