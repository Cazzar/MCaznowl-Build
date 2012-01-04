using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;
namespace MCForge
{
   namespace SingleInstance
    {
        public class SingleApplication
        {
            public SingleApplication()
            {

            }
          
            [DllImport("user32.dll")]
            private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            private static extern int SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            private static extern int IsIconic(IntPtr hWnd);
            private static IntPtr GetCurrentInstanceWindowHandle()
            {
                IntPtr hWnd = IntPtr.Zero;
                Process process = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(process.ProcessName);
                foreach (Process _process in processes)
                {
                  if (_process.Id != process.Id &&
                        _process.MainModule.FileName == process.MainModule.FileName &&
                        _process.MainWindowHandle != IntPtr.Zero)
                    {
                        hWnd = _process.MainWindowHandle;
                        break;
                    }
                }
                return hWnd;
            }
           private static void SwitchToCurrentInstance()
            {
                IntPtr hWnd = GetCurrentInstanceWindowHandle();
                if (hWnd != IntPtr.Zero)
                {
                    if (IsIconic(hWnd) != 0)
                    {
                        ShowWindow(hWnd, SW_RESTORE);
                    }
                        SetForegroundWindow(hWnd);
                }
            }
            public static bool Run(System.Windows.Forms.Form frmMain)
            {
                if (IsAlreadyRunning())
                {
                    SwitchToCurrentInstance();
                    return false;
                }
                Application.Run(frmMain);
                return true;
            }
            public static bool Run()
            {
                if (IsAlreadyRunning())
                {
                    return false;
                }
                return true;
            }
            private static bool IsAlreadyRunning()
            {
                string strLoc = Assembly.GetExecutingAssembly().Location;
                FileSystemInfo fileInfo = new FileInfo(strLoc);
                string sExeName = fileInfo.Name;
                bool bCreatedNew;

                mutex = new Mutex(true, "Global\\" + sExeName, out bCreatedNew);
                if (bCreatedNew)
                    mutex.ReleaseMutex();

                return !bCreatedNew;
            }

            static Mutex mutex;
            const int SW_RESTORE = 9;
        }
    }

}
