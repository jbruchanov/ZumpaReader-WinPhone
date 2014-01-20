using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace ZumpaReader
{
    public class AppSettings
    {
        internal enum PropertyKeys
        {
            CookieString, Login, Password, IsLoggedIn, ResponseName, LastAuthor, PushURI, ZumpaUID, Filter, AutoLoadImages, ShowImageAsButton, ShowSettingsAutoLoadImages
        }

        static AppSettings()
        {
            if (!System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                _storage = IsolatedStorageSettings.ApplicationSettings;
            }
            else
            {
                _storage = null;
            }
        }

        private static readonly IsolatedStorageSettings _storage;

        public static string CookieString
        {
            get
            {
                if (_storage == null) return "";
                string result = null;
                _storage.TryGetValue<string>(PropertyKeys.CookieString.ToString(), out result);
                return result;
            }

            set
            {
                _storage[PropertyKeys.CookieString.ToString()] = value;
                _storage.Save();
            }
        }

        public static string Login
        {
            get
            {
                if(_storage == null) return "";
                string result = null;
                _storage.TryGetValue<string>(PropertyKeys.Login.ToString(), out result);
                return result;
            }

            set
            {
                _storage[PropertyKeys.Login.ToString()] = value;
                _storage.Save();
            }
        }

        public static string ResponseName
        {
            get
            {
                if (_storage == null) return "";
                string result = null;
                _storage.TryGetValue<string>(PropertyKeys.ResponseName.ToString(), out result);
                return result;
            }

            set
            {
                _storage[PropertyKeys.ResponseName.ToString()] = value;
                _storage.Save();
            }
        }

        public static string NickOrResponseName
        {
            get{
                if (_storage == null) return "";
                string s = ResponseName;
                if (string.IsNullOrEmpty(s))
                    s = Login;
                return s;
            }
        }

        public static string Password
        {
            get
            {
                if (_storage == null) return "";
                string result = null;
                _storage.TryGetValue<string>(PropertyKeys.Password.ToString(), out result);
                return result;
            }

            set
            {
                _storage[PropertyKeys.Password.ToString()] = value;
                _storage.Save();
            }
        }

        public static bool IsLoggedIn
        {
            get
            {
                if (_storage == null) return false;
                bool result = false;
                if (!_storage.TryGetValue<bool>(PropertyKeys.IsLoggedIn.ToString(), out result))
                    result = false;
                return result;
            }

            set
            {
                _storage[PropertyKeys.IsLoggedIn.ToString()] = value;
                _storage.Save();
            }
        }

        public static bool LastAuthor
        {
            get
            {
                if (_storage == null) return false;
                bool result = false;
                if (!_storage.TryGetValue<bool>(PropertyKeys.LastAuthor.ToString(), out result))
                    result = false;
                return result;
            }

            set
            {
                _storage[PropertyKeys.LastAuthor.ToString()] = value;
                _storage.Save();
            }
        }

        public static string PushURI
        {
            get
            {
                if (_storage == null) return "";
                string result;
                if (!_storage.TryGetValue<string>(PropertyKeys.PushURI.ToString(), out result))
                    result = null;
                return result;
            }

            set
            {
                _storage[PropertyKeys.PushURI.ToString()] = value;
                _storage.Save();
            }
        }

        public static string ZumpaUID
        {
            get
            {
                if (_storage == null) return "";
                string result;
                if (!_storage.TryGetValue<string>(PropertyKeys.ZumpaUID.ToString(), out result))
                    result = null;
                return result;
            }

            set
            {
                _storage[PropertyKeys.ZumpaUID.ToString()] = value;
                _storage.Save();
            }
        }

        public static int Filter
        {
            get
            {
                if (_storage == null) return 0;
                int result = 0;
                _storage.TryGetValue<int>(PropertyKeys.Filter.ToString(), out result);
                return result;
            }

            set
            {
                _storage[PropertyKeys.Filter.ToString()] = value;
                _storage.Save();
            }
        }

        public static bool AutoLoadImages
        {
            get
            {
                if (_storage == null) return false;
                bool result;
                if (!_storage.TryGetValue<bool>(PropertyKeys.AutoLoadImages.ToString(), out result))
                {
                    result = true;
                }
                return result;
            }

            set
            {
                _storage[PropertyKeys.AutoLoadImages.ToString()] = value;
                _storage.Save();
            }
        }

        public static bool ShowImageAsButton
        {
            get
            {
                if (_storage == null) return false;
                bool result;
                if (!_storage.TryGetValue<bool>(PropertyKeys.ShowImageAsButton.ToString(), out result))
                {
                    result = false;
                }
                return result;
            }

            set
            {
                _storage[PropertyKeys.ShowImageAsButton.ToString()] = value;
                _storage.Save();
            }
        }

        public static bool ShowSettingsAutoLoadImages
        {
            get
            {
                if (_storage == null) return false;
                bool result;
                if (!_storage.TryGetValue<bool>(PropertyKeys.ShowSettingsAutoLoadImages.ToString(), out result))
                {
                    result = false;
                }
                return result;
            }

            set
            {
                _storage[PropertyKeys.ShowSettingsAutoLoadImages.ToString()] = value;
                _storage.Save();
            }
        }
    }
}
