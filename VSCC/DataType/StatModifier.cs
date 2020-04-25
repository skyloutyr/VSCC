namespace VSCC.DataType
{
    using System.Windows.Media;

    public readonly struct StatModifier
    {
        public int Value { get; }
        public string Label { get; }

        public SolidColorBrush LabelBrush => this.Value < 0 ? Brushes.Red : this.Value > 0 ? Brushes.Green : Brushes.Orange;


        public StatModifier(int value, string label)
        {
            this.Value = value;
            this.Label = label;
        }
    }
}
