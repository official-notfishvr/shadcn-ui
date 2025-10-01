using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
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

    public class DataTable
    {
        private readonly GUIHelper _guiHelper;
        private readonly StyleManager _styleManager;
        private readonly Layout _layoutComponents;

        private Dictionary<string, DataTableState> _tableStates = new Dictionary<string, DataTableState>();

        public DataTable(GUIHelper helper)
        {
            _guiHelper = helper;
            _styleManager = helper.GetStyleManager();
            _layoutComponents = new Layout(helper);
        }

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

            var containerStyle = new UnityHelpers.GUIStyle(_styleManager.cardStyle);
            containerStyle.padding = new UnityHelpers.RectOffset(0, 0, 0, 0);

            _layoutComponents.BeginVerticalGroup(containerStyle);

            DrawToolbar(id, state, columns, showSearch, showColumnToggle);

            var tableContainerStyle = new UnityHelpers.GUIStyle();
            var theme = ThemeManager.Instance.CurrentTheme;
            tableContainerStyle.normal.background = _styleManager.CreateSolidTexture(theme.ButtonOutlineBorder);
            tableContainerStyle.padding = new UnityHelpers.RectOffset(1, 1, 1, 1);

            _layoutComponents.BeginVerticalGroup(tableContainerStyle);

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

            _layoutComponents.BeginVerticalGroup();
            _layoutComponents.BeginHorizontalGroup();

            if (showSearch)
            {
                DrawSearchInput(id, state);
            }

            GUILayout.FlexibleSpace();

            if (showColumnToggle)
            {
                DrawColumnToggle(id, state, columns);
            }

            _layoutComponents.EndHorizontalGroup();
            GUILayout.EndVertical();
        }

        private void DrawSearchInput(string id, DataTableState state)
        {
            var searchContainerStyle = new UnityHelpers.GUIStyle();
            searchContainerStyle.fixedWidth = 250;

            _layoutComponents.BeginHorizontalGroup();

            var iconStyle = new UnityHelpers.GUIStyle(_styleManager.labelMutedStyle);
            iconStyle.alignment = TextAnchor.MiddleCenter;
            iconStyle.fixedWidth = 20;
            _guiHelper.Label("🔍", LabelVariant.Muted);

            var inputStyle = new UnityHelpers.GUIStyle(_styleManager.inputDefaultStyle);
            inputStyle.margin.left = 0;
            inputStyle.stretchWidth = true;

            string placeholder = string.IsNullOrEmpty(state.FilterText) ? "Filter..." : "";
#if IL2CPP_MELONLOADER
            string newFilterText = GUILayout.TextField(state.FilterText, inputStyle, new Il2CppReferenceArray<GUILayoutOption>(0));
#else
            string newFilterText = GUILayout.TextField(state.FilterText, inputStyle);
#endif
            if (newFilterText != state.FilterText)
            {
                state.FilterText = newFilterText;
                state.CurrentPage = 0;
            }

            _layoutComponents.EndHorizontalGroup();
        }

        private void DrawColumnToggle(string id, DataTableState state, List<DataTableColumn> columns)
        {
            if (_guiHelper.Button("Columns ▼", ButtonVariant.Outline)) { }
        }

        private void DrawTableHeader(string id, List<DataTableColumn> columns, DataTableState state, bool showSelection)
        {
            var theme = _styleManager.GetTheme();

            var headerStyle = new UnityHelpers.GUIStyle();
            headerStyle.normal.background = _styleManager.CreateSolidTexture(theme.SecondaryColor);
            headerStyle.padding = new UnityHelpers.RectOffset(12, 12, 12, 12);
            headerStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);

            _layoutComponents.BeginHorizontalGroup();

            if (showSelection)
            {
                var checkboxStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                checkboxStyle.fixedWidth = 16;
                checkboxStyle.fixedHeight = 16;
                checkboxStyle.margin = new UnityHelpers.RectOffset(0, 8, 0, 0);

#if IL2CPP_MELONLOADER
                bool newSelectAll = GUILayout.Toggle(state.SelectAll, new GUIContent(""), checkboxStyle, Layout.EmptyOptions);
#else
                bool newSelectAll = GUILayout.Toggle(state.SelectAll, "", checkboxStyle);
#endif
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

                    var buttonStyle = new UnityHelpers.GUIStyle(columnHeaderStyle);
                    buttonStyle.normal.background = null;
                    buttonStyle.hover.background = _styleManager.CreateSolidTexture(new Color(0, 0, 0, 0.05f));
                    buttonStyle.active.background = _styleManager.CreateSolidTexture(new Color(0, 0, 0, 0.1f));

                    if (_guiHelper.Button(headerText + sortIcon, ButtonVariant.Ghost))
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
                    _guiHelper.Label(column.Header, LabelVariant.Default);
                }
            }

            _layoutComponents.EndHorizontalGroup();
        }

        private void DrawTableBody(string id, List<DataTableColumn> columns, List<DataTableRow> data, DataTableState state, bool showSelection)
        {
            if (data.Count == 0)
            {
                DrawEmptyState();
                return;
            }

            var theme = _styleManager.GetTheme();

            for (int i = 0; i < data.Count; i++)
            {
                var row = data[i];
                bool isSelected = state.SelectedRows.Contains(row.Id);
                bool isEven = i % 2 == 0;

                var rowStyle = new UnityHelpers.GUIStyle();
                if (isSelected)
                {
                    rowStyle.normal.background = _styleManager.CreateSolidTexture(new Color(theme.PrimaryColor.r, theme.PrimaryColor.g, theme.PrimaryColor.b, 0.1f));
                }
                else if (isEven)
                {
                    rowStyle.normal.background = _styleManager.CreateSolidTexture(theme.BackgroundColor);
                }
                else
                {
                    rowStyle.normal.background = _styleManager.CreateSolidTexture(new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.3f));
                }

                rowStyle.padding = new UnityHelpers.RectOffset(12, 12, 8, 8);
                rowStyle.margin = new UnityHelpers.RectOffset(0, 0, 0, 0);

#if IL2CPP_MELONLOADER
                GUILayout.BeginHorizontal(rowStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(40) }));
#else
                GUILayout.BeginHorizontal(rowStyle, GUILayout.ExpandWidth(true), GUILayout.Height(40));
#endif

                if (showSelection)
                {
                    var checkboxStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                    checkboxStyle.fixedWidth = 16;
                    checkboxStyle.fixedHeight = 16;
                    checkboxStyle.margin = new UnityHelpers.RectOffset(0, 8, 0, 0);

#if IL2CPP_MELONLOADER
                    bool newSelected = GUILayout.Toggle(isSelected, new GUIContent(""), checkboxStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(24) }));
#else
                    bool newSelected = GUILayout.Toggle(isSelected, "", checkboxStyle, GUILayout.Width(24));
#endif
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

#if IL2CPP_MELONLOADER
                    GUILayout.Label(new GUIContent(displayText), cellStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(column.Width) }));
#else
                    GUILayout.Label(displayText, cellStyle, GUILayout.Width(column.Width));
#endif
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawEmptyState()
        {
            var emptyStyle = new UnityHelpers.GUIStyle();
            emptyStyle.padding = new UnityHelpers.RectOffset(24, 24, 48, 48);
            emptyStyle.alignment = TextAnchor.MiddleCenter;

            _layoutComponents.BeginVerticalGroup();

            var emptyTextStyle = new UnityHelpers.GUIStyle(_styleManager.labelMutedStyle);
            emptyTextStyle.fontSize = 16;
            emptyTextStyle.alignment = TextAnchor.MiddleCenter;

            _guiHelper.Label("No results.", LabelVariant.Default);
            GUILayout.Space(8);

            var emptySubtextStyle = new UnityHelpers.GUIStyle(_styleManager.labelMutedStyle);
            emptySubtextStyle.fontSize = 14;
            emptySubtextStyle.alignment = TextAnchor.MiddleCenter;

            _guiHelper.Label("No data to display.", LabelVariant.Muted);

            GUILayout.EndVertical();
        }

        private void DrawPagination(string id, DataTableState state, int totalItems)
        {
            int totalPages = Mathf.CeilToInt((float)totalItems / state.PageSize);

            _layoutComponents.BeginHorizontalGroup();

            _guiHelper.Label($"Showing {state.CurrentPage * state.PageSize + 1}-{Mathf.Min((state.CurrentPage + 1) * state.PageSize, totalItems)} of {totalItems}", LabelVariant.Muted);

            GUILayout.FlexibleSpace();

#if IL2CPP_MELONLOADER
            if (GUILayout.Button("‹‹", _styleManager.buttonOutlineStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(32) })))
#else
            if (GUILayout.Button("‹‹", _styleManager.buttonOutlineStyle, GUILayout.Width(32)))
#endif
            {
                state.CurrentPage = 0;
            }

#if IL2CPP_MELONLOADER
            if (GUILayout.Button("‹", _styleManager.buttonOutlineStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(32) })))
#else
            if (GUILayout.Button("‹", _styleManager.buttonOutlineStyle, GUILayout.Width(32)))
#endif
            {
                if (state.CurrentPage > 0)
                    state.CurrentPage--;
            }

            _guiHelper.Label($"Page {state.CurrentPage + 1} of {totalPages}", LabelVariant.Default);

#if IL2CPP_MELONLOADER
            if (GUILayout.Button("›", _styleManager.buttonOutlineStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(32) })))
#else
            if (GUILayout.Button("›", _styleManager.buttonOutlineStyle, GUILayout.Width(32)))
#endif
            {
                if (state.CurrentPage < totalPages - 1)
                    state.CurrentPage++;
            }

#if IL2CPP_MELONLOADER
            if (GUILayout.Button("››", _styleManager.buttonOutlineStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(32) })))
#else
            if (GUILayout.Button("››", _styleManager.buttonOutlineStyle, GUILayout.Width(32)))
#endif
            {
                state.CurrentPage = totalPages - 1;
            }

            _layoutComponents.EndHorizontalGroup();
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
