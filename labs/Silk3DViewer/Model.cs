// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ara3D.Serialization.VIM;
using Ara3D.Geometry;
using Ara3D.Graphics;
using Vector4 = System.Numerics.Vector4;

namespace Tutorial
{
    public class Model
    {
        public Model(GL gl, string path)
        {
            _gl = gl;
            LoadModel(path);
        }

        private readonly GL _gl;
        public List<OpenGLVAO> Meshes { get; protected set; } = new List<OpenGLVAO>();
        
        private void LoadModel(string path)
        {
            var sw = Stopwatch.StartNew();
            var vim = Serializer.Deserialize(path);
            Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");

            var g = vim.Geometry;
            Console.WriteLine($"number of meshes {g.Meshes.Count}");
            Console.WriteLine($"number of vertices {g.Vertices.Count}");
            Console.WriteLine($"number of uvs {g.AllVertexUvs.Count}");
            Console.WriteLine($"number of sub-meshes {g.SubmeshIndexCount.Count}");
            Console.WriteLine($"number of faces {g.Indices.Count / 3}");
            Console.WriteLine($"number of instances {g.InstanceMeshes.Count}");
            Console.WriteLine($"number of materials {g.Materials.Count}");

            var mesh = g;
            //var mesh = Ara3D.Geometry.Meshes.TorusMesh(20, 5, 100, 20);
            //var mesh = Meshes.TorusMesh(20, 5, 100, 20).Scale(2);
            //var mesh = Meshes.TorusMesh(20, 5, 10, 5);
            //var mesh = Meshes.TorusMesh(20, 5, 4, 4);
            //var mesh = Meshes.SquareMesh;
            //var mesh = Meshes.SquareMesh.Scale(3, 4, 5);
            //var mesh = Meshes.Tetrahedron;
            //var mesh = Meshes.Cube;
            //var mesh = Meshes.Dodecahedron;
            //var mesh = Meshes.Icosahedron;
            //var mesh = Meshes.Cylinder(10, 5).Scale(10);
            //var mesh = Meshes.Pl
            Meshes.Add(ToVAO(mesh.Triangulate()));
        }

        public static Random Random = new Random();

        public Vector4 NewRandomColor()
        {
            return new Vector4(
                Random.NextSingle(), Random.NextSingle(),
                Random.NextSingle(), Random.NextSingle());
        }

        public OpenGLVAO ToVAO(ITriMesh triMesh)
        {
            var c = NewRandomColor();
            var rm = triMesh.ToRenderMesh();
            return new OpenGLVAO(_gl, rm);
        }
    }
}
