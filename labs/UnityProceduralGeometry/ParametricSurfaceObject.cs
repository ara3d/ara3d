using Ara3D.Geometry;
using Ara3D.UnityBridge;
using UnityEngine;
using Vector2 = Ara3D.Mathematics.Vector2;
using Vector3 = Ara3D.Mathematics.Vector3;

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
        HalfCone,
        Plane,
        Torus,
        Disc,
        MonkeySaddle,
        Trefoil,

    }

    public GeometryType Type = GeometryType.Sphere;

    public bool ClosedU = false;
    public bool ClosedV = false;
    [Range(-2,2)]
    public float URange = 1;
    [Range(-2, 2)]
    public float VRange = 1f;
    [Range(-2, 2)]
    public float UOffset = 0;
    [Range(-2, 2)]
    public float VOffset = 0;

    public Vector3 Eval(Vector2 uv)
    {
        uv *= (URange, VRange);
        uv += (UOffset, VOffset);

        switch (Type)
        {
            case GeometryType.Sphere:
                return PrimitiveSurfaceFunctions.Sphere(uv);

            case GeometryType.Cylinder:
                return PrimitiveSurfaceFunctions.Cylinder(uv);
            
            case GeometryType.Cone:
                return PrimitiveSurfaceFunctions.ConicalSection(uv, 1, 0);

            case GeometryType.HalfCone:
                return PrimitiveSurfaceFunctions.ConicalSection(uv, 1, 0.5f);

            case GeometryType.Plane:
                return uv.Plane();

            case GeometryType.Torus:
                return PrimitiveSurfaceFunctions.Torus(uv, 1, 0.2f);
            
            case GeometryType.Disc:
                return PrimitiveSurfaceFunctions.Disc(uv);

            case GeometryType.MonkeySaddle:
                return PrimitiveSurfaceFunctions.MonkeySaddle(uv);

            case GeometryType.Trefoil:
                return PrimitiveSurfaceFunctions.Trefoil(uv, 0.2f);

            default:
                return uv;
        }
    }
    public override ITriMesh ComputeMesh()
    {
        return new ParametricSurface(uv => Eval(uv), ClosedU, ClosedV)
            .Tesselate(USegments, VSegments)
            .Triangulate();
    }
}