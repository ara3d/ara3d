using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
