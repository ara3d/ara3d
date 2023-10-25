namespace Ara3D.Utils
{
    public readonly struct Percent
    {
        public Percent(double value) => Value = value;
        public double Value { get; }
        public static implicit operator double(Percent percent) => percent.Value;
        public static implicit operator Percent(double value) => new Percent(value);
        public static Percent FromFraction(double numerator, double denominator) => FromDecimalValue(numerator/denominator);
        public static Percent FromDecimalValue(double fractionalValue) => fractionalValue * 100.0;
        public double AsDecimalValue => Value / 100.0;
    }
}