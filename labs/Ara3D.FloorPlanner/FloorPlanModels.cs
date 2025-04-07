using Plato;

namespace Ara3D.FloorPlanner
{
    public record CornerModel(Point2D Position);

    public record WallSegment(Line2D Line);

    public record WallModel(IReadOnlyList<CornerModel> Corners, bool Closed)
    {

        public IEnumerable<WallSegment> Segments {
            get
            {
                for (var i = 0; i < Corners.Count - 1; ++i)
                    yield return new WallSegment(new Line2D(Corners[i].Position.Vector2, Corners[i + 1].Position.Vector2));
                if (Corners.Count <= 1)
                    yield break;
                if (Closed)
                    yield return new WallSegment((Corners[Corners.Count - 1].Position, Corners[0].Position));
            }
        }
    }

    public record FloorPlanModel(IReadOnlyList<WallModel> Walls, WallModel? UnderConstruction);
}
