#define Showcase

using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class DemoSelector : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect selectorRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 380);
        private bool showSelector = true;
        private GameObject currentDemo;

        void Start()
        {
            guiHelper = new GUIHelper();
        }

        void OnGUI()
        {
            if (showSelector)
            {
                selectorRect = GUI.Window(100, selectorRect, (GUI.WindowFunction)DrawSelectorWindow, "Select Demo");
            }
        }

        void DrawSelectorWindow(int windowID)
        {
            guiHelper.UpdateAnimations(showSelector);
            if (guiHelper.BeginAnimatedGUI())
            {
                guiHelper.BeginVerticalGroup();
                GUILayout.Space(10);

                guiHelper.Label("Choose a Demo to Load", ControlVariant.Default);
                guiHelper.MutedLabel("Select which component showcase you want to view");
                guiHelper.HorizontalSeparator();

                GUILayout.Space(20);

                if (guiHelper.Button("Component Showcase", ControlVariant.Default, ControlSize.Default))
                {
                    LoadDemo<ComponentShowcase>();
                }

                GUILayout.Space(10);

                if (guiHelper.Button("Full Demo", ControlVariant.Secondary, ControlSize.Default))
                {
                    LoadDemo<FullDemo>();
                }

                GUILayout.Space(10);

                if (guiHelper.Button("Documentation", ControlVariant.Outline, ControlSize.Default))
                {
                    LoadDemo<DocsDemo>();
                }
#if Showcase

                GUILayout.Space(10);

                if (guiHelper.Button("Screenshot Utility", ControlVariant.Ghost, ControlSize.Default))
                {
                    LoadDemo<ScreenshotUtility>();
                }
#endif
                GUILayout.Space(20);
                guiHelper.HorizontalSeparator();

                guiHelper.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                if (guiHelper.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                {
                    showSelector = false;
                }
                guiHelper.EndHorizontalGroup();

                GUILayout.EndVertical();
                guiHelper.EndAnimatedGUI();
            }
            GUI.DragWindow();
        }

        void LoadDemo<T>()
            where T : MonoBehaviour
        {
            if (currentDemo != null)
            {
#if Showcase
#elif! Showcase

                Destroy(currentDemo);
#endif
            }

            currentDemo = new GameObject(typeof(T).Name);
            currentDemo.AddComponent<T>();
            DontDestroyOnLoad(currentDemo);
#if Showcase
#elif !Showcase
            showSelector = false;
#endif
            Debug.Log($"Loaded {typeof(T).Name} demo");
        }

        void OnDestroy()
        {
            if (currentDemo != null)
            {
                Destroy(currentDemo);
            }
        }
    }
}
