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
        private static readonly Texture2D WhiteTex = Texture2D.whiteTexture;

        public Chart(GUIHelper helper)
            : base(helper) { }

        public void DrawChart(ChartConfig config)
        {
            if (config == null)
                return;

            Vector2 size = config.Size;
            if (size.x <= 0 || size.y <= 0)
                size = new Vector2(320f, 220f);

            var controlSize = ((ComponentConfigBase)config).Size;
            GUIStyle style = styleManager?.GetChartStyle(config.Variant, controlSize, config.Appearance) ?? GUI.skin.box;

            Rect rect = GUILayoutUtility.GetRect(size.x * guiHelper.uiScale, size.y * guiHelper.uiScale, config.LayoutOptions);
            GUI.Box(rect, GUIContent.none, style);

            if (config.Series == null || config.Series.Count == 0)
            {
                DrawEmpty(rect, config.Appearance);
                return;
            }

            switch (config.ChartType)
            {
                case ChartType.Bar:
                    DrawBarChart(rect, config.Series);
                    break;
                case ChartType.Line:
                case ChartType.Area:
                case ChartType.Scatter:
                    DrawLineChart(rect, config.Series);
                    break;
                case ChartType.Pie:
                    DrawPieLegend(rect, config.Series, config.Appearance);
                    break;
            }
        }

        private void DrawBarChart(Rect rect, List<ChartSeries> series)
        {
            var data = series.SelectMany(s => s.Data).ToList();
            if (data.Count == 0)
            {
                DrawEmpty(rect, null);
                return;
            }

            float max = Mathf.Max(1f, data.Max(d => d.Value));
            int groupCount = series.Max(s => s.Data.Count);

            float padding = 12f * guiHelper.uiScale;
            Rect plot = new Rect(rect.x + padding, rect.y + padding, rect.width - padding * 2, rect.height - padding * 2);

            float groupWidth = plot.width / Mathf.Max(1, groupCount);
            float barWidth = groupWidth / Mathf.Max(1, series.Count) * 0.7f;

            for (int i = 0; i < groupCount; i++)
            {
                for (int s = 0; s < series.Count; s++)
                {
                    var ser = series[s];
                    if (i >= ser.Data.Count)
                        continue;

                    float value = ser.Data[i].Value;
                    float height = (value / max) * plot.height;
                    float x = plot.x + i * groupWidth + s * barWidth + (groupWidth - barWidth * series.Count) * 0.5f;
                    float y = plot.yMax - height;

                    DrawRect(new Rect(x, y, barWidth, height), ser.Data[i].Color == default ? ser.Color : ser.Data[i].Color);
                }
            }
        }

        private void DrawLineChart(Rect rect, List<ChartSeries> series)
        {
            var data = series.SelectMany(s => s.Data).ToList();
            if (data.Count == 0)
            {
                DrawEmpty(rect, null);
                return;
            }

            float max = Mathf.Max(1f, data.Max(d => d.Value));
            int maxPoints = series.Max(s => s.Data.Count);

            float padding = 12f * guiHelper.uiScale;
            Rect plot = new Rect(rect.x + padding, rect.y + padding, rect.width - padding * 2, rect.height - padding * 2);

            foreach (var ser in series)
            {
                if (ser.Data.Count < 2)
                    continue;

                for (int i = 0; i < ser.Data.Count - 1; i++)
                {
                    float x0 = plot.x + (i / (float)Mathf.Max(1, maxPoints - 1)) * plot.width;
                    float x1 = plot.x + ((i + 1) / (float)Mathf.Max(1, maxPoints - 1)) * plot.width;
                    float y0 = plot.yMax - (ser.Data[i].Value / max) * plot.height;
                    float y1 = plot.yMax - (ser.Data[i + 1].Value / max) * plot.height;

                    DrawLine(new Vector2(x0, y0), new Vector2(x1, y1), ser.Data[i].Color == default ? ser.Color : ser.Data[i].Color, 2f * guiHelper.uiScale);
                }
            }
        }

        private void DrawPieLegend(Rect rect, List<ChartSeries> series, ComponentAppearance appearance)
        {
            float padding = 12f * guiHelper.uiScale;
            Rect legend = new Rect(rect.x + padding, rect.y + padding, rect.width - padding * 2, rect.height - padding * 2);

            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default, appearance) ?? GUI.skin.label;
            float y = legend.y;
            foreach (var ser in series)
            {
                var color = ser.Color == default ? new Color(0.2f, 0.6f, 1f) : ser.Color;
                DrawRect(new Rect(legend.x, y + 4f, 10f, 10f), color);
                GUI.Label(new Rect(legend.x + 16f, y, legend.width - 16f, 20f), ser.Label ?? ser.Key ?? "Series", labelStyle);
                y += 20f;
            }
        }

        private void DrawEmpty(Rect rect, ComponentAppearance appearance)
        {
            var style = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Default, appearance) ?? GUI.skin.label;
            var centered = new UnityHelpers.GUIStyle(style) { alignment = TextAnchor.MiddleCenter };
            GUI.Label(rect, "No data", centered);
        }

        private void DrawRect(Rect rect, Color color)
        {
            Color prev = GUI.color;
            GUI.color = color == default ? new Color(0.2f, 0.6f, 1f) : color;
            GUI.DrawTexture(rect, WhiteTex);
            GUI.color = prev;
        }

        private void DrawLine(Vector2 a, Vector2 b, Color color, float width)
        {
            Color prev = GUI.color;
            Matrix4x4 prevMatrix = GUI.matrix;

            float angle = Vector3.Angle(b - a, Vector2.right);
            if (a.y > b.y)
                angle = -angle;

            GUI.color = color == default ? new Color(0.2f, 0.6f, 1f) : color;
            GUIUtility.RotateAroundPivot(angle, a);
            GUI.DrawTexture(new Rect(a.x, a.y - width / 2f, (b - a).magnitude, width), WhiteTex);

            GUI.matrix = prevMatrix;
            GUI.color = prev;
        }
    }
}
