namespace VSCC.Roll20.Macros
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    public class MacroSerializer
    {
        public static ObservableCollection<Macro> Macros { get; } = new ObservableCollection<Macro>();

        public static void WriteMacroAction(BinaryWriter bw, MacroAction ma)
        {
            bw.Write(ma.GetType().FullName);
            ma.Serialize(bw);
        }

        public static MacroAction ReadMacroAction(BinaryReader br)
        {
            Type t = Type.GetType(br.ReadString());
            MacroAction ma = (MacroAction)Activator.CreateInstance(t);
            ma.Deserialize(br);
            return ma;
        }

        public static byte[] WriteMacroAction(MacroAction ma)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, System.Text.Encoding.UTF8))
                {
                    bw.Write(ma.GetType().FullName);
                    ma.Serialize(bw);
                }

                return ms.ToArray();
            }
        }

        public static T ReadMacroAction<T>(byte[] b) where T : MacroAction
        {
            using (MemoryStream ms = new MemoryStream(b))
            {
                using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.UTF8))
                {
                    Type t = Type.GetType(br.ReadString());
                    MacroAction ma = (MacroAction)Activator.CreateInstance(t);
                    ma.Deserialize(br);
                    return (T)ma;
                }
            }
        }

        public static void ReadMacroAction(MacroAction ma, byte[] arr)
        {
            using (MemoryStream ms = new MemoryStream(arr))
            {
                using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.UTF8))
                {
                    br.ReadString(); // Type is always serialized, read it
                    ma.Deserialize(br);
                }
            }
        }

        public static void LoadAll(string s)
        {
            Macros.Clear();
            if (!string.IsNullOrEmpty(s))
            {
                byte[] b = System.Convert.FromBase64String(s);
                using (MemoryStream ms = new MemoryStream(b))
                {
                    using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.UTF8))
                    {
                        int i = br.ReadInt32();
                        while (i-- > 0)
                        {
                            Macro m = new Macro();
                            m.Deserialize(br);
                            Macros.Add(m);
                        }
                    }
                }
            }
        }

        public static string SaveAll()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, System.Text.Encoding.UTF8))
                {
                    bw.Write(Macros.Count);
                    foreach (Macro m in Macros)
                    {
                        m.Serialize(bw);
                    }
                }

                return System.Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string Save(Macro m)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, System.Text.Encoding.UTF8))
                {
                    m.Serialize(bw);
                }

                return System.Convert.ToBase64String(ms.ToArray());
            }
        }

        public static Macro Load(string s)
        {
            byte[] b = System.Convert.FromBase64String(s);
            using (MemoryStream ms = new MemoryStream(b))
            {
                using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.UTF8))
                {
                    Macro m = new Macro();
                    m.Deserialize(br);
                    return m;
                }
            }
        }
    }
}
