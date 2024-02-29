using System;
using System.Windows;

namespace Ara3D.UnityHost.Wpf
{
    /// <summary>
    /// https://stackoverflow.com/questions/44059182/embed-unity3d-app-inside-wpf-application
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Activated += MainWindow_Activated; ;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            Host.Init();
        }
    }
}
