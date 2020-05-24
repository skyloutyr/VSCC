namespace VSCC.Controls.Tabs
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.Models.ImageList;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for InventoryTab.xaml
    /// </summary>
    public partial class InventoryTab : UserControl
    {
        public ObservableCollection<InventoryItem> Items { get; set; } = new ObservableCollection<InventoryItem>();
        public ImageListModel Images { get; } = new ImageListModel();
        private bool _haltRefresh;

        public InventoryTab()
        {
            this.InitializeComponent();
            this.Images.LoadFromPhysicalFolder("./Images/Lists/Items");
            this.Items.CollectionChanged += (o, e) =>
            {
                if (!this._haltRefresh)
                {
                    this.Inventory.Items.Refresh();
                }
            };

            this.Inventory.ItemsSource = this.Items;
            this.Inventory.Items.Refresh();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource);
            view.Filter = this.Filter;
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
        }

        public bool Filter(object item) => string.IsNullOrEmpty(this.TextBox_Filter.Text) || ((InventoryItem)item).Name.IndexOf(this.TextBox_Filter.Text, StringComparison.OrdinalIgnoreCase) != -1;

        public void ChangeItemCollection(ObservableCollection<InventoryItem> collection)
        {
            this.Items = collection;
            foreach (InventoryItem ii in this.Items)
            {
                ii.ImageList = this.Images;
            }

            this.Items.CollectionChanged += (o, e) => this.Inventory.Items.Refresh();
            this.Inventory.ItemsSource = this.Items;
            this.Inventory.Items.Refresh();
            this.RecalculateWeights(true, false, false);
        }

        public void RecalculateWeights(bool min, bool max1, bool max2)
        {
            if (min)
            {
                float cw = 0;
                foreach (InventoryItem ii in this.Items)
                {
                    cw += ii.Weight * ii.Amount;
                }

                AppState.Current.State.Inventory.WeightCurrent = cw;
            }

            if (max1)
            {
                AppState.Current.State.Inventory.WeightMax1 = AppState.Current.State.General.StatStr * 15;
            }

            if (max2)
            {
                AppState.Current.State.Inventory.WeightMax2 = AppState.Current.State.General.StatStr * 30;
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            CreateIItemWindow ciiw = new CreateIItemWindow();
            ciiw.SetDataContext(new InventoryItem() { ImageList = this.Images });
            if (ciiw.ShowDialog() ?? false)
            {
                this.Items.Add((InventoryItem)ciiw.DataContext);
                AppState.Current.State.Inventory.WeightCurrent += ((InventoryItem)ciiw.DataContext).Weight * ((InventoryItem)ciiw.DataContext).Amount;
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e) =>
            this.KeyUp += (o, kea) =>
            {
                if (kea.Key == Key.Delete && this.Inventory.SelectedItems.Count > 0)
                {
                    this._haltRefresh = true;
                    foreach (InventoryItem ii in this.Inventory.SelectedItems)
                    {
                        this.Items.Remove(ii);
                    }

                    this._haltRefresh = false;
                    this.Inventory.Items.Refresh();
                }
            };

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this.Inventory.SelectedItem is InventoryItem ii)
            {
                CreateIItemWindow ciiw = new CreateIItemWindow();
                ciiw.SetDataContext(ii.Copy());
                if (ciiw.ShowDialog() ?? false)
                {
                    this.Items[this.Items.IndexOf(ii)] = (InventoryItem)ciiw.DataContext;
                    float weightDiff = (((InventoryItem)ciiw.DataContext).Weight * ((InventoryItem)ciiw.DataContext).Amount) - (ii.Weight * ii.Amount);
                    AppState.Current.State.Inventory.WeightCurrent += weightDiff;
                }
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (this.Inventory.SelectedItem is InventoryItem ii)
            {
                this.Items.Remove(ii);
                AppState.Current.State.Inventory.WeightCurrent -= ii.Weight * ii.Amount;
            }
        }

        private void TextBox_Filter_TextChanged(object sender, TextChangedEventArgs e) => CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource).Refresh();

        private void ChangeSortingMethod()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource);
            view.SortDescriptions.Clear();
            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Amount)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Amount", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Cost)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("CostSortProperty", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_CostTotal)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("CostTotalSortProperty", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Name)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Rarity)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Rarity", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Type)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Type", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Weight)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Weight", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_WeightTotal)
            {
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("WeightTotalSortProperty", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
            }

            CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource).Refresh();
        }

        private void ComboBox_SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized && this.Inventory != null)
            {
                this.ChangeSortingMethod();
            }
        }

        private void CheckBox_ReverseSearchResults_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized && this.Inventory != null)
            {
                this.ChangeSortingMethod();
            }
        }

        private void Btn_AddCash_Click(object sender, RoutedEventArgs e)
        {
            ChangeCashWindow ccw = new ChangeCashWindow();
            if (ccw.ShowDialog() ?? false)
            {
                this.IntUD_PP.Value = (this.IntUD_PP.Value ?? 0) + ccw.IntUD_PP.Value;
                this.IntUD_GP.Value = (this.IntUD_GP.Value ?? 0) + ccw.IntUD_GP.Value;
                this.IntUD_EP.Value = (this.IntUD_EP.Value ?? 0) + ccw.IntUD_EP.Value;
                this.IntUD_SP.Value = (this.IntUD_SP.Value ?? 0) + ccw.IntUD_SP.Value;
                this.IntUD_CP.Value = (this.IntUD_CP.Value ?? 0) + ccw.IntUD_CP.Value;
            }
        }

        private Point startPoint;
        private void Inventory_MouseDown(object sender, MouseButtonEventArgs e) => this.startPoint = e.GetPosition(null);

        private void Inventory_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = this.startPoint - mousePos;
            if ((e.LeftButton == MouseButtonState.Pressed && Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null && listViewItem.IsSelected)
                {
                    object contact = listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    DataObject dragData = new DataObject("InventoryItem", contact);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T t)
                {
                    return t;
                }

                current = VisualTreeHelper.GetParent(current);
            } while (current != null);

            return null;
        }

        private void Inventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Inventory.SelectedItems.Count > 0)
            {
                this.Btn_Edit_Click(null, new RoutedEventArgs());
            }
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e) => this.RecalculateWeights(true, false, false);
    }
}
