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
    public class FullDemo_old : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new(20f, 20f, 1450f, 800f);
        private bool showDemoWindow = true;
        private bool useVerticalTabs;
        private bool verticalTabsOnRight;
        private bool tabsOnBottom;
        private Vector2 scrollPosition;
        private float lastScrollViewportHeight;
        private float lastScrollContentHeight;

        private int currentDemoTab;
        private readonly string[] demoTabs = { "Button", "Badge", "Input", "Toggle", "Checkbox", "Switch", "TextArea", "Avatar", "Card", "Progress", "Separator", "Label", "Dialog", "Select", "DropdownMenu", "Popover", "Tabs", "MenuBar", "Chart", "Table", "Toast", "Tooltip", "Slider", "Layout" };

        private Texture2D img;

        private string passwordValue = "password123";
        private string textAreaValue = "Current API example with validation-ready content.";
        private string emailValue = string.Empty;
        private DateTime? selectedDate = DateTime.Today;
        private float sliderValue = 0.5f;
        private float sliderWithStepValue = 50f;
        private Vector2 rangeSliderValue = new(20f, 70f);

        private readonly Dictionary<string, bool> toggleStates = new();
        private int selectIndex;

        private readonly string[] selectItems = { "Alpha", "Bravo", "Charlie", "Delta" };
        private readonly string[] tableHeaders = { "Name", "Status", "Ping" };
        private readonly string[,] tableRows =
        {
            { "Alpha", "Ready", "18 ms" },
            { "Bravo", "Queued", "24 ms" },
            { "Charlie", "Ready", "31 ms" },
            { "Delta", "Offline", "0 ms" },
            { "Echo", "Ready", "16 ms" },
        };

        private readonly List<DataTableColumn> dataColumns = new();
        private readonly List<DataTableRow> dataRows = new();
        private readonly List<ChartSeries> chartSeries = new();
        private readonly List<ChartSeries> chartPieSeries = new();

        private bool showDialog;
        private bool showPopover;
        private int nestedTabIndex;
        private int navIndex = 1;

        private const string DialogId = "legacy_demo_dialog";
        private const string PopoverId = "legacy_demo_popover";
        private const string DataTableId = "legacy_demo_table";

        private void Start()
        {
            guiHelper = new GUIHelper();
            img = CreatePatternTexture(32, Theme.Hex("#0f172a"), Theme.Hex("#38bdf8"), 4);
            BuildData();
            BuildCharts();
        }

        private void OnDestroy()
        {
            guiHelper?.Cleanup();
            if (img != null)
                Destroy(img);
        }

        private void OnGUI()
        {
            GUI.skin.horizontalScrollbar = GUIStyle.none;
            GUI.skin.verticalScrollbar = GUIStyle.none;

            if (showDemoWindow)
                windowRect = GUI.Window(101, windowRect, (GUI.WindowFunction)DrawDemoWindow, "shadcn/ui Component Lab");

            guiHelper.DrawOverlay();
        }

        private void DrawDemoWindow(int windowID)
        {
            guiHelper.UpdateGUI(showDemoWindow);
            if (!guiHelper.BeginGUI())
                return;

            DrawHeader();

            if (useVerticalTabs)
                currentDemoTab = guiHelper.Tabs().Items(demoTabs).SelectedIndex(currentDemoTab).Side(verticalTabsOnRight ? TabSide.Right : TabSide.Left).Content(DrawScrollableContent);
            else
                currentDemoTab = guiHelper.Tabs().Items(demoTabs).SelectedIndex(currentDemoTab).Position(tabsOnBottom ? TabPosition.Bottom : TabPosition.Top).MaxLines(2).Content(DrawScrollableContent);

            guiHelper.EndGUI();
            GUI.DragWindow();
        }

        private void DrawHeader()
        {
            guiHelper.Label("shadcn/ui component laboratory").Large().Render();
            guiHelper.Label("A focused reference for the current builders, configs, state, and overlay behavior.").Muted().Render();
            useVerticalTabs = guiHelper.Toggle("Vertical tabs", useVerticalTabs);
            if (useVerticalTabs)
                verticalTabsOnRight = guiHelper.Toggle("Right side", verticalTabsOnRight);
            else
                tabsOnBottom = guiHelper.Toggle("Bottom tabs", tabsOnBottom);
            guiHelper.HorizontalSeparator();
        }

        private void DrawScrollableContent()
        {
            scrollPosition = guiHelper.ScrollView(
                scrollPosition,
                () =>
                {
                    DrawCurrentTab();

                    if (Event.current.type == EventType.Repaint)
                    {
                        Rect contentRect = GUILayoutUtility.GetLastRect();
                        lastScrollContentHeight = Mathf.Max(0f, contentRect.height);
                    }
                },
                GUILayout.ExpandHeight(true)
            );

            if (Event.current.type == EventType.Repaint)
            {
                Rect viewportRect = GUILayoutUtility.GetLastRect();
                lastScrollViewportHeight = Mathf.Max(0f, viewportRect.height);
            }
        }

        private void DrawCurrentTab()
        {
            switch (currentDemoTab)
            {
                case 0:
                    DrawButtonDemos();
                    break;
                case 1:
                    DrawBadgeDemos();
                    break;
                case 2:
                    DrawInputDemos();
                    break;
                case 3:
                    DrawToggleDemos();
                    break;
                case 4:
                    DrawCheckboxDemos();
                    break;
                case 5:
                    DrawSwitchDemos();
                    break;
                case 6:
                    DrawTextAreaDemos();
                    break;
                case 7:
                    DrawAvatarDemos();
                    break;
                case 8:
                    DrawCardDemos();
                    break;
                case 9:
                    DrawProgressDemos();
                    break;
                case 10:
                    DrawSeparatorDemos();
                    break;
                case 11:
                    DrawLabelDemos();
                    break;
                case 12:
                    DrawDialogDemos();
                    break;
                case 13:
                    DrawSelectDemos();
                    break;
                case 14:
                    DrawDropdownDemos();
                    break;
                case 15:
                    DrawPopoverDemos();
                    break;
                case 16:
                    DrawTabsDemos();
                    break;
                case 17:
                    DrawMenuBarDemo();
                    break;
                case 18:
                    DrawChartDemos();
                    break;
                case 19:
                    DrawTableDemos();
                    break;
                case 20:
                    DrawToastDemos();
                    break;
                case 21:
                    DrawTooltipNote();
                    break;
                case 22:
                    DrawSliderDemos();
                    break;
                case 23:
                    DrawLayoutDemos();
                    break;
            }
        }

        private void DrawSection(string title, Action content)
        {
            guiHelper.Label(title).Large();
            guiHelper.Label("Current API example").Muted();
            guiHelper.AddSpace(8f);
            content?.Invoke();
            guiHelper.AddSpace(18f);
            guiHelper.HorizontalSeparator();
            guiHelper.AddSpace(18f);
        }

        private void DrawButtonDemos()
        {
            DrawSection(
                "Button",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Primary");
                    guiHelper.Button("Secondary").Secondary();
                    guiHelper.Button("Outline").Outline();
                    guiHelper.Button("Danger").Destructive();
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawBadgeDemos()
        {
            DrawSection(
                "Badge",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Badge("Default");
                    guiHelper.Badge("Queued").Secondary();
                    guiHelper.Badge("42").Count(42).Outline();
                    guiHelper.Badge("Online").StatusDot();
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawInputDemos()
        {
            DrawSection(
                "Input",
                () =>
                {
                    passwordValue = guiHelper.Input(passwordValue).Label("Password").Password();
                    emailValue = guiHelper.Input(emailValue).Label("Email").Placeholder("pilot@relay.local");
                }
            );
        }

        private void DrawToggleDemos()
        {
            DrawSection(
                "Toggle",
                () =>
                {
                    toggleStates["toggle_default"] = guiHelper.Toggle("Feature Flag", GetToggle("toggle_default"));
                    toggleStates["toggle_outline"] = guiHelper.Toggle("Muted Routing", GetToggle("toggle_outline")).Outline();
                    guiHelper.Toggle("Disabled state", true).Disabled();
                }
            );
        }

        private void DrawCheckboxDemos()
        {
            DrawSection(
                "Checkbox",
                () =>
                {
                    toggleStates["checkbox_1"] = guiHelper.Checkbox("Enable Alerts", GetToggle("checkbox_1"));
                    toggleStates["checkbox_2"] = guiHelper.Checkbox("Auto Assign", GetToggle("checkbox_2")).Secondary();
                    guiHelper.Checkbox("Disabled state", true).Disabled();
                }
            );
        }

        private void DrawSwitchDemos()
        {
            DrawSection(
                "Switch",
                () =>
                {
                    toggleStates["switch_1"] = guiHelper.Switch("Maintenance Mode", GetToggle("switch_1"));
                    toggleStates["switch_2"] = guiHelper.Switch("Broadcast Changes", GetToggle("switch_2")).Small();
                    guiHelper.Switch("Disabled state", true).Disabled();
                }
            );
        }

        private void DrawTextAreaDemos()
        {
            DrawSection(
                "Text Area",
                () =>
                {
                    textAreaValue = guiHelper.TextArea(textAreaValue).Label("Notes").MinHeight(110f).ShowCharacterCount();
                }
            );
        }

        private void DrawAvatarDemos()
        {
            DrawSection(
                "Avatar",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Avatar().Image(img).Fallback("AL").Name("Ava Lane").Online();
                    guiHelper.Avatar().Fallback("BR").Shape(AvatarShape.Rounded).Border(Theme.Hex("#22c55e"));
                    guiHelper.Avatar().Fallback("CY").Shape(AvatarShape.Square);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawCardDemos()
        {
            DrawSection(
                "Card",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Card().Title("Relay Tower").Subtitle("North Wing").Content("Current builder API").Image(img).Size(220f, 200f);
                    guiHelper.Card().Title("Operator").Content("A compact example card.").Avatar(img).Size(220f, 170f);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawProgressDemos()
        {
            DrawSection(
                "Progress",
                () =>
                {
                    guiHelper.Progress(sliderValue).Label("Upload").WidthValue(420f).ShowPercentage();
                }
            );
        }

        private void DrawSeparatorDemos()
        {
            DrawSection(
                "Separator",
                () =>
                {
                    guiHelper.Label("Before");
                    guiHelper.HorizontalSeparator();
                    guiHelper.Label("After");
                }
            );
        }

        private void DrawLabelDemos()
        {
            DrawSection(
                "Label",
                () =>
                {
                    guiHelper.Label("Default");
                    guiHelper.Label("Muted").Muted();
                    guiHelper.Label("Secondary").Secondary();
                }
            );
        }

        private void DrawDialogDemos()
        {
            DrawSection(
                "Dialog",
                () =>
                {
                    if (guiHelper.Button("Open Dialog", ControlVariant.Default, ControlSize.Small))
                        showDialog = true;

                    if (showDialog && !guiHelper.IsDialogOpen())
                        guiHelper.OpenDialog(DialogId);

                    if (showDialog)
                    {
                        guiHelper
                            .Dialog(DialogId)
                            .ParentWindow(windowRect)
                            .Title("Confirm operation")
                            .Description("A focused overlay using the current dialog configuration.")
                            .OnClosed(() => showDialog = false)
                            .Content(() => guiHelper.Label("Dialog content").Muted())
                            .Footer(() =>
                            {
                                if (guiHelper.Button("Close", ControlVariant.Outline, ControlSize.Small))
                                    guiHelper.CloseDialog();
                            });
                    }
                }
            );
        }

        private void DrawSelectDemos()
        {
            DrawSection(
                "Select",
                () =>
                {
                    selectIndex = guiHelper.Select().Id("legacy_squad_select").Label("Squad").Placeholder("Choose a squad").Items(selectItems).SelectedIndex(selectIndex).CloseOnSelect().MaxHeight(180f).Width(240f);
                    guiHelper.Badge($"Selected: {selectItems[Mathf.Clamp(selectIndex, 0, selectItems.Length - 1)]}").Outline().Render();
                    selectedDate = guiHelper.DatePicker().Id("legacy_demo_date").Label("Deployment date").Value(selectedDate).DisplayFormat("MMM d, yyyy").Range(DateTime.Today, DateTime.Today.AddDays(30)).Render();
                }
            );
        }

        private void DrawDropdownDemos()
        {
            DrawSection(
                "Dropdown Menu",
                () =>
                {
                    guiHelper
                        .DropdownMenu()
                        .Id("legacy_actions_menu")
                        .Trigger(() => guiHelper.Button("Open actions", ControlVariant.Outline, ControlSize.Small))
                        .Width(220f)
                        .MaxHeight(220f)
                        .CloseOnClickOutside()
                        .CloseOnSelect()
                        .Header("Actions")
                        .Item("Deploy")
                        .Item("Duplicate")
                        .Separator()
                        .Item("Archive")
                        .Render();
                }
            );
        }

        private void DrawPopoverDemos()
        {
            DrawSection(
                "Popover",
                () =>
                {
                    if (guiHelper.Button("Open Popover", ControlVariant.Outline, ControlSize.Small))
                        showPopover = true;

                    guiHelper
                        .Popover(PopoverId)
                        .Content(() =>
                        {
                            guiHelper.Label("Quick details");
                            guiHelper.Label("Current popover configuration").Muted();
                        });

                    if (showPopover)
                        guiHelper.Popover(PopoverId).Open();
                }
            );
        }

        private void DrawTabsDemos()
        {
            DrawSection(
                "Tabs",
                () =>
                {
                    nestedTabIndex = guiHelper.Tabs().Items("Overview", "Settings", "History").SelectedIndex(nestedTabIndex).Indicator(IndicatorStyle.Background).Content(() => guiHelper.Label($"Nested tab: {nestedTabIndex + 1}"));
                }
            );
        }

        private void DrawMenuBarDemo()
        {
            DrawSection(
                "Menu Bar",
                () =>
                {
                    guiHelper.MenuBar().Item("File", items => items.Item("New").Item("Save").Separator().Item("Close")).Item("View", items => items.Item("Compact").Item("Expanded")).Item("Help", items => items.Item("Migration").Item("API"));
                }
            );
        }

        private void DrawChartDemos()
        {
            DrawSection(
                "Chart",
                () =>
                {
                    guiHelper.Chart().Type(ChartType.Line).Series(chartSeries.ToArray()).Size(560f, 260f);
                    guiHelper.AddSpace(12f);
                    guiHelper.Chart().Type(ChartType.Pie).Series(chartPieSeries.ToArray()).Size(360f, 260f);
                }
            );
        }

        private void DrawTableDemos()
        {
            DrawSection(
                "Table",
                () =>
                {
                    guiHelper.Table().Headers(tableHeaders).Rows(tableRows).Render();
                    guiHelper.AddSpace(12f);
                    guiHelper.DataTable(DataTableId).Columns(dataColumns).Rows(dataRows).ShowToolbar().ShowSearch().ShowPagination().Sorting().Filtering().FilterPlaceholder("Filter squads...").EmptyText("No squads found.").PageSize(5).PageSizeOptions(5, 10, 20).Render();
                }
            );
        }

        private void DrawToastDemos()
        {
            DrawSection(
                "Toast",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.Button("Success", ControlVariant.Default, ControlSize.Small))
                        ShowToast("Success", "Operation completed", ToastVariant.Success);
                    if (guiHelper.Button("Warning", ControlVariant.Secondary, ControlSize.Small))
                        ShowToast("Warning", "Check the queue", ToastVariant.Warning);
                    if (guiHelper.Button("Error", ControlVariant.Destructive, ControlSize.Small))
                        ShowToast("Error", "A node dropped", ToastVariant.Error);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawTooltipNote()
        {
            DrawSection(
                "Tooltip",
                () =>
                {
                    guiHelper.Label("Hover each control to preview tooltip placement and styling.").Muted();
                    guiHelper.AddSpace(8f);

                    guiHelper.BeginHorizontalGroup();
                    guiHelper.WithTooltip("Primary actions can explain the effect before the user clicks.", () => guiHelper.Button("Hover Button", ControlVariant.Default, ControlSize.Small));
                    guiHelper.WithTooltip("Badges work too, including fluent badge builders without calling Render.", () => guiHelper.Badge("Live").StatusDot());
                    guiHelper.EndHorizontalGroup();

                    guiHelper.AddSpace(10f);
                    guiHelper.WithTooltip(
                        "This tooltip uses a shorter delay and a wider wrapping area for longer explanatory copy.",
                        new TooltipConfig
                        {
                            HoverDelaySeconds = 0.15f,
                            MaxWidth = 360f,
                            Variant = ControlVariant.Secondary,
                        },
                        () => guiHelper.Label("Fast secondary tooltip").Secondary()
                    );

                    guiHelper.AddSpace(10f);
                    guiHelper.WithTooltip("Destructive affordances can warn users without taking over the layout.", new TooltipConfig { HoverDelaySeconds = 0.1f, Variant = ControlVariant.Destructive }, () => guiHelper.Button("Danger Hover", ControlVariant.Destructive, ControlSize.Small));
                }
            );
        }

        private void DrawSliderDemos()
        {
            DrawSection(
                "Slider",
                () =>
                {
                    sliderValue = guiHelper.Slider(sliderValue).Label("Volume").Range(0f, 1f).Step(0.05f).ShowValue();
                    sliderWithStepValue = guiHelper.Slider(sliderWithStepValue).Label("Percent").Range(0f, 100f).Step(10f).ShowValue().Format("F0");
                    rangeSliderValue = guiHelper.RangeSlider(rangeSliderValue.x, rangeSliderValue.y).Label("Window").Range(0f, 100f).Step(5f).ShowValue().Format("F0");
                }
            );
        }

        private void DrawLayoutDemos()
        {
            DrawSection(
                "Layout",
                () =>
                {
                    navIndex = guiHelper.Navigation().Logo("L").Width(90f).Items(new NavigationItem("home", "Home"), new NavigationItem("queue", "Queue"), new NavigationItem("logs", "Logs")).SelectedIndex(navIndex);

                    guiHelper.AddSpace(12f);
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Left");
                    guiHelper.Flex();
                    guiHelper.Button("Right");
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private bool GetToggle(string key)
        {
            if (!toggleStates.TryGetValue(key, out bool value))
            {
                value = false;
                toggleStates[key] = value;
            }

            return value;
        }

        private void ShowToast(string title, string description, ToastVariant variant)
        {
            guiHelper.Toast().Title(title).Description(description).Variant(variant).Position(ToastPosition.BottomRight).Duration(3200f);
        }

        private void BuildData()
        {
            dataColumns.Add(new DataTableColumn("name", "Name", "name", -1f));
            dataColumns.Add(new DataTableColumn("status", "Status", "status", -1f));
            dataColumns.Add(new DataTableColumn("ping", "Ping", "ping", -1f));

            dataRows.Add(
                new DataTableRow(
                    "1",
                    new Dictionary<string, object>
                    {
                        ["name"] = "Alpha",
                        ["status"] = "Ready",
                        ["ping"] = "18 ms",
                    }
                )
            );
            dataRows.Add(
                new DataTableRow(
                    "2",
                    new Dictionary<string, object>
                    {
                        ["name"] = "Bravo",
                        ["status"] = "Queued",
                        ["ping"] = "24 ms",
                    }
                )
            );
            dataRows.Add(
                new DataTableRow(
                    "3",
                    new Dictionary<string, object>
                    {
                        ["name"] = "Charlie",
                        ["status"] = "Ready",
                        ["ping"] = "31 ms",
                    }
                )
            );
            dataRows.Add(
                new DataTableRow(
                    "4",
                    new Dictionary<string, object>
                    {
                        ["name"] = "Delta",
                        ["status"] = "Offline",
                        ["ping"] = "0 ms",
                    }
                )
            );
        }

        private void BuildCharts()
        {
            chartSeries.Add(
                new ChartSeries("latency", "Latency", Theme.Hex("#38bdf8"))
                {
                    Data = new List<ChartDataPoint> { new("00", 22), new("06", 30), new("12", 18), new("18", 26), new("24", 20) },
                }
            );

            chartPieSeries.Add(
                new ChartSeries("regions", "Regions")
                {
                    Data = new List<ChartDataPoint> { new("NA", 42, Theme.Hex("#38bdf8")), new("EU", 31, Theme.Hex("#22c55e")), new("APAC", 19, Theme.Hex("#f59e0b")), new("LATAM", 8, Theme.Hex("#ef4444")) },
                }
            );
        }

        private static Texture2D CreatePatternTexture(int size, Color dark, Color light, int blockSize)
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
            return texture;
        }

        public float GetScreenshotMaxScroll()
        {
            return Mathf.Max(0f, lastScrollContentHeight - lastScrollViewportHeight);
        }
    }
}
