using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace Ara3D.ProceduralGeometry.Unity
{
    public struct CloneInstance
    { }

    public class CloneData
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<float> Ages;
        public NativeArray<int> Enabled;
        public NativeArray<int> MeshIndices;
        public NativeArray<Quaternion> Rotations;
        public NativeArray<Vector3> Scales;
        public NativeArray<Color> Colors;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;
        public NativeArray<Quaternion> RotationalVelocities;
        public NativeArray<float> Masses;

        public int Size { get; }

        public void Resize()
        {

        }
    }
}
