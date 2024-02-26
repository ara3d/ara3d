using System;
using UnityEngine;

namespace Ara3D.ProceduralGeometry.Unity
{
    public struct InstanceProps
    {
        public Matrix4x4 mat;
        public Vector4 color;

        public static int Size()
        {
            return
                sizeof(float) * 4 * 4 + // matrix;
                sizeof(float) * 4;      // color;
        }
    }

    public class InstancedMeshDrawer
    {
        private readonly Mesh _mesh;
        private readonly Material _material;
        private readonly InstanceProps[] _props;
        private int _count => _props.Length;

        private readonly ComputeBuffer _argsBuffer;
        private readonly Bounds _bounds;

        public InstancedMeshDrawer(Mesh mesh, Material material, InstanceProps[] props)
        {
            _mesh = mesh;
            _material = new Material(material);
            _props = props;

            // Boundary surrounding the meshes we will be drawing.  Used for occlusion.
            // TODO: this is currently incorrect. 
            _bounds = new Bounds(Vector3.zero, Vector3.one * (1000 + 1));

            // Argument buffer used by DrawMeshInstancedIndirect.
            var args = new uint[5] { 0, 0, 0, 0, 0 };
            // Arguments for drawing mesh.
            // 0 == number of triangle indices, 1 == population, others are only relevant if drawing submeshes.
            args[0] = (uint)_mesh.GetIndexCount(0);
            args[1] = (uint)_count;
            args[2] = (uint)_mesh.GetIndexStart(0);
            args[3] = (uint)_mesh.GetBaseVertex(0);
            _argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            _argsBuffer.SetData(args);

            var meshPropertiesBuffer = new ComputeBuffer(_count, InstanceProps.Size());
            meshPropertiesBuffer.SetData(_props);
            _material.SetBuffer("_Properties", meshPropertiesBuffer);
        }

        public void Draw()
        {
            UnityEngine.Graphics.DrawMeshInstancedIndirect(_mesh, 0, _material, _bounds, _argsBuffer);
        }
    }
}