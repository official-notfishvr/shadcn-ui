using System;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui_testing.Menu
{
    public class DemoSelector : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect selectorRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
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

                guiHelper.Label("Choose a Demo to Load", LabelVariant.Default);
                guiHelper.MutedLabel("Select which component showcase you want to view");
                guiHelper.HorizontalSeparator();

                GUILayout.Space(20);

                if (guiHelper.Button("Component Showcase", ButtonVariant.Default, ButtonSize.Default))
                {
                    LoadDemo<ComponentShowcase>();
                }

                GUILayout.Space(10);

                if (guiHelper.Button("Full Demo", ButtonVariant.Secondary, ButtonSize.Default))
                {
                    LoadDemo<FullDemo>();
                }

                GUILayout.Space(20);
                guiHelper.HorizontalSeparator();

                guiHelper.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                if (guiHelper.Button("Close", ButtonVariant.Ghost, ButtonSize.Small))
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
                Destroy(currentDemo);
            }

            currentDemo = new GameObject(typeof(T).Name);
            currentDemo.AddComponent<T>();
            DontDestroyOnLoad(currentDemo);

            showSelector = false;
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
