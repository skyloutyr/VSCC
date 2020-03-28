using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VSCC.Controls.Windows;
using VSCC.DataType;
using VSCC.Models.ImageList;
using VSCC.State;
using VSCC.Templates;

namespace VSCC.Controls.Tabs
{
    public partial class SpellIndexTab : UserControl
    {
        public ICommand ToSpellCommand { get; set; }
        public List<SpellTemplate> AllSpellTemplates { get; } = new List<SpellTemplate>();
        public ImageListModel SchoolImages { get; } = new ImageListModel() { Async = false };
        public ScrollViewer ScrollViewer_Items => ItemIndexTab.GetChildOfType<ScrollViewer>(this.ListView_SpellTemplates);

        public SpellIndexTab()
        {
            this.InitializeComponent();
            this.ToSpellCommand = new ToSpellCommand(this);
            this.SchoolImages.LoadFromEmbeddedFolder("Images/Schools");
            this.ListView_SpellTemplates.ItemsSource = this.AllSpellTemplates;
            string culture = Thread.CurrentThread.CurrentUICulture.Name;
            string database = File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "dnd5espellindex-" + culture + ".json")) ? "dnd5espellindex-" + culture + ".json" : "dnd5espellindex-en-US.json"));
            this.AllSpellTemplates.AddRange(JsonConvert.DeserializeObject<SpellTemplate[]>(database).Select(s => s.ApplyImageGetterFunc(this.ImageFromSchoolName)));
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.ListView_SpellTemplates.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Level", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            view.Filter = this.Filter;
            this.ListView_SpellTemplates.Items.Refresh();
        }

        private BitmapImage ImageFromSchoolName(string sName) => this.SchoolImages.First(i => i.Name.Equals(sName.Trim().ToLower())).Image;

        private bool Filter(object item)
        {
            if (!this.IsInitialized)
            {
                return true;
            }

            SpellTemplate st = (SpellTemplate)item;
            if (!string.IsNullOrEmpty(this.TextBox_Name.Text) && st.Name.IndexOf(this.TextBox_Name.Text, 0, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            CheckBox[] classes = { this.CB_Bard, this.CB_Cleric, this.CB_Druid, this.CB_Paladin, this.CB_Ranger, this.CB_Sorcerer, this.CB_Warlock, this.CB_Wizard };
            bool classesQualify = !classes.Select(c => c.IsChecked ?? false).Any(b => b);
            if (!classesQualify)
            {
                if (string.IsNullOrEmpty(st.Classes))
                {
                    return false;
                }

                foreach (CheckBox cb in classes)
                {
                    if ((cb.IsChecked ?? false) && st.Classes.IndexOf((string)cb.Content, 0, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        classesQualify = true;
                        break;
                    }
                }
            }

            if (!classesQualify)
            {
                return false;
            }

            if (st.Level < (this.IntUD_LvlMin.Value ?? 0) || st.Level > (this.IntUD_LvlMax.Value ?? 0))
            {
                return false;
            }

            CheckBox[] schools = { this.CB_Abj, this.CB_Con, this.CB_Evo, this.CB_Div, this.CB_Enc, this.CB_Ill, this.CB_Nec, this.CB_Tra };
            bool schoolsQualify = !schools.Select(c => c.IsChecked ?? false).Any(b => b);
            if (!schoolsQualify)
            {
                if (string.IsNullOrEmpty(st.School))
                {
                    return false;
                }

                foreach (CheckBox cb in schools)
                {
                    if ((cb.IsChecked ?? false) && st.School.Equals((string)cb.Content, StringComparison.OrdinalIgnoreCase))
                    {
                        schoolsQualify = true;
                        break;
                    }
                }
            }

            if (!schoolsQualify)
            {
                return false;
            }

            if (this.CB_Ritual.IsChecked ?? false)
            {
                if (!st.Ritual)
                {
                    return false;
                }
            }

            CheckBox[] sources = { this.CB_PHB, this.CB_XGTE, this.CB_SCAG, this.CB_SCPC, this.CB_EE, this.CB_HB };
            bool sourcesQualify = !sources.Select(c => c.IsChecked ?? false).Any(b => b);
            if (!sourcesQualify)
            {
                if (string.IsNullOrEmpty(st.Source))
                {
                    return true;
                }

                foreach (CheckBox cb in sources)
                {
                    if ((cb.IsChecked ?? false) && st.Source.IndexOf((string)cb.Content, 0, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        sourcesQualify = true;
                        break;
                    }
                }
            }

            return sourcesQualify;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => ((CollectionView)CollectionViewSource.GetDefaultView(this.ListView_SpellTemplates.ItemsSource)).Refresh();
    }

    public class ToSpellCommand : ICommand
    {
        private SpellIndexTab _owner;

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;

        public ToSpellCommand(SpellIndexTab sit) => this._owner = sit;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is SpellTemplate st)
            {
                Spell s = new Spell(st)
                {
                    ImageList = AppState.Current.TSpellbook.Images
                };

                CreateSpellWindow csw = new CreateSpellWindow();
                csw.SetDataContext(s);
                if (csw.ShowDialog() ?? false)
                {
                    AppState.Current.TSpellbook[s.Level].Add(s);
                }
            }
        }
    }
}
