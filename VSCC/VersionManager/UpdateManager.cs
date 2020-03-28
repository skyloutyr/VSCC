using System;
using System.Diagnostics;
using System.IO;
using VSCC.State;

namespace VSCC.VersionManager
{
    public class UpdateManager
    {
        public static void Update(string link)
        {
            AppState.Current.Window.Close();
            int pid = Process.GetCurrentProcess().Id;
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.exe"), $"{ pid } { link } false");
        }
    }
}
