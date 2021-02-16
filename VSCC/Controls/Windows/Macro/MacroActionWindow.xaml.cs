namespace VSCC.Controls.Windows.Macro
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using VSCC.Roll20.Macros;
    using VSCC.Roll20.Macros.Basic;

    /// <summary>
    /// Interaction logic for MacroActionWindow.xaml
    /// </summary>
    public partial class MacroActionWindow : Window
    {
        public MacroAction Action { get; set; }
        public Type TypeFilter { get; set; }

        private bool _pauseRecalc;

        public MacroActionWindow(MacroAction ma, Type type)
        {
            this._pauseRecalc = true;
            this.InitializeComponent();
            this.Action = ma;
            this.TypeFilter = type;
            if (type != null)
            {
                this.CBCategories.Items.Add(new ComboBoxItem() { Content = "All", Tag = new object() });
                foreach (string cat in MacroAction.Actions[type].Where(a => a.Item4).Select(tu => tu.Item3).Distinct())
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = cat
                    };

                    this.CBCategories.Items.Add(cbi);
                }

                if (ma != null)
                {
                    ComboBoxItem cbi = this.CBCategories.Items.Cast<ComboBoxItem>().First(cb => cb.Content.Equals(ma.Category));
                    this.CBCategories.SelectedIndex = this.CBCategories.Items.IndexOf(cbi);
                    this.CBCategories.SelectedItem = cbi;
                    this.CBCategories.Text = (string)cbi.Content;
                }
                else
                {
                    ComboBoxItem cbi = (ComboBoxItem)this.CBCategories.Items[0];
                    this.CBCategories.SelectedIndex = 0;
                    this.CBCategories.SelectedItem = cbi;
                    this.CBCategories.Text = (string)cbi.Content;
                }

                List<ComboBoxItem> t = new List<ComboBoxItem>();
                string cate = this.CBCategories.Text;
                foreach (Tuple<Type, string, string, bool> mat in MacroAction.Actions[this.TypeFilter])
                {
                    if (!mat.Item4)
                    {
                        continue;
                    }

                    if (cate.Equals("All") || mat.Item3.Equals(cate))
                    {
                        ComboBoxItem cbi = new ComboBoxItem
                        {
                            Content = mat.Item2,
                            Tag = mat.Item1
                        };

                        t.Add(cbi);
                    }
                }

                t.Sort((l, r) => StringComparer.OrdinalIgnoreCase.Compare(l.Content, r.Content));
                foreach (ComboBoxItem cbi in t)
                {
                    this.CBAllPossibleActions.Items.Add(cbi);
                }
            }

            if (ma != null)
            {
                foreach (ComboBoxItem cbi in this.CBAllPossibleActions.Items)
                {
                    if (cbi.Tag.Equals(ma.GetType()))
                    {
                        this.CBAllPossibleActions.SelectedItem = cbi;
                        this.CBAllPossibleActions.SelectedIndex = this.CBAllPossibleActions.Items.IndexOf(cbi);
                        this.CBAllPossibleActions.Text = (string)cbi.Content;
                        break;
                    }
                }

                this.SetWindowView(ma);
            }

            this.CBAllPossibleActions.InvalidateVisual();
            this._pauseRecalc = false;
        }

        private void SetWindowView(MacroAction ma)
        {
            this.CustomContent.Children.Clear();
            if (ma is MacroActionStringConstant masc)
            {
                this.TBMAInteractable.Visibility = Visibility.Hidden;
                this.IUDAConstantInt.Visibility = Visibility.Hidden;
                this.IUDAConstantFloat.Visibility = Visibility.Hidden;
                this.CBConstantBool.Visibility = Visibility.Hidden;
                this.CustomContent.Visibility = Visibility.Hidden;
                this.TBMAConstantString.Visibility = Visibility.Visible;
                this.TBMAConstantString.Text = masc.GetValue();
            }
            else
            {
                if (ma is MacroActionNumberConstant manc)
                {

                    this.TBMAInteractable.Visibility = Visibility.Hidden;
                    this.IUDAConstantInt.Visibility = Visibility.Visible;
                    this.IUDAConstantFloat.Visibility = Visibility.Hidden;
                    this.CBConstantBool.Visibility = Visibility.Hidden;
                    this.TBMAConstantString.Visibility = Visibility.Hidden;
                    this.CustomContent.Visibility = Visibility.Hidden;
                    this.IUDAConstantInt.Value = manc.GetValue();
                }
                else
                {
                    if (ma is MacroActionRealConstant marc)
                    {
                        this.TBMAInteractable.Visibility = Visibility.Hidden;
                        this.IUDAConstantInt.Visibility = Visibility.Hidden;
                        this.IUDAConstantFloat.Visibility = Visibility.Visible;
                        this.CBConstantBool.Visibility = Visibility.Hidden;
                        this.TBMAConstantString.Visibility = Visibility.Hidden;
                        this.CustomContent.Visibility = Visibility.Hidden;
                        this.IUDAConstantFloat.Value = marc.GetValue();
                    }
                    else
                    {
                        if (ma is MacroActionBoolConstant mabc)
                        {
                            this.TBMAInteractable.Visibility = Visibility.Hidden;
                            this.IUDAConstantInt.Visibility = Visibility.Hidden;
                            this.IUDAConstantFloat.Visibility = Visibility.Hidden;
                            this.CBConstantBool.Visibility = Visibility.Visible;
                            this.TBMAConstantString.Visibility = Visibility.Hidden;
                            this.CustomContent.Visibility = Visibility.Hidden;
                            this.CBConstantBool.IsChecked = mabc.GetValue();
                        }
                        else
                        {
                            if (ma.CreateCustomView(this.CustomContent))
                            {
                                this.TBMAInteractable.Visibility = Visibility.Hidden;
                                this.IUDAConstantInt.Visibility = Visibility.Hidden;
                                this.IUDAConstantFloat.Visibility = Visibility.Hidden;
                                this.CBConstantBool.Visibility = Visibility.Hidden;
                                this.TBMAConstantString.Visibility = Visibility.Hidden;
                                this.CustomContent.Visibility = Visibility.Visible;
                                foreach (Hyperlink hl in FindLogicalChildren<Hyperlink>(this.CustomContent).ToList())
                                {
                                    if (hl.Tag != null)
                                    {
                                        hl.Click += this.HandleHyperlinkClicked;
                                        Run r = (Run)hl.Inlines.FirstInline;
                                        r.Text = ma.CreateFormattedText()[(int)hl.Tag];
                                    }
                                }
                            }
                            else
                            {
                                this.TBMAInteractable.Visibility = Visibility.Visible;
                                this.IUDAConstantInt.Visibility = Visibility.Hidden;
                                this.IUDAConstantFloat.Visibility = Visibility.Hidden;
                                this.CBConstantBool.Visibility = Visibility.Hidden;
                                this.TBMAConstantString.Visibility = Visibility.Hidden;
                                this.CustomContent.Visibility = Visibility.Hidden;
                                foreach (Inline te in ma.CreateInnerText())
                                {
                                    this.TBMAInteractable.Inlines.Add(te);
                                    if (te is Hyperlink hl)
                                    {
                                        hl.Click += this.HandleHyperlinkClicked;
                                        Run r = (Run)hl.Inlines.FirstInline;
                                        r.Text = ma.CreateFormattedText()[(int)hl.Tag];
                                    }
                                }

                                this.TBMAInteractable.InvalidateVisual();
                            }
                        }
                    }
                }
            }
        }

        private void CBCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._pauseRecalc)
            {
                this._pauseRecalc = true;
                string cat = e == null ? this.CBCategories.Text : (string)((ComboBoxItem)e.AddedItems[0]).Content;
                this.CBAllPossibleActions.Items.Clear();
                List<ComboBoxItem> t = new List<ComboBoxItem>();
                foreach (Tuple<Type, string, string, bool> mat in MacroAction.Actions[this.TypeFilter])
                {
                    if (!mat.Item4)
                    {
                        continue;
                    }

                    if (cat.Equals("All") || mat.Item3.Equals(cat))
                    {
                        ComboBoxItem cbi = new ComboBoxItem
                        {
                            Content = mat.Item2,
                            Tag = mat.Item1
                        };

                        t.Add(cbi);
                    }
                }

                t.Sort((l, r) => StringComparer.OrdinalIgnoreCase.Compare(l.Content, r.Content));
                foreach (ComboBoxItem cbi in t)
                {
                    this.CBAllPossibleActions.Items.Add(cbi);
                }

                this.CBAllPossibleActions.InvalidateVisual();
                this._pauseRecalc = false;
            }
        }

        private void CBAllPossibleActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._pauseRecalc)
            {
                ComboBoxItem cbi = (ComboBoxItem)this.CBAllPossibleActions.SelectedItem;
                if (cbi != null)
                {
                    Type t = (Type)cbi.Tag;
                    MacroAction ma = (MacroAction)Activator.CreateInstance(t);
                    ma.SetDefaults();
                    this.Action = ma;
                    this.TBMAInteractable.Inlines.Clear();
                    this.SetWindowView(ma);
                }
            }
        }

        private void HandleHyperlinkClicked(object sender, RoutedEventArgs args)
        {
            Hyperlink hl = (Hyperlink)sender;
            int index = (int)hl.Tag;
            MacroActionWindow maw = new MacroActionWindow(this.Action.Params[index], this.Action.ParamTypes[index]);
            if (maw.ShowDialog() ?? false)
            {
                this.Action.Params[index] = maw.Action;
                Run r = (Run)hl.Inlines.FirstInline;
                r.Text = this.Action.CreateFormattedText()[(int)hl.Tag];
            }
        }

        // Cancel
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (this.Action is MacroActionStringConstant masc)
            {
                masc.SetValue(this.TBMAConstantString.Text);
            }

            if (this.Action is MacroActionNumberConstant manc)
            {
                manc.SetValue(this.IUDAConstantInt.Value ?? 0);
            }

            if (this.Action is MacroActionRealConstant marc)
            {
                marc.SetValue(this.IUDAConstantFloat.Value ?? 0);
            }

            if (this.Action is MacroActionBoolConstant mabc)
            {
                mabc.SetValue(this.CBConstantBool.IsChecked ?? false);
            }

            this.Close();
        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object child in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    if (child is DependencyObject dob)
                    {
                        foreach (T childOfChild in FindLogicalChildren<T>(dob))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }
    }
}
