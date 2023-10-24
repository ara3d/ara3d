using Svg;
using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public static class Utils
    {
        public static Color ToDrawingColor(this ColorModel color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        public static SvgPaintServer ToSvg(this ColorModel color)
        {
            return new SvgColourServer(color.ToDrawingColor());
        }

        public static SvgElement? ToSvg(this ElementModel e)
        {
            switch (e)
            {
                case CircleModel circle:
                    return new SvgCircle()
                    {
                        CenterX = (float)circle.CenterX,
                        CenterY = (float)circle.CenterY,
                        Radius = (float)circle.Radius,
                        Stroke = circle.StrokeColor.ToSvg(),
                        StrokeWidth = (float)circle.StrokeWidth,
                        Fill = SvgPaintServer.None,
                    };
                case EllipseModel ellipse:
                    return new SvgEllipse()
                    {
                        CenterX = (float)ellipse.CenterX,
                        CenterY = (float)ellipse.CenterY,
                        RadiusX = (float)ellipse.Width / 2,
                        RadiusY = (float)ellipse.Height / 2,
                        Stroke = ellipse.StrokeColor.ToSvg(),
                        StrokeWidth = (float)ellipse.StrokeWidth,
                        Fill = SvgPaintServer.None,
                    };
                case LineModel line:
                    return new SvgLine()
                    {
                        StartX = (float)line.Left,
                        StartY = (float)line.Top,
                        EndX = (float)line.Right,
                        EndY = (float)line.Bottom,
                        Stroke = line.StrokeColor.ToSvg(),
                        StrokeWidth = (float)line.StrokeWidth,
                        Fill = SvgPaintServer.None,
                    };
                case RectModel rect:
                    return new SvgRectangle()
                    {
                        X = (float)rect.Left,
                        Y = (float)rect.Top,
                        Width = (float)rect.Width,
                        Height = (float)rect.Height,
                        Stroke = rect.StrokeColor.ToSvg(),
                        StrokeWidth = (float)rect.StrokeWidth,
                        Fill = SvgPaintServer.None,
                    };
                case SquareModel square:
                    return new SvgRectangle()
                    {
                        X = (float)square.Left,
                        Y = (float)square.Top,
                        Width = (float)square.Radius * 2,
                        Height = (float)square.Radius * 2,
                        Stroke = square.StrokeColor.ToSvg(),
                        StrokeWidth = (float)square.StrokeWidth,
                        Fill = SvgPaintServer.None,
                    };
            }

            return null;
        }

        public static SvgDocument ToSvg(this DocumentModel documentModel)
        {
            var r = new SvgDocument();
            foreach (var layer in documentModel.Layers)
            {
                if (layer.IsActive)
                {
                    foreach (var e in layer.Elements)
                    {
                        r.Children.Add(e.ToSvg());
                    }
                }
            }

            foreach (var e in documentModel.Elements)
            {
                var tmp = e.ToSvg();
                if (tmp != null) 
                    r.Children.Add(tmp);
            }
            return r;
        }

        public static IEnumerable<ToolStripMenuItem> AllMenuItems(this ToolStripItemCollection collection)
        {
            foreach (var m in collection.OfType<ToolStripMenuItem>())
            {
                yield return m;
                foreach (var m2 in m.DropDownItems.AllMenuItems())
                    yield return m2;
            }
        }

        public static void PreventFormDisposal(this Form form)
        {
            form.FormClosing += (sender, args) =>
            {
                form.Hide();
                args.Cancel = true;
            };
        }

        public static ColorModel ToModel(this Color color)
            => new() { R = color.R, B = color.B, G = color.G };

        public static bool UpdateIfChanged<T>(this List<T> self, IEnumerable<T> newValues)
        {
            var tmp = newValues.ToList();
            if (tmp.Count != self.Count || !self.SequenceEqual(tmp))
            {
                self.Clear();
                self.AddRange(tmp);
                return true;
            }

            return false;
        }
    }
}
