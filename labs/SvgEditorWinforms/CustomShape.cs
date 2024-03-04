using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Mathematics;
using Svg;
using SvgEditorWinForms.Models;

namespace SvgEditorWinForms
{
    public class CircularSaw : ElementModel
    {
        public float InnerRadius { get; set; } = 100;
        public float OuterRadius { get; set; } = 120;
        public int TeethCount { get; set; } = 20;
        public Vector2 Position { get; set; } = new Vector2(100, 100);

        public override SvgElement ToSvg()
        {
            var ts = TeethCount.SampleZeroToOneExclusive();
            var innerPoints = ts.Select(t => PrimitiveFunctions.Circle(t) * InnerRadius + Position).ToList();
            var outerPoints = ts.Select(t => PrimitiveFunctions.Circle(t) * OuterRadius + Position).ToList();
            var pts = new SvgPointCollection();
            for (var i = 0; i < TeethCount; ++i)
            {
                var p1 = innerPoints[i];
                var p2 = outerPoints[i];
                pts.Add(p1.X);
                pts.Add(p1.Y);
                pts.Add(p2.X);
                pts.Add(p2.Y);
            }

            return new SvgPolygon
            {
                Points = pts
            };
        }
    }
}
