using System.Text.Json;
using Ara3D.Utils;
using Identification;

namespace IdentificationApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //CompareBunnies();
            CompareIdentityModels();
        }

        public static void OutputIdentityModelInfo(IdentityModel im)
        {
            Console.WriteLine($"Model has {im.Entities.Count} entities");
            var i = 0;
            foreach (var e in im.Entities)
            {
                Console.WriteLine(
                    $" entity {i++} with id {e.Id} has {e.Geometries.Count} geometric components and {e.ParameterSets.Count} parameter sets");
            }
        }

        public static void CompareIdentityModels()
        {
            var im1 = IdentityModel.Load(@"C:\Users\cdigg\dev\Identity\a.json");
            var im2 = IdentityModel.Load(@"C:\Users\cdigg\dev\Identity\b.json");

            Console.WriteLine("Old model");
            OutputIdentityModelInfo(im1);

            Console.WriteLine("New model");
            OutputIdentityModelInfo(im2);

            var addedModels = im1.GetAddedEntities(im2).ToList();
            var removedModels = im2.GetRemovedEntities(im1).ToList();
            var records = im1.GetChangedEntities(im2).ToList();

            Console.WriteLine($"Found {addedModels.Count} new entities, and {removedModels.Count} entities deleted, and {records.Count} entities changed");
            var json = JsonSerializer.Serialize(records, IdentityModel.JsonOptions);
            var output = new FilePath(@"C:\Users\cdigg\dev\Identity\diff.json");
            output.WriteAllText(json);
        }

        public static void CompareBunnies()
        {
            var m1 = ObjMesh.Load(@"C:\Users\cdigg\dev\Identity\stanford-bunny.obj");
            var m2 = ObjMesh.Load(@"C:\Users\cdigg\dev\Identity\stanford-bunny2.obj");
            var m3 = ObjMesh.Load(@"C:\Users\cdigg\dev\Identity\stanford-bunny3.obj");

            var d1 = m1.Distance(m2);
            var d2 = m2.Distance(m3);
            var d3 = m1.Distance(m3);
            var s1 = d1 < DifferenceEngine.GeometryChangeTolerance ? "not significant" : "significant";
            var s2 = d2 < DifferenceEngine.GeometryChangeTolerance ? "not significant" : "significant";
            var s3 = d3 < DifferenceEngine.GeometryChangeTolerance ? "not significant" : "significant";

            Console.WriteLine($"Difference between bunnies 1 and 2 is {d1:0.#####} which is {s1}");
            Console.WriteLine($"Difference between bunnies 1 and 3 is {d2:0.#####} which is {s2}");
            Console.WriteLine($"Difference between bunnies 2 and 3 is {d3:0.#####} which is {s3}");
        }

        public static void CreateIdentityModel()
        {
            {
                var im = IdentityModel.CreateFromFolder(@"C:\Users\cdigg\dev\Identity\A\entities", "Rvt");
                im.Serialize(@"C:\Users\cdigg\dev\Identity\a.json");
            }
            {
                var im = IdentityModel.CreateFromFolder(@"C:\Users\cdigg\dev\Identity\B_added_values\entities_B", "Ifc");
                im.Serialize(@"C:\Users\cdigg\dev\Identity\b.json");
            }
        }

        public static void CreateSampleRecordDifferenceSet()
        {
            var record = new DifferenceRecord()
                {
                    Name = "Hello",
                    DidCenterPointChange = true,
                    OldCenterPoint = (4, 5, 6),
                    NewCenterPoint = (5, 6, 8),
                    CenterPointChange = (1, 1, 2),

                    DidDimensionsChange = false,
                    OldDimensions = (3, 3, 3),
                    NewDimensions = (3, 3, 3),
                    DimensionsChange = (0, 0, 0),

                    OldParameters = new Dictionary<string, string>() { { "a", "123" } },
                    NewParameters = new Dictionary<string, string>()
                    {
                        { "a", "456" },  { "b", "999" }
                    },

                    DidGeometryChange = true,
                    GeometryChangeDelta = 3.4, 
                    GeometryObjFile = "vue.obj",
                };

            var recordSet = new[] { record, record };

            var json = JsonSerializer.Serialize(recordSet, IdentityModel.JsonOptions);
            Console.WriteLine(json);
        }
    }
}
