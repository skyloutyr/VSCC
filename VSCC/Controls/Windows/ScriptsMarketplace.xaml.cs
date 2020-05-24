namespace VSCC.Controls.Windows
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using VSCC.Scripting.Marketplace;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for ScriptsMarketplace.xaml
    /// </summary>
    public partial class ScriptsMarketplace : Window
    {
        public static ScriptsMarketplace CurrentWindow { get; set; }
        public bool IsClosed { get; set; }

        public ScriptsMarketplace()
        {
            CurrentWindow = this;
            this.InitializeComponent();
            MarketplaceManager.Instance.Index = null;
            MarketplaceManager.Instance.LocalDB.Clear();
            MarketplaceManager.Instance.RemoteDB.Clear();
            MarketplaceManager.Instance.Alerts.Clear();
            this.Closed += (o, e) =>
            {
                this.IsClosed = true;
                MarketplaceManager.Instance.RemoteDB.CollectionChanged -= this.HandleCollectionChanged;
                MarketplaceManager.Instance.LocalDB.CollectionChanged -= this.HandleLocalCollectionChanged;
                CurrentWindow = null;
            };

            ((Storyboard)this.FindResource("FadeOut")).Completed += (o, e) => this.Dispatcher.Invoke(() => this.InitMarketplace());
            MarketplaceManager.Instance.RemoteDB.CollectionChanged += this.HandleCollectionChanged;
            MarketplaceManager.Instance.LocalDB.CollectionChanged += this.HandleCollectionChanged;
            this.ListView_Online.ItemsSource = MarketplaceManager.Instance.RemoteDB;
            this.ListView_Local.ItemsSource = MarketplaceManager.Instance.LocalDB;
            Task.Run(this.FetchIndex);
        }

        public void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) => this.ListView_Online.Items.Refresh();
        public void HandleLocalCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) => this.ListView_Local.Items.Refresh();

        public void HandleIndex(string s, Exception ex)
        {
            if (ex != null)
            {
                if (MessageBox.Show(Properties.Resources.Marketplace_Error_Desc + $"" +
                    $"\n{ ex.GetType() }\n{ ex.Message }\n{ ex.StackTrace }", Properties.Resources.Marketplace_Error_Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Task.Run(this.FetchIndex);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MarketplaceManager.Instance.LoadIndex(s);
                ((Storyboard)this.FindResource("FadeOut")).Begin(this.GridLoading);
            }
        }

        public void InitMarketplace()
        {
            this.Marketplace.Visibility = Visibility.Visible;
            if (MarketplaceManager.Instance.Index == null)
            {
                Task.Run(() =>
                {
                    while (MarketplaceManager.Instance.Index == null)
                    {
                        Thread.Sleep(1000);
                        if (this.IsClosed)
                        {
                            return;
                        }
                    }

                    this.InitMarketplace();
                });
            }
            else
            {
                MarketplaceManager.Instance.LoadLocalMarketplace(this.Dispatcher);
                MarketplaceManager.Instance.LoadOnlineMarketplace(this.Dispatcher);
            }
        }

        public void FetchIndex()
        {
            try
            {
                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpWebRequest req = WebRequest.CreateHttp("https://github.com/skyloutyr/VSCCScripts/raw/master/index.json");
                req.Timeout = 5000;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                req.CachePolicy = noCachePolicy;
                using (WebResponse response = req.GetResponse())
                {
                    using (Stream s = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            string result = sr.ReadToEnd();
                            this.Dispatcher.Invoke(() => this.HandleIndex(result, null));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Dispatcher.Invoke(() => this.HandleIndex(null, e));
            }
        }

        // Local delete
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MarketplaceEntry me = (MarketplaceEntry)((Button)sender).DataContext;
            if (MessageBox.Show("Are you sure you want to delete this script? This action can't be reversed!", "Confirm script deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (me.HasNoMetadata) // Is outdated local script before the marketplace introduction
                {
                    File.Delete(me.Link);
                }
                else
                {
                    File.Delete(me.LocalPath);
                    File.Delete(me.LocalScriptPath);
                    FileInfo fi = new FileInfo(me.LocalScriptPath);
                    if (fi.Directory.GetFiles().Length == 0)
                    {
                        fi.Directory.Delete();
                    }
                }

                MarketplaceManager.Instance.LocalDB.Remove(me);
                MarketplaceEntry otherEntry = MarketplaceManager.Instance.RemoteDB.FirstOrDefault(re => re.ProjectID.Equals(me.ProjectID, StringComparison.OrdinalIgnoreCase));
                if (otherEntry != null)
                {
                    otherEntry.DownloadButtonEnabled = true;
                }
            }
        }

        // Local or remote download
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MarketplaceEntry me = (MarketplaceEntry)((Button)sender).DataContext;
            Action post = null;
            if (me.IsLocal) // Update is pressed, just delete local and treat as remote DL
            {
                MarketplaceEntry capture = me;
                post = new Action(() =>
                {

                    File.Delete(capture.LocalPath);
                    File.Delete(capture.LocalScriptPath);
                    FileInfo fi = new FileInfo(capture.LocalScriptPath);
                    if (fi.Directory.GetFiles().Length == 0)
                    {
                        fi.Directory.Delete();
                    }

                    MarketplaceManager.Instance.LocalDB.Remove(capture);
                });

                me = MarketplaceManager.Instance.RemoteDB.FirstOrDefault(re => re.ProjectID.Equals(me.ProjectID, StringComparison.OrdinalIgnoreCase));
            }

            try
            {
                WebClient wc = new WebClient();
                string s = wc.DownloadString(me.Link);
                wc.Dispose();
                using (MD5 md5 = MD5.Create())
                {
                    if (!string.Equals(AppState.GetMd5Hash(md5, s), me.MD5, StringComparison.OrdinalIgnoreCase))
                    {
                        MarketplaceManager.Instance.Alerts.Add(MarketplaceAlert.DownloadFailed);
                    }
                }

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", me.ProjectID);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                post?.Invoke();
                string meta = JsonConvert.SerializeObject(me);
                me.LocalPath = Path.Combine(path, me.ProjectID + ".json");
                me.LocalScriptPath = Path.Combine(path, me.ProjectID + ".lua");
                File.WriteAllText(me.LocalPath, meta);
                File.WriteAllText(me.LocalScriptPath, s);
                MarketplaceEntry local = JsonConvert.DeserializeObject<MarketplaceEntry>(meta);
                local.LocalPath = me.LocalPath;
                local.LocalScriptPath = me.LocalScriptPath;
                local.IsLocal = true;
                local.MD5 = me.MD5;
                MarketplaceManager.Instance.Alerts.Add(MarketplaceAlert.RestartNeeded);
                MarketplaceManager.Instance.LocalDB.Add(local);
                MarketplaceManager.Instance.CheckMarketplaceEntryAvailability(local);
            }
            catch
            {
                MarketplaceManager.Instance.Alerts.Add(MarketplaceAlert.DownloadFailed);
            }
        }
    }
}
