using System.Collections.Generic;
using UnityEngine;

namespace Ara3D.UnityBridge
{
    public class UnityMeshInstanceSet
    {
        public UnityMesh Mesh;
        public List<Matrix4x4> Matrices = new List<Matrix4x4>();
    }
}