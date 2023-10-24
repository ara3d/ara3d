using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ara3D.Math;
using Ara3D.Util;
using Ara3D.Collections;
using LineApprox = System.Tuple<Ara3D.Math.Int2, Ara3D.Math.Int2>;

namespace Ara3D.Geometry
{
    public static class PerimeterProjection
    {
        public static Int2 ToXYApproximation(this Vector3 v, float tolerance)
            => new Int2((int)(v.X / tolerance), (int)(v.Y / tolerance));

        public static Vector2 FromXYApproximation(this Int2 v, float tolerance)
            => new Vector2(v.X * tolerance, v.Y * tolerance);

        public static LineApprox ToXYApproximation(this Line line, float tolerance)
            => Tuple.Create(line.A.ToXYApproximation(tolerance), line.B.ToXYApproximation(tolerance));

        public static double Angle(this Int2 v)
            => System.Math.Atan2(v.Y, v.X);

        public static double Angle(this Int2 a, Int2 b)
            => (a - b).Angle();

        public static IEnumerable<Vector2> PerimeterXY(this IMesh mesh, float tolerance = 0.001f)
        {
            var srcLines = mesh.GetAllEdgesAsLines();
            var approxLines = srcLines.Select(line => line.ToXYApproximation(tolerance));
            var lineSet = new HashSet<LineApprox>(approxLines.ToArrayInParallel());
            Debug.WriteLine($"Went from {srcLines.Count} to {lineSet.Count}");
            var d = new DictionaryOfLists<Int2, LineApprox>();
            foreach (var ab in lineSet)
            {
                d.Add(ab.Item1, ab);
                d.Add(ab.Item2, ab);
            }
            var r = new List<Vector2>();
            if (d.Count == 0)
                return r;

            var firstKey = d.Keys.Minimize(int.MaxValue, ab => ab.X.Min(ab.Y));
            var currentKey = firstKey;
            var prevAngle = 0.0;

            // If we can't find the point in the dictionary we have completed 
            while (d.ContainsKey(currentKey))
            {
                // Find the candidate points;
                var candidates = d[currentKey].Select(line => line.Item1 == currentKey ? line.Item2 : line.Item1);

                // Find the best match by maximizing angle 
                var bestMatch = candidates.Maximize(0.0, c => currentKey.Angle(c) - prevAngle);

                // Update the return set
                r.Add(bestMatch.FromXYApproximation(tolerance));

                // Now save the angle for the next stage. 
                prevAngle = currentKey.Angle(bestMatch);

                // Remove this key from the dictionary 
                d.Remove(currentKey);

                // Now we are at a new point 
                currentKey = bestMatch;
            }

            return r;
        }

        public static List<List<Vector2>> GeneratePerimeter(this IMesh mesh, Vector3 planeNormal, float degenerateSegmentEpsilon = 10.0f, float edgeLoopThreshold = 1e-6f)
        {
            var q = GetClusterRotation(planeNormal.Normalize(), Vector3.UnitZ);

            var segments = mesh.BuildBoundarySegments(null,
                input => input.Transform(q).ToVector2());

            segments = segments.SplitIntersectingCurves(edgeLoopThreshold * 50);

            var graph = segments.BuildConnectionGraph(edgeLoopThreshold * 50);
            var loops = graph.FindGraphLoops(edgeLoopThreshold * 50);

            loops = loops.RemoveDuplicatedVertices(edgeLoopThreshold * 50);

            while (true)
            {
                var newLoops = loops.RemoveDegenerateSegmentsLooping(edgeLoopThreshold);
                if (newLoops.Count == loops.Count)
                {
                    var notEqual = false;
                    for (var i = 0; i < newLoops.Count; i++)
                    {
                        if (newLoops[i].Count != loops[i].Count)
                        {
                            notEqual = true;
                            break;
                        }
                    }

                    if (!notEqual)
                    {
                        break;
                    }
                }

                loops = newLoops;
            }

            return loops;
        }

        // TODO: move this into the Math project  
        public static bool IsNaN(this Quaternion q)
            => q.X.IsNaN() || q.Y.IsNaN() || q.Z.IsNaN() || q.W.IsNaN();

        // Finds all of the intersections between the curves and splits them
        // Also adds a pseudo intersection at the left-most point, this gives the FindGraphLoops 
        // algorithm a good starting point
        public static List<List<Vector2>> SplitIntersectingCurves(this List<List<Vector2>> curves, float threshold)
        {
            var thresholdSquared = threshold * threshold;
            var curveList = curves.ToSequence();

            var intersections = new Dictionary<List<Vector2>, List<Tuple<float, float, List<Vector2>>>>(); // Madness in great ones must not unwatched go.
            var leftMostPoint = Vector2.MaxValue;
            List<Vector2> leftMostCurve = null;
            var leftMostIntersectionPoint = 0.0f;

            foreach (var currentCurveA in curveList.Enumerate())
            {
                for (var i = 0; i < currentCurveA.Count; i++)
                {
                    var point = currentCurveA[i];
                    if (point.X < leftMostPoint.X || (point.X == leftMostPoint.X && point.Y < leftMostPoint.Y))
                    {
                        leftMostPoint = point;
                        leftMostIntersectionPoint = i;
                        leftMostCurve = currentCurveA;
                    }
                }

                foreach (var currentCurveB in curveList.Enumerate())
                {
                    if (currentCurveA != currentCurveB)
                    {
                        float t, u;
                        var distance = IntersectCurves(currentCurveA, currentCurveB, out t, out u);
                        if (distance < threshold)
                        {
                            if (!intersections.ContainsKey(currentCurveA))
                            {
                                intersections[currentCurveA] = new List<Tuple<float, float, List<Vector2>>>();
                            }
                            if (!intersections.ContainsKey(currentCurveB))
                            {
                                intersections[currentCurveB] = new List<Tuple<float, float, List<Vector2>>>();
                            }

                            intersections[currentCurveA].Add(new Tuple<float, float, List<Vector2>>(distance, t, currentCurveB));
                            intersections[currentCurveB].Add(new Tuple<float, float, List<Vector2>>(distance, u, currentCurveA));
                        }
                    }
                }
            }

            // split the left most curve so that FindGraphLoops has a good starting point
            if (leftMostIntersectionPoint != 0.0 && leftMostIntersectionPoint != leftMostCurve.Count - 1)
            {
                intersections[leftMostCurve].Add(new Tuple<float, float, List<Vector2>>(0.0f, leftMostIntersectionPoint, null));
            }

            var edgeLoops = new List<List<Vector2>>();
            foreach (var intersection in intersections)
            {
                var splits = intersection.Value.Select(x => x.Item2);

                edgeLoops.AddRange(intersection.Key.SplitCurve(splits));
            }

            return edgeLoops;
        }

        // Cuts the curve between min and max and returns the part in between
        public static List<List<Vector2>> SplitCurve(this List<Vector2> curve, IEnumerable<float> splits)
        {
            var result = new List<List<Vector2>>();
            if (!splits.Any())
            {
                result.Add(curve);
                return result;
            }

            splits = splits.OrderBy(x => x).ToList();
            var newSplits = new List<float>();
            if (splits.First() != 0.0f)
            {
                newSplits.Add(0.0f);
            }

            newSplits.AddRange(splits);

            if (splits.Last() < curve.Count - 1)
            {
                newSplits.Add(curve.Count - 1);
            }

            for (var i = 0; i < newSplits.Count - 1; i++)
            {
                var min = newSplits[i];
                var max = newSplits[i + 1];

                var clampedCurve = curve.ClampCurve(min, max);

                if (clampedCurve.CurveLength() > 1e-5f)
                {
                    result.Add(clampedCurve);
                }
            }

            return result;
        }


        // Cuts the curve between min and max and returns the part in between
        public static List<Vector2> ClampCurve(this List<Vector2> curve, float min, float max)
        {
            var floorMin = (float)System.Math.Floor(min);
            var floorMax = (float)System.Math.Floor(max);
            var fracMin = min - floorMin;
            var fracMax = max - floorMax;

            if (fracMin == 0.0 && floorMin > 0)
            {
                floorMin--;
                fracMin++;
            }
            if (fracMax == 0.0 && floorMax > 0)
            {
                floorMax--;
                fracMax++;
            }

            var indexMin = (int)floorMin;
            var indexMax = (int)floorMax;

            var result = new List<Vector2>();
            if (fracMin != 1.0)
            {
                result.Add(curve[indexMin].Lerp(curve[indexMin + 1], (float)fracMin));
            }
            for (var i = indexMin + 1; i <= indexMax; i++)
            {
                result.Add(curve[i]);
            }
            result.Add(curve[indexMax].Lerp(curve[indexMax + 1], (float)fracMax));

            return result;
        }

        // Returns the length of the curve
        public static float CurveLength(this List<Vector2> curve)
        {
            var curveLength = 0.0f;
            for (var x = 0; x < curve.Count - 1; x++)
            {
                curveLength += (curve[x] - curve[x + 1]).Length();
            }

            return curveLength;
        }

        // Returns min distance between piecewise linear curves A and B.
        // t = distance along curve A
        // u = distance along curve B
        // TODO: Return multiple intersections within a tolerance
        public static float IntersectCurves(List<Vector2> segmentsA, List<Vector2> segmentsB, out float t, out float u)
        {
            var minDistance = float.MaxValue;
            t = 0.0f;
            u = 0.0f;

            for (var segmentAIndex = 0; segmentAIndex < segmentsA.Count - 1; segmentAIndex++)
            {
                var v1a = segmentsA[segmentAIndex];
                var v1b = segmentsA[segmentAIndex + 1];

                for (var segmentBIndex = 0; segmentBIndex < segmentsB.Count - 1; segmentBIndex++)
                {
                    var v2a = segmentsB[segmentBIndex];
                    var v2b = segmentsB[segmentBIndex + 1];

                    float fracT, fracU;
                    var distance = GeometryCuttingUtils.LineLineDistance(v1a, v1b, v2a, v2b, out fracT, out fracU);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        t = segmentAIndex + fracT;
                        u = segmentBIndex + fracU;
                    }
                }
            }

            return minDistance;
        }

        public static List<List<Vector2>> RemoveDuplicatedVertices(this List<List<Vector2>> edgeLoops, float threshold)
        {
            var thresholdSquared = threshold * threshold;
            foreach (var edgeLoop in edgeLoops)
            {
                for (var i = 0; i < edgeLoop.Count; i++)
                {
                    if ((edgeLoop[i] - edgeLoop[(i + 1) % edgeLoop.Count]).LengthSquared() <= thresholdSquared)
                    {
                        edgeLoop.RemoveAt(i);
                        i--;
                    }
                }
            }

            return edgeLoops;
        }

        public static Quaternion GetClusterRotation(Vector3 clusterNormal, Vector3 alignTo, float tolerance = 0.001f)
        {
            var c = clusterNormal.Cross(alignTo);
            var d = clusterNormal.Dot(alignTo);

            if (d >= 1f - tolerance)
                return Quaternion.Identity;

            if (d <= -(1f - tolerance))
            {
                // Need an axis of rotation that is perpendicular to the two input vectors
                var axis = clusterNormal.Cross(alignTo.Dot(Vector3.UnitY) > 0.999f ? Vector3.UnitZ : Vector3.UnitY);
                return Quaternion.CreateFromAxisAngle(axis, (float)System.Math.PI);
            }

            var s = (float)System.Math.Sqrt((1.0f + d) * 2.0f);
            var invs = 1.0f / s;

            var q = new Quaternion(c * invs, s * 0.5f);
            q = q.Normalize();

            Debug.Assert(!q.IsNaN());
            return q;
        }

        public static void AddEdgeToDictionary(Dictionary<Tuple<int, int>, int> d, int x, int y)
        {
            var key = new Tuple<int, int>(x, y);
            if (!d.ContainsKey(key))
                d[key] = 1;
            else
                d[key]++;
        }

        private class EdgeEqualityComparer : IEqualityComparer<Tuple<int, int>>
        {
            public bool Equals(Tuple<int, int> x, Tuple<int, int> y)
                => ((x.Item1 == y.Item1) && (x.Item2 == y.Item2)) || ((x.Item1 == y.Item2) && (x.Item2 == y.Item1));

            public int GetHashCode(Tuple<int, int> obj)
                => obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
        }

        public static Dictionary<Tuple<int, int>, int> BuildEdgeDictionary(this IMesh mesh)
        {
            var edges = new Dictionary<Tuple<int, int>, int>(new EdgeEqualityComparer());

            var indices = mesh.Indices;

            for (var i = 0; i < indices.Count; i += 3)
            {
                var i0 = indices[i + 0];
                var i1 = indices[i + 1];
                var i2 = indices[i + 2];

                AddEdgeToDictionary(edges, i0, i1);
                AddEdgeToDictionary(edges, i1, i2);
                AddEdgeToDictionary(edges, i2, i0);
            }

            return edges;
        }

        public static List<List<Vector2>> BuildBoundarySegments(this IMesh mesh, Dictionary<Tuple<int, int>, int> edgeDictionary, Func<Vector3, Vector2> transform)
        {
            var segments = new List<List<Vector2>>();

            var indices = mesh.Indices;
            var vertices = mesh.Vertices;

            for (var i = 0; i < indices.Count; i += 3)
            {
                var i0 = indices[i + 0];
                var i1 = indices[i + 1];
                var i2 = indices[i + 2];

                var v0 = transform(vertices[i0]);
                var v1 = transform(vertices[i1]);
                var v2 = transform(vertices[i2]);

                if (edgeDictionary == null || edgeDictionary[new Tuple<int, int>(i0, i1)] != 2) segments.Add(new List<Vector2> { v0, v1 });
                if (edgeDictionary == null || edgeDictionary[new Tuple<int, int>(i1, i2)] != 2) segments.Add(new List<Vector2> { v1, v2 });
                if (edgeDictionary == null || edgeDictionary[new Tuple<int, int>(i2, i0)] != 2) segments.Add(new List<Vector2> { v2, v0 });
            }

            return segments;
        }

        // Builds a node connection graph from a list of curves
        public static Dictionary<Vector2, List<Tuple<List<Vector2>, List<Vector2>>>> BuildConnectionGraph(this List<List<Vector2>> curves, float threshold)
        {
            var connectionGraph = new Dictionary<Vector2, List<Tuple<List<Vector2>, List<Vector2>>>>();
            var reverseCurves = new Dictionary<List<Vector2>, List<Vector2>>();
            var thresholdSquared = threshold * threshold;

            var pointList = new Vector2[curves.Count];
            for (var i = 0; i < curves.Count; i++)
            {
                var curve = curves[i];
                connectionGraph[curve.First()] = new List<Tuple<List<Vector2>, List<Vector2>>>();
                connectionGraph[curve.Last()] = new List<Tuple<List<Vector2>, List<Vector2>>>();

                var reverseCurve = new List<Vector2>(curve);
                reverseCurve.Reverse();
                reverseCurves[curve] = reverseCurve;
                reverseCurves[reverseCurve] = curve;
            }

            foreach (var point in connectionGraph)
            {
                foreach (var curve in curves)
                {
                    var distanceSquared = (curve.First() - point.Key).LengthSquared();
                    if (distanceSquared <= thresholdSquared)
                    {
                        point.Value.Add(new Tuple<List<Vector2>, List<Vector2>>(curve, reverseCurves[curve]));
                    }

                    distanceSquared = (curve.Last() - point.Key).LengthSquared();
                    if (distanceSquared <= thresholdSquared)
                    {
                        point.Value.Add(new Tuple<List<Vector2>, List<Vector2>>(reverseCurves[curve], curve));
                    }
                }
            }

            return connectionGraph;
        }

        // Build a loop around the perimeter of the graph
        // TODO: Handle multiple loops
        public static List<List<Vector2>> FindGraphLoops(this Dictionary<Vector2, List<Tuple<List<Vector2>, List<Vector2>>>> connectionGraph, float threshold)
        {
            // Start from the left most point, build the perimeter by selecting the connection with the minimum clockwise angle
            var thresholdSquared = threshold * threshold;
            var leftMostPoint = Vector2.MaxValue;
            foreach (var item in connectionGraph)
            {
                if (item.Value.Count < 2)
                {
                    continue;
                }
                if (item.Key.X < leftMostPoint.X || (item.Key.X == leftMostPoint.X && item.Key.Y < leftMostPoint.Y))
                {
                    leftMostPoint = item.Key;
                }
            }

            var visitedNodes = new Dictionary<List<Vector2>, bool>();

            var result = new List<List<Vector2>>();
            var loop = new List<Vector2>();
            var currentPointLast = leftMostPoint + new Vector2(0, 1);
            var currentPoint = leftMostPoint;
            var previousPoint = currentPoint;

            var segmentCount = 0;
            while (true)
            {
                if (!connectionGraph.ContainsKey(currentPoint))
                {
                    // Failed :(
                    result.Add(loop);
                    return result;
                }

                var list = connectionGraph[currentPoint].Where(x =>
                {
                    if (connectionGraph[x.Item1.Last()].Count <= 1)
                    {
                        // Don't go down a dead end, this might need improving with some kind of backtracking when a dead end was found
                        return false;
                    }

                    if (visitedNodes.ContainsKey(x.Item1))
                    {
                        return false;
                    }

                    return true;
                });
                var sortedList = list.OrderBy(x =>
                {
                    var s1 = (currentPoint - currentPointLast).Normalize();
                    var s2 = (x.Item1[1] - x.Item1[0]).Normalize();
                    var angle = (float)System.Math.Atan2(s1.Cross(s2), s1.Dot(s2));
                    return angle;
                }).ToList();

                if (sortedList.Count == 0)
                {
                    // Failed :(
                    result.Add(loop);
                    return result;
                }

                var nextConnection = sortedList.First();
                visitedNodes[nextConnection.Item1] = true;
                visitedNodes[nextConnection.Item2] = true;
                segmentCount++;
                loop.AddRange(nextConnection.Item1);
                previousPoint = currentPoint;
                currentPoint = nextConnection.Item1.Last();
                currentPointLast = nextConnection.Item1[nextConnection.Item1.Count - 2];

                if ((currentPoint - loop[0]).LengthSquared() < thresholdSquared)
                {
                    result.Add(loop);
                    break;
                }

                if (segmentCount > connectionGraph.Count)
                {
                    // Failed :(
                    result.Add(loop);
                    return result;
                }
            }

            return result;
        }

        // Remove degenerate segments
        public static List<List<Vector2>> RemoveDegenerateSegments(this List<List<Vector2>> edgeLoops, float epsilon = 1e-10f)
        {
            var result = new List<List<Vector2>>();
            foreach (var edgeLoop in edgeLoops)
            {
                if (edgeLoop.Count < 2)
                {
                    //result.Add(edgeLoop);
                    continue;
                }

                var newEdgeLoop = new List<Vector2>(edgeLoop);
                for (var i = 1; i < newEdgeLoop.Count - 1; i++)
                {
                    var v0 = newEdgeLoop[i - 1];
                    var v1 = newEdgeLoop[i];
                    var v2 = newEdgeLoop[i + 1];
                    var area = (v1 - v0).Cross(v1 - v2).Abs();
                    if (area < epsilon)
                    {
                        newEdgeLoop.RemoveAt(i);
                        i--;
                    }
                }

                if (newEdgeLoop.Count < 2)
                {
                    continue;
                }

                result.Add(newEdgeLoop);
            }

            return result;
        }
        public static List<List<Vector2>> RemoveDegenerateSegmentsLooping(this List<List<Vector2>> edgeLoops, float epsilon = 1e-10f)
        {
            var result = new List<List<Vector2>>();
            foreach (var edgeLoop in edgeLoops)
            {
                if (edgeLoop.Count < 2)
                {
                    //result.Add(edgeLoop);
                    continue;
                }

                var newEdgeLoop = new List<Vector2>(edgeLoop);
                for (var i = 0; i < newEdgeLoop.Count; i++)
                {
                    var v0 = newEdgeLoop[(i - 1 + newEdgeLoop.Count) % newEdgeLoop.Count];
                    var v1 = newEdgeLoop[i];
                    var v2 = newEdgeLoop[(i + 1) % newEdgeLoop.Count];
                    var area = (v1 - v0).Cross(v1 - v2).Abs();
                    if (area < epsilon)
                    {
                        newEdgeLoop.RemoveAt(i);
                        i--;
                    }
                }

                if (newEdgeLoop.Count < 2)
                {
                    continue;
                }

                result.Add(newEdgeLoop);
            }

            return result;
        }

        public static List<List<Vector2>> RemoveDegenerateSegments(this List<List<Vector2>> edgeLoops, float epsilon, Func<Vector2, Vector3> faceFunction)
        {
            var result = new List<List<Vector2>>();
            foreach (var edgeLoop in edgeLoops)
            {
                if (edgeLoop.Count < 2)
                {
                    //result.Add(edgeLoop);
                    continue;
                }

                var newEdgeLoop = new List<Vector2>(edgeLoop);
                for (var i = 1; i < newEdgeLoop.Count - 1; i++)
                {
                    var v0 = faceFunction(newEdgeLoop[i - 1]);
                    var v1 = faceFunction(newEdgeLoop[i]);
                    var v2 = faceFunction(newEdgeLoop[i + 1]);
                    var area = (v1 - v0).Cross(v1 - v2).LengthSquared();
                    if (area < epsilon * epsilon)
                    {
                        newEdgeLoop.RemoveAt(i);
                        i--;
                    }
                }

                if (newEdgeLoop.Count < 2)
                {
                    continue;
                }

                result.Add(newEdgeLoop);
            }

            return result;
        }
    }
}
