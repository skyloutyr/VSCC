namespace VSCC.Controls.Windows.Macro
{
    using System.Windows;
    using VSCC.Roll20.Macros;

    /// <summary>
    /// Interaction logic for DefineLocalWindow.xaml
    /// </summary>
    public partial class DefineLocalWindow : Window
    {
        public Macro EditedMacro { get; set; }

        public DefineLocalWindow() => this.InitializeComponent();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LocalType lt = (LocalType)this.CB_Type.SelectedIndex;
            switch (lt)
            {
                case LocalType.Boolean:
                {
                    if (!bool.TryParse(this.TB_Value.Text, out _))
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_NoValue_Desc"), MainWindow.Translate("Message_Local_NoValue_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (this.EditedMacro.BoolLocals.ContainsKey(this.TB_Name.Text) && !this.TB_Name.IsReadOnly)
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_SameName_Desc"), MainWindow.Translate("Message_Local_SameName_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    break;
                }

                case LocalType.Integer:
                {
                    if (!int.TryParse(this.TB_Value.Text, out _))
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_NoValue_Desc"), MainWindow.Translate("Message_Local_NoValue_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (this.EditedMacro.NumberLocals.ContainsKey(this.TB_Name.Text) && !this.TB_Name.IsReadOnly)
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_SameName_Desc"), MainWindow.Translate("Message_Local_SameName_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    break;
                }

                case LocalType.Real:
                {
                    if (!float.TryParse(this.TB_Value.Text, out _))
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_NoValue_Desc"), MainWindow.Translate("Message_Local_NoValue_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (this.EditedMacro.RealLocals.ContainsKey(this.TB_Name.Text) && !this.TB_Name.IsReadOnly)
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_SameName_Desc"), MainWindow.Translate("Message_Local_SameName_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    break;
                }

                case LocalType.String:
                {
                    if (this.EditedMacro.StringLocals.ContainsKey(this.TB_Name.Text) && !this.TB_Name.IsReadOnly)
                    {
                        MessageBox.Show(MainWindow.Translate("Message_Local_SameName_Desc"), MainWindow.Translate("Message_Local_SameName_Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    break;
                }
            }

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
