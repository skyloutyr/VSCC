namespace VSCC.Roll20
{
    using System;
    using System.IO;

    public sealed class R20Logger
    {
        public static string LogPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public static TextWriter LogWriter { get; private set; }
        public static bool Exists { get; set; }

        public static void Init()
        {
            Exists = true;
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            string logPath = Path.Combine(LogPath, "r20-log-latest.txt");
            string logPPath = Path.Combine(LogPath, "r20-log-previous.txt");
            string logOPath = Path.Combine(LogPath, "r20-log-oldest.txt");
            if (File.Exists(logPPath))
            {
                File.Copy(logPPath, logOPath, true);
            }

            if (File.Exists(logPath))
            {
                File.Copy(logPath, logPPath, true);
            }

            LogWriter = File.CreateText(logPath);
        }

        public static void Close()
        {
            Exists = false;
            try
            {
                LogWriter?.Close();
            }
            catch (Exception)
            {
                // NOOP
            }
        }

        public static void WriteLine(string text)
        {
            if (Exists)
            {
                LogWriter.WriteLine(text);
                LogWriter.Flush();
            }
        }
    }
}
