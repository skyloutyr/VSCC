using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace VSCC.Models.ImageList
{
    public class ImageListModel : IList<ImageModel>
    {
        public bool Async { get; set; } = true;
        public ImageModel this[int index] { get => this.Images[index]; set => this.Images[index] = value; }
        public ImageModel this[string index] { get => string.IsNullOrEmpty(index) ? null : this.Images.FirstOrDefault(i => i.Name.ToLower().Equals(index.ToLower())); set => this.Images[this.Images.IndexOf(this.Images.First(i => i.Name.ToLower().Equals(index.ToLower())))] = value; }

        public ObservableCollection<ImageModel> Images { get; } = new ObservableCollection<ImageModel>();

        public int Count => this.Images.Count;
        public bool IsReadOnly => false;
        public void Add(ImageModel item) => this.Images.Add(item);
        public void Clear() => this.Images.Clear();
        public bool Contains(ImageModel item) => this.Images.Contains(item);
        public void CopyTo(ImageModel[] array, int arrayIndex) => this.Images.CopyTo(array, arrayIndex);
        public IEnumerator<ImageModel> GetEnumerator() => this.Images.GetEnumerator();
        public int IndexOf(ImageModel item) => this.Images.IndexOf(item);
        public void Insert(int index, ImageModel item) => this.Images.Insert(index, item);
        public bool Remove(ImageModel item) => this.Images.Remove(item);
        public void RemoveAt(int index) => this.Images.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void LoadFromPhysicalFolder(string folderPath, bool enumerateSubdirectories = true)
        {
            folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPath);
            List<ImageModel> prev = new List<ImageModel>();
            foreach (string path in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
            {
                string savedPath = path;
                ImageModel img;
                prev.Add(img = new ImageModel(Path.GetFileNameWithoutExtension(path), s => new Tuple<bool, Func<Stream>>(true, () => File.OpenRead(savedPath))));
                img.Async = this.Async;
                img.Load();
            }

            prev.Sort((l, r) => string.Compare(l.Name, r.Name));
            foreach (ImageModel mdl in prev)
            {
                this.Images.Add(mdl);
            }
        }

        public void LoadFromEmbeddedFolder(string folderPath)
        {
            folderPath = folderPath.ToLower();
            Assembly a = Assembly.GetExecutingAssembly();
            ResourceManager rm = new ResourceManager("VSCC.g", a);
            ResourceSet rs = rm.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            List<ImageModel> prev = new List<ImageModel>();
            foreach (DictionaryEntry kv in rs)
            {
                string path = kv.Key as string;
                if (path.StartsWith(folderPath))
                {
                    string savedPath = path;
                    ImageModel img;
                    Stream st = kv.Value as Stream;
                    string nameSubstr = path.Substring(path.LastIndexOf('/', path.LastIndexOf('.') - 1) + 1);
                    prev.Add(img = new ImageModel(nameSubstr.Substring(0, nameSubstr.LastIndexOf('.')).Trim(), s => new Tuple<bool, Func<Stream>>(true, () => st)));
                    img.Async = this.Async;
                    img.Load();
                }
            }

            prev.Sort((l, r) => string.Compare(l.Name, r.Name));
            foreach (ImageModel mdl in prev)
            {
                this.Images.Add(mdl);
            }
        }
    }
}
