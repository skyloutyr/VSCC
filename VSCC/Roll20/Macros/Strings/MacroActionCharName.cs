namespace VSCC.Roll20.Macros.Strings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.State;

    public class MacroActionCharName : MacroAction
    {
        public override string Name => this.Translate("Macro_StringsGetCharName_Name");

        public override string Category => this.Translate("Macro_Category_Strings");

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(string);

        public override string[] CreateFormattedText() => new string[0];
        public override string CreateFullInnerText() => this.Translate("Macro_StringsGetCharName_FullInnerText");
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Run(this.Translate("Macro_StringsGetCharName_Text_0"));
        }

        public override void Deserialize(BinaryReader br)
        {
        }

        public override object Execute(Macro m, List<string> errors) => AppState.Current.State.General.Name;
        public override void Serialize(BinaryWriter bw)
        {
        }

        public override void SetDefaults()
        {
        }
    }
}
