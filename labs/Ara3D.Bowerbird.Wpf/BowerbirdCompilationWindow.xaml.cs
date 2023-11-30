using System.Windows;

namespace Ara3D.Bowerbird.Wpf
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
