namespace VSCC.Roll20.AdvancedIntegration
{
    public class ScalableDamageLine
    {
        public ScalableDie Die { get; set; }
        public string Label { get; set; }
        public ScalableValue ConstantNumber { get; set; }

        // WPF Bindings
        public string DisplayDice =>
            this.Die.NumDice.Value > 0 ?
                this.ConstantNumber.Value > 0 ?
                    $"{this.Die.NumDice.Value}d{this.Die.DieSide.Value} + {this.ConstantNumber.Value}" :
                this.ConstantNumber.Value < 0 ?
                     $"{this.Die.NumDice.Value}d{this.Die.DieSide.Value} - {-this.ConstantNumber.Value}" :
                $"{this.Die.NumDice.Value}d{this.Die.DieSide.Value}" :
             $"{this.ConstantNumber.Value}";

        public string DisplayDesc => this.Label;

        public ScalableDamageLine Copy() => new ScalableDamageLine
        {
            Die = this.Die.Copy(),
            Label = this.Label,
            ConstantNumber = this.ConstantNumber.Copy()
        };
    }
}
