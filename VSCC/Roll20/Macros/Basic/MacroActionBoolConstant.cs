namespace VSCC.Roll20.Macros.Basic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;

    public class MacroActionBoolConstant : MacroAction
    {
        private bool _b = false;

        public override MacroAction[] Params => new MacroAction[0];

        public override Type ReturnType => typeof(bool);

        public override string Name => this.Translate("Macro_BasicBoolCon_Name");

        public override Type[] ParamTypes => new Type[0];

        public override string Category => this.Translate("Macro_Category_Basic");

        public override string[] CreateFormattedText() => new[] { this._b.ToString() };

        public override string CreateFullInnerText() => $"{ this._b }";

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br) => this._b = br.ReadBoolean();

        public override object Execute(Macro m, List<string> errors) => this._b;

        public override void Serialize(BinaryWriter bw) => bw.Write(this._b);

        public override void SetDefaults() => this._b = false;

        public virtual void SetValue(bool b) => this._b = b;

        public virtual bool GetValue() => this._b;
    }
}
