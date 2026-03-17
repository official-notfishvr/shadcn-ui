using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Data
{
    public class DataTable : BaseComponent
    {
        private readonly Dictionary<string, DataTableState> _states = new();
        private readonly Dictionary<string, Rect> _columnMenuAnchors = new();

        public DataTable(GUIHelper helper)
            : base(helper) { }

        public void DrawDataTable(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, params GUILayoutOption[] options)
        {
            if (string.IsNullOrEmpty(id) || columns == null)
                return;

            data ??= new List<DataTableRow>();
            var state = GetOrCreateState(id, columns);
            state.ShowColumnToggle = showColumnToggle;

            GUIStyle tableStyle = styleManager?.GetTableStyle(ControlVariant.Default, ControlSize.Small) ?? GUI.skin.box;
            layoutComponents.BeginVerticalGroup(tableStyle, options ?? Array.Empty<GUILayoutOption>());

            DrawToolbar(id, state, columns, showSearch, showColumnToggle);

            var visibleColumns = columns.Where(c => state.ColumnVisibility.TryGetValue(c.Id, out bool visible) ? visible : c.IsVisible).ToList();
            var filtered = FilterData(data, state.FilterText, columns);
            var sorted = SortData(filtered, state.SortColumn, state.SortAscending, columns);
            var paginated = showPagination ? Paginate(sorted, state.CurrentPage, state.PageSize) : sorted;

            DrawHeader(id, visibleColumns, state, showSelection, sorted);
            DrawRows(id, visibleColumns, paginated, state, showSelection);

            if (showPagination && sorted.Count > state.PageSize)
                DrawPagination(state, sorted.Count);

            layoutComponents.EndVerticalGroup();

            if (showColumnToggle)
                DrawColumnMenuOverlay(id, state, columns);
        }

        public DataTableState GetTableState(string id) => _states.TryGetValue(id, out var state) ? state : null;

        public void SetPageSize(string id, int pageSize)
        {
            if (_states.TryGetValue(id, out var state))
            {
                state.PageSize = Mathf.Max(1, pageSize);
                state.CurrentPage = 0;
            }
        }

        public void ClearSelection(string id)
        {
            if (_states.TryGetValue(id, out var state))
                state.SelectedRows.Clear();
        }

        public List<string> GetSelectedRows(string id) => _states.TryGetValue(id, out var state) ? state.SelectedRows : new List<string>();

        private DataTableState GetOrCreateState(string id, List<DataTableColumn> columns)
        {
            if (!_states.TryGetValue(id, out var state))
                _states[id] = state = new DataTableState();

            foreach (var column in columns)
            {
                if (!state.ColumnVisibility.ContainsKey(column.Id))
                    state.ColumnVisibility[column.Id] = column.IsVisible;
            }

            return state;
        }

        private void DrawToolbar(string id, DataTableState state, List<DataTableColumn> columns, bool showSearch, bool showColumnToggle)
        {
            if (!showSearch && !showColumnToggle)
                return;

            layoutComponents.BeginHorizontalGroup();

            if (showSearch)
            {
                UnityHelpers.Label("Search:", styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small) ?? GUI.skin.label, GUILayout.Width(60f * guiHelper.uiScale));

                var inputCfg = new InputConfig
                {
                    Id = $"datatable_search_{id}",
                    Value = state.FilterText ?? string.Empty,
                    Placeholder = "Filter rows",
                    Variant = ControlVariant.Default,
                    Size = ControlSize.Small,
                    Width = 200,
                    Height = 32f,
                    OnValueChanged = v =>
                    {
                        state.FilterText = v;
                        state.CurrentPage = 0;
                    },
                };

                guiHelper.Input(inputCfg);
            }

            GUILayout.FlexibleSpace();

            if (showColumnToggle)
            {
                if (guiHelper.Button("Columns", ControlVariant.Outline, ControlSize.Small))
                    ToggleColumnMenu(id, columns);

                if (Event.current.type == EventType.Repaint)
                    _columnMenuAnchors[id] = GUILayoutUtility.GetLastRect();
            }

            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private void DrawHeader(string id, List<DataTableColumn> columns, DataTableState state, bool showSelection, List<DataTableRow> allData)
        {
            var headerStyle = styleManager?.GetTableHeaderStyle(ControlVariant.Default, ControlSize.Small) ?? GUI.skin.label;
            layoutComponents.BeginHorizontalGroup();

            if (showSelection)
            {
                bool allSelected = allData.Count > 0 && allData.All(row => state.SelectedRows.Contains(row.Id));
                bool newSelectAll = UnityHelpers.Toggle(allSelected, string.Empty, GUI.skin.toggle, GUILayout.Width(18f * guiHelper.uiScale), GUILayout.Height(18f * guiHelper.uiScale));
                if (newSelectAll != allSelected)
                {
                    if (newSelectAll)
                    {
                        state.SelectedRows.Clear();
                        foreach (var row in allData.Select(r => r.Id).Distinct())
                            state.SelectedRows.Add(row);
                    }
                    else
                        state.SelectedRows.Clear();
                }
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            foreach (var column in columns)
            {
                float width = column.Width * guiHelper.uiScale;
                if (column.IsSortable)
                {
                    string sortGlyph = state.SortColumn == column.Id ? (state.SortAscending ? " ↑" : " ↓") : string.Empty;
                    if (UnityHelpers.Button(column.Header + sortGlyph, headerStyle, GUILayout.Width(width)))
                    {
                        if (state.SortColumn == column.Id)
                            state.SortAscending = !state.SortAscending;
                        else
                        {
                            state.SortColumn = column.Id;
                            state.SortAscending = true;
                        }
                    }
                }
                else
                {
                    UnityHelpers.Label(column.Header, headerStyle, GUILayout.Width(width));
                }
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawRows(string id, List<DataTableColumn> columns, List<DataTableRow> data, DataTableState state, bool showSelection)
        {
            if (data.Count == 0)
            {
                DrawEmptyState();
                return;
            }

            var rowStyle = styleManager?.GetTableRowStyle(ControlVariant.Default, ControlSize.Small) ?? GUI.skin.box;

            foreach (var row in data)
            {
                layoutComponents.BeginHorizontalGroup(rowStyle);

                if (showSelection)
                {
                    bool selected = state.SelectedRows.Contains(row.Id);
                    bool newSelected = UnityHelpers.Toggle(selected, string.Empty, GUI.skin.toggle, GUILayout.Width(18f * guiHelper.uiScale), GUILayout.Height(18f * guiHelper.uiScale));
                    if (newSelected != selected)
                    {
                        if (newSelected)
                            state.SelectedRows.Add(row.Id);
                        else
                            state.SelectedRows.Remove(row.Id);
                    }
                    layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                }

                foreach (var column in columns)
                {
                    string cellText;
                    if (column.CellRenderer != null)
                    {
                        object cellValue = row.Data.ContainsKey(column.AccessorKey) ? row.Data[column.AccessorKey] : null;
                        cellText = column.CellRenderer(cellValue);
                    }
                    else
                    {
                        cellText = row.GetValue<string>(column.AccessorKey, string.Empty);
                    }

                    var cellStyle = styleManager?.GetTableCellStyle(ControlVariant.Default, ControlSize.Small, column.Alignment) ?? GUI.skin.label;
                    UnityHelpers.Label(cellText ?? string.Empty, cellStyle, GUILayout.Width(column.Width * guiHelper.uiScale));
                }

                layoutComponents.EndHorizontalGroup();
            }
        }

        private void DrawPagination(DataTableState state, int totalItems)
        {
            int totalPages = Mathf.CeilToInt((float)totalItems / state.PageSize);

            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();

            if (guiHelper.Button("← Previous", ControlVariant.Outline, ControlSize.Small, null, false, 1f, GUILayout.Width(90f * guiHelper.uiScale)))
                state.CurrentPage = Mathf.Max(0, state.CurrentPage - 1);

            GUILayout.FlexibleSpace();

            UnityHelpers.Label($"Page {state.CurrentPage + 1} of {totalPages}", styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small) ?? GUI.skin.label);

            GUILayout.FlexibleSpace();

            if (guiHelper.Button("Next →", ControlVariant.Outline, ControlSize.Small, null, false, 1f, GUILayout.Width(90f * guiHelper.uiScale)))
                state.CurrentPage = Mathf.Min(totalPages - 1, state.CurrentPage + 1);

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawEmptyState()
        {
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            UnityHelpers.Label("No results.", styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small) ?? GUI.skin.label);
            GUILayout.FlexibleSpace();
            layoutComponents.EndHorizontalGroup();
        }

        private List<DataTableRow> FilterData(List<DataTableRow> data, string filterText, List<DataTableColumn> columns)
        {
            if (string.IsNullOrWhiteSpace(filterText))
                return data;

            string needle = filterText.Trim().ToLowerInvariant();
            return data.Where(row =>
                    columns
                        .Where(c => c.IsFilterable)
                        .Any(column =>
                        {
                            string cell = row.GetValue<string>(column.AccessorKey, string.Empty);
                            return cell != null && cell.ToLowerInvariant().Contains(needle);
                        })
                )
                .ToList();
        }

        private List<DataTableRow> SortData(List<DataTableRow> data, string sortColumn, bool ascending, List<DataTableColumn> columns)
        {
            if (string.IsNullOrEmpty(sortColumn))
                return data;

            var column = columns.FirstOrDefault(c => c.Id == sortColumn);
            if (column == null || !column.IsSortable)
                return data;

            return ascending ? data.OrderBy(row => row.GetValue<string>(column.AccessorKey, string.Empty)).ToList() : data.OrderByDescending(row => row.GetValue<string>(column.AccessorKey, string.Empty)).ToList();
        }

        private List<DataTableRow> Paginate(List<DataTableRow> data, int page, int pageSize)
        {
            int p = Mathf.Max(0, page);
            int size = Mathf.Max(1, pageSize);
            return data.Skip(p * size).Take(size).ToList();
        }

        private void ToggleColumnMenu(string id, List<DataTableColumn> columns)
        {
            string menuId = $"datatable_cols_{id}";

            if (LayerManager.Instance.IsOpen(menuId))
            {
                LayerManager.Instance.Close(menuId);
                return;
            }

            Rect anchor = _columnMenuAnchors.TryGetValue(id, out var rect) ? rect : new Rect(0, 0, 200, 30);
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(anchor.x, anchor.yMax + 4));

            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = menuId,
                    OpenPosition = screenPos,
                    Width = Mathf.Max(anchor.width, 220f * guiHelper.uiScale),
                    Height = 240f * guiHelper.uiScale,
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Dropdown,
                    Content = () => DrawColumnMenu(id, columns),
                }
            );
        }

        private void DrawColumnMenuOverlay(string id, DataTableState state, List<DataTableColumn> columns)
        {
        }

        private void DrawColumnMenu(string tableId, List<DataTableColumn> columns)
        {
            var menuStyle = styleManager?.GetDropdownMenuStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.box;
            var itemStyle = styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label;

            layoutComponents.BeginVerticalGroup(menuStyle, GUILayout.ExpandWidth(true));
            if (columns.Count == 0)
            {
                UnityHelpers.Label("No columns", itemStyle);
            }
            else
            {
                if (!_states.TryGetValue(tableId, out var state))
                {
                    layoutComponents.EndVerticalGroup();
                    return;
                }

                foreach (var column in columns)
                {
                    bool visible = state.ColumnVisibility.TryGetValue(column.Id, out var isVisible) ? isVisible : column.IsVisible;
                    bool newVisible = UnityHelpers.Toggle(visible, column.Header ?? column.Id, GUI.skin.toggle, GUILayout.ExpandWidth(true));
                    state.ColumnVisibility[column.Id] = newVisible;
                }
            }
            layoutComponents.EndVerticalGroup();
        }
    }
}
