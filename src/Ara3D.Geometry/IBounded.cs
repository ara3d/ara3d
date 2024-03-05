using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public interface IBounded
    {
        AABox Bounds { get; }
    }

    public static class Bounded
    {
        public static AABox UpdateBounds(this IBounded self, AABox box)
            => box.Merge(self.Bounds);

        public static AABox GetBounds<T>(this IArray<T> items) where T : IBounded
            => items.Any() 
                ? items.Aggregate(items.First().Bounds, (box,item) => UpdateBounds(item, box)) 
                : AABox.Empty;
    }
}