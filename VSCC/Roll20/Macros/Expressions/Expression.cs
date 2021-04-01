namespace VSCC.Roll20.Macros.Expressions
{
    public readonly struct Expression
    {
        public string InnerText { get; }

        public Expression(string innerText) => this.InnerText = innerText;

        public override string ToString() => this.InnerText;
    }
}
