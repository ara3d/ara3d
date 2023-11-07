using System;
using System.Reflection;
using Ara3D.Utils;
using Ara3D.Utils.Roslyn;
using Rhino.Geometry;

namespace Ezrx.Rhino
{
    public static class Evaluator
    {
        public static Mesh Eval(string code)
        {
            AssemblyUtil.LoadAllAssembliesInCurrentDomainBaseDirectory();
            var sourceFile = PathUtil.CreateTempFileWithContents(code);
            return EvalFile(sourceFile);
        }

        public static Mesh EvalFile(string fileName)
        {
            var fp = new FilePath(fileName);
            var compilation = fp
                .CompileCSharpStandard();

            var logger = new StdLogger();
            logger.Log($"Compilation success = {compilation.EmitResult.Success}");
            foreach (var d in compilation.EmitResult.Diagnostics)
                logger.Log($"{d}");

            if (!compilation.EmitResult.Success)
                return null;

            var assembly = Assembly.LoadFrom(compilation.OutputFile);
            var type = assembly.GetType("Test.TestClass");

            dynamic obj = Activator.CreateInstance(type);
            return obj.CreateMesh();
        }
        
        public static Mesh MakeCubeViaEval()
        {
            var code = $@"        
using Rhino.Geometry; 
namespace Test {{
    public class TestClass {{
        public Mesh CreateMesh() {{
            var mesh = new Mesh();
            mesh.Vertices.Add(new Point3d(0.5, 0.5, 0.5));
            mesh.Vertices.Add(new Point3d(0.5, 0.5, -0.5));
            mesh.Vertices.Add(new Point3d(0.5, -0.5, 0.5));
            mesh.Vertices.Add(new Point3d(0.5, -0.5, -0.5));
            mesh.Vertices.Add(new Point3d(-0.5, 0.5, 0.5));
            mesh.Vertices.Add(new Point3d(-0.5, 0.5, -0.5));
            mesh.Vertices.Add(new Point3d(-0.5, -0.5, 0.5));
            mesh.Vertices.Add(new Point3d(-0.5, -0.5, -0.5));

            mesh.Faces.AddFace(0, 1, 5, 4);
            mesh.Faces.AddFace(0, 4, 6, 2);
            mesh.Faces.AddFace(0, 2, 3, 1);
            mesh.Faces.AddFace(7, 3, 2, 6);
            mesh.Faces.AddFace(7, 6, 4, 5);
            mesh.Faces.AddFace(7, 5, 1, 3);

            mesh.FaceNormals.ComputeFaceNormals();
            mesh.Normals.ComputeNormals();
            mesh.Compact();
            return mesh;
        }}
    }}
}}";

            return Eval(code);
        }

        public static Mesh MakeCube()
        {
            var mesh = new Mesh();
            mesh.Vertices.Add(new Point3d(0.5, 0.5, 0.5));
            mesh.Vertices.Add(new Point3d(0.5, 0.5, -0.5));
            mesh.Vertices.Add(new Point3d(0.5, -0.5, 0.5));
            mesh.Vertices.Add(new Point3d(0.5, -0.5, -0.5));
            mesh.Vertices.Add(new Point3d(-0.5, 0.5, 0.5));
            mesh.Vertices.Add(new Point3d(-0.5, 0.5, -0.5));
            mesh.Vertices.Add(new Point3d(-0.5, -0.5, 0.5));
            mesh.Vertices.Add(new Point3d(-0.5, -0.5, -0.5));

            mesh.Faces.AddFace(0, 1, 5, 4);
            mesh.Faces.AddFace(0, 4, 6, 2);
            mesh.Faces.AddFace(0, 2, 3, 1);
            mesh.Faces.AddFace(7, 3, 2, 6);
            mesh.Faces.AddFace(7, 6, 4, 5);
            mesh.Faces.AddFace(7, 5, 1, 3);

            mesh.FaceNormals.ComputeFaceNormals();
            mesh.Normals.ComputeNormals();
            mesh.Compact();
            return mesh;
        }
    }
}