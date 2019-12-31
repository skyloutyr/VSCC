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
    public class PanelDefinition
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation Orientation { get; set; }
    }
}
