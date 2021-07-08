namespace VSCC.Controls.Windows
{
    using System.Windows;
    using System.Windows.Controls;
    using VSCC.Controls.Dialogs;
    using VSCC.Roll20.AdvancedIntegration;

    /// <summary>
    /// Interaction logic for SpellIntegrationWindow.xaml
    /// </summary>
    public partial class SpellIntegrationWindow : Window
    {
        public SimpleSpellIntegration Integration { get; set; }

        public SpellIntegrationWindow(SimpleSpellIntegration ssi)
        {
            this.DataContext = this.Integration = ssi;
            this.InitializeComponent();
            this.CB_ShowSpellCard.DataContext =
            this.CB_IsSaveDC.DataContext =
            this.CB_ProfBonus.DataContext =
            this.CB_SpellcastingBonus.DataContext =
            this.CB_StrBonus.DataContext =
            this.CB_DexBonus.DataContext =
            this.CB_ConBonus.DataContext =
            this.CB_WisBonus.DataContext =
            this.CB_ChaBonus.DataContext =
            this.CB_IntBonus.DataContext =
            this.CB_DmgProfBonus.DataContext =
            this.CB_DmgSpellcastingBonus.DataContext =
            this.CB_DmgStrBonus.DataContext =
            this.CB_DmgDexBonus.DataContext =
            this.CB_DmgConBonus.DataContext =
            this.CB_DmgWisBonus.DataContext =
            this.CB_DmgChaBonus.DataContext =
            this.CB_DmgIntBonus.DataContext =
            this.LV_Dice.DataContext =
            this.Btn_ScalableHitDieNum.DataContext =
            this.Btn_ScalableHitDieSide.DataContext =
            this.Btn_ScalableSaveConstant.DataContext =
            this.ComboBox_SpellcastingAbility.DataContext =
            this.Btn_ScalableHitConstant.DataContext = this.Integration;
            this.ComboBox_SpellcastingAbility.Text = this.Integration.SaveAttr;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            ScalableDamageLine context = (ScalableDamageLine)((FrameworkElement)sender).DataContext;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScalableDamageLine sdl = new ScalableDamageLine() { Die = new ScalableDie(), ConstantNumber = new ScalableValue() };
            NewScalableDamageLineDialog nsdld = new NewScalableDamageLineDialog(sdl);
            if (nsdld.ShowDialog() ?? false)
            {
                this.Integration.Damage.Add(sdl);
            }
        }

        private void Btn_ScalableHitDieNum_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Integration.HitDie.NumDice.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Integration.HitDie.NumDice = sv;
            }

            this.Btn_ScalableHitDieNum.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }

        private void Btn_ScalableHitDieSide_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Integration.HitDie.DieSide.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Integration.HitDie.DieSide = sv;
            }

            this.Btn_ScalableHitDieSide.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }

        private void Btn_ScalableHitConstant_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Integration.HitConstant.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Integration.HitConstant = sv;
            }

            this.Btn_ScalableHitConstant.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }

        private void Btn_ScalableSaveConstant_Click(object sender, RoutedEventArgs e)
        {
            ScalableValue sv = this.Integration.SaveConstant.Copy();
            ScalableValueDialog svd = new ScalableValueDialog(sv);
            if (svd.ShowDialog() ?? false)
            {
                this.Integration.SaveConstant = sv;
            }

            this.Btn_ScalableSaveConstant.GetBindingExpression(Button.ContentProperty).UpdateTarget();
        }

        private void ComboBox_SpellcastingAbility_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.Integration.SaveAttr = ((ComboBoxItem)this.ComboBox_SpellcastingAbility.Items[this.ComboBox_SpellcastingAbility.SelectedIndex]).Content.ToString();
        }
    }
}
