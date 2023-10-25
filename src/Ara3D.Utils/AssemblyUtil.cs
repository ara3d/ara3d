using System.Reflection;

namespace Ara3D.Utils
{
    public static class AssemblyUtil
    {
        public static AssemblyData ToAssemblyData(this Assembly assembly)
            => new AssemblyData(assembly);
    }
}