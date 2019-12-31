using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Windows;
using VSCC.Scripting.TabCreator.Defs;

namespace VSCC.Scripting.TabCreator
{
    public class UIDefinition
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Alignment Alignment { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UIType Type { get; set; }
        public Size Size { get; set; }
        public Margin Margin { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
        public RowColumnPositions RowColumnPositions { get; set; }
        public List<UIDefinition> Children { get; set; } = new List<UIDefinition>();
        public Dictionary<string, Action<object, string>> EventHandlers { get; set; } = new Dictionary<string, Action<object, string>>();
        public GridDefinition GridData { get; set; }
        public PanelDefinition CommonPanelData { get; set; }
        public GroupBoxDefinition GroupBoxData { get; set; }
        public BorderDefinition BorderData { get; set; }
        public LabelDefinition LabelData { get; set; }
        public ButtonDefinition ButtonData { get; set; }
        public TextBoxDefinition TextBoxData { get; set; }
        public ImageDefinition ImageData { get; set; }
        public CheckBoxDefinition CheckBoxData { get; set; }
        public RadioButtonDefinition RadioButtonData { get; set; }
        public ScrollViewerDefinition ScrollViewerData { get; set; }
        public NumericUpDownDefinition NumericUpDownData { get; set; }
    }

    public enum Alignment
    {
        TopLeft         = 0b10001000,
        TopCenter       = 0b10000100,
        TopRight        = 0b10000010,
        TopStretch      = 0b10000001,
        CenterLeft      = 0b01001000,
        CenterCenter    = 0b01000100,
        CenterRight     = 0b01000010,
        CenterStretch   = 0b01000001,
        BottomLeft      = 0b00101000,
        BottomCenter    = 0b00100100,
        BottomRight     = 0b00100010,
        BottomStretch   = 0b00100001,
        StretchLeft     = 0b00011000,
        StretchCenter   = 0b00010100,
        SretchRight     = 0b00010010,
        StretchAll      = 0b00010001
    }

    public enum UIType
    {
        Grid,
        WrapPanel,
        StackPanel,
        GroupBox,
        Border,
        Label,
        Button,
        TextBox,
        Image,
        CheckBox,
        RadioButton,
        ScrollViewer,
        IntUpDown,
        FloatUpDown
    }

    public class Rect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }

    public class Margin
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
    }

    public class RowColumnPositions
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class FontDefinition
    {
        public int Size { get; set; }
        public FontStyle Style { get; set; }
    }

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
