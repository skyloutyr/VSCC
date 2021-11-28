namespace VSCC.Controls.Tabs
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
    using System.Windows.Media;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.Models.ImageList;
    using VSCC.Roll20;
    using VSCC.Roll20.AdvancedIntegration;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for InventoryTab.xaml
    /// </summary>
    public partial class InventoryTab : UserControl
    {
        public ObservableCollection<InventoryItem> Items { get; set; } = new ObservableCollection<InventoryItem>();

        public ImageListModel Images { get; } = new ImageListModel();
        public List<ListView> ChildCollections { get; } = new List<ListView>();
        private bool _haltRefresh;

        public InventoryTab()
        {
            this.InitializeComponent();
            this.Images.LoadFromPhysicalFolder("./Images/Lists/Items");
            this.Items.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is InventoryItem f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                if (!this._haltRefresh)
                {
                    this.Inventory.Items.Refresh();
                    foreach (ListView collection in this.ChildCollections)
                    {
                        collection.Items.Refresh();
                    }
                }
            };

            this.Inventory.ItemsSource = this.Items;
            this.Inventory.Items.Refresh();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource);
            view.Filter = this.Filter;
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Name", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
        }

        public bool Filter(object item)
        {
            InventoryItem ii = (InventoryItem)item;
            return ii.ContainerID.Equals(Guid.Empty) && (string.IsNullOrEmpty(this.TextBox_Filter.Text) || ii.Name.IndexOf(this.TextBox_Filter.Text, StringComparison.OrdinalIgnoreCase) != -1);
        }

        public void ChangeItemCollection(ObservableCollection<InventoryItem> collection)
        {
            this.Items = collection;
            foreach (InventoryItem ii in this.Items)
            {
                ii.ImageList = this.Images;
            }

            this.Items.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is InventoryItem f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.Inventory.Items.Refresh();
                foreach (ListView icollection in this.ChildCollections)
                {
                    icollection.ItemsSource = this.Items;
                    icollection.Items.Refresh();
                }
            };

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
                    cw += ii.Weight * ii.Amount * ii.ItemWeightLogicMul;
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
                AppState.Current.State.Inventory.WeightCurrent += ((InventoryItem)ciiw.DataContext).Weight * ((InventoryItem)ciiw.DataContext).Amount * ((InventoryItem)ciiw.DataContext).ItemWeightLogicMul;
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.KeyUp += (o, kea) =>
            {
                if (kea.Key == Key.Delete && this.Inventory.SelectedItems.Count > 0)
                {
                    this._haltRefresh = true;
                    for (int i = this.Inventory.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        InventoryItem ii = (InventoryItem)this.Inventory.SelectedItems[i];
                        this.Items.Remove(ii);
                    }

                    this._haltRefresh = false;
                    this.Inventory.Items.Refresh();
                }
            };
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
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource);
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
                view.SortDescriptions.Add(item: new SortDescription("Type", this.CheckBox_ReverseSearchResults.IsChecked ?? false ? ListSortDirection.Descending : ListSortDirection.Ascending));
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

        private void Btn_AddCash_Click(object sender, RoutedEventArgs e)
        {
            ChangeCashWindow ccw = new ChangeCashWindow();
            if (ccw.ShowDialog() ?? false)
            {
                this.IntUD_PP.Value = this.IntUD_PP.Value + ccw.IntUD_PP.Value;
                this.IntUD_GP.Value = this.IntUD_GP.Value + ccw.IntUD_GP.Value;
                this.IntUD_EP.Value = this.IntUD_EP.Value + ccw.IntUD_EP.Value;
                this.IntUD_SP.Value = this.IntUD_SP.Value + ccw.IntUD_SP.Value;
                this.IntUD_CP.Value = this.IntUD_CP.Value + ccw.IntUD_CP.Value;
            }
        }

        private Point startPoint;
        private void Inventory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                return;
            }

            this.startPoint = Mouse.GetPosition(Application.Current.MainWindow);
        }

        private void Inventory_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                return;
            }

            Point mousePos = Mouse.GetPosition(Application.Current.MainWindow);
            Vector diff = this.startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed && ((Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null && listViewItem.IsSelected)
                {
                    object contact = listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    DataObject dragData = new DataObject("InventoryItem", new Tuple<object, object, ICollectionView>(contact, null, CollectionViewSource.GetDefaultView(this.Items)));
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        public static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
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
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                return;
            }

            if (this.Inventory.SelectedItems.Count > 0 && e.ChangedButton == MouseButton.Left)
            {
                this.Btn_Edit_Click(null, new RoutedEventArgs());
            }
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e) => this.RecalculateWeights(true, false, false);

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
                    ii.ContainerID = Guid.Empty;
                    ii.ImageList = this.Images;
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

        private void Btn_Cut_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Copy_Click(null, default);
            this.Btn_Delete_Click(null, default);
        }

        private void CommandBindingCopy_Executed(object sender, ExecutedRoutedEventArgs e) => this.Btn_Copy_Click(null, default);

        private void CommandBindingCut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Btn_Copy_Click(null, default);
            this.Btn_Delete_Click(null, default);
        }

        private void CommandBindingPaste_Executed(object sender, ExecutedRoutedEventArgs e) => this.Btn_Paste_Click(null, default);

        private InventoryItem _rmbTemporaryContext;
        private InventoryItem _lmbTemporaryContext;

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
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    this.EditItemR20(ii, new RoutedEventArgs());
                }
                else
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

            e.Handled = true;
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
                InventoryItem ii2 = (InventoryItem)data.Item2;
                ii.ContainerID = Guid.Empty;
                ii2?.OnPropertyChanged("Description");
                ii2?.OnPropertyChanged("HasItems");
                data.Item3.Refresh();
                CollectionViewSource.GetDefaultView(this.Inventory.ItemsSource).Refresh();
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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is InventoryItem ii)
            {
                this._lmbTemporaryContext = ii;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is InventoryItem ii && ii == this._lmbTemporaryContext)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && R20WSServer.Connected)
                {
                    this.RunItemR20(ii, new RoutedEventArgs());
                }
            }

            e.Handled = true;
        }


        // Handle all extra keyboard shortcuts
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control)) // New
            {
                this.Btn_Add_Click(this.Btn_Add, new RoutedEventArgs());
                e.Handled = true;
            }

            if (e.Key == Key.E && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control)) // Edit
            {
                this.Btn_Edit_Click(this.Btn_Edit, new RoutedEventArgs());
                e.Handled = true;
            }

            if (e.Key == Key.F5) // Refresh
            {
                this.Btn_Refresh_Click(this.Btn_Refresh, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void EditItemR20(object sender, RoutedEventArgs e)
        {
            if (sender is Button) // Click
            {
                if (this.Inventory.SelectedItems != null && this.Inventory.SelectedItems.Count > 0)
                {
                    InventoryItem ii = (InventoryItem)this.Inventory.SelectedItems[0];
                    SimpleItemIntegration clone = ii.Integration?.Copy() ?? new SimpleItemIntegration();
                    ItemIntegrationWindow iiw = new ItemIntegrationWindow(clone)
                    {
                        Title = ii.Name
                    };

                    if (iiw.ShowDialog() ?? false)
                    {
                        ii.Integration = iiw.Integration;
                    }
                }
            }
            else
            {
                if (sender is InventoryItem ii)
                {
                    SimpleItemIntegration clone = ii.Integration?.Copy() ?? new SimpleItemIntegration();
                    ItemIntegrationWindow iiw = new ItemIntegrationWindow(clone)
                    {
                        Title = ii.Name
                    };

                    if (iiw.ShowDialog() ?? false)
                    {
                        ii.Integration = iiw.Integration;
                    }
                }
            }
        }

        private void RunItemR20(object sender, RoutedEventArgs e)
        {
            InventoryItem ii = sender as InventoryItem;
            if (sender is Button && this.Inventory.SelectedItems != null && this.Inventory.SelectedItems.Count > 0)
            {
                ii = (InventoryItem)this.Inventory.SelectedItems[0];
            }

            if (ii == null || !R20WSServer.Connected)
            {
                return;
            }

            RunItemsRoll20Integration(ii);
        }

        public static void RunItemsRoll20Integration(InventoryItem ii)
        {
            SimpleItemIntegration sii = ii.Integration;
            if (sii != null)
            {
                // Determine integration type
                if (sii.HitDieSide <= 0) // No hit die
                {
                    if (sii.Damage.Count == 0) // No damage, description
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.Description,
                            Data = new TemplateDataDesc
                            {
                                Desc = "** " + ii.Name + " **\n" + ii.Description
                            }
                        });
                    }
                    else // Have damage, but no hit die
                    {
                        string damageString = "[[";
                        for (int i = 0; i < sii.Damage.Count; i++)
                        {
                            DamageLine dl = sii.Damage[i];
                            if (dl.DieSide > 0 && dl.ConstantNumber != 0)
                            {
                                damageString += $"[[[[{dl.NumDice}d{dl.DieSide}]] + {dl.ConstantNumber}]][{dl.Label}]";
                            }
                            else
                            {
                                if (dl.DieSide > 0)
                                {
                                    damageString += $"[[{dl.NumDice}d{dl.DieSide}]][{dl.Label}]";
                                }
                                else
                                {
                                    damageString += $"{dl.ConstantNumber}[{dl.Label}]";
                                }
                            }

                            if (i != sii.Damage.Count - 1)
                            {
                                damageString += "+";
                            }
                        }

                        if (sii.DamageIncludeStr)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_Str")}]";
                        }

                        if (sii.DamageIncludeDex)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModDex}[{MainWindow.Translate("General_Dex")}]";
                        }

                        if (sii.DamageIncludeCon)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModCon}[{MainWindow.Translate("General_Con")}]";
                        }

                        if (sii.DamageIncludeWis)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModWis}[{MainWindow.Translate("General_Wis")}]";
                        }

                        if (sii.DamageIncludeCha)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModCha}[{MainWindow.Translate("General_Cha")}]";
                        }

                        if (sii.DamageIncludeInt)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModInt}[{MainWindow.Translate("General_Int")}]";
                        }

                        damageString += "]]";
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.Dmg,
                            Data = new TemplateDataDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Name = ii.Name,
                                Dmg1 = damageString,
                                Dmg2 = damageString
                            }
                        });
                    }
                }
                else // Have hit die
                {
                    string hitString = $"[[[[1d{sii.HitDieSide}]][1d{sii.HitDieSide}]";
                    if (sii.HitConstant > 0)
                    {
                        hitString += $"+{sii.HitConstant}";
                    }

                    if (sii.HitIncludeProfficiency)
                    {
                        hitString += $"+{AppState.Current.State.General.ProfficiencyBonus}[{MainWindow.Translate("General_PBonus")}]";
                    }

                    if (sii.HitIncludeStr)
                    {
                        hitString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_Str")}]";
                    }

                    if (sii.HitIncludeDex)
                    {
                        hitString += $"+{AppState.Current.State.General.StatModDex}[{MainWindow.Translate("General_Dex")}]";
                    }

                    if (sii.HitIncludeCon)
                    {
                        hitString += $"+{AppState.Current.State.General.StatModCon}[{MainWindow.Translate("General_Con")}]";
                    }

                    if (sii.HitIncludeWis)
                    {
                        hitString += $"+{AppState.Current.State.General.StatModWis}[{MainWindow.Translate("General_Wis")}]";
                    }

                    if (sii.HitIncludeCha)
                    {
                        hitString += $"+{AppState.Current.State.General.StatModCha}[{MainWindow.Translate("General_Cha")}]";
                    }

                    if (sii.HitIncludeInt)
                    {
                        hitString += $"+{AppState.Current.State.General.StatModInt}[{MainWindow.Translate("General_Int")}]";
                    }

                    hitString += "]]";
                    if (sii.Damage.Count == 0) // Have no damage die
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.Simple,
                            Data = new TemplateDataSimple
                            {
                                CharName = AppState.Current.State.General.Name,
                                Name = ii.Name,
                                R1 = hitString,
                                R2 = hitString
                            }
                        });
                    }
                    else // Have damage die
                    {
                        string damageString = "[[";
                        string critString = "[[";
                        for (int i = 0; i < sii.Damage.Count; i++)
                        {
                            DamageLine dl = sii.Damage[i];
                            if (dl.DieSide > 0 && dl.ConstantNumber != 0)
                            {
                                damageString += $"[[[[{dl.NumDice}d{dl.DieSide}]][{dl.NumDice}d{dl.DieSide}] + {dl.ConstantNumber}]][{dl.Label}]";
                                critString += $"[[{dl.NumDice}d{dl.DieSide}]][{dl.NumDice}d{dl.DieSide} {dl.Label}]";
                            }
                            else
                            {
                                if (dl.DieSide > 0)
                                {
                                    damageString += $"[[{dl.NumDice}d{dl.DieSide}]][{dl.NumDice}d{dl.DieSide} {dl.Label}]";
                                    critString += $"[[{dl.NumDice}d{dl.DieSide}]][{dl.NumDice}d{dl.DieSide} {dl.Label}]";
                                }
                                else
                                {
                                    damageString += $"{dl.ConstantNumber}[{dl.Label}]";
                                }
                            }

                            if (i != sii.Damage.Count - 1)
                            {
                                damageString += "+";
                                critString += "+";
                            }
                        }

                        while (critString[critString.Length - 1] == '+')
                        {
                            critString = critString.Substring(0, critString.Length - 1);
                        }

                        critString += "]]";
                        if (sii.DamageIncludeStr)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_Str")}]";
                        }

                        if (sii.DamageIncludeDex)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModDex}[{MainWindow.Translate("General_Dex")}]";
                        }

                        if (sii.DamageIncludeCon)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModCon}[{MainWindow.Translate("General_Con")}]";
                        }

                        if (sii.DamageIncludeWis)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModWis}[{MainWindow.Translate("General_Wis")}]";
                        }

                        if (sii.DamageIncludeCha)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModCha}[{MainWindow.Translate("General_Cha")}]";
                        }

                        if (sii.DamageIncludeInt)
                        {
                            damageString += $"+{AppState.Current.State.General.StatModInt}[{MainWindow.Translate("General_Int")}]";
                        }

                        damageString += "]]";
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = critString,
                                Dmg = damageString,
                                R1 = hitString,
                                R2 = hitString,
                                Name = ii.Name
                            }
                        });
                    }
                }
            }
        }
    }
}
