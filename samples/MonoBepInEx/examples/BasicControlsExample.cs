using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class BasicControlsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 680);
        private Vector2 scroll;
        private string displayName = "Ada Lovelace";
        private string password = "correct horse battery staple";
        private string notes = "Try the current builder API used throughout this example.";
        private bool featureEnabled = true;
        private bool notifications;
        private bool compactMode;
        private float volume = 65f;
        private Vector2 priceRange = new Vector2(20f, 80f);
        private int selectedOption;

        private readonly string[] options = { "Option 1", "Option 2", "Option 3", "Option 4" };

        private void Start() => gui = new GUIHelper();

        private void OnGUI()
        {
            windowRect = GUI.Window(1, windowRect, DrawWindow, "Basic Controls");
            gui.DrawOverlays();
        }

        private void DrawWindow(int windowID)
        {
            gui.UpdateGUI(true);
            if (!gui.BeginGUI())
                return;

            using (gui.Scope("basic-controls"))
            {
                scroll = gui.ScrollView(
                    scroll,
                    () =>
                    {
                        gui.BeginColumn();
                        gui.Heading("Inputs & controls");
                        gui.MutedLabel("Value-returning builders can be assigned directly.");

                        displayName = gui.Input(displayName).Label("Name").Placeholder("Your name");
                        password = gui.Input(password).Label("Password").Password();
                        notes = gui.TextArea(notes).Label("Notes").Placeholder("Add a note...");

                        gui.HorizontalSeparator();
                        gui.Heading("Actions");
                        gui.BeginRow();
                        if (gui.Button("Save"))
                            gui.Toast().Title("Saved").Description("Your changes were saved.").Variant(ToastVariant.Success).Render();
                        if (gui.Button("Reset").Outline().Small())
                        {
                            displayName = "Ada Lovelace";
                            gui.Toast().Title("Form reset").Render();
                        }
                        gui.EndRow();

                        gui.HorizontalSeparator();
                        gui.Heading("Boolean controls");
                        featureEnabled = gui.Checkbox("Enable feature", featureEnabled).HelperText("Checkbox builder");
                        notifications = gui.Switch("Notifications", notifications);
                        compactMode = gui.Toggle("Compact mode", compactMode);

                        gui.HorizontalSeparator();
                        gui.Heading("Numeric controls");
                        volume = gui.Slider(volume).Label("Volume").Range(0f, 100f).Step(5f).ShowValue();
                        priceRange = gui.RangeSlider(priceRange.x, priceRange.y).Label("Price range").Range(0f, 100f).Step(5f).ShowValue();
                        selectedOption = gui.Select().Id("basic_option_select").Label("Select an option").Items(options).SelectedIndex(selectedOption).MaxHeight(180f).Width(240f).Render();

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
