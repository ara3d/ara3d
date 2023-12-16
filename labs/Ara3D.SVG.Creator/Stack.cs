using Ara3D.Math;

namespace Ara3D.SVG.Creator;

public class Stack
{
    public FunctionRendererParameters RendererParameters { get; set; } = new FunctionRendererParameters();
    public Function Function { get; set; } = new Circle();
    public Vector2 A { get; set; }
    public Vector2 B { get; set; }
    public Vector2 Delta => B - A;
    public Cloner Cloner { get; set; } = new Subdivider();
    public Vector2 GetPoint(float x) => A + Function.Func(x * Function.Length + Function.Offset) * Delta;
}