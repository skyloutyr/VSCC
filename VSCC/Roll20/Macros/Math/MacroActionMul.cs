namespace VSCC.Roll20.Macros.Expressions.Math
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionMul : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_MathMul_Name");

        public override string Category => this.Translate("Macro_Category_Math");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(int), typeof(int) };

        public override Type ReturnType => typeof(int);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_MathMul_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_MathMul_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => (int)this.Params[0].Execute(m, errors) * (int)this.Params[1].Execute(m, errors);

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionNumberConstant();
            this.Params[1] = new MacroActionNumberConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }

    public class MacroActionMulFF : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_MathMulFF_Name");

        public override string Category => this.Translate("Macro_Category_Math");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(float), typeof(float) };

        public override Type ReturnType => typeof(float);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_MathMul_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_MathMul_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => (float)this.Params[0].Execute(m, errors) * (float)this.Params[1].Execute(m, errors);

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionRealConstant();
            this.Params[1] = new MacroActionRealConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }

    public class MacroActionMulFN : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_MathMulFN_Name");

        public override string Category => this.Translate("Macro_Category_Math");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(float), typeof(int) };

        public override Type ReturnType => typeof(float);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_MathMul_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_MathMul_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => (float)this.Params[0].Execute(m, errors) * (int)this.Params[1].Execute(m, errors);

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionRealConstant();
            this.Params[1] = new MacroActionNumberConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }

    public class MacroActionMulNF : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_MathMulNF_Name");

        public override string Category => this.Translate("Macro_Category_Math");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(int), typeof(float) };

        public override Type ReturnType => typeof(float);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_MathMul_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_MathMul_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => (int)this.Params[0].Execute(m, errors) * (float)this.Params[1].Execute(m, errors);

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionNumberConstant();
            this.Params[1] = new MacroActionRealConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }
}
