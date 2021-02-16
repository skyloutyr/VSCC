namespace VSCC.Roll20.Macros.Basic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;

    public class MacroActionNumberConstant : MacroAction
    {
        private int _i = 0;

        public override MacroAction[] Params => new MacroAction[0];

        public override Type ReturnType => typeof(int);

        public override string Name => this.Translate("Macro_BasicIntCon_Name");

        public override Type[] ParamTypes => new Type[0];

        public override string Category => this.Translate("Macro_Category_Basic");

        public override string[] CreateFormattedText() => new[] { this._i.ToString() };
        public override string CreateFullInnerText() => $"{ this._i }";

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br) => this._i = br.ReadInt32();
        public override object Execute(Macro m, List<string> errors) => this._i;
        public override void Serialize(BinaryWriter bw) => bw.Write(this._i);
        public override void SetDefaults() => this._i = 0;

        public virtual void SetValue(int i) => this._i = i;
        public virtual int GetValue() => this._i;
    }
}
