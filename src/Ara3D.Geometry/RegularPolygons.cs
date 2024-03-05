using System;
using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class Sampling
    {
        public static IArray<float> SampleFloats(this int n)
            => n.Select(x => (float)x / n);

        public static IArray<T> Sample<T>(this Func<float, T> func, int n)
            => n.SampleFloats().Select(func);
    }

    public static class RegularPolygons
    {
        public static PolyLine<Vector2> ToPolyLine(this IArray<Vector2> points)
            => new PolyLine<Vector2>(points, false);

        public static PolyLine<Vector2> ToPolygon(this IArray<Vector2> points) 
            => new PolyLine<Vector2>(points, true);
        
        public static PolyLine<Vector2> Polygon(int n) 
            => PrimitiveFunctions.Circle.Sample(n).ToPolygon();
        
        public static PolyLine<Vector2> Triangle = Polygon(3);
        public static PolyLine<Vector2> Square = Polygon(4);
        public static PolyLine<Vector2> Pentagon = Polygon(5);
        public static PolyLine<Vector2> Hexagon = Polygon(6);
        public static PolyLine<Vector2> Septagon = Polygon(7);
        public static PolyLine<Vector2> Octagon = Polygon(8);
        public static PolyLine<Vector2> Decagon = Polygon(10);
        public static PolyLine<Vector2> Dodecagon = Polygon(12);
        public static PolyLine<Vector2> Icosagon = Polygon(20);   
    }
}