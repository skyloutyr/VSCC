using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSCC.Controls.Windows;
using VSCC.DataType;
using VSCC.Models.ImageList;

namespace VSCC.Controls.Tabs
{
    public partial class SpellbookTab : UserControl
    {
        public ImageListModel Images { get; } = new ImageListModel();

        public static RoutedCommand NewSpellCmd = new RoutedCommand();
        public static RoutedCommand EditSpellCmd = new RoutedCommand();
        public static RoutedCommand DeleteSpellCmd = new RoutedCommand();

        public ObservableCollection<Spell> this[int i]
        {
            get
            {
                switch (i)
                {
                    case 1:
                        return this.Spells1;

                    case 2:
                        return this.Spells2;

                    case 3:
                        return this.Spells3;

                    case 4:
                        return this.Spells4;

                    case 5:
                        return this.Spells5;

                    case 6:
                        return this.Spells6;

                    case 7:
                        return this.Spells7;

                    case 8:
                        return this.Spells8;

                    case 9:
                        return this.Spells9;

                    default:
                        return this.Spells0;
                }
            }
        }

        public ObservableCollection<Spell> Spells0 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells1 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells2 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells3 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells4 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells5 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells6 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells7 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells8 { get; } = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> Spells9 { get; } = new ObservableCollection<Spell>();

        public SpellbookTab()
        {
            InitializeComponent();
            this.Images.LoadFromPhysicalFolder("./Images/Lists/Spells");

            this.Spells0.CollectionChanged += (o, e) => this.List_Cantrips.Items.Refresh();
            this.List_Cantrips.ItemsSource = this.Spells0;
            this.List_Cantrips.Items.Refresh();

            this.Spells1.CollectionChanged += (o, e) => this.List_Spells1.Items.Refresh();
            this.List_Spells1.ItemsSource = this.Spells1;
            this.List_Spells1.Items.Refresh();

            this.Spells2.CollectionChanged += (o, e) => this.List_Spells2.Items.Refresh();
            this.List_Spells2.ItemsSource = this.Spells2;
            this.List_Spells2.Items.Refresh();

            this.Spells3.CollectionChanged += (o, e) => this.List_Spells3.Items.Refresh();
            this.List_Spells3.ItemsSource = this.Spells3;
            this.List_Spells3.Items.Refresh();

            this.Spells4.CollectionChanged += (o, e) => this.List_Spells4.Items.Refresh();
            this.List_Spells4.ItemsSource = this.Spells4;
            this.List_Spells4.Items.Refresh();

            this.Spells5.CollectionChanged += (o, e) => this.List_Spells5.Items.Refresh();
            this.List_Spells5.ItemsSource = this.Spells5;
            this.List_Spells5.Items.Refresh();

            this.Spells6.CollectionChanged += (o, e) => this.List_Spells6.Items.Refresh();
            this.List_Spells6.ItemsSource = this.Spells6;
            this.List_Spells6.Items.Refresh();

            this.Spells7.CollectionChanged += (o, e) => this.List_Spells7.Items.Refresh();
            this.List_Spells7.ItemsSource = this.Spells7;
            this.List_Spells7.Items.Refresh();

            this.Spells8.CollectionChanged += (o, e) => this.List_Spells8.Items.Refresh();
            this.List_Spells8.ItemsSource = this.Spells8;
            this.List_Spells8.Items.Refresh();

            this.Spells9.CollectionChanged += (o, e) => this.List_Spells9.Items.Refresh();
            this.List_Spells9.ItemsSource = this.Spells9;
            this.List_Spells9.Items.Refresh();
        }

        public ListView GetSpellCollection(int index)
        {
            switch (index)
            {
                case 1:
                    return this.List_Spells1;

                case 2:
                    return this.List_Spells2;

                case 3:
                    return this.List_Spells3;

                case 4:
                    return this.List_Spells4;

                case 5:
                    return this.List_Spells5;

                case 6:
                    return this.List_Spells6;

                case 7:
                    return this.List_Spells7;

                case 8:
                    return this.List_Spells8;

                case 9:
                    return this.List_Spells9;

                default:
                    return this.List_Cantrips;
            }
        }

        public void SetSpellCollection(ObservableCollection<Spell> spells, int index)
        {
            foreach (Spell s in spells)
            {
                s.ImageList = this.Images;
            }

            switch (index)
            {
                case 0:
                {
                    spells.CollectionChanged += (o, e) => this.List_Cantrips.Items.Refresh();
                    this.List_Cantrips.ItemsSource = spells;
                    this.List_Cantrips.Items.Refresh();
                    break;
                }

                case 1:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells1.Items.Refresh();
                    this.List_Spells1.ItemsSource = spells;
                    this.List_Spells1.Items.Refresh();
                    break;
                }

                case 2:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells2.Items.Refresh();
                    this.List_Spells2.ItemsSource = spells;
                    this.List_Spells2.Items.Refresh();
                    break;
                }

                case 3:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells3.Items.Refresh();
                    this.List_Spells3.ItemsSource = spells;
                    this.List_Spells3.Items.Refresh();
                    break;
                }

                case 4:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells4.Items.Refresh();
                    this.List_Spells4.ItemsSource = spells;
                    this.List_Spells4.Items.Refresh();
                    break;
                }

                case 5:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells5.Items.Refresh();
                    this.List_Spells5.ItemsSource = spells;
                    this.List_Spells5.Items.Refresh();
                    break;
                }

                case 6:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells6.Items.Refresh();
                    this.List_Spells6.ItemsSource = spells;
                    this.List_Spells6.Items.Refresh();
                    break;
                }

                case 7:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells7.Items.Refresh();
                    this.List_Spells7.ItemsSource = spells;
                    this.List_Spells7.Items.Refresh();
                    break;
                }

                case 8:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells8.Items.Refresh();
                    this.List_Spells8.ItemsSource = spells;
                    this.List_Spells8.Items.Refresh();
                    break;
                }

                case 9:
                {
                    spells.CollectionChanged += (o, e) => this.List_Spells9.Items.Refresh();
                    this.List_Spells9.ItemsSource = spells;
                    this.List_Spells9.Items.Refresh();
                    break;
                }
            }
        }

        private void NewSpellCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Spell s = new Spell()
            {
                Level = this.TabControl_Spellbook.SelectedIndex,
                ImageList = this.Images
            };

            CreateSpellWindow csw = new CreateSpellWindow();
            csw.SetDataContext(s);
            if (csw.ShowDialog() ?? false)
            {
                this[s.Level].Add(s);
            }
        }

        private void NewSpellCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EditSpellCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ObservableCollection<Spell> cc = this[this.TabControl_Spellbook.SelectedIndex];
            if (this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex).SelectedItem is Spell s)
            {
                CreateSpellWindow csw = new CreateSpellWindow();
                csw.SetDataContext(s.Copy());
                if (csw.ShowDialog() ?? false)
                {
                    cc[this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex).SelectedIndex] = (Spell)csw.DataContext;
                }
            }
        }

        private void EditSpellCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteSpellCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex).SelectedItem is Spell s)
            {
                this[this.TabControl_Spellbook.SelectedIndex].Remove(s);
            }
        }

        private void DeleteSpellCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
