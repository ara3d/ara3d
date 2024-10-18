using System.Windows.Forms;
using Ara3D.Bowerbird.Interfaces;
using Ara3D.Viewer.Api;
using Plato.DoublePrecision;
using Plato.Geometry;
using Plato.Geometry.Graphics;
using Plato.Geometry.Scenes;

namespace Ara3D.Viewer.Scripts
{
    /// <summary>
    /// Shows a message box with the text: "Hello world!"
    /// </summary>
    public class HelloWorld : IBowerbirdCommand
    {
        public string Name => "Hello World!";

        public void Execute(object _)
        {
            MessageBox.Show(Name);
        }
    }

    public class MakeTorus : IBowerbirdCommand
    {
        public string Name => "Make Torus";

        public void Execute(object arg)
        {
            var api = (IApi)arg;
            var torus = Shapes.Torus(20);
            var scene = new Scene(); 
            scene.Root.AddMesh(torus, null, Colors.BurlyWood);
            api.Scene = scene;
        }
    }
    public class MakeCylinder : IBowerbirdCommand
    {
        public string Name => "Make Cylinder";

        public void Execute(object arg)
        {
            var api = (IApi)arg;
            var torus = Shapes.Cylinder(20);
            var scene = new Scene();
            scene.Root.AddMesh(torus, null, Colors.BlanchedAlmond);
            api.Scene = scene;
        }
    }
}