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

namespace VSCC.VersionManager
{
    public class VersionChecker
    {
        public static async Task<Tuple<VersionCheckResult, SemVer.Version, string, string>> CheckVersion()
        {
            try
            {
                Task<VersionSpecV1> t1 = new Task<VersionSpecV1>(GetVersionSpecV1);
                Task<SemVer.Version> t2 = new Task<SemVer.Version>(GetCurrentVersion);
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
