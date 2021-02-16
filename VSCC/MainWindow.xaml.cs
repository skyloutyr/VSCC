﻿namespace VSCC
{
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using VSCC.Controls.Windows;
    using VSCC.DataType;
    using VSCC.Properties;
    using VSCC.Roll20;
    using VSCC.Scripting;
    using VSCC.Skins;
    using VSCC.State;
    using VSCC.Structs;
    using VSCC.VersionManager;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _forceClose;
        private bool _marketplaceOpen;

        public string OldWindowSaveData { get; set; }

        public MainWindow()
        {
            this.Dispatcher.UnhandledException += this.Dispatcher_UnhandledException;
            AppState.Current.FreezeAutocalc = true;
            AppState.Current.AppThread = Thread.CurrentThread;
            this.ChangeLanguage(Settings.Default.Language, true, false);
            this.ChangeSkin(Settings.Default.Skin, false);
            this.InitializeComponent();
            AppState.Current.FreezeAutocalc = false;
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, err) => true;
            Version v = Environment.OSVersion.Version;
            if (v < new Version(6, 2)) // Win 7 or older
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3;
            }
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                try
                {
                    if (!this.TryFindResourceSafe("CrashTitle", out string title))
                    {
                        title = "Application has terminated!";
                    }

                    if (!this.TryFindResourceSafe("CrashDesc", out string desc))
                    {
                        desc = "The application has terminated with an unhandled exception: {0}[{1}]. Would you like to reset the application settings to factory default?";
                    }

                    desc = string.Format(desc, e.Exception, e.Exception.HResult);
                    if (MessageBox.Show(desc, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Settings.Default.Reset();
                        Settings.Default.Save();
                    }
                }
                catch
                {
                    throw e.Exception;
                }
            }
        }

        public static string Translate(string key, params object[] pars) => string.Format(VSCC.Properties.Resources.ResourceManager.GetString(key), pars);

        private bool TryFindResourceSafe(string resourceName, out string resource)
        {
            try
            {
                resource = (string)this.FindResource(resourceName);
                return true;
            }
            catch
            {
                resource = string.Empty;
                return false;
            }
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
            new Thread(() => VersionChecker.CheckVersion(false, true, (s) => this.Dispatcher.Invoke(() => UpdateManager.Update(s)))).Start();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (!string.IsNullOrEmpty(this.OldWindowSaveData))
            {
                AppState.Current.Load(this.OldWindowSaveData, out _);
                this.OldWindowSaveData = null;
            }

            this.AllowThemeSwitch.IsChecked = Settings.Default.AllowSkinChangesOnOlderWindowsVersions;
            this.Language_English.IsChecked = Settings.Default.Language.Equals("en-US");
            this.Language_Russian.IsChecked = Settings.Default.Language.Equals("ru-RU");
            this.Skin_Default.IsChecked = Settings.Default.Skin == 0;
            this.Skin_Bright.IsChecked = Settings.Default.Skin == 1;
            this.Skin_Dark.IsChecked = Settings.Default.Skin == 2;
            this.Skin_Soft.IsChecked = Settings.Default.Skin == 3;
            this.Skins.IsEnabled = SkinResourceDictionary.IsRunningWin8OrGreater();
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

        private void Exit_Click(object sender, ExecutedRoutedEventArgs e) => this.Close();

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
                AppState.Current.Load(System.IO.File.ReadAllText(ofd.FileName), out LoadFlags flags);
                AppState.Current.SetSaveLocation(ofd.FileName, true);
                if (flags.HasFlag(LoadFlags.V2AdaptV1))
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

                if (flags.HasFlag(LoadFlags.V2InventoryWeightsMissing))
                {
                    if (MessageBox.Show(Properties.Resources.Generic_No_Inventory_Weight_Desc, Properties.Resources.Generic_No_Inventory_Weight_Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        AppState.Current.TInventory.RecalculateWeights(true, true, true);
                    }
                }

                if (flags.HasFlag(LoadFlags.V2NoObjectIDs))
                {
                    AppState.Current.CreateObjectIDs();
                }

                if (flags.HasFlag(LoadFlags.V2OldFeats))
                {
                    string featsStr = AppState.Current.LoadObject["Extras"]["Feats"].ToObject<string>();
                    string traitsStr = AppState.Current.LoadObject["Extras"]["Traits"].ToObject<string>();
                    foreach (string line in featsStr.Split('\n'))
                    {
                        Feat f = new Feat { ImageList = AppState.Current.TExtras.Images, DescProperty = Translate("Feat_Desc_NeedsConversion"), NameProperty = Translate("Feat_Name_Old"), FullDescProperty = line, ImageIndex = "if886_t" };
                        AppState.Current.State.Extras.FeatsArray.Add(f);
                    }

                    foreach (string line in traitsStr.Split('\n'))
                    {
                        Feat f = new Feat { ImageList = AppState.Current.TExtras.Images, DescProperty = Translate("Feat_Desc_NeedsConversion"), NameProperty = Translate("Feat_Name_Old"), FullDescProperty = line, ImageIndex = "if886_t" };
                        AppState.Current.State.Extras.TraitsArray.Add(f);
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
                if (!this.CloseSelf())
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                R20Logger.Close();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) => new InfoWindow().Show();

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

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) => new ScriptsWindow().Show();

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
            if (SkinResourceDictionary.IsRunningWin8OrGreater())
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

        private void AllowThemeSwitch_Checked(object sender, RoutedEventArgs e)
        {
            bool val = Settings.Default.AllowSkinChangesOnOlderWindowsVersions = this.AllowThemeSwitch.IsChecked;
            Settings.Default.Save();
            if (!val && Settings.Default.Skin != 0 && !SkinResourceDictionary.IsRunningWin8OrGreater())
            {
                this.ChangeSkin(0);
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (ScriptEngine.Instance.IsValueCreated && !this._marketplaceOpen)
            {
                this._marketplaceOpen = true;
                ScriptsMarketplace sm = new ScriptsMarketplace();
                sm.Closed += (o, ea) => this._marketplaceOpen = false;
                sm.Show();
            }
        }
    }
}
