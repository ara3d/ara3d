using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Ara3D.Domo;
using Ara3D.Services;
using Ara3D.Utils;

namespace Ara3D.Bowerbird.Core
{
    public class BowerbirdDataModel
    {
        public IReadOnlyList<Type> Types = Array.Empty<Type>();
        public IReadOnlyList<string> Files = Array.Empty<string>();
        public IReadOnlyList<string> Diagnostics = Array.Empty<string>();
        public string Dll = "";
        public BowerbirdOptions Options = new BowerbirdOptions();
        public bool ParseSuccess;
        public bool EmitSuccess;
        public bool LoadSuccess;
    }

    public class BowerbirdService : BaseService
    {
        public DirectoryWatcher Watcher { get; }
        public FolderCompilation FolderCompilation { get; private set; }
        public ILogger Logger { get; }
        public CancellationTokenSource TokenSource { get; private set; }
        public BowerbirdOptions Options { get; }
        public Assembly Assembly { get; private set; }
        public SingletonRepository<BowerbirdDataModel> Repo { get; } = new SingletonRepository<BowerbirdDataModel>();

        public BowerbirdService(IApi api, ILogger logger, BowerbirdOptions options)
            : base(api)
        {
            Logger = logger;
            Options = options;
            UpdateDataModel();
            CreateInitialFolders();
            Watcher = new DirectoryWatcher(Options.ScriptFolder, "*.*", Compile);
        }
        
        public void CreateInitialFolders()
        {
            Options.ScriptFolder.Create();
            Options.LibFolder.Create();
            Options.LogFolder.Create();
        }

        public void Compile()
        {
            try
            {
                Assembly = null;
                FolderCompilation = null;
                UpdateDataModel();

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
            finally
            {
                UpdateDataModel();
            }
        }

        public void UpdateDataModel()
        {
            Repo.Value = new BowerbirdDataModel()
            {
                Dll = Assembly?.Location ?? "",
                Types = Assembly?.GetExportedTypes().OrderBy(t => t.FullName).ToArray() ?? Array.Empty<Type>(),
                Files = FolderCompilation?.Files?.Select(fp => fp.Value).OrderBy(x => x).ToArray() ?? Array.Empty<string>(),
                Diagnostics = FolderCompilation?.Compilation?.Diagnostics?.Select(d => d.ToString()).ToArray() ?? Array.Empty<string>(),
                ParseSuccess = FolderCompilation?.CompilerInput?.HasParseErrors == false,
                EmitSuccess = FolderCompilation?.Success == true,
                LoadSuccess = Assembly != null,
            };
        }
    }
}