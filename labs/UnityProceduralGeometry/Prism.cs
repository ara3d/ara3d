using Ara3D.Geometry;
using Ara3D.UnityBridge;
using UnityEngine;

[ExecuteAlways]
public class Prism : ProceduralGeometryObject
{
    public CommonPolygonsEnum PolygonEnum = CommonPolygonsEnum.Triangle;
    public float Height = 1.0f;

    public override ITriMesh ComputeMesh()
        => Polygons.GetPolygon(PolygonEnum).Extrude(Height).Triangulate();
}