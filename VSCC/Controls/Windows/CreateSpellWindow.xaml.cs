using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using VSCC.DataType;

namespace VSCC.Controls.Windows
{
    /// <summary>
    /// Interaction logic for CreateSpellWindow.xaml
    /// </summary>
    public partial class CreateSpellWindow : Window
    {
        public CreateSpellWindow()
        {
            InitializeComponent();
        }

        public void SetDataContext(Spell context)
        {
            this.DataContext = this.Img_Picture.DataContext = this.TextBox_Name.DataContext = this.CheckBox_Verbal.DataContext = this.CheckBox_Somatic.DataContext = this.CheckBox_Material.DataContext = this.CheckBox_Concentration.DataContext = this.IntUD_Level.DataContext = this.TextBox_Range.DataContext = this.TextBox_Duration.DataContext = this.TextBox_CastTime.DataContext = this.ComboBox_School.DataContext = this.ComboBox_Targets.DataContext = this.TextBox_SimpleDesc.DataContext = this.TextBox_Desc.DataContext = context;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Images/Lists/Spells/")),
                Multiselect = false
            };

            if (ofd.ShowDialog() ?? false)
            {
                ((Spell)this.DataContext).ImageIndex = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
            }
        }
    }
}
