﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VSCC.Roll20;
using VSCC.State;

namespace VSCC.Controls.Tabs
{
    /// <summary>
    /// Interaction logic for Roll20Tab.xaml
    /// </summary>
    public partial class Roll20Tab : UserControl
    {
        public Roll20Tab() => this.InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (Stream s = Assembly.GetEntryAssembly().GetManifestResourceStream("VSCC.Roll20.roll20script.js"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    Clipboard.SetText(sr.ReadToEnd());
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Button_Click(sender, e);
            R20WSServer.CreateServer();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            R20WSServer.ServerStartCallback = new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Label_ServerStatus.Content = Properties.Resources.R20_SS_Listening;
                    this.Border_ServerStatus.Background = Brushes.Gold;
                });
            });

            R20WSServer.ServerStopCallback = new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Label_ServerStatus.Content = Properties.Resources.R20_SS_NotStarted;
                    this.Border_ServerStatus.Background = Brushes.Black;
                });
            });

            R20WSServer.ClientConnectCallback = new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Label_ServerStatus.Content = Properties.Resources.R20_SS_Connection;
                    this.Border_ServerStatus.Background = Brushes.Green;
                });
            });

            R20WSServer.ClientDisconnectCallback = new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Label_ServerStatus.Content = Properties.Resources.R20_SS_ConLost;
                    this.Border_ServerStatus.Background = Brushes.Red;
                });
            });

            R20WSServer.Logger = new Action<string>(s =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.TextBlock_Log.Text += s + '\n';
                });
            });
        }

        private void RollDieSimple(string die) => R20WSServer.Roll(die, die, "Simple Roll", null);

        private void RollDieStat(string statname, int stat)
        {
            string die = "1d20" + (stat < 0 ? "-" : "+") + Math.Abs(stat).ToString();
            R20WSServer.Roll(die, die, "Save/Skill Check", statname);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) => this.RollDieSimple("1d4");

        private void Button_Click_3(object sender, RoutedEventArgs e) => this.RollDieSimple("1d6");

        private void Button_Click_4(object sender, RoutedEventArgs e) => this.RollDieSimple("1d8");

        private void Button_Click_5(object sender, RoutedEventArgs e) => this.RollDieSimple("1d10");

        private void Button_Click_6(object sender, RoutedEventArgs e) => this.RollDieSimple("1d12");

        private void Button_Click_7(object sender, RoutedEventArgs e) => this.RollDieSimple("1d20");

        private void Button_Click_8(object sender, RoutedEventArgs e) => this.RollDieSimple("1d100");

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (!this.CheckBox_AsSimpleRoll.IsChecked ?? false)
            {
                this.RollDieSimple($"{ this.IntUD_AdvRollNumDice.Value }d{ this.IntUD_AdvRollSidesDice.Value }{ this.ComboBox_AdvRollMathType.Text }{ this.IntUD_AdvRollMod.Value }");
            }
            else
            {
                R20WSServer.Send(new CommandPacket()
                {
                    Template = Roll20.Template.None,
                    Data = new TemplateDataManyRolls() { Roll = $"{ this.IntUD_AdvRollNumDice.Value }d{ this.IntUD_AdvRollSidesDice.Value }{ this.ComboBox_AdvRollMathType.Text }{ this.IntUD_AdvRollMod.Value }" }
                });
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e) => this.RollDieStat("Athletics", AppState.Current.State.General.Athletics);

        private void Button_Click_11(object sender, RoutedEventArgs e) => this.RollDieStat("Strength", AppState.Current.State.General.StrSave);

        private void Button_Click_12(object sender, RoutedEventArgs e) => this.RollDieStat("Dexterity", AppState.Current.State.General.DexSave);

        private void Button_Click_13(object sender, RoutedEventArgs e) => this.RollDieStat("Wisdom", AppState.Current.State.General.WisSave);

        private void Button_Click_14(object sender, RoutedEventArgs e) => this.RollDieStat("Constitution", AppState.Current.State.General.ConSave);

        private void Button_Click_15(object sender, RoutedEventArgs e) => this.RollDieStat("Charisma", AppState.Current.State.General.ChaSave);

        private void Button_Click_16(object sender, RoutedEventArgs e) => this.RollDieStat("Intelligence", AppState.Current.State.General.IntSave);

        private void Button_Click_17(object sender, RoutedEventArgs e) => this.RollDieStat("Acrobatics", AppState.Current.State.General.Acrobatics);

        private void Button_Click_18(object sender, RoutedEventArgs e) => this.RollDieStat("Stealth", AppState.Current.State.General.Stealth);

        private void Button_Click_19(object sender, RoutedEventArgs e) => this.RollDieStat("Sleight Of Hand", AppState.Current.State.General.SleightOfHand);

        private void Button_Click_20(object sender, RoutedEventArgs e) => this.RollDieStat("Animal Handling", AppState.Current.State.General.AnimalHandling);

        private void Button_Click_21(object sender, RoutedEventArgs e) => this.RollDieStat("Medicine", AppState.Current.State.General.Medicine);

        private void Button_Click_22(object sender, RoutedEventArgs e) => this.RollDieStat("Survival", AppState.Current.State.General.Survival);

        private void Button_Click_23(object sender, RoutedEventArgs e) => this.RollDieStat("Insight", AppState.Current.State.General.Insight);

        private void Button_Click_24(object sender, RoutedEventArgs e) => this.RollDieStat("Perception", AppState.Current.State.General.Perception);

        private void Button_Click_25(object sender, RoutedEventArgs e) => this.RollDieStat("Arcana", AppState.Current.State.General.Arcana);

        private void Button_Click_26(object sender, RoutedEventArgs e) => this.RollDieStat("Investigation", AppState.Current.State.General.Investigation);

        private void Button_Click_27(object sender, RoutedEventArgs e) => this.RollDieStat("History", AppState.Current.State.General.History);

        private void Button_Click_28(object sender, RoutedEventArgs e) => this.RollDieStat("Nature", AppState.Current.State.General.Nature);

        private void Button_Click_29(object sender, RoutedEventArgs e) => this.RollDieStat("Religion", AppState.Current.State.General.Religion);

        private void Button_Click_30(object sender, RoutedEventArgs e) => this.RollDieStat("Deception", AppState.Current.State.General.Deception);

        private void Button_Click_31(object sender, RoutedEventArgs e) => this.RollDieStat("Intimidation", AppState.Current.State.General.Intimidation);

        private void Button_Click_32(object sender, RoutedEventArgs e) => this.RollDieStat("Performance", AppState.Current.State.General.Performance);

        private void Button_Click_33(object sender, RoutedEventArgs e) => this.RollDieStat("Persuasion", AppState.Current.State.General.Persuasion);

        private void Button_Click_34(object sender, RoutedEventArgs e) => R20WSServer.CloseServer();
    }
}
