namespace Ara3D.SVG.Creator;

public class OperatorStack
{
    public Generator Generator { get; set; }
    public List<Operator> Operators { get; } = new();
    public IEntity Evaluate()
        => Operators.Aggregate(Generator.Evaluate(), (e, op) => op.Evaluate(e, 1f));
}