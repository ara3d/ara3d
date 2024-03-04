using System.Diagnostics;
using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Serialization.G3D;
using Rhino.FileIO;
using Rhino.Geometry;

namespace Ara3D.RhinoReader
{
    // https://github.com/rgl-epfl/rhino-exporter/blob/master/MeshStore.cs
    // https://github.com/meliharvey/threejs-exporter/blob/master/threejs-exporter/Mesh.cs
    // https://discourse.mcneel.com/t/get-geometries-transformation-matrix/116651
    // https://snyk.io/advisor/python/rhino3dm/example
    // https://github.com/mrdoob/three.js/blob/master/examples/jsm/loaders/3DMLoader.js
    // Maybe I need to get the boudning box? 

    public static class RhinoConverterExtensions
    {
        public static Vector3 ToMath3D(this Point3f v)
            => new Vector3(v.X, v.Y, v.Z);

        public static G3D ToG3D(this Mesh mesh)
        {
            if (mesh.Faces.Count == 0)
                return G3D.Empty;

            mesh.Faces.ConvertQuadsToTriangles();
            mesh.FaceNormals.ComputeFaceNormals();
            mesh.Normals.ComputeNormals();
            mesh.Compact();

            var bldr = new G3DBuilder();
            bldr.SetObjectFaceSize(3);

            var indices = new List<int>();
            foreach (var f in mesh.Faces)
            {
                if (!f.IsValid())
                    throw new Exception("Internal error: invalid face");

                if (f.IsTriangle)
                {
                    indices.Add(f.A);
                    indices.Add(f.B);
                    indices.Add(f.C);
                }
                else if (f.IsQuad)
                {
                    indices.Add(f.A);
                    indices.Add(f.B);
                    indices.Add(f.C);

                    indices.Add(f.C);
                    indices.Add(f.D);
                    indices.Add(f.A);
                }
                else
                {
                    throw new Exception("Not supported");
                }
            }

            var v = mesh.Vertices[0];
            bldr.AddVertices(mesh.Vertices.ToIArray().Select(ToMath3D));
            bldr.AddIndices(indices.ToIArray());

            /*
            if (mesh.HasTangentBasis)
                bldr.Add(mesh.BiTangents.ToIArray().Select(ToMath3D).ToVertexBitangentAttribute());

            if (mesh.HasTangentBasis)
                bldr.Add(mesh.Tangents.ToIArray().Select(x => ToMath3D(x).ToVector4()).ToVertexTangentAttribute());

            if (mesh.HasNormals)
                bldr.Add(mesh.Normals.ToIArray().Select(ToMath3D).ToVertexNormalAttribute());

            for (var i = 0; i < mesh.TextureCoordinateChannelCount; ++i)
            {
                var uvChannel = mesh.TextureCoordinateChannels[i];
                bldr.Add(uvChannel.ToIArray().Select(ToMath3D).ToVertexUvwAttribute(i));
            }

            for (var i = 0; i < mesh.VertexColorChannelCount; ++i)
            {
                var vcChannel = mesh.VertexColorChannels[i];
                bldr.Add(vcChannel.ToIArray().Select(ToMath3D).ToVertexColorAttribute(i));
            }
            */

            return bldr.ToG3D();
        }
    }

    public static class RhinoHelpers
    {
        // https://discourse.mcneel.com/t/read-transform-matrix-of-block-instance-in-file3dm/77017/16
        // https://github.com/mcneel/rhino-developer-samples/blob/6/rhinocommon/cs/SampleCsCommands/SampleCsWriteStl.cs
        // https://discourse.mcneel.com/t/get-mesh-for-any-geometry-object-in-3dm-file/133006/6
        public static Mesh ToMesh(this GeometryBase geometry)
        {
            Mesh r = null;

            if (geometry is Mesh mesh)
            {
                r = mesh;
            }
            else if (geometry is Extrusion extrusion)
            {
                r = extrusion.GetMesh(MeshType.Any);
            }
            else if (geometry is Brep brep)
            {
                r = new Mesh();
                foreach (var brepFace in brep.Faces)
                {
                    var brepFaceMesh = brepFace.GetMesh(MeshType.Any);
                    if (null != brepFaceMesh)
                        r.Append(brepFaceMesh);
                }
            }
            else if (geometry is InstanceReferenceGeometry instance)
            {
                //var definition = table.FindId(instance.ParentIdefId);
                //var name = instance;
                //rc = definition.GetObjectIds();
                Debugger.Break();
            }
            else if (geometry is LineCurve lineCurve)
            {
                Debug.WriteLine("Line Curve is temporarily ignored");
            }
            else
            {
                throw new Exception("Unrecognized geometry type");
            }

            if (r == null || !r.IsValid)
                throw new Exception("Could not build a valid mesh");

            return r;
        }
    }

    public static class GeometryTools
    {
        public static G3D MergeMeshes(IEnumerable<G3D> meshes)
        {
            var vertices = new List<Vector3>();
            var offset = 0;
            var indices = new List<int>();
            foreach (var m in meshes)
            {
                vertices.AddRange(m.Vertices.ToEnumerable());
                foreach (var i in m.Indices.ToEnumerable())
                    indices.Add(i + offset);
                offset += m.Vertices.Count;
            }

            var bldr = new G3DBuilder();
            bldr.AddIndices(indices.ToIArray());
            bldr.AddVertices(vertices.ToIArray());
            return bldr.ToG3D();
        }
    }


    public class Program
    {
        public static string WorkingDir => Environment.CurrentDirectory;
        public static string ProjectDir => Path.Combine(WorkingDir, "..", "..", "..");
        public static string RepoDir => Path.Combine(ProjectDir, "..", "..", "..");        
        public static string DataDir => Path.Combine(RepoDir, "revit-test-datasets", "rhino");

        static void Main(string[] args)
        {
            Debug.Assert(Directory.Exists(WorkingDir));
            Debug.Assert(Directory.Exists(ProjectDir));
            Debug.Assert(Directory.Exists(DataDir));
            var file3dm = File3dm.Read(Path.Combine(DataDir, "Facade.3dm"));
            //var tmp = JsonSerializer.Serialize(file3dm, new JsonSerializerOptions() { WriteIndented = true });
            //File.WriteAllText(Path.Combine(ProjectDir, "test.json"), tmp);
            Console.WriteLine("Number of objects in file {0}", file3dm.Objects.Count);

            var outputFolder = Path.Combine(ProjectDir, "output");
            Directory.CreateDirectory(outputFolder);

            var meshes = new List<G3D>();

            Console.WriteLine("Number of objects in file {0}", file3dm.Objects.Count);

            Console.WriteLine($"# layers = {file3dm.AllLayers.Count}");
            foreach (var layer in file3dm.AllLayers)
            { }

            Console.WriteLine($"# groups = {file3dm.AllGroups.Count}");
            foreach (var group in file3dm.AllGroups)
            {

            }

            Console.WriteLine($"# instance definitions = {file3dm.AllInstanceDefinitions.Count}");
            foreach (var instnceDefn in file3dm.AllInstanceDefinitions)
            { }

            Console.WriteLine($"# materials = {file3dm.AllMaterials.Count}");
            foreach (var material in file3dm.AllMaterials)
            { }

            Console.WriteLine($"# views = {file3dm.AllViews.Count}");
            foreach (var material in file3dm.AllViews)
            { } 
            
            Console.WriteLine("Number of objects in file {0}", file3dm.Objects.Count);
            foreach (var obj in file3dm.Objects)
            {
                var mesh = obj.Geometry.ToMesh();

                var box = obj.Geometry.GetBoundingBox(true);
                var pos = box.Center;
                var extent = box.Max - box.Min;
                Console.WriteLine($"{pos} {extent}");

                Console.WriteLine(obj.ReferenceModelSerialNumber);
                Console.WriteLine(obj.ComponentType);
                Console.WriteLine(obj.Attributes.ObjectId);
                Console.WriteLine(obj.Attributes.LayerIndex);
                Console.WriteLine(obj.Attributes.Name);
                Console.WriteLine(obj.Attributes.GroupCount);
                
                if (mesh == null)
                    continue;
                var g3d = mesh.ToG3D();
                meshes.Add(g3d);
            }

            var merged = GeometryTools.MergeMeshes(meshes);
            var outputFile = Path.Combine(outputFolder, $"Merged.obj");
            merged.WriteObj(outputFile);
        }

    }
}