using System.Windows;
using Ara3D.Bowerbird.Wpf;

namespace Ara3D.Bowerbird.Revit
{
    /// <summary>
    /// Interaction logic for BowerbirdCompilationWindow.xaml
    /// </summary>
    public partial class BowerbirdCompilationWindow : Window
    {
        public BowerbirdWindowViewModel ViewModel { get; }

        public BowerbirdCompilationWindow()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
