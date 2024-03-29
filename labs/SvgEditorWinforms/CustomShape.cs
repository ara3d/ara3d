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
        public Vector2 Position { get; set; } = (100, 100);

        public override SvgElement ToSvg()
        {
            var ts = TeethCount.InterpolateExclusive();
            var innerPoints = ts.Select(t => t.Circle() * InnerRadius + Position).ToList();
            var outerPoints = ts.Select(t => t.Circle() * OuterRadius + Position).ToList();
            var pts = new SvgPointCollection();
            for (var i = 0; i < TeethCount; ++i)
            {
                var p1 = innerPoints[i];
                var p2 = outerPoints[i];
                pts.Add((float)p1.X);
                pts.Add((float)p1.Y);
                pts.Add((float)p2.X);
                pts.Add((float)p2.Y);
            }

            return new SvgPolygon
            {
                Points = pts
            };
        }
    }
}
