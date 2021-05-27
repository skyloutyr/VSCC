namespace VSCC.Scripting.Marketplace
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VSCC.VersionManager;

    public class MarketplaceEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string ProjectID { get; set; }

        [JsonProperty(PropertyName = "format_version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty(PropertyName = "long_description")]
        public string FullDescription { get; set; }

        [JsonProperty(PropertyName = "version")]
        [JsonConverter(typeof(SemVerConverter))]
        public SemanticVersioning.Version LatestVersion { get; set; }

        [JsonProperty(PropertyName = "accepted_app_version")]
        [JsonConverter(typeof(SemVerRangeConverter))]
        public SemanticVersioning.Range AppRange { get; set; }

        [JsonProperty(PropertyName = "changelog")]
        public Dictionary<string, string> Changelog { get; set; }

        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "authors")]
        public string[] Authors { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "checksum")]
        public string MD5 { get; set; }

        [JsonProperty(PropertyName = "flags", ItemConverterType = typeof(StringEnumConverter))]
        public MarketplaceEntryFlag[] Flags { get; set; }

        [JsonIgnore]
        public string LocalPath { get; set; }

        [JsonIgnore]
        public string LocalScriptPath { get; set; }

        [JsonIgnore]
        public bool IsLocal { get; set; }

        [JsonIgnore]
        public bool HasUpdate { get; set; }

        [JsonIgnore]
        public bool DownloadButtonEnabled { get; set; }

        [JsonIgnore]
        public bool DeleteButtonEnabled => this.IsLocal;

        [JsonIgnore]
        public string AuthorsProperty => string.Join(", ", this.Authors);

        [JsonIgnore]
        public string TagsProperty => string.Join(", ", this.Tags);

        [JsonIgnore]
        public string FlagsProperty => string.Join(", ", this.Flags.Select(p => Enum.GetName(typeof(MarketplaceEntryFlag), p)));

        [JsonIgnore]
        public string DeleteTooltip => this.IsLocal ? "Delete" ?? "Delete" : "Can only delete local scripts!";

        [JsonIgnore]
        public string UpdateTooltip => this.IsLocal ? this.HasUpdate ? "Download" : "Using latest version" : "Download";

        [JsonIgnore]
        public string VersionProperty => this.LatestVersion.ToString();

        [JsonIgnore]
        public bool HasNoMetadata { get; set; }

        public MarketplaceEntry()
        {
        }
    }

    public enum MarketplaceEntryFlag
    {
        Recommended,
        Example,
        Official,
        KnownIssues,
        UpdateAvailable,
        LocalScriptOnly,
        VersionIncompatible,
        Outdated
    }
}
