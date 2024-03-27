using System;
using Ara3D.Collections;
using Ara3D.Mathematics;

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

    // http://paulbourke.net/geometry/mecon/

    // https://stackoverflow.com/questions/69856578/how-to-move-along-a-bezier-curve-with-a-constant-velocity-without-a-costly-preco
    // https://mathworld.wolfram.com/Dodecahedron.
    // 
    // https://github.com/prideout/par/blob/master/par_octasphere.h

    // https://prideout.net/blog/octasphere/
}
