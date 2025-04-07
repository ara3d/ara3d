using System.Windows;
using Plato;

namespace Ara3D.FloorPlanner;

public static class WindowsConverters
{
    public static Point ToWindows(this Vector2 v) => new(v.X, v.Y);
    public static Point ToWindows(this Point2D p) => new(p.X, p.Y);
    public static Rect ToWindows(this Rect2D r) => new(r.Left, r.Top, r.Width, r.Height);
    public static Size ToWindows(this Size2D s) => new(s.Width, s.Height);

    public static Point2D ToPlato(this Point p) => new((float)p.X, (float)p.Y);
    public static Vector2 ToPlato(this Vector v) => new((float)v.X, (float)v.Y);
    public static Rect2D ToPlato(this Rect r) => new(r.TopLeft.ToPlato(), r.BottomRight.ToPlato());
    public static Size2D ToPlato(this Size s) => new((float)s.Width, (float)s.Height);
}