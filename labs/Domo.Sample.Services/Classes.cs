using System.Drawing;
using System.Windows.Input;
using Ara3D.Domo.SampleModels;
using Ara3D.Services;
using Ara3D.Utils;

namespace Ara3D.Domo.Sample.Services
{
    public class CommandService
    { }

    public class ApplicationEventService : AggregateModelBackedService<ApplicationEvent>
    {
        public ApplicationEventService(IApi api, ILogService logService) : base(api)
        {
            Repository.OnModelChanged(status => logService.Log("Application Event", status.Value.EventName));
        }

        public void ApplicationStart()
            => Repository.Add(new ApplicationEvent("Application Started", DateTimeOffset.Now));

        public void ApplicationEnd()
            => Repository.Add(new ApplicationEvent("Application Closed", DateTimeOffset.Now));
    }

    public class FileService
    {
    }

    public class ErrorService
    {
    }

    public interface ILogService : IAggregateModelBackedService<LogItem>
    {
        void Log(string category, string message);
    }

    public class LogService : AggregateModelBackedService<LogItem>, ILogService
    {
        public LogService(IApi api)
            : base(api)
        { }

        public void Log(string category, string message)
            => Repository.Add(new LogItem(category, message, "", DateTimeOffset.Now));
    }

    public interface IStatusService : ISingletonModelBackedService<Status>
    {
    }

    public class StatusService : SingletonModelBackedService<Status>, IStatusService
    {
        public StatusService(IApi api, ILogService logService)
            : base(api)
        {
            Repository.OnModelChanged(status => logService.Log("Status", status.Value.Message));
        }

        public string Status
        {
            get => Model.Value.Message;
            set => Model.Value = Model.Value with { Message = value };
        }
    }

    public interface IUserService
    {
        INamedCommand LoginCommand { get; }
        INamedCommand LogoutCommand { get; }
    }

    public class UserService : SingletonModelBackedService<User>, IUserService 
    {
        public UserService(IApi api)
            : base(api)
        {
            RegisterCommand(LogIn, () => CanLogin, Repository);
            RegisterCommand(LogOut, () => CanLogout, Repository);
        }

        public void LogIn(string name)
        {
            if (LoggedIn)
                throw new Exception($"Already logged in as {Model.Value.Name}!");
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("name cannot be null or empty");
            Model.Value = Model.Value with { Name = name, LogInTime = DateTimeOffset.Now };
        }

        public bool CanLogin 
            => !LoggedIn;

        public bool CanLogout
            => LoggedIn;

        public void LogOut()
            => Model.Value = Model.Value with { Name = "" };

        public bool LoggedIn
            => !string.IsNullOrWhiteSpace(Model.Value.Name);

        public TimeSpan TimeLoggedIn
            => LoggedIn ? DateTimeOffset.Now - Model.Value.LogInTime : TimeSpan.Zero;

        public INamedCommand LoginCommand
            => GetCommand(nameof(LogIn));

        public INamedCommand LogoutCommand
            => GetCommand(nameof(LogIn));
    }

    public class CommandLineService
    {

    }

    public class MouseService
    {

    }

    public class DrawingService
    {
    }

    public interface IUndoService : ISingletonModelBackedService<UndoState>
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        void Undo();
        void Redo();

        INamedCommand UndoCommand { get; }
        INamedCommand RedoCommand { get; }
    }

    public class UndoService : SingletonModelBackedService<UndoState>, 
        IUndoService, 
        ISubscriber<RepositoryChangedEvent>
    {
        public UndoService(IApi api)
            : base(api)
        {
            api.EventBus.Subscribe<RepositoryChangedEvent>(this);
            RegisterCommand(Undo, () => CanUndo, Repository);
            RegisterCommand(Redo, () => CanRedo, Repository);
        }

        public void OnEvent(RepositoryChangedEvent e)
        {
            // TODO: store the appropriate change. 
            if (CanRedo)
            {
                // Clear all the "undo items" in the Redo stack 
            }
        }

        public bool CanUndo
            => Value.CurrentIndex >= 0;

        public bool CanRedo
            => Value.CurrentIndex < Value.UndoItems.Count - 1;

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public INamedCommand UndoCommand => GetCommand(nameof(Undo));
        public INamedCommand RedoCommand => GetCommand(nameof(Redo));
    }

    public interface IChangeService : IAggregateModelBackedService<ChangeRecord>
    {
    }

    public class ChangeService : 
        AggregateModelBackedService<ChangeRecord>, 
        IChangeService, 
        ISubscriber<RepositoryChangedEvent>
    {
        public ChangeService(IApi api)
            : base(api)
        {
            api.EventBus.Subscribe<RepositoryChangedEvent>(this);
        }

        public void OnEvent(RepositoryChangedEvent e)
        {
            var args = e.Args;
            switch (args.ChangeType)
            {
                case RepositoryChangeType.RepositoryAdded:
                    break;
                case RepositoryChangeType.RepositoryDeleted:
                    break;
                case RepositoryChangeType.ModelAdded:
                    Repository.Add(new ChangeRecord(args.NewValue?.ToString() ?? "", args.ModelId, ChangeType.Added, DateTimeOffset.Now));
                    break;
                case RepositoryChangeType.ModelRemoved:
                    Repository.Add(new ChangeRecord("", args.ModelId, ChangeType.Removed, DateTimeOffset.Now));
                    break;
                case RepositoryChangeType.ModelUpdated:
                    Repository.Add(new ChangeRecord(args.NewValue?.ToString() ?? "", args.ModelId, ChangeType.Changed, DateTimeOffset.Now));
                    break;
            };
        }
    }

    public class KeyboardService
    { }

    public class ProfilingService
    { }

    public class DragAndDropService
    { }

    public class DownloadFile
    { }

    public class BackupService
    { }

    public class RecentFiles
    { }

    public readonly record struct Dialog
    (
        string Title,
        string Query
    );

    public class QueryUserService
    {
        public bool YesNo(string title, string message, string helpUrl) => true;
        public bool OkCanel(string title, string message, string helpUrl) => true;
        public bool AcceptCanel(string title, string message, string helpUrl) => true;
        public bool Ok(string title) => true;
        public bool Cancel(string title) => true;
        public DateTimeOffset ChooseTime() => throw new NotImplementedException();
        public float ChooseValue(float min, float max) => throw new NotImplementedException();
        public float ChooseDate() => throw new NotImplementedException();
        public string ChooseString() => throw new NotImplementedException();
        public string ChooseFile() => throw new NotImplementedException();
        public string ChooseFolder() => throw new NotImplementedException();
        public int ChooseValue(int min, int max) => throw new NotImplementedException();
        public Color ChooseColor() => throw new NotImplementedException();
        public string ChooseValue(string[] values) => throw new NotImplementedException();
    }

    public class MacroRecorder
    {
        public void StartRecording() { }
        public void StopRecording() {} 
        public void CancelRecording() { }
    }

    public class MetricsService
    {

    }

    public class ScriptingService
    {

    }

    public class LoggingService
    {
    }

    public class LogFilePolicy
    {

    }

    public class AlertPolicy
    {

    }

    public class CommonCommands
    {
        public void OpenFile() { }
        public void CloseFile() { }
        public void SaveFile(string fileName) { }
        public void ShowLogsLocation() { }
        public void ShowExeLocation() { }
        public void ShowTempLocation() { }
        public void ShowCommandLine() { }
        public ICommand[] FindCommands(string name) => throw new NotImplementedException();
        public void TriggerCommand(string name, object parameter) { }
        public void RetrieveVideoControllerData() { }
        public void EditCommands() { }
        public void CreateLogMessage(string msg) { }
        public void UpdateStatus(string msg) { }
    }
}
