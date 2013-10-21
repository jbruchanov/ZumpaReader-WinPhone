using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader.Model;

namespace ZumpaReader.Utils
{
    public class ImageLoader
    {
        private Random _random = new Random((int)DateTime.Now.Ticks);

        private Dictionary<string, ImageRecord> _cache;

        private IsolatedStorageFile _storage;

        static string[] IMAGE_EXTS = { "jpg", "jpeg", "gif", "png", "bmp" };

        private ZumpaDB _database;

        public ImageLoader()
        {
            if (System.ComponentModel.DesignerProperties.IsInDesignTool) return;

            _storage = IsolatedStorageFile.GetUserStoreForApplication();
            _database = new ZumpaDB();
            _cache = LoadMemoryCache();
        }

        /// <summary>
        /// Load link async
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<Stream> LoadAsync(string link)
        {
            link = link.ToLower();

            ImageRecord record = null;
            Stream s = null;
            if (!_cache.TryGetValue(link, out record))
            {
                s = await SaveLinkAsync(link);
            }
            else
            {
                var t = new Task<Stream>(() => LoadFile(record.File));
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
        private Stream LoadFile(string file)
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
        private async Task<Stream> SaveLinkAsync(string link)
        {
            string file = GenerateRandomFileName(ExtractExt(link));
            Stream imageStream = null;
            long len = 0;
            using (IsolatedStorageFileStream fileStream = _storage.CreateFile(file))
            {
                imageStream = await LoadLinkAsync(link);
                if (imageStream == null)
                {
                    return null;
                }
                await imageStream.CopyToAsync(fileStream);
                len = imageStream.Position;
                imageStream.Position = 0;
            }
            _cache[link] = SaveLinkToDb(link, file, len);
            return imageStream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private bool IsLoaded(string link)
        {
            return _cache.ContainsKey(link);
        }

        /// <summary>
        /// Load image from web
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private async Task<Stream> LoadLinkAsync(string link)
        {
            try
            {
                var wc = new WebClient();
                Stream s = await wc.OpenReadTaskAsync(link);
                return s;
            }
            catch (Exception e)
            {
                return null;//we can't download it now
            }
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
        public bool IsImageLink(string link)
        {
            if (!String.IsNullOrEmpty(link))
            {
                link = link.ToLower();
                if (!IsInvalidImageLink(link))
                {
                    return IsImageLinkByExtension(link);
                }
            }
            return false;
        }

        public static bool IsImageLinkByExtension(string link)
        {
            foreach (var item in IMAGE_EXTS)
            {
                if (link.EndsWith(item))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsInvalidImageLink(string link)
        {
            ImageRecord ir = null;
            if (_cache.TryGetValue(link, out ir))
            {
                return !ir.IsValid;
            }
            return false;
        }

        /// <summary>
        /// Call this if link doesn't target image
        /// </summary>
        /// <param name="link"></param>
        public void NotifyInvalidLink(string link)
        {
            ImageRecord item = null;
            if (_cache.TryGetValue(link, out item))
            {
                if (item.IsValid)
                {
                    item.IsValid = false;
                    ZumpaDB.Instance.SubmitChanges();
                }
            }
            else
            {
                //it was completely problem with loading
                ImageRecord ir = new ImageRecord{Link = link, File = "", Size = 0, IsValid = false};
                _cache[link] = ir;
                ZumpaDB.Instance.Images.InsertOnSubmit(ir);
                ZumpaDB.Instance.SubmitChanges();
            }
        }

        private Dictionary<string, ImageRecord> LoadMemoryCache()
        {
            return ZumpaDB.Instance.Images.ToDictionary((e) => e.Link);
        }

        /// <summary>
        /// Save record to db
        /// </summary>
        /// <param name="link"></param>
        /// <param name="file"></param>
        /// <param name="size"></param>
        private ImageRecord SaveLinkToDb(string link, string file, long size)
        {
            ImageRecord rec = new ImageRecord { Link = link, File = file, Size = size, IsValid = true };
            ZumpaDB.Instance.Images.InsertOnSubmit(rec);
            ZumpaDB.Instance.SubmitChanges();
            return rec;
        }

        /// <summary>
        /// Get file from db
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private string GetFile(string link)
        {
            ImageRecord item = (from q in ZumpaDB.Instance.Images
                                where q.IsValid && q.Link.Equals(link)
                                select q).FirstOrDefault();
            return item != null ? item.File : null;
        }

        public static StorageValues GetStorageValues()
        {
            StorageValues values = new StorageValues();

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                values.FreeSpace = storage.AvailableFreeSpace;
            }
            using (var db = new ZumpaDB())
            {
                if (db.Images.Count() > 0)
                {
                    values.Downloaded = db.Images.Sum(e => e.Size);
                }
            }
            return values;
        }
    }

    public struct StorageValues
    {
        public long FreeSpace;
        public long Downloaded;
    }
}
