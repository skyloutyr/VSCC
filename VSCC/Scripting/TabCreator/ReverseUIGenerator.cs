using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VSCC.Scripting.TabCreator.Defs;
using Xceed.Wpf.Toolkit;

namespace VSCC.Scripting.TabCreator
{
    public class ReverseUIGenerator
    {
        public static Dictionary<Type, UIType> Type2DefTypeTable { get; } = new Dictionary<Type, UIType>()
        {
            [typeof(Grid)] = UIType.Grid,
            [typeof(WrapPanel)] = UIType.WrapPanel,
            [typeof(StackPanel)] = UIType.StackPanel,
            [typeof(GroupBox)] = UIType.GroupBox,
            [typeof(Border)] = UIType.Border,
            [typeof(Label)] = UIType.Label,
            [typeof(Button)] = UIType.Button,
            [typeof(TextBox)] = UIType.TextBox,
            [typeof(Image)] = UIType.Image,
            [typeof(CheckBox)] = UIType.CheckBox,
            [typeof(RadioButton)] = UIType.RadioButton,
            [typeof(ScrollViewer)] = UIType.ScrollViewer,
            [typeof(IntegerUpDown)] = UIType.IntUpDown,
            [typeof(SingleUpDown)] = UIType.FloatUpDown
        };

        public string CreateJsonFromDef(UIDefinition def) => JsonConvert.SerializeObject(def, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

        public static UIDefinition CreateDefFromUI(UIElement element, ref int id)
        {
            UIDefinition ret = new UIDefinition
            {
                Name = "GeneratedUI_" + id++.ToString(),
                Type = Type2DefTypeTable[element.GetType()]
            };
            if (element is FrameworkElement felem)
            {
                ret.Name = !string.IsNullOrEmpty(felem.Name) ? felem.Name : ret.Name;
                int tb = felem.VerticalAlignment == VerticalAlignment.Top ? 0b1000 : felem.VerticalAlignment == VerticalAlignment.Center ? 0b0100 : felem.VerticalAlignment == VerticalAlignment.Bottom ? 0b0010 : 1;
                int lr = felem.HorizontalAlignment == HorizontalAlignment.Left ? 0b1000 : felem.HorizontalAlignment == HorizontalAlignment.Center ? 0b0100 : felem.HorizontalAlignment == HorizontalAlignment.Right ? 0b0010 : 1;
                ret.Alignment = (Alignment)((tb << 4) | lr);
                ret.Size = new Size() { Width = (int)felem.Width, Height = (int)felem.Height };
                if (felem.Margin != null)
                {
                    ret.Margin = new Margin() { Bottom = (int)felem.Margin.Bottom, Left = (int)felem.Margin.Left, Right = (int)felem.Margin.Right, Top = (int)felem.Margin.Top };
                }

                if (felem is Panel p)
                {
                    ret.Background = ((SolidColorBrush)p.Background).Color.ToString();
                }

                if (felem is Control c)
                {
                    ret.Background = ((SolidColorBrush)c.Background).Color.ToString();
                    ret.Foreground = ((SolidColorBrush)c.Foreground).Color.ToString();
                }

                if (felem.Parent is Grid)
                {
                    ret.RowColumnPositions = new RowColumnPositions() { Column = Grid.GetColumn(felem), Row = Grid.GetRow(felem) };
                }
            }

            if (element is Grid grid)
            {
                ret.GridData = new GridDefinition()
                {
                    ShowGridLines = grid.ShowGridLines,
                    Columns = grid.ColumnDefinitions.Select(cd => new GridColumnDefinition() { Width = (int)cd.Width.Value, UnitType = cd.Width.GridUnitType }).ToArray(),
                    Rows = grid.RowDefinitions.Select(cd => new GridRowDefinition() { Height = (int)cd.Height.Value, UnitType = cd.Height.GridUnitType }).ToArray()
                };
            }

            if (element is StackPanel spanel)
            {
                ret.CommonPanelData = new PanelDefinition() { Orientation = spanel.Orientation };
            }

            if (element is WrapPanel wpanel)
            {
                ret.CommonPanelData = new PanelDefinition() { Orientation = wpanel.Orientation };
            }

            if (element is GroupBox groupbox)
            {
                ret.GroupBoxData = new GroupBoxDefinition() { Header = groupbox.Header.ToString(), Font = new FontDefinition() { Size = (int)groupbox.FontSize, Style = groupbox.FontStyle } };
            }

            if (element is Border border)
            {
                ret.BorderData = new BorderDefinition() { BorderColor = ((SolidColorBrush)border.BorderBrush).Color.ToString(), BorderThickness = new Margin() { Bottom = (int)border.BorderThickness.Bottom, Left = (int)border.BorderThickness.Left, Right = (int)border.BorderThickness.Right, Top = (int)border.BorderThickness.Top } };
            }

            if (element is Label label)
            {
                ret.LabelData = new LabelDefinition() { Text = label.Content.ToString(), Font = new FontDefinition() { Size = (int)label.FontSize, Style = label.FontStyle } };
            }

            if (element is Button button)
            {
                ret.ButtonData = new ButtonDefinition() { Enabled = button.IsEnabled, Text = button.Content is string ? button.Content.ToString() : string.Empty, Font = new FontDefinition() { Size = (int)button.FontSize, Style = button.FontStyle } };
            }

            if (element is TextBox textbox)
            {
                ret.TextBoxData = new TextBoxDefinition() { HorizontalScrollBarVisibility = textbox.HorizontalScrollBarVisibility, VerticalScrollBarVisibility = textbox.VerticalScrollBarVisibility, IsReadOnly = textbox.IsReadOnly, WrapMode = textbox.TextWrapping, Text = textbox.Text, Font = new FontDefinition() { Size = (int)textbox.FontSize, Style = textbox.FontStyle }, Border = textbox.BorderBrush != null ? new BorderDefinition() { BorderColor = ((SolidColorBrush)textbox.BorderBrush).Color.ToString(), BorderThickness = new Margin() { Bottom = (int)textbox.BorderThickness.Bottom, Left = (int)textbox.BorderThickness.Left, Right = (int)textbox.BorderThickness.Right, Top = (int)textbox.BorderThickness.Top } } : null };
            }

            if (element is Image image)
            {
                ret.ImageData = new ImageDefinition() { StretchMode = image.Stretch, Source = "Insert your image source here" };
            }

            if (element is CheckBox checkbox)
            {
                ret.CheckBoxData = new CheckBoxDefinition() { IsChecked = checkbox.IsChecked ?? false };
            }

            if (element is RadioButton radiobutton)
            {
                ret.RadioButtonData = new RadioButtonDefinition() { IsChecked = radiobutton.IsChecked ?? false, Group = radiobutton.GroupName };
            }

            if (element is ScrollViewer scrollviewer)
            {
                ret.ScrollViewerData = new ScrollViewerDefinition() { HorizontalScrollBarVisibility = scrollviewer.HorizontalScrollBarVisibility, VerticalScrollBarVisibility = scrollviewer.VerticalScrollBarVisibility, PanningMode = scrollviewer.PanningMode };
            }

            if (element is IntegerUpDown integerupdown)
            {
                ret.NumericUpDownData = new NumericUpDownDefinition() { Value = integerupdown.Value ?? 0 };
            }

            if (element is SingleUpDown singleupdown)
            {
                ret.NumericUpDownData = new NumericUpDownDefinition() { Value = singleupdown.Value ?? 0 };
            }

            if (element is Viewbox viewbox)
            {
                ret.Children = new List<UIDefinition>() { [0] = CreateDefFromUI(viewbox.Child, ref id) };
            }
            else
            {
                if (element is Panel panel)
                {
                    List<UIDefinition> childrenDefs = new List<UIDefinition>();
                    foreach (UIElement child in panel.Children)
                    {
                        childrenDefs.Add(CreateDefFromUI(child, ref id));
                    }

                    ret.Children = childrenDefs;
                }
                else
                {
                    if (element is ContentControl control)
                    {
                        if (control.Content is UIElement contentelement)
                        {
                            ret.Children = new List<UIDefinition>() { [0] = CreateDefFromUI(contentelement, ref id) };
                        }
                    }
                }
            }

            return ret;
        }
    }
}
