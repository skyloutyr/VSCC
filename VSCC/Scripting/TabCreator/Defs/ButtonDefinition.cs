using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCC.Scripting.TabCreator.Defs
{
    public class ButtonDefinition
    {
        public string Text { get; set; }
        public bool Enabled { get; set; }
        public FontDefinition Font { get; set; }
    }
}
