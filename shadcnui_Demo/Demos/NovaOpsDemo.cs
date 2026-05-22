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
    public class NovaOpsDemo : MonoBehaviour
    {
        private GUIHelper _gui;
        private Rect _windowRect = new Rect(8f, 24f, 1420f, 760f);
        private Vector2 _activityScroll;
        private Vector2 _crewScroll;

        private int _navIndex;
        private int _tabIndex;
        private int _priorityIndex = 1;
        private int _regionIndex;
        private bool _autopilot = true;
        private bool _silentMode;
        private bool _shieldSync = true;
        private bool _thermalLimit = true;
        private float _engineLoad = 72f;
        private float _signalFocus = 58f;
        private float _scanLow = 18f;
        private float _scanHigh = 84f;
        private string _search = string.Empty;
        private string _dispatchNote = "Route supply drone through canyon relay, then hold for verification.";

        private Texture2D _heroTexture;
        private Texture2D _miniMapTexture;
        private readonly List<Texture2D> _generatedTextures = new();
        private List<ChartSeries> _energySeries;
        private List<ChartSeries> _trafficSeries;
        private List<DataTableColumn> _columns;
        private List<DataTableRow> _rows;

        private ComponentAppearance _panel;
        private ComponentAppearance _panelStrong;
        private ComponentAppearance _panelSoft;
        private ComponentAppearance _glass;
        private ComponentAppearance _accent;
        private ComponentAppearance _accentAlt;
        private ComponentAppearance _danger;
        private ComponentAppearance _pill;
        private ComponentAppearance _input;

        private readonly string[] _regions = { "Aster Gate", "Low Orbit", "Deep Array", "Harbor 7" };
        private readonly string[] _priorities = { "Observe", "Stabilize", "Intercept", "Evacuate" };

        private void Start()
        {
            _gui = new GUIHelper();
            _gui.SetTheme("Zinc");
            _gui.SetFontSize(13);

            BuildAppearances();
            BuildTextures();
            BuildCharts();
            BuildTable();
        }

        private void OnDestroy()
        {
            _gui?.Cleanup();
            for (int i = 0; i < _generatedTextures.Count; i++)
            {
                if (_generatedTextures[i] != null)
                    Destroy(_generatedTextures[i]);
            }
        }

        private void OnGUI()
        {
            _windowRect = GUI.Window(611, _windowRect, (GUI.WindowFunction)DrawWindow, string.Empty);
            _gui?.DrawOverlays();
        }

        private void DrawWindow(int id)
        {
            DrawBackdrop();

            _gui.UpdateGUI(true);
            if (!_gui.BeginGUI())
                return;

            _gui.BeginHorizontalGroup(GUILayout.Width(_windowRect.width - 26f), GUILayout.Height(_windowRect.height - 18f));
            DrawSidebar();
            _gui.AddSpace(14f);

            _gui.BeginVerticalGroup(GUILayout.ExpandWidth(true));
            DrawTopBar();
            _gui.AddSpace(12f);
            DrawHero();
            _gui.AddSpace(14f);
            DrawMainContent();
            _gui.EndVerticalGroup();

            _gui.EndHorizontalGroup();

            DrawCommandDialog();

            _gui.EndGUI();
            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, 34f));
        }

        private void DrawBackdrop()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            GUI.DrawTexture(new Rect(0f, 0f, _windowRect.width, _windowRect.height), _heroTexture, ScaleMode.StretchToFill);
            var previous = GUI.color;
            GUI.color = new Color(0.03f, 0.04f, 0.05f, 0.82f);
            GUI.DrawTexture(new Rect(0f, 0f, _windowRect.width, _windowRect.height), Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private void DrawSidebar()
        {
            _navIndex = _gui.Navigation()
                .Logo("NO")
                .Width(148f)
                .Indicator(IndicatorStyle.Border)
                .IndicatorColor(Theme.Hex("#67e8f9"))
                .Items(
                    new NavigationItem("overview", "Overview"),
                    new NavigationItem("fleet", "Fleet"),
                    new NavigationItem("signals", "Signals"),
                    new NavigationItem("storage", "Storage"),
                    new NavigationItem("settings", "Settings")
                )
                .SelectedIndex(_navIndex)
                .OnChange(HandleNavigation)
                .Appearance(_glass);
        }

        private void DrawTopBar()
        {
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup();
            _gui.Heading("Nova Ops Console");
            _gui.Caption("Live IMGUI control surface for crew routing, orbital telemetry, and launch readiness.");
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            _search = _gui.Input(_search).Placeholder("Search event stream").Appearance(_input).Width(220f);
            _gui.AddSpace(8f);
            _regionIndex = _gui.Select().Id("nova_region").Items(_regions).SelectedIndex(_regionIndex).Width(150f).MaxHeight(180f).Appearance(_input);
            _gui.AddSpace(8f);

            if (_gui.Button("Pulse", ControlVariant.Outline, ControlSize.Small, appearance: _pill))
            {
                _gui.Toast().Title("Pulse sent").Description("All active relays acknowledged the command.").Variant(ToastVariant.Success).Duration(3400f).Render();
            }
            _gui.EndHorizontalGroup();
        }

        private void DrawHero()
        {
            _gui.BeginCard(-1f, 166f, ControlVariant.Default, ControlSize.Default, _panelStrong);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                _gui.BeginVerticalGroup(GUILayout.Width(392f));
                _gui.Badge("REALTIME").StatusDot().Appearance(_accent).Render();
                _gui.AddSpace(12f);
                _gui.Heading("Command mesh is stable");
                _gui.Caption("Telemetry loop " + DateTime.Now.ToString("HH:mm:ss") + " with " + Mathf.RoundToInt(_engineLoad) + "% engine load.");
                _gui.AddSpace(14f);
                _gui.BeginHorizontalGroup();
                if (_gui.Button("Open Launch Brief", ControlVariant.Default, ControlSize.Small, appearance: _accent))
                    _gui.Dialog("nova_launch_brief").Open();
                _gui.AddSpace(8f);
                if (_gui.Button("Route Crew", ControlVariant.Outline, ControlSize.Small, appearance: _pill))
                    _tabIndex = 1;
                _gui.EndHorizontalGroup();
                _gui.EndVerticalGroup();

                GUILayout.FlexibleSpace();
                DrawMetric("Uplink", AnimatedValue(91f, 5f, 0.8f), "#67e8f9");
                DrawMetric("Fuel", AnimatedValue(68f, 8f, 1.7f), "#bef264");
                DrawMetric("Risk", AnimatedValue(14f, 4f, 2.5f), "#fda4af");
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();
        }

        private void DrawMainContent()
        {
            _gui.BeginHorizontalGroup();

            _gui.BeginVerticalGroup(GUILayout.Width(780f));
            _tabIndex = _gui.Tabs()
                .Items("Telemetry", "Crew", "Dispatch")
                .SelectedIndex(_tabIndex)
                .TabWidth(128f)
                .Content(DrawSelectedTab);
            _gui.EndVerticalGroup();

            _gui.AddSpace(14f);
            DrawRightRail();

            _gui.EndHorizontalGroup();
        }

        private void HandleNavigation(int index)
        {
            _tabIndex = index switch
            {
                1 => 1,
                2 => 0,
                3 => 2,
                4 => 2,
                _ => 0,
            };
        }

        private void DrawSelectedTab()
        {
            _gui.AddSpace(12f);
            if (_tabIndex == 0)
                DrawTelemetryTab();
            else if (_tabIndex == 1)
                DrawCrewTab();
            else
                DrawDispatchTab();
        }

        private void DrawTelemetryTab()
        {
            _gui.BeginHorizontalGroup();
            DrawChartCard("Power Flow", "Generator output and battery recovery.", ChartType.Line, _energySeries, 374f, 248f);
            _gui.AddSpace(12f);
            DrawChartCard("Relay Traffic", "Packet volume by relay cluster.", ChartType.Bar, _trafficSeries, 374f, 248f);
            _gui.EndHorizontalGroup();

            _gui.AddSpace(12f);
            _gui.BeginHorizontalGroup();
            DrawSystemCard("Orbital Relay", "Synced", 0.86f, "#67e8f9");
            _gui.AddSpace(12f);
            DrawSystemCard("Dock Array", "Warming", 0.62f, "#fbbf24");
            _gui.AddSpace(12f);
            DrawSystemCard("Hangar Bay", "Ready", 0.94f, "#86efac");
            _gui.EndHorizontalGroup();
        }

        private void DrawCrewTab()
        {
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup(GUILayout.Width(476f));
            _gui.BeginCard(476f, 320f, ControlVariant.Default, ControlSize.Default, _panel);
            _gui.CardHeader(() =>
            {
                _gui.Heading("Active Flight Teams");
                _gui.Caption("Sortable roster with selection and column controls.");
            });
            _gui.CardContent(() =>
            {
                _gui.DataTable("nova_crew").Columns(_columns).Rows(_rows).ShowSearch().ShowSelection().ShowColumnToggle().Render();
            });
            _gui.EndCard();
            _gui.EndVerticalGroup();

            _gui.AddSpace(12f);
            _gui.BeginVerticalGroup(GUILayout.Width(288f));
            _crewScroll = _gui.ScrollView(_crewScroll, DrawCrewCards, GUILayout.Height(320f), GUILayout.Width(288f));
            _gui.EndVerticalGroup();
            _gui.EndHorizontalGroup();
        }

        private void DrawDispatchTab()
        {
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup(GUILayout.Width(380f));
            _gui.BeginCard(380f, 324f, ControlVariant.Default, ControlSize.Default, _panel);
            _gui.CardHeader(() =>
            {
                _gui.Heading("Dispatch Composer");
                _gui.Caption("Compose a field instruction and tune delivery behavior.");
            });
            _gui.CardContent(() =>
            {
                _priorityIndex = _gui.Select().Id("nova_priority").Label("Priority").Items(_priorities).SelectedIndex(_priorityIndex).Width(240f).Appearance(_input);
                _gui.AddSpace(12f);
                _dispatchNote = _gui.TextArea(_dispatchNote).Label("Instruction").MinHeight(92f).MaxLength(180).Appearance(_input);
                _gui.AddSpace(10f);
                _autopilot = _gui.Switch("Autopilot confirmation", _autopilot);
                _silentMode = _gui.Switch("Silent delivery", _silentMode);
            });
            _gui.CardFooter(() =>
            {
                if (_gui.Button("Send Dispatch", ControlVariant.Default, ControlSize.Small, appearance: _accent))
                    _gui.Toast().Title("Dispatch queued").Description(_priorities[_priorityIndex] + " command routed to " + _regions[_regionIndex] + ".").Variant(ToastVariant.Info).Render();
                _gui.Button("Save Draft", ControlVariant.Outline, ControlSize.Small, appearance: _pill);
            });
            _gui.EndCard();
            _gui.EndVerticalGroup();

            _gui.AddSpace(12f);
            _gui.BeginVerticalGroup(GUILayout.Width(380f));
            _gui.BeginCard(380f, 324f, ControlVariant.Default, ControlSize.Default, _panel);
            _gui.CardHeader(() =>
            {
                _gui.Heading("Scan Envelope");
                _gui.Caption("Live threshold controls with range slider feedback.");
            });
            _gui.CardContent(() =>
            {
                _engineLoad = _gui.Slider(_engineLoad).Label("Engine Load").Range(20f, 100f).Step(1f).ShowValue().Appearance(_accentAlt);
                _signalFocus = _gui.Slider(_signalFocus).Label("Signal Focus").Range(0f, 100f).Step(1f).ShowValue().Appearance(_accent);
                Vector2 scan = _gui.RangeSlider(_scanLow, _scanHigh).Label("Scan Band").Range(0f, 100f).Step(1f).ShowValue();
                _scanLow = scan.x;
                _scanHigh = scan.y;
                _gui.AddSpace(14f);
                _shieldSync = _gui.Checkbox("Mirror shield envelope", _shieldSync);
                _thermalLimit = _gui.Checkbox("Respect thermal limit", _thermalLimit);
            });
            _gui.EndCard();
            _gui.EndVerticalGroup();
            _gui.EndHorizontalGroup();
        }

        private void DrawRightRail()
        {
            _gui.BeginVerticalGroup(GUILayout.Width(350f));

            _gui.BeginCard(350f, 230f, ControlVariant.Default, ControlSize.Default, _panel);
            _gui.CardHeader(() =>
            {
                _gui.Heading("Sector Map");
                _gui.Caption("Generated texture preview with relay markers.");
            });
            _gui.CardContent(() =>
            {
                GUILayout.Label(_miniMapTexture, GUILayout.Width(304f), GUILayout.Height(96f));
                _gui.AddSpace(10f);
                _gui.BeginHorizontalGroup();
                _gui.StatusBadge("Gate open", true);
                _gui.AddSpace(8f);
                _gui.Badge("3 relays").Secondary().Render();
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();

            _gui.AddSpace(12f);
            _gui.BeginCard(350f, 320f, ControlVariant.Default, ControlSize.Default, _panelSoft);
            _gui.CardHeader(() =>
            {
                _gui.Heading("Activity Stream");
                _gui.Caption("Hover items for detail, or clear the queue.");
            });
            _gui.CardContent(() =>
            {
                _activityScroll = _gui.ScrollView(_activityScroll, DrawActivity, GUILayout.Height(198f));
            });
            _gui.CardFooter(() =>
            {
                if (_gui.Button("Acknowledge All", ControlVariant.Outline, ControlSize.Small, appearance: _pill))
                    _gui.Toast().Title("Queue acknowledged").Description("Activity stream marked as reviewed.").Variant(ToastVariant.Success).Render();
            });
            _gui.EndCard();

            _gui.EndVerticalGroup();
        }

        private void DrawMetric(string title, float value, string color)
        {
            _gui.BeginCard(124f, 112f, ControlVariant.Default, ControlSize.Default, Surface("#111827cc", color + "66", 12f));
            _gui.CardContent(() =>
            {
                _gui.Caption(title);
                _gui.Heading(Mathf.RoundToInt(value) + "%");
                _gui.Progress(value / 100f).WidthValue(92f).HeightValue(6f).Appearance(Surface(color + "33", color, 999f)).Render();
            });
            _gui.EndCard();
            _gui.AddSpace(10f);
        }

        private void DrawChartCard(string title, string caption, ChartType type, List<ChartSeries> series, float width, float height)
        {
            _gui.BeginCard(width, height, ControlVariant.Default, ControlSize.Default, _panel);
            _gui.CardHeader(() =>
            {
                _gui.Heading(title);
                _gui.Caption(caption);
            });
            _gui.CardContent(() =>
            {
                _gui.Chart().Type(type).Series(series.ToArray()).Size(width - 34f, height - 86f).Render();
            });
            _gui.EndCard();
        }

        private void DrawSystemCard(string title, string status, float progress, string color)
        {
            _gui.BeginCard(252f, 108f, ControlVariant.Default, ControlSize.Default, _glass);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                _gui.BeginVerticalGroup();
                _gui.Label(title);
                _gui.Caption(status);
                _gui.EndVerticalGroup();
                GUILayout.FlexibleSpace();
                _gui.Badge(Mathf.RoundToInt(progress * 100f) + "%").Appearance(Surface(color + "22", color + "88", 999f)).Render();
                _gui.EndHorizontalGroup();
                _gui.AddSpace(12f);
                _gui.Progress(progress).WidthValue(208f).HeightValue(7f).ShowPercentage(false).Appearance(Surface(color + "22", color, 999f)).Render();
            });
            _gui.EndCard();
        }

        private void DrawCrewCards()
        {
            DrawCrewCard("Mira", "Pilot", "Online", "#67e8f9");
            _gui.AddSpace(10f);
            DrawCrewCard("Sol", "Systems", "Docked", "#bef264");
            _gui.AddSpace(10f);
            DrawCrewCard("Ivo", "Signals", "Scanning", "#fbbf24");
            _gui.AddSpace(10f);
            DrawCrewCard("Nara", "Medic", "Ready", "#fda4af");
        }

        private void DrawCrewCard(string name, string role, string status, string color)
        {
            _gui.BeginCard(268f, 74f, ControlVariant.Default, ControlSize.Default, _glass);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                _gui.Avatar().Fallback(name.Substring(0, 1)).Online(status != "Docked").Border(Theme.Hex(color)).Render();
                _gui.AddSpace(8f);
                _gui.BeginVerticalGroup();
                _gui.Label(name);
                _gui.Caption(role);
                _gui.EndVerticalGroup();
                GUILayout.FlexibleSpace();
                _gui.Badge(status).Small().Appearance(Surface(color + "22", color + "99", 999f)).Render();
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();
        }

        private void DrawActivity()
        {
            DrawActivityItem("02:14", "Relay handshake completed", ToastVariant.Success);
            DrawActivityItem("02:12", "Dock arm recalibrated", ToastVariant.Info);
            DrawActivityItem("02:08", "Thermal threshold nearing cap", ToastVariant.Warning);
            DrawActivityItem("02:03", "Unauthorized ping blocked", ToastVariant.Error);
            DrawActivityItem("01:58", "Crew manifest synchronized", ToastVariant.Success);
        }

        private void DrawActivityItem(string time, string text, ToastVariant variant)
        {
            _gui.WithTooltip(text, () =>
            {
                _gui.BeginHorizontalGroup();
                _gui.Badge(time).Small().Appearance(_pill).Render();
                _gui.AddSpace(8f);
                _gui.Label(text, variant == ToastVariant.Error ? ControlVariant.Destructive : ControlVariant.Muted, options: GUILayout.Width(220f));
                _gui.EndHorizontalGroup();
                _gui.AddSpace(8f);
            });
        }

        private void DrawCommandDialog()
        {
            _gui.Dialog("nova_launch_brief")
                .Title("Launch Brief")
                .Description("Current mission posture and command intent.")
                .Size(460f, 310f)
                .ParentWindow(_windowRect)
                .Content(() =>
                {
                    _gui.Badge("Priority: " + _priorities[_priorityIndex]).Appearance(_accent).Render();
                    _gui.AddSpace(12f);
                    _gui.Caption("Region: " + _regions[_regionIndex]);
                    _gui.Caption("Autopilot: " + (_autopilot ? "enabled" : "manual"));
                    _gui.Caption("Silent mode: " + (_silentMode ? "enabled" : "disabled"));
                    _gui.AddSpace(12f);
                    _gui.TextArea(_dispatchNote).MinHeight(80f).ShowCharacterCount(false).Appearance(_input).Render();
                })
                .Footer(() =>
                {
                    if (_gui.Button("Confirm", ControlVariant.Default, ControlSize.Small, appearance: _accent))
                    {
                        _gui.CloseDialog();
                        _gui.Toast().Title("Launch brief confirmed").Description("Command intent recorded for the active watch.").Variant(ToastVariant.Success).Render();
                    }
                    if (_gui.Button("Close", ControlVariant.Outline, ControlSize.Small, appearance: _pill))
                        _gui.CloseDialog();
                })
                .Render();
        }

        private void BuildAppearances()
        {
            _panel = Surface("#10131acc", "#2a334499", 12f);
            _panelStrong = Surface("#0c111bcc", "#67e8f966", 14f);
            _panelSoft = Surface("#15151acc", "#34303f99", 12f);
            _glass = Surface("#111827aa", "#ffffff22", 12f);
            _accent = Surface("#155e75", "#67e8f9", 10f);
            _accentAlt = Surface("#365314", "#bef264", 10f);
            _danger = Surface("#7f1d1d", "#fda4af", 10f);
            _pill = Surface("#18181bcc", "#3f3f4699", 999f);
            _input = Surface("#09090bcc", "#3f3f46", 10f);
        }

        private void BuildTextures()
        {
            _heroTexture = CreateNebulaTexture(512, 256);
            _miniMapTexture = CreateMapTexture(304, 96);
        }

        private void BuildCharts()
        {
            _energySeries = new List<ChartSeries>
            {
                new ChartSeries("core", "Core", Theme.Hex("#67e8f9"))
                {
                    Data = new List<ChartDataPoint> { new("00", 54), new("04", 68), new("08", 62), new("12", 83), new("16", 78), new("20", 91) },
                },
                new ChartSeries("reserve", "Reserve", Theme.Hex("#bef264"))
                {
                    Data = new List<ChartDataPoint> { new("00", 34), new("04", 46), new("08", 48), new("12", 52), new("16", 61), new("20", 74) },
                },
            };

            _trafficSeries = new List<ChartSeries>
            {
                new ChartSeries("north", "North", Theme.Hex("#fbbf24"))
                {
                    Data = new List<ChartDataPoint> { new("A", 32), new("B", 48), new("C", 44), new("D", 67), new("E", 58) },
                },
                new ChartSeries("south", "South", Theme.Hex("#fda4af"))
                {
                    Data = new List<ChartDataPoint> { new("A", 18), new("B", 30), new("C", 36), new("D", 42), new("E", 39) },
                },
            };
        }

        private void BuildTable()
        {
            _columns = new List<DataTableColumn>
            {
                new DataTableColumn("team", "Team", "team", 92f),
                new DataTableColumn("lead", "Lead", "lead", 80f),
                new DataTableColumn("zone", "Zone", "zone", 88f),
                new DataTableColumn("status", "Status", "status", 86f),
            };

            _rows = new List<DataTableRow>
            {
                Row("1", "Vector", "Mira", "A-17", "Ready"),
                Row("2", "Helix", "Sol", "C-04", "Docked"),
                Row("3", "Prism", "Ivo", "B-22", "Scanning"),
                Row("4", "Lumen", "Nara", "D-11", "Ready"),
                Row("5", "Apex", "Ren", "F-02", "Holding"),
            };
        }

        private DataTableRow Row(string id, string team, string lead, string zone, string status)
        {
            return new DataTableRow(
                id,
                new Dictionary<string, object>
                {
                    ["team"] = team,
                    ["lead"] = lead,
                    ["zone"] = zone,
                    ["status"] = status,
                }
            );
        }

        private float AnimatedValue(float center, float amplitude, float speed)
        {
            return Mathf.Clamp(center + Mathf.Sin(Time.time * speed) * amplitude, 0f, 100f);
        }

        private Texture2D CreateNebulaTexture(int width, int height)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color[width * height];
            Color top = Theme.Hex("#0f172a");
            Color mid = Theme.Hex("#164e63");
            Color flare = Theme.Hex("#bef264");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = x / (float)(width - 1);
                    float ny = y / (float)(height - 1);
                    float wave = Mathf.Sin(nx * 18f + ny * 9f) * 0.5f + 0.5f;
                    float radial = 1f - Mathf.Clamp01(Vector2.Distance(new Vector2(nx, ny), new Vector2(0.72f, 0.32f)) * 2.2f);
                    Color color = Color.Lerp(top, mid, ny * 0.8f + wave * 0.18f);
                    color = Color.Lerp(color, flare, radial * 0.42f);
                    color.a = 1f;
                    pixels[y * width + x] = color;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            _generatedTextures.Add(texture);
            return texture;
        }

        private Texture2D CreateMapTexture(int width, int height)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color[width * height];
            Color baseColor = Theme.Hex("#08111f");
            Color lineColor = Theme.Hex("#164e63");
            Color nodeColor = Theme.Hex("#67e8f9");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool grid = x % 24 == 0 || y % 24 == 0;
                    Color color = grid ? Color.Lerp(baseColor, lineColor, 0.5f) : baseColor;

                    float ridge = Mathf.Abs(y - (height * 0.52f + Mathf.Sin(x * 0.05f) * 18f));
                    if (ridge < 2f)
                        color = Color.Lerp(color, nodeColor, 0.8f);

                    float d1 = Vector2.Distance(new Vector2(x, y), new Vector2(width * 0.26f, height * 0.45f));
                    float d2 = Vector2.Distance(new Vector2(x, y), new Vector2(width * 0.62f, height * 0.34f));
                    float d3 = Vector2.Distance(new Vector2(x, y), new Vector2(width * 0.82f, height * 0.68f));
                    if (d1 < 5f || d2 < 5f || d3 < 5f)
                        color = nodeColor;

                    pixels[y * width + x] = color;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            _generatedTextures.Add(texture);
            return texture;
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
    }
}
