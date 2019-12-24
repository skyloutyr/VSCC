using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VSCC.Controls.Windows
{
    /// <summary>
    /// Interaction logic for ScriptsWindow.xaml
    /// </summary>
    public partial class ScriptsWindow : Window
    {
        public ScriptsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (KeyValuePair<string, EventHandler> kv in Scripting.ScriptEngine.Instance.Value.Scripts)
            {
                Button b = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 32,
                    Width = 200,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = kv.Key,
                    Margin = new Thickness(3, 3, 3, 3)
                };

                b.Click += (o, ev) => kv.Value?.Invoke(o, ev);
                this.WrapPanel_Main.Children.Add(b);
            }
        }
    }
}
