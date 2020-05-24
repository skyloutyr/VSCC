namespace VSCC.Scripting.Marketplace
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using VSCC.VersionManager;

    public class MarketplaceIndex
    {
        [JsonConverter(typeof(SemVerConverter))]
        [JsonProperty(PropertyName = "version")]
        public SemVer.Version Version { get; set; }

        [JsonProperty(PropertyName = "changelog")]
        public Dictionary<string, string> Changelog { get; set; }

        [JsonProperty(PropertyName = "scripts")]
        public string[] ScriptsDB { get; set; }
    }
}
