using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
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
using VSCC.State;

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
            InitializeComponent();
            AppState.Current.FreezeAutocalc = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppState.Current.Window = this;
        }

        private void NewEmpty_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show("There are unsaved changes. Do you want to save the file before resetting it? Press Yes to save the file and reset it. Press No to discard all changes and reset it. Press Cancel to abort the operation.", "Unsaved changes exist!", MessageBoxButton.YesNoCancel);
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

        private void Open_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show("There are unsaved changes. Do you want to save the file before opening a new one? Press Yes to save the file and open the new one. Press No to discard all changes and open a new file. Press Cancel to abort the operation.", "Unsaved changes exist!", MessageBoxButton.YesNoCancel);
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
                    if (MessageBox.Show("An old or incompatible save file was loaded. Due to the way stat calculations were changed it is recommended to do a stat rebuild. Do the rebuild?", "Old save file loaded", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        AppState.Current.TGeneral.RebuildAllStats();
                    }

                    if (MessageBox.Show("An old or incompatible save file was loaded. Due to general potential save partial or full incompatibility it is recommended to override the file. Save now?", "Old save file loaded", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                    if (MessageBox.Show($"Couldn't save the sheet to the previous location: { ex.GetType().FullName }, { ex.Message }. Press OK to select a different file to save to. Press Cancel to cancel the save alltogether.", "Couldn't save the sheet!", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
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

        private void Exit_Click(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppState.Current.UnsavedChangesExist)
            {
                MessageBoxResult result = MessageBox.Show("There are unsaved changes. Do you want to save the file before exiting? Press Yes to save the file and exit. Press No to discard all changes and exit. Press Cancel to abort the operation.", "Unsaved changes exist!", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = false;
                }

                if (result == MessageBoxResult.Yes)
                {
                    this.Save_Click(sender, null);
                    e.Cancel = false;
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
            try
            {
                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpWebRequest req = WebRequest.CreateHttp("https://raw.githubusercontent.com/skyloutyr/VSCC/master/VSCC/Version.json");
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                req.CachePolicy = noCachePolicy;
                Task<WebResponse> responseTask = req.GetResponseAsync();
                JObject localJObj = JObject.Parse(System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Version.json")));
                SemVer.Version localVersion = new SemVer.Version(localJObj["version"].ToObject<string>());
                using (WebResponse response = await responseTask)
                {
                    using (Stream s = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            string result = sr.ReadToEnd();
                            JObject remoteJObj = JObject.Parse(result);
                            SemVer.Version remoteVersion = new SemVer.Version(remoteJObj["version"].ToObject<string>());
                            if (localVersion < remoteVersion)
                            {
                                MessageBox.Show($"An update is available!\n\r{ remoteJObj["changelog"][remoteJObj["version"].ToObject<string>()].ToObject<string>() }", "Local version is outdated!");
                            }

                            if (localVersion > remoteVersion)
                            {
                                MessageBox.Show($"", "Remote version is outdated!");
                            }

                            if (localVersion == remoteVersion)
                            {
                                MessageBox.Show($"The local version corresponds to the latest remote version.", "You are using the latest version.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong while checking for the version:{ ex.GetType().FullName }\n\r{ ex.Message }", "Couldn't check version!");
            }
            finally
            {
                ((MenuItem)sender).IsEnabled = true;
            }
        }
    }
}
