using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Plato;

namespace PathTracer
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Vector3x8
    {
        public readonly Vector8 X;
        public readonly Vector8 Y;
        public readonly Vector8 Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3x8(Vector3 v)
            : this(v.X, v.Y, v.Z)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3x8(in Vector8 x, in Vector8 y, in Vector8 z) 
            => (X, Y, Z) = (x, y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator +(in Vector3x8 a, in Vector3x8 b) 
            => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator -(in Vector3x8 a, in Vector3x8 b) 
            => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator *(in Vector3x8 a, in float scalar) 
            => new(a.X * scalar, a.Y * scalar, a.Z * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator *(in Vector3x8 a, in Vector8 scalar)
            => new(a.X * scalar, a.Y * scalar, a.Z * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator *(in Vector3x8 a, in Vector3 scalar)
            => new(a.X * scalar.X, a.Y * scalar.Y, a.Z * scalar.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator /(in Vector3x8 a, in float scalar)
            => new(a.X / scalar, a.Y / scalar, a.Z / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator /(in Vector3x8 a, in Vector8 scalar)
            => new(a.X * scalar, a.Y * scalar, a.Z * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 operator /(in Vector3x8 a, in Vector3 scalar)
            => new(a.X / scalar.X, a.Y / scalar.Y, a.Z / scalar.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3x8 Min(in Vector3x8 b) 
            => new(X.Min(b.X), Y.Min(b.Y), Z.Min(b.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3x8 Max(in Vector3x8 b) 
            => new(X.Max(b.X), Y.Max(b.Y), Z.Max(b.Z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector8 Dot(in Vector3x8 b) 
            => X * b.X + Y * b.Y + Z * b.Z;

        public Vector8 LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Dot(this); 
        }

        public Vector8 Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => LengthSquared.Sqrt; 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3x8 Cross(in Vector3x8 a, in Vector3x8 b) 
            => new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        public Vector3x8 Normalize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X / Length, Y / Length, Z / Length);
        }

        public Vector3 this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X[index], Y[index], Z[index]);
        }
    }
}