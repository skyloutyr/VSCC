namespace VSCC.Roll20.Macros.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.Roll20.Macros.Convert;

    public class MacroActionExpressionAdvRoll : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_ExpAdvRoll_Name");

        public override string Category => this.Translate("Macro_Category_Expressions");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(Expression) };

        public override Type ReturnType => typeof(Expression);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ExpAdvRoll_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_ExpAdvRoll_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpAdvRoll_Text_1"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => new Expression($"[[{ this.Params[0].Execute(m, errors) }]]d[[{ this.Params[1].Execute(m, errors) }]]");

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionString2Exp();
            this.Params[1] = new MacroActionString2Exp();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
            ((MacroActionStringConstant)this.Params[0].Params[0]).SetValue("1");
            ((MacroActionStringConstant)this.Params[1].Params[0]).SetValue("20");
        }
    }
}
