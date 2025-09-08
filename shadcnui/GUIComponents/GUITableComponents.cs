using shadcnui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUITableComponents
    {
        private GUIHelper guiHelper;

        public GUITableComponents(GUIHelper helper)
        {
            guiHelper = helper;
        }

        public void Table(string[] headers, string[,] data, TableVariant variant = TableVariant.Default,
            TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            if (headers == null || data == null) return;

            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            GUILayout.BeginVertical(tableStyle, options);

            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
#if IL2CPP
                GUILayout.Label(headers[i], headerStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(headers[i], headerStyle);
#endif
            }
            GUILayout.EndHorizontal();

            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();

                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
#if IL2CPP
                    GUILayout.Label(cellValue, cellStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                    GUILayout.Label(cellValue, cellStyle);
#endif
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        public void Table(Rect rect, string[] headers, string[,] data, TableVariant variant = TableVariant.Default, 
            TableSize size = TableSize.Default)
        {
            if (headers == null || data == null) return;
            
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

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, 
            Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null) return;
            
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            GUILayout.BeginVertical(tableStyle, options);
            
            GUILayout.BeginHorizontal();
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
                
                if (GUILayout.Button(headerText, headerStyle))
                {
                    if (onSort != null)
                    {
                        bool newAscending = true;
                        if (sortColumns != null && sortAscending != null && i < sortColumns.Length)
                        {
                            if (sortColumns[i] == i)
                            {
                                newAscending = !sortAscending[i];
                            }
                        }
                        onSort.Invoke(columnIndex, newAscending);
                    }
                }
            }
            GUILayout.EndHorizontal();
            
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
#if IL2CPP
                    GUILayout.Label(cellValue, cellStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                    GUILayout.Label(cellValue, cellStyle);
#endif
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, 
            Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null) return;
            
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);
            GUIStyle cellStyle = styleManager.GetTableCellStyle(variant, size);

            GUILayout.BeginVertical(tableStyle, options);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Width(20 * guiHelper.uiScale));
            for (int i = 0; i < headers.Length; i++)
            {
#if IL2CPP
                GUILayout.Label(headers[i], headerStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(headers[i], headerStyle);
#endif
            }
            GUILayout.EndHorizontal();
            
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            
            if (selectedRows == null || selectedRows.Length != rowCount)
            {
                selectedRows = new bool[rowCount];
            }
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                bool newSelected = GUILayout.Toggle(selectedRows[row], "", GUILayout.Width(20 * guiHelper.uiScale));
                if (newSelected != selectedRows[row])
                {
                    selectedRows[row] = newSelected;
                    onSelectionChange?.Invoke(row, newSelected);
                }
                
                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
#if IL2CPP
                    GUILayout.Label(cellValue, cellStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                    GUILayout.Label(cellValue, cellStyle);
#endif
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, 
            params GUILayoutOption[] options)
        {
            if (headers == null || data == null || cellRenderer == null) return;
            
            var styleManager = guiHelper.GetStyleManager();
            if (styleManager == null)
            {
                DrawSimpleTable(headers, data);
                return;
            }

            GUIStyle tableStyle = styleManager.GetTableStyle(variant, size);
            GUIStyle headerStyle = styleManager.GetTableHeaderStyle(variant, size);

            GUILayout.BeginVertical(tableStyle, options);
            
            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
#if IL2CPP
                GUILayout.Label(headers[i], headerStyle, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                GUILayout.Label(headers[i], headerStyle);
#endif
            }
            GUILayout.EndHorizontal();
            
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                for (int col = 0; col < colCount; col++)
                {
                    object cellValue = data[row, col];
                    cellRenderer.Invoke(cellValue, row, col);
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, 
            Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null) return;
            
            int totalRows = data.GetLength(0);
            int totalPages = Mathf.CeilToInt((float)totalRows / pageSize);
            
            if (currentPage < 0) currentPage = 0;
            if (currentPage >= totalPages) currentPage = totalPages - 1;
            
            int startRow = currentPage * pageSize;
            int endRow = Mathf.Min(startRow + pageSize, totalRows);
            
            int pageRowCount = endRow - startRow;
            string[,] pageData = new string[pageRowCount, data.GetLength(1)];
            
            for (int row = 0; row < pageRowCount; row++)
            {
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    pageData[row, col] = data[startRow + row, col];
                }
            }
            
            Table(headers, pageData, variant, size, options);
            
            GUILayout.Space(8 * guiHelper.uiScale);
            GUILayout.BeginHorizontal();
            
            bool prevClicked = GUILayout.Button("← Previous", GUILayout.Width(80 * guiHelper.uiScale));
            if (prevClicked && currentPage > 0)
            {
                currentPage--;
                onPageChange?.Invoke(currentPage);
            }
            
            GUILayout.FlexibleSpace();
            
            string pageInfo = $"Page {currentPage + 1} of {totalPages}";
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle infoStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
            GUILayout.Label(pageInfo, infoStyle);
            
            GUILayout.FlexibleSpace();
            
            bool nextClicked = GUILayout.Button("Next →", GUILayout.Width(80 * guiHelper.uiScale));
            if (nextClicked && currentPage < totalPages - 1)
            {
                currentPage++;
                onPageChange?.Invoke(currentPage);
            }
            
            GUILayout.EndHorizontal();
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery, ref string[,] filteredData,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, 
            Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            if (headers == null || data == null) return;
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search:", GUILayout.Width(60 * guiHelper.uiScale));
            string newSearchQuery = GUILayout.TextField(searchQuery ?? "", GUILayout.Width(200 * guiHelper.uiScale));
            if (newSearchQuery != searchQuery)
            {
                searchQuery = newSearchQuery;
                onSearch?.Invoke(searchQuery);
                
                filteredData = FilterTableData(data, searchQuery);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space(8 * guiHelper.uiScale);
            
            string[,] displayData = filteredData ?? data;
            Table(headers, displayData, variant, size, options);
        }


        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths,
            TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, 
            params GUILayoutOption[] options)
        {
            if (headers == null || data == null) return;
            
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

            GUILayout.BeginVertical(tableStyle, options);
            
            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                int columnIndex = i;
                float width = columnWidths[i] * guiHelper.uiScale;
                
#if IL2CPP
                GUILayout.Label(headers[i], headerStyle, 
                    (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(width) });
#else
                GUILayout.Label(headers[i], headerStyle, GUILayout.Width(width));
#endif
            }
            GUILayout.EndHorizontal();
            
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
                    float width = columnWidths[col] * guiHelper.uiScale;
                    
#if IL2CPP
                    GUILayout.Label(cellValue, cellStyle, 
                        (Il2CppReferenceArray<GUILayoutOption>)new GUILayoutOption[] { GUILayout.Width(width) });
#else
                    GUILayout.Label(cellValue, cellStyle, GUILayout.Width(width));
#endif
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        private void DrawSimpleTable(string[] headers, string[,] data)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            
            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                GUILayout.Label(headers[i], GUI.skin.label);
            }
            GUILayout.EndHorizontal();
            
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                for (int col = 0; col < colCount; col++)
                {
                    string cellValue = data[row, col] ?? "";
                    GUILayout.Label(cellValue, GUI.skin.label);
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        private void DrawSimpleTable(string[] headers, object[,] data)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            
            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                GUILayout.Label(headers[i], GUI.skin.label);
            }
            GUILayout.EndHorizontal();
            
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            
            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();
                
                for (int col = 0; col < colCount; col++)
                {
                    object cellValue = data[row, col];
                    string cellText = cellValue?.ToString() ?? "";
                    GUILayout.Label(cellText, GUI.skin.label);
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
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
