using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private bool _forceClose;

        public string OldWindowSaveData { get; set; }

        public MainWindow()
        {
            AppState.Current.FreezeAutocalc = true;
            AppState.Current.AppThread = Thread.CurrentThread;
            ChangeLanguage(Settings.Default.Language, true, false);
            ChangeSkin(Settings.Default.Skin, false);
            InitializeComponent();
            AppState.Current.FreezeAutocalc = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppState.Current.Window = this;
            if (Debugger.IsAttached)
            {
                AppState.Current.GenerateDefaultMD5();
            }

            AppState.Current.SetDefaultMD5();
            ScriptEngine.Create();
            AppEvents.InvokeStartup();

#pragma warning disable CS4014 // This call is executed on an another thread entirely, the await is thus not needed.
            new Thread(() => VersionChecker.CheckVersion(false, false, (s) => Dispatcher.Invoke(() => UpdateManager.Update(s)))).Start();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (!string.IsNullOrEmpty(this.OldWindowSaveData))
            {
                AppState.Current.Load(this.OldWindowSaveData, out _);
                this.OldWindowSaveData = null;
            }

            this.Language_English.IsChecked = Settings.Default.Language.Equals("en-US");
            this.Language_Russian.IsChecked = Settings.Default.Language.Equals("ru-RU");
            this.Skin_Default.IsChecked = Settings.Default.Skin == 0;
            this.Skin_Bright.IsChecked = Settings.Default.Skin == 1;
            this.Skin_Dark.IsChecked = Settings.Default.Skin == 2;
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
            AppState.Current.SetDefaultMD5(false);
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
            if (!this._forceClose)
            {
                if (!CloseSelf())
                {
                    e.Cancel = true;
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
                this.ChangeLanguage((sender as MenuItem).Tag.ToString(), false, true);
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

        public void ChangeLanguage(string lang, bool forceChange = false, bool reloadApp = false)
        {
            // Legacy afapter
            if (lang.Length == 1 && char.IsNumber(lang[0]))
            {
                lang = lang[0] == '0' ? "en-US" : "ru-RU";
            }

            bool langEquals = Settings.Default.Language.Equals(lang);
            if (!langEquals || forceChange)
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                if (!langEquals)
                {
                    Settings.Default.Language = lang;
                    Settings.Default.Save();
                    if (reloadApp)
                    {
                        string s = AppState.Current.Save();
                        this._forceClose = true;
                        AppEvents.InvokeExit();
                        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                        this.Close();
                        new MainWindow()
                        {
                            OldWindowSaveData = s
                        }.Show();
                        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        return;
                    }
                }
            }
        }

        public void ChangeSkin(int skinID, bool reloadApp = false)
        {
            bool skinEquals = Settings.Default.Skin == skinID;
            if (!skinEquals)
            {
                Settings.Default.Skin = skinID;
                Settings.Default.Save();
                ((App)Application.Current).ChangeSkin();
                if (reloadApp)
                {
                    string s = AppState.Current.Save();
                    this._forceClose = true;
                    AppEvents.InvokeExit();
                    Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                    this.Close();
                    new MainWindow()
                    {
                        OldWindowSaveData = s
                    }.Show();
                    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    return;
                }
            }
        }

        public bool CloseSelf()
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Resources.Generic_Unsaved_Description, Properties.Resources.Generic_Unsaved_Title, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }

                if (result == MessageBoxResult.No)
                {
                    AppEvents.InvokeExit();
                    return true;
                }

                if (result == MessageBoxResult.Yes)
                {
                    this.Save_Click(this.Save, null);
                    AppEvents.InvokeExit();
                    return true;
                }
            }

            return true;
        }

        private void Skin_Default_Checked(object sender, RoutedEventArgs e)
        {
            if (!AppState.Current.FreezeAutocalc)
            {
                this.ChangeSkin(int.Parse((sender as MenuItem).Tag.ToString()), true);
            }
        }
    }
}
