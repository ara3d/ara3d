using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D;
using Ara3D.Mathematics;
using Ara3D.Utils;
using g3;

namespace Identification
{
    public class ObjMesh
    {
        public DMesh3 Mesh { get; }
        public DMeshAABBTree3 Tree { get; }
        public DAABox Box { get; }
        public List<Vector3d> Vertices { get; }

        public ObjMesh(DMesh3 mesh)
        {
            Mesh = mesh;
            Vertices = Mesh.Vertices().ToList();
            Box = Mesh.GetBounds().ToAra3D();
            Tree = new DMeshAABBTree3(mesh);
            Tree.Build();
        }

        public double MaxDistanceFrom(ObjMesh mesh)
            => mesh.Vertices.Max(Tree.DistanceToTree);

        public static ObjMesh Load(FilePath file)
        {
            var builder = new DMesh3Builder();
            var reader = new StandardMeshReader() { MeshBuilder = builder };
            var result = reader.Read(file, ReadOptions.Defaults);
            if (result.code != IOCode.Ok)
                throw new Exception($"Failed to load file {file}");
            if (builder.Meshes.Count != 1)
                throw new Exception($"Expected 1 mesh not {builder.Meshes.Count}");
            return new ObjMesh(builder.Meshes[0]);
        }
    }
}