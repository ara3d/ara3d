using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Ara3D.UnityHost.Wpf
{
    // https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/walkthrough-hosting-a-win32-control-in-wpf?view=netframeworkdesktop-4.8
    public class UnityHost : HwndHost
    {
        public UnityHost()
        {
        }

        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int
            WS_CHILD = 0x40000000,
            WS_VISIBLE = 0x10000000,
            LBS_NOTIFY = 0x00000001,
            HOST_ID = 0x00000002,
            LISTBOX_ID = 0x00000001,
            WS_VSCROLL = 0x00200000,
            WS_BORDER = 0x00800000;

        IntPtr hwndControl;
        IntPtr hwndHost;
        int hostHeight, hostWidth;
        Process process = new Process(); 

        public UnityHost(double height, double width)
        {
            hostHeight = (int)height;
            hostWidth = (int)width;
        }

        //PInvoke declarations
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(int dwExStyle,
            string lpszClassName,
            string lpszWindowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            hwndHost = CreateWindowEx(0, "static", "",
                WS_CHILD | WS_VISIBLE,
                0, 0,
                hostWidth, hostHeight,
                hwndParent.Handle,
                (IntPtr)HOST_ID,
                IntPtr.Zero,
                0);
            return new HandleRef(this, hwndHost);
        }

        private int RetrieveChildHWnd(IntPtr hwnd, IntPtr lparam)
        {
            hwndControl = hwnd;
            ActivateUnityWindow();
            return 0;
        }

        public void Init()
        {
            process.StartInfo.FileName = "C:\\Users\\cdigg\\TestUnityProjectBuild1\\My project.exe";
            process.StartInfo.Arguments = "-parentHWND "
                                          + hwndHost.ToInt32() + " "
                                          + Environment.CommandLine;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            process.WaitForInputIdle();


            EnumChildWindows(hwndHost, RetrieveChildHWnd, IntPtr.Zero);
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }
        
        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }

        private void ActivateUnityWindow()
        {
            //SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void DeactivateUnityWindow()
        {
            //SendMessage(unityHWND, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
        }

    }
}
