namespace VSCC.Controls.Windows.Macro
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for DefineLocalWindow.xaml
    /// </summary>
    public partial class DefineLocalWindow : Window
    {
        public DefineLocalWindow() => this.InitializeComponent();

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
    }
}
