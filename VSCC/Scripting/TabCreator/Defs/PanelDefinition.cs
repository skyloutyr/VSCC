using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Controls;

namespace VSCC.Scripting.TabCreator.Defs
{
    public class PanelDefinition
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation Orientation { get; set; }
    }
}
