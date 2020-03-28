namespace VSCC.Controls.Windows
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for ScriptContextWindow.xaml
    /// </summary>
    public partial class ScriptContextWindow : Window
    {
        public ScriptContextWindow() => this.InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
