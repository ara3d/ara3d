// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Ara3D.Buffers;
using Silk.NET.OpenGL;

namespace Tutorial
{
    public class Mesh : IDisposable
    {
        public Mesh(GL gl, float[] vertices, int[] indices)
        {
            GL = gl;
            Vertices = vertices;
            Indices = indices;
            VAO = new OpenGLVertexArrayObject(GL);
            VAO.Bind();
        }

        public float[] Vertices { get; }
        public int[] Indices { get; }
        public OpenGLVertexArrayObject VAO { get; set; }
        public GL GL { get; }

        public void Bind()
        {
            VAO.Bind();
            VAO.ElementBufferObject.SetData(Indices.ToBuffer());
            VAO.VertexBufferObject.SetData(Vertices.ToBuffer());
        }

        public void Unbind()
        {
            VAO.Unbind();
        }

        public void Dispose()
        {
            VAO.Dispose();
        }
    }
}
