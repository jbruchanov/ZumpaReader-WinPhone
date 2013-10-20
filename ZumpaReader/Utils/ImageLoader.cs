using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZumpaReader.Utils
{
    public class ImageLoader
    {
        private Random _random = new Random((int)DateTime.Now.Ticks);

        private Dictionary<string, string> _cache = new Dictionary<string, string>();

        private IsolatedStorageFile _storage;

        static string[] IMAGE_EXTS = { "jpg", "jpeg", "gif", "png", "bmp" };        

        public ImageLoader()
        {
            _storage = IsolatedStorageFile.GetUserStoreForApplication();
        }

        /// <summary>
        /// Load link async
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<Stream> LoadAsync(string link)
        {
            string localFile = null;
            Stream s = null;
            if (!_cache.TryGetValue(link, out localFile))
            {
                s = await SaveLinkAsync(link);
            }
            else
            {                
                var t = new Task<Stream>(() => LoadFile(localFile));                
                t.Start();
                s = await t;
            }
            return s;
        }

        /// <summary>
        /// Load file from local storage
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Stream LoadFile(string file)
        {
            if (_storage.FileExists(file))
            {
                return _storage.OpenFile(file, FileMode.Open, FileAccess.Read);
            }
            return null;
        }

        /// <summary>
        /// Save image to local storage
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<Stream> SaveLinkAsync(string link)
        {
            string file = GenerateRandomFileName(ExtractExt(link));

            if (_storage.FileExists(file))
            {
                //this should not happen
            }
            Stream imageStream = null;
            using (IsolatedStorageFileStream fileStream = _storage.CreateFile(file))
            {
                imageStream = await LoadLinkAsync(link);
                await imageStream.CopyToAsync(fileStream);
                imageStream.Position = 0;
            }
            _cache[link] = file;
            return imageStream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool IsLoaded(string link)
        {
            return _cache.ContainsKey(link);
        }

        /// <summary>
        /// Load image from web
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<Stream> LoadLinkAsync(string link)
        {
            var wc = new WebClient();
            Stream s = await wc.OpenReadTaskAsync(link);
            return s;
        }

        /// <summary>
        /// Get extension of link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private static string ExtractExt(string link)
        {
            string[] subs = link.Split(new char[] { '.' });
            return subs[subs.Length - 1];
        }

        /// <summary>
        /// Generate random numeric file name
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private string GenerateRandomFileName(string ext)
        {
            return String.Format("{0}.{1}", _random.Next(1000000, 9999999), ext);
        }

        /// <summary>
        /// Simple check if link targets image
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static bool IsImageLink(string link)
        {
            if (!String.IsNullOrEmpty(link))
            {
                link = link.ToLower();
                foreach (var item in IMAGE_EXTS)
                {
                    if (link.EndsWith(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
