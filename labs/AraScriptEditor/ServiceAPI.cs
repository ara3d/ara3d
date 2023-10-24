using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Windows.Forms;


// https://docs.microsoft.com/en-us/dotnet/framework/wcf/migrating-from-net-remoting-to-wcf
// https://dopeydev.com/wcf-interprocess-communication/
// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.remoting.channels.ipc.ipcchannel?view=netframework-4.7.2

namespace Ara3D
{
    /// <summary>
    /// The editor will call these methods depending on what the user does. 
    /// Everything else is managed by the editor itself. 
    /// </summary>
    public interface IEditorClientCallback
    {
        IList<string> Compile(string fileName);
        bool Run(string fileName);
        string NewSnippet();
    }

    /// <summary>
    /// The editor service just needs to be connected to: the client will provide an IEditorCallbackService
    /// The client knows how to compile, the editor just knows how to edit. The Client also knows how to create new snippets
    /// This is a singleton.
    /// </summary>
    public class EditorService : MarshalByRefObject
    {
        public static IEditorClientCallback Callback { get; private set; }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Init(IEditorClientCallback callback)
        {
            Callback = callback;
        }
    }

    /// <summary>
    /// Launches the service executable. You link to this exe like a class library, and then call this function. 
    /// </summary>
    public static class ServiceLauncher
    {
        public static Process LaunchProcess()
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = GetExePath();
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForInputIdle();
            return process;
        }

        public static string GetExePath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            return Uri.UnescapeDataString(uri.Path);
        }
    }

    /// <summary>
    /// Contains the configuration information for the service, and opens a WCF communication endpoint. 
    /// </summary>
    public static class ServiceConfig
    {
        public static string ServiceName = "ScriptEditor.rem";
        public static string ClientPortName = "localhost:9090";
        public static string ServerPortName = "localhost:9091";
        public static string ServerUrl = $"ipc://{ServerPortName}/{ServiceName}";
        public static string ClientUrl = $"ipc://{ClientPortName}/{ServiceName}";
        public static IpcChannel ClientChannel;
        public static IpcChannel ServerChannel;
        public static string ConfigPath => ServiceLauncher.GetExePath() + ".config";
        public static EditorService Service;

        public static IpcChannel CreateChannel(string portName)
        {
            Hashtable properties = new Hashtable();
            if (!string.IsNullOrEmpty(portName))
                properties["portName"] = portName;

            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            var clientProvider = new BinaryClientFormatterSinkProvider();
            var r = new IpcChannel(properties, clientProvider, serverProvider);
            
            // Register the channel.
            ChannelServices.RegisterChannel(r, false);
            return r;
        }

        public static void OpenClientChannel(IEditorClientCallback client)
        {
            // Create a new channel on a separate port (this is used for the callbacks)
            ClientChannel = CreateChannel(ClientPortName);

            // Register as client for remote object.
            var remoteType = new WellKnownClientTypeEntry(typeof(EditorService), ServerUrl);
            RemotingConfiguration.RegisterWellKnownClientType(remoteType);

            // Create an instance of the remote object.
            Service = Activator.GetObject(typeof(EditorService), ServerUrl) as EditorService;

            // Register the callback 
            Service.Init(client);            
        }

        /// <summary>
        /// Called by server. 
        /// </summary>
        public static void OpenServerChannel()
        {
            // Create the server channel.
            ServerChannel = CreateChannel(ServerPortName);

            // Expose an object for remote calls.
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(EditorService), ServiceName, WellKnownObjectMode.Singleton);            
        }
    }

    /// <summary>
    /// The entry point for the program.
    /// </summary>
    public static class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProcessDPIAware();       

        [STAThread]
        public static void Main()
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/winforms/automatic-scaling-in-windows-forms
            // https://docs.microsoft.com/en-us/dotnet/framework/winforms/advanced/how-to-improve-performance-by-avoiding-automatic-scaling
            SetProcessDPIAware();

            ServiceConfig.OpenServerChannel();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScriptEditorForm());
        }
    }
}
