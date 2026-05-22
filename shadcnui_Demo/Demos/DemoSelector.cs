#define Showcase
using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class DemoSelector : MonoBehaviour
    {
        private sealed class DemoEntry
        {
            public string Id;
            public string Title;
            public string Description;
            public Action Load;
        }

        private GUIHelper _gui;
        private Rect _windowRect = new Rect(42f, 42f, 770f, 620f);
        private Vector2 _scroll;
        private bool _showSelector = true;
        private GameObject _currentDemo;
        private string _currentDemoName = "None";
        private string _searchQuery = string.Empty;
        private List<DemoEntry> _entries;

        private ComponentAppearance _panel;
        private ComponentAppearance _activePanel;
        private ComponentAppearance _input;
        private ComponentAppearance _pill;
        private ComponentAppearance _primary;

        private void Start()
        {
            _gui = new GUIHelper();
            _gui.SetTheme("Zinc");
            _gui.SetFontSize(13);
            BuildAppearances();
            _entries = BuildEntries();
        }

        private void OnGUI()
        {
            if (_showSelector)
                _windowRect = GUI.Window(100, _windowRect, (GUI.WindowFunction)DrawWindow, string.Empty);

            _gui?.DrawOverlays();
        }

        private void DrawWindow(int windowId)
        {
            DrawWindowBackdrop();

            _gui.UpdateGUI(_showSelector);
            if (!_gui.BeginGUI())
                return;

            _gui.BeginVerticalGroup(GUILayout.Width(_windowRect.width - 24f), GUILayout.Height(_windowRect.height - 14f));
            _gui.AddSpace(12f);
            DrawFilters();
            _gui.AddSpace(12f);

            _scroll = _gui.ScrollView(_scroll, DrawDemoList, GUILayout.ExpandWidth(true), GUILayout.Height(_windowRect.height - 208f));

            _gui.AddSpace(12f);
            DrawFooter();
            _gui.EndVerticalGroup();

            _gui.EndGUI();
            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, 42f));
        }

        private void DrawWindowBackdrop()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            Color previous = GUI.color;
            GUI.color = new Color(0.03f, 0.04f, 0.06f, 0.96f);
            GUI.DrawTexture(new Rect(0f, 0f, _windowRect.width, _windowRect.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.12f, 0.16f, 0.22f, 0.55f);
            GUI.DrawTexture(new Rect(0f, 0f, _windowRect.width, 86f), Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private void DrawFilters()
        {
            _gui.BeginHorizontalGroup();
            _searchQuery = _gui.Input(_searchQuery)
                .Id("demo_selector_search")
                .Placeholder("Search demos")
                .Appearance(_input)
                .Width(340f);

            _gui.EndHorizontalGroup();
        }

        private void DrawDemoList()
        {
            List<DemoEntry> visible = GetVisibleEntries();
            if (visible.Count == 0)
            {
                _gui.Card()
                    .Title("No demos found")
                    .Description("Try another search term.")
                    .Size(-1f, 110f)
                    .Appearance(_panel)
                    .Render();
                return;
            }

            for (int i = 0; i < visible.Count; i++)
            {
                DrawDemoRow(visible[i], i);
                if (i < visible.Count - 1)
                    _gui.AddSpace(10f);
            }
        }

        private void DrawDemoRow(DemoEntry entry, int index)
        {
            bool active = _currentDemoName == entry.Title;
            ComponentAppearance cardAppearance = active ? _activePanel : _panel;

            _gui.BeginCard(-1f, 112f, ControlVariant.Default, ControlSize.Default, cardAppearance);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();

                _gui.BeginVerticalGroup(GUILayout.Width(58f));
                _gui.Badge((index + 1).ToString("00")).Appearance(_pill).Render();
                _gui.AddSpace(10f);
                _gui.EndVerticalGroup();

                _gui.AddSpace(12f);

                _gui.BeginVerticalGroup();
                _gui.BeginHorizontalGroup();
                _gui.Heading(entry.Title);
                _gui.EndHorizontalGroup();
                _gui.Caption(entry.Description);
                _gui.AddSpace(8f);
                _gui.BeginHorizontalGroup();
                _gui.MutedLabel(entry.Id);
                _gui.EndHorizontalGroup();
                _gui.EndVerticalGroup();

                GUILayout.FlexibleSpace();

                _gui.BeginVerticalGroup(GUILayout.Width(146f));
                string launchText = active ? "Reload" : "Open Demo";
                if (_gui.Button(launchText, active ? ControlVariant.Secondary : ControlVariant.Default, ControlSize.Small, appearance: active ? _pill : _primary))
                    entry.Load();

                _gui.AddSpace(8f);
                if (active && _gui.Button("Close Current", ControlVariant.Outline, ControlSize.Small, appearance: _pill))
                    CloseCurrentDemo();
                _gui.EndVerticalGroup();

                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();
        }

        private void DrawFooter()
        {
            _gui.BeginHorizontalGroup();

            if (_gui.Button("Clear Filters", ControlVariant.Ghost, ControlSize.Small))
                _searchQuery = string.Empty;

            if (_currentDemo != null && _gui.Button("Unload Demo", ControlVariant.Outline, ControlSize.Small, appearance: _pill))
                CloseCurrentDemo();

            _gui.EndHorizontalGroup();
        }

        private List<DemoEntry> BuildEntries()
        {
            var entries = new List<DemoEntry>
            {
                new DemoEntry
                {
                    Id = nameof(FullDemo),
                    Title = "Full Demo",
                    Description = "Large multi-section showcase of controls, display, layout, data, and overlays.",
                    Load = () => LoadDemo<FullDemo>("Full Demo"),
                },
                new DemoEntry
                {
                    Id = nameof(ShadcnDocsHomeDemo),
                    Title = "Docs Home",
                    Description = "A shadcn docs-inspired dashboard composition.",
                    Load = () => LoadDemo<ShadcnDocsHomeDemo>("Docs Home"),
                },
                new DemoEntry
                {
                    Id = nameof(NovaOpsDemo),
                    Title = "Nova Ops",
                    Description = "Animated command-center interface with charts, controls, and generated textures.",
                    Load = () => LoadDemo<NovaOpsDemo>("Nova Ops"),
                },
                new DemoEntry
                {
                    Id = nameof(FullDemo_old),
                    Title = "Full Demo OLD",
                    Description = "Older all-in-one showcase kept for comparison.",
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
                    Description = "Capture stills and GIFs for component documentation.",
                    Load = () => LoadDemo<ScreenshotUtility>("Screenshot Utility"),
                }
            );
#endif
#endif

            return entries;
        }

        private List<DemoEntry> GetVisibleEntries()
        {
            var visible = new List<DemoEntry>();
            string query = _searchQuery?.Trim() ?? string.Empty;

            for (int i = 0; i < _entries.Count; i++)
            {
                DemoEntry entry = _entries[i];
                if (!string.IsNullOrEmpty(query) && !Matches(entry, query))
                    continue;

                visible.Add(entry);
            }

            return visible;
        }

        private static bool Matches(DemoEntry entry, string query)
        {
            return entry.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                || entry.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                || entry.Id.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void LoadDemo<T>(string displayName)
            where T : MonoBehaviour
        {
            if (_currentDemo != null)
            {
#if Showcase
#elif !Showcase
                Destroy(_currentDemo);
#endif
            }

            _currentDemo = new GameObject(typeof(T).Name);
            _currentDemo.AddComponent<T>();
            DontDestroyOnLoad(_currentDemo);
            _currentDemoName = displayName;

            _gui.Toast()
                .Title(displayName + " loaded")
                .Description("The demo window is now active.")
                .Variant(ToastVariant.Success)
                .Duration(2800f)
                .Render();

#if Showcase
#elif !Showcase
            _showSelector = false;
#endif
            Debug.Log($"Loaded {typeof(T).Name} demo");
        }

        private void CloseCurrentDemo()
        {
            if (_currentDemo == null)
                return;

            Destroy(_currentDemo);
            _currentDemo = null;
            _currentDemoName = "None";

            _gui.Toast()
                .Title("Demo unloaded")
                .Description("The active demo object was destroyed.")
                .Variant(ToastVariant.Info)
                .Duration(2400f)
                .Render();
        }

        private void BuildAppearances()
        {
            _panel = Surface("#111318cc", "#2b313f", 10f);
            _activePanel = Surface("#0e2630cc", "#67e8f9", 10f);
            _input = Surface("#08090bcc", "#3f3f46", 10f);
            _pill = Surface("#18181bcc", "#3f3f4699", 999f);
            _primary = Surface("#155e75", "#67e8f9", 10f);
        }

        private static ComponentAppearance Surface(string fill, string border, float radius)
        {
            return new ComponentAppearance
            {
                BackgroundColor = Theme.Hex(fill),
                BorderColor = Theme.Hex(border),
                BorderRadius = radius,
                BorderThickness = 1f,
            };
        }

        private void OnDestroy()
        {
            if (_currentDemo != null)
                Destroy(_currentDemo);

            _gui?.Cleanup();
        }
    }
}
