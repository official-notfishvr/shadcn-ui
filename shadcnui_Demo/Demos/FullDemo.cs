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
        private Rect windowRect = new Rect(20, 20, 1450, 700);
        private bool showDemoWindow = false;
        private bool useVerticalTabs = false;
        private Vector2 scrollPosition;

        private bool checkboxValue = false;
        private bool switchValue = false;
        private string textAreaValue = "";
        private int selectedTab = 0;
        private Tabs.TabConfig[] demoTabs;
        private int currentDemoTab = 0;
        private bool verticalTabsOnRight = false;
        private bool tabsOnBottom = false;

        private string passwordValue = "password123";
        private string inputTextAreaValue = "This is a text area in Input Components.";

        private bool defaultSwitchVariant = false;
        private bool outlineSwitchVariant = false;
        private bool ghostSwitchVariant = false;
        private bool defaultSwitchSize = false;
        private bool smallSwitchSize = false;
        private bool largeSwitchSize = false;
        private bool defaultVariant = false;
        private bool largeSize = false;
        private bool smallSize = false;
        private bool defaultSize = false;
        private bool ghostVariant = false;
        private bool outlineVariant = false;

        private bool defaultVariantToggle = false;
        private bool outlineVariantToggle = false;
        private bool defaultSizeToggle = false;
        private bool smallSizeToggle = false;
        private bool largeSizeToggle = false;
        private bool defaultToggleValue = false;

        private bool optionAValue = false;
        private bool optionBValue = true;

        private string outlineTextAreaValue = "Outline Text Area";
        private string ghostTextAreaValue = "Ghost Text Area";
        private string labeledTextAreaValue = "Labeled Text Area";
        private float resizableTextAreaHeight = 100f;
        private string resizableTextAreaValue = "Resizable Text Area";

        private int selectedVerticalTab = 0;
        private int selectDemoCurrentSelection = 0;
        private List<ChartSeries> chartSeries;
        private bool dropdownOpen = false;

        private System.Collections.Generic.List<DropdownMenuItem> dropdownMenuItems;

        private DateTime? calendarSelectedDate;
        private System.Collections.Generic.List<DateTime> calendarDisabledDates = new System.Collections.Generic.List<DateTime>();

        private DateTime? selectedDate;
        private DateTime? selectedDateWithLabel;

        private System.Collections.Generic.List<DataTableColumn> dataTableColumns;
        private System.Collections.Generic.List<DataTableRow> dataTableData;

        private Texture2D img = new Texture2D(2, 2);
        private int componentCount = 0;

        void Start()
        {
            guiHelper = new GUIHelper();

            byte[] imageData = LoadEmbeddedBytes("shadcnui_Demo.Img.1.png");
#if MONO
            img.LoadImage(imageData);
#endif
            /*
            // Custom Font Loading

            byte[] fontData = LoadEmbeddedBytes("shadcnui_Demo.Fonts.ProggyClean.ttf"); // {file name}.Fonts.{font name}.ttf // you need Directory.Build.targets to do it or fully find the name yourself

            if (fontData != null)
            {
                guiHelper.GetStyleManager().SetCustomFont(fontData, "ProggyClean.ttf");
            }

            */
            demoTabs = new Tabs.TabConfig[]
            {
                new Tabs.TabConfig("Avatar", DrawAvatarDemos),
                new Tabs.TabConfig("Badge", DrawBadgeDemos),
                new Tabs.TabConfig("Button", DrawButtonDemos),
                new Tabs.TabConfig("Card", DrawCardDemos),
                new Tabs.TabConfig("Checkbox", DrawCheckboxDemos),
                new Tabs.TabConfig("DataTable", DrawDataTableDemos),
                new Tabs.TabConfig("DatePicker", DrawDatePickerDemos),
                new Tabs.TabConfig("Dialog", DrawDialogDemos),
                new Tabs.TabConfig("Input", DrawInputDemos),
                new Tabs.TabConfig("Label", DrawLabelDemos),
                new Tabs.TabConfig("Layout", DrawLayoutDemos),
                new Tabs.TabConfig("Progress", DrawProgressDemos),
                new Tabs.TabConfig("Separator", DrawSeparatorDemos),
                new Tabs.TabConfig("Switch", DrawSwitchDemos),
                new Tabs.TabConfig("Table", DrawTableDemos),
                new Tabs.TabConfig("Tabs", DrawTabsDemos),
                new Tabs.TabConfig("TextArea", DrawTextAreaDemos),
                new Tabs.TabConfig("Toggle", DrawToggleDemos),
                new Tabs.TabConfig("Calendar", DrawCalendarDemos),
                new Tabs.TabConfig("DropdownMenu", DrawDropdownMenuDemos),
                new Tabs.TabConfig("Popover", DrawPopoverDemos),
                new Tabs.TabConfig("Select", DrawSelectDemos),
                new Tabs.TabConfig("Chart", DrawChartDemos),
                new Tabs.TabConfig("MenuBar", DrawMenuBar),
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
                    {
                        Debug.LogError($"Embedded resource not found: {path}");
                        return null;
                    }
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading embedded resource '{path}': {ex.Message}");
                return null;
            }
        }

        void OnGUI()
        {
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
                windowRect = GUI.Window(101, windowRect, (GUI.WindowFunction)DrawDemoWindow, "shadcn/ui Demo - Dark Theme");
            }
        }

        void DrawDemoWindow(int windowID)
        {
            guiHelper.UpdateAnimations(showDemoWindow);
            if (guiHelper.BeginAnimatedGUI())
            {
                guiHelper.Label("shadcn/ui Library", LabelVariant.Default);
                guiHelper.MutedLabel("Explore all available components and their variations");
                guiHelper.HorizontalSeparator();

                guiHelper.BeginHorizontalGroup();
                useVerticalTabs = guiHelper.Toggle("Vertical Tabs", useVerticalTabs);
                GUILayout.Space(20);
                if (useVerticalTabs)
                {
                    verticalTabsOnRight = guiHelper.Toggle("Tabs on Right", verticalTabsOnRight);
                }
                else
                {
                    tabsOnBottom = guiHelper.Toggle("Tabs on Bottom", tabsOnBottom);
                }
                guiHelper.EndHorizontalGroup();
                guiHelper.HorizontalSeparator();

                if (useVerticalTabs)
                {
                    var side = verticalTabsOnRight ? Tabs.TabSide.Right : Tabs.TabSide.Left;
                    currentDemoTab = guiHelper.VerticalTabs(
                        demoTabs.Select(tab => tab.Name).ToArray(),
                        currentDemoTab,
                        () =>
                        {
                            scrollPosition = guiHelper.DrawScrollView(scrollPosition, DrawCurrentTabContent, GUILayout.Height(400));
                        },
                        tabWidth: 150,
                        maxLines: 2,
                        side: side
                    );
                }
                else
                {
                    var position = tabsOnBottom ? Tabs.TabPosition.Bottom : Tabs.TabPosition.Top;
                    currentDemoTab = guiHelper.DrawTabs(
                        demoTabs.Select(tab => tab.Name).ToArray(),
                        currentDemoTab,
                        () =>
                        {
                            scrollPosition = guiHelper.DrawScrollView(scrollPosition, DrawCurrentTabContent, GUILayout.Height(400));
                        },
                        maxLines: 2,
                        position: position
                    );
                }
                guiHelper.EndAnimatedGUI();
            }
            GUI.DragWindow();
        }

        void DrawCurrentTabContent()
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
        }

        void DrawInputDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Input", LabelVariant.Default);
            guiHelper.MutedLabel("Form input fields and components for user interaction");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Section Header", LabelVariant.Default);
            guiHelper.DrawSectionHeader("Configuration");
            guiHelper.Label("Code: guiHelper.DrawSectionHeader(\"Configuration\");", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Rendered Label", LabelVariant.Default);
            guiHelper.RenderLabel("User Input Example", 200);
            guiHelper.Label("Code: guiHelper.RenderLabel(text, width);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Password Field", LabelVariant.Default);
            passwordValue = guiHelper.DrawPasswordField(300, "Enter Password", ref passwordValue);
            guiHelper.Label($"Password Set: {(passwordValue.Length > 0 ? "Yes" : "No")}");
            guiHelper.Label("Code: guiHelper.DrawPasswordField(width, label, ref password);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Text Area", LabelVariant.Default);
            guiHelper.DrawTextArea(300, "Additional Notes", ref inputTextAreaValue, 200, 80);
            guiHelper.Label($"Content Length: {inputTextAreaValue.Length}");
            guiHelper.Label("Code: guiHelper.DrawTextArea(width, label, ref text, maxLength, height);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawTabsDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            tabsOnBottom = guiHelper.Toggle("Tabs on Bottom", tabsOnBottom);
            string[] tabNames = { "General", "Security", "Preferences" };
            var position = tabsOnBottom ? Tabs.TabPosition.Bottom : Tabs.TabPosition.Top;

            selectedTab = guiHelper.DrawTabs(
                tabNames,
                selectedTab,
                () =>
                {
                    guiHelper.BeginTabContent();
                    if (selectedTab == 0)
                        guiHelper.Label("General settings and configuration options for your account.");
                    else if (selectedTab == 1)
                        guiHelper.Label("Manage your password, two-factor authentication, and security settings.");
                    else
                        guiHelper.Label("Customize your notification preferences and display options.");
                    guiHelper.EndTabContent();
                },
                position: position
            );
            guiHelper.Label("Code: selectedTab = guiHelper.DrawTabs(names, selected, content);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Tabs with Content");
            Tabs.TabConfig[] tabConfigs = new Tabs.TabConfig[]
            {
                new Tabs.TabConfig("Overview", () => guiHelper.Label("Dashboard overview and statistics")),
                new Tabs.TabConfig("Details", () => guiHelper.Label("Detailed information and analytics")),
                new Tabs.TabConfig("Settings", () => guiHelper.Label("Advanced configuration options")),
            };
            guiHelper.TabsWithContent(tabConfigs, selectedTab, null);
            guiHelper.Label("Code: guiHelper.TabsWithContent(configs, selected);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Vertical Tabs");
            string[] verticalTabNames = { "Profile", "Settings", "Privacy" };
            selectedVerticalTab = guiHelper.VerticalTabs(
                verticalTabNames,
                selectedVerticalTab,
                () =>
                {
                    guiHelper.BeginTabContent();
                    if (selectedVerticalTab == 0)
                        guiHelper.Label("Manage your profile information and personal details.");
                    else if (selectedVerticalTab == 1)
                        guiHelper.Label("Configure application settings and preferences.");
                    else
                        guiHelper.Label("Control your privacy and data sharing preferences.");
                    guiHelper.EndTabContent();
                },
                tabWidth: 100
            );
            guiHelper.Label("Code: guiHelper.VerticalTabs(names, selected, content, tabWidth);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();
            GUILayout.EndVertical();
        }

        void DrawTextAreaDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            textAreaValue = guiHelper.TextArea(textAreaValue, placeholder: "Enter your message here", minHeight: 100);
            guiHelper.Label($"Characters: {textAreaValue.Length}");
            guiHelper.Label("Code: guiHelper.TextArea(value, placeholder, minHeight);");
            guiHelper.HorizontalSeparator();

            outlineTextAreaValue = guiHelper.OutlineTextArea(outlineTextAreaValue, placeholder: "Outline style text area");
            guiHelper.Label($"Length: {outlineTextAreaValue.Length}");
            guiHelper.Label("Code: guiHelper.OutlineTextArea(text, placeholder);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            ghostTextAreaValue = guiHelper.GhostTextArea(ghostTextAreaValue, placeholder: "Ghost style text area");
            guiHelper.Label($"Length: {ghostTextAreaValue.Length}");
            guiHelper.Label("Code: guiHelper.GhostTextArea(text, placeholder);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            labeledTextAreaValue = guiHelper.LabeledTextArea("Feedback", labeledTextAreaValue, placeholder: "Share your feedback");
            guiHelper.Label($"Length: {labeledTextAreaValue.Length}");
            guiHelper.Label("Code: guiHelper.LabeledTextArea(label, text, placeholder);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            resizableTextAreaValue = guiHelper.ResizableTextArea(resizableTextAreaValue, ref resizableTextAreaHeight, placeholder: "Resize this area");
            guiHelper.Label($"Height: {resizableTextAreaHeight:F2}, Length: {resizableTextAreaValue.Length}");
            guiHelper.Label("Code: guiHelper.ResizableTextArea(text, ref height, placeholder);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();
            GUILayout.EndVertical();
        }

        void DrawAvatarDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Avatar", LabelVariant.Default);
            guiHelper.MutedLabel("Image element with fallback for representing users");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Default Sizes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Avatar(img, "AV");
            guiHelper.Avatar(img, "SM", AvatarSize.Small);
            guiHelper.Avatar(img, "LG", AvatarSize.Large);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Avatar(texture, fallback, size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Avatar Shapes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Avatar(img, "CR", AvatarSize.Default, AvatarShape.Circle);
            guiHelper.Avatar(img, "SQ", AvatarSize.Default, AvatarShape.Square);
            guiHelper.Avatar(img, "RD", AvatarSize.Default, AvatarShape.Rounded);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Avatar(texture, fallback, size, shape);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Avatar with Status", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.AvatarWithStatus(img, "ON", true);
            guiHelper.AvatarWithStatus(img, "OF", false);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.AvatarWithStatus(image, fallback, isOnline);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Avatar with Name", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.AvatarWithName(img, "JD", "John Doe");
            guiHelper.AvatarWithName(img, "JS", "Jane Smith", showNameBelow: true);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.AvatarWithName(image, fallback, name);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Avatar with Border", LabelVariant.Default);
            guiHelper.AvatarWithBorder(img, "BR", Color.cyan);
            guiHelper.Label("Code: guiHelper.AvatarWithBorder(image, fallback, color);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawBadgeDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Badge", LabelVariant.Default);
            guiHelper.MutedLabel("Display badges, tags, or status indicators");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Badge Variants", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Badge("Default");
            guiHelper.Badge("Secondary", BadgeVariant.Secondary);
            guiHelper.Badge("Destructive", BadgeVariant.Destructive);
            guiHelper.Badge("Outline", BadgeVariant.Outline);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Badge(text, variant);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Badge Sizes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Badge("Default", size: BadgeSize.Default);
            guiHelper.Badge("Small", size: BadgeSize.Small);
            guiHelper.Badge("Large", size: BadgeSize.Large);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Badge(text, size: size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Count Badge", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.CountBadge(5);
            guiHelper.CountBadge(105, maxCount: 99);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.CountBadge(count, maxCount);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Status Badge", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.StatusBadge("Active", true);
            guiHelper.StatusBadge("Inactive", false);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.StatusBadge(text, isActive);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Progress Badge", LabelVariant.Default);
            guiHelper.ProgressBadge("Loading", 0.7f);
            guiHelper.Label("Code: guiHelper.ProgressBadge(text, progress);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Animated Badge", LabelVariant.Default);
            guiHelper.AnimatedBadge("Processing");
            guiHelper.Label("Code: guiHelper.AnimatedBadge(text);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Rounded Badge", LabelVariant.Default);
            guiHelper.RoundedBadge("Rounded", cornerRadius: 8f);
            guiHelper.Label("Code: guiHelper.RoundedBadge(text, cornerRadius);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawCalendarDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Calendar", LabelVariant.Default);
            guiHelper.MutedLabel("Select dates with an interactive calendar interface");
            guiHelper.HorizontalSeparator();

            var calendar = guiHelper.GetCalendarComponents();
            calendar.SelectedDate = calendarSelectedDate;
            calendar.DisabledDates = calendarDisabledDates;
            calendar.OnDateSelected += (date) =>
            {
                calendarSelectedDate = date;
            };

            guiHelper.Calendar();
            guiHelper.HorizontalSeparator();

            guiHelper.Label($"Selected: {(calendarSelectedDate.HasValue ? calendarSelectedDate.Value.ToString("MMM dd, yyyy") : "None")}");

            if (calendar.Ranges.Count > 0)
            {
                for (int i = 0; i < calendar.Ranges.Count; i++)
                {
                    var r = calendar.Ranges[i];
                    guiHelper.Label($"Range {i}: {r.Start:MMM dd} - {r.End:MMM dd}");
                }
            }
            else
            {
                guiHelper.Label("No range selected");
            }

            if (guiHelper.Button("Disable Today"))
            {
                if (!calendarDisabledDates.Contains(DateTime.Today))
                {
                    calendarDisabledDates.Add(DateTime.Today);
                }
            }

            guiHelper.Label("Code: calendar.OnDateSelected += handler;", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();
            GUILayout.EndVertical();
        }

        void DrawDropdownMenuDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Dropdown Menu", LabelVariant.Default);
            guiHelper.MutedLabel("Hierarchical menu that appears on trigger");
            guiHelper.HorizontalSeparator();

            if (guiHelper.Button("Open Menu"))
            {
                dropdownOpen = !dropdownOpen;
            }

            if (dropdownOpen)
            {
                dropdownMenuItems = new System.Collections.Generic.List<DropdownMenuItem>()
                {
                    new DropdownMenuItem(DropdownMenuItemType.Header, "Account"),
                    new DropdownMenuItem(
                        DropdownMenuItemType.Item,
                        "Profile",
                        () =>
                        {
                            Debug.Log("Profile selected");
                            dropdownOpen = false;
                        }
                    ),
                    new DropdownMenuItem(
                        DropdownMenuItemType.Item,
                        "Billing",
                        () =>
                        {
                            Debug.Log("Billing selected");
                            dropdownOpen = false;
                        }
                    ),
                    new DropdownMenuItem(
                        DropdownMenuItemType.Item,
                        "Settings",
                        () =>
                        {
                            Debug.Log("Settings selected");
                            dropdownOpen = false;
                        }
                    ),
                    new DropdownMenuItem(DropdownMenuItemType.Separator),
                    new DropdownMenuItem(DropdownMenuItemType.Item, "Workspace")
                    {
                        SubItems = new System.Collections.Generic.List<DropdownMenuItem>()
                        {
                            new DropdownMenuItem(
                                DropdownMenuItemType.Item,
                                "By Email",
                                () =>
                                {
                                    Debug.Log("Email selected");
                                    dropdownOpen = false;
                                }
                            ),
                            new DropdownMenuItem(
                                DropdownMenuItemType.Item,
                                "By Link",
                                () =>
                                {
                                    Debug.Log("Link selected");
                                    dropdownOpen = false;
                                }
                            ),
                        },
                    },
                    new DropdownMenuItem(DropdownMenuItemType.Separator),
                    new DropdownMenuItem(
                        DropdownMenuItemType.Item,
                        "Sign Out",
                        () =>
                        {
                            Debug.Log("Signed out");
                            dropdownOpen = false;
                        }
                    ),
                };
                guiHelper.DropdownMenu(new DropdownMenuConfig(dropdownMenuItems));
            }

            guiHelper.Label("Code: guiHelper.DropdownMenu(config);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();
            GUILayout.EndVertical();
        }

        void DrawPopoverDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Popover", LabelVariant.Default);
            guiHelper.MutedLabel("Pop-up for displaying contextual information");
            guiHelper.HorizontalSeparator();

            if (guiHelper.Button("Show Popover"))
            {
                guiHelper.OpenPopover();
            }

            if (guiHelper.IsPopoverOpen())
            {
                guiHelper.Popover(() =>
                {
                    guiHelper.Label("Popover Title", LabelVariant.Default);
                    guiHelper.Label("This is contextual information displayed in a popover.");
                });
            }

            guiHelper.Label("Code: guiHelper.Popover(content);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();
            GUILayout.EndVertical();
        }

        void DrawSelectDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Select", LabelVariant.Default);
            guiHelper.MutedLabel("Dropdown list for selecting from options");
            guiHelper.HorizontalSeparator();

            string[] options = { "Option 1", "Option 2", "Option 3", "Option 4" };

            if (guiHelper.Button("Open Select"))
            {
                guiHelper.OpenSelect();
            }

            if (guiHelper.IsSelectOpen())
            {
                selectDemoCurrentSelection = guiHelper.Select(options, selectDemoCurrentSelection);
            }

            guiHelper.Label($"Selected: {(selectDemoCurrentSelection < options.Length ? options[selectDemoCurrentSelection] : "None")}");
            guiHelper.Label("Code: guiHelper.Select(options, selectedIndex);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();
            GUILayout.EndVertical();
        }

        void DrawButtonDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.Width(windowRect.width - 25), GUILayout.ExpandHeight(true));
            guiHelper.Label("Button", LabelVariant.Default);
            guiHelper.MutedLabel("Interactive button element that triggers actions");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Button Variants", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Default");
            guiHelper.Button("Destructive", ButtonVariant.Destructive);
            guiHelper.Button("Outline", ButtonVariant.Outline);
            guiHelper.Button("Secondary", ButtonVariant.Secondary);
            guiHelper.Button("Ghost", ButtonVariant.Ghost);
            guiHelper.Button("Link", ButtonVariant.Link);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Button(label, variant);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Button Sizes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Default", ButtonVariant.Default, ButtonSize.Default);
            guiHelper.Button("Small", ButtonVariant.Default, ButtonSize.Small);
            guiHelper.Button("Large", ButtonVariant.Default, ButtonSize.Large);
            guiHelper.Button("Icon", ButtonVariant.Default, ButtonSize.Icon);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Button(label, variant, size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Button States", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Normal");
            guiHelper.Button("Disabled", disabled: true);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.Button(label, disabled: true);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Direct Button Calls", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.DestructiveButton("Delete", () => Debug.Log("Delete clicked"));
            guiHelper.OutlineButton("Cancel", () => Debug.Log("Cancel clicked"));
            guiHelper.SecondaryButton("More", () => Debug.Log("More clicked"));
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.DestructiveButton(...); etc.", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Sized Buttons", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.SmallButton("Small", () => Debug.Log("Small"));
            guiHelper.LargeButton("Large", () => Debug.Log("Large"));
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.SmallButton(...); guiHelper.LargeButton(...);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Button Group", LabelVariant.Default);
            guiHelper.ButtonGroup(() =>
            {
                guiHelper.Button("Save");
                guiHelper.Button("Draft");
                guiHelper.Button("Publish");
            });
            guiHelper.Label("Code: guiHelper.ButtonGroup(() => { ... });", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawCardDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Card", LabelVariant.Default);
            guiHelper.MutedLabel("Flexible content container with sections");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Standard Card", LabelVariant.Default);
            guiHelper.DrawCard(
                "Create Project",
                "Deploy with one-click.",
                "Choose from templates or start fresh. Includes CI/CD, monitoring, and auto-scaling.",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.Button("Cancel", ButtonVariant.Outline))
                        Debug.Log("Cancelled");
                    GUILayout.Space(8);
                    if (guiHelper.Button("Deploy", ButtonVariant.Default))
                        Debug.Log("Deployed");
                    guiHelper.EndHorizontalGroup();
                },
                400,
                180
            );
            guiHelper.Label("Code: guiHelper.DrawCard(title, subtitle, content, footer, width, height);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("User Card", LabelVariant.Default);
            guiHelper.BeginCard(350, 220);
            guiHelper.CardHeader(() =>
            {
                guiHelper.BeginHorizontalGroup();
                guiHelper.Avatar(null, "AD", AvatarSize.Default);
                GUILayout.Space(12);
                guiHelper.BeginVerticalGroup();
                guiHelper.CardTitle("Alex Developer");
                guiHelper.CardDescription("Senior Engineer");
                guiHelper.Badge("Expert", BadgeVariant.Default, BadgeSize.Small);
                guiHelper.EndVerticalGroup();
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.CardContent(() =>
            {
                guiHelper.Label("Email: alex@company.com");
                guiHelper.Label("Location: New York, USA");
                guiHelper.Label("Company: Tech Corp");
                GUILayout.Space(8);
                guiHelper.Label("Full-stack developer with 10+ years experience in C# and Unity.");
            });
            guiHelper.CardFooter(() =>
            {
                guiHelper.BeginHorizontalGroup();
                if (guiHelper.Button("Contact", ButtonVariant.Outline, ButtonSize.Small))
                    Debug.Log("Contact clicked");
                GUILayout.Space(8);
                if (guiHelper.Button("View", ButtonVariant.Default, ButtonSize.Small))
                    Debug.Log("View clicked");
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();
            guiHelper.Label("Code: guiHelper.BeginCard(); CardHeader/Content/Footer(); EndCard();", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Stat Cards", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();

            guiHelper.BeginCard(160, 100);
            guiHelper.CardContent(() =>
            {
                guiHelper.Label("Revenue", LabelVariant.Muted);
                guiHelper.Label("$125,430", LabelVariant.Default);
                guiHelper.BeginHorizontalGroup();
                guiHelper.Label("+12%", LabelVariant.Default);
                guiHelper.Label("this month", LabelVariant.Muted);
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();

            GUILayout.Space(12);

            guiHelper.BeginCard(160, 100);
            guiHelper.CardContent(() =>
            {
                guiHelper.Label("Users", LabelVariant.Muted);
                guiHelper.Label("8,542", LabelVariant.Default);
                guiHelper.BeginHorizontalGroup();
                guiHelper.Label("+24%", LabelVariant.Default);
                guiHelper.Label("this month", LabelVariant.Muted);
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();

            GUILayout.Space(12);

            guiHelper.BeginCard(160, 100);
            guiHelper.CardContent(() =>
            {
                guiHelper.Label("Orders", LabelVariant.Muted);
                guiHelper.Label("1,234", LabelVariant.Default);
                guiHelper.BeginHorizontalGroup();
                guiHelper.Label("+8%", LabelVariant.Default);
                guiHelper.Label("this month", LabelVariant.Muted);
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();

            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: Multiple cards for dashboard statistics", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Simple Card", LabelVariant.Default);
            guiHelper.DrawSimpleCard("Quick information display. Perfect for basic content without headers or footers. Use for notes or simple containers.", 400, 80);
            guiHelper.Label("Code: guiHelper.DrawSimpleCard(content, width, height);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawCheckboxDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Checkbox", LabelVariant.Default);
            guiHelper.MutedLabel("Toggle control for binary choices");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Basic Checkbox");
            checkboxValue = guiHelper.Checkbox("Remember my preference", checkboxValue);
            guiHelper.Label($"Status: {(checkboxValue ? "Checked" : "Unchecked")}");
            guiHelper.Label("Code: value = guiHelper.Checkbox(label, value);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Checkbox Variants");
            guiHelper.BeginHorizontalGroup();
            defaultVariant = guiHelper.Checkbox("Default", defaultVariant, CheckboxVariant.Default);
            outlineVariant = guiHelper.Checkbox("Outline", outlineVariant, CheckboxVariant.Outline);
            ghostVariant = guiHelper.Checkbox("Ghost", ghostVariant, CheckboxVariant.Ghost);
            GUILayout.EndHorizontal();
            guiHelper.Label("Code: guiHelper.Checkbox(label, value, variant);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Checkbox Sizes");
            guiHelper.BeginHorizontalGroup();
            defaultSize = guiHelper.Checkbox("Default", defaultSize, size: CheckboxSize.Default);
            smallSize = guiHelper.Checkbox("Small", smallSize, size: CheckboxSize.Small);
            largeSize = guiHelper.Checkbox("Large", largeSize, size: CheckboxSize.Large);
            GUILayout.EndHorizontal();
            guiHelper.Label("Code: guiHelper.Checkbox(label, value, size: size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawLabelDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Label", LabelVariant.Default);
            guiHelper.MutedLabel("Non-interactive text display element");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Label Variants", LabelVariant.Default);
            guiHelper.Label("This is a default label.", LabelVariant.Default);
            guiHelper.SecondaryLabel("This is secondary text.");
            guiHelper.MutedLabel("This is muted text.");
            guiHelper.DestructiveLabel("This is destructive text.");
            guiHelper.Label("Code: guiHelper.Label(text); guiHelper.SecondaryLabel(text); etc.", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawLayoutDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Layout", LabelVariant.Default);
            guiHelper.MutedLabel("Components for organizing and spacing elements");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Horizontal Layout", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Left");
            guiHelper.Button("Center");
            guiHelper.Button("Right");
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: BeginHorizontalGroup(); EndHorizontalGroup();", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Vertical Layout", LabelVariant.Default);
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true));
            optionAValue = guiHelper.Checkbox("Enable Feature A", optionAValue);
            optionBValue = guiHelper.Checkbox("Enable Feature B", optionBValue);
            guiHelper.EndVerticalGroup();
            guiHelper.Label("Code: BeginVerticalGroup(); EndVerticalGroup();", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Spacing", LabelVariant.Default);
            guiHelper.Label("Text before space");
            guiHelper.AddSpace(15);
            guiHelper.Label("Text after space (15px gap)");
            guiHelper.Label("Code: guiHelper.AddSpace(pixels);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawProgressDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Progress", LabelVariant.Default);
            guiHelper.MutedLabel("Display task completion progress");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Progress Bar", LabelVariant.Default);
            guiHelper.LabeledProgress("Installation", 0.65f, 300, showPercentage: true);
            guiHelper.Label("Code: guiHelper.LabeledProgress(label, value, width, showPercentage);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Circular Progress", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.CircularProgress(0.33f, 50);
            guiHelper.CircularProgress(0.66f, 50);
            guiHelper.CircularProgress(1.0f, 50);
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.CircularProgress(value, size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawSeparatorDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Separator", LabelVariant.Default);
            guiHelper.MutedLabel("Visual separator between sections");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Horizontal Separator", LabelVariant.Default);
            guiHelper.Label("Section 1");
            guiHelper.HorizontalSeparator();
            guiHelper.Label("Section 2");
            guiHelper.Label("Code: guiHelper.HorizontalSeparator();", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Decorative Separator", LabelVariant.Default);
            guiHelper.Separator(SeparatorOrientation.Horizontal, true);
            guiHelper.Label("Code: guiHelper.Separator(Horizontal, true);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Vertical Separator", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("Left Side");
            guiHelper.VerticalSeparator();
            guiHelper.Label("Right Side");
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.VerticalSeparator();", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Separator with Spacing", LabelVariant.Default);
            guiHelper.Label("Content above");
            guiHelper.SeparatorWithSpacing(SeparatorOrientation.Horizontal, 12, 12);
            guiHelper.Label("Content below");
            guiHelper.Label("Code: guiHelper.SeparatorWithSpacing(Horizontal, 12, 12);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Labeled Separator", LabelVariant.Default);
            guiHelper.LabeledSeparator("OR");
            guiHelper.Label("Code: guiHelper.LabeledSeparator(\"OR\");", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawSwitchDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Switch", LabelVariant.Default);
            guiHelper.MutedLabel("Toggle control between on and off states");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Basic Switch");
            switchValue = guiHelper.Switch("Enable Notifications", switchValue);
            guiHelper.Label($"Status: {(switchValue ? "On" : "Off")}");
            guiHelper.Label("Code: value = guiHelper.Switch(label, value);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Switch Variants");
            guiHelper.BeginHorizontalGroup();
            defaultSwitchVariant = guiHelper.Switch("Default", defaultSwitchVariant, SwitchVariant.Default);
            outlineSwitchVariant = guiHelper.Switch("Outline", outlineSwitchVariant, SwitchVariant.Outline);
            ghostSwitchVariant = guiHelper.Switch("Ghost", ghostSwitchVariant, SwitchVariant.Ghost);
            GUILayout.EndHorizontal();
            guiHelper.Label("Code: guiHelper.Switch(label, value, variant);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Switch Sizes");
            guiHelper.BeginHorizontalGroup();
            defaultSwitchSize = guiHelper.Switch("Default", defaultSwitchSize, size: SwitchSize.Default);
            smallSwitchSize = guiHelper.Switch("Small", smallSwitchSize, size: SwitchSize.Small);
            largeSwitchSize = guiHelper.Switch("Large", largeSwitchSize, size: SwitchSize.Large);
            GUILayout.EndHorizontal();
            guiHelper.Label("Code: guiHelper.Switch(label, value, size: size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawTableDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Table", LabelVariant.Default);
            guiHelper.MutedLabel("Display tabular data with various styles");
            guiHelper.HorizontalSeparator();

            string[] headers = { "ID", "Status", "Payment", "Amount" };
            string[,] data =
            {
                { "001", "Paid", "Card", "$250" },
                { "002", "Pending", "PayPal", "$150" },
                { "003", "Unpaid", "Transfer", "$350" },
            };

            guiHelper.Label("Default Table", LabelVariant.Default);
            guiHelper.Table(headers, data);
            guiHelper.Label("Code: guiHelper.Table(headers, data);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Striped Table", LabelVariant.Default);
            guiHelper.Table(headers, data, TableVariant.Striped);
            guiHelper.Label("Code: guiHelper.Table(headers, data, TableVariant.Striped);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Bordered Table", LabelVariant.Default);
            guiHelper.Table(headers, data, TableVariant.Bordered);
            guiHelper.Label("Code: guiHelper.Table(headers, data, TableVariant.Bordered);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawToggleDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Toggle", LabelVariant.Default);
            guiHelper.MutedLabel("Two-state button that can be on or off");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Basic Toggle", LabelVariant.Default);
            defaultToggleValue = guiHelper.Toggle("Feature Enabled", defaultToggleValue);
            guiHelper.Label($"Status: {(defaultToggleValue ? "Enabled" : "Disabled")}");
            guiHelper.Label("Code: value = guiHelper.Toggle(text, value);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Toggle Variants", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            defaultVariantToggle = guiHelper.Toggle("Default", defaultVariantToggle, ToggleVariant.Default);
            outlineVariantToggle = guiHelper.Toggle("Outline", outlineVariantToggle, ToggleVariant.Outline);
            GUILayout.EndHorizontal();
            guiHelper.Label("Code: guiHelper.Toggle(text, value, variant);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Toggle Sizes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            defaultSizeToggle = guiHelper.Toggle("Default", defaultSizeToggle, size: ToggleSize.Default);
            smallSizeToggle = guiHelper.Toggle("Small", smallSizeToggle, size: ToggleSize.Small);
            largeSizeToggle = guiHelper.Toggle("Large", largeSizeToggle, size: ToggleSize.Large);
            GUILayout.EndHorizontal();
            guiHelper.Label("Code: guiHelper.Toggle(text, value, size: size);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawDialogDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Dialog", LabelVariant.Default);
            guiHelper.MutedLabel("Modal dialog for displaying content");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Standard Dialog", LabelVariant.Default);
            if (guiHelper.Button("Show Dialog"))
            {
                guiHelper.OpenDialog("main-dialog");
            }

            guiHelper.DrawDialog(
                "main-dialog",
                "Confirm Action",
                "Are you sure you want to continue?",
                () =>
                {
                    guiHelper.Label("This action cannot be undone.");
                },
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.Button("Cancel", ButtonVariant.Outline))
                        guiHelper.CloseDialog();
                    GUILayout.Space(8);
                    if (guiHelper.Button("Confirm", ButtonVariant.Default))
                    {
                        Debug.Log("Action confirmed");
                        guiHelper.CloseDialog();
                    }
                    guiHelper.EndHorizontalGroup();
                }
            );

            guiHelper.Label("Code: guiHelper.DrawDialog(id, title, description, content, footer);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Simple Dialog", LabelVariant.Default);
            if (guiHelper.Button("Show Simple"))
            {
                guiHelper.OpenDialog("simple");
            }

            guiHelper.DrawDialog(
                "simple",
                () =>
                {
                    guiHelper.Label("Information", LabelVariant.Default);
                    guiHelper.Label("This is a simple dialog without title or footer.");
                    guiHelper.HorizontalSeparator();
                    if (guiHelper.Button("Close"))
                        guiHelper.CloseDialog();
                }
            );

            guiHelper.Label("Code: guiHelper.DrawDialog(id, content);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawDatePickerDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Date Picker", LabelVariant.Default);
            guiHelper.MutedLabel("Select dates with interactive calendar");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Basic Date Picker", LabelVariant.Default);
            selectedDate = guiHelper.DatePicker("Select date", selectedDate, "picker-basic", GUILayout.Width(200));
            guiHelper.Label($"Selected: {(selectedDate.HasValue ? selectedDate.Value.ToString("MMM dd, yyyy") : "None")}");
            guiHelper.Label("Code: date = guiHelper.DatePicker(placeholder, date, id);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Labeled Date Picker", LabelVariant.Default);
            selectedDateWithLabel = guiHelper.DatePickerWithLabel("Event Date", "Pick date", selectedDateWithLabel, "picker-labeled", GUILayout.Width(200));
            guiHelper.Label($"Date: {(selectedDateWithLabel.HasValue ? selectedDateWithLabel.Value.ToString("MMM dd, yyyy") : "None")}");
            guiHelper.Label("Code: date = guiHelper.DatePickerWithLabel(label, placeholder, date, id);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Date Picker Actions", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            if (guiHelper.Button("Today"))
                selectedDate = DateTime.Today;
            if (guiHelper.Button("Clear"))
                selectedDate = null;
            guiHelper.EndHorizontalGroup();
            guiHelper.Label("Code: guiHelper.CloseDatePicker(id);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        void DrawDataTableDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Data Table", LabelVariant.Default);
            guiHelper.MutedLabel("Advanced table with sorting, filtering, pagination");
            guiHelper.HorizontalSeparator();

            if (dataTableColumns == null)
            {
                dataTableColumns = new System.Collections.Generic.List<DataTableColumn>();
                var idCol = guiHelper.CreateDataTableColumn("id", "ID");
                idCol.Width = 50;
                dataTableColumns.Add(idCol);

                var nameCol = guiHelper.CreateDataTableColumn("name", "Name");
                nameCol.Width = 140;
                dataTableColumns.Add(nameCol);

                var emailCol = guiHelper.CreateDataTableColumn("email", "Email");
                emailCol.Width = 180;
                dataTableColumns.Add(emailCol);

                var statusCol = guiHelper.CreateDataTableColumn("status", "Status");
                statusCol.Width = 100;
                dataTableColumns.Add(statusCol);

                dataTableData = new System.Collections.Generic.List<DataTableRow>();
                for (int i = 1; i <= 5; i++)
                {
                    var row = guiHelper.CreateDataTableRow(i.ToString());
                    row.SetData("id", i.ToString());
                    row.SetData("name", $"User {i}");
                    row.SetData("email", $"user{i}@example.com");
                    row.SetData("status", i % 2 == 0 ? "Active" : "Inactive");
                    dataTableData.Add(row);
                }
            }

            guiHelper.Label("Main Data Table", LabelVariant.Default);
            guiHelper.DrawDataTable("data-table", dataTableColumns, dataTableData, showPagination: true, showSearch: true, showSelection: true, showColumnToggle: false, GUILayout.Height(200));

            var selectedRows = guiHelper.GetSelectedRows("data-table");
            if (selectedRows.Count > 0)
            {
                guiHelper.Label($"Selected: {string.Join(", ", selectedRows)}", LabelVariant.Muted);
                if (guiHelper.Button("Clear Selection"))
                    guiHelper.ClearSelection("data-table");
            }

            guiHelper.Label("Code: guiHelper.DrawDataTable(id, columns, data, ...);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Simple Data Table", LabelVariant.Default);
            var simpleCols = new System.Collections.Generic.List<DataTableColumn>();
            var prodCol = guiHelper.CreateDataTableColumn("product", "Product");
            prodCol.Width = 140;
            simpleCols.Add(prodCol);

            var priceCol = guiHelper.CreateDataTableColumn("price", "Price");
            priceCol.Width = 100;
            simpleCols.Add(priceCol);

            var simpleData = new System.Collections.Generic.List<DataTableRow>
            {
                guiHelper.CreateDataTableRow("p1").SetData("product", "Laptop").SetData("price", "$1200"),
                guiHelper.CreateDataTableRow("p2").SetData("product", "Mouse").SetData("price", "$30"),
                guiHelper.CreateDataTableRow("p3").SetData("product", "Monitor").SetData("price", "$400"),
            };

            guiHelper.DrawDataTable("simple-table", simpleCols, simpleData, false, false, false, false, GUILayout.Height(120));
            guiHelper.Label("Code: guiHelper.DrawDataTable(id, columns, data, false, false, false, false);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            GUILayout.EndVertical();
        }

        private void DrawMenuBar()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            var menuItems = new List<MenuBar.MenuItem>
            {
                new MenuBar.MenuItem(
                    "File",
                    null,
                    false,
                    new List<MenuBar.MenuItem>
                    {
                        new MenuBar.MenuItem("Save", () => Debug.Log("Save"), false, null, "Ctrl+S"),
                        MenuBar.MenuItem.Separator(),
                        new MenuBar.MenuItem("Recent", null, false, new List<MenuBar.MenuItem> { new MenuBar.MenuItem("File1.txt", () => Debug.Log("Open File1")), new MenuBar.MenuItem("File2.txt", () => Debug.Log("Open File2")) }),
                        MenuBar.MenuItem.Separator(),
                        new MenuBar.MenuItem("Exit", () => Debug.Log("Exit")),
                    }
                ),
                new MenuBar.MenuItem(
                    "Edit",
                    null,
                    false,
                    new List<MenuBar.MenuItem>
                    {
                        new MenuBar.MenuItem("Cut", () => Debug.Log("Cut"), false, null, "Ctrl+X"),
                        new MenuBar.MenuItem("Copy", () => Debug.Log("Copy"), false, null, "Ctrl+C"),
                        new MenuBar.MenuItem("Paste", () => Debug.Log("Paste"), false, null, "Ctrl+V"),
                        MenuBar.MenuItem.Separator(),
                        MenuBar.MenuItem.Header("Advanced"),
                        new MenuBar.MenuItem("Find", () => Debug.Log("Find"), false, null, "Ctrl+F"),
                    }
                ),
                new MenuBar.MenuItem(
                    "View",
                    null,
                    false,
                    new List<MenuBar.MenuItem> { new MenuBar.MenuItem("Zoom In", () => Debug.Log("Zoom In"), false, null, "Ctrl++"), new MenuBar.MenuItem("Zoom Out", () => Debug.Log("Zoom Out"), false, null, "Ctrl+-"), new MenuBar.MenuItem("Reset", () => Debug.Log("Reset"), false, null, "Ctrl+0") }
                ),
                new MenuBar.MenuItem("Help", null, false, new List<MenuBar.MenuItem> { new MenuBar.MenuItem("Docs", () => Debug.Log("Docs")), new MenuBar.MenuItem("About", () => Debug.Log("About")) }),
            };

            guiHelper.MenuBar(new MenuBar.MenuBarConfig(menuItems));
            guiHelper.EndVerticalGroup();
        }

        void DrawChartDemos()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Chart", LabelVariant.Default);
            guiHelper.MutedLabel("Beautiful charts for data visualization");
            guiHelper.HorizontalSeparator();

            if (chartSeries == null)
            {
                chartSeries = new List<ChartSeries>();
                var desktop = new ChartSeries("desktop", "Desktop", new Color(0.149f, 0.388f, 0.922f));
                desktop.Data.Add(new ChartDataPoint("Jan", 186, new Color(0.149f, 0.388f, 0.922f)));
                desktop.Data.Add(new ChartDataPoint("Feb", 305, new Color(0.149f, 0.388f, 0.922f)));
                desktop.Data.Add(new ChartDataPoint("Mar", 237, new Color(0.149f, 0.388f, 0.922f)));
                desktop.Data.Add(new ChartDataPoint("Apr", 73, new Color(0.149f, 0.388f, 0.922f)));
                desktop.Data.Add(new ChartDataPoint("May", 209, new Color(0.149f, 0.388f, 0.922f)));
                desktop.Data.Add(new ChartDataPoint("Jun", 214, new Color(0.149f, 0.388f, 0.922f)));

                var mobile = new ChartSeries("mobile", "Mobile", new Color(0.376f, 0.647f, 0.980f));
                mobile.Data.Add(new ChartDataPoint("Jan", 80, new Color(0.376f, 0.647f, 0.980f)));
                mobile.Data.Add(new ChartDataPoint("Feb", 200, new Color(0.376f, 0.647f, 0.980f)));
                mobile.Data.Add(new ChartDataPoint("Mar", 120, new Color(0.376f, 0.647f, 0.980f)));
                mobile.Data.Add(new ChartDataPoint("Apr", 190, new Color(0.376f, 0.647f, 0.980f)));
                mobile.Data.Add(new ChartDataPoint("May", 130, new Color(0.376f, 0.647f, 0.980f)));
                mobile.Data.Add(new ChartDataPoint("Jun", 140, new Color(0.376f, 0.647f, 0.980f)));

                chartSeries.Add(desktop);
                chartSeries.Add(mobile);
            }

            guiHelper.BeginCard(600, 400);
            guiHelper.CardHeader(() =>
            {
                guiHelper.CardTitle("Bar Chart");
                guiHelper.CardDescription("Visitor trends over 6 months");
            });
            guiHelper.CardContent(() =>
            {
                guiHelper.Chart(new ChartConfig(chartSeries, ChartType.Bar) { Size = new Vector2(600, 350) });
            });
            guiHelper.EndCard();
            guiHelper.Label("Code: guiHelper.Chart(config, ChartType.Bar);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.BeginCard(600, 350);
            guiHelper.CardHeader(() =>
            {
                guiHelper.CardTitle("Line Chart");
                guiHelper.CardDescription("Trend analysis");
            });
            guiHelper.CardContent(() =>
            {
                guiHelper.Chart(new ChartConfig(chartSeries, ChartType.Line) { Size = new Vector2(600, 300) });
            });
            guiHelper.EndCard();
            guiHelper.Label("Code: guiHelper.Chart(config, ChartType.Line);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.BeginCard(600, 350);
            guiHelper.CardHeader(() =>
            {
                guiHelper.CardTitle("Area Chart");
                guiHelper.CardDescription("Stacked data visualization");
            });
            guiHelper.CardContent(() =>
            {
                guiHelper.Chart(new ChartConfig(chartSeries, ChartType.Area) { Size = new Vector2(600, 300) });
            });
            guiHelper.EndCard();
            guiHelper.Label("Code: guiHelper.Chart(config, ChartType.Area);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.BeginCard(450, 450);
            guiHelper.CardHeader(() =>
            {
                guiHelper.CardTitle("Pie Chart");
                guiHelper.CardDescription("Distribution breakdown");
            });
            guiHelper.CardContent(() =>
            {
                guiHelper.Chart(new ChartConfig(chartSeries, ChartType.Pie) { Size = new Vector2(400, 400) });
            });
            guiHelper.EndCard();
            guiHelper.Label("Code: guiHelper.Chart(config, ChartType.Pie);", LabelVariant.Muted);
            guiHelper.HorizontalSeparator();

            guiHelper.EndVerticalGroup();
        }
    }
}
