using System;
using Ara3D.Collections;
using Ara3D.Math;

// https://en.wikipedia.org/wiki/Boy%27s_surface
// https://en.wikipedia.org/wiki/Roman_surface
// https://en.wikipedia.org/wiki/M%C3%B6bius_strip#Making_the_boundary_circular
// https://en.wikipedia.org/wiki/Trefoil_knot
// https://en.wikipedia.org/wiki/Figure-eight_knot_(mathematics)
// https://en.wikipedia.org/wiki/Metric_(mathematics)
// https://en.wikipedia.org/wiki/Implicit_curve
// https://en.wikipedia.org/wiki/Implicit_surface

// http://rodolphe-vaillant.fr/entry/86/implicit-surface-aka-signed-distance-field-definition
// https://github.com/EmmetOT/IsoMesh

// Octree ...
// https://en.wikipedia.org/wiki/Octree
// https://en.wikipedia.org/wiki/K-d_tree

// https://www.boristhebrave.com/2018/04/15/dual-contouring-tutorial/
// https://github.com/Lin20/isosurface

namespace Ara3D.Geometry
{
    using Polygon = PolyLine<Vector2>;

    public static class NumberExtensions
    {
        public const float TwoPi = Pi * 2f;
        public const float Pi = (float)System.Math.PI;

        public static double Degrees(this float t)
            => (t / 360f).Turns();

        public static float Turns(this float t)
            => t * TwoPi;

        public static float HalfTurns(this float t)
            => t.Turns() * 2;
    }

    public static class FunctionExtensions
    {
        public static Vector2 Circle(this float f)
            => PrimitiveFunctions.Circle(f);
    }

    public class PolyLine<T>
    {
        public PolyLine(IArray<T> points, bool closed)
            => (Points, Closed) = (points, closed);
        public bool Closed { get; }
        public IArray<T> Points; 

        public ICurve<T> Curve => throw new NotImplementedException();
        
        //https://stackoverflow.com/questions/69856578/how-to-move-along-a-bezier-curve-with-a-constant-velocity-without-a-costly-preco
        public ICurve<T> ConstantSpeedCurve => throw new NotImplementedException(); 
    }

    public static class PrimitivePolylines
    {
        public static IArray<Vector2> SquarePoints = new Vector2[] { (-0.5f, -0.5f), (0.5f, -0.5f), (0.5f, 0.5f), (-0.5f, 0.5f) }.ToIArray();

        // public static PolyLine Path 
        // Quadrilateral
    }

    public static class RegularPolygons
    {
        public static IArray<float> SampleFloats(this int n) 
            => n.Select(x => (float)x / n);

        public static PolyLine<Vector2> ToPolyLine(this IArray<Vector2> points)
            => new PolyLine<Vector2>(points, false);

        public static Polygon ToPolygon(this IArray<Vector2> points) 
            => new Polygon(points, true);
        
        public static IArray<T> Sample<T>(this Func<float, T> func, int n) 
            => n.SampleFloats().Select(func);
        
        public static Polygon Polygon(int n) 
            => PrimitiveFunctions.Circle.Sample(n).ToPolygon();
        
        public static Polygon Triangle = Polygon(3);
        public static Polygon Square = Polygon(4);
        public static Polygon Pentagon = Polygon(5);
        public static Polygon Hexagon = Polygon(6);
        public static Polygon Septagon = Polygon(7);
        public static Polygon Octagon = Polygon(8);
        public static Polygon Decagon = Polygon(10);
        public static Polygon Dodecagon = Polygon(12);
        public static Polygon Icosagon = Polygon(20);   
    }

    public static class Surfaces
    {
        public static IParametricSurface Plane = PrimitiveFunctions.Plane.ToSurface();
        public static IParametricSurface Cylinder = PrimitiveFunctions.Cylinder.ToSurface(true);
        public static IParametricSurface Sphere = PrimitiveFunctions.Sphere.ToSurface(true, true);
        public static IParametricSurface MonkeySaddle = PrimitiveFunctions.MonkeySaddle.ToSurface();
        public static IParametricSurface TorusKnot = PrimitiveFunctions.Torus.ToSurface();

        public static IParametricSurface ToSurface(this Func<Vector2, Vector3> func, bool closedX = false, bool closedY = false)
            => new ParametricSurface(func, closedX, closedY);

        public static IParametricSurface ToSurface(this Func<Vector2, float> func, bool closedX = false, bool closedY = false)
            => new ParametricSurface(uv => new Vector3(uv.X, uv.Y, func(uv)), closedX, closedY);
    }

        
    // http://paulbourke.net/geometry/mecon/

    // https://stackoverflow.com/questions/69856578/how-to-move-along-a-bezier-curve-with-a-constant-velocity-without-a-costly-preco
    // https://mathworld.wolfram.com/Dodecahedron.
    // 
    // https://github.com/prideout/par/blob/master/par_octasphere.h

    // https://prideout.net/blog/octasphere/
}
