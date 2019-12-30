using System;
using System.Collections.Generic;
using System.Drawing;

namespace VSCC.Scripting.TabCreator
{
    public class UIDefinition
    {
        public string Name { get; set; }
        public Alignment Alignment { get; set; }

        public UIType Type { get; set; }
        public Rect Location { get; set; }
        public Margin Margin { get; set; }
        public Margin Padding { get; set; }
        public List<UIDefinition> Children { get; set; } = new List<UIDefinition>();
        public Dictionary<string, object> TypeSpecificData { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, Action<object, string>> EventHandlers { get; set; } = new Dictionary<string, Action<object, string>>();
    }

    public enum Alignment
    {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        CenterCenter,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        TopStretch,
        CenterStretch,
        BottomStretch,
        StretchLeft,
        StretchCenter,
        SretchRight,
        StretchAll
    }

    public enum UIType
    {
        Grid,
        WrapPanel,
        StackPanel,
        Label,
        Button,
        TextField,
        RichTextField,
        Image,
        CheckBox,
        RadioBox,
        ScrollViewer,
        IntUpDown,
        FloatUpDown,
        GroupBox
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
}
