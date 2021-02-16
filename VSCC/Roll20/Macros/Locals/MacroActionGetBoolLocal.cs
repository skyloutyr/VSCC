namespace VSCC.Roll20.Macros.Locals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionGetBoolLocal : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override string Name => this.Translate("Macro_LocalGetB_Name");

        public override string Category => this.Translate("Macro_Category_Locals");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(bool);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText() };
        public override string CreateFullInnerText() => this.Translate("Macro_LocalGetB_FullInnerText", this.Params[0].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_LocalGetB_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 0 };
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            string name = this.Params[0].Execute(m, errors).ToString();
            if (m.BoolLocals.ContainsKey(name))
            {
                return m.BoolLocals[name].Item1;
            }

            errors.Add(this.Translate("Macro_Error_NoLocal"));
            return false;
        }

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
        }
    }
}
