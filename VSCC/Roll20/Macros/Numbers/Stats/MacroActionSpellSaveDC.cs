namespace VSCC.Roll20.Macros.Strings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.State;

    public class MacroActionSpellSaveDC : MacroAction
    {
        public override string Name => this.Translate("Macro_StatsSpellSaveDC_Name");

        public override string Category => this.Translate("Macro_Category_Stats");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_StatsSpellSaveDC_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_StatsSpellSaveDC_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.Spellbook.SpellSaveDC;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }

    public class MacroActionSpellAttackBonus : MacroAction
    {
        public override string Name => this.Translate("Macro_StatsSpellAttackBonus_Name");

        public override string Category => this.Translate("Macro_Category_Stats");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_StatsSpellAttackBonus_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_StatsSpellAttackBonus_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.Spellbook.SpellAttackBonus;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }
}
