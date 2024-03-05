using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry.Tests;

//==
// New classes. 
// Also see: Ara3D.Geometry.KdTree which was recently added. 
// A few functions have been scattered throughout the library to facilitate things. 

/// <summary>
/// A circle in 3D space with specified center point, radius and orientation.
/// The ray specifies the center and the normal.
/// </summary>
public readonly struct Circle3D
{
    public readonly Ray Ray;
    public readonly float Radius;

    public float Diameter => Radius * 2;
    public float Area => MathF.PI * Radius.Sqr();
    public float Circumference => MathF.PI * Diameter;
    public Vector3 Point => Ray.Position;
    public Vector3 Normal => Ray.Direction;
    public Plane Plane => Plane.CreateFromNormalAndPoint(Normal, Point);

    public Circle3D(Ray ray, float radius)
        => (Ray, Radius) = (ray, radius);
}

/// <summary>
/// A circle in two dimensions with a specified center point and radius. 
/// </summary>
public readonly struct Circle
    : ICurve<Vector2>
{
    public readonly Vector2 Center;
    public readonly float Radius;

    public float Diameter => Radius * 2;
    public float Area => MathF.PI * Radius.Sqr();
    public float Circumference => MathF.PI * Diameter;

    public Circle(Vector2 point, float radius)
        => (Center, Radius) = (point, radius);

    public Circle(float radius)
        : this(Vector2.Zero,radius)
    { }
       
    public Vector2 Eval(float x)
        => Center + (x.Sin(), x.Cos());

    public bool Closed
        => true;
}

/// <summary>
/// An arc in two dimension, for a given circle and two angles. 
/// </summary>
public readonly struct Arc
    : ICurve<Vector2>
{
    public readonly Circle Circle;
    public readonly Interval Angles;
    public Arc(Circle circle, Interval angles)
        => (Circle, Angles) = (circle, angles);

    public Vector2 Eval(float x)
        => Circle.Eval(Angles.Lerp(x));

    public bool Closed => false;
}

/// <summary>
/// An arbitrary bounding box that is represented
/// as a Unit cube transformed in space. 
/// </summary>
public readonly struct TransformedBox
{
    public readonly Transform Transform;
    public Vector3 Dimensions { get; }
    public Quaternion Rotation => Transform.Orientation;
    public Vector3 Center => Transform.Position;

    public TransformedBox(Transform transform, Vector3 dimensions)
        => (Transform, Dimensions) = (transform, dimensions);
}

/// <summary>
/// Represents a single point (node/vertex) in a Moldflow geometry 
/// </summary>
public class MoldflowPoint
    : IBounded, ITransformable
{
    public readonly Vector3 Position;
    public readonly int Label;
    public readonly int Index;
    public AABox Bounds => AABox.Create(Position);

    public MoldflowPoint(Vector3 position, int label, int index)
        => (Position, Label, Index) = (position, label, index);

    public ITransformable TransformImpl(Matrix4x4 mat)
        => new MoldflowPoint(Position.Transform(mat), Label, Index);
}

/// <summary>
/// Represents data input from Moldflow
/// </summary>
public class MoldflowGeometry
    : IBounded, ITransformable
{
    /// <summary>
    /// This is the point data with the integer labels, and indices for convenient look-up
    /// </summary>
    public IArray<MoldflowPoint> Points { get; }

    /// <summary>
    /// This binary space partitioning structure accelerates closest-point queries. 
    /// </summary>
    public KdTree<MoldflowPoint> KdTree { get; }

    public MoldflowGeometry(IArray<MoldflowPoint> points)
    {
        Points = points;
        Bounds = points.GetBounds();
        KdTree = new KdTree<MoldflowPoint>(points);
    }

    public AABox Bounds { get; }

    public ITransformable TransformImpl(Matrix4x4 mat)
        => new MoldflowGeometry(Points.Select(p => p.Transform(mat)));
}

/// <summary>
/// Some of the algorithms requested. 
/// </summary>
public static class RobsGeometryExamples
{
    // Functions are for Comparing 2 meshed parts for node differences
    // This returns indices of where the differences are. 

    // TODO: right now these functions are doing a "line by line" comparison. 

    public static IArray<int> PointInANotInB(IArray<MoldflowPoint> a, IArray<MoldflowPoint> b)
    {
        return a.Count > b.Count 
            ? b.Count.Upto(a.Count) 
            : 0.Repeat(0);
    }

    public static IArray<int> PointInBNotInA(IArray<MoldflowPoint> a, IArray<MoldflowPoint> b)
    {
        return PointInANotInB(b, a);
    }

    public static IEnumerable<int> PointsChanged(IArray<MoldflowPoint> a, IArray<MoldflowPoint> b, float tolerance = Constants.Tolerance)
    {
        var n = a.Count.Min(b.Count);
        for (var i=0; i < n; ++i)
        {
            var p0 = a[i];
            var p1 = b[i];
            if (p0.Label != p1.Label) 
                yield return i;
            else 
            if (p0.Position.AlmostEquals(p1.Position, tolerance)) 
                yield return i;
        }
    }

    /// <summary>
    /// Returns N points along the curve.
    /// First point at the beginning.
    /// Second point may be at the end iff includeEndPoint == true
    /// </summary>
    public static IArray<T> Sample<T>(this ICurve<T> curve, int n, bool includeEndPoint = true)
    {
        var xs = includeEndPoint 
            ? n.SampleZeroToOneInclusive() 
            : n.SampleZeroToOneExclusive();
        return xs.Select(curve.Eval);
    }

    /// <summary>
    /// As specified the point on the circle at angle A is assumed to be the origin.
    /// So the return values are offset.
    /// All angles are specified clockwise from 12 o'clock
    /// </summary>
    public static IArray<Vector2> CashewPoints(float angleA, float angleB, float radius, int n)
    {
        var circle0 = new Circle(radius);
        var ptA = circle0.PointOnCircle(angleA);
        var arc = new Arc(circle0, (angleA, angleB));
        return arc.Sample(n).Select(x => x - ptA);
    }

    /// <summary>
    /// Returns a point on a circle. We assume the angles are clockwise from 12 o'clock.
    /// </summary>
    public static Vector2 PointOnCircle(this Circle c, float angle)
        => c.Eval(angle);

    /// <summary>
    /// Given a center point and a point on a circle, will return a 3D circle with an arbitrary orientation 
    /// </summary>
    public static Circle3D CircleTwoPoints(Vector3 p0, Vector3 p1)
        => throw new NotImplementedException("TODO: Please validate the description." +
                                             "Infinite circles are possible: oriented in various directions along a particular plane. " +
                                             "Is there any particular method desired for choosing one?");

    /// <summary>
    /// Given three points, returns a circle that goes through them.
    /// https://stackoverflow.com/questions/13977354/build-circle-from-3-points-in-3d-space-implementation-in-c-or-c
    /// </summary>
    public static Circle3D CircleFromPoints(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // triangle "edges"
        var t = p2 - p1;
        var u = p3 - p1;
        var v = p3 - p2;

        // triangle normal
        var w = t.Cross(u);
        var wsl = w.Dot(w);

        // helpers
        var iwsl2 = 1f / (2f * wsl);
        var tt = t.Dot(t);
        var uu = u.Dot(u);

        // result circle
        var center = p1 + (u * tt * u.Dot(v) - t * uu * t.Dot(v)) * iwsl2;
        var radius = (tt * uu * Vector3.Dot(v, v) * iwsl2 * 0.5f).Sqrt();
        var circAxis = w / wsl.Sqrt();
        return new Circle3D((center, circAxis), radius);
    }

    /// <summary>
    /// TODO: use the KD-Tree structure.
    /// TODO: return an index (or MoldflowPoint), not just the position. 
    /// </summary>
    public static Vector3 FindNearestPoint(MoldflowGeometry g, Vector3 point)
    {
        // NOTE: this is an inefficient approach 
        return g.Points.Select(p => p.Position).ToEnumerable().NearestPoint(point);
    }

    public static float PerpendicularAngleFromPoints(Vector2 a, Vector2 b)
        => throw new NotImplementedException("In progress");

    /// <summary>
    /// Returns an axis-aligned bounding box for a series of points
    /// </summary>
    public static AABox GetBounds(this IEnumerable<Vector3> points)
        => points.ToBox();

    // TODO: use the TransformedBox structure above 

    /// <summary>
    /// Translating some points 
    /// </summary>
    public static IArray<Vector3> Move(this IArray<Vector3> points, Vector3 offset)
        => points.Select(p => p + offset);

    /// <summary>
    /// Rotate points around an axis
    /// </summary>
    public static IArray<Vector3> RotateAroundAxis(this IArray<Vector3> points, Vector3 axis, float angle)
        => points.Rotate(Quaternion.CreateFromAxisAngle(axis, angle));

    /// <summary>
    /// Rotate points using a quaternion. 
    /// </summary>
    public static IArray<Vector3> Rotate(this IArray<Vector3> points, Quaternion q)
        => points.Select(p => p.Transform(q));

    /// <summary>
    /// </summary>
    public static IArray<Vector3> TransformCoordinateSystem(this IEnumerable<Vector3> points, Transform transform)
        => throw new NotImplementedException("Coordinate systems (or frames of reference) typically refer to only translation and rotation,");

    //==
    // Coordinate and unit conversions
    // Note: the following functions will be important for receiving and sending data to the user interface. 

    /// <summary>
    /// Converts from radians starting in a clock wise manner from 12 O'Clock to counter clock wise degrees starting from 3 O'Clock  
    /// </summary>
    public static float ConvertAngleToMoldflowUser(float f)
        => (-f).ToDegrees() + 90;

    /// <summary>
    /// Converts from radians starting in a clock wise manner from 12 O'Clock to counter clock wise degrees starting from 3 O'Clock  
    /// </summary>
    public static float ConvertAngleFromMoldflowUser(float f)
        => (-f).ToDegrees() + 90;
}