using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using VSCC.Models.ImageList;
using VSCC.Templates;

namespace VSCC.DataType
{
    public class InventoryItem : INotifyPropertyChanged
    {
        [JsonIgnore]
        private string imageIndex;

        [JsonIgnore]
        private string description;

        [JsonIgnore]
        private string rarity;

        [JsonIgnore]
        private string type;

        [JsonIgnore]
        private CostValue cost = new CostValue(0, 0, 0);

        [JsonIgnore]
        private float weight;

        [JsonIgnore]
        private int amount;

        [JsonIgnore]
        private string name;

        [JsonIgnore]
        public ImageListModel ImageList { get; set; }

        public ItemTemplate Template { get; set; }

        [JsonIgnore]
        public BitmapImage PictureProperty => this.ImageList[this.ImageIndex]?.Image ?? null;

        [JsonIgnore]
        public string AmountProperty => $"#:{ this.Amount }";

        [JsonIgnore]
        public string TotalWeightProperty => $"⚖:{ this.Amount * this.Weight }";

        [JsonIgnore]
        public string TotalCostProperty => $"$:{ this.Cost.GP }.{ this.Cost.SP }.{ this.Cost.CP }";

        [JsonIgnore]
        public int CostSortProperty => this.Cost.Total;

        [JsonIgnore]
        public int CostTotalortProperty => this.Cost.Total * this.Amount;

        [JsonIgnore]
        public float WeightTotalortProperty => this.Weight * this.Amount;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                this.OnPropertyChanged("Name");
            }
        }
        public int Amount
        {
            get => amount;
            set
            {
                amount = value;
                this.OnPropertyChanged("Amount");
                this.OnPropertyChanged("AmountProperty");
                this.OnPropertyChanged("TotalWeightProperty");
            }
        }
        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                this.OnPropertyChanged("Weight");
                this.OnPropertyChanged("TotalWeightProperty");
            }
        }
        public CostValue Cost
        {
            get => cost;
            set
            {
                cost = value;
                this.OnPropertyChanged("TotalCostProperty");
            }
        }
        public string Type
        {
            get => type;
            set
            {
                type = value;
                this.OnPropertyChanged("Type");
            }
        }
        public string Rarity
        {
            get => rarity;
            set
            {
                rarity = value;
                this.OnPropertyChanged("Rarity");
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                this.OnPropertyChanged("Description");
            }
        }
        public string ImageIndex
        {
            get => imageIndex;
            set
            {
                imageIndex = value;
                this.OnPropertyChanged("ImageIndex");
                this.OnPropertyChanged("PictureProperty");
            }
        }

        [JsonIgnore]
        public string GeneratedDescription => $"{ this.Name }\nAmount: { this.Amount }\nWeight: { this.Weight * this.Amount }({ this.Weight })\nCost:{ this.Cost }\nType:{ this.Type }\nRarity: { this.Rarity }\n\n{ this.Description }";

        public event PropertyChangedEventHandler PropertyChanged;

        public InventoryItem()
        {
        }

        public InventoryItem(ItemTemplate template) : base()
        {
            this.Template = template;
            this.Name = template.Name;
            this.Amount = 1;
            if (float.TryParse(template.Weight, out float w))
            {
                this.Weight = w;
            }

            this.Cost = new CostValue(0, 0, 0);
            this.DetectType();
            this.Rarity = template.ItemRarity;
            this.Description = template.DescriptionProperty;
        }

        private void DetectType()
        {

        }

        private void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public InventoryItem Copy() =>
            new InventoryItem()
            {
                ImageList = this.ImageList,
                ImageIndex = this.ImageIndex,
                Template = this.Template,
                Name = this.Name,
                Amount = this.Amount,
                Weight = this.Weight,
                Cost = this.Cost.Copy(),
                Type = this.Type,
                Rarity = this.Rarity,
                Description = this.Description
            };
    }

    public class InventoryItemLegacyAdapter
    {
        public static bool CanApply(JObject itemJson)
        {
            if (itemJson.ContainsKey("Cost") && itemJson["Cost"].Type == JTokenType.Integer)
            {
                return true;
            }

            return false;
        }

        public static InventoryItem Apply(JObject obj)
        {
            return new InventoryItem()
            {
                Name = obj.Value<string>("Name"),
                Amount = obj.Value<int>("Amount"),
                Weight = (float)obj.Value<int>("Weight"),
                Cost = new CostValue(obj.Value<int>("Cost"), 0, 0),
                Type = obj.Value<string>("Type"),
                Rarity = obj.Value<string>("Rarity"),
                Description = obj.Value<string>("Description"),
                ImageIndex = "",
                Template = ItemTemplate.Empty
            };
        }
    }
}
