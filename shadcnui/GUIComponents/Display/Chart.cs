using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Chart : BaseComponent
    {
        private const float PADDING_LEFT = 40f;
        private const float PADDING_RIGHT = 20f;
        private const float PADDING_TOP = 20f;
        private const float PADDING_BOTTOM = 20f;
        private const float GRID_LINE_COUNT = 4f;
        private const float LINE_THICKNESS = 2f;
        private const float POINT_RADIUS = 4f;
        private const float BAR_GAP_RATIO = 0.1f;
        private const float BAR_GROUP_FILL_RATIO = 0.8f;
        private const float PIE_RADIUS_RATIO = 0.8f;
        private const float PIE_SEGMENT_ANGLE_STEP = 10f;
        private static readonly float Deg2Rad = (float)Math.PI / 180f;

        private Rect _chartRect;
        private Rect _plotRect;
        private float _plotWidth;
        private float _plotHeight;

        public Chart(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public void DrawChart(ChartConfig config)
        {
            if (config?.Series == null)
                return;

            GUILayoutOption[] layoutOptions = BuildChartLayoutOptions(config);
            layoutComponents.BeginVerticalGroup(styleManager.GetChartStyle(ControlVariant.Default, ControlSize.Default), layoutOptions);
            DrawChartContent(config);
            layoutComponents.EndVerticalGroup();
        }
        #endregion

        #region API
        #endregion

        #region Layout & Bounds
        private GUILayoutOption[] BuildChartLayoutOptions(ChartConfig config)
        {
            if (config.Options != null && config.Options.Length > 0)
                return config.Options;

            return new[] { GUILayout.Width(config.Size.x), GUILayout.Height(config.Size.y), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) };
        }

        private void UpdatePlotBounds()
        {
            _plotWidth = _chartRect.width - PADDING_LEFT - PADDING_RIGHT;
            _plotHeight = _chartRect.height - PADDING_TOP - PADDING_BOTTOM;
            _plotRect = new Rect(_chartRect.x + PADDING_LEFT, _chartRect.y + PADDING_TOP, _plotWidth, _plotHeight);
        }

        private Vector2 DataToPlot(int dataIndex, int dataCount, float value, float maxValue)
        {
            if (dataCount <= 0 || maxValue <= 0f)
                return new Vector2(_plotRect.x, _plotRect.yMax);

            float t = dataCount > 1 ? dataIndex / (float)(dataCount - 1) : 0f;
            float x = _plotRect.x + t * _plotWidth;
            float y = _plotRect.yMax - (value / maxValue) * _plotHeight;
            return new Vector2(x, y);
        }
        #endregion

        #region Chart Rendering
        private void DrawChartContent(ChartConfig config)
        {
#if IL2CPP_MELONLOADER_PRE57
            var options = new UnhollowerBaseLib.Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
            Rect chartArea = GUILayoutUtility.GetRect(config.Size.x, config.Size.y, options);
#else
            Rect chartArea = GUILayoutUtility.GetRect(config.Size.x, config.Size.y, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
#endif

            if (Event.current.type != EventType.Repaint)
                return;

            _chartRect = chartArea;
            UpdatePlotBounds();

            var visible = config.Series.Where(s => s != null && s.Visible).ToList();
            if (visible.Count == 0)
                return;

            switch (config.ChartType)
            {
                case ChartType.Line:
                    RenderLineChart(visible);
                    break;
                case ChartType.Bar:
                    RenderBarChart(visible);
                    break;
                case ChartType.Area:
                    RenderAreaChart(visible);
                    break;
                case ChartType.Pie:
                    RenderPieChart(visible);
                    break;
                case ChartType.Scatter:
                    RenderScatterChart(visible);
                    break;
            }
        }

        private void RenderLineChart(List<ChartSeries> series)
        {
            DrawGrid();
            DrawAxes(series);
            float maxVal = GetMaxValue(series);
            foreach (var s in series)
            {
                if (s?.Data == null || s.Data.Count < 2)
                    continue;
                DrawLineSeries(s, s.Data.Count, maxVal);
            }
        }

        private void DrawLineSeries(ChartSeries series, int dataCount, float maxValue)
        {
            var points = new Vector2[series.Data.Count];
            for (int i = 0; i < series.Data.Count; i++)
                points[i] = DataToPlot(i, dataCount, series.Data[i].Value, maxValue);

            for (int i = 0; i < points.Length - 1; i++)
                DrawLine(points[i], points[i + 1], series.Color, LINE_THICKNESS);

            foreach (var p in points)
            {
                DrawCircle(p, POINT_RADIUS, series.Color);
                DrawCircle(p, POINT_RADIUS * 0.5f, Color.white);
            }
        }

        private void RenderBarChart(List<ChartSeries> series)
        {
            DrawGrid();
            DrawAxes(series);

            int dataCount = series.Max(s => s.Data?.Count ?? 0);
            if (dataCount == 0)
                return;

            float maxVal = GetMaxValue(series);
            float groupWidth = _plotWidth / dataCount;
            float barWidth = (groupWidth * BAR_GROUP_FILL_RATIO) / series.Count;
            float gap = groupWidth * BAR_GAP_RATIO;

            for (int i = 0; i < dataCount; i++)
            {
                float groupX = _plotRect.x + i * groupWidth + gap;
                for (int si = 0; si < series.Count; si++)
                {
                    var s = series[si];
                    if (s?.Data == null || i >= s.Data.Count)
                        continue;
                    float val = s.Data[i].Value;
                    float h = maxVal > 0 ? (val / maxVal) * _plotHeight : 0f;
                    float bx = groupX + si * barWidth;
                    float by = _plotRect.yMax - h;
                    DrawRect(new Rect(bx, by, barWidth - 2f, h), s.Data[i].Color != default ? s.Data[i].Color : s.Color);
                }
            }
        }

        private void RenderAreaChart(List<ChartSeries> series)
        {
            DrawGrid();
            DrawAxes(series);
            float maxVal = GetMaxValue(series);
            foreach (var s in series)
            {
                if (s?.Data == null || s.Data.Count < 2)
                    continue;
                DrawAreaSeries(s, s.Data.Count, maxVal);
            }
        }

        private void DrawAreaSeries(ChartSeries series, int dataCount, float maxValue)
        {
            int n = series.Data.Count;
            var pts = new Vector2[n + 2];
            pts[0] = new Vector2(_plotRect.x, _plotRect.yMax);
            for (int i = 0; i < n; i++)
                pts[i + 1] = DataToPlot(i, dataCount, series.Data[i].Value, maxValue);
            pts[n + 1] = new Vector2(_plotRect.xMax, _plotRect.yMax);

            Color fillColor = new Color(series.Color.r, series.Color.g, series.Color.b, 0.3f);
            for (int i = 0; i < pts.Length - 1; i++)
                DrawLine(pts[i], pts[i + 1], fillColor, 1f);
            for (int i = 1; i < pts.Length - 2; i++)
                DrawLine(pts[i], pts[i + 1], series.Color, LINE_THICKNESS);
        }

        private void RenderPieChart(List<ChartSeries> series)
        {
            float total = GetTotalValue(series);
            if (total <= 0f)
                return;

            Vector2 center = _chartRect.center;
            float radius = Mathf.Min(_chartRect.width, _chartRect.height) * 0.5f * PIE_RADIUS_RATIO;
            float currentAngle = 0f;

            foreach (var s in series)
            {
                if (s?.Data == null)
                    continue;
                foreach (var pt in s.Data)
                {
                    float sliceAngle = (pt.Value / total) * 360f;
                    if (sliceAngle > 0f)
                    {
                        DrawPieSlice(center, radius, currentAngle, sliceAngle, pt.Color != default ? pt.Color : s.Color);
                        currentAngle += sliceAngle;
                    }
                }
            }
        }

        private void DrawPieSlice(Vector2 center, float radius, float startAngleDeg, float sliceAngleDeg, Color color)
        {
            int segments = Mathf.Max(3, Mathf.RoundToInt(sliceAngleDeg / PIE_SEGMENT_ANGLE_STEP));
            float step = sliceAngleDeg / segments;
            float startRad = startAngleDeg * Deg2Rad;
            Vector2 prev = center + new Vector2(Mathf.Cos(startRad), Mathf.Sin(startRad)) * radius;

            for (int i = 1; i <= segments; i++)
            {
                float angleRad = (startAngleDeg + step * i) * Deg2Rad;
                Vector2 next = center + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
                DrawLine(center, prev, color, 1f);
                DrawLine(prev, next, color, 1f);
                prev = next;
            }
            DrawLine(center, prev, color, 1f);
        }

        private void RenderScatterChart(List<ChartSeries> series)
        {
            DrawGrid();
            DrawAxes(series);
            float maxVal = GetMaxValue(series);
            foreach (var s in series)
            {
                if (s?.Data == null)
                    continue;
                int dataCount = s.Data.Count;
                for (int i = 0; i < s.Data.Count; i++)
                {
                    Vector2 p = DataToPlot(i, dataCount > 1 ? dataCount : 1, s.Data[i].Value, maxVal);
                    DrawCircle(p, POINT_RADIUS, s.Color);
                }
            }
        }

        private void DrawGrid()
        {
            var theme = styleManager?.GetTheme();
            if (theme == null)
                return;
            Color gridColor = new Color(theme.Border.r, theme.Border.g, theme.Border.b, 0.3f);
            for (int i = 1; i <= GRID_LINE_COUNT; i++)
            {
                float y = _plotRect.y + (_plotHeight / (GRID_LINE_COUNT + 1)) * i;
                DrawLine(new Vector2(_plotRect.x, y), new Vector2(_plotRect.xMax, y), gridColor, 1f);
            }
        }

        private void DrawAxes(List<ChartSeries> series)
        {
            var theme = styleManager?.GetTheme();
            if (theme == null || styleManager == null)
                return;

            Color axisColor = new Color(theme.Muted.r, theme.Muted.g, theme.Muted.b, 0.5f);
            DrawLine(new Vector2(_plotRect.x, _plotRect.yMax), new Vector2(_plotRect.xMax, _plotRect.yMax), axisColor, 1f);

            if (series.Count == 0 || series[0].Data == null || series[0].Data.Count == 0 || _plotWidth <= 0)
                return;

            int labelCount = series[0].Data.Count;
            float labelWidth = _plotWidth / labelCount;
            GUIStyle axisStyle = styleManager?.GetChartAxisStyle() ?? GUI.skin.label;

            for (int i = 0; i < labelCount; i++)
            {
                float x = _plotRect.x + (i + 0.5f) * labelWidth;
                string label = series[0].Data[i].Name;
                if (label != null && label.Length > 3)
                    label = label.Substring(0, 3);
                Rect labelRect = new Rect(x - 15f, _plotRect.yMax + 2f, 30f, 18f);
                if (labelRect.width > 0 && labelRect.height > 0)
                {
                    Color prev = GUI.color;
                    GUI.color = theme.Muted;
                    GUI.Label(labelRect, label ?? "", axisStyle);
                    GUI.color = prev;
                }
            }
        }

        private float GetMaxValue(IEnumerable<ChartSeries> series)
        {
            float max = series.Where(s => s?.Data != null).SelectMany(s => s.Data).Select(d => d.Value).DefaultIfEmpty(0f).Max();
            return max > 0f ? max : 1f;
        }

        private float GetTotalValue(IEnumerable<ChartSeries> series)
        {
            return series.Where(s => s?.Data != null).SelectMany(s => s.Data).Sum(d => d.Value);
        }
        #endregion

        #region Primitives
        private void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            Color prev = GUI.color;
            GUI.color = color;
            Vector2 d = end - start;
            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            float len = d.magnitude;
            Matrix4x4 m = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y - width * 0.5f, len, width), Texture2D.whiteTexture);
            GUI.matrix = m;
            GUI.color = prev;
        }

        private void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            Color prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f), Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private void DrawRect(Rect rect, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            Color prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = prev;
        }
        #endregion
    }
}
