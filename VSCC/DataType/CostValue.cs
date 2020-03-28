using Newtonsoft.Json;

namespace VSCC.DataType
{
    public class CostValue
    {
        private int _cost;

        public int CP
        {
            get => this._cost % 10;
            set => this._cost = value;
        }

        public int SP
        {
            get => (this._cost / 10) % 10;
            set => this._cost = value * 10;
        }

        public int GP
        {
            get => (this._cost / 100);
            set => this._cost = value * 100;
        }

        [JsonIgnore]
        public int Total { get => this._cost; set => this._cost = value; }

        public CostValue(int gp, int sp, int cp) => this._cost = gp * 100 + sp * 10 + cp;

        public CostValue Copy() => new CostValue(this.GP, this.SP, this.CP);
    }
}
