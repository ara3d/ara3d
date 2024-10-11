// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Ara3D.Bowerbird.Core;
using Ara3D.Bowerbird.Interfaces;
using Ara3D.Bowerbird.WinForms.Net48;
using Ara3D.IfcLoader;
using Ara3D.Utils;
using Plato.DoublePrecision;
using Plato.Geometry.Ifc;
using Plato.Geometry.Scenes;
using Plato.Geometry.WPF;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Point = System.Windows.Point;
using Vector3D = System.Windows.Media.Media3D.Vector3D;

namespace Ara3D.Viewer.Wpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            Api = new Api(this);
            InitializeComponent();
            DataContext = this;
            SetupFirstPerson();

            ServiceApp = new Services.Application();
            Options = BowerbirdOptions.CreateFromName("Ara 3D Studio");
            Host = new BowerbirdHost(ExecuteCommand);
            BowerbirdService = new BowerbirdService(Host, ServiceApp, null, Options);
            GetOrCreateWindow(BowerbirdService);

            Root = new ModelVisual3D();
            Viewport.Children.Add(Root);
        }

        public Api Api { get; }
        public ModelVisual3D Root { get; }
        public BowerbirdForm Window { get; private set; }

        public BowerbirdForm GetOrCreateWindow(IBowerbirdService service)
        {
            if (Window == null)
            {
                Window = new BowerbirdForm(service);
                Window.Text = Options.AppTitle;
                Window.FormClosing += (sender, args) =>
                {
                    Window.Hide();
                    args.Cancel = true;
                };
            }

            Window.Show();
            return Window;
        }

        public static void ExecuteCommand(IBowerbirdCommand command)
        {
            command.Execute(null);
        }

        public static BowerbirdHost Host { get; private set; }
        public static Services.Application ServiceApp { get; private set; }
        public static BowerbirdOptions Options { get; private set; }
        public static BowerbirdService BowerbirdService { get; private set; }

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
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                RaisePropertyChanged("CurrentPosition");
            }
        }

        public void LoadScene(IScene scene)
        {
            var vis = scene.ToWpf();
            Root.Children.Add(vis);
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
            
            var ifc = IfcFile.Load(file, true);
            var scene = ifc.ToScene();
            LoadScene(scene);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Root.Children.Clear();
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

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            MouseDown += MainWindow_MouseDown;
            MouseUp += MainWindow_MouseUp;
            MouseMove += MainWindow_MouseMove;
            MouseWheel += MainWindow_MouseWheel;
        
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
                ReleaseMouseCapture();
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
            Api.LoadPly(file);
        }
    }
}
