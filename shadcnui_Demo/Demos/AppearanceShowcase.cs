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
        private Rect windowRect = new Rect(100, 80, 960, 680);
        private bool showWindow = true;
        private Vector2 scrollPosition;
        private int activeTab;
        private int selectedTheme = 0;

        private string searchText = "";
        private string noteText = "";
        private float sliderValue = 65f;
        private bool toggleValue = true;
        private bool toggleValue2 = false;
        private int selectedOption = 0;
        private float progressValue = 0.72f;

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
                BackgroundColor = new Color(0.08f, 0.12f, 0.18f, 1f),
                BorderColor = new Color(0.25f, 0.45f, 0.65f, 0.40f),
                ForegroundColor = new Color(0.90f, 0.95f, 1f, 1f),
                AccentColor = new Color(0.35f, 0.70f, 0.95f, 1f),
                BorderRadius = 8f,
                BorderThickness = 1f,
            };

            sunsetTheme = new ComponentAppearance
            {
                BackgroundColor = new Color(0.18f, 0.10f, 0.12f, 1f),
                BorderColor = new Color(0.75f, 0.35f, 0.30f, 0.40f),
                ForegroundColor = new Color(1f, 0.92f, 0.90f, 1f),
                AccentColor = new Color(0.95f, 0.45f, 0.35f, 1f),
                BorderRadius = 8f,
                BorderThickness = 1f,
            };

            forestTheme = new ComponentAppearance
            {
                BackgroundColor = new Color(0.08f, 0.15f, 0.10f, 1f),
                BorderColor = new Color(0.30f, 0.60f, 0.35f, 0.40f),
                ForegroundColor = new Color(0.90f, 0.97f, 0.92f, 1f),
                AccentColor = new Color(0.45f, 0.85f, 0.45f, 1f),
                BorderRadius = 8f,
                BorderThickness = 1f,
            };
        }

        void OnGUI()
        {
            if (!showWindow)
            {
                if (GUI.Button(new Rect(20, 20, 140, 32), "Open Showcase"))
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

            DrawHeader();
            DrawLayout();

            guiHelper.EndGUI();
            GUI.DragWindow(new Rect(0, 0, windowRect.width, 48));
        }

        private void DrawHeader()
        {
            var theme = guiHelper.CurrentTheme;
            GUI.color = new Color(0.06f, 0.06f, 0.08f, 0.98f);
            GUI.DrawTexture(new Rect(0, 0, windowRect.width, 48), Texture2D.whiteTexture);
            GUI.color = Color.white;

            guiHelper.BeginHorizontalGroup(GUILayout.Height(48));
            guiHelper.AddSpace(20f);

            var titleStyle = guiHelper.GetStyleManager().GetLabelStyle(ControlVariant.Default);
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.fontSize = guiHelper.GetStyleManager().GetScaledFontSize(1.1f);
            UnityHelpers.Label("Appearance", titleStyle);

            GUILayout.FlexibleSpace();

            DrawThemeSelector();

            guiHelper.AddSpace(16f);
            guiHelper.Button("\u2715", ControlVariant.Ghost, ControlSize.Small, appearance: currentAppearance, onClick: () => showWindow = false);
            guiHelper.AddSpace(16f);
            guiHelper.EndHorizontalGroup();
        }

        private void DrawThemeSelector()
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("Theme:", ControlVariant.Muted, appearance: currentAppearance);
            guiHelper.AddSpace(8f);

            string[] themes = { "Ocean", "Sunset", "Forest" };
            for (int i = 0; i < themes.Length; i++)
            {
                bool isActive = selectedTheme == i;
                var variant = isActive ? ControlVariant.Default : ControlVariant.Ghost;
                guiHelper.Button(
                    themes[i],
                    variant,
                    ControlSize.Small,
                    appearance: currentAppearance,
                    onClick: () =>
                    {
                        selectedTheme = i;
                        ApplyTheme(i);
                    }
                );
                guiHelper.AddSpace(4f);
            }

            guiHelper.EndHorizontalGroup();
        }

        private void ApplyTheme(int index)
        {
            currentAppearance = index switch
            {
                0 => oceanTheme,
                1 => sunsetTheme,
                2 => forestTheme,
                _ => oceanTheme,
            };
        }

        private void DrawLayout()
        {
            guiHelper.BeginHorizontalGroup(GUILayout.ExpandHeight(true));

            DrawSidebar();
            DrawMainContent();

            guiHelper.EndHorizontalGroup();
        }

        private void DrawSidebar()
        {
            var sidebarBg = new Color(0.05f, 0.05f, 0.06f, 0.6f);
            float sidebarWidth = 180f;

            GUI.color = sidebarBg;
            GUI.DrawTexture(new Rect(0, 48, sidebarWidth, windowRect.height - 48), Texture2D.whiteTexture);
            GUI.color = Color.white;

            guiHelper.BeginVerticalGroup(GUILayout.Width(sidebarWidth), GUILayout.ExpandHeight(true));
            guiHelper.AddSpace(16f);

            string[] tabs = { "Overview", "Controls", "Feedback", "Data" };
            for (int i = 0; i < tabs.Length; i++)
            {
                bool isActive = activeTab == i;
                var variant = isActive ? ControlVariant.Default : ControlVariant.Ghost;
                guiHelper.Button(tabs[i], variant, ControlSize.Default, appearance: currentAppearance, onClick: () => activeTab = i);
                guiHelper.AddSpace(10f);
            }

            GUILayout.FlexibleSpace();

            guiHelper.Separator(SeparatorOrientation.Horizontal, appearance: currentAppearance);
            guiHelper.AddSpace(8f);
            guiHelper.Label("v1.0.0", ControlVariant.Muted, appearance: currentAppearance);
            guiHelper.AddSpace(12f);

            guiHelper.EndVerticalGroup();
        }

        private void DrawMainContent()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.AddSpace(20f);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);

            guiHelper.BeginVerticalGroup();
            guiHelper.AddSpace(8f);

            switch (activeTab)
            {
                case 0:
                    DrawOverviewTab();
                    break;
                case 1:
                    DrawControlsTab();
                    break;
                case 2:
                    DrawFeedbackTab();
                    break;
                case 3:
                    DrawDataTab();
                    break;
            }

            guiHelper.AddSpace(24f);
            guiHelper.EndVerticalGroup();

            GUILayout.EndScrollView();
            guiHelper.EndVerticalGroup();
        }

        private void DrawOverviewTab()
        {
            DrawSection(
                "Colors",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    DrawColorSwatch("Background", currentAppearance.BackgroundColor ?? Color.gray);
                    guiHelper.AddSpace(12f);
                    DrawColorSwatch("Border", currentAppearance.BorderColor ?? Color.gray);
                    guiHelper.AddSpace(12f);
                    DrawColorSwatch("Text", currentAppearance.ForegroundColor ?? Color.gray);
                    guiHelper.AddSpace(12f);
                    DrawColorSwatch("Accent", currentAppearance.AccentColor ?? Color.gray);
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Buttons",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Default", ControlVariant.Default, appearance: currentAppearance);
                    guiHelper.Button("Secondary", ControlVariant.Secondary, appearance: currentAppearance);
                    guiHelper.Button("Destructive", ControlVariant.Destructive, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();

                    guiHelper.AddSpace(8f);
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Outline", ControlVariant.Outline, appearance: currentAppearance);
                    guiHelper.Button("Ghost", ControlVariant.Ghost, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Sizes",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Button("Small", ControlVariant.Default, ControlSize.Small, appearance: currentAppearance);
                    guiHelper.Button("Default", ControlVariant.Default, appearance: currentAppearance);
                    guiHelper.Button("Large", ControlVariant.Default, ControlSize.Large, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Badges",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.Badge("Default", appearance: currentAppearance);
                    guiHelper.Badge("Secondary", ControlVariant.Secondary, appearance: currentAppearance);
                    guiHelper.Badge("Destructive", ControlVariant.Destructive, appearance: currentAppearance);
                    guiHelper.Badge("Outline", ControlVariant.Outline, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();
                }
            );
        }

        private void DrawColorSwatch(string label, Color color)
        {
            guiHelper.BeginVerticalGroup();
            var rect = GUILayoutUtility.GetRect(48f, 48f);
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            guiHelper.Label(label, ControlVariant.Muted, appearance: currentAppearance);
            guiHelper.EndVerticalGroup();
        }

        private void DrawControlsTab()
        {
            DrawSection(
                "Input",
                () =>
                {
                    searchText = guiHelper.Input(
                        new InputConfig
                        {
                            Label = "Search",
                            Value = searchText,
                            Placeholder = "Type to search...",
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
                            Label = "Notes",
                            Value = noteText,
                            Placeholder = "Write something...",
                            MinHeight = 80f,
                            Appearance = currentAppearance,
                        }
                    );
                }
            );

            DrawSection(
                "Toggle",
                () =>
                {
                    guiHelper.BeginVerticalGroup();
                    toggleValue = guiHelper.Toggle("Enable notifications", toggleValue, appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    toggleValue2 = guiHelper.Toggle("Dark mode", toggleValue2, appearance: currentAppearance);
                    guiHelper.EndVerticalGroup();
                }
            );

            DrawSection(
                "Slider",
                () =>
                {
                    sliderValue = guiHelper.Slider(
                        new SliderConfig
                        {
                            Label = "Volume",
                            Value = sliderValue,
                            MinValue = 0f,
                            MaxValue = 100f,
                            Appearance = currentAppearance,
                        }
                    );
                }
            );
        }

        private void DrawFeedbackTab()
        {
            DrawSection(
                "Progress",
                () =>
                {
                    guiHelper.LabeledProgress("Upload progress", progressValue, appearance: currentAppearance);
                    guiHelper.AddSpace(12f);
                    guiHelper.Progress(0.45f, appearance: currentAppearance);
                }
            );

            DrawSection(
                "Animated Progress",
                () =>
                {
                    float animated = Mathf.PingPong(Time.time * 0.4f, 1f);
                    guiHelper.AnimatedProgress("anim1", animated, appearance: currentAppearance);
                }
            );

            DrawSection(
                "Badges",
                () =>
                {
                    guiHelper.BeginHorizontalGroup();
                    guiHelper.AnimatedBadge("Live", "live1", ControlVariant.Destructive, appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    guiHelper.AnimatedBadge("Syncing", "sync1", appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    guiHelper.Badge("Stable", ControlVariant.Default, appearance: currentAppearance);
                    guiHelper.EndHorizontalGroup();
                }
            );

            DrawSection(
                "Separators",
                () =>
                {
                    guiHelper.Label("Section A", ControlVariant.Default, appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    guiHelper.Separator(SeparatorOrientation.Horizontal, appearance: currentAppearance);
                    guiHelper.AddSpace(8f);
                    guiHelper.Label("Section B", ControlVariant.Default, appearance: currentAppearance);
                }
            );
        }

        private void DrawDataTab()
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
                                new ChartSeries("revenue", "Revenue", theme.Accent)
                                {
                                    Data = new List<ChartDataPoint> { new("Jan", 42), new("Feb", 51), new("Mar", 48), new("Apr", 62), new("May", 58), new("Jun", 71) },
                                },
                            },
                            ChartType.Line
                        )
                        {
                            Size = new Vector2(520f, 200f),
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
                                new ChartSeries("pie", "Distribution")
                                {
                                    Data = new List<ChartDataPoint> { new("A", 40, theme.Accent), new("B", 35, theme.Secondary), new("C", 25, theme.Muted) },
                                },
                            },
                            ChartType.Pie
                        )
                        {
                            Size = new Vector2(200f, 200f),
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
            guiHelper.AddSpace(10f);
            content();
            guiHelper.AddSpace(24f);
            guiHelper.EndVerticalGroup();
        }

        void OnDestroy()
        {
            guiHelper?.Cleanup();
        }
    }
}
