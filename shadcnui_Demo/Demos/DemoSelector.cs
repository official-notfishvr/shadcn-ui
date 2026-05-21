#define Showcase
using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class DemoSelector : MonoBehaviour
    {
        private sealed class DemoEntry
        {
            public string Id;
            public string Title;
            public string Category;
            public string Description;
            public ControlVariant Accent;
            public Action Load;
        }

        private GUIHelper guiHelper;
        private Rect selectorRect = new Rect(Screen.width / 2f - 320f, Screen.height / 2f - 240f, 640f, 620f);
        private Vector2 scrollPosition;
        private bool showSelector = true;
        private GameObject currentDemo;
        private string currentDemoName = "None";
        private string searchQuery = string.Empty;
        private List<DemoEntry> demoEntries;

        void Start()
        {
            guiHelper = new GUIHelper();
            demoEntries = BuildEntries();
        }

        void OnGUI()
        {
            if (showSelector)
                selectorRect = GUI.Window(100, selectorRect, (GUI.WindowFunction)DrawSelectorWindow, string.Empty);
        }

        void DrawSelectorWindow(int windowID)
        {
            guiHelper.UpdateGUI(showSelector);
            if (!guiHelper.BeginGUI())
                return;

            DrawHeader();
            guiHelper.HorizontalSeparator();
            DrawSearchBar();
            guiHelper.AddSpace(8f);

            scrollPosition = guiHelper.ScrollView(scrollPosition, DrawDemoGrid, GUILayout.Height(selectorRect.height - 150f), GUILayout.ExpandWidth(true));

            guiHelper.AddSpace(10f);
            guiHelper.HorizontalSeparator();
            DrawFooter();

            guiHelper.EndGUI();
            GUI.DragWindow(new Rect(0f, 0f, selectorRect.width, 48f));
        }

        void DrawHeader()
        {
            guiHelper.BeginHorizontalGroup();

            guiHelper.BeginVerticalGroup();
            guiHelper.Heading("Demo Launcher");
            guiHelper.AddSpace(2f);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Badge(currentDemoName == "None" ? "No Demo Loaded" : currentDemoName, ControlVariant.Outline);
            guiHelper.EndHorizontalGroup();
            guiHelper.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            guiHelper.AddSpace(2f);
            guiHelper.CountBadge(GetVisibleEntries().Count, ControlVariant.Secondary);

            guiHelper.EndHorizontalGroup();
        }

        void DrawSearchBar()
        {
            searchQuery = guiHelper.Input(searchQuery, "Search demos", disabled: false, opts: new[] { GUILayout.Width(320f) });
        }

        void DrawDemoGrid()
        {
            List<DemoEntry> visibleEntries = GetVisibleEntries();
            if (visibleEntries.Count == 0)
            {
                guiHelper.ErrorAlert("No demos match the current search.");
                return;
            }

            for (int i = 0; i < visibleEntries.Count; i += 2)
            {
                guiHelper.BeginHorizontalGroup();
                DrawDemoCard(visibleEntries[i]);
                guiHelper.AddSpace(12f);

                if (i + 1 < visibleEntries.Count)
                    DrawDemoCard(visibleEntries[i + 1]);
                else
                    GUILayout.Space(292f);

                guiHelper.EndHorizontalGroup();
                guiHelper.AddSpace(12f);
            }
        }

        void DrawDemoCard(DemoEntry entry)
        {
            guiHelper.BeginCard(280f, -1f, ControlVariant.Default, ControlSize.Default);
            guiHelper.CardHeader(() =>
            {
                guiHelper.BeginHorizontalGroup();
                guiHelper.BeginVerticalGroup();
                guiHelper.Heading(entry.Title);
                guiHelper.Caption(entry.Description);
                guiHelper.EndVerticalGroup();
                GUILayout.FlexibleSpace();
                guiHelper.Badge(entry.Category, entry.Accent, ControlSize.Small);
                guiHelper.EndHorizontalGroup();
            });

            guiHelper.CardContent(() =>
            {
                guiHelper.Caption(currentDemoName == entry.Title ? "Currently loaded" : "Ready");
            });

            guiHelper.CardFooter(() =>
            {
                guiHelper.BeginHorizontalGroup();
                if (guiHelper.Button(currentDemoName == entry.Title ? "Reload" : "Open Demo", currentDemoName == entry.Title ? ControlVariant.Secondary : ControlVariant.Default, ControlSize.Small))
                    entry.Load();

                GUILayout.FlexibleSpace();

                if (currentDemoName == entry.Title)
                    guiHelper.StatusBadge("Active", true);
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();
        }

        void DrawFooter()
        {
            guiHelper.BeginHorizontalGroup();
            if (guiHelper.Button("Clear Search", ControlVariant.Ghost, ControlSize.Small))
                searchQuery = string.Empty;
            GUILayout.FlexibleSpace();
            if (guiHelper.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                showSelector = false;
            guiHelper.EndHorizontalGroup();
        }

        List<DemoEntry> BuildEntries()
        {
            var entries = new List<DemoEntry>
            {
                new DemoEntry
                {
                    Id = nameof(FullDemo),
                    Title = "Full Demo",
                    Category = "Flagship",
                    Description = "Large multi-section showcase.",
                    Accent = ControlVariant.Secondary,
                    Load = () => LoadDemo<FullDemo>("Full Demo"),
                },
                new DemoEntry
                {
                    Id = nameof(FullDemo_old),
                    Title = "Full Demo OLD",
                    Category = "Legacy",
                    Description = "Older all-in-one showcase.",
                    Accent = ControlVariant.Ghost,
                    Load = () => LoadDemo<FullDemo_old>("Full Demo OLD"),
                },
            };

#if Showcase
#if MONO
            entries.Add(
                new DemoEntry
                {
                    Id = nameof(ScreenshotUtility),
                    Title = "Screenshot Utility",
                    Category = "Tooling",
                    Description = "Capture PNGs and GIFs.",
                    Accent = ControlVariant.Ghost,
                    Load = () => LoadDemo<ScreenshotUtility>("Screenshot Utility"),
                }
            );
#endif
#endif

            return entries;
        }

        List<DemoEntry> GetVisibleEntries()
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return demoEntries;

            string query = searchQuery.Trim();
            var filtered = new List<DemoEntry>();
            for (int i = 0; i < demoEntries.Count; i++)
            {
                DemoEntry entry = demoEntries[i];
                if (entry.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || entry.Category.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || entry.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    filtered.Add(entry);
            }

            return filtered;
        }

        void LoadDemo<T>(string displayName)
            where T : MonoBehaviour
        {
            if (currentDemo != null)
            {
#if Showcase
#elif !Showcase
                Destroy(currentDemo);
#endif
            }

            currentDemo = new GameObject(typeof(T).Name);
            currentDemo.AddComponent<T>();
            DontDestroyOnLoad(currentDemo);
            currentDemoName = displayName;

#if Showcase
#elif !Showcase
            showSelector = false;
#endif
            Debug.Log($"Loaded {typeof(T).Name} demo");
        }

        void OnDestroy()
        {
            if (currentDemo != null)
                Destroy(currentDemo);
        }
    }
}
