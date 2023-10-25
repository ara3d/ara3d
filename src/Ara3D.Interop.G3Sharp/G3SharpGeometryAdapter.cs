using g3;
using IMesh = Ara3D.Geometry.IMesh;

namespace Ara3D
{
    public class Ara3DReducer
    {
        public IMesh Source { get; }
        public IMesh Result { get; }

        public Ara3DReducer(IMesh source, int vertexCount, bool project = false)
        {
            Source = source;
            var dmesh = Source.ToG3Sharp();
            var reducer = new Reducer(dmesh);

            reducer.SetExternalConstraints(new MeshConstraints());
            MeshConstraintUtil.FixAllBoundaryEdges(reducer.Constraints, dmesh);

            // reducer.ProjectionMode = Reducer.TargetProjectionMode.
            
            reducer.PreserveBoundaryShape = true;
            if (project)
            {
                var aabbTree = dmesh.AABBTree();
                var projectionTarget = new MeshProjectionTarget(dmesh, aabbTree);
                reducer.SetProjectionTarget(projectionTarget);
            }

            Result = reducer.Reduce(vertexCount, true).ToAra3D();
        }
    }
}
