using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
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

        public DataTable(GUIHelper helper) : base(helper) { }
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

            var containerStyle = new UnityHelpers.GUIStyle(styleManager.cardStyle);
            containerStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);

            layoutComponents.BeginVerticalGroup(containerStyle);

            DrawToolbar(id, state, columns, showSearch, showColumnToggle);

            var tableContainerStyle = new UnityHelpers.GUIStyle();
            var theme = ThemeManager.Instance.CurrentTheme;
            tableContainerStyle.normal.background = styleManager.CreateSolidTexture(theme.ButtonOutlineBorder);
            tableContainerStyle.padding = new UnityHelpers.RectOffset(1, 1, 1, 1);

            layoutComponents.BeginVerticalGroup(tableContainerStyle);

            var filteredData = FilterData(data, state.FilterText, columns);
            var sortedData = SortData(filteredData, state.SortColumn, state.SortAscending, columns);
            var paginatedData = showPagination ? PaginateData(sortedData, state.CurrentPage, state.PageSize) : sortedData;

            var visibleColumns = columns.Where(c => state.ColumnVisibility.ContainsKey(c.Id) ? state.ColumnVisibility[c.Id] : c.IsVisible).ToList();

            DrawTableHeader(id, visibleColumns, state, showSelection);
            DrawTableBody(id, visibleColumns, paginatedData, state, showSelection);

            GUILayout.EndVertical();

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

            var toolbarStyle = new UnityHelpers.GUIStyle();
            toolbarStyle.padding = new UnityHelpers.RectOffset(16, 16, 16, 16);

            layoutComponents.BeginVerticalGroup();
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
            GUILayout.EndVertical();
        }

        private void DrawSearchInput(string id, DataTableState state)
        {
            var searchContainerStyle = new UnityHelpers.GUIStyle();
            searchContainerStyle.fixedWidth = 250;

            layoutComponents.BeginHorizontalGroup();

            var iconStyle = new UnityHelpers.GUIStyle(styleManager.labelMutedStyle);
            iconStyle.alignment = TextAnchor.MiddleCenter;
            iconStyle.fixedWidth = 20;
            guiHelper.Label("🔍", LabelVariant.Muted);

            var inputStyle = new UnityHelpers.GUIStyle(styleManager.inputDefaultStyle);
            inputStyle.margin.left = 0;
            inputStyle.stretchWidth = true;

            string placeholder = string.IsNullOrEmpty(state.FilterText) ? "Filter..." : "";

            string newFilterText = UnityHelpers.TextField(state.FilterText, inputStyle);
            if (newFilterText != state.FilterText)
            {
                state.FilterText = newFilterText;
                state.CurrentPage = 0;
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawColumnToggle(string id, DataTableState state, List<DataTableColumn> columns)
        {
            if (guiHelper.Button("Columns ▼", ButtonVariant.Outline)) { }
        }

        private void DrawTableHeader(string id, List<DataTableColumn> columns, DataTableState state, bool showSelection)
        {
            var theme = styleManager.GetTheme();

            var headerStyle = new UnityHelpers.GUIStyle();
            headerStyle.normal.background = styleManager.CreateSolidTexture(theme.SecondaryColor);
            headerStyle.padding = new UnityHelpers.RectOffset(12, 12, 12, 12);
            headerStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);

            layoutComponents.BeginHorizontalGroup(headerStyle);

            if (showSelection)
            {
                var checkboxStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                checkboxStyle.fixedWidth = 16;
                checkboxStyle.fixedHeight = 16;
                checkboxStyle.margin = new UnityHelpers.RectOffset(0, 8, 0, 0);

                bool newSelectAll = UnityHelpers.Toggle(state.SelectAll, "", checkboxStyle);
                if (newSelectAll != state.SelectAll)
                {
                    state.SelectAll = newSelectAll;
                }
            }

            foreach (var column in columns)
            {
                var columnHeaderStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                columnHeaderStyle.fontStyle = FontStyle.Bold;
                columnHeaderStyle.fontSize = 12;
                columnHeaderStyle.alignment = TextAnchor.MiddleLeft;
                columnHeaderStyle.normal.textColor = theme.TextColor != Color.clear ? theme.TextColor : Color.black;
                columnHeaderStyle.padding = new UnityHelpers.RectOffset(4, 4, 0, 0);

                if (column.Sortable)
                {
                    string headerText = column.Header;
                    string sortIcon = "";

                    if (state.SortColumn == column.Id)
                    {
                        sortIcon = state.SortAscending ? " ↑" : " ↓";
                        columnHeaderStyle.normal.textColor = theme.PrimaryColor;
                    }

                    if (guiHelper.Button(headerText + sortIcon, ButtonVariant.Ghost, ButtonSize.Small))
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
                    guiHelper.Label(column.Header, LabelVariant.Default);
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

            var theme = styleManager.GetTheme();

            for (int i = 0; i < data.Count; i++)
            {
                var row = data[i];
                bool isSelected = state.SelectedRows.Contains(row.Id);
                bool isEven = i % 2 == 0;

                var rowStyle = new UnityHelpers.GUIStyle();
                if (isSelected)
                {
                    rowStyle.normal.background = styleManager.CreateSolidTexture(new Color(theme.PrimaryColor.r, theme.PrimaryColor.g, theme.PrimaryColor.b, 0.1f));
                }
                else if (isEven)
                {
                    rowStyle.normal.background = styleManager.CreateSolidTexture(theme.BackgroundColor);
                }
                else
                {
                    rowStyle.normal.background = styleManager.CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.3f));
                }

                rowStyle.padding = new UnityHelpers.RectOffset(12, 12, 8, 8);
                rowStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);

                layoutComponents.BeginHorizontalGroup(rowStyle, GUILayout.ExpandWidth(true), GUILayout.Height(40));

                if (showSelection)
                {
                    bool newSelected = guiHelper.Checkbox(row.Id, isSelected, CheckboxVariant.Default, CheckboxSize.Default, null, false, GUILayout.Width(16), GUILayout.Height(16));

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

                    var cellStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                    cellStyle.alignment = column.Alignment;
                    cellStyle.fontSize = 14;
                    cellStyle.normal.textColor = theme.TextColor != Color.clear && theme.TextColor.a > 0 ? theme.TextColor : Color.white;
                    cellStyle.wordWrap = false;
                    cellStyle.clipping = TextClipping.Clip;
                    cellStyle.padding = new UnityHelpers.RectOffset(4, 4, 0, 0);

                    string displayText = cellText ?? "";
                    if (string.IsNullOrEmpty(displayText))
                    {
                        displayText = $"[{column.AccessorKey}]";
                    }

                    UnityHelpers.Label(displayText, cellStyle, GUILayout.Width(column.Width));
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawEmptyState()
        {
            var emptyStyle = new UnityHelpers.GUIStyle();
            emptyStyle.padding = new UnityHelpers.RectOffset(24, 24, 48, 48);
            emptyStyle.alignment = TextAnchor.MiddleCenter;

            layoutComponents.BeginVerticalGroup();

            var emptyTextStyle = new UnityHelpers.GUIStyle(styleManager.labelMutedStyle);
            emptyTextStyle.fontSize = 16;
            emptyTextStyle.alignment = TextAnchor.MiddleCenter;

            guiHelper.Label("No results.", LabelVariant.Default);
            GUILayout.Space(8);

            var emptySubtextStyle = new UnityHelpers.GUIStyle(styleManager.labelMutedStyle);
            emptySubtextStyle.fontSize = 14;
            emptySubtextStyle.alignment = TextAnchor.MiddleCenter;

            guiHelper.Label("No data to display.", LabelVariant.Muted);

            GUILayout.EndVertical();
        }

        private void DrawPagination(string id, DataTableState state, int totalItems)
        {
            int totalPages = Mathf.CeilToInt((float)totalItems / state.PageSize);

            var paginationStyle = new UnityHelpers.GUIStyle();
            paginationStyle.padding = new UnityHelpers.RectOffset(16, 16, 16, 16);
            paginationStyle.alignment = TextAnchor.MiddleRight;

            layoutComponents.BeginHorizontalGroup(paginationStyle);

            var theme = styleManager.GetTheme();
            var labelStyle = new UnityHelpers.GUIStyle(styleManager.labelMutedStyle);
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.padding = new UnityHelpers.RectOffset(0, 16, 0, 0);

            int from = state.CurrentPage * state.PageSize + 1;
            int to = Mathf.Min((state.CurrentPage + 1) * state.PageSize, totalItems);
            UnityHelpers.Label($"{from}-{to} of {totalItems}", labelStyle);

            var buttonStyle = new UnityHelpers.GUIStyle(styleManager.buttonOutlineStyle);
            buttonStyle.fixedWidth = 32;

            if (guiHelper.Button("‹‹", ButtonVariant.Outline, ButtonSize.Small))
            {
                state.CurrentPage = 0;
            }
            if (guiHelper.Button("‹", ButtonVariant.Outline, ButtonSize.Small))
            {
                if (state.CurrentPage > 0)
                    state.CurrentPage--;
            }

            var pageLabelStyle = new UnityHelpers.GUIStyle(styleManager.labelDefaultStyle);
            pageLabelStyle.alignment = TextAnchor.MiddleCenter;
            pageLabelStyle.padding = new UnityHelpers.RectOffset(8, 8, 0, 0);
            UnityHelpers.Label($"{state.CurrentPage + 1} / {totalPages}", pageLabelStyle);

            if (guiHelper.Button("›", ButtonVariant.Outline, ButtonSize.Small))
            {
                if (state.CurrentPage < totalPages - 1)
                    state.CurrentPage++;
            }
            if (guiHelper.Button("››", ButtonVariant.Outline, ButtonSize.Small))
            {
                state.CurrentPage = totalPages - 1;
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
