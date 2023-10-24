using System;
using Ara3D.Math;
using UnityEngine;
using Matrix4x4 = Ara3D.Math.Matrix4x4;
using Quaternion = Ara3D.Math.Quaternion;
using Vector3 = Ara3D.Math.Vector3;

namespace Ara3D.UnityBridge
{
    [Serializable]
    public class TransformProperty
    {
        public Translation Translation;
        public Scale Scale;
        public Rotation Rotation;

        public Matrix4x4 Matrix => Matrix4x4.CreateTRS(Translation.Vector, Rotation.Quaternion, Scale.Vector);
    }

    [Serializable]
    public class Translation
    {
        [Range(-100, 100)] public float X;
        [Range(-100, 100)] public float Y;
        [Range(-100, 100)] public float Z;
        public Vector3 Vector => new Vector3(X, Y, Z);
    }

    [Serializable]
    public class Scale
    {
        [Range(-10, 10)] public float X = 1;
        [Range(-10, 10)] public float Y = 1;
        [Range(-10, 10)] public float Z = 1;

        public Vector3 Vector => new Vector3(X, Y, Z);
    }

    [Serializable]
    public class Rotation
    {
        [Range(-360, 360)] public float X;
        [Range(-360, 360)] public float Y;
        [Range(-360, 360)] public float Z;

        public Vector3 EulerDegrees => new Vector3(X, Y, Z);
        public Quaternion Quaternion => Ara3D.Math.Quaternion.CreateFromYawPitchRoll(
            Y * (float)Constants.DegreesToRadians,
            X * (float)Constants.DegreesToRadians,
            Z * (float)Constants.DegreesToRadians);
    }

}
