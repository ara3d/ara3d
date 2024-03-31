using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public class ParametricSurface : Procedural<Vector2, Vector3>, IParametricSurface
    {
        public bool ClosedX { get; }
        public bool ClosedY { get; }

        public ParametricSurface(Func<Vector2, Vector3> func, bool closedX, bool closedY)
            : base(func) => (ClosedX, ClosedY) = (closedX, closedY);

        public IParametricSurface TransformInput(Func<Vector2, Vector2> f)
            => new ParametricSurface(x => Eval(f(x)), ClosedX, ClosedY);

        public ITransformable TransformImpl(Matrix4x4 mat)
            => DeformImpl(v => v.Transform(mat));
            
        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new ParametricSurface(x => f(Eval(x)), ClosedX, ClosedY);
    }
}