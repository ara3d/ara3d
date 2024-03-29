using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class PrimitiveSurfaces
    {
        public static ParametricSurface Sphere
            => new ParametricSurface(PrimitiveSurfaceFunctions.Sphere, true, true);

        public static ParametricSurface Torus(float r1, float r2)
            => new ParametricSurface(uv => uv.Torus(r1, r2), true, true);

        public static ParametricSurface MonkeySaddle
            => new ParametricSurface(PrimitiveSurfaceFunctions.MonkeySaddle, false, false);

        public static ParametricSurface Plane
            => new ParametricSurface(PrimitiveSurfaceFunctions.Plane, false, false);

        public static ParametricSurface Disc
            => new ParametricSurface(PrimitiveSurfaceFunctions.Disc, false, false);

        public static ParametricSurface Cylinder
            => new ParametricSurface(PrimitiveSurfaceFunctions.Cylinder, false, false);

        public static ParametricSurface ConicalSection(float r1, float r2)
            => new ParametricSurface(uv => PrimitiveSurfaceFunctions.ConicalSection(uv, r1, r2), true, false);

        public static ParametricSurface Trefoil(float r)
            => new ParametricSurface(uv => PrimitiveSurfaceFunctions.Trefoil(uv, r), true, true);
    }
}