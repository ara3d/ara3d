using System.Windows;
using System.Windows.Media;

namespace Ara3D.FloorPlanner
{
    public record FloorPlanElementStyle
    {
        public Color BorderColor = Colors.BlueViolet;
        public double StrokeWidth = 1;
        public PenStyle PenStyle => new PenStyle(BorderColor, StrokeWidth);
    }

    public interface IView
    {
        bool HitTest(Point point);
        ICanvas Draw(ICanvas canvas);
        Rect Rect { get; }
    }

    public record FloorPlanView(FloorPlanModel Model, FloorPlanElementStyle Style) : IView
    {
        public bool HitTest(Point point) => false;
        public ICanvas Draw(ICanvas canvas) => canvas;
        public Rect Rect { get; }
    }

    public record WallView(WallModel Model, FloorPlanElementStyle Style) : IView
    {
        public bool HitTest(Point point) => false;
        public ICanvas Draw(ICanvas canvas)
            => Model.Segments.Aggregate(canvas, (current, segment) 
                => current.Draw(new StyledLine(Style.PenStyle, segment.Line)));
        public Rect Rect { get; }
    }

    public record CornerView(CornerModel Model, FloorPlanElementStyle Style) : IView
    {
        public bool HitTest(Point point) => false;
        public ICanvas Draw(ICanvas canvas) => canvas;
        public Rect Rect { get; }
    }
}
