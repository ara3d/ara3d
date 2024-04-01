using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public enum StandardPlane
    {
        Xy,
        Xz,
        Yz,
    }

    // TODO: many of these functions should live in other places, particular in the math 3D
    public static class GeometryUtil
    {
        public static IArray<Vector3> Normalize(this IArray<Vector3> vectors)
            => vectors.Select(v => v.Normalize());

        public static bool SequenceAlmostEquals(this IArray<Vector3> vs1, IArray<Vector3> vs2, float tolerance = Constants.Tolerance)
            => vs1.Count == vs2.Count && vs1.Indices().All(i => vs1[i].AlmostEquals(vs2[i], tolerance));

        public static bool AreColinear(this IEnumerable<Vector3> vectors, Vector3 reference, float tolerance = (float)Constants.OneTenthOfADegree)
            => !reference.IsNaN() && vectors.All(v => v.Colinear(reference, tolerance));

        public static bool AreColinear(this IEnumerable<Vector3> vectors, float tolerance = (float)Constants.OneTenthOfADegree)
            => vectors.ToList().AreColinear(tolerance);

        public static bool AreColinear(this System.Collections.Generic.IList<Vector3> vectors, float tolerance = (float)Constants.OneTenthOfADegree)
            => vectors.Count <= 1 || vectors.Skip(1).AreColinear(vectors[0], tolerance);

        public static AABox BoundingBox(this IArray<Vector3> vertices)
            => AABox.Create(vertices.ToEnumerable());

        public static IArray<float> Interpolate(this int count)
            => InterpolateExclusive(count);

        public static IArray<float> InterpolateInclusive(this int count)
            => count <= 0
                ? LinqArray.Empty<float>()
                : count == 1
                    ? LinqArray.Create(0f)
                    : count.Select(i => i / (float)(count - 1));

        public static IArray<float> InterpolateExclusive(this int count)
            => count <= 0
                ? LinqArray.Empty<float>()
                : count.Select(i => i / (float)count);

        public static IArray<Vector3> InterpolateInclusive(this int count, Func<float, Vector3> function)
            => count.InterpolateInclusive().Select(function);

        public static IArray<Vector3> Interpolate(this Line self, int count)
            => count.InterpolateInclusive(self.Lerp);

        public static IArray<Vector3> Rotate(this IArray<Vector3> self, Vector3 axis, float angle)
            => self.Transform(Matrix4x4.CreateFromAxisAngle(axis, angle));

        public static IArray<Vector3> Transform(this IArray<Vector3> self, Matrix4x4 matrix)
            => self.Select(x => x.Transform(matrix));

        public static Int3 Sort(this Int3 v)
        {
            if (v.X < v.Y)
            {
                return (v.Y < v.Z)
                    ? new Int3(v.X, v.Y, v.Z)
                    : (v.X < v.Z)
                        ? new Int3(v.X, v.Z, v.Y)
                        : new Int3(v.Z, v.X, v.Y);
            }
            else
            {
                return (v.X < v.Z)
                     ? new Int3(v.Y, v.X, v.Z)
                     : (v.Y < v.Z)
                        ? new Int3(v.Y, v.Z, v.X)
                        : new Int3(v.Z, v.Y, v.X);
            }
        }

        // Fins the intersection between two lines.
        // Returns true if they intersect
        // t and u are the distances of the intersection point along the two line
        public static bool LineLineIntersection(Vector2 line1Origin, Vector2 line1Direction, Vector2 line2Origin, Vector2 line2Direction, out float t, out float u, float epsilon = 0.01f)
        {
            var line1P2 = line1Origin + line1Direction;
            var line2P2 = line2Origin + line2Direction;

            var x1 = line1Origin.X;
            var y1 = line1Origin.Y;
            var x2 = line1P2.X;
            var y2 = line1P2.Y;
            var x3 = line2Origin.X;
            var y3 = line2Origin.Y;
            var x4 = line2P2.X;
            var y4 = line2P2.Y;

            var denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (denominator.Abs() < epsilon)
            {
                t = 0.0f;
                u = 0.0f;
                return false;
            }

            var tNeumerator = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
            var uNeumerator = (x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3);

            t = tNeumerator / denominator;
            u = -uNeumerator / denominator;

            return true;
        }

        // Returns the distance between two lines
        // t and u are the distances if the intersection points along the two lines 
        public static float LineLineDistance(Vector2 line1A, Vector2 line1B, Vector2 line2A, Vector2 line2B, out float t, out float u, float epsilon = 0.0000001f)
        {
            var x1 = line1A.X;
            var y1 = line1A.Y;
            var x2 = line1B.X;
            var y2 = line1B.Y;
            var x3 = line2A.X;
            var y3 = line2A.Y;
            var x4 = line2B.X;
            var y4 = line2B.Y;

            var denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (denominator.Abs() >= epsilon)
            {
                // Lines are not parallel, they should intersect nicely
                var tNeumerator = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
                var uNeumerator = (x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3);

                t = tNeumerator / denominator;
                u = -uNeumerator / denominator;

                var e = 0.0;
                if (t >= -e && t <= 1.0 + e && u >= -e && u <= 1.0 + e)
                {
                    t = t.Clamp(0.0f, 1.0f);
                    u = u.Clamp(0.0f, 1.0f);
                    return 0;
                }
            }

            // Parallel or non intersecting lines - default to point to line checks

            u = 0.0f;
            var minDistance = LinePointDistance(line1A, line1B, line2A, out t);
            var distance = LinePointDistance(line1A, line1B, line2B, out var closestPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                t = closestPoint;
                u = 1.0f;
            }

            distance = LinePointDistance(line2A, line2B, line1A, out closestPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                u = closestPoint;
                t = 0.0f;
            }

            distance = LinePointDistance(line2A, line2B, line1B, out closestPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                u = closestPoint;
                t = 1.0f;
            }

            return minDistance;
        }

        // Returns the distance between a line and a point.
        // t is the distance along the line of the closest point
        public static float LinePointDistance(Vector2 v, Vector2 w, Vector2 p, out float t)
        {
            // Return minimum distance between line segment vw and point p
            var l2 = (v - w).LengthSquared();  // i.e. |w-v|^2 -  avoid a sqrt
            if (l2 == 0.0f)  // v == w case
            {
                t = 0.5f;
                return (p - v).Length();
            }

            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            // We clamp t from [0,1] to handle points outside the segment vw.
            t = ((p - v).Dot(w - v) / l2).Clamp(0.0f, 1.0f);
            var closestPoint = v + t * (w - v);  // Projection falls on the segment
            return (p - closestPoint).Length();
        }


        /// <summary>
        /// Given an array size, and a value from 0 to 1.0 indicate a relative position on the array will return
        /// The lower bounding index (upper bound is index + 1) and a new value from 0 to 1.0 for the purpose of interpolation.
        /// If the value is out of bounds the array is either treated as circular, or the first or last pair of
        /// values.  
        /// </summary>
        public static (int, double) InterpolateArraySize(int count, double amount, bool circular)
        {
            if (count < 2) throw new Exception("Can only interpolate arrays of size 2 or more");
            var n = circular ? count + 1 : count;
            var index = n * amount;
            var lower = Math.Floor(index);
            var upper = Math.Ceiling(index);
            if (!circular)
            {
                // We need to clamp the lower index and the upper index
                if (lower < 0)
                {
                    var delta = -lower;
                    lower += delta;
                    //upper += delta;
                }
                else if (upper >= count)
                {
                    var delta = upper - (count - 1);
                    lower -= delta;
                    //upper -= delta;
                }
            }
            var rel = (index - lower);
            return ((int)lower, rel);
        }
    }
}
