using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VSCC.Scripting.TabCreator.Defs
{
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
