namespace VSCC.Roll20.Macros.Convert
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.Roll20.Macros.Expressions;

    public class MacroActionExp2String : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_ExpExp2Str_Name");

        public override string Category => this.Translate("Macro_Category_Conversion");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(bool) };

        public override Type ReturnType => typeof(string);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };
        public override string CreateFullInnerText() => this.Translate("Macro_ExpExp2Str_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpExp2Str_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors)
        {
            string s = ((Expression)this.Params[0].Execute(m, errors)).ToString();
            if ((bool)this.Params[1].Execute(m, errors))
            {
                s = $"[[{ s }]]";
            }

            return s;
        }

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this._backend[0] = new MacroActionExpressionRoll();
            this._backend[1] = new MacroActionBoolConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }
}
