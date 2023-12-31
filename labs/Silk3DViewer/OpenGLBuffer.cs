using Silk.NET.OpenGL;
using System;
using Ara3D.Buffers;

namespace Tutorial
{
    public interface IBindable
    {
        void Bind();
        void Unbind();
    }

    public class OpenGLBuffer : OpenGLObject, IDisposable, IBindable
    {
        public BufferTargetARB Target { get; }
        public BufferUsageARB Usage { get; }

        public OpenGLBuffer(GL context, IBuffer buffer, BufferTargetARB target, BufferUsageARB usage = BufferUsageARB.StaticDraw)
            : base(context, context.GenBuffer())
        {
            Target = target;
            Usage = usage;
            SetData(buffer);
        }

        public void Bind()
        {
            Context.BindBuffer(Target, Handle);
        }

        public void Unbind()
        {
            Context.BindBuffer(Target, 0);
        }

        public unsafe void SetData(IBuffer buffer)
        {
            Bind();
            var size = (nuint)buffer.GetNumBytes();
            buffer.WithPointer(ptr => Context.BufferData(Target, size, (void*)ptr, Usage));
        }

        public void Dispose()
        {
            Context.DeleteBuffer(Handle);
        }
    }
}
