using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Updater
{
    public class Program
    {
        public static int PID { get; set; }
        public static string Link { get; set; }
        public static bool Force { get; set; }
        public static int Stage { get; set; } = 0;
        public static float DownloadPercentage { get; set; } = 0;
        public static string TempFileName { get; set; }
        public static int DownloadStatus { get; set; } = 0;
        public static List<string> ErroredFiles { get; } = new List<string>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting VSCC updater.");
            try
            {
                PID = int.Parse(args[0]);
                Link = args[1];
                Force = bool.Parse(args[2]);
            }
            catch
            {
                Console.WriteLine("Error: Invalid arguments passed.");
                Console.ReadKey();
                return;
            }

            while (ConsoleLoop());
        }

        public static bool ConsoleLoop()
        {
            Console.Clear();
            if (Stage == 0)
            {
                if (CheckProcess(out Process p))
                {
                    Console.WriteLine("Main application process is still running. Close the application before updating.");
                    Console.WriteLine(string.Empty);
                    Console.WriteLine("    Application(s) still running: " + p.ProcessName);
                }
                else
                {
                    Stage = 1;
                    new Thread(StartDownload).Start();
                }
            }
            else
            {
                if (Stage == 1)
                {
                    if (DownloadStatus == 0)
                    {
                        int completed = (int)(DownloadPercentage * 100);
                        int left = 100 - completed;
                        Console.WriteLine($"Downloading the new package... ({ completed }%)");
                        Console.WriteLine(string.Empty);
                        Console.WriteLine($"[{ new string('|', completed) }{ new string('|', left) }]");
                    }
                    else
                    {
                        if (DownloadStatus == -1)
                        {
                            Console.WriteLine("An error occured downloading the new update.");
                            Console.ReadKey();
                            return false;
                        }
                        else
                        {
                            Stage = 2;
                            DownloadStatus = 0;
                            DownloadPercentage = 0;
                            new Thread(DoUpdate).Start();
                        }
                    }
                }
                else
                {
                    if (Stage == 2)
                    {
                        if (DownloadStatus == 0)
                        {
                            int completed = (int)(DownloadPercentage * 100);
                            int left = 100 - completed;
                            Console.WriteLine($"Updating... ({ completed }%)");
                            Console.WriteLine(string.Empty);
                            Console.WriteLine($"[{ new string('|', completed) }{ new string('|', left) }]");
                            Console.WriteLine(string.Empty);
                            foreach (string error in ErroredFiles)
                            {
                                Console.WriteLine("Errored file - " + error);
                            }
                        }
                        else
                        {
                            if (ErroredFiles.Count == 0)
                            {
                                Console.WriteLine($"Update complete. Press any key to finish updating.");
                                Console.ReadKey();
                                return false;
                            }
                            else
                            {
                                Console.WriteLine("There were errors during the update. It is recommended to re-download and reinstall the package manually.");
                                Console.WriteLine(Link);
                                foreach (string error in ErroredFiles)
                                {
                                    Console.WriteLine("Errored file - " + error);
                                }

                                Console.WriteLine("Press any key to exit.");
                                Console.ReadKey();
                                return false;
                            }
                        }
                    }
                }
            }

            Thread.Sleep(250);
            return true;
        }

        public static bool CheckProcess(out Process process)
        {
            try
            {
                process = Process.GetProcessById(PID);
                if (process?.HasExited ?? false)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                process = null;
                return false;
            }
        }

        public static void StartDownload()
        {
            Thread.CurrentThread.IsBackground = true;
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += (o, e) => 
            {
                DownloadPercentage = (float)(e.BytesReceived / (double)e.TotalBytesToReceive);
            };

            wc.DownloadFileCompleted += (o, e) =>
            {
                DownloadStatus = e.Cancelled || e.Error != null ? -1 : 1;
            };

            wc.DownloadFile(Link, TempFileName = Path.GetTempFileName());
        }

        public static void DoUpdate()
        {
            Thread.CurrentThread.IsBackground = true;
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            ZipFile.ExtractToDirectory(TempFileName, tempDir);
            File.Delete(TempFileName);
            int filesAll = Directory.GetFiles(tempDir, "*.*", SearchOption.AllDirectories).Length;
            int filesProcessed = 0;
            foreach (string file in Directory.EnumerateFiles(tempDir, "*.*", SearchOption.AllDirectories))
            {
                string relativeFile = file.Substring(tempDir.Length + 1);
                if (relativeFile.Contains("Updater.exe", StringComparison.OrdinalIgnoreCase))
                {
                    ++filesProcessed;
                    continue;
                }

                string currentFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFile);
                if (!File.Exists(currentFile))
                {
                    try
                    {
                        File.Move(file, currentFile);
                    }
                    catch
                    {
                        ErroredFiles.Add(currentFile);
                    }
                }
                else
                {
                    MD5 currentFileMD5 = MD5.Create();
                    byte[] hashCurrent = currentFileMD5.ComputeHash(File.ReadAllBytes(currentFile));
                    MD5 newFileMD5 = MD5.Create();
                    byte[] hashNew = newFileMD5.ComputeHash(File.ReadAllBytes(file));
                    if (!string.Equals(GetMd5Hash(hashCurrent), GetMd5Hash(hashNew)))
                    {
                        try
                        {
                            File.Replace(file, currentFile, currentFile + ".bak");
                        }
                        catch
                        {
                            File.Move(currentFile + ".bak", currentFile);
                            ErroredFiles.Add(currentFile);
                        }
                    }
                }

                ++filesProcessed;
                DownloadPercentage = filesProcessed / (float)filesAll;
            }

            Directory.Delete(tempDir, true);
            DownloadStatus = 1;
        }

        public static string GetMd5Hash(byte[] input)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte b in input)
            {
                sBuilder.Append(b.ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
