using Ara3D.Utils;

namespace Ara3D.DevOpToolsAsTests
{
    public static class DevOpTools
    {
        public static IEnumerable<FilePath> GetAllProjects()
            => SourceCodeLocation.GetFolder().RelativeFolder("..", "..").GetFiles("*.csproj", true);
        
        [Test, Explicit]
        public static void UpgradeVersion()
        {
            var oldVer = "1.3.1";
            var newVer = "1.4.0";

            foreach (var project in GetAllProjects())
            {
                Console.WriteLine($"Loading {project}");
                project.LoadXml()
                    .SetAttributesWhere(x => x.Name == "PackageReference"
                                             && x.Attribute("Include")?.Value?.StartsWith("Ara3D.") == true
                                             && x.Attribute("Version")?.Value?.Equals(oldVer) == true,
                        "Version",
                        newVer)
                    .SaveXml(project);
            }
        }
    }
}