using System.Collections.Generic;
using System.Net;
using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Utils;

namespace Ara3D.Geometry
{
    public interface IPolygon : IPolyLine2D
    { }

    public class Polygon : PolyLine2D, IPolygon
    {
        public Polygon(IArray<Vector2> points)
            : base(points, true)
        { }
    }

    public static class Polygons
    {
        public static IPolygon ToPolygon(this IArray<Vector2> points) 
            => new Polygon(points);

        public static IPolygon RegularPolygon(int n)
            => Curves2D.Circle.Sample(n).ToPolygon();
        
        public static IPolygon Triangle = RegularPolygon(3);
        public static IPolygon Square = RegularPolygon(4);
        public static IPolygon Pentagon = RegularPolygon(5);
        public static IPolygon Hexagon = RegularPolygon(6);
        public static IPolygon Septagon = RegularPolygon(7);
        public static IPolygon Octagon = RegularPolygon(8);
        public static IPolygon Decagon = RegularPolygon(10);
        public static IPolygon Dodecagon = RegularPolygon(12);
        public static IPolygon Icosagon = RegularPolygon(20);

        public static IPolygon RegularStarPolygon(int p, int q)
            => Curves2D.Circle.Sample(p).SelectEveryNth(q).ToPolygon();

        public static IPolyLine2D StarFigure(int p, int q)
        {
            Verifier.Assert(p > 1);
            Verifier.Assert(q > 1);
            if (p.RelativelyPrime(q))
                return RegularStarPolygon(p, q);
            var points = Curves2D.Circle.Sample(p);
            var r = new List<Vector2>();
            var connected = new bool[p];
            for (var i = 0; i < p; ++i)
            {
                if (connected[i])
                    break;
                var j = i;
                while (j != i)
                {
                    r.Add(points[j]);
                    j = (j + q) % p;
                    if (connected[j])
                        break;
                    connected[j] = true;
                }
            }
            return new PolyLine2D(r.ToIArray(), false);
        }

        // https://en.wikipedia.org/wiki/Pentagram
        public static IPolygon Pentagram 
            = RegularStarPolygon(5, 2);

        public static IPolyLine2D Hexagram
            = StarFigure(6, 2);

        // https://en.wikipedia.org/wiki/Heptagram
        public static IPolygon Heptagram2Step
            = RegularStarPolygon(7, 2);

        // https://en.wikipedia.org/wiki/Heptagram
        public static IPolygon Heptagram3Step
            = RegularStarPolygon(7, 3);

        // https://en.wikipedia.org/wiki/Octagram
        public static IPolygon Octagram
            = RegularStarPolygon(8, 3);

        // https://en.wikipedia.org/wiki/Enneagram_(geometry)
        public static IPolygon Enneagram2Step
            = RegularStarPolygon(9, 2);

        // https://en.wikipedia.org/wiki/Enneagram_(geometry)
        public static IPolygon Enneagram4Step
            = RegularStarPolygon(9, 4);

        // https://en.wikipedia.org/wiki/Decagram_(geometry)
        public static IPolygon Decagram
            => RegularStarPolygon(10, 3);

        public static IArray<T> SelectEveryNth<T>(this IArray<T> self, int n)
            => self.MapIndices(i => (i * n).Mod(self.Count));
    }
}