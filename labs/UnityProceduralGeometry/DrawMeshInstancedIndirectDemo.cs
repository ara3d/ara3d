using UnityEngine;

namespace Ara3D.ProceduralGeometry.Unity
{
    // https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstancedIndirect.html
    // https://toqoz.fyi/thousands-of-meshes.html
    [ExecuteAlways]
    public class DrawMeshInstancedIndirectDemo : MonoBehaviour
    {
        public int Count = 1000;
        public float range;

        public Mesh Mesh;
        public Material Material;

        private ComputeBuffer _meshPropertiesBuffer;
        private ComputeBuffer _argsBuffer;

        private Mesh _mesh;
        private Bounds _bounds;

        // Mesh Properties struct to be read from the GPU.
        // Size() is a convenience funciton which returns the stride of the struct.
        private struct MeshProperties
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
        
        private void InitializeBuffers()
        {
            // Boundary surrounding the meshes we will be drawing.  Used for occlusion.
            // TODO: this is currently incorrect. 
            _bounds = new Bounds(transform.position, Vector3.one * (1000 + 1));

            // Argument buffer used by DrawMeshInstancedIndirect.
            var args = new uint[5] { 0, 0, 0, 0, 0 };
            // Arguments for drawing mesh.
            // 0 == number of triangle indices, 1 == population, others are only relevant if drawing submeshes.
            args[0] = (uint)Mesh.GetIndexCount(0);
            args[1] = (uint)Count;
            args[2] = (uint)Mesh.GetIndexStart(0);
            args[3] = (uint)Mesh.GetBaseVertex(0);
            _argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            _argsBuffer.SetData(args);

            // Initialize buffer with the given population.
            var properties = new MeshProperties[Count];
            for (var i = 0; i < Count; i++)
            {
                var props = new MeshProperties();
                var position = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
                var rotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
                var scale = Vector3.one;

                props.mat = Matrix4x4.TRS(position, rotation, scale);
                props.color = Color.Lerp(Color.red, Color.blue, Random.value);

                properties[i] = props;
            }

            _meshPropertiesBuffer = new ComputeBuffer(Count, MeshProperties.Size());
            _meshPropertiesBuffer.SetData(properties);
            Material.SetBuffer("_Properties", _meshPropertiesBuffer);
        }

        /// <summary>
        /// Frame update function called by Unity
        /// </summary>
        private void Update()
        {
            if (_mesh != Mesh)
            {
                CleanUp();
                Mesh = _mesh;
                InitializeBuffers();
            }
            UnityEngine.Graphics.DrawMeshInstancedIndirect(Mesh, 0, Material, _bounds, _argsBuffer);
        }

        /// <summary>
        /// On element disabled, called by Unity
        /// </summary>
        private void OnDisable()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            // Release gracefully.
            _meshPropertiesBuffer?.Release();
            _meshPropertiesBuffer = null;
            _argsBuffer?.Release();
            _argsBuffer = null;
            Mesh = _mesh = null;
        }
    }
}