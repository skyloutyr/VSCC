using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VSCC.Properties;

namespace VSCC.Skins
{
    public class SkinResourceDictionary : ResourceDictionary
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryValueName = "AppsUseLightTheme";


        private Uri _defaultSource;
        private Uri _darkSource;

        public Uri DefaultSource
        {
            get => this._defaultSource;
            set
            {
                _defaultSource = value;
                this.UpdateSource();
            }
        }
        public Uri DarkSource
        {
            get => this._darkSource;
            set
            {
                _darkSource = value;
                this.UpdateSource();
            }
        }

        public void UpdateSource()
        {
            Uri value = Settings.Default.Skin == 0 ? this.ResolveSystemSkin() : Settings.Default.Skin == 1 ? this._defaultSource : this._darkSource;
            if (value != null && base.Source != value)
            {
                base.Source = value;
            }
        }

        private Uri ResolveSystemSkin()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            string query = string.Format(CultureInfo.InvariantCulture, @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'", currentUser.User.Value, RegistryKeyPath.Replace(@"\", @"\\"), RegistryValueName);
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistryValueName);
                if (registryValueObject == null)
                {
                    return this._defaultSource;
                }

                int registryValue = (int)registryValueObject;
                return registryValue > 0 ? this._defaultSource : this._darkSource;
            }
        }
    }
}
