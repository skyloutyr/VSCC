namespace VSCC.Roll20.Macros
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Controls;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.Roll20.Macros.Actions;
    using VSCC.State;

    public class Macro
    {
        public static List<Action<Macro, List<string>>> InbetweenMacroActions { get; } = new List<Action<Macro, List<string>>>();

        #region Locals
        public Dictionary<string, Tuple<int, int>> NumberLocals { get; } = new Dictionary<string, Tuple<int, int>>();
        public Dictionary<string, Tuple<float, float>> RealLocals { get; } = new Dictionary<string, Tuple<float, float>>();
        public Dictionary<string, Tuple<string, string>> StringLocals { get; } = new Dictionary<string, Tuple<string, string>>();
        public Dictionary<string, Tuple<bool, bool>> BoolLocals { get; } = new Dictionary<string, Tuple<bool, bool>>();
        #endregion

        #region Links
        public Dictionary<string, Guid> ItemsLinked { get; } = new Dictionary<string, Guid>();
        public Dictionary<string, Guid> SpellsLinked { get; } = new Dictionary<string, Guid>();
        #endregion

        public LinkedList<MacroAction> Actions { get; } = new LinkedList<MacroAction>();

        public string Name { get; set; }
        public string Description { get; set; }

        public void Populate(EditMacroWindow win, TreeViewItem links, TreeViewItem locals, TreeViewItem actions)
        {
            foreach (KeyValuePair<string, Guid> ilinks in this.ItemsLinked)
            {
                InventoryItem ii = AppState.Current.State.Inventory.Items.FirstOrDefault(i => i.ObjectID.Equals(ilinks.Value));
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ ilinks.Key }: {(ii == null ? MainWindow.Translate("Macro_Generic_Err_NoLink") : (ii.Name + "|" + ii.ObjectID.ToString()))}" };
                tvi.Tag = new Tuple<string, bool, Guid>(ilinks.Key, true, ilinks.Value);
                win.AssignMenuToLink(tvi);
                links.Items.Add(tvi);
            }

            foreach (KeyValuePair<string, Guid> slinks in this.SpellsLinked)
            {
                Spell s = AppState.Current.State.Spellbook.AllSpells.FirstOrDefault(i => i.ObjectID.Equals(slinks.Value));
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ slinks.Key }: {(s == null ? MainWindow.Translate("Macro_Generic_Err_NoLink") : (s.Name + "|" + s.ObjectID.ToString()))}" };
                tvi.Tag = new Tuple<string, bool, Guid>(slinks.Key, false, slinks.Value);
                win.AssignMenuToLink(tvi);
                links.Items.Add(tvi);
            }

            foreach (KeyValuePair<string, Tuple<int, int>> intLocal in this.NumberLocals)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ MainWindow.Translate("Macro_Type_Number") } { intLocal.Key } = { intLocal.Value.Item2 }" };
                win.AssignMenuToLocal(tvi);
                tvi.Tag = new Tuple<string, LocalType>(intLocal.Key, LocalType.Integer);
                locals.Items.Add(tvi);
            }

            foreach (KeyValuePair<string, Tuple<float, float>> floatLocal in this.RealLocals)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ MainWindow.Translate("Macro_Type_Real") } { floatLocal.Key } = { floatLocal.Value.Item2 }" };
                win.AssignMenuToLocal(tvi);
                tvi.Tag = new Tuple<string, LocalType>(floatLocal.Key, LocalType.Real);
                locals.Items.Add(tvi);
            }

            foreach (KeyValuePair<string, Tuple<bool, bool>> boolLocal in this.BoolLocals)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ MainWindow.Translate("Macro_Type_Boolean") } { boolLocal.Key } = { boolLocal.Value.Item2 }" };
                win.AssignMenuToLocal(tvi);
                tvi.Tag = new Tuple<string, LocalType>(boolLocal.Key, LocalType.Boolean);
                locals.Items.Add(tvi);
            }

            foreach (KeyValuePair<string, Tuple<string, string>> stringLocal in this.StringLocals)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ MainWindow.Translate("Macro_Type_Text") } { stringLocal.Key } = { stringLocal.Value.Item2 }" };
                win.AssignMenuToLocal(tvi);
                tvi.Tag = new Tuple<string, LocalType>(stringLocal.Key, LocalType.String);
                locals.Items.Add(tvi);
            }

            this.RecursivelyAddActions(win, actions, this.Actions);
            links.InvalidateVisual();
            locals.InvalidateVisual();
            actions.InvalidateVisual();
            links.ExpandSubtree();
            locals.ExpandSubtree();
            actions.ExpandSubtree();
        }

        public void RecursivelyAddActions(EditMacroWindow win, TreeViewItem root, LinkedList<MacroAction> actions)
        {
            foreach (MacroAction ma in actions)
            {
                if (ma is MacroActionCondition mac)
                {
                    TreeViewItem exp = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_Condition"), Tag = mac };
                    win.AssignConditionalExpressionMenu(exp);
                    TreeViewItem conditions = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_If"), Tag = mac.If };
                    TreeViewItem thens = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_Then"), Tag = mac.Then };
                    TreeViewItem elses = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_Else"), Tag = mac.Else };
                    win.AssignIfConditionsMenu(conditions);
                    win.AssignRootActionsMenu(thens);
                    win.AssignRootActionsMenu(elses);
                    exp.Items.Add(conditions);
                    exp.Items.Add(thens);
                    exp.Items.Add(elses);
                    this.RecursivelyAddActions(win, conditions, mac.If);
                    this.RecursivelyAddActions(win, thens, mac.Then);
                    this.RecursivelyAddActions(win, elses, mac.Else);
                    root.Items.Add(exp);
                }
                else
                {
                    TreeViewItem tvi = new TreeViewItem() { Header = ma.CreateFullInnerText(), Tag = ma };
                    win.AssignGenericActionMenu(tvi);
                    root.Items.Add(tvi);
                }
            }
        }

        public void Execute(List<string> errors)
        {
            foreach (string s in this.NumberLocals.Keys.ToList().AsReadOnly())
            {
                this.NumberLocals[s] = new Tuple<int, int>(this.NumberLocals[s].Item2, this.NumberLocals[s].Item2);
            }

            foreach (string s in this.RealLocals.Keys.ToList().AsReadOnly())
            {
                this.RealLocals[s] = new Tuple<float, float>(this.RealLocals[s].Item2, this.RealLocals[s].Item2);
            }

            foreach (string s in this.StringLocals.Keys.ToList().AsReadOnly())
            {
                this.StringLocals[s] = new Tuple<string, string>(this.StringLocals[s].Item2, this.StringLocals[s].Item2);
            }

            foreach (string s in this.BoolLocals.Keys.ToList().AsReadOnly())
            {
                this.BoolLocals[s] = new Tuple<bool, bool>(this.BoolLocals[s].Item2, this.BoolLocals[s].Item2);
            }

            foreach (Action<Macro, List<string>> a in InbetweenMacroActions)
            {
                a(this, errors);
            }

            InbetweenMacroActions.Clear();
            foreach (MacroAction ma in this.Actions)
            {
                ma.Execute(this, errors);
            }
        }

        #region Serialization
        public void Serialize(BinaryWriter bw)
        {
            bw.Write(this.Name);
            bw.Write(this.Description);
            bw.Write(this.NumberLocals.Count);
            foreach (KeyValuePair<string, Tuple<int, int>> kv in this.NumberLocals)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.Item1);
                bw.Write(kv.Value.Item2);
            }

            bw.Write(this.RealLocals.Count);
            foreach (KeyValuePair<string, Tuple<float, float>> kv in this.RealLocals)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.Item1);
                bw.Write(kv.Value.Item2);
            }

            bw.Write(this.StringLocals.Count);
            foreach (KeyValuePair<string, Tuple<string, string>> kv in this.StringLocals)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.Item1);
                bw.Write(kv.Value.Item2);
            }

            bw.Write(this.BoolLocals.Count);
            foreach (KeyValuePair<string, Tuple<bool, bool>> kv in this.BoolLocals)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.Item1);
                bw.Write(kv.Value.Item2);
            }

            bw.Write(this.ItemsLinked.Count);
            foreach (KeyValuePair<string, Guid> kv in this.ItemsLinked)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.ToByteArray());
            }

            bw.Write(this.SpellsLinked.Count);
            foreach (KeyValuePair<string, Guid> kv in this.SpellsLinked)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.ToByteArray());
            }

            bw.Write(this.Actions.Count);
            foreach (MacroAction ma in this.Actions)
            {
                MacroSerializer.WriteMacroAction(bw, ma);
            }
        }

        public void Deserialize(BinaryReader br)
        {
            this.Name = br.ReadString();
            this.Description = br.ReadString();
            int i = br.ReadInt32();
            this.NumberLocals.Clear();
            while (i-- > 0)
            {
                string s = br.ReadString();
                int n1 = br.ReadInt32();
                int n2 = br.ReadInt32();
                this.NumberLocals.Add(s, new Tuple<int, int>(n1, n2));
            }

            i = br.ReadInt32();
            this.RealLocals.Clear();
            while (i-- > 0)
            {
                string s = br.ReadString();
                float n1 = br.ReadSingle();
                float n2 = br.ReadSingle();
                this.RealLocals.Add(s, new Tuple<float, float>(n1, n2));
            }

            i = br.ReadInt32();
            this.StringLocals.Clear();
            while (i-- > 0)
            {
                string s = br.ReadString();
                string n1 = br.ReadString();
                string n2 = br.ReadString();
                this.StringLocals.Add(s, new Tuple<string, string>(n1, n2));
            }

            i = br.ReadInt32();
            this.BoolLocals.Clear();
            while (i-- > 0)
            {
                string s = br.ReadString();
                bool n1 = br.ReadBoolean();
                bool n2 = br.ReadBoolean();
                this.BoolLocals.Add(s, new Tuple<bool, bool>(n1, n2));
            }

            i = br.ReadInt32();
            this.ItemsLinked.Clear();
            while (i-- > 0)
            {
                string s = br.ReadString();
                Guid id = new Guid(br.ReadBytes(16));
                this.ItemsLinked.Add(s, id);
            }

            i = br.ReadInt32();
            this.SpellsLinked.Clear();
            while (i-- > 0)
            {
                string s = br.ReadString();
                Guid id = new Guid(br.ReadBytes(16));
                this.SpellsLinked.Add(s, id);
            }

            i = br.ReadInt32();
            this.Actions.Clear();
            while (i-- > 0)
            {
                this.Actions.AddLast(MacroSerializer.ReadMacroAction(br));
            }
        }
        #endregion
    }

    public enum LocalType
    {
        Integer,
        Real,
        Boolean,
        String
    }
}
