using System;
using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{

    public class PointsGeometry : IPoints
    {
        public PointsGeometry(IArray<Vector3> points)
            => Points = points; 

        public IArray<Vector3> Points { get; }

        public ITransformable TransformImpl(Matrix4x4 mat)
            => DeformImpl(p => p.Transform(mat));

        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new PointsGeometry(Points.Select(f));
    }
}