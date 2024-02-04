using System;
using Ara3D.Math;

namespace Identification
{
    public static class DifferenceEngine
    {
        public static double GeometryChangeTolerance = 0.02;

        public static DifferenceRecord CreateDifferenceRecord(this Entity e1, Entity e2)
        {
            var g1 = e1.Geometries[0];
            var g2 = e2.Geometries[1];

            var m1 = ObjMesh.Load(g1.FileName);
            var m2 = ObjMesh.Load(g2.FileName);


            var d = m1.Distance(m2);

            var r = new DifferenceRecord();
            r.NewCenterPoint = m2.Box.Center;
            r.OldCenterPoint = m1.Box.Center;
            r.NewDimensions = m2.Box.Extent;
            r.OldDimensions = m1.Box.Extent;

            r.GeometryObjFile = g2.FileName;

            r.CenterPointChange = r.OldCenterPoint - r.NewCenterPoint;
            r.DidCenterPointChange = r.CenterPointChange.Length() >= GeometryChangeTolerance;
            r.DimensionsChange = r.OldDimensions - r.NewDimensions;
            r.DidDimensionsChange = r.CenterPointChange.Length() >= GeometryChangeTolerance;
            r.GeometryChangeDelta = d;

            r.DidGeometryChange = d > GeometryChangeTolerance;

            return r;
        }

        public static double Distance(this ObjMesh m1, ObjMesh m2)
            => Math.Max(m1.MaxDistanceFrom(m2), m2.MaxDistanceFrom(m1));


    }
}