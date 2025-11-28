using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui_Demo.Menu
{
    public class DocsDemo : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new Rect(40, 40, 1200, 800);
        private bool showDocsWindow = false;
        private Vector2 scrollPosition;
        private int currentTab = 0;
        private Tabs.TabConfig[] docTabs;

        // Interactive states
        private bool toggleState = false;
        private bool checkboxState = false;
        private bool switchState = false;
        private string inputPassword = "";

        // Code snippet states
        private string setupCode =
            @"void Start() {
    guiHelper = new GUIHelper();
}

void OnGUI() {
    guiHelper.UpdateAnimations(true);
    if (guiHelper.BeginAnimatedGUI()) {
        // Your UI code here
        guiHelper.Button(""Click Me"");
        
        guiHelper.EndAnimatedGUI();
    }
}";

        void Start()
        {
            guiHelper = new GUIHelper();
            docTabs = new Tabs.TabConfig[]
            {
                new Tabs.TabConfig("Overview", DrawOverview),
                new Tabs.TabConfig("Getting Started", DrawGettingStarted),
                new Tabs.TabConfig("Theming", DrawTheming),
                new Tabs.TabConfig("Basic Inputs", DrawBasicInputs),
                new Tabs.TabConfig("Data Display", DrawDataDisplay),
                new Tabs.TabConfig("Overlays", DrawOverlays),
                new Tabs.TabConfig("Navigation", DrawNavigation),
                new Tabs.TabConfig("Pickers", DrawPickers),
                new Tabs.TabConfig("Layouts", DrawLayouts),
                new Tabs.TabConfig("Utilities", DrawUtilities),
            };
        }

        void OnGUI()
        {
            if (GUILayout.Button("Open Documentation", GUILayout.Height(30)))
            {
                showDocsWindow = !showDocsWindow;
            }

            if (showDocsWindow)
            {
                windowRect = GUI.Window(102, windowRect, (GUI.WindowFunction)DrawDocsWindow, "shadcn/ui Documentation");
            }
        }

        void DrawDocsWindow(int windowID)
        {
            guiHelper.UpdateAnimations(showDocsWindow);
            if (guiHelper.BeginAnimatedGUI())
            {
                guiHelper.BeginHorizontalGroup();

                GUILayout.BeginVertical(GUILayout.Width(200));
                guiHelper.Label("Documentation", ControlVariant.Default);
                guiHelper.HorizontalSeparator();
                currentTab = guiHelper.VerticalTabs(docTabs.Select(t => t.Name).ToArray(), currentTab, null, tabWidth: 190, side: Tabs.TabSide.Left);
                GUILayout.EndVertical();

                guiHelper.VerticalSeparator();

                scrollPosition = guiHelper.DrawScrollView(
                    scrollPosition,
                    () =>
                    {
                        GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                        if (currentTab >= 0 && currentTab < docTabs.Length)
                        {
                            guiHelper.Label(docTabs[currentTab].Name, ControlVariant.Default);
                            guiHelper.HorizontalSeparator();
                            GUILayout.Space(10);
                            docTabs[currentTab].Content?.Invoke();
                        }
                        GUILayout.EndVertical();
                    }
                );

                guiHelper.EndHorizontalGroup();
                guiHelper.EndAnimatedGUI();
            }
            GUI.DragWindow();
        }

        #region Documentation Sections

        void DrawOverview()
        {
            DrawText("Welcome to the shadcn/ui library for Unity IMGUI.");
            DrawText("This library provides a set of modern, customizable, and accessible UI components designed to look like the popular shadcn/ui web library.");

            GUILayout.Space(20);

            guiHelper.DrawCard(
                "Key Features",
                "Why use this?",
                "- Modern Aesthetic: Clean, professional look.\n" + "- Theming: Built-in support for variants (Secondary, Destructive, etc.) and sizes.\n" + "- Animations: Smooth transitions for hover states and interactions.\n" + "- Easy API: Simple helper methods to draw complex components.",
                null,
                600
            );
        }

        void DrawGettingStarted()
        {
            DrawSectionHeader("1. Initialization");
            DrawText("First, create an instance of GUIHelper in your MonoBehaviour's Start method.");

            DrawSectionHeader("2. The Loop");
            DrawText("In OnGUI, wrap your UI calls with BeginAnimatedGUI and EndAnimatedGUI to enable animations and styling.");

            DrawCodeBlock(setupCode);

            DrawSectionHeader("3. Using Components");
            DrawText("Call methods on the guiHelper instance to draw components.");
            DrawCodeBlock("guiHelper.Button(\"Submit\", ControlVariant.Default, ControlSize.Default, () => Debug.Log(\"Clicked!\"));");
        }

        void DrawTheming()
        {
            DrawText("The library uses two main enums to control the look and feel of components: ControlVariant and ControlSize.");

            DrawSectionHeader("ControlVariant");
            DrawText("Variants define the visual style (color, border, background).");

            guiHelper.BeginHorizontalGroup();
            foreach (ControlVariant variant in Enum.GetValues(typeof(ControlVariant)))
            {
                GUILayout.BeginVertical(GUILayout.Width(100));
                guiHelper.Label(variant.ToString(), ControlVariant.Muted);
                guiHelper.Button("Sample", variant);
                GUILayout.EndVertical();
            }
            guiHelper.EndHorizontalGroup();

            DrawSectionHeader("ControlSize");
            DrawText("Sizes define the dimensions and padding.");

            guiHelper.BeginHorizontalGroup();
            foreach (ControlSize size in Enum.GetValues(typeof(ControlSize)))
            {
                GUILayout.BeginVertical(GUILayout.Width(100));
                guiHelper.Label(size.ToString(), ControlVariant.Muted);
                guiHelper.Button("Sample", ControlVariant.Default, size);
                GUILayout.EndVertical();
            }
            guiHelper.EndHorizontalGroup();
        }

        void DrawBasicInputs()
        {
            DrawText("Essential input components for user interaction.");

            // Button
            DrawComponentDoc("Button", "guiHelper.Button(\"Click Me\", ControlVariant.Default, ControlSize.Default, () => Debug.Log(\"Clicked\"));", () => guiHelper.Button("Click Me"));

            // Input
            DrawComponentDoc("Input (Password)", "string val = \"\";\nguiHelper.DrawPasswordField(200, \"Placeholder\", ref val);", () => guiHelper.DrawPasswordField(200, "Placeholder", ref inputPassword));

            // TextArea
            DrawComponentDoc("TextArea", "string text = \"Multi-line\";\ntext = guiHelper.TextArea(text, ControlVariant.Default, \"Type here...\");", () => guiHelper.TextArea("Multi-line content", ControlVariant.Default));

            // Toggle
            DrawComponentDoc("Toggle", "bool isOn = false;\nisOn = guiHelper.Toggle(\"Enable Feature\", isOn);", () => toggleState = guiHelper.Toggle("Enable Feature", toggleState));

            // Checkbox
            DrawComponentDoc("Checkbox", "bool isChecked = false;\nisChecked = guiHelper.Checkbox(\"Accept Terms\", isChecked);", () => checkboxState = guiHelper.Checkbox("Accept Terms", checkboxState));

            // Switch
            DrawComponentDoc("Switch", "bool isActive = false;\nisActive = guiHelper.Switch(\"Airplane Mode\", isActive);", () => switchState = guiHelper.Switch("Airplane Mode", switchState));

            // Slider/Progress
            DrawComponentDoc("Slider / Progress", "guiHelper.Progress(0.7f, 200);", () => guiHelper.Progress(0.7f, 200));
        }

        void DrawDataDisplay()
        {
            DrawText("Components for displaying data and feedback.");

            // Badge
            DrawComponentDoc("Badge", "guiHelper.Badge(\"New\", ControlVariant.Destructive);", () => guiHelper.Badge("New", ControlVariant.Destructive));

            // Avatar
            DrawComponentDoc("Avatar", "guiHelper.Avatar(texture, \"JD\", ControlSize.Default, AvatarShape.Circle);", () => guiHelper.Avatar(null, "JD", ControlSize.Default, AvatarShape.Circle));

            // Card
            DrawComponentDoc("Card", "guiHelper.DrawCard(\"Title\", \"Subtitle\", \"Content...\", null, 300);", () => guiHelper.DrawCard("Title", "Subtitle", "Content...", null, 300));

            // Separator
            DrawComponentDoc("Separator", "guiHelper.HorizontalSeparator();", () => guiHelper.HorizontalSeparator());

            // Label
            DrawComponentDoc("Label", "guiHelper.Label(\"Important Text\", ControlVariant.Destructive);", () => guiHelper.Label("Important Text", ControlVariant.Destructive));

            // Table
            DrawComponentDoc(
                "Table (Simple)",
                "string[] headers = { \"ID\", \"Name\" };\nstring[,] data = { { \"1\", \"Alice\" }, { \"2\", \"Bob\" } };\nguiHelper.Table(headers, data);",
                () =>
                    guiHelper.Table(
                        new[] { "ID", "Name" },
                        new[,]
                        {
                            { "1", "Alice" },
                            { "2", "Bob" },
                        }
                    )
            );
        }

        void DrawOverlays()
        {
            DrawText("Components that float above other content.");

            // Dialog
            DrawComponentDoc(
                "Dialog",
                "if (guiHelper.Button(\"Open\")) guiHelper.OpenDialog(\"my_dlg\");\n\nguiHelper.DrawDialog(\"my_dlg\", \"Title\", \"Message\", \n    () => guiHelper.Label(\"Body\"), \n    () => guiHelper.Button(\"Close\", () => guiHelper.CloseDialog()));",
                () =>
                {
                    if (guiHelper.Button("Open Dialog"))
                        guiHelper.OpenDialog("doc_dlg");
                    guiHelper.DrawDialog(
                        "doc_dlg",
                        "Documentation Dialog",
                        "This is a modal dialog.",
                        () => guiHelper.Label("You can put any content here."),
                        () =>
                        {
                            if (guiHelper.Button("Close"))
                                guiHelper.CloseDialog();
                        }
                    );
                }
            );

            // Popover
            DrawComponentDoc(
                "Popover",
                "if (guiHelper.Button(\"Popover\")) guiHelper.OpenPopover();\nif (guiHelper.IsPopoverOpen()) \n    guiHelper.Popover(() => guiHelper.Label(\"Content\"));",
                () =>
                {
                    if (guiHelper.Button("Open Popover"))
                        guiHelper.OpenPopover();
                    if (guiHelper.IsPopoverOpen())
                        guiHelper.Popover(() => guiHelper.Label("This is a popover!"));
                }
            );
        }

        void DrawNavigation()
        {
            DrawText("Components for moving between views.");

            // Tabs
            DrawComponentDoc("Tabs", "int current = 0;\ncurrent = guiHelper.DrawTabs(new[]{\"A\", \"B\"}, current, () => DrawContent(current));", () => guiHelper.DrawTabs(new[] { "Tab A", "Tab B" }, 0, (i) => guiHelper.Label($"Content {i}"), maxLines: 1));

            // MenuBar
            DrawComponentDoc(
                "MenuBar",
                "var items = new List<MenuBar.MenuItem> { new MenuBar.MenuItem(\"File\") };\nguiHelper.MenuBar(new MenuBar.MenuBarConfig(items));",
                () => guiHelper.MenuBar(new MenuBar.MenuBarConfig(new List<MenuBar.MenuItem> { new MenuBar.MenuItem("File", null, false, new List<MenuBar.MenuItem> { new MenuBar.MenuItem("New"), new MenuBar.MenuItem("Exit") }), new MenuBar.MenuItem("Edit") }))
            );
        }

        void DrawPickers()
        {
            DrawText("Components for selecting values.");

            // DatePicker
            DrawComponentDoc(
                "DatePicker",
                "DateTime? date = null;\ndate = guiHelper.DatePicker(\"Select Date\", date, \"dp_id\");",
                () =>
                {
                    DateTime? d = null;
                    guiHelper.DatePicker("Select Date", d, "doc_dp");
                }
            );

            // Select
            DrawComponentDoc(
                "Select",
                "if (guiHelper.Button(\"Select\")) guiHelper.OpenSelect();\nif (guiHelper.IsSelectOpen())\n    idx = guiHelper.Select(options, idx);",
                () =>
                {
                    if (guiHelper.Button("Open Select"))
                        guiHelper.OpenSelect();
                    if (guiHelper.IsSelectOpen())
                        guiHelper.Select(new[] { "Option A", "Option B" }, 0);
                }
            );
        }

        void DrawUtilities()
        {
            DrawText("Helper methods for custom drawing.");

            DrawSectionHeader("TryExecute");
            DrawText("Wraps code in a try-catch block to prevent GUI errors from stopping execution.");
            DrawCodeBlock("guiHelper.TryExecute(() => { /* risky code */ }, \"ContextName\");");

            DrawSectionHeader("GetRect");
            DrawText("Reserves space in the layout system and returns the Rect.");
            DrawCodeBlock("Rect r = guiHelper.GetRect(100, 30);");
        }

        void DrawLayouts()
        {
            DrawText("Use layout groups to organize your UI.");

            DrawSectionHeader("Horizontal Group");
            DrawSectionHeader("Horizontal Group");
            DrawCodeBlock("guiHelper.BeginHorizontalGroup();\nguiHelper.EndHorizontalGroup();");

            guiHelper.BeginCard(300);
            guiHelper.CardHeader(() => guiHelper.CardTitle("Example"));
            guiHelper.CardContent(() =>
            {
                guiHelper.BeginHorizontalGroup();
                guiHelper.Button("Left");
                guiHelper.Button("Right");
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();

            GUILayout.Space(20);

            DrawSectionHeader("Vertical Group");
            DrawSectionHeader("Vertical Group");
            DrawCodeBlock("guiHelper.BeginVerticalGroup();\nguiHelper.EndVerticalGroup();");
        }

        #endregion

        #region Helpers
        private void DrawText(string text)
        {
            guiHelper.Label(text);
            GUILayout.Space(10);
        }

        private void DrawSectionHeader(string title)
        {
            GUILayout.Space(10);
            guiHelper.Label(title, ControlVariant.Default);
            guiHelper.HorizontalSeparator();
            GUILayout.Space(5);
        }

        private void DrawCodeBlock(string code)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            guiHelper.TextArea(code, ControlVariant.Secondary, "", true);
            GUILayout.EndVertical();
            GUILayout.Space(10);
        }

        private void DrawComponentDoc(string name, string code, Action demo)
        {
            DrawSectionHeader(name);
            guiHelper.BeginHorizontalGroup();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            DrawCodeBlock(code);
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical(GUILayout.Width(200));
            guiHelper.Label("Preview:", ControlVariant.Muted);
            demo?.Invoke();
            GUILayout.EndVertical();

            guiHelper.EndHorizontalGroup();
        }
        #endregion
    }
}
