using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ara3D.Utils.Roslyn;
using Assembly = System.Reflection.Assembly;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public struct PluginRecord
    {
        public string Name { get; }
        public bool Initialized { get; }
        public string Error { get; }

        public PluginRecord(string name, bool initialized = true, string error = null)
            => (Name, Initialized, Error) = (name, initialized, error);
    }

    public class PluginRepo : AggregateRepository<PluginRecord>
    { }

    public class PluginService : BaseService
    {
        public class Options
        {
            public string[] PluginDirectories { get; set; }
            public string[] PluginFiles { get; set; }
            public string PluginPattern { get; set; }
            public string ScriptsDirectory { get; set; }
            public string[] InputDlls { get; set; }
        }

        public IApi Api { get; }
        public List<IPlugin> LoadedPlugins { get; set; } = new List<IPlugin>();
        public List<IPlugin> ScriptedPlugins { get; set; } = new List<IPlugin>();
        public CompilerService CompilerService { get; }
        public CommandService CommandService { get; }
        public CompilerRepo CompilerRepo { get; }
        public PluginRepo PluginRepo { get; }
        public ILogger Logger { get; }
        public string Name => nameof(PluginService);

        public PluginService(IApi api, Options options, CommandService commandService, CompilerRepo compilerRepo, PluginRepo pluginRepo, ILogger logger)
            : base(api)
        {
            Api = api;
            CommandService = commandService;
            CompilerRepo = compilerRepo;
            PluginRepo = pluginRepo;
            Logger = logger;

            if (!string.IsNullOrWhiteSpace(options.ScriptsDirectory))
            {
                // TODO: we need to create this from the options passed to the PLugin service
                var compilerOptions = new CompilerOptions();
                CompilerService = new CompilerService(null, compilerOptions, options.ScriptsDirectory);
                CompilerService.RecompileEvent += Controller_RecompileEvent;
                CompilerService.AutoRecompile = true;
                CompilerService.Recompile();
            }

            foreach (var dir in options.PluginDirectories ?? Array.Empty<string>())
            {
                LoadedPlugins.AddRange(LoadPluginsFromDirectory(dir, options.PluginPattern ?? "*.dll", Logger));
            }

            foreach (var file in options.PluginFiles ?? Array.Empty<string>())
            {
                LoadedPlugins.AddRange(LoadPluginsFromFile(file, Logger));
            }

            foreach (var plugin in LoadedPlugins)
            {
                InitializePlugin(plugin);
            }
        }

        public bool InitializePlugin(IPlugin plugin)
        {
            try
            {
                plugin.Initialize(Api);
                if (plugin.Api != Api)
                    throw new Exception("Plugin does not return the API: probably forgot to call base.Initialize");
                Api.EventBus.AddSubscriberUsingReflection(plugin);
                var id = Guid.NewGuid();
                PluginRepo.Add(new PluginRecord(plugin.Name));
                CommandService.AddAttributeCommands(plugin); 
                plugin.Disposing += (_sender, _args) => PluginRepo.Delete(id);
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                PluginRepo.Add(new PluginRecord(plugin.Name, false, e.Message));
                return false;
            }
        }

        private void Controller_RecompileEvent(object sender, EventArgs e)
        {
            foreach (var p in ScriptedPlugins)
            {
                p.Dispose();
            }

            ScriptedPlugins.Clear();

            //throw new NotImplementedException();
            /*

            CompilerRepo.Model.Update(x =>
                // TODO: 
                new CompilationModel()
                {
                    Diagnostics = CompilerService.Compilation.Diagnostics.Select(s => s.ToString()).ToArray(),
                    InputFiles = CompilerService.Compilation.Input.SourceFiles.Select(s => s.FilePath.Value).ToArray(),
                    OutputDll = CompilerService.Compilation.Options.OutputFile,
                    Success = CompilerService.Compilation.EmitResult.Success,
                });

            var model = CompilerService.Model;
            if (model.Compiled)
            {
                var plugins = LoadPluginsFromFile(model.OutputDll, Logger);
                if (plugins.Count > 0)
                {
                    foreach (var p in plugins.ToArray())
                    {
                        if (!InitializePlugin(p))
                            plugins.Remove(p);
                    }

                    ScriptedPlugins.AddRange(plugins);
                }
            }*/
        }

        public static bool IsPluginType(Type t)
            => t.ImplementsInterface<IPlugin>() && t.HasDefaultConstructor();

        public static List<IPlugin> LoadPluginsFromFile(string fileName, ILogger logger, bool useLoadFrom = true)
        {
            var r = new List<IPlugin>();
            Assembly asm;
            try
            {
                asm = Assembly.LoadFile(fileName);
                //asm = useLoadFrom ? Assembly.LoadFrom(fileName) : Assembly.LoadFile(fileName);
            }
            catch (Exception e)
            {
                logger.LogError(e);
                return r;
            }

            var pluginTypes = asm.GetExportedTypes().Where(IsPluginType);
            foreach (var t in pluginTypes)
            {
                try
                {
                    var plugin = (IPlugin)Activator.CreateInstance(t);
                    if (plugin != null) 
                    {
                        r.Add(plugin);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"Error when creating {t.Name}", e);
                }
            }
            return r;
        }

        public static IPlugin[] LoadPluginsFromDirectory(string pluginDirectory, string filePattern, ILogger logger)
        {
            if (!Directory.Exists(pluginDirectory))
            {
                logger.LogWarning($"Could not find directory {pluginDirectory}");
                return Array.Empty<IPlugin>();
            }
            var dlls = Directory.GetFiles(pluginDirectory, filePattern);
            return dlls.SelectMany(f => LoadPluginsFromFile(f, logger)).ToArray();
        }
    }
}
