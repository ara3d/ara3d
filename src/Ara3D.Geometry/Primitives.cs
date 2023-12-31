﻿using System;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    // TODO: plane, cylinder, cone, ruled face, 
    public static class Primitives
    {
        public static TriMesh ToTriMesh(this IArray<Vector3> vertices, IArray<int> indices = null)
            => ToTriMesh(vertices, (indices ?? vertices.Indices()).SelectTriplets((a, b, c) => new Int3(a, b, c)));

        public static TriMesh ToTriMesh(this IArray<Vector3> vertices, IArray<Int3> indices)
            => new TriMesh(vertices, indices);

        public static QuadMesh ToQuadMesh(this IArray<Vector3> vertices, IArray<int> indices = null)
            => ToQuadMesh(vertices, (indices ?? vertices.Indices()).SelectQuartets((a, b, c, d) => new Int4(a, b, c, d)));

        public static QuadMesh ToQuadMesh(this IArray<Vector3> vertices, IArray<Int4> indices)
            => new QuadMesh(vertices, indices);

        public static TriMesh Triangle(Vector3 a, Vector3 b, Vector3 c)
            => ToTriMesh(new[] { a, b, c }.ToIArray());

        public static QuadMesh Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
            => ToQuadMesh(new[] { a, b, c, d }.ToIArray());
        
        public static QuadMesh Cube
            => SquareMesh.Translate(-Vector3.UnitZ / 2).Vertices.Concat(
                SquareMesh.Translate(Vector3.UnitZ / 2).Vertices)
            .ToQuadMesh(new Int4[] {
                    (0, 1, 2, 3), 
                    (1, 5, 6, 2), 
                    (7, 6, 5, 4),
                    (4, 0, 3, 7),
                    (4, 5, 1, 0),
                    (3, 2, 6, 7),
                }.ToIArray());

        public static IMesh ToIMesh(this AABox box)
            => Cube.Scale(box.Extent).Translate(box.Center);

        public static float Sqrt2 = 2.0f.Sqrt();
        public static float Sqrt3 = 3.0f.Sqrt();
        public static float HalfSqrt3 = Sqrt3 / 2;

        public static readonly IMesh Tetrahedron
            = ToTriMesh(new Vector3[]
            {
                (1f, 0.0f, -1f / Sqrt2), 
                (-1f, 0.0f, -1f / Sqrt2), 
                (0.0f, 1f, 1f / Sqrt2), 
                (0.0f, -1f, 1f / Sqrt2)
            }.ToIArray(),
            LinqArray.Create<Int3>((0, 1, 2), (1, 0, 3), (0, 2, 3), (1, 3, 2)));

        public static IArray<Vector2> TrianglePoints
            = LinqArray.Create<Vector2>(
                (-0.5f, -HalfSqrt3),
                (0f, HalfSqrt3),
                (0.5f, -HalfSqrt3));

        public static TriMesh TriangleMesh = TrianglePoints.To3D().ToTriMesh();

        public static IArray<Vector2> SquarePoints
            = LinqArray.Create<Vector2>(
                (-0.5f, -0.5f),
                (-0.5f, 0.5f),
                (0.5f, 0.5f),
                (0.5f, -0.5f));

        public static IArray<Vector3> To3D(this IArray<Vector2> self)
            => self.Select(x => x.ToVector3());

        public static readonly QuadMesh SquareMesh = SquarePoints.To3D().ToQuadMesh();

        public static readonly TriMesh Octahedron
            = SquareMesh.Vertices.Append(Vector3.UnitZ / 2, -Vector3.UnitZ / 2).Normalize().ToTriMesh(
                LinqArray.Create<Int3>(
                    (0, 1, 4), (1, 2, 4), (2, 3, 4),
                    (3, 2, 5), (2, 1, 5), (1, 0, 5)));

        // see: https://github.com/mrdoob/three.js/blob/9ef27d1af7809fa4d9943f8d4c4644e365ab6d2d/src/geometries/TorusBufferGeometry.js#L52
        public static Vector3 TorusFunction(Vector2 uv, float radius, float tube)
        {
            uv *= Constants.TwoPi;
            return new Vector3(
                (radius + tube * uv.Y.Cos()) * uv.X.Cos(),
                (radius + tube * uv.Y.Cos()) * uv.X.Sin(),
                tube * uv.Y.Sin());
        }

        public static ParametricSurface Torus(float radius, float tubeRadius)
            => new ParametricSurface(uv => TorusFunction(uv, radius, tubeRadius), false, false);

        public static Surface TorusMesh(float radius, float tubeRadius, int uSegs, int vSegs)
            => Torus(radius, tubeRadius).Tesselate(uSegs, vSegs);

        // see: https://github.com/mrdoob/three.js/blob/9ef27d1af7809fa4d9943f8d4c4644e365ab6d2d/src/geometries/SphereBufferGeometry.js#L76
        public static Vector3 SphereFunction(Vector2 uv, float radius)
            => new Vector3(
                (float)(-radius * System.Math.Cos(uv.X * Constants.TwoPi) * System.Math.Sin(uv.Y * Constants.Pi)),
                (float)(radius * System.Math.Cos(uv.Y * Constants.Pi)),
                (float)(radius * System.Math.Sin(uv.X * Constants.TwoPi) * System.Math.Sin(uv.Y * Constants.Pi)));

        public static ParametricSurface Sphere(float radius)
            => new ParametricSurface(uv => SphereFunction(uv, radius), true, true);

        public static Surface SphereMesh(float radius, int segs)
            => Sphere(radius).Tesselate(segs, segs);

        /// <summary>
        /// Returns a collection of circular points.
        /// </summary>
        public static IArray<Vector2> CirclePoints(float radius, int numPoints)
            => CirclePoints(numPoints).Select(x => x * radius);

        public static IArray<Vector2> CirclePoints(int numPoints)
            => numPoints.Select(i => CirclePoint(i, numPoints));

        public static Vector2 CirclePoint(int i, int numPoints)
            => new Vector2((i * (Constants.TwoPi / numPoints)).Cos(), (i * (Constants.TwoPi / numPoints)).Sin());

        private static readonly float _t = (float)((1.0 + System.Math.Sqrt(5.0)) / 2.0);
        private static readonly float _rt = _t.Inverse();

        // https://mathworld.wolfram.com/RegularDodecahedron.html
        // https://github.com/mrdoob/three.js/blob/master/src/geometries/DodecahedronGeometry.js

        public static IArray<Vector3> DodecahedronPoints = LinqArray.Create<Vector3>(
             // (±1, ±1, ±1)
            (-1, -1, -1), (-1, -1, 1),
            (-1, 1, -1), (-1, 1, 1),
            (1, -1, -1), (1, -1, 1),
            (1, 1, -1), (1, 1, 1),

            // (0, ±1/φ, ±φ)
            (0, -_rt, -_t), (0, -_rt, _t),
            (0, _rt, -_t), (0, _rt, _t),

            // (±1/φ, ±φ, 0)
            (-_rt, -_t, 0), (-_rt, _t, 0),
            (_rt, -_t, 0), (_rt, _t, 0),

            // (±φ, 0, ±1/φ)
            (-_t, 0, -_rt), (_t, 0, -_rt),
            (-_t, 0, _rt), (_t, 0, _rt));

        public static TriMesh Dodecahedron = DodecahedronPoints.ToTriMesh(LinqArray.Create<Int3>(
            (3, 11, 7), (3, 7, 15), (3, 15, 13),
            (7, 19, 17), (7, 17, 6), (7, 6, 15),
            (17, 4, 8), (17, 8, 10), (17, 10, 6),
            (8, 0, 16), (8, 16, 2), (8, 2, 10),
            (0, 12, 1), (0, 1, 18), (0, 18, 16),
            (6, 10, 2), (6, 2, 13), (6, 13, 15),
            (2, 16, 18), (2, 18, 3), (2, 3, 13),
            (18, 1, 9), (18, 9, 11), (18, 11, 3),
            (4, 14, 12), (4, 12, 0), (4, 0, 8),
            (11, 9, 5), (11, 5, 19), (11, 19, 7),
            (19, 5, 14), (19, 14, 4), (19, 4, 17),
            (1, 12, 14), (1, 14, 5), (1, 5, 9)));

        public static IArray<Vector3> IcosahedronPoints =
            LinqArray.Create<Vector3>(
                (-1f, _t, 0.0f),
                (1f, _t, 0.0f),
                (-1f, -_t, 0.0f),
                (1f, -_t, 0.0f),
                (0.0f, -1f, _t),
                (0.0f, 1f, _t),
                (0.0f, -1f, -_t),
                (0.0f, 1f, -_t),
                (_t, 0.0f, -1f),
                (_t, 0.0f, 1f),
                (-_t, 0.0f, -1f),
                (-_t, 0.0f, 1f)).Select(v => v.Normalize());
        
        public static TriMesh Icosahedron =
            IcosahedronPoints.ToTriMesh(
                LinqArray.Create<Int3>((0, 11, 5), (0, 5, 1), (0, 1, 7), (0, 7, 10), (0, 10, 11), 
                    (1, 5, 9), (5, 11, 4), (11, 10, 2), (10, 7, 6), (7, 1, 8), 
                    (3, 9, 4), (3, 4, 2), (3, 2, 6), (3, 6, 8), (3, 8, 9), 
                    (4, 9, 5), (2, 4, 11), (6, 2, 10), (8, 6, 7), (9, 8, 1)));

        public static Surface Cylinder(int usegs, int vsegs)
            => PrimitiveFunctions.Cylinder.ToSurface(true, false).Tesselate(usegs, vsegs);

        public static Curve<Vector3> TorusKnotCurve(int p, int q)
            => new Curve<Vector3>(TorusKnotFunction(p,q), true);

        // https://en.wikipedia.org/wiki/Trefoil_knot
        public static Func<float, Vector3> TrefoilKnotFunction => TorusKnotFunction(2, 3);

        // https://en.wikipedia.org/wiki/Torus_knot
        public static Func<float, Vector3> TorusKnotFunction(int p, int q)
            => t =>
            {
                var r = (q * t.Turns()).Cos() + 2;
                var x = r * (p * t.Turns()).Cos();
                var y = r * (p * t.Turns()).Sin();
                var z = -(q * t.Turns()).Sin();
                return (x, y, z);
            };
    }
}
