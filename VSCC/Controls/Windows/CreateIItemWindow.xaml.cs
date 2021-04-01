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
    /// Interaction logic for CreateIItemWindow.xaml
    /// </summary>
    public partial class CreateIItemWindow : Window
    {
        public CreateIItemWindow() => this.InitializeComponent();

        public void SetDataContext(InventoryItem context) => this.DataContext = this.Img_Picture.DataContext = this.Btn_ColorChange.DataContext = this.TextBox_Name.DataContext = this.IntUD_Amount.DataContext = this.SUD_Weight.DataContext = this.IntUD_Cost_CP.DataContext = this.IntUD_Cost_GP.DataContext = this.IntUD_Cost_SP.DataContext = this.ComboBox_Type.DataContext = this.ComboBox_Rarity.DataContext = this.TextBox_Description.DataContext = context;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Images/Lists/Items/")),
                Multiselect = false
            };

            if (ofd.ShowDialog() ?? false)
            {
                string path = ofd.FileName;
                if (AppState.Current.TInventory.Images.CheckImageValidity(ref path))
                {
                    string name = path.Substring(AppState.Current.TInventory.Images.BaseFolderPath.Length);
                    ((InventoryItem)this.DataContext).ImageIndex = name;
                }
            }
        }

        private uint Color2ARGB(Color c) => ((uint)c.A << 24) | ((uint)c.R << 16) | ((uint)c.G << 8) | c.B;

        private void Button_Click_3(object sender, RoutedEventArgs e) // Color picker button
        {
            if (ColorPickerWindow.ShowDialog(out Color color))
            {
                ((InventoryItem)this.DataContext).TitleColor = this.Color2ARGB(color);
                ((Button)sender).InvalidateVisual();
                this.TextBox_Name.InvalidateVisual();
            }
        }
    }
}
