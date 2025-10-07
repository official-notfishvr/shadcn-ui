using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public enum ChartType
    {
        Line,
        Bar,
        Area,
        Pie,
        Scatter,
    }

    [System.Serializable]
    public class ChartDataPoint
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public Color Color { get; set; }
        public Dictionary<string, object> Payload { get; set; }

        public ChartDataPoint(string name, float value, Color color = default)
        {
            Name = name;
            Value = value;
            Color = color == default ? Color.blue : color;
            Payload = new Dictionary<string, object>();
        }
    }

    [System.Serializable]
    public class ChartSeries
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public Color Color { get; set; }
        public List<ChartDataPoint> Data { get; set; }
        public bool Visible { get; set; }

        public ChartSeries(string key, string label, Color color = default)
        {
            Key = key;
            Label = label;
            Color = color == default ? Color.blue : color;
            Data = new List<ChartDataPoint>();
            Visible = true;
        }
    }

    [System.Serializable]
    public class ChartConfig
    {
        public Dictionary<string, ChartSeriesConfig> SeriesConfigs { get; set; }

        public ChartConfig()
        {
            SeriesConfigs = new Dictionary<string, ChartSeriesConfig>();
        }
    }

    [System.Serializable]
    public class ChartSeriesConfig
    {
        public string Label { get; set; }
        public Color Color { get; set; }
        public Texture2D Icon { get; set; }

        public ChartSeriesConfig(string label, Color color = default, Texture2D icon = null)
        {
            Label = label;
            Color = color == default ? Color.blue : color;
            Icon = icon;
        }
    }

    public class Chart
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;
        private ChartConfig config;
        private List<ChartSeries> series;
        private Vector2 chartSize;
        private Rect chartRect;

        public Chart(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
            config = new ChartConfig();
            series = new List<ChartSeries>();
            chartSize = new Vector2(400, 300);
        }

        public ChartConfig Config => config;
        public List<ChartSeries> Series => series;
        public Vector2 ChartSize
        {
            get => chartSize;
            set => chartSize = value;
        }

        public void DrawChart(ChartType chartType, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            List<GUILayoutOption> layoutOptions = new List<GUILayoutOption>(options);
            if (layoutOptions.Count == 0)
            {
                layoutOptions.Add(GUILayout.Width(chartSize.x));
                layoutOptions.Add(GUILayout.Height(chartSize.y));
            }

            layoutComponents.BeginVerticalGroup(styleManager.cardStyle, layoutOptions.ToArray());

            DrawChartContent(chartType);

            layoutComponents.EndVerticalGroup();
        }

        private void DrawChartContent(ChartType chartType)
        {
#if IL2CPP_MELONLOADER
            var chartArea = GUILayoutUtility.GetRect(chartSize.x, chartSize.y, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) }));
#else
            var chartArea = GUILayoutUtility.GetRect(chartSize.x, chartSize.y, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
#endif

            if (Event.current.type == EventType.Repaint)
            {
                chartRect = chartArea;

                switch (chartType)
                {
                    case ChartType.Line:
                        DrawLineChart();
                        break;
                    case ChartType.Bar:
                        DrawBarChart();
                        break;
                    case ChartType.Area:
                        DrawAreaChart();
                        break;
                    case ChartType.Pie:
                        DrawPieChart();
                        break;
                    case ChartType.Scatter:
                        DrawScatterChart();
                        break;
                }
            }
        }

        private void DrawLineChart()
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes();

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                DrawLineSeries(seriesData);
            }
        }

        private void DrawBarChart()
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes();

            float chartWidth = chartRect.width - 60;
            float chartHeight = chartRect.height - 40;
            float maxValue = GetMaxValue();

            int dataCount = series.FirstOrDefault()?.Data.Count ?? 0;
            if (dataCount == 0)
                return;

            float groupWidth = chartWidth / dataCount;
            float barWidth = (groupWidth * 0.8f) / series.Count;
            float groupSpacing = groupWidth * 0.1f;

            for (int i = 0; i < dataCount; i++)
            {
                float groupX = chartRect.x + 40 + (i * groupWidth) + groupSpacing;

                for (int seriesIndex = 0; seriesIndex < series.Count; seriesIndex++)
                {
                    var seriesData = series[seriesIndex];
                    if (!seriesData.Visible || i >= seriesData.Data.Count)
                        continue;

                    var dataPoint = seriesData.Data[i];
                    float barHeight = (dataPoint.Value / maxValue) * chartHeight;
                    float barX = groupX + (seriesIndex * barWidth);
                    float barY = chartRect.y + chartHeight - barHeight;

                    Rect barRect = new Rect(barX, barY, barWidth - 2, barHeight);

                    DrawRoundedRect(barRect, seriesData.Color, 4f);
                }
            }
        }

        private void DrawAreaChart()
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes();

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                DrawAreaSeries(seriesData);
            }
        }

        private void DrawPieChart()
        {
            if (series.Count == 0)
                return;

            Vector2 center = new Vector2(chartRect.x + chartRect.width / 2, chartRect.y + chartRect.height / 2);
            float radius = Mathf.Min(chartRect.width, chartRect.height) / 2 * 0.8f;

            float totalValue = series.SelectMany(s => s.Data).Sum(d => d.Value);
            float currentAngle = 0f;

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                foreach (var dataPoint in seriesData.Data)
                {
                    float sliceAngle = (dataPoint.Value / totalValue) * 360f;
                    DrawPieSlice(center, radius, currentAngle, sliceAngle, dataPoint.Color);
                    currentAngle += sliceAngle;
                }
            }
        }

        private void DrawScatterChart()
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes();

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                DrawScatterSeries(seriesData);
            }
        }

        private void DrawGrid()
        {
            var theme = guiHelper.GetStyleManager().GetTheme();

            for (int i = 1; i < 5; i++)
            {
                float y = chartRect.y + (chartRect.height / 5) * i;
                DrawLine(new Vector2(chartRect.x + 40, y), new Vector2(chartRect.x + chartRect.width - 20, y), new Color(theme.ChartGridColor.r, theme.ChartGridColor.g, theme.ChartGridColor.b, 0.3f));
            }
        }

        private void DrawAxes()
        {
            try
            {
                var theme = guiHelper.GetStyleManager().GetTheme();
                var styleManager = guiHelper.GetStyleManager();

                DrawLine(new Vector2(chartRect.x + 40, chartRect.y + chartRect.height - 20), new Vector2(chartRect.x + chartRect.width - 20, chartRect.y + chartRect.height - 20), new Color(theme.ChartAxisColor.r, theme.ChartAxisColor.g, theme.ChartAxisColor.b, 0.5f));

                if (series.Count > 0 && series[0].Data.Count > 0 && chartRect.width > 60)
                {
                    float labelWidth = (chartRect.width - 60) / series[0].Data.Count;
                    if (labelWidth > 0)
                    {
                        for (int i = 0; i < series[0].Data.Count; i++)
                        {
                            float x = chartRect.x + 40 + (i * labelWidth) + (labelWidth / 2);
                            string label = series[0].Data[i].Name.Length > 3 ? series[0].Data[i].Name.Substring(0, 3) : series[0].Data[i].Name;

                            Rect labelRect = new Rect(x - 15, chartRect.y + chartRect.height - 15, 30, 20);
                            if (labelRect.width > 0 && labelRect.height > 0)
                            {
                                Color originalColor = UnityEngine.GUI.color;
                                UnityEngine.GUI.color = theme.MutedColor;
                                UnityEngine.GUI.Label(labelRect, label, styleManager.GetChartAxisStyle());
                                UnityEngine.GUI.color = originalColor;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"DrawAxes error: {ex.Message}");
            }
        }

        private void DrawLineSeries(ChartSeries seriesData)
        {
            if (seriesData.Data.Count < 2)
                return;

            Vector2[] points = new Vector2[seriesData.Data.Count];
            float maxValue = GetMaxValue();
            float chartWidth = chartRect.width - 60;
            float chartHeight = chartRect.height - 40;

            for (int i = 0; i < seriesData.Data.Count; i++)
            {
                float x = chartRect.x + 40 + (i / (float)(seriesData.Data.Count - 1)) * chartWidth;
                float y = chartRect.y + chartHeight - (seriesData.Data[i].Value / maxValue) * chartHeight;
                points[i] = new Vector2(x, y);
            }

            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawThickLine(points[i], points[i + 1], seriesData.Color, 2f);
            }

            foreach (var point in points)
            {
                DrawCircle(point, 4f, seriesData.Color);
                DrawCircle(point, 2f, Color.white);
            }
        }

        private void DrawAreaSeries(ChartSeries seriesData)
        {
            if (seriesData.Data.Count < 2)
                return;

            Vector2[] points = new Vector2[seriesData.Data.Count + 2];
            float maxValue = GetMaxValue();
            float chartWidth = chartRect.width - 60;
            float chartHeight = chartRect.height - 40;

            points[0] = new Vector2(chartRect.x + 40, chartRect.y + chartHeight);

            for (int i = 0; i < seriesData.Data.Count; i++)
            {
                float x = chartRect.x + 40 + (i / (float)(seriesData.Data.Count - 1)) * chartWidth;
                float y = chartRect.y + chartHeight - (seriesData.Data[i].Value / maxValue) * chartHeight;
                points[i + 1] = new Vector2(x, y);
            }

            points[points.Length - 1] = new Vector2(chartRect.x + chartWidth + 40, chartRect.y + chartHeight);

            Color areaColor = new Color(seriesData.Color.r, seriesData.Color.g, seriesData.Color.b, 0.3f);
            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawLine(points[i], points[i + 1], areaColor);
            }

            for (int i = 1; i < points.Length - 2; i++)
            {
                DrawLine(points[i], points[i + 1], seriesData.Color);
            }
        }

        private void DrawScatterSeries(ChartSeries seriesData)
        {
            float maxValue = GetMaxValue();
            float chartWidth = chartRect.width - 60;
            float chartHeight = chartRect.height - 40;

            for (int i = 0; i < seriesData.Data.Count; i++)
            {
                float x = chartRect.x + 40 + (i / (float)(seriesData.Data.Count - 1)) * chartWidth;
                float y = chartRect.y + chartHeight - (seriesData.Data[i].Value / maxValue) * chartHeight;
                DrawCircle(new Vector2(x, y), 4f, seriesData.Color);
            }
        }

        private void DrawPieSlice(Vector2 center, float radius, float startAngle, float sliceAngle, Color color)
        {
            int segments = Mathf.Max(3, Mathf.RoundToInt(sliceAngle / 10f));
            float angleStep = sliceAngle / segments;

            Vector2 prevPoint = center + new Vector2(Mathf.Cos(startAngle * (3.1415927f / 180f)) * radius, Mathf.Sin(startAngle * (3.1415927f / 180f)) * radius);

            for (int i = 1; i <= segments; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 point = center + new Vector2(Mathf.Cos(angle * (3.1415927f / 180f)) * radius, Mathf.Sin(angle * (3.1415927f / 180f)) * radius);

                DrawLine(center, prevPoint, color);
                DrawLine(prevPoint, point, color);
                prevPoint = point;
            }
            DrawLine(center, prevPoint, color);
        }

        private float GetMaxValue()
        {
            return series.SelectMany(s => s.Data).Max(d => d.Value);
        }

        private void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            float distance = Vector2.Distance(start, end);

            Color originalColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;

            int segments = Mathf.Max(1, Mathf.RoundToInt(distance / 2f));
            for (int i = 0; i < segments; i++)
            {
                Vector2 pos = Vector2.Lerp(start, end, i / (float)segments);
                UnityEngine.GUI.DrawTexture(new Rect(pos.x - 0.5f, pos.y - 0.5f, 1f, 1f), Texture2D.whiteTexture);
            }

            UnityEngine.GUI.color = originalColor;
        }

        private void DrawCircle(Vector2 center, float radius, Color color)
        {
            Color originalColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;
            UnityEngine.GUI.DrawTexture(new Rect(center.x - radius, center.y - radius, radius * 2, radius * 2), Texture2D.whiteTexture);
            UnityEngine.GUI.color = originalColor;
        }

        private void DrawThickLine(Vector2 start, Vector2 end, Color color, float thickness)
        {
            float distance = Vector2.Distance(start, end);
            Vector2 direction = (end - start).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (thickness / 2f);

            Color originalColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;

            int segments = Mathf.Max(1, Mathf.RoundToInt(distance / 1f));
            for (int i = 0; i < segments; i++)
            {
                Vector2 pos = Vector2.Lerp(start, end, i / (float)segments);
                UnityEngine.GUI.DrawTexture(new Rect(pos.x - thickness / 2, pos.y - thickness / 2, thickness, thickness), Texture2D.whiteTexture);
            }

            UnityEngine.GUI.color = originalColor;
        }

        private void DrawRoundedRect(Rect rect, Color color, float cornerRadius)
        {
            Color originalColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;
            UnityEngine.GUI.DrawTexture(rect, Texture2D.whiteTexture);
            UnityEngine.GUI.color = originalColor;
        }

        public void AddSeries(ChartSeries seriesData)
        {
            series.Add(seriesData);
        }

        public void ClearSeries()
        {
            series.Clear();
        }

        public void SetConfig(ChartConfig chartConfig)
        {
            config = chartConfig;
        }

        public void AddDataPoint(string seriesKey, ChartDataPoint dataPoint)
        {
            var targetSeries = series.FirstOrDefault(s => s.Key == seriesKey);
            if (targetSeries != null)
            {
                targetSeries.Data.Add(dataPoint);
            }
        }

        public void SetSeriesVisibility(string seriesKey, bool visible)
        {
            var targetSeries = series.FirstOrDefault(s => s.Key == seriesKey);
            if (targetSeries != null)
            {
                targetSeries.Visible = visible;
            }
        }
    }
}
