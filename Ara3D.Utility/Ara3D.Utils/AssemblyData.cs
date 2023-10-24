using System;
using System.Reflection;

namespace Ara3D.Utils
{
    public class AssemblyData
    {
        public AssemblyData(Assembly asm)
        {
            EntryPoint = asm.EntryPoint?.Name ?? "";
            FullName = asm.FullName ?? "";
            ImageRuntimeVersion = asm.ImageRuntimeVersion;
            IsFullyTrusted = asm.IsFullyTrusted;
            Location = asm.Location;
            Architecture = asm.GetName().ProcessorArchitecture.ToString();
            Version = asm.GetName().Version ?? new Version();
            ShortName = asm.GetName().Name ?? "";
            CodeBase = asm.GetName().CodeBase ?? "";
        }

        public string EntryPoint { get; }
        public string FullName { get; }
        public string ImageRuntimeVersion { get; }
        public bool IsFullyTrusted { get; }
        public FilePath Location { get; }
        public string Architecture { get; }
        public Version Version { get; }
        public string ShortName { get; }
        public string CodeBase { get; }
        public DirectoryPath LocationDir => Location.GetDirectory();

        public static AssemblyData Current => 
            new AssemblyData(Assembly.GetExecutingAssembly());
    }
}