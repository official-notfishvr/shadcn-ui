using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui_Demo.Menu
{
    public class FullDemo : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new Rect(20, 20, 1450, 800);
        private bool showDemoWindow = false;
        private bool useVerticalTabs = false;
        private Vector2 scrollPosition;

        // Demo State Variables
        private int currentDemoTab = 0;
        private Tabs.TabConfig[] demoTabs;
        private bool verticalTabsOnRight = false;
        private bool tabsOnBottom = false;
        private int componentCount = 0;
        private Texture2D img = new Texture2D(2, 2);

        // Input States
        private string passwordValue = "password123";
        private string textAreaValue = "";
        private string outlineTextAreaValue = "Outline Text Area";
        private string ghostTextAreaValue = "Ghost Text Area";
        private string labeledTextAreaValue = "Labeled Text Area";
        private float resizableTextAreaHeight = 100f;
        private string resizableTextAreaValue = "Resizable Text Area";

        // Toggle/Switch/Checkbox States
        private Dictionary<string, bool> toggleStates = new Dictionary<string, bool>();

        // Dropdown/Select/Popover States
        private bool dropdownOpen = false;
        private int selectIndex = 0;
        private List<DropdownMenuItem> dropdownMenuItems;

        // Calendar/Date States
        private DateTime? calendarSelectedDate;
        private DateTime? datePickerDate;
        private DateTime? datePickerWithLabelDate;

        // Table States
        private List<DataTableColumn> dataTableColumns;
        private List<DataTableRow> dataTableData;
        private int[] sortColumns = new int[0];
        private bool[] sortAscending = new bool[0];
        private bool[] selectedRows = new bool[0];
        private int currentPage = 0;
        private string searchQuery = "";
        private string[,] filteredData;
        private float[] columnWidths;

        // Chart States
        private List<ChartSeries> chartSeries;

        void Start()
        {
            guiHelper = new GUIHelper();

            // Load Image
            byte[] imageData = LoadEmbeddedBytes("shadcnui_Demo.Img.1.png");
#if MONO
            if (imageData != null)
                img.LoadImage(imageData);
#endif

            demoTabs = new Tabs.TabConfig[]
            {
                new Tabs.TabConfig("Button", DrawButtonDemos),
                new Tabs.TabConfig("Badge", DrawBadgeDemos),
                new Tabs.TabConfig("Input", DrawInputDemos),
                new Tabs.TabConfig("Toggle", DrawToggleDemos),
                new Tabs.TabConfig("Checkbox", DrawCheckboxDemos),
                new Tabs.TabConfig("Switch", DrawSwitchDemos),
                new Tabs.TabConfig("TextArea", DrawTextAreaDemos),
                new Tabs.TabConfig("Avatar", DrawAvatarDemos),
                new Tabs.TabConfig("Card", DrawCardDemos),
                new Tabs.TabConfig("Progress", DrawProgressDemos),
                new Tabs.TabConfig("Separator", DrawSeparatorDemos),
                new Tabs.TabConfig("Label", DrawLabelDemos),
                new Tabs.TabConfig("Dialog", DrawDialogDemos),
                new Tabs.TabConfig("Select", DrawSelectDemos),
                new Tabs.TabConfig("Dropdown", DrawDropdownMenuDemos),
                new Tabs.TabConfig("Popover", DrawPopoverDemos),
                new Tabs.TabConfig("Calendar", DrawCalendarDemos),
                new Tabs.TabConfig("DatePicker", DrawDatePickerDemos),
                new Tabs.TabConfig("Tabs", DrawTabsDemos),
                new Tabs.TabConfig("MenuBar", DrawMenuBar),
                new Tabs.TabConfig("Chart", DrawChartDemos),
                new Tabs.TabConfig("Table", DrawTableDemos),
                new Tabs.TabConfig("Interactive Tables", DataTableDemos),
                new Tabs.TabConfig("Layout", DrawLayoutDemos),
            };

            componentCount = demoTabs.Length;
        }

        private byte[] LoadEmbeddedBytes(string path)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(path))
                {
                    if (stream == null)
                        return null;
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    return data;
                }
            }
            catch
            {
                return null;
            }
        }

        void OnGUI()
        {
            // Todo: make the GUI auto do this
            GUI.skin.horizontalScrollbar = GUIStyle.none;
            GUI.skin.verticalScrollbar = GUIStyle.none;

            GUILayout.BeginArea(new Rect(10, 10, 400, 60));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Open Demo Window", GUILayout.Width(150), GUILayout.Height(30)))
            {
                showDemoWindow = !showDemoWindow;
            }
            GUILayout.Label($"Components: {componentCount}", GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (showDemoWindow)
            {
                windowRect = GUI.Window(101, windowRect, (GUI.WindowFunction)DrawDemoWindow, "shadcn/ui Demo - Full Showcase");
            }
        }

        void DrawDemoWindow(int windowID)
        {
            guiHelper.UpdateAnimations(showDemoWindow);
            if (guiHelper.BeginAnimatedGUI())
            {
                DrawHeader();

                if (useVerticalTabs)
                {
                    var side = verticalTabsOnRight ? Tabs.TabSide.Right : Tabs.TabSide.Left;
                    currentDemoTab = guiHelper.VerticalTabs(demoTabs.Select(tab => tab.Name).ToArray(), currentDemoTab, DrawScrollableContent, tabWidth: 160, maxLines: 2, side: side);
                }
                else
                {
                    var position = tabsOnBottom ? Tabs.TabPosition.Bottom : Tabs.TabPosition.Top;
                    currentDemoTab = guiHelper.DrawTabs(demoTabs.Select(tab => tab.Name).ToArray(), currentDemoTab, DrawScrollableContent, maxLines: 2, position: position);
                }
                guiHelper.EndAnimatedGUI();
            }
            GUI.DragWindow();
        }

        void DrawHeader()
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("shadcn/ui Library", ControlVariant.Default);
            GUILayout.FlexibleSpace();
            useVerticalTabs = guiHelper.Toggle("Vertical Tabs", useVerticalTabs);
            GUILayout.Space(10);
            if (useVerticalTabs)
                verticalTabsOnRight = guiHelper.Toggle("Right Side", verticalTabsOnRight);
            else
                tabsOnBottom = guiHelper.Toggle("Bottom Tabs", tabsOnBottom);
            guiHelper.EndHorizontalGroup();
            guiHelper.MutedLabel("Explore all available components, variants, and sizes.");
            guiHelper.HorizontalSeparator();
        }

        void DrawScrollableContent()
        {
            scrollPosition = guiHelper.DrawScrollView(
                scrollPosition,
                () =>
                {
#if IL2CPP_MELONLOADER_PRE57
                    GUILayout.BeginVertical(new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { }));
#else
                    GUILayout.BeginVertical();
#endif
                    if (currentDemoTab >= 0 && currentDemoTab < demoTabs.Length)
                    {
                        demoTabs[currentDemoTab].Content?.Invoke();
                    }
                    GUILayout.EndVertical();
                },
                GUILayout.ExpandHeight(true)
            );
        }

        #region Helper Methods
        private void DrawSection(string title, Action content)
        {
            guiHelper.Label(title, ControlVariant.Default);
            guiHelper.HorizontalSeparator();
            content?.Invoke();
            GUILayout.Space(20);
        }

        private void DrawAllVariants(Action<ControlVariant> drawFunc)
        {
            guiHelper.BeginHorizontalGroup();
            foreach (ControlVariant variant in Enum.GetValues(typeof(ControlVariant)))
            {
                GUILayout.BeginVertical(GUILayout.Width(100));
                guiHelper.Label(variant.ToString(), ControlVariant.Muted);
                drawFunc(variant);
                GUILayout.EndVertical();
            }
            guiHelper.EndHorizontalGroup();
        }

        private void DrawAllSizes(Action<ControlSize> drawFunc)
        {
            guiHelper.BeginHorizontalGroup();
            foreach (ControlSize size in Enum.GetValues(typeof(ControlSize)))
            {
                GUILayout.BeginVertical(GUILayout.Width(100));
                guiHelper.Label(size.ToString(), ControlVariant.Muted);
                drawFunc(size);
                GUILayout.EndVertical();
            }
            guiHelper.EndHorizontalGroup();
        }
        #endregion

        #region Demos
        void DrawButtonDemos()
        {
            DrawSection(
                "Button Variants",
                () =>
                {
                    DrawAllVariants(variant => guiHelper.Button("Button", variant));
                }
            );

            DrawSection(
                "Button Sizes",
                () =>
                {
                    DrawAllSizes(size => guiHelper.Button("Button", ControlVariant.Default, size));
                }
            );

            DrawSection(
                "Button States",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Normal");
                    guiHelper.Button("Disabled", disabled: true);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        void DrawBadgeDemos()
        {
            DrawSection(
                "Badge Variants",
                () =>
                {
                    DrawAllVariants(variant => guiHelper.Badge("Badge", variant));
                }
            );

            DrawSection(
                "Badge Sizes",
                () =>
                {
                    DrawAllSizes(size => guiHelper.Badge("Badge", ControlVariant.Default, size));
                }
            );

            DrawSection(
                "Special Badges",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CountBadge(5);
                    guiHelper.StatusBadge("Online", true);
                    guiHelper.ProgressBadge("Loading", 0.7f);
                    guiHelper.AnimatedBadge("New");
                    guiHelper.RoundedBadge("Rounded", cornerRadius: 8f);
                    guiHelper.BadgeWithIcon("Icon", img);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        void DrawInputDemos()
        {
            DrawSection(
                "Input Variants",
                () =>
                {
                    guiHelper.Label("Default");
                    string val = "";
                    guiHelper.DrawPasswordField(200, "Default", ref val);
                }
            );

            DrawSection(
                "Input Types",
                () =>
                {
                    guiHelper.Label("Password");
                    guiHelper.DrawPasswordField(300, "Password", ref passwordValue);

                    guiHelper.Label("Section Header");
                    guiHelper.DrawSectionHeader("Header Example");

                    guiHelper.Label("Render Label");
                    guiHelper.RenderLabel("Rendered Label Text", 200);
                }
            );
        }

        void DrawToggleDemos()
        {
            DrawSection(
                "Toggle Variants",
                () =>
                {
                    DrawAllVariants(variant =>
                    {
                        string key = $"toggle_var_{variant}";
                        if (!toggleStates.ContainsKey(key))
                            toggleStates[key] = false;
                        toggleStates[key] = guiHelper.Toggle("Toggle", toggleStates[key], variant);
                    });
                }
            );

            DrawSection(
                "Toggle Sizes",
                () =>
                {
                    DrawAllSizes(size =>
                    {
                        string key = $"toggle_size_{size}";
                        if (!toggleStates.ContainsKey(key))
                            toggleStates[key] = false;
                        toggleStates[key] = guiHelper.Toggle("Toggle", toggleStates[key], ControlVariant.Default, size);
                    });
                }
            );
        }

        void DrawCheckboxDemos()
        {
            DrawSection(
                "Checkbox Variants",
                () =>
                {
                    DrawAllVariants(variant =>
                    {
                        string key = $"check_var_{variant}";
                        if (!toggleStates.ContainsKey(key))
                            toggleStates[key] = false;
                        toggleStates[key] = guiHelper.Checkbox("Check", toggleStates[key], variant);
                    });
                }
            );

            DrawSection(
                "Checkbox Sizes",
                () =>
                {
                    DrawAllSizes(size =>
                    {
                        string key = $"check_size_{size}";
                        if (!toggleStates.ContainsKey(key))
                            toggleStates[key] = false;
                        toggleStates[key] = guiHelper.Checkbox("Check", toggleStates[key], ControlVariant.Default, size);
                    });
                }
            );
        }

        void DrawSwitchDemos()
        {
            DrawSection(
                "Switch Variants",
                () =>
                {
                    DrawAllVariants(variant =>
                    {
                        string key = $"switch_var_{variant}";
                        if (!toggleStates.ContainsKey(key))
                            toggleStates[key] = false;
                        toggleStates[key] = guiHelper.Switch("Switch", toggleStates[key], variant);
                    });
                }
            );

            DrawSection(
                "Switch Sizes",
                () =>
                {
                    DrawAllSizes(size =>
                    {
                        string key = $"switch_size_{size}";
                        if (!toggleStates.ContainsKey(key))
                            toggleStates[key] = false;
                        toggleStates[key] = guiHelper.Switch("Switch", toggleStates[key], ControlVariant.Default, size);
                    });
                }
            );
        }

        void DrawTextAreaDemos()
        {
            DrawSection(
                "TextArea Variants",
                () =>
                {
                    guiHelper.Label("Default");
                    textAreaValue = guiHelper.TextArea(textAreaValue, ControlVariant.Default, "Default");

                    guiHelper.Label("Outline");
                    outlineTextAreaValue = guiHelper.OutlineTextArea(outlineTextAreaValue, "Outline");

                    guiHelper.Label("Ghost");
                    ghostTextAreaValue = guiHelper.GhostTextArea(ghostTextAreaValue, "Ghost");
                }
            );

            DrawSection(
                "Special TextAreas",
                () =>
                {
                    guiHelper.Label("Labeled");
                    labeledTextAreaValue = guiHelper.LabeledTextArea("Label", labeledTextAreaValue, placeholder: "Labeled");

                    guiHelper.Label("Resizable");
                    resizableTextAreaValue = guiHelper.ResizableTextArea(resizableTextAreaValue, ref resizableTextAreaHeight, placeholder: "Resize me");
                }
            );
        }

        void DrawAvatarDemos()
        {
            DrawSection(
                "Avatar Sizes",
                () =>
                {
                    DrawAllSizes(size => guiHelper.Avatar(img, "AV", size));
                }
            );

            DrawSection(
                "Avatar Shapes",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Avatar(img, "CR", ControlSize.Default, AvatarShape.Circle);
                    guiHelper.Avatar(img, "SQ", ControlSize.Default, AvatarShape.Square);
                    guiHelper.Avatar(img, "RD", ControlSize.Default, AvatarShape.Rounded);
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Avatar Features",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.AvatarWithStatus(img, "ON", true);
                    guiHelper.AvatarWithStatus(img, "OF", false);
                    guiHelper.AvatarWithName(img, "JD", "John Doe");
                    guiHelper.AvatarWithBorder(img, "BR", Color.cyan);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        void DrawCardDemos()
        {
            DrawSection(
                "Card Types",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();

                    guiHelper.DrawCard("Standard", "Subtitle", "Content goes here.", () => guiHelper.Button("Action"), 200, 150);

                    GUILayout.Space(10);

                    guiHelper.DrawSimpleCard("Simple card content.", 200, 100);

                    GUILayout.Space(10);

                    guiHelper.BeginCard(200, 150);
                    guiHelper.CardHeader(() => guiHelper.CardTitle("Manual"));
                    guiHelper.CardContent(() => guiHelper.Label("Content"));
                    guiHelper.CardFooter(() => guiHelper.Button("Footer"));
                    guiHelper.EndCard();

                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Rich Cards",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.DrawCardWithImage(img, "Image Card", "Subtitle", "Content with image.", null, 200, 250);
                    GUILayout.Space(10);
                    guiHelper.DrawCardWithAvatar(img, "Avatar Card", "User Name", "Content with avatar.", null, 200, 150);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        void DrawProgressDemos()
        {
            DrawSection(
                "Progress Styles",
                () =>
                {
                    guiHelper.Label("Standard");
                    guiHelper.Progress(0.6f, 300);

                    guiHelper.Label("Labeled");
                    guiHelper.LabeledProgress("Loading...", 0.4f, 300);

                    guiHelper.Label("Circular");
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CircularProgress(0.25f, 40);
                    guiHelper.CircularProgress(0.75f, 40);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        void DrawSeparatorDemos()
        {
            DrawSection(
                "Separators",
                () =>
                {
                    guiHelper.Label("Horizontal");
                    guiHelper.HorizontalSeparator();

                    guiHelper.Label("Labeled");
                    guiHelper.LabeledSeparator("Section");

                    guiHelper.Label("Vertical (in horizontal group)");
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Label("Left");
                    guiHelper.VerticalSeparator();
                    guiHelper.Label("Right");
                    guiHelper.EndHorizontalGroup();

                    guiHelper.Label("With Spacing");
                    guiHelper.SeparatorWithSpacing(SeparatorOrientation.Horizontal, 20, 20);
                    guiHelper.Label("After Spacing");
                }
            );
        }

        void DrawLabelDemos()
        {
            DrawSection(
                "Label Variants",
                () =>
                {
                    DrawAllVariants(variant => guiHelper.Label("Label", variant));
                }
            );
        }

        void DrawDialogDemos()
        {
            DrawSection(
                "Dialogs",
                () =>
                {
                    if (guiHelper.Button("Open Standard Dialog"))
                        guiHelper.OpenDialog("std_dlg");
                    if (guiHelper.Button("Open Simple Dialog"))
                        guiHelper.OpenDialog("smp_dlg");

                    guiHelper.DrawDialog(
                        "std_dlg",
                        "Title",
                        "Description",
                        () => guiHelper.Label("Content"),
                        () =>
                        {
                            if (guiHelper.Button("Close"))
                                guiHelper.CloseDialog();
                        }
                    );

                    guiHelper.DrawDialog(
                        "smp_dlg",
                        () =>
                        {
                            guiHelper.Label("Simple Content");
                            if (guiHelper.Button("Close"))
                                guiHelper.CloseDialog();
                        }
                    );
                }
            );
        }

        void DrawSelectDemos()
        {
            DrawSection(
                "Select",
                () =>
                {
                    if (guiHelper.Button("Open Select"))
                        guiHelper.OpenSelect();
                    if (guiHelper.IsSelectOpen())
                    {
                        string[] opts = { "Option 1", "Option 2", "Option 3" };
                        selectIndex = guiHelper.Select(opts, selectIndex);
                    }
                    guiHelper.Label($"Selected: {selectIndex}");
                }
            );
        }

        void DrawDropdownMenuDemos()
        {
            DrawSection(
                "Dropdown",
                () =>
                {
                    if (guiHelper.Button("Open Dropdown"))
                        dropdownOpen = !dropdownOpen;
                    if (dropdownOpen)
                    {
                        if (dropdownMenuItems == null)
                        {
                            dropdownMenuItems = new List<DropdownMenuItem>
                            {
                                new DropdownMenuItem(DropdownMenuItemType.Header, "Header"),
                                new DropdownMenuItem(DropdownMenuItemType.Item, "Item 1", () => dropdownOpen = false),
                                new DropdownMenuItem(DropdownMenuItemType.Separator),
                                new DropdownMenuItem(DropdownMenuItemType.Item, "Item 2", () => dropdownOpen = false),
                            };
                        }
                        guiHelper.DropdownMenu(new DropdownMenuConfig(dropdownMenuItems));
                    }
                }
            );
        }

        void DrawPopoverDemos()
        {
            DrawSection(
                "Popover",
                () =>
                {
                    if (guiHelper.Button("Open Popover"))
                        guiHelper.OpenPopover();
                    if (guiHelper.IsPopoverOpen())
                    {
                        guiHelper.Popover(() => guiHelper.Label("Popover Content"));
                    }
                }
            );
        }

        void DrawCalendarDemos()
        {
            DrawSection(
                "Calendar",
                () =>
                {
                    GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(320));
                    var cal = guiHelper.GetCalendarComponents();
                    cal.SelectedDate = calendarSelectedDate;
                    cal.OnDateSelected += d => calendarSelectedDate = d;
                    guiHelper.Calendar();
                    GUILayout.EndVertical();

                    guiHelper.Label($"Selected: {calendarSelectedDate}");
                }
            );
        }

        void DrawDatePickerDemos()
        {
            DrawSection(
                "Date Pickers",
                () =>
                {
                    guiHelper.Label("Basic");
                    datePickerDate = guiHelper.DatePicker("Pick Date", datePickerDate, "dp1");

                    guiHelper.Label("Labeled");
                    datePickerWithLabelDate = guiHelper.DatePickerWithLabel("Label", "Pick Date", datePickerWithLabelDate, "dp2");

                    guiHelper.Label("Range Picker");
                    guiHelper.DateRangePicker("Pick Range", DateTime.Now, DateTime.Now.AddDays(7), "drp1");
                }
            );
        }

        void DrawTabsDemos()
        {
            DrawSection(
                "Tabs",
                () =>
                {
                    guiHelper.Label("See the main demo window tabs for examples of Vertical/Horizontal tabs.");
                }
            );
        }

        private void DrawMenuBar()
        {
            DrawSection(
                "Menu Bar",
                () =>
                {
                    var menuItems = new List<MenuBar.MenuItem>
                    {
                        new MenuBar.MenuItem("File", null, false, new List<MenuBar.MenuItem> { new MenuBar.MenuItem("New"), new MenuBar.MenuItem("Open"), MenuBar.MenuItem.Separator(), new MenuBar.MenuItem("Exit") }),
                        new MenuBar.MenuItem("Edit", null, false, new List<MenuBar.MenuItem> { new MenuBar.MenuItem("Cut"), new MenuBar.MenuItem("Copy"), new MenuBar.MenuItem("Paste") }),
                    };
                    guiHelper.MenuBar(new MenuBar.MenuBarConfig(menuItems));
                }
            );
        }

        void DrawChartDemos()
        {
            DrawSection(
                "Charts",
                () =>
                {
                    if (chartSeries == null)
                    {
                        chartSeries = new List<ChartSeries>
                        {
                            new ChartSeries("S1", "Series 1", Color.cyan) { Data = { new ChartDataPoint("A", 10, Color.cyan), new ChartDataPoint("B", 20, Color.cyan) } },
                            new ChartSeries("S2", "Series 2", Color.magenta) { Data = { new ChartDataPoint("A", 15, Color.magenta), new ChartDataPoint("B", 5, Color.magenta) } },
                        };
                    }

                    guiHelper.BeginHorizontalGroup();

                    guiHelper.BeginVerticalGroup();
                    guiHelper.Label("Bar");
                    guiHelper.Chart(new ChartConfig(chartSeries, ChartType.Bar) { Size = new Vector2(300, 200) });
                    guiHelper.EndVerticalGroup();

                    guiHelper.BeginVerticalGroup();
                    guiHelper.Label("Line");
                    guiHelper.Chart(new ChartConfig(chartSeries, ChartType.Line) { Size = new Vector2(300, 200) });
                    guiHelper.EndVerticalGroup();

                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        void DrawTableDemos()
        {
            DrawSection(
                "Basic Tables",
                () =>
                {
                    string[] headers = { "ID", "Name", "Role" };
                    string[,] data =
                    {
                        { "1", "Alice", "Admin" },
                        { "2", "Bob", "User" },
                    };

                    guiHelper.Label("Default");
                    guiHelper.Table(headers, data);

                    guiHelper.Label("Striped");
                    guiHelper.Table(headers, data, ControlVariant.Secondary);

                    guiHelper.Label("Bordered");
                    guiHelper.Table(headers, data, ControlVariant.Outline);

                    guiHelper.Label("Custom Cell Renderer");
                    object[,] objData =
                    {
                        { "Item 1", 100 },
                        { "Item 2", 200 },
                    };
                    guiHelper.CustomTable(
                        new[] { "Name", "Value" },
                        objData,
                        (val, row, col) =>
                        {
                            string text = val.ToString();
                            if (col == 1)
                                text = $"${val}";
                            guiHelper.Label(text);
                        }
                    );
                }
            );
        }

        void DataTableDemos()
        {
            if (dataTableColumns == null)
            {
                dataTableColumns = new List<DataTableColumn>
                {
                    new DataTableColumn("id", "ID") { Width = 50, Sortable = true },
                    new DataTableColumn("name", "Name") { Width = 150, Sortable = true },
                    new DataTableColumn("role", "Role") { Width = 100, Sortable = true },
                };

                dataTableData = new List<DataTableRow>();
                for (int i = 0; i < 20; i++)
                {
                    var row = new DataTableRow(i.ToString());
                    row.SetData("id", i.ToString());
                    row.SetData("name", $"User {i}");
                    row.SetData("role", i % 3 == 0 ? "Admin" : "User");
                    dataTableData.Add(row);
                }

                columnWidths = new float[] { 50, 150, 100 };
            }

            DrawSection(
                "Sortable Table",
                () =>
                {
                    guiHelper.SortableTable(dataTableColumns.Select(c => c.Header).ToArray(), ConvertData(dataTableData), ref sortColumns, ref sortAscending, ControlVariant.Default, ControlSize.Default, (col, asc) => Debug.Log($"Sort {col} {asc}"));
                }
            );

            DrawSection(
                "Selectable Table",
                () =>
                {
                    if (selectedRows.Length != dataTableData.Count)
                        selectedRows = new bool[dataTableData.Count];
                    guiHelper.SelectableTable(dataTableColumns.Select(c => c.Header).ToArray(), ConvertData(dataTableData.Take(5).ToList()), ref selectedRows, ControlVariant.Default, ControlSize.Default, (idx, sel) => Debug.Log($"Select {idx} {sel}"));
                }
            );

            DrawSection(
                "Paginated Table",
                () =>
                {
                    guiHelper.PaginatedTable(dataTableColumns.Select(c => c.Header).ToArray(), ConvertData(dataTableData), ref currentPage, 5, ControlVariant.Default, ControlSize.Default, page => Debug.Log($"Page {page}"));
                }
            );

            DrawSection(
                "Searchable Table",
                () =>
                {
                    guiHelper.SearchableTable(dataTableColumns.Select(c => c.Header).ToArray(), ConvertData(dataTableData), ref searchQuery, ref filteredData, ControlVariant.Default, ControlSize.Default, query => Debug.Log($"Search {query}"));
                }
            );

            DrawSection(
                "Resizable Table",
                () =>
                {
                    guiHelper.ResizableTable(dataTableColumns.Select(c => c.Header).ToArray(), ConvertData(dataTableData.Take(5).ToList()), ref columnWidths, ControlVariant.Default, ControlSize.Default);
                }
            );

            DrawSection(
                "Comprehensive Data Table",
                () =>
                {
                    guiHelper.DrawDataTable("main_datatable", dataTableColumns, dataTableData, showPagination: true, showSearch: true, showSelection: true, showColumnToggle: true);

                    var selected = guiHelper.GetSelectedRows("main_datatable");
                    if (selected.Count > 0)
                    {
                        guiHelper.Label($"Selected IDs: {string.Join(", ", selected)}");
                    }
                }
            );
        }

        private string[,] ConvertData(List<DataTableRow> rows)
        {
            if (rows == null || rows.Count == 0)
                return new string[0, 0];
            string[,] result = new string[rows.Count, 3];
            for (int i = 0; i < rows.Count; i++)
            {
                result[i, 0] = rows[i].GetValue<string>("id");
                result[i, 1] = rows[i].GetValue<string>("name");
                result[i, 2] = rows[i].GetValue<string>("role");
            }
            return result;
        }

        void DrawLayoutDemos()
        {
            DrawSection(
                "Layout Groups",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Left");
                    GUILayout.FlexibleSpace();
                    guiHelper.Button("Right");
                    guiHelper.EndHorizontalGroup();
                }
            );
        }
        #endregion
    }
}
