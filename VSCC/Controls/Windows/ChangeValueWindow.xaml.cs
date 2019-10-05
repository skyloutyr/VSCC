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

namespace VSCC.Controls.Windows
{
    /// <summary>
    /// Interaction logic for ChangeValueWindow.xaml
    /// </summary>
    public partial class ChangeValueWindow : Window
    {
        public ChangeValueWindow()
        {
            InitializeComponent();
        }

        public void Button_Click(object sender, RoutedEventArgs args)
        {
            this.DialogResult = false;
            this.Close();
        }

        public void Button_Click_1(object sender, RoutedEventArgs args)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
