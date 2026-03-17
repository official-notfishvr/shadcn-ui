using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class BasicControlsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 550, 700);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        private string inputText = "";
        private string password = "";
        private string description = "";
        private bool checkboxValue = false;
        private bool toggleValue = false;
        private bool switchValue = false;
        private float sliderValue = 50f;
        private int selectedOption = 0;
        private string[] options = new[] { "Option 1", "Option 2", "Option 3", "Option 4" };

        void Start()
        {
            gui = new GUIHelper();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(1, windowRect, DrawControlsWindow, "Basic Controls Example");
            }

            gui.DrawOverlay();
        }

        void DrawControlsWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI())
                return;

            scroll = gui.ScrollView(
                scroll,
                () =>
                {
                    gui.BeginVerticalGroup();

                    gui.Label("Text Inputs");
                    gui.Label("Basic Input:");
                    inputText = gui.Input(inputText, "Type something...");

                    gui.Label("Password Field:");
                    password = gui.Password(
                        new shadcnui.GUIComponents.Core.Utils.InputConfig
                        {
                            Value = password,
                            Label = "Password",
                            MaskCharacter = '*',
                            Width = Mathf.RoundToInt(windowRect.width - 40),
                        }
                    );

                    gui.Label("Description:");
                    description = gui.TextArea(description, ControlVariant.Default, "Enter description...", false, 60f);

                    gui.HorizontalSeparator();

                    gui.Label("Buttons");
                    gui.BeginHorizontalGroup();
                    if (gui.Button("Primary", ControlVariant.Default))
                        gui.ShowSuccessToast("Primary clicked!");
                    if (gui.Button("Secondary", ControlVariant.Secondary))
                        gui.ShowInfoToast("Secondary clicked!");
                    if (gui.Button("Destructive", ControlVariant.Destructive))
                        gui.ShowErrorToast("Destructive clicked!");
                    gui.EndHorizontalGroup();

                    gui.BeginHorizontalGroup();
                    if (gui.Button("Outline", ControlVariant.Outline))
                        gui.ShowToast("Outline clicked!");
                    if (gui.Button("Ghost", ControlVariant.Ghost))
                        gui.ShowToast("Ghost clicked!");
                    if (gui.Button("Link", ControlVariant.Link))
                        gui.ShowToast("Link clicked!");
                    gui.EndHorizontalGroup();

                    gui.HorizontalSeparator();

                    gui.Label("Toggles & Checkboxes");
                    checkboxValue = gui.Checkbox("Enable Feature", checkboxValue);
                    toggleValue = gui.Toggle("Toggle State", toggleValue);
                    switchValue = gui.Switch("Switch Control", switchValue);

                    gui.HorizontalSeparator();

                    gui.Label("Slider");
                    sliderValue = gui.Slider(
                        new shadcnui.GUIComponents.Core.Utils.SliderConfig
                        {
                            Label = "Volume",
                            Value = sliderValue,
                            MinValue = 0f,
                            MaxValue = 100f,
                        }
                    );

                    gui.HorizontalSeparator();

                    gui.Label("Select Dropdown");
                    selectedOption = gui.Select(null, options, selectedOption);

                    gui.EndVerticalGroup();
                },
                GUILayout.Width(windowRect.width - 20),
                GUILayout.Height(windowRect.height - 60)
            );

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
