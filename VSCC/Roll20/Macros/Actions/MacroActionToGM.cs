﻿namespace VSCC.Roll20.Macros.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.State;

    public class MacroActionToGM : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override string Name => this.Translate("Macro_ActionToGM_Name");

        public override string Category => this.Translate("Macro_Category_Actions");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(bool) };

        public override Type ReturnType => typeof(void);

        public override string[] CreateFormattedText() => new string[] { $"{ this.Params[0].CreateFullInnerText() }" };
        public override string CreateFullInnerText() => this.Translate("Macro_ActionToGM_FullInnerText", this.Params[0].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            Hyperlink hl = new Hyperlink(new Run("text"))
            {
                Tag = 0
            };

            yield return new Run(this.Translate("Macro_ActionToGM_Text"));
            yield return hl;
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            AppState.Current.TRoll20.MacroToGMMode = (bool)this.Params[0].Execute(m, errors);
            return null;
        }

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionBoolConstant();
            this.Params[0].SetDefaults();
        }
    }
}
