using Ara3D.Serialization.VIM;
using Plato.Geometry.VIM;

namespace BIMDemos
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var vim = VimSerializer.Deserialize(@"C:\Users\cdigg\OneDrive\Documents\VIM\Snowdon Towers Sample Electrical.vim");
            var doc = new VimDocument(vim);

            var ga = new VimGeometryAdapter(vim);
            Console.WriteLine($"# Points {ga.Points.Count}");
            Console.WriteLine($"# Indices {ga.Indices.Count}");
            Console.WriteLine($"# Transforms {ga.Transforms.Count}");
            Console.WriteLine($"# Materials {ga.Materials.Count}");
            Console.WriteLine($"# Meshes {ga.SubMeshes.Count}");
            Console.WriteLine($"# Sub-meshes {ga.SubMeshes.Sum(x => x.Count)}");


            for (var i=0; i < ga.SubMeshes.Count; i++)
            {
                Console.WriteLine($"Mesh {i}");
                foreach (var sm in ga.SubMeshes[i])
                    Console.WriteLine($"  Mesh has {sm.Mesh.Faces.Count} faces");
            }

            //foreach (var cat in doc.CategoryNames)
            //    Console.WriteLine(cat);

            foreach (var e in doc.Elements)
            {
                Console.WriteLine($"Element {e.Key} {e.Value.Type} {e.Value.Name}");
                foreach (var p in e.Value.Parameters)
                    Console.WriteLine($"  {p.Name} = {p.Value}");
            }

        }
    }
}
