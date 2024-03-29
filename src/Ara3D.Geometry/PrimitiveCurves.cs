namespace Ara3D.Geometry
{
    public static class PrimitiveCurves
    {
        public static Curve2D Circle
            => new Curve2D(PrimitiveCurveFunctions2D.Circle, true);
    }
}