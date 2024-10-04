// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Ara3D.IfcLoader;
using Ara3D.Speckle.Data;
using Ara3D.Utils;
using HelixToolkit.Wpf;
using Objects.Other;
using Objects.Utils;
using Plato.Geometry.Ifc;
using Plato.Geometry.Scenes;
using Plato.Geometry.Speckle;
using Plato.Geometry.WPF;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models;
using Speckle.Core.Transports;
using Color = System.Windows.Media.Color;
using Mesh = Objects.Geometry.Mesh;
using SpeckleObject = Ara3D.Speckle.Data.SpeckleObject;

namespace Ara3D.Speckle.Wpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        private Point3D _currentPosition;

        public Point3D CurrentPosition
        {
            get
            {
                return this._currentPosition;
            }
            set
            {
                this._currentPosition = value;
                RaisePropertyChanged("CurrentPosition");
            }
        }

        private void CreateBoxMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var diceMesh = new MeshBuilder();
            diceMesh.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
            for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
            for (int k = 0; k < 2; k++)
            {
                var points = new List<Point3D>();
                diceMesh.ChamferCorner(new Point3D(i - 0.5, j - 0.5, k - 0.5), 0.1, 1e-6, points);
                //foreach (var p in points)
                //    b.ChamferCorner(p, 0.03);
            }

            var model = new ModelVisual3D { Content = new GeometryModel3D { Geometry = diceMesh.ToMesh(), Material = Materials.Green } };

            this.Viewport.Children.Add(model);
        }

        private async void OpenRemoteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1);

            // The id of the stream to work with (we're assuming it already exists in your default account's server)
            //var streamId = "51d8c73c9d";
            //var streamId = "97529188be"; 

            // Advanced Revit Project 
            //var streamId = "8f64180899";

            // Default Speckl architecture 
            var streamId = "3247bdd4ee";
                
            // The name of the branch we'll receive data from.
            var branchName = "base design";

            // Get default account on this machine
            // If you don't have Speckle Manager installed download it from https://speckle-releases.netlify.app
            var defaultAccount = AccountManager.GetDefaultAccount();

            // Or get all the accounts and manually choose the one you want
            // var accounts = AccountManager.GetAccounts();
            // var defaultAccount = accounts.ToList().FirstOrDefault();

            if (defaultAccount == null)
                throw new Exception("Could not find a default account. You may need to install and run the Speckle Manager");

            // Authenticate using the account
            using var client = new Client(defaultAccount);

            // Get the main branch with it's latest commit reference
            var branch = await client.BranchGet(streamId, branchName, 1);

            // Get the id of the object referenced in the commit
            var hash = branch.commits.items[0].referencedObject;

            // Create the server transport for the specified stream.
            var transport = new ServerTransport(defaultAccount, streamId);

            // Receive the object
            var root = await Operations.Receive(hash, transport);

            ConvertToMeshes(root);
        }

        public Color GetRenderMaterialColor(RenderMaterial material)
        {   
            if (material == null)
                return Colors.DarkSlateGray;
            return Color.FromArgb((byte)(material.opacity * 255), material.diffuseColor.R, material.diffuseColor.G,
                material.diffuseColor.B);
        }
        
        public void AddMeshes(SpeckleObject native)
        {
            var parentVisual = new SortingVisual3D();
            Viewport.Children.Add(parentVisual);

            var grp = new Model3DGroup();
            CreateModels(grp, native);
            grp.Freeze();
            var vis = new ModelVisual3D() { Content = grp };
            parentVisual.Children.Add(vis);
        }

        /*
        public void CreateModels(Visual3DCollection parent, SpeckleObject native)
        {
            var m = native.Mesh;
            if (m != null)
            {
                var meshGeo = ToMeshGeometry3D(m);
                var color = GetRenderMaterialColor(native.Material);
                var mat = new DiffuseMaterial(new SolidColorBrush(color));
                var model = new GeometryModel3D(meshGeo, mat);
                var tmp = new ModelVisual3D() { Content = model };
                parent.Add(tmp);
                parent = tmp.Children;
            }

            foreach (var child in native.Children)
                CreateModels(parent, child);
        }
        */

        public void CreateModels(Model3DGroup grp, SpeckleObject native)
        {
            var m = native.Mesh;
            if (m != null)
            {
                var meshGeo = ToMeshGeometry3D(m);
                var color = GetRenderMaterialColor(native.Material);
                var mat = new DiffuseMaterial(new SolidColorBrush(color));
                mat.Freeze();
                var model = new GeometryModel3D(meshGeo, mat);
                model.Freeze();
                grp.Children.Add(model);
            }

            var children = native.Children.ToList();
            if (children.Count == 0)
                return;
            var childGrp = new Model3DGroup();
            foreach (var child in children)
                CreateModels(childGrp, child);
            grp.Children.Add(childGrp);
            childGrp.Freeze();
        }

        public Point3D GetPoint(Mesh mesh, int i)
        {
            var ix = i * 3 + 0;
            var iy = i * 3 + 1;
            var iz = i * 3 + 2;
            var scale = 0.01;
            return new Point3D(
                mesh.vertices[ix] * scale, 
                mesh.vertices[iy] * scale, 
                mesh.vertices[iz] * scale);
        }

        public MeshGeometry3D ToMeshGeometry3D(Mesh mesh)
        {
            mesh.TriangulateMesh();

            var mb = new MeshBuilder();
            for (var i = 0; i < mesh.faces.Count; i += 4)
            {
                var faceSize = mesh.faces[i];
                if (faceSize != 3) throw new Exception("Forgot to triangulate the mesh");
                var v0 = GetPoint(mesh, mesh.faces[i + 1]);
                var v1 = GetPoint(mesh, mesh.faces[i + 2]);
                var v2 = GetPoint(mesh, mesh.faces[i + 3]);
                mb.AddTriangle(v0, v1, v2);
            }

            return mb.ToMesh();
        }

        private void OpenLocalMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filePath = @"C:\Users\cdigg\AppData\Local\Temp\Speckle";

            var localSql = new SQLiteTransport(filePath);
            var root = Operations.Receive("f0fa094f0c24fba78171bd57816f3797", localSql).Result;
            localSql.Dispose();
            var newObject = root.ToSpeckleObject();
            var scene = newObject.ToScene();
            LoadScene(scene);
        }

        public void LoadScene(IScene scene)
        {
            var vis = scene.ToWpf();
            Viewport.Children.Add(vis);
        }

        public void ConvertToMeshes(Base root)
        {
            var newObject = root.ToSpeckleObject();
            AddMeshes(newObject);
        }

        private void OpenIfcMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var testFilesFolder = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "..", "IFC-toolkit", "test-files");
            var file = testFilesFolder.RelativeFile("AC20-FZK-Haus.ifc");
            var ifc = IfcFile.Load(file, true);
            var scene = ifc.ToScene();
            LoadScene(scene);
        }
    }
}