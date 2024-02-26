using System.Collections.Generic;
using System.Linq;
using Ara3D.UnityBridge;
using UnityEngine;

namespace Ara3D.ProceduralGeometry.Unity
{
    [ExecuteAlways]
    public class SceneRenderer : MonoBehaviour
    {
        public Material Material;

        public UnityMeshScene Scene;
        
        private List<InstancedMeshDrawer> drawers = new List<InstancedMeshDrawer>();
        
        public void Update()
        {
            foreach (var drawer in drawers)
            {
                drawer.Draw();
            }
        }

        public void Init(UnityMeshScene scene)
        {
            drawers.Clear();
            foreach (var set in scene.InstanceSets)
            {
                // If there are no instances, skip it
                if (set.Matrices.Count <= 0)
                    continue;

                // If there are no faces, skip it
                if (set.Mesh.Faces.Count == 0)
                    continue;

                var mesh = new Mesh();
                set.Mesh.AssignToMesh(mesh);
                var drawer = new InstancedMeshDrawer(mesh, Material,
                    set.Matrices.Select(m => 
                        new InstanceProps()
                        {
                            color = set.Color,
                            mat = m
                        })
                        .ToArray());

                drawers.Add(drawer);
            }
            Scene = scene;
        }
    }
}