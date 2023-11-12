using System;
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
            TokenSource?.Cancel();
            TokenSource = new CancellationTokenSource();

            Logger.Log($"Starting compilation");
            FolderCompilation = new FolderCompilation(Options.ScriptFolder);
            FolderCompilation.Compile(Logger, TokenSource.Token);
            foreach (var d in FolderCompilation.Compilation.Diagnostics)
            {
                Logger.Log($"{d}");
            }

            var succeeded = FolderCompilation.Compilation.EmitResult.Success ? "succeeded" : "failed";
            Logger.Log($"Compilation {succeeded}");

        }
    }
}