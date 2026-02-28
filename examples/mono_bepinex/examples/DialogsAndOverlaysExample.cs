using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class DialogsAndOverlaysExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 550, 650);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        private bool showConfirmDialog = false;
        private bool showFormDialog = false;

        private string dialogName = "";
        private string dialogEmail = "";
        private bool dialogSubscribe = false;

        void Start()
        {
            gui = new GUIHelper();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(5, windowRect, DrawDialogsWindow, "Dialogs & Overlays Example");
            }

            if (showConfirmDialog)
            {
                gui.Dialog("confirm_dialog", "Confirm Action", "Are you sure you want to delete this item? This action cannot be undone.",
                    () =>
                    {
                        gui.Label("This is a confirmation dialog with a title, description, and footer buttons.", ControlVariant.Default);
                    },
                    () =>
                    {
                        gui.BeginHorizontalGroup();
                        GUILayout.FlexibleSpace();
                        if (gui.Button("Cancel", ControlVariant.Outline, ControlSize.Small, () => showConfirmDialog = false))
                            showConfirmDialog = false;
                        gui.AddSpace(10);
                        if (gui.Button("Delete", ControlVariant.Destructive, ControlSize.Small, () =>
                        {
                            showConfirmDialog = false;
                            gui.ShowSuccessToast("Item deleted successfully");
                        }));
                        gui.EndHorizontalGroup();
                    }, 400, 250);
            }

            if (showFormDialog)
            {
                gui.Dialog("form_dialog", "Edit Profile", "Update your profile information.",
                    () =>
                    {
                        gui.LabeledInput("Name", dialogName, "Enter your name");
                        gui.LabeledInput("Email", dialogEmail, "Enter your email");
                        dialogSubscribe = gui.Checkbox("Subscribe to newsletter", dialogSubscribe);
                    },
                    () =>
                    {
                        gui.BeginHorizontalGroup();
                        GUILayout.FlexibleSpace();
                        if (gui.Button("Cancel", ControlVariant.Ghost, ControlSize.Small, () => showFormDialog = false))
                            showFormDialog = false;
                        gui.AddSpace(10);
                        if (gui.Button("Save Changes", ControlVariant.Default, ControlSize.Small, () =>
                        {
                            showFormDialog = false;
                            gui.ShowSuccessToast("Profile updated!");
                        }));
                        gui.EndHorizontalGroup();
                    }, 400, 350);
            }

            gui.DrawOverlay();
        }

        void DrawDialogsWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI()) return;

            scroll = gui.ScrollView(scroll, () =>
            {
                gui.BeginVerticalGroup();

                gui.Label("Dialogs", ControlVariant.Default);
                gui.MutedLabel("Modal dialogs for user interactions");

                gui.AddSpace(10);

                if (gui.Button("Open Confirm Dialog", ControlVariant.Default))
                    showConfirmDialog = true;

                gui.AddSpace(10);

                if (gui.Button("Open Form Dialog", ControlVariant.Default))
                    showFormDialog = true;

                gui.HorizontalSeparator();

                gui.Label("Toasts", ControlVariant.Default);
                gui.MutedLabel("Non-intrusive notifications");

                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                if (gui.Button("Success Toast", ControlVariant.Default))
                    gui.ShowSuccessToast("Operation completed!", "Your changes have been saved.");
                if (gui.Button("Error Toast", ControlVariant.Destructive))
                    gui.ShowErrorToast("Something went wrong", "Please try again later.");
                gui.EndHorizontalGroup();

                gui.BeginHorizontalGroup();
                if (gui.Button("Warning Toast", ControlVariant.Secondary))
                    gui.ShowWarningToast("Attention needed", "Please review your input.");
                if (gui.Button("Info Toast", ControlVariant.Outline))
                    gui.ShowInfoToast("Did you know?", "You can customize toast duration.");
                gui.EndHorizontalGroup();

                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                if (gui.Button("Custom Toast", ControlVariant.Default))
                {
                    gui.ShowToast("Custom Notification", "This is a custom toast with default styling.", ToastVariant.Default);
                }
                gui.EndHorizontalGroup();

                gui.HorizontalSeparator();

                gui.Label("Tooltips", ControlVariant.Default);
                gui.MutedLabel("Hover over elements to see tooltips");

                gui.AddSpace(10);

                gui.WithTooltip("This button has a tooltip!", () =>
                {
                    gui.Button("Hover Me for Tooltip", ControlVariant.Default);
                });

                gui.AddSpace(10);

                gui.WithTooltip("Enter your username here", () =>
                {
                    gui.InputLabel("Username:");
                    gui.Input("", "username");
                });

                gui.HorizontalSeparator();

                gui.Label("Popover", ControlVariant.Default);

                if (gui.Button("Open Popover", ControlVariant.Outline))
                    gui.OpenPopover();

                gui.Popover(() =>
                {
                    gui.BeginVerticalGroup();
                    gui.Label("Popover Content", ControlVariant.Default);
                    gui.MutedLabel("This appears when you click the button");
                    gui.AddSpace(10);
                    if (gui.Button("Action in Popover", ControlVariant.Ghost, ControlSize.Small))
                        gui.ClosePopover();
                    gui.EndVerticalGroup();
                });

                gui.EndVerticalGroup();
            }, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
