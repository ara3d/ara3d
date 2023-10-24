using Ara3D.Geometry;
using Ara3D.Graphics;
using UnityEngine;

namespace Ara3D.UnityBridge
{
    /// <summary>
    /// A Deformer will make a copy of a mesh, and update it according to the deformation function. 
    /// </summary>
    public abstract class Deformer : MonoBehaviour
    {
        private UnityMesh _source;

        public IRenderMesh SourceMesh { get; private set; }

        public abstract IMesh Deform(IMesh g);

        public virtual void Update()
        {
            //Debug.Log("Update called");
            var mesh = this.GetMesh();
            if (mesh == null) return;
            _source = _source ?? (_source = new UnityMesh(mesh));

            if (_source != null)
            {
                var g = Deform(SourceMesh.ToIMesh());

                // TODO: support more types
                mesh.Clear();
                mesh.vertices = g.Vertices.ToUnity();
                mesh.triangles = g.Indices().ToUnityIndexBuffer();
                mesh.RecalculateNormals();
            }
        }

        public virtual void Reset()
        {
            //Debug.Log("Reset");
            Disable();
        }

        public void Enable()
        {
            //Debug.Log("Enabling");
        }

        public void Disable()
        {
            //Debug.Log("Disabling");
            // Restore the original mesh
            _source?.AssignToMesh(this.GetMesh());
            _source = null;
        }

        public virtual void OnDisable()
        {
            Disable();
        }

        public virtual void OnDestroy()
        {
            Disable();
        }
    }
}
