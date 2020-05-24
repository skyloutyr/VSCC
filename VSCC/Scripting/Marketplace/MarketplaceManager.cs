namespace VSCC.Scripting.Marketplace
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using VSCC.State;

    public class MarketplaceManager
    {
        public static MarketplaceManager Instance { get; set; } = new MarketplaceManager();

        public MarketplaceIndex Index { get; set; }
        public ObservableCollection<MarketplaceEntry> RemoteDB { get; } = new ObservableCollection<MarketplaceEntry>();
        public ObservableCollection<MarketplaceEntry> LocalDB { get; } = new ObservableCollection<MarketplaceEntry>();
        public ObservableCollection<MarketplaceAlert> Alerts { get; } = new ObservableCollection<MarketplaceAlert>();

        public void LoadIndex(string index)
        {
            try
            {
                this.Index = JsonConvert.DeserializeObject<MarketplaceIndex>(index);
            }
            catch (Exception e)
            {
                throw new Exception("Marketplace index JSON malformed!", e);
            }
        }

        public void CheckMarketplaceEntryAvailability(MarketplaceEntry me)
        {
            MarketplaceEntry otherEntry = me.IsLocal ? this.RemoteDB.FirstOrDefault(e => e.ProjectID.Equals(me.ProjectID, StringComparison.OrdinalIgnoreCase)) : this.LocalDB.FirstOrDefault(e => e.ProjectID.Equals(me.ProjectID, StringComparison.OrdinalIgnoreCase));
            if (otherEntry != null)
            {
                MarketplaceEntry local = me.IsLocal ? me : otherEntry;
                MarketplaceEntry remote = me.IsLocal ? otherEntry : me;
                local.HasUpdate = remote.HasUpdate = remote.LatestVersion > local.LatestVersion;
                local.DownloadButtonEnabled = remote.DownloadButtonEnabled = local.HasUpdate || !remote.MD5.Equals(local.MD5, StringComparison.OrdinalIgnoreCase);
                if (!remote.MD5.Equals(local.MD5, StringComparison.OrdinalIgnoreCase))
                {
                    this.Alerts.Add(MarketplaceAlert.ChecksumMismatch);
                }

                if (remote.HasUpdate)
                {
                    this.Alerts.Add(MarketplaceAlert.UpdatesAvailable);
                }
            }
            else
            {
                if (!me.IsLocal)
                {
                    me.DownloadButtonEnabled = true;
                }
            }
        }

        public void LoadLocalMarketplace(Dispatcher dispatcher)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");
            if (Directory.Exists(path))
            {
                // Outdated scripts first
                foreach (string file in Directory.EnumerateFiles(path, "*.lua", SearchOption.TopDirectoryOnly))
                {
                    string fileCapture = file;
                    Task.Run(() =>
                    {
                        FileInfo fi = new FileInfo(fileCapture);
                        string checksum;
                        using (MD5 md5 = MD5.Create())
                        {
                            checksum = AppState.GetMd5Hash(md5, File.ReadAllText(fileCapture));
                        }

                        dispatcher.Invoke(() =>
                        {
                            MarketplaceEntry me =
                            new MarketplaceEntry()
                            {
                                ProjectID = "local_unknown_" + Guid.NewGuid().ToString(),
                                Name = fi.Name,
                                ShortDescription = "Unknown local script",
                                FullDescription = "This script was found locally in the Scripts directory but it doesn't use the new scripting format. This file was still loaded as a script but it will have no interaction with the scripting marketplace!",
                                IsLocal = true,
                                Version = 0,
                                LatestVersion = new SemVer.Version(1, 0, 0),
                                AppRange = new SemVer.Range("*"),
                                Changelog = new System.Collections.Generic.Dictionary<string, string>(),
                                License = "Unknown, assume All Rights Reserved.",
                                Authors = new string[] { "Unknown" },
                                Tags = new string[0],
                                Link = fileCapture,
                                MD5 = checksum,
                                Flags = new MarketplaceEntryFlag[] { MarketplaceEntryFlag.LocalScriptOnly },
                                LocalPath = fileCapture,
                                HasNoMetadata = true
                            };

                            this.LocalDB.Add(me);
                            this.CheckMarketplaceEntryAvailability(me);
                        });
                    });
                }

                // Iterate directories
                foreach (string directory in Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly))
                {
                    string mpe = Directory.EnumerateFiles(directory, "*.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (mpe != null)
                    {
                        Task.Run(() =>
                        {
                            MarketplaceEntry me = JsonConvert.DeserializeObject<MarketplaceEntry>(File.ReadAllText(mpe));
                            me.IsLocal = true;
                            me.LocalPath = mpe;
                            string lp = Directory.EnumerateFiles(directory, "*.lua", SearchOption.TopDirectoryOnly).FirstOrDefault();
                            me.LocalScriptPath = mpe;
                            using (MD5 md5 = MD5.Create())
                            {
                                me.MD5 = AppState.GetMd5Hash(md5, File.ReadAllText(lp));
                            }

                            dispatcher.Invoke(() =>
                            {
                                this.LocalDB.Add(me);
                                this.CheckMarketplaceEntryAvailability(me);
                            });
                        });
                    }
                }
            }
        }

        public void LoadOnlineMarketplace(Dispatcher dispatcher)
        {
            foreach (string s in this.Index.ScriptsDB)
            {
                string capture = s;
                Task.Run(() => this.FetchScript("https://raw.githubusercontent.com/skyloutyr/VSCCScripts/master/defines/" + capture, dispatcher));
            }
        }

        private void FetchScript(string path, Dispatcher dispatcher)
        {
            try
            {
                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpWebRequest req = WebRequest.CreateHttp(path);
                req.Timeout = 5000;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                req.CachePolicy = noCachePolicy;
                using (WebResponse response = req.GetResponse())
                {
                    using (Stream s = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            MarketplaceEntry me = JsonConvert.DeserializeObject<MarketplaceEntry>(sr.ReadToEnd());
                            dispatcher.Invoke(() =>
                            {
                                this.RemoteDB.Add(me);
                                this.CheckMarketplaceEntryAvailability(me);
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Silently log it?
                if (Debugger.IsAttached)
                {
                    Debugger.Log(0, string.Empty, "An exception has occured fetching script at " + path + "\n" + e);
                }
            }
        }
    }

    public enum MarketplaceAlert
    {
        ChecksumMismatch,
        UpdatesAvailable,
        RestartNeeded,
        DownloadFailed
    }
}
