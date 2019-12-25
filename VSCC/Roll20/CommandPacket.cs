using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VSCC.Roll20
{
    public class CommandPacket
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

    public class MessagePacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Message;

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

    public class RollPacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Roll;

        [JsonProperty(PropertyName = "numDice")]
        public int NumDice { get; set; }

        [JsonProperty(PropertyName = "numSides")]
        public int NumSides { get; set; }
    }

    public class ClosePacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Close;
    }

    public enum Template
    {
        Default,
        Description,
        Simple,
        None,
        Custom
    }

    public class TemplateDataDefault
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class TemplateDataManyRolls
    {
        [JsonProperty(PropertyName = "roll")]
        public string Roll { get; set; }
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
