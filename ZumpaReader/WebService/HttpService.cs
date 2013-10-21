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
    public class HttpService : WebService
    {
        private const string ITEMS = "/zumpa";
        private const string LOGIN = "/login";
        private const string LOGOUT = "/logout";
        private const string THREAD = "/thread";
        private const string POST = "/post";
        private const string SURVEY = "/survey";
        private const string FAVOURITE = "/favorite";
        private const string IMAGE = "/image";       
        private const string HTTP_POST = "POST";

        private const string PARAM_COOKIES = "Cookies";
        private const string PARAM_PAGE = "Page";
        private const string PARAM_USER_NAME = "UserName";
        private const string PARAM_USER_PASSWORD = "Password";
        private const string PARAM_THREAD_URL = "ItemsUrl";
        private const string PARAM_FAKE_NICK = "FakeUserName";
        private const string PARAM_MESSAGE = "Message";
        private const string PARAM_SUBJECT = "Subject";
        private const string PARAM_THREAD_ID = "ThreadID";
        private const string PARAM_SURVEY_ID = "SurveyID";
        private const string PARAM_SURVEY_ITEM = "SurveyItem";
        private const string PARAM_LAST_ANSWER_AUTHOR = "LastAnswerAuthor";
        private const string PARAM_FILTER_TYPE = "FilterType";
        private const string PARAM_SURVEY = "Survey";


        private const string TYPE_JSON = "application/json";
        private const string TYPE_IMAGE = "image/jpeg";

        public HttpService() : this(null) { }

        public HttpService(WebServiceConfig config) : base(config) { }

        #region Help methods

        /// <summary>
        /// Create post request for json data
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpWebRequest CreatePostRequest(string url, string type)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(url);
            req.Method = HTTP_POST;
            req.ContentType = type;
            return req;
        }

        /// <summary>
        /// Create json string of params
        /// </summary>
        /// <param name="values">array of params convertible into string</param>
        /// <returns>Serialized data into json object</returns>
        private string JsonParamsCreator(params object[] values)
        {
            Dictionary<string, object> pars = new Dictionary<string, object>();
            if (values != null)
            {
                if (values.Length % 2 != 0)
                {
                    throw new ArgumentException("Values must be even, exactly value:key pairs!");
                }
                for (int i = 0, n = values.Length; i < n; i++)
                {
                    object key = values[i];
                    object value = values[++i];
                    if (key != null && value != null)
                    {
                        pars[key.ToString()] = value;
                    }
                }
            }

            if (!String.IsNullOrEmpty(Config.Cookies)) { pars[PARAM_COOKIES] = Config.Cookies; }            
            if (!String.IsNullOrEmpty(Config.FakeNickName)) { pars[PARAM_FAKE_NICK] = Config.FakeNickName; }
            else if (!String.IsNullOrEmpty(Config.NickName)) { pars[PARAM_USER_NAME] = Config.NickName; }
            pars[PARAM_LAST_ANSWER_AUTHOR] = Convert.ToString(Config.LastAnswerAuthor);
            pars[PARAM_FILTER_TYPE] = Convert.ToString(Config.FilterType);
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
            return await PostData(url, System.Text.Encoding.UTF8.GetBytes(data));
        }

        private async Task<string> PostData(string url, byte[] data, string type = TYPE_JSON)
        {
            WebRequest req = CreatePostRequest(url, type);
            using (Stream s = await req.GetRequestStreamAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }

            WebResponse wr = await req.GetResponseAsync();
            string result = null;
            using (Stream s = wr.GetResponseStream())
            {
                StreamReader sr = new StreamReader(s);
                result = await sr.ReadToEndAsync();
            }
            return result;
        }

        private void EnsureLoggedIn()
        {
            if (String.IsNullOrEmpty(Config.Cookies))
            {
                throw new InvalidOperationException("Not logged in!");
            }
        }

        #endregion

        /// <summary>
        /// Download page of items, after loading OnDownloadedItems is called
        /// </summary>
        /// <param name="page">Option param for page url, if null main page</param>
        public async override Task<WebService.ContextResult<ZumpaItemsResult>> DownloadItems(string url = null)
        {
            string @params = JsonParamsCreator(PARAM_PAGE, url);
            string jsonResponse = await PostData(Config.BaseURL + ITEMS, @params);
            return Parse<ZumpaItemsResult>(jsonResponse);
        }

        /// <summary>
        /// Login into zumpa
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Cookie string value</returns>
        public async override Task<WebService.ContextResult<LoginResult>> Login(string username, string password)
        {
            string @params = JsonParamsCreator(PARAM_USER_NAME, username, PARAM_USER_PASSWORD, password);
            string jsonResponse = await PostData(Config.BaseURL + LOGIN, @params);
            return Parse<LoginResult>(jsonResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if logout was successful</returns>
        public async override Task<WebService.ContextResult<bool>> Logout()
        {
            string @params = JsonParamsCreator();
            string jsonResponse = await PostData(Config.BaseURL + LOGOUT, @params);
            return Parse<bool>(jsonResponse);
        }

        public async override Task<ContextResult<List<ZumpaSubItem>>> DownloadThread(string url)
        {
            string @params = JsonParamsCreator(PARAM_THREAD_URL, url);            
            string jsonResponse = await PostData(Config.BaseURL + THREAD, @params);
            return Parse<List<ZumpaSubItem>>(jsonResponse);
        }

        public async override Task<ContextResult<bool>> SendMessage(string subject, string message, Survey survey, string threadId = null)
        {
            EnsureLoggedIn();

            if (String.IsNullOrEmpty(Config.NickName) && String.IsNullOrEmpty(Config.FakeNickName))
            {
                throw new InvalidOperationException("User not even NickName set!");
            }
            string @params = JsonParamsCreator(PARAM_SUBJECT, subject,
                                               PARAM_MESSAGE, message,
                                               PARAM_SURVEY, survey,
                                               PARAM_THREAD_ID, threadId);
            string jsonResponse = await PostData(Config.BaseURL + POST, @params);
            return Parse<bool>("{}");
        }

        public async override Task<ContextResult<Survey>> VoteSurvey(int id, int vote)
        {
            EnsureLoggedIn();

            string @params = JsonParamsCreator(PARAM_SURVEY_ID, id, PARAM_SURVEY_ITEM, vote);
            string jsonResponse = await PostData(Config.BaseURL + SURVEY, @params);
            return Parse<Survey>(jsonResponse);
        }

        public async override Task<ContextResult<string>> UploadImage(byte[] data)
        {
            string jsonResponse = await PostData(Config.BaseURL + IMAGE, data, TYPE_IMAGE);
            return Parse<string>(jsonResponse);
        }

        public async override Task<WebService.ContextResult<bool>> SwitchThreadFavourite(int threadId)
        {
            EnsureLoggedIn();

            string @params = JsonParamsCreator(PARAM_THREAD_ID, threadId);
            string jsonResponse = await PostData(Config.BaseURL + FAVOURITE, @params);
            return Parse<bool>(jsonResponse);
        }

        public override async Task<bool> RegisterPushURI(string username, string uid, string pushUrl)
        {
            string mainUrl = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.ZumpaPushRegisterURL];
            string queryString = String.Format("?user={0}&uid={1}&regid={2}&platform=windowsphone&register",
                                               HttpUtility.UrlEncode(username),
                                               HttpUtility.UrlEncode(uid),
                                               HttpUtility.UrlEncode(pushUrl));
            string url = mainUrl + queryString;
            string result = await new WebClient().DownloadStringTaskAsync(url);
            return "[OK]".Equals(result);
        }

        /// <summary>
        /// Crete instance of webservice with current user's settings
        /// </summary>
        /// <returns></returns>
        public static HttpService CreateInstance()
        {
            var c = new WebService.WebServiceConfig();
            c.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            if (AppSettings.IsLoggedIn)
            {
                c.Cookies = AppSettings.CookieString;
                c.FakeNickName = AppSettings.NickOrResponseName;
                c.NickName = AppSettings.Login;
                c.FilterType = AppSettings.Filter;
            }
            c.LastAnswerAuthor = AppSettings.LastAuthor;
            return new HttpService(c);
        }
        
    }
}
