﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows;

namespace VSCC.Scripting.TabCreator.Defs
{
    public class GridDefinition
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
