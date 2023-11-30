using Ara3D.Bowerbird.Core;
using Ara3D.Services;

namespace Ara3D.Bowerbird.Wpf.Demo;

public class BowerBirdDemoApp 
{
    public IApi Api { get; } = new Api();
    public BowerbirdService Service { get; }
    public LoggingService Logger { get; }
    public LogRepo LogRepo { get; } 

    public readonly BowerbirdOptions Options = BowerbirdOptions
        .CreateFromName("Ara 3D", "Bowerbird WPF Demo");

    public BowerBirdDemoApp()
    {
        LogRepo = new LogRepo();
        Logger = new LoggingService("Compilation", Api, LogRepo);
        Service = new BowerbirdService(Api, Logger, Options);
    }

}