using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Table : BaseComponent
    {
        public Table(GUIHelper helper)
            : base(helper) { }

        public void DrawTable(TableConfig config)
        {
            if (config == null)
                return;

            string[,] rows = config.FilteredRows ?? config.Rows;
            DrawTableCore(config.ColumnHeaders, rows, config.ColumnWidths, config.Variant, config.Size, config.Appearance);
        }

        public void DrawTable(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, null, variant, size, null, options);
        }

        public void DrawTable(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            GUILayout.BeginArea(new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale));
            DrawTableCore(headers, data, null, variant, size, null);
            GUILayout.EndArea();
        }

        public void SortableTable(TableConfig config) => DrawTable(config);

        public void SortableTable(string[] headers, string[,] data, ref int[] sortCols, ref bool[] sortAsc, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, null, variant, size, null, options);
        }

        public void SelectableTable(TableConfig config) => DrawTable(config);

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selected, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChanged = null, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, null, variant, size, null, options);
        }

        public void PaginatedTable(TableConfig config) => DrawTable(config);

        public void PaginatedTable(string[] headers, string[,] data, ref int page, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, null, variant, size, null, options);
        }

        public void SearchableTable(TableConfig config) => DrawTable(config);

        public void SearchableTable(string[] headers, string[,] data, ref string query, ref string[,] filtered, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            filtered = data;
            DrawTableCore(headers, filtered, null, variant, size, null, options);
        }

        public void ResizableTable(TableConfig config) => DrawTable(config);

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, columnWidths, variant, size, null, options);
        }

        public void CustomTable(TableConfig config)
        {
            DrawTable(config);
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            int rows = data?.GetLength(0) ?? 0;
            int cols = data?.GetLength(1) ?? 0;
            string[,] converted = new string[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (cellRenderer != null)
                        converted[r, c] = string.Empty;
                    else
                        converted[r, c] = data[r, c]?.ToString() ?? string.Empty;
                }
            }

            DrawTableCore(headers, converted, null, variant, size, null, options, cellRenderer, data);
        }

        private void DrawTableCore(string[] headers, string[,] rows, float[] columnWidths, ControlVariant variant, ControlSize size, ComponentAppearance appearance, GUILayoutOption[] options = null, Action<object, int, int> customRenderer = null, object[,] objectRows = null)
        {
            headers ??= Array.Empty<string>();
            rows ??= new string[0, headers.Length];

            var tableStyle = styleManager.GetTableStyle(variant, size, appearance);
            var headerStyle = styleManager.GetTableHeaderStyle(variant, size, appearance);
            var rowStyle = styleManager.GetTableRowStyle(variant, size, appearance);

            layoutComponents.BeginVerticalGroup(tableStyle, options ?? Array.Empty<GUILayoutOption>());
            DrawHeader(headers, columnWidths, headerStyle);

            int rowCount = rows.GetLength(0);
            int colCount = headers.Length > 0 ? headers.Length : rows.GetLength(1);
            for (int r = 0; r < rowCount; r++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);
                for (int c = 0; c < colCount; c++)
                {
                    float width = columnWidths != null && c < columnWidths.Length && columnWidths[c] > 0f ? columnWidths[c] * guiHelper.uiScale : 0f;
                    var cellStyle = styleManager.GetTableCellStyle(ControlVariant.Default, size);
                    if (customRenderer != null && objectRows != null)
                    {
                        GUILayout.BeginVertical(width > 0f ? GUILayout.Width(width) : GUILayout.ExpandWidth(true));
                        customRenderer(objectRows[r, c], r, c);
                        GUILayout.EndVertical();
                    }
                    else
                    {
                        if (width > 0f)
                            GUILayout.Label(rows[r, c] ?? string.Empty, cellStyle, GUILayout.Width(width));
                        else
                            GUILayout.Label(rows[r, c] ?? string.Empty, cellStyle, GUILayout.ExpandWidth(true));
                    }
                }
                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeader(string[] headers, float[] columnWidths, GUIStyle headerStyle)
        {
            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                float width = columnWidths != null && i < columnWidths.Length && columnWidths[i] > 0f ? columnWidths[i] * guiHelper.uiScale : 0f;
                if (width > 0f)
                    GUILayout.Label(headers[i] ?? string.Empty, headerStyle, GUILayout.Width(width));
                else
                    GUILayout.Label(headers[i] ?? string.Empty, headerStyle, GUILayout.ExpandWidth(true));
            }
            layoutComponents.EndHorizontalGroup();
        }
    }
}
