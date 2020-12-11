namespace VSCC.Controls.Windows
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ColorPickerWindow.xaml
    /// </summary>
    public partial class ColorPickerWindow : Window
    {
        public Color? Color { get; set; }
        public uint ARGB => this.Color.HasValue ?
                    ((uint)this.Color.Value.A << 24) |
                    ((uint)this.Color.Value.R << 16) |
                    ((uint)this.Color.Value.G << 8) |
                    ((uint)this.Color.Value.B)
                : 0;

        public ColorPickerWindow() => this.InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e) // Cancel
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // OK
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ColorPicker_Picker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e) => this.Color = e.NewValue;
    }
}
