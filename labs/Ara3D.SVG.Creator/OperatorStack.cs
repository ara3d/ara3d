namespace Ara3D.SVG.Creator;

public class OperatorStack
{
    public Generator Generator { get; set; }
    public List<Operator> Operators { get; } = new();

    public IEntity Evaluate()
    {
        var e = Generator.Evaluate();
        foreach (var op in Operators)
        {
            e = op.Evaluate(e, 1);
        }

        return e;
    }
    
}