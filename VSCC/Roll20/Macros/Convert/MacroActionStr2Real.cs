namespace VSCC.Roll20.Macros.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionStr2Real : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override string Name => this.Translate("Macro_ConvStr2Real_Name");

        public override string Category => this.Translate("Macro_Category_Conversion");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(float);

        public override string[] CreateFormattedText() => new string[] { $"{ this.Params[0].CreateFullInnerText() }" };

        public override string CreateFullInnerText() => this.Translate("Macro_ConvStr2Real_FullInnerText", this.Params[0].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ConvStr2Real_Text_0"));
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            if (float.TryParse(this._backend[0].Execute(m, errors).ToString(), out float i))
            {
                return i;
            }

            errors.Add(this.Translate("Macro_Error_CantParseFloat"));
            return 0;
        }

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);

        public override void SetDefaults()
        {
            this._backend[0] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
            ((MacroActionStringConstant)this.Params[0]).SetValue("0");
        }
    }
}
