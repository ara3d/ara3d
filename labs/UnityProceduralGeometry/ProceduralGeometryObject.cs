using Ara3D.Geometry;
using UnityEngine;

namespace Ara3D.UnityBridge
{
    public abstract class ProceduralGeometryObject : MonoBehaviour
    {
        private IMesh _newGeometry;
        private IMesh _oldGeometry;

        public virtual void Reset()
        {
            _newGeometry = null;
            _oldGeometry = null;
            Start();
        }

        public virtual void Update()
        {
            if (_newGeometry == null)
                _newGeometry = ComputeMesh();

            if (_newGeometry != _oldGeometry)
            {
                _oldGeometry = _newGeometry;
                this.UpdateMesh(_newGeometry);
            }
        }

        public virtual void OnEnable()
        {
            this.CreateMesh();
        }

        public virtual void Start()
        {
            Update();
        }

        public void OnValidate()
        {
            _newGeometry = ComputeMesh();
        }

        public abstract IMesh ComputeMesh();
    }
}
