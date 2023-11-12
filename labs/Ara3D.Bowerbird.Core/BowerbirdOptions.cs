using Ara3D.Utils;

namespace Ara3D.Bowerbird.Core
{
    public class BowerbirdOptions
    {
        public string AppName { get; set; }
        public DirectoryPath OutputFolder { get; set; }
        public DirectoryPath ScriptFolder { get; set; }
        public DirectoryPath LibFolder { get; set; }
        public FilePath SettingsFile { get; set; }
        public DirectoryPath LogFolder { get; set; }

        public static BowerbirdOptions CreateFromName(string appName)
        {
            var appData = SpecialFolders.LocalApplicationData;
            return new BowerbirdOptions()
            {
                AppName = appName,
                OutputFolder = appData.RelativeFolder(appName, "bin"),
                ScriptFolder = appData.RelativeFolder(appName, "scripts"),
                LibFolder = appData.RelativeFolder(appName, "lib"),
                SettingsFile = appData.RelativeFile(appName, "settings.json"),
                LogFolder = appData.RelativeFolder(appName, "logs"),
            };
        }
    }
}