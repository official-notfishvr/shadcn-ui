using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using shadcnui;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Table
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;

        public Table(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new Layout(helper);
        }

        public void DrawTable(string[] headers, string[,] data, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            if (headers == null || data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                UnityHelpers.Label(headers[i], headerStyle);
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
                    UnityHelpers.Label(cellValue, cellStyle);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void DrawTable(Rect rect, string[] headers, string[,] data, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default)
        {
            if (headers == null || data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                GUI.Box(rect, "Table", GUI.skin.box);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            GUI.Box(scaledRect, "", tableStyle);

            GUILayout.BeginArea(scaledRect);
            DrawTable(headers, data, variant, size);
            GUILayout.EndArea();
        }

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                int columnIndex = i;
                string headerText = headers[i];

                if (sortColumns != null && sortAscending != null && i < sortColumns.Length)
                {
                    if (sortColumns[i] == i)
                    {
                        headerText += sortAscending[i] ? " ↑" : " ↓";
                    }
                }

                if (UnityHelpers.Button(headerText, headerStyle, options))
                {
                    if (onSort != null)
                    {
                        bool newAscending = true;
                        if (sortColumns != null && sortAscending != null && i < sortColumns.Length)
                        {
                            if (sortColumns[i] == i)
                                newAscending = !sortAscending[i];
                        }
                        onSort.Invoke(columnIndex, newAscending);
                    }
                }
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

                    UnityHelpers.Label(cellValue, cellStyle);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            layoutComponents.BeginHorizontalGroup();

            UnityHelpers.Label("", GUILayout.Width(20 * guiHelper.uiScale));

            for (int i = 0; i < headers.Length; i++)
            {
                UnityHelpers.Label(headers[i], headerStyle);
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            if (selectedRows == null || selectedRows.Length != rowCount)
                selectedRows = new bool[rowCount];

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup();

                bool newSelected = UnityHelpers.Toggle(selectedRows[row], "", GUILayout.Width(20 * guiHelper.uiScale));

                if (newSelected != selectedRows[row])
                {
                    selectedRows[row] = newSelected;
                    onSelectionChange?.Invoke(row, newSelected);
                }

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
                    UnityHelpers.Label(cellValue, cellStyle);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            if (headers == null || data == null || cellRenderer == null)
                return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                UnityHelpers.Label(headers[i], headerStyle);
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
                    cellRenderer.Invoke(cellValue, row, col);
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null)
                return;

            int totalRows = data.GetLength(0);
            int totalPages = Mathf.CeilToInt((float)totalRows / pageSize);

            if (currentPage < 0)
                currentPage = 0;
            if (currentPage >= totalPages)
                currentPage = totalPages - 1;

            int startRow = currentPage * pageSize;
            int endRow = Mathf.Min(startRow + pageSize, totalRows);

            int pageRowCount = endRow - startRow;
            string[,] pageData = new string[pageRowCount, data.GetLength(1)];

            for (int row = 0; row < pageRowCount; row++)
            for (int col = 0; col < data.GetLength(1); col++)
                pageData[row, col] = data[startRow + row, col];

            DrawTable(headers, pageData, variant, size, options);

            layoutComponents.AddSpace(8);
            layoutComponents.BeginHorizontalGroup();

            if (UnityHelpers.Button("← Previous", GUILayout.Width(80 * guiHelper.uiScale)))
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    onPageChange?.Invoke(currentPage);
                }
            }

            GUILayout.FlexibleSpace();

            string pageInfo = $"Page {currentPage + 1} of {totalPages}";
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle infoStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

            UnityHelpers.Label(pageInfo, infoStyle);

            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("Next →", GUILayout.Width(80 * guiHelper.uiScale)))
            {
                if (currentPage < totalPages - 1)
                {
                    currentPage++;
                    onPageChange?.Invoke(currentPage);
                }
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery, ref string[,] filteredData, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null)
                return;

            layoutComponents.BeginHorizontalGroup();

            UnityHelpers.Label("Search:", GUILayout.Width(60 * guiHelper.uiScale));

#if IL2CPP_MELONLOADER
            string newSearchQuery = GUILayout.TextField(searchQuery ?? "", GUI.skin.textField, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(200 * guiHelper.uiScale) }));
#else
            string newSearchQuery = GUILayout.TextField(searchQuery ?? "", GUILayout.Width(200 * guiHelper.uiScale));
#endif

            if (newSearchQuery != searchQuery)
            {
                searchQuery = newSearchQuery;
                onSearch?.Invoke(searchQuery);

                filteredData = FilterTableData(data, searchQuery);
            }

            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(8);

            string[,] displayData = filteredData ?? data;
            DrawTable(headers, displayData, variant, size, options);
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            if (headers == null || data == null)
                return;

            if (columnWidths == null || columnWidths.Length != headers.Length)
            {
                columnWidths = new float[headers.Length];
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    columnWidths[i] = 100f;
                }
            }

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < headers.Length; i++)
            {
                int columnIndex = i;
                float width = columnWidths[i] * guiHelper.uiScale;

                UnityHelpers.Label(headers[i], headerStyle, GUILayout.Width(width));
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
                    float width = columnWidths[col] * guiHelper.uiScale;

                    UnityHelpers.Label(cellValue, cellStyle, GUILayout.Width(width));
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

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
    }
}
