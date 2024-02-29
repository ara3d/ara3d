using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    /// <summary>
    /// A deformable shape, can accept an arbitrary function from R3 -> R3 and
    /// produce a new shape.
    /// </summary>
    public interface IDeformable : ITransformable
    {
        IDeformable DeformImpl(Func<Vector3, Vector3> f);
    }
}