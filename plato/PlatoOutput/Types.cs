using System;
public class Count: Numerical<Count>
{
    public Integer Value { get; }
    public Count(Integer value) => (Value) = (value);
    public Count() { }
    public static Count New(Integer value) => new Count(value);
    public static implicit operator Integer(Count self) => self.Value;
    public static implicit operator Count(Integer value) => new Count(value);
    public Count Zero() => throw new NotImplementedException();
    public Count One() => throw new NotImplementedException();
    public Count MinValue() => throw new NotImplementedException();
    public Count MaxValue() => throw new NotImplementedException();
    public Count Default() => throw new NotImplementedException();
    public Count Add(Count other) => throw new NotImplementedException();
    public static Count operator +(Count self, Count other) => throw new NotImplementedException();
    public Count Subtract(Count other) => throw new NotImplementedException();
    public static Count operator -(Count self, Count other) => throw new NotImplementedException();
    public Count Negative() => throw new NotImplementedException();
    public static Count operator -(Count self) => throw new NotImplementedException();
    public Count Reciprocal() => throw new NotImplementedException();
    public Count Multiply(Count other) => throw new NotImplementedException();
    public static Count operator *(Count self, Count other) => throw new NotImplementedException();
    public Count Divide(Count other) => throw new NotImplementedException();
    public static Count operator /(Count self, Count other) => throw new NotImplementedException();
    public Count Modulo(Count other) => throw new NotImplementedException();
    public static Count operator %(Count self, Count other) => throw new NotImplementedException();
    public Count Add(Number scalar) => throw new NotImplementedException();
    public static Count operator +(Count self, Number scalar) => throw new NotImplementedException();
    public Count Subtract(Number scalar) => throw new NotImplementedException();
    public static Count operator -(Count self, Number scalar) => throw new NotImplementedException();
    public Count Multiply(Number scalar) => throw new NotImplementedException();
    public static Count operator *(Count self, Number scalar) => throw new NotImplementedException();
    public Count Divide(Number scalar) => throw new NotImplementedException();
    public static Count operator /(Count self, Number scalar) => throw new NotImplementedException();
    public Count Modulo(Number scalar) => throw new NotImplementedException();
    public static Count operator %(Count self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Count b) => throw new NotImplementedException();
    public static Boolean operator ==(Count a, Count b) => throw new NotImplementedException();
    public Boolean NotEquals(Count b) => throw new NotImplementedException();
    public static Boolean operator !=(Count a, Count b) => throw new NotImplementedException();
    public Integer Compare(Count y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Count y) => throw new NotImplementedException();
}
public class Index: Value<Index>
{
    public Integer Value { get; }
    public Index(Integer value) => (Value) = (value);
    public Index() { }
    public static Index New(Integer value) => new Index(value);
    public static implicit operator Integer(Index self) => self.Value;
    public static implicit operator Index(Integer value) => new Index(value);
    public Index Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Unit: Numerical<Unit>
{
    public Number Value { get; }
    public Unit(Number value) => (Value) = (value);
    public Unit() { }
    public static Unit New(Number value) => new Unit(value);
    public static implicit operator Number(Unit self) => self.Value;
    public static implicit operator Unit(Number value) => new Unit(value);
    public Unit Zero() => throw new NotImplementedException();
    public Unit One() => throw new NotImplementedException();
    public Unit MinValue() => throw new NotImplementedException();
    public Unit MaxValue() => throw new NotImplementedException();
    public Unit Default() => throw new NotImplementedException();
    public Unit Add(Unit other) => throw new NotImplementedException();
    public static Unit operator +(Unit self, Unit other) => throw new NotImplementedException();
    public Unit Subtract(Unit other) => throw new NotImplementedException();
    public static Unit operator -(Unit self, Unit other) => throw new NotImplementedException();
    public Unit Negative() => throw new NotImplementedException();
    public static Unit operator -(Unit self) => throw new NotImplementedException();
    public Unit Reciprocal() => throw new NotImplementedException();
    public Unit Multiply(Unit other) => throw new NotImplementedException();
    public static Unit operator *(Unit self, Unit other) => throw new NotImplementedException();
    public Unit Divide(Unit other) => throw new NotImplementedException();
    public static Unit operator /(Unit self, Unit other) => throw new NotImplementedException();
    public Unit Modulo(Unit other) => throw new NotImplementedException();
    public static Unit operator %(Unit self, Unit other) => throw new NotImplementedException();
    public Unit Add(Number scalar) => throw new NotImplementedException();
    public static Unit operator +(Unit self, Number scalar) => throw new NotImplementedException();
    public Unit Subtract(Number scalar) => throw new NotImplementedException();
    public static Unit operator -(Unit self, Number scalar) => throw new NotImplementedException();
    public Unit Multiply(Number scalar) => throw new NotImplementedException();
    public static Unit operator *(Unit self, Number scalar) => throw new NotImplementedException();
    public Unit Divide(Number scalar) => throw new NotImplementedException();
    public static Unit operator /(Unit self, Number scalar) => throw new NotImplementedException();
    public Unit Modulo(Number scalar) => throw new NotImplementedException();
    public static Unit operator %(Unit self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Unit b) => throw new NotImplementedException();
    public static Boolean operator ==(Unit a, Unit b) => throw new NotImplementedException();
    public Boolean NotEquals(Unit b) => throw new NotImplementedException();
    public static Boolean operator !=(Unit a, Unit b) => throw new NotImplementedException();
    public Integer Compare(Unit y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Unit y) => throw new NotImplementedException();
}
public class Percent: Numerical<Percent>
{
    public Number Value { get; }
    public Percent(Number value) => (Value) = (value);
    public Percent() { }
    public static Percent New(Number value) => new Percent(value);
    public static implicit operator Number(Percent self) => self.Value;
    public static implicit operator Percent(Number value) => new Percent(value);
    public Percent Zero() => throw new NotImplementedException();
    public Percent One() => throw new NotImplementedException();
    public Percent MinValue() => throw new NotImplementedException();
    public Percent MaxValue() => throw new NotImplementedException();
    public Percent Default() => throw new NotImplementedException();
    public Percent Add(Percent other) => throw new NotImplementedException();
    public static Percent operator +(Percent self, Percent other) => throw new NotImplementedException();
    public Percent Subtract(Percent other) => throw new NotImplementedException();
    public static Percent operator -(Percent self, Percent other) => throw new NotImplementedException();
    public Percent Negative() => throw new NotImplementedException();
    public static Percent operator -(Percent self) => throw new NotImplementedException();
    public Percent Reciprocal() => throw new NotImplementedException();
    public Percent Multiply(Percent other) => throw new NotImplementedException();
    public static Percent operator *(Percent self, Percent other) => throw new NotImplementedException();
    public Percent Divide(Percent other) => throw new NotImplementedException();
    public static Percent operator /(Percent self, Percent other) => throw new NotImplementedException();
    public Percent Modulo(Percent other) => throw new NotImplementedException();
    public static Percent operator %(Percent self, Percent other) => throw new NotImplementedException();
    public Percent Add(Number scalar) => throw new NotImplementedException();
    public static Percent operator +(Percent self, Number scalar) => throw new NotImplementedException();
    public Percent Subtract(Number scalar) => throw new NotImplementedException();
    public static Percent operator -(Percent self, Number scalar) => throw new NotImplementedException();
    public Percent Multiply(Number scalar) => throw new NotImplementedException();
    public static Percent operator *(Percent self, Number scalar) => throw new NotImplementedException();
    public Percent Divide(Number scalar) => throw new NotImplementedException();
    public static Percent operator /(Percent self, Number scalar) => throw new NotImplementedException();
    public Percent Modulo(Number scalar) => throw new NotImplementedException();
    public static Percent operator %(Percent self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Percent b) => throw new NotImplementedException();
    public static Boolean operator ==(Percent a, Percent b) => throw new NotImplementedException();
    public Boolean NotEquals(Percent b) => throw new NotImplementedException();
    public static Boolean operator !=(Percent a, Percent b) => throw new NotImplementedException();
    public Integer Compare(Percent y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Percent y) => throw new NotImplementedException();
}
public class Quaternion: Value<Quaternion>
{
    public Number X { get; }
    public Number Y { get; }
    public Number Z { get; }
    public Number W { get; }
    public Quaternion(Number x, Number y, Number z, Number w) => (X, Y, Z, W) = (x, y, z, w);
    public Quaternion() { }
    public static Quaternion New(Number x, Number y, Number z, Number w) => new Quaternion(x, y, z, w);
    public static implicit operator (Number, Number, Number, Number)(Quaternion self) => (self.X, self.Y, self.Z, self.W);
    public static implicit operator Quaternion((Number, Number, Number, Number) value) => new Quaternion(value.Item1, value.Item2, value.Item3, value.Item4);
    public Quaternion Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Unit2D: Value<Unit2D>
{
    public Unit X { get; }
    public Unit Y { get; }
    public Unit2D(Unit x, Unit y) => (X, Y) = (x, y);
    public Unit2D() { }
    public static Unit2D New(Unit x, Unit y) => new Unit2D(x, y);
    public static implicit operator (Unit, Unit)(Unit2D self) => (self.X, self.Y);
    public static implicit operator Unit2D((Unit, Unit) value) => new Unit2D(value.Item1, value.Item2);
    public Unit2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Unit3D: Value<Unit3D>
{
    public Unit X { get; }
    public Unit Y { get; }
    public Unit Z { get; }
    public Unit3D(Unit x, Unit y, Unit z) => (X, Y, Z) = (x, y, z);
    public Unit3D() { }
    public static Unit3D New(Unit x, Unit y, Unit z) => new Unit3D(x, y, z);
    public static implicit operator (Unit, Unit, Unit)(Unit3D self) => (self.X, self.Y, self.Z);
    public static implicit operator Unit3D((Unit, Unit, Unit) value) => new Unit3D(value.Item1, value.Item2, value.Item3);
    public Unit3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Direction3D: Value<Direction3D>
{
    public Unit3D Value { get; }
    public Direction3D(Unit3D value) => (Value) = (value);
    public Direction3D() { }
    public static Direction3D New(Unit3D value) => new Direction3D(value);
    public static implicit operator Unit3D(Direction3D self) => self.Value;
    public static implicit operator Direction3D(Unit3D value) => new Direction3D(value);
    public Direction3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class AxisAngle: Value<AxisAngle>
{
    public Unit3D Axis { get; }
    public Angle Angle { get; }
    public AxisAngle(Unit3D axis, Angle angle) => (Axis, Angle) = (axis, angle);
    public AxisAngle() { }
    public static AxisAngle New(Unit3D axis, Angle angle) => new AxisAngle(axis, angle);
    public static implicit operator (Unit3D, Angle)(AxisAngle self) => (self.Axis, self.Angle);
    public static implicit operator AxisAngle((Unit3D, Angle) value) => new AxisAngle(value.Item1, value.Item2);
    public AxisAngle Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class EulerAngles: Value<EulerAngles>
{
    public Angle Yaw { get; }
    public Angle Pitch { get; }
    public Angle Roll { get; }
    public EulerAngles(Angle yaw, Angle pitch, Angle roll) => (Yaw, Pitch, Roll) = (yaw, pitch, roll);
    public EulerAngles() { }
    public static EulerAngles New(Angle yaw, Angle pitch, Angle roll) => new EulerAngles(yaw, pitch, roll);
    public static implicit operator (Angle, Angle, Angle)(EulerAngles self) => (self.Yaw, self.Pitch, self.Roll);
    public static implicit operator EulerAngles((Angle, Angle, Angle) value) => new EulerAngles(value.Item1, value.Item2, value.Item3);
    public EulerAngles Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Rotation3D: Value<Rotation3D>
{
    public Quaternion Quaternion { get; }
    public Rotation3D(Quaternion quaternion) => (Quaternion) = (quaternion);
    public Rotation3D() { }
    public static Rotation3D New(Quaternion quaternion) => new Rotation3D(quaternion);
    public static implicit operator Quaternion(Rotation3D self) => self.Quaternion;
    public static implicit operator Rotation3D(Quaternion value) => new Rotation3D(value);
    public Rotation3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Vector2D: Vector<Vector2D, Number>
{
    public Number X { get; }
    public Number Y { get; }
    public Vector2D(Number x, Number y) => (X, Y) = (x, y);
    public Vector2D() { }
    public static Vector2D New(Number x, Number y) => new Vector2D(x, y);
    public static implicit operator (Number, Number)(Vector2D self) => (self.X, self.Y);
    public static implicit operator Vector2D((Number, Number) value) => new Vector2D(value.Item1, value.Item2);
    public Integer Count() => throw new NotImplementedException();
    public Number At(Integer n) => throw new NotImplementedException();
    public Number this[Integer n] => throw new NotImplementedException();public Vector2D Zero() => throw new NotImplementedException();
    public Vector2D One() => throw new NotImplementedException();
    public Vector2D MinValue() => throw new NotImplementedException();
    public Vector2D MaxValue() => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Vector2D y) => throw new NotImplementedException();
}
public class Vector3D: Vector<Vector3D, Number>
{
    public Number X { get; }
    public Number Y { get; }
    public Number Z { get; }
    public Vector3D(Number x, Number y, Number z) => (X, Y, Z) = (x, y, z);
    public Vector3D() { }
    public static Vector3D New(Number x, Number y, Number z) => new Vector3D(x, y, z);
    public static implicit operator (Number, Number, Number)(Vector3D self) => (self.X, self.Y, self.Z);
    public static implicit operator Vector3D((Number, Number, Number) value) => new Vector3D(value.Item1, value.Item2, value.Item3);
    public Integer Count() => throw new NotImplementedException();
    public Number At(Integer n) => throw new NotImplementedException();
    public Number this[Integer n] => throw new NotImplementedException();public Vector3D Zero() => throw new NotImplementedException();
    public Vector3D One() => throw new NotImplementedException();
    public Vector3D MinValue() => throw new NotImplementedException();
    public Vector3D MaxValue() => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Vector3D y) => throw new NotImplementedException();
}
public class Vector4D: Vector<Vector4D, Number>
{
    public Number X { get; }
    public Number Y { get; }
    public Number Z { get; }
    public Number W { get; }
    public Vector4D(Number x, Number y, Number z, Number w) => (X, Y, Z, W) = (x, y, z, w);
    public Vector4D() { }
    public static Vector4D New(Number x, Number y, Number z, Number w) => new Vector4D(x, y, z, w);
    public static implicit operator (Number, Number, Number, Number)(Vector4D self) => (self.X, self.Y, self.Z, self.W);
    public static implicit operator Vector4D((Number, Number, Number, Number) value) => new Vector4D(value.Item1, value.Item2, value.Item3, value.Item4);
    public Integer Count() => throw new NotImplementedException();
    public Number At(Integer n) => throw new NotImplementedException();
    public Number this[Integer n] => throw new NotImplementedException();public Vector4D Zero() => throw new NotImplementedException();
    public Vector4D One() => throw new NotImplementedException();
    public Vector4D MinValue() => throw new NotImplementedException();
    public Vector4D MaxValue() => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Vector4D y) => throw new NotImplementedException();
}
public class Orientation3D: Value<Orientation3D>
{
    public Rotation3D Value { get; }
    public Orientation3D(Rotation3D value) => (Value) = (value);
    public Orientation3D() { }
    public static Orientation3D New(Rotation3D value) => new Orientation3D(value);
    public static implicit operator Rotation3D(Orientation3D self) => self.Value;
    public static implicit operator Orientation3D(Rotation3D value) => new Orientation3D(value);
    public Orientation3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Pose2D: Value<Pose2D>
{
    public Vector3D Position { get; }
    public Orientation3D Orientation { get; }
    public Pose2D(Vector3D position, Orientation3D orientation) => (Position, Orientation) = (position, orientation);
    public Pose2D() { }
    public static Pose2D New(Vector3D position, Orientation3D orientation) => new Pose2D(position, orientation);
    public static implicit operator (Vector3D, Orientation3D)(Pose2D self) => (self.Position, self.Orientation);
    public static implicit operator Pose2D((Vector3D, Orientation3D) value) => new Pose2D(value.Item1, value.Item2);
    public Pose2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Pose3D: Value<Pose3D>
{
    public Vector3D Position { get; }
    public Orientation3D Orientation { get; }
    public Pose3D(Vector3D position, Orientation3D orientation) => (Position, Orientation) = (position, orientation);
    public Pose3D() { }
    public static Pose3D New(Vector3D position, Orientation3D orientation) => new Pose3D(position, orientation);
    public static implicit operator (Vector3D, Orientation3D)(Pose3D self) => (self.Position, self.Orientation);
    public static implicit operator Pose3D((Vector3D, Orientation3D) value) => new Pose3D(value.Item1, value.Item2);
    public Pose3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Transform3D: Value<Transform3D>
{
    public Vector3D Translation { get; }
    public Rotation3D Rotation { get; }
    public Vector3D Scale { get; }
    public Transform3D(Vector3D translation, Rotation3D rotation, Vector3D scale) => (Translation, Rotation, Scale) = (translation, rotation, scale);
    public Transform3D() { }
    public static Transform3D New(Vector3D translation, Rotation3D rotation, Vector3D scale) => new Transform3D(translation, rotation, scale);
    public static implicit operator (Vector3D, Rotation3D, Vector3D)(Transform3D self) => (self.Translation, self.Rotation, self.Scale);
    public static implicit operator Transform3D((Vector3D, Rotation3D, Vector3D) value) => new Transform3D(value.Item1, value.Item2, value.Item3);
    public Transform3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Transform2D: Value<Transform2D>
{
    public Vector2D Translation { get; }
    public Angle Rotation { get; }
    public Vector2D Scale { get; }
    public Transform2D(Vector2D translation, Angle rotation, Vector2D scale) => (Translation, Rotation, Scale) = (translation, rotation, scale);
    public Transform2D() { }
    public static Transform2D New(Vector2D translation, Angle rotation, Vector2D scale) => new Transform2D(translation, rotation, scale);
    public static implicit operator (Vector2D, Angle, Vector2D)(Transform2D self) => (self.Translation, self.Rotation, self.Scale);
    public static implicit operator Transform2D((Vector2D, Angle, Vector2D) value) => new Transform2D(value.Item1, value.Item2, value.Item3);
    public Transform2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class AlignedBox2D: Interval<AlignedBox2D, Vector2D>
{
    public Vector2D A { get; }
    public Vector2D B { get; }
    public AlignedBox2D(Vector2D a, Vector2D b) => (A, B) = (a, b);
    public AlignedBox2D() { }
    public static AlignedBox2D New(Vector2D a, Vector2D b) => new AlignedBox2D(a, b);
    public static implicit operator (Vector2D, Vector2D)(AlignedBox2D self) => (self.A, self.B);
    public static implicit operator AlignedBox2D((Vector2D, Vector2D) value) => new AlignedBox2D(value.Item1, value.Item2);
    public Vector2D Min() => throw new NotImplementedException();
    public Vector2D Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class AlignedBox3D: Interval<AlignedBox3D, Vector3D>
{
    public Vector3D A { get; }
    public Vector3D B { get; }
    public AlignedBox3D(Vector3D a, Vector3D b) => (A, B) = (a, b);
    public AlignedBox3D() { }
    public static AlignedBox3D New(Vector3D a, Vector3D b) => new AlignedBox3D(a, b);
    public static implicit operator (Vector3D, Vector3D)(AlignedBox3D self) => (self.A, self.B);
    public static implicit operator AlignedBox3D((Vector3D, Vector3D) value) => new AlignedBox3D(value.Item1, value.Item2);
    public Vector3D Min() => throw new NotImplementedException();
    public Vector3D Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class Complex: Vector<Complex, Number>
{
    public Number Real { get; }
    public Number Imaginary { get; }
    public Complex(Number real, Number imaginary) => (Real, Imaginary) = (real, imaginary);
    public Complex() { }
    public static Complex New(Number real, Number imaginary) => new Complex(real, imaginary);
    public static implicit operator (Number, Number)(Complex self) => (self.Real, self.Imaginary);
    public static implicit operator Complex((Number, Number) value) => new Complex(value.Item1, value.Item2);
    public Integer Count() => throw new NotImplementedException();
    public Number At(Integer n) => throw new NotImplementedException();
    public Number this[Integer n] => throw new NotImplementedException();public Complex Zero() => throw new NotImplementedException();
    public Complex One() => throw new NotImplementedException();
    public Complex MinValue() => throw new NotImplementedException();
    public Complex MaxValue() => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Complex y) => throw new NotImplementedException();
}
public class Ray3D: Value<Ray3D>
{
    public Vector3D Direction { get; }
    public Point3D Position { get; }
    public Ray3D(Vector3D direction, Point3D position) => (Direction, Position) = (direction, position);
    public Ray3D() { }
    public static Ray3D New(Vector3D direction, Point3D position) => new Ray3D(direction, position);
    public static implicit operator (Vector3D, Point3D)(Ray3D self) => (self.Direction, self.Position);
    public static implicit operator Ray3D((Vector3D, Point3D) value) => new Ray3D(value.Item1, value.Item2);
    public Ray3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Ray2D: Value<Ray2D>
{
    public Vector2D Direction { get; }
    public Point2D Position { get; }
    public Ray2D(Vector2D direction, Point2D position) => (Direction, Position) = (direction, position);
    public Ray2D() { }
    public static Ray2D New(Vector2D direction, Point2D position) => new Ray2D(direction, position);
    public static implicit operator (Vector2D, Point2D)(Ray2D self) => (self.Direction, self.Position);
    public static implicit operator Ray2D((Vector2D, Point2D) value) => new Ray2D(value.Item1, value.Item2);
    public Ray2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Sphere: Value<Sphere>
{
    public Point3D Center { get; }
    public Number Radius { get; }
    public Sphere(Point3D center, Number radius) => (Center, Radius) = (center, radius);
    public Sphere() { }
    public static Sphere New(Point3D center, Number radius) => new Sphere(center, radius);
    public static implicit operator (Point3D, Number)(Sphere self) => (self.Center, self.Radius);
    public static implicit operator Sphere((Point3D, Number) value) => new Sphere(value.Item1, value.Item2);
    public Sphere Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Plane: Value<Plane>
{
    public Unit3D Normal { get; }
    public Number D { get; }
    public Plane(Unit3D normal, Number d) => (Normal, D) = (normal, d);
    public Plane() { }
    public static Plane New(Unit3D normal, Number d) => new Plane(normal, d);
    public static implicit operator (Unit3D, Number)(Plane self) => (self.Normal, self.D);
    public static implicit operator Plane((Unit3D, Number) value) => new Plane(value.Item1, value.Item2);
    public Plane Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Triangle3D: Value<Triangle3D>
{
    public Point3D A { get; }
    public Point3D B { get; }
    public Point3D C { get; }
    public Triangle3D(Point3D a, Point3D b, Point3D c) => (A, B, C) = (a, b, c);
    public Triangle3D() { }
    public static Triangle3D New(Point3D a, Point3D b, Point3D c) => new Triangle3D(a, b, c);
    public static implicit operator (Point3D, Point3D, Point3D)(Triangle3D self) => (self.A, self.B, self.C);
    public static implicit operator Triangle3D((Point3D, Point3D, Point3D) value) => new Triangle3D(value.Item1, value.Item2, value.Item3);
    public Triangle3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Triangle2D: Value<Triangle2D>
{
    public Point2D A { get; }
    public Point2D B { get; }
    public Point2D C { get; }
    public Triangle2D(Point2D a, Point2D b, Point2D c) => (A, B, C) = (a, b, c);
    public Triangle2D() { }
    public static Triangle2D New(Point2D a, Point2D b, Point2D c) => new Triangle2D(a, b, c);
    public static implicit operator (Point2D, Point2D, Point2D)(Triangle2D self) => (self.A, self.B, self.C);
    public static implicit operator Triangle2D((Point2D, Point2D, Point2D) value) => new Triangle2D(value.Item1, value.Item2, value.Item3);
    public Triangle2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Quad3D: Value<Quad3D>
{
    public Point3D A { get; }
    public Point3D B { get; }
    public Point3D C { get; }
    public Point3D D { get; }
    public Quad3D(Point3D a, Point3D b, Point3D c, Point3D d) => (A, B, C, D) = (a, b, c, d);
    public Quad3D() { }
    public static Quad3D New(Point3D a, Point3D b, Point3D c, Point3D d) => new Quad3D(a, b, c, d);
    public static implicit operator (Point3D, Point3D, Point3D, Point3D)(Quad3D self) => (self.A, self.B, self.C, self.D);
    public static implicit operator Quad3D((Point3D, Point3D, Point3D, Point3D) value) => new Quad3D(value.Item1, value.Item2, value.Item3, value.Item4);
    public Quad3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Quad2D: Value<Quad2D>
{
    public Point2D A { get; }
    public Point2D B { get; }
    public Point2D C { get; }
    public Point2D D { get; }
    public Quad2D(Point2D a, Point2D b, Point2D c, Point2D d) => (A, B, C, D) = (a, b, c, d);
    public Quad2D() { }
    public static Quad2D New(Point2D a, Point2D b, Point2D c, Point2D d) => new Quad2D(a, b, c, d);
    public static implicit operator (Point2D, Point2D, Point2D, Point2D)(Quad2D self) => (self.A, self.B, self.C, self.D);
    public static implicit operator Quad2D((Point2D, Point2D, Point2D, Point2D) value) => new Quad2D(value.Item1, value.Item2, value.Item3, value.Item4);
    public Quad2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Point3D: Value<Point3D>
{
    public Vector3D Value { get; }
    public Point3D(Vector3D value) => (Value) = (value);
    public Point3D() { }
    public static Point3D New(Vector3D value) => new Point3D(value);
    public static implicit operator Vector3D(Point3D self) => self.Value;
    public static implicit operator Point3D(Vector3D value) => new Point3D(value);
    public Point3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Point2D: Value<Point2D>
{
    public Number X { get; }
    public Number Y { get; }
    public Point2D(Number x, Number y) => (X, Y) = (x, y);
    public Point2D() { }
    public static Point2D New(Number x, Number y) => new Point2D(x, y);
    public static implicit operator (Number, Number)(Point2D self) => (self.X, self.Y);
    public static implicit operator Point2D((Number, Number) value) => new Point2D(value.Item1, value.Item2);
    public Point2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Line3D: Interval<Line3D, Point3D>
{
    public Point3D A { get; }
    public Point3D B { get; }
    public Line3D(Point3D a, Point3D b) => (A, B) = (a, b);
    public Line3D() { }
    public static Line3D New(Point3D a, Point3D b) => new Line3D(a, b);
    public static implicit operator (Point3D, Point3D)(Line3D self) => (self.A, self.B);
    public static implicit operator Line3D((Point3D, Point3D) value) => new Line3D(value.Item1, value.Item2);
    public Point3D Min() => throw new NotImplementedException();
    public Point3D Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class Line2D: Interval<Line2D, Point2D>
{
    public Point2D A { get; }
    public Point2D B { get; }
    public Line2D(Point2D a, Point2D b) => (A, B) = (a, b);
    public Line2D() { }
    public static Line2D New(Point2D a, Point2D b) => new Line2D(a, b);
    public static implicit operator (Point2D, Point2D)(Line2D self) => (self.A, self.B);
    public static implicit operator Line2D((Point2D, Point2D) value) => new Line2D(value.Item1, value.Item2);
    public Point2D Min() => throw new NotImplementedException();
    public Point2D Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class Color: Value<Color>
{
    public Unit R { get; }
    public Unit G { get; }
    public Unit B { get; }
    public Unit A { get; }
    public Color(Unit r, Unit g, Unit b, Unit a) => (R, G, B, A) = (r, g, b, a);
    public Color() { }
    public static Color New(Unit r, Unit g, Unit b, Unit a) => new Color(r, g, b, a);
    public static implicit operator (Unit, Unit, Unit, Unit)(Color self) => (self.R, self.G, self.B, self.A);
    public static implicit operator Color((Unit, Unit, Unit, Unit) value) => new Color(value.Item1, value.Item2, value.Item3, value.Item4);
    public Color Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ColorLUV: Value<ColorLUV>
{
    public Percent Lightness { get; }
    public Unit U { get; }
    public Unit V { get; }
    public ColorLUV(Percent lightness, Unit u, Unit v) => (Lightness, U, V) = (lightness, u, v);
    public ColorLUV() { }
    public static ColorLUV New(Percent lightness, Unit u, Unit v) => new ColorLUV(lightness, u, v);
    public static implicit operator (Percent, Unit, Unit)(ColorLUV self) => (self.Lightness, self.U, self.V);
    public static implicit operator ColorLUV((Percent, Unit, Unit) value) => new ColorLUV(value.Item1, value.Item2, value.Item3);
    public ColorLUV Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ColorLAB: Value<ColorLAB>
{
    public Percent Lightness { get; }
    public Integer A { get; }
    public Integer B { get; }
    public ColorLAB(Percent lightness, Integer a, Integer b) => (Lightness, A, B) = (lightness, a, b);
    public ColorLAB() { }
    public static ColorLAB New(Percent lightness, Integer a, Integer b) => new ColorLAB(lightness, a, b);
    public static implicit operator (Percent, Integer, Integer)(ColorLAB self) => (self.Lightness, self.A, self.B);
    public static implicit operator ColorLAB((Percent, Integer, Integer) value) => new ColorLAB(value.Item1, value.Item2, value.Item3);
    public ColorLAB Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ColorLCh: Value<ColorLCh>
{
    public Percent Lightness { get; }
    public PolarCoordinate ChromaHue { get; }
    public ColorLCh(Percent lightness, PolarCoordinate chromaHue) => (Lightness, ChromaHue) = (lightness, chromaHue);
    public ColorLCh() { }
    public static ColorLCh New(Percent lightness, PolarCoordinate chromaHue) => new ColorLCh(lightness, chromaHue);
    public static implicit operator (Percent, PolarCoordinate)(ColorLCh self) => (self.Lightness, self.ChromaHue);
    public static implicit operator ColorLCh((Percent, PolarCoordinate) value) => new ColorLCh(value.Item1, value.Item2);
    public ColorLCh Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ColorHSV: Value<ColorHSV>
{
    public Angle Hue { get; }
    public Unit S { get; }
    public Unit V { get; }
    public ColorHSV(Angle hue, Unit s, Unit v) => (Hue, S, V) = (hue, s, v);
    public ColorHSV() { }
    public static ColorHSV New(Angle hue, Unit s, Unit v) => new ColorHSV(hue, s, v);
    public static implicit operator (Angle, Unit, Unit)(ColorHSV self) => (self.Hue, self.S, self.V);
    public static implicit operator ColorHSV((Angle, Unit, Unit) value) => new ColorHSV(value.Item1, value.Item2, value.Item3);
    public ColorHSV Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ColorHSL: Value<ColorHSL>
{
    public Angle Hue { get; }
    public Unit Saturation { get; }
    public Unit Luminance { get; }
    public ColorHSL(Angle hue, Unit saturation, Unit luminance) => (Hue, Saturation, Luminance) = (hue, saturation, luminance);
    public ColorHSL() { }
    public static ColorHSL New(Angle hue, Unit saturation, Unit luminance) => new ColorHSL(hue, saturation, luminance);
    public static implicit operator (Angle, Unit, Unit)(ColorHSL self) => (self.Hue, self.Saturation, self.Luminance);
    public static implicit operator ColorHSL((Angle, Unit, Unit) value) => new ColorHSL(value.Item1, value.Item2, value.Item3);
    public ColorHSL Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ColorYCbCr: Value<ColorYCbCr>
{
    public Unit Y { get; }
    public Unit Cb { get; }
    public Unit Cr { get; }
    public ColorYCbCr(Unit y, Unit cb, Unit cr) => (Y, Cb, Cr) = (y, cb, cr);
    public ColorYCbCr() { }
    public static ColorYCbCr New(Unit y, Unit cb, Unit cr) => new ColorYCbCr(y, cb, cr);
    public static implicit operator (Unit, Unit, Unit)(ColorYCbCr self) => (self.Y, self.Cb, self.Cr);
    public static implicit operator ColorYCbCr((Unit, Unit, Unit) value) => new ColorYCbCr(value.Item1, value.Item2, value.Item3);
    public ColorYCbCr Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class SphericalCoordinate: Value<SphericalCoordinate>
{
    public Number Radius { get; }
    public Angle Azimuth { get; }
    public Angle Polar { get; }
    public SphericalCoordinate(Number radius, Angle azimuth, Angle polar) => (Radius, Azimuth, Polar) = (radius, azimuth, polar);
    public SphericalCoordinate() { }
    public static SphericalCoordinate New(Number radius, Angle azimuth, Angle polar) => new SphericalCoordinate(radius, azimuth, polar);
    public static implicit operator (Number, Angle, Angle)(SphericalCoordinate self) => (self.Radius, self.Azimuth, self.Polar);
    public static implicit operator SphericalCoordinate((Number, Angle, Angle) value) => new SphericalCoordinate(value.Item1, value.Item2, value.Item3);
    public SphericalCoordinate Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class PolarCoordinate: Value<PolarCoordinate>
{
    public Number Radius { get; }
    public Angle Angle { get; }
    public PolarCoordinate(Number radius, Angle angle) => (Radius, Angle) = (radius, angle);
    public PolarCoordinate() { }
    public static PolarCoordinate New(Number radius, Angle angle) => new PolarCoordinate(radius, angle);
    public static implicit operator (Number, Angle)(PolarCoordinate self) => (self.Radius, self.Angle);
    public static implicit operator PolarCoordinate((Number, Angle) value) => new PolarCoordinate(value.Item1, value.Item2);
    public PolarCoordinate Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class LogPolarCoordinate: Value<LogPolarCoordinate>
{
    public Number Rho { get; }
    public Angle Azimuth { get; }
    public LogPolarCoordinate(Number rho, Angle azimuth) => (Rho, Azimuth) = (rho, azimuth);
    public LogPolarCoordinate() { }
    public static LogPolarCoordinate New(Number rho, Angle azimuth) => new LogPolarCoordinate(rho, azimuth);
    public static implicit operator (Number, Angle)(LogPolarCoordinate self) => (self.Rho, self.Azimuth);
    public static implicit operator LogPolarCoordinate((Number, Angle) value) => new LogPolarCoordinate(value.Item1, value.Item2);
    public LogPolarCoordinate Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class CylindricalCoordinate: Value<CylindricalCoordinate>
{
    public Number RadialDistance { get; }
    public Angle Azimuth { get; }
    public Number Height { get; }
    public CylindricalCoordinate(Number radialDistance, Angle azimuth, Number height) => (RadialDistance, Azimuth, Height) = (radialDistance, azimuth, height);
    public CylindricalCoordinate() { }
    public static CylindricalCoordinate New(Number radialDistance, Angle azimuth, Number height) => new CylindricalCoordinate(radialDistance, azimuth, height);
    public static implicit operator (Number, Angle, Number)(CylindricalCoordinate self) => (self.RadialDistance, self.Azimuth, self.Height);
    public static implicit operator CylindricalCoordinate((Number, Angle, Number) value) => new CylindricalCoordinate(value.Item1, value.Item2, value.Item3);
    public CylindricalCoordinate Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class HorizontalCoordinate: Value<HorizontalCoordinate>
{
    public Number Radius { get; }
    public Angle Azimuth { get; }
    public Number Height { get; }
    public HorizontalCoordinate(Number radius, Angle azimuth, Number height) => (Radius, Azimuth, Height) = (radius, azimuth, height);
    public HorizontalCoordinate() { }
    public static HorizontalCoordinate New(Number radius, Angle azimuth, Number height) => new HorizontalCoordinate(radius, azimuth, height);
    public static implicit operator (Number, Angle, Number)(HorizontalCoordinate self) => (self.Radius, self.Azimuth, self.Height);
    public static implicit operator HorizontalCoordinate((Number, Angle, Number) value) => new HorizontalCoordinate(value.Item1, value.Item2, value.Item3);
    public HorizontalCoordinate Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class GeoCoordinate: Value<GeoCoordinate>
{
    public Angle Latitude { get; }
    public Angle Longitude { get; }
    public GeoCoordinate(Angle latitude, Angle longitude) => (Latitude, Longitude) = (latitude, longitude);
    public GeoCoordinate() { }
    public static GeoCoordinate New(Angle latitude, Angle longitude) => new GeoCoordinate(latitude, longitude);
    public static implicit operator (Angle, Angle)(GeoCoordinate self) => (self.Latitude, self.Longitude);
    public static implicit operator GeoCoordinate((Angle, Angle) value) => new GeoCoordinate(value.Item1, value.Item2);
    public GeoCoordinate Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class GeoCoordinateWithAltitude: Value<GeoCoordinateWithAltitude>
{
    public GeoCoordinate Coordinate { get; }
    public Number Altitude { get; }
    public GeoCoordinateWithAltitude(GeoCoordinate coordinate, Number altitude) => (Coordinate, Altitude) = (coordinate, altitude);
    public GeoCoordinateWithAltitude() { }
    public static GeoCoordinateWithAltitude New(GeoCoordinate coordinate, Number altitude) => new GeoCoordinateWithAltitude(coordinate, altitude);
    public static implicit operator (GeoCoordinate, Number)(GeoCoordinateWithAltitude self) => (self.Coordinate, self.Altitude);
    public static implicit operator GeoCoordinateWithAltitude((GeoCoordinate, Number) value) => new GeoCoordinateWithAltitude(value.Item1, value.Item2);
    public GeoCoordinateWithAltitude Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Circle: Value<Circle>
{
    public Point2D Center { get; }
    public Number Radius { get; }
    public Circle(Point2D center, Number radius) => (Center, Radius) = (center, radius);
    public Circle() { }
    public static Circle New(Point2D center, Number radius) => new Circle(center, radius);
    public static implicit operator (Point2D, Number)(Circle self) => (self.Center, self.Radius);
    public static implicit operator Circle((Point2D, Number) value) => new Circle(value.Item1, value.Item2);
    public Circle Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Chord: Value<Chord>
{
    public Circle Circle { get; }
    public Arc Arc { get; }
    public Chord(Circle circle, Arc arc) => (Circle, Arc) = (circle, arc);
    public Chord() { }
    public static Chord New(Circle circle, Arc arc) => new Chord(circle, arc);
    public static implicit operator (Circle, Arc)(Chord self) => (self.Circle, self.Arc);
    public static implicit operator Chord((Circle, Arc) value) => new Chord(value.Item1, value.Item2);
    public Chord Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Size2D: Value<Size2D>
{
    public Number Width { get; }
    public Number Height { get; }
    public Size2D(Number width, Number height) => (Width, Height) = (width, height);
    public Size2D() { }
    public static Size2D New(Number width, Number height) => new Size2D(width, height);
    public static implicit operator (Number, Number)(Size2D self) => (self.Width, self.Height);
    public static implicit operator Size2D((Number, Number) value) => new Size2D(value.Item1, value.Item2);
    public Size2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Size3D: Value<Size3D>
{
    public Number Width { get; }
    public Number Height { get; }
    public Number Depth { get; }
    public Size3D(Number width, Number height, Number depth) => (Width, Height, Depth) = (width, height, depth);
    public Size3D() { }
    public static Size3D New(Number width, Number height, Number depth) => new Size3D(width, height, depth);
    public static implicit operator (Number, Number, Number)(Size3D self) => (self.Width, self.Height, self.Depth);
    public static implicit operator Size3D((Number, Number, Number) value) => new Size3D(value.Item1, value.Item2, value.Item3);
    public Size3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Rectangle2D: Value<Rectangle2D>
{
    public Point2D Center { get; }
    public Size2D Size { get; }
    public Rectangle2D(Point2D center, Size2D size) => (Center, Size) = (center, size);
    public Rectangle2D() { }
    public static Rectangle2D New(Point2D center, Size2D size) => new Rectangle2D(center, size);
    public static implicit operator (Point2D, Size2D)(Rectangle2D self) => (self.Center, self.Size);
    public static implicit operator Rectangle2D((Point2D, Size2D) value) => new Rectangle2D(value.Item1, value.Item2);
    public Rectangle2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Proportion: Numerical<Proportion>
{
    public Number Value { get; }
    public Proportion(Number value) => (Value) = (value);
    public Proportion() { }
    public static Proportion New(Number value) => new Proportion(value);
    public static implicit operator Number(Proportion self) => self.Value;
    public static implicit operator Proportion(Number value) => new Proportion(value);
    public Proportion Zero() => throw new NotImplementedException();
    public Proportion One() => throw new NotImplementedException();
    public Proportion MinValue() => throw new NotImplementedException();
    public Proportion MaxValue() => throw new NotImplementedException();
    public Proportion Default() => throw new NotImplementedException();
    public Proportion Add(Proportion other) => throw new NotImplementedException();
    public static Proportion operator +(Proportion self, Proportion other) => throw new NotImplementedException();
    public Proportion Subtract(Proportion other) => throw new NotImplementedException();
    public static Proportion operator -(Proportion self, Proportion other) => throw new NotImplementedException();
    public Proportion Negative() => throw new NotImplementedException();
    public static Proportion operator -(Proportion self) => throw new NotImplementedException();
    public Proportion Reciprocal() => throw new NotImplementedException();
    public Proportion Multiply(Proportion other) => throw new NotImplementedException();
    public static Proportion operator *(Proportion self, Proportion other) => throw new NotImplementedException();
    public Proportion Divide(Proportion other) => throw new NotImplementedException();
    public static Proportion operator /(Proportion self, Proportion other) => throw new NotImplementedException();
    public Proportion Modulo(Proportion other) => throw new NotImplementedException();
    public static Proportion operator %(Proportion self, Proportion other) => throw new NotImplementedException();
    public Proportion Add(Number scalar) => throw new NotImplementedException();
    public static Proportion operator +(Proportion self, Number scalar) => throw new NotImplementedException();
    public Proportion Subtract(Number scalar) => throw new NotImplementedException();
    public static Proportion operator -(Proportion self, Number scalar) => throw new NotImplementedException();
    public Proportion Multiply(Number scalar) => throw new NotImplementedException();
    public static Proportion operator *(Proportion self, Number scalar) => throw new NotImplementedException();
    public Proportion Divide(Number scalar) => throw new NotImplementedException();
    public static Proportion operator /(Proportion self, Number scalar) => throw new NotImplementedException();
    public Proportion Modulo(Number scalar) => throw new NotImplementedException();
    public static Proportion operator %(Proportion self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Proportion b) => throw new NotImplementedException();
    public static Boolean operator ==(Proportion a, Proportion b) => throw new NotImplementedException();
    public Boolean NotEquals(Proportion b) => throw new NotImplementedException();
    public static Boolean operator !=(Proportion a, Proportion b) => throw new NotImplementedException();
    public Integer Compare(Proportion y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Proportion y) => throw new NotImplementedException();
}
public class Fraction: Value<Fraction>
{
    public Number Numerator { get; }
    public Number Denominator { get; }
    public Fraction(Number numerator, Number denominator) => (Numerator, Denominator) = (numerator, denominator);
    public Fraction() { }
    public static Fraction New(Number numerator, Number denominator) => new Fraction(numerator, denominator);
    public static implicit operator (Number, Number)(Fraction self) => (self.Numerator, self.Denominator);
    public static implicit operator Fraction((Number, Number) value) => new Fraction(value.Item1, value.Item2);
    public Fraction Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Angle: Measure<Angle>
{
    public Number Radians { get; }
    public Angle(Number radians) => (Radians) = (radians);
    public Angle() { }
    public static Angle New(Number radians) => new Angle(radians);
    public static implicit operator Number(Angle self) => self.Radians;
    public static implicit operator Angle(Number value) => new Angle(value);
    public Number Value() => throw new NotImplementedException();
    public Angle Default() => throw new NotImplementedException();
    public Angle Add(Number scalar) => throw new NotImplementedException();
    public static Angle operator +(Angle self, Number scalar) => throw new NotImplementedException();
    public Angle Subtract(Number scalar) => throw new NotImplementedException();
    public static Angle operator -(Angle self, Number scalar) => throw new NotImplementedException();
    public Angle Multiply(Number scalar) => throw new NotImplementedException();
    public static Angle operator *(Angle self, Number scalar) => throw new NotImplementedException();
    public Angle Divide(Number scalar) => throw new NotImplementedException();
    public static Angle operator /(Angle self, Number scalar) => throw new NotImplementedException();
    public Angle Modulo(Number scalar) => throw new NotImplementedException();
    public static Angle operator %(Angle self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Angle b) => throw new NotImplementedException();
    public static Boolean operator ==(Angle a, Angle b) => throw new NotImplementedException();
    public Boolean NotEquals(Angle b) => throw new NotImplementedException();
    public static Boolean operator !=(Angle a, Angle b) => throw new NotImplementedException();
    public Integer Compare(Angle y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Angle y) => throw new NotImplementedException();
}
public class Length: Measure<Length>
{
    public Number Meters { get; }
    public Length(Number meters) => (Meters) = (meters);
    public Length() { }
    public static Length New(Number meters) => new Length(meters);
    public static implicit operator Number(Length self) => self.Meters;
    public static implicit operator Length(Number value) => new Length(value);
    public Number Value() => throw new NotImplementedException();
    public Length Default() => throw new NotImplementedException();
    public Length Add(Number scalar) => throw new NotImplementedException();
    public static Length operator +(Length self, Number scalar) => throw new NotImplementedException();
    public Length Subtract(Number scalar) => throw new NotImplementedException();
    public static Length operator -(Length self, Number scalar) => throw new NotImplementedException();
    public Length Multiply(Number scalar) => throw new NotImplementedException();
    public static Length operator *(Length self, Number scalar) => throw new NotImplementedException();
    public Length Divide(Number scalar) => throw new NotImplementedException();
    public static Length operator /(Length self, Number scalar) => throw new NotImplementedException();
    public Length Modulo(Number scalar) => throw new NotImplementedException();
    public static Length operator %(Length self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Length b) => throw new NotImplementedException();
    public static Boolean operator ==(Length a, Length b) => throw new NotImplementedException();
    public Boolean NotEquals(Length b) => throw new NotImplementedException();
    public static Boolean operator !=(Length a, Length b) => throw new NotImplementedException();
    public Integer Compare(Length y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Length y) => throw new NotImplementedException();
}
public class Mass: Measure<Mass>
{
    public Number Kilograms { get; }
    public Mass(Number kilograms) => (Kilograms) = (kilograms);
    public Mass() { }
    public static Mass New(Number kilograms) => new Mass(kilograms);
    public static implicit operator Number(Mass self) => self.Kilograms;
    public static implicit operator Mass(Number value) => new Mass(value);
    public Number Value() => throw new NotImplementedException();
    public Mass Default() => throw new NotImplementedException();
    public Mass Add(Number scalar) => throw new NotImplementedException();
    public static Mass operator +(Mass self, Number scalar) => throw new NotImplementedException();
    public Mass Subtract(Number scalar) => throw new NotImplementedException();
    public static Mass operator -(Mass self, Number scalar) => throw new NotImplementedException();
    public Mass Multiply(Number scalar) => throw new NotImplementedException();
    public static Mass operator *(Mass self, Number scalar) => throw new NotImplementedException();
    public Mass Divide(Number scalar) => throw new NotImplementedException();
    public static Mass operator /(Mass self, Number scalar) => throw new NotImplementedException();
    public Mass Modulo(Number scalar) => throw new NotImplementedException();
    public static Mass operator %(Mass self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Mass b) => throw new NotImplementedException();
    public static Boolean operator ==(Mass a, Mass b) => throw new NotImplementedException();
    public Boolean NotEquals(Mass b) => throw new NotImplementedException();
    public static Boolean operator !=(Mass a, Mass b) => throw new NotImplementedException();
    public Integer Compare(Mass y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Mass y) => throw new NotImplementedException();
}
public class Temperature: Measure<Temperature>
{
    public Number Celsius { get; }
    public Temperature(Number celsius) => (Celsius) = (celsius);
    public Temperature() { }
    public static Temperature New(Number celsius) => new Temperature(celsius);
    public static implicit operator Number(Temperature self) => self.Celsius;
    public static implicit operator Temperature(Number value) => new Temperature(value);
    public Number Value() => throw new NotImplementedException();
    public Temperature Default() => throw new NotImplementedException();
    public Temperature Add(Number scalar) => throw new NotImplementedException();
    public static Temperature operator +(Temperature self, Number scalar) => throw new NotImplementedException();
    public Temperature Subtract(Number scalar) => throw new NotImplementedException();
    public static Temperature operator -(Temperature self, Number scalar) => throw new NotImplementedException();
    public Temperature Multiply(Number scalar) => throw new NotImplementedException();
    public static Temperature operator *(Temperature self, Number scalar) => throw new NotImplementedException();
    public Temperature Divide(Number scalar) => throw new NotImplementedException();
    public static Temperature operator /(Temperature self, Number scalar) => throw new NotImplementedException();
    public Temperature Modulo(Number scalar) => throw new NotImplementedException();
    public static Temperature operator %(Temperature self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Temperature b) => throw new NotImplementedException();
    public static Boolean operator ==(Temperature a, Temperature b) => throw new NotImplementedException();
    public Boolean NotEquals(Temperature b) => throw new NotImplementedException();
    public static Boolean operator !=(Temperature a, Temperature b) => throw new NotImplementedException();
    public Integer Compare(Temperature y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Temperature y) => throw new NotImplementedException();
}
public class TimeSpan: Measure<TimeSpan>
{
    public Number Seconds { get; }
    public TimeSpan(Number seconds) => (Seconds) = (seconds);
    public TimeSpan() { }
    public static TimeSpan New(Number seconds) => new TimeSpan(seconds);
    public static implicit operator Number(TimeSpan self) => self.Seconds;
    public static implicit operator TimeSpan(Number value) => new TimeSpan(value);
    public Number Value() => throw new NotImplementedException();
    public TimeSpan Default() => throw new NotImplementedException();
    public TimeSpan Add(Number scalar) => throw new NotImplementedException();
    public static TimeSpan operator +(TimeSpan self, Number scalar) => throw new NotImplementedException();
    public TimeSpan Subtract(Number scalar) => throw new NotImplementedException();
    public static TimeSpan operator -(TimeSpan self, Number scalar) => throw new NotImplementedException();
    public TimeSpan Multiply(Number scalar) => throw new NotImplementedException();
    public static TimeSpan operator *(TimeSpan self, Number scalar) => throw new NotImplementedException();
    public TimeSpan Divide(Number scalar) => throw new NotImplementedException();
    public static TimeSpan operator /(TimeSpan self, Number scalar) => throw new NotImplementedException();
    public TimeSpan Modulo(Number scalar) => throw new NotImplementedException();
    public static TimeSpan operator %(TimeSpan self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(TimeSpan b) => throw new NotImplementedException();
    public static Boolean operator ==(TimeSpan a, TimeSpan b) => throw new NotImplementedException();
    public Boolean NotEquals(TimeSpan b) => throw new NotImplementedException();
    public static Boolean operator !=(TimeSpan a, TimeSpan b) => throw new NotImplementedException();
    public Integer Compare(TimeSpan y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(TimeSpan y) => throw new NotImplementedException();
}
public class TimeRange: Interval<TimeRange, DateTime>
{
    public DateTime Min { get; }
    public DateTime Max { get; }
    public TimeRange(DateTime min, DateTime max) => (Min, Max) = (min, max);
    public TimeRange() { }
    public static TimeRange New(DateTime min, DateTime max) => new TimeRange(min, max);
    public static implicit operator (DateTime, DateTime)(TimeRange self) => (self.Min, self.Max);
    public static implicit operator TimeRange((DateTime, DateTime) value) => new TimeRange(value.Item1, value.Item2);
    public DateTime Min() => throw new NotImplementedException();
    public DateTime Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class DateTime: Value<DateTime>
{
    public Integer Year { get; }
    public Integer Month { get; }
    public Integer TimeZoneOffset { get; }
    public Integer Day { get; }
    public Integer Minute { get; }
    public Integer Second { get; }
    public Number Milliseconds { get; }
    public DateTime(Integer year, Integer month, Integer timeZoneOffset, Integer day, Integer minute, Integer second, Number milliseconds) => (Year, Month, TimeZoneOffset, Day, Minute, Second, Milliseconds) = (year, month, timeZoneOffset, day, minute, second, milliseconds);
    public DateTime() { }
    public static DateTime New(Integer year, Integer month, Integer timeZoneOffset, Integer day, Integer minute, Integer second, Number milliseconds) => new DateTime(year, month, timeZoneOffset, day, minute, second, milliseconds);
    public static implicit operator (Integer, Integer, Integer, Integer, Integer, Integer, Number)(DateTime self) => (self.Year, self.Month, self.TimeZoneOffset, self.Day, self.Minute, self.Second, self.Milliseconds);
    public static implicit operator DateTime((Integer, Integer, Integer, Integer, Integer, Integer, Number) value) => new DateTime(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7);
    public DateTime Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class AnglePair: Interval<AnglePair, Angle>
{
    public Angle Start { get; }
    public Angle End { get; }
    public AnglePair(Angle start, Angle end) => (Start, End) = (start, end);
    public AnglePair() { }
    public static AnglePair New(Angle start, Angle end) => new AnglePair(start, end);
    public static implicit operator (Angle, Angle)(AnglePair self) => (self.Start, self.End);
    public static implicit operator AnglePair((Angle, Angle) value) => new AnglePair(value.Item1, value.Item2);
    public Angle Min() => throw new NotImplementedException();
    public Angle Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class Ring: Numerical<Ring>
{
    public Circle Circle { get; }
    public Number InnerRadius { get; }
    public Ring(Circle circle, Number innerRadius) => (Circle, InnerRadius) = (circle, innerRadius);
    public Ring() { }
    public static Ring New(Circle circle, Number innerRadius) => new Ring(circle, innerRadius);
    public static implicit operator (Circle, Number)(Ring self) => (self.Circle, self.InnerRadius);
    public static implicit operator Ring((Circle, Number) value) => new Ring(value.Item1, value.Item2);
    public Ring Zero() => throw new NotImplementedException();
    public Ring One() => throw new NotImplementedException();
    public Ring MinValue() => throw new NotImplementedException();
    public Ring MaxValue() => throw new NotImplementedException();
    public Ring Default() => throw new NotImplementedException();
    public Ring Add(Ring other) => throw new NotImplementedException();
    public static Ring operator +(Ring self, Ring other) => throw new NotImplementedException();
    public Ring Subtract(Ring other) => throw new NotImplementedException();
    public static Ring operator -(Ring self, Ring other) => throw new NotImplementedException();
    public Ring Negative() => throw new NotImplementedException();
    public static Ring operator -(Ring self) => throw new NotImplementedException();
    public Ring Reciprocal() => throw new NotImplementedException();
    public Ring Multiply(Ring other) => throw new NotImplementedException();
    public static Ring operator *(Ring self, Ring other) => throw new NotImplementedException();
    public Ring Divide(Ring other) => throw new NotImplementedException();
    public static Ring operator /(Ring self, Ring other) => throw new NotImplementedException();
    public Ring Modulo(Ring other) => throw new NotImplementedException();
    public static Ring operator %(Ring self, Ring other) => throw new NotImplementedException();
    public Ring Add(Number scalar) => throw new NotImplementedException();
    public static Ring operator +(Ring self, Number scalar) => throw new NotImplementedException();
    public Ring Subtract(Number scalar) => throw new NotImplementedException();
    public static Ring operator -(Ring self, Number scalar) => throw new NotImplementedException();
    public Ring Multiply(Number scalar) => throw new NotImplementedException();
    public static Ring operator *(Ring self, Number scalar) => throw new NotImplementedException();
    public Ring Divide(Number scalar) => throw new NotImplementedException();
    public static Ring operator /(Ring self, Number scalar) => throw new NotImplementedException();
    public Ring Modulo(Number scalar) => throw new NotImplementedException();
    public static Ring operator %(Ring self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Ring b) => throw new NotImplementedException();
    public static Boolean operator ==(Ring a, Ring b) => throw new NotImplementedException();
    public Boolean NotEquals(Ring b) => throw new NotImplementedException();
    public static Boolean operator !=(Ring a, Ring b) => throw new NotImplementedException();
    public Integer Compare(Ring y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Ring y) => throw new NotImplementedException();
}
public class Arc: Value<Arc>
{
    public AnglePair Angles { get; }
    public Circle Cirlce { get; }
    public Arc(AnglePair angles, Circle cirlce) => (Angles, Cirlce) = (angles, cirlce);
    public Arc() { }
    public static Arc New(AnglePair angles, Circle cirlce) => new Arc(angles, cirlce);
    public static implicit operator (AnglePair, Circle)(Arc self) => (self.Angles, self.Cirlce);
    public static implicit operator Arc((AnglePair, Circle) value) => new Arc(value.Item1, value.Item2);
    public Arc Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class TimeInterval: Interval<TimeInterval, TimeSpan>
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public TimeInterval(TimeSpan start, TimeSpan end) => (Start, End) = (start, end);
    public TimeInterval() { }
    public static TimeInterval New(TimeSpan start, TimeSpan end) => new TimeInterval(start, end);
    public static implicit operator (TimeSpan, TimeSpan)(TimeInterval self) => (self.Start, self.End);
    public static implicit operator TimeInterval((TimeSpan, TimeSpan) value) => new TimeInterval(value.Item1, value.Item2);
    public TimeSpan Min() => throw new NotImplementedException();
    public TimeSpan Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class RealInterval: Interval<RealInterval, Number>
{
    public Number A { get; }
    public Number B { get; }
    public RealInterval(Number a, Number b) => (A, B) = (a, b);
    public RealInterval() { }
    public static RealInterval New(Number a, Number b) => new RealInterval(a, b);
    public static implicit operator (Number, Number)(RealInterval self) => (self.A, self.B);
    public static implicit operator RealInterval((Number, Number) value) => new RealInterval(value.Item1, value.Item2);
    public Number Min() => throw new NotImplementedException();
    public Number Max() => throw new NotImplementedException();
    public Integer Count() => throw new NotImplementedException();
    public Any At(Integer n) => throw new NotImplementedException();
    public Any this[Integer n] => throw new NotImplementedException();}
public class Capsule: Value<Capsule>
{
    public Line3D Line { get; }
    public Number Radius { get; }
    public Capsule(Line3D line, Number radius) => (Line, Radius) = (line, radius);
    public Capsule() { }
    public static Capsule New(Line3D line, Number radius) => new Capsule(line, radius);
    public static implicit operator (Line3D, Number)(Capsule self) => (self.Line, self.Radius);
    public static implicit operator Capsule((Line3D, Number) value) => new Capsule(value.Item1, value.Item2);
    public Capsule Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Matrix3D: Value<Matrix3D>
{
    public Vector4D Column1 { get; }
    public Vector4D Column2 { get; }
    public Vector4D Column3 { get; }
    public Vector4D Column4 { get; }
    public Matrix3D(Vector4D column1, Vector4D column2, Vector4D column3, Vector4D column4) => (Column1, Column2, Column3, Column4) = (column1, column2, column3, column4);
    public Matrix3D() { }
    public static Matrix3D New(Vector4D column1, Vector4D column2, Vector4D column3, Vector4D column4) => new Matrix3D(column1, column2, column3, column4);
    public static implicit operator (Vector4D, Vector4D, Vector4D, Vector4D)(Matrix3D self) => (self.Column1, self.Column2, self.Column3, self.Column4);
    public static implicit operator Matrix3D((Vector4D, Vector4D, Vector4D, Vector4D) value) => new Matrix3D(value.Item1, value.Item2, value.Item3, value.Item4);
    public Matrix3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Cylinder: Value<Cylinder>
{
    public Line3D Line { get; }
    public Number Radius { get; }
    public Cylinder(Line3D line, Number radius) => (Line, Radius) = (line, radius);
    public Cylinder() { }
    public static Cylinder New(Line3D line, Number radius) => new Cylinder(line, radius);
    public static implicit operator (Line3D, Number)(Cylinder self) => (self.Line, self.Radius);
    public static implicit operator Cylinder((Line3D, Number) value) => new Cylinder(value.Item1, value.Item2);
    public Cylinder Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Cone: Value<Cone>
{
    public Line3D Line { get; }
    public Number Radius { get; }
    public Cone(Line3D line, Number radius) => (Line, Radius) = (line, radius);
    public Cone() { }
    public static Cone New(Line3D line, Number radius) => new Cone(line, radius);
    public static implicit operator (Line3D, Number)(Cone self) => (self.Line, self.Radius);
    public static implicit operator Cone((Line3D, Number) value) => new Cone(value.Item1, value.Item2);
    public Cone Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Tube: Value<Tube>
{
    public Line3D Line { get; }
    public Number InnerRadius { get; }
    public Number OuterRadius { get; }
    public Tube(Line3D line, Number innerRadius, Number outerRadius) => (Line, InnerRadius, OuterRadius) = (line, innerRadius, outerRadius);
    public Tube() { }
    public static Tube New(Line3D line, Number innerRadius, Number outerRadius) => new Tube(line, innerRadius, outerRadius);
    public static implicit operator (Line3D, Number, Number)(Tube self) => (self.Line, self.InnerRadius, self.OuterRadius);
    public static implicit operator Tube((Line3D, Number, Number) value) => new Tube(value.Item1, value.Item2, value.Item3);
    public Tube Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class ConeSegment: Value<ConeSegment>
{
    public Line3D Line { get; }
    public Number Radius1 { get; }
    public Number Radius2 { get; }
    public ConeSegment(Line3D line, Number radius1, Number radius2) => (Line, Radius1, Radius2) = (line, radius1, radius2);
    public ConeSegment() { }
    public static ConeSegment New(Line3D line, Number radius1, Number radius2) => new ConeSegment(line, radius1, radius2);
    public static implicit operator (Line3D, Number, Number)(ConeSegment self) => (self.Line, self.Radius1, self.Radius2);
    public static implicit operator ConeSegment((Line3D, Number, Number) value) => new ConeSegment(value.Item1, value.Item2, value.Item3);
    public ConeSegment Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Box2D: Value<Box2D>
{
    public Point2D Center { get; }
    public Angle Rotation { get; }
    public Size2D Extent { get; }
    public Box2D(Point2D center, Angle rotation, Size2D extent) => (Center, Rotation, Extent) = (center, rotation, extent);
    public Box2D() { }
    public static Box2D New(Point2D center, Angle rotation, Size2D extent) => new Box2D(center, rotation, extent);
    public static implicit operator (Point2D, Angle, Size2D)(Box2D self) => (self.Center, self.Rotation, self.Extent);
    public static implicit operator Box2D((Point2D, Angle, Size2D) value) => new Box2D(value.Item1, value.Item2, value.Item3);
    public Box2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Box3D: Value<Box3D>
{
    public Point3D Center { get; }
    public Rotation3D Rotation { get; }
    public Size3D Extent { get; }
    public Box3D(Point3D center, Rotation3D rotation, Size3D extent) => (Center, Rotation, Extent) = (center, rotation, extent);
    public Box3D() { }
    public static Box3D New(Point3D center, Rotation3D rotation, Size3D extent) => new Box3D(center, rotation, extent);
    public static implicit operator (Point3D, Rotation3D, Size3D)(Box3D self) => (self.Center, self.Rotation, self.Extent);
    public static implicit operator Box3D((Point3D, Rotation3D, Size3D) value) => new Box3D(value.Item1, value.Item2, value.Item3);
    public Box3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class CubicBezierTriangle3D: Value<CubicBezierTriangle3D>
{
    public Point3D A { get; }
    public Point3D B { get; }
    public Point3D C { get; }
    public Point3D A2B { get; }
    public Point3D AB2 { get; }
    public Point3D B2C { get; }
    public Point3D BC2 { get; }
    public Point3D AC2 { get; }
    public Point3D A2C { get; }
    public Point3D ABC { get; }
    public CubicBezierTriangle3D(Point3D a, Point3D b, Point3D c, Point3D a2B, Point3D aB2, Point3D b2C, Point3D bC2, Point3D aC2, Point3D a2C, Point3D aBC) => (A, B, C, A2B, AB2, B2C, BC2, AC2, A2C, ABC) = (a, b, c, a2B, aB2, b2C, bC2, aC2, a2C, aBC);
    public CubicBezierTriangle3D() { }
    public static CubicBezierTriangle3D New(Point3D a, Point3D b, Point3D c, Point3D a2B, Point3D aB2, Point3D b2C, Point3D bC2, Point3D aC2, Point3D a2C, Point3D aBC) => new CubicBezierTriangle3D(a, b, c, a2B, aB2, b2C, bC2, aC2, a2C, aBC);
    public static implicit operator (Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D)(CubicBezierTriangle3D self) => (self.A, self.B, self.C, self.A2B, self.AB2, self.B2C, self.BC2, self.AC2, self.A2C, self.ABC);
    public static implicit operator CubicBezierTriangle3D((Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D, Point3D) value) => new CubicBezierTriangle3D(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7, value.Item8, value.Item9, value.Item10);
    public CubicBezierTriangle3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class CubicBezier2D: Value<CubicBezier2D>
{
    public Point2D A { get; }
    public Point2D B { get; }
    public Point2D C { get; }
    public Point2D D { get; }
    public CubicBezier2D(Point2D a, Point2D b, Point2D c, Point2D d) => (A, B, C, D) = (a, b, c, d);
    public CubicBezier2D() { }
    public static CubicBezier2D New(Point2D a, Point2D b, Point2D c, Point2D d) => new CubicBezier2D(a, b, c, d);
    public static implicit operator (Point2D, Point2D, Point2D, Point2D)(CubicBezier2D self) => (self.A, self.B, self.C, self.D);
    public static implicit operator CubicBezier2D((Point2D, Point2D, Point2D, Point2D) value) => new CubicBezier2D(value.Item1, value.Item2, value.Item3, value.Item4);
    public CubicBezier2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class UV: Vector<UV, Unit>
{
    public Unit U { get; }
    public Unit V { get; }
    public UV(Unit u, Unit v) => (U, V) = (u, v);
    public UV() { }
    public static UV New(Unit u, Unit v) => new UV(u, v);
    public static implicit operator (Unit, Unit)(UV self) => (self.U, self.V);
    public static implicit operator UV((Unit, Unit) value) => new UV(value.Item1, value.Item2);
    public Integer Count() => throw new NotImplementedException();
    public Unit At(Integer n) => throw new NotImplementedException();
    public Unit this[Integer n] => throw new NotImplementedException();public UV Zero() => throw new NotImplementedException();
    public UV One() => throw new NotImplementedException();
    public UV MinValue() => throw new NotImplementedException();
    public UV MaxValue() => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(UV y) => throw new NotImplementedException();
}
public class UVW: Vector<UVW, Unit>
{
    public Unit U { get; }
    public Unit V { get; }
    public Unit W { get; }
    public UVW(Unit u, Unit v, Unit w) => (U, V, W) = (u, v, w);
    public UVW() { }
    public static UVW New(Unit u, Unit v, Unit w) => new UVW(u, v, w);
    public static implicit operator (Unit, Unit, Unit)(UVW self) => (self.U, self.V, self.W);
    public static implicit operator UVW((Unit, Unit, Unit) value) => new UVW(value.Item1, value.Item2, value.Item3);
    public Integer Count() => throw new NotImplementedException();
    public Unit At(Integer n) => throw new NotImplementedException();
    public Unit this[Integer n] => throw new NotImplementedException();public UVW Zero() => throw new NotImplementedException();
    public UVW One() => throw new NotImplementedException();
    public UVW MinValue() => throw new NotImplementedException();
    public UVW MaxValue() => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(UVW y) => throw new NotImplementedException();
}
public class CubicBezier3D: Value<CubicBezier3D>
{
    public Point3D A { get; }
    public Point3D B { get; }
    public Point3D C { get; }
    public Point3D D { get; }
    public CubicBezier3D(Point3D a, Point3D b, Point3D c, Point3D d) => (A, B, C, D) = (a, b, c, d);
    public CubicBezier3D() { }
    public static CubicBezier3D New(Point3D a, Point3D b, Point3D c, Point3D d) => new CubicBezier3D(a, b, c, d);
    public static implicit operator (Point3D, Point3D, Point3D, Point3D)(CubicBezier3D self) => (self.A, self.B, self.C, self.D);
    public static implicit operator CubicBezier3D((Point3D, Point3D, Point3D, Point3D) value) => new CubicBezier3D(value.Item1, value.Item2, value.Item3, value.Item4);
    public CubicBezier3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class QuadraticBezier2D: Value<QuadraticBezier2D>
{
    public Point2D A { get; }
    public Point2D B { get; }
    public Point2D C { get; }
    public QuadraticBezier2D(Point2D a, Point2D b, Point2D c) => (A, B, C) = (a, b, c);
    public QuadraticBezier2D() { }
    public static QuadraticBezier2D New(Point2D a, Point2D b, Point2D c) => new QuadraticBezier2D(a, b, c);
    public static implicit operator (Point2D, Point2D, Point2D)(QuadraticBezier2D self) => (self.A, self.B, self.C);
    public static implicit operator QuadraticBezier2D((Point2D, Point2D, Point2D) value) => new QuadraticBezier2D(value.Item1, value.Item2, value.Item3);
    public QuadraticBezier2D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class QuadraticBezier3D: Value<QuadraticBezier3D>
{
    public Point3D A { get; }
    public Point3D B { get; }
    public Point3D C { get; }
    public QuadraticBezier3D(Point3D a, Point3D b, Point3D c) => (A, B, C) = (a, b, c);
    public QuadraticBezier3D() { }
    public static QuadraticBezier3D New(Point3D a, Point3D b, Point3D c) => new QuadraticBezier3D(a, b, c);
    public static implicit operator (Point3D, Point3D, Point3D)(QuadraticBezier3D self) => (self.A, self.B, self.C);
    public static implicit operator QuadraticBezier3D((Point3D, Point3D, Point3D) value) => new QuadraticBezier3D(value.Item1, value.Item2, value.Item3);
    public QuadraticBezier3D Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Area: Measure<Area>
{
    public Number MetersSquared { get; }
    public Area(Number metersSquared) => (MetersSquared) = (metersSquared);
    public Area() { }
    public static Area New(Number metersSquared) => new Area(metersSquared);
    public static implicit operator Number(Area self) => self.MetersSquared;
    public static implicit operator Area(Number value) => new Area(value);
    public Number Value() => throw new NotImplementedException();
    public Area Default() => throw new NotImplementedException();
    public Area Add(Number scalar) => throw new NotImplementedException();
    public static Area operator +(Area self, Number scalar) => throw new NotImplementedException();
    public Area Subtract(Number scalar) => throw new NotImplementedException();
    public static Area operator -(Area self, Number scalar) => throw new NotImplementedException();
    public Area Multiply(Number scalar) => throw new NotImplementedException();
    public static Area operator *(Area self, Number scalar) => throw new NotImplementedException();
    public Area Divide(Number scalar) => throw new NotImplementedException();
    public static Area operator /(Area self, Number scalar) => throw new NotImplementedException();
    public Area Modulo(Number scalar) => throw new NotImplementedException();
    public static Area operator %(Area self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Area b) => throw new NotImplementedException();
    public static Boolean operator ==(Area a, Area b) => throw new NotImplementedException();
    public Boolean NotEquals(Area b) => throw new NotImplementedException();
    public static Boolean operator !=(Area a, Area b) => throw new NotImplementedException();
    public Integer Compare(Area y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Area y) => throw new NotImplementedException();
}
public class Volume: Measure<Volume>
{
    public Number MetersCubed { get; }
    public Volume(Number metersCubed) => (MetersCubed) = (metersCubed);
    public Volume() { }
    public static Volume New(Number metersCubed) => new Volume(metersCubed);
    public static implicit operator Number(Volume self) => self.MetersCubed;
    public static implicit operator Volume(Number value) => new Volume(value);
    public Number Value() => throw new NotImplementedException();
    public Volume Default() => throw new NotImplementedException();
    public Volume Add(Number scalar) => throw new NotImplementedException();
    public static Volume operator +(Volume self, Number scalar) => throw new NotImplementedException();
    public Volume Subtract(Number scalar) => throw new NotImplementedException();
    public static Volume operator -(Volume self, Number scalar) => throw new NotImplementedException();
    public Volume Multiply(Number scalar) => throw new NotImplementedException();
    public static Volume operator *(Volume self, Number scalar) => throw new NotImplementedException();
    public Volume Divide(Number scalar) => throw new NotImplementedException();
    public static Volume operator /(Volume self, Number scalar) => throw new NotImplementedException();
    public Volume Modulo(Number scalar) => throw new NotImplementedException();
    public static Volume operator %(Volume self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Volume b) => throw new NotImplementedException();
    public static Boolean operator ==(Volume a, Volume b) => throw new NotImplementedException();
    public Boolean NotEquals(Volume b) => throw new NotImplementedException();
    public static Boolean operator !=(Volume a, Volume b) => throw new NotImplementedException();
    public Integer Compare(Volume y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Volume y) => throw new NotImplementedException();
}
public class Velocity: Measure<Velocity>
{
    public Number MetersPerSecond { get; }
    public Velocity(Number metersPerSecond) => (MetersPerSecond) = (metersPerSecond);
    public Velocity() { }
    public static Velocity New(Number metersPerSecond) => new Velocity(metersPerSecond);
    public static implicit operator Number(Velocity self) => self.MetersPerSecond;
    public static implicit operator Velocity(Number value) => new Velocity(value);
    public Number Value() => throw new NotImplementedException();
    public Velocity Default() => throw new NotImplementedException();
    public Velocity Add(Number scalar) => throw new NotImplementedException();
    public static Velocity operator +(Velocity self, Number scalar) => throw new NotImplementedException();
    public Velocity Subtract(Number scalar) => throw new NotImplementedException();
    public static Velocity operator -(Velocity self, Number scalar) => throw new NotImplementedException();
    public Velocity Multiply(Number scalar) => throw new NotImplementedException();
    public static Velocity operator *(Velocity self, Number scalar) => throw new NotImplementedException();
    public Velocity Divide(Number scalar) => throw new NotImplementedException();
    public static Velocity operator /(Velocity self, Number scalar) => throw new NotImplementedException();
    public Velocity Modulo(Number scalar) => throw new NotImplementedException();
    public static Velocity operator %(Velocity self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Velocity b) => throw new NotImplementedException();
    public static Boolean operator ==(Velocity a, Velocity b) => throw new NotImplementedException();
    public Boolean NotEquals(Velocity b) => throw new NotImplementedException();
    public static Boolean operator !=(Velocity a, Velocity b) => throw new NotImplementedException();
    public Integer Compare(Velocity y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Velocity y) => throw new NotImplementedException();
}
public class Acceleration: Measure<Acceleration>
{
    public Number MetersPerSecondSquared { get; }
    public Acceleration(Number metersPerSecondSquared) => (MetersPerSecondSquared) = (metersPerSecondSquared);
    public Acceleration() { }
    public static Acceleration New(Number metersPerSecondSquared) => new Acceleration(metersPerSecondSquared);
    public static implicit operator Number(Acceleration self) => self.MetersPerSecondSquared;
    public static implicit operator Acceleration(Number value) => new Acceleration(value);
    public Number Value() => throw new NotImplementedException();
    public Acceleration Default() => throw new NotImplementedException();
    public Acceleration Add(Number scalar) => throw new NotImplementedException();
    public static Acceleration operator +(Acceleration self, Number scalar) => throw new NotImplementedException();
    public Acceleration Subtract(Number scalar) => throw new NotImplementedException();
    public static Acceleration operator -(Acceleration self, Number scalar) => throw new NotImplementedException();
    public Acceleration Multiply(Number scalar) => throw new NotImplementedException();
    public static Acceleration operator *(Acceleration self, Number scalar) => throw new NotImplementedException();
    public Acceleration Divide(Number scalar) => throw new NotImplementedException();
    public static Acceleration operator /(Acceleration self, Number scalar) => throw new NotImplementedException();
    public Acceleration Modulo(Number scalar) => throw new NotImplementedException();
    public static Acceleration operator %(Acceleration self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Acceleration b) => throw new NotImplementedException();
    public static Boolean operator ==(Acceleration a, Acceleration b) => throw new NotImplementedException();
    public Boolean NotEquals(Acceleration b) => throw new NotImplementedException();
    public static Boolean operator !=(Acceleration a, Acceleration b) => throw new NotImplementedException();
    public Integer Compare(Acceleration y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Acceleration y) => throw new NotImplementedException();
}
public class Force: Measure<Force>
{
    public Number Newtons { get; }
    public Force(Number newtons) => (Newtons) = (newtons);
    public Force() { }
    public static Force New(Number newtons) => new Force(newtons);
    public static implicit operator Number(Force self) => self.Newtons;
    public static implicit operator Force(Number value) => new Force(value);
    public Number Value() => throw new NotImplementedException();
    public Force Default() => throw new NotImplementedException();
    public Force Add(Number scalar) => throw new NotImplementedException();
    public static Force operator +(Force self, Number scalar) => throw new NotImplementedException();
    public Force Subtract(Number scalar) => throw new NotImplementedException();
    public static Force operator -(Force self, Number scalar) => throw new NotImplementedException();
    public Force Multiply(Number scalar) => throw new NotImplementedException();
    public static Force operator *(Force self, Number scalar) => throw new NotImplementedException();
    public Force Divide(Number scalar) => throw new NotImplementedException();
    public static Force operator /(Force self, Number scalar) => throw new NotImplementedException();
    public Force Modulo(Number scalar) => throw new NotImplementedException();
    public static Force operator %(Force self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Force b) => throw new NotImplementedException();
    public static Boolean operator ==(Force a, Force b) => throw new NotImplementedException();
    public Boolean NotEquals(Force b) => throw new NotImplementedException();
    public static Boolean operator !=(Force a, Force b) => throw new NotImplementedException();
    public Integer Compare(Force y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Force y) => throw new NotImplementedException();
}
public class Pressure: Measure<Pressure>
{
    public Number Pascals { get; }
    public Pressure(Number pascals) => (Pascals) = (pascals);
    public Pressure() { }
    public static Pressure New(Number pascals) => new Pressure(pascals);
    public static implicit operator Number(Pressure self) => self.Pascals;
    public static implicit operator Pressure(Number value) => new Pressure(value);
    public Number Value() => throw new NotImplementedException();
    public Pressure Default() => throw new NotImplementedException();
    public Pressure Add(Number scalar) => throw new NotImplementedException();
    public static Pressure operator +(Pressure self, Number scalar) => throw new NotImplementedException();
    public Pressure Subtract(Number scalar) => throw new NotImplementedException();
    public static Pressure operator -(Pressure self, Number scalar) => throw new NotImplementedException();
    public Pressure Multiply(Number scalar) => throw new NotImplementedException();
    public static Pressure operator *(Pressure self, Number scalar) => throw new NotImplementedException();
    public Pressure Divide(Number scalar) => throw new NotImplementedException();
    public static Pressure operator /(Pressure self, Number scalar) => throw new NotImplementedException();
    public Pressure Modulo(Number scalar) => throw new NotImplementedException();
    public static Pressure operator %(Pressure self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Pressure b) => throw new NotImplementedException();
    public static Boolean operator ==(Pressure a, Pressure b) => throw new NotImplementedException();
    public Boolean NotEquals(Pressure b) => throw new NotImplementedException();
    public static Boolean operator !=(Pressure a, Pressure b) => throw new NotImplementedException();
    public Integer Compare(Pressure y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Pressure y) => throw new NotImplementedException();
}
public class Energy: Measure<Energy>
{
    public Number Joules { get; }
    public Energy(Number joules) => (Joules) = (joules);
    public Energy() { }
    public static Energy New(Number joules) => new Energy(joules);
    public static implicit operator Number(Energy self) => self.Joules;
    public static implicit operator Energy(Number value) => new Energy(value);
    public Number Value() => throw new NotImplementedException();
    public Energy Default() => throw new NotImplementedException();
    public Energy Add(Number scalar) => throw new NotImplementedException();
    public static Energy operator +(Energy self, Number scalar) => throw new NotImplementedException();
    public Energy Subtract(Number scalar) => throw new NotImplementedException();
    public static Energy operator -(Energy self, Number scalar) => throw new NotImplementedException();
    public Energy Multiply(Number scalar) => throw new NotImplementedException();
    public static Energy operator *(Energy self, Number scalar) => throw new NotImplementedException();
    public Energy Divide(Number scalar) => throw new NotImplementedException();
    public static Energy operator /(Energy self, Number scalar) => throw new NotImplementedException();
    public Energy Modulo(Number scalar) => throw new NotImplementedException();
    public static Energy operator %(Energy self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Energy b) => throw new NotImplementedException();
    public static Boolean operator ==(Energy a, Energy b) => throw new NotImplementedException();
    public Boolean NotEquals(Energy b) => throw new NotImplementedException();
    public static Boolean operator !=(Energy a, Energy b) => throw new NotImplementedException();
    public Integer Compare(Energy y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Energy y) => throw new NotImplementedException();
}
public class Memory: Measure<Memory>
{
    public Count Bytes { get; }
    public Memory(Count bytes) => (Bytes) = (bytes);
    public Memory() { }
    public static Memory New(Count bytes) => new Memory(bytes);
    public static implicit operator Count(Memory self) => self.Bytes;
    public static implicit operator Memory(Count value) => new Memory(value);
    public Number Value() => throw new NotImplementedException();
    public Memory Default() => throw new NotImplementedException();
    public Memory Add(Number scalar) => throw new NotImplementedException();
    public static Memory operator +(Memory self, Number scalar) => throw new NotImplementedException();
    public Memory Subtract(Number scalar) => throw new NotImplementedException();
    public static Memory operator -(Memory self, Number scalar) => throw new NotImplementedException();
    public Memory Multiply(Number scalar) => throw new NotImplementedException();
    public static Memory operator *(Memory self, Number scalar) => throw new NotImplementedException();
    public Memory Divide(Number scalar) => throw new NotImplementedException();
    public static Memory operator /(Memory self, Number scalar) => throw new NotImplementedException();
    public Memory Modulo(Number scalar) => throw new NotImplementedException();
    public static Memory operator %(Memory self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Memory b) => throw new NotImplementedException();
    public static Boolean operator ==(Memory a, Memory b) => throw new NotImplementedException();
    public Boolean NotEquals(Memory b) => throw new NotImplementedException();
    public static Boolean operator !=(Memory a, Memory b) => throw new NotImplementedException();
    public Integer Compare(Memory y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Memory y) => throw new NotImplementedException();
}
public class Frequency: Measure<Frequency>
{
    public Number Hertz { get; }
    public Frequency(Number hertz) => (Hertz) = (hertz);
    public Frequency() { }
    public static Frequency New(Number hertz) => new Frequency(hertz);
    public static implicit operator Number(Frequency self) => self.Hertz;
    public static implicit operator Frequency(Number value) => new Frequency(value);
    public Number Value() => throw new NotImplementedException();
    public Frequency Default() => throw new NotImplementedException();
    public Frequency Add(Number scalar) => throw new NotImplementedException();
    public static Frequency operator +(Frequency self, Number scalar) => throw new NotImplementedException();
    public Frequency Subtract(Number scalar) => throw new NotImplementedException();
    public static Frequency operator -(Frequency self, Number scalar) => throw new NotImplementedException();
    public Frequency Multiply(Number scalar) => throw new NotImplementedException();
    public static Frequency operator *(Frequency self, Number scalar) => throw new NotImplementedException();
    public Frequency Divide(Number scalar) => throw new NotImplementedException();
    public static Frequency operator /(Frequency self, Number scalar) => throw new NotImplementedException();
    public Frequency Modulo(Number scalar) => throw new NotImplementedException();
    public static Frequency operator %(Frequency self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Frequency b) => throw new NotImplementedException();
    public static Boolean operator ==(Frequency a, Frequency b) => throw new NotImplementedException();
    public Boolean NotEquals(Frequency b) => throw new NotImplementedException();
    public static Boolean operator !=(Frequency a, Frequency b) => throw new NotImplementedException();
    public Integer Compare(Frequency y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Frequency y) => throw new NotImplementedException();
}
public class Loudness: Measure<Loudness>
{
    public Number Decibels { get; }
    public Loudness(Number decibels) => (Decibels) = (decibels);
    public Loudness() { }
    public static Loudness New(Number decibels) => new Loudness(decibels);
    public static implicit operator Number(Loudness self) => self.Decibels;
    public static implicit operator Loudness(Number value) => new Loudness(value);
    public Number Value() => throw new NotImplementedException();
    public Loudness Default() => throw new NotImplementedException();
    public Loudness Add(Number scalar) => throw new NotImplementedException();
    public static Loudness operator +(Loudness self, Number scalar) => throw new NotImplementedException();
    public Loudness Subtract(Number scalar) => throw new NotImplementedException();
    public static Loudness operator -(Loudness self, Number scalar) => throw new NotImplementedException();
    public Loudness Multiply(Number scalar) => throw new NotImplementedException();
    public static Loudness operator *(Loudness self, Number scalar) => throw new NotImplementedException();
    public Loudness Divide(Number scalar) => throw new NotImplementedException();
    public static Loudness operator /(Loudness self, Number scalar) => throw new NotImplementedException();
    public Loudness Modulo(Number scalar) => throw new NotImplementedException();
    public static Loudness operator %(Loudness self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Loudness b) => throw new NotImplementedException();
    public static Boolean operator ==(Loudness a, Loudness b) => throw new NotImplementedException();
    public Boolean NotEquals(Loudness b) => throw new NotImplementedException();
    public static Boolean operator !=(Loudness a, Loudness b) => throw new NotImplementedException();
    public Integer Compare(Loudness y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Loudness y) => throw new NotImplementedException();
}
public class LuminousIntensity: Measure<LuminousIntensity>
{
    public Number Candelas { get; }
    public LuminousIntensity(Number candelas) => (Candelas) = (candelas);
    public LuminousIntensity() { }
    public static LuminousIntensity New(Number candelas) => new LuminousIntensity(candelas);
    public static implicit operator Number(LuminousIntensity self) => self.Candelas;
    public static implicit operator LuminousIntensity(Number value) => new LuminousIntensity(value);
    public Number Value() => throw new NotImplementedException();
    public LuminousIntensity Default() => throw new NotImplementedException();
    public LuminousIntensity Add(Number scalar) => throw new NotImplementedException();
    public static LuminousIntensity operator +(LuminousIntensity self, Number scalar) => throw new NotImplementedException();
    public LuminousIntensity Subtract(Number scalar) => throw new NotImplementedException();
    public static LuminousIntensity operator -(LuminousIntensity self, Number scalar) => throw new NotImplementedException();
    public LuminousIntensity Multiply(Number scalar) => throw new NotImplementedException();
    public static LuminousIntensity operator *(LuminousIntensity self, Number scalar) => throw new NotImplementedException();
    public LuminousIntensity Divide(Number scalar) => throw new NotImplementedException();
    public static LuminousIntensity operator /(LuminousIntensity self, Number scalar) => throw new NotImplementedException();
    public LuminousIntensity Modulo(Number scalar) => throw new NotImplementedException();
    public static LuminousIntensity operator %(LuminousIntensity self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(LuminousIntensity b) => throw new NotImplementedException();
    public static Boolean operator ==(LuminousIntensity a, LuminousIntensity b) => throw new NotImplementedException();
    public Boolean NotEquals(LuminousIntensity b) => throw new NotImplementedException();
    public static Boolean operator !=(LuminousIntensity a, LuminousIntensity b) => throw new NotImplementedException();
    public Integer Compare(LuminousIntensity y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(LuminousIntensity y) => throw new NotImplementedException();
}
public class ElectricPotential: Measure<ElectricPotential>
{
    public Number Volts { get; }
    public ElectricPotential(Number volts) => (Volts) = (volts);
    public ElectricPotential() { }
    public static ElectricPotential New(Number volts) => new ElectricPotential(volts);
    public static implicit operator Number(ElectricPotential self) => self.Volts;
    public static implicit operator ElectricPotential(Number value) => new ElectricPotential(value);
    public Number Value() => throw new NotImplementedException();
    public ElectricPotential Default() => throw new NotImplementedException();
    public ElectricPotential Add(Number scalar) => throw new NotImplementedException();
    public static ElectricPotential operator +(ElectricPotential self, Number scalar) => throw new NotImplementedException();
    public ElectricPotential Subtract(Number scalar) => throw new NotImplementedException();
    public static ElectricPotential operator -(ElectricPotential self, Number scalar) => throw new NotImplementedException();
    public ElectricPotential Multiply(Number scalar) => throw new NotImplementedException();
    public static ElectricPotential operator *(ElectricPotential self, Number scalar) => throw new NotImplementedException();
    public ElectricPotential Divide(Number scalar) => throw new NotImplementedException();
    public static ElectricPotential operator /(ElectricPotential self, Number scalar) => throw new NotImplementedException();
    public ElectricPotential Modulo(Number scalar) => throw new NotImplementedException();
    public static ElectricPotential operator %(ElectricPotential self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(ElectricPotential b) => throw new NotImplementedException();
    public static Boolean operator ==(ElectricPotential a, ElectricPotential b) => throw new NotImplementedException();
    public Boolean NotEquals(ElectricPotential b) => throw new NotImplementedException();
    public static Boolean operator !=(ElectricPotential a, ElectricPotential b) => throw new NotImplementedException();
    public Integer Compare(ElectricPotential y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(ElectricPotential y) => throw new NotImplementedException();
}
public class ElectricCharge: Measure<ElectricCharge>
{
    public Number Columbs { get; }
    public ElectricCharge(Number columbs) => (Columbs) = (columbs);
    public ElectricCharge() { }
    public static ElectricCharge New(Number columbs) => new ElectricCharge(columbs);
    public static implicit operator Number(ElectricCharge self) => self.Columbs;
    public static implicit operator ElectricCharge(Number value) => new ElectricCharge(value);
    public Number Value() => throw new NotImplementedException();
    public ElectricCharge Default() => throw new NotImplementedException();
    public ElectricCharge Add(Number scalar) => throw new NotImplementedException();
    public static ElectricCharge operator +(ElectricCharge self, Number scalar) => throw new NotImplementedException();
    public ElectricCharge Subtract(Number scalar) => throw new NotImplementedException();
    public static ElectricCharge operator -(ElectricCharge self, Number scalar) => throw new NotImplementedException();
    public ElectricCharge Multiply(Number scalar) => throw new NotImplementedException();
    public static ElectricCharge operator *(ElectricCharge self, Number scalar) => throw new NotImplementedException();
    public ElectricCharge Divide(Number scalar) => throw new NotImplementedException();
    public static ElectricCharge operator /(ElectricCharge self, Number scalar) => throw new NotImplementedException();
    public ElectricCharge Modulo(Number scalar) => throw new NotImplementedException();
    public static ElectricCharge operator %(ElectricCharge self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(ElectricCharge b) => throw new NotImplementedException();
    public static Boolean operator ==(ElectricCharge a, ElectricCharge b) => throw new NotImplementedException();
    public Boolean NotEquals(ElectricCharge b) => throw new NotImplementedException();
    public static Boolean operator !=(ElectricCharge a, ElectricCharge b) => throw new NotImplementedException();
    public Integer Compare(ElectricCharge y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(ElectricCharge y) => throw new NotImplementedException();
}
public class ElectricCurrent: Measure<ElectricCurrent>
{
    public Number Amperes { get; }
    public ElectricCurrent(Number amperes) => (Amperes) = (amperes);
    public ElectricCurrent() { }
    public static ElectricCurrent New(Number amperes) => new ElectricCurrent(amperes);
    public static implicit operator Number(ElectricCurrent self) => self.Amperes;
    public static implicit operator ElectricCurrent(Number value) => new ElectricCurrent(value);
    public Number Value() => throw new NotImplementedException();
    public ElectricCurrent Default() => throw new NotImplementedException();
    public ElectricCurrent Add(Number scalar) => throw new NotImplementedException();
    public static ElectricCurrent operator +(ElectricCurrent self, Number scalar) => throw new NotImplementedException();
    public ElectricCurrent Subtract(Number scalar) => throw new NotImplementedException();
    public static ElectricCurrent operator -(ElectricCurrent self, Number scalar) => throw new NotImplementedException();
    public ElectricCurrent Multiply(Number scalar) => throw new NotImplementedException();
    public static ElectricCurrent operator *(ElectricCurrent self, Number scalar) => throw new NotImplementedException();
    public ElectricCurrent Divide(Number scalar) => throw new NotImplementedException();
    public static ElectricCurrent operator /(ElectricCurrent self, Number scalar) => throw new NotImplementedException();
    public ElectricCurrent Modulo(Number scalar) => throw new NotImplementedException();
    public static ElectricCurrent operator %(ElectricCurrent self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(ElectricCurrent b) => throw new NotImplementedException();
    public static Boolean operator ==(ElectricCurrent a, ElectricCurrent b) => throw new NotImplementedException();
    public Boolean NotEquals(ElectricCurrent b) => throw new NotImplementedException();
    public static Boolean operator !=(ElectricCurrent a, ElectricCurrent b) => throw new NotImplementedException();
    public Integer Compare(ElectricCurrent y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(ElectricCurrent y) => throw new NotImplementedException();
}
public class ElectricResistance: Measure<ElectricResistance>
{
    public Number Ohms { get; }
    public ElectricResistance(Number ohms) => (Ohms) = (ohms);
    public ElectricResistance() { }
    public static ElectricResistance New(Number ohms) => new ElectricResistance(ohms);
    public static implicit operator Number(ElectricResistance self) => self.Ohms;
    public static implicit operator ElectricResistance(Number value) => new ElectricResistance(value);
    public Number Value() => throw new NotImplementedException();
    public ElectricResistance Default() => throw new NotImplementedException();
    public ElectricResistance Add(Number scalar) => throw new NotImplementedException();
    public static ElectricResistance operator +(ElectricResistance self, Number scalar) => throw new NotImplementedException();
    public ElectricResistance Subtract(Number scalar) => throw new NotImplementedException();
    public static ElectricResistance operator -(ElectricResistance self, Number scalar) => throw new NotImplementedException();
    public ElectricResistance Multiply(Number scalar) => throw new NotImplementedException();
    public static ElectricResistance operator *(ElectricResistance self, Number scalar) => throw new NotImplementedException();
    public ElectricResistance Divide(Number scalar) => throw new NotImplementedException();
    public static ElectricResistance operator /(ElectricResistance self, Number scalar) => throw new NotImplementedException();
    public ElectricResistance Modulo(Number scalar) => throw new NotImplementedException();
    public static ElectricResistance operator %(ElectricResistance self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(ElectricResistance b) => throw new NotImplementedException();
    public static Boolean operator ==(ElectricResistance a, ElectricResistance b) => throw new NotImplementedException();
    public Boolean NotEquals(ElectricResistance b) => throw new NotImplementedException();
    public static Boolean operator !=(ElectricResistance a, ElectricResistance b) => throw new NotImplementedException();
    public Integer Compare(ElectricResistance y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(ElectricResistance y) => throw new NotImplementedException();
}
public class Power: Measure<Power>
{
    public Number Watts { get; }
    public Power(Number watts) => (Watts) = (watts);
    public Power() { }
    public static Power New(Number watts) => new Power(watts);
    public static implicit operator Number(Power self) => self.Watts;
    public static implicit operator Power(Number value) => new Power(value);
    public Number Value() => throw new NotImplementedException();
    public Power Default() => throw new NotImplementedException();
    public Power Add(Number scalar) => throw new NotImplementedException();
    public static Power operator +(Power self, Number scalar) => throw new NotImplementedException();
    public Power Subtract(Number scalar) => throw new NotImplementedException();
    public static Power operator -(Power self, Number scalar) => throw new NotImplementedException();
    public Power Multiply(Number scalar) => throw new NotImplementedException();
    public static Power operator *(Power self, Number scalar) => throw new NotImplementedException();
    public Power Divide(Number scalar) => throw new NotImplementedException();
    public static Power operator /(Power self, Number scalar) => throw new NotImplementedException();
    public Power Modulo(Number scalar) => throw new NotImplementedException();
    public static Power operator %(Power self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Power b) => throw new NotImplementedException();
    public static Boolean operator ==(Power a, Power b) => throw new NotImplementedException();
    public Boolean NotEquals(Power b) => throw new NotImplementedException();
    public static Boolean operator !=(Power a, Power b) => throw new NotImplementedException();
    public Integer Compare(Power y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Power y) => throw new NotImplementedException();
}
public class Density: Measure<Density>
{
    public Number KilogramsPerMeterCubed { get; }
    public Density(Number kilogramsPerMeterCubed) => (KilogramsPerMeterCubed) = (kilogramsPerMeterCubed);
    public Density() { }
    public static Density New(Number kilogramsPerMeterCubed) => new Density(kilogramsPerMeterCubed);
    public static implicit operator Number(Density self) => self.KilogramsPerMeterCubed;
    public static implicit operator Density(Number value) => new Density(value);
    public Number Value() => throw new NotImplementedException();
    public Density Default() => throw new NotImplementedException();
    public Density Add(Number scalar) => throw new NotImplementedException();
    public static Density operator +(Density self, Number scalar) => throw new NotImplementedException();
    public Density Subtract(Number scalar) => throw new NotImplementedException();
    public static Density operator -(Density self, Number scalar) => throw new NotImplementedException();
    public Density Multiply(Number scalar) => throw new NotImplementedException();
    public static Density operator *(Density self, Number scalar) => throw new NotImplementedException();
    public Density Divide(Number scalar) => throw new NotImplementedException();
    public static Density operator /(Density self, Number scalar) => throw new NotImplementedException();
    public Density Modulo(Number scalar) => throw new NotImplementedException();
    public static Density operator %(Density self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Density b) => throw new NotImplementedException();
    public static Boolean operator ==(Density a, Density b) => throw new NotImplementedException();
    public Boolean NotEquals(Density b) => throw new NotImplementedException();
    public static Boolean operator !=(Density a, Density b) => throw new NotImplementedException();
    public Integer Compare(Density y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Density y) => throw new NotImplementedException();
}
public class NormalDistribution: Value<NormalDistribution>
{
    public Number Mean { get; }
    public Number StandardDeviation { get; }
    public NormalDistribution(Number mean, Number standardDeviation) => (Mean, StandardDeviation) = (mean, standardDeviation);
    public NormalDistribution() { }
    public static NormalDistribution New(Number mean, Number standardDeviation) => new NormalDistribution(mean, standardDeviation);
    public static implicit operator (Number, Number)(NormalDistribution self) => (self.Mean, self.StandardDeviation);
    public static implicit operator NormalDistribution((Number, Number) value) => new NormalDistribution(value.Item1, value.Item2);
    public NormalDistribution Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class PoissonDistribution: Value<PoissonDistribution>
{
    public Number Expected { get; }
    public Count Occurrences { get; }
    public PoissonDistribution(Number expected, Count occurrences) => (Expected, Occurrences) = (expected, occurrences);
    public PoissonDistribution() { }
    public static PoissonDistribution New(Number expected, Count occurrences) => new PoissonDistribution(expected, occurrences);
    public static implicit operator (Number, Count)(PoissonDistribution self) => (self.Expected, self.Occurrences);
    public static implicit operator PoissonDistribution((Number, Count) value) => new PoissonDistribution(value.Item1, value.Item2);
    public PoissonDistribution Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class BernoulliDistribution: Value<BernoulliDistribution>
{
    public Probability P { get; }
    public BernoulliDistribution(Probability p) => (P) = (p);
    public BernoulliDistribution() { }
    public static BernoulliDistribution New(Probability p) => new BernoulliDistribution(p);
    public static implicit operator Probability(BernoulliDistribution self) => self.P;
    public static implicit operator BernoulliDistribution(Probability value) => new BernoulliDistribution(value);
    public BernoulliDistribution Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
public class Probability: Numerical<Probability>
{
    public Number Value { get; }
    public Probability(Number value) => (Value) = (value);
    public Probability() { }
    public static Probability New(Number value) => new Probability(value);
    public static implicit operator Number(Probability self) => self.Value;
    public static implicit operator Probability(Number value) => new Probability(value);
    public Probability Zero() => throw new NotImplementedException();
    public Probability One() => throw new NotImplementedException();
    public Probability MinValue() => throw new NotImplementedException();
    public Probability MaxValue() => throw new NotImplementedException();
    public Probability Default() => throw new NotImplementedException();
    public Probability Add(Probability other) => throw new NotImplementedException();
    public static Probability operator +(Probability self, Probability other) => throw new NotImplementedException();
    public Probability Subtract(Probability other) => throw new NotImplementedException();
    public static Probability operator -(Probability self, Probability other) => throw new NotImplementedException();
    public Probability Negative() => throw new NotImplementedException();
    public static Probability operator -(Probability self) => throw new NotImplementedException();
    public Probability Reciprocal() => throw new NotImplementedException();
    public Probability Multiply(Probability other) => throw new NotImplementedException();
    public static Probability operator *(Probability self, Probability other) => throw new NotImplementedException();
    public Probability Divide(Probability other) => throw new NotImplementedException();
    public static Probability operator /(Probability self, Probability other) => throw new NotImplementedException();
    public Probability Modulo(Probability other) => throw new NotImplementedException();
    public static Probability operator %(Probability self, Probability other) => throw new NotImplementedException();
    public Probability Add(Number scalar) => throw new NotImplementedException();
    public static Probability operator +(Probability self, Number scalar) => throw new NotImplementedException();
    public Probability Subtract(Number scalar) => throw new NotImplementedException();
    public static Probability operator -(Probability self, Number scalar) => throw new NotImplementedException();
    public Probability Multiply(Number scalar) => throw new NotImplementedException();
    public static Probability operator *(Probability self, Number scalar) => throw new NotImplementedException();
    public Probability Divide(Number scalar) => throw new NotImplementedException();
    public static Probability operator /(Probability self, Number scalar) => throw new NotImplementedException();
    public Probability Modulo(Number scalar) => throw new NotImplementedException();
    public static Probability operator %(Probability self, Number scalar) => throw new NotImplementedException();
    public Boolean Equals(Probability b) => throw new NotImplementedException();
    public static Boolean operator ==(Probability a, Probability b) => throw new NotImplementedException();
    public Boolean NotEquals(Probability b) => throw new NotImplementedException();
    public static Boolean operator !=(Probability a, Probability b) => throw new NotImplementedException();
    public Integer Compare(Probability y) => throw new NotImplementedException();
    public Number Magnitude() => throw new NotImplementedException();
    public Integer Compare(Probability y) => throw new NotImplementedException();
}
public class BinomialDistribution: Value<BinomialDistribution>
{
    public Count Trials { get; }
    public Probability P { get; }
    public BinomialDistribution(Count trials, Probability p) => (Trials, P) = (trials, p);
    public BinomialDistribution() { }
    public static BinomialDistribution New(Count trials, Probability p) => new BinomialDistribution(trials, p);
    public static implicit operator (Count, Probability)(BinomialDistribution self) => (self.Trials, self.P);
    public static implicit operator BinomialDistribution((Count, Probability) value) => new BinomialDistribution(value.Item1, value.Item2);
    public BinomialDistribution Default() => throw new NotImplementedException();
    public Array<String> FieldNames() => throw new NotImplementedException();
    public Array<Any> FieldValues() => throw new NotImplementedException();
}
