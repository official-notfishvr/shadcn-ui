using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class FullDemo : MonoBehaviour
    {
        private readonly string[] _tabs = { "Overview", "Controls", "Inputs", "Display", "Layout", "Data", "Overlay" };
        private readonly string[] _priorityItems = { "Low", "Normal", "High", "Critical" };
        private readonly string[] _locationItems = { "North Wing", "Transit Hub", "Orbital Dock", "Relay Tower" };
        private readonly string[] _simpleHeaders = { "Squad", "Status", "Ping", "Role" };
        private readonly string[,] _simpleRows =
        {
            { "Alpha", "Ready", "18 ms", "Scout" },
            { "Bravo", "Queued", "24 ms", "Medic" },
            { "Charlie", "Ready", "31 ms", "Tank" },
            { "Delta", "Offline", "0 ms", "Support" },
            { "Echo", "Ready", "16 ms", "Recon" },
            { "Foxtrot", "Queued", "28 ms", "Engineer" },
        };

        private GUIHelper _gui;
        private Rect _windowRect = new(24f, 24f, 1460f, 900f);
        private Vector2 _scroll;
        private float _lastScrollViewportHeight;
        private float _lastScrollContentHeight;
        private int _activeTab;

        private readonly List<Texture2D> _generatedTextures = new();
        private Texture2D _sampleTexture;
        private Texture2D _coverTexture;

        private float _uiScale = 1f;
        private float _fontSize = 14f;
        private float _masterVolume = 0.72f;
        private float _dangerThreshold = 65f;
        private Vector2 _rangeValues = new(20f, 80f);

        private string _search = "orbital relay";
        private string _email = "operator@station.local";
        private string _password = "flat-ui-demo";
        private string _notes = "The new demo is intentionally smaller and only uses the builder/direct API that still exists.";

        private bool _featureToggle = true;
        private bool _alertsEnabled = true;
        private bool _compactMode;
        private bool _allowSpectators = true;
        private bool _showDialog;
        private bool _showPopover;
        private bool _previewToastsPrimed;

        private int _priorityIndex = 1;
        private int _locationIndex = 2;
        private int _navigationIndex = 1;
        private int _tablePage;

        private DateTime? _shipDate = DateTime.Today.AddDays(3);
        private DateTime? _meetingDate = DateTime.Today;

        private string _screenshotPreview = string.Empty;
        private bool _screenshotScrollOverrideActive;
        private float _screenshotScrollOverrideY;

        private List<DataTableColumn> _dataColumns;
        private List<DataTableRow> _dataRows;
        private List<ChartSeries> _barSeries;
        private List<ChartSeries> _lineSeries;
        private List<ChartSeries> _pieSeries;

        private const string MissionDialogId = "mission_dialog";
        private const string StatusPopoverId = "status_popover";
        private const string DataTableId = "ops_table";

        private void Start()
        {
            _gui = new GUIHelper();
            _sampleTexture = CreatePatternTexture(32, Theme.Hex("#0f172a"), Theme.Hex("#38bdf8"), 4);
            _coverTexture = CreatePatternTexture(96, Theme.Hex("#111827"), Theme.Hex("#22c55e"), 8);
            BuildDataTable();
            BuildCharts();
        }

        private void OnGUI()
        {
            _windowRect = GUI.Window(104, _windowRect, (GUI.WindowFunction)DrawWindow, string.Empty);
            _gui.DrawOverlays();
        }

        private void OnDestroy()
        {
            _gui?.Cleanup();
            foreach (var texture in _generatedTextures)
            {
                if (texture != null)
                    Destroy(texture);
            }
        }

        private void DrawWindow(int windowId)
        {
            _gui.UpdateGUI(true);
            _gui.SetUiScale(_uiScale);
            _gui.SetFontSize(Mathf.RoundToInt(_fontSize));

            if (!_gui.BeginGUI())
                return;

            DrawHeader();
            _gui.HorizontalSeparator();

            _activeTab = _gui.Tabs().Items(_tabs).SelectedIndex(_activeTab).Indicator(IndicatorStyle.Background).Content(DrawBody).Render();

            _gui.EndGUI();
            GUI.DragWindow();
        }

        private void DrawHeader()
        {
            _gui.BeginHorizontalGroup();

            _gui.BeginVerticalGroup();
            _gui.Label("shadcn/ui Full Demo").Large().Render();
            _gui.Label("Both demos now exercise the current builder/direct API instead of the removed legacy helper surface.").Muted().Render();
            _gui.BeginHorizontalGroup();
            _gui.Badge(_gui.CurrentTheme.Name).Secondary().Render();
            _gui.Badge($"{_tabs.Length} tabs").Outline().Render();
            _gui.Badge(_alertsEnabled ? "Alerts Enabled" : "Alerts Paused").StatusDot(_alertsEnabled).Render();
            _gui.EndHorizontalGroup();
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            _gui.BeginVerticalGroup(GUILayout.Width(360f));
            _gui.BeginHorizontalGroup();
            if (_gui.Button("Dark", ControlVariant.Outline, ControlSize.Small))
                _gui.SetTheme("Dark");
            if (_gui.Button("Light", ControlVariant.Outline, ControlSize.Small))
                _gui.SetTheme("Light");
            if (_gui.Button("Cyan", ControlVariant.Outline, ControlSize.Small))
                _gui.SetTheme("Cyan");
            _gui.EndHorizontalGroup();

            _gui.ThemeChanger().Id("full_demo_theme").Width(220f).ShowPreview().Render();
            _gui.FontChanger().Id("full_demo_font").Width(220f).ShowPreview().PreviewText("Control room typography sample").Render();
            _uiScale = _gui.Slider(_uiScale).Label("UI Scale").Range(0.85f, 1.35f).Step(0.05f).ShowValue().Format("F2").Render();
            _fontSize = _gui.Slider(_fontSize).Label("Base Font").Range(12f, 18f).Step(1f).ShowValue().Format("F0").Render();
            _gui.EndVerticalGroup();

            _gui.EndHorizontalGroup();
        }

        private void DrawBody()
        {
            if (_screenshotScrollOverrideActive)
            {
                _scroll = GUILayout.BeginScrollView(new Vector2(_scroll.x, Mathf.Max(0f, _screenshotScrollOverrideY)), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                DrawBodyContent();
                GUILayout.EndScrollView();
            }
            else
            {
                _scroll = _gui.ScrollView(_scroll, DrawBodyContent, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            }

            if (Event.current.type == EventType.Repaint)
            {
                Rect viewportRect = GUILayoutUtility.GetLastRect();
                _lastScrollViewportHeight = Mathf.Max(0f, viewportRect.height);
            }
        }

        private void DrawBodyContent()
        {
            switch (_activeTab)
            {
                case 0:
                    DrawOverviewTab();
                    break;
                case 1:
                    DrawControlsTab();
                    break;
                case 2:
                    DrawInputsTab();
                    break;
                case 3:
                    DrawDisplayTab();
                    break;
                case 4:
                    DrawLayoutTab();
                    break;
                case 5:
                    DrawDataTab();
                    break;
                case 6:
                    DrawOverlayTab();
                    break;
            }

            if (Event.current.type == EventType.Repaint)
            {
                Rect contentRect = GUILayoutUtility.GetLastRect();
                _lastScrollContentHeight = Mathf.Max(0f, contentRect.height);
            }
        }

        private void DrawOverviewTab()
        {
            DrawSection(
                "Mission Brief",
                () =>
                {
                    _gui.Card()
                        .Title("Orbital Relay")
                        .Subtitle("Current build surface")
                        .Description("The demos now show only the API we plan to keep.")
                        .Content("Builder calls are the primary path. Thin direct helpers remain for the common one-liners.")
                        .Image(_coverTexture)
                        .Size(420f, 260f)
                        .Render();
                }
            );

            DrawSection(
                "Quick Stats",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Card().Title("Deploys").Content("24 queued").Size(180f, 120f).Render();
                    _gui.Card().Title("Latency").Content("18 ms").Size(180f, 120f).Render();
                    _gui.Card().Title("Coverage").Content("91%").Size(180f, 120f).Render();
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawControlsTab()
        {
            DrawSection(
                "Buttons",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Button("Primary").Render();
                    _gui.Button("Secondary").Secondary().Render();
                    _gui.Button("Outline").Outline().Render();
                    _gui.Button("Ghost").Ghost().Render();
                    _gui.Button("Delete").Destructive().Render();
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Boolean Controls",
                () =>
                {
                    _featureToggle = _gui.Toggle("Feature Flag", _featureToggle).Render();
                    _alertsEnabled = _gui.Checkbox("Alert Routing", _alertsEnabled).Render();
                    _allowSpectators = _gui.Switch("Allow Spectators", _allowSpectators).Render();
                }
            );

            DrawSection(
                "Sliders",
                () =>
                {
                    _masterVolume = _gui.Slider(_masterVolume).Label("Master Volume").Range(0f, 1f).Step(0.05f).ShowValue().Render();
                    _dangerThreshold = _gui.Slider(_dangerThreshold).Label("Danger Threshold").Range(0f, 100f).Step(5f).ShowValue().Format("F0").Destructive().Render();
                    _rangeValues = _gui.RangeSlider(_rangeValues.x, _rangeValues.y).Label("Operational Window").Range(0f, 100f).Step(5f).ShowValue().Format("F0").Render();
                }
            );
        }

        private void DrawInputsTab()
        {
            DrawSection(
                "Input Fields",
                () =>
                {
                    _search = _gui.Input(_search).Label("Search").Placeholder("Find a squad").Render();
                    _email = _gui.Input(_email).Label("Email").Placeholder("operator@station.local").Render();
                    _password = _gui.Input(_password).Label("Password").Password().Render();
                }
            );

            DrawSection(
                "Select, Dropdown, Date",
                () =>
                {
                    _priorityIndex = _gui.Select().Label("Priority").Items(_priorityItems).SelectedIndex(_priorityIndex).Width(240f).Render();
                    _locationIndex = _gui.Select().Label("Location").Items(_locationItems).SelectedIndex(_locationIndex).Width(240f).Render();

                    _gui.DropdownMenu().Trigger(() => _screenshotPreview == "inputs_dropdown" || _gui.Button("Quick Actions", ControlVariant.Outline, ControlSize.Small)).Header("Actions").Item("Queue Deploy").Item("Run Diagnostics").Separator().Item("Archive").Render();

                    _meetingDate = _gui.DatePicker().Id("meeting_picker").Label("Meeting Date").Value(_meetingDate).Range(DateTime.Today, DateTime.Today.AddDays(30)).Render();
                    _shipDate = _gui.DatePicker().Id("ship_picker").Label("Ship Date").Value(_shipDate).Range(DateTime.Today, DateTime.Today.AddDays(45)).Render();
                }
            );

            DrawSection(
                "Text Area",
                () =>
                {
                    _notes = _gui.TextArea(_notes).Label("Notes").Placeholder("Write a short operational note").MinHeight(110f).ShowCharacterCount().Render();
                    if (_screenshotPreview == "inputs_select")
                        _gui.Badge("Select preview requested").Outline().Render();
                }
            );
        }

        private void DrawDisplayTab()
        {
            DrawSection(
                "Labels and Badges",
                () =>
                {
                    _gui.Label("Default label").Render();
                    _gui.Label("Muted helper copy").Muted().Render();
                    _gui.Badge("Online").StatusDot().Render();
                    _gui.Badge("42").Count(42).Outline().Render();
                    _gui.Badge("Build Sync").Progress(0.72f).Secondary().Render();
                }
            );

            DrawSection(
                "Avatar and Progress",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Avatar().Image(_sampleTexture).Fallback("AL").Name("Ava Lane").Online().Render();
                    _gui.Avatar().Fallback("BR").Shape(AvatarShape.Rounded).Border(Theme.Hex("#38bdf8")).Render();
                    _gui.EndHorizontalGroup();

                    _gui.Progress(0.64f).Label("Sync Status").WidthValue(420f).ShowPercentage().Render();
                }
            );

            DrawSection(
                "Charts",
                () =>
                {
                    _gui.Chart().Type(ChartType.Bar).Series(_barSeries.ToArray()).Size(520f, 240f).Render();
                    _gui.AddSpace(12f);
                    _gui.Chart().Type(ChartType.Line).Series(_lineSeries.ToArray()).Size(520f, 240f).Render();
                }
            );
        }

        private void DrawLayoutTab()
        {
            DrawSection(
                "Navigation",
                () =>
                {
                    _navigationIndex = _gui.Navigation().Logo("S").Width(110f).Items(new NavigationItem("overview", "Overview"), new NavigationItem("teams", "Teams"), new NavigationItem("logs", "Logs"), new NavigationItem("settings", "Settings")).SelectedIndex(_navigationIndex).Render();
                }
            );

            DrawSection(
                "Menu Bar",
                () =>
                {
                    _gui.MenuBar().Item("File", items => items.Item("New Run").Item("Duplicate").Separator().Item("Close")).Item("View", items => items.Item("Compact Mode").Item("Expanded Grid")).Item("Help", items => items.Item("API Surface").Item("Migration Notes")).Render();
                }
            );

            DrawSection(
                "Card Grid",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Card().Title("North Wing").Content("Traffic stable").Size(220f, 150f).Render();
                    _gui.Card().Title("Transit Hub").Content("2 alerts pending").Size(220f, 150f).Render();
                    _gui.Card().Title("Relay Tower").Content("Maintenance window").Size(220f, 150f).Render();
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawDataTab()
        {
            DrawSection(
                "Table",
                () =>
                {
                    _gui.Table().Headers(_simpleHeaders).Rows(_simpleRows).Page(_tablePage, 4).OnPage(page => _tablePage = page).Search(_search).OnSearch(value => _search = value).Render();
                }
            );

            DrawSection(
                "Data Table",
                () =>
                {
                    _gui.DataTable(DataTableId).Columns(_dataColumns).Rows(_dataRows).ShowPagination().ShowSearch().ShowSelection().ShowColumnToggle().Render();
                }
            );

            DrawSection(
                "Pie Breakdown",
                () =>
                {
                    _gui.Chart().Type(ChartType.Pie).Series(_pieSeries.ToArray()).Size(360f, 260f).Render();
                }
            );
        }

        private void DrawOverlayTab()
        {
            DrawSection(
                "Transient UI",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Open Dialog", ControlVariant.Default, ControlSize.Small))
                        _showDialog = true;
                    if (_gui.Button("Open Popover", ControlVariant.Outline, ControlSize.Small))
                        _showPopover = true;
                    if (_gui.Button("Show Toast", ControlVariant.Secondary, ControlSize.Small))
                        ShowToast("Status update", "Overlay example", ToastVariant.Info);
                    _gui.EndHorizontalGroup();
                }
            );

            _gui.Dialog(MissionDialogId)
                .ParentWindow(_windowRect)
                .Title("Mission Confirmation")
                .Description("This uses the new builder-style dialog API.")
                .Content(() => _gui.Label("Confirm the next deployment window.").Muted().Render())
                .Footer(() =>
                {
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Cancel", ControlVariant.Outline, ControlSize.Small))
                    {
                        _showDialog = false;
                        _gui.Dialog(MissionDialogId).Close();
                    }
                    if (_gui.Button("Confirm", ControlVariant.Default, ControlSize.Small))
                    {
                        _showDialog = false;
                        _gui.Dialog(MissionDialogId).Close();
                        ShowToast("Deploy queued", "Mission window approved", ToastVariant.Success);
                    }
                    _gui.EndHorizontalGroup();
                })
                .Render();

            _gui.Popover(StatusPopoverId)
                .Content(() =>
                {
                    _gui.Label("Popover content").Render();
                    _gui.Label("Anchored overlay state").Muted().Render();
                })
                .Render();

            if (_showDialog || _screenshotPreview == "overlay_dialog")
                _gui.Dialog(MissionDialogId).Open();

            if (_showPopover || _screenshotPreview == "overlay_popover")
                _gui.Popover(StatusPopoverId).Open();

            if (_screenshotPreview == "overlay_toasts" && !_previewToastsPrimed)
            {
                _previewToastsPrimed = true;
                ShowToast("Deploy queued", "Primary system channel", ToastVariant.Success);
                ShowToast("Watch relay traffic", "Secondary channel", ToastVariant.Warning);
            }
        }

        private void DrawSection(string title, Action content)
        {
            _gui.BeginVerticalGroup();
            _gui.Label(title).Large().Render();
            _gui.Label("Current API example").Muted().Render();
            _gui.AddSpace(8f);
            content?.Invoke();
            _gui.AddSpace(18f);
            _gui.HorizontalSeparator();
            _gui.AddSpace(18f);
            _gui.EndVerticalGroup();
        }

        private void ShowToast(string title, string description, ToastVariant variant)
        {
            _gui.Toast().Title(title).Description(description).Variant(variant).Position(ToastPosition.BottomRight).Duration(3200f).Render();
        }

        private void BuildDataTable()
        {
            _dataColumns = new List<DataTableColumn>
            {
                new("squad", "Squad", "squad", 140f),
                new("lead", "Lead", "lead", 160f),
                new("status", "Status", "status", 120f),
                new("power", "Power", "power", 100f) { Alignment = TextAnchor.MiddleRight, CellRenderer = value => $"{value}%" },
                new("region", "Region", "region", 120f),
            };

            _dataRows = new List<DataTableRow>
            {
                new(
                    "1",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Alpha",
                        ["lead"] = "Rhea",
                        ["status"] = "Ready",
                        ["power"] = 93,
                        ["region"] = "NA",
                    }
                ),
                new(
                    "2",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Bravo",
                        ["lead"] = "Noah",
                        ["status"] = "Queued",
                        ["power"] = 71,
                        ["region"] = "EU",
                    }
                ),
                new(
                    "3",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Charlie",
                        ["lead"] = "Yara",
                        ["status"] = "Offline",
                        ["power"] = 14,
                        ["region"] = "APAC",
                    }
                ),
                new(
                    "4",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Delta",
                        ["lead"] = "Mina",
                        ["status"] = "Ready",
                        ["power"] = 88,
                        ["region"] = "NA",
                    }
                ),
                new(
                    "5",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Echo",
                        ["lead"] = "Kai",
                        ["status"] = "Ready",
                        ["power"] = 64,
                        ["region"] = "LATAM",
                    }
                ),
                new(
                    "6",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Foxtrot",
                        ["lead"] = "Iris",
                        ["status"] = "Queued",
                        ["power"] = 52,
                        ["region"] = "EU",
                    }
                ),
            };
        }

        private void BuildCharts()
        {
            _barSeries = new List<ChartSeries>
            {
                new ChartSeries("build", "Build", Theme.Hex("#38bdf8"))
                {
                    Data = new List<ChartDataPoint> { new("Jan", 21), new("Feb", 35), new("Mar", 28), new("Apr", 44) },
                },
                new ChartSeries("ship", "Ship", Theme.Hex("#22c55e"))
                {
                    Data = new List<ChartDataPoint> { new("Jan", 18), new("Feb", 29), new("Mar", 33), new("Apr", 41) },
                },
            };

            _lineSeries = new List<ChartSeries>
            {
                new ChartSeries("latency", "Latency", Theme.Hex("#f59e0b"))
                {
                    Data = new List<ChartDataPoint> { new("00", 22), new("06", 30), new("12", 18), new("18", 26), new("24", 20) },
                },
                new ChartSeries("errors", "Errors", Theme.Hex("#ef4444"))
                {
                    Data = new List<ChartDataPoint> { new("00", 3), new("06", 4), new("12", 1), new("18", 5), new("24", 2) },
                },
            };

            _pieSeries = new List<ChartSeries>
            {
                new ChartSeries("regions", "Regions")
                {
                    Data = new List<ChartDataPoint> { new("NA", 42, Theme.Hex("#38bdf8")), new("EU", 31, Theme.Hex("#22c55e")), new("APAC", 19, Theme.Hex("#f59e0b")), new("LATAM", 8, Theme.Hex("#ef4444")) },
                },
            };
        }

        private Texture2D CreatePatternTexture(int size, Color dark, Color light, int blockSize)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool stripe = ((x / blockSize) + (y / blockSize)) % 2 == 0;
                    pixels[y * size + x] = stripe ? light : dark;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            _generatedTextures.Add(texture);
            return texture;
        }

        public void SetScreenshotPreview(float scrollY, string previewState)
        {
            _scroll = new Vector2(0f, Mathf.Max(0f, scrollY));
            _screenshotScrollOverrideActive = true;
            _screenshotScrollOverrideY = Mathf.Max(0f, scrollY);

            string nextPreview = previewState ?? string.Empty;
            if (!string.Equals(_screenshotPreview, nextPreview, StringComparison.Ordinal))
                _previewToastsPrimed = false;

            _screenshotPreview = nextPreview;
        }

        public void ClearScreenshotPreview()
        {
            _screenshotPreview = string.Empty;
            _screenshotScrollOverrideActive = false;
            _screenshotScrollOverrideY = 0f;
            _previewToastsPrimed = false;
            _showDialog = false;
            _showPopover = false;
        }

        public float GetScreenshotMaxScroll()
        {
            return Mathf.Max(0f, _lastScrollContentHeight - _lastScrollViewportHeight);
        }
    }
}
