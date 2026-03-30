using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class AppearanceShowcase : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new Rect(50, 50, 1100, 750);
        private bool showWindow = true;
        private Vector2 scrollPosition;
        private int activeTab;
        private bool settingsOpen;
        private int selectedTheme = 0;

        private string searchText = "";
        private string noteText = "";
        private float sliderValue = 65f;
        private bool toggleValue = true;
        private int selectedOption = 0;
        private float pulseTime;
        private bool animateElements = true;

        private ComponentAppearance oceanTheme;
        private ComponentAppearance sunsetTheme;
        private ComponentAppearance forestTheme;
        private ComponentAppearance currentAppearance;

        void Start()
        {
            guiHelper = new GUIHelper();
            InitializeAppearances();
            currentAppearance = oceanTheme;
        }

        void InitializeAppearances()
        {
            oceanTheme = new ComponentAppearance
            {
                BackgroundColor = new Color(0.10f, 0.18f, 0.25f, 0.95f),
                BorderColor = new Color(0.30f, 0.65f, 0.80f, 0.60f),
                ForegroundColor = new Color(0.95f, 0.97f, 0.98f, 1f),
                AccentColor = new Color(0.40f, 0.75f, 0.90f, 1f),
                BorderRadius = 10f,
                BorderThickness = 1f,
            };

            sunsetTheme = new ComponentAppearance
            {
                BackgroundColor = new Color(0.22f, 0.12f, 0.15f, 0.96f),
                BorderColor = new Color(0.90f, 0.50f, 0.40f, 0.55f),
                ForegroundColor = new Color(1f, 0.96f, 0.94f, 1f),
                AccentColor = new Color(1f, 0.60f, 0.45f, 1f),
                BorderRadius = 10f,
                BorderThickness = 1f,
            };

            forestTheme = new ComponentAppearance
            {
                BackgroundColor = new Color(0.12f, 0.20f, 0.14f, 0.95f),
                BorderColor = new Color(0.40f, 0.70f, 0.45f, 0.55f),
                ForegroundColor = new Color(0.95f, 0.97f, 0.95f, 1f),
                AccentColor = new Color(0.55f, 0.85f, 0.55f, 1f),
                BorderRadius = 10f,
                BorderThickness = 1f,
            };
        }

        void Update()
        {
            if (animateElements)
                pulseTime += Time.deltaTime * 2f;
        }

        void OnGUI()
        {
            if (!showWindow)
            {
                if (GUI.Button(new Rect(20, 20, 160, 32), "Open Showcase"))
                    showWindow = true;
                return;
            }

            windowRect = GUI.Window(404, windowRect, (GUI.WindowFunction)DrawWindow, "", GUIStyle.none);
            guiHelper.DrawOverlays();
        }

        private void DrawWindow(int id)
        {
            guiHelper.UpdateGUI(showWindow);
            if (!guiHelper.BeginGUI())
                return;

            DrawTitleBar();
            DrawContent();

            guiHelper.EndGUI();

            GUI.DragWindow(new Rect(0, 0, windowRect.width, 44));
        }

        private void DrawTitleBar()
        {
            var theme = guiHelper.CurrentTheme;
            GUI.color = theme.Elevated;
            GUI.DrawTexture(new Rect(0, 0, windowRect.width, 44), Texture2D.whiteTexture);
            GUI.color = Color.white;

            guiHelper.BeginHorizontalGroup(GUILayout.Height(44));
            guiHelper.AddSpace(16f);

            guiHelper.Label("shadcn/ui", ControlVariant.Default, appearance: currentAppearance);

            GUILayout.FlexibleSpace();

            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Settings", ControlVariant.Ghost, ControlSize.Small, appearance: currentAppearance, onClick: () => settingsOpen = !settingsOpen);
            guiHelper.AddSpace(12f);
            animateElements = guiHelper.Toggle("Animate", animateElements, appearance: currentAppearance);
            guiHelper.AddSpace(12f);
            guiHelper.Button("X", ControlVariant.Ghost, ControlSize.Small, appearance: currentAppearance, onClick: () => showWindow = false);
            guiHelper.EndHorizontalGroup();

            guiHelper.AddSpace(16f);
            guiHelper.EndHorizontalGroup();

            if (settingsOpen)
                DrawSettingsPanel();
        }

        private void DrawSettingsPanel()
        {
            int previousTheme = selectedTheme;

            guiHelper.BeginCard(-1f, 140f, ControlVariant.Default, ControlSize.Default, currentAppearance);
            guiHelper.CardHeader(() => guiHelper.CardTitle("Appearance Settings", appearance: currentAppearance));
            guiHelper.CardContent(() =>
            {
                guiHelper.BeginHorizontalGroup();
                guiHelper.Label("Theme:", ControlVariant.Default, appearance: currentAppearance);
                selectedTheme = guiHelper.Select(
                    new SelectConfig
                    {
                        Options = new[] { new SelectOption("Ocean", "ocean"), new SelectOption("Sunset", "sunset"), new SelectOption("Forest", "forest") },
                        SelectedIndex = selectedTheme,
                        Appearance = currentAppearance,
                    }
                );
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();

            if (selectedTheme != previousTheme)
                ApplyTheme(selectedTheme);
        }

        private void ApplyTheme(int themeIndex)
        {
            switch (themeIndex)
            {
                case 0:
                    currentAppearance = oceanTheme;
                    break;
                case 1:
                    currentAppearance = sunsetTheme;
                    break;
                case 2:
                    currentAppearance = forestTheme;
                    break;
            }
        }

        private void DrawContent()
        {
            guiHelper.BeginVerticalGroup();
            guiHelper.AddSpace(12f);

            activeTab = guiHelper.Tabs(new[] { "Overview", "Inputs", "Display", "Charts" }, activeTab, DrawTabContent, appearance: currentAppearance);

            guiHelper.AddSpace(8f);
            guiHelper.EndVerticalGroup();
        }

        private void DrawTabContent()
        {
            guiHelper.BeginCard(-1f, -1f, ControlVariant.Default, ControlSize.Default, currentAppearance);
            guiHelper.CardContent(() =>
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);

                guiHelper.BeginVerticalGroup();
                guiHelper.AddSpace(12f);

                switch (activeTab)
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
                        DrawChartsTab();
                        break;
                }

                guiHelper.AddSpace(12f);
                guiHelper.EndVerticalGroup();

                GUILayout.EndScrollView();
            });
            guiHelper.EndCard();
        }

        private void DrawOverviewTab()
        {
            DrawSection(
                "Button Variants",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Default", ControlVariant.Default, appearance: currentAppearance);
                    guiHelper.Button("Secondary", ControlVariant.Secondary, appearance: currentAppearance);
                    guiHelper.Button("Outline", ControlVariant.Outline, appearance: currentAppearance);
                    guiHelper.Button("Ghost", ControlVariant.Ghost, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();

                    guiHelper.AddSpace(8f);
                    guiHelper.Button("Destructive", ControlVariant.Destructive, appearance: currentAppearance);
                }
            );

            DrawSection(
                "Button Sizes",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Small", ControlVariant.Default, ControlSize.Small, appearance: currentAppearance);
                    guiHelper.Button("Default", ControlVariant.Default, ControlSize.Default, appearance: currentAppearance);
                    guiHelper.Button("Large", ControlVariant.Default, ControlSize.Large, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Themed Buttons",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button(new ButtonConfig { Text = "Ocean", Appearance = oceanTheme });
                    guiHelper.Button(new ButtonConfig { Text = "Sunset", Appearance = sunsetTheme });
                    guiHelper.Button(new ButtonConfig { Text = "Forest", Appearance = forestTheme });
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Themed Badges",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Badge(new BadgeConfig { Text = "Ocean", Appearance = oceanTheme });
                    guiHelper.Badge(new BadgeConfig { Text = "Sunset", Appearance = sunsetTheme });
                    guiHelper.Badge(new BadgeConfig { Text = "Forest", Appearance = forestTheme });
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawInputsTab()
        {
            DrawSection(
                "Text Input",
                () =>
                {
                    searchText = guiHelper.Input(
                        new InputConfig
                        {
                            Label = "Search",
                            Value = searchText,
                            Placeholder = "Search...",
                            Appearance = currentAppearance,
                        }
                    );
                }
            );

            DrawSection(
                "Text Area",
                () =>
                {
                    noteText = guiHelper.TextArea(
                        new TextAreaConfig
                        {
                            Label = "Description",
                            Value = noteText,
                            MinHeight = 60f,
                            Appearance = currentAppearance,
                        }
                    );
                }
            );

            DrawSection(
                "Slider",
                () =>
                {
                    sliderValue = guiHelper.Slider(
                        new SliderConfig
                        {
                            Label = "Progress",
                            Value = sliderValue,
                            MinValue = 0f,
                            MaxValue = 100f,
                            Appearance = currentAppearance,
                        }
                    );
                }
            );

            DrawSection(
                "Toggle",
                () =>
                {
                    toggleValue = guiHelper.Toggle("Enable feature", toggleValue, appearance: currentAppearance);
                }
            );
        }

        private void DrawDisplayTab()
        {
            DrawSection(
                "Progress Indicators",
                () =>
                {
                    if (animateElements)
                    {
                        float animated = Mathf.PingPong(Time.time * 0.3f, 1f);
                        guiHelper.AnimatedProgress("prog1", animated, appearance: currentAppearance);
                    }
                    else
                    {
                        guiHelper.Progress(0.6f, appearance: currentAppearance);
                    }

                    guiHelper.AddSpace(12f);
                    guiHelper.LabeledProgress("Upload", 0.75f, showPercentage: true, appearance: currentAppearance);
                }
            );

            DrawSection(
                "Animated Badges",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    if (animateElements)
                    {
                        guiHelper.AnimatedBadge("Live", "pulse1", ControlVariant.Destructive, appearance: currentAppearance);
                        guiHelper.AnimatedBadge("Sync", "pulse2", ControlVariant.Default, appearance: currentAppearance);
                    }
                    else
                    {
                        guiHelper.Badge("Live", ControlVariant.Destructive, appearance: currentAppearance);
                        guiHelper.Badge("Sync", ControlVariant.Default, appearance: currentAppearance);
                    }
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Separators",
                () =>
                {
                    guiHelper.Separator(SeparatorOrientation.Horizontal, appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    guiHelper.Label("Content between lines", ControlVariant.Muted, appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    guiHelper.Separator(SeparatorOrientation.Horizontal, true, appearance: currentAppearance);
                }
            );
        }

        private void DrawChartsTab()
        {
            var theme = guiHelper.CurrentTheme;

            DrawSection(
                "Line Chart",
                () =>
                {
                    guiHelper.Chart(
                        new ChartConfig(
                            new List<ChartSeries>
                            {
                                new ChartSeries("data", "Revenue", theme.Accent)
                                {
                                    Data = new List<ChartDataPoint> { new("Jan", 42), new("Feb", 51), new("Mar", 48), new("Apr", 62), new("May", 58), new("Jun", 71) },
                                },
                            },
                            ChartType.Line
                        )
                        {
                            Size = new Vector2(480f, 180f),
                            Appearance = currentAppearance,
                        }
                    );
                }
            );

            DrawSection(
                "Pie Chart",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    GUILayout.FlexibleSpace();

                    guiHelper.Chart(
                        new ChartConfig(
                            new List<ChartSeries>
                            {
                                new ChartSeries("pie", "Data")
                                {
                                    Data = new List<ChartDataPoint> { new("A", 40, theme.Accent), new("B", 35, theme.Secondary), new("C", 25, theme.Muted) },
                                },
                            },
                            ChartType.Pie
                        )
                        {
                            Size = new Vector2(180f, 180f),
                            Appearance = currentAppearance,
                        }
                    );

                    GUILayout.FlexibleSpace();
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawSection(string title, Action content)
        {
            guiHelper.BeginVerticalGroup();
            guiHelper.Label(title, ControlVariant.Default, appearance: currentAppearance);
            guiHelper.AddSpace(8f);
            content();
            guiHelper.AddSpace(20f);
            guiHelper.EndVerticalGroup();
        }

        void OnDestroy()
        {
            guiHelper?.Cleanup();
        }
    }
}
