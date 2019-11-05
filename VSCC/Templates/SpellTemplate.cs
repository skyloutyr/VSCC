using System;
using System.Windows.Media.Imaging;

namespace VSCC.Templates
{
    public class SpellTemplate
    {
        public static SpellTemplate Empty => new SpellTemplate();

        private Func<string, BitmapImage> _imgGetter;

        public BitmapSource SchoolIconProperty => string.IsNullOrEmpty(this.SchoolPointer) ? null : this._imgGetter(this.SchoolPointer ?? this.School);

        public string Name { get; set; }
        public int Level { get; set; }
        public string School { get; set; }
        public string SchoolPointer { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Materials { get; set; }
        public string Duration { get; set; }
        public string Classes { get; set; }
        public string Description { get; set; }
        public string AtHigherLevels { get; set; }
        public bool Ritual { get; set; }
        public bool Concentration { get; set; }

        public string DescriptionProperty
        {
            get => $"{ this.Materials }\n{ this.Description }\n\n{ this.AtHigherLevels }";
            set { }
        }

        public string ComponentsProperty
        {
            get => $"{ this.Components }{ (this.Ritual && this.Concentration ? "R C" : this.Ritual ? "R" : this.Concentration ? "C" : "") }";
            set { }
        }

        // Chaining for linq
        public SpellTemplate ApplyImageGetterFunc(Func<string, BitmapImage> func)
        {
            this._imgGetter = func;
            return this;
        }
    }
}
