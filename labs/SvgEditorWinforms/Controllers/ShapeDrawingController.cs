using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public class ShapeDrawingController
    {
        public Control Control { get; }
        public PointModel Start { get; private set; }
        public PointModel Current { get; private set; }
        public SimpleShapeModel? Shape { get; private set; }

        public SizeModel Size => new()
        {
            Width = Current.X - Start.X, 
            Height = Current.Y - Start.Y
        };

        public ShapeDrawingController(Control control)
        {
            Control = control;
            Control.Capture = true;     
            Start = new PointModel();
            Current = Start;
        }

        public PointModel ScreenToControl(Point pt)
        {
            var tmp = Control.PointToScreen(Point.Empty);
            return new PointModel() { X = pt.X - tmp.X, Y = pt.Y - tmp.Y };
        }

        public void StartDrawing(SimpleShapeModel shapeModel, Point pt)
        {
            Shape = shapeModel;
            Start = ScreenToControl(pt);
            Shape.Position = Start;
            Shape.Size = new SizeModel() { Width = 0, Height = 0 };
            Current = Start;
        }

        public void Update(Point pt)
        {
            Current = ScreenToControl(pt);
            if (Shape != null)
            {
                Shape.Size = Size;
            }
        }

        public void StopDrawing()
        {
            Shape = null;
        }

        public bool IsDrawing()
        {
            return Shape != null;
        }
    }
}
