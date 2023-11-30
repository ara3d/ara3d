using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Ara3D.Bowerbird.Core;
using Ara3D.Domo;
using Ara3D.Services;
using Autodesk.Revit.UI;

namespace Ara3D.Bowerbird.Revit
{
    public class BowerbirdApp : IExternalApplication
    {
        public UIControlledApplication UicApp { get; private set; }
        public static BowerbirdApp Instance { get; private set; }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            UicApp = application;
            Instance = this;
            var rvtRibbonPanel = application.CreateRibbonPanel("Ara 3D");
            var pushButtonData = new PushButtonData("Bowerbird", "Bowerbird", 
                Assembly.GetExecutingAssembly().Location,
                typeof(BowerbirdExternalCommand).FullName);
            // https://www.revitapidocs.com/2020/544c0af7-6124-4f64-a25d-46e81ac5300f.htm
            if (!(rvtRibbonPanel.AddItem(pushButtonData) is PushButton runButton))
                return Result.Failed;
            runButton.LargeImage = BitmapToImageSource(Resources.Bowerbird_32x32);
            runButton.ToolTip = "Compile and Load C# Scripts";
            return Result.Succeeded;
        }

        public IApi Api { get; }
        public LogRepo LogRepo { get; }
        public LoggingService Logger { get; }
        public BowerbirdService Service { get; }
        public BowerbirdOptions Options { get; }

        public BowerbirdApp()
        {
            Api = new Api();
            LogRepo = new LogRepo();
            Logger = new LoggingService("Bowerbird", Api, LogRepo);
            LogRepo.OnModelAdded(model => OnLogEntry(model.Value));
            Options = BowerbirdOptions.CreateFromName("Ara 3D", "Bowerbird for Revit");
            Service = new BowerbirdService(Api, Logger, Options);
        }

        public void OnLogEntry(LogEntry entry)
        {
            Debug.WriteLine($"Log entry: {entry.Text}");
        }

        public void Run(UIApplication application)
        {
            Logger.Log("Running command");
            var window = new BowerbirdCompilationWindow();
            Service.Compile();
        }
    }
}
