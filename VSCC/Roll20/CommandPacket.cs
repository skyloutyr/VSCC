namespace VSCC.Roll20
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

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

        [JsonProperty(PropertyName = "gmr")]
        public bool GMRoll { get; set; }
    }

    public class MessagePacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Message;

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "gmr")]
        public bool GMRoll { get; set; }
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

        [JsonProperty(PropertyName = "gmr")]
        public bool GMRoll { get; set; }
    }

    public class ClosePacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Close;
    }

    public class PollPacket
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public PacketType Type { get; } = PacketType.Poll;
    }

    public enum Template
    {
        Default,
        Description,
        Simple,
        AtkDmg,
        Dmg,
        Spell,
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

        [JsonProperty(PropertyName = "gmr")]
        public bool GMRoll { get; set; }
    }

    public class TemplateDataDesc
    {
        [JsonProperty(PropertyName = "desc")]
        public string Desc { get; set; }
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

    public class TemplateDataAtkDmg
    {
        [JsonProperty(PropertyName = "mod")]
        public string Mod { get; set; }

        [JsonProperty(PropertyName = "rname")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "r1")]
        public string R1 { get; set; }

        [JsonProperty(PropertyName = "r2")]
        public string R2 { get; set; }

        [JsonProperty(PropertyName = "range")]
        public string Range { get; set; }

        [JsonProperty(PropertyName = "dmg1")]
        public string Dmg { get; set; }

        [JsonProperty(PropertyName = "dmg1type")]
        public string DmgType { get; set; }

        [JsonProperty(PropertyName = "crit1")]
        public string Crit { get; set; }

        [JsonProperty(PropertyName = "charname")]
        public string CharName { get; set; }
    }

    public class TemplateDataSaveDmg
    {
        [JsonProperty(PropertyName = "rname")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "range")]
        public string Range { get; set; }

        [JsonProperty(PropertyName = "dmg1")]
        public string Dmg { get; set; }

        [JsonProperty(PropertyName = "dmg1type")]
        public string DmgType { get; set; }

        [JsonProperty(PropertyName = "crit1")]
        public string Crit { get; set; }

        [JsonProperty(PropertyName = "saveattr")]
        public string SaveAttr { get; set; }

        [JsonProperty(PropertyName = "savedesc")]
        public string SaveDesc { get; set; }

        [JsonProperty(PropertyName = "savedc")]
        public string SaveDC { get; set; }

        [JsonProperty(PropertyName = "charname")]
        public string CharName { get; set; }
    }

    public class TemplateDataAtkSaveDmg
    {
        [JsonProperty(PropertyName = "mod")]
        public string Mod { get; set; }

        [JsonProperty(PropertyName = "rname")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "r1")]
        public string R1 { get; set; }

        [JsonProperty(PropertyName = "r2")]
        public string R2 { get; set; }

        [JsonProperty(PropertyName = "range")]
        public string Range { get; set; }

        [JsonProperty(PropertyName = "dmg1")]
        public string Dmg { get; set; }

        [JsonProperty(PropertyName = "dmg1type")]
        public string DmgType { get; set; }

        [JsonProperty(PropertyName = "crit1")]
        public string Crit { get; set; }

        [JsonProperty(PropertyName = "charname")]
        public string CharName { get; set; }

        [JsonProperty(PropertyName = "saveattr")]
        public string SaveAttr { get; set; }

        [JsonProperty(PropertyName = "savedesc")]
        public string SaveDesc { get; set; }

        [JsonProperty(PropertyName = "savedc")]
        public string SaveDC { get; set; }
    }

    public class TemplateDataDmg
    {

        [JsonProperty(PropertyName = "rname")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "dmg1")]
        public string Dmg1 { get; set; }

        [JsonProperty(PropertyName = "dmg2")]
        public string Dmg2 { get; set; }

        [JsonProperty(PropertyName = "range")]
        public string Range { get; set; }

        [JsonProperty(PropertyName = "dmg1type")]
        public string Dmg1Type { get; set; }

        [JsonProperty(PropertyName = "dmg2type")]
        public string Dmg2Type { get; set; }

        [JsonProperty(PropertyName = "charname")]
        public string CharName { get; set; }
    }

    public class TemplateDataSpell
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "level")]
        public string SchoolLevel { get; set; }

        [JsonProperty(PropertyName = "castingtime")]
        public string CastingTime { get; set; }

        [JsonProperty(PropertyName = "range")]
        public string Range { get; set; }

        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        [JsonProperty(PropertyName = "v")]
        public string Verbal { get; set; }

        [JsonProperty(PropertyName = "s")]
        public string Somatic { get; set; }

        [JsonProperty(PropertyName = "m")]
        public string Material { get; set; }

        [JsonProperty(PropertyName = "material")]
        public string MaterialComponents { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Desc { get; set; }

        [JsonProperty(PropertyName = "ritual")]
        public string Ritual { get; set; }

        [JsonProperty(PropertyName = "concentration")]
        public string Concentration { get; set; }

        [JsonProperty(PropertyName = "charname")]
        public string CharName { get; set; }
    }

    public enum PacketType
    {
        Close,
        Message,
        Roll,
        Poll,
        Command
    }
}
