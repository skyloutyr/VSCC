using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VSCC.Skins;

namespace VSCC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ChangeSkin()
        {
            foreach (ResourceDictionary dict in this.Resources.MergedDictionaries)
            {
                if (dict is SkinResourceDictionary skinDict)
                {
                    skinDict.UpdateSource();
                }
                else
                {
                    dict.Source = dict.Source;
                }
            }
        }
    }
}
