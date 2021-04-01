namespace VSCC.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueDependency = DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));
        public static readonly DependencyProperty MaximumDependency = DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(int.MaxValue));
        public static readonly DependencyProperty MinimumDependency = DependencyProperty.Register("MinValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));
        public static readonly DependencyProperty StepDependency = DependencyProperty.Register("Step", typeof(int), typeof(NumericUpDown), new PropertyMetadata(1));
        public static readonly DependencyProperty TextAlignmentDep = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(NumericUpDown), new PropertyMetadata(TextAlignment.Left));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericUpDown));

        private bool _recalcValue;

        [Category("Common")]
        public int Value
        {
            get => (int)this.GetValue(ValueDependency);

            set
            {
                int ov = (int)this.GetValue(ValueDependency);
                this.SetValue(ValueDependency, value);
                this.OnValueChanged(new RoutedPropertyChangedEventArgs<int>(ov, this.Value, ValueChangedEvent));
            }
        }

        [Category("Common")]
        public int Maximum { get => (int)this.GetValue(MaximumDependency); set => this.SetValue(MaximumDependency, value); }

        [Category("Common")]
        public int Minimum { get => (int)this.GetValue(MinimumDependency); set => this.SetValue(MinimumDependency, value); }

        [Category("Common")]
        public int Step { get => (int)this.GetValue(StepDependency); set => this.SetValue(StepDependency, value); }

        [Category("Layout")]
        public TextAlignment TextAlignment { get => (TextAlignment)this.GetValue(TextAlignmentDep); set => this.SetValue(TextAlignmentDep, value); }

        public event RoutedPropertyChangedEventHandler<int> ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }

        public NumericUpDown()
        {
            this.InitializeComponent();
            this.TB_Content.TextChanged += this.TB_Content_TextChanged;
            this.BtnUp.Click += this.BtnUp_Click;
            this.BtnDown.Click += this.BtnDown_Click;
            this._recalcValue = false;
            this.TB_Content.Text = this.Value.ToString();
            this._recalcValue = true;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == FontSizeProperty)
            {
                this.TB_Content.FontSize = (double)e.NewValue;
            }

            if (e.Property == TextAlignmentDep)
            {
                this.TB_Content.TextAlignment = (TextAlignment)e.NewValue;
            }
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e) => this.Value -= this.Step;

        private void BtnUp_Click(object sender, RoutedEventArgs e) => this.Value += this.Step;

        private void TB_Content_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this._recalcValue)
            {
                if (int.TryParse(this.TB_Content.Text, out int i))
                {
                    if (i >= this.Minimum && i <= this.Maximum)
                    {
                        e.Handled = true;
                        this._recalcValue = false;
                        this.Value = i;
                        this.EnableDisableButtons();
                        this._recalcValue = true;
                    }
                }

                e.Handled = true;
                this._recalcValue = false;
                this.TB_Content.Text = this.Value.ToString();
                this.EnableDisableButtons();
                this._recalcValue = true;
            }
        }

        private void EnableDisableButtons()
        {
            this.BtnDown.IsEnabled = this.Value - this.Step >= this.Minimum;
            this.BtnUp.IsEnabled = this.Value + this.Step <= this.Maximum;
        }

        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            this.RaiseEvent(args);
            this.EnableDisableButtons();
            this._recalcValue = false;
            this.TB_Content.Text = this.Value.ToString();
            this._recalcValue = true;
        }
    }
}
