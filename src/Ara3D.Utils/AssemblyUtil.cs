using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Ara3D.Utils
{   
    public static class AssemblyUtil
    {
        public static AssemblyData ToAssemblyData(this Assembly assembly)
            => new AssemblyData(assembly);

        public static void LoadAllAssembliesInCurrentDomainBaseDirectory()
            => LoadAllAssembliesInFolder(AppDomain.CurrentDomain.BaseDirectory);

        // https://stackoverflow.com/questions/44956194/check-if-assembly-exists-by-name-before-loading-it
        public static bool IsLoaded(this AssemblyName asmName)
            => AppDomain.CurrentDomain.GetAssemblies().Any(asm => asmName.FullName == asm.FullName);

        // https://stackoverflow.com/questions/2384592/is-there-a-way-to-force-all-referenced-assemblies-to-be-loaded-into-the-app-doma
        // https://github.com/microsoft/vs-mef/blob/main/doc/hosting.md#hosting-mef-in-an-extensible-application
        public static void LoadAllAssembliesInFolder(DirectoryPath directory)
        {
            foreach (var path in directory.GetFiles("*.dll"))
            {
                try
                {
                    // Don't load an already load
                    var asmName = AssemblyName.GetAssemblyName(path);
                    if (!asmName.IsLoaded())
                        AppDomain.CurrentDomain.Load(asmName);
                }
                catch
                {
                    Debug.WriteLine($"Failed to load {path}");
                }
            }
        }
    }
}