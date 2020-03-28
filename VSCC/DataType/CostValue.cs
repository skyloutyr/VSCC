namespace VSCC.DataType
{
    using Newtonsoft.Json;

    public class CostValue
    {
        public int CP
        {
            get => this.Total % 10;
            set => this.Total = value;
        }

        public int SP
        {
            get => this.Total / 10 % 10;
            set => this.Total = value * 10;
        }

        public int GP
        {
            get => (this.Total / 100);
            set => this.Total = value * 100;
        }

        [JsonIgnore]
        public int Total { get; set; }

        public CostValue(int gp, int sp, int cp) => this.Total = (gp * 100) + (sp * 10) + cp;

        public CostValue Copy() => new CostValue(this.GP, this.SP, this.CP);
    }
}
