using Ara3D.Geometry;
using Ara3D.Mathematics;
using Ara3D.UnityBridge;

[UnityEngine.ExecuteAlways]
public class SampleTorus : ProceduralGeometryObject
{
    [UnityEngine.Range(0, 100)] public float Radius = 10f;
    [UnityEngine.Range(0, 100)] public float Tube = 1f;
    [UnityEngine.Range(1, 200)] public int USegments = 10;
    [UnityEngine.Range(1, 200)] public int VSegments = 24;

    // https://github.com/mrdoob/three.js/blob/master/src/geometries/TorusGeometry.js
    public static Vector3 TorusFunction(Vector2 uv, float radius, float tube)
    {
        uv *= Constants.TwoPi;
        return new Vector3(
            (radius + tube * uv.Y.Cos()) * uv.X.Cos(),
            (radius + tube * uv.Y.Cos()) * uv.X.Sin(),
            tube * uv.Y.Sin());
    }

    public override ITriMesh ComputeMesh() 
        => new ParametricSurface(uv => TorusFunction(uv, Radius, Tube), true, true)
            .Tesselate(USegments, VSegments)
            .Triangulate();    
}
