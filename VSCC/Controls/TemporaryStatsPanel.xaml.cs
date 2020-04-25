namespace VSCC.Controls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using VSCC.Controls.Dialogs;
    using VSCC.DataType;

    /// <summary>
    /// Interaction logic for TemporaryStatsPanel.xaml
    /// </summary>
    public partial class TemporaryStatsPanel : UserControl
    {
        private readonly Popup _parent;
        private readonly NotifyCollectionChangedEventHandler _handler;

        public UIElement PlacementTarget { get; set; }
        public double VerticalOffset { get; set; }
        public double HorizontalOffset { get; set; }
        public Rect PlacementRectangle { get; set; }
        public PlacementMode Placement { get; set; } = PlacementMode.Relative;
        public bool StaysOpen { get; set; }
        public CustomPopupPlacementCallback CustomPopupPlacementCallback { get; set; }
        public bool IsOpen { get; set; } = true;
        public ObservableCollection<StatModifier> ContextData { get; set; }

        public TemporaryStatsPanel(Popup parent, ObservableCollection<StatModifier> data)
        {
            this.InitializeComponent();
            this._parent = parent;
            this.ContextData = data;
            this._handler = new NotifyCollectionChangedEventHandler((o, e) => this.List_Modifiers.Items.Refresh());
            data.CollectionChanged += this._handler;
            this.List_Modifiers.ItemsSource = this.ContextData;
            this.List_Modifiers.Items.Refresh();
            this._parent.Closed += (o, e) => data.CollectionChanged -= this._handler;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;
            this._parent.IsOpen = false;
        }

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            this.StaysOpen = this._parent.StaysOpen = true;
            NewStatModifierDialog nsmd = new NewStatModifierDialog();
            if (nsmd.ShowDialog() ?? false)
            {
                this.ContextData.Add(new StatModifier(nsmd.IntUD_Value.Value ?? 0, nsmd.TB_Label.Text));
            }

            this.StaysOpen = this._parent.StaysOpen = false;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e) => this.ContextData.Remove((StatModifier)((Button)sender).DataContext);
    }
}
