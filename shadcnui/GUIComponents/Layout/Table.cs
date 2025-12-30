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
            if (config.Headers == null || config.Data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(config.Headers, config.Data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.Options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.Headers.Length; i++)
            {
                UnityHelpers.Label(config.Headers[i], headerStyle);
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = config.Data.GetLength(0);
            int colCount = config.Data.GetLength(1);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = config.Data[row, col] ?? "";
                    UnityHelpers.Label(cellValue, cellStyle);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void DrawRectTable(TableConfig config)
        {
            if (config.Headers == null || config.Data == null || !config.Rect.HasValue)
                return;

            var styleManager = guiHelper.GetStyleManager();
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
                    Headers = headers,
                    Data = data,
                    Variant = variant,
                    Size = size,
                    Options = options,
                }
            );
        }

        public void DrawTable(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            DrawRectTable(
                new TableConfig
                {
                    Rect = rect,
                    Headers = headers,
                    Data = data,
                    Variant = variant,
                    Size = size,
                }
            );
        }

        #endregion

        #region Sortable Table

        public void SortableTable(TableConfig config)
        {
            if (config.Headers == null || config.Data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(config.Headers, config.Data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.Options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.Headers.Length; i++)
            {
                int columnIndex = i;
                string headerText = config.Headers[i];

                if (config.SortColumns != null && config.SortAscending != null && i < config.SortColumns.Length)
                {
                    if (config.SortColumns[i] == i)
                    {
                        headerText += config.SortAscending[i] ? " ↑" : " ↓";
                    }
                }

                if (UnityHelpers.Button(headerText, headerStyle, config.Options))
                {
                    if (config.OnSort != null)
                    {
                        bool newAscending = true;
                        if (config.SortColumns != null && config.SortAscending != null && i < config.SortColumns.Length)
                        {
                            if (config.SortColumns[i] == i)
                                newAscending = !config.SortAscending[i];
                        }
                        config.OnSort.Invoke(columnIndex, newAscending);
                    }
                }
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = config.Data.GetLength(0);
            int colCount = config.Data.GetLength(1);
            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = config.Data[row, col] ?? "";
                    UnityHelpers.Label(cellValue, cellStyle);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            SortableTable(
                new TableConfig
                {
                    Headers = headers,
                    Data = data,
                    SortColumns = sortColumns,
                    SortAscending = sortAscending,
                    Variant = variant,
                    Size = size,
                    OnSort = onSort,
                    Options = options,
                }
            );
        }

        #endregion

        #region Selectable Table

        public void SelectableTable(TableConfig config)
        {
            if (config.Headers == null || config.Data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(config.Headers, config.Data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.Options);

            layoutComponents.BeginHorizontalGroup(headerStyle);

            UnityHelpers.Label("", headerStyle, GUILayout.Width(20 * guiHelper.uiScale));

            for (int i = 0; i < config.Headers.Length; i++)
            {
                UnityHelpers.Label(config.Headers[i], headerStyle);
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = config.Data.GetLength(0);
            int colCount = config.Data.GetLength(1);

            var selectedRows = config.SelectedRows ?? new bool[rowCount];

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);

                bool newSelected = UnityHelpers.Toggle(selectedRows[row], "", GUILayout.Width(20 * guiHelper.uiScale));

                if (newSelected != selectedRows[row])
                {
                    selectedRows[row] = newSelected;
                    config.OnSelectionChange?.Invoke(row, newSelected);
                }

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = config.Data[row, col] ?? "";
                    UnityHelpers.Label(cellValue, cellStyle);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            int rowCount = data.GetLength(0);
            if (selectedRows == null || selectedRows.Length != rowCount)
                selectedRows = new bool[rowCount];

            var config = new TableConfig
            {
                Headers = headers,
                Data = data,
                SelectedRows = selectedRows,
                Variant = variant,
                Size = size,
                OnSelectionChange = onSelectionChange,
                Options = options,
            };
            SelectableTable(config);
        }

        #endregion

        #region Custom Table

        public void CustomTable(TableConfig config)
        {
            if (config.Headers == null || config.ObjectData == null || config.CellRenderer == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(config.Headers, config.ObjectData);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.Options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.Headers.Length; i++)
            {
                UnityHelpers.Label(config.Headers[i], headerStyle);
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = config.ObjectData.GetLength(0);
            int colCount = config.ObjectData.GetLength(1);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);

                for (int col = 0; col < colCount; col++)
                {
                    object cellValue = config.ObjectData[row, col];
                    config.CellRenderer.Invoke(cellValue, row, col);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            CustomTable(
                new TableConfig
                {
                    Headers = headers,
                    ObjectData = data,
                    CellRenderer = cellRenderer,
                    Variant = variant,
                    Size = size,
                    Options = options,
                }
            );
        }

        #endregion

        #region Paginated Table

        public void PaginatedTable(TableConfig config)
        {
            if (config.Headers == null || config.Data == null)
                return;

            int totalRows = config.Data.GetLength(0);
            int pageSize = config.PageSize > 0 ? config.PageSize : 10;
            int totalPages = Mathf.Max(1, Mathf.CeilToInt((float)totalRows / pageSize));

            int currentPage = Mathf.Clamp(config.CurrentPage, 0, totalPages - 1);

            int startRow = currentPage * pageSize;
            int endRow = Mathf.Min(startRow + pageSize, totalRows);

            int pageRowCount = endRow - startRow;
            string[,] pageData = new string[pageRowCount, config.Data.GetLength(1)];

            for (int row = 0; row < pageRowCount; row++)
            for (int col = 0; col < config.Data.GetLength(1); col++)
                pageData[row, col] = config.Data[startRow + row, col];

            DrawTable(
                new TableConfig
                {
                    Headers = config.Headers,
                    Data = pageData,
                    Variant = config.Variant,
                    Size = config.Size,
                    Options = config.Options,
                }
            );

            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();

            if (guiHelper.Button("← Previous", ControlVariant.Outline, ControlSize.Default, null, false, 1f, GUILayout.Width(100 * guiHelper.uiScale)))
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    config.OnPageChange?.Invoke(currentPage);
                }
            }

            GUILayout.FlexibleSpace();

            string pageInfo = $"Page {currentPage + 1} of {totalPages}";
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle infoStyle = styleManager?.GetLabelStyle(ControlVariant.Muted) ?? GUI.skin.label;

            UnityHelpers.Label(pageInfo, infoStyle);

            GUILayout.FlexibleSpace();

            if (guiHelper.Button("Next →", ControlVariant.Outline, ControlSize.Default, null, false, 1f, GUILayout.Width(100 * guiHelper.uiScale)))
            {
                if (currentPage < totalPages - 1)
                {
                    currentPage++;
                    config.OnPageChange?.Invoke(currentPage);
                }
            }

            layoutComponents.EndHorizontalGroup();

            config.CurrentPage = currentPage;
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                Headers = headers,
                Data = data,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Variant = variant,
                Size = size,
                OnPageChange = onPageChange,
                Options = options,
            };
            PaginatedTable(config);
            currentPage = config.CurrentPage;
        }

        #endregion

        #region Searchable Table

        public void SearchableTable(TableConfig config)
        {
            if (config.Headers == null || config.Data == null)
                return;

            layoutComponents.BeginHorizontalGroup();

            var styleManager = guiHelper.GetStyleManager();
            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Default) ?? GUI.skin.label;
            var inputStyle = styleManager?.GetInputStyle(ControlVariant.Default) ?? GUI.skin.textField;

            UnityHelpers.Label("Search:", labelStyle, GUILayout.Width(60 * guiHelper.uiScale));

#if IL2CPP_MELONLOADER_PRE57
            string newSearchQuery = GUILayout.TextField(config.SearchQuery ?? "", inputStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(200 * guiHelper.uiScale) }));
#else
            string newSearchQuery = GUILayout.TextField(config.SearchQuery ?? "", inputStyle, GUILayout.Width(200 * guiHelper.uiScale));
#endif

            if (newSearchQuery != config.SearchQuery)
            {
                config.SearchQuery = newSearchQuery;
                config.OnSearch?.Invoke(config.SearchQuery);
                config.FilteredData = FilterTableData(config.Data, config.SearchQuery);
            }

            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);

            string[,] displayData = config.FilteredData ?? config.Data;

            DrawTable(
                new TableConfig
                {
                    Headers = config.Headers,
                    Data = displayData,
                    Variant = config.Variant,
                    Size = config.Size,
                    Options = config.Options,
                }
            );
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery, ref string[,] filteredData, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                Headers = headers,
                Data = data,
                SearchQuery = searchQuery,
                FilteredData = filteredData,
                Variant = variant,
                Size = size,
                OnSearch = onSearch,
                Options = options,
            };
            SearchableTable(config);
            searchQuery = config.SearchQuery;
            filteredData = config.FilteredData;
        }

        #endregion

        #region Resizable Table

        public void ResizableTable(TableConfig config)
        {
            if (config.Headers == null || config.Data == null)
                return;

            if (config.ColumnWidths == null || config.ColumnWidths.Length != config.Headers.Length)
            {
                config.ColumnWidths = new float[config.Headers.Length];
                for (int i = 0; i < config.ColumnWidths.Length; i++)
                {
                    config.ColumnWidths[i] = 100f;
                }
            }

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(config.Headers, config.Data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(config.Variant, config.Size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(config.Variant, config.Size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(config.Variant, config.Size);

            layoutComponents.BeginVerticalGroup(tableStyle, config.Options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < config.Headers.Length; i++)
            {
                float width = config.ColumnWidths[i] * guiHelper.uiScale;
                UnityHelpers.Label(config.Headers[i], headerStyle, GUILayout.Width(width));
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = config.Data.GetLength(0);
            int colCount = config.Data.GetLength(1);

            GUIStyle rowStyle = styleManager.GetTableRowStyle(config.Variant, config.Size);

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = config.Data[row, col] ?? "";
                    float width = config.ColumnWidths[col] * guiHelper.uiScale;
                    UnityHelpers.Label(cellValue, cellStyle, GUILayout.Width(width));
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
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
                Headers = headers,
                Data = data,
                ColumnWidths = columnWidths,
                Variant = variant,
                Size = size,
                Options = options,
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

        private string[,] FilterTableData(string[,] data, string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
                return data;

            List<int> matchingRows = new List<int>();
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
                    if (cellValue.ToLower().Contains(searchQuery.ToLower()))
                    {
                        matchingRows.Add(row);
                        break;
                    }
                }
            }

            string[,] filteredData = new string[matchingRows.Count, colCount];
            for (int i = 0; i < matchingRows.Count; i++)
            {
                int row = matchingRows[i];
                for (int col = 0; col < colCount; col++)
                {
                    filteredData[i, col] = data[row, col];
                }
            }

            return filteredData;
        }

        #endregion
    }
}
