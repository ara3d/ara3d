using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class PrimitiveSurfaceFunctions
    {
        public static Vector3 Sphere(this Vector2 uv)
            => Sphere(uv.X.Turns(), uv.Y.Turns());

        public static Vector3 Sphere(Angle u, Angle v) 
            => (-u.Cos * v.Sin, v.Cos, u.Sin * v.Sin);

        public static Vector3 Torus(this Vector2 uv, float r1, float r2)
            => Torus(uv.X.Turns(), uv.Y.Turns(), r1, r2);

        // https://en.wikipedia.org/wiki/Torus#Geometry
        public static Vector3 Torus(Angle u, Angle v, float r1, float r2)
            => ((r1 + r2 * u.Cos) * v.Cos,
                (r1 + r2 * u.Cos) * v.Sin,
                r2 * v.Sin);

        // https://en.wikipedia.org/wiki/Monkey_saddle
        public static float MonkeySaddle(this Vector2 uv)           
            => uv.X.Cube() - 3 * uv.X * uv.Y.Sqr();

        public static Vector3 Plane(this Vector2 uv)
            => (uv.X, uv.Y, 0);

        public static Vector3 Disc(this Vector2 uv)
            => uv.X.Circle3D() * uv.Y;

        public static Vector3 Cylinder(this Vector2 uv)
            => uv.X.Circle3D().SetZ(uv.Y);

        public static Vector3 ConicalSection(this Vector2 uv, float r1, float r2) 
            => (uv.X.Circle3D() * r1.Lerp(r2, uv.Y)).SetZ(uv.Y);

        public static Vector3 Trefoil(Vector2 uv, float r)
            => Trefoil(uv.X.Turns(), uv.Y.Turns(), r);

        // https://commons.wikimedia.org/wiki/File:Parametric_surface_illustration_(trefoil_knot).png
        public static Vector3 Trefoil(Angle u, Angle v, float r)
            => (r * (3 * u).Sin / (2 + v.Cos),
                r * (u.Sin + 2 * (2 * u).Sin) / (2 + (v + 1.Turns() / 3).Cos),
                r / 2 * (u.Cos - 2 * (2 * u).Cos) * (2 + v.Cos) * (2 + (v + 1.Turns() / 3).Cos) / 4);
    }
    
}