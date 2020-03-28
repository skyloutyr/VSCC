namespace VSCC.Controls.Windows
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for ChangeValueWindow.xaml
    /// </summary>
    public partial class ChangeValueWindow : Window
    {
        public ChangeValueWindow() => this.InitializeComponent();

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
