using UnityEngine;
using System;

namespace Ara3D
{
    public enum Axis { 
        XAxis, YAxis, ZAxis
    }

    [ExecuteAlways]
    public class TransformDeformer : Ara3D.Deformer
    {
        public Axis Axis;
        public bool FlipDirection;
        public bool UseAxis = true;

        public TransformProperty Transform; 

        public override void Reset()
        {
            Transform = new TransformProperty();            
            base.Reset();
        }

        public override IGeometry Deform(IGeometry g)
        {
            var matrix = Transform?.Matrix ?? Matrix4x4.Identity;
            if (!UseAxis)
                return g.Deform(v => v.Transform(matrix));
            var box = g.BoundingBox();
            var min = FlipDirection ? box.Max : box.Min;
            var max = FlipDirection ? box.Min : box.Max;
            switch (Axis)
            {
                case Axis.XAxis:
                    return g.Deform(v => v.Transform(matrix), v => v.X.InverseLerp(min.X, max.X));
                case Axis.YAxis:
                    return g.Deform(v => v.Transform(matrix), v => v.Y.InverseLerp(min.Y, max.Y));
                case Axis.ZAxis:
                    return g.Deform(v => v.Transform(matrix), v => v.Z.InverseLerp(min.Z, max.Z));
            }
            throw new Exception("Invalid enumeration");
        }
    }
}