namespace VSCC.Skins
{
    using Microsoft.Win32;
    using System;
    using System.Windows;
    using VSCC.Properties;

    public class SkinResourceDictionary : ResourceDictionary
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryValueName = "AppsUseLightTheme";


        private Uri _defaultSource;
        private Uri _darkSource;
        private Uri _softSource;

        public Uri DefaultSource
        {
            get => this._defaultSource;
            set
            {
                this._defaultSource = value;
                this.UpdateSource();
            }
        }
        public Uri DarkSource
        {
            get => this._darkSource;
            set
            {
                this._darkSource = value;
                this.UpdateSource();
            }
        }

        public Uri SoftSource
        {
            get => this._softSource;
            set
            {
                this._softSource = value;
                this.UpdateSource();
            }
        }

        public void UpdateSource()
        {
            Uri value =
                IsRunningWin8OrGreater() ?
                    Settings.Default.Skin == 0 ?
                        this.ResolveSystemSkin() :
                    Settings.Default.Skin == 1 ?
                        this._defaultSource :
                    Settings.Default.Skin == 3 ?
                        this._softSource :
                    this._darkSource :
                this._defaultSource;

            if (value != null && base.Source != value)
            {
                base.Source = value;
            }
        }

        private Uri ResolveSystemSkin()
        {
            try
            {
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
            catch
            {
                return this._defaultSource;
            }
        }

        public static bool IsRunningWin8OrGreater()
        {
            OperatingSystem os = Environment.OSVersion;

            // Only support skins on windows
            if (os.Platform == PlatformID.Win32NT)
            {
                Version vs = os.Version;

                // win 8 or greater
                return Settings.Default.AllowSkinChangesOnOlderWindowsVersions || vs.Major == 10 || (vs.Major == 6 && vs.Minor >= 2);
            }

            return true;
        }
    }
}
