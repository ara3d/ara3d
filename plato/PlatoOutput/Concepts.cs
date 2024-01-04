public interface Any
{
    Array<String> FieldNames();
    Array<Any> FieldValues();
    Array<Type> FieldTypes();
    Type TypeOf();
}
public interface Value<Self>: Any
{
    Self Default();
}
public interface Array<T>
{
    Integer Count();
    T At(Integer n);
}
public interface Vector<Self, T>: Array<T>, Numerical<Self>, Magnitudinal<Self>
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
public interface Magnitudinal<Self>: Comparable<Self>
{
    Number Magnitude();
    Integer Compare(Self y);
}
public interface Comparable<Self>
{
    Integer Compare(Self y);
}
public interface Equatable<Self>
{
    Boolean Equals(Self b);
}
public interface Arithmetic<Self>
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
public interface AdditiveArithmetic<Self, T>
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
    Self Not();
}
public interface Interval<Self, T>: Vector<Self, T>
{
    T Min();
    T Max();
    Integer Count();
    Any At(Integer n);
}
