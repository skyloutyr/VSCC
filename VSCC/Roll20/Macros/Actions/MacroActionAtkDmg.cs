namespace VSCC.Roll20.Macros.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using VSCC.Controls.Templates.Macro;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.Roll20.Macros.Expressions;
    using VSCC.State;

    public class MacroActionAtkDmg : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[9];

        public override string Name => this.Translate("Macro_ActionAtkDmg_Name");

        public override string Category => this.Translate("Macro_Category_Actions");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(Expression), typeof(string), typeof(string), typeof(string), typeof(string), typeof(Expression), typeof(Expression), typeof(string) };

        public override Type ReturnType => typeof(void);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText(), this.Params[2].CreateFullInnerText(), this.Params[3].CreateFullInnerText(), this.Params[4].CreateFullInnerText(), this.Params[5].CreateFullInnerText(), this.Params[6].CreateFullInnerText(), this.Params[7].CreateFullInnerText(), this.Params[8].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ActionAtkDmg_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText(), this.Params[2].CreateFullInnerText(), this.Params[3].CreateFullInnerText(), this.Params[4].CreateFullInnerText(), this.Params[5].CreateFullInnerText(), this.Params[6].CreateFullInnerText(), this.Params[7].CreateFullInnerText(), this.Params[8].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
            this.Params[2] = MacroSerializer.ReadMacroAction(br);
            this.Params[3] = MacroSerializer.ReadMacroAction(br);
            this.Params[4] = MacroSerializer.ReadMacroAction(br);
            this.Params[5] = MacroSerializer.ReadMacroAction(br);
            this.Params[6] = MacroSerializer.ReadMacroAction(br);
            this.Params[7] = MacroSerializer.ReadMacroAction(br);
            this.Params[8] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors)
        {
            if (!R20WSServer.Connected)
            {
                errors.Add(this.Translate("Macro_Error_NoServer"));
                return null;
            }

            R20WSServer.Send(new CommandPacket()
            {
                Template = Template.AtkDmg,
                GMRoll = AppState.Current.TRoll20.MacroToGMMode,
                Data = new TemplateDataAtkDmg
                {
                    R1 = this.Expressionify(this.Params[0].Execute(m, errors).ToString()),
                    R2 = this.Expressionify(this.Params[1].Execute(m, errors).ToString()),
                    Range = this.Params[2].Execute(m, errors).ToString(),
                    Name = this.Params[3].Execute(m, errors).ToString(),
                    Mod = this.Params[4].Execute(m, errors).ToString(),
                    CharName = this.Params[5].Execute(m, errors).ToString(),
                    Dmg = this.Expressionify(this.Params[6].Execute(m, errors).ToString()),
                    Crit = this.Expressionify(this.Params[7].Execute(m, errors).ToString()),
                    DmgType = this.Params[8].Execute(m, errors).ToString(),
                }
            });

            return null;
        }

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
            MacroSerializer.WriteMacroAction(bw, this.Params[2]);
            MacroSerializer.WriteMacroAction(bw, this.Params[3]);
            MacroSerializer.WriteMacroAction(bw, this.Params[4]);
            MacroSerializer.WriteMacroAction(bw, this.Params[5]);
            MacroSerializer.WriteMacroAction(bw, this.Params[6]);
            MacroSerializer.WriteMacroAction(bw, this.Params[7]);
            MacroSerializer.WriteMacroAction(bw, this.Params[8]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionExpressionRoll();
            this.Params[1] = new MacroActionExpressionRoll();
            this.Params[2] = new MacroActionStringConstant();
            this.Params[3] = new MacroActionStringConstant();
            this.Params[4] = new MacroActionStringConstant();
            this.Params[5] = new MacroActionStringConstant();
            this.Params[6] = new MacroActionExpressionRoll();
            this.Params[7] = new MacroActionExpressionRoll();
            this.Params[8] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
            this.Params[2].SetDefaults();
            this.Params[3].SetDefaults();
            this.Params[4].SetDefaults();
            this.Params[5].SetDefaults();
            this.Params[6].SetDefaults();
            this.Params[7].SetDefaults();
            this.Params[8].SetDefaults();
            ((MacroActionStringConstant)this.Params[2]).SetValue("desc");
            ((MacroActionStringConstant)this.Params[3]).SetValue("name");
            ((MacroActionStringConstant)this.Params[4]).SetValue("mod");
            ((MacroActionStringConstant)this.Params[5]).SetValue("character name");
            ((MacroActionStringConstant)this.Params[8]).SetValue("dmg type");
        }

        public override bool CreateCustomView(Grid grid)
        {
            TemplateAtkDmg ts = new TemplateAtkDmg();
            ts.HR1.Tag = 0;
            ts.HR2.Tag = 1;
            ts.HLRange.Tag = 2;
            ts.HLRname.Tag = 3;
            ts.HLMod.Tag = 4;
            ts.HLCharname.Tag = 5;
            ts.HLDmg.Tag = 6;
            ts.HLCrit.Tag = 7;
            ts.HLDmgType.Tag = 8;
            grid.Children.Add(ts);
            Grid.SetColumn(ts, 1);
            Grid.SetRow(ts, 1);
            return true;
        }
    }
}
