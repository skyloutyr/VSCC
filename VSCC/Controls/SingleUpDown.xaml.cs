namespace VSCC.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SingleUpDown.xaml
    /// </summary>
    public partial class SingleUpDown : UserControl
    {
        public static readonly DependencyProperty ValueDependency = DependencyProperty.Register("Value", typeof(float), typeof(SingleUpDown), new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public static readonly DependencyProperty MaximumDependency = DependencyProperty.Register("MaxValue", typeof(float), typeof(SingleUpDown), new PropertyMetadata(float.MaxValue));
        public static readonly DependencyProperty MinimumDependency = DependencyProperty.Register("MinValue", typeof(float), typeof(SingleUpDown), new PropertyMetadata(0f));
        public static readonly DependencyProperty StepDependency = DependencyProperty.Register("Step", typeof(float), typeof(SingleUpDown), new PropertyMetadata(1f));
        public static readonly DependencyProperty TextAlignmentDep = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(SingleUpDown), new PropertyMetadata(TextAlignment.Left));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<float>), typeof(SingleUpDown));

        private bool _recalcValue;

        [Category("Common")]
        public float Value
        {
            get => (float)this.GetValue(ValueDependency);

            set
            {
                float ov = (float)this.GetValue(ValueDependency);
                this.SetValue(ValueDependency, value);
                this.OnValueChanged(new RoutedPropertyChangedEventArgs<float>(ov, this.Value, ValueChangedEvent));
            }
        }

        [Category("Common")]
        public float Maximum { get => (float)this.GetValue(MaximumDependency); set => this.SetValue(MaximumDependency, value); }

        [Category("Common")]
        public float Minimum { get => (float)this.GetValue(MinimumDependency); set => this.SetValue(MinimumDependency, value); }

        [Category("Common")]
        public float Step { get => (float)this.GetValue(StepDependency); set => this.SetValue(StepDependency, value); }

        [Category("Layout")]
        public TextAlignment TextAlignment { get => (TextAlignment)this.GetValue(TextAlignmentDep); set => this.SetValue(TextAlignmentDep, value); }

        public event RoutedPropertyChangedEventHandler<float> ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }

        public SingleUpDown()
        {
            this.InitializeComponent();
            this.TB_Content.TextChanged += this.TB_Content_TextChanged;
            this.BtnUp.Click += this.BtnUp_Click;
            this.BtnDown.Click += this.BtnDown_Click;
            this._recalcValue = false;
            this.TB_Content.Text = this.Value.ToString();
            this._recalcValue = true;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleUpDown nud = (SingleUpDown)d;
            nud._recalcValue = false;
            nud.TB_Content.Text = e.NewValue.ToString();
            nud._recalcValue = true;
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
                if (float.TryParse(this.TB_Content.Text, out float i))
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

        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<float> args)
        {
            this.RaiseEvent(args);
            this.EnableDisableButtons();
            this._recalcValue = false;
            this.TB_Content.Text = this.Value.ToString();
            this._recalcValue = true;
        }
    }
}
