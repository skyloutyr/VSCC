using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VSCC.Models.ImageList
{
    public class ImageModel
    {
        private volatile bool _finished;
        private Func<string, Tuple<bool, Func<Stream>>> _imageSourceGetter;
        private BitmapImage _img;

        public BitmapImage Image
        {
            get => this._finished ? this._img : null;
        }

        public bool Async { get; set; }
        public string Name { get; }

        public ImageModel(string name, Func<string, Tuple<bool, Func<Stream>>> imgGetter)
        {
            this.Name = name;
            this._imageSourceGetter = imgGetter;
        }

        public void Load()
        {
            void a()
            {
                Tuple<bool, Func<Stream>> d = this._imageSourceGetter(this.Name);
                if (d.Item1)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Stream s = d.Item2())
                        {
                            s.CopyTo(ms);
                        }

                        ms.Position = 0;
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                        bi.Freeze();
                        if (this.Async)
                        {
                            Dispatcher.CurrentDispatcher.Invoke(() => this._img = bi);
                        }
                        else
                        {
                            this._img = bi;
                        }

                        this._finished = true;
                    }
                }
            }

            if (this.Async)
            {
                Task.Run(a);
            }
            else
            {
                a();
            }
        }
    }
}
