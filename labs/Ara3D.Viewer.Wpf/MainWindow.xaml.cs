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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Ara3D.IfcLoader;
using Ara3D.Speckle.Data;
using Ara3D.Utils;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using Objects.Other;
using Objects.Structural.Loading;
using Objects.Utils;
using Plato.DoublePrecision;
using Plato.Geometry.Ifc;
using Plato.Geometry.IO;
using Plato.Geometry.Scenes;
using Plato.Geometry.Speckle;
using Plato.Geometry.WPF;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models;
using Speckle.Core.Transports;
using SQLitePCL;
using Color = System.Windows.Media.Color;
using Mesh = Objects.Geometry.Mesh;
using Quaternion = Plato.DoublePrecision.Quaternion;
using Rotation3D = Plato.DoublePrecision.Rotation3D;
using SpeckleObject = Ara3D.Speckle.Data.SpeckleObject;
using Vector3D = System.Windows.Media.Media3D.Vector3D;

namespace Ara3D.Speckle.Wpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SetupFirstPerson();
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

        private void OpenRemoteMenuItem_Click(object sender, RoutedEventArgs e)
        {
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
            var branch = client.BranchGet(streamId, branchName, 1).Result;

            // Get the id of the object referenced in the commit
            var hash = branch.commits.items[0].referencedObject;

            // Create the server transport for the specified stream.
            var transport = new ServerTransport(defaultAccount, streamId);

            // Receive the object
            var root = Operations.Receive(hash, transport).Result;
            var newObject = root.ToSpeckleObject();
            var scene = newObject.ToScene();
            transport.Dispose();
            LoadScene(scene); ;
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

        public OpenFileDialog IfcOpenFileDialog = new OpenFileDialog()
        {
            DefaultDirectory = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "..", "IFC-toolkit", "test-files").GetFullPath(),
            DefaultExt = ".ifc",
            Filter = "IFC Files (*.ifc)|*.ifc|All Files (*.*)|*.*",
            Title = "Open IFC File"
        };

        private void OpenIfcMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IfcOpenFileDialog.ShowDialog() != true)
                return;
            var file = IfcOpenFileDialog.FileName;
            
            //var testFilesFolder = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "..", "IFC-toolkit", "test-files");
            //var file = testFilesFolder.RelativeFile("AC20-FZK-Haus.ifc");
            var ifc = IfcFile.Load(file, true);
            var scene = ifc.ToScene();
            LoadScene(scene);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Viewport.Children.Clear();
        }

        //==
        
        private double _firstPersonSpeed = 20.0; // Units per second
        private double _mouseSensitivity = 0.2;
        private Point _lastMousePosition;
        private Vector2D _yawPitch;
        private bool _isFirstPersonMode = false;

        private DispatcherTimer _timer;
        private HashSet<Key> _keysPressed = new HashSet<Key>();
        private PerspectiveCamera _camera;

        private void SetupFirstPerson()
        {
            _camera = Viewport.Camera as PerspectiveCamera;

            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;
            this.MouseDown += MainWindow_MouseDown;
            this.MouseUp += MainWindow_MouseUp;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseWheel += MainWindow_MouseWheel;
        
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isFirstPersonMode)
            {
                double deltaTime = _timer.Interval.TotalSeconds;
                Vector3D movement = new Vector3D();

                if (_keysPressed.Contains(Key.W))
                    movement += _camera.LookDirection;
                if (_keysPressed.Contains(Key.S))
                    movement -= _camera.LookDirection;
                if (_keysPressed.Contains(Key.A))
                    movement -= Vector3D.CrossProduct(_camera.LookDirection, _camera.UpDirection);
                if (_keysPressed.Contains(Key.D))
                    movement += Vector3D.CrossProduct(_camera.LookDirection, _camera.UpDirection);
                if (_keysPressed.Contains(Key.E))
                    movement += _camera.UpDirection;
                if (_keysPressed.Contains(Key.Q))
                    movement -= _camera.UpDirection;

                var multiplier = (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) ? 3 : 1;
                multiplier *= (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) ? 3 : 1;

                if (movement.Length > 0)
                {
                    movement.Normalize();
                    movement *= _firstPersonSpeed * deltaTime * multiplier;
                    _camera.Position += movement;
                }
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F) // Toggle First-Person Mode
            {
                _isFirstPersonMode = !_isFirstPersonMode;
                Viewport.Cursor = _isFirstPersonMode ? Cursors.Cross : Cursors.Arrow;
            }
            else
            {
                _keysPressed.Add(e.Key);
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _keysPressed.Remove(e.Key);
        }

        private void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isFirstPersonMode)
            {
                this.ReleaseMouseCapture();
            }
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFirstPersonMode)
            {
                _lastMousePosition = e.GetPosition(this);
                _yawPitch = DirectionToYawPitch(_camera.LookDirection);                
                CaptureMouse();
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            const double _pitchLimit = 89.5;

            if (_isFirstPersonMode && e.LeftButton == MouseButtonState.Pressed)
            {                
                var delta = (e.GetPosition(this) - _lastMousePosition) * _mouseSensitivity;                
                var _yaw = _yawPitch.X - delta.X;
                var _pitch = _yawPitch.Y - delta.Y;

                // Clamp the pitch to prevent looking directly up or down
                _pitch = Math.Clamp(_pitch, -_pitchLimit, _pitchLimit);
                _camera.LookDirection = YawPitchToDirection(new Vector2D(_yaw, _pitch));
                _camera.UpDirection = new Vector3D(0, 0, 1);
            }
        }

        public static Vector3D YawPitchToDirection(Vector2D self)
        {
            var r = new Vector3D(self.Y.Degrees.Cos * self.X.Degrees.Cos,
                self.Y.Degrees.Cos * self.X.Degrees.Sin,
                self.Y.Degrees.Sin);
            r.Normalize();
            return r;
        }

        public static Vector2D DirectionToYawPitch(Vector3D dir)
        {
            dir.Normalize();
            if (dir.LengthSquared < 1e-5)
                dir = new Vector3D(0, 1, 0);

            // Compute pitch (rotation around X - axis) in degrees
            var pitch = Math.Atan2(
                dir.Z,
                Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y)
            ).ToDegrees();

            // Compute yaw (rotation around Z-axis) in degrees
            var yaw = Math.Atan2(dir.Y, dir.X).ToDegrees();

            // Normalize yaw to be within [0, 360) degrees
            yaw = (yaw + 360.0) % 360.0;

            return new Vector2D(yaw, pitch);
        }

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_isFirstPersonMode)
            {
                double delta = e.Delta > 0 ? 1 : -1;
                _firstPersonSpeed += delta;
                _firstPersonSpeed = Math.Max(1, _firstPersonSpeed); // Prevent negative speed
            }
        }

        public OpenFileDialog PlyOpenFileDialog = new OpenFileDialog()
        {
            DefaultDirectory = "C:\\Users\\cdigg\\git\\3d-format-shootout\\data\\big\\ply",
            DefaultExt = ".ply",
            Filter = "PLY Files (*.ply)|*.ply|All Files (*.*)|*.*",
            Title = "Open PLY File"
        };

        private void OpenPlyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (PlyOpenFileDialog.ShowDialog() != true)
                return;
            var file = PlyOpenFileDialog.FileName;
            var buffers = PlyImporter.LoadBuffers(file);
            var mesh = buffers.ToMesh();
            var geometry = mesh.ToWpf();
            
            var diffuse = new DiffuseMaterial(new SolidColorBrush(Colors.DarkSlateBlue));
            var specular = new SpecularMaterial(new SolidColorBrush(Colors.LightSeaGreen), 85); // Higher specular power = smaller, sharper highlights
            var emissive = new EmissiveMaterial(new SolidColorBrush(Colors.Red));

            // Create the material group
            var material = new MaterialGroup();
            material.Children.Add(diffuse); // More detailed diffuse texture
            material.Children.Add(specular); // Sharper specular highlights
            material.Children.Add(emissive); // Subtle glow or emission

            // Create a 90-degree rotation around the X-axis to convert Y-Up to Z-Up
            var axis = new Vector3D(1, 0, 0); // Rotation axis (X-axis)
            var angle = 90; // Angle in degrees
            var rotation = new AxisAngleRotation3D(axis, angle);
            var rotateTransform = new RotateTransform3D(rotation);

            var model = new GeometryModel3D { 
                Geometry = geometry, 
                Material = material, 
                Transform = rotateTransform
            }; 
            var visual = new ModelVisual3D { Content = model };
            Viewport.Children.Add(visual);
        }
    }
}
