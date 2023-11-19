using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Ara3D.Bowerbird.Core;
using Ara3D.Domo;

namespace Ara3D.Bowerbird.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BowerbirdDemoMainWindow : Window
    {
        public BowerBirdDemoApp App { get; } = new();
    
        public BowerbirdDemoMainWindow()
        {
            App = new BowerBirdDemoApp();
            var repo = App.Service.Repo;
            DataContext = repo;
            InitializeComponent();
            ConsoleListBox.ItemsSource = App.LogRepo.GetModels();
            App.Service.Repo.OnModelChanged(model => ModelChanged(model.Value));
            App.Service.Compile();
        }

        public void ModelChanged(BowerbirdDataModel dataModel)
        {
            TypeListBox.ItemsSource = dataModel.Types;
            DiagnosticsListBox.ItemsSource = dataModel.Diagnostics;
            FileListBox.ItemsSource = dataModel.Files;
        }
    }
}
