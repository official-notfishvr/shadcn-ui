using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class OverlaysExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 800, 680);
        private Vector2 scroll;
        private bool confirmOpen;
        private bool formOpen;
        private bool popoverOpen;
        private string profileName = "Ada Lovelace";
        private string profileEmail = "ada@example.com";
        private bool subscribe;

        private void Start() => gui = new GUIHelper();

        private void OnGUI()
        {
            windowRect = GUI.Window(5, windowRect, DrawWindow, "Overlays");
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
                    gui.Heading("Toasts");
                    gui.BeginRow();
                    if (gui.Button("Success"))
                        gui.Toast().Title("Success").Description("The operation completed.").Variant(ToastVariant.Success).Render();
                    if (gui.Button("Warning").Outline())
                        gui.Toast().Title("Review needed").Description("Check the highlighted settings.").Variant(ToastVariant.Warning).Render();
                    if (gui.Button("Error").Destructive())
                        gui.Toast().Title("Could not save").Description("Please try again.").Variant(ToastVariant.Error).Render();
                    gui.EndRow();

                    gui.HorizontalSeparator();
                    gui.Heading("Tooltip & popover");
                    gui.WithTooltip("Tooltips are flushed by DrawOverlays.", () => gui.Button("Hover me").Ghost().Render());
                    gui.Space(8f);
                    if (gui.Button(popoverOpen ? "Close popover" : "Open popover").Outline())
                        popoverOpen = !popoverOpen;
                    var popover = gui.Popover("help_popover")
                        .Content(() =>
                        {
                            gui.BeginColumn();
                            gui.Heading("Quick help");
                            gui.MutedLabel("Popover content can contain any current GUIHelper component.");
                            if (gui.Button("Done").Small())
                                popoverOpen = false;
                            gui.EndColumn();
                        });
                    if (popoverOpen)
                        popover.Open();
                    popover.Render();

                    gui.HorizontalSeparator();
                    gui.Heading("Inline feedback");
                    gui.ErrorAlert("This is an error alert rendered as a destructive label.");
                },
                GUILayout.Width(windowRect.width - 20f),
                GUILayout.Height(windowRect.height - 60f)
            );

            gui.EndGUI();
            GUI.DragWindow();
        }

        private void OnDestroy() => gui?.Cleanup();
    }
}
