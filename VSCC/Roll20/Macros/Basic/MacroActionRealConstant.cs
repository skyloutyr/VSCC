namespace VSCC.Roll20.Macros.Basic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;

    public class MacroActionRealConstant : MacroAction
    {
        private float _f = 0;

        public override MacroAction[] Params => new MacroAction[0];

        public override Type ReturnType => typeof(float);

        public override string Name => this.Translate("Macro_BasicRealCon_Name");

        public override Type[] ParamTypes => new Type[0];

        public override string Category => this.Translate("Macro_Category_Basic");

        public override string[] CreateFormattedText() => new[] { this._f.ToString() };
        public override string CreateFullInnerText() => $"{ this._f }";

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br) => this._f = br.ReadSingle();
        public override object Execute(Macro m, List<string> errors) => this._f;
        public override void Serialize(BinaryWriter bw) => bw.Write(this._f);
        public override void SetDefaults() => this._f = 0;

        public virtual void SetValue(float f) => this._f = f;
        public virtual float GetValue() => this._f;
    }
}
