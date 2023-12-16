using Ara3D.Collections;

namespace Ara3D.SVG.Creator;

public abstract class Cloner
{
    public abstract IArray<Stack> Clone(Stack stack);
}



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