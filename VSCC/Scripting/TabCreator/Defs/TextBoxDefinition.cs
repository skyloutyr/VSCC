﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows;
using System.Windows.Controls;

namespace VSCC.Scripting.TabCreator.Defs
{
    public class TextBoxDefinition
    {
        public string Text { get; set; }
        public FontDefinition Font { get; set; }
        public bool IsReadOnly { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TextWrapping WrapMode { get; set; }
        public BorderDefinition Border { get; set; }
    }
}
