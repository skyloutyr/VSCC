namespace VSCC.Roll20.Macros.Numbers.Skills
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.State;

    public class MacroActionSleightOfHand : MacroAction
    {
        public override string Name => this.Translate("Macro_SkillsSleightOfHand_Name");

        public override string Category => this.Translate("Macro_Category_Skills");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_SkillsSleightOfHand_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_SkillsSleightOfHand_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.SleightOfHand;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }

    public class MacroActionHasPSleightOfHand : MacroAction
    {
        public override string Name => this.Translate("Macro_SkillsHasPSleightOfHand_Name");

        public override string Category => this.Translate("Macro_Category_Skills");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(bool);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_SkillsHasPSleightOfHand_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_SkillsHasPSleightOfHand_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.ProfficientAtSleightOfHand;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }
}
