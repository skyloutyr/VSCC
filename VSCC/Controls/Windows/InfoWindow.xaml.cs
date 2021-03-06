﻿namespace VSCC.Controls.Windows
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        private readonly Lazy<string> _currentVersion = new Lazy<string>(() => VersionManager.VersionChecker.GetCurrentVersion().ToString());

        public string CurrentVersion
        {
            get => this._currentVersion.Value;

            set
            {
            }
        }

        public InfoWindow() => this.InitializeComponent();

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) => System.Diagnostics.Process.Start(e.Uri.ToString());
    }
}
