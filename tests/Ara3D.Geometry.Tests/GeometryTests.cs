using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Utils;

namespace Ara3D.Geometry.Tests
{
    public static class GeometryTests
    {
        public static DirectoryPath MoldFlowDirectoryPath =
            SourceCodeLocation.GetFolder()
                .RelativeFolder("..", "..", "..", "3d-format-shootout", "data", "files", "moldflow");

        public static IEnumerable<CommonPolygonsEnum> Polygons
            => EnumUtils.GetEnumValues<CommonPolygonsEnum>();

        public static IEnumerable<PlatonicSolidsEnum> PlatonicSolids
            => EnumUtils.GetEnumValues<PlatonicSolidsEnum>();

        public static void TestIPolyLine3D(IPolyLine3D poly)
        {
            Console.WriteLine($"Polygon 3D has {poly.Points.Count} points and is closed {poly.Closed}");
            foreach (var pt in poly.Points.Enumerate())
                Console.WriteLine(pt.ToString());
        }

        [Test, TestCaseSource(nameof(Polygons))]
        public static void TestPolygon(CommonPolygonsEnum poly)
        {
            var polygon = poly.ToPolygon();
            Console.WriteLine($"Polygon: {poly} has {polygon.Points.Count} points");
            foreach (var pt in polygon.Points.Enumerate())
                Console.WriteLine(pt.ToString());

            var scaledPolygon = polygon.Scale((2f, 2f));
            Console.WriteLine("Scaled polygon");
            foreach (var pt in scaledPolygon.Points.Enumerate())
                Console.WriteLine(pt.ToString());

            var poly3d = polygon.To3D();
            TestIPolyLine3D(poly3d);
        }

        [Test, TestCaseSource(nameof(PlatonicSolids))]
        public static void TestPlatonicSolids(PlatonicSolidsEnum pse)
        {
            var mesh = pse.ToMesh();
            TestPointsGeometry(mesh);
        }

        public static void TestGeometry(IGeometry g)
        {
            Console.WriteLine($"Geometry of type {g.GetType().Name}");
        }

        public static IEnumerable<T> CreateDeformations<T>(T g) where T : IDeformable<T>
        {
            yield return g.Translate(1, 0, 0);
            yield return g.Scale(2);
            yield return g.RotateX(90.Degrees());
        }

        public static void TestPointsGeometry(IPoints p)
        {
            TestGeometry(p);

            Console.WriteLine($"Points of type {p.GetType().Name} has {p.Points.Count} points");
            Console.WriteLine($"First 3 points are: {p.Points.TakeAtMost(5).Join(", ")}");
            Console.WriteLine($"Last 3 points are: {p.Points.TakeAtMost(5).Join(", ")}");

            var avg = p.Points.Average();
            Console.WriteLine($"Average = {avg}");

            var avgMag = p.Points.Select(p => p.Magnitude()).Average();
            Console.WriteLine($"Average magnitude = {avgMag}");

            var bounds = p.BoundingBox();
            Console.WriteLine($"Bounds = {bounds}");
            Console.WriteLine($"Center = {bounds.Center}");
            Console.WriteLine($"Extent = {bounds.Extent}");
        }

        public static IEnumerable<ITriMesh> TestMeshes()
        {
            return PlatonicSolids.Select(p => p.ToMesh());
        }

        [Test, TestCaseSource(nameof(TestMeshes))]
        public static void TestDeformations(ITriMesh mesh)
        {
            TestPointsGeometry(mesh);
            foreach (var m in CreateDeformations(mesh))
            {
                TestPointsGeometry(m);
            }
        }

        [Test, TestCaseSource(nameof(TestMeshes))]
        public static void TestMerge(ITriMesh mesh1)
        {
            var mesh2 = mesh1.Translate(1, 0, 0);
            Assert.AreEqual(mesh1.Points.Count, mesh2.Points.Count);
            Assert.AreEqual(mesh1.GetNumFaces(), mesh2.GetNumFaces());
            var meshes = new[] { mesh1, mesh2 };
            var mesh3 = meshes.Merge();
            Assert.AreEqual(mesh3.Points.Count, mesh1.Points.Count + mesh2.Points.Count);
            Assert.AreEqual(mesh3.GetNumFaces(), mesh1.GetNumFaces() + mesh2.GetNumFaces());

            TestPointsGeometry(mesh1);
            TestPointsGeometry(mesh2);
            TestPointsGeometry(mesh3);
        }

        public static void TestTriangleMesh()
        {
            yut
        }
    }
}