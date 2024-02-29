using Ara3D.Geometry;
using Ara3D.UnityBridge;

[UnityEngine.ExecuteAlways]
public class SamplePrimitiveGeometry : ProceduralGeometryObject
{
    public enum GeometryType
    {
        Square,
        Cube,
        Tetrahedron,
        Octahedron,
    }

    public GeometryType Type = GeometryType.Square;

    public override ITriMesh ComputeMesh()
    {
        switch (Type)
        {
            case GeometryType.Square: return Primitives.Square.Triangulate();
            case GeometryType.Cube: return Primitives.Cube.Triangulate();
            case GeometryType.Tetrahedron: return Primitives.Tetrahedron;
            case GeometryType.Octahedron: return Primitives.Octahedron;
        }
        return null;
    }
}
