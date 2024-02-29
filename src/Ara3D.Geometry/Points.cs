using System;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface IPoints
        : IArray<Vector3>, IGeometry
    { }

    public class Points : IPoints
    {
        public Points(IArray<Vector3> points)
            => _points = points; 

        private IArray<Vector3> _points { get; }
        public IIterator<Vector3> Iterator => _points.Iterator;
        public Vector3 this[int n] => _points[n];
        public int Count => _points.Count;

        public ITransformable TransformImpl(Matrix4x4 mat)
            => DeformImpl(p => p.Transform(mat));

        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new Points(_points.Select(f));

    }
}