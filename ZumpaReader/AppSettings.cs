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
        private enum PropertyKeys
        {
            CookieString, Login, Password, IsLoggedIn, ResponseName, LastAuthor, PushURI
        }

        private static readonly IsolatedStorageSettings _storage = IsolatedStorageSettings.ApplicationSettings;

        public static string CookieString
        {
            get
            {
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
    }
}
