using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Ara3D.Domo.Sample.WpfApp
{
    public record struct Point(
        double X,
        double Y
        );

    public record struct Vector(
        double X,
        double Y,
        double Z
    );

    public record struct Line(
        Vector A,
        Vector B
    );

    public class TestViewModel : INotifyPropertyChanged
    {
        public double _x = 42;

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAggregateRepository<Point> repo1;
        private IAggregateRepository<Vector> repo2;
        private ISingletonRepository<Line> repo3;
        private ISingletonRepository<Point> repo4;
        private ISingletonRepository<Vector> repo5;

        ObservableCollection<TestViewModel> collection = new();

        public MainWindow()
        {
            InitializeComponent();

            // TODO: createa a set of repositories 

            // TODO: for each repository create a tab. 

            var mgr = new RepositoryManager();
            repo1 = mgr.AddAggregateRepository<Point>();
            repo1.Add(new Point(1, 2)).PropertyChanged += PropertyChanged;
            repo1.Add(new Point(3, 4)).PropertyChanged += PropertyChanged;
            repo2 = mgr.AddAggregateRepository<Vector>();
            repo2.Add(new Vector(1, 2, 3));
            repo2.Add(new Vector(3, 4, 5));
            repo3 = mgr.AddSingletonRepository(new Line(new(5, 5, 5), new(7, 7, 7)));
            TabControl.Items.Clear();

            repo4 = mgr.AddSingletonRepository<Point>(new(1, 2));
            repo5 = mgr.AddSingletonRepository<Vector>(new(4, 5, 6));

            repo4.OnModelChanged(x =>
                repo5.Model.Value = repo5.Model.Value with { X = x.Value.X }
            );

            repo5.OnModelChanged(x =>
                repo4.Model.Value = repo4.Model.Value with { X = x.Value.X }
            );

            /*
            {
                var tab = new TabItem { Header = "Test" };
                var grid = new DataGrid
                {
                    ItemsSource = collection
                };
                var viewModel = new TestViewModel();
                collection.Add(viewModel);
                viewModel.PropertyChanged += PropertyChanged;
                tab.Content = grid;
                TabControl.Items.Add(tab);
            }
            */
            AddTab(repo1);
            AddTab(repo2);
            AddTab(repo3);
            AddTab(repo4);
            AddTab(repo5);
        }

        public void AddTab<T>(IRepository<T> repo, string? name = null)
        {
            name ??= repo.ValueType.Name;
            var tab = new TabItem { Header = name };
            var collection = new ObservableCollection<dynamic>(repo.GetModels());
            var grid = new DataGrid
            {
                ItemsSource = collection
            };
            tab.Content = grid;
            TabControl.Items.Add(tab);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //collection[0].X = 13;
            var m = repo1.GetModels()[0];
            m.Value = new Point(9, 9);
        }

        private void PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Trace.WriteLine($"Property {e} changed");
        }
    }
}
