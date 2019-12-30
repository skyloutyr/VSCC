using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using VSCC.Controls.Tabs;

namespace VSCC.State
{
    public class AppState
    {
        private string _lastSaveHash;
        private bool _ignoreLastSaveFile;

        public static AppState Current { get; set; } = new AppState();

        public string LastSaveFile { get; set; } = string.Empty;
        public bool FreezeAutocalc { get; set; } = false;
        public MainWindow Window { get; set; }

        public Thread AppThread { get; set; }

        public SaveState State { get; set; } = new SaveState();

        public GeneralTab TGeneral => ((DockPanel)this.Window.TabGeneral.Content).Children[0] as GeneralTab;
        public ExtrasTab TExtras => ((DockPanel)this.Window.TabExtra.Content).Children[0] as ExtrasTab;
        public InventoryTab TInventory => ((DockPanel)this.Window.TabInventory.Content).Children[0] as InventoryTab;
        public SpellbookTab TSpellbook => ((DockPanel)this.Window.TabSpellbook.Content).Children[0] as SpellbookTab;
        public ScriptingTab TScripting => ((DockPanel)this.Window.TabScripting.Content).Children[0] as ScriptingTab;

        public void SetDefaultMD5(bool isDefault = true)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Defaults.md5");
            if (File.Exists(path))
            {
                byte[] b = File.ReadAllBytes(path);
                byte[] md5 = new byte[16];
                Array.Copy(b, isDefault ? 0 : 16, md5, 0, 16);
                StringBuilder sBuilder = new StringBuilder();
                foreach (byte b1 in md5)
                {
                    sBuilder.Append(b1.ToString("x2"));
                }

                this._lastSaveHash = sBuilder.ToString();
                this._ignoreLastSaveFile = true;
            }
        }

        public void GenerateDefaultMD5()
        {
            byte[] GetMD5(string input)
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    return md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                }
            }

            string s = this.Save();
            byte[] l = GetMD5(s);
            this.State.Clear();
            byte[] r = GetMD5(this.Save());
            
            // Idealy both MD5 will be the same
            byte[] joined = new byte[l.Length + r.Length];
            Array.Copy(l, 0, joined, 0, l.Length);
            Array.Copy(r, 0, joined, l.Length, r.Length);
            File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Defaults.md5"), joined);
            this.Load(s, out _);
        }

        public string Save()
        {
            this.FreezeAutocalc = true;
            string s = this.State.Save();
            AppEvents.InvokeSave(ref s);
            using (MD5 md5Hash = MD5.Create())
            {
                this._lastSaveHash = GetMd5Hash(md5Hash, s);
            }

            this.FreezeAutocalc = false;
            this._ignoreLastSaveFile = false;
            return s;
        }

        public void Load(string s, out bool wantsRecalc)
        {
            this.FreezeAutocalc = true;
            int saveVersion = this.LookupSaveVersion(s);
            this.State.Clear();
            switch (saveVersion)
            {
                case 1:
                    {
                        this.LoadV1(s);
                        wantsRecalc = true;
                        break;
                    }

                case 2:
                    {
                        this.LoadV2(s);
                        wantsRecalc = false;
                        break;
                    }

                default:
                    throw new NotSupportedException($"The specified save file version can't be loaded - no format converter exists for version { saveVersion }.");
            }

            AppEvents.InvokeLoad(ref s);
            this.FreezeAutocalc = false;
            this._ignoreLastSaveFile = false;
        }

        public void SetSaveLocation(string location, bool calculateMD5)
        {
            this.LastSaveFile = location;
            if (calculateMD5)
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    this._lastSaveHash = GetMd5Hash(md5Hash, File.ReadAllText(location));
                }
            }
        }

        private int LookupSaveVersion(string s)
        {
            try
            {
                JObject jobj = JObject.Parse(s);
                if (jobj.TryGetValue("Version", out JToken versionToken))
                {
                    if (versionToken.Type == JTokenType.Integer)
                    {
                        return versionToken.ToObject<int>();
                    }

                    if (versionToken.Type == JTokenType.String)
                    {
                        if (int.TryParse(versionToken.ToObject<string>(), out int ret))
                        {
                            return ret;
                        }
                    }
                }
            }
            catch (JsonReaderException)
            {
                return -1;
            }

            return 1;
        }

        private void LoadV2(string s)
        {
            this.State.Load(s);
            if (this.State.Inventory.WeightCurrent == 0 && this.State.Inventory.WeightMax1 == 0 && this.State.Inventory.WeightMax2 == 0)
            {
                this.TInventory.RecalculateWeights(true, true, true);
            }
        }

        private void LoadV1(string s)
        {
            Legacy.SaveV1Adapter.Load(s);
        }

        //TODO make this reasonable
        public bool UnsavedChangesExist
        {
            get
            {
                if (string.IsNullOrEmpty(this.LastSaveFile) && !this._ignoreLastSaveFile)
                {
                    return true;
                }

                using (MD5 md5Hash = MD5.Create())
                {
                    string s = this.State.Save();
                    string hash = GetMd5Hash(md5Hash, s);
                    if (hash.Equals(this._lastSaveHash, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte b in data)
            {
                sBuilder.Append(b.ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
