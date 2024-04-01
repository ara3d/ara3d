using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class Deformable
    {
        public static T Deform<T>(this T self, Func<Vector3, Vector3> f) where T : IDeformable
            => (T)self.DeformImpl(f);

        public static T Deform<T>(this T self, Func<Vector2, Vector2> f) where T : IDeformable2D
            => (T)self.DeformImpl(f);

        public static T Translate<T>(this T self, Vector2 vector) where T : IDeformable2D
            => (T)self.DeformImpl(x => x + vector);

        public static T Scale<T>(this T self, Vector2 vector) where T : IDeformable2D
            => (T)self.DeformImpl(x => x * vector);
    }
}