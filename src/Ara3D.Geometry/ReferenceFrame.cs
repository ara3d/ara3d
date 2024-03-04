using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    // https://en.wikipedia.org/wiki/Frame_of_reference
    public class ReferenceFrame
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Up;
        public readonly Vector3 Forward;

        public ReferenceFrame(Vector3 origin, Vector3 up, Vector3 forward)
            => (Origin, Up, Forward) = (origin, up, forward);
    }
}