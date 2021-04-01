﻿namespace VSCC.Roll20.Macros.ItemLinks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.DataType;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionILCostCopper : MacroActionItemLinkBase
    {
        private readonly MacroAction[] _backend = new MacroAction[1];
        public override string Name => this.Translate("Macro_ILCostCopper");

        public override string Category => this.Translate("Macro_Category_IL");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ILCostCopper_FullInnerText", this.Params[0].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ILCostCopper_Text_0"));
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            string n = (string)this.Params[0].Execute(m, errors);
            if (this.TryGetItemLink(m, n, out InventoryItem ii))
            {
                return ii.Cost.CP;
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

    public class MacroActionILCostTotalCopper : MacroActionItemLinkBase
    {
        private readonly MacroAction[] _backend = new MacroAction[1];
        public override string Name => this.Translate("Macro_ILCostTotalCopper");

        public override string Category => this.Translate("Macro_Category_IL");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ILCostTotalCopper_FullInnerText", this.Params[0].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_ILCostTotalCopper_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ILCostTotalCopper_Text_1"));
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            string n = (string)this.Params[0].Execute(m, errors);
            if (this.TryGetItemLink(m, n, out InventoryItem ii))
            {
                return ii.Cost.Total;
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
