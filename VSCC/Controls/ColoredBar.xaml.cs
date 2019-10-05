using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VSCC.Controls
{
    /// <summary>
    /// Interaction logic for ColoredBar.xaml
    /// </summary>
    public partial class ColoredBar : UserControl
    {
        public static readonly DependencyProperty CurrentValueDependency = DependencyProperty.Register("CurrentValue", typeof(int), typeof(ColoredBar));

        [Category("Appearance")]
        public int CurrentValue { get => (int)GetValue(CurrentValueDependency); set => SetValue(CurrentValueDependency, value); }

        public static readonly DependencyProperty MaximumValueDependency = DependencyProperty.Register("MaximumValue", typeof(int), typeof(ColoredBar));

        [Category("Appearance")]
        public int MaximumValue { get => (int)GetValue(MaximumValueDependency); set => SetValue(MaximumValueDependency, value); }

        public static readonly DependencyProperty BarColorDependency = DependencyProperty.Register("BarColor", typeof(Brush), typeof(ColoredBar));

        [Category("Appearance")]
        public Brush BarColor { get => (Brush)GetValue(BarColorDependency); set => SetValue(BarColorDependency, value); }

        public static readonly DependencyProperty BarBackgroundDependency = DependencyProperty.Register("BarBackground", typeof(Brush), typeof(ColoredBar));

        [Category("Appearance")]
        public Brush BarBackground { get => (Brush)GetValue(BarBackgroundDependency); set => SetValue(BarBackgroundDependency, value); }

        public static readonly DependencyProperty BarBorderDependency = DependencyProperty.Register("BarBorder", typeof(Pen), typeof(ColoredBar));

        [Category("Appearance")]
        public Pen BarBorder { get => (Pen)GetValue(BarBorderDependency); set => SetValue(BarBorderDependency, value); }

        public ColoredBar()
        {
            InitializeComponent();
            DefaultStyleKey = typeof(ColoredBar);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(this.BarBackground, this.BarBorder, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawRectangle(this.BarColor, null, new Rect(2, 2, (this.ActualWidth - 4) * ((float)this.CurrentValue / this.MaximumValue), this.ActualHeight - 4));
            FormattedText ft = new FormattedText(
                $"{ this.CurrentValue }/{ this.MaximumValue }", 
                CultureInfo.CurrentCulture, 
                this.FlowDirection, 
                new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch), 
                this.FontSize, 
                Brushes.Black, 
                (PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11 ?? 1) * 96.0
            );

            drawingContext.DrawText(ft, new Point(this.ActualWidth / 2 - ft.Width / 2, this.ActualHeight / 2 - ft.Height / 2));
        }
    }
}
