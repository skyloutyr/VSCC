namespace VSCC.Roll20.Macros.Basic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Documents;

    public class MacroActionStringConcat : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_BasicStrConcat_Name");

        public override string Category => this.Translate("Macro_Category_Basic");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string), typeof(string) };

        public override Type ReturnType => typeof(string);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };
        public override string CreateFullInnerText() => this.Translate("Macro_BasicStrConcat_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_BasicStrConcat_Text_0"));
            yield return new Hyperlink(new Run("text_0")) { Tag = 0, TextDecorations = TextDecorations.Underline };
            yield return new Run(this.Translate("Macro_BasicStrConcat_Text_1"));
            yield return new Hyperlink(new Run("text_1")) { Tag = 1, TextDecorations = TextDecorations.Underline };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => string.Concat(this.Params[0].Execute(m, errors), this.Params[1].Execute(m, errors));
        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[1] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }
}
