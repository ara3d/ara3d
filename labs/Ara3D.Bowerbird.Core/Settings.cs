using System;
using System.Collections.Generic;
using Ara3D.Utils;

namespace Ara3D.Bowerbird.Core
{
    public class Settings
    {
        public string ProjectPath { get; }
        public List<string> AssemblySearchPath { get; }
    }

    public class Constants
    {
        public const string ReferenceFilesName = "references.txt";
    }   

    public interface ICompilerService 
    { }

    public interface IPluginService
    {
        IReadOnlyList<IPlugin> GetPlugins();
    }

    public interface ICommandService
    {
        IReadOnlyList<INamedCommand> GetCommands();
    }

    public interface IPlugin
    {
        void Initialize(IBowerbirdService service);
        void Start();
        void Stop();
        bool Started { get; }
    }

    public interface IScript
    {
        void Run(IBowerbirdService serfvice);
    }

    // One of these per folder. 
    public class PluginDescriptor
    {
        public string Name { get; }
        public string Author { get; }
        public string Description { get; }
        public string SupportUrl { get; }
        public Guid PluginId { get; }
        public Guid VersionId { get; }
        public Version Version { get; }
        List<string> ReferencedAssemblies { get; }
        List<Guid> ReferencedPlugins { get; }
    }

    public interface IHostService
    { }

    public interface IBowerbirdService
    {
        ICommandService CommandService { get; }
        IPluginService PluginService { get; }
        IHostService HostService { get; }
    }

    public interface IPluginCommand : INamedCommand
    {
        string Name { get; }
        Guid Id { get; }
        string Description { get; }
    }

    // Something that is run every X minutes, 
}
