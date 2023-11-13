using System;
using Ara3D.Bowerbird.Core;
using Ara3D.Domo;
using Ara3D.Services;
using Ara3D.Utils;
using LogEntry = Ara3D.Services.LogEntry;

namespace Ara3D.Bowerbird.Wpf.Demo;

public class BowerBirdDemoApp
{
    public IApi Api { get; } = new Api();
    public BowerbirdService Service { get; }
    public LoggingService Logger { get; }
    public LogRepo LogRepo { get; } 
    public BowerbirdCompilationWindow Window { get; }

    public BowerbirdOptions Options = BowerbirdOptions
        .CreateFromName("Bowerbird WPF Demo");

    public BowerBirdDemoApp()
    {
        LogRepo = new LogRepo();
        Logger = new LoggingService("Compilation", Api, LogRepo);
        Window = new BowerbirdCompilationWindow();
        Window.Show();
        LogRepo.OnModelAdded(model => OnLogEntry(model.Value));
        Service = new BowerbirdService(Api, Logger, Options);
        Service.Recompile();
    }

    public void OnLogEntry(LogEntry logEntry)
    {
        Window.TextBoxLog.Dispatcher.Invoke(() =>
            Window.TextBoxLog.AppendText($"{logEntry.Created.ToLoggingTimeStamp()} {logEntry.Text} {Environment.NewLine}"));
    }
}