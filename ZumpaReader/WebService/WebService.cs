using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZumpaReader.Model;

namespace ZumpaReader.WebService
{
    public abstract class WebService : IWebService
    {
        public class WebServiceConfig
        {
            public string Cookies { get; set; }

            public string BaseURL { get; set; }

            public string NickName { get; set; }

            public string FakeNickName { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual WebServiceConfig Config { get; private set; }

        public WebService() : this(new WebServiceConfig()) { }

        public WebService(WebServiceConfig config)
        {
            Config = config;
        }

        /// <summary>
        /// Parse any generic response from server
        /// </summary>
        /// <typeparam name="T">Type for expected result</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ContextResult<T> Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<ContextResult<T>>(json);
        }

        /// <summary>
        /// Generic class for any kind of response from WebService
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public class ContextResult<T>
        {
            [JsonProperty("Context")]
            public T Context { get; set; }

            [JsonProperty("HasError")]
            public bool HasError { get; set; }
        }

        /// <summary>
        /// Download main page
        /// </summary>
        /// <param name="url">Specific url for older pages, pass null for latest one</param>
        public abstract Task<WebService.ContextResult<ZumpaItemsResult>> DownloadItems(string url = null);

        /// <summary>
        /// Download thread for particular link
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>                
        public abstract Task<ContextResult<List<ZumpaSubItem>>> DownloadThread(string url);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public abstract Task<WebService.ContextResult<string>> Login(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task<WebService.ContextResult<bool>> Logout();

        /// <summary>
        /// Send new thread or message into thread
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="threadId">Numeric value for thread, if it's null new thread is created</param>
        /// <returns></returns>
        public abstract Task<WebService.ContextResult<bool>> SendMessage(string subject, string message, string threadId = null);
    }
}
