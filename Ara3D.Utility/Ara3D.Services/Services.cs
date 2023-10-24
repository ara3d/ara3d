using System;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public enum ApplicationMode
    {
        Release,
        Debug,
        Developer,
        QA,
        Support, 
    }

    public enum ReleaseType
    {
        ReleaseCandidate,
        Beta,
        Alpha,
        Gold,
        HotFix,
        ServicePack,
        Internal
    }

    public class ApplicationModeService<TMode>
    {
        public ApplicationModeService<TMode> ChangeMode(TMode mode)
            => throw new NotImplementedException();
        public TMode Mode { get; }
    }

    /// <summary>
    /// When the application is start-up, a number of useful 
    /// </summary>
    public class StartupInfoService
    {
    }

    /// <summary>
    /// This service stores system information 
    /// </summary>
    public class CurrentSystemStatusService
    {
        public TimeSpan FrequencyOfUpdate { get; }
        public TimeSpan FrequencyOfSaving { get; }
    }

    /// <summary>
    /// This system manages temporary files, and cleans them up  
    /// </summary>
    public class TempFileService
    {

    }

    /// <summary>
    /// This service retrieves the command line arguments upon start-up and parses them. 
    /// </summary>
    public class CommandLineParsingService
    {

    }

    public class ProfilingService { }
    public class JobService { }
    public class CacheService { }

    /// <summary>
    /// 
    /// </summary>
    public class ClockService { }
    
    public class DispatchingService { }
    
    public class StatusService { }

    public class ApplicationLogService { }
    public class UserService { }
    public class FileSystemService { }

    public class FirstRunService
    {

    }

    public class UserPrefererencesService { }

    public class BackupService
    {
        public TimeSpan Frequency { get; }
    }

    public class MacroService { }
    
    /// <summary>
    /// Determines where the data store is stored, and which repositories are stored. 
    /// </summary>
    public class DataStoreSerializationService
    {
        public DataStoreSerializationService()
        {
        }
    }

    // TODO: https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.eventlog?view=dotnet-plat-ext-6.0
    public class WindowsLogEntry
    {
        public int Index { get; }
        public string Message { get; }
        public int EntryType { get; }
        public string CategoryName { get; }
    }

    public class QueryUserService
    {
        public bool YesNo(string title, string message, string helpUrl) => true;
        public bool OkCanel(string title, string message, string helpUrl) => true;
        public bool AcceptCanel(string title, string message, string helpUrl) => true;
        public bool Ok(string title) => true;
        public bool Cancel(string title) => true;
        public DateTimeOffset ChooseTime() => throw new NotImplementedException();
        public float ChooseValue(float min, float max) => throw new NotImplementedException();
        public float ChooseDate() => throw new NotImplementedException();
        public string ChooseString() => throw new NotImplementedException();
        public string ChooseFile() => throw new NotImplementedException();
        public string ChooseFolder() => throw new NotImplementedException();
        public int ChooseValue(int min, int max) => throw new NotImplementedException();
        public string ChooseValue(string[] values) => throw new NotImplementedException();
    }

    public class ErrorService
    {
        public ErrorService()
        {
            ErrorUtil.SetFirstChanceExceptionCallback((sender, args) => throw new NotImplementedException());
            ErrorUtil.SetUnhandledExceptionCallback((sender, args) => throw new NotImplementedException());
        }

        public void ProcessException(Exception e)
            => throw new ArgumentException();
    }
}
