namespace VSCC.Roll20.Macros.Expressions.Math
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros.Basic;

    public class MacroActionExpressionDiv : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_ExpDiv_Name");

        public override string Category => this.Translate("Macro_Category_ExpressionMath");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(Expression) };

        public override Type ReturnType => typeof(Expression);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ExpDiv_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpDiv_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => new Expression($"{ this.Params[0].Execute(m, errors) } / { this.Params[1].Execute(m, errors) }");

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionExpressionRoll();
            this.Params[1] = new MacroActionExpressionRoll();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }

    public class MacroActionExpressionSimpleDiv : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_ExpSDiv_Name");

        public override string Category => this.Translate("Macro_Category_ExpressionMath");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(int) };

        public override Type ReturnType => typeof(Expression);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ExpDiv_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpDiv_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => new Expression($"{ this.Params[0].Execute(m, errors) } / { this.Params[1].Execute(m, errors) }");

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionExpressionRoll();
            this.Params[1] = new MacroActionNumberConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }

    public class MacroActionExpressionHPDiv : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_ExpHPDiv_Name");

        public override string Category => this.Translate("Macro_Category_ExpressionMath");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(Expression) };

        public override Type ReturnType => typeof(Expression);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ExpHPDiv_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpHPDiv_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
            yield return new Run(this.Translate("Macro_ExpHPDiv_Text_1"));
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => new Expression($"[[{ this.Params[0].Execute(m, errors) } / { this.Params[1].Execute(m, errors) }]]");

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionExpressionRoll();
            this.Params[1] = new MacroActionExpressionRoll();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }

    public class MacroActionExpressionSimpleHPDiv : MacroAction
    {
        private readonly MacroAction[] _backend = new MacroAction[2];

        public override string Name => this.Translate("Macro_ExpSHPDiv_Name");

        public override string Category => this.Translate("Macro_Category_ExpressionMath");

        public override MacroAction[] Params => this._backend;

        public override Type[] ParamTypes => new Type[] { typeof(Expression), typeof(int) };

        public override Type ReturnType => typeof(Expression);

        public override string[] CreateFormattedText() => new string[] { this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText() };

        public override string CreateFullInnerText() => this.Translate("Macro_ExpHPDiv_FullInnerText", this.Params[0].CreateFullInnerText(), this.Params[1].CreateFullInnerText());

        public override IEnumerable<Inline> CreateInnerText()
        {
            yield return new Hyperlink(new Run()) { Tag = 0 };
            yield return new Run(this.Translate("Macro_ExpHPDiv_Text_0"));
            yield return new Hyperlink(new Run()) { Tag = 1 };
            yield return new Run(this.Translate("Macro_ExpHPDiv_Text_1"));
        }

        public override void Deserialize(BinaryReader br)
        {
            this.Params[0] = MacroSerializer.ReadMacroAction(br);
            this.Params[1] = MacroSerializer.ReadMacroAction(br);
        }

        public override object Execute(Macro m, List<string> errors) => new Expression($"[[{ this.Params[0].Execute(m, errors) } / { this.Params[1].Execute(m, errors) }]]");

        public override void Serialize(BinaryWriter bw)
        {
            MacroSerializer.WriteMacroAction(bw, this.Params[0]);
            MacroSerializer.WriteMacroAction(bw, this.Params[1]);
        }

        public override void SetDefaults()
        {
            this.Params[0] = new MacroActionExpressionRoll();
            this.Params[1] = new MacroActionNumberConstant();
            this.Params[0].SetDefaults();
            this.Params[1].SetDefaults();
        }
    }
}
