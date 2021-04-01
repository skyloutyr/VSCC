namespace VSCC.Roll20.Macros.Basic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;

    public class MacroActionStringConstant : MacroAction
    {
        private string _s = "text";

        public override MacroAction[] Params => new MacroAction[0];

        public override Type ReturnType => typeof(string);

        public override string Name => this.Translate("Macro_BasicStrCon_Name");

        public override Type[] ParamTypes => new Type[0];

        public override string Category => this.Translate("Macro_Category_Basic");

        public override string[] CreateFormattedText() => new[] { this._s };

        public override string CreateFullInnerText() => $"{ this._s }";

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br) => this._s = br.ReadString();

        public override object Execute(Macro m, List<string> errors) => this._s;

        public override void Serialize(BinaryWriter bw) => bw.Write(this._s);

        public override void SetDefaults() => this._s = "text";

        public virtual void SetValue(string s) => this._s = string.IsNullOrEmpty(s) ? " " : s;

        public virtual string GetValue() => this._s;
    }
}
