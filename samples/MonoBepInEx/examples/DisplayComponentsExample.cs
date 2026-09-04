using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class DisplayComponentsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 680);
        private Vector2 scroll;
        private float progress = 0.65f;
        private bool online = true;
        private bool animate;

        private void Start() => gui = new GUIHelper();

        private void Update()
        {
            if (animate)
                progress = Mathf.PingPong(Time.time * 0.35f, 1f);
        }

        private void OnGUI()
        {
            windowRect = GUI.Window(6, windowRect, DrawWindow, "Display Components");
            gui.DrawOverlays();
        }

        private void DrawWindow(int windowID)
        {
            gui.UpdateGUI(true);
            if (!gui.BeginGUI())
                return;

            using (gui.Scope("display-components"))
            {
                scroll = gui.ScrollView(
                    scroll,
                    () =>
                    {
                        gui.BeginColumn();
                        gui.Heading("Badges");
                        gui.MutedLabel("Status, counts, and progress can share one component family.");
                        gui.BeginRow();
                        gui.Badge("Default").Render();
                        gui.Badge("Secondary").Secondary().Render();
                        gui.Badge("Warning").Destructive().Render();
                        gui.Badge("Outline").Outline().Render();
                        gui.EndRow();
                        gui.Space(8f);
                        gui.BeginRow();
                        gui.Badge("Online").StatusDot(online).Secondary().Render();
                        gui.CountBadge(7, ControlVariant.Secondary);
                        gui.Badge().Text("Loading").Progress(progress).Secondary().Render();
                        gui.EndRow();

                        gui.HorizontalSeparator();
                        gui.Heading("Progress");
                        progress = gui.Slider(progress * 100f).Label("Completion").Range(0f, 100f).Step(5f).ShowValue() / 100f;
                        gui.Progress(progress).Label("Upload progress").WidthValue(420f).ShowPercentage().Render();
                        animate = gui.Switch("Animate progress", animate);

                        gui.HorizontalSeparator();
                        gui.Heading("Avatars");
                        gui.BeginRow();
                        gui.Avatar().Fallback("JD").Shape(AvatarShape.Circle).Small().Render();
                        gui.Space(12f);
                        gui.Avatar().Fallback("AB").Shape(AvatarShape.Circle).Online().Render();
                        gui.Space(12f);
                        gui.Avatar().Fallback("XY").Shape(AvatarShape.Square).Border(Color.cyan).Large().Render();
                        gui.EndRow();

                        gui.HorizontalSeparator();
                        gui.Heading("Labels & theme");
                        gui.Label("Default label", ControlVariant.Default);
                        gui.Label("Secondary label", ControlVariant.Secondary);
                        gui.MutedLabel("Muted label");
                        gui.ErrorAlert("Destructive label");
                        gui.ThemeChanger().Width(220f).ShowPreview().Render();
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
