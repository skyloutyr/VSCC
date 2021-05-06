namespace VSCC.Controls.Windows
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using VSCC.Controls.Tabs;
    using VSCC.DataType;
    using VSCC.Models.ImageList;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for InventoryContainerWindow.xaml
    /// </summary>
    public partial class InventoryContainerWindow : Window
    {
        public ObservableCollection<InventoryItem> Items => AppState.Current.TInventory.Items;
        public ImageListModel Images => AppState.Current.TInventory.Images;
        public CollectionViewSource LocalSource = new CollectionViewSource();

        public InventoryItem OwnerItem { get; set; }

        public InventoryContainerWindow(InventoryItem owner)
        {
            this.OwnerItem = owner;
            this.InitializeComponent();
            this.Closed += this.InventoryContainerWindow_Closed;
            InventoryTab itab = AppState.Current.TInventory;
            itab.ChildCollections.Add(this.Inventory);
            this.LocalSource = new CollectionViewSource
            {
                Source = itab.Items
            };

            this.Inventory.ItemsSource = this.LocalSource.View;
            this.Inventory.Items.Refresh();
            this.LocalSource.Filter += this.LocalSource_Filter;
            this.LocalSource.SortDescriptions.Clear();
            this.LocalSource.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending));
        }

        private void LocalSource_Filter(object sender, FilterEventArgs e) => e.Accepted = this.Filter(e.Item);

        private void InventoryContainerWindow_Closed(object sender, EventArgs e)
        {
            AppState.Current.TInventory.ChildCollections.Remove(this.Inventory);
            AppState.Current.Window.Focus();
        }

        public bool Filter(object item)
        {
            InventoryItem ii = (InventoryItem)item;
            return ii.ContainerID.Equals(this.OwnerItem.ObjectID) && (string.IsNullOrEmpty(this.TextBox_Filter.Text) || ii.Name.IndexOf(this.TextBox_Filter.Text, StringComparison.OrdinalIgnoreCase) != -1);
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            CreateIItemWindow ciiw = new CreateIItemWindow();
            ciiw.SetDataContext(new InventoryItem() { ImageList = this.Images });
            if (ciiw.ShowDialog() ?? false)
            {
                ((InventoryItem)ciiw.DataContext).ContainerID = this.OwnerItem.ObjectID;
                this.Items.Add((InventoryItem)ciiw.DataContext);
                AppState.Current.State.Inventory.WeightCurrent += ((InventoryItem)ciiw.DataContext).Weight * ((InventoryItem)ciiw.DataContext).Amount * ((InventoryItem)ciiw.DataContext).ItemWeightLogicMul;
            }
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this.Inventory.SelectedItem is InventoryItem ii)
            {
                CreateIItemWindow ciiw = new CreateIItemWindow();
                ciiw.SetDataContext(ii.Copy());
                if (ciiw.ShowDialog() ?? false)
                {
                    this.Items[this.Items.IndexOf(ii)] = (InventoryItem)ciiw.DataContext;
                    float weightDiff = (((InventoryItem)ciiw.DataContext).Weight * ((InventoryItem)ciiw.DataContext).Amount * ((InventoryItem)ciiw.DataContext).ItemWeightLogicMul) - (ii.Weight * ii.Amount * ii.ItemWeightLogicMul);
                    AppState.Current.State.Inventory.WeightCurrent += weightDiff;
                }
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (object o in this.Inventory.SelectedItems.AsQueryable().Cast<object>().ToList().AsReadOnly())
            {
                if (o is InventoryItem ii)
                {
                    this.DeleteItem(ii);
                }
            }

            this.Inventory.Items.Refresh();
        }

        private void DeleteItem(InventoryItem ii)
        {
            this.Items.Remove(ii);
            AppState.Current.State.Inventory.WeightCurrent -= ii.Weight * ii.Amount * ii.ItemWeightLogicMul;
            Stack<InventoryItem> itorem = new Stack<InventoryItem>();
            foreach (InventoryItem item in this.Items)
            {
                if (item.ContainerID.Equals(ii.ObjectID))
                {
                    itorem.Push(item);
                }
            }

            if (itorem.Count > 0)
            {
                MessageBoxResult mbr = MessageBox.Show(MainWindow.Translate("Message_ContainerWithItems_Desc"), MainWindow.Translate("Message_ContainerWithItems_Title"), MessageBoxButton.YesNo);
                if (mbr.HasFlag(MessageBoxResult.Yes))
                {
                    while (itorem.Count > 0)
                    {
                        InventoryItem item = itorem.Pop();
                        this.DeleteItem(item);
                    }
                }
                else
                {
                    InventoryItem item = itorem.Pop();
                    item.ContainerID = Guid.Empty;
                }
            }
        }

        private void TextBox_Filter_TextChanged(object sender, TextChangedEventArgs e) => CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource).Refresh();

        private void ChangeSortingMethod()
        {
            CollectionViewSource view = this.LocalSource;
            view.SortDescriptions.Clear();
            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Amount)
            {
                view.SortDescriptions.Add(new SortDescription("Amount", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Cost)
            {
                view.SortDescriptions.Add(new SortDescription("CostSortProperty", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_CostTotal)
            {
                view.SortDescriptions.Add(new SortDescription("CostTotalSortProperty", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Name)
            {
                view.SortDescriptions.Add(new SortDescription("Name", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Rarity)
            {
                view.SortDescriptions.Add(new SortDescription("Rarity", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Type)
            {
                view.SortDescriptions.Add(new SortDescription("Type", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_Weight)
            {
                view.SortDescriptions.Add(new SortDescription("Weight", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            if (this.ComboBox_SortBy.SelectedItem == this.SortBy_WeightTotal)
            {
                view.SortDescriptions.Add(new SortDescription("WeightTotalSortProperty", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
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

        private Point startPoint;
        private void Inventory_MouseDown(object sender, MouseButtonEventArgs e) => this.startPoint = Mouse.GetPosition(Application.Current.MainWindow);

        private void Inventory_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = Mouse.GetPosition(Application.Current.MainWindow);
            Vector diff = this.startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed && ((Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = InventoryTab.FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null && listViewItem.IsSelected)
                {
                    object contact = listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    DataObject dragData = new DataObject("InventoryItem", new Tuple<object, object, ICollectionView>(contact, this.OwnerItem, this.LocalSource.View));
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }


        private void Inventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Inventory.SelectedItems.Count > 0 && e.ChangedButton == MouseButton.Left)
            {
                this.Btn_Edit_Click(null, new RoutedEventArgs());
            }
        }

        private void Btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            List<InventoryItem> copied = new List<InventoryItem>();
            foreach (object o in this.Inventory.SelectedItems)
            {
                if (o is InventoryItem ii)
                {
                    copied.Add(ii);
                }
            }

            if (copied.Count > 0)
            {
                List<string> texts = new List<string>();
                foreach (InventoryItem ii in copied)
                {
                    texts.Add(ii.ToShareString());
                }

                Clipboard.SetText(string.Join(new string((char)0x1D, 1), texts));
            }
        }

        private void Btn_Paste_Click(object sender, RoutedEventArgs e)
        {
            string text = Clipboard.GetText();
            try
            {
                foreach (string line in text.Split((char)0x1D))
                {
                    JObject item = JObject.Parse(line);
                    InventoryItem ii = this.LoadItemFromJSON(item);
                    ii.ImageList = this.Images;
                    ii.ContainerID = this.OwnerItem.ObjectID;
                    this.Items.Add(ii);
                }

                this.Inventory.Items.Refresh();
            }
            catch (Exception)
            {
                // NOOP
            }
        }

        private InventoryItem LoadItemFromJSON(JObject item)
        {
            JObject mitem = item["main_item"] as JObject;
            InventoryItem ii = mitem.ToObject<InventoryItem>();
            Guid oid = Guid.NewGuid();
            ii.ObjectID = oid;
            if (item["secondary_items"] is JArray sitems)
            {
                foreach (JToken token in sitems)
                {
                    if (token is JObject jo)
                    {
                        InventoryItem secItem = this.LoadItemFromJSON(jo);
                        secItem.ContainerID = oid;
                        this.Items.Add(secItem);
                    }
                }
            }

            return ii;
        }

        private void CommandBindingCopy_Executed(object sender, ExecutedRoutedEventArgs e) => this.Btn_Copy_Click(null, default);

        private void CommandBindingCut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Btn_Copy_Click(null, default);
            this.Btn_Delete_Click(null, default);
        }

        private void CommandBindingPaste_Executed(object sender, ExecutedRoutedEventArgs e) => this.Btn_Paste_Click(null, default);

        private InventoryItem _rmbTemporaryContext;

        private void InventoryItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is InventoryItem ii)
            {
                this._rmbTemporaryContext = ii;
            }
        }

        private void InventoryItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is InventoryItem ii && ii == this._rmbTemporaryContext)
            {
                Point relativePoint = g.PointToScreen(new Point(0, 0));
                InventoryContainerWindow icw = new InventoryContainerWindow(ii)
                {
                    Owner = AppState.Current.Window,
                    Topmost = false,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Left = relativePoint.X + 64,
                    Top = relativePoint.Y + 48,
                    Title = ii.Name
                };

                icw.Show();
            }
        }

        private void Inventory_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("InventoryItem"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void Inventory_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("InventoryItem"))
            {
                Tuple<object, object, ICollectionView> data = (Tuple<object, object, ICollectionView>)e.Data.GetData("InventoryItem");
                InventoryItem ii = (InventoryItem)data.Item1;
                ii.ContainerID = this.OwnerItem.ObjectID;
                this.OwnerItem.OnPropertyChanged("Description");
                this.OwnerItem.OnPropertyChanged("HasItems");
                AppState.Current.TInventory.Inventory.Items.Refresh();
                this.LocalSource.View.Refresh();
                data.Item3.Refresh();
                e.Handled = true;
            }
        }

        private void ButtonItemAmountUp_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            InventoryItem ii = (InventoryItem)b.DataContext;
            ii.Amount += 1;
            AppState.Current.State.Inventory.WeightCurrent += ii.Weight * ii.ItemWeightLogicMul;
            e.Handled = true;
        }

        private void ButtonItemAmountDown_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            InventoryItem ii = (InventoryItem)b.DataContext;
            ii.Amount -= 1;
            AppState.Current.State.Inventory.WeightCurrent -= ii.Weight * ii.ItemWeightLogicMul;
            e.Handled = true;
        }
    }
}
