using System.Reflection;
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
                "Ara3D.Bowerbird.Revit.BowerbirdExternalCommand");
            var runButton = rvtRibbonPanel.AddItem(pushButtonData) as PushButton;
            if (runButton == null)
                return Result.Failed;
            runButton.ToolTip = "Create and run dynamic commands";
            return Result.Succeeded;
        }

    }
}
