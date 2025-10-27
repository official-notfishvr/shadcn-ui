using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class DataTableColumn
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string AccessorKey { get; set; }
        public bool Sortable { get; set; } = true;
        public bool Filterable { get; set; } = true;
        public float Width { get; set; } = 120f;
        public float MinWidth { get; set; } = 80f;
        public bool CanHide { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public Func<object, string> CellRenderer { get; set; }
        public TextAnchor Alignment { get; set; } = TextAnchor.MiddleLeft;

        public DataTableColumn(string id, string header, string accessorKey = null)
        {
            Id = id;
            Header = header;
            AccessorKey = accessorKey ?? id;
        }
    }

    public class DataTableRow
    {
        public string Id { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public bool Selected { get; set; } = false;

        public DataTableRow(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            if (Data.TryGetValue(key, out object value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public void SetValue(string key, object value)
        {
            Data[key] = value;
        }

        public DataTableRow SetData(string key, object value)
        {
            Data[key] = value;
            return this;
        }
    }

    public class DataTableState
    {
        public string SortColumn { get; set; }
        public bool SortAscending { get; set; } = true;
        public string FilterText { get; set; } = "";
        public int CurrentPage { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public List<string> SelectedRows { get; set; } = new List<string>();
        public bool SelectAll { get; set; } = false;
        public Dictionary<string, bool> ColumnVisibility { get; set; } = new Dictionary<string, bool>();
        public bool ShowColumnToggle { get; set; } = false;
    }

    public class DataTable : BaseComponent
    {
        private Dictionary<string, DataTableState> _tableStates = new Dictionary<string, DataTableState>();

        public DataTable(GUIHelper helper)
            : base(helper) { }

        public void DrawDataTable(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, params GUILayoutOption[] options)
        {
            if (!_tableStates.ContainsKey(id))
            {
                _tableStates[id] = new DataTableState();
                foreach (var column in columns)
                {
                    _tableStates[id].ColumnVisibility[column.Id] = column.IsVisible;
                }
            }

            var state = _tableStates[id];
            state.ShowColumnToggle = showColumnToggle;

            var theme = styleManager?.GetTheme();
            var tableStyle = styleManager?.GetTableStyle(TableVariant.Default, TableSize.Default) ?? GUI.skin.box;

            layoutComponents.BeginVerticalGroup(tableStyle, options);

            DrawToolbar(id, state, columns, showSearch, showColumnToggle);

            var filteredData = FilterData(data, state.FilterText, columns);
            var sortedData = SortData(filteredData, state.SortColumn, state.SortAscending, columns);
            var paginatedData = showPagination ? PaginateData(sortedData, state.CurrentPage, state.PageSize) : sortedData;

            var visibleColumns = columns.Where(c => state.ColumnVisibility.ContainsKey(c.Id) ? state.ColumnVisibility[c.Id] : c.IsVisible).ToList();

            DrawTableHeader(id, visibleColumns, state, showSelection, data);
            DrawTableBody(id, visibleColumns, paginatedData, state, showSelection);

            if (showPagination && sortedData.Count > state.PageSize)
            {
                DrawPagination(id, state, sortedData.Count);
            }

            GUILayout.EndVertical();
        }

        private void DrawToolbar(string id, DataTableState state, List<DataTableColumn> columns, bool showSearch, bool showColumnToggle)
        {
            if (!showSearch && !showColumnToggle)
                return;

            layoutComponents.BeginHorizontalGroup();

            if (showSearch)
            {
                DrawSearchInput(id, state);
            }

            GUILayout.FlexibleSpace();

            if (showColumnToggle)
            {
                DrawColumnToggle(id, state, columns);
            }

            layoutComponents.EndHorizontalGroup();

            layoutComponents.AddSpace(8);
        }

        private void DrawSearchInput(string id, DataTableState state)
        {
            var styleManager = guiHelper.GetStyleManager();
            var labelStyle = styleManager?.GetLabelStyle(LabelVariant.Default) ?? GUI.skin.label;

            UnityHelpers.Label("Search:", labelStyle, GUILayout.Width(60 * guiHelper.uiScale));

            var inputStyle = styleManager?.GetInputStyle(InputVariant.Default) ?? GUI.skin.textField;

#if IL2CPP_MELONLOADER_PRE57
            string newFilterText = GUILayout.TextField(state.FilterText ?? "", inputStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(200 * guiHelper.uiScale) }));
#else
            string newFilterText = GUILayout.TextField(state.FilterText ?? "", inputStyle, GUILayout.Width(200 * guiHelper.uiScale));
#endif

            if (newFilterText != state.FilterText)
            {
                state.FilterText = newFilterText;
                state.CurrentPage = 0;
            }
        }

        private void DrawColumnToggle(string id, DataTableState state, List<DataTableColumn> columns)
        {
            guiHelper.Button("Columns ▼", ButtonVariant.Outline, ButtonSize.Small);
        }

        private void DrawTableHeader(string id, List<DataTableColumn> columns, DataTableState state, bool showSelection, List<DataTableRow> allData)
        {
            var styleManager = guiHelper.GetStyleManager();
            var headerStyle = styleManager?.GetTableHeaderStyle(TableVariant.Default, TableSize.Default) ?? GUI.skin.label;

            layoutComponents.BeginHorizontalGroup();

            if (showSelection)
            {
                bool allSelected = allData.Count > 0 && allData.All(row => state.SelectedRows.Contains(row.Id));
                bool newSelectAll = guiHelper.Toggle("", allSelected, ToggleVariant.Default, ToggleSize.Default, null, false, GUILayout.Width(20 * guiHelper.uiScale));

                if (newSelectAll != allSelected)
                {
                    if (newSelectAll)
                    {
                        foreach (var row in allData)
                        {
                            if (!state.SelectedRows.Contains(row.Id))
                            {
                                state.SelectedRows.Add(row.Id);
                            }
                        }
                    }
                    else
                    {
                        state.SelectedRows.Clear();
                    }
                    state.SelectAll = newSelectAll;
                }
            }

            foreach (var column in columns)
            {
                if (column.Sortable)
                {
                    string headerText = column.Header;
                    string sortIcon = "";

                    if (state.SortColumn == column.Id)
                    {
                        sortIcon = state.SortAscending ? " ↑" : " ↓";
                    }

                    if (guiHelper.Button(headerText + sortIcon, ButtonVariant.Ghost, ButtonSize.Default, null, false, 1f, GUILayout.Width(column.Width * guiHelper.uiScale)))
                    {
                        if (state.SortColumn == column.Id)
                        {
                            state.SortAscending = !state.SortAscending;
                        }
                        else
                        {
                            state.SortColumn = column.Id;
                            state.SortAscending = true;
                        }
                    }
                }
                else
                {
                    UnityHelpers.Label(column.Header, headerStyle, GUILayout.Width(column.Width * guiHelper.uiScale));
                }
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawTableBody(string id, List<DataTableColumn> columns, List<DataTableRow> data, DataTableState state, bool showSelection)
        {
            if (data.Count == 0)
            {
                DrawEmptyState();
                return;
            }

            var styleManager = guiHelper.GetStyleManager();
            var cellStyle = styleManager?.GetTableCellStyle(TableVariant.Default, TableSize.Default) ?? GUI.skin.label;

            for (int i = 0; i < data.Count; i++)
            {
                var row = data[i];
                bool isSelected = state.SelectedRows.Contains(row.Id);

                layoutComponents.BeginHorizontalGroup();

                if (showSelection)
                {
                    bool newSelected = guiHelper.Toggle("", isSelected, ToggleVariant.Default, ToggleSize.Default, null, false, GUILayout.Width(20 * guiHelper.uiScale));

                    if (newSelected != isSelected)
                    {
                        if (newSelected)
                        {
                            if (!state.SelectedRows.Contains(row.Id))
                                state.SelectedRows.Add(row.Id);
                        }
                        else
                        {
                            state.SelectedRows.Remove(row.Id);
                        }
                    }
                }

                foreach (var column in columns)
                {
                    string cellText = "";

                    if (column.CellRenderer != null)
                    {
                        object cellValue = row.Data.ContainsKey(column.AccessorKey) ? row.Data[column.AccessorKey] : null;
                        cellText = column.CellRenderer(cellValue);
                    }
                    else
                    {
                        cellText = row.GetValue<string>(column.AccessorKey, "");
                    }

                    var customCellStyle = new UnityHelpers.GUIStyle(cellStyle);
                    customCellStyle.alignment = column.Alignment;

                    string displayText = cellText ?? "";
                    if (string.IsNullOrEmpty(displayText))
                    {
                        displayText = "";
                    }

                    UnityHelpers.Label(displayText, customCellStyle, GUILayout.Width(column.Width * guiHelper.uiScale));
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawEmptyState()
        {
            var styleManager = guiHelper.GetStyleManager();
            var labelStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            UnityHelpers.Label("No results.", labelStyle);
            GUILayout.FlexibleSpace();
            layoutComponents.EndHorizontalGroup();

            layoutComponents.AddSpace(8);
        }

        private void DrawPagination(string id, DataTableState state, int totalItems)
        {
            int totalPages = Mathf.CeilToInt((float)totalItems / state.PageSize);

            layoutComponents.AddSpace(8);
            layoutComponents.BeginHorizontalGroup();

            if (guiHelper.Button("← Previous", ButtonVariant.Default, ButtonSize.Default, null, false, 1f, GUILayout.Width(80 * guiHelper.uiScale)))
            {
                if (state.CurrentPage > 0)
                    state.CurrentPage--;
            }

            GUILayout.FlexibleSpace();

            var styleManager = guiHelper.GetStyleManager();
            var labelStyle = styleManager?.GetLabelStyle(LabelVariant.Muted) ?? GUI.skin.label;
            string pageInfo = $"Page {state.CurrentPage + 1} of {totalPages}";
            UnityHelpers.Label(pageInfo, labelStyle);

            GUILayout.FlexibleSpace();

            if (guiHelper.Button("Next →", ButtonVariant.Default, ButtonSize.Default, null, false, 1f, GUILayout.Width(80 * guiHelper.uiScale)))
            {
                if (state.CurrentPage < totalPages - 1)
                    state.CurrentPage++;
            }

            layoutComponents.EndHorizontalGroup();
        }

        private List<DataTableRow> FilterData(List<DataTableRow> data, string filterText, List<DataTableColumn> columns)
        {
            if (string.IsNullOrEmpty(filterText))
                return data;

            return data.Where(row =>
                {
                    foreach (var column in columns.Where(c => c.Filterable))
                    {
                        string cellValue = row.GetValue<string>(column.AccessorKey, "");
                        if (cellValue.ToLower().Contains(filterText.ToLower()))
                            return true;
                    }
                    return false;
                })
                .ToList();
        }

        private List<DataTableRow> SortData(List<DataTableRow> data, string sortColumn, bool ascending, List<DataTableColumn> columns)
        {
            if (string.IsNullOrEmpty(sortColumn))
                return data;

            var column = columns.FirstOrDefault(c => c.Id == sortColumn);
            if (column == null || !column.Sortable)
                return data;

            return ascending ? data.OrderBy(row => row.GetValue<string>(column.AccessorKey, "")).ToList() : data.OrderByDescending(row => row.GetValue<string>(column.AccessorKey, "")).ToList();
        }

        private List<DataTableRow> PaginateData(List<DataTableRow> data, int currentPage, int pageSize)
        {
            return data.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public DataTableState GetTableState(string id)
        {
            return _tableStates.ContainsKey(id) ? _tableStates[id] : null;
        }

        public void SetPageSize(string id, int pageSize)
        {
            if (_tableStates.ContainsKey(id))
            {
                _tableStates[id].PageSize = pageSize;
                _tableStates[id].CurrentPage = 0;
            }
        }

        public void ClearSelection(string id)
        {
            if (_tableStates.ContainsKey(id))
            {
                _tableStates[id].SelectedRows.Clear();
                _tableStates[id].SelectAll = false;
            }
        }

        public List<string> GetSelectedRows(string id)
        {
            return _tableStates.ContainsKey(id) ? _tableStates[id].SelectedRows : new List<string>();
        }
    }
}
