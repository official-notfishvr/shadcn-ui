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
        private readonly Dictionary<string, bool> _columnMenus = new();

        public DataTable(GUIHelper helper)
            : base(helper) { }

        public void Render(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, ComponentAppearance appearance = null, params GUILayoutOption[] options) =>
            Render(
                new DataTableConfig
                {
                    Id = id,
                    Columns = columns ?? new List<DataTableColumn>(),
                    Rows = data ?? new List<DataTableRow>(),
                    ShowPagination = showPagination,
                    ShowSearch = showSearch,
                    ShowSelection = showSelection,
                    ShowColumnToggle = showColumnToggle,
                    EnableColumnVisibility = showColumnToggle,
                    Appearance = appearance,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );

        public void Render(DataTableConfig config)
        {
            if (config == null)
                return;
            string id = string.IsNullOrWhiteSpace(config.Id) ? "datatable" : config.Id;
            var state = GetTableState(id);
            var columns = (config.Columns ?? new List<DataTableColumn>()).Where(c => c != null).ToList();
            var data = (config.Rows ?? new List<DataTableRow>()).Where(r => r != null).ToList();
            state.PageSize = state.PageSize <= 0 ? Mathf.Max(1, config.PageSize) : state.PageSize;
            SyncColumns(state, columns);
            state.SelectedRows.RemoveAll(selected => !data.Any(row => row.Id == selected));

            var visible = VisibleColumns(state, columns);
            var filtered = Filter(data, visible, state.FilterText, config.EnableFiltering);
            if (config.EnableSorting)
                Sort(filtered, state);
            state.TotalFilteredRows = filtered.Count;
            state.TotalSelectedRows = filtered.Count(row => state.SelectedRows.Contains(row.Id));
            int pageCount = Mathf.Max(1, Mathf.CeilToInt(filtered.Count / (float)state.PageSize));
            state.CurrentPage = Mathf.Clamp(state.CurrentPage, 0, pageCount - 1);
            var pageRows = filtered.Skip(state.CurrentPage * state.PageSize).Take(state.PageSize).ToList();

            layoutComponents.BeginVerticalGroup(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
            if (config.ShowToolbar)
                DrawToolbar(id, config, state, columns);
            DrawSurface(config, state, visible, pageRows);
            if (config.ShowPagination)
                DrawFooter(config, state, pageCount);
            layoutComponents.EndVerticalGroup();
            config.OnStateChanged?.Invoke(state);
        }

        public DataTableState GetTableState(string id)
        {
            id = string.IsNullOrWhiteSpace(id) ? "datatable" : id;
            if (!_states.TryGetValue(id, out var state))
            {
                state = new DataTableState();
                _states[id] = state;
            }
            return state;
        }

        public void SetPageSize(string id, int size)
        {
            var state = GetTableState(id);
            state.PageSize = Mathf.Max(1, size);
            state.CurrentPage = 0;
        }

        public void ClearSelection(string id) => GetTableState(id).SelectedRows.Clear();

        public List<string> GetSelectedRows(string id) => new(GetTableState(id).SelectedRows);

        private void DrawToolbar(string id, DataTableConfig config, DataTableState state, List<DataTableColumn> columns)
        {
            layoutComponents.BeginHorizontalGroup();
            if (config.ShowSearch && config.EnableFiltering)
            {
                string oldText = state.FilterText ?? string.Empty;
                string newText = guiHelper.Input(oldText).Placeholder(config.FilterPlaceholder ?? "Filter...").Variant(ControlVariant.Outline).Size(ControlSize.Small).Appearance(config.Appearance).Width(240f * guiHelper.uiScale);
                if (!string.Equals(oldText, newText, StringComparison.Ordinal))
                {
                    state.FilterText = newText;
                    state.CurrentPage = 0;
                }
            }
            guiHelper.Flex();
            if (config.EnableColumnVisibility && config.ShowColumnToggle)
            {
                if (guiHelper.Button("Columns", ControlVariant.Outline, ControlSize.Small, appearance: config.Appearance))
                    _columnMenus[id] = !_columnMenus.TryGetValue(id, out bool open) || !open;
            }
            layoutComponents.EndHorizontalGroup();
            if (_columnMenus.TryGetValue(id, out bool menuOpen) && menuOpen)
                DrawColumnMenu(config, state, columns);
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private void DrawColumnMenu(DataTableConfig config, DataTableState state, List<DataTableColumn> columns)
        {
            layoutComponents.BeginHorizontalGroup(styleManager.GetDropdownMenuStyle(config.Variant, ControlSize.Small, config.Appearance));
            foreach (var column in columns)
            {
                if (!column.CanHide)
                    continue;
                string key = Key(column);
                bool current = IsVisible(state, key);
                bool next = guiHelper.Checkbox(column.Header ?? key, current, ControlVariant.Default, ControlSize.Small, appearance: config.Appearance, options: GUILayout.ExpandWidth(false));
                if (next != current)
                    state.ColumnVisibility[key] = next;
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawSurface(DataTableConfig config, DataTableState state, List<DataTableColumn> columns, List<DataTableRow> rows)
        {
            layoutComponents.BeginVerticalGroup(styleManager.GetTableStyle(config.Variant, config.Size, config.Appearance), GUILayout.ExpandHeight(false));
            layoutComponents.BeginHorizontalGroup(styleManager.GetTableHeaderStyle(config.Variant, config.Size, config.Appearance));
            if (config.ShowSelection)
                DrawHeaderSelection(config, state, rows);
            foreach (var column in columns)
                DrawHeaderCell(config, state, column);
            if (config.EnableRowActions && config.OnRowAction != null)
                guiHelper.Label(string.Empty, ControlVariant.Muted, appearance: config.Appearance, options: GUILayout.Width(72f * guiHelper.uiScale));
            layoutComponents.EndHorizontalGroup();

            if (rows.Count == 0)
            {
                layoutComponents.BeginHorizontalGroup(styleManager.GetTableRowStyle(config.Variant, config.Size, config.Appearance));
                guiHelper.MutedLabel(config.EmptyText ?? "No results.");
                layoutComponents.EndHorizontalGroup();
            }
            else
            {
                foreach (var row in rows)
                    DrawRow(config, state, columns, row);
            }
            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeaderSelection(DataTableConfig config, DataTableState state, List<DataTableRow> rows)
        {
            bool all = rows.Count > 0 && rows.All(row => state.SelectedRows.Contains(row.Id));
            bool next = guiHelper.Checkbox(string.Empty, all).Variant(ControlVariant.Default).IconSmall().FullRowClick(false).Appearance(config.Appearance).Width(32f * guiHelper.uiScale);
            if (next == all)
                return;
            foreach (var row in rows)
            {
                if (next && !state.SelectedRows.Contains(row.Id))
                    state.SelectedRows.Add(row.Id);
                if (!next)
                    state.SelectedRows.Remove(row.Id);
            }
        }

        private void DrawHeaderCell(DataTableConfig config, DataTableState state, DataTableColumn column)
        {
            string key = Key(column);
            string title = column.Header ?? key;
            if (config.EnableSorting && column.IsSortable)
            {
                bool active = state.SortColumn == key;
                if (active)
                    title += state.SortAscending ? "  ↑" : "  ↓";
                if (guiHelper.Button(title, active ? ControlVariant.Secondary : ControlVariant.Ghost, ControlSize.Small, appearance: config.Appearance, options: ColumnOptions(column)))
                {
                    if (active)
                        state.SortAscending = !state.SortAscending;
                    else
                    {
                        state.SortColumn = key;
                        state.SortAscending = true;
                    }
                    state.CurrentPage = 0;
                }
            }
            else
            {
                guiHelper.Label(title, ControlVariant.Default, appearance: config.Appearance, options: ColumnOptions(column));
            }
        }

        private void DrawRow(DataTableConfig config, DataTableState state, List<DataTableColumn> columns, DataTableRow row)
        {
            layoutComponents.BeginHorizontalGroup(styleManager.GetTableRowStyle(config.Variant, config.Size, config.Appearance));
            if (config.ShowSelection)
            {
                bool current = state.SelectedRows.Contains(row.Id);
                bool next = guiHelper.Checkbox(string.Empty, current).Variant(ControlVariant.Default).IconSmall().FullRowClick(false).Appearance(config.Appearance).Width(32f * guiHelper.uiScale);
                if (next != current)
                {
                    if (next)
                        state.SelectedRows.Add(row.Id);
                    else
                        state.SelectedRows.Remove(row.Id);
                }
            }
            foreach (var column in columns)
            {
                object value = null;
                if (row.Data != null)
                {
                    bool found = !string.IsNullOrEmpty(column.AccessorKey) && row.Data.TryGetValue(column.AccessorKey, out value);
                    if (!found)
                        found = !string.IsNullOrEmpty(column.Id) && row.Data.TryGetValue(column.Id, out value);
                    if (!found && !string.IsNullOrEmpty(column.Header))
                        row.Data.TryGetValue(column.Header, out value);
                }
                string text = column.CellRenderer == null ? value?.ToString() ?? string.Empty : column.CellRenderer(value);
                guiHelper.Label(text, ControlVariant.Default, appearance: config.Appearance, options: ColumnOptions(column));
            }
            if (config.EnableRowActions && config.OnRowAction != null && guiHelper.Button(config.RowActionLabel ?? "Open", ControlVariant.Ghost, ControlSize.Small, appearance: config.Appearance, options: GUILayout.Width(72f * guiHelper.uiScale)))
                config.OnRowAction(row);
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawFooter(DataTableConfig config, DataTableState state, int pageCount)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();
            if (config.ShowSelectedCount)
                guiHelper.MutedLabel($"{state.TotalSelectedRows} of {state.TotalFilteredRows} selected");
            guiHelper.Flex();
            guiHelper.MutedLabel($"Page {state.CurrentPage + 1} of {pageCount}");
            foreach (int configuredSize in config.PageSizeOptions ?? Array.Empty<int>())
            {
                int size = Mathf.Max(1, configuredSize);
                if (guiHelper.Button(size == state.PageSize ? $"{size} rows" : size.ToString(), size == state.PageSize ? ControlVariant.Secondary : ControlVariant.Ghost, ControlSize.Small, appearance: config.Appearance))
                {
                    state.PageSize = size;
                    state.CurrentPage = 0;
                }
            }
            if (guiHelper.Button("First", variant: ControlVariant.Outline, size: ControlSize.Small, disabled: state.CurrentPage == 0, appearance: config.Appearance))
                state.CurrentPage = 0;
            if (guiHelper.Button("Prev", variant: ControlVariant.Outline, size: ControlSize.Small, disabled: state.CurrentPage == 0, appearance: config.Appearance))
                state.CurrentPage = Mathf.Max(0, state.CurrentPage - 1);
            if (guiHelper.Button("Next", variant: ControlVariant.Outline, size: ControlSize.Small, disabled: state.CurrentPage >= pageCount - 1, appearance: config.Appearance))
                state.CurrentPage = Mathf.Min(pageCount - 1, state.CurrentPage + 1);
            if (guiHelper.Button("Last", variant: ControlVariant.Outline, size: ControlSize.Small, disabled: state.CurrentPage >= pageCount - 1, appearance: config.Appearance))
                state.CurrentPage = pageCount - 1;
            layoutComponents.EndHorizontalGroup();
        }

        private static List<DataTableRow> Filter(List<DataTableRow> rows, List<DataTableColumn> columns, string query, bool enabled)
        {
            if (!enabled || string.IsNullOrWhiteSpace(query))
                return new List<DataTableRow>(rows);
            string needle = query.Trim();
            return rows.Where(row => columns.Any(column => column.IsFilterable && row.Data != null && row.Data.TryGetValue(column.AccessorKey, out var value) && value?.ToString()?.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }

        private static void Sort(List<DataTableRow> rows, DataTableState state)
        {
            if (string.IsNullOrWhiteSpace(state.SortColumn))
                return;
            rows.Sort(
                (left, right) =>
                {
                    left.Data.TryGetValue(state.SortColumn, out var a);
                    right.Data.TryGetValue(state.SortColumn, out var b);
                    int result =
                        a == null && b == null ? 0
                        : a == null ? -1
                        : b == null ? 1
                        : a is IComparable comparable ? comparable.CompareTo(b)
                        : string.Compare(a.ToString(), b.ToString(), StringComparison.OrdinalIgnoreCase);
                    return state.SortAscending ? result : -result;
                }
            );
        }

        private static void SyncColumns(DataTableState state, List<DataTableColumn> columns)
        {
            foreach (var column in columns)
                if (!state.ColumnVisibility.ContainsKey(Key(column)))
                    state.ColumnVisibility[Key(column)] = column.IsVisible;
        }

        private static List<DataTableColumn> VisibleColumns(DataTableState state, List<DataTableColumn> columns) => columns.Where(column => IsVisible(state, Key(column))).ToList();

        private static bool IsVisible(DataTableState state, string key) => !state.ColumnVisibility.TryGetValue(key, out bool visible) || visible;

        private static string Key(DataTableColumn column) => column.AccessorKey ?? column.Id ?? column.Header ?? string.Empty;

        private GUILayoutOption[] ColumnOptions(DataTableColumn column) => column.Width > 0f ? new[] { GUILayout.Width(Mathf.Max(64f, column.Width) * guiHelper.uiScale) } : new[] { GUILayout.ExpandWidth(true) };

        protected override void OnBeforeDispose()
        {
            _states.Clear();
            _columnMenus.Clear();
        }
    }
}
