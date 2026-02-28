using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using shadcnui.GUIComponents.Data;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class ChartsExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 650, 700);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        void Start()
        {
            gui = new GUIHelper();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(7, windowRect, DrawChartsWindow, "Charts Example");
            }
            gui.DrawOverlay();
        }

        private List<ChartDataPoint> CreateDataPoints(string name, params float[] values)
        {
            return values.Select((v, i) => new ChartDataPoint($"{name}_{i}", v, Color.white)).ToList();
        }

        void DrawChartsWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI()) return;

            scroll = gui.ScrollView(scroll, () =>
            {
                gui.BeginVerticalGroup();

                gui.Label("Bar Chart", ControlVariant.Default);
                var barSeries = new List<ChartSeries>
                {
                    new ChartSeries("sales", "Sales", new Color(0.2f, 0.6f, 1f))
                    {
                        Data = CreateDataPoints("sales", 120, 200, 150, 80, 70, 110, 130)
                    },
                    new ChartSeries("revenue", "Revenue", new Color(0.3f, 0.8f, 0.4f))
                    {
                        Data = CreateDataPoints("revenue", 100, 180, 140, 90, 60, 100, 120)
                    }
                };
                gui.Chart(barSeries, ChartType.Bar, new Vector2(500, 200));

                gui.AddSpace(20);

                gui.Label("Line Chart", ControlVariant.Default);
                var lineSeries = new List<ChartSeries>
                {
                    new ChartSeries("visitors", "Visitors", new Color(0.9f, 0.4f, 0.4f))
                    {
                        Data = CreateDataPoints("visitors", 30, 45, 35, 50, 40, 60, 55)
                    }
                };
                gui.Chart(lineSeries, ChartType.Line, new Vector2(500, 150));

                gui.AddSpace(20);

                gui.Label("Area Chart", ControlVariant.Default);
                var areaSeries = new List<ChartSeries>
                {
                    new ChartSeries("growth", "Growth", new Color(0.5f, 0.3f, 0.9f, 0.6f))
                    {
                        Data = CreateDataPoints("growth", 20, 25, 35, 40, 45, 55, 65)
                    }
                };
                gui.Chart(areaSeries, ChartType.Area, new Vector2(500, 150));

                gui.AddSpace(20);

                gui.Label("Pie Chart", ControlVariant.Default);
                var pieSeries = new List<ChartSeries>
                {
                    new ChartSeries("catA", "Category A", new Color(0.2f, 0.6f, 1f))
                    {
                        Data = CreateDataPoints("catA", 35)
                    },
                    new ChartSeries("catB", "Category B", new Color(0.3f, 0.8f, 0.4f))
                    {
                        Data = CreateDataPoints("catB", 25)
                    },
                    new ChartSeries("catC", "Category C", new Color(0.9f, 0.4f, 0.4f))
                    {
                        Data = CreateDataPoints("catC", 20)
                    },
                    new ChartSeries("catD", "Category D", new Color(0.9f, 0.7f, 0.2f))
                    {
                        Data = CreateDataPoints("catD", 20)
                    }
                };
                gui.Chart(pieSeries, ChartType.Pie, new Vector2(200, 200));

                gui.EndVerticalGroup();
            }, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
