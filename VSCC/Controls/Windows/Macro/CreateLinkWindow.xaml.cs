namespace VSCC.Controls.Windows.Macro
{
    using System.Windows;
    using VSCC.DataType;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for CreateLinkWindow.xaml
    /// </summary>
    public partial class CreateLinkWindow : Window
    {
        public bool IsItemLink { get; set; }
        public string SuggestedName { get; set; }

        public CreateLinkWindow() => this.InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.TB_LinkName.Text = this.SuggestedName;
            if (this.IsItemLink)
            {
                foreach (InventoryItem ii in AppState.Current.State.Inventory.Items)
                {
                    this.CB_Value.Items.Add($"{ ii.Amount }x { ii.Name } |{ ii.ObjectID }");
                }
            }
            else
            {
                foreach (Spell s in AppState.Current.State.Spellbook.AllSpells)
                {
                    this.CB_Value.Items.Add($"{ s.Name } ({ s.Level })|{ s.ObjectID }");
                }
            }

            if (this.CB_Value.Items.Count == 0)
            {
                this.Close();
                MessageBox.Show("No items/spells to link!", "Invalid Action");
                return;
            }

            this.CB_Value.Text = this.CB_Value.Items[0].ToString();
            this.CB_Value.InvalidateVisual();
        }

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
