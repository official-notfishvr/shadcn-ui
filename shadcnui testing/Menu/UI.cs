using System;
using System.IO;
using System.Linq;
using System.Reflection;
using shadcnui;
using shadcnui.GUIComponents;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

public class UI : MonoBehaviour
{
    private GUIHelper guiHelper;
    private Rect windowRect = new Rect(20, 20, 1450, 600);
    private bool showDemoWindow = false;
    private bool useVerticalTabs = false;
    private Vector2 scrollPosition;

    private bool checkboxValue = false;
    private bool switchValue = false;
    private float sliderValue = 50f;
    private string textAreaValue = "";
    private int selectedTab = 0;
    private int selectedToggle = 0;
    private float progressValue = 0.3f;
    private int glowButtonIndex = 0;
    private Tabs.TabConfig[] demoTabs;
    private int currentDemoTab = 0;
    private bool verticalTabsOnRight = false;
    private bool tabsOnBottom = false;

    private string passwordValue = "password123";
    private string inputTextAreaValue = "This is a text area in Input Components.";
    private string glowInputFieldText = "Glow Input";
    private int glowInputFieldIdx = 0;
    private bool drawToggleValue = false;
    private bool drawCheckboxValue = false;
    private int selectionGridValue = 0;

    private bool defaultSwitchVariant = false;
    private bool outlineSwitchVariant = false;
    private bool ghostSwitchVariant = false;
    private bool defaultSwitchSize = false;
    private bool smallSwitchSize = false;
    private bool largeSwitchSize = false;
    private bool switchWithLabelValue = false;
    private bool[] switchGroupValues = { false, true, false };
    private bool switchWithDescriptionValue = false;
    private bool validatedSwitchValue = false;
    private bool switchWithTooltipValue = false;
    private bool switchWithIconValue = false;
    private bool switchWithLoadingValue = false;
    private bool customSwitchValue = false;
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
    private bool[] multiToggleGroupValues = { false, true, false };

    private bool checkboxWithLabelValue = false;
    private bool[] checkboxGroupValues = { false, true, false };
    private bool checkboxWithIconValue = false;
    private bool checkboxWithDescriptionValue = false;
    private bool validatedCheckboxValue = false;
    private bool checkboxWithTooltipValue = false;
    private bool customCheckboxValue = false;

    private bool optionAValue = false;
    private bool optionBValue = true;

    private bool expandableAlertExpanded = false;
    private bool dismissibleBadgeDismissed = false;

    private string outlineTextAreaValue = "Outline Text Area";
    private string ghostTextAreaValue = "Ghost Text Area";
    private string labeledTextAreaValue = "Labeled Text Area";
    private float resizableTextAreaHeight = 100f;
    private string resizableTextAreaValue = "Resizable Text Area";

    private int intSliderValue = 50;
    private float visualProgressBarValue = 0.7f;
    private int selectedVerticalTab = 0;
    private int selectDemoCurrentSelection = 0;
    private string inputFieldValue = "Default Input";
    private Vector2 scrollAreaScrollPosition = Vector2.zero;

    private System.Collections.Generic.List<DropdownMenuItem> dropdownMenuItems;

    private DateTime? calendarSelectedDate;
    private DateTime? calendarRangeStart;
    private DateTime? calendarRangeEnd;
    private System.Collections.Generic.List<DateTime> calendarDisabledDates = new System.Collections.Generic.List<DateTime>();

    // Dialog state
    private bool dialogOpen = false;

    // DatePicker state
    private DateTime? selectedDate;
    private DateTime? selectedDateWithLabel;

    // DataTable state
    private System.Collections.Generic.List<DataTableColumn> dataTableColumns;
    private System.Collections.Generic.List<DataTableRow> dataTableData;

    private bool fontInitialized = false;

    void Start()
    {
        guiHelper = new GUIHelper();
        /*
        // Custom Font Loading
        byte[] fontData = LoadEmbeddedFontBytes("shadcnui_testing.Fonts.ProggyClean.ttf"); // {file name}.Fonts.{font name}.ttf // you need Directory.Build.targets to do it or fully find the name yourself
        if (fontData != null)
        {
            guiHelper.GetStyleManager().SetCustomFont(fontData, "ProggyClean.ttf");
        }
        */
        demoTabs = new Tabs.TabConfig[]
        {
            new Tabs.TabConfig("Alert", DrawAlertDemos),
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
            new Tabs.TabConfig("Skeleton", DrawSkeletonDemos),
            new Tabs.TabConfig("Slider", DrawSliderDemos),
            new Tabs.TabConfig("Switch", DrawSwitchDemos),
            new Tabs.TabConfig("Table", DrawTableDemos),
            new Tabs.TabConfig("Tabs", DrawTabsDemos),
            new Tabs.TabConfig("Text Area", DrawTextAreaDemos),
            new Tabs.TabConfig("Toggle", DrawToggleDemos),
            new Tabs.TabConfig("Calendar", DrawCalendarDemos),
            new Tabs.TabConfig("DropdownMenu", DrawDropdownMenuDemos),
            new Tabs.TabConfig("Popover", DrawPopoverDemos),
            new Tabs.TabConfig("Select", DrawSelectDemos),
        };
    }

    private byte[] LoadEmbeddedFontBytes(string path)
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    Debug.LogError($"Embedded font resource not found: {path}. Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");
                    return null;
                }
                byte[] fontData = new byte[stream.Length];
                stream.Read(fontData, 0, fontData.Length);
                return fontData;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading embedded font resource '{path}': {ex.Message}");
            return null;
        }
    }

    void OnGUI()
    {
        GUI.skin.horizontalScrollbar = GUIStyle.none;
        GUI.skin.verticalScrollbar = GUIStyle.none;

        if (GUI.Button(new Rect(10, 10, 150, 30), "Open Demo Window"))
        {
            showDemoWindow = !showDemoWindow;
        }

        if (showDemoWindow)
        {
            windowRect = GUI.Window(101, windowRect, (GUI.WindowFunction)DrawDemoWindow, "shadcn/ui Demo");
        }
    }

    void DrawDemoWindow(int windowID)
    {
        guiHelper.UpdateAnimations(showDemoWindow);
        if (guiHelper.BeginAnimatedGUI())
        {
            useVerticalTabs = guiHelper.Toggle("Use Vertical Tabs", useVerticalTabs);
            guiHelper.HorizontalSeparator();

            if (useVerticalTabs)
            {
                verticalTabsOnRight = guiHelper.Toggle("Tabs on Right", verticalTabsOnRight);
                guiHelper.HorizontalSeparator();

                var side = verticalTabsOnRight ? Tabs.TabSide.Right : Tabs.TabSide.Left;

                currentDemoTab = guiHelper.VerticalTabs(
                    demoTabs.Select(tab => tab.Name).ToArray(),
                    currentDemoTab,
                    () =>
                    {
                        guiHelper.BeginTabContent();
                        scrollPosition = guiHelper.DrawScrollView(scrollPosition, DrawCurrentTabContent, GUILayout.ExpandHeight(true));
                        guiHelper.EndTabContent();
                    },
                    tabWidth: 150,
                    maxLines: 2,
                    side: side
                );
            }
            else
            {
                tabsOnBottom = guiHelper.Toggle("Tabs on Bottom", tabsOnBottom);
                guiHelper.HorizontalSeparator();
                var position = tabsOnBottom ? Tabs.TabPosition.Bottom : Tabs.TabPosition.Top;
                currentDemoTab = guiHelper.Tabs(
                    demoTabs.Select(tab => tab.Name).ToArray(),
                    currentDemoTab,
                    () =>
                    {
                        guiHelper.BeginTabContent();
                        scrollPosition = guiHelper.DrawScrollView(scrollPosition, DrawCurrentTabContent, GUILayout.ExpandHeight(true));
                        guiHelper.EndTabContent();
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
#if IL2CPP_MELONLOADER
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
        guiHelper.MutedLabel("Displays a form input field or a component that takes user input.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Section Header", LabelVariant.Default);
        guiHelper.DrawSectionHeader("Section Header");
        guiHelper.Label("Code: guiHelper.DrawSectionHeader(\"Section Header\");", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Rendered Label (width 150)", LabelVariant.Default);
        guiHelper.RenderLabel("Rendered Label (width 150)", 150);
        guiHelper.Label("Code: guiHelper.RenderLabel(\"Rendered Label (width 150)\", 150);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Password Field", LabelVariant.Default);
        passwordValue = guiHelper.DrawPasswordField(300, "Password", ref passwordValue);
        guiHelper.Label($"Password Value: {passwordValue}");
        guiHelper.Label("Code: guiHelper.DrawPasswordField(width, label, ref password);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Text Area Input", LabelVariant.Default);
        guiHelper.DrawTextArea(300, "Input Text Area", ref inputTextAreaValue, 200, 80);
        guiHelper.Label($"Input Text Area Value: {inputTextAreaValue}");
        guiHelper.Label("Code: guiHelper.DrawTextArea(width, label, ref text, maxLength, height);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawTabsDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        tabsOnBottom = guiHelper.Toggle("Tabs on Bottom", tabsOnBottom);
        string[] tabNames = { "Account", "Password", "Notifications" };
        var position = tabsOnBottom ? Tabs.TabPosition.Bottom : Tabs.TabPosition.Top;
        selectedTab = guiHelper.Tabs(
            tabNames,
            selectedTab,
            () =>
            {
                guiHelper.BeginTabContent();

                if (selectedTab == 0)
                    guiHelper.Label("Make changes to your account here.");
                else if (selectedTab == 1)
                    guiHelper.Label("Change your password here.");
                else
                    guiHelper.Label("Manage your notification settings here.");

                guiHelper.EndTabContent();
            },
            position: position
        );
        guiHelper.Label("Code: selectedTab = guiHelper.Tabs(tabNames, selectedTab, content, position);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Tabs with Content (using TabConfig)");
        Tabs.TabConfig[] tabConfigs = new Tabs.TabConfig[] { new Tabs.TabConfig("Tab A", () => guiHelper.Label("Content for Tab A.")), new Tabs.TabConfig("Tab B", () => guiHelper.Label("Content for Tab B.")), new Tabs.TabConfig("Tab C", () => guiHelper.Label("Content for Tab C.")) };
        guiHelper.TabsWithContent(tabConfigs, selectedTab);
        guiHelper.Label("Code: guiHelper.TabsWithContent(tabConfigs, selectedTab);", LabelVariant.Muted);
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
                    guiHelper.Label("Profile content.");
                else if (selectedVerticalTab == 1)
                    guiHelper.Label("Settings content.");
                else
                    guiHelper.Label("Privacy content.");

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

        textAreaValue = guiHelper.TextArea(textAreaValue, placeholder: "Type your message here.", minHeight: 100);
        guiHelper.Label("Code: textAreaValue = guiHelper.TextArea(value, placeholder, minHeight);");
        guiHelper.HorizontalSeparator();

        outlineTextAreaValue = guiHelper.OutlineTextArea(outlineTextAreaValue, placeholder: "Outline Text Area");
        guiHelper.Label($"Outline Text Area Value: {outlineTextAreaValue}");
        guiHelper.Label("Code: guiHelper.OutlineTextArea(text, placeholder);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        ghostTextAreaValue = guiHelper.GhostTextArea(ghostTextAreaValue, placeholder: "Ghost Text Area");
        guiHelper.Label($"Ghost Text Area Value: {ghostTextAreaValue}");
        guiHelper.Label("Code: guiHelper.GhostTextArea(text, placeholder);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        labeledTextAreaValue = guiHelper.LabeledTextArea("Your Message", labeledTextAreaValue, placeholder: "Type here...");
        guiHelper.Label($"Labeled Text Area Value: {labeledTextAreaValue}");
        guiHelper.Label("Code: guiHelper.LabeledTextArea(label, text, placeholder);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        resizableTextAreaValue = guiHelper.ResizableTextArea(resizableTextAreaValue, ref resizableTextAreaHeight, placeholder: "Resize me!");
        guiHelper.Label($"Resizable Text Area Value: {resizableTextAreaValue} (Height: {resizableTextAreaHeight:F2})");
        guiHelper.Label("Code: guiHelper.ResizableTextArea(text, ref height, placeholder);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawAlertDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Alert", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a callout for user attention.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Info Alert", LabelVariant.Default);
        guiHelper.Alert("Heads up!", "You can use this component to show a message to the user.", AlertVariant.Default, AlertType.Info);
        guiHelper.Label("Code: guiHelper.Alert(title, description, variant, type);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Destructive Alert", LabelVariant.Default);
        guiHelper.Alert("Error", "Your session has expired. Please log in again.", AlertVariant.Destructive, AlertType.Error);
        guiHelper.Label("Code: guiHelper.Alert(title, description, AlertVariant.Destructive, AlertType.Error);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Success Alert with Icon", LabelVariant.Default);
        guiHelper.Alert("Success", "Your profile has been updated successfully.", AlertVariant.Default, AlertType.Success, null);
        guiHelper.Label("Code: guiHelper.Alert(title, description, variant, type, iconTexture);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Custom Alert", LabelVariant.Default);
        guiHelper.CustomAlert("Custom Title", "This is a custom alert with custom colors.", Color.magenta, Color.white);
        guiHelper.Label("Code: guiHelper.CustomAlert(title, description, backgroundColor, textColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Alert with Progress", LabelVariant.Default);
        guiHelper.AlertWithProgress("Downloading", "Please wait while the file downloads.", 0.75f);
        guiHelper.Label("Code: guiHelper.AlertWithProgress(title, description, progress);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Animated Alert", LabelVariant.Default);
        guiHelper.AnimatedAlert("New Update Available", "A new version of the application is ready to be installed.");
        guiHelper.Label("Code: guiHelper.AnimatedAlert(title, description);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Alert with Countdown", LabelVariant.Default);
        guiHelper.AlertWithCountdown(
            "Session Timeout",
            "Your session will expire in 10 seconds.",
            10f,
            () =>
            {
                Debug.Log("Countdown finished!");
            }
        );
        guiHelper.Label("Code: guiHelper.AlertWithCountdown(title, description, countdownTime, onTimeout);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        /*
        guiHelper.Label("Expandable Alert", LabelVariant.Default);
        expandableAlertExpanded = guiHelper.ExpandableAlert("Details", "Click to see more information.", "This is the expanded content with more details about the alert.", ref expandableAlertExpanded);
        guiHelper.Label($"Expandable Alert Expanded: {expandableAlertExpanded}");
        guiHelper.Label("Code: guiHelper.ExpandableAlert(title, description, expandedContent, ref isExpanded);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        
        guiHelper.Label("Alert with Status", LabelVariant.Default);
        guiHelper.AlertWithStatus("Service Status", "All systems are operational.", true);
        guiHelper.Label("Code: guiHelper.AlertWithStatus(title, description, isActive);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        
        guiHelper.Label("Alert with Custom Icon", LabelVariant.Default);
        guiHelper.AlertWithCustomIcon("Custom Icon Alert", "This alert uses a custom icon.", null, Color.yellow);
        guiHelper.Label("Code: guiHelper.AlertWithCustomIcon(title, description, icon, iconColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        */
        GUILayout.EndVertical();
    }

    void DrawAvatarDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Avatar", LabelVariant.Default);
        guiHelper.MutedLabel("An image element with a fallback for representing a user.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Avatar", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Avatar(null, "AV");
        guiHelper.Avatar(null, "SM", AvatarSize.Small);
        guiHelper.Avatar(null, "LG", AvatarSize.Large);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Avatar(texture, fallbackText, size, shape);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar Shapes", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Avatar(null, "CR", AvatarSize.Default, AvatarShape.Circle);
        guiHelper.Avatar(null, "SQ", AvatarSize.Default, AvatarShape.Square);
        guiHelper.Avatar(null, "RD", AvatarSize.Default, AvatarShape.Rounded);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Avatar(texture, fallbackText, size, shape);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar with Status", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.AvatarWithStatus(null, "ON", true);
        guiHelper.AvatarWithStatus(null, "OFF", false);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.AvatarWithStatus(image, fallbackText, isOnline);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar with Name", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.AvatarWithName(null, "JD", "John Doe");
        guiHelper.AvatarWithName(null, "JS", "Jane Smith", showNameBelow: true);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.AvatarWithName(image, fallbackText, name, showNameBelow);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Custom Avatar", LabelVariant.Default);
        guiHelper.CustomAvatar(null, "CA", Color.blue, Color.white);
        guiHelper.Label("Code: guiHelper.CustomAvatar(image, fallbackText, backgroundColor, textColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar with Border", LabelVariant.Default);
        guiHelper.AvatarWithBorder(null, "BR", Color.red);
        guiHelper.Label("Code: guiHelper.AvatarWithBorder(image, fallbackText, borderColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar with Hover", LabelVariant.Default);
        guiHelper.AvatarWithHover(null, "HV", onClick: () => Debug.Log("Avatar Hover Clicked!"));
        guiHelper.Label("Code: guiHelper.AvatarWithHover(image, fallbackText, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar with Loading", LabelVariant.Default);
        guiHelper.AvatarWithLoading(null, "LD", true);
        guiHelper.Label("Code: guiHelper.AvatarWithLoading(image, fallbackText, isLoading);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Avatar with Tooltip", LabelVariant.Default);
        guiHelper.AvatarWithTooltip(null, "TP", "This is a user avatar.");
        guiHelper.Label("Code: guiHelper.AvatarWithTooltip(image, fallbackText, tooltip);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawBadgeDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Badge", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a badge or a tag.");
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
        guiHelper.Label("Code: guiHelper.Badge(text, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Badge with Icon", LabelVariant.Default);
        guiHelper.BadgeWithIcon("New", null);
        guiHelper.Label("Code: guiHelper.BadgeWithIcon(text, icon);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Custom Badge", LabelVariant.Default);
        guiHelper.CustomBadge("Custom", Color.yellow, Color.black);
        guiHelper.Label("Code: guiHelper.CustomBadge(text, backgroundColor, textColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Count Badge", LabelVariant.Default);
        guiHelper.CountBadge(5);
        guiHelper.CountBadge(105, maxCount: 99);
        guiHelper.Label("Code: guiHelper.CountBadge(count, maxCount);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Status Badge", LabelVariant.Default);
        guiHelper.StatusBadge("Active", true);
        guiHelper.StatusBadge("Inactive", false);
        guiHelper.Label("Code: guiHelper.StatusBadge(text, isActive);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Dismissible Badge", LabelVariant.Default);
        if (!dismissibleBadgeDismissed)
        {
            dismissibleBadgeDismissed = guiHelper.DismissibleBadge("Dismissible", onDismiss: () => dismissibleBadgeDismissed = true);
        }
        else
        {
            guiHelper.Button("Reset Dismissible Badge", onClick: () => dismissibleBadgeDismissed = false);
        }
        guiHelper.Label("Code: guiHelper.DismissibleBadge(text, onDismiss);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Progress Badge", LabelVariant.Default);
        guiHelper.ProgressBadge("Loading", 0.7f);
        guiHelper.Label("Code: guiHelper.ProgressBadge(text, progress);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Animated Badge", LabelVariant.Default);
        guiHelper.AnimatedBadge("Animating");
        guiHelper.Label("Code: guiHelper.AnimatedBadge(text);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Badge with Tooltip", LabelVariant.Default);
        guiHelper.BadgeWithTooltip("Info", "This is a helpful tooltip.");
        guiHelper.Label("Code: guiHelper.BadgeWithTooltip(text, tooltip);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Badge Group", LabelVariant.Default);
        guiHelper.BadgeGroup(new string[] { "Tag1", "Tag2", "Tag3" }, new BadgeVariant[] { BadgeVariant.Default, BadgeVariant.Secondary, BadgeVariant.Destructive });
        guiHelper.Label("Code: guiHelper.BadgeGroup(texts, variants);", LabelVariant.Muted);
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
        guiHelper.MutedLabel("A component for displaying a calendar and selecting dates.");
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

        guiHelper.Label($"Selected Date: {(calendarSelectedDate.HasValue ? calendarSelectedDate.Value.ToShortDateString() : "None")}");

        if (calendar.Ranges.Count > 0)
        {
            for (int i = 0; i < calendar.Ranges.Count; i++)
            {
                var r = calendar.Ranges[i];
                guiHelper.Label($"Range {i + 1}: {r.Start.ToShortDateString()} - {r.End.ToShortDateString()}");
            }
        }
        else
        {
            guiHelper.Label("No ranges selected");
        }

        if (guiHelper.Button("Disable Today"))
        {
            calendarDisabledDates.Add(DateTime.Today);
        }

        guiHelper.Label("Code: var calendar = guiHelper.GetCalendarComponents(); ... calendar.DrawCalendar();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawDropdownMenuDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Dropdown Menu", LabelVariant.Default);
        guiHelper.MutedLabel("A menu that appears when a trigger is clicked.");
        guiHelper.HorizontalSeparator();

        if (guiHelper.Button("Open Dropdown Menu"))
        {
            dropdownMenuItems = new System.Collections.Generic.List<DropdownMenuItem>()
            {
                new DropdownMenuItem(DropdownMenuItemType.Header, "My Account"),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Profile", () => Debug.Log("Profile selected")),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Billing", () => Debug.Log("Billing selected")),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Settings", () => Debug.Log("Settings selected")),
                new DropdownMenuItem(DropdownMenuItemType.Separator),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Team")
                {
                    SubItems = new System.Collections.Generic.List<DropdownMenuItem>() { new DropdownMenuItem(DropdownMenuItemType.Item, "Email", () => Debug.Log("Invite by Email")), new DropdownMenuItem(DropdownMenuItemType.Item, "Phone", () => Debug.Log("Invite by Phone")) },
                },
                new DropdownMenuItem(DropdownMenuItemType.Separator),
                new DropdownMenuItem(DropdownMenuItemType.Item, "Log out", () => Debug.Log("Log out selected")),
            };
            guiHelper.OpenDropdownMenu(dropdownMenuItems);
        }

        if (guiHelper.IsDropdownMenuOpen())
        {
            guiHelper.DropdownMenu();
        }

        guiHelper.Label("Code: guiHelper.DropdownMenu(items);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawPopoverDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Popover", LabelVariant.Default);
        guiHelper.MutedLabel("A pop-up that displays information related to an element.");
        guiHelper.HorizontalSeparator();

        if (guiHelper.Button("Open Popover"))
        {
            guiHelper.OpenPopover();
        }

        if (guiHelper.IsPopoverOpen())
        {
            guiHelper.Popover(() =>
            {
                guiHelper.Label("This is a popover.");
            });
        }

        guiHelper.Label("Code: guiHelper.Popover(rect, content);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawSelectDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Select", LabelVariant.Default);
        guiHelper.MutedLabel("A dropdown list for selecting a value.");
        guiHelper.HorizontalSeparator();

        string[] options = { "Option 1", "Option 2", "Option 3" };

        if (guiHelper.Button("Open Select"))
        {
            guiHelper.OpenSelect();
        }

        if (guiHelper.IsSelectOpen())
        {
            selectDemoCurrentSelection = guiHelper.Select(options, selectDemoCurrentSelection);
        }

        guiHelper.Label($"Selected: {options[selectDemoCurrentSelection]}");
        guiHelper.Label("Code: guiHelper.Select(rect, options, selectedIndex);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawButtonDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.Width(windowRect.width - 25), GUILayout.ExpandHeight(true));
        guiHelper.Label("Button", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a button or a clickable element that activates an event.");
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
        guiHelper.Label("Code: guiHelper.Button(label, variant, size, onClick, disabled);", LabelVariant.Muted);
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

        guiHelper.Label("Button Disabled State", LabelVariant.Default);
        guiHelper.Button("Disabled", disabled: true);
        guiHelper.Label("Code: guiHelper.Button(label, disabled: true);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Render Color Preset Button", LabelVariant.Default);
        guiHelper.RenderColorPresetButton("Red Button", Color.red);
        guiHelper.RenderColorPresetButton("Blue Button", Color.blue);
        guiHelper.Label("Code: guiHelper.RenderColorPresetButton(name, color);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Button (Simple)", LabelVariant.Default);
        guiHelper.DrawButton(200, "Simple Draw Button", () => Debug.Log("Simple Draw Button Clicked!"));
        guiHelper.Label("Code: guiHelper.DrawButton(width, text, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Colored Button", LabelVariant.Default);
        guiHelper.DrawColoredButton(200, "Colored Draw Button", Color.magenta, () => Debug.Log("Colored Draw Button Clicked!"));
        guiHelper.Label("Code: guiHelper.DrawColoredButton(width, text, color, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Fixed Button", LabelVariant.Default);
        guiHelper.DrawFixedButton("Fixed Button", 150, 40, () => Debug.Log("Fixed Button Clicked!"));
        guiHelper.Label("Code: guiHelper.DrawFixedButton(text, width, height, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Specific Variant Buttons (Direct Calls)", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.DestructiveButton("Destructive", () => Debug.Log("Destructive Button Clicked!"));
        guiHelper.OutlineButton("Outline", () => Debug.Log("Outline Button Clicked!"));
        guiHelper.SecondaryButton("Secondary", () => Debug.Log("Secondary Button Clicked!"));
        guiHelper.GhostButton("Ghost", () => Debug.Log("Ghost Button Clicked!"));
        guiHelper.LinkButton("Link", () => Debug.Log("Link Button Clicked!"));
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.DestructiveButton(...); etc.", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Sized Variant Buttons (Direct Calls)", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.SmallButton("Small", () => Debug.Log("Small Button Clicked!"));
        guiHelper.LargeButton("Large", () => Debug.Log("Large Button Clicked!"));
        guiHelper.IconButton("Icon", () => Debug.Log("Icon Button Clicked!"));
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.SmallButton(...); etc.", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Button Variant (Direct Call)", LabelVariant.Default);
        guiHelper.DrawButtonVariant("Direct Variant", ButtonVariant.Destructive, ButtonSize.Large);
        guiHelper.Label("Code: guiHelper.DrawButtonVariant(\"Direct Variant\", ButtonVariant.Destructive, ButtonSize.Large);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Button Group", LabelVariant.Default);
        guiHelper.ButtonGroup(() =>
        {
            guiHelper.Button("Btn 1");
            guiHelper.Button("Btn 2");
            guiHelper.Button("Btn 3");
        });
        guiHelper.Label("Code: guiHelper.ButtonGroup(() => { ... });", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawCardDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Card", LabelVariant.Default);
        guiHelper.MutedLabel("A flexible content container with header, content, and footer sections.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Project Card", LabelVariant.Default);
        guiHelper.DrawCard(
            "Create New Project",
            "Deploy your new project in one-click with our streamlined deployment process.",
            "Choose from our pre-configured templates or start from scratch. All projects include automatic CI/CD, monitoring, and scaling capabilities.",
            () =>
            {
                guiHelper.BeginHorizontalGroup();
                if (guiHelper.Button("Cancel", ButtonVariant.Outline))
                {
                    Debug.Log("Project creation cancelled");
                }
                GUILayout.Space(8);
                if (guiHelper.Button("Deploy Now", ButtonVariant.Default))
                {
                    Debug.Log("Project deployed!");
                }
                guiHelper.EndHorizontalGroup();
            },
            400,
            180
        );
        guiHelper.Label("Code: guiHelper.DrawCard(title, description, content, footerAction, width, height);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("User Profile Card", LabelVariant.Default);
        guiHelper.BeginCard(350, 220);
        guiHelper.CardHeader(() =>
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.Avatar(null, "JD", AvatarSize.Default);
            GUILayout.Space(12);
            guiHelper.BeginVerticalGroup();
            guiHelper.CardTitle("John Doe");
            guiHelper.CardDescription("Senior Developer");
            guiHelper.Badge("Pro", BadgeVariant.Default, BadgeSize.Small);
            guiHelper.EndVerticalGroup();
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.CardContent(() =>
        {
            guiHelper.Label("📧 john.doe@company.com");
            guiHelper.Label("📍 San Francisco, CA");
            guiHelper.Label("🏢 Tech Solutions Inc.");
            GUILayout.Space(8);
            guiHelper.Label("Experienced full-stack developer with expertise in Unity, C#, and web technologies.");
        });
        guiHelper.CardFooter(() =>
        {
            guiHelper.BeginHorizontalGroup();
            if (guiHelper.Button("Message", ButtonVariant.Outline, ButtonSize.Small))
            {
                Debug.Log("Send message to John");
            }
            GUILayout.Space(8);
            if (guiHelper.Button("View Profile", ButtonVariant.Default, ButtonSize.Small))
            {
                Debug.Log("View John's profile");
            }
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.EndCard();
        guiHelper.Label("Code: guiHelper.BeginCard(); guiHelper.CardHeader/Content/Footer(() => {}); guiHelper.EndCard();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Statistics Cards", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();

        guiHelper.BeginCard(180, 120);
        guiHelper.CardContent(() =>
        {
            guiHelper.Label("Total Revenue", LabelVariant.Muted);
            guiHelper.Label("$45,231.89", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("+20.1%", LabelVariant.Default);
            guiHelper.Label("from last month", LabelVariant.Muted);
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.EndCard();

        GUILayout.Space(12);

        guiHelper.BeginCard(180, 120);
        guiHelper.CardContent(() =>
        {
            guiHelper.Label("Active Users", LabelVariant.Muted);
            guiHelper.Label("2,350", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("+180.1%", LabelVariant.Default);
            guiHelper.Label("from last month", LabelVariant.Muted);
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.EndCard();

        GUILayout.Space(12);

        guiHelper.BeginCard(180, 120);
        guiHelper.CardContent(() =>
        {
            guiHelper.Label("Sales", LabelVariant.Muted);
            guiHelper.Label("+12,234", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("+19%", LabelVariant.Default);
            guiHelper.Label("from last month", LabelVariant.Muted);
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.EndCard();

        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: Multiple cards in horizontal layout for dashboard statistics", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Notification Card", LabelVariant.Default);
        guiHelper.BeginCard(400, 140);
        guiHelper.CardHeader(() =>
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.CardTitle("🔔 New Notification");
            GUILayout.FlexibleSpace();
            guiHelper.Badge("New", BadgeVariant.Destructive, BadgeSize.Small);
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.CardContent(() =>
        {
            guiHelper.Label("You have 3 new messages and 2 friend requests waiting for your attention.");
        });
        guiHelper.CardFooter(() =>
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("2 minutes ago", LabelVariant.Muted);
            GUILayout.FlexibleSpace();
            if (guiHelper.Button("Mark as Read", ButtonVariant.Ghost, ButtonSize.Small))
            {
                Debug.Log("Notification marked as read");
            }
            guiHelper.EndHorizontalGroup();
        });
        guiHelper.EndCard();
        guiHelper.Label("Code: Card with notification styling and timestamp", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Simple Content Card", LabelVariant.Default);
        guiHelper.DrawSimpleCard("This is a simple card with just content. Perfect for displaying basic information without headers or footers. You can use it for quick notes, simple displays, or as a container for other UI elements.", 400, 80);
        guiHelper.Label("Code: guiHelper.DrawSimpleCard(content, width, height);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawCheckboxDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Checkbox", LabelVariant.Default);
        guiHelper.MutedLabel("A control that allows the user to toggle between checked and unchecked states.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Checkbox");
        checkboxValue = guiHelper.Checkbox("Accept terms and conditions", checkboxValue);
        guiHelper.Label($"The checkbox is {(checkboxValue ? "checked" : "unchecked")}");
        guiHelper.Label("Code: checkboxValue = guiHelper.Checkbox(label, value);", LabelVariant.Muted);
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
        guiHelper.Label("Code: guiHelper.Checkbox(label, value, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Checkbox with Label");
        guiHelper.CheckboxWithLabel("Remember me", ref checkboxWithLabelValue);
        guiHelper.Label($"Checkbox with Label: {checkboxWithLabelValue}");
        guiHelper.Label("Code: guiHelper.CheckboxWithLabel(label, ref value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Checkbox Group");
        checkboxGroupValues = guiHelper.CheckboxGroup(new string[] { "Option 1", "Option 2", "Option 3" }, checkboxGroupValues);
        guiHelper.Label($"Checkbox Group Values: {string.Join(", ", checkboxGroupValues)}");
        guiHelper.Label("Code: guiHelper.CheckboxGroup(labels, values);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Checkbox with Icon");
        guiHelper.CheckboxWithIcon("Enable notifications", ref checkboxWithIconValue, null);
        guiHelper.Label($"Checkbox with Icon: {checkboxWithIconValue}");
        guiHelper.Label("Code: guiHelper.CheckboxWithIcon(label, ref value, icon);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Checkbox with Description");
        guiHelper.CheckboxWithDescription("Email marketing", "Send me marketing emails about new products and features.", ref checkboxWithDescriptionValue);
        guiHelper.Label($"Checkbox with Description: {checkboxWithDescriptionValue}");
        guiHelper.Label("Code: guiHelper.CheckboxWithDescription(label, description, ref value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Validated Checkbox");
        guiHelper.ValidatedCheckbox("I agree to the terms", ref validatedCheckboxValue, false, "You must agree to the terms.");
        guiHelper.Label($"Validated Checkbox: {validatedCheckboxValue}");
        guiHelper.Label("Code: guiHelper.ValidatedCheckbox(label, ref value, isValid, validationMessage);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Checkbox with Tooltip");
        guiHelper.CheckboxWithTooltip("Show advanced settings", ref checkboxWithTooltipValue, "This will reveal additional configuration options.");
        guiHelper.Label($"Checkbox with Tooltip: {checkboxWithTooltipValue}");
        guiHelper.Label("Code: guiHelper.CheckboxWithTooltip(label, ref value, tooltip);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Custom Checkbox");
        customCheckboxValue = guiHelper.CustomCheckbox("Custom Checkbox", customCheckboxValue, Color.cyan, Color.black);
        guiHelper.Label($"Custom Checkbox: {customCheckboxValue}");
        guiHelper.Label("Code: guiHelper.CustomCheckbox(label, value, checkColor, backgroundColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawLabelDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Label", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a non-interactive text element.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Label", LabelVariant.Default);
        guiHelper.Label("This is a default label.");
        guiHelper.Label("Code: guiHelper.Label(text);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Label Variants", LabelVariant.Default);
        guiHelper.SecondaryLabel("This is a secondary label.");
        guiHelper.MutedLabel("This is a muted label.");
        guiHelper.DestructiveLabel("This is a destructive label.");
        guiHelper.Label("Code: guiHelper.SecondaryLabel(text); etc.", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawLayoutDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Layout", LabelVariant.Default);
        guiHelper.MutedLabel("Components for organizing and spacing UI elements.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Horizontal Group", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Button("One");
        guiHelper.Button("Two");
        guiHelper.Button("Three");
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.BeginHorizontalGroup(); ... guiHelper.EndHorizontalGroup();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Vertical Group", LabelVariant.Default);
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        optionAValue = guiHelper.Checkbox("Option A", optionAValue);
        optionBValue = guiHelper.Checkbox("Option B", optionBValue);
        guiHelper.EndVerticalGroup();
        guiHelper.Label("Code: guiHelper.BeginVerticalGroup(); ... guiHelper.EndVerticalGroup();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Add Space", LabelVariant.Default);
        guiHelper.Label("Text above space.");
        guiHelper.AddSpace(20);
        guiHelper.Label("Text below space (20 pixels).");
        guiHelper.Label("Code: guiHelper.AddSpace(pixels);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawProgressDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Progress", LabelVariant.Default);
        guiHelper.MutedLabel("Displays an indicator showing the completion progress of a task.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Progress Bar", LabelVariant.Default);
        guiHelper.Progress(progressValue, 300);
        guiHelper.DrawSlider(300, "Progress", ref progressValue, 0, 1);
        guiHelper.Label("Code: guiHelper.Progress(value, width);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Labeled Progress Bar", LabelVariant.Default);
        guiHelper.LabeledProgress("Download", 0.75f, 300, showPercentage: true);
        guiHelper.Label("Code: guiHelper.LabeledProgress(label, value, width, showPercentage);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Circular Progress", LabelVariant.Default);
        guiHelper.CircularProgress(0.6f, 50);
        guiHelper.Label("Code: guiHelper.CircularProgress(value, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawSeparatorDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Separator", LabelVariant.Default);
        guiHelper.MutedLabel("A visual separator between sections.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Horizontal Separator", LabelVariant.Default);
        guiHelper.Label("Above");
        guiHelper.HorizontalSeparator();
        guiHelper.Label("Below");
        guiHelper.Label("Code: guiHelper.HorizontalSeparator();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Generic Separator (Horizontal, Decorative)", LabelVariant.Default);
        guiHelper.Separator(SeparatorOrientation.Horizontal, true);
        guiHelper.Label("Code: guiHelper.Separator(Horizontal, true);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Vertical Separator", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Label("Left");
        guiHelper.VerticalSeparator();
        guiHelper.Label("Right");
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.VerticalSeparator();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Separator with Spacing", LabelVariant.Default);
        guiHelper.Label("Text above spaced separator.");
        guiHelper.SeparatorWithSpacing(SeparatorOrientation.Horizontal, 10, 10);
        guiHelper.Label("Text below spaced separator.");
        guiHelper.Label("Code: guiHelper.SeparatorWithSpacing(Horizontal, 10, 10);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Labeled Separator", LabelVariant.Default);
        guiHelper.LabeledSeparator("OR");
        guiHelper.Label("Code: guiHelper.LabeledSeparator(\"OR\");");
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawSkeletonDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Skeleton", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a loading skeleton.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Skeleton", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Skeleton(50, 50, SkeletonVariant.Circular);
        guiHelper.BeginVerticalGroup();
        guiHelper.Skeleton(200, 20, SkeletonVariant.Default, SkeletonSize.Default);
        guiHelper.Skeleton(150, 20, SkeletonVariant.Default, SkeletonSize.Default);
        guiHelper.EndVerticalGroup();
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Skeleton(width, height, variant, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Skeleton Variants", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Skeleton(100, 30, SkeletonVariant.Default);
        guiHelper.Skeleton(100, 30, SkeletonVariant.Rounded);
        guiHelper.Skeleton(30, 30, SkeletonVariant.Circular);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Skeleton(width, height, variant);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Skeleton Sizes", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Skeleton(100, 20, SkeletonVariant.Default, SkeletonSize.Small);
        guiHelper.Skeleton(100, 30, SkeletonVariant.Default, SkeletonSize.Default);
        guiHelper.Skeleton(100, 40, SkeletonVariant.Default, SkeletonSize.Large);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Skeleton(width, height, variant, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawSliderDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Slider", LabelVariant.Default);
        guiHelper.MutedLabel("A control that allows the user to select a value from a range.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Slider", LabelVariant.Default);
        guiHelper.DrawSlider(300, "Volume", ref sliderValue, 0, 100);
        guiHelper.Label($"Current Value: {sliderValue:F2}");
        guiHelper.Label("Code: guiHelper.DrawSlider(width, label, ref value, min, max);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Integer Slider", LabelVariant.Default);
        guiHelper.DrawIntSlider(300, "Integer Value", ref intSliderValue, 0, 100);
        guiHelper.Label($"Current Integer Value: {intSliderValue}");
        guiHelper.Label("Code: guiHelper.DrawIntSlider(width, label, ref value, min, max);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawSwitchDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Switch", LabelVariant.Default);
        guiHelper.MutedLabel("A control that allows the user to toggle between on and off states.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Switch");
        switchValue = guiHelper.Switch("Dark Mode", switchValue);
        guiHelper.Label($"Dark mode is {(switchValue ? "on" : "off")}");
        guiHelper.Label("Code: switchValue = guiHelper.Switch(label, value);", LabelVariant.Muted);
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
        guiHelper.Label("Code: guiHelper.Switch(label, value, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Switch with Label");
        switchWithLabelValue = guiHelper.SwitchWithLabel("Enable notifications", switchWithLabelValue);
        guiHelper.Label($"Switch with Label: {switchWithLabelValue}");
        guiHelper.Label("Code: guiHelper.SwitchWithLabel(label, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Switch Group");
        switchGroupValues = guiHelper.SwitchGroup(new string[] { "Option A", "Option B", "Option C" }, switchGroupValues);
        guiHelper.Label($"Switch Group Values: {string.Join(", ", switchGroupValues)}");
        guiHelper.Label("Code: guiHelper.SwitchGroup(labels, values);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Switch with Description");
        switchWithDescriptionValue = guiHelper.SwitchWithDescription("Auto-update", "Automatically download and install updates.", switchWithDescriptionValue);
        guiHelper.Label($"Switch with Description: {switchWithDescriptionValue}");
        guiHelper.Label("Code: guiHelper.SwitchWithDescription(label, description, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Validated Switch");
        validatedSwitchValue = guiHelper.ValidatedSwitch("Accept privacy policy", validatedSwitchValue, false, "You must accept the privacy policy.");
        guiHelper.Label($"Validated Switch: {validatedSwitchValue}");
        guiHelper.Label("Code: guiHelper.ValidatedSwitch(label, value, isValid, validationMessage);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Switch with Tooltip");
        switchWithTooltipValue = guiHelper.SwitchWithTooltip("Show advanced options", switchWithTooltipValue, "Toggle to reveal more settings.");
        guiHelper.Label($"Switch with Tooltip: {switchWithTooltipValue}");
        guiHelper.Label("Code: guiHelper.SwitchWithTooltip(label, value, tooltip);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Switch with Icon");
        switchWithIconValue = guiHelper.SwitchWithIcon("Enable sound", switchWithIconValue, null, null);
        guiHelper.Label($"Switch with Icon: {switchWithIconValue}");
        guiHelper.Label("Code: guiHelper.SwitchWithIcon(label, value, onIcon, offIcon);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Switch with Loading");
        switchWithLoadingValue = guiHelper.SwitchWithLoading("Syncing data", switchWithLoadingValue, true);
        guiHelper.Label($"Switch with Loading: {switchWithLoadingValue}");
        guiHelper.Label("Code: guiHelper.SwitchWithLoading(label, value, isLoading);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Custom Switch");
        customSwitchValue = guiHelper.CustomSwitch("Custom Switch", customSwitchValue, Color.green, Color.red, Color.white);
        guiHelper.Label($"Custom Switch: {customSwitchValue}");
        guiHelper.Label("Code: guiHelper.CustomSwitch(label, value, onColor, offColor, thumbColor);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawTableDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Table", LabelVariant.Default);
        guiHelper.MutedLabel("A component for displaying tabular data.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Table", LabelVariant.Default);
        string[] headers = { "Invoice", "Status", "Method", "Amount" };
        string[,] data =
        {
            { "INV001", "Paid", "Credit Card", "$250.00" },
            { "INV002", "Pending", "PayPal", "$150.00" },
            { "INV003", "Unpaid", "Bank Transfer", "$350.00" },
        };
        guiHelper.Table(headers, data);
        guiHelper.Label("Code: guiHelper.Table(headers, data, variant, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Striped Table", LabelVariant.Default);
        guiHelper.Table(headers, data, TableVariant.Striped);
        guiHelper.Label("Code: guiHelper.Table(headers, data, TableVariant.Striped);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Bordered Table", LabelVariant.Default);
        guiHelper.Table(headers, data, TableVariant.Bordered);
        guiHelper.Label("Code: guiHelper.Table(headers, data, TableVariant.Bordered);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Hover Table", LabelVariant.Default);
        guiHelper.Table(headers, data, TableVariant.Hover);
        guiHelper.Label("Code: guiHelper.Table(headers, data, TableVariant.Hover);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Small Table", LabelVariant.Default);
        guiHelper.Table(headers, data, TableVariant.Default, TableSize.Small);
        guiHelper.Label("Code: guiHelper.Table(headers, data, variant, TableSize.Small);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Large Table", LabelVariant.Default);
        guiHelper.Table(headers, data, TableVariant.Default, TableSize.Large);
        guiHelper.Label("Code: guiHelper.Table(headers, data, variant, TableSize.Large);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawToggleDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Toggle", LabelVariant.Default);
        guiHelper.MutedLabel("A two-state button that can be either on or off.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Default Toggle", LabelVariant.Default);
        defaultToggleValue = guiHelper.Toggle("Enable Feature", defaultToggleValue);
        guiHelper.Label($"Default Toggle Value: {defaultToggleValue}");
        guiHelper.Label("Code: guiHelper.Toggle(text, value);", LabelVariant.Muted);
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
        guiHelper.Label("Code: guiHelper.Toggle(text, value, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Toggle Group", LabelVariant.Default);
        string[] toggleLabels = { "Left", "Center", "Right" };
        selectedToggle = guiHelper.ToggleGroup(toggleLabels, selectedToggle);
        guiHelper.Label($"Selected alignment: {toggleLabels[selectedToggle]}");
        guiHelper.Label("Code: selectedToggle = guiHelper.ToggleGroup(labels, selectedIndex);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Multi Toggle Group", LabelVariant.Default);
        multiToggleGroupValues = guiHelper.MultiToggleGroup(new string[] { "Red", "Green", "Blue" }, multiToggleGroupValues);
        guiHelper.Label($"Multi Toggle Group Values: {string.Join(", ", multiToggleGroupValues)}");
        guiHelper.Label("Code: guiHelper.MultiToggleGroup(labels, values);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawDialogDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Dialog", LabelVariant.Default);
        guiHelper.MutedLabel("A modal dialog component for displaying content over the main interface.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Basic Dialog", LabelVariant.Default);
        if (guiHelper.Button("Open Dialog"))
        {
            guiHelper.OpenDialog("basic-dialog");
        }

        guiHelper.DrawDialog(
            "basic-dialog",
            "Dialog Title",
            "This is a basic dialog with a title and description.",
            () =>
            {
                guiHelper.Label("This is the dialog content area.");
                guiHelper.Label("You can put any content here.");
            },
            () =>
            {
                if (guiHelper.Button("Cancel", ButtonVariant.Outline))
                {
                    guiHelper.CloseDialog();
                }
                if (guiHelper.Button("Confirm"))
                {
                    guiHelper.CloseDialog();
                    Debug.Log("Dialog confirmed!");
                }
            }
        );

        guiHelper.Label("Code: guiHelper.OpenDialog(id); guiHelper.DrawDialog(id, title, description, content, footer);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Simple Dialog", LabelVariant.Default);
        if (guiHelper.Button("Open Simple Dialog"))
        {
            guiHelper.OpenDialog("simple-dialog");
        }

        guiHelper.DrawDialog(
            "simple-dialog",
            () =>
            {
                guiHelper.Label("Simple Dialog Content");
                guiHelper.Label("This dialog has no title or footer.");
                guiHelper.HorizontalSeparator();
                if (guiHelper.Button("Close"))
                {
                    guiHelper.CloseDialog();
                }
            }
        );

        guiHelper.Label("Code: guiHelper.DrawDialog(id, content);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Dialog Status", LabelVariant.Default);
        guiHelper.Label($"Dialog Open: {guiHelper.IsDialogOpen()}");
        if (guiHelper.IsDialogOpen())
        {
            if (guiHelper.Button("Force Close Dialog"))
            {
                guiHelper.CloseDialog();
            }
        }
        guiHelper.Label("Code: guiHelper.IsDialogOpen(); guiHelper.CloseDialog();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawDatePickerDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Date Picker", LabelVariant.Default);
        guiHelper.MutedLabel("A date picker component for selecting dates with a calendar interface.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Basic Date Picker", LabelVariant.Default);
        selectedDate = guiHelper.DatePicker("Select a date", selectedDate, "basic-datepicker", GUILayout.Width(200));
        guiHelper.Label($"Selected Date: {(selectedDate.HasValue ? selectedDate.Value.ToString("MMM dd, yyyy") : "None")}");
        guiHelper.Label("Code: selectedDate = guiHelper.DatePicker(placeholder, selectedDate, id);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Date Picker with Label", LabelVariant.Default);
        selectedDateWithLabel = guiHelper.DatePickerWithLabel("Birth Date", "Pick your birth date", selectedDateWithLabel, "labeled-datepicker", GUILayout.Width(200));
        guiHelper.Label($"Birth Date: {(selectedDateWithLabel.HasValue ? selectedDateWithLabel.Value.ToString("MMM dd, yyyy") : "None")}");
        guiHelper.Label("Code: selectedDate = guiHelper.DatePickerWithLabel(label, placeholder, selectedDate, id);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Date Picker Controls", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        if (guiHelper.Button("Set Today"))
        {
            selectedDate = DateTime.Today;
        }
        if (guiHelper.Button("Clear Date"))
        {
            selectedDate = null;
        }
        if (guiHelper.Button("Close Picker"))
        {
            guiHelper.CloseDatePicker("basic-datepicker");
        }
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.CloseDatePicker(id);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Date Picker Status", LabelVariant.Default);
        guiHelper.Label($"Basic Picker Open: {guiHelper.IsDatePickerOpen("basic-datepicker")}");
        guiHelper.Label($"Labeled Picker Open: {guiHelper.IsDatePickerOpen("labeled-datepicker")}");
        guiHelper.Label("Code: guiHelper.IsDatePickerOpen(id);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }

    void DrawDataTableDemos()
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Data Table", LabelVariant.Default);
        guiHelper.MutedLabel("A powerful data table component with sorting, filtering, and pagination.");
        guiHelper.HorizontalSeparator();

        if (dataTableColumns == null)
        {
            dataTableColumns = new System.Collections.Generic.List<DataTableColumn>();

            var idColumn = guiHelper.CreateDataTableColumn("id", "ID");
            idColumn.Width = 60;
            dataTableColumns.Add(idColumn);

            var nameColumn = guiHelper.CreateDataTableColumn("name", "Name");
            nameColumn.Width = 150;
            dataTableColumns.Add(nameColumn);

            var emailColumn = guiHelper.CreateDataTableColumn("email", "Email");
            emailColumn.Width = 200;
            dataTableColumns.Add(emailColumn);

            var statusColumn = guiHelper.CreateDataTableColumn("status", "Status");
            statusColumn.Width = 100;
            dataTableColumns.Add(statusColumn);

            dataTableData = new System.Collections.Generic.List<DataTableRow>();

            var row1 = guiHelper.CreateDataTableRow("1");
            row1.SetData("id", "1");
            row1.SetData("name", "John Doe");
            row1.SetData("email", "john@example.com");
            row1.SetData("status", "Active");
            dataTableData.Add(row1);

            var row2 = guiHelper.CreateDataTableRow("2");
            row2.SetData("id", "2");
            row2.SetData("name", "Jane Smith");
            row2.SetData("email", "jane@example.com");
            row2.SetData("status", "Active");
            dataTableData.Add(row2);

            var row3 = guiHelper.CreateDataTableRow("3");
            row3.SetData("id", "3");
            row3.SetData("name", "Bob Johnson");
            row3.SetData("email", "bob@example.com");
            row3.SetData("status", "Inactive");
            dataTableData.Add(row3);

            var row4 = guiHelper.CreateDataTableRow("4");
            row4.SetData("id", "4");
            row4.SetData("name", "Alice Brown");
            row4.SetData("email", "alice@example.com");
            row4.SetData("status", "Active");
            dataTableData.Add(row4);

            var row5 = guiHelper.CreateDataTableRow("5");
            row5.SetData("id", "5");
            row5.SetData("name", "Charlie Wilson");
            row5.SetData("email", "charlie@example.com");
            row5.SetData("status", "Active");
            dataTableData.Add(row5);
        }

        guiHelper.Label("Basic Data Table", LabelVariant.Default);

        guiHelper.DrawDataTable("demo-table", dataTableColumns, dataTableData, showPagination: true, showSearch: true, showSelection: true, showColumnToggle: false, GUILayout.Height(200));

        var selectedRows = guiHelper.GetSelectedRows("demo-table");
        if (selectedRows.Count > 0)
        {
            guiHelper.Label($"Selected: {string.Join(", ", selectedRows)}", LabelVariant.Muted);
            if (guiHelper.Button("Clear Selection"))
            {
                guiHelper.ClearSelection("demo-table");
            }
        }

        guiHelper.Label("Code: guiHelper.DrawDataTable(id, columns, data, showPagination, showSearch, showSelection, showColumnToggle);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Simple Table", LabelVariant.Default);
        var simpleColumns = new System.Collections.Generic.List<DataTableColumn>();

        var productColumn = guiHelper.CreateDataTableColumn("product", "Product");
        productColumn.Width = 150;
        simpleColumns.Add(productColumn);

        var priceColumn = guiHelper.CreateDataTableColumn("price", "Price");
        priceColumn.Width = 100;
        simpleColumns.Add(priceColumn);

        var simpleData = new System.Collections.Generic.List<DataTableRow>
        {
            guiHelper.CreateDataTableRow("p1").SetData("product", "Laptop").SetData("price", "$999"),
            guiHelper.CreateDataTableRow("p2").SetData("product", "Mouse").SetData("price", "$25"),
            guiHelper.CreateDataTableRow("p3").SetData("product", "Keyboard").SetData("price", "$75"),
        };

        guiHelper.DrawDataTable("simple-table", simpleColumns, simpleData, showPagination: false, showSearch: false, showSelection: false, showColumnToggle: false, GUILayout.Height(120));

        guiHelper.Label("Code: guiHelper.DrawDataTable(id, columns, data, false, false, false, false);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Table Actions", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();

        if (guiHelper.Button("Add Row"))
        {
            int newId = dataTableData.Count + 1;
            var newRow = guiHelper.CreateDataTableRow(newId.ToString()).SetData("id", newId.ToString()).SetData("name", $"User {newId}").SetData("email", $"user{newId}@example.com").SetData("status", "Active");
            dataTableData.Add(newRow);
        }

        if (guiHelper.Button("Remove Last"))
        {
            if (dataTableData.Count > 0)
            {
                dataTableData.RemoveAt(dataTableData.Count - 1);
            }
        }

        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.CreateDataTableRow(id).SetData(key, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        GUILayout.EndVertical();
    }
}
