using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ZumpaReader.Model;

namespace ZumpaReader.WebService
{
    public class WebServiceClient : WebService
    {
        private string _baseUrl;

        private const string ITEMS = "/zumpa";
        private const string LOGIN = "/login";
        private const string LOGOUT = "/logout";
        private const string THREAD = "/thread";        
        private const string POST = "POST";

        private const string PARAM_COOKIES = "Cookies";
        private const string PARAM_PAGE = "Page";
        private const string PARAM_USER_NAME = "UserName";
        private const string PARAM_USER_PASSWORD = "Password";
        private const string PARAM_THREAD_URL = "ItemsUrl";        

        private const string TYPE_JSON = "application/json";

        private string _cookies;

        public WebServiceClient(string cookies = null)
        {
            _baseUrl = ZumpaReaderResources.Instance[ZumpaReader.ZumpaReaderResources.Keys.WebServiceURL];
            _cookies = cookies;
        }
        
        #region Help methods

        /// <summary>
        /// Create post request for json data
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpWebRequest CreatePostRequest(string url)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(url);
            req.Method = POST;
            req.ContentType = TYPE_JSON;     
            return req;
        }

        /// <summary>
        /// Create json string of params
        /// </summary>
        /// <param name="values">array of params convertible into string</param>
        /// <returns>Serialized data into json object</returns>
        private string JsonParamsCreator(params object[] values)
        {
            if (values.Length % 2 != 0)
            { 
                throw new ArgumentException("Values must be even, exactly value:key pairs!");
            }

            Dictionary<string, string> pars = new Dictionary<string, string>();
            for (int i = 0, n = values.Length; i < n; i++)
            {
                object key = values[i];
                object value = values[++i];
                if (key != null && value != null)
                { 
                    pars[key.ToString()] = value.ToString();
                }
            }
            if (_cookies != null)
            {
                pars[PARAM_COOKIES] = _cookies;
            }
            return JsonConvert.SerializeObject(pars);
        }

        /// <summary>
        /// Post json data to url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<string> PostData(string url, string data)
        {
            WebRequest req = CreatePostRequest(url);
            using (Stream s = await req.GetRequestStreamAsync())
            {
                byte[] raw = System.Text.Encoding.UTF8.GetBytes(data);
                await s.WriteAsync(raw, 0, data.Length);                
            }

            WebResponse wr = await req.GetResponseAsync();
            string result = null;
            using(Stream s = wr.GetResponseStream())
            {
                StreamReader sr = new StreamReader(s);                
                result = await sr.ReadToEndAsync();
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Download page of items, after loading OnDownloadedItems is called
        /// </summary>
        /// <param name="page">Option param for page url, if null main page</param>
        public async override Task<WebService.ContextResult<ZumpaItemsResult>> DownloadItems(string url = null)
        {
            string @params = JsonParamsCreator(PARAM_PAGE, url);
            string jsonResponse = await PostData(_baseUrl + ITEMS, @params);
            return Parse<ZumpaItemsResult>(jsonResponse);
        }

        /// <summary>
        /// Login into zumpa
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Cookie string value</returns>
        public async override Task<WebService.ContextResult<string>> Login(string username, string password)
        {
            string @params = JsonParamsCreator(PARAM_USER_NAME, username, PARAM_USER_PASSWORD, password);
            string jsonResponse = await PostData(_baseUrl + LOGIN, @params);
            return Parse<string>(jsonResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if logout was successful</returns>
        public async override Task<WebService.ContextResult<bool>> Logout()
        {
            string @params = JsonParamsCreator();
            string jsonResponse = await PostData(_baseUrl + LOGOUT, @params);
            return Parse<bool>(jsonResponse);
        }
        
        public async override Task<ContextResult<List<ZumpaSubItem>>> DownloadThread(string url)
        {
            string @params = JsonParamsCreator(PARAM_THREAD_URL, url);
            string jsonResponse = await PostData(_baseUrl + THREAD, @params);
            return Parse<List<ZumpaSubItem>>(jsonResponse);
        }
    }
}
