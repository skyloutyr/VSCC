namespace VSCC.Controls.Windows
{
    using Microsoft.Win32;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using VSCC.DataType;

    /// <summary>
    /// Interaction logic for CreateFeatWindow.xaml
    /// </summary>
    public partial class CreateFeatWindow : Window
    {
        public CreateFeatWindow() => this.InitializeComponent();

        public void SetDataContext(Feat context) => this.DataContext = this.Img_Picture.DataContext = this.Btn_ColorChange.DataContext = this.Btn_ColorChangeBack.DataContext = this.Btn_ColorChangeVal.DataContext = this.TextBox_Name.DataContext = this.IntUD_CurrentValue.DataContext = this.IntUD_MaxValue.DataContext = this.TextBox_SimpleDesc.DataContext = this.TextBox_Desc.DataContext = context;

        // Select Image
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Images/Lists/Skills/")),
                Multiselect = false
            };

            if (ofd.ShowDialog() ?? false)
            {
                ((Feat)this.DataContext).ImageIndex = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Btn_ColorChange_Click(object sender, RoutedEventArgs e) // Name Color
        {
            ColorPickerWindow cpw = new ColorPickerWindow();
            cpw.ColorPicker_Picker.SelectedColor = ((SolidColorBrush)((Feat)this.DataContext).NameColorProperty).Color;
            if (cpw.ShowDialog() ?? false)
            {
                ((Feat)this.DataContext).NameColor = cpw.ARGB;
                ((Button)sender).InvalidateVisual();
                this.TextBox_Name.InvalidateVisual();
            }
        }

        private void Btn_ValColorChange_Click(object sender, RoutedEventArgs e) // Foreground Bar Color
        {
            ColorPickerWindow cpw = new ColorPickerWindow();
            cpw.ColorPicker_Picker.SelectedColor = ((SolidColorBrush)((Feat)this.DataContext).BarForegroundProperty).Color;
            if (cpw.ShowDialog() ?? false)
            {
                ((Feat)this.DataContext).BarColorForeground = cpw.ARGB;
                ((Button)sender).InvalidateVisual();
                this.IntUD_CurrentValue.InvalidateVisual();
            }
        }

        private void Btn_BackColorChange_Click(object sender, RoutedEventArgs e) // Background Bar Color
        {
            ColorPickerWindow cpw = new ColorPickerWindow();
            cpw.ColorPicker_Picker.SelectedColor = ((SolidColorBrush)((Feat)this.DataContext).BarBackgroundProperty).Color;
            if (cpw.ShowDialog() ?? false)
            {
                ((Feat)this.DataContext).BarColorBackground = cpw.ARGB;
                ((Button)sender).InvalidateVisual();
                this.IntUD_MaxValue.InvalidateVisual();
            }
        }
    }
}
