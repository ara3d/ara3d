using Ara3D.Geometry;
using Ara3D.UnityBridge;
using UnityEngine;

[ExecuteAlways]
public class ParametricSurfaceObject : ProceduralGeometryObject
{
    public int USegments = 20;
    public int VSegments = 20;

    public enum GeometryType
    {
        Sphere,
        Cylinder,
        Cone,
        Plane,
        Torus,
        Disc,
    }

    public GeometryType Type = GeometryType.Sphere;

    public bool ClosedU = false;
    public bool ClosedV = false;

    [Range(0,1)]
    public float URange = 1;
    [Range(0, 1)]
    public float VRange = 1f;

    [Range(-1, 1)]
    public float UOffset = 0;
    [Range(-1, 1)]
    public float VOffset = 0;

    public Ara3D.Mathematics.Vector3 Eval(Ara3D.Mathematics.Vector2 uv)
    {
        uv *= (URange, VRange);
        uv += (UOffset, VOffset);

        switch (Type)
        {
            case GeometryType.Sphere:
                return PrimitiveFunctions.Sphere(uv);

            case GeometryType.Cylinder:
                return PrimitiveFunctions.Cylinder(uv);
            
            case GeometryType.Cone:
                return PrimitiveFunctions.Conical(uv, 1, 0);
            
            case GeometryType.Plane:
                return uv.ToVector3();

            case GeometryType.Torus:
                return PrimitiveFunctions.Torus(uv, 5, 1);
            
            case GeometryType.Disc:
                return PrimitiveFunctions.Disc(uv);

            default:
                return uv.ToVector3();
        }
    }
    public override ITriMesh ComputeMesh()
    {
        return new ParametricSurface(Eval, ClosedU, ClosedV)
            .Tesselate(USegments, VSegments)
            .Triangulate();
    }
}