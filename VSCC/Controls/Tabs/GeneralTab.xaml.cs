namespace VSCC.Controls.Tabs
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    public partial class GeneralTab : UserControl
    {
        private readonly KeyValuePair<CheckBox, NumericUpDown>[] _statsProfMap;
        private readonly KeyValuePair<NumericUpDown, NumericUpDown[]>[] _statsSkillsMap;

        public ObservableCollection<StatModifier> ModifiersStrength { get; set; } = new ObservableCollection<StatModifier>();
        public ObservableCollection<StatModifier> ModifiersDexterity { get; set; } = new ObservableCollection<StatModifier>();
        public ObservableCollection<StatModifier> ModifiersConstitution { get; set; } = new ObservableCollection<StatModifier>();
        public ObservableCollection<StatModifier> ModifiersWisdom { get; set; } = new ObservableCollection<StatModifier>();
        public ObservableCollection<StatModifier> ModifiersIntelligence { get; set; } = new ObservableCollection<StatModifier>();
        public ObservableCollection<StatModifier> ModifiersCharisma { get; set; } = new ObservableCollection<StatModifier>();

        public GeneralTab()
        {
            this.InitializeComponent();
            this._statsProfMap = new[]
            {
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Saves_Str, this.IntUD_Saves_Str),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Saves_Dex, this.IntUD_Saves_Dex),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Saves_Con, this.IntUD_Saves_Con),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Saves_Cha, this.IntUD_Saves_Cha),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Saves_Int, this.IntUD_Saves_Int),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Saves_Wis, this.IntUD_Saves_Wis),

                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Athletics, this.IntUD_Athletics),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Acrobatics, this.IntUD_Acrobatics),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_SleightOfHand, this.IntUD_SleightOfHand),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Stealth, this.IntUD_Stealth),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Arcana, this.IntUD_Arcana),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_History, this.IntUD_History),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Investigation, this.IntUD_Investigation),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Nature, this.IntUD_Nature),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Religion, this.IntUD_Religion),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_AnimalHandling, this.IntUD_AnimalHandling),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Insight, this.IntUD_Insight),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Medicine, this.IntUD_Medicine),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Perception, this.IntUD_Perception),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Survival, this.IntUD_Survival),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Deception, this.IntUD_Deception),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Intimidation, this.IntUD_Intimidation),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Performance, this.IntUD_Performance),
                new KeyValuePair<CheckBox, NumericUpDown>(this.CheckBox_Persuasion, this.IntUD_Persuasion),
            };

            this._statsSkillsMap = new[]
            {
                new KeyValuePair<NumericUpDown, NumericUpDown[]>(this.IntUD_Str, new []{ this.IntUD_Saves_Str, this.IntUD_Athletics }),
                new KeyValuePair<NumericUpDown, NumericUpDown[]>(this.IntUD_Dex, new []{ this.IntUD_Saves_Dex, this.IntUD_Acrobatics, this.IntUD_SleightOfHand, this.IntUD_Stealth }),
                new KeyValuePair<NumericUpDown, NumericUpDown[]>(this.IntUD_Int, new []{ this.IntUD_Saves_Int, this.IntUD_Arcana, this.IntUD_History, this.IntUD_Investigation, this.IntUD_Nature, this.IntUD_Religion }),
                new KeyValuePair<NumericUpDown, NumericUpDown[]>(this.IntUD_Wis, new []{ this.IntUD_Saves_Wis, this.IntUD_AnimalHandling, this.IntUD_Insight, this.IntUD_Medicine, this.IntUD_Perception, this.IntUD_Survival }),
                new KeyValuePair<NumericUpDown, NumericUpDown[]>(this.IntUD_Cha, new []{ this.IntUD_Saves_Cha, this.IntUD_Deception, this.IntUD_Persuasion, this.IntUD_Performance, this.IntUD_Intimidation }),
                new KeyValuePair<NumericUpDown, NumericUpDown[]>(this.IntUD_Con, new []{ this.IntUD_Saves_Con }),
            };

            this.ModifiersStrength.CollectionChanged += this.ChangeStatModifiers;
            this.ModifiersDexterity.CollectionChanged += this.ChangeStatModifiers;
            this.ModifiersConstitution.CollectionChanged += this.ChangeStatModifiers;
            this.ModifiersWisdom.CollectionChanged += this.ChangeStatModifiers;
            this.ModifiersCharisma.CollectionChanged += this.ChangeStatModifiers;
            this.ModifiersIntelligence.CollectionChanged += this.ChangeStatModifiers;
            this.Btn_TempStat_Str.Tag = this.ModifiersStrength;
            this.Btn_TempStat_Dex.Tag = this.ModifiersDexterity;
            this.Btn_TempStat_Con.Tag = this.ModifiersConstitution;
            this.Btn_TempStat_Cha.Tag = this.ModifiersCharisma;
            this.Btn_TempStat_Int.Tag = this.ModifiersIntelligence;
            this.Btn_TempStat_Wis.Tag = this.ModifiersWisdom;
        }

        public void ChangeStatModifiers(object sender, NotifyCollectionChangedEventArgs args)
        {
            int modsOld = args.OldItems?.Cast<StatModifier>().Sum(sm => sm.Value) ?? 0;
            int modsNew = args.NewItems?.Cast<StatModifier>().Sum(sm => sm.Value) ?? 0;
            if (sender == this.ModifiersStrength)
            {
                this.IntUD_Str.Value += modsNew - modsOld;
                return;
            }

            if (sender == this.ModifiersDexterity)
            {
                this.IntUD_Dex.Value += modsNew - modsOld;
                return;
            }

            if (sender == this.ModifiersConstitution)
            {
                this.IntUD_Con.Value += modsNew - modsOld;
                return;
            }

            if (sender == this.ModifiersCharisma)
            {
                this.IntUD_Cha.Value += modsNew - modsOld;
                return;
            }

            if (sender == this.ModifiersWisdom)
            {
                this.IntUD_Wis.Value += modsNew - modsOld;
                return;
            }

            if (sender == this.ModifiersIntelligence)
            {
                this.IntUD_Int.Value += modsNew - modsOld;
                return;
            }
        }

        public void RebuildAllStats()
        {
            AppState.Current.FreezeAutocalc = true;
            AppState.Current.State.General.ProfficiencyBonus = this.GetProfBonusForLvl(AppState.Current.State.General.Level);
            (NumericUpDown, TextBox)[] statMods = { (this.IntUD_Str, this.TextBox_Str), (this.IntUD_Dex, this.TextBox_Dex), (this.IntUD_Con, this.TextBox_Con), (this.IntUD_Cha, this.TextBox_Cha), (this.IntUD_Wis, this.TextBox_Wis), (this.IntUD_Int, this.TextBox_Int) };
            for (int i = 0; i < 6; ++i)
            {
                (NumericUpDown, TextBox) dat = statMods[i];
                dat.Item2.Text = this.GetModifierForStat(dat.Item1.Value).ToString();
            }

            foreach (KeyValuePair<NumericUpDown, NumericUpDown[]> kvp in this._statsSkillsMap)
            {
                int res = this.GetModifierForStat(kvp.Key.Value);
                foreach (NumericUpDown intUD in kvp.Value)
                {
                    intUD.Value = res;
                    if (this._statsProfMap.First(kv => kv.Value == intUD).Key.IsChecked ?? false)
                    {
                        intUD.Value += AppState.Current.State.General.ProfficiencyBonus;
                    }
                }
            }

            AppState.Current.FreezeAutocalc = false;
        }

        private void Stat_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (AppState.Current.FreezeAutocalc)
            {
                return;
            }

            int oldVal = e.OldValue;
            int newVal = e.NewValue;
            int oldModVal = this.GetModifierForStat(oldVal);
            int newModVal = this.GetModifierForStat(newVal);
            NumericUpDown inUD = (NumericUpDown)sender;
            if (inUD == this.IntUD_Str)
            {
                AppState.Current.TInventory.RecalculateWeights(false, true, true);
            }

            Grid g = inUD.Parent as Grid;
            TextBox tb = g?.Children.OfType<TextBox>().FirstOrDefault();
            if (tb != null)
            {
                if (!int.TryParse(tb.Text, out int actualOldVal))
                {
                    actualOldVal = 0;
                }

                int statDiff = actualOldVal - oldModVal;
                tb.Text = (newModVal + statDiff).ToString();
            }

            foreach (NumericUpDown stat in this._statsSkillsMap.First(kv => kv.Key == inUD).Value)
            {
                stat.Value += newModVal - oldModVal;
            }
        }

        private void IntUD_Lvl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (AppState.Current.FreezeAutocalc)
            {
                return;
            }

            int oldVal = e.OldValue;
            int newVal = e.NewValue;
            int currProfVal = this.IntUD_ProfBonus.Value;
            int shouldBeProfVal = this.GetProfBonusForLvl(oldVal);
            this.IntUD_ProfBonus.Value = this.GetProfBonusForLvl(newVal) + (currProfVal - shouldBeProfVal);
        }

        private void IntUD_ProfBonus_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (AppState.Current.FreezeAutocalc)
            {
                return;
            }

            int oldVal = e.OldValue;
            int newVal = e.NewValue;
            foreach (KeyValuePair<CheckBox, NumericUpDown> kv in this._statsProfMap)
            {
                if (kv.Key.IsChecked ?? false)
                {
                    kv.Value.Value += newVal - oldVal;
                }
            }
        }

        private void CheckBox_Stat_Checked(object sender, RoutedEventArgs e) => this.ChangeStatData((CheckBox)sender, true);

        private void CheckBox_Stat_UnChecked(object sender, RoutedEventArgs e) => this.ChangeStatData((CheckBox)sender, false);

        private void ChangeStatData(CheckBox stat, bool check)
        {
            if (AppState.Current.FreezeAutocalc)
            {
                return;
            }

            int val = (this.IntUD_ProfBonus.Value);
            NumericUpDown intUD = this._statsProfMap.First(kv => kv.Key == stat).Value;
            intUD.Value += check ? val : -val;
        }

        private int GetModifierForStat(int stat) => (int)Math.Floor((stat - 10) / 2F);

        private int GetProfBonusForLvl(int v) => v < 5 ? 2 : v < 9 ? 3 : v < 13 ? 4 : v < 17 ? 5 : 6;

        private void Bar_Exp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeMinMaxWindow cmmw = new ChangeMinMaxWindow();
            Func<int> minGetter;
            Action<int> minSetter;
            Func<int> maxGetter;
            Action<int> maxSetter;
            if (sender == this.Bar_Exp)
            {
                minGetter = () => AppState.Current.State.General.CurrentExp;
                maxGetter = () => AppState.Current.State.General.MaxExp;
                minSetter = i => AppState.Current.State.General.CurrentExp = i;
                maxSetter = i => AppState.Current.State.General.MaxExp = i;
            }
            else
            {
                if (sender == this.Bar_HP)
                {
                    minGetter = () => AppState.Current.State.General.CurrentHP;
                    maxGetter = () => AppState.Current.State.General.MaxHP;
                    minSetter = i => AppState.Current.State.General.CurrentHP = i;
                    maxSetter = i => AppState.Current.State.General.MaxHP = i;
                }
                else
                {
                    minGetter = () => AppState.Current.State.General.CurrentTempHP;
                    maxGetter = () => AppState.Current.State.General.MaxTempHP;
                    minSetter = i => AppState.Current.State.General.CurrentTempHP = i;
                    maxSetter = i => AppState.Current.State.General.MaxTempHP = i;
                }
            }

            cmmw.DUD_Min.Value = minGetter();
            cmmw.DUD_Max.Value = maxGetter();
            if (cmmw.ShowDialog() ?? false)
            {
                minSetter((int)cmmw.DUD_Min.Value);
                maxSetter((int)cmmw.DUD_Max.Value);
                ((ColoredBar)sender).InvalidateVisual();
            }
        }

        private void Button_ChangeValue_Clicked(object sender, RoutedEventArgs e)
        {
            Action<int> valueChanger = sender == this.Btn_Add_Exp
                ? (i => AppState.Current.State.General.CurrentExp += i)
                : sender == this.Btn_Remove_Exp
                    ? (i => AppState.Current.State.General.CurrentExp -= i)
                    : sender == this.Btn_Add_HP
                        ? (i => AppState.Current.State.General.CurrentHP += i)
                        : sender == this.Btn_Remove_HP
                            ? (i => AppState.Current.State.General.CurrentHP -= i)
                            : sender == this.Btn_Add_TempHP
                                ? (i => AppState.Current.State.General.CurrentTempHP += i)
                                : new Action<int>(i => AppState.Current.State.General.CurrentTempHP -= i); // Need this for compiler

            ChangeValueWindow cvw = new ChangeValueWindow();
            if (cvw.ShowDialog() ?? false)
            {
                valueChanger((int)cvw.DUD_Value.Value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "Images(*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif",
                Multiselect = false
            };

            if (ofd.ShowDialog() ?? false)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(ofd.FileName);
                    AppState.Current.State.General.PortraitLocation = Convert.ToBase64String(data);
                }
                catch
                {
                    // NOOP
                }
            }
        }

        private void Btn_TempStat_Str_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup();
            TemporaryStatsPanel tsp = new TemporaryStatsPanel(popup, ((Button)sender).Tag as ObservableCollection<StatModifier>);
            tsp.HorizontalOffset = tsp.VerticalOffset = -1;
            tsp.PlacementTarget = (UIElement)sender;
            Popup.CreateRootPopup(popup, tsp);
        }

        /// <summary>
        /// Refresh death saving throws
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e) => this.CheckBox_DS_Fail1.IsChecked = this.CheckBox_DS_Fail2.IsChecked = this.CheckBox_DS_Fail3.IsChecked = this.CheckBox_DS_Pass1.IsChecked = this.CheckBox_DS_Pass2.IsChecked = this.CheckBox_DS_Pass3.IsChecked = false;

        /// <summary>
        /// Recalculate all stats button click
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e) => this.RebuildAllStats();
    }
}
