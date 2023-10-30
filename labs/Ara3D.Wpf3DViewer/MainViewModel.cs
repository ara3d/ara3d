// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Ara3D.Geometry;
using Ara3D.Interop.WPF;
using HelixToolkit.Wpf;

namespace Wpf3DViewer
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class MainViewModel : Observable
    {
        private const string OpenFileFilter = "3D model files (*.vim;*.g3d;*.3ds;*.obj;*.lwo;*.stl;*.ply;)|*.vim;*.g3d;*.3ds;*.obj;*.objz;*.lwo;*.stl;*.ply;|All files(*.*)|*.*";

        private const string TitleFormatString = "3D model viewer - {0}";

        private readonly IFileDialogService fileDialogService;  

        private readonly IHelixViewport3D viewport;

        private readonly Dispatcher dispatcher;

        private string currentModelPath;

        private string applicationTitle;

        private double expansion;

        private Model3D currentModel;

        public MainViewModel(IFileDialogService fds, HelixViewport3D viewport)
        {
            if (viewport == null)
            {
                throw new ArgumentNullException("viewport");
            }

            dispatcher = Dispatcher.CurrentDispatcher;
            Expansion = 1;
            fileDialogService = fds;
            this.viewport = viewport;
            FileOpenCommand = new DelegateCommand(FileOpen);
            FileExportCommand = new DelegateCommand(FileExport);
            FileExitCommand = new DelegateCommand(FileExit);
            ViewZoomExtentsCommand = new DelegateCommand(ViewZoomExtents);
            EditCopyXamlCommand = new DelegateCommand(CopyXaml);
            TorusCommand = new DelegateCommand(CreateTorus);
            ApplicationTitle = "3D Model viewer";
            Elements = new List<VisualViewModel>();
            foreach (var c in viewport.Children)
            {
                Elements.Add(new VisualViewModel(c));
            }
        }

        public string CurrentModelPath
        {
            get => currentModelPath;

            set
            {
                currentModelPath = value;
                RaisePropertyChanged("CurrentModelPath");
            }
        }

        public string ApplicationTitle
        {
            get => applicationTitle;

            set
            {
                applicationTitle = value;
                RaisePropertyChanged("ApplicationTitle");
            }
        }

        public List<VisualViewModel> Elements { get; set; }

        public double Expansion
        {
            get => expansion;

            set
            {
                if (!expansion.Equals(value))
                {
                    expansion = value;
                    RaisePropertyChanged("Expansion");
                }
            }
        }

        public Model3D CurrentModel
        {
            get => currentModel;

            set
            {
                currentModel = value;
                RaisePropertyChanged("CurrentModel");
            }
        }

        public ICommand FileOpenCommand { get; set; }

        public ICommand FileExportCommand { get; set; }

        public ICommand FileExitCommand { get; set; }

        public ICommand HelpAboutCommand { get; set; }

        public ICommand ViewZoomExtentsCommand { get; set; }

        public ICommand EditCopyXamlCommand { get; set; }

        public ICommand TorusCommand { get; set; }

        private static void FileExit()
        {
            Application.Current.Shutdown();
        }

        private void FileExport()
        {
            var path = fileDialogService.SaveFileDialog(null, null, Exporters.Filter, ".png");
            if (path == null)
            {
                return;
            }

            viewport.Export(path);
        }

        public void CreateTorus()
        {
            var torus = Primitives.TorusMesh(20, 5, 100, 20);
            var material = new DiffuseMaterial(Brushes.ForestGreen);
            CurrentModel = torus.ToMeshGeometry3D().ToWpfModel3D(material);
        }

        private void CopyXaml()
        {
            if (CurrentModel is null)
                return;
            var rd = XamlExporter.WrapInResourceDictionary(CurrentModel);
            Clipboard.SetText(XamlHelper.GetXaml(rd));
        }

        private void ViewZoomExtents()
        {
            viewport.ZoomExtents(500);
        }

        private void FileOpen()
        {
            CurrentModelPath = fileDialogService.OpenFileDialog("models", null, OpenFileFilter, ".3ds");
            CurrentModel = ModelLoader.Load(CurrentModelPath);
            ApplicationTitle = string.Format(TitleFormatString, CurrentModelPath);
            viewport.ZoomExtents(0);
        }
   }
}