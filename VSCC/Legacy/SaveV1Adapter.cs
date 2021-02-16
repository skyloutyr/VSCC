namespace VSCC.Legacy
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.ObjectModel;
    using VSCC.DataType;
    using VSCC.State;

    public class SaveV1Adapter
    {
        public static void Load(string s)
        {
            JObject from = JObject.Parse(s);
            ReadGeneral(from["General"] as JObject);
            ReadExtras(from["Extras"] as JObject);
            ReadInventory(from["Inventory"] as JObject);
            ReadSpells(from["Spells"] as JObject);
        }

        private static void ReadGeneral(JObject from)
        {
            AppState.Current.State.General.Name = from["Name"].ToObject<string>();
            (int, int) exp = AdaptBar(from["Exp"].ToObject<string>());
            AppState.Current.State.General.CurrentExp = exp.Item1;
            AppState.Current.State.General.MaxExp = exp.Item2;
            (int, int) hp = AdaptBar(from["HP"].ToObject<string>());
            AppState.Current.State.General.CurrentHP = hp.Item1;
            AppState.Current.State.General.MaxHP = hp.Item2;
            (int, int) tempHP = AdaptBar(from["THP"].ToObject<string>());
            AppState.Current.State.General.CurrentTempHP = tempHP.Item1;
            AppState.Current.State.General.MaxTempHP = tempHP.Item2;
            AppState.Current.State.General.Level = from["Lvl"].ToObject<int>();
            AppState.Current.State.General.Class = from["Class"].ToObject<string>();
            AppState.Current.State.General.Race = from["Race"].ToObject<string>();
            AppState.Current.State.General.Alignment = from["Alignment"].ToObject<string>();
            AppState.Current.State.General.Background = from["Background"].ToObject<string>();
            AppState.Current.State.General.Traits = from["Traits"].ToObject<string>();
            AppState.Current.State.General.Ideals = from["Ideals"].ToObject<string>();
            AppState.Current.State.General.Bonds = from["Bonds"].ToObject<string>();
            AppState.Current.State.General.Flaws = from["Flaws"].ToObject<string>();
            AppState.Current.State.General.ProfficiencyBonus = from["Prof"].ToObject<int>();
            AppState.Current.State.General.ArmorClass = from["AC"].ToObject<int>();
            AppState.Current.State.General.Speed = from["Speed"].ToObject<int>();
            AppState.Current.State.General.HitDice12Current = from["HD12C"].ToObject<int>();
            AppState.Current.State.General.HitDice10Current = from["HD10C"].ToObject<int>();
            AppState.Current.State.General.HitDice8Current = from["HD8C"].ToObject<int>();
            AppState.Current.State.General.HitDice6Current = from["HD6C"].ToObject<int>();
            AppState.Current.State.General.HitDice12Max = from["HD12M"].ToObject<int>();
            AppState.Current.State.General.HitDice10Max = from["HD10M"].ToObject<int>();
            AppState.Current.State.General.HitDice8Max = from["HD8M"].ToObject<int>();
            AppState.Current.State.General.HitDice6Max = from["HD6M"].ToObject<int>();
            AppState.Current.State.General.StatStr = from["Str"].ToObject<int>();
            AppState.Current.State.General.StatDex = from["Dex"].ToObject<int>();
            AppState.Current.State.General.StatCon = from["Con"].ToObject<int>();
            AppState.Current.State.General.StatCha = from["Cha"].ToObject<int>();
            AppState.Current.State.General.StatInt = from["Int"].ToObject<int>();
            AppState.Current.State.General.StatWis = from["Wis"].ToObject<int>();
            AppState.Current.State.General.Languages = from["Langs"].ToObject<string>();
            AppState.Current.State.General.Profficiencies = from["Profs"].ToObject<string>();
            for (int i = 0; i < 24; ++i)
            {
                (Action<bool>, Action<int>) routers = AdaptSkillsSaves(i);
                routers.Item1(from[$"SS{ i }K"].ToObject<bool>());
                routers.Item2(from[$"SS{ i }V"].ToObject<int>());
            }

            AppState.Current.State.General.DeathThrowFails = AdaptDeathData(from["D0"].ToObject<bool>(), from["D1"].ToObject<bool>(), from["D2"].ToObject<bool>());
            AppState.Current.State.General.DeathThrowPasses = AdaptDeathData(from["S0"].ToObject<bool>(), from["S1"].ToObject<bool>(), from["S2"].ToObject<bool>());
            AppState.Current.State.General.HasInspiration = from["Inspiration"].ToObject<bool>();
        }

        private static void ReadExtras(JObject from)
        {
            foreach (string line in from["Feats"].ToObject<string>().Split('\n'))
            {
                Feat f = new Feat { ImageList = AppState.Current.TExtras.Images, DescProperty = MainWindow.Translate("Feat_Desc_NeedsConversion"), NameProperty = MainWindow.Translate("Feat_Name_Old"), FullDescProperty = line, ImageIndex = "if886_t" };
                AppState.Current.State.Extras.FeatsArray.Add(f);
            }

            foreach (string line in from["Traits"].ToObject<string>().Split('\n'))
            {
                Feat f = new Feat { ImageList = AppState.Current.TExtras.Images, DescProperty = MainWindow.Translate("Feat_Desc_NeedsConversion"), NameProperty = MainWindow.Translate("Feat_Name_Old"), FullDescProperty = line, ImageIndex = "if886_t" };
                AppState.Current.State.Extras.TraitsArray.Add(f);
            }

            AppState.Current.State.Extras.Extra = from["Extra"].ToObject<string>();
            AppState.Current.State.Extras.Bio = from["Bio"].ToObject<string>();
            AppState.Current.State.Extras.Appearance = from["Appearance"].ToObject<string>();
            AppState.Current.State.Extras.Age = from["Age"].ToObject<int>();
            AppState.Current.State.Extras.Height = from["Height"].ToObject<float>();
            AppState.Current.State.Extras.Weight = from["Weight"].ToObject<float>();
            AppState.Current.State.Extras.Gender = from["Gender"].ToObject<string>();
        }

        private static void ReadInventory(JObject from)
        {
            AppState.Current.State.Inventory.PP = from["PP"].ToObject<int>();
            AppState.Current.State.Inventory.GP = from["GP"].ToObject<int>();
            AppState.Current.State.Inventory.EP = from["EP"].ToObject<int>();
            AppState.Current.State.Inventory.SP = from["SP"].ToObject<int>();
            AppState.Current.State.Inventory.CP = from["CP"].ToObject<int>();
            AppState.Current.State.Inventory.WeightCurrent = from.ContainsKey("Weight") ? from["Weight"].ToObject<int>() : 0;
            AppState.Current.State.Inventory.WeightMax1 = from.ContainsKey("MidWeight") ? from["MidWeight"].ToObject<int>() : 0;
            AppState.Current.State.Inventory.WeightMax2 = from.ContainsKey("MaxWeight") ? from["MaxWeight"].ToObject<int>() : 0;
            Action<InventoryItem>[] iiPanelSetters = {
                v => AppState.Current.State.Inventory.Misc1 = v,
                v => AppState.Current.State.Inventory.Helmet = v,
                v => AppState.Current.State.Inventory.Misc2 = v,
                v => AppState.Current.State.Inventory.WeaponRight = v,
                v => AppState.Current.State.Inventory.Chestpiece = v,
                v => AppState.Current.State.Inventory.WeaponLeft = v,
                v => AppState.Current.State.Inventory.Boots = v,
                v => AppState.Current.State.Inventory.Ring0 = v,
                v => AppState.Current.State.Inventory.Ring1 = v,
                v => AppState.Current.State.Inventory.Ring2 = v,
                v => AppState.Current.State.Inventory.Ring3 = v,
                v => AppState.Current.State.Inventory.Ring4 = v,
                v => AppState.Current.State.Inventory.Ring5 = v,
                v => AppState.Current.State.Inventory.Ring6 = v,
                v => AppState.Current.State.Inventory.Ring7 = v
            };

            int eqIndex = 0;
            while (from.ContainsKey($"Equip{ eqIndex }"))
            {
                JObject jo = from[$"Equip{ eqIndex++ }"] as JObject;
                InventoryItem ii = InventoryItemLegacyAdapter.Apply(jo);
                ii.ImageList = AppState.Current.TInventory.Images;
                if (eqIndex - 1 < iiPanelSetters.Length)
                {
                    iiPanelSetters[eqIndex - 1](ii);
                }
            }

            AppState.Current.State.Inventory.Items.Clear();
            for (int i = 0; i < from["Items"].ToObject<int>(); ++i)
            {
                JObject jI = from[$"Item{ i }"] as JObject;
                InventoryItem ii = InventoryItemLegacyAdapter.Apply(jI["Item"] as JObject);
                ii.ImageList = AppState.Current.TInventory.Images;
                AppState.Current.State.Inventory.Items.Add(ii);
            }
        }

        private static void ReadSpells(JObject from)
        {
            AppState.Current.State.Spellbook.SpellcastingAbility = from["Ability"].ToObject<string>();
            AppState.Current.State.Spellbook.SpellSaveDC = from["Save"].ToObject<int>();
            AppState.Current.State.Spellbook.SpellAttackBonus = from["Attack"].ToObject<int>();
            for (int i = 0; i < 10; ++i)
            {
                (Action<int>, Action<int>, ObservableCollection<Spell>) spellPage = AdaptSpellSlots(i);
                JObject s = from[$"Spell{ i }"] as JObject;
                if (i != 0)
                {
                    spellPage.Item1(s[$"Slot0"].ToObject<int>());
                    spellPage.Item2(s[$"Slot1"].ToObject<int>());
                }

                spellPage.Item3.Clear();
                for (int j = 0; j < s["Spells"].ToObject<int>(); ++j)
                {
                    Spell spell = SpellLegacyAdapter.Apply(s[$"Spell{ j }"] as JObject);
                    spell.ImageList = AppState.Current.TSpellbook.Images;
                    spellPage.Item3.Add(spell);
                }
            }
        }

        private static (Action<int>, Action<int>, ObservableCollection<Spell>) AdaptSpellSlots(int i)
        {
            switch (i)
            {
                case 0:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots0Current = j), (j => AppState.Current.State.Spellbook.SpellSlots0Max = j), AppState.Current.State.Spellbook.Cantrips);

                case 1:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots1Current = j), (j => AppState.Current.State.Spellbook.SpellSlots1Max = j), AppState.Current.State.Spellbook.Lvl1Spells);

                case 2:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots2Current = j), (j => AppState.Current.State.Spellbook.SpellSlots2Max = j), AppState.Current.State.Spellbook.Lvl2Spells);

                case 3:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots3Current = j), (j => AppState.Current.State.Spellbook.SpellSlots3Max = j), AppState.Current.State.Spellbook.Lvl3Spells);

                case 4:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots4Current = j), (j => AppState.Current.State.Spellbook.SpellSlots4Max = j), AppState.Current.State.Spellbook.Lvl4Spells);

                case 5:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots5Current = j), (j => AppState.Current.State.Spellbook.SpellSlots5Max = j), AppState.Current.State.Spellbook.Lvl5Spells);

                case 6:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots6Current = j), (j => AppState.Current.State.Spellbook.SpellSlots6Max = j), AppState.Current.State.Spellbook.Lvl6Spells);

                case 7:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots7Current = j), (j => AppState.Current.State.Spellbook.SpellSlots7Max = j), AppState.Current.State.Spellbook.Lvl7Spells);

                case 8:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots8Current = j), (j => AppState.Current.State.Spellbook.SpellSlots8Max = j), AppState.Current.State.Spellbook.Lvl8Spells);

                case 9:
                    return ((j => AppState.Current.State.Spellbook.SpellSlots9Current = j), (j => AppState.Current.State.Spellbook.SpellSlots9Max = j), AppState.Current.State.Spellbook.Lvl9Spells);

                default:
                    return ((j => { }), (j => { }), new ObservableCollection<Spell>());
            }
        }

        private static int AdaptDeathData(bool b0, bool b1, bool b2) => 0 + (b0 ? 1 : 0) + (b1 ? 1 : 0) + (b2 ? 1 : 0);

        private static (Action<bool>, Action<int>) AdaptSkillsSaves(int i)
        {
            switch (i)
            {
                case 0:
                    return ((b => AppState.Current.State.General.ProfficientAtStrSave = b), (j => AppState.Current.State.General.StrSave = j));

                case 1:
                    return ((b => AppState.Current.State.General.ProfficientAtDexSave = b), (j => AppState.Current.State.General.DexSave = j));

                case 2:
                    return ((b => AppState.Current.State.General.ProfficientAtConSave = b), (j => AppState.Current.State.General.ConSave = j));

                case 3:
                    return ((b => AppState.Current.State.General.ProfficientAtIntSave = b), (j => AppState.Current.State.General.IntSave = j));

                case 4:
                    return ((b => AppState.Current.State.General.ProfficientAtWisSave = b), (j => AppState.Current.State.General.WisSave = j));

                case 5:
                    return ((b => AppState.Current.State.General.ProfficientAtChaSave = b), (j => AppState.Current.State.General.ChaSave = j));

                case 6:
                    return ((b => AppState.Current.State.General.ProfficientAtAthletics = b), (j => AppState.Current.State.General.Athletics = j));

                case 7:
                    return ((b => AppState.Current.State.General.ProfficientAtAcrobatics = b), (j => AppState.Current.State.General.Acrobatics = j));

                case 8:
                    return ((b => AppState.Current.State.General.ProfficientAtSleightOfHand = b), (j => AppState.Current.State.General.SleightOfHand = j));

                case 9:
                    return ((b => AppState.Current.State.General.ProfficientAtStealth = b), (j => AppState.Current.State.General.Stealth = j));

                case 10:
                    return ((b => AppState.Current.State.General.ProfficientAtArcana = b), (j => AppState.Current.State.General.Arcana = j));

                case 11:
                    return ((b => AppState.Current.State.General.ProfficientAtHistory = b), (j => AppState.Current.State.General.History = j));

                case 12:
                    return ((b => AppState.Current.State.General.ProfficientAtInvestigation = b), (j => AppState.Current.State.General.Investigation = j));

                case 13:
                    return ((b => AppState.Current.State.General.ProfficientAtNature = b), (j => AppState.Current.State.General.Nature = j));

                case 14:
                    return ((b => AppState.Current.State.General.ProfficientAtReligion = b), (j => AppState.Current.State.General.Religion = j));

                case 15:
                    return ((b => AppState.Current.State.General.ProfficientAtAnimalHandling = b), (j => AppState.Current.State.General.AnimalHandling = j));

                case 16:
                    return ((b => AppState.Current.State.General.ProfficientAtInsight = b), (j => AppState.Current.State.General.Insight = j));

                case 17:
                    return ((b => AppState.Current.State.General.ProfficientAtMedicine = b), (j => AppState.Current.State.General.Medicine = j));

                case 18:
                    return ((b => AppState.Current.State.General.ProfficientAtPerception = b), (j => AppState.Current.State.General.Perception = j));

                case 19:
                    return ((b => AppState.Current.State.General.ProfficientAtSurvival = b), (j => AppState.Current.State.General.Survival = j));

                case 20:
                    return ((b => AppState.Current.State.General.ProfficientAtDeception = b), (j => AppState.Current.State.General.Deception = j));

                case 21:
                    return ((b => AppState.Current.State.General.ProfficientAtIntimidation = b), (j => AppState.Current.State.General.Intimidation = j));

                case 22:
                    return ((b => AppState.Current.State.General.ProfficientAtPerformance = b), (j => AppState.Current.State.General.Performance = j));

                case 23:
                    return ((b => AppState.Current.State.General.ProfficientAtPersuasion = b), (j => AppState.Current.State.General.Persuasion = j));

                default:
                    return ((b => { }), (j => { }));
            }
        }

        private static (int, int) AdaptBar(string s)
        {
            string[] splt = s.Split('/');
            if (splt.Length != 2)
            {
                return (0, 0);
            }

            int.TryParse(splt[0], out int l);
            int.TryParse(splt[1], out int r);
            return (l, r);
        }
    }
}
