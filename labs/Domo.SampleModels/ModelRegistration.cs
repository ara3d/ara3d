using System.Drawing;

namespace Ara3D.Domo.SampleModels
{
    #region Application and Infrastructure Domain

    public readonly struct LogItem
    {
        public LogItem(string Category, string Message, string Data, DateTimeOffset Time)
        {
            this.Category = Category;
            this.Message = Message;
            this.Data = Data;
            this.Time = Time;
        }

        public string Category { get; init; }
        public string Message { get; init; }
        public string Data { get; init; }
        public DateTimeOffset Time { get; init; }

        public void Deconstruct(out string Category, out string Message, out string Data, out DateTimeOffset Time)
        {
            Category = this.Category;
            Message = this.Message;
            Data = this.Data;
            Time = this.Time;
        }
    }

    public readonly struct ApplicationEvent
    {
        public ApplicationEvent(string EventName, DateTimeOffset Time)
        {
            this.EventName = EventName;
            this.Time = Time;
        }

        public string EventName { get; init; }
        public DateTimeOffset Time { get; init; }

        public void Deconstruct(out string EventName, out DateTimeOffset Time)
        {
            EventName = this.EventName;
            Time = this.Time;
        }
    }

    public readonly struct Error
    {
        public Error(string Category, string Message)
        {
            this.Category = Category;
            this.Message = Message;
        }

        public string Category { get; init; }
        public string Message { get; init; }

        public void Deconstruct(out string Category, out string Message)
        {
            Category = this.Category;
            Message = this.Message;
        }
    }

    public readonly struct User
    {
        public User(string Name, DateTimeOffset LogInTime)
        {
            this.Name = Name;
            this.LogInTime = LogInTime;
        }

        public string Name { get; init; }
        public DateTimeOffset LogInTime { get; init; }

        public void Deconstruct(out string Name, out DateTimeOffset LogInTime)
        {
            Name = this.Name;
            LogInTime = this.LogInTime;
        }
    }

    public readonly struct Folders
    {
        public Folders(string ApplicationFolder, string LogFolder, string TempFolder, string DocumentFiles)
        {
            this.ApplicationFolder = ApplicationFolder;
            this.LogFolder = LogFolder;
            this.TempFolder = TempFolder;
            this.DocumentFiles = DocumentFiles;
        }

        public string ApplicationFolder { get; init; }
        public string LogFolder { get; init; }
        public string TempFolder { get; init; }
        public string DocumentFiles { get; init; }

        public void Deconstruct(out string ApplicationFolder, out string LogFolder, out string TempFolder, out string DocumentFiles)
        {
            ApplicationFolder = this.ApplicationFolder;
            LogFolder = this.LogFolder;
            TempFolder = this.TempFolder;
            DocumentFiles = this.DocumentFiles;
        }
    }

    public readonly struct Files
    {
        public Files(string ExecutableFile, string LogFile, string SharedStateFile, string CollaboratorsFile, string PreferencesFiles)
        {
            this.ExecutableFile = ExecutableFile;
            this.LogFile = LogFile;
            this.SharedStateFile = SharedStateFile;
            this.CollaboratorsFile = CollaboratorsFile;
            this.PreferencesFiles = PreferencesFiles;
        }

        public string ExecutableFile { get; init; }
        public string LogFile { get; init; }
        public string SharedStateFile { get; init; }
        public string CollaboratorsFile { get; init; }
        public string PreferencesFiles { get; init; }

        public void Deconstruct(out string ExecutableFile, out string LogFile, out string SharedStateFile, out string CollaboratorsFile, out string PreferencesFiles)
        {
            ExecutableFile = this.ExecutableFile;
            LogFile = this.LogFile;
            SharedStateFile = this.SharedStateFile;
            CollaboratorsFile = this.CollaboratorsFile;
            PreferencesFiles = this.PreferencesFiles;
        }
    }

    public readonly struct UserPreferences
    {
        public UserPreferences(Folders Folders, Files Files, IReadOnlyList<RecentFile> RecentFiles, IReadOnlyList<Macro> Macros)
        {
            this.Folders = Folders;
            this.Files = Files;
            this.RecentFiles = RecentFiles;
            this.Macros = Macros;
        }

        public Folders Folders { get; init; }
        public Files Files { get; init; }
        public IReadOnlyList<RecentFile> RecentFiles { get; init; }
        public IReadOnlyList<Macro> Macros { get; init; }

        public void Deconstruct(out Folders Folders, out Files Files, out IReadOnlyList<RecentFile> RecentFiles, out IReadOnlyList<Macro> Macros)
        {
            Folders = this.Folders;
            Files = this.Files;
            RecentFiles = this.RecentFiles;
            Macros = this.Macros;
        }
    }

    public readonly struct CommandLineArg
    {
        public CommandLineArg(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public string Name { get; init; }
        public string Value { get; init; }

        public void Deconstruct(out string Name, out string Value)
        {
            Name = this.Name;
            Value = this.Value;
        }
    }

    public readonly struct EnvironmentVariable
    {
        public EnvironmentVariable(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public string Name { get; init; }
        public string Value { get; init; }

        public void Deconstruct(out string Name, out string Value)
        {
            Name = this.Name;
            Value = this.Value;
        }
    }

    public readonly struct Command
    {
        public Command(string Name, string Data)
        {
            this.Name = Name;
            this.Data = Data;
        }

        public string Name { get; init; }
        public string Data { get; init; }

        public void Deconstruct(out string Name, out string Data)
        {
            Name = this.Name;
            Data = this.Data;
        }
    }

    public readonly struct Macro
    {
        public Macro(string Name, IReadOnlyList<Command> Commands)
        {
            this.Name = Name;
            this.Commands = Commands;
        }

        public string Name { get; init; }
        public IReadOnlyList<Command> Commands { get; init; }

        public void Deconstruct(out string Name, out IReadOnlyList<Command> Commands)
        {
            Name = this.Name;
            Commands = this.Commands;
        }
    }

    public readonly struct RecentFile
    {
        public RecentFile(string Path, DateTimeOffset DateOpened)
        {
            this.Path = Path;
            this.DateOpened = DateOpened;
        }

        public string Path { get; init; }
        public DateTimeOffset DateOpened { get; init; }

        public void Deconstruct(out string Path, out DateTimeOffset DateOpened)
        {
            Path = this.Path;
            DateOpened = this.DateOpened;
        }
    }

    public enum ChangeType { Added, Removed, Changed }

    public readonly struct ChangeRecord
    {
        public ChangeRecord(string Data, Guid ModelId, ChangeType ChangeType, DateTimeOffset DateChanged)
        {
            this.Data = Data;
            this.ModelId = ModelId;
            this.ChangeType = ChangeType;
            this.DateChanged = DateChanged;
        }

        public string Data { get; init; }
        public Guid ModelId { get; init; }
        public ChangeType ChangeType { get; init; }
        public DateTimeOffset DateChanged { get; init; }

        public void Deconstruct(out string Data, out Guid ModelId, out ChangeType ChangeType, out DateTimeOffset DateChanged)
        {
            Data = this.Data;
            ModelId = this.ModelId;
            ChangeType = this.ChangeType;
            DateChanged = this.DateChanged;
        }
    }

    public readonly struct KeyBindings
    {
        public KeyBindings(string Key1, string Key2, string Command)
        {
            this.Key1 = Key1;
            this.Key2 = Key2;
            this.Command = Command;
        }

        public string Key1 { get; init; }
        public string Key2 { get; init; }
        public string Command { get; init; }

        public void Deconstruct(out string Key1, out string Key2, out string Command)
        {
            Key1 = this.Key1;
            Key2 = this.Key2;
            Command = this.Command;
        }
    }

    public readonly struct Job
    {
        public Job(string Name, bool Completed, bool Canceled, float Progress, bool Determinate)
        {
            this.Name = Name;
            this.Completed = Completed;
            this.Canceled = Canceled;
            this.Progress = Progress;
            this.Determinate = Determinate;
        }

        public string Name { get; init; }
        public bool Completed { get; init; }
        public bool Canceled { get; init; }
        public float Progress { get; init; }
        public bool Determinate { get; init; }

        public void Deconstruct(out string Name, out bool Completed, out bool Canceled, out float Progress, out bool Determinate)
        {
            Name = this.Name;
            Completed = this.Completed;
            Canceled = this.Canceled;
            Progress = this.Progress;
            Determinate = this.Determinate;
        }
    }

    public readonly struct LastInput
    {
        public LastInput(DateTimeOffset Time)
        {
            this.Time = Time;
        }

        public DateTimeOffset Time { get; init; }

        public void Deconstruct(out DateTimeOffset Time)
        {
            Time = this.Time;
        }
    }

    public enum StatusCode { Good, Warning, Critical }

    public readonly struct Status
    {
        public Status(string Message, StatusCode Code)
        {
            this.Message = Message;
            this.Code = Code;
        }

        public string Message { get; init; }
        public StatusCode Code { get; init; }

        public void Deconstruct(out string Message, out StatusCode Code)
        {
            Message = this.Message;
            Code = this.Code;
        }
    }

    public readonly struct CurrentFile
    {
        public CurrentFile(string FilePath)
        {
            this.FilePath = FilePath;
        }

        public string FilePath { get; init; }

        public void Deconstruct(out string FilePath)
        {
            FilePath = this.FilePath;
        }
    }

    public readonly struct UndoItem
    {
        public UndoItem(Guid RepoId, Guid ModelId, string OldValue, string NewValue)
        {
            this.RepoId = RepoId;
            this.ModelId = ModelId;
            this.OldValue = OldValue;
            this.NewValue = NewValue;
        }

        public Guid RepoId { get; init; }
        public Guid ModelId { get; init; }
        public string OldValue { get; init; }
        public string NewValue { get; init; }

        public void Deconstruct(out Guid RepoId, out Guid ModelId, out string OldValue, out string NewValue)
        {
            RepoId = this.RepoId;
            ModelId = this.ModelId;
            OldValue = this.OldValue;
            NewValue = this.NewValue;
        }
    }

    public readonly struct UndoState
    {
        public UndoState(int CurrentIndex, IReadOnlyList<UndoItem> UndoItems)
        {
            this.CurrentIndex = CurrentIndex;
            this.UndoItems = UndoItems;
        }

        public int CurrentIndex { get; init; }
        public IReadOnlyList<UndoItem> UndoItems { get; init; }

        public void Deconstruct(out int CurrentIndex, out IReadOnlyList<UndoItem> UndoItems)
        {
            CurrentIndex = this.CurrentIndex;
            UndoItems = this.UndoItems;
        }
    }

    #endregion

    #region Presentation Domain

    public readonly record struct ActiveRepo(Guid RepoId);

    public enum ViewTypeEnum
    {
        Canvas,
        Text,
        List
    }

    public readonly record struct ViewSettings(ViewTypeEnum ViewType);

    public readonly struct ClickAnimation
    {
        public ClickAnimation(Point Position, DateTimeOffset Time)
        {
            this.Position = Position;
            this.Time = Time;
        }

        public Point Position { get; init; }
        public DateTimeOffset Time { get; init; }

        public void Deconstruct(out Point Position, out DateTimeOffset Time)
        {
            Position = this.Position;
            Time = this.Time;
        }
    }

    public class Select
    {
        public Select(Guid ShapeId)
        {
            this.ShapeId = ShapeId;
        }

        public Guid ShapeId { get; init; }

        public void Deconstruct(out Guid ShapeId)
        {
            ShapeId = this.ShapeId;
        }
    }

    #endregion

    #region Business Domain

    public interface IShape {}

    public interface IDrawingCommand {}

    public interface IInteraction {}

    public readonly struct Line : IDrawingCommand
    {
        public Line(Point Start, Point End)
        {
            this.Start = Start;
            this.End = End;
        }

        public Point Start { get; init; }
        public Point End { get; init; }

        public void Deconstruct(out Point Start, out Point End)
        {
            Start = this.Start;
            End = this.End;
        }
    }

    public readonly struct Ellipse : IShape
    {
        public Ellipse(Point Center, Size Size)
        {
            this.Center = Center;
            this.Size = Size;
        }

        public Point Center { get; init; }
        public Size Size { get; init; }

        public void Deconstruct(out Point Center, out Size Size)
        {
            Center = this.Center;
            Size = this.Size;
        }
    }

    public readonly struct Rectangle : IShape
    {
        public Rectangle(Point Position, Size Size)
        {
            this.Position = Position;
            this.Size = Size;
        }

        public Point Position { get; init; }
        public Size Size { get; init; }

        public void Deconstruct(out Point Position, out Size Size)
        {
            Position = this.Position;
            Size = this.Size;
        }
    }

    public readonly struct SetPen : IDrawingCommand
    {
        public SetPen(Color Color, float Width)
        {
            this.Color = Color;
            this.Width = Width;
        }

        public Color Color { get; init; }
        public float Width { get; init; }

        public void Deconstruct(out Color Color, out float Width)
        {
            Color = this.Color;
            Width = this.Width;
        }
    }

    public readonly struct SetBrush : IDrawingCommand
    {
        public SetBrush(Color Color)
        {
            this.Color = Color;
        }

        public Color Color { get; init; }

        public void Deconstruct(out Color Color)
        {
            Color = this.Color;
        }
    }

    public readonly struct WriteText : IDrawingCommand
    {
        public WriteText(Point Position, string Text)
        {
            this.Position = Position;
            this.Text = Text;
        }

        public Point Position { get; init; }
        public string Text { get; init; }

        public void Deconstruct(out Point Position, out string Text)
        {
            Position = this.Position;
            Text = this.Text;
        }
    }

    public readonly struct Draw : IDrawingCommand
    {
        public Draw(IShape Shape)
        {
            this.Shape = Shape;
        }

        public IShape Shape { get; init; }

        public void Deconstruct(out IShape Shape)
        {
            Shape = this.Shape;
        }
    }

    public readonly struct Clear : IDrawingCommand
    {
        public Clear()
        {
        }
    }

    #endregion

    public static class ModelRegistration
    {

        public static IRepositoryManager RegisterRepos(IRepositoryManager store)
        {
            store.AddAggregateRepository<LogItem>();
            store.AddAggregateRepository<Error>();
            store.AddSingletonRepository<User>();
            store.AddSingletonRepository<UserPreferences>();
            store.AddAggregateRepository<CommandLineArg>();
            store.AddAggregateRepository<EnvironmentVariable>();
            store.AddAggregateRepository<RecentFile>();
            store.AddAggregateRepository<ChangeRecord>();
            store.AddAggregateRepository<Command>();
            store.AddAggregateRepository<Macro>();
            store.AddAggregateRepository<Job>();
            
            store.AddSingletonRepository<ActiveRepo>();
            store.AddSingletonRepository<ViewSettings>();
            store.AddAggregateRepository<ClickAnimation>();

            //store.AddAggregateRepository<IShape>();
            //store.AddAggregateRepository<IDrawingCommand>();
            store.AddSingletonRepository<CurrentFile>();
            store.AddSingletonRepository<LastInput>();
            store.AddAggregateRepository<UndoItem>();
            store.AddSingletonRepository<UndoState>();
            return store;

            /*   
    
    
            public readonly record struct BackupSettings(string BackupFilePath, TimeSpan Frequency);
    
            public readonly record struct Command(string Name, string Data);
    
            public readonly record struct KeyBindings(string Key1, string Key2, string Command);
    
            public readonly record struct Job(string Name, bool Completed, bool Canceled, float Progress, bool Determinate);
    
            public readonly record struct LastInput(DateTimeOffset Time);
    
            public enum StatusCode { Good, Warning, Critical }
    
            public readonly record struct Status(string Message, StatusCode Code);
    
            public readonly record struct CuurentFile(string FilePath);
    
            public readonly record struct UndoItem(Guid RepoId, Guid ModelId, string OldValue, string NewValue);
    
            public readonly record struct Freeze();
            */
        }
    }
}