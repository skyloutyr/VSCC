using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSCC.Controls.Windows;
using VSCC.Properties;
using VSCC.Scripting;
using VSCC.State;
using VSCC.VersionManager;

namespace VSCC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AppState.Current.FreezeAutocalc = true;
            AppState.Current.AppThread = Thread.CurrentThread;
            if (!Debugger.IsAttached)
            {
                if (Settings.Default.Language == 0)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                }

                if (Settings.Default.Language == 1)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
                }
            }

            InitializeComponent();
            AppState.Current.FreezeAutocalc = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppState.Current.Window = this;
            ScriptEngine.Create();
            AppEvents.InvokeStartup();
#pragma warning disable CS4014 // This call is executed on an another thread entirely, the await is thus not needed.
            new Thread(() => VersionChecker.CheckVersion(false)).Start();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void NewEmpty_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Resources.Generic_Unsaved_Description, Properties.Resources.Generic_Unsaved_Title, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    this.Save_Click(sender, null);
                }
            }

            AppState.Current.FreezeAutocalc = true;
            AppState.Current.State.Clear();
            AppState.Current.LastSaveFile = string.Empty;
            AppState.Current.FreezeAutocalc = false;
        }

        private void Exit_Click(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Resources.Generic_Unsaved_Description, Properties.Resources.Generic_Unsaved_Title, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    this.Save_Click(sender, null);
                }
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "JSON Files(*.json)|*.json",
                Multiselect = false
            };

            if (ofd.ShowDialog() ?? false)
            {
                AppState.Current.Load(System.IO.File.ReadAllText(ofd.FileName), out bool wantsRecalc);
                AppState.Current.SetSaveLocation(ofd.FileName, true);
                if (wantsRecalc)
                {
                    if (MessageBox.Show(Properties.Resources.Generic_OldSave_Pass1, Properties.Resources.Generic_OldSave_Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        AppState.Current.TGeneral.RebuildAllStats();
                    }

                    if (MessageBox.Show(Properties.Resources.Generic_OldSave_Pass2, Properties.Resources.Generic_OldSave_Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        this.Save_Click(sender, e);
                    }
                }
            }
        }

        private void Save_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AppState.Current.LastSaveFile))
            {
                this.SaveAs_Click(sender, e);
            }
            else
            {
                try
                {
                    System.IO.File.WriteAllText(AppState.Current.LastSaveFile, AppState.Current.Save());
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show(string.Format(Properties.Resources.Generic_SaveError_Description, ex.GetType().FullName, ex.Message), Properties.Resources.Generic_SaveError_Title, MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        this.SaveAs_Click(sender, e);
                    }
                }
            }
        }

        private void SaveAs_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "JSON Files(*.json)|*.json",
            };

            if (sfd.ShowDialog() ?? false)
            {
                AppState.Current.SetSaveLocation(sfd.FileName, false);
                System.IO.File.WriteAllText(AppState.Current.LastSaveFile, AppState.Current.Save());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Resources.Generic_Unsaved_Description, Properties.Resources.Generic_Unsaved_Title, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == MessageBoxResult.No)
                {
                    AppEvents.InvokeExit();
                    e.Cancel = false;
                }

                if (result == MessageBoxResult.Yes)
                {
                    this.Save_Click(sender, null);
                    e.Cancel = false;
                    AppEvents.InvokeExit();
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new InfoWindow().Show();
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ((MenuItem)sender).IsEnabled = false;
            await VersionChecker.CheckVersion();
            ((MenuItem)sender).IsEnabled = true;
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            if (!AppState.Current.FreezeAutocalc)
            {
                this.Language_English.IsChecked = true;
                this.Language_Russian.IsChecked = false;
                Settings.Default.Language = 0;
                Settings.Default.Save();
            }
        }

        private void MenuItem_Checked_1(object sender, RoutedEventArgs e)
        {
            if (!AppState.Current.FreezeAutocalc)
            {
                this.Language_Russian.IsChecked = true;
                this.Language_English.IsChecked = false;
                Settings.Default.Language = 1;
                Settings.Default.Save();
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"),
                Filter = "LUA Script(*.lua)|*.lua",
                Multiselect = false
            };

            if (ofd.ShowDialog() ?? false)
            {
                ScriptEngine.Instance.Value.DoFile(ofd.FileName);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            new ScriptsWindow().Show();
        }
    }
}
