namespace VSCC.Roll20.Macros.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using VSCC.Controls.Templates.Macro;
    using VSCC.DataType;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.State;

    public class MacroActionSpell : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[14];

        public override MacroAction[] Params => this._backend;

        public override Type ReturnType => typeof(void);

        public override string Name => this.Translate("Macro_ActionSpell_Name");

        public override Type[] ParamTypes => new Type[] {
            typeof(string),   // Name
            typeof(string),   // School
            typeof(int),      // Level
            typeof(bool),     // IsRitual
            typeof(string),   // CastingTime
            typeof(string),   // Range
            typeof(string),   // Target
            typeof(bool),     // IsVerbal
            typeof(bool),     // IsSomatic
            typeof(bool),     // IsMaterial
            typeof(bool),     // IsConcentration
            typeof(string),   // MaterialComponents
            typeof(string),   // Duration
            typeof(string)    // Description
        };

        public override string Category => this.Translate("Macro_Category_Actions");

        public override string[] CreateFormattedText() => this._backend.Select(s => s.CreateFullInnerText()).ToArray();
        public override string CreateFullInnerText() => this.Translate("Macro_ActionSpell_FullInnerText", this.CreateFormattedText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public bool TryGetSpellLink(Macro m, string name, out Spell s)
        {
            if (m.SpellsLinked.ContainsKey(name))
            {
                Guid gid = m.SpellsLinked[name];
                foreach (Spell sp in AppState.Current.State.Spellbook.AllSpells)
                {
                    if (sp.ObjectID.Equals(gid))
                    {
                        s = sp;
                        return true;
                    }
                }
            }

            s = null;
            return false;
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
            this.Params[9] = MacroSerializer.ReadMacroAction(br);
            this.Params[10] = MacroSerializer.ReadMacroAction(br);
            this.Params[11] = MacroSerializer.ReadMacroAction(br);
            this.Params[12] = MacroSerializer.ReadMacroAction(br);
            this.Params[13] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors)
        {
            if (!R20WSServer.Connected)
            {
                errors.Add(this.Translate("Macro_Error_NoServer"));
                return null;
            }

            if (this.TryGetSpellLink(m, (string)this.Params[0].Execute(m, errors), out _))
            {
                R20WSServer.Send(new CommandPacket
                {
                    Template = Template.Spell,
                    Data = new TemplateDataSpell
                    {
                        Name = this.Params[0].Execute(m, errors).ToString(),
                        SchoolLevel = $"{ this.Params[1].Execute(m, errors) } { this.Params[2].Execute(m, errors) }",
                        CastingTime = this.Params[4].Execute(m, errors).ToString(),
                        Range = this.Params[5].Execute(m, errors).ToString(),
                        Target = this.Params[6].Execute(m, errors).ToString(),
                        Verbal = (bool)this.Params[7].Execute(m, errors) ? "1" : "0",
                        Somatic = (bool)this.Params[8].Execute(m, errors) ? "1" : "0",
                        Material = (bool)this.Params[9].Execute(m, errors) ? "1" : "0",
                        Duration = this.Params[12].Execute(m, errors).ToString(),
                        Desc = this.Params[13].Execute(m, errors).ToString(),
                        Ritual = (bool)this.Params[3].Execute(m, errors) ? "1" : "0",
                        Concentration = (bool)this.Params[10].Execute(m, errors) ? "1" : "0",
                        MaterialComponents = this.Params[11].Execute(m, errors).ToString(),
                        CharName = AppState.Current.State.General.Name
                    }
                });
            }

            errors.Add(this.Translate("Macro_Error_NoLink"));
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
            MacroSerializer.WriteMacroAction(bw, this.Params[9]);
            MacroSerializer.WriteMacroAction(bw, this.Params[10]);
            MacroSerializer.WriteMacroAction(bw, this.Params[11]);
            MacroSerializer.WriteMacroAction(bw, this.Params[12]);
            MacroSerializer.WriteMacroAction(bw, this.Params[13]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[1] = new MacroActionStringConstant();
            this.Params[2] = new MacroActionNumberConstant();
            this.Params[3] = new MacroActionBoolConstant();
            this.Params[4] = new MacroActionStringConstant();
            this.Params[5] = new MacroActionStringConstant();
            this.Params[6] = new MacroActionStringConstant();
            this.Params[7] = new MacroActionBoolConstant();
            this.Params[8] = new MacroActionBoolConstant();
            this.Params[9] = new MacroActionBoolConstant();
            this.Params[10] = new MacroActionBoolConstant();
            this.Params[11] = new MacroActionStringConstant();
            this.Params[12] = new MacroActionStringConstant();
            this.Params[13] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
            this.Params[2].SetDefaults();
            this.Params[3].SetDefaults();
            this.Params[4].SetDefaults();
            this.Params[5].SetDefaults();
            this.Params[6].SetDefaults();
            this.Params[7].SetDefaults();
            this.Params[8].SetDefaults();
            this.Params[9].SetDefaults();
            this.Params[10].SetDefaults();
            this.Params[11].SetDefaults();
            this.Params[12].SetDefaults();
            this.Params[13].SetDefaults();
            ((MacroActionStringConstant)this.Params[0]).SetValue("name");
            ((MacroActionStringConstant)this.Params[1]).SetValue("school");
            ((MacroActionStringConstant)this.Params[4]).SetValue("casting time");
            ((MacroActionStringConstant)this.Params[5]).SetValue("range");
            ((MacroActionStringConstant)this.Params[6]).SetValue("target");
            ((MacroActionStringConstant)this.Params[11]).SetValue("material components");
            ((MacroActionStringConstant)this.Params[12]).SetValue("duration");
            ((MacroActionStringConstant)this.Params[13]).SetValue("description");
        }

        public override bool CreateCustomView(Grid grid)
        {
            TemplateSpell ts = new TemplateSpell();
            ts.HLName.Tag = 0;
            ts.HLSchool.Tag = 1;
            ts.HLLevel.Tag = 2;
            ts.HLIsRitual.Tag = 3;
            ts.HLCastingTime.Tag = 4;
            ts.HLRange.Tag = 5;
            ts.HLTarget.Tag = 6;
            ts.HLVerbal.Tag = 7;
            ts.HLSomatic.Tag = 8;
            ts.HLMaterial.Tag = 9;
            ts.HLConcentration.Tag = 10;
            ts.HLMatComponents.Tag = 11;
            ts.HLDuration.Tag = 12;
            ts.HLDesc.Tag = 13;
            grid.Children.Add(ts);
            Grid.SetColumn(ts, 1);
            Grid.SetRow(ts, 1);
            return true;
        }
    }
}
