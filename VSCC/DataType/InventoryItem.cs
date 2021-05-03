namespace VSCC.DataType
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using VSCC.Models.ImageList;
    using VSCC.State;
    using VSCC.Templates;

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
        private uint color;

        [JsonIgnore]
        public ImageListModel ImageList { get; set; }


        [JsonIgnore]
        public BitmapImage PictureProperty => this.imageIndex == null ? null : this.ImageList[this.ImageIndex].Image;

        [JsonIgnore]
        public string AmountProperty => $"{ this.Amount }";

        [JsonIgnore]
        public string TotalWeightProperty => $"⚖:{ this.Amount * this.Weight }";

        [JsonIgnore]
        public string TotalCostProperty => $"$:{ this.Cost.GP }.{ this.Cost.SP }.{ this.Cost.CP }";

        [JsonIgnore]
        public int CostSortProperty => this.Cost.Total;

        [JsonIgnore]
        public int CostTotalortProperty => this.Cost.Total * this.Amount;

        [JsonIgnore]
        public float WeightTotalSortProperty => this.Weight * this.Amount;

        [JsonIgnore]
        public Brush Color
        {
            get
            {
                if (this.color == 0)
                {
                    return (SolidColorBrush)AppState.Current.Window.TryFindResource("Static.Foreground");
                }

                byte a = (byte)((this.color >> 24) & 0xFF);
                byte r = (byte)((this.color >> 16) & 0xFF);
                byte g = (byte)((this.color >> 8) & 0xFF);
                byte b = (byte)((this.color) & 0xFF);
                Color c = System.Windows.Media.Color.FromArgb(a, r, g, b);
                return new SolidColorBrush(c);
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }
        public int Amount
        {
            get => this.amount;
            set
            {
                this.amount = value;
                this.OnPropertyChanged("Amount");
                this.OnPropertyChanged("AmountProperty");
                this.OnPropertyChanged("TotalWeightProperty");
                this.OnPropertyChanged("WeightTotalSortProperty");
            }
        }
        public float Weight
        {
            get => this.weight;
            set
            {
                this.weight = value;
                this.OnPropertyChanged("Weight");
                this.OnPropertyChanged("TotalWeightProperty");
                this.OnPropertyChanged("WeightTotalSortProperty");
            }
        }
        public CostValue Cost
        {
            get => this.cost;
            set
            {
                this.cost = value;
                this.OnPropertyChanged("TotalCostProperty");
            }
        }
        public string Type
        {
            get => this.type;
            set
            {
                this.type = value;
                this.OnPropertyChanged("Type");
            }
        }
        public string Rarity
        {
            get => this.rarity;
            set
            {
                this.rarity = value;
                this.OnPropertyChanged("Rarity");
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
                this.OnPropertyChanged("Description");
            }
        }

        [JsonIgnore]
        public string DescriptionProperty
        {
            get
            {
                int i = AppState.Current.TInventory.Items.Count(it => it.ContainerID.Equals(this.ObjectID));
                return i == 0 ? this.Description : MainWindow.Translate("Text_Item_HasContents", i) + "\n\n" + this.Description;
            }

            set
            {
                this.Description = value;
            }
        }

        public Visibility HasItems
        {
            get => AppState.Current.TInventory.Items.Any(it => it.ContainerID.Equals(this.ObjectID)) ? Visibility.Visible : Visibility.Hidden;
            set => this.OnPropertyChanged("HasItems");
        }

        public string ImageIndex
        {
            get => this.imageIndex;
            set
            {
                this.imageIndex = value;
                this.OnPropertyChanged("ImageIndex");
                this.OnPropertyChanged("PictureProperty");
            }
        }

        public int GP
        {
            get => this.Cost.GP;
            set
            {
                int valDiff = value - this.Cost.GP;
                this.cost.Total += valDiff * 100;
                this.OnPropertyChanged("Cost");
                this.OnPropertyChanged("GP");
                this.OnPropertyChanged("SP");
                this.OnPropertyChanged("CP");
            }
        }

        public int SP
        {
            get => this.Cost.SP;
            set
            {
                int valDiff = value - this.Cost.SP;
                this.cost.Total += valDiff * 10;
                this.OnPropertyChanged("Cost");
                this.OnPropertyChanged("GP");
                this.OnPropertyChanged("SP");
                this.OnPropertyChanged("CP");
            }
        }

        public int CP
        {
            get => this.Cost.CP;
            set
            {
                int valDiff = value - this.Cost.CP;
                this.cost.Total += valDiff;
                this.OnPropertyChanged("Cost");
                this.OnPropertyChanged("GP");
                this.OnPropertyChanged("SP");
                this.OnPropertyChanged("CP");
            }
        }

        public uint TitleColor
        {
            get => this.color;
            set
            {
                this.color = value;
                this.OnPropertyChanged("Color");
            }
        }

        public Guid ObjectID { get; set; } = Guid.NewGuid();

        public Guid ContainerID { get; set; } = Guid.Empty;

        [JsonIgnore]
        public string GeneratedDescription => $"{ this.Name }\nAmount: { this.Amount }\nWeight: { this.Weight * this.Amount }({ this.Weight })\nCost:{ this.Cost }\nType:{ this.Type }\nRarity: { this.Rarity }\n\n{ this.DescriptionProperty }";

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IgnoreItemWeight { get; set; }

        [JsonIgnore]
        public float ItemWeightLogicMul => this.IgnoreItemWeight ? 0 : 1;

        public InventoryItem()
        {
        }

        public InventoryItem(ItemTemplate template) : base()
        {
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

        public void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public InventoryItem Copy()
        {
            return new InventoryItem()
            {
                ImageList = this.ImageList,
                ImageIndex = this.ImageIndex,
                Name = this.Name,
                Amount = this.Amount,
                Weight = this.Weight,
                IgnoreItemWeight = this.IgnoreItemWeight,
                Cost = this.Cost.Copy(),
                Type = this.Type,
                Rarity = this.Rarity,
                Description = this.Description,
                TitleColor = this.TitleColor,
                ObjectID = this.ObjectID
            };
        }

        public JObject ToShareObject()
        {
            JObject mio = JObject.FromObject(this);
            JArray mia = new JArray();
            foreach (InventoryItem ii in AppState.Current.State.Inventory.Items)
            {
                if (ii.ContainerID.Equals(this.ObjectID))
                {
                    mia.Add(ii.ToShareObject());
                }
            }

            JObject ret = new JObject
            {
                ["main_item"] = mio,
                ["secondary_items"] = mia
            };

            return ret;
        }

        public string ToShareString() => this.ToShareObject().ToString(Formatting.None);
    }

    public class InventoryItemLegacyAdapter
    {
        public static bool CanApply(JObject itemJson) => itemJson.ContainsKey("Cost") && itemJson["Cost"].Type == JTokenType.Integer;

        public static InventoryItem Apply(JObject obj)
        {
            return new InventoryItem()
            {
                Name = obj.Value<string>("Name"),
                Amount = obj.Value<int>("Amount"),
                Weight = obj.Value<int>("Weight"),
                Cost = new CostValue(obj.Value<int>("Cost"), 0, 0),
                Type = obj.Value<string>("Type"),
                Rarity = obj.Value<string>("Rarity"),
                Description = obj.Value<string>("Description"),
                ImageIndex = "",
                TitleColor = 0
            };
        }
    }
}
