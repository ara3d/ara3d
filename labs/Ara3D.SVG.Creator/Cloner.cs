using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.SVG.Creator;

public class Cloner : Operator
{
    public int Count { get; set; } = 10;

    public override IEntity Evaluate(IEntity e, float strength)
    {
        var list = new List<IEntity>();
        for (var i = 0; i < Count; ++i)
        {
            var tmp = e.Clone();
            list.Add(tmp);
        }

        return new Compound(list.ToIArray());
    }
}

public class ArrangeRadially : Operator
{
    public Vector Center { get; set; } = new(150, 150);
    public Size Radius { get; set; } = new(50, 75);

    public Vector UnitCirclePosition(double t)
        => new DVector2(System.Math.Sin(t * System.Math.PI * 2), System.Math.Cos(t * System.Math.PI * 2));

    public Vector Position(double t)
        => UnitCirclePosition(t) * Radius.ToVector() + Center;

    public override IEntity Evaluate(IEntity e, float strength)
    {
        if (e is Compound c)
        {
            return c.With(ab => ab.Transform(VectorAttributesEnum.Position, (ab, v, i) =>
                Position(ab.Amount[i])));
        }

        return e;
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