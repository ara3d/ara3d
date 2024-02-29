using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public static class Deformable
    {
        public static T Deform<T>(this T self, Func<Vector3, Vector3> f) where T : IDeformable
            => (T)self.DeformImpl(f);
    }
}