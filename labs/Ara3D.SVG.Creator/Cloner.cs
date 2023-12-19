using Ara3D.Collections;
using Ara3D.Math;
using Svg;
using Svg.Transforms;

namespace Ara3D.SVG.Creator;

public abstract class Cloner : Operator
{
}

public class RadialCloner : Cloner
{
    public Vector2 Center { get; set; } = (100, 100);
    public float Radius { get; set; } = 10f;
    public int Count { get; set; } = 10;

    public override IEntity Evaluate(IElement e, float strength)
    {
        var list = new List<IEntity>();
        for (var i = 0; i < Count; ++i)
        {
            var tmp = e.Clone();
            list.Add(tmp);
        }

        var r = new Compound(list);
        var opRotate = new Rotate() { Angle = 360f, Center = Center, LinearRamp = true };
        return opRotate.Evaluate(r, strength);
    }
}

/*
public class Subdivider : Cloner
{
    public int Rows { get; set; } = 2;
    public int Columns { get; set; } = 2;

    public override IArray<Stack> Clone(Stack stack)
    {
        var list = new List<Stack>();
        var delta = stack.Delta / (Columns, Rows);
        for (var i = 0; i < Columns; i++)
        {
            for (var j = 0; j < Rows; j++)
            {
                var offset = delta * (i, j);
                var newStack = new Stack
                {
                    A = stack.A + offset,
                    B = stack.A + offset + delta,
                    Function = stack.Function,
                    RendererParameters = stack.RendererParameters
                };
                list.Add(newStack);
            }
        }

        return list.ToIArray();
    }
}
*/