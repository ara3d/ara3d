using System.Diagnostics;
using Ara3D.Utils;
using Ara3D.Graphics;
using Ara3D.Buffers;
using Ara3D.Mathematics;
using Vim.Math3d;
using ColorRGBA = Ara3D.Mathematics.ColorRGBA;
using Vector3 = Ara3D.Mathematics.Vector3;

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
        public static void Save(this Bitmap bitmap, FilePath path)
        {
            var writer = path.OpenWrite();
            var dibHeader = new DibHeader(bitmap.Width, bitmap.Height);
            var bmpHeader = new BitmapHeader(dibHeader.biSizeImage);
            writer.WriteValue(bmpHeader);
            Debug.Assert(writer.Position == 14);
            writer.WriteValue(dibHeader);
            Debug.Assert(writer.Position == 54);
            writer.Write(bitmap.PixelBuffer);
            var n = bitmap.PixelBuffer.GetNumBytes();
            Debug.Assert(n == bitmap.Width * bitmap.Height * 4);
            Debug.Assert(dibHeader.biSizeImage == n);
            Debug.Assert(bmpHeader.Size == 54 + n);
            writer.Close();
        } 
         public static Bitmap EvaluateInParallel(this IPathTracer source)
         {
            var target = new Bitmap(source.Width, source.Height);
            Parallel.For(0, source.Width * source.Height, i =>
            {
                var x = i % source.Width;
                var y = i / source.Width;
                var v = source.Eval(x, y);
                var rgb = ColorRGBA.FromVector(new Vector3(v.Item1, v.Item2, v.Item3));
                target.SetPixel(x, (source.Height - y - 1), rgb);
            });
            return target;
         }
         
         public static Bitmap EvaluateInSerial(this IPathTracer source)
         {
             var target = new Bitmap(source.Width, source.Height);
             for (var i = 0; i < source.Width * source.Height; i++)
             {
                 var x = i % source.Width;
                 var y = i / source.Width;
                 var v = source.Eval(x, y);
                 var rgb = ColorRGBA.FromVector(new Vector3(v.Item1, v.Item2, v.Item3));
                 target.SetPixel(x, (source.Height - y - 1), rgb);
             }
             return target;
         }


        public static void CreateBitmap(IPathTracer pathTracer, bool show = false, int iterations = 3)
        {
            var t = pathTracer.GetType().Name;
            const int divisor = 2;
            pathTracer.Width = 960 / divisor;
            pathTracer.Height = 540 / divisor;
            pathTracer.BounceCount = 4;
            pathTracer.SamplesCount = 1 << iterations;

            var sw = Stopwatch.StartNew();
            
            //var bmp = pathTracer.EvaluateInParallel();
            var bmp = pathTracer.EvaluateInSerial();

            sw.OutputTimeElapsed($"Evaluated {t} with {pathTracer.SamplesCount}");

            if (show)
            {
                var file = new FilePath(Path.GetTempFileName());
                file = file.ChangeExtension("bmp");
                bmp.Save(file);
                file.ShellExecute();
            }
        }

        public static void Main(string[] args)
        {

            /*
            for (var i = 3; i < 7; i++)
            {
                //CreateBitmap(new PathTracerVim());
                CreateBitmap(new PathTracerSystem(), true, i);
                //CreateBitmap(new PathTracerPlato()); 
                CreateBitmap(new PathTracerPlato_v2(), true, i);
            }*/
            //CreateBitmap(new PathTracerPlato_v2(), true, 3);
            //CreateBitmap(new PathTracerPlatoSIMD(), true, 3);
            CreateBitmap(new WavefrontPathTracer(), true, 3);

            //CreateBitmap(new PathTracerPlato(), true);
            //CreateBitmap(new PathTracerPlato_v2(), true);
            //CreateBitmap(new PathTracerSystem(), true);

            /*
            CreateBitmap(new PathTracerVim());
            CreateBitmap(new PathTracerSystem());
            CreateBitmap(new PathTracerPlato()); ;
            CreateBitmap(new PathTracerPlato_v2());
            */
        }
    }
}
