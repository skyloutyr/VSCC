namespace VSCC.Roll20.Macros.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.DataType;
    using VSCC.Roll20.Macros.Basic;
    using VSCC.State;

    public class MacroActionInferredSpell : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[1];

        public override MacroAction[] Params => this._backend;

        public override Type ReturnType => typeof(void);

        public override string Name => this.Translate("Macro_ActionInferredSpell_Name");

        public override Type[] ParamTypes => new Type[] { typeof(string) };

        public override string Category => this.Translate("Macro_Category_Actions");

        public override string[] CreateFormattedText() => new string[] { $"{ this.Params[0].CreateFullInnerText() }" };

        public override string CreateFullInnerText() => this.Translate("Macro_ActionInferredSpell_FullInnerText", this.Params[0].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            Hyperlink hl = new Hyperlink(new Run("text"))
            {
                Tag = 0
            };

            yield return new Run(this.Translate("Macro_ActionInferredSpell_Text_0"));
            yield return hl;
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

        public override void Deserialize(BinaryReader br) => this.Params[0] = MacroSerializer.ReadMacroAction(br);

        public override object Execute(Macro m, List<string> errors)
        {
            if (!R20WSServer.Connected)
            {
                errors.Add(this.Translate("Macro_Error_NoServer"));
                return null;
            }

            if (this.TryGetSpellLink(m, (string)this.Params[0].Execute(m, errors), out Spell s))
            {
                R20WSServer.Send(new CommandPacket
                {
                    Template = Template.Spell,
                    GMRoll = AppState.Current.TRoll20.MacroToGMMode,
                    Data = new TemplateDataSpell
                    {
                        Name = s.Name,
                        SchoolLevel = $"{ s.School } { s.Level }",
                        CastingTime = s.CastTime,
                        Range = s.Range,
                        Target = s.Target,
                        Verbal = s.PropertyVerbal ? "1" : "0",
                        Somatic = s.PropertySomatic ? "1" : "0",
                        Material = s.PropertyMaterial ? "1" : "0",
                        Duration = s.Duration,
                        Desc = s.Description,
                        Ritual = s.PropertyRitual ? "1" : "0",
                        Concentration = s.PropertyConcentration ? "1" : "0",
                        CharName = AppState.Current.State.General.Name
                    }
                });
            }

            errors.Add(this.Translate("Macro_Error_NoLink"));
            return null;
        }

        public override void Serialize(BinaryWriter bw) => MacroSerializer.WriteMacroAction(bw, this.Params[0]);

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionStringConstant();
            this.Params[0].SetDefaults();
        }
    }
}
