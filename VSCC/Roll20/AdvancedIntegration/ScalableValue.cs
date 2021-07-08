namespace VSCC.Roll20.AdvancedIntegration
{
    using Newtonsoft.Json;

    public class ScalableValue
    {
        public int Value { get; set; }
        public int ValuePerLevel { get; set; }

        public bool EnableCustomScaling { get; set; }
        public int ValueLvl1 { get; set; }
        public int ValueLvl2 { get; set; }
        public int ValueLvl3 { get; set; }
        public int ValueLvl4 { get; set; }
        public int ValueLvl5 { get; set; }
        public int ValueLvl6 { get; set; }
        public int ValueLvl7 { get; set; }
        public int ValueLvl8 { get; set; }
        public int ValueLvl9 { get; set; }

        [JsonIgnore]
        public string TextLabel => this.Value.ToString();

        public ScalableValue Copy() => new ScalableValue
        {
            Value = this.Value,
            ValueLvl1 = this.ValueLvl1,
            ValueLvl2 = this.ValueLvl2,
            ValueLvl3 = this.ValueLvl3,
            ValueLvl4 = this.ValueLvl4,
            ValueLvl5 = this.ValueLvl5,
            ValueLvl6 = this.ValueLvl6,
            ValueLvl7 = this.ValueLvl7,
            ValueLvl8 = this.ValueLvl8,
            ValueLvl9 = this.ValueLvl9,
            ValuePerLevel = this.ValuePerLevel,
            EnableCustomScaling = this.EnableCustomScaling
        };

        public int GetForLevel(int l, int bl)
        {
            if (this.EnableCustomScaling)
            {
                switch (l)
                {
                    default:
                        {
                            return this.ValueLvl1;
                        }

                    case 2:
                        {
                            return this.ValueLvl2;
                        }

                    case 3:
                        {
                            return this.ValueLvl3;
                        }

                    case 4:
                        {
                            return this.ValueLvl4;
                        }

                    case 5:
                        {
                            return this.ValueLvl5;
                        }

                    case 6:
                        {
                            return this.ValueLvl6;
                        }

                    case 7:
                        {
                            return this.ValueLvl7;
                        }

                    case 8:
                        {
                            return this.ValueLvl8;
                        }

                    case 9:
                        {
                            return this.ValueLvl9;
                        }
                }
            }
            else
            {
                return this.Value + this.ValuePerLevel * (l - bl);
            }
        }
    }
}
