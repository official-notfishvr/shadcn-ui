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
        private List<(Texture2D img, string fallback)> _avatarGroup;

        private float _uiScale = 1f;
        private float _fontSize = 14f;

        private string _search = "orbital relay";
        private string _email = "operator@station.local";
        private string _password = "flat-ui-demo";
        private string _passwordFieldValue = "token-Delta-19";
        private string _notes = "The full demo should feel like a polished control room instead of a dump of isolated widgets.";
        private string _outlineNotes = "Outline text area";
        private string _ghostNotes = "Ghost text area";
        private string _labeledNotes = "Labeled notes with character count.";
        private float _notesHeight = 110f;

        private bool _featureToggle = true;
        private bool _compactMode;
        private bool _alertsEnabled = true;
        private bool _allowSpectators = true;
        private bool _dangerMode;
        private bool _confirmDeploy;
        private bool _showDialog;
        private float _masterVolume = 0.72f;
        private float _musicVolume = 0.58f;
        private float _dangerThreshold = 65f;
        private float _steppedValue = 40f;
        private float _smallSlider = 0.28f;
        private float _largeSlider = 0.84f;
        private string _screenshotPreview = string.Empty;
        private bool _previewToastsPrimed;

        private int _priorityIndex = 1;
        private int _locationIndex = 2;
        private int _nestedTabIndex;
        private int _verticalTabIndex;
        private int _sidebarIndex = 1;
        private int _navigationIndex = 1;
        private int _tablePage;

        private string[] _closableTabs = { "Alpha", "Bravo", "Charlie", "Delta" };
        private bool[] _closableFlags = { true, true, true, true };

        private string _tableQuery = string.Empty;
        private string[,] _filteredTableRows;
        private int[] _sortColumns = Array.Empty<int>();
        private bool[] _sortAscending = Array.Empty<bool>();
        private bool[] _selectedTableRows;
        private float[] _resizableColumnWidths = { 130f, 130f, 110f, 140f };

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

        private const string MissionDialogId = "mission_dialog";
        private const string StatusPopoverId = "status_popover";
        private const string LocationSelectId = "location_select";
        private const string MeetingPickerId = "meeting_picker";
        private const string ShipPickerId = "ship_picker";
        private const string RangePickerId = "maintenance_range";
        private const string DataTableId = "ops_table";
        private const string PinnedToastId = "pinned_toast";

        private void Start()
        {
            _gui = new GUIHelper();

            _sampleTexture = CreatePatternTexture(32, Theme.Hex("#0f172a"), Theme.Hex("#38bdf8"), 4);
            _coverTexture = CreatePatternTexture(96, Theme.Hex("#111827"), Theme.Hex("#22c55e"), 8);
            _avatarGroup = new List<(Texture2D img, string fallback)>
            {
                (CreatePatternTexture(32, Theme.Hex("#172554"), Theme.Hex("#60a5fa"), 4), "AL"),
                (CreatePatternTexture(32, Theme.Hex("#3f1d2e"), Theme.Hex("#f472b6"), 4), "BR"),
                (CreatePatternTexture(32, Theme.Hex("#1f2937"), Theme.Hex("#f59e0b"), 4), "CY"),
                (CreatePatternTexture(32, Theme.Hex("#052e16"), Theme.Hex("#4ade80"), 4), "DT"),
                (CreatePatternTexture(32, Theme.Hex("#1e1b4b"), Theme.Hex("#a78bfa"), 4), "EX"),
            };

            _filteredTableRows = (string[,])_simpleRows.Clone();
            _selectedTableRows = new bool[_simpleRows.GetLength(0)];

            BuildDropdownItems();
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

            _activeTab = _gui.Tabs(_tabs, _activeTab, DrawBody, maxLines: 1, position: TabPosition.Top, indicatorStyle: IndicatorStyle.Background, overflowScroll: true);

            _gui.EndGUI();
            GUI.DragWindow();
        }

        private void DrawHeader()
        {
            _gui.BeginHorizontalGroup();

            _gui.BeginVerticalGroup();
            _gui.Heading("shadcn/ui Full Demo");
            _gui.Caption("A broader live surface for the C# GUI helper, rebuilt to show composition instead of isolated controls.");
            _gui.BeginHorizontalGroup();
            _gui.Badge(_gui.CurrentTheme.Name, ControlVariant.Secondary);
            _gui.CountBadge(_tabs.Length, ControlVariant.Outline);
            _gui.StatusBadge("Overlay Layer", true);
            _gui.EndHorizontalGroup();
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            _gui.BeginVerticalGroup(GUILayout.Width(410f));
            _gui.BeginHorizontalGroup();
            if (_gui.Button("Dark", ControlVariant.Outline, ControlSize.Small))
                _gui.SetTheme("Dark");
            if (_gui.Button("Light", ControlVariant.Outline, ControlSize.Small))
                _gui.SetTheme("Light");
            if (_gui.Button("Cyan", ControlVariant.Outline, ControlSize.Small))
                _gui.SetTheme("Cyan");
            _gui.EndHorizontalGroup();

            _gui.ThemeChanger(
                new ThemeChangerConfig
                {
                    Id = "full_demo_theme",
                    Width = 240f,
                    ShowPreview = true,
                }
            );
            _gui.FontChanger(
                new FontChangerConfig
                {
                    Id = "full_demo_font",
                    Width = 240f,
                    ShowPreview = true,
                }
            );
            _uiScale = _gui.Slider(
                new SliderConfig
                {
                    Label = "UI Scale",
                    Value = _uiScale,
                    MinValue = 0.85f,
                    MaxValue = 1.35f,
                    Step = 0.05f,
                    ShowValue = true,
                }
            );
            _fontSize = _gui.Slider(
                new SliderConfig
                {
                    Label = "Base Font",
                    Value = _fontSize,
                    MinValue = 12f,
                    MaxValue = 18f,
                    Step = 1f,
                    ShowValue = true,
                    ValueFormat = "F0",
                }
            );
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
                        default:
                            DrawOverlayTab();
                            break;
                    }

                    _gui.AddSpace(28f);
                    _gui.EndVerticalGroup();

                    if (Event.current.type == EventType.Repaint)
                    {
                        Rect contentRect = GUILayoutUtility.GetLastRect();
                        _lastScrollContentHeight = Mathf.Max(0f, contentRect.height);
                    }
                },
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true)
            );

            if (Event.current.type == EventType.Repaint)
            {
                Rect viewportRect = GUILayoutUtility.GetLastRect();
                _lastScrollViewportHeight = Mathf.Max(0f, viewportRect.height);
            }
        }

        private void DrawOverviewTab()
        {
            DrawSection(
                "Snapshot",
                "High-level status surfaces, stat cards, quick actions, and helper summaries.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.StatCard("Themes", _gui.GetThemeManager().Themes.Count.ToString(), _gui.CurrentTheme.Name, 220f);
                    _gui.StatCard("Toasts", _gui.GetActiveToastCount().ToString(), "active overlays", 220f);
                    _gui.StatCard("Data Rows", _dataRows.Count.ToString(), "selectable + searchable", 220f);
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);

                    _gui.BeginHorizontalGroup();
                    _gui.AvatarWithStatus(_sampleTexture, "UI", true, ControlSize.Large);
                    _gui.BeginVerticalGroup(GUILayout.Width(460f));
                    _gui.Heading("Mission Control Surface");
                    _gui.Caption("This pass keeps the demo compact while covering the broader helper API exposed by GUIHelper.");
                    _gui.LabeledProgress("Completion", 0.87f, width: 340f);
                    _gui.EndVerticalGroup();
                    GUILayout.FlexibleSpace();
                    _gui.AvatarGroup(_avatarGroup, ControlSize.Default, 4);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Quick Actions",
                "Shortcut buttons, tooltips, and summary helpers that make the rest of the demo feel like an app instead of a test harness.",
                () =>
                {
                    _gui.ButtonGroup(() =>
                    {
                        if (_gui.Button("Save Preset", ControlVariant.Secondary))
                            _gui.ShowSuccessToast("Preset Saved", "Window state committed");
                        if (_gui.Button("Broadcast", new IconConfig(_sampleTexture), ControlVariant.Default))
                            _gui.ShowInfoToast("Broadcast", "Sent current status to squad");
                        if (_gui.WithTooltip("Show a warning toast", new TooltipConfig { HoverDelaySeconds = 0.15f }, () => _gui.Button("Alert", ControlVariant.Outline)))
                            _gui.ShowWarningToast("Signal Weak", "Relay jitter crossed threshold");
                        if (_gui.Button("Dismiss Toasts", ControlVariant.Ghost))
                            _gui.DismissAllToasts();
                    });

                    _gui.AddSpace(10f);
                    _gui.KeyValueRow("Current Theme", _gui.CurrentTheme.Name);
                    _gui.KeyValueRow("Search Query", string.IsNullOrWhiteSpace(_search) ? "<empty>" : _search);
                    _gui.KeyValueRow("Ship Date", _shipDate?.ToString("MMM dd, yyyy") ?? "unset");
                    _gui.ErrorAlert("Overlay components render outside the window bounds using the shared layer manager.");
                }
            );

            DrawSection(
                "What This Demo Covers",
                "A shorter directory of the helper surface without returning to the old one-component-per-tab approach.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.BeginVerticalGroup(GUILayout.Width(340f));
                    _gui.SectionHeader("Controls");
                    _gui.CodeLabel("Buttons, toggles, checkboxes, switches, sliders, selects, dropdown menus");
                    _gui.SectionHeader("Display");
                    _gui.CodeLabel("Labels, badges, avatars, progress bars, charts");
                    _gui.EndVerticalGroup();

                    _gui.BeginVerticalGroup(GUILayout.Width(360f));
                    _gui.SectionHeader("Layout");
                    _gui.CodeLabel("Cards, separators, tabs, sidebars, navigation, menu bars");
                    _gui.SectionHeader("Data + Overlay");
                    _gui.CodeLabel("Tables, DataTable, dialogs, popovers, tooltips, toasts, date pickers");
                    _gui.EndVerticalGroup();
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawControlsTab()
        {
            DrawSection(
                "Buttons",
                "Variants, sizes, icon support, and disabled states.",
                () =>
                {
                    DrawVariantShowcase(variant => _gui.Button(variant.ToString(), variant, ControlSize.Small));
                    _gui.AddSpace(10f);
                    DrawSizeShowcase(size => _gui.Button(size.ToString(), ControlVariant.Default, size));

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    _gui.Button("Icon Left", new IconConfig(_sampleTexture, IconPosition.Left) { Size = 14f, Spacing = 6f }, ControlVariant.Outline);
                    _gui.Button("Icon Right", new IconConfig(_sampleTexture, IconPosition.Right) { Size = 14f, Spacing = 6f }, ControlVariant.Secondary);
                    _gui.Button("Disabled", ControlVariant.Ghost, ControlSize.Default, disabled: true);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Toggles, Checkboxes, Switches",
                "The shared styling surface stays consistent across stateful controls.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _featureToggle = _gui.Toggle("Feature Flags", _featureToggle);
                    _compactMode = _gui.Checkbox("Compact HUD", _compactMode);
                    _alertsEnabled = _gui.Switch("Alerts", _alertsEnabled);
                    _allowSpectators = _gui.Toggle("Spectators", new IconConfig(_sampleTexture), _allowSpectators);
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.Disabled(
                        true,
                        () =>
                        {
                            _gui.BeginHorizontalGroup();
                            _gui.Toggle("Disabled Toggle", true);
                            _gui.Checkbox("Disabled Checkbox", true);
                            _gui.Switch("Disabled Switch", false);
                            _gui.EndHorizontalGroup();
                        }
                    );
                }
            );

            DrawSection(
                "Sliders",
                "Simple, labeled, stepped, disabled, and size-based slider variants.",
                () =>
                {
                    _masterVolume = _gui.Slider(
                        new SliderConfig
                        {
                            Label = "Master Volume",
                            Value = _masterVolume,
                            MinValue = 0f,
                            MaxValue = 1f,
                            Step = 0.01f,
                            ShowValue = true,
                        }
                    );
                    _musicVolume = _gui.LabeledSlider("Music", _musicVolume, 0f, 1f, true, format: "F2");
                    _steppedValue = _gui.LabeledSlider("CPU Budget", _steppedValue, 0f, 100f, 5f, true, ControlVariant.Secondary, format: "F0");
                    _dangerThreshold = _gui.Slider(
                        new SliderConfig
                        {
                            Label = "Danger Threshold",
                            Value = _dangerThreshold,
                            MinValue = 0f,
                            MaxValue = 100f,
                            Step = 5f,
                            Variant = ControlVariant.Destructive,
                            ShowValue = true,
                            ValueFormat = "F0",
                        }
                    );

                    _gui.AddSpace(8f);
                    _smallSlider = _gui.Slider(
                        new SliderConfig
                        {
                            Value = _smallSlider,
                            MinValue = 0f,
                            MaxValue = 1f,
                            Size = ControlSize.Small,
                            Label = "Small",
                        }
                    );
                    _largeSlider = _gui.Slider(
                        new SliderConfig
                        {
                            Value = _largeSlider,
                            MinValue = 0f,
                            MaxValue = 1f,
                            Size = ControlSize.Large,
                            Label = "Large",
                        }
                    );
                    _gui.DisabledSlider(0.35f, 0f, 1f);
                }
            );
        }

        private void DrawInputsTab()
        {
            DrawSection(
                "Text Inputs",
                "Single-line inputs, icon support, password helpers, and section labels.",
                () =>
                {
                    _gui.SectionHeader("Operator Identity");
                    _email = _gui.Input(
                        new InputConfig
                        {
                            Label = "Email",
                            Value = _email,
                            Placeholder = "name@station.local",
                            Width = 320,
                        }
                    );
                    _search = _gui.Input(
                        new InputConfig
                        {
                            Label = "Search",
                            Value = _search,
                            Placeholder = "Search command palette",
                            Icon = new IconConfig(_sampleTexture, IconPosition.Left) { Size = 14f, Spacing = 6f },
                            Width = 340,
                        }
                    );
                    _password = _gui.Password(
                        new InputConfig
                        {
                            Label = "Access Token",
                            Value = _password,
                            Width = 340,
                        }
                    );

                    _gui.InputLabel("Legacy password helper");
                    _gui.PasswordField(280f, "Paste backup token", ref _passwordFieldValue);
                }
            );

            DrawSection(
                "Text Areas",
                "Default, outline, ghost, labeled, and resizable text areas.",
                () =>
                {
                    _notes = _gui.TextArea(_notes, placeholder: "Enter notes", minHeight: 80f);
                    _outlineNotes = _gui.OutlineTextArea(_outlineNotes, placeholder: "Outline variant", minHeight: 70f);
                    _ghostNotes = _gui.GhostTextArea(_ghostNotes, placeholder: "Ghost variant", minHeight: 70f);
                    _labeledNotes = _gui.LabeledTextArea("Mission Summary", _labeledNotes, placeholder: "Labeled text area", minHeight: 80f, maxLen: 180);
                    _notes = _gui.ResizableTextArea(_notes, ref _notesHeight, placeholder: "Resizable notes", minHeight: 70f, maxH: 220f);
                }
            );

            DrawSection(
                "Select, Dropdown, Date",
                "Choice-heavy controls with menu surfaces and calendar tooling.",
                () =>
                {
                    _priorityIndex = _gui.Select("Priority", _priorityItems, _priorityIndex);
                    var locationConfig = new SelectConfig
                    {
                        Id = LocationSelectId,
                        Label = "Location",
                        SelectedIndex = _locationIndex,
                        Width = 280f,
                        Options = Array.ConvertAll(_locationItems, item => new SelectOption(item.ToLowerInvariant(), item)),
                    };
                    _locationIndex = _gui.Select(locationConfig);
                    if (_screenshotPreview == "inputs_select" && Event.current.type == EventType.Repaint && !_gui.IsSelectOpen(LocationSelectId))
                        _gui.OpenSelect(locationConfig, GUILayoutUtility.GetLastRect());

                    _gui.AddSpace(6f);
                    _gui.MutedLabel(_gui.IsSelectOpen(LocationSelectId) ? "Location select is open." : "Location select is closed.");

                    _gui.AddSpace(10f);
                    var dropdownConfig = new DropdownMenuConfig(_dropdownItems) { Id = "full_demo_dropdown", Trigger = () => _gui.Button("Open Dropdown", ControlVariant.Outline) };
                    _gui.DropdownMenu(dropdownConfig);
                    if (_screenshotPreview == "inputs_dropdown" && Event.current.type == EventType.Repaint && !_gui.IsDropdownMenuOpen("full_demo_dropdown"))
                        _gui.OpenDropdownMenu(dropdownConfig, GUILayoutUtility.GetLastRect());

                    _gui.AddSpace(12f);
                    _gui.BeginHorizontalGroup();
                    _gui.Calendar(
                        new CalendarConfig
                        {
                            SelectedDate = _meetingDate,
                            DisabledDates = new List<DateTime> { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2) },
                            Ranges = new List<(DateTime Start, DateTime End)> { (DateTime.Today.AddDays(5), DateTime.Today.AddDays(8)) },
                        }
                    );

                    _gui.BeginVerticalGroup(GUILayout.Width(340f));
                    _shipDate = _gui.DatePicker("Ship date", _shipDate, DateTime.Today, DateTime.Today.AddDays(30), ShipPickerId);
                    _meetingDate = _gui.LabeledDatePicker("Daily Sync", "Meeting date", _meetingDate, MeetingPickerId);
                    _rangeStart = _gui.DateRangePicker("Maintenance range", _rangeStart, _rangeEnd, DateTime.Today, DateTime.Today.AddDays(45), RangePickerId);
                    _gui.MutedLabel($"Meeting picker open: {_gui.IsDatePickerOpen(MeetingPickerId)}");
                    if (_gui.Button("Close Meeting Picker", ControlVariant.Ghost, ControlSize.Small))
                        _gui.CloseDatePicker(MeetingPickerId);
                    _gui.EndVerticalGroup();
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawDisplayTab()
        {
            DrawSection(
                "Labels and Badges",
                "Typography helpers and compact status surfaces.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Heading("Heading");
                    _gui.MutedLabel("Muted");
                    _gui.SecondaryLabel("Secondary");
                    _gui.DestructiveLabel("Destructive");
                    _gui.CodeLabel("Code Label");
                    _gui.EndHorizontalGroup();

                    _gui.Caption("Caption helpers are useful for card descriptions and lower-contrast metadata.");
                    _gui.Label("Icon Label", new IconConfig(_sampleTexture, IconPosition.Left) { Size = 12f, Spacing = 4f });

                    _gui.AddSpace(10f);
                    DrawVariantShowcase(variant => _gui.Badge(variant.ToString(), variant, ControlSize.Small));

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    _gui.Badge("Asset", new IconConfig(_sampleTexture));
                    _gui.CountBadge(128, ControlVariant.Outline);
                    _gui.StatusBadge("Synced", true);
                    _gui.ProgressBadge("Deploy", 0.66f);
                    _gui.RoundedBadge("Rounded", cornerRadius: 16f);
                    _gui.AnimatedBadge("Pulse", "badge_pulse", ControlVariant.Secondary);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Avatars and Progress",
                "Avatar layouts and progress components share the same theme primitives.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Avatar(_sampleTexture, "UI", ControlSize.Default, AvatarShape.Circle);
                    _gui.Avatar(_sampleTexture, "SQ", ControlSize.Default, AvatarShape.Square);
                    _gui.AvatarWithStatus(_sampleTexture, "OP", true, ControlSize.Default);
                    _gui.AvatarWithName(_sampleTexture, "JD", "Jordan Data", showNameBelow: true);
                    _gui.AvatarWithBorder(_sampleTexture, "AI", _gui.CurrentTheme.Accent);
                    _gui.AvatarGroup(_avatarGroup, ControlSize.Default, 5);
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.Progress(0.34f, width: 320f);
                    _gui.LabeledProgress("Streaming Assets", 0.67f, width: 320f);
                    _gui.AnimatedProgress("deploy_anim", Mathf.PingPong(Time.time * 0.15f, 1f), width: 320f);
                    _gui.IndeterminateProgress("background_sync", width: 320f);

                    _gui.BeginHorizontalGroup();
                    _gui.CircularProgress(0.24f, 54f);
                    _gui.CircularProgress(0.79f, 54f);
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Charts",
                "Bar, line, and pie charts all route through the same chart helper.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Chart(new ChartConfig(_barSeries, ChartType.Bar) { Size = new Vector2(320f, 210f) });
                    _gui.Chart(new ChartConfig(_lineSeries, ChartType.Line) { Size = new Vector2(320f, 210f) });
                    _gui.Chart(new ChartConfig(_pieSeries, ChartType.Pie) { Size = new Vector2(320f, 210f) });
                    _gui.EndHorizontalGroup();
                }
            );
        }

        private void DrawLayoutTab()
        {
            DrawSection(
                "Cards",
                "Convenience cards, image cards, avatar cards, manual card composition, and stat surfaces.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _gui.Card("Mission Brief", "Convenience card", "Use the one-call helper when you want a title, copy, and compact footer action.", () => _gui.Button("Acknowledge", ControlVariant.Secondary, ControlSize.Small), 260f, 210f);
                    _gui.CardWithImage(_coverTexture, "Image Card", "Reusable content block", "Cards with images are useful for dashboards, launchers, and detail previews.", () => _gui.Button("Inspect", ControlVariant.Outline, ControlSize.Small), 260f, 210f);
                    _gui.CardWithAvatar(_sampleTexture, "Squad Lead", "Rhea Vale", "Avatar cards keep title, subtitle, and body aligned to the shared token system.", () => _gui.Button("Message", ControlVariant.Ghost, ControlSize.Small), 260f, 210f);
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    _gui.BeginCard(320f, 190f);
                    _gui.CardHeader(() =>
                    {
                        _gui.CardTitle("Manual Composition");
                        _gui.CardDescription("Use the lower-level card helpers when you need tighter control.");
                    });
                    _gui.CardContent(() =>
                    {
                        _gui.Label("Cards can nest separators, button groups, and helper text.");
                        _gui.LabeledSeparator("Actions");
                        _gui.ButtonGroup(() =>
                        {
                            _gui.Button("Save", ControlVariant.Secondary, ControlSize.Small);
                            _gui.Button("Publish", ControlVariant.Default, ControlSize.Small);
                        });
                    });
                    _gui.CardFooter(() => _gui.Caption("Footer content uses the same spacing system."));
                    _gui.EndCard();

                    _gui.BeginVerticalGroup();
                    _gui.SimpleCard("Simple card surfaces are useful when the content itself already carries the hierarchy.", 320f, 90f);
                    _gui.AddSpace(8f);
                    _gui.StatCard("FPS Budget", "16.6 ms", "-0.8 ms", 320f);
                    _gui.EndVerticalGroup();
                    _gui.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Separators and Tabs",
                "Spacing primitives, nested tabs, vertical tabs, and closable tabs.",
                () =>
                {
                    _gui.Label("Horizontal separator");
                    _gui.HorizontalSeparator();
                    _gui.LabeledSeparator("Navigation");
                    _gui.SeparatorWithSpacing(SeparatorOrientation.Horizontal, 4f, 8f);

                    _gui.BeginHorizontalGroup();
                    _gui.Label("Left");
                    _gui.VerticalSeparator(GUILayout.Height(28f));
                    _gui.Label("Right");
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(12f);
                    _nestedTabIndex = _gui.Tabs(new[] { "Overview", "Loadout", "Intel" }, _nestedTabIndex, () => _gui.SimpleCard($"Nested tab: {_nestedTabIndex}", 260f, 90f), maxLines: 1, position: TabPosition.Top, indicatorStyle: IndicatorStyle.Underline);

                    _verticalTabIndex = _gui.VerticalTabs(new[] { "Status", "Map", "Logs" }, _verticalTabIndex, () => _gui.SimpleCard($"Vertical tab: {_verticalTabIndex}", 240f, 90f), tabWidth: 120f, side: TabSide.Left, style: IndicatorStyle.Background);

                    _gui.AddSpace(10f);
                    if (_closableTabs.Length == 0)
                    {
                        if (_gui.Button("Reset Closable Tabs", ControlVariant.Outline, ControlSize.Small))
                        {
                            _closableTabs = new[] { "Alpha", "Bravo", "Charlie", "Delta" };
                            _closableFlags = new[] { true, true, true, true };
                        }
                    }
                    else
                    {
                        var closableIndex = Mathf.Clamp(_nestedTabIndex, 0, Mathf.Max(_closableTabs.Length - 1, 0));
                        closableIndex = _gui.ClosableTabs(ref _closableTabs, ref _closableFlags, closableIndex, () => _gui.SimpleCard($"Closable tab count: {_closableTabs.Length}", 260f, 90f));
                        _nestedTabIndex = Mathf.Clamp(closableIndex, 0, Mathf.Max(_closableTabs.Length - 1, 0));
                    }
                }
            );

            DrawSection(
                "Navigation and Menu Bar",
                "Sidebar shortcut navigation, config-based navigation, and nested menu commands.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    _sidebarIndex = _gui.Sidebar(new[] { "Ops", "Feed", "Settings" }, _sidebarIndex, new[] { "O", "F", "S" }, "U", width: 76f);

                    _navigationIndex = _gui.Navigation(
                        new NavigationConfig
                        {
                            Width = 92f,
                            LogoText = "MC",
                            IndicatorStyle = IndicatorStyle.Background,
                            IndicatorColor = _gui.CurrentTheme.Accent,
                            SelectedIndex = _navigationIndex,
                            Items = new[]
                            {
                                new NavigationItem("overview", "Overview", "OV"),
                                new NavigationItem("queue", "Queue", "Q"),
                                new NavigationItem("deploy", "Deploy", "DP") { IsDisabled = true },
                                new NavigationItem("logs", "Logs", "LG"),
                            },
                        }
                    );

                    _gui.BeginVerticalGroup(GUILayout.Width(380f));
                    _gui.SimpleCard($"Sidebar selection: {_sidebarIndex}", 360f, 70f);
                    _gui.AddSpace(6f);
                    _gui.SimpleCard($"Navigation selection: {_navigationIndex}", 360f, 70f);
                    _gui.EndVerticalGroup();
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(12f);
                    _gui.MenuBar(
                        new List<MenuBar.MenuItem>
                        {
                            new MenuBar.MenuItem(
                                "File",
                                subItems: new List<MenuBar.MenuItem>
                                {
                                    MenuBar.MenuItem.Header("Project"),
                                    new MenuBar.MenuItem("New Mission", () => _gui.ShowInfoToast("New Mission", "Started a new mission draft"), shortcut: "Ctrl+N"),
                                    new MenuBar.MenuItem("Save Layout", () => _gui.ShowSuccessToast("Saved", "Layout persisted"), shortcut: "Ctrl+S"),
                                    MenuBar.MenuItem.Separator(),
                                    new MenuBar.MenuItem("Close", () => _gui.ShowWarningToast("Closed", "Session closed")),
                                }
                            ),
                            new MenuBar.MenuItem("Edit", subItems: new List<MenuBar.MenuItem> { new MenuBar.MenuItem("Duplicate", () => _gui.ShowInfoToast("Duplicate", "Copied selection")), new MenuBar.MenuItem("Delete", () => _gui.ShowErrorToast("Delete", "Removed selection")) }),
                            new MenuBar.MenuItem("View", subItems: new List<MenuBar.MenuItem> { new MenuBar.MenuItem("Dark Theme", () => _gui.SetTheme("Dark")), new MenuBar.MenuItem("Light Theme", () => _gui.SetTheme("Light")), new MenuBar.MenuItem("Cyan Theme", () => _gui.SetTheme("Cyan")) }),
                        }
                    );
                }
            );
        }

        private void DrawDataTab()
        {
            DrawSection(
                "Basic and Custom Tables",
                "Standard table rendering plus a custom cell renderer path.",
                () =>
                {
                    _gui.Table(_simpleHeaders, _simpleRows, ControlVariant.Default, ControlSize.Default, GUILayout.Width(560f));
                    _gui.HorizontalSeparator();

                    object[,] customRows =
                    {
                        { "Build Queue", 18, "Healthy" },
                        { "Render Farm", 6, "Busy" },
                        { "Asset Sync", 2, "Blocked" },
                    };

                    _gui.CustomTable(
                        new[] { "System", "Workers", "State" },
                        customRows,
                        (value, row, col) =>
                        {
                            if (col == 1)
                                _gui.Label($"{value} nodes", ControlVariant.Secondary);
                            else if (col == 2 && string.Equals(value?.ToString(), "Blocked", StringComparison.OrdinalIgnoreCase))
                                _gui.DestructiveLabel(value?.ToString() ?? string.Empty);
                            else
                                _gui.Label(value?.ToString() ?? string.Empty);
                        }
                    );
                }
            );

            DrawSection(
                "Interactive Table Modes",
                "Sortable, selectable, paginated, searchable, and resizable table helpers.",
                () =>
                {
                    _gui.SortableTable(_simpleHeaders, _simpleRows, ref _sortColumns, ref _sortAscending);
                    _gui.MutedLabel(_sortColumns.Length == 0 ? "No active sort." : $"Sorted column: {_simpleHeaders[_sortColumns[0]]} ({(_sortAscending[0] ? "asc" : "desc")})");

                    _gui.AddSpace(12f);
                    _gui.SelectableTable(_simpleHeaders, _simpleRows, ref _selectedTableRows);
                    _gui.MutedLabel($"Selected rows: {CountSelectedRows(_selectedTableRows)}");

                    _gui.AddSpace(12f);
                    _gui.PaginatedTable(_simpleHeaders, _simpleRows, ref _tablePage, 2);
                    _gui.MutedLabel($"Current page: {_tablePage + 1}");
                }
            );

            DrawSection(
                "Search, Resize, DataTable",
                "Searchable and resizable tables plus the object-backed DataTable surface.",
                () =>
                {
                    _gui.SearchableTable(_simpleHeaders, _simpleRows, ref _tableQuery, ref _filteredTableRows);
                    _gui.MutedLabel($"Filtered rows: {GetRowCount(_filteredTableRows)}");

                    _gui.AddSpace(12f);
                    _gui.ResizableTable(_simpleHeaders, _simpleRows, ref _resizableColumnWidths);

                    _gui.AddSpace(12f);
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Page Size 3", ControlVariant.Outline, ControlSize.Small))
                        _gui.SetDataTablePageSize(DataTableId, 3);
                    if (_gui.Button("Page Size 6", ControlVariant.Outline, ControlSize.Small))
                        _gui.SetDataTablePageSize(DataTableId, 6);
                    if (_gui.Button("Clear Selection", ControlVariant.Ghost, ControlSize.Small))
                        _gui.ClearDataTableSelection(DataTableId);
                    _gui.EndHorizontalGroup();

                    _gui.DataTable(DataTableId, _dataColumns, _dataRows, true, true, true, true, GUILayout.Width(900f));

                    var state = _gui.GetDataTableState(DataTableId);
                    var selectedRows = _gui.GetSelectedDataTableRows(DataTableId);
                    if (state != null)
                    {
                        _gui.KeyValueRow("Filter", string.IsNullOrWhiteSpace(state.FilterText) ? "<none>" : state.FilterText);
                        _gui.KeyValueRow("Page Size", state.PageSize.ToString());
                        _gui.KeyValueRow("Selected", selectedRows.Count.ToString());
                        _gui.KeyValueRow("Sort", string.IsNullOrWhiteSpace(state.SortColumn) ? "<none>" : $"{state.SortColumn} ({(state.SortAscending ? "asc" : "desc")})");
                    }
                }
            );
        }

        private void DrawOverlayTab()
        {
            if (_screenshotPreview == "overlay_dialog")
                _showDialog = true;

            DrawSection(
                "Dialog and Popover",
                "Modal overlays and lightweight supporting surfaces.",
                () =>
                {
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Open Dialog", ControlVariant.Default, ControlSize.Small))
                    {
                        _showDialog = true;
                        _gui.OpenDialog(MissionDialogId);
                    }
                    if (_gui.Button("Open Popover", ControlVariant.Outline, ControlSize.Small))
                        _gui.OpenPopover(StatusPopoverId);
                    _gui.EndHorizontalGroup();

                    _gui.MutedLabel(_gui.IsPopoverOpen() ? $"Popover open at z-index {_gui.GetPopoverZIndex()}." : "Popover closed.");

                    if (_showDialog)
                    {
                        if (Event.current.type == EventType.Repaint)
                            _gui.OpenDialog(MissionDialogId);

                        _gui.Dialog(
                            new DialogConfig
                            {
                                Id = MissionDialogId,
                                Title = "Launch Sequence",
                                Description = "A themed dialog driven by the shared style and layer systems.",
                                Width = 440f,
                                Height = 250f,
                                CloseOnOverlayClick = true,
                                Content = () =>
                                {
                                    _gui.Label("Validate the mission package before deployment.");
                                    _confirmDeploy = _gui.Checkbox("Confirm destructive action", _confirmDeploy);
                                    _dangerMode = _gui.Switch("Danger mode", _dangerMode, ControlVariant.Destructive);
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
                            _gui.Label("Status Popover");
                            _gui.MutedLabel("Use this for supporting context without taking over the flow.");
                            _gui.KeyValueRow("Priority", _priorityItems[_priorityIndex]);
                            _gui.KeyValueRow("Location", _locationItems[_locationIndex]);
                            if (_gui.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                                _gui.ClosePopover();
                        });
                    }
                    else if (_screenshotPreview == "overlay_popover" && Event.current.type == EventType.Repaint)
                    {
                        _gui.OpenPopover(StatusPopoverId);
                    }
                }
            );

            DrawSection(
                "Tooltips and Toasts",
                "Tooltip wrappers and a broader set of toast behaviors.",
                () =>
                {
                    if (_screenshotPreview == "overlay_toasts" && !_previewToastsPrimed)
                    {
                        _previewToastsPrimed = true;
                        _gui.ShowSuccessToast("Saved", "Preset updated", 12000f);
                        _gui.ShowWarningToast("Signal Weak", "Relay strength dropped below threshold", 12000f);
                        _gui.ShowInfoToast("Build Info", "The demo is using the rebuilt showcase", 12000f);
                    }

                    _gui.BeginHorizontalGroup();
                    _gui.WithTooltip("Primary action", () => _gui.Button("Primary"));
                    _gui.WithTooltip("Secondary action", () => _gui.Button("Secondary", ControlVariant.Secondary));
                    _gui.WithTooltip("Outline action", new TooltipConfig { HoverDelaySeconds = 0.2f, MaxWidth = 180f }, () => _gui.Button("Outline", ControlVariant.Outline));
                    _gui.WithTooltip("Runtime status", () => _gui.StatusBadge("Live", true));
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Success", ControlVariant.Default, ControlSize.Small))
                        _gui.ShowSuccessToast("Saved", "Preset updated");
                    if (_gui.Button("Warning", ControlVariant.Outline, ControlSize.Small))
                        _gui.ShowWarningToast("Signal Weak", "Relay strength dropped below threshold");
                    if (_gui.Button("Error", ControlVariant.Destructive, ControlSize.Small))
                        _gui.ShowErrorToast("Sync Failed", "Unable to refresh mission state");
                    if (_gui.Button("Info", ControlVariant.Ghost, ControlSize.Small))
                        _gui.ShowInfoToast("Build Info", "The demo is using the rebuilt showcase");
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Action Toast", ControlVariant.Secondary, ControlSize.Small))
                    {
                        _gui.ShowToast(
                            new ToastConfig
                            {
                                Title = "Confirm Action",
                                Description = "Do you want to continue with the queued deployment?",
                                Variant = ToastVariant.Warning,
                                DurationMs = 9000f,
                                Position = ToastPosition.Center,
                                ActionLabel = "Confirm",
                                OnAction = () => _gui.ShowSuccessToast("Confirmed", "Deployment is continuing"),
                                ShowAccentBar = true,
                                ShowProgressBar = true,
                            }
                        );
                    }

                    if (_gui.Button("Pinned Toast", ControlVariant.Outline, ControlSize.Small))
                    {
                        _gui.ShowToast(
                            new ToastConfig
                            {
                                Id = PinnedToastId,
                                Title = "Pinned Status",
                                Description = "This toast can be dismissed by id.",
                                Variant = ToastVariant.Info,
                                Position = ToastPosition.TopRight,
                                DurationMs = 12000f,
                            }
                        );
                    }

                    if (_gui.Button("Dismiss Pinned", ControlVariant.Ghost, ControlSize.Small))
                        _gui.DismissToast(PinnedToastId);
                    if (_gui.Button("Dismiss All", ControlVariant.Ghost, ControlSize.Small))
                        _gui.DismissAllToasts();
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Top Left", ControlVariant.Outline, ControlSize.Small))
                        ShowPositionToast("Top Left", ToastPosition.TopLeft, ToastVariant.Default);
                    if (_gui.Button("Center", ControlVariant.Outline, ControlSize.Small))
                        ShowPositionToast("Center", ToastPosition.Center, ToastVariant.Warning);
                    if (_gui.Button("Bottom Right", ControlVariant.Outline, ControlSize.Small))
                        ShowPositionToast("Bottom Right", ToastPosition.BottomRight, ToastVariant.Success);
                    _gui.EndHorizontalGroup();

                    _gui.AddSpace(10f);
                    _gui.BeginHorizontalGroup();
                    if (_gui.Button("Stack Up", ControlVariant.Outline, ControlSize.Small))
                        ShowStackedToasts(ToastPosition.BottomRight, ToastStackDirection.Up);
                    if (_gui.Button("Stack Down", ControlVariant.Outline, ControlSize.Small))
                        ShowStackedToasts(ToastPosition.TopRight, ToastStackDirection.Down);
                    if (_gui.Button("Stack Left", ControlVariant.Outline, ControlSize.Small))
                        ShowStackedToasts(ToastPosition.CenterRight, ToastStackDirection.Left);
                    if (_gui.Button("Stack Right", ControlVariant.Outline, ControlSize.Small))
                        ShowStackedToasts(ToastPosition.CenterLeft, ToastStackDirection.Right);
                    _gui.EndHorizontalGroup();

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

        private void DrawVariantShowcase(Action<ControlVariant> draw)
        {
            _gui.BeginHorizontalGroup();
            foreach (ControlVariant variant in Enum.GetValues(typeof(ControlVariant)).Cast<ControlVariant>())
            {
                _gui.BeginVerticalGroup(GUILayout.Width(120f));
                _gui.Caption(variant.ToString());
                draw(variant);
                _gui.EndVerticalGroup();
            }
            _gui.EndHorizontalGroup();
        }

        private void DrawSizeShowcase(Action<ControlSize> draw)
        {
            _gui.BeginHorizontalGroup();
            foreach (ControlSize size in Enum.GetValues(typeof(ControlSize)).Cast<ControlSize>())
            {
                _gui.BeginVerticalGroup(GUILayout.Width(120f));
                _gui.Caption(size.ToString());
                draw(size);
                _gui.EndVerticalGroup();
            }
            _gui.EndHorizontalGroup();
        }

        private void BuildDropdownItems()
        {
            var broadcast = new DropdownMenuItem(DropdownMenuItemType.Item, "Broadcast", () => _gui.ShowInfoToast("Broadcast", "Sent current state"), _sampleTexture);
            broadcast.SubItems = new List<DropdownMenuItem>
            {
                new DropdownMenuItem(DropdownMenuItemType.Item, "Broadcast to Ops", () => _gui.ShowSuccessToast("Ops", "Message sent")),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Broadcast to Squad", () => _gui.ShowSuccessToast("Squad", "Message sent")),
            };

            _dropdownItems = new List<DropdownMenuItem>
            {
                new DropdownMenuItem(DropdownMenuItemType.Header, "Actions"),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Deploy", () => _gui.ShowSuccessToast("Deploy", "Deployment command queued"), _sampleTexture),
                broadcast,
                new DropdownMenuItem(DropdownMenuItemType.Item, "Duplicate", () => _gui.ShowInfoToast("Duplicate", "Copied current preset")),
                new DropdownMenuItem(DropdownMenuItemType.Separator),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Archive", () => _gui.ShowWarningToast("Archive", "Preset archived")),
            };
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
                new(
                    "7",
                    new Dictionary<string, object>
                    {
                        ["squad"] = "Gamma",
                        ["lead"] = "Juno",
                        ["status"] = "Ready",
                        ["power"] = 81,
                        ["region"] = "APAC",
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

        private void ShowPositionToast(string title, ToastPosition position, ToastVariant variant)
        {
            _gui.ShowToast(
                new ToastConfig
                {
                    Title = title,
                    Description = $"Positioned at {position}",
                    Variant = variant,
                    Position = position,
                    DurationMs = 3000f,
                }
            );
        }

        private void ShowStackedToasts(ToastPosition position, ToastStackDirection direction)
        {
            for (int i = 0; i < 3; i++)
            {
                _gui.ShowToast(
                    new ToastConfig
                    {
                        Title = $"Toast {i + 1}",
                        Description = $"Stack direction: {direction}",
                        Variant = (ToastVariant)(i % 4),
                        Position = position,
                        StackDirection = direction,
                        DurationMs = 5000f,
                    }
                );
            }
        }

        private int CountSelectedRows(bool[] rows)
        {
            if (rows == null)
                return 0;

            int selected = 0;
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i])
                    selected++;
            }

            return selected;
        }

        private int GetRowCount(string[,] rows)
        {
            return rows == null ? 0 : rows.GetLength(0);
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

            string nextPreview = previewState ?? string.Empty;
            if (!string.Equals(_screenshotPreview, nextPreview, StringComparison.Ordinal))
                _previewToastsPrimed = false;

            _screenshotPreview = nextPreview;
        }

        public void ClearScreenshotPreview()
        {
            _screenshotPreview = string.Empty;
            _previewToastsPrimed = false;
        }

        public float GetScreenshotMaxScroll()
        {
            return Mathf.Max(0f, _lastScrollContentHeight - _lastScrollViewportHeight);
        }
    }
}
