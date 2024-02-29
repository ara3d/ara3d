using Ara3D.Geometry;
using g3;

namespace Ara3D
{
    public class Ara3DReducer
    {
        public ITriMesh Source { get; }
        public ITriMesh Result { get; }

        public Ara3DReducer(ITriMesh source, int vertexCount, bool project = false)
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
