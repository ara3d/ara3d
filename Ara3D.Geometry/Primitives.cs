
using System;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    // TODO: plane, cylinder, cone, ruled face, 
    public static class Primitives
    {
        public static TriMesh TriMesh(this IArray<Vector3> vertices, IArray<int> indices = null)
            => TriMesh(vertices, (indices ?? vertices.Indices()).SelectTriplets((a, b, c) => new Int3(a, b, c)));

        public static TriMesh TriMesh(this IArray<Vector3> vertices, IArray<Int3> indices)
            => new TriMesh(vertices, indices);

        public static QuadMesh QuadMesh(this IArray<Vector3> vertices, IArray<int> indices = null)
            => QuadMesh(vertices, (indices ?? vertices.Indices()).SelectQuartets((a, b, c, d) => new Int4(a, b, c, d)));

        public static QuadMesh QuadMesh(this IArray<Vector3> vertices, IArray<Int4> indices)
            => new QuadMesh(vertices, indices);

        public static TriMesh Triangle(Vector3 a, Vector3 b, Vector3 c)
            => TriMesh(new[] { a, b, c }.ToIArray());

        public static QuadMesh Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
            => QuadMesh(new[] { a, b, c, d }.ToIArray());
        
        public static QuadMesh Cube
            => SquareMesh.Translate(-Vector3.UnitZ / 2).Vertices.Concat(
                SquareMesh.Translate(Vector3.UnitZ / 2).Vertices)
            .QuadMesh(new Int4[] {
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

        public static readonly IMesh Tetrahedron
            = TriMesh(new Vector3[]
            {
                (1f, 0.0f, -1f / Sqrt2), 
                (-1f, 0.0f, -1f / Sqrt2), 
                (0.0f, 1f, 1f / Sqrt2), 
                (0.0f, -1f, 1f / Sqrt2)
            }.ToIArray(),
            LinqArray.Create<Int3>((0, 1, 2), (1, 0, 3), (0, 2, 3), (1, 3, 2)));

        public static readonly IMesh SquareMesh
            = LinqArray.Create<Vector2>(
                (-0.5f, -0.5f),
                (-0.5f, 0.5f),
                (0.5f, 0.5f),
                (0.5f, -0.5f)).Select(x => x.ToVector3()).QuadMesh();

        public static readonly IMesh Octahedron
            = SquareMesh.Vertices.Append(Vector3.UnitZ / 2, -Vector3.UnitZ / 2).Normalize().TriMesh(
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

        public static QuadMesh QuadMesh(Func<Vector2, Vector3> f, int uSegs, int vSegs, bool closedX = true, bool closedY = true)
            => new ParametricSurface(f, closedX, closedY).Tesselate(uSegs, vSegs);

        public static ParametricSurface Torus(float radius, float tubeRadius)
            => new ParametricSurface(uv => TorusFunction(uv, radius, tubeRadius), true, true);

        public static QuadMesh TorusMesh(float radius, float tubeRadius, int uSegs, int vSegs)
            => Torus(radius, tubeRadius).Tesselate(uSegs, vSegs);

        // see: https://github.com/mrdoob/three.js/blob/9ef27d1af7809fa4d9943f8d4c4644e365ab6d2d/src/geometries/SphereBufferGeometry.js#L76
        public static Vector3 SphereFunction(Vector2 uv, float radius)
            => new Vector3(
                (float)(-radius * System.Math.Cos(uv.X * Constants.TwoPi) * System.Math.Sin(uv.Y * Constants.Pi)),
                (float)(radius * System.Math.Cos(uv.Y * Constants.Pi)),
                (float)(radius * System.Math.Sin(uv.X * Constants.TwoPi) * System.Math.Sin(uv.Y * Constants.Pi)));

        public static ParametricSurface Sphere(float radius)
            => new ParametricSurface(uv => SphereFunction(uv, radius), true, true);

        public static QuadMesh SphereMesh(float radius, int segs)
            => Sphere(radius).Tesselate(segs, segs);

        // TODO: Icosahedron, Dodecahedron,

        /// <summary>
        /// Returns a collection of circular points.
        /// </summary>
        public static IArray<Vector2> CirclePoints(float radius, int numPoints)
            => CirclePoints(numPoints).Select(x => x * radius);

        public static IArray<Vector2> CirclePoints(int numPoints)
            => numPoints.Select(i => CirclePoint(i, numPoints));

        public static Vector2 CirclePoint(int i, int numPoints)
            => new Vector2((i * (Constants.TwoPi / numPoints)).Cos(), (i * (Constants.TwoPi / numPoints)).Sin());
    }
}
