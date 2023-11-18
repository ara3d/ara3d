using System;
using System.Reflection;
using System.Threading;
using Ara3D.Services;
using Ara3D.Utils;

namespace Ara3D.Bowerbird.Core
{
    public class BowerbirdService : BaseService
    {
        public DirectoryWatcher Watcher { get; }
        public FolderCompilation FolderCompilation { get; private set; }
        public ILogger Logger { get; }
        public CancellationTokenSource TokenSource { get; private set; }
        public BowerbirdOptions Options { get; }
        public Assembly Assembly { get; private set; }

        public BowerbirdService(IApi api, ILogger logger, BowerbirdOptions options)
            : base(api)
        {
            Logger = logger;
            Options = options;
            if (!options.ScriptFolder.Exists())
                CreateInitialFolders();
            Watcher = new DirectoryWatcher(Options.ScriptFolder, "*.*", Recompile);
        }
        
        public void CreateInitialFolders()
        {
            Options.ScriptFolder.Create();
            Options.LibFolder.Create();
            Options.LogFolder.Create();
        }
        
        public void Recompile()
        {
            Assembly = null;
            TokenSource?.Cancel();
            using (TokenSource = new CancellationTokenSource())
            {
                var token = TokenSource.Token;

                Logger.Log($"Starting compilation");
                FolderCompilation = new FolderCompilation(Options.ScriptFolder);
                FolderCompilation.Compile(Logger, token);
                foreach (var d in FolderCompilation.Compilation.Diagnostics)
                {
                    Logger.Log($"{d}");
                }

                if (token.IsCancellationRequested)
                {
                    Logger.Log($"Compilation cancelled");
                    return;
                }

                var succeeded = FolderCompilation.Compilation.EmitResult.Success ? "succeeded" : "failed";
                Logger.Log($"Compilation {succeeded}");

                var outputFile = FolderCompilation.CompilerOptions.OutputFile;
                if (outputFile.Exists())
                {
                    Logger.Log($"Loading assembly from {outputFile}");
                    Assembly = Assembly.LoadFile(outputFile);
                    Logger.Log($"Loaded assembly");
                }
                else
                {
                    Logger.Log($"No assembly to load");
                }
            }

            TokenSource = null;
        }
    }
}