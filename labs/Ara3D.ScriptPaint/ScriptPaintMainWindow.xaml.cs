using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ara3D.Buffers;
using Ara3D.Graphics;
using Ara3D.Utils;
using PathTracer;

namespace Ara3D.ScriptPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ScriptPaintMainWindow : Window
    {
        public DemoPathTracer Renderer { get; }
        public Bitmap Bitmap { get; }
        public WriteableBitmap Writeable { get; }

        public ScriptPaintMainWindow()
        {
            InitializeComponent();
            Renderer = new DemoPathTracer(0);
            Bitmap = new Bitmap(Renderer.Width, Renderer.Height);
            Writeable = new WriteableBitmap(Bitmap.Width, Bitmap.Height, 96, 96, PixelFormats.Bgr32, null);
            MyImage.Source = Writeable;
            Recompute().FireAndForget();
            var sourceFile = PathUtil.GetCallerSourceFolder().RelativeFile("..", "PathTracer", "DemoPathTracer.cs");
            var compiler = new CompilerService();
        }

        public Task Recompute()
        {
            if (MyImage == null)
                return Task.CompletedTask;

            return Task.Run(() =>
            {
                for (var i = 0; i < Renderer.MaxIterations; i++)
                {
                    var stage = Renderer.GetIteration(i);
                    stage.EvaluateInParallel(Bitmap);
                    var r = new Int32Rect(0, 0, Bitmap.Width, Bitmap.Height);
                    var stride = Bitmap.Width * 4;
                    var numBytes = (int)Bitmap.PixelBuffer.GetNumBytes();

                    Dispatcher.Invoke(() =>
                    {
                        // Write the pixels on the main thread
                        Bitmap.PixelBuffer.WithPointer(ptr =>
                            Writeable.WritePixels(r, ptr, numBytes, stride));

                    });
                }
            });
        }
    }
}
