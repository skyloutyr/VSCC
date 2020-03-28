namespace VSCC
{
    using System.Windows;
    using VSCC.Skins;

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
