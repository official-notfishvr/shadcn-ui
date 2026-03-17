using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Table : BaseComponent
    {
        private const float CheckboxWidth = 26f;
        private const float MinColumnWidth = 60f;
        private const float RowHeight = 32f;

        private int _resizingColumn = -1;
        private float _resizeStartX;
        private float _resizeStartWidth;
        private string _resizeTableId;

        public Table(GUIHelper helper)
            : base(helper) { }

        public void DrawTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: false, selectable: false, paginated: false, searchable: false, resizable: false);
        }

        public void DrawTable(string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            DrawTable(
                new TableConfig
                {
                    ColumnHeaders = headers,
                    Rows = data,
                    Variant = variant,
                    Size = size,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void DrawTable(Rect rect, string[] headers, string[,] data, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            DrawRectTable(
                new TableConfig
                {
                    Rect = rect,
                    ColumnHeaders = headers,
                    Rows = data,
                    Variant = variant,
                    Size = size,
                }
            );
        }

        public void SortableTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: true, selectable: false, paginated: false, searchable: false, resizable: false);
        }

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumnIndices, ref bool[] sortAscending, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSortChanged = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                SortColumnIndices = sortColumnIndices,
                SortAscending = sortAscending,
                OnSortChanged = onSortChanged,
                Variant = variant,
                Size = size,
                LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
            };

            SortableTable(config);

            sortColumnIndices = config.SortColumnIndices;
            sortAscending = config.SortAscending;
        }

        public void SelectableTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: false, selectable: true, paginated: false, searchable: false, resizable: false);
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int, bool> onSelectionChanged = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                SelectedRowFlags = selectedRows,
                OnSelectionChanged = onSelectionChanged,
                Variant = variant,
                Size = size,
                LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
            };

            SelectableTable(config);

            selectedRows = config.SelectedRowFlags;
        }

        public void PaginatedTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: false, selectable: false, paginated: true, searchable: false, resizable: false);
        }

        public void PaginatedTable(string[] headers, string[,] data, ref int page, int pageSize, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                CurrentPage = page,
                PageSize = pageSize,
                OnPageChanged = onPageChange,
                Variant = variant,
                Size = size,
                LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
            };

            PaginatedTable(config);
            page = config.CurrentPage;
        }

        public void SearchableTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: false, selectable: false, paginated: false, searchable: true, resizable: false);
        }

        public void SearchableTable(string[] headers, string[,] data, ref string query, ref string[,] filtered, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                SearchText = query,
                FilteredRows = filtered,
                OnSearchChanged = onSearch,
                Variant = variant,
                Size = size,
                LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
            };

            SearchableTable(config);

            query = config.SearchText;
            filtered = config.FilteredRows;
        }

        public void ResizableTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: false, selectable: false, paginated: false, searchable: false, resizable: true);
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] colWidths, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                Rows = data,
                ColumnWidths = colWidths,
                Variant = variant,
                Size = size,
                LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
            };

            ResizableTable(config);
            colWidths = config.ColumnWidths;
        }

        public void CustomTable(TableConfig config)
        {
            if (!IsValidTable(config))
                return;
            DrawTableCore(config, sortable: false, selectable: false, paginated: false, searchable: false, resizable: false, custom: true);
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, params GUILayoutOption[] options)
        {
            var config = new TableConfig
            {
                ColumnHeaders = headers,
                ObjectRows = data,
                CellRenderer = cellRenderer,
                Variant = variant,
                Size = size,
                LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
            };

            CustomTable(config);
        }

        private void DrawRectTable(TableConfig config)
        {
            if (!config.Rect.HasValue)
            {
                DrawTable(config);
                return;
            }

            var r = config.Rect.Value;
            var rect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            GUILayout.BeginArea(rect);
            DrawTableCore(config, sortable: false, selectable: false, paginated: false, searchable: false, resizable: false);
            GUILayout.EndArea();
        }

        private void DrawTableCore(TableConfig config, bool sortable, bool selectable, bool paginated, bool searchable, bool resizable, bool custom = false)
        {
            var headers = config.ColumnHeaders ?? Array.Empty<string>();
            var rows = config.Rows;
            var objRows = config.ObjectRows;
            var useObjects = objRows != null;

            var rowCount = objRows?.GetLength(0) ?? rows?.GetLength(0) ?? 0;
            var colCount = Mathf.Max(headers.Length, objRows?.GetLength(1) ?? rows?.GetLength(1) ?? 0);

            if (colCount == 0)
                return;

            var tableStyle = styleManager?.GetTableStyle(config.Variant, config.Size) ?? GUI.skin.box;
            var headerStyle = styleManager?.GetTableHeaderStyle(config.Variant, config.Size) ?? GUI.skin.label;
            var cellStyle = styleManager?.GetTableCellStyle(config.Variant, config.Size) ?? GUI.skin.label;
            UnityHelpers.GUIStyle rowStyle = styleManager?.GetTableRowStyle(config.Variant, config.Size) ?? GUIStyle.none;
            var altRowStyle = new UnityHelpers.GUIStyle(rowStyle);

            if (styleManager?.Textures?.TableRowAlternate != null)
                altRowStyle.normal.background = styleManager.Textures.TableRowAlternate;

            if (searchable)
                DrawSearchBar(config);

            var rowIndices = BuildRowIndex(rowCount, rows, objRows, config.SearchText, useObjects);
            ApplySort(config, rowIndices, rows, objRows, useObjects);

            var paged = ApplyPagination(config, rowIndices, paginated, out int totalPages);

            var widths = ResolveColumnWidths(config, headers, rows, objRows, colCount);

            layoutComponents.BeginVerticalGroup(tableStyle, config.LayoutOptions);
            DrawHeaderRow(config, headers, colCount, widths, sortable, selectable, resizable);

            for (int displayRow = 0; displayRow < paged.Count; displayRow++)
            {
                int rowIndex = paged[displayRow];
                var style = displayRow % 2 == 0 ? rowStyle : altRowStyle;
                DrawRow(config, style, cellStyle, rowIndex, colCount, widths, selectable, useObjects);
            }

            layoutComponents.EndVerticalGroup();

            if (paginated)
                DrawPagination(config, totalPages);
        }

        private void DrawSearchBar(TableConfig config)
        {
            layoutComponents.BeginHorizontalGroup();
            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small) ?? GUI.skin.label;
            UnityHelpers.Label("Search", labelStyle);

            var inputStyle = styleManager?.GetInputStyle(ControlVariant.Outline, ControlSize.Small) ?? GUI.skin.textField;
            var query = config.SearchText ?? string.Empty;
            var newQuery = UnityHelpers.TextField(query, inputStyle, GUILayout.Width(200f * guiHelper.uiScale));

            if (!string.Equals(newQuery, query, StringComparison.Ordinal))
            {
                config.SearchText = newQuery;
                config.OnSearchChanged?.Invoke(newQuery);
            }

            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private void DrawHeaderRow(TableConfig config, string[] headers, int colCount, float[] widths, bool sortable, bool selectable, bool resizable)
        {
            layoutComponents.BeginHorizontalGroup();

            if (selectable)
            {
                GUILayoutUtility.GetRect(CheckboxWidth * guiHelper.uiScale, RowHeight * guiHelper.uiScale, GUILayout.Width(CheckboxWidth * guiHelper.uiScale), GUILayout.Height(RowHeight * guiHelper.uiScale));
            }

            for (int col = 0; col < colCount; col++)
            {
                var header = col < headers.Length ? headers[col] : $"Col {col + 1}";
                var width = widths[col] * guiHelper.uiScale;
                var options = new[] { GUILayout.Width(width), GUILayout.Height(RowHeight * guiHelper.uiScale) };
                var content = new GUIContent(GetSortedHeaderLabel(config, col, header, sortable));

                Rect rect = GUILayoutUtility.GetRect(content, styleManager?.GetTableHeaderStyle(config.Variant, config.Size) ?? GUI.skin.label, options);
                var clicked = sortable && GUI.Button(rect, content, styleManager?.GetTableHeaderStyle(config.Variant, config.Size) ?? GUI.skin.label);

                if (!sortable)
                    GUI.Label(rect, content, styleManager?.GetTableHeaderStyle(config.Variant, config.Size) ?? GUI.skin.label);

                if (clicked)
                    ToggleSort(config, col);

                if (resizable)
                    HandleResize(rect, config, widths, col);
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawRow(TableConfig config, UnityHelpers.GUIStyle rowStyle, UnityHelpers.GUIStyle cellStyle, int rowIndex, int colCount, float[] widths, bool selectable, bool useObjects)
        {
            layoutComponents.BeginHorizontalGroup(rowStyle);

            if (selectable)
                DrawSelectionCell(config, rowIndex, rowStyle);

            for (int col = 0; col < colCount; col++)
            {
                var width = widths[col] * guiHelper.uiScale;
                layoutComponents.BeginVerticalGroup(GUILayout.Width(width));

                if (config.CellRenderer != null)
                {
                    object value = null;
                    if (config.ObjectRows != null && rowIndex < config.ObjectRows.GetLength(0) && col < config.ObjectRows.GetLength(1))
                        value = config.ObjectRows[rowIndex, col];
                    else if (config.Rows != null && rowIndex < config.Rows.GetLength(0) && col < config.Rows.GetLength(1))
                        value = config.Rows[rowIndex, col];
                    config.CellRenderer.Invoke(value, rowIndex, col);
                }
                else
                {
                    var text = ResolveCellText(config, rowIndex, col);
                    UnityHelpers.Label(text, cellStyle);
                }

                layoutComponents.EndVerticalGroup();
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawSelectionCell(TableConfig config, int rowIndex, GUIStyle rowStyle)
        {
            var rowCount = config.Rows?.GetLength(0) ?? config.ObjectRows?.GetLength(0) ?? 0;
            var flags = config.SelectedRowFlags ?? new bool[rowCount];
            if (flags.Length != rowCount)
                flags = new bool[rowCount];

            config.SelectedRowFlags = flags;

            var checkboxStyle = styleManager?.GetCheckboxStyle(ControlVariant.Default, ControlSize.Small) ?? GUI.skin.toggle;
            var rect = GUILayoutUtility.GetRect(CheckboxWidth * guiHelper.uiScale, RowHeight * guiHelper.uiScale, GUILayout.Width(CheckboxWidth * guiHelper.uiScale), GUILayout.Height(RowHeight * guiHelper.uiScale));
            var current = rowIndex < flags.Length && flags[rowIndex];
            var next = GUI.Toggle(rect, current, GUIContent.none, checkboxStyle);
            if (next != current && rowIndex < flags.Length)
            {
                flags[rowIndex] = next;
                config.OnSelectionChanged?.Invoke(rowIndex, next);
            }
        }

        private float[] ResolveColumnWidths(TableConfig config, string[] headers, string[,] rows, object[,] objRows, int colCount)
        {
            if (config.ColumnWidths == null || config.ColumnWidths.Length != colCount)
                config.ColumnWidths = new float[colCount];

            var widths = config.ColumnWidths;

            for (int col = 0; col < colCount; col++)
            {
                if (widths[col] <= 0f)
                {
                    var header = col < headers.Length ? headers[col] : $"Col {col + 1}";
                    var width = MeasureTextWidth(header, styleManager?.GetTableHeaderStyle(config.Variant, config.Size) ?? GUI.skin.label);
                    int rowCount = rows?.GetLength(0) ?? objRows?.GetLength(0) ?? 0;
                    for (int row = 0; row < Mathf.Min(rowCount, 25); row++)
                    {
                        var text = ResolveCellText(config, row, col);
                        width = Mathf.Max(width, MeasureTextWidth(text, styleManager?.GetTableCellStyle(config.Variant, config.Size) ?? GUI.skin.label));
                    }
                    widths[col] = Mathf.Max(MinColumnWidth, width + DesignTokens.Spacing.LG);
                }
            }

            return widths;
        }

        private float MeasureTextWidth(string text, GUIStyle style)
        {
            var content = new GUIContent(text ?? string.Empty);
            return style.CalcSize(content).x;
        }

        private List<int> BuildRowIndex(int rowCount, string[,] rows, object[,] objRows, string query, bool useObjects)
        {
            var indices = new List<int>(rowCount);
            for (int i = 0; i < rowCount; i++)
                indices.Add(i);

            if (string.IsNullOrEmpty(query))
                return indices;

            var filtered = new List<int>();
            var lower = query.ToLowerInvariant();

            for (int i = 0; i < rowCount; i++)
            {
                var match = false;
                var colCount = useObjects ? (objRows?.GetLength(1) ?? 0) : (rows?.GetLength(1) ?? 0);
                for (int col = 0; col < colCount; col++)
                {
                    var text = ResolveCellText(useObjects ? objRows : rows, i, col);
                    if (!string.IsNullOrEmpty(text) && text.ToLowerInvariant().Contains(lower))
                    {
                        match = true;
                        break;
                    }
                }
                if (match)
                    filtered.Add(i);
            }

            return filtered;
        }

        private void ApplySort(TableConfig config, List<int> rowIndices, string[,] rows, object[,] objRows, bool useObjects)
        {
            var sortColumns = ResolveSortColumns(config, config.ColumnHeaders?.Length ?? 0);
            if (sortColumns.Count == 0)
                return;

            rowIndices.Sort(
                (left, right) =>
                {
                    foreach (var (col, asc) in sortColumns)
                    {
                        var a = ResolveCellText(useObjects ? objRows : rows, left, col);
                        var b = ResolveCellText(useObjects ? objRows : rows, right, col);
                        var cmp = string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
                        if (cmp != 0)
                            return asc ? cmp : -cmp;
                    }

                    return 0;
                }
            );
        }

        private List<int> ApplyPagination(TableConfig config, List<int> rowIndices, bool paginated, out int totalPages)
        {
            totalPages = 1;
            if (!paginated || config.PageSize <= 0)
                return rowIndices;

            var pageSize = Mathf.Max(1, config.PageSize);
            totalPages = Mathf.Max(1, Mathf.CeilToInt(rowIndices.Count / (float)pageSize));
            config.CurrentPage = Mathf.Clamp(config.CurrentPage, 0, totalPages - 1);

            var start = config.CurrentPage * pageSize;
            var end = Mathf.Min(start + pageSize, rowIndices.Count);

            return rowIndices.GetRange(start, end - start);
        }

        private void DrawPagination(TableConfig config, int totalPages)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            layoutComponents.BeginHorizontalGroup();

            var buttonStyle = styleManager?.GetButtonStyle(ControlVariant.Secondary, ControlSize.Small) ?? GUI.skin.button;
            var prev = UnityHelpers.Button("Prev", buttonStyle, GUILayout.ExpandWidth(false));
            var next = UnityHelpers.Button("Next", buttonStyle, GUILayout.ExpandWidth(false));

            var labelStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small) ?? GUI.skin.label;
            GUILayout.FlexibleSpace();
            UnityHelpers.Label($"Page {config.CurrentPage + 1}/{Mathf.Max(1, totalPages)}", labelStyle);
            GUILayout.FlexibleSpace();

            if (prev && config.CurrentPage > 0)
            {
                config.CurrentPage--;
                config.OnPageChanged?.Invoke(config.CurrentPage);
            }

            if (next && config.CurrentPage < totalPages - 1)
            {
                config.CurrentPage++;
                config.OnPageChanged?.Invoke(config.CurrentPage);
            }

            layoutComponents.EndHorizontalGroup();
        }

        private string ResolveCellText(TableConfig config, int row, int col)
        {
            if (config.ObjectRows != null)
                return ResolveCellText(config.ObjectRows, row, col);
            return ResolveCellText(config.Rows, row, col);
        }

        private string ResolveCellText(string[,] rows, int row, int col)
        {
            if (rows == null)
                return string.Empty;
            if (row < 0 || row >= rows.GetLength(0) || col < 0 || col >= rows.GetLength(1))
                return string.Empty;
            return rows[row, col] ?? string.Empty;
        }

        private string ResolveCellText(object[,] rows, int row, int col)
        {
            if (rows == null)
                return string.Empty;
            if (row < 0 || row >= rows.GetLength(0) || col < 0 || col >= rows.GetLength(1))
                return string.Empty;
            return rows[row, col]?.ToString() ?? string.Empty;
        }

        private bool IsValidTable(TableConfig config)
        {
            if (config == null)
                return false;

            if (config.ColumnHeaders == null || config.ColumnHeaders.Length == 0)
                return false;

            if (config.Rows == null && config.ObjectRows == null)
                return false;

            return true;
        }

        private List<(int col, bool asc)> ResolveSortColumns(TableConfig config, int headerCount)
        {
            var result = new List<(int, bool)>();
            if (config.SortColumnIndices == null || config.SortColumnIndices.Length == 0)
                return result;

            if (config.SortAscending == null || config.SortAscending.Length == 0)
                config.SortAscending = new bool[config.SortColumnIndices.Length];

            if (config.SortColumnIndices.Length == headerCount && config.SortAscending.Length == headerCount)
            {
                for (int i = 0; i < headerCount; i++)
                {
                    if (config.SortColumnIndices[i] == i)
                        result.Add((i, config.SortAscending[i]));
                }
            }
            else
            {
                for (int i = 0; i < config.SortColumnIndices.Length; i++)
                {
                    var col = config.SortColumnIndices[i];
                    var asc = i < config.SortAscending.Length ? config.SortAscending[i] : true;
                    if (col >= 0)
                        result.Add((col, asc));
                }
            }

            return result;
        }

        private string GetSortedHeaderLabel(TableConfig config, int col, string label, bool sortable)
        {
            if (!sortable || config.SortColumnIndices == null || config.SortAscending == null)
                return label;

            if (config.SortColumnIndices.Length == config.ColumnHeaders.Length && col < config.SortAscending.Length && config.SortColumnIndices[col] == col)
                return label + (config.SortAscending[col] ? " ↑" : " ↓");

            for (int i = 0; i < config.SortColumnIndices.Length; i++)
            {
                if (config.SortColumnIndices[i] == col)
                    return label + (i < config.SortAscending.Length && config.SortAscending[i] ? " ↑" : " ↓");
            }

            return label;
        }

        private void ToggleSort(TableConfig config, int col)
        {
            if (config.SortColumnIndices == null || config.SortAscending == null)
            {
                config.SortColumnIndices = new[] { col };
                config.SortAscending = new[] { true };
                config.OnSortChanged?.Invoke(col, true);
                return;
            }

            bool isActive = false;
            bool asc = true;

            if (config.SortColumnIndices.Length == config.ColumnHeaders.Length && col < config.SortAscending.Length)
            {
                isActive = config.SortColumnIndices[col] == col;
                asc = !config.SortAscending[col];
                for (int i = 0; i < config.SortColumnIndices.Length; i++)
                    config.SortColumnIndices[i] = -1;
                config.SortColumnIndices[col] = col;
                config.SortAscending[col] = asc;
            }
            else
            {
                for (int i = 0; i < config.SortColumnIndices.Length; i++)
                {
                    if (config.SortColumnIndices[i] == col)
                    {
                        isActive = true;
                        asc = !(i < config.SortAscending.Length && config.SortAscending[i]);
                        config.SortAscending[i] = asc;
                        break;
                    }
                }

                if (!isActive)
                {
                    config.SortColumnIndices = new[] { col };
                    config.SortAscending = new[] { true };
                    asc = true;
                }
            }

            config.OnSortChanged?.Invoke(col, asc);
        }

        private void HandleResize(Rect headerRect, TableConfig config, float[] widths, int colIndex)
        {
            var handle = new Rect(headerRect.xMax - 4f * guiHelper.uiScale, headerRect.y, 8f * guiHelper.uiScale, headerRect.height);
            var mouse = Event.current.mousePosition;
            var tableId = config.Id ?? "table";

            if (Event.current.type == EventType.MouseDown && handle.Contains(mouse))
            {
                _resizingColumn = colIndex;
                _resizeStartX = mouse.x;
                _resizeStartWidth = widths[colIndex];
                _resizeTableId = tableId;
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseDrag && _resizingColumn == colIndex && _resizeTableId == tableId)
            {
                var delta = (mouse.x - _resizeStartX) / guiHelper.uiScale;
                widths[colIndex] = Mathf.Max(MinColumnWidth, _resizeStartWidth + delta);
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseUp && _resizingColumn == colIndex && _resizeTableId == tableId)
            {
                _resizingColumn = -1;
                _resizeTableId = null;
            }
        }
    }
}
