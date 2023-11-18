using System.Diagnostics;
using Ara3D.Utils;
using Ara3D.Utils.Roslyn;

namespace Ara3D.ScriptPaint;

public class CompilationService
{
    public void Compile()
    {
        var sourceFile = PathUtil.GetCallerSourceFolder().RelativeFile("..", "PathTracer", "DemoPathTracer.cs");
        var compilation = sourceFile.CompileCSharpStandard();
        //var options = new CompilerOptions();
        //var service = new CompilerService(logger, options, )
        Debug.WriteLine(compilation.EmitResult.Success);
    }
}