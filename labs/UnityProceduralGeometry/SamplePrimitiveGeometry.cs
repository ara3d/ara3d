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
            case GeometryType.Square: return Meshes.Square.Triangulate();
            case GeometryType.Cube: return Meshes.Cube.Triangulate();
            case GeometryType.Tetrahedron: return Meshes.Tetrahedron;
            case GeometryType.Octahedron: return Meshes.Octahedron;
        }
        return null;
    }
}
