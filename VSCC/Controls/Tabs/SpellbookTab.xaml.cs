namespace VSCC.Controls.Tabs
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.Models.ImageList;
    using VSCC.Roll20;
    using VSCC.Roll20.AdvancedIntegration;
    using VSCC.State;

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
            this.InitializeComponent();
            this.Images.LoadFromPhysicalFolder("./Images/Lists/Spells");

            this.Spells0.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Cantrips.Items.Refresh();
            };

            this.List_Cantrips.ItemsSource = this.Spells0;
            this.List_Cantrips.Items.Refresh();

            this.Spells1.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells1.Items.Refresh();
            };

            this.List_Spells1.ItemsSource = this.Spells1;
            this.List_Spells1.Items.Refresh();

            this.Spells2.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells2.Items.Refresh();
            };

            this.List_Spells2.ItemsSource = this.Spells2;
            this.List_Spells2.Items.Refresh();

            this.Spells3.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells3.Items.Refresh();
            };

            this.List_Spells3.ItemsSource = this.Spells3;
            this.List_Spells3.Items.Refresh();

            this.Spells4.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells4.Items.Refresh();
            };

            this.List_Spells4.ItemsSource = this.Spells4;
            this.List_Spells4.Items.Refresh();

            this.Spells5.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells5.Items.Refresh();
            };

            this.List_Spells5.ItemsSource = this.Spells5;
            this.List_Spells5.Items.Refresh();

            this.Spells6.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells6.Items.Refresh();
            };

            this.List_Spells6.ItemsSource = this.Spells6;
            this.List_Spells6.Items.Refresh();

            this.Spells7.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells7.Items.Refresh();
            };

            this.List_Spells7.ItemsSource = this.Spells7;
            this.List_Spells7.Items.Refresh();

            this.Spells8.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells8.Items.Refresh();
            };

            this.List_Spells8.ItemsSource = this.Spells8;
            this.List_Spells8.Items.Refresh();

            this.Spells9.CollectionChanged += (o, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object t in e.NewItems)
                    {
                        if (t is Spell f)
                        {
                            f.ImageList = this.Images;
                        }
                    }
                }

                this.List_Spells9.Items.Refresh();
            };

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

        private void NewSpellCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

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

        private void EditSpellCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void DeleteSpellCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex).SelectedItem is Spell s)
            {
                object o = new object();
                BindingOperations.EnableCollectionSynchronization(this[this.TabControl_Spellbook.SelectedIndex], o);
                this[this.TabControl_Spellbook.SelectedIndex].Remove(s);
                BindingOperations.DisableCollectionSynchronization(this[this.TabControl_Spellbook.SelectedIndex]);
                //this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex)
            }
        }

        private void DeleteSpellCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void List_Cantrips_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)?.SelectedItems?.Count > 0)
            {
                this.EditSpellCommand_Executed(null, null);
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => this.MoveSelectedSpell(true);

        private void Button_Click_1(object sender, RoutedEventArgs e) => this.MoveSelectedSpell(false);

        private void MoveSelectedSpell(bool direction)
        {
            int page = this.TabControl_Spellbook.SelectedIndex;
            int selectedIndex = this.GetSpellCollection(page).SelectedIndex;
            if (this.GetSpellCollection(page).SelectedItem is Spell s)
            {
                int dir = direction && selectedIndex != 0 ? -1 : !direction && selectedIndex != this[page].Count - 1 ? 1 : 0;
                if (dir != 0)
                {
                    this[page].Remove(s);
                    this[page].Insert(selectedIndex + dir, s);
                }
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control)) // New
            {
                this.NewSpellCommand_Executed(sender, default);
                e.Handled = true;
            }

            if (e.Key == Key.Delete)
            {
                this.DeleteSpellCommand_Executed(sender, default);
                e.Handled = true;
            }

            if (e.Key == Key.E && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                this.EditSpellCommand_Executed(sender, default);
                e.Handled = true;
            }
        }

        private void CopySpells(object sender, RoutedEventArgs e) => Clipboard.SetText(string.Join(new string((char)0x1D, 1), this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex).SelectedItems.Cast<Spell>().Select(s => JObject.FromObject(s))));
        private void CutSpells(object sender, RoutedEventArgs e)
        {
            this.CopySpells(sender, e);
            this.DeleteSpellCommand_Executed(sender, default);
        }

        private void PasteSpells(object sender, RoutedEventArgs e)
        {
            string s = Clipboard.GetText();
            try
            {
                foreach (string line in s.Split((char)0x1D))
                {
                    Spell sp = JsonConvert.DeserializeObject<Spell>(line);
                    this[sp.Level].Add(sp);
                }
            }
            catch
            {
                // NOOP
            }
        }

        // Copy
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) => this.CopySpells(sender, default);

        // Cut
        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e) => this.CutSpells(sender, default);

        // Paste
        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e) => this.PasteSpells(sender, default);

        private Spell _lmbSpell;
        private Spell _rmbSpell;

        private void RBMDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is Spell s)
            {
                this._rmbSpell = s;
            }
        }

        private void RMBUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is Spell s && s == this._rmbSpell)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    this.EditSpellRoll20(s, new RoutedEventArgs());
                    e.Handled = true;
                }
            }
        }

        private void LBMDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is Spell s)
            {
                this._lmbSpell = s;
            }
        }

        private void LMBUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.DataContext is Spell s && s == this._rmbSpell)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    this.RunSpellRoll20(s, new RoutedEventArgs());
                    e.Handled = true;
                }
            }
        }

        private void EditSpellRoll20(object sender, RoutedEventArgs e)
        {
            if (!(sender is Spell s))
            {
                ListView collection = this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex);
                s = collection.SelectedItems.Count > 0 ? (Spell)collection.SelectedItems[0] : null;
            }

            if (s != null)
            {
                SimpleSpellIntegration ssi = s.Integration?.Copy() ?? new SimpleSpellIntegration();
                SpellIntegrationWindow siw = new SpellIntegrationWindow(ssi);
                if (siw.ShowDialog() ?? false)
                {
                    s.Integration = ssi;
                }
            }
        }

        private void RunSpellRoll20(object sender, RoutedEventArgs e)
        {
            if (!(sender is Spell s))
            {
                ListView collection = this.GetSpellCollection(this.TabControl_Spellbook.SelectedIndex);
                s = collection.SelectedItems.Count > 0 ? (Spell)collection.SelectedItems[0] : null;
            }

            if (s.Integration == null || !R20WSServer.Connected)
            {
                return;
            }

            ChangeValueWindow cvw = new ChangeValueWindow();
            cvw.DUD_Value.Value = s.Level;
            cvw.DUD_Value.Minimum = s.Level;
            cvw.DUD_Value.Maximum = 9;
            cvw.DUD_Value.Step = 1;
            cvw.Title = MainWindow.Translate("Window_CastAs_Title");
            cvw.ShowDialog();
            int aLvl = (int)cvw.DUD_Value.Value;
            if (s.Integration.ShowSpellDescription)
            {
                R20WSServer.Send(new CommandPacket
                {
                    GMRoll = false,
                    Template = Roll20.Template.Spell,
                    Data = new TemplateDataSpell
                    {
                        CastingTime = s.CastTime,
                        CharName = AppState.Current.State.General.Name,
                        Concentration = s.PropertyConcentration ? "1" : "0",
                        Desc = s.Description,
                        Duration = s.Duration,
                        Material = s.PropertyMaterial ? "1" : "0",
                        Name = s.Name,
                        Range = s.Range,
                        Ritual = s.PropertyRitual ? "1" : "0",
                        SchoolLevel = $"{ s.School } { s.Level }",
                        Somatic = s.PropertySomatic ? "1" : "0",
                        Verbal = s.PropertyVerbal ? "1" : "0",
                        Target = s.Target
                    }
                });
            }

            string hitString = "";
            string saveText = "";
            string damageString = "";
            string critString = "";
            if (s.Integration.HitDie.NumDice.GetForLevel(aLvl, s.Level) > 0) // Have a hit effect
            {
                hitString = $"[[[[{s.Integration.HitDie.NumDice.GetForLevel(aLvl, s.Level)}d{s.Integration.HitDie.DieSide.GetForLevel(aLvl, s.Level)}]][{s.Integration.HitDie.NumDice.GetForLevel(aLvl, s.Level)}d{s.Integration.HitDie.DieSide.GetForLevel(aLvl, s.Level)}]";
                if (s.Integration.HitConstant.GetForLevel(aLvl, s.Level) > 0)
                {
                    hitString += $"+{s.Integration.HitConstant.GetForLevel(aLvl, s.Level)}";
                }

                if (s.Integration.HitIncludeProfficiency)
                {
                    hitString += $"+{AppState.Current.State.General.ProfficiencyBonus}[{MainWindow.Translate("General_PBonus")}]";
                }

                if (s.Integration.HitIncludeSpellcastingAbility)
                {
                    hitString += $"+{AppState.Current.State.Spellbook.SpellAttackBonus}[{MainWindow.Translate("General_SBonus")}]";
                }

                if (s.Integration.HitIncludeStr)
                {
                    hitString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_Str")}]";
                }

                if (s.Integration.HitIncludeDex)
                {
                    hitString += $"+{AppState.Current.State.General.StatModDex}[{MainWindow.Translate("General_Dex")}]";
                }

                if (s.Integration.HitIncludeCon)
                {
                    hitString += $"+{AppState.Current.State.General.StatModCon}[{MainWindow.Translate("General_Con")}]";
                }

                if (s.Integration.HitIncludeWis)
                {
                    hitString += $"+{AppState.Current.State.General.StatModWis}[{MainWindow.Translate("General_Wis")}]";
                }

                if (s.Integration.HitIncludeCha)
                {
                    hitString += $"+{AppState.Current.State.General.StatModCha}[{MainWindow.Translate("General_Cha")}]";
                }

                if (s.Integration.HitIncludeInt)
                {
                    hitString += $"+{AppState.Current.State.General.StatModInt}[{MainWindow.Translate("General_Int")}]";
                }

                hitString += "]]";
            }

            if (s.Integration.Damage.Count > 0)
            {
                damageString = "[[";
                critString = "[[";
                for (int i = 0; i < s.Integration.Damage.Count; i++)
                {
                    ScalableDamageLine dl = s.Integration.Damage[i];
                    if (dl.Die.NumDice.GetForLevel(aLvl, s.Level) > 0 && dl.ConstantNumber.GetForLevel(aLvl, s.Level) != 0)
                    {
                        damageString += $"[[[[{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)}]][{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)}] + {dl.ConstantNumber.GetForLevel(aLvl, s.Level)}]][{dl.Label}]";
                        critString += $"[[{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)}]][{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)} {dl.Label}]";
                    }
                    else
                    {
                        if (dl.Die.DieSide.GetForLevel(aLvl, s.Level) > 0)
                        {
                            damageString += $"[[{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)}]][{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)} {dl.Label}]";
                            critString += $"[[{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)}]][{dl.Die.NumDice.GetForLevel(aLvl, s.Level)}d{dl.Die.DieSide.GetForLevel(aLvl, s.Level)} {dl.Label}]";
                        }
                        else
                        {
                            damageString += $"{dl.ConstantNumber.GetForLevel(aLvl, s.Level)}[{dl.Label}]";
                        }
                    }

                    if (i != s.Integration.Damage.Count - 1)
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
                if (s.Integration.DamageIncludeProfficiency)
                {
                    damageString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_PBonus")}]";
                }

                if (s.Integration.DamageIncludeSpellcastingAbility)
                {
                    damageString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_SBonus")}]";
                }

                if (s.Integration.DamageIncludeStr)
                {
                    damageString += $"+{AppState.Current.State.General.StatModStr}[{MainWindow.Translate("General_Str")}]";
                }

                if (s.Integration.DamageIncludeDex)
                {
                    damageString += $"+{AppState.Current.State.General.StatModDex}[{MainWindow.Translate("General_Dex")}]";
                }

                if (s.Integration.DamageIncludeCon)
                {
                    damageString += $"+{AppState.Current.State.General.StatModCon}[{MainWindow.Translate("General_Con")}]";
                }

                if (s.Integration.DamageIncludeWis)
                {
                    damageString += $"+{AppState.Current.State.General.StatModWis}[{MainWindow.Translate("General_Wis")}]";
                }

                if (s.Integration.DamageIncludeCha)
                {
                    damageString += $"+{AppState.Current.State.General.StatModCha}[{MainWindow.Translate("General_Cha")}]";
                }

                if (s.Integration.DamageIncludeInt)
                {
                    damageString += $"+{AppState.Current.State.General.StatModInt}[{MainWindow.Translate("General_Int")}]";
                }

                damageString += "]]";
            }

            if (s.Integration.HitIsSpellSave)
            {
                saveText = "[[";
                saveText += AppState.Current.State.Spellbook.SpellSaveDC + "[" + MainWindow.Translate("Generic_Save") + "]";
                int a = s.Integration.SaveConstant.GetForLevel(aLvl, s.Level);
                if (a != 0)
                {
                    saveText += $"+{a}[{MainWindow.Translate("Generic_Bonus")}]";
                }

                saveText += "]]";
            }

            // Start sending data.
            // Main priority goes to save
            if (!string.IsNullOrEmpty(saveText)) // Have save DC
            {
                if (!string.IsNullOrEmpty(hitString)) // Have both save AND hit
                {
                    if (!string.IsNullOrEmpty(damageString)) // Have damage too
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkSaveDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = critString,
                                Dmg = damageString,
                                Name = s.Name,
                                R1 = hitString,
                                R2 = hitString,
                                Range = s.Range,
                                SaveAttr = s.Integration.SaveAttr,
                                SaveDC = saveText,
                                SaveDesc = string.Empty
                            }
                        });
                    }
                    else // Only save + hit?
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkSaveDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = critString,
                                Dmg = string.Empty, // Should result in no damage section output
                                Name = s.Name,
                                R1 = hitString,
                                R2 = hitString,
                                Range = s.Range,
                                SaveAttr = s.Integration.SaveAttr,
                                SaveDC = saveText,
                                SaveDesc = string.Empty
                            }
                        });
                    }
                }
                else // No hit
                {
                    if (!string.IsNullOrEmpty(damageString)) // Have damage too
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkSaveDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = critString,
                                Dmg = damageString,
                                Name = s.Name,
                                R1 = string.Empty,
                                R2 = string.Empty,
                                Range = s.Range,
                                SaveAttr = s.Integration.SaveAttr,
                                SaveDC = saveText,
                                SaveDesc = string.Empty
                            }
                        });
                    }
                    else // Only have save
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkSaveDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = string.Empty,
                                Dmg = string.Empty,
                                Name = s.Name,
                                R1 = string.Empty,
                                R2 = string.Empty,
                                Range = s.Range,
                                SaveAttr = s.Integration.SaveAttr,
                                SaveDC = saveText,
                                SaveDesc = string.Empty
                            }
                        });
                    }
                }
            }
            else // Do not have a save
            {
                if (!string.IsNullOrEmpty(hitString)) // Have hit
                {
                    if (!string.IsNullOrEmpty(damageString)) // Have default hit + damage
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = critString,
                                Dmg = damageString,
                                Name = s.Name,
                                R1 = hitString,
                                R2 = hitString,
                                Range = s.Range
                            }
                        });
                    }
                    else // Only have hit?
                    {
                        R20WSServer.Send(new CommandPacket
                        {
                            GMRoll = false,
                            Template = Roll20.Template.AtkDmg,
                            Data = new TemplateDataAtkDmg
                            {
                                CharName = AppState.Current.State.General.Name,
                                Crit = string.Empty,
                                Dmg = string.Empty,
                                Name = s.Name,
                                R1 = hitString,
                                R2 = hitString,
                                Range = s.Range
                            }
                        });
                    }
                }
                else // Only have damage?
                {
                    R20WSServer.Send(new CommandPacket
                    {
                        GMRoll = false,
                        Template = Roll20.Template.AtkDmg,
                        Data = new TemplateDataAtkDmg
                        {
                            CharName = AppState.Current.State.General.Name,
                            Crit = critString,
                            Dmg = damageString,
                            Name = s.Name,
                            R1 = string.Empty,
                            R2 = string.Empty,
                            Range = s.Range
                        }
                    });
                }
            }
        }
    }
}
