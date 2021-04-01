namespace VSCC.Roll20.Macros.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using VSCC.Controls.Templates.Macro;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionShowDescription : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override string Name => this.Translate("Macro_ActionDesc_Name");

        public override string Category => this.Translate("Macro_Category_Actions");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override Type ReturnType => typeof(void);

        public override string[] CreateFormattedText() => new string[] { $"{ this.Params[0].CreateFullInnerText() }" };

        public override string CreateFullInnerText() => this.Translate("Macro_ActionDesc_FullInnerText", this.Params[0].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            if (!R20WSServer.Connected)
            {
                errors.Add(this.Translate("Macro_Error_NoServer"));
                return null;
            }

            R20WSServer.Send(new CommandPacket() { Template = Template.Description, Data = new TemplateDataDesc { Desc = this.Params[0].Execute(m, errors).ToString() } });
            return null;
        }

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
        }

        public override bool CreateCustomView(Grid grid)
        {
            TemplateDesc td = new TemplateDesc();
            td.HL0.Tag = 0;
            grid.Children.Add(td);
            Grid.SetColumn(td, 1);
            Grid.SetRow(td, 1);
            return true;
        }
    }
}
