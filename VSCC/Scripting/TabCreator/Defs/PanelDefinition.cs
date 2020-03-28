namespace VSCC.Scripting.TabCreator.Defs
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Windows.Controls;

    public class PanelDefinition
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation Orientation { get; set; }
    }
}
