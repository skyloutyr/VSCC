using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSCC.VersionManager
{
    public class VersionSpecV1
    {
        [JsonProperty(PropertyName = "version")]
        [JsonConverter(typeof(SemVerConverter))]
        public SemVer.Version Version { get; set; }

        [JsonProperty(PropertyName = "changelog")]
        public Dictionary<string, string> Changelog { get; set; } = new Dictionary<string, string>();
    }
}
