namespace VSCC.Controls.Windows
{
    using System.Windows;
    using VSCC.Controls.Dialogs;
    using VSCC.Roll20.AdvancedIntegration;

    /// <summary>
    /// Interaction logic for ItemIntegrationWindow.xaml
    /// </summary>
    public partial class ItemIntegrationWindow : Window
    {
        public SimpleItemIntegration Integration { get; set; }

        public ItemIntegrationWindow(SimpleItemIntegration sii)
        {
            this.DataContext = this.Integration = sii;
            this.InitializeComponent();
            this.CB_ProfBonus.DataContext =
            this.CB_StrBonus.DataContext =
            this.CB_DexBonus.DataContext =
            this.CB_ConBonus.DataContext =
            this.CB_WisBonus.DataContext =
            this.CB_ChaBonus.DataContext =
            this.CB_IntBonus.DataContext =
            this.CB_DmgStrBonus.DataContext =
            this.CB_DmgDexBonus.DataContext =
            this.CB_DmgConBonus.DataContext =
            this.CB_DmgWisBonus.DataContext =
            this.CB_DmgChaBonus.DataContext =
            this.CB_DmgIntBonus.DataContext =
            this.LV_Dice.DataContext =
            this.IntUD_PP.DataContext = this.Integration;
        }

        // Add clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewDamageDieDialog nddd = new NewDamageDieDialog();
            if (nddd.ShowDialog() ?? false)
            {
                DamageLine dl = new DamageLine()
                {
                    DieSide = nddd.IntUD_DieSide.Value,
                    NumDice = nddd.IntUD_NumDice.Value,
                    ConstantNumber = nddd.IntUD_Constant.Value,
                    Label = nddd.TB_Label.Text
                };

                this.Integration.Damage.Add(dl);
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            DamageLine context = (DamageLine)((FrameworkElement)sender).DataContext;
            this.Integration.Damage.Remove(context);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
