#define Showcase
using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
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
        private Rect _windowRect = new Rect(56f, 42f, 700f, 560f);
        private Vector2 _scroll;
        private string _searchQuery = string.Empty;
        private string _currentDemoName = "None";
        private bool _showSelector = true;
        private GameObject _currentDemo;
        private List<DemoEntry> _entries;

        private void Start()
        {
            _gui = new GUIHelper();
            _gui.SetTheme("Shadcn");
            _gui.SetFontSize(13);
            _entries = BuildEntries();
        }

        private void OnGUI()
        {
            if (_showSelector)
            {
                KeepWindowVisible();
                _windowRect = GUI.Window(100, _windowRect, (GUI.WindowFunction)DrawWindow, string.Empty);
            }

            _gui?.DrawOverlays();
        }

        private void KeepWindowVisible()
        {
            const float margin = 14f;
            _windowRect.width = Mathf.Min(_windowRect.width, Mathf.Max(540f, Screen.width - margin * 2f));
            _windowRect.height = Mathf.Min(_windowRect.height, Mathf.Max(400f, Screen.height - margin * 2f));
            _windowRect.x = Mathf.Clamp(_windowRect.x, margin, Mathf.Max(margin, Screen.width - _windowRect.width - margin));
            _windowRect.y = Mathf.Clamp(_windowRect.y, margin, Mathf.Max(margin, Screen.height - _windowRect.height - margin));
        }

        private void DrawWindow(int windowId)
        {
            _gui.UpdateGUI(_showSelector);
            if (!_gui.BeginGUI())
                return;

            _gui.BeginVerticalGroup(GUILayout.Width(_windowRect.width - 28f), GUILayout.Height(_windowRect.height - 18f));
            _gui.Heading("Demos");
            _gui.Caption("Choose an example to open.");
            _gui.AddSpace(10f);

            _searchQuery = _gui.Input(_searchQuery).Id("demo_selector_search").Placeholder("Search demos").Width(_windowRect.width - 56f);
            _gui.AddSpace(12f);

            List<DemoEntry> visible = GetVisibleEntries();
            _gui.BeginHorizontalGroup();
            _gui.Label(visible.Count + " demos", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            if (_currentDemo != null)
            {
                _gui.Caption("Open: " + _currentDemoName);
                _gui.AddSpace(10f);
                if (_gui.Button("Unload", ControlVariant.Outline, ControlSize.Small))
                    CloseCurrentDemo();
            }
            _gui.EndHorizontalGroup();
            _gui.AddSpace(8f);

            _scroll = _gui.ScrollView(_scroll, DrawDemoList, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            _gui.EndVerticalGroup();
            _gui.EndGUI();

            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, 38f));
        }

        private void DrawDemoList()
        {
            List<DemoEntry> visible = GetVisibleEntries();
            if (visible.Count == 0)
            {
                _gui.AddSpace(20f);
                _gui.Heading("No demos found");
                _gui.Caption("Try a different search.");
                return;
            }

            for (int i = 0; i < visible.Count; i++)
            {
                DemoEntry entry = visible[i];
                _gui.BeginHorizontalGroup(GUILayout.MinHeight(58f));
                _gui.BeginVerticalGroup(GUILayout.ExpandWidth(true));
                _gui.Label(entry.Title);
                _gui.Caption(entry.Description);
                _gui.MutedLabel(entry.Id);
                _gui.EndVerticalGroup();

                if (_gui.Button("Open", ControlVariant.Default, ControlSize.Small, options: GUILayout.Width(70f)))
                    entry.Load();
                _gui.EndHorizontalGroup();
                if (i < visible.Count - 1)
                    _gui.HorizontalSeparator();
            }
        }

        private List<DemoEntry> BuildEntries()
        {
            var entries = new List<DemoEntry>
            {
                new DemoEntry
                {
                    Id = nameof(FullDemo),
                    Title = "Full Demo",
                    Description = "Controls, display, layout, data, and overlays.",
                    Load = () => LoadDemo<FullDemo>("Full Demo"),
                },
                new DemoEntry
                {
                    Id = nameof(ShadcnDocsHomeDemo),
                    Title = "Docs Home",
                    Description = "A documentation-style composition using the library.",
                    Load = () => LoadDemo<ShadcnDocsHomeDemo>("Docs Home"),
                },
                new DemoEntry
                {
                    Id = nameof(FullDemo_old),
                    Title = "Full Demo OLD",
                    Description = "The previous all-in-one showcase for comparison.",
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
                    Description = "Capture stills and GIFs for documentation.",
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
                if (string.IsNullOrEmpty(query) || entry.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || entry.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || entry.Id.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    visible.Add(entry);
            }

            return visible;
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
            _gui.Toast().Title(displayName + " loaded").Description("The demo window is now active.").Variant(ToastVariant.Success).Duration(2800f).Render();

#if Showcase
#elif !Showcase
            _showSelector = false;
#endif
            Debug.Log("Loaded " + typeof(T).Name + " demo");
        }

        private void CloseCurrentDemo()
        {
            if (_currentDemo == null)
                return;

            Destroy(_currentDemo);
            _currentDemo = null;
            _currentDemoName = "None";
            _gui.Toast().Title("Demo unloaded").Description("The active demo object was destroyed.").Variant(ToastVariant.Info).Duration(2400f).Render();
        }

        private void OnDestroy()
        {
            if (_currentDemo != null)
                Destroy(_currentDemo);
            _gui?.Cleanup();
        }
    }
}
