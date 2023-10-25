using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Ara3D.Utils
{
    public static class Helpers
    {
        public static void SetFirstChanceExceptionCallback(Action<object, FirstChanceExceptionEventArgs> handler)
            => AppDomain.CurrentDomain.FirstChanceException += (sender, args) => handler(sender, args);

        public static void SetProcessExitCallback(Action<object, EventArgs> handler)
        {
            var p = Process.GetCurrentProcess();
            p.Exited += (sender, args) => handler(sender, args);
            //AppDomain.CurrentDomain.ProcessExit += (sender, args) => handler(sender, args);
        }

        public static void SetUnhandledExceptionCallback(Action<object, UnhandledExceptionEventArgs> handler)
            => AppDomain.CurrentDomain.UnhandledException += (sender, args) => handler(sender, args);   

        public static FileVersionInfo ToFileVersion(this string fileName)
            => FileVersionInfo.GetVersionInfo(fileName);
    }
}