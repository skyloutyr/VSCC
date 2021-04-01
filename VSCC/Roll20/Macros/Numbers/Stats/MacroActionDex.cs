namespace VSCC.Roll20.Macros.Strings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.State;

    public class MacroActionDex : MacroAction
    {
        public override string Name => this.Translate("Macro_StatsDex_Name");

        public override string Category => this.Translate("Macro_Category_Stats");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_StatsDex_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_StatsDex_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.StatDex;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }

    public class MacroActionModDex : MacroAction
    {
        public override string Name => this.Translate("Macro_StatsModDex_Name");

        public override string Category => this.Translate("Macro_Category_Stats");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_StatsModDex_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_StatsModDex_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.StatModDex;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }

    public class MacroActionSaveDex : MacroAction
    {
        public override string Name => this.Translate("Macro_StatsSaveDex_Name");

        public override string Category => this.Translate("Macro_Category_Stats");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[0];

        public override string CreateFullInnerText() => this.Translate("Macro_StatsSaveDex_FullInnerText");

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_StatsSaveDex_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.DexSave;

        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }
}
