using UnityEngine;

namespace Ara3D.UnityBridge
{
    [ExecuteAlways]
    public class Decimate : Deformer
    {
        [Range(0, 100)]
        public float Percent = 50;

        private G3SharpGeometryAdapter Adapter 
            = new G3SharpGeometryAdapter();

        public override IGeometry Deform(IGeometry g)
            => Adapter.Reduce(g, (int)(g.Vertices.Count * Percent / 100), false);
    }
}