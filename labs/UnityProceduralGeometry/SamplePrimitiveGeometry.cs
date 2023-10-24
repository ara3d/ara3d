using Ara3D;

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

    public override IGeometry ComputeMesh()
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
