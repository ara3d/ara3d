using Ara3D.Math;

namespace Ara3D.Geometry.ToRemove
{
    public interface IBounded
    {
        AABox Bounds { get; }
    }

    public static class Bounded
    {
        public static AABox UpdateBounds(this IBounded self, AABox box)
            => box.Merge(self.Bounds);
    }
}
