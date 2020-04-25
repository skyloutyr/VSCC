namespace VSCC.Controls.Dialogs
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for NewStatModifierDialog.xaml
    /// </summary>
    public partial class NewStatModifierDialog : Window
    {
        public NewStatModifierDialog() => this.InitializeComponent();

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
