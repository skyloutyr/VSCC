using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VSCC.Scripting.TabCreator.Defs
{
    public class GridDefinition : UIDefinition
    {
        public GridColumnDefinition[] Columns { get; set; }
        public GridRowDefinition[] Rows { get; set; }
        public bool ShowGridLines { get; set; }
    }

    public class GridColumnDefinition
    {
        public double Width { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GridUnitType UnitType { get; set; }
    }

    public class GridRowDefinition
    {
        public double Height { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GridUnitType UnitType { get; set; }
    }
}
