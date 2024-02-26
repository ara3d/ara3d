using System.Collections.Generic;
using Ara3D.UnityBridge;
using UnityEngine;

namespace Ara3D.ProceduralGeometry.Unity
{
    [ExecuteAlways]
    public class SceneRenderer2 : MonoBehaviour
    {
        public Material Material;

        public UnityMeshScene Scene;
        
        private List<Mesh> meshes = new List<Mesh>();
        
        public void Update()
        {
            for (var i = 0; i < meshes.Count; i++)
            {
                var mesh = meshes[i];
                var instanceSet = Scene.InstanceSets[i];
                const int MaxInstances = 1023;
                for (var j = 0; j < instanceSet.Matrices.Count; j += MaxInstances)
                {
                    var end = j + MaxInstances;
                    if (end > instanceSet.Matrices.Count)
                        end = instanceSet.Matrices.Count;
                    var cnt = end - j;
                    var mats = instanceSet.Matrices.GetRange(j, cnt);
                    UnityEngine.Graphics.DrawMeshInstanced(mesh, 0, Material, mats);
                }
            }
        }

        public void Init(UnityMeshScene scene)
        {
            meshes.Clear();
            foreach (var set in scene.InstanceSets)
            {
                var mesh = new Mesh();
                set.Mesh.AssignToMesh(mesh);
                meshes.Add(mesh);
            }
            Scene = scene;
        }
    }
}