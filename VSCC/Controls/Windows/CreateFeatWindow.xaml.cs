namespace VSCC.Controls.Windows
{
    using ColorPickerWPF;
    using Microsoft.Win32;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using VSCC.DataType;
    using VSCC.State;

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
                string s = ofd.FileName;
                if (AppState.Current.TExtras.Images.CheckImageValidity(ref s))
                {
                    ((Feat)this.DataContext).ImageIndex = s.Substring(AppState.Current.TExtras.Images.BaseFolderPath.Length);
                }
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

        private uint Color2ARGB(Color c) => ((uint)c.A << 24) | ((uint)c.R << 16) | ((uint)c.G << 8) | c.B;

        private void Btn_ColorChange_Click(object sender, RoutedEventArgs e) // Name Color
        {
            if (ColorPickerWindow.ShowDialog(out Color color))
            {
                ((Feat)this.DataContext).NameColor = this.Color2ARGB(color);
                ((Button)sender).InvalidateVisual();
                this.TextBox_Name.InvalidateVisual();
            }
        }

        private void Btn_ValColorChange_Click(object sender, RoutedEventArgs e) // Foreground Bar Color
        {
            if (ColorPickerWindow.ShowDialog(out Color color))
            {
                ((Feat)this.DataContext).BarColorForeground = this.Color2ARGB(color);
                ((Button)sender).InvalidateVisual();
                this.IntUD_CurrentValue.InvalidateVisual();
            }
        }

        private void Btn_BackColorChange_Click(object sender, RoutedEventArgs e) // Background Bar Color
        {
            if (ColorPickerWindow.ShowDialog(out Color color))
            {
                ((Feat)this.DataContext).BarColorBackground = this.Color2ARGB(color);
                ((Button)sender).InvalidateVisual();
                this.IntUD_MaxValue.InvalidateVisual();
            }
        }
    }
}
