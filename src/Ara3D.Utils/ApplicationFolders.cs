using System.IO;

namespace Ara3D.Utils
{
    public class ApplicationFolders
    {
        public ApplicationId ApplicationId { get; }

        public string AppFolder
            => Path.Combine(ApplicationId.CompanyName, ApplicationId.AppName);

        public ApplicationFolders(ApplicationId appId)
            => ApplicationId = appId;

        public DirectoryPath ApplicationData => SpecialFolders.ApplicationData.RelativeFolder(AppFolder);
        public DirectoryPath CommonApplicationData => SpecialFolders.CommonApplicationData.RelativeFolder(AppFolder);
        public DirectoryPath ProgramFiles => SpecialFolders.ProgramFiles.RelativeFolder(AppFolder);
        public DirectoryPath Documents => SpecialFolders.ProgramFiles.RelativeFolder(AppFolder);
        public DirectoryPath TempFolder => SpecialFolders.Temp.RelativeFolder(AppFolder);
    }
}