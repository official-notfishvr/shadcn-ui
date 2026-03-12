using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_examples.Examples
{
    public class TablesExample : MonoBehaviour
    {
        private GUIHelper gui;
        private Rect windowRect = new Rect(50, 50, 750, 650);
        private bool showWindow = true;
        private Vector2 scroll = Vector2.zero;

        private string[] headers = new[] { "Name", "Role", "Status", "Last Active" };
        private string[,] data = new[,]
        {
            { "Alice Johnson", "Admin", "Active", "2 min ago" },
            { "Bob Smith", "User", "Away", "1 hour ago" },
            { "Carol White", "Moderator", "Active", "Just now" },
            { "David Brown", "User", "Offline", "2 days ago" },
            { "Eve Green", "User", "Active", "5 min ago" },
            { "Frank Black", "Admin", "Active", "1 min ago" },
        };

        private int[] sortColumns = new int[0];
        private bool[] sortAscending = new bool[0];
        private bool[] selectedRows = new bool[6];
        private int currentPage = 0;
        private string searchQuery = "";
        private string[,] filteredData;

        void Start()
        {
            gui = new GUIHelper();
            filteredData = (string[,])data.Clone();
        }

        void OnGUI()
        {
            if (showWindow)
            {
                windowRect = GUI.Window(3, windowRect, DrawTablesWindow, "Tables Example");
            }
            gui.DrawOverlay();
        }

        void DrawTablesWindow(int windowID)
        {
            gui.UpdateGUI(showWindow);
            if (!gui.BeginGUI())
                return;

            scroll = gui.ScrollView(
                scroll,
                () =>
                {
                    gui.BeginVerticalGroup();

                    gui.Label("Basic Table", ControlVariant.Default);
                    gui.Table(headers, data, ControlVariant.Default, ControlSize.Default);

                    gui.AddSpace(20);

                    gui.Label("Sortable Table", ControlVariant.Default);
                    gui.Table(
                        new TableConfig
                        {
                            Headers = headers,
                            Data = data,
                            SortColumns = sortColumns,
                            SortAscending = sortAscending,
                            OnSort = (col, asc) => gui.ShowToast($"Sorted by column {col} {(asc ? "ascending" : "descending")}"),
                        }
                    );

                    gui.AddSpace(20);

                    gui.Label("Selectable Table", ControlVariant.Default);
                    gui.Table(
                        new TableConfig
                        {
                            Headers = headers,
                            Data = data,
                            SelectedRows = selectedRows,
                            OnSelectionChange = (row, selected) => gui.ShowToast($"Row {row} {(selected ? "selected" : "deselected")}"),
                        }
                    );

                    gui.AddSpace(20);

                    gui.Label("Paginated Table (Page Size: 3)", ControlVariant.Default);
                    gui.Table(
                        new TableConfig
                        {
                            Headers = headers,
                            Data = data,
                            CurrentPage = currentPage,
                            PageSize = 3,
                            OnPageChange = page => gui.ShowToast($"Page {page + 1}"),
                        }
                    );

                    gui.AddSpace(20);

                    gui.Label("Searchable Table", ControlVariant.Default);
                    searchQuery = gui.Input(searchQuery, "Search...");
                    gui.Table(
                        new TableConfig
                        {
                            Headers = headers,
                            Data = filteredData,
                            SearchQuery = searchQuery,
                        }
                    );

                    gui.EndVerticalGroup();
                },
                GUILayout.Width(windowRect.width - 20),
                GUILayout.Height(windowRect.height - 60)
            );

            gui.EndGUI();
            GUI.DragWindow();
        }
    }
}
