namespace VSCC.Roll20.Macros.Locals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionSetNextStringLocal : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_LocalSetNextS_Name");

        public override string Category => this.Translate("Macro_Category_Locals");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string), typeof(string) };

        public override Type ReturnType => typeof(void);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_LocalSetNextS_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_LocalSetNextS_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_LocalSetNextS_Text_1"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors)
        {
            Macro.InbetweenMacroActions.Add((macro, errs) =>
            {
                string name = this.Params[0].Execute(macro, errs).ToString();
                if (macro.StringLocals.ContainsKey(name))
                {
                    macro.StringLocals[name] = new Tuple<string, string>((string)this.Params[1].Execute(macro, errs), macro.StringLocals[name].Item2);
                }
            });

            return null;
        }

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[1] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }
}
