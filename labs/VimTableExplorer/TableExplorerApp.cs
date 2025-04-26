using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Ara3D.Serialization.VIM;
using Ara3D.Utils.Wpf;

namespace VimTableExplorer
{
    // This is a minimal application that loads a VIM and creates a standards WPF data-grid from it.
    // This tool can be useful for examining the content of a VIM file visually, and it demonstrates
    // how closely the entity data of a VIM maps to a relational database.
    public class TableExplorerApp : Application
    {
        // The entry point of a WPF application 
        [STAThread]
        public static void Main()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2) throw new Exception("Pass file name on command-line");
            var file = args[1];
            var vim = VimSerializer.Deserialize(file);
            var window = CreateTableExplorerWindow(vim);
            window.Show();
            new TableExplorerApp().Run();
        }

        public static DataGrid CreateDataGrid(VimTable table, RowViewModel model)
        {
            var r = new DataGrid
            {
                ItemsSource = table,
                RowHeight = 20,
                ColumnWidth = 100,
                EnableColumnVirtualization = true,
                EnableRowVirtualization = true,
            };
            r.SelectedCellsChanged += (sender, args) => SelectedCellsChanged(sender, args, model);
            return r;
        }

        private static void SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e, RowViewModel model)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                var cells = dataGrid.SelectedCells;
                if (cells.Count > 0)
                {
                    var cell = cells[0];
                    if (cell.Item is VimRow row)
                    {
                        model.Row = row;
                    }
                    else
                    {
                        model.Row = null;
                    }
                }
            }
        }

        public static Window CreateTableExplorerWindow(SerializableDocument vim)
        {
            var doc = new VimDocument(vim);
            var rowViewModel = new RowViewModel();

            var tabControl = new TabControl
            {
                // Create a set of tabs, each one containing a WPF data-grid bound to one of the tables
                ItemsSource = doc.Tables.OrderBy(table => table.Name)
                    .Select(table => new TabItem
                {
                    Header = table.Name,
                    Content = CreateDataGrid(table, rowViewModel)
                })
            };

            var treeView = new TreeView();

            var template = new HierarchicalDataTemplate(typeof(ITreeViewModel));
            template.ItemsSource = new Binding("Items");
            treeView.ItemTemplate = template;

            var titleFactory = new FrameworkElementFactory(typeof(TextBlock));
            titleFactory.SetBinding(TextBlock.TextProperty, new Binding("Title"));
            template.VisualTree = titleFactory;

            var grid = WpfHelpers.TwoColumnGridWithSplitter(treeView, tabControl, 400);

            treeView.Items.Add(rowViewModel); 

            return new Window
            {
                Title = $"VIM Table Explorer - {vim.FileName}",
                Content = grid
            };
        }
    }
}
