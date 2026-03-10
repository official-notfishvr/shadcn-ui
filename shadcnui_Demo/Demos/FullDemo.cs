using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Controls;
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
        private readonly string[] _tabs = { "Overview", "Inputs", "Display", "Layout", "Data", "Overlay" };

        private GUIHelper _gui;
        private Rect _windowRect = new Rect(24f, 24f, 1420f, 860f);
        private Vector2 _scroll;
        private int _activeTab;
        private Texture2D _sampleTexture;

        private float _uiScale = 1f;
        private float _fontSize = 14f;
        private string _search = "Squad loadout";
        private string _email = "operator@station.local";
        private string _password = "flat-ui-demo";
        private string _notes = "This demo is the reference surface for the rebuilt core.";
        private float _notesHeight = 110f;

        private bool _featureToggle = true;
        private bool _compactMode;
        private bool _alertsEnabled = true;
        private bool _confirmDelete;
        private float _volume = 0.72f;
        private float _danger = 35f;

        private int _selectIndex = 1;
        private bool _showSelect;
        private bool _showDropdown;
        private bool _showDialog;
        private int _nestedTabs;
        private int _verticalTabs;
        private int _navigationIndex = 1;

        private DateTime? _shipDate = DateTime.Today.AddDays(3);
        private DateTime? _meetingDate = DateTime.Today;
        private DateTime? _rangeStart = DateTime.Today;
        private DateTime? _rangeEnd = DateTime.Today.AddDays(5);

        private List<DropdownMenuItem> _dropdownItems;
        private List<DataTableColumn> _dataColumns;
        private List<DataTableRow> _dataRows;
        private List<ChartSeries> _barSeries;
        private List<ChartSeries> _lineSeries;
        private List<ChartSeries> _pieSeries;

        private readonly string[] _tableHeaders = { "Squad", "Status", "Ping", "Loadout" };
        private readonly string[,] _tableRows =
        {
            { "Alpha", "Ready", "18 ms", "Scout" },
            { "Bravo", "Queued", "24 ms", "Medic" },
            { "Charlie", "Ready", "31 ms", "Tank" },
        };

        private readonly string[] _selectItems = { "North Wing", "Transit Hub", "Orbital Dock", "Relay Tower" };

        private void Start()
        {
            _gui = new GUIHelper();
            _sampleTexture = CreateSampleTexture();

            RegisterThemes();
            BuildDropdownItems();
            BuildDataTable();
            BuildCharts();
        }

        private void OnGUI()
        {
            _windowRect = GUI.Window(104, _windowRect, DrawWindow, string.Empty);
            _gui.DrawOverlays();
        }

        private void OnDestroy()
        {
            _gui?.Cleanup();
            if (_sampleTexture != null)
                Destroy(_sampleTexture);
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

            _activeTab = _gui.Tabs(_tabs, _activeTab, DrawBody, maxLines: 1, position: TabPosition.Top, indicatorStyle: IndicatorStyle.Background);

            _gui.EndGUI();
            GUI.DragWindow();
        }

        private void DrawHeader()
        {
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup();
            _gui.Label("shadcn/ui Core Rebuild", ControlVariant.Default);
            _gui.MutedLabel("Flat, theme-driven IMGUI components for in-game tooling.");
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            _gui.BeginVerticalGroup(GUILayout.Width(360f));
            _gui.ThemeChangerWithPreview("full_demo_theme", 220f);
            _uiScale = _gui.LabeledSlider("UI Scale", _uiScale, 0.85f, 1.35f, 0.05f);
            _fontSize = _gui.LabeledSlider("Base Font", _fontSize, 12f, 18f, 1f);
            _gui.EndVerticalGroup();
            _gui.EndHorizontalGroup();
        }

        private void DrawBody()
        {
            _scroll = _gui.ScrollView(
                _scroll,
                () =>
                {
                    _gui.BeginVerticalGroup();
                    switch (_activeTab)
                    {
                        case 0:
                            DrawOverviewTab();
                            break;
                        case 1:
                            DrawInputsTab();
                            break;
                        case 2:
                            DrawDisplayTab();
                            break;
                        case 3:
                            DrawLayoutTab();
                            break;
                        case 4:
                            DrawDataTab();
                            break;
                        default:
                            DrawOverlayTab();
                            break;
                    }
                    _gui.EndVerticalGroup();
                },
                GUILayout.Height(_windowRect.height - 170f)
            );
        }

        private void DrawOverviewTab()
        {
            DrawSection(
                "Theme System",
                "Centralized metrics, colors, typography, and live scaling.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.SimpleCard($"Theme: {_gui.CurrentTheme.Name}\nAccent: {_gui.CurrentTheme.Accent}\nRadius: {_gui.CurrentTheme.Metrics.CornerRadius}", 280f, 110f);
                    _gui.SimpleCard($"Scale: {_uiScale:F2}\nFont: {_fontSize:F0}px\nBorder: {_gui.CurrentTheme.Metrics.BorderWidth:F1}px", 280f, 110f);
                    _gui.CardWithImage(_sampleTexture, "Flat Surface", "Minimal chrome", "Cards, buttons, and overlays all resolve through the shared style manager.", width: 300f, height: 210f);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Core Controls",
                "Representative variants and sizes.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Button("Primary");
                    _gui.Button("Secondary", ControlVariant.Secondary);
                    _gui.Button("Outline", ControlVariant.Outline);
                    _gui.Button("Ghost", ControlVariant.Ghost);
                    _gui.Button("Delete", ControlVariant.Destructive);
                    _gui.EndHorizontalGroup();

                    _gui.BeginHorizontalGroup();
                    _gui.Badge("Live");
                    _gui.Badge("Muted", ControlVariant.Secondary);
                    _gui.CountBadge(17);
                    _gui.StatusBadge("Sync", true);
                    _gui.ProgressBadge("Deploy", 0.64f);
                    _gui.AnimatedBadge("Pulse");
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Live Samples",
                "Every category in the rebuilt library is exercised in the tabs below.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.AvatarWithStatus(_sampleTexture, "OP", true);
                    _gui.Progress(0.58f, width: 240f);
                    _gui.LabeledSeparator("Session");
                    _gui.Label("Orbital Relay Queue", ControlVariant.Default);
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawInputsTab()
        {
            DrawSection(
                "Text Entry",
                "Inputs, password fields, and text areas all inherit the same theme metrics.",
                () =>
                {
                    _email = _gui.Input(
                        new InputConfig
                        {
                            Label = "Email",
                            Value = _email,
                            Placeholder = "name@game.local",
                            Width = 280,
                        }
                    );
                    _search = _gui.Input(_search, new IconConfig(_sampleTexture, IconPosition.Left) { Size = 14f, Spacing = 6f }, "Search command palette", width: 320);
                    _gui.PasswordField(320f, "Access Token", ref _password);
                    _notes = _gui.ResizableTextArea(_notes, ref _notesHeight, placeholder: "Write operator notes");
                }
            );

            DrawSection(
                "Choices",
                "Toggles, checkboxes, switches, select, slider, and dropdown menu.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _featureToggle = _gui.Toggle("Feature Flags", _featureToggle);
                    _compactMode = _gui.Checkbox("Compact HUD", _compactMode);
                    _alertsEnabled = _gui.Switch("Alerts", _alertsEnabled);
                    _gui.EndHorizontalGroup();

                    _volume = _gui.LabeledSlider("Effects Volume", _volume, 0f, 1f, 0.01f);
                    _danger = _gui.LabeledSlider("Danger Threshold", _danger, 0f, 100f, 5f);

                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Open Select", ControlVariant.Outline, ControlSize.Default))
                        _showSelect = true;
                    if (_showSelect)
                    {
                        _gui.OpenSelect("overview_select");
                        _showSelect = false;
                    }
                    _gui.Label($"Selected: {_selectItems[_selectIndex]}", ControlVariant.Muted);
                    _gui.EndHorizontalGroup();

                    if (_gui.IsSelectOpen())
                        _selectIndex = _gui.Select(new SelectConfig { Items = _selectItems, SelectedIndex = _selectIndex });

                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Open Dropdown", ControlVariant.Outline, ControlSize.Default))
                        _showDropdown = true;
                    _gui.Label("Context menu with headers and actions.", ControlVariant.Muted);
                    _gui.EndHorizontalGroup();

                    if (_showDropdown)
                        _gui.DropdownMenu(new DropdownMenuConfig(_dropdownItems));
                }
            );

            DrawSection(
                "Date Tools",
                "Calendar, single date picker, labeled picker, and range picker.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Calendar(new CalendarConfig());
                    _gui.BeginVerticalGroup(GUILayout.Width(320f));
                    _shipDate = _gui.DatePicker("Select ship date", _shipDate, "ship_date");
                    _meetingDate = _gui.LabeledDatePicker("Daily Sync", "Pick meeting date", _meetingDate, "meeting_date");
                    _rangeStart = _gui.DateRangePicker("Choose maintenance range", _rangeStart, _rangeEnd, "maintenance_range");
                    _gui.MutedLabel($"Ship: {_shipDate:MMM dd, yyyy}");
                    _gui.MutedLabel($"Meeting: {_meetingDate:MMM dd, yyyy}");
                    _gui.EndVerticalGroup();
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawDisplayTab()
        {
            DrawSection(
                "Labels, Badges, Avatars",
                "Display primitives and compact status surfaces.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Label("Default");
                    _gui.MutedLabel("Muted");
                    _gui.SecondaryLabel("Secondary");
                    _gui.DestructiveLabel("Destructive");
                    _gui.EndHorizontalGroup();

                    _gui.BeginHorizontalGroup();
                    _gui.Badge("Outline", ControlVariant.Outline);
                    _gui.BadgeWithIcon("Asset", _sampleTexture);
                    _gui.RoundedBadge("Metric", cornerRadius: 2f);
                    _gui.EndHorizontalGroup();

                    _gui.BeginHorizontalGroup();
                    _gui.Avatar(_sampleTexture, "OP", ControlSize.Default, AvatarShape.Square);
                    _gui.AvatarWithName(_sampleTexture, "JD", "Jordan Data", showNameBelow: true);
                    _gui.AvatarWithBorder(_sampleTexture, "AI", _gui.CurrentTheme.Accent);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Progress",
                "Linear, animated, labeled, and circular progress components.",
                () =>
                {
                    _gui.Progress(0.34f, width: 320f);
                    _gui.LabeledProgress("Streaming Assets", 0.67f, width: 320f);
                    _gui.AnimatedProgress("deploy_anim", Mathf.PingPong(Time.time * 0.15f, 1f), width: 320f);
                    _gui.BeginHorizontalGroup();
                    _gui.CircularProgress(0.28f, 54f);
                    _gui.CircularProgress(0.81f, 54f);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Charts",
                "Bar, line, and pie charts share the same chart component.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Chart(new ChartConfig(_barSeries, ChartType.Bar) { Size = new Vector2(320f, 200f) });
                    _gui.Chart(new ChartConfig(_lineSeries, ChartType.Line) { Size = new Vector2(320f, 200f) });
                    _gui.Chart(new ChartConfig(_pieSeries, ChartType.Pie) { Size = new Vector2(320f, 200f) });
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawLayoutTab()
        {
            DrawSection(
                "Cards and Separators",
                "Composable containers and spacing primitives.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Card("Mission Brief", "Flat container", "Cards can render title, description, footer actions, images, or avatars.", () => _gui.Button("Acknowledge", ControlVariant.Secondary, ControlSize.Small), 280f, 190f);
                    _gui.CardWithAvatar(_sampleTexture, "Squad Lead", "Rhea Vale", "Avatar cards use the same shared avatar and label styles.", width: 280f, height: 190f);
                    _gui.BeginCard(280f, 190f);
                    _gui.CardHeader(() => _gui.CardTitle("Manual Composition"));
                    _gui.CardContent(() =>
                    {
                        _gui.Label("Compose sections directly when you need finer layout control.", ControlVariant.Muted);
                        _gui.LabeledSeparator("Actions");
                        _gui.ButtonGroup(() =>
                        {
                            _gui.Button("Save", ControlVariant.Secondary, ControlSize.Small);
                            _gui.Button("Publish", ControlVariant.Default, ControlSize.Small);
                        });
                    });
                    _gui.EndCard();
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Tabs and Navigation",
                "Horizontal tabs, vertical tabs, and sidebar navigation.",
                () =>
                {
                    _nestedTabs = _gui.Tabs(new[] { "Overview", "Loadout", "Intel" }, _nestedTabs, () => _gui.SimpleCard($"Nested tab: {_nestedTabs}", 220f, 80f), maxLines: 1, position: TabPosition.Top, indicatorStyle: IndicatorStyle.Underline);

                    _verticalTabs = _gui.VerticalTabs(new[] { "Status", "Map", "Logs" }, _verticalTabs, () => _gui.SimpleCard($"Vertical tab: {_verticalTabs}", 220f, 80f), tabWidth: 120f, side: TabSide.Left);

                    _gui.BeginHorizontalGroup();
                    _navigationIndex = _gui.Sidebar(new[] { "Ops", "Feed", "Settings" }, _navigationIndex, new[] { "O", "F", "S" }, "U", width: 72f);
                    _gui.SimpleCard($"Sidebar selection: {_navigationIndex}", 220f, 80f);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Menu Bar",
                "Top-level commands with nested menu items.",
                () =>
                {
                    _gui.MenuBar(
                        new List<MenuBar.MenuItem>
                        {
                            new MenuBar.MenuItem(
                                "File",
                                subItems: new List<MenuBar.MenuItem>
                                {
                                    MenuBar.MenuItem.Header("Project"),
                                    new MenuBar.MenuItem("New Mission", () => _gui.ShowInfoToast("New Mission", "Started a new mission draft")),
                                    new MenuBar.MenuItem("Save Layout", () => _gui.ShowSuccessToast("Saved", "Layout persisted")),
                                    MenuBar.MenuItem.Separator(),
                                    new MenuBar.MenuItem("Close", () => _gui.ShowWarningToast("Closed", "Session closed")),
                                }
                            ),
                            new MenuBar.MenuItem("Edit", subItems: new List<MenuBar.MenuItem> { new MenuBar.MenuItem("Duplicate", () => _gui.ShowInfoToast("Duplicate", "Copied selection")), new MenuBar.MenuItem("Delete", () => _gui.ShowErrorToast("Delete", "Removed selection")) }),
                        }
                    );
                }
            );
        }

        private void DrawDataTab()
        {
            DrawSection(
                "Table",
                "Simple layout table and object-backed data table.",
                () =>
                {
                    _gui.Table(_tableHeaders, _tableRows, ControlVariant.Default, ControlSize.Default, GUILayout.Width(520f));
                    _gui.HorizontalSeparator();
                    _gui.DataTable("ops_table", _dataColumns, _dataRows, true, true, true, false, GUILayout.Width(840f));
                }
            );
        }

        private void DrawOverlayTab()
        {
            DrawSection(
                "Dialogs and Popovers",
                "Modal and lightweight overlay surfaces.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Open Dialog", ControlVariant.Default, ControlSize.Small))
                    {
                        _showDialog = true;
                        _gui.OpenDialog("mission_dialog");
                    }
                    if (_gui.Button("Open Popover", ControlVariant.Outline, ControlSize.Small))
                        _gui.OpenPopover("status_popover");
                    _gui.EndHorizontalGroup();

                    if (_showDialog)
                    {
                        _gui.Dialog(
                            new DialogConfig
                            {
                                Id = "mission_dialog",
                                Title = "Launch Sequence",
                                Description = "A themed dialog driven by the rebuilt style system.",
                                Width = 420f,
                                Height = 240f,
                                CloseOnOverlayClick = true,
                                Content = () =>
                                {
                                    _gui.Label("Validate the mission package before deployment.");
                                    _gui.Checkbox("Confirm destructive action", _confirmDelete, onToggle: value => _confirmDelete = value);
                                },
                                Footer = () =>
                                {
                                    if (_gui.Button("Cancel", ControlVariant.Ghost, ControlSize.Small))
                                    {
                                        _showDialog = false;
                                        _gui.CloseDialog();
                                    }
                                    if (_gui.Button("Deploy", ControlVariant.Destructive, ControlSize.Small))
                                    {
                                        _showDialog = false;
                                        _gui.CloseDialog();
                                        _gui.ShowSuccessToast("Deployment queued", "Launch window confirmed");
                                    }
                                },
                            }
                        );
                    }

                    if (_gui.IsPopoverOpen())
                    {
                        _gui.Popover(() =>
                        {
                            _gui.Label("Popover");
                            _gui.MutedLabel("Use this for transient supporting information.");
                            if (_gui.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                                _gui.ClosePopover();
                        });
                    }
                }
            );

            DrawSection(
                "Toasts and Tooltips",
                "Transient messaging and hover guidance.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Success", ControlVariant.Default, ControlSize.Small))
                        _gui.ShowSuccessToast("Saved", "Preset updated");
                    if (_gui.Button("Warning", ControlVariant.Outline, ControlSize.Small))
                        _gui.ShowWarningToast("Signal Weak", "Relay strength dropped below threshold");
                    if (_gui.Button("Error", ControlVariant.Destructive, ControlSize.Small))
                        _gui.ShowErrorToast("Sync Failed", "Unable to refresh mission state");
                    if (_gui.Button("Info", ControlVariant.Ghost, ControlSize.Small))
                        _gui.ShowInfoToast("Build Info", "The demo is using the new core");
                    _gui.EndHorizontalGroup();

                    _gui.WithTooltip("Tooltips route through the layer manager and the shared theme.", () => _gui.Button("Hover Me", ControlVariant.Secondary, ControlSize.Small));
                    _gui.MutedLabel($"Active toasts: {_gui.GetActiveToastCount()}");
                }
            );
        }

        private void DrawSection(string title, string summary, Action draw)
        {
            _gui.BeginCard(-1f, -1f);
            _gui.CardHeader(() =>
            {
                _gui.CardTitle(title);
                _gui.CardDescription(summary);
            });
            _gui.CardContent(draw);
            _gui.EndCard();
            _gui.AddSpace(12f);
        }

        private void RegisterThemes()
        {
            _gui.RegisterTheme(
                new Theme
                {
                    Name = "Signal Flat",
                    Base = Theme.Hex("#0c1016"),
                    Secondary = Theme.Hex("#151c24"),
                    Elevated = Theme.Hex("#1a222d"),
                    Text = Theme.Hex("#f5f7fa"),
                    Muted = Theme.Hex("#93a1b5"),
                    Border = Theme.Hex("#273241"),
                    Accent = Theme.Hex("#33c5ff"),
                    Destructive = Theme.Hex("#ef4444"),
                    Success = Theme.Hex("#22c55e"),
                    Warning = Theme.Hex("#f59e0b"),
                    Info = Theme.Hex("#33c5ff"),
                    Overlay = new Color(0f, 0f, 0f, 0.68f),
                    Shadow = new Color(0f, 0f, 0f, 0.34f),
                }
            );

            _gui.RegisterTheme(
                new Theme
                {
                    Name = "Sand Grid",
                    Base = Theme.Hex("#f2efe8"),
                    Secondary = Theme.Hex("#e5dfd3"),
                    Elevated = Theme.Hex("#faf8f2"),
                    Text = Theme.Hex("#201d18"),
                    Muted = Theme.Hex("#746b5f"),
                    Border = Theme.Hex("#c8bfaf"),
                    Accent = Theme.Hex("#cc7a00"),
                    Destructive = Theme.Hex("#bb3e03"),
                    Success = Theme.Hex("#588157"),
                    Warning = Theme.Hex("#d4a373"),
                    Info = Theme.Hex("#457b9d"),
                    Overlay = new Color(0f, 0f, 0f, 0.22f),
                    Shadow = new Color(0f, 0f, 0f, 0.18f),
                }
            );
        }

        private void BuildDropdownItems()
        {
            _dropdownItems = new List<DropdownMenuItem>
            {
                new DropdownMenuItem(DropdownMenuItemType.Header, "Actions"),
                new DropdownMenuItem(
                    DropdownMenuItemType.Item,
                    "Deploy",
                    () =>
                    {
                        _showDropdown = false;
                        _gui.ShowSuccessToast("Deploy", "Deployment command queued");
                    },
                    icon: _sampleTexture
                ),
                new DropdownMenuItem(
                    DropdownMenuItemType.Item,
                    "Duplicate",
                    () =>
                    {
                        _showDropdown = false;
                        _gui.ShowInfoToast("Duplicate", "Copied current preset");
                    }
                ),
                new DropdownMenuItem(DropdownMenuItemType.Separator),
                new DropdownMenuItem(
                    DropdownMenuItemType.Item,
                    "Archive",
                    () =>
                    {
                        _showDropdown = false;
                        _gui.ShowWarningToast("Archive", "Preset archived");
                    }
                ),
            };
        }

        private void BuildDataTable()
        {
            _dataColumns = new List<DataTableColumn>
            {
                new DataTableColumn("squad", "Squad", "squad", 140f),
                new DataTableColumn("lead", "Lead", "lead", 160f),
                new DataTableColumn("status", "Status", "status", 120f),
                new DataTableColumn("power", "Power", "power", 100f) { Alignment = TextAnchor.MiddleRight },
            };

            _dataRows = new List<DataTableRow>
            {
                new DataTableRow(
                    "1",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Alpha",
                        ["lead"] = "Rhea",
                        ["status"] = "Ready",
                        ["power"] = 93,
                    }
                ),
                new DataTableRow(
                    "2",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Bravo",
                        ["lead"] = "Noah",
                        ["status"] = "Queued",
                        ["power"] = 71,
                    }
                ),
                new DataTableRow(
                    "3",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Charlie",
                        ["lead"] = "Yara",
                        ["status"] = "Offline",
                        ["power"] = 14,
                    }
                ),
                new DataTableRow(
                    "4",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Delta",
                        ["lead"] = "Mina",
                        ["status"] = "Ready",
                        ["power"] = 88,
                    }
                ),
                new DataTableRow(
                    "5",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Echo",
                        ["lead"] = "Kai",
                        ["status"] = "Ready",
                        ["power"] = 64,
                    }
                ),
                new DataTableRow(
                    "6",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Foxtrot",
                        ["lead"] = "Iris",
                        ["status"] = "Queued",
                        ["power"] = 52,
                    }
                ),
            };
        }

        private void BuildCharts()
        {
            _barSeries = new List<ChartSeries>
            {
                new ChartSeries("build", "Build", Theme.Hex("#33c5ff"))
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
            };

            _pieSeries = new List<ChartSeries>
            {
                new ChartSeries("regions", "Regions")
                {
                    Data = new List<ChartDataPoint> { new("NA", 42, Theme.Hex("#33c5ff")), new("EU", 31, Theme.Hex("#22c55e")), new("APAC", 19, Theme.Hex("#f59e0b")), new("LATAM", 8, Theme.Hex("#ef4444")) },
                },
            };
        }

        private Texture2D CreateSampleTexture()
        {
            var texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            var dark = Theme.Hex("#1f2937");
            var light = Theme.Hex("#33c5ff");
            var pixels = new Color[32 * 32];

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    bool stripe = (x / 4 + y / 4) % 2 == 0;
                    pixels[y * 32 + x] = stripe ? light : dark;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}
