using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

namespace Ara3D.Utils.Roslyn
{
    public static partial class RoslynUtils
    {
        public static IEnumerable<MetadataReference> ReferencesFromFiles(IEnumerable<string> files)
            => files.Select(x => MetadataReference.CreateFromFile(x));

        public static IEnumerable<MetadataReference> ReferencesFromLoadedAssemblies()
            => ReferencesFromFiles(LoadedAssemblyLocations());

        public static IEnumerable<string> LoadedAssemblyLocations(AppDomain domain = null)
            => (domain ?? AppDomain.CurrentDomain).GetAssemblies().Where(x => !x.IsDynamic).Select(x => x.Location);

        public static string ToPackageReference(this AssemblyIdentity asm)
            => $"<PackageReference Include=\"{asm.Name}\" Version=\"{asm.Version}\" />";

        public static string GetOrCreateDir(string path)
            => Directory.Exists(path) ? path : Directory.CreateDirectory(path).FullName;

        public static string GenerateNewDllFileName()
            => PathUtil.CreateTempFile("dll");

        public static string GenerateNewSourceFileName()
            => PathUtil.CreateTempFile("cs");

        public static FilePath WriteToTempFile(string source)
        {
            var path = GenerateNewSourceFileName();
            File.WriteAllText(path, source);
            return path;
        }

    }
}
