using Newtonsoft.Json;
using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VSCC.Scripting.TabCreator.Defs;
using Xceed.Wpf.Toolkit;

namespace VSCC.Scripting.TabCreator
{
    public class UIGenerator
    {
        public static Dictionary<UIType, Func<UIDefinition, UIElement>> Generators { get; } = new Dictionary<UIType, Func<UIDefinition, UIElement>>()
        {
            [UIType.Grid] = def =>
            {
                Grid g = new Grid();
                SetBasicData(g, def);
                if (def.GridData != null)
                {
                    foreach (GridColumnDefinition gcDef in GetSaveEnumerable(def.GridData.Columns))
                    {
                        g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(gcDef.Width, gcDef.UnitType) });
                    }

                    foreach (GridRowDefinition grDef in GetSaveEnumerable(def.GridData.Rows))
                    {
                        g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(grDef.Height, grDef.UnitType) });
                    }

                    g.ShowGridLines = def.GridData.ShowGridLines;
                }

                return g;
            },

            [UIType.WrapPanel] = def =>
            {
                WrapPanel wp = new WrapPanel();
                SetBasicData(wp, def);
                if (def.CommonPanelData != null)
                {
                    wp.Orientation = def.CommonPanelData.Orientation;
                }

                return wp;
            },

            [UIType.StackPanel] = def =>
            {
                StackPanel sp = new StackPanel();
                SetBasicData(sp, def);
                if (def.CommonPanelData != null)
                {
                    sp.Orientation = def.CommonPanelData.Orientation;
                }

                return sp;
            },

            [UIType.GroupBox] = def =>
            {
                GroupBox gb = new GroupBox();
                SetBasicData(gb, def);
                if (def.GroupBoxData != null)
                {
                    gb.Header = def.GroupBoxData.Header;
                    if (def.GroupBoxData.Font != null)
                    {
                        gb.FontSize = def.GroupBoxData.Font.Size;
                        gb.FontStyle = def.GroupBoxData.Font.Style;
                    }
                }

                return gb;
            },

            [UIType.Border] = def =>
            {
                Border b = new Border();
                SetBasicData(b, def);
                if (def.BorderData != null)
                {
                    b.BorderBrush = new SolidColorBrush(ColorFromColorString(def.BorderData.BorderColor));
                    b.BorderThickness = new Thickness(def.BorderData.BorderThickness.Left, def.BorderData.BorderThickness.Top, def.BorderData.BorderThickness.Right, def.BorderData.BorderThickness.Bottom);
                }

                return b;
            },

            [UIType.Label] = def =>
            {
                Label l = new Label();
                SetBasicData(l, def);
                if (def.LabelData != null)
                {
                    l.Content = def.LabelData.Text;
                    if (def.LabelData.Font != null)
                    {
                        l.FontSize = def.LabelData.Font.Size;
                        l.FontStyle = def.LabelData.Font.Style;
                    }
                }

                return l;
            },

            [UIType.Button] = def =>
            {
                Button b = new Button();
                SetBasicData(b, def);
                if (def.ButtonData != null)
                {
                    b.Content = def.ButtonData.Text;
                    b.IsEnabled = def.ButtonData.Enabled;
                    if (def.ButtonData.Font != null)
                    {
                        b.FontSize = def.ButtonData.Font.Size;
                        b.FontStyle = def.ButtonData.Font.Style;
                    }
                }

                return b;
            },

            [UIType.TextBox] = def =>
            {
                TextBox tb = new TextBox();
                SetBasicData(tb, def);
                if (def.TextBoxData != null)
                {
                    tb.Text = def.TextBoxData.Text;
                    tb.IsReadOnly = def.TextBoxData.IsReadOnly;
                    if (def.TextBoxData.Font != null)
                    {
                        tb.FontSize = def.TextBoxData.Font.Size;
                        tb.FontStyle = def.TextBoxData.Font.Style;
                    }

                    tb.VerticalScrollBarVisibility = def.TextBoxData.VerticalScrollBarVisibility;
                    tb.HorizontalScrollBarVisibility = def.TextBoxData.HorizontalScrollBarVisibility;
                    tb.TextWrapping = def.TextBoxData.WrapMode;
                    if (def.TextBoxData.Border != null)
                    {
                        tb.BorderBrush = new SolidColorBrush(ColorFromColorString(def.TextBoxData.Border.BorderColor));
                        tb.BorderThickness = new Thickness() { Top = def.TextBoxData.Border.BorderThickness.Top, Right = def.TextBoxData.Border.BorderThickness.Right, Left = def.TextBoxData.Border.BorderThickness.Left, Bottom = def.TextBoxData.Border.BorderThickness.Bottom };
                    }
                }

                return tb;
            },

            [UIType.Image] = def =>
            {
                Image i = new Image();
                SetBasicData(i, def);
                if (def.ImageData != null)
                {
                    i.Source = BitmapFrame.Create(new Uri(def.ImageData.Source, UriKind.Relative));
                    i.Stretch = def.ImageData.StretchMode;
                }

                return i;
            },

            [UIType.CheckBox] = def =>
            {
                CheckBox cb = new CheckBox();
                SetBasicData(cb, def);
                if (def.CheckBoxData != null)
                {
                    cb.IsChecked = def.CheckBoxData.IsChecked;
                }

                return cb;
            },

            [UIType.RadioButton] = def =>
            {
                RadioButton rb = new RadioButton();
                SetBasicData(rb, def);
                if (def.RadioButtonData != null)
                {
                    rb.IsChecked = def.RadioButtonData.IsChecked;
                    rb.GroupName = def.RadioButtonData.Group;
                }

                return rb;
            },

            [UIType.ScrollViewer] = def =>
            {
                ScrollViewer sv = new ScrollViewer();
                SetBasicData(sv, def);
                if (def.ScrollViewerData != null)
                {
                    sv.VerticalScrollBarVisibility = def.ScrollViewerData.VerticalScrollBarVisibility;
                    sv.HorizontalScrollBarVisibility = def.ScrollViewerData.HorizontalScrollBarVisibility;
                    sv.PanningMode = def.ScrollViewerData.PanningMode;
                }

                return sv;
            },

            [UIType.IntUpDown] = def =>
            {
                IntegerUpDown intUD = new IntegerUpDown();
                SetBasicData(intUD, def);
                if (def.NumericUpDownData != null)
                {
                    intUD.Value = (int)def.NumericUpDownData.Value;
                }

                return intUD;
            },

            [UIType.FloatUpDown] = def =>
            {
                SingleUpDown floatUD = new SingleUpDown();
                SetBasicData(floatUD, def);
                if (def.NumericUpDownData != null)
                {
                    floatUD.Value = (float)def.NumericUpDownData.Value;
                }

                return floatUD;
            },

            [UIType.Viewbox] = def =>
            {
                Viewbox v = new Viewbox();
                SetBasicData(v, def);
                return v;
            },
        };

        public static Dictionary<UIType, Action<UIElement, UIElement[], UIDefinition[]>> ChildrenSetters { get; } = new Dictionary<UIType, Action<UIElement, UIElement[], UIDefinition[]>>()
        {
            [UIType.Grid] = (parent, children, defs) =>
            {
                Grid g = parent as Grid;
                int i = 0;
                foreach (UIElement elem in children)
                {
                    g.Children.Add(elem);
                    UIDefinition elemDef = defs[i++];
                    if (elemDef.RowColumnPositions != null)
                    {
                        Grid.SetRow(elem, elemDef.RowColumnPositions.Row);
                        Grid.SetColumn(elem, elemDef.RowColumnPositions.Column);
                    }
                }
            },

            [UIType.WrapPanel] = (parent, children, defs) =>
            {
                WrapPanel wp = parent as WrapPanel;
                foreach (UIElement elem in children)
                {
                    wp.Children.Add(elem);
                }
            },

            [UIType.StackPanel] = (parent, children, defs) =>
            {
                StackPanel sp = parent as StackPanel;
                foreach (UIElement elem in children)
                {
                    sp.Children.Add(elem);
                }
            },

            [UIType.GroupBox] = AddContext,
            [UIType.Border] = AddContext,
            [UIType.Label] = (e, c, d) => { },
            [UIType.Button] = AddContext,
            [UIType.TextBox] = AddContext,
            [UIType.Image] = AddContext,
            [UIType.CheckBox] = (e, c, d) => { },
            [UIType.RadioButton] = (e, c, d) => { },
            [UIType.ScrollViewer] = AddContext,
            [UIType.IntUpDown] = AddContext,
            [UIType.FloatUpDown] = AddContext,
            [UIType.Viewbox] = (e, c, d) =>
            {
                if (c.Length > 0)
                {
                    ((Viewbox)e).Child = c[0];
                }
            }
        };

        public static LuaTable FromFile(string path) => FromJSON(File.ReadAllText(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path))));

        public static LuaTable FromJSON(string json)
        {
            UIDefinition def = JsonConvert.DeserializeObject<UIDefinition>(json, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            LuaTable table = ScriptEngine.Instance.Value.Lua.DoString("return {}")[0] as LuaTable;
            table["root"] = ElementFromDef(def, table);
            return table;
        }

        public static UIElement ElementFromDef(UIDefinition def, LuaTable tableRef)
        {
            UIElement ret = Generators[def.Type](def);
            tableRef[def.Name] = ret;
            UIElement[] arr = new UIElement[def.Children.Count];
            UIDefinition[] defArr = def.Children.ToArray();
            int i = 0;
            foreach (UIDefinition child in def.Children)
            {
                UIElement elem = ElementFromDef(child, tableRef);
                arr[i++] = elem;
            }

            ChildrenSetters[def.Type](ret, arr, defArr);
            return ret;
        }

        public static void SetBasicData(UIElement element, UIDefinition define)
        {
            if (element is FrameworkElement felem)
            {
                int vData = ((int)define.Alignment & 0b11110000) >> 4;
                int hData = ((int)define.Alignment & 0b00001111);
                felem.VerticalAlignment = (vData & 0b1000) == 0b1000 ? VerticalAlignment.Top : (vData & 0b0100) == 0b0100 ? VerticalAlignment.Center : (vData & 0b0010) == 0b0010 ? VerticalAlignment.Bottom : VerticalAlignment.Stretch;
                felem.HorizontalAlignment = (hData & 0b1000) == 0b1000 ? HorizontalAlignment.Left : (hData & 0b0100) == 0b0100 ? HorizontalAlignment.Center : (hData & 0b0010) == 0b0010 ? HorizontalAlignment.Right : HorizontalAlignment.Stretch;
                if (define.Size != null)
                {
                    felem.Width = define.Size.Width;
                    felem.Height = define.Size.Height;
                }

                if (define.Margin != null)
                {
                    felem.Margin = new Thickness(define.Margin.Left, define.Margin.Top, define.Margin.Right, define.Margin.Bottom);
                }

                if (!string.IsNullOrEmpty(define.Background))
                {
                    if (element is Panel panel)
                    {
                        if (!string.IsNullOrEmpty(define.Background))
                        {
                            panel.Background = new SolidColorBrush(ColorFromColorString(define.Background));
                        }
                    }
                    else
                    {
                        if (element is Control control)
                        {
                            if (!string.IsNullOrEmpty(define.Background))
                            {
                                control.Background = new SolidColorBrush(ColorFromColorString(define.Background));
                                if (!string.IsNullOrEmpty(define.Foreground))
                                {
                                    control.Foreground = new SolidColorBrush(ColorFromColorString(define.Foreground));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<T> GetSaveEnumerable<T>(IEnumerable<T> baseEnumerable)
        {
            if (baseEnumerable == null)
            {
                yield break;
            }

            foreach (T t in baseEnumerable)
            {
                yield return t;
            }

            yield break;
        }

        private static Color ColorFromColorString(string color)
        {
            if (color[0] == '#')
            {
                color = color.Substring(1);
                uint clr = uint.Parse(color.Substring(1), System.Globalization.NumberStyles.HexNumber);
                byte a = (byte)((clr & 0xFF000000) >> 24);
                byte r = (byte)((clr & 0xFF0000) >> 16);
                byte g = (byte)((clr & 0xFF00) >> 8);
                byte b = (byte)(clr & 0xFF);
                return Color.FromArgb(a, r, g, b);
            }

            if (color.StartsWith("0x"))
            {
                uint clr = Convert.ToUInt32(color, 16);
                byte a = (byte)((clr & 0xFF000000) >> 24);
                byte r = (byte)((clr & 0xFF0000) >> 16);
                byte g = (byte)((clr & 0xFF00) >> 8);
                byte b = (byte)(clr & 0xFF);
                return Color.FromArgb(a, r, g, b);
            }

            return (Color)ColorConverter.ConvertFromString(color);
        }

        private static void AddContext(UIElement element, UIElement[] children, UIDefinition[] defines)
        {
            if (element is ContentControl control && children.Length > 0)
            {
                control.Content = children[0];
            }
        }
    }
}
