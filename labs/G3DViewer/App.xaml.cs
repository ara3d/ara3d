using System.Windows;

namespace G3DViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow window;

        App()
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            window = new MainWindow();

            if (e.Args.Length == 1)
            {
                window.OpenFile(e.Args[0]);
            }

            window.Show();
        }
    }
}
