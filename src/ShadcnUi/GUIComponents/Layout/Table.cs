using System;
using System.Collections.Generic;
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

            DrawSurface(
                config.ColumnHeaders,
                config.FilteredRows ?? config.Rows,
                config.ObjectRows,
                config.ColumnWidths,
                config.Variant,
                config.Size,
                config.Appearance,
                config.LayoutOptions,
                config.CellRenderer,
                config.OnSortChanged,
                config.OnSelectionChanged,
                config.SelectedRowFlags,
                config.SortColumnIndices,
                config.SortAscending,
                config.CurrentPage,
                config.PageSize,
                config.OnPageChanged
            );
        }

        internal void DrawTable(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => DrawSurface(headers, data, null, null, variant, size, null, options);

        internal void DrawTable(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            GUILayout.BeginArea(ControlLayoutUtility.ScaleRect(rect, guiHelper.uiScale));
            DrawSurface(headers, data, null, null, variant, size, null, null);
            GUILayout.EndArea();
        }

        internal void SortableTable(TableConfig config) => Render(config);

        internal void SortableTable(string[] headers, string[,] data, ref int[] sortCols, ref bool[] sortAsc, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            sortCols ??= Array.Empty<int>();
            sortAsc ??= Array.Empty<bool>();
            int column = sortCols.Length == 0 ? -1 : sortCols[0];
            bool ascending = sortAsc.Length == 0 || sortAsc[0];
            string[,] sorted = ApplySorting(data, column, ascending);
            DrawSurface(headers, sorted, null, null, variant, size, null, options, null, onSort, null, null, sortCols, sortAsc);
        }

        internal void SelectableTable(TableConfig config) => Render(config);

        internal void SelectableTable(string[] headers, string[,] data, ref bool[] selected, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChanged = null, params GUILayoutOption[] options)
        {
            int count = data?.GetLength(0) ?? 0;
            selected = selected == null || selected.Length != count ? new bool[count] : selected;
            DrawSurface(headers, data, null, null, variant, size, null, options, null, null, onSelectionChanged, selected);
        }

        internal void PaginatedTable(TableConfig config) => Render(config);

        internal void PaginatedTable(string[] headers, string[,] data, ref int page, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            int totalPages = Mathf.Max(1, Mathf.CeilToInt((data?.GetLength(0) ?? 0) / (float)Mathf.Max(1, pageSize)));
            page = Mathf.Clamp(page, 0, totalPages - 1);
            int nextPage = page;
            DrawSurface(
                headers,
                SlicePage(data, page, pageSize),
                null,
                null,
                variant,
                size,
                null,
                options,
                null,
                null,
                null,
                null,
                null,
                null,
                page,
                pageSize,
                next =>
                {
                    nextPage = next;
                    onPageChange?.Invoke(next);
                }
            );
            page = nextPage;
        }

        internal void SearchableTable(TableConfig config) => Render(config);

        internal void SearchableTable(string[] headers, string[,] data, ref string query, ref string[,] filtered, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            filtered ??= data;
            DrawSurface(headers, filtered, null, null, variant, size, null, options);
        }

        internal void ResizableTable(TableConfig config) => Render(config);

        internal void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options) => DrawSurface(headers, data, null, columnWidths, variant, size, null, options);

        internal void CustomTable(TableConfig config) => Render(config);

        internal void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            int rows = data?.GetLength(0) ?? 0;
            int columns = data?.GetLength(1) ?? headers?.Length ?? 0;
            var text = new string[rows, columns];
            DrawSurface(headers, text, data, null, variant, size, null, options, cellRenderer);
        }

        private void DrawSurface(
            string[] headers,
            string[,] rows,
            object[,] objectRows,
            float[] widths,
            ControlVariant variant,
            ControlSize size,
            ComponentAppearance appearance,
            GUILayoutOption[] options,
            Action<object, int, int> cellRenderer = null,
            Action<int, bool> onSort = null,
            Action<int, bool> onSelection = null,
            bool[] selectedRows = null,
            int[] sortColumns = null,
            bool[] sortAscending = null,
            int currentPage = -1,
            int pageSize = 0,
            Action<int> onPageChanged = null
        )
        {
            headers ??= Array.Empty<string>();
            rows ??= new string[0, headers.Length];
            bool selectable = selectedRows != null;
            bool sortable = onSort != null;
            int activeSort = sortColumns?.Length > 0 ? sortColumns[0] : -1;
            bool ascending = sortAscending?.Length == 0 || sortAscending == null || sortAscending[0];
            var tableStyle = styleManager.GetTableStyle(variant, size, appearance);

            layoutComponents.BeginVerticalGroup(tableStyle, Combine(options, GUILayout.ExpandHeight(false)));
            DrawHeader(headers, widths, size, appearance, selectable, sortable, activeSort, ascending, selectedRows, rows, onSort, onSelection);

            int rowCount = rows.GetLength(0);
            int columnCount = headers.Length > 0 ? headers.Length : rows.GetLength(1);
            if (rowCount == 0)
            {
                layoutComponents.BeginHorizontalGroup(styleManager.GetTableRowStyle(variant, size, appearance));
                guiHelper.MutedLabel("No rows", GUILayout.ExpandWidth(true));
                layoutComponents.EndHorizontalGroup();
            }
            else
            {
                for (int r = 0; r < rowCount; r++)
                {
                    layoutComponents.BeginHorizontalGroup(styleManager.GetTableRowStyle(variant, size, appearance));
                    if (selectable)
                        DrawSelectionCell(selectedRows, r, size, appearance, onSelection);

                    for (int c = 0; c < columnCount; c++)
                    {
                        float width = GetWidth(widths, c);
                        if (cellRenderer != null && objectRows != null)
                        {
                            GUILayout.BeginVertical(width > 0 ? GUILayout.Width(width) : GUILayout.ExpandWidth(true));
                            cellRenderer(objectRows[r, c], r, c);
                            GUILayout.EndVertical();
                        }
                        else
                        {
                            var label = guiHelper.Label(c < rows.GetLength(1) ? rows[r, c] ?? string.Empty : string.Empty).Size(size).Appearance(appearance).Options(width > 0 ? GUILayout.Width(width) : GUILayout.ExpandWidth(true));
                            label.Render();
                        }
                    }
                    layoutComponents.EndHorizontalGroup();
                    if (r < rowCount - 1)
                        guiHelper.HorizontalSeparator();
                }
            }

            layoutComponents.EndVerticalGroup();
            if (currentPage >= 0 && pageSize > 0 && onPageChanged != null)
                DrawPagination(currentPage, Mathf.Max(1, Mathf.CeilToInt(rowCount / (float)pageSize)), size, appearance, onPageChanged);
        }

        private void DrawHeader(string[] headers, float[] widths, ControlSize size, ComponentAppearance appearance, bool selectable, bool sortable, int activeSort, bool ascending, bool[] selectedRows, string[,] rows, Action<int, bool> onSort, Action<int, bool> onSelection)
        {
            layoutComponents.BeginHorizontalGroup(styleManager.GetTableHeaderStyle(appearance: appearance, size: size));
            if (selectable)
            {
                bool all = rows.GetLength(0) > 0 && Enumerable.Range(0, rows.GetLength(0)).All(i => i < selectedRows.Length && selectedRows[i]);
                bool next = guiHelper.Checkbox(string.Empty, all).Variant(ControlVariant.Default).IconSmall().FullRowClick(false).Appearance(appearance).Width(32f * guiHelper.uiScale);
                if (next != all)
                {
                    for (int i = 0; i < selectedRows.Length; i++)
                        selectedRows[i] = next;
                    for (int i = 0; i < selectedRows.Length; i++)
                        onSelection?.Invoke(i, next);
                }
            }

            for (int i = 0; i < headers.Length; i++)
            {
                float width = GetWidth(widths, i);
                string text = headers[i] ?? string.Empty;
                if (sortable)
                {
                    bool active = activeSort == i;
                    if (active)
                        text += ascending ? "  ↑" : "  ↓";
                    if (guiHelper.Button(text, active ? ControlVariant.Secondary : ControlVariant.Ghost, ControlSize.Small, appearance: appearance, options: width > 0 ? GUILayout.Width(width) : GUILayout.ExpandWidth(true)))
                        onSort?.Invoke(i, !active || !ascending);
                }
                else
                {
                    guiHelper.Label(text, ControlVariant.Default, appearance: appearance, options: width > 0 ? GUILayout.Width(width) : GUILayout.ExpandWidth(true));
                }
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawSelectionCell(bool[] selected, int row, ControlSize size, ComponentAppearance appearance, Action<int, bool> callback)
        {
            bool current = row < selected.Length && selected[row];
            bool next = guiHelper.Checkbox(string.Empty, current).Variant(ControlVariant.Default).IconSmall().FullRowClick(false).Appearance(appearance).Width(32f * guiHelper.uiScale);
            if (next != current)
            {
                selected[row] = next;
                callback?.Invoke(row, next);
            }
        }

        private void DrawPagination(int page, int pages, ControlSize size, ComponentAppearance appearance, Action<int> onPageChanged)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();
            guiHelper.MutedLabel($"Page {page + 1} of {pages}");
            guiHelper.Flex();
            if (guiHelper.Button("Previous", variant: ControlVariant.Outline, size: ControlSize.Small, disabled: page <= 0, appearance: appearance))
                onPageChanged(Mathf.Max(0, page - 1));
            if (guiHelper.Button("Next", variant: ControlVariant.Outline, size: ControlSize.Small, disabled: page >= pages - 1, appearance: appearance))
                onPageChanged(Mathf.Min(pages - 1, page + 1));
            layoutComponents.EndHorizontalGroup();
        }

        private static float GetWidth(float[] widths, int index) => widths != null && index < widths.Length && widths[index] > 0 ? widths[index] : 0;

        private static GUILayoutOption[] Combine(GUILayoutOption[] options, GUILayoutOption extra)
        {
            var result = new List<GUILayoutOption>(options ?? Array.Empty<GUILayoutOption>()) { extra };
            return result.ToArray();
        }

        private static string[,] SlicePage(string[,] data, int page, int pageSize)
        {
            if (data == null)
                return new string[0, 0];
            int start = Mathf.Clamp(page * Mathf.Max(1, pageSize), 0, data.GetLength(0));
            int count = Mathf.Clamp(pageSize, 0, data.GetLength(0) - start);
            var result = new string[count, data.GetLength(1)];
            for (int r = 0; r < count; r++)
            for (int c = 0; c < data.GetLength(1); c++)
                result[r, c] = data[start + r, c];
            return result;
        }

        private static string[,] ApplySorting(string[,] data, int column, bool ascending)
        {
            if (data == null || column < 0 || column >= data.GetLength(1))
                return data;
            var order = Enumerable.Range(0, data.GetLength(0));
            order = ascending ? order.OrderBy(i => data[i, column] ?? string.Empty, StringComparer.OrdinalIgnoreCase) : order.OrderByDescending(i => data[i, column] ?? string.Empty, StringComparer.OrdinalIgnoreCase);
            var indices = order.ToArray();
            var result = new string[data.GetLength(0), data.GetLength(1)];
            for (int r = 0; r < indices.Length; r++)
            for (int c = 0; c < data.GetLength(1); c++)
                result[r, c] = data[indices[r], c];
            return result;
        }
    }
}
