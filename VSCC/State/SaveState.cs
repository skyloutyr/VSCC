using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using VSCC.Controls.Tabs;
using VSCC.DataType;
using VSCC.Properties;

namespace VSCC.State
{
    public class SaveState
    {
        public int Version { get; set; } = 2;
        public General General { get; set; } = new General();
        public Extras Extras { get; set; } = new Extras();
        public Inventory Inventory { get; set; } = new Inventory();
        public Spellbook Spellbook { get; set; } = new Spellbook();

        public void Clear()
        {
            this.General.Clear();
            this.Extras.Clear();
            this.Inventory.Clear();
            this.Spellbook.Clear();
        }

        public string Save() => JsonConvert.SerializeObject(this, Formatting.Indented);
        public void Load(string s) => AppState.Current.State = JsonConvert.DeserializeObject<SaveState>(s);
    }

    public sealed class General
    {
        public string Name
        {
            get => AppState.Current.TGeneral.TextBox_Name.Text;
            set => AppState.Current.TGeneral.TextBox_Name.Text = value;
        }

        public int CurrentExp
        {
            get => AppState.Current.TGeneral.Bar_Exp.CurrentValue;
            set
            {
                AppState.Current.TGeneral.Bar_Exp.CurrentValue = value;
                AppState.Current.TGeneral.Bar_Exp.InvalidateVisual();
            }
        }

        public int MaxExp
        {
            get => AppState.Current.TGeneral.Bar_Exp.MaximumValue;
            set
            {
                AppState.Current.TGeneral.Bar_Exp.MaximumValue = value;
                AppState.Current.TGeneral.Bar_Exp.InvalidateVisual();
            }
        }

        public int CurrentHP
        {
            get => AppState.Current.TGeneral.Bar_HP.CurrentValue;
            set
            {
                AppState.Current.TGeneral.Bar_HP.CurrentValue = value;
                AppState.Current.TGeneral.Bar_HP.InvalidateVisual();
            }
        }

        public int MaxHP
        {
            get => AppState.Current.TGeneral.Bar_HP.MaximumValue;
            set
            {
                AppState.Current.TGeneral.Bar_HP.MaximumValue = value;
                AppState.Current.TGeneral.Bar_HP.InvalidateVisual();
            }
        }

        public int CurrentTempHP
        {
            get => AppState.Current.TGeneral.Bar_Temp_HP.CurrentValue;
            set
            {
                AppState.Current.TGeneral.Bar_Temp_HP.CurrentValue = value;
                AppState.Current.TGeneral.Bar_Temp_HP.InvalidateVisual();
            }
        }

        public int MaxTempHP
        {
            get => AppState.Current.TGeneral.Bar_Temp_HP.MaximumValue;
            set
            {
                AppState.Current.TGeneral.Bar_Temp_HP.MaximumValue = value;
                AppState.Current.TGeneral.Bar_Temp_HP.InvalidateVisual();
            }
        }

        public int Level
        {
            get => AppState.Current.TGeneral.IntUD_Lvl.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Lvl.Value = value;
        }

        public string Class
        {
            get => AppState.Current.TGeneral.ComboBox_Class.Text;
            set => AppState.Current.TGeneral.ComboBox_Class.Text = value;
        }

        public string Race
        {
            get => AppState.Current.TGeneral.ComboBox_Race.Text;
            set => AppState.Current.TGeneral.ComboBox_Race.Text = value;
        }

        public string Alignment
        {
            get => AppState.Current.TGeneral.ComboBox_Alignment.Text;
            set => AppState.Current.TGeneral.ComboBox_Alignment.Text = value;
        }

        public string Background
        {
            get => AppState.Current.TGeneral.ComboBox_Background.Text;
            set => AppState.Current.TGeneral.ComboBox_Background.Text = value;
        }

        public string Traits
        {
            get => AppState.Current.TGeneral.TextBox_Traits.Text;
            set => AppState.Current.TGeneral.TextBox_Traits.Text = value;
        }

        public string Flaws
        {
            get => AppState.Current.TGeneral.TextBox_Flaws.Text;
            set => AppState.Current.TGeneral.TextBox_Flaws.Text = value;
        }

        public string Bonds
        {
            get => AppState.Current.TGeneral.TextBox_Bonds.Text;
            set => AppState.Current.TGeneral.TextBox_Bonds.Text = value;
        }

        public string Ideals
        {
            get => AppState.Current.TGeneral.TextBox_Ideals.Text;
            set => AppState.Current.TGeneral.TextBox_Ideals.Text = value;
        }

        public int ProfficiencyBonus
        {
            get => AppState.Current.TGeneral.IntUD_ProfBonus.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_ProfBonus.Value = value;
        }

        public int ArmorClass
        {
            get => AppState.Current.TGeneral.IntUD_AC.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_AC.Value = value;
        }

        public int Speed
        {
            get => AppState.Current.TGeneral.IntUD_Speed.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Speed.Value = value;
        }

        public int HitDice12Current
        {
            get => AppState.Current.TGeneral.IntUD_HD12C.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD12C.Value = value;
        }

        public int HitDice12Max
        {
            get => AppState.Current.TGeneral.IntUD_HD12M.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD12M.Value = value;
        }

        public int HitDice10Current
        {
            get => AppState.Current.TGeneral.IntUD_HD10C.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD10C.Value = value;
        }

        public int HitDice10Max
        {
            get => AppState.Current.TGeneral.IntUD_HD10M.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD10M.Value = value;
        }

        public int HitDice8Current
        {
            get => AppState.Current.TGeneral.IntUD_HD8C.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD8C.Value = value;
        }

        public int HitDice8Max
        {
            get => AppState.Current.TGeneral.IntUD_HD8M.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD8M.Value = value;
        }

        public int HitDice6Current
        {
            get => AppState.Current.TGeneral.IntUD_HD6C.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD6C.Value = value;
        }

        public int HitDice6Max
        {
            get => AppState.Current.TGeneral.IntUD_HD6M.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_HD6M.Value = value;
        }

        public int DeathThrowPasses
        {
            get => 0 + (AppState.Current.TGeneral.CheckBox_DS_Pass1.IsChecked ?? false ? 1 : 0) + (AppState.Current.TGeneral.CheckBox_DS_Pass2.IsChecked ?? false ? 1 : 0) + (AppState.Current.TGeneral.CheckBox_DS_Pass3.IsChecked ?? false ? 1 : 0);
            set
            {
                AppState.Current.TGeneral.CheckBox_DS_Pass1.IsChecked = value >= 1;
                AppState.Current.TGeneral.CheckBox_DS_Pass2.IsChecked = value >= 2;
                AppState.Current.TGeneral.CheckBox_DS_Pass3.IsChecked = value >= 3;
            }
        }

        public int DeathThrowFails
        {
            get => 0 + (AppState.Current.TGeneral.CheckBox_DS_Fail1.IsChecked ?? false ? 1 : 0) + (AppState.Current.TGeneral.CheckBox_DS_Fail2.IsChecked ?? false ? 1 : 0) + (AppState.Current.TGeneral.CheckBox_DS_Fail3.IsChecked ?? false ? 1 : 0);
            set
            {
                AppState.Current.TGeneral.CheckBox_DS_Fail1.IsChecked = value >= 1;
                AppState.Current.TGeneral.CheckBox_DS_Fail2.IsChecked = value >= 2;
                AppState.Current.TGeneral.CheckBox_DS_Fail3.IsChecked = value >= 3;
            }
        }

        public int StatStr
        {
            get => AppState.Current.TGeneral.IntUD_Str.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Str.Value = value;
        }

        public int StatDex
        {
            get => AppState.Current.TGeneral.IntUD_Dex.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Dex.Value = value;
        }

        public int StatCon
        {
            get => AppState.Current.TGeneral.IntUD_Con.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Con.Value = value;
        }

        public int StatCha
        {
            get => AppState.Current.TGeneral.IntUD_Cha.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Cha.Value = value;
        }

        public int StatWis
        {
            get => AppState.Current.TGeneral.IntUD_Wis.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Wis.Value = value;
        }

        public int StatInt
        {
            get => AppState.Current.TGeneral.IntUD_Int.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Int.Value = value;
        }

        public int StatModStr
        {
            get => int.Parse(AppState.Current.TGeneral.TextBox_Str.Text);
            set => AppState.Current.TGeneral.TextBox_Str.Text = value.ToString();
        }

        public int StatModDex
        {
            get => int.Parse(AppState.Current.TGeneral.TextBox_Dex.Text);
            set => AppState.Current.TGeneral.TextBox_Dex.Text = value.ToString();
        }

        public int StatModCon
        {
            get => int.Parse(AppState.Current.TGeneral.TextBox_Con.Text);
            set => AppState.Current.TGeneral.TextBox_Con.Text = value.ToString();
        }

        public int StatModCha
        {
            get => int.Parse(AppState.Current.TGeneral.TextBox_Cha.Text);
            set => AppState.Current.TGeneral.TextBox_Cha.Text = value.ToString();
        }

        public int StatModWis
        {
            get => int.Parse(AppState.Current.TGeneral.TextBox_Wis.Text);
            set => AppState.Current.TGeneral.TextBox_Wis.Text = value.ToString();
        }

        public int StatModInt
        {
            get => int.Parse(AppState.Current.TGeneral.TextBox_Int.Text);
            set => AppState.Current.TGeneral.TextBox_Int.Text = value.ToString();
        }

        public int StrSave
        {
            get => AppState.Current.TGeneral.IntUD_Saves_Str.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Saves_Str.Value = value;
        }

        public int DexSave
        {
            get => AppState.Current.TGeneral.IntUD_Saves_Dex.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Saves_Dex.Value = value;
        }

        public int ConSave
        {
            get => AppState.Current.TGeneral.IntUD_Saves_Con.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Saves_Con.Value = value;
        }

        public int IntSave
        {
            get => AppState.Current.TGeneral.IntUD_Saves_Int.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Saves_Int.Value = value;
        }

        public int WisSave
        {
            get => AppState.Current.TGeneral.IntUD_Saves_Wis.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Saves_Wis.Value = value;
        }

        public int ChaSave
        {
            get => AppState.Current.TGeneral.IntUD_Saves_Cha.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Saves_Cha.Value = value;
        }

        public int Athletics
        {
            get => AppState.Current.TGeneral.IntUD_Athletics.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Athletics.Value = value;
        }

        public int Acrobatics
        {
            get => AppState.Current.TGeneral.IntUD_Acrobatics.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Acrobatics.Value = value;
        }

        public int SleightOfHand
        {
            get => AppState.Current.TGeneral.IntUD_SleightOfHand.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_SleightOfHand.Value = value;
        }

        public int Stealth
        {
            get => AppState.Current.TGeneral.IntUD_Stealth.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Stealth.Value = value;
        }

        public int Arcana
        {
            get => AppState.Current.TGeneral.IntUD_Arcana.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Arcana.Value = value;
        }

        public int History
        {
            get => AppState.Current.TGeneral.IntUD_History.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_History.Value = value;
        }

        public int Investigation
        {
            get => AppState.Current.TGeneral.IntUD_Investigation.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Investigation.Value = value;
        }

        public int Nature
        {
            get => AppState.Current.TGeneral.IntUD_Nature.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Nature.Value = value;
        }

        public int Religion
        {
            get => AppState.Current.TGeneral.IntUD_Religion.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Religion.Value = value;
        }

        public int AnimalHandling
        {
            get => AppState.Current.TGeneral.IntUD_AnimalHandling.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_AnimalHandling.Value = value;
        }

        public int Insight
        {
            get => AppState.Current.TGeneral.IntUD_Insight.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Insight.Value = value;
        }

        public int Medicine
        {
            get => AppState.Current.TGeneral.IntUD_Medicine.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Medicine.Value = value;
        }

        public int Perception
        {
            get => AppState.Current.TGeneral.IntUD_Perception.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Perception.Value = value;
        }

        public int Survival
        {
            get => AppState.Current.TGeneral.IntUD_Survival.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Survival.Value = value;
        }

        public int Deception
        {
            get => AppState.Current.TGeneral.IntUD_Deception.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Deception.Value = value;
        }

        public int Performance
        {
            get => AppState.Current.TGeneral.IntUD_Performance.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Performance.Value = value;
        }

        public int Persuasion
        {
            get => AppState.Current.TGeneral.IntUD_Persuasion.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Persuasion.Value = value;
        }

        public int Intimidation
        {
            get => AppState.Current.TGeneral.IntUD_Intimidation.Value ?? 0;
            set => AppState.Current.TGeneral.IntUD_Intimidation.Value = value;
        }

        public bool ProfficientAtStrSave
        {
            get => AppState.Current.TGeneral.CheckBox_Saves_Str.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Saves_Str.IsChecked = value;
        }

        public bool ProfficientAtDexSave
        {
            get => AppState.Current.TGeneral.CheckBox_Saves_Dex.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Saves_Dex.IsChecked = value;
        }

        public bool ProfficientAtConSave
        {
            get => AppState.Current.TGeneral.CheckBox_Saves_Con.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Saves_Con.IsChecked = value;
        }

        public bool ProfficientAtIntSave
        {
            get => AppState.Current.TGeneral.CheckBox_Saves_Int.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Saves_Int.IsChecked = value;
        }

        public bool ProfficientAtWisSave
        {
            get => AppState.Current.TGeneral.CheckBox_Saves_Wis.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Saves_Wis.IsChecked = value;
        }

        public bool ProfficientAtChaSave
        {
            get => AppState.Current.TGeneral.CheckBox_Saves_Cha.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Saves_Cha.IsChecked = value;
        }

        public bool ProfficientAtAthletics
        {
            get => AppState.Current.TGeneral.CheckBox_Athletics.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Athletics.IsChecked = value;
        }

        public bool ProfficientAtAcrobatics
        {
            get => AppState.Current.TGeneral.CheckBox_Acrobatics.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Acrobatics.IsChecked = value;
        }

        public bool ProfficientAtSleightOfHand
        {
            get => AppState.Current.TGeneral.CheckBox_SleightOfHand.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_SleightOfHand.IsChecked = value;
        }

        public bool ProfficientAtStealth
        {
            get => AppState.Current.TGeneral.CheckBox_Stealth.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Stealth.IsChecked = value;
        }

        public bool ProfficientAtArcana
        {
            get => AppState.Current.TGeneral.CheckBox_Arcana.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Arcana.IsChecked = value;
        }

        public bool ProfficientAtHistory
        {
            get => AppState.Current.TGeneral.CheckBox_History.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_History.IsChecked = value;
        }

        public bool ProfficientAtInvestigation
        {
            get => AppState.Current.TGeneral.CheckBox_Investigation.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Investigation.IsChecked = value;
        }

        public bool ProfficientAtNature
        {
            get => AppState.Current.TGeneral.CheckBox_Nature.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Nature.IsChecked = value;
        }

        public bool ProfficientAtReligion
        {
            get => AppState.Current.TGeneral.CheckBox_Religion.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Religion.IsChecked = value;
        }

        public bool ProfficientAtAnimalHandling
        {
            get => AppState.Current.TGeneral.CheckBox_AnimalHandling.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_AnimalHandling.IsChecked = value;
        }

        public bool ProfficientAtInsight
        {
            get => AppState.Current.TGeneral.CheckBox_Insight.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Insight.IsChecked = value;
        }

        public bool ProfficientAtMedicine
        {
            get => AppState.Current.TGeneral.CheckBox_Medicine.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Medicine.IsChecked = value;
        }

        public bool ProfficientAtPerception
        {
            get => AppState.Current.TGeneral.CheckBox_Perception.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Perception.IsChecked = value;
        }

        public bool ProfficientAtSurvival
        {
            get => AppState.Current.TGeneral.CheckBox_Survival.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Survival.IsChecked = value;
        }

        public bool ProfficientAtDeception
        {
            get => AppState.Current.TGeneral.CheckBox_Deception.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Deception.IsChecked = value;
        }

        public bool ProfficientAtPerformance
        {
            get => AppState.Current.TGeneral.CheckBox_Performance.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Performance.IsChecked = value;
        }

        public bool ProfficientAtPersuasion
        {
            get => AppState.Current.TGeneral.CheckBox_Persuasion.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Persuasion.IsChecked = value;
        }

        public bool ProfficientAtIntimidation
        {
            get => AppState.Current.TGeneral.CheckBox_Intimidation.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Intimidation.IsChecked = value;
        }

        public bool HasInspiration
        {
            get => AppState.Current.TGeneral.CheckBox_Inspiration.IsChecked ?? false;
            set => AppState.Current.TGeneral.CheckBox_Inspiration.IsChecked = value;
        }

        public string Languages
        {
            get => AppState.Current.TGeneral.TextBox_Languages.Text;
            set => AppState.Current.TGeneral.TextBox_Languages.Text = value;
        }

        public string Profficiencies
        {
            get => AppState.Current.TGeneral.TextBox_Profficencies.Text;
            set => AppState.Current.TGeneral.TextBox_Profficencies.Text = value;
        }

        public void Clear()
        {
            this.Name = string.Empty;
            this.CurrentExp = this.MaxExp = 0;
            this.CurrentHP = this.MaxHP = 0;
            this.CurrentTempHP = this.MaxTempHP = 0;
            this.HitDice6Max = this.HitDice6Current = this.HitDice8Current = this.HitDice8Max = this.HitDice10Current = this.HitDice10Max = this.HitDice12Current = this.HitDice12Max = 0;
            this.Class = this.Race = this.Alignment = string.Empty;
            this.Traits = this.Bonds = this.Flaws = this.Ideals = this.Background = string.Empty;
            this.ProfficiencyBonus = this.Level = this.Speed = this.ArmorClass = 0;
            this.DeathThrowFails = this.DeathThrowPasses = 0;
            this.StatStr = this.StatDex = this.StatCon = this.StatCha = this.StatInt = this.StatWis = 0;
            this.StatModStr = this.StatModDex = this.StatModCon = this.StatModCha = this.StatModInt = this.StatModWis = 0;
            this.StrSave = this.DexSave = this.ConSave = this.ChaSave = this.WisSave = this.IntSave = 0;
            this.ProfficientAtStrSave = this.ProfficientAtDexSave = this.ProfficientAtConSave = this.ProfficientAtIntSave = this.ProfficientAtChaSave = this.ProfficientAtWisSave = false;
            this.Athletics = this.Acrobatics = this.SleightOfHand = this.Stealth = this.Arcana = this.History = this.Investigation = this.Nature = this.Religion = this.AnimalHandling = this.Insight = this.Medicine = this.Perception = this.Survival = this.Deception = this.Intimidation = this.Performance = this.Persuasion = 0;
            this.ProfficientAtAthletics = this.ProfficientAtAcrobatics = this.ProfficientAtSleightOfHand = this.ProfficientAtStealth = this.ProfficientAtArcana = this.ProfficientAtHistory = this.ProfficientAtInvestigation = this.ProfficientAtNature = this.ProfficientAtReligion = this.ProfficientAtAnimalHandling = this.ProfficientAtInsight = this.ProfficientAtMedicine = this.ProfficientAtPerception = this.ProfficientAtSurvival = this.ProfficientAtDeception = this.ProfficientAtIntimidation = this.ProfficientAtPerformance = this.ProfficientAtPersuasion = false;
            this.HasInspiration = false;
            this.Languages = this.Profficiencies = string.Empty;
        }
    }

    public sealed class Extras
    {
        public string Feats
        {
            get => AppState.Current.TExtras.TextBox_Feats.Text;
            set => AppState.Current.TExtras.TextBox_Feats.Text = value;
        }

        public string Traits
        {
            get => AppState.Current.TExtras.TextBox_Traits.Text;
            set => AppState.Current.TExtras.TextBox_Traits.Text = value;
        }

        public string Extra
        {
            get => AppState.Current.TExtras.TextBox_Extras.Text;
            set => AppState.Current.TExtras.TextBox_Extras.Text = value;
        }

        public string Bio
        {
            get => AppState.Current.TExtras.TextBox_Bio.Text;
            set => AppState.Current.TExtras.TextBox_Bio.Text = value;
        }

        public string Appearance
        {
            get => AppState.Current.TExtras.TextBox_Appearance.Text;
            set => AppState.Current.TExtras.TextBox_Appearance.Text = value;
        }

        public string Gender
        {
            get => AppState.Current.TExtras.ComboBox_Gender.Text;
            set => AppState.Current.TExtras.ComboBox_Gender.Text = value;
        }

        public int Age
        {
            get => AppState.Current.TExtras.IntUD_Age.Value ?? 0;
            set => AppState.Current.TExtras.IntUD_Age.Value = value;
        }

        public float Height
        {
            get => AppState.Current.TExtras.FloatUD_Height.Value ?? 0;
            set => AppState.Current.TExtras.FloatUD_Height.Value = value;
        }

        public float Weight
        {
            get => AppState.Current.TExtras.FloatUD_Weight.Value ?? 0;
            set => AppState.Current.TExtras.FloatUD_Weight.Value = value;
        }

        public void Clear()
        {
            this.Feats = this.Traits = this.Extra = this.Bio = this.Appearance = this.Gender = string.Empty;
            this.Age = 0;
            this.Height = this.Weight = 0;
        }
    }

    public sealed class Inventory
    {
        public int PP
        {
            get => AppState.Current.TInventory.IntUD_PP.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_PP.Value = value;
        }

        public int GP
        {
            get => AppState.Current.TInventory.IntUD_GP.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_GP.Value = value;
        }

        public int EP
        {
            get => AppState.Current.TInventory.IntUD_EP.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_EP.Value = value;
        }

        public int SP
        {
            get => AppState.Current.TInventory.IntUD_SP.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_SP.Value = value;
        }

        public int CP
        {
            get => AppState.Current.TInventory.IntUD_CP.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_CP.Value = value;
        }

        public string SortingMethod
        {
            get => AppState.Current.TInventory.ComboBox_SortBy.Text;
            set => AppState.Current.TInventory.ComboBox_SortBy.Text = value;
        }

        public bool SortInverted
        {
            get => AppState.Current.TInventory.CheckBox_ReverseSearchResults.IsChecked ?? false;
            set => AppState.Current.TInventory.CheckBox_ReverseSearchResults.IsChecked = value;
        }

        public string Filter
        {
            get => AppState.Current.TInventory.TextBox_Filter.Text;
            set => AppState.Current.TInventory.TextBox_Filter.Text = value;
        }

        public ObservableCollection<InventoryItem> Items
        {
            get => AppState.Current.TInventory.Items;
            set => AppState.Current.TInventory.ChangeItemCollection(value);
        }

        public float WeightCurrent
        {
            get => AppState.Current.TInventory.IntUD_WeightCurrent.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_WeightCurrent.Value = value;
        }

        public float WeightMax1
        {
            get => AppState.Current.TInventory.IntUD_WeightMax1.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_WeightMax1.Value = value;
        }

        public float WeightMax2
        {
            get => AppState.Current.TInventory.IntUD_WeightMax2.Value ?? 0;
            set => AppState.Current.TInventory.IntUD_WeightMax2.Value = value;
        }

        public InventoryItem Helmet
        {
            get => AppState.Current.TInventory.IIP_Head.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Head.SetDataContext(value);
        }

        public InventoryItem Necklace
        {
            get => AppState.Current.TInventory.IIP_Neck.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Neck.SetDataContext(value);
        }

        public InventoryItem Chestpiece
        {
            get => AppState.Current.TInventory.IIP_Chest.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Chest.SetDataContext(value);
        }

        public InventoryItem Leggings
        {
            get => AppState.Current.TInventory.IIP_Legs.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Legs.SetDataContext(value);
        }

        public InventoryItem Boots
        {
            get => AppState.Current.TInventory.IIP_Boots.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Boots.SetDataContext(value);
        }

        public InventoryItem WeaponLeft
        {
            get => AppState.Current.TInventory.IIP_LeftHand.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_LeftHand.SetDataContext(value);
        }

        public InventoryItem WeaponRight
        {
            get => AppState.Current.TInventory.IIP_RightHand.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_RightHand.SetDataContext(value);
        }

        public InventoryItem Misc1
        {
            get => AppState.Current.TInventory.IIP_Misc1.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Misc1.SetDataContext(value);
        }

        public InventoryItem Misc2
        {
            get => AppState.Current.TInventory.IIP_Misc2.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Misc2.SetDataContext(value);
        }

        public InventoryItem Ring0
        {
            get => AppState.Current.TInventory.IIP_Ring0.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring0.SetDataContext(value);
        }

        public InventoryItem Ring1
        {
            get => AppState.Current.TInventory.IIP_Ring1.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring1.SetDataContext(value);
        }

        public InventoryItem Ring2
        {
            get => AppState.Current.TInventory.IIP_Ring2.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring2.SetDataContext(value);
        }

        public InventoryItem Ring3
        {
            get => AppState.Current.TInventory.IIP_Ring3.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring3.SetDataContext(value);
        }

        public InventoryItem Ring4
        {
            get => AppState.Current.TInventory.IIP_Ring4.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring4.SetDataContext(value);
        }

        public InventoryItem Ring5
        {
            get => AppState.Current.TInventory.IIP_Ring5.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring5.SetDataContext(value);
        }

        public InventoryItem Ring6
        {
            get => AppState.Current.TInventory.IIP_Ring6.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring6.SetDataContext(value);
        }

        public InventoryItem Ring7
        {
            get => AppState.Current.TInventory.IIP_Ring7.DataContext as InventoryItem;
            set => AppState.Current.TInventory.IIP_Ring7.SetDataContext(value);
        }

        public void Clear()
        {
            this.PP = this.GP = this.EP = this.SP = this.CP = 0;
            this.WeightCurrent = this.WeightMax1 = this.WeightMax2 = 0;
            this.SortingMethod = Resources.Inventory_Sort_Name;
            this.SortInverted = false;
            this.Filter = string.Empty;
            this.Items.Clear();
            this.Helmet = this.Necklace = this.Chestpiece = this.Leggings = this.Boots = WeaponLeft = this.WeaponRight = this.Misc1 = this.Misc2 = this.Ring0 = this.Ring1 = this.Ring2 = this.Ring3 = this.Ring4 = this.Ring5 = this.Ring6 = this.Ring7 = null;
        }
    }

    public sealed class Spellbook
    {
        public string SpellcastingAbility
        {
            get => AppState.Current.TSpellbook.ComboBox_SpellcastingAbility.Text;
            set => AppState.Current.TSpellbook.ComboBox_SpellcastingAbility.Text = value;
        }

        public int SpellSaveDC
        {
            get => AppState.Current.TSpellbook.IntUD_SpellSave.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_SpellSave.Value = value;
        }

        public int SpellAttackBonus
        {
            get => AppState.Current.TSpellbook.IntUD_SpellAttackAbility.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_SpellAttackAbility.Value = value;
        }

        public int MaxSpells
        {
            get => AppState.Current.TSpellbook.IntUD_SpellsAmount.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_SpellsAmount.Value = value;
        }

        public int SpellSlots0Current
        {
            get => AppState.Current.TSpellbook.IntUD_0CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_0CS.Value = value;
        }

        public int SpellSlots1Current
        {
            get => AppState.Current.TSpellbook.IntUD_1CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_1CS.Value = value;
        }

        public int SpellSlots2Current
        {
            get => AppState.Current.TSpellbook.IntUD_2CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_2CS.Value = value;
        }

        public int SpellSlots3Current
        {
            get => AppState.Current.TSpellbook.IntUD_3CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_3CS.Value = value;
        }

        public int SpellSlots4Current
        {
            get => AppState.Current.TSpellbook.IntUD_4CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_4CS.Value = value;
        }

        public int SpellSlots5Current
        {
            get => AppState.Current.TSpellbook.IntUD_5CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_5CS.Value = value;
        }

        public int SpellSlots6Current
        {
            get => AppState.Current.TSpellbook.IntUD_6CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_6CS.Value = value;
        }

        public int SpellSlots7Current
        {
            get => AppState.Current.TSpellbook.IntUD_7CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_7CS.Value = value;
        }

        public int SpellSlots8Current
        {
            get => AppState.Current.TSpellbook.IntUD_8CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_8CS.Value = value;
        }

        public int SpellSlots9Current
        {
            get => AppState.Current.TSpellbook.IntUD_9CS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_9CS.Value = value;
        }

        public int SpellSlots0Max
        {
            get => AppState.Current.TSpellbook.IntUD_0MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_0MS.Value = value;
        }

        public int SpellSlots1Max
        {
            get => AppState.Current.TSpellbook.IntUD_1MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_1MS.Value = value;
        }

        public int SpellSlots2Max
        {
            get => AppState.Current.TSpellbook.IntUD_2MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_2MS.Value = value;
        }

        public int SpellSlots3Max
        {
            get => AppState.Current.TSpellbook.IntUD_3MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_3MS.Value = value;
        }

        public int SpellSlots4Max
        {
            get => AppState.Current.TSpellbook.IntUD_4MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_4MS.Value = value;
        }

        public int SpellSlots5Max
        {
            get => AppState.Current.TSpellbook.IntUD_5MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_5MS.Value = value;
        }

        public int SpellSlots6Max
        {
            get => AppState.Current.TSpellbook.IntUD_6MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_6MS.Value = value;
        }

        public int SpellSlots7Max
        {
            get => AppState.Current.TSpellbook.IntUD_7MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_7MS.Value = value;
        }

        public int SpellSlots8Max
        {
            get => AppState.Current.TSpellbook.IntUD_8MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_8MS.Value = value;
        }

        public int SpellSlots9Max
        {
            get => AppState.Current.TSpellbook.IntUD_9MS.Value ?? 0;
            set => AppState.Current.TSpellbook.IntUD_9MS.Value = value;
        }

        public ObservableCollection<Spell> Cantrips
        {
            get => AppState.Current.TSpellbook.Spells0;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 0);
        }

        public ObservableCollection<Spell> Lvl1Spells
        {
            get => AppState.Current.TSpellbook.Spells1;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 1);
        }

        public ObservableCollection<Spell> Lvl2Spells
        {
            get => AppState.Current.TSpellbook.Spells2;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 2);
        }

        public ObservableCollection<Spell> Lvl3Spells
        {
            get => AppState.Current.TSpellbook.Spells3;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 3);
        }

        public ObservableCollection<Spell> Lvl4Spells
        {
            get => AppState.Current.TSpellbook.Spells4;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 4);
        }

        public ObservableCollection<Spell> Lvl5Spells
        {
            get => AppState.Current.TSpellbook.Spells5;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 5);
        }

        public ObservableCollection<Spell> Lvl6Spells
        {
            get => AppState.Current.TSpellbook.Spells6;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 6);
        }

        public ObservableCollection<Spell> Lvl7Spells
        {
            get => AppState.Current.TSpellbook.Spells7;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 7);
        }

        public ObservableCollection<Spell> Lvl8Spells
        {
            get => AppState.Current.TSpellbook.Spells8;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 8);
        }

        public ObservableCollection<Spell> Lvl9Spells
        {
            get => AppState.Current.TSpellbook.Spells9;
            set => AppState.Current.TSpellbook.SetSpellCollection(value, 9);
        }

        public void Clear()
        {
            this.SpellcastingAbility = string.Empty;
            this.SpellAttackBonus = this.SpellSaveDC = this.MaxSpells = 0;
            this.SpellSlots0Current = this.SpellSlots0Max = this.SpellSlots1Current = this.SpellSlots1Max = this.SpellSlots2Current = this.SpellSlots2Max = this.SpellSlots3Current = this.SpellSlots3Max = this.SpellSlots4Current = this.SpellSlots4Max = this.SpellSlots5Current = this.SpellSlots5Max = this.SpellSlots6Current = this.SpellSlots6Max = this.SpellSlots7Current = this.SpellSlots7Max = this.SpellSlots8Current = this.SpellSlots8Max = this.SpellSlots9Current = this.SpellSlots9Max = 0;
            this.Cantrips.Clear();
            this.Lvl1Spells.Clear();
            this.Lvl2Spells.Clear();
            this.Lvl3Spells.Clear();
            this.Lvl4Spells.Clear();
            this.Lvl5Spells.Clear();
            this.Lvl6Spells.Clear();
            this.Lvl7Spells.Clear();
            this.Lvl8Spells.Clear();
            this.Lvl9Spells.Clear();
        }
    }
}
