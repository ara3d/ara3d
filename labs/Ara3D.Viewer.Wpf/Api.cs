using System;
using System.IO;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Ara3D.IfcLoader;
using Plato.Geometry.Ifc;
using Plato.Geometry.IO;
using Plato.Geometry.Scenes;
using Plato.Geometry.WPF;

namespace Ara3D.Viewer.Wpf
{
    public class Api
    {
        public Api(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }
        public Scene _scene;

        public Scene Scene
        {
            get => _scene;
            set => UpdateScene(value);
        }

        public void UpdateScene(Scene scene)
        {
            _scene = scene;
            MainWindow.LoadScene(scene);
        }

        public void LoadIfc(string filePath)
        {
            var ifc = IfcFile.Load(filePath, true);
            var scene = ifc.ToScene();
            MainWindow.LoadScene(scene);
        }

        public void SaveViewAsImage(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            switch (ext)
            {
                case ".png":
                    MainWindow.Viewport.Viewport.RenderToPng(filePath);
                    break;
                case ".jpg":
                case ".jpeg":
                    MainWindow.Viewport.Viewport.RenderToJpg(filePath);
                    break;
                case ".bmp":
                    MainWindow.Viewport.Viewport.RenderToBmp(filePath);
                    break;
                default:
                    throw new Exception("Invalid file extension. Supported extensions are .png, .jpg, .bmp");
            }
        }

        public void LoadPly(string file)
        {
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

            var model = new GeometryModel3D
            {
                Geometry = geometry,
                Material = material,
                Transform = rotateTransform
            };
            var visual = new ModelVisual3D { Content = model };
            MainWindow.Root.Children.Add(visual);
        }
    }
}
