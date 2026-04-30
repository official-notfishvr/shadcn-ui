using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Display
{
    public class Chart : BaseComponent
    {
        public Chart(GUIHelper helper)
            : base(helper) { }

        public void DrawChart(ChartConfig config)
        {
            if (config?.Series == null || config.Series.Count == 0)
                return;

            float width = config.Size.x * guiHelper.uiScale;
            float height = config.Size.y * guiHelper.uiScale;
            var style = styleManager.GetChartStyle(config.Variant, ControlSize.Default, config.Appearance);

            layoutComponents.BeginVerticalGroup(style, GUILayout.Width(width), GUILayout.Height(height));
            Rect rect = ControlLayoutUtility.ReserveRect(UnityHelpers.GUIContent.none, GUIStyle.none, new[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) }, height - style.padding.vertical - 24f * guiHelper.uiScale);
            DrawPlot(rect, config);
            DrawLegend(config);
            layoutComponents.EndVerticalGroup();
        }

        private void DrawPlot(Rect rect, ChartConfig config)
        {
            var visibleSeries = config.Series.Where(s => s != null && s.Visible && s.Data != null && s.Data.Count > 0).ToArray();
            if (visibleSeries.Length == 0)
                return;

            float maxValue = Mathf.Max(1f, visibleSeries.SelectMany(s => s.Data).Max(d => d.Value));
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), Texture2D.whiteTexture);

            switch (config.ChartType)
            {
                case ChartType.Bar:
                    DrawBars(rect, visibleSeries, maxValue);
                    break;
                default:
                    DrawLines(rect, visibleSeries, maxValue);
                    break;
            }
        }

        private void DrawBars(Rect rect, ChartSeries[] series, float maxValue)
        {
            int count = Mathf.Max(1, series.Max(s => s.Data.Count));
            float groupWidth = rect.width / count;
            float barWidth = groupWidth / Mathf.Max(1, series.Length + 0.5f);

            for (int i = 0; i < count; i++)
            {
                for (int s = 0; s < series.Length; s++)
                {
                    if (i >= series[s].Data.Count)
                        continue;
                    float normalized = series[s].Data[i].Value / maxValue;
                    float h = rect.height * normalized;
                    Rect barRect = new Rect(rect.x + i * groupWidth + s * barWidth, rect.yMax - h, barWidth - 2f, h);
                    Color color = ResolveSeriesColor(series[s], i);
                    SurfaceDrawUtility.DrawRoundedFill(styleManager, barRect, color, styleManager.GetScaledBorderRadius(DesignTokens.Radius.SM));
                }
            }
        }

        private void DrawLines(Rect rect, ChartSeries[] series, float maxValue)
        {
            foreach (var seriesItem in series)
            {
                for (int i = 1; i < seriesItem.Data.Count; i++)
                {
                    Vector2 p0 = GetPoint(rect, seriesItem.Data.Count, i - 1, seriesItem.Data[i - 1].Value, maxValue);
                    Vector2 p1 = GetPoint(rect, seriesItem.Data.Count, i, seriesItem.Data[i].Value, maxValue);
                    DrawLine(p0, p1, ResolveSeriesColor(seriesItem, i), 2f * guiHelper.uiScale);
                }
            }
        }

        private Vector2 GetPoint(Rect rect, int count, int index, float value, float maxValue)
        {
            float x = rect.x + (count <= 1 ? rect.width * 0.5f : rect.width * index / (count - 1f));
            float y = rect.yMax - rect.height * Mathf.Clamp01(value / maxValue);
            return new Vector2(x, y);
        }

        private void DrawLine(Vector2 a, Vector2 b, Color color, float thickness)
        {
            Matrix4x4 matrix = GUI.matrix;
            Color prev = GUI.color;
            GUI.color = color;
            float angle = Vector3.Angle(b - a, Vector2.right) * (a.y > b.y ? -1f : 1f);
            float length = (b - a).magnitude;
            GUIUtility.RotateAroundPivot(angle, a);
            GUI.DrawTexture(new Rect(a.x, a.y - thickness * 0.5f, length, thickness), Texture2D.whiteTexture);
            GUI.matrix = matrix;
            GUI.color = prev;
        }

        private void DrawLegend(ChartConfig config)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();
            foreach (var series in config.Series.Where(s => s != null && s.Visible))
            {
                Rect dot = SurfaceDrawUtility.ReserveSquare(8f * guiHelper.uiScale);
                SurfaceDrawUtility.DrawRoundedFill(styleManager, dot, ResolveSeriesColor(series, 0), Mathf.RoundToInt(dot.width / 2f));
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                UnityHelpers.Label(series.Label ?? series.Key ?? "Series", styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small, config.Appearance));
                layoutComponents.AddSpace(DesignTokens.Spacing.MD);
            }
            layoutComponents.EndHorizontalGroup();
        }

        private Color ResolveSeriesColor(ChartSeries series, int index)
        {
            if (index < series.Data.Count && series.Data[index].Color.a > 0f)
                return series.Data[index].Color;
            if (series.Color.a > 0f)
                return series.Color;
            return styleManager.GetTheme().ButtonPrimaryBg;
        }
    }
}
