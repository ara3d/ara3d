using System.Diagnostics;
using System.Reflection;
using Ara3D.Bowerbird.Core;
using Ara3D.Domo;
using Ara3D.Services;
using Autodesk.Revit.Creation;
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

        public Result OnStartup(UIControlledApplication application)
        {
            UicApp = application;
            Instance = this;
            var rvtRibbonPanel = application.CreateRibbonPanel("Ara 3D");
            var pushButtonData = new PushButtonData("Bowerbird", "Bowerbird", 
                Assembly.GetExecutingAssembly().Location,
                typeof(BowerbirdExternalCommand).FullName);
            if (!(rvtRibbonPanel.AddItem(pushButtonData) is PushButton runButton))
                return Result.Failed;
            runButton.ToolTip = "Create and run dynamic commands";
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
            Options = BowerbirdOptions.CreateFromName("Bowerbird for Revit");
            Service = new BowerbirdService(Api, Logger, Options);
        }

        public void OnLogEntry(LogEntry entry)
        {
            Debug.WriteLine($"Log entry: {entry.Text}");
        }

        public void Run(UIApplication application)
        {
            Logger.Log("Running command");
            Service.Recompile();
        }
    }
}
