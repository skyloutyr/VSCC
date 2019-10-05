using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VSCC.Templates
{
    public class ItemTemplate
    {
        public const string NewLine = "\n\r";

        public static ItemTemplate Empty => new ItemTemplate()
        {
            Name = string.Empty,
            Description = string.Empty,
            Subtitle = string.Empty,
            Category = string.Empty,
            Damage = string.Empty,
            DamageType = string.Empty,
            Properties = string.Empty,
            Range = string.Empty,
            Weight = string.Empty,
            Save = string.Empty,
            ItemRarity = string.Empty,
            AC = string.Empty,
            Stealth = string.Empty
        };

        public string Name { get; set; }
        public string Description { get; set; }
        public string Subtitle { get; set; }
        public string Category { get; set; }
        public string Damage { get; set; }
        public string DamageType { get; set; }
        public string Properties { get; set; }
        public string Range { get; set; }
        public string Weight { get; set; }
        public string Save { get; set; }
        public string ItemRarity { get; set; }
        public string AC { get; set; }
        public string Stealth { get; set; }

        public string DamageProperty
        {
            get
            {
                if (string.IsNullOrEmpty(this.Damage))
                {
                    return "Damage: N/A";
                }

                if (this.Damage.IndexOf('d') == -1)
                {
                    return $"Damage: { this.Damage } { this.DamageType }";
                }

                string[] d = this.Damage.Split('d');
                int numDie = int.Parse(d[0]);
                int dieSides = int.Parse(d[1]);
                return $"Damage: { this.Damage } ({ numDie } - { numDie * dieSides }) { this.DamageType }";
            }
        }

        public string ACProperty => string.IsNullOrEmpty(this.AC) ? "AC: N/A" : "AC: " + this.AC;
        public string RangeProperty => string.IsNullOrEmpty(this.Range) ? "Range: N/A" : "Range: " + this.Range;
        public string WeightProperty => string.IsNullOrEmpty(this.Weight) ? "Weight: 0" : "Weight: " + this.Weight;
        public string RarityProperty => string.IsNullOrEmpty(this.ItemRarity) ? "Rarity: Common" : "Rarity: "+ this.ItemRarity;
        public string PropertiesProperty
        {
            get
            {
                if (string.IsNullOrEmpty(this.Properties))
                {
                    return "None";
                }

                string ret = "";
                string[] props = this.Properties.Split(',');
                foreach (string s in props)
                {
                    ret += "• " + s.Trim() + NewLine;
                }

                return ret;
            }
        }

        public string DescriptionProperty => this.Subtitle + NewLine + (string.IsNullOrEmpty(this.Save) ? "" : ("Saving throws: " + this.Save + NewLine)) + "Properties:" + NewLine + this.PropertiesProperty + NewLine + this.Description;
    }
}
