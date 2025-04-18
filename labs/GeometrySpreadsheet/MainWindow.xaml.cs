using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeometrySpreadsheet
{
    public partial class MainWindow : Window
    {
        private readonly DataTable _table;

        public MainWindow()
        {
            InitializeComponent();

            // Create a 100‑row, 26‑column table (A..Z)
            _table = new DataTable("Sheet");
            for (char c = 'A'; c <= 'Z'; c++)
                _table.Columns.Add(c.ToString(), typeof(string));

            for (int r = 0; r < 100; r++)
                _table.Rows.Add(_table.NewRow());

            // Bind to DataGrid
            Grid.ItemsSource = _table.DefaultView;

            // Build explicit columns so users can resize them
            foreach (DataColumn col in _table.Columns)
            {
                Grid.Columns.Add(new DataGridTextColumn
                {
                    Header = col.ColumnName,
                    Binding = new System.Windows.Data.Binding(col.ColumnName),
                    MinWidth = 40,
                    Width = 80,           
                    // Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                });
            }
        }

        /* ---------- NEW: number the row headers ---------- */
        private void Grid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            // Row indices are zero‑based → add 1 so users see 1, 2, 3 …
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        /* ---------- Selection → Formula bar ---------- */
        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cell = FirstSelectedCell();
            if (cell != null)
                FormulaBar.Text = cell.Value.ToString() ?? string.Empty;
        }

        /* ---------- Formula bar → grid ---------- */
        private void FormulaBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            string text = FormulaBar.Text;

            foreach (var cell in Grid.SelectedCells)
            {
                if (cell.Item is DataRowView drv &&
                    cell.Column is DataGridTextColumn col &&
                    col.Binding is System.Windows.Data.Binding b &&
                    !string.IsNullOrEmpty(b.Path.Path))
                {
                    drv.Row[b.Path.Path] = text;
                }
            }

            // keep focus on the grid after commit
            Grid.Focus();
            e.Handled = true;
        }

        /* ---------- In‑place edit commits keep formula bar in sync ---------- */
        private void Grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var cell = FirstSelectedCell();
                if (cell != null)
                    FormulaBar.Text = cell.Value.ToString() ?? string.Empty;
            }));
        }

        /* ---------- Helper ---------- */
        private DataGridCellInfo? FirstSelectedCell()
            => Grid.SelectedCells.Count > 0 ? Grid.SelectedCells[0] : (DataGridCellInfo?)null;
    }
}
