using Ara3D;
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

    public override IMesh ComputeMesh()
    {
        switch (Type)
        {
            case GeometryType.Square: return Primitives.Square;
            case GeometryType.Cube: return Primitives.Cube;
            case GeometryType.Tetrahedron: return Primitives.Tetrahedron;
            case GeometryType.Octahedron: return Primitives.Octahedron;
        }
        return null;
    }
}
