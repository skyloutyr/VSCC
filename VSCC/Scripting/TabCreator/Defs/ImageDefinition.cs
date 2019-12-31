using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VSCC.Scripting.TabCreator.Defs
{
    public class ImageDefinition
    {
        public string Source { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Stretch StretchMode { get; set; }
    }
}
