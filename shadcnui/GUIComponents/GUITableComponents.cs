using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    internal class GUITableComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUITableComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public void Table(
            string[] headers,
            string[,] data,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            params GUILayoutOption[] options
        )
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
#if IL2CPP
                GUILayout.Label(
                    headers[i],
                    headerStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label(headers[i], headerStyle);
#endif
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
#if IL2CPP
                    GUILayout.Label(
                        cellValue,
                        cellStyle,
                        (Il2CppReferenceArray<GUILayoutOption>)null
                    );
#else
                    GUILayout.Label(cellValue, cellStyle);
#endif
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void Table(
            Rect rect,
            string[] headers,
            string[,] data,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default
        )
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

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            GUI.Box(scaledRect, "", tableStyle);

            GUILayout.BeginArea(scaledRect);
            Table(headers, data, variant, size);
            GUILayout.EndArea();
        }

        public void SortableTable(
            string[] headers,
            string[,] data,
            ref int[] sortColumns,
            ref bool[] sortAscending,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            Action<int, bool> onSort = null,
            params GUILayoutOption[] options
        )
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

#if IL2CPP
            var il2cppOptions =
                options != null ? (Il2CppReferenceArray<GUILayoutOption>)options : null;
#else
            var il2cppOptions = options;
#endif

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

#if IL2CPP
                if (GUILayout.Button(new GUIContent(headerText), headerStyle, il2cppOptions))
#else
                if (GUILayout.Button(headerText, headerStyle, options))
#endif
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
#if IL2CPP
                    GUILayout.Label(new GUIContent(cellValue), cellStyle, il2cppOptions);
#else
                    GUILayout.Label(cellValue, cellStyle);
#endif
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void SelectableTable(
            string[] headers,
            string[,] data,
            ref bool[] selectedRows,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            Action<int, bool> onSelectionChange = null,
            params GUILayoutOption[] options
        )
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

#if IL2CPP
            var il2cppOptions =
                options != null ? (Il2CppReferenceArray<GUILayoutOption>)options : null;
#else
            var il2cppOptions = options;
#endif

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            layoutComponents.BeginHorizontalGroup();
#if IL2CPP
            GUILayout.Label(GUIContent.none, GUI.skin.label, null);
#else
            GUILayout.Label("", GUILayout.Width(20 * guiHelper.uiScale));
#endif
            for (int i = 0; i < headers.Length; i++)
            {
#if IL2CPP
                GUILayout.Label(new GUIContent(headers[i]), headerStyle, null);
#else
                GUILayout.Label(headers[i], headerStyle);
#endif
            }
            layoutComponents.EndHorizontalGroup();

            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            if (selectedRows == null || selectedRows.Length != rowCount)
                selectedRows = new bool[rowCount];

            for (int row = 0; row < rowCount; row++)
            {
                layoutComponents.BeginHorizontalGroup();
#if IL2CPP
                bool newSelected = GUILayout.Toggle(
                    selectedRows[row],
                    "",
                    GUI.skin.toggle,
                    new Il2CppReferenceArray<GUILayoutOption>(
                        new GUILayoutOption[] { GUILayout.Width(20 * guiHelper.uiScale) }
                    )
                );
#else
                bool newSelected = GUILayout.Toggle(
                    selectedRows[row],
                    "",
                    GUILayout.Width(20 * guiHelper.uiScale)
                );
#endif

                if (newSelected != selectedRows[row])
                {
                    selectedRows[row] = newSelected;
                    onSelectionChange?.Invoke(row, newSelected);
                }

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
#if IL2CPP
                    GUILayout.Label(new GUIContent(cellValue), cellStyle, il2cppOptions);
#else
                    GUILayout.Label(cellValue, cellStyle);
#endif
                }

                layoutComponents.EndHorizontalGroup();
            }

            layoutComponents.EndVerticalGroup();
        }

        public void CustomTable(
            string[] headers,
            object[,] data,
            Action<object, int, int> cellRenderer,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            params GUILayoutOption[] options
        )
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
#if IL2CPP
                GUILayout.Label(
                    headers[i],
                    headerStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label(headers[i], headerStyle);
#endif
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

        public void PaginatedTable(
            string[] headers,
            string[,] data,
            ref int currentPage,
            int pageSize,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            Action<int> onPageChange = null,
            params GUILayoutOption[] options
        )
        {
            if (headers == null || data == null)
                return;

#if IL2CPP
            var il2cppOptions =
                options != null ? (Il2CppReferenceArray<GUILayoutOption>)options : null;
#else
            var il2cppOptions = options;
#endif

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

            Table(headers, pageData, variant, size, options);

            layoutComponents.AddSpace(8);
            layoutComponents.BeginHorizontalGroup();

#if IL2CPP
            if (GUILayout.Button(new GUIContent("← Previous"), GUI.skin.button, null))
#else
            if (GUILayout.Button("← Previous", GUILayout.Width(80 * guiHelper.uiScale)))
#endif
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

#if IL2CPP
            GUILayout.Label(new GUIContent(pageInfo), infoStyle, null);
#else
            GUILayout.Label(pageInfo, infoStyle);
#endif

            GUILayout.FlexibleSpace();

#if IL2CPP
            if (GUILayout.Button(new GUIContent("Next →"), GUI.skin.button, null))
#else
            if (GUILayout.Button("Next →", GUILayout.Width(80 * guiHelper.uiScale)))
#endif
            {
                if (currentPage < totalPages - 1)
                {
                    currentPage++;
                    onPageChange?.Invoke(currentPage);
                }
            }

            layoutComponents.EndHorizontalGroup();
        }

        public void SearchableTable(
            string[] headers,
            string[,] data,
            ref string searchQuery,
            ref string[,] filteredData,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            Action<string> onSearch = null,
            params GUILayoutOption[] options
        )
        {
            if (headers == null || data == null)
                return;

#if IL2CPP
            var il2cppOptions =
                options != null ? (Il2CppReferenceArray<GUILayoutOption>)options : null;
#else
            var il2cppOptions = options;
#endif

            layoutComponents.BeginHorizontalGroup();

#if IL2CPP
            GUILayout.Label(new GUIContent("Search:"), GUI.skin.label, null);
#else
            GUILayout.Label("Search:", GUILayout.Width(60 * guiHelper.uiScale));
#endif
#if IL2CPP
            string newSearchQuery = GUILayout.TextField(
                searchQuery ?? "",
                GUI.skin.textField,
                new Il2CppReferenceArray<GUILayoutOption>(
                    new GUILayoutOption[] { GUILayout.Width(200 * guiHelper.uiScale) }
                )
            );
#else
            string newSearchQuery = GUILayout.TextField(
                searchQuery ?? "",
                GUILayout.Width(200 * guiHelper.uiScale)
            );
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
            Table(headers, displayData, variant, size, options);
        }

        public void ResizableTable(
            string[] headers,
            string[,] data,
            ref float[] columnWidths,
            TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default,
            params GUILayoutOption[] options
        )
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

#if IL2CPP
                GUILayout.Label(
                    headers[i],
                    headerStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)
                        new GUILayoutOption[] { GUILayout.Width(width) }
                );
#else
                GUILayout.Label(headers[i], headerStyle, GUILayout.Width(width));
#endif
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

#if IL2CPP
                    GUILayout.Label(
                        cellValue,
                        cellStyle,
                        (Il2CppReferenceArray<GUILayoutOption>)
                            new GUILayoutOption[] { GUILayout.Width(width) }
                    );
#else
                    GUILayout.Label(cellValue, cellStyle, GUILayout.Width(width));
#endif
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
#if IL2CPP
                GUILayout.Label(new GUIContent(headers[i]), GUI.skin.label, null);
#else
                GUILayout.Label(headers[i], GUI.skin.label);
#endif
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
#if IL2CPP
                    GUILayout.Label(new GUIContent(cellValue), GUI.skin.label, null);
#else
                    GUILayout.Label(cellValue, GUI.skin.label);
#endif
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
#if IL2CPP
                GUILayout.Label(new GUIContent(headers[i]), GUI.skin.label, null);
#else
                GUILayout.Label(headers[i], GUI.skin.label);
#endif
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
#if IL2CPP
                    GUILayout.Label(new GUIContent(cellText), GUI.skin.label, null);
#else
                    GUILayout.Label(cellText, GUI.skin.label);
#endif
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
