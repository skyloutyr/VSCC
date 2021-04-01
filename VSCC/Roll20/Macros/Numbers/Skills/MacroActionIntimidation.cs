namespace VSCC.Roll20.Macros.Numbers.Skills
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.State;

    public class MacroActionIntimidation : MacroAction
    {
        public override string Name => this.Translate("Macro_SkillsIntimidation_Name");

        public override string Category => this.Translate("Macro_Category_Skills");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_SkillsIntimidation_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_SkillsIntimidation_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.Intimidation;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }

    public class MacroActionHasPIntimidation : MacroAction
    {
        public override string Name => this.Translate("Macro_SkillsHasPIntimidation_Name");

        public override string Category => this.Translate("Macro_Category_Skills");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(bool);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_SkillsHasPIntimidation_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_SkillsHasPIntimidation_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.ProfficientAtIntimidation;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }
}
