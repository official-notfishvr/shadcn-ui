using System;
using System.Linq;
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

        public void Render(TableConfig config)
        {
            if (config == null)
                return;

            string[,] rows = config.FilteredRows ?? config.Rows;
            DrawTableCore(config.ColumnHeaders, rows, config.ColumnWidths, config.Variant, config.Size, config.Appearance);
        }

        internal void DrawTable(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, null, variant, size, null, options);
        }

        internal void DrawTable(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            GUILayout.BeginArea(ControlLayoutUtility.ScaleRect(rect, guiHelper.uiScale));
            DrawTableCore(headers, data, null, variant, size, null);
            GUILayout.EndArea();
        }

        internal void SortableTable(TableConfig config) => Render(config);

        internal void SortableTable(string[] headers, string[,] data, ref int[] sortCols, ref bool[] sortAsc, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            sortCols ??= Array.Empty<int>();
            sortAsc ??= Array.Empty<bool>();
            int activeSortColumn = sortCols.Length > 0 ? sortCols[0] : -1;
            bool ascending = sortAsc.Length > 0 ? sortAsc[0] : true;
            string[,] sorted = ApplySorting(data, activeSortColumn, ascending);
            int[] nextSortCols = (int[])sortCols.Clone();
            bool[] nextSortAsc = (bool[])sortAsc.Clone();

            DrawTableCore(
                headers,
                sorted,
                null,
                variant,
                size,
                null,
                options,
                sortable: true,
                activeSortColumn: activeSortColumn,
                activeSortAscending: ascending,
                onSort: (column, nextAscending) =>
                {
                    nextSortCols = new[] { column };
                    nextSortAsc = new[] { nextAscending };
                    onSort?.Invoke(column, nextAscending);
                }
            );

            sortCols = nextSortCols;
            sortAsc = nextSortAsc;
        }

        internal void SelectableTable(TableConfig config) => Render(config);

        internal void SelectableTable(string[] headers, string[,] data, ref bool[] selected, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChanged = null, params GUILayoutOption[] options)
        {
            int rowCount = data?.GetLength(0) ?? 0;
            EnsureSelectionArray(ref selected, rowCount);
            bool[] workingSelection = (bool[])selected.Clone();
            DrawTableCore(
                headers,
                data,
                null,
                variant,
                size,
                null,
                options,
                selectable: true,
                selectedRows: workingSelection,
                onSelectionChanged: (row, value) =>
                {
                    workingSelection[row] = value;
                    onSelectionChanged?.Invoke(row, value);
                }
            );
            selected = workingSelection;
        }

        internal void PaginatedTable(TableConfig config) => Render(config);

        internal void PaginatedTable(string[] headers, string[,] data, ref int page, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            int totalRows = data?.GetLength(0) ?? 0;
            int resolvedPageSize = Mathf.Max(1, pageSize);
            int totalPages = Mathf.Max(1, Mathf.CeilToInt(totalRows / (float)resolvedPageSize));
            page = Mathf.Clamp(page, 0, totalPages - 1);
            int nextPageValue = page;
            DrawTableCore(
                headers,
                SlicePage(data, page, resolvedPageSize),
                null,
                variant,
                size,
                null,
                options,
                currentPage: page,
                totalPages: totalPages,
                onPageChanged: nextPage =>
                {
                    nextPageValue = nextPage;
                    onPageChange?.Invoke(nextPage);
                }
            );
            page = nextPageValue;
        }

        internal void SearchableTable(TableConfig config) => Render(config);

        internal void SearchableTable(string[] headers, string[,] data, ref string query, ref string[,] filtered, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            filtered = data;
            DrawTableCore(headers, filtered, null, variant, size, null, options);
        }

        internal void ResizableTable(TableConfig config) => Render(config);

        internal void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawTableCore(headers, data, columnWidths, variant, size, null, options);
        }

        internal void CustomTable(TableConfig config)
        {
            Render(config);
        }

        internal void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
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

        private void DrawTableCore(
            string[] headers,
            string[,] rows,
            float[] columnWidths,
            ControlVariant variant,
            ControlSize size,
            ComponentAppearance appearance,
            GUILayoutOption[] options = null,
            Action<object, int, int> customRenderer = null,
            object[,] objectRows = null,
            bool sortable = false,
            int activeSortColumn = -1,
            bool activeSortAscending = true,
            Action<int, bool> onSort = null,
            bool selectable = false,
            bool[] selectedRows = null,
            Action<int, bool> onSelectionChanged = null,
            int currentPage = -1,
            int totalPages = 0,
            Action<int> onPageChanged = null
        )
        {
            headers ??= Array.Empty<string>();
            rows ??= new string[0, headers.Length];

            var tableStyle = styleManager.GetTableStyle(variant, size, appearance);
            var headerStyle = styleManager.GetTableHeaderStyle(variant, size, appearance);
            var rowStyle = styleManager.GetTableRowStyle(variant, size, appearance);

            layoutComponents.BeginVerticalGroup(tableStyle, options ?? Array.Empty<GUILayoutOption>());
            DrawHeader(headers, columnWidths, headerStyle, size, sortable, activeSortColumn, activeSortAscending, onSort, selectable, rows, selectedRows, onSelectionChanged);

            int rowCount = rows.GetLength(0);
            int colCount = headers.Length > 0 ? headers.Length : rows.GetLength(1);
            for (int r = 0; r < rowCount; r++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);
                if (selectable)
                {
                    bool isSelected = selectedRows != null && r < selectedRows.Length && selectedRows[r];
                    bool nextSelected = guiHelper.DrawToggle(string.Empty, isSelected, ControlVariant.Outline, size, value => onSelectionChanged?.Invoke(r, value), false, null, GUILayout.Width(28f * guiHelper.uiScale), GUILayout.Height(GetRowToggleHeight(size)));
                    if (selectedRows != null && r < selectedRows.Length && nextSelected != isSelected)
                        selectedRows[r] = nextSelected;
                }

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

            if (currentPage >= 0 && totalPages > 0 && onPageChanged != null)
                DrawPaginationFooter(currentPage, totalPages, onPageChanged);

            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeader(string[] headers, float[] columnWidths, GUIStyle headerStyle, ControlSize size, bool sortable, int activeSortColumn, bool activeSortAscending, Action<int, bool> onSort, bool selectable, string[,] rows, bool[] selectedRows, Action<int, bool> onSelectionChanged)
        {
            layoutComponents.BeginHorizontalGroup();
            if (selectable)
            {
                bool allSelected = AreAllRowsSelected(rows, selectedRows);
                bool nextAll = guiHelper.DrawToggle(
                    string.Empty,
                    allSelected,
                    ControlVariant.Outline,
                    size,
                    value =>
                    {
                        if (selectedRows == null)
                            return;

                        for (int i = 0; i < selectedRows.Length; i++)
                        {
                            if (selectedRows[i] == value)
                                continue;

                            selectedRows[i] = value;
                            onSelectionChanged?.Invoke(i, value);
                        }
                    },
                    false,
                    null,
                    GUILayout.Width(28f * guiHelper.uiScale),
                    GUILayout.Height(GetRowToggleHeight(size))
                );

                if (selectedRows != null && nextAll != allSelected)
                {
                    for (int i = 0; i < selectedRows.Length; i++)
                        selectedRows[i] = nextAll;
                }
            }

            for (int i = 0; i < headers.Length; i++)
            {
                float width = columnWidths != null && i < columnWidths.Length && columnWidths[i] > 0f ? columnWidths[i] * guiHelper.uiScale : 0f;
                if (sortable)
                {
                    bool isActive = i == activeSortColumn;
                    bool nextAscending = isActive ? !activeSortAscending : true;
                    string label = headers[i] ?? string.Empty;
                    if (isActive)
                        label = $"{label} {(activeSortAscending ? "↑" : "↓")}";

                    if (width > 0f)
                        guiHelper.DrawButton(label, () => onSort?.Invoke(i, nextAscending), ControlVariant.Ghost, ControlSize.Small, false, 1f, null, GUILayout.Width(width), GUILayout.Height(GetHeaderButtonHeight(size)));
                    else
                        guiHelper.DrawButton(label, () => onSort?.Invoke(i, nextAscending), ControlVariant.Ghost, ControlSize.Small, false, 1f, null, GUILayout.ExpandWidth(true), GUILayout.Height(GetHeaderButtonHeight(size)));
                }
                else if (width > 0f)
                {
                    GUILayout.Label(headers[i] ?? string.Empty, headerStyle, GUILayout.Width(width));
                }
                else
                    GUILayout.Label(headers[i] ?? string.Empty, headerStyle, GUILayout.ExpandWidth(true));
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawPaginationFooter(int currentPage, int totalPages, Action<int> onPageChanged)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();
            guiHelper.DrawButton("Prev", () => onPageChanged?.Invoke(Mathf.Max(0, currentPage - 1)), ControlVariant.Outline, ControlSize.Small, currentPage <= 0);
            GUILayout.FlexibleSpace();
            GUILayout.Label($"Page {currentPage + 1} of {totalPages}", styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small));
            GUILayout.FlexibleSpace();
            guiHelper.DrawButton("Next", () => onPageChanged?.Invoke(Mathf.Min(totalPages - 1, currentPage + 1)), ControlVariant.Outline, ControlSize.Small, currentPage >= totalPages - 1);
            layoutComponents.EndHorizontalGroup();
        }

        private float GetHeaderButtonHeight(ControlSize size)
        {
            return (
                    size == ControlSize.Small ? 28f
                    : size == ControlSize.Large ? 38f
                    : 32f
                ) * guiHelper.uiScale;
        }

        private float GetRowToggleHeight(ControlSize size)
        {
            return (
                    size == ControlSize.Small ? 24f
                    : size == ControlSize.Large ? 34f
                    : 28f
                ) * guiHelper.uiScale;
        }

        private static void EnsureSelectionArray(ref bool[] selected, int rowCount)
        {
            if (selected == null || selected.Length != rowCount)
                selected = new bool[rowCount];
        }

        private static bool AreAllRowsSelected(string[,] rows, bool[] selectedRows)
        {
            int rowCount = rows?.GetLength(0) ?? 0;
            if (rowCount == 0 || selectedRows == null || selectedRows.Length < rowCount)
                return false;

            for (int i = 0; i < rowCount; i++)
            {
                if (!selectedRows[i])
                    return false;
            }

            return true;
        }

        private static string[,] SlicePage(string[,] data, int page, int pageSize)
        {
            if (data == null)
                return new string[0, 0];

            int totalRows = data.GetLength(0);
            int cols = data.GetLength(1);
            int start = Mathf.Clamp(page * pageSize, 0, totalRows);
            int length = Mathf.Clamp(pageSize, 0, Mathf.Max(0, totalRows - start));
            var sliced = new string[length, cols];

            for (int r = 0; r < length; r++)
            {
                for (int c = 0; c < cols; c++)
                    sliced[r, c] = data[start + r, c];
            }

            return sliced;
        }

        private static string[,] ApplySorting(string[,] data, int sortColumn, bool ascending)
        {
            if (data == null || sortColumn < 0 || data.GetLength(0) == 0 || sortColumn >= data.GetLength(1))
                return data;

            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            IOrderedEnumerable<int> ordered;
            if (ascending)
                ordered = Enumerable.Range(0, rows).OrderBy(i => data[i, sortColumn] ?? string.Empty, StringComparer.OrdinalIgnoreCase);
            else
                ordered = Enumerable.Range(0, rows).OrderByDescending(i => data[i, sortColumn] ?? string.Empty, StringComparer.OrdinalIgnoreCase);

            var indices = ordered.ToArray();
            var sorted = new string[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                    sorted[r, c] = data[indices[r], c];
            }

            return sorted;
        }
    }
}
