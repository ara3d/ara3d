using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class PrimitiveCurveFunctions2D
    {
        public static Vector2 Circle(this float t)
            => t.Turns().Circle();

        public static Vector2 Circle(this Angle t)
            => (t.Sin, t.Cos);
    }
}