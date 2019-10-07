﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VSCC.Roll20
{
    public class RollPacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Command;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "template")]
        public Template Template { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }

    public enum Template
    {
        Default,
        Description,
        Simple
    }

    public class TemplateDataDefault
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class TemplateDataSimple
    {
        [JsonProperty(PropertyName = "r1")]
        public string R1 { get; set; }

        [JsonProperty(PropertyName = "r2")]
        public string R2 { get; set; }

        [JsonProperty(PropertyName = "rname")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "mod")]
        public string Mod { get; set; }

        [JsonProperty(PropertyName = "charname")]
        public string CharName { get; set; }
    }

    public enum PacketType
    {
        Close,
        Message,
        Roll,
        Command
    }
}
