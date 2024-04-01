using Ara3D.Geometry;
using Ara3D.UnityBridge;
using UnityEngine;

[ExecuteAlways]
public class PlatonicSolid : ProceduralGeometryObject
{
    public PlatonicSolidsEnum Type = PlatonicSolidsEnum.Tetrahedron;
    public bool FlipFaces = false;
    public bool Faceted = true;

    public override ITriMesh ComputeMesh()
    {
        var r = PlatonicSolids.ToMesh(Type);
        if (FlipFaces)
            r = r.FlipFaces();
        if (Faceted) 
            r = r.Faceted();
        return r;
    }
}