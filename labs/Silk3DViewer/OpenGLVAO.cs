using Silk.NET.OpenGL;
using System;
using System.Diagnostics;
using Ara3D.Graphics;

namespace Tutorial
{
    public class OpenGLVAO : OpenGLObject, IDisposable, IBindable
    {
        public int NumFaces => RenderMesh.GetNumFaces();
        public int NumVertices => RenderMesh.GetNumVertices();
        public IRenderMesh RenderMesh { get; }
        public OpenGLBuffer VertexPositionBuffer { get; }
        public OpenGLBuffer VertexColorBuffer { get; }
        public OpenGLBuffer ElementBufferObject { get; }

        public OpenGLVAO(GL gl, IRenderMesh mesh)
            : base(gl, gl.GenVertexArray())
        {
            RenderMesh = mesh;

            Bind();

            var indexData = mesh.IndexBuffer;
            ElementBufferObject = new OpenGLBuffer(Context, indexData, BufferTargetARB.ElementArrayBuffer);

            VertexPositionBuffer = CreateAttributeBuffer(0, mesh.PositionBuffer);
            //VertexColorBuffer = CreateAttributeBuffer(1, mesh.ColorBuffer);
        }

        // https://registry.khronos.org/OpenGL-Refpages/gl4/html/glVertexAttribPointer.xhtml
        // 
        public OpenGLBuffer CreateAttributeBuffer(uint index, IRenderBuffer buffer)
        {
            var r = new OpenGLBuffer(Context, buffer, BufferTargetARB.ArrayBuffer);
            SetupVertexAttributePointer(index, buffer.Arity, (uint)buffer.ElementSize, 0);
            return r;
        }

        public unsafe void SetupVertexAttributePointer(uint index, int componentCount, uint vertexStrideInBytes, int offsetInBytes)
        {
            Debug.Assert(componentCount >= 1 && componentCount <= 4);
            Debug.Assert(vertexStrideInBytes <= 255);
            Debug.Assert(offsetInBytes <= 255);
            Context.EnableVertexAttribArray(index);
            Context.VertexAttribPointer(index, componentCount, 
                VertexAttribPointerType.Float, false,
                vertexStrideInBytes, (void*)offsetInBytes);
        }

        public void Bind()
        {
            Context.BindVertexArray(Handle);
        }

        public void Unbind()
        {
            // You MUST unbind the vertex array first, before unbinding the other buffers.
            // If you forget to do it in this order, the buffer will be unbound from the vertex array,
            // meaning you'll see incorrect results when you render the object.
            Context.BindVertexArray(0);
            VertexPositionBuffer.Unbind();
            VertexColorBuffer.Unbind();
            ElementBufferObject.Unbind();
        }

        public void Dispose()
        {
            Unbind();
            Context.DeleteVertexArray(Handle);
            VertexPositionBuffer.Dispose();
            VertexColorBuffer.Dispose();
            ElementBufferObject.Dispose();
        }
    }
}
