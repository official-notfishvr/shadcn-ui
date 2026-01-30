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
    public class FullDemo_FluentBuilders : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new Rect(20, 20, 1450, 800);
        private bool showDemoWindow = false;
        private bool useVerticalTabs = false;
        private Vector2 scrollPosition;

        private int currentDemoTab = 0;
        private TabConfig[] demoTabs;
        private bool verticalTabsOnRight = false;
        private bool tabsOnBottom = false;
        private int componentCount = 0;
        private Texture2D img = new Texture2D(2, 2);

        private string passwordValue = "password123";
        private string textAreaValue = "";
        private string outlineTextAreaValue = "Outline Text Area";
        private string ghostTextAreaValue = "Ghost Text Area";
        private string labeledTextAreaValue = "Labeled Text Area";

        private float sliderValue = 0.5f;
        private float sliderWithStepValue = 50f;
        private float sliderLabeledValue = 0.75f;
        private float sliderDisabledValue = 0.3f;
        private float sliderDestructiveValue = 0.6f;
        private float sliderSmallValue = 0.4f;
        private float sliderLargeValue = 0.8f;

        private Dictionary<string, bool> toggleStates = new Dictionary<string, bool>();

        private bool dropdownOpen = false;
        private int selectIndex = 0;

        private DateTime? calendarSelectedDate;
        private DateTime? datePickerDate;
        private DateTime? datePickerWithLabelDate;

        private List<DataTableColumn> dataTableColumns;
        private List<DataTableRow> dataTableData;
        private int[] sortColumns = new int[0];
        private bool[] sortAscending = new bool[0];
        private bool[] selectedRows = new bool[0];
        private int currentPage = 0;
        private string searchQuery = "";
        private string[,] filteredData;
        private float[] columnWidths;

        private List<ChartSeries> chartSeries;

        void Start()
        {
            guiHelper = new GUIHelper();

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
            GUI.skin.horizontalScrollbar = GUIStyle.none;
            GUI.skin.verticalScrollbar = GUIStyle.none;

            GUILayout.BeginArea(new Rect(10, 10, 400, 60));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Open FluentBuilders Demo", GUILayout.Width(200), GUILayout.Height(30)))
            {
                showDemoWindow = !showDemoWindow;
            }
            GUILayout.Label($"Components: {componentCount}", GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (showDemoWindow)
            {
                windowRect = GUI.Window(102, windowRect, (GUI.WindowFunction)DrawDemoWindow, "shadcn/ui FluentBuilders Demo");
            }

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
            guiHelper.CreateLabel("shadcn/ui FluentBuilders Library").Draw();
            GUILayout.FlexibleSpace();
            guiHelper.ThemeChangerWithPreview("header_theme_changer", 180f);
            GUILayout.Space(10);
            useVerticalTabs = guiHelper.CreateToggle("Vertical Tabs", useVerticalTabs).Draw();
            GUILayout.Space(10);
            if (useVerticalTabs)
                verticalTabsOnRight = guiHelper.CreateToggle("Right Side", verticalTabsOnRight).Draw();
            else
                tabsOnBottom = guiHelper.CreateToggle("Bottom Tabs", tabsOnBottom).Draw();
            guiHelper.EndHorizontalGroup();
            guiHelper.CreateLabel("Explore all components using fluent API builders.").Muted().Draw();
            guiHelper.CreateSeparator().Horizontal().Draw();
        }

        void DrawScrollableContent()
        {
            scrollPosition = guiHelper.ScrollView(
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

        private void DrawSection(string title, Action content)
        {
            guiHelper.CreateLabel(title).Draw();
            guiHelper.CreateSeparator().Horizontal().Draw();
            content?.Invoke();
            GUILayout.Space(20);
        }

        private void DrawAllVariants(Action<ControlVariant> drawFunc)
        {
            guiHelper.BeginHorizontalGroup();
            foreach (ControlVariant variant in Enum.GetValues(typeof(ControlVariant)))
            {
                GUILayout.BeginVertical(GUILayout.Width(100));
                guiHelper.CreateLabel(variant.ToString()).Muted().Draw();
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
                guiHelper.CreateLabel(size.ToString()).Muted().Draw();
                drawFunc(size);
                GUILayout.EndVertical();
            }
            guiHelper.EndHorizontalGroup();
        }

        void DrawButtonDemos()
        {
            DrawSection(
                "Button Variants",
                () =>
                {
                    DrawAllVariants(variant => guiHelper.CreateButton("Button").Variant(variant).Draw());
                }
            );

            DrawSection(
                "Button Sizes",
                () =>
                {
                    DrawAllSizes(size => guiHelper.CreateButton("Button").Size(size).Draw());
                }
            );

            DrawSection(
                "Button States",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateButton("Normal").Draw();
                    guiHelper.CreateButton("Disabled").Disabled().Draw();
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Buttons with Icons",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateButton("Button with icon").Icon(img).Draw();
                    guiHelper.CreateButton("Left Icon").Icon(img, IconPosition.Left).Outline().Draw();
                    guiHelper.CreateButton("Secondary").Icon(img).Secondary().Draw();
                    guiHelper.EndHorizontalGroup();

                    guiHelper.CreateLabel("Icon Positioning").Draw();
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateButton("Left").Icon(img, IconPosition.Left, 14f, 4f).Draw();
                    guiHelper.CreateButton("Right").Icon(img, IconPosition.Right, 14f, 4f).Draw();
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Button Actions",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateButton("Click Me").OnClick(() => guiHelper.CreateToast("Button Clicked!", "You clicked the fluent button!").Success().Show()).Draw();

                    guiHelper.CreateButton("Destructive Action").Destructive().OnClick(() => guiHelper.CreateToast("Destructive Action", "This was a destructive action").Error().Show()).Draw();
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
                    DrawAllVariants(variant => guiHelper.CreateBadge("Badge").Variant(variant).Draw());
                }
            );

            DrawSection(
                "Badge Sizes",
                () =>
                {
                    DrawAllSizes(size => guiHelper.CreateBadge("Badge").Size(size).Draw());
                }
            );

            DrawSection(
                "Special Badges",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateBadge().Count(5).Draw();
                    guiHelper.CreateBadge("Online").Active().StatusDot().Draw();
                    guiHelper.CreateBadge("Loading").Progress(0.7f).Draw();
                    guiHelper.CreateBadge("Rounded").CornerRadius(8f).Draw();
                    guiHelper.CreateBadge("Icon").Icon(img).Draw();
                    guiHelper.EndHorizontalGroup();

                    guiHelper.CreateLabel("Badge with Icon").Draw();
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateBadge("Icon").Icon(img, IconPosition.Left, 12f, 3f).Draw();
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Animated Badges",
                () =>
                {
                    guiHelper.CreateLabel("Badges with smooth pulse animation using AnimationManager:").Draw();
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
                    guiHelper.CreateLabel("Default").Draw();
                    passwordValue = guiHelper.CreateInput().Value(passwordValue).Placeholder("Default").Password().InputWidth(200).Draw();
                }
            );

            DrawSection(
                "Input Types",
                () =>
                {
                    guiHelper.CreateLabel("Password").Draw();
                    passwordValue = guiHelper.CreateInput().Value(passwordValue).Placeholder("Password").Password().InputWidth(300).Draw();

                    guiHelper.CreateLabel("Section Header").Draw();
                    guiHelper.SectionHeader("Header Example");

                    guiHelper.CreateLabel("Render Label").Draw();
                    guiHelper.InputLabel("Rendered Label Text", 200);
                }
            );

            DrawSection(
                "Inputs with Icons",
                () =>
                {
                    guiHelper.CreateLabel("Input with Icon").Draw();
                    guiHelper.CreateInput().Value(passwordValue).Placeholder("Input with Icon").Icon(img).Draw();

                    guiHelper.CreateLabel("Icon Config").Draw();
                    guiHelper.CreateInput().Placeholder("Left Icon").Icon(img, IconPosition.Left, 14f, 4f).Draw();
                    guiHelper.CreateInput().Placeholder("Right Icon").Icon(img, IconPosition.Right, 14f, 4f).Draw();
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
                        toggleStates[key] = guiHelper.CreateToggle("Toggle", toggleStates[key]).Variant(variant).Draw();
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
                        toggleStates[key] = guiHelper.CreateToggle("Toggle", toggleStates[key]).Size(size).Draw();
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
                    toggleStates[key] = guiHelper.CreateToggle("Toggle with Icon", toggleStates[key]).Icon(img, IconPosition.Left).Draw();

                    guiHelper.CreateLabel("Icon Config").Draw();
                    string leftKey = "toggle_left";
                    if (!toggleStates.ContainsKey(leftKey))
                        toggleStates[leftKey] = false;
                    toggleStates[leftKey] = guiHelper.CreateToggle("Left", toggleStates[leftKey]).Icon(img, IconPosition.Left, 14f, 4f).Draw();

                    string rightKey = "toggle_right";
                    if (!toggleStates.ContainsKey(rightKey))
                        toggleStates[rightKey] = false;
                    toggleStates[rightKey] = guiHelper.CreateToggle("Right", toggleStates[rightKey]).Icon(img, IconPosition.Right, 14f, 4f).Draw();
                }
            );

            DrawSection(
                "Toggle Actions",
                () =>
                {
                    string key = "action_toggle";
                    if (!toggleStates.ContainsKey(key))
                        toggleStates[key] = false;
                    toggleStates[key] = guiHelper.CreateToggle("Toggle with Action", toggleStates[key]).OnToggle(value => guiHelper.CreateToast($"Toggle {(value ? "ON" : "OFF")}", "Toggle state changed").Info().Show()).Draw();
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
                        toggleStates[key] = guiHelper.CreateCheckbox("Check", toggleStates[key]).Variant(variant).Draw();
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
                    toggleStates[leftKey] = guiHelper.CreateCheckbox("Left", toggleStates[leftKey]).Icon(img, IconPosition.Left, 14f, 4f).Draw();

                    string rightKey = "check_right";
                    if (!toggleStates.ContainsKey(rightKey))
                        toggleStates[rightKey] = false;
                    toggleStates[rightKey] = guiHelper.CreateCheckbox("Right", toggleStates[rightKey]).Icon(img, IconPosition.Right, 14f, 4f).Draw();
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
                        toggleStates[key] = guiHelper.CreateCheckbox("Check", toggleStates[key]).Size(size).Draw();
                    });
                }
            );

            DrawSection(
                "Checkbox Actions",
                () =>
                {
                    string key = "action_checkbox";
                    if (!toggleStates.ContainsKey(key))
                        toggleStates[key] = false;
                    toggleStates[key] = guiHelper.CreateCheckbox("Checkbox with Action", toggleStates[key]).OnToggle(value => guiHelper.CreateToast($"Checkbox {(value ? "Checked" : "Unchecked")}", "Checkbox state changed").Info().Show()).Draw();
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
                        toggleStates[key] = guiHelper.CreateSwitch("Switch", toggleStates[key]).Variant(variant).Draw();
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
                        toggleStates[key] = guiHelper.CreateSwitch("Switch", toggleStates[key]).Size(size).Draw();
                    });
                }
            );

            DrawSection(
                "Switch Actions",
                () =>
                {
                    string key = "action_switch";
                    if (!toggleStates.ContainsKey(key))
                        toggleStates[key] = false;
                    toggleStates[key] = guiHelper.CreateSwitch("Switch with Action", toggleStates[key]).OnToggle(value => guiHelper.CreateToast($"Switch {(value ? "ON" : "OFF")}", "Switch state changed").Info().Show()).Draw();
                }
            );
        }

        void DrawTextAreaDemos()
        {
            DrawSection(
                "TextArea Variants",
                () =>
                {
                    guiHelper.CreateLabel("Default").Draw();
                    textAreaValue = guiHelper.CreateTextArea(textAreaValue).Placeholder("Default").Draw();

                    guiHelper.CreateLabel("Outline").Draw();
                    outlineTextAreaValue = guiHelper.CreateTextArea(outlineTextAreaValue).Placeholder("Outline").Outline().Draw();

                    guiHelper.CreateLabel("Ghost").Draw();
                    ghostTextAreaValue = guiHelper.CreateTextArea(ghostTextAreaValue).Placeholder("Ghost").Ghost().Draw();
                }
            );

            DrawSection(
                "Special TextAreas",
                () =>
                {
                    guiHelper.CreateLabel("Labeled").Draw();
                    labeledTextAreaValue = guiHelper.CreateTextArea(labeledTextAreaValue).TextAreaLabel("Label").Placeholder("Labeled").Draw();

                    guiHelper.CreateLabel("With Character Count").Draw();
                    textAreaValue = guiHelper.CreateTextArea(textAreaValue).MaxLength(100).ShowCharCount().Placeholder("Max 100 characters").Draw();
                }
            );

            DrawSection(
                "TextArea Features",
                () =>
                {
                    guiHelper.CreateTextArea("Disabled TextArea").Disabled().Draw();
                    guiHelper.CreateTextArea("Min Height").TextAreaMinHeight(80).Placeholder("Minimum height 80px").Draw();
                    guiHelper.CreateTextArea("Max Height").TextAreaMaxHeight(60).Placeholder("Maximum height 60px").Draw();
                }
            );
        }

        void DrawAvatarDemos()
        {
            DrawSection(
                "Avatar Sizes",
                () =>
                {
                    DrawAllSizes(size => guiHelper.CreateAvatar(img).Fallback("AV").Size(size).Draw());
                }
            );

            DrawSection(
                "Avatar Shapes",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateAvatar(img).Fallback("CR").Circle().Draw();
                    guiHelper.CreateAvatar(img).Fallback("SQ").Square().Draw();
                    guiHelper.CreateAvatar(img).Fallback("RD").Rounded().Draw();
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Avatar Features",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateAvatar(img).Fallback("ON").Online().Draw();
                    guiHelper.CreateAvatar(img).Fallback("OF").Offline().Draw();
                    guiHelper.CreateAvatar(img).Fallback("JD").NameBelow("John Doe").Draw();
                    guiHelper.CreateAvatar(img).Fallback("BR").Border(Color.cyan).Draw();
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

                    guiHelper.CreateCard("Standard").Description("Subtitle").Content("Content goes here.").Footer(() => guiHelper.CreateButton("Action").Draw()).CardSize(200, 150).Draw();

                    GUILayout.Space(10);

                    guiHelper.CreateCard().Content("Simple card content.").CardSize(200, 100).Draw();

                    GUILayout.Space(10);

                    guiHelper.CreateCard("Manual").Content("Content").Footer(() => guiHelper.CreateButton("Footer").Draw()).CardSize(200, 150).Draw();

                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Rich Cards",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateCard("Image Card").Description("Subtitle").Content("Content with image.").Image(img).CardSize(200, 250).Draw();

                    GUILayout.Space(10);

                    guiHelper.CreateCard("Avatar Card").Subtitle("User Name").Content("Content with avatar.").Avatar(img).CardSize(200, 150).Draw();
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
                    guiHelper.CreateLabel("Standard").Draw();
                    guiHelper.CreateProgress(0.6f).ProgressWidth(300).Draw();

                    guiHelper.CreateLabel("Labeled").Draw();
                    guiHelper.CreateProgress(0.4f).ProgressLabel("Loading...").ProgressWidth(300).Draw();

                    guiHelper.CreateLabel("With Percentage").Draw();
                    guiHelper.CreateProgress(0.75f).ShowPercentage().ProgressWidth(300).Draw();
                }
            );

            DrawSection(
                "Progress Features",
                () =>
                {
                    guiHelper.CreateProgress(0.3f).ProgressHeight(20).ProgressWidth(300).Draw();
                    guiHelper.CreateProgress(0.8f).ProgressLabel("Custom Height").ProgressHeight(8).ProgressWidth(300).Draw();
                }
            );

            DrawSection(
                "Animated Progress",
                () =>
                {
                    guiHelper.CreateLabel("Click buttons to animate progress smoothly:").Draw();
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.CreateButton("0%").Draw())
                        animatedProgressTarget = 0f;
                    if (guiHelper.CreateButton("25%").Draw())
                        animatedProgressTarget = 0.25f;
                    if (guiHelper.CreateButton("50%").Draw())
                        animatedProgressTarget = 0.5f;
                    if (guiHelper.CreateButton("75%").Draw())
                        animatedProgressTarget = 0.75f;
                    if (guiHelper.CreateButton("100%").Draw())
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
                    guiHelper.CreateLabel("Horizontal").Draw();
                    guiHelper.CreateSeparator().Horizontal().Draw();

                    guiHelper.CreateLabel("Labeled").Draw();
                    guiHelper.CreateSeparator().WithLabel("Section").Draw();

                    guiHelper.CreateLabel("Vertical (in horizontal group)").Draw();
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.CreateLabel("Left").Draw();
                    guiHelper.CreateSeparator().Vertical().Draw();
                    guiHelper.CreateLabel("Right").Draw();
                    guiHelper.EndHorizontalGroup();

                    guiHelper.CreateLabel("With Spacing").Draw();
                    guiHelper.CreateSeparator().Spacing(20, 20).Draw();
                    guiHelper.CreateLabel("After Spacing").Draw();
                }
            );
        }

        void DrawLabelDemos()
        {
            DrawSection(
                "Label Variants",
                () =>
                {
                    DrawAllVariants(variant => guiHelper.CreateLabel("Label").Variant(variant).Draw());
                }
            );

            DrawSection(
                "Labels with Icons",
                () =>
                {
                    guiHelper.CreateLabel("Label with Icon").Icon(img, IconPosition.Left, 14f, 4f).Draw();
                    guiHelper.CreateLabel("Muted Icon Label").Icon(img, IconPosition.Left, 14f, 4f).Muted().Draw();
                }
            );
        }

        void DrawDialogDemos()
        {
            DrawSection(
                "Dialogs",
                () =>
                {
                    if (guiHelper.CreateButton("Open Standard Dialog").Draw())
                        guiHelper.CreateDialog("std_dlg").Open();
                    if (guiHelper.CreateButton("Open Simple Dialog").Draw())
                        guiHelper.CreateDialog("smp_dlg").Open();

                    guiHelper
                        .CreateDialog("std_dlg")
                        .Title("Title")
                        .Description("Description")
                        .Content(() => guiHelper.CreateLabel("Content").Draw())
                        .Footer(() =>
                        {
                            if (guiHelper.CreateButton("Close").Draw())
                                guiHelper.CloseDialog();
                        })
                        .Draw();

                    guiHelper
                        .CreateDialog("smp_dlg")
                        .Content(() =>
                        {
                            guiHelper.CreateLabel("Simple Content").Draw();
                            if (guiHelper.CreateButton("Close").Draw())
                                guiHelper.CloseDialog();
                        })
                        .Draw();
                }
            );

            DrawSection(
                "Dialog with Animation",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.CreateButton("Animated Dialog").Draw())
                        guiHelper.CreateDialog("anim_dlg").Open();
                    guiHelper.EndHorizontalGroup();

                    guiHelper
                        .CreateDialog("anim_dlg")
                        .Title("Animated Dialog")
                        .Description("This dialog opens with a fade and scale animation.")
                        .Content(() => guiHelper.CreateLabel("Watch the smooth animation!").Draw())
                        .Footer(() =>
                        {
                            if (guiHelper.CreateButton("Close").Draw())
                                guiHelper.CloseDialog();
                        })
                        .DialogSize(400, 200)
                        .Draw();
                }
            );

            DrawSection(
                "Dialog with Overlay Click",
                () =>
                {
                    if (guiHelper.CreateButton("Click Overlay to Close").Draw())
                        guiHelper.CreateDialog("overlay_dlg").Open();

                    guiHelper
                        .CreateDialog("overlay_dlg")
                        .Title("Click Outside to Close")
                        .Description("Click the dark overlay area to close this dialog.")
                        .CloseOnOverlay()
                        .Content(() => guiHelper.CreateLabel("Try clicking outside this dialog!").Draw())
                        .Footer(() =>
                        {
                            if (guiHelper.CreateButton("Or Click Here").Draw())
                                guiHelper.CloseDialog();
                        })
                        .DialogSize(400, 200)
                        .Draw();
                }
            );

            DrawSection(
                "Dialog Z-Ordering",
                () =>
                {
                    guiHelper.CreateLabel("Open multiple dialogs to see z-ordering in action.").Draw();
                    guiHelper.BeginHorizontalGroup();
                    if (guiHelper.CreateButton("Dialog Z=100").Draw())
                        guiHelper.CreateDialog("z100_dlg").Open();
                    if (guiHelper.CreateButton("Dialog Z=200").Draw())
                        guiHelper.CreateDialog("z200_dlg").Open();
                    guiHelper.EndHorizontalGroup();

                    guiHelper
                        .CreateDialog("z100_dlg")
                        .Title("Z-Index: 100")
                        .Description("This dialog has ZIndex=100 (lower priority).")
                        .ZIndex(100)
                        .Content(() => guiHelper.CreateLabel("I should appear behind Z=200 dialog.").Draw())
                        .Footer(() =>
                        {
                            if (guiHelper.CreateButton("Close").Draw())
                                guiHelper.CloseDialog();
                        })
                        .DialogSize(350, 180)
                        .Draw();

                    guiHelper
                        .CreateDialog("z200_dlg")
                        .Title("Z-Index: 200")
                        .Description("This dialog has ZIndex=200 (higher priority).")
                        .ZIndex(200)
                        .Content(() => guiHelper.CreateLabel("I should appear in front of Z=100 dialog.").Draw())
                        .Footer(() =>
                        {
                            if (guiHelper.CreateButton("Close").Draw())
                                guiHelper.CloseDialog();
                        })
                        .DialogSize(350, 180)
                        .Draw();
                }
            );
        }

        void DrawSelectDemos()
        {
            DrawSection(
                "Select",
                () =>
                {
                    if (guiHelper.CreateButton("Open Select").Draw())
                        guiHelper.CreateSelect().Open();
                    if (guiHelper.CreateSelect().IsOpen())
                    {
                        string[] opts = { "Option 1", "Option 2", "Option 3" };
                        selectIndex = guiHelper.CreateSelect(selectIndex, opts).Draw();
                    }
                    guiHelper.CreateLabel($"Selected: {selectIndex}").Draw();
                }
            );
        }

        void DrawDropdownMenuDemos()
        {
            DrawSection(
                "Dropdown",
                () =>
                {
                    if (guiHelper.CreateButton("Open Dropdown").Draw())
                        dropdownOpen = !dropdownOpen;
                    if (dropdownOpen)
                    {
                        guiHelper.CreateDropdownMenu().Header("Header").Item("Item 1", () => dropdownOpen = false).AddSeparator().Item("Item 2", () => dropdownOpen = false).Draw();
                    }
                }
            );

            DrawSection(
                "Advanced Dropdown",
                () =>
                {
                    if (guiHelper.CreateButton("Advanced Dropdown").Draw())
                        dropdownOpen = !dropdownOpen;
                    if (dropdownOpen)
                    {
                        guiHelper
                            .CreateDropdownMenu()
                            .Header("Actions")
                            .Item(
                                "Edit",
                                () =>
                                {
                                    dropdownOpen = false;
                                    guiHelper.CreateToast("Edit", "Edit action selected").Info().Show();
                                }
                            )
                            .Item(
                                "Copy",
                                () =>
                                {
                                    dropdownOpen = false;
                                    guiHelper.CreateToast("Copy", "Copy action selected").Info().Show();
                                }
                            )
                            .AddSeparator()
                            .Item(
                                "Delete",
                                () =>
                                {
                                    dropdownOpen = false;
                                    guiHelper.CreateToast("Delete", "Delete action selected").Error().Show();
                                }
                            )
                            .Draw();
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
                    if (guiHelper.CreateButton("Open Popover").Draw())
                        guiHelper.CreatePopover().Open();
                    if (guiHelper.IsPopoverOpen())
                    {
                        guiHelper.CreatePopover(() => guiHelper.CreateLabel("Popover Content").Draw()).Draw();
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
                    guiHelper.CreateCalendar().Draw();
                    GUILayout.EndVertical();

                    guiHelper.CreateLabel($"Selected: {calendarSelectedDate}").Draw();
                }
            );
        }

        void DrawDatePickerDemos()
        {
            DrawSection(
                "Date Pickers",
                () =>
                {
                    guiHelper.CreateLabel("Basic").Draw();
                    datePickerDate = guiHelper.CreateDatePicker("dp1").Placeholder("Pick Date").SelectedDate(datePickerDate).Draw();

                    guiHelper.CreateLabel("Labeled").Draw();
                    datePickerWithLabelDate = guiHelper.CreateDatePicker("dp2").DatePickerLabel("Label").Placeholder("Pick Date").SelectedDate(datePickerWithLabelDate).Draw();

                    guiHelper.CreateLabel("Date Range").Draw();
                    guiHelper.CreateDatePicker("drp1").Placeholder("Pick Range").StartDate(DateTime.Now).EndDate(DateTime.Now.AddDays(7)).Draw();
                }
            );
        }

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
                    guiHelper.CreateLabel("Tabs with disabled state - disabled tabs cannot be selected").Draw();
                    var disabledTabConfigs = new TabConfig[]
                    {
                        new TabConfig("Enabled 1", () => guiHelper.CreateLabel("This tab is enabled").Draw()),
                        new TabConfig("Disabled", () => guiHelper.CreateLabel("This tab is disabled").Draw(), true),
                        new TabConfig("Enabled 2", () => guiHelper.CreateLabel("This tab is also enabled").Draw()),
                    };

                    disabledTabIndex = guiHelper.TabsWithContent(disabledTabConfigs, disabledTabIndex);
                }
            );

            DrawSection(
                "Tabs with Selection Indicator",
                () =>
                {
                    guiHelper.CreateLabel("Tabs with different indicator styles").Draw();

                    guiHelper.CreateLabel("Underline Indicator (default)").Draw();
                    var underlineConfig = new TabsConfig(new[] { "Tab 1", "Tab 2", "Tab 3" }, indicatorUnderlineIndex)
                    {
                        Content = () => guiHelper.CreateLabel("Content for selected tab").Draw(),
                        ShowIndicator = true,
                        IndicatorStyle = IndicatorStyle.Underline,
                    };
                    indicatorUnderlineIndex = guiHelper.Tabs(underlineConfig);

                    GUILayout.Space(10);

                    guiHelper.CreateLabel("Background Indicator").Draw();
                    var bgConfig = new TabsConfig(new[] { "Tab 1", "Tab 2", "Tab 3" }, indicatorBgIndex)
                    {
                        Content = () => guiHelper.CreateLabel("Content for selected tab").Draw(),
                        ShowIndicator = true,
                        IndicatorStyle = IndicatorStyle.Background,
                    };
                    indicatorBgIndex = guiHelper.Tabs(bgConfig);

                    GUILayout.Space(10);

                    guiHelper.CreateLabel("Border Indicator").Draw();
                    var borderConfig = new TabsConfig(new[] { "Tab 1", "Tab 2", "Tab 3" }, indicatorBorderIndex)
                    {
                        Content = () => guiHelper.CreateLabel("Content for selected tab").Draw(),
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
                    guiHelper.CreateLabel("Tabs combining multiple features").Draw();
                    var combinedConfigs = new TabConfig[]
                    {
                        new TabConfig("Overview", () => guiHelper.CreateLabel("Overview content").Draw()),
                        new TabConfig("Details", () => guiHelper.CreateLabel("Details content").Draw()),
                        new TabConfig("Disabled", () => guiHelper.CreateLabel("This won't show").Draw(), true),
                        new TabConfig("Advanced", () => guiHelper.CreateLabel("Advanced content").Draw()),
                    };

                    combinedTabIndex = guiHelper.TabsWithContent(combinedConfigs, combinedTabIndex);
                }
            );

            DrawSection(
                "Main Demo Tabs",
                () =>
                {
                    guiHelper.CreateLabel("See the main demo window tabs for examples of Vertical/Horizontal tabs.").Draw();
                }
            );

            DrawSection(
                "Closable Tabs",
                () =>
                {
                    guiHelper.CreateLabel("Tabs with close buttons - click X to close a tab").Draw();

                    if (closableTabNames.Length > 0)
                    {
                        closableTabIndex = guiHelper.ClosableTabs(ref closableTabNames, ref closableTabs, closableTabIndex, content: () => guiHelper.CreateLabel($"Content for {closableTabNames[Math.Min(closableTabIndex, closableTabNames.Length - 1)]}").Draw());
                    }
                    else
                    {
                        guiHelper.CreateLabel("All tabs closed!").Draw();
                    }

                    GUILayout.Space(5);
                    if (guiHelper.CreateButton("Reset Tabs").Outline().Draw())
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
                    guiHelper.CreateLabel("Tabs with icons displayed alongside labels").Draw();

                    var iconConfig = new TabsConfig(new[] { "Home", "Settings", "Profile", "Help" }, iconTabIndex) { Content = () => guiHelper.CreateLabel($"Content for tab {iconTabIndex + 1}").Draw(), TabIcons = new Texture2D[] { img, img, img, img } };
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
                    guiHelper
                        .CreateMenuBar()
                        .Item(
                            "File",
                            file => file.Item("New", () => guiHelper.CreateToast("New", "New file created").Info().Show()).Item("Open", () => guiHelper.CreateToast("Open", "File opened").Info().Show()).Separator().Item("Exit", () => guiHelper.CreateToast("Exit", "Application exit").Warning().Show())
                        )
                        .Item("Edit", edit => edit.Item("Cut", () => guiHelper.CreateToast("Cut", "Cut operation").Info().Show()).Item("Copy", () => guiHelper.CreateToast("Copy", "Copy operation").Info().Show()).Item("Paste", () => guiHelper.CreateToast("Paste", "Paste operation").Info().Show()))
                        .Draw();
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
                    guiHelper.CreateLabel("Bar").Draw();
                    guiHelper.CreateChart(ChartType.Bar).Series(chartSeries[0]).Series(chartSeries[1]).ChartSize(300, 200).Draw();
                    guiHelper.EndVerticalGroup();

                    guiHelper.BeginVerticalGroup();
                    guiHelper.CreateLabel("Line").Draw();
                    guiHelper.CreateChart(ChartType.Line).Series(chartSeries[0]).Series(chartSeries[1]).ChartSize(300, 200).Draw();
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

                    guiHelper.CreateLabel("Default").Draw();
                    guiHelper.CreateTable(headers).Data(data).Draw();

                    guiHelper.CreateLabel("Striped").Draw();
                    guiHelper.CreateTable(headers).Data(data).Secondary().Draw();

                    guiHelper.CreateLabel("Bordered").Draw();
                    guiHelper.CreateTable(headers).Data(data).Outline().Draw();
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
                        guiHelper.CreateLabel($"Selected IDs: {string.Join(", ", selected)}").Draw();
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
                    guiHelper.CreateLabel("Show notifications with one click:").Draw();
                    guiHelper.CreateSeparator().Horizontal().Draw();

                    if (guiHelper.CreateButton("Success").Draw())
                        guiHelper.CreateToast("Success!", "Operation completed successfully").Success().Show();

                    if (guiHelper.CreateButton("Error").Draw())
                        guiHelper.CreateToast("Error", "Something went wrong").Error().Show();

                    if (guiHelper.CreateButton("Warning").Draw())
                        guiHelper.CreateToast("Warning", "Please be careful").Warning().Show();

                    if (guiHelper.CreateButton("Info").Draw())
                        guiHelper.CreateToast("Info", "Here is some information").Info().Show();
                }
            );

            DrawSection(
                "Advanced Toast Builder",
                () =>
                {
                    guiHelper.CreateLabel("Fully customized toasts using fluent API:").Draw();
                    guiHelper.CreateSeparator().Horizontal().Draw();

                    if (guiHelper.CreateButton("Toast with Action").Draw())
                    {
                        guiHelper.CreateToast("Confirm Action", "Do you want to proceed?").Warning().Duration(8000f).Center().Action("Confirm", () => guiHelper.CreateToast("Confirmed!", "Action was executed").Success().Show()).AccentBar().ProgressBar().Show();
                    }

                    if (guiHelper.CreateButton("Click-to-Dismiss Toast").Draw())
                    {
                        guiHelper.CreateToast("Click Me!", "Click anywhere on this toast to dismiss").Info().Duration(10000f).TopCenter().ClickToDismiss().ProgressBar().Show();
                    }

                    if (guiHelper.CreateButton("Long-Duration Toast").Draw())
                    {
                        guiHelper.CreateToast("Long Message", "This toast will stay for 15 seconds. Hover to pause the timer!").Info().Duration(15000f).BottomLeft().PauseOnHover().HoverDelay(0.5f).AccentBar().ProgressBar().Show();
                    }
                }
            );

            DrawSection(
                "Toast Positions",
                () =>
                {
                    guiHelper.CreateLabel("Show toasts at different screen positions:").Draw();
                    guiHelper.CreateSeparator().Horizontal().Draw();

                    if (guiHelper.CreateButton("Top Left").Draw())
                        guiHelper.CreateToast("Top Left").TopLeft().Duration(3000f).Show();

                    if (guiHelper.CreateButton("Top Center").Draw())
                        guiHelper.CreateToast("Top Center").Info().TopCenter().Duration(3000f).Show();

                    if (guiHelper.CreateButton("Top Right").Draw())
                        guiHelper.CreateToast("Top Right").Success().TopRight().Duration(3000f).Show();

                    if (guiHelper.CreateButton("Center").Draw())
                        guiHelper.CreateToast("Center", "Perfectly centered on screen").Warning().Center().Duration(3000f).Show();

                    if (guiHelper.CreateButton("Bottom Left").Draw())
                        guiHelper.CreateToast("Bottom Left").Error().BottomLeft().Duration(3000f).Show();

                    if (guiHelper.CreateButton("Bottom Center").Draw())
                        guiHelper.CreateToast("Bottom Center").BottomCenter().Duration(3000f).Show();

                    if (guiHelper.CreateButton("Bottom Right").Draw())
                        guiHelper.CreateToast("Bottom Right").Success().BottomRight().Duration(3000f).Show();
                }
            );

            DrawSection(
                "Stack Directions",
                () =>
                {
                    guiHelper.CreateLabel("Show multiple toasts stacking in different directions:").Draw();
                    guiHelper.CreateSeparator().Horizontal().Draw();

                    if (guiHelper.CreateButton("Stack Up").Draw())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            guiHelper.CreateToast($"Toast {i + 1}", $"Stacking upward - Message {i + 1}").Variant((ToastVariant)(i % 4)).BottomRight().StackUp().Duration(5000f).Show();
                        }
                    }

                    if (guiHelper.CreateButton("Stack Down").Draw())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            guiHelper.CreateToast($"Toast {i + 1}", $"Stacking downward - Message {i + 1}").Variant((ToastVariant)(i % 4)).TopRight().StackDown().Duration(5000f).Show();
                        }
                    }

                    if (guiHelper.CreateButton("Stack Left").Draw())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            guiHelper.CreateToast($"Toast {i + 1}", $"Stacking left - Message {i + 1}").Variant((ToastVariant)(i % 4)).CenterRight().StackLeft().Duration(5000f).Show();
                        }
                    }

                    if (guiHelper.CreateButton("Stack Right").Draw())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            guiHelper.CreateToast($"Toast {i + 1}", $"Stacking right - Message {i + 1}").Variant((ToastVariant)(i % 4)).CenterLeft().StackRight().Duration(5000f).Show();
                        }
                    }
                }
            );

            DrawSection(
                "Toast Management",
                () =>
                {
                    guiHelper.CreateLabel("Monitor and control active toasts:").Draw();
                    guiHelper.CreateSeparator().Horizontal().Draw();

                    int activeCount = guiHelper.GetActiveToastCount();
                    guiHelper.CreateLabel($"Active: {activeCount}").Draw();

                    if (activeCount > 0)
                    {
                        if (guiHelper.CreateButton("Dismiss All Toasts").Draw())
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
                    guiHelper.CreateLabel("Simple slider with default settings:").Draw();
                    sliderValue = guiHelper.CreateSlider(sliderValue).Range(0f, 1f).Draw();
                    guiHelper.CreateLabel($"Value: {sliderValue:F2}").Muted().Draw();
                }
            );

            DrawSection(
                "Labeled Slider",
                () =>
                {
                    guiHelper.CreateLabel("Slider with label and value display:").Draw();
                    sliderLabeledValue = guiHelper.CreateSlider(sliderLabeledValue).Range(0f, 1f).SliderLabel("Volume").ShowValue().Draw();
                }
            );

            DrawSection(
                "Slider with Step",
                () =>
                {
                    guiHelper.CreateLabel("Slider that snaps to increments of 10:").Draw();
                    sliderWithStepValue = guiHelper.CreateSlider(sliderWithStepValue).Range(0f, 100f).Step(10f).SliderLabel("Percentage").ShowValue().Draw();
                }
            );

            DrawSection(
                "Disabled Slider",
                () =>
                {
                    guiHelper.CreateLabel("Slider in disabled state:").Draw();
                    sliderDisabledValue = guiHelper.CreateSlider(sliderDisabledValue).Range(0f, 1f).Disabled().Draw();
                    guiHelper.CreateLabel("This slider cannot be interacted with").Muted().Draw();
                }
            );

            DrawSection(
                "Slider Variants",
                () =>
                {
                    guiHelper.CreateLabel("Destructive variant:").Draw();
                    sliderDestructiveValue = guiHelper.CreateSlider(sliderDestructiveValue).Range(0f, 1f).SliderLabel("Danger Level").Destructive().ShowValue().Draw();
                }
            );

            DrawSection(
                "Slider Sizes",
                () =>
                {
                    guiHelper.CreateLabel("Small slider:").Draw();
                    sliderSmallValue = guiHelper.CreateSlider(sliderSmallValue).Range(0f, 1f).Small().Draw();

                    guiHelper.AddSpace(10);

                    guiHelper.CreateLabel("Large slider:").Draw();
                    sliderLargeValue = guiHelper.CreateSlider(sliderLargeValue).Range(0f, 1f).Large().Draw();
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
                    guiHelper.CreateButton("Left").Draw();
                    GUILayout.FlexibleSpace();
                    guiHelper.CreateButton("Right").Draw();
                    guiHelper.EndHorizontalGroup();
                }
            );
        }
    }
}
