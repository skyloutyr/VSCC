namespace VSCC.Roll20.AdvancedIntegration
{
    using Newtonsoft.Json;

    public class DamageLine
    {
        public int NumDice { get; set; }
        public int DieSide { get; set; }
        public string Label { get; set; }
        public int ConstantNumber { get; set; }

        // WPF Bindings
        [JsonIgnore]
        public string DisplayDice =>
            this.NumDice > 0 ?
                this.ConstantNumber > 0 ?
                    $"{this.NumDice}d{this.DieSide} + {this.ConstantNumber}" :
                this.ConstantNumber < 0 ?
                     $"{this.NumDice}d{this.DieSide} - {-this.ConstantNumber}" :
                $"{this.NumDice}d{this.DieSide}" :
             $"{this.ConstantNumber}";

        public string DisplayDesc => this.Label;

        public DamageLine Copy() => new DamageLine
        {
            NumDice = this.NumDice,
            DieSide = this.DieSide,
            Label = this.Label,
            ConstantNumber = this.ConstantNumber
        };
    }
}
