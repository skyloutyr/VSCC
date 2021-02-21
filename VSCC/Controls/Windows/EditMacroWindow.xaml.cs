namespace VSCC.Controls.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using VSCC.Controls.Windows.Macro;
    using VSCC.Roll20.Macros;
    using VSCC.Roll20.Macros.Actions;

    /// <summary>
    /// Interaction logic for EditMacroWindow.xaml
    /// </summary>
    public partial class EditMacroWindow : Window
    {
        public Roll20.Macros.Macro EditedMacro { get; }
        public ContextMenu ContextMenu_Links { get; }

        public EditMacroWindow()
        {
            this.InitializeComponent();
            this.ContextMenu_Links = new ContextMenu();
            this.ContextMenu_Links.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_DeleteLink") });
        }

        public EditMacroWindow(Roll20.Macros.Macro macro) : this() => this.EditedMacro = macro;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.EditedMacro?.Populate(this, this.TVI_Links, this.TVI_Locals, this.TVI_Actions);
            this.TVI_Actions.Tag = this.EditedMacro?.Actions;
        }

        // New Item Link clicked
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateLinkWindow clw = new CreateLinkWindow { SuggestedName = MainWindow.Translate("Macro_Generic_NewLinkItem"), IsItemLink = true };
            if (clw.ShowDialog() ?? false)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ clw.TB_LinkName.Text }: { clw.CB_Value.Text }" };
                Guid val = Guid.Parse(clw.CB_Value.Text.Split('|')[1]);
                this.AssignMenuToLink(tvi);
                this.TVI_Links.Items.Add(tvi);
                this.EditedMacro.ItemsLinked.Add(clw.TB_LinkName.Text, val);
                tvi.Tag = new Tuple<string, bool, Guid>(clw.TB_LinkName.Text, true, val);
            }
        }

        public void AssignMenuToLink(TreeViewItem tvi)
        {
            ContextMenu cm = new ContextMenu();
            cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Delete") });
            cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Edit") });
            ((MenuItem)cm.Items[0]).Click += this.DeleteLinkMenuClicked;
            ((MenuItem)cm.Items[1]).Click += this.EditLinkClicked;
            tvi.ContextMenu = cm;
        }

        private void EditLinkClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget;
            Tuple<string, bool, Guid> dat = (Tuple<string, bool, Guid>)tvi.Tag;
            CreateLinkWindow clw = new CreateLinkWindow { SuggestedName = dat.Item1, IsItemLink = dat.Item2 };
            clw.TB_LinkName.IsReadOnly = true;
            if (clw.ShowDialog() ?? false)
            {
                tvi.Header = $"{ clw.TB_LinkName.Text }: { clw.CB_Value.Text }";
                Guid val = Guid.Parse(clw.CB_Value.Text.Split('|')[1]);
                if (dat.Item2)
                {
                    this.EditedMacro.ItemsLinked[clw.TB_LinkName.Text] = val;
                }
                else
                {
                    this.EditedMacro.SpellsLinked[clw.TB_LinkName.Text] = val;
                }

                tvi.Tag = new Tuple<string, bool, Guid>(clw.TB_LinkName.Text, dat.Item2, val);
            }
        }

        // New Spell Link clicked
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            CreateLinkWindow clw = new CreateLinkWindow { SuggestedName = MainWindow.Translate("Macro_Generic_NewLinkSpell"), IsItemLink = false };
            if (clw.ShowDialog() ?? false)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ clw.TB_LinkName.Text }: { clw.CB_Value.Text }" };
                Guid val = Guid.Parse(clw.CB_Value.Text.Split('|')[1]);
                ContextMenu cm = new ContextMenu();
                cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Delete") });
                cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Edit") });
                ((MenuItem)cm.Items[0]).Click += this.DeleteLinkMenuClicked;
                ((MenuItem)cm.Items[1]).Click += this.EditLinkClicked;
                tvi.ContextMenu = cm;
                this.TVI_Links.Items.Add(tvi);
                this.EditedMacro.SpellsLinked.Add(clw.TB_LinkName.Text, val);
                tvi.Tag = new Tuple<string, bool, Guid>(clw.TB_LinkName.Text, false, val);
            }
        }

        private void DeleteLinkMenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget;
            Tuple<string, bool, Guid> dat = (Tuple<string, bool, Guid>)tvi.Tag;
            if (dat.Item2)
            {
                this.EditedMacro.ItemsLinked.Remove(dat.Item1);
            }
            else
            {
                this.EditedMacro.SpellsLinked.Remove(dat.Item1);
            }

            this.TVI_Links.Items.Remove(tvi);
            this.TVI_Links.InvalidateVisual();
        }

        private void DeleteActionMenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget;
            MacroAction ma = (MacroAction)tvi.Tag;
            if (tvi.Parent is TreeViewItem ptvi && ptvi.Parent is TreeViewItem) // Nested
            {
                ((LinkedList<MacroAction>)ptvi.Tag).Remove(ma);
                ptvi.Items.Remove(tvi);
            }
            else
            {
                this.EditedMacro.Actions.Remove(ma);
                this.TVI_Actions.Items.Remove(tvi);
            }

            this.TVI_Actions.InvalidateVisual();
        }

        private void DeleteLocalMenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget;
            Tuple<string, LocalType> dat = (Tuple<string, LocalType>)tvi.Tag;
            switch (dat.Item2)
            {
                case LocalType.Integer:
                {
                    this.EditedMacro.NumberLocals.Remove(dat.Item1);
                    break;
                }

                case LocalType.Real:
                {
                    this.EditedMacro.RealLocals.Remove(dat.Item1);
                    break;
                }

                case LocalType.Boolean:
                {
                    this.EditedMacro.BoolLocals.Remove(dat.Item1);
                    break;
                }

                case LocalType.String:
                {
                    this.EditedMacro.StringLocals.Remove(dat.Item1);
                    break;
                }
            }

            this.TVI_Locals.Items.Remove(tvi);
            this.TVI_Locals.InvalidateVisual();
        }

        // Create Local clicked
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            DefineLocalWindow dlw = new DefineLocalWindow { EditedMacro = this.EditedMacro };
            if (dlw.ShowDialog() ?? false)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = $"{ dlw.CB_Type.Text } { dlw.TB_Name.Text } = { dlw.TB_Value.Text }" };
                this.AssignMenuToLocal(tvi);
                LocalType lt = (LocalType)dlw.CB_Type.SelectedIndex;
                string lName = dlw.TB_Name.Text;
                this.TVI_Locals.Items.Add(tvi);
                switch (lt)
                {
                    case LocalType.Integer:
                    {
                        int i = int.Parse(dlw.TB_Value.Text);
                        this.EditedMacro.NumberLocals.Add(lName, new Tuple<int, int>(i, i));
                        break;
                    }

                    case LocalType.Real:
                    {
                        float f = float.Parse(dlw.TB_Value.Text);
                        this.EditedMacro.RealLocals.Add(lName, new Tuple<float, float>(f, f));
                        break;
                    }

                    case LocalType.Boolean:
                    {
                        bool b = bool.Parse(dlw.TB_Value.Text);
                        this.EditedMacro.BoolLocals.Add(lName, new Tuple<bool, bool>(b, b));
                        break;
                    }

                    case LocalType.String:
                    {
                        this.EditedMacro.StringLocals.Add(lName, new Tuple<string, string>(dlw.TB_Value.Text, dlw.TB_Value.Text));
                        break;
                    }
                }

                tvi.Tag = new Tuple<string, LocalType>(lName, lt);
            }
        }

        public void AssignMenuToLocal(TreeViewItem tvi)
        {
            ContextMenu cm = new ContextMenu();
            cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Delete") });
            cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Edit") });
            ((MenuItem)cm.Items[0]).Click += this.DeleteLocalMenuClicked;
            ((MenuItem)cm.Items[1]).Click += this.EditLocalClicked;
            tvi.ContextMenu = cm;
        }

        private void EditLocalClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget;
            Tuple<string, LocalType> dat = (Tuple<string, LocalType>)tvi.Tag;
            LocalType startingT = dat.Item2;
            string lName = dat.Item1;
            DefineLocalWindow dlw = new DefineLocalWindow { EditedMacro = this.EditedMacro };
            dlw.TB_Name.Text = lName;
            dlw.TB_Name.IsReadOnly = true;
            if (dlw.ShowDialog() ?? false)
            {
                LocalType lt = (LocalType)dlw.CB_Type.SelectedIndex;
                if (lt != startingT)
                {
                    switch (startingT)
                    {
                        case LocalType.Integer:
                        {
                            this.EditedMacro.NumberLocals.Remove(lName);
                            break;
                        }

                        case LocalType.Real:
                        {
                            this.EditedMacro.RealLocals.Remove(lName);
                            break;
                        }

                        case LocalType.Boolean:
                        {
                            this.EditedMacro.BoolLocals.Remove(lName);
                            break;
                        }

                        case LocalType.String:
                        {
                            this.EditedMacro.StringLocals.Remove(lName);
                            break;
                        }
                    }
                }

                switch (lt)
                {
                    case LocalType.Integer:
                    {
                        int i = int.Parse(dlw.TB_Value.Text);
                        this.EditedMacro.NumberLocals[lName] = new Tuple<int, int>(i, i);
                        break;
                    }

                    case LocalType.Real:
                    {
                        float f = float.Parse(dlw.TB_Value.Text);
                        this.EditedMacro.RealLocals[lName] = new Tuple<float, float>(f, f);
                        break;
                    }

                    case LocalType.Boolean:
                    {
                        bool b = bool.Parse(dlw.TB_Value.Text);
                        this.EditedMacro.BoolLocals[lName] = new Tuple<bool, bool>(b, b);
                        break;
                    }

                    case LocalType.String:
                    {
                        this.EditedMacro.StringLocals[lName] = new Tuple<string, string>(dlw.TB_Value.Text, dlw.TB_Value.Text);
                        break;
                    }
                }

                tvi.Header = $"{ dlw.CB_Type.Text } { dlw.TB_Name.Text } = { dlw.TB_Value.Text }";
                tvi.Tag = new Tuple<string, LocalType>(lName, lt);
            }
        }

        // Create Action clicked
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem placementTarget = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget; // Placement target

            MacroActionWindow maw = new MacroActionWindow(null, typeof(void));
            if (maw.ShowDialog() ?? false)
            {
                TreeViewItem tvi = new TreeViewItem() { Header = maw.Action.CreateFullInnerText(), Tag = maw.Action };
                this.AssignGenericActionMenu(tvi);

                if (placementTarget.Tag == null) // Placing at ROOT
                {
                    this.EditedMacro.Actions.AddLast(maw.Action);
                    this.TVI_Actions.Items.Add(tvi);
                }
                else // Placing inside THEN/ELSE block
                {
                    ((LinkedList<MacroAction>)placementTarget.Tag).AddLast(maw.Action);
                    placementTarget.Items.Add(tvi);
                }

                this.TVI_Actions.InvalidateVisual();
            }
        }

        public void AssignGenericActionMenu(TreeViewItem tvi)
        {
            if (!tvi.AllowDrop)
            {
                ContextMenu cm = new ContextMenu();
                cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Delete") });
                cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Edit") });
                ((MenuItem)cm.Items[0]).Click += this.DeleteActionMenuClicked;
                ((MenuItem)cm.Items[1]).Click += this.EditMacroActionClick;
                tvi.ContextMenu = cm;
                tvi.MouseDown += this.Tvi_MouseDown;
                tvi.PreviewMouseMove += this.Tvi_PreviewMouseMove;
                tvi.PreviewDrop += this.Tvi_PreviewDrop;
                tvi.PreviewDragOver += this.Tvi_PreviewDragOver;
                tvi.Drop += this.Tvi_Drop;
                tvi.DragLeave += this.Tvi_DragLeave;
                tvi.AllowDrop = true;
            }
        }

        private void Tvi_DragLeave(object sender, DragEventArgs e) => this.Line_InsertionLineAbs.Visibility = Visibility.Hidden;

        private Point startPoint;

        private void Tvi_MouseDown(object sender, MouseButtonEventArgs e) => this.startPoint = Mouse.GetPosition(Application.Current.MainWindow);
        private void Tvi_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = Mouse.GetPosition(Application.Current.MainWindow);
            Vector diff = this.startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed && ((Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                TreeViewItem tvi = (TreeViewItem)sender;
                TreeViewItem parent = (TreeViewItem)tvi.Parent;
                LinkedList<MacroAction> container = (LinkedList<MacroAction>)parent.Tag;
                MacroAction cA = (MacroAction)tvi.Tag;
                Tuple<LinkedListNode<MacroAction>, LinkedList<MacroAction>, TreeViewItem, TreeViewItem> data = new Tuple<LinkedListNode<MacroAction>, LinkedList<MacroAction>, TreeViewItem, TreeViewItem>(container.Find(cA), container, tvi, parent);
                DataObject dragDrop = new DataObject("MacroAction", data);
                DragDrop.DoDragDrop(tvi, dragDrop, DragDropEffects.Copy);
            }
        }

        private void Tvi_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("MacroAction"))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private bool _insertAfter;
        private void Tvi_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("MacroAction"))
            {
                TreeViewItem to = (TreeViewItem)sender;
                Point pos = to.TransformToAncestor(this).Transform(new Point(0, 0));
                Point cMousePos = e.GetPosition(this);
                double h = to.ActualHeight;
                double half = pos.Y + (h / 2);
                this.Line_InsertionLineAbs.Visibility = Visibility.Visible;
                if (cMousePos.Y > half)
                {
                    this.Line_InsertionLineAbs.Y1 = this.Line_InsertionLineAbs.Y2 = pos.Y + h;
                    this._insertAfter = true;
                }
                else
                {
                    this.Line_InsertionLineAbs.Y1 = this.Line_InsertionLineAbs.Y2 = pos.Y;
                    this._insertAfter = false;
                }

                this.Line_InsertionLineAbs.X1 = pos.X;
                double end = this.TV_Content.TransformToAncestor(this).Transform(new Point(0, 0)).X + this.TV_Content.ActualWidth;
                this.Line_InsertionLineAbs.X2 = end - 16;
            }
        }

        private void Tvi_Drop(object sender, DragEventArgs e)
        {
            TreeViewItem to = (TreeViewItem)sender;
            if (e.Data != null && e.Data.GetDataPresent("MacroAction"))
            {
                Tuple<LinkedListNode<MacroAction>, LinkedList<MacroAction>, TreeViewItem, TreeViewItem> data = (Tuple<LinkedListNode<MacroAction>, LinkedList<MacroAction>, TreeViewItem, TreeViewItem>)e.Data.GetData("MacroAction");
                if (to.Tag != data.Item1.Value)
                {
                    TreeViewItem oldParent = data.Item4;
                    TreeViewItem moved = data.Item3;
                    data.Item2.Remove(data.Item1);
                    oldParent.Items.Remove(moved);
                    if (to.Tag is LinkedList<MacroAction> llist)
                    {
                        llist.AddLast(data.Item1.Value);
                        to.Items.Add(moved);
                    }
                    else
                    {
                        TreeViewItem newParent = (TreeViewItem)to.Parent;
                        LinkedList<MacroAction> insertTo = (LinkedList<MacroAction>)newParent.Tag;
                        LinkedListNode<MacroAction> nodeOver = insertTo.Find((MacroAction)to.Tag);
                        if (this._insertAfter)
                        {
                            insertTo.AddAfter(nodeOver, data.Item1.Value);
                            newParent.Items.Insert(newParent.Items.IndexOf(to) + 1, moved);
                        }
                        else
                        {
                            insertTo.AddBefore(nodeOver, data.Item1.Value);
                            newParent.Items.Insert(newParent.Items.IndexOf(to), moved);
                        }
                    }

                    this.TVI_Actions.InvalidateVisual();
                    e.Handled = true;
                }
            }

            this.Line_InsertionLineAbs.Visibility = Visibility.Hidden;
        }

        private void EditMacroActionClick(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget;
            MacroAction ma = (MacroAction)tvi.Tag;
            LinkedList<MacroAction> container = ((TreeViewItem)tvi.Parent).Tag == null ? this.EditedMacro.Actions : (LinkedList<MacroAction>)((TreeViewItem)tvi.Parent).Tag;
            LinkedListNode<MacroAction> node = container.Find(ma);
            byte[] arr = MacroSerializer.WriteMacroAction(ma);
            MacroActionWindow maw = new MacroActionWindow(ma, ma.ReturnType);
            if (maw.ShowDialog() ?? false)
            {
                tvi.Header = maw.Action.CreateFullInnerText();
                tvi.Tag = maw.Action;
                container.AddBefore(node, maw.Action);
                container.Remove(node);
            }
            else
            {
                MacroSerializer.ReadMacroAction(ma, arr);
            }
        }

        // Cancel
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // OK
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void TB_Desc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.EditedMacro != null)
            {
                this.EditedMacro.Description = ((TextBox)sender).Text;
            }
        }

        private void TB_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.EditedMacro != null)
            {
                this.EditedMacro.Name = ((TextBox)sender).Text;
            }
        }

        // Create If... Then... Else...
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem placementTarget = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget; // Placement target

            MacroActionCondition ifthenelse = new MacroActionCondition();
            TreeViewItem tvi = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_Condition"), Tag = ifthenelse };
            this.AssignConditionalExpressionMenu(tvi);
            TreeViewItem conditions = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_If"), Tag = ifthenelse.If };
            TreeViewItem thens = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_Then"), Tag = ifthenelse.Then };
            TreeViewItem elses = new TreeViewItem { Header = MainWindow.Translate("Macro_Generic_Else"), Tag = ifthenelse.Else };
            tvi.Items.Add(conditions);
            tvi.Items.Add(thens);
            tvi.Items.Add(elses);

            // IF conditions
            this.AssignIfConditionsMenu(conditions);

            // Then/Else
            this.AssignRootActionsMenu(thens);
            this.AssignRootActionsMenu(elses);

            if (placementTarget.Tag == null) // Placing at ROOT
            {
                this.EditedMacro.Actions.AddLast(ifthenelse);
                this.TVI_Actions.Items.Add(tvi);
            }
            else // Placing inside THEN/ELSE block
            {
                ((LinkedList<MacroAction>)placementTarget.Tag).AddLast(ifthenelse);
                placementTarget.Items.Add(tvi);
            }

            this.TVI_Actions.InvalidateVisual();
        }

        public void AssignRootActionsMenu(TreeViewItem thens)
        {
            ContextMenu cm = new ContextMenu();
            cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_NewAction") });
            cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_IfThenElse") });
            ((MenuItem)cm.Items[0]).Click += this.MenuItem_Click_3;
            ((MenuItem)cm.Items[1]).Click += this.MenuItem_Click_4;
            thens.ContextMenu = cm;
            thens.AllowDrop = true;
            thens.PreviewDrop += this.Tvi_PreviewDrop;
            thens.Drop += this.Tvi_Drop;
        }

        public void AssignIfConditionsMenu(TreeViewItem conditions)
        {
            ContextMenu cm2 = new ContextMenu();
            cm2.Items.Add(new MenuItem { Header = MainWindow.Translate("Macro_Generic_NewCondition") });
            ((MenuItem)cm2.Items[0]).Click += this.AddConditionMenuClicked;
            conditions.ContextMenu = cm2;
        }

        public void AssignConditionalExpressionMenu(TreeViewItem tvi)
        {
            ContextMenu cm0 = new ContextMenu();
            cm0.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Delete") });
            ((MenuItem)cm0.Items[0]).Click += this.DeleteActionMenuClicked;
            tvi.ContextMenu = cm0;
        }

        private void AddConditionMenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TreeViewItem tvi = (TreeViewItem)((ContextMenu)mi.Parent).PlacementTarget; // If
            LinkedList<MacroAction> mas = (LinkedList<MacroAction>)tvi.Tag;
            MacroActionWindow maw = new MacroActionWindow(null, typeof(bool));
            if (maw.ShowDialog() ?? false)
            {
                mas.AddLast(maw.Action);
                TreeViewItem tvi2 = new TreeViewItem() { Header = maw.Action.CreateFullInnerText(), Tag = maw.Action };
                ContextMenu cm = new ContextMenu();
                cm.Items.Add(new MenuItem() { Header = MainWindow.Translate("Macro_Generic_Delete") });
                ((MenuItem)cm.Items[0]).Click += this.DeleteActionMenuClicked;
                tvi2.ContextMenu = cm;
                tvi.Items.Add(tvi2);
                this.TVI_Actions.InvalidateVisual();
            }
        }
    }
}
