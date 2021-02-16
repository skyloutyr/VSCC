namespace VSCC.Controls.Tabs
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.Models.ImageList;

    /// <summary>
    /// Interaction logic for ExtrasTab.xaml
    /// </summary>
    public partial class ExtrasTab : UserControl
    {
        private bool _haltRefresh;

        public ImageListModel Images { get; } = new ImageListModel();

        public ObservableCollection<Feat> Feats { get; set; } = new ObservableCollection<Feat>();
        public ObservableCollection<Feat> Traits { get; set; } = new ObservableCollection<Feat>();

        public ExtrasTab()
        {
            this.InitializeComponent();
            this.Images.LoadFromPhysicalFolder("./Images/Lists/Skills");
            this.Feats.CollectionChanged += (o, e) =>
            {
                if (!this._haltRefresh)
                {
                    this.LV_Feats.Items.Refresh();
                }
            };

            this.Traits.CollectionChanged += (o, e) =>
            {
                if (!this._haltRefresh)
                {
                    this.LV_Traits.Items.Refresh();
                }
            };

            this.LV_Feats.ItemsSource = this.Feats;
            this.LV_Feats.Items.Refresh();
            this.LV_Traits.ItemsSource = this.Traits;
            this.LV_Traits.Items.Refresh();
        }

        private void Feats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView lv && lv.SelectedItems.Count > 0 && !(Mouse.DirectlyOver is Button))
            {
                this.Btn_FeatEdit_Click(lv == this.LV_Feats ? this.Btn_FeatEdit : this.Btn_TraitEdit, new System.Windows.RoutedEventArgs());
            }
        }

        public void ChangeFeatCollection(ObservableCollection<Feat> collection)
        {
            this.Feats = collection;
            foreach (Feat f in this.Feats)
            {
                f.ImageList = this.Images;
            }

            this.Feats.CollectionChanged += (o, e) =>
            {
                this.LV_Feats.Items.Refresh();
            };

            this.LV_Feats.ItemsSource = this.Feats;
            this.LV_Feats.Items.Refresh();
        }

        public void ChangeTraitCollection(ObservableCollection<Feat> collection)
        {
            this.Traits = collection;
            foreach (Feat f in this.Traits)
            {
                f.ImageList = this.Images;
            }

            this.Traits.CollectionChanged += (o, e) =>
            {
                this.LV_Traits.Items.Refresh();
            };

            this.LV_Traits.ItemsSource = this.Feats;
            this.LV_Traits.Items.Refresh();
        }

        private void Btn_FeatNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ObservableCollection<Feat> addedTo = btn.Parent == this.Grid_Feats ? this.Feats : this.Traits;
            CreateFeatWindow cfw = new CreateFeatWindow();
            cfw.SetDataContext(new Feat() { ImageList = this.Images });
            if (cfw.ShowDialog() ?? false)
            {
                addedTo.Add((Feat)cfw.DataContext);
            }
        }

        private void Btn_FeatEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ObservableCollection<Feat> addedTo = btn.Parent == this.Grid_Feats ? this.Feats : this.Traits;
            ListView editedFrom = btn.Parent == this.Grid_Feats ? this.LV_Feats : this.LV_Traits;
            if (editedFrom.SelectedItem is Feat f)
            {
                CreateFeatWindow cfw = new CreateFeatWindow();
                cfw.SetDataContext(f.Copy());
                if (cfw.ShowDialog() ?? false)
                {
                    addedTo[addedTo.IndexOf(f)] = (Feat)cfw.DataContext;
                }
            }
        }

        private void Btn_FeatCopy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ListView editedFrom = btn.Parent == this.Grid_Feats ? this.LV_Feats : this.LV_Traits;
            List<Feat> copied = new List<Feat>();
            foreach (object o in editedFrom.SelectedItems)
            {
                if (o is Feat f)
                {
                    copied.Add(f);
                }
            }

            if (copied.Count > 0)
            {
                List<string> texts = new List<string>();
                foreach (Feat f in copied)
                {
                    texts.Add(f.ToShareString());
                }

                Clipboard.SetText(string.Join(new string((char)0x1D, 1), texts));
            }
        }

        private void Btn_FeatPaste_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ObservableCollection<Feat> addedTo = btn.Parent == this.Grid_Feats ? this.Feats : this.Traits;
            ListView editedFrom = btn.Parent == this.Grid_Feats ? this.LV_Feats : this.LV_Traits;
            string text = Clipboard.GetText();
            try
            {
                foreach (string line in text.Split((char)0x1D))
                {
                    JObject item = JObject.Parse(line);
                    Feat f = this.LoadFeatFromJSON(item);
                    f.ImageList = this.Images;
                    addedTo.Add(f);
                }

                editedFrom.Items.Refresh();
            }
            catch (Exception)
            {
                // NOOP
            }
        }

        private Feat LoadFeatFromJSON(JObject item)
        {
            JObject mitem = item["main_item"] as JObject;
            Feat f = mitem.ToObject<Feat>();
            Guid oid = Guid.NewGuid();
            f.ObjectID = oid;
            return f;
        }

        private void Btn_FeatDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ObservableCollection<Feat> addedTo = btn.Parent == this.Grid_Feats ? this.Feats : this.Traits;
            ListView editedFrom = btn.Parent == this.Grid_Feats ? this.LV_Feats : this.LV_Traits;
            foreach (object o in editedFrom.SelectedItems.AsQueryable().Cast<object>().ToList().AsReadOnly())
            {
                if (o is Feat f)
                {
                    addedTo.Remove(f);
                }
            }

            editedFrom.Items.Refresh();
        }

        private void Btn_Value2Max_Click(object sender, RoutedEventArgs e)
        {
            Feat f = (Feat)((Button)sender).DataContext;
            f.ValueProperty = f.ValueMaxProperty;
        }

        private void Btn_ValueIncrement_Click(object sender, RoutedEventArgs e)
        {
            Feat f = (Feat)((Button)sender).DataContext;
            f.ValueProperty = Math.Min(f.ValueMaxProperty, f.ValueProperty + 1);
        }

        private void Btn_ValueDecrement_Click(object sender, RoutedEventArgs e)
        {
            Feat f = (Feat)((Button)sender).DataContext;
            f.ValueProperty = Math.Max(0, f.ValueProperty - 1);
            Button sb = (Button)sender;

        }

        private void Btn_Value2Zero_Click(object sender, RoutedEventArgs e)
        {
            Feat f = (Feat)((Button)sender).DataContext;
            f.ValueProperty = 0;
        }
    }
}
