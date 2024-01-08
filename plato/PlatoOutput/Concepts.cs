public interface Any
{
    Array<String> FieldNames { get; }
    Array<Any> FieldValues { get; }
}
public interface Value: Any
{
}
public interface Array<T>
where T : Any
{
    Integer Count { get; }
    T At(Integer n);
}
public interface Vector<Self, T>: Array<T>, Numerical<Self>, Magnitudinal<Self>
where T : Numerical<T>
{
}
public interface Measure<Self>: Value, ScalarArithmetic<Self>, Equatable<Self>, Comparable<Self>, Magnitudinal<Self>
{
    Number Value { get; }
}
public interface Numerical<Self>: Value, Arithmetic<Self>, ScalarArithmetic<Self>, Equatable<Self>, Comparable<Self>, Magnitudinal<Self>
{
    Self Zero { get; }
    Self One { get; }
    Self MinValue { get; }
    Self MaxValue { get; }
}
public interface Magnitudinal<Self>: Comparable<Self>
{
    Number Magnitude { get; }
}
public interface Comparable<Self>
{
    Integer Compare(Self y);
}
public interface Equatable<Self>
{
    Boolean Equals(Self b);
    Boolean NotEquals(Self b);
}
public interface Arithmetic<Self>: AdditiveInverse<Self>
{
    Self Add(Self other);
    Self Subtract(Self other);
    Self Negative { get; }
    Self Reciprocal { get; }
    Self Multiply(Self other);
    Self Divide(Self other);
    Self Modulo(Self other);
}
public interface AdditiveInverse<Self>
{
    Self Negative { get; }
}
public interface AdditiveArithmetic<Self, T>
where T : AdditiveInverse<T>
{
    Self Add(T other);
    Self Subtract(T other);
}
public interface ScalarArithmetic<Self>: AdditiveArithmetic<Self, Number>
{
    Self Add(Number scalar);
    Self Subtract(Number scalar);
    Self Multiply(Number scalar);
    Self Divide(Number scalar);
    Self Modulo(Number scalar);
}
public interface BooleanOperations<Self>
{
    Self And(Self b);
    Self Or(Self b);
    Self Not { get; }
}
public interface Interval<T>
where T : Measure<T>
{
    T Min { get; }
    T Max { get; }
}
