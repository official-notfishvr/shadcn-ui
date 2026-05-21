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

        public DataTable(GUIHelper helper)
            : base(helper) { }

        public void Render(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, ComponentAppearance appearance = null, params GUILayoutOption[] options)
        {
            if (string.IsNullOrEmpty(id))
                id = "datatable";

            columns ??= new List<DataTableColumn>();
            data ??= new List<DataTableRow>();
            DataTableState state = GetTableState(id);
            state.ShowColumnToggle = showColumnToggle;

            layoutComponents.BeginVerticalGroup(styleManager.GetCardStyle(ControlVariant.Default, ControlSize.Default, appearance), options ?? Array.Empty<GUILayoutOption>());
            if (showSearch)
                DrawSearch(state);

            var visibleColumns = columns.Where(c => c != null && c.IsVisible).ToList();
            var filteredRows = FilterRows(data, visibleColumns, state.FilterText);
            var pagedRows = GetPagedRows(filteredRows, state);
            DrawTable(state, visibleColumns, pagedRows, showSelection);

            if (showPagination)
                DrawPagination(state, filteredRows.Count);

            layoutComponents.EndVerticalGroup();
        }

        public DataTableState GetTableState(string id)
        {
            if (!_states.TryGetValue(id, out var state))
            {
                state = new DataTableState();
                _states[id] = state;
            }
            return state;
        }

        public void SetPageSize(string id, int size)
        {
            GetTableState(id).PageSize = Mathf.Max(1, size);
        }

        public void ClearSelection(string id)
        {
            GetTableState(id).SelectedRows.Clear();
        }

        public List<string> GetSelectedRows(string id)
        {
            return new List<string>(GetTableState(id).SelectedRows);
        }

        private void DrawSearch(DataTableState state)
        {
            string current = state.FilterText ?? string.Empty;
            string next = GUILayout.TextField(current, styleManager.GetInputStyle(ControlVariant.Outline), GUILayout.ExpandWidth(true), GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale));
            if (!string.Equals(current, next, StringComparison.Ordinal))
                state.FilterText = next;
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private List<DataTableRow> FilterRows(List<DataTableRow> rows, List<DataTableColumn> columns, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return rows;

            string needle = query.Trim();
            return rows.Where(row => columns.Any(col => (row.Data.TryGetValue(col.AccessorKey, out var value) ? value?.ToString() : null)?.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }

        private void DrawTable(DataTableState state, List<DataTableColumn> columns, List<DataTableRow> rows, bool showSelection)
        {
            var headerStyle = styleManager.GetTableHeaderStyle();
            var cellStyle = styleManager.GetTableCellStyle();

            layoutComponents.BeginHorizontalGroup();
            if (showSelection)
                GUILayout.Label(string.Empty, headerStyle, GUILayout.Width(24f * guiHelper.uiScale));
            foreach (var column in columns)
                GUILayout.Label(column.Header ?? column.Id ?? string.Empty, headerStyle, GUILayout.Width(column.Width * guiHelper.uiScale));
            layoutComponents.EndHorizontalGroup();

            foreach (var row in rows)
            {
                layoutComponents.BeginHorizontalGroup(styleManager.GetTableRowStyle());
                if (showSelection)
                {
                    bool isSelected = state.SelectedRows.Contains(row.Id);
                    bool nextSelected = GUILayout.Toggle(isSelected, string.Empty, GUILayout.Width(24f * guiHelper.uiScale));
                    if (nextSelected != isSelected)
                    {
                        if (nextSelected)
                            state.SelectedRows.Add(row.Id);
                        else
                            state.SelectedRows.Remove(row.Id);
                    }
                }

                foreach (var column in columns)
                {
                    string text = row.Data.TryGetValue(column.AccessorKey, out var value) ? value?.ToString() ?? string.Empty : string.Empty;
                    GUILayout.Label(text, cellStyle, GUILayout.Width(column.Width * guiHelper.uiScale));
                }
                layoutComponents.EndHorizontalGroup();
            }
        }

        private void DrawPagination(DataTableState state, int totalRows)
        {
            int pages = Mathf.Max(1, Mathf.CeilToInt(totalRows / (float)Mathf.Max(1, state.PageSize)));
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();
            GUILayout.Label($"Page {state.CurrentPage + 1} of {pages}", styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Prev", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Small)))
                state.CurrentPage = Mathf.Max(0, state.CurrentPage - 1);
            if (GUILayout.Button("Next", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Small)))
                state.CurrentPage = Mathf.Min(pages - 1, state.CurrentPage + 1);
            layoutComponents.EndHorizontalGroup();
        }

        private List<DataTableRow> GetPagedRows(List<DataTableRow> rows, DataTableState state)
        {
            if (rows == null || rows.Count == 0)
                return new List<DataTableRow>();

            int pageSize = Mathf.Max(1, state.PageSize);
            int pageCount = Mathf.Max(1, Mathf.CeilToInt(rows.Count / (float)pageSize));
            state.CurrentPage = Mathf.Clamp(state.CurrentPage, 0, pageCount - 1);

            return rows.Skip(state.CurrentPage * pageSize).Take(pageSize).ToList();
        }
    }
}
