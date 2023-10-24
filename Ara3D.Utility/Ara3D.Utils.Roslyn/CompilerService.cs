using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ara3D.Utils.Roslyn
{
    public class CompilerService
    {
        public CompilerModel Model { get; private set; }
        public CompilerViewModel ViewModel { get; } = new CompilerViewModel();
        public object CompilationLock { get; } = new object();

        public ILogger Logger { get; }

        public DirectoryWatcher Watcher;
        public Compilation Compilation = new Compilation();
        public string ScriptDirectory => Watcher.Watcher.Path;
        public CancellationTokenSource TokenSource = new CancellationTokenSource();

        public static string AssemblyDirectory
            = AssemblyData.Current.LocationDir;

        public void Log(string s)
            => Logger?.Log(s);

        public event EventHandler RecompileEvent;
        
        public CompilerService(ILogger logger, string dir, IEnumerable<string> inputDlls)
        {
            Logger = logger.Create("CompilerService");
            Compilation = Compilation.UpdateReferences(inputDlls);
            Watcher = new DirectoryWatcher(dir, "*.cs", false, OnChange, null);
        }

        public void OnChange()            
        {
            ViewModel.Changed = true;
            if (ViewModel.AutoCompile)
                Recompile();
        }

        public void Reset()
        {
            lock (CompilationLock)
            {
                Model = null;
                ViewModel.Model = null;
            }
        }
        
        public CompilerModel Recompile()
        {
            Reset();
            Compilation = Compilation?.UpdateOutputFile();

            try
            {
                Log("Requesting cancel of existing work...");
                TokenSource.Cancel();
                TokenSource = new CancellationTokenSource();

                Log("Compilation task Started");
                var inputFiles = Watcher.GetFiles().ToArray();
                foreach (var f in inputFiles)
                    Log($"  Input file {f}");

                Log("Updating input files");
                Compilation = Compilation.UpdateInputFiles(inputFiles, TokenSource.Token);

                Log("Emitting project file");
                Compilation = Compilation.Emit(TokenSource.Token);

                Log($"Emitted assembly success = {Compilation.EmitResult.Success} output = {Compilation.Options.OutputFileName}");

                Log($"Diagnostics");
                foreach (var x in Compilation.EmitResult.Diagnostics)
                    Log($"Diagnostic: {x}");

                Log("Creating a new model");
                Model = new CompilerModel(Compilation, ScriptDirectory);

                Log("Inform the view model of a change to the model");
                ViewModel.Model = Model;

                return Model;
            }
            finally
            {
                ViewModel.Changed = false;
                RecompileEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
