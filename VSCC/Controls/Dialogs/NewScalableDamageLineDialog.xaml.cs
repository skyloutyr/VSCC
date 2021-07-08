namespace VSCC.Controls.Dialogs
{
    using System.Windows;
    using System.Windows.Controls;
    using VSCC.Roll20.AdvancedIntegration;

    /// <summary>
    /// Interaction logic for NewScalableDamageLineDialog.xaml
    /// </summary>
    public partial class NewScalableDamageLineDialog : Window
    {
        public ScalableDamageLine Value { get; set; }

        public NewScalableDamageLineDialog(ScalableDamageLine die)
        {
            this.DataContext = this.Value = die;
            this.InitializeComponent();
            this.Btn_Constant.DataContext =
            this.Btn_DieSide.DataContext =
            this.Btn_NumDice.DataContext =
            this.TB_Label.DataContext =
            this.Value;
        }

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

        private void Btn_NumDice_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Value.Die.NumDice.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Value.Die.NumDice = sv;
            }

            this.Btn_NumDice.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }

        private void Btn_DieSide_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Value.Die.DieSide.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Value.Die.DieSide = sv;
            }


            this.Btn_DieSide.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }

        private void Btn_Constant_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Value.ConstantNumber.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Value.ConstantNumber = sv;
            }

            this.Btn_Constant.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }
    }
}
