﻿namespace VSCC.Roll20.Macros.Convert
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.Roll20.Macros.Expressions;

    public class MacroActionString2Exp : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override string Name => this.Translate("Macro_ExpStr2Exp_Name");

        public override string Category => this.Translate("Macro_Category_Conversion");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(Expression);

        public override string[] CreateFormattedText() => new string[] { $"{ this.Params[0].CreateFullInnerText() }" };
        public override string CreateFullInnerText() => this.Translate("Macro_ExpStr2Exp_FullInnerText", this.Params[0].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpStr2Exp_Text_0"));
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);
        public override object Execute(Macro m, List<string> errors) => new Expression(this._backend[0].Execute(m, errors).ToString());
        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);
        public override void SetDefaults()
        {
            this._backend[0] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
        }
    }
}
