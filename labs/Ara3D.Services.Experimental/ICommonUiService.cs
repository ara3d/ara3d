using System;
using Ara3D.Utils;

namespace Ara3D.Services.Experimental
{
    public enum YesNoCancelResult
    {
        Yes,
        No,
        Cancel,
    }

    public enum OkCancelResult
    {
        Ok,
        Cancel
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }

    public interface INotificationService
    {
        void Notify(string category, string message, NotificationType type, bool modal = false, double autoHideDuration = 0);
    }

    public interface ICommonUiService
    {
        string EnterText(string message, string label, Func<string, bool> validation, string caption);
        int EnterNumber(string message, string label, Func<int, bool> validation, string caption);
        YesNoCancelResult YesNo(string query);
        OkCancelResult OkCancel(string query);
        FilePath SaveFileDialog(string caption);
        FilePath OpenFileDialog(string caption);
    }
}
