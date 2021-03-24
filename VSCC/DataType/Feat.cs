namespace VSCC.DataType
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using VSCC.Models.ImageList;
    using VSCC.State;

    public class Feat : INotifyPropertyChanged
    {
        [JsonIgnore]
        private string name;

        [JsonIgnore]
        private string desc;

        [JsonIgnore]
        private string full_desc;

        [JsonIgnore]
        private string image_index;

        [JsonIgnore]
        private uint color_bar;

        [JsonIgnore]
        private uint color_bar_active;

        [JsonIgnore]
        private uint color_name;

        [JsonIgnore]
        private int amt;

        [JsonIgnore]
        private int max;

        public string NameProperty
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged("NameProperty");
            }
        }

        public string DescProperty
        {
            get => this.desc;
            set
            {
                this.desc = value;
                this.OnPropertyChanged("DescProperty");
            }
        }

        public string FullDescProperty
        {
            get => this.full_desc;
            set
            {
                this.full_desc = value;
                this.OnPropertyChanged("FullDescProperty");
            }
        }

        [JsonIgnore]
        public string ValueAndMaxProperty => $"{ this.ValueProperty }/{ this.ValueMaxProperty }";

        public string ImageIndex
        {
            get => this.image_index;
            set
            {
                this.image_index = value;
                this.OnPropertyChanged("ImageIndex");
                this.OnPropertyChanged("PictureProperty");
            }
        }

        public uint NameColor
        {
            get => this.color_name;
            set
            {
                this.color_name = value;
                this.OnPropertyChanged("NameColorProperty");
                this.OnPropertyChanged("NameProperty");
            }
        }

        public uint BarColorForeground
        {
            get => this.color_bar_active;
            set
            {
                this.color_bar_active = value;
                this.OnPropertyChanged("BarForegroundProperty");
            }
        }

        public uint BarColorBackground
        {
            get => this.color_bar;
            set
            {
                this.color_bar = value;
                this.OnPropertyChanged("BarBackgroundProperty");
            }
        }

        public int ValueProperty
        {
            get => this.amt;
            set
            {
                this.amt = value;
                this.OnPropertyChanged("ValueProperty");
                this.OnPropertyChanged("ValueAndMaxProperty");
            }
        }

        public int ValueMaxProperty
        {
            get => this.max;
            set
            {
                this.max = value;
                this.OnPropertyChanged("ValueMaxProperty");
                this.OnPropertyChanged("ValueAndMaxProperty");
            }
        }

        [JsonIgnore]
        public Brush NameColorProperty
        {
            get
            {
                if (this.color_name == 0)
                {
                    return (SolidColorBrush)AppState.Current.Window.TryFindResource("Static.Foreground");
                }

                byte a = (byte)((this.color_name >> 24) & 0xFF);
                byte r = (byte)((this.color_name >> 16) & 0xFF);
                byte g = (byte)((this.color_name >> 8) & 0xFF);
                byte b = (byte)((this.color_name) & 0xFF);
                Color c = Color.FromArgb(a, r, g, b);
                return new SolidColorBrush(c);
            }
        }

        [JsonIgnore]
        public Brush BarForegroundProperty
        {
            get
            {
                if (this.color_bar_active == 0)
                {
                    return (SolidColorBrush)AppState.Current.Window.TryFindResource("Static.Background");
                }

                byte a = (byte)((this.color_bar_active >> 24) & 0xFF);
                byte r = (byte)((this.color_bar_active >> 16) & 0xFF);
                byte g = (byte)((this.color_bar_active >> 8) & 0xFF);
                byte b = (byte)((this.color_bar_active) & 0xFF);
                Color c = Color.FromArgb(a, r, g, b);
                return new SolidColorBrush(c);
            }
        }

        [JsonIgnore]
        public Brush BarBackgroundProperty
        {
            get
            {
                if (this.color_bar == 0)
                {
                    return (SolidColorBrush)AppState.Current.Window.TryFindResource("Static.Gray");
                }

                byte a = (byte)((this.color_bar >> 24) & 0xFF);
                byte r = (byte)((this.color_bar >> 16) & 0xFF);
                byte g = (byte)((this.color_bar >> 8) & 0xFF);
                byte b = (byte)((this.color_bar) & 0xFF);
                Color c = Color.FromArgb(a, r, g, b);
                return new SolidColorBrush(c);
            }
        }

        [JsonIgnore]
        public ImageListModel ImageList { get; set; }

        [JsonIgnore]
        public BitmapImage PictureProperty => this.image_index == null ? null : this.ImageList[this.image_index].Image;


        public Guid ObjectID { get; set; } = Guid.NewGuid();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Feat Copy() => new Feat()
        {
            name = this.name,
            desc = this.desc,
            full_desc = this.full_desc,
            image_index = this.image_index,
            color_bar = this.color_bar,
            color_bar_active = this.color_bar_active,
            color_name = this.color_name,
            amt = this.amt,
            max = this.max,
            ImageList = this.ImageList,
            ObjectID = this.ObjectID
        };

        public JObject ToShareObject()
        {
            JObject mio = JObject.FromObject(this);
            JObject ret = new JObject
            {
                ["main_feat"] = mio
            };

            return ret;
        }

        public string ToShareString() => this.ToShareObject().ToString(Formatting.None);
    }
}
