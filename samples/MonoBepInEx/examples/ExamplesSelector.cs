using shadcnui_examples.Examples;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
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
            if (showSelector)
            {
                selectorRect = GUI.Window(100, selectorRect, (GUI.WindowFunction)DrawSelectorWindow, "shadcnui Examples");
            }

            guiHelper.DrawOverlays();
        }

        void DrawSelectorWindow(int windowID)
        {
            guiHelper.UpdateGUI(showSelector);
            if (guiHelper.BeginGUI())
            {
                using (guiHelper.Scope("examples-selector"))
                {
                    guiHelper.BeginVerticalGroup();
                    GUILayout.Space(10);

                    guiHelper.Label("Choose a Demo to Load", ControlVariant.Default);
                    guiHelper.MutedLabel("Select which component showcase you want to view");
                    guiHelper.HorizontalSeparator();

                    GUILayout.Space(10);

                    selectorScroll = guiHelper.ScrollView(
                        selectorScroll,
                        () =>
                        {
                            guiHelper.BeginVerticalGroup();

                            guiHelper.Label("Getting Started", ControlVariant.Default);
                            if (guiHelper.Button("Basic Controls", ControlVariant.Default))
                                LoadDemo<BasicControlsExample>("BasicControlsDemo");

                            guiHelper.AddSpace(5);

                            if (guiHelper.Button("Layout & Cards", ControlVariant.Default))
                                LoadDemo<LayoutExample>("LayoutDemo");

                            guiHelper.HorizontalSeparator();

                            if (guiHelper.Button("Display Components", ControlVariant.Default))
                                LoadDemo<DisplayComponentsExample>("DisplayDemo");

                            guiHelper.HorizontalSeparator();

                            guiHelper.Label("Navigation & Layout", ControlVariant.Default);
                            if (guiHelper.Button("Tabs & Navigation", ControlVariant.Default))
                                LoadDemo<TabsAndNavigationExample>("TabsDemo");

                            guiHelper.AddSpace(5);

                            if (guiHelper.Button("Overlays", ControlVariant.Default))
                                LoadDemo<OverlaysExample>("OverlaysDemo");

                            guiHelper.AddSpace(5);

                            if (guiHelper.Button("Advanced Controls", ControlVariant.Default))
                                LoadDemo<AdvancedControlsExample>("AdvancedDemo");

                            guiHelper.EndVerticalGroup();
                        },
                        GUILayout.Height(250)
                    );

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
            }
            GUI.DragWindow();
        }

        void LoadDemo<T>(string name)
            where T : MonoBehaviour
        {
            if (currentDemo != null)
                Destroy(currentDemo);

            currentDemo = new GameObject(name);
            currentDemo.AddComponent<T>();
            showSelector = false;

            guiHelper.Toast().Title("Demo Loaded").Description($"{name} is now running. Press ESC to return to selector.").Render();
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
                guiHelper.Toast().Title("Demo Unloaded").Render();
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
