namespace VSCC.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ColoredBar.xaml
    /// </summary>
    public partial class ColoredBar : UserControl
    {
        public static readonly DependencyProperty CurrentValueDependency = DependencyProperty.Register("CurrentValue", typeof(int), typeof(ColoredBar));

        [Category("Appearance")]
        public int CurrentValue { get => (int)this.GetValue(CurrentValueDependency); set => this.SetValue(CurrentValueDependency, value); }

        public static readonly DependencyProperty MaximumValueDependency = DependencyProperty.Register("MaximumValue", typeof(int), typeof(ColoredBar));

        [Category("Appearance")]
        public int MaximumValue { get => (int)this.GetValue(MaximumValueDependency); set => this.SetValue(MaximumValueDependency, value); }

        public static readonly DependencyProperty BarColorDependency = DependencyProperty.Register("BarColor", typeof(Brush), typeof(ColoredBar));

        [Category("Appearance")]
        public Brush BarColor { get => (Brush)this.GetValue(BarColorDependency); set => this.SetValue(BarColorDependency, value); }

        public static readonly DependencyProperty BarBackgroundDependency = DependencyProperty.Register("BarBackground", typeof(Brush), typeof(ColoredBar));

        [Category("Appearance")]
        public Brush BarBackground { get => (Brush)this.GetValue(BarBackgroundDependency); set => this.SetValue(BarBackgroundDependency, value); }

        public static readonly DependencyProperty BarBorderDependency = DependencyProperty.Register("BarBorder", typeof(Pen), typeof(ColoredBar));

        [Category("Appearance")]
        public Pen BarBorder { get => (Pen)this.GetValue(BarBorderDependency); set => this.SetValue(BarBorderDependency, value); }

        public ColoredBar()
        {
            this.InitializeComponent();
            this.DefaultStyleKey = typeof(ColoredBar);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(this.BarBackground, this.BarBorder, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawRectangle(this.BarColor, null, new Rect(2, 2, Math.Max(0, (this.ActualWidth - 4) * Math.Min(1, this.MaximumValue == 0 ? 0 : (float)this.CurrentValue / this.MaximumValue)), this.ActualHeight - 4));
            FormattedText ft = new FormattedText(
                $"{ this.CurrentValue }/{ this.MaximumValue }",
                CultureInfo.CurrentCulture,
                this.FlowDirection,
                new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                this.FontSize,
                this.Foreground,
                (PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11 ?? 1) * 96.0
            );

            drawingContext.DrawText(ft, new Point((this.ActualWidth / 2) - (ft.Width / 2), (this.ActualHeight / 2) - (ft.Height / 2)));
        }
    }
}
