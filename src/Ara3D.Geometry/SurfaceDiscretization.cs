using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    /// <summary>
    /// This class is used for sampling parameterized surfaces and computing quad-mesh strip indices.
    /// Note that the members are computed on demand, so be careful about over-computation. 
    /// x == column == u, y == row == v
    /// </summary>
    public class SurfaceDiscretization
    {
        public IArray<float> Xs { get; }
        public IArray<float> Ys { get; }
        public IArray2D<Vector2> Uvs { get; }
        public bool ClosedX { get; }
        public bool ClosedY { get; }
        public IArray2D<Int4> Indices { get; }

        public SurfaceDiscretization(int nColumns, int nRows, bool closedX, bool closedY)
        {
            ClosedX = closedX;
            ClosedY = closedY;
            
            var nx = nColumns + (closedX ? 0 : 1);
            var ny = nRows + (closedY ? 0 : 1);

            Xs = closedX ? nx.InterpolateExclusive() : nx.InterpolateInclusive();
            Ys = closedY ? ny.InterpolateExclusive() : ny.InterpolateInclusive();
            
            Uvs = Ys.CartesianProduct(Xs, (v, u) => new Vector2(u, v));

            Int4 QuadMeshFaceVertices(int row, int col)
            {
                var a = row * nx + col;
                var b = row * nx + (col + 1) % nx;
                var c = (row + 1) % ny * nx + (col + 1) % nx;
                var d = (row + 1) % ny * nx + col;
                return (a, b, c, d);
            }

            Indices = nRows.Range().CartesianProduct(nColumns.Range(), QuadMeshFaceVertices);
        }
    }
}