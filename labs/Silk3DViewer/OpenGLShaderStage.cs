using System;
using Silk.NET.OpenGL;

namespace Tutorial;

public class OpenGLShaderStage : OpenGLObject, IDisposable
{
    public string Code { get; }
    public ShaderType Type { get; }
    public string Error { get; }

    public OpenGLShaderStage(GL gl, ShaderType type, string code)
        : base(gl, gl.CreateShader(type))
    {
        Code = code;
        Type = type;
        Context.ShaderSource(Handle, Code);
        Context.CompileShader(Handle);
        Error = Context.GetShaderInfoLog(Handle);
    }

    public void Dispose()
    {
        Context.DeleteShader(Handle);
    }
}