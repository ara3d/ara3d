using System;
using System.Linq;
using System.Threading;

namespace Ara3D.Utils.Roslyn
{
    public class CompilerService
    {
        public ILogger Logger { get; }
        // TODO: a compiler service might create assemblies in sub-folders.
        // We would only want one watcher for the whole application. 
        public DirectoryWatcher Watcher;
        public Compilation Compilation { get; set; }
        public CompilerOptions Options { get; set; }
        public bool AutoRecompile { get; set; } = true;
        public CancellationTokenSource TokenSource = new CancellationTokenSource();

        public void Log(string s)
            => Logger?.Log(s);

        public event EventHandler RecompileEvent;
        
        public CompilerService(ILogger logger, CompilerOptions options, DirectoryPath dir)
        {
            Logger = logger.Create($"CompilerService: {dir}");
            Watcher = new DirectoryWatcher(dir, "*.cs", false, OnChange, null);
            Recompile();
        }

        public void OnChange()            
        {
            if (AutoRecompile)
                Recompile();
        }

        public void Recompile()
        {
            try
            {
                Log("Requesting cancel of existing work...");
                TokenSource.Cancel();
                TokenSource = new CancellationTokenSource();
                var token = TokenSource.Token;

                Log("Compilation task started");

                // TODO: get the input reference files from somewhere 
                Options = Options?.WithNewOutputFilePath() ?? new CompilerOptions();
                Log($"Chosen output file = {Options.OutputFile}");
                Log($"References:");
                foreach (var f in Options.MetadataReferences)
                    Log($"  Reference: {f.Display}");

                var inputFiles = Watcher.GetFiles().ToArray();
                foreach (var f in inputFiles)
                    Log($"  Input file {f}");

                Log("Parsing input files");
                var sourceFiles = inputFiles.ParseCSharp(Options, token);
                
                if (token.IsCancellationRequested)
                {
                    Log($"Compilation canceled");
                    return;
                }

                Log("Generating compiler input");
                var input = sourceFiles.ToCompilerInput(Options);

                Log("Compiling");
                Compilation = input.CompileCSharpStandard(Compilation?.Compiler, token);
                if (token.IsCancellationRequested)
                {
                    Log($"Canceled");
                    return;
                }

                Log($"Emitted assembly success = {Compilation.EmitResult?.Success}");

                Log($"Output file {Options.OutputFile} exists {Options.OutputFile.Exists()}");

                Log($"Diagnostics:");
                foreach (var x in Compilation.Diagnostics)
                    Log($"  Diagnostic: {x}");

                Log($"Completed compilation");
            }
            finally
            {
                RecompileEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
