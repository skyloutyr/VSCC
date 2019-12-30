using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCC.Scripting.TabCreator
{
    public class UIGenerator
    {
        public LuaTable FromFile(string path) => FromJSON(File.ReadAllText(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path))));

        public LuaTable FromJSON(string json)
        {
            return null;
        }
    }
}
