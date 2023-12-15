using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ara3D.Math;
using ExCSS;
using Color = System.Drawing.Color;

namespace Ara3D.SVG.Creator;

public class Primitives
{
}

// Data that flows through a pipeline
public interface IPrimitive 
{ }

// Like a Unity component 
public interface IComponent
{
    string Name { get; }
    IParameters Parameters { get; }
    bool Active { get; }
}

// Controls how things are converted into SVG elements 
// There is always one at the top of the stack.
// There can be multiple. 
// Only "Renderers" can be "used" (because they actually generate SVG). 
public interface IRenderer : IComponent { }

// TODO: maybe can we just add random CSS, and JavaScript, and attributes? 

// NOTE: what about text? 
// A text element can't feed anything else. 
// It could flow through the stack. 

public interface IFont
{
    string Family { get; }
    string Weight { get; }
    string Size { get; }
    string Style { get; }
}

public interface ITextRender : IRenderer
{
    IFont Font { get; }
}

//==

// 
public interface IGenerator : IComponent
{
    IPrimitive Output { get; }
}

public interface IModifier : IComponent { }

//==

public interface IPoint : IPrimitive { }
public interface IPoints : IPrimitive { }
public interface ILine : IPrimitive { }
public interface ILines : IPrimitive { }
public interface IPath : IPrimitive { }
public interface IPathSegment : IPrimitive { } 
public interface IBezierCurve : IPrimitive { }
public interface ICubicCurve : IPrimitive { }

public interface IFunction1D : IPrimitive { }



public interface IPolarFunction : IPrimitive { }
public interface IFunction2D : IPrimitive { }

public interface IPointsCollection : IPrimitive { }

//==

public class FunctionRendererParameters
{
    [Range(0, 10000)] public int NumSamples { get; set; } = 100;
    public bool AsPointsOrLines { get; set; }
    public Color StrokeColor { get; set;  } = Color.Blue;
    [Range(0.001, 1000.0)] public double InnerThickness { get; set; } = 3;
    [Range(0.001, 1000.0)] public double OuterThickness { get; set; } = 5;
    public Color FillColor { get; set; } = Color.Beige;
}

public interface IParameters
{ }


// https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/transform#general_transformation
public interface ITransform2D : IComponent { }

public interface ITransform3D : IComponent { }

public interface IDeformer : IModifier { }


public interface ISineWave : IGenerator
{ }

public interface IReflect : IModifier { }
public interface ISmooth : IModifier { } 

// Add more points 
public interface ISubdivide : IModifier { } 

//==

public interface ILineCloner : IModifier { } 
public interface IRadialCloner : IModifier { }

//==

// Convert between the types of primitives. 
public interface IConverter : IComponent { }

//==

// Things I can expect for organizational purposes 
public interface ILayer { }
public interface ITheme { }
public interface IPalette { }
public interface ILibrary { }

//==

public interface IColorGenerator { }
public interface ILinearGradient : IColorGenerator { }
public interface IRadialGradient : IColorGenerator { }
public interface IColor { }

//== 

// https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Patterns
// https://developer.mozilla.org/en-US/docs/Web/SVG/Element/pattern
public interface IPattern { }

//==

// https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Texts
public interface IText { }

//==

// https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Basic_Transformations
public interface ISvg { }

//===

// https://developer.mozilla.org/en-US/docs/Web/SVG/Element/g
public interface IGroup { }


//==

// https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Clipping_and_masking
public interface IClip { }
public interface IMask { }

//==

// https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Filter_effects
public interface IFilter { }
public interface IFilterElement { }

//==

// https://developer.mozilla.org/en-US/docs/Web/SVG/Element/use
public interface IUse { }

//==

//https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/style
public interface IStyle { }

//==

// https://stackoverflow.com/questions/3975499/convert-svg-to-image-jpeg-png-etc-in-the-browser

//==

/*
There 
*/

/*
 * 
Animation elements
<animate>, <animateMotion>, <animateTransform>, <mpath>, <set>

Basic shapes
<circle>, <ellipse>, <line>, <polygon>, <polyline>, <rect>

Container elements
<a>, <defs>, <g>, <marker>, <mask>, <missing-glyph>, <pattern>, <svg>, <switch>, <symbol>

Descriptive elements
<desc>, <metadata>, <title>

Filter primitive elements
<feBlend>, <feColorMatrix>, <feComponentTransfer>, <feComposite>, <feConvolveMatrix>, <feDiffuseLighting>, <feDisplacementMap>, <feDropShadow>, <feFlood>, <feFuncA>, <feFuncB>, <feFuncG>, <feFuncR>, <feGaussianBlur>, <feImage>, <feMerge>, <feMergeNode>, <feMorphology>, <feOffset>, <feSpecularLighting>, <feTile>, <feTurbulence>

Font elements
<font>, <font-face>, <font-face-format>, <font-face-name>, <font-face-src>, <font-face-uri>, <hkern>, <vkern>

Gradient elements
<linearGradient>, <radialGradient>, <stop>

Graphics elements
<circle>, <ellipse>, <image>, <line>, <path>, <polygon>, <polyline>, <rect>, <text>, <use>

Graphics referencing elements
<use>

Light source elements
<feDistantLight>, <fePointLight>, <feSpotLight>

Never-rendered elements
<clipPath>, <defs>, <hatch>, <linearGradient>, <marker>, <mask>, <metadata>, <pattern>, <radialGradient>, <script>, <style>, <symbol>, <title>

Paint server elements
<hatch>, <linearGradient>, <pattern>, <radialGradient>, <solidcolor>

Renderable elements
<a>, <circle>, <ellipse>, <foreignObject>, <g>, <image>, <line>, <path>, <polygon>, <polyline>, <rect>, <svg>, <switch>, <symbol>, <text>, <textPath>, <tspan>, <use>
Note: The SVG 2 spec requires that any unknown elements be treated as <g> for the purpose of rendering.

Shape elements
<circle>, <ellipse>, <line>, <path>, <polygon>, <polyline>, <rect>

Structural elements
<defs>, <g>, <svg>, <symbol>, <use>

Text content elements
<glyph>, <glyphRef>, <textPath>, <text>, <tref>, <tspan>

Text content child elements
<textPath>, <tref>, <tspan>

Uncategorized elements
<clipPath>, <cursor>, <filter>, <foreignObject>, <hatchpath>, <script>, <style>, <view>

*/

public class Function<TOutput>
{
    public float MinInput { get; set; }
    public float MaxInput { get; set; }
    public Func<float, TOutput> Func { get; set; }
    public float Range => MaxInput - MinInput;
}

public class Function1D : Function<float>
{
}

public class Function2D : Function<Vector2>
{
}

public class FunctionPolar : Function<float>
{
    public FunctionPolar()
    {
        MinInput = 0;
        MaxInput = MathF.PI * 2;
    }
}

public class Circle : FunctionPolar
{
    public Circle()
    {
        Func = x => 1;
    }
}

public class Rose : FunctionPolar
{
    public int N { get; set; } = 3;
    public int D { get; set; } = 4;
    
    public Rose()
    {
        Func = x => MathF.Cos(N * x / D);
    }
}

public class Spiral : FunctionPolar
{
    public Spiral()
    {
        Count = 5;
        Func = x => MinRadius + x * (RadiusDelta) / Range;
    }

    public float Count
    {
        get => MaxInput / (MathF.PI * 2);
        set => MaxInput = MathF.PI * 2 * value;
    }

    public float MinRadius { get; set; } = 0;
    public float MaxRadius { get; set; } = 1;
    public float RadiusDelta => MaxRadius - MinRadius;
}

public class Converters2
{
    public Function2D FunctionPolarToFunction2D(FunctionPolar fp) => throw new NotImplementedException();
    public FunctionPolar FunctionPolarTransform(FunctionPolar fp) => throw new NotImplementedException();
    public Function2D PathToFunction2D(Path path) => throw new NotImplementedException();
}