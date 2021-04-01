namespace VSCC.VersionManager
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using VSCC.State;

    public class UpdateManager
    {
        public static void Update(string link)
        {
            AppState.Current.Window.Close();
            int pid = Process.GetCurrentProcess().Id;
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.exe"), $"{ pid } { link } false");
        }

        public static void UpdateUpdater(string link)
        {
            WebClient wc = new WebClient();
            string temp = Path.GetTempFileName();
            wc.DownloadFile(link, temp);
            File.Copy(temp, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.exe"), true);
        }
    }
}
