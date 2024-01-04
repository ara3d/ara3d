using System.Drawing;
using Ara3D.Math;
using Svg;
using Svg.Pathing;

namespace Ara3D.SVG.Creator;

//== https://svgwg.org/svg2-draft/paths.html#PathElement

public class PathBuilder
{
    public readonly SvgPath Path = new() { PathData = new SvgPathSegmentList() };

    public string ToXml()
    {
        Path.Stroke = new SvgColourServer(Color.Blue);
        Path.StrokeWidth = 5;
        return Path.GetXML();
    }

    public void MoveRel(Vector2 v)
    {
        Path.PathData.Add(new SvgMoveToSegment(true, v.ToSvg()));
    }

    public void MoveAbs(Vector2 v)
    {
        Path.PathData.Add(new SvgMoveToSegment(false, v.ToSvg()));
    }

    public void LineRel(Vector2 v)
    {
        Path.PathData.Add(new SvgLineSegment(true, v.ToSvg()));
    }

    public void LineAbs(Vector2 v)
    {
        Path.PathData.Add(new SvgLineSegment(false, v.ToSvg()));
    }

    public void CubicRel(Vector2 cp1, Vector2 cp2, Vector2 final)
    {
        Path.PathData.Add(new SvgCubicCurveSegment(true, cp1.ToSvg(), cp2.ToSvg(), final.ToSvg()));
    }

    public void CubicAbs(Vector2 cp1, Vector2 cp2, Vector2 final)
    {
        Path.PathData.Add(new SvgCubicCurveSegment(true, cp1.ToSvg(), cp2.ToSvg(), final.ToSvg()));
    }

    public void QuadRel(Vector2 cp, Vector2 final)
    {
        Path.PathData.Add(new SvgQuadraticCurveSegment(true, cp.ToSvg(), final.ToSvg()));
    }

    public void QuadAbs(Vector2 cp, Vector2 final)
    {
        Path.PathData.Add(new SvgQuadraticCurveSegment(false, cp.ToSvg(), final.ToSvg()));
    }

    public void PolyCubicSegmentRel(Vector2 cp, Vector2 final)
    {
        Path.PathData.Add(new SvgCubicCurveSegment(true, cp.ToSvg(), final.ToSvg()));
    }

    public void PolyCubicSegmentAbs(Vector2 cp, Vector2 final)
    {
        Path.PathData.Add(new SvgCubicCurveSegment(false, cp.ToSvg(), final.ToSvg()));
    }

    public void PolyQuadSegmentRel(Vector2 final)
    {
        Path.PathData.Add(new SvgQuadraticCurveSegment(true, final.ToSvg()));
    }

    public void PolyQuadSegmentAbs(Vector2 final)
    {
        Path.PathData.Add(new SvgQuadraticCurveSegment(true, final.ToSvg()));
    }

    public void ClosePath()
    {
        Path.PathData.Add(new SvgClosePathSegment(true));
    }

    //==

    // TODO: elliptical arcs. So complicated, I don't even know how to get them to work.     
}

public class SvgDocumentBuilder
{
    public readonly SvgDocument Doc = new();
    //public Dictionary<string, SvgElement> Operators { get; } = new Dictionary<string, SvgElement>();

    public void AddElement(SvgElement element)
    {
        //var guid = Guid.NewGuid();
        //element.ID = guid.ToString();
        //Operators.Add(element.ID, element);
        Doc.Children.Add(element);
    }
}

public static class Converters
{
    public static PointF ToSvg(this Vector2 p) => new(p.X, p.Y);
    public static PointF ToSvg(this DVector2 p) => p.Vector2.ToSvg();

    public static Color ToSvg(this ColorHDR self) =>
        Color.FromArgb(
            (int)(self.A * 255),
            (int)(self.R * 255),
            (int)(self.G * 255),
            (int)(self.B * 255));
}

/*
 * SVG basic shapes: <circle>, <ellipse>, <line>, <polygon>, <polyline>, <rect>
 */

/*
<a>
<animate>
<animateMotion>
<animateTransform>
<circle>
<clipPath>
<cursor> 9 Deprecated
<defs>
<desc>
<ellipse>
<feBlend>
<feColorMatrix>
<feComponentTransfer>
<feComposite>
<feConvolveMatrix>
<feDiffuseLighting>
<feDisplacementMap>
<feDistantLight>
<feDropShadow>
<feFlood>
<feFuncA>
<feFuncB>
<feFuncG>
<feFuncR>
<feGaussianBlur>
<feImage>
<feMerge>
<feMergeNode>
<feMorphology>
<feOffset>
<fePointLight>
<feSpecularLighting>
<feSpotLight>
<feTile>
<feTurbulence>
<filter>
<font-face-format> Deprecated
<font-face-name> Deprecated
<font-face-src> Deprecated 
<font-face-uri> Deprecated
<font-face> Deprecated
<font> Deprecated
<foreignObject>
<g>
<glyph> Deprecated
<glyphRef> Deprecated
<hkern> Deprecated
<image>
<line>
<linearGradient>
<marker>
<mask>
<metadata>
<missing-glyph>  Deprecated
<mpath>
<path>
<pattern>
<polygon>
<polyline>
<radialGradient>
<rect>
<script>
<set>
<stop>
<style>
<svg>
<switch>
<symbol>
<text>
<textPath>
<title> — the SVG accessible name element
<tref> Deprecated
<tspan>
<use>
<view>
<vkern> Deprecated
*/