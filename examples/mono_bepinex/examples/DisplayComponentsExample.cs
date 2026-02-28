using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class DisplayComponentsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 550, 700);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        private float progressValue = 0.65f;
        private float animatedProgress = 0f;
        private bool isAnimating = false;

        void Start()
        {
            gui = new GUIHelper();
        }

        void Update()
        {
            if (isAnimating)
            {
                animatedProgress = Mathf.PingPong(Time.time, 1f);
            }
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(6, windowRect, DrawDisplayWindow, "Display Components Example");
            }
            gui.DrawOverlay();
        }

        void DrawDisplayWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI()) return;

            scroll = gui.ScrollView(scroll, () =>
            {
                gui.BeginVerticalGroup();

                gui.Label("Badges", ControlVariant.Default);
                gui.MutedLabel("Status indicators and labels");
                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.Badge("Default", ControlVariant.Default);
                gui.AddSpace(5);
                gui.Badge("Secondary", ControlVariant.Secondary);
                gui.AddSpace(5);
                gui.Badge("Destructive", ControlVariant.Destructive);
                gui.AddSpace(5);
                gui.Badge("Outline", ControlVariant.Outline);
                gui.EndHorizontalGroup();

                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.PillBadge("Pill Badge");
                gui.AddSpace(5);
                gui.RoundedBadge("Rounded", ControlVariant.Default, ControlSize.Default, 8f);
                gui.AddSpace(5);
                gui.CountBadge(5, ControlVariant.Destructive);
                gui.AddSpace(5);
                gui.CountBadge(100, ControlVariant.Default, ControlSize.Default, 99);
                gui.EndHorizontalGroup();

                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.StatusBadge("Online", true, ControlVariant.Default);
                gui.AddSpace(5);
                gui.StatusBadge("Offline", false, ControlVariant.Secondary);
                gui.AddSpace(5);
                gui.ProgressBadge("Loading", 0.6f, ControlVariant.Secondary);
                gui.EndHorizontalGroup();

                gui.HorizontalSeparator();

                gui.Label("Progress Indicators", ControlVariant.Default);
                gui.MutedLabel("Show completion or loading states");
                gui.AddSpace(10);

                gui.LabeledProgress("Upload Progress", progressValue, 350, 8, true);
                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.CircularProgress(animatedProgress, 48f);
                gui.AddSpace(20);
                gui.BeginVerticalGroup();
                gui.Label("Animated Progress", ControlVariant.Default);
                isAnimating = gui.Toggle("Enable Animation", isAnimating);
                gui.EndVerticalGroup();
                gui.EndHorizontalGroup();

                gui.AddSpace(10);

                gui.Progress(0.3f, 350, 6);
                gui.AddSpace(5);
                gui.AnimatedProgress("progress1", 0.6f, 350, 6);
                gui.AddSpace(5);
                gui.Progress(0.9f, 350, 6);

                gui.HorizontalSeparator();

                gui.Label("Avatars", ControlVariant.Default);
                gui.MutedLabel("User profile pictures");
                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.Avatar(null, "JD", ControlSize.Small, AvatarShape.Circle);
                gui.AddSpace(10);
                gui.Avatar(null, "AB", ControlSize.Default, AvatarShape.Circle);
                gui.AddSpace(10);
                gui.Avatar(null, "XY", ControlSize.Large, AvatarShape.Square);
                gui.EndHorizontalGroup();

                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.AvatarWithStatus(null, "ON", true, ControlSize.Default, AvatarShape.Circle);
                gui.AddSpace(10);
                gui.AvatarWithStatus(null, "OFF", false, ControlSize.Default, AvatarShape.Circle);
                gui.AddSpace(10);
                gui.AvatarWithName(null, "NM", "John Doe", ControlSize.Default, AvatarShape.Circle, true);
                gui.EndHorizontalGroup();

                gui.AddSpace(10);

                gui.BeginHorizontalGroup();
                gui.AvatarWithBorder(null, "BD", Color.red, ControlSize.Default, AvatarShape.Circle);
                gui.EndHorizontalGroup();

                gui.HorizontalSeparator();

                gui.Label("Labels", ControlVariant.Default);
                gui.MutedLabel("Text styling variations");
                gui.AddSpace(10);

                gui.Label("Default Label", ControlVariant.Default);
                gui.SecondaryLabel("Secondary Label - less emphasis");
                gui.MutedLabel("Muted Label - disabled or placeholder text");
                gui.DestructiveLabel("Destructive Label - errors or warnings");

                gui.HorizontalSeparator();

                gui.Label("Theme Changer", ControlVariant.Default);
                gui.ThemeChanger();

                gui.EndVerticalGroup();
            }, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
