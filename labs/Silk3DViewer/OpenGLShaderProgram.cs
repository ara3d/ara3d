using System;
using System.Numerics;
using Ara3D.Utils;
using Silk.NET.OpenGL;

namespace Tutorial
{
    public class OpenGLShaderProgram : OpenGLObject, IDisposable
    {
        public string Error { get; }
        public OpenGLShaderStage Vertex { get; }
        public OpenGLShaderStage Fragment { get; }

        public OpenGLShaderProgram(GL gl, string vertexShaderCode, string fragmentShaderCode)
            : base(gl, gl.CreateProgram())
        {
            Vertex = new OpenGLShaderStage(Context, ShaderType.VertexShader, vertexShaderCode);
            Fragment = new OpenGLShaderStage(Context, ShaderType.FragmentShader, fragmentShaderCode);

            Context.AttachShader(Handle, Vertex.Handle);
            Context.AttachShader(Handle, Fragment.Handle);
            Context.LinkProgram(Handle);
            Context.GetProgram(Handle, GLEnum.LinkStatus, out var status);
            Context.DetachShader(Handle, Vertex.Handle);
            Context.DetachShader(Handle, Fragment.Handle);
            
            Vertex.Dispose();
            Fragment.Dispose();

            if (status == 0)
            {
                Error = Context.GetProgramInfoLog(Handle);
                throw new Exception($"Program failed to link with error: {Context.GetProgramInfoLog(Handle)}");
            }
        }

        public void Use()
        {
            Context.UseProgram(Handle);
        }

        public int GetUniformLocation(string name)
        {
            var location = Context.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            return location;
        }

        public void SetUniform(string name, int value)
        {
            Context.Uniform1(GetUniformLocation(name), value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            Context.UniformMatrix4(GetUniformLocation(name), 1, false, (float*) &value);
        }

        public void SetUniform(string name, float value)
        {
            Context.Uniform1(GetUniformLocation(name), value);
        }

        public void Dispose()
        {
            Context.DeleteProgram(Handle);
        }

        public static OpenGLShaderProgram CreateFromFiles(GL gl, FilePath vertexShaderFile, FilePath fragmentShaderFile)
        {
            return new OpenGLShaderProgram(gl, 
                vertexShaderFile.ReadAllText(), 
                fragmentShaderFile.ReadAllText());
        }
    }
}
