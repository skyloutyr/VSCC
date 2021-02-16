namespace VSCC.Roll20.Macros.ItemLinks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.DataType;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionILName : MacroActionItemLinkBase
    {
        private readonly MacroAction[] _backend = new MacroAction[1];
        public override string Name => this.Translate("Macro_ILName");

        public override string Category => this.Translate("Macro_Category_IL");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(string);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText() };
        public override string CreateFullInnerText() => this.Translate("Macro_ILName_FullInnerText", this.Params[0].CreateFullInnerText());
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_ILName_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 0 };
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            string n = (string)this.Params[0].Execute(m, errors);
            if (this.TryGetItemLink(m, n, out InventoryItem ii))
            {
                return ii.Name;
            }

            errors.Add(this.Translate("Macro_Error_NoLink"));
            return string.Empty;
        }

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
        }
    }
}
