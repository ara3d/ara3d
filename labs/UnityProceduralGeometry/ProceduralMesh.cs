using UnityEngine;

namespace Ara3D.UnityBridge
{
    /// <summary>
    /// Manipulate the Buffer mesh to your heart's content, and call "UpdateTarget" whenever you want. 
    /// </summary>
    public class ProceduralMesh
    {
        public UnityTriMesh Original { get; private set; }
        public UnityTriMesh Buffer { get; private set; }
        public Mesh Target { get; private set; }

        public ProceduralMesh(Mesh mesh)
        {
            Target = mesh;
            Original = new UnityTriMesh(Target);
            Buffer = Original.Clone();  
        }

        public void ResetTarget()
        {
            Buffer.CopyFrom(Original);
            UpdateTarget();
        }

        public void UpdateTarget()
        {
            Buffer.AssignToMesh(Target);
        }
    }
}
