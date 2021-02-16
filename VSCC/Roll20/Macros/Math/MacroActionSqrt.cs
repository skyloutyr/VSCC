namespace VSCC.Roll20.Macros.Math
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionSqrt : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override string Name => this.Translate("Macro_MathSqrt_Name");

        public override string Category => this.Translate("Macro_Category_Math");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(float) };

        public override Type ReturnType => typeof(float);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText() };
        public override string CreateFullInnerText() => this.Translate("Macro_MathSqrt_FullInnerText", this.Params[0].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_MathSqrt_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 0 };
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);
        public override object Execute(Macro m, List<string> errors) => (float)Math.Sqrt((float)this.Params[0].Execute(m, errors));

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);
        public override void SetDefaults()
        {
            this._backend[0] = new MacroActionRealConstant();
            this.Params[0].SetDefaults();
        }
    }
}
