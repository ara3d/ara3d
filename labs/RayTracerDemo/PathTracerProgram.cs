using System.Diagnostics;
using Ara3D.Utils;
using Ara3D.Graphics;

// https://gist.github.com/mattwarren/d17a0c356bd6fdb9f596bee6b9a5e63c
// https://fabiensanglard.net/postcard_pathtracer/index.html
// https://mattwarren.org/2019/03/01/Is-CSharp-a-low-level-language/

// https://fabiensanglard.net/postcard_pathtracer/index.html
// https://fabiensanglard.net/postcard_pathtracer/formatted_full.html

// https://graphics.pixar.com/library/indexAuthorAndrew_Kensler.html

// https://news.ycombinator.com/item?id=6425965
// http://tog.acm.org/resources/RTNews/html/ (Somewhat dead, but the archives are great)
// http://ompf2.com/ (Active, lots of ray tracing specific discussion)
// http://www.realtimerendering.com/ (I'm fond of the book as well)
// http://kesen.realtimerendering.com/ (Especially the sections for "Symposium on Interactive Ray Tracing" and it's successor, "High Performance Graphics")

namespace PathTracer
{
    public static class PathTracerProgram
    {
        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var img = new DemoPathTracer(5);
            var bmp = img.EvaluateInParallel();
            sw.OutputTimeElapsed("Creating bitmap");
            var file = new FilePath(Path.GetTempFileName());
            file = file.ChangeExtension("bmp");
            bmp.Save(file); 
            file.OpenFile();
        }
    }
}
