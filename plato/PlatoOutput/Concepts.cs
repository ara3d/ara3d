public interface Any<Self>
{
    Array<String> FieldNames();
    Array<Any> FieldValues();
    Array<Type> FieldTypes();
    Type TypeOf();
}
public interface Value<Self>: Any<Self>
{
    Self Default();
}
public interface Array<T, Self>
{
    Integer Count();
    T At(Integer n);
}
public interface Vector<T, Self>: Array<Self, T>, Numerical<Self>
{
}
public interface Measure<Self>: Value<Self>, ScalarArithmetic<Self>, Equatable<Self>, Comparable<Self>, Magnitudinal<Self>
{
    Number Value();
}
public interface Numerical<Self>: Value<Self>, Arithmetic<Self>, ScalarArithmetic<Self>, Equatable<Self>, Comparable<Self>, Magnitudinal<Self>
{
    Self Zero();
    Self One();
    Self MinValue();
    Self MaxValue();
}
public interface Magnitudinal<Self>: Value<Self>
{
    Number Magnitude();
}
public interface Comparable<Self>: Value<Self>
{
    Integer Compare(Self y);
}
public interface Equatable<Self>: Value<Self>
{
    Boolean Equals(Self b);
}
public interface Arithmetic<Self>: Value<Self>
{
    Self Add(Self other);
    Self Subtract(Self other);
    Self Negative();
    Self Reciprocal();
    Self Multiply(Self other);
    Self Divide(Self other);
    Self Modulo(Self other);
}
public interface AdditiveInverse<Self>
{
    Self Negative();
}
public interface AdditiveArithmetic<T, Self>
{
    Self Add(T other);
    Self Subtract(T other);
}
public interface ScalarArithmetic<Self>: AdditiveArithmetic<Self, Number>, Value<Self>
{
    Self Add(Number scalar);
    Self Subtract(Number scalar);
    Self Multiply(Number scalar);
    Self Divide(Number scalar);
    Self Modulo(Number scalar);
}
public interface BooleanOperations<Self>: Value<Self>
{
    Self And(Self b);
    Self Or(Self b);
    Self Not();
}
public interface Interval<T, Self>: Vector<Self, T>
{
    T Min();
    T Max();
}
