namespace VSCC.DataType
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;
    using VSCC.Models.ImageList;
    using VSCC.State;

    public class Spell : INotifyPropertyChanged
    {
        [JsonIgnore]
        private string name;

        [JsonIgnore]
        private SpellComponents spellComponents = SpellComponents.None;

        [JsonIgnore]
        private string school;

        [JsonIgnore]
        private int level;

        [JsonIgnore]
        private string imageIndex;

        [JsonIgnore]
        private string target;

        [JsonIgnore]
        private string range;

        [JsonIgnore]
        private string description;

        [JsonIgnore]
        private string duration;

        [JsonIgnore]
        private string castTime;

        [JsonIgnore]
        private string simpleDescription;

        [JsonIgnore]
        public ImageListModel ImageList { get; set; }

        [JsonIgnore]
        public BitmapImage PictureProperty => (this.ImageList ?? AppState.Current.TSpellbook.Images)[this.ImageIndex]?.Image ?? null;

        public Templates.SpellTemplate Template { get; set; }

        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }
        public SpellComponents SpellComponents
        {
            get => this.spellComponents;
            set
            {
                this.spellComponents = value;
                this.OnPropertyChanged("SpellComponents");
                this.OnPropertyChanged("GeneratedDescription");
                this.OnPropertyChanged("PropertyVerbal");
                this.OnPropertyChanged("PropertySomatic");
                this.OnPropertyChanged("PropertyMaterial");
                this.OnPropertyChanged("PropertyConcentration");
            }
        }
        public string School
        {
            get => this.school;
            set
            {
                this.school = value;
                this.OnPropertyChanged("School");
            }
        }
        public int Level
        {
            get => this.level;
            set
            {
                this.level = value;
                this.OnPropertyChanged("Level");
                this.OnPropertyChanged("GeneratedDescription");
            }
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
        public string Target
        {
            get => this.target;
            set
            {
                this.target = value;
                this.OnPropertyChanged("Target");
            }
        }
        public string Range
        {
            get => this.range;
            set
            {
                this.range = value;
                this.OnPropertyChanged("Range");
                this.OnPropertyChanged("GeneratedDescription");
            }
        }
        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.OnPropertyChanged("Description");
                this.OnPropertyChanged("GeneratedDescription");
            }
        }
        public string Duration
        {
            get => this.duration;
            set
            {
                this.duration = value;
                this.OnPropertyChanged("Duration");
                this.OnPropertyChanged("GeneratedDescription");
            }
        }
        public string CastTime
        {
            get => this.castTime;
            set
            {
                this.castTime = value;
                this.OnPropertyChanged("CastTime");
                this.OnPropertyChanged("GeneratedDescription");
            }
        }
        public string SimpleDescription
        {
            get => this.simpleDescription;
            set
            {
                this.simpleDescription = value;
                this.OnPropertyChanged("SimpleDescription");
            }
        }

        public bool PropertyVerbal
        {
            get => this.SpellComponents.HasFlag(SpellComponents.Verbal);
            set
            {
                if (value)
                {
                    this.SpellComponents |= SpellComponents.Verbal;
                }
                else
                {
                    this.SpellComponents &= ~SpellComponents.Verbal;
                }
            }
        }

        public bool PropertySomatic
        {
            get => this.SpellComponents.HasFlag(SpellComponents.Somatic);
            set
            {
                if (value)
                {
                    this.SpellComponents |= SpellComponents.Somatic;
                }
                else
                {
                    this.SpellComponents &= ~SpellComponents.Somatic;
                }
            }
        }

        public bool PropertyMaterial
        {
            get => this.SpellComponents.HasFlag(SpellComponents.Material);
            set
            {
                if (value)
                {
                    this.SpellComponents |= SpellComponents.Material;
                }
                else
                {
                    this.SpellComponents &= ~SpellComponents.Material;
                }
            }
        }

        public bool PropertyConcentration
        {
            get => this.SpellComponents.HasFlag(SpellComponents.Concentration);
            set
            {
                if (value)
                {
                    this.SpellComponents |= SpellComponents.Concentration;
                }
                else
                {
                    this.SpellComponents &= ~SpellComponents.Concentration;
                }
            }
        }

        public bool PropertyRitual
        {
            get => this.SpellComponents.HasFlag(SpellComponents.Ritual);
            set
            {
                if (value)
                {
                    this.SpellComponents |= SpellComponents.Ritual;
                }
                else
                {
                    this.SpellComponents &= ~SpellComponents.Ritual;
                }
            }
        }

        [JsonIgnore]
        public string GeneratedDescription =>
            $"{ this.Name }\n" +
                $"\n" +
                $"Level { this.Level } { this.School } spell.\n" +
                $"Concentration: { (this.SpellComponents.HasFlag(SpellComponents.Concentration) ? "Yes" : "No") }\n" +
                $"Components: { (this.SpellComponents.HasFlag(SpellComponents.Verbal) ? "V" : "") + (this.SpellComponents.HasFlag(SpellComponents.Somatic) ? "S" : "") + (this.SpellComponents.HasFlag(SpellComponents.Material) ? "M" : "") }\n" +
                $"CastTime: { this.CastTime }\n" +
                $"Range: { this.Range }, Targets: { this.Target }\n" +
                $"Duration: { this.Duration }\n" +
                $"{ this.Description }\n";

        public event PropertyChangedEventHandler PropertyChanged;

        public Spell()
        {
        }

        public Spell(Templates.SpellTemplate template) : base()
        {
            this.Name = template.Name;
            this.CastTime = template.CastingTime;
            this.Description = template.DescriptionProperty;
            this.Duration = template.Duration;
            this.Level = template.Level;
            this.Range = template.Range;
            this.School = template.School;
            this.SimpleDescription = string.Empty;
            this.SpellComponents = this.ParseComponents(template.Components);
            if (template.Concentration)
            {
                this.SpellComponents |= SpellComponents.Concentration;
            }

            if (template.Ritual)
            {
                this.SpellComponents |= SpellComponents.Ritual;
            }

            this.Target = string.Empty;
            this.Template = template;
        }

        private void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private SpellComponents ParseComponents(string s)
        {
            SpellComponents ret = SpellComponents.None;
            if (string.IsNullOrEmpty(s))
            {
                return ret;
            }

            if (s.IndexOf('V') != -1)
            {
                ret |= SpellComponents.Verbal;
            }

            if (s.IndexOf('S') != -1)
            {
                ret |= SpellComponents.Somatic;
            }

            if (s.IndexOf('M') != -1)
            {
                ret |= SpellComponents.Material;
            }

            if (s.IndexOf('C') != -1)
            {
                ret |= SpellComponents.Concentration;
            }

            return ret;
        }

        public Spell Copy() => new Spell()
        {
            Name = this.Name,
            Level = this.Level,
            Template = this.Template,
            School = this.School,
            Range = this.Range,
            Target = this.Target,
            CastTime = this.CastTime,
            Duration = this.Duration,
            SpellComponents = this.SpellComponents,
            SimpleDescription = this.SimpleDescription,
            Description = this.Description,
            ImageIndex = this.ImageIndex,
            ImageList = this.ImageList,
        };
    }

    public class SpellLegacyAdapter
    {
        public static bool CanApply(JObject obj) => obj.ContainsKey("Verbal") && obj["Verbal"].Type == JTokenType.Boolean;

        public static Spell Apply(JObject obj) => new Spell()
        {
            Name = obj.Value<string>("Name"),
            SpellComponents = (obj.Value<bool>("Verbal") ? SpellComponents.Verbal : 0) | (obj.Value<bool>("Somatic") ? SpellComponents.Somatic : 0) | (obj.Value<bool>("Material") ? SpellComponents.Material : 0) | (obj.Value<bool>("Concentration") ? SpellComponents.Concentration : 0),
            School = obj.Value<string>("School"),
            Level = obj.Value<int>("Level"),
            ImageIndex = string.Empty,
            Target = obj.Value<string>("Target"),
            Range = obj.Value<int>("Range").ToString(),
            Description = obj.Value<string>("Description"),
            Duration = obj.Value<string>("Duration"),
            CastTime = obj.Value<string>("CastTime"),
            SimpleDescription = "Spell converted from older version. Edit it to make this description be a thing.",
            Template = Templates.SpellTemplate.Empty
        };
    }

    [Flags]
    public enum SpellComponents
    {
        None = 0,
        Verbal = 1,
        Somatic = 2,
        Material = 4,
        Concentration = 8,
        Ritual = 16,
    }
}
