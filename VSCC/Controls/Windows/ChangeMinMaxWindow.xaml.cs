using System.Windows;

namespace VSCC.Controls.Windows
{
    /// <summary>
    /// Interaction logic for ChangeMinMaxWindow.xaml
    /// </summary>
    public partial class ChangeMinMaxWindow : Window
    {
        public ChangeMinMaxWindow() => this.InitializeComponent();

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
    }
}
