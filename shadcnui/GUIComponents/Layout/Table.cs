using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Layout
{
    public class Table : BaseComponent
    {
        public Table(GUIHelper helper)
            : base(helper) { }

        #region Config-based API

        public void DrawTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null)
                return;

            if (styleManager == null)
            {
                DrawSimpleTable(config.ColumnHeaders, config.Rows);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.LayoutOptions);

            DrawTableHeader(config, headerStyle);

            int rowCount = config.Rows.GetLength(0);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                DrawTableRow(row, config, rowStyle, cellStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawTableHeader(TableConfig config, GUIStyle headerStyle)
        {
            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.ColumnHeaders.Length; i++)
            {
                UnityHelpers.Label(config.ColumnHeaders[i], headerStyle);
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawTableRow(int rowIndex, TableConfig config, GUIStyle rowStyle, GUIStyle cellStyle)
        {
            layoutComponents.BeginHorizontalGroup(rowStyle);

            int colCount = config.Rows.GetLength(1);
            for (int col = 0; col < colCount; col++)
            {
                string cellValue = config.Rows[rowIndex, col] ?? "";
                UnityHelpers.Label(cellValue, cellStyle);
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void DrawRectTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null || !config.Rect.HasValue)
                return;

            if (styleManager == null)
            {
                GUI.Box(config.Rect.Value, "Table", GUI.skin.box);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);

            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);

            GUI.Box(scaledRect, "", tableStyle);

            GUILayout.BeginArea(scaledRect);
            DrawTable(config);
            GUILayout.EndArea();
        }

        #endregion

        #region API

        public void DrawTable(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawTable(
                new TableConfig
                {
                    ColumnHeaders = headers,
                    Rows = data,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options,
                }
            );
        }

        public void DrawTable(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            DrawRectTable(
                new TableConfig
                {
                    Rect = rect,
                    ColumnHeaders = headers,
                    Rows = data,
                    Variant = variant,
                    Size = size,
                }
            );
        }

        #endregion

        #region Sortable Table

        public void SortableTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null)
                return;

            if (styleManager == null)
            {
                DrawSimpleTable(config.ColumnHeaders, config.Rows);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.LayoutOptions);

            DrawSortableHeader(config, headerStyle);

            int rowCount = config.Rows.GetLength(0);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                DrawTableRow(row, config, rowStyle, cellStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawSortableHeader(TableConfig config, GUIStyle headerStyle)
        {
            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.ColumnHeaders.Length; i++)
            {
                int columnIndex = i;
                string headerText = config.ColumnHeaders[i];

                if (config.SortColumnIndices != null && config.SortAscending != null && i < config.SortColumnIndices.Length)
                {
                    if (config.SortColumnIndices[i] == i)
                    {
                        headerText += config.SortAscending[i] ? " ↑" : " ↓";
                    }
                }

                if (UnityHelpers.Button(headerText, headerStyle, config.LayoutOptions))
                {
                    if (config.OnSortChanged != null)
                    {
                        bool newAscending = true;
                        if (config.SortColumnIndices != null && config.SortAscending != null && i < config.SortColumnIndices.Length)
                        {
                            if (config.SortColumnIndices[i] == i)
                                newAscending = !config.SortAscending[i];
                        }
                        config.OnSortChanged.Invoke(columnIndex, newAscending);
                    }
                }
            }
            layoutComponents.EndHorizontalGroup();
        }

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumnIndices, ref bool[] sortAscending, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSortChanged = null, params GUILayoutOption[] options)
        {
            SortableTable(
                new TableConfig
                {
                    ColumnHeaders = headers,
                    Rows = data,
                    SortColumnIndices = sortColumnIndices,
                    SortAscending = sortAscending,
                    Variant = variant,
                    Size = size,
                    OnSortChanged = onSortChanged,
                    LayoutOptions = options,
                }
            );
        }

        #endregion

        #region Selectable Table

        public void SelectableTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null)
                return;

            if (styleManager == null)
            {
                DrawSimpleTable(config.ColumnHeaders, config.Rows);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.LayoutOptions);

            DrawSelectableHeader(headerStyle);

            int rowCount = config.Rows.GetLength(0);

            var selectedRowFlags = config.SelectedRowFlags ?? new bool[rowCount];

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                DrawSelectableRow(row, config, selectedRowFlags, rowStyle, cellStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawSelectableHeader(GUIStyle headerStyle)
        {
            layoutComponents.BeginHorizontalGroup(headerStyle);
            UnityHelpers.Label("", headerStyle, GUILayout.Width(20 * guiHelper.uiScale));
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawSelectableRow(int rowIndex, TableConfig config, bool[] selectedRowFlags, GUIStyle rowStyle, GUIStyle cellStyle)
        {
            layoutComponents.BeginHorizontalGroup(rowStyle);

            bool newSelected = UnityHelpers.Toggle(selectedRowFlags[rowIndex], "", GUI.skin.toggle, GUILayout.Width(20 * guiHelper.uiScale));

            if (newSelected != selectedRowFlags[rowIndex])
            {
                selectedRowFlags[rowIndex] = newSelected;
                config.OnSelectionChanged?.Invoke(rowIndex, newSelected);
            }

            int colCount = config.Rows.GetLength(1);
            for (int col = 0; col < colCount; col++)
            {
                string cellValue = config.Rows[rowIndex, col] ?? "";
                UnityHelpers.Label(cellValue, cellStyle);
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRowFlags, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChanged = null, params GUILayoutOption[] options)
        {
            int rowCount = data.GetLength(0);
            if (selectedRowFlags == null || selectedRowFlags.Length != rowCount)
                selectedRowFlags = new bool[rowCount];

            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                SelectedRowFlags = selectedRowFlags,
                Variant = variant,
                Size = size,
                OnSelectionChanged = onSelectionChanged,
                LayoutOptions = options,
            };
            SelectableTable(config);
        }

        #endregion

        #region Custom Table

        public void CustomTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.ObjectRows == null || config.CellRenderer == null)
                return;

            if (styleManager == null)
            {
                DrawSimpleTable(config.ColumnHeaders, config.ObjectRows);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.LayoutOptions);

            DrawTableHeader(config, headerStyle);

            int rowCount = config.ObjectRows.GetLength(0);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                DrawCustomTableRow(row, config, rowStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawCustomTableRow(int rowIndex, TableConfig config, GUIStyle rowStyle)
        {
            layoutComponents.BeginHorizontalGroup(rowStyle);

            int colCount = config.ObjectRows.GetLength(1);
            for (int col = 0; col < colCount; col++)
            {
                object cellValue = config.ObjectRows[rowIndex, col];
                config.CellRenderer.Invoke(cellValue, rowIndex, col);
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            CustomTable(
                new TableConfig
                {
                    ColumnHeaders = headers,
                    ObjectRows = data,
                    CellRenderer = cellRenderer,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options,
                }
            );
        }

        #endregion

        #region Paginated Table

        public void PaginatedTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null)
                return;

            int totalRows = config.Rows.GetLength(0);
            int pageSize = config.PageSize > 0 ? config.PageSize : 10;
            int totalPages = Mathf.Max(1, Mathf.CeilToInt((float)totalRows / pageSize));

            int currentPage = Mathf.Clamp(config.CurrentPage, 0, totalPages - 1);

            int startRow = currentPage * pageSize;
            int endRow = Mathf.Min(startRow + pageSize, totalRows);

            int pageRowCount = endRow - startRow;
            string[,] pageRows = new string[pageRowCount, config.Rows.GetLength(1)];

            for (int row = 0; row < pageRowCount; row++)
            {
                for (int col = 0; col < config.Rows.GetLength(1); col++)
                {
                    pageRows[row, col] = config.Rows[startRow + row, col];
                }
            }

            DrawTable(
                new TableConfig
                {
                    ColumnHeaders = config.ColumnHeaders,
                    Rows = pageRows,
                    Variant = config.Variant,
                    Size = config.Size,
                    LayoutOptions = config.LayoutOptions,
                }
            );

            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();

            if (guiHelper.Button("← Previous", ControlVariant.Outline, ControlSize.Default, null, false, 1f, GUILayout.Width(100 * guiHelper.uiScale)))
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    config.OnPageChanged?.Invoke(currentPage);
                }
            }

            GUILayout.FlexibleSpace();

            string pageInfo = $"Page {currentPage + 1} of {totalPages}";
            GUIStyle infoStyle = styleManager?.GetLabelStyle(ControlVariant.Muted) ?? GUI.skin.label;

            UnityHelpers.Label(pageInfo, infoStyle);

            GUILayout.FlexibleSpace();

            if (guiHelper.Button("Next →", ControlVariant.Outline, ControlSize.Default, null, false, 1f, GUILayout.Width(100 * guiHelper.uiScale)))
            {
                if (currentPage < totalPages - 1)
                {
                    currentPage++;
                    config.OnPageChanged?.Invoke(currentPage);
                }
            }

            layoutComponents.EndHorizontalGroup();

            config.CurrentPage = currentPage;
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChanged = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Variant = variant,
                Size = size,
                OnPageChanged = onPageChanged,
                LayoutOptions = options,
            };
            PaginatedTable(config);
            currentPage = config.CurrentPage;
        }

        #endregion

        #region Searchable Table

        public void SearchableTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null)
                return;

            layoutComponents.BeginHorizontalGroup();

            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default) ?? GUI.skin.label;
            var inputStyle = styleManager?.GetInputStyle(ControlVariant.Default) ?? GUI.skin.textField;

            UnityHelpers.Label("Search:", labelStyle, GUILayout.Width(60 * guiHelper.uiScale));

#if IL2CPP_MELONLOADER_PRE57
            string newSearchText = GUILayout.TextField(config.SearchText ?? "", inputStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(200 * guiHelper.uiScale) }));
#else
            string newSearchText = GUILayout.TextField(config.SearchText ?? "", inputStyle, GUILayout.Width(200 * guiHelper.uiScale));
#endif

            if (newSearchText != config.SearchText)
            {
                config.SearchText = newSearchText;
                config.OnSearchChanged?.Invoke(config.SearchText);
                config.FilteredRows = FilterTableData(config.Rows, config.SearchText);
            }

            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);

            string[,] displayRows = config.FilteredRows ?? config.Rows;

            DrawTable(
                new TableConfig
                {
                    ColumnHeaders = config.ColumnHeaders,
                    Rows = displayRows,
                    Variant = config.Variant,
                    Size = config.Size,
                    LayoutOptions = config.LayoutOptions,
                }
            );
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchText, ref string[,] filteredRows, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearchChanged = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                SearchText = searchText,
                FilteredRows = filteredRows,
                Variant = variant,
                Size = size,
                OnSearchChanged = onSearchChanged,
                LayoutOptions = options,
            };
            SearchableTable(config);
            searchText = config.SearchText;
            filteredRows = config.FilteredRows;
        }

        #endregion

        #region Resizable Table

        public void ResizableTable(TableConfig config)
        {
            if (config.ColumnHeaders == null || config.Rows == null)
                return;

            if (config.ColumnWidths == null || config.ColumnWidths.Length != config.ColumnHeaders.Length)
            {
                config.ColumnWidths = new float[config.ColumnHeaders.Length];
                for (int i = 0; i < config.ColumnWidths.Length; i++)
                {
                    config.ColumnWidths[i] = 100f;
                }
            }

            if (styleManager == null)
            {
                DrawSimpleTable(config.ColumnHeaders, config.Rows);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.LayoutOptions);

            DrawResizableHeader(config, headerStyle);

            int rowCount = config.Rows.GetLength(0);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                DrawResizableRow(row, config, rowStyle, cellStyle);
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawResizableHeader(TableConfig config, GUIStyle headerStyle)
        {
            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.ColumnHeaders.Length; i++)
            {
                float width = config.ColumnWidths[i] * guiHelper.uiScale;
                UnityHelpers.Label(config.ColumnHeaders[i], headerStyle, GUILayout.Width(width));
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawResizableRow(int rowIndex, TableConfig config, GUIStyle rowStyle, GUIStyle cellStyle)
        {
            layoutComponents.BeginHorizontalGroup(rowStyle);

            int colCount = config.Rows.GetLength(1);
            for (int col = 0; col < colCount; col++)
            {
                string cellValue = config.Rows[rowIndex, col] ?? "";
                float width = config.ColumnWidths[col] * guiHelper.uiScale;
                UnityHelpers.Label(cellValue, cellStyle, GUILayout.Width(width));
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            if (columnWidths == null || columnWidths.Length != headers.Length)
            {
                columnWidths = new float[headers.Length];
                for (int i = 0; i < columnWidths.Length; i++)
                    columnWidths[i] = 100f;
            }

            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                ColumnWidths = columnWidths,
                Variant = variant,
                Size = size,
                LayoutOptions = options,
            };
            ResizableTable(config);
        }

        #endregion

        #region Internal Helpers

        private void DrawSimpleTable(string[] headers, string[,] data)
        {
            layoutComponents.BeginVerticalGroup(GUI.skin.box);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                UnityHelpers.Label(headers[i], GUI.skin.label);
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup();

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
                    UnityHelpers.Label(cellValue, GUI.skin.label);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawSimpleTable(string[] headers, object[,] data)
        {
            layoutComponents.BeginVerticalGroup(GUI.skin.box);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                UnityHelpers.Label(headers[i], GUI.skin.label);
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup();

                for (int col = 0; col < colCount; col++)
                {
                    object cellValue = data[row, col];
                    string cellText = cellValue?.ToString() ?? "";
                    UnityHelpers.Label(cellText, GUI.skin.label);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        private static string[,] FilterTableData(string[,] data, string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return data;

            var matchingRows = new List<int>();
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
                    if (cellValue.ToLower().Contains(searchText.ToLower()))
                    {
                        matchingRows.Add(row);
                        break;
                    }
                }
            }

            string[,] filteredRows = new string[matchingRows.Count, colCount];
            for (int i = 0; i < matchingRows.Count; i++)
            {
                int row = matchingRows[i];
                for (int col = 0; col < colCount; col++)
                {
                    filteredRows[i, col] = data[row, col];
                }
            }

            return filteredRows;
        }

        #endregion
    }
}
