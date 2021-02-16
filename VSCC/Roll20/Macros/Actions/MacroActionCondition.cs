namespace VSCC.Roll20.Macros.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;

    public class MacroActionCondition : MacroAction
    {
        public LinkedList<MacroAction> If { get; } = new LinkedList<MacroAction>();
        public LinkedList<MacroAction> Then { get; } = new LinkedList<MacroAction>();
        public LinkedList<MacroAction> Else { get; } = new LinkedList<MacroAction>();

        public override string Name => string.Empty;

        public override string Category => string.Empty;

        public override MacroAction[] Params => new MacroAction[0];

        public override Type[] ParamTypes => new Type[0];

        public override Type ReturnType => typeof(void);
        public override bool IsQueryable => false;

        public override string[] CreateFormattedText() => new string[0];
        public override string CreateFullInnerText() => string.Empty;
        public override IEnumerable<Inline> CreateInnerText()
        {
            yield break;
        }

        public override void Deserialize(BinaryReader br)
        {
            int i = br.ReadInt32();
            this.If.Clear();
            while (i-- > 0)
            {
                this.If.AddLast(MacroSerializer.ReadMacroAction(br));
            }

            i = br.ReadInt32();
            this.Then.Clear();
            while (i-- > 0)
            {
                this.Then.AddLast(MacroSerializer.ReadMacroAction(br));
            }

            i = br.ReadInt32();
            this.Else.Clear();
            while (i-- > 0)
            {
                this.Else.AddLast(MacroSerializer.ReadMacroAction(br));
            }
        }

        public override object Execute(Macro m, List<string> errors)
        {
            bool b = true;
            if (this.If.Count == 0)
            {
                errors.Add(this.Translate("Macro_Error_NoConditions"));
            }

            foreach (MacroAction ma in this.If)
            {
                b &= (bool)ma.Execute(m, errors);
            }

            if (b)
            {
                foreach (MacroAction ma in this.Then)
                {
                    ma.Execute(m, errors);
                }
            }
            else
            {
                foreach (MacroAction ma in this.Else)
                {
                    ma.Execute(m, errors);
                }
            }

            return null;
        }

        public override void Serialize(BinaryWriter bw)
        {
            bw.Write(this.If.Count);
            foreach (MacroAction ma in this.If)
            {
                MacroSerializer.WriteMacroAction(bw, ma);
            }

            bw.Write(this.Then.Count);
            foreach (MacroAction ma in this.Then)
            {
                MacroSerializer.WriteMacroAction(bw, ma);
            }

            bw.Write(this.Else.Count);
            foreach (MacroAction ma in this.Else)
            {
                MacroSerializer.WriteMacroAction(bw, ma);
            }
        }

        public override void SetDefaults()
        {
        }
    }
}
