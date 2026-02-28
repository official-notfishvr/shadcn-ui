using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui_examples.Examples;
using UnityEngine;

namespace shadcnui_examples.Menu
{
    public class ExamplesSelector : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect selectorRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 450);
        private bool showSelector = true;
        private GameObject currentDemo;
        private Vector2 selectorScroll = Vector2.zero;

        void Start()
        {
            guiHelper = new GUIHelper();
        }

        void OnGUI()
        {
            guiHelper.DrawOverlay();

            if (showSelector)
            {
                selectorRect = GUI.Window(100, selectorRect, (GUI.WindowFunction)DrawSelectorWindow, "shadcnui Examples");
            }
        }

        void DrawSelectorWindow(int windowID)
        {
            guiHelper.UpdateGUI(showSelector);
            if (guiHelper.BeginGUI())
            {
                guiHelper.BeginVerticalGroup();
                GUILayout.Space(10);

                guiHelper.Label("Choose a Demo to Load", ControlVariant.Default);
                guiHelper.MutedLabel("Select which component showcase you want to view");
                guiHelper.HorizontalSeparator();

                GUILayout.Space(10);

                selectorScroll = guiHelper.ScrollView(selectorScroll, () =>
                {
                    guiHelper.BeginVerticalGroup();

                    guiHelper.SectionHeader("Getting Started");
                    if (guiHelper.Button("Basic Controls", ControlVariant.Default))
                        LoadDemo<BasicControlsExample>("BasicControlsDemo");

                    guiHelper.AddSpace(5);

                    if (guiHelper.Button("Layout & Cards", ControlVariant.Default))
                        LoadDemo<LayoutExample>("LayoutDemo");

                    guiHelper.HorizontalSeparator();

                    guiHelper.SectionHeader("Data Display");
                    if (guiHelper.Button("Tables", ControlVariant.Default))
                        LoadDemo<TablesExample>("TablesDemo");

                    guiHelper.AddSpace(5);

                    if (guiHelper.Button("Charts", ControlVariant.Default))
                        LoadDemo<ChartsExample>("ChartsDemo");

                    guiHelper.AddSpace(5);

                    if (guiHelper.Button("Display Components", ControlVariant.Default))
                        LoadDemo<DisplayComponentsExample>("DisplayDemo");

                    guiHelper.HorizontalSeparator();

                    guiHelper.SectionHeader("Navigation & Layout");
                    if (guiHelper.Button("Tabs & Navigation", ControlVariant.Default))
                        LoadDemo<TabsAndNavigationExample>("TabsDemo");

                    guiHelper.AddSpace(5);

                    if (guiHelper.Button("Dialogs & Overlays", ControlVariant.Default))
                        LoadDemo<DialogsAndOverlaysExample>("DialogsDemo");

                    guiHelper.AddSpace(5);

                    if (guiHelper.Button("Advanced Controls", ControlVariant.Default))
                        LoadDemo<AdvancedControlsExample>("AdvancedDemo");

                    guiHelper.EndVerticalGroup();
                }, GUILayout.Height(250));

                GUILayout.Space(15);
                guiHelper.HorizontalSeparator();

                guiHelper.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                if (guiHelper.Button("Close Selector", ControlVariant.Ghost, ControlSize.Small))
                {
                    showSelector = false;
                }
                guiHelper.EndHorizontalGroup();

                guiHelper.EndVerticalGroup();
                guiHelper.EndGUI();
            }
            GUI.DragWindow();
        }

        void LoadDemo<T>(string name) where T : MonoBehaviour
        {
            if (currentDemo != null)
                Destroy(currentDemo);

            currentDemo = new GameObject(name);
            currentDemo.AddComponent<T>();
            showSelector = false;

            guiHelper.ShowSuccessToast("Demo Loaded", $"{name} is now running. Press ESC to return to selector.");
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                showSelector = !showSelector;
            }

            if (UnityEngine.InputSystem.Keyboard.current.deleteKey.wasPressedThisFrame && currentDemo != null)
            {
                Destroy(currentDemo);
                currentDemo = null;
                guiHelper.ShowToast("Demo Unloaded");
            }
        }

        void OnDestroy()
        {
            if (currentDemo != null)
            {
                Destroy(currentDemo);
            }
            guiHelper.Cleanup();
        }
    }
}
