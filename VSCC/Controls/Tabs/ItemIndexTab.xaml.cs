﻿namespace VSCC.Controls.Tabs
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.State;
    using VSCC.Templates;

    /// <summary>
    /// Interaction logic for ItemIndexTab.xaml
    /// </summary>
    public partial class ItemIndexTab : UserControl
    {
        public ICommand ToItemCommand { get; set; }
        public List<ItemTemplate> AllItemTemplates { get; } = new List<ItemTemplate>();

        public ScrollViewer ScrollViewer_Items => GetChildOfType<ScrollViewer>(this.ListView_ItemTemplates);

        public ItemIndexTab()
        {
            this.InitializeComponent();
            this.ToItemCommand = new CommandToItem();
            this.ListView_ItemTemplates.ItemsSource = this.AllItemTemplates;
            string database = Properties.Resources.dnd5eitemindex;
            this.AllItemTemplates.AddRange(JsonConvert.DeserializeObject<ItemTemplate[]>(database));
            this.ListView_ItemTemplates.Items.Refresh();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.ListView_ItemTemplates.ItemsSource);
            view.Filter = this.Filter;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => CollectionViewSource.GetDefaultView(this.ListView_ItemTemplates.ItemsSource).Refresh();

        private bool Filter(object item)
        {
            if (!this.IsInitialized)
            {
                return true;
            }

            ItemTemplate template = (ItemTemplate)item;
            if (!string.IsNullOrEmpty(this.TextBox_Filter.Text) && template.Name.IndexOf(this.TextBox_Filter.Text, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            bool rarityQualifies = !((this.CB_Rarity_Common.IsChecked ?? false) || (this.CB_Rarity_Uncommon.IsChecked ?? false) || (this.CB_Rarity_Rare.IsChecked ?? false) || (this.CB_Rarity_VeryRare.IsChecked ?? false) || (this.CB_Rarity_Legendary.IsChecked ?? false));
            if (!rarityQualifies && (this.CB_Rarity_Common.IsChecked ?? false))
            {
                if (!string.IsNullOrEmpty(template.ItemRarity) && template.ItemRarity.Equals(Properties.Resources.ItemIndex_Rarity_Common, StringComparison.OrdinalIgnoreCase))
                {
                    rarityQualifies = true;
                }
            }

            if (!rarityQualifies && (this.CB_Rarity_Uncommon.IsChecked ?? false))
            {
                if (Properties.Resources.ItemIndex_Rarity_Uncommon.Equals(template.ItemRarity, StringComparison.OrdinalIgnoreCase))
                {
                    rarityQualifies = true;
                }
            }

            if (!rarityQualifies && (this.CB_Rarity_Rare.IsChecked ?? false))
            {
                if (Properties.Resources.ItemIndex_Rarity_Rare.Equals(template.ItemRarity, StringComparison.OrdinalIgnoreCase))
                {
                    rarityQualifies = true;
                }
            }

            if (!rarityQualifies && (this.CB_Rarity_VeryRare.IsChecked ?? false))
            {
                if (Properties.Resources.ItemIndex_Rarity_VeryRare.Equals(template.ItemRarity, StringComparison.OrdinalIgnoreCase))
                {
                    rarityQualifies = true;
                }
            }

            if (!rarityQualifies && (this.CB_Rarity_Legendary.IsChecked ?? false))
            {
                if (Properties.Resources.ItemIndex_Rarity_Legendary.Equals(template.ItemRarity, StringComparison.OrdinalIgnoreCase))
                {
                    rarityQualifies = true;
                }
            }

            if (!rarityQualifies)
            {
                return false;
            }

            if ((this.CB_WondrousOnly.IsChecked ?? false) && !string.IsNullOrEmpty(template.Subtitle) && template.Subtitle.IndexOf(VSCC.Properties.Resources.Generic_Wondrous, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.TextBox_TypeFilter.Text))
            {
                if (string.IsNullOrEmpty(template.Subtitle))
                {
                    return false;
                }

                bool hasAny = false;
                foreach (string s in this.TextBox_TypeFilter.Text.Split(new string[] { "\n", "\r\n" }, int.MaxValue, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (template.Subtitle.ToLower().IndexOf(s.ToLower()) != -1)
                    {
                        hasAny = true;
                        break;
                    }
                }

                if (!hasAny)
                {
                    return false;
                }
            }

            int boxes_checked = 0;
            int matches = 0;
            foreach (CheckBox cb in new[] { this.CheckBox_Property_Light, this.CheckBox_Property_Heavy, this.CheckBox_Property_Ammo, this.CheckBox_Property_Finesse, this.CheckBox_Property_Loading, this.CheckBox_Property_Range, this.CheckBox_Property_Reach, this.CheckBox_Property_Thrown, this.CheckBox_Property_TwoHanded, this.CheckBox_Property_Versatile })
            {
                if (cb.IsChecked ?? false)
                {
                    ++boxes_checked;
                    if (!string.IsNullOrEmpty(template.Properties) && template.Properties.ToLower().IndexOf(((string)cb.Content).ToLower()) != -1)
                    {
                        ++matches;
                    }
                }
            }

            return matches > 0 || boxes_checked <= 0;
        }

        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                T result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }

    internal class CommandToItem : ICommand
    {
#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            ItemTemplate it = (ItemTemplate)parameter;
            InventoryItem ii = new InventoryItem(it)
            {
                ImageList = AppState.Current.TInventory.Images
            };

            CreateIItemWindow cciw = new CreateIItemWindow();
            cciw.SetDataContext(ii);
            if (cciw.ShowDialog() ?? false)
            {
                AppState.Current.TInventory.Items.Add((InventoryItem)cciw.DataContext);
            }
        }
    }
}
