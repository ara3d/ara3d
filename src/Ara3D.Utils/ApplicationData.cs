using System;

namespace Ara3D.Utils
{
    public class ApplicationData
    {
        public string CompanyName { get; }
        public string AppName { get; }
        public Version AppVersion { get; }

        public ApplicationData(string companyName, string appName, Version appVersion)
            => (CompanyName, AppName, AppVersion) = (companyName, appName, appVersion);
    }
}