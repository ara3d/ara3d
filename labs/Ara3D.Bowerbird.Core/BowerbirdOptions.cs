using Ara3D.Utils;

namespace Ara3D.Bowerbird.Core
{
    public class BowerbirdOptions
    {
        public string AppName { get; set; }
        public string OrgName { get; set; }
        public DirectoryPath OutputFolder { get; set; }
        public DirectoryPath ScriptFolder { get; set; }
        public DirectoryPath LibFolder { get; set; }
        public FilePath SettingsFile { get; set; }
        public DirectoryPath LogFolder { get; set; }

        public static BowerbirdOptions CreateFromName(string orgName, string appName)
        {
            var appData = SpecialFolders.LocalApplicationData;
            return new BowerbirdOptions()
            {
                OrgName = orgName,
                AppName = appName,
                OutputFolder = appData.RelativeFolder(orgName, appName, "bin"),
                ScriptFolder = appData.RelativeFolder(orgName, appName, "scripts"),
                LibFolder = appData.RelativeFolder(orgName, appName, "lib"),
                SettingsFile = appData.RelativeFile(orgName, appName, "settings.json"),
                LogFolder = appData.RelativeFolder(orgName, appName, "logs"),
            };
        }
    }
}