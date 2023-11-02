using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ara3D.Buffers;
using Ara3D.Graphics;
using Ara3D.Utils;
using Ara3D.Utils.Roslyn;

namespace Ara3D.ScriptPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ScriptPaintMainWindow : Window
    {
        public IProgressiveRenderer Renderer { get; }
        public Bitmap Bitmap { get; }
        public WriteableBitmap Writeable { get; }

        public ScriptPaintMainWindow()
        {
            InitializeComponent();

            AssemblyUtil.LoadAllAssembliesInCurrentDomainBaseDirectory();

            var sourceFile = PathUtil.GetCallerSourceFolder().RelativeFile("..", "PathTracer", "DemoPathTracer.cs");
            var compilation = sourceFile.CompileCSharpStandard();

            var logger = new LoggingWindowService();
            logger.Logger.Log($"Compilation success = {compilation.EmitResult.Success}");
            foreach (var d in compilation.EmitResult.Diagnostics)
                logger.Logger.Log($"{d}");

            if (!compilation.EmitResult.Success)
                return;

            var assembly = Assembly.LoadFrom(compilation.OutputFile);
            var type = assembly.GetType("PathTracer.DemoPathTracer");

            var obj = Activator.CreateInstance(type);
            Renderer = obj as IProgressiveRenderer;
            Bitmap = new Bitmap(Renderer.Width, Renderer.Height);
            Writeable = new WriteableBitmap(Bitmap.Width, Bitmap.Height, 96, 96, PixelFormats.Bgr32, null);
            MyImage.Source = Writeable;

            Recompute().FireAndForget();
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
