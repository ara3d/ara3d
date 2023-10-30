using Ara3D.Collections;
using Ara3D.Utils;

namespace Ara3D.Bowerbird.Demo.Winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

    public class CompilerServiceSettings
    {
        public DirectoryPath DirectoryToWatch { get; set; }
        public DirectoryPath AssemblyResultsFolder { get; set; }
        public DirectoryPath BackupFilesFolder { get; set; }
    }

    public class ProjectSettings
    {
        public string Name { get; }
        public Version Version { get; }
        public string[] ReferencedAssemblies { get; set; }
        public ProjectSettings InheritedSettings { get; }
    }

    public class CompilationError 
    { }

    public class CompilationResult
    {
        public AssemblyData Assembly { get; }
    }

    public class FileChangedHash
    {
        public long FileSize { get; }
        public DateTimeOffset ModifiedDate { get; }
        public string Name { get; }
    }

    public class CompilationProject
    {
        public string Name { get; }
        public DirectoryPath Folder { get; }
        public ProjectSettings Settings { get; }
        public CompilationResult Result {get;}
        public ISequence<FilePath> Files { get; }
        public bool AllOrNothing { get; }
    }

    public class CompilerService
    {
        public CompilerServiceSettings Settings { get; }
        
        public CompilerService(CompilerServiceSettings settings)
        {
            Settings = settings;
        }

        public ISequence<string> ProjectFolders { get; }

        public bool CompilerResult { get; }

        public event EventHandler OnSourceFileChanged;
        public event EventHandler CompilationStarted;
        public event EventHandler CompilationCompleted;

        public IArray<CompilationProject> Projects { get; }

        public void Recompile(CompilationProject project) 
            => throw new NotImplementedException();
    }
}