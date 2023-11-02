using System;
using Ara3D.Domo;
using Ara3D.Services;
using LogEntry = Ara3D.Services.LogEntry;

namespace Ara3D.ScriptPaint;

public class LoggingWindowService
{
    public LoggingWindow Window;
    public LoggingService Logger;
    public IApi Api = new Api();
    public LogRepo Repo = new LogRepo();
    
    // TODO: really the host should 
    public LoggingWindowService()
    {
        Logger = new LoggingService("Compilation", Api, Repo);
        Window = new LoggingWindow();
        Window.Show();
        Repo.OnModelAdded(OnLogEntry);
    }

    public void OnLogEntry(IModel<LogEntry> model)
    {
        Window.LoggingEditBox.AppendText(model.Value.Text + Environment.NewLine);
    }
}