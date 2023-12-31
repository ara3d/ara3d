﻿using System;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public enum Axis
    {
        X,
        Y,
        Z,
        NegativeX,
        NegativeY,
        NegativeZ,
    }

    public class Amount
    {

    }

    public static class Modifiers
    {
        public static IMesh Faceted(this IMesh self)
            => self.Indices().Select(i => self.Vertices[i])
                .ToTriMesh(self.Indices().Count.Range());

        public static Vector3 Skew(Vector3 v, Line line, Vector3 from, Vector3 to)
            => throw new NotImplementedException();

        public static Vector3 Taper(Vector3 v)
            => throw new NotImplementedException();

        public static Vector3 Bend(Vector3 v)
            => throw new NotFiniteNumberException();

        public static Vector3 Twist(Vector3 v)
            => throw new NotImplementedException();

        public static Line Axis(this AABox box, Axis axis)
            => throw new NotImplementedException();

        public static Vector3 Translate(Vector3 v, Vector3 amount)
            => throw new NotImplementedException();

        public static Func<Vector3, Vector3> ApplyFallOff(Func<Vector3, Vector3> f, Func<Vector3, Amount> fallOff)
            => throw new NotImplementedException();
        
        public static Func<Vector3, Amount> AmountAlongLine(Vector3 v, Line line)
            => throw new NotImplementedException();

        public static Func<Vector3, Amount> AmountAlongAxis(Vector3 v, AABox box, Axis axis)
            => throw new NotImplementedException();

        public static Func<Vector3, Amount> DistanceAsAmount(Vector3 v, Vector3 center, float max)
            => throw new NotImplementedException();

        public static Vector3 DeformAlong(Vector3 v, Func<Vector3, Vector3> deform)
            => throw new NotImplementedException();

    }
}