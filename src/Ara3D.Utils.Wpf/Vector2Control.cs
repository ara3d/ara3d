using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Ara3D.Math;

namespace Ara3D.Utils.Wpf
{
    public class Vector2Control : MathControl<Vector2>
    {
        public Vector2Control()
        {
            var grid = new UniformGrid
            {
                Rows = 1,
                Columns = 2
            };
            grid.Children.Add(CreateFloatControl("X"));
            grid.Children.Add(CreateFloatControl("Y"));
            Content = grid;
        }

        public float X { get => Value.X; set => Value = Value.SetX(value); }
        public float Y { get => Value.Y; set => Value = Value.SetY(value); }

        public static Vector2Control CreateBound(object source, string propName, BindingMode mode = BindingMode.TwoWay)
            => BindTo(new Vector2Control(), source, propName, mode);

    }
}