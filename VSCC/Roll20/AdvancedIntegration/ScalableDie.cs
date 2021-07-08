namespace VSCC.Roll20.AdvancedIntegration
{
    using Newtonsoft.Json;

    public class ScalableDie
    {
        public ScalableValue NumDice { get; set; } = new ScalableValue();
        public ScalableValue DieSide { get; set; } = new ScalableValue();

        [JsonIgnore]
        public string TextLabel => $"{this.NumDice}d{this.DieSide}";

        public ScalableDie Copy() => new ScalableDie
        {
            NumDice = this.NumDice.Copy(),
            DieSide = this.DieSide.Copy()
        };
    }
}
