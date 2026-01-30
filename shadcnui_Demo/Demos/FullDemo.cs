using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
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
        private TabConfig[] demoTabs;
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

        // Slider States
        private float sliderValue = 0.5f;
        private float sliderWithStepValue = 50f;
        private float sliderLabeledValue = 0.75f;
        private float sliderDisabledValue = 0.3f;
        private float sliderDestructiveValue = 0.6f;
        private float sliderSmallValue = 0.4f;
        private float sliderLargeValue = 0.8f;

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

            demoTabs = new TabConfig[]
            {
                new TabConfig("Button", DrawButtonDemos),
                new TabConfig("Badge", DrawBadgeDemos),
                new TabConfig("Input", DrawInputDemos),
                new TabConfig("Toggle", DrawToggleDemos),
                new TabConfig("Checkbox", DrawCheckboxDemos),
                new TabConfig("Switch", DrawSwitchDemos),
                new TabConfig("TextArea", DrawTextAreaDemos),
                new TabConfig("Avatar", DrawAvatarDemos),
                new TabConfig("Card", DrawCardDemos),
                new TabConfig("Progress", DrawProgressDemos),
                new TabConfig("Separator", DrawSeparatorDemos),
                new TabConfig("Label", DrawLabelDemos),
                new TabConfig("Dialog", DrawDialogDemos),
                new TabConfig("Select", DrawSelectDemos),
                new TabConfig("DropdownMenu", DrawDropdownMenuDemos),
                new TabConfig("Popover", DrawPopoverDemos),
                new TabConfig("Calendar", DrawCalendarDemos),
                new TabConfig("DatePicker", DrawDatePickerDemos),
                new TabConfig("Tabs", DrawTabsDemos),
                new TabConfig("MenuBar", DrawMenuBar),
                new TabConfig("Chart", DrawChartDemos),
                new TabConfig("Table", DrawTableDemos),
                new TabConfig("DataTable", DataTableDemos),
                new TabConfig("Toast", DrawToastDemos),
                new TabConfig("Slider", DrawSliderDemos),
                new TabConfig("Layout", DrawLayoutDemos),
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

            // Draw toasts for Toast
            guiHelper.DrawOverlay();
        }

        void DrawDemoWindow(int windowID)
        {
            guiHelper.UpdateGUI(showDemoWindow);
            if (guiHelper.BeginGUI())
            {
                DrawHeader();

                if (useVerticalTabs)
                {
                    var side = verticalTabsOnRight ? TabSide.Right : TabSide.Left;
                    currentDemoTab = guiHelper.VerticalTabs(demoTabs.Select(tab => tab.Name).ToArray(), currentDemoTab, DrawScrollableContent, tabWidth: 160, maxLines: 2, side: side);
                }
                else
                {
                    var position = tabsOnBottom ? TabPosition.Bottom : TabPosition.Top;
                    currentDemoTab = guiHelper.Tabs(demoTabs.Select(tab => tab.Name).ToArray(), currentDemoTab, DrawScrollableContent, maxLines: 2, position: position);
                }
                guiHelper.EndGUI();
            }
            GUI.DragWindow();
        }

        void DrawHeader()
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("shadcn/ui Library", ControlVariant.Default);
            GUILayout.FlexibleSpace();
            guiHelper.ThemeChangerWithPreview("header_theme_changer", 180f);
            GUILayout.Space(10);
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
            scrollPosition = guiHelper.ScrollView(
                scrollPosition,
                () =>
                {
                    guiHelper.BeginVerticalGroup();
                    if (currentDemoTab >= 0 && currentDemoTab < demoTabs.Length)
                    {
                        demoTabs[currentDemoTab].Content?.Invoke();
                    }
                    guiHelper.EndVerticalGroup();
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

            DrawSection(
                "Buttons with Icons",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Button with a icon", img);
                    guiHelper.Button("Left Icon", img, ControlVariant.Outline);
                    guiHelper.Button("Secondary", img, ControlVariant.Secondary);
                    guiHelper.EndHorizontalGroup();

                    guiHelper.Label("Icon Positioning");
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button(
                        "Left",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Left,
                            Size = 14f,
                            Spacing = 4f,
                        }
                    );
                    guiHelper.Button(
                        "Right",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Right,
                            Size = 14f,
                            Spacing = 4f,
                        }
                    );
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
                    guiHelper.RoundedBadge("Rounded", cornerRadius: 8f);
                    guiHelper.BadgeWithIcon("Icon", img);
                    guiHelper.EndHorizontalGroup();

                    guiHelper.Label("Badge with Icon");
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Badge("Icon", new IconConfig(img) { Size = 12f, Spacing = 3f });
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Animated Badges",
                () =>
                {
                    guiHelper.Label("Badges with smooth pulse animation using AnimationManager:");
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.AnimatedBadge("Pulse 1", "badge_anim_1");
                    guiHelper.AnimatedBadge("Pulse 2", "badge_anim_2", ControlVariant.Destructive);
                    guiHelper.AnimatedBadge("Pulse 3", "badge_anim_3", ControlVariant.Secondary);
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
                    guiHelper.PasswordField(200, "Default", ref passwordValue);
                }
            );

            DrawSection(
                "Input Types",
                () =>
                {
                    guiHelper.Label("Password");
                    guiHelper.PasswordField(300, "Password", ref passwordValue);

                    guiHelper.Label("Section Header");
                    guiHelper.SectionHeader("Header Example");

                    guiHelper.Label("Render Label");
                    guiHelper.InputLabel("Rendered Label Text", 200);
                }
            );

            DrawSection(
                "Inputs with Icons",
                () =>
                {
                    guiHelper.Input("Input with Icon", img, passwordValue);

                    guiHelper.Label("Icon Config");
                    guiHelper.Input(
                        "Left Icon",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Left,
                            Size = 14f,
                            Spacing = 4f,
                        }
                    );
                    guiHelper.Input(
                        "Right Icon",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Right,
                            Size = 14f,
                            Spacing = 4f,
                        }
                    );
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

            DrawSection(
                "Toggles with Icons",
                () =>
                {
                    string key = "icon_toggle";
                    if (!toggleStates.ContainsKey(key))
                        toggleStates[key] = false;
                    toggleStates[key] = guiHelper.Toggle("Toggle with Icon", new IconConfig(img, IconPosition.Left), toggleStates[key]);

                    guiHelper.Label("Icon Config");
                    string leftKey = "toggle_left";
                    if (!toggleStates.ContainsKey(leftKey))
                        toggleStates[leftKey] = false;
                    toggleStates[leftKey] = guiHelper.Toggle(
                        "Left",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Left,
                            Size = 14f,
                            Spacing = 4f,
                        },
                        toggleStates[leftKey]
                    );

                    string rightKey = "toggle_right";
                    if (!toggleStates.ContainsKey(rightKey))
                        toggleStates[rightKey] = false;
                    toggleStates[rightKey] = guiHelper.Toggle(
                        "Right",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Right,
                            Size = 14f,
                            Spacing = 4f,
                        },
                        toggleStates[rightKey]
                    );
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
                "Checkboxes with Icons",
                () =>
                {
                    string leftKey = "check_left";
                    if (!toggleStates.ContainsKey(leftKey))
                        toggleStates[leftKey] = false;
                    toggleStates[leftKey] = guiHelper.Checkbox(
                        "Left",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Left,
                            Size = 14f,
                            Spacing = 4f,
                        },
                        toggleStates[leftKey]
                    );

                    string rightKey = "check_right";
                    if (!toggleStates.ContainsKey(rightKey))
                        toggleStates[rightKey] = false;
                    toggleStates[rightKey] = guiHelper.Checkbox(
                        "Right",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Right,
                            Size = 14f,
                            Spacing = 4f,
                        },
                        toggleStates[rightKey]
                    );
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

                    guiHelper.Card("Standard", "Subtitle", "Content goes here.", () => guiHelper.Button("Action"), 200, 150);

                    GUILayout.Space(10);

                    guiHelper.SimpleCard("Simple card content.", 200, 100);

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
                    guiHelper.CardWithImage(img, "Image Card", "Subtitle", "Content with image.", null, 200, 250);
                    GUILayout.Space(10);
                    guiHelper.CardWithAvatar(img, "Avatar Card", "User Name", "Content with avatar.", null, 200, 150);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private float animatedProgressTarget = 0.7f;

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

            DrawSection(
                "Animated Progress",
                () =>
                {
                    guiHelper.Label("Click buttons to animate progress smoothly:");
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.Button("0%"))
                        animatedProgressTarget = 0f;
                    if (guiHelper.Button("25%"))
                        animatedProgressTarget = 0.25f;
                    if (guiHelper.Button("50%"))
                        animatedProgressTarget = 0.5f;
                    if (guiHelper.Button("75%"))
                        animatedProgressTarget = 0.75f;
                    if (guiHelper.Button("100%"))
                        animatedProgressTarget = 1f;
                    guiHelper.EndHorizontalGroup();
                    guiHelper.AnimatedProgress("demo_progress", animatedProgressTarget, 300);
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

            DrawSection(
                "Labels with Icons",
                () =>
                {
                    guiHelper.Label(
                        "Label with Icon",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Left,
                            Size = 14f,
                            Spacing = 4f,
                        }
                    );
                    guiHelper.Label(
                        "Muted Icon Label",
                        new IconConfig(img)
                        {
                            Position = IconPosition.Left,
                            Size = 14f,
                            Spacing = 4f,
                        },
                        ControlVariant.Muted
                    );
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

                    guiHelper.Dialog(
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

                    guiHelper.Dialog(
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

            DrawSection(
                "Dialog with Animation",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.Button("Animated Dialog"))
                        guiHelper.OpenDialog("anim_dlg");

                    guiHelper.EndHorizontalGroup();

                    guiHelper.Dialog(
                        new DialogConfig
                        {
                            Id = "anim_dlg",
                            Title = "Animated Dialog",
                            Description = "This dialog opens with a fade and scale animation.",
                            Content = () => guiHelper.Label("Watch the smooth animation!"),
                            Footer = () =>
                            {
                                if (guiHelper.Button("Close"))
                                    guiHelper.CloseDialog();
                            },
                            Width = 400,
                            Height = 200,
                        }
                    );
                }
            );

            DrawSection(
                "Dialog with Overlay Click",
                () =>
                {
                    if (guiHelper.Button("Click Overlay to Close"))
                        guiHelper.OpenDialog("overlay_dlg");

                    guiHelper.Dialog(
                        new DialogConfig
                        {
                            Id = "overlay_dlg",
                            Title = "Click Outside to Close",
                            Description = "Click the dark overlay area to close this dialog.",
                            CloseOnOverlayClick = true,
                            Content = () => guiHelper.Label("Try clicking outside this dialog!"),
                            Footer = () =>
                            {
                                if (guiHelper.Button("Or Click Here"))
                                    guiHelper.CloseDialog();
                            },
                            Width = 400,
                            Height = 200,
                        }
                    );
                }
            );

            DrawSection(
                "Dialog Z-Ordering",
                () =>
                {
                    guiHelper.Label("Open multiple dialogs to see z-ordering in action.");
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.Button("Dialog Z=100"))
                        guiHelper.OpenDialog("z100_dlg");
                    if (guiHelper.Button("Dialog Z=200"))
                        guiHelper.OpenDialog("z200_dlg");
                    guiHelper.EndHorizontalGroup();

                    guiHelper.Dialog(
                        new DialogConfig
                        {
                            Id = "z100_dlg",
                            Title = "Z-Index: 100",
                            Description = "This dialog has ZIndex=100 (lower priority).",
                            ZIndex = 100,
                            Content = () => guiHelper.Label("I should appear behind Z=200 dialog."),
                            Footer = () =>
                            {
                                if (guiHelper.Button("Close"))
                                    guiHelper.CloseDialog();
                            },
                            Width = 350,
                            Height = 180,
                        }
                    );

                    guiHelper.Dialog(
                        new DialogConfig
                        {
                            Id = "z200_dlg",
                            Title = "Z-Index: 200",
                            Description = "This dialog has ZIndex=200 (higher priority).",
                            ZIndex = 200,
                            Content = () => guiHelper.Label("I should appear in front of Z=100 dialog."),
                            Footer = () =>
                            {
                                if (guiHelper.Button("Close"))
                                    guiHelper.CloseDialog();
                            },
                            Width = 350,
                            Height = 180,
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

        // Tab state variables
        private int disabledTabIndex = 0;
        private int indicatorUnderlineIndex = 0;
        private int indicatorBgIndex = 0;
        private int indicatorBorderIndex = 0;
        private int combinedTabIndex = 0;
        private int closableTabIndex = 0;
        private string[] closableTabNames = new string[] { "Tab 1", "Tab 2", "Tab 3", "Tab 4" };
        private bool[] closableTabs = new bool[] { true, true, true, false };
        private int iconTabIndex = 0;

        void DrawTabsDemos()
        {
            DrawSection(
                "Disabled Tabs",
                () =>
                {
                    guiHelper.Label("Tabs with disabled state - disabled tabs cannot be selected");
                    var disabledTabConfigs = new TabConfig[] { new TabConfig("Enabled 1", () => guiHelper.Label("This tab is enabled")), new TabConfig("Disabled", () => guiHelper.Label("This tab is disabled"), true), new TabConfig("Enabled 2", () => guiHelper.Label("This tab is also enabled")) };

                    disabledTabIndex = guiHelper.TabsWithContent(disabledTabConfigs, disabledTabIndex);
                }
            );

            DrawSection(
                "Tabs with Selection Indicator",
                () =>
                {
                    guiHelper.Label("Tabs with different indicator styles");

                    guiHelper.Label("Underline Indicator (default)");
                    var underlineConfig = new TabsConfig(new[] { "Tab 1", "Tab 2", "Tab 3" }, indicatorUnderlineIndex)
                    {
                        Content = () => guiHelper.Label("Content for selected tab"),
                        ShowIndicator = true,
                        IndicatorStyle = IndicatorStyle.Underline,
                    };
                    indicatorUnderlineIndex = guiHelper.Tabs(underlineConfig);

                    GUILayout.Space(10);

                    guiHelper.Label("Background Indicator");
                    var bgConfig = new TabsConfig(new[] { "Tab 1", "Tab 2", "Tab 3" }, indicatorBgIndex)
                    {
                        Content = () => guiHelper.Label("Content for selected tab"),
                        ShowIndicator = true,
                        IndicatorStyle = IndicatorStyle.Background,
                    };
                    indicatorBgIndex = guiHelper.Tabs(bgConfig);

                    GUILayout.Space(10);

                    guiHelper.Label("Border Indicator");
                    var borderConfig = new TabsConfig(new[] { "Tab 1", "Tab 2", "Tab 3" }, indicatorBorderIndex)
                    {
                        Content = () => guiHelper.Label("Content for selected tab"),
                        ShowIndicator = true,
                        IndicatorStyle = IndicatorStyle.Border,
                    };
                    indicatorBorderIndex = guiHelper.Tabs(borderConfig);
                }
            );

            DrawSection(
                "Combined Features",
                () =>
                {
                    guiHelper.Label("Tabs combining multiple features");
                    var combinedConfigs = new TabConfig[]
                    {
                        new TabConfig("Overview", () => guiHelper.Label("Overview content")),
                        new TabConfig("Details", () => guiHelper.Label("Details content")),
                        new TabConfig("Disabled", () => guiHelper.Label("This won't show"), true),
                        new TabConfig("Advanced", () => guiHelper.Label("Advanced content")),
                    };

                    combinedTabIndex = guiHelper.TabsWithContent(combinedConfigs, combinedTabIndex);
                }
            );

            DrawSection(
                "Main Demo Tabs",
                () =>
                {
                    guiHelper.Label("See the main demo window tabs for examples of Vertical/Horizontal tabs.");
                }
            );

            DrawSection(
                "Closable Tabs",
                () =>
                {
                    guiHelper.Label("Tabs with close buttons - click X to close a tab");

                    if (closableTabNames.Length > 0)
                    {
                        closableTabIndex = guiHelper.ClosableTabs(ref closableTabNames, ref closableTabs, closableTabIndex, content: () => guiHelper.Label($"Content for {closableTabNames[Math.Min(closableTabIndex, closableTabNames.Length - 1)]}"));
                    }
                    else
                    {
                        guiHelper.Label("All tabs closed!");
                    }

                    GUILayout.Space(5);
                    if (guiHelper.Button("Reset Tabs", ControlVariant.Outline))
                    {
                        closableTabNames = new string[] { "Tab 1", "Tab 2", "Tab 3", "Tab 4" };
                        closableTabs = new bool[] { true, true, true, false };
                        closableTabIndex = 0;
                    }
                }
            );

            DrawSection(
                "Tabs with Icons",
                () =>
                {
                    guiHelper.Label("Tabs with icons displayed alongside labels");

                    var iconConfig = new TabsConfig(new[] { "Home", "Settings", "Profile", "Help" }, iconTabIndex) { Content = () => guiHelper.Label($"Content for tab {iconTabIndex + 1}"), TabIcons = new Texture2D[] { img, img, img, img } };
                    iconTabIndex = guiHelper.Tabs(iconConfig);
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
                    guiHelper.DataTable("main_datatable", dataTableColumns, dataTableData, showPagination: true, showSearch: true, showSelection: true, showColumnToggle: true);

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

        void DrawToastDemos()
        {
            DrawSection(
                "Quick Toast Variants",
                () =>
                {
                    guiHelper.Label("Show notifications with one click:");
                    guiHelper.HorizontalSeparator();

                    if (guiHelper.Button("Success"))
                        guiHelper.ShowSuccessToast("Success!", "Operation completed successfully");

                    if (guiHelper.Button("Error"))
                        guiHelper.ShowErrorToast("Error", "Something went wrong");

                    if (guiHelper.Button("Warning"))
                        guiHelper.ShowWarningToast("Warning", "Please be careful");

                    if (guiHelper.Button("Info"))
                        guiHelper.ShowInfoToast("Info", "Here is some information");
                }
            );

            DrawSection(
                "Advanced Toasts",
                () =>
                {
                    guiHelper.Label("Fully customized toasts using config objects:");
                    guiHelper.HorizontalSeparator();

                    if (guiHelper.Button("Toast with Action"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Confirm Action",
                            Description = "Do you want to proceed?",
                            Variant = ToastVariant.Warning,
                            DurationMs = 8000f,
                            Position = ToastPosition.Center,
                            ActionLabel = "Confirm",
                            OnAction = () =>
                                guiHelper.ShowToast(new ToastConfig
                                {
                                    Title = "Confirmed!",
                                    Description = "Action was executed",
                                    Variant = ToastVariant.Success,
                                    DurationMs = 4000f,
                                }),
                            ShowAccentBar = true,
                            ShowProgressBar = true,
                        });
                    }

                    if (guiHelper.Button("Click-to-Dismiss Toast"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Click Me!",
                            Description = "Click anywhere on this toast to dismiss",
                            Variant = ToastVariant.Info,
                            DurationMs = 10000f,
                            Position = ToastPosition.TopCenter,
                            EnableClickToDismiss = true,
                            ShowProgressBar = true,
                        });
                    }

                    if (guiHelper.Button("Long-Duration Toast"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Long Message",
                            Description = "This toast will stay for 15 seconds. Hover to pause the timer!",
                            Variant = ToastVariant.Info,
                            DurationMs = 15000f,
                            Position = ToastPosition.BottomLeft,
                            EnablePauseOnHover = true,
                            HoverPauseDelay = 0.5f,
                            ShowAccentBar = true,
                            ShowProgressBar = true,
                        });
                    }
                }
            );
            DrawSection(
                "Toast Positions",
                () =>
                {
                    guiHelper.Label("Show toasts at different screen positions:");
                    guiHelper.HorizontalSeparator();

                    if (guiHelper.Button("Top Left"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Top Left",
                            Variant = ToastVariant.Default,
                            Position = ToastPosition.TopLeft,
                            DurationMs = 3000f,
                        });
                    }

                    if (guiHelper.Button("Top Center"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Top Center",
                            Variant = ToastVariant.Info,
                            Position = ToastPosition.TopCenter,
                            DurationMs = 3000f,
                        });
                    }

                    if (guiHelper.Button("Top Right"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Top Right",
                            Variant = ToastVariant.Success,
                            Position = ToastPosition.TopRight,
                            DurationMs = 3000f,
                        });
                    }

                    if (guiHelper.Button("Center"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Center",
                            Description = "Perfectly centered on screen",
                            Variant = ToastVariant.Warning,
                            Position = ToastPosition.Center,
                            DurationMs = 3000f,
                        });
                    }

                    if (guiHelper.Button("Bottom Left"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Bottom Left",
                            Variant = ToastVariant.Error,
                            Position = ToastPosition.BottomLeft,
                            DurationMs = 3000f,
                        });
                    }

                    if (guiHelper.Button("Bottom Center"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Bottom Center",
                            Variant = ToastVariant.Default,
                            Position = ToastPosition.BottomCenter,
                            DurationMs = 3000f,
                        });
                    }

                    if (guiHelper.Button("Bottom Right"))
                    {
                        guiHelper.ShowToast(new ToastConfig
                        {
                            Title = "Bottom Right",
                            Variant = ToastVariant.Success,
                            Position = ToastPosition.BottomRight,
                            DurationMs = 3000f,
                        });
                    }
                }
            );

            DrawSection(
    "Stack Directions",
    () =>
    {
        guiHelper.Label("Show multiple toasts stacking in different directions:");
        guiHelper.HorizontalSeparator();

        if (guiHelper.Button("Stack Up"))
        {
            for (int i = 0; i < 3; i++)
            {
                guiHelper.ShowToast(new ToastConfig
                {
                    Title = $"Toast {i + 1}",
                    Description = $"Stacking upward - Message {i + 1}",
                    Variant = (ToastVariant)(i % 4),
                    Position = ToastPosition.BottomRight,
                    StackDirection = ToastStackDirection.Up,
                    DurationMs = 5000f,
                });
            }
        }

        if (guiHelper.Button("Stack Down"))
        {
            for (int i = 0; i < 3; i++)
            {
                guiHelper.ShowToast(new ToastConfig
                {
                    Title = $"Toast {i + 1}",
                    Description = $"Stacking downward - Message {i + 1}",
                    Variant = (ToastVariant)(i % 4),
                    Position = ToastPosition.TopRight,
                    StackDirection = ToastStackDirection.Down,
                    DurationMs = 5000f,
                });
            }
        }

        if (guiHelper.Button("Stack Left"))
        {
            for (int i = 0; i < 3; i++)
            {
                guiHelper.ShowToast(new ToastConfig
                {
                    Title = $"Toast {i + 1}",
                    Description = $"Stacking left - Message {i + 1}",
                    Variant = (ToastVariant)(i % 4),
                    Position = ToastPosition.CenterRight,
                    StackDirection = ToastStackDirection.Left,
                    DurationMs = 5000f,
                });
            }
        }

        if (guiHelper.Button("Stack Right"))
        {
            for (int i = 0; i < 3; i++)
            {
                guiHelper.ShowToast(new ToastConfig
                {
                    Title = $"Toast {i + 1}",
                    Description = $"Stacking right - Message {i + 1}",
                    Variant = (ToastVariant)(i % 4),
                    Position = ToastPosition.CenterLeft,
                    StackDirection = ToastStackDirection.Right,
                    DurationMs = 5000f,
                });
            }
        }
    }
);

            DrawSection(
                "Toast Management",
                () =>
                {
                    guiHelper.Label("Monitor and control active toasts:");
                    guiHelper.HorizontalSeparator();

                    int activeCount = guiHelper.GetActiveToastCount();

                    guiHelper.Label($"Active: {activeCount}");

                    if (activeCount > 0)
                    {
                        if (guiHelper.Button("Dismiss All Toasts"))
                            guiHelper.DismissAllToasts();
                    }
                }
            );
        }

        void DrawSliderDemos()
        {
            DrawSection(
                "Basic Slider",
                () =>
                {
                    guiHelper.Label("Simple slider with default settings:");
                    sliderValue = guiHelper.Slider(sliderValue, 0f, 1f);
                    guiHelper.MutedLabel($"Value: {sliderValue:F2}");
                }
            );

            DrawSection(
                "Labeled Slider",
                () =>
                {
                    guiHelper.Label("Slider with label and value display:");
                    sliderLabeledValue = guiHelper.LabeledSlider("Volume", sliderLabeledValue, 0f, 1f, true);
                }
            );

            DrawSection(
                "Slider with Step",
                () =>
                {
                    guiHelper.Label("Slider that snaps to increments of 10:");
                    sliderWithStepValue = guiHelper.LabeledSlider("Percentage", sliderWithStepValue, 0f, 100f, 10f, true);
                }
            );

            DrawSection(
                "Disabled Slider",
                () =>
                {
                    guiHelper.Label("Slider in disabled state:");
                    sliderDisabledValue = guiHelper.DisabledSlider(sliderDisabledValue, 0f, 1f);
                    guiHelper.MutedLabel("This slider cannot be interacted with");
                }
            );

            DrawSection(
                "Slider Variants",
                () =>
                {
                    guiHelper.Label("Destructive variant:");
                    sliderDestructiveValue = guiHelper.Slider(
                        new SliderConfig
                        {
                            Value = sliderDestructiveValue,
                            MinValue = 0f,
                            MaxValue = 1f,
                            Label = "Danger Level",
                            Variant = ControlVariant.Destructive,
                            ShowValue = true,
                        }
                    );
                }
            );

            DrawSection(
                "Slider Sizes",
                () =>
                {
                    guiHelper.Label("Small slider:");
                    sliderSmallValue = guiHelper.Slider(
                        new SliderConfig
                        {
                            Value = sliderSmallValue,
                            MinValue = 0f,
                            MaxValue = 1f,
                            Size = ControlSize.Small,
                        }
                    );

                    guiHelper.AddSpace(10);

                    guiHelper.Label("Large slider:");
                    sliderLargeValue = guiHelper.Slider(
                        new SliderConfig
                        {
                            Value = sliderLargeValue,
                            MinValue = 0f,
                            MaxValue = 1f,
                            Size = ControlSize.Large,
                        }
                    );
                }
            );
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
