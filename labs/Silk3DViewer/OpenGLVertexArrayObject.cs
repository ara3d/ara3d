using Silk.NET.OpenGL;
using System;
using System.Diagnostics;

namespace Tutorial
{
    public class OpenGLVertexArrayObject : OpenGLObject, IDisposable, IBindable
    {
        public OpenGLBuffer VertexBufferObject { get; }
        public OpenGLBuffer ElementBufferObject { get; }

        public OpenGLVertexArrayObject(GL gl)
            : base(gl, gl.GenVertexArray())
        {
            ElementBufferObject = new OpenGLBuffer(Context, BufferTargetARB.ElementArrayBuffer);
            VertexBufferObject= new OpenGLBuffer(Context, BufferTargetARB.ArrayBuffer);
            Bind();
            Context.EnableVertexAttribArray(0);
            VertexAttributePointer(0, 3, 6 * 4, 0 * 4);
            Context.EnableVertexAttribArray(1);
            VertexAttributePointer(1, 3, 6 * 4, 3 * 4);
        }

        // https://registry.khronos.org/OpenGL-Refpages/gl4/html/glVertexAttribPointer.xhtml
        // 
        public unsafe void VertexAttributePointer(uint index, int componentCount, uint vertexStrideInBytes, int offsetInBytes)
        {
            Debug.Assert(componentCount >= 1 && componentCount <= 4);
            Debug.Assert(vertexStrideInBytes <= 255);
            Debug.Assert(offsetInBytes <= 255);
            Context.VertexAttribPointer(index, componentCount, 
                VertexAttribPointerType.Float, false,
                vertexStrideInBytes, (void*)offsetInBytes);
            Context.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            Context.BindVertexArray(Handle);
            VertexBufferObject.Bind();
            ElementBufferObject.Bind();
        }

        public void Unbind()
        {
            // You MUST unbind the vertex array first, before unbinding the other buffers.
            // If you forget to do it in this order, the buffer will be unbound from the vertex array,
            // meaning you'll see incorrect results when you render the object.
            Context.BindVertexArray(0);
            VertexBufferObject.Unbind();
            ElementBufferObject.Unbind();
        }

        public void Dispose()
        {
            Unbind();
            Context.DeleteVertexArray(Handle);
            VertexBufferObject.Dispose();
            ElementBufferObject.Dispose();
        }
    }
}
