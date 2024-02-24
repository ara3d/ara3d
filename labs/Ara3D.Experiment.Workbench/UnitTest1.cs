using GH_IO.Serialization;

namespace Ara3D.Experiment.Workbench
{

    public class Tests
    {
        [Test, Explicit]
        public static void ConvertGhToGhx()
        {
            var a = new GH_Archive();
            a.ReadFromFile(@"C:\Users\cdigg\dev\grasshopper\first.gh");
            a.WriteToFile(@"C:\Users\cdigg\dev\grasshopper\first-output.ghx", false, false);
        }
    }
}