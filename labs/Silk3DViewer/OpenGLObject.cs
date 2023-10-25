using Silk.NET.OpenGL;

namespace Tutorial;

public class OpenGLObject
{
    public uint Handle { get; }
    public GL Context { get; }

    public OpenGLObject(GL context, uint handle)
    {
        Context = context;
        Handle = handle;
    }
}