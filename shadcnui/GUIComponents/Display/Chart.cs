using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [Serializable]
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

    [Serializable]
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

    [Serializable]
    public class ChartConfig
    {
        public List<ChartSeries> Series { get; set; }
        public ChartType ChartType { get; set; }
        public Vector2 Size { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public ChartConfig(List<ChartSeries> series, ChartType chartType)
        {
            Series = series;
            ChartType = chartType;
            Size = new Vector2(400, 300);
            Options = Array.Empty<GUILayoutOption>();
        }
    }

    public class Chart
    {
        private readonly GUIHelper _guiHelper;
        private readonly Layout _layoutComponents;
        private Rect _chartRect;

        public Chart(GUIHelper helper)
        {
            _guiHelper = helper;
            _layoutComponents = new Layout(helper);
        }

        public void DrawChart(ChartConfig config)
        {
            var styleManager = _guiHelper.GetStyleManager();

            var layoutOptions = new List<GUILayoutOption>(config.Options);
            if (layoutOptions.Count == 0)
            {
                layoutOptions.Add(GUILayout.Width(config.Size.x));
                layoutOptions.Add(GUILayout.Height(config.Size.y));
            }

            _layoutComponents.BeginVerticalGroup(styleManager.cardStyle, layoutOptions.ToArray());

            DrawChartContent(config);

            _layoutComponents.EndVerticalGroup();
        }

        private void DrawChartContent(ChartConfig config)
        {
            var chartArea = GUILayoutUtility.GetRect(config.Size.x, config.Size.y,
#if IL2CPP_MELONLOADER
                new UnhollowerBaseLib.Il2CppReferenceArray<UnityEngine.GUILayoutOption>(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) })
#else
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)
#endif
            );

            if (Event.current.type != EventType.Repaint)
                return;

            _chartRect = chartArea;

            switch (config.ChartType)
            {
                case ChartType.Line:
                    DrawLineChart(config.Series);
                    break;
                case ChartType.Bar:
                    DrawBarChart(config.Series);
                    break;
                case ChartType.Area:
                    DrawAreaChart(config.Series);
                    break;
                case ChartType.Pie:
                    DrawPieChart(config.Series);
                    break;
                case ChartType.Scatter:
                    DrawScatterChart(config.Series);
                    break;
            }
        }

        private void DrawLineChart(List<ChartSeries> series)
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes(series);

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                DrawLineSeries(seriesData, series);
            }
        }

        private void DrawBarChart(List<ChartSeries> series)
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes(series);

            var chartWidth = _chartRect.width - 60;
            var chartHeight = _chartRect.height - 40;
            var maxValue = GetMaxValue(series);

            var dataCount = series.FirstOrDefault()?.Data.Count ?? 0;
            if (dataCount == 0)
                return;

            var groupWidth = chartWidth / dataCount;
            var barWidth = (groupWidth * 0.8f) / series.Count;
            var groupSpacing = groupWidth * 0.1f;

            for (var i = 0; i < dataCount; i++)
            {
                var groupX = _chartRect.x + 40 + (i * groupWidth) + groupSpacing;

                for (var seriesIndex = 0; seriesIndex < series.Count; seriesIndex++)
                {
                    var seriesData = series[seriesIndex];
                    if (!seriesData.Visible || i >= seriesData.Data.Count)
                        continue;

                    var dataPoint = seriesData.Data[i];
                    var barHeight = (dataPoint.Value / maxValue) * chartHeight;
                    var barX = groupX + (seriesIndex * barWidth);
                    var barY = _chartRect.y + chartHeight - barHeight;

                    var barRect = new Rect(barX, barY, barWidth - 2, barHeight);

                    DrawRoundedRect(barRect, seriesData.Color, 4f);
                }
            }
        }

        private void DrawAreaChart(List<ChartSeries> series)
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes(series);

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                DrawAreaSeries(seriesData, series);
            }
        }

        private void DrawPieChart(List<ChartSeries> series)
        {
            if (series.Count == 0)
                return;

            var center = new Vector2(_chartRect.x + _chartRect.width / 2, _chartRect.y + _chartRect.height / 2);
            var radius = Mathf.Min(_chartRect.width, _chartRect.height) / 2 * 0.8f;

            var totalValue = series.SelectMany(s => s.Data).Sum(d => d.Value);
            var currentAngle = 0f;

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                foreach (var dataPoint in seriesData.Data)
                {
                    var sliceAngle = (dataPoint.Value / totalValue) * 360f;
                    DrawPieSlice(center, radius, currentAngle, sliceAngle, dataPoint.Color);
                    currentAngle += sliceAngle;
                }
            }
        }

        private void DrawScatterChart(List<ChartSeries> series)
        {
            if (series.Count == 0)
                return;

            DrawGrid();
            DrawAxes(series);

            foreach (var seriesData in series.Where(s => s.Visible))
            {
                DrawScatterSeries(seriesData, series);
            }
        }

        private void DrawGrid()
        {
            var theme = _guiHelper.GetStyleManager().GetTheme();

            for (var i = 1; i < 5; i++)
            {
                var y = _chartRect.y + (_chartRect.height / 5) * i;
                DrawLine(new Vector2(_chartRect.x + 40, y), new Vector2(_chartRect.x + _chartRect.width - 20, y), new Color(theme.ChartGridColor.r, theme.ChartGridColor.g, theme.ChartGridColor.b, 0.3f));
            }
        }

        private void DrawAxes(List<ChartSeries> series)
        {
            try
            {
                var theme = _guiHelper.GetStyleManager().GetTheme();
                var styleManager = _guiHelper.GetStyleManager();

                DrawLine(new Vector2(_chartRect.x + 40, _chartRect.y + _chartRect.height - 20), new Vector2(_chartRect.x + _chartRect.width - 20, _chartRect.y + _chartRect.height - 20), new Color(theme.ChartAxisColor.r, theme.ChartAxisColor.g, theme.ChartAxisColor.b, 0.5f));

                if (series.Count > 0 && series[0].Data.Count > 0 && _chartRect.width > 60)
                {
                    var labelWidth = (_chartRect.width - 60) / series[0].Data.Count;
                    if (labelWidth > 0)
                    {
                        for (var i = 0; i < series[0].Data.Count; i++)
                        {
                            var x = _chartRect.x + 40 + (i * labelWidth) + (labelWidth / 2);
                            var label = series[0].Data[i].Name.Length > 3 ? series[0].Data[i].Name.Substring(0, 3) : series[0].Data[i].Name;

                            var labelRect = new Rect(x - 15, _chartRect.y + _chartRect.height - 15, 30, 20);
                            if (labelRect.width > 0 && labelRect.height > 0)
                            {
                                var originalColor = GUI.color;
                                GUI.color = theme.MutedColor;
                                GUI.Label(labelRect, label, styleManager.GetChartAxisStyle());
                                GUI.color = originalColor;
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

        private void DrawLineSeries(ChartSeries seriesData, List<ChartSeries> series)
        {
            if (seriesData.Data.Count < 2)
                return;

            var points = new Vector2[seriesData.Data.Count];
            var maxValue = GetMaxValue(series);
            var chartWidth = _chartRect.width - 60;
            var chartHeight = _chartRect.height - 40;

            for (var i = 0; i < seriesData.Data.Count; i++)
            {
                var x = _chartRect.x + 40 + (i / (float)(seriesData.Data.Count - 1)) * chartWidth;
                var y = _chartRect.y + chartHeight - (seriesData.Data[i].Value / maxValue) * chartHeight;
                points[i] = new Vector2(x, y);
            }

            for (var i = 0; i < points.Length - 1; i++)
            {
                DrawThickLine(points[i], points[i + 1], seriesData.Color, 2f);
            }

            foreach (var point in points)
            {
                DrawCircle(point, 4f, seriesData.Color);
                DrawCircle(point, 2f, Color.white);
            }
        }

        private void DrawAreaSeries(ChartSeries seriesData, List<ChartSeries> series)
        {
            if (seriesData.Data.Count < 2)
                return;

            var points = new Vector2[seriesData.Data.Count + 2];
            var maxValue = GetMaxValue(series);
            var chartWidth = _chartRect.width - 60;
            var chartHeight = _chartRect.height - 40;

            points[0] = new Vector2(_chartRect.x + 40, _chartRect.y + chartHeight);

            for (var i = 0; i < seriesData.Data.Count; i++)
            {
                var x = _chartRect.x + 40 + (i / (float)(seriesData.Data.Count - 1)) * chartWidth;
                var y = _chartRect.y + chartHeight - (seriesData.Data[i].Value / maxValue) * chartHeight;
                points[i + 1] = new Vector2(x, y);
            }

            points[points.Length - 1] = new Vector2(_chartRect.x + chartWidth + 40, _chartRect.y + chartHeight);

            var areaColor = new Color(seriesData.Color.r, seriesData.Color.g, seriesData.Color.b, 0.3f);
            for (var i = 0; i < points.Length - 1; i++)
            {
                DrawLine(points[i], points[i + 1], areaColor);
            }

            for (var i = 1; i < points.Length - 2; i++)
            {
                DrawLine(points[i], points[i + 1], seriesData.Color);
            }
        }

        private void DrawScatterSeries(ChartSeries seriesData, List<ChartSeries> series)
        {
            var maxValue = GetMaxValue(series);
            var chartWidth = _chartRect.width - 60;
            var chartHeight = _chartRect.height - 40;

            for (var i = 0; i < seriesData.Data.Count; i++)
            {
                var x = _chartRect.x + 40 + (i / (float)(seriesData.Data.Count - 1)) * chartWidth;
                var y = _chartRect.y + chartHeight - (seriesData.Data[i].Value / maxValue) * chartHeight;
                DrawCircle(new Vector2(x, y), 4f, seriesData.Color);
            }
        }

        private void DrawPieSlice(Vector2 center, float radius, float startAngle, float sliceAngle, Color color)
        {
            var segments = Mathf.Max(3, Mathf.RoundToInt(sliceAngle / 10f));
            var angleStep = sliceAngle / segments;

            var prevPoint = center + new Vector2(Mathf.Cos(startAngle * (float)System.Math.PI / 180f) * radius, Mathf.Sin(startAngle * (float)System.Math.PI / 180f) * radius);

            for (var i = 1; i <= segments; i++)
            {
                var angle = startAngle + angleStep * i;
                var point = center + new Vector2(Mathf.Cos(angle * (float)System.Math.PI / 180f) * radius, Mathf.Sin(angle * (float)System.Math.PI / 180f) * radius);

                DrawLine(center, prevPoint, color);
                DrawLine(prevPoint, point, color);
                prevPoint = point;
            }
            DrawLine(center, prevPoint, color);
        }

        private float GetMaxValue(IEnumerable<ChartSeries> series)
        {
            return series.SelectMany(s => s.Data).Max(d => d.Value);
        }

        private void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            var distance = Vector2.Distance(start, end);

            var originalColor = GUI.color;
            GUI.color = color;

            var segments = Mathf.Max(1, Mathf.RoundToInt(distance / 2f));
            for (var i = 0; i < segments; i++)
            {
                var pos = Vector2.Lerp(start, end, i / (float)segments);
                GUI.DrawTexture(new Rect(pos.x - 0.5f, pos.y - 0.5f, 1f, 1f), Texture2D.whiteTexture);
            }

            GUI.color = originalColor;
        }

        private void DrawCircle(Vector2 center, float radius, Color color)
        {
            var originalColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(center.x - radius, center.y - radius, radius * 2, radius * 2), Texture2D.whiteTexture);
            GUI.color = originalColor;
        }

        private void DrawThickLine(Vector2 start, Vector2 end, Color color, float thickness)
        {
            var distance = Vector2.Distance(start, end);

            var originalColor = GUI.color;
            GUI.color = color;

            var segments = Mathf.Max(1, Mathf.RoundToInt(distance / 1f));
            for (var i = 0; i < segments; i++)
            {
                var pos = Vector2.Lerp(start, end, i / (float)segments);
                GUI.DrawTexture(new Rect(pos.x - thickness / 2, pos.y - thickness / 2, thickness, thickness), Texture2D.whiteTexture);
            }

            GUI.color = originalColor;
        }

        private void DrawRoundedRect(Rect rect, Color color, float cornerRadius)
        {
            var originalColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = originalColor;
        }
    }
}
