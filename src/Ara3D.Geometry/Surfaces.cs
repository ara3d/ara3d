using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class Surfaces
    {
        public static IParametricSurface Plane = PrimitiveFunctions.Plane.ToSurface();
        public static IParametricSurface Cylinder = PrimitiveFunctions.Cylinder.ToSurface(true);
        public static IParametricSurface Sphere = PrimitiveFunctions.Sphere.ToSurface(true, true);
        public static IParametricSurface MonkeySaddle = PrimitiveFunctions.MonkeySaddle.ToSurface();
        public static IParametricSurface TorusKnot = ToSurface(uv => PrimitiveFunctions.Torus(uv, 8, 1));

        public static IParametricSurface ToSurface(this Func<Vector2, Vector3> func, bool closedX = false, bool closedY = false)
            => new ParametricSurface(func, closedX, closedY);

        public static IParametricSurface ToSurface(this Func<Vector2, float> func, bool closedX = false, bool closedY = false)
            => new ParametricSurface(uv => new Vector3(uv.X, uv.Y, func(uv)), closedX, closedY);
    }
}