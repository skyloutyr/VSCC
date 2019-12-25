using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VSCC.VersionManager
{
    public class VersionChecker
    {
        public static async Task CheckVersion(bool showFineWindows = true, bool callUpdateFromVC = true, Action<string> updateCallback = null)
        {
            Tuple<VersionCheckResult, SemVer.Version, string, string> t = await CheckVersionInternal();
            switch (t.Item1)
            {
                case VersionCheckResult.Behind:
                {
                    if (MessageBox.Show($"An update is available!\n\r{ t.Item2.ToString() }\n\r{ t.Item3 }\n\r. Do you want to update now?", "Local version is outdated!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (callUpdateFromVC)
                        {
                            UpdateManager.Update(t.Item4);
                        }
                        else
                        {
                            updateCallback?.Invoke(t.Item4);
                        }
                    }

                    break;
                }

                case VersionCheckResult.Ahead:
                {
                    if (showFineWindows)
                    {
                        MessageBox.Show($"", "Remote version is outdated!");
                    }

                    break;
                }

                case VersionCheckResult.Current:
                {
                    if (showFineWindows)
                    {
                        MessageBox.Show($"The local version corresponds to the latest remote version.", "You are using the latest version.");
                    }

                    break;
                }

                case VersionCheckResult.Error:
                {
                    if (showFineWindows)
                    {
                        MessageBox.Show($"Something went wrong while checking for the version:{ t.Item3 }", "Couldn't check version!");
                    }

                    break;
                }
            }
        }

        public static async Task<Tuple<VersionCheckResult, SemVer.Version, string, string>> CheckVersionInternal()
        {
            try
            {
                Task<VersionSpecV1> t1 = new Task<VersionSpecV1>(GetVersionSpecV1);
                Task<SemVer.Version> t2 = new Task<SemVer.Version>(GetCurrentVersion);
                t1.Start();
                t2.Start();
                VersionSpecV1 spec = await t1;
                SemVer.Version local = await t2;
                SemVer.Version remote = spec.Version;
                string changelog = spec.Changelog.ContainsKey(remote.ToString()) ? spec.Changelog[remote.ToString()] : "No changelog provided";
                string latestLink = $"https://github.com/skyloutyr/VSCC/releases/download/{ remote.ToString() }/VSCC.zip";
                VersionCheckResult result = remote > local ? VersionCheckResult.Behind : remote < local ? VersionCheckResult.Ahead : VersionCheckResult.Current;
                return new Tuple<VersionCheckResult, SemVer.Version, string, string>(result, remote, changelog, latestLink);
            }
            catch (Exception e)
            {
                return new Tuple<VersionCheckResult, SemVer.Version, string, string>(VersionCheckResult.Error, new SemVer.Version(0, 0, 0), $"{ e.Message }:\n{ e.StackTrace }", string.Empty);
            }
        }

        public static SemVer.Version GetCurrentVersion()
        {
            JObject localJObj = JObject.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Version.json")));
            return new SemVer.Version(localJObj["version"].ToObject<string>());
        }

        public static VersionSpecV1 GetVersionSpecV1()
        {
            return JsonConvert.DeserializeObject<VersionSpecV1>(ReadRemoteVersion());
        }

        private static string ReadRemoteVersion()
        {
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
            HttpWebRequest.DefaultCachePolicy = policy;
            HttpWebRequest req = WebRequest.CreateHttp("https://raw.githubusercontent.com/skyloutyr/VSCC/master/VSCC/Version.json");
            req.Timeout = 5000;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            req.CachePolicy = noCachePolicy;
            using (WebResponse response = req.GetResponse())
            {
                using (Stream s = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        string result = sr.ReadToEnd();
                        return result;
                    }
                }
            }
        }
    }

    public enum VersionCheckResult
    { 
        Error,
        Behind,
        Current,
        Ahead
    }
}
