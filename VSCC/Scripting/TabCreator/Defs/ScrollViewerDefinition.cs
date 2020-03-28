namespace VSCC.Scripting.TabCreator.Defs
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Windows.Controls;

    public class ScrollViewerDefinition
    {

        [JsonConverter(typeof(StringEnumConverter))]
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PanningMode PanningMode { get; set; }
    }
}
