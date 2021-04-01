namespace VSCC.Roll20.Macros
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using VSCC.Properties;

    public abstract class MacroAction
    {
        public static Dictionary<Type, List<Tuple<Type, string, string, bool>>> Actions { get; } = new Dictionary<Type, List<Tuple<Type, string, string, bool>>>();

        static MacroAction()
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {
                    if (!t.IsAbstract && typeof(MacroAction).IsAssignableFrom(t))
                    {
                        MacroAction ma = (MacroAction)Activator.CreateInstance(t);
                        Type k = ma.ReturnType;
                        if (!Actions.ContainsKey(k))
                        {
                            Actions[k] = new List<Tuple<Type, string, string, bool>>();
                        }

                        Actions[k].Add(new Tuple<Type, string, string, bool>(t, ma.Name, ma.Category, ma.IsQueryable));
                    }
                }
            }
        }

        public abstract string Name { get; }
        public abstract string Category { get; }
        public abstract MacroAction[] Params { get; }
        public abstract Type[] ParamTypes { get; }
        public virtual bool IsQueryable => true;

        public abstract IEnumerable<Inline> CreateInnerText();

        public abstract Type ReturnType { get; }
        public abstract object Execute(Macro m, List<string> errors);
        public abstract void SetDefaults();
        public abstract string[] CreateFormattedText();
        public abstract string CreateFullInnerText();
        public abstract void Serialize(BinaryWriter bw);
        public abstract void Deserialize(BinaryReader br);

        public virtual string Translate(string key, params object[] pars) => string.Format(Resources.ResourceManager.GetString(key), pars);

        public virtual bool CreateCustomView(Grid grid) => false;

        public Regex GroupRX = new Regex(@"(?<=\[\[)([^\[\[].*?)(?=\]\])", RegexOptions.Compiled);
        public virtual string Expressionify(string exp)
        {
            string ec = exp;
            while (true)
            {
                MatchCollection mc = this.GroupRX.Matches(ec);
                if (mc.Count == 0) // No matches
                {
                    if (string.IsNullOrEmpty(ec)) // String empty too
                    {
                        return exp; // The expression was already properly escaped
                    }
                    else
                    {
                        return "[[" + exp + "]]"; // Ran out of matches but still had stuff in expression, nonescaped. Escape it.
                    }
                }
                else // Have matches.
                {
                    string eec = ec;
                    int i = 0;
                    foreach (Match m in mc)
                    {
                        bool hasInner = m.Value.Contains("["); // Check for 'annotated' capures which are not captured properly by regex
                        eec = ec.Remove(
                            m.Index - 2 - i, // offset by 2 to capture the start of the token ([[) and by i to account for other removals
                            m.Length + 4 + (hasInner ? 1 : 0) // offset by 2 to account for index offset by 2 and by 2 extra to capture the end of the token (]])
                        );

                        i += m.Length + 4 + (hasInner ? 1 : 0); // Offset the i to account for removals
                    }

                    ec = eec;
                }
            }
        }
    }
}
