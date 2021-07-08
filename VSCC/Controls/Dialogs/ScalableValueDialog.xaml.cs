namespace VSCC.Controls.Dialogs
{
    using System.Windows;
    using VSCC.Roll20.AdvancedIntegration;

    /// <summary>
    /// Interaction logic for ScalableValueDialog.xaml
    /// </summary>
    public partial class ScalableValueDialog : Window
    {
        public ScalableValue Value { get; set; }

        public ScalableValueDialog(ScalableValue val)
        {
            this.DataContext = this.Value = val;
            InitializeComponent();
            this.CB_EnableCustom.DataContext =
            this.IntUD_CustomLvl1Val.DataContext =
            this.IntUD_CustomLvl2Val.DataContext =
            this.IntUD_CustomLvl3Val.DataContext =
            this.IntUD_CustomLvl4Val.DataContext =
            this.IntUD_CustomLvl5Val.DataContext =
            this.IntUD_CustomLvl6Val.DataContext =
            this.IntUD_CustomLvl7Val.DataContext =
            this.IntUD_CustomLvl8Val.DataContext =
            this.IntUD_CustomLvl9Val.DataContext =
            this.IntUD_Scales.DataContext =
            this.IntUD_Value.DataContext =
            this.DataContext;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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
